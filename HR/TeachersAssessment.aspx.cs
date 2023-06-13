using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_TeachersAssessment : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (!Page.IsPostBack)
        {
            objStatic.FillSessionYr(drpSession);
            drpSession.Items.RemoveAt(0);
            drpSession.Items.Insert(0, new ListItem("- SELECT -", "0"));
            BindTeachers();
        }
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
    }

    private void BindTeachers()
    {
        drpTeacher.DataSource = obj.GetDataTable("PS_GetTeacherList");
        drpTeacher.DataTextField = "TeacherName";
        drpTeacher.DataValueField = "TeacherId";
        drpTeacher.DataBind();
        drpTeacher.Items.Insert(0, new ListItem("- All Teachers -", "0"));
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        if (obj.ExecuteScalarQry("select COUNT(*) from dbo.PS_ClasswiseStudent where SessionYear='" + drpSession.SelectedValue + "' and StatusId=1 and Section=''").Trim() != "0")
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Please Assign Sections to all Students before viewing Teacher's Assessment";
        }
        else if (drpTeacher.SelectedIndex > 0)
            TeachersAssessmentIndv();
        else
            TeachersAssessment();
    }

    private void TeachersAssessmentIndv()
    {
        Decimal num1 = new Decimal(0);
        int num2 = 0;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table width='100%' cellpadding='2' cellspacing='0' style='border:solid 1px;border-bottom:none 1px;'>");
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SessionYr", drpSession.SelectedValue);
        hashtable.Add("@TeacherEmpId", drpTeacher.SelectedValue);
        DataTable dataTable = obj.GetDataTable("PS_TeacherAssessment", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center' style='border-bottom:solid 1px;' colspan='6'><b>Assessment Report of " + drpTeacher.SelectedItem.Text + " for the Session " + drpSession.SelectedValue + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr style='font-weight: bold;'>");
            stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>Sl No</td>");
            stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>Class</td>");
            stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>Section</td>");
            stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>Exam</td>");
            stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>Subject</td>");
            stringBuilder.Append("<td align='center' style='border-bottom:solid 1px;'>Avg. (%)</td>");
            stringBuilder.Append("</tr>");
            int num3 = 1;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                hashtable.Clear();
                hashtable.Add("@SessionYr", drpSession.SelectedValue);
                hashtable.Add("@ExamSubId", Convert.ToInt32(row["ExamSubId"].ToString()));
                hashtable.Add("@ClassId", Convert.ToInt32(row["ClassId"].ToString()));
                hashtable.Add("@Section", row["Section"].ToString());
                string str = obj.GetDataTable("ExamStudTotPercentage", hashtable).Rows[0]["TotPer"].ToString();
                if (str.Trim() != string.Empty && Convert.ToDecimal(str) > new Decimal(0))
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>" + num3 + "</td>");
                    stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>" + row["ClassName"].ToString() + "</td>");
                    stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>" + row["Section"].ToString() + "</td>");
                    stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>" + row["ExamName"].ToString() + "</td>");
                    stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>" + row["SubjectName"].ToString() + "</td>");
                    stringBuilder.Append("<td align='center' style='border-bottom:solid 1px;'>" + Convert.ToDecimal(str).ToString("0.00") + "</td>");
                    stringBuilder.Append("</tr>");
                    num1 += Convert.ToDecimal(str);
                    ++num2;
                    ++num3;
                }
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' colspan='5'><b>Avg. Assessment Percentage</b></td>");
            stringBuilder.Append("<td align='center' style='border-bottom:solid 1px;'>" + (num1 == 0 ? 0 : num1 / (Decimal)num2).ToString("0.00") + "</td>");
            stringBuilder.Append("</tr>");
            btnPrint.Visible = true;
        }
        else
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center' style='border-bottom:solid 1px;'><b>No Data Available</b></td>");
            stringBuilder.Append("</tr>");
            btnPrint.Visible = false;
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["rptTeacherAsst"] = stringBuilder.ToString();
        btnShow.Focus();
    }

    private void TeachersAssessment()
    {
        DataTable dataTable1 = new DataTable();
        dataTable1.Columns.Add(new DataColumn("EmpId", typeof(int)));
        dataTable1.Columns.Add(new DataColumn("TeacherName", typeof(string)));
        dataTable1.Columns.Add(new DataColumn("Avg", typeof(Decimal)));
        dataTable1.AcceptChanges();
        foreach (ListItem listItem in drpTeacher.Items)
        {
            Decimal num1 = new Decimal(0);
            int num2 = 0;
            if (listItem.Value != "0")
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("@SessionYr", drpSession.SelectedValue);
                hashtable.Add("@TeacherEmpId", listItem.Value);
                DataTable dataTable2 = obj.GetDataTable("PS_TeacherAssessment", hashtable);
                if (dataTable2.Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
                    {
                        hashtable.Clear();
                        hashtable.Add("@SessionYr", drpSession.SelectedValue);
                        hashtable.Add("@ExamSubId", Convert.ToInt32(row["ExamSubId"].ToString()));
                        hashtable.Add("@ClassId", Convert.ToInt32(row["ClassId"].ToString()));
                        hashtable.Add("@Section", row["Section"].ToString());
                        string str = obj.GetDataTable("ExamStudTotPercentage", hashtable).Rows[0]["TotPer"].ToString();
                        if (str.Trim() != string.Empty && Convert.ToDecimal(str) > new Decimal(0))
                        {
                            num1 += Convert.ToDecimal(str);
                            ++num2;
                        }   
                    }
                    DataRow row1 = dataTable1.NewRow();
                    row1["EmpId"] = listItem.Value;
                    row1["TeacherName"] = listItem.Text;
                    row1["Avg"] = num2 <= 0 ? num1 : (num1 / (Decimal)num2);
                    dataTable1.Rows.Add(row1);
                    dataTable1.AcceptChanges();
                }
            }
        }
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table width='100%' cellpadding='2' cellspacing='0' style='border:solid 1px;border-bottom:none 1px;'>");
        if (dataTable1.Rows.Count > 0)
        {
            DataView defaultView = dataTable1.DefaultView;
            defaultView.Sort = "Avg DESC";
            DataTable table = defaultView.ToTable();
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center' style='border-bottom:solid 1px;' colspan='4'><b>Teacher's Assessment Report for the Session " + drpSession.SelectedValue + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr style='font-weight: bold;'>");
            stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>Rank</td>");
            stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>Teacher's Name</td>");
            stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>Appointment Date</td>");
            stringBuilder.Append("<td align='center' style='border-bottom:solid 1px;'>Avg. Assessment %</td>");
            stringBuilder.Append("</tr>");
            int num = 1;
            foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
            {
                string str = obj.ExecuteScalarQry("SELECT CONVERT(NVARCHAR,AppointmentDt,106) FROM dbo.HR_EmployeeMaster WHERE EmpId=" + row["EmpId"].ToString());
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>" + num + "</td>");
                stringBuilder.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;'>" + row["TeacherName"].ToString() + "</td>");
                stringBuilder.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;'>" + str + "</td>");
                stringBuilder.Append("<td align='center' style='border-bottom:solid 1px;'>" + Convert.ToDecimal(row["Avg"]).ToString("0.00") + "</td>");
                stringBuilder.Append("</tr>");
                ++num;
            }
            btnPrint.Visible = true;
        }
        else
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center' style='border-bottom:solid 1px;'><b>No Data Available</b></td>");
            stringBuilder.Append("</tr>");
            btnPrint.Visible = false;
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["rptTeacherAsst"] = stringBuilder.ToString();
        btnShow.Focus();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("TeachersAssessmentPrint.aspx");
    }
}