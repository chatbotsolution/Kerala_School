using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Exam_SubjectTopMarksPrint : System.Web.UI.Page
{
   protected void Page_Load(object sender, EventArgs e)
   {
    if (Page.IsPostBack)
      return;
    Session["title"] = (object) Page.Title.ToString();
    lblPrintDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
    if (Session["printData"] == null)
      return;
    lblReport.Text = Session["printData"].ToString().Trim();
   }
}