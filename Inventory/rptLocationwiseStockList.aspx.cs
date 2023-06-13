using ASP;
using Classes.DA;
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

public partial class Inventory_rptLocationwiseStockList : System.Web.UI.Page
{
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
           Page.Form.DefaultButton =btnShow.UniqueID;
           BindDropDown();
           btnPrint.Enabled = false;
           btnExport.Enabled = false;
        }
        else
           Response.Redirect("../Login.aspx");
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
       GenerateReport();
    }

    private void GenerateReport()
    {
        if (drpItem.SelectedIndex > 0)
           ht.Add("@ItemCode", drpItem.SelectedValue.Trim());
        if (drpLocation.SelectedIndex > 0)
           ht.Add("@LocationId", drpLocation.SelectedValue);
        if (Session["User"].ToString() != "admin")
           ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
       dt =obj.GetDataTable("SI_GetLocationItem",ht);
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            if (drpItem.SelectedIndex > 0)
            {
                stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' class='gridtxt'><font color='Black'>" + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt") + "</font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td colspan='2' align='right' class='gridtxt'><font color='Black'>No Of Records: " +dt.Rows.Count.ToString() + "</font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='width: 200px' align='left' class='tblheader'><b>Location Name</b></td>");
                stringBuilder.Append("<td style='align='left' class='tblheader'><b>Items</b></td>");
                stringBuilder.Append("<td style='width: 100px' align='right' class='tblheader'><b>Quantity</b></td>");
                stringBuilder.Append("</tr>");
                foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' class='gridtxt'>");
                    stringBuilder.Append(row["Location"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='left' class='gridtxt'>");
                    stringBuilder.Append(row["ItemName"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='right' class='gridtxt'>");
                    stringBuilder.Append(row["quantity"].ToString().Trim());
                    stringBuilder.Append("</td></tr>");
                }
                string str1 =dt.Rows[0][2].ToString();
                string str2 =dt.Compute("SUM(Qty)", "").ToString();
                stringBuilder.Append("<tr style='font-weight: bold; text-align: left;'>");
                stringBuilder.Append("<td>&nbsp;</td>");
                stringBuilder.Append("<td align='right'  class='gridtxt'><b>Total Quantity :</b></td>");
                stringBuilder.Append("<td align='right'  class='gridtxt'><b>" + str2.ToString() + " " + str1.ToString() + "</b></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
               lblReport.Text = stringBuilder.ToString().Trim();
               Session["Stock"] = stringBuilder.ToString().Trim();
               btnPrint.Enabled = true;
               btnExport.Enabled = true;
            }
            else
            {
                stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' class='gridtxt'><font color='Black'>" + DateTime.Now.ToString("dddd, dd MMMM yyyy hh:mm tt") + "</font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td colspan='2' align='right' class='gridtxt'><font color='Black'>No Of Records: " +dt.Rows.Count.ToString() + "</font>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td  align='left' class='tblheader'><b>Item Name</b></td>");
                stringBuilder.Append("<td style='width: 90px' align='right' class='tblheader'><b>Consumable</b></td>");
                stringBuilder.Append("<td style='width: 90px' align='right' class='tblheader'><b>Quantity</b></td>");
                stringBuilder.Append("</tr>");
                string str =dt.Rows[0]["Location"].ToString();
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='center' colspan='3' style='font-size:14; font-style:italic;'>" + str + "</td></tr>");
                foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                {
                    if (str != row["Location"].ToString())
                    {
                        str = row["Location"].ToString();
                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td align='center' colspan='3' style='font-size:14; font-style:italic;'>" + str + "</td></tr>");
                    }
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' class='gridtxt'>");
                    stringBuilder.Append(row["ItemName"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='right' class='gridtxt'>");
                    stringBuilder.Append(row["Consume"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='right' class='gridtxt'>");
                    stringBuilder.Append(row["quantity"].ToString().Trim());
                    stringBuilder.Append("</td></tr>");
                }
                stringBuilder.Append("</table>");
               lblReport.Text = stringBuilder.ToString().Trim();
               Session["Stock"] = stringBuilder.ToString().Trim();
               btnPrint.Enabled = true;
               btnExport.Enabled = true;
            }
        }
        else
        {
           Session["Stock"] = (lblReport.Text = "<div style='border:solid 1px Black'>No records found !</div>");
           btnPrint.Enabled = false;
           btnExport.Enabled = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
       Response.Redirect("rptLocationwiseStockListPrint.aspx");
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
           Response.AddHeader("content-disposition", "attachment;filename=REPORT_LocationwiseStock" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
           Response.WriteFile(str);
           Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td align='left' colspan='14'><b>");
                stringBuilder.Append("Stock Report :-");
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

    private void BindDropDown()
    {
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
       drpItem.DataSource = new clsDAL().GetDataTableQry("SELECT  ItemName,ItemCode FROM dbo.SI_ItemMaster ORDER BY ItemName");
       drpItem.DataTextField = "ItemName";
       drpItem.DataValueField = "ItemCode";
       drpItem.DataBind();
       drpItem.Items.Insert(0, new ListItem("--All--", "0"));
       BindLocation();
    }

    private void BindLocation()
    {
        if (Session["User"].ToString() == "admin")
           bindDropDown(drpLocation, "select LocationId, Location from dbo.SI_LocationMaster", "Location", "LocationId");
        else
           bindDropDown(drpLocation, "select LocationId, Location from dbo.SI_LocationMaster WHERE SchoolId=" + Convert.ToInt32(Session["SchoolId"]) + " ", "Location", "LocationId");
    }

    protected void drpLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpLocation.SelectedIndex <= 0)
            return;
       dt = new DataTable();
       obj = new Common();
       ht = new Hashtable();
       ht.Add("@LocationId", drpLocation.SelectedValue.Trim());
       dt =obj.GetDataTable("SI_GetLocationIC",ht);
        if (dt.Rows.Count > 0)
        {
           lblIncharge.Text = "Location Incharge: " +dt.Rows[0]["LocIC"].ToString();
           pnlIncharge.Visible = true;
        }
        else
           lblIncharge.Text = string.Empty;
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = new clsDAL().GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--All--", "0"));
    }
}