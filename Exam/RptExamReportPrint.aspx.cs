using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Exam_RptExamReportPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["printExamProgressReport"] == null)
            return;
        this.lblprint.Text = this.Session["printExamProgressReport"].ToString();
    }
}