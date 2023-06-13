using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Reports_rptStudListClass : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        FillDropDown();
        txtSession.Text = new clsGenerateFee().CreateCurrSession();
    }

    protected void FillDropDown()
    {
        drpClasses.DataSource = new Common().GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
        drpClasses.DataTextField = "ClassName";
        drpClasses.DataValueField = "ClassID";
        drpClasses.DataBind();
    }

    private void createRemarkReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='0' Border='' width='70%'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Addmission No</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Addmission Date</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='right'><strong><u>Full Name</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Nick Name</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Date Of Birth</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='right'><strong><u>Father Name</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Father Occupation</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Mother Name</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='right'><strong><u>Mother Occupation</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Present Address</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Permanent Address</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='right'><strong><u>Office Phone No</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Residence Phone No</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Nationality</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='right'><strong><u>Office Phone No</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Sports Proof</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Hobbies</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='right'><strong><u>Status</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>House Name</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Categroy</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='right'><strong><u>Session</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='right'><strong><u>Class</u></strong></td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["AdmnNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["AdmnDate"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["NickName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["DOB"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["FatherName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["FatherOccupation"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["MotherName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["MotherOccupation"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["PresentAddress"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["PermAddress"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["TeleNoOffice"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["TelNoResidence"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Nationality"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["SportsProf"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Hobbies"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Status"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["HouseName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["CatName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Session"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["ClassName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            lblReport.Text = stringBuilder.ToString().Trim();
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (drpClasses.Text == "ALL")
        {
            DataTable dataTable = new Common().GetDataTable("ps_sp_get_StudentListReport", new Hashtable()
      {
        {
           "session",
           txtSession.Text
        }
      });
            grdStudentMasterReport.DataSource = dataTable;
            grdStudentMasterReport.DataBind();
            createRemarkReport(dataTable);
        }
        else
        {
            DataTable dataTable = new Common().GetDataTable("ps_sp_get_StudentListReport", new Hashtable()
      {
        {
           "session",
           txtSession.Text
        },
        {
           "classid",
           drpClasses.SelectedValue
        }
      });
            grdStudentMasterReport.DataSource = dataTable;
            grdStudentMasterReport.DataBind();
            createRemarkReport(dataTable);
        }
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
                stringBuilder.Append("Fee Receipt Bank :-");
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
}