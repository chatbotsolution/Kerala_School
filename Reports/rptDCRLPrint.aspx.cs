﻿using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptDCRLPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        lblPrintDate.Text = Session["DCRDT"].ToString().Trim();
        if (Session["DCRL"] == null)
            return;
        lblReport.Text = Session["DCRL"].ToString().Trim();
    }
}