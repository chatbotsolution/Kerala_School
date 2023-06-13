using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Exam_rptMeritListPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Session["title"] = (object)Page.Title.ToString();
        if (Session["MeritList"] == null)
            return;
        lblprint.Text = Session["MeritList"].ToString();
    }
}