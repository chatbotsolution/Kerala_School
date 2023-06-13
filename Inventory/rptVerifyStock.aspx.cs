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

public partial class Inventory_rptVerifyStock : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
           Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
       Page.Form.DefaultButton =btnShow.UniqueID;
       btnPrint.Enabled = false;
       btnExpExcel.Visible = true;
       btnExpExcel.Enabled = false;
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
       ht.Clear();
        if (txtFromDate.Text != "")
            ht.Add("@FromDate", pcalFromDate.GetDateValue());
        if (txtToDate.Text != "")
            ht.Add("@Todate", pcalToDate.GetDateValue());
        if (Session["User"].ToString() != "admin")
           ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
       dt =obj.GetDataTable("SI_rptVerifyStock",ht);
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='6' style='font-size:16; font-weight:bold; text-align:center;'>Verify Stock Report</td>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='6' align='right' class='gridtext'><font color='Black'><strong>No Of Records: " +dt.Rows.Count.ToString() + "</strong></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0'  width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='tblheader'><b>Item Name</b></td>");
            stringBuilder.Append("<td align='left' class='tblheader'><b>Physical Stock</b></td>");
            stringBuilder.Append("<td align='left' class='tblheader'><b>Transaction Stock</b></td>");
            stringBuilder.Append("<td align='left' class='tblheader'><b>Verified Dt</b></td>");
            stringBuilder.Append("<td align='left' class='tblheader'><b>Verified By</b></td>");
            stringBuilder.Append("<td align='left' class='tblheader'><b>Remarks</b></td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                stringBuilder.Append(row["ItemName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                stringBuilder.Append(row["PhysicalStock"].ToString().Trim());
                stringBuilder.Append(" " + row["MesuringUnit"].ToString() + " ");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                stringBuilder.Append(row["TransactionStock"].ToString().Trim());
                stringBuilder.Append(" " + row["MesuringUnit"].ToString() + " ");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                stringBuilder.Append(row["VerifiedDate"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                stringBuilder.Append(row["VerifiedBy"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                stringBuilder.Append(row["Remarks"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
           lblReport.Text = stringBuilder.ToString().Trim();
           Session["printData"] = Regex.Replace(stringBuilder.ToString(), "<a\\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
           btnExpExcel.Visible = true;
           btnPrint.Enabled = true;
           btnExpExcel.Enabled = true;
        }
        else
        {
           Session["printData"] = (lblReport.Text = "<div style='border:solid 1px Black'>No records found !</div>");
           btnPrint.Enabled = false;
           btnExpExcel.Enabled = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
       Response.Redirect("rptIssueDetailsPrint.aspx");
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
               ExportToExcel(Session["printData"].ToString().Trim());
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
            string str =Server.MapPath("Exported_Files/" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
           Response.ClearContent();
           Response.ContentType = "application/vnd.ms-excel";
           Response.AddHeader("content-disposition", "attachment;filename=REPORT_StockVerify" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
           Response.WriteFile(str);
           Response.End();
        }
        catch (Exception ex)
        {
        }
    }
}