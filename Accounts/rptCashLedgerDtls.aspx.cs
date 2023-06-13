using AjaxControlToolkit;
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

public partial class Accounts_rptCashLedgerDtls : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private NumberFormatInfo format = new NumberFormatInfo();
    private int[] indSizes = new int[4] { 3, 2, 2, 2 };
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        btnprnt.Visible = false;
        if (!Page.IsPostBack)
        {
            txtstartdate.Text = DateTime.Now.Month <= 3 ? new DateTime(DateTime.Now.Year - 1, 4, 1).ToString("dd-MM-yyyy") : new DateTime(DateTime.Now.Year, 4, 1).ToString("dd-MM-yyyy");
            txtenddate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        }
        format.CurrencyDecimalDigits = 2;
        format.CurrencyDecimalSeparator = ".";
        format.CurrencyGroupSeparator = ",";
        format.CurrencySymbol = "";
        format.CurrencyGroupSizes = indSizes;
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        Decimal num3 = new Decimal(0);
        double num4 = 0.0;
        double num5 = 0.0;
        double num6 = 0.0;
        double num7 = 0.0;
        StringBuilder sb = new StringBuilder();
        ArrayList arrayList1 = new ArrayList();
        ArrayList arrayList2 = new ArrayList();
        ArrayList arrayList3 = new ArrayList();
        ArrayList arrayList4 = new ArrayList();
        ArrayList arrayList5 = new ArrayList();
        ArrayList arrayList6 = new ArrayList();
        ArrayList arrayList7 = new ArrayList();
        ArrayList arrayList8 = new ArrayList();
        ArrayList arrayList9 = new ArrayList();
        string str1 = "";
        DataTable dataTableQry1 = obj.GetDataTableQry("select FY from ACTS_FinancialYear where StartDate <='" + PopCalendar1.GetDateValue().ToString("dd MMM yyyy") + "' and EndDate >='" + PopCalendar2.GetDateValue().ToString("dd MMM yyyy") + "'");
        if (dataTableQry1.Rows.Count == 0)
        {
            lblMsg.Text = "Selected year is not valid financial year...";
            lblMsg.ForeColor = Color.Red;
            Literal1.Text = "";
        }
        else
        {
            string FY = dataTableQry1.Rows[0]["FY"].ToString();
            DataTable dataTableQry2 = obj.GetDataTableQry("select AcctsHeadId from ACTS_AccountHeads a inner join dbo.Acts_AccountGroups ag on ag.AG_Code=a.AG_Code where a.AG_Code in(1,7) or ag.AG_Parent=7");
            if (dataTableQry2.Rows.Count > 0)
            {
                for (int index = 0; index < dataTableQry2.Rows.Count; ++index)
                    str1 = str1 + dataTableQry2.Rows[index]["AcctsHeadId"].ToString() + ",";
                str1 = str1.Substring(0, str1.LastIndexOf(","));
            }
            DataTable dataTableQry3 = obj.GetDataTableQry("select * from dbo.ACTS_GenLedger where SessionYr='" + FY + "' and TransType='I' and AccountHead in (" + str1 + ") order by TransDate asc");
            double num8;
            if (dataTableQry3.Rows.Count > 0)
            {
                for (int index = 0; index < dataTableQry3.Rows.Count; ++index)
                {
                    if (dataTableQry3.Rows[index]["CrDr"].ToString().Trim().ToUpper() == "DR")
                        num5 += Convert.ToDouble(dataTableQry3.Rows[index]["TransAmt"]);
                    else
                        num6 += Convert.ToDouble(dataTableQry3.Rows[index]["TransAmt"]);
                }
                num8 = num4 - num5 + num6;
                num5 = 0.0;
                num6 = 0.0;
                num7 = num8;
            }
            else
                num8 = 0.0;
            DateTime dateTime = Convert.ToDateTime("01 APR" + FY.Substring(0, 4));
            DataTable dataTableQry4 = obj.GetDataTableQry("select * from dbo.ACTS_GenLedger where TransType<>'I' and AccountHead in(" + str1 + ") and (TransDate >='" + dateTime.ToString("dd MMM yyyy") + "' and TransDate<'" + PopCalendar1.GetDateValue().ToString("dd MMM yyyy") + "') order by TransDate asc");
            double num9;
            if (dataTableQry4.Rows.Count > 0)
            {
                for (int index = 0; index < dataTableQry4.Rows.Count; ++index)
                {
                    if (dataTableQry4.Rows[index]["CrDr"].ToString().Trim().ToUpper() == "DR")
                        num5 += Convert.ToDouble(dataTableQry4.Rows[index]["TransAmt"]);
                    else
                        num6 += Convert.ToDouble(dataTableQry4.Rows[index]["TransAmt"]);
                }
                num8 = num8 - num5 + num6;
                num5 = 0.0;
                num6 = 0.0;
                num9 = num8;
            }
            else
                num9 = num8;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select G.*,isnull(G.PmtRecptVoucherNo,'') as ChekNum, P.Description, AH.AcctsHead from dbo.ACTS_GenLedger G left join dbo.Acts_PaymentReceiptVoucher P on P.PR_Id=G.PR_Id inner join dbo.Acts_AccountHeads AH on AH.AcctsHeadId=G.AccountHead left join dbo.ACTS_BankTransactions b on b.TransId=G.BankTransId where  G.TransDate  >= '" + PopCalendar1.GetDateValue().ToString("dd MMM yyyy 00:00:00") + "' and G.TransDate  <= '" + PopCalendar2.GetDateValue().ToString("dd MMM yyyy 23:59:59") + "' and G.TransType<>'I' and AH.AG_Code in(1,7) and (P.IsDeleted is null or P.IsDeleted=0) and (b.IsDeleted is null or b.IsDeleted=0)");
            if (drpVType.SelectedValue.ToString() == "P")
                stringBuilder.Append(" and G.TransType in('P','E')");
            else if (drpVType.SelectedValue.ToString() == "R")
                stringBuilder.Append(" and G.TransType='R'");
            stringBuilder.Append("order by G.TransDate asc");
            DataTable dataTableQry5 = obj.GetDataTableQry(stringBuilder.ToString());
            if (dataTableQry5.Rows.Count > 0)
            {
                for (int index = 0; index < dataTableQry5.Rows.Count; ++index)
                {
                    arrayList1.Add(Convert.ToDateTime(dataTableQry5.Rows[index]["TransDate"]).ToString("dd/MM/yyyy"));
                    arrayList2.Add(dataTableQry5.Rows[index]["Particulars"].ToString());
                    arrayList7.Add(dataTableQry5.Rows[index]["TransType"].ToString());
                    arrayList8.Add(dataTableQry5.Rows[index]["Description"].ToString());
                    arrayList9.Add(dataTableQry5.Rows[index]["AcctsHead"].ToString());
                    if (dataTableQry5.Rows[index]["CrDr"].ToString().Trim().ToUpper() == "DR")
                    {
                        arrayList3.Add(Convert.ToDouble(dataTableQry5.Rows[index]["TransAmt"]));
                        num5 += Convert.ToDouble(dataTableQry5.Rows[index]["TransAmt"]);
                        arrayList4.Add("0.00");
                        num9 -= Convert.ToDouble(dataTableQry5.Rows[index]["TransAmt"]);
                        arrayList5.Add(num9);
                    }
                    else
                    {
                        arrayList4.Add(Convert.ToDouble(dataTableQry5.Rows[index]["TransAmt"]));
                        num6 += Convert.ToDouble(dataTableQry5.Rows[index]["TransAmt"]);
                        arrayList3.Add("0.00");
                        num9 += Convert.ToDouble(dataTableQry5.Rows[index]["TransAmt"]);
                        arrayList5.Add(num9);
                    }
                    arrayList6.Add(dataTableQry5.Rows[index]["ChekNum"].ToString());
                    btnprnt.Visible = true;
                }
            }
            double num10 = num8 - num5 + num6;
            string str2;
            if (num8 < 0.0)
            {
                str2 = "Dr.";
                num8 = Convert.ToDouble(num8.ToString().Replace("-", ""));
                num1 = Convert.ToDecimal(num8);
            }
            else
            {
                str2 = "Cr.";
                num2 = Convert.ToDecimal(num8);
            }
            if (dataTableQry5.Rows.Count > 0)
            {
                sb.Append("<table width='100%' class='innertbltxt'><tr><td colspan='7'></td></tr>");
                sb.Append("<tr><td colspan='7' height='4px'></td></tr>");
                sb.Append("<tr><td colspan='7' align='center' class='pageSectionLabel' style='font-size:16'><b>Cash Book</b></td></tr>");
                sb.Append("<tr><td colspan='7' height='2px'></td></tr>");
                sb.Append("<tr><td colspan='7' align='center' class='pageSectionLabel' style= 'border-bottom:dotted 1px black;'>" + PopCalendar1.GetDateValue().ToString("dd MMM yyyy") + " to " + PopCalendar2.GetDateValue().ToString("dd MMM yyyy") + "</td></tr>");
                sb.Append("<tr><td colspan='7' height='2px'>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='6' height='2px' align='right'><b>Opening Balance: " + str2 + "</b></td>");
                sb.Append("<td align='right' width='10%'><b>" + Convert.ToDecimal(num8).ToString("C", (IFormatProvider)format) + "</b></td>");
                sb.Append("<tr>");
                sb.Append("<tr><td colspan='6' height='2px'></td></tr>");
                sb.Append("<td width='2%'><b>Sno.</b></td>");
                sb.Append("<td width='10%'><b>Date</b></td>");
                sb.Append("<td ><b>Particulars</b></td>");
                sb.Append("<td width='8%'><b>Vch Type</b></td>");
                sb.Append("<td width='10%'><b>Rcpt/Vr. No.</b></td>");
                sb.Append("<td width='10%' align='right'><b>Debit</b></td>");
                sb.Append("<td width='10%' align='right'><b>Credit</b></td>");
                sb.Append("</tr>");
                string str3 = arrayList1[0].ToString();
                for (int index = 0; index < arrayList1.Count; ++index)
                {
                    if (str3 != arrayList1[index].ToString())
                    {
                        sb.Append("<tr height='10px'>");
                        sb.Append("<td colspan='5'></td>");
                        sb.Append("<td valign='top' align='right' style='border-top:dashed 1; '>" + num1.ToString("C", (IFormatProvider)format) + "</td>");
                        sb.Append("<td valign='top' align='right' style='border-top:dashed 1; '>" + num2.ToString("C", (IFormatProvider)format) + "</td></tr>");
                        sb.Append("<tr height='10px'>");
                        sb.Append("<td width='2%' valign='top'></td>");
                        sb.Append("<td width='10%' valign='top'></td>");
                        sb.Append("<td valign='top' style='font:14'><b>By Closing Balance</b></td>");
                        sb.Append("<td width='' valign='top'></td>");
                        sb.Append("<td width='10%' valign='top'></td>");
                        Decimal num11;
                        if (num1 >= num2)
                        {
                            num11 = num1 - num2;
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; '>" + num11.ToString("C", (IFormatProvider)format) + "</td>");
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; '></td></tr>");
                            num1 = num11;
                            num2 = new Decimal(0);
                        }
                        else
                        {
                            num11 = num2 - num1;
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; '></td>");
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; '>" + num11.ToString("C", (IFormatProvider)format) + "</td></tr>");
                            num2 = num11;
                            num1 = new Decimal(0);
                        }
                        str3 = arrayList1[index].ToString();
                        sb.Append("<tr height='10px'>");
                        sb.Append("<td width='2%' valign='top'></td>");
                        sb.Append("<td width='10%' valign='top'>" + str3 + "</td>");
                        sb.Append("<td valign='top' style='font:14'><b>To Opening Balance</b></td>");
                        sb.Append("<td width='' valign='top'></td>");
                        sb.Append("<td width='10%' valign='top'></td>");
                        if (num1 >= num2)
                        {
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; border-bottom-style:inset'>" + num11.ToString("C", (IFormatProvider)format) + "</td>");
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; border-bottom-style:inset'></td></tr>");
                        }
                        else
                        {
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; border-bottom-style:inset'></td>");
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; border-bottom-style:inset'>" + num11.ToString("C", (IFormatProvider)format) + "</td></tr>");
                        }
                        num11 = new Decimal(0);
                    }
                    sb.Append("<tr>");
                    sb.Append("<td width='2%' valign='top'>" + Convert.ToString(index + 1) + "</td>");
                    sb.Append("<td width='10%' valign='top'>" + arrayList1[index].ToString() + "</td>");
                    if (arrayList8[index].ToString() != "")
                        sb.Append("<td >" + arrayList9[index].ToString() + " (" + arrayList8[index].ToString() + ")</td>");
                    else
                        sb.Append("<td >" + arrayList9[index].ToString() + " (" + arrayList2[index].ToString() + ")</td>");
                    if (arrayList7[index].ToString().Trim() == "PP")
                        sb.Append("<td >Purchase</td>");
                    else if (arrayList7[index].ToString().Trim() == "P")
                        sb.Append("<td >Payment</td>");
                    else if (arrayList7[index].ToString().Trim() == "E")
                        sb.Append("<td >Payment</td>");
                    else if (arrayList7[index].ToString().Trim() == "SS")
                        sb.Append("<td >Sales</td>");
                    else if (arrayList7[index].ToString().Trim() == "R")
                        sb.Append("<td >Receipt</td>");
                    sb.Append("<td width='10%'>" + arrayList6[index].ToString() + "</td>");
                    sb.Append("<td width='15%' align='right'>" + Convert.ToDecimal(arrayList3[index]).ToString("C", (IFormatProvider)format) + "</td>");
                    sb.Append("<td width='15%' align='right'>" + Convert.ToDecimal(arrayList4[index]).ToString("C", (IFormatProvider)format) + "</td>");
                    num1 += Convert.ToDecimal(arrayList3[index]);
                    num2 += Convert.ToDecimal(arrayList4[index]);
                    sb.Append("</tr>");
                }
                sb.Append("<tr>");
                sb.Append("<td width='2%'></td>");
                sb.Append("<td width='10%'></td>");
                sb.Append("<td width='40%'></td>");
                sb.Append("<td width='10%'></td>");
                sb.Append("<td width='10%'></td>");
                sb.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                sb.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                sb.Append("<td width='10%' align='right'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td width='2%'></td>");
                sb.Append("<td width='10%'></td>");
                sb.Append("<td width='10%'></td>");
                sb.Append("<td width='10%'></td>");
                sb.Append("<td width='10%' align='right'><b>Total -:</b></td>");
                sb.Append("<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>" + Convert.ToDecimal(num5).ToString("C", (IFormatProvider)format) + "</b></td>");
                sb.Append("<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>" + Convert.ToDecimal(num6).ToString("C", (IFormatProvider)format) + "</b></td>");
                sb.Append("<td width='10%' align='right'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td width='2%'></td>");
                sb.Append("<td width='10%'></td>");
                sb.Append("<td width='40%'></td>");
                sb.Append("<td width='10%' align='right'>&nbsp;</td>");
                sb.Append("<td width='10%' align='right'>&nbsp;</td>");
                sb.Append("<td width='10%' align='right'></td>");
                sb.Append("</tr>");
                string str4;
                if (num10 < 0.0)
                {
                    num10 = Math.Round(Convert.ToDouble(num10.ToString().Replace("-", "")), 2);
                    str4 = "Dr.";
                }
                else
                    str4 = "Cr.";
                sb.Append("<tr><td colspan='6' height='2px' align='right'><b>Closing Balance: " + str4 + "</b></td>");
                sb.Append("<td align='right'><b>" + Convert.ToDecimal(num10).ToString("C", (IFormatProvider)format) + "</b></td>");
                sb.Append("<tr>");
                sb.Append("</table>");
                sb = ShowSplit(sb, FY);
            }
            else
            {
                lblMsg.Text = "No Data Found";
                lblMsg.ForeColor = Color.Red;
            }
            Literal1.Text = sb.ToString();
            btnprnt.Visible = true;
        }
    }

    private StringBuilder ShowSplit(StringBuilder sb, string FY)
    {
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        Decimal num3 = new Decimal(0);
        Decimal num4 = new Decimal(0);
        string str1 = "";
        obj = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (txtstartdate.Text.Trim() != "")
            hashtable.Add("@FrmDt", PopCalendar1.GetDateValue().ToString("dd MMM yyyy 00:00:00"));
        if (txtenddate.Text.Trim() != "")
            hashtable.Add("@ToDt", PopCalendar2.GetDateValue().ToString("dd MMM yyyy 00:00:00"));
        DataSet dataSet1 = new DataSet();
        DataSet dataSet2 = obj.GetDataSet("ACTS_GetCashDtls", hashtable);
        StringBuilder stringBuilder1 = new StringBuilder();
        DataTable dataTable = new DataTable();
        DataTable table1 = dataSet2.Tables[0];
        if (table1.Rows.Count > 0)
        {
            sb.Append("<br/><br/><table width='50%' class='innertbltxt'><tr><td><b>Account Head</b></td><td align='right'><b>Balance</b></td></tr>");
            sb.Append("<tr><td colspan='2' style='border-top:dotted 1px black;'></td></tr>");
            string str2 = table1.Rows[0]["AcctsHeadId"].ToString();
            string str3 = table1.Rows[0]["AcctsHead"].ToString();
            for (int index = 0; index < table1.Rows.Count; ++index)
            {
                if (str2 == table1.Rows[index]["AcctsHeadId"].ToString())
                {
                    if (table1.Rows[index]["CrDr"].ToString().Trim().ToUpper() == "CR")
                        num2 = Convert.ToDecimal(table1.Rows[index]["transamt"].ToString());
                    else
                        num1 = Convert.ToDecimal(table1.Rows[index]["transamt"].ToString());
                }
                else
                {
                    Decimal num5;
                    string str4;
                    if (num2 > num1)
                    {
                        num5 = num2 - num1;
                        str4 = "Cr";
                        num4 = num1 - num2 + num4;
                    }
                    else
                    {
                        num5 = num1 - num2;
                        str4 = "Dr";
                        num4 = num5 + num4;
                    }
                    sb.Append("<tr><td>" + str3 + "</td><td align='right'>" + num5.ToString("C", (IFormatProvider)format) + " <i>" + str4 + "</i></td></tr>");
                    Decimal num6;
                    num3 = num6 = new Decimal(0);
                    num1 = num6;
                    num2 = num6;
                    str1 = "";
                    str2 = table1.Rows[index]["AcctsHeadId"].ToString();
                    str3 = table1.Rows[index]["AcctsHead"].ToString();
                    if (table1.Rows[index]["CrDr"].ToString().Trim().ToUpper() == "CR")
                        num2 = Convert.ToDecimal(table1.Rows[index]["transamt"].ToString());
                    else
                        num1 = Convert.ToDecimal(table1.Rows[index]["transamt"].ToString());
                }
            }
            Decimal num7;
            string str5;
            Decimal num8;
            if (num2 > num1)
            {
                num7 = num2 - num1;
                str5 = "Cr";
                num8 = num1 - num2 + num4;
            }
            else
            {
                num7 = num1 - num2;
                str5 = "Dr";
                num8 = num7 + num4;
            }
            sb.Append("<tr><td>" + str3 + "</td><td align='right'>" + num7.ToString("C", (IFormatProvider)format) + " <i>" + str5 + "</i></td></tr>");
            sb.Append("<tr><td colspan='2' style='border-top:dotted 1px black;'></td></tr>");
            sb.Append("<tr><td><b>Total</b></td><td align='right'><b>" + num8.ToString("C", (IFormatProvider)format));
            if (num8.ToString().Substring(0, 1) == "-")
                sb.Append(" <i> Cr </i></b></td></tr></table>");
            else
                sb.Append(" <i> Dr </i></b></td></tr></table>");
            Decimal num9;
            num3 = num9 = new Decimal(0);
            str1 = "";
        }
        dataSet2.Tables.Count.ToString();
        DataTable table2 = dataSet2.Tables[1];
        if (table2.Rows.Count > 0)
        {
            StringBuilder stringBuilder2 = new StringBuilder();
            StringBuilder stringBuilder3 = new StringBuilder();
            foreach (DataRow row in (InternalDataCollectionBase)table2.Rows)
            {
                if (row["Payment_Receipt"].ToString().Trim() == "P")
                {
                    stringBuilder2.Append("<tr><td>" + row["AcctsHead"].ToString() + "</td>");
                    stringBuilder2.Append("<td align='right'>" + Convert.ToDecimal(row["Pending"].ToString()).ToString("0.00") + "</td></tr>");
                }
                else
                {
                    stringBuilder3.Append("<tr><td>" + row["AcctsHead"].ToString() + "</td>");
                    stringBuilder3.Append("<td align='right'>" + Convert.ToDecimal(row["Pending"].ToString()).ToString("0.00") + "</td></tr>");
                }
            }
            if (stringBuilder2.ToString() != "")
            {
                sb.Append("<br/><table width='50%' class='innertbltxt'>");
                sb.Append("<tr><td colspan='2' style='border-top:dotted 1px black;'></td></tr>");
                sb.Append("<tr><td colspan='2' ></td></tr>");
                sb.Append("<tr><td colspan='2' align='left'><strong><u>Paid Amount To Be Encashed</u></Strong></td></tr>");
                sb.Append("<tr><td colspan='2' align='center'></td></tr>");
                sb.Append(stringBuilder2.ToString());
                sb.Append("</table>");
            }
            if (stringBuilder3.ToString() != "")
            {
                sb.Append("<br/><table width='50%' class='innertbltxt'>");
                sb.Append("<tr><td colspan='2' style='border-top:dotted 1px black;'></td></tr>");
                sb.Append("<tr><td colspan='2' ></td></tr>");
                sb.Append("<tr><td colspan='2' align='left'><strong><u>Received Amount To Be Encashed</u></Strong></td></tr>");
                sb.Append("<tr><td colspan='2' align='center'></td></tr>");
                sb.Append(stringBuilder3.ToString());
                sb.Append("</table>");
            }
        }
        DataTable table3 = dataSet2.Tables[2];
        if (table3.Rows.Count > 0)
        {
            string str2 = "";
            Decimal num5 = new Decimal(0);
            int index = 0;
            sb.Append("<br/><table width='50%' class='innertbltxt'>");
            sb.Append("<tr><td colspan='2' style='border-top:dotted 1px black;'></td></tr>");
            sb.Append("<tr><td colspan='2' ></td></tr>");
            sb.Append("<tr><td colspan='3' align='left'><strong><u>Loan To Be Paid(Liability)</u></Strong></td></tr>");
            sb.Append("<tr><td colspan='3' align='center'></td></tr>");
            foreach (DataRow row in (InternalDataCollectionBase)table3.Rows)
            {
                if (row["AccountHead"].ToString().Trim() == str2)
                {
                    if (row["CrDr"].ToString().Trim().ToUpper() == "CR")
                        num5 -= Convert.ToDecimal(row["Amount"].ToString());
                    else
                        num5 += Convert.ToDecimal(row["Amount"].ToString());
                    ++index;
                    sb.Append("<tr><td>" + row["AcctsHead"].ToString() + "</td>");
                    num5 *= new Decimal(-1);
                    sb.Append("<td align='right'>" + Convert.ToDecimal(num5.ToString()).ToString("0.00") + "</td></tr>");
                }
                else
                {
                    if (row["CrDr"].ToString().Trim().ToUpper() == "CR")
                        num5 -= Convert.ToDecimal(row["Amount"].ToString());
                    else
                        num5 += Convert.ToDecimal(row["Amount"].ToString());
                    str2 = row["AccountHead"].ToString().Trim();
                }
            }
            if (index == 0 && num5 < new Decimal(0))
            {
                sb.Append("<tr><td>" + table3.Rows[index]["AcctsHead"] + "</td>");
                if (num5 < new Decimal(0))
                    sb.Append("<td align='right'>" + Convert.ToDecimal((num5 * new Decimal(-1)).ToString()).ToString("0.00") + "</td></tr>");
                else
                    sb.Append("<td align='right'>" + Convert.ToDecimal(num5.ToString()).ToString("0.00") + "</td></tr>");
            }
            else if (num5 < new Decimal(0))
                sb.Append("<tr><td>NIL</td></tr>");
            sb.Append("</table>");
        }
        DataTable table4 = dataSet2.Tables[3];
        if (table4.Rows.Count > 0)
        {
            sb.Append("<br/><table width='50%' class='innertbltxt'>");
            sb.Append("<tr><td colspan='2' style='border-top:dotted 1px black;'></td></tr>");
            sb.Append("<tr><td colspan='2' ></td></tr>");
            sb.Append("<tr><td colspan='3' align='left'><strong><u>Loan/Advance To Be Recovered(Assects)</u></Strong></td></tr>");
            sb.Append("<tr><td colspan='3' align='center'></td></tr>");
            foreach (DataRow row in (InternalDataCollectionBase)table4.Rows)
            {
                sb.Append("<tr><td>" + row["AcctsHead"].ToString() + "</td>");
                sb.Append("<td align='right'>" + Convert.ToDecimal(row["Amount"].ToString()).ToString("0.00") + "</td></tr>");
            }
            sb.Append("</table>");
        }
        return sb;
    }

    protected void btnShowConsol_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptCashLedger.aspx");
    }
}