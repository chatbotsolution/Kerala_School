﻿using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptMlyFeeStatusPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["MlyFeeStatus"] == null)
            return;
        lblRpt.Text = Session["MlyFeeStatus"].ToString();
    }
}