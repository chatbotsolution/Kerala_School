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

public partial class HR_rptLeaveAvailedSum : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        bindDropDown(drpAccYear, "select distinct CalYear from dbo.HR_EmpLeaveDtls order by CalYear desc", "CalYear", "CalYear");
        bindLeave(drpLeave, "SELECT LeaveId,LeaveCode FROm dbo.HR_LeaveMaster ORDER BY LeaveCode", "LeaveCode", "LeaveId");
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
        DataTable dataTableQry1 = obj.GetDataTableQry("SELECT EmpId, SevName from HR_EmployeeMaster where ActiveStatus=1  order by SevName");
        int num1 = 0;
        stringBuilder.Append("<table width='100%' style='border-collapse:collapse;' border='1px' cellpadding='2px' cellspacing='0'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='5' style='font-size: 14px; background-color: Silver;' align='center'><b>Leave Register For The Year " + dtpFromDt.GetDateValue().ToString("yyyy") + " (All Staff)</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("</br>");
        stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 4%; border-bottom:solid 1px black; border-right:solid 1px black;' align='Center'><b>Sl No.</b></td>");
        stringBuilder.Append("<td style='width: 25%; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>Name</b></td>");
        stringBuilder.Append("<td  border-bottom:solid 1px black; border-right:solid 1px black;' align='Center'><b>Details</b></td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row1 in (InternalDataCollectionBase)dataTableQry1.Rows)
        {
            Hashtable hashtable = new Hashtable();
            ++num1;
            if (drpLeave.SelectedIndex > 0)
                hashtable.Add("@LeaveId", drpLeave.SelectedValue);
            hashtable.Add("@EmpId", Convert.ToInt32(row1["EmpId"].ToString()));
            hashtable.Add("@FromDt", dtpFromDt.GetDateValue().ToString("dd-MMM-yyyy"));
            hashtable.Add("@ToDt", dtpToDt.GetDateValue().ToString("dd-MMM-yyyy"));
            hashtable.Add("@CalYear", Convert.ToInt32(ViewState["LeaveYr"]));
            DataTable table = obj.GetDataSet("HR_RptLeaveAvailed", hashtable).Tables[1];
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 4%; border-bottom:solid 1px black; border-right:solid 1px black;' align='Center' valign='top'><b>" + num1 + "</b></td>");
            stringBuilder.Append("<td style='width: 25%; border-bottom:solid 1px black; border-right:solid 1px black;' align='left' valign='top'><b>" + row1["SevName"].ToString().Trim() + "</b></td>");
            stringBuilder.Append("<td>");
            if (table.Rows.Count > 0)
            {
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                double num5 = 0.0;
                stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='width: 5%; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>No.</b></td>");
                stringBuilder.Append("<td style='width: 20%; border-bottom:solid 1px black; border-right:solid 1px black;' align='left'><b>Leave Type</b></td>");
                stringBuilder.Append("<td style='width: 22%; border-bottom:solid 1px black; border-right:solid 1px black;' align='Right'><b>Balance Leave (Prev. Year)</b></td>");
                stringBuilder.Append("<td style='width: 20%; border-bottom:solid 1px black; border-right:solid 1px black;' align='right'><b>Days Authorized</b></td>");
                stringBuilder.Append("<td style='width: 13%; border-bottom:solid 1px black; border-right:solid 1px black;' align='right'><b>Days Availed</b></td>");
                stringBuilder.Append("<td style='width: 20%; border-bottom:solid 1px black;' align='right'><b>Balance Leave (Prev. Year + Current Year)</b></td>");
                stringBuilder.Append("</tr>");
                int num6 = 1;
                foreach (DataRow row2 in (InternalDataCollectionBase)table.Rows)
                {
                    Decimal num7 = new Decimal(0);
                    Decimal num8 = new Decimal(0);
                    Decimal num9 = new Decimal(0);
                    Decimal num10 = new Decimal(0);
                    Decimal num11 = new Decimal(0);
                    Decimal num12 = new Decimal(0);
                    if (row2["AuthDays"].ToString().Trim() != string.Empty)
                        num7 = Convert.ToDecimal(row2["AuthDays"].ToString().Trim());
                    if (row2["DaysAvailed"].ToString().Trim() != string.Empty)
                        num8 = Convert.ToDecimal(row2["DaysAvailed"].ToString().Trim());
                    string str = "select ISNULL(SUM(AuthDays)-SUM(AvlDays),0),ISNULL(SUM(AvlDays),0) from HR_EmpLeaveDtls where EmpId=" + row1["EmpId"].ToString() + "  and LeaveId=" + row2["LeaveId"].ToString() + " and CalYear<" + ViewState["LeaveYr"];
                    if (row2["BalanceLeave"].ToString().Trim() != string.Empty)
                        num11 = Convert.ToDecimal(row2["BalanceLeave"]);
                    Decimal num13;
                    if (row2["CFAllowed"].ToString().Trim().ToLower() == "true")
                    {
                        DataTable dataTableQry2 = obj.GetDataTableQry(str);
                        if (dataTableQry2.Rows[0][0].ToString().Trim() != string.Empty)
                            num9 = Convert.ToDecimal(dataTableQry2.Rows[0][0].ToString().Trim());
                        if (dataTableQry2.Rows[0][0].ToString().Trim() != string.Empty)
                            num10 = Convert.ToDecimal(dataTableQry2.Rows[0][1].ToString().Trim());
                        num13 = num9 + num7 - (num10 + num8);
                    }
                    else
                        num13 = num11;
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder.Append(num6.ToString());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='left' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder.Append(row2["LeaveCode"].ToString().Trim());
                    stringBuilder.Append("&nbsp(" + row2["LeaveDesc"].ToString().Trim() + ")");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='Right' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder.Append((num9 - num10).ToString());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='right' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder.Append(num7.ToString());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='right' style='border-bottom:solid 1px black; border-right:solid 1px black;' >");
                    stringBuilder.Append(num8.ToString());
                    stringBuilder.Append("</td>");
                    if (row2["LeaveId"].ToString() == "5")
                    {
                        stringBuilder.Append("<td align='right' style='border-bottom:solid 1px black;'>0</td>");
                        num4 += Convert.ToDouble("0");
                    }
                    else
                    {
                        stringBuilder.Append("<td align='right' style='border-bottom:solid 1px black;'>");
                        stringBuilder.Append(num13.ToString());
                        stringBuilder.Append("</td>");
                        num4 += Convert.ToDouble(num13.ToString());
                    }
                    num5 += Convert.ToDouble((num9 - num10).ToString());
                    num2 += Convert.ToDouble(num7.ToString());
                    num3 += Convert.ToDouble(num8.ToString());
                    stringBuilder.Append("</tr>");
                    ++num6;
                }
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='2' align='Right' style='border-bottom:solid 1px black; '><b>Total</b></td>");
                stringBuilder.Append("<td  align='Right' style='border-bottom:solid 1px black; '><b>" + num5.ToString() + "<b></td>");
                stringBuilder.Append("<td  align='Right' style='border-bottom:solid 1px black; '><b>" + num2.ToString() + "<b></td>");
                stringBuilder.Append("<td align='Right' style='border-bottom:solid 1px black; '><b>" + num3.ToString() + "</b></td>");
                stringBuilder.Append("<td align='Right' style='border-bottom:solid 1px black; '><b>" + num4.ToString() + "</b></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
            }
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table></fieldset>");
        Session["printData"] = (lblReport.Text = stringBuilder.ToString().Trim());
        if (!(stringBuilder.ToString().Trim() != string.Empty))
            return;
        btnPrint.Visible = true;
    }

    protected void drpLeave_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpAccYear.SelectedIndex > 0)
        {
            GenerateReport();
        }
        else
        {
            btnPrint.Visible = false;
            Session["printData"] = (lblReport.Text = (string)null);
        }
        drpLeave.Focus();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (drpAccYear.SelectedIndex > 0)
            GenerateReport();
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
            GenerateReport();
        }
        else
        {
            btnPrint.Visible = false;
            Session["printData"] = (lblReport.Text = (string)null);
        }
        drpAccYear.Focus();
    }
}