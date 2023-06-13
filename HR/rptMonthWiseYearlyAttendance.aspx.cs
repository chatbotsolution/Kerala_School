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

public partial class HR_rptMonthWiseYearlyAttendance : System.Web.UI.Page
{
    private clsDAL DAL = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        bindDropDown(drpYear, "SELECT DISTINCT(YEAR(ISNULL(AttendDate,GETDATE()))) AS CalYear FROM dbo.HR_EmpAttendance order by CalYear desc", "CalYear", "CalYear");
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

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        if (rbtnCal.Checked)
        {
            hashtable.Add("@Year", drpYear.SelectedValue);
            DataTable dataTable2 = DAL.GetDataTable("HR_ATTEND_StudAttendMonthWiseColumn", hashtable);
            if (dataTable2.Rows.Count > 0)
            {
                GenerateAttendanceRport(dataTable2);
            }
            else
            {
                btnPrint.Enabled = false;
                btnExpExcel.Enabled = false;
                litReport.Text = "<div style='color:Red;'><b>No Record Found !!!</b></div>";
            }
        }
        else
        {
            hashtable.Add("@StDt", Convert.ToDateTime("1 Apr " + drpYear.SelectedValue.Trim().Split('-')[0]));
            hashtable.Add("@EndDt", Convert.ToDateTime("31 Mar " + (Convert.ToInt32(drpYear.SelectedValue.Trim().Split('-')[0]) + 1).ToString()));
            DataTable dataTable2 = DAL.GetDataTable("HR_ATTEND_EmpAttendColumnSY", hashtable);
            if (dataTable2.Rows.Count > 0)
            {
                GenerateAttendanceRportSY(dataTable2);
            }
            else
            {
                btnPrint.Enabled = false;
                btnExpExcel.Enabled = false;
                litReport.Text = "<div style='color:Red;'><b>No Record Found !!!</b></div>";
            }
        }
    }

    private void GenerateAttendanceRport(DataTable dtAttend)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Clear();
        hashtable.Add("@Year", drpYear.SelectedValue);
        DataTable dataTable2 = DAL.GetDataTable("HR_ATTEND_ColumnHeader", hashtable);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table style='border-collapse:collapse;' border='1px' cellpadding='2px' cellspacing='0' width='100%'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td rowspan='2' align='left' style='width: 50px' ><b>Emp ID</b></td>");
        stringBuilder.Append("<td rowspan='2' align='left' style='width: 200px'><b>Employee Name</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Jan(" + dataTable2.Rows[0]["TotJan"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Feb(" + dataTable2.Rows[0]["TotFeb"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Mar(" + dataTable2.Rows[0]["TotMar"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Apr(" + dataTable2.Rows[0]["TotApr"] + ")</b> </td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>May(" + dataTable2.Rows[0]["TotMay"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Jun(" + dataTable2.Rows[0]["TotJun"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Jul(" + dataTable2.Rows[0]["TotJul"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Aug(" + dataTable2.Rows[0]["TotAug"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Sep(" + dataTable2.Rows[0]["TotSep"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Oct(" + dataTable2.Rows[0]["TotOct"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Nov(" + dataTable2.Rows[0]["TotNov"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Dec(" + dataTable2.Rows[0]["TotDec"] + ")</b></td>");
        stringBuilder.Append("<td colspan='3' align='center'><b>Total(" + dataTable2.Rows[0]["TotYear"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Attendance<b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td><td align='center' style='color:Black'>L</td>");
        stringBuilder.Append("<td align='center' colspan='2' style='color:Green'>%</td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dtAttend.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["EmpId"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' style='white-space:nowrap;'>");
            stringBuilder.Append(row["EmpName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["JanP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["JanA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["FebP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["FebA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["MarP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["MarA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["AprP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["AprA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["MayP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["MayA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["JunP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["JunA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["JulP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["JulA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["AugP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["AugA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["SepP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["SepA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["OctP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["OctA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["NovP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["NovA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["DecP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["DecA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["TotP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["TotA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Black'>");
            stringBuilder.Append(row["TotL"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center'>");
            if (Convert.ToDecimal(row["TotP"].ToString().Trim()) + Convert.ToDecimal(row["TotA"].ToString().Trim()) != new Decimal(0))
                stringBuilder.Append((Convert.ToDecimal(row["TotP"].ToString().Trim()) / Convert.ToDecimal(dataTable2.Rows[0]["TotYear"]) * new Decimal(100)).ToString("0.00"));
            else
                stringBuilder.Append("0");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        litReport.Text = stringBuilder.ToString();
        Session["MonthwiseYrlyAttendance"] = stringBuilder.ToString().Trim();
        Session["SessYr"] = drpYear.SelectedValue.ToString();
        btnPrint.Enabled = true;
        btnExpExcel.Enabled = true;
    }

    private void GenerateAttendanceRportSY(DataTable dtAttend)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Clear();
        hashtable.Add("@Year", drpYear.SelectedValue.Trim().Split('-')[0]);
        DataTable dataTable2 = DAL.GetDataTable("HR_ATTEND_ColumnHeaderSY", hashtable);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table style='border-collapse:collapse;' border='1px' cellpadding='2px' cellspacing='0' width='100%'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td rowspan='2' align='left' style='width: 50px' ><b>Emp ID</b></td>");
        stringBuilder.Append("<td rowspan='2' align='left' style='width: 200px'><b>Employee Name</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Apr(" + dataTable2.Rows[0]["TotApr"] + ")</b> </td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>May(" + dataTable2.Rows[0]["TotMay"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Jun(" + dataTable2.Rows[0]["TotJun"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Jul(" + dataTable2.Rows[0]["TotJul"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Aug(" + dataTable2.Rows[0]["TotAug"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Sep(" + dataTable2.Rows[0]["TotSep"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Oct(" + dataTable2.Rows[0]["TotOct"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Nov(" + dataTable2.Rows[0]["TotNov"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Dec(" + dataTable2.Rows[0]["TotDec"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Jan(" + dataTable2.Rows[0]["TotJan"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Feb(" + dataTable2.Rows[0]["TotFeb"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Mar(" + dataTable2.Rows[0]["TotMar"] + ")</b></td>");
        stringBuilder.Append("<td colspan='3' align='center'><b>Total(" + dataTable2.Rows[0]["TotYear"] + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Attendance<b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td><td align='center' style='color:Black'>L</td>");
        stringBuilder.Append("<td align='center' colspan='2' style='color:Green'>%</td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dtAttend.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["EmpId"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' style='white-space:nowrap;'>");
            stringBuilder.Append(row["EmpName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["AprP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["AprA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["MayP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["MayA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["JunP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["JunA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["JulP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["JulA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["AugP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["AugA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["SepP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["SepA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["OctP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["OctA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["NovP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["NovA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["DecP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["DecA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["JanP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["JanA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["FebP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["FebA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["MarP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["MarA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["TotP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["TotA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Black'>");
            stringBuilder.Append(row["TotL"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center'>");
            if (Convert.ToDecimal(row["TotP"].ToString().Trim()) + Convert.ToDecimal(row["TotA"].ToString().Trim()) != new Decimal(0))
                stringBuilder.Append((Convert.ToDecimal(row["TotP"].ToString().Trim()) / Convert.ToDecimal(dataTable2.Rows[0]["TotYear"]) * new Decimal(100)).ToString("0.00"));
            else
                stringBuilder.Append("0");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        litReport.Text = stringBuilder.ToString();
        Session["MonthwiseYrlyAttendance"] = stringBuilder.ToString().Trim();
        Session["SessYr"] = drpYear.SelectedValue.ToString();
        btnPrint.Enabled = true;
        btnExpExcel.Enabled = true;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptMonthWiseYearlyAttendancePrint.aspx');", true);
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
                stringBuilder.Append("Attendance Report :- ");
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
            string str = Server.MapPath("../Reports/Exported_Files/MonthWiseYearlyAttendance" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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

    protected void rbtnCal_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnCal.Checked)
        {
            bindDropDown(drpYear, "SELECT DISTINCT(YEAR(ISNULL(AttendDate,GETDATE()))) AS CalYear FROM dbo.HR_EmpAttendance order by CalYear desc", "CalYear", "CalYear");
        }
        else
        {
            DataTable dataTableQry = DAL.GetDataTableQry("select FY_Id,FY from dbo.ACTS_FinancialYear order by FY_Id desc");
            if (dataTableQry.Rows.Count > 0)
            {
                drpYear.DataSource = dataTableQry;
                drpYear.DataTextField = "FY";
                drpYear.DataValueField = "FY";
                drpYear.DataBind();
            }
        }
        litReport.Text = "";
    }
}