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

public partial class HR_rptEmployeeList : System.Web.UI.Page
{
    private clsDAL ObjCommon = new clsDAL();
    private Hashtable ht;
    private DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        Session["title"] = Page.Title.ToString();
        bindAllDrps();
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        ObjCommon = new clsDAL();
        dt = ObjCommon.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--select--", "0"));
    }

    private void bindAllDrps()
    {
        bindSingleDrp(drpDesig, "select DesgId,Designation,TeachingStaff from dbo.HR_DesignationMaster order by Designation", "Designation", "DesgId");
        clsStaticDropdowns clsStaticDropdowns = new clsStaticDropdowns();
        drpEstType.SelectedValue = "SSVM";
        drpEstType.Enabled = false;
    }

    private void bindSingleDrp(DropDownList drp, string query, string text, string value)
    {
        dt = new DataTable();
        dt = ObjCommon.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = text;
        drp.DataValueField = value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    protected void drpEstType_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void GenerateReport()
    {
        dt = new DataTable();
        ObjCommon = new clsDAL();
        StringBuilder stringBuilder1 = new StringBuilder();
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        stringBuilder1.Append("SELECT EmpId, SevabratiId, SevName, Sex, DOB, MaritalStatus, FatherName, SpouseName, MotherName, EM.Phone, Mobile, EM.emailid, ActiveStatus, Remarks,PresentEstType,Designation,PresAddress");
        stringBuilder1.Append(",");
        stringBuilder1.Append(" from HR_EmployeeMaster EM ");
        stringBuilder1.Append(" inner join HR_DesignationMaster D on D.DesgId=EM.DesignationId");
        stringBuilder1.Append(empty2);
        stringBuilder1.Append(" WHERE 1=1 ");
        stringBuilder1.Append(empty3);
        if (drpDesig.SelectedIndex > 0)
            stringBuilder1.Append(" AND DesignationId=" + drpDesig.SelectedValue.ToString());
        if (!txtEmp.Text.Trim().Equals(string.Empty))
            stringBuilder1.Append(" AND SevName LIKE'%" + txtEmp.Text.Trim() + "%'");
        stringBuilder1.Append("  order by PresentEstType,OfficeName,SortOrder");
        dt = ObjCommon.GetDataTableQry(stringBuilder1.ToString());
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder2 = new StringBuilder("");
            stringBuilder2.Append("<table cellpadding='3' cellspacing='0' border='1'  width='100%'>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td style='width: 180px' align='left' class='headergrid'><b>Sevabrati Name</b></td>");
            stringBuilder2.Append("<td style='width: 150px' align='left' class='headergrid'>Designation</td>");
            stringBuilder2.Append("<td align='left' class='headergrid'><b>Address</b></td>");
            stringBuilder2.Append("<td style='width: 110px' align='left' class='headergrid'><b>Phone</b></td>");
            stringBuilder2.Append("<td style='width: 110px' align='left' class='headergrid'><b>Mobile</b></td>");
            stringBuilder2.Append("<td style='width: 170px' align='left' class='headergrid'><b>Email ID</b></td>");
            stringBuilder2.Append("</tr>");
            string empty4 = string.Empty;
            string empty5 = string.Empty;
            string empty6 = string.Empty;
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td align='left' style='width: 60px' class='gridtext'>");
                stringBuilder2.Append(row["SevName"].ToString().Trim());
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='left' valign='top' class='gridtext'>");
                stringBuilder2.Append(row["Designation"].ToString().Trim());
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='left' valign='top' class='gridtext'>");
                stringBuilder2.Append(row["PresAddress"].ToString().Trim());
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["Phone"].ToString().Trim().Equals(string.Empty))
                    stringBuilder2.Append(row["Phone"].ToString().Trim());
                else
                    stringBuilder2.Append("<font color='gray'>-N/A-<font>");
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["Mobile"].ToString().Trim().Equals(string.Empty))
                    stringBuilder2.Append(row["Mobile"].ToString().Trim());
                else
                    stringBuilder2.Append("<font color='gray'>-N/A-<font>");
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='left' valign='top' class='gridtext'>");
                if (!row["emailid"].ToString().Trim().Equals(string.Empty))
                    stringBuilder2.Append(row["emailid"].ToString().Trim());
                else
                    stringBuilder2.Append("<font color='gray'>-N/A-<font>");
                stringBuilder2.Append("</td>");
            }
            stringBuilder2.Append("</table>");
            lblReport.Text = stringBuilder2.ToString().Trim();
            Session["printDataEmp"] = stringBuilder2.ToString().Trim();
        }
        else
        {
            lblReport.Text = "No Records";
            Session["printDataEmp"] = "No Records";
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptEmployeeListPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (Session["printDataEmp"] != null)
        {
            try
            {
                ExportToExcel(Session["printDataEmp"].ToString());
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('No data exists to export !');", true);
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("../Up_Files/reportfiles/" + DateTime.Now.ToString("ddMMyyyyhhmmssfffffff") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=REPORT_EmployeeList" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}