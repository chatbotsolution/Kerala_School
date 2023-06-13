using ASP;
using RJS.Web.WebControl;
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

public partial class Reports_rptDupRecieptCash : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        Session["ConsolidatedReciept"] = null;
        txtadminno.Text = "0";
        fillsession();
        fillclass();
        fillstudent();
        lbldetail.Text = "";
        btnPrint.Enabled = false;
        btnConsolidat.Enabled = false;
        Disable();
    }

    protected void fillsession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        drpClasses.Items.Clear();
        drpClasses.DataSource = new clsDAL().GetDataTable("ps_sp_get_classesForDDL");
        drpClasses.DataTextField = "classname";
        drpClasses.DataValueField = "classid";
        drpClasses.DataBind();
        drpClasses.Items.Insert(0, new ListItem("All", "0"));
    }

    private void fillstudent()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTableQry;
        if (drpClasses.SelectedIndex != 0)
            dataTableQry = clsDal.GetDataTableQry("select fullname,cs.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.classid=" + drpClasses.SelectedValue + " and cs.SessionYear='" + drpSession.SelectedValue.ToString() + "'  order by fullname");
        else
            dataTableQry = clsDal.GetDataTableQry("select  fullname,cs.admnno  from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.SessionYear='" + drpSession.SelectedValue.ToString() + "'  order by fullname");
        drpstudent.DataSource = dataTableQry;
        drpstudent.DataTextField = "fullname";
        drpstudent.DataValueField = "admnno";
        drpstudent.DataBind();
        drpstudent.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void fillfeereciept()
    {
        drpReciept.DataSource = new clsDAL().GetDataTable("Ps_Sp_Get_RcptNo", new Hashtable()
    {
      {
         "@AdmnNo",
         drpstudent.SelectedValue
      },
      {
         "@SessionYr",
         drpSession.SelectedValue.ToString().Trim()
      }
    });
        drpReciept.DataTextField = "Receipt_Dt";
        drpReciept.DataValueField = "InvoiceReceiptNo";
        drpReciept.DataBind();
        drpReciept.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
        lbldetail.Text = "";
        btnPrint.Enabled = false;
        btnConsolidat.Enabled = false;
        txtadminno.Text = "0";
    }

    protected void drpClasses_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
        lbldetail.Text = "";
        txtadminno.Text = "0";
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbldetail.Text = "";
        fillstudent();
        btnPrint.Enabled = false;
        btnConsolidat.Enabled = false;
        txtadminno.Text = "0";
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtadminno.Text = drpstudent.SelectedValue;
        lbldetail.Text = "";
        btnPrint.Enabled = false;
        btnConsolidat.Enabled = false;
        fillfeereciept();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (rbtnStud.Checked.Equals(true))
        {
            btnConsolidat.Enabled = false;
            btnPrint.Enabled = false;
            getdata();
            getConsolidated();
        }
        else
        {
            btnConsolidat.Enabled = false;
            btnPrint.Enabled = false;
            getMiscdata();
        }
    }

    private void getdata()
    {
        clsDAL clsDal = new clsDAL();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SessionYear", drpSession.SelectedValue.ToString().Trim());
        hashtable.Add("@AdmnNo", drpstudent.SelectedValue.ToString().Trim());
        hashtable.Add("@ReceiptNo", drpReciept.SelectedValue.ToString().Trim());
        if (Convert.ToDateTime(clsDal.ExecuteScalarQry("select TransDt from dbo.Acts_PaymentReceiptVoucher where InvoiceReceiptNo='" + drpReciept.SelectedValue.ToString().Trim() + "'").Trim().Split(' ')[0]) >= Convert.ToDateTime("4/1/2014"))
        {
            DataSet dataSet2 = clsDal.GetDataSet("Ps_GetDupRcptNew", hashtable);
            if (Convert.ToDouble(dataSet2.Tables[6].Compute("SUM(MiscRcpt)", "").ToString()) == 0.0)
            {
                DataTable table = dataSet2.Tables[0];
                if (table.Rows.Count <= 0)
                    return;
                btnPrint.Enabled = true;
                btnConsolidat.Enabled = true;
                string str = Information();
                StringBuilder stringBuilder1 = new StringBuilder();
                stringBuilder1.Append("<center>");
                stringBuilder1.Append(str);
                stringBuilder1.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:14px;' class='tbltd'>");
                stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td style='width:18%'><strong>Date:&nbsp;</strong>" + dataSet2.Tables[1].Rows[0]["RecvDate"].ToString() + "</td>");
                stringBuilder1.Append("<td><strong>Receipt No:</strong>&nbsp;" + drpReciept.SelectedValue.ToString() + "&nbsp;&nbsp;&nbsp;</td>");
                stringBuilder1.Append("</tr>");
                Session.Remove("rcptFee");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td colspan='6'><strong>Name :&nbsp;</strong>" + table.Rows[0][0].ToString() + "&nbsp;&nbsp;&nbsp;");
                stringBuilder1.Append("<strong>Father's Name :&nbsp;</strong>" + table.Rows[0]["FatherName"].ToString() + "&nbsp;&nbsp;&nbsp;");
                stringBuilder1.Append("<strong>Class:</strong>&nbsp;" + table.Rows[0][1].ToString());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td colspan='4'><strong>Fee Details:</strong>&nbsp;" + dataSet2.Tables[1].Rows[0]["Description"].ToString() + "</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("</table>");
                stringBuilder1.Append("</center>");
                double num = 0.0;
                StringBuilder stringBuilder2 = new StringBuilder();
                stringBuilder2.Append("<center>");
                stringBuilder2.Append("<table style='font-size:14px;' border=1 width='85%' cellspacing=0 cellpadding=0 class='tbltd'><tr><td colspan=2 align=center>");
                stringBuilder2.Append("<strong>PARTICULARS</strong></td><td align=center><strong>AMOUNT</strong></td></tr>");
                for (int index = 0; index <= dataSet2.Tables[2].Rows.Count - 1; ++index)
                {
                    stringBuilder2.Append("<tr><td style='border-right:0;border-bottom:0;border-top:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
                    stringBuilder2.Append(dataSet2.Tables[2].Rows[index][1].ToString());
                    stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
                    stringBuilder2.Append(".............");
                    stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
                    stringBuilder2.Append(dataSet2.Tables[2].Rows[index][0].ToString());
                    stringBuilder2.Append("</td></tr>");
                    num += Convert.ToDouble(dataSet2.Tables[2].Rows[index][0].ToString().Trim());
                }
                stringBuilder2.Append("<tr><td colspan='2' style='border-bottom:0;border-top:0;'>&nbsp;");
                stringBuilder2.Append("");
                stringBuilder2.Append("</td><td style='border-bottom:0;border-top:0;'>&nbsp;</td></tr>");
                for (int index = 0; index <= dataSet2.Tables[3].Rows.Count - 1; ++index)
                {
                    stringBuilder2.Append("<tr><td style='border-right:0;border-bottom:0;border-top:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
                    stringBuilder2.Append(dataSet2.Tables[3].Rows[index][1].ToString());
                    stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
                    stringBuilder2.Append(".............");
                    stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
                    stringBuilder2.Append(dataSet2.Tables[3].Rows[index][0].ToString());
                    stringBuilder2.Append("</td></tr>");
                    num += Convert.ToDouble(dataSet2.Tables[3].Rows[index][0].ToString().Trim());
                }
                stringBuilder2.Append("<tr><td colspan='3'></td></tr>");
                if (dataSet2.Tables[4].Rows[0]["BusFee"].ToString() != "0.00")
                {
                    stringBuilder2.Append("<tr><td style='border-bottom:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Bus Fee  </td><td align='center' style='border-left:0;border-bottom:0;'>.............</td><td style='border-bottom:0;' align='right'> " + string.Format("{0:F2}", dataSet2.Tables[4].Rows[0]["BusFee"].ToString()) + "</td></tr>");
                    num += Convert.ToDouble(dataSet2.Tables[4].Rows[0]["BusFee"].ToString());
                }
                if (dataSet2.Tables[5].Rows[0]["BooksFee"].ToString() != "0.00")
                {
                    stringBuilder2.Append("<tr><td style='border-bottom:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Books Fee  </td><td align='center' style='border-left:0;border-bottom:0;'>.............</td><td style='border-bottom:0;' align='right'> " + string.Format("{0:F2}", dataSet2.Tables[5].Rows[0]["BooksFee"].ToString()) + "</td></tr>");
                    num += Convert.ToDouble(dataSet2.Tables[5].Rows[0]["BooksFee"].ToString());
                }
                stringBuilder2.Append("<tr><td colspan='2' align='right'><strong>TOTAL</strong></TD>");
                stringBuilder2.Append("<td align='right'>");
                stringBuilder2.Append(string.Format("{0:F2}", num));
                stringBuilder2.Append("</td></tr>");
                stringBuilder2.Append("</table>");
                stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' class='tbltd' style='border-bottom:1;'>");
                stringBuilder2.Append("<tr><td style='font-size:12px'></strong>(<strong>In Words :</strong> Rupees&nbsp;" + NumberToText(num.ToString()) + "Only)</td><td>&nbsp;</td></tr>");
                stringBuilder2.Append("<tr><td colspan='3' style='font-size:12px'>&nbsp;Received Rs " + string.Format("{0:F2}", dataSet2.Tables[1].Rows[0]["Fee"].ToString()) + " through " + dataSet2.Tables[1].Rows[0]["PaymentMode"].ToString());
                if (dataSet2.Tables[1].Rows[0]["DrawanOnBank"].ToString().Trim() != "")
                    stringBuilder2.Append(" Vide </br>&nbsp;Instrument No : <u> " + dataSet2.Tables[1].Rows[0]["ChequeNo"] + "</u>, Dated : <u>" + dataSet2.Tables[1].Rows[0]["ChequeDate"].ToString().Trim() + "</u>, Bank Name : <u>" + dataSet2.Tables[1].Rows[0]["DrawanOnBank"].ToString().Trim() + "</u>");
                stringBuilder2.Append("<tr style='height:30px;'><td colspan='2'></td><td align='right' style='font-size:13px;'><b>Received By " + dataSet2.Tables[1].Rows[0]["FullName"].ToString() + "</b></td></tr>");
                stringBuilder2.Append("<tr><td colspan='3' align='center' style='font-size:10px'><u>This is a computer generated receipt</u></td></tr>");
                stringBuilder2.Append("</table>");
                stringBuilder2.Append("</center>");
                if (num > 0.0)
                {
                    btnPrint.Enabled = true;
                    btnConsolidat.Enabled = true;
                    lbldetail.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
                    Session["PrintReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
                }
                else
                {
                    btnPrint.Enabled = false;
                    btnConsolidat.Enabled = false;
                    lbldetail.Text = string.Empty;
                }
            }
            else
                getMiscdata();
        }
        else
        {
            DataSet dataSet2 = clsDal.GetDataSet("Ps_GetDupRcpt", hashtable);
            if (Convert.ToDouble(dataSet2.Tables[7].Compute("SUM(Donation)", "").ToString()) == 0.0)
            {
                DataTable table = dataSet2.Tables[0];
                if (table.Rows.Count <= 0)
                    return;
                btnPrint.Enabled = true;
                btnConsolidat.Enabled = true;
                string str = Information();
                StringBuilder stringBuilder1 = new StringBuilder();
                stringBuilder1.Append("<center>");
                stringBuilder1.Append(str);
                stringBuilder1.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:14px;' class='tbltd'>");
                stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td style='width:25%'><strong>Date:&nbsp;</strong>" + dataSet2.Tables[1].Rows[0]["RecvDate"].ToString() + "</td>");
                stringBuilder1.Append("<td  style='width:35%'><strong>Admn No.:&nbsp;</strong>" + txtadminno.Text.ToString() + "&nbsp;&nbsp;&nbsp;</td>");
                stringBuilder1.Append("<td><strong>Receipt No:</strong>&nbsp;" + drpReciept.SelectedValue.ToString() + "</td><td>&nbsp;</td>");
                stringBuilder1.Append("</tr>");
                Session.Remove("rcptFee");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td style='width:60%' colspan='2'><strong>Student Name:&nbsp;</strong>" + table.Rows[0][0].ToString() + "</td><td colspan='2'><strong>Class:</strong>&nbsp;" + table.Rows[0][1].ToString() + "&nbsp;</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td colspan='4'><strong>Fee Details:</strong>&nbsp;" + dataSet2.Tables[1].Rows[0]["Description"].ToString() + "</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("</table>");
                stringBuilder1.Append("</center>");
                double num = 0.0;
                StringBuilder stringBuilder2 = new StringBuilder();
                stringBuilder2.Append("<center>");
                stringBuilder2.Append("<table style='font-size:14px;' border=1 width='85%' cellspacing=0 cellpadding=0 class='tbltd'><tr><td colspan=2 align=center>");
                stringBuilder2.Append("<strong>PARTICULARS</strong></td><td align=center><strong>AMOUNT</strong></td></tr>");
                for (int index = 0; index <= dataSet2.Tables[2].Rows.Count - 1; ++index)
                {
                    stringBuilder2.Append("<tr><td style='border-right:0;border-bottom:0;border-top:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
                    stringBuilder2.Append(dataSet2.Tables[2].Rows[index][1].ToString());
                    stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
                    stringBuilder2.Append(".............");
                    stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
                    stringBuilder2.Append(dataSet2.Tables[2].Rows[index][0].ToString());
                    stringBuilder2.Append("</td></tr>");
                    num += Convert.ToDouble(dataSet2.Tables[2].Rows[index][0].ToString().Trim());
                }
                stringBuilder2.Append("<tr><td colspan='2' style='border-bottom:0;border-top:0;'>&nbsp;");
                stringBuilder2.Append("");
                stringBuilder2.Append("</td><td style='border-bottom:0;border-top:0;'>&nbsp;</td></tr>");
                for (int index = 0; index <= dataSet2.Tables[3].Rows.Count - 1; ++index)
                {
                    stringBuilder2.Append("<tr><td style='border-right:0;border-bottom:0;border-top:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
                    stringBuilder2.Append(dataSet2.Tables[3].Rows[index][1].ToString());
                    stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
                    stringBuilder2.Append(".............");
                    stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
                    stringBuilder2.Append(dataSet2.Tables[3].Rows[index][0].ToString());
                    stringBuilder2.Append("</td></tr>");
                    num += Convert.ToDouble(dataSet2.Tables[3].Rows[index][0].ToString().Trim());
                }
                stringBuilder2.Append("<tr><td colspan='3'></td></tr>");
                if (dataSet2.Tables[4].Rows[0]["Fine"].ToString() != "0.00")
                {
                    stringBuilder2.Append("<tr><td style='border-bottom:0;border-top:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Fine Amount  </td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>.............</td><td style='border-bottom:0;border-top:0;' align='right'> " + string.Format("{0:F2}", dataSet2.Tables[4].Rows[0]["Fine"].ToString()) + "</td></tr>");
                    num += Convert.ToDouble(dataSet2.Tables[4].Rows[0]["Fine"].ToString());
                }
                if (dataSet2.Tables[5].Rows[0]["BusFee"].ToString() != "0.00")
                {
                    stringBuilder2.Append("<tr><td style='border-bottom:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Bus Fee  </td><td align='center' style='border-left:0;border-bottom:0;'>.............</td><td style='border-bottom:0;' align='right'> " + string.Format("{0:F2}", dataSet2.Tables[5].Rows[0]["BusFee"].ToString()) + "</td></tr>");
                    num += Convert.ToDouble(dataSet2.Tables[5].Rows[0]["BusFee"].ToString());
                }
                if (dataSet2.Tables[6].Rows[0]["BooksFee"].ToString() != "0.00")
                {
                    stringBuilder2.Append("<tr><td style='border-bottom:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Books Fee  </td><td align='center' style='border-left:0;border-bottom:0;'>.............</td><td style='border-bottom:0;' align='right'> " + string.Format("{0:F2}", dataSet2.Tables[6].Rows[0]["BooksFee"].ToString()) + "</td></tr>");
                    num += Convert.ToDouble(dataSet2.Tables[6].Rows[0]["BooksFee"].ToString());
                }
                stringBuilder2.Append("<tr><td colspan='2' align='right'><strong>TOTAL</strong></TD>");
                stringBuilder2.Append("<td align='right'>");
                stringBuilder2.Append(string.Format("{0:F2}", num));
                stringBuilder2.Append("</td></tr>");
                stringBuilder2.Append("</table>");
                stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' class='tbltd' style='border-bottom:1;'>");
                stringBuilder2.Append("<tr><td>&nbsp;</td></tr>");
                stringBuilder2.Append("<tr><td style='font-size:12px'></strong>(<strong>In Words :</strong> Rupees&nbsp;" + NumberToText(num.ToString()) + "Only)</td><td>&nbsp;</td></tr>");
                stringBuilder2.Append("<tr><td colspan='3'><b>&nbsp;NOTE:</b></td></tr>");
                stringBuilder2.Append("<tr><td colspan='3' style='font-size:12px'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;1. Total amount received - Rs : " + string.Format("{0:F2}", dataSet2.Tables[1].Rows[0]["Fee"].ToString()) + " through " + dataSet2.Tables[1].Rows[0]["PaymentMode"].ToString());
                if (dataSet2.Tables[1].Rows[0]["DrawanOnBank"].ToString().Trim() != "")
                    stringBuilder2.Append(" Vide </br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Instrument No : <u> " + dataSet2.Tables[1].Rows[0]["ChequeNo"] + "</u>, Dated : <u>" + dataSet2.Tables[1].Rows[0]["ChequeDate"].ToString().Trim() + "</u>, Bank Name : <u>" + dataSet2.Tables[1].Rows[0]["DrawanOnBank"].ToString().Trim() + "</u>");
                stringBuilder2.Append("</td></tr>");
                stringBuilder2.Append("<tr><td colspan='3' style='font-size:12px'> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;2. Advance Amount Paid - Rs : " + string.Format("{0:F2}", dataSet2.Tables[8].Rows[0]["AdvFee"].ToString()) + " </td></tr>");
                stringBuilder2.Append("<tr><td colspan='2'></td><td align='center' style='font-size:14px'><b>Receiver's</b></td></tr>");
                stringBuilder2.Append("<tr><td colspan='2'></td><td align='center' style='font-size:14px'><b>Signature</b></td></tr>");
                stringBuilder2.Append("</table>");
                stringBuilder2.Append("</center>");
                if (num > 0.0)
                {
                    btnPrint.Enabled = true;
                    btnConsolidat.Enabled = true;
                    lbldetail.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
                    Session["PrintReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
                }
                else
                {
                    btnPrint.Enabled = false;
                    btnConsolidat.Enabled = false;
                    lbldetail.Text = string.Empty;
                }
            }
            else
                getMiscdata();
        }
    }

    private void getConsolidated()
    {
        clsDAL clsDal = new clsDAL();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SessionYear", drpSession.SelectedValue.ToString().Trim());
        hashtable.Add("@AdmnNo", drpstudent.SelectedValue.ToString().Trim());
        hashtable.Add("@ReceiptNo", drpReciept.SelectedValue.ToString().Trim());
        if (Convert.ToDateTime(clsDal.ExecuteScalarQry("select TransDt from dbo.Acts_PaymentReceiptVoucher where InvoiceReceiptNo='" + drpReciept.SelectedValue.ToString().Trim() + "'").Trim().Split(' ')[0]) >= Convert.ToDateTime("4/1/2014"))
        {
            DataSet dataSet2 = clsDal.GetDataSet("Ps_GetConsolidateDupRcptNew", hashtable);
            if (Convert.ToDouble(dataSet2.Tables[5].Compute("SUM(MiscRcpt)", "").ToString()) == 0.0)
            {
                DataTable table = dataSet2.Tables[0];
                if (table.Rows.Count <= 0)
                    return;
                string str = Information();
                double num1 = 0.0;
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("<center>");
                stringBuilder.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:14px;'>");
                stringBuilder.Append("<tr><td style='border-right:dotted 1px black;'>" + str + "</td>");
                stringBuilder.Append("<td style='padding-left:10px;'>" + str + "</td></tr>");
                stringBuilder.Append("<tr><td style='border-right:dotted 1px black;'>");
                stringBuilder.Append("<table style='font-size:15px;'><tr><td colspan='3' ></td></tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='width:30%'><strong>Date:&nbsp;</strong> " + dataSet2.Tables[1].Rows[0]["RecvDate"].ToString() + "</td>");
                stringBuilder.Append("<td><strong>R.No:</strong>&nbsp;<span style='font-size:17px;'>" + drpReciept.SelectedValue.ToString() + "</span>&nbsp;&nbsp;</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='6' style='font-size:14px;'><strong>Name :&nbsp;</strong>" + table.Rows[0][0].ToString() + "&nbsp;&nbsp;&nbsp;");
                stringBuilder.Append("<strong>Class:</strong>&nbsp;" + table.Rows[0][1].ToString());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='4' style='font-size:14px;'><strong>Father's Name :&nbsp;</strong>" + table.Rows[0]["FatherName"].ToString() + "&nbsp;");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='4'><strong>Fee Details:</strong>&nbsp;" + dataSet2.Tables[1].Rows[0]["Description"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("</center>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td style='padding-left:10px;'>");
                stringBuilder.Append("<center>");
                stringBuilder.Append("<table style='font-size:15px;'><tr><td colspan='3'></td></tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='width:30%'><strong>Date:&nbsp;</strong>" + dataSet2.Tables[1].Rows[0]["RecvDate"].ToString() + "</td>");
                stringBuilder.Append("<td><strong>R.No:</strong>&nbsp;<span style='font-size:17px;'>" + drpReciept.SelectedValue.ToString() + "</span></td>&nbsp;");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='6' style='font-size:14px;'><strong>Name :&nbsp;</strong>" + table.Rows[0][0].ToString() + "&nbsp;&nbsp;&nbsp;");
                stringBuilder.Append("<strong>Class:</strong>&nbsp;" + table.Rows[0][1].ToString());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='4' style='font-size:14px;'><strong>Father's Name :&nbsp;</strong>" + table.Rows[0]["FatherName"].ToString() + "&nbsp;");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='4'><strong>Fee Details:</strong>&nbsp;" + dataSet2.Tables[1].Rows[0]["Description"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("</center>");
                stringBuilder.Append("</td></tr>");
                stringBuilder.Append("<tr><td style='border-right:dotted 1px black;'>");
                stringBuilder.Append("<table style='font-size:18px;' border=1 width='85%' cellspacing=0 cellpadding=0><tr><td colspan=2 align=center>");
                stringBuilder.Append("<strong>PARTICULARS</strong></td><td align=center><strong>AMOUNT</strong></td></tr>");
                stringBuilder.Append("<tr><td colspan='2' style='border-bottom:0;border-top:0;'>&nbsp;");
                stringBuilder.Append("");
                stringBuilder.Append("</td><td style='border-bottom:0;border-top:0;'>&nbsp;</td></tr>");
                stringBuilder.Append("<tr><td style='border-bottom:0;border-top:0;border-right:0;'>&nbsp;&nbsp;&nbsp;&nbsp;School Fee</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>.............</td>");
                stringBuilder.Append("<td align='right' style='border-bottom:0;border-top:0;'>");
                double num2 = num1 + Convert.ToDouble(dataSet2.Tables[2].Rows[0][0].ToString());
                stringBuilder.Append(string.Format("{0:F2}", num2));
                stringBuilder.Append("</td></tr>");
                if (dataSet2.Tables[3].Rows[0]["BusFee"].ToString() != "0.00")
                {
                    stringBuilder.Append("<tr><td style='border-bottom:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Bus Fee  </td><td align='center' style='border-left:0;border-bottom:0;'>.............</td><td style='border-bottom:0;' align='right'> " + string.Format("{0:F2}", dataSet2.Tables[3].Rows[0]["BusFee"].ToString()) + "</td></tr>");
                    num2 += Convert.ToDouble(dataSet2.Tables[3].Rows[0]["BusFee"].ToString());
                }
                if (dataSet2.Tables[4].Rows[0]["BooksFee"].ToString() != "0.00")
                {
                    stringBuilder.Append("<tr><td style='border-bottom:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Books Fee  </td><td align='center' style='border-left:0;border-bottom:0;'>.............</td><td style='border-bottom:0;' align='right'> " + string.Format("{0:F2}", dataSet2.Tables[4].Rows[0]["BooksFee"].ToString()) + "</td></tr>");
                    num2 += Convert.ToDouble(dataSet2.Tables[4].Rows[0]["BooksFee"].ToString());
                }
                stringBuilder.Append("<tr><td colspan='2' align='right'><strong>TOTAL</strong></TD>");
                stringBuilder.Append("<td align='right'>");
                stringBuilder.Append(string.Format("{0:F2}", num2));
                stringBuilder.Append("</td></tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:14px;'>");
                stringBuilder.Append("<tr><td colspan='3'></strong>&nbsp;(<strong>In Words :</strong> Rupees&nbsp;" + NumberToText(num2.ToString()) + "Only)</td><td>&nbsp;</td></tr>");
                stringBuilder.Append("<tr><td colspan='3' style='font-size:12px'>&nbsp;Received Rs " + string.Format("{0:F2}", dataSet2.Tables[1].Rows[0]["Fee"].ToString()) + " through " + dataSet2.Tables[1].Rows[0]["PaymentMode"].ToString());
                if (dataSet2.Tables[1].Rows[0]["DrawanOnBank"].ToString().Trim() != "")
                    stringBuilder.Append(" Vide </br>&nbsp;Instrument No : <u> " + dataSet2.Tables[1].Rows[0]["ChequeNo"] + "</u>, Dated : <u>" + dataSet2.Tables[1].Rows[0]["ChequeDate"].ToString().Trim() + "</u>, Bank Name : <u>" + dataSet2.Tables[1].Rows[0]["DrawanOnBank"].ToString().Trim() + "</u>");
                stringBuilder.Append("</td></tr>");
                stringBuilder.Append("<tr style='height:30px;'><td colspan='2'></td><td align='right'><b>Received By " + dataSet2.Tables[1].Rows[0]["FullName"].ToString() + "</b></td></tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='1' align='left' style='font-size:13px;width:160px;'>(Office Copy)</td>");
                stringBuilder.Append("<td colspan='3' align='left' style='font-size:10px'><u>This is a computer generated receipt</u></td></tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("</td>");
                double num3 = 0.0;
                stringBuilder.Append("<td style='padding-left:10px;'>");
                stringBuilder.Append("<table style='font-size:18px;' border=1 width='85%' cellspacing=0 cellpadding=0><tr><td colspan=2 align=center>");
                stringBuilder.Append("<strong>PARTICULARS</strong></td><td align=center><strong>AMOUNT</strong></td></tr>");
                stringBuilder.Append("<tr><td colspan='2' style='border-bottom:0;border-top:0;'>&nbsp;");
                stringBuilder.Append("");
                stringBuilder.Append("</td><td style='border-bottom:0;border-top:0;'>&nbsp;</td></tr>");
                stringBuilder.Append("<tr><td style='border-bottom:0;border-top:0;border-right:0;'>&nbsp;&nbsp;&nbsp;&nbsp;School Fee</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>.............</td>");
                stringBuilder.Append("<td align='right' style='border-bottom:0;border-top:0;'>");
                double num4 = num3 + Convert.ToDouble(dataSet2.Tables[2].Rows[0][0].ToString());
                stringBuilder.Append(string.Format("{0:F2}", num4));
                stringBuilder.Append("</td></tr>");
                if (dataSet2.Tables[3].Rows[0]["BusFee"].ToString() != "0.00")
                {
                    stringBuilder.Append("<tr><td style='border-bottom:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Bus Fee  </td><td align='center' style='border-left:0;border-bottom:0;'>.............</td><td style='border-bottom:0;' align='right'> " + string.Format("{0:F2}", dataSet2.Tables[3].Rows[0]["BusFee"].ToString()) + "</td></tr>");
                    num4 += Convert.ToDouble(dataSet2.Tables[3].Rows[0]["BusFee"].ToString());
                }
                if (dataSet2.Tables[4].Rows[0]["BooksFee"].ToString() != "0.00")
                {
                    stringBuilder.Append("<tr><td style='border-bottom:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Books Fee  </td><td align='center' style='border-left:0;border-bottom:0;'>.............</td><td style='border-bottom:0;' align='right'> " + string.Format("{0:F2}", dataSet2.Tables[4].Rows[0]["BooksFee"].ToString()) + "</td></tr>");
                    num4 += Convert.ToDouble(dataSet2.Tables[4].Rows[0]["BooksFee"].ToString());
                }
                stringBuilder.Append("<tr><td colspan='2' align='right'><strong>TOTAL</strong></TD>");
                stringBuilder.Append("<td align='right'>");
                stringBuilder.Append(string.Format("{0:F2}", num4));
                stringBuilder.Append("</td></tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:14px;'>");
                stringBuilder.Append("<tr><td colspan='4'></strong>&nbsp;(<strong>In Words :</strong> Rupees&nbsp;" + NumberToText(num4.ToString()) + "Only)</td><td>&nbsp;</td></tr>");
                stringBuilder.Append("<tr><td colspan='3' style='font-size:12px'>&nbsp;Received Rs " + string.Format("{0:F2}", dataSet2.Tables[1].Rows[0]["Fee"].ToString()) + " through " + dataSet2.Tables[1].Rows[0]["PaymentMode"].ToString());
                if (dataSet2.Tables[1].Rows[0]["DrawanOnBank"].ToString().Trim() != "")
                    stringBuilder.Append(" Vide </br>&nbsp;Instrument No : <u> " + dataSet2.Tables[1].Rows[0]["ChequeNo"] + "</u>, Dated : <u>" + dataSet2.Tables[1].Rows[0]["ChequeDate"].ToString().Trim() + "</u>, Bank Name : <u>" + dataSet2.Tables[1].Rows[0]["DrawanOnBank"].ToString().Trim() + "</u>");
                stringBuilder.Append("</td></tr>");
                stringBuilder.Append("<tr style='height:30px;'><td colspan='2'></td><td align='right'><b>Received By " + dataSet2.Tables[1].Rows[0]["FullName"].ToString() + "</b></td></tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td colspan='1' align='left' style='font-size:13px;width:160px;'>(Student Copy)</td>");
                stringBuilder.Append("<td colspan='3' align='left' style='font-size:10px'><u>This is a computer generated receipt</u></td></tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("</td></tr>");
                stringBuilder.Append("</table>");
                Session["ConsolidatedReciept"] = stringBuilder.ToString();
            }
            else
                getMiscdata();
        }
        else
        {
            DataSet dataSet2 = clsDal.GetDataSet("Ps_GetConsolidateDupRcpt", hashtable);
            if (dataSet2.Tables[6].Rows[0]["Donation"].ToString() == "0.00")
            {
                DataTable table = dataSet2.Tables[0];
                if (table.Rows.Count <= 0)
                    return;
                string str = Information();
                double num1 = 0.0;
                StringBuilder stringBuilder1 = new StringBuilder();
                stringBuilder1.Append("<center>");
                stringBuilder1.Append(str);
                stringBuilder1.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:14px;'>");
                stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
                stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td style='width:25%'><strong>Date:&nbsp;</strong>" + dataSet2.Tables[1].Rows[0]["RecvDate"].ToString() + "</td>");
                stringBuilder1.Append("<td  style='width:35%'><strong>Admn No.:&nbsp;</strong>" + txtadminno.Text.ToString() + "&nbsp;&nbsp;&nbsp;</td>");
                stringBuilder1.Append("<td><strong>Receipt No:</strong>&nbsp;" + drpReciept.SelectedValue.ToString() + "</td><td>&nbsp;</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td style='width:60%' colspan='2'><strong>Student Name :&nbsp;</strong>" + table.Rows[0][0].ToString() + "</td><td colspan='3'><strong>Class:</strong>&nbsp;" + table.Rows[0][1].ToString() + "&nbsp;</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("<tr>");
                stringBuilder1.Append("<td colspan='4'><strong>Fee Details:</strong>&nbsp;" + dataSet2.Tables[1].Rows[0]["Description"].ToString() + "</td>");
                stringBuilder1.Append("</tr>");
                stringBuilder1.Append("</table>");
                stringBuilder1.Append("</center>");
                StringBuilder stringBuilder2 = new StringBuilder();
                stringBuilder2.Append("<center>");
                stringBuilder2.Append("<table style='font-size:14px;' border=1 width='85%' cellspacing=0 cellpadding=0><tr><td colspan=2 align=center>");
                stringBuilder2.Append("<strong>PARTICULARS</strong></td><td align=center><strong>AMOUNT</strong></td></tr>");
                stringBuilder2.Append("<tr><td colspan='2' style='border-bottom:0;border-top:0;'>&nbsp;");
                stringBuilder2.Append("");
                stringBuilder2.Append("</td><td style='border-bottom:0;border-top:0;'>&nbsp;</td></tr>");
                stringBuilder2.Append("<tr><td style='border-bottom:0;border-top:0;border-right:0;'>&nbsp;&nbsp;&nbsp;&nbsp;School Fee</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>.............</td>");
                stringBuilder2.Append("<td align='right' style='border-bottom:0;border-top:0;'>");
                double num2 = num1 + Convert.ToDouble(dataSet2.Tables[2].Rows[0][0].ToString());
                stringBuilder2.Append(string.Format("{0:F2}", num2));
                stringBuilder2.Append("</td></tr>");
                if (dataSet2.Tables[3].Rows[0]["Fine"].ToString() != "0.00")
                {
                    stringBuilder2.Append("<tr><td style='border-bottom:0;border-top:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Fine Amount  </td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>.............</td><td style='border-bottom:0;border-top:0;' align='right'> " + string.Format("{0:F2}", dataSet2.Tables[3].Rows[0]["Fine"].ToString()) + "</td></tr>");
                    num2 += Convert.ToDouble(dataSet2.Tables[3].Rows[0]["Fine"].ToString());
                }
                if (dataSet2.Tables[4].Rows[0]["BusFee"].ToString() != "0.00")
                {
                    stringBuilder2.Append("<tr><td style='border-bottom:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Bus Fee  </td><td align='center' style='border-left:0;border-bottom:0;'>.............</td><td style='border-bottom:0;' align='right'> " + string.Format("{0:F2}", dataSet2.Tables[4].Rows[0]["BusFee"].ToString()) + "</td></tr>");
                    num2 += Convert.ToDouble(dataSet2.Tables[4].Rows[0]["BusFee"].ToString());
                }
                if (dataSet2.Tables[5].Rows[0]["BooksFee"].ToString() != "0.00")
                {
                    stringBuilder2.Append("<tr><td style='border-bottom:0;border-right:0;'> &nbsp;&nbsp;&nbsp;&nbsp;Books Fee  </td><td align='center' style='border-left:0;border-bottom:0;'>.............</td><td style='border-bottom:0;' align='right'> " + string.Format("{0:F2}", dataSet2.Tables[5].Rows[0]["BooksFee"].ToString()) + "</td></tr>");
                    num2 += Convert.ToDouble(dataSet2.Tables[5].Rows[0]["BooksFee"].ToString());
                }
                stringBuilder2.Append("<tr><td colspan='2' align='right'><strong>TOTAL</strong></TD>");
                stringBuilder2.Append("<td align='right'>");
                stringBuilder2.Append(string.Format("{0:F2}", num2));
                stringBuilder2.Append("</td></tr>");
                stringBuilder2.Append("</table>");
                stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:12px;'>");
                stringBuilder2.Append("<tr><td>&nbsp;</td></tr>");
                stringBuilder2.Append("<tr><td colspan='2'></strong>&nbsp;(<strong>In Words :</strong> Rupees&nbsp;" + NumberToText(num2.ToString()) + "Only)</td><td>&nbsp;</td></tr>");
                stringBuilder2.Append("<tr><td colspan='3'><b>NOTE:</b></td></tr>");
                stringBuilder2.Append("<tr><td colspan='3' style='font-size:12px'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;1. Total amount received - Rs : " + string.Format("{0:F2}", dataSet2.Tables[1].Rows[0]["Fee"].ToString()) + " through " + dataSet2.Tables[1].Rows[0]["PaymentMode"].ToString() + "  Payment");
                if (dataSet2.Tables[1].Rows[0]["DrawanOnBank"].ToString().Trim() != "")
                    stringBuilder2.Append(" Vide </br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Instrument No : <u> " + dataSet2.Tables[1].Rows[0]["ChequeNo"] + "</u>, Dated : <u>" + dataSet2.Tables[1].Rows[0]["ChequeDate"].ToString().Trim() + "</u>, Bank Name : <u>" + dataSet2.Tables[1].Rows[0]["DrawanOnBank"].ToString().Trim() + "</u>");
                stringBuilder2.Append("</td></tr>");
                stringBuilder2.Append("<tr><td colspan='3' style='font-size:12px'> &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;2. Advance Amount Paid - Rs : " + string.Format("{0:F2}", dataSet2.Tables[7].Rows[0]["AdvFee"].ToString()) + " </td></tr>");
                stringBuilder2.Append("<tr><td colspan='2'></td><td align='center' style='font-size:14px'><b>Receiver's</b></td></tr>");
                stringBuilder2.Append("<tr><td colspan='2'></td><td align='center' style='font-size:14px'><b>Signature</b></td></tr>");
                stringBuilder2.Append("</table>");
                stringBuilder2.Append("</center>");
                Session["ConsolidatedReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
            }
            else
                getMiscdata();
        }
    }

    private void getMiscdata()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = clsDal.GetDataTable("Ps_Sp_Get_MiscRcptDtl", new Hashtable()
    {
      {
         "@ReceiptNo",
         drpReciept.SelectedValue.ToString().Trim()
      }
    });
        if (dataTable2.Rows.Count <= 0)
            return;
        if (dataTable2.Rows[0]["Description"].ToString().Trim() != "Prospectus Sale")
            Miscellaneous();
        else
            ProspectusDtl();
        btnPrint.Enabled = true;
    }

    private void Miscellaneous()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@ReceiptNo", drpReciept.SelectedValue.ToString().Trim());
        DataTable dataTable2 = clsDal.GetDataTable("Ps_Sp_Get_MiscIncomeDtl", hashtable);
        if (dataTable2.Rows.Count <= 0)
            return;
        btnPrint.Visible = true;
        string str1 = Information();
        string str2 = "";
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<center>");
        stringBuilder.Append("<table width='100%'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='border-right:dotted 1px black;'>");
        stringBuilder.Append(str1);
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td>");
        stringBuilder.Append(str1);
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='border-right:dotted 1px black;'>");
        stringBuilder.Append("<table width='100%'>");
        stringBuilder.Append("<tr><td style='font-size: 16px;'>");
        stringBuilder.Append("Receipt No: <font style='font-family:Arial Rounded MT Bold; font-size:18px;'>" + dataTable2.Rows[0]["InvoiceReceiptNo"].ToString() + "</font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td style='font-size: 16px;' align='right'>");
        stringBuilder.Append("Date : <span style='font-size:17px;'>" + dataTable2.Rows[0]["RecvDateStr"].ToString() + "</span>");
        stringBuilder.Append("</td></tr>");
        dataTable2.Rows[0]["Description"].ToString().Trim();
        if (dataTable2.Rows[0]["PartyId"].ToString().Trim() != "" && Convert.ToInt64(dataTable2.Rows[0]["PartyId"].ToString().Trim()) > 1000000000L)
        {
            str2 = new clsDAL().ExecuteScalarQry("select FatherName from dbo.PS_StudMaster where AdmnNo=" + drpstudent.SelectedValue.ToString().Trim());
            stringBuilder.Append("<tr><td style='font-size: 17px;  line-height:25px;' colspan='2'>Received with thanks from Sri/Smt <span style='border-bottom:dotted 1px black;'>&nbsp;<b>" + str2 + "</b>&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;</span><br />");
        }
        else
            stringBuilder.Append("<tr><td style='font-size: 17px;  line-height:25px;' colspan='2'>Received with thanks from Sri/Smt <span style='border-bottom:dotted 1px black;'>&nbsp;<b>" + dataTable2.Rows[0]["SNAME"].ToString() + "</b>&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;</span><br />");
        stringBuilder.Append(" the sum of&nbsp;&nbsp;");
        stringBuilder.Append(" <span style='border-bottom:dotted 1px black;'><b> Rupees&nbsp; " + NumberToText(dataTable2.Rows[0]["Amount"].ToString()) + "  only </b></span> towards " + dataTable2.Rows[0]["Description"].ToString() + " by <span style='border-bottom:dotted 1px black;'><b>" + dataTable2.Rows[0]["PmtMode"].ToString() + "</b></span></i>");
        if (dataTable2.Rows[0]["PmtMode"].ToString().Trim() != "Cash")
            stringBuilder.Append("vide Instrument No : <span style='border-bottom:dotted 1px black;'><b>" + dataTable2.Rows[0]["InstrumentNo"].ToString() + "</b></span>,  Dated : <span style='border-bottom:dotted 1px black;'><b>" + dataTable2.Rows[0]["InstrumentDt"].ToString() + "</b></span> , Bank Name : <span style='border-bottom:dotted 1px black;'><b>" + dataTable2.Rows[0]["DrawanOnBank"].ToString() + "</b></span>");
        stringBuilder.Append(" &nbsp; .</td></tr><tr><td style='width: 70%;' align='left'valign='top'><br /><div style='width:165px; font-family:Arial Rounded MT Bold; border:solid 1px black; height:30px;'><div style='width:35px; float:left; border-right:solid 1px black; height:30px;'><img src='../images/Rs.jpg' width='35px' height='30px'/></div><div style='float:right;padding-top:4px; font-size:18px;'><b> " + dataTable2.Rows[0]["Amount"].ToString() + "&nbsp;</b></div></div></td><td align='center' style='font-size: 15px;'> <br /> <br /> <br />Receiver's<br />  Signature</td></tr></table>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td>");
        stringBuilder.Append("<table width='100%'>");
        stringBuilder.Append("<tr><td style='font-size: 16px;'>");
        stringBuilder.Append("Receipt No: <font style='font-family:Arial Rounded MT Bold; font-size:18px;'>" + dataTable2.Rows[0]["InvoiceReceiptNo"].ToString() + "</font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td style='font-size: 16px;' align='right'>");
        stringBuilder.Append("Date : <span style='font-size:17px;'>" + dataTable2.Rows[0]["RecvDateStr"].ToString() + "</span>");
        stringBuilder.Append("</td></tr>");
        if (dataTable2.Rows[0]["PartyId"].ToString().Trim() != "" && Convert.ToInt64(dataTable2.Rows[0]["PartyId"].ToString().Trim()) > 1000000000L)
            stringBuilder.Append("<tr><td style='font-size: 17px;  line-height:25px;' colspan='2'>Received with thanks from Sri/Smt <span style='border-bottom:dotted 1px black;'>&nbsp;<b>" + str2 + "</b>&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;</span><br />");
        else
            stringBuilder.Append("<tr><td style='font-size: 17px;  line-height:25px;' colspan='2'>Received with thanks from Sri/Smt <span style='border-bottom:dotted 1px black;'>&nbsp;<b>" + dataTable2.Rows[0]["SNAME"].ToString() + "</b>&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;</span><br />");
        stringBuilder.Append(" the sum of&nbsp;&nbsp;");
        stringBuilder.Append(" <span style='border-bottom:dotted 1px black;'><b> Rupees&nbsp; " + NumberToText(dataTable2.Rows[0]["Amount"].ToString()) + "  only </b></span> towards " + dataTable2.Rows[0]["Description"].ToString() + " by <span style='border-bottom:dotted 1px black;'><b>" + dataTable2.Rows[0]["PmtMode"].ToString() + "</b></span></i>");
        if (dataTable2.Rows[0]["PmtMode"].ToString().Trim() != "Cash")
            stringBuilder.Append("vide Instrument No : <span style='border-bottom:dotted 1px black;'><b>" + dataTable2.Rows[0]["InstrumentNo"].ToString() + "</b></span>,  Dated : <span style='border-bottom:dotted 1px black;'><b>" + dataTable2.Rows[0]["InstrumentDt"].ToString() + "</b></span> , Bank Name : <span style='border-bottom:dotted 1px black;'><b>" + dataTable2.Rows[0]["DrawanOnBank"].ToString() + "</b></span>");
        stringBuilder.Append(" &nbsp; .</td></tr><tr><td style='width: 70%;' align='left'valign='top'><br /><div style='width:165px; font-family:Arial Rounded MT Bold; border:solid 1px black; height:30px;'><div style='width:35px; float:left; border-right:solid 1px black; height:30px;'><img src='../images/Rs.jpg' width='35px' height='30px'/></div><div style='float:right;padding-top:4px; font-size:18px;'><b> " + dataTable2.Rows[0]["Amount"].ToString() + "&nbsp;</b></div></div></td><td align='center' style='font-size: 15px;'> <br /> <br /> <br />Receiver's<br />  Signature</td></tr></table>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("</center>");
        lbldetail.Text = stringBuilder.ToString();
        Session["PrintReciept"] = stringBuilder.ToString();
    }

    private void ProspectusDtl()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@MR_No", drpReciept.SelectedValue.ToString().Trim());
        if (Convert.ToInt32(drpSession.SelectedValue.ToString().Trim().Split('-')[0].Trim()) >= 2014)
        {
            DataTable dataTable2 = clsDal.GetDataTable("Ps_Sp_StudMiscRcpt", hashtable);
            if (dataTable2.Rows.Count <= 0)
                return;
            btnPrint.Visible = true;
            string str = Information();
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:16px;' class='tbltd'>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='border-right:dotted 1px black;padding-right:10px;'>");
            stringBuilder1.Append(str);
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td style='padding-left:10px;'>");
            stringBuilder1.Append(str);
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='border-right:dotted 1px black;padding-right:10px;' align='center'>");
            stringBuilder1.Append("<table style='height:16px' style='font-size:17px;'>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:50%' ><strong>Date:&nbsp;</strong>" + DateTime.Now.ToString("dd-MM-yyyy") + "</td>");
            stringBuilder1.Append("<td style='width:50%' ><strong>Receipt No:</strong>&nbsp;" + drpReciept.SelectedValue.ToString().Trim() + "</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:70%' colspan='3'><strong>Name of the Pupil:&nbsp;</strong>" + dataTable2.Rows[0]["RcvdFrom"].ToString() + "</td><td >&nbsp;</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td><strong>Class:</strong>&nbsp;" + dataTable2.Rows[0]["ClassName"].ToString() + "</td><td>&nbsp;</td>");
            stringBuilder1.Append("<td><strong>&nbsp;</strong></td><td>&nbsp;</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("</table>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td style='padding-left:10px;' align='center'>");
            stringBuilder1.Append("<table style='height:16px' style='font-size:17px;'>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:50%' ><strong>Date:&nbsp;</strong>" + DateTime.Now.ToString("dd-MM-yyyy") + "</td>");
            stringBuilder1.Append("<td style='width:50%' ><strong>Receipt No:</strong>&nbsp;" + drpReciept.SelectedValue.ToString().Trim() + "</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:70%' colspan='3'><strong>Name of the Pupil:&nbsp;</strong>" + dataTable2.Rows[0]["RcvdFrom"].ToString() + "</td><td >&nbsp;</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td><strong>Class:</strong>&nbsp;" + dataTable2.Rows[0]["ClassName"].ToString() + "</td><td>&nbsp;</td>");
            stringBuilder1.Append("<td><strong>&nbsp;</strong></td><td>&nbsp;</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("</table>");
            stringBuilder1.Append("</td>");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<table style='font-size:18px;' border=1 width='85%' cellspacing=0 cellpadding=0 class='tbltd'><tr><td colspan=3 align=center>");
            stringBuilder2.Append("<strong>PARTICULARS</strong></td><td align=center><strong>AMOUNT</strong></td></tr>");
            stringBuilder2.Append("<tr><td align='center' style='width: 50px; border-right:0; border-bottom:0; border-top:0;'>1</td><td style='border-right:0;border-bottom:0;border-top:0; border-left:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
            stringBuilder2.Append(dataTable2.Rows[0]["Description"].ToString());
            stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
            stringBuilder2.Append(".............");
            stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
            stringBuilder2.Append(dataTable2.Rows[0]["AmountStr"].ToString());
            stringBuilder2.Append("</td></tr>");
            stringBuilder2.Append("<tr></tr><tr></tr><tr></tr><tr></tr>");
            stringBuilder2.Append("<tr><td colspan='3'></td></tr>");
            stringBuilder2.Append("<tr><td colspan='3' align='right'><strong>TOTAL</strong></TD>");
            stringBuilder2.Append("<td align='right'>");
            double num = 0.0;
            if (dataTable2.Rows.Count > 0)
                num = Convert.ToDouble(dataTable2.Rows[0]["AmountStr"].ToString());
            stringBuilder2.Append(string.Format("{0:F2}", num));
            stringBuilder2.Append("</td></tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' class='tbltd'>");
            stringBuilder2.Append("<tr><td>&nbsp;</td></tr>");
            stringBuilder2.Append("<tr><td  style='font-size:16px'></strong>&nbsp;(In Words Rupees&nbsp;<strong>" + NumberToText(num.ToString()) + "</strong>Only)");
            if (dataTable2.Rows[0]["PmtMode"].ToString().Trim() == "Cash")
                stringBuilder2.Append(" Throuch Cash</td></tr>");
            else
                stringBuilder2.Append(" Throuch Bank(" + dataTable2.Rows[0]["DrawnOnBank"].ToString() + "/" + dataTable2.Rows[0]["InstrumentDt"].ToString() + "/" + dataTable2.Rows[0]["InstrumentNo"].ToString() + "</td></tr>");
            stringBuilder2.Append("<tr><td colspan='2'></td><td align='right' style='font-size:14px'><b><br />Received By " + dataTable2.Rows[0]["FullName"].ToString() + "</b></td></tr>");
            stringBuilder2.Append("<tr><td colspan='3' align='center' style='font-size:10px'><u>This is a computer generated receipt</u></td></tr>");
            stringBuilder2.Append("</table>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='border-right:dotted 1px black;padding-right:10px;'>");
            stringBuilder1.Append("<center>");
            stringBuilder1.Append(stringBuilder2.ToString());
            stringBuilder1.Append("</center>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("<td style='padding-left:5px;'>");
            stringBuilder1.Append("<center>");
            stringBuilder1.Append(stringBuilder2.ToString());
            stringBuilder1.Append("</center>");
            stringBuilder1.Append("</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("</table>");
            lbldetail.Text = stringBuilder1.ToString();
            Session["PrintReciept"] = stringBuilder1.ToString();
        }
        else
        {
            DataTable dataTable2 = clsDal.GetDataTable("Ps_Sp_IndivProspectus", hashtable);
            if (dataTable2.Rows.Count <= 0)
                return;
            btnPrint.Visible = true;
            string str = Information();
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("<center>");
            stringBuilder1.Append(str);
            stringBuilder1.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:14px;' class='tbltd'>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:50%' colspan='2'><strong>Date:&nbsp;</strong>" + DateTime.Now.ToString("dd-MM-yyyy") + "</td>");
            stringBuilder1.Append("<td style='width:50%' colspan='2'><strong>Receipt No:</strong>&nbsp;" + dataTable2.Rows[0]["MR_No"].ToString() + "</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:70%' colspan='3'><strong>Name of the Pupil:&nbsp;</strong>" + dataTable2.Rows[0]["StudentName"].ToString() + "</td><td >&nbsp;</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td><strong>Class:</strong>&nbsp;" + dataTable2.Rows[0]["ClassName"].ToString() + "</td><td>&nbsp;</td>");
            stringBuilder1.Append("<td><strong>&nbsp;</strong></td><td>&nbsp;</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("</table>");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<table style='font-size:14px;' border=1 width='85%' cellspacing=0 cellpadding=0 class='tbltd'><tr><td colspan=3 align=center>");
            stringBuilder2.Append("<strong>PARTICULARS</strong></td><td align=center><strong>AMOUNT</strong></td></tr>");
            stringBuilder2.Append("<tr><td align='center' style='width: 50px; border-right:0; border-bottom:0; border-top:0;'>1</td><td style='border-right:0;border-bottom:0;border-top:0; border-left:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
            stringBuilder2.Append("Prospectus Sale");
            stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
            stringBuilder2.Append(".............");
            stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
            stringBuilder2.Append(dataTable2.Rows[0]["Amount"].ToString());
            stringBuilder2.Append("</td></tr>");
            stringBuilder2.Append("<tr></tr><tr></tr><tr></tr><tr></tr>");
            stringBuilder2.Append("<tr><td colspan='3'></td></tr>");
            stringBuilder2.Append("<tr><td colspan='3' align='right'><strong>TOTAL</strong></TD>");
            stringBuilder2.Append("<td align='right'>");
            double num = 0.0;
            if (dataTable2.Rows.Count > 0)
                num = Convert.ToDouble(dataTable2.Rows[0]["Amount"].ToString());
            stringBuilder2.Append(string.Format("{0:F2}", num));
            stringBuilder2.Append("</td></tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' class='tbltd'>");
            stringBuilder2.Append("<tr><td>&nbsp;</td></tr>");
            stringBuilder2.Append("<tr><td  style='font-size:12px'></strong>&nbsp;(In Words Rupees&nbsp;<strong>" + NumberToText(num.ToString()) + "</strong>Only)</td><td>&nbsp;</td></tr>");
            stringBuilder2.Append("<tr><td colspan='2'></td><td  style='font-size:12px'><b><br />Receiver's<br />  Signature</b></td></tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("</center>");
            lbldetail.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
            Session["PrintReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
        }
    }

    private void createRemarkReport(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            double num = 0.0;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table width='85%'  cellpadding='1' cellspacing='1' style='background-color:#CCC;'>");
            stringBuilder.Append("<tr><td class='tblheader'>Due Date</td><td class='tblheader'>Additional Charges</td><td style='width: 100px;' class='tblheader'>Balance Amount</td></tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
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
            stringBuilder.Append("<tr><td style='text-align:right;' colspan='2'><strong>Total Amount</strong></td><td style='text-align:right;'><strong>" + num.ToString().Trim() + ".00</strong></td></tr>");
            stringBuilder.Append("</table>");
            lbldetail.Text = stringBuilder.ToString().Trim();
            Session["PrintAdditionalReciept"] = stringBuilder.ToString().Trim();
        }
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<div class='error'>No Additional Charges Exist</div>");
            lbldetail.Text = stringBuilder.ToString().Trim();
        }
    }

    private string Information()
    {
        clsDAL clsDal1 = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataSet dataSet = new DataSet();
        StringBuilder stringBuilder = new StringBuilder();
        clsDAL clsDal2 = new clsDAL();
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
                    stringBuilder.Append("<tr><td colspan='3'><div style='float:right; font-weight:bold;' class='tbltxt'><u>Duplicate</u></div></td></tr>");
                    stringBuilder.Append("<tr><td rowspan='4'> <img src='../images/leftlogo.jpg' width='75px' height='80px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + clsDal2.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + clsDal2.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + "," + clsDal2.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</strong></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>" + clsDal2.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + clsDal2.Decrypt(row["Pin"].ToString().Trim(), str2) + "</strong> </td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><strong>Phone-" + clsDal2.Decrypt(row["Phone"].ToString().Trim(), str2) + "</strong></td></tr><tr><td colspan='5'></td></tr></table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected void txtadminno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lbldetail.Text = "";
            btnConsolidat.Enabled = false;
            btnPrint.Enabled = false;
            DataTable dataTableQry = new clsDAL().GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent where AdmnNo=" + txtadminno.Text.Trim() + " and Detained_Promoted=''");
            if (dataTableQry.Rows.Count <= 0)
                return;
            drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
            fillclass();
            drpClasses.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
            fillstudent();
            drpstudent.SelectedValue = txtadminno.Text.Trim();
            fillfeereciept();
        }
        catch (Exception ex)
        {
            if (!(ex.Message != ""))
                return;
            ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Invalid Data')", true);
        }
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

    protected void btnConsolidat_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('rptConsolidatePrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('rptRcptPrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnFillRcpt_Click(object sender, EventArgs e)
    {
        fillMiscRcpt();
    }

    private void fillMiscRcpt()
    {
        drpReciept.DataSource = new clsDAL().GetDataTable("Ps_Sp_Get_MiscRcptNo", new Hashtable()
    {
      {
         "@FrmDt",
         dtpFromDate.GetDateValue().ToString("MM/dd/yyy")
      },
      {
         "@ToDt",
         dtpToDate.GetDateValue().ToString("MM/dd/yyy")
      }
    });
        drpReciept.DataTextField = "Receipt_Dt";
        drpReciept.DataValueField = "InvoiceReceiptNo";
        drpReciept.DataBind();
        drpReciept.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    protected void rbtnStud_CheckedChanged(object sender, EventArgs e)
    {
        lbldetail.Text = string.Empty;
        if (rbtnStud.Checked.Equals(true))
            Enable();
        else
            Disable();
    }

    private void Disable()
    {
        drpSession.Enabled = false;
        drpClasses.Enabled = false;
        drpstudent.Enabled = false;
        txtadminno.Enabled = false;
        btnFillRcpt.Enabled = true;
        txtFromDate.Enabled = true;
        txtToDate.Enabled = true;
        drpClasses.SelectedIndex = -1;
        drpstudent.SelectedIndex = -1;
        txtadminno.Text = string.Empty;
        drpReciept.Items.Clear();
    }

    private void Enable()
    {
        drpSession.Enabled = true;
        drpClasses.Enabled = true;
        drpstudent.Enabled = true;
        txtadminno.Enabled = true;
        btnFillRcpt.Enabled = false;
        txtFromDate.Enabled = false;
        txtToDate.Enabled = false;
        drpReciept.Items.Clear();
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
    }
}
