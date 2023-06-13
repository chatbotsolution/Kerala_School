using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptProspectusStock : System.Web.UI.Page
{
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        fillSession();
        drpSession.Items.Insert(0, new ListItem("All", "All"));
        ViewState["totAmt"] = string.Empty;
        btnPrint.Enabled = false;
        btnExport.Enabled = false;
    }

    private void fillSession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    private void GenerateReport()
    {
        if (drpSession.SelectedIndex > 0)
            ht.Add("@SessionYr", drpSession.SelectedValue);
        if (txtFromDt.Text != "")
            ht.Add("@FromDate", dtpPros.GetDateValue());
        if (txtToDt.Text != "")
            ht.Add("@Todate", dtpPros1.GetDateValue());
        dt = obj.GetDataTable("[Sp_RptProspectusStock]", ht);
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            Decimal num = new Decimal(0);
            stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'  class='tbltxt'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='2' align='left'><font color='Black'>" + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt") + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td colspan='2' align='right'><font color='Black'>No Of Records: " + dt.Rows.Count.ToString() + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 120px' align='left' ><b>Session Year</b></td>");
            stringBuilder.Append("<td style='width: 120px' align='left'><b>Transction Date</b></td>");
            stringBuilder.Append("<td style='width: 90px' align='left'><b>Prospectus Type</b></td>");
            stringBuilder.Append("<td align='Right'><b>Unit Sale Price</b></td>");
            stringBuilder.Append("<td align='Right'><b>Quantity In</b></td>");
            stringBuilder.Append("<td align='Right'><b>Stock Value</b></td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["SessionYr"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["TransDateStr"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                if (row["ForClass"].ToString().Trim() != "")
                    stringBuilder.Append(row["ForClass"].ToString().Trim());
                else
                    stringBuilder.Append("ALL");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='Right'>");
                if (row["Price"].ToString().Trim() != "")
                    stringBuilder.Append(row["Price"].ToString().Trim());
                else
                    stringBuilder.Append("N/A");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='Right'>");
                if (row["QtyIn"].ToString().Trim() != "")
                    stringBuilder.Append(row["QtyIn"].ToString().Trim());
                else
                    stringBuilder.Append("&nbsp;");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='Right'>");
                if (row["QtyIn"].ToString().Trim() == "" || row["Price"].ToString().Trim() == "")
                {
                    stringBuilder.Append("N/A");
                }
                else
                {
                    stringBuilder.Append(Convert.ToDecimal(row["QtyIn"].ToString().Trim()) * Convert.ToDecimal(row["Price"].ToString().Trim()));
                    num += Convert.ToDecimal(row["QtyIn"].ToString().Trim()) * Convert.ToDecimal(row["Price"].ToString().Trim());
                }
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            string str;
            ViewState["totAmt"] = (str = dt.Compute("SUM(QtyIn)", "").ToString());
            stringBuilder.Append("<tr style='font-weight: bold; text-align: left;'>");
            stringBuilder.Append("<td align='Left' style='Font-Size:14px;'><b>Grand Total :</b></td>");
            stringBuilder.Append("<td align='right'></td>");
            stringBuilder.Append("<td align='right' colspan='3'><b>" + str.ToString() + "</b></td>");
            stringBuilder.Append("<td align='right'><b>" + num.ToString("0.00") + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table></center></fieldset>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["printDataStock"] = stringBuilder.ToString().Trim();
            btnPrint.Enabled = true;
            btnExport.Enabled = true;
        }
        else
        {
            Session["printDataStock"] = (lblReport.Text = "<div style='border:solid 1px Black'>No records found !</div>");
            btnPrint.Enabled = false;
            btnExport.Enabled = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptProspectusStockPrint.aspx");
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
            Response.AddHeader("content-disposition", "attachment;filename=REPORT_ProspectusStock" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
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
                stringBuilder.Append("Prospectus Stock Report :-");
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