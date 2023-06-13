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

public partial class HR_rptBankPayroll : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private string CompanyName = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        if (!ChkIsHRUsed())
        {
            Response.Redirect("HRHome.aspx?IsHRUsed=0");
        }
        else
        {
            Session["rptBankPayroll"] = null;
            bindDrpYear();
        }
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
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

    protected void drpMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBillNo();
        drpMonth.Focus();
    }

    protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindBillNo();
        drpYear.Focus();
    }

    private void BindBillNo()
    {
        drpBillNo.DataSource = null;
        txtTo.Text = string.Empty;
        txtSubject.Text = string.Empty;
        txtDesc.Text = string.Empty;
        btnGenerate.Enabled = false;
        btnPrint.Enabled = false;
        btnExport.Enabled = false;
        DataTable dataTable = new DataTable();
        if (drpMonth.SelectedIndex > 0 && drpYear.SelectedIndex > 0)
            dataTable = obj.GetDataTableQry("SELECT BillNo FROM dbo.HR_GenerateMlySalary WHERE SalMonth='" + drpMonth.SelectedValue + "' AND SalYear=" + drpYear.SelectedValue);
        drpBillNo.DataSource = dataTable;
        drpBillNo.DataTextField = "BillNo";
        drpBillNo.DataValueField = "BillNo";
        drpBillNo.DataBind();
        if (dataTable.Rows.Count <= 0)
            return;
        drpBillNo.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    protected void drpBillNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtTo.Text = string.Empty;
        txtSubject.Text = string.Empty;
        txtDesc.Text = string.Empty;
        btnPrint.Enabled = false;
        btnExport.Enabled = false;
        if (drpBillNo.SelectedIndex > 0)
        {
            DataTable dataTable = new DataTable();
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("select ah.AcctsHead from Acts_GenLedger gl inner join dbo.Acts_AccountHeads ah on gl.AccountHead=ah.AcctsHeadId");
            stringBuilder1.Append(" where PmtRecptVoucherNo='" + drpBillNo.SelectedItem.Text.Trim() + "' and AG_Code=7");
            stringBuilder1.Append(" and Particulars like 'Salary For " + drpMonth.SelectedValue + "-" + drpYear.SelectedValue + "'");
            DataTable dataTableQry = obj.GetDataTableQry(stringBuilder1.ToString());
            if (dataTableQry.Rows.Count > 0)
            {
                txtTo.Text = "Manager, " + dataTableQry.Rows[0]["AcctsHead"].ToString();
                txtSubject.Text = "Payment of Salary into the Accounts of our teaching & non-teaching staff.";
                StringBuilder stringBuilder2 = new StringBuilder();
                stringBuilder2.Append("We are obliged to pay the monthly salary of our teaching & non-Teaching staff in shape of deposit in their respective account numbers. ");
                stringBuilder2.Append("So the salary of " + drpMonth.SelectedItem.Text + "-" + drpYear.SelectedValue + " as mentioned against the name and account number ");
                stringBuilder2.Append("of the staff may be deposited deducting the same from our account number " + dataTableQry.Rows[0]["AcctsHead"] + " & obliged.");
                txtDesc.Text = stringBuilder2.ToString().Trim();
                btnGenerate.Enabled = true;
            }
            else
                lblReport.Text = "<font style='color:red; font-size:14px;'><b>No Bank Transaction Available</b></font>";
        }
        drpBillNo.Focus();
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
                    stringBuilder.Append("<table width='97%' cellpadding='2' cellspacing='2'>");
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td rowspan='4'><img src='../images/leftlogo.jpg' width='75px' height='80px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + obj.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td>");
                    stringBuilder.Append("<td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/></td>");
                    stringBuilder.Append("</tr>");
                    CompanyName = obj.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper();
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='center' style='font-size:14px;'><strong>" + obj.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + ", " + obj.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</strong></td>");
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='center' style='font-size:14px;'><strong>" + obj.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + obj.Decrypt(row["Pin"].ToString().Trim(), str2) + "</strong></td>");
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='center' style='font-size:14px;'><strong>Phone-" + obj.Decrypt(row["Phone"].ToString().Trim(), str2) + "</strong></td></tr><tr><td colspan='5'></td>");
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("</table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        StringBuilder stringBuilder = new StringBuilder();
        DataSet dataSet = obj.GetDataSet("HR_rptPayrollForBank", new Hashtable()
    {
      {
         "@Month",
         drpMonth.SelectedValue
      },
      {
         "@Year",
         drpYear.SelectedValue
      },
      {
         "@BillNo",
         drpBillNo.SelectedValue
      }
    });
        DataTable table1 = dataSet.Tables[0];
        DataTable table2 = dataSet.Tables[1];
        if (table1.Rows.Count > 0)
        {
            stringBuilder.Append("<table width='97%' cellpadding='2' cellspacing='0' style='font-family:Calibri;background-color:White;padding:2px;border:solid 1px'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='height: 30px;' colspan='2'></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center' colspan='2'>");
            stringBuilder.Append("<span style='font-weight: bold;'>" + schoolInfo() + "</span>");
            stringBuilder.Append("<hr />");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='font-weight: bold;font-size:14px;' align='center' colspan='4'><u>Payroll for Month of " + drpMonth.SelectedItem.Text + " - " + drpYear.SelectedItem.Text + "</u></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' colspan='4'>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' colspan='4'>To</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' colspan='4'><b>" + txtTo.Text.Trim() + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' colspan='4'>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' colspan='4'>Subject : <b>" + txtSubject.Text.Trim() + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' colspan='4'>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' colspan='4'><b>Madam/Sir,</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' colspan='4'>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' colspan='4'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + txtDesc.Text + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' colspan='4'>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center' width='5%' style='border:solid 1px;border-right:none;'><b>Sl No</b></td>");
            stringBuilder.Append("<td align='center' width='45%' style='border:solid 1px;border-right:none;'><b>Employee Name</b></td>");
            stringBuilder.Append("<td align='center' width='35%' style='border:solid 1px;border-right:none;'><b>Bank Name</b></td>");
            stringBuilder.Append("<td align='center' width='10%' style='border:solid 1px;'><b>Net Amount</b></td>");
            stringBuilder.Append("</tr>");
            int num1 = 1;
            double num2 = 0.0;
            foreach (DataRow row in (InternalDataCollectionBase)table1.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='center' style='border-bottom:solid 1px;border-left:solid 1px;' valign='top'>" + num1 + "</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:solid 1px;border-left:solid 1px;' valign='top'>" + row["EmpName"] + "</td>");
                stringBuilder.Append("<td align='left' style='border-bottom:solid 1px;border-left:solid 1px;' valign='top'>" + row["BankName"] + "</td>");
                stringBuilder.Append("<td align='right' style='border-bottom:solid 1px;border-left:solid 1px;border-right:solid 1px;' valign='top'>" + Convert.ToDecimal(row["TransAmt"]).ToString("0.00") + "</td>");
                stringBuilder.Append("</tr>");
                ++num1;
                if (row["TransAmt"].ToString().Trim() != string.Empty)
                    num2 += Convert.ToDouble(row["TransAmt"]);
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='right' style='border-bottom:solid 1px;border-left:solid 1px;' colspan='3'><b>Total</b></td>");
            stringBuilder.Append("<td align='right' style='border-bottom:solid 1px;border-left:solid 1px;border-right:solid 1px;'><b>" + num2.ToString("0.00") + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center' style='border:solid 1px;border-top:none;' colspan='4'><b>Rupees " + NumberToText(num2.ToString("0.00")) + " Only</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' colspan='4'>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' colspan='4'>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='right' colspan='2'>&nbsp;</td>");
            stringBuilder.Append("<td align='right' style='font-size:14px'>Secretary</td>");
            stringBuilder.Append("<td align='right'>&nbsp;</td>");
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString();
            Session["rptBankPayroll"] = stringBuilder.ToString();
            btnExport.Enabled = true;
            btnPrint.Enabled = true;
        }
        else
        {
            btnExport.Enabled = false;
            btnPrint.Enabled = false;
        }
    }

    public string NumberToText(string strNum)
    {
        int num1 = (int)Math.Round(double.Parse(strNum));
        switch (num1)
        {
            case 0:
                return "Zero";
            case int.MinValue:
                return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";
            default:
                int[] numArray = new int[4];
                int num2 = 0;
                StringBuilder stringBuilder = new StringBuilder();
                if (num1 < 0)
                {
                    stringBuilder.Append("Minus ");
                    num1 = -num1;
                }
                string[] strArray1 = new string[10]
        {
          "",
          "One ",
          "Two ",
          "Three ",
          "Four ",
          "Five ",
          "Six ",
          "Seven ",
          "Eight ",
          "Nine "
        };
                string[] strArray2 = new string[10]
        {
          "Ten ",
          "Eleven ",
          "Twelve ",
          "Thirteen ",
          "Fourteen ",
          "Fifteen ",
          "Sixteen ",
          "Seventeen ",
          "Eighteen ",
          "Nineteen "
        };
                string[] strArray3 = new string[8]
        {
          "Twenty ",
          "Thirty ",
          "Forty ",
          "Fifty ",
          "Sixty ",
          "Seventy ",
          "Eighty ",
          "Ninety "
        };
                string[] strArray4 = new string[3]
        {
          "Thousand ",
          "Lakh ",
          "Crore "
        };
                numArray[0] = num1 % 1000;
                numArray[1] = num1 / 1000;
                numArray[2] = num1 / 100000;
                numArray[1] = numArray[1] - 100 * numArray[2];
                numArray[3] = num1 / 10000000;
                numArray[2] = numArray[2] - 100 * numArray[3];
                for (int index = 3; index > 0; --index)
                {
                    if (numArray[index] != 0)
                    {
                        num2 = index;
                        break;
                    }
                }
                for (int index1 = num2; index1 >= 0; --index1)
                {
                    if (numArray[index1] != 0)
                    {
                        int index2 = numArray[index1] % 10;
                        int num3 = numArray[index1] / 10;
                        int index3 = numArray[index1] / 100;
                        int num4 = num3 - 10 * index3;
                        if (index3 > 0)
                            stringBuilder.Append(strArray1[index3] + "Hundred ");
                        if (index2 > 0 || num4 > 0)
                        {
                            if (num4 == 0)
                                stringBuilder.Append(strArray1[index2]);
                            else if (num4 == 1)
                                stringBuilder.Append(strArray2[index2]);
                            else
                                stringBuilder.Append(strArray3[num4 - 2] + strArray1[index2]);
                        }
                        if (index1 != 0)
                            stringBuilder.Append(strArray4[index1 - 1]);
                    }
                }
                return stringBuilder.ToString().TrimEnd();
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["rptBankPayroll"] != null)
                ExportToExcel(Session["rptBankPayroll"].ToString());
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
            string str = Server.MapPath("Exported_Files/PayrollForBank_" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".xls");
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

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (Session["rptBankPayroll"] != null)
            Response.Redirect("rptBankPayrollPrint.aspx");
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnPrint, btnPrint.GetType(), "ShowMessage", "alert('No Data Exists to Print');", true);
    }
}