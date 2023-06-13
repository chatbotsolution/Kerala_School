using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_StockWritesoffList : System.Web.UI.Page
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
            dtpFromDt.SetDateValue(DateTime.Now.AddMonths(-1));
            dtpToDt.SetDateValue(DateTime.Now);
            BindGrid();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void BindGrid()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Select * from ACTS_VWWritesOffList where SchoolId=" + int.Parse(Session["SchoolId"].ToString()));
        if (!txtFromDt.Text.Trim().Equals(string.Empty) && !txtToDt.Text.Trim().Equals(string.Empty))
            stringBuilder.Append(" and WritesOffDate >='" + dtpFromDt.GetDateValue().ToString("dd MMM yyyy") + " 00:00:00' and  WritesOffDate<='" + dtpToDt.GetDateValue().ToString("dd MMM yyyy") + " 23:59:59'");
        dt = obj.GetDataTableQry(stringBuilder.ToString());
        gvWritesOffList.DataSource = dt;
        gvWritesOffList.DataBind();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            BindGrid();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again .";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("StockWritesoff.aspx");
    }

    protected void gvWritesOffList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (!(e.CommandName == "Remove"))
                return;
            ht.Clear();
            ht.Add("@WritesoffId", int.Parse(e.CommandArgument.ToString()));
            lblMsg.Text = obj.ExecuteScalar("ACTS_DelWritesOff", ht);
            if (lblMsg.Text == "WritesOff Cancelled Successfully")
                lblMsg.ForeColor = Color.Green;
            else
                lblMsg.ForeColor = Color.Red;
            BindGrid();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again .";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void gvWritesOffList_DataBound(object sender, EventArgs e)
    {
        if (!(Session["userrights"].ToString().Trim() == "a"))
            return;
        foreach (Control row in gvWritesOffList.Rows)
            row.FindControl("imgbtnDelete").Visible = true;
    }
}