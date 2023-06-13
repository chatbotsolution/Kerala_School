using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_LoanMod : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        if (!ChkIsHRUsed())
        {
            Response.Redirect("HRHome.aspx?IsHRUsed=0");
        }
        else
        {
            ChkEmpMlySalary();
            FillDrp(drpEmp, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster Where ActiveStatus=1 ORDER BY EmpName", "EmpName", "EmpId");
            TempTable();
        }
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void ChkEmpMlySalary()
    {
        if (!(obj.ExecuteScalarQry("SELECT COUNT(*) FROM dbo.HR_EmpMlySalary WHERE Month='APR' AND Year=" + obj.ExecuteScalarQry("SELECT YEAR(StartDate) FROM dbo.ACTS_FinancialYear WHERE IsFinalized IS NULL")).Trim() != "0"))
            return;
        rbModify.Checked = true;
    }

    protected void rbModify_CheckedChanged(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        ClearFields();
        rbModify.Checked = true;
        rbModify.Focus();
    }

    protected void rbPostpone_CheckedChanged(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        ClearFields();
        rbPostpone.Checked = true;
        rbPostpone.Focus();
    }

    private void FillDrp(DropDownList drp, string Qry, string Text, string Value)
    {
        DataTable dataTableQry = obj.GetDataTableQry(Qry.Trim());
        drp.DataSource = dataTableQry.DefaultView;
        drp.DataTextField = Text;
        drp.DataValueField = Value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    private void BindLoan()
    {
        drpLoan.Items.Clear();
        DataTable dataTable = obj.GetDataTable("HR_GetLoanListLoanInit", new Hashtable()
    {
      {
         "@EmpId",
         drpEmp.SelectedValue
      }
    });
        drpLoan.DataSource = dataTable.DefaultView;
        drpLoan.DataTextField = "AcctsHead";
        drpLoan.DataValueField = "AcctsHeadId";
        drpLoan.DataBind();
        if (dataTable.Rows.Count > 0)
            drpLoan.Items.Insert(0, new ListItem("- SELECT -", "0"));
        else
            drpLoan.Items.Insert(0, new ListItem("- No Loan Available -", "0"));
    }

    protected void drpEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        drpLoan.Items.Clear();
        grdLoanInit.DataSource = null;
        grdLoanInit.DataBind();
        trLoan.Visible = false;
        row2.Visible = false;
        row3.Visible = false;
        if (drpEmp.SelectedIndex > 0)
            BindLoan();
        drpEmp.Focus();
    }

    protected void drpLoan_SelectedIndexChanged(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        grdLoanInit.DataSource = null;
        grdLoanInit.DataBind();
        trLoan.Visible = false;
        row2.Visible = false;
        row3.Visible = false;
        if (drpLoan.SelectedIndex > 0)
        {
            trLoan.Visible = true;
            FillGrid();
        }
        drpLoan.Focus();
    }

    private void TempTable()
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn("CalMonth", typeof(string)));
        dataTable.Columns.Add(new DataColumn("CalYear", typeof(string)));
        dataTable.Columns.Add(new DataColumn("LoanRecId", typeof(string)));
        dataTable.Columns.Add(new DataColumn("RecAmt", typeof(string)));
        dataTable.Columns.Add(new DataColumn("RecType", typeof(string)));
        dataTable.Columns.Add(new DataColumn("GenLedgerId", typeof(string)));
        dataTable.AcceptChanges();
        ViewState["TempTable"] = dataTable;
    }

    protected void drpMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    private void FillGrid()
    {
        DataTable dataTable = new DataTable();
        if (rbModify.Checked)
        {
            DataTable dataTableQry = obj.GetDataTableQry("SELECT GenLedgerId,LoanRecId,CalMonth,CalYear,CONVERT(DECIMAL(15,2),RecAmt) AS RecAmt,RecType FROM dbo.HR_EmpLoanRecovery WHERE EmpId=" + drpEmp.SelectedValue + " AND RcvdStatus=0 AND LoanAcHeadId=" + drpLoan.SelectedValue + " ORDER BY LoanRecId ASC");
            grdLoanInit.DataSource = dataTableQry;
            grdLoanInit.DataBind();
            txtLoanAmt.Text = obj.ExecuteScalarQry("SELECT CONVERT(DECIMAL(15,2),TransAmt) FROM dbo.Acts_GenLedger WHERE GenLedgerId=" + dataTableQry.Rows[0]["GenLedgerId"].ToString());
            Decimal num1 = new Decimal(0);
            Decimal num2 = new Decimal(0);
            foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
            {
                if (row["RecType"].ToString().Trim() == "P")
                    num1 += Convert.ToDecimal(row["RecAmt"].ToString().Trim());
                else
                    num2 += Convert.ToDecimal(row["RecAmt"].ToString().Trim());
            }
            txtInt.Text = num2.ToString("0.00");
            txtPending.Text = num1.ToString().Trim();
            row2.Visible = true;
            row3.Visible = false;
            grdLoanInit.Columns[2].Visible = true;
        }
        else if (rbPostpone.Checked)
        {
            BindLoanDtls();
            grdLoanInit.Columns[2].Visible = false;
        }
        lblRecords.Text = "No of Records : " + grdLoanInit.Rows.Count;
    }

    private void BindLoanDtls()
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = obj.GetDataTableQry("SELECT GenLedgerId,LoanRecId,CalMonth,CalYear,CONVERT(DECIMAL(15,2),RecAmt) AS RecAmt,RecType FROM dbo.HR_EmpLoanRecovery WHERE EmpId=" + drpEmp.SelectedValue + " AND RcvdStatus=0 AND LoanAcHeadId=" + drpLoan.SelectedValue + " AND RTRIM(Remarks)='Loan Principal Recovery' ORDER BY LoanRecId ASC");
        grdLoanInit.DataSource = dataTableQry;
        grdLoanInit.DataBind();
        row2.Visible = false;
        row3.Visible = true;
        txtMonth.Text = dataTableQry.Rows[0]["CalMonth"].ToString();
        txtYear.Text = dataTableQry.Rows[0]["CalYear"].ToString();
        txtRecAmt.Text = dataTableQry.Rows[0]["RecAmt"].ToString();
        hfLoanRecId.Value = dataTableQry.Rows[0]["LoanRecId"].ToString();
        hfGenLedgerId.Value = dataTableQry.Rows[0]["GenLedgerId"].ToString();
        foreach (Control row in grdLoanInit.Rows)
            (row.FindControl("txtAmount") as TextBox).Enabled = false;
        btnPostpone.Focus();
    }

    private void ClearFields()
    {
        trMsg.Style["background-color"] = "Transparent";
        lblMsg.Text = string.Empty;
        FillDrp(drpEmp, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster Where ActiveStatus=1 ORDER BY EmpName", "EmpName", "EmpId");
        trLoan.Visible = false;
        grdLoanInit.DataSource = null;
        grdLoanInit.DataBind();
        ViewState["GenLedgerId"] = null;
        txtInt.Text = "0";
        txtLoanAmt.Text = "0";
        drpLoan.Items.Clear();
        txtMonth.Text = string.Empty;
        txtYear.Text = string.Empty;
        txtRecAmt.Text = "0";
        hfLoanRecId.Value = "0";
        hfGenLedgerId.Value = "0";
    }

    protected void btnModify_Click(object sender, EventArgs e)
    {
        string empty1 = string.Empty;
        string str1 = valGrid();
        if (str1.Trim() == string.Empty)
        {
            string empty2 = string.Empty;
            string empty3 = string.Empty;
            trMsg.Style["background-color"] = "Transparent";
            lblMsg.Text = string.Empty;
            ViewState["GenLedgerId"] = null;
            Convert.ToDecimal(txtPending.Text);
            Convert.ToDecimal(txtInt.Text);
            foreach (GridViewRow row in grdLoanInit.Rows)
            {
                Label control1 = (Label)row.FindControl("lblMonth");
                Label control2 = (Label)row.FindControl("lblYear");
                HiddenField control3 = (HiddenField)row.FindControl("hfGenLedgerId");
                HiddenField control4 = (HiddenField)row.FindControl("hfLoanRecId");
                HiddenField control5 = (HiddenField)row.FindControl("hfRecType");
                TextBox control6 = (TextBox)row.FindControl("txtAmount");
                string upper = control1.Text.Trim().ToUpper();
                string str2 = control2.Text.Trim();
                ViewState["GenLedgerId"] = control3.Value;
                Hashtable hashtable = new Hashtable();
                hashtable.Clear();
                hashtable.Add("@LoanAcHeadId", drpLoan.SelectedValue);
                hashtable.Add("@LoanRecId", control4.Value);
                hashtable.Add("@EmpId", drpEmp.SelectedValue);
                hashtable.Add("@RecType", control5.Value);
                hashtable.Add("@CalMonth", upper);
                hashtable.Add("@CalYear", str2);
                hashtable.Add("@GenLedgerId", control3.Value);
                hashtable.Add("@RecAmt", Convert.ToDecimal(control6.Text));
                hashtable.Add("@UserId", Session["User_Id"]);
                str1 = obj.ExecuteScalar("HR_UpdateLoan", hashtable);
            }
            if (str1.Trim() == string.Empty)
            {
                ClearFields();
                trMsg.Style["background-color"] = "Green";
                lblMsg.Text = "Data Saved Successfully";
            }
            else
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = str1;
            }
        }
        else
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = str1;
        }
    }

    private string valGrid()
    {
        string str = string.Empty;
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        TextBox textBox = new TextBox();
        foreach (GridViewRow row in grdLoanInit.Rows)
        {
            TextBox control1 = (TextBox)row.FindControl("txtAmount");
            HiddenField control2 = (HiddenField)row.FindControl("hfRecType");
            if (Convert.ToDecimal(control1.Text) > new Decimal(0))
            {
                if (control2.Value.Trim() == "P")
                    num2 += Convert.ToDecimal(control1.Text.Trim());
                else
                    num1 += Convert.ToDecimal(control1.Text.Trim());
            }
            else
            {
                str = "Month wise recovery amount shouldn't be Zero";
                break;
            }
        }
        if (str.Trim() == string.Empty)
        {
            if (num2 != Convert.ToDecimal(txtPending.Text.Trim()))
            {
                str = "Sum Of the Month wise Principal Recovery Amount(instalment) is not equal to the Total Principal amount";
                txtPending.Focus();
            }
            else if (num1 != Convert.ToDecimal(txtInt.Text.Trim()))
            {
                str = "Sum Of the Month wise Intrest Recovery Amount(instalment) is not equal to the Total Interest Amount";
                txtInt.Focus();
            }
        }
        return str;
    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        string str = string.Empty;
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        TextBox textBox = new TextBox();
        foreach (GridViewRow row in grdLoanInit.Rows)
        {
            TextBox control1 = (TextBox)row.FindControl("txtAmount");
            HiddenField control2 = (HiddenField)row.FindControl("hfRecType");
            if (Convert.ToDecimal(control1.Text) > new Decimal(0))
            {
                if (control2.Value.Trim() == "P")
                    num2 += Convert.ToDecimal(control1.Text.Trim());
                else
                    num1 += Convert.ToDecimal(control1.Text.Trim());
            }
            else
            {
                str = "Month wise recovery amount shouldn't be Zero";
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Month wise recovery amount shouldn't be Zero.";
                break;
            }
        }
        if (str.Trim() == string.Empty)
        {
            if (num2 != Convert.ToDecimal(txtPending.Text.Trim()))
                txtPending.Focus();
            else
                txtInt.Text = num1.ToString("0.00");
        }
        txtInt.Focus();
    }

    protected void btnPostpone_Click(object sender, EventArgs e)
    {
        string empty = string.Empty;
        if (obj.ExecuteScalar("HR_LoanRecPostpone", new Hashtable()
    {
      {
         "@LoanRecId",
         hfLoanRecId.Value
      },
      {
         "@UserId",
        Session["User_Id"]
      }
    }).Trim().ToUpper() == "S")
        {
            trMsg.Style["background-color"] = "Green";
            lblMsg.Text = "Loan Recovery Postponed Successfully";
            BindLoanDtls();
        }
        else
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Failed to Save. Try Again.";
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("HRInit.aspx");
    }
}