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


public partial class Exam_RptExamReport : System.Web.UI.Page
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
            }
            else
                Response.Redirect("../Login.aspx");
        }
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
        }
        else
        {
            drpSession.Items.Insert(0, new ListItem("- No Exams Defined -", "0"));
            drpClass.Enabled = false;
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

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        drpStud.Items.Clear();
        if (drpSession.SelectedIndex > 0 && drpClass.SelectedIndex > 0)
        {
            DataTable dataTable = obj.GetDataTable("ExamGetStudent", new Hashtable()
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
            drpStud.DataSource = dataTable;
            if (dataTable.Rows.Count > 0)
            {
                drpStud.DataTextField = "FullName";
                drpStud.DataValueField = "admnno";
                drpStud.DataBind();
                drpStud.Items.Insert(0, new ListItem("- Select -", "0"));
            }
        }
        drpClass.Focus();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (drpStud.SelectedIndex > 0)
            GenerateReport();
        btnPrint.Focus();
    }

    private void GenerateReport()
    {
        StringBuilder stringBuilder = new StringBuilder();
        try
        {
            DataTable dataTable1 = new DataTable();
            DataSet dataSet1 = new DataSet();
            string str1 = Server.MapPath("../XMLFiles") + "\\Detail.xml";
            if (File.Exists(str1))
            {
                int num = (int)dataSet1.ReadXml(str1);
                string str2 = Session["SSVMID"].ToString();
                if (dataSet1.Tables.Count > 0)
                {
                    DataTable dataTable2 = new DataTable();
                    DataTable table = dataSet1.Tables[0];
                    foreach (DataRow row in (InternalDataCollectionBase)dataSet1.Tables[0].Rows)
                    {
                        stringBuilder.Append("<table><tr><td width=400px>");
                        stringBuilder.Append("<table width='100%' cellpadding='2' cellspacing='2' align='center'>");
                        stringBuilder.Append("<tr><td rowspan='4' align='center' > <img src='../images/leftlogo.jpg' width='75px' height='80px'/></td>");
                        stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + obj.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/></td></tr>");
                        stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + obj.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + ", " + obj.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</strong></td></tr>");
                        stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + obj.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + obj.Decrypt(row["Pin"].ToString().Trim(), str2) + "</strong> </td></tr>");
                        stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>Phone-" + obj.Decrypt(row["Phone"].ToString().Trim(), str2) + "</strong></td></tr><tr><td colspan='5'></td></tr></table>");
                    }
                }
            }
            //-----------modify by akash on 13/03/2022
            //----new start
            DataTable dtstud = obj.GetDataTableQry("SELECT cs.Section,cs.RollNo,s.OldAdmnNo,cast(s.DOB as date) as DOB,s.StudAdhar,s.FatherName,s.MotherName,s.PresAddr1 FROM dbo.PS_ClasswiseStudent cs join PS_StudMaster s on cs.admnno=s.AdmnNo WHERE cs.admnno=" + drpStud.SelectedValue + " AND cs.ClassId=" + drpClass.SelectedValue);
            stringBuilder.Append("<table width='100%' border='1' cellspacing='2' cellpadding='2' style='border-collapse:collapse;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:center;font-size:18px;' colspan='5'>");
            stringBuilder.Append("<b>Exam Progress Report (2021-22)</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:left;font-size:12px;'colspan='5'>Name of the Student&nbsp;:&nbsp;");
            stringBuilder.Append("<b>" + drpStud.SelectedItem.Text.ToString().Trim() + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:left;font-size:12px;'>Class&nbsp;:&nbsp;");
            stringBuilder.Append("<b>" + drpClass.SelectedItem.Text.ToString().Trim() + "</b></td>");
            stringBuilder.Append("<td style='text-align:left;font-size:12px;'><b>Roll No :</b>");
            if(dtstud.Rows[0]["RollNo"].ToString()=="")
                stringBuilder.Append("<b>&nbsp; ________ </b></td>");
            else
                stringBuilder.Append("<b> "+dtstud.Rows[0]["RollNo"].ToString() +"</b></td>");
            stringBuilder.Append("<td style='text-align:left;font-size:12px;'>Admission No&nbsp;:&nbsp;");
            stringBuilder.Append("<b>" + dtstud.Rows[0]["OldAdmnNo"].ToString() + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");

            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:left;font-size:12px;'>Date Of Barth&nbsp;:&nbsp;<b>" + Convert.ToDateTime(dtstud.Rows[0]["DOB"]).ToString("dd MMM yyyy") + "</b></td>");
            stringBuilder.Append("<td style='text-align:left;font-size:12px;'colspan='5'>Adhar No&nbsp;:&nbsp;<b>" + dtstud.Rows[0]["StudAdhar"] + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:left;font-size:12px;'colspan='5'>Mother's Name&nbsp;:&nbsp;<b>" + dtstud.Rows[0]["MotherName"] + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:left;font-size:12px;'colspan='5'>Father's/Guardian's Name&nbsp;:&nbsp;<b>" + dtstud.Rows[0]["FatherName"] + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:left;font-size:12px;'colspan='5'>Residential Address&nbsp;:&nbsp;<b>" + dtstud.Rows[0]["PresAddr1"] + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            //----new end
            stringBuilder.Append("<table width='100%' border='1' cellspacing='2' cellpadding='2' style='border-collapse:collapse;'>");
            stringBuilder.Append("<tr>");
            
            stringBuilder.Append("<td style='text-align:center;font-size:12px;' rowspan='2'><b>Subject</b></td>");
            DataTable dataTable3 = new DataTable();
            Hashtable hashtable1 = new Hashtable();
            float num1 = 0.0f;
            float num2 = 0.0f;
            float num3 = 0.0f;
            float num4 = 0.0f;
            float num5 = 0.0f;
            float num6 = 0.0f;
            float num7 = 0.0f;
            float num8 = 0.0f;
            float num9 = 0.0f;
            float num10 = 0.0f;
            hashtable1.Add("@ClassId", drpClass.SelectedValue);
            hashtable1.Add("@SessionYr", drpSession.SelectedValue);
            DataTable dataTable4 = obj.GetDataTable("ExamGetExamNameForRpt", hashtable1);
            DataRow[] dataRowArray1 = dataTable4.Select("ExamType='UT'");
            DataRow[] dataRowArray2 = dataTable4.Select("ExamType='PT'");
            DataRow[] dataRowArray3 = dataTable4.Select("ExamType='TE'");
            DataRow[] dataRowArray4 = dataTable4.Select("ExamType='PB'");
            DataRow[] dataRowArray5 = dataTable4.Select("ExamType='HF'");
            DataRow[] dataRowArray6 = dataTable4.Select("ExamType='AN'");
            if (dataRowArray1.Length > 0)
            {
                stringBuilder.Append("<td style='text-align:center;font-size:12px;' colspan='" + (dataRowArray1.Length + 1) + "'><b>Unit Test</b></td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;' colspan='2'><b>Total Unit Test Marks</b></td>");
            }
            if (dataRowArray5.Length > 0)
                stringBuilder.Append("<td style='text-align:center;font-size:12px;' colspan='2'><b>Half Yearly Exam</b></td>");
            if (dataRowArray2.Length > 0)
                stringBuilder.Append("<td style='text-align:center;font-size:12px;' colspan='2'><b>Pre-Test Exam</b></td>");
            if (dataRowArray3.Length > 0)
                stringBuilder.Append("<td style='text-align:center;font-size:12px;' colspan='2'><b>Test Exam</b></td>");
            if (dataRowArray4.Length > 0)
                stringBuilder.Append("<td style='text-align:center;font-size:12px;' colspan='2'><b>Pre-Board Exam</b></td>");
            if (dataRowArray6.Length > 0)
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'colspan='6' ><b>Term II</b></td>");
            //stringBuilder.Append("<td style='text-align:center;font-size:12px;' colspan='2'><b>Aggr. Marks</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            if (dataRowArray1.Length > 0)
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>F.M.</td>");
            foreach (DataRow dataRow in dataRowArray1)
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow["ExamName"].ToString().Trim() + "</td>");
            if (dataRowArray1.Length > 0)
            {
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>F.M.</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>M.Obt.</td>");
            }
            for (int index = 1; index <= dataTable4.Rows.Count - dataRowArray1.Length; ++index)
            {
                //stringBuilder.Append("<td style='text-align:center;font-size:12px;'>F.M.</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>PT-1(10)</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>SE(05)</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>M.A/Viva(05)</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>Final Exam(80)</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>Marks Secured(100)</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>Grade</td>");
            }
            //stringBuilder.Append("<td style='text-align:center;font-size:12px;'>F.M.</td>");
            //stringBuilder.Append("<td style='text-align:center;font-size:12px;'>M.Obt.</td>");
            stringBuilder.Append("</tr>");
            DataTable dataTable5 = new DataTable();
            DataTable dataTableQry = obj.GetDataTableQry("SELECT SubjectId,SubjectName FROM dbo.PS_SubjectMaster WHERE ClassId=" + drpClass.SelectedValue);
            int num11 = 0;
            foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
            {
                ++num11;
                Hashtable hashtable2 = new Hashtable();
                hashtable2.Add("@SessionYr", drpSession.SelectedValue);
                hashtable2.Add("@SubjectId", row["SubjectId"].ToString().Trim());
                hashtable2.Add("@AdmnNo", drpStud.SelectedValue);
                DataSet dataSet2 = new DataSet();
                DataSet dataSet3 = obj.GetDataSet("ExamGetProgressRpt", hashtable2);
                DataTable dataTable2 = new DataTable();
                DataTable table = dataSet3.Tables[0];
                stringBuilder.Append("<tr>");
                //stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + num11 + "</td>");
                stringBuilder.Append("<td style='text-align:left;font-size:12px;'>" + row["SubjectName"].ToString().Trim() + "</td>");
                int num12 = 1;
                float num13 = 0.0f;
                float num14 = 0.0f;
                float num15 = 0.0f;
                float num16 = 0.0f;
                float num17 = 0.0f;
                foreach (DataRow dataRow1 in dataRowArray1)
                {
                    DataRow[] dataRowArray7 = table.Select("ExamType='UT'");
                    if (dataRowArray7.Length > 0)
                    {
                        foreach (DataRow dataRow2 in dataRowArray7)
                        {
                            if (dataRow2["ExamId"].ToString().Trim() == dataRow1["ExamClassId"].ToString().Trim())
                            {
                                if (num12 == 1)
                                {
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["FullMarks"].ToString().Trim() + "</td>");
                                    num14 += float.Parse(dataRow2["FullMarks"].ToString());
                                    num1 += num14;
                                }
                                if (dataRow2["IsPresent"].ToString().Trim().ToUpper() == "TRUE")
                                {
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MarksObtained"].ToString().Trim() + "</td>");
                                    num15 += float.Parse(dataRow2["MarksObtained"].ToString());
                                    dataRow1["ObtMarks"] = (float)((double)float.Parse(dataRow1["ObtMarks"].ToString()) + (double)float.Parse(dataRow2["MarksObtained"].ToString()));
                                    dataRow1.AcceptChanges();
                                }
                                else
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;color:Red;'>A</td>");
                                num13 += float.Parse(dataRow2["FullMarks"].ToString());
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (num12 == 1)
                            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                    }
                    ++num12;
                }
                if (dataRowArray1.Length > 0)
                {
                    if ((double)num13 > 0.0)
                    {
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + num13 + "</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + num15 + "</td>");
                        num9 += num13;
                        num10 += num15;
                    }
                    else
                    {
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                    }
                }
                float num18 = num16 + num13;
                float num19 = num17 + num15;
                int num20 = 1;
                float num21 = 0.0f;
                float num22 = 0.0f;
                float num23 = 0.0f;
                foreach (DataRow dataRow1 in dataRowArray5)
                {
                    DataRow[] dataRowArray7 = table.Select("ExamType='HF'");
                    if (dataRowArray7.Length > 0)
                    {
                        foreach (DataRow dataRow2 in dataRowArray7)
                        {
                            if (dataRow2["ExamId"].ToString().Trim() == dataRow1["ExamClassId"].ToString().Trim())
                            {
                                if (num20 == 1)
                                {
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["FullMarks"].ToString().Trim() + "</td>");
                                    num22 += float.Parse(dataRow2["FullMarks"].ToString());
                                    num5 += num22;
                                }
                                if (dataRow2["IsPresent"].ToString().Trim().ToUpper() == "TRUE")
                                {
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MarksObtained"].ToString().Trim() + "</td>");
                                    num23 += float.Parse(dataRow2["MarksObtained"].ToString());
                                    dataRow1["ObtMarks"] = (float)((double)float.Parse(dataRow1["ObtMarks"].ToString()) + (double)float.Parse(dataRow2["MarksObtained"].ToString()));
                                    dataRow1.AcceptChanges();
                                }
                                else
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;color:Red;'>A</td>");
                                num21 += float.Parse(dataRow2["FullMarks"].ToString());
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (num20 == 1)
                            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                    }
                    ++num20;
                }
                float num24 = num18 + num22;
                float num25 = num19 + num23;
                int num26 = 1;
                float num27 = 0.0f;
                float num28 = 0.0f;
                float num29 = 0.0f;
                foreach (DataRow dataRow1 in dataRowArray2)
                {
                    DataRow[] dataRowArray7 = table.Select("ExamType='PT'");
                    if (dataRowArray7.Length > 0)
                    {
                        foreach (DataRow dataRow2 in dataRowArray7)
                        {
                            if (dataRow2["ExamId"].ToString().Trim() == dataRow1["ExamClassId"].ToString().Trim())
                            {
                                if (num26 == 1)
                                {
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["FullMarks"].ToString().Trim() + "</td>");
                                    num28 += float.Parse(dataRow2["FullMarks"].ToString());
                                    num2 += num28;
                                }
                                if (dataRow2["IsPresent"].ToString().Trim().ToUpper() == "TRUE")
                                {
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MarksObtained"].ToString().Trim() + "</td>");
                                    num29 += float.Parse(dataRow2["MarksObtained"].ToString());
                                    dataRow1["ObtMarks"] = (float)((double)float.Parse(dataRow1["ObtMarks"].ToString()) + (double)float.Parse(dataRow2["MarksObtained"].ToString()));
                                    dataRow1.AcceptChanges();
                                }
                                else
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;color:Red;'>A</td>");
                                num27 += float.Parse(dataRow2["FullMarks"].ToString());
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (num26 == 1)
                            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                    }
                    ++num26;
                }
                float num30 = num24 + num28;
                float num31 = num25 + num29;
                int num32 = 1;
                float num33 = 0.0f;
                float num34 = 0.0f;
                float num35 = 0.0f;
                foreach (DataRow dataRow1 in dataRowArray3)
                {
                    DataRow[] dataRowArray7 = table.Select("ExamType='TE'");
                    if (dataRowArray7.Length > 0)
                    {
                        foreach (DataRow dataRow2 in dataRowArray7)
                        {
                            if (dataRow2["ExamId"].ToString().Trim() == dataRow1["ExamClassId"].ToString().Trim())
                            {
                                if (num32 == 1)
                                {
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["FullMarks"].ToString().Trim() + "</td>");
                                    num34 += float.Parse(dataRow2["FullMarks"].ToString());
                                    num3 += num34;
                                }
                                if (dataRow2["IsPresent"].ToString().Trim().ToUpper() == "TRUE")
                                {
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MarksObtained"].ToString().Trim() + "</td>");
                                    num35 += float.Parse(dataRow2["MarksObtained"].ToString());
                                    dataRow1["ObtMarks"] = (float)((double)float.Parse(dataRow1["ObtMarks"].ToString()) + (double)float.Parse(dataRow2["MarksObtained"].ToString()));
                                    dataRow1.AcceptChanges();
                                }
                                else
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;color:Red;'>A</td>");
                                num33 += float.Parse(dataRow2["FullMarks"].ToString());
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (num32 == 1)
                            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                    }
                    ++num32;
                }
                float num36 = num30 + num34;
                float num37 = num31 + num35;
                int num38 = 1;
                float num39 = 0.0f;
                float num40 = 0.0f;
                float num41 = 0.0f;
                foreach (DataRow dataRow1 in dataRowArray4)
                {
                    DataRow[] dataRowArray7 = table.Select("ExamType='PB'");
                    if (dataRowArray7.Length > 0)
                    {
                        foreach (DataRow dataRow2 in dataRowArray7)
                        {
                            if (dataRow2["ExamId"].ToString().Trim() == dataRow1["ExamClassId"].ToString().Trim())
                            {
                                if (num38 == 1)
                                {
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["FullMarks"].ToString().Trim() + "</td>");
                                    num40 += float.Parse(dataRow2["FullMarks"].ToString());
                                    num4 += num40;
                                }
                                if (dataRow2["IsPresent"].ToString().Trim().ToUpper() == "TRUE")
                                {
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MarksObtained"].ToString().Trim() + "</td>");
                                    num41 += float.Parse(dataRow2["MarksObtained"].ToString());
                                    dataRow1["ObtMarks"] = (float)((double)float.Parse(dataRow1["ObtMarks"].ToString()) + (double)float.Parse(dataRow2["MarksObtained"].ToString()));
                                    dataRow1.AcceptChanges();
                                }
                                else
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;color:Red;'>A</td>");
                                num39 += float.Parse(dataRow2["FullMarks"].ToString());
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (num38 == 1)
                            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                    }
                    ++num38;
                }
                float num42 = num36 + num40;
                float num43 = num37 + num41;
                int num44 = 1;
                float num45 = 0.0f;
                float num46 = 0.0f;
                float num47 = 0.0f;
                foreach (DataRow dataRow1 in dataRowArray6)
                {
                    DataRow[] dataRowArray7 = table.Select("ExamType='AN'");
                    string grade = "";
                    if (dataRowArray7.Length > 0)
                    {
                        foreach (DataRow dataRow2 in dataRowArray7)
                        {
                            if (dataRow2["ExamId"].ToString().Trim() == dataRow1["ExamClassId"].ToString().Trim())
                            {
                                if (num44 == 1)
                                {
                                    //stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["FullMarks"].ToString().Trim() + "</td>");
                                    num46 += float.Parse(dataRow2["FullMarks"].ToString());
                                    num6 += num46;
                                }
                                if (dataRow2["IsPresent"].ToString().Trim().ToUpper() == "TRUE")
                                {
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["ProjMarks"].ToString().Trim() + "</td>");
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["PractMarks"].ToString().Trim() + "</td>");
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MAVIVA"].ToString().Trim() + "</td>");
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["TheoryMarks"].ToString().Trim() + "</td>");
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + dataRow2["MarksObtained"].ToString().Trim() + "</td>");
                                    if (Convert.ToDouble(dataRow2["MarksObtained"].ToString().Trim()) >= 90.1)
                                        grade = "A1";
                                    else if (Convert.ToDouble(dataRow2["MarksObtained"].ToString().Trim()) >= 80.1)
                                        grade = "A";
                                    else if (Convert.ToDouble(dataRow2["MarksObtained"].ToString().Trim()) >= 70.1)
                                        grade = "B1";
                                    else if (Convert.ToDouble(dataRow2["MarksObtained"].ToString().Trim()) >= 60.1)
                                        grade = "B2";
                                    else if (Convert.ToDouble(dataRow2["MarksObtained"].ToString().Trim()) >= 50.1)
                                        grade = "C1";
                                    else if (Convert.ToDouble(dataRow2["MarksObtained"].ToString().Trim()) >= 40.1)
                                        grade = "C2";
                                    else if (Convert.ToDouble(dataRow2["MarksObtained"].ToString().Trim()) >= 32.1)
                                        grade = "D";
                                    else
                                        grade = "E";
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'> "+grade+" </td>");
                                    num47 += float.Parse(dataRow2["MarksObtained"].ToString());
                                    dataRow1["ObtMarks"] = (float)((double)float.Parse(dataRow1["ObtMarks"].ToString()) + (double)float.Parse(dataRow2["MarksObtained"].ToString()));
                                    dataRow1.AcceptChanges();
                                }
                                else
                                    stringBuilder.Append("<td style='text-align:center;font-size:12px;color:Red;'>AB</td>");
                                num45 += float.Parse(dataRow2["FullMarks"].ToString());
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (num44 == 1)
                            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                        stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                    }
                    ++num44;
                }
                float num48 = num42 + num46;
                float num49 = num43 + num47;
                //if ((double)num48 > 0.0)
                //{
                    //stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + num48 + "</td>");
                    //stringBuilder.Append("<td style='text-align:center;font-size:12px;'>" + num49 + "</td>");
                //}
                //else
                //{
                //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
                //}
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("<tr>");
            DataTable dtgrade= obj.GetDataTableQry("SELECT SubjectId,SubjectName FROM dbo.GradeSubjectMaster WHERE ClassId=" + drpClass.SelectedValue);
            foreach (DataRow row in (InternalDataCollectionBase)dtgrade.Rows)
            {
                string grade = obj.ExecuteScalarQry("select Grade from GradeMarks where SessionYr='" + drpSession.SelectedValue+"' and GradeSubjectId="+row["SubjectId"] +" and AdmnNo="+drpStud.SelectedValue);
                stringBuilder.Append("<td style='text-align:left;font-size:12px;'>"+row["SubjectName"] +"</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>NA</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>NA</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>NA</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>NA</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>NA</td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'>"+grade+"</td>");
            }
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:left;font-size:12px;' colspan='5'><b>Total Marks</b></td>");
            if (dataRowArray1.Length > 0)
            {
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + num1 + "</b></td>");
                foreach (DataRow dataRow in dataRowArray1)
                {
                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + dataRow["ObtMarks"] + "</b></td>");
                    num8 += float.Parse(dataRow["ObtMarks"].ToString());
                }
                num7 += num9;
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + num9 + "</b></td>");
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + num10 + "</b></td>");
            }
            if (dataRowArray5.Length > 0)
            {
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + num5 + "</b></td>");
                foreach (DataRow dataRow in dataRowArray5)
                {
                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + dataRow["ObtMarks"] + "</b></td>");
                    num8 += float.Parse(dataRow["ObtMarks"].ToString());
                }
                num7 += num5;
            }
            if (dataRowArray2.Length > 0)
            {
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + num2 + "</b></td>");
                foreach (DataRow dataRow in dataRowArray2)
                {
                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + dataRow["ObtMarks"] + "</b></td>");
                    num8 += float.Parse(dataRow["ObtMarks"].ToString());
                }
                num7 += num2;
            }
            if (dataRowArray3.Length > 0)
            {
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + num3 + "</b></td>");
                foreach (DataRow dataRow in dataRowArray3)
                {
                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + dataRow["ObtMarks"] + "</b></td>");
                    num8 += float.Parse(dataRow["ObtMarks"].ToString());
                }
                num7 += num3;
            }
            if (dataRowArray4.Length > 0)
            {
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + num4 + "</b></td>");
                foreach (DataRow dataRow in dataRowArray4)
                {
                    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + dataRow["ObtMarks"] + "</b></td>");
                    num8 += float.Parse(dataRow["ObtMarks"].ToString());
                }
                num7 += num4;
            }
            if (dataRowArray6.Length > 0)
            {
                stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + num6 + "</b></td>");
                foreach (DataRow dataRow in dataRowArray6)
                {
                    //stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + dataRow["ObtMarks"] + "</b></td>");
                    num8 += float.Parse(dataRow["ObtMarks"].ToString());
                }
                num7 += num6;
            }
            //if ((double)num7 > 0.0)
            //{
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + num7 + "</b></td>");
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + num8 + "</b></td>");
            //}
            //else
            //{
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
            //}
            stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>&nbsp;</b></td>");
            stringBuilder.Append("</tr>");
            //stringBuilder.Append("<tr>");
            //stringBuilder.Append("<td style='text-align:left;font-size:12px;' colspan='2'><b>Percentage (%) & Grade</b></td>");
            //if (dataRowArray1.Length > 0)
            //{
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>--</b></td>");
            //    foreach (DataRow dataRow in dataRowArray1)
            //    {
            //        float per = (float)((double)float.Parse(dataRow["ObtMarks"].ToString()) / (double)num1 * 100.0);
            //        stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + per.ToString("0.00") + " (" + GetGrade(per).Trim() + ")</b></td>");
            //    }
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>--</b></td>");
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>--</b></td>");
            //}
            //if (dataRowArray5.Length > 0)
            //{
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>--</b></td>");
            //    foreach (DataRow dataRow in dataRowArray5)
            //    {
            //        float per = (float)((double)float.Parse(dataRow["ObtMarks"].ToString()) / (double)num5 * 100.0);
            //        stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + per.ToString("0.00") + " (" + GetGrade(per).Trim() + ")</b></td>");
            //    }
            //}
            //if (dataRowArray2.Length > 0)
            //{
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>--</b></td>");
            //    foreach (DataRow dataRow in dataRowArray2)
            //    {
            //        float per = (float)((double)float.Parse(dataRow["ObtMarks"].ToString()) / (double)num2 * 100.0);
            //        stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + per.ToString("0.00") + " (" + GetGrade(per).Trim() + ")</b></td>");
            //    }
            //}
            //if (dataRowArray3.Length > 0)
            //{
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>--</b></td>");
            //    foreach (DataRow dataRow in dataRowArray3)
            //    {
            //        float per = (float)((double)float.Parse(dataRow["ObtMarks"].ToString()) / (double)num3 * 100.0);
            //        stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + per.ToString("0.00") + " (" + GetGrade(per).Trim() + ")</b></td>");
            //    }
            //}
            //if (dataRowArray4.Length > 0)
            //{
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>--</b></td>");
            //    foreach (DataRow dataRow in dataRowArray4)
            //    {
            //        float per = (float)((double)float.Parse(dataRow["ObtMarks"].ToString()) / (double)num4 * 100.0);
            //        stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + per.ToString("0.00") + " (" + GetGrade(per).Trim() + ")</b></td>");
            //    }
            //}
            //if (dataRowArray6.Length > 0)
            //{
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>--</b></td>");
            //    foreach (DataRow dataRow in dataRowArray6)
            //    {
            //        float per = (float)((double)float.Parse(dataRow["ObtMarks"].ToString()) / (double)num6 * 100.0);
            //        //stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + per.ToString("0.00") + " (" + GetGrade(per).Trim() + ")</b></td>");
            //    }
            //}
            //if ((double)num7 > 0.0)
            //{
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>--</b></td>");
            //    float per = (float)((double)num8 / (double)num7 * 100.0);
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'><b>" + per.ToString("0.00") + " (" + GetGrade(per).Trim() + ")</b></td>");
            //}
            //else
            //{
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
            //    stringBuilder.Append("<td style='text-align:center;font-size:12px;'>--</td>");
            //}
            //stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table width='100%' border='1' cellspacing='0' cellpadding='0' style='border-collapse:collapse;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:left;font-size:12px;'colspan='3'>");
            stringBuilder.Append("<b>Remarks :-</b><br/><br/><br/><br/></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            
            //stringBuilder.Append("<td style='text-align:center;font-size:12px;'>");
            //stringBuilder.Append("<br/><br/><br/><br/><b>Exam Controller</b></td>");
            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>");
            stringBuilder.Append("<br/><br/><br/><br/><b>Signature of Parent</b></td>");
            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>");
            stringBuilder.Append("<br/><br/><br/><br/><b>Signature of Principal</b></td>");
            stringBuilder.Append("<td style='text-align:center;font-size:12px;'>");
            stringBuilder.Append("<br/><br/><br/><br/><b>Signature of Class Teacher</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["printExamProgressReport"] = stringBuilder.ToString().Trim();
            btnPrint.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }

    private string GetGrade(float per)
    {
        return obj.ExecuteScalar("ExamGetGrade", new Hashtable()
    {
      {
         "@Per",
         per
      }
    });
    }

    protected void drpStud_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        if (drpStud.SelectedIndex > 0)
        {
            GenerateReport();
            btnPrint.Enabled = true;
        }
        else
            btnPrint.Enabled = false;
        drpStud.Focus();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("RptExamReportPrint.aspx");
        btnPrint.Focus();
    }
}