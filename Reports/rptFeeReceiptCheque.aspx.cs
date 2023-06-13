using ASP;
using Classes.DA;
using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptFeeReceiptCheque : System.Web.UI.Page
{
    private Common clsobj = new Common();
    private string RecptNo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Request.QueryString["status"] != null)
        {
            btnPrint.Visible = true;
            btnContinueChequeReceipt.Visible = true;
            RecptNo = Request.QueryString["rno"].ToString();
        }
        else
        {
            btnPrint.Visible = false;
            btnContinueChequeReceipt.Visible = false;
        }
        getdata();
        getchequedetail();
    }

    protected void getchequedetail()
    {
        DataTable dataTable = clsobj.ExecuteSql("select ChequeDate,ChequeNo,DrawanOnBank from PS_ChequeDetails where AdmnNo=" + Convert.ToInt32(Request.QueryString["admnno"].ToString()));
        lblchequeno.Text = dataTable.Rows[0]["ChequeNo"].ToString();
        lblchequedate.Text = Convert.ToDateTime(dataTable.Rows[0]["ChequeDate"].ToString()).ToString("dd/MM/yyyy");
        lblbank.Text = dataTable.Rows[0]["DrawanOnBank"].ToString();
    }

    private void getdata()
    {
        DataTable dataTable1 = clsobj.ExecuteSql("select distinct FullName,ClassName,Section,FatherName,convert(varchar,sm.UserDate,103) as usersdate from dbo.PS_StudMaster sm inner join dbo.PS_ClasswiseStudent cs on sm.admnno=cs.AdmnNo inner join dbo.PS_ClassMaster cm on cm.ClassID=cs.ClassID where cs.SessionYear='" + Request.QueryString["sess"] + "' and sm.AdmnNo=" + Convert.ToInt32(Request.QueryString["admnno"]));
        if (dataTable1.Rows.Count > 0)
        {
            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
            DateTime dateTime1 = Convert.ToDateTime(clsobj.ExecuteSql("select max(TransDate) as TransDate from dbo.PS_FeeLedger where AdmnNo=" + Convert.ToInt32(Request.QueryString["admnno"]) + " and debit > 0 ").Rows[0]["TransDate"]);
            DateTime dateTime2 = dateTime1.AddMonths(1);
            string str = dateTimeFormatInfo.GetMonthName(dateTime1.Month) + "-" + dateTimeFormatInfo.GetMonthName(dateTime2.Month);
            clsobj.ExecuteSql("select datename(month,(max(TransDate))) from dbo.PS_FeeLedger where AdmnNo=" + Convert.ToInt32(Request.QueryString["admnno"]) + " and debit > 0 ");
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("<table cellpadding='0' cellspacing='0' border='0' width='80%'>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            if (Session["rcptFee"] != null)
            {
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td style='width:25%'><strong>Receipt No:</strong></td><td style='width:25%'>" + Session["rcptFee"].ToString().Trim() + "</td>");
                stringBuilder1.Append("<td style='width:25%'><strong>Date:</strong></td><td style='width:25%'>" + dataTable1.Rows[0]["usersdate"].ToString() + "</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
                Session.Remove("rcptFee");
            }
            stringBuilder1.Append("<tr>");
            DataTable dataTable2 = clsobj.ExecuteSql("select * from PS_FeeNormsNew");
            if (dataTable2.Rows.Count > 0 && dataTable2.Rows[0]["FeeCollPeriod"].ToString() == "Bi Monthly")
                stringBuilder1.Append("<td style='width:25%'><strong>For the month of:</strong></td><td style='width:25%'>" + str + "</td>");
            stringBuilder1.Append("<td style='width:25%'><strong>Admission No:</strong></td><td style='width:25%'>" + Request.QueryString["admnno"].ToString() + "</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:25%'><strong>Name of the Pupil:</strong></td><td style='width:25%'>" + dataTable1.Rows[0][0].ToString() + "</td>");
            stringBuilder1.Append("<td style='width:25%'><strong>Father Name:</strong></td><td style='width:25%'>" + dataTable1.Rows[0][3].ToString() + "</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:25%'><strong>Class:</strong></td><td style='width:25%'>" + dataTable1.Rows[0][1].ToString() + "</td>");
            stringBuilder1.Append("<td style='width:25%'><strong>Section:</strong></td><td style='width:25%'>" + dataTable1.Rows[0][2].ToString() + "</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("</table>");
            DataTable dataTable3 = new DataTable();
            DataTable dataTable4;
            if (Request.QueryString["status"] != null)
                dataTable4 = clsobj.ExecuteSql("select convert(decimal(18,2),sum(credit)),TransDesc,admnno from dbo.PS_FeeLedger fl join dbo.PS_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt32(Request.QueryString["admnno"]) + " and PeriodicityID<=2 and Receipt_VrNo=" + RecptNo + " group by AdmnNo,TransDesc order by TransDesc");
            else
                dataTable4 = clsobj.ExecuteSql("select convert(decimal(18,2),sum(Balance)),TransDesc,admnno from dbo.PS_FeeLedger fl join dbo.PS_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt32(Request.QueryString["admnno"]) + " and PeriodicityID<=2 group by AdmnNo,TransDesc order by TransDesc");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<table border=1 width=70% cellspacing=0 cellpadding=0><tr><td colspan=2 align=center>");
            stringBuilder2.Append("<strong>PARTICULARS</strong></td><td align=center><strong>AMOUNT</strong></td></tr>");
            stringBuilder2.Append("<tr><td colspan=2 style='border-bottom:0;'><strong>ADMISSION CHARGES :</strong>");
            stringBuilder2.Append("");
            stringBuilder2.Append("</td><td></td></tr>");
            for (int index = 0; index <= dataTable4.Rows.Count - 1; ++index)
            {
                stringBuilder2.Append("<tr><td style='border-right:0;border-bottom:0;border-top:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
                stringBuilder2.Append(dataTable4.Rows[index][1].ToString());
                stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
                stringBuilder2.Append(".............");
                stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
                stringBuilder2.Append(dataTable4.Rows[index][0].ToString());
                stringBuilder2.Append("</td></tr>");
            }
            DataTable dataTable5 = new DataTable();
            DataTable dataTable6;
            if (Request.QueryString["status"] != null)
                dataTable6 = clsobj.ExecuteSql("select convert(decimal(18,2),sum(credit)),TransDesc,admnno from dbo.PS_FeeLedger fl join dbo.PS_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt32(Request.QueryString["admnno"]) + " and PeriodicityID>2 and Receipt_VrNo=" + RecptNo + " group by AdmnNo,TransDesc order by TransDesc");
            else
                dataTable6 = clsobj.ExecuteSql("select convert(decimal(18,2),sum(Balance)),TransDesc,admnno from dbo.PS_FeeLedger fl join dbo.PS_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt32(Request.QueryString["admnno"]) + " and PeriodicityID>2 group by AdmnNo,TransDesc order by TransDesc");
            stringBuilder2.Append("<tr></tr><tr></tr><tr></tr><tr></tr>");
            stringBuilder2.Append("<tr><td colspan=2 style='border-bottom:0;border-top:0;'><strong>MONTHLY CHARGES :</strong>");
            stringBuilder2.Append("");
            stringBuilder2.Append("</td><td></td></tr>");
            for (int index = 0; index <= dataTable6.Rows.Count - 1; ++index)
            {
                stringBuilder2.Append("<tr><td style='border-right:0;border-bottom:0;border-top:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
                stringBuilder2.Append(dataTable6.Rows[index][1].ToString());
                stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
                stringBuilder2.Append("..........");
                stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
                stringBuilder2.Append(dataTable6.Rows[index][0].ToString());
                stringBuilder2.Append("</td></tr>");
            }
            DataTable dataTable7 = new DataTable();
            DataTable dataTable8;
            if (Request.QueryString["status"] != null)
                dataTable8 = clsobj.ExecuteSql("select sum(credit) from dbo.PS_FeeLedger fl join dbo.PS_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt32(Request.QueryString["admnno"]) + " and Receipt_VrNo=" + RecptNo + " group by AdmnNo");
            else
                dataTable8 = clsobj.ExecuteSql("select sum(balance) from dbo.PS_FeeLedger fl join dbo.PS_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt32(Request.QueryString["admnno"]) + " group by AdmnNo");
            stringBuilder2.Append("<tr><td colspan='3'></td></tr></br>");
            stringBuilder2.Append("<tr><td colspan='2' align='right'><strong>TOTAL</strong></TD>");
            stringBuilder2.Append("<td align='right'>");
            double num1 = 0.0;
            if (dataTable8.Rows.Count > 0)
            {
                num1 = Convert.ToDouble(dataTable8.Rows[0][0].ToString());
                dataTable8.Rows[0][0].ToString();
            }
            stringBuilder2.Append(string.Format("{0:F2}", num1));
            stringBuilder2.Append("</td></tr>");
            stringBuilder2.Append("<tr><td colspan='2' align='right' style='border-bottom:0;'><strong>Fine Amount:</strong></td><td align='right'>");
            if (Session["PaidFine"] != null)
                stringBuilder2.Append(Session["PaidFine"]);
            else
                stringBuilder2.Append("0.00");
            stringBuilder2.Append("</td></tr>");
            stringBuilder2.Append("<tr><td colspan='2' align='right' style='border-bottom:0;'><strong>Amount in Credit:</strong></td><td align='right'>");
            if (Session["CreditAmount"] != null)
                stringBuilder2.Append(Session["CreditAmount"]);
            else
                stringBuilder2.Append("0.00");
            stringBuilder2.Append("</td></tr>");
            double num2 = num1;
            if (Session["CreditAmount"] != null)
                num2 += Convert.ToDouble(Session["CreditAmount"].ToString());
            if (Session["PaidFine"] != null)
                num2 += Convert.ToDouble(Session["PaidFine"].ToString());
            stringBuilder2.Append("<tr><td colspan='2' align='right' style='border-bottom:0;'><strong>Net Amount Recieved:</strong></td><td align='right'>");
            stringBuilder2.Append(string.Format("{0:F2}", num2));
            stringBuilder2.Append("</td></tr><table>");
            literaldata.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
            Session["printchequedata"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
        }
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('No Pending'); window.location='" + "../FeeManagement/feercptcheque.aspx" + "';", true);
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
    }

    protected void btnContinueCashReceipt_Click(object sender, EventArgs e)
    {
        Response.Redirect("../FeeManagement/feercptcheque.aspx");
    }

    protected void btnMainMenu_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Administrations/Home.aspx");
    }

    protected void btnPrint_Click1(object sender, EventArgs e)
    {
        Response.Redirect("rptFeeReceiptChequePrint.aspx");
    }
}