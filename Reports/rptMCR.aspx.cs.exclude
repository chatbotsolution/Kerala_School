using AjaxControlToolkit;
using ASP;
using SanLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptMCR : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            try
            {
                fillsession();
                fillclass();
                FillClassSection();
            }
            catch (Exception ex)
            {
            }
        }
        else
            Response.Redirect("~/Login.aspx");
    }

    private void fillsession()
    {
        dt = obj.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataSource = dt;
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        drpClass.Items.Clear();
        dt = obj.GetDataTable("ps_sp_get_classesForDDL");
        drpClass.DataSource = dt;
        drpClass.DataTextField = "classname";
        drpClass.DataValueField = "classid";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("All", "0"));
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillClassSection();
        }
        catch (Exception ex)
        {
        }
    }

    private void FillClassSection()
    {
        try
        {
            ddlSection.Items.Clear();
            ht.Clear();
            ht.Add("@classId", drpClass.SelectedValue.ToString().Trim());
            ht.Add("@session", drpSession.SelectedValue);
            dt = obj.GetDataTable("ps_sp_get_ClassSections", ht);
            ddlSection.DataSource = dt;
            ddlSection.DataTextField = "Section";
            ddlSection.DataValueField = "Section";
            ddlSection.DataBind();
            ddlSection.Items.Insert(0, new ListItem("All", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        DataSet dataSet1 = new DataSet();
        Decimal num1 = new Decimal(0);
        string str1 = "";
        Decimal num2 = new Decimal(0);
        Hashtable hashtable1 = new Hashtable();
        DataTable dataTable1 = new DataTable();
        ht.Clear();
        try
        {
            ht.Add("@sessionyr", drpSession.SelectedValue);
            if (drpClass.SelectedIndex > 0)
                ht.Add("@classId", drpClass.SelectedValue);
            if (ddlSection.SelectedIndex > 0)
                ht.Add("@section", ddlSection.SelectedValue);
            DataSet dataSet2 = obj.GetDataSet("ps_sp_AnnualMontlyFee", ht);
            if (dataSet2.Tables[0].Rows.Count > 0 || dataSet2.Tables[1].Rows.Count > 0)
            {
                StringBuilder stringBuilder1 = new StringBuilder("");
                stringBuilder1.Append("<div style='overflow:scroll; overflow-y:hidden; width:1000px;'>");
                stringBuilder1.Append("<table cellpadding='1' cellspacing='1' border='1' style='border-collapse:collapse'>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td colspan='29' style='text-align:left; font-weight:bold; font-size:16; padding:5px 0px 15px 0px;'>Monthly Collection Report For the Session : " + drpSession.SelectedValue + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Class : " + drpClass.SelectedItem + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Section : " + ddlSection.SelectedItem + "</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td rowspan='2' style='width:50px; font-weight:bold;' >SlNo</td>");
                stringBuilder1.Append("<td rowspan='2' style='font-weight:bold;' >Name</td>");
                stringBuilder1.Append("<td style='width:100px; font-weight:bold;' rowspan='2' >Annual Adm/Re-Adm</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Apr</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >May</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Jun</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Jul</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Aug</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Sep</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Oct</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Nov</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Dec</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Jan</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Feb</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Mar</td>");
                stringBuilder1.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Total</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("<tr>");
                for (int index = 0; index < 12; ++index)
                {
                    stringBuilder1.Append("<td style='width: 100px; text-align:left; font-weight:bold;' >Amount</td>");
                    stringBuilder1.Append("<td style='width: 200px; text-align:left; font-weight:bold;' >R.No/Date</td>");
                }
                stringBuilder1.Append("<td style='width: 100px; text-align:left; font-weight:bold;' >Received</td>");
                stringBuilder1.Append("<td style='width: 200px; text-align:left; font-weight:bold;' >Due</td>");
                stringBuilder1.Append("</tr>");
                int num3 = 1;
                StringBuilder stringBuilder2 = new StringBuilder("");
                if (dataSet2.Tables[0].Rows.Count > 0)
                {
                    if (dataSet2.Tables[1].Rows.Count > 0)
                    {
                        int index1 = 0;
                        int index2 = 0;
                        while (index2 < dataSet2.Tables[0].Rows.Count)
                        {
                            string s = dataSet2.Tables[0].Rows[index2]["admnno"].ToString();
                            DataTable dataTable2 = new DataTable();
                            DataRow[] dataRowArray = dataSet2.Tables[2].Select("AdmnNo=" + s);
                            if (dataRowArray.Length > 0)
                                ((IEnumerable<DataRow>)dataRowArray).CopyToDataTable<DataRow>();
                            if (index1 == dataSet2.Tables[1].Rows.Count || long.Parse(s) < long.Parse(dataSet2.Tables[1].Rows[index1]["admnno"].ToString()))
                            {
                                stringBuilder2.Append("<tr class='tbltd'>");
                                stringBuilder2.Append("<td>" + num3 + "</td>");
                                stringBuilder2.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[0].Rows[index2]["FullName"] + "</td>");
                                stringBuilder2.Append("<td>" + dataSet2.Tables[0].Rows[index2]["Amt"] + "</td>");
                                int num4 = 12;
                                while (num4-- > 0)
                                {
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                }
                                num1 = Convert.ToDecimal(dataSet2.Tables[0].Rows[index2]["Amt"].ToString().Trim());
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable3 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (s != "")
                                {
                                    hashtable2.Add("@AdmnNo", s);
                                    dataTable3 = obj.GetDataTable("ps_sp_AnnualMontlyFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                {
                                    stringBuilder2.Append("<td align='right'>" + num1 + "</td>");
                                    num2 += num1;
                                }
                                else
                                    stringBuilder2.Append("<td align='right'>0.00</td>");
                                if (dataTable3.Rows.Count > 0)
                                    stringBuilder2.Append("<td align='right'>" + Convert.ToDecimal(dataTable3.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                                else
                                    stringBuilder2.Append("<td align='right'>0.00</td>");
                                ++index2;
                            }
                            else if (long.Parse(s) > long.Parse(dataSet2.Tables[1].Rows[index1]["admnno"].ToString()))
                            {
                                string str2 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder2.Append("<tr class='tbltd'>");
                                stringBuilder2.Append("<td>" + num3 + "</td>");
                                stringBuilder2.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[1].Rows[index1]["FullName"] + "</td>");
                                stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                if (dataSet2.Tables[1].Rows[index1]["admnno"].ToString().Trim() == dataSet2.Tables[0].Rows[index2]["admnno"].ToString().Trim())
                                    num1 = Convert.ToDecimal(dataSet2.Tables[0].Rows[index2]["Amt"].ToString().Trim());
                                int num4 = 0;
                                int num5 = 0;
                                int int16 = int.Parse(dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString());
                                if (int16 > 4)
                                    num4 = int16 - 4;
                                else if (int16 < 4)
                                    num4 = 8 + int16;
                                while (num4-- > 0)
                                {
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                    ++num5;
                                }
                                StringBuilder stringBuilder3 = new StringBuilder();
                                StringBuilder stringBuilder4 = new StringBuilder();
                                for (; index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str2; ++index1)
                                {
                                    if (int16 == (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index1]["MonthNo"]))
                                    {
                                        if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                        {
                                            double num6 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                            num1 += Convert.ToDecimal(num6);
                                            stringBuilder4.Append(dataSet2.Tables[1].Rows[index1]["Amount"].ToString() + "<br/>");
                                            stringBuilder3.Append(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")<br/>");
                                        }
                                    }
                                    else
                                    {
                                        int16 = (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index1]["MonthNo"]);
                                        if (stringBuilder4.ToString().Trim() != "")
                                            stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "</td>");
                                        else
                                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                        if (stringBuilder4.ToString().Trim() != "")
                                            stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder3.ToString() + "</td>");
                                        else
                                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                        stringBuilder4 = new StringBuilder();
                                        stringBuilder3 = new StringBuilder();
                                        if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                        {
                                            double num6 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                            num1 += Convert.ToDecimal(num6);
                                            stringBuilder4.Append(dataSet2.Tables[1].Rows[index1]["Amount"].ToString() + "<br/>");
                                            stringBuilder3.Append(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")<br/>");
                                        }
                                        ++num5;
                                    }
                                }
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "</td>");
                                else
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder3.ToString() + "</td>");
                                else
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                int num7 = num5 + 1;
                                while (num7++ < 12)
                                {
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                }
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable3 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str2 != "")
                                {
                                    hashtable2.Add("@AdmnNo", str2);
                                    dataTable3 = obj.GetDataTable("ps_sp_AnnualMontlyFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                {
                                    stringBuilder2.Append("<td align='right' valign='top'>" + num1 + "</td>");
                                    num2 += num1;
                                }
                                else
                                    stringBuilder2.Append("<td align='right' valign='top'>0.00</td>");
                                if (dataTable3.Rows.Count > 0)
                                    stringBuilder2.Append("<td align='right' valign='top'>" + Convert.ToDecimal(dataTable3.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                                else
                                    stringBuilder2.Append("<td align='right' valign='top'>0.00</td>");
                                stringBuilder2.Append("</tr>");
                                if (num1 == new Decimal(0) && (int)Convert.ToInt16(dataSet2.Tables[0].Rows[index2]["Status"].ToString()) == 3)
                                {
                                    stringBuilder2.Length = 0;
                                    stringBuilder2.Capacity = 0;
                                }
                                else
                                {
                                    stringBuilder1.Append(stringBuilder2.ToString() ?? "");
                                    stringBuilder2.Length = 0;
                                    stringBuilder2.Capacity = 0;
                                    ++num3;
                                }
                                num1 = new Decimal(0);
                            }
                            else
                            {
                                string str2 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder2.Append("<tr class='tbltd'>");
                                stringBuilder2.Append("<td>" + num3 + "</td>");
                                stringBuilder2.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[0].Rows[index2]["FullName"] + "</td>");
                                stringBuilder2.Append("<td valign='top' >" + dataSet2.Tables[0].Rows[index2]["Amt"] + "</td>");
                                Decimal num4 = Convert.ToDecimal(dataSet2.Tables[0].Rows[index2]["Amt"].ToString().Trim());
                                int num5 = 0;
                                int num6 = 0;
                                int int16 = int.Parse(dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString());
                                if (int16 > 4)
                                    num5 = int16 - 4;
                                else if (int16 < 4)
                                    num5 = 8 + int16;
                                while (num5-- > 0)
                                {
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                    ++num6;
                                }
                                StringBuilder stringBuilder3 = new StringBuilder();
                                StringBuilder stringBuilder4 = new StringBuilder();
                                for (; index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str2; ++index1)
                                {
                                    if (int16 == (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index1]["MonthNo"]))
                                    {
                                        if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                        {
                                            double num7 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                            num4 += Convert.ToDecimal(num7);
                                            stringBuilder4.Append(dataSet2.Tables[1].Rows[index1]["Amount"].ToString() + "<br/>");
                                            stringBuilder3.Append(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")<br/>");
                                        }
                                    }
                                    else
                                    {
                                        int16 = (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index1]["MonthNo"]);
                                        if (stringBuilder4.ToString().Trim() != "")
                                            stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "</td>");
                                        else
                                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                        if (stringBuilder4.ToString().Trim() != "")
                                            stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder3.ToString() + "</td>");
                                        else
                                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                        stringBuilder4 = new StringBuilder();
                                        stringBuilder3 = new StringBuilder();
                                        if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                        {
                                            double num7 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                            num4 += Convert.ToDecimal(num7);
                                            stringBuilder4.Append(dataSet2.Tables[1].Rows[index1]["Amount"].ToString() + "<br/>");
                                            stringBuilder3.Append(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")<br/>");
                                        }
                                        ++num6;
                                    }
                                }
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "</td>");
                                else
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder3.ToString() + "</td>");
                                else
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                int num8 = num6 + 1;
                                while (num8++ < 12)
                                {
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                }
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable3 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str2 != "")
                                {
                                    hashtable2.Add("@AdmnNo", str2);
                                    dataTable3 = obj.GetDataTable("ps_sp_AnnualMontlyFeeDue", hashtable2);
                                }
                                if (num4 > new Decimal(0))
                                {
                                    stringBuilder2.Append("<td align='right' valign='top'>" + num4 + "</td>");
                                    num2 += num4;
                                }
                                else
                                    stringBuilder2.Append("<td align='right' valign='top'>0.00</td>");
                                if (dataTable3.Rows.Count > 0)
                                    stringBuilder2.Append("<td align='right' valign='top'>" + Convert.ToDecimal(dataTable3.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                                else
                                    stringBuilder2.Append("<td align='right' valign='top'>0.00</td>");
                                stringBuilder2.Append("</tr>");
                                if (num4 == new Decimal(0) && (int)Convert.ToInt16(dataSet2.Tables[0].Rows[index2]["Status"].ToString()) == 3)
                                {
                                    stringBuilder2.Length = 0;
                                    stringBuilder2.Capacity = 0;
                                }
                                else
                                {
                                    stringBuilder1.Append(stringBuilder2.ToString() ?? "");
                                    stringBuilder2.Length = 0;
                                    stringBuilder2.Capacity = 0;
                                    ++num3;
                                }
                                num1 = new Decimal(0);
                                ++index2;
                            }
                        }
                        if (index1 < dataSet2.Tables[1].Rows.Count)
                        {
                            while (index1 < dataSet2.Tables[1].Rows.Count)
                            {
                                string str2 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder2.Append("<tr class='tbltd'>");
                                stringBuilder2.Append("<td>" + num3 + "</td>");
                                stringBuilder2.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[1].Rows[index1]["FullName"] + "</td>");
                                stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                int num4 = 0;
                                int num5 = 0;
                                int int16 = int.Parse(dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString());
                                if (int16 > 4)
                                    num4 = int16 - 4;
                                else if (int16 < 4)
                                    num4 = 8 + int16;
                                while (num4-- > 0)
                                {
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                    ++num5;
                                }
                                StringBuilder stringBuilder3 = new StringBuilder();
                                StringBuilder stringBuilder4 = new StringBuilder();
                                for (; index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str2; ++index1)
                                {
                                    if (int16 == (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index1]["MonthNo"]))
                                    {
                                        if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                        {
                                            double num6 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                            num1 += Convert.ToDecimal(num6);
                                            stringBuilder4.Append(dataSet2.Tables[1].Rows[index1]["Amount"].ToString() + "<br/>");
                                            stringBuilder3.Append(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")<br/>");
                                        }
                                    }
                                    else
                                    {
                                        int16 = (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index1]["MonthNo"]);
                                        if (stringBuilder4.ToString().Trim() != "")
                                            stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "</td>");
                                        else
                                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                        if (stringBuilder4.ToString().Trim() != "")
                                            stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder3.ToString() + "</td>");
                                        else
                                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                        stringBuilder4 = new StringBuilder();
                                        stringBuilder3 = new StringBuilder();
                                        if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                        {
                                            double num6 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                            num1 += Convert.ToDecimal(num6);
                                            stringBuilder4.Append(dataSet2.Tables[1].Rows[index1]["Amount"].ToString() + "<br/>");
                                            stringBuilder3.Append(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")<br/>");
                                        }
                                        ++num5;
                                    }
                                }
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "</td>");
                                else
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder3.ToString() + "</td>");
                                else
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                int num7 = num5 + 1;
                                while (num7++ < 12)
                                {
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                }
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str1 != "")
                                {
                                    hashtable2.Add("@AdmnNo", str1);
                                    dataTable2 = obj.GetDataTable("ps_sp_AnnualMontlyFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                {
                                    stringBuilder2.Append("<td align='right' valign='top'>" + num1 + "</td>");
                                    num2 += num1;
                                }
                                else
                                    stringBuilder2.Append("<td align='right' valign='top'>0.00</td>");
                                if (dataTable2.Rows.Count > 0)
                                    stringBuilder2.Append("<td align='right' valign='top'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                                else
                                    stringBuilder2.Append("<td align='right' valign='top'>0.00</td>");
                                num1 = new Decimal(0);
                            }
                        }
                    }
                    else
                    {
                        for (int index = 0; index < dataSet2.Tables[0].Rows.Count; ++index)
                        {
                            stringBuilder2.Append("<tr class='tbltd'>");
                            stringBuilder2.Append("<td>" + num3 + "</td>");
                            stringBuilder2.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[0].Rows[index]["FullName"] + "</td>");
                            stringBuilder2.Append("<td>" + dataSet2.Tables[0].Rows[index]["Amt"] + "</td>");
                            int num4 = 12;
                            while (num4-- > 0)
                            {
                                stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                            }
                            Decimal num5 = Convert.ToDecimal(dataSet2.Tables[0].Rows[index]["Amt"].ToString().Trim());
                            obj = new clsDAL();
                            Hashtable hashtable2 = new Hashtable();
                            DataTable dataTable2 = new DataTable();
                            hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                            string str2 = dataSet2.Tables[0].Rows[index]["admnno"].ToString();
                            if (str2 != "")
                            {
                                hashtable2.Add("@AdmnNo", str2);
                                dataTable2 = obj.GetDataTable("ps_sp_AnnualMontlyFeeDue", hashtable2);
                            }
                            if (num5 > new Decimal(0))
                            {
                                stringBuilder2.Append("<td align='right' valign='top'>" + num5 + "</td>");
                                num2 += num5;
                            }
                            else
                                stringBuilder2.Append("<td align='right' valign='top'>0.00</td>");
                            if (dataTable2.Rows.Count > 0)
                                stringBuilder2.Append("<td align='right' valign='top'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                            else
                                stringBuilder2.Append("<td align='right' valign='top'>0.00</td>");
                            stringBuilder2.Append("</tr>");
                            if (num5 == new Decimal(0) && (int)Convert.ToInt16(dataSet2.Tables[0].Rows[index]["Status"].ToString()) == 3)
                            {
                                stringBuilder2.Length = 0;
                                stringBuilder2.Capacity = 0;
                            }
                            else
                            {
                                stringBuilder1.Append(stringBuilder2.ToString() ?? "");
                                stringBuilder2.Length = 0;
                                stringBuilder2.Capacity = 0;
                                ++num3;
                            }
                            Decimal num6 = new Decimal(0);
                        }
                    }
                }
                else if (dataSet2.Tables[1].Rows.Count > 0)
                {
                    for (int index = 0; index < dataSet2.Tables[1].Rows.Count; index = index - 1 + 1)
                    {
                        int num4 = 0;
                        int num5 = 0;
                        string str2 = dataSet2.Tables[1].Rows[index]["admnno"].ToString();
                        int int16 = int.Parse(dataSet2.Tables[1].Rows[index]["MonthNo"].ToString());
                        stringBuilder2.Append("<tr class='tbltd'>");
                        stringBuilder2.Append("<td>" + num3 + "</td>");
                        stringBuilder2.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[1].Rows[index]["FullName"] + "</td>");
                        stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                        if (int16 > 4)
                            num4 = int16 - 4;
                        else if (int16 < 4)
                            num4 = 8 + int16;
                        while (num4-- > 0)
                        {
                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                            ++num5;
                        }
                        StringBuilder stringBuilder3 = new StringBuilder();
                        StringBuilder stringBuilder4 = new StringBuilder();
                        for (; index < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index]["admnno"].ToString() == str2; ++index)
                        {
                            if (int16 == (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index]["MonthNo"]))
                            {
                                if (dataSet2.Tables[1].Rows[index]["Amount"].ToString() != "--")
                                {
                                    double num6 = double.Parse(dataSet2.Tables[1].Rows[index]["Amount"].ToString());
                                    num1 += Convert.ToDecimal(num6);
                                    stringBuilder4.Append(dataSet2.Tables[1].Rows[index]["Amount"].ToString() + "<br/>");
                                    stringBuilder3.Append(dataSet2.Tables[1].Rows[index]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index]["RecvDate"] + ")<br/>");
                                }
                            }
                            else
                            {
                                int16 = (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index]["MonthNo"]);
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "</td>");
                                else
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder3.ToString() + "</td>");
                                else
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                stringBuilder4 = new StringBuilder();
                                stringBuilder3 = new StringBuilder();
                                if (dataSet2.Tables[1].Rows[index]["Amount"].ToString() != "--")
                                {
                                    double num6 = double.Parse(dataSet2.Tables[1].Rows[index]["Amount"].ToString());
                                    num1 += Convert.ToDecimal(num6);
                                    stringBuilder4.Append(dataSet2.Tables[1].Rows[index]["Amount"].ToString() + "<br/>");
                                    stringBuilder3.Append(dataSet2.Tables[1].Rows[index]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index]["RecvDate"] + ")<br/>");
                                }
                                ++num5;
                            }
                        }
                        if (stringBuilder4.ToString().Trim() != "")
                            stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "</td>");
                        else
                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                        if (stringBuilder4.ToString().Trim() != "")
                            stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder3.ToString() + "</td>");
                        else
                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                        int num7 = num5 + 1;
                        while (num7++ < 12)
                        {
                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                        }
                        obj = new clsDAL();
                        Hashtable hashtable2 = new Hashtable();
                        DataTable dataTable2 = new DataTable();
                        hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                        if (str2 != "")
                        {
                            hashtable2.Add("@AdmnNo", str2);
                            dataTable2 = obj.GetDataTable("ps_sp_AnnualMontlyFeeDue", hashtable2);
                        }
                        if (num1 > new Decimal(0))
                        {
                            stringBuilder2.Append("<td align='right' valign='top'>" + num1 + "</td>");
                            num2 += num1;
                        }
                        else
                            stringBuilder2.Append("<td align='right' valign='top'>0.00</td>");
                        if (dataTable2.Rows.Count > 0)
                            stringBuilder2.Append("<td align='right' valign='top'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                        else
                            stringBuilder2.Append("<td align='right' valign='top'>0.00</td>");
                        stringBuilder2.Append("</tr>");
                        if (num1 == new Decimal(0) && (int)Convert.ToInt16(dataSet2.Tables[0].Rows[index]["Status"].ToString()) == 3)
                        {
                            stringBuilder2.Length = 0;
                            stringBuilder2.Capacity = 0;
                        }
                        else
                        {
                            stringBuilder1.Append(stringBuilder2.ToString() ?? "");
                            stringBuilder2.Length = 0;
                            stringBuilder2.Capacity = 0;
                            ++num3;
                        }
                        num1 = new Decimal(0);
                    }
                }
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td style='text-align:right; width:100px; font-weight:bold;' colspan='27' >Grand Total</td>");
                stringBuilder1.Append("<td style='text-align:right; font-weight:bold;' colspan='2' >" + num2.ToString() + "</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("</table>");
                stringBuilder1.Append("</div>");
                lblreport.Text = stringBuilder1.ToString();
                Session["MCR"] = stringBuilder1.ToString();
                btnExpExcel.Enabled = true;
            }
            else
            {
                lblreport.Text = "No Data Found";
                Session["MCR"] = null;
                btnExpExcel.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
        }
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        if (Session["userrights"].ToString() == "a")
        {
            try
            {
                if (lblreport.Text.ToString().Trim() != "")
                    ExportToExcel((new StringBuilder("").ToString().Trim() + lblreport.Text.ToString().Trim()).ToString().Trim());
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
        string str = Server.MapPath("Exported_Files/MCR-" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptMCRPrint.aspx");
    }
}