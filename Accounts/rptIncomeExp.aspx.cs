using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_rptIncomeExp : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        if (Page.IsPostBack)
            return;
        BindSessionYr();
        getFyDate();
    }

    private void BindSessionYr()
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = obj.GetDataTableQry("select FY_Id,FY from dbo.ACTS_FinancialYear order by FY_Id desc");
        if (dataTableQry.Rows.Count > 0)
        {
            drpSession.DataSource = dataTableQry;
            drpSession.DataTextField = "FY";
            drpSession.DataValueField = "FY";
            drpSession.DataBind();
        }
        else
        {
            lblMsg.Text = "Please Define Financial Year To View Report";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void getFyDate()
    {
        if (drpSession.Items.Count <= 0)
            return;
        string str = drpSession.SelectedValue.Trim().Split('-')[0];
        dtpfromdt.SetDateValue(Convert.ToDateTime("1 Apr " + str));
        dtptodt.SetDateValue(Convert.ToDateTime("31 Mar " + (Convert.ToInt32(str.Trim()) + 1).ToString()));
        lblReport.Text = "";
    }

    private string checkDate()
    {
        obj = new clsDAL();
        string str = obj.ExecuteScalarQry("select count(*) from dbo.ACTS_FinancialYear where FY='" + drpSession.SelectedValue.Trim() + "' and StartDate<='" + dtpfromdt.GetDateValue().ToString("dd MMM yyyy") + "' and EndDate>='" + dtpfromdt.GetDateValue().ToString("dd MMM yyyy") + "'");
        if (str.Trim() == "" || str.Trim() == "0")
        {
            lblMsg.Text = "'From Date' is not in the selected finnancial year!!";
            lblMsg.ForeColor = Color.Red;
            return "NO";
        }
        if (!(obj.ExecuteScalarQry("select count(*) from dbo.ACTS_FinancialYear where FY='" + drpSession.SelectedValue.Trim() + "' and StartDate<='" + dtptodt.GetDateValue().ToString("dd MMM yyyy") + "' and EndDate>='" + dtptodt.GetDateValue().ToString("dd MMM yyyy") + "'").Trim() == "") && !(str.Trim() == "0"))
            return "";
        lblMsg.Text = "'To Date' is not in the selected finnancial year!!";
        lblMsg.ForeColor = Color.Red;
        return "NO";
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        getFyDate();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    private void GenerateReport()
    {
        try
        {
            if (checkDate().Trim() == "")
            {
                Hashtable hashtable = new Hashtable();
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = new DataTable();
                DataTable dataTable3 = new DataTable();
                DataSet dataSet1 = new DataSet();
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                hashtable.Add("@StartDt", dtpfromdt.GetDateValue().ToString("dd MMM yyyy 00:00:00"));
                hashtable.Add("@EndDt", dtptodt.GetDateValue().ToString("dd MMM yyyy 23:59:59"));
                DataSet dataSet2 = obj.GetDataSet("ACTS_GetTotIncExp", hashtable);
                DataTable table1 = dataSet2.Tables[1];
                DataTable table2 = dataSet2.Tables[0];
                DataTable table3 = dataSet2.Tables[2];
                StringBuilder stringBuilder = new StringBuilder();
                lblReport.ForeColor = Color.Black;
                if (table1.Rows.Count > 0 || table2.Rows.Count > 0 || table3.Rows.Count > 0)
                {
                    string str1 = drpSession.SelectedValue.ToString().Trim();
                    string str2 = Information();
                    stringBuilder.Append("<center>");
                    stringBuilder.Append(str2);
                    stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='1000px' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td colspan='4'  align='center' class='gridtext'><font color='Black'><b>INCOME AND EXPENDITURE ACCOUNT FOR THE FINANCIAL YEAR " + str1 + " FROM " + dtpfromdt.GetDateValue().ToString("dd MMM yyyy") + " To " + dtptodt.GetDateValue().ToString("dd MMM yyyy") + "</b></font>");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("<tr><td style='width:500px'  valign='top'>");
                    stringBuilder.Append("<table width='100%'>");
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td style='width: 300px' align='left'  class='gridtext'><b>Expenditure</b></td>");
                    stringBuilder.Append("<td align='right' class='gridtext'><b>Amount</b></td>");
                    stringBuilder.Append("</tr>");
                    foreach (DataRow row in (InternalDataCollectionBase)table2.Rows)
                    {
                        num1 += Convert.ToDecimal(row["TransAmtStr"]);
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td align='left' class='gridtext'>");
                        stringBuilder.Append(row["AcctsHead"].ToString() + "</td>");
                        stringBuilder.Append("<td align='right' class='gridtext'>");
                        stringBuilder.Append(row["TransAmtStr"].ToString() + "</td>");
                        stringBuilder.Append("</tr>");
                    }
                    stringBuilder.Append("</table></td>");
                    stringBuilder.Append("<td style='width:500px' colspan='2' valign='top'><table width='100%'>");
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td style='width: 300px' align='left'  class='gridtext'><b>Income</b></td>");
                    stringBuilder.Append("<td align='right' class='gridtext'><b>Amount</b></td>");
                    stringBuilder.Append("</tr>");
                    foreach (DataRow row in (InternalDataCollectionBase)table1.Rows)
                    {
                        num2 += Convert.ToDecimal(row["TransAmtStr"]);
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td align='left' class='gridtext'>");
                        stringBuilder.Append(row["AcctsHead"].ToString() + "</td>");
                        stringBuilder.Append("<td align='right' class='gridtext'>");
                        stringBuilder.Append(row["TransAmtStr"].ToString() + "</td>");
                        stringBuilder.Append("</tr>");
                    }
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' class='gridtext'>");
                    stringBuilder.Append("<b><hr /><u>School Fee Collection</u></b></td>");
                    stringBuilder.Append("</tr>");
                    foreach (DataRow row in (InternalDataCollectionBase)table3.Rows)
                    {
                        num2 += Convert.ToDecimal(row["TransAmtStr"]);
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td align='left' class='gridtext'>");
                        stringBuilder.Append(row["FeeName"].ToString() + "</td>");
                        stringBuilder.Append("<td align='right' class='gridtext'>");
                        stringBuilder.Append(row["TransAmtStr"].ToString() + "</td>");
                        stringBuilder.Append("</tr>");
                    }
                    stringBuilder.Append("</table></td></tr>");
                    if (num2 > num1)
                    {
                        stringBuilder.Append("<tr><td align='left'>Excess Of Income Over Expenditure: &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;");
                        stringBuilder.Append(Convert.ToString(num2 - num1) + "</td><td colspan='4'></td></tr>");
                        num1 = num1 + num2 - num1;
                    }
                    else if (num1 > num2)
                    {
                        stringBuilder.Append("<tr><td></td><td align='left' class='gridtext' >Excess Of Expenditure Over Income</td>");
                        stringBuilder.Append("<td align='right'>" + Convert.ToString(num1 - num2) + "</td></tr>");
                        num2 = num2 + num1 - num2;
                    }
                    stringBuilder.Append("<tr><td align='right'><b>Total: " + num1.ToString() + "</b></td>");
                    stringBuilder.Append("<td align='right' colspan='2'><b>Total: " + num2.ToString() + "</b></td></tr>");
                    stringBuilder.Append("</td></tr></table>");
                    stringBuilder.Append("</center>");
                    lblReport.Text = stringBuilder.ToString();
                    Session["PrntIncExp"] = stringBuilder.ToString();
                }
                else
                {
                    lblReport.Text = "No Relevant Data Found";
                    lblReport.ForeColor = Color.Red;
                    Session["PrntIncExp"] = null;
                }
            }
            else
                lblReport.Text = "";
        }
        catch (Exception ex)
        {
        }
    }

    private string Information()
    {
        clsDAL clsDal1 = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataSet dataSet = new DataSet();
        StringBuilder stringBuilder = new StringBuilder();
        clsDAL clsDal2 = new clsDAL();
        string str1 = Server.MapPath("../XMLFiles") + "\\Detail.xml";
        if (File.Exists(str1))
        {
            int num = (int)dataSet.ReadXml(str1);
            string str2 = Session["SSVMID"].ToString();
            if (dataSet.Tables.Count > 0)
            {
                DataTable dataTable2 = new DataTable();
                DataTable table = dataSet.Tables[0];
                foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    stringBuilder.Append("<table width='85%' class='tbltd' cellpadding='2' cellspacing='2'>");
                    stringBuilder.Append("<tr><td rowspan='4'> <img src='../images/leftlogo.jpg' width='75px' height='80px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + clsDal2.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + clsDal2.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + "," + clsDal2.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</strong></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + clsDal2.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + clsDal2.Decrypt(row["Pin"].ToString().Trim(), str2) + "</strong> </td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>Phone-" + clsDal2.Decrypt(row["Phone"].ToString().Trim(), str2) + "</strong></td></tr><tr><td colspan='5'></td></tr></table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptIncomeExpPrint.aspx");
    }
}