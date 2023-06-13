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

public partial class Reports_AddFeeDefaulters : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        GetReportTable("");
        fillclass();
        FillClassSection();
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("All", "0"));
    }

    private void FillClassSection()
    {
        ddlSection.Items.Clear();
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@classId", drpclass.SelectedValue.ToString().Trim());
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        hashtable.Add("@session", clsGenerateFee.CreateCurrSession());
        ddlSection.DataSource = common.GetDataTable("ps_sp_get_ClassSections", hashtable);
        ddlSection.DataTextField = "Section";
        ddlSection.DataValueField = "Section";
        ddlSection.DataBind();
        ddlSection.Items.Insert(0, new ListItem("All", "0"));
    }

    protected void GetReportTable(string condition)
    {
        DataTable dt = new Common().ExecuteSql((!(condition.ToString().Trim() == "") ? "select f.ClassWiseId, fullname, c.ClassName,cl.RollNo,cast(sum(f.Balance) as decimal(18,2)) as totBalance, Section from dbo.PS_AdFeeLedger f, PS_ClassMaster c,PS_StudMaster s,PS_ClasswiseStudent cl where  f.Balance > 0 and  s.AdmnNo=cl.admnno and f.ClassWiseId=cl.id and c.ClassID=s.Class   and " + condition.ToString().Trim() + "group by f.ClassWiseId,fullname,c.ClassName,cl.RollNo, Section " : "select f.ClassWiseId, fullname, c.ClassName,cl.RollNo,cast(sum(f.Balance) as decimal(18,2)) as totBalance, Section from dbo.PS_AdFeeLedger f, PS_ClassMaster c,PS_StudMaster s,PS_ClasswiseStudent cl where  f.Balance > 0 and  s.AdmnNo=cl.admnno and f.ClassWiseId=cl.id and c.ClassID=s.Class  group by f.ClassWiseId,fullname,c.ClassName,cl.RollNo, Section ").ToString().Trim());
        if (dt.Rows.Count > 0)
        {
            lblGrdMsg.Text = "";
            createDefaultersReport(dt);
        }
        else
        {
            lblGrdMsg.Text = "No Record Found.";
            lblreport.Text = "";
        }
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
    }

    private void createDefaultersReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1'  width='100%' style='background-color:#CCC;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='text-align:left;' class='tblheader'>Name</td>");
        stringBuilder.Append("<td style='width: 80px; text-align:left;' class='tblheader'>Class</td>");
        stringBuilder.Append("<td style='width: 80px; text-align:left;' class='tblheader'>Section</td>");
        stringBuilder.Append("<td style='width: 100px; text-align:left;' class='tblheader'>Roll No</td>");
        stringBuilder.Append("<td style='width: 100px; text-align:right;' class='tblheader'>Balance</td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["fullname"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["ClassName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["Section"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["RollNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            stringBuilder.Append(row["totBalance"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblreport.Text = stringBuilder.ToString().Trim();
        Session["printData"] = stringBuilder.ToString().Trim();
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        string str = " ClassName = '" + drpclass.SelectedItem.Text.ToString().Trim() + "'";
        if (ddlSection.SelectedItem.Text.ToString().Trim().ToLower() != "all")
            str = str + " and Section='" + ddlSection.SelectedItem.Text.ToString().Trim() + "'";
        GetReportTable(str.ToString().Trim());
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblreport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("<tr><td align='left'><b>");
                stringBuilder.Append("Defaulters:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + lblreport.Text.ToString().Trim()).ToString().Trim());
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

    protected void btnBack_Click(object sender, EventArgs e)
    {
        GetReportTable("");
    }

    protected void btnPrint_Click1(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("AddFeeDefaultersPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}