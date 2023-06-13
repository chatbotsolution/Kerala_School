using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class HR_EmployeeDAMaster : System.Web.UI.Page
{
    private clsDAL objDAL = new clsDAL();
    private Hashtable ht;
    private DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        getDARate();
    }

    private void getDARate()
    {
        DataTable dataTableQry = objDAL.GetDataTableQry("select DAPer,FromDt from dbo.HR_EmpDAMaster where ToDt is null");
        if (dataTableQry.Rows.Count > 0)
            lblCurrDARate.Text = dataTableQry.Rows[0]["DAPer"].ToString() + " % Effective with Date " + Convert.ToDateTime(dataTableQry.Rows[0]["FromDt"]).ToString("dd-MMM-yyyy");
        else
            lblCurrDARate.Text = "Not Available";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        instValues();
    }

    private void instValues()
    {
        string empty = string.Empty;
        ht = new Hashtable();
        ht.Add("@FromDt", dtpDate.GetDateValue().ToString("dd-MMM-yyyy"));
        if (txtDARate.Text.Trim().Equals(string.Empty))
            return;
        ht.Add("@DAPer", Convert.ToDouble(txtDARate.Text.Trim()));
        ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
        string str = objDAL.ExecuteScalar("HR_InsEmpDAMaster", ht);
        if (str.Trim().ToString().Trim().ToUpper() == "F")
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = "Failed to Save. Try Again.";
        }
        else
        {
            getDARate();
            clearFields();
            trMsg.Style["Background-Color"] = "Green";
            lblMsg.Text = str.Trim();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clearFields();
    }

    private void clearFields()
    {
        txtDARate.Text = "0.00";
        txtDate.Text = string.Empty;
        trMsg.Style["Background-Color"] = "Transparent";
        lblMsg.Text = string.Empty;
    }
}