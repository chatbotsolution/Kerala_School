using ASP;
using Classes.DA;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptProsStockStatus : System.Web.UI.Page
{
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        FillSession();
        FillProspType();
    }

    private void FillSession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select distinct SessionID,SessionYr from dbo.PS_FeeNormsNew order by sessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillProspType()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        drpProspectusType.DataSource = clsDal.GetDataTableQry("select ProspTypeId,ProspType from dbo.ProspTypeMaster order by ProspTypeId");
        drpProspectusType.DataTextField = "ProspType";
        drpProspectusType.DataValueField = "ProspType";
        drpProspectusType.DataBind();
        drpProspectusType.Items.Insert(0, "- ALL -");
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    private void GenerateReport()
    {
        ht.Add("@SessionYr", drpSession.SelectedValue);
        if (drpProspectusType.SelectedIndex > 0)
            ht.Add("@ProspectusType", drpProspectusType.SelectedValue);
        dt = obj.GetDataTable("Ps_Sp_ProsStockPosition", ht);
        try
        {
            if (dt.Rows.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='1' style='background-color:#FFF;  width:100%;'>");
                stringBuilder.Append("<tr><td colspan='4' class='tbltd'><strong>Total Record :" + dt.Rows.Count + "</strong></td></tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td class='tblheader' align='left'><b>Prospectus Type</b></td>");
                stringBuilder.Append("<td class='tblheader' align='left'><b>Quantity Available</b></td>");
                stringBuilder.Append("</tr>");
                foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td class='tbltd'>");
                    stringBuilder.Append(row["ProspectusType"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("<td class='tbltd'>");
                    stringBuilder.Append(row["Bal"].ToString().Trim());
                    stringBuilder.Append("</td>");
                    stringBuilder.Append("</tr>");
                }
                stringBuilder.Append("</table>");
                lblReport.Text = stringBuilder.ToString().Trim();
            }
            else
                lblReport.Text = "";
        }
        catch (Exception ex)
        {
        }
    }
}