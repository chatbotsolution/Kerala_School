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

public partial class Accounts_AdvReceiptVoucherList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        int num = Page.IsPostBack ? 1 : 0;
    }

    private void BindPaymentGrid()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (txtFromDt.Text != "")
            hashtable.Add("@FrmDt", dtpfromdt.GetDateValue().ToString("MM/dd/yyyy"));
        if (txtToDt.Text != "")
            hashtable.Add("@ToDt", dtptodt.GetDateValue().ToString("MM/dd/yyyy"));
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        try
        {
            DataTable dataTable = clsDal.GetDataTable("Acts_GetReceiptListAdv", hashtable);
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
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
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
            DataTable dataTable2 = clsDal.GetDataTable("ACTS_DelRcptVoucherAdv", hashtable);
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
        Response.Redirect("AdvReceiptVoucher.aspx");
    }
}