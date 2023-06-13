using AjaxControlToolkit;
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

public partial class HR_rptLeaveClaim : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (!ChkIsHRUsed())
            Response.Redirect("HRHome.aspx?IsHRUsed=0");
        else
            bindEmployee();
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void bindEmployee()
    {
        drpEmpName.DataSource = obj.GetDataTable("HR_GetEmpList");
        drpEmpName.DataTextField = "Employee";
        drpEmpName.DataValueField = "EmpId";
        drpEmpName.DataBind();
        drpEmpName.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    protected void drpEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateReport();
    }

    protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateReport();
    }

    private void GenerateReport()
    {
        Session["LeaveClaimReport"] = null;
        StringBuilder stringBuilder = new StringBuilder();
        Hashtable hashtable = new Hashtable();
        if (drpEmpName.SelectedIndex > 0)
            hashtable.Add("EmpId", drpEmpName.SelectedValue);
        if (drpStatus.SelectedIndex > 0)
            hashtable.Add("ClaimStatus", drpStatus.SelectedValue);
        DataTable dataTable = obj.GetDataTable("HR_rptLeaveClaim", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            stringBuilder.Append("<fieldset>");
            stringBuilder.Append("<legend style='color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana'>");
            stringBuilder.Append("Leave Claim Report</legend>");
            stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left'><font color='Black'><strong>Employee Name : </strong> " + drpEmpName.SelectedItem.Text + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center'><font color='Black'><strong>Status : </strong> " + drpStatus.SelectedItem.Text + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right'><font color='Black'><strong>No of Records : </strong>" + dataTable.Rows.Count + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table cellpadding='2' cellspacing='0' style='border:1px solid;border-bottom:none'  width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 150px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Employee Name</b></td>");
            stringBuilder.Append("<td style='width: 80px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Claim Date</b></td>");
            stringBuilder.Append("<td style='width: 100px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Leave Type</b></td>");
            stringBuilder.Append("<td style='width: 80px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Leave Start Date</b></td>");
            stringBuilder.Append("<td style='width: 80px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Leave End Date</b></td>");
            stringBuilder.Append("<td style='width: 50px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Extra Days Availed</b></td>");
            stringBuilder.Append("<td style='width: 30px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>WPL</b></td>");
            stringBuilder.Append("<td style='width: 150px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Reason</b></td>");
            stringBuilder.Append("<td style='width: 80px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Approved Date</b></td>");
            stringBuilder.Append("<td style='width: 80px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Approved Days</b></td>");
            stringBuilder.Append("<td style='width: 70px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Amount</b></td>");
            stringBuilder.Append("<td style='width: 30px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Month</b></td>");
            stringBuilder.Append("<td style='width: 50px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Year</b></td>");
            stringBuilder.Append("<td style='width: 80px;border-bottom:1px solid;border-right:1px solid;' align='left'><b>Paid Date</b></td>");
            stringBuilder.Append("<td style='width: 50px;border-bottom:1px solid;' align='left'><b>Status</b></td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;'>");
                stringBuilder.Append(row["EmpName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;' >");
                stringBuilder.Append(Convert.ToDateTime(row["ClaimDate"]).ToString("dd-MMM-yyyy"));
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;' >");
                stringBuilder.Append(row["LeaveCode"].ToString().Trim());
                stringBuilder.Append("&nbsp;(" + row["LeaveDesc"].ToString().Trim() + ")");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;'>");
                stringBuilder.Append(Convert.ToDateTime(row["LeaveStartDt"]).ToString("dd-MMM-yyyy"));
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;' >");
                stringBuilder.Append(Convert.ToDateTime(row["LeaveEndDt"]).ToString("dd-MMM-yyyy"));
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;' >");
                stringBuilder.Append(row["ExtraDaysAvailed"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;'>");
                stringBuilder.Append(row["WPL"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;'>");
                stringBuilder.Append(row["Reason"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;'>");
                if (row["ApprovedDate"].ToString().Trim() != string.Empty)
                    stringBuilder.Append(Convert.ToDateTime(row["ApprovedDate"]).ToString("dd-MMM-yyyy"));
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;'>");
                stringBuilder.Append(row["DaysApproved"]);
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;'>");
                if (row["PaidAmt"].ToString().Trim() != string.Empty)
                    stringBuilder.Append(Convert.ToDecimal(row["PaidAmt"]).ToString("0.00"));
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;'>");
                stringBuilder.Append(row["PaidMonth"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;'>");
                stringBuilder.Append(row["PaidYear"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;border-right:1px solid;'>");
                if (row["PaidDate"].ToString().Trim() != string.Empty)
                    stringBuilder.Append(Convert.ToDateTime(row["PaidDate"]).ToString("dd-MMM-yyyy"));
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:1px solid;' >");
                stringBuilder.Append(row["ClaimStatus"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table></fieldset>");
            Session["LeaveClaimReport"] = stringBuilder.ToString();
            btnPrint.Enabled = true;
        }
        else
        {
            stringBuilder.Append("<fieldset><table width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center'><b>No Record Available</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table></fieldset>");
            btnPrint.Enabled = false;
        }
        lblReport.Text = stringBuilder.ToString().Trim();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }
}