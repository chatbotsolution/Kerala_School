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
using System.Web.UI.WebControls;

public partial class Accounts_SuppliersList : System.Web.UI.Page
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
        dt = obj.GetDataTable("ACTS_SupplierList");
        gvSupplierList.DataSource = dt;
        gvSupplierList.DataBind();
    }

    protected void gvSupplierList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (!(e.CommandName == "Remove"))
                return;
            ht.Clear();
            ht.Add("@SupplierPartyId", int.Parse(e.CommandArgument.ToString()));
            lblMsg.Text = obj.ExecuteScalar("ACTS_DeleteSupplier", ht);
            if (lblMsg.Text == "Deleted Successfully")
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("Suppliers.aspx");
    }
}