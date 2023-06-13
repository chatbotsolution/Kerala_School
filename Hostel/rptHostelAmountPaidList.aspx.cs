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

public partial class Hostel_rptHostelAmountPaidList : System.Web.UI.Page
{
    private clsGenerateFee obj1 = new clsGenerateFee();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Request.QueryString["Id"] == null)
            return;
        IndvPaidDetail();
    }

    private void IndvPaidDetail()
    {
        Common common = new Common();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@AdmnNo", Request.QueryString["Id"].ToString());
        string currSession = obj1.CreateCurrSession();
        hashtable.Add("@SessionYr", currSession);
        string empty = string.Empty;
        if (Request.QueryString["FP"] != null)
            hashtable.Add("@FullSess", Request.QueryString["FP"].ToString());
        hashtable.Add("@SchoolID", int.Parse(Session["SchoolId"].ToString()));
        hashtable.Add("@CurrSessStDt", ("04/01/" + currSession.Substring(0, 4)));
        hashtable.Add("@CurrSessEndDt", ("03/31/" + currSession.Substring(0, 2) + currSession.Substring(5, 2)));
        currSession.Split('-');
        DataSet dataSet2 = common.GetDataSet("HostRptGetIndivPaidFee", hashtable);
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
        if (dataSet2.Tables[2].Rows.Count > 0)
            PrevDue(dataSet2);
        else
            lblPrevDue.Text = "No Previous Due Paid for This Session ! ";
        TotalAmount(dataSet2);
    }

    private void TotalAmount(DataSet ds)
    {
        double num1;
        double num2 = num1 = 0.0;
        new clsGenerateFee().CreateCurrSession().Split('-');
        if (ds.Tables[1].Rows.Count > 0)
            num2 = Convert.ToDouble(ds.Tables[1].Compute("SUM(CreditAmt)", "").ToString());
        if (ds.Tables[2].Rows.Count > 0)
            num1 = Convert.ToDouble(ds.Tables[2].Compute("SUM(Balance)", "").ToString());
        lblTotal.Text = "Total Amount Due  : " + string.Format("{0:f2}", (num2 + num1));
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
        stringBuilder.Append("<td style='background-color: #666; color: White; width: 100px' align='right' class='tbltxt'>Amount Paid</td>");
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
            stringBuilder.Append(row["CreditAmt"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        string empty = string.Empty;
        string str = table.Compute("SUM(CreditAmt)", "").ToString();
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='right' class='tbltd' style='Font-Size:12px;' colspan='2'><b>Total Regular Fee Paid :</b></td>");
        stringBuilder.Append("<td align='right' class='tbltd'><b>" + str.ToString() + "</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblSchoolFee.Text = stringBuilder.ToString().Trim();
    }

    private void PrevDue(DataSet ds)
    {
        DataTable dataTable = new DataTable();
        obj1.CreateCurrSession();
        DataTable table = ds.Tables[2];
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
        foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append("Previous Balance Paid :");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd' style='width: 100px'>");
            stringBuilder.Append(row["Balance"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblPrevDue.Text = stringBuilder.ToString().Trim();
    }
}