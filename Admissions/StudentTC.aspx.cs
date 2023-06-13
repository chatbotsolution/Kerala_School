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
using System.Web.UI.WebControls;

public partial class Admissions_StudentTC : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        fillsession();
        fillclass();
        if (Request.QueryString["AdmnNo"] != null)
        {
            txtAdmnno.Text = Request.QueryString["AdmnNo"].ToString();
            StudSrch();
        }
        if (!(Session["userrights"].ToString().Trim() == "a"))
            return;
        txtStudName.Enabled = true;
        txtFAtherNm.Enabled = true;
        txtMotherNm.Enabled = true;
        txtDOB.Enabled = true;
        txtAdmnDt.Enabled = true;
        txtCurrCls.Enabled = true;
        dtpIssdt.SetDateValue(DateTime.Now);
        //txtAddr.Enabled = true;
        //txtPS.Enabled = true;
        //txtDist.Enabled = true;
        //txtPin.Enabled = true;
       // txtSubStudies.Text = "English";
    }

    protected void fillsession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        drpClass.Items.Clear();
        drpClass.DataSource = new clsDAL().GetDataTableQry("SELECT ClassID,ClassName FROM dbo.PS_ClassMaster where ClassID <=13");
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassID";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearAll();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSection();
        drpSection_SelectedIndexChanged(new object(), new EventArgs());
    }

    private void FillSection()
    {
        drpSection.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "classId",
         drpClass.SelectedValue
      },
      {
         "session",
         drpSession.SelectedValue
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("ALL", "0"));
    }
    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnTcForm.Enabled = false;
        pnlStud.Visible = false;
        StringBuilder stringBuilder = new StringBuilder();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue.ToString().Trim());
            hashtable.Add("@Class", drpClass.SelectedValue.ToString().Trim());
            hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
            DataTable dataTable2 = obj.GetDataTable("Ps_Sp_GetStudsTC", hashtable);
            try
            {
                drpStudent.DataSource = dataTable2;
                drpStudent.DataTextField = "fullname";
                drpStudent.DataValueField = "admnno";
                drpStudent.DataBind();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            drpStudent.Items.Insert(0, new ListItem("-Select-", "0"));
        //}
        //else
        //    ClearAll();
    }

    protected void drpStudent_SelectedIndexChanged(object sender, EventArgs e)
    {

        DataTable dt = new clsDAL().GetDataTableQry("select Admnno from Ps_StudMaster where Admnno =" + drpStudent.SelectedValue.ToString().Trim() + "");
        txtAdmnno.Text = dt.Rows[0]["Admnno"].ToString().Trim();
        
        if (drpStudent.SelectedIndex > 0)
        {
            btnTcForm.Enabled = true;
        }
        else
        {
            btnTcForm.Enabled = false;
            Clear();
        }
    }

    private void ClearAll()
    {
        drpClass.SelectedIndex = 0;
       // drpSection.SelectedIndex = 0;
        drpStudent.Items.Clear();
        drpStudent.Items.Insert(0, new ListItem("-Select-", "0"));
        txtAdmnno.Text = string.Empty;
        btnTcForm.Enabled = false;
        pnlStud.Visible = false;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        StudSrch();
        GenerateForm();
    }

    private void StudSrch()
    {

        //DataTable dt = new clsDAL().GetDataTableQry("select Admnno from Ps_StudMaster where OldAdmnNo ='" + txtAdmnno.Text.Trim() + "'");
        //Int64 admno = Convert.ToInt64(dt.Rows[0]["AdmnNo"].ToString().Trim());
        try
        {
            Int64 admno = Convert.ToInt64(txtAdmnno.Text.Trim());
            DataTable dataTableQry = obj.GetDataTableQry("select SessionYear,ClassID,Section from PS_ClasswiseStudent cs inner join PS_StudMaster sm on sm.AdmnNo=cs.AdmnNo where sm.AdmnNo='" + admno + "' and Statusid in (2,3) ");
            if (dataTableQry.Rows.Count > 0)
            {
                drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
                drpClass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
                FillSection();
                drpSection.SelectedValue = dataTableQry.Rows[0]["Section"].ToString();
                drpSection_SelectedIndexChanged(drpStudent, EventArgs.Empty);
                drpStudent.SelectedValue = admno.ToString();
                btnTcForm.Enabled = true;
            }
            else
            {
                //ScriptManager.RegisterClientScriptBlock((Control)txtAdmnno, txtAdmnno.GetType(), "ShowMessage", "alert('Admission number does not exists')", true);


            }
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock((Control)txtAdmnno, txtAdmnno.GetType(), "ShowMessage", "alert('Admission number does not exists')", true);
        }
    }

    protected void btnTcForm_Click(object sender, EventArgs e)
    {
        GenerateForm();
    }

    private void GenerateForm()
    {
        txtTitle.Text = "Transfer/School Leaving Certificate";
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("Ps_Sp_GetTcStudDtls", new Hashtable()
 
    {
      {
         "@AdmnNo",
         drpStudent.SelectedValue.ToString()
      }
    });
        if (dataTable2.Rows.Count > 0)
        {
            txtStudName.Text = dataTable2.Rows[0]["fullname"].ToString();
            txtMotherNm.Text = dataTable2.Rows[0]["MotherName"].ToString();
            txtFAtherNm.Text = dataTable2.Rows[0]["FatherName"].ToString();
            //txtDOB.Text = dataTable2.Rows[0]["DOBStr"].ToString();
            if (dataTable2.Rows[0]["DOBStr"].ToString() != "")
            dtpDOB.SetDateValue(Convert.ToDateTime(dataTable2.Rows[0]["DOBStr"]));
            //dtpDOB.SetDateValue(Convert.ToDateTime(dataTable2.Rows[0]["DOBStr"]));
            txtAdmno.Text = dataTable2.Rows[0]["OldAdmnNo"].ToString();
            txtAdmnDt.Text = dataTable2.Rows[0]["AdmnDateStr"].ToString();
            dtpAdmnDt.SetDateValue(Convert.ToDateTime(dataTable2.Rows[0]["AdmnDateStr"]));
            txtCurrCls.Text = dataTable2.Rows[0]["ClassName"].ToString();
            //txtAddr.Text = dataTable2.Rows[0]["PermAddr1"].ToString();
            //txtPS.Text = dataTable2.Rows[0]["PermAddrCity"].ToString();
            //txtDist.Text = dataTable2.Rows[0]["PermAddrDist"].ToString();
            //txtPin.Text = dataTable2.Rows[0]["PermAddrPin"].ToString();
            //txtLetterNo.Text = dataTable2.Rows[0]["LetterNo"].ToString();
            //txtRecogDt.Text = dataTable2.Rows[0]["RecogDt"].ToString();
            //if (dataTable2.Rows[0]["RecogDt"].ToString() != "")
            //    dtpRecogDt.SetDateValue(Convert.ToDateTime(dataTable2.Rows[0]["RecogDt"]));
            //txtEduCir.Text = dataTable2.Rows[0]["EduCircle"].ToString();
            txtTcSl.Text = !(dataTable2.Rows[0]["TCSlTxt"].ToString() != "") ? drpSession.SelectedValue.ToString().Trim() : dataTable2.Rows[0]["TCSlTxt"].ToString();
            txtTcNo.Text = !(dataTable2.Rows[0]["TCSlno"].ToString() != "") ? "1" : dataTable2.Rows[0]["TCSlno"].ToString();
            txtTcDt.Text = dataTable2.Rows[0]["TCDate"].ToString();
            if (dataTable2.Rows[0]["TCDate"].ToString() != "")
                dtpTcDt.SetDateValue(Convert.ToDateTime(dataTable2.Rows[0]["TCDate"]));
            //txtReason.Text = dataTable2.Rows[0]["ReasonFrLeaving"].ToString();
            if (dataTable2.Rows[0]["ConductCharacter"].ToString() != "")
                txtCharactr.Text = dataTable2.Rows[0]["ConductCharacter"].ToString();
            txtRemarks.Text = dataTable2.Rows[0]["Remarks"].ToString();
            //txtIns.Text = dataTable2.Rows[0]["InsuredOrnot"].ToString();
            txtPromot.Text = dataTable2.Rows[0]["IfPromoted"].ToString();
            txtClass1.Text = dataTable2.Rows[0]["FrClass"].ToString();
            txtClass2.Text = dataTable2.Rows[0]["ToClass"].ToString();
            txtWorking.Text = dataTable2.Rows[0]["NoWorkingDys"].ToString();
            txtPresent.Text = dataTable2.Rows[0]["TotNoDyPresent"].ToString();
            txtMonth.Text = dataTable2.Rows[0]["DuePaidUpto"].ToString();
            txtSyr.Text = dataTable2.Rows[0]["MrkSheetSessYr"].ToString();
            TxtAdmnCls.Text = dataTable2.Rows[0]["Class"].ToString();
            TxtPrmSts.Text = dataTable2.Rows[0]["PromStatus"].ToString();
            ViewState["Gender"] = dataTable2.Rows[0]["Sex"].ToString();
            if (dataTable2.Rows[0]["Class"].ToString().Trim() != null)
            {
                DataTable dt = new clsDAL().GetDataTableQry("Select classname From Ps_ClassMaster where classid=" + dataTable2.Rows[0]["Class"].ToString().Trim() + "");
                if (dt.Rows.Count > 0)
                {
                    TxtAdmnCls.Text = dt.Rows[0]["ClassName"].ToString();
                }
            }
            TxtFromSchl.Text = dataTable2.Rows[0]["PrevSchoolName"].ToString();

            //puja code start
            txtSlno.Text = dataTable2.Rows[0]["SlNo"].ToString();
            txtSchBoardLastExam.Text = dataTable2.Rows[0]["SchBoardLastExam"].ToString();
            txtFailedStatus.Text = dataTable2.Rows[0]["FailedStatus"].ToString();
            txtPromotionClass.Text = dataTable2.Rows[0]["PromotionClass"].ToString();
            txtPromotionClassInWord.Text= dataTable2.Rows[0]["PromotionClassInWord"].ToString();
            txtMontLastPaidDue.Text = dataTable2.Rows[0]["LastPaidDue"].ToString();
           // txtSubStudies.Text = dataTable2.Rows[0]["SubStudies"].ToString();
           if(dataTable2.Rows[0]["ResLeavingScl"].ToString()!=""&& dataTable2.Rows[0]["ResLeavingScl"].ToString()!=null)
            drpReasonForleave.Text = dataTable2.Rows[0]["ResLeavingScl"].ToString();
            txtApplReqForCerft.Text = dataTable2.Rows[0]["ApplDtReqFrCerft"].ToString();
            txtIsueDtCerft.Text = dataTable2.Rows[0]["IsueDtCerft"].ToString();
            txtStudNccScoutDtl.Text = dataTable2.Rows[0]["StudNccScoutDtl"].ToString();
            txtExtraCurActivity.Text = dataTable2.Rows[0]["ExtracurActivities"].ToString();
            txtFeeConcession.Text = dataTable2.Rows[0]["FeeConcession"].ToString();
            txtAdmtclass.Text= dataTable2.Rows[0]["Admtclass"].ToString();
            //puja code End

            hifStudNationality.Value = dataTable2.Rows[0]["Nationality"].ToString();
            hifStudCat.Value = dataTable2.Rows[0]["CatName"].ToString();
            //HifStudentClass.Value = dataTable2.Rows[0]["SessionYear"].ToString();
            //hifStudfstClass.Value = dataTable2.Rows[0]["ClassName"].ToString();
            hifStudfstClass.Value = new clsDAL().ExecuteScalarQry("select ClassName from PS_StudMaster s join PS_ClassMaster c on s.Class=c.ClassID where s.AdmnNo="+drpStudent.SelectedValue.ToString() + "");

            pnlStud.Visible = true;
        }
        else
            pnlStud.Visible = false;
    }

    private void StudExtraDataDetailsEntry()
    {
        if(txtSlno.Text != "") 
        { 
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@Type", "insert");
        hashtable.Add("@SlNo", txtSlno.Text.ToString().Trim());
        hashtable.Add("@AdmnNo", drpStudent.SelectedValue);
        hashtable.Add("@SchBoardLastExam", txtSchBoardLastExam.Text.ToString().Trim());
        hashtable.Add("@FailedStatus", txtFailedStatus.Text.ToString().Trim());
       // hashtable.Add("@SubStudies", txtSubStudies.Text.ToString().Trim());
        hashtable.Add("@PromotionClass", txtPromotionClass.Text.ToString().Trim());
        hashtable.Add("@PromotionClassInWord", txtPromotionClassInWord.Text.ToString().Trim());
        hashtable.Add("@LastPaidDue", txtMontLastPaidDue.Text.ToString().Trim());
        hashtable.Add("@FeeConcession", txtFeeConcession.Text.ToString().Trim());
        hashtable.Add("@StudNccScoutDtl", txtStudNccScoutDtl.Text.ToString().Trim());
        hashtable.Add("@ExtracurActivities", txtExtraCurActivity.Text.ToString().Trim());
       // hashtable.Add("@ResLeavingScl", txtResLeavingSchool.Text.ToString().Trim());
            hashtable.Add("@ResLeavingScl", drpReasonForleave.SelectedValue.ToString().Trim());
            hashtable.Add("@ApplDtReqFrCerft", dtAppln.GetDateValue().ToString("dd-MMM-yyyy"));
        hashtable.Add("@IsueDtCerft", dtIsue.GetDateValue().ToString("dd-MMM-yyyy"));
        hashtable.Add("@Admtclass", txtAdmtclass.Text.ToString().Trim());
            obj.ExcuteProcInsUpdt("Sp_StudExtraDataDetailsForTc", hashtable);
    }
    else {
            Hashtable hashtable = new Hashtable();
            hashtable.Clear();
            hashtable.Add("@Type", "update");
            hashtable.Add("@SlNo", txtSlno.Text.ToString().Trim());
            hashtable.Add("@AdmnNo", drpStudent.SelectedValue);
            hashtable.Add("@SchBoardLastExam", txtSchBoardLastExam.Text.ToString().Trim());
            hashtable.Add("@FailedStatus", txtFailedStatus.Text.ToString().Trim());
           // hashtable.Add("@SubStudies", txtSubStudies.Text.ToString().Trim());
            hashtable.Add("@PromotionClass", txtPromotionClass.Text.ToString().Trim());
            hashtable.Add("@PromotionClassInWord", txtPromotionClassInWord.Text.ToString().Trim());
            hashtable.Add("@LastPaidDue", txtMontLastPaidDue.Text.ToString().Trim());
            hashtable.Add("@FeeConcession", txtFeeConcession.Text.ToString().Trim());
            hashtable.Add("@StudNccScoutDtl", txtStudNccScoutDtl.Text.ToString().Trim());
            hashtable.Add("@ExtracurActivities", txtExtraCurActivity.Text.ToString().Trim());
           // hashtable.Add("@ResLeavingScl", txtResLeavingSchool.ToString().Trim());
            hashtable.Add("@ResLeavingScl", drpReasonForleave.Text.ToString().Trim());
            hashtable.Add("@ApplDtReqFrCerft", dtAppln.GetDateValue().ToString("dd-MMM-yyyy"));
            hashtable.Add("@IsueDtCerft", dtIsue.GetDateValue().ToString("dd-MMM-yyyy"));
            obj.ExcuteProcInsUpdt("Sp_StudExtraDataDetailsForTc", hashtable);

        }

    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        if (Session["userrights"].ToString().Trim() == "a")
        {
            hashtable.Add("@FullName", txtStudName.Text.ToString().Trim());
            hashtable.Add("@FatherName", txtFAtherNm.Text.ToString().Trim());
            hashtable.Add("@MotherName", txtMotherNm.Text.ToString().Trim());
            hashtable.Add("@DOB", dtpDOB.GetDateValue().ToString("dd-MMM-yyyy"));
            hashtable.Add("@AdmnDate", dtpAdmnDt.GetDateValue().ToString("dd-MMM-yyyy"));
            hashtable.Add("@Class", drpClass.SelectedValue.ToString().Trim());
            hashtable.Add("@PrevSchoolName", TxtFromSchl.Text.ToString().Trim());
            //hashtable.Add("@PermAddr1", txtAddr.Text.ToString().Trim());
            //hashtable.Add("@PermAddrCity", txtPS.Text.ToString().Trim());
            //hashtable.Add("@PermAddrDist", txtDist.Text.ToString().Trim());
            //hashtable.Add("@PermAddrPin", txtPin.Text.ToString().Trim());
            hashtable.Add("@AdmRgt", "a");
        }
        //hashtable.Add("@LetterNo", txtLetterNo.Text.ToString().Trim());
        //if (txtRecogDt.Text.ToString() != "")
        //    hashtable.Add("@RecognisationDt", dtpRecogDt.GetDateValue().ToString("dd-MMM-yyyy"));
        //hashtable.Add("@EduCircle", txtEduCir.Text.ToString().Trim());
        hashtable.Add("@TCSlTxt", txtTcSl.Text.ToString().Trim());
        hashtable.Add("@TCSlno", txtTcNo.Text.ToString().Trim());
        if (txtTcDt.Text.ToString() != "")
            hashtable.Add("@TCDate", dtpTcDt.GetDateValue().ToString("dd-MMM-yyyy"));
        //hashtable.Add("@ReasonFrLeaving", txtReason.Text.ToString().Trim());
        hashtable.Add("@ConductCharacter", txtCharactr.Text.ToString().Trim());
        hashtable.Add("@Remarks", txtRemarks.Text.ToString().Trim());
        //hashtable.Add("@InsuredOrnot", txtIns.Text.ToString().Trim());
        hashtable.Add("@IfPromoted", txtPromot.Text.ToString().Trim());
        hashtable.Add("@PromStatus", TxtPrmSts.Text.ToString().Trim());
        hashtable.Add("@FrClass", txtClass1.Text.ToString().Trim());
        hashtable.Add("@ToClass", txtClass2.Text.ToString().Trim());
        hashtable.Add("@NoWorkingDys", txtWorking.Text.ToString().Trim());
        hashtable.Add("@TotNoDyPresent", txtPresent.Text.ToString().Trim());
        hashtable.Add("@DuePaidUpto", txtMonth.Text.ToString().Trim());
        hashtable.Add("@MrkSheetSessYr", txtSyr.Text.ToString().Trim());
        hashtable.Add("@AdmissionNo", txtAdmno.Text.ToString().Trim());
        hashtable.Add("@AdmnNo", drpStudent.SelectedValue);
        hashtable.Add("@UserID", Convert.ToInt32(Session["User_Id"]));

       
        obj.ExcuteProcInsUpdt("Ps_sp_InsUpdtTCStudents", hashtable);

        StudExtraDataDetailsEntry();
        PrintForm();
        Clear();
        ClearAll();
        try
        {
           // ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('StudentTCPrint.aspx');", true);
            Response.Redirect("~/Admissions/StudentTCPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void Clear()
    {
        txtStudName.Text = string.Empty;
        txtMotherNm.Text = string.Empty;
        txtFAtherNm.Text = string.Empty;
        txtDOB.Text = string.Empty;
        txtAdmno.Text = string.Empty;
        txtAdmnDt.Text = string.Empty;
        txtCurrCls.Text = string.Empty;
        //txtAddr.Text = string.Empty;
        //txtDist.Text = string.Empty;
        //txtPin.Text = string.Empty;
        //txtLetterNo.Text = string.Empty;
        //txtRecogDt.Text = string.Empty;
        //txtEduCir.Text = string.Empty;
        txtTcSl.Text = string.Empty;
        txtTcNo.Text = string.Empty;
        txtTcDt.Text = string.Empty;
        //txtReason.Text = string.Empty;
        txtCharactr.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        //txtIns.Text = string.Empty;
        txtPromot.Text = string.Empty;
        txtClass1.Text = string.Empty;
        txtClass2.Text = string.Empty;
        txtWorking.Text = string.Empty;
        txtPresent.Text = string.Empty;
        txtMonth.Text = string.Empty;
        txtSyr.Text = string.Empty;
    }

    private void PrintForm()
    {
        StringBuilder stringBuilder = new StringBuilder();
        string str = SchoolInformation();
        stringBuilder.Append("<table width='100%' >");
        stringBuilder.Append("<tr><td>");
        stringBuilder.Append("<fieldset>");
        stringBuilder.Append("<table width='100%'>");
        stringBuilder.Append("<tr><td colspan='4' align='center'>");
        stringBuilder.Append(str);
        stringBuilder.Append("</td></tr>");
        stringBuilder.Append("<tr><td colspan='4' align='center' ></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' align='center' >    </td></tr>");
        //stringBuilder.Append("<tr align='left'><td style='font-size:17px\;'> Book No:___________<b></b></td>");
        stringBuilder.Append("<tr align='left'><td align='center' style='font-size:17px;'> Sl. No: <b style='font-family:Times New Roman; font-Size:15px;'>" + txtSlno.Text.Trim() +"</b></td>");
        stringBuilder.Append("<td align='center' style='font-size:17px;'> Admission No: <b style='font-family:Times New Roman; font-Size:15px;'><span class='UlSpan'>" + txtAdmno.Text.Trim() + "</span></b></td>");
        stringBuilder.Append("<td align='right' style='font-size:14px;'>SCHOOL CODE NO: <b>15399</b><br></td></tr>");
        //stringBuilder.Append("<tr><td align='left' style='width:250px;font-size:17px;'>Admission No :  <i><span class='UlSpan'>" + txtAdmno.Text.Trim() + "</span></i><b></b></td>");
        //stringBuilder.Append("<td align='right' style='width:250px;font-size:17px\;'>Sl/TC/SLC No:<b></b></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' align='center' style='font-size:17px;' ><u></u></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' align='center'></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' align='center'></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' align='center'></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>1. Name of the Student : <b style='font-family:Times New Roman; font-Size:15px;'>" + txtStudName.Text.Trim() + "</b></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>2. Mother's Name :<b style='font-family:Times New Roman; font-Size:15px;'><span class='UlSpan'>" + txtMotherNm.Text.Trim() + "</span></b></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>3. Father's Name : <b style='font-family:Times New Roman; font-Size:15px;'><span class='UlSpan'>" + txtFAtherNm.Text.Trim() + "</span></b></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px\;'>4. Permanent Address : <i><span class='UlSpan'></span></i></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px\;'>&nbsp;&nbsp;&nbsp;Police Station: <i><span class='UlSpan'></span></i> District: <i><span class='UlSpan'></span>Pin: <i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>4. Nationality : <b style='font-family:Times New Roman; font-Size:15px;'><span class='UlSpan'>" + hifStudNationality.Value.ToString() + "</span></b></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>5. Whether  S.C , S.T , OBC : <b style='font-family:Times New Roman; font-Size:15px;'><span class='UlSpan'>" + hifStudCat.Value.ToString() + "</span></b></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>6. Date of first admission in the School :<b style='font-family:Times New Roman; font-Size:15px;'><span class='UlSpan'>" + txtAdmnDt.Text.Trim() + "</span></b> , in class : <b style='font-family:Times New Roman'><span class='UlSpan'>" + txtAdmtclass.Text + "</span></b></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>7. Date of birth(in Christian Era) as recorded in the Admission Register: (in figure) <b style='font-family:Times New Roman'><span class='UlSpan'>" + txtDOB.Text.Trim() + "</span></b</td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='padding-left:16px;font-size:17px;'>In Words :<b style='font-family:Times New Roman; font-Size:15px;'><span class='UlSpan'>" + Admissions_StudentTC.NumberToDay(dtpDOB.GetDateValue().Day) + " " + dtpDOB.GetDateValue().ToString("MMMM") + " " + Admissions_StudentTC.NumberToWords(dtpDOB.GetDateValue().Year) + "</span></b></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'> &nbsp;&nbsp;&nbsp;&nbsp;Class Of First Admission: <i><span class='UlSpan'>" + hifStudfstClass.Value.ToString() + "</span></i></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>8. Date Of Leaving : <i><span class='UlSpan'>" + txtTcDt.Text.Trim() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>8. Class in which the student studied last :  <b style='font-family:Times New Roman; font-Size:15px;'><span class='UlSpan'>" + txtCurrCls.Text.Trim() + "</span></i>, (in the Session) :"+drpSession.SelectedValue+"<b style='font-family:Times New Roman'><span class='UlSpan'>" + HifStudentClass.Value.ToString() + "</span></b></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>  <i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>9. School/Board Annual Examination last taken with result :<b style='font-family:Times New Roman; font-Size:15px;'> <span style='color: black;'>" + txtSchBoardLastExam.Text.Trim() + "</span></b></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>10. Whether failed, once or twice, in the same class : <b style='font-family:Times New Roman; font-Size:15px;'> <span style='color: black;'>" + txtFailedStatus.Text.Trim() + "</span></b></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>11. Subjects Studied:1. ENGLISH 2.MATHEMATICS 3.SCIENCE 4.SOCIAL SCIENCE 5.HINDI 6. INFORMATION TECHNOLOGY </span><br>");
       // stringBuilder.Append("<span style='padding-left:16px;'>4</span>.<span style='color: gray;'>...........................</span>,5.<span style='color: gray;'>...........................</span>,6.<span style='color: gray;'>...........................</span> </td>");
        // stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>11. Subjects Studied:<b style='font-family:Times New Roman; font-Size:15px;'><span style='color: black;'>" + txtSubStudies.Text.Trim() + "</span></b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>12. Whether qualified for promotion to higher class if so, which class : <br>");
        stringBuilder.Append("<span style='padding-left:16px;'>(in figures):</span> <b style='font-family:Times New Roman; font-Size:15px;'><span class='UlSpan'>" + txtPromotionClass.Text.Trim() + "</span></b> </br>");
        stringBuilder.Append("<span style='padding-left:16px;'>(in Words):</span> <b style='font-family:Times New Roman; font-Size:15px;'><span style='color: black;'>" + txtPromotionClassInWord.Text.Trim() + "</span></b></td>");
        stringBuilder.Append("</tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>11. If so, From And To Which Class : <i><span class='UlSpan'>" + txtClass1.Text.Trim() + " , " + txtClass2.Text + "</span></i></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>12. Reason For Leaving The School : <i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>13. Month upto which the Student has paid school dues :<b style='font-family:Times New Roman; font-Size:15px;'><span style='color: black;'>" + txtMontLastPaidDue.Text.Trim() + "</span></b> <i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>14. Any fee concession availed of : if so ,the nature of such concession :<b style='font-family:Times New Roman; font-Size:15px;'> <span style='color: black;'>" + txtFeeConcession.Text.Trim() + "</span></b><i><span class='UlSpan'></span></i></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px\;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; If Leaving During Mid-Session : <i><span class='UlSpan'>" + txtTcDt.Text.Trim() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>15. Total No. of School working days:</span> <b style='font-family:Times New Roman'><span class='UlSpan'>" + txtWorking.Text.Trim() + "</span></b> <br> 16.&nbsp;Total days present: <b style='font-family:Times New Roman'><span class='UlSpan'>" + txtPresent.Text.Trim() + "</span></b></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>17. Whether NCC Cadet/Boy Scout/Girl Guide(give details) : <b style='font-family:Times New Roman; font-Size:15px;'> <span style='color: black;'>" + txtStudNccScoutDtl.Text.Trim() + "</span></b><i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>18. Games played / extra curricular activities in which, <br>");
        stringBuilder.Append("<span style='padding-left:16px;'>the student took part</span>(mention <span class='UlSpan1'>achievement level there in) : <b style='font-family:Times New Roman; font-Size:15px;'> <span style='color: black;'>" + txtExtraCurActivity.Text.Trim() + "</span></b><i><span class='UlSpan'></span></i></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>19. Reason for leaving the school : <b style='font-family:Times New Roman; font-Size:15px;'> <span style='color: black;'>" + drpReasonForleave.SelectedValue.ToString().Trim() + "</span></b><i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>20. General Conduct : <b style='font-family:Times New Roman; font-Size:15px;'><span class='UlSpan'>" + txtCharactr.Text.Trim() + "</span></b></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>21. Date of application for Certificate : <b style='font-family:Times New Roman; font-Size:15px;'> <span style='color: black;'>" + txtApplReqForCerft.Text.Trim() + "</span></b><i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>22. Date of issue of Certificate : <b style='font-family:Times New Roman; font-Size:15px;'> <span style='color: black;'>" + txtIsueDtCerft.Text.Trim() + "</span><i><span class='UlSpan'></span></b></td></tr>");      
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>23. Any other remarks : <b style='font-family:Times New Roman; font-Size:15px;'><span class='UlSpan'>" + txtRemarks.Text.Trim() + "</span></b></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>16. All Mark Sheet For the Session Year <i><span class='UlSpan'>" + txtSyr.Text.Trim() + "</span></i> Have Been</td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Attached With This Certificate </td></tr>");
        //stringBuilder.Append("<tr><td>&nbsp;</td></tr>");
        //stringBuilder.Append("<tr><td>&nbsp;</td></tr>");
        //stringBuilder.Append("<tr><td>&nbsp;</td></tr>");
        //stringBuilder.Append("<tr><td>&nbsp;</td></tr>");
        stringBuilder.Append("<tr><td>&nbsp;</td></tr>");
        stringBuilder.Append("<tr><td>Class Teacher</td><td style='text-align: center;'>Checked By</td><td></td><td>Principal</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr><td></td><td margin-right: 30px;></td></tr>");
        stringBuilder.Append("<tr><td>&nbsp;</td></tr>");
        //stringBuilder.Append("<tr><td>&nbsp;</td></tr>");
        
        stringBuilder.Append("</table>");

        stringBuilder.Append("</fieldset>");
        stringBuilder.Append("</td></tr>");
        stringBuilder.Append("</table>");

        Session["TCDtls"] = stringBuilder.ToString();


        stringBuilder.Append("</fieldset>");
        stringBuilder.Append("<td>");
        stringBuilder.Append("<fieldset>");
        stringBuilder.Append("<table width='100%' style='border-right-width:1px;'>");
        stringBuilder.Append("<tr><td colspan='2' align='center'>");
        stringBuilder.Append(str);
        stringBuilder.Append("</td></tr>");
        stringBuilder.Append("<tr><td colspan='2' align='center'></td></tr>");
        //stringBuilder.Append("<tr align='left'><td style='font-size:17px;'> Book No:<b></b></td>");
        stringBuilder.Append("<tr align='left'><td align='left' style='font-size:17px;'> Date :<b></b></td></tr>");
        stringBuilder.Append("<tr><td align='left' style='width:250px;font-size:17px;'>Admission No :  <i><span class='UlSpan'>" + txtAdmno.Text.Trim() + "</span></i><b></b></td>");
        stringBuilder.Append("<td align='center' style='width:250px;font-size:17px;'> Sl/TC/SLC No:<b></b></td></tr>");
        stringBuilder.Append("<tr><td colspan='2' align='center' style='font-size:17px;font-weight:bold;' ><u></u></td></tr>");
        stringBuilder.Append("<tr><td colspan='2' align='center'></td></tr>");
        stringBuilder.Append("<tr><td colspan='2' align='center'></td></tr>");
        stringBuilder.Append("<tr><td colspan='2' align='center'></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>1. Name of the Student : <i><span class='UlSpan'>" + txtStudName.Text.Trim() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>2. Mother's Name : <i><span class='UlSpan'>" + txtMotherNm.Text.Trim() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>3. Father's Name : <i><span class='UlSpan'>" + txtFAtherNm.Text.Trim() + "</span></i></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>4. Permanent Address : <i><span class='UlSpan'></span></i></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>&nbsp;&nbsp;&nbsp;Police Station: <i><span class='UlSpan'></span></i> District: <i><span class='UlSpan'></span>Pin: <i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>4. Nationality : <i><span class='UlSpan'>" + hifStudNationality.Value.ToString() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>5. Whether  S.C or S.T : <i><span class='UlSpan'>" + hifStudCat.Value.ToString() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>6. Date of first admission in the School : <i><span class='UlSpan'>" + txtAdmnDt.Text.Trim() + "</span></i> , in class : <i><span class='UlSpan'>" + hifStudfstClass.Value.ToString() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>7. Date of birth(in Christian Era) as recorded in the Addmission Register: (in figure) <i><span class='UlSpan'>" + txtDOB.Text.Trim() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='padding-left:16px;font-size:17px;'>In Words : <i><span class='UlSpan'>" + Admissions_StudentTC.NumberToDay(dtpDOB.GetDateValue().Day) + " " + dtpDOB.GetDateValue().ToString("MMMM") + " " + Admissions_StudentTC.NumberToWords(dtpDOB.GetDateValue().Year) + "</span></i></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'> &nbsp;&nbsp;&nbsp;&nbsp;Class Of First Admission: <i><span class='UlSpan'>" + hifStudfstClass.Value.ToString() + "</span></i></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>8. Date Of Leaving : <i><span class='UlSpan'>" + txtTcDt.Text.Trim() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>8. Class in which the student studied last :  <i><span class='UlSpan'>" + txtCurrCls.Text.Trim() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>  <i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>9. School/Board Annual Examination last taken with result : _____________________ </td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>10. Whether failed, once or twice, in the same class : ______________________</td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>11. Subjects Studied:1._____________,2._____________, 3._____________,4._____________,5._____________,6._____________ </td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>12. Whether qualified for promotion to higher class if so, which class : (in figures)  __________________ <i><span class='UlSpan'>" + txtPromot.Text.Trim() + "</span></i> </br>(in Words) <i><span class='UlSpan'>____________________</span></i></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>11. If so, From And To Which Class : <i><span class='UlSpan'>" + txtClass1.Text.Trim() + " , " + txtClass2.Text + "</span></i></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>12. Reason For Leaving The School : <i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>13. Month upto which the Student has paid school dues : <i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>14. Any fee concession availed of : if so ,the nature of such concession : __________________<i><span class='UlSpan'></span></i></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; If Leaving During Mid-Session : <i><span class='UlSpan'>" + txtTcDt.Text.Trim() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>15. Total No. of School working days: <i><span class='UlSpan'>" + txtWorking.Text.Trim() + "</span></i> <br> 16.&nbsp;Total days present: <i><span class='UlSpan'>" + txtPresent.Text.Trim() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>17. Whether NCC Cadet/Boy Scout/Girl Guide(give details) : ____________________<i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>18. Games played / extra curricular activities in which the student took part(mention achievement level there in) : __________________________<i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>19. Reasons for leaving the school : ________________<i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>20. General Conduct : <i><span class='UlSpan'>" + txtCharactr.Text.Trim() + "</span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>21. Date of application for Certificate : ____________<i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>22. Date of issue of Certificate : ________________<i><span class='UlSpan'></span></i></td></tr>");
        stringBuilder.Append("<tr><td colspan='4' style='font-size:17px;'>23. Any other remarks : <i><span class='UlSpan'>" + txtRemarks.Text.Trim() + "</span></i></td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>16. All Mark Sheet For the Session Year <i><span class='UlSpan'>" + txtSyr.Text.Trim() + "</span></i> Have Been</td></tr>");
        //stringBuilder.Append("<tr><td colspan='2' style='font-size:17px;'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Attached With This Certificate </td></tr>");

        //stringBuilder.Append("<tr><td>&nbsp;</td></tr>");
        //stringBuilder.Append("<tr><td>&nbsp;</td></tr>");
        stringBuilder.Append("<tr><td align='Center' >Signature of:</td><td align='Center' >Checked By</td><td align='Center' style='font-size:17px;'>Signature of Principal</td></tr>");
        stringBuilder.Append("<tr><td align='Center' >Class Teacher</td><td align='Center' >(With Full Name & designation)</td><td align='Center' style='font-size:17px;'>(With Date and School Seal)</td></tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("</fieldset>");
        stringBuilder.Append("</td></tr>");
        stringBuilder.Append("</table>");
        //Session["TCDtls"] = stringBuilder.ToString();
    }

    //private void PrintForm()
    //{
    //    string Gender1 = "";
    //    string Gender2= "";
    //    string Gender3 = "";
    //    string Gender4 = "";
    //    string from = "";
    //    if (ViewState["Gender"].ToString() == "Male")
    //    {
    //        Gender1 = "S/o :";
    //        Gender2 = "He";
    //        Gender3 = "his";
    //        Gender4 = "His";
    //    }
    //    else
    //    {
    //        Gender1 = "D/o :";
    //        Gender2 = "She";
    //        Gender3 = "her";
    //        Gender4 = "Her";
    //    }
    //    if (TxtFromSchl.Text == "")
    //    {
    //        from = "";
    //    }
    //    else
    //        from = "from";

    //    DateTime dd = DateTime.Now;
    //    string date = dd.ToString("dd-MM-yyyy");

    //    StringBuilder stringBuilder = new StringBuilder();
    //    stringBuilder.Append("<table width='100%' >");
    //    stringBuilder.Append("<tr><td><span style='margin-left: 50px;font-size: 17px\;'>  Admn. No: "+ txtAdmno.Text.Trim()+"</span></td><td></td>");
    //    stringBuilder.Append("<td><span style='font-size: 17px\;'>Sl no: " + txtTcNo.Text.Trim() + "</span></td></tr>");
    //    stringBuilder.Append("<tr><td colspan='3' height='40Px' ></td></tr>");
    //    stringBuilder.Append("<tr><td colspan='3' align='center'  style='font-size: 30px;'> <u>TRANSFER CERTIFICATE</u></td></tr>");
    //    stringBuilder.Append("<tr><td colspan='3' align='center'  style='font-size: 25px;'> (SCHOOL LEAVING CERTIFICATE)</td></tr>");
    //    stringBuilder.Append("<tr><td colspan='3' height='40Px' ></td></tr>");
    //    stringBuilder.Append("<tr><td colspan='3'><p style='margin-left:50px; font-size:17px\; margin-right: 40px; text-align: justify;'>This is to certify that <b> " + txtStudName.Text.Trim() + " </b><br>" + Gender1 + "<b>Mr. " + txtFAtherNm.Text.Trim() + "</b> and <b> Mrs." + txtMotherNm.Text.Trim() + "</b> was admitted into the school on <b> " + txtAdmnDt.Text.Trim() + " </b> in <b> " + TxtAdmnCls.Text + "</b> " + from + " <b>" + TxtFromSchl.Text + "</b> and left the school on <b> " + txtTcDt.Text.Trim() + " </b> with a <b> " + txtCharactr.Text.Trim() + "</b> character. " + Gender2 + " was then studying in the <b> " + txtCurrCls.Text.Trim() + " </b> class of the ICSE stream .");
    //    stringBuilder.Append("The school academic year is from March to February.<br><br>All sums due to this school on "+Gender3+" account have been remitted.<br><br><br>"+Gender4+" date of birth according to our School Admission Register is <br><b>" + txtDOB.Text.Trim() + "</b> &nbsp <b>(" + Admissions_StudentTC.NumberToDay(dtpDOB.GetDateValue().Day) + " " + dtpDOB.GetDateValue().ToString("MMMM") + " " + Admissions_StudentTC.NumberToWords(dtpDOB.GetDateValue().Year) + ")</b><br><br>Promotion Status: <b>"+ TxtPrmSts.Text+"</b><br><br>Issue Date: <b>" + dtpIssdt.GetDateValue().ToString("dd-MM-yyyy") + "</b><br><br>");
    //    stringBuilder.Append("<br><div style='float: right;font-size:22px; margin-right: 40px;'><b></b><br><span style=' margin-left: 100px'><b> Principal</b></span></div></p></td></tr></table>");
    //    Session["TCDtls"] = stringBuilder.ToString();
    //}


    private string SchoolInformation()
    {
        DataTable dataTable1 = new DataTable();
        DataSet dataSet = new DataSet();
        StringBuilder stringBuilder = new StringBuilder();
        clsDAL clsDal = new clsDAL();
        string str1 = Server.MapPath("../XMLFiles") + "\\Detail.xml";
        if (File.Exists(str1))
        {
            int num = (int)dataSet.ReadXml(str1);
            string str2 = Session["SSVMID"].ToString();
            if (dataSet.Tables.Count > 0)
            {
                DataTable dataTable2 = new DataTable();
                DataTable table = dataSet.Tables[0];
                foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' class='tbltd'><tr><td rowspan='6'> <img src='../images/leftlogo.png' width='120px' height='100px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:17px; white-space:nowrap;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='55px' height='60px'/ ></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:12px;'><b>(SENIOR SECONDARY (10+2) AFFILIATED TO C.B.S.E., NEW DELHI)</b></td></tr>");
                    
                    //stringBuilder.Append("<tr><td align='center' style='font-size:16px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</b></td></tr>");
                    //stringBuilder.Append("<tr><td align='center' style='font-size:16px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + clsDal.Decrypt(row["Pin"].ToString().Trim(), str2) + "</b> </td></tr>");
                    //stringBuilder.Append("<tr><td align='center' style='font-size:16px;'><b>Phone-" + clsDal.Decrypt(row["Phone"].ToString().Trim(), str2) + "</b></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:12px;'><b>AFFILIATION NO.-1530109</b></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:12px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "," + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ", Pin:-" + clsDal.Decrypt(row["Pin"].ToString().Trim(), str2) + "</b></td></tr>");
                    //stringBuilder.Append("<tr><td align='center' style='font-size:16px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + clsDal.Decrypt(row["Pin"].ToString().Trim(), str2) + "</b> </td></tr>");
                    //stringBuilder.Append("<tr><td align='right' style='font-size:14px;'></td></tr>");
                    //stringBuilder.Append("<tr><td align='right'width='100%'' style='font-size:14px;'><b>SCHOOL CODE NO: 15399</b><br></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:17px;'><b><u>TRANSFER CERTIFICATE</u></b></td>");
                    stringBuilder.Append("<td style='border:2px solid black; height:100px; text-align:center;color:gray;'>School Stamp</td></tr>");


                    stringBuilder.Append("<tr><td align='center' colspan='3' style='font-size:17px;'><b></b></td></tr></table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    public static string NumberToWords(int number)
    {
        if (number == 0)
            return "zero";
        if (number < 0)
            return "minus " + Admissions_StudentTC.NumberToWords(Math.Abs(number));
        string str = "";
        if (number / 1000000 > 0)
        {
            str = str + Admissions_StudentTC.NumberToWords(number / 1000000) + " million ";
            number %= 1000000;
        }
        if (number / 1000 > 0)
        {
            str = str + Admissions_StudentTC.NumberToWords(number / 1000) + " thousand ";
            number %= 1000;
        }
        if (number / 100 > 0)
        {
            str = str + Admissions_StudentTC.NumberToWords(number / 100) + " hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (str != "")
                str += "and ";
            string[] strArray1 = new string[20]
      {
        "Zero",
        "One",
        "Two",
        "Three",
        "Four",
        "Five",
        "Six",
        "Seven",
        "Eight",
        "Nine",
        "Ten",
        "Eleven",
        "Twelve",
        "Thirteen",
        "Fourteen",
        "Fifteen",
        "Sixteen",
        "Seventeen",
        "Eighteen",
        "Nineteen"
      };
            string[] strArray2 = new string[10]
      {
        "Zero",
        "Ten",
        "Twenty",
        "Thirty",
        "Forty",
        "Fifty",
        "Sixty",
        "Seventy",
        "Eighty",
        "Ninety"
      };
            if (number < 20)
            {
                str += strArray1[number];
            }
            else
            {
                str += strArray2[number / 10];
                if (number % 10 > 0)
                    str = str + "-" + strArray1[number % 10];
            }
        }
        return str;
    }

    public static string NumberToDay(int number)
    {
        if (number == 0)
            return "zero";
        if (number < 0)
            return "minus " + Admissions_StudentTC.NumberToWords(Math.Abs(number));
        string str = "";
        if (number / 1000000 > 0)
        {
            str = str + Admissions_StudentTC.NumberToWords(number / 1000000) + " million ";
            number %= 1000000;
        }
        if (number / 1000 > 0)
        {
            str = str + Admissions_StudentTC.NumberToWords(number / 1000) + " thousand ";
            number %= 1000;
        }
        if (number / 100 > 0)
        {
            str = str + Admissions_StudentTC.NumberToWords(number / 100) + " hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (str != "")
                str += "and ";
            string[] strArray1 = new string[32]
      {
        "zero",
        "First",
        "Second",
        "Third",
        "Fourth",
        "Fifth",
        "Sixth",
        "Seventh",
        "Eighth",
        "Nineth",
        "Tenth",
        "Eleventh",
        "Twelveth",
        "Thirteenth",
        "Fourteenth",
        "Fifteenth",
        "Sixteenth",
        "Seventeenth",
        "Eighteenth",
        "Nineteeth",
        "Twentieth",
        "Twenty First",
        "Twenty Second",
        "Twenty Third",
        "Twenty Fourth",
        "Twenty Fifth",
        "twenty Sixth",
        "twenty Seventh",
        "twenty Eighth",
        "twenty Nineth",
        "Thirtieth",
        "Thirty First"
      };
            string[] strArray2 = new string[10]
      {
        "Zero",
        "Ten",
        "Twenty",
        "Thirty",
        "Forty",
        "Fifty",
        "Sixty",
        "Seventy",
        "Eighty",
        "Ninety"
      };
            if (number < 20)
            {
                str += strArray1[number];
            }
            else
            {
                str += strArray2[number / 10];
                if (number % 10 > 0)
                    str = str + "-" + strArray1[number % 10];
            }
        }
        return str;
    }


}