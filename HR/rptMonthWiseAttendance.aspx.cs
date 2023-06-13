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

public partial class HR_rptMonthWiseAttendance : System.Web.UI.Page
{
    private clsDAL DAL = new clsDAL();
    private const string EmpId = "EmpId";
    private const string EmpName = "EmpName";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        bindDropDown(drpYear, "SELECT DISTINCT(YEAR(ISNULL(AttendDate,GETDATE()))) AS CalYear FROM dbo.HR_EmpAttendance order by CalYear desc", "CalYear", "CalYear");
        bindDropDownShift(drpShift, "SELECT ShiftId,ShiftCode+' ('+StartTime+' - '+EndTime+')' as ShiftCode FROM dbo.HR_EmpShift", "ShiftCode", "ShiftId");
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = DAL.GetDataTableQry(query);
        if (dataTableQry.Rows.Count == 0)
        {
            DataRow row = dataTableQry.NewRow();
            row["CalYear"] = DateTime.Now.Year;
            dataTableQry.Rows.InsertAt(row, 0);
            dataTableQry.AcceptChanges();
        }
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
    }

    private void bindDropDownShift(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = DAL.GetDataTableQry(query);
        if (dataTableQry.Rows.Count < 0)
            return;
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- All -", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        string empty = string.Empty;
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Month", drpMonth.SelectedValue.Trim());
        hashtable.Add("@MnthNm", drpMonth.SelectedItem.ToString().Trim());
        hashtable.Add("@Year", drpYear.SelectedValue.Trim());
        if (drpShift.SelectedIndex > 0)
            hashtable.Add("@ShiftId", drpShift.SelectedValue.ToString().Trim());
        DataSet dataSet1 = new DataSet();
        DataSet dataSet2 = DAL.GetDataSet("HR_ATTEND_StudAttendMonthWise", hashtable);
        if (dataSet2.Tables[0].Rows.Count > 0 || dataSet2.Tables[1].Rows.Count > 0)
        {
            GenerateAttendanceRport(dataSet2);
        }
        else
        {
            btnPrint.Enabled = false;
            btnExpExcel.Enabled = false;
            litReport.Text = "<div style='color:Red;'><b>No Record Found !!!</b></div>";
        }
    }

    private string GenerateAttendanceRport(DataSet ds)
    {
        string str1 = string.Empty;
        try
        {
            DataTable dataTable1 = new DataTable();
            DataTable table1 = ds.Tables[0];
            DataTable structureOfFinalRpt = CreateStructureOfFinalRpt();
            if (structureOfFinalRpt.Columns.Count == 0)
                throw new Exception("Can not Generate Report for " + drpMonth.SelectedItem.Text + "-" + drpYear.SelectedValue);
            foreach (DataRow row1 in (InternalDataCollectionBase)table1.DefaultView.ToTable(true, "EmpId", "EmpName", "TotWrkng").Rows)
            {
                DataRow row2 = structureOfFinalRpt.NewRow();
                row2["EmpId"] = row1["EmpId"];
                row2["EmpName"] = row1["EmpName"];
                row2["T"] = row1["TotWrkng"];
                for (int index = 1; index <= structureOfFinalRpt.Columns.Count - 5; ++index)
                {
                    DataRow[] dataRowArray = table1.Select("EmpId=" + row1["EmpId"].ToString() + " AND Day=" + index.ToString());
                    row2[index.ToString()] = dataRowArray.Length <= 0 ? "--" : dataRowArray[0]["AttendStatus"].ToString();
                }
                structureOfFinalRpt.Rows.Add(row2);
            }
            structureOfFinalRpt.AcceptChanges();
            if (structureOfFinalRpt.Rows.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<table width='100%' style='border-collapse:collapse;' border='1px' cellpadding='2px' cellspacing='0'>");
                stringBuilder.Append("<tr><td colspan='20' align='Left'><b>Attendance Report For the month of : " + drpMonth.SelectedItem.ToString() + "-" + drpYear.SelectedItem.ToString() + "</b></td><td colspan='19' align='right'>No Of Records: " + structureOfFinalRpt.DefaultView.ToTable(true, "EmpId").Rows.Count + "</td></tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left'><b>Emp Id.</b></td>");
                stringBuilder.Append("<td align='left'><b>Employee Name</b></td>");
                for (int day = 1; day <= structureOfFinalRpt.Columns.Count - 5; ++day)
                {
                    string str2 = "color:";
                    if (new DateTime(int.Parse(drpYear.SelectedValue.ToString().Substring(0, 4)), Convert.ToInt32(drpMonth.SelectedValue), day).DayOfWeek.Equals(DayOfWeek.Sunday))
                        str2 += "Red";
                    stringBuilder.Append("<td align='center' style='" + str2 + "'><b>" + day.ToString().PadLeft(2, '0') + " " + Convert.ToDateTime(day.ToString() + " " + drpMonth.SelectedItem.ToString() + " " + drpYear.SelectedValue.ToString()).ToString("ddd") + "</b></td>");
                }
                stringBuilder.Append("<td align='center' style='padding-left:4px;padding-right:4px;'>T</th>");
                stringBuilder.Append("<td align='center' style='padding-left:4px;padding-right:4px;'>P</th>");
                stringBuilder.Append("<td align='center' style='padding-left:4px;padding-right:4px;'>A</th>");
                stringBuilder.Append("<td align='center' style='padding-left:4px;padding-right:4px;'>L</th>");
                stringBuilder.Append("</tr>");
                foreach (DataRow row in (InternalDataCollectionBase)structureOfFinalRpt.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left'>" + row["EmpId"].ToString() + "</td>");
                    stringBuilder.Append("<td align='left' style='white-space:nowrap;'>" + row["EmpName"].ToString() + "</td>");
                    int num1;
                    int num2 = num1 = 0;
                    int num3 = num1;
                    int num4 = num1;
                    for (int day = 1; day <= structureOfFinalRpt.Columns.Count - 5; ++day)
                    {
                        string str2 = "color:";
                        string str3;
                        if (row[day.ToString()].ToString().Trim().Equals("P") || row[day.ToString()].ToString().Trim().ToUpper().Equals("OFF") || (row[day.ToString()].ToString().Trim().ToUpper().Equals("DL") || row[day.ToString()].ToString().Trim().ToUpper().Equals("TOUR")))
                        {
                            str3 = str2 + "Green";
                            ++num4;
                        }
                        else if (row[day.ToString()].ToString().Trim().Equals("A"))
                        {
                            str3 = str2 + "Red";
                            ++num3;
                        }
                        else if (row[day.ToString()].ToString().Trim().Equals("L"))
                        {
                            str3 = str2 + "Black";
                            ++num2;
                        }
                        else
                            str3 = !new DateTime(int.Parse(drpYear.SelectedValue.ToString().Substring(0, 4)), Convert.ToInt32(drpMonth.SelectedValue), day).DayOfWeek.Equals(DayOfWeek.Sunday) ? str2 + "Black" : str2 + "Red";
                        stringBuilder.Append("<td align='center' style='" + str3 + ";'>" + row[day.ToString()].ToString() + "</td>");
                    }
                    stringBuilder.Append("<td align='center'>" + row["T"].ToString() + "</td>");
                    stringBuilder.Append("<td align='center'>" + num4.ToString() + "</td>");
                    stringBuilder.Append("<td align='center'>" + num3.ToString() + "</td>");
                    stringBuilder.Append("<td align='center'>" + num2.ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("</table>");
                DataTable dataTable2 = new DataTable();
                DataTable table2 = ds.Tables[1];
                if (table2.Rows.Count > 0)
                {
                    stringBuilder.Append("<br/>");
                    stringBuilder.Append("<table width='80%' style='border-collapse:collapse;' border='1px' cellpadding='2px' cellspacing='0'>");
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td colspan='4' align='center'><b>Extra Working</b></td>");
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' style='width:50px;'><b>Emp Id.</b></td>");
                    stringBuilder.Append("<td align='left' style='width:250px;'><b>Employee Name</b></td>");
                    stringBuilder.Append("<td align='left' style='width:80px;'><b>Date</b></td>");
                    stringBuilder.Append("<td align='left'><b>Remarks</b></td>");
                    stringBuilder.Append("</tr>");
                    foreach (DataRow row in (InternalDataCollectionBase)table2.Rows)
                    {
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td align='left'>" + row["EmpId"].ToString() + "</td>");
                        stringBuilder.Append("<td align='left' style='white-space:nowrap;'>" + row["EmpName"].ToString() + "</td>");
                        stringBuilder.Append("<td align='left'>" + row["EWDate"].ToString() + "</td>");
                        stringBuilder.Append("<td align='left'>" + row["Remarks"].ToString() + "</td>");
                        stringBuilder.Append("</tr>");
                    }
                    stringBuilder.Append("</table>");
                }
                litReport.Text = stringBuilder.ToString();
                Session["MonthwiseAttendance"] = stringBuilder.ToString().Trim();
                Session["Month"] = drpMonth.SelectedItem.Text;
                Session["Year"] = drpYear.SelectedValue;
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
        int year = int.Parse(drpYear.SelectedValue.ToString());
        try
        {
            dataTable1.Columns.Add("EmpId");
            dataTable1.Columns.Add("EmpName");
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
            throw new Exception("Can not Generate Report for " + drpMonth.SelectedItem.Text + "-" + drpYear.SelectedValue);
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
                stringBuilder.Append("Employee Attendance Report :- ");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(litReport.Text.ToString().Trim());
                stringBuilder.Append("<table><tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel(stringBuilder.ToString());
            }
            else
                Response.Write("<script language='javascript'>alert('No Data Exists to Export');</script>");
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
            string str = Server.MapPath("../Reports/Exported_Files/EmpAttendance" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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