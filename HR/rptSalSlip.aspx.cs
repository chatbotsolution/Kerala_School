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


public partial class HR_rptSalSlip : System.Web.UI.Page
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
            Session["rptPaySlip"] = null;
            bindDrpYear();
            bindDropDown(drpDesgn, "SELECT DesgId, Designation FROM dbo.HR_DesignationMaster ORDER BY Designation", "Designation", "DesgId");
            drpDesgn.Items.RemoveAt(0);
            drpDesgn.Items.Insert(0, new ListItem("- ALL -", "0"));
            bindDropDown(drpEmp, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster Where ActiveStatus=1 ORDER BY EmpName", "EmpName", "EmpId");
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

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = objDAL.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    private string schoolInfo()
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
                    stringBuilder.Append("<table width='90%' cellpadding='2' cellspacing='2'>");
                    stringBuilder.Append("<tr><td rowspan='4'> <img src='../images/leftlogo.jpg' width='75px' height='80px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + objDAL.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + objDAL.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + ", " + objDAL.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</strong></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + objDAL.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + objDAL.Decrypt(row["Pin"].ToString().Trim(), str2) + "</strong> </td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>Phone-" + objDAL.Decrypt(row["Phone"].ToString().Trim(), str2) + "</strong></td></tr><tr><td colspan='5'></td></tr></table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected void generateReport()
    {
        if (drpMonth.SelectedIndex <= 0 || drpYear.SelectedIndex <= 0 || drpEmp.SelectedIndex <= 0)
            return;
        DataTable dataTable1 = new DataTable();
        DataTable dataTableQry1 = objDAL.GetDataTableQry("select * from dbo.HR_view_MonthlySalaryNew where SalPaidDate IS NOT NULL and Month='" + drpMonth.SelectedValue.ToString() + "' and Year=" + drpYear.SelectedValue.ToString() + " and EmpId=" + drpEmp.SelectedValue.ToString());
        if (dataTableQry1.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Hashtable hashtable = new Hashtable();
            hashtable.Clear();
            hashtable.Add("@EmpId", drpEmp.SelectedValue);
            hashtable.Add("@Month", drpMonth.SelectedValue);
            hashtable.Add("@Year", drpYear.SelectedValue);
            DataTable dataTable2 = objDAL.GetDataTable("HR_GetPrevAmt", hashtable);
            string str1 = dataTable2.Rows[0]["ClaimAmt"].ToString();
            string str2 = dataTable2.Rows[0]["PrevAmt"].ToString();
            string str3 = dataTable2.Rows[0]["Arrear"].ToString();
            if (str1.Trim() == string.Empty)
                ;
            if (str2.Trim() == string.Empty)
                str2 = "0";
            if (str3.Trim() == string.Empty)
                str3 = "0";
            double num1 = Convert.ToDouble(str2) + Convert.ToDouble(str3);
            stringBuilder.Append("<table width='90%' cellpadding='0' cellspacing='0' style='font-family:Calibri;background-color:White;padding:10px;border:solid 1px'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='height: 30px;' colspan='3'></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center'>");
            stringBuilder.Append("<span style='font-size: 19px; font-weight: bold;'>" + schoolInfo() + "</span>");
            stringBuilder.Append("<hr />");
            stringBuilder.Append("<span style='font-weight: bold;'>");
            stringBuilder.Append("PAY SLIP FOR MONTH OF " + drpMonth.SelectedItem.Text.Trim().ToUpper() + " - " + drpYear.SelectedValue.ToString() + "</span>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center' colspan='3'>");
            stringBuilder.Append("<table width='100%' border='0' cellpadding='0' cellspacing='0'>");
            stringBuilder.Append("<tr style='background-color: Silver;'>");
            stringBuilder.Append("<td style='padding-left: 100px;' width='100px'>Employee Name</td>");
            stringBuilder.Append("<td align='left'>&nbsp;:&nbsp;" + dataTableQry1.Rows[0]["EmpName"].ToString().ToUpper() + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr style='background-color: Silver;'>");
            stringBuilder.Append("<td style='padding-left: 100px;'>Designation</td>");
            stringBuilder.Append("<td align='left'>&nbsp;:&nbsp;" + dataTableQry1.Rows[0]["Designation"].ToString() + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='font-weight: bold; padding-left: 10px;' colspan='3'>Salary Details<hr /></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='right' colspan='3' style='padding-right: 10px;'>");
            stringBuilder.Append("<table width='90%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left'>Basic</td>");
            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry1.Rows[0]["Pay"].ToString()).ToString("0.00") + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left'>DA</td>");
            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTableQry1.Rows[0]["DA"].ToString()).ToString("0.00") + "</td>");
            stringBuilder.Append("</tr>");
            DataTable dataTable3 = new DataTable();
            foreach (DataRow row in (InternalDataCollectionBase)objDAL.GetDataTableQry("SELECT Amount,Allowance FROM HR_view_MonthlyAllowancesNew WHERE SalaryId=" + dataTableQry1.Rows[0]["SalId"].ToString() + " ORDER BY AllowanceId ASC").Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left'>" + row["Allowance"].ToString() + "</td>");
                stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["Amount"].ToString()).ToString("0.00") + "</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("<tr style='font-weight: bold;'>");
            stringBuilder.Append("<td align='left'>Gross Salary</td>");
            stringBuilder.Append("<td align='right'>" + Convert.ToDouble(dataTableQry1.Rows[0]["GrossTot"].ToString()).ToString("0.00") + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='height: 20px;' colspan='3'></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='font-weight: bold; padding-left: 10px;' colspan='3'>Deduction Details<hr /></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='right' colspan='3' style='padding-right: 10px;'>");
            double num2 = 0.0;
            DataTable dataTable4 = new DataTable();
            DataTable dataTableQry2 = objDAL.GetDataTableQry("SELECT DedDetails, AmtFromEmp FROM dbo.HR_view_MonthlyDeductionsNew WHERE SalaryId=" + dataTableQry1.Rows[0]["SalId"].ToString() + " AND AmtFromEmp>0");
            stringBuilder.Append("<table width='90%'>");
            foreach (DataRow row in (InternalDataCollectionBase)dataTableQry2.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left'>" + row["DedDetails"].ToString() + "</td>");
                stringBuilder.Append("<td align='right'>" + Convert.ToDouble(row["AmtFromEmp"]).ToString("0.00") + "</td>");
                stringBuilder.Append("</tr>");
                num2 += Convert.ToDouble(row["AmtFromEmp"]);
            }
            DataTable dataTable5 = objDAL.GetDataTable("HR_GetLoanDed", new Hashtable()
      {
        {
           "@EmpId",
           drpEmp.SelectedValue
        },
        {
           "@Month",
           drpMonth.SelectedItem.Text
        },
        {
           "@Year",
           drpYear.SelectedValue
        }
      });
            if (dataTable5.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTable5.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left'>" + row["LoanType"].ToString() + "</td>");
                    stringBuilder.Append("<td align='right'>" + Convert.ToDouble(row["LoanAmt"]).ToString("0.00") + "</td>");
                    stringBuilder.Append("</tr>");
                    num2 += Convert.ToDouble(row["LoanAmt"]);
                }
            }
            stringBuilder.Append("<tr style='font-weight:bold;'>");
            stringBuilder.Append("<td align='left'>Total Deductions</td>");
            stringBuilder.Append("<td align='right'>" + num2.ToString("0.00") + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='height:20px;'>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            double num3 = Math.Round(Convert.ToDouble(dataTableQry1.Rows[0]["GrossTot"].ToString()) - num2);
            double num4 = num3 + num1;
            double num5 = num1 - (num4 - Convert.ToDouble(dataTableQry1.Rows[0]["PaidAmount"]));
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' style='font-weight: bold; padding-left: 10px;'>Net Payable</td>");
            stringBuilder.Append("<td align='right' style='font-weight: bold; padding-right: 10px;'>" + (Convert.ToDouble(dataTableQry1.Rows[0]["PaidAmount"].ToString()) - num5).ToString("0.00") + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='2'>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' style='font-weight: bold; padding-left: 10px;'>Arrear</td>");
            stringBuilder.Append("<td align='right' style='font-weight: bold; padding-right: 10px;'>" + num5.ToString("0.00") + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='2'>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' style='font-weight: bold; padding-left: 10px;'>Net Salary Paid</td>");
            stringBuilder.Append("<td align='right' style='font-weight: bold; padding-right: 10px;'>" + (num3 + num5).ToString("0.00") + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' style='font-size:12px; padding-left: 10px;text-align:left;' colspan='3'>");
            stringBuilder.Append("<div style='height:60px;'></div>");
            stringBuilder.Append("<div style='float:left;width:30%;'>" + dataTableQry1.Rows[0]["EmpName"].ToString() + "</div>");
            stringBuilder.Append("<div style='float:right;width:70%;text-align:right;'>For Saraswati Sishu Vidya Mandir</div>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString();
            Session["rptPaySlip"] = finalReport(stringBuilder.ToString());
        }
        else
        {
            lblReport.Font.Bold = true;
            lblReport.Text = "Pay Slip not Generated for Month of " + drpMonth.SelectedItem.Text.Trim().ToUpper() + "-" + drpYear.SelectedValue.ToString();
            Session["rptPaySlip"] = null;
        }
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
        if (Session["rptPaySlip"] != null)
            Response.Redirect("rptPaySlipPrint.aspx");
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnPrint, btnPrint.GetType(), "ShowMessage", "alert('No Data Exists to Print');", true);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["rptPaySlip"] != null)
                ExportToExcel(Session["rptPaySlip"].ToString());
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

    protected void drpDesgn_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDesgn.SelectedIndex > 0)
            bindDropDown(drpEmp, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster WHERE DesignationId=" + drpDesgn.SelectedValue.ToString() + " ORDER BY EmpName", "EmpName", "EmpId");
        else
            bindDropDown(drpEmp, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster ORDER BY EmpName", "EmpName", "EmpId");
        drpDesgn.Focus();
    }
}