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

public partial class Accounts_BankTransactionList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            
            if (Page.IsPostBack)
                return;
            FillBank();
            clsDAL clsDal = new clsDAL();
            if (clsDal.ExecuteScalarQry("select top 1 StartDate from dbo.ACTS_FinancialYear where (IsFinalized is null or IsFinalized<>1) order by FY_Id desc ") != "")
                dtpfromdt.SetDateValue(DateTime.Parse(clsDal.ExecuteScalarQry("select top 1 StartDate from dbo.ACTS_FinancialYear order by FY_Id desc")));
            else
                lblMsg.Text = "Please Initialize Financial Year";
            dtpfromdt.Visible = false;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void FillBank()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        drpBank.DataSource = clsDal.GetDataTableQry("SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads a inner join Acts_AccountGroups ag on ag.AG_Code=a.AG_Code WHERE ( a.AG_Code=7 or ag.AG_Parent=7 ) ORDER BY AcctsHead");
        drpBank.DataTextField = "AcctsHead";
        drpBank.DataValueField = "AcctsHeadId";
        drpBank.DataBind();
        drpBank.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("BankTransactions.aspx");
    }

    protected void grdTransactions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTransactions.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    private void BindGrid()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        try
        {
            if (drpBank.SelectedValue.ToString().Trim() != "0")
                hashtable.Add("@BankAccountHead", Convert.ToInt32(drpBank.SelectedValue.ToString()));
            if (txtFromDt.Text != "")
                hashtable.Add("@FrmDt", dtpfromdt.GetDateValue().ToString("MM/dd/yyyy"));
            if (txtToDt.Text != "")
                hashtable.Add("@ToDt", dtptodt.GetDateValue().ToString("MM/dd/yyyy"));
            hashtable.Add("@SchoolId", Session["SchoolId"]);
            DataTable dataTable = clsDal.GetDataTable("Acts_GetBankDipWDL", hashtable);
            DataView dataView = ModDT(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                lblMsg.Text = "";
                grdTransactions.DataSource = dataView;
                grdTransactions.DataBind();
                grdTransactions.Visible = true;
                lblRecord.Text = "Total Record : " + dataTable.Rows.Count.ToString();
                float num1 = float.Parse(dataTable.Compute("Sum(Diposit)", "Remarks is null OR Remarks<>'Ac Intitialized'").ToString());
                float num2 = float.Parse(dataTable.Compute("Sum(Withdrwal)", "Remarks is null OR Remarks<>'Ac Intitialized'").ToString());
                lblDip.Text = "TOTAL DIPOSIT DURING THE PERIOD: Rs. " + Math.Round((double)num1, 2).ToString();
                lblWDL.Text = "TOTAL WITHDRAWL DURING THE PERIOD: Rs. " + Math.Round((double)num2, 2).ToString();
            }
            else
            {
                grdTransactions.Visible = false;
                lblRecord.Text = lblDip.Text = lblWDL.Text = string.Empty;
                lblMsg.Text = "No Records Found";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    protected DataView ModDT(DataTable dt1)
    {
        DataTable dataTable = new DataTable();
        clsDAL clsDal = new clsDAL();
        Decimal num = new Decimal(0);
        if (dt1.Rows.Count > 1)
        {
            for (int index = 0; index < dt1.Rows.Count; ++index)
            {
                num = num + Decimal.Parse(dt1.Rows[index]["Diposit"].ToString()) - Decimal.Parse(dt1.Rows[index]["Withdrwal"].ToString());
                dt1.Rows[index]["ClosingBal"] = num;
            }
        }
        dt1.AcceptChanges();
        DataView defaultView = dt1.DefaultView;
        defaultView.Sort = "TransDate desc, TransId desc";
        return defaultView;
    }

    protected void grdTransactions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            clsDAL clsDal1 = new clsDAL();
            DataTable dataTable = new DataTable();
            Hashtable hashtable = new Hashtable();
            Label control = (Label)grdTransactions.Rows[e.RowIndex].FindControl("lblTransId");
            if (control.Text.Trim().Trim() == control.Text.Trim())
            {
                clsDAL clsDal2 = new clsDAL();
                hashtable.Add("@BankTransId", control.Text);
                hashtable.Add("@UserId", Session["User_Id"].ToString());
                string str = clsDal2.ExecuteScalar("ACTS_DelBankTrans", hashtable);
                BindGrid();
                if (str == "false")
                {
                    lblMsg.Text = "The selected transaction can not be deleted. Since Payment Encashed ";
                    lblMsg.ForeColor = Color.Red;
                }
                else
                {
                    lblMsg.Text = "Record Deleted Successfully ";
                    lblMsg.ForeColor = Color.Green;
                }
            }
            else
            {
                lblMsg.Text = "The selected transaction can not be deleted. Transaction exists after this transaction";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Dependency for this record exist ";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void grdTransactions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        Label control1 = (Label)e.Row.FindControl("TransId");
        Label control2 = (Label)e.Row.FindControl("lblBankTransRks");
        ImageButton control3 = (ImageButton)e.Row.FindControl("btnDelete");
        if (!(Session["userrights"].ToString().Trim() == "a"))
            return;
        control3.Visible = true;
        if (control2.Text.Trim() == "" || control2.Text.Trim() == "Ac Intitialized")
            control3.Visible = false;
        else
            control3.Visible = true;
    }
}