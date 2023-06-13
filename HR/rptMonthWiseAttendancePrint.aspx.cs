using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class HR_rptMonthWiseAttendancePrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Session["MonthwiseAttendance"] == null)
            return;
        lblDesc.Text = "Attendance Report For Month of (" + Session["Month"] + " - " + Session["Year"] + ")";
        lblReport.Text = Session["MonthwiseAttendance"].ToString().Trim();
    }
}