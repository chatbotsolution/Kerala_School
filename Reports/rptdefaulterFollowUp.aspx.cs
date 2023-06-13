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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptdefaulterFollowUp : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        fillclass();
        fillSession();
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
    }
    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = obj.GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("--All--", "0"));
    }

    private void fillSession()
    {
        DataTable dataTable = new DataTable();
        drpSession.DataSource = obj.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Session", drpSession.SelectedValue.ToString().Trim());
        if (drpclass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpclass.SelectedValue.ToString().Trim());
        if (drpQuarter.SelectedIndex > 0)
        {
            FeeDetails fd = new FeeDetails();
            if (drpQuarter.SelectedValue == "1")
            {
                //hashtable.Add("@Quarter", "APR-JUN");
                hashtable.Add("@TransDate", fd.getDuedate("APR-JUN", drpSession.SelectedValue.ToString().Trim()));
            }
            else if (drpQuarter.SelectedValue == "2")
            {
                // hashtable.Add("@Quarter", "JUL-SEP");
                hashtable.Add("@TransDate", fd.getDuedate("JUL-SEP", drpSession.SelectedValue.ToString().Trim()));
            }
            else if (drpQuarter.SelectedValue == "3")
            {
                // hashtable.Add("@Quarter", "OCT-DEC");
                hashtable.Add("@TransDate", fd.getDuedate("OCT-DEC", drpSession.SelectedValue.ToString().Trim()));
            }
            else
            {
                // hashtable.Add("@Quarter", "JAN-MAR");
                hashtable.Add("@TransDate", fd.getDuedate("JAN-MAR", drpSession.SelectedValue.ToString().Trim()));
            }
        }
        // DataTable dt = drpclass.SelectedIndex <= 0 ? obj.GetDataTable("NewGetAllQtrlyFeeStatus", hashtable) : obj.GetDataTable("NewQuarterlyFeeStatus", hashtable);
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        // ds = obj.GetDataSet("NewDefaulterList", hashtable);
         dt = obj.GetDataTable("NewDefaulterList", hashtable);
      //  ds = obj.GetDataSet("DefaulterFollowUp", hashtable);
        //-------------------------------------------------------
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table id='reporttable' border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
        stringBuilder.Append("<tr  style='background-color:#CCCCCC'>");
        stringBuilder.Append("<td style='width: 30px' align='left'><strong>Sl.No</strong></td>");
        stringBuilder.Append("<td style='width: 30px' align='left'><strong>Roll.No</strong></td>");
        stringBuilder.Append("<td style='width: 60px' align='left'>Admission No</td>");
        stringBuilder.Append("<td style='width: 150px' align='left'>Name</td>");
        stringBuilder.Append("<td style='width: 40px' align='left'>Class</td>");
        stringBuilder.Append("<td style='width: 30px' align='left'>Section</td>");
        stringBuilder.Append("<td style='width: 30px' align='left'>Status</td>");
        stringBuilder.Append("<td style='width: 100px' align='left'>MobileNo</td>");
        //------------------------
        stringBuilder.Append("</tr>");
        //---------------------
           int num2 = 1;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center'>" + num2.ToString() + "</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["RollNo"].ToString().Trim());
            //stringBuilder.Append("&nbsp;");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["OldAdmnno"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["ClassName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Section"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Status"].ToString().Trim());
            //stringBuilder.Append("&nbsp;");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["MobileNo1"].ToString().Trim());
           // stringBuilder.Append("&nbsp;");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");

            ++num2;
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["DefalterFollow"] = stringBuilder.ToString().Trim();
        btnPrint.Enabled = true;
        btnExpExcel.Enabled = true;
    }
   
    
    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/DefaulterFollowUp_" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
                stringBuilder.Append("Defaulter Follow Up :-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["DefalterFollow"].ToString()).ToString().Trim());
            }
            else
                Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
  
}