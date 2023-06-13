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

public partial class Reports_rptStudRemarks : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        btnPrint.Visible = false;
        FillSessionDropDown();
        FillClassDropDown();
        FillSectionDropDown();
        fillstudent();
    }

    protected void FillSessionDropDown()
    {
        drpSession.DataSource = new Common().GetDataTable("ps_sp_get_Session", new Hashtable());
        drpSession.DataTextField = "SessionYear";
        drpSession.DataValueField = "SessionYear";
        drpSession.DataBind();
    }

    private void FillClassDropDown()
    {
        drpClass.DataSource = new Common().GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassID";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("ALL", "0"));
    }

    protected void FillSectionDropDown()
    {
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_Section", new Hashtable()
    {
      {
         "class",
         drpClass.SelectedValue
      },
      {
         "session",
         drpSession.SelectedValue
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("ALL", "0"));
    }

    private void fillstudent()
    {
        drpSelectStudent.Items.Clear();
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@Session", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        DataTable dataTable = common.GetDataTable("ps_sp_get_StudNamesForClassSec", hashtable);
        drpSelectStudent.DataSource = dataTable;
        drpSelectStudent.DataTextField = "fullname";
        drpSelectStudent.DataValueField = "admnno";
        drpSelectStudent.DataBind();
        if (dataTable.Rows.Count > 0)
            drpSelectStudent.Items.Insert(0, new ListItem("Select", "0"));
        else
            drpSelectStudent.Items.Insert(0, new ListItem("No Record", "0"));
    }

    private void createRemarkReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1'  width='100%' style='background-color:#CCC;' >");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='7' class='tbltxt' style='background-color:#FFF;'>");
        stringBuilder.Append("<div class='spacer'><img src='../images/mask.gif'></div>");
        stringBuilder.Append("<strong>Student Admission No :</strong> <span class='error'>" + dt.Rows[0]["AdmnNo"].ToString().Trim() + "</span>&nbsp;&nbsp;");
        stringBuilder.Append("<strong>Name :</strong> <span class='error'>" + dt.Rows[0]["FullName"].ToString().Trim() + "</span>");
        stringBuilder.Append("<div class='spacer'><img src='../images/mask.gif'></div>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 100px; text-align:left;' class='tblheader'>Remark's Date</td>");
        stringBuilder.Append("<td style='width: 150px;text-align:left;' class='tblheader'>Teacher's Name</td>");
        stringBuilder.Append("<td style=' text-align:left;' class='tblheader'>Study Remarks</td>");
        stringBuilder.Append("<td style=' text-align:left;' class='tblheader'>Sports Remarks </td>");
        stringBuilder.Append("<td style=' text-align:left;' class='tblheader'>Cultural Remarks </td>");
        stringBuilder.Append("<td style=' text-align:left;'class='tblheader'>Special Reference </td>");
        stringBuilder.Append("<td style=' text-align:left;'class='tblheader'> Annual Result</td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td class='tbltd'>");
            stringBuilder.Append(row["RemarksDate"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td class='tbltd'>");
            stringBuilder.Append(row["Teacher"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td class='tbltd'>");
            stringBuilder.Append(row["StudyRemarks"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td class='tbltd'>");
            stringBuilder.Append(row["SportsRemarks"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td class='tbltd'>");
            stringBuilder.Append(row["CulturalRemarks"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td class='tbltd'>");
            stringBuilder.Append(row["SpecialReference"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td class='tbltd'>");
            stringBuilder.Append(row["AnnualResult"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["printData"] = stringBuilder.ToString().Trim();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        StringBuilder stringBuilder = new StringBuilder();
        string str1 = PopCalendar1.GetDateValue().ToString("MM/dd/yyyy");
        string str2 = PopCalendar2.GetDateValue().ToString("MM/dd/yyyy");
        string str3 = "04/01/" + new clsGenerateFee().CreateCurrSession().Substring(0, 4);
        stringBuilder.Append("select  Convert(varchar,r.RemarksDate,103)as RemarksDate , c.admnno,m.FullName,r.Teacher,r.StudyRemarks,r.SportsRemarks,r.CulturalRemarks,r.SpecialReference,r.AnnualResult,c.ClassID, c.SessionYear, c.Section ");
        stringBuilder.Append(" from  PS_StudRemarks r inner join PS_StudMaster m on m.AdmnNo=r.AdmnNo inner join PS_ClasswiseStudent c on c.admnno=m.AdmnNo  ");
        stringBuilder.Append(" where 1=1");
        if (txtFromDate.Text.Trim() != "")
            stringBuilder.Append(" and r.RemarksDate >= '" + str1 + "'");
        else
            stringBuilder.Append(" and r.RemarksDate >= '" + str3 + "'");
        if (txtToDate.Text.Trim() != "")
            stringBuilder.Append(" and r.RemarksDate <= '" + str2 + "'");
        else
            stringBuilder.Append(" and r.RemarksDate <= '" + DateTime.Today.ToString("MM/dd/yyyy") + "'");
        if (drpSelectStudent.SelectedIndex > 0)
            stringBuilder.Append(" and m.AdmnNo = " + drpSelectStudent.SelectedValue);
        DataTable dt = new Common().ExecuteSql(stringBuilder.ToString().Trim());
        if (dt.Rows.Count > 0)
        {
            createRemarkReport(dt);
            btnPrint.Visible = true;
        }
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
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
                stringBuilder.Append("Student Remarks :-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + lblReport.Text.ToString().Trim()).ToString().Trim());
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
            string str = Server.MapPath("Exported_Files/" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
            Response.Redirect("rptStudRemarksPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSectionDropDown();
        fillstudent();
        lblReport.Text = "";
        btnPrint.Visible = false;
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
        lblReport.Text = "";
        btnPrint.Visible = false;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSectionDropDown();
        fillstudent();
        lblReport.Text = "";
        btnPrint.Visible = false;
    }

    protected void drpSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnPrint.Visible = false;
        lblReport.Text = "";
    }
}