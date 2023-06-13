using AjaxControlToolkit;
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
public partial class Accounts_PartyMasterList : System.Web.UI.Page
{
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            lblMsg.Text = string.Empty;
            if (Page.IsPostBack)
                return;
            FillDropdowns();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void FillDropdowns()
    {
        new clsStaticDropdowns().FillPartyType(drpPartyType);
        drpPartyType.Items.RemoveAt(0);
        drpPartyType.Items.Insert(0, new ListItem("--All--", "0"));
    }

    private void Fillgrid()
    {
        dt = new DataTable();
        ht = new Hashtable();
        obj = new clsDAL();
        if (drpPartyType.SelectedIndex > 0)
            ht.Add("@PartyType", drpPartyType.SelectedValue.ToString().Trim());
        if (drpACGroup.SelectedIndex > 0)
            ht.Add("@AGCode", drpACGroup.SelectedValue.ToString().Trim());
        ht.Add("@SchoolId", Session["SchoolId"]);
        dt = obj.GetDataTable("ACTS_GrdPartyDetails", ht);
        grdParty.DataSource = dt;
        grdParty.DataBind();
        grdParty.Visible = true;
        if (dt.Rows.Count > 0)
            lblRecord.Text = "Total Record : " + dt.Rows.Count.ToString();
        else
            lblRecord.Text = string.Empty;
    }

    protected void grdParty_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdParty.PageIndex = e.NewPageIndex;
        Fillgrid();
    }

    protected void grdParty_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            clsDAL clsDal = new clsDAL();
            DataTable dataTable = new DataTable();
            Hashtable hashtable = new Hashtable();
            Label control = (Label)grdParty.Rows[e.RowIndex].FindControl("lbl2");
            hashtable.Add("@PartyId", int.Parse(control.Text));
            hashtable.Add("@SchoolId", Session["SchoolId"]);
            if (clsDal.GetDataTable("ACTS_DelParty", hashtable).Rows.Count > 0)
            {
                lblMsg.Text = "Dependency for this record exist";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                Fillgrid();
                lblMsg.Text = "Record Deleted Successfully";
                lblMsg.ForeColor = Color.Green;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Dependency for this record exist";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("PartyMaster.aspx");
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        Fillgrid();
    }

    protected void drpACGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdParty.Visible = false;
    }

    protected void drpPartyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdParty.Visible = false;
    }
}