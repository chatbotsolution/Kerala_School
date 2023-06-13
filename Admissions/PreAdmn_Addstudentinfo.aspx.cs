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

public partial class Admissions_PreAdmn_Addstudentinfo : System.Web.UI.Page
{
    public static int sibcount;
    protected void Page_Load(object sender, EventArgs e)
    {
        string appSetting = ConfigurationManager.AppSettings["BkUp"];
        Page.MaintainScrollPositionOnPostBack = true;
        if (Page.IsPostBack)
            return;
        lblMsgBottom.Text = "";
        lblMsgTop.Text = "";
        Page.Form.DefaultButton = btnSubmit.UniqueID;
        ViewState["Admn"] = (object)string.Empty;
        ViewState["StudImg"] = (object)string.Empty;
        ViewState["StudDoc"] = (object)string.Empty;
        btnPrintReceipt1.Enabled = false;
        filldropdowns();
        if (Request.QueryString["sid"] != null)
        {
            ViewState["Admn"] = (object)"Update";
            DispStudInfo();
            btnPrintReceipt1.Enabled = false;
            hfSaveMode.Value = "Update";
        }
        else
        {
            PopCalAdmnDt.DateValue = DateTime.Today;
            
            txtnationality.Text = "Indian";
            txtMotherTongue.Text = "Oriya";
            txtrollno.Text = "0";
            txtprevmedium.Text = "Oriya";
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
        drpclass.DataSource = (object)dataTable;
        drpclass.DataTextField = "ClassName";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpnewclass.DataSource = (object)dataTable;
        drpnewclass.DataTextField = "ClassName";
        drpnewclass.DataValueField = "classid";
        drpnewclass.DataBind();
    }

    private void getsections()
    {
        new clsStaticDropdowns().FillSection(drpsection);
    }

    private void Fillcategory()
    {
        drpcat.DataSource = (object)new clsDAL().GetDataTableQry("select * from PS_CategoryMaster order by CatName");
        drpcat.DataTextField = "CatName";
        drpcat.DataValueField = "CatID";
        drpcat.DataBind();
    }

    private void FillReligion()
    {
        drprelgn.DataSource = (object)new clsDAL().GetDataTableQry("select * from ps_studentreligion order by ReligionId");
        drprelgn.DataTextField = "Religion";
        drprelgn.DataValueField = "ReligionId";
        drprelgn.DataBind();
    }

    private void FillSession()
    {
        drpSession.DataSource = (object)new clsDAL().GetDataTableQry("select distinct SessionID,SessionYr from dbo.PS_FeeNormsNew order by sessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void DispStudInfo()
    {
        DataSet dataSet = new clsDAL().GetDataSet("sp_display_studentinfo", new Hashtable()
    {
      {
        (object) "admnno",
        (object) Request.QueryString["sid"]
      },
      {
        (object) "session",
        (object) Request.QueryString["sy"].ToString()
      }
    });
        GridFill(dataSet.Tables[1]);
        showdata(dataSet.Tables[0]);
        ChkBusHostel(dataSet.Tables[2]);
        ViewState["Admn"] = (object)Request.QueryString["sid"];
        ViewState["SessYr"] = (object)Request.QueryString["sy"].ToString();
        ViewState["classid"] = (object)drpnewclass.SelectedValue.ToString().Trim();
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
        if (dt.Rows.Count <= 0)
            return;
        ViewState["sibling"] = (object)dt;
    }

    private void generatesibling()
    {
        ViewState["sibling"] = (object)new DataTable()
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
       
       
        PopCalAdmnDt.DateValue = Convert.ToDateTime(tb.Rows[0]["AdmnDate"].ToString());
        sibcount = Convert.ToInt32(tb.Rows[0]["sibcount"]);
        lblAdmnNo.Text = Request.QueryString["sid"].ToString();
        ViewState["StudType"] = (object)tb.Rows[0]["StudType"].ToString();
        drpAdmSessYr.SelectedValue = tb.Rows[0]["AdmnSessYr"].ToString();
        txtname.Text = tb.Rows[0]["FullName"].ToString();
        txtnickname.Text = tb.Rows[0]["NickName"].ToString();
        PopCalDOB.DateValue = Convert.ToDateTime(tb.Rows[0]["DOB"].ToString());
        drpGender.SelectedValue = tb.Rows[0]["sex"].ToString();
        drprelgn.SelectedValue = tb.Rows[0]["religion"].ToString();
        drpBloodGroup.SelectedValue = tb.Rows[0]["BloodGroup"].ToString();
        txtnationality.Text = tb.Rows[0]["Nationality"].ToString();
        drpcat.SelectedValue = tb.Rows[0]["Cat"].ToString();
        txtMotherTongue.Text = tb.Rows[0]["MotherTongue"].ToString();
        drpLocality.SelectedValue = tb.Rows[0]["Locality"].ToString();
        ViewState["StudImg"] = (object)tb.Rows[0]["StudentPhoto"].ToString();
        ViewState["StudDoc"] = (object)tb.Rows[0]["StudentDoc"].ToString();
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
           
            PopCalTCDate.DateValue = Convert.ToDateTime(tb.Rows[0]["TCDate"].ToString());
        else
            txtTCDt.Text = "";
        txtadmsnno.Text = tb.Rows[0]["OldAdmnNo"].ToString();
        drpcat.SelectedValue = Convert.ToString(tb.Rows[0]["cat"].ToString());
        drprelgn.SelectedValue = Convert.ToString(tb.Rows[0]["religion"].ToString());
        drpGender.SelectedValue = Convert.ToString(tb.Rows[0]["sex"].ToString());
        txtadmsnno.Text = tb.Rows[0]["OldAdmnNo"].ToString();
        drpSession.SelectedValue = tb.Rows[0]["SessionYear"].ToString();
        ViewState["SessYrBeforeMod"] = (object)tb.Rows[0]["SessionYear"].ToString();
        drpnewclass.SelectedValue = tb.Rows[0]["Classidstream"].ToString();
        ViewState["ClassBeforeMod"] = (object)tb.Rows[0]["Classidstream"].ToString();
        tb.Rows[0]["section"].ToString();
        drpsection.SelectedValue = tb.Rows[0]["section"].ToString();
        txtrollno.Text = tb.Rows[0]["RollNo"].ToString();
        txtStudAdhar.Text = tb.Rows[0]["StudAdhar"].ToString();
        txtFatAdhar.Text = tb.Rows[0]["FatherAdhar"].ToString();
        txtMotAdhar.Text = tb.Rows[0]["MotherAdhar"].ToString();
        ViewState["RollNoBeforeMod"] = (object)tb.Rows[0]["RollNo"].ToString();
        ViewState["FeeStDate"] = (object)tb.Rows[0]["FeestartDate"].ToString();
        drpSession.Enabled = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblMsgBottom.Text = "";
        lblMsgTop.Text = "";
        ViewState["Admn"] = (object)null;
        ViewState["classid"] = (object)null;
        ViewState["SessYr"] = (object)null;
        if (new clsDAL().ExecuteScalar("ps_spCheckFeeAmount", new Hashtable()
    {
      {
        (object) "@SessionYr",
        (object) drpSession.SelectedValue.Trim()
      },
      {
        (object) "@ClassId",
        (object) drpnewclass.SelectedValue.Trim()
      },
      {
        (object) "@StudType",
        (object) "N"
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
            if (ViewState["SessYrBeforeMod"].ToString().Trim() != drpSession.SelectedValue.ToString().Trim() || ViewState["ClassBeforeMod"].ToString().Trim() != drpnewclass.SelectedValue.ToString().Trim() || ViewState["StudType"].ToString().Trim() != "N")
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
              (object) "AdmnNo",
              (object) Request.QueryString["sid"]
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
            hashtable.Add((object)"AdmnNo", (object)Request.QueryString["sid"]);
        PopCalAdmnDt.GetDateValue().ToString("MM/dd/yyyy");
        hashtable.Add((object)"AdmnDate", (object)PopCalAdmnDt.GetDateValue());
        hashtable.Add((object)"FullName", (object)txtname.Text.Trim());
        hashtable.Add((object)"NickName", (object)txtnickname.Text.Trim());
        hashtable.Add((object)"@DOB", (object)PopCalDOB.GetDateValue());
        hashtable.Add((object)"@MotherTongue", (object)txtMotherTongue.Text.Trim());
        hashtable.Add((object)"@Locality", (object)drpLocality.SelectedValue);
        hashtable.Add((object)"@FatherName", (object)txtfathername.Text.Trim());
        hashtable.Add((object)"@FatherOccupation", (object)txtFathOcc.Text.Trim());
        hashtable.Add((object)"@MotherName", (object)txtmothername.Text.Trim());
        hashtable.Add((object)"@MotherOccupation", (object)txtMothOcc.Text.Trim());
        hashtable.Add((object)"@LocalGuardianName", (object)txtLocalGuard.Text.Trim());
        hashtable.Add((object)"@RelationWithLG", (object)txtRelWithLocalGuard.Text.Trim());
        hashtable.Add((object)"@PresentAddress", (object)txtPreAddr.Text.Trim());
        hashtable.Add((object)"@PresAddrDist", (object)txtPreDist.Text.Trim());
        hashtable.Add((object)"@PresAddrPin", (object)txtPrePin.Text.Trim());
        hashtable.Add((object)"@PresAddrPS", (object)txtPrePS.Text.Trim());
        hashtable.Add((object)"@PermAddress", (object)txtPermAddr.Text.Trim());
        hashtable.Add((object)"@PermAddrDist", (object)txtPermDist.Text.Trim());
        hashtable.Add((object)"@PermAddrPin", (object)txtPermPin.Text.Trim());
        hashtable.Add((object)"@PermAddrPS", (object)txtPermPS.Text.Trim());
        hashtable.Add((object)"@TeleNoOffice", (object)txtPrePhone.Text.Trim());
        hashtable.Add((object)"@TelNoResidence", (object)txtPreMob.Text.Trim());
        hashtable.Add((object)"@EmailId", (object)txtPreEmail.Text.Trim());
        hashtable.Add((object)"@Nationality", (object)txtnationality.Text.Trim());
        hashtable.Add((object)"@Class", (object)drpnewclass.SelectedValue);
        hashtable.Add((object)"@SportsProf", (object)"");
        hashtable.Add((object)"@Hobbies", (object)"");
        hashtable.Add((object)"@Cat", (object)drpcat.SelectedValue);
        hashtable.Add((object)"@StudAdhar", (object)txtStudAdhar.Text.Trim());
        hashtable.Add((object)"@FatherAdhar", (object)txtFatAdhar.Text.Trim());
        hashtable.Add((object)"@MotherAdhar", (object)txtMotAdhar.Text.Trim());
        hashtable.Add((object)"@BankName", (object)"");
        hashtable.Add((object)"@BankAcNo", (object)"");
        hashtable.Add((object)"@IFSCCode", (object)"");
        hashtable.Add((object)"@Branch", (object)"");
        hashtable.Add((object)"@HouseID", (object)1);
        hashtable.Add((object)"@Status", (object)1);
        hashtable.Add((object)"@FeestartDate", (object)PopCalAdmnDt.GetDateValue().ToString("dd-MMM-yyyy"));
        hashtable.Add((object)"@sex", (object)drpGender.SelectedValue);
        hashtable.Add((object)"@religion", (object)drprelgn.SelectedValue);
        hashtable.Add((object)"@OldAdmnNo", (object)txtadmsnno.Text.Trim());
        if (fldUpImage.HasFile)
        {
            string str = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(fldUpImage.FileName);
            hashtable.Add((object)"StudentPhoto", (object)str);
            fldUpImage.SaveAs(Server.MapPath("~/Up_Files/Studimage/" + str));
        }
        else if (ViewState["StudImg"].ToString() != "")
            hashtable.Add((object)"StudentPhoto", (object)ViewState["StudImg"].ToString());
        else
            hashtable.Add((object)"StudentPhoto", (object)"NoImage.jpg");
        if (fldUpDoc.HasFile)
        {
            string str = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(fldUpDoc.FileName);
            hashtable.Add((object)"@StudentDoc", (object)str);
            fldUpDoc.SaveAs(Server.MapPath("~/Up_Files/Doc/" + str));
        }
        else if (ViewState["StudDoc"] != null && ViewState["StudDoc"].ToString() != "")
            hashtable.Add((object)"@StudentDoc", (object)ViewState["StudDoc"].ToString());
        else
            hashtable.Add((object)"@StudentDoc", (object)"");
        hashtable.Add((object)"@UserID", (object)Session["User_Id"].ToString().Trim());
        hashtable.Add((object)"@SchoolID", (object)Session["SchoolId"].ToString().Trim());
        hashtable.Add((object)"@MediumOfInst", (object)txtprevmedium.Text.Trim());
        hashtable.Add((object)"@ClassJoinDate", (object)PopCalAdmnDt.GetDateValue());
        hashtable.Add((object)"@Detained_Promoted", (object)"");
        hashtable.Add((object)"@Grade", (object)"");
        hashtable.Add((object)"@PrevSchoolname", (object)txtschoolname.Text.Trim());
        hashtable.Add((object)"@SessionYear", (object)drpSession.SelectedValue.ToString().Trim());
        hashtable.Add((object)"@ClassID", (object)drpnewclass.SelectedValue);
        if (drpsection.SelectedValue.ToString() == "Not Allotted")
            hashtable.Add((object)"@Section", (object)"");
        else
            hashtable.Add((object)"@Section", (object)drpsection.SelectedValue.ToString());
        hashtable.Add((object)"@RollNo", (object)txtrollno.Text.Trim());
        hashtable.Add((object)"@PrevClass", (object)drpclass.SelectedValue);
        hashtable.Add((object)"@TCNo", (object)txtTCNo.Text);
        if (!txtTCDt.Text.Trim().Equals(string.Empty))
            hashtable.Add((object)"TCDate", (object)PopCalTCDate.GetDateValue());
        else
            hashtable.Add((object)"TCDate", (object)null);
        hashtable.Add((object)"@AdmnSessYr", (object)drpSession.SelectedValue.ToString().Trim());
        hashtable.Add((object)"@StudType", (object)"N");
        if (Request.QueryString["sid"] == null)
        {
            hashtable.Add((object)"feeconssionper", (object)"0");
            hashtable.Add((object)"authority", (object)"Principal");
            hashtable.Add((object)"reason", (object)"");
        }
        if (ChkBus.Checked.Equals(true))
            hashtable.Add((object)"@BusFacility", (object)"Y");
        else
            hashtable.Add((object)"@BusFacility", (object)"N");
        if (ChkHostel.Checked.Equals(true))
            hashtable.Add((object)"@HostelFacility", (object)"Y");
        else
            hashtable.Add((object)"@HostelFacility", (object)"N");
        ViewState["Admn"] = (object)string.Empty;
        hashtable.Add((object)"@BloodGroup", (object)drpBloodGroup.SelectedValue.ToString());
        DataTable dataTable = clsDal.GetDataTable("ps_sp_insert_PreAdmnAdmnInfo", hashtable);
        if (dataTable.Rows.Count > 0)
            ViewState["Admn"] = (object)dataTable.Rows[0]["AdmnNo"].ToString();
        else
            ViewState["Admn"] = (object)Request.QueryString["sid"].ToString();
    }

    private void InsertDetailsInLedger()
    {
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        string str1 = "Y";
        string str2 = "Y";
        Convert.ToDateTime(ViewState["FeeStDate"].ToString());
        DateTime dateValue = PopCalAdmnDt.GetDateValue();
        clsGenerateFee.GenerateFeeOnAdmission(PopCalAdmnDt.GetDateValue(), dateValue, ViewState["Admn"].ToString(), str1, str2, drpSession.SelectedValue.ToString().Trim(), Session["User_Id"].ToString(), Session["SchoolId"].ToString(), char.Parse("N"));
        btnPrintReceipt1.Enabled = true;
        ViewState["classid"] = (object)drpnewclass.SelectedValue;
        ViewState["SessYr"] = (object)drpSession.SelectedValue.ToString();
        clear();
    }

    private void InsertDetailsForBusFee()
    {
        new clsDAL().ExcuteProcInsUpdt("ps_sp_insert_AdFeeLedgerBus", new Hashtable()
    {
      {
        (object) "Admnno",
        ViewState["Admn"]
      },
      {
        (object) "uid",
        (object) Convert.ToInt32(Session["User_Id"])
      },
      {
        (object) "Session",
        (object) drpSession.SelectedValue.ToString().Trim()
      },
      {
        (object) "SchoolId",
        (object) Session["SchoolId"].ToString().Trim()
      }
    });
    }

    private void InsertSibling()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = ViewState["sibling"] as DataTable;
        if (dataTable == null || dataTable.Rows.Count <= 0)
            return;
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            clsDal.ExcuteProcInsUpdt("ps_sp_insert_Sibling", new Hashtable()
      {
        {
          (object) "admnno",
          (object) ViewState["Admn"].ToString()
        },
        {
          (object) "UserID",
          (object) Session["User_Id"].ToString().Trim()
        },
        {
          (object) "SchoolID",
          (object) Session["SchoolId"].ToString().Trim()
        },
        {
          (object) "SiblingAdmnNo",
          (object) row["SiblingAdmnNo"].ToString()
        },
        {
          (object) "SiblingClass",
          (object) row["ClassId"].ToString()
        },
        {
          (object) "SiblingNo",
          (object) row["SiblingNo"].ToString()
        }
      });
    }

    private void clear()
    {
        PopCalAdmnDt.DateValue = DateTime.Now;
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
        txtadmsnno.Text = string.Empty;
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
    }

    protected void btnGoToList_Click(object sender, EventArgs e)
    {
        Response.Redirect("showstudents.aspx");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
        ViewState["Admn"] = (object)null;
        ViewState["classid"] = (object)null;
        ViewState["SessYr"] = (object)null;
        lblMsgTop.Text = "";
        lblMsgBottom.Text = "";
        UpdatePanel2.Update();
    }

    protected void btnPrintReceipt1_Click(object sender, EventArgs e)
    {
        if (!(ViewState["Admn"].ToString().Trim() != ""))
            return;
       // "../FeeManagement/feercptcash.aspx?admnno=" + ViewState["Admn"].ToString() + "&cid=" + ViewState["classid"].ToString() + "&sess=" + ViewState["SessYr"].ToString();
        Response.Redirect("../FeeManagement/feercptcash.aspx?admnno=" + ViewState["Admn"].ToString() + "&cid=" + ViewState["classid"].ToString() + "&sess=" + ViewState["SessYr"].ToString());
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("PreAdmn_Showstudents.aspx");
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