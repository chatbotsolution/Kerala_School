using ASP;
using RJS.Web.WebControl;
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

public partial class Reports_rptBusHostelCollection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        Page.Form.DefaultButton = btnShow.UniqueID;
        fillSession();
        fillClass();
        btnExpExcel.Enabled = false;
        btnPrint.Enabled = false;
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
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
    }

    protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal = new clsDAL();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.Trim());
        hashtable.Add("@ClassID", drpClass.SelectedValue.Trim());
        if (txtFromDt.Text != "")
            hashtable.Add("@FrmDt", dtpfrmDt.GetDateValue().ToString("MM/dd/yyyy"));
        if (txtToDt.Text != "")
            hashtable.Add("@ToDt", dtpToDt.GetDateValue().ToString("MM/dd/yyyy"));
        hashtable.Add("@SchoolID", Session["SchoolId"].ToString());
        if (rBtnBus.Checked.Equals(true))
            hashtable.Add("@Ad_Id", 1);
        else
            hashtable.Add("@Ad_Id", 2);
        GenerateReport(clsDal.GetDataTable("Ps_Sp_GetBusHostelFeePaid", hashtable));
    }

    private void GenerateReport(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='80%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' style='border-right:0px;'><font color='Black'>" + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt") + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td colspan='2' align='right' style='border-left:0px;'><font color='Black'>No Of Records: " + dt.Rows.Count.ToString() + "</font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='80%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 150px' align='left'><b>Student Name</b></td>");
            stringBuilder.Append("<td  align='left'><b>Description</b></td>");
            stringBuilder.Append("<td  style='width: 90px' align='right'><b>Amount</b></td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["FullName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left'>");
                stringBuilder.Append(row["TransDesc"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right'>");
                stringBuilder.Append(row["credit"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            string str = dt.Compute("SUM(credit)", "").ToString();
            stringBuilder.Append("<tr style='font-weight: bold; text-align: left; background-color: Black;color:White;'>");
            stringBuilder.Append("<td align='Left' style='Font-Size:14px;' colspan='2'><b>Grand Total :</b></td>");
            stringBuilder.Append("<td align='right' ><b>" + str.ToString() + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["BusHostel"] = stringBuilder.ToString().Trim();
            btnExpExcel.Enabled = true;
            btnPrint.Enabled = true;
        }
        else
        {
            lblReport.Text = "<div style='border:solid 1px Black'>No records found !</div>";
            btnExpExcel.Enabled = false;
            btnPrint.Enabled = false;
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
                stringBuilder.Append("Bus/Hostel Collection Report :- ");
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
            string str = Server.MapPath("Exported_Files/BusHostelFeeCollection" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptBusHostelPaidStatusPrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}
