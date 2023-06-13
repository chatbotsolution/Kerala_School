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


public partial class Accounts_rptCashBookNew : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private NumberFormatInfo format = new NumberFormatInfo();
    private int[] indSizes = new int[4] { 3, 2, 2, 2 };
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
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
        double num3 = 0.0;
        double num4 = 0.0;
        Decimal num5 = new Decimal(0);
        Decimal num6 = new Decimal(0);
        StringBuilder sb = new StringBuilder();
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
            double num7 = Convert.ToDouble(obj.ExecuteScalar("ACTS_GetOpningBal", new Hashtable()
      {
        {
           "@FY",
           FY
        }
      }));
            DataTable dataTableQry2 = obj.GetDataTableQry("select * from dbo.ACTS_GenLedger l inner join dbo.Acts_AccountHeads a on a.AcctsHeadId=l.AccountHead inner join dbo.Acts_AccountGroups ag on ag.AG_Code=a.AG_Code where TransType<>'I' and (a.AG_Code in (1,7) or ag.AG_Parent=7) and (TransDate >='" + Convert.ToDateTime("01 APR" + FY.Substring(0, 4)).ToString("dd MMM yyyy") + "' and TransDate<'" + PopCalendar1.GetDateValue().ToString("dd MMM yyyy") + "') order by TransDate asc");
            if (dataTableQry2.Rows.Count > 0)
            {
                for (int index = 0; index < dataTableQry2.Rows.Count; ++index)
                {
                    if (dataTableQry2.Rows[index]["CrDr"].ToString().Trim().ToUpper() == "DR")
                        num3 += Convert.ToDouble(dataTableQry2.Rows[index]["TransAmt"]);
                    else
                        num4 += Convert.ToDouble(dataTableQry2.Rows[index]["TransAmt"]);
                }
                num7 = num7 - num3 + num4;
            }
            string str1;
            if (num7 < 0.0)
            {
                str1 = "Dr.";
                string str2 = num7.ToString().Replace("-", "");
                num7 = Convert.ToDouble(str2);
                num1 = Convert.ToDecimal(str2);
            }
            else
            {
                str1 = "Cr.";
                num2 = Convert.ToDecimal(num7);
            }
            Hashtable hashtable1 = new Hashtable();
            if (txtstartdate.Text.Trim() != "")
                hashtable1.Add("FrmDt", PopCalendar1.GetDateValue().ToString("dd MMM yyyy 00:00:00"));
            if (txtenddate.Text.Trim() != "")
                hashtable1.Add("ToDt", PopCalendar2.GetDateValue().ToString("dd MMM yyyy 00:00:00"));
            DataTable dataTable1 = obj.GetDataTable("Acts_GetCashBookNew", hashtable1);
            if (dataTable1.Rows.Count > 0)
            {
                sb.Append("<table width='100%' class='innertbltxt'><tr><td colspan='7'></td></tr>");
                sb.Append("<tr><td colspan='7' height='4px'></td></tr>");
                sb.Append("<tr><td colspan='7' align='center' class='pageSectionLabel' style='font-size:16'><b>Cash Book</b></td></tr>");
                sb.Append("<tr><td colspan='7' height='2px'></td></tr>");
                sb.Append("<tr><td colspan='7' align='center' class='pageSectionLabel' style= 'border-bottom:dotted 1px black;'>" + PopCalendar1.GetDateValue().ToString("dd MMM yyyy") + " to " + PopCalendar2.GetDateValue().ToString("dd MMM yyyy") + "</td></tr>");
                sb.Append("<tr><td colspan='7' height='2px'>&nbsp;</td></tr>");
                sb.Append("<tr><td colspan='6' height='2px' align='right'><b>Opening Balance: " + str1 + "</b></td>");
                sb.Append("<td align='right' width='10%'><b>" + Convert.ToDecimal(num7).ToString("C", (IFormatProvider)format) + "</b></td></tr>");
                sb.Append("<tr>");
                sb.Append("<tr><td colspan='6' height='2px'></td></tr>");
                sb.Append("<td width='2%'><b>Sno.</b></td>");
                sb.Append("<td width='4%'><b>Date</b></td>");
                sb.Append("<td width='25%'><b>Account Head</b></td>");
                sb.Append("<td width='45%' ><b>Particulars</b></td>");
                sb.Append("<td width='4%'><b>Vch Type</b></td>");
                sb.Append("<td width='10%' align='right'><b>Debit</b></td>");
                sb.Append("<td width='10%' align='right'><b>Credit</b></td>");
                sb.Append("</tr>");
                int num8 = 1;
                string str2 = dataTable1.Rows[0]["TransDate"].ToString();
                foreach (DataRow row in (InternalDataCollectionBase)dataTable1.Rows)
                {
                    if (str2 != row["TransDate"].ToString())
                    {
                        Hashtable hashtable2 = new Hashtable();
                        obj = new clsDAL();
                        DataTable dataTable2 = new DataTable();
                        hashtable2.Add("@Date", Convert.ToDateTime(str2).ToString("dd MMM yyyy"));
                        DataTable dataTable3 = obj.GetDataTable("Acts_GetCrDrSumDtWise", hashtable2);
                        Decimal num9;
                        Decimal num10;
                        if (dataTable3.Rows[0]["CrDr"].ToString().ToUpper() == "DR")
                        {
                            num9 = Convert.ToDecimal(dataTable3.Rows[0]["Amount"].ToString());
                            num10 = dataTable3.Rows.Count <= 1 ? new Decimal(0) : Convert.ToDecimal(dataTable3.Rows[1]["Amount"].ToString());
                        }
                        else
                        {
                            num10 = Convert.ToDecimal(dataTable3.Rows[0]["Amount"].ToString());
                            num9 = dataTable3.Rows.Count <= 1 ? new Decimal(0) : Convert.ToDecimal(dataTable3.Rows[1]["Amount"].ToString());
                        }
                        sb.Append("<tr height='10px'>");
                        sb.Append("<td colspan='5'></td>");
                        sb.Append("<td valign='top' align='right' style='border-top:dashed 1; '>" + num9.ToString("C", (IFormatProvider)format) + "</td>");
                        sb.Append("<td valign='top' align='right' style='border-top:dashed 1; '>" + num10.ToString("C", (IFormatProvider)format) + "</td></tr>");
                        sb.Append("<tr height='10px'>");
                        sb.Append("<td width='2%' valign='top'></td>");
                        sb.Append("<td width='10%' valign='top'></td>");
                        sb.Append("<td valign='top' style='font:14' colspan='2'><b>By Closing Balance</b></td>");
                        sb.Append("<td width='10%' valign='top'></td>");
                        Decimal num11;
                        if (num9 >= num10)
                        {
                            num11 = num9 - num10;
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; '>" + num11.ToString("C", (IFormatProvider)format) + "</td>");
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; '></td></tr>");
                        }
                        else
                        {
                            num11 = num10 - num9;
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; '></td>");
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; '>" + num11.ToString("C", (IFormatProvider)format) + "</td></tr>");
                        }
                        str2 = row["TransDate"].ToString();
                        sb.Append("<tr height='10px'>");
                        sb.Append("<td width='2%' valign='top'></td>");
                        sb.Append("<td width='10%' valign='top'>" + Convert.ToDateTime(row["TransDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
                        sb.Append("<td valign='top' style='font:14' colspan='2'><b>To Opening Balance</b></td>");
                        sb.Append("<td width='10%' valign='top'></td>");
                        if (num11 > new Decimal(0))
                        {
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; border-bottom-style:inset'>" + num11.ToString("C", (IFormatProvider)format) + "</td>");
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; border-bottom-style:inset'></td></tr>");
                        }
                        else
                        {
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; border-bottom-style:inset'></td>");
                            sb.Append("<td width='15%' valign='top' style='border-top-style:inset; border-bottom-style:inset'>" + num11.ToString("C", (IFormatProvider)format) + "</td></tr>");
                        }
                    }
                    sb.Append("<tr>");
                    sb.Append("<td width='2%' valign='top'>" + num8++ + "</td>");
                    sb.Append("<td width='10%' valign='top'>" + Convert.ToDateTime(row["TransDate"].ToString()).ToString("dd/MM/yyyy") + "</td>");
                    sb.Append("<td >" + row["AcctsHead"] + "</td>");
                    sb.Append("<td >" + row["Particulars"] + "</td>");
                    if (row["TransType"].ToString().Trim() == "PP")
                        sb.Append("<td >Purchase</td>");
                    else if (row["TransType"].ToString().Trim() == "P")
                        sb.Append("<td >Payment</td>");
                    else if (row["TransType"].ToString().Trim() == "E")
                        sb.Append("<td >Payment</td>");
                    else if (row["TransType"].ToString().Trim() == "SS")
                        sb.Append("<td >Sales</td>");
                    else if (row["TransType"].ToString().Trim() == "R")
                        sb.Append("<td >Receipt</td>");
                    if (row["CrDr"].ToString().ToUpper() == "DR")
                    {
                        sb.Append("<td width='15%' align='right'>" + Convert.ToDecimal(row["TransAmt"]).ToString("C", (IFormatProvider)format) + "</td>");
                        sb.Append("<td width='15%' align='right'>0.00</td>");
                    }
                    else
                    {
                        sb.Append("<td width='15%' align='right'>0.00</td>");
                        sb.Append("<td width='15%' align='right'>" + Convert.ToDecimal(row["TransAmt"]).ToString("C", (IFormatProvider)format) + "</td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("<tr>");
                sb.Append("<td colspan='5'></td>");
                sb.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                sb.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='4'></td>");
                sb.Append("<td width='10%' align='right'><b>Total -:</b></td>");
                Hashtable hashtable3 = new Hashtable();
                obj = new clsDAL();
                DataTable dataTable4 = new DataTable();
                hashtable3.Add("@Date", PopCalendar2.GetDateValue().ToString("dd MMM yyyy 00:00:00"));
                hashtable3.Add("@FrmDt", PopCalendar1.GetDateValue().ToString("dd MMM yyyy 00:00:00"));
                DataTable dataTable5 = obj.GetDataTable("Acts_GetCrDrSumDtWise", hashtable3);
                double num12;
                if (dataTable5.Rows.Count > 0)
                {
                    Decimal num9;
                    Decimal num10;
                    if (dataTable5.Rows[0]["CrDr"].ToString().ToUpper() == "DR")
                    {
                        num9 = Convert.ToDecimal(dataTable5.Rows[0]["Amount"].ToString());
                        num10 = dataTable5.Rows.Count <= 1 ? new Decimal(0) : Convert.ToDecimal(dataTable5.Rows[1]["Amount"].ToString());
                    }
                    else
                    {
                        num10 = Convert.ToDecimal(dataTable5.Rows[0]["Amount"].ToString());
                        num9 = dataTable5.Rows.Count <= 1 ? new Decimal(0) : Convert.ToDecimal(dataTable5.Rows[1]["Amount"].ToString());
                    }
                    num12 = Convert.ToDouble(num9 - num10 + num1 - num2);
                    if (num12 < 0.0)
                    {
                        num12 = Math.Round(Convert.ToDouble(num12.ToString().Replace("-", "")), 2);
                        str1 = "Cr.";
                    }
                    else
                        str1 = "Dr.";
                    sb.Append("<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>" + num9.ToString("C", (IFormatProvider)format) + "</b></td>");
                    sb.Append("<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>" + num10.ToString("C", (IFormatProvider)format) + "</b></td>");
                }
                else
                {
                    sb.Append("<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>0.00</b></td>");
                    sb.Append("<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>0.00</b></td>");
                    num12 = num7;
                }
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td colspan='5'></td>");
                sb.Append("<td width='10%' align='right'>&nbsp;</td>");
                sb.Append("<td width='10%' align='right'>&nbsp;</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td colspan='6' height='2px' align='right'><b>Closing Balance: " + str1 + "</b></td>");
                sb.Append("<td align='right'><b>" + Convert.ToDecimal(num12).ToString("C", (IFormatProvider)format) + "</b></td>");
                sb.Append("<tr>");
                sb.Append("</table>");
                Literal1.Text = ShowSplit(sb, FY).ToString();
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
            sb.Append("<br/><table width='50%' class='innertbltxt'>");
            sb.Append("<tr><td colspan='2' style='border-top:dotted 1px black;'></td></tr>");
            sb.Append("<tr><td colspan='2' ></td></tr>");
            sb.Append("<tr><td colspan='3' align='left'><strong><u>Loan To Be Paid(Liability)</u></Strong></td></tr>");
            sb.Append("<tr><td colspan='3' align='center'></td></tr>");
            foreach (DataRow row in (InternalDataCollectionBase)table3.Rows)
            {
                sb.Append("<tr><td>" + row["AcctsHead"].ToString() + "</td>");
                sb.Append("<td align='right'>" + (Convert.ToDecimal(row["Amount"].ToString()) * new Decimal(-1)).ToString("0.00") + "</td></tr>");
            }
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

    protected void btnSplit_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptCashBookSplit.aspx");
    }
}