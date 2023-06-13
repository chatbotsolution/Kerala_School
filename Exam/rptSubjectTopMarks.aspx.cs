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

public partial class Exam_rptSubjectTopMarks : System.Web.UI.Page
{
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Session["title"] = Page.Title.ToString();
        new clsStaticDropdowns().FillSessionYr(drpSession);
        BindDropDown(drpExamName, "SELECT ExamId,ExamName FROM dbo.ExamDetails ORDER BY ExamName", "ExamName", "ExamId");
        btnExport.Enabled = false;
        btnPrint.Enabled = false;
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---ALL---", "0"));
    }

    protected void drpExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpExamName.SelectedIndex > 0)
        {
            try
            {
                BindDropDown(drpSubject, "SELECT ExamSubId,ExamSubject,ExamId FROM dbo.ExamSubjects WHERE ExamId=" + drpExamName.SelectedValue.ToString() + "ORDER BY ExamSubject", "ExamSubject", "ExamSubId");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        else
            drpSubject.Items.Clear();
    }

    private void TopStudentReport(DataTable dt)
    {
        if (dt.Rows.Count >= 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table border='1' cellpadding='1' cellspacing='0'  width='97%'>");
            stringBuilder.Append("<tr border='1' cellpadding='1' cellspacing='0'  width='97%' style='background-color:silver;'>");
            stringBuilder.Append("<td colspan='4' align='center'><font color='Black' size='5'><b>Subject Wise Top Ten Student</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='4' align='right'><font color='red'> No Of Records: " + dt.Rows.Count.ToString() + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr style='background-color:silver;'>");
            stringBuilder.Append("<td style='width: 150px' align='left'><strong>Subject</strong></td>");
            stringBuilder.Append("<td style='width: 200px' align='left'><strong>Student Name</strong></td>");
            stringBuilder.Append("<td style='width: 80px' align='center'><strong>Class</strong></td>");
            stringBuilder.Append("<td style='width: 100px' align='center'><strong>Mark Obtained</strong></td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' >");
                stringBuilder.Append(row["ExamSubject"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["FullName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='center' >");
                stringBuilder.Append(row["ExamClass"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='center' >");
                stringBuilder.Append(row["MarksObtained"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["printData"] = stringBuilder.ToString().Trim();
            btnExport.Enabled = true;
            btnPrint.Enabled = true;
        }
        else
        {
            lblReport.Text = "No Records Found";
            btnPrint.Enabled = false;
            btnExport.Enabled = false;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(";WITH CTETopMarks AS (SELECT ROW_NUMBER() OVER ( PARTITION BY ExamSubjectId ORDER BY MarksObtained DESC ) AS 'RowNumber',ExamMarksId,EM.SessionYr,EM.ExamId,ExamName,ExamClass,EM.StudentId,FullName,EM.ExamSubjectId,ExamSubject,MarksObtained FROM dbo.ExamMarks EM INNER JOIN dbo.ExamDetails ED ON ED.ExamId=EM.ExamId INNER JOIN dbo.ExamSubjects ES ON ES.ExamSubId=EM.ExamSubjectId INNER JOIN dbo.StudMaster SM ON SM.StudId=EM.StudentId)SELECT ExamMarksId,SessionYr,ExamId,StudentId,FullName,ExamClass,ExamSubjectId,ExamSubject,MarksObtained FROM CTETopMarks WHERE RowNumber <= 10 ");
        if (drpSession.SelectedIndex > 0)
            stringBuilder.Append(" and SessionYr='" + drpSession.SelectedValue.ToString() + "'");
        if (drpExamName.SelectedIndex > 0)
            stringBuilder.Append(" and ExamId='" + drpExamName.SelectedValue.ToString() + "'");
        if (drpSubject.SelectedIndex > 0)
            stringBuilder.Append(" and ExamSubjectId='" + drpSubject.SelectedValue.ToString() + "'");
        stringBuilder.Append(" ORDER BY MarksObtained DESC");
        dt = obj.GetDataTableQry(stringBuilder.ToString());
        if (dt.Rows.Count >= 0)
        {
            TopStudentReport(dt);
        }
        else
        {
            lblReport.Text = "No Records To Display.";
            btnExport.Visible = false;
            btnPrint.Visible = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("SubjectTopMarksPrint.aspx");
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
            string str = Server.MapPath("../Up_Files/reportfiles/SubjectTopMarks" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("<tr><td align='left'><b>");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + lblReport.Text.ToString().Trim()).ToString().Trim());
            }
            else
                Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}