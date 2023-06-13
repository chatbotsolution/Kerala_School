using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_EmpSalaryDetails : System.Web.UI.Page
{
    private clsDAL obj;
    private DataTable dt;
    private Hashtable ht;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SSVMID"] == null)
            Response.Redirect("../UserLogin.aspx");
        textAlignInTextBox();
        if (Page.IsPostBack)
            return;
        ViewState["emid"] = string.Empty;
        Session["title"] = Page.Title.ToString();
        if (Request.QueryString["salary"] != null)
        {
            FillData();
        }
        else
        {
            if (Request.QueryString["em"] == null)
                return;
            obj = new clsDAL();
            lblEmp.Text = obj.ExecuteScalarQry("select SevName from dbo.EmployeeMaster where EmpId=" + Request.QueryString["em"]).ToUpper();
        }
    }

    private void FillData()
    {
        obj = new clsDAL();
        dt = new DataTable();
        ht = new Hashtable();
        ht.Add("@EmpSalId", Request.QueryString["salary"]);
        dt = obj.GetDataTable("SelectEmpSalaryStruct", ht);
        ViewState["emid"] = Convert.ToInt32(dt.Rows[0]["EmpId"].ToString());
        if (!dt.Rows[0]["FromDtate"].ToString().Equals(string.Empty))
            dtpFrom.SetDateValue(Convert.ToDateTime(dt.Rows[0]["FromDtate"]));
        txtBasic.Text = dt.Rows[0]["Basic"].ToString();
        txtDA.Text = dt.Rows[0]["DA"].ToString();
        txtAllowance.Text = dt.Rows[0]["Allow"].ToString();
        txtSplAllowance.Text = dt.Rows[0]["SplAllow"].ToString();
        txtHR.Text = dt.Rows[0]["HR"].ToString();
        txtOther.Text = dt.Rows[0]["Other"].ToString();
        txtEPF.Text = dt.Rows[0]["EPFDed"].ToString();
        txtGSLI.Text = dt.Rows[0]["GSLIDed"].ToString();
        txtInsurance.Text = dt.Rows[0]["Insurance"].ToString();
        txtAnyAdv.Text = dt.Rows[0]["AdvDed"].ToString();
        obj = new clsDAL();
        lblEmp.Text = obj.ExecuteScalarQry("select SevName from dbo.EmployeeMaster where EmpId=" + ViewState["emid"].ToString()).ToUpper();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        if (InsertSalaryInfo().Rows.Count > 0)
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + dt.Rows[0][0].ToString() + "');", true);
        else if (Request.QueryString["em"] != null)
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Record Saved'); window.location='SalaryDetails.aspx?em=" + Request.QueryString["em"].ToString() + "';", true);
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Record Saved'); window.location='SalaryDetails.aspx?em=" + ViewState["emid"].ToString() + "';", true);
    }

    private DataTable InsertSalaryInfo()
    {
        ht = new Hashtable();
        obj = new clsDAL();
        dt = new DataTable();
        if (txtInsurance.Text.Trim().Equals(string.Empty))
            txtInsurance.Text = "0";
        if (txtAllowance.Text.Trim().Equals(string.Empty))
            txtAllowance.Text = "0";
        if (txtAnyAdv.Text.Trim().Equals(string.Empty))
            txtAnyAdv.Text = "0";
        if (txtBasic.Text.Trim().Equals(string.Empty))
            txtBasic.Text = "0";
        if (txtDA.Text.Trim().Equals(string.Empty))
            txtDA.Text = "0";
        if (txtEPF.Text.Trim().Equals(string.Empty))
            txtEPF.Text = "0";
        if (txtGSLI.Text.Trim().Equals(string.Empty))
            txtGSLI.Text = "0";
        if (txtHR.Text.Trim().Equals(string.Empty))
            txtHR.Text = "0";
        if (txtOther.Text.Trim().Equals(string.Empty))
            txtOther.Text = "0";
        if (txtSplAllowance.Text.Trim().Equals(string.Empty))
            txtSplAllowance.Text = "0";
        double num1 = Convert.ToDouble(txtAllowance.Text.Trim()) + Convert.ToDouble(txtBasic.Text.Trim()) + Convert.ToDouble(txtDA.Text.Trim()) + Convert.ToDouble(txtHR.Text.Trim()) + Convert.ToDouble(txtOther.Text.Trim()) + Convert.ToDouble(txtSplAllowance.Text.Trim());
        double num2 = num1 - (Convert.ToDouble(txtAnyAdv.Text.Trim()) + Convert.ToDouble(txtEPF.Text.Trim()) + Convert.ToDouble(txtGSLI.Text.Trim()) + Convert.ToDouble(txtInsurance.Text.Trim()));
        if (Request.QueryString["salary"] != null)
            ht.Add("EmpSalId", Convert.ToInt32(Request.QueryString["salary"]));
        if (Request.QueryString["em"] != null)
            ht.Add("EmpId", Request.QueryString["em"]);
        if (Request.QueryString["salary"] != null)
            ht.Add("EmpId", ViewState["emid"].ToString());
        if (!txtFromDt.Text.Trim().Equals(string.Empty))
            ht.Add("FromDtate", dtpFrom.GetDateValue());
        else
            ht.Add("FromDtate", "01/01/1900");
        ht.Add("Basic", txtBasic.Text.Trim());
        ht.Add("DA", txtDA.Text.Trim());
        ht.Add("Allow", txtAllowance.Text.Trim());
        ht.Add("SplAllow", txtSplAllowance.Text.Trim());
        ht.Add("HR", txtHR.Text.Trim());
        ht.Add("Other", txtOther.Text.Trim());
        ht.Add("Gross", num1);
        ht.Add("EPFDed", txtEPF.Text.Trim());
        ht.Add("GSLIDed", txtGSLI.Text.Trim());
        ht.Add("Insurance", txtInsurance.Text.Trim());
        ht.Add("AdvDed", txtAnyAdv.Text.Trim());
        ht.Add("NetPaybleAmt", num2);
        ht.Add("UserId", Session["UserId"]);
        dt = obj.GetDataTable("InsUpdEmpSalaryStruct", ht);
        return dt;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["em"] != null)
            Response.Redirect("SalaryDetails.aspx?em=" + Request.QueryString["em"].ToString());
        else
            Response.Redirect("SalaryDetails.aspx?em=" + ViewState["emid"].ToString());
    }

    private void ClearFields()
    {
        txtFromDt.Text = string.Empty;
        txtBasic.Text = string.Empty;
        txtDA.Text = string.Empty;
        txtAllowance.Text = string.Empty;
        txtSplAllowance.Text = string.Empty;
        txtHR.Text = string.Empty;
        txtOther.Text = string.Empty;
        txtEPF.Text = string.Empty;
        txtGSLI.Text = string.Empty;
        txtInsurance.Text = string.Empty;
        txtAnyAdv.Text = string.Empty;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        if (InsertSalaryInfo().Rows.Count > 0)
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + dt.Rows[0][0].ToString() + "');", true);
        else if (Request.QueryString["em"] != null)
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Record Saved'); window.location='DetailedEmployeeInformation.aspx?empno=" + Request.QueryString["em"] + "&e=1';", true);
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Record Saved'); window.location='DetailedEmployeeInformation.aspx?empno=" + ViewState["emid"].ToString() + "&e=1';", true);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["em"] != null)
            Response.Redirect("DetailedEmployeeInformation.aspx?empno=" + Request.QueryString["em"] + "&e=1");
        else
            Response.Redirect("DetailedEmployeeInformation.aspx?empno=" + ViewState["emid"].ToString() + "&e=1");
    }

    private void textAlignInTextBox()
    {
        txtBasic.Style["text-align"] = "right";
        txtDA.Style["text-align"] = "right";
        txtAllowance.Style["text-align"] = "right";
        txtAnyAdv.Style["text-align"] = "right";
        txtEPF.Style["text-align"] = "right";
        txtGSLI.Style["text-align"] = "right";
        txtHR.Style["text-align"] = "right";
        txtInsurance.Style["text-align"] = "right";
        txtOther.Style["text-align"] = "right";
        txtSplAllowance.Style["text-align"] = "right";
    }
}