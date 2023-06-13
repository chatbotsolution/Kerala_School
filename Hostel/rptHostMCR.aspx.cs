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

public partial class Hostel_rptHostMCR : System.Web.UI.Page
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
            DataSet dataSet2 = obj.GetDataSet("HostRptAnnualMontlyFee", ht);
            if (dataSet2.Tables[0].Rows.Count > 0 || dataSet2.Tables[1].Rows.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<div style='overflow:scroll; overflow-y:hidden; width:1000px;'>");
                stringBuilder.Append("<table cellpadding='1' cellspacing='1' border='1' style='border-collapse:collapse'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='29' style='text-align:left; font-weight:bold; font-size:16; padding:5px 0px 15px 0px;'>Monthly Collection Report For the Session : " + drpSession.SelectedValue + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Class : " + drpClass.SelectedItem + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Section : " + ddlSection.SelectedItem + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td rowspan='2' style='width:50px; font-weight:bold;' >SlNo</td>");
                stringBuilder.Append("<td rowspan='2' style='font-weight:bold;' >Name</td>");
                stringBuilder.Append("<td style='width:100px; font-weight:bold;' rowspan='2' >Annual Adm/Re-Adm</td>");
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
                stringBuilder.Append("<tr>");
                for (int index = 0; index < 12; ++index)
                {
                    stringBuilder.Append("<td style='width: 100px; text-align:left; font-weight:bold;' >Amount</td>");
                    stringBuilder.Append("<td style='width: 200px; text-align:left; font-weight:bold;' >R.No/Date</td>");
                }
                stringBuilder.Append("<td style='width: 100px; text-align:left; font-weight:bold;' >Received</td>");
                stringBuilder.Append("<td style='width: 200px; text-align:left; font-weight:bold;' >Due</td>");
                stringBuilder.Append("</tr>");
                int num2 = 1;
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
                                stringBuilder.Append("<td>" + num2 + "</td>");
                                stringBuilder.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[0].Rows[index2]["FullName"] + "</td>");
                                stringBuilder.Append("<td>" + dataSet2.Tables[0].Rows[index2]["Amt"] + "</td>");
                                int num3 = 12;
                                while (num3-- > 0)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                }
                                num1 = Convert.ToDecimal(dataSet2.Tables[0].Rows[index2]["Amt"].ToString().Trim());
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (s != "")
                                {
                                    hashtable2.Add("@AdmnNo", s);
                                    dataTable2 = obj.GetDataTable("HostRptAnnualMontlyFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                    stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                if (dataTable2.Rows.Count > 0)
                                    stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                ++index2;
                            }
                            else if (long.Parse(s) > long.Parse(dataSet2.Tables[1].Rows[index1]["admnno"].ToString()))
                            {
                                string str2 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder.Append("<tr class='tbltd'>");
                                stringBuilder.Append("<td>" + num2 + "</td>");
                                stringBuilder.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[1].Rows[index1]["FullName"] + "</td>");
                                stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                if (dataSet2.Tables[1].Rows[index1]["admnno"].ToString().Trim() == dataSet2.Tables[0].Rows[index2]["admnno"].ToString().Trim())
                                    num1 = Convert.ToDecimal(dataSet2.Tables[0].Rows[index2]["Amt"].ToString().Trim());
                                int num3 = 0;
                                int num4 = 0;
                                int num5 = int.Parse(dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString());
                                if (num5 > 4)
                                    num3 = num5 - 4;
                                else if (num5 < 4)
                                    num3 = 8 + num5;
                                while (num3-- > 0)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    ++num4;
                                }
                                while (index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str2)
                                {
                                    if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                    {
                                        double num6 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                        num1 += Convert.ToDecimal(num6);
                                        stringBuilder.Append("<td style='text-align:center;'>" + num6 + "</td>");
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                        stringBuilder.Append(" (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")");
                                    }
                                    else
                                    {
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Amount"] + "</td>");
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                    }
                                    stringBuilder.Append("</td>");
                                    ++index1;
                                    ++num4;
                                }
                                while (num4++ < 12)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                }
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str2 != "")
                                {
                                    hashtable2.Add("@AdmnNo", str2);
                                    dataTable2 = obj.GetDataTable("HostRptAnnualMontlyFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                    stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                if (dataTable2.Rows.Count > 0)
                                    stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                num1 = new Decimal(0);
                            }
                            else
                            {
                                string str2 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder.Append("<tr class='tbltd'>");
                                stringBuilder.Append("<td>" + num2 + "</td>");
                                stringBuilder.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[0].Rows[index2]["FullName"] + "</td>");
                                stringBuilder.Append("<td>" + dataSet2.Tables[0].Rows[index2]["Amt"] + "</td>");
                                Decimal num3 = Convert.ToDecimal(dataSet2.Tables[0].Rows[index2]["Amt"].ToString().Trim());
                                int num4 = 0;
                                int num5 = 0;
                                int num6 = int.Parse(dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString());
                                if (num6 > 4)
                                    num4 = num6 - 4;
                                else if (num6 < 4)
                                    num4 = 8 + num6;
                                while (num4-- > 0)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    ++num5;
                                }
                                while (index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str2)
                                {
                                    if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                    {
                                        double num7 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                        num3 += Convert.ToDecimal(num7);
                                        stringBuilder.Append("<td style='text-align:center;'>" + num7 + "</td>");
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                        stringBuilder.Append(" (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")");
                                    }
                                    else
                                    {
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Amount"] + "</td>");
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                    }
                                    stringBuilder.Append("</td>");
                                    ++index1;
                                    ++num5;
                                }
                                while (num5++ < 12)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                }
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str2 != "")
                                {
                                    hashtable2.Add("@AdmnNo", str2);
                                    dataTable2 = obj.GetDataTable("HostRptAnnualMontlyFeeDue", hashtable2);
                                }
                                if (num3 > new Decimal(0))
                                    stringBuilder.Append("<td align='right'>" + num3 + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                if (dataTable2.Rows.Count > 0)
                                    stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                num1 = new Decimal(0);
                                ++index2;
                            }
                            stringBuilder.Append("</tr>");
                            ++num2;
                        }
                        if (index1 < dataSet2.Tables[1].Rows.Count)
                        {
                            while (index1 < dataSet2.Tables[1].Rows.Count)
                            {
                                string str2 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder.Append("<tr class='tbltd'>");
                                stringBuilder.Append("<td>" + num2 + "</td>");
                                stringBuilder.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[1].Rows[index1]["FullName"] + "</td>");
                                stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                int num3 = 0;
                                int num4 = 0;
                                int num5 = int.Parse(dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString());
                                if (num5 > 4)
                                    num3 = num5 - 4;
                                else if (num5 < 4)
                                    num3 = 8 + num5;
                                while (num3-- > 0)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    ++num4;
                                }
                                while (index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str2)
                                {
                                    if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                    {
                                        double num6 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                        num1 += Convert.ToDecimal(num6);
                                        stringBuilder.Append("<td style='text-align:center;'>" + num6 + "</td>");
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                        stringBuilder.Append(" (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")");
                                    }
                                    else
                                    {
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Amount"] + "</td>");
                                        stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]);
                                    }
                                    stringBuilder.Append("</td>");
                                    ++index1;
                                    ++num4;
                                }
                                while (num4++ < 12)
                                {
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
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
                                    stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                if (dataTable2.Rows.Count > 0)
                                    stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
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
                            stringBuilder.Append("<td>" + num2 + "</td>");
                            stringBuilder.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[0].Rows[index]["FullName"] + "</td>");
                            stringBuilder.Append("<td>" + dataSet2.Tables[0].Rows[index]["Amt"] + "</td>");
                            int num3 = 12;
                            while (num3-- > 0)
                            {
                                stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                stringBuilder.Append("<td style='text-align:center;'>--</td>");
                            }
                            Decimal num4 = Convert.ToDecimal(dataSet2.Tables[0].Rows[index]["Amt"].ToString().Trim());
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
                            if (num4 > new Decimal(0))
                                stringBuilder.Append("<td align='right'>" + num4 + "</td>");
                            else
                                stringBuilder.Append("<td align='right'>0.00</td>");
                            if (dataTable2.Rows.Count > 0)
                                stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                            else
                                stringBuilder.Append("<td align='right'>0.00</td>");
                            stringBuilder.Append("</tr>");
                            ++num2;
                            Decimal num5 = new Decimal(0);
                        }
                    }
                }
                else if (dataSet2.Tables[1].Rows.Count > 0)
                {
                    for (int index = 0; index < dataSet2.Tables[1].Rows.Count; index = index - 1 + 1)
                    {
                        int num3 = 0;
                        int num4 = 0;
                        string str2 = dataSet2.Tables[1].Rows[index]["admnno"].ToString();
                        int num5 = int.Parse(dataSet2.Tables[1].Rows[index]["MonthNo"].ToString());
                        stringBuilder.Append("<tr class='tbltd'>");
                        stringBuilder.Append("<td>" + num2 + "</td>");
                        stringBuilder.Append("<td style='width:300px; white-space:nowrap'>" + dataSet2.Tables[1].Rows[index]["FullName"] + "</td>");
                        stringBuilder.Append("<td style='text-align:center;'>--</td>");
                        if (num5 > 4)
                            num3 = num5 - 4;
                        else if (num5 < 4)
                            num3 = 8 + num5;
                        while (num3-- > 0)
                        {
                            stringBuilder.Append("<td style='text-align:center;'>--</td>");
                            stringBuilder.Append("<td style='text-align:center;'>--</td>");
                            ++num4;
                        }
                        while (index < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index]["admnno"].ToString() == str2)
                        {
                            if (dataSet2.Tables[1].Rows[index]["Amount"].ToString() != "--")
                            {
                                double num6 = double.Parse(dataSet2.Tables[1].Rows[index]["Amount"].ToString());
                                num1 += Convert.ToDecimal(num6);
                                stringBuilder.Append("<td style='text-align:center;'>" + num6 + "</td>");
                                stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index]["Receipt_VrNo"]);
                                stringBuilder.Append(" (" + dataSet2.Tables[1].Rows[index]["RecvDate"] + ")");
                            }
                            else
                            {
                                stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index]["Amount"] + "</td>");
                                stringBuilder.Append("<td style='text-align:center;'>" + dataSet2.Tables[1].Rows[index]["Receipt_VrNo"]);
                            }
                            stringBuilder.Append("</td>");
                            ++index;
                            ++num4;
                        }
                        while (num4++ < 12)
                        {
                            stringBuilder.Append("<td style='text-align:center;'>--</td>");
                            stringBuilder.Append("<td style='text-align:center;'>--</td>");
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
                            stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                        else
                            stringBuilder.Append("<td align='right'>0.00</td>");
                        if (dataTable2.Rows.Count > 0)
                            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(dataTable2.Rows[0]["Due"].ToString()).ToString("0.00") + "</td>");
                        else
                            stringBuilder.Append("<td align='right'>0.00</td>");
                        stringBuilder.Append("</tr>");
                        num1 = new Decimal(0);
                        ++num2;
                    }
                }
                stringBuilder.Append("</table>");
                stringBuilder.Append("</div>");
                lblreport.Text = stringBuilder.ToString();
                Session["MCR"] = stringBuilder.ToString();
                btnExpExcel.Enabled = true;
                btnPrint.Enabled = true;
            }
            else
            {
                lblreport.Text = "No Data Found";
                Session["MCR"] = null;
                btnExpExcel.Enabled = false;
                btnPrint.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
        }
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
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

    private void ExportToExcel(string dataToExport)
    {
        string str = Server.MapPath("Exported_Files/HostelMCR-" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
        Response.Redirect("rptHostMCRPrint.aspx");
    }
}