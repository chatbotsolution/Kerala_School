using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using SanLib;
using System.Web.SessionState;
using System.Web.Profile;
using System.Web.SessionState;

public partial class Admissions_UploadOldData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button_Clicked(object sender, EventArgs e)
    {
        string connString = "";
        string strFileType = Path.GetExtension(fileUpload.FileName).ToLower();
        string path = Server.MapPath("~/Up_Files/OldData/") + Path.GetFileName(fileUpload.PostedFile.FileName);
        if (File.Exists(path))
            File.Delete(path);
        fileUpload.PostedFile.SaveAs(path);
        //Connection String to Excel Workbook
        if (strFileType.Trim() == ".xls")
        {
            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
        }
        else if (strFileType.Trim() == ".xlsx")
        {
            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
        }
        OleDbConnection conn = new OleDbConnection(connString);
        if (conn.State == ConnectionState.Closed)
            conn.Open();
        string sheet1 = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
        // DataTable activityDataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        string query = "SELECT* FROM [" + sheet1 + "]";

        if (conn.State == ConnectionState.Closed)
            conn.Open();
        OleDbCommand cmd = new OleDbCommand(query, conn);
        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
       // grdstudentsColl.DataSource = ds.Tables[0];
      //  grdstudentsColl.DataBind();
        InsertStudDetails(ds);
        da.Dispose();
        conn.Close();
        conn.Dispose();
    }
    private void InsertStudDetails(DataSet ds)
    {
        if (ds.Tables.Count < 0)
        {
            return;
        }
        else
        {

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                clsDAL clsDal = new clsDAL();
                Hashtable hashtable = new Hashtable();
                //if (Request.QueryString["sid"] != null)
                //    hashtable.Add("AdmnNo", Request.QueryString["sid"]);
                // PopCalAdmnDt.GetDateValue().ToString("MM/dd/yyyy");
                hashtable.Add("AdmnDate", ds.Tables[0].Rows[i]["admission_date"].ToString().Trim());
                hashtable.Add("FullName", ds.Tables[0].Rows[i]["student_name"].ToString().Trim());
                hashtable.Add("NickName", "");
                //String dob = ds.Tables[0].Rows[i]["dob_month"].ToString().Trim() + "/" + ds.Tables[0].Rows[i]["dob_day"].ToString().Trim() + "/" + ds.Tables[0].Rows[i]["dob_year"].ToString().Trim();
                //DateTime d = Convert.ToDateTime(dob);
                hashtable.Add("@DOB", ds.Tables[0].Rows[i]["birth_date"].ToString().Trim());
                hashtable.Add("@StudId", ds.Tables[0].Rows[i]["student_uid"].ToString().Trim());
                hashtable.Add("@POB", ds.Tables[0].Rows[i]["birth_place"].ToString().Trim());
                hashtable.Add("@FormNo", ds.Tables[0].Rows[i]["form_no"].ToString().Trim());
                hashtable.Add("@MotherTongue", ds.Tables[0].Rows[i]["mother_tongue"].ToString().Trim());
                hashtable.Add("@Locality", "");
                hashtable.Add("@FatherName", ds.Tables[0].Rows[i]["father_name"].ToString().Trim());
                hashtable.Add("@FatherOccupation", ds.Tables[0].Rows[i]["father_prof"].ToString().Trim());
                hashtable.Add("@MotherName", ds.Tables[0].Rows[i]["mother_name"].ToString().Trim());
                hashtable.Add("@MotherOccupation", ds.Tables[0].Rows[i]["mother_prof"].ToString().Trim());
                hashtable.Add("@LocalGuardianName", ds.Tables[0].Rows[i]["guardian_name"].ToString().Trim());
                hashtable.Add("@RelationWithLG", "");
                hashtable.Add("@PresentAddress1", ds.Tables[0].Rows[i]["present_address"].ToString().Trim());
                hashtable.Add("@PresentAddress2", ds.Tables[0].Rows[i]["present_post"].ToString().Trim());
                hashtable.Add("@PresAddrDist", ds.Tables[0].Rows[i]["present_dist"].ToString().Trim());
                hashtable.Add("@PresAddrState", ds.Tables[0].Rows[i]["present_state"].ToString().Trim());
                hashtable.Add("@PresAddrCon", ds.Tables[0].Rows[i]["present_country"].ToString().Trim());
                hashtable.Add("@PresAddrPin", ds.Tables[0].Rows[i]["present_pin"].ToString().Trim());
                hashtable.Add("@PresAddrCity", ds.Tables[0].Rows[i]["present_village"].ToString().Trim());
                hashtable.Add("@PermAddress1", ds.Tables[0].Rows[i]["permanent_address"].ToString().Trim());
                hashtable.Add("@PermAddress2", ds.Tables[0].Rows[i]["permanent_post"].ToString().Trim());
                hashtable.Add("@PermAddrDist", ds.Tables[0].Rows[i]["permanent_dist"].ToString().Trim());
                hashtable.Add("@PermAddrState", ds.Tables[0].Rows[i]["permanent_state"].ToString().Trim());
                hashtable.Add("@PermAddrCon", ds.Tables[0].Rows[i]["permanent_country"].ToString().Trim());

                hashtable.Add("@PermAddrPin", ds.Tables[0].Rows[i]["permanent_pin"].ToString().Trim());
                hashtable.Add("@PermAddrCity", ds.Tables[0].Rows[i]["permanent_village"].ToString().Trim());
                hashtable.Add("@TeleNoOffice", ds.Tables[0].Rows[i]["present_landline_no_2"].ToString().Trim());
                hashtable.Add("@TelNoResidence", ds.Tables[0].Rows[i]["present_landline_no_1"].ToString().Trim());
                hashtable.Add("@Mobile1", ds.Tables[0].Rows[i]["present_mobile_1"].ToString().Trim());
                hashtable.Add("@Mobile2", ds.Tables[0].Rows[i]["present_mobile_2"].ToString().Trim());
                hashtable.Add("@EmailId1", ds.Tables[0].Rows[i]["present_email_1"].ToString().Trim());
                hashtable.Add("@EmailId2", ds.Tables[0].Rows[i]["present_email_2"].ToString().Trim());
                hashtable.Add("@Nationality", ds.Tables[0].Rows[i]["nationality"].ToString().Trim());
                string cls = ds.Tables[0].Rows[i]["admission_class"].ToString().Trim();
                DataTable dt = clsDal.GetDataTableQry("Select ClassId from PS_ClassMaster where ClassName='" + cls + "'");
                if (dt.Rows.Count > 0)
                {
                    hashtable.Add("@Class", dt.Rows[0][0].ToString().Trim());
                    hashtable.Add("@ClassID", dt.Rows[0][0].ToString().Trim());
                    if (Convert.ToInt32(dt.Rows[0][0].ToString().Trim()) < 13)
                        hashtable.Add("@Stream", 1);
                    else
                        hashtable.Add("@Stream","");
                }
                else
                {
                    hashtable.Add("@Class", "");
                    hashtable.Add("@ClassID","");
                    hashtable.Add("@Stream","");
                }
                hashtable.Add("@SportsProf", ds.Tables[0].Rows[i]["sports"].ToString().Trim());
                hashtable.Add("@Hobbies", ds.Tables[0].Rows[i]["hobby"].ToString().Trim());
                string cat = ds.Tables[0].Rows[i]["category_abbr"].ToString().Trim();
                if (cat == "GEN")
                    cat = "General";
                DataTable dt1 = clsDal.GetDataTableQry("Select CatID from PS_CategoryMaster where CatName='" + cat + "'");
                if(dt1.Rows.Count>0)
                              hashtable.Add("@Cat", dt1.Rows[0][0].ToString().Trim());
                else
                    hashtable.Add("@Cat","");
                hashtable.Add("@TribeName", ds.Tables[0].Rows[i]["tribe_name"].ToString().Trim());
                hashtable.Add("@StudAdhar", ds.Tables[0].Rows[i]["aadhaar_no"].ToString().Trim());
                hashtable.Add("@FatherAdhar", ds.Tables[0].Rows[i]["father_aadhaar_no"].ToString().Trim());

                hashtable.Add("@FatherSchl", ds.Tables[0].Rows[i]["father_school"].ToString().Trim());
                hashtable.Add("@FatherSchlPlc", ds.Tables[0].Rows[i]["father_school_place"].ToString().Trim());
                hashtable.Add("@FatherClg", ds.Tables[0].Rows[i]["father_college"].ToString().Trim());
                hashtable.Add("@FatherClgPlc", ds.Tables[0].Rows[i]["father_college_place"].ToString().Trim());
                hashtable.Add("@FatherDesg", ds.Tables[0].Rows[i]["father_designation"].ToString().Trim());
                hashtable.Add("@FatherDept", ds.Tables[0].Rows[i]["father_department"].ToString().Trim());
                hashtable.Add("@FatherComp", ds.Tables[0].Rows[i]["father_ins_org_comp"].ToString().Trim());
                hashtable.Add("@FatherEQ", ds.Tables[0].Rows[i]["father_qualif"].ToString().Trim());

                hashtable.Add("@MotherEQ", ds.Tables[0].Rows[i]["mother_qualif"].ToString().Trim());
                hashtable.Add("@MotherSchl", ds.Tables[0].Rows[i]["mother_school"].ToString().Trim());
                hashtable.Add("@MotherSchlPlc", ds.Tables[0].Rows[i]["mother_school_place"].ToString().Trim());
                hashtable.Add("@MotherClg", ds.Tables[0].Rows[i]["mother_college"].ToString().Trim());
                hashtable.Add("@MotherClgPlc", ds.Tables[0].Rows[i]["mother_college_place"].ToString().Trim());
                hashtable.Add("@MotherDesg", ds.Tables[0].Rows[i]["mother_designation"].ToString().Trim());
                hashtable.Add("@MotherDept", ds.Tables[0].Rows[i]["mother_department"].ToString().Trim());
                hashtable.Add("@MotherComp", ds.Tables[0].Rows[i]["mother_ins_org_comp"].ToString().Trim());
                hashtable.Add("@MotherAdhar", ds.Tables[0].Rows[i]["mother_aadhaar_no"].ToString().Trim());
                hashtable.Add("@BankName", ds.Tables[0].Rows[i]["bank_name"].ToString().Trim());
                hashtable.Add("@BankAcNo", ds.Tables[0].Rows[i]["bank_ac_no"].ToString().Trim());
                hashtable.Add("@IFSCCode", ds.Tables[0].Rows[i]["bank_ifsc"].ToString().Trim());
                hashtable.Add("@Branch", "");
                hashtable.Add("@HouseID", 1);
                hashtable.Add("@Status", 1);
                hashtable.Add("@EngFirst", 0.00);
                hashtable.Add("@EngSecond", 0.00);
                hashtable.Add("@EngThird", 0.00);
                hashtable.Add("@EngTotal", 0.00);
                hashtable.Add("@EACFirst", 0.00);
                hashtable.Add("@EACSecond", 0.00);
                hashtable.Add("@EACThird", 0.00);
                hashtable.Add("@EACTotal", 0.00);
                hashtable.Add("@FifthOptional","");
                if (ds.Tables[0].Rows[i]["SixthSub"].ToString().Trim() == "CA")
                    if(cls == "IX")
                        hashtable.Add("@SixthOptional", "85");
                    else
                        hashtable.Add("@SixthOptional", "86");
               else if (ds.Tables[0].Rows[i]["SixthSub"].ToString().Trim() == "EA")
                    if (cls == "IX")
                        hashtable.Add("@SixthOptional", "83");
                    else
                        hashtable.Add("@SixthOptional", "84");
                else
                    hashtable.Add("@SixthOptional","");
                
               // hashtable.Add("@SixthOptional", ds.Tables[0].Rows[i]["SixthSub"].ToString().Trim());
                hashtable.Add("@SecondLang", ds.Tables[0].Rows[i]["second_language"].ToString().Trim());
                hashtable.Add("@Denomination", ds.Tables[0].Rows[i]["denomination"].ToString().Trim());
                //DateTime dateTime;
                //if (Request.QueryString["sid"] != null)
                //{
                //    dateTime = DateTime.Parse(ViewState["FeeStDate"].ToString());
                //}
                //else
                //{
                //    string str1 = drpFeeStartFrom.SelectedValue.ToString();
                //    string str2 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
                //    string str3 = drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
                //    dateTime = Convert.ToInt32(str1) <= 3 ? Convert.ToDateTime(str1 + "/01/" + str3) : Convert.ToDateTime(str1 + "/01/" + str2);
                //}
                //hashtable.Add("@FeestartDate", dateTime);
                string gen = ds.Tables[0].Rows[i]["gender"].ToString().Trim();
                if (gen == "F")
                    gen = "Female";
                else
                    gen = "Male";
                hashtable.Add("@sex", gen);
                string rel = ds.Tables[0].Rows[i]["religion"].ToString().Trim();
                DataTable dt2 = clsDal.GetDataTableQry("Select ReligionId from PS_StudentReligion where Religion ='" + rel + "'");
                if(dt2.Rows.Count>0)
                                hashtable.Add("@religion", dt2.Rows[0][0].ToString().Trim());
                else
                    hashtable.Add("@religion",null);
                hashtable.Add("@OldAdmnNo", ds.Tables[0].Rows[i]["admission_no"].ToString().Trim());
                //if (fldUpImage.HasFile)
                //{
                //    string str = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(fldUpImage.FileName);
                //    hashtable.Add("StudentPhoto", str);
                //    fldUpImage.SaveAs(Server.MapPath("~/Up_Files/Studimage/" + str));
                //}
                //else if (ViewState["StudImg"].ToString() != "")
                //    hashtable.Add("StudentPhoto", ViewState["StudImg"].ToString());
                //else
                //    hashtable.Add("StudentPhoto", "NoImage.jpg");
                //if (fldUpDoc.HasFile)
                //{
                //    string str = DateTime.Now.ToString("yyyyMMddhhmmss") + Path.GetExtension(fldUpDoc.FileName);
                //    hashtable.Add("@StudentDoc", str);
                //    fldUpDoc.SaveAs(Server.MapPath("~/Up_Files/Doc/" + str));
                //}
                //else if (ViewState["StudDoc"] != null && ViewState["StudDoc"].ToString() != "")
                //    hashtable.Add("@StudentDoc", ViewState["StudDoc"].ToString());
                //else
                //    hashtable.Add("@StudentDoc", "");
               

                hashtable.Add("@UserID", Session["User_Id"].ToString().Trim());
                hashtable.Add("@SchoolID", Session["SchoolId"].ToString().Trim());
                hashtable.Add("@MediumOfInst", "");
                hashtable.Add("@ClassJoinDate", ds.Tables[0].Rows[i]["admission_date"].ToString().Trim());
                hashtable.Add("@Detained_Promoted", "");
                hashtable.Add("@Grade", "");
                hashtable.Add("@PrevSchoolname", ds.Tables[0].Rows[i]["prev_school"].ToString().Trim());
               // string ayear = ds.Tables[0].Rows[i]["academic_year"].ToString().Trim();
               // ayear = "20" + ayear;
                hashtable.Add("@SessionYear", ds.Tables[0].Rows[i]["academic_year"].ToString().Trim());
               

                hashtable.Add("@Section", ds.Tables[0].Rows[i]["section_code"].ToString().Trim());
                hashtable.Add("@RollNo", ds.Tables[0].Rows[i]["student_rollno"].ToString().Trim());
                string prevcls = ds.Tables[0].Rows[i]["prev_class"].ToString().Trim();
                DataTable dt4 = clsDal.GetDataTableQry("Select ClassId from PS_ClassMaster where ClassName='" + prevcls + "'");
                if (dt4.Rows.Count > 0)
                    hashtable.Add("@PrevClass", dt4.Rows[0][0].ToString());
                else
                    hashtable.Add("@PrevClass", "");

                hashtable.Add("@TCNo", ds.Tables[0].Rows[i]["prev_tcno"].ToString().Trim());
                hashtable.Add("TCDate", null);
                hashtable.Add("@AdmnSessYr", ds.Tables[0].Rows[i]["academic_year"].ToString().Trim());
                hashtable.Add("@StudType", "N");
                //if (Request.QueryString["sid"] == null)
                //{
                //    hashtable.Add("feeconssionper", "");
                //    hashtable.Add("authority", "Principal");
                //    hashtable.Add("reason", "");
                //}
                //if (ChkBus.Checked.Equals(true))
                //    hashtable.Add("@BusFacility", "Y");
                //else
                //    hashtable.Add("@BusFacility", "N");
                //if (ChkHostel.Checked.Equals(true))
                //    hashtable.Add("@HostelFacility", "Y");
                //else
                //    hashtable.Add("@HostelFacility", "N");
                ViewState["Admn"] = string.Empty;
                hashtable.Add("@BloodGroup", ds.Tables[0].Rows[i]["blood_group"].ToString().Trim());
                hashtable.Add("@StudentPhoto", "");
                hashtable.Add("@StudentDoc", "");
                DataTable dataTable = clsDal.GetDataTable("insert_StudentinfoDuplicate", hashtable);
                if (dataTable.Rows.Count > 0)
                    ViewState["Admn"] = dataTable.Rows[0]["AdmnNo"].ToString();
                else
                    ViewState["Admn"] = null;
                //if (Request.QueryString["sid"] != null)
                //{
                //    new clsDAL().ExcuteQryInsUpdt("delete PS_SiblingDetails where admnno=" + Request.QueryString["sid"]);
                //    InsertSibling();
                //    lkaddmore.Visible = false;
                //    lnkdelete.Visible = false;
                //    radiosiblingno.Checked = true;
                //    grdsiblings.DataSource = null;
                //    grdsiblings.DataBind();
                //    tblsibling.Visible = false;
                //    lblsibcount.Visible = false;
                //}
                //else
                //{
                //    InsertSibling();
                //    InsertConcession();
                //}
            }
        }
    }
}