using ASP;
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

public partial class HR_rptEmployeeListNew : System.Web.UI.Page
{
    private Hashtable ht;
    private DataTable dt;
    private clsDAL ObjCommon;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        bindAllDrps();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    private void GenerateReport()
    {
        ht = new Hashtable();
        dt = new DataTable();
        ObjCommon = new clsDAL();
        if (drpDesig.SelectedIndex > 0)
            ht.Add("@DesigId", drpDesig.SelectedValue.ToString());
        if (drpEduQual.SelectedIndex > 0)
            ht.Add("@QualId", drpEduQual.SelectedValue.ToString());
        if (drpStatus.SelectedIndex > 0)
            ht.Add("@ActiveStatus", drpStatus.SelectedValue.ToString());
        if (!txtSevabratiName.Text.Trim().Equals(string.Empty))
            ht.Add("@SevName", txtSevabratiName.Text.Trim());
        dt = ObjCommon.GetDataTable("HR_GetEmployeeDetailsRpt", ht);
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table cellpadding='1' cellspacing='0' border='1'  width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='6' align='left' class='gridtext'><font color='Black'>" + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt") + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td colspan='5' align='right' class='gridtext'><font color='Black'>No Of Records: " + dt.Rows.Count.ToString() + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 120px' align='left' class='headergrid'>Employee Name</td>");
            stringBuilder.Append("<td style='width: 100px' align='left' class='headergrid'>Designation</td>");
            stringBuilder.Append("<td style='width: 80px' align='left' class='headergrid'>Appt.Date</td>");
            stringBuilder.Append("<td style='width: 80px' align='left' class='headergrid'>Leaving Dt.</td>");
            stringBuilder.Append("<td style='width: 50px' align='left' class='headergrid'>Gender</td>");
            stringBuilder.Append("<td style='width: 80px' align='left' class='headergrid'>D.O.B</td>");
            stringBuilder.Append("<td style='width: 100px' align='left' class='headergrid'>Qual</td>");
            stringBuilder.Append("<td style='width: 80px' align='left' class='headergrid'>Mobile</td>");
            stringBuilder.Append("<td style='width: 50px' align='left' class='headergrid'>Blood Group</td>");
            stringBuilder.Append("<td style='width: 120px' align='left' class='headergrid'>Marital Status</td>");
            stringBuilder.Append("<td style='width: 120px' align='left' class='headergrid'>Address</td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' valign='top' class='gridtext'>");
                stringBuilder.Append(row["SevName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["Designation"].ToString().Trim().Equals(string.Empty))
                    stringBuilder.Append(row["Designation"].ToString().Trim());
                else
                    stringBuilder.Append("<font color='gray'>-N/A-<font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["AppointmentDate"].ToString().Trim().Equals(string.Empty))
                    stringBuilder.Append(row["AppointmentDate"].ToString().Trim());
                else
                    stringBuilder.Append("<font color='gray'>-N/A-<font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["LeavingDateStr"].ToString().Trim().Equals(string.Empty))
                    stringBuilder.Append(row["LeavingDateStr"].ToString().Trim());
                else
                    stringBuilder.Append("<font color='gray'>-N/A-<font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["Sex"].ToString().Trim().Equals(string.Empty))
                    stringBuilder.Append(row["Sex"].ToString().Trim());
                else
                    stringBuilder.Append("<font color='gray'>-N/A-<font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["BirthDate"].ToString().Trim().Equals(string.Empty))
                    stringBuilder.Append(row["BirthDate"].ToString().Trim());
                else
                    stringBuilder.Append("<font color='gray'>-N/A-<font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["EduQual"].ToString().Trim().Equals(string.Empty))
                    stringBuilder.Append(row["EduQual"].ToString().Trim());
                else
                    stringBuilder.Append("<font color='gray'>-N/A-<font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["Mobile"].ToString().Trim().Equals(string.Empty))
                    stringBuilder.Append(row["Mobile"].ToString().Trim());
                else
                    stringBuilder.Append("<font color='gray'>-N/A-<font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["BloodGroup"].ToString().Trim().Equals(string.Empty))
                    stringBuilder.Append(row["BloodGroup"].ToString().Trim());
                else
                    stringBuilder.Append("<font color='gray'>-N/A-<font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["MaritalStatus"].ToString().Trim().Equals(string.Empty))
                    stringBuilder.Append(row["MaritalStatus"].ToString().Trim());
                else
                    stringBuilder.Append("<font color='gray'>-N/A-<font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["PresAddress"].ToString().Trim().Equals(string.Empty))
                    stringBuilder.Append(row["PresAddress"].ToString().Trim());
                else
                    stringBuilder.Append("<font color='gray'>-N/A-<font>");
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            btnExport.Enabled = true;
        }
        else
        {
            lblReport.Text = "No Records";
            btnExport.Enabled = false;
        }
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        ObjCommon = new clsDAL();
        dt = ObjCommon.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--All--", "0"));
    }

    private void bindAllDrps()
    {
        BindDropDown(drpEduQual, "SELECT QualId,QualName FROM dbo.HR_QualMaster ORDER BY QualName", "QualName", "QualId");
        BindDropDown(drpDesig, "select DesgId,Designation,TeachingStaff from dbo.HR_DesignationMaster order by Designation", "Designation", "DesgId");
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td align='left'><b>");
                stringBuilder.Append("Employee List :-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
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
            string str = Server.MapPath("~/Up_Files/Reportfiles/" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=REPORT_StaffList" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}