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

public partial class HR_LoanRecovery : System.Web.UI.Page
{
       protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (!Page.IsPostBack)
        {
            FillDrp(drpCreditAccount, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=7 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
            FillDrp(drpEmployee, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster Where ActiveStatus=1 ORDER BY EmpName", "EmpName", "EmpId");
            FillDrp(drpDedHead, "Select DedTypeId,DedDetails from dbo.HR_EmpDedMaster DM inner join dbo.Acts_AccountHeads AH on AH.AcctsHeadId=DM.AcctsHeadId where ah.AG_Code=25 order by DedDetails", "DedDetails", "DedTypeId");
        }
        lblMsg.Text = "";
    }

    private void FillDrp(DropDownList drp, string Qry, string Text, string Value)
    {
        DataTable dataTableQry = new clsDAL().GetDataTableQry(Qry.Trim());
        drp.DataSource = dataTableQry.DefaultView;
        drp.DataTextField = Text;
        drp.DataValueField = Value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void drpDedHead_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDedHead.SelectedIndex <= 0)
            return;
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Clear();
        hashtable.Add("@EmpId", drpEmployee.SelectedValue);
        hashtable.Add("@DedTypeId", drpDedHead.SelectedValue);
        DataTable dataTable2 = clsDal.GetDataTable("HR_CheckRecoverAmt", hashtable);
        if (dataTable2.Rows[0]["Msg"].ToString() == "")
        {
            ViewState["PendingAmt"] = 0;
            lblMsg.Text = "No OutStanding Amount";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            ViewState["PendingAmt"] = dataTable2.Rows[0]["Amt"].ToString();
            lblMsg.Text = dataTable2.Rows[0]["Msg"].ToString();
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@TransDate", dtpTransDt.GetDateValue());
        hashtable.Add("@EmpId", drpEmployee.SelectedValue);
        hashtable.Add("@DedTypeId", drpDedHead.SelectedValue);
        if (ViewState["PendingAmt"] != null)
        {
            if (ViewState["PendingAmt"].ToString() == "0")
            {
                lblMsg.Text = "No Dues To Pay";
                lblMsg.ForeColor = Color.Red;
                return;
            }
            if (Convert.ToDecimal(ViewState["PendingAmt"].ToString()) < Convert.ToDecimal(txtAmount.Text.Trim()))
            {
                lblMsg.Text = "Please Pay Max Amount : " + Convert.ToDecimal(ViewState["PendingAmt"].ToString()).ToString("0.00");
                lblMsg.ForeColor = Color.Red;
                return;
            }
        }
        if (rbtnMode.SelectedValue == "B")
        {
            if (drpCreditAccount.SelectedIndex == 0 || txtInstrumentNo.Text.Trim() == "" || txtInstrumentDt.Text.Trim() == "")
            {
                lblMsg.Text = "Please Enter Bank Name / Instrument Date / Instrument No";
                lblMsg.ForeColor = Color.Red;
                return;
            }
            hashtable.Add("@PayMode", "Bank");
            hashtable.Add("@DrId", drpCreditAccount.SelectedValue);
            hashtable.Add("@InstrumentNo", txtInstrumentNo.Text.Trim());
            hashtable.Add("@InstrumentDt", dtpInstrumentDt.GetDateValue());
        }
        else
        {
            hashtable.Add("@PayMode", "Cash");
            hashtable.Add("@DrId", 3);
        }
        hashtable.Add("@Amount", txtAmount.Text.Trim());
        hashtable.Add("@PmtDtls", txtDetails.Text.Trim());
        hashtable.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
        string str = clsDal.ExecuteScalar("HR_UpdtLoanRepay", hashtable);
        if (str == "SUCCESS")
        {
            lblMsg.Text = "Loan Amount Paid Successfully";
            lblMsg.ForeColor = Color.Green;
            Clear();
        }
        else
        {
            lblMsg.Text = str.ToString();
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        txtTransactionDate.Text = "";
        drpEmployee.SelectedIndex = 0;
        drpDedHead.SelectedIndex = 0;
        txtAmount.Text = "";
        txtDetails.Text = "";
        rbtnMode.SelectedValue = "C";
        enabletxt();
    }

    protected void drpEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDedHead.SelectedIndex <= 0)
            return;
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Clear();
        hashtable.Add("@EmpId", drpEmployee.SelectedValue);
        hashtable.Add("@DedTypeId", drpDedHead.SelectedValue);
        DataTable dataTable2 = clsDal.GetDataTable("HR_CheckRecoverAmt", hashtable);
        if (dataTable2.Rows[0]["Msg"].ToString() == "")
        {
            ViewState["PendingAmt"] = 0;
            lblMsg.Text = "No OutStanding Amount";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            ViewState["PendingAmt"] = dataTable2.Rows[0]["Amt"].ToString();
            lblMsg.Text = dataTable2.Rows[0]["Msg"].ToString();
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void rbtnMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        enabletxt();
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
}