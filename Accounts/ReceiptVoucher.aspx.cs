using AjaxControlToolkit;
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

public partial class Accounts_ReceiptVoucher : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DateTime TransDt = new DateTime();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        FillCreditHeads();
        bindDropDown(drpBankAc, "ACTS_GetBankAcHeads", "AcctsHead", "AcctsHeadId");
        bindDropDown(drpParty, "ACTS_GetPmtRcptAcHeads", "AcctsHead", "AcctsHeadId");
        BindRcptAcHead();
        enabletxt();
        drpParty.Visible = false;
        txtGiver.Visible = true;
        drpAcHead.Enabled = true;
        txtTransactionDate.Focus();
    }

    private void BindRcptAcHead()
    {
        obj = new clsDAL();
        drpAcHead.DataSource = obj.GetDataTable("ACTS_GetReceiptAcHead");
        drpAcHead.DataTextField = "AcctsHead";
        drpAcHead.DataValueField = "AcctsHeadId";
        drpAcHead.DataBind();
        drpAcHead.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    private void enabletxt()
    {
        if (rbtnMode.SelectedValue == "B")
        {
            spBankNm.Visible = true;
            txtInstrumentNo.Enabled = true;
            spInstNo.Visible = true;
            txtInstrumentDt.Enabled = true;
            spInstDt.Visible = true;
            drpBankAc.Enabled = true;
            drpBankAc.SelectedIndex = -1;
            txtDrawnBank.Enabled = true;
            spDrawn.Visible = true;
            rbtnMode.Focus();
        }
        else
        {
            drpBankAc.Enabled = false;
            drpBankAc.SelectedIndex = -1;
            spBankNm.Visible = false;
            txtInstrumentNo.Enabled = false;
            txtInstrumentNo.Text = string.Empty;
            spInstNo.Visible = false;
            txtInstrumentDt.Enabled = false;
            txtInstrumentDt.Text = string.Empty;
            spInstDt.Visible = false;
            txtDrawnBank.Enabled = false;
            spDrawn.Visible = false;
            rbtnMode.Focus();
        }
    }

    private void filldate()
    {
        try
        {
            TransDt = Convert.ToDateTime(obj.ExecuteScalarQry("select CONVERT(nvarchar,TransDate,106) from dbo.ACTS_MasterControl where ActsClosed=0"));
            dtpTransDt.SetDateValue(TransDt);
        }
        catch (Exception ex)
        {
            dtpTransDt.SetDateValue(DateTime.Now);
        }
    }

    protected void rbtnMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        enabletxt();
    }

    private void FillCreditHeads()
    {
        if (rbtnMode.SelectedValue == "C")
        {
            drpBankAc.Items.Clear();
            drpBankAc.Enabled = false;
            txtInstrumentDt.Enabled = false;
            txtInstrumentNo.Enabled = false;
            txtDrawnBank.Enabled = false;
        }
        else
        {
            txtInstrumentDt.Enabled = true;
            txtInstrumentNo.Enabled = true;
            txtDrawnBank.Enabled = true;
            drpBankAc.Enabled = true;
            bindDropDown(drpBankAc, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=7 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (obj.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=11").ToString().Trim() == "")
            {
                ScriptManager.RegisterStartupScript((Page)this, GetType(), "alertMessage", "alert('No Account Head Set For Bank Intrest!! Please Set Account Head To Continue!!'); window.location='" + Request.ApplicationPath + "/Masters/SetAccountHead.aspx'", true);
            }
            else
            {
                string text1 = drpParty.Text;
                string text2 = drpParty.Items[drpParty.SelectedIndex].Text;
                string text3 = drpParty.SelectedItem.Text;
                DateTime now = DateTime.Now;
                if (txtTransactionDate.Text.Trim() == "")
                {
                    lblMsg.Text = "Please Select Transaction Date";
                    lblMsg.ForeColor = Color.Red;
                }
                else
                {
                    DataTable dataTableQry = obj.GetDataTableQry("select top(1) FY,StartDate,EndDate from dbo.ACTS_FinancialYear where (IsFinalized is null or IsFinalized=0) and fy=dbo.fuGetSessionYr('" + dtpTransDt.GetDateValue().ToString("dd-MMM-yyyy") + "') order by FY_Id desc");
                    string str1 = "select max(TransDate) from dbo.Acts_GenLedger where AccountHead=" + drpBankAc.SelectedValue.ToString();
                    if (dataTableQry.Rows.Count > 0)
                    {
                        if (obj.ExecuteScalarQry("select fy from [dbo].[ACTS_FinancialYear] where StartDate<='" + dtpTransDt.GetDateValue().ToString("dd MMM yyyy") + "' and enddate>='" + dtpTransDt.GetDateValue().ToString("dd MMM yyyy") + "'").ToString().Trim() != obj.ExecuteScalarQry("select fy from [dbo].[ACTS_FinancialYear] where StartDate<='" + DateTime.Now.ToString("dd MMM yyyy") + "' and enddate>='" + DateTime.Now.ToString("dd MMM yyyy") + "'").ToString().Trim())
                        {
                            lblMsg.Text = "Receive date is not within the current Financial Year";
                            lblMsg.ForeColor = Color.Red;
                        }
                        else if (dtpTransDt.GetDateValue().Date.CompareTo(now.Date) > 0)
                        {
                            ScriptManager.RegisterStartupScript(Page, GetType(), "showmess", "alert('Year is not initialized');window.open ('../FinancialYearChild.aspx?idate=" + dtpTransDt.GetDateValue().ToString("MM/dd/yyyy") + "','mywindow','menubar=1,resizable=1,width=600,height=600,scrollbars=1');", true);
                        }
                        else
                        {
                            obj = new clsDAL();
                            DataTable dataTable = new DataTable();
                            Hashtable hashtable = new Hashtable();
                            hashtable.Add("@TransDate", dtpTransDt.GetDateValue().ToString("dd MMM yyyy"));
                            if (rBtnParty.SelectedValue == "p")
                            {
                                if (drpParty.SelectedIndex > 0)
                                {
                                    hashtable.Add("@AccountHeadCr", int.Parse(drpParty.SelectedValue.ToString()));
                                    hashtable.Add("@RcvdFrom", drpParty.SelectedItem.Text);
                                    hashtable.Add("@PartyAcHead", int.Parse(drpParty.SelectedValue.ToString()));
                                }
                                else
                                {
                                    lblMsg.Text = "Please Select Party";
                                    lblMsg.ForeColor = Color.Red;
                                    return;
                                }
                            }
                            else if (drpAcHead.SelectedIndex > 0)
                            {
                                if (txtGiver.Text.ToString().Trim() != "")
                                {
                                    hashtable.Add("@AccountHeadCr", int.Parse(drpAcHead.SelectedValue.ToString()));
                                    hashtable.Add("@RcvdFrom", txtGiver.Text.ToString().Trim());
                                }
                                else
                                {
                                    lblMsg.Text = "Please Enter Received From";
                                    lblMsg.ForeColor = Color.Red;
                                    return;
                                }
                            }
                            else
                            {
                                lblMsg.Text = "Please Select Income Head";
                                lblMsg.ForeColor = Color.Red;
                                return;
                            }
                            if (rbtnMode.SelectedValue == "C")
                                hashtable.Add("@AccountHeadDr", 3);
                            else if (drpBankAc.SelectedIndex > 0)
                            {
                                hashtable.Add("@AccountHeadDr", int.Parse(drpBankAc.SelectedValue.ToString()));
                            }
                            else
                            {
                                lblMsg.Text = "Please Bank Name!!";
                                lblMsg.ForeColor = Color.Red;
                                return;
                            }
                            hashtable.Add("@TransAmt", txtAmount.Text.ToString().Trim());
                            hashtable.Add("@Description", txtDesc.Text.Trim());
                            hashtable.Add("@DrawanOnBank", txtDrawnBank.Text.Trim());
                            if (txtInstrumentNo.Text.Trim() != "")
                                hashtable.Add("@InstrumentNo", txtInstrumentNo.Text.ToString().Trim());
                            if (txtInstrumentDt.Text.Trim() != "")
                                hashtable.Add("@InstrumentDt", dtpInstDt.GetDateValue().ToString("MM/dd/yyyy"));
                            hashtable.Add("@LinkForm", "rv");
                            hashtable.Add("@UserId", Session["User_Id"]);
                            hashtable.Add("@ShopId", Session["SchoolId"]);
                            string str2 = obj.ExecuteScalar("ACTS_InsertReceiptVoucher", hashtable);
                            if (str2.Trim().ToUpper() != "DUP" && str2.Trim().ToUpper() != "ERROR")
                            {
                                ViewState["MId"] = str2.Trim();
                                if (Request.QueryString["party"] == null)
                                {
                                    lblMsg.Text = "Data saved successfully !";
                                    lblMsg.ForeColor = Color.Green;
                                    btnPrint.Enabled = true;
                                }
                                else
                                    ScriptManager.RegisterClientScriptBlock((Control)btnSubmit, btnSubmit.GetType(), "Message", "alert('Data saved successfully !');window.location='../Sales/ItemSale.aspx';", true);
                                ClearPage();
                            }
                            else if (str2.Trim().ToUpper() == "DUP")
                            {
                                lblMsg.Text = "Receipt No Already Exists !";
                                lblMsg.ForeColor = Color.Green;
                            }
                            else
                            {
                                lblMsg.Text = "Transaction Failed! Please Try Again !";
                                lblMsg.ForeColor = Color.Green;
                            }
                        }
                    }
                    else
                    {
                        lblMsg.Text = "No running Financial available For Given Trans Date !!";
                        lblMsg.ForeColor = Color.Red;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message.ToString();
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["MId"].ToString().Trim();
            string str = "rptActReceiptVoucherPrint.aspx?PrId=" + ViewState["MId"].ToString().Trim();
            btnPrint.Enabled = false;
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('" + str + "');", true);
        }
        catch
        {
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearPage();
    }

    public void ClearPage()
    {
        txtDrawnBank.Text = string.Empty;
        txtDesc.Text = string.Empty;
        txtGiver.Text = string.Empty;
        txtAmount.Text = string.Empty;
        rbtnMode.SelectedValue = "B";
        txtTransactionDate.Text = string.Empty;
        txtInstrumentDt.Text = string.Empty;
        txtInstrumentNo.Text = string.Empty;
        FillCreditHeads();
        txtTransactionDate.Focus();
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReceiptVoucherList.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Accounts/Welcome.aspx");
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

    protected void rBtnParty_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rBtnParty.SelectedValue == "p")
        {
            drpParty.Visible = true;
            txtGiver.Visible = false;
            drpAcHead.Enabled = false;
            drpParty.Focus();
        }
        else
        {
            drpParty.Visible = false;
            txtGiver.Visible = true;
            drpAcHead.Enabled = true;
            txtGiver.Focus();
        }
    }

    protected void dtpTransDt_SelectionChanged(object sender, EventArgs e)
    {
        string str = new clsDAL().ExecuteScalarQry("select max(TransDt) from dbo.Acts_PaymentReceiptVoucher where upper(Payment_Receipt)='R'");
        if (str.Trim() != "" && Convert.ToDateTime(dtpTransDt.GetDateValue().ToString("dd-MMM-yyyy")) < Convert.ToDateTime(str.Trim()))
        {
            lblMsg.Text = "Warning : You are going to make a back date transaction. The <b>DCR will be modified</b> for this date! So verify the DCR accordingly !";
            lblMsg.ForeColor = Color.Red;
        }
        else
            lblMsg.Text = "";
    }
}