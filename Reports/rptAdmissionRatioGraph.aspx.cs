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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Reports_rptAdmissionRatioGraph : System.Web.UI.Page
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
      bindDropDown(ddlSession, "Select SessionYr from dbo.PS_FeeNormsNew where SchoolId=" +  int.Parse(Session["SchoolId"].ToString()) + " order by SessionYr Desc", "SessionYr", "SessionYr");
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
      drp.DataSource =  dataTableQry;
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

  protected void rdbtnAdmission_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (rdbtnAdmission.SelectedValue == "0")
      dvSessiionYr.Visible = true;
    else
      dvSessiionYr.Visible = false;
    Show();
  }

  private void Show()
  {
    try
    {
      ht.Clear();
      if (rdbtnAdmission.SelectedValue == "0")
        ht.Add( "@SessionYr",  ddlSession.SelectedValue);
      ht.Add( "@SchoolId",  int.Parse(Session["SchoolId"].ToString()));
      dt = obj.GetDataTable("AdmissionChart", ht);
      if (dt.Rows.Count > 0)
      {
        lblMsg.Text = "";
        Chart1.Visible = true;
        if (rdbtnAdmission.SelectedValue == "0")
        {
          foreach (DataRow row in (InternalDataCollectionBase) dt.Rows)
            Chart1.Series.FindByName("Series1").Points.AddXY(row["ClassName"], new object[1]
            {
              row["NoOfStudents"]
            });
        }
        else
        {
          foreach (DataRow row in (InternalDataCollectionBase) dt.Rows)
            Chart1.Series.FindByName("Series1").Points.AddXY(row["AdmnSessYr"], new object[1]
            {
              row["NoOfStudents"]
            });
        }
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