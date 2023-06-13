using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_DefaulterBankCopyPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Session["BankCopy"] == null)
            return;
        lblprint.Text = Session["BankCopy"].ToString();
    }
   
}