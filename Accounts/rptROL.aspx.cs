using AjaxControlToolkit;
using ASP;
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

public partial class Accounts_rptROL : System.Web.UI.Page
{
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        FillDroupDown();
        btnPrint.Visible = false;
        btnExcel.Visible = false;
    }

    private void FillDroupDown()
    {
        DataTable dataTable = new clsDAL().GetDataTable("Proc_DrpGetCat");
        if (dataTable.Rows.Count > 0)
        {
            drpCat.DataSource = dataTable;
            drpCat.DataTextField = "CatName";
            drpCat.DataValueField = "CatId";
            drpCat.DataBind();
            drpCat.Items.Insert(0, new ListItem("--All--", "0"));
        }
        DataTable dataTableQry = new clsDAL().GetDataTableQry("SELECT BrandName,BrandId FROM SI_BrandMaster");
        if (dataTableQry.Rows.Count <= 0)
            return;
        drpBrand.DataSource = dataTableQry;
        drpBrand.DataTextField = "BrandName";
        drpBrand.DataValueField = "BrandId";
        drpBrand.DataBind();
        drpBrand.Items.Insert(0, new ListItem("--All--", "0"));
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    private void GenerateReport()
    {
        obj = new clsDAL();
        dt = new DataTable();
        ht = new Hashtable();
        if (drpCat.SelectedIndex > 0)
            ht.Add("@CatId", drpCat.SelectedValue.ToString().Trim());
        if (drpBrand.SelectedValue != "")
            ht.Add("@BrandId", drpBrand.SelectedValue.ToString().Trim());
        ht.Add("@SchoolId", Session["SchoolId"]);
        dt = obj.GetDataTable("ACTS_GetItemListForROL", ht);
        if (dt.Rows.Count > 0)
        {
            CreateHtmlRepotr(dt);
        }
        else
        {
            lblReport.Text = string.Empty;
            lblMsg.Text = "No Record Found !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void CreateHtmlRepotr(DataTable dt)
    {
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='5' style='border-right:0px;' align='right' class='gridtext'><font color='Black'><b>List of Items Reached ROL</b></font>");
        stringBuilder.Append("<td colspan='1' style='border-left:0px;' class='gridtext'><font color='Black'><div style='float:right;'>No Of Records: " + dt.Rows.Count.ToString() + "</div></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 40px' align='left'  class='gridtext'><b>Sl No</b></td>");
        stringBuilder.Append("<td style='width: 400px' align='left' class='gridtext'><b>Item Name</b></td>");
        stringBuilder.Append("<td style='width: 180px' align='left' class='gridtext'><b>Category Name</b></td>");
        stringBuilder.Append("<td style='width: 180px' align='left' class='gridtext'><b>Brand Name</b></td>");
        stringBuilder.Append("<td style='width: 80px' align='left' class='gridtext'><b>Avl. Qty.</b></td>");
        stringBuilder.Append("<td style='width: 90px' align='left' class='gridtext'><b>MesuringUnit</b></td>");
        stringBuilder.Append("</tr>");
        int num = 1;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='gridtext'>" + num.ToString() + "</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["ItemName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["CatName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["BrandName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["BalQty"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["MesuringUnit"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            ++num;
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["RolItem"] = Regex.Replace(stringBuilder.ToString(), "<a\\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
        btnExcel.Visible = true;
        btnPrint.Visible = true;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptROLPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["RolItem"].ToString()).ToString().Trim());
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
            string str = Server.MapPath("Exported_Files/rptRol" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=rptRol" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}