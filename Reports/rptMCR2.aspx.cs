using AjaxControlToolkit;
using ASP;
using Classes.DA;
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

public partial class Reports_rptMCR2 : System.Web.UI.Page
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
        Decimal num2 = new Decimal(0);
        string str1 = "";
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
                string str2 = Information();
                StringBuilder stringBuilder1 = new StringBuilder("");
                stringBuilder1.Append("<div style='overflow:scroll; overflow-y:hidden; width:921px;' class='tbltxt lbl2'>");
                stringBuilder1.Append("<table cellpadding='1' cellspacing='1' border='1' style='border-collapse:collapse'>");
                stringBuilder1.Append("<tr><td colspan='16' style='border-right:dotted 1px black;text-align:center;'>" + str2 + "</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td colspan='16' style='text-align:left; font-weight:bold; font-size:16; padding:5px 0px 15px 0px;'>Monthly Collection Report For the Session : " + drpSession.SelectedValue + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Class : " + drpClass.SelectedItem + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Section : " + ddlSection.SelectedItem + "</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td rowspan='2' style='width:25px; font-weight:bold;' >SN</td>");
                stringBuilder1.Append("<td rowspan='2'style='width:60px; style='font-weight:bold;' >Name</td>");
                stringBuilder1.Append("<td style='width:100px; font-weight:bold;' rowspan='2' >Adm/Re-Adm</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Apr</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >May</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Jun</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Jul</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Aug</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Sep</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Oct</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Nov</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Dec</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Jan</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Feb</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Mar</td>");
                stringBuilder1.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1'  rowspan='2' >Total</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("<tr>");
                for (int index = 0; index < 12; ++index)
                {
                    stringBuilder1.Append("<td style='width: 100px; text-align:left; font-weight:bold;' rowspan='1'>Amount<br/>");
                    stringBuilder1.Append("R.No/Date</td>");
                }
                stringBuilder1.Append("</tr>");
                int num3 = 1;
                StringBuilder stringBuilder2 = new StringBuilder("");
                Decimal num4;
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
                                stringBuilder2.Append("<td style='width:60px;'>" + dataSet2.Tables[0].Rows[index2]["FullName"] + "</td>");
                                stringBuilder2.Append("<td>" + dataSet2.Tables[0].Rows[index2]["Amt"] + "</td>");
                                int num5 = 12;
                                while (num5-- > 0)
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                num1 = Convert.ToDecimal(dataSet2.Tables[0].Rows[index2]["Amt"].ToString().Trim());
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable3 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (s != "")
                                {
                                    hashtable2.Add("@AdmnNo", s);
                                    obj.GetDataTable("ps_sp_AnnualMontlyFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                {
                                    stringBuilder2.Append("<td align='right'>" + num1 + "</td>");
                                    num2 += num1;
                                }
                                else
                                    stringBuilder2.Append("<td align='right'>0.00</td>");
                                ++index2;
                            }
                            else if (long.Parse(s) > long.Parse(dataSet2.Tables[1].Rows[index1]["admnno"].ToString()))
                            {
                                string str3 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder2.Append("<tr class='tbltd'>");
                                stringBuilder2.Append("<td>" + num3 + "</td>");
                                stringBuilder2.Append("<td style='width:60px;'>" + dataSet2.Tables[1].Rows[index1]["FullName"] + "</td>");
                                stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                if (dataSet2.Tables[1].Rows[index1]["admnno"].ToString().Trim() == dataSet2.Tables[0].Rows[index2]["admnno"].ToString().Trim())
                                    num1 = Convert.ToDecimal(dataSet2.Tables[0].Rows[index2]["Amt"].ToString().Trim());
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
                                    ++num6;
                                }
                                StringBuilder stringBuilder3 = new StringBuilder();
                                StringBuilder stringBuilder4 = new StringBuilder();
                                for (; index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str3; ++index1)
                                {
                                    if (int16 == (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index1]["MonthNo"]))
                                    {
                                        if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                        {
                                            double num7 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                            num1 += Convert.ToDecimal(num7);
                                            stringBuilder4.Append(dataSet2.Tables[1].Rows[index1]["Amount"].ToString() + "<br/>");
                                            stringBuilder3.Append(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")<br/>");
                                        }
                                    }
                                    else
                                    {
                                        int16 = (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index1]["MonthNo"]);
                                        if (stringBuilder4.ToString().Trim() != "")
                                            stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "<br/>");
                                        else
                                            stringBuilder2.Append("<td style='text-align:center;'>--<br/>");
                                        if (stringBuilder4.ToString().Trim() != "")
                                            stringBuilder2.Append(stringBuilder3.ToString() + "</td>");
                                        else
                                            stringBuilder2.Append("--</td>");
                                        stringBuilder4 = new StringBuilder();
                                        stringBuilder3 = new StringBuilder();
                                        if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                        {
                                            double num7 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                            num1 += Convert.ToDecimal(num7);
                                            stringBuilder4.Append(dataSet2.Tables[1].Rows[index1]["Amount"].ToString() + "<br/>");
                                            stringBuilder3.Append(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")<br/>");
                                        }
                                        ++num6;
                                    }
                                }
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "<br/>");
                                else
                                    stringBuilder2.Append("<td style='text-align:center;'>--<br/>");
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append(stringBuilder3.ToString() + "</td>");
                                else
                                    stringBuilder2.Append("--</td>");
                                int num8 = num6 + 1;
                                while (num8++ < 12)
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable3 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str3 != "")
                                {
                                    hashtable2.Add("@AdmnNo", str3);
                                    obj.GetDataTable("ps_sp_AnnualMontlyFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                {
                                    stringBuilder2.Append("<td align='right' valign='top'>" + num1 + "</td>");
                                    num2 += num1;
                                }
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
                                num4 = new Decimal(0);
                                num1 = new Decimal(0);
                            }
                            else
                            {
                                string str3 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder2.Append("<tr class='tbltd'>");
                                stringBuilder2.Append("<td>" + num3 + "</td>");
                                stringBuilder2.Append("<td style='width:60px;'>" + dataSet2.Tables[0].Rows[index2]["FullName"] + "</td>");
                                stringBuilder2.Append("<td valign='top' >" + dataSet2.Tables[0].Rows[index2]["Amt"] + "</td>");
                                Decimal num5 = Convert.ToDecimal(dataSet2.Tables[0].Rows[index2]["Amt"].ToString().Trim());
                                int num6 = 0;
                                int num7 = 0;
                                int int16 = int.Parse(dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString());
                                if (int16 > 4)
                                    num6 = int16 - 4;
                                else if (int16 < 4)
                                    num6 = 8 + int16;
                                while (num6-- > 0)
                                {
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                    ++num7;
                                }
                                StringBuilder stringBuilder3 = new StringBuilder();
                                StringBuilder stringBuilder4 = new StringBuilder();
                                for (; index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str3; ++index1)
                                {
                                    if (int16 == (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index1]["MonthNo"]))
                                    {
                                        if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                        {
                                            double num8 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                            num5 += Convert.ToDecimal(num8);
                                            stringBuilder4.Append(dataSet2.Tables[1].Rows[index1]["Amount"].ToString() + "<br/>");
                                            stringBuilder3.Append(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")<br/>");
                                        }
                                    }
                                    else
                                    {
                                        int16 = (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index1]["MonthNo"]);
                                        if (stringBuilder4.ToString().Trim() != "")
                                            stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "<br/>");
                                        else
                                            stringBuilder2.Append("<td style='text-align:center;'>--<br/>");
                                        if (stringBuilder4.ToString().Trim() != "")
                                            stringBuilder2.Append(stringBuilder3.ToString() + "</td>");
                                        else
                                            stringBuilder2.Append("--</td>");
                                        stringBuilder4 = new StringBuilder();
                                        stringBuilder3 = new StringBuilder();
                                        if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                        {
                                            double num8 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                            num5 += Convert.ToDecimal(num8);
                                            stringBuilder4.Append(dataSet2.Tables[1].Rows[index1]["Amount"].ToString() + "<br/>");
                                            stringBuilder3.Append(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")<br/>");
                                        }
                                        ++num7;
                                    }
                                }
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "<br/>");
                                else
                                    stringBuilder2.Append("<td style='text-align:center;'>--<br/>");
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append(stringBuilder3.ToString() + "</td>");
                                else
                                    stringBuilder2.Append("--</td>");
                                int num9 = num7 + 1;
                                while (num9++ < 12)
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable3 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str3 != "")
                                {
                                    hashtable2.Add("@AdmnNo", str3);
                                    obj.GetDataTable("ps_sp_AnnualMontlyFeeDue", hashtable2);
                                }
                                if (num5 > new Decimal(0))
                                {
                                    stringBuilder2.Append("<td align='right' valign='top'>" + num5 + "</td>");
                                    num2 += num5;
                                }
                                else
                                    stringBuilder2.Append("<td align='right' valign='top'>0.00</td>");
                                stringBuilder2.Append("</tr>");
                                if (num5 == new Decimal(0) && (int)Convert.ToInt16(dataSet2.Tables[0].Rows[index2]["Status"].ToString()) == 3)
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
                                string str3 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder2.Append("<tr class='tbltd'>");
                                stringBuilder2.Append("<td>" + num3 + "</td>");
                                stringBuilder2.Append("<td style='width:60px;'>" + dataSet2.Tables[1].Rows[index1]["FullName"] + "</td>");
                                stringBuilder2.Append("<td style='text-align:center;'>--</td>");
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
                                    ++num6;
                                }
                                StringBuilder stringBuilder3 = new StringBuilder();
                                StringBuilder stringBuilder4 = new StringBuilder();
                                for (; index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str3; ++index1)
                                {
                                    if (int16 == (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index1]["MonthNo"]))
                                    {
                                        if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                        {
                                            double num7 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                            num1 += Convert.ToDecimal(num7);
                                            stringBuilder4.Append(dataSet2.Tables[1].Rows[index1]["Amount"].ToString() + "<br/>");
                                            stringBuilder3.Append(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")<br/>");
                                        }
                                    }
                                    else
                                    {
                                        int16 = (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index1]["MonthNo"]);
                                        if (stringBuilder4.ToString().Trim() != "")
                                            stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "<br/>");
                                        else
                                            stringBuilder2.Append("<td style='text-align:center;'>--<br/>");
                                        if (stringBuilder4.ToString().Trim() != "")
                                            stringBuilder2.Append(stringBuilder3.ToString() + "</td>");
                                        else
                                            stringBuilder2.Append("--</td>");
                                        stringBuilder4 = new StringBuilder();
                                        stringBuilder3 = new StringBuilder();
                                        if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                        {
                                            double num7 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                            num1 += Convert.ToDecimal(num7);
                                            stringBuilder4.Append(dataSet2.Tables[1].Rows[index1]["Amount"].ToString() + "<br/>");
                                            stringBuilder3.Append(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")<br/>");
                                        }
                                        ++num6;
                                    }
                                }
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "<br/>");
                                else
                                    stringBuilder2.Append("<td style='text-align:center;'>--<br/>");
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append(stringBuilder3.ToString() + "</td>");
                                else
                                    stringBuilder2.Append("--</td>");
                                int num8 = num6 + 1;
                                while (num8++ < 12)
                                    stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str1 != "")
                                {
                                    hashtable2.Add("@AdmnNo", str1);
                                    obj.GetDataTable("ps_sp_AnnualMontlyFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                {
                                    stringBuilder2.Append("<td align='right' valign='top'>" + num1 + "</td>");
                                    num2 += num1;
                                }
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
                            stringBuilder2.Append("<td style='width:60px;'>" + dataSet2.Tables[0].Rows[index]["FullName"] + "</td>");
                            stringBuilder2.Append("<td>" + dataSet2.Tables[0].Rows[index]["Amt"] + "</td>");
                            int num5 = 12;
                            while (num5-- > 0)
                                stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                            Decimal num6 = Convert.ToDecimal(dataSet2.Tables[0].Rows[index]["Amt"].ToString().Trim());
                            obj = new clsDAL();
                            Hashtable hashtable2 = new Hashtable();
                            DataTable dataTable2 = new DataTable();
                            hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                            string str3 = dataSet2.Tables[0].Rows[index]["admnno"].ToString();
                            if (str3 != "")
                            {
                                hashtable2.Add("@AdmnNo", str3);
                                obj.GetDataTable("ps_sp_AnnualMontlyFeeDue", hashtable2);
                            }
                            if (num6 > new Decimal(0))
                            {
                                stringBuilder2.Append("<td align='right' valign='top'>" + num6 + "</td>");
                                num2 += num6;
                            }
                            else
                                stringBuilder2.Append("<td align='right' valign='top'>0.00</td>");
                            stringBuilder2.Append("</tr>");
                            if (num6 == new Decimal(0) && (int)Convert.ToInt16(dataSet2.Tables[0].Rows[index]["Status"].ToString()) == 3)
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
                            num4 = new Decimal(0);
                        }
                    }
                }
                else if (dataSet2.Tables[1].Rows.Count > 0)
                {
                    for (int index = 0; index < dataSet2.Tables[1].Rows.Count; index = index - 1 + 1)
                    {
                        int num5 = 0;
                        int num6 = 0;
                        string str3 = dataSet2.Tables[1].Rows[index]["admnno"].ToString();
                        int int16 = int.Parse(dataSet2.Tables[1].Rows[index]["MonthNo"].ToString());
                        stringBuilder2.Append("<tr class='tbltd'>");
                        stringBuilder2.Append("<td>" + num3 + "</td>");
                        stringBuilder2.Append("<td style='width:60px;'>" + dataSet2.Tables[1].Rows[index]["FullName"] + "</td>");
                        stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                        if (int16 > 4)
                            num5 = int16 - 4;
                        else if (int16 < 4)
                            num5 = 8 + int16;
                        while (num5-- > 0)
                        {
                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                            ++num6;
                        }
                        StringBuilder stringBuilder3 = new StringBuilder();
                        StringBuilder stringBuilder4 = new StringBuilder();
                        for (; index < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index]["admnno"].ToString() == str3; ++index)
                        {
                            if (int16 == (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index]["MonthNo"]))
                            {
                                if (dataSet2.Tables[1].Rows[index]["Amount"].ToString() != "--")
                                {
                                    double num7 = double.Parse(dataSet2.Tables[1].Rows[index]["Amount"].ToString());
                                    num1 += Convert.ToDecimal(num7);
                                    stringBuilder4.Append(dataSet2.Tables[1].Rows[index]["Amount"].ToString() + "<br/>");
                                    stringBuilder3.Append(dataSet2.Tables[1].Rows[index]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index]["RecvDate"] + ")<br/>");
                                }
                            }
                            else
                            {
                                int16 = (int)Convert.ToInt16(dataSet2.Tables[1].Rows[index]["MonthNo"]);
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "<br/>");
                                else
                                    stringBuilder2.Append("<td style='text-align:center;'>--<br/>");
                                if (stringBuilder4.ToString().Trim() != "")
                                    stringBuilder2.Append(stringBuilder3.ToString() + "</td>");
                                else
                                    stringBuilder2.Append("--</td>");
                                stringBuilder4 = new StringBuilder();
                                stringBuilder3 = new StringBuilder();
                                if (dataSet2.Tables[1].Rows[index]["Amount"].ToString() != "--")
                                {
                                    double num7 = double.Parse(dataSet2.Tables[1].Rows[index]["Amount"].ToString());
                                    num1 += Convert.ToDecimal(num7);
                                    stringBuilder4.Append(dataSet2.Tables[1].Rows[index]["Amount"].ToString() + "<br/>");
                                    stringBuilder3.Append(dataSet2.Tables[1].Rows[index]["Receipt_VrNo"].ToString() + " (" + dataSet2.Tables[1].Rows[index]["RecvDate"] + ")<br/>");
                                }
                                ++num6;
                            }
                        }
                        if (stringBuilder4.ToString().Trim() != "")
                            stringBuilder2.Append("<td style='text-align:center;' valign='top'>" + stringBuilder4.ToString() + "<br/>");
                        else
                            stringBuilder2.Append("<td style='text-align:center;'>--<br/>");
                        if (stringBuilder4.ToString().Trim() != "")
                            stringBuilder2.Append(stringBuilder3.ToString() + "</td>");
                        else
                            stringBuilder2.Append("--</td>");
                        int num8 = num6 + 1;
                        while (num8++ < 12)
                            stringBuilder2.Append("<td style='text-align:center;'>--</td>");
                        obj = new clsDAL();
                        Hashtable hashtable2 = new Hashtable();
                        DataTable dataTable2 = new DataTable();
                        hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                        if (str3 != "")
                        {
                            hashtable2.Add("@AdmnNo", str3);
                            obj.GetDataTable("ps_sp_AnnualMontlyFeeDue", hashtable2);
                        }
                        if (num1 > new Decimal(0))
                        {
                            stringBuilder2.Append("<td align='right' valign='top'>" + num1 + "</td>");
                            num2 += num1;
                        }
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
                stringBuilder1.Append("<td style='text-align:right; width:100px; font-weight:bold;' colspan='14' >Grand Total</td>");
                stringBuilder1.Append("<td style='text-align:right; font-weight:bold;' colspan='2' >" + num2.ToString() + "</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("</table>");
                stringBuilder1.Append("</div>");
                lblreport.Text = stringBuilder1.ToString();
                Session["MCR2"] = stringBuilder1.ToString();
                btnExpExcel.Enabled = true;
            }
            else
            {
                lblreport.Text = "No Data Found";
                Session["MCR2"] = null;
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
        Response.Redirect("rptMCRPrint2.aspx");
    }

    private string Information()
    {
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        DataSet dataSet = new DataSet();
        StringBuilder stringBuilder = new StringBuilder();
        clsDAL clsDal = new clsDAL();
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
                    stringBuilder.Append("<table width='100%' cellpadding='2' cellspacing='2' class='tbltd'><tr><td rowspan='4'> </td>");
                    stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</b></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + clsDal.Decrypt(row["Pin"].ToString().Trim(), str2) + "</b> </td></tr>");
                    stringBuilder.Append("</table>");
                }
            }
        }
        return stringBuilder.ToString();
    }
}