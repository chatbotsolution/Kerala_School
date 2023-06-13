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
public partial class Accounts_rptCashLedger : System.Web.UI.Page
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
        double num1 = 0.0;
        double num2 = 0.0;
        StringBuilder sb = new StringBuilder();
        ArrayList arrayList1 = new ArrayList();
        ArrayList arrayList2 = new ArrayList();
        ArrayList arrayList3 = new ArrayList();
        ArrayList arrayList4 = new ArrayList();
        ArrayList arrayList5 = new ArrayList();
        ArrayList arrayList6 = new ArrayList();
        ArrayList arrayList7 = new ArrayList();
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
            double num3 = Convert.ToDouble(obj.ExecuteScalar("ACTS_GetOpningBal", new Hashtable()
      {
        {
           "@FY",
           FY
        }
      }));
            DataTable dataTableQry2 = obj.GetDataTableQry("select * from dbo.ACTS_GenLedger l inner join dbo.Acts_AccountHeads a on a.AcctsHeadId=l.AccountHead inner join dbo.Acts_AccountGroups ag on ag.AG_Code=a.AG_Code where TransType<>'I' and (a.AG_Code in (1,7) or ag.AG_Parent=7) and (TransDate >='" + Convert.ToDateTime("01 APR" + FY.Substring(0, 4)).ToString("dd MMM yyyy") + "' and TransDate<'" + PopCalendar1.GetDateValue().ToString("dd MMM yyyy") + "') order by TransDate asc");
            double num4;
            if (dataTableQry2.Rows.Count > 0)
            {
                for (int index = 0; index < dataTableQry2.Rows.Count; ++index)
                {
                    if (dataTableQry2.Rows[index]["CrDr"].ToString().Trim().ToUpper() == "DR")
                        num1 += Convert.ToDouble(dataTableQry2.Rows[index]["TransAmt"]);
                    else
                        num2 += Convert.ToDouble(dataTableQry2.Rows[index]["TransAmt"]);
                }
                num3 = num3 - num1 + num2;
                num1 = 0.0;
                num2 = 0.0;
                num4 = num3;
            }
            else
                num4 = num3;
            Hashtable hashtable = new Hashtable();
            if (txtstartdate.Text.Trim() != "")
                hashtable.Add("FrmDt", PopCalendar1.GetDateValue().ToString("dd MMM yyyy 00:00:00"));
            if (txtenddate.Text.Trim() != "")
                hashtable.Add("ToDt", PopCalendar2.GetDateValue().ToString("dd MMM yyyy 00:00:00"));
            DataTable dataTable = obj.GetDataTable("Acts_GetCashBook", hashtable);
            if (dataTable.Rows.Count > 0)
            {
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                {
                    arrayList2.Add(dataTable.Rows[index]["AcctsHead"].ToString());
                    arrayList7.Add(dataTable.Rows[index]["TransType"].ToString());
                    if (dataTable.Rows[index]["CrDr"].ToString().Trim().ToUpper() == "CR")
                    {
                        arrayList3.Add(Convert.ToDouble(dataTable.Rows[index]["TransAmt"]));
                        num1 += Convert.ToDouble(dataTable.Rows[index]["TransAmt"]);
                        arrayList4.Add("0.00");
                        num4 -= Convert.ToDouble(dataTable.Rows[index]["TransAmt"]);
                        arrayList5.Add(num4);
                    }
                    else
                    {
                        arrayList4.Add(Convert.ToDouble(dataTable.Rows[index]["TransAmt"]));
                        num2 += Convert.ToDouble(dataTable.Rows[index]["TransAmt"]);
                        arrayList3.Add("0.00");
                        num4 += Convert.ToDouble(dataTable.Rows[index]["TransAmt"]);
                        arrayList5.Add(num4);
                    }
                    btnprnt.Visible = true;
                }
            }
            double num5 = num3 - num1 + num2;
            string str1;
            if (num3 < 0.0)
            {
                str1 = "Dr.";
                num3 = Convert.ToDouble(num3.ToString().Replace("-", ""));
            }
            else
                str1 = "Cr.";
            sb.Append("<table width='98%' class='innertbltxt'><tr><td colspan='5'></td></tr>");
            sb.Append("<tr><td colspan='6' height='2px'></td></tr>");
            sb.Append("<tr><td colspan='6' align='center' class='pageSectionLabel' style= 'border-bottom:dotted 1px black;'><b>Cash Book From:&nbsp;  " + txtstartdate.Text + " &nbsp;&nbsp;&nbsp;To:&nbsp;  " + txtenddate.Text + "</b></td></tr>");
            sb.Append("<tr><td colspan='6' height='2px'>&nbsp;</td></tr>");
            sb.Append("<tr><td colspan='4' height='2px' align='right'><b>Opening Balance: " + str1 + "</b></td>");
            sb.Append("<td align='right' ><b>" + Convert.ToDecimal(num3).ToString("C", (IFormatProvider)format) + "</b></td>");
            sb.Append("<tr>");
            sb.Append("<tr><td colspan='6' height='2px'></td></tr>");
            sb.Append("<td width='5%'><b>Sno.</b></td>");
            sb.Append("<td width='45%'><b>TXN. Details, If Any.</b></td>");
            sb.Append("<td ><b>Vch Type</b></td>");
            sb.Append("<td  align='right'><b>Debit</b></td>");
            sb.Append("<td  align='right'><b>Credit</b></td>");
            sb.Append("</tr>");
            for (int index = 0; index < arrayList2.Count; ++index)
            {
                sb.Append("<tr>");
                sb.Append("<td width='5%'>" + Convert.ToString(index + 1) + "</td>");
                sb.Append("<td width='45%' >" + arrayList2[index].ToString() + "</td>");
                if (arrayList7[index].ToString().Trim() == "SS")
                    sb.Append("<td >Receipt</td>");
                else if (arrayList7[index].ToString().Trim() == "P")
                    sb.Append("<td >Receipt</td>");
                else if (arrayList7[index].ToString().Trim() == "E")
                    sb.Append("<td >Payment</td>");
                else if (arrayList7[index].ToString().Trim() == "RC")
                    sb.Append("<td >Receipt Cancel</td>");
                else if (arrayList7[index].ToString().Trim() == "R")
                    sb.Append("<td >Payment</td>");
                else
                    sb.Append("<td></td>");
                sb.Append("<td  align='right'>" + Convert.ToDecimal(arrayList3[index]).ToString("C", (IFormatProvider)format) + "</td>");
                sb.Append("<td  align='right'>" + Convert.ToDecimal(arrayList4[index]).ToString("C", (IFormatProvider)format) + "</td>");
                sb.Append("</tr>");
            }
            sb.Append("<tr>");
            sb.Append("<td width='2%'></td>");
            sb.Append("<td width='10%'></td>");
            sb.Append("<td width='40%'></td>");
            sb.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
            sb.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
            sb.Append("<td width='10%' align='right'></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td width='2%'></td>");
            sb.Append("<td width='10%'></td>");
            sb.Append("<td width='10%' align='right'><b>Total -:</b></td>");
            sb.Append("<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>" + Convert.ToDecimal(num1).ToString("C", (IFormatProvider)format) + "</b></td>");
            sb.Append("<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>" + Convert.ToDecimal(num2).ToString("C", (IFormatProvider)format) + "</b></td>");
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
            string str2;
            if (num5 < 0.0)
            {
                num5 = Math.Round(Convert.ToDouble(num5.ToString().Replace("-", "")), 2);
                str2 = "Dr.";
            }
            else
                str2 = "Cr.";
            sb.Append("<tr><td colspan='4' height='2px' align='right'><b>Closing Balance: " + str2 + "</b></td>");
            sb.Append("<td align='right'><b>" + Convert.ToDecimal(num5).ToString("C", (IFormatProvider)format) + "</b></td>");
            sb.Append("<tr>");
            sb.Append("</table>");
            Literal1.Text = ShowSplit(sb, FY).ToString();
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

    protected void btnShowDtls_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptCashBookNew.aspx");
    }

    protected void btnSplit_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptCashBookSplit.aspx");
    }
}