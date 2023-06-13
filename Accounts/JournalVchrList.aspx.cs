using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Accounts_JournalVchrList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        int num = Page.IsPostBack ? 1 : 0;
        lblMsg.Text = "";
    }

    private void BindGrid()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (txtFromDt.Text != "")
            hashtable.Add("@FromDt", (dtpfromdt.GetDateValue().ToString("dd-MMM-yyyy") + " 00:00:00'"));
        if (txtToDt.Text != "")
            hashtable.Add("@ToDt", (dtptodt.GetDateValue().ToString("dd-MMM-yyyy") + " 23:59:59'"));
        DataTable dataTable = clsDal.GetDataTable("Acts_GetJournalVchrList", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            grdJrnl.DataSource = dataTable.DefaultView;
            grdJrnl.DataBind();
            grdJrnl.Visible = true;
            lblRecord.Text = "Total Record : " + dataTable.Rows.Count.ToString();
        }
        else
        {
            grdJrnl.Visible = false;
            lblRecord.Text = string.Empty;
            lblMsg.Text = "No Record Found !";
            lblMsg.ForeColor = Color.Red;
        }
        updadddetail.Update();
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("JournalVoucher.aspx");
    }

    protected void grdJrnl_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            clsDAL clsDal = new clsDAL();
            DataTable dataTable1 = new DataTable();
            Hashtable hashtable = new Hashtable();
            Label control = (Label)grdJrnl.Rows[e.RowIndex].FindControl("lblJrnlId");
            hashtable.Add("@JournalId", control.Text);
            DataTable dataTable2 = clsDal.GetDataTable("ACTS_DelJournalVoucher", hashtable);
            if (dataTable2.Rows.Count > 0)
            {
                lblMsg.Text = dataTable2.Rows[0][0].ToString();
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                BindGrid();
                lblMsg.Text = "Record Deleted Successfully ";
                lblMsg.ForeColor = Color.Green;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Dependency for this record exist ";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void grdJrnl_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdJrnl.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void grdJrnl_DataBound(object sender, EventArgs e)
    {
        if (!(Session["userrights"].ToString().Trim() == "a"))
            return;
        foreach (Control row in grdJrnl.Rows)
            row.FindControl("btnDelete").Visible = true;
    }
}