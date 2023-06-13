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

public partial class Masters_BrandMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Page.Form.DefaultButton = btnSave.UniqueID;
        if (Session["User"] != null)
        {
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
        txtBrandName.Text = new clsDAL().GetDataTableQry("SELECT * FROM SI_BrandMaster WHERE BrandId=" + Request.QueryString["Id"].ToString()).Rows[0]["BrandName"].ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveData();
    }

    public void SaveData()
    {
        try
        {
            clsDAL clsDal = new clsDAL();
            Hashtable ht = new Hashtable();
            if (Request.QueryString["Id"] != null)
                ht.Add("@BrandId", int.Parse(Request.QueryString["Id"].ToString()));
            ht.Add("@BrandName", txtBrandName.Text);
            ht.Add("@UserID", Convert.ToInt32(Session["User_Id"]));
            ht.Add("@UserDate", DateTime.Now.ToString("MM/dd/yyyy"));
            ht.Add("@ModRights", "u");
            ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
            if (clsDal.GetDataTable("SI_InsUpdtBrand", ht).Rows.Count > 0)
            {
                lblMsg.Text = "Record Already Exits  !";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                Clear();
                lblMsg.Text = "Data Saved Successfully ! ";
                lblMsg.ForeColor = Color.Green;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void Clear()
    {
        txtBrandName.Text = string.Empty;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void btngoto_Click(object sender, EventArgs e)
    {
        Response.Redirect("BrandList.aspx");
    }
}