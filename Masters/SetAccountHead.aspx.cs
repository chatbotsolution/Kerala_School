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

public partial class Masters_SetAccountHead : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
            FillGrid();
        else
            Response.Redirect("../Login.aspx");
    }

    private void FillGrid()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        grdAdFees.DataSource = clsDal.GetDataTable("Ps_Sp_GetAccountHeads");
        grdAdFees.DataBind();
    }

    protected void lnkbtnAccHead_Click(object sender, EventArgs e)
    {
        GridViewRow parent = (GridViewRow)((Control)sender).Parent.Parent;
        int int32 = Convert.ToInt32(grdAdFees.DataKeys[parent.DataItemIndex].Value);
        DropDownList control = (DropDownList)parent.FindControl("drpAccHeads");
        if (control.SelectedIndex > 0)
        {
            clsDAL clsDal = new clsDAL();
            Hashtable ht = new Hashtable();
            DataTable dataTable = new DataTable();
            ht.Add("@Ad_Id", int32);
            ht.Add("@AcctsHeadId", control.SelectedValue.Trim());
            if (clsDal.GetDataTable("Ps_Sp_UpdtAccHeadIdInAddFee", ht).Rows.Count > 0)
            {
                lblMsg.Text = "Account Head is already assigned !";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                FillGrid();
                lblMsg.Text = "Data Saved Successfully !";
                lblMsg.ForeColor = Color.Green;
            }
        }
        else
        {
            lblMsg.Text = "Please select Account Head !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void grdAdFees_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        DropDownList control = (DropDownList)e.Row.FindControl("drpAccHeads");
        clsDAL clsDal = new clsDAL();
        control.DataSource = clsDal.GetDataTableQry("select AcctsHeadId, AcctsHead from dbo.Acts_AccountHeads a inner join dbo.Acts_AccountGroups ag on ag.AG_Code=a.AG_Code order by AcctsHead");
        control.DataTextField = "AcctsHead";
        control.DataValueField = "AcctsHeadId";
        control.DataBind();
        control.Items.Insert(0, new ListItem("-Select-", ""));
        int int32 = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "AcctsHeadId"));
        if (int32 == 0)
            return;
        control.SelectedValue = int32.ToString();
        control.BackColor = ColorTranslator.FromHtml("#E5FFCE");
    }

    protected void drpAccHeads_SelectedIndexChanged(object sender, EventArgs e)
    {
        ((Control)sender).Parent.Parent.FindControl("lnkbtnAccHead").Visible = true;
    }
}