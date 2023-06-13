using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_PaymentVoucher : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DateTime TransDt = new DateTime();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        FillCreditHeads();
        FillDebitHeads();
        enabletxt();
        txtTransactionDate.Focus();
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
        }
        else
        {
            txtInstrumentNo.Enabled = true;
            txtInstrumentDt.Enabled = true;
            spBankNm.Visible = true;
            spInstDt.Visible = true;
            spInstNo.Visible = true;
        }
    }

    private void filldate()
    {
        try
        {
            TransDt = Convert.ToDateTime(obj.ExecuteScalarQry("select CONVERT(nvarchar,TransDate,106) from dbo.ACTS_MasterControl where ActsClosed=0"));
            PopCalendar2.SetDateValue(TransDt);
        }
        catch (Exception ex)
        {
            PopCalendar2.SetDateValue(DateTime.Now);
        }
    }

    private void FillDebitHeads()
    {
        drpDebtors.DataSource = new clsDAL().GetDataTable("ACTS_GetPmtRcptAcHeads").DefaultView;
        drpDebtors.DataTextField = "AcctsHead";
        drpDebtors.DataValueField = "AcctsHeadId";
        drpDebtors.DataBind();
        drpDebtors.Items.Insert(0, new ListItem("Select Debit Head", "0"));
    }

    private void FillCreditHeads()
    {
        clsDAL clsDal = new clsDAL();
        if (rbtnMode.SelectedValue == "C")
        {
            drpCreditAccount.Items.Clear();
            drpCreditAccount.Enabled = false;
        }
        else
        {
            drpCreditAccount.Enabled = true;
            drpCreditAccount.DataSource = clsDal.GetDataTable("ACTS_GetBankAcHeads").DefaultView;
            drpCreditAccount.DataTextField = "AcctsHead";
            drpCreditAccount.DataValueField = "AcctsHeadId";
            drpCreditAccount.DataBind();
            drpCreditAccount.Items.Insert(0, new ListItem("Select Bank Name", "0"));
        }
    }

    protected void rbtnMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ScriptManager control = (ScriptManager)Master.FindControl("ScriptManager1");
        FillCreditHeads();
        enabletxt();
        control.SetFocus(rbtnMode.ClientID);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime now = DateTime.Now;
            DataTable dataTableQry = obj.GetDataTableQry("select FY,StartDate,EndDate from dbo.ACTS_FinancialYear where '" + PopCalendar2.GetDateValue().ToString("dd MMM yyyy") + "' between StartDate and EndDate and  (IsFinalized=0 or IsFinalized is null)");
            if (dataTableQry.Rows.Count > 0)
            {
                if (chkCashBal())
                {
                    obj = new clsDAL();
                    string str1 = obj.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=7");
                    string str2 = obj.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=17");
                    if (str1.ToString().Trim() == "")
                        ScriptManager.RegisterStartupScript((Page)this, GetType(), "alertMessage", "alert('No Account Head Set For Bank Charges!! Please Set Account Head To Continue!!'); window.location='" + Request.ApplicationPath + "/Masters/SetAccountHead.aspx'", true);
                    else if (str2.ToString().Trim() == "")
                    {
                        ScriptManager.RegisterStartupScript((Page)this, GetType(), "alertMessage", "alert('No Account Head Set For TDS!! Please Set Account Head To Continue!!'); window.location='" + Request.ApplicationPath + "/Masters/SetAccountHead.aspx'", true);
                    }
                    else
                    {
                        obj = new clsDAL();
                        DataTable dataTable1 = new DataTable();
                        Hashtable hashtable = new Hashtable();
                        hashtable.Add("@FY", dataTableQry.Rows[0]["FY"].ToString());
                        hashtable.Add("@TransDate", checkDate(PopCalendar2.GetDateValue().ToString("dd MMM yyyy")));
                        if (txtRcptVoucherNo.Text.Trim() != "")
                            hashtable.Add("@ReceiptVoucherNo", txtRcptVoucherNo.Text.Trim());
                        hashtable.Add("@AccountHeadDr", drpDebtors.SelectedValue.ToString().Trim());
                        if (rbtnMode.SelectedValue == "C")
                            hashtable.Add("@AccountHeadCr", 3);
                        else
                            hashtable.Add("@AccountHeadCr", drpCreditAccount.SelectedValue.ToString());
                        hashtable.Add("@TransAmt", txtAmount.Text.ToString().Trim());
                        if (txtTransCharge.Text.Trim() != "")
                            hashtable.Add("@TransCharge", txtTransCharge.Text.Trim());
                        if (chkTds.Checked && txtTdsAmt.Text.Trim() != "" && Convert.ToDecimal(txtTdsAmt.Text.Trim()) > new Decimal(0))
                            hashtable.Add("@TDSAmt", txtTdsAmt.Text.Trim());
                        hashtable.Add("@Description", txtDetails.Text.Trim());
                        if (txtInstrumentNo.Text.Trim() != "")
                            hashtable.Add("@InstrumentNo", txtInstrumentNo.Text.ToString().Trim());
                        if (txtInstrumentDt.Text.Trim() != "")
                            hashtable.Add("@InstrumentDt", checkDate(txtInstrumentDt.Text));
                        hashtable.Add("@UserId", Session["User_Id"]);
                        hashtable.Add("@SchoolId", Session["SchoolId"]);
                        DataTable dataTable2 = obj.GetDataTable("ACTS_InsertPaymentVoucher", hashtable);
                        if (dataTable2.Rows[0][0].ToString() == "ERROR")
                        {
                            lblMsg.Text = "TRANSACTION FAILED,PLEASE TRY AGAIN";
                            lblMsg.ForeColor = Color.Red;
                        }
                        else
                        {
                            ViewState["MId"] = dataTable2.Rows[0][0].ToString().Trim();
                            lblMsg.Text = "Data saved successfully !";
                            lblMsg.ForeColor = Color.Green;
                            btnPrint.Enabled = true;
                        }
                        ClearPage();
                    }
                }
                else
                {
                    lblMsg.Text = "Cash Balance not enough to complete this transaction !!";
                    lblMsg.ForeColor = Color.Red;
                }
            }
            else if (PopCalendar2.GetDateValue().Date.CompareTo(now.Date) > 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "showmess", "alert('Year is not initialized');window.open ('../MasterSettings/OpenFY.aspx?id=" + PopCalendar2.GetDateValue().ToString("MM/dd/yyyy") + "','mywindow','menubar=1,resizable=1,width=600,height=600,scrollbars=1');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "showmess", "alert('Transaction date is not valid')", true);
                txtTransactionDate.Focus();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message.ToString();
            lblMsg.ForeColor = Color.Red;
        }
    }

    private bool chkCashBal()
    {
        if (!(rbtnMode.SelectedValue == "C"))
            return true;
        Hashtable hashtable = new Hashtable();
        obj = new clsDAL();
        hashtable.Add("@CurrTransDt", PopCalendar2.GetDateValue().ToString("dd MMM yyyy"));
        string str1 = !(txtTransCharge.Text.Trim() == "") ? txtTransCharge.Text.Trim() : "0";
        string str2 = obj.ExecuteScalar("ACTS_ChkCashBalance", hashtable);
        return !(str2.Trim() == "") && !(Convert.ToDecimal(str2.Trim()) < Convert.ToDecimal(txtAmount.Text.Trim()) + Convert.ToDecimal(str1.Trim()));
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearPage();
    }

    private void ClearPage()
    {
        drpDebtors.SelectedIndex = -1;
        drpCreditAccount.SelectedIndex = -1;
        txtAmount.Text = string.Empty;
        txtInstrumentNo.Text = string.Empty;
        txtInstrumentDt.Text = string.Empty;
        txtTransactionDate.Text = string.Empty;
        txtRcptVoucherNo.Text = string.Empty;
        PopCalendar2.SetDateValue(DateTime.Now);
        FillCreditHeads();
        FillDebitHeads();
        txtInstrumentNo.Enabled = true;
        txtInstrumentDt.Enabled = true;
        txtDetails.Text = "";
        txtTransactionDate.Focus();
        txtTransCharge.Text = "";
    }

    public string checkDate(string dt2)
    {
        try
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US")
            {
                DateTimeFormat = new DateTimeFormatInfo()
                {
                    ShortDatePattern = "dd-MM-yyyy"
                }
            };
            DateTime dateTime = DateTime.Parse(dt2);
            return dateTime.Year.ToString() + "/" + ("0" + dateTime.Month).Substring(("0" + dateTime.Month).Length - 2) + "/" + ("0" + dateTime.Day).Substring(("0" + dateTime.Day).Length - 2);
        }
        catch
        {
            return "";
        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("PaymentVoucherList.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Accounts/Welcome.aspx");
    }

    protected void chkTds_CheckedChanged(object sender, EventArgs e)
    {
        if (chkTds.Checked)
        {
            txtTdsAmt.Visible = true;
            txtTdsAmt.Text = "";
            lblTds.Visible = true;
        }
        else
        {
            txtTdsAmt.Visible = false;
            lblTds.Visible = false;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["MId"].ToString().Trim();
            string str = "rptActPaymentVoucherPrint.aspx?PrId=" + ViewState["MId"].ToString().Trim();
            btnPrint.Enabled = false;
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('" + str + "');", true);
        }
        catch
        {
        }
    }
}