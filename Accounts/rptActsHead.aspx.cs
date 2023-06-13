using AjaxControlToolkit;
using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_rptActsHead : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        FillAccountGroupsList();
        btnPrint.Visible = false;
        BtnExcel.Visible = false;
    }

    private void FillAccountGroupsList()
    {
        DataTable dataTable = new clsDAL().GetDataTable("ACTS_GetDrpAccGroup");
        if (dataTable.Rows.Count <= 0)
            return;
        drpAccGroup.Items.Clear();
        drpAccGroup.Items.Add(new ListItem("-All-", "0"));
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            drpAccGroup.Items.Add(new ListItem(row["Ag_Name"].ToString(), row["Ag_Code"].ToString()));
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        GetReportTable();
    }

    private void GetReportTable()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpAccGroup.SelectedIndex > 0)
            hashtable.Add("@AG_Code", drpAccGroup.SelectedValue.ToString().Trim());
        DataTable dataTable2 = clsDal.GetDataTable("ACTS_GetAccountHeadsRpt", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            BtnExcel.Visible = true;
            btnPrint.Visible = true;
            createDefaultersReport(dataTable2);
        }
        else
        {
            lblMsg.Text = "No Record Found.";
            lblMsg.ForeColor = Color.Red;
            lblReport.Text = string.Empty;
            BtnExcel.Visible = false;
            btnPrint.Visible = false;
        }
    }

    private void createDefaultersReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='2' style='border-right:0px;' class='gridtext'><font color='Black'><div style='float:left;'>No Of Records: " + dt.Rows.Count.ToString() + "</div></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td colspan='2' style='border-left:0px;' align='left' class='gridtext'><font color='Black'><b>Account Heads Name</b></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 40px' align='left' class='gridtext'><strong>Sl No</strong></td>");
        stringBuilder.Append("<td align='left'class='gridtext'><strong>Account Heads Name</strong></td>");
        stringBuilder.Append("<td align='left'class='gridtext'><strong>Account Groups Name</strong></td>");
        stringBuilder.Append("<td align='left' style='width: 100px'class='gridtext'><strong>Type</strong></td>");
        stringBuilder.Append("</tr>");
        int num = 1;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='gridtext'>" + num.ToString() + "</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["AcctsHead"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["AG_Name"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["AH_Type"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            ++num;
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["ActsHead"] = stringBuilder.ToString().Trim();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('rptActsHeadPrint.aspx');", true);
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
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("<tr><td align='left'><b>");
                stringBuilder.Append("Account Heads Name :-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["ActsHead"].ToString()).ToString().Trim());
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
            string str = Server.MapPath("Exported_Files/ActsHead" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=rptActsHead" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}