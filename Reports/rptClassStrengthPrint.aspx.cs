using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptClassStrengthPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Session["ClassStrength"] == null)
            return;
        lblReport.Text = Session["ClassStrength"].ToString().Trim();
        lblDate.Text = DateTime.Now.ToString("dd-MM-yyyy");
    }
    //protected void btnPrev_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("rptClassStrength.aspx");
    //}
}