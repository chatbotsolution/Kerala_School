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

public partial class Accounts_rptSundryDebtors : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private NumberFormatInfo format = new NumberFormatInfo();
    private int[] indSizes = new int[4] { 3, 2, 2, 2 };
    protected void Page_Load(object sender, EventArgs e)
    {
        btnprnt.Visible = false;
        lblMsg.Text = string.Empty;
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
        double num = 0.0;
        StringBuilder stringBuilder = new StringBuilder();
        ArrayList arrayList1 = new ArrayList();
        ArrayList arrayList2 = new ArrayList();
        ArrayList arrayList3 = new ArrayList();
        ArrayList arrayList4 = new ArrayList();
        ArrayList arrayList5 = new ArrayList();
        ArrayList arrayList6 = new ArrayList();
        DataTable dataTableQry1 = obj.GetDataTableQry("select FY,StartDate from dbo.ACTS_FinancialYear where StartDate <='" + PopCalendar1.GetDateValue().ToString("MM/dd/yyyy") + "' and EndDate >='" + PopCalendar1.GetDateValue().ToString("MM/dd/yyyy") + "' ");
        if (dataTableQry1.Rows.Count == 0)
        {
            lblMsg.Text = "Selected year is not valid financial year...";
            lblMsg.ForeColor = Color.Red;
            Literal1.Text = "";
        }
        else
        {
            dataTableQry1.Rows[0]["FY"].ToString();
            DataTable dataTableQry2 = obj.GetDataTableQry("select AG_Code from dbo.ACTS_AccountGroups where AG_Code=12");
            if (dataTableQry2.Rows.Count > 0)
            {
                DataTable dataTableQry3 = obj.GetDataTableQry("select PartyName from dbo.ACTS_PartyMaster where AcctsHeadId='" + dataTableQry2.Rows[0]["AG_Code"].ToString().Trim() + "' order by PartyName");
                for (int index1 = 0; index1 < dataTableQry3.Rows.Count; ++index1)
                {
                    DataTable dataTableQry4 = obj.GetDataTableQry("select AcctsHeadId from dbo.ACTS_AccountHeads where AcctsHead='" + dataTableQry3.Rows[index1]["PartyName"].ToString() + "' ");
                    if (dataTableQry4.Rows.Count > 0)
                    {
                        for (int index2 = 0; index2 < dataTableQry4.Rows.Count; ++index2)
                        {
                            DataTable dataTableQry5 = obj.GetDataTableQry("select * from dbo.ACTS_GenLedger where AccountHead='" + dataTableQry4.Rows[index2]["AcctsHeadId"].ToString() + "' and TransDate<='" + PopCalendar1.GetDateValue().ToString("MM/dd/yyyy") + "' order by TransDate asc");
                            if (dataTableQry5.Rows.Count > 0)
                            {
                                for (int index3 = 0; index3 < dataTableQry5.Rows.Count; ++index3)
                                    num = !(dataTableQry5.Rows[index3]["CrDr"].ToString().Trim().ToUpper() == "DR") ? Convert.ToDouble(num + Convert.ToDouble(dataTableQry5.Rows[index3]["TransAmt"])) : Convert.ToDouble(num - Convert.ToDouble(dataTableQry5.Rows[index3]["TransAmt"]));
                            }
                        }
                        arrayList5.Add(num.ToString());
                        arrayList3.Add(dataTableQry3.Rows[index1]["PartyName"].ToString());
                        num = 0.0;
                    }
                }
                num = 0.0;
                stringBuilder.Append("<table width='98%' class='innertbltxt'><tr><td colspan='3'></td></tr>");
                stringBuilder.Append("<tr><td colspan='4' height='2px'></td></tr>");
                stringBuilder.Append("<tr><td colspan='4' align='center' class='pageSectionLabel' style= 'border-bottom:dotted 1px black;'><b>List of Sundry Debtors as on " + txtstartdate.Text + "</b></td></tr>");
                stringBuilder.Append("<tr><td colspan='4' height='2px'>&nbsp;</td></tr>");
                stringBuilder.Append("<tr><td colspan='4' height='2px'></td></tr>");
                stringBuilder.Append("<td width='5%'><b>Sno.</b></td>");
                stringBuilder.Append("<td width='30%'><b>Debtors Name</b></td>");
                stringBuilder.Append("<td width='15%' align='right'><b>Amount</b></td>");
                stringBuilder.Append("<td width='55%' align='right'></td>");
                stringBuilder.Append("</tr>");
                for (int index = 0; index < arrayList3.Count; ++index)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td width='5%'>" + Convert.ToString(index + 1) + "</td>");
                    stringBuilder.Append("<td width='30%'>" + arrayList3[index].ToString() + "</td>");
                    stringBuilder.Append("<td width='15%' align='right'>" + Convert.ToDecimal(arrayList5[index]).ToString("C", (IFormatProvider)format) + "</td>");
                    stringBuilder.Append("<td width='55%' align='right'></td>");
                    stringBuilder.Append("</tr>");
                    num += Convert.ToDouble(arrayList5[index]);
                }
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td width='5%'></td>");
                stringBuilder.Append("<td width='30%'></td>");
                stringBuilder.Append("<td width='10%' align='right' style= 'border-bottom:dotted 1px black;'>&nbsp;</td>");
                stringBuilder.Append("<td width='55%' align='right'></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td width='5%'></td>");
                stringBuilder.Append("<td width='30%' align='right'><b>Total -:</b></td>");
                stringBuilder.Append("<td width='15%' align='right' style= 'border-bottom:dotted 1px black;'><b>" + Convert.ToDecimal(num).ToString("C", (IFormatProvider)format) + "</b></td>");
                stringBuilder.Append("<td width='55%' align='right'></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td width='5%'></td>");
                stringBuilder.Append("<td width='30%'></td>");
                stringBuilder.Append("<td width='10%' align='right'>&nbsp;</td>");
                stringBuilder.Append("<td width='55%' align='right'></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                Literal1.Text = stringBuilder.ToString();
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