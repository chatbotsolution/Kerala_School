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

public partial class Hostel_rptHostFeeReceipt : System.Web.UI.Page
{
    private Common clsobj = new Common();
    private string RecptNo = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SSVMID"] != null)
        {
            if (Page.IsPostBack)
                return;
            if (Request.QueryString["status"] != null)
            {
                btnContinueCashReceipt.Visible = true;
                btnConsolidat.Visible = true;
                btnDetail.Visible = true;
                RecptNo = Request.QueryString["rno"].ToString();
                getdata();
                getConsolidated();
            }
            else
            {
                btnContinueCashReceipt.Visible = false;
                btnConsolidat.Visible = false;
                btnDetail.Visible = false;
            }
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void getdata()
    {
        DataTable dataTable1 = clsobj.ExecuteSql("select distinct FullName,ClassName,Section,FatherName,TelNoResidence,TeleNoOffice,convert(varchar,sm.UserDate,103) as usersdate from dbo.PS_StudMaster sm inner join dbo.PS_ClasswiseStudent cs on sm.admnno=cs.AdmnNo inner join dbo.PS_ClassMaster cm on cm.ClassID=cs.ClassID where cs.SessionYear='" + Request.QueryString["sess"] + "' and sm.AdmnNo=" + Convert.ToInt64(Request.QueryString["admnno"]));
        if (dataTable1.Rows.Count > 0)
        {
            string str1 = Information();
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("<center>");
            stringBuilder1.Append(str1);
            stringBuilder1.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:14px;' class='tbltd'>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            if (Session["rcptFee"] != null)
                Session.Remove("rcptFee");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            string str2 = "";
            if (Request.QueryString["rcptdt"] != null)
                str2 = Request.QueryString["rcptdt"].ToString();
            stringBuilder1.Append("<td style='width:25%'><strong>Date:&nbsp;</strong>" + str2 + "</td>");
            if (Request.QueryString["D"] != null && Request.QueryString["D"].ToString().ToLower() != "d")
                stringBuilder1.Append("<td><strong>Receipt No:</strong>&nbsp;" + Request.QueryString["rno"] + "</td>&nbsp;&nbsp;");
            else
                stringBuilder1.Append("<td colspan='2'>&nbsp;&nbsp;&nbsp;</td>");
            stringBuilder1.Append("<td  style='width:35%'><strong>Admn No.:&nbsp;</strong>" + Convert.ToInt64(Request.QueryString["admnno"]) + "&nbsp;</td><td>&nbsp;</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='6'><strong>Name :&nbsp;</strong>" + dataTable1.Rows[0][0].ToString() + "&nbsp;&nbsp;&nbsp;");
            stringBuilder1.Append("<strong>Father's Name :&nbsp;</strong>" + dataTable1.Rows[0]["FatherName"].ToString() + "&nbsp;&nbsp;&nbsp;");
            stringBuilder1.Append("<strong>Class:</strong>&nbsp;" + dataTable1.Rows[0][1].ToString());
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("</tr>");
            DataTable dataTable2 = new DataTable();
            if (Request.QueryString["D"] != null && Request.QueryString["D"].ToString() != "d")
            {
                Common common = new Common();
                dataTable2 = new clsDAL().GetDataTableQry("select p.Description,u.FullName from dbo.Acts_PaymentReceiptVoucher p inner join dbo.PS_USER_MASTER u on u.USER_ID=p.UserId where InvoiceReceiptNo= '" + Request.QueryString["rno"].ToString() + "'");
                stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
                stringBuilder1.Append("<tr>");
                if (dataTable1.Rows[0]["TelNoResidence"] != null && dataTable1.Rows[0]["TelNoResidence"].ToString().Trim() != "")
                    stringBuilder1.Append("<td colspan='4'><strong>Mobile No :</strong>&nbsp;" + dataTable1.Rows[0]["TelNoResidence"].ToString() + "&emsp; <strong>Fee Details:</strong>&nbsp;" + dataTable2.Rows[0]["Description"] + "</td>");
                else
                    stringBuilder1.Append("<td colspan='4'><strong>Mobile No :</strong>&nbsp;--&emsp; <strong>Fee Details:</strong>&nbsp;</td>");
                //stringBuilder1.Append("<td colspan='4'><strong>Mobile No :</strong>&nbsp;--&emsp; <strong>Fee Details:</strong>&nbsp;" + dataTable2.Rows[0]["Description"] + "</td>");
                stringBuilder1.Append("</tr>");
            }
            stringBuilder1.Append("</table>");
            stringBuilder1.Append("</center>");
            DataTable dataTable3 = new DataTable();
            DataTable dataTable4;
            if (Request.QueryString["status"] != null)
                dataTable4 = clsobj.ExecuteSql("select convert(decimal(18,2),sum(credit)),fs.FeeName as TransDesc,admnno,fs.FeeID from dbo.Host_FeeLedger fl join dbo.Host_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt64(Request.QueryString["admnno"]) + " and PeriodicityID<=2 and Receipt_VrNo='" + RecptNo + "' group by AdmnNo,fs.FeeID,fs.FeeName order by fs.FeeID,fs.FeeName");
            else
                dataTable4 = clsobj.ExecuteSql("select convert(decimal(18,2),sum(Balance)),fs.FeeName as TransDesc,admnno,fs.FeeID from dbo.Host_FeeLedger fl join dbo.Host_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt64(Request.QueryString["admnno"]) + " and PeriodicityID<=2  and TransDate <= getdate() and fl.TransDesc<>'AC Initialized' group by AdmnNo,fs.FeeID,fs.FeeName order by fs.FeeID,fs.FeeName");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<center>");
            stringBuilder2.Append("<table style='font-size:14px;' border=1 width='85%' class='tbltd' cellspacing=0 cellpadding=0><tr><td colspan=2 align=center>");
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
                dataTable6 = clsobj.ExecuteSql("select convert(decimal(18,2),sum(credit)),fs.FeeName as TransDesc,admnno from dbo.Host_FeeLedger fl join dbo.Host_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt64(Request.QueryString["admnno"]) + " and PeriodicityID>2 and Receipt_VrNo='" + RecptNo + "' group by AdmnNo,fs.FeeName order by fs.FeeName");
            else
                dataTable6 = clsobj.ExecuteSql("select convert(decimal(18,2),sum(Balance)),fs.FeeName as TransDesc,admnno from dbo.Host_FeeLedger fl join dbo.Host_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt64(Request.QueryString["admnno"]) + " and PeriodicityID>2  and TransDate <= getdate() group by AdmnNo,fs.FeeName order by fs.FeeName");
            stringBuilder2.Append("<tr><td colspan='2' style='border-bottom:0;border-top:0;'>&nbsp;");
            stringBuilder2.Append("");
            stringBuilder2.Append("</td><td style='border-bottom:0;border-top:0;'>&nbsp;</td></tr>");
            for (int index = 0; index <= dataTable6.Rows.Count - 1; ++index)
            {
                stringBuilder2.Append("<tr><td style='border-right:0;border-bottom:0;border-top:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
                stringBuilder2.Append(dataTable6.Rows[index][1].ToString());
                stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
                stringBuilder2.Append(".............");
                stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
                stringBuilder2.Append(dataTable6.Rows[index][0].ToString());
                stringBuilder2.Append("</td></tr>");
            }
            DataTable dataTable7 = new DataTable();
            DataTable dataTable8;
            if (Request.QueryString["status"] != null)
                dataTable8 = clsobj.ExecuteSql("select sum(credit) from dbo.Host_FeeLedger fl join dbo.Host_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt64(Request.QueryString["admnno"]) + " and Receipt_VrNo='" + RecptNo + "' group by AdmnNo");
            else
                dataTable8 = clsobj.ExecuteSql("select sum(balance) from dbo.Host_FeeLedger fl join dbo.Host_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt64(Request.QueryString["admnno"]) + "  and TransDate <= getdate() group by AdmnNo");
            stringBuilder2.Append("<tr><td colspan='3'></td></tr></br>");
            string s1 = new clsDAL().ExecuteScalarQry("Select CAST(SUM(Credit) AS DECIMAL(10,2)) AS BusFee  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=1 and admnno='" + Convert.ToInt64(Request.QueryString["admnno"]) + "' and Receipt_No='" + RecptNo + "'");
            if (s1 != "")
            {
                if (double.Parse(s1) > 0.0)
                    stringBuilder2.Append("<tr><td style='border-bottom:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Bus Fee  </td><td align='center' style='border-left:0;border-bottom:0;'>.............</td><td style='border-bottom:0;' align='right'> " + string.Format("{0:F2}", s1) + "</td></tr>");
            }
            else
                s1 = "0";
            string s2 = new clsDAL().ExecuteScalarQry("Select CAST(SUM(Credit) AS DECIMAL(10,2)) AS BusFee  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=2 and admnno='" + Convert.ToInt64(Request.QueryString["admnno"]) + "' and Receipt_No='" + RecptNo + "'");
            if (s2 != "")
            {
                if (double.Parse(s2) > 0.0)
                    stringBuilder2.Append("<tr><td style='border-bottom:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Hostel Fee  </td><td align='center' style='border-left:0;border-bottom:0;'>.............</td><td style='border-bottom:0;' align='right'> " + string.Format("{0:F2}", s2) + "</td></tr>");
            }
            else
                s2 = "0";
            stringBuilder2.Append("<tr><td colspan='2' align='right'><strong>TOTAL</strong></TD>");
            stringBuilder2.Append("<td align='right'>");
            double num1;
            if (dataTable8.Rows.Count > 0)
            {
                num1 = Convert.ToDouble(dataTable8.Rows[0][0].ToString()) + Convert.ToDouble(s1.ToString()) + Convert.ToDouble(s2.ToString());
                dataTable8.Rows[0][0].ToString();
            }
            else
                num1 = Convert.ToDouble(s1.ToString()) + Convert.ToDouble(s2.ToString());
            stringBuilder2.Append(string.Format("{0:F2}", num1));
            stringBuilder2.Append("</td></tr>");
            double num2 = 0.0;
            double num3 = num1;
            if (Session["CreditAmount"] != null)
                num2 = num3 + Convert.ToDouble(Session["CreditAmount"].ToString());
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%'  class='tbltd' style='font-size:14px;'>");
            stringBuilder2.Append("<tr><td colspan='2'></strong>&nbsp;(<strong>In Words :</strong> Rupees&nbsp;" + NumberToText(num1.ToString().Trim()) + "Only)</td><td>&nbsp;</td></tr>");
            Common common1 = new Common();
            if (Request.QueryString["D"] != null && Request.QueryString["D"].ToString() != "d")
            {
                string str3 = "SELECT isnull(Convert(Decimal(18,2),sum(Amount)),0) AS Fee FROM Acts_PaymentReceiptVoucher WHERE InvoiceReceiptNo= '" + Request.QueryString["rno"].ToString() + "'";
                string str4 = common1.ExecuteScalarQry(str3);
                stringBuilder2.Append("<tr><td colspan='3' style='font-size:12px'>&nbsp;Received Rs " + string.Format("{0:F2}", str4) + ", through " + Request.QueryString["Pmnt"].ToString() + " Payment ");
                if (Request.QueryString["Bn"] != null && Request.QueryString["Bn"].ToString().Trim() != "")
                    stringBuilder2.Append(" Vide </br>&nbsp;Instrument No : <u> " + Request.QueryString["ChkNo"].ToString() + "</u>, Dated : <u>" + Request.QueryString["ChkDt"].ToString() + "</u>, Bank Name : <u>" + Request.QueryString["Bn"].ToString() + "</u>");
                stringBuilder2.Append("</td></tr>");
            }
            if (dataTable2.Rows.Count > 0)
            {
                stringBuilder2.Append("<tr style='height: 30px;'>");
                stringBuilder2.Append("<td colspan='2' style='text-align: left'>");
                stringBuilder2.Append("&nbsp;");
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='right'>");
                stringBuilder2.Append("<b>Received By  " + dataTable2.Rows[0]["FullName"] + "</b>");
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("</tr>");
                stringBuilder2.Append("<tr style='height:30px;'><td colspan='2' ></td><td align='right'></td></tr>");
            }
            else
            {
                stringBuilder2.Append("<tr style='height:30px;'><td colspan='2' ></td><td align='right'></td></tr>");
                stringBuilder2.Append("<tr style='height: 30px;'>");
                stringBuilder2.Append("<td colspan='2' style='text-align: left'>");
                stringBuilder2.Append("&nbsp;");
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='right'>");
                stringBuilder2.Append("<b>Receiver's Signature</b>");
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("</tr>");
                stringBuilder2.Append("<tr style='height:30px;'><td colspan='2' ></td><td align='right'></td></tr>");
            }
            stringBuilder2.Append("<tr><td colspan='3' align='center' style='font-size:10px'><u>This is a computer generated receipt</u></td></tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("</center>");
            literaldata.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
            Session["HostPrintReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
        }
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('No Pending'); window.location='" + "../Hostel/FeeReceiptHostel.aspx" + "';", true);
    }

    private void getConsolidated()
    {
        btnContinueCashReceipt.Visible = true;
        RecptNo = Request.QueryString["rno"].ToString();
        DataTable dataTable1 = clsobj.ExecuteSql("select distinct FullName,ClassName,Section,FatherName,TelNoResidence,TeleNoOffice,convert(varchar,sm.UserDate,103) as usersdate from dbo.PS_StudMaster sm inner join dbo.PS_ClasswiseStudent cs on sm.admnno=cs.AdmnNo inner join dbo.PS_ClassMaster cm on cm.ClassID=cs.ClassID where cs.SessionYear='" + Request.QueryString["sess"] + "' and sm.AdmnNo=" + Convert.ToInt64(Request.QueryString["admnno"]));
        if (dataTable1.Rows.Count > 0)
        {
            clsobj.ExecuteSql("select datename(month,(max(TransDate))) from dbo.Host_FeeLedger where AdmnNo=" + Convert.ToInt64(Request.QueryString["admnno"]) + " and debit > 0  and TransDate <= getdate()");
            string str1 = Information();
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("<center>");
            stringBuilder1.Append(str1);
            stringBuilder1.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:14px;'>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            if (Session["rcptFee"] != null)
                Session.Remove("rcptFee");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            string str2 = "";
            if (Request.QueryString["rcptdt"] != null)
                str2 = Request.QueryString["rcptdt"].ToString();
            stringBuilder1.Append("<td style='width:25%'><strong>Date:&nbsp;</strong>" + str2 + "</td>");
            if (Request.QueryString["D"] != null && Request.QueryString["D"].ToString().ToLower() != "d")
                stringBuilder1.Append("<td><strong>Receipt No:</strong>&nbsp;" + Request.QueryString["rno"] + "&nbsp;&nbsp;</td>");
            else
                stringBuilder1.Append("<td colspan='2'>&nbsp;&nbsp;&nbsp;</td>");
            stringBuilder1.Append("<td  style='width:35%'><strong>Admn No.:&nbsp;</strong>" + Convert.ToInt64(Request.QueryString["admnno"]) + "&nbsp;</td><td>&nbsp;</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='6'><strong>Name :&nbsp;</strong>" + dataTable1.Rows[0][0].ToString() + "&nbsp;&nbsp;&nbsp;");
            stringBuilder1.Append("<strong>Father's Name :&nbsp;</strong>" + dataTable1.Rows[0]["FatherName"].ToString() + "&nbsp;&nbsp;&nbsp;");
            stringBuilder1.Append("<strong>Class:</strong>&nbsp;" + dataTable1.Rows[0][1].ToString());
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("</tr>");
            DataTable dataTable2 = new DataTable();
            if (Request.QueryString["D"] != null && Request.QueryString["D"].ToString() != "d")
            {
                Common common = new Common();
                dataTable2 = new clsDAL().GetDataTableQry("select p.Description,u.FullName from dbo.Acts_PaymentReceiptVoucher p inner join dbo.PS_USER_MASTER u on u.USER_ID=p.UserId where InvoiceReceiptNo= '" + Request.QueryString["rno"].ToString() + "'");
                stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
                stringBuilder1.Append("<tr>");
                if (dataTable1.Rows[0]["TelNoResidence"] != null && dataTable1.Rows[0]["TelNoResidence"].ToString().Trim() != "")
                    stringBuilder1.Append("<td colspan='4'><strong>Mobile No :</strong>&nbsp;" + dataTable1.Rows[0]["TelNoResidence"].ToString() + "&emsp; <strong>Fee Details:</strong>&nbsp;" + dataTable2.Rows[0]["Description"].ToString() + "</td>");
                else
                    stringBuilder1.Append("<td colspan='4'><strong>Mobile No :</strong>&nbsp;--&emsp; <strong>Fee Details:</strong>&nbsp;</td>");
                //stringBuilder1.Append("<td colspan='4'><strong>Mobile No :</strong>&nbsp;--&emsp; <strong>Fee Details:</strong>&nbsp;" + dataTable2.Rows[0]["Description"].ToString() + "</td>");
                stringBuilder1.Append("</tr>");
            }
            stringBuilder1.Append("</table>");
            stringBuilder1.Append("</center>");
            DataTable dataTable3 = new DataTable();
            if (Request.QueryString["status"] != null)
                clsobj.ExecuteSql("select convert(decimal(18,2),sum(credit)),fs.FeeName as TransDesc,admnno,fs.FeeID from dbo.Host_FeeLedger fl join dbo.Host_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt64(Request.QueryString["admnno"]) + " and PeriodicityID<=2 and Receipt_VrNo='" + RecptNo + "' group by AdmnNo,fs.FeeID,fs.FeeName order by fs.FeeID,fs.FeeName");
            else
                clsobj.ExecuteSql("select convert(decimal(18,2),sum(Balance)),fs.FeeName as TransDesc,admnno,fs.FeeID from dbo.Host_FeeLedger fl join dbo.Host_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt64(Request.QueryString["admnno"]) + " and PeriodicityID<=2  and TransDate <= getdate() and fl.TransDesc<>'AC Initialized' group by AdmnNo,fs.FeeID,fs.FeeName order by fs.FeeID,fs.FeeName");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<center>");
            stringBuilder2.Append("<table style='font-size:14px;' border=1 width='85%' cellspacing=0 cellpadding=0><tr><td colspan=2 align=center>");
            stringBuilder2.Append("<strong>PARTICULARS</strong></td><td align=center><strong>AMOUNT</strong></td></tr>");
            DataTable dataTable4 = new DataTable();
            DataTable dataTable5;
            if (Request.QueryString["status"] != null)
                dataTable5 = clsobj.ExecuteSql("select isnull(sum(credit),0) from dbo.Host_FeeLedger fl join dbo.Host_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt64(Request.QueryString["admnno"]) + " and Receipt_VrNo='" + RecptNo + "' group by AdmnNo");
            else
                dataTable5 = clsobj.ExecuteSql("select isnull(sum(balance),0) from dbo.Host_FeeLedger fl join dbo.Host_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + Convert.ToInt64(Request.QueryString["admnno"]) + "  and TransDate <= getdate() group by AdmnNo");
            stringBuilder2.Append("<tr><td colspan='2' style='border-bottom:0;border-top:0;'>&nbsp;");
            stringBuilder2.Append("");
            stringBuilder2.Append("</td><td style='border-bottom:0;border-top:0;'>&nbsp;</td></tr>");
            stringBuilder2.Append("<tr><td style='border-bottom:0;border-top:0;border-right:0;'>&nbsp;&nbsp;&nbsp;&nbsp;Hostel Fee</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>.............</td>");
            stringBuilder2.Append("<td align='right' style='border-bottom:0;border-top:0;'>");
            double num1 = 0.0;
            if (dataTable5.Rows.Count > 0)
            {
                num1 = Convert.ToDouble(dataTable5.Rows[0][0].ToString());
                dataTable5.Rows[0][0].ToString();
            }
            stringBuilder2.Append(string.Format("{0:F2}", num1));
            stringBuilder2.Append("</td></tr>");
            DataTable dataTable6 = new DataTable();
            if (Request.QueryString["status"] != null)
            {
                dataTable6 = new clsDAL().GetDataTableQry("Select isnull(CAST(SUM(Credit) AS DECIMAL(10,2)),0) AS BusFee  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=1 and admnno='" + Convert.ToInt64(Request.QueryString["admnno"]) + "' and Receipt_No='" + RecptNo + "'");
                double num2 = Convert.ToDouble(dataTable6.Rows[0][0].ToString());
                if (num2 > 0.0)
                {
                    stringBuilder2.Append("<tr><td style='border-bottom:0;border-top:0;border-right:0;'>&nbsp;&nbsp;&nbsp;&nbsp;Bus Fee</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>.............</td>");
                    stringBuilder2.Append("<td align='right' style='border-bottom:0;border-top:0;'>");
                    stringBuilder2.Append(string.Format("{0:F2}", num2));
                    stringBuilder2.Append("</td></tr>");
                }
            }
            DataTable dataTable7 = new DataTable();
            if (Request.QueryString["status"] != null)
            {
                dataTable7 = new clsDAL().GetDataTableQry("Select isnull(CAST(SUM(Credit) AS DECIMAL(10,2)),0) AS BusFee  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=2 and admnno='" + Convert.ToInt64(Request.QueryString["admnno"]) + "' and Receipt_No='" + RecptNo + "'");
                double num2 = Convert.ToDouble(dataTable7.Rows[0][0].ToString());
                if (num2 > 0.0)
                {
                    stringBuilder2.Append("<tr><td style='border-bottom:0;border-top:0;border-right:0;'>&nbsp;&nbsp;&nbsp;&nbsp;Hostel Fee</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>.............</td>");
                    stringBuilder2.Append("<td align='right' style='border-bottom:0;border-top:0;'>");
                    stringBuilder2.Append(string.Format("{0:F2}", num2));
                    stringBuilder2.Append("</td></tr>");
                }
            }
            stringBuilder2.Append("<tr><td colspan='2' align='right'><strong>TOTAL</strong></TD>");
            stringBuilder2.Append("<td align='right'>");
            double num3 = Convert.ToDouble(num1) + Convert.ToDouble(dataTable6.Rows[0][0].ToString()) + Convert.ToDouble(dataTable7.Rows[0][0].ToString());
            stringBuilder2.Append(string.Format("{0:F2}", num3));
            stringBuilder2.Append("</td></tr>");
            double num4 = 0.0;
            double num5 = num1;
            if (Session["CreditAmount"] != null)
                num4 = num5 + Convert.ToDouble(Session["CreditAmount"].ToString());
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:14px;'>");
            stringBuilder2.Append("<tr><td colspan='2'></strong>&nbsp;(<strong>In Words :</strong> Rupees&nbsp;" + NumberToText(num3.ToString().Trim()) + "Only)</td><td>&nbsp;</td></tr>");
            Common common1 = new Common();
            if (Request.QueryString["D"] != null && Request.QueryString["D"].ToString() != "d")
            {
                string str3 = "SELECT isnull(Convert(Decimal(18,2),sum(Amount)),0) AS Fee FROM Acts_PaymentReceiptVoucher WHERE InvoiceReceiptNo= '" + Request.QueryString["rno"].ToString() + "'";
                string str4 = common1.ExecuteScalarQry(str3);
                stringBuilder2.Append("<tr><td colspan='3' style='font-size:12px'>&nbsp;Received Rs " + string.Format("{0:F2}", str4) + ", through " + Request.QueryString["Pmnt"].ToString() + " Payment ");
                if (Request.QueryString["Bn"] != null && Request.QueryString["Bn"].ToString().Trim() != "")
                    stringBuilder2.Append(" Vide </br>&nbsp;&nbsp;Instrument No : <u> " + Request.QueryString["ChkNo"].ToString() + "</u>, Dated : <u>" + Request.QueryString["ChkDt"].ToString() + "</u>, Bank Name : <u>" + Request.QueryString["Bn"].ToString() + "</u>");
                stringBuilder2.Append("</td></tr>");
            }
            Common common2 = new Common();
            if (dataTable2.Rows.Count > 0)
                stringBuilder2.Append("<tr style='height:30px;'><td colspan='2'></td><td align='right'><b>Received By " + dataTable2.Rows[0]["FullName"].ToString() + "</b></td></tr>");
            else
                stringBuilder2.Append("<tr style='height:30px;'><td colspan='2'></td><td align='right'><b>Receiver's Signature</b></td></tr>");
            stringBuilder2.Append("<tr><td colspan='3' align='center' style='font-size:10px'><u>This is a computer generated receipt</u></td></tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("</center>");
            lblConsolidated.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
            Session["HostConsolidatedReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
        }
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('No Pending'); window.location='" + "../Hostel/FeeReceiptHostel.aspx" + "';", true);
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

    protected void btnConsolidat_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptHostConsolidatePrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnDetail_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('rptHostRcptPrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Sw"] != null)
        {
            if (Request.QueryString["Sw"] != null && Request.QueryString["Pmnt"] != null)
                Response.Redirect("../Hostel/HostelLeaving.aspx?admnno=" + Request.QueryString["admnno"].ToString().Trim() + "&sess=" + Request.QueryString["sess"].ToString().Trim() + "&cid=" + Request.QueryString["class"].ToString().Trim());
            else
                Response.Redirect("../Hostel/FeeReceiptHostel.aspx?admnno=" + Request.QueryString["admnno"].ToString().Trim() + "&sess=" + Request.QueryString["sess"].ToString().Trim() + "&cid=" + Request.QueryString["class"].ToString().Trim() + "&D=p &Sw=" + Request.QueryString["Sw"]);
        }
        else
            Response.Redirect("../Hostel/FeeReceiptHostel.aspx?admnno=" + Request.QueryString["admnno"].ToString().Trim() + "&sess=" + Request.QueryString["sess"].ToString().Trim() + "&cid=" + Request.QueryString["class"].ToString().Trim() + "&D=p");
    }

    protected void btnContinueCashReceipt_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Hostel/FeeReceiptHostel.aspx");
    }

    protected void btnMainMenu_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Hostel/HostelHome.aspx");
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