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

public partial class Inventory_ViewPurchaseDetails : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User"] == null)
           Response.Redirect("../Login.aspx");
        if (this.Page.IsPostBack ||Request.QueryString["Id"] == null)
            return;
       GenerateReport();
    }

    private void GenerateReport()
    {
       ht.Clear();
       ht.Add("@PurchaseId", this.Request.QueryString["Id"].ToString());
       ht.Add("@SchoolId", Convert.ToInt32(this.Session["SchoolId"]));
       dt =obj.GetDataTable("SI_rptViewPurchaseDetails",ht);
        if (this.dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1' cellpadding='5' cellspacing='0' width='100%'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='3' class='tblheader'><div style='float:left; width=200px;'><b>Purchase Date : " +dt.Rows[0]["PurDate"] + "</b></div>");
        stringBuilder.Append("<div style='float:right;'><b>Inv No : " +dt.Rows[0]["InvNo"] + "</b></div></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='3' align='left' class='tblheader'><div style='float:left; width=500px;'><b>Supplier Name : " +dt.Rows[0]["SupplierName"] + "</b></div>");
        stringBuilder.Append("<div style='float:right;'><b>ContactNo : " +dt.Rows[0]["SupplierContactNo"] + "</b></div></td>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<div>&nbsp;</div>");
        stringBuilder.Append("<table border='1' cellpadding='5' cellspacing='0' width='100%'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='left' class='tblheader'><b>Item Name</b></td>");
        stringBuilder.Append("<td style='width: 90px' align='right' class='tblheader'><b>Quantity</b></td>");
        stringBuilder.Append("<td style='width: 120px' align='right' class='tblheader'><b>Unit Price</b></td>");
        stringBuilder.Append("</tr>");
        double num = 0.0;
        for (int index = 0; index <dt.Rows.Count; ++index)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='gridtxt'>" +dt.Rows[index]["ItemName"] + "</td>");
            stringBuilder.Append("<td style='width: 120px' align='right' class='gridtxt'>" +dt.Rows[index]["Qty"] + "</td>");
            stringBuilder.Append("<td style='width: 90px' align='right' class='gridtxt'>" +dt.Rows[index]["UnitPrice"] + "</td>");
            stringBuilder.Append("</tr>");
            num += double.Parse(this.dt.Rows[index]["Qty"].ToString()) * double.Parse(this.dt.Rows[index]["UnitPrice"].ToString());
        }
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='right' colspan='3' class='gridtxt'><b>Total : " + string.Format("{0:f2}", num) + "</b></td>");
        stringBuilder.Append("</table>");
       lblReport.Text = stringBuilder.ToString().Trim();
       Session["printData"] = stringBuilder.ToString();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
       Response.Redirect("rptPurchaseDetailsPrint.aspx");
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.lblReport.Text.ToString().Trim() != "")
               ExportToExcel(this.Session["printData"].ToString().Trim());
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
           Response.AddHeader("content-disposition", "attachment;filename=Purchase Item Detail" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
           Response.WriteFile(str);
           Response.End();
        }
        catch (Exception ex)
        {
        }
    }
}