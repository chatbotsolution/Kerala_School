using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Hostel_rptHostFeeDefaulters : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        btnPrint.Enabled = false;
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        drpDueMnth.SelectedValue = DateTime.Now.Month.ToString();
        fillsession();
        fillclass();
    }

    private void fillsession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = new clsDAL().GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("-All-", "0"));
    }

    private void FillClassSection()
    {
        ddlSection.Items.Clear();
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@classId", drpclass.SelectedValue.ToString().Trim());
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        hashtable.Add("@session", clsGenerateFee.CreateCurrSession());
        ddlSection.DataSource = clsDal.GetDataTable("ps_sp_get_ClassSections", hashtable);
        ddlSection.DataTextField = "Section";
        ddlSection.DataValueField = "Section";
        ddlSection.DataBind();
        ddlSection.Items.Insert(0, new ListItem("All", "0"));
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        GetReportTable();
    }

    protected void GetReportTable()
    {
        Session["DueDate"] = string.Empty;
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
        if (drpclass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpclass.SelectedValue.ToString().Trim());
        if (ddlSection.SelectedIndex > 0)
            hashtable.Add("@Section", ddlSection.SelectedValue.ToString().Trim());
        if (drpFeeType.SelectedIndex > 0)
            hashtable.Add("@Type", drpFeeType.SelectedValue.Trim());
        if (drpDueMnth.SelectedIndex > 0)
        {
            string feeDateStr = GetFeeDateStr();
            hashtable.Add("@DueDate", feeDateStr);
            Session["DueDate"] = feeDateStr;
        }
        DataTable dataTable = clsDal.GetDataTable("HostRptGetFeeDefaulters", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            lblGrdMsg.Text = string.Empty;
            createDefaultersReport(dataTable);
            btnPrint.Enabled = true;
        }
        else
        {
            lblReport.Text = string.Empty;
            lblGrdMsg.Text = "No Record Found.";
            btnPrint.Enabled = false;
        }
    }

    private string GetFeeDateStr()
    {
        clsDAL clsDal = new clsDAL();
        string str1 = drpDueMnth.SelectedValue.ToString();
        string str2 = (Convert.ToInt32(drpDueMnth.SelectedValue.ToString().Trim()) + 1).ToString();
        string currSession = new clsGenerateFee().CreateCurrSession();
        string str3 = currSession.Substring(0, 4);
        string str4 = currSession.Substring(0, 2) + currSession.Substring(5, 2);
        return Convert.ToInt32(str1) <= 3 ? str2 + "/01/" + str4 : (Convert.ToInt32(str1) <= 11 ? str2 + "/01/" + str3 : "01/01/" + str4);
    }

    private DateTime feedate()
    {
        clsDAL clsDal = new clsDAL();
        string str1 = drpDueMnth.SelectedValue.ToString();
        string str2 = (Convert.ToInt32(drpDueMnth.SelectedValue.ToString().Trim()) + 1).ToString();
        string str3 = clsDal.ExecuteScalarQry("select top 1 SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        string str4 = str3.Substring(0, 4);
        string str5 = str3.Substring(0, 2) + str3.Substring(5, 2);
        DateTime dateTime;
        if (Convert.ToInt32(str1) > 3)
        {
            if (Convert.ToInt32(str1) > 11)
            {
                dateTime = Convert.ToDateTime("01/01/" + str5);
                dateTime.Date.AddDays(-1.0);
            }
            else
            {
                dateTime = Convert.ToDateTime(str2 + "/01/" + str4);
                dateTime.Date.AddDays(-1.0);
            }
        }
        else
        {
            dateTime = Convert.ToDateTime(str2 + "/01/" + str5);
            dateTime.Date.AddDays(-1.0);
        }
        return dateTime;
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
        lblReport.Text = string.Empty;
        btnPrint.Enabled = false;
    }

    private void createDefaultersReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
        stringBuilder.Append("<tr style='background-color:#CCCCCC'>");
        stringBuilder.Append("<td style='width: 100px' align='left'><strong><u>Admission No</u></strong></td>");
        stringBuilder.Append("<td align='left'><strong><u>Name</u></strong></td>");
        stringBuilder.Append("<td style='width: 70px' align='left'><strong><u>Class</u></strong></td>");
        stringBuilder.Append("<td style='width: 70px' align='left'><strong><u>Section</u></strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='left'><strong><u>Roll No</u></strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='right'><strong><u>Regular Fee Due</u></strong></td>");
        stringBuilder.Append("</tr>");
        double num1 = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["AdmnNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["fullname"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["ClassName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Section"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["RollNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            int num2 = 0;
            if (drpFeeType.SelectedIndex > 0)
                num2 = int.Parse(drpFeeType.SelectedValue.Trim());
            stringBuilder.Append("<td align='right'>");
            string str1 = "<a href='javascript:popUp";
            string str2 = "('rptHostDefaultersDetails.aspx?Admnno=" + row["Admnno"].ToString() + " &Type=" + num2 + "&DueDt=" + Session["DueDate"].ToString().Trim() + "&SessionYr=" + drpSession.SelectedValue.ToString().Trim() + "')";
            string str3 = "'>";
            stringBuilder.Append(str1.Replace('\'', '"') + str2 + str3.Replace('\'', '"'));
            stringBuilder.Append("<b>" + row["totBalance"].ToString() + "</b></a>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            num1 += Convert.ToDouble(row["totBalance"].ToString().Trim().Equals(string.Empty) ? "0" : row["totBalance"].ToString().Trim());
        }
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='5'  align='right'  class='tbltd'>");
        stringBuilder.Append("<strong>Grand Total &nbsp;:&nbsp;</strong>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='right'>");
        stringBuilder.Append("<strong>" + string.Format("{0:F2}", num1));
        stringBuilder.Append("</strong></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["rptDefaulters"] = Regex.Replace(stringBuilder.ToString(), "<a\\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("<tr><td align='left'><b>");
                stringBuilder.Append("Defaulters:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append(lblReport.Text.ToString().Trim());
                ExportToExcel(stringBuilder.ToString().Trim());
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
            string str = Server.MapPath("Exported_Files/Hostel Fee Deafaulters" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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

    protected void btnBack_Click(object sender, EventArgs e)
    {
        pnlDetail.Visible = false;
        GetReportTable();
        pnlSummary.Visible = true;
    }

    protected void btnPrint_Click1(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptHostDefaultersprint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpDueMnth_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["DueDate"] = string.Empty;
        lblReport.Text = string.Empty;
        btnPrint.Enabled = false;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillclass();
        lblReport.Text = string.Empty;
        btnPrint.Enabled = false;
    }
}