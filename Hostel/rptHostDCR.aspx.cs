using AjaxControlToolkit;
using ASP;
using Classes.DA;
using RJS.Web.WebControl;
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


public partial class Hostel_rptHostDCR : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        fillclass();
        FillClassSection();
        FillCollectCounter();
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
    }

    private void FillCollectCounter()
    {
        drpFeeCounter.DataSource = new Common().GetDataTable("ps_sp_get_FeeWorkStation");
        drpFeeCounter.DataTextField = "FeeCollector";
        drpFeeCounter.DataValueField = "USER_ID";
        drpFeeCounter.DataBind();
        drpFeeCounter.Items.Insert(0, new ListItem("All", "0"));
    }

    private void fillclass()
    {
        DataTable dataTable = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassID";
        drpClass.DataSource = dataTable;
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("All", "0"));
    }

    private void FillClassSection()
    {
        drpSection.Items.Clear();
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassWiseSections", new Hashtable()
    {
      {
         "@classId",
         drpClass.SelectedValue.ToString().Trim()
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("All", "0"));
    }

    private void fillstudent()
    {
        drpstudents.Items.Clear();
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        DataTable dataTable = common.GetDataTable("HostRptStudNamesForFeeColRpt", hashtable);
        drpstudents.DataSource = dataTable;
        drpstudents.DataTextField = "fullname";
        drpstudents.DataValueField = "admnno";
        drpstudents.DataBind();
        if (dataTable.Rows.Count > 0)
            drpstudents.Items.Insert(0, new ListItem("--ALL--", "0"));
        else
            drpstudents.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (!ValidAdmn())
            return;
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpFeeCounter.SelectedIndex > 0)
            hashtable.Add("@UserID", drpFeeCounter.SelectedValue.ToString());
        if (txtFromDate.Text.Trim() != "")
            hashtable.Add("@FrmDt", dtpFromDate.GetDateValue().ToString("MM/dd/yyyy"));
        else
            hashtable.Add("@FrmDt", get_FistdayOfCurrSession());
        if (txtToDate.Text.Trim() != "")
            hashtable.Add("@ToDt", dtpToDate.GetDateValue().ToString("MM/dd/yyyy"));
        else
            hashtable.Add("@ToDt", DateTime.Now.ToString("MM/dd/yyyy"));
        if (drpPmtMode.SelectedIndex > 0)
            hashtable.Add("@PaymentMode", drpPmtMode.SelectedValue.ToString());
        if (drpClass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpClass.SelectedValue.ToString());
        if (drpstudents.SelectedIndex > 0)
            hashtable.Add("@AdmnNo", drpstudents.SelectedValue.ToString());
        if (txtFromDate.Text.Trim() == "" && txtToDate.Text.Trim() == "")
            hashtable.Add("@Session", get_CurrentSessionYr());
        get_CurrentSessionYr().Trim().Split('-');
        DataTable dataTable2 = common.GetDataTable("HostRptFeeReceivedALLNew", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            GenerateReport(dataTable2);
            btnPrint.Enabled = true;
            btnExpExcel.Enabled = true;
        }
        else
        {
            lblReport.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='center'>No Records Found  !</div>";
            btnPrint.Enabled = false;
            btnExpExcel.Enabled = false;
        }
    }

    public string get_CurrentSessionYr()
    {
        DateTime today = DateTime.Today;
        int month = today.Month;
        int year = today.Year;
        return month <= 3 ? Convert.ToString(year - 1) + "-" + year.ToString().Substring(2, 2) : year.ToString() + "-" + Convert.ToString(year + 1).Substring(2, 2);
    }

    public DateTime get_FistdayOfCurrSession()
    {
        DateTime today = DateTime.Today;
        int month = today.Month;
        int year = today.Year;
        DateTime dateTime = new DateTime();
        dateTime = month <= 3 ? new DateTime(DateTime.Today.Year - 1, 4, 1) : new DateTime(DateTime.Today.Year, 4, 1);
        return dateTime;
    }

    private void GenerateReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
        stringBuilder.Append("<tr  style='background-color:#CCCCCC'>");
        stringBuilder.Append("<td style='width: 30px' align='left'><strong>Sl.No</strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='left'>Received Date</td>");
        stringBuilder.Append("<td style='width: 60px' align='left'>Receipt No</td>");
        stringBuilder.Append("<td style='width: 80px' align='left'>Admission No</td>");
        stringBuilder.Append("<td style='width: 150px' align='left'>Name</td>");
        stringBuilder.Append("<td style='width: 60px' align='left'>Class</td>");
        stringBuilder.Append("<td align='left'>Description</td>");
        stringBuilder.Append("<td style='width: 100px' align='right'>Amount</td>");
        stringBuilder.Append("</tr>");
        double num1 = 0.0;
        int num2 = 1;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center'>" + num2.ToString() + "</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["RecvDatestr"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["ReceiptNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["AdmnNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["ClassName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Particulars"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right'>");
            stringBuilder.Append(row["Amount"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            num1 += Convert.ToDouble(row["Amount"].ToString().Trim());
            ++num2;
        }
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='7'  align='right'>");
        stringBuilder.Append("<strong class='error'>Grand Total &nbsp;:&nbsp;</strong>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='right'>");
        stringBuilder.Append("<strong class='error'>" + string.Format("{0:F2}", num1));
        stringBuilder.Append("</strong></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["DCR"] = stringBuilder.ToString().Trim();
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td align='left' colspan='14'><b>");
                stringBuilder.Append("Fee Collection Report From : ");
                if (txtFromDate.Text.Trim() != "")
                {
                    stringBuilder.Append(dtpFromDate.GetDateValue().ToString("d MMM yyyy"));
                }
                else
                {
                    DateTime fistdayOfCurrSession = get_FistdayOfCurrSession();
                    stringBuilder.Append(fistdayOfCurrSession.ToString("d MMM yyyy"));
                }
                stringBuilder.Append("  - To : ");
                if (txtToDate.Text.Trim() != "")
                    stringBuilder.Append(dtpToDate.GetDateValue().ToString("d MMM yyyy"));
                else
                    stringBuilder.Append(DateTime.Now.ToString("d MMM yyyy"));
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(lblReport.Text.ToString().Trim());
                stringBuilder.Append("<table><tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel(stringBuilder.ToString());
            }
            else
                Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/HostelDCR" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + str);
            Response.WriteFile(str);
            Response.End();
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
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptHostDCRPrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpSection_SelectedIndexChanged1(object sender, EventArgs e)
    {
        lblReport.Text = "";
        fillstudent();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpClass.SelectedIndex > 0)
        {
            lblReport.Text = "";
            drpSection.SelectedIndex = -1;
            drpstudents.SelectedIndex = -1;
            txtadminno.Text = "";
            FillClassSection();
            fillstudent();
        }
        else
            lblReport.Text = "";
    }

    protected void drpFeeCounter_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = "";
    }

    protected void drpstudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpstudents.SelectedIndex > 0)
        {
            lblReport.Text = "";
            txtadminno.Text = drpstudents.SelectedValue;
        }
        else
        {
            txtadminno.Text = "";
            lblReport.Text = "";
        }
    }

    protected bool ValidAdmn()
    {
        if (!(txtadminno.Text.Trim() != ""))
            return true;
        try
        {
            btnPrint.Enabled = false;
            Common common = new Common();
            if (Convert.ToInt32(common.ExecuteScalarQry("select count(*) from PS_StudMaster where AdmnNo=" + txtadminno.Text)) == 0)
            {
                lblReport.Text = "";
                drpSection.SelectedIndex = -1;
                drpstudents.SelectedIndex = -1;
                ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Admission no. does not exists')", true);
                return false;
            }
            fillclass();
            fillstudent();
            drpstudents.SelectedValue = txtadminno.Text;
            DataTable dataTable = common.ExecuteSql("select section,ClassID from PS_ClasswiseStudent where admnno=" + txtadminno.Text + " and Detained_Promoted='' ");
            fillclass();
            drpClass.SelectedValue = dataTable.Rows[0]["ClassID"].ToString();
            FillClassSection();
            drpSection.SelectedValue = dataTable.Rows[0]["section"].ToString();
            return true;
        }
        catch (Exception ex)
        {
            if (!(ex.Message != ""))
                return false;
            ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Invalid Data')", true);
            return false;
        }
    }
}