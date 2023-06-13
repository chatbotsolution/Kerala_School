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

public partial class Accounts_ViewItemList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["SupplierId"] == null)
            return;
        lblSupplier.Text = Request.QueryString["Name"].ToString();
        BindGrid(int.Parse(Request.QueryString["SupplierId"].ToString()));
    }

    private void BindGrid(int Supplierid)
    {
        ht.Clear();
        ht.Add("@SupplierPartyId", Supplierid);
        dt = obj.GetDataTable("ACTS_SupplierItemList", ht);
        gvItemList.DataSource = dt;
        gvItemList.DataBind();
    }
}