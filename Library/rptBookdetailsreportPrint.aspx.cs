using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_rptBookdetailsreportPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        //Session["title"] = (object)Page.Title.ToString();
        if (Session["bookdetailsReport"] == null)
            return;
        lblprint.Text = Session["bookdetailsReport"].ToString();
    }
}