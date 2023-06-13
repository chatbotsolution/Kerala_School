using AjaxControlToolkit;
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

public partial class HR_MiscRecoveryList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            BindData();
        lblMsg.Text = string.Empty;
        trMsg.BgColor = "Transparent";
    }

    private void BindData()
    {
        DataTable dataTable = new DataTable();
        obj = new clsDAL();
        grdMiscRec.DataSource = obj.GetDataTable("HR_MiscRecoveryList");
        grdMiscRec.DataBind();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("MiscRecovery.aspx");
    }

    protected void grdMiscRec_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandName == "CM"))
            return;
        string empty = string.Empty;
        Hashtable hashtable = new Hashtable();
        obj = new clsDAL();
        hashtable.Add("@MiscId", e.CommandArgument);
        string str = obj.ExecuteScalar("HR_CancelMiscRecovery", hashtable);
        if (str.Trim() == string.Empty)
        {
            lblMsg.Text = "Delete Successfully";
            trMsg.BgColor = "Green";
            BindData();
        }
        else if (str.Trim().ToUpper() == "E")
        {
            lblMsg.Text = "Transaction on Future Date exists. Can not be deleted.";
            trMsg.BgColor = "Red";
        }
        else if (str.Trim().ToUpper() == "G")
        {
            lblMsg.Text = "Salary already Generated, Can not be deleted.";
            trMsg.BgColor = "Red";
        }
        else
        {
            lblMsg.Text = "Failed to Delete. Try Again.";
            trMsg.BgColor = "Red";
        }
    }

    protected void grdMiscRec_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdMiscRec.PageIndex = e.NewPageIndex;
        BindData();
    }
}