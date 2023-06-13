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

public partial class Library_rptIssue : System.Web.UI.Page
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
            bindDropDown(ddlClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
            bindDropDown(ddlMemberId, "select MemberId,MemberName+' ( '+EmpNo+' )' as MemberName from Lib_Member where MemberType='2' and  (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') AND CollegeId=" + Session["SchoolId"] + " order by MemberName", "MemberName", "MemberId");
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
            stringBuilder.Append("<td style='text-align:center' colspan='5'><span style='font-size:large; font-weight:bold'>Issue List Of Books</span>");
            if (rdbtnlstUsertype.SelectedValue == "0")
                stringBuilder.Append("<span style='font-size:14; font-weight:bold'> (For Staff)</span></td>");
            else
                stringBuilder.Append("<span style='font-size:14; font-weight:bold'> (For Student)</span></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr style='font-weight:bold;text-align:left;'>");
            stringBuilder.Append("<td>Sl No.</td>");
            stringBuilder.Append("<td>Accession Number</td>");
            stringBuilder.Append("<td>Member Name</td>");
            stringBuilder.Append("<td>Book Title</td>");
            stringBuilder.Append("<td>Issue Date</td>");
            stringBuilder.Append("</tr>");
            int num = 1;
            foreach (DataRow row in (InternalDataCollectionBase)dataForReport.Rows)
            {
                stringBuilder.Append("<tr style='text-align:left;'>");
                stringBuilder.Append("<td>" + num.ToString() + "</td>");
                stringBuilder.Append("<td>" + row["AccessionNo"].ToString() + "</td>");
                stringBuilder.Append("<td>" + row["MemberName"].ToString() + "</td>");
                stringBuilder.Append("<td>" + row["BookTitle"].ToString() + "</td>");
                stringBuilder.Append("<td>" + row["IssueDate"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
                ++num;
            }
            lblReport.Text = stringBuilder.ToString();
            stringBuilder.Append("</table>");
            btnPrint.Enabled = true;
            btnExport.Enabled = true;
        }
        else
        {
            lblReport.Text = "No Data Found";
            btnPrint.Enabled = false;
            btnExport.Enabled = false;
        }
        if (stringBuilder.ToString() != "")
            Session["rptIssuePrint"] = stringBuilder.ToString();
        else
            Session["rptIssuePrint"] = null;
    }

    private DataTable getDataForReport()
    {
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder = new StringBuilder();
        if (rdbtnlstUsertype.SelectedValue == "0")
            stringBuilder.Append("select * from Lib_VW_IssueReturnList where CollegeId=" + Session["SchoolId"] + " and BookCollegeId=" + Session["SchoolId"] + " and MemberType='1'");
        else
            stringBuilder.Append("select * from Lib_VW_IssueReturnListStud where CollegeId=" + Session["SchoolId"] + " and BookCollegeId=" + Session["SchoolId"]);
        if (txtFrmDate.Text != "")
            stringBuilder.Append(" and IssueDtOr >='" + dtpFromdt.SelectedDate + "' and IssueDtOr <='" + dtpTodt.SelectedDate + "'");
        if (ddlClass.SelectedIndex > 0)
            stringBuilder.Append(" and ClassID=" + ddlClass.SelectedValue);
        if (ddlMemberId.SelectedIndex > 0)
            stringBuilder.Append(" and MemberId=" + ddlMemberId.SelectedValue);
        return obj.GetDataTableQry(stringBuilder.ToString());
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (Session["rptIssuePrint"] != null)
        {
            try
            {
                ExportToExcel(Session["rptIssuePrint"].ToString());
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
            string str = Server.MapPath("Exported_Files/Issue_Report_" + DateTime.Now.ToString("ddMMyyyyhhmmssfffffff") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Issue_Report_" + DateTime.Now.ToString("ddMMyyyyhhmmssfffffff") + ".xls");
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

    protected void rdbtnlstUsertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtnlstUsertype.SelectedValue == "0")
        {
            bindDropDown(ddlMemberId, "select MemberId,MemberName+' ( '+EmpNo+' )' as MemberName from Lib_Member where MemberType='1' and (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') and CollegeId=" + Session["SchoolId"] + " order by MemberName", "MemberName", "MemberId");
            ddlClass.SelectedIndex = 0;
            ddlClass.Enabled = false;
        }
        else
        {
            bindDropDown(ddlClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
            ddlClass.Enabled = true;
            BindStudent();
        }
    }

    private void BindStudent()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select MemberId,MemberName+' ( '+EmpNo+' )' as MemberName from Lib_Member M");
        stringBuilder.Append(" inner join dbo.PS_ClasswiseStudent C on C.admnno=M.EmpNo");
        stringBuilder.Append(" where MemberType='2' and (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') and M.CollegeId=" + Session["SchoolId"] + " and (detained_promoted='' or Detained_Promoted is null)");
        if (ddlClass.SelectedIndex > 0)
            stringBuilder.Append(" and C.ClassID=" + int.Parse(ddlClass.SelectedValue));
        stringBuilder.Append(" order by MemberName");
        bindDropDown(ddlMemberId, stringBuilder.ToString(), "MemberName", "MemberId");
    }

    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindStudent();
        }
        catch (Exception ex)
        {
        }
    }
}