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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Library_rptDtwiseBookPurchase : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();

    protected void Page_Load(object sender, EventArgs e)
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
                stringBuilder.Append("<table width='100%' border='1' cellspacing='0' cellpadding='0'>");
                stringBuilder.Append("<tr style='font-weight:bold;  background-color: silver;'>");
                stringBuilder.Append("<td class='table-header' style='text-align:center; font-weight:bold;' colspan='7'>Purchase Details on : " + dataForReport.Rows[0]["PurchaseDateStr"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr style='font-weight:bold;'>");
                stringBuilder.Append("<td width='50px'>Sl No.</td>");
                stringBuilder.Append("<td width='150px'>AccessionNo</td>");
                stringBuilder.Append("<td width='150px'>Category</td>");
                stringBuilder.Append("<td>Book Title</td>");
                stringBuilder.Append("<td width='150px'>Author Name</td>");
                stringBuilder.Append("<td width='100px'>BillNo</td>");
                stringBuilder.Append("<td width='100px' align='right'> Price</td>");
                stringBuilder.Append("</tr>");
                double num1 = 0.0;
                int num2 = 1;
                foreach (DataRow row in (InternalDataCollectionBase)dataForReport.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td>" + num2.ToString() + "</td>");
                    stringBuilder.Append("<td>" + row["AccessionNo"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + row["BriefName"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + row["BookTitle"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + row["AuthorName1"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + row["BillN"].ToString() + "</td>");
                    stringBuilder.Append("<td align='right'>" + row["Price"].ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                    if (row["Price"].ToString().Trim() != "")
                        num1 += Convert.ToDouble(row["Price"].ToString().Trim());
                    ++num2;
                }
                stringBuilder.Append("<tr style='font-weight:bold;  background-color: silver;'>");
                stringBuilder.Append("<td colspan='6' align='right'>");
                stringBuilder.Append("<strong>Total Amount</strong>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right'><strong>" + string.Format("{0:F2}", num1) + "</strong>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                lblReport.Text = stringBuilder.ToString();
            }
            else
                lblReport.Text = "No Data Found";
            if (stringBuilder.ToString() != "")
                Session["rptDtwiseBookPurchase"] = stringBuilder.ToString();
            else
                Session["rptDtwiseBookPurchase"] = null;
        }
        catch (Exception ex)
        {
        }
    }

    private DataTable getDataForReport()
    {
        DataTable dataTable = new DataTable();
        return obj.GetDataTable("Lib_GetBooksPerDate", new Hashtable()
    {
      {
         "@PurchaseDate",
         DateTime.Parse(Request.QueryString["Id"].ToString())
      }
    });
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
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["rptDtwiseBookPurchase"].ToString()).ToString().Trim());
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
            Response.AddHeader("content-disposition", "attachment;filename=DateWiseBookPurchase" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
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
            Response.Redirect("rptDtwiseBookPurchasePrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}