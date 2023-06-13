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

public partial class Accounts_ItemSaleReturnList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsDAL DAL = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            try
            {
                dtpFromDt.SetDateValue(DateTime.Now);
                dtpToDt.SetDateValue(DateTime.Now);
                fillsession();
                fillclass();
                fillstudent();
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Transaction Failed ! Try Again.";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void fillsession()
    {
        DAL = new clsDAL();
        drpSession.DataSource = DAL.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        DAL = new clsDAL();
        DataTable dataTableQry = DAL.GetDataTableQry("select * from ps_classmaster");
        drpclass.DataSource = dataTableQry;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("-ALL-", "0"));
        if (dataTableQry.Rows.Count > 0)
            fillstudent();
        else
            drpclass.Items.Insert(0, new ListItem("No Record", "0"));
    }

    private void fillstudent()
    {
        DAL = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select distinct Remarks from dbo.ACTS_PurchaseDoc where invno='Sale Return'");
        DataTable dataTableQry = DAL.GetDataTableQry(stringBuilder.ToString().Trim());
        try
        {
            drpstudent.DataSource = dataTableQry;
            drpstudent.DataTextField = "Remarks";
            drpstudent.DataValueField = "Remarks";
            drpstudent.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        if (dataTableQry.Rows.Count > 0)
        {
            drpstudent.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        else
        {
            drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
            txtadmnno.Text = "";
        }
    }

    private void BindGrid()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Select * from ACTS_VWSaleReturnList where SchoolId=" + int.Parse(Session["SchoolId"].ToString()));
        if (!txtFrmDt.Text.Trim().Equals(string.Empty) && !txtToDt.Text.Trim().Equals(string.Empty))
            stringBuilder.Append(" and PurDate >='" + dtpFromDt.GetDateValue().ToString("MM/dd/yyyy") + " 00:00:00' and  PurDate<='" + dtpToDt.GetDateValue().ToString("MM/dd/yyyy") + " 23:59:59'");
        if (drpstudent.SelectedIndex > 0)
            stringBuilder.Append(" and remarks ='" + drpstudent.SelectedItem.Text.ToString().Trim() + "'");
        stringBuilder.Append(" Order by PurDate desc");
        dt = obj.GetDataTableQry(stringBuilder.ToString());
        gvStockList.DataSource = dt;
        gvStockList.DataBind();
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
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("ItemSaleReturn.aspx");
    }

    protected void gvStockList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (!(e.CommandName == "Remove"))
                return;
            ht.Clear();
            ht.Add("@PurchaseId", int.Parse(e.CommandArgument.ToString()));
            ht.Add("@TransDt", Session["TransDt"]);
            ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
            ht.Add("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
            string str = obj.ExecuteScalar("ACTS_DelPurchaseStock", ht);
            if (str.Trim() == string.Empty)
            {
                lblMsg.ForeColor = Color.Green;
                lblMsg.Text = "Stock Deleted Successfully";
            }
            else
            {
                lblMsg.ForeColor = Color.Red;
                lblMsg.Text = str;
            }
            BindGrid();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void gvStockList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            gvStockList.PageIndex = e.NewPageIndex;
            BindGrid();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void gvStockList_DataBound(object sender, EventArgs e)
    {
        if (!(Session["userrights"].ToString().Trim() == "a"))
            return;
        foreach (Control row in gvStockList.Rows)
            row.FindControl("imgbtnDelete").Visible = true;
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpstudent.Items.Clear();
        drpclass.SelectedIndex = 0;
        txtadmnno.Text = string.Empty;
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
    }
}