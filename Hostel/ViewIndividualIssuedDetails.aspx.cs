using ASP;
using SanLib;
using System;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Hostel_ViewIndividualIssuedDetails : System.Web.UI.Page
{
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
    clsDAL clsDal = new clsDAL();
    DataTable dataTable = new DataTable();
    DataTable dataTableQry = clsDal.GetDataTableQry("select H.ItemCode,H.IssueId,H.Qty ,It.ItemName from dbo.Host_IssueDetails H inner join dbo.SI_ItemMaster It on H.ItemCode=It.ItemCode where  IssueId='" + Request.QueryString["Id"].ToString() + "'");
    grdIssueDetails.DataSource =  dataTableQry;
    grdIssueDetails.DataBind();
    if (dataTableQry.Rows.Count > 0)
      grdIssueDetails.Visible = true;
    else
      grdIssueDetails.Visible = false;
  }
}
