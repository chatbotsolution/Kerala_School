﻿using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptRecieptChequePrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Session["PrintReciept"] == null)
            return;
        literaldata.Text = Session["PrintReciept"].ToString().Trim();
    }
}