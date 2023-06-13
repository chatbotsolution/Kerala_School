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

public partial class Accounts_RaisePOList : System.Web.UI.Page
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
            try
            {
                dtpFromDt.SetDateValue(DateTime.Now);
                dtpToDt.SetDateValue(DateTime.Now);
                BindGrid();
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Transaction Failed ! Try Again .";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void BindGrid()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Select * from ACTS_VWPOList where UserId=" + int.Parse(Session["User_Id"].ToString()) + " and SchoolId=" + int.Parse(Session["SchoolId"].ToString()));
        if (!txtFrmDt.Text.Trim().Equals(string.Empty) && !txtToDt.Text.Trim().Equals(string.Empty))
            stringBuilder.Append(" and OrderDate >='" + dtpFromDt.GetDateValue().ToString("MM/dd/yyyy") + " 00:00:00' and  OrderDate<='" + dtpToDt.GetDateValue().ToString("MM/dd/yyyy") + " 23:59:59'");
        dt = obj.GetDataTableQry(stringBuilder.ToString());
        gvPOList.DataSource = dt;
        gvPOList.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("RaisePO.aspx");
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

    protected void gvPOList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (!(e.CommandName == "Remove"))
                return;
            ht.Clear();
            ht.Add("@PurOrderId", int.Parse(e.CommandArgument.ToString()));
            lblMsg.Text = obj.ExecuteScalar("ACTS_DeletePO", ht);
            if (lblMsg.Text == "Purchase Order Deleted Successfully")
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
}