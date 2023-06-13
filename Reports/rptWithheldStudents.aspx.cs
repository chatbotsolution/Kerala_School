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

public partial class Reports_rptWithheldStudents : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        fillsession();
        fillclass();
        FillClassSection();
        btnPrint.Visible = false;
    }

    private void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        drpClasses.Items.Clear();
        drpClasses.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpClasses.DataTextField = "classname";
        drpClasses.DataValueField = "classid";
        drpClasses.DataBind();
        drpClasses.Items.Insert(0, new ListItem("All", "0"));
    }

    private void FillClassSection()
    {
        drpSection.Items.Clear();
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "@classId",
         drpClasses.SelectedValue.ToString().Trim()
      },
      {
         "@session",
         drpSession.SelectedValue.ToString().Trim()
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("All", "0"));
    }

    private void createRemarkReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='750px'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='7' style='background-color:#FFF;'>");
        stringBuilder.Append("<div class='spacer'><img src='../images/mask.gif'></div>");
        if (!(drpClasses.SelectedItem.ToString() == "ALL"))
            dt.Rows[0]["ClassName"].ToString().Trim();
        stringBuilder.Append("<strong>Class :</strong>  <span class='error'>" + drpClasses.SelectedItem.ToString().Trim() + " </span>&nbsp;&nbsp;&nbsp;&nbsp;");
        stringBuilder.Append("<strong>Session : </strong> <span class='error'>" + dt.Rows[0]["SessionYear"].ToString().Trim() + "</span>");
        stringBuilder.Append("<div class='spacer'><img src='../images/mask.gif'></div>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 80px; text-align:left;' class='tblheader'>Class</td>");
        stringBuilder.Append("<td style='width: 80px; text-align:left;' class='tblheader'>Section</td>");
        stringBuilder.Append("<td style='text-align:left;' class='tblheader'>Admission No</td>");
        stringBuilder.Append("<td style='width: 50px; text-align:left;' class='tblheader'>Roll No</td>");
        stringBuilder.Append("<td style='width: 100px; text-align:left;' class='tblheader'>Name</td>");
        stringBuilder.Append("<td style='width: 100px; text-align:left;' class='tblheader'>Father Name</td>");
        stringBuilder.Append("<td style='width: 50px; text-align:left;'class='tblheader'>Grade</td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align:left;'>");
            stringBuilder.Append(row["ClassName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td style='text-align:left;'>");
            stringBuilder.Append(row["Section"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td style='text-align:left;'>");
            stringBuilder.Append(row["admnno"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td style='text-align:left;'>");
            stringBuilder.Append(row["RollNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td style='text-align:left;'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td style='text-align:left;'>");
            stringBuilder.Append(row["FatherName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td style='text-align:left;'>");
            stringBuilder.Append(row["Grade"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["printData"] = stringBuilder.ToString().Trim();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select cs.SessionYear,c.ClassName,cs.Section,cs.admnno,cs.RollNo,s.FullName,s.FatherName,cs.Grade ");
        stringBuilder.Append(" from dbo.PS_ClasswiseStudent cs ");
        stringBuilder.Append(" inner join PS_ClassMaster c on c.ClassID = cs.ClassID ");
        stringBuilder.Append(" inner join dbo.PS_StudMaster s on s.admnno=cs.admnno ");
        stringBuilder.Append(" where cs.Detained_Promoted='d' ");
        if (drpSession.SelectedValue.ToString().Trim() != "0")
            stringBuilder.Append(" and cs.SessionYear= '" + drpSession.SelectedValue + "'");
        if (drpClasses.SelectedValue.ToString().Trim() != "0")
            stringBuilder.Append(" and cs.ClassID= '" + drpClasses.SelectedValue + "'");
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            stringBuilder.Append(" and Section= '" + drpSection.SelectedValue + "'");
        stringBuilder.Append(" order by cs.ClassID,cs.RollNo,s.FullName ");
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
                stringBuilder.Append("Detained Students :-");
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
            Response.Redirect("rptWithheldStudentsprint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnPrint.Visible = false;
        lblReport.Text = "";
    }

    protected void drpClasses_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnPrint.Visible = false;
        FillClassSection();
        lblReport.Text = "";
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnPrint.Visible = false;
        lblReport.Text = "";
    }
}