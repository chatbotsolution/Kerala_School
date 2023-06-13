using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_ItemSaleInvoicePrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page.IsPostBack || this.Session["PrintInvoice"] == null)
            return;
        this.lblReport.Text = this.Session["PrintInvoice"].ToString().Trim();
    }
}