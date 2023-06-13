using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Inventory_InvStockCheckList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dtItemSection = new DataTable();
    private Hashtable htSection = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] != null)
        {
           lblMsg.Text = string.Empty;
            if (Page.IsPostBack)
                return;
           Page.Form.DefaultButton =btnAddNew.UniqueID;
           bindGrd();
        }
        else
           Response.Redirect("~/Login.aspx");
    }

    private void bindGrd()
    {
        DataTable dataTable1 = new DataTable();
        Hashtable ht = new Hashtable();
        if (Session["User"].ToString() != "admin")
            ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
        DataTable dataTable2 =obj.GetDataTable("SI_GetStockCheck", ht);
       grdStock.DataSource = dataTable2;
       grdStock.DataBind();
        if (dataTable2.Rows.Count > 0)
        {
           btnDelete.Enabled = true;
           lblRecord.Text = "Total Record(s) : " + dataTable2.Rows.Count.ToString();
        }
        else
        {
           lblRecord.Text = string.Empty;
           btnDelete.Enabled = false;
           lblMsg.Text = "No Record !";
           lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
       Response.Redirect("InvStockCheck.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Control)btnDelete,btnDelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                string str =Request["Checkb"];
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str.Split(chArray))
                   DeleteStock(obj.ToString());
               bindGrd();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DeleteStock(string Id)
    {
        Hashtable ht = new Hashtable();
        DataTable dataTable1 = new DataTable();
        clsDAL clsDal = new clsDAL();
        ht.Add("@StockCheckId", Id);
        DataTable dataTable2 = clsDal.GetDataTable("SI_DelStock", ht);
        if (dataTable2.Rows.Count > 0)
        {
           lblMsg.Text = dataTable2.Rows[0][0].ToString();
           lblMsg.ForeColor = Color.Red;
        }
        else
        {
           lblMsg.Text = "Data Deleted Successfully !";
           lblMsg.ForeColor = Color.Green;
        }
    }

    protected void grdStock_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       grdStock.PageIndex = e.NewPageIndex;
       bindGrd();
    }

    protected void grdStock_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        if (DataBinder.Eval(e.Row.DataItem, "VerifiedBy").ToString().Equals(""))
        {
            e.Row.FindControl("dvEdit").Visible = true;
            e.Row.FindControl("dvHide").Visible = false;
        }
        else
        {
            e.Row.FindControl("dvEdit").Visible = false;
            e.Row.FindControl("dvHide").Visible = true;
        }
    }
}