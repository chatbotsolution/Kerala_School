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


public partial class Hostel_rptHostAdmnList : System.Web.UI.Page
{
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            fillsession();
            fillclass();
            lblRecCount.Text = "";
            btnExpExcel.Visible = false;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
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

    protected void btngo_Click(object sender, EventArgs e)
    {
        bindstudentlist();
    }

    private void bindstudentlist()
    {
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
        if (drpclass.SelectedIndex > 0)
            hashtable.Add("@ClassId", drpclass.SelectedValue.ToString().Trim());
        if (drpGender.SelectedIndex > 0)
            hashtable.Add("@Sex", drpGender.SelectedValue.ToString().Trim());
        if (drpStatus.SelectedIndex > 0)
        {
            if (drpStatus.SelectedIndex == 1)
                hashtable.Add("@Status", "Active");
            else
                hashtable.Add("@Status", "InActive");
        }
        DataTable dataTable = common.GetDataTable("Host_rptGetAllStud", hashtable);
        lblRecCount.Text = "Total Number of Student: " + dataTable.Rows.Count.ToString();
        CreateReport(dataTable);
        lblreport.Visible = true;
        if (dataTable.Rows.Count > 0)
        {
            btnprint.Visible = true;
            btnExpExcel.Visible = true;
        }
        else
        {
            btnprint.Visible = false;
            btnExpExcel.Visible = false;
        }
        UpdatePanel1.Update();
    }

    private void CreateReport(DataTable dt)
    {
        try
        {
            if (dt.Rows.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table align='center' border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='98%'>");
                stringBuilder.Append("<tr><td colspan='8' class='tbltd'><strong>Total no of Students :" + dt.Rows.Count + "</strong></td></tr>");
                stringBuilder.Append("<tr  style='background-color:#CCCCCC'>");
                stringBuilder.Append("<td  class='tbltxt'><strong>Admission No.</strong></td>");
                stringBuilder.Append("<td  class='tbltxt'><strong>Admn Date</strong></td>");
                stringBuilder.Append("<td  class='tbltxt'><strong>Admn Session Year</strong></td>");
                stringBuilder.Append("<td class='tbltxt'><strong>Class</strong></td>");
                stringBuilder.Append("<td  class='tbltxt'><strong>Name</strong></td>");
                stringBuilder.Append("<td  class='tbltxt'><strong>Gender</strong></td>");
                stringBuilder.Append("<td class='tbltxt'><strong>Status</strong></td>");
                stringBuilder.Append("<td class='tbltxt'><strong>Leaving Date</strong></td>");
                stringBuilder.Append("</tr>");
                foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td>");
                    stringBuilder.Append(row["AdmnNo"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td>");
                    stringBuilder.Append(row["admdate"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td>");
                    stringBuilder.Append(row["SessionYr"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td>");
                    stringBuilder.Append(row["ClassName"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td>");
                    stringBuilder.Append(row["FullName"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td>");
                    stringBuilder.Append(row["sex"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td>");
                    stringBuilder.Append(row["Status"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td>");
                    stringBuilder.Append(row["leavingdt"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("</table>");
                lblreport.Text = stringBuilder.ToString().Trim();
                Session["printData"] = stringBuilder.ToString().Trim();
            }
            else
            {
                lblreport.Text = "<div style='color:Red;  font-weight:bold;  font-size:14px;' class='tbltd'>No records to display for the current filters.</div>";
                Session["printData"] = "";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblreport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("Student Details:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(lblreport.Text.ToString().Trim());
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

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/Hostel Student details" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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

    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptHostAdmnListPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}