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
public partial class Reports_rptBookDueDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        getBookReport(Request.QueryString["admino"].ToString().Trim());
    }

    private void getBookReport(string studAdmnno)
    {
        clsDAL clsDal = new clsDAL();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        hashtable.Add("@AdmnNo", studAdmnno);
        hashtable.Add("@Due", Request.QueryString["Due"].ToString());
        hashtable.Add("@SessionYr", Request.QueryString["sess"].ToString());
        hashtable.Add("@TransDate", ("04/01/" + Request.QueryString["sess"].ToString().Substring(0, 4)));
        DataSet dataSet2 = clsDal.GetDataSet("Ps_Sp_GetBookFeeDtls", hashtable);
        if (dataSet2.Tables[0].Rows.Count <= 0)
            return;
        lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
        lblRegd.Text = dataSet2.Tables[0].Rows[0]["AdmnNo"].ToString();
        CreateIndividualRpt(dataSet2.Tables[1], lblBalMisc);
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
}
