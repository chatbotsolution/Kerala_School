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

public partial class Attendance_rptStudAttendanceClasswise : System.Web.UI.Page
{
    private clsDAL DAL = new clsDAL();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    private const string MONTH = "MONTH";
    private const string MonthName = "MonthName";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        bindDropDown(drpClass, "select ClassId,ClassName from PS_ClassMaster", "ClassName", "ClassId");
        bindDropDown(drpSession, "select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc", "SessionYr", "SessionYr");
        drpSession.Items.RemoveAt(0);
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
        objStatic.FillSection(drpSection);
        drpSection.Items.RemoveAt(0);
        drpSection.Items.Insert(0, new ListItem("- All -", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        litReport.Text = string.Empty;
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillStudent();
        txtadmnno.Text = string.Empty;
    }

    private void FillStudent()
    {
        clsDAL clsDal = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select fullname,s.admnno from PS_ClasswiseStudent cs join ps_studmaster s on ");
        stringBuilder.Append(" s.admnno=cs.admnno where 1=1");
        if (drpClass.SelectedIndex != 0)
            stringBuilder.Append(" and cs.classid=" + drpClass.SelectedValue);
        if (drpSection.SelectedIndex != 0)
            stringBuilder.Append(" and cs.Section='" + drpSection.SelectedValue + "'");
        stringBuilder.Append(" and cs.SessionYear='" + drpSession.SelectedValue.ToString().Trim() + "' and  Detained_Promoted='' order by fullname ");
        DataTable dataTableQry = clsDal.GetDataTableQry(stringBuilder.ToString().Trim());
        try
        {
            drpStudent.DataSource = dataTableQry;
            drpStudent.DataTextField = "fullname";
            drpStudent.DataValueField = "admnno";
            drpStudent.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        if (dataTableQry.Rows.Count > 0)
            drpStudent.Items.Insert(0, new ListItem("-Select-", "0"));
        else
            drpStudent.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void drpStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        litReport.Text = string.Empty;
        txtadmnno.Text = drpStudent.SelectedValue;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTableQry = new clsDAL().GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent where AdmnNo=" + txtadmnno.Text.Trim() + " and Detained_Promoted=''");
            if (dataTableQry.Rows.Count > 0)
            {
                drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
                drpClass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
                FillStudent();
                drpStudent.SelectedValue = txtadmnno.Text.Trim();
                GetAttendanceDetails();
            }
            else
                ScriptManager.RegisterClientScriptBlock((Control)txtadmnno, txtadmnno.GetType(), "ShowMessage", "alert('Admission number does not exists')", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void GetAttendanceDetails()
    {
        string empty = string.Empty;
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Session", drpSession.SelectedValue);
        hashtable.Add("@AdmnNo", drpStudent.SelectedValue);
        hashtable.Add("@ClassId", drpClass.SelectedValue);
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = DAL.GetDataTable("ATTEND_StudAttendSessionWise", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            GenerateAttendanceRport(dataTable2);
        }
        else
        {
            btnPrint.Enabled = false;
            btnExpExcel.Enabled = false;
            litReport.Text = "<span style='color:red;'>Could not generate report for current session year !</span>";
        }
    }

    private string GenerateAttendanceRport(DataTable dtAttend)
    {
        string str1 = string.Empty;
        try
        {
            DataTable structureOfFinalRpt = CreateStructureOfFinalRpt();
            if (structureOfFinalRpt.Columns.Count == 0)
                throw new Exception("Could not generate report for current session year !");
            foreach (DataRow row1 in (InternalDataCollectionBase)dtAttend.DefaultView.ToTable(true, new string[2]
      {
        "MONTH",
        "MonthName"
      }).Rows)
            {
                DataRow row2 = structureOfFinalRpt.NewRow();
                row2["MonthName"] = row1["MonthName"];
                row2["MONTH"] = row1["MONTH"].ToString();
                for (int index = 1; index <= structureOfFinalRpt.Columns.Count - 5; ++index)
                {
                    DataRow[] dataRowArray = dtAttend.Select("Day=" + index.ToString() + " AND MONTH=" + row1["MONTH"].ToString());
                    row2[index.ToString()] = dataRowArray.Length <= 0 ? "--" : dataRowArray[0]["AttendStatus"].ToString();
                }
                structureOfFinalRpt.Rows.Add(row2);
            }
            structureOfFinalRpt.AcceptChanges();
            if (structureOfFinalRpt.Rows.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<table class='tbltxt' style='border-collapse:collapse;' border='1px' cellpadding='2px' cellspacing='0' width='100%'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<th align='left'>&nbsp;</th>");
                for (int index = 1; index <= structureOfFinalRpt.Columns.Count - 5; ++index)
                {
                    string str2 = "black";
                    stringBuilder.Append("<th align='center' style='" + str2 + "'>" + index.ToString().PadLeft(2, '0') + "</th>");
                }
                stringBuilder.Append("<th align='center' style='padding-left:4px;padding-right:4px;'>T</th>");
                stringBuilder.Append("<th align='center' style='padding-left:4px;padding-right:4px;'>P</th>");
                stringBuilder.Append("<th align='center' style='padding-left:4px;padding-right:4px;'>A</th>");
                stringBuilder.Append("</tr>");
                foreach (DataRow row in (InternalDataCollectionBase)structureOfFinalRpt.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left'><b>" + row["MonthName"].ToString() + "</b></td>");
                    int num1;
                    int num2 = num1 = 0;
                    int num3 = num1;
                    int num4 = num1;
                    for (int day = 1; day <= structureOfFinalRpt.Columns.Count - 5; ++day)
                    {
                        string str2 = "color:";
                        try
                        {
                            if (row[day.ToString()].ToString().Trim().Equals("P"))
                            {
                                str2 += "Green";
                                ++num4;
                                ++num2;
                            }
                            else if (row[day.ToString()].ToString().Trim().Equals("A"))
                            {
                                str2 += "Red";
                                ++num3;
                                ++num2;
                            }
                            else
                                str2 = !new DateTime(Convert.ToInt32(row["MONTH"].ToString()) <= 3 ? int.Parse(drpSession.SelectedValue.ToString().Substring(0, 4)) + 1 : int.Parse(drpSession.SelectedValue.ToString().Substring(0, 4)), Convert.ToInt32(row["MONTH"].ToString()), day).DayOfWeek.Equals(DayOfWeek.Sunday) ? str2 + "Black" : str2 + "Red";
                            stringBuilder.Append("<td align='center' style='" + str2 + ";'>" + row[day.ToString()].ToString() + "</td>");
                        }
                        catch
                        {
                            string str3 = str2 + "#003399";
                            stringBuilder.Append("<td align='center' style='" + str3 + ";'><b>*</b></td>");
                        }
                    }
                    stringBuilder.Append("<td align='center'>" + num2.ToString() + "</td>");
                    stringBuilder.Append("<td align='center'>" + num4.ToString() + "</td>");
                    stringBuilder.Append("<td align='center'>" + num3.ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("</table>");
                litReport.Text = stringBuilder.ToString();
                Session["StudentwiseAttendance"] = stringBuilder.ToString().Trim();
                Session["Student"] = new clsDAL().ExecuteScalarQry("select FullName from dbo.PS_StudMaster where AdmnNo=" + drpStudent.SelectedValue.Trim());
                btnPrint.Enabled = true;
                btnExpExcel.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            str1 = ex.Message;
        }
        return str1;
    }

    private DataTable CreateStructureOfFinalRpt()
    {
        DataTable dataTable1 = new DataTable();
        try
        {
            dataTable1.Columns.Add("MonthName");
            dataTable1.Columns.Add("MONTH");
            for (int index = 1; index <= 31; ++index)
                dataTable1.Columns.Add(index.ToString());
            dataTable1.Columns.Add("T");
            dataTable1.Columns.Add("P");
            dataTable1.Columns.Add("A");
            dataTable1.AcceptChanges();
        }
        catch
        {
            DataTable dataTable2 = new DataTable();
            throw new Exception("Could not generate report for selected year !");
        }
        return dataTable1;
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = DAL.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptStudAttendanceClasswisePrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (litReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td align='left' colspan='14'><b>");
                stringBuilder.Append("Attendance Report of " + Session["Student"] + ":- ");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(litReport.Text.ToString().Trim());
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
            string str = Server.MapPath("../Up_Files/Exported_Files/StudAttendanceClasswise" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillStudent();
    }
}