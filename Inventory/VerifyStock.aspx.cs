using ASP;
using SanLib;
using System;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Inventory_VerifyStock : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dtItemSection = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
            bindGrd();
        else
            Response.Redirect("~/Login.aspx");
    }

    private void bindGrd()
    {
        DataTable dataTable = new DataTable();
        grdVerify.DataSource = (object)obj.GetDataTable("GetStockVerify");
        grdVerify.DataBind();
    }

    protected void grdVerify_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdVerify.PageIndex = e.NewPageIndex;
        bindGrd();
    }
}