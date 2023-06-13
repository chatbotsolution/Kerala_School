using ASP;
using SanLib;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class HR_rptPayBill : System.Web.UI.Page
{
    private clsDAL objDAL = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Session["rptPayBill"] = null;
        bindDrpYear();
    }

    private void bindDrpYear()
    {
        int year = DateTime.Now.Year;
        drpYear.Items.Insert(0, new ListItem("--Select--", "0"));
        for (int index = 1; index < 11; ++index)
        {
            drpYear.Items.Insert(index, new ListItem(year.ToString(), year.ToString()));
            --year;
        }
    }

    protected void generateReport()
    {
        if (drpMonth.SelectedIndex > 0 && drpYear.SelectedIndex > 0)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTableQry1 = objDAL.GetDataTableQry("select * from HR_view_MonthlySalary where Month='" + drpMonth.SelectedValue.ToString() + "' and Year=" + drpYear.SelectedValue.ToString() + " order by DesignationId,EmpName");
            string str1 = "select BillNo from dbo.HR_GenerateMlySalary where SalYear='" + drpYear.SelectedValue.ToString().Trim() + "' and SalMonth='" + drpMonth.SelectedValue.ToString().Trim() + "'";
            objDAL = new clsDAL();
            string str2 = objDAL.ExecuteScalarQry(str1);
            if (dataTableQry1.Rows.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                string empty1 = string.Empty;
                string empty2 = string.Empty;
                stringBuilder.Append("<table width='100%' border='1' cellpadding='2' cellspacing='0' style='border: solid 2px Black;'>");
                stringBuilder.Append("<tr><td colspan='11' style='font-weight: bold; font-size: 22px; background-color: Silver;padding-bottom:5px;' align='center'>PAY BILL FOR MONTH OF " + drpMonth.SelectedItem.Text.Trim().ToUpper() + " -" + drpYear.SelectedValue.ToString() + "</td></tr>");
                stringBuilder.Append("<tr><td colspan='11' style='font-weight: bold; font-size: 22px; background-color: Silver;padding-bottom:5px;' align='center'>BILL No- " + str2 + "</td></tr>");
                stringBuilder.Append("</table><div style='height:5px;'></div>");
                int num1 = 1;
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry1.Rows)
                {
                    DataTable dataTable2 = new DataTable();
                    string str3 = "SELECT DedDetails, AmtFromEmp FROM HR_view_MonthlyDeductions WHERE SalaryId=" + row["SalId"].ToString() + " AND AmtFromEmp>0";
                    DataTable dataTableQry2 = objDAL.GetDataTableQry("SELECT DedDetails, AmtFromEmp FROM HR_view_MonthlyDeductions WHERE SalaryId=" + row["SalId"].ToString() + " AND AmtFromEmp>0");
                    double num2 = Convert.ToDouble(row["GrossTot"].ToString()) - Convert.ToDouble(dataTableQry2.Compute("Sum(AmtFromEmp)", "").ToString());
                    stringBuilder.Append("<table width='100%' border='1' cellpadding='2' cellspacing='0' style='border: solid 2px Black;'>");
                    stringBuilder.Append("<tr style='font-weight: bold; font-size: 18px; background-color: White'>");
                    stringBuilder.Append("<td align='center' colspan='10'>" + row["EmpName"].ToString() + ", " + row["Designation"].ToString() + "</td></tr>");
                    if (row["PayWithHeld"].ToString().Trim() == "N")
                    {
                        stringBuilder.Append("<tr style='font-weight: bold;'>");
                        stringBuilder.Append("<td align='center'>Sl No.</td>");
                        stringBuilder.Append("<td align='right'>Pay</td>");
                        stringBuilder.Append("<td align='right'>DA</td>");
                        stringBuilder.Append("<td align='right'>HRA</td>");
                        stringBuilder.Append("<td align='center' colspan='2'>Allowances</td>");
                        stringBuilder.Append("<td align='right'>Gross Total</td>");
                        stringBuilder.Append("<td align='center' colspan='2'>Total Deduction</td>");
                        stringBuilder.Append("<td align='right'>Net Payable</td></tr>");
                        stringBuilder.Append("<tr><td align='center'>" + num1.ToString() + "</td>");
                        stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["Pay"].ToString()).ToString("0.00") + "</td>");
                        stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["DA"].ToString()).ToString("0.00") + "</td>");
                        stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["HRA"].ToString()).ToString("0.00") + "</td>");
                        stringBuilder.Append("<td align='left' style='font-weight: bold;'>Tranining Allowance</td>");
                        stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["TrainingAllowance"].ToString()).ToString("0.00") + "</td>");
                        stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["GrossTot"].ToString()).ToString("0.00") + "</td>");
                        if (dataTableQry2.Rows.Count > 3)
                        {
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>" + dataTableQry2.Rows[0]["DedDetails"].ToString() + "</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry2.Rows[0]["AmtFromEmp"]).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td align='right' style='font-weight: bold;'>" + Convert.ToDecimal(num2.ToString()).ToString("0.00") + "</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Local Allowance</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["LocalAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>" + dataTableQry2.Rows[1]["DedDetails"].ToString() + "</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry2.Rows[1]["AmtFromEmp"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td rowspan='10'>&nbsp;</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Special Allowance</td>");
                            stringBuilder.Append("<td align='right'>&nbsp;" + Convert.ToDecimal(row["SpecialAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>" + dataTableQry2.Rows[2]["DedDetails"].ToString() + "</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry2.Rows[2]["AmtFromEmp"].ToString()).ToString("0.00") + "</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Other Allowance</td>");
                            stringBuilder.Append("<td align='right'>&nbsp;" + Convert.ToDecimal(row["OtherAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>" + dataTableQry2.Rows[3]["DedDetails"].ToString() + "</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry2.Rows[3]["AmtFromEmp"].ToString()).ToString("0.00") + "</td></tr>");
                        }
                        else if (dataTableQry2.Rows.Count > 2)
                        {
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>" + dataTableQry2.Rows[0]["DedDetails"].ToString() + "</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry2.Rows[0]["AmtFromEmp"]).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td align='right' style='font-weight: bold;'>" + Convert.ToDecimal(num2.ToString()).ToString("0.00") + "</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Local Allowance</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["LocalAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>" + dataTableQry2.Rows[1]["DedDetails"].ToString() + "</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry2.Rows[1]["AmtFromEmp"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td rowspan='10'>&nbsp;</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Special Allowance</td>");
                            stringBuilder.Append("<td align='right'>&nbsp;" + Convert.ToDecimal(row["SpecialAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>" + dataTableQry2.Rows[2]["DedDetails"].ToString() + "</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry2.Rows[2]["AmtFromEmp"].ToString()).ToString("0.00") + "</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Other Allowance</td>");
                            stringBuilder.Append("<td align='right'>&nbsp;" + Convert.ToDecimal(row["OtherAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td>&nbsp;</td>");
                            stringBuilder.Append("<td align='right' style='font-weight: bold;'>Total=</td>");
                            stringBuilder.Append("<td align='right' style='font-weight: bold;'>" + Convert.ToDecimal(dataTableQry2.Compute("Sum(AmtFromEmp)", "")).ToString("0.00") + "</td></tr>");
                        }
                        else if (dataTableQry2.Rows.Count > 1)
                        {
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>" + dataTableQry2.Rows[0]["DedDetails"].ToString() + "</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry2.Rows[0]["AmtFromEmp"]).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td align='right' style='font-weight: bold;'>" + Convert.ToDecimal(num2.ToString()).ToString("0.00") + "</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Local Allowance</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["LocalAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>" + dataTableQry2.Rows[1]["DedDetails"].ToString() + "</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry2.Rows[1]["AmtFromEmp"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td rowspan='10'>&nbsp;</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Special Allowance</td>");
                            stringBuilder.Append("<td align='right'>&nbsp;" + Convert.ToDecimal(row["SpecialAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td>&nbsp;</td>");
                            stringBuilder.Append("<td align='right' style='font-weight: bold;'>Total=</td>");
                            stringBuilder.Append("<td align='right' style='font-weight: bold;'>" + Convert.ToDecimal(dataTableQry2.Compute("Sum(AmtFromEmp)", "")).ToString("0.00") + "</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Other Allowance</td>");
                            stringBuilder.Append("<td align='right'>&nbsp;" + Convert.ToDecimal(row["OtherAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("</tr>");
                        }
                        else if (dataTableQry2.Rows.Count > 0)
                        {
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>" + dataTableQry2.Rows[0]["DedDetails"].ToString() + "</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry2.Rows[0]["AmtFromEmp"]).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td align='right' style='font-weight: bold;'>" + Convert.ToDecimal(num2.ToString()).ToString("0.00") + "</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Local Allowance</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["LocalAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td>&nbsp;</td>");
                            stringBuilder.Append("<td align='right' style='font-weight: bold;'>Total=</td>");
                            stringBuilder.Append("<td align='right' style='font-weight: bold;'>" + Convert.ToDecimal(dataTableQry2.Compute("Sum(AmtFromEmp)", "")).ToString("0.00") + "</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Special Allowance</td>");
                            stringBuilder.Append("<td align='right'>&nbsp;" + Convert.ToDecimal(row["SpecialAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("</tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Other Allowance</td>");
                            stringBuilder.Append("<td align='right'>&nbsp;" + Convert.ToDecimal(row["OtherAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("</tr>");
                        }
                        else
                        {
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Local Allowance</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["LocalAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td colspan='4'>&nbsp;</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Special Allowance</td>");
                            stringBuilder.Append("<td align='right'>&nbsp;" + Convert.ToDecimal(row["SpecialAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td colspan='4'>&nbsp;</td></tr>");
                            stringBuilder.Append("<tr><td align='right' colspan='4'>&nbsp;</td>");
                            stringBuilder.Append("<td align='left' style='font-weight: bold;'>Other Allowance</td>");
                            stringBuilder.Append("<td align='right'>&nbsp;" + Convert.ToDecimal(row["OtherAllowance"].ToString()).ToString("0.00") + "</td>");
                            stringBuilder.Append("<td colspan='4'>&nbsp;</td></tr>");
                        }
                        if (dataTableQry2.Rows.Count >= 4)
                        {
                            for (int index = 4; index < dataTableQry2.Rows.Count; ++index)
                            {
                                stringBuilder.Append("<tr><td colspan='7'>&nbsp;</td>");
                                stringBuilder.Append("<td align='left' style='font-weight: bold;'>" + dataTableQry2.Rows[index]["DedDetails"].ToString() + "</td>");
                                stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry2.Rows[index]["AmtFromEmp"].ToString()).ToString("0.00") + "</td></tr>");
                            }
                            stringBuilder.Append("<tr style='font-weight: bold;'>");
                            stringBuilder.Append("<td colspan='7'></td>");
                            stringBuilder.Append("<td align='right'>Total=</td>");
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry2.Compute("Sum(AmtFromEmp)", "")).ToString("0.00") + "</td></tr>");
                        }
                        ++num1;
                    }
                    else
                        stringBuilder.Append("<tr style='font-weight: bold; font-size: 18px; background-color: White'><td align='center' colspan='11'>Pay Withheld</td></tr>");
                    stringBuilder.Append("</table>");
                    stringBuilder.Append("<div style='height:30px;'>&nbsp;</div>");
                }
                lblReport.Text = stringBuilder.ToString();
                Session["rptPayBill"] = finalReport(stringBuilder.ToString());
            }
            else
            {
                lblReport.Font.Bold = true;
                lblReport.Text = "Bill not yet generated for month of " + drpMonth.SelectedItem.Text.Trim().ToUpper() + "-" + drpYear.SelectedValue.ToString() + " !";
                Session["rptPayBill"] = null;
            }
        }
        rowdata.Visible = true;
    }

    protected string finalReport(string strRpt)
    {
        return strRpt;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        generateReport();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (Session["rptPayBill"] != null)
            Response.Redirect("rptPayBillPrint.aspx");
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnPrint, btnPrint.GetType(), "ShowMessage", "alert('No data exists to print !');", true);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["rptPayBill"] != null)
                ExportToExcel(Session["rptPayBill"].ToString());
            else
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('No data exists to export !');", true);
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