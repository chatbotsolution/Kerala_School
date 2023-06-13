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

public partial class Accounts_PrintDupBill : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        Session["PrntConsoldtReciept"] = null;
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
        drpClasses.DataSource = new clsDAL().GetDataTableQry("SELECT ClassID,ClassName FROM dbo.PS_ClassMaster");
        drpClasses.DataTextField = "ClassName";
        drpClasses.DataValueField = "ClassID";
        drpClasses.DataBind();
        drpClasses.Items.Insert(0, new ListItem("All", "0"));
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

    protected void drpClasses_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
        lbldetail.Text = "";
        txtadminno.Text = "0";
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
        lbldetail.Text = "";
        btnPrint.Enabled = false;
        btnConsolidat.Enabled = false;
        txtadminno.Text = "0";
    }

    private void fillstudent()
    {
        DataTable dataTableQry;
        if (drpClasses.SelectedIndex != 0)
            dataTableQry = obj.GetDataTableQry("select fullname,cs.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.classid=" + drpClasses.SelectedValue + " and cs.SessionYear='" + drpSession.SelectedValue.ToString() + "' and CS.Detained_Promoted='' order by fullname");
        else
            dataTableQry = obj.GetDataTableQry("select  fullname,cs.admnno  from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.SessionYear='" + drpSession.SelectedValue.ToString() + "' and CS.Detained_Promoted='' order by fullname");
        drpstudent.DataSource = dataTableQry;
        drpstudent.DataTextField = "fullname";
        drpstudent.DataValueField = "admnno";
        drpstudent.DataBind();
        drpstudent.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtadminno.Text = drpstudent.SelectedValue;
        lbldetail.Text = "";
        btnPrint.Enabled = false;
        btnConsolidat.Enabled = false;
        fillfeereciept();
    }

    private void fillfeereciept()
    {
        drpReciept.DataSource = new clsDAL().GetDataTable("ACTS_DupRcptStud", new Hashtable()
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
        drpReciept.DataTextField = "TransDtStr";
        drpReciept.DataValueField = "InvoiceReceiptNo";
        drpReciept.DataBind();
        drpReciept.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    protected void btnFillRcpt_Click(object sender, EventArgs e)
    {
        fillMiscRcpt();
    }

    private void fillMiscRcpt()
    {
        drpReciept.DataSource = new clsDAL().GetDataTable("ACTS_DupMiscRcptNo", new Hashtable()
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
        drpReciept.DataTextField = "TransDtStr";
        drpReciept.DataValueField = "InvoiceReceiptNo";
        drpReciept.DataBind();
        drpReciept.Items.Insert(0, new ListItem("-Select-", "0"));
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
        DataSet dataSet2 = clsDal.GetDataSet("ACTS_DupGetRcpt", new Hashtable()
    {
      {
         "@SessionYear",
         drpSession.SelectedValue.ToString().Trim()
      },
      {
         "@AdmnNo",
         drpstudent.SelectedValue.ToString().Trim()
      },
      {
         "@ReceiptNo",
         drpReciept.SelectedValue.ToString().Trim()
      }
    });
        if (dataSet2.Tables.Count <= 3)
        {
            DataTable table1 = dataSet2.Tables[0];
            DataRow row = dataSet2.Tables[1].Rows[0];
            DataTable table2 = dataSet2.Tables[2];
            string str = Information();
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("<center>");
            stringBuilder1.Append(str);
            stringBuilder1.Append("<table cellspacing='0' border='0' width='85%' style='font-size:14px;border-collapse:collapse;' class='tbltd'>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='4' style='font-size:13px' align='left'>");
            stringBuilder1.Append("<strong>Name :&nbsp;</strong>" + table1.Rows[0]["FullName"]);
            stringBuilder1.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Class:</strong>&nbsp;" + table1.Rows[0]["ClassName"] + "&nbsp;");
            stringBuilder1.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Admn No.:&nbsp;</strong>" + table1.Rows[0]["AdmnNo"].ToString());
            stringBuilder1.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Date:&nbsp;</strong>" + row["TransDtStr"].ToString());
            stringBuilder1.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Receipt No:</strong>&nbsp;" + drpReciept.SelectedValue.ToString());
            stringBuilder1.Append("</td></tr>");
            stringBuilder1.Append("<tr><td></td></tr>");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<tr>");
            int num = table2.Rows.Count / 2;
            stringBuilder2.Append("<td><table width='400px'>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='left'><b>Item Name</b></td>");
            stringBuilder2.Append("<td align='left'><b>Qty</b></td>");
            stringBuilder2.Append("<td align='right'><b>Price</b></td>");
            stringBuilder2.Append("<td align='right'><b>Amount</b></td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("<tr><td colspan='4'><hr /></td></tr>");
            int index1;
            for (index1 = 0; index1 <= num - 1; ++index1)
            {
                if (index1 < table2.Rows.Count)
                {
                    stringBuilder2.Append("<tr>");
                    stringBuilder2.Append("<td align='left'>" + table2.Rows[index1]["ItemName"].ToString() + "</td>");
                    stringBuilder2.Append("<td align='left'>" + table2.Rows[index1]["Qty"].ToString() + "</td>");
                    stringBuilder2.Append("<td align='right'>" + table2.Rows[index1]["SalePriceStr"].ToString() + "</td>");
                    stringBuilder2.Append("<td align='right'>" + table2.Rows[index1]["TotAmtStr"].ToString() + "</td></tr>");
                }
            }
            stringBuilder2.Append("</table></center>&nbsp;</td>");
            if (index1 <= table2.Rows.Count)
            {
                stringBuilder2.Append("<td valign='top' colspan='2'><table width='400px'>");
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td align='left'><b>Item Name</b></td>");
                stringBuilder2.Append("<td align='left'><b>Qty</b></td>");
                stringBuilder2.Append("<td align='right'><b>Price</b></td>");
                stringBuilder2.Append("<td align='right'><b>Amount</b></td>");
                stringBuilder2.Append("</tr>");
                stringBuilder2.Append("<tr><td colspan='4'><hr /></td></tr>");
                for (int index2 = index1; index2 <= table2.Rows.Count; ++index2)
                {
                    if (index2 < table2.Rows.Count)
                    {
                        stringBuilder2.Append("<tr>");
                        stringBuilder2.Append("<td align='left'>" + table2.Rows[index2]["ItemName"].ToString() + "</td>");
                        stringBuilder2.Append("<td align='left'>" + table2.Rows[index2]["Qty"].ToString() + "</td>");
                        stringBuilder2.Append("<td align='right'>" + table2.Rows[index2]["SalePriceStr"].ToString() + "</td>");
                        stringBuilder2.Append("<td align='right'>" + table2.Rows[index2]["TotAmtStr"].ToString() + "</td></tr>");
                    }
                }
                stringBuilder2.Append("</table></td>");
            }
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("<tr><td colspan='3'><hr/></td></tr>");
            stringBuilder2.Append("<tr align='left'><td colspan='3'><strong>Total : " + table2.Compute("SUM(TotAmtStr)", "").ToString() + "&nbsp;(Rupees&nbsp;" + NumberToWords(double.Parse(row["Amount"].ToString())) + "Only)</strong></td><td>&nbsp;</td></tr>");
            stringBuilder2.Append("<tr><td></td><td></td><td align='right' style='padding-right:10px;'><b>Receiver's Signature</b></td></tr></table>");
            stringBuilder2.Append("</center>");
            btnPrint.Enabled = true;
            btnConsolidat.Enabled = true;
            lbldetail.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
            Session["PrntDupReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('No such Invoice found !');", true);
    }

    private void getConsolidated()
    {
        clsDAL clsDal = new clsDAL();
        DataSet dataSet1 = new DataSet();
        DataSet dataSet2 = clsDal.GetDataSet("Acts_DupGetRcpt", new Hashtable()
    {
      {
         "@SessionYear",
         drpSession.SelectedValue.ToString().Trim()
      },
      {
         "@AdmnNo",
         drpstudent.SelectedValue.ToString().Trim()
      },
      {
         "@ReceiptNo",
         drpReciept.SelectedValue.ToString().Trim()
      }
    });
        DataTable table = dataSet2.Tables[0];
        if (table.Rows.Count <= 0 || dataSet2.Tables[1].Rows.Count <= 0)
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
        stringBuilder1.Append("<td style='width:25%'><strong>Date:&nbsp;</strong>" + dataSet2.Tables[1].Rows[0]["TransDtStr"].ToString() + "</td>");
        stringBuilder1.Append("<td  style='width:35%'><strong>Admn No.:&nbsp;</strong>" + txtadminno.Text.ToString() + "&nbsp;&nbsp;&nbsp;</td>");
        stringBuilder1.Append("<td><strong>Receipt No:</strong>&nbsp;" + drpReciept.SelectedValue.ToString() + "</td><td>&nbsp;</td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
        stringBuilder1.Append("<tr>");
        stringBuilder1.Append("<td style='width:60%' colspan='2'><strong>Student Name :&nbsp;</strong>" + table.Rows[0][0].ToString() + "</td><td colspan='3'><strong>Class:</strong>&nbsp;" + table.Rows[0][1].ToString() + "&nbsp;</td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("<tr>");
        stringBuilder1.Append("<td colspan='4'><strong>Details:</strong>&nbsp;" + dataSet2.Tables[1].Rows[0]["Description"].ToString() + "</td>");
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
        stringBuilder2.Append("<tr><td style='border-bottom:0;border-top:0;border-right:0;'>&nbsp;&nbsp;&nbsp;&nbsp;Receipt Amount</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>.............</td>");
        stringBuilder2.Append("<td align='right' style='border-bottom:0;border-top:0;'>");
        double num2 = num1 + Convert.ToDouble(dataSet2.Tables[1].Rows[0]["Amount"].ToString());
        stringBuilder2.Append(string.Format("{0:F2}", num2));
        stringBuilder2.Append("</td></tr>");
        stringBuilder2.Append("<tr><td colspan='2' align='right'><strong>TOTAL</strong></TD>");
        stringBuilder2.Append("<td align='right'>");
        stringBuilder2.Append(string.Format("{0:F2}", num2));
        stringBuilder2.Append("</td></tr>");
        stringBuilder2.Append("</table>");
        stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='85%' style='font-size:12px;'>");
        stringBuilder2.Append("<tr><td>&nbsp;</td></tr>");
        stringBuilder2.Append("<tr><td colspan='2'></strong>&nbsp;(<strong>In Words :</strong> Rupees&nbsp;" + NumberToText(num2.ToString()) + "Only)</td><td>&nbsp;</td></tr>");
        stringBuilder2.Append("<tr><td colspan='3'><b>NOTE:</b></td></tr>");
        stringBuilder2.Append("<tr><td colspan='3' style='font-size:12px'>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;1. Total amount received - Rs : " + string.Format("{0:F2}", dataSet2.Tables[1].Rows[0]["Amount"].ToString()) + " through " + dataSet2.Tables[1].Rows[0]["PmtMode"].ToString() + "  Payment");
        if (dataSet2.Tables[1].Rows[0]["DrawanOnBank"].ToString().Trim() != "")
            stringBuilder2.Append(" Vide </br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Instrument No : <u> " + dataSet2.Tables[1].Rows[0]["InstrumentNo"] + "</u>, Dated : <u>" + dataSet2.Tables[1].Rows[0]["InstrumentDtStr"].ToString().Trim() + "</u>, Bank Name : <u>" + dataSet2.Tables[1].Rows[0]["DrawanOnBank"].ToString().Trim() + "</u>");
        stringBuilder2.Append("</td></tr>");
        stringBuilder2.Append("<tr><td colspan='2'></td><td align='center' style='font-size:14px'><b>Receiver's</b></td></tr>");
        stringBuilder2.Append("<tr><td colspan='2'></td><td align='center' style='font-size:14px'><b>Signature</b></td></tr>");
        stringBuilder2.Append("</table>");
        stringBuilder2.Append("</center>");
        Session["PrntConsoldtReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
    }

    private void getMiscdata()
    {
        clsDAL clsDal = new clsDAL();
        DataSet dataSet1 = new DataSet();
        DataSet dataSet2 = clsDal.GetDataSet("ACTS_DupMiscRcptDtl", new Hashtable()
    {
      {
         "@ReceiptNo",
         drpReciept.SelectedValue.ToString().Trim()
      }
    });
        if (dataSet2.Tables.Count <= 3)
        {
            DataRow row = dataSet2.Tables[0].Rows[0];
            DataTable table = dataSet2.Tables[1];
            string str = Information();
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("<center>");
            stringBuilder1.Append(str);
            stringBuilder1.Append("<table cellspacing='0' border='0' width='85%' style='font-size:14px;border-collapse:collapse;' class='tbltd'>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td colspan='4' style='width:40%;font-size:12px' align='left'>");
            stringBuilder1.Append("<strong>Name :&nbsp;</strong>" + row["RcvdFrom"]);
            stringBuilder1.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Date:&nbsp;</strong>" + row["TransDtStr"].ToString());
            stringBuilder1.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<strong>Receipt No:</strong>&nbsp;" + drpReciept.SelectedValue.ToString());
            stringBuilder1.Append("</td></tr>");
            stringBuilder1.Append("<tr><td></td></tr>");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<tr>");
            int num = table.Rows.Count / 2;
            stringBuilder2.Append("<td><table width='370px'>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='left'><b>Item Name</b></td>");
            stringBuilder2.Append("<td align='left'><b>Qty</b></td>");
            stringBuilder2.Append("<td align='right'><b>Price</b></td>");
            stringBuilder2.Append("<td align='right'><b>Amount</b></td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("<tr><td colspan='4'><hr /></td></tr>");
            int index1;
            for (index1 = 0; index1 <= num - 1; ++index1)
            {
                if (index1 < table.Rows.Count)
                {
                    stringBuilder2.Append("<tr>");
                    stringBuilder2.Append("<td align='left'>" + table.Rows[index1]["ItemName"].ToString() + "</td>");
                    stringBuilder2.Append("<td align='left'>" + table.Rows[index1]["Qty"].ToString() + "</td>");
                    stringBuilder2.Append("<td align='left'>" + table.Rows[index1]["SalePriceStr"].ToString() + "</td>");
                    stringBuilder2.Append("<td align='right'>" + table.Rows[index1]["TotAmtStr"].ToString() + "</td></tr>");
                }
            }
            stringBuilder2.Append("</table></center>&nbsp;</td>");
            if (index1 <= table.Rows.Count)
            {
                stringBuilder2.Append("<td valign='top' colspan='2'><table width='400px'>");
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td align='left'><b>Item Name</b></td>");
                stringBuilder2.Append("<td align='left'><b>Qty</b></td>");
                stringBuilder2.Append("<td align='right'><b>Price</b></td>");
                stringBuilder2.Append("<td align='right'><b>Amount</b></td>");
                stringBuilder2.Append("</tr>");
                stringBuilder2.Append("<tr><td colspan='4'><hr /></td></tr>");
                for (int index2 = index1; index2 <= table.Rows.Count; ++index2)
                {
                    if (index2 < table.Rows.Count)
                    {
                        stringBuilder2.Append("<tr>");
                        stringBuilder2.Append("<td align='left'>" + table.Rows[index2]["ItemName"].ToString() + "</td>");
                        stringBuilder2.Append("<td align='left'>" + table.Rows[index2]["Qty"].ToString() + "</td>");
                        stringBuilder2.Append("<td align='right'>" + table.Rows[index2]["SalePriceStr"].ToString() + "</td>");
                        stringBuilder2.Append("<td align='right'>" + table.Rows[index2]["TotAmtStr"].ToString() + "</td></tr>");
                    }
                }
                stringBuilder2.Append("</table></td>");
            }
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("<tr><td colspan='3'><hr/></td></tr>");
            stringBuilder2.Append("<tr align='left'><td colspan='3'><strong>Total : " + table.Compute("SUM(TotAmtStr)", "").ToString() + "&nbsp;(Rupees&nbsp;" + NumberToWords(double.Parse(row["Amount"].ToString())) + "Only)</strong></td><td>&nbsp;</td></tr>");
            stringBuilder2.Append("<tr><td colspan='2'></td><td align='right' style='padding-right:10px;'><b>Receiver's Signature</b></td></tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("</center>");
            btnPrint.Enabled = true;
            lbldetail.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
            Session["PrntDupReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('No Data found !');", true);
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
            ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Invalid Admission No')", true);
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptDupRcptDtlsPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnConsolidat_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptDupRcptConsPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
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