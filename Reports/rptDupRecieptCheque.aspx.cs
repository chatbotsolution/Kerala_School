using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptDupRecieptCheque : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        txtadminno.Text = "0";
        fillsession();
        fillclass();
        fillstudent();
        FillClassSection();
        lbldetail.Text = "";
        lblReport.Text = "";
        btnPrint.Visible = false;
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
        drpClasses.Items.Clear();
        drpClasses.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpClasses.DataTextField = "classname";
        drpClasses.DataValueField = "classid";
        drpClasses.DataBind();
        drpClasses.Items.Insert(0, new ListItem("All", "0"));
    }

    private void FillClassSection()
    {
        drpSection.Items.Clear();
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "@classId",
         drpClasses.SelectedValue.ToString().Trim()
      },
      {
         "@session",
         drpSession.SelectedValue.ToString().Trim()
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("All", "0"));
    }

    private void fillstudent()
    {
        Common common = new Common();
        DataTable dataTable;
        if (drpSection.SelectedIndex != 0)
            dataTable = common.ExecuteSql("select * from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.classid=" + drpClasses.SelectedValue + " and  cs.Section='" + drpSection.SelectedValue + "' order by s.fullname");
        else
            dataTable = drpClasses.SelectedIndex == 0 ? common.ExecuteSql("select * from  ps_studmaster order by fullname") : common.ExecuteSql("select * from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.classid=" + drpClasses.SelectedValue + " order by fullname");
        drpstudent.DataSource = dataTable;
        drpstudent.DataTextField = "fullname";
        drpstudent.DataValueField = "admnno";
        drpstudent.DataBind();
        drpstudent.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void fillfeereciept()
    {
        drpReciept.DataSource = new Common().ExecuteSql("select distinct ReceiptNo from PS_ChequeDetails where AdmnNo=" + drpstudent.SelectedValue);
        drpReciept.DataTextField = "ReceiptNo";
        drpReciept.DataValueField = "ReceiptNo";
        drpReciept.DataBind();
        drpReciept.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void filladditionfeereciept()
    {
        drpReciept.DataSource = new Common().ExecuteSql("select a.Receipt_No,a.AdFeeNo from PS_AdFeeLedger a, PS_ClasswiseStudent c where c.id=a.ClassWiseId and c.admnno=" + drpstudent.SelectedValue);
        drpReciept.DataTextField = "Receipt_No";
        drpReciept.DataValueField = "AdFeeNo";
        drpReciept.DataBind();
        drpReciept.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
        lblReport.Text = "";
        lbldetail.Text = "";
        btnPrint.Visible = false;
    }

    protected void drpClasses_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
        fillstudent();
        lblReport.Text = "";
        lbldetail.Text = "";
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = "";
        lbldetail.Text = "";
        fillstudent();
        btnPrint.Visible = false;
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtadminno.Text = drpstudent.SelectedValue;
        lblReport.Text = "";
        lbldetail.Text = "";
        btnPrint.Visible = false;
        fillfeereciept();
        fillclass();
        DataTable dataTable = new Common().ExecuteSql("select section,ClassID from PS_ClasswiseStudent where admnno=" + drpstudent.SelectedValue);
        fillclass();
        drpClasses.SelectedValue = dataTable.Rows[0]["ClassID"].ToString();
        FillClassSection();
        drpSection.SelectedValue = dataTable.Rows[0]["section"].ToString();
    }

    protected void drpfeetype_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = "";
        lbldetail.Text = "";
        btnPrint.Visible = false;
        if (Convert.ToInt32(drpfeetype.SelectedValue.ToString()) == 0)
            fillfeereciept();
        else
            filladditionfeereciept();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Common common = new Common();
        DataTable dataTable = new DataTable();
        if (Convert.ToInt32(drpfeetype.SelectedValue.ToString()) != 0)
            createRemarkReport(common.ExecuteSql("Select ad_Description,Convert(Decimal(18,2),isnull(balance,0)) as Balance, convert(varchar,AdFeeDate,103) as date  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId inner join ps_AdditionalFeeMaster on ps_AdditionalFeeMaster.Ad_id = PS_AdFeeLedger.ad_id where  balance > 0 and Receipt_No='" + drpReciept.SelectedItem.ToString() + "'"));
        else
            getdata();
    }

    private void createRemarkReport(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            btnPrint.Visible = true;
            double num = 0.0;
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table cellpadding='0' cellspacing='0' border='1' width='60%' >");
            stringBuilder.Append("<tr style='text-align:left;height:25px'><td style='width: 25%;'><strong>Due Date</strong></td><td style='width: 50%;'><strong>Additional Charges</strong></td><td style='width: 25%;'><strong>Balance Amount</strong></td></tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr style='text-align:left;height:20px;'><td>");
                stringBuilder.Append(row["date"].ToString().Trim());
                stringBuilder.Append("</td><td>");
                stringBuilder.Append(row["ad_Description"].ToString().Trim());
                stringBuilder.Append("</td><td style='text-align:right;'>");
                stringBuilder.Append(row["Balance"].ToString().Trim());
                stringBuilder.Append("</td></tr>");
                num += Convert.ToDouble(row["Balance"].ToString().Trim());
            }
            stringBuilder.Append("<tr><td style='text-align:right;' colspan='2'><strong>Total Amount</strong></td><td style='text-align:right;'><strong>" + num.ToString().Trim() + ".00</strong></td></tr>");
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["PrintAdditionalReciept"] = stringBuilder.ToString().Trim();
        }
        else
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("<table cellpadding='0' cellspacing='0' border='0' width='60%'>");
            stringBuilder.Append("<tr><td style='color:Red;' style='text-align:center;'>No Additional Charges Exist</td></tr>");
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            btnPrint.Visible = false;
        }
    }

    private void getdata()
    {
        Common common = new Common();
        new clsGenerateFee().CreateCurrSession();
        DataTable dataTable1 = common.ExecuteSql("select distinct FullName,ClassName,Section,FatherName from dbo.PS_StudMaster sm inner join dbo.PS_ClasswiseStudent cs on sm.admnno=cs.AdmnNo inner join dbo.PS_ClassMaster cm on cm.ClassID=cs.ClassID where cs.SessionYear='" + drpSession.SelectedItem.ToString() + "' and sm.AdmnNo=" + drpstudent.SelectedValue);
        if (dataTable1.Rows.Count <= 0)
            return;
        btnPrint.Visible = true;
        DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
        DateTime dateTime1 = Convert.ToDateTime(common.ExecuteSql("select max(TransDate) as TransDate from dbo.PS_FeeLedger where AdmnNo=" + txtadminno.Text + " and debit > 0").Rows[0]["TransDate"]);
        DateTime dateTime2 = dateTime1.AddMonths(1);
        string monthName1 = dateTimeFormatInfo.GetMonthName(dateTime1.Month);
        string monthName2 = dateTimeFormatInfo.GetMonthName(dateTime2.Month);
        string str = monthName1 + "-" + monthName2;
        StringBuilder stringBuilder1 = new StringBuilder();
        stringBuilder1.Append("<table cellpadding='0' cellspacing='0' border='0' width='80%'>");
        stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
        stringBuilder1.Append("<tr><td colspan='4' style='height:15px'></td></tr>");
        stringBuilder1.Append("<tr>");
        stringBuilder1.Append("<td style='width:25%'><strong>Receipt No:</strong></td><td style='width:25%'>" + drpReciept.SelectedItem.ToString() + "</td>");
        stringBuilder1.Append("<td style='width:25%'><strong>Date:</strong></td><td style='width:25%'>" + DateTime.Now.ToString("dd-MM-yyyy") + "</td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
        Session.Remove("rcptFee");
        stringBuilder1.Append("<tr>");
        DataTable dataTable2 = common.ExecuteSql("select * from PS_FeeNormsNew");
        if (dataTable2.Rows.Count > 0)
        {
            if (dataTable2.Rows[0]["FeeCollPeriod"].ToString() == "Bi Monthly")
                stringBuilder1.Append("<td style='width:25%'><strong>For the month of:</strong></td><td style='width:25%'>" + str + "</td>");
            else
                stringBuilder1.Append("<td style='width:25%'><strong>For the month of:</strong></td><td style='width:25%'>" + monthName1 + "</td>");
            stringBuilder1.Append("<td style='width:25%'><strong>Admission No:</strong></td><td style='width:25%'>" + drpstudent.SelectedValue + "</td)>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:25%'><strong>Name of the Pupil:</strong></td><td style='width:25%'>" + dataTable1.Rows[0][0].ToString() + "</td>");
            stringBuilder1.Append("<td style='width:25%'><strong>Father Name:</strong></td><td style='width:25%'>" + dataTable1.Rows[0][3].ToString() + "</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("<tr>");
            stringBuilder1.Append("<td style='width:25%'><strong>Class:</strong></td><td style='width:25%'>" + dataTable1.Rows[0][1].ToString() + "</td>");
            stringBuilder1.Append("<td style='width:25%'><strong>Section:</strong></td><td style='width:25%'>" + dataTable1.Rows[0][2].ToString() + "</td>");
            stringBuilder1.Append("</tr>");
            stringBuilder1.Append("<tr><td colspan='4' style='height:5px'></td></tr>");
            stringBuilder1.Append("</table>");
            DataTable dataTable3 = new DataTable();
            DataTable dataTable4 = common.ExecuteSql("select convert(decimal(18,2),sum(credit)),TransDesc,admnno from dbo.PS_FeeLedger fl join dbo.PS_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + drpstudent.SelectedValue + " and PeriodicityID<=2 and Receipt_VrNo='" + drpReciept.SelectedItem.ToString() + "' group by AdmnNo,TransDesc order by TransDesc");
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<table border=1 width=70% cellspacing=0 cellpadding=0><tr><td colspan=2 align=center>");
            stringBuilder2.Append("<strong>PARTICULARS</strong></td><td align=center><strong>AMOUNT</strong></td></tr>");
            stringBuilder2.Append("<tr><td colspan=2 style='border-bottom:0;'><strong>ADMISSION CHARGES :</strong>");
            stringBuilder2.Append("");
            stringBuilder2.Append("</td><td></td></tr>");
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
            DataTable dataTable6 = common.ExecuteSql("select convert(decimal(18,2),sum(credit)),TransDesc,admnno from dbo.PS_FeeLedger fl join dbo.PS_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + drpstudent.SelectedValue + " and PeriodicityID>2 and Receipt_VrNo='" + drpReciept.SelectedItem.ToString() + "' group by AdmnNo,TransDesc order by TransDesc");
            stringBuilder2.Append("<tr></tr><tr></tr><tr></tr><tr></tr>");
            stringBuilder2.Append("<tr><td colspan=2 style='border-bottom:0;border-top:0;'><strong>MONTHLY CHARGES :</strong>");
            stringBuilder2.Append("");
            stringBuilder2.Append("</td><td></td></tr>");
            for (int index = 0; index <= dataTable6.Rows.Count - 1; ++index)
            {
                stringBuilder2.Append("<tr><td style='border-right:0;border-bottom:0;border-top:0;'>&nbsp;&nbsp;&nbsp;&nbsp;");
                stringBuilder2.Append(dataTable6.Rows[index][1].ToString());
                stringBuilder2.Append("</td><td align='center' style='border-left:0;border-bottom:0;border-top:0;'>");
                stringBuilder2.Append("..........");
                stringBuilder2.Append("</td><td align='right' style='border-bottom:0;border-top:0;'>");
                stringBuilder2.Append(dataTable6.Rows[index][0].ToString());
                stringBuilder2.Append("</td></tr>");
            }
            DataTable dataTable7 = new DataTable();
            DataTable dataTable8 = common.ExecuteSql("select sum(credit) from dbo.PS_FeeLedger fl join dbo.PS_FeeComponents fs on fl.FeeId=fs.FeeID where admnno=" + drpstudent.SelectedValue + " and Receipt_VrNo='" + drpReciept.SelectedItem.ToString() + "' group by AdmnNo");
            stringBuilder2.Append("<tr><td colspan='3'></td></tr></br>");
            stringBuilder2.Append("<tr><td colspan='2' align='right'><strong>TOTAL</strong></TD>");
            stringBuilder2.Append("<td align='right'>");
            double num1 = 0.0;
            if (dataTable8.Rows.Count > 0)
            {
                num1 = Convert.ToDouble(dataTable8.Rows[0][0].ToString());
                dataTable8.Rows[0][0].ToString();
            }
            stringBuilder2.Append(string.Format("{0:F2}", num1));
            stringBuilder2.Append("</td></tr>");
            stringBuilder2.Append("<tr><td colspan='2' align='right' style='border-bottom:0;'><strong>Fine Amount:</strong></td><td align='right'>");
            if (Session[""] != null)
                stringBuilder2.Append(Session["PaidFine"]);
            else
                stringBuilder2.Append("0.00");
            stringBuilder2.Append("</td></tr>");
            stringBuilder2.Append("<tr><td colspan='2' align='right' style='border-bottom:0;'><strong>Amount in Credit:</strong></td><td align='right'>");
            if (Session[""] != null)
                stringBuilder2.Append(Session["CreditAmount"]);
            else
                stringBuilder2.Append("0.00");
            stringBuilder2.Append("</td></tr>");
            double num2 = num1;
            if (Session["CreditAmount"] != null)
                num2 += Convert.ToDouble(Session["CreditAmount"].ToString());
            if (Session["PaidFine"] != null)
                num2 += Convert.ToDouble(Session["PaidFine"].ToString());
            stringBuilder2.Append("<tr><td colspan='2' align='right' style='border-bottom:0;'><strong>Net Amount Recieved:</strong></td><td align='right'>");
            stringBuilder2.Append(string.Format("{0:F2}", num2));
            stringBuilder2.Append("</td></tr><table>");
            lbldetail.Text = stringBuilder1.ToString() + stringBuilder2.ToString();
            Session["PrintReciept"] = (stringBuilder1.ToString() + stringBuilder2.ToString());
        }
        else
        {
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("<table cellpadding='0' cellspacing='0' border='0' width='60%'>");
            stringBuilder2.Append("<tr><td style='color:Red;' style='text-align:center;'>No Additional Charges Exist</td></tr>");
            stringBuilder2.Append("</table>");
            lblReport.Text = stringBuilder2.ToString().Trim();
            btnPrint.Visible = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (Convert.ToInt32(drpfeetype.SelectedValue.ToString()) == 0)
            Response.Redirect("rptRecieptCashPrint.aspx");
        else
            Response.Redirect("rptRecieptCashAdditionPrint.aspx");
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
    }

    protected void txtadminno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            lblReport.Text = "";
            lbldetail.Text = "";
            btnPrint.Visible = false;
            Common common = new Common();
            if (Convert.ToInt32(common.ExecuteScalarQry("select count(*) from PS_StudMaster where AdmnNo=" + txtadminno.Text)) == 0)
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Admission no. does not exists')", true);
            }
            else
            {
                fillclass();
                fillstudent();
                drpstudent.SelectedValue = txtadminno.Text;
                DataTable dataTable = common.ExecuteSql("select section,ClassID from PS_ClasswiseStudent where admnno=" + txtadminno.Text);
                fillclass();
                drpClasses.SelectedValue = dataTable.Rows[0]["ClassID"].ToString();
                FillClassSection();
                drpSection.SelectedValue = dataTable.Rows[0]["section"].ToString();
                fillfeereciept();
            }
        }
        catch (Exception ex)
        {
            if (!(ex.Message != ""))
                return;
            ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Invalid Data')", true);
        }
    }
}