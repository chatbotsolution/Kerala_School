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

public partial class HR_rptPayBillNew : System.Web.UI.Page
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
        DataTable dataTable1 = new DataTable();
        DataTable dataTableQry1 = objDAL.GetDataTableQry("SELECT * FROM HR_view_MonthlySalaryNew WHERE Month='" + drpMonth.SelectedValue.ToString() + "' AND Year=" + drpYear.SelectedValue.ToString() + " AND (SalPaidDate IS NOT NULL OR PayWithHeld='Y' ) ORDER BY DesignationId,EmpName");
        string str1 = "SELECT BillNo FROM dbo.HR_GenerateMlySalary WHERE SalYear='" + drpYear.SelectedValue.ToString().Trim() + "' AND SalMonth='" + drpMonth.SelectedValue.ToString().Trim() + "'";
        objDAL = new clsDAL();
        string str2 = objDAL.ExecuteScalarQry(str1);
        if (dataTableQry1.Rows.Count > 0)
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            string str3 = Information();
            stringBuilder1.Append("<table width='100%' cellpadding='1' cellspacing='0'>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='font-size: 14px; background-color: Silver;' align='center'><b>" + str3 + "</b></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='font-size: 14px; background-color: Silver;' align='center'><b>");
            stringBuilder1.Append("DAKSHINA BILL FOR THE MONTH OF " + drpMonth.SelectedItem.Text.Trim().ToUpper() + " - " + drpYear.SelectedValue.ToString() + ", ");
            stringBuilder1.Append("BILL NO - " + str2 + "</b>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("</table>");
            stringBuilder1.Append("<div style='height:5px;'></div>");
            stringBuilder1.Append("<table width='100%' cellpadding='1' cellspacing='0' style='border:solid 1px;border-bottom:none 1px;'>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;' width='10px'><b>S.No.</b></td>");
            stringBuilder1.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;' width='150px'><b>Name</b></td>");
            stringBuilder1.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;' width='70px'><b>Date Of Joining</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='40px'><b>Year of Service</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>Basic</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>D.A</b></td>");
            DataTable dataTableQry2 = objDAL.GetDataTableQry("SELECT DISTINCT AllowanceId,Allowance FROM HR_view_MonthlyAllowancesNew WHERE AllowanceId>0 ORDER BY AllowanceId ASC");
            if (dataTableQry2.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry2.Rows)
                {
                    stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>" + row["Allowance"].ToString() + "</b></td>");
                    ViewState["totAllw" + row["Allowance"]] = 0;
                    ++num1;
                }
            }
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>Gross Total</b></td>");
            DataTable dataTableQry3 = objDAL.GetDataTableQry("SELECT DISTINCT DedTypeId,DedDetails FROM HR_view_MonthlyDeductionsNew WHERE DedTypeId>0 ORDER BY DedTypeId ASC");
            if (dataTableQry3.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry3.Rows)
                {
                    if (row["DedTypeId"].ToString() != "0")
                    {
                        stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>" + row["DedDetails"].ToString() + "</b></td>");
                        ViewState["totDed" + row["DedDetails"]] = 0;
                        ++num1;
                    }
                }
            }
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>Loan Recovery</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>Loss of Pay</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>Misc Recovery</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>Total Ded.</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='50px'><b>Arrear</b></td>");
            stringBuilder1.Append("<td align='right' style='border-bottom:solid 1px;' width='50px'><b>Net Payment</b></td>");
            stringBuilder1.Append("</tr>");
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
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;'>" + num11 + "</td>");
                stringBuilder1.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;'>" + row1["EmpName"] + " (" + row1["Designation"] + ")</td>");
                stringBuilder1.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;'>" + row1["DOJ"] + "</td>");
                stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + row1["NoOfYear"] + "+</td>");
                if (row1["PayWithHeld"].ToString().Trim() == "N")
                {
                    StringBuilder stringBuilder2 = stringBuilder1;
                    string str7 = "<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>";
                    Decimal num14 = Convert.ToDecimal(row1["Pay"]);
                    string str8 = num14.ToString("0.00");
                    string str9 = "</td>";
                    string str10 = str7 + str8 + str9;
                    stringBuilder2.Append(str10);
                    stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(row1["DA"]).ToString("0.00") + "</td>");
                    foreach (DataRow row2 in (InternalDataCollectionBase)dataTableQry2.Rows)
                    {
                        string str11 = objDAL.ExecuteScalarQry("SELECT Amount FROM HR_view_MonthlyAllowancesNew  WHERE SalaryId=" + row1["SalId"].ToString() + " AND Allowance='" + row2["Allowance"] + "' ORDER BY AllowanceId ASC");
                        if (str11.Trim() != string.Empty)
                        {
                            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(str11).ToString("0.00") + "</td>");
                        }
                        else
                        {
                            str11 = "0";
                            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                        }
                        ViewState["totAllw" + row2["Allowance"]] = (Convert.ToDecimal(ViewState["totAllw" + row2["Allowance"]]) + Convert.ToDecimal(str11));
                    }
                    stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(row1["GrossTot"]).ToString("0.00") + "</td>");
                    Decimal num15 = new Decimal(0);
                    foreach (DataRow row2 in (InternalDataCollectionBase)dataTableQry3.Rows)
                    {
                        string str11 = objDAL.ExecuteScalarQry("SELECT AmtFromEmp FROM HR_view_MonthlyDeductionsNew WHERE SalaryId=" + row1["SalId"].ToString() + " AND DedDetails='" + row2["DedDetails"] + "' AND AmtFromEmp>0 AND DedTypeId>0 ORDER BY DedTypeId ASC");
                        if (str11.Trim() != string.Empty)
                        {
                            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(str11).ToString("0.00") + "</td>");
                            num15 += Convert.ToDecimal(str11);
                        }
                        else
                        {
                            str11 = "0";
                            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                        }
                        ViewState["totDed" + row2["DedDetails"]] = (Convert.ToDecimal(ViewState["totDed" + row2["DedDetails"]]) + Convert.ToDecimal(str11));
                    }
                    string str12 = objDAL.ExecuteScalarQry("SELECT SUM(RecAmt) FROM dbo.HR_EmpLoanRecovery WHERE EmpId=" + row1["EmpId"].ToString() + " AND CalYear=" + drpYear.SelectedValue + " AND CalMonth='" + drpMonth.SelectedValue.ToUpper() + "' AND RcvdStatus=1 AND RecAmt>0 AND (RTRIM(Remarks) like 'Loan Principal Recovery%' OR RTRIM(Remarks) like 'Loan Interest Recovery%')");
                    if (str12.Trim() != string.Empty)
                    {
                        num15 += Convert.ToDecimal(str12.Trim());
                        StringBuilder stringBuilder3 = stringBuilder1;
                        string str11 = "<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>";
                        num14 = Convert.ToDecimal(str12);
                        string str13 = num14.ToString("0.00");
                        string str14 = "</td>";
                        string str15 = str11 + str13 + str14;
                        stringBuilder3.Append(str15);
                        num7 += Convert.ToDecimal(str12.Trim());
                    }
                    else
                        stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                    string str16 = objDAL.ExecuteScalarQry("SELECT AmtFromEmp FROM HR_view_MonthlyDeductionsNew WHERE SalaryId=" + row1["SalId"].ToString() + " AND DedTypeId=0");
                    if (str16.Trim() != string.Empty)
                    {
                        num15 += Convert.ToDecimal(str16.Trim());
                        StringBuilder stringBuilder3 = stringBuilder1;
                        string str11 = "<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>";
                        num14 = Convert.ToDecimal(str16);
                        string str13 = num14.ToString("0.00");
                        string str14 = "</td>";
                        string str15 = str11 + str13 + str14;
                        stringBuilder3.Append(str15);
                        num8 += Convert.ToDecimal(str16.Trim());
                    }
                    else
                        stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                    string str17 = objDAL.ExecuteScalarQry("SELECT AmtFromEmp FROM HR_view_MonthlyDeductionsNew WHERE SalaryId=" + row1["SalId"].ToString() + " AND DedTypeId=-1");
                    if (str17.Trim() != string.Empty)
                    {
                        num15 += Convert.ToDecimal(str17.Trim());
                        StringBuilder stringBuilder3 = stringBuilder1;
                        string str11 = "<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>";
                        num14 = Convert.ToDecimal(str17);
                        string str13 = num14.ToString("0.00");
                        string str14 = "</td>";
                        string str15 = str11 + str13 + str14;
                        stringBuilder3.Append(str15);
                        num9 += Convert.ToDecimal(str17.Trim());
                    }
                    else
                        stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                    Decimal num16 = Math.Round(Convert.ToDecimal(row1["GrossTot"].ToString()) - num15);
                    Decimal num17 = num16 + num13;
                    Decimal num18 = num13 - (num17 - Convert.ToDecimal(row1["PaidAmount"]));
                    stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + num15.ToString("0.00") + "</td>");
                    stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + num18.ToString("0.00") + "</td>");
                    StringBuilder stringBuilder4 = stringBuilder1;
                    string str18 = "<td align='right' style='border-bottom:solid 1px;'>";
                    num14 = Convert.ToDecimal(row1["PaidAmount"]);
                    string str19 = num14.ToString("0.00");
                    string str20 = "</td>";
                    string str21 = str18 + str19 + str20;
                    stringBuilder4.Append(str21);
                    stringBuilder1.Append("</tr>");
                    num2 += Convert.ToDecimal(row1["Pay"]);
                    num3 += Convert.ToDecimal(row1["DA"]);
                    num4 += Convert.ToDecimal(row1["GrossTot"]);
                    num6 += num15;
                    num10 += num18;
                    num5 = num5 + num16 + num18;
                }
                else
                {
                    stringBuilder1.Append("<td style='border-bottom:solid 1px;' align='center' colspan='" + (num1 - 4) + "' ><b>Pay Withheld</b></td>");
                    stringBuilder1.Append("</tr>");
                }
                ++num11;
            }
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;' colspan='4'><b>Total</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num2.ToString("0.00") + "</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num3.ToString("0.00") + "</b></td>");
            if (dataTableQry2.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry2.Rows)
                    stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + Convert.ToDecimal(ViewState["totAllw" + row["Allowance"]]).ToString("0.00") + "</b></td>");
            }
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num4.ToString("0.00") + "</b></td>");
            if (dataTableQry3.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry3.Rows)
                    stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + Convert.ToDecimal(ViewState["totDed" + row["DedDetails"]]).ToString("0.00") + "</b></td>");
            }
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num7.ToString("0.00") + "</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num8.ToString("0.00") + "</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num9.ToString("0.00") + "</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num6.ToString("0.00") + "</b></td>");
            stringBuilder1.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num10.ToString("0.00") + "</b></td>");
            stringBuilder1.Append("<td align='right' style='border-bottom:solid 1px;'><b>" + num5.ToString("0.00") + "</b></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("</table>");
            lblReport.Text = stringBuilder1.ToString();
            Session["rptPayBill"] = finalReport(stringBuilder1.ToString());
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