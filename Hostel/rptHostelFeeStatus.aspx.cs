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


public partial class Hostel_rptHostelFeeStatus : System.Web.UI.Page
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
        currSession.Split('-');
        DataTable dt = !rbtnTillDt.Checked ? common.GetDataTable("Host_RptGetFullFeeStatus", hashtable) : common.GetDataTable("HostRptGetFeeStatus", hashtable);
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
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
        stringBuilder.Append("<tr style='background-color:#CCCCCC'>");
        stringBuilder.Append("<td style='width: 70px' align='right'><strong>Sl. No.</strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='right'><strong>Admission No</strong></td>");
        stringBuilder.Append("<td align='left'><strong>Student Name</strong></td>");
        stringBuilder.Append("<td style='width: 120px' align='right'><strong>Previous Year Due</strong></td>");
        stringBuilder.Append("<td style='width: 120px' align='right'><strong>Hostel Fee</strong></td>");
        stringBuilder.Append("<td style='width: 120px' align='right'><strong>Total Amount Paid</strong></td>");
        stringBuilder.Append("<td style='width: 120px' align='right'><strong>Balance</strong></td>");
        stringBuilder.Append("</tr>");
        int num = 1;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='right'>" + num.ToString() + "</td>");
            stringBuilder.Append("<td align='right'>");
            stringBuilder.Append(row["Admnno"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right'>");
            string str1 = "<a href='javascript:popUp";
            string str2 = "('rptHostPrevYrBal.aspx?Admnno=" + row["Admnno"].ToString() + "')";
            string str3 = "'>";
            stringBuilder.Append(str1.Replace('\'', '"') + str2 + str3.Replace('\'', '"'));
            stringBuilder.Append("<b>" + row["PrevBal"].ToString() + "</b></a>");
            stringBuilder.Append("</td>");
            if (rbtnTillDt.Checked)
            {
                stringBuilder.Append("<td align='right'>");
                string str4 = "<a href='javascript:popUp";
                string str5 = "('rptHostelFeeList.aspx?Id=" + row["Admnno"].ToString() + "')";
                string str6 = "'>";
                stringBuilder.Append(str4.Replace('\'', '"') + str5 + str6.Replace('\'', '"'));
                stringBuilder.Append("<b>" + row["SchoolFee"].ToString() + "</b></a>");
                stringBuilder.Append("</td>");
                if (Convert.ToDouble(row["PaidAmount"].ToString()) <= 0.0)
                {
                    stringBuilder.Append("<td align='right'>");
                    stringBuilder.Append("0.00");
                    stringBuilder.Append("</td>");
                }
                else
                {
                    stringBuilder.Append("<td align='right'>");
                    string str7 = "<a href='javascript:popUp";
                    string str8 = "('rptHostelAmountPaidList.aspx?Id=" + row["Admnno"].ToString() + "')";
                    string str9 = "'>";
                    stringBuilder.Append(str7.Replace('\'', '"') + str8 + str9.Replace('\'', '"'));
                    stringBuilder.Append("<b>" + row["PaidAmount"].ToString() + "</b></a>");
                    stringBuilder.Append("</td>");
                }
                stringBuilder.Append("<td align='right'>");
                string str10 = "<a href='javascript:popUp";
                string str11 = "('rptHostelFeeBalance.aspx?admnno=" + row["Admnno"].ToString() + " &AD=" + row["TotalFee"].ToString() + " &AP=" + row["PaidAmount"].ToString() + "')";
                string str12 = "'>";
                stringBuilder.Append(str10.Replace('\'', '"') + str11 + str12.Replace('\'', '"'));
                stringBuilder.Append("<b>" + row["Balance"].ToString() + "</b></a>");
                stringBuilder.Append("</td>");
            }
            else
            {
                stringBuilder.Append("<td align='right'>");
                string str4 = "<a href='javascript:popUp";
                string str5 = "('rptHostelFeeList.aspx?Id=" + row["Admnno"].ToString() + "&&FP=F')";
                string str6 = "'>";
                stringBuilder.Append(str4.Replace('\'', '"') + str5 + str6.Replace('\'', '"'));
                stringBuilder.Append("<b>" + row["SchoolFee"].ToString() + "</b></a>");
                stringBuilder.Append("</td>");
                if (Convert.ToDouble(row["PaidAmount"].ToString()) <= 0.0)
                {
                    stringBuilder.Append("<td align='right'>");
                    stringBuilder.Append("0.00");
                    stringBuilder.Append("</td>");
                }
                else
                {
                    stringBuilder.Append("<td align='right'>");
                    string str7 = "<a href='javascript:popUp";
                    string str8 = "('rptHostelAmountPaidList.aspx?Id=" + row["Admnno"].ToString() + "&&FP=F')";
                    string str9 = "'>";
                    stringBuilder.Append(str7.Replace('\'', '"') + str8 + str9.Replace('\'', '"'));
                    stringBuilder.Append("<b>" + row["PaidAmount"].ToString() + "</b></a>");
                    stringBuilder.Append("</td>");
                }
                stringBuilder.Append("<td align='right'>");
                string str10 = "<a href='javascript:popUp";
                string str11 = "('rptHostelFeeBalance.aspx?admnno=" + row["Admnno"].ToString() + "&AD=" + row["TotalFee"].ToString() + " &AP=" + row["PaidAmount"].ToString() + "&&FP=F')";
                string str12 = "'>";
                stringBuilder.Append(str10.Replace('\'', '"') + str11 + str12.Replace('\'', '"'));
                stringBuilder.Append("<b>" + row["Balance"].ToString() + "</b></a>");
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
            ++num;
        }
        string empty = string.Empty;
        string str13 = dt.Compute("SUM(PrevBal)", "").ToString();
        string str14 = dt.Compute("SUM(SchoolFee)", "").ToString();
        string str15 = dt.Compute("SUM(PaidAmount)", "").ToString();
        string str16 = dt.Compute("SUM(Balance)", "").ToString();
        stringBuilder.Append("<tr style='font-weight: bold; text-align: right; background-color: #CCCCCC;'>");
        stringBuilder.Append("<td align='right' style='Font-Size:14px;' colspan='3'><b>Grand Total :</b></td>");
        stringBuilder.Append("<td align='right'><b>" + str13.ToString() + "</b></td>");
        stringBuilder.Append("<td align='right'><b>" + str14.ToString() + "</b></td>");
        stringBuilder.Append("<td align='right'><b>" + str15.ToString() + "</b></td>");
        stringBuilder.Append("<td align='right'><b>" + str16.ToString() + "</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["FeeStatus"] = Regex.Replace(stringBuilder.ToString(), "<a\\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptHostelFeeStatusPrint.aspx');", true);
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
            string str = Server.MapPath("Exported_Files/HostelStudentFeeStatusRpt" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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