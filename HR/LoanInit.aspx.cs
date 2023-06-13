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


public partial class HR_LoanInit : System.Web.UI.Page
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
        rbInit.Checked = false;
        rbInit.Visible = false;
    }

    protected void rbInit_CheckedChanged(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        ClearFields();
        rbInit.Checked = true;
        rbInit.Focus();
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
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@EmpId", drpEmp.SelectedValue);
        if (rbInit.Checked)
            hashtable.Add("@IsInit", true);
        DataTable dataTable = obj.GetDataTable("HR_GetLoanListLoanInit", hashtable);
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
        row1.Visible = false;
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
        row1.Visible = false;
        if (drpLoan.SelectedIndex > 0)
        {
            trLoan.Visible = true;
            FillGrid();
        }
        txtTotalAmt.Text = "0";
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
        drpMonth.Focus();
    }

    private void FillGrid()
    {
        DataTable dataTable1 = new DataTable();
        if (rbInit.Checked)
        {
            DataTable dataTable2 = (DataTable)ViewState["TempTable"];
            dataTable2.Clear();
            string str1 = drpMonth.SelectedValue;
            string str2 = obj.ExecuteScalarQry("SELECT YEAR(StartDate) FROM dbo.ACTS_FinancialYear WHERE IsFinalized IS NULL");
            if (str2.Trim() != string.Empty)
            {
                for (int index = 1; index <= 36; ++index)
                {
                    DataRow row = dataTable2.NewRow();
                    row["CalMonth"] = str1.ToUpper();
                    row["CalYear"] = str2;
                    row["RecAmt"] = "0";
                    dataTable2.Rows.Add(row);
                    dataTable2.AcceptChanges();
                    if (str1 == "JAN")
                        str1 = "FEB";
                    else if (str1 == "FEB")
                        str1 = "MAR";
                    else if (str1 == "MAR")
                        str1 = "APR";
                    else if (str1 == "APR")
                        str1 = "MAY";
                    else if (str1 == "MAY")
                        str1 = "JUN";
                    else if (str1 == "JUN")
                        str1 = "JUL";
                    else if (str1 == "JUL")
                        str1 = "AUG";
                    else if (str1 == "AUG")
                        str1 = "SEP";
                    else if (str1 == "SEP")
                        str1 = "OCT";
                    else if (str1 == "OCT")
                        str1 = "NOV";
                    else if (str1 == "NOV")
                    {
                        str1 = "DEC";
                    }
                    else
                    {
                        str1 = "JAN";
                        str2 = (Convert.ToInt32(str2) + 1).ToString();
                    }
                }
                grdLoanInit.DataSource = dataTable2;
                grdLoanInit.DataBind();
                row1.Visible = true;
            }
            else
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('First Initialize the Current Session Year.');window.location='HRHome.aspx'", true);
        }
        lblRecords.Text = "No of Records : " + grdLoanInit.Rows.Count;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string empty = string.Empty;
        string str1 = valGrid();
        if (str1.Trim() == string.Empty)
        {
            ViewState["GenLedgerId"] = null;
            Hashtable hashtable = new Hashtable();
            DataTable dataTable1 = new DataTable();
            string str2 = obj.ExecuteScalarQry("SELECT FY FROM dbo.ACTS_FinancialYear WHERE IsFinalized IS NULL");
            string[] strArray = str2.Split('-');
            string str3 = strArray[0];
            string str4 = strArray[1];
            int num1 = 0;
            Decimal num2 = new Decimal(0);
            Label label1 = new Label();
            Label label2 = new Label();
            foreach (GridViewRow row in grdLoanInit.Rows)
            {
                TextBox control1 = (TextBox)row.FindControl("txtAmount");
                if (Convert.ToDecimal(control1.Text) > new Decimal(0))
                {
                    label1 = (Label)row.FindControl("lblYear");
                    label2 = (Label)row.FindControl("lblMonth");
                    TextBox control2 = (TextBox)grdLoanInit.Rows[num1 + 1].FindControl("txtAmount");
                    num2 += Convert.ToDecimal(control1.Text);
                    hashtable.Clear();
                    if (ViewState["GenLedgerId"] == null)
                        hashtable.Add("@PendingAmt", Convert.ToDecimal(txtTotalAmt.Text));
                    if (ViewState["GenLedgerId"] != null)
                        hashtable.Add("@GenLedgerId", ViewState["GenLedgerId"]);
                    hashtable.Add("@LoanAcHeadId", drpLoan.SelectedValue);
                    hashtable.Add("@FY", str2);
                    hashtable.Add("@EmpId", drpEmp.SelectedValue);
                    hashtable.Add("@CalYear", Convert.ToInt32(label1.Text));
                    hashtable.Add("@CalMonth", label2.Text.Trim().ToUpper());
                    hashtable.Add("@TransDate", Convert.ToDateTime("APR-01-" + str3));
                    hashtable.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
                    hashtable.Add("@SchoolId", Session["SchoolId"]);
                    hashtable.Add("@RecType", "P");
                    hashtable.Add("@RecAmt", Convert.ToDecimal(control1.Text.Trim()));
                    DataTable dataTable2 = obj.GetDataTable("HR_InitLoan", hashtable);
                    if (dataTable2.Rows[0][0].ToString().ToUpper() == "S")
                    {
                        ViewState["GenLedgerId"] = dataTable2.Rows[0][1].ToString();
                        ++num1;
                    }
                    else
                    {
                        str1 = dataTable2.Rows[0][1].ToString();
                        break;
                    }
                }
                else
                    break;
            }
            if (str1.Trim() == string.Empty && Convert.ToDecimal(txtInterst.Text.Trim()) > new Decimal(0))
            {
                hashtable.Clear();
                hashtable.Add("@LoanAcHeadId", drpLoan.SelectedValue);
                hashtable.Add("@GenLedgerId", ViewState["GenLedgerId"]);
                hashtable.Add("@FY", str2);
                hashtable.Add("@EmpId", drpEmp.SelectedValue);
                hashtable.Add("@CalYear", Convert.ToInt32(label1.Text));
                hashtable.Add("@CalMonth", label2.Text.Trim().ToUpper());
                hashtable.Add("@TransDate", Convert.ToDateTime(drpMonth.SelectedValue + "-01-" + str3));
                hashtable.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
                hashtable.Add("@RecType", "I");
                hashtable.Add("@RecAmt", Convert.ToDecimal(txtInterst.Text.Trim()));
                DataTable dataTable2 = obj.GetDataTable("HR_InitLoan", hashtable);
                if (dataTable2.Rows[0][0].ToString().ToUpper() == "S")
                {
                    ViewState["GenLedgerId"] = dataTable2.Rows[0][1].ToString();
                    int num3 = num1 + 1;
                }
                else
                    str1 = dataTable2.Rows[0][1].ToString();
            }
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

    private string valGrid()
    {
        string str = string.Empty;
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        Decimal num3 = new Decimal(0);
        Decimal num4 = Convert.ToDecimal(txtTotalAmt.Text.Trim());
        TextBox textBox = new TextBox();
        int num5 = 0;
        foreach (GridViewRow row in grdLoanInit.Rows)
        {
            Decimal num6 = num3;
            TextBox control = (TextBox)row.FindControl("txtAmount");
            num3 = Convert.ToDecimal(control.Text.Trim());
            if (num5 > 0 && num6 == new Decimal(0) && num3 > new Decimal(0))
            {
                str = "Please Enter Recovery Amount (Instalment) for Consecutive Months!! Do not leave month gap between instalments!";
                textBox.Focus();
                break;
            }
            textBox = (TextBox)row.FindControl("txtAmount");
            ++num5;
            num1 += Convert.ToDecimal(control.Text.Trim());
        }
        if (str.Trim() == string.Empty && num1 != num4)
        {
            str = "Total Month wise Recovery Amount(instalment) is not equal to the Principal and Interest Amount";
            txtTotalAmt.Focus();
        }
        return str;
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
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("HRInit.aspx");
    }
}