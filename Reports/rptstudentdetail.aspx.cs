using ASP;
using Classes.DA;
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

public partial class Reports_rptstudentdetail : System.Web.UI.Page
{
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            fillsession();
            fillclass();
            FillStream();
            DRP.FillStatus(drpStatus);
            drpStatus.Items.RemoveAt(0);
            drpStatus.Items.Insert(0, new ListItem("All", "0"));
            drpStatus.SelectedValue = "1";
            FillClassSection();
            FillClassStream();
            FillReligion();
            FillCategory();
            fillstudent();
            lblRecCount.Text = "";
            btnExpExcel.Visible = false;
            cbAll.Checked = true;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void FillClassStream()
    {
        if (Convert.ToInt32(drpclass.SelectedValue.ToString().Trim()) > 12)
        {
            ddlStream.Enabled = true;
            ddlStream.SelectedValue = "0";
        }
        else if (drpclass.SelectedValue.ToString().Trim() == "0")
        {
            ddlStream.SelectedValue = "0";
            ddlStream.Enabled = false;
        }
        else
        {
            ddlStream.SelectedValue = "1";
            ddlStream.Enabled = false;
        }
    }

    private void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("All", "0"));
    }

    private void FillReligion()
    {
        ddlReligion.Items.Clear();
        ddlReligion.DataSource = new Common().GetDataTable("ps_sp_get_ReligionList");
        ddlReligion.DataTextField = "Religion";
        ddlReligion.DataValueField = "ReligionId";
        ddlReligion.DataBind();
        ddlReligion.Items.Insert(0, new ListItem("All", "0"));
    }

    private void FillCategory()
    {
        ddlCategory.Items.Clear();
        ddlCategory.DataSource = new Common().GetDataTable("ps_sp_get_CategoryList");
        ddlCategory.DataTextField = "catName";
        ddlCategory.DataValueField = "catId";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("All", "0"));
    }
    private void FillStream()
    {
        DataTable dt = new Common().ExecuteSql("select StreamId,description from Ps_StreamMaster ");
        ddlStream.DataSource = dt;
        ddlStream.DataValueField = "StreamId";
        ddlStream.DataTextField = "description";
        ddlStream.DataBind();
        ddlStream.Items.Insert(0, new ListItem("All", "0"));

    }

    private void FillClassSection()
    {
        ddlSection.Items.Clear();
        ddlSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "@classId",
         drpclass.SelectedValue.ToString().Trim()
      },
      {
         "@session",
         drpSession.SelectedValue
      }
    });
        ddlSection.DataTextField = "Section";
        ddlSection.DataValueField = "Section";
        ddlSection.DataBind();
        ddlSection.Items.Insert(0, new ListItem("All", "0"));
    }

    private void fillstudent()
    {
        drpstudents.Items.Clear();
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (drpSession.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Session", drpSession.SelectedValue.ToString().Trim());
        if (drpclass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpclass.SelectedValue);
        if (ddlSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", ddlSection.SelectedValue);
        if (ddlReligion.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Religion", ddlReligion.SelectedValue);
        if (ddlCategory.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Category", ddlCategory.SelectedValue);
        if (ddlStream.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Stream", ddlStream.SelectedValue);
        DataTable dataTable = common.GetDataTable("ps_sp_get_StudentNames", hashtable);
        drpstudents.DataSource = dataTable;
        drpstudents.DataTextField = "fullname";
        drpstudents.DataValueField = "admnno";
        drpstudents.DataBind();
        if (dataTable.Rows.Count > 0)
            drpstudents.Items.Insert(0, new ListItem("All", "0"));
        else
            drpstudents.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
         if (drpstudents.SelectedValue != "0")
            bindsinglestudent();
        else
            bindstudentlist();
        chk();
    }

    private void bindsinglestudent()
    {
        trbtn.Visible = true;
        trlist.Visible = true;
        trgrd.Visible = false;
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
        if (drpstudents.SelectedIndex > 0)
            hashtable.Add("@Admnno", drpstudents.SelectedValue.ToString().Trim());
        DataTable dataTable = common.GetDataTable("ps_sp_rptGetIndvStud", hashtable);
        Session["Datalist"] = dataTable;
        Session["printData"] = dataTable;
        if (dataTable.Rows.Count > 0)
        {
            btnprint.Visible = true;
            btnExpExcel.Visible = false;
        }
        else
        {
            btnprint.Visible = false;
            btnExpExcel.Visible = true;
        }
        liststudent.DataSource = dataTable.DefaultView;
        liststudent.DataBind();
        lblreport.Visible = false;
    }

    private void bindstudentlist()
    {
        trbtn.Visible = true;
        trlist.Visible = false;
        trgrd.Visible = true;
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
        if (drpclass.SelectedIndex > 0)
            hashtable.Add("@ClassId", drpclass.SelectedValue.ToString().Trim());
        if (ddlSection.SelectedIndex > 0)
            hashtable.Add("@Section", ddlSection.SelectedValue.ToString().Trim());
        if (drpGender.SelectedIndex > 0)
            hashtable.Add("@Sex", drpGender.SelectedValue.ToString().Trim());
        if (ddlCategory.SelectedIndex > 0)
            hashtable.Add("@CatId", ddlCategory.SelectedValue.ToString().Trim());
        if (ddlReligion.SelectedIndex > 0)
            hashtable.Add("@ReligionId", ddlReligion.SelectedValue.ToString().Trim());
        if(ddlDenomination.SelectedIndex >0)
            hashtable.Add("@Denomination", ddlDenomination.SelectedValue.ToString().Trim());
        if (drpType.SelectedIndex > 0)
            hashtable.Add("@StudTyp", drpType.SelectedValue.ToString().Trim());
        if (drpStatus.SelectedIndex > 0)
            hashtable.Add("@StatusId", drpStatus.SelectedValue.ToString().Trim());
        if (ddlStream.SelectedIndex > 0)
            hashtable.Add("@Stream", ddlStream.SelectedValue.ToString().Trim());
        if (drpstudents.SelectedIndex > 0)
            hashtable.Add("@Adnmno", drpstudents.SelectedValue.ToString().Trim());
        if (drpHostStud.SelectedValue =="1")
            hashtable.Add("@HostelFacility", "Y");
        if (drpHostStud.SelectedValue =="2")
            hashtable.Add("@BusFacility", "Y");
        DataTable dataTable = common.GetDataTable("ps_sp_rptGetAllStud", hashtable);
        lblRecCount.Text = "Total Number of Student: " + dataTable.Rows.Count.ToString();
        CreateReport(dataTable);
        lblreport.Visible = true;
        if (dataTable.Rows.Count > 0)
        {
            btnprint.Visible = false;
            btnprint1.Visible = true;
            btnExpExcel.Visible = true;
        }
        else
        {
            btnprint.Visible = false;
            btnprint1.Visible = false;
            btnExpExcel.Visible = false;
        }
        UpdatePanel1.Update();
    }

    private void CreateReport(DataTable dt)
    {
        dt.Columns.Add("Count", typeof(int));
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["Count"]= i+1;
        }
            try
            {
                if (dt.Rows.Count > 0)
                {
                    StringBuilder stringBuilder = new StringBuilder("");
                    stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
                    
                    stringBuilder.Append("<tr><td colspan='22' class='tbltd'><strong>Total no of Students :" + dt.Rows.Count + "</strong></td></tr>");
                    stringBuilder.Append("<tr  style='background-color:#CCCCCC'>");
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong> Sl No.</strong></td>");
                    if (cbRollNo.Checked)
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Roll No.</strong></td>");
                    if (cbAdmn.Checked)
                        stringBuilder.Append("<td style='width: 100px' class='tbltxt'><strong>Admission No.</strong></td>");
                   
                    if (cbAdmnDate.Checked)
                        stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Admn Date</strong></td>");
                    if (cbAdmnYear.Checked)
                        stringBuilder.Append("<td  class='tbltxt' style='width: 100px' ><strong>Admn Session Year</strong></td>");
                    if (cbClass.Checked)
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Class</strong></td>");
                    if (cbSection.Checked)
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Section</strong></td>");
                    if (cbPresentSes.Checked)
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Present Session</strong></td>");
                    if (cbName.Checked)
                        stringBuilder.Append("<td  class='tbltxt'><strong>Name</strong></td>");
                    if (cbDOB.Checked)
                        stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>DOB</strong></td>");
                    if (cbAge.Checked)
                        stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Age</strong></td>");
                    if (cbSex.Checked)
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Sex</strong></td>");
                    if (cbReligion.Checked)
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Religion</strong></td>");
                    if (cbDenom.Checked)
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Denomination</strong></td>");
                    if (cbCat.Checked)
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Category</strong></td>");
                    if (cbFather.Checked)
                        stringBuilder.Append("<td class='tbltxt'><strong>Father's Name</strong></td>");
                    if (cbMother.Checked)
                        stringBuilder.Append("<td class='tbltxt'><strong>Mother's Name</strong></td>");
                    if (cbLocalGrdName.Checked)
                        stringBuilder.Append("<td class='tbltxt'><strong>Local Guardian Name</strong></td>");
                    if (cbStream.Checked)
                        stringBuilder.Append("<td class='tbltxt'><strong>Stream</strong></td>");
                    if (cbPresentAddress.Checked)
                        stringBuilder.Append("<td class='tbltxt' style='width: 100px'><strong>Present Address</strong></td>");
                    if (cbTelephone.Checked)
                        stringBuilder.Append("<td class='tbltxt'><strong>Telephone</strong></td>");
                    if (cbEmail.Checked)
                        stringBuilder.Append("<td class='tbltxt'><strong>EmailId</strong></td>");
                    if (cbStatus.Checked)
                        stringBuilder.Append("<td class='tbltxt'><strong>Status</strong></td>");
                    if (cbStatusDate.Checked)
                        stringBuilder.Append("<td style='width: 70px' class='tbltxt'><strong>Status Date</strong></td>");
                    if (cbAll.Checked)
                    {
                        stringBuilder.Append("<td style='width: 50px'  class='tbltxt'><strong>Roll No.</strong></td>");
                        stringBuilder.Append("<td style='width: 100px'  class='tbltxt'><strong>Admission No.</strong></td>");
                         
                        stringBuilder.Append("<td style='width: 80px'  class='tbltxt'><strong>Admn Date</strong></td>");
                        stringBuilder.Append("<td  class='tbltxt' style='width: 100px' ><strong>Admn Session Year</strong></td>");
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Class</strong></td>");
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Section</strong></td>");
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Present Session</strong></td>");
                        stringBuilder.Append("<td  class='tbltxt'><strong>Name</strong></td>");
                        stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>DOB</strong></td>");
                        stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Age</strong></td>");
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Sex</strong></td>");
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Religion</strong></td>");
                        stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Category</strong></td>");
                        stringBuilder.Append("<td class='tbltxt'><strong>Father's Name</strong></td>");
                        stringBuilder.Append("<td class='tbltxt'><strong>Mother's Name</strong></td>");
                        stringBuilder.Append("<td class='tbltxt'><strong>Local Guardian Name</strong></td>");
                        stringBuilder.Append("<td class='tbltxt'><strong>Stream</strong></td>");
                        stringBuilder.Append("<td class='tbltxt' style='width: 100px'><strong>Present Address</strong></td>");
                        stringBuilder.Append("<td class='tbltxt'><strong>Telephone</strong></td>");
                        stringBuilder.Append("<td class='tbltxt'><strong>EmailId</strong></td>");
                        stringBuilder.Append("<td class='tbltxt'><strong>Status</strong></td>");
                        stringBuilder.Append("<td style='width: 70px' class='tbltxt'><strong>Status Date</strong></td>");
                    }
                    stringBuilder.Append("</tr>");
                    foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                    {
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td style='align:center'>");
                        stringBuilder.Append(row["Count"].ToString().Trim());
                        stringBuilder.Append("</td>");
                        if (cbRollNo.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["RollNo"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbAdmn.Checked)
                        {
                            stringBuilder.Append("<td>&nbsp;");
                            stringBuilder.Append(row["AdmnNo"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                       
                        if (cbAdmnDate.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["admdate"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbAdmnYear.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["AdmnSessYr"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbClass.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["ClassName"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbSection.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["Section"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbPresentSes.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["SessionYear"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbName.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["FullName"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbDOB.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["dateofbirth"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbAge.Checked)
                        {
                            DateTime dateTime1 = new DateTime();
                            TimeSpan timeSpan = DateTime.Now - Convert.ToDateTime(row["dateofbirth"].ToString().Trim());
                            DateTime dateTime2 = DateTime.MinValue + timeSpan;
                            int num1 = dateTime2.Year - 1;
                            int num2 = dateTime2.Month - 1;
                            int num3 = dateTime2.Day - 1;
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(num1.ToString() + " Years, " + num2.ToString() + " Months, " + num3.ToString() + " Days");
                            stringBuilder.Append("</td>");
                        }
                        if (cbSex.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["sex"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbReligion.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["religion"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbDenom.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["Denomination"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbCat.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["CatName"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbFather.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["FatherName"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbMother.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["MotherName"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbLocalGrdName.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["LocalGuardianName"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbStream.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["Description"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbPresentAddress.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["PresAddr1"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbTelephone.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["TelNoResidence"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbEmail.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["EmailId1"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbStatus.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["Status"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbStatusDate.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["StatusDate"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        if (cbAll.Checked)
                        {
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["RollNo"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>&nbsp;");
                            stringBuilder.Append(row["OldAdmnNo"].ToString().Trim());
                            stringBuilder.Append("</td>");
                           
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["admdate"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["AdmnSessYr"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["ClassName"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["Section"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["SessionYear"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["FullName"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["dateofbirth"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            DateTime dateTime1 = new DateTime();
                            TimeSpan timeSpan = DateTime.Now - Convert.ToDateTime(row["dateofbirth"].ToString().Trim());
                            DateTime dateTime2 = DateTime.MinValue + timeSpan;
                            int num1 = dateTime2.Year - 1;
                            int num2 = dateTime2.Month - 1;
                            int num3 = dateTime2.Day - 1;
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(num1.ToString() + " Years, " + num2.ToString() + " Months, " + num3.ToString() + " Days");
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["sex"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["religion"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["CatName"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["FatherName"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["MotherName"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["LocalGuardianName"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["Description"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["PresAddr1"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["TelNoResidence"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["EmailId1"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["Status"].ToString().Trim());
                            stringBuilder.Append("</td>");
                            stringBuilder.Append("<td>");
                            stringBuilder.Append(row["StatusDate"].ToString().Trim());
                            stringBuilder.Append("</td>");
                        }
                        stringBuilder.Append("</tr>");
                    }
                    stringBuilder.Append("</table>");
                    lblreport.Text = stringBuilder.ToString().Trim();

                    Session["printData"] = stringBuilder.ToString().Trim();
                }
                else
                {
                    lblreport.Text = "<div style='color:Red;  font-weight:bold;  font-size:14px;' class='tbltd'>No records to display for the current filters.</div>";
                    Session["printData"] = "";
                }
            }
            catch (Exception ex)
            {
            }
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
        FillClassStream();
        fillstudent();
    }

    public void showdetail(object sender, EventArgs e)
    {
        LinkButton linkButton = sender as LinkButton;
        TableCell parent = (TableCell)linkButton.Parent;
        Label control1 = parent.FindControl("lblclassid") as Label;
        Label control2 = parent.FindControl("lblsession") as Label;
        string commandArgument = linkButton.CommandArgument;
        drpstudents.SelectedValue = linkButton.CommandArgument;
        drpclass.SelectedValue = control1.Text;
        drpSession.SelectedValue = control2.Text;
        bindsinglestudent();
    }

    protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    protected void ddlReligion_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblreport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("Student Details:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(lblreport.Text.ToString().Trim());
                stringBuilder.Append("<table><tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel(stringBuilder.ToString());
            }
            else
                Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/student details" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + str);
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptstudentdetailPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
    protected void btnprint1_Click(object sender, EventArgs e)
    {
        try
        {
            string session = drpSession.SelectedItem.Text.Trim();
            string Cls = drpclass.SelectedItem.Text.Trim();
            string sec = ddlSection.SelectedItem.Text.Trim();
            Response.Redirect("rptStudDetailsPrint.aspx?Session="+session+"&Class="+Cls+"&Sec="+sec+"");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
    protected void cbAll_CheckedChanged(object sender, EventArgs e)
    {
        if (!cbAll.Checked)
            return;
        cbAdmn.Checked = false;
        cbRollNo.Checked = false;
        cbAdmnDate.Checked = false;
        cbAdmnYear.Checked = false;
        cbClass.Checked = false;
        cbPresentSes.Checked = false;
        cbName.Checked = false;
        cbDOB.Checked = false;
        cbAge.Checked = false;
        cbSex.Checked = false;
        cbReligion.Checked = false;
        cbCat.Checked = false;
        cbFather.Checked = false;
        cbMother.Checked = false;
        cbLocalGrdName.Checked = false;
        cbStream.Checked = false;
        cbPresentAddress.Checked = false;
        cbTelephone.Checked = false;
        cbEmail.Checked = false;
        cbStatus.Checked = false;
        cbStatusDate.Checked = false;
    }

    private void chk()
    {
    }

    protected void cbAdmn_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbRollNo_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbAdmnDate_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbAdmnYear_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbClass_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbSection_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbPresentSes_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbName_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbDOB_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbSex_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbReligion_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbDenom_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbCat_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbFather_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbMother_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbLocalGrdName_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbStream_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbPresentAddress_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbTelephone_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbEmail_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }

    protected void cbAge_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
}