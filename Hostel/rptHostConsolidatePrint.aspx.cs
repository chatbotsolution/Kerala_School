using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Hostel_rptHostConsolidatePrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Session["HostConsolidatedReciept"] == null)
            return;
        lblReport.Text = Session["HostConsolidatedReciept"].ToString().Trim();
    }
}