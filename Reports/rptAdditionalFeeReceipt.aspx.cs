using ASP;
using Classes.DA;
using SanLib;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptAdditionalFeeReceipt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnprint.Visible = false;
        if (Request.QueryString["id"] != null)
        {
            getDetails(Request.QueryString["id"].ToString().Trim());
        }
        else
        {
            if (Request.QueryString["admino"] == null || Request.QueryString["FY"] == null)
                return;
            if (Request.QueryString["FY"] == "y")
                GetDataFullYr(Request.QueryString["admino"].ToString().Trim());
            else
                getreport(Request.QueryString["admino"].ToString().Trim());
        }
    }

    private void GetDataFullYr(string studAdmnno)
    {
        tdreport.Visible = false;
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        if (Request.QueryString["BH"] == null)
            return;
        DataTable dataTable2 = !(Request.QueryString["BH"] == "B") ? (Session["HostelFeeMonth"] == null ? common.ExecuteSql("Select AdFeeDesc as ad_Description,Convert(Decimal(18,2),isnull(balance,0)) as Balance, convert(varchar,AdFeeDate,106) as date  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId  where  balance > 0 and Ad_Id=2  and admnno=" + studAdmnno.ToString().Trim()) : common.ExecuteSql("Select AdFeeDesc as ad_Description,Convert(Decimal(18,2),isnull(balance,0)) as Balance, convert(varchar,AdFeeDate,106) as date  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId  where  balance > 0 and Ad_Id=2  and admnno=" + studAdmnno.ToString().Trim())) : (Session["BusFeeMonth"] == null ? common.ExecuteSql("Select AdFeeDesc as ad_Description,Convert(Decimal(18,2),isnull(balance,0)) as Balance, convert(varchar,AdFeeDate,106) as date  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId  where  balance > 0 and Ad_Id=1   and admnno=" + studAdmnno.ToString().Trim()) : common.ExecuteSql("Select AdFeeDesc as ad_Description,Convert(Decimal(18,2),isnull(balance,0)) as Balance, convert(varchar,AdFeeDate,106) as date  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId  where  balance > 0 and Ad_Id=1  and admnno=" + studAdmnno.ToString().Trim()));
        if (dataTable2.Rows.Count > 0)
        {
            double num = 0.0;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table cellpadding='1' cellspacing='1' border='0' width='100%' style='background-color:#CCC;'>");
            stringBuilder.Append("<tr><td style='width:100px;' class='tblheader'>Due Date</td><td class='tblheader'>Additional Charges</td><td style='width: 100px;' class='tblheader'>Balance Amount</td></tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                stringBuilder.Append("<tr><td class='tbltd'>");
                stringBuilder.Append(row["date"].ToString().Trim());
                stringBuilder.Append("</td><td class='tbltd'>");
                stringBuilder.Append(row["ad_Description"].ToString().Trim());
                stringBuilder.Append("</td><td style='text-align:right;' class='tbltd'>");
                stringBuilder.Append(row["Balance"].ToString().Trim());
                stringBuilder.Append("</td></tr>");
                num += Convert.ToDouble(row["Balance"].ToString().Trim());
            }
            stringBuilder.Append("<tr><td style='text-align:right;' colspan='2' class='tbltd'><strong class='error'>Total Amount</strong></td><td style='text-align:right;' class='tbltd'><strong class='error'>" + num.ToString().Trim() + ".00</strong></td></tr>");
            stringBuilder.Append("</table>");
            lblDetails.Text = stringBuilder.ToString().Trim();
        }
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table cellpadding='0' cellspacing='0' border='0' width='60%'>");
            stringBuilder.Append("<tr><td style='color:Red;' style='text-align:center;'>No Additional Charges Exist</td></tr>");
            stringBuilder.Append("</table>");
            lblDetails.Text = stringBuilder.ToString().Trim();
        }
    }

    private void getreport(string studAdmnno)
    {
        tdreport.Visible = false;
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        if (Request.QueryString["BH"] == null)
            return;
        DataTable dataTable2;
        if (Request.QueryString["BH"] == "B")
        {
            if (Session["BusFeeMonth"] != null)
                dataTable2 = common.ExecuteSql("Select AdFeeDesc as ad_Description,Convert(Decimal(18,2),isnull(balance,0)) as Balance, convert(varchar,AdFeeDate,106) as date  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId  where  balance > 0 and Ad_Id=1 and AdFeeDate <= '" + Session["BusFeeMonth"] + "'  and admnno=" + studAdmnno.ToString().Trim());
            else
                dataTable2 = common.ExecuteSql("Select AdFeeDesc as ad_Description,Convert(Decimal(18,2),isnull(balance,0)) as Balance, convert(varchar,AdFeeDate,106) as date  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId  where  balance > 0 and Ad_Id=1 and AdFeeDate <= getdate()  and admnno=" + studAdmnno.ToString().Trim() + " order by AdFeeDate");
        }
        else if (Session["HostelFeeMonth"] != null)
            dataTable2 = common.ExecuteSql("Select AdFeeDesc as ad_Description,Convert(Decimal(18,2),isnull(balance,0)) as Balance, convert(varchar,AdFeeDate,106) as date  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId  where  balance > 0 and Ad_Id=2 and AdFeeDate <= '" + Session["HostelFeeMonth"] + "'  and admnno=" + studAdmnno.ToString().Trim());
        else
            dataTable2 = common.ExecuteSql("Select AdFeeDesc as ad_Description,Convert(Decimal(18,2),isnull(balance,0)) as Balance, convert(varchar,AdFeeDate,106) as date  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId  where  balance > 0 and Ad_Id=2 and AdFeeDate <= getdate()  and admnno=" + studAdmnno.ToString().Trim() + " order by AdFeeDate");
        if (dataTable2.Rows.Count > 0)
        {
            double num = 0.0;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table cellpadding='1' cellspacing='1' border='0' width='100%' style='background-color:#CCC;'>");
            stringBuilder.Append("<tr><td style='width:100px;' class='tblheader'>Due Date</td><td class='tblheader'>Additional Charges</td><td style='width: 100px;' class='tblheader'>Balance Amount</td></tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                stringBuilder.Append("<tr><td class='tbltd'>");
                stringBuilder.Append(row["date"].ToString().Trim());
                stringBuilder.Append("</td><td class='tbltd'>");
                stringBuilder.Append(row["ad_Description"].ToString().Trim());
                stringBuilder.Append("</td><td style='text-align:right;' class='tbltd'>");
                stringBuilder.Append(row["Balance"].ToString().Trim());
                stringBuilder.Append("</td></tr>");
                num += Convert.ToDouble(row["Balance"].ToString().Trim());
            }
            stringBuilder.Append("<tr><td style='text-align:right;' colspan='2' class='tbltd'><strong class='error'>Total Amount</strong></td><td style='text-align:right;' class='tbltd'><strong class='error'>" + num.ToString().Trim() + ".00</strong></td></tr>");
            stringBuilder.Append("</table>");
            lblDetails.Text = stringBuilder.ToString().Trim();
        }
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table cellpadding='0' cellspacing='0' border='0' width='60%'>");
            stringBuilder.Append("<tr><td style='color:Red;' style='text-align:center;'>No Additional Charges Exist</td></tr>");
            stringBuilder.Append("</table>");
            lblDetails.Text = stringBuilder.ToString().Trim();
        }
    }

    private void getDetails(string studAdmnno)
    {
        Common common = new Common();
        string str1 = Request.QueryString["session"];
        string str2 = Request.QueryString["recieptno"];
        int int32 = Convert.ToInt32(Request.QueryString["id"].ToString());
        DataTable dataTable1 = common.ExecuteSql("select distinct FullName,ClassName,Section,FatherName from dbo.PS_StudMaster sm inner join dbo.PS_ClasswiseStudent cs on sm.admnno=cs.AdmnNo inner join dbo.PS_ClassMaster cm on cm.ClassID=cs.ClassID where cs.SessionYear='" + str1 + "' and sm.AdmnNo=" + studAdmnno.ToString().Trim());
        if (dataTable1.Rows.Count > 0)
        {
            tdreport.Visible = true;
            btnprint.Visible = true;
            common.ExecuteSql("select datename(month,(max(AdFeeDate))) from dbo.PS_AdFeeLedger af inner join PS_ClasswiseStudent cs on cs.id=af.ClassWiseId where cs.admnno=" + int32 + " and af.debit > 0  and af.Receipt_No='" + str2 + "' ");
            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
            DateTime dateTime1 = Convert.ToDateTime(common.ExecuteSql("select max(AdFeeDate) as adTransDate from dbo.PS_AdFeeLedger af inner join PS_ClasswiseStudent cs on cs.id=af.ClassWiseId where cs.admnno=" + int32 + " and debit > 0 ").Rows[0]["adTransDate"]);
            DateTime dateTime2 = dateTime1.AddMonths(1);
            string str3 = dateTimeFormatInfo.GetMonthName(dateTime1.Month) + "-" + dateTimeFormatInfo.GetMonthName(dateTime2.Month);
            string str4 = Information();
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append(str4);
            stringBuilder1.Append("<table cellpadding='0' cellspacing='0' border='0' width='490px' style='font-size:14px;'>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='4' ><strong>Date:&nbsp;</strong>" + DateTime.Now.ToString("dd-MM-yyyy"));
            stringBuilder1.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Receipt No:</strong>" + str2 + "</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'>&nbsp;</td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='4'><strong>Name of the Pupil:&nbsp;</strong>" + dataTable1.Rows[0][0].ToString());
            stringBuilder1.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Class:</strong>&nbsp;" + dataTable1.Rows[0][1].ToString() + "</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("</table>");
            DataTable dataTable2 = new DataTable();
            DataTable dataTable3 = common.ExecuteSql("SELECT convert(decimal(18,2),sum(fl.credit)),fl.AdFeeDesc FROM dbo.PS_AdFeeLedger fl inner join dbo.PS_FeeComponents fs on fl.Ad_Id=fs.FeeID inner join PS_ClasswiseStudent cs on cs.Id=fl.ClassWiseId WHERE cs.admnno=" + int32 + " and fs.PeriodicityID<=2 and fl.Receipt_No='" + str2 + "' group by fl.AdFeeDesc ");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<table style='font-size:14px;' border=1 width='490px' cellspacing=0 cellpadding=0><tr><td colspan=2 align=center>");
            stringBuilder2.Append("<strong>PARTICULARS</strong></td><td align='right'><strong>AMOUNT</strong></td></tr>");
            for (int index = 0; index <= dataTable3.Rows.Count - 1; ++index)
            {
                stringBuilder2.Append("<tr><td style='border-right:0;border-bottom:0;border-top:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
                stringBuilder2.Append(dataTable3.Rows[index][1].ToString());
                stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
                stringBuilder2.Append(".............");
                stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
                stringBuilder2.Append(dataTable3.Rows[index][0].ToString());
                stringBuilder2.Append("</td></tr>");
            }
            DataTable dataTable4 = new DataTable();
            DataTable dataTable5 = common.ExecuteSql("select convert(decimal(18,2),sum(fl.credit)),ad.Ad_Description,fl.AdFeeDesc,cs.admnno from dbo.PS_AdFeeLedger fl join dbo.PS_FeeComponents fs on fl.Ad_Id=fs.FeeID inner join PS_ClasswiseStudent cs on cs.Id=fl.ClassWiseId inner join PS_AdditionalFeeMaster ad on fl.Ad_Id=ad.Ad_Id where cs.admnno=" + int32 + " and fs.PeriodicityID>2 and fl.Receipt_No='" + str2 + "' group by cs.AdmnNo,fl.AdFeeDesc,ad.Ad_Description order by fl.AdFeeDesc");
            stringBuilder2.Append("<tr></tr><tr></tr><tr></tr><tr></tr>");
            for (int index = 0; index <= dataTable5.Rows.Count - 1; ++index)
            {
                stringBuilder2.Append("<tr><td style='border-right:0;border-bottom:0;border-top:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
                stringBuilder2.Append(dataTable5.Rows[index][1].ToString());
                stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
                stringBuilder2.Append("..........");
                stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
                stringBuilder2.Append(dataTable5.Rows[index][0].ToString());
                stringBuilder2.Append("</td></tr>");
            }
            DataTable dataTable6 = new DataTable();
            DataTable dataTable7 = common.ExecuteSql("select sum(credit) from dbo.PS_AdFeeLedger fl join dbo.PS_FeeComponents fs on fl.Ad_Id=fs.FeeID inner join PS_ClasswiseStudent cs on cs.Id=fl.ClassWiseId where cs.admnno=" + int32 + " and fl.Receipt_No='" + str2 + "' group by cs.AdmnNo,fl.AdFeeDesc order by fl.AdFeeDesc");
            stringBuilder2.Append("<tr><td colspan='3'></td></tr></br>");
            stringBuilder2.Append("<tr><td colspan='2' align='right'><strong>TOTAL</strong></TD>");
            stringBuilder2.Append("<td align='right'>");
            double numb = 0.0;
            if (dataTable7.Rows.Count > 0)
            {
                numb = Convert.ToDouble(dataTable7.Rows[0][0].ToString());
                dataTable7.Rows[0][0].ToString();
            }
            double num1 = Convert.ToDouble(common.ExecuteScalarQry("select AdditinalFeeCredit from PS_StudCreditLedger where AdmnNo=" + int32).ToString());
            double num2 = numb;
            if (num1 != 0.0)
                num2 += num1;
            stringBuilder2.Append(string.Format("{0:F2}", num2));
            stringBuilder2.Append("</td></tr></table>");
            stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='490px' style='font-size:14px;'>");
            stringBuilder2.Append("<tr><td>&nbsp;</td></tr>");
            stringBuilder2.Append("<tr><td colspan='2'></strong>&nbsp;(In Words Rupees&nbsp;<strong>" + NumberToWords(numb) + "</strong>Only)</td><td>&nbsp;</td></tr>");
            stringBuilder2.Append("<tr><td colspan='2'></td><td><b>Cashier</b></td></tr>");
            stringBuilder2.Append("</table>");
            literaldata.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
            Session["PrintAddReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
        }
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table cellpadding='0' cellspacing='0' border='0' width='60%'>");
            stringBuilder.Append("<tr><td style='color:Red;' style='text-align:center;'>No Additional Charges Exist</td></tr>");
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            tdreport.Visible = false;
        }
    }

    private string Information()
    {
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        DataSet dataSet = new DataSet();
        StringBuilder stringBuilder = new StringBuilder();
        clsDAL clsDal = new clsDAL();
        string str = Server.MapPath("../XMLFiles") + "\\Detail.xml";
        if (File.Exists(str))
        {
            int num = (int)dataSet.ReadXml(str);
            if (dataSet.Tables.Count > 0)
            {
                DataTable dataTable2 = new DataTable();
                DataTable table = dataSet.Tables[0];
                foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    stringBuilder.Append("<table width='490px'><tr><td rowspan='3'> <img src='../images/leftlogo.jpg' width='75px' height='80px'/>");
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='center' style='font-size:20px;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), "SIPL") + "</strong></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), "SIPL") + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), "SIPL") + "</b></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), "SIPL") + ",Phone-" + clsDal.Decrypt(row["Phone"].ToString().Trim(), "SIPL") + "</b>");
                    stringBuilder.Append("</td></tr>");
                    stringBuilder.Append("</table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["dup"] != null)
            Response.Redirect("rptAdditionalFeeReceiptPrint.aspx?dup=" + 1);
        else
            Response.Redirect("rptAdditionalFeeReceiptPrint.aspx");
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
        Response.Redirect("../FeeManagement/feercptcash.aspx?admnno=" + Request.QueryString["admino"] + "&sess=" + Request.QueryString["sess"] + "&cid=" + Request.QueryString["class"]);
    }
}
