using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Inventory_rptROLItems : System.Web.UI.Page
{
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
            GenerateReport();
        else
            Response.Redirect("../Login.aspx");
    }

    private void GenerateReport()
    {
        DataTable dataTable1 = new DataTable();
        Hashtable ht = new Hashtable();
        Common common = new Common();
        if (Session["User"].ToString() != "admin")
            ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
        DataTable dataTable2 = common.GetDataTable("SI_GetROLItems", ht);
        if (dataTable2.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table width='100%' border='0' cellpadding='2' cellspacing='0' style='border:solid 1px black;border-bottom:0px; font-size:14px;' >");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' ><font color='Black'>" + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt") + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td colspan='2' align='right' style='width: 350px'><font color='Black'>No Of Records: " + dataTable2.Rows.Count.ToString() + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table width='100%' border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black; font-size:14px;' >");
            stringBuilder.Append("<tr style='background-color: #D1CCCC'>");
            stringBuilder.Append("<td  align='left' ><b>Item Name</b></td>");
            stringBuilder.Append("<td style='width: 150px' align='right'><b>ROL</b></td>");
            stringBuilder.Append("<td style='width: 100px' align='right'><b>Quantity</b></td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' >");
                stringBuilder.Append(row["ItemName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' >");
                stringBuilder.Append(row["ROL"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' >");
                stringBuilder.Append(row["quantity"].ToString().Trim());
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["ROL"] = stringBuilder.ToString().Trim();
            btnPrint.Enabled = true;
            btnExport.Enabled = true;
        }
        else
        {
            Session["ROL"] = (lblReport.Text = "<div style='border:solid 1px Black; padding:5px 0 5px 20px'>No records found !</div>");
            btnPrint.Enabled = false;
            btnExport.Enabled = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptROLItemsPrint.aspx");
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
            Response.AddHeader("content-disposition", "attachment;filename=REPORT_ItemOnROL" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td align='left' colspan='14'><b>");
                stringBuilder.Append("Item on ROL :-");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(lblReport.Text.ToString().Trim());
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
}