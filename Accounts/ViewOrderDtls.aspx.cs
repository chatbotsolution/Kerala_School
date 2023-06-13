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

public partial class Accounts_ViewOrderDtls : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["POId"] == null)
            return;
        lblSupplier.Text = Request.QueryString["Party"].ToString();
        lblPODate.Text = Request.QueryString["POdt"].ToString();
        BindGrid(int.Parse(Request.QueryString["POId"].ToString()));
    }

    private void BindGrid(int POID)
    {
        ht.Clear();
        ht.Add("@PurchaseOrderId", POID);
        ht.Add("@NoOfChar", 8);
        dt = obj.GetDataTable("ACTS_PODetails", ht);
        gvOrderDtls.DataSource = dt;
        gvOrderDtls.DataBind();
    }
}