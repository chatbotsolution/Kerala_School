using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_HRHome : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["IsHrUsed"] == null || !(Request.QueryString["IsHrUsed"].ToString() == "0"))
            return;
        lblMsg.Text = "You have already Paid Salary, Loan/Advance without using the HR & Payroll module. So, some features of HR & Payroll module will not work in this financial year. You will be able to use complete HR & Payroll module from next financial year.";
    }
}