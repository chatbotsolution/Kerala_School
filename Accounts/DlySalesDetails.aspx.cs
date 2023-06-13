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

public partial class Accounts_DlySalesDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Request.QueryString["Id"] == null)
            return;
        FillSalesDetails();
    }

    private void FillSalesDetails()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = clsDal.GetDataTable("ACTS_GetSalesDetail", new Hashtable()
    {
      {
         "@InvId",
         Request.QueryString["Id"].ToString()
      },
      {
         "@ShopId",
        Session["SchoolId"]
      }
    });
        if (dataTable2.Rows.Count > 0)
            SalesDetails(dataTable2);
        else
            lblSalesDetails.Text = "No supplier to display !";
    }

    private void SalesDetails(DataTable dt)
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = dt;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr style='background-color: #666; color: White;'>");
        stringBuilder.Append("<td style='background-color: #666; color: White;' align='left' class='tbltxt'>Item Name</td>");
        stringBuilder.Append("<td style='background-color: #666; color: White;  White; width:100px;' align='right' class='tbltxt'>Quantity</td>");
        stringBuilder.Append("<td  align='right' class='tbltxt' style='background-color: #666; color: White;  White; width:120px;'>Price</td>");
        stringBuilder.Append("<td style='background-color: #666; White; width:100px;' align='right' class='tbltxt'>Disc Amount</td>");
        stringBuilder.Append("<td style='background-color: #666; White; width:100px;' align='right' class='tbltxt'>Total Amount</td>");
        stringBuilder.Append("</tr>");
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='tbltd'  style='white-space:nowrap;'>");
            stringBuilder.Append(row["ItemName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            stringBuilder.Append(row["Qty"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            stringBuilder.Append(row["SalePrice"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            stringBuilder.Append(row["DiscAmt"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            stringBuilder.Append(row["TotAmt"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            num1 += Convert.ToDouble(row["Qty"].ToString().Trim());
            num2 += Convert.ToDouble(row["SalePrice"].ToString().Trim());
            num3 += Convert.ToDouble(row["DiscAmt"].ToString().Trim());
            num4 += Convert.ToDouble(row["TotAmt"].ToString().Trim());
        }
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='right' style='border-top:0px;' class='innertbltxt'>");
        stringBuilder.Append("<b>Total :</b></td>");
        stringBuilder.Append("<td align='right' style='border-top:0px;' class='innertbltxt'>");
        stringBuilder.Append("<b>" + string.Format("{0:F2}", num1) + "</b></td>");
        stringBuilder.Append("<td align='right' style='border-top:0px;' class='innertbltxt'>");
        stringBuilder.Append("<b>" + string.Format("{0:F2}", num2) + "</b></td>");
        stringBuilder.Append("<td align='right' style='border-top:0px;' class='innertbltxt'>");
        stringBuilder.Append("<b>" + string.Format("{0:F2}", num3) + "</b></td>");
        stringBuilder.Append("<td align='right' style='border-top:0px;' class='innertbltxt'>");
        stringBuilder.Append("<b>" + string.Format("{0:F2}", num4) + "</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblSalesDetails.Text = stringBuilder.ToString().Trim();
    }
}