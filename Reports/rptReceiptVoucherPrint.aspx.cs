using ASP;
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

public partial class Reports_rptReceiptVoucherPrint : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
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
        DataTable dataTable2 = obj.GetDataTable("Acts_GetRcptVchrPrnt", new Hashtable()
    {
      {
         "@PR_Id",
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
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<center>");
        stringBuilder.Append(str);
        stringBuilder.Append("<table width='92%' cellpadding='4' cellspacing='4'>");
        stringBuilder.Append("<tr><td style='font-size: 16px;'>");
        stringBuilder.Append("Receipt No: <font style='font-family:Arial Rounded MT Bold; font-size:18px;'>" + dt.Rows[0]["InvoiceReceiptNo"].ToString() + "</font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td style='font-size: 16px;' align='right'>");
        stringBuilder.Append("Date : " + dt.Rows[0]["TransDtStr"].ToString());
        stringBuilder.Append("</td></tr>");
        stringBuilder.Append("<tr><td style='font-size: 16px;  line-height:25px;' colspan='2'>Received with thanks from Sri/Smt <span style='border-bottom:dotted 1px black;'>&nbsp;<b>" + dt.Rows[0]["RcvdFrom"].ToString() + "</b>&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;</span><br />");
        stringBuilder.Append(" the sum of&nbsp;&nbsp;");
        stringBuilder.Append(" <span style='border-bottom:dotted 1px black;'><b> Rupees&nbsp; " + NumberToText(dt.Rows[0]["Amount"].ToString()) + "  only </b></span> towards " + dt.Rows[0]["Particulars"].ToString() + " by <span style='border-bottom:dotted 1px black;'><b>" + dt.Rows[0]["PmtMode"].ToString() + "</b></span></i>");
        if (dt.Rows[0]["PmtMode"].ToString().Trim() != "Cash")
            stringBuilder.Append("vide Instrument No : <span style='border-bottom:dotted 1px black;'><b>" + dt.Rows[0]["InstrumentNo"].ToString() + "</b></span>,  Dated : <span style='border-bottom:dotted 1px black;'><b>" + dt.Rows[0]["InstrumentDt"].ToString() + "</b></span> , Bank Name : <span style='border-bottom:dotted 1px black;'><b>" + dt.Rows[0]["DrawanOnBank"].ToString() + "</b></span>");
        stringBuilder.Append(" &nbsp; .</td></tr><tr><td style='width: 70%;' align='left' valign='top'><br /><div style='width:155px; font-family:Arial Rounded MT Bold; border:solid 1px black; height:30px;'><div style='width:35px; float:left; border-right:solid 1px black; height:30px;'><img src='../images/Rs.jpg' width='35px' height='30px'/></div><div style='float:right;padding-top:4px; font-size:18px;'><b> " + dt.Rows[0]["Amount"].ToString() + "&nbsp;</b></div></div></td><td align='center' style='font-size: 15px;'>  <br /> <br /> <br /><br />Receiver's<br />  Signature</td></tr></table>");
        stringBuilder.Append("</center>");
        lbldetail.Text = stringBuilder.ToString();
        Session["PrintReciept"] = stringBuilder.ToString();
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
                    stringBuilder.Append("<table width='92%' cellpadding='2' cellspacing='2'  style='border-bottom:solid 1px black;'><tr><td rowspan='4'> <img src='../images/leftlogo.jpg' width='75px' height='80px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:22px;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/ ></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</b></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + clsDal.Decrypt(row["Pin"].ToString().Trim(), str2) + "</b> </td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>Phone-" + clsDal.Decrypt(row["Phone"].ToString().Trim(), str2) + "</b></td></tr><tr><td colspan='5'></td></tr></table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    public string NumberToText(string strNum)
    {
        int num1 = (int)Math.Round(double.Parse(strNum));
        switch (num1)
        {
            case 0:
                return "Zero";
            case int.MinValue:
                return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";
            default:
                int[] numArray = new int[4];
                int num2 = 0;
                StringBuilder stringBuilder = new StringBuilder();
                if (num1 < 0)
                {
                    stringBuilder.Append("Minus ");
                    num1 = -num1;
                }
                string[] strArray1 = new string[10]
        {
          "",
          "One ",
          "Two ",
          "Three ",
          "Four ",
          "Five ",
          "Six ",
          "Seven ",
          "Eight ",
          "Nine "
        };
                string[] strArray2 = new string[10]
        {
          "Ten ",
          "Eleven ",
          "Twelve ",
          "Thirteen ",
          "Fourteen ",
          "Fifteen ",
          "Sixteen ",
          "Seventeen ",
          "Eighteen ",
          "Nineteen "
        };
                string[] strArray3 = new string[8]
        {
          "Twenty ",
          "Thirty ",
          "Forty ",
          "Fifty ",
          "Sixty ",
          "Seventy ",
          "Eighty ",
          "Ninety "
        };
                string[] strArray4 = new string[3]
        {
          "Thousand ",
          "Lakh ",
          "Crore "
        };
                numArray[0] = num1 % 1000;
                numArray[1] = num1 / 1000;
                numArray[2] = num1 / 100000;
                numArray[1] = numArray[1] - 100 * numArray[2];
                numArray[3] = num1 / 10000000;
                numArray[2] = numArray[2] - 100 * numArray[3];
                for (int index = 3; index > 0; --index)
                {
                    if (numArray[index] != 0)
                    {
                        num2 = index;
                        break;
                    }
                }
                for (int index1 = num2; index1 >= 0; --index1)
                {
                    if (numArray[index1] != 0)
                    {
                        int index2 = numArray[index1] % 10;
                        int num3 = numArray[index1] / 10;
                        int index3 = numArray[index1] / 100;
                        int num4 = num3 - 10 * index3;
                        if (index3 > 0)
                            stringBuilder.Append(strArray1[index3] + "Hundred ");
                        if (index2 > 0 || num4 > 0)
                        {
                            if (num4 == 0)
                                stringBuilder.Append(strArray1[index2]);
                            else if (num4 == 1)
                                stringBuilder.Append(strArray2[index2]);
                            else
                                stringBuilder.Append(strArray3[num4 - 2] + strArray1[index2]);
                        }
                        if (index1 != 0)
                            stringBuilder.Append(strArray4[index1 - 1]);
                    }
                }
                return stringBuilder.ToString().TrimEnd();
        }
    }
}