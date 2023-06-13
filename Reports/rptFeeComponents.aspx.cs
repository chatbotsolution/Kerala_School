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

public partial class Reports_rptFeeComponents : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        GetReportTable();
    }

    protected void GetReportTable()
    {
        createComponentReport(new Common().GetDataTable("ps_sp_Rpt_FeeComponents", new Hashtable()));
    }

    private void createComponentReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1' width='100%' style='background-color:#CCC;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 50px' class='tblheader'>Fee Id</td>");
        stringBuilder.Append("<td class='tblheader'>Fee Type</td>");
        stringBuilder.Append("<td class='tblheader'>Periodicity Type</td>");
        stringBuilder.Append("<td style='width: 120px; text-align:center;' class='tblheader'>Fine Applicable</td>");
        stringBuilder.Append("<td style='width: 150px; text-align:center;' class='tblheader'>Concession Applicable</td>");
        stringBuilder.Append("<td style='width: 100px; text-align:center;' class='tblheader'>Refundable</td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td class='tbltd'>");
            stringBuilder.Append(row["FeeID"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td class='tbltd'>");
            stringBuilder.Append(row["FeeName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td class='tbltd'>");
            stringBuilder.Append(row["PeriodicityType"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td class='tbltd'  align='center'>");
            stringBuilder.Append(row["FineApplicable"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td class='tbltd'  align='center'>");
            stringBuilder.Append(row["ConcessionApplicable"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td class='tbltd' align='center'>");
            stringBuilder.Append(row["Refundable"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["printData"] = stringBuilder.ToString().Trim();
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("<tr><td align='left'><b>");
                stringBuilder.Append("Fee Components:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + lblReport.Text.ToString().Trim()).ToString().Trim());
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
            Response.AddHeader("content-disposition", "attachment;filename=" + str);
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
            Response.Redirect("rptFeeComponentsPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}