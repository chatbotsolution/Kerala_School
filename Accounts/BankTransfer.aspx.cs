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

public partial class Accounts_BankTransfer : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            BindDrpBankFrom();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void BindDrpBankFrom()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        drpBankFrom.DataSource = (object)clsDal.GetDataTable("ACTS_GetBankAccHd", hashtable);
        drpBankFrom.DataTextField = "AcctsHead";
        drpBankFrom.DataValueField = "AcctsHeadId";
        drpBankFrom.DataBind();
        drpBankFrom.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    protected void drpBankFrom_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindDrpBankTo();
        lblMsg.Text = string.Empty;
        (Master.FindControl("ScriptManager1") as ScriptManager).SetFocus((Control)drpBankFrom);
    }

    private void BindDrpBankTo()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpBankFrom.SelectedIndex > 0)
            hashtable.Add((object)"@AccHdId", (object)drpBankFrom.SelectedValue.ToString());
        drpBankTo.DataSource = (object)clsDal.GetDataTable("ACTS_GetBankAccHd", hashtable);
        drpBankTo.DataTextField = "AcctsHead";
        drpBankTo.DataValueField = "AcctsHeadId";
        drpBankTo.DataBind();
        drpBankTo.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string str1 = dtpTranDt.GetDateValue().ToString("MM/dd/yyyy");
        string str2 = "select count(*) from dbo.ACTS_FinancialYear where CAST('" + str1 + "' AS datetime)  >= StartDate and CAST ('" + str1 + "' AS datetime)  <= EndDate and FY_Id=(select max(FY_Id) from dbo.ACTS_FinancialYear)";
        obj = new clsDAL();
        if (int.Parse(obj.ExecuteScalarQry(str2)) > 0)
        {
            Hashtable hashtable = new Hashtable();
            DataTable dataTable = new DataTable();
            hashtable.Clear();
            dataTable.Clear();
            try
            {
                hashtable.Add((object)"@TransDate", (object)dtpTranDt.GetDateValue  ().ToString("dd MMM yyyy"));
                hashtable.Add((object)"@WdlAcHead", (object)drpBankFrom.SelectedValue.ToString());
                hashtable.Add((object)"@DipAcHead", (object)drpBankTo.SelectedValue.ToString());
                hashtable.Add((object)"@WdlBankName", (object)drpBankFrom.SelectedItem.Text);
                hashtable.Add((object)"@DipBankName", (object)drpBankTo.SelectedItem.Text);
                hashtable.Add((object)"@Amount", (object)txtAmount.Text.ToString());
                hashtable.Add((object)"@UserId", (object)Session["User_Id"].ToString());
                hashtable.Add((object)"@SchoolId", (object)Session["SchoolId"].ToString());
                obj = new clsDAL();
                string s1 = obj.ExecuteScalarQry("select sum(DipositAmt-WithdrawlAmt) from dbo.ACTS_BankTransactions where BankAcHeadId=" + drpBankFrom.SelectedValue.Trim() + " and (IsDeleted=0  or IsDeleted is null)");
                if (s1.Trim() == "")
                    s1 = "0";
                float num1 = float.Parse(s1) - float.Parse(txtAmount.Text.ToString());
                hashtable.Add((object)"@WdlClosingBal", (object)num1);
                obj = new clsDAL();
                string s2 = obj.ExecuteScalarQry("select sum(DipositAmt-WithdrawlAmt) from dbo.ACTS_BankTransactions where BankAcHeadId=" + drpBankTo.SelectedValue.Trim() + " and (IsDeleted=0  or IsDeleted is null)");
                if (s2.Trim() == "")
                    s2 = "0";
                float num2 = float.Parse(s2) + float.Parse(txtAmount.Text.ToString());
                hashtable.Add((object)"@DipClosingBal", (object)num2);
                hashtable.Add((object)"@Remarks", (object)txtRemarks.Text.Trim());
                if ((double)float.Parse(s1) >= (double)float.Parse(txtAmount.Text.ToString()))
                {
                    obj = new clsDAL();
                    obj.ExcuteProcInsUpdt("ACTS_InsBankTransfer", hashtable);
                    lblMsg.Visible = true;
                    lblMsg.Text = "Data Saved Successfully";
                    lblMsg.ForeColor = Color.Green;
                    ClearAll();
                }
                else
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Insufficient  Balance to Transfer";
                    lblMsg.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = ex.Message.ToString();
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
        {
            lblMsg.Visible = true;
            lblMsg.Text = "The transaction date is not within the current financial year";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    private void ClearAll()
    {
        drpBankFrom.SelectedIndex = -1;
        drpBankTo.Items.Clear();
        drpBankTo.SelectedIndex = -1;
        txtAmount.Text = string.Empty;
        txtTranDt.Text = string.Empty;
        txtRemarks.Text = string.Empty;
    }

    protected void btnViewList_Click(object sender, EventArgs e)
    {
        Response.Redirect("BankTransactionList.aspx");
    }
}