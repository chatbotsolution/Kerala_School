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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Reports_rptQtrlyFeeStatus : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        fillclass();
        fillSession();
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
    }
    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = obj.GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("--All--", "0"));
    }

    private void fillSession()
    {
        DataTable dataTable = new DataTable();
        drpSession.DataSource = obj.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Session", drpSession.SelectedValue.ToString().Trim());
        if (drpclass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpclass.SelectedValue.ToString().Trim());
        if (drpQuarter.SelectedIndex > 0)
        {
            FeeDetails fd = new FeeDetails();
            if (drpQuarter.SelectedValue == "1")
            {
                hashtable.Add("@Quarter", "APR-JUN");
                hashtable.Add("@TransDate", fd.getDuedate("APR-JUN", drpSession.SelectedValue.ToString().Trim()));
            }
            else if (drpQuarter.SelectedValue == "2")
            {
                hashtable.Add("@Quarter", "JUL-SEP");
                hashtable.Add("@TransDate", fd.getDuedate("JUL-SEP", drpSession.SelectedValue.ToString().Trim()));
            }
            else if (drpQuarter.SelectedValue == "3")
            {
                hashtable.Add("@Quarter", "OCT-DEC");
                hashtable.Add("@TransDate", fd.getDuedate("OCT-DEC", drpSession.SelectedValue.ToString().Trim()));
            }
            else
            {
                hashtable.Add("@Quarter", "JAN-MAR");
                hashtable.Add("@TransDate", fd.getDuedate("JAN-MAR", drpSession.SelectedValue.ToString().Trim()));
            }
        }
       // DataTable dt = drpclass.SelectedIndex <= 0 ? obj.GetDataTable("NewGetAllQtrlyFeeStatus", hashtable) : obj.GetDataTable("NewQuarterlyFeeStatus", hashtable);
        DataTable dt = new DataTable();
        
        if (drpclass.SelectedIndex <= 0)
        {
             dt = obj.GetDataTable("NewGetAllQtrlyFeeStatus", hashtable);
        }
        else
        {
            dt = obj.GetDataTable("NewQuarterlyFeeStatus", hashtable);
            //if (ds != null)
            //{
            //    ds.Tables[0].Columns.Add("TotalFee");
            //    ds.Tables[0].Columns.Add("SchoolFee");
            //    for (int i=0; i<ds.Tables[1].Rows.Count;i++)
            //    {
            //        ds.Tables[0].Rows[i]["SchoolFee"]= ds.Tables[1].Rows[i]["SchoolFee"].ToString();
            //        ds.Tables[0].Rows[i]["TotalFee"] = Convert.ToString(Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["SchoolFee"].ToString()) + Convert.ToDouble(ds.Tables[0].Rows[i]["Fine"].ToString()),2));
            //    }
            //}
            //dt = ds.Tables[0];

        }

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
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptQtrlyFeeStatusPrint.aspx");
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
                stringBuilder.Append("Defaulter For Bank :-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["QtrlyFeeStatus"].ToString()).ToString().Trim());
            }
            else
                Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
    private void GenerateReport(DataTable dt)
    {
        StringBuilder stringBuilder1 = new StringBuilder("");
        stringBuilder1.Append("<table border='1px' cellpadding='0' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;'   width='100%'>");
        if (drpclass.SelectedIndex > 0)
            stringBuilder1.Append("<tr style='background-color:#CCCCCC'><td colspan='9' align='center'><b>Quarterly Fee Status Report of Class-" + drpclass.SelectedItem.ToString() + " For The Session Year " + drpSession.SelectedItem.ToString() + " ( " + drpQuarter.SelectedItem.ToString() + " ) </b></td></tr>");
        else
            stringBuilder1.Append("<tr style='background-color:#CCCCCC'><td colspan='8' align='center'><b>Quarterly Fee Status Report for all Classes For The Session Year " + drpSession.SelectedItem.ToString() + " ( " + drpQuarter.SelectedItem.ToString() + " ) </b></td></tr>");
        stringBuilder1.Append("<tr style='background-color:#eee'>");
        stringBuilder1.Append("<td style='width: 40px' align='center'><strong>Sl. No.</strong></td>");
        if (drpclass.SelectedIndex > 0)
        {
            stringBuilder1.Append("<td style='width: 100px' align='right'><strong>Admission No</strong></td>");
            stringBuilder1.Append("<td align='left'><strong>Student Name</strong></td>");
        }
        else
            stringBuilder1.Append("<td style='width: 100px;' align='left'><strong>Class</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Quarterly Fee</strong></td>");
         stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Fine</strong></td>");
       // stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Books Fee</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Total Amount Due</strong></td>");
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
        StringBuilder stringBuilder2 = new StringBuilder("");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='center'>" + num1.ToString() + "</td>");
            if (drpclass.SelectedIndex > 0)
            {
                stringBuilder2.Append("<td align='right'>");
                stringBuilder2.Append(row["OldAdmnno"].ToString().Trim());
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='left'>");
                stringBuilder2.Append(row["FullName"].ToString().Trim());
                stringBuilder2.Append("</td>");
            }
            else
            {
                stringBuilder2.Append("<td align='left'>");
                stringBuilder2.Append(row["ClassName"].ToString().Trim());
                stringBuilder2.Append("</td>");
            }
            stringBuilder2.Append("<td align='right'>");
            stringBuilder2.Append("<b>" + row["SchoolFee"].ToString() + "</b>");
            stringBuilder2.Append("</td>");
            if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                num2 += 0.0;
            else
                num2 += Convert.ToDouble(row["SchoolFee"].ToString());
            double num8 = 0.0;
            if (row["Fine"].ToString() != "")
                num8 = Convert.ToDouble(row["Fine"].ToString());
            if (num8 <= 0.0)
            {
                stringBuilder2.Append("<td align='right'>");
                stringBuilder2.Append("0.00");
                stringBuilder2.Append("</td>");
            }
            else
            {
                stringBuilder2.Append("<td align='right'>");
                stringBuilder2.Append("<b>" + row["Fine"].ToString() + "</b>");
                stringBuilder2.Append("</td>");
                if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                    num3 += 0.0;
                else
                    num3 += Convert.ToDouble(row["Fine"].ToString());
            }
           
            stringBuilder2.Append("<td align='right'>");
            stringBuilder2.Append("<b>" + row["TotalFee"].ToString() + "</b>");
            stringBuilder2.Append("</td>");
            if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                num5 += 0.0;
            else
                num5 += Convert.ToDouble(row["TotalFee"].ToString());
            if (Convert.ToDouble(row["PaidAmount"].ToString()) <= 0.0)
            {
                stringBuilder2.Append("<td align='right'>");
                stringBuilder2.Append("0.00");
                stringBuilder2.Append("</td>");
            }
            else
            {
                stringBuilder2.Append("<td align='right'>");
                stringBuilder2.Append("<b>" + row["PaidAmount"].ToString() + "</b>");
                stringBuilder2.Append("</td>");
                if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                    num6 += 0.0;
                else
                    num6 += Convert.ToDouble(row["PaidAmount"].ToString());
            }
            stringBuilder2.Append("<td align='right'>");
            stringBuilder2.Append("<b>" + row["Balance"].ToString() + "</b>");
            stringBuilder2.Append("</td>");
            stringBuilder2.Append("</tr>");
            if (Convert.ToDouble(row["Balance"].ToString()) == 0.0 && (int)Convert.ToInt16(row["Status"].ToString()) == 3)
                num7 += 0.0;
            else
                num7 += Convert.ToDouble(row["Balance"].ToString());
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
        if (drpclass.SelectedIndex > 0)
            stringBuilder1.Append("<td align='right' style='Font-Size:14px;' colspan='3'><b>Grand Total :</b></td>");
        else
            stringBuilder1.Append("<td align='right' style='Font-Size:14px;' colspan='2'><b>Grand Total :</b></td>");
        stringBuilder1.Append("<td align='right'><b>" + num2.ToString() + "</b></td>");
        stringBuilder1.Append("<td align='right'><b>" + num3.ToString() + "</b></td>");
        //stringBuilder1.Append("<td align='right'><b>" + num4.ToString() + "</b></td>");
        stringBuilder1.Append("<td align='right'><b>" + num5.ToString() + "</b></td>");
        stringBuilder1.Append("<td align='right'><b>" + num6.ToString() + "</b></td>");
        stringBuilder1.Append("<td align='right'><b>" + num7.ToString() + "</b></td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("</table>");
        lblReport.Text = stringBuilder1.ToString().Trim();
        Session["QtrlyFeeStatus"] = stringBuilder1.ToString();
    }
    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/QtrlyStudentFeeStatusRpt_" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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