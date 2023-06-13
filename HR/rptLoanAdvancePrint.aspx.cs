using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_rptLoanAdvancePrint : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        if (!ChkIsHRUsed())
        {
            Response.Redirect("HRHome.aspx?IsHRUsed=0");
        }
        else
        {
            BindDropDown(drpEmp, "SELECT EmpId,SevName FROM dbo.HR_EmployeeMaster ORDER BY SevName", "SevName", "EmpId");
            string str1 = obj.ExecuteScalarQry("select dbo.fuGetSessionYr('" + DateTime.Now.ToString("dd-MMM-yyyy") + "')");
            ViewState["Yr"] = str1;
            string[] strArray = str1.Split('-');
            string str2 = strArray[0] + strArray[1];
            ViewState["YearStart"] = Convert.ToDateTime("01-APR-" + strArray[0]);
            ViewState["YearEnd"] = Convert.ToDateTime("31-MAR-" + (Convert.ToInt32(strArray[0]) + 1).ToString());
            dtpFromDt.SetDateValue(Convert.ToDateTime(ViewState["YearStart"]));
            dtpToDt.SetDateValue(Convert.ToDateTime(ViewState["YearEnd"]));
        }
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    protected void drpEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpEmp.SelectedIndex > 0)
        {
            GenerateReportNew();
        }
        else
        {
            lblReport.Text = string.Empty;
            btnPrint.Visible = false;
        }
        drpEmp.Focus();
    }

    private void GenerateReportNew()
    {
        StringBuilder stringBuilder = new StringBuilder("");
        Hashtable hashtable = new Hashtable();
        if (drpEmp.SelectedIndex > 0)
            hashtable.Add("@EmpId", drpEmp.SelectedValue);
        if (txtFromDt.Text.Trim() != string.Empty && txtToDt.Text.Trim() != string.Empty)
        {
            hashtable.Add("@FromDt", dtpFromDt.GetDateValue().ToString("dd-MMM-yyyy"));
            hashtable.Add("@ToDt", dtpToDt.GetDateValue().ToString("dd-MMM-yyyy"));
        }
        DataSet dataSet = obj.GetDataSet("HR_rptLoanAdvance", hashtable);
        DataTable table = dataSet.Tables[0];
        DataTable dataTable = new DataTable();
        if (dataSet.Tables.Count > 1)
            dataTable = dataSet.Tables[1];
        if (table.Rows.Count > 0)
        {
            if (drpEmp.SelectedIndex > 0)
            {
                stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0'  width='100%' style='border:none 0px;'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='center' style='border-bottom:none 0px;' colspan='6'><strong>Loan Details of " + drpEmp.SelectedItem.Text.ToUpper() + " from " + dtpFromDt.GetDateValue().ToString("dd-MMM-yyy") + " to " + dtpToDt.GetDateValue().ToString("dd-MMM-yyyy") + " </strong>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='right' colspan='6'><font color='Black'><strong>No Of Records : </strong> " + table.Rows.Count.ToString() + "</font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 5%' align='left'><b>No.</b></td>");
                stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 10%' align='left'><b>Transaction Date</b></td>");
                stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 25%' align='left'><b>Description</b></td>");
                stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 10%' align='right'><b>Loan Amount</b></td>");
                stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 10%' align='right'><b>Interset Amount</b></td>");
                stringBuilder.Append("<td style='border-top:none 0px; width: 10%' align='right'><b>Recovered Amount</b></td>");
                stringBuilder.Append("</tr>");
                int num1 = 1;
                Decimal num2 = new Decimal(0);
                foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
                {
                    Decimal num3 = new Decimal(0);
                    Decimal num4 = new Decimal(0);
                    Decimal num5 = new Decimal(0);
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' style='border-right:none 0px; border-top:none 0px; '>");
                    stringBuilder.Append(num1.ToString());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='left' style='border-right:none 0px; border-top:none 0px;'>");
                    stringBuilder.Append(Convert.ToDateTime(row["TransDate"]).ToString("dd-MMM-yyyy"));
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='left' style='border-right:none 0px; border-top:none 0px;'>");
                    stringBuilder.Append(row["LoanDesc"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    if (row["LoanAmt"].ToString() != string.Empty)
                    {
                        Decimal num6 = Convert.ToDecimal(row["LoanAmt"]);
                        stringBuilder.Append("<td align='right' style='border-right:none 0px; border-top:none 0px;'>" + num6.ToString("0.00") + "</td>");
                    }
                    else
                        stringBuilder.Append("<td align='right' style='border-right:none 0px; border-top:none 0px;'>--</td>");
                    if (row["IntAmt"].ToString() != string.Empty)
                    {
                        Decimal num6 = Convert.ToDecimal(row["IntAmt"]);
                        stringBuilder.Append("<td align='right' style='border-right:none 0px; border-top:none 0px;'>" + num6.ToString("0.00") + "</td>");
                    }
                    else
                        stringBuilder.Append("<td align='right' style='border-right:none 0px; border-top:none 0px;'>--</td>");
                    if (row["RecAmt"].ToString() != string.Empty)
                    {
                        Decimal num6 = Convert.ToDecimal(row["RecAmt"]);
                        stringBuilder.Append("<td align='right' style='border-top:none 0px;'>" + num6.ToString("0.00") + "</td>");
                    }
                    else
                        stringBuilder.Append("<td align='right' style='border-top:none 0px;'>--</td>");
                    ++num1;
                }
                if (dataTable.Rows.Count > 0 && Convert.ToDecimal(dataTable.Rows[0][0]) > new Decimal(0))
                    num2 = Convert.ToDecimal(dataTable.Rows[0][0]);
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='border-top:none 0px;' colspan='6' align='right'>");
                stringBuilder.Append("<b>Total Pending Amount to Recover (till date) : " + num2.ToString("0.00") + "</b></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
            }
            else if (table.Rows.Count > 0)
            {
                int num1 = 1;
                Decimal num2 = new Decimal(0);
                Decimal num3 = new Decimal(0);
                Decimal num4 = new Decimal(0);
                stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0'  width='100%' style='border-collapse:collapse;'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='center' colspan='5'><strong>Loan Details of all employees from " + dtpFromDt.GetDateValue().ToString("dd-MMM-yyy") + " to " + dtpToDt.GetDateValue().ToString("dd-MMM-yyyy") + " </strong>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='right' colspan='5'><font color='Black'><strong>No Of Records : </strong> " + table.Rows.Count.ToString() + "</font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='width: 5%' align='left'><b>No.</b></td>");
                stringBuilder.Append("<td style='width: 25%' align='left'><b>Employee Name</b></td>");
                stringBuilder.Append("<td style='width: 10%' align='right'><b>Loan Amount</b></td>");
                stringBuilder.Append("<td style='width: 10%' align='right'><b>Recovered Amount</b></td>");
                stringBuilder.Append("<td style='width: 10%' align='right'><b>Pending Amount</b></td>");
                stringBuilder.Append("</tr>");
                foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' >");
                    stringBuilder.Append(num1.ToString());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='left' >");
                    stringBuilder.Append(row["Emp"]);
                    stringBuilder.Append("</td>");
                    if (row["TotLoan"].ToString() != string.Empty)
                    {
                        num2 += Convert.ToDecimal(row["TotLoan"]);
                        stringBuilder.Append("<td align='right' >" + Convert.ToDecimal(row["TotLoan"]).ToString("0.00") + "</td>");
                    }
                    else
                        stringBuilder.Append("<td align='right' >0</td>");
                    if (row["TotRec"].ToString() != string.Empty)
                    {
                        num3 += Convert.ToDecimal(row["TotRec"]);
                        stringBuilder.Append("<td align='right' >" + Convert.ToDecimal(row["TotRec"]).ToString("0.00") + "</td>");
                    }
                    else
                        stringBuilder.Append("<td align='right' >0</td>");
                    if (row["TotPending"].ToString() != string.Empty && Convert.ToDecimal(row["TotPending"]) > new Decimal(0))
                    {
                        num4 += Convert.ToDecimal(row["TotPending"]);
                        stringBuilder.Append("<td align='right' >" + Convert.ToDecimal(row["TotPending"]).ToString("0.00") + "</td>");
                    }
                    else
                        stringBuilder.Append("<td align='right' >0</td>");
                    ++num1;
                }
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='width: 5%' align='left'>&nbsp;</td>");
                stringBuilder.Append("<td style='width: 25%' align='right'><b>Total</b></td>");
                stringBuilder.Append("<td style='width: 10%' align='right'><b>" + num2.ToString("0.00") + "</b></td>");
                stringBuilder.Append("<td style='width: 10%' align='right'><b>" + num3.ToString("0.00") + "</b></td>");
                stringBuilder.Append("<td style=width: 10%' align='right'><b>" + (num2 - num3).ToString("0.00") + "</b></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
            }
            Session["rptLoanData"] = (lblReport.Text = stringBuilder.ToString().Trim());
            btnPrint.Visible = true;
        }
        else
        {
            stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0'  width='100%' style='border:none 0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center'><b>No Record Available</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            btnPrint.Visible = false;
        }
    }

    private void GenerateReport()
    {
        Decimal num1 = new Decimal(0);
        StringBuilder stringBuilder = new StringBuilder("");
        DataTable dataTable1 = obj.GetDataTable("HR_rptLoanAdvance", new Hashtable()
    {
      {
         "@EmpId",
         drpEmp.SelectedValue
      }
    });
        if (dataTable1.Rows.Count > 0)
        {
            stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0'  width='100%' style='border:none 0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center' style='border-bottom:none 0px;' colspan='9'><strong>Loan Details of " + drpEmp.SelectedItem.Text.ToUpper() + " </strong>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='right' colspan='9'><font color='Black'><strong>No Of Records : </strong> " + dataTable1.Rows.Count.ToString() + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 5%' align='left'><b>No.</b></td>");
            stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 10%' align='left'><b>Loan Date</b></td>");
            stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 10%' align='left'><b>Loan Type</b></td>");
            stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 15%' align='left'><b>Reason</b></td>");
            stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 10%' align='right'><b>Loan Amount</b></td>");
            stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 10%' align='right'><b>Interset Amount</b></td>");
            stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 10%' align='right'><b>Recovered Amount</b></td>");
            stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px; width: 10%' align='right'><b>Pending Amount</b></td>");
            stringBuilder.Append("<td style='border-top:none 0px; width: 10%' align='center'><b>Details</b></td>");
            stringBuilder.Append("</tr>");
            int num2 = 1;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable1.Rows)
            {
                Decimal num3 = new Decimal(0);
                Decimal num4 = new Decimal(0);
                Decimal num5 = new Decimal(0);
                Decimal num6 = new Decimal(0);
                DataTable dataTable2 = obj.GetDataTable("HR_rptLoanAdvDetails", new Hashtable()
        {
          {
             "@GenLedgerId",
            row["GenLedgerId"]
          }
        });
                if (dataTable2.Rows[0]["PrincipalAmt"].ToString().Trim() != string.Empty)
                    num4 = Convert.ToDecimal(dataTable2.Rows[0]["PrincipalAmt"].ToString().Trim());
                if (dataTable2.Rows[0]["RecoveredAmt"].ToString().Trim() != string.Empty)
                    num5 = Convert.ToDecimal(dataTable2.Rows[0]["RecoveredAmt"].ToString().Trim());
                Decimal num7 = num4 - num5;
                num1 += num7;
                Decimal num8;
                if (dataTable2.Rows[0]["IsDirect"].ToString().ToUpper() == "TRUE")
                {
                    num8 = Convert.ToDecimal(dataTable2.Rows[0]["InterestAmt"].ToString().Trim());
                    num5 += num8;
                }
                else
                    num8 = num4 - Convert.ToDecimal(row["LoanAmt"]);
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' style='border-right:none 0px; border-top:none 0px; '>");
                stringBuilder.Append(num2.ToString());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-right:none 0px; border-top:none 0px;'>");
                stringBuilder.Append(Convert.ToDateTime(row["LoanDate"]).ToString("dd-MMM-yyyy"));
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-right:none 0px; border-top:none 0px; '>");
                stringBuilder.Append(dataTable2.Rows[0]["LoanType"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-right:none 0px; border-top:none 0px;'>");
                stringBuilder.Append(row["LoanDesc"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' style='border-right:none 0px; border-top:none 0px;'>");
                stringBuilder.Append(Convert.ToDecimal(row["LoanAmt"]).ToString("0.00"));
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' style='border-right:none 0px; border-top:none 0px;'>" + num8.ToString("0.00") + "</td>");
                stringBuilder.Append("<td align='right' style='border-right:none 0px; border-top:none 0px;'>" + num5.ToString("0.00") + "</td>");
                stringBuilder.Append("<td align='right' style='border-right:none 0px; border-top:none 0px;'>" + num7.ToString("0.00") + "</td>");
                stringBuilder.Append("<td align='center' style='border-top:none 0px;' >");
                if (dataTable2.Rows[0]["IsDirect"].ToString().ToUpper() == "TRUE")
                {
                    stringBuilder.Append("<b>Direct Recovery</b>");
                }
                else
                {
                    string str1 = "<a href='javascript:popUp";
                    string str2 = "('rptLoanRecDetails.aspx?gl_id=" + row["GenLedgerId"].ToString() + "&lt=" + dataTable2.Rows[0]["LoanType"].ToString().Trim() + "&emp=" + drpEmp.SelectedItem.Text + "&dt=" + row["LoanDate"].ToString() + "&amt=" + row["LoanAmt"].ToString() + "')";
                    string str3 = "'>";
                    stringBuilder.Append(str1.Replace('\'', '"') + str2 + str3.Replace('\'', '"'));
                    stringBuilder.Append("<b>View</b></a>");
                }
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                ++num2;
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px;' colspan='7' align='right'><b>Total Pending Amount to Recover</b>");
            stringBuilder.Append("<td style='border-right:none 0px; border-top:none 0px;' align='right'><b>" + num1.ToString("0.00") + "</b></td>");
            stringBuilder.Append("<td style='border-top:none 0px;' align='left'>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            Session["rptLoanData"] = (lblReport.Text = stringBuilder.ToString().Trim());
            btnPrint.Visible = true;
        }
        else
        {
            stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0'  width='100%' style='border:none 0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center'><b>No Record Available</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            btnPrint.Visible = false;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GenerateReportNew();
    }
}