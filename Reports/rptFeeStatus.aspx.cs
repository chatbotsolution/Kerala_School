using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptFeeStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        fillclass();
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Common common = new Common();
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        string currSession = new clsGenerateFee().CreateCurrSession();
        hashtable.Add("@SessionYr", currSession);
        if (drpclass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpclass.SelectedValue.ToString());
        hashtable.Add("@SchoolID", Session["SchoolId"].ToString());
        hashtable.Add("@CurrSessStDt", ("04/01/" + currSession.Substring(0, 4)));
        hashtable.Add("@CurrSessEndDt", ("03/31/" + currSession.Substring(0, 2) + currSession.Substring(5, 2)));
        string[] strArray = currSession.Split('-');
        DataTable dt = !rbtnTillDt.Checked ? (Convert.ToInt32(currSession.Substring(0, 4)) < 2014 ? common.GetDataTable("Ps_Sp_GetFullFeeStatus", hashtable) : common.GetDataTable("Ps_Sp_GetFullFeeStatusNew", hashtable)) : (Convert.ToInt32(strArray[0]) < 2014 ? common.GetDataTable("Ps_Sp_GetFeeStatus", hashtable) : common.GetDataTable("Ps_Sp_GetFeeStatusNew", hashtable));
        if (dt.Rows.Count > 0)
        {
            GenerateReport(dt);
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

    private void GenerateReport(DataTable dt)
    {
        StringBuilder stringBuilder1 = new StringBuilder("");
        stringBuilder1.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
        stringBuilder1.Append("<tr style='background-color:#CCCCCC'>");
        stringBuilder1.Append("<td style='width: 40px' align='right'><strong>Sl. No.</strong></td>");
        stringBuilder1.Append("<td style='width: 100px' align='right'><strong>Admission No</strong></td>");
        stringBuilder1.Append("<td align='left'><strong>Student Name</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Previous Year Due</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>School Fee</strong></td>");
        //stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Bus Fee</strong></td>");
        //stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Books Fee</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Total Amount</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Total Amount Paid</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Balance</strong></td>");
        stringBuilder1.Append("</tr>");
        int num1 = 1;
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        double num5 = 0.0;
        double num6 = 0.0;
        double num7 = 0.0;
        double num8 = 0.0;
        StringBuilder stringBuilder2 = new StringBuilder("");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='right'>" + num1.ToString() + "</td>");
            stringBuilder2.Append("<td align='right'>");
            stringBuilder2.Append(row["OldAdmnno"].ToString().Trim());
            stringBuilder2.Append("</td>");
            stringBuilder2.Append("<td align='left'>");
            stringBuilder2.Append(row["FullName"].ToString().Trim());
            stringBuilder2.Append("</td>");
            stringBuilder2.Append("<td align='right'>");
            string str1 = "<a href='javascript:popUp";
            string str2 = "('rptPrevYrBal.aspx?Admnno=" + row["Admnno"].ToString() + "')";
            string str3 = "'>";
            stringBuilder2.Append(str1.Replace('\'', '"') + str2 + str3.Replace('\'', '"'));
            stringBuilder2.Append("<b>" + row["PrevBal"].ToString() + "</b></a>");
            stringBuilder2.Append("</td>");
            if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                num2 += 0.0;
            else
                num2 += Convert.ToDouble(row["PrevBal"].ToString());
            if (rbtnTillDt.Checked)
            {
                stringBuilder2.Append("<td align='right'>");
                string str4 = "<a href='javascript:popUp";
                string str5 = "('rptSchoolFeeList.aspx?Id=" + row["Admnno"].ToString() + "')";
                string str6 = "'>";
                stringBuilder2.Append(str4.Replace('\'', '"') + str5 + str6.Replace('\'', '"'));
                stringBuilder2.Append("<b>" + row["SchoolFee"].ToString() + "</b></a>");
                stringBuilder2.Append("</td>");
                if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                    num3 += 0.0;
                else
                    num3 += Convert.ToDouble(row["SchoolFee"].ToString());
               // double num9 = 0.0;
                //if (row["BusFee"].ToString() != "")
                //    num9 = Convert.ToDouble(row["BusFee"].ToString());
                //if (num9 <= 0.0)
                //{
                //    stringBuilder2.Append("<td align='right'>");
                //    stringBuilder2.Append("0.00");
                //    stringBuilder2.Append("</td>");
                //}
                //else
                //{
                //    stringBuilder2.Append("<td align='right'>");
                //    string str7 = "<a href='javascript:popUp";
                //    string str8 = "('rptMiscFeeList.aspx?Id=" + row["Admnno"].ToString() + " &AdId=1')";
                //    string str9 = "'>";
                //    stringBuilder2.Append(str7.Replace('\'', '"') + str8 + str9.Replace('\'', '"'));
                //    stringBuilder2.Append("<b>" + row["BusFee"].ToString() + "</b></a>");
                //    stringBuilder2.Append("</td>");
                //    if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                //        num4 += 0.0;
                //    else
                //        num4 += Convert.ToDouble(row["BusFee"].ToString());
                //}
                //double num10 = 0.0;
                //if (row["BooksFee"].ToString() != "")
                //    num10 = Convert.ToDouble(row["BooksFee"].ToString());
                //if (num10 <= 0.0)
                //{
                //    stringBuilder2.Append("<td align='right'>");
                //    stringBuilder2.Append("0.00");
                //    stringBuilder2.Append("</td>");
                //}
                //else
                //{
                //    stringBuilder2.Append("<td align='right'>");
                //    string str7 = "<a href='javascript:popUp";
                //    string str8 = "('rptBookFeeList.aspx?Id=" + row["Admnno"].ToString() + " &AdId=2')";
                //    string str9 = "'>";
                //    stringBuilder2.Append(str7.Replace('\'', '"') + str8 + str9.Replace('\'', '"'));
                //    stringBuilder2.Append("<b>" + row["BooksFee"].ToString() + "</b></a>");
                //    stringBuilder2.Append("</td>");
                //    if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                //        num5 += 0.0;
                //    else
                //        num5 += Convert.ToDouble(row["BooksFee"].ToString());
                //}
                stringBuilder2.Append("<td align='right'>");
                string str10 = "<a href='javascript:popUp";
                string str11 = "('rptAmountDueList.aspx?Id=" + row["Admnno"].ToString() + "')";
                string str12 = "'>";
                stringBuilder2.Append(str10.Replace('\'', '"') + str11 + str12.Replace('\'', '"'));
                stringBuilder2.Append("<b>" + row["TotalFee"].ToString() + "</b></a>");
                stringBuilder2.Append("</td>");
                if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                    num6 += 0.0;
                else
                    num6 += Convert.ToDouble(row["TotalFee"].ToString());
                if (Convert.ToDouble(row["PaidAmount"].ToString()) <= 0.0)
                {
                    stringBuilder2.Append("<td align='right'>");
                    stringBuilder2.Append("0.00");
                    stringBuilder2.Append("</td>");
                }
                else
                {
                    stringBuilder2.Append("<td align='right'>");
                    string str7 = "<a href='javascript:popUp";
                    string str8 = "('rptAmountPaidList.aspx?Id=" + row["Admnno"].ToString() + "')";
                    string str9 = "'>";
                    stringBuilder2.Append(str7.Replace('\'', '"') + str8 + str9.Replace('\'', '"'));
                    stringBuilder2.Append("<b>" + row["PaidAmount"].ToString() + "</b></a>");
                    stringBuilder2.Append("</td>");
                    if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                        num7 += 0.0;
                    else
                        num7 += Convert.ToDouble(row["PaidAmount"].ToString());
                }
                stringBuilder2.Append("<td align='right'>");
                string str13 = "<a href='javascript:popUp";
                string str14 = "('rptFeeBalance.aspx?admnno=" + row["Admnno"].ToString() + " &AD=" + row["TotalFee"].ToString() + " &AP=" + row["PaidAmount"].ToString() + "')";
                string str15 = "'>";
                stringBuilder2.Append(str13.Replace('\'', '"') + str14 + str15.Replace('\'', '"'));
                stringBuilder2.Append("<b>" + row["Balance"].ToString() + "</b></a>");
                stringBuilder2.Append("</td>");
                if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                    num8 += 0.0;
                else
                    num8 += Convert.ToDouble(row["Balance"].ToString());
            }
            else
            {
                stringBuilder2.Append("<td align='right'>");
                string str4 = "<a href='javascript:popUp";
                string str5 = "('rptSchoolFeeList.aspx?Id=" + row["Admnno"].ToString() + "&&FP=F')";
                string str6 = "'>";
                stringBuilder2.Append(str4.Replace('\'', '"') + str5 + str6.Replace('\'', '"'));
                stringBuilder2.Append("<b>" + row["SchoolFee"].ToString() + "</b></a>");
                stringBuilder2.Append("</td>");
                if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                    num3 += 0.0;
                else
                    num3 += Convert.ToDouble(row["SchoolFee"].ToString());
                double num9 = 0.0;
                if (row["BusFee"].ToString() != "")
                    num9 = Convert.ToDouble(row["BusFee"].ToString());
                if (num9 <= 0.0)
                {
                    stringBuilder2.Append("<td align='right'>");
                    stringBuilder2.Append("0.00");
                    stringBuilder2.Append("</td>");
                }
                else
                {
                    stringBuilder2.Append("<td align='right'>");
                    string str7 = "<a href='javascript:popUp";
                    string str8 = "('rptMiscFeeList.aspx?Id=" + row["Admnno"].ToString() + "&&FP=F &AdId=1')";
                    string str9 = "'>";
                    stringBuilder2.Append(str7.Replace('\'', '"') + str8 + str9.Replace('\'', '"'));
                    stringBuilder2.Append("<b>" + row["BusFee"].ToString() + "</b></a>");
                    stringBuilder2.Append("</td>");
                    if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                        num4 += 0.0;
                    else
                        num4 += Convert.ToDouble(row["BusFee"].ToString());
                }
                double num10 = 0.0;
                if (row["BooksFee"].ToString() != "")
                    num10 = Convert.ToDouble(row["BooksFee"].ToString());
                if (num10 <= 0.0)
                {
                    stringBuilder2.Append("<td align='right'>");
                    stringBuilder2.Append("0.00");
                    stringBuilder2.Append("</td>");
                }
                else
                {
                    stringBuilder2.Append("<td align='right'>");
                    string str7 = "<a href='javascript:popUp";
                    string str8 = "('rptBookFeeList.aspx?Id=" + row["Admnno"].ToString() + "&&FP=F &AdId=2')";
                    string str9 = "'>";
                    stringBuilder2.Append(str7.Replace('\'', '"') + str8 + str9.Replace('\'', '"'));
                    stringBuilder2.Append("<b>" + row["BooksFee"].ToString() + "</b></a>");
                    stringBuilder2.Append("</td>");
                    if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                        num5 += 0.0;
                    else
                        num5 += Convert.ToDouble(row["BooksFee"].ToString());
                }
                stringBuilder2.Append("<td align='right'>");
                string str10 = "<a href='javascript:popUp";
                string str11 = "('rptAmountDueList.aspx?Id=" + row["Admnno"].ToString() + "&&FP=F')";
                string str12 = "'>";
                stringBuilder2.Append(str10.Replace('\'', '"') + str11 + str12.Replace('\'', '"'));
                stringBuilder2.Append("<b>" + row["TotalFee"].ToString() + "</b></a>");
                stringBuilder2.Append("</td>");
                if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                    num6 += 0.0;
                else
                    num6 += Convert.ToDouble(row["TotalFee"].ToString());
                if (Convert.ToDouble(row["PaidAmount"].ToString()) <= 0.0)
                {
                    stringBuilder2.Append("<td align='right'>");
                    stringBuilder2.Append("0.00");
                    stringBuilder2.Append("</td>");
                }
                else
                {
                    stringBuilder2.Append("<td align='right'>");
                    string str7 = "<a href='javascript:popUp";
                    string str8 = "('rptAmountPaidList.aspx?Id=" + row["Admnno"].ToString() + "&&FP=F')";
                    string str9 = "'>";
                    stringBuilder2.Append(str7.Replace('\'', '"') + str8 + str9.Replace('\'', '"'));
                    stringBuilder2.Append("<b>" + row["PaidAmount"].ToString() + "</b></a>");
                    stringBuilder2.Append("</td>");
                    if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                        num7 += 0.0;
                    else
                        num7 += Convert.ToDouble(row["PaidAmount"].ToString());
                }
                stringBuilder2.Append("<td align='right'>");
                string str13 = "<a href='javascript:popUp";
                string str14 = "('rptFeeBalance.aspx?admnno=" + row["Admnno"].ToString() + "&AD=" + row["TotalFee"].ToString() + " &AP=" + row["PaidAmount"].ToString() + "&&FP=F')";
                string str15 = "'>";
                stringBuilder2.Append(str13.Replace('\'', '"') + str14 + str15.Replace('\'', '"'));
                stringBuilder2.Append("<b>" + row["Balance"].ToString() + "</b></a>");
                stringBuilder2.Append("</td>");
                if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                    num8 += 0.0;
                else
                    num8 += Convert.ToDouble(row["Balance"].ToString());
            }
            stringBuilder2.Append("</tr>");
            if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
            {
                stringBuilder2.Length = 0;
                stringBuilder2.Capacity = 0;
            }
            else
            {
                stringBuilder1.Append(stringBuilder2.ToString() ?? "");
                stringBuilder2.Length = 0;
                stringBuilder2.Capacity = 0;
                ++num1;
            }
        }
        stringBuilder1.Append("<tr style='font-weight: bold; text-align: right; background-color: #CCCCCC;'>");
        stringBuilder1.Append("<td align='right' style='Font-Size:14px;' colspan='3'><b>Grand Total :</b></td>");
        stringBuilder1.Append("<td align='right'><b>" + num2.ToString() + "</b></td>");
        stringBuilder1.Append("<td align='right'><b>" + num3.ToString() + "</b></td>");
        //stringBuilder1.Append("<td align='right'><b>" + num4.ToString() + "</b></td>");
        //stringBuilder1.Append("<td align='right'><b>" + num5.ToString() + "</b></td>");
        stringBuilder1.Append("<td align='right'><b>" + num6.ToString() + "</b></td>");
        stringBuilder1.Append("<td align='right'><b>" + num7.ToString() + "</b></td>");
        stringBuilder1.Append("<td align='right'><b>" + num8.ToString() + "</b></td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("</table>");
        lblReport.Text = stringBuilder1.ToString().Trim();
        Session["FeeStatus"] = Regex.Replace(stringBuilder1.ToString(), "<a\\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptFeeStatusPrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td align='left' colspan='14'><b>");
                stringBuilder.Append("Student Fee Status Report :-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["FeeStatus"].ToString()).ToString().Trim());
            }
            else
                Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
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
            string str = Server.MapPath("Exported_Files/StudentFeeStatusRpt" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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

    protected void rbtnTillDt_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnTillDt.Checked)
        {
            rbtnFullSess.Checked = false;
            lblReport.Text = string.Empty;
            btnExpExcel.Enabled = false;
            btnPrint.Enabled = false;
        }
        else
        {
            rbtnTillDt.Checked = false;
            lblReport.Text = string.Empty;
            btnExpExcel.Enabled = false;
            btnPrint.Enabled = false;
        }
    }
}
