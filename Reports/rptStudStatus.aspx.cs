using ASP;
using SanLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptStudStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            fillSession();
            fillClass();
            btnExpExcel.Enabled = false;
            btnPrint.Enabled = false;
        }
        else
            Response.Redirect("~/Login.aspx");
    }

    protected void fillSession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillClass()
    {
        drpClass.Items.Clear();
        drpClass.DataSource = new clsDAL().GetDataTable("ps_sp_get_classesForDDL").DefaultView;
        drpClass.DataTextField = "classname";
        drpClass.DataValueField = "classid";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("-All-", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal = new clsDAL();
        int num1 = Convert.ToInt32(drpSession.SelectedValue.ToString().Substring(0, 4)) - 1;
        int num2 = Convert.ToInt32(((IEnumerable<string>)drpSession.SelectedValue.ToString().Split('-')).Last<string>()) - 1;
        hashtable.Add("@SessionYr", (num1.ToString() + "-" + num2.ToString()));
        if (drpClass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpClass.SelectedValue.Trim());
        hashtable.Add("@Status", drpStatus.SelectedValue.Trim());
        DataTable dataTable2 = clsDal.GetDataTable("Ps_Sp_GetStudStatus", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            GenerateReport(dataTable2);
        }
        else
        {
            lblReport.Text = "<div style='border:solid 1px #eee'>No records found !</div>";
            btnExpExcel.Enabled = false;
            btnPrint.Enabled = false;
        }
    }

    private void GenerateReport(DataTable dt)
    {
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:#eee;' class='tbltxt'  width='100%'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='center' colspan='2'><b>" + drpStatus.SelectedItem.ToString() + " Student Status Report For Class " + drpClass.SelectedItem.ToString() + " In The Session Year " + drpSession.SelectedItem.ToString() + "</b></td>");
        stringBuilder.Append("<td colspan='2' align='right' style='border-left:0px;'><font color='Black'>No Of Records: " + dt.Rows.Count.ToString() + "</font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:#eee;' class='tbltxt'  width='100%'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 40px;' align='Center'><b>Roll No</b></td>");
        stringBuilder.Append("<td style='width: 70px;' align='left'><b>Admn No.</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left'><b>Student Name</b></td>");
       
        stringBuilder.Append("<td style='width: 70px;' align='left'><b>Class</b></td>");
        stringBuilder.Append("<td style='width: 70px;' align='left'><b>Section</b></td>");
        //stringBuilder.Append("<td  style='width: 70px' align='left'><b>Telephone No.</b></td>");
        //stringBuilder.Append("<td  style='width: 100px' align='left'><b>Present Address</b></td>");
        stringBuilder.Append("<td style='width: 70px;' align='left'><b>Status</b></td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center'>");
            stringBuilder.Append(row["RollNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["OldAdmnNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
           
            stringBuilder.Append("<td align='center'>");
            stringBuilder.Append(row["ClassName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center'>");
            stringBuilder.Append(row["Section"].ToString().Trim());
            stringBuilder.Append("</td>");
            //stringBuilder.Append("<td align='left'>");
            //stringBuilder.Append(row["Telephone"].ToString().Trim());
            //stringBuilder.Append("</td>");
            //stringBuilder.Append("<td align='left'>");
            //stringBuilder.Append(row["Address"].ToString().Trim());
            //stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Status"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["StudStatus"] = stringBuilder.ToString().Trim();
        btnExpExcel.Enabled = true;
        btnPrint.Enabled = true;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptStudStatusPrint.aspx");
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
                stringBuilder.Append("Student Status Report :- ");
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

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/StudentStatusReport" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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