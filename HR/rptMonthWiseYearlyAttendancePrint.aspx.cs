using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_rptMonthWiseYearlyAttendancePrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Session["MonthwiseYrlyAttendance"] == null)
            return;
        lblDesc.Text = "Monthwise Attendance Report (" + Session["SessYr"] + ")";
        lblReport.Text = Session["MonthwiseYrlyAttendance"].ToString().Trim();
    }
}