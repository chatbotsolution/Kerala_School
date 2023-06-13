using ASP;
using SanLib;
using System;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptMlyFeePaidStatusPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Session["MonthlyPayment"] == null)
            return;
        GenerateXML();
        lblReport.Text = Session["MonthlyPayment"].ToString().Trim();
    }

    private void GenerateXML()
    {
        string str1 = Session["SSVMID"].ToString();
        clsDAL clsDal = new clsDAL();
        DataSet dataSet = new DataSet();
        int num = (int)dataSet.ReadXml(Server.MapPath("../XMLFiles/Detail.xml"));
        DataTable dataTable = new DataTable();
        DataTable table = dataSet.Tables[0];
        string str2 = clsDal.Decrypt(table.Rows[0]["SchoolName"].ToString().Trim(), str1);
        string str3 = clsDal.Decrypt(table.Rows[0]["Address1"].ToString().Trim(), str1);
        string str4 = clsDal.Decrypt(table.Rows[0]["Address2"].ToString().Trim(), str1);
        string str5 = clsDal.Decrypt(table.Rows[0]["Address3"].ToString().Trim(), str1);
        string str6 = clsDal.Decrypt(table.Rows[0]["Pin"].ToString().Trim(), str1);
        string str7 = clsDal.Decrypt(table.Rows[0]["Phone"].ToString().Trim(), str1);
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<Center><table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td rowspan='3' style='width:110px; height:100px;'>");
        stringBuilder.Append("<img src='../images/leftlogo.jpg' width='75px' height='80px' />");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='center' style='font-size: 23px; font-weight: bold;  padding-bottom:2px; line-height:30px;'><u>");
        stringBuilder.Append("Fee Paid Details");
        stringBuilder.Append("</u></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='center' style='font-size: 15px; font-weight: bold;'>(");
        stringBuilder.Append("Session Year : " + Session["Session"].ToString().Trim());
        stringBuilder.Append(")</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='center' style='font-size: 15px; font-weight: bold;'>");
        stringBuilder.Append(str2 + " <b>, </b>" + str3);
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='left' style='font-size: 12px;font-weight: bold;'>");
        stringBuilder.Append("Established - 1977");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='center' style='font-size: 15px;'>");
        stringBuilder.Append(" " + str4 + " <b>, </b>  " + str5 + " <b>, </b>   <b>Pin : </b>" + str6 + "<b>, Phone No. : </b>" + str7);
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("</center>");
        lblName.Text = stringBuilder.ToString().Trim();
    }
}