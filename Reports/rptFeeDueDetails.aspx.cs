﻿using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptFeeDueDetails : System.Web.UI.Page
{
    private clsDAL clsobj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SSVMID"] != null)
        {
            if (Page.IsPostBack || Request.QueryString["FY"] == null)
                return;
            if (Request.QueryString["FY"] == "y")
                GetDataFullYr();
            else if (Request.QueryString["FY"] == "n")
                getdata();
            else
                GetDataForMonth();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void getdata()
    {
        clsDAL clsDal = new clsDAL();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        string currSession = new clsGenerateFee().CreateCurrSession();
        string str = "04/01/" + Request.QueryString["sess"].ToString().Trim().Substring(0, 4);
        hashtable.Add("@AdmnNo", Request.QueryString["admnno"].ToString());
        hashtable.Add("@SchoolID", int.Parse(Session["SchoolId"].ToString()));
        hashtable.Add("@SessionYr", Request.QueryString["sess"].ToString());
        hashtable.Add("@PrevSession", currSession);
        hashtable.Add("@CurrSessStartDt", str);
        hashtable.Add("@Due", Request.QueryString["Due"].ToString());
        hashtable.Add("@FeeTillDt", Request.QueryString["ft"].ToString());
        DataSet dataSet2 = clsDal.GetDataSet("Ps_Sp_GetStudFeeDtls", hashtable);
        if (dataSet2.Tables[0].Rows.Count <= 0)
            return;
        lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
        lblRegd.Text = dataSet2.Tables[0].Rows[0]["AdmnNo"].ToString();
        CreateIndividualRpt(dataSet2.Tables[1], lblBalReg);
    }

    private void CreateIndividualRpt(DataTable dt, Label lbl)
    {
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
            stringBuilder.Append("<tr style='background-color: #666; color: White;'>");
            stringBuilder.Append("<td style='width: 120px; background-color: #666; color: White;' align='right' class='tbltxt'>Transaction Date</td>");
            stringBuilder.Append("<td  align='left' class='tbltxt' style='background-color: #666; color: White;'>Fee Name</td>");
            stringBuilder.Append("<td style='background-color: #666; color: White; width: 100px' align='right' class='tbltxt'>Balance Amount</td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='right' class='tbltd'>");
                stringBuilder.Append(row["TransDt"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='tbltd'>");
                stringBuilder.Append(row["TransDesc"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' class='tbltd'>");
                stringBuilder.Append(row["balance"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            string empty = string.Empty;
            string str = dt.Compute("SUM(balance)", "").ToString();
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='right' class='tbltd' style='Font-Size:12px;' colspan='2'><b>Total Balance :</b></td>");
            stringBuilder.Append("<td align='right' class='tbltd'><b>" + str.ToString() + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            lbl.Text = stringBuilder.ToString().Trim();
        }
        else
            lbl.Text = "No balance !";
    }

    private void GetDataFullYr()
    {
        clsDAL clsDal = new clsDAL();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        new clsGenerateFee().CreateCurrSession();
        hashtable.Add("@AdmnNo", Request.QueryString["admnno"].ToString());
        hashtable.Add("@SchoolID", int.Parse(Session["SchoolId"].ToString()));
        hashtable.Add("@SessionYr", Request.QueryString["sess"].ToString());
        DataSet dataSet2 = clsDal.GetDataSet("Ps_Sp_GetStudFullFeeDtls", hashtable);
        if (dataSet2.Tables[0].Rows.Count <= 0)
            return;
        lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
        lblRegd.Text = dataSet2.Tables[0].Rows[0]["AdmnNo"].ToString();
        CreateIndividualRpt(dataSet2.Tables[1], lblBalReg);
    }

    private void GetDataForMonth()
    {
        clsDAL clsDal = new clsDAL();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        string str1 = "04/01/" + Request.QueryString["sess"].ToString().Trim().Substring(0, 4);
        if (Request.QueryString["Due"].ToString() == "P")
        {
            string str2 = "04/01/" + (Convert.ToInt32(Request.QueryString["sess"].ToString().Trim().Substring(0, 4)) - 1).ToString();
        }
        hashtable.Add("@AdmnNo", Request.QueryString["admnno"].ToString());
        hashtable.Add("@SchoolID", int.Parse(Session["SchoolId"].ToString()));
        if (Request.QueryString["Due"].ToString() == "P")
            hashtable.Add("@SessionYr", PrevYr(Request.QueryString["sess"].ToString()));
        else
            hashtable.Add("@SessionYr", Request.QueryString["sess"].ToString());
        if (Request.QueryString["AdmnFee"] != null)
            hashtable.Add("@AdmnFee", Request.QueryString["AdmnFee"]);
        else
            hashtable.Add("@FeeDate", Session["FeeMonth"]);
        DataSet dataSet2 = clsDal.GetDataSet("Ps_Sp_GetStudMnthlyFeeDtls", hashtable);
        if (dataSet2.Tables[0].Rows.Count <= 0)
            return;
        lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
        lblRegd.Text = dataSet2.Tables[0].Rows[0]["AdmnNo"].ToString();
        CreateIndividualRpt(dataSet2.Tables[1], lblBalReg);
    }

    private string PrevYr(string p)
    {
        int int32_1 = Convert.ToInt32(Request.QueryString["sess"].ToString().Trim().Substring(0, 4));
        int num = int32_1 - 1;
        int int32_2 = Convert.ToInt32(int32_1.ToString().Trim().Substring(2));
        string empty = string.Empty;
        return Convert.ToString(num) + "-" + Convert.ToString(int32_2);
    }
}