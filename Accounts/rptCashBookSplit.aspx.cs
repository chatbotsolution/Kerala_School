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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_rptCashBookSplit : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private NumberFormatInfo format = new NumberFormatInfo();
    private int[] indSizes = new int[4] { 3, 2, 2, 2 };
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        format.CurrencyDecimalDigits = 2;
        format.CurrencyDecimalSeparator = ".";
        format.CurrencyGroupSeparator = ",";
        format.CurrencySymbol = "";
        format.CurrencyGroupSizes = indSizes;
        if (Page.IsPostBack)
            return;
        dtpDt.SetDateValue(DateTime.Now);
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        StringBuilder stringBuilder1 = new StringBuilder();
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        Decimal num3 = new Decimal(0);
        Decimal num4 = new Decimal(0);
        Decimal num5 = new Decimal(0);
        Decimal num6 = new Decimal(0);
        DataTable dataTableQry = obj.GetDataTableQry("select FY from ACTS_FinancialYear where StartDate <='" + dtpDt.GetDateValue().ToString("dd MMM yyyy") + "' and EndDate >='" + dtpDt.GetDateValue().ToString("dd MMM yyyy") + "'");
        if (dataTableQry.Rows.Count == 0)
        {
            lblMsg.Text = "Selected year is not valid financial year...";
            lblMsg.ForeColor = Color.Red;
            Literal1.Text = "";
        }
        else
        {
            string str1 = dataTableQry.Rows[0]["FY"].ToString();
            stringBuilder1.Append("<table width='100%' border='1' cellspacing='0' cellpadding='2' style='border-collapse:collapse;'>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td align='center' colspan='10' style='font-size:14px'><strong> Cash Book as on " + dtpDt.GetDateValue().ToString("dd MMM yyyy") + "</strong></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td width='50%' align='left' colspan='4' style='font-size:13px'><b>RECEIPT</b></td>");
            stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
            stringBuilder1.Append("<td width='50%' align='left' colspan='5' style='font-size:13px'><b>PAYMENT</b></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='2' style='width:36%;'><b>Particulars</b></td><td align='right'><b>CASH</b></td><td align='right'><b>BANK</b></td>");
            stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
            stringBuilder1.Append("<td style='width:35%;' colspan='2'><b>Particulars</b></td><td align='right' style='width:5%;'><b>V No.</b></td><td align='right'><b>CASH</b></td><td align='right'><b>BANK</b></td>");
            stringBuilder1.Append("</tr>");
            Hashtable hashtable1 = new Hashtable();
            DataSet dataSet1 = new DataSet();
            DateTime dateTime = Convert.ToDateTime("01 APR" + str1.Substring(0, 4));
            hashtable1.Add("@StartDt", dateTime);
            hashtable1.Add("@Dt", Convert.ToDateTime(dtpDt.GetDateValue().ToString("dd MMM yyyy")));
            DataSet dataSet2 = obj.GetDataSet("ACTS_GetSplitOpngBal", hashtable1);
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='2'><b>To Opening Balance</b></td>");
            stringBuilder1.Append("<td></td>");
            stringBuilder1.Append("<td></td>");
            stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
            stringBuilder1.Append("<td colspan='2'></td>");
            stringBuilder1.Append("<td></td>");
            stringBuilder1.Append("<td></td><td></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='2'>Cash In Hand</td>");
            stringBuilder1.Append("<td align='right'>" + Convert.ToDecimal(dataSet2.Tables[0].Rows[0]["Cash"]).ToString("0.00") + "</td>");
            stringBuilder1.Append("<td></td>");
            stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
            stringBuilder1.Append("<td colspan='2'></td>");
            stringBuilder1.Append("<td></td>");
            stringBuilder1.Append("<td></td><td></td>");
            stringBuilder1.Append("</tr>");
            Decimal num7 = num3 + Convert.ToDecimal(dataSet2.Tables[0].Rows[0]["Cash"].ToString().Trim());
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='2'>Cash At Bank</td>");
            stringBuilder1.Append("<td></td>");
            StringBuilder stringBuilder2 = new StringBuilder();
            string str2 = "";
            if (dataSet2.Tables[1].Rows.Count > 0)
                str2 = dataSet2.Tables[1].Rows[0]["AcctsHead"].ToString();
            if (dataSet2.Tables.Count > 0)
            {
                if (str2 != "")
                {
                    foreach (DataRow row in (InternalDataCollectionBase)dataSet2.Tables[1].Rows)
                    {
                        if (str2 != row["AcctsHead"].ToString())
                        {
                            stringBuilder2.Append("<tr>");
                            stringBuilder2.Append("<td >" + str2 + "</td><td align='right'>" + num1.ToString("0.00") + "</td>");
                            stringBuilder2.Append("<td></td>");
                            stringBuilder2.Append("<td></td>");
                            stringBuilder2.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
                            stringBuilder2.Append("<td  colspan='2'></td>");
                            stringBuilder2.Append("<td></td>");
                            stringBuilder2.Append("<td></td>");
                            stringBuilder2.Append("<td></td>");
                            stringBuilder2.Append("</tr>");
                            str2 = row["AcctsHead"].ToString();
                            num2 += num1;
                            num1 = new Decimal(0);
                        }
                        if (row["CrDr"].ToString().ToUpper() == "CR")
                            num1 -= Convert.ToDecimal(row["Amount"].ToString().Trim());
                        else
                            num1 += Convert.ToDecimal(row["Amount"].ToString().Trim());
                    }
                    num2 += num1;
                    num4 = num2;
                    stringBuilder2.Append("<tr>");
                    stringBuilder2.Append("<td >" + str2 + "</td><td align='right'>" + num1.ToString("0.00") + "</td>");
                    stringBuilder2.Append("<td></td>");
                    stringBuilder2.Append("<td></td>");
                    stringBuilder2.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
                    stringBuilder2.Append("<td  colspan='2'></td>");
                    stringBuilder2.Append("<td></td>");
                    stringBuilder2.Append("<td></td><td></td>");
                    stringBuilder2.Append("</tr>");
                }
                stringBuilder1.Append("<td align='right'>" + num2.ToString("0.00") + "</td>");
                stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
                stringBuilder1.Append("<td  colspan='2'></td>");
                stringBuilder1.Append("<td></td>");
                stringBuilder1.Append("<td></td><td></td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append(stringBuilder2.ToString());
                Hashtable hashtable2 = new Hashtable();
                if (txtstartdate.Text.Trim() != "")
                    hashtable2.Add("@FrmDt", dtpDt.GetDateValue().ToString("dd MMM yyyy 00:00:00"));
                DataSet dataSet3 = obj.GetDataSet("Acts_GetCashBookSplit", hashtable2);
                DataTable table1 = dataSet3.Tables[0];
                DataTable table2 = dataSet3.Tables[1];
                if (table1.Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)table1.Rows)
                    {
                        if (row["CrDr"].ToString().ToUpper() == "DR")
                        {
                            stringBuilder1.Append("<tr><td colspan='2'> To " + row["Particulars"] + "</td>");
                            if (row["AcctsHead"].ToString().Trim().ToUpper() == "CASH IN HAND")
                            {
                                stringBuilder1.Append("<td align='right'>" + Convert.ToDecimal(row["TransAmt"].ToString().Trim()).ToString("0.00") + "</td>");
                                stringBuilder1.Append("<td></td>");
                                num7 += Convert.ToDecimal(row["TransAmt"].ToString().Trim());
                            }
                            else
                            {
                                stringBuilder1.Append("<td></td>");
                                stringBuilder1.Append("<td align='right'>" + Convert.ToDecimal(row["TransAmt"].ToString().Trim()).ToString("0.00") + "</td>");
                                num4 += Convert.ToDecimal(row["TransAmt"].ToString().Trim());
                            }
                            stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
                            stringBuilder1.Append("<td  colspan='2'></td>");
                            stringBuilder1.Append("<td></td>");
                            stringBuilder1.Append("<td></td><td></td>");
                            stringBuilder1.Append("</tr>");
                        }
                    }
                }
                if (table2.Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)table2.Rows)
                    {
                        if (row["CrDr"].ToString().ToUpper() == "CR")
                        {
                            stringBuilder1.Append("<tr>");
                            stringBuilder1.Append("<td colspan='2'></td>");
                            stringBuilder1.Append("<td></td>");
                            stringBuilder1.Append("<td ></td>");
                            if (row["InstrumentNo"].ToString().Trim() == "")
                                stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td><td  colspan='2'> By " + row["Particulars"].ToString().Replace("By", "") + "</td>");
                            else
                                stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td><td  colspan='2'> By " + row["Particulars"].ToString().Replace("By", "") + " Vide Inst No. " + row["InstrumentNo"].ToString() + "</td>");
                            stringBuilder1.Append("<td align='right'>" + row["PmtRecptVoucherNo"].ToString() + "</td>");
                            if (row["AcctsHead"].ToString().Trim().ToUpper() == "CASH IN HAND")
                            {
                                stringBuilder1.Append("<td align='right'>" + Convert.ToDecimal(row["TransAmt"].ToString().Trim()).ToString("0.00") + "</td>");
                                stringBuilder1.Append("<td></td>");
                                num5 += Convert.ToDecimal(row["TransAmt"].ToString().Trim());
                            }
                            else
                            {
                                stringBuilder1.Append("<td></td>");
                                stringBuilder1.Append("<td align='right'>" + Convert.ToDecimal(row["TransAmt"].ToString().Trim()).ToString("0.00") + "</td>");
                                num6 += Convert.ToDecimal(row["TransAmt"].ToString().Trim());
                            }
                            stringBuilder1.Append("</tr>");
                        }
                    }
                }
                DataTable table3 = dataSet3.Tables[2];
                DataTable table4 = dataSet3.Tables[3];
                if (table3.Rows.Count > 0 && table4.Rows.Count > 0 && table3.Rows.Count == table4.Rows.Count)
                {
                    for (int index = 0; index < table3.Rows.Count; ++index)
                    {
                        stringBuilder1.Append("<tr><td colspan='2'> To " + table3.Rows[index]["Particulars"].ToString().Replace("By", "") + "</td>");
                        if (table3.Rows[index]["AcctsHead"].ToString().Trim().ToUpper() == "CASH IN HAND")
                        {
                            stringBuilder1.Append("<td align='right'>" + Convert.ToDecimal(table3.Rows[index]["TransAmt"].ToString().Trim()).ToString("0.00") + "</td>");
                            stringBuilder1.Append("<td></td>");
                            num7 += Convert.ToDecimal(table3.Rows[index]["TransAmt"].ToString().Trim());
                        }
                        else
                        {
                            stringBuilder1.Append("<td></td>");
                            stringBuilder1.Append("<td align='right'>" + Convert.ToDecimal(table3.Rows[index]["TransAmt"].ToString().Trim()).ToString("0.00") + "</td>");
                            num4 += Convert.ToDecimal(table3.Rows[index]["TransAmt"].ToString().Trim());
                        }
                        stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
                        if (table4.Rows[index]["TransType"].ToString().Trim().ToUpper() == "")
                        {
                            stringBuilder1.Append("<td  colspan='2'> By " + table4.Rows[index]["Particulars"].ToString().Replace("By", "").Replace("To", "") + "</td>");
                            stringBuilder1.Append("<td></td>");
                        }
                        else
                        {
                            stringBuilder1.Append("<td  colspan='2'></td>");
                            stringBuilder1.Append("<td></td>");
                        }
                        if (table4.Rows[index]["AcctsHead"].ToString().Trim().ToUpper() == "CASH IN HAND")
                        {
                            stringBuilder1.Append("<td align='right'>" + Convert.ToDecimal(table4.Rows[index]["TransAmt"].ToString().Trim()).ToString("0.00") + "</td>");
                            stringBuilder1.Append("<td></td>");
                            num5 += Convert.ToDecimal(table4.Rows[index]["TransAmt"].ToString().Trim());
                        }
                        else if (table4.Rows[index]["TransType"].ToString().Trim().ToUpper() == "")
                        {
                            stringBuilder1.Append("<td></td>");
                            stringBuilder1.Append("<td align='right'>" + Convert.ToDecimal(table3.Rows[index]["TransAmt"].ToString().Trim()).ToString("0.00") + "</td>");
                            num6 += Convert.ToDecimal(table4.Rows[index]["TransAmt"].ToString().Trim());
                        }
                        else
                        {
                            stringBuilder1.Append("<td></td>");
                            stringBuilder1.Append("<td></td>");
                        }
                        stringBuilder1.Append("</tr>");
                    }
                }
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td colspan='2'></td>");
                stringBuilder1.Append("<td></td>");
                stringBuilder1.Append("<td></td>");
                stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
                stringBuilder1.Append("<td></td>");
                stringBuilder1.Append("<td></td>");
                stringBuilder1.Append("<td></td>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td colspan='2'></td>");
                stringBuilder1.Append("<td></td>");
                stringBuilder1.Append("<td></td>");
                stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
                stringBuilder1.Append("<td colspan='2'><b>Total</b></td>");
                stringBuilder1.Append("<td ></td>");
                stringBuilder1.Append("<td align='right'>" + num5.ToString("0.00") + "</td>");
                stringBuilder1.Append("<td align='right'>" + num6.ToString("0.00") + "</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td colspan='2'></td>");
                stringBuilder1.Append("<td></td>");
                stringBuilder1.Append("<td></td>");
                stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
                stringBuilder1.Append("<td colspan='2'><b>By c/d</b></td>");
                stringBuilder1.Append("<td ></td>");
                stringBuilder1.Append("<td align='right'>" + (num7 - num5).ToString("0.00") + "</td>");
                stringBuilder1.Append("<td align='right'>" + (num4 - num6).ToString("0.00") + "</td>");
                stringBuilder1.Append("</tr>");
                Hashtable hashtable3 = new Hashtable();
                hashtable3.Add("@StartDt", dateTime);
                hashtable3.Add("@Dt", dtpDt.GetDateValue().ToString("dd MMM yyyy"));
                DataTable dataTable = new DataTable();
                foreach (DataRow row in (InternalDataCollectionBase)obj.GetDataTable("ACTS_GetCashBookClsng", hashtable3).Rows)
                {
                    stringBuilder1.Append("<tr>");
                    stringBuilder1.Append("<td colspan='2'></td>");
                    stringBuilder1.Append("<td></td>");
                    stringBuilder1.Append("<td></td>");
                    stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
                    stringBuilder1.Append("<td>" + row["AcctsHead"] + "</td>");
                    stringBuilder1.Append("<td>" + Convert.ToDecimal(row["Amount"]).ToString("0.00") + "</td>");
                    stringBuilder1.Append("<td></td><td></td><td></td></tr>");
                }
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td colspan='2'><b>Grand Total</b></td>");
                stringBuilder1.Append("<td align='right'><b>" + num7.ToString("0.00") + "</b></td>");
                stringBuilder1.Append("<td align='right'><b>" + num4.ToString("0.00") + "</b></td>");
                stringBuilder1.Append("<td style='border-bottom-style:none;border-top-style:none;'></td>");
                stringBuilder1.Append("<td colspan='2'> <b>Grand Total</b></td>");
                stringBuilder1.Append("<td ></td>");
                stringBuilder1.Append("<td align='right'><b>" + (num5 + (num7 - num5)).ToString("0.00") + "</b></td>");
                stringBuilder1.Append("<td align='right'><b>" + (num6 + (num4 - num6)).ToString("0.00") + "</b></td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("</table>");
                Literal1.Text = stringBuilder1.ToString();
                btnprnt.Visible = true;
            }
            else
            {
                lblMsg.Text = "No Data Found";
                lblMsg.ForeColor = Color.Red;
                Literal1.Text = "";
            }
        }
    }

    protected void btnShowConsol_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptCashLedger.aspx");
    }

    protected void btnShowDtls_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptCashBookNew.aspx");
    }
}