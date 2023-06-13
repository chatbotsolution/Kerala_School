using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_ViewSaleReturnList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Invoice"] == null)
            return;
        lblInvoice.Text = Request.QueryString["Invoice"].ToString();
        BindGrid(int.Parse(Request.QueryString["PId"].ToString()));
    }

    private void BindGrid(int PId)
    {
        ht.Clear();
        ht.Add("@PurchaseId", PId);
        dt = obj.GetDataTable("ACTS_RcvStockDtls", ht);
        gvStockList.DataSource = dt;
        gvStockList.DataBind();
    }
}