using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_LoanAdjustment : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (!Page.IsPostBack)
        {
            if (!ChkIsHRUsed())
            {
                Response.Redirect("HRHome.aspx?IsHRUsed=0");
            }
            else
            {
                FillDrp(drpEmp, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster Where ActiveStatus=1 ORDER BY EmpName", "EmpName", "EmpId");
                FillDrp(drpLoan, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=25", "AcctsHead", "AcctsHeadId");
            }
        }
        lblMsg.Text = string.Empty;
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
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

    protected void rbLoan_CheckedChanged(object sender, EventArgs e)
    {
        if (rbEmp.Checked)
        {
            pnlLoan.Visible = false;
            pnlEmp.Visible = true;
            drpEmp.SelectedIndex = 0;
            drpMonth.SelectedIndex = 0;
        }
        else
        {
            pnlLoan.Visible = true;
            grdDisplay.DataSource = null;
            grdDisplay.DataBind();
            pnlEmp.Visible = false;
        }
        fillGrid();
    }

    protected void drpEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpEmp.SelectedIndex > 0)
        {
            fillGrid();
        }
        else
        {
            grdDisplay.DataSource = null;
            grdDisplay.DataBind();
        }
    }

    protected void drpMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpMonth.SelectedIndex > 0)
        {
            fillGrid();
        }
        else
        {
            grdDisplay.DataSource = null;
            grdDisplay.DataBind();
            lblTotAmt.Text = "0";
        }
    }

    private void fillGrid()
    {
        if (drpLoan.SelectedIndex > 0)
        {
            if (rbEmp.Checked)
            {
                DataTable dataTable1 = new DataTable();
                Hashtable hashtable = new Hashtable();
                hashtable.Add("@LoanAcHeadId", drpLoan.SelectedValue);
                hashtable.Add("@EmpId", drpEmp.SelectedValue);
                if (drpMonth.SelectedIndex != 0)
                    hashtable.Add("@Month", drpMonth.SelectedValue);
                else
                    hashtable.Add("@Month", 3);
                string str = obj.ExecuteScalarQry("SELECT FY FROM dbo.ACTS_FinancialYear WHERE IsFinalized IS NULL").Split('-')[0];
                hashtable.Add("@Year", str.Trim());
                DataTable dataTable2 = obj.GetDataTable("HR_GetLoanAdjust", hashtable);
                grdDisplay.DataSource = dataTable2;
                grdDisplay.DataBind();
                Decimal num = new Decimal(0);
                foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
                    num += Convert.ToDecimal(row["RecAmt"].ToString().Trim());
                lblTotAmt.Text = num.ToString().Trim();
            }
            else
            {
                Hashtable hashtable = new Hashtable();
                obj = new clsDAL();
                hashtable.Add("@LoanId", drpLoan.SelectedValue);
                string str = obj.ExecuteScalar("HR_GetLoanPending", hashtable);
                if (str != "")
                    lblPendAmt.Text = str.Trim();
                else
                    lblPendAmt.Text = "0";
            }
        }
        else
        {
            lblMsg.Text = "Please Loan Account Head!!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void drpLoan_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpLoan.SelectedIndex > 0)
            fillGrid();
        else if (rbEmp.Checked)
        {
            lblTotAmt.Text = "0";
            grdDisplay.DataSource = null;
            grdDisplay.DataBind();
        }
        else
            lblPendAmt.Text = "0";
    }

    protected void btnAdjust_Click(object sender, EventArgs e)
    {
        if (!(lblTotAmt.Text != "0") || !(lblTotAmt.Text != ""))
            return;
        Hashtable hashtable1 = new Hashtable();
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        int num = 0;
        foreach (GridViewRow row in grdDisplay.Rows)
        {
            HiddenField control1 = (HiddenField)row.FindControl("hfGenLedgerId");
            HiddenField control2 = (HiddenField)row.FindControl("hfLoanRecId");
            Hashtable hashtable2 = new Hashtable();
            hashtable2.Clear();
            hashtable2.Add("@LoanAcHeadId", drpLoan.SelectedValue);
            hashtable2.Add("@LoanRecId", control2.Value);
            hashtable2.Add("@GenLedgerId", control1.Value);
            hashtable2.Add("@UserId", Session["User_Id"]);
            if (obj.ExecuteScalar("HR_UpdtLoanAdj", hashtable2).Trim() == string.Empty)
                ++num;
        }
        if (num == grdDisplay.Rows.Count)
        {
            Hashtable hashtable2 = new Hashtable();
            hashtable2.Clear();
            string str = obj.ExecuteScalarQry("SELECT FY FROM dbo.ACTS_FinancialYear WHERE IsFinalized IS NULL");
            hashtable2.Add("@EmpId", drpEmp.SelectedValue.Trim());
            hashtable2.Add("@Amount", Convert.ToDecimal(lblTotAmt.Text));
            hashtable2.Add("@FY", str);
            hashtable2.Add("@TransDt", DateTime.Today.ToString("dd MMM yyyy"));
            hashtable2.Add("@SchoolId", Session["SchoolId"]);
            hashtable2.Add("@UserId", Session["User_Id"].ToString().Trim());
            if (obj.ExecuteScalar("HR_InsLoanAdjust", hashtable2).Trim() == string.Empty)
            {
                lblMsg.Text = "Loan Adjusted Successfully!!";
                lblMsg.ForeColor = Color.Green;
                fillGrid();
            }
            else
            {
                lblMsg.Text = "Unable To AdjustLoan!!Please Try Again!!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
        {
            lblMsg.Text = "Unable To AdjustLoan!!Please Try Again!!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnAdjLoan_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        string str = obj.ExecuteScalarQry("SELECT FY FROM dbo.ACTS_FinancialYear WHERE IsFinalized IS NULL");
        hashtable.Add("@LoanId", drpLoan.SelectedValue);
        hashtable.Add("@Amount", Convert.ToDecimal(txtLoanAmt.Text));
        hashtable.Add("@FY", str);
        hashtable.Add("@TransDt", DateTime.Today.ToString("dd MMM yyyy"));
        hashtable.Add("@SchoolId", Session["SchoolId"].ToString().Trim());
        hashtable.Add("@UserId", Session["User_Id"].ToString().Trim());
        if (obj.ExecuteScalar("HR_InsLoanAdjust", hashtable).Trim() == string.Empty)
        {
            lblMsg.Text = "Loan Adjusted Successfully!!";
            lblMsg.ForeColor = Color.Green;
            fillGrid();
            txtLoanAmt.Text = "0";
        }
        else
        {
            lblMsg.Text = "Unable To AdjustLoan!!Please Try Again!!";
            lblMsg.ForeColor = Color.Red;
        }
    }
}