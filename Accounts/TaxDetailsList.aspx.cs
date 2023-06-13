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

public partial class Accounts_TaxDetailsList : System.Web.UI.Page
{
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        fillGrid();
    }

    protected void grdTax_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTax.PageIndex = e.NewPageIndex;
        fillGrid();
    }

    protected void grdTax_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dataTable1 = new DataTable();
            Hashtable hashtable = new Hashtable();
            clsDAL clsDal = new clsDAL();
            string empty = string.Empty;
            Label control = (Label)grdTax.Rows[e.RowIndex].FindControl("lbl2");
            string str = clsDal.ExecuteScalarQry("SELECT TaxId FROM dbo.SI_TaxMaster WHERE TaxId=" + control.Text);
            hashtable.Add("@TaxId", Convert.ToInt32(str));
            DataTable dataTable2 = clsDal.GetDataTable("ACTS_DelTax", hashtable);
            fillGrid();
            if (dataTable2.Rows.Count > 0)
            {
                lblMsg.Text = "Tax already in Use. Can't be deleted.";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                lblMsg.Text = "Record Deleted Successfully";
                lblMsg.ForeColor = Color.Green;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void fillGrid()
    {
        dt = new DataTable();
        ht = new Hashtable();
        obj = new clsDAL();
        dt = obj.GetDataTable("ACTS_GrdTaxDetails", ht);
        grdTax.DataSource = dt;
        grdTax.DataBind();
        lblRecord.Text = "Total Record(s) : " + dt.Rows.Count.ToString();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("TaxDetails.aspx");
    }
}