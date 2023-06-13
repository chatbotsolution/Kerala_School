using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admissions_SetBusRoute : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns clsStatic = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;
        Page.Form.DefaultButton = btngo.UniqueID;
        fillclass();
        FillClassSection();
        fillsession();
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = obj.GetDataTable("ps_sp_get_classesForDDL").DefaultView;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
    }

    private void FillClassSection()
    {
        string selectedValue = drpSession.SelectedValue;
        drpSection.Items.Clear();
        drpSection.DataSource = obj.GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "@classId",
         drpclass.SelectedValue.ToString().Trim()
      },
      {
         "@session",
         selectedValue
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("ALL", "0"));
    }

    protected void fillsession()
    {
        drpSession.DataSource = obj.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetVisible(false);
        lblRecCount.Visible = false;
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
        SetVisible(false);
        lblRecCount.Visible = false;
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void SetVisible(bool st)
    {
        grvBusRoute.Visible = st;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        FillGrid();
        UpdatePanel1.Update();
    }

    protected void FillGrid()
    {
        FillBusRouteStudentDetails();
    }

    private void FillBusRouteStudentDetails()
    {
        Hashtable hashtable = new Hashtable();
        string selectedValue = drpSession.SelectedValue;
        hashtable.Add("@Session", selectedValue);
        hashtable.Add("@classId", drpclass.SelectedValue.ToString().Trim());
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@section", drpSection.SelectedValue.ToString().Trim());
        DataTable dataTable = obj.GetDataTable("ps_setBusRoute", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            grvBusRoute.DataSource = dataTable;
            grvBusRoute.DataBind();
            lblMsg.Visible = false;
            lblRecCount.Visible = true;
            lblRecCount.Text = "Total Record: " + dataTable.Rows.Count.ToString();
            SetVisible(true);
        }
        else
        {
            lblRecCount.Visible = false;
            lblMsg.Visible = true;
            lblMsg.Text = "No Record Found";
            SetVisible(false);
        }
    }

    protected void grvBusRoute_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        DropDownList control1 = (DropDownList)e.Row.FindControl("drpBusRoute");
        clsDAL clsDal = new clsDAL();
        control1.DataSource = clsDal.GetDataTableQry("select RouteId, RouteName from dbo.PS_BusRouteMaster");
        control1.DataTextField = "RouteName";
        control1.DataValueField = "RouteId";
        control1.DataBind();
        control1.Items.Insert(0, new ListItem("-Select-", ""));
        Label control2 = (Label)e.Row.FindControl("lblRouteID");
        if (!(control2.Text.Trim() != ""))
            return;
        control1.SelectedValue = control2.Text.Trim();
        control1.BackColor = ColorTranslator.FromHtml("#E5FFCE");
    }

    protected void drpBusRoute_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow parent = (GridViewRow)((Control)sender).Parent.Parent;
        LinkButton control1 = (LinkButton)parent.FindControl("lnkbtnBusRoute");
        Label control2 = (Label)parent.FindControl("fee");
        DropDownList control3 = (DropDownList)parent.FindControl("drpBusRoute");
        control1.Visible = true;
        string str = new clsDAL().ExecuteScalarQry("select fee from dbo.PS_BusRouteMaster where RouteId='" + control3.SelectedValue + "'");
        control2.Text = str.ToString();
    }

    protected void lnkbtnBusRoute_Click(object sender, EventArgs e)
    {
        GridViewRow parent = (GridViewRow)((Control)sender).Parent.Parent;
        DropDownList control = (DropDownList)parent.FindControl("drpBusRoute");
        if (control.SelectedIndex > 0)
        {
            clsDAL clsDal = new clsDAL();
            Hashtable hashtable = new Hashtable();
            DataTable dataTable = new DataTable();
            string selectedValue = drpSession.SelectedValue;
            hashtable.Add("@SessionYr", selectedValue);
            int int32 = Convert.ToInt32(parent.Cells[0].Text);
            hashtable.Add("@AdmnNo", int32);
            hashtable.Add("@BusRouteID", control.SelectedValue);
            if (clsDal.ExecuteScalar("ps_uptSetBusRoute", hashtable).Trim() == "s")
            {
                FillGrid();
                lblMsg.Visible = true;
                lblMsg.Text = "Data Saved Successfully !";
                lblMsg.ForeColor = Color.Green;
            }
            else
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Data is Not Saved";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
        {
            lblMsg.Text = "Please select Route";
            lblMsg.ForeColor = Color.Red;
        }
    }
}