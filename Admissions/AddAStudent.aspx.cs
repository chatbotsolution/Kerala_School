using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.Profile;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Admissions_AddAStudent : System.Web.UI.Page
{
     public static int sibcount;
    private ScriptManager sm;
    protected void Page_Load(object sender, EventArgs e)
    {
        string appSetting = ConfigurationManager.AppSettings["BkUp"];
        Page.MaintainScrollPositionOnPostBack = true;
        if (Page.IsPostBack)
            return;
        ViewState["Stream"] = 0;
        lblMsgBottom.Text = "";
        lblMsgTop.Text = "";
        Page.Form.DefaultButton = btnSubmit.UniqueID;
        lkaddmore.Visible = false;
        lnkdelete.Visible = false;
        ViewState["Admn"] = string.Empty;
        ViewState["StudImg"] = string.Empty;
        ViewState["StudDoc"] = string.Empty;
        btnPrintReceipt1.Enabled = false;
        filldropdowns();
        GetStuType();
        GetFormType();
        FillStream();
        if (Request.QueryString["sid"] != null)
        {
            ViewState["Admn"] = "Update";
            DispStudInfo();
            btnPrintReceipt1.Enabled = false;
            hfSaveMode.Value = "Update";
            feeOptionRow.Visible = false;
        }
        else
        {
           // feeOptionRow.Visible = true;
            chkAnnual.Checked = true;
            chkOT.Checked = true;
            PopCalAdmnDt.SetDateValue(DateTime.Today);
            txtnationality.Text = "Indian";
            txtMotherTongue.Text = "Oriya";
            txtrollno.Text = "0";
            txtprevmedium.Text = "Oriya";
           // feeOptionRow.Visible = true;
            lblAdmnNo.Text = "";
            ChkBus.Checked = true;
            ChkHostel.Checked = true;
        }
        
    }

    private void filldropdowns()
    {
        new clsStaticDropdowns().FillSessYrForAdmn(drpAdmSessYr);
        FillClass();
        
        //fillClassNew();
        getsections();
        Fillcategory();
        FillReligion();
        FillSession();
       // FillHouse();
    }


    private void fillClassNew()
     {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@FormType", Convert.ToInt16(Request.QueryString["FormType"]));
        string Sp_Name="";
        if (Request.QueryString["sid"] != null)
        {
            Sp_Name = "ps_sp_get_ClassForadmnnEdit";
        }
        else
        {
            Sp_Name = "ps_sp_get_ClassForadmnnNew";
        }
        DataTable dataTable = clsDal.GetDataTable(Sp_Name, hashtable);
        drpnewclass.DataSource = dataTable;

        drpnewclass.DataTextField = "ClassName";
        drpnewclass.DataValueField = "classid";
        drpnewclass.DataBind();
        drpnewclass.Items.Insert(0,(new ListItem("-select-", "0")));  




    }

    private void FillClass()
    {
        DataTable dataTable = new clsDAL().GetDataTable("ps_sp_get_ClassForAdmnDDL");
        drpclass.DataSource = dataTable;
        drpclass.DataTextField = "ClassName";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpnewclass.DataSource = dataTable;
        drpnewclass.DataTextField = "ClassName";
        drpnewclass.DataValueField = "classid";
        drpnewclass.DataBind();
        drpsiblingclass.DataSource = dataTable;
        drpsiblingclass.DataTextField = "ClassName";
        drpsiblingclass.DataValueField = "classid";

        drpsiblingclass.DataBind();
    }

    private void getsections()
    {
        new clsStaticDropdowns().FillSection(drpsection);
    }

    private void Fillcategory()
    {
        drpcat.DataSource = new clsDAL().GetDataTableQry("select * from PS_CategoryMaster order by CatName");
        drpcat.DataTextField = "CatName";
        drpcat.DataValueField = "CatID";
        drpcat.DataBind();
    }

    private void FillReligion()
    {
        drprelgn.DataSource = new clsDAL().GetDataTableQry("select * from ps_studentreligion order by ReligionId");
        drprelgn.DataTextField = "Religion";
        drprelgn.DataValueField = "ReligionId";
        drprelgn.DataBind();
    }

    private void FillSession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select distinct SessionID,SessionYr from dbo.PS_FeeNormsNew order by sessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }
    private void FillStream()
    {
        RbCourse.DataSource = new clsDAL().GetDataTableQry("SELECT StreamID,Description FROM dbo.PS_StreamMaster ORDER BY StreamID ASC");
        RbCourse.DataValueField = "StreamID";
        RbCourse.DataTextField = "Description";
        RbCourse.DataBind();
        RbCourse.Items.RemoveAt(0);
       // RbCourse.SelectedIndex = 1;
    }
    private void FillHouse()
    {
        drpHouse.DataSource = new clsDAL().GetDataTableQry("select * from PS_StudentHouse order by HouseID");
        drpHouse.DataTextField = "HouseName";
        drpHouse.DataValueField = "HouseID";
        drpHouse.DataBind();
        drpHouse.Items.Insert(0, (new ListItem("-select-", "0")));
    }
    protected void RbCourse_SelectedIndexChanged(object sender, EventArgs e)
    {
        int streamId = Convert.ToInt32(RbCourse.SelectedValue);
        FillCourse(streamId);
        Filloptional(streamId);
    }
    protected void FillCourse(int streamId)
    {
        ViewState["Stream"] = streamId;
        //if (streamId != 0)
        if (0 != 0)
        {
            DataTable dt = new clsDAL().GetDataTableQry("SELECT DISTINCT SubjectName FROM dbo.PS_SubjectMaster Where StreamID=" + streamId + "and ClassId=13 and IsOptional='false'");
            lbltd1.Text = dt.Rows[0][0].ToString();
            lbltd2.Text = dt.Rows[1][0].ToString();
            lbltd3.Text = dt.Rows[2][0].ToString();
            lbltd4.Text = dt.Rows[3][0].ToString();
            lbltd5.Text = "N/A";
            lbltd6.Text = "N/A";
        }
    }
    protected void Filloptional(int streamId)
    {
        RbOptional.DataSource = new clsDAL().GetDataTableQry("SELECT DISTINCT SubjectId,SubjectName FROM dbo.PS_SubjectMaster Where StreamID=" + streamId + "and ClassId=13 and IsOptional='true'");
        //if (streamId == 3)
        if (4 == 3)
        {
            RbOptional.DataTextField = "SubjectName";
            RbOptional.DataValueField = "SubjectId";
            RbOptional.DataBind();
            RbOptional.Items.RemoveAt(2);
            ChksixthSub.Visible = true;
            ChksixthSub.Checked = false;
            DataTable dt = new clsDAL().GetDataTableQry("Select SubjectId from dbo.Ps_Subjectmaster where StreamId=3 and ClassId=13 and Subjectname='COMPUTER SCIENCE'");
            ViewState["SixthOptional"] = dt.Rows[0]["SubjectId"].ToString();
        }
        else
        {

            RbOptional.DataTextField = "SubjectName";
            RbOptional.DataValueField = "SubjectId";
            RbOptional.DataBind();
            ChksixthSub.Visible = false;
        }
       // RbOptional.SelectedIndex = 1;
    }
    protected void RbOptional_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbltd5.Text = RbOptional.SelectedItem.Text;
    }
    protected void ChksixthSub_Changed(object sender, EventArgs e)
    {
        if (ChksixthSub.Checked)
            lbltd6.Text = "COMPUTER SCIENCE";
        else
            lbltd6.Text = "N/A";

    }
    protected void drpnewclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FillOfficeOptional();
    }
    private void FillOfficeOptional()
    {
        //if ((Request.QueryString["FormType"] == "0" && Request.QueryString["FormType"] != null) || (Request.QueryString["Class"] != null && Convert.ToInt32(Request.QueryString["Class"].ToString()) <= 12))
        //{
            int cls = Convert.ToInt32(drpnewclass.SelectedValue.ToString().Trim());
            if (cls > 1 && cls < 13)
            {
                if (cls == 11 || cls == 12)
                {
                    drpSixthSubject.Enabled = true;
                    drpBindSixth(cls);
                    
                }
                else
                    drpSixthSubject.Enabled = false;
                drpSixthSubject.Items.Insert(0, new ListItem("-Select-", "0"));
                ViewState["SixthOptional"] = drpSixthSubject.SelectedValue;
                
                OptionalPanel.Visible = true;

            }
            else
                OptionalPanel.Visible = false;
        //}
    }

    private void drpBindSixth(int cls)
    {
        DataTable dt = new clsDAL().GetDataTableQry("Select SubjectId,SubjectName from PS_SubjectMaster where streamid=1 and classid =" + cls + " and IsOptional='true'");
        drpSixthSubject.DataSource = dt;
        drpSixthSubject.DataTextField = "SubjectName";
        drpSixthSubject.DataValueField = "SubjectId";
        drpSixthSubject.DataBind();
       
    }
    protected void drpBindSixth_SelectedIndexchanged(object sender, EventArgs e)
    {
        ViewState["SixthOptional"] = drpSixthSubject.SelectedValue;
    }
    private void DispStudInfo()
    {
        DataSet dataSet = new clsDAL().GetDataSet("sp_display_studentinfo", new Hashtable()
    {
      {
         "admnno",
         Request.QueryString["sid"]
      },
      {
         "session",
         Request.QueryString["sy"].ToString()
      }
    });
        GridFill(dataSet.Tables[1], dataSet.Tables[2]);
        showdata(dataSet.Tables[0]);
        ChkBusHostel(dataSet.Tables[3]);
        ViewState["Admn"] = Request.QueryString["sid"];
        ViewState["SessYr"] = Request.QueryString["sy"].ToString();
        ViewState["classid"] = drpnewclass.SelectedValue.ToString().Trim();
    }

    private void GetStuType()
    {
        if (Request.QueryString["Type"] == null)
            return;
        drpStudType.SelectedValue = Request.QueryString["Type"].ToString().Trim();
    }
    private void GetFormType()
    {
        if (Request.QueryString["FormType"] == null && Request.QueryString["Class"] == null)
        {
            Response.Redirect("~/Admissions/Home.aspx");
            return;
        }
        else if (Request.QueryString["FormType"] == "0" && Request.QueryString["FormType"] != null)
        {
            lblFormtype.Text = "I-X";
            CoursePanel.Visible = false;
            StdXAvgAPanel.Visible = false;
            ViewState["Stream"] = "1";
            FillOfficeOptional();
        }
        else if (Request.QueryString["FormType"] == "2" && Request.QueryString["FormType"] != null)
        {
            lblFormtype.Text = "NUR-UKG";
            CoursePanel.Visible = false;
            StdXAvgAPanel.Visible = false;
            ViewState["Stream"] = RbCourse.SelectedValue;
            FillOfficeOptional();
        }
        else if (Request.QueryString["Class"] != null)
        {
            if (Convert.ToInt32(Request.QueryString["Class"].ToString()) <= 12)
            {
                lblFormtype.Text = "LKG-STD X";
                CoursePanel.Visible = false;
                StdXAvgAPanel.Visible = false;
                ViewState["Stream"] = "1";
                FillOfficeOptional();
            }
        }
        else
        {
            lblFormtype.Text = "XI-XII";
            CoursePanel.Visible = true;
            //StdXAvgAPanel.Visible = true;
            ViewState["Stream"] = RbCourse.SelectedValue;
        }
      
        
    }
    private void ChkBusHostel(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        ChkBus.Checked = dt.Rows[0]["BusFacility"].ToString() == "Y";
        if (dt.Rows[0]["HostelFacility"].ToString() == "Y")
            ChkHostel.Checked = true;
        else
            ChkHostel.Checked = false;
    }

    protected void GridFill(DataTable dt,DataTable dt1)
    {
        if (dt.Rows.Count > 0)
        {
            grdsiblings.DataSource = dt.DefaultView;
            grdsiblings.DataBind();
            lkaddmore.Visible = true;
            lnkdelete.Visible = true;
            radiosiblingno.Checked = false;
            radiosiblingyes.Checked = true;
            tblsibling.Visible = true;
            ViewState["sibling"] = dt;
        }
        if (dt1.Rows.Count > 0)
        {
            grdsiblings2.DataSource = dt1.DefaultView;
            grdsiblings2.DataBind();
            ChkSibling.Checked = true;
            NewSibPanel.Visible = true;
            lkaddmore1.Visible = true;
            lnkdelete1.Visible = true;
            radiosiblingno.Checked = false;
            radiosiblingyes.Checked = true;
            tblsibling.Visible = true;
            ViewState["siblingOther"] = dt1;
        }
        else
        {
            lnkdelete.Visible = false;
            radiosiblingyes.Checked = false;
        }
    }

    private void generatesibling()
    {
        ViewState["sibling"] = new DataTable()
        {
            Columns = {
        "ClassName",
        "ClassId",
        "SiblingAdmnNo",
        "SiblingNo",
        "FullName"
      }
        };
        ViewState["siblingOther"] = new DataTable()
        {
            Columns = {
        "SchoolName",
        "ClassName",
       
        "SiblingNo",
        "FullName"
      }
        };
        ViewState["Count"] = lblsibcount.Text;
    }

    private void showdata(DataTable tb)
    {
        if (tb.Rows.Count <= 0)
            return;
       PopCalAdmnDt.SetDateValue(Convert.ToDateTime(tb.Rows[0]["AdmnDate"].ToString()));
        Admissions_AddAStudent.sibcount = Convert.ToInt32(tb.Rows[0]["sibcount"]);
        if (Admissions_AddAStudent.sibcount != 0)
        {
            lblsibcount.Visible = true;
            lblsibcount.Text = "(Siblings for this student=" + Admissions_AddAStudent.sibcount.ToString() + ")";
        }
        else
            lblsibcount.Visible = false;
        lblAdmnNo.Text = Request.QueryString["sid"].ToString();
        drpStudType.SelectedValue = tb.Rows[0]["StudType"].ToString();
        ViewState["StudType"] = tb.Rows[0]["StudType"].ToString();
        drpAdmSessYr.SelectedValue = tb.Rows[0]["AdmnSessYr"].ToString();
        txtname.Text = tb.Rows[0]["FullName"].ToString();
        txtnickname.Text = tb.Rows[0]["NickName"].ToString();
        PopCalDOB.SetDateValue(Convert.ToDateTime(tb.Rows[0]["DOB"].ToString()));
        txtPoB.Text = tb.Rows[0]["PlaceOfBirth"].ToString();
        drpGender.SelectedValue = tb.Rows[0]["sex"].ToString();
        drprelgn.SelectedValue = tb.Rows[0]["religion"].ToString();
        drpDenomination.SelectedValue = tb.Rows[0]["Denomination"].ToString();
        drpBloodGroup.SelectedValue = tb.Rows[0]["BloodGroup"].ToString();
        txtnationality.Text = tb.Rows[0]["Nationality"].ToString();
        drpcat.SelectedValue = tb.Rows[0]["Cat"].ToString();
        txtMotherTongue.Text = tb.Rows[0]["MotherTongue"].ToString();
        drpsecondlang.SelectedValue = tb.Rows[0]["SecondLang"].ToString();
        drpLocality.SelectedValue = tb.Rows[0]["Locality"].ToString();
        ViewState["StudImg"] = tb.Rows[0]["StudentPhoto"].ToString();
        ViewState["StudDoc"] = tb.Rows[0]["StudentDoc"].ToString();
        hlDoc.Text = tb.Rows[0]["StudentDoc"].ToString();
        hlDoc.NavigateUrl = "~/Up_Files/Doc/" + tb.Rows[0]["StudentDoc"].ToString();
        imgStud.Src = "https://kemschampua.co.in/Temp_Files/temp_img/" + tb.Rows[0]["StudentPhoto"].ToString();
        //imgStud.ImageUrl = "~/Up_Files/Studimage/" + tb.Rows[0]["StudentPhoto"].ToString();
        txtfathername.Text = tb.Rows[0]["FatherName"].ToString();
        txtmothername.Text = tb.Rows[0]["MotherName"].ToString();
        txtFatEQ.Text = tb.Rows[0]["FatherEQ"].ToString();
        txtMotEQ.Text = tb.Rows[0]["MotherEQ"].ToString();
        txtFathOcc.Text = tb.Rows[0]["FatherOccupation"].ToString();
        txtMothOcc.Text = tb.Rows[0]["MotherOccupation"].ToString();
        txtFatSchlName.Text = tb.Rows[0]["FatherSchool"].ToString();
        txtFatSchlPlc.Text = tb.Rows[0]["FatherSchlPlace"].ToString();
        txtFatClgName.Text = tb.Rows[0]["FatherCollege"].ToString();
        txtFatClgPlc.Text = tb.Rows[0]["FatherClgPlace"].ToString();
        txtFatDesg.Text = tb.Rows[0]["FatherDesig"].ToString();
        txtFatDep.Text = tb.Rows[0]["FatherDept"].ToString();
        txtFatCompName.Text = tb.Rows[0]["FatherComp"].ToString();

        txtMotSchlName.Text = tb.Rows[0]["MotherSchool"].ToString();
        txtMotSchlPlc.Text = tb.Rows[0]["MotherSchlPlace"].ToString();
        txtMotClgName.Text = tb.Rows[0]["MotherCollege"].ToString();
        txtMotClgPlc.Text = tb.Rows[0]["MotherClgPlace"].ToString();
        txtMotDeg.Text = tb.Rows[0]["MotherDesig"].ToString();
        txtMotDep.Text = tb.Rows[0]["MotherDept"].ToString();
        txtMotCompName.Text = tb.Rows[0]["MotherComp"].ToString();
      

        txtLocalGuard.Text = tb.Rows[0]["LocalGuardianName"].ToString();
        txtRelWithLocalGuard.Text = tb.Rows[0]["RelationWithLG"].ToString();
        txtPreAddr1.Text = tb.Rows[0]["PresAddr1"].ToString();
        txtPreAddr2.Text = tb.Rows[0]["PresAddr2"].ToString();
        txtPreDist.Text = tb.Rows[0]["PresAddrDist"].ToString();
        txtPreState.Text = tb.Rows[0]["PresAddrState"].ToString();
        txtPreCountry.Text = tb.Rows[0]["PresAddrCntry"].ToString();
        txtPreCity.Text = tb.Rows[0]["PresAddrCity"].ToString();
        txtPrePin.Text = tb.Rows[0]["PresAddrPin"].ToString();
        txtPrePhone1.Text = tb.Rows[0]["TelNoResidence"].ToString();
        txtPrePhone2.Text = tb.Rows[0]["TeleNoOffice"].ToString();
        txtPreEmail1.Text = tb.Rows[0]["EmailId1"].ToString();
        txtPreEmail2.Text = tb.Rows[0]["EmailId2"].ToString();
        txtPreMob1.Text = tb.Rows[0]["MobileNo1"].ToString();
        txtPreMob2.Text = tb.Rows[0]["MobileNo2"].ToString();

        txtPermAddr1.Text = tb.Rows[0]["PermAddr1"].ToString();
        txtPermAddr2.Text = tb.Rows[0]["PermAddr2"].ToString();
        txtPermDist.Text = tb.Rows[0]["PermAddrDist"].ToString();
        txtPermState.Text = tb.Rows[0]["PermAddrDist"].ToString();
        txtPermCountry.Text = tb.Rows[0]["PermAddrCntry"].ToString();
        txtPermCity.Text = tb.Rows[0]["PermAddrCity"].ToString();
        txtPermPin.Text = tb.Rows[0]["PermAddrPin"].ToString();
        txtschoolname.Text = tb.Rows[0]["PrevSchoolname"].ToString();
        drpclass.SelectedValue = tb.Rows[0]["Prevclass"].ToString();
        txtprevmedium.Text = tb.Rows[0]["MediumOfInst"].ToString();
        txtTCNo.Text = tb.Rows[0]["TCNo"].ToString();
        if (tb.Rows[0]["TCDate"].ToString() != string.Empty)
            PopCalTCDate.SetDateValue(Convert.ToDateTime(tb.Rows[0]["TCDate"].ToString()));
        else
            txtTCDt.Text = "";
        txtgames.Text = tb.Rows[0]["SportsProf"].ToString();
        txthobbies.Text = tb.Rows[0]["Hobbies"].ToString();
        txtadmsnno.Text = tb.Rows[0]["oldAdmnNo"].ToString();
        drpcat.SelectedValue = Convert.ToString(tb.Rows[0]["cat"].ToString());
        drprelgn.SelectedValue = Convert.ToString(tb.Rows[0]["religion"].ToString());
        drpGender.SelectedValue = Convert.ToString(tb.Rows[0]["sex"].ToString());
        txtadmsnno.Text = tb.Rows[0]["oldAdmnNo"].ToString();
        drpSession.SelectedValue = tb.Rows[0]["SessionYear"].ToString();
        ViewState["SessYrBeforeMod"] = tb.Rows[0]["SessionYear"].ToString();
        drpnewclass.SelectedValue = Convert.ToString(tb.Rows[0]["Classidstream"].ToString());
        FillOfficeOptional();
        ViewState["ClassBeforeMod"] = tb.Rows[0]["Classidstream"].ToString();
        tb.Rows[0]["section"].ToString();
        drpsection.SelectedValue = tb.Rows[0]["section"].ToString();
        txtrollno.Text = tb.Rows[0]["RollNo"].ToString();
        txtStudAdhar.Text = tb.Rows[0]["StudAdhar"].ToString();
        txtFatAdhar.Text = tb.Rows[0]["FatherAdhar"].ToString();
        txtMotAdhar.Text = tb.Rows[0]["MotherAdhar"].ToString();
        txtBank.Text = tb.Rows[0]["BankName"].ToString();
        txtEmpBankAcNo.Text = tb.Rows[0]["BankAcNo"].ToString();
        txtIFSC.Text = tb.Rows[0]["IFSCCode"].ToString();
        txtBranch.Text = tb.Rows[0]["Branch"].ToString();
        //txtEng1st.Text = tb.Rows[0]["Engfirst"].ToString();
        txtEng2nd.Text=tb.Rows[0]["Engsecond"].ToString();
        txtEng3rd.Text=tb.Rows[0]["Engthird"].ToString();
        txtEngTot.Text = tb.Rows[0]["Engtotal"].ToString();
        txtEAC1st.Text = tb.Rows[0]["EACfirst"].ToString();
        txtEAC2nd.Text = tb.Rows[0]["EACsecond"].ToString();
        txtEAC3rd.Text = tb.Rows[0]["EACthird"].ToString();
        txtEACtot.Text = tb.Rows[0]["EACtotal"].ToString();
        ViewState["RollNoBeforeMod"] = tb.Rows[0]["RollNo"].ToString();
        ViewState["FeeStDate"] = tb.Rows[0]["FeestartDate"].ToString();
        drpSession.Enabled = false;

       // drpHouse.SelectedValue = tb.Rows[0]["HouseID1"].ToString();

        if (tb.Rows[0]["Stream"].ToString() != "1")
        {
            CoursePanel.Visible = true;
            FillStream();
          
            RbCourse.SelectedValue = tb.Rows[0]["Stream"].ToString();
            FillCourse(Convert.ToInt32(tb.Rows[0]["Stream"].ToString()));
            Filloptional(Convert.ToInt32(tb.Rows[0]["Stream"].ToString()));
            ViewState["Stream"] = tb.Rows[0]["Stream"].ToString();
            RbOptional.SelectedValue = tb.Rows[0]["FifthOptional"].ToString();
            if (tb.Rows[0]["SixthOptional"].ToString() != string.Empty && Convert.ToInt32(tb.Rows[0]["SixthOptional"].ToString()) != 0)
            {
                ChksixthSub.Checked = true;
            }
        }
        else
        {
            if (tb.Rows[0]["SixthOptional"].ToString() != string.Empty)
            {
                drpSixthSubject.SelectedValue = tb.Rows[0]["SixthOptional"].ToString().Trim();
            }
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblMsgBottom.Text = "";
        lblMsgTop.Text = "";
        ViewState["Admn"] = null;
        ViewState["classid"] = null;
        ViewState["SessYr"] = null;
        if (new clsDAL().ExecuteScalar("ps_spCheckFeeAmount", new Hashtable()
    {
      {
         "@SessionYr",
         drpSession.SelectedValue.Trim()
      },
      {
         "@ClassId",
         drpnewclass.SelectedValue.Trim()
      },
      {
         "@StudType",
         drpStudType.SelectedValue.Trim()
      }
    }).Trim() == "S")
        {
            SaveRecord();
        }
        else
        {
            lblMsgBottom.Text = "Please define fee amount for current session before admission!!";
            lblMsgTop.Text = "Please define fee amount for current session before admission!!";
            lblMsgBottom.ForeColor = Color.Red;
            lblMsgTop.ForeColor = Color.Red;
        }
     }

    protected void SaveRecord()
    {
        if (txtdob.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock((Control)txtdob, txtdob.GetType(), "ShowMassage", "alert('Enter Date of Birth');", true);
            txtdob.Focus();
        }
        else if (Request.QueryString["sid"] != null)
        {
            if (ViewState["RollNoBeforeMod"].ToString().Trim() != txtrollno.Text.Trim())
            {
                if (Convert.ToInt32(new clsDAL().ExecuteScalarQry("select count(*) from dbo.PS_ClasswiseStudent where SessionYear='" + drpSession.SelectedValue + "' and ClassID=" + drpnewclass.SelectedValue + " and Section='" + drpsection.SelectedValue + "' and RollNo=" + txtrollno.Text.Trim())) > 0)
                {
                    ScriptManager.RegisterClientScriptBlock((Control)txtrollno, txtrollno.GetType(), "ShowMassage", "alert('The allotted roll number already exists');", true);
                    return;
                }
            }
            if (ViewState["SessYrBeforeMod"].ToString().Trim() != drpSession.SelectedValue.ToString().Trim() || ViewState["ClassBeforeMod"].ToString().Trim() != drpnewclass.SelectedValue.ToString().Trim() || ViewState["StudType"].ToString().Trim() != drpStudType.SelectedValue.ToString().Trim())
            {
                if (Convert.ToInt32(new clsDAL().ExecuteScalarQry("select count(*) from dbo.Acts_PaymentReceiptVoucher where PartyId=" + Request.QueryString["sid"].ToString() + " and (IsDeleted is null or IsDeleted=0)")) > 0)
                {
                    lblMsgBottom.Text = "Session ,Class or Type cann't be modified now. Fee already received from this student.";
                    lblMsgTop.Text = "Session and Class or Type cann't be modified now. Fee already received from this student.";
                    lblMsgBottom.ForeColor = Color.Red;
                    lblMsgTop.ForeColor = Color.Red;
                }
                else
                {
                    InsertStudDetails();
          //          new clsDAL().ExcuteProcInsUpdt("ps_sp_delete_Ledger", new Hashtable()
          //{
          //  {
          //     "AdmnNo",
          //     Request.QueryString["sid"]
          //  }
          //});
                   // InsertDetailsInLedger();
                    lblMsgBottom.Text = "Record Updated Successfully";
                    lblMsgTop.Text = "Record Updated Successfully";
                    lblMsgBottom.ForeColor = Color.Green;
                    lblMsgTop.ForeColor = Color.Green;
                   
                }
            }
            else
            {
                InsertStudDetails();
                lblMsgBottom.Text = "Record Updated Successfully";
                lblMsgTop.Text = "Record Updated Successfully";
                               
            }
            //** Required Changes - to redirect to search page  ----1/3/2019
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Record Updated Succesfully'); window.location = '../Admissions/SearchStudent.aspx';", true);
           // Response.Redirect("SearchStudent.aspx");
        }
        else
        {
            try
            {
                InsertStudDetails();
                if (chkGenFee.Checked)
                   // InsertDetailsInLedger();
                clear();
                lblMsgBottom.Text = "Student record inserted successfully with Admission No: " + ViewState["Admn"].ToString();
                lblMsgTop.Text = "Student record inserted successfully with Admission No: " + ViewState["Admn"].ToString();
                lblMsgBottom.ForeColor = Color.Green;
                lblMsgTop.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                lblMsgTop.Text = ex.Message.ToString();
                lblMsgBottom.Text = ex.Message.ToString();
                lblMsgBottom.ForeColor = Color.Red;
                lblMsgTop.ForeColor = Color.Red;
            }
        }
    }

    private void InsertStudDetails()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (Request.QueryString["sid"] != null)
            hashtable.Add("AdmnNo", Request.QueryString["sid"]);
       
        PopCalAdmnDt.GetDateValue().ToString("MM/dd/yyyy");
        hashtable.Add("AdmnDate", PopCalAdmnDt.GetDateValue());
        hashtable.Add("FullName", txtname.Text.Trim());
        hashtable.Add("NickName", "");
        hashtable.Add("@DOB", PopCalDOB.GetDateValue());
        hashtable.Add("@POB",txtPoB.Text.Trim());
        hashtable.Add("@MotherTongue", txtMotherTongue.Text.Trim());
      
        hashtable.Add("@Locality", "");
        hashtable.Add("@FatherName", txtfathername.Text.Trim());
        hashtable.Add("@FatherOccupation", txtFathOcc.Text.Trim());
        hashtable.Add("@MotherName", txtmothername.Text.Trim());
        hashtable.Add("@MotherOccupation", txtMothOcc.Text.Trim());
        hashtable.Add("@LocalGuardianName", txtLocalGuard.Text.Trim());
        hashtable.Add("@RelationWithLG", txtRelWithLocalGuard.Text.Trim());
        hashtable.Add("@PresentAddress1", txtPreAddr1.Text.Trim());
        hashtable.Add("@PresentAddress2", txtPreAddr2.Text.Trim());
        hashtable.Add("@PresAddrDist", txtPreDist.Text.Trim());
        hashtable.Add("@PresAddrState", txtPreState.Text.Trim());
        hashtable.Add("@PresAddrCon", txtPreCountry.Text.Trim());
        hashtable.Add("@PresAddrPin", txtPrePin.Text.Trim());
        hashtable.Add("@PresAddrCity", txtPreCity.Text.Trim());
        hashtable.Add("@PermAddress1", txtPermAddr1.Text.Trim());
        hashtable.Add("@PermAddress2", txtPermAddr2.Text.Trim());
        hashtable.Add("@PermAddrDist", txtPermDist.Text.Trim());
        hashtable.Add("@PermAddrState", txtPermState.Text.Trim());
        hashtable.Add("@PermAddrCon", txtPermCountry.Text.Trim());

        hashtable.Add("@PermAddrPin", txtPermPin.Text.Trim());
        hashtable.Add("@PermAddrCity", txtPermCity.Text.Trim());
        hashtable.Add("@TelNoResidence", txtPrePhone1.Text.Trim());
        hashtable.Add("@TeleNoOffice", txtPrePhone2.Text.Trim());
        hashtable.Add("@Mobile1", txtPreMob1.Text.Trim());
        hashtable.Add("@Mobile2", txtPreMob2.Text.Trim());
        hashtable.Add("@EmailId1", txtPreEmail1.Text.Trim());
        hashtable.Add("@EmailId2", txtPreEmail2.Text.Trim());
        hashtable.Add("@Nationality", txtnationality.Text.Trim());
        hashtable.Add("@Class", drpnewclass.SelectedValue);
        hashtable.Add("@SportsProf", "");
        hashtable.Add("@Hobbies", "");
        hashtable.Add("@Cat", drpcat.SelectedValue);
        hashtable.Add("@TribeName", txtTribeName.Text.Trim());
        hashtable.Add("@StudAdhar", txtStudAdhar.Text.Trim());
        hashtable.Add("@FatherAdhar", txtFatAdhar.Text.Trim());

        hashtable.Add("@FatherSchl", txtFatSchlName.Text.Trim());
        hashtable.Add("@FatherSchlPlc", txtFatSchlPlc.Text.Trim());
        hashtable.Add("@FatherClg", txtFatClgName.Text.Trim());
        hashtable.Add("@FatherClgPlc",txtFatClgPlc.Text.Trim());
        hashtable.Add("@FatherDesg",txtFatDesg.Text.Trim());
        hashtable.Add("@FatherDept",txtFatDep.Text.Trim());
        hashtable.Add("@FatherComp",txtFatCompName.Text.Trim());
        hashtable.Add("@FatherEQ", txtFatEQ.Text.Trim());

        hashtable.Add("@MotherEQ", txtMotEQ.Text.Trim());
        hashtable.Add("@MotherSchl", txtMotSchlName.Text.Trim());
        hashtable.Add("@MotherSchlPlc", txtMotSchlPlc.Text.Trim());
        hashtable.Add("@MotherClg", txtMotClgName.Text.Trim());
        hashtable.Add("@MotherClgPlc", txtMotClgPlc.Text.Trim());
        hashtable.Add("@MotherDesg",txtMotDeg.Text.Trim());
        hashtable.Add("@MotherDept",txtMotDep.Text.Trim());
        hashtable.Add("@MotherComp", txtMotCompName.Text.Trim());
        hashtable.Add("@MotherAdhar", txtMotAdhar.Text.Trim());
        hashtable.Add("@BankName", txtBank.Text.Trim());
        hashtable.Add("@BankAcNo", txtEmpBankAcNo.Text.Trim());
        hashtable.Add("@IFSCCode", txtIFSC.Text.Trim());
        hashtable.Add("@Branch", txtBranch.Text.Trim());

        //if (ViewState["Stream"].ToString() == "1")
        //{
        //    hashtable.Add("@EngFirst", 0.00);
        //    hashtable.Add("@EngSecond", 0.00);
        //    hashtable.Add("@EngThird", 0.00);
        //    hashtable.Add("@EngTotal", 0.00);
        //    hashtable.Add("@EACFirst", 0.00);
        //    hashtable.Add("@EACSecond", 0.00);
        //    hashtable.Add("@EACThird", 0.00);
        //    hashtable.Add("@EACTotal", 0.00);
        //}
        //else
        //{
          //  double D=
            //hashtable.Add("@EngFirst", Math.Round((Convert.ToDouble(txtEng1st.Text.Trim())),2));
            //hashtable.Add("@EngSecond", Math.Round((Convert.ToDouble(txtEng2nd.Text.Trim())), 2));
            //hashtable.Add("@EngThird", Math.Round((Convert.ToDouble(txtEng3rd.Text.Trim())), 2));
            //hashtable.Add("@EngTotal", Math.Round((Convert.ToDouble(txtEngTot.Text.Trim())), 2));
            //hashtable.Add("@EACFirst", Math.Round((Convert.ToDouble(txtEAC1st.Text.Trim())), 2));
            //hashtable.Add("@EACSecond",Math.Round((Convert.ToDouble( txtEAC2nd.Text.Trim())), 2));
            //hashtable.Add("@EACThird", Math.Round((Convert.ToDouble(txtEAC3rd.Text.Trim())), 2));
            //hashtable.Add("@EACTotal", Math.Round((Convert.ToDouble(txtEACtot.Text.Trim())), 2));
        //}
        string Stream = ViewState["Stream"].ToString();

        if(Request.QueryString["FormType"]=="0")
            hashtable.Add("@Stream", 1);
        else if (Request.QueryString["FormType"] == "2")
            hashtable.Add("@Stream", 1);
        else
        hashtable.Add("@Stream", Convert.ToInt32(ViewState["Stream"].ToString()));

        
       // hashtable.Add("@SecondLang", drpsecondlang.SelectedItem.Text.Trim());
      //  hashtable.Add("@FifthOptional", RbOptional.SelectedValue);
        //if (Convert.ToInt32(drpnewclass.SelectedValue) < 13 && Convert.ToInt32(drpnewclass.SelectedValue) != 1)
        //{
        //    hashtable.Add("@SixthOptional", ViewState["SixthOptional"].ToString());
        //}
        //else
        //{
        //    if (ChksixthSub.Checked || drpSixthSubject.SelectedIndex > 0)
        //    {
        //        hashtable.Add("@SixthOptional", ViewState["SixthOptional"].ToString());
        //    }
        //}

        hashtable.Add("@HouseID", 1);

        if (Request.QueryString["status"] != null)
        {
            hashtable.Add("@Status", Request.QueryString["status"].ToString());
        }
        else
             hashtable.Add("@Status", 1);
        DateTime dateTime;
        if (Request.QueryString["sid"] != null && ViewState["FeeStDate"].ToString() != string.Empty)
        {
            dateTime = DateTime.Parse(ViewState["FeeStDate"].ToString());
        }
        else
        {
            string str1 = drpFeeStartFrom.SelectedValue.ToString();
            string str2 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
            string str3 = drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
            dateTime = Convert.ToInt32(str1) <= 3 ? Convert.ToDateTime(str1 + "/01/" + str3) : Convert.ToDateTime(str1 + "/01/" + str2);
        }
        hashtable.Add("@FeestartDate", dateTime);
        hashtable.Add("@sex", drpGender.SelectedValue);
        hashtable.Add("@religion", drprelgn.SelectedValue);
        hashtable.Add("@Denomination", drpDenomination.SelectedValue);
        hashtable.Add("@OldAdmnNo", txtadmsnno.Text.Trim());

        if (fldUpImage.HasFile)
        {
            
            string str = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(fldUpImage.FileName);
            hashtable.Add("StudentPhoto", str);
            fldUpImage.SaveAs(Server.MapPath("~/Up_Files/Studimage/" + str));
        }
        else if (ViewState["StudImg"].ToString() != "")
            hashtable.Add("StudentPhoto", ViewState["StudImg"].ToString());
        else
            hashtable.Add("StudentPhoto", "NoImage.jpg");
        if (fldUpDoc.HasFile)
        {
            string str = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(fldUpDoc.FileName);
            hashtable.Add("@StudentDoc", str);
            fldUpDoc.SaveAs(Server.MapPath("~/Up_Files/Doc/" + str));
        }
        else if (ViewState["StudDoc"] != null && ViewState["StudDoc"].ToString() != "")
            hashtable.Add("@StudentDoc", ViewState["StudDoc"].ToString());
        else
            hashtable.Add("@StudentDoc", "");
        hashtable.Add("@UserID", Session["User_Id"].ToString().Trim());
        hashtable.Add("@SchoolID", Session["SchoolId"].ToString().Trim());
        hashtable.Add("@MediumOfInst", txtprevmedium.Text.Trim());
        hashtable.Add("@ClassJoinDate", PopCalAdmnDt.GetDateValue());
        hashtable.Add("@Detained_Promoted", "");
        hashtable.Add("@Grade", "");
        hashtable.Add("@PrevSchoolname", txtschoolname.Text.Trim());
        hashtable.Add("@SessionYear", drpSession.SelectedValue.ToString().Trim());
        hashtable.Add("@ClassID", drpnewclass.SelectedValue);
        if (drpsection.SelectedValue.ToString() == "Not Allotted")
            hashtable.Add("@Section", "");
        else
            hashtable.Add("@Section", drpsection.SelectedValue.ToString());
        hashtable.Add("@RollNo", txtrollno.Text.Trim());
        hashtable.Add("@PrevClass", drpclass.SelectedValue);
        hashtable.Add("@TCNo", txtTCNo.Text);
        if (!txtTCDt.Text.Trim().Equals(string.Empty))
            hashtable.Add("TCDate", PopCalTCDate.GetDateValue());
        else
            hashtable.Add("TCDate", null);
        if (drpStudType.SelectedValue.ToString().Trim() == "N")
            hashtable.Add("@AdmnSessYr", drpSession.SelectedValue.ToString().Trim());
        else
            hashtable.Add("@AdmnSessYr", drpAdmSessYr.SelectedValue.ToString().Trim());
        hashtable.Add("@StudType", drpStudType.SelectedValue.ToString().Trim());
        if (Request.QueryString["sid"] == null)
        {
            hashtable.Add("feeconssionper", txtFeeConcession.Text.Trim());
            hashtable.Add("authority", "Principal");
            hashtable.Add("reason", txtConceReason.Text);
        }
        if (ChkBus.Checked.Equals(true))
            hashtable.Add("@BusFacility", "Y");
        else
            hashtable.Add("@BusFacility", "N");
        if (ChkHostel.Checked.Equals(true))
            hashtable.Add("@HostelFacility", "Y");
        else
            hashtable.Add("@HostelFacility", "N");
        ViewState["Admn"] = string.Empty;
        hashtable.Add("@BloodGroup", drpBloodGroup.SelectedValue.ToString());
        //hashtable.Add("@HouseDetailID", drpHouse.SelectedValue);
        DataTable dataTable = clsDal.GetDataTable("insert_Studentinfo", hashtable);
        if (dataTable.Rows.Count > 0)
            ViewState["Admn"] = dataTable.Rows[0]["AdmnNo"].ToString();
        else
            ViewState["Admn"] = Request.QueryString["sid"].ToString();
        if (Request.QueryString["sid"] != null)
        {
            new clsDAL().ExcuteQryInsUpdt("delete PS_SiblingDetails where admnno=" + Request.QueryString["sid"]);
            InsertSibling();
            InsertSiblingOther();
            lkaddmore.Visible = false;
            lnkdelete.Visible = false;
            radiosiblingno.Checked = true;
            grdsiblings.DataSource = null;
            grdsiblings.DataBind();
            tblsibling.Visible = false;
            lblsibcount.Visible = false;
        }
        else
        {
            InsertSibling();
            InsertSiblingOther();
            InsertConcession();
        }
    }

    private void InsertConcession()
    {
        int num;
        try
        {
            num = Convert.ToInt32(txtFeeConcession.Text);
        }
        catch
        {
            num = 0;
        }
        new clsDAL().ExcuteProcInsUpdt("ps_sp_insert_FeeConcession", new Hashtable()
    {
      {
         "adminno",
         ViewState["Admn"].ToString()
      },
      {
         "session",
         drpSession.SelectedValue.ToString()
      },
      {
         "feeconssionper",
         num
      },
      {
         "fineconssionper",
         "0"
      },
      {
         "authority",
         "Principal"
      },
      {
         "reason",
         txtConceReason.Text
      },
      {
         "UserID",
         Convert.ToInt32(Session["User_Id"])
      },
      {
         "UserDate",
         DateTime.Now.ToString("MM/dd/yyyy")
      },
      {
         "schoolid",
         Session["SchoolId"].ToString()
      }
    });
    }

    private void InsertDetailsInLedger()
    {
        //InsertDetailsForBusFee();
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        string str1;
        string str2;
        DateTime dateTime;
        if (Request.QueryString["sid"] != null)
        {
            str1 = "Y";
            str2 = "Y";
            dateTime = Convert.ToDateTime(ViewState["FeeStDate"].ToString());
        }
        else
        {
            str1 = !chkOT.Checked ? "N" : "Y";
            str2 = !chkAnnual.Checked ? "N" : "Y";
            PopCalAdmnDt.GetDateValue();
            string str3 = drpFeeStartFrom.SelectedValue.ToString();
            string str4 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
            string str5 = drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
            dateTime = Convert.ToInt32(str3) <= 3 ? Convert.ToDateTime(str3 + "/01/" + str5) : Convert.ToDateTime(str3 + "/01/" + str4);
        }
        clsGenerateFee.GenerateFeeOnAdmission(PopCalAdmnDt.GetDateValue(), dateTime, ViewState["Admn"].ToString(), str1, str2, drpSession.SelectedValue.ToString().Trim(), Session["User_Id"].ToString(), Session["SchoolId"].ToString(), char.Parse(drpStudType.SelectedValue));
        btnPrintReceipt1.Enabled = true;
        ViewState["classid"] = drpnewclass.SelectedValue;
        ViewState["SessYr"] = drpSession.SelectedValue.ToString();
        clear();
    }

    private void InsertDetailsForBusFee()
    {
        new clsDAL().ExcuteProcInsUpdt("ps_sp_insert_AdFeeLedgerBus", new Hashtable()
    {
      {
         "Admnno",
        ViewState["Admn"]
      },
      {
         "uid",
         Convert.ToInt32(Session["User_Id"])
      },
      {
         "Session",
         drpSession.SelectedValue.ToString().Trim()
      },
      {
         "SchoolId",
         Session["SchoolId"].ToString().Trim()
      }
    });
    }

    private void InsertSibling()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = ViewState["sibling"] as DataTable;
        if (dataTable == null)
            return;
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                clsDal.ExcuteProcInsUpdt("ps_sp_insert_Sibling", new Hashtable()
        {
          {
             "admnno",
             ViewState["Admn"].ToString()
          },
          {
             "UserID",
             Session["User_Id"].ToString().Trim()
          },
          {
             "SchoolID",
             Session["SchoolId"].ToString().Trim()
          },
          {
             "SiblingAdmnNo",
             row["SiblingAdmnNo"].ToString()
          },
          {
             "SiblingClass",
             row["ClassId"].ToString()
          },
          {
             "SiblingNo",
             row["SiblingNo"].ToString()
          }
        });
        }
        else
        {
            lkaddmore.Visible = false;
            lnkdelete.Visible = false;
            radiosiblingno.Checked = true;
            grdsiblings.DataSource = null;
            grdsiblings.DataBind();
            tblsibling.Visible = false;
            lblsibcount.Visible = false;
        }
    }
    private void InsertSiblingOther()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = ViewState["siblingOther"] as DataTable;
        if (dataTable == null)
            return;
        if (dataTable.Rows.Count > 0)
        {
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                clsDal.ExcuteProcInsUpdt("ps_sp_insert_SiblingOther", new Hashtable()
        {
          {
             "admnno",
             ViewState["Admn"].ToString()
          },
          {
             "UserID",
             Session["User_Id"].ToString().Trim()
          },
          {
             "SiblingSchool",
              row["SchoolName"].ToString().Trim()
          },
          {
             "SiblingName",
            row["FullName"].ToString()
          },
          {
             "SiblingClass",
             row["ClassName"].ToString()
          },
          {
             "SiblingNo",
              row["SiblingNo"].ToString()
          }
        });
        }
        else
        {
            lkaddmore1.Visible = false;
            lnkdelete1.Visible = false;
            radiosiblingno.Checked = true;
            grdsiblings2.DataSource = null;
            grdsiblings2.DataBind();
            tblsibling.Visible = false;
            lblsibcount.Visible = false;
        }
    }

    protected void radiosiblingyes_CheckedChanged(object sender, EventArgs e)
    {
        tblsibling.Visible = true;
        lkaddmore.Visible = true;
        lnkdelete.Visible = false;
        generatesibling();
    }

    protected void lkaddmore_Click(object sender, EventArgs e)
    {
        Fillsibling();
        drpsiblingclass.SelectedIndex = 0;
        drpsiblingadminno.SelectedIndex = 0;
    }

    private DataTable Fillsibling()
    {
       
        DataTable dataTable = ViewState["sibling"] as DataTable;
        if (dataTable == null)
            return dataTable;
        if (drpsiblingadminno.SelectedIndex > 0)
        {
            DataRow row = dataTable.NewRow();
            clsDAL clsDal = new clsDAL();
            string str1 = "select ClassName from dbo.PS_ClassMaster where ClassID=" + drpsiblingclass.SelectedValue.ToString().Trim();
            row["ClassName"] = clsDal.ExecuteScalarQry(str1);
            row["classId"] = drpsiblingclass.SelectedValue;
            row["SiblingAdmnNo"] = drpsiblingadminno.SelectedValue.ToString();
            row["SiblingNo"] = Convert.ToString(dataTable.Rows.Count + 1);
            string str2 = "select FullName from dbo.PS_StudMaster where AdmnNo=" + drpsiblingadminno.SelectedValue.ToString().Trim();
            row["FullName"] = clsDal.ExecuteScalarQry(str2);
            dataTable.Rows.Add(row);
            ViewState["sibling"] = dataTable;
            if (ViewState["Count"].ToString() == string.Empty)
            {
                ViewState["Count"] = "0";
            }
            string str = ViewState["Count"].ToString();
            ViewState["Count"] = Convert.ToString(Convert.ToInt32(ViewState["Count"]) + 1);

            lblsibcount.Text = " (Siblings Added:" + ViewState["Count"].ToString() + ")";
        }
        
       // lblsibcount.Text = " (Siblings Added:" + dataTable.Rows.Count.ToString() + ")";
        if (dataTable.Rows.Count > 0)
        {
            grdsiblings.DataSource = dataTable;
            grdsiblings.DataBind();
            grdsiblings.Visible = true;
            lkaddmore.Visible = true;
            lnkdelete.Visible = true;
        }
        return dataTable;
    }

    protected void radiosiblingno_CheckedChanged(object sender, EventArgs e)
    {
        ViewState["sibling"] = null;
        tblsibling.Visible = false;
        lkaddmore.Visible = false;
        lnkdelete.Visible = false;
        if (Admissions_AddAStudent.sibcount != 0)
            return;
        lblsibcount.Text = "";
    }

    private void clear()
    {
        PopCalAdmnDt.SetDateValue(DateTime.Now);
        txtadmsnno.Text = string.Empty;
        txtname.Text = string.Empty;
        txtnickname.Text = string.Empty;
        txtdob.Text = string.Empty;
        txtPoB.Text = string.Empty;
        txtfathername.Text = string.Empty;
        txtmothername.Text = string.Empty;
        txtFathOcc.Text = string.Empty;
        txtMothOcc.Text = string.Empty;
        txtLocalGuard.Text = string.Empty;
        txtRelWithLocalGuard.Text = string.Empty;
        txtnationality.Text = "Indian";
        txtMotherTongue.Text = "Oriya";
        txtTribeName.Text = string.Empty;
        txtschoolname.Text = string.Empty;
        drpclass.SelectedIndex = 0;
        txtprevmedium.Text = string.Empty;
        drpnewclass.SelectedIndex = 0;
        txtgames.Text = string.Empty;
        txthobbies.Text = string.Empty;
        txtadmsnno.Text = string.Empty;
        drpsiblingclass.SelectedIndex = 0;
        txtrollno.Text = "0";
        drpsection.SelectedIndex = 0;
        txtPreAddr1.Text = string.Empty;
        txtPreAddr2.Text = string.Empty;
        txtPreDist.Text = string.Empty;
        txtPreState.Text = string.Empty;
        txtPreCountry.Text = string.Empty;
        txtPreCity.Text = string.Empty;
        txtPrePin.Text = string.Empty;
        txtPrePhone1.Text = string.Empty;
        txtPrePhone2.Text = string.Empty;
        txtPreEmail1.Text = string.Empty;
        txtPermAddr1.Text = string.Empty;
        txtPermAddr2.Text = string.Empty;
        txtPermDist.Text = string.Empty;
        txtPermState.Text = string.Empty;
        txtPermCountry.Text = string.Empty;
        txtPermPin.Text = string.Empty;
        txtPermCity.Text = string.Empty;
        txtTCNo.Text = string.Empty;
        txtTCDt.Text = string.Empty;
        txtStudAdhar.Text = string.Empty;
        txtFatAdhar.Text = string.Empty;
        txtFatSchlName.Text = string.Empty;
        txtFatSchlPlc.Text = string.Empty;
        txtFatClgName.Text = string.Empty;
        txtFatClgPlc.Text = string.Empty;
        txtFatCompName.Text = string.Empty;
        txtFatDesg.Text = string.Empty;
        txtFatDep.Text = string.Empty;
        txtMotSchlName.Text = string.Empty;
        txtMotSchlPlc.Text = string.Empty;
        txtMotClgName.Text = string.Empty;
        txtMotClgPlc.Text = string.Empty;
        txtMotCompName.Text = string.Empty;
        txtMotDeg.Text = string.Empty;
        txtMotDep.Text = string.Empty;
        txtMotAdhar.Text = string.Empty;
        txtBank.Text = string.Empty;
        txtEmpBankAcNo.Text = string.Empty;
        txtIFSC.Text = string.Empty;
        txtBranch.Text = string.Empty;
        txtFeeConcession.Text = "0";
        txtConceReason.Text = string.Empty;
       
        //string b = Request.QueryString["FormType"].ToString();
        if ( Request.QueryString["FormType"] != null && Request.QueryString["FormType"] !="0" && Request.QueryString["FormType"] != "2")
        {
            RbCourse.SelectedIndex = 0;
            RbOptional.SelectedIndex = 0;
            ChksixthSub.Visible = false;
            txtEAC1st.Text = txtEAC2nd.Text = txtEAC3rd.Text = txtEACtot.Text = string.Empty;
            txtEAC1st.Text = txtEAC2nd.Text = txtEAC3rd.Text = txtEACtot.Text = string.Empty;
       
        }
        else if (Request.QueryString["Class"] != null)
        {
            if (Convert.ToInt32(Request.QueryString["Class"].ToString()) > 12)
            {
                RbCourse.SelectedIndex = 0;
                RbOptional.SelectedIndex = 0;
                ChksixthSub.Visible = false;
                txtEAC1st.Text = txtEAC2nd.Text = txtEAC3rd.Text = txtEACtot.Text = string.Empty;
                txtEAC1st.Text = txtEAC2nd.Text = txtEAC3rd.Text = txtEACtot.Text = string.Empty;
            }
        }
        txtPreMob1.Text = string.Empty;
        txtPreMob2.Text = string.Empty;
        txtPreEmail1.Text = txtPreEmail2.Text = string.Empty;
        lblAdmnNo.Text = string.Empty;
        imgStud.Src = "~/images/student.png";
       

        
    }

    protected void lnkdelete_Click(object sender, EventArgs e)
    {
        DataTable dataTable = ViewState["sibling"] as DataTable;
        clsDAL clsDal = new clsDAL();
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)lnkdelete, lnkdelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str1 = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str1.Split(chArray))
            {
                string str2 = obj.ToString();
                foreach (DataRow dataRow in dataTable.Select("SiblingNo=" + str2))
                    dataRow.Delete();
                    dataTable.AcceptChanges();
              ViewState["Count"] = Convert.ToString(Convert.ToUInt32(ViewState["Count"]) - 1);
            }
            ViewState["sibling"] = dataTable;
            grdsiblings.DataSource = dataTable.DefaultView;
            lblsibcount.Text = " (Siblings Added:" + ViewState["Count"].ToString() + ")";
            grdsiblings.DataBind();
        }
    }

    protected void drpsiblingclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillsiblingadminno();
    }

    protected void fillsiblingadminno()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        if (drpsiblingclass.SelectedIndex != 0)
            dataTable = clsDal.GetDataTableQry("select distinct c.admnno,s.FullName + '('+ Convert(varchar,c.admnno) + ')' as FullName from  dbo.PS_ClasswiseStudent c inner join PS_StudMaster s on  s.admnno=c.admnno where classid=" + drpsiblingclass.SelectedValue + " order by FullName ");
        drpsiblingadminno.DataSource = dataTable.DefaultView;
        drpsiblingadminno.DataTextField = "FullName";
        drpsiblingadminno.DataValueField = "admnno";
        drpsiblingadminno.DataBind();
        drpsiblingadminno.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    protected void FillSiblingForEdit(object sender, EventArgs e)
    {
        GridViewRow parent = (GridViewRow)(sender as LinkButton).Parent.Parent;
        Label control1 = (Label)parent.FindControl("lbladmnno");
        Label control2 = (Label)parent.FindControl("lblclassId");
        string text1 = control1.Text;
        string text2 = control2.Text;
        drpsiblingclass.SelectedValue = control2.Text;
        fillsiblingadminno();
        drpsiblingadminno.SelectedValue = control1.Text;
        tblsibling.Visible = true;
    }

    protected void btnGoToList_Click(object sender, EventArgs e)
    {
        Response.Redirect("showstudents.aspx");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
        ViewState["Admn"] = null;
        ViewState["classid"] = null;
        ViewState["SessYr"] = null;
        Admissions_AddAStudent.sibcount = 0;
        radiosiblingno.Checked = true;
        lblMsgTop.Text = "";
        lblMsgBottom.Text = "";
        UpdatePanel1.Update();
       // UpdatePanel2.Update();
    }

    protected void btnPrintReceipt1_Click(object sender, EventArgs e)
    {
        if (!(ViewState["Admn"].ToString().Trim() != ""))
            return;
        //"../FeeManagement/feercptcash.aspx?admnno=" + ViewState["Admn"].ToString() + "&cid=" + ViewState["classid"].ToString() + "&sess=" + ViewState["SessYr"].ToString();
        Response.Redirect("../FeeManagement/feercptcash.aspx?admnno=" + ViewState["Admn"].ToString() + "&cid=" + ViewState["classid"].ToString() + "&sess=" + ViewState["SessYr"].ToString());
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("SearchStudent.aspx");
    }
    protected void btnTC_Click(object sender, EventArgs e)
    {
        Response.Redirect("StudentWithdrawal.aspx?admnno=" + Request.QueryString["sid"]+"&cid="+drpnewclass.SelectedValue+"&sess="+drpSession.SelectedValue+"");
    }

    protected void btnSetAddonFacility_Click(object sender, EventArgs e)
    {
        if (!(ViewState["Admn"].ToString().Trim() != ""))
            return;
        ViewState["Admn"].ToString().Trim();
        ViewState["SessYr"].ToString();
        ViewState["classid"].ToString();
        Response.Redirect("AddonFacility.aspx?admnno=" + ViewState["Admn"].ToString() + "&sess=" + ViewState["SessYr"].ToString() + "&cid=" + ViewState["classid"].ToString());
    }
    protected void btnPreview_Click(object sender, EventArgs e)
    {

        var name = fldUpImage.FileName;
        if (fldUpImage.HasFile)
        {
            Stream strm = fldUpImage.PostedFile.InputStream;
            BinaryReader reader = new BinaryReader(strm);
            Byte[] bytes = reader.ReadBytes(Convert.ToInt32(strm.Length));
            imgStud.Src = "data:image/png;base64," + Convert.ToBase64String(bytes, 0, bytes.Length);
            imgStud.Visible = true;
        }
        else
        {
            if (imgStud.Src == "~/images/student.png")
            {
                ScriptManager.RegisterStartupScript(btnPreview, btnPreview.GetType(), "Alert", "alert('Upload an image to preview');", true);
            }
          
        }

    }

    protected void chkboxAdd_Clicked(object sender, EventArgs e)
      {
          if (chkboxAdd.Checked)
          {
              txtPermAddr1.Text =txtPreAddr1.Text ;
              txtPermAddr2.Text =txtPreAddr2.Text ;
              txtPermCity.Text = txtPreCity.Text;
              txtPermState.Text = txtPreState.Text;
              txtPermCountry.Text = txtPreCountry.Text;
              txtPermPin.Text = txtPrePin.Text;
              txtPermDist.Text = txtPreDist.Text;
          }
          else
          {
              txtPermAddr1.Text = "";
              txtPermAddr2.Text = "";
              txtPermCity.Text = "";
              txtPermState.Text = "";
              txtPermCountry.Text = "";
              txtPermPin.Text = "";
              txtPermDist.Text = "";
          }
    }
    protected void drpcat_Changed(object sender, EventArgs e)
          {
              if(drpcat.SelectedValue =="4")
              {
                  txtTribeName.Enabled=true;
              }
              else{
                  txtTribeName.Enabled=false;
                  
              }
          }

    protected void ChkSibling_CheckedChanged(object sender, EventArgs e)
          {
              if (ChkSibling.Checked)
              {
                  if (ViewState["siblingOther"] != null)
                  {

                      lnkdelete1.Visible = true;
                  }
                  else
                  {
                      lnkdelete1.Visible = false;
                  }
                  NewSibPanel.Visible = true;
              }
              else
              {
                  NewSibPanel.Visible = false;
              }
          }

    protected void lnkdelete1_Click(object sender, EventArgs e)
          {
              DataTable dataTable = ViewState["siblingOther"] as DataTable;
              clsDAL clsDal = new clsDAL();
              if (Request["Checkb"] == null)
              {
                  ScriptManager.RegisterClientScriptBlock((Control)lnkdelete1, lnkdelete1.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
              }
              else
              {
                  string str1 = Request["Checkb"];
                  char[] chArray = new char[1] { ',' };
                  foreach (object obj in str1.Split(chArray))
                  {
                      string str2 = obj.ToString();
                      foreach (DataRow dataRow in dataTable.Select("SiblingNo=" + str2))
                       dataRow.Delete();
                      dataTable.AcceptChanges();
                      ViewState["Count"] = Convert.ToString(Convert.ToUInt32(ViewState["Count"]) - 1);
                  }
                  ViewState["siblingOther"] = dataTable;
                  grdsiblings2.DataSource = dataTable.DefaultView;
                  lblsibcount.Text = " (Siblings Added:" + ViewState["Count"].ToString() + ")";
                  grdsiblings2.DataBind();
              }
          }
    protected void lkaddmore1_Click(object sender, EventArgs e)
          {
              FillsiblingOther();
              txtSibName.Text = txtSibSchl.Text = txtSibCls.Text = "";

          }
    private DataTable FillsiblingOther()
          {
              
              DataTable dataTable = ViewState["siblingOther"] as DataTable;
              grdsiblings2.Visible = true;
              if (dataTable == null)
                  return dataTable;

              if (txtSibName.Text.Trim() == string.Empty || txtSibName.Text.Trim()=="")
              {
                  txtSibName.Focus();
                  return dataTable;

              }
              DataRow row = dataTable.NewRow();
              row["FullName"] = txtSibName.Text;
              row["SchoolName"] = txtSibSchl.Text;
              row["ClassName"] = txtSibCls.Text;
              row["SiblingNo"] = Convert.ToString(dataTable.Rows.Count + 1);

              dataTable.Rows.Add(row);
              ViewState["siblingOther"] = dataTable;
              if (ViewState["Count"].ToString() == string.Empty)
              {
                  ViewState["Count"] = "0";
              }
              ViewState["Count"] = Convert.ToString(Convert.ToInt32(ViewState["Count"]) + 1);

              lblsibcount.Text = " (Siblings Added:" + ViewState["Count"].ToString() + ")";
              if (dataTable.Rows.Count > 0)
              {
                  grdsiblings2.DataSource = dataTable;
                  grdsiblings2.DataBind();
                  grdsiblings2.Visible = true;
                  lkaddmore1.Visible = true;
                  lnkdelete1.Visible = true;
              }
              return dataTable;
          }
    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> AutoComplete(string prefixText, int count, string contextKey)
    {
        int id = Convert.ToInt32(contextKey);
        List<string> newlist = new List<string>();
        Hashtable ht = new Hashtable();
        ht.Add("@Term",prefixText);
        switch(id)
            {
            case 1:
                ht.Add("@Flag", 1);
                break;
            case 2:
                ht.Add("@Flag", 2);
                break;
            case 3:
                ht.Add("@Flag", 3);
                break;
            case 4:
                ht.Add("@Flag", 4);
                break;
            default:
                ht.Add("@Flag", 0);
                break;
             
            }
          // ht.Add("@Flag", 1);
        DataTable dt = new clsDAL().GetDataTable("AutoComplete",ht);
        foreach( DataRow row in dt.Rows)
        {
            newlist.Add(row["Result"].ToString());
        }
        return newlist;
    }
  }
