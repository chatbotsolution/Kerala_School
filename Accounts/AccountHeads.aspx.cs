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

public partial class Accounts_AccountHeads : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            lblMsg.Text = string.Empty;
            btnClear.Enabled = true;
            clsDAL clsDal = new clsDAL();
            if (Page.IsPostBack)
                return;
            FillAccountGroupsList();
            BindAccountGroupGrid();
            pnlAddDetail.Visible = false;
            ViewState["Id"] = string.Empty;
            FillAccGroups();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void FillAccGroups()
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable = new DataTable();
        drpAccGroups.DataSource = new clsDAL().GetDataTableQry("Select AG_Code,AG_Name from dbo.Acts_AccountGroups order by AG_Name");
        drpAccGroups.DataTextField = "AG_Name";
        drpAccGroups.DataValueField = "AG_Code";
        drpAccGroups.DataBind();
        drpAccGroups.Items.Insert(0, new ListItem("-All-", "0"));
    }

    private void FillAccountGroupsList()
    {
        ddlAccountGroup.DataSource = new clsDAL().GetDataTable("ACTS_GetDrpAccGroup");
        ddlAccountGroup.DataTextField = "AG_Name";
        ddlAccountGroup.DataValueField = "AG_Code";
        ddlAccountGroup.DataBind();
        ddlAccountGroup.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void BindAccountGroupGrid()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        if (drpAccGroups.SelectedIndex > 0)
            hashtable.Add("@AGCode", drpAccGroups.SelectedValue.ToString());
        DataTable dataTable = clsDal.GetDataTable("ACTS_GetAccGroupsList", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            grdAccountGroup.DataSource = dataTable.DefaultView;
            grdAccountGroup.DataBind();
            grdAccountGroup.Visible = true;
            norecord.Visible = false;
        }
        else
        {
            grdAccountGroup.Visible = false;
            norecord.Visible = true;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@AcctsHead", txtAccountMasterName.Text.ToString().Trim());
        hashtable.Add("@AG_Code", ddlAccountGroup.SelectedValue.Trim());
        hashtable.Add("@AH_Type", "User");
        DataTable dataTable2;
        if (ViewState["Id"].ToString().Trim() == "")
        {
            dataTable2 = clsDal.GetDataTable("ACTS_InsertAccountHeads", hashtable);
        }
        else
        {
            hashtable.Add("@AcctsHeadId", int.Parse(ViewState["Id"].ToString()));
            dataTable2 = clsDal.GetDataTable("ACTS_UpdateAccountHeads", hashtable);
        }
        if (dataTable2.Rows.Count > 0)
        {
            lblMsg.Text = "Duplicate record exist";
            lblMsg.ForeColor = Color.Red;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "display", "ViewDivAdd();", true);
            updadddetail.Update();
        }
        else if (ViewState["Id"].ToString().Trim() == "")
        {
            ViewState["Id"] = string.Empty;
            ClearAccountGroupControls();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "display", "ViewDivAdd();", true);
            BindAccountGroupGrid();
            lblMsg.Text = "Record Saved Successfully";
            lblMsg.ForeColor = Color.Green;
            updadddetail.Update();
        }
        else
        {
            ViewState["Id"] = string.Empty;
            ClearAccountGroupControls();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "display", "ViewDivAdd();", true);
            BindAccountGroupGrid();
            lblMsg.Text = "Record Updated Successfully";
            lblMsg.ForeColor = Color.Green;
            updadddetail.Update();
        }
    }

    protected void btnGotoList_Click(object sender, EventArgs e)
    {
        ClearAccountGroupControls();
        pnlAddDetail.Visible = false;
        pnlList.Visible = true;
        updadddetail.Update();
    }

    private void ClearAccountGroupControls()
    {
        BindAccountGroupGrid();
        txtAccountMasterName.Text = "";
        ddlAccountGroup.SelectedIndex = 0;
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "text", "changelabel(' ');", true);
        ViewState["Id"] = "";
    }

    protected void grdAccountGroup_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (grdAccountGroup.EditIndex > -1)
            grdAccountGroup.EditIndex = -1;
        grdAccountGroup.PageIndex = e.NewPageIndex;
        BindAccountGroupGrid();
    }

    protected void grdAccountGroup_RowEditing(object sender, GridViewEditEventArgs e)
    {
        btnClear.Enabled = false;
        grdAccountGroup.EditIndex = e.NewEditIndex;
        BindAccountGroupGrid();
        DataTable dataTableQry = new clsDAL().GetDataTableQry("select * from ACTS_AccountHeads where AcctsHeadId='" + grdAccountGroup.DataKeys[e.NewEditIndex].Value.ToString().Trim() + "'");
        updadddetail.Update();
        if (dataTableQry.Rows.Count > 0)
        {
            FillAccountGroupsList();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "display", "ViewDivAdd();", true);
            txtAccountMasterName.Text = dataTableQry.Rows[0]["AcctsHead"].ToString();
            ddlAccountGroup.SelectedValue = dataTableQry.Rows[0]["AG_Code"].ToString();
            ViewState["Id"] = dataTableQry.Rows[0]["AcctsHeadId"].ToString();
        }
        pnlAddDetail.Visible = true;
        pnlList.Visible = false;
        updadddetail.Update();
    }

    protected void grdAccountGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Label control = (Label)grdAccountGroup.Rows[e.RowIndex].FindControl("lbl2");
            clsDAL clsDal = new clsDAL();
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = clsDal.GetDataTable("ACTS_DelAccountHead", new Hashtable()
      {
        {
           "@AcctsHeadId",
           control.Text.Trim()
        }
      });
            if (dataTable2.Rows.Count > 0)
            {
                lblMsg.Text = dataTable2.Rows[0][0].ToString();
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                grdAccountGroup.EditIndex = -1;
                BindAccountGroupGrid();
                lblMsg.Text = "Record Deleted Successfully ";
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

    protected void grdAccountGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        if (DataBinder.Eval(e.Row.DataItem, "AH_Type").ToString().Equals("System"))
        {
            ((WebControl)e.Row.Cells[0].FindControl("imgEdit")).Enabled = false;
            ((WebControl)e.Row.Cells[0].FindControl("imgDelete")).Enabled = false;
        }
        else
        {
            ((WebControl)e.Row.Cells[0].FindControl("imgEdit")).Enabled = true;
            ((WebControl)e.Row.Cells[0].FindControl("imgDelete")).Enabled = true;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        FillAccountGroupsList();
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "display", "ViewDivAdd();", true);
        ddlAccountGroup.Focus();
        pnlAddDetail.Visible = true;
        pnlList.Visible = false;
        updadddetail.Update();
        ViewState["Id"] = "";
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearAccountGroupControls();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Welcome.aspx");
    }

    protected void drpAccGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindAccountGroupGrid();
    }
}