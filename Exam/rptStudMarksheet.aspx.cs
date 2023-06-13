using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Exam_rptStudMarksheet : System.Web.UI.Page
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
        btnPrint.Visible = false;
        lblReport.Text = string.Empty;
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

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    public string NumberToText(int number)
    {
        if (number == 0)
            return "Zero";
        if (number == int.MinValue)
            return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";
        int[] numArray = new int[4];
        int num1 = 0;
        StringBuilder stringBuilder = new StringBuilder();
        if (number < 0)
        {
            stringBuilder.Append("Minus ");
            number = -number;
        }
        string[] strArray1 = new string[10]
    {
      "",
      "One ",
      "Two ",
      "Three ",
      "Four ",
      "Five ",
      "Six ",
      "Seven ",
      "Eight ",
      "Nine "
    };
        string[] strArray2 = new string[10]
    {
      "Ten ",
      "Eleven ",
      "Twelve ",
      "Thirteen ",
      "Fourteen ",
      "Fifteen ",
      "Sixteen ",
      "Seventeen ",
      "Eighteen ",
      "Nineteen "
    };
        string[] strArray3 = new string[8]
    {
      "Twenty ",
      "Thirty ",
      "Forty ",
      "Fifty ",
      "Sixty ",
      "Seventy ",
      "Eighty ",
      "Ninety "
    };
        string[] strArray4 = new string[3]
    {
      "Thousand ",
      "Lakh ",
      "Crore "
    };
        numArray[0] = number % 1000;
        numArray[1] = number / 1000;
        numArray[2] = number / 100000;
        numArray[1] = numArray[1] - 100 * numArray[2];
        numArray[3] = number / 10000000;
        numArray[2] = numArray[2] - 100 * numArray[3];
        for (int index = 3; index > 0; --index)
        {
            if (numArray[index] != 0)
            {
                num1 = index;
                break;
            }
        }
        for (int index1 = num1; index1 >= 0; --index1)
        {
            if (numArray[index1] != 0)
            {
                int index2 = numArray[index1] % 10;
                int num2 = numArray[index1] / 10;
                int index3 = numArray[index1] / 100;
                int num3 = num2 - 10 * index3;
                if (index3 > 0)
                    stringBuilder.Append(strArray1[index3] + "Hundred ");
                if (index2 > 0 || num3 > 0)
                {
                    if (num3 == 0)
                        stringBuilder.Append(strArray1[index2]);
                    else if (num3 == 1)
                        stringBuilder.Append(strArray2[index2]);
                    else
                        stringBuilder.Append(strArray3[num3 - 2] + strArray1[index2]);
                }
                if (index1 != 0)
                    stringBuilder.Append(strArray4[index1 - 1]);
            }
        }
        return stringBuilder.ToString().TrimEnd();
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpClass.SelectedIndex > 0)
        {
            FillExam();
            drpStudent.Items.Clear();
        }
        drpSession.Focus();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillExam();
        drpClass.Focus();
    }

    protected void drpExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetStudents();
        drpExam.Focus();
    }

    private void GetStudents()
    {
        drpStudent.Items.Clear();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Clear();
        hashtable.Add("@ExamId", drpExam.SelectedValue.ToString());
        DataTable dataTable2 = obj.GetDataTable("ExamRptGetStudents", hashtable);
        if (dataTable2.Rows.Count <= 0)
            return;
        drpStudent.DataSource = dataTable2;
        drpStudent.DataTextField = "StudentStr";
        drpStudent.DataValueField = "AdmnNo";
        drpStudent.DataBind();
        drpStudent.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    private void FillExam()
    {
        btnPrint.Visible = false;
        lblReport.Text = string.Empty;
        drpExam.Items.Clear();
        drpStudent.Items.Clear();
        DataTable dataTable = obj.GetDataTable("ExamGetExDtlsSession", new Hashtable()
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
        if (dataTable.Rows.Count <= 0)
            return;
        drpExam.DataSource = dataTable;
        drpExam.DataTextField = "ExamName";
        drpExam.DataValueField = "ExamClassId";
        drpExam.DataBind();
        drpExam.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    protected void drpStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnPrint.Visible = false;
        lblReport.Text = string.Empty;
        if (drpStudent.SelectedIndex > 0)
            GenerateReport();
        drpStudent.Focus();
    }

    private void GenerateReport()
    {
        btnPrint.Visible = false;
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder = new StringBuilder();
        try
        {
            DataTable dataForReport = GetDataForReport();
            if (dataForReport.Rows.Count > 0)
            {
                stringBuilder.Append("<table border='1' width='100%' cellpadding='2' cellspacing='2' style='border-collapse:collapse;'>");
                stringBuilder.Append("<tr style='font-weight: bold; text-align: left;'>");
                stringBuilder.Append("<td align='center' colspan='5' style='width:80px;Font-Size:15px;'>Mark Sheet</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr style='font-weight: bold; text-align: left;'>");
                stringBuilder.Append("<td align='left' colspan='3'><b>Name : " + drpStudent.SelectedItem.ToString() + "</b></td>");
                stringBuilder.Append("<td align='left'><b>Session : " + drpSession.SelectedValue.ToString() + "</b></td>");
                stringBuilder.Append("<td align='left'><b>Exam : " + drpExam.SelectedItem.ToString() + "</b></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr style='font-weight: bold; text-align: left;color:Black;'>");
                stringBuilder.Append("<td align='left' colspan='5'><b>&nbsp</b></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td width='20%' style='font-weight: bold; text-align: left;' >Subject Name</td>");
                stringBuilder.Append("<td width='20%' style='font-weight: bold; text-align: center;' >Total Mark</td>");
                stringBuilder.Append("<td width='20%' style='font-weight: bold; text-align: center;'>Mark Secured</td>");
                stringBuilder.Append("<td width='20%' style='font-weight: bold; text-align: center;'>Grade</td>");
                stringBuilder.Append("<td width='20%' style='font-weight: bold; text-align: center;'>Result</td>");
                stringBuilder.Append("</tr>");
                double num1 = 0.0;
                double num2 = 0.0;
                foreach (DataRow row in (InternalDataCollectionBase)dataForReport.Rows)
                {
                    Decimal num3 = new Decimal(0);
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left'>" + row["SubjectName"].ToString() + "</td>");
                    stringBuilder.Append("<td align='center'>" + row["FullMarks"].ToString() + "</td>");
                    num1 += double.Parse(row["FullMarks"].ToString());
                    if (row["MarksObtained"].ToString().Trim() != string.Empty)
                    {
                        stringBuilder.Append("<td align='center'>" + row["MarksObtained"].ToString() + "</td>");
                        num2 += double.Parse(row["MarksObtained"].ToString());
                        Decimal per = Convert.ToDecimal(row["MarksObtained"]) / Convert.ToDecimal(row["FullMarks"]) * new Decimal(100);
                        stringBuilder.Append("<td align='center'>" + GetGrade(per) + "</td>");
                    }
                    else
                    {
                        stringBuilder.Append("<td align='center'>N/A</td>");
                        stringBuilder.Append("<td align='center'>N/A</td>");
                    }
                    stringBuilder.Append("<td align='center'>" + row["IndSubResult"].ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("<tr style='font-weight: bold; text-align: left;'>");
                stringBuilder.Append("<td align='right' ><b>Grand Total</b></td>");
                stringBuilder.Append("<td align='center' ><b>" + num1.ToString() + "</b></td>");
                stringBuilder.Append("<td align='center' ><b>" + num2.ToString() + " (" + dataForReport.Rows[0]["OverallPercent"] + " %) </b></td>");
                if (dataForReport.Rows[0]["Result"].ToString().Trim().ToUpper() != "ABSENT")
                    stringBuilder.Append("<td align='center' ><b>" + GetGrade(Convert.ToDecimal(dataForReport.Rows[0]["OverallPercent"])) + "</b></td>");
                else
                    stringBuilder.Append("<td align='center' ><b>N/A</b></td>");
                stringBuilder.Append("<td align='center' ><b>" + dataForReport.Rows[0]["Result"] + "</b></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                lblReport.Text = stringBuilder.ToString();
                btnPrint.Visible = true;
            }
            else
                lblReport.Text = "No Data Found";
            if (stringBuilder.ToString() != "")
                Session["MarkSheet"] = stringBuilder.ToString();
            else
                Session["MarkSheet"] = null;
        }
        catch (Exception ex)
        {
        }
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

    private DataTable GetDataForReport()
    {
        DataTable dataTable = new DataTable();
        return obj.GetDataTable("ExamRptMarkSheet", new Hashtable()
    {
      {
         "@SessionYr",
         drpSession.SelectedValue.ToString()
      },
      {
         "@ExamId",
         drpExam.SelectedValue.ToString()
      },
      {
         "@ClassId",
         drpClass.SelectedValue.ToString()
      },
      {
         "@AdmnNo",
         drpStudent.SelectedValue.ToString()
      }
    });
    }
}