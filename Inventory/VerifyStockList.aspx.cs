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

public partial class Inventory_VerifyStockList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] != null)
        {
            lblMsg.Text = string.Empty;
            if (Page.IsPostBack)
                return;
            Page.Form.DefaultButton = btnShow.UniqueID;
            bindrp();
            btnSubmit.Visible = false;
            btnSubmit.Attributes.Add("onclick", "return GridCheck()");
        }
        else
            Response.Redirect("~/Login.aspx");
    }

    private void bindrp()
    {
        bindDropDown(drpLoc, "select distinct sd.LocationId, L.Location from dbo.SI_StockCheckDoc sd inner join dbo.SI_LocationMaster L ON sd.LocationId=L.LocationId WHERE L.SchoolId=" + Session["SchoolId"] + " order by LocationId", "Location", "LocationId");
        bindDropDown(drpchkdt, "select DISTINCT CONVERT(VARCHAR,CheckDate,106)AS SDate from dbo.SI_StockCheckDoc", "SDate", "SDate");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
    }

    private void bindGrd()
    {
        DataTable dataTable1 = new DataTable();
        clsDAL clsDal = new clsDAL();
        Hashtable ht = new Hashtable();
        if (drpchkdt.SelectedIndex > 0)
            ht.Add("@date", drpchkdt.SelectedValue);
        if (drpLoc.SelectedIndex > 0)
            ht.Add("@LocationId", drpLoc.SelectedValue);
        ht.Add("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
        DataTable dataTable2 = clsDal.GetDataTable("SI_GetStockVerify", ht);
        grdVerify.DataSource = dataTable2;
        grdVerify.DataBind();
        if (dataTable2.Rows.Count > 0)
        {
            grdVerify.Visible = true;
            btnSubmit.Visible = true;
        }
        else
        {
            btnSubmit.Visible = false;
            lblMsg.Text = "No Record Found !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void grdVerify_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdVerify.PageIndex = e.NewPageIndex;
        bindGrd();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int num = 0;
        foreach (GridViewRow row in grdVerify.Rows)
        {
            TextBox control1 = row.FindControl("txtRemarks") as TextBox;
            HiddenField control2 = row.FindControl("hfStockCheckId") as HiddenField;
            HiddenField control3 = row.FindControl("hfLoc") as HiddenField;
            HiddenField control4 = row.FindControl("hfItemCode") as HiddenField;
            HiddenField control5 = row.FindControl("hfPhyStock") as HiddenField;
            HiddenField control6 = row.FindControl("hfTranStock") as HiddenField;
            if (row.RowType == DataControlRowType.DataRow)
            {
                Hashtable ht = new Hashtable();
                DataTable dataTable = new DataTable();
                clsDAL clsDal = new clsDAL();
                ht.Add("@StockCheckId", Convert.ToInt32(control2.Value));
                ht.Add("@LocationId", Convert.ToInt32(control3.Value));
                ht.Add("@ItemCode", Convert.ToInt32(control4.Value));
                ht.Add("@PhysicalStock", Convert.ToInt32(control5.Value));
                ht.Add("@TransactionStock", Convert.ToInt32(control6.Value));
                ht.Add("@Remarks", control1.Text);
                ht.Add("@VerifiedBy", Session["User"].ToString());
                ht.Add("@UserID", Convert.ToInt32(Session["User_Id"]));
                ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
                if (clsDal.GetDataTable("SI_InsUpdtStockVerify", ht).Rows.Count <= 0)
                    ++num;
            }
        }
        if (num > 0)
        {
            lblMsg.Text = "Record Verified Successfully !";
            lblMsg.ForeColor = Color.Green;
            grdVerify.Visible = false;
            btnSubmit.Visible = false;
        }
        else
        {
            lblMsg.Text = "Record Already Verified !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        bindGrd();
    }

    protected void btnMismatched_Click(object sender, EventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        clsDAL clsDal = new clsDAL();
        Hashtable ht = new Hashtable();
        if (drpchkdt.SelectedIndex > 0)
            ht.Add("@date", drpchkdt.SelectedValue);
        if (drpLoc.SelectedIndex > 0)
            ht.Add("@LocationId", drpLoc.SelectedValue);
        ht.Add("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
        DataTable dataTable2 = clsDal.GetDataTable("SI_GetStockVerified", ht);
        grdVerify.DataSource = dataTable2;
        grdVerify.DataBind();
        if (dataTable2.Rows.Count > 0)
        {
            grdVerify.Visible = true;
            btnSubmit.Visible = true;
        }
        else
        {
            btnSubmit.Visible = false;
            lblMsg.Text = "No Record Found !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void grdVerify_PreRender(object sender, EventArgs e)
    {
        ClientScriptManager clientScript = Page.ClientScript;
        foreach (Control row in grdVerify.Rows)
        {
            TextBox control = (TextBox)row.FindControl("txtRemarks");
            clientScript.RegisterArrayDeclaration("grd_remarks", "'" + control.ClientID + "'");
        }
    }
}