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


public partial class HR_EmployeeHRAMaster : System.Web.UI.Page
{
    private clsDAL objDAL = new clsDAL();
    private Hashtable ht;
    private DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        getHRARate();
    }

    private void getHRARate()
    {
        DataTable dataTableQry = objDAL.GetDataTableQry("select HRAPer,FromDt from dbo.HR_EmpHRAMaster where ToDt is null");
        if (dataTableQry.Rows.Count > 0)
            lblCurrRate.Text = dataTableQry.Rows[0]["HRAPer"].ToString() + " % Effective From " + Convert.ToDateTime(dataTableQry.Rows[0]["FromDt"]).ToString("dd-MMM-yyyy");
        else
            lblCurrRate.Text = "Not Available";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        instValues();
    }

    private void instValues()
    {
        string empty = string.Empty;
        ht = new Hashtable();
        ht.Add("@FromDt", dtpDate.GetDateValue());
        if (txtHRARate.Text.Trim().Equals(string.Empty))
            return;
        ht.Add("@HRAPer", Convert.ToDouble(txtHRARate.Text.Trim()));
        ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
        string str = objDAL.ExecuteScalar("HR_InsEmpHRAMaster", ht);
        if (str.Trim().Equals(string.Empty))
        {
            getHRARate();
            clearFields();
            trMsg.Style["Background-Color"] = "Green";
            lblMsg.Text = "Data Saved Successfully";
        }
        else
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = str.ToString().Trim();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clearFields();
    }

    private void clearFields()
    {
        txtHRARate.Text = "0.00";
        txtDate.Text = string.Empty;
        trMsg.Style["Background-Color"] = "Transparent";
        lblMsg.Text = string.Empty;
    }
}