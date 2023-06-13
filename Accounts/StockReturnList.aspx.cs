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

public partial class Accounts_StockReturnList : System.Web.UI.Page
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

    private void BindGrid()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select  R.*,convert(varchar,R.RetDate,106) RetDt,P.InvNo from dbo.ACTS_PurchaseReturnDoc R Inner Join dbo.ACTS_PurchaseDoc P on P.PurchaseId=R.PurchaseId  where R.IsDeleted=0 and R.SchoolId=" + int.Parse(Session["SchoolId"].ToString()));
        if (!txtFrmDt.Text.Trim().Equals(string.Empty) && !txtToDt.Text.Trim().Equals(string.Empty))
            stringBuilder.Append(" and RetDate >='" + dtpFromDt.GetDateValue().ToString("dd MMM yyyy") + " 00:00:00' and  RetDate<='" + dtpToDt.GetDateValue().ToString("dd MMM yyyy") + " 23:59:59'");
        if (txtInvoice.Text.Trim() != "")
            stringBuilder.Append(" and RetInvNo ='" + txtInvoice.Text.Trim() + "'");
        stringBuilder.Append(" Order by RetDate desc");
        dt = obj.GetDataTableQry(stringBuilder.ToString());
        gvStockReturnList.DataSource = dt;
        gvStockReturnList.DataBind();
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

    protected void gvStockReturnList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvStockReturnList.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReturnStock.aspx");
    }
}