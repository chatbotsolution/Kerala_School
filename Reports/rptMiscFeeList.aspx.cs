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

public partial class Reports_rptMiscFeeList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        if (Request.QueryString["AdId"] != null)
            lblHeader.Text = !(Request.QueryString["AdId"] == "1") ? "Hostel Fee :" : "Bus Fee :";
        if (Request.QueryString["Id"] == null)
            return;
        FillMiscData();
    }

    private void FillMiscData()
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
        hashtable.Add("@Ad_Id", Request.QueryString["AdId"].ToString());
        hashtable.Add("@SchoolID", Session["SchoolId"].ToString());
        hashtable.Add("@CurrSessStDt", ("04/01/" + currSession.Substring(0, 4)));
        hashtable.Add("@CurrSessEndDt", ("03/31/" + currSession.Substring(0, 2) + currSession.Substring(5, 2)));
        DataSet dataSet2 = common.GetDataSet("Ps_Sp_GetMiscFee", hashtable);
        if (dataSet2.Tables[0].Rows.Count > 0)
        {
            lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
            lblRegd.Text = dataSet2.Tables[0].Rows[0]["AdmnNo"].ToString();
        }
        else
            lblName.Text = "No Students available";
        if (dataSet2.Tables[1].Rows.Count > 0)
        {
            MiscFee(dataSet2);
            lblTotal.Text = "<b>Total Amount Due : </b>" + string.Format("{0:f2}", Convert.ToDouble(dataSet2.Tables[1].Compute("SUM(MiscFee)", "").ToString())).ToString();
        }
        else
            lblMiscFee.Text = "No Bus/Hostel Fee Available !";
    }

    private void MiscFee(DataSet ds)
    {
        DataTable dataTable = new DataTable();
        DataTable table = ds.Tables[1];
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 120px; background-color: #666; color: White;' align='right' class='tbltxt'>Transaction Date</td>");
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
            stringBuilder.Append(row["Ad_Description"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            stringBuilder.Append(row["MiscFee"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblMiscFee.Text = stringBuilder.ToString().Trim();
    }
}