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

public partial class HR_EmployeeEPFMaster : System.Web.UI.Page
{
    private clsDAL objDAL = new clsDAL();
    private Hashtable ht;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        getEPFRate();
    }

    private void getEPFRate()
    {
        DataTable dataTableQry = objDAL.GetDataTableQry("select EPFPer,EPFAllocationPer,FromDt from dbo.HR_EmpEPFMaster where ToDt is null");
        if (dataTableQry.Rows.Count > 0)
            lblCurrRate.Text = dataTableQry.Rows[0]["EPFPer"].ToString() + " % (Employee Share) and " + dataTableQry.Rows[0]["EPFAllocationPer"].ToString() + " % (Employer Share)</br> Effective From " + Convert.ToDateTime(dataTableQry.Rows[0]["FromDt"]).ToString("dd-MMM-yyyy");
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
        if (txtEPFEmployeeShare.Text.Trim().Equals(string.Empty))
            return;
        ht.Add("@EPFPer", Convert.ToDouble(txtEPFEmployeeShare.Text.Trim()));
        ht.Add("@EPFAllocationPer", Convert.ToDouble(txtEPFEmployerShare.Text.Trim()));
        ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
        string str = objDAL.ExecuteScalar("HR_InsEmpEPFMaster", ht);
        if (str.Trim().Equals(string.Empty))
        {
            getEPFRate();
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
        txtEPFEmployeeShare.Text = "0.00";
        txtEPFEmployerShare.Text = "0.00";
        txtDate.Text = string.Empty;
        trMsg.Style["Background-Color"] = "Transparent";
        lblMsg.Text = string.Empty;
    }
}