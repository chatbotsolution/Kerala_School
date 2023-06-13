using ASP;
using Classes.DA;
using SanLib;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Reports_rptPrevDueReceipt : System.Web.UI.Page
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
            btnContinueCashReceipt.Visible = true;
            RecptNo = Request.QueryString["rno"].ToString();
            getdata();
        }
        else
        {
            btnPrint.Visible = false;
            btnContinueCashReceipt.Visible = false;
        }
        if (Request.QueryString["FY"] == null)
            return;
        getdata();
    }

    private void getdata()
    {
        string str1 = Request.QueryString["sess"];
        DataTable dataTable1 = clsobj.ExecuteSql("select distinct FullName,ClassName,Section,FatherName,TelNoResidence,TeleNoOffice,convert(varchar,sm.UserDate,103) as usersdate from dbo.PS_StudMaster sm inner join dbo.PS_ClasswiseStudent cs on sm.admnno=cs.AdmnNo   and Detained_Promoted='' inner join dbo.PS_ClassMaster cm on cm.ClassID=cs.ClassID where sm.AdmnNo=" + Convert.ToInt64(Request.QueryString["admnno"]));
        if (dataTable1.Rows.Count > 0)
        {
            string str2 = Information();
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("<center>");
            stringBuilder1.Append(str2);
            stringBuilder1.Append("<table cellpadding='0' cellspacing='0' border='0' width='600px' style='font-size:14px;'>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            if (Session["rcptFee"] != null)
                Session.Remove("rcptFee");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr>");
            DataTable dataTable2 = new DataTable();
            if (Request.QueryString["D"].ToString() != "d")
            {
                dataTable2 = new clsDAL().GetDataTableQry("SELECT CONVERT(VARCHAR,TransDt,106)AS RecvDate, Description,ISNULL(CONVERT(Decimal(18,2),Amount),0) AS Fee, PmtMode, InstrumentNo as ChequeNo,convert(nvarchar,InstrumentDt,106) AS ChequeDate,DrawanOnBank,u.FullName FROM dbo.Acts_PaymentReceiptVoucher p inner join dbo.PS_USER_MASTER u on u.USER_ID=p.UserId WHERE InvoiceReceiptNo='" + Request.QueryString["rno"] + "' ");
                stringBuilder1.Append("<td style='width:25%'><strong>Date:&nbsp;</strong>" + dataTable2.Rows[0]["RecvDate"].ToString() + "</td>");
            }
            if (Request.QueryString["D"].ToString() != "d")
                stringBuilder1.Append("<td style='width:30%'><strong>Receipt No:</strong>&nbsp;" + Request.QueryString["rno"] + "</td><td>&nbsp;</td>");
            else
                stringBuilder1.Append("<td colspan='2'>&nbsp;</td>");
            stringBuilder1.Append("<td  style='width:45%'><strong>Admission No.:&nbsp;</strong>" + Request.QueryString["admnno"] + "&nbsp;&nbsp;&nbsp;</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='6'><strong>Name :&nbsp;</strong>" + dataTable1.Rows[0][0].ToString() + "&nbsp;&nbsp;&nbsp;");
            stringBuilder1.Append("<strong>Father's Name :&nbsp;</strong>" + dataTable1.Rows[0]["FatherName"].ToString() + "&nbsp;&nbsp;&nbsp;");
            stringBuilder1.Append("<strong>Class:</strong>&nbsp;" + dataTable1.Rows[0][1].ToString());
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("</tr>");
            if (Request.QueryString["D"].ToString() != "d")
            {
                stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
                if (dataTable1.Rows[0]["TelNoResidence"] != null && dataTable1.Rows[0]["TelNoResidence"].ToString().Trim() != "")
                    stringBuilder1.Append("<td colspan='4'><strong>Mobile No :</strong>&nbsp;" + dataTable1.Rows[0]["TelNoResidence"].ToString() + "&emsp; <strong>Fee Details:</strong>&nbsp;" + dataTable2.Rows[0]["Description"] + "</td>");
                else
                    stringBuilder1.Append("<td colspan='4'><strong>Mobile No :</strong>&nbsp;--&emsp; <strong>Fee Details:</strong>&nbsp;" + dataTable2.Rows[0]["Description"] + "</td>");
                stringBuilder1.Append("</tr>");
            }
            stringBuilder1.Append("</table>");
            DataTable dataTable3 = new DataTable();
            DataTable dataTable4;
            if (Request.QueryString["status"] != null)
                dataTable4 = clsobj.ExecuteSql("select convert(decimal(18,2),sum(credit)),TransDesc from dbo.PS_FeeLedger where admnno=" + Convert.ToInt32(Request.QueryString["admnno"]) + " and  Receipt_VrNo='" + RecptNo + "' and TransDesc='Previous due' group by TransDesc");
            else
                dataTable4 = clsobj.ExecuteSql("select convert(decimal(18,2),sum(Balance)), TransDesc from dbo.PS_FeeLedger where admnno=" + Convert.ToInt32(Request.QueryString["admnno"]) + " and TransDesc='Previous Balance' group by TransDesc");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<table style='font-size:14px;' border=1 width='600px' cellspacing=0 cellpadding=0><tr><td colspan=2 align=center>");
            stringBuilder2.Append("<strong>PARTICULARS</strong></td><td align=center><strong>AMOUNT</strong></td></tr>");
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
                dataTable6 = clsobj.ExecuteSql("select sum(credit) from dbo.PS_FeeLedger where admnno=" + Convert.ToInt32(Request.QueryString["admnno"]) + " and Receipt_VrNo='" + RecptNo + "' group by AdmnNo");
            else
                dataTable6 = clsobj.ExecuteSql("select sum(balance) from dbo.PS_FeeLedger where admnno=" + Convert.ToInt32(Request.QueryString["admnno"]) + "  and TransDesc='Previous Balance'");
            stringBuilder2.Append("<tr><td colspan='3'></td></tr></br>");
            stringBuilder2.Append("<tr><td colspan='2' align='right'><strong>TOTAL</strong></TD>");
            stringBuilder2.Append("<td align='right'>");
            double numb = 0.0;
            if (dataTable6.Rows.Count > 0)
            {
                numb = Convert.ToDouble(dataTable6.Rows[0][0].ToString()) + Convert.ToDouble(Session["PaidFine"]);
                dataTable6.Rows[0][0].ToString();
            }
            stringBuilder2.Append(string.Format("{0:F2}", numb));
            stringBuilder2.Append("</td></tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='600px' style='font-size:14px;'>");
            stringBuilder2.Append("<tr><td colspan='2' align='left'></strong>&nbsp;(<strong>In Words :</strong> Rupees&nbsp;" + NumberToWords(numb) + "Only)</td><td>&nbsp;</td></tr>");
            if (dataTable2.Rows.Count > 0)
            {
                stringBuilder2.Append("<tr style='height: 30px;'>");
                stringBuilder2.Append("<td align='right'>");
                stringBuilder2.Append("<b>Received By  " + dataTable2.Rows[0]["FullName"] + "</b>");
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("</tr>");
                stringBuilder2.Append("<tr style='height:30px;'><td colspan='2' ></td><td align='right'></td></tr>");
            }
            else
            {
                stringBuilder2.Append("<tr style='height: 30px;'>");
                stringBuilder2.Append("<td align='right'>");
                stringBuilder2.Append("<b>Receiver's Signature</b>");
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("</tr>");
                stringBuilder2.Append("<tr style='height:30px;'><td colspan='2' ></td><td align='right'></td></tr>");
            }
            stringBuilder2.Append("<tr><td colspan='3' align='center' style='font-size:10px'><u>This is a computer generated receipt</u></td></tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("<center>");
            literaldata.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
            Session["PrintReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
        }
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('No Pending'); window.location='" + "../FeeManagement/feercptcash.aspx" + "';", true);
    }

    private string Information()
    {
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        DataSet dataSet = new DataSet();
        StringBuilder stringBuilder = new StringBuilder();
        clsDAL clsDal = new clsDAL();
        string str1 = Server.MapPath("../XMLFiles") + "\\Detail.xml";
        if (File.Exists(str1))
        {
            int num = (int)dataSet.ReadXml(str1);
            string str2 = Session["SSVMID"].ToString();
            if (dataSet.Tables.Count > 0)
            {
                DataTable dataTable2 = new DataTable();
                DataTable table = dataSet.Tables[0];
                foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    stringBuilder.Append("<table width='85%' class='tbltd' cellpadding='2' cellspacing='2'>");
                    stringBuilder.Append("<tr><td rowspan='4'> <img src='../images/leftlogo.jpg' width='75px' height='80px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</strong></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + clsDal.Decrypt(row["Pin"].ToString().Trim(), str2) + "</strong> </td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>Phone-" + clsDal.Decrypt(row["Phone"].ToString().Trim(), str2) + "</strong></td></tr><tr><td colspan='5'></td></tr></table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
    }

    protected void btnContinueCashReceipt_Click(object sender, EventArgs e)
    {
        Response.Redirect("../FeeManagement/FeeRecptPrevDue.aspx");
    }

    protected void btnMainMenu_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Administrations/Home.aspx");
    }

    public string NumberToWords(string numb)
    {
        return changeToWords(numb, true);
    }

    public string NumberToWords(double numb)
    {
        return changeToWords(numb.ToString(), true);
    }

    private string changeToWords(string numb, bool isCurrency)
    {
        string str1 = "";
        string number1 = numb;
        string str2 = "";
        string str3 = "";
        string str4 = "";
        try
        {
            int length = numb.IndexOf(".");
            if (length > 0)
            {
                number1 = numb.Substring(0, length);
                string number2 = numb.Substring(length + 1);
                if (Convert.ToInt32(number2) > 0)
                {
                    str2 = isCurrency ? "Rupees and " : "point";
                    str4 = isCurrency ? "Paise Only " + str4 : "";
                    str3 = translateWholeNumber(number2);
                }
            }
            str1 = string.Format("{0} {1}{2} {3}", translateWholeNumber(number1).Trim(), str2, str3, str4);
        }
        catch
        {
        }
        return str1;
    }

    private string translateWholeNumber(string number)
    {
        string str1 = "";
        try
        {
            bool flag1 = false;
            if (Convert.ToDouble(number) > 0.0)
            {
                bool flag2 = number.StartsWith("0");
                int length = number.Length;
                int num = 0;
                string str2 = "";
                switch (length)
                {
                    case 1:
                        str1 = ones(number);
                        flag1 = true;
                        break;
                    case 2:
                        str1 = tens(number);
                        flag1 = true;
                        break;
                    case 3:
                        num = length % 3 + 1;
                        str2 = " Hundred ";
                        break;
                    case 4:
                    case 5:
                    case 6:
                        num = length % 4 + 1;
                        str2 = " Thousand ";
                        break;
                    case 7:
                    case 8:
                    case 9:
                        num = length % 7 + 1;
                        str2 = " Million ";
                        break;
                    case 10:
                        num = length % 10 + 1;
                        str2 = " Billion ";
                        break;
                    default:
                        flag1 = true;
                        break;
                }
                if (!flag1)
                {
                    str1 = translateWholeNumber(number.Substring(0, num)) + str2 + translateWholeNumber(number.Substring(num));
                    if (flag2)
                        str1 = " and " + str1.Trim();
                }
                if (str1.Trim().Equals(str2.Trim()))
                    str1 = "";
            }
        }
        catch
        {
        }
        return str1.Trim();
    }

    private string tens(string digit)
    {
        int int32 = Convert.ToInt32(digit);
        string str = (string)null;
        switch (int32)
        {
            case 80:
                str = "Eighty";
                break;
            case 90:
                str = "Ninety";
                break;
            case 60:
                str = "Sixty";
                break;
            case 70:
                str = "Seventy";
                break;
            case 10:
                str = "Ten";
                break;
            case 11:
                str = "Eleven";
                break;
            case 12:
                str = "Twelve";
                break;
            case 13:
                str = "Thirteen";
                break;
            case 14:
                str = "Fourteen";
                break;
            case 15:
                str = "Fifteen";
                break;
            case 16:
                str = "Sixteen";
                break;
            case 17:
                str = "Seventeen";
                break;
            case 18:
                str = "Eighteen";
                break;
            case 19:
                str = "Nineteen";
                break;
            case 20:
                str = "Twenty";
                break;
            case 30:
                str = "Thirty";
                break;
            case 40:
                str = "Fourty";
                break;
            case 50:
                str = "Fifty";
                break;
            default:
                if (int32 > 0)
                {
                    str = tens(digit.Substring(0, 1) + "0") + " " + ones(digit.Substring(1));
                    break;
                }
                break;
        }
        return str;
    }

    private string ones(string digit)
    {
        int int32 = Convert.ToInt32(digit);
        string str = "";
        switch (int32)
        {
            case 1:
                str = "One";
                break;
            case 2:
                str = "Two";
                break;
            case 3:
                str = "Three";
                break;
            case 4:
                str = "Four";
                break;
            case 5:
                str = "Five";
                break;
            case 6:
                str = "Six";
                break;
            case 7:
                str = "Seven";
                break;
            case 8:
                str = "Eight";
                break;
            case 9:
                str = "Nine";
                break;
        }
        return str;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("../FeeManagement/FeeRecptPrevDue.aspx?admnno=" + Request.QueryString["admnno"].ToString().Trim() + "&sess=" + Request.QueryString["sess"].ToString().Trim() + "&cid=" + Request.QueryString["class"].ToString().Trim() + "&D=p");
    }
}