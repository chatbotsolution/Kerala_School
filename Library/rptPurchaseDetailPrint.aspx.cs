using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Library_rptPurchaseDetailPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack ||Session["rptPurchaseDetail"] == null)
            return;
       lblPrintDate.Text = DateTime.Now.ToString("dd MMM yyyy  H:mm:ss");
       lblReport.Text =Session["rptPurchaseDetail"].ToString().Trim();
    }
}