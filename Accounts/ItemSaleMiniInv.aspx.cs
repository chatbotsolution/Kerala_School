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


public partial class Accounts_ItemSaleMiniInv : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User_Id"] != null)
        {
            if (this.Page.IsPostBack || this.Request.QueryString["InvNo"] == null)
                return;
            this.GetBill();
        }
        else
            this.Response.Redirect("../Login.aspx");
    }

    private void GetBill()
    {
        DataSet dataSet1 = new DataSet();
        DataSet dataSet2 = this.obj.GetDataSet("ACTS_GetItemSaleForInvoice", new Hashtable()
    {
      {
        (object) "@InvNo",
        (object) long.Parse(this.Request.QueryString["InvNo"].ToString())
      }
    });
        if (dataSet2.Tables.Count <= 0)
            return;
        DataTable table1 = dataSet2.Tables[0];
        DataRow row = dataSet2.Tables[1].Rows[0];
        DataTable table2 = dataSet2.Tables[2];
        DataTable table3 = dataSet2.Tables[3];
        string str1;
        string str2;
        if (table1.Rows.Count > 0)
        {
            str1 = table1.Rows[0]["FullName"].ToString();
            str2 = table1.Rows[0]["ClassName"].ToString();
            table1.Rows[0]["AdmnNo"].ToString();
        }
        else
        {
            str1 = row["RcvdFrom"].ToString();
            str2 = "----";
        }
        if (row == null)
            return;
        string str3 = this.SchoolInformation();
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append("<center>");
        //stringBuilder2.Append(str3);
        stringBuilder2.Append("<table cellspacing='0' border='0' width='85%' style='font-size:14px;border-collapse:collapse;' class='tbltd'>");
        stringBuilder2.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
        stringBuilder2.Append("<tr>");
        stringBuilder2.Append("<td colspan='4' style='width:40%;font-size:12px' align='left'>");
        stringBuilder2.Append("<strong>Name :&nbsp;</strong>" + str1);
        if (str2.Trim() != "----")
            stringBuilder2.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Class:</strong>&nbsp;" + str2 + "&nbsp;");
        stringBuilder2.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Date:&nbsp;</strong>" + row["InvDate"].ToString());
        stringBuilder2.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Receipt No:</strong>&nbsp;" + row["PhysicalBillNo"].ToString());
        stringBuilder2.Append("</td></tr>");
        stringBuilder2.Append("<tr><td></td></tr>");
        stringBuilder1.Append("<tr>");
        int num1 = table2.Rows.Count / 2;
        int index1 = 0;
        if (table2.Rows.Count > 12)
        {
            stringBuilder1.Append("<td valign='top' colspan='2'><table width='370px'>");
            stringBuilder1.Append("<tr><td align='left'><b>Item Name</b></td>");
            stringBuilder1.Append("<td align='left'><b>Qty</b></td>");
            stringBuilder1.Append("<td align='right'><b>Price</b></td>");
            stringBuilder1.Append("<td align='right'><b>Amount</b></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4'><hr /></td></tr>");
            for (index1 = 0; index1 <= num1 - 1; ++index1)
            {
                if (index1 < table2.Rows.Count)
                {
                    stringBuilder1.Append("<tr>");
                    stringBuilder1.Append("<td align='left'>" + table2.Rows[index1]["ItemName"].ToString() + "</td>");
                    stringBuilder1.Append("<td align='left'>" + table2.Rows[index1]["Qty"].ToString() + "</td>");
                    stringBuilder1.Append("<td align='right'>" + table2.Rows[index1]["SalePrice"].ToString() + "</td>");
                    stringBuilder1.Append("<td align='right'>" + table2.Rows[index1]["TotAmt"].ToString() + "</td></tr>");
                }
            }
            stringBuilder1.Append("</table></td>");
            stringBuilder1.Append("<td valign='top' colspan='2'><table width='400px'>");
        }
        else
            stringBuilder1.Append("<td colspan='4'><center><table width='100%'>");
        if (index1 <= table2.Rows.Count)
        {
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td align='left'><b>Item Name</b></td>");
            stringBuilder1.Append("<td align='left'><b>Qty</b></td>");
            stringBuilder1.Append("<td align='right'><b>Price</b></td>");
            stringBuilder1.Append("<td align='right'><b>Amount</b></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4'><hr /></td></tr>");
            for (int index2 = index1; index2 <= table2.Rows.Count; ++index2)
            {
                if (index2 < table2.Rows.Count)
                {
                    stringBuilder1.Append("<tr>");
                    stringBuilder1.Append("<td align='left'>" + table2.Rows[index2]["ItemName"].ToString() + "</td>");
                    stringBuilder1.Append("<td align='left'>" + table2.Rows[index2]["Qty"].ToString() + "</td>");
                    stringBuilder1.Append("<td align='right'>" + table2.Rows[index2]["SalePrice"].ToString() + "</td>");
                    stringBuilder1.Append("<td align='right'>" + table2.Rows[index2]["TotAmt"].ToString() + "</td></tr>");
                }
            }
            stringBuilder1.Append("</table></td>");
        }
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("<tr><td colspan='4'><hr/></td></tr>");
        if (table3.Rows.Count > 0)
        {
            stringBuilder1.Append("<tr align='left'><td colspan='3'><strong>Sub Total&nbsp;:&nbsp;" + table2.Compute("SUM(TotAmt)", "").ToString());
            stringBuilder1.Append(",&nbsp;Return Amt Agnst This Bill&nbsp;:&nbsp;" + table3.Rows[0]["TotBillAmt"].ToString());
            Decimal num2 = Convert.ToDecimal(row["NetPayableAmt"].ToString()) - Convert.ToDecimal(table3.Rows[0]["TotBillAmt"].ToString());
            stringBuilder1.Append(",&nbsp;Total : " + (object)num2 + "&nbsp;(Rupees&nbsp;" + this.NumberToWords(double.Parse(num2.ToString())) + "Only)</strong></td></tr>");
        }
        else
            stringBuilder1.Append("<tr align='left'><td colspan='3'><strong>Total : " + table2.Compute("SUM(TotAmt)", "").ToString() + "&nbsp;(Rupees&nbsp;" + this.NumberToWords(double.Parse(row["NetPayableAmt"].ToString())) + "Only)</strong></td><td>&nbsp;</td></tr>");
        stringBuilder1.Append("<tr><td colspan='4' align='right' style='padding-right:10px;'><b>Receiver's Signature</b></td></tr>");
        stringBuilder1.Append("</table>");
        stringBuilder1.Append("</center>");
        this.lblBill.Text = stringBuilder2.ToString() + stringBuilder1.ToString();
    }

    private string SchoolInformation()
    {
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        DataSet dataSet = new DataSet();
        StringBuilder stringBuilder = new StringBuilder();
        clsDAL clsDal = new clsDAL();
        string str1 = this.Server.MapPath("../XMLFiles") + "\\Detail.xml";
        if (File.Exists(str1))
        {
            int num = (int)dataSet.ReadXml(str1);
            string str2 = this.Session["SSVMID"].ToString();
            if (dataSet.Tables.Count > 0)
            {
                DataTable dataTable2 = new DataTable();
                DataTable table = dataSet.Tables[0];
                foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    stringBuilder.Append("<table width='70%' cellpadding='0' cellspacing='0' class='tbltd'><tr><td rowspan='4'> <img src='../images/leftlogo.jpg' width='120px' height='111px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:18px; white-space:nowrap;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='55px' height='60px'/ ></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:12px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</b></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:12px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + clsDal.Decrypt(row["Pin"].ToString().Trim(), str2) + "</b> </td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:12px;'><b>Phone-" + clsDal.Decrypt(row["Phone"].ToString().Trim(), str2) + "</b></td></tr></table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    public string NumberToWords(string numb)
    {
        return this.changeToWords(numb, true);
    }

    public string NumberToWords(double numb)
    {
        return this.changeToWords(numb.ToString(), true);
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
                    str3 = this.translateWholeNumber(number2);
                }
            }
            str1 = string.Format("{0} {1}{2} {3}", (object)this.translateWholeNumber(number1).Trim(), (object)str2, (object)str3, (object)str4);
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
                        str1 = this.ones(number);
                        flag1 = true;
                        break;
                    case 2:
                        str1 = this.tens(number);
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
                    str1 = this.translateWholeNumber(number.Substring(0, num)) + str2 + this.translateWholeNumber(number.Substring(num));
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
                    str = this.tens(digit.Substring(0, 1) + "0") + " " + this.ones(digit.Substring(1));
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