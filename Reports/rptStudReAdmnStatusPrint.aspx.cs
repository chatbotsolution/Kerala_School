using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptStudReAdmnStatusPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        lblPrintDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        if (Session["ReadmnStatus"] == null)
            return;
        lblReport.Text = Session["ReadmnStatus"].ToString().Trim();
    }
    //protected void btnPrev_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("rptStudReAdmnStatus.aspx");
    //}
}