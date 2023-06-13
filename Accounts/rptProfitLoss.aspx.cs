using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_rptProfitLoss : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        BindSessionYr();
        GenerateReport();
    }

    private void BindSessionYr()
    {
        DataTable dataTable = new DataTable();
        drpSession.DataSource = obj.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void GenerateReport()
    {
        try
        {
            lblReport.ForeColor = Color.Black;
            Hashtable hashtable = new Hashtable();
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = new DataTable();
            DataSet dataSet1 = new DataSet();
            Decimal num1 = new Decimal(0);
            Decimal num2 = new Decimal(0);
            string str = drpSession.SelectedValue.ToString().Trim();
            hashtable.Add("@StartDt", Convert.ToDateTime("1 Apr" + str.Substring(0, 4)));
            hashtable.Add("@EndDt", Convert.ToDateTime("31 Mar" + (Convert.ToInt32(str.Substring(0, 4)) + 1).ToString()));
            DataSet dataSet2 = obj.GetDataSet("ACTS_GetTotProftLos", hashtable);
            DataTable table1 = dataSet2.Tables[1];
            DataTable table2 = dataSet2.Tables[0];
            StringBuilder stringBuilder = new StringBuilder();
            if (table1.Rows.Count > 0 || table2.Rows.Count > 0)
            {
                stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='1000px' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='4' style='border-right-style:none;' align='center' class='gridtext'><font color='Black'><b>PROFIT AND LOSS ACCOUNT FOR THE FINANCIAL YEAR " + str + "</b></font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr><td style='width:500px'  valign='top'>");
                stringBuilder.Append("<table width='100%'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='width: 300px' align='left'  class='gridtext'><b>Particulars</b></td>");
                stringBuilder.Append("<td align='right' class='gridtext'></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                foreach (DataRow row in (InternalDataCollectionBase)table2.Rows)
                    num1 += Convert.ToDecimal(row["TransAmtStr"]);
                foreach (DataRow row in (InternalDataCollectionBase)table1.Rows)
                    num2 += Convert.ToDecimal(row["TransAmtStr"]);
                stringBuilder.Append("<td style='width: 300px' align='left'  class='gridtext'><b>Indirect Expenses</b></td><td></td>");
                stringBuilder.Append("<td align='right' class='gridtext'><b>" + num1 + "</b></td>");
                stringBuilder.Append("</tr>");
                foreach (DataRow row in (InternalDataCollectionBase)table2.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' class='gridtext'>");
                    stringBuilder.Append(row["AcctsHead"].ToString() + "</td>");
                    stringBuilder.Append("<td align='right' class='gridtext'>");
                    stringBuilder.Append(row["TransAmtStr"].ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                }
                if (num2 > num1)
                {
                    stringBuilder.Append("<tr><td colspan='3'>&nbsp;</td></tr>");
                    stringBuilder.Append("<tr><td align='left'><b>Net Profit:</b></td><td></td>");
                    stringBuilder.Append("<td align='right' class='gridtext'><b>" + Convert.ToString(num2 - num1) + "</b></td></tr>");
                    num1 = num1 + num2 - num1;
                }
                stringBuilder.Append("</table></td>");
                stringBuilder.Append("<td style='width:500px' colspan='2' valign='top'><table width='100%'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='width: 300px' align='left'  class='gridtext'><b>Particulars</b></td>");
                stringBuilder.Append("<td align='right' class='gridtext'></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='width: 300px' align='left'  class='gridtext'><b>Indirect Incomes</b></td><td></td>");
                stringBuilder.Append("<td align='right' class='gridtext'><b>" + num2 + "</b></td>");
                stringBuilder.Append("</tr>");
                foreach (DataRow row in (InternalDataCollectionBase)table1.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' class='gridtext'>");
                    stringBuilder.Append(row["AcctsHead"].ToString() + "</td>");
                    stringBuilder.Append("<td align='right' class='gridtext'>");
                    stringBuilder.Append(row["TransAmtStr"].ToString() + "</td>");
                    stringBuilder.Append("</tr>");
                }
                if (num1 > num2)
                {
                    stringBuilder.Append("<tr><td colspan='3'>&nbsp;</td></tr>");
                    stringBuilder.Append("<tr><td align='left' class='gridtext' ><b>Net Loss<></td><td>&nbsp;</td>");
                    stringBuilder.Append("<td align='right' class='gridtext'><b>" + Convert.ToString(num1 - num2) + "</b></td></tr>");
                    num2 = num2 + num1 - num2;
                }
                stringBuilder.Append("</table></td></tr>");
                stringBuilder.Append("<tr><td align='right'><b>Total: " + num1.ToString() + "</b></td>");
                stringBuilder.Append("<td align='right' colspan='2'><b>Total: " + num2.ToString() + "</b></td></tr>");
                stringBuilder.Append("</td></tr></table>");
                lblReport.Text = stringBuilder.ToString();
                Session["PrntPrftLoss"] = stringBuilder.ToString();
            }
            else
            {
                lblReport.Text = "No Relevant Data Found";
                lblReport.ForeColor = Color.Red;
                Session["PrntPrftLoss"] = null;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        GenerateReport();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptProfitLossPrint.aspx");
    }
}