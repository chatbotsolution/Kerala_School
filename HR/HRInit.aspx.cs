using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_HRInit : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnLoanInit_Click(object sender, EventArgs e)
    {
        Response.Redirect("LoanInit.aspx");
    }

    protected void btnLoanMod_Click(object sender, EventArgs e)
    {
        Response.Redirect("LoanMod.aspx");
    }

    protected void btnSalaryInit_Click(object sender, EventArgs e)
    {
        Response.Redirect("SalaryInit.aspx");
    }

    protected void btnSalaryFin_Click(object sender, EventArgs e)
    {
        Response.Redirect("SalaryInitConfirm.aspx");
    }

    protected void btnLeave_Click(object sender, EventArgs e)
    {
        Response.Redirect("LeaveBalance.aspx");
    }
}