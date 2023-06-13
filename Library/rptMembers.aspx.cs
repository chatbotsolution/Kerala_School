using ASP;
using SanLib;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_rptMembers : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            Session["title"] = Page.Title.ToString();
            Form.DefaultButton = btnShow.UniqueID;
            btnExport.Enabled = false;
            btnPrint.Enabled = false;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---All---", "0"));
    }

    private void generateReport()
    {
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder = new StringBuilder();
        DataTable dataForReport = getDataForReport();
        if (dataForReport.Rows.Count > 0)
        {
            stringBuilder.Append("<table width='100%' border='1px' cellspacing='0' cellpadding='2px' class='tbltxt'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:center' colspan='6'><span style='font-size:large; font-weight:bold'>List Of Members</span>");
            if (ddlType.SelectedValue == "1")
                stringBuilder.Append("<span style='font-size:14; font-weight:bold'> (Staff)</span></td>");
            else if (ddlType.SelectedValue == "2")
                stringBuilder.Append("<span style='font-size:14; font-weight:bold'> (Student)</span></td>");
            else
                stringBuilder.Append("<span style='font-size:14; font-weight:bold'> (All)</span></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:right; font-weight:bold' colspan='6'>Total Member : " + dataForReport.Rows.Count + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr style='font-weight:bold; text-align:left;'>");
            if (ddlType.SelectedValue == "0")
                stringBuilder.Append("<td>EmpNo/Admn No</td>");
            else if (ddlType.SelectedValue == "1")
                stringBuilder.Append("<td>Emp No</td>");
            else
                stringBuilder.Append("<td>Admission No</td>");
            stringBuilder.Append("<td>Member Type</td>");
            stringBuilder.Append("<td style='width:200px;'>Member Name</td>");
            stringBuilder.Append("<td>Address</td>");
            stringBuilder.Append("<td>Phone</td>");
            stringBuilder.Append("<td style='width:200px;'>EmailId</td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dataForReport.Rows)
            {
                stringBuilder.Append("<tr style='text-align:left;'>");
                if (ddlType.SelectedValue == "0")
                    stringBuilder.Append("<td>" + row["EmpNo"] + "</td>");
                else if (ddlType.SelectedValue == "1")
                    stringBuilder.Append("<td>" + row["EmpNo"] + "</td>");
                else
                    stringBuilder.Append("<td>" + row["OldAdmnNo"] + "</td>");
             
                if (row["MemberType"].ToString() == "1")
                    stringBuilder.Append("<td>Staff</td>");
                else
                    stringBuilder.Append("<td>Student</td>");
                stringBuilder.Append("<td>" + row["MemberName"].ToString() + "</td>");
                stringBuilder.Append("<td>" + row["Address"].ToString() + "</td>");
                stringBuilder.Append("<td>" + row["Phone"].ToString() + "</td>");
                stringBuilder.Append("<td>" + row["EmailId"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString();
            btnPrint.Enabled = true;
            btnExport.Enabled = true;
        }
        else
        {
            lblReport.Text = "No Data Found";
            btnExport.Enabled = false;
            btnPrint.Enabled = false;
        }
        if (stringBuilder.ToString() != "")
            Session["memberReport"] = stringBuilder.ToString();
        else
            Session["memberReport"] = null;
    }

    private DataTable getDataForReport()
    {
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT * FROM Lib_VW_MemberList WHERE CollegeId=" + Session["SchoolId"]);
        if (ddlType.SelectedIndex > 0)
            stringBuilder.Append(" AND MemberType=" + ddlType.SelectedValue);
        if (txtName.Text != "")
            stringBuilder.Append(" and MemberName like '%" + txtName.Text.Trim() + "%'");
        stringBuilder.Append(" ORDER BY MemberName");
        return obj.GetDataTableQry(stringBuilder.ToString());
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (Session["memberReport"] != null)
        {
            try
            {
                ExportToExcel(Session["memberReport"].ToString());
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('No data exists to export !');", true);
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/Members_Report_" + DateTime.Now.ToString("ddMMyyyyhhmmssfffffff") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Members_Report_" + DateTime.Now.ToString("ddMMyyyyhhmmssfffffff") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        generateReport();
    }
}