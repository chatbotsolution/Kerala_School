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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_MiscExpensesEntry : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private DataSet ds = new DataSet();
    private DateTime TransDt = new DateTime();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
            Response.Redirect("../Login.aspx");
        if (!Page.IsPostBack)
        {
            bindDropDown(drpCreditHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads a inner join dbo.Acts_AccountGroups ag on ag.AG_Code=a.AG_Code where (a.AG_Code=7 or ag.AG_Parent=7) ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
            SPbindDropDown(drpAcHead, "ACTS_GetExpenseAcHeads", "AcctsHead", "AcctsHeadId");
            txtInstrumentNo.Enabled = true;
            txtInstrumentDt.Enabled = true;
            if (Request.QueryString["ExpId"] != null && Request.QueryString["ET"] != null)
            {
                FillDetails();
                ViewState["IsSalHead"] = 0;
            }
        }
        lblMsg.Text = string.Empty;
    }

    private void FillDetails()
    {
        Hashtable hashtable = new Hashtable();
        DataSet dataSet1 = new DataSet();
        hashtable.Add("@ExpId", Request.QueryString["ExpId"].ToString().Trim());
        hashtable.Add("@Type", Request.QueryString["ET"].ToString().Trim());
        DataSet dataSet2 = obj.GetDataSet("ACTS_GetMiscExpEdit", hashtable);
        DataTable table = dataSet2.Tables[0];
        if (Request.QueryString["ET"].ToString().Trim() == "CB")
        {
            optCashBank.Checked = true;
            optCredit.Checked = false;
            optCashBank_CheckedChanged(optCashBank, EventArgs.Empty);
            if (table.Rows[0]["PmtMode"].ToString().Trim() == "Cash")
                rbtnMode.SelectedValue = "C";
            else
                rbtnMode.SelectedValue = "B";
            rbtnMode_SelectedIndexChanged(rbtnMode, EventArgs.Empty);
            if (rbtnMode.SelectedValue == "B")
            {
                txtInstrumentNo.Text = table.Rows[0]["InstrumentNo"].ToString().Trim();
                txtInstrumentDt.Text = table.Rows[0]["InstrumentDt"].ToString().Trim();
            }
        }
        else
        {
            optCredit.Checked = true;
            optCashBank.Checked = false;
            optCashBank_CheckedChanged(optCredit, EventArgs.Empty);
        }
        rbtnMode.Enabled = false;
        optCredit.Enabled = false;
        optCashBank.Enabled = false;
        drpAcHead.SelectedValue = table.Rows[0]["ExpAcHeadId"].ToString().Trim();
        drpCreditHead.SelectedValue = table.Rows[0]["CrAccHdId"].ToString().Trim();
        txtRcptVoucherNo.Text = table.Rows[0]["RcptNo"].ToString().Trim();
        txtAmount.Text = table.Rows[0]["TransAmt"].ToString().Trim();
        txtExpDetails.Text = table.Rows[0]["ExpDetails"].ToString().Trim();
        dtpExpenseDt.SetDateValue(Convert.ToDateTime(table.Rows[0]["TransDateStr"].ToString().Trim()));
        hfExpId.Value = table.Rows[0]["ExpId"].ToString().Trim();
        if (dataSet2.Tables.Count > 1 && dataSet2.Tables[1].Rows.Count > 0)
            txtTransCharge.Text = dataSet2.Tables[1].Rows[0]["BnkChrgs"].ToString();
        btnReset.Enabled = false;
    }

    private void ChkSalHead()
    {
        if (drpAcHead.SelectedValue.Trim() == "14")
            ViewState["IsSalHead"] = 1;
        else
            ViewState["IsSalHead"] = 0;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        InsertToTable();
    }

    private void InsertToTable()
    {
        obj = new clsDAL();
        DateTime now = DateTime.Now;
        if (obj.GetDataTableQry("select FY,StartDate,EndDate from dbo.ACTS_FinancialYear where '" + dtpExpenseDt.GetDateValue().ToString("dd MMM yyyy") + "' between StartDate and EndDate and  (IsFinalized=0 or IsFinalized is null)").Rows.Count > 0)
        {
            if (chkCashBal())
            {
                if (!(getDedAmount("CHK") == ""))
                    return;
                obj = new clsDAL();
                Hashtable hashtable = new Hashtable();
                if (hfExpId.Value.ToString().Trim() != "")
                    hashtable.Add("@ExpncId", hfExpId.Value.ToString().Trim());
                hashtable.Add("@DrAccountHead", drpAcHead.SelectedValue);
                if (optCashBank.Checked && rbtnMode.SelectedValue.ToString().Trim() == "C")
                    hashtable.Add("@CrAccountHead", 3);
                else
                    hashtable.Add("@CrAccountHead", drpCreditHead.SelectedValue);
                hashtable.Add("@TransDate", dtpExpenseDt.GetDateValue().ToString("MM/dd/yyyy"));
                hashtable.Add("@Particulars", txtExpDetails.Text.Trim());
                hashtable.Add("@TransAmt", txtAmount.Text.Trim());
                hashtable.Add("@PmtRecptVoucherNo", txtRcptVoucherNo.Text.Trim());
                hashtable.Add("@UserId", Session["User_Id"]);
                hashtable.Add("@SchoolId", Session["SchoolId"]);
                if (rbtnMode.SelectedValue == "C" && optCashBank.Checked.Equals(true))
                    hashtable.Add("@PmtMode", "Cash");
                else if (rbtnMode.SelectedValue == "B" && optCashBank.Checked.Equals(true))
                    hashtable.Add("@PmtMode", "Bank");
                else
                    hashtable.Add("@PmtMode", null);
                if (txtInstrumentNo.Enabled.Equals(true))
                    hashtable.Add("@InstrumentNo", txtInstrumentNo.Text.Trim());
                if (txtInstrumentDt.Enabled.Equals(true))
                    hashtable.Add("@InstrumentDt", txtInstrumentDt.Text.Trim());
                string ExpId;
                if (optCredit.Checked)
                {
                    ExpId = obj.ExecuteScalar("ACTS_InsUpdt_MiscExpCredit", hashtable);
                }
                else
                {
                    if (txtTransCharge.Text.Trim() != "")
                        hashtable.Add("@TransCharge", txtTransCharge.Text.Trim());
                    else
                        hashtable.Add("@TransCharge", 0);
                    ExpId = obj.ExecuteScalar("ACTS_InsUpdt_MiscExp", hashtable);
                }
                if (ExpId.Trim() == "DUP")
                {
                    lblMsg.Text = "Transaction can't be done on " + txtExpDate.Text.Trim();
                    lblMsg.ForeColor = Color.Red;
                }
                else if (ExpId.Trim() != "" && ExpId.Trim() != "DUP" && pnlEmpDed.Visible)
                {
                    InsDesPaidStatus(ExpId);
                }
                else
                {
                    if (ViewState["IsSalHead"].ToString() == "1")
                    {
                        lblMsg.Text = "Data Saved Successfully. <a href='Adjustment.aspx'>Click here</a> to Enter Loan/Advance amount deducted from the Salary.";
                        lblMsg.ForeColor = Color.Green;
                    }
                    else if (Request.QueryString["ExpId"] != null)
                    {
                        Response.Redirect("MiscExpenseList.aspx");
                    }
                    else
                    {
                        lblMsg.Text = "Data Saved Successfully";
                        lblMsg.ForeColor = Color.Green;
                    }
                    ClearFields();
                }
            }
            else
            {
                lblMsg.Text = "Cash Balance not enough to complete this transaction !!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else if (dtpExpenseDt.GetDateValue().Date.CompareTo(now.Date) > 0)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "showmess", "alert('Year is not initialized');window.open ('../MasterSettings/OpenFY.aspx?id=" + dtpExpenseDt.GetDateValue().ToString("MM/dd/yyyy") + "','mywindow','menubar=1,resizable=1,width=600,height=600,scrollbars=1');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "showmess", "alert('Transaction date is not valid')", true);
            txtExpDate.Focus();
        }
    }

    private bool chkCashBal()
    {
        if (!optCashBank.Checked || !(rbtnMode.SelectedValue == "C"))
            return true;
        Hashtable hashtable = new Hashtable();
        obj = new clsDAL();
        hashtable.Add("@CurrTransDt", dtpExpenseDt.GetDateValue().ToString("dd MMM yyyy"));
        string str1 = !(txtTransCharge.Text.Trim() == "") ? txtTransCharge.Text.Trim() : "0";
        string str2 = obj.ExecuteScalar("ACTS_ChkCashBalance", hashtable);
        return !(str2.Trim() == "") && !(Convert.ToDecimal(str2.Trim()) < Convert.ToDecimal(txtAmount.Text.Trim()) + Convert.ToDecimal(str1.Trim()));
    }

    private string getDedAmount(string typ)
    {
        if (!pnlEmpDed.Visible)
            return "";
        Decimal d = new Decimal(0);
        int num = 0;
        foreach (GridViewRow row in grdEmpDed.Rows)
        {
            CheckBox control1 = (CheckBox)row.FindControl("optEmp");
            Label control2 = (Label)row.FindControl("lblAmt");
            if (control1.Checked)
            {
                d += Convert.ToDecimal(control2.Text.Trim());
                ++num;
            }
        }
        if (hfDedDesc.Value.Trim().ToUpper() == "ESI" || hfDedDesc.Value.Trim().ToUpper() == "E.S.I")
            d = Math.Ceiling(d);
        else if (rbtnRound.SelectedIndex == 1)
            d = Math.Round(d);
        if (!(typ == "CHK"))
            return d.ToString("0.00");
        if (Convert.ToDecimal(txtAmount.Text.Trim()) != d)
        {
            lblMsg.Text = "Entered Amount and Total Selected deduction amount are not equal!!";
            lblMsg.ForeColor = Color.Red;
            return "No";
        }
        if (num != 0)
            return "";
        lblMsg.Text = "Please Select an Employee";
        lblMsg.ForeColor = Color.Red;
        return "No";
    }

    private void InsDesPaidStatus(string ExpId)
    {
        Hashtable hashtable = new Hashtable();
        int num1 = 0;
        int num2 = 0;
        foreach (GridViewRow row in grdEmpDed.Rows)
        {
            CheckBox control1 = (CheckBox)row.FindControl("optEmp");
            Label control2 = (Label)row.FindControl("lblEmpId");
            if (control1.Checked)
            {
                ++num2;
                hashtable.Clear();
                hashtable.Add("@ExpId", ExpId.Trim());
                if (drpMonth.SelectedIndex > 0)
                {
                    hashtable.Add("@MonthNo", drpMonth.SelectedValue.Trim());
                    hashtable.Add("@Month", drpMonth.SelectedItem.ToString().Trim());
                }
                hashtable.Add("@EmpId", control2.Text.Trim());
                hashtable.Add("@AccountHead", drpAcHead.SelectedValue);
                if (obj.ExecuteScalar("Acts_UpdtEmpDedStatus", hashtable) == "")
                    ++num1;
            }
        }
        if (num2 != num1)
            return;
        lblMsg.Text = "Data Saved Successfully";
        lblMsg.ForeColor = Color.Green;
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
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    private void SPbindDropDown(DropDownList drp, string SP, string textfield, string valuefield)
    {
        obj = new clsDAL();
        DataTable dataTable = obj.GetDataTable(SP);
        drp.DataSource = dataTable;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    private void ClearFields()
    {
        optCashBank.Checked = true;
        optCredit.Checked = false;
        rbtnMode.SelectedValue = "B";
        rbtnMode.Enabled = true;
        drpCreditHead.SelectedIndex = -1;
        drpCreditHead.Enabled = true;
        drpAcHead.SelectedIndex = -1;
        txtInstrumentDt.Text = string.Empty;
        txtInstrumentNo.Text = string.Empty;
        txtInstrumentNo.Enabled = true;
        txtInstrumentDt.Enabled = true;
        txtRcptVoucherNo.Text = string.Empty;
        txtTransCharge.Enabled = true;
        txtTransCharge.Text = "";
        txtExpDate.Text = string.Empty;
        txtExpDetails.Text = string.Empty;
        txtAmount.Text = string.Empty;
        bindDropDown(drpCreditHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=7 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
        ViewState["IsSalHead"] = null;
        grdEmpDed.DataSource = null;
        grdEmpDed.DataBind();
        pnlEmpDed.Visible = false;
        hfDedDesc.Value = "";
    }

    protected void btnShowList_Click(object sender, EventArgs e)
    {
        Response.Redirect("MiscExpenseList.aspx");
    }

    protected void optCashBank_CheckedChanged(object sender, EventArgs e)
    {
        if (optCashBank.Checked.Equals(true))
        {
            rbtnMode.Enabled = true;
            rbtnMode.SelectedValue = "B";
            txtInstrumentDt.Enabled = true;
            txtInstrumentNo.Enabled = true;
            txtTransCharge.Enabled = true;
            bindDropDown(drpCreditHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=7 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
            optCashBank.Focus();
            if (drpAcHead.SelectedIndex <= 0)
                return;
            GetDeductions("0");
        }
        else
        {
            rbtnMode.Enabled = false;
            txtInstrumentDt.Enabled = false;
            txtInstrumentNo.Enabled = false;
            txtTransCharge.Enabled = false;
            bindDropDown(drpCreditHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code in(4,5,11,13) ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
            drpCreditHead.Enabled = true;
            optCredit.Focus();
            pnlEmpDed.Visible = false;
        }
    }

    protected void rbtnMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCreditHeads();
        rbtnMode.Focus();
    }

    private void FillCreditHeads()
    {
        clsDAL clsDal = new clsDAL();
        if (rbtnMode.SelectedValue == "C")
        {
            drpCreditHead.Items.Clear();
            drpCreditHead.Enabled = false;
            txtInstrumentDt.Enabled = false;
            txtInstrumentNo.Enabled = false;
        }
        else
        {
            txtInstrumentDt.Enabled = true;
            txtInstrumentNo.Enabled = true;
            drpCreditHead.Enabled = true;
            string str = "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads a inner join dbo.Acts_AccountGroups ag on ag.AG_Code=a.AG_Code where (a.AG_Code=7 or ag.AG_Parent=7) ORDER BY AcctsHead";
            drpCreditHead.DataSource = clsDal.GetDataTableQry(str.ToString().Trim()).DefaultView;
            drpCreditHead.DataTextField = "AcctsHead";
            drpCreditHead.DataValueField = "AcctsHeadId";
            drpCreditHead.DataBind();
            drpCreditHead.Items.Insert(0, new ListItem("---Select---", "0"));
        }
    }

    private void enabletxt()
    {
        if (rbtnMode.SelectedValue == "C")
        {
            txtInstrumentNo.Enabled = false;
            txtInstrumentDt.Enabled = false;
            txtInstrumentDt.Text = string.Empty;
            txtInstrumentNo.Text = string.Empty;
        }
        else
        {
            txtInstrumentNo.Enabled = true;
            txtInstrumentDt.Enabled = true;
        }
    }

    protected void drpAcHead_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (drpAcHead.SelectedIndex > 0)
        {
            ChkSalHead();
            if (optCashBank.Checked)
                GetDeductions("0");
            else
                pnlEmpDed.Visible = false;
        }
        else
        {
            pnlEmpDed.Visible = false;
            txtAmount.Text = "";
        }
        drpAcHead.Focus();
    }

    private void GetDeductions(string Init)
    {
        obj = new clsDAL();
        string str = obj.ExecuteScalarQry("SELECT count(*) from dbo.HR_EmpDedMaster where AcctsHeadId=" + drpAcHead.SelectedValue.Trim());
        if (str.Trim() != "" && (int)Convert.ToInt16(str.Trim()) > 0)
        {
            Hashtable hashtable = new Hashtable();
            DataSet dataSet1 = new DataSet();
            hashtable.Add("@AcHead", drpAcHead.SelectedValue.Trim());
            if (Init.Trim() == "")
            {
                hashtable.Add("@Monthno", drpMonth.SelectedValue.Trim());
                hashtable.Add("@Month", drpMonth.SelectedItem.Text.Trim());
            }
            DataSet dataSet2 = obj.GetDataSet("Acts_GetEmpDedPayble", hashtable);
            if (dataSet2.Tables[0].Rows.Count > 0)
            {
                hfDedDesc.Value = dataSet2.Tables[0].Rows[0]["DedDesc"].ToString().Trim();
                grdEmpDed.DataSource = dataSet2.Tables[0];
                grdEmpDed.DataBind();
                if (hfDedDesc.Value.Trim().ToUpper() == "ESI" || hfDedDesc.Value.Trim().ToUpper() == "E.S.I")
                {
                    rbtnRound.Visible = false;
                    txtAmount.Text = Math.Ceiling(Convert.ToDecimal(dataSet2.Tables[0].Compute("Sum(Amount)", "").ToString().Trim())).ToString();
                }
                else
                {
                    rbtnRound.Visible = true;
                    txtAmount.Text = rbtnRound.SelectedIndex != 0 ? Math.Round(Convert.ToDecimal(dataSet2.Tables[0].Compute("Sum(Amount)", "").ToString().Trim())).ToString() : Convert.ToDecimal(dataSet2.Tables[0].Compute("Sum(Amount)", "").ToString().Trim()).ToString();
                }
                pnlEmpDed.Visible = true;
                if (dataSet2.Tables.Count <= 1)
                    return;
                drpMonth.ClearSelection();
                if (dataSet2.Tables[1].Rows.Count > 0 && dataSet2.Tables[1].Rows[0][0].ToString().Trim() != "")
                    drpMonth.Items.FindByText(dataSet2.Tables[1].Rows[0][0].ToString().Trim()).Selected = true;
                else
                    drpMonth.SelectedIndex = 0;
            }
            else
            {
                hfDedDesc.Value = "";
                grdEmpDed.DataSource = null;
                grdEmpDed.DataBind();
                pnlEmpDed.Visible = false;
                if (!(hfExpId.Value.Trim() == ""))
                    return;
                txtAmount.Text = "";
            }
        }
        else
        {
            hfDedDesc.Value = "";
            grdEmpDed.DataSource = null;
            grdEmpDed.DataBind();
            pnlEmpDed.Visible = false;
            if (!(hfExpId.Value.Trim() == ""))
                return;
            txtAmount.Text = "";
        }
    }

    protected void drpMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetDeductions("");
    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        txtAmount.Text = getDedAmount("");
    }

    protected void optEmp_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        CheckBox control = (CheckBox)grdEmpDed.HeaderRow.FindControl("optEmpAll");
        if (checkBox.Checked)
            return;
        control.Checked = false;
    }

    protected void optEmpAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox control = (CheckBox)grdEmpDed.HeaderRow.FindControl("optEmpAll");
        foreach (Control row in grdEmpDed.Rows)
            ((CheckBox)row.FindControl("optEmp")).Checked = control.Checked;
    }

    protected void rbtnRound_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpAcHead.SelectedIndex <= 0 || !optCashBank.Checked)
            return;
        GetDeductions("0");
    }
}