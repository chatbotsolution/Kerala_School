using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptTemporaryIdCardPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
       // lblPrintDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        if (Session["TempICard"] == null)
            return;
        lblReport.Text = Session["TempICard"].ToString().Trim();
    }
}