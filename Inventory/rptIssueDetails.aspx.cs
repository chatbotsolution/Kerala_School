using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Inventory_rptIssueDetails : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        Page.Form.DefaultButton = btnShow.UniqueID;
        fillLoc();
        btnPrint.Enabled = false;
        btnExpExcel.Visible = true;
        btnExpExcel.Enabled = false;
    }

    private void fillLoc()
    {
        dt = new DataTable();
        Hashtable ht = new Hashtable();
        if (Session["User"].ToString() != "admin")
            ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
        dt = obj.GetDataTable("SI_GetAllLoc", ht);
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
        if (txtFromDate.Text != "")
            ht.Add("@FromDate", pcalFromDate.GetDateValue());
        if (txtToDate.Text != "")
            ht.Add("@Todate", pcalToDate.GetDateValue());
        if (Session["User"].ToString() != "admin")
            ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
        dt = obj.GetDataTable("SI_RptIssueDetails", ht);
        DataSet ds = new DataSet();
        ds = obj.GetDataSet("SI_RptIssueDetails", ht);
        
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='3' align='right' class='gridtext'><font color='Black'><strong>No Of Records: " + dt.Rows.Count.ToString() + "</strong></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("<table border='1' cellpadding='2' cellspacing='0'  width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 90px' align='left' class='tblheader'><b>Issue Date</b></td>");
            stringBuilder.Append("<td style='align='left' class='tblheader'><b>Issued By</b></td>");
            stringBuilder.Append("<td style='align='left' class='tblheader'><b>Received By</b></td>");
            stringBuilder.Append("<td style='align='left' class='tblheader'><b>Item Name</b></td>");
            stringBuilder.Append("<td align='right' class='tblheader'><b>Qty</b></td>");
            stringBuilder.Append("</tr>");
            string str = dt.Rows[0]["IssueId"].ToString();
            int num = 0;
            int index1 = 0;
            for (int index2 = 0; index2 < dt.Rows.Count; ++index2)
            {
                if (str == dt.Rows[index2]["IssueId"].ToString())
                {
                    ++num;
                }
                else
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td align='left' class='gridtxt' rowspan=" + num + ">");
                    stringBuilder.Append(dt.Rows[index1]["IssueDateStr"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='left' class='gridtxt' rowspan=" + num + ">");
                    stringBuilder.Append(dt.Rows[index1]["IssuedBy"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td align='left' class='gridtxt' rowspan=" + num + ">");
                    stringBuilder.Append(dt.Rows[index1]["IssuedTo"].ToString());
                    stringBuilder.Append("</td>");
                    for (; index1 < dt.Rows.Count && num > 0; --num)
                    {
                        stringBuilder.Append("<td align='left' class='gridtxt'>");
                        stringBuilder.Append(ds.Tables[1].Rows[index1]["ItemName"].ToString().Trim());
                        stringBuilder.Append("</td>");
                        stringBuilder.Append("<td align='right' class='gridtxt'>");
                        stringBuilder.Append(ds.Tables[1].Rows[index1]["Qty"].ToString().Trim());
                        stringBuilder.Append("</td>");
                        stringBuilder.Append("</tr>");
                        ++index1;
                    }
                    str = dt.Rows[index2]["IssueId"].ToString();
                    num = 1;
                }
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='gridtxt' rowspan=" + num + ">");
            stringBuilder.Append(dt.Rows[index1]["IssueDateStr"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtxt' rowspan=" + num + ">");
            stringBuilder.Append(dt.Rows[index1]["IssuedBy"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtxt' rowspan=" + num + ">");
            stringBuilder.Append(dt.Rows[index1]["IssuedTo"].ToString());
            stringBuilder.Append("</td>");
            for (; index1 < dt.Rows.Count && num > 0; --num)
            {
                stringBuilder.Append("<td align='left' class='gridtxt'>");
                stringBuilder.Append(ds.Tables[1].Rows[index1]["ItemName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' class='gridtxt'>");
                stringBuilder.Append(ds.Tables[1].Rows[index1]["Qty"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                ++index1;
            }
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["printData"] = Regex.Replace(stringBuilder.ToString(), "<a\\b[^>]+>([^<]*(?:(?!</a)<[^<]*)*)</a>", "$1");
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
        Response.Redirect("rptIssueDetailsPrint.aspx");
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
                stringBuilder.Append("Issue Details Report :-");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(Session["printData"].ToString().Trim());
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
            string str = Server.MapPath("Exported_Files/" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=REPORT_IssueDetails" + DateTime.Now.ToString("ddMMMyyyy_hh-mmtt") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}