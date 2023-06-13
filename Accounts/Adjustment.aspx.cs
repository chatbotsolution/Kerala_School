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


public partial class Accounts_Adjustment : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckHR();
        if (!Page.IsPostBack)
        {
            BindSessionYr();
            BindGrid();
        }
        lblMsg1.Text = lblMsg.Text = string.Empty;
    }

    private void CheckHR()
    {
        if (Convert.ToInt32(obj.ExecuteScalarQry("SELECT COUNT(*) FROM dbo.HR_EmpLoanRecovery")) + Convert.ToInt32(obj.ExecuteScalarQry("SELECT COUNT(*) FROM dbo.HR_EmpMlySalary")) <= 0)
            return;
        ScriptManager.RegisterStartupScript((Page)this, GetType(), "redirect", "alert('You have already used HR & Payroll Module. This page can not be accessed.'); window.location='Welcome.aspx';", true);
    }

    private void BindSessionYr()
    {
        DataTable dataTable = new DataTable();
        drpSession.DataSource = obj.GetDataTableQry("select FY_Id,FY from dbo.ACTS_FinancialYear order by FY_Id desc");
        drpSession.DataTextField = "FY";
        drpSession.DataValueField = "FY";
        drpSession.DataBind();
    }

    private void BindGrid()
    {
        DataTable dataTable = obj.GetDataTable("Acts_GetSalForAdj", new Hashtable()
    {
      {
         "@FY",
         drpSession.SelectedValue
      }
    });
        grdMonth.DataSource = dataTable;
        grdMonth.DataBind();
        lblrecords.Text = "No of Records : " + dataTable.Rows.Count;
        if (dataTable.Rows.Count > 0)
        {
            btnSave.Enabled = true;
            lblTotalAdj.Text = "Total Loan/Advance Amount adjusted with Salary : " + dataTable.Compute("SUM(AdjAmt)", "");
        }
        else
        {
            btnSave.Enabled = false;
            lblTotalAdj.Text = string.Empty;
        }
    }

    private void TempTable()
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn("CalMonth", typeof(string)));
        dataTable.Columns.Add(new DataColumn("CalYear", typeof(string)));
        dataTable.Columns.Add(new DataColumn("TransAmt", typeof(string)));
        dataTable.Columns.Add(new DataColumn("GenLedgerId", typeof(string)));
        dataTable.AcceptChanges();
        ViewState["TempTable"] = dataTable;
    }

    private void FillGrid()
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = (DataTable)ViewState["TempTable"];
        dataTable2.Clear();
        string str1 = "APR";
        string str2 = obj.ExecuteScalarQry("SELECT YEAR(StartDate) FROM dbo.ACTS_FinancialYear WHERE IsFinalized IS NULL");
        if (str2.Trim() != string.Empty)
        {
            if (str1.ToUpper() == "JAN" || str1.ToUpper() == "FEB" || str1.ToUpper() == "MAR")
                str2 = (Convert.ToInt32(str2) + 1).ToString();
            for (int index = 1; index <= 12; ++index)
            {
                DataRow row = dataTable2.NewRow();
                row["CalMonth"] = str1.ToUpper();
                row["CalYear"] = str2;
                row["TransAmt"] = "0";
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
            grdMonth.DataSource = dataTable2;
            grdMonth.DataBind();
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('First Initialize the Current Session Year.');window.location='HRHome.aspx'", true);
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ImageButton control1 = (ImageButton)((Control)sender).Parent.Parent.FindControl("btnDelete");
        HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfGenLedgerId");
        obj.ExecuteScalarQry("DELETE Acts_GenLedger WHERE GenLedgerId IN (" + Convert.ToInt64(control2.Value) + "," + (Convert.ToInt64(control2.Value) + 1L) + ")");
        BindGrid();
        lblMsg1.Text = lblMsg.Text = "Deleted Successfully. Check the <a href='rptAccountsLedger.aspx' target='_blank'>Account Ledger Report</a>.";
        lblMsg1.ForeColor = lblMsg.ForeColor = Color.Green;
    }

    protected void btnSaveIndv_Click(object sender, EventArgs e)
    {
        Button control1 = (Button)((Control)sender).Parent.Parent.FindControl("btnSaveIndv");
        Label control2 = (Label)((Control)sender).Parent.Parent.FindControl("lblTransDate");
        Label control3 = (Label)((Control)sender).Parent.Parent.FindControl("lblVrNo");
        HiddenField control4 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfGenLedgerId");
        TextBox control5 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtAmount");
        if (control5.Text.Trim() != string.Empty && Convert.ToDouble(control5.Text.Trim()) > 0.0)
        {
            Hashtable hashtable = new Hashtable();
            if (control3.Text.Trim() == string.Empty)
                hashtable.Add("@GenLedgerId", control4.Value);
            hashtable.Add("@FY", drpSession.SelectedValue);
            hashtable.Add("@TransDate", control2.Text.Trim());
            hashtable.Add("@TransAmt", control5.Text);
            hashtable.Add("@UserId", Session["User_Id"]);
            hashtable.Add("@SchoolId", Session["SchoolId"]);
            if (!(obj.ExecuteScalar("Acts_InsSalAdj", hashtable).Trim() == string.Empty))
                return;
            lblMsg1.Text = lblMsg.Text = "Data Saved Successfully. Check the <a href='rptAccountsLedger.aspx' target='_blank'>Account Ledger Report</a>.";
            lblMsg1.ForeColor = lblMsg.ForeColor = Color.Green;
            BindGrid();
        }
        else
        {
            lblMsg1.Text = lblMsg.Text = "Enter Total Amount Deducted from Salary against Loan/Advance.";
            lblMsg1.ForeColor = lblMsg.ForeColor = Color.Red;
            control5.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int num = 0;
        foreach (GridViewRow row in grdMonth.Rows)
        {
            Label control1 = row.FindControl("lblTransDate") as Label;
            Label control2 = row.FindControl("lblVrNo") as Label;
            HiddenField control3 = row.FindControl("hfGenLedgerId") as HiddenField;
            TextBox control4 = row.FindControl("txtAmount") as TextBox;
            if (control4.Text.Trim() != string.Empty && Convert.ToDouble(control4.Text.Trim()) > 0.0)
            {
                Hashtable hashtable = new Hashtable();
                if (control2.Text.Trim() == string.Empty)
                    hashtable.Add("@GenLedgerId", control3.Value);
                hashtable.Add("@FY", drpSession.SelectedValue);
                hashtable.Add("@TransDate", control1.Text.Trim());
                hashtable.Add("@TransAmt", control4.Text);
                hashtable.Add("@UserId", Session["User_Id"]);
                hashtable.Add("@SchoolId", Session["SchoolId"]);
                if (obj.ExecuteScalar("Acts_InsSalAdj", hashtable).Trim() == string.Empty)
                    ++num;
            }
        }
        if (num > 0)
        {
            lblMsg1.Text = lblMsg.Text = "Data Saved Successfully. Check the <a href='rptAccountsLedger.aspx' target='_blank'>Account Ledger Report</a>.";
            lblMsg1.ForeColor = lblMsg.ForeColor = Color.Green;
            BindGrid();
            grdMonth.Focus();
        }
        else if (num == 0)
        {
            lblMsg1.Text = lblMsg.Text = "Enter Total Amount Deducted from Salary against Loan/Advance.";
            lblMsg1.ForeColor = lblMsg.ForeColor = Color.Red;
            grdMonth.Focus();
        }
        else
        {
            lblMsg1.Text = lblMsg.Text = "Failed to Save Data.";
            lblMsg1.ForeColor = lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void btnExpense_Click(object sender, EventArgs e)
    {
        Response.Redirect("MiscExpensesEntry.aspx");
    }
}