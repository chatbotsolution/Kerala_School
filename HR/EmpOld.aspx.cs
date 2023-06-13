using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;

public partial class HR_EmpOld : System.Web.UI.Page
{
    private clsDAL ObjCommon = new clsDAL();
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    protected string Focus = "";
    protected string ControlType = "";
    private DataTable dt;
    private Hashtable ht;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        setTeachNonTeachList();
        bindDrps();
        DRP.FillSessionYr(drpSessionYr);
        DataTable dt = new DataTable();
        dt.Columns.Add("QualId", typeof(int));
        dt.Columns.Add("QualName", typeof(string));
        dt.Columns.Add("BoardUnivName", typeof(string));
        dt.Columns.Add("Subjects", typeof(string));
        dt.Columns.Add("MarkPercent", typeof(double));
        dt.AcceptChanges();
        ViewState["EduQual"] = dt;
        if (Request.QueryString["empno"] != null)
            FillDataForEdit(dt);
        else
            setBtnDispToAddMoreInfo(false);
    }

    private void bindDrps()
    {
        bindSingleDrp(drpPermAddDistId, "SELECT ZilaId,ZilaName FROM dbo.ZilaMaster ORDER BY ZilaName", "ZilaName", "ZilaId");
        bindSingleDrp(drpPresAddDistId, "SELECT ZilaId,ZilaName FROM dbo.ZilaMaster ORDER BY ZilaName", "ZilaName", "ZilaId");
        bindSingleDrp(drpReligion, "select * from ps_studentreligion order by ReligionId", "Religion", "ReligionId");
        bindSingleDrp(drpCategory, "select * from PS_CategoryMaster order by CatName", "CatName", "CatID");
        bindSingleDrp(drpQual, "SELECT QualId, QualName FROM dbo.HR_QualMaster ORDER BY QualName", "QualName", "QualId");
        bindSingleDrp(drpEduQual, "SELECT QualId,QualName FROM dbo.HR_QualMaster ORDER BY QualName", "QualName", "QualId");
    }

    protected void optTeach_CheckedChanged(object sender, EventArgs e)
    {
        setTeachNonTeachList();
    }

    protected void optNonTeach_CheckedChanged(object sender, EventArgs e)
    {
        setTeachNonTeachList();
    }

    public void setTeachNonTeachList()
    {
        if (optTeach.Checked)
            bindSingleDrp(drpDesignation, "SELECT DesgId,Designation FROM dbo.HR_DesignationMaster WHERE TeachingStaff='T' ORDER BY Designation", "Designation", "DesgId");
        else
            bindSingleDrp(drpDesignation, "SELECT DesgId,Designation FROM dbo.HR_DesignationMaster WHERE TeachingStaff='N' ORDER BY Designation", "Designation", "DesgId");
    }

    private void setBtnDispToAddMoreInfo(bool st)
    {
    }

    private void fillAppEduQual(DataTable dataTable)
    {
        bindGvEduQual(dataTable);
        bindDrpEduQualOnCondition(dataTable);
        ViewState["EduQual"] = dataTable;
    }

    private void bindDrpEduQualOnCondition(DataTable dt)
    {
        string empty = string.Empty;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            if (!empty.Trim().Equals(string.Empty))
                empty += ",";
            empty += row["QualId"].ToString();
        }
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT QualId, QualName FROM dbo.HR_QualMaster");
        if (!empty.Trim().Equals(string.Empty))
            stringBuilder.Append(" WHERE QualId NOT IN(" + empty.Trim() + ")");
        stringBuilder.Append(" ORDER BY QualName");
        bindSingleDrp(drpQual, stringBuilder.ToString().Trim(), "QualName", "QualId");
    }

    private void bindGvEduQual(DataTable dataTable)
    {
        gvEduction.DataSource = dataTable;
        gvEduction.DataBind();
        hfEduQual.Value = gvEduction.Rows.Count.ToString();
    }

    private void FillDataForEdit(DataTable dt)
    {
        ObjCommon = new clsDAL();
        dt = new DataTable();
        ht = new Hashtable();
        if (Request.QueryString["empno"] == null)
            return;
        ht.Add("@EmpId", Request.QueryString["empno"]);
        dt = ObjCommon.GetDataTable("HR_SelOldEmployeeForEdit", ht);
        if (dt.Rows.Count <= 0)
            return;
        bindSingleDrp(drpPresAddDistId, "select ZilaId,ZilaName from dbo.ZilaMaster order by ZilaName", "ZilaName", "ZilaId");
        bindSingleDrp(drpPermAddDistId, "select ZilaId,ZilaName from dbo.ZilaMaster order by ZilaName", "ZilaName", "ZilaId");
        txtAppOrderNo.Text = dt.Rows[0]["AppointmentOrderNo"].ToString().Trim();
        drpSessionYr.SelectedValue = dt.Rows[0]["PenalYear"].ToString().Trim();
        txtSevabratiId.Text = dt.Rows[0]["SevabratiId"].ToString().Trim();
        if (dt.Rows[0]["DOJ"] != null)
        {
            dtpFirstJoinDt.SetDateValue(Convert.ToDateTime(dt.Rows[0]["DOJ"].ToString().Trim()));
            if (ObjCommon.ExecuteScalarQry("SELECT COUNT(*) FROM dbo.HR_EmpMlySalary WHERE EmpId=" + Request.QueryString["empno"]).Trim() != "0")
            {
                txtFirstJoinDt.Enabled = false;
                dtpFirstJoinDt.Enabled = false;
                ImageButton3.Visible = false;
            }
            else
            {
                txtFirstJoinDt.Enabled = true;
                dtpFirstJoinDt.Enabled = false;
                ImageButton3.Visible = true;
            }
        }
        if (dt.Rows[0]["TeachingStaff"].ToString().Trim() == "T")
        {
            optTeach.Checked = true;
            optTeach_CheckedChanged(drpDesignation, EventArgs.Empty);
        }
        else if (dt.Rows[0]["TeachingStaff"].ToString().Trim() == "N")
        {
            optNonTeach.Checked = true;
            optNonTeach_CheckedChanged(drpDesignation, EventArgs.Empty);
        }
        drpDesignation.SelectedValue = dt.Rows[0]["DesignationId"].ToString().Trim();
        drpAcharyaType.SelectedValue = dt.Rows[0]["AcharyaType"].ToString().Trim();
        txtSevabratiName.Text = dt.Rows[0]["SevName"].ToString();
        txtFathName.Text = dt.Rows[0]["FatherName"].ToString();
        txtSposeName.Text = dt.Rows[0]["SpouseName"].ToString();
        txtMothName.Text = dt.Rows[0]["MotherName"].ToString();
        DateTime result1;
        if (DateTime.TryParse(dt.Rows[0]["DOB"].ToString(), out result1))
            dtpDOB.SetDateValue(result1);
        txtPhoneNo.Text = dt.Rows[0]["Phone"].ToString();
        txtMobileNo.Text = dt.Rows[0]["Mobile"].ToString();
        txtEmail.Text = dt.Rows[0]["emailid"].ToString();
        if (!dt.Rows[0]["BloodGroup"].ToString().Trim().Equals(""))
            txtBloodGrp.Text = dt.Rows[0]["BloodGroup"].ToString();
        string str = dt.Rows[0]["Sex"].ToString();
        if (str == "Male")
            drpGender.SelectedValue = "M";
        else if (str == "Female")
            drpGender.SelectedValue = "F";
        drpReligion.SelectedValue = dt.Rows[0]["Religion"].ToString();
        drpCategory.SelectedValue = dt.Rows[0]["Category"].ToString();
        txtPermAddress.Text = dt.Rows[0]["PermAddress"].ToString();
        drpPermAddDistId.SelectedValue = dt.Rows[0]["PermAddrDistId"].ToString();
        txtPermPin.Text = dt.Rows[0]["PermAddrPin"].ToString();
        txtPresAddress.Text = dt.Rows[0]["PresAddress"].ToString();
        drpPresAddDistId.SelectedValue = dt.Rows[0]["PresAddrDistId"].ToString();
        txtPresPin.Text = dt.Rows[0]["PresAddrPin"].ToString();
        drpEduQual.SelectedValue = dt.Rows[0]["EduQual"].ToString();
        txtExtraQual.Text = dt.Rows[0]["ExtraQual"].ToString();
        txtEmpBankAcNo.Text = dt.Rows[0]["EmpBankAcNo"].ToString();
        txtBank.Text = dt.Rows[0]["BankName"].ToString();
        txtBranch.Text = dt.Rows[0]["Branch"].ToString();
        txtIFSC.Text = dt.Rows[0]["IFSCCode"].ToString();
        txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
        if (dt.Rows[0]["GSLI_Done"].ToString() == "Yes")
        {
            txtGSLIAmt.Text = dt.Rows[0]["GSLIDipAmt"].ToString();
            DateTime result2;
            if (DateTime.TryParse(dt.Rows[0]["GSLIDate"].ToString(), out result2))
                dtpGLSIDate.SetDateValue(result2);
        }
        else
        {
            optGLSIDoneN.Checked = true;
            optGLSIDoneY.Checked = false;
            optGLSIDoneY_CheckedChanged(optGLSIDoneY, EventArgs.Empty);
        }
        DateTime result3;
        if (DateTime.TryParse(dt.Rows[0]["TrainedDate"].ToString(), out result3))
            dtpTrainedDate.SetDateValue(result3);
        bool result4;
        if (bool.TryParse(dt.Rows[0]["TrainedTeacher"].ToString(), out result4))
        {
            optTrained.Checked = result4;
            optUntrained.Checked = !result4;
        }
        if (dt.Rows[0]["ActiveStatus"].ToString().ToUpper() == "FALSE")
            drpStatus.SelectedIndex = 1;
        else
            drpStatus.SelectedIndex = 0;
        txtEPFAcNo.Text = dt.Rows[0]["EPFAcNo"].ToString();
        DateTime result5;
        if (DateTime.TryParse(dt.Rows[0]["EPFAcDt"].ToString(), out result5))
            dtpEPFDate.SetDateValue(result5);
        drpMaritalStat.SelectedValue = dt.Rows[0]["MaritalStatus"].ToString();
        if (!dt.Rows[0]["ImageFileName"].ToString().Trim().Equals(string.Empty))
            imgEmployee.ImageUrl = "../Up_Files/EmpImages/" + dt.Rows[0]["ImageFileName"].ToString().Trim();
        if (!dt.Rows[0]["LeavingDate"].ToString().Trim().Equals(string.Empty))
            dtpLeavingDate.SetDateValue(DateTime.Parse(dt.Rows[0]["LeavingDate"].ToString()));
        ht = new Hashtable();
        dt = new DataTable();
        ObjCommon = new clsDAL();
        if (Request.QueryString["empno"] != null)
            ht.Add("@EmpId", Request.QueryString["empno"]);
        dt = ObjCommon.GetDataTable("HR_EmpQualDetail", ht);
        ViewState["EduQual"] = dt;
        gvEduction.DataSource = dt;
        gvEduction.DataBind();
        txtSevabratiName.Focus();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = InsertEmpInfo();
        if (dataTable2 == null || dataTable2.Rows.Count <= 0)
            return;
        int result;
        if (!int.TryParse(dataTable2.Rows[0][0].ToString(), out result))
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + dataTable2.Rows[0][0].ToString() + "');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Information Saved Successfully.');window.location='EmpOld.aspx'", true);
    }

    private DataTable InsertEmpInfo()
    {
        ht = new Hashtable();
        ObjCommon = new clsDAL();
        if (Request.QueryString["empno"] != null)
            ht.Add("EmpId", Convert.ToInt32(Request.QueryString["empno"]));
        if (!txtSevabratiId.Text.Trim().Equals(string.Empty))
            ht.Add("SevabratiId", txtSevabratiId.Text.Trim());
        ht.Add("SevName", txtSevabratiName.Text.Trim());
        ht.Add("FatherName", txtFathName.Text.Trim());
        ht.Add("SpouseName", txtSposeName.Text.Trim());
        ht.Add("MotherName", txtMothName.Text.Trim());
        ht.Add("PresAddress", txtPresAddress.Text.Trim());
        ht.Add("PresAddrDistId", drpPresAddDistId.SelectedValue.ToString().Trim());
        ht.Add("PresAddrPin", txtPresPin.Text.Trim());
        ht.Add("PermAddress", txtPermAddress.Text.Trim());
        ht.Add("PermAddrDistId", drpPermAddDistId.SelectedValue.ToString().Trim());
        ht.Add("PermAddrPin", txtPermPin.Text.Trim());
        ht.Add("Phone", txtPhoneNo.Text.Trim());
        ht.Add("Mobile", txtMobileNo.Text.Trim());
        if (txtBloodGrp.Text != "")
            ht.Add("BloodGroup", txtBloodGrp.Text.Trim());
        ht.Add("emailid", txtEmail.Text.Trim());
        ht.Add("Sex", drpGender.SelectedItem.Text);
        if (drpReligion.SelectedIndex > -1)
            ht.Add("Religion", drpReligion.SelectedValue.ToString());
        if (drpCategory.SelectedIndex > 0)
            ht.Add("Category", drpCategory.SelectedValue.ToString());
        if (drpDesignation.SelectedIndex > 0)
            ht.Add("DesignationId", drpDesignation.SelectedValue);
        ht.Add("EduQual", drpEduQual.SelectedValue.Trim());
        ht.Add("ExtraQual", txtExtraQual.Text.Trim());
        if (!txtDOB.Text.Trim().Equals(string.Empty))
        {
            try
            {
                if (Convert.ToDateTime(txtDOB.Text) > DateTime.Now.AddYears(-15))
                {
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Invalid Date Of Birth.');", true);
                    return (DataTable)null;
                }
                ht.Add("DOB", dtpDOB.GetDateValue());
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Invalid Date Of Birth.');", true);
                txtDOB.Focus();
                string message = ex.Message;
                return (DataTable)null;
            }
        }
        else
            ht.Add("DOB", "01/01/1900");
        if (!txtAppOrderNo.Text.Trim().Equals(string.Empty))
            ht.Add("AppointmentOrderNo", txtAppOrderNo.Text.Trim());
        if (!txtFirstJoinDt.Text.Trim().Equals(string.Empty))
        {
            try
            {
                ht.Add("DOJ", dtpFirstJoinDt.GetDateValue().ToString("dd MMM yyyy"));
                ht.Add("AppointmentDt", dtpFirstJoinDt.GetDateValue().ToString("dd MMM yyyy"));
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Invalid Joining Date.');", true);
                txtFirstJoinDt.Focus();
                string message = ex.Message;
                return (DataTable)null;
            }
        }
        else if (tblInsAppointments.Visible)
            ht.Add("DOJ", "01/01/1900");
        ht.Add("EmpBankAcNo", txtEmpBankAcNo.Text.Trim());
        ht.Add("BankName", txtBank.Text.Trim().ToString());
        ht.Add("Branch", txtBranch.Text.Trim());
        ht.Add("IFSCCode", txtIFSC.Text.Trim());
        if (drpAcharyaType.SelectedIndex > 0)
            ht.Add("AcharyaType", drpAcharyaType.SelectedValue.ToString());
        ht.Add("UserId", Session["User_Id"]);
        ht.Add("@TrainedTeacher", optTrained.Checked);
        DateTime result1;
        if (optTrained.Checked && DateTime.TryParse(txtTrainedDate.Text, out result1))
            ht.Add("@TrainedDate", dtpTrainedDate.GetDateValue());
        ht.Add("@MaritalStatus", drpMaritalStat.SelectedValue.ToString());
        ht.Add("@PrestEstId", Session["SSVMID"]);
        ht.Add("@PresentEstType", "SSVM");
        ht.Add("@EPFAcNo", txtEPFAcNo.Text.Trim());
        if (!txtEPFDate.Text.Trim().Equals(string.Empty))
            ht.Add("@EPFAcDt", dtpEPFDate.GetDateValue().ToString("dd MMM yyyy"));
        else
            ht.Add("@EPFAcDt", "01/01/1900");
        if (optGLSIDoneY.Checked)
            ht.Add("@GSLI_Done", "Yes");
        else
            ht.Add("@GSLI_Done", "No");
        if (optGLSIDoneY.Checked)
        {
            ht.Add("@GSLIDipAmt", txtGSLIAmt.Text.Trim() == string.Empty ? "0" : txtGSLIAmt.Text.Trim());
            ht.Add("@GSLIDate", (txtGLSIDate.Text.Trim() == string.Empty ? Convert.ToDateTime("01/01/1900") : dtpGLSIDate.GetDateValue()));
        }
        ht.Add("@Remarks", txtRemarks.Text.Trim());
        ht.Add("@ActiveStatus", drpStatus.SelectedIndex.ToString());
        if (drpStatus.SelectedIndex > 0 && txtLeavingDate.Text.Trim() != string.Empty)
            ht.Add("@LeavingDate", dtpLeavingDate.GetDateValue().ToString("dd MMM yyyy"));
        if (drpSessionYr.SelectedIndex > 0)
            ht.Add("@PenalYear", drpSessionYr.SelectedValue);
        if (fuEmpImage.HasFile)
        {
            string str = DateTime.Now.ToString("yyyyMMddhhmmssfffffff") + Path.GetExtension(fuEmpImage.FileName);
            ht.Add("@ImageFileName", str);
            fuEmpImage.SaveAs(Server.MapPath("../Up_Files/EmpImages/" + str));
        }
        dt = ObjCommon.GetDataTable("HR_InsUpdOldEmployeeMaster", ht);
        string str1 = string.Empty;
        string empty1 = string.Empty;
        int result2;
        if (int.TryParse(dt.Rows[0][0].ToString(), out result2))
        {
            ViewState["emp"] = result2;
            string empty2 = string.Empty;
            string str2 = instApplEduQual(result2);
            if (!str2.Trim().Equals(string.Empty))
                str1 = str2;
        }
        else
            str1 = empty1;
        if (str1.Trim().Equals(string.Empty))
            return dt;
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("Msg", typeof(string));
        dataTable.NewRow()["Msg"] = str1;
        dataTable.AcceptChanges();
        return dataTable;
    }

    private string instApplEduQual(int empid)
    {
        string str = string.Empty;
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = (DataTable)ViewState["EduQual"];
        try
        {
            char ch = 'U';
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("@Mode", ch);
                hashtable.Add("@EmpId", empid);
                hashtable.Add("@QualId", row["QualId"].ToString());
                hashtable.Add("@BoardUnivName", row["BoardUnivName"].ToString());
                hashtable.Add("@Subjects", row["Subjects"].ToString());
                hashtable.Add("@MarkPercent", row["MarkPercent"].ToString());
                if (!str.Trim().Equals(string.Empty))
                    str += ',';
                str += ObjCommon.ExecuteScalar("HR_InsUpdtEmpEduQual", hashtable);
                ch = 'I';
            }
            if (!str.Trim().Equals(string.Empty))
                str = "Qualifications : " + str.Trim() + " not be added to Applicant's Qualification List !";
            return str.Trim();
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmpOld.aspx");
    }

    private void ClearFields()
    {
        drpSessionYr.SelectedIndex = -1;
        txtSevabratiId.Text = string.Empty;
        txtSevabratiName.Text = string.Empty;
        txtFathName.Text = string.Empty;
        txtSposeName.Text = string.Empty;
        txtMothName.Text = string.Empty;
        txtDOB.Text = string.Empty;
        txtPhoneNo.Text = string.Empty;
        txtMobileNo.Text = string.Empty;
        txtMothName.Text = string.Empty;
        txtEmail.Text = string.Empty;
        drpReligion.SelectedIndex = 0;
        drpCategory.SelectedIndex = 0;
        txtPermAddress.Text = string.Empty;
        drpPermAddDistId.SelectedIndex = 0;
        txtPermPin.Text = string.Empty;
        txtPresAddress.Text = string.Empty;
        drpPresAddDistId.SelectedIndex = 0;
        txtPresPin.Text = string.Empty;
        drpDesignation.SelectedIndex = 0;
        drpEduQual.SelectedIndex = 0;
        txtExtraQual.Text = string.Empty;
        txtAppOrderNo.Text = string.Empty;
        txtEmpBankAcNo.Text = string.Empty;
        txtBank.Text = string.Empty;
        txtBranch.Text = string.Empty;
        txtIFSC.Text = string.Empty;
        drpAcharyaType.SelectedIndex = 0;
        txtFirstJoinDt.Enabled = true;
        dtpFirstJoinDt.Enabled = true; ;
        ImageButton3.Visible = true;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = InsertEmpInfo();
        if (dataTable2 == null || dataTable2.Rows.Count <= 0)
            return;
        int result;
        if (!int.TryParse(dataTable2.Rows[0][0].ToString(), out result))
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + dataTable2.Rows[0][0].ToString() + "');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Information Saved Successfully.');window.location='EmpListSSVM.aspx'", true);
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmpListSSVM.aspx");
    }

    private void bindSingleDrp(DropDownList drp, string query, string text, string value)
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = ObjCommon.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = text;
        drp.DataValueField = value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        ObjCommon = new clsDAL();
        DataTable dataTableQry = ObjCommon.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("Head Office", "0"));
    }

    protected void optTrained_CheckedChanged(object sender, EventArgs e)
    {
        txtTrainedDate.Text = string.Empty;
        txtTrainedDate.Enabled = optTrained.Checked;
        dtpTrainedDate.Enabled = optTrained.Checked;
        ScriptManager.RegisterStartupScript((Page)this, GetType(), "selectAndFocus", "$get('" + optTrained.ClientID + "').focus();$get('" + optTrained.ClientID + "').select();", true);
    }

    protected void optGLSIDoneY_CheckedChanged(object sender, EventArgs e)
    {
        txtGLSIDate.Text = txtGSLIAmt.Text = string.Empty;
        TextBox txtGlsiDate = txtGLSIDate;
        TextBox txtGsliAmt = txtGSLIAmt;
        bool flag1;
        dtpGLSIDate.Enabled = flag1 = optGLSIDoneY.Checked;
        int num1;
        bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
        txtGsliAmt.Enabled = num1 != 0;
        int num2 = flag2 ? 1 : 0;
        txtGlsiDate.Enabled = num2 != 0;
        optGLSIDoneY.Focus();
        ScriptManager.RegisterStartupScript((Page)this, GetType(), "selectAndFocus", "$get('" + optGLSIDoneY.ClientID + "').focus();$get('" + optGLSIDoneY.ClientID + "').select();", true);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        DataTable dt = addNewRowToDt();
        bindGvEduQual(dt);
        bindDrpEduQualOnCondition(dt);
        clearEduQual();
    }

    private void clearEduQual()
    {
        drpQual.SelectedIndex = -1;
        txtBoardUnivers.Text = string.Empty;
        txtSubject.Text = string.Empty;
        txtMarksPer.Text = string.Empty;
    }

    private DataTable addNewRowToDt()
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = (DataTable)ViewState["EduQual"];
        DataRow row = dataTable2.NewRow();
        row["QualId"] = drpQual.SelectedValue.ToString();
        row["QualName"] = drpQual.SelectedItem.Text.ToString();
        row["BoardUnivName"] = txtBoardUnivers.Text.Trim();
        row["Subjects"] = txtSubject.Text.Trim();
        row["MarkPercent"] = txtMarksPer.Text.Trim();
        dataTable2.Rows.Add(row);
        ViewState["EduQual"] = dataTable2;
        return dataTable2;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clearEduQual();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = (DataTable)ViewState["EduQual"];
            foreach (DataRow row in dataTable2.Select("QualId IN(" + Request["Checkb"].ToString() + ")"))
                dataTable2.Rows.Remove(row);
            dataTable2.AcceptChanges();
            ViewState["EduQual"] = dataTable2;
            bindGvEduQual(dataTable2);
            bindDrpEduQualOnCondition(dataTable2);
        }
    }

    protected void dtpLeavingDate_SelectionChanged(object sender, EventArgs e)
    {
        if (txtLeavingDate.Text != "")
            drpStatus.SelectedIndex = 1;
        else
            drpStatus.SelectedIndex = 1;
        ScriptManager.RegisterStartupScript((Page)this, GetType(), "selectAndFocus", "$get('" + txtLeavingDate.ClientID + "').focus();$get('" + txtLeavingDate.ClientID + "').select();", true);
    }

    protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpStatus.SelectedIndex == 1)
            dtpLeavingDate.SetDateValue(DateTime.Today);
        else
            txtLeavingDate.Text = string.Empty;
        ScriptManager.RegisterStartupScript((Page)this, GetType(), "selectAndFocus", "$get('" + drpStatus.ClientID + "').focus();$get('" + drpStatus.ClientID + "').select();", true);
    }

    protected void btnReassign_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "Call my function", "pop('pop1');", true);
    }

    protected void btnSaveShift_Click(object sender, EventArgs e)
    {
        string str = ObjCommon.ExecuteScalar("HR_InsertZilaMaster", new Hashtable()
        {
          {
             "@ZilaName",
             txtdistric.Text.Trim()
          },
          {
             "@UserId",
              Session["User_Id"].ToString()
          },
          {
             "@UserDate",
             DateTime.Now.ToString("dd MMM yyyy")
          }
        });
        if (str == "")
        {

            lblMsgTop.Text = "District Created Successfully";
            lblMsgTop.ForeColor = Color.Green;
            txtdistric.Text = "";
        }
        else
        {
            lblMsgTop.Text = str;
            lblMsgTop.ForeColor = Color.Red;
        }
        bindSingleDrp(drpPermAddDistId, "SELECT ZilaId,ZilaName FROM dbo.ZilaMaster ORDER BY ZilaName", "ZilaName", "ZilaId");
        bindSingleDrp(drpPresAddDistId, "SELECT ZilaId,ZilaName FROM dbo.ZilaMaster ORDER BY ZilaName", "ZilaName", "ZilaId");
    }
}