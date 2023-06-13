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

public partial class HR_rptLoanRecDetails : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Decimal num1 = new Decimal(0);
        DataTable dataTable = obj.GetDataTable("HR_LoanAdvanceDetails", new Hashtable()
    {
      {
         "@GenLedgerId",
         Request.QueryString["gl_id"]
      }
    });
        if (dataTable.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0'  width='100%' style='border:none 0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td  align='center' colspan='3'  style='border-bottom:none 0px; font-size:medium; font-weight:bold'>");
            stringBuilder.Append("Month-wise Loan Recovery Details of " + Request.QueryString["emp"].ToUpper());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' style='border-bottom:none 0px;border-right:none 0px; font-size:small;'><strong>Loan Type&nbsp;:&nbsp;</strong> " + Request.QueryString["lt"]).ToString().Trim();
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' style='border-bottom:none 0px;border-right:none 0px; border-left:none 0px; font-size:small;'><strong>Loan Date&nbsp;:&nbsp;</strong> " + Convert.ToDateTime(Request.QueryString["dt"]).ToString("dd-MMM-yyyy"));
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td  align='right' style='border-bottom:none 0px;border-left:none 0px;'><strong>Loan Amount&nbsp;:&nbsp;</strong>" + Convert.ToDecimal(Request.QueryString["amt"]).ToString("0.00"));
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0'  width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='border-left:none 0px; border-top:none 0px; border-bottom:none 0px; width: 10%;' align='left'><b>No.</b></td>");
            stringBuilder.Append("<td style='border-left:none 0px; border-top:none 0px; border-bottom:none 0px; width: 20%;' align='left'><b>Month</b></td>");
            stringBuilder.Append("<td style='border-left:none 0px; border-top:none 0px; border-bottom:none 0px; width: 20%;' align='left'><b>Year</b></td>");
            stringBuilder.Append("<td style='border-left:none 0px; border-top:none 0px; border-bottom:none 0px; width: 20%;' align='right'><b>Amount</b></td>");
            stringBuilder.Append("<td style='border-left:none 0px; border-top:none 0px; border-bottom:none 0px; width: 20%;' align='left'><b>Recovery Type (Principal/Interest)</b></td>");
            stringBuilder.Append("<td style='border-right:none 0px; border-left:none 0px; border-top:none 0px; border-bottom:none 0px; width: 10%;' align='left'><b>Recovery Status</b></td>");
            stringBuilder.Append("</tr>");
            int num2 = 1;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' style='border-left:none 0px; border-bottom:none 0px;' >");
                stringBuilder.Append(num2.ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-left:none 0px; border-bottom:none 0px;'>");
                stringBuilder.Append(row["CalMonth"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-left:none 0px; border-bottom:none 0px;'>");
                stringBuilder.Append(row["CalYear"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' style='border-left:none 0px; border-bottom:none 0px;'>");
                stringBuilder.Append(Convert.ToDecimal(row["RecAmt"]).ToString("0.00"));
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-left:none 0px; border-bottom:none 0px;'>");
                stringBuilder.Append(row["RecType"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' style='border-left:none 0px; border-right:none 0px; border-bottom:none 0px;'>");
                stringBuilder.Append(row["RcvdStatus"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                num1 += Convert.ToDecimal(row["RecAmt"]);
                ++num2;
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='right' style='border-left:none 0px; border-bottom:none 0px;' colspan='3' ><b>Total Amount</b></td>");
            stringBuilder.Append("<td align='right' style='border-left:none 0px; border-bottom:none 0px;' ><b>" + num1.ToString("0.00") + "</b></td>");
            stringBuilder.Append("<td align='right' style='border-left:none 0px; border-bottom:none 0px; border-right:none 0px;' colspan='2' >&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            btnPrint.Enabled = true;
        }
        else
        {
            lblReport.Text = "<div align='center' style='border:solid 1px Black'>No Record Available</div>";
            btnPrint.Enabled = false;
        }
    }
}