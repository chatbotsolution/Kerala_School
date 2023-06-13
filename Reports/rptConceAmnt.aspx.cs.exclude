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

public partial class Reports_rptConceAmnt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        BindClass();
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
    }

    private void BindClass()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        drpForCls.DataSource = clsDal.GetDataTable("Ps_Sp_BindClassDrp");
        drpForCls.DataTextField = "ClassName";
        drpForCls.DataValueField = "ClassId";
        drpForCls.DataBind();
        drpForCls.Items.Insert(0, new ListItem("-All-", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        if (drpForCls.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpForCls.SelectedValue.ToString());
        if (drpStud.SelectedIndex > 0)
            hashtable.Add("@AdmnNo", drpStud.SelectedValue.ToString());
        if (txtFromDt.Text != "")
            hashtable.Add("@FrmDt", dtpFrmDt.GetDateValue().ToString("MM/dd/yyyy"));
        if (txtToDt.Text != "")
            hashtable.Add("@ToDt", dtpToDt.GetDateValue().ToString("MM/dd/yyyy"));
        DataTable dataTable2 = clsDal.GetDataTable("Ps_GetFeeAdjst", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            GenerateReport(dataTable2);
            btnPrint.Enabled = true;
            btnExpExcel.Enabled = true;
        }
        else
        {
            lblReport.Text = "No Record Found  !";
            btnPrint.Enabled = false;
            btnExpExcel.Enabled = false;
        }
    }

    private void GenerateReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;' class='tbltxt'  width='100%'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 110px' align='left' class='tblheader'>Concession Date</td>");
        stringBuilder.Append("<td style='width: 90px' align='left' class='tblheader'>Admission No</td>");
        stringBuilder.Append("<td align='left' class='tblheader'>Student Name</td>");
        stringBuilder.Append("<td style='width: 90px' align='left' class='tblheader'>Reason</td>");
        stringBuilder.Append("<td style='width: 100px' align='right' class='tblheader'>Amount</td>");
        stringBuilder.Append("</tr>");
        double num = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["AdjDateStr"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["AdmnNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["Reason"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd'>");
            string str1 = "<a href='javascript:popUp";
            string str2 = "('rptConceDtl.aspx?AdmnNo=" + row["AdmnNo"].ToString() + " &Dt= " + row["AdjDate"].ToString() + "')";
            string str3 = "'>";
            stringBuilder.Append(str1.Replace('\'', '"') + str2 + str3.Replace('\'', '"'));
            stringBuilder.Append("<b>" + row["Amount"].ToString() + "</b></a>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            num += Convert.ToDouble(row["Amount"].ToString().Trim());
        }
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='4'  align='right'  class='tbltd'>");
        stringBuilder.Append("<strong>Total &nbsp;:&nbsp;</strong>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='right' class='tbltd'>");
        stringBuilder.Append("<strong>" + string.Format("{0:F2}", num));
        stringBuilder.Append("</strong></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["FeeConcess"] = stringBuilder.ToString().Trim();
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
                stringBuilder.Append("Fee Concession Report :-");
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
            string str = Server.MapPath("Exported_Files/FeeConcessionRpt" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptConceAmntPrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpForCls_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillStud();
        lblReport.Text = string.Empty;
    }

    private void FillStud()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpForCls.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpForCls.SelectedValue.Trim());
        drpStud.DataSource = clsDal.GetDataTable("Ps_StudlistForFeeAdjst", hashtable);
        drpStud.DataTextField = "FullName";
        drpStud.DataValueField = "AdmnNo";
        drpStud.DataBind();
        drpStud.Items.Insert(0, new ListItem("-All-", "0"));
    }
}
