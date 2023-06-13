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
using System.Web.UI.WebControls;

public partial class Accounts_ItemSaleInvoice : System.Web.UI.Page
{
    private Common clsobj = new Common();
    private clsDAL DAL = new clsDAL();
    private long InvNo;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SSVMID"] != null)
        {
            if (Page.IsPostBack)
                return;
            btnConsolidat.Visible = true;
            btnDetail.Visible = true;
            InvNo = long.Parse(Request.QueryString["InvNo"].ToString());
            GetDetailedReport();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void GetDetailedReport()
    {
        DataSet dataSet = new DataSet();
        DataSet dataForReport = GetDataForReport();
        if (dataForReport.Tables.Count <= 0)
            return;
        DataTable table1 = dataForReport.Tables[0];
        DataRow row1 = dataForReport.Tables[1].Rows[0];
        DataTable table2 = dataForReport.Tables[2];
        string str1;
        string str2;
        string str3;
        if (table1.Rows.Count > 0)
        {
            str1 = table1.Rows[0]["FullName"].ToString();
            str2 = table1.Rows[0]["ClassName"].ToString();
            str3 = table1.Rows[0]["AdmnNo"].ToString();
        }
        else
        {
            str1 = row1["RcvdFrom"].ToString();
            str2 = "----";
            str3 = "----";
        }
        if (row1 != null)
        {
            string str4 = SchoolInformation();
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("<center>");
            stringBuilder1.Append(str4);
            stringBuilder1.Append("<table cellspacing='0' border='0' width='85%' style='font-size:14px;border-collapse:collapse;' class='tbltd'>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:25%'><strong>Date:&nbsp;</strong>" + row1["InvDate"].ToString() + "</td>");
            stringBuilder1.Append("<td  style='width:35%'><strong>Admn No.:&nbsp;</strong>" + str3 + "</td>");
            stringBuilder1.Append("<td><strong>Receipt No:</strong>&nbsp;" + row1["PhysicalBillNo"].ToString() + "</td><td></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:60%' colspan='2'><strong>Name :&nbsp;</strong>" + str1 + "</td>");
            stringBuilder1.Append("<td colspan='3'><strong>Class:</strong>&nbsp;" + str2 + "&nbsp;</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4'>&nbsp;</td></tr>");
            stringBuilder1.Append("</table>");
            stringBuilder1.Append("</center>");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<center>");
            stringBuilder2.Append("<table style='font-size:14px;border-collapse:collapse;border-color:black;' border='1' width='85%' class='tbltd' cellspacing='0' cellpadding='3px'>");
            stringBuilder2.Append("<tr style='font-weight:bold;'>");
            stringBuilder2.Append("<td align='left'>Item Name</td>");
            stringBuilder2.Append("<td align='left'>Qty</td>");
            stringBuilder2.Append("<td align='left'>Sale Price</td>");
            stringBuilder2.Append("<td align='left'>Amount</td>");
            stringBuilder2.Append("</tr>");
            foreach (DataRow row2 in (InternalDataCollectionBase)table2.Rows)
            {
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td align='left'>" + row2["ItemName"].ToString() + "</td>");
                stringBuilder2.Append("<td align='left'>" + row2["Qty"].ToString() + "</td>");
                stringBuilder2.Append("<td align='left'>" + row2["SalePrice"].ToString() + "</td>");
                stringBuilder2.Append("<td align='right'>" + row2["TotAmt"].ToString() + "</td>");
            }
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='left' style='border-right:0px;'>Total</td>");
            stringBuilder2.Append("<td colspan='3' style='border-left:0px;' align='right'>" + table2.Compute("SUM(TotAmt)", "").ToString() + "</td>");
            stringBuilder2.Append("</tr>");
            if (Decimal.Parse(row1["AddnlDiscount"].ToString()) > new Decimal(0))
            {
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td colspan='3' align='right'>Additional Discount&nbsp;</td>");
                stringBuilder2.Append("<td align='right'>" + row1["AddnlDiscount"].ToString() + "</td>");
                stringBuilder2.Append("</tr>");
            }
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%'  class='tbltd' style='font-size:14px;border-collapse:collapse;'>");
            stringBuilder2.Append("<tr><td>&nbsp;</td></tr>");
            stringBuilder2.Append("<tr><td colspan='2'></strong>&nbsp;(<strong>In Words :</strong> Rupees&nbsp;" + NumberToWords(double.Parse(row1["NetPayableAmt"].ToString())) + "Only)</td><td>&nbsp;</td></tr>");
            stringBuilder2.Append("<tr><td colspan='2'></td><td align='right' style='padding-right:10px;'><b>Receiver's Signature</b></td></tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("</center>");
            literaldata.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
            Session["PrintInvoice"] = (object)(stringBuilder1.ToString() + stringBuilder2.ToString());
        }
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('No such Invoice found !'); window.location='" + "../Accounts/Welcome.aspx" + "';", true);
    }

    private DataSet GetDataForReport()
    {
        DataSet dataSet = new DataSet();
        return DAL.GetDataSet("ACTS_GetItemSaleForInvoice", new Hashtable()
    {
      {
        (object) "@InvNo",
        (object) long.Parse(Request.QueryString["InvNo"].ToString())
      }
    });
    }

    private void GetConsolidatedReport()
    {
        DataSet dataSet = new DataSet();
        DataSet dataForReport = GetDataForReport();
        if (dataForReport.Tables.Count <= 0)
            return;
        DataTable table1 = dataForReport.Tables[0];
        DataRow row = dataForReport.Tables[1].Rows[0];
        DataTable table2 = dataForReport.Tables[2];
        string str1;
        string str2;
        string str3;
        if (table1.Rows.Count > 0)
        {
            str1 = table1.Rows[0]["FullName"].ToString();
            str2 = table1.Rows[0]["ClassName"].ToString();
            str3 = table1.Rows[0]["AdmnNo"].ToString();
        }
        else
        {
            str1 = row["RcvdFrom"].ToString();
            str2 = "----";
            str3 = "----";
        }
        if (row != null)
        {
            string str4 = SchoolInformation();
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("<center>");
            stringBuilder1.Append(str4);
            stringBuilder1.Append("<table cellspacing='0' border='0' width='85%' style='font-size:14px;border-collapse:collapse;' class='tbltd'>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:25%'><strong>Date:&nbsp;</strong>" + row["InvDate"].ToString() + "</td>");
            stringBuilder1.Append("<td  style='width:35%'><strong>Admn No.:&nbsp;</strong>" + row["AdmnNo"].ToString() + "</td>");
            stringBuilder1.Append("<td><strong>Receipt No:</strong>&nbsp;" + row["PhysicalBillNo"].ToString() + "</td><td></td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:60%' colspan='2'><strong>Name :&nbsp;</strong>" + str1 + "</td>");
            stringBuilder1.Append("<td colspan='3'><strong>Class:</strong>&nbsp;" + str2 + "&nbsp;</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4'>&nbsp;</td></tr>");
            stringBuilder1.Append("</table>");
            stringBuilder1.Append("</center>");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<center>");
            stringBuilder2.Append("<table style='font-size:16px;' border='0' width='85%' class='tbltd' cellspacing='0' cellpadding='3px'>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td style='line-height:20px;text-align:justify;'>Received <b>Rs.&nbsp;" + row["NetPayableAmt"].ToString() + "</b> (Rupees&nbsp;" + NumberToWords(double.Parse(row["NetPayableAmt"].ToString())) + "Only) from Miss/Mr.&nbsp;" + str1 + " (" + str3 + ") towards the sale of Books & Study materials.</td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%'  class='tbltd' style='font-size:14px;border-collapse:collapse;'>");
            stringBuilder2.Append("<tr><td>&nbsp;</td></tr>");
            stringBuilder2.Append("<tr><td colspan='2'></td><td align='right' style='padding-right:20px;'><b>Receiver's Signature</b></td></tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("</center>");
            Session["PrintInvoice"] = (object)(stringBuilder1.ToString() + stringBuilder2.ToString());
        }
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('No such Invoice found !'); window.location='" + "../Accounts/Welcome.aspx" + "';", true);
    }

    private string SchoolInformation()
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
                    stringBuilder.Append("<table width='85%' cellpadding='2' cellspacing='2' class='tbltd'><tr><td rowspan='4'> <img src='../images/leftlogo.jpg' width='75px' height='80px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/ ></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</b></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + clsDal.Decrypt(row["Pin"].ToString().Trim(), str2) + "</b> </td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>Phone-" + clsDal.Decrypt(row["Phone"].ToString().Trim(), str2) + "</b></td></tr><tr><td colspan='5'></td></tr></table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected void btnMainMenu_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Accounts/Welcome.aspx");
    }

    protected void btnConsolidat_Click(object sender, EventArgs e)
    {
        try
        {
            GetConsolidatedReport();
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('ItemSaleInvoicePrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnDetail_Click(object sender, EventArgs e)
    {
        GetDetailedReport();
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('ItemSaleInvoicePrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
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
            str1 = string.Format("{0} {1}{2} {3}", (object)translateWholeNumber(number1).Trim(), (object)str2, (object)str3, (object)str4);
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
        Response.Redirect("ItemSale.aspx");
    }
}