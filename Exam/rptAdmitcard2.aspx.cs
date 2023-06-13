using ASP;
using Classes.DA;
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

public partial class Exam_rptAdmitcard2 : System.Web.UI.Page
{
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        txtadminno.Text = "0";
        fillsession();
        fillclass();
        FillClassSection();
        fillstudent();
        fillExam();
        btnPrint.Visible = false;
    }

    protected void fillsession()
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
        drpclass.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    private void fillExam()
    {
        drpExam.Items.Clear();
        drpExam.DataSource = new Common().ExecuteSql("select [ExamId],[ExamName] from [dbo].[ExamDetails] order by [ExamName] ");
        drpExam.DataTextField = "ExamName";
        drpExam.DataValueField = "ExamId";
        drpExam.DataBind();
        drpExam.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    private void FillClassSection()
    {
        drpSection.Items.Clear();
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "@classId",
         drpclass.SelectedValue.ToString().Trim()
      },
      {
         "@session",
         drpSession.SelectedValue.ToString().Trim()
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpclass.SelectedIndex > 0)
        {
            FillClassSection();
            fillstudent();
            drpSection.SelectedIndex = 0;
            lblReport.Text = "";
            btnPrint.Visible = false;
            txtadminno.Text = "0";
        }
        else
        {
            drpSection.SelectedIndex = 0;
            drpstudents.SelectedIndex = 0;
            txtadminno.Text = "0";
            lblReport.Text = "";
            btnPrint.Visible = false;
        }
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSection.SelectedIndex > 0)
        {
            fillstudent();
            lblReport.Text = "";
            btnPrint.Visible = false;
            txtadminno.Text = "0";
        }
        else
        {
            drpstudents.SelectedIndex = 0;
            txtadminno.Text = "0";
            lblReport.Text = "";
            btnPrint.Visible = false;
        }
    }

    private void fillstudent()
    {
        drpstudents.Items.Clear();
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@Session", drpSession.SelectedValue);
        if (drpclass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpclass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        DataTable dataTable = common.GetDataTable("ps_sp_get_StudNamesForMly", hashtable);
        drpstudents.DataSource = dataTable;
        drpstudents.DataTextField = "fullname";
        drpstudents.DataValueField = "admnno";
        drpstudents.DataBind();
        if (dataTable.Rows.Count > 0)
            drpstudents.Items.Insert(0, new ListItem("--ALL--", "0"));
        else
            drpstudents.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void drpstudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpstudents.SelectedIndex > 0)
        {
            string query ="Select OldAdmnno from Ps_StudMaster where admnno="+ drpstudents.SelectedValue ;
            DataTable dt = new clsDAL().GetDataTableQry(query);
            if (dt.Rows.Count > 0)
                txtadminno.Text = dt.Rows[0]["OldAdmnno"].ToString();
            else
                txtadminno.Text = "";

            lblReport.Text = "";
            btnPrint.Visible = false;
        }
        else
        {
            txtadminno.Text = "0";
            lblReport.Text = "";
            btnPrint.Visible = false;
        }
    }

    private void FillStudent()
    {
        try
        {
            lblReport.Text = "";
            btnPrint.Visible = false;
            Common common = new Common();
            if (Convert.ToInt32(common.ExecuteScalarQry("select count(*) from PS_StudMaster where AdmnNo=" + txtadminno.Text)) == 0)
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Admission no. does not exists')", true);
            }
            else
            {
                DataTable dataTable = common.ExecuteSql("select SessionYear,section,ClassID from PS_ClasswiseStudent where Detained_Promoted='' and  admnno=" + txtadminno.Text);
                drpSession.SelectedValue = dataTable.Rows[0]["SessionYear"].ToString();
                fillclass();
                drpclass.SelectedValue = dataTable.Rows[0]["ClassID"].ToString();
                FillClassSection();
                drpSection.SelectedValue = dataTable.Rows[0]["section"].ToString();
                fillstudent();
                drpstudents.SelectedValue = txtadminno.Text;
            }
        }
        catch (Exception ex)
        {
            if (!(ex.Message != ""))
                return;
            ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Invalid Data')", true);
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        if (!(drpstudents.SelectedValue != txtadminno.Text.Trim()))
            return;
        FillStudent();
    }

    private string Information()
    {
        clsDAL clsDal1 = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataSet dataSet = new DataSet();
        StringBuilder stringBuilder = new StringBuilder();
        clsDAL clsDal2 = new clsDAL();
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
                    stringBuilder.Append("<center>");
                    stringBuilder.Append("<table width='97%' class='tbltd' cellpadding='2' cellspacing='2'>");
                    stringBuilder.Append("<tr><td rowspan='4'> <img src='../images/logo-new.png' width='75px' height='80px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + clsDal2.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>EXAM &nbsp;:&nbsp;</b><strong>" + drpExam.SelectedItem.ToString() + "("+drpSession.SelectedItem.ToString()+")</strong></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong><u>ADMIT CARD</u></strong></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'></td></tr></table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        getdata();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptAdmitcard2Print.aspx");
    }

    protected void drpForCls_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
    }

    private void getdata()
    {
       
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
        if (drpclass.SelectedValue.ToString() != "0")
            hashtable.Add("@ClassId", drpclass.SelectedValue.ToString().Trim());
        if (drpExam.SelectedValue.ToString() != "0")
            hashtable.Add("@ExamId", drpExam.SelectedValue.ToString().Trim());
        if (drpSection.SelectedValue.ToString() != "0")
            hashtable.Add("@section", drpSection.SelectedValue.ToString().Trim());
        if (drpstudents.SelectedValue.ToString() != "0")
            hashtable.Add("@Admnno", drpstudents.SelectedValue.ToString().Trim());
        if (txtRollfrom .Text.ToString() != string.Empty || txtRollfrom.Text.ToString() != "")
            hashtable.Add("@FrmRollNo", txtRollfrom.Text.ToString().Trim());
        if (txtRollNoTo.Text.ToString() != string.Empty || txtRollNoTo.Text.ToString() != "")
            hashtable.Add("@ToRollNo", txtRollNoTo.Text.ToString().Trim());
        DataTable dataTable2 = clsDal.GetDataTable("ExamGetStudsForAdmitCard", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            btnPrint.Enabled = true;
            btnPrint.Visible = true;
            StringBuilder stringBuilder = new StringBuilder();
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                string str = Information();
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0'>");
                stringBuilder.Append("<tr><td colspan='4'><br /></tr></td>");
                stringBuilder.Append("<tr><td>");
                stringBuilder.Append(str);
                stringBuilder.Append("<table width='97%' class='txtadmitcard' cellpadding='2' cellspacing='2'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append(" <td align='left' colspan='2' style='width: 100px; white-space: nowrap; height: 14px;'><b>Student Name</b>&nbsp;:&nbsp;" + row["FullName"].ToString() + "</td>");
                stringBuilder.Append("<td style='border: thin solid #808080; width: 159px;'><b>Exam Room</b>&nbsp;:&nbsp;" + txtExamRoom.Text.Trim() + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' style='height: 14px'><b>Class</b>&nbsp;:&nbsp;" + row["ClassName"].ToString() + " &nbsp;&nbsp<b> Section</b> &nbsp;:&nbsp;" + row["Section"].ToString() +" &nbsp; <b>Roll No</b> :&nbsp;"+ row["RollNo"].ToString() + "</td>");
                stringBuilder.Append(" <td align='left' style='height: 14px'><b>Admn No</b>&nbsp;:&nbsp;" + row["AdmnNo"].ToString() + "</td>");
                stringBuilder.Append("<td rowspan='4'>&nbsp;&nbsp;&nbsp;<img src='../Up_Files/Studimage/" + row["StudentPhoto"] + "' height='150' width='140px'> </td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' style='height: 14px;'><b>Date Of Birth</b>&nbsp;:&nbsp;" + row["DOBStr"].ToString() + "</td>");
                stringBuilder.Append("<td align='left' style='height: 14px;'><b>Gender:</b> &nbsp;" + row["sex"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left'  style='height: 14px;'><b>Mother's Name</b>&nbsp;:&nbsp;" + row["MotherName"].ToString() + "</td>");
               // stringBuilder.Append(" </tr>");
              //  stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left'  style='height: 14px;'><b>Father's Name</b>&nbsp;:&nbsp;" + row["FatherName"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' style='height: 14px'><b>Exam Start Date</b>&nbsp;:&nbsp;" + row["ExamFromDateStr"].ToString() + "</td>");
                stringBuilder.Append("<td align='left' style='height: 14px'><b>Exam End Date</b>&nbsp;:&nbsp;" + row["ExamToDateStr"].ToString() + "</td>");
               // stringBuilder.Append("<td  height: 14px; width: 159px;'><b>Exam room</b>&nbsp;:&nbsp;"+ txtExamRoom.Text.Trim()+  "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='3'>");
                stringBuilder.Append("<table>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td  class='txtadmitcard' style='height: 25px; border: thin solid #808080; width:200px;' align='center' valign='bottom'><img src='../images/principal.jpg' height='50' width='110'><br /><strong>Principal</strong></td>");
                stringBuilder.Append("<td  class='txtadmitcard' style='border: thin solid #808080; width:500px;' align='center' valign='bottom'><br /><br /><strong>Candidate's Signature</strong></td>");
                stringBuilder.Append("<td  class='txtadmitcard' style='border: thin solid #808080; width:200px;' align='center' valign='bottom'><br /><br /><strong>Class Teacher</strong></td>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("</tr><tr><td colspan='4'><br /><br /><hr /></td></tr>");
                stringBuilder.Append(" </table>");
                stringBuilder.Append("<tr><td></td></tr></table>");
                stringBuilder.Append("</center>");
            }
            lblReport.Text = stringBuilder.ToString();
            Session["admitcardprint"] = stringBuilder.ToString();
        }
        else
        {
            btnPrint.Enabled = false;
            btnPrint.Visible = false;
        }
    }

    protected void drpExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (txtExamRoom.Text == "")
            lblReport.Text = "";
        else if (drpExam.SelectedIndex == 0)
             lblReport.Text = "";
        else
            getdata();
    }
}