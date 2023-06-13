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

public partial class Accounts_AcDayOpenClose : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL(); 
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            try
            {
                if (Request.QueryString["msg"] != null)
                {
                    lblMsg.Text = Request.QueryString["msg"].ToString();
                    lblMsg.ForeColor = Color.Red;
                }
                InitFY();
            }
            catch (Exception ex)
            {
            }
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void InitFY()
    {
        dt = obj.GetDataTableQry("Select * from dbo.ACTS_FinancialYear where IsFinalized=0 or IsFinalized is null");
        if (dt.Rows.Count > 0)
        {
            ViewState["FY"] = dt.Rows[0]["FY"].ToString();
            CheckStatus();
        }
        else
        {
            lblMsg.Text = "Open Financial Year ";
            lblMsg.ForeColor = Color.Red;
            btnOpen.Enabled = false;
            btnClose.Enabled = false;
        }
    }

    private void CheckStatus()
    {
        ht.Clear();
        ht.Add("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
        dt = obj.GetDataTable("ACTS_CheckStatus", ht);
        if (dt.Rows.Count > 0)
        {
             
            if (dt.Rows[0]["ActsClosed"].ToString() == "True")
            {
                txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy");
                PopCalendar1.From.Date = Convert.ToDateTime(dt.Rows[0]["TransDate"].ToString()).AddDays(1.0);
                PopCalendar1.To.Date = DateTime.Now;
                if (DateTime.Now.ToString("dd-MM-yyyy") == Convert.ToDateTime(dt.Rows[0]["TransDate"].ToString()).ToString("dd-MM-yyyy"))
                {
                    btnClose.Enabled = false;
                    btnOpen.Enabled = false;
                    PopCalendar1.Enabled = false;
                    txtdate.Enabled = false;
                }
                else
                {
                    btnClose.Enabled = false;
                    btnOpen.Enabled = true;
                    PopCalendar1.Enabled = false;
                    txtdate.Enabled = true;
                }
            }
            else
            {
                txtdate.Text = Convert.ToDateTime(dt.Rows[0]["TransDate"].ToString()).ToString("dd-MM-yyyy");
                btnClose.Enabled = true;
                btnOpen.Enabled = false;
                PopCalendar1.Enabled = false;
                txtdate.Enabled = false;
                ViewState["MasterControlId"] = dt.Rows[0]["MasterControlId"].ToString();
                pnlSummary.Visible = false;
            }
            Session["TransDt"] = dt.Rows[0]["TransDate"].ToString();
            Session["ActsClosedStatus"] = dt.Rows[0]["ActsClosed"].ToString();
            ht.Clear();
            ht.Add("@transDt", Convert.ToDateTime(dt.Rows[0]["TransDate"].ToString()));
            ht.Add("@schoolId", int.Parse(Session["SchoolId"].ToString()));
            dt = obj.GetDataTable("ACTS_GetAcctsHeadSummary", ht);
            int num = 0;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                {
                    if (row["AccountHead"].ToString() == "1")
                        lblIncome.Text = row["Amount"].ToString();
                    if (row["AccountHead"].ToString() == "2")
                        lblExpense.Text = row["Amount"].ToString();
                    if (row["AccountHead"].ToString() == "3")
                    {
                        if (num == 0)
                        {
                            lblCashInHand.Text = row["Amount"].ToString();
                            num = 1;
                        }
                        else
                            lblCashInHand.Text = (double.Parse(row["Amount"].ToString()) - double.Parse(lblCashInHand.Text.ToString())).ToString();
                    }
                    if (row["AccountHead"].ToString() == "4")
                        lblPurchase.Text = row["Amount"].ToString();
                    if (row["AccountHead"].ToString() == "5")
                        lblSale.Text = row["Amount"].ToString();
                }
                pnlSummary.Visible = true;
            }
            dt = obj.GetDataTable("ACTS_GetAcctsHeadSummaryDtwise", ht);
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                {
                    if (row["AccountHead"].ToString() == "1")
                        lblIncomeOnDt.Text = row["Amount"].ToString();
                    if (row["AccountHead"].ToString() == "2")
                        lblExpenseOnDt.Text = row["Amount"].ToString();
                    if (row["AccountHead"].ToString() == "4")
                        lblPurchaseOnDt.Text = row["Amount"].ToString();
                    if (row["AccountHead"].ToString() == "5")
                        lblSaleOnDt.Text = row["Amount"].ToString();
                }
            }
            else
                lblIncomeOnDt.Text = lblExpenseOnDt.Text = lblPurchaseOnDt.Text = lblSaleOnDt.Text = "0.00";
        }
        else
        {
            txtdate.Text = DateTime.Now.ToString("dd-MM-yyyy");
            PopCalendar1.To.Date = DateTime.Now;
            btnClose.Enabled = false;
            Session["TransDt"] = null;
        }
    }

    protected void btnOpen_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            if (obj.ExecuteScalarQry("select dbo.fuGetSessionYr ('" + PopCalendar1.GetDateValue() + "')") == ViewState["FY"].ToString())
            {
                ht.Clear();
                ht.Add("@transDt", PopCalendar1.GetDateValue());
                ht.Add("@userid", int.Parse(Session["User_Id"].ToString()));
                ht.Add("@schoolid", int.Parse(Session["SchoolId"].ToString()));
                obj.ExcuteProcInsUpdt("ACTS_InsUpdtMasterControl", ht);
                CheckStatus();
            }
            else
            {
                lblMsg.Text = "Transaction Date Should lie in Financial Yr " + ViewState["FY"].ToString();
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in Transaction Open";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnClose_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        try
        {
            if (ViewState["MasterControlId"] == null)
                return;
            int num = int.Parse(ViewState["MasterControlId"].ToString());
            ht.Clear();
            ht.Add("@masterControlId", num);
            ht.Add("@userid", int.Parse(Session["User_Id"].ToString()));
            obj.ExcuteProcInsUpdt("ACTS_InsUpdtMasterControl", ht);
            CheckStatus();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error in Transaction Close";
            lblMsg.ForeColor = Color.Red;
        }
    }
}