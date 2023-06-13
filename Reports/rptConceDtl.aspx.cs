using ASP;
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

public partial class Reports_rptConceDtl : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        lblHeader.Text = "Fee Concession:";
        FillData();
    }

    private void FillData()
    {
        clsDAL clsDal = new clsDAL();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@AdmnNo", Request.QueryString["AdmnNo"].ToString());
        DateTime dateTime = Convert.ToDateTime(Request.QueryString["Dt"].ToString());
        hashtable.Add("@AdjDate", dateTime.ToString("MM/dd/yyyy"));
        DataSet dataSet2 = clsDal.GetDataSet("Ps_FeeAdjstDtl", hashtable);
        if (dataSet2.Tables[0].Rows.Count > 0)
        {
            lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
            lblRegd.Text = dataSet2.Tables[0].Rows[0]["AdmnNo"].ToString();
        }
        else
            lblName.Text = "No Students available";
        if (dataSet2.Tables[1].Rows.Count > 0)
            ConcessionFee(dataSet2);
        else
            lblConcessFee.Text = "No Bus/Hostel Fee Available !";
    }

    private void ConcessionFee(DataSet ds)
    {
        DataTable dataTable = new DataTable();
        DataTable table = ds.Tables[1];
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
        stringBuilder.Append("<tr><td colspan='3'><b>Concession Date:" + table.Rows[0]["AdjDateStr"] + "</b></td></tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 120px; background-color: #666; color: White;' align='left' class='tbltxt'>Fee Date</td>");
        stringBuilder.Append("<td  align='left' class='tbltxt' style='background-color: #666; color: White;'>Fee Name</td>");
        stringBuilder.Append("<td style='background-color: #666; color: White; width: 100px' align='right' class='tbltxt'>Concession Amount</td>");
        stringBuilder.Append("</tr>");
        double num = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)table.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["TransDtStr"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["FeeName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            stringBuilder.Append(row["Amount"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            num += Convert.ToDouble(row["Amount"].ToString().Trim());
        }
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='2'  align='right'  class='tbltd'>");
        stringBuilder.Append("<strong class='error'>Total &nbsp;:&nbsp;</strong>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='right' class='tbltd'>");
        stringBuilder.Append("<strong class='error'>" + string.Format("{0:F2}", num));
        stringBuilder.Append("</strong></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblConcessFee.Text = stringBuilder.ToString().Trim();
    }
}
