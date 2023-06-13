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

public partial class HR_LoanAdvanceList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        if (!ChkIsHRUsed())
            Response.Redirect("HRHome.aspx?IsHRUsed=0");
        else
            BindDropdown(drpEmp, "SELECT EmpId,AccHeadId,SevName as EmpName FROM dbo.HR_EmployeeMaster Where ActiveStatus=1 ORDER BY EmpName", "EmpName", "AccHeadId");
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void BindDropdown(DropDownList drp, string Qry, string Text, string Value)
    {
        DataTable dataTableQry = obj.GetDataTableQry(Qry.Trim());
        drp.DataSource = dataTableQry.DefaultView;
        drp.DataTextField = Text;
        drp.DataValueField = Value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
        btnSearch.Focus();
    }

    private void FillGrid()
    {
        trMsg.Style["background-color"] = "Transparent";
        lblMsg.Text = string.Empty;
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpEmp.SelectedIndex > 0)
            hashtable.Add("@EmpAccHeadId", drpEmp.SelectedValue);
        DataTable dataTable2 = obj.GetDataTable("HR_LoanAdvanceList", hashtable);
        grdLoan.DataSource = dataTable2;
        grdLoan.DataBind();
        lblRecords.Text = "No of Records : " + dataTable2.Rows.Count;
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("LoanAdvance.aspx");
    }

    protected void grdLoan_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandName == "DeleteLoan"))
            return;
        trMsg.Style["background-color"] = "Transparent";
        lblMsg.Text = string.Empty;
        string str = obj.ExecuteScalar("HR_DeleteLoanAdvance", new Hashtable()
    {
      {
         "@GenLedgerId",
        e.CommandArgument
      }
    });
        if (str.Trim().ToUpper() == "S")
        {
            FillGrid();
            trMsg.Style["background-color"] = "Green";
            lblMsg.Text = "Loan Deleted Successfully";
        }
        else
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = str;
        }
    }

    protected void drpEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        drpEmp.Focus();
    }

    protected void grdLoan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdLoan.PageIndex = e.NewPageIndex;
        FillGrid();
    }
}