using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptAdditionalfee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        fillsession();
        fillclass();
        FillClassSection();
        btnPrint.Visible = false;
    }

    protected void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
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

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
        lblReport.Text = "";
        btnPrint.Visible = false;
        txtFromDate.Text = "";
        txtToDate.Text = "";
    }

    protected void drpClasses_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
        lblReport.Text = "";
        btnPrint.Visible = true;
    }

    private void createRemarkReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='0'  width='70%'>");
        stringBuilder.Append("<tr>");
        if (!(drpClasses.SelectedItem.ToString() == "ALL"))
            dt.Rows[0]["ClassName"].ToString().Trim();
        stringBuilder.Append("<td align='left'>");
        stringBuilder.Append("<strong><u>Class:</u></strong> " + drpClasses.SelectedItem.ToString());
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='left' colspan='2'>");
        stringBuilder.Append("<strong><u>Session:</u></strong>" + dt.Rows[0]["Session"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table cellpadding='1' cellspacing='0'  width='70%'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td> &nbsp;");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Receiving Date</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Transcation Date</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Admission No</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Name</u></strong></td>");
        stringBuilder.Append("<td style='width: 110px' align='left'><strong><u>Description</u></strong></td>");
        stringBuilder.Append("<td style='width: 80px' align='right'><strong><u>Amount</u></strong></td>");
        stringBuilder.Append("</tr>");
        double num = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["UsersDate"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["RecvDate"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["AdmnNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Description"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' style='width: 80px'>");
            stringBuilder.Append(row["Amount"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            num += Convert.ToDouble(row["Amount"].ToString().Trim());
        }
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='right' colspan='6'");
        stringBuilder.Append("<strong>-----------------</strong>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='right' colspan='6'");
        stringBuilder.Append("<strong>Total:</strong>" + string.Format("{0:F2}", num));
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["printData"] = stringBuilder.ToString().Trim();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        StringBuilder stringBuilder = new StringBuilder();
        string str1 = pcalFromDate.GetDateValue().ToString("MM/dd/yyyy");
        string str2 = pcalToDate.GetDateValue().ToString("MM/dd/yyyy");
        string str3 = "04/01/" + new clsGenerateFee().CreateCurrSession().Substring(0, 4);
        stringBuilder.Append("select r.recvdate as RecvDt,Convert(varchar,r.RecvDate,103)as RecvDate,Convert(varchar,r.UserDate,103)as UsersDate,r.Description,s.AdmnNo, s.FullName,");
        stringBuilder.Append("Convert(Decimal(18,2),isnull(sum(balance),0) as Amount, r.Session, c.ClassName , cs.Section ");
        stringBuilder.Append("from PS_AdFeeLedger r inner join PS_StudMaster s on r.admnno=s.admnno ");
        stringBuilder.Append("inner join PS_ClasswiseStudent cs on r.admnno=cs.admnno and r.class=cs.classid inner join PS_ClassMaster c on cs.classid=c.classid where ");
        if (txtFromDate.Text != "")
            stringBuilder.Append("r.RecvDate >= '" + str1 + "'");
        else
            stringBuilder.Append("r.RecvDate >= '" + str3 + "'");
        if (txtToDate.Text != "")
            stringBuilder.Append(" and r.RecvDate <= '" + str2 + "'");
        else
            stringBuilder.Append(" and r.RecvDate <= '" + DateTime.Today.ToString("MM/dd/yyyy") + "'");
        if (pcalFromDate.GetDateValue() > pcalToDate.GetDateValue())
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('From date should be smaller than to date');", true);
        }
        else
        {
            if (drpSession.SelectedValue.ToString() != "0")
                stringBuilder.Append(" and r.Session= '" + drpSession.SelectedValue.ToString() + "'");
            if (drpClasses.SelectedValue.ToString() != "0")
                stringBuilder.Append(" and c.ClassID= " + drpClasses.SelectedValue.ToString());
            if (drpSection.SelectedValue.ToString() != "0")
                stringBuilder.Append(" and cs.Section= '" + drpSection.SelectedValue.ToString() + "'");
            stringBuilder.Append(" order by cs.classid,cs.section,RecvDt");
            DataTable dt = new Common().ExecuteSql(stringBuilder.ToString().Trim());
            if (dt.Rows.Count > 0)
            {
                createRemarkReport(dt);
                btnPrint.Visible = true;
            }
            else
                ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
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
                stringBuilder.Append("Fee Cash Receipt:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + lblReport.Text.ToString().Trim()).ToString().Trim());
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
            Response.AddHeader("content-disposition", "attachment;filename=" + str);
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = "";
        btnPrint.Visible = false;
    }

    protected void drpSection_SelectedIndexChanged1(object sender, EventArgs e)
    {
        lblReport.Text = "";
    }
}
