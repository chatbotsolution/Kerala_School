using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptStudStatusPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Session["StudStatus"] == null)
            return;
        lblReport.Text = Session["StudStatus"].ToString().Trim();
        lblDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
    }
   
}