using AjaxControlToolkit;
using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptDCRL : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        if (Request.QueryString["Dt"] != null)
            dtpDate.SetDateValue(Convert.ToDateTime(Request.QueryString["Dt"].Trim()));
        else
            dtpDate.SetDateValue(DateTime.Today);
        FillCollectCounter();
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
        rbtnConsol.Checked = true;
    }

    private void FillCollectCounter()
    {
        drpFeeCounter.DataSource = new Common().GetDataTable("ps_sp_get_FeeWorkStation");
        drpFeeCounter.DataTextField = "FeeCollector";
        drpFeeCounter.DataValueField = "USER_ID";
        drpFeeCounter.DataBind();
        drpFeeCounter.Items.Insert(0, new ListItem("All", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Common common = new Common();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        if (drpFeeCounter.SelectedIndex > 0)
            hashtable.Add("@UserID", drpFeeCounter.SelectedValue.ToString());
        if (txtDate.Text.Trim() != "")
            hashtable.Add("@Dt", dtpDate.GetDateValue().ToString("dd MMM yyyy"));
        if (drpPmtMode.SelectedIndex > 0)
            hashtable.Add("@PaymentMode", drpPmtMode.SelectedValue.ToString());
        if (rbtnFeeHead.Checked)
        {
            Response.Redirect("rptDCRMix.aspx");
        }
        else
        {
            DataSet dataSet2 = common.GetDataSet("ps_sp_get_ALLFeeReceived", hashtable);
            if (dataSet2.Tables[0].Rows.Count > 0 || dataSet2.Tables[1].Rows.Count > 0 || dataSet2.Tables[2].Rows.Count > 0)
            {
                GenerateReport(dataSet2);
                btnPrint.Enabled = true;
                btnExpExcel.Enabled = true;
            }
            else
            {
                lblReport.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='center'>No Records Found  !</div>";
                btnPrint.Enabled = false;
                btnExpExcel.Enabled = false;
            }
        }
    }

    private void GenerateReport(DataSet ds)
    {
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        DataTable dataTable1 = new DataTable();
        DataTable table1 = ds.Tables[0];
        DataTable dataTable2 = new DataTable();
        DataTable table2 = ds.Tables[1];
        DataTable dataTable3 = new DataTable();
        DataTable table3 = ds.Tables[2];
        StringBuilder stringBuilder1 = new StringBuilder("");
        if (table1.Rows.Count > 0)
        {
            stringBuilder1.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
            stringBuilder1.Append("<tr  style='background-color:#eeeeee'>");
            stringBuilder1.Append("<td style='width: 30px' align='left'><strong>Sl.No</strong></td>");
            stringBuilder1.Append("<td style='width: 120px' align='left'>Receipt No</td>");
            stringBuilder1.Append("<td style='width: 80px' align='left'>Admission No(If Any)</td>");
            stringBuilder1.Append("<td style='width: 150px' align='left'>Name</td>");
            stringBuilder1.Append("<td style='width: 60px' align='left'>Class(If Any)</td>");
            stringBuilder1.Append("<td align='left'>Description</td>");
            stringBuilder1.Append("<td style='width: 100px' align='right'>Amount</td>");
            stringBuilder1.Append("<td style='width: 100px' align='center'>Received By</td>");
            stringBuilder1.Append("</tr>");
            int num4 = 1;
            foreach (DataRow row in (InternalDataCollectionBase)table1.Rows)
            {
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td align='center'>" + num4.ToString() + "</td>");
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["InvoiceReceiptNo"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='left'>");
                if (row["PartyId"].ToString().Trim() == "0")
                    stringBuilder1.Append("");
                else
                    stringBuilder1.Append(row["PartyId"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["RcvdFrom"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["ClassName"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["Description"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='right'>");
                stringBuilder1.Append(row["Amount"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(row["FullName"].ToString());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("</tr>");
                num1 += Convert.ToDouble(row["Amount"].ToString().Trim());
                ++num4;
            }
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='6'  align='right'>");
            stringBuilder1.Append("<strong >Total &nbsp;:&nbsp;</strong>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td align='right'>");
            stringBuilder1.Append("<strong >" + string.Format("{0:F2}", num1));
            stringBuilder1.Append("</strong></td>");
            stringBuilder1.Append("<td></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("</table>");
        }
        if (table2.Rows.Count > 0)
        {
            stringBuilder1.Append("<br/><b>Adjustments</b><br/><br/>");
            stringBuilder1.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
            stringBuilder1.Append("<tr  style='background-color:#CCCCCC'>");
            stringBuilder1.Append("<td style='width: 30px' align='left'><strong>Sl.No</strong></td>");
            stringBuilder1.Append("<td style='width: 120px' align='left'>Receipt No</td>");
            stringBuilder1.Append("<td style='width: 150px' align='left'>Name</td>");
            stringBuilder1.Append("<td align='left'>Description</td>");
            stringBuilder1.Append("<td style='width: 100px' align='right'>Amount</td>");
            stringBuilder1.Append("<td style='width: 100px' align='center'>Received By</td>");
            stringBuilder1.Append("</tr>");
            int num4 = 1;
            foreach (DataRow row in (InternalDataCollectionBase)table2.Rows)
            {
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td align='center'>" + num4.ToString() + "</td>");
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["InvoiceReceiptNo"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["RcvdFrom"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["Description"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='right'>");
                stringBuilder1.Append(row["Amount"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(row["FullName"].ToString());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("</tr>");
                num2 += Convert.ToDouble(row["Amount"].ToString().Trim());
                ++num4;
            }
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='4'  align='right'>");
            stringBuilder1.Append("<strong >Total &nbsp;:&nbsp;</strong>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td align='right'>");
            stringBuilder1.Append("<strong >" + string.Format("{0:F2}", num2));
            stringBuilder1.Append("</strong></td>");
            stringBuilder1.Append("<td></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("</table>");
        }
        stringBuilder1.Append("<br/><div style='float:right;'><strong >Grand Total &nbsp;:&nbsp; </strong >" + (num1 + num2).ToString() + "</strong>&nbsp;(<strong>In Words :</strong> Rupees&nbsp;" + NumberToText((num1 + num2).ToString()) + " Only)</div>");
        if (table3.Rows.Count > 0)
        {
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder1.Append("<br/><b>Sale Return</b><br/><br/>");
            stringBuilder1.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
            stringBuilder1.Append("<tr  style='background-color:#CCCCCC'>");
            stringBuilder1.Append("<td style='width: 30px' align='left'><strong>Sl.No</strong></td>");
            stringBuilder1.Append("<td style='width: 120px' align='left'>Receipt No</td>");
            stringBuilder1.Append("<td style='width: 150px' align='left'>Name</td>");
            stringBuilder1.Append("<td align='left'>Description</td>");
            stringBuilder1.Append("<td style='width: 100px' align='right'>Amount</td>");
            stringBuilder1.Append("<td style='width: 100px' align='center'>Returned By</td>");
            stringBuilder1.Append("</tr>");
            int num4 = 1;
            foreach (DataRow row in (InternalDataCollectionBase)table3.Rows)
            {
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td align='center'>" + num4.ToString() + "</td>");
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["InvoiceReceiptNo"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["RcvdFrom"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["Description"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='right'>");
                stringBuilder1.Append(row["Amount"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(row["FullName"].ToString());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("</tr>");
                num3 += Convert.ToDouble(row["Amount"].ToString().Trim());
                ++num4;
            }
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='4'  align='right'>");
            stringBuilder1.Append("<strong >Total Sale Return Amount &nbsp;:&nbsp;</strong>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td align='right'>");
            stringBuilder1.Append("<strong >" + string.Format("{0:F2}", num3));
            stringBuilder1.Append("</strong></td>");
            stringBuilder1.Append("<td></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("</table>");
            stringBuilder1.Append("<table border='0px' cellpadding='1' cellspacing='0' width='100%'>");
            stringBuilder1.Append("<tr align='right'>");
            stringBuilder1.Append("<td colspan='5' style='border:0px' align='right'></br>");
            stringBuilder1.Append("<strong >Net Amount Collected &nbsp;:&nbsp;</strong>");
            double num5 = num1 + num2 - num3;
            stringBuilder1.Append("<strong >" + string.Format("{0:F2}", num5) + "</strong> &nbsp;(<strong>In Words :</strong> Rupees&nbsp;" + NumberToText(num5.ToString()) + " Only)");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("</table>");
            btnExpExcel.Enabled = true;
        }
        lblReport.Text = stringBuilder1.ToString().Trim();
        Session["DCRL"] = stringBuilder1.ToString().Trim();
        Session["DCRDT"] = dtpDate.GetDateValue().ToString("dd MMM yyyy");
    }

    private DataTable mergertable(DataTable tbstudent)
    {
        Hashtable hashtable1 = new Hashtable();
        for (int index = 0; index < tbstudent.Rows.Count; ++index)
        {
            string str1 = tbstudent.Rows[index]["admnno"].ToString();
            DateTime dateTime = Convert.ToDateTime(tbstudent.Rows[index]["tdate"]);
            string str2 = tbstudent.Rows[index]["PmtRecptVoucherNo"].ToString();
            if (hashtable1[(str1 + dateTime + str2)] == null)
            {
                hashtable1.Add((str1 + dateTime + str2), (str1 + dateTime + str2));
                Common common = new Common();
                DataTable dataTable = new DataTable();
                Hashtable hashtable2 = new Hashtable();
                hashtable2.Add("@AdmnNo", str1);
                hashtable2.Add("@TransDate", dateTime.ToString("dd MMM yyyy"));
                hashtable2.Add("@Receipt_VrNo", str2);
                foreach (DataRow row in (InternalDataCollectionBase)(!(Convert.ToDateTime(dtpDate.GetDateValue().ToString("dd MMM yyyy")) >= Convert.ToDateTime("01 Apr 2014")) ? common.GetDataTable("Ps_Sp_GetFeeNames", hashtable2) : common.GetDataTable("Ps_Sp_GetFeeNamesNew", hashtable2)).Rows)
                {
                    string str3 = row["TransAmt"].ToString();
                    string str4 = tbstudent.Rows[index][row["AccountHead"].ToString()].ToString();
                    Decimal num1 = new Decimal(0);
                    Decimal num2 = !(str4 != "") ? Convert.ToDecimal(str3) : Convert.ToDecimal(str4) + Convert.ToDecimal(str3);
                    tbstudent.Rows[index][row["AccountHead"].ToString()] = num2.ToString();
                }
            }
        }
        return tbstudent;
    }

    protected void CreateReport(DataTable dt, DataTable tbcomp)
    {
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        Hashtable hashtable1 = new Hashtable();
        Hashtable hashtable2 = new Hashtable();
        int num1 = 0;
        int num2 = 6;
        if (dt.Rows.Count > 0)
        {
            stringBuilder1.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
            stringBuilder1.Append("<tr style='background-color:#CCCCCC'>");
            stringBuilder1.Append("<td style='width: 30px' align='left'><strong>Sl.No</strong></td>");
            stringBuilder1.Append("<td style='width:100px;'>");
            stringBuilder1.Append("<strong>Fee Date</strong>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td style='width:100px;'>");
            stringBuilder1.Append("<strong>Receipt No</strong>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td style='width:100px;'>");
            stringBuilder1.Append("<strong>Admn No</strong>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td>");
            stringBuilder1.Append("<strong>Name</strong>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td style='width:150px;'>");
            stringBuilder1.Append("<strong>Class</strong>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td style='width:250px;'>");
            stringBuilder1.Append("<strong>Description</strong>");
            stringBuilder1.Append("</td>");
            double num3 = 0.0;
            int num4 = 1;
            int index1 = 11;
            foreach (DataRow row in (InternalDataCollectionBase)tbcomp.Rows)
            {
                double num5 = Convert.ToDouble(dt.Compute("Sum([" + dt.Columns[index1].ColumnName + "])", "").ToString() == "" ? "0" : dt.Compute("Sum([" + dt.Columns[index1].ColumnName + "])", "").ToString());
                if (row["AcctsHeadId"].ToString() == dt.Columns[index1].ColumnName && row["AcctsHeadId"].ToString() != tbcomp.Rows[tbcomp.Rows.Count - 1]["AcctsHeadId"].ToString().Trim())
                {
                    if (num5 > 0.0)
                    {
                        stringBuilder1.Append("<td style='width:50px;' align='left'>");
                        stringBuilder1.Append("<strong>" + row["AcctsHead"].ToString() + "</strong>");
                        stringBuilder1.Append("</td>");
                    }
                }
                else
                {
                    stringBuilder1.Append("<td style='width:50px;'  align='right'>");
                    stringBuilder1.Append("<strong>" + row["AcctsHead"].ToString() + "</strong>");
                    stringBuilder1.Append("</td>");
                }
                ++index1;
                if (num5 > 0.0)
                    ++num2;
            }
            stringBuilder1.Append("</tr>");
            foreach (DataRow row1 in (InternalDataCollectionBase)dt.Rows)
            {
                bool flag;
                if (hashtable1[(row1["TransDate"].ToString() + row1["AdmnNo"].ToString() + row1["PmtRecptVoucherNo"].ToString())] == null)
                {
                    hashtable1.Add((row1["TransDate"].ToString() + row1["AdmnNo"].ToString() + row1["PmtRecptVoucherNo"].ToString()), (row1["TransDate"].ToString() + row1["AdmnNo"].ToString() + row1["PmtRecptVoucherNo"].ToString()));
                    flag = true;
                }
                else
                    flag = false;
                if (flag)
                {
                    stringBuilder1.Append("<tr>");
                    stringBuilder1.Append("<td align='center'>" + num4.ToString() + "</td>");
                    stringBuilder1.Append("<td  valign='top' align='left'>");
                    stringBuilder1.Append(row1["TransDate"].ToString().Trim());
                    stringBuilder1.Append("</td>");
                    stringBuilder1.Append("<td  valign='top' align='left'>");
                    stringBuilder1.Append(row1["PmtRecptVoucherNo"].ToString().Trim());
                    stringBuilder1.Append("</td>");
                    if (row1["AdmnNo"].ToString().Trim() != "")
                    {
                        if (Convert.ToInt64(row1["AdmnNo"].ToString().Trim()) < 1000000000L || row1["AdmnNo"].ToString().Trim() == "0")
                        {
                            stringBuilder1.Append("<td  valign='top' align='left'>&nbsp;");
                            stringBuilder1.Append("</td>");
                        }
                        else
                        {
                            stringBuilder1.Append("<td  valign='top' align='left'>");
                            stringBuilder1.Append(row1["AdmnNo"].ToString().Trim());
                            stringBuilder1.Append("</td>");
                        }
                    }
                    else
                    {
                        stringBuilder1.Append("<td  valign='top' align='left'>&nbsp;");
                        stringBuilder1.Append("</td>");
                    }
                    stringBuilder1.Append("<td  valign='top' align='left' style='white-space:nowrap;'>");
                    stringBuilder1.Append(row1["FullName"].ToString().Trim());
                    stringBuilder1.Append("</td>");
                    stringBuilder1.Append("<td  valign='top' align='left' style='white-space:nowrap;'>");
                    stringBuilder1.Append(row1["ClassName"].ToString().Trim());
                    stringBuilder1.Append("</td>");
                    stringBuilder1.Append("<td  valign='top' align='left' style='white-space:nowrap;'>");
                    stringBuilder1.Append(row1["Description"].ToString().Trim());
                    stringBuilder1.Append("</td>");
                    double num5 = 0.0;
                    int num6 = tbcomp.Rows.Count - 1;
                    int num7 = 0;
                    int index2 = 11;
                    foreach (DataRow row2 in (InternalDataCollectionBase)tbcomp.Rows)
                    {
                        double num8 = Convert.ToDouble(dt.Compute("Sum([" + dt.Columns[index2].ColumnName + "])", "").ToString() == "" ? "0" : dt.Compute("Sum([" + dt.Columns[index2].ColumnName + "])", "").ToString());
                        if (num7++ < num6)
                        {
                            string str = row1[row2["AcctsHeadId"].ToString()].ToString().Trim();
                            if (row2["AcctsHeadId"].ToString() == dt.Columns[index2].ColumnName.ToString())
                            {
                                if (num8 > 0.0)
                                {
                                    stringBuilder1.Append("<td align='left'>");
                                    stringBuilder1.Append(row1[row2["AcctsHeadId"].ToString()].ToString().Trim());
                                    stringBuilder1.Append("</td>");
                                }
                            }
                            else
                            {
                                stringBuilder1.Append("<td align='left'>");
                                stringBuilder1.Append(row1[row2["AcctsHeadId"].ToString()].ToString().Trim());
                                stringBuilder1.Append("</td>");
                            }
                            if (str != "")
                                num5 += Convert.ToDouble(str);
                            ++index2;
                        }
                    }
                    stringBuilder1.Append("<td align='right'>");
                    stringBuilder1.Append(num5.ToString());
                    stringBuilder1.Append("</td>");
                    ++num1;
                    ++num4;
                    stringBuilder1.Append("</tr>");
                    num3 += Convert.ToDouble(num5);
                }
            }
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='7' align='right'><strong>Grand Total:");
            stringBuilder1.Append("</strong></td>");
            int index3 = 11;
            foreach (DataRow row in (InternalDataCollectionBase)tbcomp.Rows)
            {
                double num5 = Convert.ToDouble(dt.Compute("Sum([" + dt.Columns[index3].ColumnName + "])", "").ToString() == "" ? "0" : dt.Compute("Sum([" + dt.Columns[index3].ColumnName + "])", "").ToString());
                if (row["AcctsHeadId"].ToString() == dt.Columns[index3].ColumnName && row["AcctsHead"].ToString() != "Total")
                {
                    if (num5 > 0.0)
                    {
                        stringBuilder1.Append("<td style='width:50px;' align='left'>");
                        stringBuilder1.Append("<strong>" + num5 + "</strong>");
                        stringBuilder1.Append("</td>");
                    }
                    ++index3;
                }
            }
            stringBuilder1.Append("<td align='right'><strong>");
            stringBuilder1.Append(num3.ToString());
            string strNum = num3.ToString();
            stringBuilder1.Append("</strong></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("</table>");
            stringBuilder1.Append("<table border='0px' cellpadding='5' cellspacing='5' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
            stringBuilder1.Append("<tr><td colspan='" + (Convert.ToInt32(num2) + 2) + "' align='right' style='padding:5px 0px 0px 0px; border:solid 0px white;'></strong>&nbsp;(<strong>In Words :</strong> Rupees&nbsp;" + NumberToText(strNum) + " Only)</td></tr>");
            stringBuilder1.Append("</table>");
            if (num2 < 10)
                btnPrint.Enabled = true;
            else
                btnPrint.Enabled = false;
        }
        lblReport.Text = stringBuilder1.ToString().Trim();
        Session["DCRL"] = stringBuilder1.ToString().Trim();
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        if (Session["userrights"].ToString() == "a")
        {
            try
            {
                if (lblReport.Text.ToString().Trim() != "")
                {
                    StringBuilder stringBuilder = new StringBuilder("");
                    stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                    stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                    stringBuilder.Append("<tr><td align='left' colspan='14'><b>");
                    stringBuilder.Append("Daily Collection Report :- ");
                    if (txtDate.Text.Trim() != "")
                        stringBuilder.Append(dtpDate.GetDateValue().ToString("d MMM yyyy"));
                    stringBuilder.Append("</b></td></tr>");
                    stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                    stringBuilder.Append(lblReport.Text.ToString().Trim());
                    stringBuilder.Append("<table><tr><td style='height: 10px'></td></tr>");
                    stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                    stringBuilder.Append("</table>");
                    ExportToExcel(stringBuilder.ToString());
                }
                else
                    Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
        else
            Response.Write("<script language='javascript'>alert('ONLY ADMIN CAN EXPORT');</script>");
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/DCRL" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptDCRLPrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpFeeCounter_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = "";
    }

    protected void rbtnConsol_CheckedChanged(object sender, EventArgs e)
    {
        lblReport.Text = "";
        if (!rbtnFeeHead.Checked)
            return;
        Response.Redirect("rptDCRMix.aspx?Dt=" + dtpDate.GetDateValue().ToString("dd MMM yyyy"));
    }

    protected void btnCancelled_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptCancelledRcpt.aspx?Dt=" + dtpDate.GetDateValue().ToString("dd MMM yyyy"));
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
}
