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

public partial class Reports_rptMiscellaneousFee : System.Web.UI.Page
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
        DataSet dataSet2 = common.GetDataSet("Ps_Sp_GetMiscellaneousFee", new Hashtable()
    {
      {
         "@AdmnNo",
         Request.QueryString["Id"].ToString()
      },
      {
         "@SchoolID",
         Session["SchoolId"].ToString()
      }
    });
        if (dataSet2.Tables[0].Rows.Count > 0)
        {
            lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
            lblRegd.Text = dataSet2.Tables[0].Rows[0]["AdmnNo"].ToString();
        }
        else
            lblName.Text = "No records to display !";
        if (dataSet2.Tables[1].Rows.Count > 0)
            MiscFee(dataSet2);
        else
            lblMiscFee.Text = "No Misc Fee available to display !";
        TotalAmount(dataSet2);
    }

    private void TotalAmount(DataSet ds)
    {
        double num = 0.0;
        if (ds.Tables[1].Rows.Count > 0)
            num = Convert.ToDouble(ds.Tables[1].Compute("SUM(DebitAmt)", "").ToString());
        lblTotal.Text = "<b>Total Amount Due : </b>" + string.Format("{0:f2}", num);
    }

    private void MiscFee(DataSet ds)
    {
        DataTable dataTable = new DataTable();
        DataTable table = ds.Tables[1];
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
        stringBuilder.Append("<tr style='background-color: #666; color: White;'>");
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
        stringBuilder.Append("<td align='right' class='tbltd' style='Font-Size:12px;' colspan='2'><b>Total Miscellaneous Fee :</b></td>");
        stringBuilder.Append("<td align='right' class='tbltd'><b>" + str.ToString() + "</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblMiscFee.Text = stringBuilder.ToString().Trim();
    }
}