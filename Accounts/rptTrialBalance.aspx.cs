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

public partial class Accounts_rptTrialBalance : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private NumberFormatInfo format = new NumberFormatInfo();
    private int[] indSizes = new int[4] { 3, 2, 2, 2 };
    protected void Page_Load(object sender, EventArgs e)
    {
        btnprnt.Visible = false;
        if (!Page.IsPostBack)
            PopCalendar1.SetDateValue(DateTime.Now);
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
        double num4 = 0.0;
        double num5 = 0.0;
        StringBuilder stringBuilder = new StringBuilder();
        ArrayList arrayList1 = new ArrayList();
        ArrayList arrayList2 = new ArrayList();
        ArrayList arrayList3 = new ArrayList();
        ArrayList arrayList4 = new ArrayList();
        ArrayList arrayList5 = new ArrayList();
        ArrayList arrayList6 = new ArrayList();
        ArrayList arrayList7 = new ArrayList();
        ArrayList arrayList8 = new ArrayList();
        double num6 = 0.0;
        double num7 = 0.0;
        double num8 = 0.0;
        double num9 = 0.0;
        DataTable dataTableQry = obj.GetDataTableQry("select FY,StartDate from ACTS_FinancialYear where StartDate <='" + PopCalendar1.GetDateValue().ToString("MM/dd/yyyy") + "' and EndDate >='" + PopCalendar1.GetDateValue().ToString("MM/dd/yyyy") + "' ");
        if (dataTableQry.Rows.Count == 0)
        {
            lblMsg.Text = "Selected year is not valid financial year...";
            lblMsg.ForeColor = Color.Red;
            Literal1.Text = "";
        }
        else
        {
            dataTableQry.Rows[0]["FY"].ToString();
            DateTime dateTime = Convert.ToDateTime(dataTableQry.Rows[0]["StartDate"].ToString());
            DataTable dataTable1 = obj.GetDataTable("ACTS_GetAcHdsForTrlBal");
            if (dataTable1.Rows.Count > 0)
            {
                for (int index1 = 0; index1 < dataTable1.Rows.Count; ++index1)
                {
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("@AccHd", dataTable1.Rows[index1]["AcctsHeadId"].ToString());
                    hashtable.Add("@StrtDt", dateTime);
                    hashtable.Add("@ToDt", PopCalendar1.GetDateValue().ToString("dd MMM yyyy 23:59:59"));
                    if (dataTable1.Rows[index1]["ExpAccHd"].ToString() != "")
                        hashtable.Add("@ExpAccHd", dataTable1.Rows[index1]["ExpAccHd"].ToString());
                    DataTable dataTable2 = obj.GetDataTable("ACTS_GetAmtForTrlBal", hashtable);
                    DataTable dataTable3 = new DataTable();
                    if (dataTable1.Rows[index1]["ExpAccHd"].ToString() == "")
                    {
                        //"Select TransAmt,CrDr from dbo.Acts_GenLedger where AccountHead='" + dataTable1.Rows[index1]["AcctsHeadId"].ToString() + "' and TransDate>='" + dateTime + "' and TransDate<='" + PopCalendar1.GetDateValue().ToString("MM/dd/yyyy 23:59:59") + "' and TransType = 'I' order by TransDate asc";
                        dataTable3 = obj.GetDataTableQry("Select TransAmt,CrDr from dbo.Acts_GenLedger where AccountHead='" + dataTable1.Rows[index1]["AcctsHeadId"].ToString() + "' and TransDate>='" + dateTime + "' and TransDate<='" + PopCalendar1.GetDateValue().ToString("MM/dd/yyyy 23:59:59") + "' and TransType = 'I' order by TransDate asc");
                    }
                    if (dataTable2.Rows.Count > 0 || dataTable3.Rows.Count > 0)
                    {
                        string str1 = "";
                        string str2 = "";
                        if (dataTable3.Rows.Count > 0)
                        {
                            str2 = dataTable3.Rows[0]["CrDr"].ToString();
                            if (dataTable3.Rows[0]["TransAmt"] != "")
                                str1 = dataTable3.Rows[0]["TransAmt"].ToString();
                        }
                        if (str1 == "")
                            str1 = "0";
                        arrayList6.Add(str1);
                        arrayList8.Add(str2);
                        for (int index2 = 0; index2 < dataTable2.Rows.Count; ++index2)
                        {
                            if (dataTable2.Rows[index2]["CrDr"].ToString().Trim().ToUpper() == "DR")
                                num2 = Convert.ToDouble(num2 + Convert.ToDouble(dataTable2.Rows[index2]["TransAmt"]));
                            else
                                num3 = Convert.ToDouble(num3 + Convert.ToDouble(dataTable2.Rows[index2]["TransAmt"]));
                        }
                        double num10 = Math.Round(num2, 2);
                        double num11 = Math.Round(num3, 2);
                        arrayList4.Add(num10);
                        arrayList5.Add(num11);
                        arrayList3.Add(dataTable1.Rows[index1]["AcctsHead"].ToString());
                        num2 = num3 = 0.0;
                    }
                }
                stringBuilder.Append("<table width='100%' style='font-size:12px;'><tr><td colspan='6'></td></tr>");
                stringBuilder.Append("<tr><td colspan='6' height='2px'></td></tr>");
                stringBuilder.Append("<tr><td colspan='6' align='center' class='pageSectionLabel' style= 'border-bottom:dotted 1px black;'><b>Trial Balance as on " + txtstartdate.Text + "</b></td></tr>");
                stringBuilder.Append("<tr><td colspan='6' height='2px'>&nbsp;</td></tr>");
                stringBuilder.Append("<tr><td colspan='6' height='2px'></td></tr>");
                stringBuilder.Append("<tr><td width='5px' valign='top'><b><u>Sno.</u></b></td>");
                stringBuilder.Append("<td  width='200px'><b><u>Account Head</u></b></td>");
                stringBuilder.Append("<td width='110px' align='right'><b><u>Opening Bal</u></b></td>");
                stringBuilder.Append("<td width='110px' align='right'><b><u>Debit</u></b></td>");
                stringBuilder.Append("<td width='100px' align='right'><b><u>Credit</u></b></td>");
                stringBuilder.Append("<td width='100px' align='right'><b><u>Closing Bal</u></b></td>");
                stringBuilder.Append("</tr>");
                for (int index = 0; index < arrayList4.Count; ++index)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td >" + Convert.ToString(index + 1) + "</td>");
                    stringBuilder.Append("<td >" + arrayList3[index].ToString() + "</td>");
                    if (Convert.ToDecimal(arrayList6[index]).ToString("C", (IFormatProvider)format) == "0.00")
                        stringBuilder.Append("<td  align='right'></td>");
                    else
                        stringBuilder.Append("<td width='20%' align='right'>" + Convert.ToDecimal(arrayList6[index]).ToString("C", (IFormatProvider)format) + " <i>" + arrayList8[index].ToString() + "</i></td>");
                    stringBuilder.Append("<td  align='right'>" + Convert.ToDecimal(arrayList4[index]).ToString("C", (IFormatProvider)format) + "</td>");
                    stringBuilder.Append("<td  align='right'>" + Convert.ToDecimal(arrayList5[index]).ToString("C", (IFormatProvider)format) + "</td>");
                    string str1 = "";
                    double num10;
                    string str2;
                    if (Convert.ToDouble(arrayList4[index]) > Convert.ToDouble(arrayList5[index]))
                    {
                        num10 = Convert.ToDouble(arrayList4[index]) - Convert.ToDouble(arrayList5[index]);
                        str2 = "Dr";
                        if (Convert.ToDecimal(arrayList6[index]).ToString("C", (IFormatProvider)format) != "0.00")
                        {
                            if (arrayList8[index].ToString() == "CR")
                            {
                                if (Convert.ToDouble(arrayList6[index]) > num10)
                                {
                                    num10 = Convert.ToDouble(arrayList6[index]) - num10;
                                    str2 = "Cr";
                                }
                                else if (Convert.ToDouble(arrayList6[index]) < num10)
                                {
                                    num10 -= Convert.ToDouble(arrayList6[index]);
                                    str2 = "Dr";
                                }
                                else
                                {
                                    num10 -= Convert.ToDouble(arrayList6[index]);
                                    str2 = "";
                                }
                            }
                            else if (Convert.ToDouble(arrayList6[index]) > num10)
                            {
                                num10 = Convert.ToDouble(arrayList6[index]) + num10;
                                str2 = "Dr";
                            }
                            else if (Convert.ToDouble(arrayList6[index]) < num10)
                            {
                                num10 += Convert.ToDouble(arrayList6[index]);
                                str2 = "Dr";
                            }
                            else
                            {
                                num10 -= Convert.ToDouble(arrayList6[index]);
                                str2 = "";
                            }
                        }
                    }
                    else if (Convert.ToDouble(arrayList5[index]) > Convert.ToDouble(arrayList4[index]))
                    {
                        num10 = Convert.ToDouble(arrayList5[index]) - Convert.ToDouble(arrayList4[index]);
                        str2 = "Cr";
                        if (Convert.ToDecimal(arrayList6[index]).ToString("C", (IFormatProvider)format) != "0.00")
                        {
                            if (arrayList8[index].ToString() == "CR")
                            {
                                if (Convert.ToDouble(arrayList6[index]) > num10)
                                {
                                    num10 = Convert.ToDouble(arrayList6[index]) + num10;
                                    str2 = "Cr";
                                }
                                else if (Convert.ToDouble(arrayList6[index]) < num10)
                                {
                                    num10 += Convert.ToDouble(arrayList6[index]);
                                    str2 = "Cr";
                                }
                                else
                                {
                                    num10 -= Convert.ToDouble(arrayList6[index]);
                                    str2 = "";
                                }
                            }
                            else if (Convert.ToDouble(arrayList6[index]) > num10)
                            {
                                num10 = Convert.ToDouble(arrayList6[index]) - num10;
                                str2 = "Dr";
                            }
                            else if (Convert.ToDouble(arrayList6[index]) < num10)
                            {
                                num10 -= Convert.ToDouble(arrayList6[index]);
                                str2 = "Cr";
                            }
                            else
                            {
                                num10 -= Convert.ToDouble(arrayList6[index]);
                                str2 = "";
                            }
                        }
                    }
                    else
                    {
                        str1 = "";
                        num10 = Convert.ToDouble(arrayList6[index]);
                        str2 = arrayList8[index].ToString();
                    }
                    if (num10 == 0.0)
                    {
                        stringBuilder.Append("<td  align='right'></td>");
                    }
                    else
                    {
                        if (str2 == "Cr")
                            num6 += num10;
                        else
                            num7 += num10;
                        stringBuilder.Append("<td  align='right'>" + Convert.ToDecimal(num10).ToString("C", (IFormatProvider)format) + " <i>" + str2 + "</i></td>");
                    }
                    stringBuilder.Append("</tr>");
                    num5 += Convert.ToDouble(arrayList4[index]);
                    num4 += Convert.ToDouble(arrayList5[index]);
                    if (arrayList8[index].ToString() == "CR")
                        num8 += Convert.ToDouble(arrayList6[index]);
                    else
                        num9 += Convert.ToDouble(arrayList6[index]);
                }
                if (num8 != num9)
                    ;
                if (num6 != num7)
                    ;
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td></td>");
                stringBuilder.Append("<td ></td>");
                stringBuilder.Append("<td align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                stringBuilder.Append("<td  align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                stringBuilder.Append("<td   align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                stringBuilder.Append("<td   align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td ></td>");
                stringBuilder.Append("<td  align='right'><b>Total -:</b></td>");
                stringBuilder.Append("<td ' align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                stringBuilder.Append("<td  align='right' style= 'border-bottom:dotted 1px black;'><b>" + Convert.ToDecimal(num5).ToString("C", (IFormatProvider)format) + "</b></td>");
                stringBuilder.Append("<td  align='right' style= 'border-bottom:dotted 1px black;'><b>" + Convert.ToDecimal(num4).ToString("C", (IFormatProvider)format) + "</b></td>");
                stringBuilder.Append("<td ' align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                stringBuilder.Append("</tr>");
                if (num1 < 0.0)
                    Math.Round(Convert.ToDouble(num1.ToString().Replace("-", "")), 2);
                stringBuilder.Append("</table>");
                Literal1.Text = stringBuilder.ToString();
                if (Math.Round(Convert.ToDouble(num5), 2) != Math.Round(Convert.ToDouble(num4), 2))
                {
                    lblMsg.Text = "Sum of debit and credit amount is not equal";
                    lblMsg.ForeColor = Color.Red;
                }
                btnprnt.Visible = true;
            }
            else
            {
                lblMsg.Text = "No Record Found !";
                lblMsg.ForeColor = Color.Red;
                Literal1.Text = "";
            }
        }
    }
}