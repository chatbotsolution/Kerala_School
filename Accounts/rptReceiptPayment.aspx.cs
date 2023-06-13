using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_rptReceiptPayment : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private NumberFormatInfo format = new NumberFormatInfo();
    private int[] indSizes = new int[4] { 3, 2, 2, 2 };
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        if (!Page.IsPostBack)
        {
            BindSessionYr();
            getFyDate();
        }
        format.CurrencyDecimalDigits = 2;
        format.CurrencyDecimalSeparator = ".";
        format.CurrencyGroupSeparator = ",";
        format.CurrencySymbol = "";
        format.CurrencyGroupSizes = indSizes;
    }

    private void BindSessionYr()
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = obj.GetDataTableQry("select FY_Id,FY from dbo.ACTS_FinancialYear order by FY_Id desc");
        if (dataTableQry.Rows.Count > 0)
        {
            drpSession.DataSource = dataTableQry;
            drpSession.DataTextField = "FY";
            drpSession.DataValueField = "FY";
            drpSession.DataBind();
        }
        else
        {
            lblMsg.Text = "Please Define Financial Year To View Report";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void getFyDate()
    {
        if (drpSession.Items.Count <= 0)
            return;
        string str = drpSession.SelectedValue.Trim().Split('-')[0];
        dtpfromdt.SetDateValue(Convert.ToDateTime("1 Apr " + str));
        dtptodt.SetDateValue(Convert.ToDateTime("31 Mar " + (Convert.ToInt32(str.Trim()) + 1).ToString()));
        lblReport.Text = "";
    }

    private string checkDate()
    {
        obj = new clsDAL();
        string str1 = obj.ExecuteScalarQry("select count(*) from dbo.ACTS_FinancialYear where FY='" + drpSession.SelectedValue.Trim() + "' and StartDate<='" + dtpfromdt.GetDateValue().ToString("dd MMM yyyy") + "' and EndDate>='" + dtpfromdt.GetDateValue().ToString("dd MMM yyyy") + "'");
        if (str1.Trim() == "" || str1.Trim() == "0")
        {
            lblMsg.Text = "'From Date' is not in the selected finnancial year!!";
            lblMsg.ForeColor = Color.Red;
            return "NO";
        }
        string str2 = obj.ExecuteScalarQry("select count(*) from dbo.ACTS_FinancialYear where FY='" + drpSession.SelectedValue.Trim() + "' and StartDate<='" + dtptodt.GetDateValue().ToString("dd MMM yyyy") + "' and EndDate>='" + dtptodt.GetDateValue().ToString("dd MMM yyyy") + "'");
        if (!(str2.Trim() == "") && !(str2.Trim() == "0"))
            return "";
        lblMsg.Text = "'To Date' is not in the selected finnancial year!!";
        lblMsg.ForeColor = Color.Red;
        return "NO";
    }

    private void GenerateReport()
    {
        if (checkDate().Trim() == "")
        {
            Hashtable hashtable = new Hashtable();
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            DataTable dataTable3 = new DataTable();
            DataTable dataTable4 = new DataTable();
            DataTable dataTable5 = new DataTable();
            DataTable dataTable6 = new DataTable();
            DataTable dataTable7 = new DataTable();
            DataSet dataSet1 = new DataSet();
            Decimal num1 = new Decimal(0);
            Decimal num2 = new Decimal(0);
            Decimal num3 = new Decimal(0);
            hashtable.Add("@StartDt", dtpfromdt.GetDateValue().ToString("dd MMM yyyy"));
            hashtable.Add("@EndDt", dtptodt.GetDateValue().ToString("dd MMM yyyy"));
            hashtable.Add("@FYStDt", Convert.ToDateTime(obj.ExecuteScalarQry("select StartDate from dbo.ACTS_FinancialYear where FY='" + drpSession.SelectedValue.Trim() + "'")).ToString("dd MMM yyyy"));
            DataSet dataSet2 = obj.GetDataSet("ACTS_GetTotPayRcpt", hashtable);
            DataTable table1 = dataSet2.Tables[1];
            DataTable table2 = dataSet2.Tables[4];
            DataTable table3 = dataSet2.Tables[2];
            DataTable table4 = dataSet2.Tables[0];
            DataTable table5 = dataSet2.Tables[6];
            DataTable table6 = dataSet2.Tables[3];
            DataTable table7 = dataSet2.Tables[5];
            StringBuilder stringBuilder = new StringBuilder();
            if (dataSet2.Tables.Count > 0)
            {
                string str = drpSession.SelectedValue.ToString().Trim();
                stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='1000px' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='4' style='border-right-style:none;' align='center' class='gridtext'><font color='Black'><b>RECEIPT AND PAYMENT ACCOUNT FOR THE FINANCIAL YEAR " + str + " FROM " + dtpfromdt.GetDateValue().ToString("dd MMM yyyy") + " To " + dtptodt.GetDateValue().ToString("dd MMM yyyy") + "</b></font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr><td style='width:500px'  valign='top'>");
                stringBuilder.Append("<table width='100%'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='width: 30px' align='left'  class='gridtext'><b>Receipt</b></td><td></td>");
                stringBuilder.Append("<td align='right' class='gridtext'><b>Amount(Rs.)</b></td>");
                stringBuilder.Append("</tr><tr><td colspan='2' style='text-decoration:underline'><b>Openning Balance</b></td></tr>");
                stringBuilder.Append("<td align='left' class='gridtext'>Cash In Hand</td>");
                stringBuilder.Append("<td align='right' class='gridtext'>");
                if (table4.Rows.Count > 0)
                {
                    stringBuilder.Append(table4.Rows[0]["TransAmtStr"].ToString() + "</td><td></td>");
                    num3 = Convert.ToDecimal(table4.Rows[0]["TransAmtStr"]);
                }
                else
                    stringBuilder.Append("0</td><td></td>");
                if (table1.Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)table1.Rows)
                    {
                        num3 += Convert.ToDecimal(row["TransAmtStr"]);
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td align='left' class='gridtext'>");
                        stringBuilder.Append(row["AcctsHead"].ToString() + "</td>");
                        stringBuilder.Append("<td align='right' class='gridtext'>");
                        stringBuilder.Append(row["TransAmtStr"].ToString() + "</td><td></td>");
                        stringBuilder.Append("</tr>");
                    }
                }
                stringBuilder.Append("<tr><td></td><td colspan='2'><hr /></td></tr><tr><td colspan='3' align='right'><b>" + num3 + "</b></td></tr><tr colspan='3'><td>&nbsp;</td></tr>");
                Decimal num4 = num3;
                foreach (DataRow row in (InternalDataCollectionBase)table3.Rows)
                {
                    num4 += Convert.ToDecimal(row["AmountStr"]);
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' class='gridtext' style='width:250px'>");
                    stringBuilder.Append(row["AcctsHead"].ToString() + "</td><td></td>");
                    stringBuilder.Append("<td colspan='2' align='right' class='gridtext'>");
                    stringBuilder.Append(row["AmountStr"].ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                }
                foreach (DataRow row in (InternalDataCollectionBase)table6.Rows)
                {
                    num4 += Convert.ToDecimal(row["AmountStr"]);
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' class='gridtext' style='width:250px'>");
                    stringBuilder.Append(row["AcctsHead"].ToString() + "</td><td></td>");
                    stringBuilder.Append("<td colspan='2' align='right' class='gridtext'>");
                    stringBuilder.Append(row["AmountStr"].ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("</table></td>");
                stringBuilder.Append("<td style='width:500px' colspan='2' valign='top'><table width='100%'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='width: 30px' align='left'  class='gridtext'><b>Payment</b></td><td></td>");
                stringBuilder.Append("<td align='right' class='gridtext'><b>Amount(Rs.)</b></td>");
                stringBuilder.Append("</tr>");
                foreach (DataRow row in (InternalDataCollectionBase)table2.Rows)
                {
                    num2 += Convert.ToDecimal(row["AmountStr"]);
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' class='gridtext' style='width:250px'>");
                    stringBuilder.Append(row["AcctsHead"].ToString() + "</td><td></td>");
                    stringBuilder.Append("<td align='right' class='gridtext'>");
                    stringBuilder.Append(row["AmountStr"].ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                }
                foreach (DataRow row in (InternalDataCollectionBase)table7.Rows)
                {
                    num2 += Convert.ToDecimal(row["AmountStr"]);
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' class='gridtext' style='width:250px'>");
                    stringBuilder.Append(row["AcctsHead"].ToString() + "</td><td></td>");
                    stringBuilder.Append("<td align='right' class='gridtext'>");
                    stringBuilder.Append(row["AmountStr"].ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("<tr><td><b><u>Closing Balance</u></b></td></tr>");
                foreach (DataRow row in (InternalDataCollectionBase)table5.Rows)
                {
                    num2 += Convert.ToDecimal(row["TransAmtStr"]);
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' class='gridtext' style='width:250px'>");
                    stringBuilder.Append(row["AcctsHead"].ToString() + "</td><td></td>");
                    stringBuilder.Append("<td align='right' class='gridtext'>");
                    stringBuilder.Append(row["TransAmtStr"].ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("</table></td></tr>");
                stringBuilder.Append("<tr><td align='right'><b>Total:" + Convert.ToDecimal(num4).ToString("C", (IFormatProvider)format) + "</b></td>");
                stringBuilder.Append("<td align='right' colspan='2'><b>Total:" + Convert.ToDecimal(num2).ToString("C", (IFormatProvider)format) + "</b></td></tr>");
                stringBuilder.Append("</td></tr></table>");
                lblReport.Text = stringBuilder.ToString();
                Session["PrntRcptPay"] = stringBuilder.ToString();
            }
            else
            {
                lblReport.Text = "No Relevant Data Found";
                Session["PrntRcptPay"] = null;
            }
        }
        else
            lblReport.Text = "";
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptReceiptPaymentPrint.aspx");
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        getFyDate();
    }
}