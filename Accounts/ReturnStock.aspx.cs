using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_ReturnStock : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            try
            {
                dtpFromDt.SetDateValue(DateTime.Now);
                dtpToDt.SetDateValue(DateTime.Now);
                bindDropDown();
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Transaction Failed ! Try Again.";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void bindDropDown()
    {
        try
        {
            ddlReceiveFrom.DataSource = (object)null;
            ddlReceiveFrom.DataBind();
            ddlReceiveFrom.DataSource = (object)obj.GetDataTableQry("select PartyId,PartyName from dbo.ACTS_PartyMaster order by PartyName");
            ddlReceiveFrom.DataTextField = "PartyName";
            ddlReceiveFrom.DataValueField = "PartyId";
            ddlReceiveFrom.DataBind();
            ddlReceiveFrom.Items.Insert(0, new ListItem("- ALL -", "0"));
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void BindGrid()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Select * from ACTS_VWStockList where SchoolId=" + (object)int.Parse(Session["SchoolId"].ToString()) + " and TotBillAmount != 0");
        if (!txtFrmDt.Text.Trim().Equals(string.Empty) && !txtToDt.Text.Trim().Equals(string.Empty))
            stringBuilder.Append(" and PurDate >='" + dtpFromDt.GetDateValue().ToString("MM/dd/yyyy") + " 00:00:00' and  PurDate<='" + dtpToDt.GetDateValue().ToString("MM/dd/yyyy") + " 23:59:59'");
        if (txtInvoice.Text.Trim() != "")
            stringBuilder.Append(" and InvNo ='" + txtInvoice.Text.Trim() + "'");
        if (ddlReceiveFrom.SelectedIndex > 0)
            stringBuilder.Append(" and PartyId = " + (object)int.Parse(ddlReceiveFrom.SelectedValue));
        stringBuilder.Append(" Order by PurDate desc");
        dt = obj.GetDataTableQry(stringBuilder.ToString());
        gvStockList.DataSource = (object)dt;
        gvStockList.DataBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            BindGrid();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void gvStockList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvStockList.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("StockReturnList.aspx");
    }
}