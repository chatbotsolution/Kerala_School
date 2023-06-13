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

public partial class Hostel_rptHostCancelledRcpt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Request.QueryString["Dt"] == null)
            return;
        dtpToDt.SetDateValue(Convert.ToDateTime(Request.QueryString["Dt"].ToString()));
        dtpFrmDt.SetDateValue(Convert.ToDateTime(Request.QueryString["Dt"].ToString()));
        GenerateReport();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    private void GenerateReport()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (txtFromDt.Text != "")
            hashtable.Add("@FrmDt", dtpFrmDt.GetDateValue().ToString("dd MMM yyyy"));
        if (txtToDt.Text != "")
            hashtable.Add("@ToDt", dtpToDt.GetDateValue().ToString("dd MMM yyyy"));
        CreateReportNew(clsDal.GetDataTable("HostRptGetCancelRcptNew", hashtable));
    }

    private void CreateReportNew(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
            stringBuilder.Append("<tr style='background-color:#CCCCCC'><td colspan='8' align='center'><b>Cancelled Receipt From " + dtpFrmDt.GetDateValue().ToString("dd MMM yyyy") + " To " + dtpToDt.GetDateValue().ToString("dd MMM yyyy") + "</b></td></tr>");
            stringBuilder.Append("<tr  style='background-color:#CCCCCC'>");
            stringBuilder.Append("<td style='width: 30px' align='left'><strong>Sl.No</strong></td>");
            stringBuilder.Append("<td style='width: 100px' align='left'>Cancelled Date</td>");
            stringBuilder.Append("<td style='width: 100px' align='left'>Details / Cancel Reason</td>");
            stringBuilder.Append("<td style='width: 100px' align='left'>Transaction Date</td>");
            stringBuilder.Append("<td style='width: 120px' align='left'>Receipt No</td>");
            stringBuilder.Append("<td style='width: 120px' align='left'>Received From</td>");
            stringBuilder.Append("<td align='left'>Description</td>");
            stringBuilder.Append("<td style='width: 100px' align='right'>Amount</td>");
            stringBuilder.Append("</tr>");
            int num1 = 1;
            double num2 = 0.0;
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='center'>" + num1.ToString() + "</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["UserDatestr"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["Particulars"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["TransDtStr"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["InvoiceReceiptNo"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["RcvdFrom"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["Description"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right'>");
                stringBuilder.Append(row["AmountStr"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                num2 += Convert.ToDouble(row["AmountStr"].ToString().Trim());
                ++num1;
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='7'  align='right'>");
            stringBuilder.Append("<strong class='error'>Grand Total &nbsp;:&nbsp;</strong>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right'>");
            stringBuilder.Append("<strong class='error'>" + string.Format("{0:F2}", num2));
            stringBuilder.Append("</strong></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["PrntCancel"] = stringBuilder.ToString();
            btnPrint.Enabled = true;
            btnExp.Enabled = true;
        }
        else
        {
            lblReport.Text = "No Records Found !";
            btnPrint.Enabled = false;
            btnExp.Enabled = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptHostCancelledRcptPrint.aspx");
    }

    protected void btnExp_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("<tr><td align='left'><b>");
                stringBuilder.Append("Cancelled Receipt:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["PrntCancel"].ToString()).ToString().Trim());
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
            string str = Server.MapPath("Exported_Files/HostelCancelledReceipt" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=HostelCancelledReceipt" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}