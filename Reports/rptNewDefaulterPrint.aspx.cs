using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptNewDefaulterPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        lblPrintDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        if (Session["NewDefaulter"] == null)
            return;
        lblReport.Text = Session["NewDefaulter"].ToString().Trim();
    }
}