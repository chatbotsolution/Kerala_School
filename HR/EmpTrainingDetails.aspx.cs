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

public partial class HR_EmpTrainingDetails : System.Web.UI.Page
{
    private clsDAL obj;
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
        BindDrpQry(drpTrgName, "select TrgId,TrgName+' : '+CONVERT(VARCHAR,FromDt,106)+'-'+CONVERT(VARCHAR,ToDt,106) AS TrgName from dbo.HR_EmpTrgMaster where ActiveStatus=1 order by TrgName", "TrgName", "TrgId");
        if (Request.QueryString["trainingno"] != null)
        {
            FillData();
        }
        else
        {
            if (Request.QueryString["em"] == null)
                return;
            obj = new clsDAL();
            lblEmp.Text = obj.ExecuteScalarQry("select SevName from dbo.HR_EmployeeMaster where EmpId=" + Request.QueryString["em"]).ToUpper();
        }
    }

    private void BindDrpQry(DropDownList drp, string query, string text, string value)
    {
        DataTable dataTable = new DataTable();
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = text;
        drp.DataValueField = value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void FillData()
    {
        obj = new clsDAL();
        ht = new Hashtable();
        dt = new DataTable();
        ht.Add("@EmpTrgId", Request.QueryString["trainingno"]);
        dt = obj.GetDataTable("HR_SelEmpTrainings", ht);
        ViewState["emid"] = Convert.ToInt32(dt.Rows[0]["EmpId"].ToString());
        drpTrgName.SelectedValue = dt.Rows[0]["TrgId"].ToString();
        txtTrainingPlace.Text = dt.Rows[0]["TrgPlace"].ToString();
        if (!dt.Rows[0]["FromDt"].ToString().Trim().Equals(string.Empty))
            dtpFrom.SetDateValue(Convert.ToDateTime(dt.Rows[0]["FromDt"]));
        if (!dt.Rows[0]["ToDt"].ToString().Trim().Equals(string.Empty))
            dtpTo.SetDateValue(Convert.ToDateTime(dt.Rows[0]["ToDt"]));
        txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
        obj = new clsDAL();
        lblEmp.Text = obj.ExecuteScalarQry("select SevName from dbo.HR_EmployeeMaster where EmpId=" + ViewState["emid"].ToString()).ToUpper();
    }

    private DataTable InsertTrainingInfo()
    {
        ht = new Hashtable();
        dt = new DataTable();
        obj = new clsDAL();
        if (Request.QueryString["trainingno"] != null)
            ht.Add("EmpTrgId", Convert.ToInt32(Request.QueryString["trainingno"]));
        if (Request.QueryString["em"] != null)
            ht.Add("EmpId", Request.QueryString["em"]);
        if (Request.QueryString["trainingno"] != null)
            ht.Add("EmpId", ViewState["emid"].ToString());
        ht.Add("TrgId", drpTrgName.SelectedValue.ToString());
        ht.Add("TrgPlace", txtTrainingPlace.Text.Trim());
        ht.Add("FromDt", dtpFrom.GetDateValue());
        ht.Add("ToDt", dtpTo.GetDateValue());
        ht.Add("UserId", Session["User_Id"]);
        ht.Add("Remarks", txtRemarks.Text.Trim());
        dt = obj.GetDataTable("HR_InsUpdEmpTrainings", ht);
        return dt;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["em"] != null)
            Response.Redirect("EmpTrainingDetails.aspx?em=" + Request.QueryString["em"].ToString());
        else
            Response.Redirect("EmpTrainingDetails.aspx?em=" + ViewState["emid"].ToString());
    }

    private void ClearFields()
    {
        drpTrgName.SelectedIndex = -1;
        txtTrainingPlace.Text = string.Empty;
        txtFromDt.Text = string.Empty;
        txtToDt.Text = string.Empty;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!txtFromDt.Text.Trim().Equals(string.Empty))
        {
            try
            {
                if (Convert.ToDateTime(txtFromDt.Text) > DateTime.Now)
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid From Date.');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid From Date.');", true);
                txtFromDt.Focus();
                return;
            }
            if (!txtToDt.Text.Trim().Equals(string.Empty))
            {
                try
                {
                    Convert.ToDateTime(txtToDt.Text);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid To Date.');", true);
                    txtToDt.Focus();
                    string message = ex.Message;
                    return;
                }
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = InsertTrainingInfo();
                if (dataTable2 != null)
                {
                    if (dataTable2.Rows.Count > 0)
                        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + dt.Rows[0][0].ToString() + "');", true);
                    else if (Request.QueryString["em"] != null)
                        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Record Saved'); window.location='EmpTrainingDetails.aspx?em=" + Request.QueryString["em"].ToString() + "';", true);
                    else
                        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Record Saved'); window.location='EmpTrainingDetails.aspx?em=" + ViewState["emid"].ToString() + "';", true);
                }
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Data save failed, Please try again !');", true);
            }
            else
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Select Training End Date');", true);
        }
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Select Training Start Date');", true);
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (!txtFromDt.Text.Trim().Equals(string.Empty))
        {
            try
            {
                if (Convert.ToDateTime(txtFromDt.Text) > DateTime.Now)
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid From Date.');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid From Date.');", true);
                txtFromDt.Focus();
                return;
            }
            if (!txtToDt.Text.Trim().Equals(string.Empty))
            {
                try
                {
                    Convert.ToDateTime(txtToDt.Text);
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid To Date.');", true);
                    txtToDt.Focus();
                    string message = ex.Message;
                    return;
                }
                DataTable dataTable = new DataTable();
                if (InsertTrainingInfo().Rows.Count > 0)
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + dt.Rows[0][0].ToString() + "');", true);
                else if (Request.QueryString["em"] != null)
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Record Saved'); window.location='EmpInfoDtls.aspx?empno=" + Request.QueryString["em"] + "&d=1';", true);
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Record Saved'); window.location='EmpInfoDtls.aspx?empno=" + ViewState["emid"].ToString() + "&d=1';", true);
            }
            else
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Select Training Start Date');", true);
        }
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Select Training Start Date');", true);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["em"] != null)
            Response.Redirect("EmpInfoDtls.aspx?empno=" + Request.QueryString["em"] + "&d=1");
        else
            Response.Redirect("EmpInfoDtls.aspx?empno=" + ViewState["emid"].ToString() + "&d=1");
    }

    protected void btnAddTrng_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["em"] != null)
            Response.Redirect("EmpTrngMaster.aspx?empno=" + Request.QueryString["em"].ToString());
        else if (Request.QueryString["trainingno"] != null)
            Response.Redirect("EmpTrngMaster.aspx?trngNo=" + Request.QueryString["trainingno"].ToString());
        else
            Response.Redirect("EmpTrngMaster.aspx");
    }
}