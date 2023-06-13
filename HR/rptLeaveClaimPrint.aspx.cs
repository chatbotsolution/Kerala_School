using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_rptLeaveClaimPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../AdminLogin.aspx");
        if (Page.IsPostBack || Session["LeaveClaimReport"] == null)
            return;
        lblReport.Text = Session["LeaveClaimReport"].ToString();
    }
}