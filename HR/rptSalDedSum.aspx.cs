using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class HR_rptSalDedSum : System.Web.UI.Page
{
    private clsDAL objDAL = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (!ChkIsHRUsed())
        {
            Response.Redirect("HRHome.aspx?IsHRUsed=0");
        }
        else
        {
            Session["rptPayBill"] = null;
            bindDrpYear();
        }
    }

    private bool ChkIsHRUsed()
    {
        return objDAL.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void bindDrpYear()
    {
        int year = DateTime.Now.Year;
        drpYear.Items.Insert(0, new ListItem("- SELECT -", "0"));
        for (int index = 1; index < 11; ++index)
        {
            drpYear.Items.Insert(index, new ListItem(year.ToString(), year.ToString()));
            --year;
        }
    }

    protected void GenerateReport()
    {
        if (drpMonth.SelectedIndex <= 0 || drpYear.SelectedIndex <= 0)
            return;
        int num1 = 13;
        Decimal num2 = new Decimal(0);
        Decimal num3 = new Decimal(0);
        Decimal num4 = new Decimal(0);
        Decimal num5 = new Decimal(0);
        Decimal num6 = new Decimal(0);
        Decimal num7 = new Decimal(0);
        Decimal num8 = new Decimal(0);
        Decimal num9 = new Decimal(0);
        Decimal num10 = new Decimal(0);
        StringBuilder stringBuilder1 = new StringBuilder();
        DataTable dataTable1 = new DataTable();
        stringBuilder1.Append("SELECT * FROM HR_view_MonthlySalaryNew WHERE Month='" + drpMonth.SelectedValue.ToString() + "' AND Year=" + drpYear.SelectedValue.ToString() + " AND (SalPaidDate IS NOT NULL OR PayWithHeld='Y' )");
        stringBuilder1.Append("ORDER BY DesignationId,EmpName");
        DataTable dataTableQry1 = objDAL.GetDataTableQry(stringBuilder1.ToString());
        string str1 = "SELECT BillNo FROM dbo.HR_GenerateMlySalary WHERE SalYear='" + drpYear.SelectedValue.ToString().Trim() + "' AND SalMonth='" + drpMonth.SelectedValue.ToString().Trim() + "'";
        objDAL = new clsDAL();
        string str2 = objDAL.ExecuteScalarQry(str1);
        if (dataTableQry1.Rows.Count > 0)
        {
            StringBuilder stringBuilder2 = new StringBuilder();
            string str3 = Information();
            stringBuilder2.Append("<table width='100%' cellpadding='1' cellspacing='0'>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td style='font-size: 14px; background-color: Silver;' align='center'><b>" + str3 + "</b></td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td style='font-size: 14px; background-color: Silver;' align='center'><b>");
            stringBuilder2.Append("SALARY FOR THE MONTH OF " + drpMonth.SelectedItem.Text.Trim().ToUpper() + " - " + drpYear.SelectedValue.ToString() + ", ");
            stringBuilder2.Append("BILL NO - " + str2 + "</b>");
            stringBuilder2.Append("</td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("<div style='height:5px;'></div>");
            stringBuilder2.Append("<table width='100%' cellpadding='1' cellspacing='0' style='border:solid 1px;border-bottom:none 1px;'>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;' width='10px'><b>S.No.</b></td>");
            stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;' width='80px'><b>Name</b></td>");
            DataTable dataTableQry2 = objDAL.GetDataTableQry("SELECT DISTINCT DedTypeId,DedDetails FROM HR_view_MonthlyDeductionsNew WHERE DedTypeId>0 ORDER BY DedTypeId ASC");
            if (dataTableQry2.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry2.Rows)
                {
                    if (row["DedTypeId"].ToString() != "0")
                    {
                        stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>" + row["DedDetails"].ToString() + "</b></td>");
                        ViewState["totDed" + row["DedDetails"]] = 0;
                        ++num1;
                    }
                }
            }
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>Loan Recovery</b></td>");
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>Loss of Pay</b></td>");
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>Misc Recovery</b></td>");
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>Total Ded.</b></td>");
            stringBuilder2.Append("</tr>");
            int num11 = 1;
            foreach (DataRow row1 in (InternalDataCollectionBase)dataTableQry1.Rows)
            {
                Decimal num12 = new Decimal(0);
                Hashtable hashtable = new Hashtable();
                hashtable.Clear();
                hashtable.Add("@EmpId", row1["EmpId"]);
                hashtable.Add("@Month", drpMonth.SelectedValue);
                hashtable.Add("@Year", drpYear.SelectedValue);
                DataTable dataTable2 = objDAL.GetDataTable("HR_GetPrevAmt", hashtable);
                string str4 = dataTable2.Rows[0]["ClaimAmt"].ToString();
                string str5 = dataTable2.Rows[0]["PrevAmt"].ToString();
                string str6 = dataTable2.Rows[0]["Arrear"].ToString();
                if (str4.Trim() == string.Empty)
                    ;
                if (str5.Trim() == string.Empty)
                    str5 = "0";
                if (str6.Trim() == string.Empty)
                    str6 = "0";
                Decimal num13 = Convert.ToDecimal(str5) + Convert.ToDecimal(str6);
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;'>" + num11 + "</td>");
                stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;'>" + row1["EmpName"] + " (" + row1["Designation"] + ")</td>");
                if (row1["PayWithHeld"].ToString().Trim() == "N")
                {
                    Decimal num14 = new Decimal(0);
                    foreach (DataRow row2 in (InternalDataCollectionBase)dataTableQry2.Rows)
                    {
                        string str7 = objDAL.ExecuteScalarQry("SELECT AmtFromEmp FROM HR_view_MonthlyDeductionsNew WHERE SalaryId=" + row1["SalId"].ToString() + " AND DedDetails='" + row2["DedDetails"] + "' AND AmtFromEmp>0 AND DedTypeId>0 ORDER BY DedTypeId ASC");
                        if (str7.Trim() != string.Empty)
                        {
                            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(str7).ToString("0.00") + "</td>");
                            num14 += Convert.ToDecimal(str7);
                        }
                        else
                        {
                            str7 = "0";
                            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                        }
                        ViewState["totDed" + row2["DedDetails"]] = (Convert.ToDecimal(ViewState["totDed" + row2["DedDetails"]]) + Convert.ToDecimal(str7));
                    }
                    string str8 = objDAL.ExecuteScalarQry("SELECT SUM(RecAmt) FROM dbo.HR_EmpLoanRecovery WHERE EmpId=" + row1["EmpId"].ToString() + " AND CalYear=" + drpYear.SelectedValue + " AND CalMonth='" + drpMonth.SelectedValue.ToUpper() + "' AND RcvdStatus=1 AND RecAmt>0 AND (RTRIM(Remarks) like 'Loan Principal Recovery%' OR RTRIM(Remarks) like 'Loan Interest Recovery%')");
                    if (str8.Trim() != string.Empty)
                    {
                        num14 += Convert.ToDecimal(str8.Trim());
                        stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(str8).ToString("0.00") + "</td>");
                        num7 += Convert.ToDecimal(str8.Trim());
                    }
                    else
                        stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                    string str9 = objDAL.ExecuteScalarQry("SELECT AmtFromEmp FROM HR_view_MonthlyDeductionsNew WHERE SalaryId=" + row1["SalId"].ToString() + " AND DedTypeId=0");
                    if (str9.Trim() != string.Empty)
                    {
                        num14 += Convert.ToDecimal(str9.Trim());
                        stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(str9).ToString("0.00") + "</td>");
                        num8 += Convert.ToDecimal(str9.Trim());
                    }
                    else
                        stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                    string str10 = objDAL.ExecuteScalarQry("SELECT AmtFromEmp FROM HR_view_MonthlyDeductionsNew WHERE SalaryId=" + row1["SalId"].ToString() + " AND DedTypeId=-1");
                    if (str10.Trim() != string.Empty)
                    {
                        num14 += Convert.ToDecimal(str10.Trim());
                        stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(str10).ToString("0.00") + "</td>");
                        num9 += Convert.ToDecimal(str10.Trim());
                    }
                    else
                        stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                    Decimal num15 = Math.Round(Convert.ToDecimal(row1["GrossTot"].ToString()) - num14);
                    Decimal num16 = num15 + num13;
                    Decimal num17 = num13 - (num16 - Convert.ToDecimal(row1["PaidAmount"]));
                    stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + num14.ToString("0.00") + "</td>");
                    stringBuilder2.Append("</tr>");
                    num2 += Convert.ToDecimal(row1["Pay"]);
                    num3 += Convert.ToDecimal(row1["DA"]);
                    num4 += Convert.ToDecimal(row1["GrossTot"]);
                    num6 += num14;
                    num10 += num17;
                    num5 = num5 + num15 + num17;
                }
                else
                {
                    stringBuilder2.Append("<td style='border-bottom:solid 1px;' align='center' colspan='" + (num1 - 4) + "' ><b>Pay Withheld</b></td>");
                    stringBuilder2.Append("</tr>");
                }
                ++num11;
            }
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;' colspan='2'><b>Total</b></td>");
            if (dataTableQry2.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry2.Rows)
                    stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + Convert.ToDecimal(ViewState["totDed" + row["DedDetails"]]).ToString("0.00") + "</b></td>");
            }
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num7.ToString("0.00") + "</b></td>");
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num8.ToString("0.00") + "</b></td>");
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num9.ToString("0.00") + "</b></td>");
            stringBuilder2.Append("<td align='right' style='border-bottom:solid 1px;'><b>" + num6.ToString("0.00") + "</b></td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("</table>");
            lblReport.Text = stringBuilder2.ToString();
            Session["rptPayBill"] = finalReport(stringBuilder2.ToString());
        }
        else
        {
            lblReport.Font.Bold = true;
            lblReport.Text = "Bill not yet Generated for the Month of " + drpMonth.SelectedItem.Text.Trim().ToUpper() + "-" + drpYear.SelectedValue.ToString();
            Session["rptPayBill"] = null;
        }
    }

    private string Information()
    {
        DataTable dataTable1 = new DataTable();
        DataSet dataSet = new DataSet();
        StringBuilder stringBuilder = new StringBuilder();
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
                    stringBuilder.Append(objDAL.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + ", ");
                    stringBuilder.Append(objDAL.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + ", " + objDAL.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper());
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected string finalReport(string strRpt)
    {
        return strRpt;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (Session["rptPayBill"] != null)
            Response.Redirect("rptPayBillPrint.aspx");
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnPrint, btnPrint.GetType(), "ShowMessage", "alert('No Data Exists to Print');", true);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["rptPayBill"] != null)
                ExportToExcel(Session["rptPayBill"].ToString());
            else
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('No Data Exists to Export');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + str);
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}