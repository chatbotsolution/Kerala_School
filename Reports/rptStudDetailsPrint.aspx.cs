using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptStudDetailsPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["printData"] == null)
            return;
        else
            lblReport.Text = Session["printData"].ToString().Trim();
        if (Request.QueryString["Session"].ToString().Trim() != null && Request.QueryString["Class"].ToString().Trim() != null)
        {
            lblCls.Text = Request.QueryString["Class"].ToString().Trim();
            lblSes.Text = Request.QueryString["Session"].ToString().Trim();
            lblSec.Text= Request.QueryString["Sec"].ToString().Trim();
            lblDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
        else
        {
            lblCls.Text = "";
            lblSes.Text = "";
            lblSec.Text = "";
        }

    }
    
}