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

public partial class Reports_rptNewAdmissionRatioGraph : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    //protected Label lblMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            bindDropDown(ddlClass, "select ClassID,ClassName from dbo.PS_ClassMaster where SchoolId=" + (object)int.Parse(Session["SchoolId"].ToString()) + " order by ClassID", "ClassName", "ClassID");
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
            drp.DataSource = (object)dataTableQry;
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
            ht.Add((object)"@ClassId", (object)ddlClass.SelectedValue);
            ht.Add((object)"@SchoolId", (object)int.Parse(Session["SchoolId"].ToString()));
            dt = obj.GetDataTable("NewAdmissionChart", ht);
            if (dt.Rows.Count > 0)
            {
                lblMsg.Text = "";
                Chart1.Visible = true;
                foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                    Chart1.Series.FindByName("Series1").Points.AddXY(row["AdmnSessYr"], new object[1]
          {
            row["NoOfStudents"]
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

    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        Show();
    }
}