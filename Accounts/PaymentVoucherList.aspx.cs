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

public partial class Accounts_PaymentVoucherList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        Page.Form.DefaultButton = btnView.UniqueID;
        lblRecord.Text = string.Empty;
        int num = Page.IsPostBack ? 1 : 0;
    }

    private void BindPaymentGrid()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (txtFromDt.Text != "")
            hashtable.Add("@FromDt", (dtpfromdt.GetDateValue().ToString("dd-MMM-yyyy") + " 00:00:00'"));
        if (txtToDt.Text != "")
            hashtable.Add("@ToDt", (dtptodt.GetDateValue().ToString("dd-MMM-yyyy") + " 23:59:59'"));
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        DataTable dataTable = clsDal.GetDataTable("Acts_GetPaymentList", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            grdParty.DataSource = dataTable.DefaultView;
            grdParty.DataBind();
            grdParty.Visible = true;
            lblRecord.Text = "Total Record : " + dataTable.Rows.Count.ToString();
        }
        else
        {
            grdParty.Visible = false;
            lblRecord.Text = string.Empty;
            lblMsg.Text = "No Record Found !";
            lblMsg.ForeColor = Color.Red;
        }
        updadddetail.Update();
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        BindPaymentGrid();
    }

    protected void grdParty_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdParty.PageIndex = e.NewPageIndex;
        BindPaymentGrid();
    }

    protected void grdParty_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            clsDAL clsDal = new clsDAL();
            DataTable dataTable1 = new DataTable();
            Hashtable hashtable = new Hashtable();
            Label control = (Label)grdParty.Rows[e.RowIndex].FindControl("lbl2");
            hashtable.Add("@PR_Id", control.Text);
            DataTable dataTable2 = clsDal.GetDataTable("ACTS_DeletePmntVoucher", hashtable);
            if (dataTable2.Rows.Count > 0)
            {
                lblMsg.Text = dataTable2.Rows[0][0].ToString();
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                BindPaymentGrid();
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("PaymentVoucher.aspx");
    }

    protected void grdParty_DataBound(object sender, EventArgs e)
    {
        if (!(Session["userrights"].ToString().Trim() == "a"))
            return;
        foreach (Control row in grdParty.Rows)
            row.FindControl("btnDelete").Visible = true;
    }
}