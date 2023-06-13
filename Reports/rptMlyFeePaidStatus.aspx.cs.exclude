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

public partial class Reports_rptMlyFeePaidStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        txtadminno.Text = "0";
        fillsession();
        fillclass();
        FillClassSection();
        fillstudent();
        btnprint.Visible = false;
    }

    protected void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("--All--", "0"));
    }

    private void FillClassSection()
    {
        drpSection.Items.Clear();
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "@classId",
         drpclass.SelectedValue.ToString().Trim()
      },
      {
         "@session",
         drpSession.SelectedValue.ToString().Trim()
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpclass.SelectedIndex > 0)
        {
            FillClassSection();
            fillstudent();
            drpSection.SelectedIndex = 0;
            lblReport.Text = "";
            btnprint.Visible = false;
            txtadminno.Text = "0";
        }
        else
        {
            drpSection.SelectedIndex = 0;
            drpstudents.SelectedIndex = 0;
            txtadminno.Text = "0";
            lblReport.Text = "";
            btnprint.Visible = false;
        }
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSection.SelectedIndex > 0)
        {
            fillstudent();
            lblReport.Text = "";
            btnprint.Visible = false;
            txtadminno.Text = "0";
        }
        else
        {
            drpstudents.SelectedIndex = 0;
            txtadminno.Text = "0";
            lblReport.Text = "";
            btnprint.Visible = false;
        }
    }

    private void fillstudent()
    {
        drpstudents.Items.Clear();
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@Session", drpSession.SelectedValue);
        if (drpclass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpclass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        DataTable dataTable = common.GetDataTable("ps_sp_get_StudNamesForMly", hashtable);
        drpstudents.DataSource = dataTable;
        drpstudents.DataTextField = "fullname";
        drpstudents.DataValueField = "admnno";
        drpstudents.DataBind();
        if (dataTable.Rows.Count > 0)
            drpstudents.Items.Insert(0, new ListItem("--SELECT--", "0"));
        else
            drpstudents.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void drpstudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpstudents.SelectedIndex > 0)
        {
            txtadminno.Text = drpstudents.SelectedValue;
            lblReport.Text = "";
            btnprint.Visible = false;
        }
        else
        {
            txtadminno.Text = "0";
            lblReport.Text = "";
            btnprint.Visible = false;
        }
    }

    private void FillStudent()
    {
        try
        {
            lblReport.Text = "";
            btnprint.Visible = false;
            Common common = new Common();
            if (Convert.ToInt32(common.ExecuteScalarQry("select count(*) from PS_StudMaster where AdmnNo=" + txtadminno.Text)) == 0)
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Admission no. does not exists')", true);
            }
            else
            {
                DataTable dataTable = common.ExecuteSql("select SessionYear,section,ClassID from PS_ClasswiseStudent where Detained_Promoted='' and  admnno=" + txtadminno.Text);
                drpSession.SelectedValue = dataTable.Rows[0]["SessionYear"].ToString();
                fillclass();
                drpclass.SelectedValue = dataTable.Rows[0]["ClassID"].ToString();
                FillClassSection();
                drpSection.SelectedValue = dataTable.Rows[0]["section"].ToString();
                fillstudent();
                drpstudents.SelectedValue = txtadminno.Text;
            }
        }
        catch (Exception ex)
        {
            if (!(ex.Message != ""))
                return;
            ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Invalid Data')", true);
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        if (drpstudents.SelectedValue != txtadminno.Text.Trim())
            FillStudent();
        GenerateFeePaid();
    }

    private void GenerateFeePaid()
    {
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal = new clsDAL();
        Decimal num1 = new Decimal(0);
        hashtable.Add("@SessionYear", drpSession.SelectedValue);
        hashtable.Add("@AdmnNo", drpstudents.SelectedValue.ToString());
        DataSet dataSet2 = Convert.ToInt32(drpSession.SelectedValue.ToString().Trim().Split('-')[0].Trim()) < 2014 ? clsDal.GetDataSet("ps_sp_get_MonthWisePmtDetails", hashtable) : clsDal.GetDataSet("ps_sp_get_StudFeeReport", hashtable);
        int num2 = 1;
        int num3 = 4;
        string str = Information();
        StringBuilder stringBuilder1 = new StringBuilder("");
        stringBuilder1.Append("<center>");
        stringBuilder1.Append(str);
        stringBuilder1.Append("<table border='0' cellpadding='0' cellspacing='0'  width='100%' class='cnt-box2 lbl2 tbltxt'>");
        stringBuilder1.Append("<tr>");
        stringBuilder1.Append("<td align='left' style='Font-Size:15px;width:30%;'><b>Admission No : </b>" + txtadminno.Text.ToString().Trim());
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("<td align='left' style='Font-Size:15px;width:30%;'><b>Name  :</b>" + dataSet2.Tables[0].Rows[0]["FullName"].ToString().Trim());
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("<td align='left' style='Font-Size:15px;'><b>Class : </b>" + dataSet2.Tables[0].Rows[0]["ClassName"].ToString() + "</td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("<tr style='text-align: left;'>");
        stringBuilder1.Append("<td colspan='3' align='left' style='Font-Size:15px;'><b>Father's Name :   </b>" + dataSet2.Tables[0].Rows[0]["FatherName"].ToString() + "</td>");
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("</table>");
        stringBuilder1.Append("<table border='1' cellpadding='2' cellspacing='0'  width='100%'>");
        stringBuilder1.Append("<tr style='background-color: #DFDFDF;' class='gridtxt'>");
        stringBuilder1.Append("<td style='width: 50px' align='left'><b>Month</b></td>");
        stringBuilder1.Append("<td style='width: 50px' align='center'><b>Tution Fee</b></td>");
        stringBuilder1.Append("<td style='width: 50px' align='center'><b>Other Fees</b></td>");
        stringBuilder1.Append("<td style='width: 50px' align='center'><b>Admission /Readmission Fee</b></td>");
        stringBuilder1.Append("<td style='width: 90px' align='center'><b>Bus Fees</b></td>");
        stringBuilder1.Append("<td style='width: 90px' align='center'><b>Books Fees</b></td>");
        stringBuilder1.Append("<td style='width: 80px' align='center'><b>Total</b></td>");
        stringBuilder1.Append("<td style='width: 80px' align='center'><b>Receipt No.</b></td>");
        stringBuilder1.Append("<td style='width: 110px' align='center'><b>Recvd Date</b></td>");
        stringBuilder1.Append("<td style='width: 140px' align='center'><b>Recvd By</b></td>");
        while (num2 <= 12)
        {
            DataRow[] dataRowArray1 = dataSet2.Tables[1].Select("feemonth=" + num3);
            if (dataRowArray1.Length > 0)
                stringBuilder1.Append("<tr class='largetb'><td valign='top' class='gridtext'>" + dataRowArray1[0]["MonthNm"].ToString() + "</td>");
            else
                stringBuilder1.Append("<tr class='largetb'><td valign='top' class='gridtext'>&nbsp;</td>");
            DataTable table = dataSet2.Tables[2];
            StringBuilder stringBuilder2 = new StringBuilder();
            StringBuilder stringBuilder3 = new StringBuilder();
            StringBuilder stringBuilder4 = new StringBuilder();
            StringBuilder stringBuilder5 = new StringBuilder();
            StringBuilder stringBuilder6 = new StringBuilder();
            Decimal num4 = new Decimal(0);
            Decimal num5 = new Decimal(0);
            foreach (DataRow dataRow in dataRowArray1)
            {
                Decimal num6 = Convert.ToDecimal(dataRow["Debit"].ToString().Trim());
                IEnumerator enumerator = table.Rows.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        DataRow drpaid = (DataRow)enumerator.Current;
                        if (num6 != new Decimal(0) && Convert.ToDecimal(drpaid["Credit"].ToString().Trim()) > new Decimal(0) && drpaid["FeeId"].ToString().Trim() == dataRow["FeeId"].ToString().Trim())
                        {
                            num6 -= Convert.ToDecimal(drpaid["Credit"].ToString().Trim());
                            if (drpaid["PeriodicityID"].ToString() == "3" || drpaid["PeriodicityID"].ToString() == "4" || drpaid["PeriodicityID"].ToString() == "5")
                            {
                                stringBuilder2.Append(drpaid["Credit"].ToString() + "<br/>");
                                num5 += Convert.ToDecimal(drpaid["Credit"].ToString().Trim());
                            }
                            else if (drpaid["PeriodicityID"].ToString() == "1" || drpaid["PeriodicityID"].ToString() == "2")
                            {
                                num4 += Convert.ToDecimal(drpaid["Credit"].ToString().Trim());
                            }
                            else
                            {
                                stringBuilder3.Append(drpaid["Credit"].ToString() + "<br/>");
                                num5 += Convert.ToDecimal(drpaid["Credit"].ToString().Trim());
                            }
                            if (!Array.Exists<string>(stringBuilder4.ToString().Trim().Split(new string[1]
              {
                "<br/>"
              }, StringSplitOptions.RemoveEmptyEntries), (Predicate<string>)(sa => sa == drpaid["Receipt_VrNo"].ToString().Trim())))
                            {
                                stringBuilder4.Append(drpaid["Receipt_VrNo"].ToString() + "<br/>");
                                stringBuilder5.Append(drpaid["TransDt"].ToString() + "<br/>");
                                stringBuilder6.Append(drpaid["USER_NAME"].ToString() + "<br/>");
                            }
                            drpaid["Credit"] = 0;
                        }
                        table.AcceptChanges();
                    }
                }
                finally
                {
                    IDisposable disposable = enumerator as IDisposable;
                    if (disposable != null)
                        disposable.Dispose();
                }
            }
            if (num4 > new Decimal(0))
            {
                if (stringBuilder2.ToString().Trim() != "")
                    stringBuilder1.Append("<td valign='top' class='gridtext'><br/>" + stringBuilder2.ToString() + "</td>");
                else
                    stringBuilder1.Append("<td valign='top' class='gridtext'>&nbsp;</td>");
                num5 += num4;
            }
            else if (stringBuilder2.ToString().Trim() != "")
                stringBuilder1.Append("<td valign='top' class='gridtext'>" + stringBuilder2.ToString() + "</td>");
            else
                stringBuilder1.Append("<td valign='top' class='gridtext'>&nbsp;</td>");
            if (stringBuilder3.ToString().Trim() != "")
                stringBuilder1.Append("<td valign='top' class='gridtext'>" + stringBuilder3.ToString() + "</td>");
            else
                stringBuilder1.Append("<td valign='top' class='gridtext'>&nbsp;</td>");
            if (num4 > new Decimal(0))
                stringBuilder1.Append("<td valign='top' class='gridtext'>" + num4.ToString("0.00") + "</td>");
            else
                stringBuilder1.Append("<td valign='top' class='gridtext'>&nbsp;</td>");
            DataRow[] drbus = dataSet2.Tables[3].Select("AdMonth=" + num3);
            if (drbus.Length > 0)
            {
                if (!Array.Exists<string>(stringBuilder4.ToString().Trim().Split(new string[1]
        {
          "<br/>"
        }, StringSplitOptions.RemoveEmptyEntries), (Predicate<string>)(sa => sa == drbus[0]["Receipt_No"].ToString().Trim())))
                {
                    stringBuilder4.Append(drbus[0]["Receipt_No"].ToString() + "<br/>");
                    stringBuilder5.Append(drbus[0]["TransDt"].ToString() + "<br/>");
                    stringBuilder6.Append(drbus[0]["USER_NAME"].ToString() + "<br/>");
                }
                if (num4 > new Decimal(0))
                    stringBuilder1.Append("<td valign='top' class='gridtext'><br/>" + drbus[0]["BusFee"].ToString() + "</td>");
                else
                    stringBuilder1.Append("<td valign='top' class='gridtext'>" + drbus[0]["BusFee"].ToString() + "</td>");
                num5 += Convert.ToDecimal(drbus[0]["BusFee"].ToString().Trim());
            }
            else
                stringBuilder1.Append("<td valign='top' class='gridtext'>&nbsp;</td>");
            DataRow[] dataRowArray2 = dataSet2.Tables[4].Select("AdMonth=" + num3);
            if (dataRowArray2.Length > 0)
            {
                Decimal num6 = new Decimal(0);
                foreach (DataRow dataRow in dataRowArray2)
                {
                    DataRow drBooks = dataRow;
                    if (!Array.Exists<string>(stringBuilder4.ToString().Trim().Split(new string[1]
          {
            "<br/>"
          }, StringSplitOptions.RemoveEmptyEntries), (Predicate<string>)(sa => sa == drBooks["Receipt_VrNo"].ToString().Trim())))
                    {
                        stringBuilder4.Append(drBooks["Receipt_VrNo"].ToString() + "<br/>");
                        stringBuilder5.Append(drBooks["TransDt"].ToString() + "<br/>");
                        stringBuilder6.Append(drBooks["USER_NAME"].ToString() + "<br/>");
                    }
                    num6 += Convert.ToDecimal(drBooks["BooksFee"].ToString().Trim());
                    num5 += Convert.ToDecimal(drBooks["BooksFee"].ToString().Trim());
                }
                stringBuilder1.Append("<td valign='top' class='gridtext'><br/>" + num6 + "</td>");
            }
            else
                stringBuilder1.Append("<td valign='top' class='gridtext'>&nbsp;</td>");
            if (num5 > new Decimal(0))
                stringBuilder1.Append("<td valign='top' class='gridtext'>" + num5.ToString("0.00") + "</td>");
            else
                stringBuilder1.Append("<td valign='top' class='gridtext'>&nbsp;</td>");
            if (stringBuilder4.ToString().Trim() != "")
                stringBuilder1.Append("<td valign='top' class='gridtext'>" + stringBuilder4.ToString() + "</td>");
            else
                stringBuilder1.Append("<td valign='top' class='gridtext'>&nbsp;</td>");
            if (stringBuilder5.ToString().Trim() != "")
                stringBuilder1.Append("<td valign='top' class='gridtext'>" + stringBuilder5.ToString() + "</td>");
            else
                stringBuilder1.Append("<td valign='top' class='gridtext'>&nbsp;</td>");
            if (stringBuilder6.ToString().Trim() != "")
                stringBuilder1.Append("<td valign='top' class='gridtext'>" + stringBuilder6.ToString() + "</td>");
            else
                stringBuilder1.Append("<td valign='top' class='gridtext'>&nbsp;</td>");
            stringBuilder1.Append("</tr>");
            if (num3 == 12)
                num3 = 0;
            ++num3;
            ++num2;
            num1 += num5;
        }
        stringBuilder1.Append("</table>");
        stringBuilder1.Append("<br/>");
        stringBuilder1.Append("<div style='text-align:left;padding-left:20px;color:Red;'><b>Total Amount Paid: " + num1.ToString("0.00") + "&nbsp;(In Words : Rupees" + NumberToText(num1.ToString()) + ")</b></div>");
        lblReport.Text = stringBuilder1.ToString().Trim();
        Session["MonthlyPayment"] = stringBuilder1.ToString().Trim();
        btnprint.Visible = true;
        Session["AdmnNo"] = txtadminno.Text.Trim();
        Session["Session"] = drpSession.SelectedItem.Text.Trim();
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