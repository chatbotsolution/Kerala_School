using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Hostel_rptHostIssueInventory : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] != null)
            return;
        Response.Redirect("../Login.aspx");
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        try
        {
            GenerateReport();
        }
        catch (Exception ex)
        {
        }
    }

    private void GenerateReport()
    {
        if (txtFromDate.Text != "")
            ht.Add("@FromDate", pcalFromDate.GetDateValue());
        if (txtToDate.Text != "")
            ht.Add("@Todate", pcalToDate.GetDateValue());
        if (Session["User"].ToString() != "admin")
            ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
        dt = obj.GetDataTable("Host_rptIssueDtls", ht);
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0'  width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 250px' align='left' class='tblheader'><b>Issue To</b></td>");
            stringBuilder.Append("<td style='width: 90px' align='left' class='tblheader'><b>Issue Date</b></td>");
            stringBuilder.Append("<td align='left' class='tblheader'><b>Item Name</b></td>");
            stringBuilder.Append("<td style='width: 100px' align='right' class='tblheader'><b>Quantity</b></td>");
            stringBuilder.Append("</tr>");
            int num = 0;
            int index1 = 0;
            string str = dt.Rows[0]["IssuedToAdmnNo"].ToString();
            for (int index2 = 0; index2 < dt.Rows.Count; ++index2)
            {
                stringBuilder.Append("<tr>");
                if (str == dt.Rows[index2]["IssuedToAdmnNo"].ToString())
                {
                    ++num;
                }
                else
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' rowspan=" + num + ">");
                    stringBuilder.Append(dt.Rows[index1]["IssuedTo"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    for (; index1 < dt.Rows.Count; ++index1)
                    {
                        stringBuilder.Append("<td align='left' >");
                        stringBuilder.Append(dt.Rows[index1]["IssueDt"].ToString().Trim());
                        stringBuilder.Append("</td>");
                        stringBuilder.Append("<td align='left' >");
                        stringBuilder.Append(dt.Rows[index1]["ItemName"].ToString().Trim());
                        stringBuilder.Append("</td>");
                        stringBuilder.Append("<td align='right' >");
                        stringBuilder.Append(dt.Rows[index1]["Qty"].ToString().Trim());
                        stringBuilder.Append("</td>");
                        stringBuilder.Append("</tr>");
                    }
                }
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' rowspan=" + num + ">");
            stringBuilder.Append(dt.Rows[index1]["IssuedTo"].ToString().Trim());
            stringBuilder.Append("</td>");
            for (; index1 < dt.Rows.Count; ++index1)
            {
                stringBuilder.Append("<td align='left' >");
                stringBuilder.Append(dt.Rows[index1]["IssueDt"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' >");
                stringBuilder.Append(dt.Rows[index1]["ItemName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' >");
                stringBuilder.Append(dt.Rows[index1]["Qty"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["printData"] = Regex.Replace(stringBuilder.ToString(), "<a\\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
            btnExpExcel.Visible = true;
            btnExpExcel.Enabled = true;
        }
        else
        {
            Session["printData"] = (lblReport.Text = "<div style='border:solid 1px Black'>No records found !</div>");
            btnExpExcel.Enabled = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td align='left' colspan='14'><b>");
                stringBuilder.Append("Hostel Issue Details Report Against Candidate :-");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(Session["printData"].ToString().Trim());
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
        }
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=REPORT_HostIssueDetails" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
        }
    }
}