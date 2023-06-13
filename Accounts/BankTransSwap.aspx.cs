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

public partial class Accounts_BankTransSwap : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SchoolId"] == null)
            Response.Redirect("../Login.aspx");
        lblMsg.Text = string.Empty;
        lblRecord.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        FillBank();
        ViewState["PRId"] = null;
        dtpfromdt.SetDateValue(DateTime.Now.AddMonths(-1));
        dtptodt.SetDateValue(DateTime.Now);
    }

    private void FillBank()
    {
        obj = new clsDAL();
        DataTable dataTable = new DataTable();
        drpBank.DataSource = obj.GetDataTableQry("SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads ah inner join dbo.Acts_AccountGroups ag on ag.AG_Code=ah.AG_Code WHERE (ah.AG_Code in (7) or ag.AG_Parent=7) ORDER BY ah.AcctsHead");
        drpBank.DataTextField = "AcctsHead";
        drpBank.DataValueField = "AcctsHeadId";
        drpBank.DataBind();
        drpBank.Items.Insert(0, new ListItem("--All--", "0"));
    }

    private void FillGrid()
    {
        Hashtable hashtable = new Hashtable();
        obj = new clsDAL();
        DataTable dataTable1 = new DataTable();
        if (!txtFromDt.Text.Trim().Equals(string.Empty))
            hashtable.Add("@FrmDt", dtpfromdt.GetDateValue().ToString("MM/dd/yyyy"));
        if (!txtToDt.Text.Trim().Equals(string.Empty))
            hashtable.Add("@ToDt", dtptodt.GetDateValue().ToString("MM/dd/yyyy"));
        if (drpBank.SelectedIndex > 0)
            hashtable.Add("@BankAcHeadId", drpBank.SelectedValue.Trim());
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        DataTable dataTable2 = obj.GetDataTable("Acts_GetBankReceipts", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            grdDisplay.DataSource = dataTable2;
            grdDisplay.DataBind();
            lblRecord.Text = "No Of Records:- " + dataTable2.Rows.Count.ToString();
        }
        else
        {
            grdDisplay.DataSource = null;
            grdDisplay.DataBind();
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void grdDisplay_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        DropDownList control1 = (DropDownList)e.Row.FindControl("drpBankSwp");
        Label control2 = (Label)e.Row.FindControl("lblBnkAc");
        obj = new clsDAL();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = obj.GetDataTableQry("SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads ah inner join dbo.Acts_AccountGroups ag on ag.AG_Code=ah.AG_Code WHERE (ah.AG_Code in (7) or ag.AG_Parent=7) and ah.AcctsHeadId<>" + control2.Text.Trim() + " ORDER BY ah.AcctsHead");
        control1.DataSource = dataTableQry;
        control1.DataTextField = "AcctsHead";
        control1.DataValueField = "AcctsHeadId";
        control1.DataBind();
        control1.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    protected void btnSwap_Click(object sender, EventArgs e)
    {
        GridViewRow parent = (GridViewRow)((Control)sender).Parent.Parent;
        DropDownList control1 = (DropDownList)parent.FindControl("drpBankSwp");
        if (control1.SelectedIndex > 0)
        {
            HiddenField control2 = (HiddenField)parent.FindControl("hfPRNo");
            if (obj.ExecuteScalar("ACTS_UpdtRecptBank", new Hashtable()
      {
        {
           "@BankAcHeadId",
           control1.SelectedValue.Trim()
        },
        {
           "@PR_Id",
           control2.Value.Trim()
        }
      }).Trim() == "")
            {
                lblMsg.Text = "Bank Account Changed Successfully!";
                lblMsg.ForeColor = Color.Green;
                FillGrid();
            }
            else
            {
                lblMsg.Text = "Could not Changed Bank Account!!Please Try Again!";
                lblMsg.ForeColor = Color.Green;
            }
        }
        else
        {
            lblMsg.Text = "Please Select Credit Bank Account Head!!";
            lblMsg.ForeColor = Color.Red;
        }
    }
}