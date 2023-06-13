using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptAdmnSumPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        lblPrintDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        if (Session["AdmnSum"] == null)
            return;
        lblReport.Text = Session["AdmnSum"].ToString().Trim();
    }
}