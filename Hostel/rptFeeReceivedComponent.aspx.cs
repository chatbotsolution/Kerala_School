using ASP;
using Classes.DA;
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
using System.Web.UI.WebControls;

public partial class Hostel_rptFeeReceivedComponent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SSVMID"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        btnPrint.Visible = false;
        pcalFromDate.SetDateValue(Convert.ToDateTime("1 Apr" + new clsGenerateFee().CreateCurrSession().Trim().Substring(0, 4)));
        pcalToDate.SetDateValue(DateTime.Now);
        SetDate();
    }

    private void SetDate()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = clsDal.GetDataTableQry("select top(1) FY,StartDate,EndDate from dbo.ACTS_FinancialYear where (IsFinalized is null or IsFinalized=0) order by FY_Id DESC");
        if (dataTableQry.Rows.Count > 0)
        {
            pcalFromDate.SetDateValue(Convert.ToDateTime(dataTableQry.Rows[0]["StartDate"].ToString().Trim()));
            pcalToDate.SetDateValue(Convert.ToDateTime(dataTableQry.Rows[0]["EndDate"].ToString().Trim()));
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Call my function", "alert('Please Set the Financial year!');window.location.href='../accounts/OpenFy.aspx'", true);
    }

    private void clearAll()
    {
        lblPrevDue.Text = "";
        lblReport.Text = "";
        lblTotal.Text = "";
        lblGTotal.Text = "";
        lblAdv.Text = "";
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        clearAll();
        StringBuilder stringBuilder = new StringBuilder();
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        string str1 = common.ExecuteScalarQry("select dbo.fuGetSessionYr('" + pcalFromDate.GetDateValue().ToString("dd MMM yyyy") + "') as FY");
        ViewState["FY"] = (object)str1;
        DateTime dateTime1 = Convert.ToDateTime("1 Apr" + str1.Substring(0, 4));
        DateTime dateTime2 = Convert.ToDateTime("31 Mar" + Convert.ToString(Convert.ToInt32(str1.Substring(0, 4)) + 1));
        if (pcalToDate.GetDateValue() > dateTime2 || pcalToDate.GetDateValue() < dateTime1)
            pcalToDate.SetDateValue(dateTime2);
        string[] strArray = str1.Split('-');
        string str2 = Convert.ToString(Convert.ToInt32(strArray[0].Trim()) - 1) + "-" + Convert.ToString(Convert.ToInt32(strArray[1].Trim()) - 1);
        Convert.ToDateTime("1 Apr" + str2.Substring(0, 4));
        string str3 = Convert.ToString(Convert.ToInt32(strArray[0].Trim()) + 1) + "-" + Convert.ToString(Convert.ToInt32(strArray[1].Trim()) + 1);
        if (drpModeOfPayment.Text != "All")
            hashtable.Add((object)"@ModeOfPayment", (object)drpModeOfPayment.SelectedValue.ToString());
        if (txtFromDate.Text.Trim() != "")
            hashtable.Add((object)"@FrmDt", (object)pcalFromDate.GetDateValue().ToString("dd MMM yyyy"));
        else
            hashtable.Add((object)"@FrmDt", (object)dateTime1.ToString("dd MMM yyyy"));
        if (txtToDate.Text.Trim() != "")
            hashtable.Add((object)"@ToDt", (object)pcalToDate.GetDateValue().ToString("dd MMM yyyy"));
        else if (Convert.ToInt32(strArray[0]) < 2014)
        {
            DateTime dateTime3 = Convert.ToDateTime("31 Mar" + str1.Substring(0, 4)).AddYears(1);
            hashtable.Add((object)"@ToDt", (object)dateTime3.ToString("dd MMM yyyy"));
        }
        hashtable.Add((object)"@SessionYr", (object)str1.ToString().Trim());
        hashtable.Add((object)"@SchoolID", (object)Convert.ToInt32(Session["SchoolId"]));
        hashtable.Add((object)"@NxtSY", (object)str3);
        DataSet dataSet1 = new DataSet();
        hashtable.Add((object)"@PrevSnY", (object)str2.Trim());
        DataSet dataSet2 = common.GetDataSet("HostRptGetCompWiseFeeNew", hashtable);
        if (dataSet2.Tables[0].Rows.Count + dataSet2.Tables[1].Rows.Count + dataSet2.Tables[2].Rows.Count > 0)
        {
            CreateReport(dataSet2);
            btnPrint.Visible = true;
        }
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
    }

    protected void CreateReport(DataSet ds)
    {
        double num1 = 0.0;
        string str1 = Information();
        StringBuilder stringBuilder1 = new StringBuilder();
        stringBuilder1.Append(str1);
        StringBuilder stringBuilder2 = new StringBuilder("");
        StringBuilder stringBuilder3 = new StringBuilder("");
        StringBuilder stringBuilder4 = new StringBuilder("");
        StringBuilder stringBuilder5 = new StringBuilder("");
        StringBuilder stringBuilder6 = new StringBuilder("");
        StringBuilder stringBuilder7 = new StringBuilder();
        double num2 = 0.0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            stringBuilder2.Append("<table cellpadding='1' cellspacing='1'  width='700px' style='background-color:#CCC;' >");
            stringBuilder2.Append("<tr>");
            if (Convert.ToInt32(ViewState["FY"].ToString().Substring(0, 4)) < 2014)
            {
                stringBuilder2.Append("<td colspan='2' align='center'><strong>Componentwise Collection Report For Session </strong>" + ViewState["FY"].ToString().ToString() + "</td>");
            }
            else
            {
                stringBuilder2.Append("<td colspan='2' align='center'><strong>Componentwise Collection Report</strong> ( From " + pcalFromDate.GetDateValue().ToString("dd MMM yyyy"));
                stringBuilder2.Append(" To " + pcalToDate.GetDateValue().ToString("dd MMM yyyy") + " )</td>");
            }
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td style='text-align:left;' class='tblheader'>Fee Components</td>");
            stringBuilder2.Append("<td style='width: 120px' align='right' class='tblheader'>Amount</td>");
            stringBuilder2.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[0].Rows)
            {
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td align='left' class='tbltd'>");
                stringBuilder2.Append(row["TransDesc"].ToString().Trim());
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='right' class='tbltd'>");
                stringBuilder2.Append(row["credit"].ToString().Trim());
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("</tr>");
                num2 += Convert.ToDouble(row["credit"].ToString().Trim());
            }
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td colspan='2' align='right' class='tbltd'>");
            stringBuilder2.Append("<strong>Total &nbsp;:&nbsp; &nbsp;&nbsp;" + string.Format("{0:F2}", (object)num2));
            stringBuilder2.Append("</td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("</table>");
            lblReport.Text = stringBuilder2.ToString().Trim();
        }
        string[] strArray = ViewState["FY"].ToString().Trim().Split('-');
        if (ds.Tables[1].Rows.Count > 0)
        {
            stringBuilder5.Append("<table cellpadding='1' cellspacing='1'  width='700px' style='background-color:#CCC;' >");
            foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[4].Rows)
                num1 += Convert.ToDouble(row["credit"].ToString().Trim());
            stringBuilder5.Append("<tr>");
            stringBuilder5.Append("<td align='left' class='tbltd'><b>Prev School Fee Collection</b></td>");
            stringBuilder5.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            stringBuilder5.Append(num1.ToString().Trim());
            stringBuilder5.Append("</b></td>");
            stringBuilder5.Append("</tr>");
            stringBuilder5.Append("</table>");
            lblPrev.Text = stringBuilder5.ToString().Trim();
        }
        if (ds.Tables[2].Rows.Count > 0)
        {
            stringBuilder7.Append("<table cellpadding='1' cellspacing='1'  width='700px' style='background-color:#CCC;' >");
            stringBuilder7.Append("<tr>");
            stringBuilder7.Append("<td align='left' class='tbltd'><b>");
            string str2 = Convert.ToString(Convert.ToInt32(strArray[0].Trim()) + 1) + "-" + Convert.ToString(Convert.ToInt32(strArray[1].Trim()) + 1);
            stringBuilder7.Append("Advance Hostel Fee Collcetion For Session " + str2);
            stringBuilder7.Append("</b></td>");
            stringBuilder7.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            stringBuilder7.Append(ds.Tables[2].Rows[0]["credit"].ToString().Trim());
            stringBuilder7.Append("</b></td>");
            stringBuilder7.Append("</tr>");
            stringBuilder7.Append("</table>");
        }
        lblAdv.Text = stringBuilder7.ToString().Trim();
        double num3 = num2 + Convert.ToDouble(ds.Tables[2].Rows[0][0].ToString()) + num1;
        stringBuilder6.Append("<table cellpadding='1' cellspacing='1'  width='700px' style='background-color:#CCC;' >");
        stringBuilder6.Append("<tr>");
        stringBuilder6.Append("<td align='right' class='tbltd'><b>");
        stringBuilder6.Append("<strong>Total &nbsp;:&nbsp; &nbsp;&nbsp;" + string.Format("{0:F2}", (object)num3));
        stringBuilder6.Append("</b></td>");
        stringBuilder6.Append("</tr>");
        stringBuilder6.Append("</table>");
        StringBuilder stringBuilder8 = new StringBuilder();
        if (Convert.ToDouble(num3) > 0.0)
        {
            lblTotal.Text = stringBuilder6.ToString().Trim();
            Session["printData"] = (object)(stringBuilder1.ToString().Trim() + stringBuilder2.ToString().Trim() + stringBuilder5.ToString().Trim() + stringBuilder7.ToString().Trim() + stringBuilder6.ToString().Trim() + stringBuilder8.ToString().Trim());
        }
        else
        {
            lblPrevDue.Text = string.Empty;
            lblReport.Text = string.Empty;
            lblTotal.Text = string.Empty;
            lblAdv.Text = string.Empty;
            lblPrev.Text = string.Empty;
            ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
        }
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
                    stringBuilder.Append("<table width='700px' border='0px'><tr>");
                    stringBuilder.Append("<td align='center' style='font-size:20px;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2) + "</strong></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2) + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2) + "</b></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2) + ",Phone-" + clsDal.Decrypt(row["Phone"].ToString().Trim(), str2) + "</b>");
                    stringBuilder.Append("</td></tr>");
                    stringBuilder.Append("</table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptHostFeeReceivedComponentPrint.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    protected void drpModeOfPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = "";
    }

    protected void btnOutstandingFee_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptHostFeeToBeReceivedComponent.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}