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


public partial class HR_LoanAdvance : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        if (!ChkIsHRUsed())
        {
            Response.Redirect("HRHome.aspx?IsHRUsed=0");
        }
        else
        {
            FillDrp(drpCreditAccount, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=7 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
            FillDrp(drpDebtors, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster Where ActiveStatus=1 ORDER BY EmpName", "EmpName", "EmpId");
            FillDrp(drpDedHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=25 order by AcctsHead", "AcctsHead", "AcctsHeadId");
            string str = obj.ExecuteScalar("HR_LastSalGenDt", new Hashtable());
            if (!(str != ""))
                return;
           
            dtpTransDt.From.Date = Convert.ToDateTime(str);
        }
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void FillDrp(DropDownList drp, string Qry, string Text, string Value)
    {
        DataTable dataTableQry = obj.GetDataTableQry(Qry.Trim());
        drp.DataSource = dataTableQry.DefaultView;
        drp.DataTextField = Text;
        drp.DataValueField = Value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    private void BindYear()
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("Year", typeof(string));
        DataRow row1 = dataTable.NewRow();
        row1["Year"] = dtpTransDt.GetDateValue().ToString("yyyy");
        dataTable.Rows.Add(row1);
        DataRow row2 = dataTable.NewRow();
        row2["Year"] = dtpTransDt.GetDateValue().AddYears(1).ToString("yyyy");
        dataTable.Rows.Add(row2);
        dataTable.AcceptChanges();
        drpYear.DataSource = dataTable;
        drpYear.DataValueField = "Year";
        drpYear.DataTextField = "Year";
        drpYear.DataBind();
        drpYear.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    private void enabletxt()
    {
        if (rbtnMode.SelectedValue == "C")
        {
            txtInstrumentNo.Enabled = false;
            txtInstrumentDt.Enabled = false;
            txtInstrumentDt.Text = string.Empty;
            txtInstrumentNo.Text = string.Empty;
            spBankNm.Visible = false;
            spInstDt.Visible = false;
            spInstNo.Visible = false;
            drpCreditAccount.SelectedIndex = 0;
            drpCreditAccount.Enabled = false;
        }
        else
        {
            txtInstrumentNo.Enabled = true;
            txtInstrumentDt.Enabled = true;
            spBankNm.Visible = true;
            spInstDt.Visible = true;
            spInstNo.Visible = true;
            drpCreditAccount.Enabled = true;
        }
    }

    protected void rbtnMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        enabletxt();
        rbtnMode.Focus();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (rbtnMode.SelectedValue == "B" && (drpCreditAccount.SelectedIndex == 0 || txtInstrumentNo.Text.Trim() == "" || txtInstrumentDt.Text.Trim() == ""))
        {
            lblMsg.Text = "Please Enter Bank Name / Instrument Date / Instrument No";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            if (rdbtnlstRecovery.SelectedValue == "S")
            {
                if (txtPrincipalEMI.Text.Trim() == string.Empty || Convert.ToDecimal(txtPrincipalEMI.Text) == new Decimal(0))
                {
                    lblMsg.Text = "Enter No of EMI for Principal Recovery";
                    lblMsg.ForeColor = Color.Red;
                    txtPrincipalEMI.Focus();
                    return;
                }
                if (txtInterestEMI.Text.Trim() != string.Empty && Convert.ToDecimal(txtInterestEMI.Text) > new Decimal(0) && (txtInterest.Text.Trim() == string.Empty || Convert.ToDecimal(txtInterest.Text) == new Decimal(0)))
                {
                    lblMsg.Text = "Enter Interest Amount";
                    lblMsg.ForeColor = Color.Red;
                    txtInterest.Focus();
                    return;
                }
                if (txtInterest.Text.Trim() != string.Empty && Convert.ToDecimal(txtInterest.Text) > new Decimal(0) && (txtInterestEMI.Text.Trim() == string.Empty || Convert.ToDecimal(txtInterestEMI.Text) == new Decimal(0)))
                {
                    lblMsg.Text = "Enter No of EMI for Interest Recovery";
                    lblMsg.ForeColor = Color.Red;
                    txtInterestEMI.Focus();
                    return;
                }
                if (Convert.ToInt32(drpMonth.SelectedValue) < dtpTransDt.GetDateValue().Month && Convert.ToInt32(drpYear.SelectedValue) <= dtpTransDt.GetDateValue().Year)
                {
                    lblMsg.Text = "Deduction Start From Month shouldn't be the Previous Month of Transaction Date.";
                    lblMsg.ForeColor = Color.Red;
                    drpMonth.Focus();
                    return;
                }
            }
            if (chkCashBal())
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Clear();
                hashtable.Add("@TransDate", dtpTransDt.GetDateValue());
                hashtable.Add("@EmpId", drpDebtors.SelectedValue);
                hashtable.Add("@LoanAcHeadId", drpDedHead.SelectedValue);
                if (rbtnMode.SelectedValue == "B")
                {
                    hashtable.Add("@PayMode", "Bank");
                    hashtable.Add("@CrId", drpCreditAccount.SelectedValue);
                    hashtable.Add("@InstrumentNo", txtInstrumentNo.Text.Trim());
                    hashtable.Add("@InstrumentDt", dtpInstrumentDt.GetDateValue());
                }
                else
                {
                    hashtable.Add("@PayMode", "Cash");
                    hashtable.Add("@CrId", 3);
                }
                hashtable.Add("@PrincipalAmt", Convert.ToDecimal(txtAmount.Text.Trim()));
                hashtable.Add("@RecoveryMode", rdbtnlstRecovery.SelectedValue);
                if (rdbtnlstRecovery.SelectedValue == "S")
                {
                    hashtable.Add("@InterestAmt", Convert.ToDecimal(txtInterest.Text.Trim()));
                    hashtable.Add("@NoOfPrincipalEMI", Convert.ToInt32(txtPrincipalEMI.Text.Trim()));
                    hashtable.Add("@NoOfInterestEMI", Convert.ToInt32(txtInterestEMI.Text.Trim()));
                }
                if (txtDetails.Text.Trim() != string.Empty)
                    hashtable.Add("@LoanDtls", txtDetails.Text.Trim());
                else
                    hashtable.Add("@LoanDtls", drpDedHead.SelectedItem.Text);
                hashtable.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
                hashtable.Add("@SchoolId", Session["SchoolId"]);
                hashtable.Add("@CalMonth", drpMonth.SelectedItem.Text.ToString().ToUpper());
                hashtable.Add("@CalYear", drpYear.SelectedItem.Text.ToString());
                string str = obj.ExecuteScalar("HR_InsLoanAdvance", hashtable);
                if (str == "SUCCESS")
                {
                    Clear();
                    lblMsg.Text = "Record Submitted";
                    lblMsg.ForeColor = Color.Green;
                }
                else
                {
                    lblMsg.Text = str;
                    lblMsg.ForeColor = Color.Red;
                }
            }
            else
            {
                lblMsg.Text = "Cash Balance not enough to complete this transaction !!";
                lblMsg.ForeColor = Color.Red;
            }
        }
    }

    private bool chkCashBal()
    {
        if (!(rbtnMode.SelectedValue.Trim() == "C"))
            return true;
        Hashtable hashtable = new Hashtable();
        obj = new clsDAL();
        hashtable.Add("@CurrTransDt", dtpTransDt.GetDateValue().ToString("dd MMM yyyy"));
        string str = obj.ExecuteScalar("ACTS_ChkCashBalance", hashtable);
        return !(str.Trim() == "") && !(Convert.ToDecimal(str.Trim()) < Convert.ToDecimal(txtAmount.Text.Trim()) + Convert.ToDecimal(txtInterest.Text.Trim()));
    }

    private void Clear()
    {
        hfIsDirect.Value = "0";
        lblMsg.Text = string.Empty;
        txtTransactionDate.Text = txtDetails.Text = txtInstrumentNo.Text = string.Empty;
        drpDebtors.SelectedIndex = 0;
        drpDedHead.SelectedIndex = 0;
        drpCreditAccount.SelectedIndex = 0;
        rbtnMode.SelectedValue = "B";
        rdbtnlstRecovery.SelectedValue = "S";
        trInt.Visible = true;
        txtAmount.Text = txtInterest.Text = txtPrincipalEMI.Text = txtInterestEMI.Text = "0";
        drpCreditAccount.Enabled = true;
        txtInstrumentNo.Enabled = true;
        txtInstrumentDt.Enabled = true;
        txtTransactionDate.Focus();
        drpMonth.SelectedIndex = 0;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void rdbtnlstRecovery_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtInterest.Text = "0";
        txtPrincipalEMI.Text = "0";
        txtInterestEMI.Text = "0";
        drpMonth.SelectedIndex = 0;
        if (rdbtnlstRecovery.SelectedValue == "D")
        {
            hfIsDirect.Value = "1";
            trInt.Visible = false;
        }
        else
        {
            hfIsDirect.Value = "0";
            trInt.Visible = true;
        }
        rdbtnlstRecovery.Focus();
    }

    protected void drpDedHead_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChkOutStandingAmt();
        drpDedHead.Focus();
    }

    protected void drpDebtors_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChkOutStandingAmt();
        drpDebtors.Focus();
    }

    private void ChkOutStandingAmt()
    {
        lblMsg.Text = string.Empty;
        if (drpDedHead.SelectedIndex <= 0 || drpDebtors.SelectedIndex <= 0)
            return;
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@EmpId", drpDebtors.SelectedValue);
        hashtable.Add("@LoanAcHeadId", drpDedHead.SelectedValue);
        string str = obj.ExecuteScalar("HR_CheckRepayAmt", hashtable);
        if (str.Trim() == string.Empty)
        {
            lblMsg.Text = "No OutStanding Amount";
            lblMsg.ForeColor = Color.Green;
            btnSubmit.Enabled = true;
        }
        else
        {
            lblMsg.Text = str;
            lblMsg.ForeColor = Color.Red;
            btnSubmit.Enabled = false;
        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("LoanAdvanceList.aspx");
    }

    protected void dtpTransDt_SelectionChanged(object sender, EventArgs e)
    {
        BindYear();
    }
}