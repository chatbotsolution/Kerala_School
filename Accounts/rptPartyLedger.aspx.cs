using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
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

public partial class Accounts_rptPartyLedger : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        bindPartyType();
        BtnExcel.Visible = false;
        btnPrint.Visible = false;
    }

    private void bindPartyType()
    {
        new clsStaticDropdowns().FillPartyType(drpPartyType);
    }

    private void bindPrty()
    {
        clsDAL clsDal = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select a.AcctsHeadId,a.AcctsHead from dbo.ACTS_AccountHeads a ");
        stringBuilder.Append("inner join dbo.ACTS_PartyMaster p on p.AcctsHeadId=a.AcctsHeadId ");
        stringBuilder.Append("where p.PartyType='" + drpPartyType.SelectedValue + "' order by AcctsHead");
        drpParty.DataSource = clsDal.GetDataTableQry(stringBuilder.ToString().Trim());
        drpParty.DataTextField = "AcctsHead";
        drpParty.DataValueField = "AcctsHeadId";
        drpParty.DataBind();
        drpParty.Items.Insert(0, new ListItem("--Select--", "0"));
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
        if (rbnAmount.SelectedValue == "")
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnView, btnView.GetType(), "Message", "alert('Please select  Debit/Credit !');", true);
        }
        else
        {
            if (drpPartyType.SelectedIndex > 0)
                hashtable.Add("@PartyType", drpPartyType.SelectedValue.Trim().ToString());
            if (drpParty.SelectedIndex > 0)
                hashtable.Add("@AcctsHeadId", drpParty.SelectedValue.Trim().ToString());
            if (txtFromDt.Text != "")
                hashtable.Add("@FromDt", dtpfromdt.GetDateValue().ToString("MM/dd/yyyy 00:00:00"));
            if (txtToDt.Text != "")
                hashtable.Add("@ToDt", dtptodt.GetDateValue().ToString("MM/dd/yyyy 23:59:59"));
            hashtable.Add("@SchoolId", Session["SchoolId"]);
            DataTable dataTable2 = clsDal.GetDataTable("ACTS_GetPartyLedger", hashtable);
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
    }

    private void createDefaultersReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        obj = new clsDAL();
        string str1 = obj.ExecuteScalarQry("select PartyName from dbo.ACTS_PartyMaster where PartyId='" + drpParty.SelectedValue + "'");
        string str2 = rbnAmount.SelectedValue.ToString();
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='border-right:0px;' class='gridtext'><font color='Black'><div style='float:left;'>No Of Records: " + dt.Rows.Count.ToString() + "</div></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td colspan='2' style='border-left:0px;' align='left' class='gridtext'><font color='Black'><b>Ledger Details Of " + str1 + " For " + str2 + " Amount</b></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 150px' align='left'class='innertbltxt'><strong>Transaction Date</strong></td>");
        stringBuilder.Append("<td align='left'class='innertbltxt'><strong>Description</strong></td>");
        stringBuilder.Append("<td style='width: 120px' align='right'class='innertbltxt'><strong>Amount</strong></td>");
        stringBuilder.Append("</tr>");
        double num = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='innertbltxt'>");
            stringBuilder.Append(row["TransDate"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='innertbltxt'>");
            stringBuilder.Append(row["Descript"].ToString().Trim());
            stringBuilder.Append("</td>");
            if (rbnAmount.SelectedValue == "Credit")
            {
                stringBuilder.Append("<td align='right' class='innertbltxt'>");
                stringBuilder.Append(row["CrAmnt"].ToString().Trim());
                stringBuilder.Append("</td>");
                num += Convert.ToDouble(row["CrAmnt"].ToString().Trim());
            }
            else if (rbnAmount.SelectedValue == "Debit")
            {
                stringBuilder.Append("<td align='right' class='innertbltxt'>");
                stringBuilder.Append(row["DrAmnt"].ToString().Trim());
                stringBuilder.Append("</td>");
                num += Convert.ToDouble(row["DrAmnt"].ToString().Trim());
            }
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("<tr><td class='innertbltxt' align='right' style='border-top:0px;' colspan='3'>");
        stringBuilder.Append("<b>Total :&nbsp;&nbsp;&nbsp;" + string.Format("{0:F2}", num) + "</b></td></tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["printData"] = stringBuilder.ToString().Trim();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('rptPartyLedgerPrint.aspx');", true);
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
                stringBuilder.Append("Party Ledger List:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["printData"].ToString().Trim()).ToString().Trim());
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
            Response.AddHeader("content-disposition", "attachment;filename=rptPartyLedger" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
        if (drpPartyType.SelectedIndex > 0)
            bindPrty();
        else
            drpParty.Items.Clear();
    }
}