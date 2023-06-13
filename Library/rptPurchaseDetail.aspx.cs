using ASP;
using RJS.Web.WebControl;
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

public partial class Library_rptPurchaseDetail : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        btnExport.Enabled = false;
        btnPrint.Enabled = false;
        Session["title"] = Page.Title.ToString();
        Form.DefaultButton = btnSearch.UniqueID;
        if (Session["User_Id"] != null)
            return;
        Response.Redirect("../Login.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        generateReport();
    }

    private void generateReport()
    {
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        try
        {
            DataTable dataForReport = getDataForReport();
            if (dataForReport.Rows.Count > 0)
            {
                StringBuilder stringBuilder3 = new StringBuilder();
                stringBuilder3.Append("<table width='100%' border='1px' cellspacing='0' cellpadding='2px' class='tbltxt'>");
                stringBuilder3.Append("<tr>");
                stringBuilder3.Append("<td style='text-align:center; font-size:large; font-weight:bold' colspan='5'>Purchase Detail Report</td>");
                stringBuilder3.Append("</tr>");
                stringBuilder3.Append("<tr style='font-weight:bold; text-align:left'>");
                stringBuilder3.Append("<td>Sl No.</td>");
                stringBuilder3.Append("<td>Book Title.</td>");
                stringBuilder3.Append("<td>Book SubTitle.</td>");
                stringBuilder3.Append("<td>Purchase date</td>");
                stringBuilder3.Append("<td>No. of Books</td>");
                stringBuilder3.Append("<td align='right'>Amount</td>");
                stringBuilder1.Append(stringBuilder3.ToString());
                stringBuilder2.Append(stringBuilder3.ToString());
                stringBuilder1.Append("<td align='right'>Details</td>");
                StringBuilder stringBuilder4 = new StringBuilder();
                stringBuilder4.Append("</tr>");
                stringBuilder1.Append(stringBuilder4.ToString());
                stringBuilder2.Append(stringBuilder4.ToString());
                double num1 = 0.0;
                int num2 = 1;
                foreach (DataRow row in (InternalDataCollectionBase)dataForReport.Rows)
                {
                    StringBuilder stringBuilder5 = new StringBuilder();
                    stringBuilder5.Append("<tr style='text-align:left'>");
                    stringBuilder5.Append("<td>" + num2.ToString() + "</td>");
                    stringBuilder5.Append("<td>" + row["BookTitle"].ToString() + "</td>");
                    stringBuilder5.Append("<td>" + row["bookSubTitle"].ToString() + "</td>");
                    stringBuilder5.Append("<td>" + row["PurchaseDateStr"].ToString() + "</td>");
                    stringBuilder5.Append("<td>" + row["NoOfBook"].ToString() + "</td>");
                    stringBuilder5.Append("<td align='right'>" + row["Price"].ToString() + "</td>");
                    stringBuilder1.Append(stringBuilder5.ToString());
                    stringBuilder2.Append(stringBuilder5.ToString());
                    stringBuilder1.Append("<td align='right' >");
                    string str1 = "<a href='javascript:popUp";
                    string str2 = "('rptDtwiseBookPurchase.aspx?Id=" + row["PurchaseDate"].ToString() + "')";
                    string str3 = "'>";
                    stringBuilder1.Append(str1.Replace('\'', '"') + str2 + str3.Replace('\'', '"'));
                    stringBuilder1.Append("<b>View Details</b></a>");
                    stringBuilder1.Append("</td>");
                    StringBuilder stringBuilder6 = new StringBuilder();
                    stringBuilder6.Append("</tr>");
                    stringBuilder1.Append(stringBuilder6.ToString());
                    stringBuilder2.Append(stringBuilder6.ToString());
                    if (row["Price"].ToString().Trim() != "")
                        num1 += Convert.ToDouble(row["Price"].ToString().Trim());
                    ++num2;
                }
                StringBuilder stringBuilder7 = new StringBuilder();
                stringBuilder7.Append("<tr>");
                stringBuilder7.Append("<td colspan='3' align='right'>");
                stringBuilder7.Append("<strong>Total Amount</strong>");
                stringBuilder7.Append("</td>");
                stringBuilder7.Append("<td align='right'><strong>" + string.Format("{0:F2}", num1) + "</strong>");
                stringBuilder7.Append("</td>");
                stringBuilder1.Append(stringBuilder7.ToString());
                stringBuilder2.Append(stringBuilder7.ToString());
                stringBuilder1.Append("<td>&nbsp;");
                stringBuilder1.Append("</td>");
                new StringBuilder().Append("</tr>");
                StringBuilder stringBuilder8 = new StringBuilder();
                stringBuilder8.Append("</table>");
                stringBuilder1.Append(stringBuilder8.ToString());
                stringBuilder2.Append(stringBuilder8.ToString());
                lblReport.Text = stringBuilder1.ToString();
                btnPrint.Enabled = true;
                btnExport.Enabled = true;
            }
            else
            {
                btnPrint.Enabled = false;
                btnExport.Enabled = false;
                lblReport.Text = "<center>No Data Found</center>";
            }
            if (stringBuilder1.ToString() != "")
                Session["rptPurchaseDetail"] = stringBuilder2.ToString();
            else
                Session["rptPurchaseDetail"] = null;
        }
        catch (Exception ex)
        {
        }
    }

    private DataTable getDataForReport()
    {
        DataTable dataTable = new DataTable();
        Hashtable ht = new Hashtable();
        if (txtFromDate.Text.Trim() != "")
            ht.Add("@FrmDt", dtpFromDate.GetDateValue().ToString());
        if (txtToDate.Text.Trim() != "")
            ht.Add("@ToDt", dtpToDate.GetDateValue().ToString());
        ht.Add("@CollegeId", Session["SchoolId"]);
        return obj.GetDataTable("Lib_GetPurchaseDetail", ht);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["rptPurchaseDetail"] != null)
                ExportToExcel(Session["rptPurchaseDetail"].ToString());
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
            Response.AddHeader("content-disposition", "attachment;filename=Purchase Details" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
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
            Response.Redirect("rptPurchaseDetailPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}