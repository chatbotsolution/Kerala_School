using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_rptLeaveAvailed : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        bindDropDown(drpAccYear, "select distinct CalYear from dbo.HR_EmpLeaveDtls order by CalYear desc", "CalYear", "CalYear");
        BindDrpEmp();
        bindLeave(drpLeave, "SELECT LeaveId,LeaveCode FROm dbo.HR_LeaveMaster ORDER BY LeaveCode", "LeaveCode", "LeaveId");
        ViewState["LeaveYr"] = GetLeaveYear();
        dtpFromDt.SetDateValue(Convert.ToDateTime(ViewState["YearStart"]));
        dtpToDt.SetDateValue(Convert.ToDateTime(ViewState["YearEnd"]));
        CheckAuthLeaveAll();
    }

    private void BindDrpEmp()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT EmpId, SevName from HR_EmployeeMaster ");
        stringBuilder.Append(" WHERE 1=1 ");
        stringBuilder.Append("  order by SevName");
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        drpEmpName.DataSource = obj.GetDataTableQry(stringBuilder.ToString());
        drpEmpName.DataTextField = "SevName";
        drpEmpName.DataValueField = "EmpId";
        drpEmpName.DataBind();
        drpEmpName.Items.Insert(0, new ListItem("- All -", "0"));
    }

    private void CheckAuthLeaveAll()
    {
        string str = obj.ExecuteScalar("HR_LeaveAuthAll", new Hashtable()
    {
      {
         "@CalYear",
         Convert.ToInt32(ViewState["LeaveYr"].ToString().Trim())
      },
      {
         "@UserId",
        Session["User_Id"]
      }
    });
        if (!(str.Trim() != "") || !(str.Trim().ToUpper() == "NA"))
            return;
        lblMsg.ForeColor = Color.Red;
        lblMsg.Text = "Please Authorize Leave For all Designations!!";
    }

    private string GetLeaveYear()
    {
        string str1 = string.Empty;
        ViewState["LeaveAcYr"] = ConfigurationManager.AppSettings["LeaveAcYr"].ToString().Trim();
        if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL")
        {
            str1 = DateTime.Now.Year.ToString();
            ViewState["Yr"] = str1;
            ViewState["YearStart"] = Convert.ToDateTime("01-JAN-" + str1);
            ViewState["YearEnd"] = Convert.ToDateTime("31-DEC-" + str1);
        }
        else if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "SESS")
        {
            string str2 = obj.ExecuteScalarQry("select dbo.fuGetSessionYr('" + DateTime.Now.ToString("dd-MMM-yyyy") + "')");
            ViewState["Yr"] = str2;
            string[] strArray = str2.Split('-');
            str1 = strArray[0] + strArray[1];
            ViewState["YearStart"] = Convert.ToDateTime("01-APR-" + strArray[0]);
            ViewState["YearEnd"] = Convert.ToDateTime("31-MAR-" + (Convert.ToInt32(strArray[0]) + 1).ToString());
        }
        return str1;
    }

    private string GetLeaveYearFrmDrp()
    {
        string empty = string.Empty;
        ViewState["LeaveAcYr"] = ConfigurationManager.AppSettings["LeaveAcYr"].ToString().Trim();
        if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL")
        {
            empty = drpAccYear.SelectedValue.ToString();
            ViewState["Yr"] = empty;
            ViewState["YearStart"] = Convert.ToDateTime("01-JAN-" + empty);
            ViewState["YearEnd"] = Convert.ToDateTime("31-DEC-" + empty);
        }
        else if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "SESS")
        {
            empty = drpAccYear.SelectedValue.ToString();
            ViewState["Yr"] = empty;
            string str = drpAccYear.SelectedValue.Substring(0, 4);
            ViewState["YearStart"] = Convert.ToDateTime("01-APR-" + str);
            ViewState["YearEnd"] = Convert.ToDateTime("31-MAR-" + (Convert.ToInt32(str) + 1).ToString());
        }
        return empty;
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    private void bindLeave(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    private void GenerateReport()
    {
        StringBuilder stringBuilder = new StringBuilder();
        Hashtable hashtable = new Hashtable();
        if (drpEmpName.SelectedIndex > 0)
            hashtable.Add("@EmpId", drpEmpName.SelectedValue);
        if (drpLeave.SelectedIndex > 0)
            hashtable.Add("@LeaveId", drpLeave.SelectedValue);
        hashtable.Add("@FromDt", dtpFromDt.GetDateValue().ToString("dd-MMM-yyyy"));
        hashtable.Add("@ToDt", dtpToDt.GetDateValue().ToString("dd-MMM-yyyy"));
        hashtable.Add("@CalYear", Convert.ToInt32(ViewState["LeaveYr"]));
        DataSet dataSet = obj.GetDataSet("HR_RptLeaveAvailed", hashtable);
        DataTable table1 = dataSet.Tables[0];
        DataTable table2 = dataSet.Tables[1];
        if (table2.Rows.Count > 0)
        {
            double num1 = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            double num4 = 0.0;
            stringBuilder.Append("<fieldset>");
            stringBuilder.Append("<legend style='color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana'>");
            stringBuilder.Append("Leave Status : " + drpEmpName.SelectedItem.ToString().ToUpper() + "&nbsp;</legend>");
            stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 5%; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>No.</b></td>");
            stringBuilder.Append("<td style='width: 20%; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>Leave Type</b></td>");
            stringBuilder.Append("<td style='width: 20%; border-bottom:solid 1px black; border-right:solid 1px black;' align='Right'><b>Balance Leave (Prev. Year)</b></td>");
            stringBuilder.Append("<td style='width: 20%; border-bottom:solid 1px black; border-right:solid 1px black;' align='Right'><b>Days Authorized (Current Year)</b></td>");
            stringBuilder.Append("<td style='width: 15%; border-bottom:solid 1px black; border-right:solid 1px black;' align='Right'><b>Days Availed</b></td>");
            stringBuilder.Append("<td style='width: 20%; border-bottom:solid 1px black;' align='Right'><b>Balance Leave (Prev. Year + Current Year)</b></td>");
            stringBuilder.Append("</tr>");
            int num5 = 1;
            foreach (DataRow row in (InternalDataCollectionBase)table2.Rows)
            {
                Decimal num6 = new Decimal(0);
                Decimal num7 = new Decimal(0);
                Decimal num8 = new Decimal(0);
                Decimal num9 = new Decimal(0);
                Decimal num10 = new Decimal(0);
                Decimal num11 = new Decimal(0);
                if (row["AuthDays"].ToString().Trim() != string.Empty)
                    num6 = Convert.ToDecimal(row["AuthDays"].ToString().Trim());
                if (row["DaysAvailed"].ToString().Trim() != string.Empty)
                    num7 = Convert.ToDecimal(row["DaysAvailed"].ToString().Trim());
                string str = "select ISNULL(SUM(AuthDays)-SUM(AvlDays),0),ISNULL(SUM(AvlDays),0) from HR_EmpLeaveDtls where EmpId=" + drpEmpName.SelectedValue + "  and LeaveId=" + row["LeaveId"].ToString() + " and CalYear<" + ViewState["LeaveYr"];
                if (row["BalanceLeave"].ToString().Trim() != string.Empty)
                    num10 = Convert.ToDecimal(row["BalanceLeave"]);
                if (row["CFAllowed"].ToString().Trim().ToLower() == "true")
                {
                    DataTable dataTableQry = obj.GetDataTableQry(str);
                    if (dataTableQry.Rows[0][0].ToString().Trim() != string.Empty)
                        num8 = Convert.ToDecimal(dataTableQry.Rows[0][0].ToString().Trim());
                    if (dataTableQry.Rows[0][0].ToString().Trim() != string.Empty)
                        num9 = Convert.ToDecimal(dataTableQry.Rows[0][1].ToString().Trim());
                    num11 = num8 + num6 - (num9 + num7);
                }
                else
                    num11 = num10;
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                stringBuilder.Append(num5.ToString());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                stringBuilder.Append(row["LeaveCode"].ToString().Trim());
                stringBuilder.Append("&nbsp(" + row["LeaveDesc"].ToString().Trim() + ")");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='Right' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                stringBuilder.Append((num8 - num9).ToString());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                stringBuilder.Append(num6.ToString());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                stringBuilder.Append(num7.ToString());
                stringBuilder.Append("</td>");
                if (row["LeaveId"].ToString() == "5")
                {
                    stringBuilder.Append("<td align='Right' style='border-bottom:solid 1px black;'>0</td>");
                    num3 += Convert.ToDouble("0");
                }
                else
                {
                    stringBuilder.Append("<td align='Right' style='border-bottom:solid 1px black;'>");
                    stringBuilder.Append(num11.ToString());
                    stringBuilder.Append("</td>");
                    num3 += Convert.ToDouble(num11.ToString());
                }
                num4 += Convert.ToDouble((num8 - num9).ToString());
                num1 += Convert.ToDouble(num6.ToString());
                num2 += Convert.ToDouble(num7.ToString());
                stringBuilder.Append("</tr>");
                ++num5;
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='2' align='Right' style='border-bottom:solid 1px black; '><b>Total</b></td>");
            stringBuilder.Append("<td  align='Right' style='border-bottom:solid 1px black; '><b>" + num4.ToString() + "<b></td>");
            stringBuilder.Append("<td  align='Right' style='border-bottom:solid 1px black; '><b>" + num1.ToString() + "<b></td>");
            stringBuilder.Append("<td align='Right' style='border-bottom:solid 1px black; '><b>" + num2.ToString() + "</b></td>");
            stringBuilder.Append("<td align='Right' style='border-bottom:solid 1px black; '><b>" + num3.ToString() + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table></fieldset>");
        }
        if (table1.Rows.Count > 0)
        {
            double num1 = 0.0;
            stringBuilder.Append("<fieldset>");
            stringBuilder.Append("<legend style='color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana'>");
            stringBuilder.Append("Leave History : " + drpEmpName.SelectedItem.ToString().ToUpper() + "</legend>");
            stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left'><font color='Black'>");
            stringBuilder.Append("<strong>Leave Type : </strong> " + drpLeave.SelectedItem.ToString() + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'><font color='Black'>");
            stringBuilder.Append("<strong>From Date : </strong> " + dtpFromDt.GetDateValue().ToString("dd-MMM-yyyy") + "</font>");
            stringBuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;<strong>To Date : </strong> " + dtpToDt.GetDateValue().ToString("dd-MMM-yyyy") + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right'><font color='Black'><strong>No Of Records : </strong> " + table1.Rows.Count.ToString() + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 40px; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>No.</b></td>");
            if (drpLeave.SelectedIndex == 0)
                stringBuilder.Append("<td style='width: 70px; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>Leave Type</b></td>");
            stringBuilder.Append("<td style='width: 100px; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>Start Date</b></td>");
            stringBuilder.Append("<td style='width: 100px; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>End Date</b></td>");
            stringBuilder.Append("<td style='width: 150px; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>Reason</b></td>");
            stringBuilder.Append("<td style='width: 100px; border-bottom:solid 1px black;' align='left'><b>Days Availed</b></td>");
            stringBuilder.Append("</tr>");
            int num2 = 1;
            foreach (DataRow row in (InternalDataCollectionBase)table1.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                stringBuilder.Append(num2.ToString());
                stringBuilder.Append("</td>");
                if (drpLeave.SelectedIndex == 0)
                {
                    stringBuilder.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder.Append(row["LeaveCode"].ToString().Trim());
                    stringBuilder.Append("</td>");
                }
                stringBuilder.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                stringBuilder.Append(row["LeaveStartDt"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                stringBuilder.Append(row["LeaveEndDt"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                stringBuilder.Append(row["Reason"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' style='border-bottom:solid 1px black;'>");
                stringBuilder.Append(row["DaysAvailed"].ToString().Trim());
                stringBuilder.Append("</td>");
                num1 += Convert.ToDouble(row["DaysAvailed"].ToString());
                stringBuilder.Append("</tr>");
                ++num2;
            }
            stringBuilder.Append("<tr>");
            if (drpLeave.SelectedIndex == 0)
                stringBuilder.Append("<td colspan='5' align='Right' style='border-bottom:solid 1px black; '><b>Total</b></td>");
            else
                stringBuilder.Append("<td colspan='4' align='Right' style='border-bottom:solid 1px black; '><b>Total</b></td>");
            stringBuilder.Append("<td  align='Right' style='border-bottom:solid 1px black; '><b>" + num1.ToString() + "<b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table></fieldset>");
        }
        Session["printData"] = (lblReport.Text = stringBuilder.ToString().Trim());
        if (!(stringBuilder.ToString().Trim() != string.Empty))
            return;
        btnPrint.Visible = true;
    }

    private void GenerateReportNew()
    {
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append("SELECT EmpId, SevName from HR_EmployeeMaster ");
        stringBuilder2.Append(" WHERE activestatus=1 ");
        stringBuilder2.Append("  order by SevName");
        DataTable dataTable = new DataTable();
        DataTable dataTableQry1 = obj.GetDataTableQry(stringBuilder2.ToString());
        stringBuilder1.Append("<table width='100%' style='border-collapse:collapse;' border='1px' cellpadding='2px' cellspacing='0'>");
        stringBuilder1.Append("<tr>");
        stringBuilder1.Append("<td colspan='5' style='font-size: 14px; background-color: Silver;' align='center'><b>Leave Register For The Year " + dtpFromDt.GetDateValue().ToString("yyyy") + " (All Staff)</b></td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("</table>");
        foreach (DataRow row1 in (InternalDataCollectionBase)dataTableQry1.Rows)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@EmpId", row1["EmpId"].ToString());
            if (drpLeave.SelectedIndex > 0)
                hashtable.Add("@LeaveId", drpLeave.SelectedValue);
            hashtable.Add("@FromDt", dtpFromDt.GetDateValue().ToString("dd-MMM-yyyy"));
            hashtable.Add("@ToDt", dtpToDt.GetDateValue().ToString("dd-MMM-yyyy"));
            hashtable.Add("@CalYear", Convert.ToInt32(ViewState["LeaveYr"]));
            DataSet dataSet = obj.GetDataSet("HR_RptLeaveAvailed", hashtable);
            DataTable table1 = dataSet.Tables[0];
            DataTable table2 = dataSet.Tables[1];
            if (table2.Rows.Count > 0)
            {
                double num1 = 0.0;
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                stringBuilder1.Append("</br>");
                stringBuilder1.Append("<fieldset>");
                stringBuilder1.Append("<legend style='color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana'>");
                stringBuilder1.Append(row1["Sevname"].ToString().ToUpper() + "</legend>");
                stringBuilder1.Append("<legend style='color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana'>");
                stringBuilder1.Append("Leave Status : </legend>");
                stringBuilder1.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td style='width: 5%; border-top:solid 2px black; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>No.</b></td>");
                stringBuilder1.Append("<td style='width: 20%; border-top:solid 2px black; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>Leave Type</b></td>");
                stringBuilder1.Append("<td style='width: 20%; border-top:solid 2px black; border-bottom:solid 1px black; border-right:solid 1px black;' align='Right'><b>Balance Leave (Prev. Year)</b></td>");
                stringBuilder1.Append("<td style='width: 20%; border-top:solid 2px black; border-bottom:solid 1px black; border-right:solid 1px black;' align='Right'><b>Days Authorized(Current Year)</b></td>");
                stringBuilder1.Append("<td style='width: 15%; border-top:solid 2px black; border-bottom:solid 1px black; border-right:solid 1px black;' align='Right'><b>Days Availed</b></td>");
                stringBuilder1.Append("<td style='width: 20%; border-top:solid 2px black; border-bottom:solid 1px black;' align='Right'><b>Balance Leave (Prev. Year + Current Year)</b></td>");
                stringBuilder1.Append("</tr>");
                int num5 = 1;
                foreach (DataRow row2 in (InternalDataCollectionBase)table2.Rows)
                {
                    Decimal num6 = new Decimal(0);
                    Decimal num7 = new Decimal(0);
                    Decimal num8 = new Decimal(0);
                    Decimal num9 = new Decimal(0);
                    Decimal num10 = new Decimal(0);
                    Decimal num11 = new Decimal(0);
                    if (row2["AuthDays"].ToString().Trim() != string.Empty)
                        num6 = Convert.ToDecimal(row2["AuthDays"].ToString().Trim());
                    if (row2["DaysAvailed"].ToString().Trim() != string.Empty)
                        num7 = Convert.ToDecimal(row2["DaysAvailed"].ToString().Trim());
                    string str = "select ISNULL(SUM(AuthDays)-SUM(AvlDays),0),ISNULL(SUM(AvlDays),0) from HR_EmpLeaveDtls where EmpId=" + row1["EmpId"].ToString() + "  and LeaveId=" + row2["LeaveId"].ToString() + " and CalYear<" + ViewState["LeaveYr"];
                    if (row2["BalanceLeave"].ToString().Trim() != string.Empty)
                        num10 = Convert.ToDecimal(row2["BalanceLeave"]);
                    Decimal num12;
                    if (row2["CFAllowed"].ToString().Trim().ToLower() == "true")
                    {
                        DataTable dataTableQry2 = obj.GetDataTableQry(str);
                        if (dataTableQry2.Rows[0][0].ToString().Trim() != string.Empty)
                            num8 = Convert.ToDecimal(dataTableQry2.Rows[0][0].ToString().Trim());
                        if (dataTableQry2.Rows[0][0].ToString().Trim() != string.Empty)
                            num9 = Convert.ToDecimal(dataTableQry2.Rows[0][1].ToString().Trim());
                        num12 = num8 + num6 - (num9 + num7);
                    }
                    else
                        num12 = num10;
                    stringBuilder1.Append("<tr>");
                    stringBuilder1.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder1.Append(num5.ToString());
                    stringBuilder1.Append("</td>");
                    stringBuilder1.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder1.Append(row2["LeaveCode"].ToString().Trim());
                    stringBuilder1.Append("&nbsp(" + row2["LeaveDesc"].ToString().Trim() + ")");
                    stringBuilder1.Append("</td>");
                    stringBuilder1.Append("<td align='Right' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder1.Append((num8 - num9).ToString());
                    stringBuilder1.Append("</td>");
                    stringBuilder1.Append("<td align='Right' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder1.Append(num6.ToString());
                    stringBuilder1.Append("</td>");
                    stringBuilder1.Append("<td align='Right' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder1.Append(num7.ToString());
                    stringBuilder1.Append("</td>");
                    if (row2["LeaveId"].ToString() == "5")
                    {
                        stringBuilder1.Append("<td align='Right' style='border-bottom:solid 1px black;'>0</td>");
                        num3 += Convert.ToDouble("0");
                    }
                    else
                    {
                        stringBuilder1.Append("<td align='Right' style='border-bottom:solid 1px black;'>");
                        stringBuilder1.Append(num12.ToString());
                        stringBuilder1.Append("</td>");
                        num3 += Convert.ToDouble(num12.ToString());
                    }
                    num4 += Convert.ToDouble((num8 - num9).ToString());
                    num1 += Convert.ToDouble(num6.ToString());
                    num2 += Convert.ToDouble(num7.ToString());
                    stringBuilder1.Append("</tr>");
                    ++num5;
                }
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td colspan='2' align='Right' style='border-bottom:solid 1px black; '><b>Total</b></td>");
                stringBuilder1.Append("<td  align='Right' style='border-bottom:solid 1px black; '><b>" + num4.ToString() + "<b></td>");
                stringBuilder1.Append("<td  align='Right' style='border-bottom:solid 1px black; '><b>" + num1.ToString() + "<b></td>");
                stringBuilder1.Append("<td align='Right' style='border-bottom:solid 1px black; '><b>" + num2.ToString() + "</b></td>");
                stringBuilder1.Append("<td align='Right' style='border-bottom:solid 1px black; '><b>" + num3.ToString() + "</b></td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("</table></fieldset>");
            }
            if (table1.Rows.Count > 0)
            {
                double num1 = 0.0;
                stringBuilder1.Append("<fieldset>");
                stringBuilder1.Append("<legend style='color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana'>");
                stringBuilder1.Append("Leave History</legend>");
                stringBuilder1.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td align='left'><font color='Black'>");
                stringBuilder1.Append("<strong>Leave Type : </strong> " + drpLeave.SelectedItem.ToString() + "</font>");
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='left'><font color='Black'>");
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='right'><font color='Black'><strong>No Of Records : </strong> " + table1.Rows.Count.ToString() + "</font>");
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("</table>");
                stringBuilder1.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td style='width: 40px; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>No.</b></td>");
                if (drpLeave.SelectedIndex == 0)
                    stringBuilder1.Append("<td style='width: 70px; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>Leave Type</b></td>");
                stringBuilder1.Append("<td style='width: 100px; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>Start Date</b></td>");
                stringBuilder1.Append("<td style='width: 100px; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>End Date</b></td>");
                stringBuilder1.Append("<td style='width: 150px; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>Reason</b></td>");
                stringBuilder1.Append("<td style='width: 100px; border-bottom:solid 1px black;' align='left'><b>Days Availed</b></td>");
                stringBuilder1.Append("</tr>");
                int num2 = 1;
                foreach (DataRow row2 in (InternalDataCollectionBase)table1.Rows)
                {
                    stringBuilder1.Append("<tr>");
                    stringBuilder1.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder1.Append(num2.ToString());
                    stringBuilder1.Append("</td>");
                    if (drpLeave.SelectedIndex == 0)
                    {
                        stringBuilder1.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                        stringBuilder1.Append(row2["LeaveCode"].ToString().Trim());
                        stringBuilder1.Append("</td>");
                    }
                    stringBuilder1.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder1.Append(row2["LeaveStartDt"].ToString().Trim());
                    stringBuilder1.Append("</td>");
                    stringBuilder1.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder1.Append(row2["LeaveEndDt"].ToString().Trim());
                    stringBuilder1.Append("</td>");
                    stringBuilder1.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder1.Append(row2["Reason"].ToString().Trim());
                    stringBuilder1.Append("</td>");
                    stringBuilder1.Append("<td align='Right' style='border-bottom:solid 1px black;'>");
                    stringBuilder1.Append(row2["DaysAvailed"].ToString().Trim());
                    stringBuilder1.Append("</td>");
                    num1 += Convert.ToDouble(row2["DaysAvailed"].ToString());
                    stringBuilder1.Append("</tr>");
                    ++num2;
                }
                stringBuilder1.Append("<tr>");
                if (drpLeave.SelectedIndex == 0)
                    stringBuilder1.Append("<td colspan='5' align='Right' style='border-bottom:solid 3px black; '><b>Total</b></td>");
                else
                    stringBuilder1.Append("<td colspan='4' align='Right' style='border-bottom:solid 3px black; '><b>Total</b></td>");
                stringBuilder1.Append("<td  align='Right' style='border-bottom:solid 3px black; '><b>" + num1.ToString() + "<b></td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("</table></fieldset>");
            }
        }
        Session["printData"] = (lblReport.Text = stringBuilder1.ToString().Trim());
        if (!(stringBuilder1.ToString().Trim() != string.Empty))
            return;
        btnPrint.Visible = true;
    }

    protected void drpEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpAccYear.SelectedIndex > 0)
        {
            if (drpEmpName.SelectedIndex > 0)
                GenerateReport();
            else if (drpEmpName.SelectedIndex == 0)
                GenerateReportNew();
        }
        drpEmpName.Focus();
    }

    protected void drpLeave_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpAccYear.SelectedIndex > 0)
        {
            if (drpEmpName.SelectedIndex > 0)
                GenerateReport();
            else if (drpEmpName.SelectedIndex == 0)
                GenerateReportNew();
        }
        drpLeave.Focus();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (drpAccYear.SelectedIndex > 0)
        {
            if (drpEmpName.SelectedIndex > 0)
            {
                GenerateReport();
            }
            else
            {
                if (drpEmpName.SelectedIndex != 0)
                    return;
                GenerateReportNew();
            }
        }
        else
            lblReport.Text = "Select Accounting Year";
    }

    protected void drpAccYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["LeaveYr"] = GetLeaveYearFrmDrp();
        dtpFromDt.SetDateValue(Convert.ToDateTime(ViewState["YearStart"]));
        dtpToDt.SetDateValue(Convert.ToDateTime(ViewState["YearEnd"]));
        CheckAuthLeaveAll();
        if (drpAccYear.SelectedIndex > 0)
        {
            if (drpEmpName.SelectedIndex > 0)
            {
                GenerateReport();
            }
            else
            {
                if (drpEmpName.SelectedIndex != 0)
                    return;
                GenerateReportNew();
            }
        }
        else
        {
            btnPrint.Visible = false;
            Session["printData"] = (lblReport.Text = (string)null);
        }
    }
}