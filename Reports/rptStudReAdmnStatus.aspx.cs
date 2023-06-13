using ASP;
using Classes.DA;
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

public partial class Reports_rptStudReAdmnStatus : System.Web.UI.Page
{
    private string x;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            fillsession();
            fillclass();
            btnprint.Visible = false;
            btnExpExcel.Visible = false;
        }
        else
            Response.Redirect("~/Login.aspx");
    }

    protected void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID DESC");
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
        drpclass.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    private void GenerateReport()
    {
        DataTable dataTable1 = new DataTable();
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
        DataTable dataTable2;
        if (rbad.Checked)
        {
            if (drpclass.SelectedIndex > 0)
                hashtable.Add("@ClassId", drpclass.SelectedValue);
            dataTable2 = clsDal.GetDataTable("Ps_Sp_Admission_Detail", hashtable);
            x = "New Admission Detail";
        }
        else if (rbread.Checked)
        {
            if (drpclass.SelectedIndex > 0)
                hashtable.Add("@ClassId", drpclass.SelectedValue);
            dataTable2 = clsDal.GetDataTable("Ps_Sp_ReadmissionDone_Detail", hashtable);
            x = "Readmission Done Detail";
        }
        else
        {
            if (drpclass.SelectedIndex > 0)
                hashtable.Add("@ClassId", drpclass.SelectedValue);
            dataTable2 = clsDal.GetDataTable("Ps_Sp_ReadmissionNOTDone_Detail", hashtable);
            x = "Readmission Not Done Detail";
        }
        if (dataTable2.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table align='center' border='1px' cellpadding='1' cellspacing='1' width='100%' style='font-size:14px;background-color: #FFF; border-collapse:collapse;border-color:#ccc;' class='cnt-box2 tbltxt'>");
            //stringBuilder.Append("<tr><td class='tbltxt' colspan='5'> <center>LOYOLA SCHOOL,BHUBANESWAR</center></td></tr>");   
                                 
            stringBuilder.Append("<tr>");           
            stringBuilder.Append("<td colspan='2' style='border-left:0px;' align='center' class='innertbltxt tbltxt'><font color='Black' size=2px><b>Student Admission & Readmission Status</b></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td  style='border-right:0px;' align='left' class='innertbltxt'><font color='Black'><b>For:- " + x + " </b></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td  style='border-left:0px; border-bottom:0px;' align='right' class='innertbltxt'><font color='Black'>No Of Records: " + dataTable2.Rows.Count.ToString() + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table cellpadding='2' cellspacing='0'  width='100%' class='tbltxt'>");
            stringBuilder.Append("<tr style='background-color: #2092d0; color: #fff' class='gridtxt'>");
            stringBuilder.Append("<td align='left' width='13%'><b>Class</b></td>");
            stringBuilder.Append("<td align='left' width='11%'><b>AdmnNo</b></td>");
            stringBuilder.Append("<td align='left' width='12%'><b>AdmnDate</b></td>");
            stringBuilder.Append("<td style='width: 180px' align='left'><b>Name</b></td>");
            stringBuilder.Append("<td style='width: 200px' align='left'><b>Father's Name</b></td>");
            stringBuilder.Append("<td align='left' width='13%'><b>TelNo</b></td>");
            stringBuilder.Append("<td align='left'><b>Gender</b></td>");
            stringBuilder.Append("</tr>");
            string empty = string.Empty;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' class='gridtext'>");
                stringBuilder.Append(row["ClassName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtext'>");
                stringBuilder.Append(row["OldAdmnNo"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtext'>");
                stringBuilder.Append(row["AdmnDate"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtext'>");
                stringBuilder.Append(row["FullName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtext'>");
                stringBuilder.Append(row["FatherName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtext'>");
                stringBuilder.Append(row["TelNoResidence"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtext'>");
                stringBuilder.Append(row["Sex"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["ReadmnStatus"] = stringBuilder.ToString().Trim();
            btnprint.Visible = true;
            btnExpExcel.Visible = true;
        }
        else
        {
            lblReport.Text = "No records found !";
            btnprint.Visible = false;
            btnExpExcel.Visible = false;
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
                stringBuilder.Append("Fee Received:-");
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

    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptStudReAdmnStatusPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void LblReportClear()
    {
        lblReport.Text = "";
        btnprint.Visible = false;
        btnExpExcel.Visible = false;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSession.SelectedIndex > 0)
        {
            LblReportClear();
            fillclass();
            GenerateReport();
        }
        else
        {
            LblReportClear();
            GenerateReport();
        }
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        LblReportClear();
        GenerateReport();
    }
}