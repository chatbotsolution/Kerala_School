﻿using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptRecieptChequeAdditionPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        lblPrintDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        if (Session["PrintAdditionalReciept"] == null)
            return;
        lblReport.Text = Session["PrintAdditionalReciept"].ToString().Trim();
    }
}