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

public partial class Accounts_rptPartyList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();
    private clsStaticDropdowns drp = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        drp.FillCusomerCat(drpCustomerType);
        drpCustomerType.Items.RemoveAt(0);
        drpCustomerType.Items.Insert(0, new ListItem("--ALL --", "0"));
        drp.FillPartyType(drpPartyType);
        BtnExcel.Visible = false;
        btnPrint.Visible = false;
        drpCustomerType.Enabled = false;
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
        if (drpPartyType.SelectedIndex > 0)
            hashtable.Add("@PartyType", drpPartyType.SelectedValue);
        if (drpCustomerType.SelectedIndex > 0)
            hashtable.Add("@CustomerType", drpCustomerType.SelectedValue);
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        DataTable dataTable2 = clsDal.GetDataTable("ACTS_GetPartyList", hashtable);
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
        stringBuilder.Append("<td style='border-right:0px; width:300px;' class='gridtext'><font color='Black'><div style='float:left;'>No Of Records: " + dt.Rows.Count.ToString() + "</div></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td colspan='4' style='border-left:0px;' align='left' class='gridtext'><font color='Black'><b>Party List  (" + drpPartyType.SelectedValue.ToString() + ")</b></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='left'class='gridtext'><strong>Party Name</strong></td>");
        stringBuilder.Append("<td align='left'class='gridtext'><strong>Contact Person</strong></td>");
        stringBuilder.Append("<td align='left'class='gridtext'><strong>Address</strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='left'class='gridtext'><strong>Mobile</strong></td>");
        stringBuilder.Append("<td style='width: 150px' align='left'class='gridtext'><strong>Email</strong></td>");
        if (drpPartyType.SelectedValue == "Customer")
            stringBuilder.Append("<td align='left'class='gridtext'><strong>Customer Type</strong></td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["PartyName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["ContactPerson"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["Address"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["Mobile"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["Email"].ToString().Trim());
            stringBuilder.Append("</td>");
            if (drpPartyType.SelectedValue == "Customer")
            {
                stringBuilder.Append("<td align='left' class='gridtext'>");
                stringBuilder.Append(row["CustomerType"].ToString().Trim());
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["printData"] = stringBuilder.ToString().Trim();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('rptPartyListPrint.aspx');", true);
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
                stringBuilder.Append("Party List:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["printData"].ToString()).ToString().Trim());
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
            Response.AddHeader("content-disposition", "attachment;filename=rptPartyList" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpPartyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        if (drpPartyType.SelectedValue == "Customer")
        {
            drp.FillCusomerCat(drpCustomerType);
            drpCustomerType.Items.RemoveAt(0);
            drpCustomerType.Items.Insert(0, new ListItem("--ALL --", "0"));
            drpCustomerType.Enabled = true;
        }
        else
        {
            drpCustomerType.Enabled = false;
            drpCustomerType.Items.Clear();
        }
    }
}