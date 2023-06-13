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


public partial class Hostel_HostExpensesEntry : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private DataSet ds = new DataSet();
    private DateTime TransDt = new DateTime();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
            Response.Redirect("../Login.aspx");
        lblMsg.Text = "";
        if (Page.IsPostBack)
            return;
        bindDropDown(drpCreditHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=7 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
        bindDropDown(drpAcHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=22 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
        txtInstrumentNo.Enabled = true;
        txtInstrumentDt.Enabled = true;
        TransDt = Convert.ToDateTime(obj.ExecuteScalarQry("select CONVERT(nvarchar,TransDate,106) from dbo.ACTS_MasterControl where ActsClosed=0"));
        dtpExpenseDt.SetDateValue(TransDt);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        InsertToTable();
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

    private void InsertToTable()
    {
        obj = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@DrAccountHead", drpAcHead.SelectedValue);
        if (rbtnMode.SelectedValue == "C")
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
            hashtable.Add("@InstrumentDt", dtpInstrumentDt.GetDateValue().ToString("MM/dd/yyyy"));
        dt = obj.GetDataTable("Host_InsUpdt_MiscExp", hashtable);
        if (dt.Rows.Count > 0)
        {
            lblMsg.Text = "Transaction can't be done on " + txtExpDate.Text.Trim();
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Data Saved Successfully !";
            lblMsg.ForeColor = Color.Green;
            ClearFields();
        }
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
        dtpExpenseDt.SetDateValue(Convert.ToDateTime(obj.ExecuteScalarQry("select CONVERT(nvarchar,TransDate,106) from dbo.ACTS_MasterControl where ActsClosed=0")));
        txtExpDetails.Text = string.Empty;
        txtAmount.Text = string.Empty;
        bindDropDown(drpCreditHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=7 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
    }

    protected void btnShowList_Click(object sender, EventArgs e)
    {
        Response.Redirect("HostExpenseList.aspx");
    }

    protected void optCashBank_CheckedChanged(object sender, EventArgs e)
    {
        if (optCashBank.Checked.Equals(true))
        {
            rbtnMode.Enabled = true;
            rbtnMode.SelectedValue = "B";
            txtInstrumentDt.Enabled = true;
            txtInstrumentNo.Enabled = true;
            bindDropDown(drpCreditHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=7 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
        }
        else
        {
            rbtnMode.Enabled = false;
            txtInstrumentDt.Enabled = false;
            txtInstrumentNo.Enabled = false;
            bindDropDown(drpCreditHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=11 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
            drpCreditHead.Enabled = true;
        }
    }

    protected void rbtnMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCreditHeads();
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
            string str = "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads AH INNER JOIN dbo.ACTS_AccountGroups AG ON AH.AG_Code=AG.AG_Code WHERE AG.AG_Code =7 ORDER BY AcctsHead ";
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
}