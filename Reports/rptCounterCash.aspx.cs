using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptCounterCash : System.Web.UI.Page
{
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        FillCollectCounter();
    }

    private void FillCollectCounter()
    {
        drpFeeCounter.DataSource = new Common().GetDataTable("ps_sp_get_FeeWorkStation");
        drpFeeCounter.DataTextField = "FeeCollector";
        drpFeeCounter.DataValueField = "USER_ID";
        drpFeeCounter.DataBind();
        drpFeeCounter.Items.Insert(0, new ListItem("All", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    private void GenerateReport()
    {
        if (drpFeeCounter.SelectedIndex > 0)
            ht.Add("@FeeCounter", drpFeeCounter.SelectedValue);
        if (drpModeOfPayment.SelectedIndex > 0)
            ht.Add("@PaymentMode", drpModeOfPayment.SelectedValue.Trim());
        if (txtFromDt.Text != "")
            ht.Add("@FromDate", dtpPros.GetDateValue());
        if (txtToDt.Text != "")
            ht.Add("@Todate", dtpPros1.GetDateValue());
        ht.Add("@SchoolID", Session["SchoolId"].ToString());
        dt = obj.GetDataTable("ps_sp_RptCounterCash", ht);
        cashReport(dt);
    }

    private void cashReport(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='3' align='left'><font color='Black'>" + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt") + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td colspan='3' align='right'><font color='Black'>No Of Records: " + dt.Rows.Count.ToString() + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 50px' align='left'><b>Receive Date</b></td>");
            stringBuilder.Append("<td style='width: 100px' align='left'><b>Payment Mode</b></td>");
            stringBuilder.Append("<td  align='right'><b>Total Amount</b></td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["TransDtStr"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["PmtMode"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right'>");
                string str1 = "<a target='_blank' href=";
                string str2;
                if (drpFeeCounter.SelectedIndex > 0)
                    str2 = "'rptRecievedDetail.aspx?Dt=" + Convert.ToDateTime(row["TransDt"].ToString().Trim()).ToString("dd MMM yyyy") + " &PMode=" + row["PmtMode"].ToString().Trim() + "&UId=" + drpFeeCounter.SelectedValue.ToString().Trim() + "'";
                else
                    str2 = "'rptRecievedDetail.aspx?Dt=" + Convert.ToDateTime(row["TransDt"].ToString().Trim()).ToString("dd MMM yyyy") + " &PMode=" + row["PmtMode"].ToString().Trim() + "'";
                string str3 = ">";
                stringBuilder.Append(str1.Replace('\'', '"') + str2 + str3.Replace('\'', '"'));
                stringBuilder.Append("<b>" + row["Amnt"].ToString() + "</b></a>");
                stringBuilder.Append("</td>");
            }
            string str = dt.Compute("SUM(Amnt)", "").ToString();
            stringBuilder.Append("<tr style='font-weight: bold; text-align: left; background-color: Black;color:White;'>");
            stringBuilder.Append("<td align='Left' style='Font-Size:14px;'><b>Grand Total :</b></td>");
            stringBuilder.Append("<td align='right' colspan='5'><b>" + str.ToString() + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table></center></fieldset>");
            lblReport.Text = stringBuilder.ToString().Trim();
        }
        else
            lblReport.Text = "<div style='border:solid 1px Black'>No records found !</div>";
    }
}
