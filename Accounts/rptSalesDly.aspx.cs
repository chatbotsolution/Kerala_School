using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_rptSalesDly : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        btnPrint.Visible = false;
        btnExcel.Visible = false;
        dtptodt.SetDateValue(DateTime.Now);
        dtpfromdt.SetDateValue(DateTime.Now);
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        GetReportTable();
    }

    private void GetReportTable()
    {
        clsDAL clsDal = new clsDAL();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        if (txtFromDt.Text != "")
            hashtable.Add("@FromDt", (dtpfromdt.GetDateValue().ToString("dd-MMM-yyyy") + " 00:00:00'"));
        if (txtToDt.Text != "")
            hashtable.Add("@ToDt", (dtptodt.GetDateValue().ToString("dd-MMM-yyyy") + " 23:59:59'"));
        hashtable.Add("@ShopId", Session["SchoolId"]);
        DataSet dataSet2 = clsDal.GetDataSet("ACTS_GetDailySalesRpt", hashtable);
        if (dataSet2.Tables[0].Rows.Count > 0)
        {
            btnExcel.Visible = true;
            btnPrint.Visible = true;
            createDefaultersReport(dataSet2);
        }
        else
        {
            lblMsg.Text = "No Record Found.";
            lblMsg.ForeColor = Color.Red;
            lblReport.Text = string.Empty;
            btnExcel.Visible = false;
            btnPrint.Visible = false;
        }
    }

    private void createDefaultersReport(DataSet ds)
    {
        DataTable table = ds.Tables[0];
        if (table.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='80%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='4' style='border-right:0px;' class='innertbltxt'><font color='Black'><div style='float:left;'>No Of Records: " + table.Rows.Count.ToString() + "</div></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td colspan='8' style='border-left:0px;' align='left' class='innertbltxt'><font color='Black'><b>Sales Report(" + txtFromDt.Text.ToString() + " to " + txtToDt.Text.ToString() + ")</b></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='80%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 80px' align='left'class='innertbltxt'><strong>Receipt No</strong></td>");
        stringBuilder.Append("<td style='width: 80px' align='left' class='innertbltxt'><strong>Date</strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='innertbltxt'><strong>Particulars</strong></td>");
        stringBuilder.Append("<td style='width: 70px' align='left'class='innertbltxt'><strong>Total Amt.</strong></td>");
        stringBuilder.Append("<td style='width: 70px' align='left' class='innertbltxt'><strong>Additional Disc</strong></td>");
        stringBuilder.Append("<td style='width: 70px' align='left'class='innertbltxt'><strong>Additional Charges</strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='right'class='innertbltxt'><strong>Net Amount</strong></td>");
        stringBuilder.Append("</tr>");
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center' style='width: 80px' >");
            string str1 = "<a href='javascript:popUp";
            string str2 = "('DlySalesDetails.aspx?Id=" + row["InvNo"].ToString() + "')";
            string str3 = "'>";
            stringBuilder.Append(str1.Replace('\'', '"') + str2 + str3.Replace('\'', '"'));
            stringBuilder.Append("<b>" + row["PhysicalBillNo"].ToString().Trim() + " </b></a>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='innertbltxt'>");
            stringBuilder.Append(row["InvDt"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='innertbltxt' style='white-space:nowrap;'>");
            stringBuilder.Append(row["PartyName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='innertbltxt'>");
            stringBuilder.Append(row["TotalAmt"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='innertbltxt'>");
            stringBuilder.Append(row["AddnlDiscount"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='innertbltxt'>");
            stringBuilder.Append(row["AddnlCharges"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right'  class='innertbltxt'>");
            stringBuilder.Append(row["RoundNetPayble"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            num1 += Convert.ToDouble(row["TotalAmt"].ToString().Trim());
            num2 += Convert.ToDouble(row["AddnlDiscount"].ToString().Trim());
            num3 += Convert.ToDouble(row["AddnlCharges"].ToString().Trim());
            num4 += Convert.ToDouble(row["RoundNetPayble"].ToString().Trim());
        }
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='right' style='border-top:0px;' colspan='3' class='innertbltxt'>");
        stringBuilder.Append("<b>Total :</b></td>");
        stringBuilder.Append("<td align='right' style='border-top:0px;' class='innertbltxt'>");
        stringBuilder.Append("<b>" + string.Format("{0:F2}", num1) + "</b></td>");
        stringBuilder.Append("<td align='right' style='border-top:0px;' class='innertbltxt'>");
        stringBuilder.Append("<b>" + string.Format("{0:F2}", num2) + "</b></td>");
        stringBuilder.Append("<td align='right' style='border-top:0px;' class='innertbltxt'>");
        stringBuilder.Append("<b>" + string.Format("{0:F2}", num3) + "</b></td>");
        stringBuilder.Append("<td align='right' style='border-top:0px;' class='innertbltxt'>");
        stringBuilder.Append("<b>" + string.Format("{0:F2}", num4) + "</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table border='0px' cellpadding='1' cellspacing='5' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;border:1px;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 25%' valign='top'>");
        stringBuilder.Append("<table border='0px' cellpadding='1' cellspacing='5' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;border:1px;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='left' class='innertbltxt' style='width: 100px'>Cash :");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='left' class='innertbltxt' style='width: 60px'>");
        stringBuilder.Append(ds.Tables[1].Rows[0]["Cash"].ToString());
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='left' class='innertbltxt'>Bank :");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='left' class='innertbltxt'>");
        stringBuilder.Append(ds.Tables[2].Rows[0]["Bank"].ToString());
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        Decimal num5 = Convert.ToDecimal(ds.Tables[1].Rows[0]["Cash"].ToString()) + Convert.ToDecimal(ds.Tables[2].Rows[0]["Bank"].ToString());
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='left' class='innertbltxt'><b>Total Sale :</b>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='left' class='innertbltxt'  style='border-top:1px black solid;' ><b>");
        stringBuilder.Append(num5);
        stringBuilder.Append("</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td valign='top'>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["DailySales"] = Regex.Replace(stringBuilder.ToString(), "<a\\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('rptSalesDlyPrint.aspx');", true);
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
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("<tr><td align='left'><b>");
                stringBuilder.Append("Sales Report:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["DailySales"].ToString()).ToString().Trim());
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
            string str = Server.MapPath("Exported_Files/rptDailySales" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=rptSalesDly" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}