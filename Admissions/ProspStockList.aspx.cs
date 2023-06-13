using ASP;
using Classes.DA;
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

public partial class Admissions_ProspStockList : System.Web.UI.Page
{
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        ViewState["Stockist"] = string.Empty;
        FillSession();
        Fillgrid();
    }

    private void FillSession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select distinct SessionID,SessionYr from dbo.PS_FeeNormsNew order by sessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
        drpSession.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    private void Fillgrid()
    {
        dt = new DataTable();
        ht = new Hashtable();
        obj = new Common();
        if (drpSession.SelectedIndex > 0)
            ht.Add("@SessionYr", drpSession.SelectedValue);
        if (txtFromDt.Text != "")
            ht.Add("@FromDate", dtpPros.GetDateValue());
        if (txtToDt.Text != "")
            ht.Add("@Todate", dtpPros1.GetDateValue());
        int pageIndex = grdProspect.PageIndex;
        dt = obj.GetDataTable("Sp_GetProspectusStock", ht);
        ViewState["Stockist"] = dt;
        grdProspect.DataSource = dt;
        grdProspect.DataBind();
        grdProspect.PageIndex = pageIndex;
        if (dt.Rows.Count > 0)
            lblRecCount.Text = "Total Record: " + dt.Rows.Count.ToString();
        else
            lblRecCount.Text = "";
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Fillgrid();
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProspStock.aspx");
    }

    private void Delete(string id)
    {
        ht = new Hashtable();
        dt = new DataTable();
        obj = new Common();
        ht.Add("@TransId", id);
        dt = obj.GetDataTable("Ps_Sp_DelProspectusStock", ht);
        if (dt.Rows.Count > 0)
        {
            lblmsg1.Text = "Data Already Used";
            lblmsg1.ForeColor = Color.Red;
        }
        else
        {
            lblmsg1.Text = "Data Deleted Successfully";
            lblmsg1.ForeColor = Color.Green;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            lblmsg1.Text = "Select a checkbox";
            lblmsg1.ForeColor = Color.Red;
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                Delete(obj.ToString());
            Fillgrid();
        }
    }

    protected void grdProspect_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdProspect.PageIndex = e.NewPageIndex;
        Fillgrid();
    }
}