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

public partial class Accounts_ReceiveStockList : System.Web.UI.Page
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
            ddlReceiveFrom.DataSource = null;
            ddlReceiveFrom.DataBind();
            ddlReceiveFrom.DataSource = obj.GetDataTableQry("select PartyId,PartyName from dbo.ACTS_PartyMaster order by PartyName");
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
        stringBuilder.Append("Select * from ACTS_VWStockList where SchoolId=" + int.Parse(Session["SchoolId"].ToString()));
        if (!txtFrmDt.Text.Trim().Equals(string.Empty) && !txtToDt.Text.Trim().Equals(string.Empty))
            stringBuilder.Append(" and PurDate >='" + dtpFromDt.GetDateValue().ToString("MM/dd/yyyy") + " 00:00:00' and  PurDate<='" + dtpToDt.GetDateValue().ToString("MM/dd/yyyy") + " 23:59:59'");
        if (txtInvoice.Text.Trim() != "")
            stringBuilder.Append(" and InvNo ='" + txtInvoice.Text.Trim() + "'");
        if (ddlReceiveFrom.SelectedIndex > 0)
            stringBuilder.Append(" and PartyId = " + int.Parse(ddlReceiveFrom.SelectedValue));
        stringBuilder.Append(" Order by PurDate desc");
        dt = obj.GetDataTableQry(stringBuilder.ToString());
        gvStockList.DataSource = dt;
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReceiveStockPO.aspx");
    }

    protected void gvStockList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (!(e.CommandName == "Remove"))
                return;
            ht.Clear();
            ht.Add("@PurchaseId", int.Parse(e.CommandArgument.ToString()));
            ht.Add("@TransDt", Session["TransDt"]);
            ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
            ht.Add("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
            string str = obj.ExecuteScalar("ACTS_DelPurchaseStock", ht);
            if (str.Trim() == string.Empty)
            {
                lblMsg.ForeColor = Color.Green;
                lblMsg.Text = "Stock Deleted Successfully";
            }
            else
            {
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = str;
            }
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

    protected void gvStockList_DataBound(object sender, EventArgs e)
    {
        if (!(Session["userrights"].ToString().Trim() == "a"))
            return;
        foreach (Control row in gvStockList.Rows)
            row.FindControl("imgbtnDelete").Visible = true;
    }
}