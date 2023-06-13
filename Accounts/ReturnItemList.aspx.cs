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

public partial class Accounts_ReturnItemList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Request.QueryString["PRId"] == null)
            return;
        this.lblInvoice.Text = this.Request.QueryString["RetInv"].ToString();
        this.BindGrid(int.Parse(this.Request.QueryString["PRId"].ToString()));
    }

    private void BindGrid(int PurchaseRetId)
    {
        this.ht.Clear();
        this.ht.Add((object)"@PurchaseRetId", (object)PurchaseRetId);
        this.dt = this.obj.GetDataTable("ACTS_ReturnItemList", this.ht);
        this.gvRetItemList.DataSource = (object)this.dt;
        this.gvRetItemList.DataBind();
    }
}