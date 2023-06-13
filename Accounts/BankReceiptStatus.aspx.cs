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

public partial class Accounts_BankReceiptStatus : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SchoolId"] == null)
            Response.Redirect("../Login.aspx");
        lblMsg.Text = string.Empty;
        lblRecord.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        FillGrid();
        FillDepositedBank();
        ViewState["PRId"] = null;
    }

    private void FillDepositedBank()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        drpBank.DataSource = clsDal.GetDataTableQry("SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads a inner join Acts_AccountGroups ag on ag.AG_Code=a.AG_Code WHERE a.AG_Code=7 or ag.AG_Parent=7 ORDER BY AcctsHead");
        drpBank.DataTextField = "AcctsHead";
        drpBank.DataValueField = "AcctsHeadId";
        drpBank.DataBind();
        drpBank.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    protected void btnStatus_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        if (drpStatus.SelectedValue.ToString().Trim() != "b")
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@PR_Id", ViewState["PRId"].ToString());
            hashtable.Add("@BankAcHeadId", drpBank.SelectedValue.Trim());
            hashtable.Add("@InstrumentStatus", drpStatus.SelectedValue.Trim());
            hashtable.Add("@StatusDate", dtpStatusDt.SelectedDate.ToString("dd MMM yyyy"));
            string s = new clsDAL().ExecuteScalarQry("select top 1 isnull(ClosingBal,0) from dbo.ACTS_BankTransactions where BankAcHeadId=" + drpBank.SelectedValue.Trim() + " order by TransDate desc,TransId desc");
            if (s.Trim() == "")
                s = "0";
            DataTable dataTable = ViewState["StatusUpdtTbl"] as DataTable;
            float num;
            if (dataTable.Rows[0]["Payment_Receipt"].ToString().Trim().ToLower() == "p")
            {
                hashtable.Add("@WithdrawlAmt", dataTable.Rows[0]["Amount"]);
                hashtable.Add("@DipositAmt", 0);
                num = float.Parse(s) - float.Parse(dataTable.Rows[0]["Amount"].ToString());
            }
            else
            {
                hashtable.Add("@DipositAmt", dataTable.Rows[0]["Amount"]);
                hashtable.Add("@WithdrawlAmt", 0);
                num = float.Parse(s) + float.Parse(dataTable.Rows[0]["Amount"].ToString());
            }
            hashtable.Add("@ClosingBal", num);
            hashtable.Add("@Remarks", "");
            hashtable.Add("@UserId", Session["User_Id"]);
            new clsDAL().ExcuteProcInsUpdt("ACTS_UptdBankStatus", hashtable);
            ViewState["PRId"] = null;
            ViewState["StatusUpdtTbl"] = null;
            lblMsg.Text = "Record Updated Successfully";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            DataTable dataTable = new DataTable();
            DataTable dataTableQry = new clsDAL().GetDataTableQry("Select LinkForm,InvoiceReceiptNo,PR_Id from dbo.Acts_PaymentReceiptVoucher where PR_Id=" + ViewState["PRId"].ToString().Trim());
            if (dataTableQry.Rows.Count > 0)
            {
                if (dataTableQry.Rows[0]["LinkForm"].ToString().Trim() == "fr")
                {
                    DeleteTrans(dataTableQry.Rows[0]["InvoiceReceiptNo"].ToString().Trim());
                    lblMsg.Text = "Status Updated Successfully";
                    lblMsg.ForeColor = Color.Green;
                }
                else if (dataTableQry.Rows[0]["LinkForm"].ToString().Trim() == "rv")
                    DeleteReceiptVr(dataTableQry.Rows[0]["PR_Id"].ToString().Trim());
                else if (dataTableQry.Rows[0]["LinkForm"].ToString().Trim() == "pv")
                    DeletePaymentVr(dataTableQry.Rows[0]["PR_Id"].ToString().Trim());
                else if (dataTableQry.Rows[0]["LinkForm"].ToString().Trim() == "mv")
                    DeleteExpVr(dataTableQry.Rows[0]["PR_Id"].ToString().Trim());
                else if (dataTableQry.Rows[0]["LinkForm"].ToString().Trim() == "bs")
                    DeleteBookSale(dataTableQry.Rows[0]["InvoiceReceiptNo"].ToString().Trim());
                else if (dataTableQry.Rows[0]["LinkForm"].ToString().Trim() == "pr")
                    DeletePurchase(dataTableQry.Rows[0]["PR_Id"].ToString().Trim());
                else if (dataTableQry.Rows[0]["LinkForm"].ToString().Trim() == "LR")
                    DeleteLoanRecovery(dataTableQry.Rows[0]["PR_Id"].ToString().Trim());
            }
        }
        ViewState["PRId"] = null;
        ViewState["StatusUpdtTbl"] = null;
        drpBank.SelectedIndex = -1;
        drpStatus.SelectedIndex = 0;
        txtStatusDt.Text = string.Empty;
        FillGrid();
        pnlEntry.Visible = false;
        pnlList.Visible = true;
    }

    private void DeleteTrans(string RecptNo)
    {
        Decimal CrAmount = new Decimal(0);
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal1 = new clsDAL();
        hashtable.Add("receiptno", RecptNo);
        hashtable.Add("@CancelledReason", "Check Bounced");
        DataSet dataSet1 = new DataSet();
        DataSet dataSet2 = new clsDAL().GetDataSet("ps_sp_delete_feecashNew", hashtable);
        clsDAL clsDal2 = new clsDAL();
        if (dataSet2.Tables[0].Rows.Count > 0)
        {
            new clsDAL().ExecuteScalar("ps_sp_insert_FeeLedger_h", new Hashtable()
      {
        {
           "AdmnNo",
          dataSet2.Tables[0].Rows[0]["AdmnNo"]
        },
        {
           "VrNo",
          dataSet2.Tables[0].Rows[0]["receiptno"]
        }
      });
            updatefeeledger(dataSet2.Tables[0], CrAmount);
        }
        UpdateGenLedger(dataSet2.Tables[1]);
        UpdateBankStatus("", RecptNo);
    }

    private void UpdateGenLedger(DataTable dtGenLedger)
    {
        clsDAL clsDal = new clsDAL();
        foreach (DataRow row in (InternalDataCollectionBase)dtGenLedger.Rows)
            clsDal.GetDataTable("PS_SP_CancelGenLedgerFR", new Hashtable()
      {
        {
           "GenLedgerId",
          row["GenLedgerId"]
        }
      });
    }

    private void updatefeeledger(DataTable tb, Decimal CrAmount)
    {
        Convert.ToDecimal(tb.Rows[0]["oldamount"].ToString());
        Decimal num1 = new Decimal(0);
        clsDAL clsDal = new clsDAL();
        foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
        {
            Decimal result = new Decimal(0);
            Decimal num2 = new Decimal(0);
            Decimal.TryParse(Convert.ToString(row["Credit"]), out result);
            row["Credit"] = 0;
            Decimal bal = result;
            tb.AcceptChanges();
            UpdateBalAmtInLedger(row, bal);
            num1 += bal;
        }
        updatefeeledger(tb, 0);
    }

    private void UpdateBalAmtInLedger(DataRow dr, Decimal bal)
    {
        new clsDAL().GetDataTable("ps_sp_update_FeeLedgerBank", new Hashtable()
    {
      {
         "transno",
         Convert.ToInt64(dr["TransNo"])
      },
      {
         "AdmnNo",
         Convert.ToInt64(dr["AdmnNo"])
      },
      {
         "VrNo",
         dr["receiptno"].ToString()
      },
      {
         "FeeId",
         Convert.ToInt64(dr["FeeId"])
      },
      {
         "Balance",
         bal
      },
      {
         "userid",
         Convert.ToInt32(Session["User_Id"].ToString())
      }
    });
    }

    private void updatefeeledger(DataTable tbcredit, int mode)
    {
        clsDAL clsDal = new clsDAL();
        foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
            new clsDAL().GetDataTable("ps_sp_update_feeledger", new Hashtable()
      {
        {
           "TransNo",
          row["TransNo"]
        },
        {
           "AdmnNo",
          row["AdmnNo"]
        },
        {
           "debit",
          row["debit"]
        },
        {
           "credit",
          row["credit"]
        },
        {
           "balance",
          row["balance"]
        },
        {
           "Receipt_VrNo",
          row["ReceiptNo"]
        },
        {
           "UserID",
          Session["User_Id"]
        },
        {
           "UserDate",
           DateTime.Now.ToString("MM/dd/yyyy")
        },
        {
           "SchoolID",
           Session["SchoolId"].ToString()
        },
        {
           "mode",
           "Cash"
        }
      });
    }

    private void DeleteReceiptVr(string PRId)
    {
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal = new clsDAL();
        hashtable.Add("@PR_Id", Convert.ToInt32(PRId));
        hashtable.Add("@CancelledReason", "Check Bounced");
        string str = clsDal.ExecuteScalar("ACTS_DeleteRcptVoucher", hashtable);
        if (str.Trim() == "")
        {
            if (!(UpdateBankStatus(PRId, "").Trim() == ""))
                return;
            lblMsg.Text = "Status Updated Successfully";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = str.Trim();
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void DeletePaymentVr(string PRId)
    {
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal = new clsDAL();
        hashtable.Add("@PR_Id", Convert.ToInt32(PRId));
        string str = clsDal.ExecuteScalar("ACTS_DeletePmntVoucher", hashtable);
        if (str.Trim() == "")
        {
            if (!(UpdateBankStatus(PRId, "").Trim() == ""))
                return;
            lblMsg.Text = "Status Updated Successfully";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = str.Trim();
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void DeleteExpVr(string PRId)
    {
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal = new clsDAL();
        hashtable.Add("@ExpId", Convert.ToInt32(PRId));
        if (clsDal.ExecuteScalar("ACTS_DelMiscExp", hashtable).Trim() == "")
        {
            if (!(UpdateBankStatus(PRId, "").Trim() == ""))
                return;
            lblMsg.Text = "Status Updated Successfully";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = "Transaction Failed . Please try again!!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void DeleteBookSale(string ReceiptNo)
    {
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal = new clsDAL();
        string str = clsDal.ExecuteScalarQry("select InvNo from dbo.ACTS_SaleDoc where PhysicalBillNo=" + ReceiptNo);
        hashtable.Add("@InvNo", Convert.ToInt32(str));
        if (clsDal.ExecuteScalar("ACTS_RevertSale", hashtable).Trim() == "")
        {
            if (!(UpdateBankStatus("", ReceiptNo).Trim() == ""))
                return;
            lblMsg.Text = "Status Updated Successfully";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = "Transaction Failed . Please try again!!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void DeletePurchase(string PR_Id)
    {
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal = new clsDAL();
        string str = clsDal.ExecuteScalarQry("select distinct PurchaseId from dbo.Acts_GenLedger where PR_Id=" + PR_Id);
        hashtable.Add("@PurchaseId", Convert.ToInt32(str));
        if (clsDal.ExecuteScalar("ACTS_DelPurchaseStock", hashtable).Trim() == "")
        {
            if (!(UpdateBankStatus(PR_Id, "").Trim() == ""))
                return;
            lblMsg.Text = "Status Updated Successfully";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = "Transaction Failed . Please try again!!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void DeleteLoanRecovery(string PR_Id)
    {
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal = new clsDAL();
        string str1 = clsDal.ExecuteScalarQry("select GenledgerId from dbo.HR_EmpLoanRecovery where PR_Id=" + PR_Id.Trim());
        string str2 = clsDal.ExecuteScalarQry("select EmpId from dbo.HR_EmpLoanRecovery where PR_Id=" + PR_Id.Trim());
        hashtable.Add("@PR_Id", PR_Id.Trim());
        hashtable.Add("@EmpId", str2.Trim());
        hashtable.Add("@GenledgerId", str1.Trim());
        string str3 = clsDal.ExecuteScalar("HR_DelLoanRec", hashtable);
        if (str3.Trim() == "")
        {
            if (!(UpdateBankStatus(PR_Id, "").Trim() == ""))
                return;
            lblMsg.Text = "Status Updated Successfully";
            lblMsg.ForeColor = Color.Green;
        }
        else if (str3.Trim().ToUpper() == "GENERATED")
        {
            lblMsg.Text = "Unable to revert loan recovery! Salary already generated for the recovered month!!";
            lblMsg.ForeColor = Color.Red;
        }
        else if (str3.Trim() == "No")
        {
            lblMsg.Text = "Unable to revert loan recovery! New Loan has already been Applied!!!!";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Transaction Failed . Please try again!!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private string UpdateBankStatus(string PR_Id, string ReceiptNo)
    {
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal = new clsDAL();
        if (PR_Id != "")
            hashtable.Add("@PR_Id", Convert.ToInt32(PR_Id));
        else
            hashtable.Add("@RcptNo", ReceiptNo);
        hashtable.Add("@InstrumentStatus", drpStatus.SelectedValue.Trim());
        hashtable.Add("@StatusDate", dtpStatusDt.SelectedDate.ToString("dd MMM yyyy"));
        return clsDal.ExecuteScalar("ACTS_UptdBouncedStatus", hashtable);
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        pnlEntry.Visible = true;
        pnlList.Visible = false;
        double int64 = (double)Convert.ToInt64(grdReceiptStatus.DataKeys[((GridViewRow)((Control)sender).Parent.Parent).DataItemIndex].Value);
        ViewState["PRId"] = int64;
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = clsDal.GetDataTable("Acts_GetReceiptList", new Hashtable()
    {
      {
         "@PR_Id",
         int64
      },
      {
         "@SchoolId",
        Session["SchoolId"]
      }
    });
        ViewState["StatusUpdtTbl"] = dataTable2;
        if (dataTable2.Rows.Count > 0)
        {
            if (dataTable2.Rows[0]["InstrumentStatus"].ToString() != "" && dataTable2.Rows[0]["InstrumentStatus"].ToString() != "P")
                drpStatus.SelectedValue = dataTable2.Rows[0]["InstrumentStatus"].ToString().Trim();
            if (dataTable2.Rows[0]["BankAcHeadId"].ToString() != "")
            {
                drpBank.SelectedValue = dataTable2.Rows[0]["BankAcHeadId"].ToString().Trim();
                drpBank.Enabled = false;
            }
            if (dataTable2.Rows[0]["StatusDate"].ToString() != "")
                dtpStatusDt.SelectedDate = Convert.ToDateTime(dataTable2.Rows[0]["StatusDate"].ToString());
        }
        pnlEntry.Visible = true;
    }

    protected void btnSelectPaid_Click(object sender, EventArgs e)
    {
        pnlEntry.Visible = true;
        pnlList.Visible = false;
        double int64 = (double)Convert.ToInt64(grdPayment.DataKeys[((GridViewRow)((Control)sender).Parent.Parent).DataItemIndex].Value);
        ViewState["PRId"] = int64;
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = clsDal.GetDataTable("Acts_GetPaymentListForStatus", new Hashtable()
    {
      {
         "@PR_Id",
         int64
      },
      {
         "@SchoolId",
        Session["SchoolId"]
      }
    });
        ViewState["StatusUpdtTbl"] = dataTable2;
        if (dataTable2.Rows.Count > 0)
        {
            if (dataTable2.Rows[0]["InstrumentStatus"].ToString() != "" && dataTable2.Rows[0]["InstrumentStatus"].ToString() != "P")
                drpStatus.SelectedValue = dataTable2.Rows[0]["InstrumentStatus"].ToString().Trim();
            if (dataTable2.Rows[0]["BankAcHeadId"].ToString() != "")
            {
                drpBank.SelectedValue = dataTable2.Rows[0]["BankAcHeadId"].ToString().Trim();
                drpBank.Enabled = false;
            }
            if (dataTable2.Rows[0]["StatusDate"].ToString() != "")
                dtpStatusDt.SelectedDate = Convert.ToDateTime(dataTable2.Rows[0]["StatusDate"].ToString());
        }
        dtpStatusDt.SelectedDate = DateTime.Today;
        pnlEntry.Visible = true;
        UpdatePanel1.Update();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        pnlEntry.Visible = false;
        pnlList.Visible = true;
        ViewState["PRId"] = null;
        drpBank.SelectedIndex = -1;
        drpStatus.SelectedIndex = 0;
        txtStatusDt.Text = string.Empty;
    }

    protected void FillGrid()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (!txtFromDt.Text.Trim().Equals(string.Empty))
            hashtable.Add("@FrmDt", dtpfromdt.GetDateValue().ToString("MM/dd/yyyy"));
        if (!txtToDt.Text.Trim().Equals(string.Empty))
            hashtable.Add("@ToDt", dtptodt.GetDateValue().ToString("MM/dd/yyyy"));
        hashtable.Add("@Status", drpStatusFilter.SelectedValue.Trim());
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        if (rBtnPayment.Checked)
        {
            DataTable dataTable = clsDal.GetDataTable("Acts_GetPaymentListForStatus", hashtable);
            grdPayment.DataSource = dataTable.DefaultView;
            grdPayment.DataBind();
            grdPayment.Visible = true;
            lblRecord.Text = "Total Record : " + dataTable.Rows.Count.ToString();
        }
        else
        {
            DataTable dataTable = clsDal.GetDataTable("Acts_GetReceiptList", hashtable);
            grdReceiptStatus.DataSource = dataTable.DefaultView;
            grdReceiptStatus.DataBind();
            grdReceiptStatus.Visible = true;
            lblRecord.Text = "Total Record : " + dataTable.Rows.Count.ToString();
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void grdReceiptStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        if (((Label)e.Row.FindControl("lblStatus")).Text.Split('(')[0].ToString().Trim() == "Encashed")
            ((System.Web.UI.WebControls.WebControl)e.Row.Cells[0].FindControl("btnSelect")).Enabled = false;
        else
            ((System.Web.UI.WebControls.WebControl)e.Row.Cells[0].FindControl("btnSelect")).Enabled = true;
    }

    protected void grdPayment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        if (((Label)e.Row.FindControl("lblStatusPaid")).Text.Split('(')[0].ToString().Trim() == "Encashed")
            ((System.Web.UI.WebControls.WebControl)e.Row.Cells[0].FindControl("btnSelectPaid")).Enabled = false;
        else
            ((System.Web.UI.WebControls.WebControl)e.Row.Cells[0].FindControl("btnSelectPaid")).Enabled = true;
    }

    protected void btnTrans_Click(object sender, EventArgs e)
    {
        Response.Redirect("BankTransactions.aspx");
    }

    protected void rBtnReceipt_CheckedChanged(object sender, EventArgs e)
    {
        grdPayment.Visible = false;
        grdReceiptStatus.Visible = true;
        ViewState["PRId"] = null;
    }

    protected void rBtnPayment_CheckedChanged(object sender, EventArgs e)
    {
        grdPayment.Visible = true;
        grdReceiptStatus.Visible = false;
        ViewState["PRId"] = null;
    }

    protected void btnStatusDt_Click(object sender, ImageClickEventArgs e)
    {
        dtpStatusDt.Visible = true;
    }

    protected void cdrCalendar_SelectionChanged(object sender, EventArgs e)
    {
        txtStatusDt.Text = dtpStatusDt.SelectedDate.ToString("dd/MM/yyyy");
        dtpStatusDt.Visible = false;
    }

    protected void dtpStatusDt_DayRender(object sender, DayRenderEventArgs e)
    {
        DateTime today = DateTime.Today;
        e.Day.IsSelectable = e.Day.Date <= today;
    }

 

}