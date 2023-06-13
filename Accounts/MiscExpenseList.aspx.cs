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

public partial class Accounts_MiscExpenseList : System.Web.UI.Page
{
    private Hashtable ht = new Hashtable();
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Decimal totProfit;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        ClearFields();
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("All", "0"));
    }

    private void ClearFields()
    {
        Convert.ToDateTime("01/01/" + DateTime.Now.Year.ToString());
    }

    private void fillgrid()
    {
        lblMsg.Text = "";
        ht = new Hashtable();
        dt = new DataTable();
        if (dtpFromDate.GetDateValue() > dtpToDate.GetDateValue())
        {
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('From Date can not be greater than To Date');", true);
        }
        else
        {
            try
            {
                if (txtFromDate.Text.Trim() != "")
                    ht.Add("@FrmDt", (dtpFromDate.GetDateValue().ToString("MM/dd/yyyy") + " 00:00:00"));
                if (txtToDate.Text.Trim() != "")
                    ht.Add("@ToDt", (dtpToDate.GetDateValue().ToString("MM/dd/yyyy") + " 23:59:59"));
                dt = obj.GetDataTable("ACTS_GetMiscExp", ht);
                gvMiscExp.DataSource = dt;
                gvMiscExp.DataBind();
                lblNoOfRec.Text = dt.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
    }

    protected void btnshow_Click(object sender, EventArgs e)
    {
        fillgrid();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("MiscExpensesEntry.aspx");
    }

    protected void gvMiscExp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvMiscExp.PageIndex = e.NewPageIndex;
        fillgrid();
    }

    protected void gvMiscExp_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            totProfit = totProfit + Decimal.Parse(((Label)e.Row.FindControl("lblAmnt")).Text);
            if (!(Session["userrights"].ToString().Trim() == "a"))
                return;
            ImageButton control1 = (ImageButton)e.Row.FindControl("btnDelete");
            ImageButton control2 = (ImageButton)e.Row.FindControl("btnEdit");
            control1.Visible = true;
            control2.Visible = true;
        }
        else
        {
            if (e.Row.RowType != DataControlRowType.Footer)
                return;
            e.Row.Cells[2].Text = "Total: ";
            e.Row.Cells[2].ForeColor = Color.White;
            e.Row.Cells[3].Text = totProfit.ToString();
            e.Row.Cells[3].ForeColor = Color.White;
        }
    }

    protected void gvMiscExp_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            clsDAL clsDal = new clsDAL();
            Hashtable hashtable = new Hashtable();
            Label control = (Label)gvMiscExp.Rows[e.RowIndex].FindControl("lbl2");
            string str;
            if (((Label)gvMiscExp.Rows[e.RowIndex].FindControl("lblType")).Text == "C")
            {
                hashtable.Add("@ExpId", control.Text);
                str = clsDal.ExecuteScalar("ACTS_DelMiscExpCrdt", hashtable);
            }
            else
            {
                hashtable.Add("@ExpId", control.Text);
                str = clsDal.ExecuteScalar("ACTS_DelMiscExp", hashtable);
            }
            if (str.Trim().ToUpper() == "ENCASHED")
            {
                lblMsg.Text = "Record Could Not Be Deleted !!As Amount Already Enchased !!!";
                lblMsg.ForeColor = Color.Red;
            }
            else if (str.Trim() == "No")
            {
                lblMsg.Text = "Record Could Not Be Deleted !!As Transaction After This Date Exists !!!";
                lblMsg.ForeColor = Color.Red;
            }
            else if (str.Trim() == "FY")
            {
                lblMsg.Text = "Cannot Delete Record! Transaction does not belong to current Financial year !!";
                lblMsg.ForeColor = Color.Red;
            }
            else if (str.Trim() != "")
            {
                lblMsg.Text = str.Trim();
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                fillgrid();
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

    protected void gvMiscExp_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            Hashtable hashtable = new Hashtable();
            gvMiscExp.EditIndex = e.NewEditIndex;
            Label control1 = (Label)gvMiscExp.Rows[e.NewEditIndex].FindControl("lbl2");
            Label control2 = (Label)gvMiscExp.Rows[e.NewEditIndex].FindControl("lblType");
            if (control2.Text == "C")
                hashtable.Add("@ExpId", control1.Text);
            else
                hashtable.Add("@PR_Id", control1.Text);
            string str = obj.ExecuteScalar("ACTS_ChkExpenseStatus", hashtable);
            if (str.Trim().ToUpper() == "ENCASHED")
            {
                lblMsg.Text = "Record Could Not Be Edited !!As Amount Already Enchased !!!";
                lblMsg.ForeColor = Color.Red;
            }
            else if (str.Trim() == "No")
            {
                lblMsg.Text = "Record Could Not Be Edited !!";
                lblMsg.ForeColor = Color.Red;
            }
            else if (str.Trim() == "FY")
            {
                lblMsg.Text = "Cannot Edit Record! Transaction does not belong to current Financial year !!";
                lblMsg.ForeColor = Color.Red;
            }
            else if (str.Trim() != "")
            {
                lblMsg.Text = str.Trim();
                lblMsg.ForeColor = Color.Red;
            }
            else
                Response.Redirect("MiscExpensesEntry.aspx?ExpId=" + control1.Text.Trim() + "&ET=" + control2.Text.Trim());
        }
        catch (Exception ex)
        {
        }
    }
}