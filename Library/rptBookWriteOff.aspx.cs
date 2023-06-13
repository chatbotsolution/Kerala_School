using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Library_rptBookWriteOff : System.Web.UI.Page
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

    private void generateReport()
    {
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder = new StringBuilder();
        DataTable dataForReport = getDataForReport();
        if (dataForReport.Rows.Count > 0)
        {
            stringBuilder.Append("<table width='100%' border='1px' cellspacing='0' cellpadding='2px' class='tbltxt'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:center; font-size:large; font-weight:bold' colspan='6'>List Of Book WritesOff </td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr style='font-weight:bold; text-align:left'>");
            stringBuilder.Append("<td>Sl No.</td>");
            stringBuilder.Append("<td>Accession Number</td>");
            stringBuilder.Append("<td>Book Title</td>");
            stringBuilder.Append("<td>WriteOff Date</td>");
            stringBuilder.Append("<td>Reason</td>");
            stringBuilder.Append("<td>Description</td>");
            stringBuilder.Append("</tr>");
            int num = 1;
            foreach (DataRow row in (InternalDataCollectionBase)dataForReport.Rows)
            {
                stringBuilder.Append("<tr style='text-align:left'>");
                stringBuilder.Append("<td>" + num.ToString() + "</td>");
                stringBuilder.Append("<td>" + row["AccessionNo"].ToString() + "</td>");
                stringBuilder.Append("<td>" + row["BookTitle"].ToString() + "</td>");
                stringBuilder.Append("<td>" + row["WriteOffDate"].ToString() + "</td>");
                stringBuilder.Append("<td>" + row["Reason"].ToString() + "</td>");
                stringBuilder.Append("<td>" + row["Description"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
                ++num;
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
            Session["BookWriteoffReport"] = stringBuilder.ToString();
        else
            Session["BookWriteoffReport"] = null;
    }

    private DataTable getDataForReport()
    {
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select * from Lib_VW_BookWriteOffList where CollegeId=" + Session["SchoolId"] + " and 1=1");
        if (txtFrmDate.Text != "")
            stringBuilder.Append(" and WriteOffDateOr >='" + dtpFromdt.SelectedDate + "' and WriteOffDateOr <='" + dtpTodt.SelectedDate + "'");
        if (ddlReason.SelectedIndex > 0)
            stringBuilder.Append(" and Status='" + ddlReason.SelectedValue + "'");
        return obj.GetDataTableQry(stringBuilder.ToString());
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (Session["BookWriteoffReport"] != null)
        {
            try
            {
                ExportToExcel(Session["BookWriteoffReport"].ToString());
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
            string str = Server.MapPath("Exported_Files/WritesOff_Report_" + DateTime.Now.ToString("ddMMyyyyhhmmssfffffff") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=WritesOff_Report_" + DateTime.Now.ToString("ddMMyyyyhhmmssfffffff") + ".xls");
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