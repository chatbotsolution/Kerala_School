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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_Brands : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            btnClear.Enabled = true;
            clsDAL clsDal = new clsDAL();
            lblMsg.Text = string.Empty;
            if (Page.IsPostBack)
                return;
            BindBrandGrid();
            pnlAddDetail.Visible = false;
            ViewState["Id"] = string.Empty;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void BindBrandGrid()
    {
        DataTable dataTable = new clsDAL().GetDataTable("SI_GetBrand");
        if (dataTable.Rows.Count > 0)
        {
            grdBrands.DataSource = dataTable.DefaultView;
            grdBrands.DataBind();
            grdBrands.Visible = true;
            norecord.Visible = false;
        }
        else
        {
            grdBrands.Visible = false;
            norecord.Visible = true;
        }
        updadddetail.Update();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@BrandName", txtBrandName.Text.ToString().Trim());
        DataTable dataTable2;
        if (ViewState["Id"].ToString().Trim() == "")
        {
            dataTable2 = clsDal.GetDataTable("SI_InsertBrandMaster", hashtable);
        }
        else
        {
            hashtable.Add("@BrandId", int.Parse(ViewState["Id"].ToString()));
            dataTable2 = clsDal.GetDataTable("SI_UpdateBrandMaster", hashtable);
        }
        if (dataTable2.Rows.Count > 0)
        {
            lblMsg.Text = "Duplicate record exist";
            lblMsg.ForeColor = Color.Red;
            pnlAddDetail.Visible = true;
            pnlList.Visible = false;
            updadddetail.Update();
        }
        else if (ViewState["Id"].ToString().Trim() == "")
        {
            ViewState["Id"] = string.Empty;
            ClearControls();
            BindBrandGrid();
            lblMsg.Text = "Record Saved Successfully";
            lblMsg.ForeColor = Color.Green;
            updadddetail.Update();
        }
        else
        {
            ViewState["Id"] = string.Empty;
            ClearControls();
            BindBrandGrid();
            lblMsg.Text = "Record Updated Successfully";
            lblMsg.ForeColor = Color.Green;
            updadddetail.Update();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Welcome.aspx");
    }

    private void ClearControls()
    {
        txtBrandName.Text = "";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "changelabel(' ');", true);
        ViewState["Id"] = "";
    }

    protected void grdBrands_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (grdBrands.EditIndex > -1)
            grdBrands.EditIndex = -1;
        grdBrands.PageIndex = e.NewPageIndex;
        BindBrandGrid();
    }

    protected void grdBrands_RowEditing(object sender, GridViewEditEventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        btnClear.Enabled = false;
        grdBrands.EditIndex = e.NewEditIndex;
        BindBrandGrid();
        DataTable dataTableQry = clsDal.GetDataTableQry("select * from SI_BrandMaster where BrandId='" + grdBrands.DataKeys[e.NewEditIndex].Value.ToString().Trim() + "'");
        updadddetail.Update();
        if (dataTableQry.Rows.Count > 0)
        {
            txtBrandName.Text = dataTableQry.Rows[0]["BrandName"].ToString();
            ViewState["Id"] = dataTableQry.Rows[0]["BrandId"].ToString();
        }
        pnlAddDetail.Visible = true;
        pnlList.Visible = false;
        updadddetail.Update();
    }

    protected void grdBrands_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            clsDAL clsDal = new clsDAL();
            Label control = (Label)grdBrands.Rows[e.RowIndex].FindControl("lbl2");
            DataTable dataTable = new DataTable();
            if (clsDal.GetDataTable("SI_DelBrand", new Hashtable()
      {
        {
           "@BrandId",
           control.Text.Trim()
        }
      }).Rows.Count > 0)
            {
                lblMsg.Text = "Dependency for this record exist";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                BindBrandGrid();
                lblMsg.Text = "Record Deleted Successfully";
                lblMsg.ForeColor = Color.Green;
                updadddetail.Update();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Dependency for this record exist";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void grdBrands_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        if (DataBinder.Eval(e.Row.DataItem, "BrandId").ToString().Equals("0"))
        {
            ((WebControl)e.Row.Cells[0].FindControl("imgEdit")).Enabled = false;
            ((WebControl)e.Row.Cells[0].FindControl("btnDelete")).Enabled = false;
        }
        else
        {
            ((WebControl)e.Row.Cells[0].FindControl("imgEdit")).Enabled = true;
            ((WebControl)e.Row.Cells[0].FindControl("btnDelete")).Enabled = true;
        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        ClearControls();
        pnlAddDetail.Visible = false;
        pnlList.Visible = true;
        updadddetail.Update();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtBrandName.Text = string.Empty;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "display", "ViewDivAdd();", true);
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "changelabel('-Insert Mode');", true);
        txtBrandName.Focus();
        pnlAddDetail.Visible = true;
        pnlList.Visible = false;
        updadddetail.Update();
        ViewState["Id"] = "";
    }
}