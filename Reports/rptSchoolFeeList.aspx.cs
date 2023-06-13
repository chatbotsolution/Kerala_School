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


public partial class Reports_rptSchoolFeeList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Request.QueryString["Id"] == null)
            return;
        FillSchoolData();
    }

    private void FillSchoolData()
    {
        Common common = new Common();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        string currSession = new clsGenerateFee().CreateCurrSession();
        hashtable.Add("@SessionYr", currSession);
        string empty = string.Empty;
        if (Request.QueryString["FP"] != null)
            hashtable.Add("@FullSess", Request.QueryString["FP"].ToString());
        hashtable.Add("@AdmnNo", Request.QueryString["Id"].ToString());
        hashtable.Add("@SchoolID", Session["SchoolId"].ToString());
        hashtable.Add("@CurrSessStDt", ("04/01/" + currSession.Substring(0, 4)));
        hashtable.Add("@CurrSessEndDt", ("03/31/" + currSession.Substring(0, 2) + currSession.Substring(5, 2)));
        DataSet dataSet2;
        if (Convert.ToInt32(currSession.Split('-')[0]) >= 2014)
        {
            dataSet2 = common.GetDataSet("Ps_Sp_GetSchoolFeeNew", hashtable);
            if (dataSet2.Tables[0].Rows.Count > 0)
            {
                lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
                lblRegd.Text = dataSet2.Tables[0].Rows[0]["OldAdmnNo"].ToString();
            }
            else
                lblName.Text = "No Students available";
            if (dataSet2.Tables[1].Rows.Count > 0)
                SchoolFee(dataSet2);
            else
                lblSchoolFee.Text = "No School Fee Available !";
        }
        else
        {
            dataSet2 = common.GetDataSet("Ps_Sp_GetSchoolFee", hashtable);
            if (dataSet2.Tables[0].Rows.Count > 0)
            {
                lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
                lblRegd.Text = dataSet2.Tables[0].Rows[0]["AdmnNo"].ToString();
            }
            else
                lblName.Text = "No Students available";
            if (dataSet2.Tables[1].Rows.Count > 0)
                SchoolFee(dataSet2);
            else
                lblSchoolFee.Text = "No School Fee Available !";
            if (dataSet2.Tables[2].Rows.Count > 0)
                Fine(dataSet2);
            else
                lblFine.Text = "No Fine Available !";
        }
        TotalAmount(dataSet2);
    }

    private void TotalAmount(DataSet ds)
    {
        string[] strArray = new clsGenerateFee().CreateCurrSession().Split('-');
        double num1 = 0.0;
        double num2 = 0.0;
        if (Convert.ToInt32(strArray[0]) >= 2014)
        {
            if (ds.Tables[1].Rows.Count > 0)
                num1 = Convert.ToDouble(ds.Tables[1].Compute("SUM(DebitAmt)", "").ToString());
        }
        else
        {
            if (ds.Tables[1].Rows.Count > 0)
                num1 = Convert.ToDouble(ds.Tables[1].Compute("SUM(DebitAmt)", "").ToString());
            if (ds.Tables[2].Rows.Count > 0)
                num2 = Convert.ToDouble(ds.Tables[2].Compute("SUM(DebitAmt)", "").ToString());
        }
        lblTotal.Text = "<b>Total Amount Due : </b>" + string.Format("{0:f2}", (num1 + num2));
    }

    private void SchoolFee(DataSet ds)
    {
        DataTable dataTable = new DataTable();
        DataTable table = ds.Tables[1];
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
        stringBuilder.Append("<tr style='background-color: #666; color: White;'>");
        stringBuilder.Append("<td style='width: 120px; background-color: #666; color: White;' align='right' class='tbltxt'>Due Date</td>");
        stringBuilder.Append("<td  align='left' class='tbltxt' style='background-color: #666; color: White;'>Fee Name</td>");
        stringBuilder.Append("<td style='background-color: #666; color: White; width: 100px' align='right' class='tbltxt'>Amount</td>");
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
        stringBuilder.Append("<td align='right' class='tbltd' style='Font-Size:12px;' colspan='2'><b>Total Regular Fee :</b></td>");
        stringBuilder.Append("<td align='right' class='tbltd'><b>" + str.ToString() + "</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblSchoolFee.Text = stringBuilder.ToString().Trim();
    }

    private void Fine(DataSet ds)
    {
        DataTable dataTable = new DataTable();
        DataTable table = ds.Tables[2];
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td  align='left' class='tbltxt' style='background-color: #666; color: White;'>Transaction Date</td>");
        stringBuilder.Append("<td style='background-color: #666; color: White; width: 100px' align='right' class='tbltxt'>Amount</td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["TransDt"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            stringBuilder.Append(row["DebitAmt"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        string empty = string.Empty;
        string str = table.Compute("SUM(DebitAmt)", "").ToString();
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='right' class='tbltd' style='Font-Size:12px;'><b>Total Fine :</b></td>");
        stringBuilder.Append("<td align='right' class='tbltd'><b>" + str.ToString() + "</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblFine.Text = stringBuilder.ToString().Trim();
    }
}