using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptAmountDueList : System.Web.UI.Page
{
    private clsGenerateFee obj1 = new clsGenerateFee();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Request.QueryString["Id"] == null)
            return;
        IndvDueDetail();
    }

    private void IndvDueDetail()
    {
        Common common = new Common();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        string empty = string.Empty;
        if (Request.QueryString["FP"] != null)
            hashtable.Add("@FullSess", Request.QueryString["FP"].ToString());
        string currSession = new clsGenerateFee().CreateCurrSession();
        hashtable.Add("@SessionYr", currSession);
        hashtable.Add("@AdmnNo", Request.QueryString["Id"].ToString());
        hashtable.Add("@SchoolID", Session["SchoolId"].ToString());
        hashtable.Add("@CurrSessStDt", ("04/01/" + currSession.Substring(0, 4)));
        hashtable.Add("@CurrSessEndDt", ("03/31/" + currSession.Substring(0, 2) + currSession.Substring(5, 2)));
        DataSet dataSet2;
        if (Convert.ToInt32(currSession.Split('-')[0]) >= 2014)
        {
            dataSet2 = common.GetDataSet("Ps_Sp_GetStudAmtDueNew", hashtable);
            if (dataSet2.Tables[0].Rows.Count > 0)
            {
                lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
                lblRegd.Text = dataSet2.Tables[0].Rows[0]["OldAdmnNo"].ToString();
            }
            else
                lblName.Text = "No Student Found !";
            if (dataSet2.Tables[1].Rows.Count > 0)
                DueSchoolFee(dataSet2);
            else
                lblSchoolFee.Text = "No School Fee Due For This Session Year !";
            //if (dataSet2.Tables[2].Rows.Count > 0)
            //    BusDue(dataSet2);
            //else
            //    lblBus.Text = "No Bus Due for This Session ! ";
            //if (dataSet2.Tables[3].Rows.Count > 0)
            //    HostelDue(dataSet2);
            //else
            //    lblHostel.Text = "No Book Due for This Session ! ";
        }
        else
        {
            dataSet2 = common.GetDataSet("Ps_Sp_GetStudAmtDue", hashtable);
            if (dataSet2.Tables[0].Rows.Count > 0)
            {
                lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
                lblRegd.Text = dataSet2.Tables[0].Rows[0]["AdmnNo"].ToString();
            }
            else
                lblName.Text = "No Student Found !";
            if (dataSet2.Tables[1].Rows.Count > 0)
                DueSchoolFee(dataSet2);
            else
                lblSchoolFee.Text = "No School Fee Due For This Session Year !";
            //if (dataSet2.Tables[2].Rows.Count > 0)
            //    FineDue(dataSet2);
            //else
            //    lblFine.Text = "No Fine Due for This Session !";
            //if (dataSet2.Tables[3].Rows.Count > 0)
            //    BusDue(dataSet2);
            //else
            //    lblBus.Text = "No Bus Due for This Session ! ";
            //if (dataSet2.Tables[4].Rows.Count > 0)
            //    HostelDue(dataSet2);
            //else
            //    lblHostel.Text = "No Book Due for This Session ! ";
        }
        TotalAmount(dataSet2);
    }

    private void TotalAmount(DataSet ds)
    {
        string currSession = new clsGenerateFee().CreateCurrSession();
        double num1;
        double num2 = num1 = 0.0;
        double num3 = num1;
        double num4 = num1;
        double num5 = num1;
        if (Convert.ToInt32(currSession.Split('-')[0]) >= 2014)
        {
            if (ds.Tables[1].Rows.Count > 0)
                num5 = Convert.ToDouble(ds.Tables[1].Compute("SUM(DebitAmt)", "").ToString());
            if (ds.Tables[2].Rows.Count > 0)
                num3 = Convert.ToDouble(ds.Tables[2].Compute("SUM(MiscFee)", "").ToString());
            if (ds.Tables[3].Rows.Count > 0)
                num2 = Convert.ToDouble(ds.Tables[3].Compute("SUM(MiscFee)", "").ToString());
        }
        else
        {
            if (ds.Tables[1].Rows.Count > 0)
                num5 = Convert.ToDouble(ds.Tables[1].Compute("SUM(DebitAmt)", "").ToString());
            if (ds.Tables[2].Rows.Count > 0)
                num4 = Convert.ToDouble(ds.Tables[2].Compute("SUM(DebitAmt)", "").ToString());
            if (ds.Tables[3].Rows.Count > 0)
                num3 = Convert.ToDouble(ds.Tables[3].Compute("SUM(MiscFee)", "").ToString());
            if (ds.Tables[4].Rows.Count > 0)
                num2 = Convert.ToDouble(ds.Tables[4].Compute("SUM(MiscFee)", "").ToString());
        }
        lblTotal.Text = "Total Amount Due  : " + string.Format("{0:f2}", (num5 + num4 + num3 + num2));
    }

    private void DueSchoolFee(DataSet ds)
    {
        DataTable dataTable = new DataTable();
        DataTable table = ds.Tables[1];
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 120px; background-color: #666; color: White;' align='right' class='tbltxt'>Transaction Date</td>");
        stringBuilder.Append("<td align='left' class='tbltxt' style='background-color: #666; color: White;'>Fee Name</td>");
        stringBuilder.Append("<td style='background-color: #666; color: White; width: 100px' align='right' class='tbltxt'>Amount Due</td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            stringBuilder.Append(row["TransDt"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["FeeName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            stringBuilder.Append(row["DebitAmt"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        string empty = string.Empty;
        string str = table.Compute("SUM(DebitAmt)", "").ToString();
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='right' class='tbltd' style='Font-Size:12px;' colspan='2'><b>Total Regular Fee Due :</b></td>");
        stringBuilder.Append("<td align='right' class='tbltd'><b>" + str.ToString() + "</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblSchoolFee.Text = stringBuilder.ToString().Trim();
    }

    //    private void FineDue(DataSet ds)
    //    {
    //        DataTable dataTable = new DataTable();
    //        DataTable table = ds.Tables[2];
    //        StringBuilder stringBuilder = new StringBuilder("");
    //        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
    //        stringBuilder.Append("<tr>");
    //        stringBuilder.Append("<td  align='left' class='tbltxt' style='background-color: #666; color: White;'>Due Date</td>");
    //        stringBuilder.Append("<td style='background-color: #666; color: White; width: 100px' align='right' class='tbltxt'>Amount Due</td>");
    //        stringBuilder.Append("</tr>");
    //        foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
    //        {
    //            stringBuilder.Append("<tr>");
    //            stringBuilder.Append("<td align='left' class='tbltd'>");
    //            stringBuilder.Append(row["TransDt"].ToString().Trim());
    //            stringBuilder.Append("</td>");
    //            stringBuilder.Append("<td align='right' class='tbltd'>");
    //            stringBuilder.Append(row["DebitAmt"].ToString().Trim());
    //            stringBuilder.Append("</td>");
    //            stringBuilder.Append("</tr>");
    //        }
    //        string empty = string.Empty;
    //        string str = table.Compute("SUM(DebitAmt)", "").ToString();
    //        stringBuilder.Append("<tr>");
    //        stringBuilder.Append("<td align='right' class='tbltd' style='Font-Size:12px;'><b>Total Fine Due :</b></td>");
    //        stringBuilder.Append("<td align='right' class='tbltd'><b>" + str.ToString() + "</b></td>");
    //        stringBuilder.Append("</tr>");
    //        stringBuilder.Append("</table>");
    //        lblFine.Text = stringBuilder.ToString().Trim();
    //    }

    //    private void BusDue(DataSet ds)
    //    {
    //        DataTable dataTable1 = new DataTable();
    //        DataTable dataTable2 = Convert.ToInt32(obj1.CreateCurrSession().Substring(0, 4).Trim()) < 2014 ? ds.Tables[3] : ds.Tables[2];
    //        StringBuilder stringBuilder = new StringBuilder("");
    //        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
    //        stringBuilder.Append("<tr>");
    //        stringBuilder.Append("<td  align='left' class='tbltxt' style='background-color: #666; color: White;'>Transaction Date</td>");
    //        stringBuilder.Append("<td align='left' class='tbltxt' style='background-color: #666; color: White;'>Fee Name</td>");
    //        stringBuilder.Append("<td style='background-color: #666; color: White; width: 100px' align='right' class='tbltxt'>Amount Due</td>");
    //        stringBuilder.Append("</tr>");
    //        foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
    //        {
    //            stringBuilder.Append("<tr>");
    //            stringBuilder.Append("<td align='right' class='tbltd'>");
    //            stringBuilder.Append(row["TransDt"].ToString().Trim());
    //            stringBuilder.Append("</td>");
    //            stringBuilder.Append("<td align='left' class='tbltd'>");
    //            stringBuilder.Append(row["Ad_Description"].ToString().Trim());
    //            stringBuilder.Append("</td>");
    //            stringBuilder.Append("<td align='right' class='tbltd'>");
    //            stringBuilder.Append(row["MiscFee"].ToString().Trim());
    //            stringBuilder.Append("</td>");
    //            stringBuilder.Append("</tr>");
    //        }
    //        string empty = string.Empty;
    //        string str = dataTable2.Compute("SUM(MiscFee)", "").ToString();
    //        stringBuilder.Append("<tr>");
    //        stringBuilder.Append("<td align='right' class='tbltd' style='Font-Size:12px;' colspan='2'><b>Total Bus Fee Due :</b></td>");
    //        stringBuilder.Append("<td align='right' class='tbltd'><b>" + str.ToString() + "</b></td>");
    //        stringBuilder.Append("</tr>");
    //        stringBuilder.Append("</table>");
    //        lblBus.Text = stringBuilder.ToString().Trim();
    //    }

    //    private void HostelDue(DataSet ds)
    //    {
    //        DataTable dataTable1 = new DataTable();
    //        DataTable dataTable2 = Convert.ToInt32(obj1.CreateCurrSession().Substring(0, 4).Trim()) < 2014 ? ds.Tables[4] : ds.Tables[3];
    //        StringBuilder stringBuilder = new StringBuilder("");
    //        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
    //        stringBuilder.Append("<tr>");
    //        stringBuilder.Append("<td  align='left' class='tbltxt' style='background-color: #666; color: White;'>Transaction Date</td>");
    //        stringBuilder.Append("<td align='left' class='tbltxt' style='background-color: #666; color: White;'>Fee Name</td>");
    //        stringBuilder.Append("<td style='background-color: #666; color: White; width: 100px' align='right' class='tbltxt'>Amount Due</td>");
    //        stringBuilder.Append("</tr>");
    //        foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
    //        {
    //            stringBuilder.Append("<tr>");
    //            stringBuilder.Append("<td align='right' class='tbltd'>");
    //            stringBuilder.Append(row["TransDt"].ToString().Trim());
    //            stringBuilder.Append("</td>");
    //            stringBuilder.Append("<td align='left' class='tbltd'>");
    //            stringBuilder.Append(row["Ad_Description"].ToString().Trim());
    //            stringBuilder.Append("</td>");
    //            stringBuilder.Append("<td align='right' class='tbltd'>");
    //            stringBuilder.Append(row["MiscFee"].ToString().Trim());
    //            stringBuilder.Append("</td>");
    //            stringBuilder.Append("</tr>");
    //        }
    //        string empty = string.Empty;
    //        string str = dataTable2.Compute("SUM(MiscFee)", "").ToString();
    //        stringBuilder.Append("<tr>");
    //        stringBuilder.Append("<td align='right' class='tbltd' style='Font-Size:12px;' colspan='2'><b>Total Book Fee Due :</b></td>");
    //        stringBuilder.Append("<td align='right' class='tbltd'><b>" + str.ToString() + "</b></td>");
    //        stringBuilder.Append("</tr>");
    //        stringBuilder.Append("</table>");
    //        lblHostel.Text = stringBuilder.ToString().Trim();
    //    }
    //}
}