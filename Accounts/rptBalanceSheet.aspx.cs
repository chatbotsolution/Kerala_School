using ASP;
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
public partial class Accounts_rptBalanceSheet : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private NumberFormatInfo format = new NumberFormatInfo();
    private int[] indSizes = new int[4] { 3, 2, 2, 2 };
    protected void Page_Load(object sender, EventArgs e)
    {
        format.CurrencyDecimalDigits = 2;
        format.CurrencyDecimalSeparator = ".";
        format.CurrencyGroupSeparator = ",";
        format.CurrencySymbol = "";
        format.CurrencyGroupSizes = indSizes;
        if (!Page.IsPostBack)
        {
            BindSessionYr();
            if (drpSession.Items.Count > 0)
            {
                GenerateReport();
            }
            else
            {
                lblMsg.Text = "Please Initialize Financial Year To View Balance Sheet";
                lblMsg.ForeColor = Color.Red;
            }
        }
        lblMsg.Text = "";
    }

    private void GenerateReport()
    {
        Hashtable hashtable = new Hashtable();
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        string str1 = drpSession.SelectedValue.ToString().Substring(0, 4);
        hashtable.Add("@StDt", Convert.ToDateTime("31 Mar " + str1));
        hashtable.Add("@EndDt", Convert.ToDateTime("1 Apr " + (Convert.ToInt32(str1) + 1).ToString()));
        DataSet dataSet = obj.GetDataSet("ACTS_GetBalanceSheet", hashtable);
        DataTable table1 = dataSet.Tables[0];
        DataTable table2 = dataSet.Tables[1];
        DataTable table3 = dataSet.Tables[3];
        DataTable table4 = dataSet.Tables[4];
        DataTable table5 = dataSet.Tables[5];
        DataTable table6 = dataSet.Tables[6];
        DataTable table7 = dataSet.Tables[7];
        DataTable table8 = dataSet.Tables[8];
        DataTable table9 = dataSet.Tables[2];
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        StringBuilder stringBuilder3 = new StringBuilder();
        if (table3.Rows.Count <= 0 && table1.Rows.Count <= 0)
            return;
        stringBuilder1.Append("<table width='100%' class='innertbltxt'>");
        stringBuilder1.Append("<tr><td align='center' class='pageSectionLabel' style='font-size:16'><b>Balance Sheet</b></td></tr>");
        stringBuilder1.Append("<tr><td height='2px'></td></tr>");
        stringBuilder1.Append("<tr><td align='center' class='pageSectionLabel'>1-Apr-" + drpSession.SelectedValue.ToString().Substring(0, 4) + " to 31-Mar-" + (Convert.ToInt32(str1) + 1).ToString() + "</td></tr>");
        stringBuilder1.Append("<tr><td height='4px'></td></tr></table>");
        stringBuilder1.Append("<table border='1px' cellpadding='1' cellspacing='0' width='1000px' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder1.Append("<tr>");
        stringBuilder1.Append("<td style='border-right-style:none; font-size:14;' align='left' class='gridtext'><font color='Black'><b>Liabilities</b></font>");
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("<td style='border-right-style:none; font-size:14;' align='left' class='gridtext'><font color='Black'><b>Assets</b></font>");
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("</tr>");
        stringBuilder3.Append("<table width='100%'>");
        int index1 = 0;
        StringBuilder stringBuilder4 = new StringBuilder();
        Decimal num3 = new Decimal(0);
        if (table9.Rows.Count > 0)
        {
            foreach (DataRow row in (InternalDataCollectionBase)table9.Rows)
            {
                if (stringBuilder4.ToString().Trim() == "" && table8.Rows[0]["TransAmtStr"].ToString().Trim() != "" && Convert.ToDecimal(table8.Rows[0]["TransAmtStr"].ToString().Trim()) < new Decimal(0))
                {
                    stringBuilder4.Append("<tr>");
                    stringBuilder4.Append("<td align='left' class='gridtext' style='padding-left:10px'>" + row["AcctsHead"].ToString() + "</td>");
                    stringBuilder4.Append("<td align='right' class='gridtext' >" + (Convert.ToDecimal(row["TransAmtStr"]) + Convert.ToDecimal(table8.Rows[0]["TransAmtStr"].ToString().Trim())).ToString("C", (IFormatProvider)format) + "</td>");
                    stringBuilder4.Append("<td width='100px'></td>");
                    stringBuilder4.Append("</tr>");
                    num1 = num1 + Convert.ToDecimal(row["TransAmtStr"].ToString().Trim()) + Convert.ToDecimal(table8.Rows[0]["TransAmtStr"].ToString().Trim());
                    num3 = num3 + Convert.ToDecimal(row["TransAmtStr"].ToString().Trim()) + Convert.ToDecimal(table8.Rows[0]["TransAmtStr"].ToString().Trim());
                }
                else
                {
                    stringBuilder4.Append("<tr>");
                    stringBuilder4.Append("<td align='left' class='gridtext' style='padding-left:10px'>" + row["AcctsHead"].ToString() + "</td>");
                    if (Convert.ToDecimal(row["TransAmtStr"]) < new Decimal(0))
                        stringBuilder4.Append("<td align='right' class='gridtext' >" + Convert.ToDecimal(Convert.ToDecimal(row["TransAmtStr"]) * new Decimal(-1)).ToString("C", (IFormatProvider)format) + "</td>");
                    else
                        stringBuilder4.Append("<td align='right' class='gridtext' >" + Convert.ToDecimal(row["TransAmtStr"]).ToString("C", (IFormatProvider)format) + "</td>");
                    stringBuilder4.Append("<td width='100px'></td>");
                    stringBuilder4.Append("</tr>");
                    num1 += Convert.ToDecimal(row["TransAmtStr"].ToString().Trim());
                    num3 += Convert.ToDecimal(row["TransAmtStr"].ToString().Trim());
                }
            }
        }
        if (table8.Rows[0]["TransAmtStr"].ToString().Trim() != "" && Convert.ToDecimal(table8.Rows[0]["TransAmtStr"].ToString().Trim()) > new Decimal(0))
        {
            stringBuilder4.Append("<tr>");
            stringBuilder4.Append("<td align='left' class='gridtext' style='padding-left:10px'>Excess Of Income Over Expenditure</td>");
            stringBuilder4.Append("<td align='right' class='gridtext' >" + Convert.ToDecimal(table8.Rows[0]["TransAmtStr"]).ToString("C", (IFormatProvider)format) + "</td>");
            stringBuilder4.Append("<td width='100px'></td>");
            stringBuilder4.Append("</tr>");
            num1 += Convert.ToDecimal(table8.Rows[0]["TransAmtStr"].ToString().Trim());
            num3 += Convert.ToDecimal(table8.Rows[0]["TransAmtStr"].ToString().Trim());
        }
        if (stringBuilder4.ToString().Trim() != "")
        {
            stringBuilder3.Append("<tr>");
            stringBuilder3.Append("<td align='left'  class='gridtext'><b>Capital Account</b></td>");
            stringBuilder3.Append("<td colspan='2' align='right' class='gridtext'><b>" + num3.ToString("C", (IFormatProvider)format) + "</b></td>");
            stringBuilder3.Append("</tr>");
            stringBuilder3.Append(stringBuilder4);
            stringBuilder3.Append("<tr>");
            stringBuilder3.Append("<td></td><td style='border-top-style:ridge'></td><td></td></tr>");
        }
        foreach (DataRow row in (InternalDataCollectionBase)table1.Rows)
        {
            string str2 = row["AG_Name"].ToString();
            if (Convert.ToDecimal(row["TransAmtStr"].ToString()) != new Decimal(0))
            {
                stringBuilder3.Append("<tr>");
                stringBuilder3.Append("<td align='left'  class='gridtext'><b>" + row["AG_Name"].ToString() + "</b></td>");
                if (row["AG_Name"].ToString().Trim().ToUpper() == "CAPITAL ACCOUNT")
                {
                    if (Convert.ToDecimal(table8.Rows[0]["TransAmtStr"].ToString().Trim()) > new Decimal(0))
                        stringBuilder3.Append("<td colspan='2' align='right' class='gridtext'><b>" + (Convert.ToDecimal(row["TransAmtStr"]) + Convert.ToDecimal(table8.Rows[0]["TransAmtStr"].ToString().Trim())).ToString("C", (IFormatProvider)format) + "</b></td>");
                    else
                        stringBuilder3.Append("<td colspan='2' align='right' class='gridtext'><b>" + Convert.ToDecimal(row["TransAmtStr"]).ToString("C", (IFormatProvider)format) + "</b></td>");
                    stringBuilder3.Append("</tr>");
                }
                else
                {
                    if (Convert.ToDecimal(row["TransAmtStr"].ToString()) < new Decimal(0))
                        stringBuilder3.Append("<td colspan='2' align='right' class='gridtext'><b>" + Convert.ToDecimal(Convert.ToDecimal(row["TransAmtStr"]) * new Decimal(-1)).ToString("C", (IFormatProvider)format) + "</b></td>");
                    else
                        stringBuilder3.Append("<td colspan='2' align='right' class='gridtext'><b>" + Convert.ToDecimal(row["TransAmtStr"]).ToString("C", (IFormatProvider)format) + "</b></td>");
                    stringBuilder3.Append("</tr>");
                }
            }
            stringBuilder3.Append("<tr><td colspan='3'></td></tr>");
            for (; index1 < table2.Rows.Count; ++index1)
            {
                if (table2.Rows[index1]["AG_Name"].ToString() == str2)
                {
                    if (Convert.ToDecimal(table2.Rows[index1]["TransAmtStr"]) != new Decimal(0))
                    {
                        stringBuilder3.Append("<tr>");
                        stringBuilder3.Append("<td align='left' class='gridtext' style='padding-left:10px'>" + table2.Rows[index1]["AcctsHead"].ToString() + "</td>");
                        if (Convert.ToDecimal(table2.Rows[index1]["TransAmtStr"]) < new Decimal(0))
                            stringBuilder3.Append("<td align='right' class='gridtext' >" + Convert.ToDecimal(Convert.ToDecimal(table2.Rows[index1]["TransAmtStr"]) * new Decimal(-1)).ToString("C", (IFormatProvider)format) + "</td>");
                        else
                            stringBuilder3.Append("<td align='right' class='gridtext' >" + Convert.ToDecimal(table2.Rows[index1]["TransAmtStr"]).ToString("C", (IFormatProvider)format) + "</td>");
                        stringBuilder3.Append("<td width='100px'></td>");
                        stringBuilder3.Append("</tr>");
                    }
                }
                else
                {
                    stringBuilder3.Append("<tr>");
                    stringBuilder3.Append("<td></td><td style='border-top-style:ridge'></td><td></td></tr>");
                    break;
                }
            }
            num1 += Convert.ToDecimal(row["TransAmtStr"]);
        }
        stringBuilder2.Append("<table width='100%'>");
        int index2 = 0;
        foreach (DataRow row in (InternalDataCollectionBase)table3.Rows)
        {
            string str2 = row["AG_Name"].ToString();
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='left'  class='gridtext'><b>" + row["AG_Name"].ToString() + "</b></td>");
            if (Convert.ToDecimal(row["TransAmtStr"]) < new Decimal(0))
                stringBuilder2.Append("<td colspan='2' align='right' class='gridtext'><b>" + Convert.ToDecimal(Convert.ToDecimal(row["TransAmtStr"]) * new Decimal(-1)).ToString("C", (IFormatProvider)format) + "</b></td>");
            else
                stringBuilder2.Append("<td colspan='2' align='right' class='gridtext'><b>" + Convert.ToDecimal(row["TransAmtStr"]).ToString("C", (IFormatProvider)format) + "</b></td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("<tr><td colspan='3'></td></tr>");
            for (; index2 < table4.Rows.Count; ++index2)
            {
                if (table4.Rows[index2]["AG_Name"].ToString() == str2)
                {
                    if (Convert.ToDecimal(table4.Rows[index2]["TransAmtStr"]) != new Decimal(0))
                    {
                        stringBuilder2.Append("<tr>");
                        stringBuilder2.Append("<td align='left' class='gridtext' style='padding-left:10px'>" + table4.Rows[index2]["AcctsHead"].ToString() + "</td>");
                        stringBuilder2.Append("<td align='right' class='gridtext' >" + Convert.ToDecimal(table4.Rows[index2]["TransAmtStr"]).ToString("C", (IFormatProvider)format) + "</td>");
                        stringBuilder2.Append("<td width='100px'></td>");
                        stringBuilder2.Append("</tr>");
                    }
                }
                else
                {
                    stringBuilder2.Append("<tr>");
                    stringBuilder2.Append("<td></td><td style='border-top-style:ridge'></td><td></td></tr>");
                    break;
                }
            }
            num2 += Convert.ToDecimal(row["TransAmtStr"]);
        }
        Decimal num4 = new Decimal(0);
        Decimal num5 = new Decimal(0);
        if (table7.Rows.Count > 0)
        {
            foreach (DataRow row in (InternalDataCollectionBase)table7.Rows)
            {
                if (Convert.ToDecimal(row["TransAmtStr"].ToString().Trim()) > new Decimal(0))
                {
                    num1 += Convert.ToDecimal(row["TransAmtStr"]);
                    num4 += Convert.ToDecimal(row["TransAmtStr"]);
                }
                else
                {
                    num2 += new Decimal(-1) * Convert.ToDecimal(row["TransAmtStr"]);
                    num5 += new Decimal(-1) * Convert.ToDecimal(row["TransAmtStr"]);
                }
            }
            if (num4 != new Decimal(0))
            {
                stringBuilder3.Append("<tr>");
                stringBuilder3.Append("<td align='left'  class='gridtext'><b>" + table7.Rows[0]["AG_Name"].ToString() + "</b></td>");
                stringBuilder3.Append("<td align='right' class='gridtext' >" + num4.ToString("C", (IFormatProvider)format) + "</td>");
                stringBuilder3.Append("<td width='100px'></td>");
                stringBuilder3.Append("</tr>");
            }
        }
        int index3 = 0;
        foreach (DataRow row in (InternalDataCollectionBase)table5.Rows)
        {
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='left'  class='gridtext'><b>" + row["AG_Name"].ToString() + "</b></td>");
            if (Convert.ToDecimal(row["TransAmtStr"]) < new Decimal(0))
                stringBuilder2.Append("<td colspan='2' align='right' class='gridtext'><b>" + (Convert.ToDecimal(Convert.ToDecimal(row["TransAmtStr"]) * new Decimal(-1)) + num5).ToString("C", (IFormatProvider)format) + "</b></td>");
            else
                stringBuilder2.Append("<td colspan='2' align='right' class='gridtext'><b>" + (Convert.ToDecimal(row["TransAmtStr"]) + num5).ToString("C", (IFormatProvider)format) + "</b></td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("<tr><td colspan='3'></td></tr>");
            for (; index3 < table6.Rows.Count; ++index3)
            {
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td align='left' class='gridtext' style='padding-left:10px'>" + table6.Rows[index3]["AcctsHead"].ToString() + "</td>");
                if (Convert.ToDecimal(table6.Rows[index3]["TransAmtStr"]) < new Decimal(0))
                    stringBuilder2.Append("<td align='right' class='gridtext' >" + Convert.ToDecimal(Convert.ToDecimal(table6.Rows[index3]["TransAmtStr"]) * new Decimal(-1)).ToString("C", (IFormatProvider)format) + "</td>");
                else
                    stringBuilder2.Append("<td align='right' class='gridtext' >" + Convert.ToDecimal(table6.Rows[index3]["TransAmtStr"]).ToString("C", (IFormatProvider)format) + "</td>");
                stringBuilder2.Append("<td width='100px'></td>");
                stringBuilder2.Append("</tr>");
            }
            num2 += Convert.ToDecimal(row["TransAmtStr"]);
        }
        foreach (DataRow row in (InternalDataCollectionBase)table7.Rows)
        {
            if (Convert.ToDecimal(row["TransAmtStr"].ToString().Trim()) > new Decimal(0) && Convert.ToDecimal(row["TransAmtStr"].ToString().Trim()) != new Decimal(0))
            {
                stringBuilder3.Append("<tr>");
                stringBuilder3.Append("<td align='left' class='gridtext' style='padding-left:10px'>" + row["AcctsHead"].ToString() + "</td>");
                stringBuilder3.Append("<td align='right' class='gridtext' >" + Convert.ToDecimal(row["TransAmtStr"]).ToString("C", (IFormatProvider)format) + "</td>");
                stringBuilder3.Append("<td width='100px'></td>");
                stringBuilder3.Append("</tr>");
            }
            else if (Convert.ToDecimal(row["TransAmtStr"].ToString().Trim()) < new Decimal(0) && Convert.ToDecimal(row["TransAmtStr"].ToString().Trim()) != new Decimal(0))
            {
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td align='left' class='gridtext' style='padding-left:10px'>" + row["AcctsHead"].ToString() + "</td>");
                stringBuilder2.Append("<td align='right' class='gridtext' >" + (Convert.ToDecimal(row["TransAmtStr"]) * new Decimal(-1)).ToString("C", (IFormatProvider)format) + "</td>");
                stringBuilder2.Append("<td width='100px'></td>");
                stringBuilder2.Append("</tr>");
            }
        }
        stringBuilder2.Append("</table>");
        stringBuilder3.Append("</table>");
        stringBuilder1.Append("<tr><td style='width:500px'  valign='top'>");
        stringBuilder1.Append(stringBuilder3.ToString());
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("<td style='width:500px'  valign='top'>");
        stringBuilder1.Append(stringBuilder2.ToString());
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("<tr>");
        stringBuilder1.Append("<td align='right'><b>" + num1.ToString("C", (IFormatProvider)format) + "</b></td>");
        stringBuilder1.Append("<td align='right'><b>" + num2.ToString("C", (IFormatProvider)format) + "</b></td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("</table>");
        lblReport.Text = stringBuilder1.ToString();
        Session["PrntBalanceSheet"] = lblReport.Text;
    }

    private void BindSessionYr()
    {
        DataTable dataTable = new DataTable();
        drpSession.DataSource = obj.GetDataTableQry("select FY_Id,FY from dbo.ACTS_FinancialYear order by FY_Id desc");
        drpSession.DataTextField = "FY";
        drpSession.DataValueField = "FY";
        drpSession.DataBind();
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateReport();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (Session["PrntBalanceSheet"] != null)
        {
            Response.Redirect("rptBalanceSheetPrint.aspx");
        }
        else
        {
            lblMsg.Text = "No Data To Print";
            lblMsg.ForeColor = Color.Red;
        }
    }
}