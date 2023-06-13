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

public partial class Accounts_AccountGroups : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            btnClear.Enabled = true;
            lblMsg.Text = string.Empty;
            clsDAL clsDal = new clsDAL();
            if (Page.IsPostBack)
                return;
            FillAccountGroupsList();
            BindAccountGroupGrid();
            pnlAddDetail.Visible = false;
            ViewState["Id"] = string.Empty;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void FillAccountGroupsList()
    {
        DataTable dataTable = new clsDAL().GetDataTable("ACTS_GetDrpAccGroup");
        if (dataTable.Rows.Count <= 0)
            return;
        ddlParentAccountGroup.Items.Clear();
        ddlParentAccountGroup.Items.Add(new ListItem("-Parent-", "0"));
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            ddlParentAccountGroup.Items.Add(new ListItem(row["Ag_Name"].ToString(), row["Ag_Code"].ToString()));
    }

    private void BindAccountGroupGrid()
    {
        DataTable dataTable = new clsDAL().GetDataTable("ACTS_GetAccountGroups");
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
        hashtable.Add("@AG_Name", txtGroupName.Text.ToString().Trim());
        hashtable.Add("@AG_Type", "USER");
        hashtable.Add("@AG_Parent", ddlParentAccountGroup.SelectedValue.Trim());
        DataTable dataTable2;
        if (ViewState["Id"].ToString().Trim() == "")
        {
            dataTable2 = clsDal.GetDataTable("ACTS_InsertAccountGroupS", hashtable);
        }
        else
        {
            hashtable.Add("@AG_Code", int.Parse(ViewState["Id"].ToString()));
            dataTable2 = clsDal.GetDataTable("ACTS_UpdateAccountGroupS", hashtable);
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
        txtGroupName.Text = "";
        ddlParentAccountGroup.SelectedIndex = 0;
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
        DataTable dataTableQry = new clsDAL().GetDataTableQry("select * from ACTS_AccountGroups where AG_Code='" + grdAccountGroup.DataKeys[e.NewEditIndex].Value.ToString().Trim() + "'");
        updadddetail.Update();
        if (dataTableQry.Rows.Count > 0)
        {
            FillAccountGroupsList();
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "display", "ViewDivAdd();", true);
            txtGroupName.Text = dataTableQry.Rows[0]["AG_Name"].ToString();
            ddlParentAccountGroup.SelectedValue = dataTableQry.Rows[0]["AG_Parent"].ToString();
            ViewState["Id"] = dataTableQry.Rows[0]["AG_Code"].ToString();
            foreach (ListItem listItem in ddlParentAccountGroup.Items)
            {
                if (listItem.Value == dataTableQry.Rows[0]["AG_Code"].ToString())
                {
                    ddlParentAccountGroup.Items.Remove(listItem);
                    break;
                }
            }
        }
        pnlAddDetail.Visible = true;
        pnlList.Visible = false;
        updadddetail.Update();
    }

    protected void grdAccountGroup_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            clsDAL clsDal = new clsDAL();
            DataTable dataTable1 = new DataTable();
            Label control = (Label)grdAccountGroup.Rows[e.RowIndex].FindControl("lbl2");
            DataTable dataTable2 = clsDal.GetDataTable("ACTS_DelAcGroup", new Hashtable()
      {
        {
           "@AG_Code",
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
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Record can not be deleted";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        FillAccountGroupsList();
        btnCancel.Enabled = true;
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "display", "ViewDivAdd();", true);
        ddlParentAccountGroup.Focus();
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

    protected void grdAccountGroup_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        if (!DataBinder.Eval(e.Row.DataItem, "AG_Type").ToString().Equals("USER"))
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
}