using ASP;
using Classes.DA;
using SanLib;
using System;
using System.Collections;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Masters_BusRouteMasterList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
  {
    lblMsg.Text = string.Empty;
    if (Page.IsPostBack)
      return;
    if (Session["User"] != null)
      GridFill();
    else
      Response.Redirect("../Login.aspx");
  }

  protected void GridFill()
  {
    int pageIndex = grvRouteList.PageIndex;
    Common common = new Common();
    Hashtable hashtable = new Hashtable();
    grvRouteList.DataSource =  common.GetDataTable("[ps_spGetBusRoute]");
    grvRouteList.DataBind();
    grvRouteList.PageIndex = pageIndex;
  }

  protected void btnNew_Click(object sender, EventArgs e)
  {
    Response.Redirect("BusRouteMaster.aspx");
  }

  protected void btndelete_Click(object sender, EventArgs e)
  {
    Common common1 = new Common();
    if (Request["Checkb"] == null)
    {
      ScriptManager.RegisterClientScriptBlock((Control) btndelete, btndelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
    }
    else
    {
      string[] strArray = Request["Checkb"].Split(',');
      int num = 0;
      for (int index = 0; index < strArray.Length; ++index)
      {
        Common common2 = new Common();
        if (new clsDAL().ExecuteScalar("ps_delRoute", new Hashtable()
        {
          {
             "@RouteId",
             Convert.ToInt32(strArray[index].ToString())
          }
        }) != "")
          ++num;
      }
      GridFill();
      if (num > 0)
      {
        lblMsg.Text = num != 1 ? num.ToString() + " Records could not be deleted. The fee components are in use" : num.ToString() + " Record could not be deleted. The fee component is in use";
        lblMsg.ForeColor = Color.Red;
      }
      else
      {
        lblMsg.Text = "Data Deleted Successfully !";
        lblMsg.ForeColor = Color.Green;
      }
    }
  }

  protected void btnSetPriority_Click(object sender, EventArgs e)
  {
  }

  protected void lnkbtnAccHead_Click(object sender, EventArgs e)
  {
  }

  protected void grdFeeComponent_RowDataBound(object sender, GridViewRowEventArgs e)
  {
  }

  protected void drpAccHeads_SelectedIndexChanged(object sender, EventArgs e)
  {
  }
}