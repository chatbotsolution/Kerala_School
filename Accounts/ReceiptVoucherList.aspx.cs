using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_ReceiptVoucherList : System.Web.UI.Page
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
        ViewState["dt"] = null;
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (txtFromDt.Text != "")
            hashtable.Add("@FrmDt", dtpfromdt.GetDateValue().ToString("MM/dd/yyyy"));
        if (txtToDt.Text != "")
            hashtable.Add("@ToDt", dtptodt.GetDateValue().ToString("MM/dd/yyyy"));
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        try
        {
            DataTable dataTable = clsDal.GetDataTable("Acts_GetReceiptListAll", hashtable);
            if (dataTable.Rows.Count > 0)
            {
                ViewState["dt"] = dataTable;
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
        updadddetail.Update();
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        pnlDelReason.Visible = false;
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
            Label control = (Label)grdParty.Rows[e.RowIndex].FindControl("lbl2");
            DataRow[] dataRowArray = ((DataTable)ViewState["dt"]).Select("PR_Id=" + control.Text.Trim());
            hfRcptId.Value = control.Text.Trim();
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<div><b>Records to be deleted :-</b></div>");
            stringBuilder.Append("<table border='0px' cellpadding='3' cellspacing='3' style='border:solid black 1px;' class='tbltxt'  width='100%'><tr>");
            stringBuilder.Append("");
            stringBuilder.Append("<td width='100'>Receipt No </td><td width='5'>: </td><td width='150'>" + dataRowArray[0]["InvoiceReceiptNo"].ToString() + "</td>");
            stringBuilder.Append("<td>Recieved date </td><td>: </td><td>" + dataRowArray[0]["TransDtStr"].ToString() + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td>Recieved From </td><td>: </td><td>" + dataRowArray[0]["RcvdFrom"].ToString() + "</td>");
            stringBuilder.Append("<td>Amount </td><td>: </td><td>" + dataRowArray[0]["Amount"].ToString() + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            litDetails.Text = stringBuilder.ToString().Trim();
            pnlDelReason.Visible = true;
            grdParty.Visible = false;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Dependency for this record exist ";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReceiptVoucher.aspx");
    }

    protected void btnSaveReason_Click(object sender, EventArgs e)
    {
        string str = DeleteTrans(hfRcptId.Value.ToString());
        if (str == "")
        {
            lblMsg.Text = "Receipt cancelled successfully !";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = str;
            lblMsg.ForeColor = Color.Red;
        }
        pnlDelReason.Visible = false;
        txtDelReason.Text = string.Empty;
        grdParty.Visible = true;
        BindPaymentGrid();
    }

    private string DeleteTrans(string id)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        clsDAL clsDal = new clsDAL();
        hashtable.Add("@PR_Id", id);
        hashtable.Add("@CancelledReason", txtDelReason.Text.Trim().ToString());
        DataTable dataTable2 = clsDal.GetDataTable("ACTS_DeleteRcptVoucher", hashtable);
        return dataTable2.Rows.Count <= 0 ? "" : dataTable2.Rows[0][0].ToString();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtDelReason.Text = string.Empty;
        grdParty.Visible = true;
        pnlDelReason.Visible = false;
    }

    protected void grdParty_DataBound(object sender, EventArgs e)
    {
        if (!(Session["userrights"].ToString().Trim() == "a"))
            return;
        foreach (Control row in grdParty.Rows)
            row.FindControl("btnDelete").Visible = true;
    }
}