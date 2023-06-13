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

public partial class Exam_rptResult : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session["title"] = Page.Title.ToString();
            if (Session["User_Id"] != null)
            {
                BindSession();
                bindDropDown(drpClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
                objStatic.FillSection(drpSection);
                drpSection.Items.RemoveAt(0);
                drpSection.Items.Insert(0, new ListItem("- ALL -", "0"));
            }
            else
                Response.Redirect("../Login.aspx");
        }
        btnExport.Enabled = false;
        btnPrint.Enabled = false;
    }

    private void BindSession()
    {
        ViewState["Session"] = obj.ExecuteScalarQry("select top 1 SessionYr from dbo.ExamDetails order by UserDt desc");
        DataTable dataTableQry = obj.GetDataTableQry("SELECT DISTINCT SessionYr FROM ExamDetails ORDER BY SessionYr DESC");
        if (dataTableQry.Rows.Count > 0)
        {
            drpSession.DataSource = dataTableQry;
            drpSession.DataValueField = "SessionYr";
            drpSession.DataTextField = "SessionYr";
            drpSession.DataBind();
            drpSession.Items.Insert(0, new ListItem("- Select -", "0"));
            drpClass.Enabled = true;
            drpExam.Enabled = true;
        }
        else
        {
            drpSession.Items.Insert(0, new ListItem("- No Exams Defined -", "0"));
            drpClass.Enabled = false;
            drpExam.Enabled = false;
        }
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        drpExam.Items.Clear();
        if (drpClass.SelectedIndex > 0 && drpSession.SelectedIndex > 0)
        {
            DataTable dataTable = obj.GetDataTable("ExamsForGenResult", new Hashtable()
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
            if (dataTable.Rows.Count > 0)
            {
                drpExam.DataSource = dataTable;
                drpExam.DataTextField = "ExamName";
                drpExam.DataValueField = "ExamClassId";
                drpExam.DataBind();
                drpExam.Items.Insert(0, new ListItem("- Select -", "0"));
            }
        }
        drpSession.Focus();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        drpExam.Items.Clear();
        if (drpSession.SelectedIndex > 0 && drpClass.SelectedIndex > 0)
        {
            DataTable dataTable = obj.GetDataTable("ExamsForGenResult", new Hashtable()
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
            drpExam.DataSource = dataTable;
            if (dataTable.Rows.Count > 0)
            {
                drpExam.DataTextField = "ExamName";
                drpExam.DataValueField = "ExamClassId";
                drpExam.DataBind();
                drpExam.Items.Insert(0, new ListItem("- ALL -", "0"));
            }
        }
        drpClass.Focus();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (Session["printExamResult"] != null)
        {
            try
            {
                ExportToExcel(Session["printExamResult"].ToString());
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('No Data exists to Export');", true);
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str1 = "ExamResult_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".xls";
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

    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (drpExam.SelectedIndex == 0)
            ConsolidatedReport();
        else
            GenerateReport();
        btnShow.Focus();
    }

    private void GenerateReport()
    {
        try
        {
            lblReport.Text = string.Empty;
            DataTable dataTable1 = new DataTable();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
            hashtable.Add("@ClassId", drpClass.SelectedValue.ToString());
            if (drpExam.SelectedIndex > 0)
                hashtable.Add("@ExamId", drpExam.SelectedValue.ToString());
            if (drpSection.SelectedIndex > 0)
                hashtable.Add("@Section", drpSection.SelectedValue.ToString());
            if (drpStatus.SelectedIndex > 0)
                hashtable.Add("@Result", drpStatus.SelectedValue.ToString());
            DataTable dataTable2 = obj.GetDataTable("ExamRptGetResults", hashtable);
            StringBuilder stringBuilder = new StringBuilder();
            if (dataTable2.Rows.Count > 0)
            {
                DataTable dataTable3 = new DataTable();
                DataSet dataSet = new DataSet();
                string str1 = Server.MapPath("../XMLFiles") + "\\Detail.xml";
                if (File.Exists(str1))
                {
                    int num = (int)dataSet.ReadXml(str1);
                    string str2 = Session["SSVMID"].ToString();
                    if (dataSet.Tables.Count > 0)
                    {
                        DataTable dataTable4 = new DataTable();
                        DataTable table = dataSet.Tables[0];
                        foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                        {
                            stringBuilder.Append("<table width='100%' cellpadding='2' cellspacing='2' align='center'>");
                            stringBuilder.Append("<tr><td rowspan='4' align='center' > <img src='../images/leftlogo.jpg' width='75px' height='80px'/></td>");
                            stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + obj.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/></td></tr>");
                            stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + obj.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + ", " + obj.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</strong></td></tr>");
                            stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + obj.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + obj.Decrypt(row["Pin"].ToString().Trim(), str2) + "</strong> </td></tr>");
                            stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>Phone-" + obj.Decrypt(row["Phone"].ToString().Trim(), str2) + "</strong></td></tr><tr><td colspan='5'></td></tr></table>");
                        }
                    }
                }
                stringBuilder.Append("<table width='98%' border='1' cellspacing='0' cellpadding='0' style='border-collapse:collapse;'>");
                stringBuilder.Append("<tr align='center'><td colspan='9'><strong>Exam Result</strong></td></tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='border-right-style:none' colspan='6' align='left'><font color='Black'><strong>Session&nbsp;:&nbsp;</strong>" + drpSession.SelectedValue.ToString());
                stringBuilder.Append("&nbsp;&nbsp;&nbsp;<strong>Examination&nbsp;:&nbsp;</strong>" + drpExam.SelectedItem.ToString());
                stringBuilder.Append("&nbsp;&nbsp;&nbsp;<strong>Class&nbsp;:&nbsp;</strong>" + drpClass.SelectedItem.ToString());
                stringBuilder.Append("&nbsp;&nbsp;&nbsp;<strong>Section&nbsp;:&nbsp;</strong>" + drpSection.SelectedItem.ToString() + "</font></td>");
                stringBuilder.Append("<td style='border-left-style:none' align='right' colspan='3'><font color='Black'><strong>No Of Records&nbsp;:&nbsp;</strong>" + dataTable2.Rows.Count.ToString() + "</font>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr style='font-weight:bold;'>");
                if (drpStatus.SelectedIndex > 1)
                    stringBuilder.Append("<td width='10%' align='center'>Sl.No.</td>");
                else
                    stringBuilder.Append("<td width='10%' align='center'>Rank</td>");
                stringBuilder.Append("<td width='20%' align='left'>Student Name</td>");
                stringBuilder.Append("<td width='10%' align='left'>Admission No.</td>");
                stringBuilder.Append("<td width='10%' align='center'>Full Marks</td>");
                stringBuilder.Append("<td width='10%' align='center'>Marks Obtained</td>");
                stringBuilder.Append("<td width='10%' align='center'>Percentage</td>");
                stringBuilder.Append("<td width='10%' align='center'>Grade</td>");
                stringBuilder.Append("<td width='10%' align='center'>Result</td>");
                stringBuilder.Append("</tr>");
                int num1 = 0;
                string empty = string.Empty;
                Decimal num2 = new Decimal(0);
                foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
                {
                    if (row["MarksPercent"].ToString().Trim() != string.Empty && Convert.ToDecimal(row["MarksPercent"]) != num2)
                    {
                        ++num1;
                        num2 = Convert.ToDecimal(row["MarksPercent"]);
                    }
                    else if (row["MarksPercent"].ToString().Trim() == string.Empty)
                    {
                        ++num1;
                        num2 = new Decimal(0);
                    }
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='center'>" + num1.ToString() + "</td>");
                    stringBuilder.Append("<td align='left'>" + row["FullName"].ToString() + "</td>");
                    stringBuilder.Append("<td align='left'>" + row["AdmnNo"].ToString() + "</td>");
                    if (row["FullMarks"].ToString().Trim() != string.Empty)
                        stringBuilder.Append("<td align='center'>" + Convert.ToDecimal(row["FullMarks"]).ToString("0.00") + "</td>");
                    else
                        stringBuilder.Append("<td align='center'>N/A</td>");
                    if (row["MarksObtained"].ToString().Trim() != string.Empty)
                        stringBuilder.Append("<td align='center'>" + Convert.ToDecimal(row["MarksObtained"]).ToString("0.00") + "</td>");
                    else
                        stringBuilder.Append("<td align='center'>N/A</td>");
                    if (row["MarksPercent"].ToString().Trim() != string.Empty)
                    {
                        stringBuilder.Append("<td align='center'>" + Convert.ToDecimal(row["MarksPercent"]).ToString("0.00") + " %</td>");
                        stringBuilder.Append("<td align='center' >" + GetGrade(Convert.ToDecimal(row["MarksPercent"])) + "</td>");
                    }
                    else
                    {
                        stringBuilder.Append("<td align='center'>N/A</td>");
                        stringBuilder.Append("<td align='center'>N/A</td>");
                    }
                    stringBuilder.Append("<td align='center'>" + row["Result"].ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("</table>");
                lblReport.Text = stringBuilder.ToString().Trim();
                Session["printExamResult"] = stringBuilder.ToString().Trim();
                btnExport.Enabled = true;
                btnPrint.Enabled = true;
            }
            else
            {
                lblReport.Text = "No Data Found";
                Session["printExamResult"] = string.Empty;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void ConsolidatedReport()
    {
        lblReport.Text = string.Empty;
        DataSet dataSet1 = new DataSet();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = new DataTable();
        DataTable dataTable3 = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@ClassId", drpClass.SelectedValue);
        hashtable.Add("@SessionYr", drpSession.SelectedValue);
        DataTable dataTable4 = obj.GetDataTable("ExamGetExamNameForRpt", hashtable);
        hashtable.Clear();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
        hashtable.Add("@ClassId", drpClass.SelectedValue.ToString());
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue.ToString());
        DataSet dataSet2 = obj.GetDataSet("ExamRptGetResults", hashtable);
        DataTable table1 = dataSet2.Tables[0];
        DataTable table2 = dataSet2.Tables[1];
        StringBuilder stringBuilder = new StringBuilder();
        DataRow[] dataRowArray1 = dataTable4.Select("ExamType='UT'");
        DataRow[] dataRowArray2 = dataTable4.Select("ExamType='PT'");
        DataRow[] dataRowArray3 = dataTable4.Select("ExamType='TE'");
        DataRow[] dataRowArray4 = dataTable4.Select("ExamType='PB'");
        DataRow[] dataRowArray5 = dataTable4.Select("ExamType='HF'");
        DataRow[] dataRowArray6 = dataTable4.Select("ExamType='AN'");
        DataTable dataTable5 = new DataTable();
        DataSet dataSet3 = new DataSet();
        string str1 = Server.MapPath("../XMLFiles") + "\\Detail.xml";
        if (File.Exists(str1))
        {
            int num = (int)dataSet3.ReadXml(str1);
            string str2 = Session["SSVMID"].ToString();
            if (dataSet3.Tables.Count > 0)
            {
                DataTable dataTable6 = new DataTable();
                DataTable table3 = dataSet3.Tables[0];
                foreach (DataRow row in (InternalDataCollectionBase)dataSet3.Tables[0].Rows)
                {
                    stringBuilder.Append("<table width='100%' cellpadding='2' cellspacing='2' align='center'>");
                    stringBuilder.Append("<tr><td rowspan='4' align='center' > <img src='../images/leftlogo.jpg' width='75px' height='80px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + obj.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + obj.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + ", " + obj.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</strong></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + obj.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + obj.Decrypt(row["Pin"].ToString().Trim(), str2) + "</strong> </td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>Phone-" + obj.Decrypt(row["Phone"].ToString().Trim(), str2) + "</strong></td></tr><tr><td colspan='5'></td></tr></table>");
                }
            }
        }
        stringBuilder.Append("<table width='98%' border='1' cellspacing='0' cellpadding='0' style='border-collapse:collapse;'>");
        stringBuilder.Append("<tr align='center'><td colspan='2'><strong>Exam Result</strong></td></tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='border-right-style:none' align='left'>");
        stringBuilder.Append("<strong>Session&nbsp;:&nbsp;</strong>" + drpSession.SelectedValue.ToString());
        stringBuilder.Append("&nbsp;&nbsp;&nbsp;<strong>Class&nbsp;:&nbsp;</strong>" + drpClass.SelectedItem.ToString());
        stringBuilder.Append("&nbsp;&nbsp;&nbsp;<strong>Section&nbsp;:&nbsp;</strong>" + drpSection.SelectedItem.ToString() + "</font></td>");
        stringBuilder.Append("<td style='border-left-style:none' align='right'><font color='Black'><strong>No Of Records&nbsp;:&nbsp;</strong>" + table1.Rows.Count.ToString() + "</font>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table width='98%' border='1' cellspacing='0' cellpadding='0' style='border-collapse:collapse;'>");
        stringBuilder.Append("<tr style='font-weight:bold;'>");
        stringBuilder.Append("<td width='5%' align='center'>Rank</td>");
        stringBuilder.Append("<td width='15%' align='left'>Student Name</td>");
        stringBuilder.Append("<td width='10%' align='left'>Admission No.</td>");
        if (dataRowArray1.Length > 0)
        {
            foreach (DataRow dataRow in dataRowArray1)
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + dataRow["ExamName"] + "</b></td>");
        }
        if (dataRowArray5.Length > 0)
            stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>Half Yearly Exam</b></td>");
        if (dataRowArray2.Length > 0)
            stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>Pre-Test Exam</b></td>");
        if (dataRowArray3.Length > 0)
            stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>Test Exam</b></td>");
        if (dataRowArray4.Length > 0)
            stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>Pre-Board Exam</b></td>");
        if (dataRowArray6.Length > 0)
            stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>Annual Exam</b></td>");
        stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>Aggr. F.M.</b></td>");
        stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>Aggr. M.Obt.</b></td>");
        stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>Percentage</b></td>");
        stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>Grade</b></td>");
        stringBuilder.Append("</tr>");
        int num1 = 0;
        string empty = string.Empty;
        float num2 = 0.0f;
        foreach (DataRow row in (InternalDataCollectionBase)table1.Rows)
        {
            float num3 = 0.0f;
            float num4 = 0.0f;
            if (row["MarksObtained"].ToString().Trim() != string.Empty && (double)float.Parse(row["MarksObtained"].ToString()) != (double)num2)
            {
                ++num1;
                num2 = float.Parse(row["MarksObtained"].ToString());
            }
            else if (row["MarksObtained"].ToString().Trim() == string.Empty)
            {
                ++num1;
                num2 = 0.0f;
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center'>" + num1.ToString() + "</td>");
            stringBuilder.Append("<td align='left'>" + row["FullName"].ToString() + "</td>");
            stringBuilder.Append("<td align='left'>" + row["AdmnNo"].ToString() + "</td>");
            DataView dataView = new DataView(table2, "AdmnNo=" + row["AdmnNo"] + " AND ExamType='UT'", "", DataViewRowState.CurrentRows);
            DataRow[] dataRowArray7 = table2.Select("AdmnNo=" + row["AdmnNo"] + " AND ExamType='PT'");
            DataRow[] dataRowArray8 = table2.Select("AdmnNo=" + row["AdmnNo"] + " AND ExamType='TE'");
            DataRow[] dataRowArray9 = table2.Select("AdmnNo=" + row["AdmnNo"] + " AND ExamType='PB'");
            DataRow[] dataRowArray10 = table2.Select("AdmnNo=" + row["AdmnNo"] + " AND ExamType='HF'");
            DataRow[] dataRowArray11 = table2.Select("AdmnNo=" + row["AdmnNo"] + " AND ExamType='AN'");
            int num5 = 1;
            if (dataRowArray1.Length > 0)
            {
                foreach (DataRow dataRow1 in dataRowArray1)
                {
                    if (num5 == 1)
                        num3 += float.Parse(dataRow1["FullMarks"].ToString());
                    if (dataView.Count > 0)
                    {
                        DataRow[] dataRowArray12 = dataView.ToTable().Select("ExamId=" + dataRow1["ExamClassId"].ToString().Trim());
                        if (dataRowArray12.Length > 0)
                        {
                            DataRow[] dataRowArray13 = dataRowArray12;
                            int index = 0;
                            if (index < dataRowArray13.Length)
                            {
                                DataRow dataRow2 = dataRowArray13[index];
                                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MarksObtained"] + "</td>");
                                num4 += float.Parse(dataRow2["MarksObtained"].ToString());
                            }
                        }
                        else
                            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                    }
                    else
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                }
            }
            int num6 = 1;
            if (dataRowArray2.Length > 0)
            {
                foreach (DataRow dataRow1 in dataRowArray2)
                {
                    if (num6 == 1)
                        num3 += float.Parse(dataRow1["FullMarks"].ToString());
                    if (dataRowArray7.Length > 0)
                    {
                        foreach (DataRow dataRow2 in dataRowArray7)
                        {
                            if (dataRow2["ExamId"].ToString().Trim() == dataRow1["ExamClassId"].ToString().Trim())
                            {
                                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MarksObtained"] + "</td>");
                                num4 += float.Parse(dataRow2["MarksObtained"].ToString());
                                break;
                            }
                        }
                    }
                    else
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                }
            }
            int num7 = 1;
            foreach (DataRow dataRow1 in dataRowArray3)
            {
                if (num7 == 1)
                    num3 += float.Parse(dataRow1["FullMarks"].ToString());
                if (dataRowArray8.Length > 0)
                {
                    foreach (DataRow dataRow2 in dataRowArray8)
                    {
                        if (dataRow2["ExamId"].ToString().Trim() == dataRow1["ExamClassId"].ToString().Trim())
                        {
                            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MarksObtained"] + "</td>");
                            num4 += float.Parse(dataRow2["MarksObtained"].ToString());
                            break;
                        }
                    }
                }
                else
                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
            }
            int num8 = 1;
            foreach (DataRow dataRow1 in dataRowArray4)
            {
                if (num8 == 1)
                    num3 += float.Parse(dataRow1["FullMarks"].ToString());
                if (dataRowArray9.Length > 0)
                {
                    foreach (DataRow dataRow2 in dataRowArray9)
                    {
                        if (dataRow2["ExamId"].ToString().Trim() == dataRow1["ExamClassId"].ToString().Trim())
                        {
                            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MarksObtained"] + "</td>");
                            num4 += float.Parse(dataRow2["MarksObtained"].ToString());
                            break;
                        }
                    }
                }
                else
                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
            }
            int num9 = 1;
            foreach (DataRow dataRow1 in dataRowArray5)
            {
                if (num9 == 1)
                    num3 += float.Parse(dataRow1["FullMarks"].ToString());
                if (dataRowArray10.Length > 0)
                {
                    foreach (DataRow dataRow2 in dataRowArray10)
                    {
                        if (dataRow2["ExamId"].ToString().Trim() == dataRow1["ExamClassId"].ToString().Trim())
                        {
                            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MarksObtained"] + "</td>");
                            num4 += float.Parse(dataRow2["MarksObtained"].ToString());
                            break;
                        }
                    }
                }
                else
                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
            }
            int num10 = 1;
            foreach (DataRow dataRow1 in dataRowArray6)
            {
                if (num10 == 1)
                    num3 += float.Parse(dataRow1["FullMarks"].ToString());
                if (dataRowArray11.Length > 0)
                {
                    foreach (DataRow dataRow2 in dataRowArray11)
                    {
                        if (dataRow2["ExamId"].ToString().Trim() == dataRow1["ExamClassId"].ToString().Trim())
                        {
                            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MarksObtained"] + "</td>");
                            num4 += float.Parse(dataRow2["MarksObtained"].ToString());
                            break;
                        }
                    }
                }
                else
                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
            }
            stringBuilder.Append("<td align='center'>" + num3 + "</td>");
            stringBuilder.Append("<td align='center'>" + num4 + "</td>");
            float num11 = (float)((double)num4 / (double)num3 * 100.0);
            stringBuilder.Append("<td align='center'>" + num11.ToString("0.00") + " %</td>");
            stringBuilder.Append("<td align='center'>" + GetGrade(Convert.ToDecimal(num11)) + "</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["printExamResult"] = stringBuilder.ToString().Trim();
        btnExport.Enabled = true;
        btnPrint.Enabled = true;
    }

    private string GetGrade(Decimal per)
    {
        return obj.ExecuteScalar("ExamGetGrade", new Hashtable()
    {
      {
         "@Per",
         per
      }
    });
    }

    protected void drpExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpExam.SelectedIndex > 0)
        {
            drpStatus.Enabled = true;
            GenerateReport();
        }
        else
        {
            drpStatus.SelectedIndex = 0;
            drpStatus.Enabled = false;
            ConsolidatedReport();
        }
        drpExam.Focus();
    }
}