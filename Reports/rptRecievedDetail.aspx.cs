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

public partial class Reports_rptRecievedDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Request.QueryString["Dt"] == null)
            return;
        PaidDetail();
    }

    private void PaidDetail()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Dt", Request.QueryString["Dt"].ToString());
        hashtable.Add("@Pmt", Request.QueryString["PMode"].ToString());
        if (Request.QueryString["UId"] != null)
            hashtable.Add("@UId", Request.QueryString["UId"].ToString().Trim());
        DataTable dataTable2 = clsDal.GetDataTable("ps_sp_get_ReceivedDtl", hashtable);
        if (dataTable2.Rows.Count > 0)
            GenerateReport(dataTable2);
        else
            lblReport.Text = string.Empty;
    }

    private void GenerateReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
        stringBuilder.Append("<tr  style='background-color:#CCCCCC'>");
        stringBuilder.Append("<td style='width: 30px' align='left'><strong>Sl.No</strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='left'>Received Date</td>");
        stringBuilder.Append("<td style='width: 60px' align='left'>Receipt No</td>");
        stringBuilder.Append("<td style='width: 80px' align='left'>Admission No</td>");
        stringBuilder.Append("<td align='left'>Student Name</td>");
        stringBuilder.Append("<td style='width: 60px' align='left'>Class</td>");
        stringBuilder.Append("<td align='left'>Description</td>");
        stringBuilder.Append("<td style='width: 100px' align='right'>Amount</td>");
        stringBuilder.Append("</tr>");
        double num1 = 0.0;
        int num2 = 1;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center'>" + num2.ToString() + "</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["TransDtStr"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["InvoiceReceiptNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            if (row["PartyId"].ToString().Trim() == "" || row["PartyId"].ToString().Trim() == "0" || Convert.ToInt64(row["PartyId"].ToString().Trim()) < 1000000000L)
            {
                stringBuilder.Append("<td align='left'>&nbsp;");
                stringBuilder.Append("</td>");
            }
            else
            {
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["PartyId"].ToString().Trim());
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("<td align='left'  style='white-space:nowrap;'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["ClassName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Description"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right'>");
            stringBuilder.Append(row["Amount"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            if (row["Amount"].ToString() != "")
                num1 += Convert.ToDouble(row["Amount"].ToString().Trim());
            ++num2;
        }
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='7'  align='right'>");
        stringBuilder.Append("<strong>Grand Total &nbsp;:&nbsp;</strong>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='right'>");
        stringBuilder.Append("<strong>" + string.Format("{0:F2}", num1));
        stringBuilder.Append("</strong></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["FeeRecvDtl"] = stringBuilder.ToString().Trim();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptRecievedDetailPrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
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
                stringBuilder.Append("Collection Details : ");
                stringBuilder.Append("</b></td></tr>");
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

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/Cash recieved details" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
}