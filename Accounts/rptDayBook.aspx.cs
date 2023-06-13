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

public partial class Accounts_rptDayBook : System.Web.UI.Page
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
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        string str1 = "";
        string str2 = "";
        StringBuilder stringBuilder = new StringBuilder();
        ArrayList arrayList1 = new ArrayList();
        ArrayList arrayList2 = new ArrayList();
        ArrayList arrayList3 = new ArrayList();
        ArrayList arrayList4 = new ArrayList();
        ArrayList arrayList5 = new ArrayList();
        ArrayList arrayList6 = new ArrayList();
        ArrayList arrayList7 = new ArrayList();
        DataTable dataTableQry1 = obj.GetDataTableQry("select FY from ACTS_FinancialYear where StartDate <='" + PopCalendar1.GetDateValue().ToString("MM/dd/yyyy") + "' and EndDate >='" + PopCalendar2.GetDateValue().ToString("MM/dd/yyyy") + "'");
        if (dataTableQry1.Rows.Count == 0)
        {
            lblMsg.Text = "Selected year is not valid financial year...";
            lblMsg.ForeColor = Color.Red;
            Literal1.Text = "";
        }
        else
        {
            string str3 = dataTableQry1.Rows[0]["FY"].ToString();
            DataTable dataTableQry2 = obj.GetDataTableQry("select AcctsHeadId from ACTS_AccountHeads where AcctsHeadId=3");
            if (dataTableQry2.Rows.Count > 0)
            {
                for (int index = 0; index < dataTableQry2.Rows.Count; ++index)
                    str2 = str2 + dataTableQry2.Rows[index]["AcctsHeadId"].ToString() + ",";
                str2 = str2.Substring(0, str2.LastIndexOf(","));
            }
            DataTable dataTableQry3 = obj.GetDataTableQry("select * from dbo.ACTS_GenLedger where SessionYr='" + str3 + "' and TransType='I' and AccountHead in (" + str2 + ") order by TransDate asc");
            double num4;
            if (dataTableQry3.Rows.Count > 0)
            {
                if (Convert.ToDouble(dataTableQry3.Rows[0]["TransAmt"]) == 0.0)
                {
                    str1 = "CR";
                    num4 = 0.0;
                }
                else
                {
                    str1 = dataTableQry3.Rows[0]["CrDr"].ToString();
                    num4 = Convert.ToDouble(dataTableQry3.Rows[0]["TransAmt"]);
                }
            }
            else
                num4 = 0.0;
            DateTime dateTime = Convert.ToDateTime("01 APR 2014");
            DataTable dataTableQry4 = obj.GetDataTableQry("select * from dbo.ACTS_GenLedger where  TransType<>'I' and AccountHead in(" + str2 + ") and (TransDate >='" + dateTime.ToString("MM/dd/yyyy") + "' and TransDate<'" + PopCalendar1.GetDateValue().ToString("MM/dd/yyyy") + "') order by TransDate asc");
            if (dataTableQry4.Rows.Count > 0)
            {
                for (int index = 0; index < dataTableQry4.Rows.Count; ++index)
                {
                    if (dataTableQry4.Rows[index]["CrDr"].ToString().Trim().ToUpper() == "DR")
                        num2 += Convert.ToDouble(dataTableQry4.Rows[index]["TransAmt"]);
                    else
                        num1 += Convert.ToDouble(dataTableQry4.Rows[index]["TransAmt"]);
                }
                num4 = num4 - num1 + num2;
                num1 = 0.0;
                num2 = 0.0;
                num3 = num4;
            }
            Hashtable hashtable = new Hashtable();
            if (txtstartdate.Text.Trim() != "")
                hashtable.Add("FrmDt", PopCalendar1.GetDateValue().ToString("MM/dd/yyyy 00:00:00"));
            if (txtenddate.Text.Trim() != "")
                hashtable.Add("ToDt", PopCalendar2.GetDateValue().ToString("MM/dd/yyyy 00:00:00"));
            DataTable dataTable = obj.GetDataTable("Acts_GetDayBook", hashtable);
            if (dataTable.Rows.Count > 0)
            {
                for (int index = 0; index < dataTable.Rows.Count; ++index)
                {
                    arrayList2.Add(dataTable.Rows[index]["Particulars"].ToString());
                    arrayList4.Add(dataTable.Rows[index]["AcctsHead"].ToString());
                    if (dataTable.Rows[index]["CrDr"].ToString().Trim().ToUpper() == "DR")
                    {
                        arrayList6.Add(Convert.ToDouble(dataTable.Rows[index]["TransAmt"]));
                        num1 += Convert.ToDouble(dataTable.Rows[index]["TransAmt"]);
                        arrayList7.Add("0.00");
                        num3 += Convert.ToDouble(dataTable.Rows[index]["TransAmt"]);
                        arrayList3.Add(num3);
                    }
                    else
                    {
                        arrayList7.Add(Convert.ToDouble(dataTable.Rows[index]["TransAmt"]));
                        num2 += Convert.ToDouble(dataTable.Rows[index]["TransAmt"]);
                        arrayList6.Add("0.00");
                        num3 -= Convert.ToDouble(dataTable.Rows[index]["TransAmt"]);
                        arrayList3.Add(num3);
                        btnprnt.Visible = true;
                    }
                }
                string str4;
                if (num4 < 0.0)
                {
                    str4 = "Dr.";
                    num4 = Convert.ToDouble(num4.ToString().Replace("-", ""));
                }
                else
                    str4 = "Cr.";
                stringBuilder.Append("<table width='100%' class='innertbltxt'><tr><td colspan='5'></td></tr>");
                stringBuilder.Append("<tr><td colspan='6' height='2px'></td></tr>");
                stringBuilder.Append("<tr><td colspan='6' align='center' class='pageSectionLabel' style= 'border-bottom:dotted 1px black;'><b>Day Book From:&nbsp;  " + txtstartdate.Text + " &nbsp;&nbsp;&nbsp;To:&nbsp;  " + txtenddate.Text + "</b></td></tr>");
                stringBuilder.Append("<tr><td colspan='6' height='2px'>&nbsp;</td></tr>");
                stringBuilder.Append("<tr><td colspan='4' height='2px' align='right'><b>Cash in Hand Opening Balance: " + str4 + "</b></td>");
                stringBuilder.Append("<td width='2%'></td>");
                stringBuilder.Append("<td align='right'><b>" + Convert.ToDecimal(num4).ToString("C", (IFormatProvider)format) + "</b></td>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<tr><td colspan='6' height='2px'></td></tr>");
                stringBuilder.Append("<td width='4%'><b>Sno.</b></td>");
                stringBuilder.Append("<td width='30%'><b>Account Head</b></td>");
                stringBuilder.Append("<td width='38%'><b>TXN. Details, If Any.</b></td>");
                stringBuilder.Append("<td width='8%' align='right'><b>Debit</b></td>");
                stringBuilder.Append("<td width='1%'></td>");
                stringBuilder.Append("<td width='10%' align='right'><b>Credit</b></td>");
                stringBuilder.Append("</tr>");
                for (int index = 0; index < arrayList4.Count; ++index)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td width='5%'>" + Convert.ToString(index + 1) + "</td>");
                    if (arrayList4[index].ToString().Trim() == "STOCK IN TRADE(PURCHASE)" && dataTable.Rows[index]["TransType"].ToString() != "PP")
                        stringBuilder.Append("<td width='20%'>STOCK IN TRADE(SALES)</td>");
                    else
                        stringBuilder.Append("<td width='20%'>" + arrayList4[index].ToString() + "</td>");
                    stringBuilder.Append("<td width='38%' >" + arrayList2[index].ToString() + "</td>");
                    stringBuilder.Append("<td width='8%' align='right'>" + Convert.ToDecimal(arrayList6[index]).ToString("C", (IFormatProvider)format) + "</td>");
                    stringBuilder.Append("<td width='1%'></td>");
                    stringBuilder.Append("<td width='10%' align='right'>" + Convert.ToDecimal(arrayList7[index]).ToString("C", (IFormatProvider)format) + "</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td width='5%'></td>");
                stringBuilder.Append("<td width='10%'></td>");
                stringBuilder.Append("<td width='30%'></td>");
                stringBuilder.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                stringBuilder.Append("<td width='2%'></td>");
                stringBuilder.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td width='5%'></td>");
                stringBuilder.Append("<td width='20%'></td>");
                stringBuilder.Append("<td width='30%' align='right' ><b>Total -:</b></td>");
                stringBuilder.Append("<td width='10%' align='right' style= 'border-bottom:dotted 1px black;'><b>" + Convert.ToDecimal(num1).ToString("C", (IFormatProvider)format) + "</b></td>");
                stringBuilder.Append("<td width='5%'></td>");
                stringBuilder.Append("<td width='10%' align='right' style= 'border-bottom:dotted 1px black;'><b>" + Convert.ToDecimal(num2).ToString("C", (IFormatProvider)format) + "</b></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td width='5%'></td>");
                stringBuilder.Append("<td width='20%'></td>");
                stringBuilder.Append("<td width='10%'></td>");
                stringBuilder.Append("<td width='30%'></td>");
                stringBuilder.Append("<td width='10%'  align='right'>&nbsp;</td>");
                stringBuilder.Append("<td width='2%'></td>");
                stringBuilder.Append("<td width='10%'  align='right'>&nbsp;</td>");
                stringBuilder.Append("</tr>");
                double num5 = 0.0;
                double num6 = 0.0;
                DataTable dataTableQry5 = obj.GetDataTableQry("select *,isnull(PmtRecptVoucherNo,'') as ChekNum from dbo.ACTS_GenLedger l inner join dbo.Acts_AccountHeads a on a.AcctsHeadId=l.AccountHead where  TransDate  between '" + PopCalendar1.GetDateValue().ToString("MM/dd/yyyy") + "' and '" + PopCalendar2.GetDateValue().ToString("MM/dd/yyyy") + "' and TransType<>'I' and (AccountHead in(" + str2 + ")or a.AG_Code=7)and PR_Id is not null order by TransDate asc");
                if (dataTableQry5.Rows.Count > 0)
                {
                    for (int index = 0; index < dataTableQry5.Rows.Count; ++index)
                    {
                        if (dataTableQry5.Rows[index]["CrDr"].ToString().Trim().ToUpper() == "DR")
                            num5 += Convert.ToDouble(dataTableQry5.Rows[index]["TransAmt"]);
                        else
                            num6 += Convert.ToDouble(dataTableQry5.Rows[index]["TransAmt"]);
                    }
                }
                double num7 = !(str4 == "Dr.") ? num4 - num6 + num5 : num4 * -1.0 - num6 + num5;
                string str5;
                if (num7 < 0.0)
                {
                    num7 = Math.Round(Convert.ToDouble(num7.ToString().Replace("-", "")), 2);
                    str5 = "Dr.";
                }
                else
                    str5 = "Cr.";
                stringBuilder.Append("<tr><td colspan='4' height='2px' align='right'><b>Cash in Hand Closing Balance: " + str5 + "</b></td>");
                stringBuilder.Append("<td width='2%'></td>");
                stringBuilder.Append("<td align='right'><b>" + Convert.ToDecimal(num7).ToString("C", (IFormatProvider)format) + "</b></td>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("</table>");
                Literal1.Text = stringBuilder.ToString();
                btnprnt.Visible = true;
            }
            else
            {
                lblMsg.Text = "No Record Found !";
                lblMsg.ForeColor = Color.Red;
                Literal1.Text = "";
                btnprnt.Visible = false;
            }
        }
    }
}