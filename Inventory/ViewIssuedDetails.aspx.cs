using ASP;
using Classes.DA;
using System;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Inventory_ViewIssuedDetails : System.Web.UI.Page
{
    private Common obj;
    private DataTable dt;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Request.QueryString["Id"] == null)
            return;
        fillGrid();
    }

    private void fillGrid()
    {
        obj = new Common();
        dt = new DataTable();
        dt = obj.ExecuteSql("select I.ItemCode,I.IssueId,I.Qty ,It.ItemName from dbo.SI_IssueDetails I inner join dbo.SI_ItemMaster It on I.ItemCode=It.ItemCode where  IssueId='" + Request.QueryString["Id"].ToString() + "'");
        grdIssueDetails.DataSource = dt;
        grdIssueDetails.DataBind();
        if (dt.Rows.Count > 0)
            grdIssueDetails.Visible = true;
        else
            grdIssueDetails.Visible = false;
    }
}