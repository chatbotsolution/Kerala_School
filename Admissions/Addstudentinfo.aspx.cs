using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Admissions_Addstudentinfo : System.Web.UI.Page
{
    public static int sibcount;
    private ScriptManager sm;
    protected void Page_Load(object sender, EventArgs e)
    {
        string appSetting = ConfigurationManager.AppSettings["BkUp"];
        Page.MaintainScrollPositionOnPostBack = true;
        if (Page.IsPostBack)
            return;
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
            feeOptionRow.Visible = true;
            chkAnnual.Checked = true;
            chkOT.Checked = true;
            PopCalAdmnDt.SetDateValue(DateTime.Today);
            txtnationality.Text = "Indian";
            txtMotherTongue.Text = "Oriya";
            txtrollno.Text = "0";
            txtprevmedium.Text = "Oriya";
            feeOptionRow.Visible = true;
            lblAdmnNo.Text = "";
            ChkBus.Checked = true;
            ChkHostel.Checked = true;
        }
        GetType();
    }

    private void filldropdowns()
    {
        new clsStaticDropdowns().FillSessYrForAdmn(drpAdmSessYr);
        FillClass();
        getsections();
        Fillcategory();
        FillReligion();
        FillSession();
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
        GridFill(dataSet.Tables[1]);
        showdata(dataSet.Tables[0]);
        ChkBusHostel(dataSet.Tables[2]);
        ViewState["Admn"] = Request.QueryString["sid"];
        ViewState["SessYr"] = Request.QueryString["sy"].ToString();
        ViewState["classid"] = drpnewclass.SelectedValue.ToString().Trim();
    }

    private void GetType()
    {
        if (Request.QueryString["Type"] == null)
            return;
        drpStudType.SelectedValue = Request.QueryString["Type"].ToString().Trim();
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

    protected void GridFill(DataTable dt)
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
    }

    private void showdata(DataTable tb)
    {
        if (tb.Rows.Count <= 0)
            return;
        PopCalAdmnDt.SetDateValue(Convert.ToDateTime(tb.Rows[0]["AdmnDate"].ToString()));
        Admissions_Addstudentinfo.sibcount = Convert.ToInt32(tb.Rows[0]["sibcount"]);
        if (Admissions_Addstudentinfo.sibcount != 0)
        {
            lblsibcount.Visible = true;
            lblsibcount.Text = "(Siblings for this student=" + Admissions_Addstudentinfo.sibcount.ToString() + ")";
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
        drpGender.SelectedValue = tb.Rows[0]["sex"].ToString();
        drprelgn.SelectedValue = tb.Rows[0]["religion"].ToString();
        drpBloodGroup.SelectedValue = tb.Rows[0]["BloodGroup"].ToString();
        txtnationality.Text = tb.Rows[0]["Nationality"].ToString();
        drpcat.SelectedValue = tb.Rows[0]["Cat"].ToString();
        txtMotherTongue.Text = tb.Rows[0]["MotherTongue"].ToString();
        drpLocality.SelectedValue = tb.Rows[0]["Locality"].ToString();
        ViewState["StudImg"] = tb.Rows[0]["StudentPhoto"].ToString();
        ViewState["StudDoc"] = tb.Rows[0]["StudentDoc"].ToString();
        hlDoc.Text = tb.Rows[0]["StudentDoc"].ToString();
        hlDoc.NavigateUrl = "~/Up_Files/Doc/" + tb.Rows[0]["StudentDoc"].ToString();
        imgStud.Src = "~/Up_Files/Studimage/" + tb.Rows[0]["StudentPhoto"].ToString();
        txtfathername.Text = tb.Rows[0]["FatherName"].ToString();
        txtmothername.Text = tb.Rows[0]["MotherName"].ToString();
        txtFathOcc.Text = tb.Rows[0]["FatherOccupation"].ToString();
        txtMothOcc.Text = tb.Rows[0]["MotherOccupation"].ToString();
        txtLocalGuard.Text = tb.Rows[0]["LocalGuardianName"].ToString();
        txtRelWithLocalGuard.Text = tb.Rows[0]["RelationWithLG"].ToString();
        txtPreAddr.Text = tb.Rows[0]["PresentAddress"].ToString();
        txtPreDist.Text = tb.Rows[0]["PresAddrDist"].ToString();
        txtPrePS.Text = tb.Rows[0]["PresAddrPS"].ToString();
        txtPrePin.Text = tb.Rows[0]["PresAddrPin"].ToString();
        txtPrePhone.Text = tb.Rows[0]["TeleNoOffice"].ToString();
        txtPreMob.Text = tb.Rows[0]["TelNoResidence"].ToString();
        txtPreEmail.Text = tb.Rows[0]["EmailId"].ToString();
        txtPermAddr.Text = tb.Rows[0]["PermAddress"].ToString();
        txtPermDist.Text = tb.Rows[0]["PermAddrDist"].ToString();
        txtPermPS.Text = tb.Rows[0]["PermAddrPS"].ToString();
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
        txtadmsnno.Text = tb.Rows[0]["OldAdmnNo"].ToString();
        drpcat.SelectedValue = Convert.ToString(tb.Rows[0]["cat"].ToString());
        drprelgn.SelectedValue = Convert.ToString(tb.Rows[0]["religion"].ToString());
        drpGender.SelectedValue = Convert.ToString(tb.Rows[0]["sex"].ToString());
        txtadmsnno.Text = tb.Rows[0]["OldAdmnNo"].ToString();
        drpSession.SelectedValue = tb.Rows[0]["SessionYear"].ToString();
        ViewState["SessYrBeforeMod"] = tb.Rows[0]["SessionYear"].ToString();
        drpnewclass.SelectedValue = tb.Rows[0]["Classidstream"].ToString();
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
        ViewState["RollNoBeforeMod"] = tb.Rows[0]["RollNo"].ToString();
        ViewState["FeeStDate"] = tb.Rows[0]["FeestartDate"].ToString();
        drpSession.Enabled = false;
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
                    new clsDAL().ExcuteProcInsUpdt("ps_sp_delete_Ledger", new Hashtable()
          {
            {
               "AdmnNo",
               Request.QueryString["sid"]
            }
          });
                    InsertDetailsInLedger();
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
        }
        else
        {
            try
            {
                InsertStudDetails();
                if (chkGenFee.Checked)
                    InsertDetailsInLedger();
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
        hashtable.Add("NickName", txtnickname.Text.Trim());
        hashtable.Add("@DOB", PopCalDOB.GetDateValue());
        hashtable.Add("@MotherTongue", txtMotherTongue.Text.Trim());
        hashtable.Add("@Locality", drpLocality.SelectedValue);
        hashtable.Add("@FatherName", txtfathername.Text.Trim());
        hashtable.Add("@FatherOccupation", txtFathOcc.Text.Trim());
        hashtable.Add("@MotherName", txtmothername.Text.Trim());
        hashtable.Add("@MotherOccupation", txtMothOcc.Text.Trim());
        hashtable.Add("@LocalGuardianName", txtLocalGuard.Text.Trim());
        hashtable.Add("@RelationWithLG", txtRelWithLocalGuard.Text.Trim());
        hashtable.Add("@PresentAddress", txtPreAddr.Text.Trim());
        hashtable.Add("@PresAddrDist", txtPreDist.Text.Trim());
        hashtable.Add("@PresAddrPin", txtPrePin.Text.Trim());
        hashtable.Add("@PresAddrPS", txtPrePS.Text.Trim());
        hashtable.Add("@PermAddress", txtPermAddr.Text.Trim());
        hashtable.Add("@PermAddrDist", txtPermDist.Text.Trim());
        hashtable.Add("@PermAddrPin", txtPermPin.Text.Trim());
        hashtable.Add("@PermAddrPS", txtPermPS.Text.Trim());
        hashtable.Add("@TeleNoOffice", txtPrePhone.Text.Trim());
        hashtable.Add("@TelNoResidence", txtPreMob.Text.Trim());
        hashtable.Add("@EmailId", txtPreEmail.Text.Trim());
        hashtable.Add("@Nationality", txtnationality.Text.Trim());
        hashtable.Add("@Class", drpnewclass.SelectedValue);
        hashtable.Add("@SportsProf", txtgames.Text.Trim());
        hashtable.Add("@Hobbies", txthobbies.Text.Trim());
        hashtable.Add("@Cat", drpcat.SelectedValue);
        hashtable.Add("@StudAdhar", txtStudAdhar.Text.Trim());
        hashtable.Add("@FatherAdhar", txtFatAdhar.Text.Trim());
        hashtable.Add("@MotherAdhar", txtMotAdhar.Text.Trim());
        hashtable.Add("@BankName", txtBank.Text.Trim());
        hashtable.Add("@BankAcNo", txtEmpBankAcNo.Text.Trim());
        hashtable.Add("@IFSCCode", txtIFSC.Text.Trim());
        hashtable.Add("@Branch", txtBranch.Text.Trim());
        hashtable.Add("@HouseID", 1);
        hashtable.Add("@Status", 1);
        DateTime dateTime;
        if (Request.QueryString["sid"] != null)
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
        DataTable dataTable = clsDal.GetDataTable("ps_sp_insert_addmissioninfo", hashtable);
        if (dataTable.Rows.Count > 0)
            ViewState["Admn"] = dataTable.Rows[0]["AdmnNo"].ToString();
        else
            ViewState["Admn"] = Request.QueryString["sid"].ToString();
        if (Request.QueryString["sid"] != null)
        {
            new clsDAL().ExcuteQryInsUpdt("delete PS_SiblingDetails where admnno=" + Request.QueryString["sid"]);
            InsertSibling();
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
        InsertDetailsForBusFee();
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
        }
        lblsibcount.Text = " (Siblings Added:" + dataTable.Rows.Count.ToString() + ")";
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
        if (Admissions_Addstudentinfo.sibcount != 0)
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
        txtfathername.Text = string.Empty;
        txtmothername.Text = string.Empty;
        txtFathOcc.Text = string.Empty;
        txtMothOcc.Text = string.Empty;
        txtLocalGuard.Text = string.Empty;
        txtRelWithLocalGuard.Text = string.Empty;
        txtnationality.Text = "Indian";
        txtMotherTongue.Text = "Oriya";
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
        txtPreAddr.Text = string.Empty;
        txtPreDist.Text = string.Empty;
        txtPrePS.Text = string.Empty;
        txtPrePin.Text = string.Empty;
        txtPrePhone.Text = string.Empty;
        txtPreMob.Text = string.Empty;
        txtPreEmail.Text = string.Empty;
        txtPermAddr.Text = string.Empty;
        txtPermDist.Text = string.Empty;
        txtPermPin.Text = string.Empty;
        txtPermPS.Text = string.Empty;
        txtTCNo.Text = string.Empty;
        txtTCDt.Text = string.Empty;
        txtStudAdhar.Text = string.Empty;
        txtFatAdhar.Text = string.Empty;
        txtMotAdhar.Text = string.Empty;
        txtBank.Text = string.Empty;
        txtEmpBankAcNo.Text = string.Empty;
        txtIFSC.Text = string.Empty;
        txtBranch.Text = string.Empty;
        txtFeeConcession.Text = "0";
        txtConceReason.Text = string.Empty;
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
            }
            ViewState["sibling"] = dataTable;
            grdsiblings.DataSource = dataTable.DefaultView;
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
        lblMsgTop.Text = "";
        lblMsgBottom.Text = "";
        UpdatePanel1.Update();
        UpdatePanel2.Update();
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
        Response.Redirect("showstudents.aspx");
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
}