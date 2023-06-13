using ASP;
using SanLib;
using System;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_ArrearList : System.Web.UI.Page
{
    private clsDAL ObjDAL = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!ChkIsHRUsed())
            {
                Response.Redirect("HRHome.aspx?IsHRUsed=0");
            }
            else
            {
                bindDropDown(drpDesignation, "select DesgId,Designation from dbo.HR_DesignationMaster where TeachingStaff<>'M' order by Designation", "Designation", "DesgId");
                bindDropDown(drpEmpName, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster Where ActiveStatus=1 ORDER BY EmpName", "EmpName", "EmpId");
                bindDropDown(drpDesc, "SELECT DISTINCT Reason FROM dbo.HR_LeaveClaim WHERE LeaveApplyId IS NULL AND Remarks LIKE 'Interim Payment%'", "Reason", "Reason");
                if (drpDesc.Items.Count <= 1)
                    btnSearch.Enabled = false;
            }
        }
        lblMsg.Text = string.Empty;
        trMsg.BgColor = "Transparent";
    }

    private bool ChkIsHRUsed()
    {
        return ObjDAL.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.Items.Clear();
        DataTable dataTable = new DataTable();
        ObjDAL = new clsDAL();
        DataTable dataTableQry = ObjDAL.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        if (dataTableQry.Rows.Count > 0)
            drp.Items.Insert(0, new ListItem("- ALL -", "0"));
        else
            drp.Items.Insert(0, new ListItem("- Not Available -", "0"));
    }

    protected void drpEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpEmpName.Focus();
        FillGrid();
    }

    protected void drpDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDesignation.SelectedIndex > 0)
            bindDropDown(drpEmpName, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster where DesignationId=" + drpDesignation.SelectedValue + " ORDER BY EmpName", "EmpName", "EmpId");
        else
            bindDropDown(drpEmpName, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster ORDER BY EmpName", "EmpName", "EmpId");
        FillGrid();
        drpDesignation.Focus();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
        btnSearch.Focus();
    }

    private void FillGrid()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT emp.EmpId, emp.SevName AS EmpName, dm.Designation, lc.ClaimId, lc.Reason as ArrearDesc, CAST(PaidAmt AS DECIMAL(12,2)) as Amt ");
        stringBuilder.Append("FROM dbo.HR_LeaveClaim lc ");
        stringBuilder.Append("INNER JOIN dbo.HR_EmployeeMaster emp ON lc.EmpId=emp.EmpId ");
        stringBuilder.Append("LEFT JOIN dbo.HR_DesignationMaster dm ON emp.DesignationId=dm.DesgId ");
        stringBuilder.Append("WHERE lc.SalId IS NULL AND lc.LeaveApplyId IS NULL AND PaidDate IS NULL AND lc.Remarks LIKE 'Interim Payment%'");
        stringBuilder.Append(" AND PaidMonth NOT IN (SELECT SalMonth FROM dbo.HR_GenerateMlySalary WHERE SalMonth=lc.PaidMonth AND SalYear=lc.Paidyear)");
        stringBuilder.Append(" AND PaidYear NOT IN (SELECT SalYear FROM dbo.HR_GenerateMlySalary WHERE SalMonth=lc.PaidMonth AND SalYear=lc.Paidyear)");
        if (drpDesignation.SelectedIndex > 0)
            stringBuilder.Append(" AND emp.DesignationId=" + drpDesignation.SelectedValue);
        if (drpEmpName.SelectedIndex > 0)
            stringBuilder.Append(" AND lc.EmpId=" + drpEmpName.SelectedValue);
        if (drpDesc.SelectedIndex > 0)
            stringBuilder.Append(" AND lc.Reason='" + drpDesc.SelectedValue + "'");
        DataTable dataTableQry = ObjDAL.GetDataTableQry(stringBuilder.ToString());
        grdList.DataSource = dataTableQry;
        grdList.DataBind();
        if (dataTableQry.Rows.Count > 0)
            btnDelete.Enabled = true;
        else
            btnDelete.Enabled = false;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select any Record');", true);
                btnDelete.Enabled = true;
                btnDelete.Focus();
            }
            else
            {
                string str1 = Request["Checkb"];
                string empty = string.Empty;
                string str2 = str1;
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str2.Split(chArray))
                {
                    foreach (GridViewRow row in grdList.Rows)
                    {
                        HiddenField control = (HiddenField)row.FindControl("hfClaimId");
                        if (((HiddenField)row.FindControl("hfEmpId")).Value == obj.ToString())
                            ObjDAL.ExecuteScalarQry("DELETE dbo.HR_LeaveClaim WHERE ClaimId=" + control.Value);
                    }
                }
                lblMsg.Text = "Deleted Successfully.";
                trMsg.BgColor = "Green";
                FillGrid();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Failed to Delete. Try Again.";
            trMsg.BgColor = "Red";
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("ArrearAuthorize.aspx");
    }
}