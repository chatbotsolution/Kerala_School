using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Accounts_rptProfitLossPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Session["PrntPrftLoss"] == null)
            return;
        lblReport.Text = Session["PrntPrftLoss"].ToString().Trim();
    }
}