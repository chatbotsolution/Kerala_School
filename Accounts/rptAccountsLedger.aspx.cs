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

public partial class Accounts_rptAccountsLedger : System.Web.UI.Page
{
    private NumberFormatInfo format = new NumberFormatInfo();
    private int[] indSizes = new int[4] { 3, 2, 2, 2 };
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        drpAccHead.Focus();
        FillAcGroup();
        fillacchead();
        txtstartdate.Text = DateTime.Now.Month <= 3 ? new DateTime(DateTime.Now.Year - 1, 4, 1).ToString("dd-MM-yyyy") : new DateTime(DateTime.Now.Year, 4, 1).ToString("dd-MM-yyyy");
        txtenddate.Text = DateTime.Now.ToString("dd-MM-yyyy");
    }

    private void fillacchead()
    {
        drpAccHead.Items.Clear();
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpAcGroup.SelectedIndex > 0)
            hashtable.Add("@AgCode", drpAcGroup.SelectedValue.ToString().Trim());
        DataTable dataTable = clsDal.GetDataTable("ACTS_GetAccountHds", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            drpAccHead.DataSource = dataTable;
            drpAccHead.DataTextField = "AcctsHead";
            drpAccHead.DataValueField = "AcctsHeadId";
            drpAccHead.DataBind();
            drpAccHead.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        else
            drpAccHead.Items.Insert(0, new ListItem("N0 DATA FOUND", "0"));
    }

    private void FillAcGroup()
    {
        DataTable dataTable = new clsDAL().GetDataTable("ACTS_GetAccountGroups");
        if (dataTable.Rows.Count > 0)
        {
            drpAcGroup.DataSource = dataTable;
            drpAcGroup.DataTextField = "AG_Name";
            drpAcGroup.DataValueField = "AG_Code";
            drpAcGroup.DataBind();
            drpAcGroup.Items.Insert(0, new ListItem("--ALL--", "0"));
        }
        else
            drpAccHead.Items.Insert(0, new ListItem("N0 DATA FOUND", "0"));
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        format.CurrencyDecimalDigits = 2;
        format.CurrencyDecimalSeparator = ".";
        format.CurrencyGroupSeparator = ",";
        format.CurrencySymbol = "";
        format.CurrencyGroupSizes = indSizes;
        btnprnt.Visible = true;
        double num1 = 0.0;
        double num2 = 0.0;
        string str1 = "";
        StringBuilder stringBuilder1 = new StringBuilder();
        ArrayList arrayList1 = new ArrayList();
        ArrayList arrayList2 = new ArrayList();
        ArrayList arrayList3 = new ArrayList();
        ArrayList arrayList4 = new ArrayList();
        ArrayList arrayList5 = new ArrayList();
        DataTable dataTableQry1 = obj.GetDataTableQry("select FY from ACTS_FinancialYear where StartDate <='" + dtpStart.GetDateValue().ToString("MM/dd/yyyy") + "' and EndDate >='" + dtpEnd.GetDateValue().ToString("MM/dd/yyyy") + "'");
        if (dataTableQry1.Rows.Count == 0)
        {
            lblMsg.Text = "Selected year is not valid financial year....";
            lblMsg.ForeColor = Color.Red;
            Literal1.Text = "";
        }
        else if (drpFeeHead.Visible && drpFeeHead.SelectedIndex > 0)
        {
            DataSet dataSet1 = new DataSet();
            Hashtable hashtable = new Hashtable();
            DataTable dataTable = new DataTable();
            obj = new clsDAL();
            int num3 = 1;
            hashtable.Add("@FromDt", dtpStart.GetDateValue().ToString("dd MMM yyyy"));
            hashtable.Add("@ToDt", dtpEnd.GetDateValue().ToString("dd MMM yyyy"));
            hashtable.Add("@FeeId", drpFeeHead.SelectedValue.ToString());
            DataSet dataSet2 = obj.GetDataSet("ACTS_GetFeeLedgerAcc", hashtable);
            double num4 = Convert.ToDouble(dataSet2.Tables[1].Rows[0]["ExpAmnt"]) - Convert.ToDouble(dataSet2.Tables[0].Rows[0]["FeeAmnt"]);
            DataTable table = dataSet2.Tables[2];
            stringBuilder1.Append("<table width='100%' class='innertbltxt'><tr><td colspan='5'></td></tr>");
            stringBuilder1.Append("<tr><td colspan='5' height='2px'></td></tr>");
            stringBuilder1.Append("<tr><td colspan='5' align='center' class='pageSectionLabel' style= 'border-bottom:dotted 1px black;'><b>Account Ledger for " + drpFeeHead.SelectedItem.Text.ToString().Trim() + "  From:&nbsp;  " + txtstartdate.Text + " &nbsp;&nbsp;&nbsp;To:&nbsp;  " + txtenddate.Text + "</b></td></tr>");
            stringBuilder1.Append("<tr><td colspan='5' height='2px'>&nbsp;</td></tr>");
            string str2 = num4 >= 0.0 ? "Dr." : "Cr.";
            stringBuilder1.Append("<tr><td colspan='4' height='2px' align='right'><b>Opening Balance: " + str2 + "</b></td>");
            stringBuilder1.Append("<td align='right' width='10%'><b>" + Convert.ToDecimal(num4.ToString().Replace("-", "")).ToString("C", (IFormatProvider)format) + "</b></td>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<tr><td colspan='5' height='2px'></td></tr>");
            stringBuilder1.Append("<td width='5%'><b>Sno.</b></td>");
            stringBuilder1.Append("<td width='15%'><b>Date</b></td>");
            stringBuilder1.Append("<td width='50%'><b>Particulars</b></td>");
            stringBuilder1.Append("<td width='15%' align='right'><b>Debit</b></td>");
            stringBuilder1.Append("<td width='15%' align='right'><b>Credit</b></td>");
            foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
            {
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td width='5%'>" + num3 + "</td>");
                stringBuilder1.Append("<td width='15%'>" + row["TransDateStr"] + "</td>");
                stringBuilder1.Append("<td width='50%' >" + row["Particulars"] + "</td>");
                if (row["CrDr"].ToString().Trim() == "CR")
                {
                    stringBuilder1.Append("<td align='right'>0.00</td><td width='15%' align='right'>" + Convert.ToDecimal(row["Amount"]).ToString("C", (IFormatProvider)format) + "</td>");
                    num2 += Convert.ToDouble(row["Amount"]);
                }
                else
                {
                    stringBuilder1.Append("<td width='15%' align='right'>" + Convert.ToDecimal(row["Amount"]).ToString("C", (IFormatProvider)format) + "</td><td align='right'>0.00</td>");
                    num1 += Convert.ToDouble(row["Amount"]);
                }
                ++num3;
            }
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td width='2%'></td>");
            stringBuilder1.Append("<td width='10%'></td>");
            stringBuilder1.Append("<td width='30%'></td>");
            stringBuilder1.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
            stringBuilder1.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
            stringBuilder1.Append("<td width='10%' align='right'></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td width='2%'></td>");
            stringBuilder1.Append("<td width='10%' align='right'></td>");
            stringBuilder1.Append("<td width='10%' align='right'><b>Total -:</b></td>");
            stringBuilder1.Append("<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>" + Convert.ToDecimal(num1).ToString("C", (IFormatProvider)format) + "</b></td>");
            stringBuilder1.Append("<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>" + Convert.ToDecimal(num2).ToString("C", (IFormatProvider)format) + "</b></td>");
            stringBuilder1.Append("<td width='10%' align='right'></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td width='2%'></td>");
            stringBuilder1.Append("<td width='10%'></td>");
            stringBuilder1.Append("<td width='30%'></td>");
            stringBuilder1.Append("<td width='10%'></td>");
            stringBuilder1.Append("<td width='10%'  align='right'>&nbsp;</td>");
            stringBuilder1.Append("<td width='10%'  align='right'>&nbsp;</td>");
            stringBuilder1.Append("<td width='10%' align='right'></td>");
            stringBuilder1.Append("</tr>");
            double num5 = num4 + num1 - num2;
            string str3;
            if (num5 < 0.0)
            {
                num5 = Math.Round(Convert.ToDouble(num5.ToString().Replace("-", "")), 2);
                str3 = "Cr.";
            }
            else
                str3 = "Dr.";
            stringBuilder1.Append("<tr><td colspan='4' height='2px' align='right'><b>Closing Balance: " + str3 + "</b></td>");
            stringBuilder1.Append("<td align='right'><b>" + Convert.ToDecimal(num5).ToString("C", (IFormatProvider)format) + "</b></td>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("</table>");
            Literal1.Text = stringBuilder1.ToString();
        }
        else if (CheckExp().Trim() != "")
        {
            DataSet dataSet1 = new DataSet();
            Hashtable hashtable = new Hashtable();
            DataTable dataTable = new DataTable();
            obj = new clsDAL();
            int num3 = 1;
            hashtable.Add("@FromDt", dtpStart.GetDateValue().ToString("dd MMM yyyy"));
            hashtable.Add("@ToDt", dtpEnd.GetDateValue().ToString("dd MMM yyyy"));
            hashtable.Add("@AdId", CheckExp().Trim());
            DataSet dataSet2 = obj.GetDataSet("ACTS_GetLedgerWithExp", hashtable);
            double num4 = Convert.ToDouble(dataSet2.Tables[1].Rows[0]["ExpAmnt"]) - Convert.ToDouble(dataSet2.Tables[0].Rows[0]["IncAmnt"]);
            DataTable table = dataSet2.Tables[2];
            stringBuilder1.Append("<table width='100%' class='innertbltxt'><tr><td colspan='5'></td></tr>");
            stringBuilder1.Append("<tr><td colspan='5' height='2px'></td></tr>");
            stringBuilder1.Append("<tr><td colspan='5' align='center' class='pageSectionLabel' style= 'border-bottom:dotted 1px black;'><b>Account Ledger for " + drpAccHead.SelectedItem.Text.ToString().Trim() + "  From:&nbsp;  " + txtstartdate.Text + " &nbsp;&nbsp;&nbsp;To:&nbsp;  " + txtenddate.Text + "</b></td></tr>");
            stringBuilder1.Append("<tr><td colspan='5' height='2px'>&nbsp;</td></tr>");
            string str2 = num4 >= 0.0 ? "Dr." : "Cr.";
            stringBuilder1.Append("<tr><td colspan='4' height='2px' align='right'><b>Opening Balance: " + str2 + "</b></td>");
            stringBuilder1.Append("<td align='right' width='10%'><b>" + Convert.ToDecimal(num4.ToString().Replace("-", "")).ToString("C", (IFormatProvider)format) + "</b></td>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<tr><td colspan='5' height='2px'></td></tr>");
            stringBuilder1.Append("<td width='5%'><b>Sno.</b></td>");
            stringBuilder1.Append("<td width='15%'><b>Date</b></td>");
            stringBuilder1.Append("<td width='50%'><b>Particulars</b></td>");
            stringBuilder1.Append("<td width='15%' align='right'><b>Debit</b></td>");
            stringBuilder1.Append("<td width='15%' align='right'><b>Credit</b></td>");
            foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
            {
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td width='5%'>" + num3 + "</td>");
                stringBuilder1.Append("<td width='15%'>" + row["TransDateStr"] + "</td>");
                stringBuilder1.Append("<td width='50%' >" + row["Particulars"] + "</td>");
                Decimal num5;
                if (row["CrDr"].ToString().Trim() == "CR")
                {
                    StringBuilder stringBuilder2 = stringBuilder1;
                    string str3 = "<td align='right'>0.00</td><td width='15%' align='right'>";
                    num5 = Convert.ToDecimal(row["Amount"]);
                    string str4 = num5.ToString("C", (IFormatProvider)format);
                    string str5 = "</td>";
                    string str6 = str3 + str4 + str5;
                    stringBuilder2.Append(str6);
                    num2 += Convert.ToDouble(row["Amount"]);
                }
                else
                {
                    StringBuilder stringBuilder2 = stringBuilder1;
                    string str3 = "<td width='15%' align='right'>";
                    num5 = Convert.ToDecimal(row["Amount"]);
                    string str4 = num5.ToString("C", (IFormatProvider)format);
                    string str5 = "</td><td align='right'>0.00</td>";
                    string str6 = str3 + str4 + str5;
                    stringBuilder2.Append(str6);
                    num1 += Convert.ToDouble(row["Amount"]);
                }
                ++num3;
            }
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td width='2%'></td>");
            stringBuilder1.Append("<td width='10%'></td>");
            stringBuilder1.Append("<td width='30%'></td>");
            stringBuilder1.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
            stringBuilder1.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
            stringBuilder1.Append("<td width='10%' align='right'></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td width='2%'></td>");
            stringBuilder1.Append("<td width='10%' align='right'></td>");
            stringBuilder1.Append("<td width='10%' align='right'><b>Total -:</b></td>");
            StringBuilder stringBuilder3 = stringBuilder1;
            string str7 = "<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>";
            Decimal num6 = Convert.ToDecimal(num1);
            string str8 = num6.ToString("C", (IFormatProvider)format);
            string str9 = "</b></td>";
            string str10 = str7 + str8 + str9;
            stringBuilder3.Append(str10);
            StringBuilder stringBuilder4 = stringBuilder1;
            string str11 = "<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>";
            num6 = Convert.ToDecimal(num2);
            string str12 = num6.ToString("C", (IFormatProvider)format);
            string str13 = "</b></td>";
            string str14 = str11 + str12 + str13;
            stringBuilder4.Append(str14);
            stringBuilder1.Append("<td width='10%' align='right'></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td width='2%'></td>");
            stringBuilder1.Append("<td width='10%'></td>");
            stringBuilder1.Append("<td width='30%'></td>");
            stringBuilder1.Append("<td width='10%'></td>");
            stringBuilder1.Append("<td width='10%'  align='right'>&nbsp;</td>");
            stringBuilder1.Append("<td width='10%'  align='right'>&nbsp;</td>");
            stringBuilder1.Append("<td width='10%' align='right'></td>");
            stringBuilder1.Append("</tr>");
            double num7 = num4 + num1 - num2;
            string str15;
            if (num7 < 0.0)
            {
                num7 = Math.Round(Convert.ToDouble(num7.ToString().Replace("-", "")), 2);
                str15 = "Cr.";
            }
            else
                str15 = "Dr.";
            stringBuilder1.Append("<tr><td colspan='4' height='2px' align='right'><b>Closing Balance: " + str15 + "</b></td>");
            StringBuilder stringBuilder5 = stringBuilder1;
            string str16 = "<td align='right'><b>";
            num6 = Convert.ToDecimal(num7);
            string str17 = num6.ToString("C", (IFormatProvider)format);
            string str18 = "</b></td>";
            string str19 = str16 + str17 + str18;
            stringBuilder5.Append(str19);
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("</table>");
            Literal1.Text = stringBuilder1.ToString();
        }
        else
        {
            string str2 = dataTableQry1.Rows[0]["FY"].ToString();
            DataTable dataTableQry2 = new clsDAL().GetDataTableQry("select AG_ALIE from ACTS_AccountGroups inner join ACTS_AccountHeads on ACTS_AccountHeads.AG_Code=ACTS_AccountGroups.AG_Code where AcctsHeadId='" + drpAccHead.SelectedValue + "'");
            if (dataTableQry2.Rows.Count > 0)
                dataTableQry2.Rows[0]["AG_ALIE"].ToString();
            DataTable dataTableQry3 = new clsDAL().GetDataTableQry("select sum(TransAmt) as TransAmt,CrDr from dbo.ACTS_GenLedger where SessionYr='" + str2 + "' and TransType='I' and AccountHead='" + drpAccHead.SelectedValue + "' group By CrDr");
            double num3;
            if (dataTableQry3.Rows.Count > 0)
            {
                if (Convert.ToDouble(dataTableQry3.Rows[0]["TransAmt"]) == 0.0)
                {
                    str1 = "CR";
                    num3 = 0.0;
                }
                else
                {
                    string str3 = dataTableQry3.Rows[0]["CrDr"].ToString();
                    num3 = Convert.ToDouble(dataTableQry3.Rows[0]["TransAmt"]);
                    if (str3.ToString().Trim().ToUpper() == "DR")
                        num3 = -1.0 * num3;
                }
            }
            else
                num3 = 0.0;
            clsDAL clsDal1 = new clsDAL();
            DateTime dateTime1 = Convert.ToDateTime("01 APR " + str2.Substring(0, 4));
            DataTable dataTable = new DataTable();
            clsDAL clsDal2 = clsDal1;
            string[] strArray1 = new string[7]
      {
        "select * from dbo.ACTS_GenLedger where  TransType<>'I' and AccountHead='",
        drpAccHead.SelectedValue,
        "' and (TransDate >='",
        dateTime1.ToString("MM/dd/yyyy"),
        "' and TransDate<'",
        null,
        null
      };
            string[] strArray2 = strArray1;
            int index1 = 5;
            DateTime dateTime2 = dtpStart.GetDateValue();
            string str4 = dateTime2.ToString("MM/dd/yyyy");
            strArray2[index1] = str4;
            strArray1[6] = "') order by TransDate,GenLedgerId asc";
            string str5 = string.Concat(strArray1);
            DataTable dataTableQry4 = clsDal2.GetDataTableQry(str5);
            double num4;
            if (dataTableQry4.Rows.Count > 0)
            {
                for (int index2 = 0; index2 < dataTableQry4.Rows.Count; ++index2)
                {
                    if (dataTableQry4.Rows[index2]["CrDr"].ToString().Trim().ToUpper() == "DR")
                        num1 += Convert.ToDouble(dataTableQry4.Rows[index2]["TransAmt"]);
                    else
                        num2 += Convert.ToDouble(dataTableQry4.Rows[index2]["TransAmt"]);
                }
                num3 = num3 - num1 + num2;
                num1 = 0.0;
                num2 = 0.0;
                num4 = num3;
            }
            else
                num4 = num3;
            clsDAL clsDal3 = new clsDAL();
            new DataTable().Clear();
            string[] strArray3 = new string[7];
            strArray3[0] = "select g.*,isnull(g.PmtRecptVoucherNo,'') as ChekNum,pr.InstrumentNo from dbo.ACTS_GenLedger g left join dbo.Acts_PaymentReceiptVoucher pr on  pr.PR_Id=g.PR_Id where  g.TransDate  between '";
            string[] strArray4 = strArray3;
            int index3 = 1;
            dateTime2 = dtpStart.GetDateValue();
            string str6 = dateTime2.ToString("MM/dd/yyyy 00:00:00");
            strArray4[index3] = str6;
            strArray3[2] = "' and '";
            string[] strArray5 = strArray3;
            int index4 = 3;
            dateTime2 = dtpEnd.GetDateValue();
            string str7 = dateTime2.ToString("MM/dd/yyyy 23:59:59");
            strArray5[index4] = str7;
            strArray3[4] = "' and  g.TransType<>'I' and (pr.IsDeleted is null or pr.IsDeleted=0) and g.AccountHead='";
            strArray3[5] = drpAccHead.SelectedValue;
            strArray3[6] = "' and g.Particulars not like '%Cancelled%' order by g.TransDate,g.GenLedgerId asc";
            string str8 = string.Concat(strArray3);
            DataTable dataTableQry5 = clsDal3.GetDataTableQry(str8);
            if (dataTableQry5.Rows.Count > 0)
            {
                for (int index2 = 0; index2 < dataTableQry5.Rows.Count; ++index2)
                {
                    ArrayList arrayList6 = arrayList1;
                    dateTime2 = Convert.ToDateTime(dataTableQry5.Rows[index2]["TransDate"]);
                    string str3 = dateTime2.ToString("dd/MM/yyyy");
                    arrayList6.Add(str3);
                    arrayList2.Add(dataTableQry5.Rows[index2]["Particulars"].ToString());
                    if (dataTableQry5.Rows[index2]["CrDr"].ToString().Trim().ToUpper() == "DR")
                    {
                        arrayList3.Add(Convert.ToDouble(dataTableQry5.Rows[index2]["TransAmt"]));
                        num1 += Convert.ToDouble(dataTableQry5.Rows[index2]["TransAmt"]);
                        arrayList4.Add("0.00");
                        num4 -= Convert.ToDouble(dataTableQry5.Rows[index2]["TransAmt"]);
                        arrayList5.Add(num4);
                    }
                    else
                    {
                        arrayList4.Add(Convert.ToDouble(dataTableQry5.Rows[index2]["TransAmt"]));
                        num2 += Convert.ToDouble(dataTableQry5.Rows[index2]["TransAmt"]);
                        arrayList3.Add("0.00");
                        num4 += Convert.ToDouble(dataTableQry5.Rows[index2]["TransAmt"]);
                        arrayList5.Add(num4);
                    }
                    btnprnt.Visible = true;
                }
            }
            double num5 = num3 - num1 + num2;
            string str9;
            if (num3 < 0.0)
            {
                str9 = "Dr.";
                num3 = Convert.ToDouble(num3.ToString().Replace("-", ""));
            }
            else
                str9 = "Cr.";
            stringBuilder1.Append("<table width='100%' class='innertbltxt'><tr><td colspan='5'></td></tr>");
            stringBuilder1.Append("<tr><td colspan='5' height='2px'></td></tr>");
            stringBuilder1.Append("<tr><td colspan='5' align='center' class='pageSectionLabel' style= 'border-bottom:dotted 1px black;'><b>Account Ledger for " + drpAccHead.SelectedItem.Text.ToString().Trim() + "  From:&nbsp;  " + txtstartdate.Text + " &nbsp;&nbsp;&nbsp;To:&nbsp;  " + txtenddate.Text + "</b></td></tr>");
            stringBuilder1.Append("<tr><td colspan='5' height='2px'>&nbsp;</td></tr>");
            stringBuilder1.Append("<tr><td colspan='5' height='2px' align='right'><b>Opening Balance: " + str9 + "</b></td>");
            stringBuilder1.Append("<td align='right' width='10%'><b>" + Convert.ToDecimal(num3).ToString("C", (IFormatProvider)format) + "</b></td>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<tr><td colspan='5' height='2px'></td></tr>");
            stringBuilder1.Append("<td width='5%'><b>Sno.</b></td>");
            stringBuilder1.Append("<td width='15%'><b>Date</b></td>");
            stringBuilder1.Append("<td width='50%'><b>Particulars</b></td>");
            stringBuilder1.Append("<td width='15%' align='right'><b>Cash/Bank(Inst No)</b></td>");
            stringBuilder1.Append("<td width='15%' align='right'><b>Debit</b></td>");
            stringBuilder1.Append("<td width='15%' align='right'><b>Credit</b></td>");
            stringBuilder1.Append("</tr>");
            Decimal num6;
            for (int index2 = 0; index2 < arrayList1.Count; ++index2)
            {
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td width='5%'>" + Convert.ToString(index2 + 1) + "</td>");
                stringBuilder1.Append("<td width='15%'>" + arrayList1[index2].ToString() + "</td>");
                stringBuilder1.Append("<td width='50%' >" + arrayList2[index2].ToString() + "</td>");
                if (dataTableQry5.Rows[index2]["BankTransId"] == null || dataTableQry5.Rows[index2]["BankTransId"].ToString().Trim() == "")
                {
                    if (dataTableQry5.Rows[index2]["InstrumentNo"] != null && dataTableQry5.Rows[index2]["InstrumentNo"].ToString().Trim() != "")
                        stringBuilder1.Append("<td width='15%' align='right'>Bank(" + dataTableQry5.Rows[index2]["InstrumentNo"].ToString() + ")</td>");
                    else if (dataTableQry5.Rows[index2]["PR_Id"].ToString().Trim() != "" && dataTableQry5.Rows[index2]["PR_Id"] != null)
                        stringBuilder1.Append("<td width='15%' align='right'>Cash</td>");
                    else
                        stringBuilder1.Append("<td width='15%' align='right'>&nbsp;</td>");
                }
                else
                    stringBuilder1.Append("<td width='15%' align='right'>Bank</td>");
                StringBuilder stringBuilder2 = stringBuilder1;
                string str3 = "<td width='15%' align='right'>";
                num6 = Convert.ToDecimal(arrayList3[index2]);
                string str10 = num6.ToString("C", (IFormatProvider)format);
                string str11 = "</td>";
                string str12 = str3 + str10 + str11;
                stringBuilder2.Append(str12);
                StringBuilder stringBuilder3 = stringBuilder1;
                string str13 = "<td width='15%' align='right'>";
                num6 = Convert.ToDecimal(arrayList4[index2]);
                string str14 = num6.ToString("C", (IFormatProvider)format);
                string str15 = "</td>";
                string str16 = str13 + str14 + str15;
                stringBuilder3.Append(str16);
                stringBuilder1.Append("</tr>");
            }
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td width='5%'></td>");
            stringBuilder1.Append("<td width='15%'></td>");
            stringBuilder1.Append("<td width='50%'></td>");
            stringBuilder1.Append("<td width='10%'></td>");
            stringBuilder1.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
            stringBuilder1.Append("<td width='10%'  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
            stringBuilder1.Append("<td width='10%' align='right'></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td width='2%'></td>");
            stringBuilder1.Append("<td width='10%'></td>");
            stringBuilder1.Append("<td width='10%' align='right'></td>");
            stringBuilder1.Append("<td width='10%' align='right'><b>Total -:</b></td>");
            StringBuilder stringBuilder4 = stringBuilder1;
            string str17 = "<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>";
            num6 = Convert.ToDecimal(num1);
            string str18 = num6.ToString("C", (IFormatProvider)format);
            string str19 = "</b></td>";
            string str20 = str17 + str18 + str19;
            stringBuilder4.Append(str20);
            StringBuilder stringBuilder5 = stringBuilder1;
            string str21 = "<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>";
            num6 = Convert.ToDecimal(num2);
            string str22 = num6.ToString("C", (IFormatProvider)format);
            string str23 = "</b></td>";
            string str24 = str21 + str22 + str23;
            stringBuilder5.Append(str24);
            stringBuilder1.Append("<td width='10%' align='right'></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td width='5%'></td>");
            stringBuilder1.Append("<td width='15%'></td>");
            stringBuilder1.Append("<td width='50%'></td>");
            stringBuilder1.Append("<td width='10%'></td>");
            stringBuilder1.Append("<td width='10%'  align='right'>&nbsp;</td>");
            stringBuilder1.Append("<td width='10%'  align='right'>&nbsp;</td>");
            stringBuilder1.Append("<td width='10%' align='right'></td>");
            stringBuilder1.Append("</tr>");
            string str25;
            if (num5 < 0.0)
            {
                num5 = Math.Round(Convert.ToDouble(num5.ToString().Replace("-", "")), 2);
                str25 = "Dr.";
            }
            else
                str25 = "Cr.";
            stringBuilder1.Append("<tr><td colspan='5' height='2px' align='right'><b>Closing Balance: " + str25 + "</b></td>");
            StringBuilder stringBuilder6 = stringBuilder1;
            string str26 = "<td align='right'><b>";
            num6 = Convert.ToDecimal(num5);
            string str27 = num6.ToString("C", (IFormatProvider)format);
            string str28 = "</b></td>";
            string str29 = str26 + str27 + str28;
            stringBuilder6.Append(str29);
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("</table>");
            Literal1.Text = stringBuilder1.ToString();
        }
    }

    private string CheckExp()
    {
        obj = new clsDAL();
        string str = "";
        if (drpAccHead.SelectedIndex > 0)
            str = obj.ExecuteScalarQry("select Ad_Id from dbo.PS_AdditionalFeeMaster where Ad_Id in (1,2,12,13) and AcctsHeadId=" + drpAccHead.SelectedValue.ToString().Trim());
        return str;
    }

    protected void drpAccHead_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpAccHead.SelectedValue.ToString().Trim() == obj.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=5").Trim())
        {
            DataTable dataTable = new DataTable();
            drpFeeHead.DataSource = obj.GetDataTableQry("select FeeName,FeeID from dbo.PS_FeeComponents order by FeeName");
            drpFeeHead.DataTextField = "FeeName";
            drpFeeHead.DataValueField = "FeeID";
            drpFeeHead.DataBind();
            drpFeeHead.Items.Insert(0, new ListItem("--Select School Fee Head--", "0"));
            drpFeeHead.Visible = true;
            lblFee.Visible = true;
        }
        else
        {
            drpFeeHead.Visible = false;
            lblFee.Visible = false;
        }
    }

    protected void drpAcGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillacchead();
        lblFee.Visible = false;
        drpFeeHead.Visible = false;
    }
}