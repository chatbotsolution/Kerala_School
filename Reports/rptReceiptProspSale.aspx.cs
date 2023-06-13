using ASP;
using Classes.DA;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptReceiptProspSale : System.Web.UI.Page
{
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Request.QueryString["mrno"] == null)
            return;
        FillData();
    }

    private void FillData()
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("Ps_Sp_IndivProspectus", new Hashtable()
    {
      {
         "@MR_No",
         Request.QueryString["mrno"].ToString()
      }
    });
        if (dataTable2.Rows.Count <= 0)
            return;
        getdata(dataTable2);
    }

    private void getdata(DataTable dt)
    {
        string str = Information();
        StringBuilder stringBuilder1 = new StringBuilder();
        stringBuilder1.Append("<center>");
        stringBuilder1.Append(str);
        stringBuilder1.Append("<table cellpadding='0' cellspacing='0' border='0' width='470px' style='font-size:11px;' class='tbltd'>");
        stringBuilder1.Append("<tr>");
        stringBuilder1.Append("<td style='width:100%' align='left' colspan='4'><strong>Date:&nbsp;</strong>" + DateTime.Now.ToString("dd-MM-yyyy") + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
        stringBuilder1.Append("<strong>Receipt No:</strong>&nbsp;" + dt.Rows[0]["MR_No"].ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
        stringBuilder1.Append("<strong>Sl.No:</strong>&nbsp;" + dt.Rows[0]["ProspectusSlNo"].ToString() + "</td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("<tr>");
        stringBuilder1.Append("<td colspan='4' style='height:5px'></td></tr>");
        stringBuilder1.Append("<tr>");
        stringBuilder1.Append("<td style='width:70%'  align='left' colspan='3'><strong>Name of the Pupil:&nbsp;</strong>" + dt.Rows[0]["StudentName"].ToString() + "</td>");
        stringBuilder1.Append("<td align='left' colspan='1'><strong>Class:</strong>&nbsp;" + dt.Rows[0]["ClassName"].ToString() + "</td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
        stringBuilder1.Append("</table>");
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append("<table style='font-size:11px;' border=1 width='470px' cellspacing=0 cellpadding=0 class='tbltd'><tr><td colspan=3 align=center>");
        stringBuilder2.Append("<strong>PARTICULARS</strong></td><td align=center><strong>AMOUNT</strong></td></tr>");
        stringBuilder2.Append("<tr><td align='center' style='width: 50px; border-right:0; border-bottom:0; border-top:0;'>1</td><td style='border-right:0;border-bottom:0;border-top:0; border-left:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
        stringBuilder2.Append("Prospectus Sale");
        stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
        stringBuilder2.Append(".............");
        stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
        stringBuilder2.Append(dt.Rows[0]["Amount"].ToString());
        stringBuilder2.Append("</td></tr>");
        stringBuilder2.Append("<tr></tr><tr></tr><tr></tr><tr></tr>");
        stringBuilder2.Append("<tr><td colspan='3'></td></tr>");
        stringBuilder2.Append("<tr><td colspan='3' align='right'><strong>TOTAL</strong></TD>");
        stringBuilder2.Append("<td align='right'>");
        double numb = 0.0;
        if (dt.Rows.Count > 0)
            numb = Convert.ToDouble(dt.Rows[0]["Amount"].ToString());
        stringBuilder2.Append(string.Format("{0:F2}", numb));
        stringBuilder2.Append("</td></tr>");
        stringBuilder2.Append("</table>");
        stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='470px' class='tbltd'>");
        stringBuilder2.Append("<tr><td  style='font-size:12px'></strong>&nbsp;(In Words: Rupees&nbsp;<strong>" + NumberToWords(numb) + "</strong>Only)</td><td>&nbsp;</td></tr>");
        stringBuilder2.Append("<tr><td colspan='2'></td><td align='right'  style='font-size:11px'><b><br />Received By " + dt.Rows[0]["FullName"].ToString() + "</b></td></tr>");
        stringBuilder2.Append("<tr><td colspan='3' align='center' style='font-size:10px'><u>This is a computer generated receipt</u></td></tr>");
        stringBuilder2.Append("</table>");
        stringBuilder2.Append("</center>");
        lbldetail.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
        Session["PrintReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
    }

    private string Information()
    {
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
                DataTable dataTable = new DataTable();
                DataTable table = dataSet.Tables[0];
                foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    stringBuilder.Append("<table width='470px' cellpadding='2' cellspacing='2'  style='border-bottom:solid 1px black;'><tr><td rowspan='4'> <img src='../images/leftlogo.jpg' width='65px' height='70px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:14px;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='75px' height='70px' /></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:11px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</b></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:11px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + clsDal.Decrypt(row["Pin"].ToString().Trim(), str2) + "</b> </td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:11px;'><b>Phone-" + clsDal.Decrypt(row["Phone"].ToString().Trim(), str2) + "</b></td></tr><tr><td colspan='5'></td></tr></table>");
                }
            }
        }
        return stringBuilder.ToString();
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
}