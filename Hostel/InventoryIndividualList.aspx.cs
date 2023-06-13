using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Hostel_InventoryIndividualList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            lblMsg.Text = string.Empty;
            if (Page.IsPostBack)
                return;
            Page.Form.DefaultButton = btnSearch.UniqueID;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            bindGrid();
        }
        catch (Exception ex)
        {
        }
    }

    private void bindGrid()
    {
        if (txtFrmDt.Text != "")
            ht.Add("@FromDate", PopCalendar1.GetDateValue());
        if (txtTo.Text != "")
            ht.Add("@Todate", PopCalendar2.GetDateValue());
        if (Session["User"].ToString() != "admin")
        {
            ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
            ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
        }
        dt = obj.GetDataTable("Host_GetAllIssue", ht);
        grdItem.DataSource = dt;
        grdItem.DataBind();
        if (dt.Rows.Count != 0)
            return;
        lblMsg.Text = "No Record !";
        lblMsg.ForeColor = Color.Red;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("InventoryIndividual.aspx");
    }

    protected void grdItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdItem.PageIndex = e.NewPageIndex;
        bindGrid();
    }

    protected void grdItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandName == "Delete"))
            return;
        ht.Clear();
        ht.Add("@IssueId", int.Parse(e.CommandArgument.ToString()));
        if (obj.ExecuteScalar("Host_DelIssue", ht) == "")
        {
            lblMsg.Text = "Data Deleted Successfully !";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = "Deletion Failed";
            lblMsg.ForeColor = Color.Red;
        }
    }
}