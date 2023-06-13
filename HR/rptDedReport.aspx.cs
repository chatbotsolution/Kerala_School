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


public partial class HR_rptDedReport : System.Web.UI.Page
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
            bindDed();
        }
    }

    private void bindDed()
    {
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        objDAL = new clsDAL();
        drpDed.DataSource = objDAL.GetDataTableQry("SELECT DISTINCT DedTypeId,DedDetails FROM HR_view_MonthlyDeductionsNew WHERE DedTypeId>0 ORDER BY DedTypeId ASC");
        drpDed.DataTextField = "DedDetails";
        drpDed.DataValueField = "DedTypeId";
        drpDed.DataBind();
        drpDed.Items.Insert(0, new ListItem("- Select -", "0"));
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
        StringBuilder stringBuilder1 = new StringBuilder();
        DataTable dataTable1 = new DataTable();
        stringBuilder1.Append("SELECT * FROM HR_view_MonthlySalaryNew WHERE Month='" + drpMonth.SelectedValue.ToString() + "' AND Year=" + drpYear.SelectedValue.ToString() + " AND (SalPaidDate IS NOT NULL OR PayWithHeld='Y' )");
        stringBuilder1.Append("ORDER BY DesignationId,EmpName");
        DataTable dataTableQry1 = objDAL.GetDataTableQry(stringBuilder1.ToString());
        string str1 = "SELECT BillNo FROM dbo.HR_GenerateMlySalary WHERE SalYear='" + drpYear.SelectedValue.ToString().Trim() + "' AND SalMonth='" + drpMonth.SelectedValue.ToString().Trim() + "'";
        objDAL = new clsDAL();
        objDAL.ExecuteScalarQry(str1);
        if (dataTableQry1.Rows.Count > 0)
        {
            StringBuilder stringBuilder2 = new StringBuilder();
            string str2 = Information();
            stringBuilder2.Append("<table width='100%' cellpadding='1' cellspacing='0'>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td style='font-size: 14px; background-color: Silver;' align='center'><b>" + str2 + "</b></td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td style='font-size: 14px; background-color: Silver;' align='center'><b>");
            stringBuilder2.Append(drpDed.SelectedItem.ToString() + " FOR THE MONTH OF " + drpMonth.SelectedItem.Text.Trim().ToUpper() + " - " + drpYear.SelectedValue.ToString() + ", </b>");
            stringBuilder2.Append("</td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("<div style='height:5px;'></div>");
            stringBuilder2.Append("<table width='100%' cellpadding='1' cellspacing='0' style='border:solid 1px;border-bottom:none 1px;'>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;' width='10px'><b>S.No.</b></td>");
            stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;' width='80px'><b>Name</b></td>");
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>Basic</b></td>");
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'  width='45px'><b>D.A</b></td>");
            DataTable dataTableQry2 = objDAL.GetDataTableQry("SELECT DISTINCT AllowanceId,Allowance FROM HR_view_MonthlyAllowancesNew WHERE AllowanceId>0 ORDER BY AllowanceId ASC");
            if (dataTableQry2.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry2.Rows)
                {
                    stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>" + row["Allowance"].ToString() + "</b></td>");
                    ViewState["totAllw" + row["Allowance"]] = 0;
                    ++num1;
                }
            }
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'  width='45px'><b>Gross Total</b></td>");
            DataTable dataTableQry3 = objDAL.GetDataTableQry("SELECT DISTINCT DedTypeId,DedDetails FROM HR_view_MonthlyDeductionsNew WHERE DedTypeId>0 and DedDetails='" + drpDed.SelectedItem.ToString() + "' ORDER BY DedTypeId ASC");
            if (dataTableQry3.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry3.Rows)
                {
                    if (row["DedTypeId"].ToString() != "0")
                    {
                        stringBuilder2.Append("<td align='Center' style='border-right:solid 1px;border-bottom:solid 1px;' width='45px'><b>" + row["DedDetails"].ToString() + "</b></td>");
                        ViewState["totDed" + row["DedDetails"]] = 0;
                        ++num1;
                    }
                }
            }
            stringBuilder2.Append("</tr>");
            int num8 = 1;
            foreach (DataRow row1 in (InternalDataCollectionBase)dataTableQry1.Rows)
            {
                Decimal num9 = new Decimal(0);
                Hashtable hashtable = new Hashtable();
                hashtable.Clear();
                hashtable.Add("@EmpId", row1["EmpId"]);
                hashtable.Add("@Month", drpMonth.SelectedValue);
                hashtable.Add("@Year", drpYear.SelectedValue);
                DataTable dataTable2 = objDAL.GetDataTable("HR_GetPrevAmt", hashtable);
                string str3 = dataTable2.Rows[0]["ClaimAmt"].ToString();
                string str4 = dataTable2.Rows[0]["PrevAmt"].ToString();
                string str5 = dataTable2.Rows[0]["Arrear"].ToString();
                if (str3.Trim() == string.Empty)
                    ;
                if (str4.Trim() == string.Empty)
                    str4 = "0";
                if (str5.Trim() == string.Empty)
                    str5 = "0";
                Decimal num10 = Convert.ToDecimal(str4) + Convert.ToDecimal(str5);
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;'>" + num8 + "</td>");
                stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;'>" + row1["EmpName"] + " (" + row1["Designation"] + ")</td>");
                if (row1["PayWithHeld"].ToString().Trim() == "N")
                {
                    stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(row1["Pay"]).ToString("0.00") + "</td>");
                    stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(row1["DA"]).ToString("0.00") + "</td>");
                    foreach (DataRow row2 in (InternalDataCollectionBase)dataTableQry2.Rows)
                    {
                        string str6 = objDAL.ExecuteScalarQry("SELECT Amount FROM HR_view_MonthlyAllowancesNew  WHERE SalaryId=" + row1["SalId"].ToString() + " AND Allowance='" + row2["Allowance"] + "' ORDER BY AllowanceId ASC");
                        if (str6.Trim() != string.Empty)
                        {
                            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(str6).ToString("0.00") + "</td>");
                        }
                        else
                        {
                            str6 = "0";
                            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                        }
                        ViewState["totAllw" + row2["Allowance"]] = (Convert.ToDecimal(ViewState["totAllw" + row2["Allowance"]]) + Convert.ToDecimal(str6));
                    }
                    stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(row1["GrossTot"]).ToString("0.00") + "</td>");
                    Decimal num11 = new Decimal(0);
                    foreach (DataRow row2 in (InternalDataCollectionBase)dataTableQry3.Rows)
                    {
                        string str6 = objDAL.ExecuteScalarQry("SELECT AmtFromEmp FROM HR_view_MonthlyDeductionsNew WHERE SalaryId=" + row1["SalId"].ToString() + " AND DedDetails='" + row2["DedDetails"] + "' AND AmtFromEmp>0 AND DedTypeId>0 ORDER BY DedTypeId ASC");
                        if (str6.Trim() != string.Empty)
                        {
                            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(str6).ToString("0.00") + "</td>");
                            num11 += Convert.ToDecimal(str6);
                        }
                        else
                        {
                            str6 = "0";
                            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                        }
                        ViewState["totDed" + row2["DedDetails"]] = (Convert.ToDecimal(ViewState["totDed" + row2["DedDetails"]]) + Convert.ToDecimal(str6));
                    }
                    Decimal num12 = Math.Round(Convert.ToDecimal(row1["GrossTot"].ToString()) - num11);
                    Decimal num13 = num12 + num10;
                    Decimal num14 = num10 - (num13 - Convert.ToDecimal(row1["PaidAmount"]));
                    stringBuilder2.Append("</tr>");
                    num2 += Convert.ToDecimal(row1["Pay"]);
                    num3 += Convert.ToDecimal(row1["DA"]);
                    num4 += Convert.ToDecimal(row1["GrossTot"]);
                    num6 += num11;
                    num7 += num14;
                    num5 = num5 + num12 + num14;
                }
                else
                {
                    stringBuilder2.Append("<td style='border-bottom:solid 1px;' align='center' colspan='" + (num1 - 4) + "' ><b>Pay Withheld</b></td>");
                    stringBuilder2.Append("</tr>");
                }
                ++num8;
            }
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='center' style='border-right:solid 1px;border-bottom:solid 1px;' colspan='2'><b>Total</b></td>");
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num2.ToString("0.00") + "</b></td>");
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num3.ToString("0.00") + "</b></td>");
            if (dataTableQry2.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry2.Rows)
                    stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + Convert.ToDecimal(ViewState["totAllw" + row["Allowance"]]).ToString("0.00") + "</b></td>");
            }
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + num4.ToString("0.00") + "</b></td>");
            if (dataTableQry3.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry3.Rows)
                    stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'><b>" + Convert.ToDecimal(ViewState["totDed" + row["DedDetails"]]).ToString("0.00") + "</b></td>");
            }
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