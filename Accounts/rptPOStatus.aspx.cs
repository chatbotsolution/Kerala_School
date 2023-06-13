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

public partial class Accounts_rptPOStatus : System.Web.UI.Page
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
        obj = new clsDAL();
        DataTable dataTable = obj.GetDataTable("ACTS_GetPOSSupplier", new Hashtable()
    {
      {
         "@SchoolId",
        Session["SchoolId"]
      }
    });
        if (dataTable.Rows.Count > 0)
        {
            drpSupplier.Items.Clear();
            drpSupplier.Items.Add(new ListItem("--All--", ""));
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                drpSupplier.Items.Add(new ListItem(row["PartyName"].ToString(), row["PartyId"].ToString()));
        }
        else
        {
            drpSupplier.Items.Clear();
            drpSupplier.Items.Add(new ListItem("--No Supplier Available --", ""));
        }
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
        if (txtFrmDt.Text.Trim() != "")
            ht.Add("@FrmDt", (dtpFrmDt.GetDateValue().ToString("dd-MMM-yyyy") + " 00:00:00'"));
        if (txtToDt.Text.Trim() != "")
            ht.Add("@ToDt", (dtpToDt.GetDateValue().ToString("dd-MMM-yyyy") + " 23:59:59'"));
        if (drpSupplier.SelectedValue != "")
            ht.Add("@PartyId", drpSupplier.SelectedValue.ToString().Trim());
        ht.Add("@SchoolId", Session["SchoolId"]);
        dt = obj.GetDataTable("ACTS_GetPurchaseOrders", ht);
        if (dt.Rows.Count > 0)
        {
            CreateHtmlRepotr(dt);
        }
        else
        {
            lblReport.Text = string.Empty;
            lblMsg.Text = "No Records Found";
            lblMsg.ForeColor = Color.Red;
            btnPrint.Visible = false;
            btnExcel.Visible = false;
        }
    }

    private void CreateHtmlRepotr(DataTable dt)
    {
        StringBuilder strGenerateReport = new StringBuilder("");
        strGenerateReport.Append("<table border='0px' cellpadding='0' cellspacing='10' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        strGenerateReport.Append("<tr>");
        strGenerateReport.Append("<td>");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            strGenerateReport.Append("<table border='0px' cellpadding='0' cellspacing='6' width='100%'>");
            strGenerateReport.Append("<tr>");
            strGenerateReport.Append("<td style='width: 110px' align='left'class='gridtext'><strong>Order Id </strong></td>");
            strGenerateReport.Append("<td style='width: 100px' align='left'class='gridtext'><strong>:</strong>&nbsp;");
            strGenerateReport.Append(row["PurOrderId"].ToString().Trim());
            strGenerateReport.Append("</td>");
            strGenerateReport.Append("<td style='width: 100px' align='left'class='gridtext'><strong>Order Date :</strong></td>");
            strGenerateReport.Append("<td  align='left'class='gridtext'>");
            strGenerateReport.Append(row["OrderDate"].ToString().Trim());
            strGenerateReport.Append("</td>");
            strGenerateReport.Append("</tr>");
            strGenerateReport.Append("<tr>");
            strGenerateReport.Append("<td  align='left'class='gridtext'><strong>Supplier Name </strong></td>");
            strGenerateReport.Append("<td align='left'class='gridtext' colspan='3'><strong>:</strong>&nbsp;");
            strGenerateReport.Append(row["PartyName"].ToString().Trim());
            strGenerateReport.Append("</td>");
            strGenerateReport.Append("</tr>");
            strGenerateReport.Append("</table>");
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = obj.GetDataTable("ACTS_GetOrderDetail", new Hashtable()
      {
        {
           "@PurOrderId",
           int.Parse(row["PurOrderId"].ToString().Trim())
        },
        {
           "@SchoolID",
          Session["SchoolId"]
        }
      });
            if (dataTable2.Rows.Count > 0)
            {
                OrderDetail(strGenerateReport, dataTable2);
            }
            else
            {
                strGenerateReport.Append("<table border='0px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
                strGenerateReport.Append("<tr>");
                strGenerateReport.Append("<td style='width: 120px' align='left'class='gridtext'><strong>Not Recieved Yet </strong></td>");
                strGenerateReport.Append("</tr>");
                strGenerateReport.Append("</table>");
            }
        }
        strGenerateReport.Append("</td>");
        strGenerateReport.Append("</tr>");
        strGenerateReport.Append("</table>");
        lblReport.Text = strGenerateReport.ToString().Trim();
        Session["POStatus"] = Regex.Replace(strGenerateReport.ToString(), "<a\\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
        btnExcel.Visible = true;
        btnPrint.Visible = true;
    }

    private static void OrderDetail(StringBuilder strGenerateReport, DataTable dtDet)
    {
        strGenerateReport.Append("<table border='0px' cellpadding='4' cellspacing='0' width='100%'>");
        strGenerateReport.Append("<tr>");
        strGenerateReport.Append("<td align='left'class='gridtext'><strong><u>Item Name</u> </strong></td>");
        strGenerateReport.Append("<td style='width: 150px;' align='left'class='gridtext'><strong><u>Ordered Qty</u> </strong></td>");
        strGenerateReport.Append("<td style='width: 150px;' align='left'class='gridtext'><strong><u>Received Qty </u></strong></td>");
        strGenerateReport.Append("<td style='width: 150px;' align='left'class='gridtext'><strong><u>Balance </u></strong></td>");
        strGenerateReport.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dtDet.Rows)
        {
            strGenerateReport.Append("<tr>");
            strGenerateReport.Append("<td align='left'class='gridtext'>");
            strGenerateReport.Append(row["ItemName"].ToString().Trim());
            strGenerateReport.Append("</td>");
            strGenerateReport.Append("<td align='left'class='gridtext'>");
            strGenerateReport.Append(row["OrderQty"].ToString().Trim());
            strGenerateReport.Append("</td>");
            strGenerateReport.Append("<td align='left'class='gridtext'>");
            strGenerateReport.Append(row["RecvdQty"].ToString().Trim());
            strGenerateReport.Append("</td>");
            strGenerateReport.Append("<td align='left'class='gridtext'>");
            strGenerateReport.Append(row["Balance"].ToString().Trim());
            strGenerateReport.Append("</td>");
            strGenerateReport.Append("</tr>");
        }
        strGenerateReport.Append("<tr>");
        strGenerateReport.Append("<td colspan='4' style='border-bottom:dotted 1px black;'>&nbsp;");
        strGenerateReport.Append("</td>");
        strGenerateReport.Append("</tr>");
        strGenerateReport.Append("</table>");
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptPOStatusPrint.aspx');", true);
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
                ExportToExcel((stringBuilder.ToString().Trim() + Session["POStatus"].ToString()).ToString().Trim());
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
            string str = Server.MapPath("Exported_Files/PurchaseOrderStatus" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=rptPOStatus" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}