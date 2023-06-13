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

public partial class Library_rptMemwiseFineRecieved : System.Web.UI.Page
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
                stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='3' align='left' class='gridtext'><font color='Black'>" + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt") + "</font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' class='gridtext' colspan='2'><font color='Black'>No Of Records: " + dataForReport.Rows.Count.ToString() + "</font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("<table width='100%' border='1' cellspacing='0' cellpadding='0'>");
                stringBuilder.Append("<tr style='font-weight:bold;  background-color: silver;'>");
                stringBuilder.Append("<td class='table-header' style='text-align:center; font-weight:bold;' colspan='5'>Report on Fine Recieved</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr style='font-weight:bold;'>");
                stringBuilder.Append("<td width='50px'>Sl No</td>");
                stringBuilder.Append("<td width='100px'>Fine Date</td>");
                stringBuilder.Append("<td width='150px'>MR_Number</td>");
                stringBuilder.Append("<td>Narration</td>");
                stringBuilder.Append("<td width='100px' align='right'> Fine Paid</td>");
                stringBuilder.Append("</tr>");
                double num1 = 0.0;
                int num2 = 1;
                foreach (DataRow row in (InternalDataCollectionBase)dataForReport.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td>" + num2.ToString() + "</td>");
                    stringBuilder.Append("<td>" + row["FinePaidDt"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + row["MR_No"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + row["NarrationStr"].ToString() + "</td>");
                    stringBuilder.Append("<td align='right'>" + row["Credit"].ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                    if (row["Credit"].ToString().Trim() != "")
                        num1 += Convert.ToDouble(row["Credit"].ToString().Trim());
                    ++num2;
                }
                stringBuilder.Append("<tr style='font-weight:bold;  background-color: silver;'>");
                stringBuilder.Append("<td colspan='4' align='right'>");
                stringBuilder.Append("<strong>Total Amount</strong>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right'><strong>" + string.Format("{0:F2}", num1) + "</strong>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                lblReport.Text = stringBuilder.ToString();
                Session["rptMemwiseFineRecieved"] = stringBuilder.ToString();
                btnPrint.Visible = true;
                btnExport.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
                btnExport.Visible = false;
                lblReport.Text = "<div style='font-weight:bold;  background-color: silver; width: 550px; border: solid 2px #666; text-align:center;'>No Data Found !</div>";
                Session["rptMemwiseFineRecieved"] = null;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private DataTable getDataForReport()
    {
        DataTable dataTable = new DataTable();
        return obj.GetDataTable("Lib_SP_GetFineRecieveIndiv", new Hashtable()
    {
      {
         "@MemberId",
         int.Parse(Request.QueryString["Id"].ToString())
      },
      {
         "@CollegeId",
        Session["SchoolId"]
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
                stringBuilder.Append("<tr><td></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px' colspan='4'>Report: Fine Received </td></tr>");
                stringBuilder.Append("<tr><td></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["rptMemwiseFineRecieved"].ToString()).ToString().Trim());
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
            Response.AddHeader("content-disposition", "attachment;filename=Fine Received" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
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
            Response.Redirect("rptMemwiseFineRecievedPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}