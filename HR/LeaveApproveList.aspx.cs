using AjaxControlToolkit;
using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_LeaveApproveList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        GetPendingLeaveApproval();
    }

    protected void rbAction_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetPendingLeaveApproval();
    }

    private void GetPendingLeaveApproval()
    {
        Hashtable hashtable = new Hashtable();
        if (rbAction.SelectedValue == "A")
            hashtable.Add("@Action", "a");
        else if (rbAction.SelectedValue == "M")
            hashtable.Add("@Action", "m");
        DataTable dataTable = obj.GetDataTable("HR_GetPendingLeaveApproval", hashtable);
        grdLeave.DataSource = dataTable.DefaultView;
        grdLeave.DataBind();
        lblRecCount.Text = "No of Record(s) : " + dataTable.Rows.Count;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetPendingLeaveApproval();
    }
}