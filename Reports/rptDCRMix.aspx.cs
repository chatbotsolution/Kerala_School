using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
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

public partial class Reports_rptDCRMix : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Decimal Total;
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
        btnExpExcel.Enabled = false;
    }

    private void FillCollectCounter()
    {
        drpFeeCounter.DataSource = obj.GetDataTable("ps_sp_get_FeeWorkStation");
        drpFeeCounter.DataTextField = "FeeCollector";
        drpFeeCounter.DataValueField = "USER_ID";
        drpFeeCounter.DataBind();
        drpFeeCounter.Items.Insert(0, new ListItem("All", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (rbtnConsol.Checked)
            Response.Redirect("rptDCRL.aspx");
        else
            GenerateReport();
    }

    private void GenerateReport()
    {
        ViewState["TotFee"] = null;
        ViewState["TotAcHd"] = null;
        ViewState["TotAdj"] = null;
        Total = new Decimal(0);
        GetFeeHeadRpt();
        GetAcHeadsRpt();
        GetAdjustments();
        if (ViewState["TotFee"] != null)
            Total = Convert.ToDecimal(ViewState["TotFee"].ToString().Trim());
        if (ViewState["TotAcHd"] != null)
            Total = Total + Convert.ToDecimal(ViewState["TotAcHd"].ToString().Trim());
        if (ViewState["TotAdj"] != null)
            Total = Total + Convert.ToDecimal(ViewState["TotAdj"].ToString().Trim());
        if (Total != new Decimal(0))
        {
            lblTotal.Text = "<div class='tblheader' align='right'  float='right'><b>Grand Total :&nbsp;</b>" + Total + "&nbsp;(In Words : Rupees" + NumberToText(Total.ToString()) + ")";
            btnExpExcel.Enabled = true;
        }
        else
            lblTotal.Text = "";
        GetSaleReturn();
    }

    private void GetFeeHeadRpt()
    {
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpFeeCounter.SelectedIndex > 0)
            hashtable.Add("@UserID", drpFeeCounter.SelectedValue.ToString());
        if (txtDate.Text.Trim() != "")
            hashtable.Add("@Dt", dtpDate.GetDateValue().ToString("dd MMM yyyy"));
        else
            hashtable.Add("@Dt", Request.QueryString["Dt"].ToString());
        if (drpPmtMode.SelectedIndex > 0)
            hashtable.Add("@PaymentMode", drpPmtMode.SelectedValue.ToString());
        DataTable dataTable2 = obj.GetDataTable("Ps_Sp_GetDCRLMixFee", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            DataTable dataTable3 = new DataTable();
            DataTable dataTable4 = obj.GetDataTable("Ps_Sp_GetDCRMixFeeIdNames");
            foreach (DataRow row in (InternalDataCollectionBase)dataTable4.Rows)
                dataTable2.Columns.Add(new DataColumn(row["AcctsHeadId"].ToString(), Type.GetType("System.Double")));
            dataTable2.AcceptChanges();
            lblFee.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='left'><b>&nbsp;Fee Heads</b></div>" + CreateReport(mergertable(dataTable2, 1), dataTable4, 1).ToString().Trim();
            btnExpExcel.Enabled = true;
        }
        else
        {
            lblFee.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='center'>No Records Found  !</div>";
            btnExpExcel.Enabled = false;
        }
    }

    private void GetAcHeadsRpt()
    {
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpFeeCounter.SelectedIndex > 0)
            hashtable.Add("@UserID", drpFeeCounter.SelectedValue.ToString());
        if (txtDate.Text.Trim() != "")
            hashtable.Add("@Dt", dtpDate.GetDateValue().ToString("dd MMM yyyy"));
        else
            hashtable.Add("@Dt", Request.QueryString["Dt"].ToString());
        if (drpPmtMode.SelectedIndex > 0)
            hashtable.Add("@PaymentMode", drpPmtMode.SelectedValue.ToString());
        DataTable dataTable2 = obj.GetDataTable("Ps_Sp_GetDCRLMixAcHd", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            DataTable dataTable3 = new DataTable();
            DataTable dataTable4 = obj.GetDataTable("Ps_Sp_GetDCRMixAhIdNames");
            foreach (DataRow row in (InternalDataCollectionBase)dataTable4.Rows)
                dataTable2.Columns.Add(new DataColumn(row["AcctsHeadId"].ToString(), Type.GetType("System.Double")));
            dataTable2.AcceptChanges();
            lblAccHd.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='left'><b>&nbsp;Miscellaneous Heads</b></div>" + CreateReport(mergertable(dataTable2, 2), dataTable4, 2).ToString().Trim();
            btnExpExcel.Enabled = true;
        }
        else
        {
            lblAccHd.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='center'>No Records Found  !</div>";
            btnExpExcel.Enabled = false;
        }
    }

    private void GetAdjustments()
    {
        DataSet dataSet = new DataSet();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpFeeCounter.SelectedIndex > 0)
            hashtable.Add("@UserID", drpFeeCounter.SelectedValue.ToString());
        if (txtDate.Text.Trim() != "")
            hashtable.Add("@Dt", dtpDate.GetDateValue().ToString("dd MMM yyyy"));
        else
            hashtable.Add("@Dt", Request.QueryString["Dt"].ToString());
        if (drpPmtMode.SelectedIndex > 0)
            hashtable.Add("@PaymentMode", drpPmtMode.SelectedValue.ToString());
        DataTable dataTable2 = obj.GetDataTable("Ps_Sp_GetDCRLMixAdjAcHd", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            DataTable dataTable3 = new DataTable();
            DataTable table = obj.GetDataSet("Ps_Sp_GetDCRMixAdjustment").Tables[1];
            foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
                dataTable2.Columns.Add(new DataColumn(row["AcctsHeadId"].ToString(), Type.GetType("System.Double")));
            dataTable2.AcceptChanges();
            lblAdj.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='left'><b>&nbsp;Adjustment Heads</b></div>" + CreateReport(mergertable(dataTable2, 3), table, 3).ToString().Trim();
            btnExpExcel.Enabled = true;
        }
        else
        {
            lblAdj.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='center'>No Records Found  !</div>";
            btnExpExcel.Enabled = false;
        }
    }

    private void GetSaleReturn()
    {
        double num1 = 0.0;
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpFeeCounter.SelectedIndex > 0)
            hashtable.Add("@UserID", drpFeeCounter.SelectedValue.ToString());
        if (txtDate.Text.Trim() != "")
            hashtable.Add("@Dt", dtpDate.GetDateValue().ToString("dd MMM yyyy"));
        else
            hashtable.Add("@Dt", Request.QueryString["Dt"].ToString());
        if (drpPmtMode.SelectedIndex > 0)
            hashtable.Add("@PaymentMode", drpPmtMode.SelectedValue.ToString());
        DataTable dataTable2 = obj.GetDataTable("Ps_Sp_GetSaleReturn", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
            stringBuilder.Append("<tr  style='background-color:#CCCCCC'>");
            stringBuilder.Append("<td style='width: 30px' align='left'><strong>Sl.No</strong></td>");
            stringBuilder.Append("<td style='width: 120px' align='left'>Receipt No</td>");
            stringBuilder.Append("<td style='width: 150px' align='left'>Name</td>");
            stringBuilder.Append("<td align='left'>Description</td>");
            stringBuilder.Append("<td style='width: 100px' align='right'>Amount</td>");
            stringBuilder.Append("<td style='width: 100px' align='center'>Returned By</td>");
            stringBuilder.Append("</tr>");
            int num2 = 1;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='center'>" + num2.ToString() + "</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["InvoiceReceiptNo"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["RcvdFrom"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["Description"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right'>");
                stringBuilder.Append(row["Amount"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='center'>");
                stringBuilder.Append(row["FullName"].ToString());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                num1 += Convert.ToDouble(row["Amount"].ToString().Trim());
                ++num2;
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='4'  align='right'>");
            stringBuilder.Append("<strong >Total Sale Ret &nbsp;:&nbsp;</strong>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right'>");
            stringBuilder.Append("<strong >" + string.Format("{0:F2}", num1));
            stringBuilder.Append("</strong></td>");
            stringBuilder.Append("<td></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table border='0px' cellpadding='1' cellspacing='0' width='100%'>");
            stringBuilder.Append("<tr align='right'>");
            stringBuilder.Append("<td colspan='5' style='border:0px' align='right'></br>");
            stringBuilder.Append("<strong >Net Amount Collected &nbsp;:&nbsp;</strong>");
            double num3 = Convert.ToDouble(Total) - num1;
            stringBuilder.Append("<strong >" + string.Format("{0:F2}", num3) + "</strong> &nbsp;(<strong>In Words :</strong> Rupees&nbsp;" + NumberToText(num3.ToString()) + " Only)");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            lblSaleRet.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='left'><b>&nbsp;Sale Return</b></div>" + stringBuilder.ToString().Trim();
            btnExpExcel.Enabled = true;
        }
        else
            lblSaleRet.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='center'>No Records Found  !</div>";
    }

    private DataTable mergertable(DataTable tbstudent, int no)
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
                DataTable dataTable = new DataTable();
                Hashtable hashtable2 = new Hashtable();
                hashtable2.Add("@AdmnNo", str1);
                hashtable2.Add("@TransDate", dateTime.ToString("dd MMM yyyy"));
                hashtable2.Add("@Receipt_VrNo", str2);
                foreach (DataRow row in (InternalDataCollectionBase)(no != 1 ? (no != 2 ? obj.GetDataSet("Ps_Sp_GetDCRMixAdjustment", hashtable2).Tables[0] : obj.GetDataTable("Ps_Sp_GetDCRMixAcHdNms", hashtable2)) : obj.GetDataTable("Ps_Sp_GetDCRMixFeeNms", hashtable2)).Rows)
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

    protected StringBuilder CreateReport(DataTable dt, DataTable tbcomp, int type)
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
            stringBuilder1.Append("<td colspan='7' align='right'><strong>Total:");
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
            string str1 = num3.ToString();
            stringBuilder1.Append("</strong></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("</table>");
            if (type == 1)
                ViewState["TotFee"] = str1;
            else if (type == 2)
                ViewState["TotAcHd"] = str1;
            else
                ViewState["TotAdj"] = str1;
        }
        return stringBuilder1;
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        if (Session["userrights"].ToString() == "a")
        {
            try
            {
                if (lblFee.Text.ToString().Trim() != "" || lblAccHd.Text.ToString().Trim() != "")
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
                    stringBuilder.Append(lblFee.Text.ToString().Trim());
                    stringBuilder.Append("<table><tr><td style='height: 10px'></td></tr>");
                    stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                    stringBuilder.Append(lblAccHd.Text.ToString().Trim());
                    stringBuilder.Append("<table><tr><td style='height: 10px'></td></tr>");
                    stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                    stringBuilder.Append(lblAdj.Text.ToString().Trim());
                    stringBuilder.Append("<table><tr><td style='height: 10px'></td></tr>");
                    stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                    stringBuilder.Append(lblTotal.Text.ToString().Trim());
                    stringBuilder.Append("<table><tr><td style='height: 10px'></td></tr>");
                    stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                    stringBuilder.Append(lblSaleRet.Text.ToString().Trim());
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

    protected void drpFeeCounter_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblFee.Text = "";
        lblAccHd.Text = "";
    }

    protected void btnCancelled_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptCancelledRcpt.aspx?Dt=" + dtpDate.GetDateValue().ToString("dd MMM yyyy"));
    }

    protected void rbtnConsol_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnConsol.Checked)
            Response.Redirect("rptDCRL.aspx?Dt=" + dtpDate.GetDateValue().ToString("dd MMM yyyy"));
        lblAccHd.Text = "";
        lblFee.Text = "";
        lblTotal.Text = "";
    }
}
