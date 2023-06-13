using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptSiblingDtl : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        fillclass();
        btnPrint.Enabled = false;
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = new clsDAL().GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("Select", "0"));
    }

    private void FillClassSection()
    {
        ddlSection.Items.Clear();
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@classId", drpclass.SelectedValue.ToString().Trim());
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        hashtable.Add("@session", clsGenerateFee.CreateCurrSession());
        ddlSection.DataSource = clsDal.GetDataTable("ps_sp_get_ClassSections", hashtable);
        ddlSection.DataTextField = "Section";
        ddlSection.DataValueField = "Section";
        ddlSection.DataBind();
        ddlSection.Items.Insert(0, new ListItem("All", "0"));
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GetData();
    }

    private void GetData()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpclass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpclass.SelectedValue.ToString().Trim());
        if (ddlSection.SelectedIndex > 0)
            hashtable.Add("@Section", ddlSection.SelectedValue.ToString().Trim());
        DataTable dataTable = clsDal.GetDataTable("Ps_GetSiblings", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            GenerateReport(dataTable);
            btnPrint.Enabled = true;
            lblGrdMsg.Text = string.Empty;
        }
        else
        {
            lblReport.Text = string.Empty;
            lblGrdMsg.Text = "No Record Found.";
            btnPrint.Enabled = false;
        }
    }

    private void GenerateReport(DataTable dt)
    {
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
        stringBuilder.Append("<tr  style='background-color:#CCCCCC'>");
        stringBuilder.Append("<td style='width: 100px' align='left'>Admn. No.</td>");
        stringBuilder.Append("<td style='width: 150px' align='left'>Student Name</td>");
        stringBuilder.Append("<td style='width: 60px' align='left'>Sibling Name (Class)</td>");
        stringBuilder.Append("</tr>");
        string str = string.Empty;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            if (row["AdmnNo"].ToString() != str)
            {
                stringBuilder.Append("<td align='left' rowspan='" + Convert.ToInt32(row["slno"].ToString().Trim()) + "'>");
                stringBuilder.Append(row["AdmnNo"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' rowspan='" + Convert.ToInt32(row["slno"].ToString().Trim()) + "'>");
                stringBuilder.Append(row["StudName"].ToString().Trim());
                stringBuilder.Append("</td>");
            }
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["SiblingName"].ToString().Trim());
            stringBuilder.Append("</td>");
            str = row["AdmnNo"].ToString().Trim();
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["Sibling"] = stringBuilder.ToString().Trim();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptSiblingDtlPrint.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}