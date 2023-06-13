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

public partial class Masters_BrandList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dtItemSection = new DataTable();
    private Hashtable htSection = new Hashtable();

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
        Hashtable ht = new Hashtable();
        clsDAL clsDal = new clsDAL();
        ht.Add("@SchoolId",Convert.ToInt32(Session["SchoolId"]));
        try
        {
            dataTable = clsDal.GetDataTable("SI_GetAllBrand", ht);
            grdBrandMaster.DataSource =dataTable;
            grdBrandMaster.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        if (dataTable.Rows.Count > 0)
        {
            btnDelete.Enabled = true;
            lblRecord.Text = "Total Record(s) : " + dataTable.Rows.Count.ToString();
            lblMsg.Text = string.Empty;
        }
        else
        {
            lblRecord.Text = string.Empty;
            btnDelete.Enabled = false;
            lblMsg.Text = "No Record !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("BrandMaster.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            lblMsg.Text = "select a checkbox !";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                deleteRecord(obj.ToString());
            bindGrd();
        }
    }

    private void deleteRecord(string id)
    {
        Hashtable ht = new Hashtable();
        DataTable dataTable = new DataTable();
        clsDAL clsDal = new clsDAL();
        ht.Add((object)"@BrandId",id);
        if (clsDal.GetDataTable("SI_DelBrand", ht).Rows.Count > 0)
        {
            lblMsg.Text = "Record Can't be Deleted !";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Record Deleted Successfully !";
            lblMsg.ForeColor = Color.Green;
        }
    }

    protected void grdBrandMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdBrandMaster.PageIndex = e.NewPageIndex;
        bindGrd();
    }
}