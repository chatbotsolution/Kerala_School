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
using System.Web.UI.WebControls;

public partial class Inventory_rptInvStockVal : System.Web.UI.Page
{

    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
           Response.Redirect("../../Login.aspx");
        if (Page.IsPostBack)
            return;
       fillLoc();
       btnPrint.Enabled = false;
       btnExpExcel.Visible = true;
       btnExpExcel.Enabled = false;
    }

    private void fillLoc()
    {
       dt = new DataTable();
       dt =obj.GetDataTable("GetAllLoc");
       drpLoc.DataSource = dt;
       drpLoc.DataTextField = "Location";
       drpLoc.DataValueField = "LocationId";
       drpLoc.DataBind();
       drpLoc.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
       GenerateReport();
    }

    private void GenerateReport()
    {
        if (drpLoc.SelectedIndex > 0)
           ht.Add("@LocationId", drpLoc.SelectedValue);
       dt =obj.GetDataTable("rptInvStockVal",ht);
        if (dt.Rows.Count > 0)
        {
            double num = 0.0;
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='4' align='right' class='gridtext'><font color='Black'><strong>No Of Records: " +dt.Rows.Count.ToString() + "</strong></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table cellspacing='0' border='1px' style='border-collapse:collapse;border-color:black;'width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='align='left' class='tblheader'><b>Item</b></td>");
            stringBuilder.Append("<td style='width: 50px' align='left' class='tblheader'><b>Quantity</b></td>");
            stringBuilder.Append("<td style='width: 100px' align='right' class='tblheader'><b>Value</b></td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' class='gridtext'>");
                stringBuilder.Append(row["ItemName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='gridtext'>");
                stringBuilder.Append(row["Qty"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right'>");
                stringBuilder.Append(row["Value"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                num += Convert.ToDouble(row["Value"]);
            }
            stringBuilder.Append("<tr><td align='right' style='border-bottom:0px;' colspan='4'>________________________</td></tr>");
            stringBuilder.Append("<tr><td align='right' style='border-top:0px;' colspan='4'>");
            stringBuilder.Append("<b>Total :&nbsp;&nbsp;&nbsp;" + string.Format("{0:F2}", num) + "</b></td></tr>");
            stringBuilder.Append("</table>");
           Session["PrintData"] = stringBuilder.ToString().Trim();
           lblReport.Text = stringBuilder.ToString().Trim();
           btnPrint.Visible = true;
           btnExpExcel.Visible = true;
           btnPrint.Enabled = true;
           btnExpExcel.Enabled = true;
        }
        else
        {
           Session["printData"] = (lblReport.Text = "<div style='border:solid 1px Black'>No records found !</div>");
           btnPrint.Enabled = false;
           btnExpExcel.Enabled = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
       Response.Redirect("rptInvStockValPrint.aspx");
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
                stringBuilder.Append("<tr><td align='left' colspan='8'><b>");
                stringBuilder.Append("Stock Value Report :-");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(Session["printData"].ToString());
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
            string str =Server.MapPath("Exported_Files/" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
           Response.ClearContent();
           Response.ContentType = "application/vnd.ms-excel";
           Response.AddHeader("content-disposition", "attachment;filename=REPORT_InvStockValue" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
           Response.WriteFile(str);
           Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}