
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

public partial class Inventory_rptPurchaseDetails : System.Web.UI.Page
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
        Page.Form.DefaultButton = btnShow.UniqueID;
        btnPrint.Enabled = false;
        btnExpExcel.Visible = true;
        btnExpExcel.Enabled = false;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    private void GenerateReport()
    {
        if (txtFromDate.Text != "")
            ht.Add("@FromDate", pcalFromDate.GetDateValue());
        if (txtToDate.Text != "")
            ht.Add("@Todate", pcalToDate.GetDateValue());
        ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
        dt = obj.GetDataTable("SI_rptPurchaseDetails", ht);
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='6' align='right' class='gridtxt'><font color='Black'><strong>No Of Records: " + dt.Rows.Count.ToString() + "</strong></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0'  width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 90px' align='left' class='tblheader'><b>Purchase Date</b></td>");
            stringBuilder.Append("<td style='align='left' class='tblheader'><b>Supplier Name</b></td>");
            stringBuilder.Append("<td style='align='left' class='tblheader'><b>Address</b></td>");
            stringBuilder.Append("<td style='width: 150px' align='Left' class='tblheader'><b>ContactNo</b></td>");
            stringBuilder.Append("<td style='width: 90px' align='Left' class='tblheader'><b>Source of acquiring</b></td>");
            stringBuilder.Append("<td style='width: 120px' align='Left' class='tblheader'><b>Total Amount</b></td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                stringBuilder.Append(row["PurchaseDateStr"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                stringBuilder.Append(row["SupplierName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                stringBuilder.Append(row["SupplierAddress"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                stringBuilder.Append(row["SupplierContactNo"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                stringBuilder.Append(row["sources"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                string str1 = "<a href='javascript:popUp";
                string str2 = "('ViewPurchaseDetails.aspx?Id=" + row["PurchaseId"].ToString() + "')";
                string str3 = "'>";
                stringBuilder.Append(str1.Replace('\'', '"') + str2 + str3.Replace('\'', '"'));
                stringBuilder.Append("<b>" + row["totalamnt"].ToString() + "</b></a>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table></center></fieldset>");
            Session["printData"] = Regex.Replace(stringBuilder.ToString(), "<a\\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
            lblReport.Text = stringBuilder.ToString().Trim();
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
        Response.Redirect("rptPurchaseDetailsPrint.aspx");
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
                stringBuilder.Append("Purchase Details Report :-");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(Session["printData"].ToString());
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
            Response.AddHeader("content-disposition", "attachment;filename=REPORT_PurchaseDetails" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}