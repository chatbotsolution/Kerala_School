using AjaxControlToolkit;
using ASP;
using Classes.DA;
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

public partial class Reports_rptBusMCR2 : System.Web.UI.Page
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
                string str1 = Information();
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<div style='overflow:scroll; overflow-y:hidden; width:921px;' class='lbl2 tbltxt cnt-box2'>");
                stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0' width='200px' style='border-collapse:collapse;'>");
                stringBuilder.Append("<tr><td colspan='15' style='border-right:dotted 1px black;text-align:center;'>" + str1 + "</td>");
                stringBuilder.Append("<tr class='tbltd'>");
                stringBuilder.Append("<td colspan='15' style='text-align:left; font-weight:bold; font-size:16; padding:5px 0px 15px 0px;'>Monthly Bus Fee Collection Report For the Session : " + drpSession.SelectedValue + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Class : " + drpClass.SelectedItem + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Section : " + ddlSection.SelectedItem + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr class='tbltd'>");
                stringBuilder.Append("<td rowspan='2' style='width:25px; font-weight:bold;' >SN</td>");
                stringBuilder.Append("<td rowspan='2' style='width:60px;font-weight:bold;' >Name</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Apr</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >May</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Jun</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Jul</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Aug</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Sep</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Oct</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Nov</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Dec</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Jan</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Feb</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' >Mar</td>");
                stringBuilder.Append("<td style='text-align:center; width:100px; font-weight:bold;' colspan='1' rowspan='2'>Total</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr class='tbltd'>");
                for (int index = 0; index < 12; ++index)
                {
                    stringBuilder.Append("<td style='width: 100px; text-align:left; font-weight:bold;' rowspan='1'>Amount<br/>");
                    stringBuilder.Append("R.No/Date</td>");
                }
                stringBuilder.Append("</tr>");
                int num3 = 1;
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
                                stringBuilder.Append("<td>" + num3 + "</td>");
                                stringBuilder.Append("<td style='width:60px;'>" + dataSet2.Tables[0].Rows[index2]["FullName"] + "</td>");
                                int num4 = 12;
                                while (num4-- > 0)
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                num1 = Convert.ToDecimal(double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString()));
                                num2 += num1;
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                string str2 = dataSet2.Tables[0].Rows[index2]["admnno"].ToString();
                                if (str2 != "")
                                {
                                    hashtable2.Add("@AdmnNo", str2);
                                    obj.GetDataTable("ps_sp_AnnualMontlyBusFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                    stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                ++index2;
                            }
                            else if (long.Parse(s) > long.Parse(dataSet2.Tables[1].Rows[index1]["admnno"].ToString()))
                            {
                                string str2 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString().Trim();
                                stringBuilder.Append("<tr class='tbltd'>");
                                stringBuilder.Append("<td>" + num3 + "</td>");
                                stringBuilder.Append("<td style='width:60px;'>" + dataSet2.Tables[1].Rows[index1]["FullName"] + "</td>");
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
                                    ++num5;
                                }
                                while (index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str2)
                                {
                                    if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                    {
                                        int index3 = 0;
                                        double num7 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                        num1 = Convert.ToDecimal(num7);
                                        num2 += num1;
                                        while (index3++ < dt.Rows.Count)
                                        {
                                            if (dt.Rows[index3]["month"].ToString() == dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString())
                                            {
                                                num7 += double.Parse(dt.Rows[index3]["credit"].ToString());
                                                break;
                                            }
                                        }
                                        stringBuilder.Append("<td style='text-align:right;'>" + num7 + "<br/>");
                                        stringBuilder.Append(string.Concat(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]));
                                        stringBuilder.Append(" (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")");
                                    }
                                    else
                                    {
                                        stringBuilder.Append("<td style='text-align:right;'>" + dataSet2.Tables[1].Rows[index1]["Amount"] + "<br/>");
                                        stringBuilder.Append(string.Concat(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]));
                                    }
                                    stringBuilder.Append("</td>");
                                    ++index1;
                                    ++num5;
                                }
                                while (num5++ < 12)
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str2 != "")
                                {
                                    hashtable2.Add("@AdmnNo", str2);
                                    obj.GetDataTable("ps_sp_AnnualMontlyBusFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                    stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                num1 = new Decimal(0);
                            }
                            else
                            {
                                string str2 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder.Append("<tr class='tbltd'>");
                                stringBuilder.Append("<td>" + num3 + "</td>");
                                stringBuilder.Append("<td style='width:60px;'>" + dataSet2.Tables[0].Rows[index2]["FullName"] + "</td>");
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
                                    ++num5;
                                }
                                while (index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str2)
                                {
                                    if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                    {
                                        int index3 = 0;
                                        double num7 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                        num1 += Convert.ToDecimal(num7);
                                        num2 += num1;
                                        for (; index3 < dt.Rows.Count; ++index3)
                                        {
                                            if (dt.Rows[index3]["month"].ToString() == dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString())
                                            {
                                                num7 += double.Parse(dt.Rows[index3]["credit"].ToString());
                                                break;
                                            }
                                        }
                                        stringBuilder.Append("<td style='text-align:right;'>" + num7 + "<br/>");
                                        stringBuilder.Append(string.Concat(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]));
                                        stringBuilder.Append(" (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")");
                                    }
                                    else
                                    {
                                        stringBuilder.Append("<td style='text-align:right;'>" + dataSet2.Tables[1].Rows[index1]["Amount"] + "<br/>");
                                        stringBuilder.Append(string.Concat(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]));
                                    }
                                    stringBuilder.Append("</td>");
                                    ++index1;
                                    ++num5;
                                }
                                while (num5++ < 12)
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str2 != "")
                                {
                                    hashtable2.Add("@AdmnNo", str2);
                                    obj.GetDataTable("ps_sp_AnnualMontlyBusFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                    stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                                else
                                    stringBuilder.Append("<td align='right'>0.00</td>");
                                num1 = new Decimal(0);
                                ++index2;
                            }
                            stringBuilder.Append("</tr>");
                            ++num3;
                        }
                        if (index1 < dataSet2.Tables[1].Rows.Count)
                        {
                            while (index1 < dataSet2.Tables[1].Rows.Count)
                            {
                                string str2 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                                stringBuilder.Append("<tr class='tbltd'>");
                                stringBuilder.Append("<td>" + num3 + "</td>");
                                stringBuilder.Append("<td style='width:60px;'>" + dataSet2.Tables[1].Rows[index1]["FullName"] + "</td>");
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
                                    ++num5;
                                }
                                while (index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str2)
                                {
                                    if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                                    {
                                        int index3 = 0;
                                        double num7 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                        num1 += Convert.ToDecimal(num7);
                                        num2 += num1;
                                        for (; index3 < dt.Rows.Count; ++index3)
                                        {
                                            if (dt.Rows[index3]["month"].ToString() == dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString())
                                            {
                                                num7 += double.Parse(dt.Rows[index3]["credit"].ToString());
                                                break;
                                            }
                                        }
                                        stringBuilder.Append("<td style='text-align:right;'>" + num7 + "<br/>");
                                        stringBuilder.Append(string.Concat(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]));
                                        stringBuilder.Append(" (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")");
                                    }
                                    else
                                    {
                                        stringBuilder.Append("<td style='text-align:right;'>" + dataSet2.Tables[1].Rows[index1]["Amount"] + "<br/>");
                                        stringBuilder.Append(string.Concat(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]));
                                    }
                                    stringBuilder.Append("</td>");
                                    ++index1;
                                    ++num5;
                                }
                                while (num5++ < 12)
                                    stringBuilder.Append("<td style='text-align:center;'>--</td>");
                                obj = new clsDAL();
                                Hashtable hashtable2 = new Hashtable();
                                DataTable dataTable2 = new DataTable();
                                hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                                if (str2 != "")
                                {
                                    hashtable2.Add("@AdmnNo", str2);
                                    obj.GetDataTable("ps_sp_AnnualMontlyBusFeeDue", hashtable2);
                                }
                                if (num1 > new Decimal(0))
                                    stringBuilder.Append("<td align='right'>" + num1 + "</td>");
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
                            stringBuilder.Append("<td>" + num3 + "</td>");
                            stringBuilder.Append("<td style='width:60px;'>" + dataSet2.Tables[0].Rows[index]["FullName"] + "</td>");
                            int num4 = 12;
                            while (num4-- > 0)
                                stringBuilder.Append("<td style='text-align:center;'>--</td>");
                            double num5 = double.Parse(dataSet2.Tables[1].Rows[index]["Amount"].ToString());
                            num1 += Convert.ToDecimal(num5);
                            num2 += num1;
                            obj = new clsDAL();
                            Hashtable hashtable2 = new Hashtable();
                            DataTable dataTable2 = new DataTable();
                            hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                            string str2 = dataSet2.Tables[0].Rows[index]["admnno"].ToString();
                            if (str2 != "")
                            {
                                hashtable2.Add("@AdmnNo", str2);
                                obj.GetDataTable("ps_sp_AnnualMontlyBusFeeDue", hashtable2);
                            }
                            if (num1 > new Decimal(0))
                                stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                            else
                                stringBuilder.Append("<td align='right'>0.00</td>");
                            stringBuilder.Append("</tr>");
                            ++num3;
                        }
                    }
                }
                else if (dataSet2.Tables[1].Rows.Count > 0)
                {
                    for (int index1 = 0; index1 < dataSet2.Tables[1].Rows.Count; index1 = index1 - 1 + 1)
                    {
                        int num4 = 0;
                        int num5 = 0;
                        string str2 = dataSet2.Tables[1].Rows[index1]["admnno"].ToString();
                        int num6 = int.Parse(dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString());
                        stringBuilder.Append("<tr class='tbltd'>");
                        stringBuilder.Append("<td>" + num3 + "</td>");
                        stringBuilder.Append("<td style='width:60px;'>" + dataSet2.Tables[1].Rows[index1]["FullName"] + "</td>");
                        if (num6 > 4)
                            num4 = num6 - 4;
                        else if (num6 < 4)
                            num4 = 8 + num6;
                        while (num4-- > 0)
                        {
                            stringBuilder.Append("<td style='text-align:center;'>--</td>");
                            ++num5;
                        }
                        while (index1 < dataSet2.Tables[1].Rows.Count && dataSet2.Tables[1].Rows[index1]["admnno"].ToString() == str2)
                        {
                            if (dataSet2.Tables[1].Rows[index1]["Amount"].ToString() != "--")
                            {
                                int index2 = 0;
                                double num7 = double.Parse(dataSet2.Tables[1].Rows[index1]["Amount"].ToString());
                                num1 += Convert.ToDecimal(num7);
                                num2 += num1;
                                for (; index2 < dt.Rows.Count; ++index2)
                                {
                                    if (dt.Rows[index2]["month"].ToString() == dataSet2.Tables[1].Rows[index1]["MonthNo"].ToString())
                                    {
                                        num7 += double.Parse(dt.Rows[index2]["credit"].ToString());
                                        break;
                                    }
                                }
                                stringBuilder.Append("<td style='text-align:right;'>" + num7 + "</br>");
                                stringBuilder.Append(string.Concat(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]));
                                stringBuilder.Append(" (" + dataSet2.Tables[1].Rows[index1]["RecvDate"] + ")");
                            }
                            else
                            {
                                stringBuilder.Append("<td style='text-align:right;'>" + dataSet2.Tables[1].Rows[index1]["Amount"] + "</br>");
                                stringBuilder.Append(string.Concat(dataSet2.Tables[1].Rows[index1]["Receipt_VrNo"]));
                            }
                            stringBuilder.Append("</td>");
                            ++index1;
                            ++num5;
                        }
                        while (num5++ < 12)
                            stringBuilder.Append("<td style='text-align:center;'>--</td>");
                        obj = new clsDAL();
                        Hashtable hashtable2 = new Hashtable();
                        DataTable dataTable2 = new DataTable();
                        hashtable2.Add("@SessionYr", drpSession.SelectedValue.ToString());
                        if (str2 != "")
                        {
                            hashtable2.Add("@AdmnNo", str2);
                            obj.GetDataTable("ps_sp_AnnualMontlyBusFeeDue", hashtable2);
                        }
                        if (num1 > new Decimal(0))
                            stringBuilder.Append("<td align='right'>" + num1 + "</td>");
                        else
                            stringBuilder.Append("<td align='right'>0.00</td>");
                        stringBuilder.Append("</tr>");
                        num1 = new Decimal(0);
                        stringBuilder.Append("</tr>");
                        ++num3;
                    }
                }
                stringBuilder.Append("<tr class='tbltd'>");
                stringBuilder.Append("<td align='right' style='font-weight:bold;' colspan='2'>Total :</td>");
                foreach (DataRow row in (InternalDataCollectionBase)dataSet2.Tables[2].Rows)
                {
                    stringBuilder.Append("<td align='right' style='font-weight:bold;'>");
                    stringBuilder.Append(row["TotAmount"].ToString().Trim());
                    stringBuilder.Append("</td>");
                }
                stringBuilder.Append("<td align='right' style='font-weight:bold;'>" + num2.ToString("0.00") + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("</div>");
                lblreport.Text = stringBuilder.ToString();
                Session["BusMCR2"] = stringBuilder.ToString();
            }
            else
            {
                lblreport.Text = "No Data Found";
                Session["BusMCR2"] = null;
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
        Response.Redirect("rptBusMCRPrint2.aspx");
    }
}
