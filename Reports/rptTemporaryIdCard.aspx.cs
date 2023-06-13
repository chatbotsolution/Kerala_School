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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptTemporaryIdCard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        FillSession();
        FillClass();
        FillSectionDropDown();

    }
    protected void FillSession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillSectionDropDown()
    {
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "classId",
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

    private void FillClass()
    {
        drpClass.Items.Clear();
        drpClass.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpClass.DataTextField = "classname";
        drpClass.DataValueField = "classid";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("--Select--", "0"));
    }
 
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@Session", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Admnno", drpSelectStudent.SelectedValue);
        DataTable dataTable2 = common.GetDataTable("ps_sp_get_StudInfoForTempICard ", hashtable);
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
    private void GenerateReport(DataTable dt)
    {
        StringBuilder stringBuilder = new StringBuilder("");
        int num = 1;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<div style='margin:0px;padding: 0px; padding-bottom:10px; width: 100%'>");
            string table1 = CreateTable(row);
            string table2 = CreateTable(row);
            stringBuilder.Append(table1);
            stringBuilder.Append(table2);
            stringBuilder.Append("</div>");

            stringBuilder.Append("<hr/>");

            ++num;

        }
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["TempICard"] = stringBuilder.ToString().Trim();


    }

    private string CreateTable(DataRow row)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table style='margin-top: 10px; padding-left: 15px; padding-right: 15px; width: 50% !important; float: left; border:solid 1px red; display: table'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='2' style='font-size: 18px'>");
        stringBuilder.Append("<img src='../images/logo-new.png' width='70' height='70' alt='logo' style='float: left;margin-right: 10px'>");
        stringBuilder.Append("<b>Kerala School</b><br>Temporary Identity Card");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");

        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 60%;padding-top: 3px'>");
        stringBuilder.Append("<b>Name:</b> " + row["FullName"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td style='width: 40%;padding-top: 3px;' rowspan='6'>");
        stringBuilder.Append("<div style='width:150px; height:187px; border:solid 1px #322c2c;'></div>");
      
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");

        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 60%;padding-top:3px'>");
        stringBuilder.Append("<b>Admin No:</b>" + row["AdmnNo"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");

        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 60%;padding-top:3px'>");
        stringBuilder.Append("<b>Class:</b>&nbsp;" + row["ClassName"].ToString().Trim() + " &nbsp;<b>Sec:</b> &nbsp;" + row["Section"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");

        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 60%;padding-top:3px'>");
        stringBuilder.Append("<b>DOB:</b>" + row["dob"].ToString().Trim());
        stringBuilder.Append("</td>");

        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 60%;padding-top:3px'>");
        stringBuilder.Append("<b>Blood Group:</b>" + row["BloodGroup"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");

        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 60%;padding-top:3px'>");
        stringBuilder.Append("<b>Mother's Name:</b>" + row["MotherName"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");

        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 80%;padding-top:3px'>");
        stringBuilder.Append("<b>Father's Name:</b>" + row["FatherName"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td style='width: 20%;padding-top: 3px;' rowspan='2'>");
        stringBuilder.Append("<img src='' width='100px' height='55' style='margin-left:25px; '>");
        stringBuilder.Append("<center><span>Principal</span></center>");
        stringBuilder.Append("</td></tr>");

        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 80%;padding-top:3px'>");
        stringBuilder.Append("<b>Phone No:</b>" + row["MobileNo1"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");

        return stringBuilder.ToString();
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/StudentICardrpt" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptTemporaryIdCardPrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
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
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td align='left' colspan='14'><b>");
                stringBuilder.Append("Temporary ICard Report :-");
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

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSectionDropDown();
        FillSelectStudent();
    }
    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillSelectStudent();
    }

    private void FillSelectStudent()
    {
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@SessionYear", drpSession.SelectedValue);
        if (drpClass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpClass.SelectedValue);
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue);
        drpSelectStudent.DataSource = common.GetDataTable("ps_StudSectionAlert", hashtable);
        drpSelectStudent.DataTextField = "FullName";
        drpSelectStudent.DataValueField = "admnno";
        drpSelectStudent.DataBind();
        drpSelectStudent.Items.Insert(0, new ListItem("--ALL--", "0"));
    }
}