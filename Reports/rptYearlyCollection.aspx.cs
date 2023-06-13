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

public partial class Reports_rptYearlyCollection : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            Show();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void Show()
    {
        try
        {
            ht.Clear();
            ht.Add("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
            ht.Add("@FeeType", int.Parse(ddlFeeType.SelectedValue));
            ht.Add("@CollectionType", rdbtnCollectiontype.SelectedValue);
            dt = obj.GetDataTable("FeeChartYearly", ht);
            if (dt.Rows.Count > 0)
            {
                lblMsg.Text = "";
                Chart1.Visible = true;
                foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                    Chart1.Series.FindByName("Series1").Points.AddXY(row["Session"], new object[1]
          {
            row["Amount"]
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

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Show();
    }
}