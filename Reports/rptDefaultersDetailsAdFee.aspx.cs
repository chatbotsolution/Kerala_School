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

public partial class Reports_rptDefaultersDetailsAdFee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Request.QueryString["Admnno"] == null)
            return;
        FillDefaultData(Session["DueDate"].ToString().Trim());
    }

    private void FillDefaultData(string DueDate)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@AdmnNo", Request.QueryString["Admnno"].ToString());
        hashtable.Add("@Ad_Id", Request.QueryString["Ad"].ToString());
        if (DueDate != "")
            hashtable.Add("@DueDate", DueDate);
        hashtable.Add("@SessionYr", Request.QueryString["SessionYr"].ToString().Trim());
        DataTable dataTable2 = clsDal.GetDataTable("Ps_Sp_getAdFeeDefaultdetail", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
            stringBuilder.Append("<tr style='background-color: #666; color: White;'>");
            stringBuilder.Append("<td style='width: 120px; background-color: #666; color: White;' align='right'>Fee Date</td>");
            stringBuilder.Append("<td  align='left' style='background-color: #666; color: White;'>Details</td>");
            stringBuilder.Append("<td style='background-color: #666; color: White; width: 100px' align='right'>Amount</td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='right'>");
                stringBuilder.Append(row["TransDtStr"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["AdFeeDesc"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right'>");
                stringBuilder.Append(row["Balance"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            string str = dataTable2.Compute("SUM(Balance)", "").ToString();
            stringBuilder.Append("<tr style='font-weight: bold; text-align: left;'>");
            stringBuilder.Append("<td align='right'  colspan='2'><b>Grand Total :</b></td>");
            stringBuilder.Append("<td align='right'><b>" + str.ToString() + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            lblDetails.Text = stringBuilder.ToString().Trim();
        }
        else
            lblDetails.Text = "No records to display !";
    }
}
