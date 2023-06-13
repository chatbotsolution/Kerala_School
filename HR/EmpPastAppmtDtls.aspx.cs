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

public partial class HR_EmpPastAppmtDtls : System.Web.UI.Page
{
    private clsDAL DAL = new clsDAL();
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    private DataTable dt;
    private Hashtable ht;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        ViewState["emid"] = string.Empty;
        Session["title"] = Page.Title.ToString();
        bindAllDrps();
        if (Request.QueryString["Appointno"] != null)
        {
            FillData();
        }
        else
        {
            if (Request.QueryString["em"] == null)
                return;
            DAL = new clsDAL();
            DataTable dataTable = new DataTable();
            DataTable dataTableQry = DAL.GetDataTableQry("select SevName,AppointmentOrderNo,AppointmentDt,DOJ,DesignationId from dbo.HR_EmployeeMaster where EmpId=" + Request.QueryString["em"]);
            lblEmp.Text = dataTableQry.Rows[0]["SevName"].ToString().ToUpper();
            if (Request.QueryString["n"] == null)
                return;
            txtTransferOrdNo.Text = dataTableQry.Rows[0]["AppointmentOrderNo"].ToString();
            DateTime result1;
            if (DateTime.TryParse(dataTableQry.Rows[0]["AppointmentDt"].ToString(), out result1))
                dtpTransfer.DateValue = result1;
            DateTime result2;
            if (DateTime.TryParse(dataTableQry.Rows[0]["DOJ"].ToString(), out result2))
                dtpFrom.DateValue = result2;
            if (dataTableQry.Rows[0]["DesignationId"].ToString().Equals(string.Empty))
                return;
            drpDesignation.SelectedValue = dataTableQry.Rows[0]["DesignationId"].ToString();
        }
    }

    private void bindAllDrps()
    {
        bindSingleDrp(drpDesignation, "SELECT DesgId,Designation FROM dbo.HR_DesignationMaster ORDER BY Designation", "Designation", "DesgId");
    }

    private void bindSingleDrp(DropDownList drp, string query, string text, string value)
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = DAL.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = text;
        drp.DataValueField = value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DAL = new clsDAL();
        dt = new DataTable();
        dt = DAL.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void FillData()
    {
        DAL = new clsDAL();
        DataSet dataSet1 = new DataSet();
        ht = new Hashtable();
        ht.Add("@PastApptId", Request.QueryString["Appointno"]);
        DataSet dataSet2 = DAL.GetDataSet("HR_SelEmpPastAptmntsForEdit", ht);
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = new DataTable();
        DataTable table = dataSet2.Tables[0];
        ViewState["emid"] = Convert.ToInt32(table.Rows[0]["EmpId"].ToString());
        txtSchoolAddr.Text = table.Rows[0]["SchoolAddress"].ToString();
        txtSSVM.Text = table.Rows[0]["EstablishmentName"].ToString();
        drpDesignation.SelectedValue = table.Rows[0]["DesignationId"].ToString();
        if (!table.Rows[0]["FromDt"].ToString().Equals(string.Empty))
            dtpFrom.DateValue = Convert.ToDateTime(table.Rows[0]["FromDt"]);
        if (!table.Rows[0]["ToDt"].ToString().Equals(string.Empty))
            dtpToDt.DateValue = Convert.ToDateTime(table.Rows[0]["ToDt"]);
        txtTransferOrdNo.Text = table.Rows[0]["TransferOrderNo"].ToString();
        if (!table.Rows[0]["TransferDt"].ToString().Equals(string.Empty))
            dtpTransfer.DateValue = Convert.ToDateTime(table.Rows[0]["TransferDt"]);
        txtRemarks.Text = table.Rows[0]["Remarks"].ToString();
        lblEmp.Text = table.Rows[0]["SevName"].ToString().ToUpper();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = InsertPastAppointmentInfo();
        if (dataTable2 == null)
            return;
        if (dataTable2.Rows.Count > 0)
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + dt.Rows[0][0].ToString() + "');", true);
        else if (Request.QueryString["n"] != null)
        {
            if (Request.QueryString["n"] == "1")
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Data saved successfully !');window.location='EmpOld.aspx?empno=" + Request.QueryString["em"].ToString() + "&n=1';", true);
            else
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Data saved successfully !');window.location='EmpListSSVM.aspx?empno=" + Request.QueryString["em"].ToString() + "&n=2';", true);
        }
        else if (Request.QueryString["em"] != null)
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Data saved successfully !');window.location='EmpPastAppmtDtls.aspx?em=" + Request.QueryString["em"].ToString() + "&a=1';", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Data saved successfully !');window.location='EmpPastAppmtDtls.aspx?em=" + ViewState["emid"].ToString() + "&a=1';", true);
    }

    private DataTable InsertPastAppointmentInfo()
    {
        ht = new Hashtable();
        DAL = new clsDAL();
        dt = new DataTable();
        if (Request.QueryString["Appointno"] != null)
            ht.Add("PastApptId", Convert.ToInt32(Request.QueryString["Appointno"]));
        if (Request.QueryString["em"] != null)
            ht.Add("EmpId", Request.QueryString["em"]);
        if (Request.QueryString["Appointno"] != null)
            ht.Add("EmpId", ViewState["emid"].ToString());
        ht.Add("@SchoolNm", txtSSVM.Text.Trim());
        if (txtSchoolAddr.Text.Trim() != "")
            ht.Add("@SchoolAddr", txtSchoolAddr.Text.Trim());
        if (drpDesignation.SelectedIndex > 0)
            ht.Add("DesignationId", drpDesignation.SelectedValue.ToString());
        if (!txtFromDt.Text.Trim().Equals(string.Empty))
            ht.Add("FromDt", dtpFrom.GetDateValue());
        if (!txtToDt.Text.Trim().Equals(string.Empty))
            ht.Add("ToDt", dtpToDt.GetDateValue());
        if (!txtTransferOrdNo.Text.Trim().Equals(string.Empty))
            ht.Add("TransferOrderNo", txtTransferOrdNo.Text.Trim());
        if (!txtTransferDt.Text.Trim().Equals(string.Empty))
        {
            try
            {
                if (Convert.ToDateTime(txtTransferDt.Text) > DateTime.Now)
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid Transfer Date.');", true);
                    return (DataTable)null;
                }
                ht.Add("TransferDt", dtpTransfer.GetDateValue());
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Invalid Transfer Date.');", true);
                txtTransferDt.Focus();
                string message = ex.Message;
                return (DataTable)null;
            }
        }
        else
            ht.Add("TransferDt", "01/01/1900");
        ht.Add("@Remarks", txtRemarks.Text.Trim());
        ht.Add("UserId", Session["User_Id"]);
        dt = DAL.GetDataTable("HR_InsUpdEmpPastAppointments", ht);
        return dt;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    private void ClearFields()
    {
        drpDesignation.SelectedIndex = 0;
        txtFromDt.Text = string.Empty;
        txtToDt.Text = string.Empty;
        txtTransferOrdNo.Text = string.Empty;
        txtTransferDt.Text = string.Empty;
        txtRemarks.Text = string.Empty;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = InsertPastAppointmentInfo();
        if (dataTable2 == null)
            return;
        if (dataTable2.Rows.Count > 0)
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + dt.Rows[0][0].ToString() + "');", true);
        else if (Request.QueryString["em"] != null)
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Data saved successfully !');window.location='EmpInfoDtls.aspx?empno=" + Request.QueryString["em"] + "&a=1';", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Data saved successfully !');window.location='EmpInfoDtls.aspx?empno=" + ViewState["emid"].ToString() + "&a=1';", true);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["n"] != null)
            ScriptManager.RegisterClientScriptBlock((Control)btnBack, btnBack.GetType(), "ShowMessage", "alert('You haven't added any appointment details for " + lblEmp.Text.Trim() + ". Please provide appointment details !');", true);
        else if (Request.QueryString["em"] != null)
            Response.Redirect("EmpInfoDtls.aspx?empno=" + Request.QueryString["em"] + "&a=1", true);
        else
            Response.Redirect("EmpInfoDtls.aspx?empno=" + ViewState["emid"].ToString() + "&a=1", true);
    }
}