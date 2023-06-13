using ASP;
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

public partial class Exam_rptCandidateAbsent : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        clsStaticDropdowns clsStaticDropdowns = new clsStaticDropdowns();
        Session["printStudExamAttendance"] = string.Empty;
        BindSession();
        BindDrp(drpClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
        clsStaticDropdowns.FillSection(drpSection);
    }

    private void BindSession()
    {
        ViewState["Session"] = obj.ExecuteScalarQry("select top 1 SessionYr from dbo.ExamDetails order by UserDt desc");
        DataTable dataTableQry = obj.GetDataTableQry("SELECT DISTINCT SessionYr FROM ExamMarks ORDER BY SessionYr DESC");
        if (dataTableQry.Rows.Count > 0)
        {
            drpSession.DataSource = dataTableQry;
            drpSession.DataValueField = "SessionYr";
            drpSession.DataTextField = "SessionYr";
            drpSession.DataBind();
            drpClass.Enabled = true;
            drpExamName.Enabled = true;
        }
        else
        {
            drpSession.Items.Insert(0, new ListItem("- No Exams Defined -", "0"));
            drpClass.Enabled = false;
            drpExamName.Enabled = false;
        }
    }

    private void BindDrp(DropDownList drp, string query, string text, string value)
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = text;
        drp.DataValueField = value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        BindExam();
        drpSession.Focus();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        BindExam();
        drpClass.Focus();
    }

    private void BindExam()
    {
        if (drpClass.SelectedIndex <= 0)
            return;
        drpExamName.DataSource = obj.GetDataTable("ExamsForGenResult", new Hashtable()
    {
      {
         "@ClassID",
         drpClass.SelectedValue
      },
      {
         "@SessionYr",
         drpSession.SelectedValue
      }
    });
        drpExamName.DataTextField = "ExamName";
        drpExamName.DataValueField = "ExamClassId";
        drpExamName.DataBind();
        drpExamName.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        if (drpClass.SelectedIndex > 0 && drpExamName.SelectedIndex > 0 && drpSubject.SelectedIndex > 0)
            GenerateReport();
        drpSection.Focus();
    }

    protected void drpExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        drpSubject.Items.Clear();
        BindExamSub();
        drpExamName.Focus();
    }

    private void BindExamSub()
    {
        if (drpExamName.SelectedIndex <= 0)
            return;
        obj = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("ExamGetExamwiseSub", new Hashtable()
    {
      {
         "@ExamId",
         drpExamName.SelectedValue
      }
    });
        if (dataTable2.Rows.Count <= 0)
            return;
        drpSubject.DataSource = dataTable2;
        drpSubject.DataValueField = "ExamSubId";
        drpSubject.DataTextField = "SubjectName";
        drpSubject.DataBind();
        drpSubject.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    protected void drpSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        if (drpClass.SelectedIndex > 0 && drpExamName.SelectedIndex > 0 && drpSubject.SelectedIndex > 0)
            GenerateReport();
        drpSubject.Focus();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        GenerateReport();
    }

    private void GenerateReport()
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
        hashtable.Add("@ClassId", drpClass.SelectedValue.ToString());
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue.ToString());
        hashtable.Add("@ExamId", drpExamName.SelectedValue.ToString());
        hashtable.Add("@ExamSubId", drpSubject.SelectedValue);
        hashtable.Add("@Status", drpStatus.SelectedIndex);
        DataTable dataTable2 = obj.GetDataTable("ExamRptExAttendance", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table border='0' cellpadding='0' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='2' align='left'><font color='Black'><strong>Session&nbsp;:&nbsp;</strong>" + drpSession.SelectedValue.ToString());
            stringBuilder.Append("&nbsp;&nbsp;&nbsp;<strong>Class&nbsp;:&nbsp;</strong>" + drpClass.SelectedItem.Text);
            stringBuilder.Append("&nbsp;&nbsp;&nbsp;<strong>Examination&nbsp;:&nbsp;</strong>" + drpExamName.SelectedItem.Text);
            stringBuilder.Append("&nbsp;&nbsp;&nbsp;<strong>Subject&nbsp;:&nbsp;</strong>" + drpSubject.SelectedItem.Text + "</font></td>");
            stringBuilder.Append("<td align='right'><font color='Black'><strong>No Of Records&nbsp;:&nbsp;</strong>" + dataTable2.Rows.Count.ToString() + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table cellpadding='1' cellspacing='0' border='1' width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 30%' align='left'><strong>Admission No</strong></td>");
            stringBuilder.Append("<td style='width: 40%' align='left'><strong>Student Name</strong></td>");
            stringBuilder.Append("<td style='width: 30%' align='center'><strong>Present/Absent</strong></td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' >");
                stringBuilder.Append(row["AdmnNo"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' >");
                stringBuilder.Append(row["FullName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='center'>");
                stringBuilder.Append(row["ExStatus"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["printStudExamAttendance"] = stringBuilder.ToString().Trim();
            btnExport.Enabled = true;
            btnPrint.Enabled = true;
        }
        else
        {
            lblReport.Text = string.Empty;
            Session["printStudExamAttendance"] = string.Empty;
            btnExport.Enabled = false;
            btnPrint.Enabled = false;
        }
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str1 = "ExamAttendance_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".xls";
            string str2 = Server.MapPath("../Up_Files/Exported_Files/" + str1);
            FileStream fileStream = new FileStream(str2, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + str1);
            Response.WriteFile(str2);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != string.Empty)
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td align='left'><b>");
                stringBuilder.Append("Examination Attendance Report :-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + lblReport.Text.ToString().Trim()).ToString().Trim());
            }
            else
                Response.Write("<script language='javascript'>alert('No Data Exists to Export');</script>");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptCandidateAbsentPrint.aspx");
    }

    protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        if (drpClass.SelectedIndex > 0 && drpExamName.SelectedIndex > 0 && drpSubject.SelectedIndex > 0)
            GenerateReport();
        drpStatus.Focus();
    }
}