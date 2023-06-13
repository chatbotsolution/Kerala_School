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

public partial class Masters_ItemCatagory : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Page.Form.DefaultButton = btnsubmit.UniqueID;
        if (Session["User"] != null)
        {
            BindCat();
            if (Request.QueryString["Id"] == null)
                return;
            Fillfilds(sender, e);
        }
        else
            Response.Redirect("~/Login.aspx");
    }

    private void Fillfilds(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = obj.GetDataTableQry("select * from SI_CategoryMaster where CatId='" + Request.QueryString["id"] + "'");
        dptcat.SelectedValue = dataTableQry.Rows[0]["ParentCatId"].ToString();
        txtcatname.Text = dataTableQry.Rows[0]["CatName"].ToString();
    }

    private void BindCat()
    {
        DataTable dataTable = new clsDAL().GetDataTable("SI_CatList");
        dptcat.Items.Clear();
        dptcat.Items.Add(new ListItem("-Root Category-", "0"));
        if (dataTable.Rows.Count <= 0)
            return;
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            dptcat.Items.Add(new ListItem(row["CatName"].ToString(), row["CatId"].ToString()));
        foreach (ListItem listItem in dptcat.Items)
        {
            if (!listItem.Text.StartsWith("-"))
            {
                listItem.Attributes.Add("Style", "font-weight:bold");
                listItem.Attributes.Add("Style", "text-transform:uppercase");
                listItem.Attributes.Add("Style", "color:maroon");
            }
        }
    }

    protected void Submit_Click(object sender, EventArgs e)
    {
        Hashtable ht = new Hashtable();
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        if (Request.QueryString["id"] != null)
            ht.Add("CatId", Request.QueryString["id"]);
        ht.Add("ParentCatId", dptcat.SelectedValue.ToString());
        ht.Add("CatName", txtcatname.Text.Trim());
        ht.Add("UserID", Convert.ToInt32(Session["User_Id"]));
        ht.Add("ModRights", "u");
        ht.Add("SchoolId", Convert.ToInt32(Session["SchoolId"]));
        if (clsDal.GetDataTable("SI_InsUpdateCat", ht).Rows.Count > 0)
        {
            lblMsg.Text = "Record Already Exits  !";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            Clear();
            lblMsg.Text = "Data Saved Successfully ! ";
            lblMsg.ForeColor = Color.Green;
            BindCat();
        }
    }

    private void Clear()
    {
        txtcatname.Text = "";
        dptcat.SelectedIndex = 0;
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        Response.Redirect("CatagoryList.aspx");
    }
}