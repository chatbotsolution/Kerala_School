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

public partial class Accounts_rptItemWiseSupplierList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
       FillCategoryList();
       btnPrint.Visible = false;
       BtnExcel.Visible = false;
    }

    private void FillCategoryList()
    {
        DataTable dataTable = new clsDAL().GetDataTable("Proc_DrpGetCat");
        if (dataTable.Rows.Count <= 0)
            return;
       drpCat.Items.Clear();
       drpCat.Items.Add(new ListItem("--Select--", "0"));
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
           drpCat.Items.Add(new ListItem(row["CatName"].ToString(), row["CatId"].ToString()));
    }

    private void BindItemDropdown()
    {
       drpItem.DataSource = (object)new clsDAL().GetDataTable("ACTS_GetSupplierItems", new Hashtable()
    {
      {
        (object) "@CatId",
        (object)drpCat.SelectedValue.ToString().Trim()
      },
      {
        (object) "@SchoolId",
       Session["SchoolId"]
      }
    });
       drpItem.DataTextField = "ItemName";
       drpItem.DataValueField = "ItemCode";
       drpItem.DataBind();
       drpItem.Items.Insert(0, new ListItem("--ALL--", "0"));
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
            hashtable.Add((object)"@CatId", (object)drpCat.SelectedValue);
        if (drpItem.SelectedIndex > 0)
            hashtable.Add((object)"@ItemCode", (object)drpItem.SelectedValue);
        DataTable dataTable2 = clsDal.GetDataTable("ACTS_GetItemWiseSupplierList", hashtable);
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
        if (drpItem.SelectedIndex > 0)
        {
            clsDAL clsDal = new clsDAL();
            string empty = string.Empty;
            string str = clsDal.ExecuteScalarQry("SELECT ItemName+' '+'('+BrandName+')'AS ItemName FROM dbo.SI_ItemMaster I INNER JOIN dbo.SI_BrandMaster B ON B.BrandId=I.BrandId WHERE ItemCode=" +drpItem.SelectedValue.ToString());
            stringBuilder.Append("<td colspan='6' style='border-left:0px;' align='left' class='gridtext'><font color='Black'><b>Supplier Lists Of " + str + " </b></font>");
        }
        else
            stringBuilder.Append("<td colspan='6' style='border-left:0px;' align='left' class='gridtext'><font color='Black'><b>Item Wise Supplier Lists</b></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='left'class='gridtext'><strong>Party Name</strong></td>");
        stringBuilder.Append("<td align='left'class='gridtext'><strong>Address</strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='left'class='gridtext'><strong>Email</strong></td>");
        stringBuilder.Append("<td style='width: 150px' align='left'class='gridtext'><strong>Mobile  No.</strong></td>");
        stringBuilder.Append("<td style='width: 150px' align='left'class='gridtext'><strong>MaxSupply Capacity</strong></td>");
        stringBuilder.Append("<td style='width: 100px' align='left'class='gridtext'><strong>Supply Delay</strong></td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["PartyName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["Address"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["Email"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["Mobile"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["MaxSupplyCapacity"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["SupplyDelay"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
       lblReport.Text = stringBuilder.ToString().Trim();
       Session["printData"] = (object)stringBuilder.ToString().Trim();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "Message", "window.open('rptItemWiseSupplierListPrint.aspx');", true);
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
                stringBuilder.Append("Item Wise Supplier List:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
               ExportToExcel((stringBuilder.ToString().Trim() +Session["printData"].ToString()).ToString().Trim());
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
            string str =Server.MapPath("Exported_Files/" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
           Response.ClearContent();
           Response.ContentType = "application/vnd.ms-excel";
           Response.AddHeader("content-disposition", "attachment;filename=rptItemWiseSupplierList" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
        if (drpCat.SelectedIndex > 0)
           BindItemDropdown();
        else
           drpItem.Items.Clear();
    }
}