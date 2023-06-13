using AjaxControlToolkit;
using ASP;
using Classes.DA;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_SupplierInvoice : System.Web.UI.Page
{
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CollegeId"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        bindDropDown(drpSupplier, "select SupId,SupName from PS_Supplier where SupId<>1 order by SupName", "SupName", "SupId");
        drpSupplier.Items.Insert(1, new ListItem("Others", "1"));
        bindDropDown(drpAcHead, "SELECT AccountHeadId,AccountHeadDetail FROM dbo.PS_AccountHeadMaster WHERE AccountTypeCrDr='Dr' ORDER BY AccountHeadDetail", "AccountHeadDetail", "AccountHeadId");
        if (Request.QueryString["inv"] == null)
            return;
        FillData();
    }

    private void FillData()
    {
        DataSet dataSet = new DataSet();
        new Hashtable()
    {
      {
         "invid",
         Request.QueryString["inv"]
      }
    };
        DataTable dataTableQry = new clsDAL().GetDataTableQry("select SupId, MRNo, MRDate, PayOrderNo, PaySlipNo, PmtDetails, ReceivedBy, CAST(Amount AS DECIMAL(10,2)) AS Amount, PaymentMode, ChequeNo, ChequeDate, DrawnOnBank from dbo.PS_SupplierInvoice where invoiceid=" + Request.QueryString["inv"] + " and CollegeId=" + Session["CollegeId"].ToString());
        if (dataTableQry.Rows.Count <= 0)
            return;
        drpSupplier.SelectedValue = dataTableQry.Rows[0]["SupId"].ToString();
        txtBillNo.Text = dataTableQry.Rows[0]["MRNo"].ToString();
        txtBillDate.Text = Convert.ToDateTime(dataTableQry.Rows[0]["MRDate"].ToString()).ToString("dd MMM yyyy");
        lblBal.Text = "";
        txtPayOrderNo.Text = dataTableQry.Rows[0]["PayOrderNo"].ToString();
        txtPaySlipNo.Text = dataTableQry.Rows[0]["PaySlipNo"].ToString();
        txtPmtDetails.Text = dataTableQry.Rows[0]["PmtDetails"].ToString();
        txtRcvdBy.Text = dataTableQry.Rows[0]["ReceivedBy"].ToString();
        txtPaidAmt.Text = dataTableQry.Rows[0]["Amount"].ToString();
        drpPmtMode.SelectedValue = dataTableQry.Rows[0]["PaymentMode"].ToString();
        txtChequeNo.Text = dataTableQry.Rows[0]["ChequeNo"].ToString();
        txtBank.Text = dataTableQry.Rows[0]["DrawnOnBank"].ToString();
        if (dataTableQry.Rows[0]["PaymentMode"].ToString().Trim() == "Cash")
        {
            txtBank.Enabled = false;
            txtChequeNo.Enabled = false;
            rfvBank.Enabled = false;
            rfvDDNo.Enabled = false;
        }
        else
        {
            txtBank.Enabled = true;
            txtChequeNo.Enabled = true;
        }
        if (!drpSupplier.SelectedValue.ToString().Trim().Equals("1"))
            return;
        drpSupplier.Enabled = false;
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new Common();
        dt = new DataTable();
        dt = obj.ExecuteSql(query);
        drp.DataSource = dt;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    protected void drpPmtMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!drpPmtMode.SelectedIndex.Equals(1))
        {
            txtBank.Enabled = true;
            txtChequeNo.Enabled = true;
            rfvBank.Enabled = rfvDDNo.Enabled = true;
        }
        else
        {
            txtBank.Text = string.Empty;
            txtBank.Enabled = false;
            txtChequeNo.Text = string.Empty;
            txtChequeNo.Enabled = false;
            rfvBank.Enabled = rfvDDNo.Enabled = false;
        }
    }

    private void ClearFields()
    {
        drpSupplier.SelectedIndex = -1;
        drpAcHead.SelectedIndex = -1;
        AcHead.Visible = false;
        txtBillNo.Text = string.Empty;
        txtBillDate.Text = string.Empty;
        lblBal.Text = "";
        txtBank.Text = string.Empty;
        txtChequeNo.Text = string.Empty;
        drpPmtMode.SelectedIndex = -1;
        txtRcvdBy.Text = string.Empty;
        txtPaidAmt.Text = string.Empty;
        txtBank.Enabled = true;
        txtChequeNo.Enabled = true;
        txtPmtDetails.Text = string.Empty;
        txtPayOrderNo.Text = string.Empty;
        txtPaySlipNo.Text = string.Empty;
    }

    private long InsertSupplierInvoice()
    {
        long result = 0;
        try
        {
            obj = new Common();
            ht = new Hashtable();
            dt = new DataTable();
            if (drpSupplier.SelectedValue.ToString().Equals("1"))
                ht.Add("TransType", "Cash");
            else
                ht.Add("TransType", "Credit");
            ht.Add("SupId", drpSupplier.SelectedValue);
            ht.Add("CollegeId", Session["CollegeId"]);
            ht.Add("MRNo", txtBillNo.Text.Trim());
            if (txtBillDate.Text.Trim() != "")
            {
                try
                {
                    ht.Add("MRDate", Convert.ToDateTime(txtBillDate.Text.Trim()));
                }
                catch (Exception ex)
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid Date');", true);
                    return 0;
                }
            }
            else
                ht.Add("MRDate", "01/01/1900");
            ht.Add("PayOrderNo", txtPayOrderNo.Text.Trim());
            ht.Add("PaySlipNo", txtPaySlipNo.Text.Trim());
            ht.Add("PmtDetails", txtPmtDetails.Text.Trim());
            ht.Add("ReceivedBy", txtRcvdBy.Text.Trim());
            ht.Add("Amount", Convert.ToDouble(txtPaidAmt.Text.Trim()));
            ht.Add("PaymentMode", drpPmtMode.SelectedValue.ToString());
            ht.Add("ChequeNo", txtChequeNo.Text.Trim());
            ht.Add("ChequeDate", Convert.ToDateTime("01 Jan 1900"));
            ht.Add("DrawnOnBank", txtBank.Text.Trim());
            ht.Add("UserID", Session["User_Id"]);
            ht.Add("AcHead", drpAcHead.SelectedValue);
            if (Request.QueryString["inv"] != null)
            {
                ht.Add("InvId", Request.QueryString["inv"].ToString());
                ht.Remove("AcHead");
                obj.ExcuteProcInsUpdt("ps_sp_Updt_SupplierInvoice", ht);
                result = (long)int.Parse(Request.QueryString["inv"].ToString());
            }
            else
            {
                dt = obj.GetDataTable("ps_sp_insert_SupplierInvoice", ht);
                long.TryParse(dt.Rows[0][0].ToString().Trim(), out result);
            }
            return result;
        }
        catch (Exception ex)
        {
        }
        return result;
    }

    protected void drpSupplier_SelectedIndexChanged(object sender, EventArgs e)
    {
        obj = new Common();
        if (drpSupplier.SelectedIndex > 0)
        {
            if (drpSupplier.SelectedValue.ToString().Trim().Equals("1"))
            {
                AcHead.Visible = true;
                lblBal.Text = string.Empty;
            }
            else
            {
                AcHead.Visible = false;
                string str = obj.ExecuteScalarQry("select isnull(cast(sum(AmountCr)-sum(AmountDr) as decimal(10,2)),'0.00') as TobePaid from dbo.PS_SupplierLedger where SupId=" + drpSupplier.SelectedValue);
                if (Convert.ToDouble(str) != 0.0)
                    lblBal.Text = "Credit Amount: " + str;
                else
                    lblBal.Text = "No Credit Balance ";
            }
        }
        else
        {
            AcHead.Visible = false;
            lblBal.Text = string.Empty;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    protected void btnPay_Click(object sender, EventArgs e)
    {
        try
        {
            long num = InsertSupplierInvoice();
            if (num > 0L)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Record Saved Successfully');", true);
                ClearFields();
                hlPrint.NavigateUrl = "../Reports/rptCashVouchers.aspx?inv=" + num.ToString();
                hlPrint.Visible = true;
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Record Saved failed. Please try again !');", true);
                hlPrint.NavigateUrl = "";
                hlPrint.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Record Saved failed. Please try again !');", true);
            hlPrint.NavigateUrl = "";
            hlPrint.Visible = false;
        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("SupplierInvoiceList.aspx");
    }
}