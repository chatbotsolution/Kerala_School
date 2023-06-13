using AjaxControlToolkit;
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

public partial class Reports_rptBusMCR : System.Web.UI.Page
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
            lblreport.Text = string.Empty;
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
        Decimal num3 = new Decimal(0);
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
            DataSet dataSet2 = obj.GetDataSet("Ps_Sp_BusFeeMCR", ht);
            if (dataSet2.Tables[0].Rows.Count > 0 || dataSet2.Tables[1].Rows.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<div style='overflow:scroll; overflow-y:hidden; width:921px;' class='lbl2 tbltxt'>");
                stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0' width='200px' style='border-collapse:collapse;'>");
                stringBuilder.Append("<tr class='tbltd'>");
                stringBuilder.Append("<td colspan='28' style='text-align:left; font-weight:bold; font-size:16; padding:5px 0px 15px 0px;'>Monthly Bus Fee Collection Report For the Session : " + drpSession.SelectedValue + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Class : " + drpClass.SelectedItem + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Section : " + ddlSection.SelectedItem + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr class='tbltd'>");
                stringBuilder.Append("<td rowspan='2' style='width:50px; font-weight:bold;' >SlNo</td>");
                stringBuilder.Append("<td rowspan='2' style='font-weight:bold;' >Name</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Apr</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >May</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Jun</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Jul</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Aug</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Sep</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Oct</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Nov</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Dec</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Jan</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Feb</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Mar</td>");
                stringBuilder.Append("<td style='text-align:center; width:300px; font-weight:bold;' colspan='2' >Total</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr class='tbltd'>");
                for (int index = 0; index < 12; ++index)
                {
                    stringBuilder.Append("<td style='width: 100px; text-align:left; font-weight:bold;' >Amount</td>");
                    stringBuilder.Append("<td style='width: 200px; text-align:left; font-weight:bold;' >R.No/Date</td>");
                }
                stringBuilder.Append("<td style='width: 100px; text-align:left; font-weight:bold;' >Received</td>");
                stringBuilder.Append("<td style='width: 200px; text-align:left; font-weight:bold;' >Due</td>");
                stringBuilder.Append("</tr>");
                int num4 = 1;
                if (dataSet2.Tables[0].Rows.Count > 0)
                {
                    if (dataSet2.Tables[1].Rows.Count > 0)
                    {
                        int index1 = 0;
                        int index2 = 0;
                        while (index2 < dataSet2.Tables[0].Rows.Count)
                        {
                            string s = dataSet2.Tables[0].Rows[index2]["admnno"].ToString();
                            if (index1 == dataSet2.Tables[1].Rows.Count || long.Parse(s) < long.Parse(dataSet2.Tables[1].Rows[index1]["admnno"].ToString()))
                            {
                                stringBuilder.Append("<tr class='tbltd'>");
                                stringBuilder.Append("<td>" + num4 + "</td>");
                                stringBuilder.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[0].Rows[index2]["FullName"] + "</td>");
                                int num5 = 12;
                                while (num5-- > 0)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                }
                                num1 = Convert.ToDecimal(double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString()));
                                num2 += num1;
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                string str = dataSet2.Tables[0].Rows[index2]["admnno"].ToString();
                                if (str != "")
                                {
                                    hashtable2.Add("@AdmnNo", str);
                                    dataTable2 = obj.GetDataTable("ps_sp_AnnualMontlyBusFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                    stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                if (dataTable2.Rows.Count > 0)
                                {
                                    stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                                    num3 += Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString().Trim());
                                }
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                ++index2;
                            }
                            else if (long.Parse(s) > long.Parse(dataSet2.Tables[1].Rows[index1]["admnno"].ToString()))
                            {
                                string str = dataSet2.Tables[1].Rows[index1]["admnno"].ToString().Trim();
                                stringBuilder.Append("<tr class='tbltd'>");
                                stringBuilder.Append("<td>" + num4 + "</td>");
                                stringBuilder.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[1].Rows[index1]["FullName"] + "</td>");
                                int num5 = 0;
                                int num6 = 0;
                                int num7 = int.Parse(dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString());
                                if (num7 > 4)
                                    num5 = num7 - 4;
                                else if (num7 < 4)
                                    num5 = 8 + num7;
                                while (num5-- > 0)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    ++num6;
                                }
                                while (index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str)
                                {
                                    if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                    {
                                        int index3 = 0;
                                        double num8 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                        num1 = Convert.ToDecimal(num8);
                                        num2 += num1;
                                        while (index3++ < dt.Rows.Count)
                                        {
                                            if (dt.Rows[index3]["month"].ToString() == dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString())
                                            {
                                                num8 += double.Parse(dt.Rows[index3]["credit"].ToString());
                                                break;
                                            }
                                        }
                                        stringBuilder.Append("<td style='text-align:right;'>" + num8 + "</td>");
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                        stringBuilder.Append(" (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")");
                                    }
                                    else
                                    {
                                        stringBuilder.Append("<td style='text-align:right;'>" + dataSet2.Tables[1].Rows[index1]["Amount"] + "</td>");
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                    }
                                    stringBuilder.Append("</td>");
                                    ++index1;
                                    ++num6;
                                }
                                while (num6++ < 12)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                }
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str != "")
                                {
                                    hashtable2.Add("@AdmnNo", str);
                                    dataTable2 = obj.GetDataTable("ps_sp_AnnualMontlyBusFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                    stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                if (dataTable2.Rows.Count > 0)
                                {
                                    stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                                    num3 += Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString().Trim());
                                }
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                num1 = new Decimal(0);
                            }
                            else
                            {
                                string str = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder.Append("<tr class='tbltd'>");
                                stringBuilder.Append("<td>" + num4 + "</td>");
                                stringBuilder.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[0].Rows[index2]["FullName"] + "</td>");
                                int num5 = 0;
                                int num6 = 0;
                                int num7 = int.Parse(dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString());
                                if (num7 > 4)
                                    num5 = num7 - 4;
                                else if (num7 < 4)
                                    num5 = 8 + num7;
                                while (num5-- > 0)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    ++num6;
                                }
                                while (index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str)
                                {
                                    if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                    {
                                        int index3 = 0;
                                        double num8 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                        num1 += Convert.ToDecimal(num8);
                                        num2 += num1;
                                        for (; index3 < dt.Rows.Count; ++index3)
                                        {
                                            if (dt.Rows[index3]["month"].ToString() == dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString())
                                            {
                                                num8 += double.Parse(dt.Rows[index3]["credit"].ToString());
                                                break;
                                            }
                                        }
                                        stringBuilder.Append("<td style='text-align:right;'>" + num8 + "</td>");
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                        stringBuilder.Append(" (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")");
                                    }
                                    else
                                    {
                                        stringBuilder.Append("<td style='text-align:right;'>" + dataSet2.Tables[1].Rows[index1]["Amount"] + "</td>");
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                    }
                                    stringBuilder.Append("</td>");
                                    ++index1;
                                    ++num6;
                                }
                                while (num6++ < 12)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                }
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str != "")
                                {
                                    hashtable2.Add("@AdmnNo", str);
                                    dataTable2 = obj.GetDataTable("ps_sp_AnnualMontlyBusFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                    stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                if (dataTable2.Rows.Count > 0)
                                {
                                    stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                                    num3 += Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString().Trim());
                                }
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                num1 = new Decimal(0);
                                ++index2;
                            }
                            stringBuilder.Append("</tr>");
                            ++num4;
                        }
                        if (index1 < dataSet2.Tables[1].Rows.Count)
                        {
                            while (index1 < dataSet2.Tables[1].Rows.Count)
                            {
                                string str = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder.Append("<tr class='tbltd'>");
                                stringBuilder.Append("<td>" + num4 + "</td>");
                                stringBuilder.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[1].Rows[index1]["FullName"] + "</td>");
                                int num5 = 0;
                                int num6 = 0;
                                int num7 = int.Parse(dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString());
                                if (num7 > 4)
                                    num5 = num7 - 4;
                                else if (num7 < 4)
                                    num5 = 8 + num7;
                                while (num5-- > 0)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    ++num6;
                                }
                                while (index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str)
                                {
                                    if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                    {
                                        int index3 = 0;
                                        double num8 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                        num1 += Convert.ToDecimal(num8);
                                        num2 += num1;
                                        for (; index3 < dt.Rows.Count; ++index3)
                                        {
                                            if (dt.Rows[index3]["month"].ToString() == dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString())
                                            {
                                                num8 += double.Parse(dt.Rows[index3]["credit"].ToString());
                                                break;
                                            }
                                        }
                                        stringBuilder.Append("<td style='text-align:right;'>" + num8 + "</td>");
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                        stringBuilder.Append(" (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")");
                                    }
                                    else
                                    {
                                        stringBuilder.Append("<td style='text-align:right;'>" + dataSet2.Tables[1].Rows[index1]["Amount"] + "</td>");
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                    }
                                    stringBuilder.Append("</td>");
                                    ++index1;
                                    ++num6;
                                }
                                while (num6++ < 12)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                }
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str != "")
                                {
                                    hashtable2.Add("@AdmnNo", str);
                                    dataTable2 = obj.GetDataTable("ps_sp_AnnualMontlyBusFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                    stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                if (dataTable2.Rows.Count > 0)
                                {
                                    stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                                    num3 += Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString().Trim());
                                }
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                num1 = new Decimal(0);
                            }
                        }
                    }
                    else
                    {
                        for (int index = 0; index < dataSet2.Tables[0].Rows.Count; ++index)
                        {
                            stringBuilder.Append("<tr class='tbltd'>");
                            stringBuilder.Append("<td>" + num4 + "</td>");
                            stringBuilder.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[0].Rows[index]["FullName"] + "</td>");
                            int num5 = 12;
                            while (num5-- > 0)
                            {
                                stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                stringBuilder.Append("<td style='text-align:center;'>--</td>");
                            }
                            double num6 = double.Parse(dataSet2.Tables[1].Rows[index]["Amount"].ToString());
                            num1 += Convert.ToDecimal(num6);
                            num2 += num1;
                            obj = new clsDAL();
                            Hashtable hashtable2 = new Hashtable();
                            DataTable dataTable2 = new DataTable();
                            hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                            string str = dataSet2.Tables[0].Rows[index]["admnno"].ToString();
                            if (str != "")
                            {
                                hashtable2.Add("@AdmnNo", str);
                                dataTable2 = obj.GetDataTable("ps_sp_AnnualMontlyBusFeeDue", hashtable2);
                            }
                            if (num1 > new Decimal(0))
                                stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                            else
                                stringBuilder.Append("<td align='right'>0.00</td>");
                            if (dataTable2.Rows.Count > 0)
                            {
                                stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                                num3 += Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString().Trim());
                            }
                            else
                                stringBuilder.Append("<td align='right'>0.00</td>");
                            stringBuilder.Append("</tr>");
                            ++num4;
                        }
                    }
                }
                else if (dataSet2.Tables[1].Rows.Count > 0)
                {
                    for (int index1 = 0; index1 < dataSet2.Tables[1].Rows.Count; index1 = index1 - 1 + 1)
                    {
                        int num5 = 0;
                        int num6 = 0;
                        string str = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                        int num7 = int.Parse(dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString());
                        stringBuilder.Append("<tr class='tbltd'>");
                        stringBuilder.Append("<td>" + num4 + "</td>");
                        stringBuilder.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[1].Rows[index1]["FullName"] + "</td>");
                        if (num7 > 4)
                            num5 = num7 - 4;
                        else if (num7 < 4)
                            num5 = 8 + num7;
                        while (num5-- > 0)
                        {
                            stringBuilder.Append("<td style='text-align:center;'>--</td>");
                            stringBuilder.Append("<td style='text-align:center;'>--</td>");
                            ++num6;
                        }
                        while (index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str)
                        {
                            if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                            {
                                int index2 = 0;
                                double num8 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                num1 += Convert.ToDecimal(num8);
                                num2 += num1;
                                for (; index2 < dt.Rows.Count; ++index2)
                                {
                                    if (dt.Rows[index2]["month"].ToString() == dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString())
                                    {
                                        num8 += double.Parse(dt.Rows[index2]["credit"].ToString());
                                        break;
                                    }
                                }
                                stringBuilder.Append("<td style='text-align:right;'>" + num8 + "</td>");
                                stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                stringBuilder.Append(" (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")");
                            }
                            else
                            {
                                stringBuilder.Append("<td style='text-align:right;'>" + dataSet2.Tables[1].Rows[index1]["Amount"] + "</td>");
                                stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                            }
                            stringBuilder.Append("</td>");
                            ++index1;
                            ++num6;
                        }
                        while (num6++ < 12)
                        {
                            stringBuilder.Append("<td style='text-align:center;'>--</td>");
                            stringBuilder.Append("<td style='text-align:center;'>--</td>");
                        }
                        obj = new clsDAL();
                        Hashtable hashtable2 = new Hashtable();
                        DataTable dataTable2 = new DataTable();
                        hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                        if (str != "")
                        {
                            hashtable2.Add("@AdmnNo", str);
                            dataTable2 = obj.GetDataTable("ps_sp_AnnualMontlyBusFeeDue", hashtable2);
                        }
                        if (num1 > new Decimal(0))
                            stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                        else
                            stringBuilder.Append("<td align='right'>0.00</td>");
                        if (dataTable2.Rows.Count > 0)
                        {
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                            num3 += Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString().Trim());
                        }
                        else
                            stringBuilder.Append("<td style='text-align:center;'>0.00</td>");
                        stringBuilder.Append("</tr>");
                        num1 = new Decimal(0);
                        stringBuilder.Append("</tr>");
                        ++num4;
                    }
                }
                stringBuilder.Append("<tr class='tbltd'>");
                stringBuilder.Append("<td align='right' style='font-weight:bold;' colspan='2'>Total :</td>");
                foreach (DataRow row in (InternalDataCollectionBase)dataSet2.Tables[2].Rows)
                {
                    stringBuilder.Append("<td align='right' style='font-weight:bold;'>");
                    stringBuilder.Append(row["TotAmount"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td>&nbsp;</td>");
                }
                stringBuilder.Append("<td align='right' style='font-weight:bold;'>" + num2.ToString("0.00") + "</td>");
                stringBuilder.Append("<td align='right' style='font-weight:bold;'>" + num3.ToString("0.00") + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("</div>");
                lblreport.Text = stringBuilder.ToString();
                Session["BusMCR"] = stringBuilder.ToString();
            }
            else
            {
                lblreport.Text = "No Data Found";
                Session["BusMCR"] = null;
            }
        }
        catch (Exception ex)
        {
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
        string str = Server.MapPath("Exported_Files/BusMCR-" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
        Response.Redirect("rptBusMCRPrint.aspx");
    }
}
