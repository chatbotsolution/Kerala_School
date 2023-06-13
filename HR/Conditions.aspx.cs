using ASP;
using SanLib;
using System;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_Conditions : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;
        updateValue();
    }

    private void updateValue()
    {
        string str = obj.ExecuteScalarQry("select EW_Days_Count from dbo.HR_Conditions");
        if (!(str.Trim() != ""))
            return;
        txtEL.Text = str.Trim();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        obj = new clsDAL();
        if (txtEL.Text != "")
        {
            if (!(obj.ExecuteScalarQry("update dbo.HR_Conditions set EW_Days_Count=" + txtEL.Text.Trim()).Trim() == ""))
                return;
            lblmsg.Text = "Record Saved Successfully!";
            lblmsg.ForeColor = Color.Green;
            updateValue();
        }
        else
        {
            lblmsg.Text = "Please Enter No Of Days!";
            lblmsg.ForeColor = Color.Red;
            txtEL.Focus();
        }
    }
}