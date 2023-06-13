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

public partial class Masters_ItemLocationList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dtItemSection = new DataTable();
    private Hashtable htSection = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        Page.Form.DefaultButton = btnAddNew.UniqueID;
        if (Session["User"] != null)
            bindGrd();
        else
            Response.Redirect("~/Login.aspx");
    }

    private void bindGrd()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = clsDal.GetDataTable("SI_GetAllLocGrd", new Hashtable()
    {
      {
         "@SchoolId",
         Convert.ToInt32(Session["SchoolId"])
      }
    });
        grdLoc.DataSource = dataTable2;
        grdLoc.DataBind();
        if (dataTable2.Rows.Count > 0)
        {
            btnDelete.Enabled = true;
            lblRecord.Text = "Total Record(s) : " + dataTable2.Rows.Count.ToString();
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
        Response.Redirect("ItemLocationMaster.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["Checkb"] == null)
            {
                lblMsg.Text = "Select a checkbox !";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                string str = Request["Checkb"];
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str.Split(chArray))
                    DeleteItem(obj.ToString());
                bindGrd();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DeleteItem(string Id)
    {
        Hashtable ht = new Hashtable();
        clsDAL clsDal = new clsDAL();
        ht.Add("@LocationId", Id);
        if (clsDal.ExecuteScalar("SI_DelLocation", ht) == "Success")
        {
            lblMsg.Text = "Location Deleted Successfully !";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = "Location Already Used !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void grdLoc_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdLoc.PageIndex = e.NewPageIndex;
        bindGrd();
    }
}