using AjaxControlToolkit;
using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

public partial class Reports_rptMonthlyCollectionGraph : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            bindDropDown(ddlSession, "Select SessionYr from dbo.PS_FeeNormsNew where SchoolId=" + int.Parse(Session["SchoolId"].ToString()) + " order by SessionYr Desc", "SessionYr", "SessionYr");
            Show();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        try
        {
            DataTable dataTableQry = obj.GetDataTableQry(query);
            drp.DataSource = dataTableQry;
            drp.DataTextField = textfield;
            drp.DataValueField = valuefield;
            drp.DataBind();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        try
        {
            ht.Clear();
            ht.Add("@SessionYr", ddlSession.SelectedValue);
            ht.Add("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
            ht.Add("@FeeType", int.Parse(ddlFeeType.SelectedValue));
            dt = obj.GetDataTable("FeeChartMonthly", ht);
            if (dt.Rows.Count > 0)
            {
                lblMsg.Text = "";
                Chart1.Visible = true;
                foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                    Chart1.Series.FindByName("Series1").Points.AddXY(row["EstMonth"], new object[1]
          {
            row["Collections"]
          });
            }
            else
            {
                Chart1.Visible = false;
                lblMsg.Text = "Data Not Exist";
                lblMsg.ForeColor = Color.Red;
            }
            if (ddlChartType.SelectedValue == "Column")
                Chart1.Series.FindByName("Series1").ChartType = SeriesChartType.Column;
            else if (ddlChartType.SelectedValue == "Bar")
            {
                Chart1.Series.FindByName("Series1").ChartType = SeriesChartType.Bar;
                Chart1.Series.FindByName("Series1").Color = Color.Purple;
            }
            else if (ddlChartType.SelectedValue == "Line")
            {
                Chart1.Series.FindByName("Series1").ChartType = SeriesChartType.Line;
                Chart1.Series.FindByName("Series1").Color = Color.Red;
            }
            else if (ddlChartType.SelectedValue == "BoxPlot")
            {
                Chart1.Series.FindByName("Series1").ChartType = SeriesChartType.BoxPlot;
                Chart1.Series.FindByName("Series1").Color = Color.Violet;
            }
            if (rdbtnOrientation.SelectedValue == "0")
                Chart1.ChartAreas.FindByName("MainChartArea").Area3DStyle.Enable3D = false;
            else
                Chart1.ChartAreas.FindByName("MainChartArea").Area3DStyle.Enable3D = true;
        }
        catch (Exception ex)
        {
        }
    }
}