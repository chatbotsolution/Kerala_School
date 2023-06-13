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

public partial class Reports_rptStudFineLedger : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void FillGrid()
    {
        grdDefaultersReport.DataSource = new Common().GetDataTable("ps_sp_get_DefaultersReport", new Hashtable());
        grdDefaultersReport.DataBind();
    }

    private void createRemarkReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='0' Border='' width='100%'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='WIDTH: 25%' align='left'>");
        stringBuilder.Append("<strong>Student Admission No:</strong>" + dt.Rows[0]["AdmnNo"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td style='WIDTH: 75%' align='left' colspan='2'>");
        stringBuilder.Append("<strong>Class:</strong>" + dt.Rows[0]["ClassName"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td style='WIDTH: 75%' align='left' colspan='2'>");
        stringBuilder.Append("<strong>Roll No:</strong>" + dt.Rows[0]["RollNo"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table cellpadding='1' cellspacing='0' Border='' width='100%'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Due Date</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Fee type</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Amount</u></strong></td>");
        stringBuilder.Append("</tr>");
        double num = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["TransDate"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["TransDesc"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["TransAmount"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            num += Convert.ToDouble(row["TransAmount"].ToString().Trim());
        }
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='3' align='right'>");
        stringBuilder.Append("<strong>Total:</strong> " + num.ToString());
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["printData"] = stringBuilder.ToString().Trim();
    }

    protected void grdDefaultersReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        lblReport.Visible = false;
        createRemarkReport(new Common().GetDataTable("ps_sp_get_DefaultersReportTable", new Hashtable()));
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
                stringBuilder.Append("Fine Ledger:-");
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
            Response.Redirect("rptStudFineLedgerPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}