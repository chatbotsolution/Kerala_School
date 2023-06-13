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
using System.Text.RegularExpressions;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_rptStockRecvd : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        FillCategoryList();
        btnPrint.Visible = false;
        BtnExcel.Visible = false;
    }

    private void FillProductList()
    {
        drpItem.DataSource = new clsDAL().GetDataTable("ACTS_GetStockItems", new Hashtable()
    {
      {
         "@CatId",
         drpCat.SelectedValue.ToString().Trim()
      },
      {
         "@SchoolId",
        Session["SchoolId"]
      }
    });
        drpItem.DataTextField = "ItemName";
        drpItem.DataValueField = "ItemCode";
        drpItem.DataBind();
        drpItem.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    private void FillCategoryList()
    {
        DataTable dataTable = new clsDAL().GetDataTable("Proc_DrpGetCat");
        if (dataTable.Rows.Count <= 0)
            return;
        drpCat.Items.Clear();
        drpCat.Items.Add(new ListItem("--ALL--", "0"));
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            drpCat.Items.Add(new ListItem(row["CatName"].ToString(), row["CatId"].ToString()));
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
        if (drpCat.SelectedIndex > 0)
            hashtable.Add("@CatId", drpCat.SelectedValue);
        if (drpItem.SelectedIndex > 0)
            hashtable.Add("@ItemCode", drpItem.SelectedValue);
        if (txtFromDt.Text != "")
            hashtable.Add("@FromDt", (dtpfromdt.GetDateValue().ToString("dd-MMM-yyyy") + " 00:00:00'"));
        if (txtToDt.Text != "")
            hashtable.Add("@ToDt", (dtptodt.GetDateValue().ToString("dd-MMM-yyyy") + " 23:59:59'"));
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        DataTable dataTable2 = clsDal.GetDataTable("ACTS_GetStockRcvList", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            BtnExcel.Visible = true;
            BtnExcel.Enabled = true;
            btnPrint.Enabled = true;
            createDefaultersReport(dataTable2);
        }
        else
        {
            lblMsg.Text = "No Record Found.";
            lblMsg.ForeColor = Color.Red;
            lblReport.Text = string.Empty;
            BtnExcel.Enabled = false;
            btnPrint.Enabled = false;
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
        stringBuilder.Append("<td colspan='2' style='border-left:0px;' align='left' class='gridtext'><font color='Black'><b>Stock Received Details</b></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 100px' align='left'class='gridtext'><strong>Purchase Date</strong></td>");
        stringBuilder.Append("<td align='left'class='gridtext'><strong>Item Name</strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='right'class='gridtext'><strong>MesuringUnit</strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='right'class='gridtext'><strong>Quantity</strong></td>");
        stringBuilder.Append("</tr>");
        double num = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='innertbltxt'>");
            stringBuilder.Append(row["PurchaseDt"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='innertbltxt'>");
            stringBuilder.Append(row["ItemName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='innertbltxt'>");
            stringBuilder.Append(row["MesuringUnit"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='innertbltxt'>");
            stringBuilder.Append(row["quantity"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            num += Convert.ToDouble(row["quantity"].ToString().Trim());
        }
        stringBuilder.Append("<tr><td class='innertbltxt' align='right' style='border-top:0px;' colspan='4'>");
        stringBuilder.Append("<b>Total :&nbsp;&nbsp;&nbsp;" + string.Format("{0:F2}", num) + "</b></td></tr>");
        stringBuilder.Append("</table>");
        btnPrint.Visible = true;
        BtnExcel.Visible = true;
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["printData"] = Regex.Replace(stringBuilder.ToString(), "<a\\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('rptStockRecvdPrint.aspx');", true);
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
                stringBuilder.Append("Stock Received List:-");
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
            Response.AddHeader("content-disposition", "attachment;filename=rptStockRecvd" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillProductList();
    }
}