using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Reports_rptAdvanceLedger : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        fillclass();
        fillsession();
        lblReport.Text = "";
        FillClassSection();
        fillstudent();
        btnPrint.Visible = false;
        filladminno();
    }

    protected void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select distinct SessionYear from dbo.PS_ClasswiseStudent order by SessionYear");
        drpSession.DataTextField = "SessionYear";
        drpSession.DataValueField = "SessionYear";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        drpClasses.Items.Clear();
        drpClasses.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpClasses.DataTextField = "classname";
        drpClasses.DataValueField = "classid";
        drpClasses.DataBind();
        drpClasses.Items.Insert(0, new ListItem("All", "0"));
    }

    private void FillClassSection()
    {
        drpSection.Items.Clear();
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "@classId",
         drpClasses.SelectedValue.ToString().Trim()
      },
      {
         "@session",
         drpSession.SelectedValue.ToString().Trim()
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("All", "0"));
    }

    private void fillstudent()
    {
        Common common = new Common();
        DataTable dataTable;
        if (drpSection.SelectedIndex != 0)
            dataTable = common.ExecuteSql("select * from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.classid=" + drpClasses.SelectedValue + " and cs.Section='" + drpSection.SelectedValue + "' order by s.fullname");
        else
            dataTable = drpClasses.SelectedIndex == 0 ? (drpadminno.SelectedIndex <= 0 ? common.ExecuteSql("select * from  ps_studmaster order by fullname") : common.ExecuteSql("select * from ps_studmaster where admnno=" + drpadminno.SelectedValue + " order by fullname")) : common.ExecuteSql("select * from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.classid=" + drpClasses.SelectedValue + " order by fullname");
        drpstudent.DataSource = dataTable;
        drpstudent.DataTextField = "fullname";
        drpstudent.DataValueField = "admnno";
        drpstudent.DataBind();
        drpstudent.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    protected void filladminno()
    {
        Common common = new Common();
        DataTable dataTable;
        if (drpSection.SelectedIndex != 0)
            dataTable = common.ExecuteSql("select * from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.classid=" + drpClasses.SelectedValue + " and cs.Section='" + drpSection.SelectedValue + "' order by cs.admnno");
        else
            dataTable = drpClasses.SelectedIndex == 0 ? (drpstudent.SelectedIndex == 0 ? common.ExecuteSql("select * from  ps_studmaster order by admnno") : common.ExecuteSql("select * from ps_studmaster where admnno=" + drpstudent.SelectedValue + " order by admnno")) : common.ExecuteSql("select * from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.classid=" + drpClasses.SelectedValue + " order by cs.admnno");
        drpadminno.DataSource = dataTable;
        drpadminno.DataTextField = "admnno";
        drpadminno.DataValueField = "admnno";
        drpadminno.DataBind();
        drpadminno.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
        lblReport.Text = "";
        btnPrint.Visible = false;
    }

    protected void drpClasses_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
        filladminno();
        fillstudent();
        lblReport.Text = "";
        btnPrint.Visible = false;
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = "";
        filladminno();
        fillstudent();
        btnPrint.Visible = false;
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = "";
        btnPrint.Visible = false;
        filladminno();
        drpadminno.SelectedValue = drpstudent.SelectedValue;
    }

    protected void drpadminno_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = "";
        btnPrint.Visible = false;
        fillstudent();
        drpstudent.SelectedValue = drpadminno.SelectedValue;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Common common = new Common();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(" select *,convert(Decimal(18,2),sc.FeeCredit) as credit, convert(Decimal(18,2),sc.AdditinalFeeCredit) as additionalcredit ,s.FullName,c.ClassName ");
        stringBuilder.Append(" from PS_StudCreditLedger sc inner join PS_StudMaster s on sc.AdmnNo=s.AdmnNo inner join PS_ClasswiseStudent cs on sc.AdmnNo=cs.admnno");
        stringBuilder.Append(" inner join PS_ClassMaster c on cs.ClassID=c.ClassID where cs.SessionYear='" + drpSession.SelectedItem.ToString() + "'");
        if (drpClasses.SelectedIndex != 0)
            stringBuilder.Append(" and cs.ClassID =" + drpClasses.SelectedValue);
        if (drpSection.SelectedIndex != 0)
            stringBuilder.Append(" and cs.Section='" + drpSection.SelectedValue + "'");
        if (drpstudent.SelectedIndex != 0)
            stringBuilder.Append(" and sc.AdmnNo=" + drpstudent.SelectedValue);
        stringBuilder.Append(" and sc.FeeCredit >0 or sc.AdditinalFeeCredit >0");
        stringBuilder.Append(" order by cs.ClassID,s.FullName");
        DataTable dt = common.ExecuteSql(stringBuilder.ToString());
        if (dt.Rows.Count > 0)
            createRemarkReport(dt);
        else
            lblReport.Text = "<div style='width:700px;background-color:#70cab9;color:Black;border:solid 1px Black;padding:2px;'>No records to display for the current filters.</div>";
    }

    private void createRemarkReport(DataTable dt)
    {
        if (dt.Rows.Count > 0)
        {
            btnPrint.Visible = true;
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table cellpadding='1' cellspacing='1'  width='100%' style='background-color:#CCC;' >");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 80px; text-align:left;' class='tblheader'>Class</td>");
            stringBuilder.Append("<td style='width: 100px; text-align:left;' class='tblheader'>Admission No</td>");
            stringBuilder.Append("<td style='text-align:left;' class='tblheader'>Name</td>");
            stringBuilder.Append("<td style='width: 100px; text-align:right;' class='tblheader'>Fee Credit</td>");
            stringBuilder.Append("<td style='width: 120px; text-align:right;' class='tblheader'>Additinal Fee Credit</td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left' class='tbltd'>");
                stringBuilder.Append(row["ClassName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='tbltd'>");
                stringBuilder.Append(row["AdmnNo"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='tbltd'>");
                stringBuilder.Append(row["FullName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' class='tbltd'>");
                stringBuilder.Append(row["credit"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' class='tbltd'>");
                stringBuilder.Append(row["additionalcredit"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["printData"] = stringBuilder.ToString().Trim();
        }
        else
            lblReport.Text = "<div style='width:700px;background-color:#70cab9;color:Black;border:solid 1px Black;padding:2px;'>No records to display for the current filters.</div>";
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptAdvanceLedgerPrint.aspx");
    }
}