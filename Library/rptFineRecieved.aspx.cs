using ASP;
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

public partial class Library_rptFineRecieved : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            btnExport.Enabled = false;
            btnPrint.Enabled = false;
            Session["title"] = Page.Title.ToString();
            Form.DefaultButton = btnSearch.UniqueID;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        generateReport();
    }

    private void generateReport()
    {
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder = new StringBuilder();
        try
        {
            DataTable dataForReport = getDataForReport();
            if (dataForReport.Rows.Count > 0)
            {
                stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='3'  width='100%' style='border:solid 1px black;border-bottom:0px;' class='tbltxt'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' width='300px'><font color='Black'>" + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt") + "</font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td style='font-size:14'>Fine Received For ");
                if (drpMemType.SelectedValue == "1")
                    stringBuilder.Append("<span style='font-weight:bold'>'Staff'</span>");
                else if (drpMemType.SelectedValue == "2")
                    stringBuilder.Append("<span style='font-weight:bold'>'Student'</span>");
                else
                    stringBuilder.Append("<span style='font-weight:bold'>'All'</span>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' ><font color='Black'>No Of Records: " + dataForReport.Rows.Count.ToString() + "</font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("<table width='100%' border='1' cellspacing='0' cellpadding='0' class='tbltxt'>");
                stringBuilder.Append("<tr style='font-weight:bold;  background-color: silver;'>");
                stringBuilder.Append("<td width='50px'>Sl No.</td>");
                stringBuilder.Append("<td width='150px'>Member Name</td>");
                stringBuilder.Append("<td width='150px' align='right'>Total Fine Paid</td>");
                stringBuilder.Append("</tr>");
                double num1 = 0.0;
                int num2 = 1;
                foreach (DataRow row in (InternalDataCollectionBase)dataForReport.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td>" + num2.ToString() + "</td>");
                    stringBuilder.Append("<td>" + row["MemberName"].ToString() + "</td>");
                    stringBuilder.Append("<td align='right'>");
                    string str1 = "<a href='javascript:popUp";
                    string str2 = "('rptMemwiseFineRecieved.aspx?Id=" + row["LibMemberId"].ToString() + "')";
                    string str3 = "'>";
                    stringBuilder.Append(str1.Replace('\'', '"') + str2 + str3.Replace('\'', '"'));
                    stringBuilder.Append("<b>" + row["FinePaid"].ToString() + "</b></a>");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                    if (row["FinePaid"].ToString().Trim() != "")
                        num1 += Convert.ToDouble(row["FinePaid"].ToString().Trim());
                    ++num2;
                }
                stringBuilder.Append("<tr style='font-weight:bold;  background-color: silver;'>");
                stringBuilder.Append("<td align='right'>&nbsp;");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right'>");
                stringBuilder.Append("<strong>Total Amount </strong>&nbsp;");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right'><strong>" + string.Format("{0:F2}", num1) + "</strong>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                lblReport.Text = stringBuilder.ToString();
                Session["rptFineRecieved"] = Regex.Replace(stringBuilder.ToString(), "<a\\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
                btnPrint.Enabled = true;
                btnExport.Enabled = true;
            }
            else
            {
                btnPrint.Enabled = false;
                btnExport.Enabled = false;
                lblReport.Text = "<div style='font-weight:bold;  background-color: silver; width: 100%; border: solid 2px #666; text-align:center;'>No Data Found !</div>";
                Session["rptFineRecieved"] = null;
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private DataTable getDataForReport()
    {
        DataTable dataTable = new DataTable();
        Hashtable ht = new Hashtable();
        if (drpMemType.SelectedIndex > 0)
            ht.Add("@MemberType", drpMemType.SelectedValue.ToString());
        ht.Add("@CollegeId", Session["SchoolId"]);
        return obj.GetDataTable("Lib_SP_GetFineRecieve", ht);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("<tr><td colspan='4'>Report : Fine Received Details </td></tr>");
                stringBuilder.Append("<tr><td></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["rptFineRecieved"].ToString()).ToString().Trim());
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
            string str = Server.MapPath("Exported_Files/" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Fine Received Details" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptFineRecievedPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}