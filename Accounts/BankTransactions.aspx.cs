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

public partial class Accounts_BankTransactions : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            FillBank();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void FillBank()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        drpBank.DataSource = clsDal.GetDataTableQry("SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads a  inner join Acts_AccountGroups ag on ag.AG_Code=a.AG_Code WHERE ( a.AG_Code=7 or ag.AG_Parent=7 ) ORDER BY AcctsHead");
        drpBank.DataTextField = "AcctsHead";
        drpBank.DataValueField = "AcctsHeadId";
        drpBank.DataBind();
        drpBank.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (obj.ExecuteScalar("ACTS_CheckCorrectTransDate", new Hashtable()
    {
      {
         "@CurrTransDt",
         dtpTranDt.GetDateValue().ToString("dd MMM yyyy")
      },
      {
         "@AccountHead",
         drpBank.SelectedValue.ToString().Trim()
      }
    }).Trim().ToLower() == "y")
        {
            if (chkCashBal())
            {
                Hashtable hashtable = new Hashtable();
                DataTable dataTable = new DataTable();
                hashtable.Clear();
                dataTable.Clear();
                hashtable.Add("@BankHd", drpBank.SelectedValue.ToString());
                hashtable.Add("@BankNm", drpBank.SelectedItem.Text);
                hashtable.Add("@Amount", txtAmount.Text.ToString());
                hashtable.Add("@TransDate", dtpTranDt.GetDateValue().ToString("dd MMM yyyy"));
                hashtable.Add("@UserId", Session["User_Id"].ToString());
                hashtable.Add("@SchoolId", Session["SchoolId"].ToString());
                hashtable.Add("@Remarks", txtRemarks.Text.Trim());
                obj = new clsDAL();
                string s = obj.ExecuteScalarQry("select sum(DipositAmt-WithdrawlAmt) from dbo.ACTS_BankTransactions where BankAcHeadId=" + drpBank.SelectedValue.Trim() + " and (IsDeleted=0  or IsDeleted is null)");
                if (s.Trim() == "")
                    s = "0";
                if ((double)float.Parse(s) < (double)float.Parse(txtAmount.Text.ToString()) && drpTransType.SelectedValue.ToString().Trim() == "w")
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = "Insufficient  Balance to Withdraw";
                    lblMsg.ForeColor = Color.Red;
                }
                else
                {
                    string str;
                    if (drpTransType.SelectedValue.ToString().Trim() == "d" || drpTransType.SelectedValue.ToString().Trim() == "i")
                    {
                        float num = float.Parse(s) + float.Parse(txtAmount.Text.ToString());
                        hashtable.Add("@ClosingBal", num);
                        if (drpTransType.SelectedValue.ToString().Trim() == "i")
                            hashtable.Add("@IsInterest", 1);
                        str = "ACTS_InsBankDeposite";
                    }
                    else
                    {
                        float num = float.Parse(s) - float.Parse(txtAmount.Text.ToString());
                        hashtable.Add("@ClosingBal", num);
                        str = "ACTS_InsBankWithdrl";
                    }
                    try
                    {
                        obj.ExcuteProcInsUpdt(str, hashtable);
                        lblMsg.Visible = true;
                        lblMsg.Text = "Data Saved Successfully";
                        lblMsg.ForeColor = Color.Green;
                        ClearAll();
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = ex.Message.ToString();
                        lblMsg.ForeColor = Color.Red;
                    }
                }
            }
            else
            {
                lblMsg.Text = "Cash Balance not enough to complete this transaction !!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Invalid transaction date or the transaction exists for later date.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private bool chkCashBal()
    {
        if (!(drpTransType.SelectedValue.Trim() == "d"))
            return true;
        Hashtable hashtable = new Hashtable();
        obj = new clsDAL();
        hashtable.Add("@CurrTransDt", dtpTranDt.GetDateValue().ToString("dd MMM yyyy"));
        string str = obj.ExecuteScalar("ACTS_ChkCashBalance", hashtable);
        return !(str.Trim() == "") && !(Convert.ToDecimal(str.Trim()) < Convert.ToDecimal(txtAmount.Text.Trim()));
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    private void ClearAll()
    {
        drpBank.SelectedIndex = 0;
        txtAmount.Text = "";
        txtTranDt.Text = "";
        txtRemarks.Text = "";
    }

    protected void drpBank_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnViewList_Click(object sender, EventArgs e)
    {
        Response.Redirect("BankTransactionList.aspx");
    }

    protected void rBtnCash_CheckedChanged(object sender, EventArgs e)
    {
    }

    protected void rbtnBank_CheckedChanged(object sender, EventArgs e)
    {
    }
}