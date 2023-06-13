using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Attendance_rptStudAttendanceClasswisePrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Session["StudentwiseAttendance"] == null)
            return;
        lblDesc.Text = "Attendance Report of " + Session["Student"];
        lblReport.Text = Session["StudentwiseAttendance"].ToString().Trim();
    }
}