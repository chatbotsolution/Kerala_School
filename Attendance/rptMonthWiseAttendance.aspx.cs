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

public partial class Attendance_rptMonthWiseAttendance : System.Web.UI.Page
{
    private clsDAL DAL = new clsDAL();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    private const string AdmnNo = "AdmnNo";
    private const string FullName = "FullName";
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

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = DAL.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        string empty = string.Empty;
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Session", drpSession.SelectedValue);
        hashtable.Add("@Month", drpMonth.SelectedValue);
        hashtable.Add("@ClassId", drpClass.SelectedValue);
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue);
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = DAL.GetDataTable("ATTEND_StudAttendMonthWise", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            GenerateAttendanceRport(dataTable2);
        }
        else
        {
            btnPrint.Enabled = false;
            btnExpExcel.Enabled = false;
            litReport.Text = "<div style='color:Red;' class='tbltxt'><b>No Record Found !!!</b></div>";
        }
    }

    private string GenerateAttendanceRport(DataTable dtAttend)
    {
        string str1 = string.Empty;
        try
        {
            DataTable structureOfFinalRpt = CreateStructureOfFinalRpt();
            if (structureOfFinalRpt.Columns.Count == 0)
                throw new Exception("Could not generate report for current month !");
            foreach (DataRow row1 in (InternalDataCollectionBase)dtAttend.DefaultView.ToTable(true, new string[2]
      {
        "AdmnNo",
        "FullName"
      }).Rows)
            {
                DataRow row2 = structureOfFinalRpt.NewRow();
                row2["AdmnNo"] = row1["AdmnNo"];
                row2["FullName"] = row1["FullName"];
                for (int index = 1; index <= structureOfFinalRpt.Columns.Count - 5; ++index)
                {
                    DataRow[] dataRowArray = dtAttend.Select("AdmnNo=" + row1["AdmnNo"].ToString() + " AND Day=" + index.ToString());
                    row2[index.ToString()] = dataRowArray.Length <= 0 ? "--" : dataRowArray[0]["AttendStatus"].ToString();
                }
                structureOfFinalRpt.Rows.Add(row2);
            }
            structureOfFinalRpt.AcceptChanges();
            if (structureOfFinalRpt.Rows.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<table class='tbltxt' style='border-collapse:collapse;' border='1px' cellpadding='2px' cellspacing='0'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<th align='left'>Admn. No.</th>");
                stringBuilder.Append("<th align='left'>Student Name</th>");
                for (int day = 1; day <= structureOfFinalRpt.Columns.Count - 5; ++day)
                {
                    string str2 = "color:";
                    if (new DateTime(Convert.ToInt32(drpMonth.SelectedValue) <= 3 ? int.Parse(drpSession.SelectedValue.ToString().Substring(0, 4)) + 1 : int.Parse(drpSession.SelectedValue.ToString().Substring(0, 4)), Convert.ToInt32(drpMonth.SelectedValue), day).DayOfWeek.Equals(DayOfWeek.Sunday))
                        str2 += "Red";
                    stringBuilder.Append("<th align='center' style='" + str2 + "'>" + day.ToString().PadLeft(2, '0') + "</th>");
                }
                stringBuilder.Append("<th align='center' style='padding-left:4px;padding-right:4px;'>T</th>");
                stringBuilder.Append("<th align='center' style='padding-left:4px;padding-right:4px;'>P</th>");
                stringBuilder.Append("<th align='center' style='padding-left:4px;padding-right:4px;'>A</th>");
                stringBuilder.Append("</tr>");
                foreach (DataRow row in (InternalDataCollectionBase)structureOfFinalRpt.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left'>" + row["AdmnNo"].ToString() + "</td>");
                    stringBuilder.Append("<td align='left' style='white-space:nowrap;'>" + row["FullName"].ToString() + "</td>");
                    int num1;
                    int num2 = num1 = 0;
                    int num3 = num1;
                    int num4 = num1;
                    for (int day = 1; day <= structureOfFinalRpt.Columns.Count - 5; ++day)
                    {
                        string str2 = "color:";
                        string str3;
                        if (row[day.ToString()].ToString().Trim().Equals("P"))
                        {
                            str3 = str2 + "Green";
                            ++num4;
                            ++num2;
                        }
                        else if (row[day.ToString()].ToString().Trim().Equals("A"))
                        {
                            str3 = str2 + "Red";
                            ++num3;
                            ++num2;
                        }
                        else
                            str3 = !new DateTime(Convert.ToInt32(drpMonth.SelectedValue) <= 3 ? int.Parse(drpSession.SelectedValue.ToString().Substring(0, 4)) + 1 : int.Parse(drpSession.SelectedValue.ToString().Substring(0, 4)), Convert.ToInt32(drpMonth.SelectedValue), day).DayOfWeek.Equals(DayOfWeek.Sunday) ? str2 + "Black" : str2 + "Red";
                        stringBuilder.Append("<td align='center' style='" + str3 + ";'>" + row[day.ToString()].ToString() + "</td>");
                    }
                    stringBuilder.Append("<td align='center'>" + num2.ToString() + "</td>");
                    stringBuilder.Append("<td align='center'>" + num4.ToString() + "</td>");
                    stringBuilder.Append("<td align='center'>" + num3.ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("</table>");
                litReport.Text = stringBuilder.ToString();
                Session["MonthwiseAttendance"] = stringBuilder.ToString().Trim();
                Session["Month"] = drpMonth.SelectedItem.Text;
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
        int int32 = Convert.ToInt32(drpMonth.SelectedValue);
        int year = int32 <= 3 ? int.Parse(drpSession.SelectedValue.ToString().Substring(0, 4)) + 1 : int.Parse(drpSession.SelectedValue.ToString().Substring(0, 4));
        try
        {
            dataTable1.Columns.Add("AdmnNo");
            dataTable1.Columns.Add("FullName");
            for (int index = 1; index <= DateTime.DaysInMonth(year, int32); ++index)
                dataTable1.Columns.Add(index.ToString());
            dataTable1.Columns.Add("T");
            dataTable1.Columns.Add("P");
            dataTable1.Columns.Add("A");
            dataTable1.AcceptChanges();
        }
        catch
        {
            DataTable dataTable2 = new DataTable();
            throw new Exception("Could not generate report for current month !");
        }
        return dataTable1;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptMonthWiseAttendancePrint.aspx');", true);
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
                stringBuilder.Append("Studentwise Attendance Report :- ");
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
}