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

public partial class Admissions_ProspStock : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    private DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        FillSession();
        FillProspType();
        ViewState["Stock"] = string.Empty;
        if (Request.QueryString["RId"] != null)
            FillData();
        if (Request.QueryString["Id"] == null)
            return;
        drpProspectusType.SelectedValue = obj.ExecuteScalarQry("select ProspType from dbo.ProspTypeMaster Where ProspTypeId=" + int.Parse(Request.QueryString["Id"].ToString()));
    }

    private void FillSession()
    {
        obj = new clsDAL();
        drpSession.DataSource = obj.GetDataTableQry("select distinct SessionID,SessionYr from dbo.PS_FeeNormsNew order by sessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillProspType()
    {
        obj = new clsDAL();
        DataTable dataTable = new DataTable();
        drpProspectusType.DataSource = obj.GetDataTableQry("select ProspTypeId,ProspType from dbo.ProspTypeMaster order by ProspTypeId");
        drpProspectusType.DataTextField = "ProspType";
        drpProspectusType.DataValueField = "ProspType";
        drpProspectusType.DataBind();
        drpProspectusType.Items.Insert(0, "- SELECT -");
    }

    public void FillData()
    {
        obj = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("Ps_Sp_SelProspectusStock", new Hashtable()
    {
      {
         "@TransId",
         Request.QueryString["RId"].ToString()
      }
    });
        drpSession.SelectedValue = dataTable2.Rows[0]["SessionYr"].ToString();
        txtTransdt.Text = Convert.ToDateTime(dataTable2.Rows[0]["TransDate"].ToString()).ToString("dd-MMM-yyyy");
        drpProspectusType.SelectedValue = dataTable2.Rows[0]["ProspectusType"].ToString().Trim();
        txtQtyIn.Text = dataTable2.Rows[0]["QtyIn"].ToString();
        txtPrice.Text = dataTable2.Rows[0]["UnitSP"].ToString();
        ViewState["Stock"] = "Update";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        if (Request.QueryString["RId"] != null && ViewState["Stock"].ToString() == "Update")
            hashtable.Add("@TransId", Request.QueryString["RId"].ToString());
        hashtable.Add("@SessionYr", drpSession.SelectedValue);
        hashtable.Add("@TransDate", txtTransdt.Text);
        hashtable.Add("@ProspectusType", drpProspectusType.SelectedValue);
        hashtable.Add("@QtyIn", txtQtyIn.Text);
        hashtable.Add("@UnitSP", txtPrice.Text.Trim());
        hashtable.Add("@UserId", Session["User_Id"]);
        obj = new clsDAL();
        if (obj.GetDataTable("Ps_sp_InsUpdProspectusStock", hashtable).Rows.Count > 0)
        {
            lblerr.Text = "Data Already Exists";
            lblerr.ForeColor = Color.Red;
        }
        else
        {
            lblerr.Text = "Data Saved Sucessfully";
            lblerr.ForeColor = Color.Green;
            if (Request.QueryString["RId"] != null && ViewState["Stock"].ToString() == "Update")
                Response.Redirect("~/Admissions/ProspStockList.aspx");
        }
        ClearAll();
        ViewState["Stock"] = string.Empty;
    }

    private void ClearAll()
    {
        txtQtyIn.Text = string.Empty;
        txtTransdt.Text = string.Empty;
        drpProspectusType.SelectedIndex = 0;
        txtPrice.Text = string.Empty;
        drpSession.SelectedIndex = 0;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProspStockList.aspx");
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admissions/ProspTypeAddNew.aspx");
    }
}