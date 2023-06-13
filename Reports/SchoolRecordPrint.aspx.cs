using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_SchoolRecordPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!(Session["DetailsVerify"].ToString() != ""))
            return;
        lblRpt.Text = Session["DetailsVerify"].ToString();
    }
   
}