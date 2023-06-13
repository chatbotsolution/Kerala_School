using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_SupplierList : System.Web.UI.Page
{
    private Common obj = new Common();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CollegeId"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        fillgrid();
    }

    private void fillgrid()
    {
        dt = new DataTable();
        ht = new Hashtable();
        obj = new Common();
        int pageIndex = grdSupplierMaster.PageIndex;
        ht.Add("CollegeId", Session["CollegeId"]);
        dt = obj.GetDataTable("ps_sp_get_Supplier", ht);
        grdSupplierMaster.DataSource = dt;
        grdSupplierMaster.DataBind();
        grdSupplierMaster.PageIndex = pageIndex;
        if (dt.Rows.Count > 0)
            lblRecCount.Text = "Total Record: " + dt.Rows.Count.ToString();
        else
            lblRecCount.Text = "";
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("SupplierMaster.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                DeleteDept(obj.ToString());
            fillgrid();
        }
    }

    private void DeleteDept(string id)
    {
        ht = new Hashtable();
        dt = new DataTable();
        obj = new Common();
        ht.Add("@SupId", id);
        dt = obj.GetDataTable("ps_sp_delete_SupplierAfterCheck", ht);
        if (dt.Rows.Count > 0)
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Data can not be deleted !');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Data deleted Successfully !');", true);
    }

    protected void grdSupplierMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSupplierMaster.PageIndex = e.NewPageIndex;
        fillgrid();
    }
}