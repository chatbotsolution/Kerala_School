﻿using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Hostel_HostFeeDueNotificationPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Session["printDue"] == null)
            return;
        lblprint.Text = Session["printDue"].ToString();
    }
}