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
using System.Web.UI.WebControls;

public partial class Reports_rptFeeReceivedAll : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        fillclass();
        FillClassSection();
        FillCollectCounter();
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
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

    private void FillCollectCounter()
    {
        drpFeeCounter.DataSource = new Common().GetDataTable("ps_sp_get_FeeWorkStation");
        drpFeeCounter.DataTextField = "FeeCollector";
        drpFeeCounter.DataValueField = "USER_ID";
        drpFeeCounter.DataBind();
        drpFeeCounter.Items.Insert(0, new ListItem("All", "0"));
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpClass.SelectedIndex > 0)
        {
            lblReport.Text = "";
            drpSection.SelectedIndex = -1;
            drpstudents.SelectedIndex = -1;
            txtadminno.Text = "0";
            FillClassSection();
            fillstudent();
        }
        else
            lblReport.Text = "";
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpFeeCounter.SelectedIndex > 0)
            hashtable.Add("@UserID", drpFeeCounter.SelectedValue.ToString());
        if (txtFromDate.Text.Trim() != "")
            hashtable.Add("@fromdate", dtpFromDate.GetDateValue());
        if (txtToDate.Text.Trim() != "")
            hashtable.Add("@todate", dtpToDate.GetDateValue());
        if (drpPmtMode.SelectedIndex > 0)
            hashtable.Add("@PaymentMode", drpPmtMode.SelectedValue.ToString());
        if (drpClass.SelectedIndex > 0)
            hashtable.Add("@Class", drpClass.SelectedValue.ToString());
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue.ToString());
        if (drpstudents.SelectedIndex > 0)
            hashtable.Add("@AdmnNo", drpstudents.SelectedValue.ToString());
        DataTable dataTable2 = common.GetDataTable("ps_sp_get_FeeReceivedALL", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            GenerateReport(dataTable2);
            btnPrint.Enabled = true;
            btnExpExcel.Enabled = true;
        }
        else
        {
            lblReport.Text = "No Record Found  !";
            btnPrint.Enabled = false;
            btnExpExcel.Enabled = false;
        }
    }

    private void GenerateReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'>Received Date</td>");
        stringBuilder.Append("<td style='width: 60px' align='left' class='tblheader'>Receipt No</td>");
        stringBuilder.Append("<td style='width: 80px' align='left' class='tblheader'>Admission No</td>");
        stringBuilder.Append("<td style='width: 150px' align='left' class='tblheader'>Student Name</td>");
        stringBuilder.Append("<td style='width: 60px' align='left' class='tblheader'>Class</td>");
        stringBuilder.Append("<td align='left' class='tblheader'>Description</td>");
        stringBuilder.Append("<td style='width: 100px' align='right' class='tblheader'>Amount</td>");
        stringBuilder.Append("<td style='width: 100px' align='right' class='tblheader'>Fine Amount</td>");
        stringBuilder.Append("</tr>");
        double num1 = 0.0;
        double num2 = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["RecvDate"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["ReceiptNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["AdmnNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["Class"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["Description"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            stringBuilder.Append(row["Amount"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            stringBuilder.Append(row["fine"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            num1 += Convert.ToDouble(row["Amount"].ToString().Trim());
            num2 += Convert.ToDouble(row["fine"].ToString().Trim());
        }
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='6'  align='right'  class='tbltd'>");
        stringBuilder.Append("<strong class='error'>Total &nbsp;:&nbsp;</strong>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='right' class='tbltd'>");
        stringBuilder.Append("<strong class='error'>" + string.Format("{0:F2}", num1));
        stringBuilder.Append("</strong></td>");
        stringBuilder.Append("<td align='right'  class='tbltd'>");
        stringBuilder.Append("<strong class='error'>" + string.Format("{0:F2}", num2));
        stringBuilder.Append("</strong></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["FeeData"] = stringBuilder.ToString().Trim();
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
                stringBuilder.Append("Fee Cash Receipt :-");
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
            string str = Server.MapPath("Exported_Files/FeeReceiptRpt" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptFeeReceivedAllPrint.aspx');", true);
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

    private void fillstudent()
    {
        drpstudents.Items.Clear();
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        hashtable.Add("@Session", clsGenerateFee.CreateCurrSession());
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        DataTable dataTable = common.GetDataTable("ps_sp_get_StudNamesForClassSec", hashtable);
        drpstudents.DataSource = dataTable;
        drpstudents.DataTextField = "fullname";
        drpstudents.DataValueField = "admnno";
        drpstudents.DataBind();
        if (dataTable.Rows.Count > 0)
            drpstudents.Items.Insert(0, new ListItem("--ALL--", "0"));
        else
            drpstudents.Items.Insert(0, new ListItem("No Record", "0"));
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
            txtadminno.Text = "0";
            lblReport.Text = "";
        }
    }

    protected void txtadminno_TextChanged(object sender, EventArgs e)
    {
    }
}