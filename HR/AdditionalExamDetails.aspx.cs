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

public partial class HR_AdditionalExamDetails : System.Web.UI.Page
{
    private clsDAL obj;
    private DataTable dt;
    private Hashtable ht;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
            Response.Redirect("../UserLogin.aspx");
        if (Page.IsPostBack)
            return;
        ViewState["emid"] = string.Empty;
        Session["title"] = Page.Title.ToString();
        if (Request.QueryString["examno"] != null)
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
        ht.Add("@EmpAddnlExamId", Request.QueryString["examno"]);
        dt = obj.GetDataTable("SelectEmpAddnlExams", ht);
        if (dt.Rows.Count > 0)
        {
            ViewState["emid"] = Convert.ToInt32(dt.Rows[0]["EmpId"]);
            if (!dt.Rows[0]["ExamDate"].ToString().Equals(string.Empty))
                dtpExamDate.SetDateValue(Convert.ToDateTime(dt.Rows[0]["ExamDate"]));
            txtExamDetails.Text = dt.Rows[0]["ExamDetails"].ToString();
            txtExamResult.Text = dt.Rows[0]["ExamResult"].ToString();
            obj = new clsDAL();
            lblEmp.Text = obj.ExecuteScalarQry("select SevName from dbo.EmployeeMaster where EmpId=" + ViewState["emid"].ToString()).ToUpper();
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "history.back();", true);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        if (InsertAddExamInfo().Rows.Count > 0)
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + dt.Rows[0][0].ToString() + "');", true);
        else if (Request.QueryString["em"] != null)
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Record Saved'); window.location='AdditionalExamDetails.aspx?em=" + Request.QueryString["em"].ToString() + "';", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Record Saved'); window.location='AdditionalExamDetails.aspx?em=" + ViewState["emid"].ToString() + "';", true);
    }

    private DataTable InsertAddExamInfo()
    {
        string text = txtExamDate.Text;
        dtpExamDate.GetDateValue();
        ht = new Hashtable();
        dt = new DataTable();
        obj = new clsDAL();
        if (Request.QueryString["examno"] != null)
            ht.Add("EmpAddnlExamId", Convert.ToInt32(Request.QueryString["examno"]));
        if (Request.QueryString["em"] != null)
            ht.Add("EmpId", Request.QueryString["em"]);
        else if (Request.QueryString["examno"] != null)
            ht.Add("EmpId", ViewState["emid"].ToString());
        if (!txtExamDate.Text.Trim().Equals(string.Empty))
        {
            if (dtpExamDate.GetDateValue() > DateTime.Now)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid Examination Date.');", true);
                return (DataTable)null;
            }
            ht.Add("ExamDate", dtpExamDate.GetDateValue());
        }
        else
            ht.Add("ExamDate", "01/01/1900");
        ht.Add("ExamDetails", txtExamDetails.Text.Trim());
        ht.Add("ExamResult", txtExamResult.Text.Trim());
        ht.Add("UserId", Session["UserId"]);
        ht.Add("SSVMID", Session["SSVMID"]);
        dt = obj.GetDataTable("InsUpdEmpAddnlExams", ht);
        return dt;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        if (InsertAddExamInfo().Rows.Count > 0)
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + dt.Rows[0][0].ToString() + "');", true);
        else if (Request.QueryString["em"] != null)
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Record Saved'); window.location='DetailedEmployeeInformation.aspx?empno=" + Request.QueryString["em"].ToString() + "&c=1';", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Record Saved'); window.location='DetailedEmployeeInformation.aspx?empno=" + ViewState["emid"].ToString() + "&c=1';", true);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["em"] != null)
            Response.Redirect("AdditionalExamDetails.aspx?em=" + Request.QueryString["em"].ToString());
        else
            Response.Redirect("AdditionalExamDetails.aspx?em=" + ViewState["emid"].ToString());
    }

    private void ClearFields()
    {
        txtExamDate.Text = string.Empty;
        txtExamDetails.Text = string.Empty;
        txtExamResult.Text = string.Empty;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["em"] != null)
            Response.Redirect("DetailedEmployeeInformation.aspx?empno=" + Request.QueryString["em"].ToString() + "&c=1", true);
        else
            Response.Redirect("DetailedEmployeeInformation.aspx?empno=" + ViewState["emid"].ToString() + "&c=1", true);
    }
}