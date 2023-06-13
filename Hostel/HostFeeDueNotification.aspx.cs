using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Hostel_HostFeeDueNotification : System.Web.UI.Page
{
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        btnPrint.Enabled = false;
        fillclass();
        txtWrite.Text = "Dear Parents,\r\rYou are requested to pay the hostel dues of your ward by " + DateTime.Today.AddMonths(1).ToString("dd MMM yyyy") + " . Ignore if already paid.";
        pcalTillDate.SetDateValue(DateTime.Now);
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    private void GenerateReport()
    {
        if (drpForCls.SelectedIndex > 0)
            ht.Add("@ClassID", drpForCls.SelectedValue);
        if (rbtnFullSess.Checked)
            ht.Add("@FullSess", 1);
        else
            ht.Add("@FullSess", 0);
        if (drpFeeType.SelectedIndex > 0)
            ht.Add("@Type", drpFeeType.SelectedValue.Trim());
        ht.Add("@ToDate", pcalTillDate.GetDateValue().ToString("MM/dd/yyyy"));
        dt = obj.GetDataTable("HostRptDefaultersForNotification", ht);
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<table border='0' cellpadding='0' cellspacing='0'  width='650px'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' class='tbltxt' colspan='4'>");
                stringBuilder.Append(txtWrite.Text.ToString().Replace("\r\n", "</br>"));
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' class='tbltxt'><b> Details:-&nbsp;</b></b>");
                stringBuilder.Append("</td>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<td align='left' class='tbltxt'><b>Admission No:&nbsp;</b>");
                stringBuilder.Append(row["AdmnNo"].ToString().Trim());
                stringBuilder.Append("<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Student Name:&nbsp;</b>");
                stringBuilder.Append(row["FullName"].ToString().Trim());
                stringBuilder.Append("<b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Class:&nbsp;</b>");
                stringBuilder.Append(row["ClassName"].ToString().Trim());
                stringBuilder.Append("<br/><br/><b>School Fee Dues: Rs.&nbsp;</b>");
                stringBuilder.Append(row["RegularFeeDue"].ToString().Trim());
                stringBuilder.Append("<br/></td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td>&nbsp;");
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                stringBuilder.Append("<hr />");
                stringBuilder.Append("</br>");
            }
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["printDue"] = stringBuilder.ToString().Trim();
            btnPrint.Enabled = true;
        }
        else
        {
            Session["printDue"] = (lblReport.Text = "<div style='border:solid 1px Black'>No records found !</div>");
            btnPrint.Enabled = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("HostFeeDueNotificationPrint.aspx");
    }

    private void fillclass()
    {
        drpForCls.Items.Clear();
        drpForCls.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpForCls.DataTextField = "classname";
        drpForCls.DataValueField = "classid";
        drpForCls.DataBind();
        drpForCls.Items.Insert(0, new ListItem("Select", "0"));
    }

    protected void drpForCls_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
    }
}