using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_MiscRecovery : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!ChkIsHRUsed())
                Response.Redirect("HRHome.aspx?IsHRUsed=0");
            else
                bindEmployee();
        }
        lblMsg.Text = string.Empty;
        trMsg.BgColor = "Transparent";
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void bindEmployee()
    {
        drpEmpName.DataSource = obj.GetDataTable("HR_GetEmpList");
        drpEmpName.DataTextField = "Employee";
        drpEmpName.DataValueField = "EmpId";
        drpEmpName.DataBind();
        drpEmpName.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string empty = string.Empty;
        try
        {
            if (rbRecMode.SelectedIndex == 1 && Convert.ToDecimal(txtAmount.Text) > Convert.ToDecimal(hfBasic.Value))
            {
                lblMsg.Text = "Amount to recover shouldn't be more than Basic Salary of the Employee.";
                trMsg.BgColor = "Red";
                txtAmount.Focus();
            }
            else if (rbRecMode.SelectedIndex == 0 && txtPmtDate.Text.Trim() == string.Empty)
            {
                lblMsg.Text = "Payment Date is required for Direct Cash Payment.";
                trMsg.BgColor = "Red";
                txtPmtDate.Focus();
            }
            else
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("@EmpId", drpEmpName.SelectedValue);
                hashtable.Add("@RecAmt", Convert.ToDecimal(txtAmount.Text));
                hashtable.Add("@RecReason", txtReason.Text.Trim());
                hashtable.Add("@RecType", rbRecType.SelectedValue);
                hashtable.Add("@RecMode", rbRecMode.SelectedValue);
                hashtable.Add("@UserId", Session["User_Id"]);
                if (rbRecMode.SelectedIndex == 0)
                {
                    hashtable.Add("@SchoolId", Session["SchoolId"]);
                    hashtable.Add("@TransDate", dtpPmtDate.GetDateValue().ToString("dd-MMM-yyyy"));
                }
                if (obj.ExecuteScalar("HR_InsMiscRecovery", hashtable).Trim() == string.Empty)
                {
                    lblMsg.Text = "Data Saved Successfully";
                    trMsg.BgColor = "Green";
                    ClearFields();
                }
                else
                {
                    lblMsg.Text = "Failed to Save. Try Again.";
                    trMsg.BgColor = "Red";
                }
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Failed to Save. Try Again.";
            trMsg.BgColor = "Red";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    private void ClearFields()
    {
        drpEmpName.SelectedIndex = 0;
        rbRecType.Text = "0";
        rbRecMode.SelectedIndex = 0;
        txtAmount.Text = "0";
        txtReason.Text = string.Empty;
        lblPending.Visible = false;
        lblBasic.Text = string.Empty;
        rbRecType.SelectedIndex = 0;
        rbRecMode.SelectedIndex = 0;
        txtPmtDate.Text = string.Empty;
        txtPmtDate.Enabled = true;
        dtpPmtDate.Enabled = true;
        hfBasic.Value = "0";
        drpEmpName.Focus();
    }

    protected void drpEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblBasic.Text = string.Empty;
        hfBasic.Value = "0";
        if (drpEmpName.SelectedIndex > 0)
        {
            string str = obj.ExecuteScalarQry("SELECT ISNULL(SUM(RecAmt),0) FROM dbo.HR_MiscRecovery WHERE EmpId=" + drpEmpName.SelectedValue + " AND DedId IS NULL AND RecStatus IS NULL");
            if (str.Trim() != string.Empty && Convert.ToDecimal(str) > new Decimal(0))
            {
                lblPending.Text = "Pending Amount to be Recovered : " + str;
                lblPending.ForeColor = Color.Red;
                lblPending.Visible = true;
                btnSave.Enabled = false;
            }
            else
            {
                btnSave.Enabled = true;
                lblPending.Visible = false;
            }
            GetBasic();
        }
        else
        {
            lblBasic.Text = string.Empty;
            hfBasic.Value = "0";
            lblPending.Visible = false;
        }
        drpEmpName.Focus();
    }

    private void GetBasic()
    {
        string str = obj.ExecuteScalarQry("SELECT ISNULL(Pay,0) FROM dbo.HR_EmpSalStructure WHERE EmpId=" + drpEmpName.SelectedValue + " AND ToDt IS NULL");
        if (str.Trim() == string.Empty)
            str = "0";
        hfBasic.Value = str;
        lblBasic.Text = "Basic Salary is : " + Convert.ToDecimal(str).ToString("0.00");
    }

    protected void rbRecMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtPmtDate.Text = string.Empty;
        if (rbRecMode.SelectedIndex == 0)
        {
            txtPmtDate.Enabled = true;
            dtpPmtDate.Enabled = true;
        }
        else
        {
            txtPmtDate.Enabled = false;
            dtpPmtDate.Enabled = false;
        }
        rbRecMode.Focus();
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("MiscRecoveryList.aspx");
    }
}