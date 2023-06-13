﻿using ASP;
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
using Classes.DA;

public partial class Reports_rptDefaulters : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnPrint.Enabled = false;
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        drpDueMnth.SelectedValue = DateTime.Now.Month.ToString();
        fillsession();
        fillclass();
        
    }

    protected void fillsession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = new clsDAL().GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("-All-", "0"));
    }

    private void FillClassSection()
    {
        ddlSection.Items.Clear();
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@classId", drpclass.SelectedValue.ToString().Trim());
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        hashtable.Add("@session", clsGenerateFee.CreateCurrSession());
        ddlSection.DataSource = clsDal.GetDataTable("ps_sp_get_ClassSections", hashtable);
        ddlSection.DataTextField = "Section";
        ddlSection.DataValueField = "Section";
        ddlSection.DataBind();
        ddlSection.Items.Insert(0, new ListItem("All", "0"));
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        lblTotRec.Text = "";
        if (rbConsolidated.Checked)
            GetReportTable();
        if (!rbDetails.Checked)
            return;
        GetReportTableDetails();
    }

    protected void GetReportTable()
    {
        Session["DueDate"] = string.Empty;
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpclass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpclass.SelectedValue.ToString().Trim());
        if (ddlSection.SelectedIndex > 0)
            hashtable.Add("@Section", ddlSection.SelectedValue.ToString().Trim());
        if (drpFeeType.SelectedIndex > 0)
            hashtable.Add("@Type", drpFeeType.SelectedValue.Trim());
        if (drpDueMnth.SelectedIndex > 0)
        {
            string feeDateStr = GetFeeDateStr();
            hashtable.Add("@DueDate", feeDateStr);
            Session["DueDate"] = feeDateStr;
        }
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
        //DataTable dataTable = clsDal.GetDataTable("Ps_Sp_GetFeeDefaulters", hashtable);
        DataTable dataTable =new Common().GetDataTable("Ps_Sp_GetFeeDefaulters", hashtable);
      
        if (dataTable.Rows.Count > 0)
        {
            lblTotRec.Text = "Total Record:-" + dataTable.Rows.Count.ToString();
            lblGrdMsg.Text = string.Empty;
            createDefaultersReport(dataTable);
            btnPrint.Enabled = true;
        }
        else
        {
            lblTotRec.Text = "";
            lblReport.Text = string.Empty;
            lblGrdMsg.Text = "No Record Found.";
            btnPrint.Enabled = false;
        }
    }

    protected void GetReportTableDetails()
    {
        Session["DueDate"] = string.Empty;
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpclass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpclass.SelectedValue.ToString().Trim());
        if (ddlSection.SelectedIndex > 0)
            hashtable.Add("@Section", ddlSection.SelectedValue.ToString().Trim());
        if (drpFeeType.SelectedIndex > 0)
            hashtable.Add("@Type", drpFeeType.SelectedValue.Trim());
        if (drpDueMnth.SelectedIndex > 0)
        {
            string feeDateStr = GetFeeDateStr();
            hashtable.Add("@DueDate", feeDateStr);
            Session["DueDate"] = feeDateStr;
        }
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
        DataTable dataTable = clsDal.GetDataTable("Ps_Sp_GetFeeDefaulters", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            lblTotRec.Text = "Total Record:-" + dataTable.Rows.Count.ToString();
            lblGrdMsg.Text = string.Empty;
            createDefaultersReportDetails(dataTable);
            btnPrint.Enabled = true;
        }
        else
        {
            lblTotRec.Text = "";
            lblReport.Text = string.Empty;
            lblGrdMsg.Text = "No Record Found.";
            btnPrint.Enabled = false;
        }
    }

    private string GetFeeDateStr()
    {
        clsDAL clsDal = new clsDAL();
        string str1 = drpDueMnth.SelectedValue.ToString();
        string str2 = (Convert.ToInt32(drpDueMnth.SelectedValue.ToString().Trim()) + 1).ToString();
        string str3 = drpSession.SelectedValue.Trim().Substring(0, 4);
        string str4 = drpSession.SelectedValue.Trim().Substring(0, 2) + drpSession.SelectedValue.Trim().Substring(5, 2);
        return Convert.ToInt32(str1) <= 3 ? str2 + "/01/" + str4 : (Convert.ToInt32(str1) <= 11 ? str2 + "/01/" + str3 : "01/01/" + str4);
    }

    private DateTime feedate()
    {
        clsDAL clsDal = new clsDAL();
        string str1 = drpDueMnth.SelectedValue.ToString();
        string str2 = (Convert.ToInt32(drpDueMnth.SelectedValue.ToString().Trim()) + 1).ToString();
        string str3 = clsDal.ExecuteScalarQry("select top 1 SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        string str4 = str3.Substring(0, 4);
        string str5 = str3.Substring(0, 2) + str3.Substring(5, 2);
        DateTime dateTime;
        if (Convert.ToInt32(str1) > 3)
        {
            if (Convert.ToInt32(str1) > 11)
            {
                dateTime = Convert.ToDateTime("01/01/" + str5);
                dateTime.Date.AddDays(-1.0);
            }
            else
            {
                dateTime = Convert.ToDateTime(str2 + "/01/" + str4);
                dateTime.Date.AddDays(-1.0);
            }
        }
        else
        {
            dateTime = Convert.ToDateTime(str2 + "/01/" + str5);
            dateTime.Date.AddDays(-1.0);
        }
        return dateTime;
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
        lblReport.Text = string.Empty;
        btnPrint.Enabled = false;
    }

    private void createDefaultersReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
        stringBuilder.Append("<tr style='background-color:#CCCCCC'>");
        stringBuilder.Append("<td style='width: 100px' align='left'><strong><u>Admission No</u></strong></td>");
        stringBuilder.Append("<td align='left'><strong><u>Name</u></strong></td>");
        stringBuilder.Append("<td style='width: 70px' align='left'><strong><u>Class</u></strong></td>");
        stringBuilder.Append("<td style='width: 50px' align='left'><strong><u>Section</u></strong></td>");
        stringBuilder.Append("<td style='width: 90px' align='left'><strong><u>Roll No</u></strong></td>");
        //stringBuilder.Append("<td style='width: 120px' align='right'><strong><u>Regular Fee Due</u></strong></td>");
        //stringBuilder.Append("<td style='width: 100px' align='right'><strong><u>Bus Due</u></strong></td>");
        //stringBuilder.Append("<td style='width: 100px' align='right'><strong><u>Books Due</u></strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='right'><strong><u>Total Due</u></strong></td>");
        stringBuilder.Append("</tr>");
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["AdmnNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["fullname"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["ClassName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Section"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["RollNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            int num5 = 0;
            if (drpFeeType.SelectedIndex > 0)
                num5 = int.Parse(drpFeeType.SelectedValue.Trim());
            //stringBuilder.Append("<td align='right'>");
            //string str1 = "<a href='javascript:popUp";
            //string str2 = "('rptDefaultersDetails.aspx?Admnno=" + row["AdmnsNo"].ToString() + " &Type=" + num5 + "&DueDt=" + Session["DueDate"].ToString().Trim() + "&SessionYr=" + drpSession.SelectedValue.Trim() + "')";
            //string str3 = "'>";
            //stringBuilder.Append(str1.Replace('\'', '"') + str2 + str3.Replace('\'', '"'));
            //stringBuilder.Append("<b>" + row["totBalance"].ToString() + "</b></a>");
            //stringBuilder.Append("</td>");
            //stringBuilder.Append("<td align='right'>");
            //string str4 = "<a href='javascript:popUp";
            //string str5 = "('rptDefaultersDetailsAdFee.aspx?Admnno=" + row["AdmnsNo"].ToString() + " &Ad=1&DueDt=" + Session["DueDate"].ToString().Trim() + "&SessionYr=" + drpSession.SelectedValue.Trim() + "')";
            //string str6 = "'>";
            //stringBuilder.Append(str4.Replace('\'', '"') + str5 + str6.Replace('\'', '"'));
            //stringBuilder.Append("<b>" + row["totBusBalance"].ToString() + "</b></a>");
            //stringBuilder.Append("</td>");
            //stringBuilder.Append("<td align='right'>");
            //string str7 = "<a href='javascript:popUp";
            //string str8 = "('rptDefaultersDetailsBooks.aspx?Admnno=" + row["AdmnsNo"].ToString() + " &Ad=2&DueDt=" + Session["DueDate"].ToString().Trim() + "&SessionYr=" + drpSession.SelectedValue.Trim() + "')";
            //string str9 = "'>";
            //stringBuilder.Append(str7.Replace('\'', '"') + str8 + str9.Replace('\'', '"'));
            //stringBuilder.Append("<b>" + row["totBooksBalance"].ToString() + "</b></a>");
            //stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right'>");
            stringBuilder.Append(row["totfeedues"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            num1 += Convert.ToDouble(row["totBalance"].ToString().Trim().Equals(string.Empty) ? "0" : row["totBalance"].ToString().Trim());
            //num2 += Convert.ToDouble(row["totBusBalance"].ToString().Trim().Equals(string.Empty) ? "0" : row["totBusBalance"].ToString().Trim());
            //num3 += Convert.ToDouble(row["totBooksBalance"].ToString().Trim().Equals(string.Empty) ? "0" : row["totBooksBalance"].ToString().Trim());
            num4 += Convert.ToDouble(row["totfeedues"].ToString().Trim().Equals(string.Empty) ? "0" : row["totfeedues"].ToString().Trim());
        }
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='5'  align='right'  class='tbltd'>");
        stringBuilder.Append("<strong>Grand Total &nbsp;:&nbsp;</strong>");
        //stringBuilder.Append("</td>");
        //stringBuilder.Append("<td align='right'>");
        //stringBuilder.Append("<strong>" + string.Format("{0:F2}", num1));
        //stringBuilder.Append("</strong></td>");
        //stringBuilder.Append("<td align='right'>");
        //stringBuilder.Append("<strong>" + string.Format("{0:F2}", num2));
        //stringBuilder.Append("</strong></td>");
        //stringBuilder.Append("<td align='right'>");
        //stringBuilder.Append("<strong>" + string.Format("{0:F2}", num3));
        //stringBuilder.Append("</strong></td>");
        stringBuilder.Append("<td align='right'>");
        stringBuilder.Append("<strong>" + string.Format("{0:F2}", num4));
        stringBuilder.Append("</strong></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["rptDefaulters"] = stringBuilder.ToString();
    }

    private void createDefaultersReportDetails(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder1 = new StringBuilder("");
        stringBuilder1.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
        stringBuilder1.Append("<tr style='background-color:#CCCCCC'>");
        stringBuilder1.Append("<td style='width: 100px' align='left'><strong><u>Admission No</u></strong></td>");
        stringBuilder1.Append("<td align='left' ><strong><u>Name</u></strong></td>");
        stringBuilder1.Append("<td style='width: 70px' align='left'><strong><u>Class</u></strong></td>");
        stringBuilder1.Append("<td style='width: 70px' align='left' ><strong><u>Section</u></strong></td>");
        stringBuilder1.Append("<td style='width: 100px' align='left' ><strong><u>Roll No</u></strong></td>");
        stringBuilder1.Append("<td style='width: 400px'><table width='100%'><td style='width: 100px; font-size:12px' align='left' ><strong><u>Date</u></strong></td><td style='width: 200px ; font-size:12px;' align='left'  ><strong><u>Details</u></strong></td><td style='width: 100px ; font-size:12px' align='right' ><strong><u>Amount</u></strong></td></table></td>");
        stringBuilder1.Append("</tr>");
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        foreach (DataRow row1 in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td align='left'>");
            stringBuilder1.Append(row1["AdmnNo"].ToString().Trim());
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td align='left'>");
            stringBuilder1.Append(row1["fullname"].ToString().Trim());
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td align='left'>");
            stringBuilder1.Append(row1["ClassName"].ToString().Trim());
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td align='left'>");
            stringBuilder1.Append(row1["Section"].ToString().Trim());
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td align='left'>");
            stringBuilder1.Append(row1["RollNo"].ToString().Trim());
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td align='centre'>");
            int num5 = 0;
            if (drpFeeType.SelectedIndex > 0)
                num5 = int.Parse(drpFeeType.SelectedValue.Trim());
            StringBuilder stringBuilder2 = new StringBuilder("");
            clsDAL clsDal = new clsDAL();
            DataTable dataTable1 = new DataTable();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@AdmnNo", row1["Admnno"].ToString());
            if (num5 > 0)
                hashtable.Add("@Type", num5);
            if (Session["DueDate"].ToString().Trim() != "")
                hashtable.Add("@DueDate", Session["DueDate"].ToString().Trim());
            hashtable.Add("@SessionYr", drpSession.SelectedValue.Trim());
            DataTable dataTable2 = clsDal.GetDataTable("Ps_Sp_getFeeDefaultdetailAll", hashtable);
            if (dataTable2.Rows.Count > 0)
            {
                stringBuilder2.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
                foreach (DataRow row2 in (InternalDataCollectionBase)dataTable2.Rows)
                {
                    stringBuilder2.Append("<tr>");
                    stringBuilder2.Append("<td align='right' style='width: 100px'>");
                    stringBuilder2.Append(row2["TransDtStr"].ToString().Trim());
                    stringBuilder2.Append("</td>");
                    stringBuilder2.Append("<td align='left' style='width: 200px'>");
                    stringBuilder2.Append(row2["TransDesc"].ToString().Trim());
                    stringBuilder2.Append("</td>");
                    stringBuilder2.Append("<td align='right' style='width: 100px'>");
                    stringBuilder2.Append(row2["Balance"].ToString().Trim());
                    stringBuilder2.Append("</td>");
                    stringBuilder2.Append("</tr>");
                }
                string str = dataTable2.Compute("SUM(Balance)", "").ToString();
                stringBuilder2.Append("<tr style='font-weight: bold; text-align: left;'>");
                stringBuilder2.Append("<td align='right'  colspan='2'><b>Total Amount:</b></td>");
                stringBuilder2.Append("<td align='right'><b>" + str.ToString() + "</b></td>");
                stringBuilder2.Append("</tr>");
                stringBuilder2.Append("</table>");
            }
            else
            {
                stringBuilder2.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
                stringBuilder2.Append("<tr style='font-weight: bold; text-align: left;'>");
                stringBuilder2.Append("<td align='right'  colspan='2'><b>0.00</b></td>");
                stringBuilder2.Append("</tr>");
                stringBuilder2.Append("</table>");
            }
            stringBuilder1.Append("<b>" + stringBuilder2.ToString().Trim() + "</b>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("</tr>");
            num1 += Convert.ToDouble(row1["totBalance"].ToString().Trim().Equals(string.Empty) ? "0" : row1["totBalance"].ToString().Trim());
            //num2 += Convert.ToDouble(row1["totBusBalance"].ToString().Trim().Equals(string.Empty) ? "0" : row1["totBusBalance"].ToString().Trim());
            //num3 += Convert.ToDouble(row1["totBooksBalance"].ToString().Trim().Equals(string.Empty) ? "0" : row1["totBooksBalance"].ToString().Trim());
            num4 += Convert.ToDouble(row1["totfeedues"].ToString().Trim().Equals(string.Empty) ? "0" : row1["totfeedues"].ToString().Trim());
        }
        stringBuilder1.Append("<tr>");
        stringBuilder1.Append("<td colspan='5'  align='right'  class='tbltd'>");
        stringBuilder1.Append("<strong>Grand Total &nbsp;:&nbsp;</strong>");
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("<td align='right'>");
        stringBuilder1.Append("<strong>" + string.Format("{0:F2}", num4));
        stringBuilder1.Append("</strong></td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("</table>");
        lblReport.Text = stringBuilder1.ToString().Trim();
        Session["rptDefaulters"] = stringBuilder1.ToString();
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("<tr><td align='left'><b>");
                stringBuilder.Append("Defaulters:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + lblReport.Text.ToString().Trim()).ToString().Trim());
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
        string str = Server.MapPath("Exported_Files/FeeDefaulters-" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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

    protected void btnBack_Click(object sender, EventArgs e)
    {
        pnlDetail.Visible = false;
        GetReportTable();
        pnlSummary.Visible = true;
    }

    protected void btnPrint_Click1(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptDefaultersprint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpDueMnth_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["DueDate"] = string.Empty;
        lblReport.Text = string.Empty;
        btnPrint.Enabled = false;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        btnPrint.Enabled = false;
    }
}
