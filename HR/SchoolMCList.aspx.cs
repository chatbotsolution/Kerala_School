using ASP;
using SanLib;
using System;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_SchoolMCList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["ApprLtrNo"] = 0;
        GenerateMCList();
    }

    private void GenerateMCList()
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("HR_GetAllMC");
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<font style='font-size: large;'><b>Managing Commitee List</b></font>");
        stringBuilder.Append("<table width='100%' style='height: 100%;' align='center'>");
        foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
        {
            if (row["ApprovalLetterNo"].ToString().Trim() != ViewState["ApprLtrNo"].ToString().Trim())
            {
                ViewState["ApprLtrNo"] = row["ApprovalLetterNo"].ToString().Trim();
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='5'><hr/></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small; width:21%'><b>Approval Letter No</b></td>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small; width:29%'>&nbsp;:&nbsp;" + row["ApprovalLetterNo"] + "</td>");
                stringBuilder.Append("<td align='right' valign='baseline' style='font-size: small; width:50%' colspan='3'><b>Approval Date</b>");
                stringBuilder.Append("&nbsp;:&nbsp;" + Convert.ToDateTime(row["ApprovalDate"]).ToString("dd-MMM-yyyy") + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small; width:21%'><b>Start Date</b></td>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small; width:29%'>&nbsp;:&nbsp;" + Convert.ToDateTime(row["MCPeriodStartDt"]).ToString("dd-MMM-yyyy") + "</td>");
                stringBuilder.Append("<td align='right' valign='baseline' style='font-size: small; width:50%' colspan='3'><b>End Date</b>");
                stringBuilder.Append("&nbsp;:&nbsp;" + Convert.ToDateTime(row["MCPeriodEndDt"]).ToString("dd-MMM-yyyy") + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='5'><hr/></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small; width:20%'><b>Designation</b></td>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small; width:30%'><b>Member Name</b></td>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small; width:20%'><b>Period</b></td>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small; width:20%'><b>Contact No</b></td>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small; width:20%'><b>Address</b></td>");
                stringBuilder.Append("</tr>");
            }
            if (row["Name"].ToString() != string.Empty)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small;'>" + row["Designation"] + "</td>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small;'>" + row["Name"] + "</td>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: smaller;'>");
                stringBuilder.Append(Convert.ToDateTime(row["FromDt"]).ToString("dd-MMM-yyyy"));
                if (row["ToDt"].ToString() != string.Empty)
                    stringBuilder.Append(" to " + Convert.ToDateTime(row["ToDt"]).ToString("dd-MMM-yyyy") + "</td>");
                else
                    stringBuilder.Append(" to Present</font></td>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small;'>" + row["ContactTel"] + "</td>");
                stringBuilder.Append("<td align='left' valign='baseline' style='font-size: small;'>" + row["Address"] + "</td>");
                stringBuilder.Append("</tr>");
            }
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
    }
}