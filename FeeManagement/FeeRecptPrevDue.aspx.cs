using ASP;
using Classes.DA;
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

public partial class FeeManagement_FeeRecptPrevDue : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblerr.Text = "";
        if (Page.IsPostBack)
            return;
        Session["PaidFine"] = "0";
        ViewState["FY"] = "n";
        Page.Form.DefaultButton = btnsave.UniqueID;
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        fillsession();
        fillclass();
        PopCalendar2.SetDateValue(DateTime.Today);
        txtadmnno.Focus();
        if (Request.QueryString["admnno"] == null)
            return;
        FillSelStudent();
        fillstudent();
        txtadmnno.Text = drpstudent.SelectedValue;
        getbalance(drpstudent.SelectedValue.ToString());
    }

    private void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    protected void drpsession_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    private void fillclass()
    {
        DataTable dataTable = new Common().ExecuteSql("select * from ps_classmaster");
        drpclass.DataSource = dataTable;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("-Select-", "0"));
        if (dataTable.Rows.Count > 0)
            fillstudent();
        else
            drpclass.Items.Insert(0, new ListItem("No Record", "0"));
    }

    private void fillclassWithAdmnNo()
    {
        DataTable dataTable = new Common().ExecuteSql("select * from ps_classmaster");
        drpclass.DataSource = dataTable;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("-Select-", "0"));
        if (dataTable.Rows.Count > 0)
            fillstudent();
        else
            drpclass.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    private void fillstudent()
    {
        DataTable dataTable = new Common().ExecuteSql("select fullname,s.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.classid=" + drpclass.SelectedValue + " and cs.SessionYear='" + drpSession.SelectedValue.ToString().Trim() + "' order by fullname");
        drpstudent.DataSource = dataTable;
        drpstudent.DataTextField = "fullname";
        drpstudent.DataValueField = "admnno";
        drpstudent.DataBind();
        if (dataTable.Rows.Count > 0)
        {
            drpstudent.Items.Insert(0, new ListItem("-Select-", "0"));
            txtadmnno.Text = "0";
        }
        else
        {
            drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
            lblbalance.Text = "";
            txtadmnno.Text = "";
        }
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillFeeAmt();
    }

    private void getbalance(string AdmnNo)
    {
        lblbalance.Text = new Common().ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(Balance),0)) from dbo.PS_FeeLedger where TransDesc='Previous Balance' and AdmnNo=" + AdmnNo);
        string text = lblbalance.Text;
    }

    protected void FillFeeAmt()
    {
        txtadmnno.Text = drpstudent.SelectedValue;
        getbalance(drpstudent.SelectedValue.ToString());
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblerr.Text = "";
        clsDAL clsDal = new clsDAL();
        string str = "select count(*) from dbo.Acts_PaymentReceiptVoucher where TransDt > '" + PopCalendar2.GetDateValue().ToString("dd MMM yyyy") + "' and (IsDeleted is null or IsDeleted=0)";
        if (clsDal.GetDataTableQry("select top(1) FY,StartDate,EndDate from dbo.ACTS_FinancialYear where (IsFinalized is null or IsFinalized=0) and fy=dbo.fuGetSessionYr('" + PopCalendar2.GetDateValue().ToString("dd MMM yyyy") + "') order by FY_Id desc").Rows.Count > 0)
        {
            if (clsDal.ExecuteScalarQry("select fy from [dbo].[ACTS_FinancialYear] where StartDate<='" + PopCalendar2.GetDateValue().ToString("dd MMM yyyy") + "' and enddate>='" + PopCalendar2.GetDateValue().ToString("dd MMM yyyy") + "'").ToString().Trim() != clsDal.ExecuteScalarQry("select fy from [dbo].[ACTS_FinancialYear] where StartDate<='" + DateTime.Now.ToString("dd MMM yyyy") + "' and enddate>='" + DateTime.Now.ToString("dd MMM yyyy") + "'").ToString().Trim())
            {
                lblerr.Text = "Receive date is not within the current Financial Year";
                lblerr.ForeColor = Color.Red;
            }
            else
                SaveData();
        }
        else
        {
            lblerr.Text = "No running Financial available For Given Trans Date !!";
            lblerr.ForeColor = Color.Red;
        }
    }

    private void SaveData()
    {
        Decimal result1 = new Decimal(0);
        Decimal result2 = new Decimal(0);
        Decimal.TryParse(lblbalance.Text, out result1);
        Decimal.TryParse(txtamt.Text, out result2);
        if (result1 < result2)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Receive amount cannot greater than due amount !');", true);
        }
        else
        {
            string RcptNo = string.Empty;
            save(out RcptNo);
            if (RcptNo.Trim() != "")
                Response.Redirect("../Reports/rptPrevDueReceipt.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + " &status=1 &rno=" + RcptNo + " &class=" + drpclass.SelectedValue.ToString() + " &D=p");
            clear();
        }
    }

    private void save(out string RcptNo)
    {
        Decimal num1 = new Decimal(0);
        Decimal num2;
        try
        {
            num2 = Convert.ToDecimal(txtamt.Text);
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock((Control)txtamt, txtamt.GetType(), "ShowMessage", "alert('Invalid amount');", true);
            RcptNo = string.Empty;
            return;
        }
        if (num2 <= new Decimal(0))
        {
            ScriptManager.RegisterClientScriptBlock((Control)txtamt, txtamt.GetType(), "ShowMessage", "alert('Enter Amount greater than 0.');", true);
            RcptNo = string.Empty;
        }
        else
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("Session", drpSession.SelectedValue);
            hashtable.Add("RecvDate", PopCalendar2.GetDateValue().ToString("dd MMM yyyy"));
            hashtable.Add("AdmnNo", txtadmnno.Text);
            hashtable.Add("Description", "Previous due");
            hashtable.Add("Amount", num2);
            hashtable.Add("UserID", Convert.ToInt32(Session["User_Id"].ToString().Trim()));
            hashtable.Add("SchoolID", Session["SchoolId"].ToString());
            Common common = new Common();
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = common.GetDataTable("ps_sp_insert_prevfeecash", hashtable);
            if (dataTable2.Rows[0][0].ToString().Trim() != "" && dataTable2.Rows[0][0].ToString().Trim().ToUpper() != "DUP" && dataTable2.Rows[0][0].ToString().Trim().ToUpper() != "ERROR")
            {
                RcptNo = dataTable2.Rows[0][0].ToString();
                insertfeeledger(RcptNo);
            }
            else if (dataTable2.Rows[0][0].ToString().Trim().ToUpper() != "DUP")
            {
                lblerr.Text = "Receipt No already Exists!";
                lblerr.ForeColor = Color.Red;
                RcptNo = string.Empty;
            }
            else
            {
                lblerr.Text = "Transaction Failed To Generate Receiptno!!!!";
                lblerr.ForeColor = Color.Red;
                RcptNo = string.Empty;
            }
        }
    }

    private void insertfeeledger(string RcptNo)
    {
        Decimal result1 = new Decimal(0);
        Decimal num = new Decimal(0);
        Decimal result2 = new Decimal(0);
        Decimal.TryParse(lblbalance.Text, out result1);
        Decimal.TryParse(txtamt.Text, out result2);
        Decimal Balance = result1 - result2;
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Receipt_VrNo", RcptNo);
        hashtable.Add("TransDate", PopCalendar2.GetDateValue().ToString("dd MMM yyyy"));
        hashtable.Add("AdmnNo", Convert.ToInt64(txtadmnno.Text));
        hashtable.Add("TransDesc", "Previous due");
        hashtable.Add("debit", 0);
        hashtable.Add("credit", result2);
        hashtable.Add("balance", 0);
        hashtable.Add("userid", Convert.ToInt32(Session["User_Id"].ToString()));
        hashtable.Add("UserDate", DateTime.Now.ToString("dd MMM yyyy"));
        hashtable.Add("schoolid", Session["SchoolId"].ToString());
        hashtable.Add("PayMode", "Cash");
        hashtable.Add("Session", drpSession.SelectedValue.ToString().Trim());
        DataTable dataTable = new DataTable();
        if (new Common().GetDataTable("ps_sp_insert_FeeLedgerPrevBal", hashtable).Rows.Count > 0)
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Previous due already received !');", true);
        else
            updatefeeledger(Balance, RcptNo);
    }

    private void updatefeeledger(Decimal Balance, string RcptNo)
    {
        new Common().GetDataTable("ps_sp_update_feeledgerPrevBal", new Hashtable()
    {
      {
         "@Receipt_VrNo",
         RcptNo
      },
      {
         "AdmnNo",
         txtadmnno.Text
      },
      {
         "balance",
         Balance
      },
      {
         "UserID",
         Session["User_Id"].ToString()
      },
      {
         "UserDate",
         DateTime.Now.ToString("dd MMM yyyy")
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

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("FeeRecptPrevDueList.aspx");
    }

    private void clear()
    {
        txtamt.Text = string.Empty;
        PopCalendar2.SetDateValue(DateTime.Today);
    }

    private void DisableControls()
    {
        drpSession.Enabled = false;
        drpclass.Enabled = false;
        drpstudent.Enabled = false;
        txtadmnno.ReadOnly = true;
    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        if (drpstudent.SelectedIndex > 0)
        {
            if (lblbalance.Text != "0.00")
            {
                Session["PaidFine"] = "0";
                Response.Redirect("../Reports/rptPrevDueReceipt.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &D=d&FY=f");
            }
            else
                ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails, btnShowDetails.GetType(), "ShowMessage", "alert('No dues for the selected student');", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails, btnShowDetails.GetType(), "ShowMessage", "alert('Please select a student !');", true);
    }

    protected void txtadminno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Session["PaidFine"] = "0";
            Session["CreditAmount"] = "0";
            Common common = new Common();
            if (Convert.ToInt32(common.ExecuteScalarQry("select count(*) from PS_StudMaster where AdmnNo=" + txtadmnno.Text)) == 0)
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtadmnno, txtadmnno.GetType(), "ShowMessage", "alert('Admission number does not exists')", true);
            }
            else
            {
                getbalance(txtadmnno.Text.Trim());
                fillStudent();
                drpstudent.SelectedValue = txtadmnno.Text;
                drpclass.SelectedValue = common.ExecuteSql("select section,ClassID from PS_ClasswiseStudent where admnno=" + txtadmnno.Text).Rows[0]["ClassID"].ToString();
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void FillSelStudent()
    {
        drpSession.SelectedValue = Request.QueryString["sess"].ToString();
        drpclass.SelectedValue = Request.QueryString["cid"].ToString();
        fillstudent();
        drpstudent.SelectedValue = Request.QueryString["admnno"].ToString().Trim();
        FillFeeAmt();
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpstudent.Items.Clear();
        drpclass.SelectedIndex = 0;
    }

    protected void btnDetail_Click(object sender, EventArgs e)
    {
        string str = new Common().ExecuteScalarQry("select s.admnno from dbo.PS_ClasswiseStudent c inner join  dbo.PS_StudMaster s on c.admnno=s.AdmnNo where c.admnno=" + txtadmnno.Text.ToString().Trim());
        if (str != "")
        {
            if (drpstudent.SelectedValue.ToString().Trim() != txtadmnno.Text.ToString().Trim())
            {
                fillStudent();
                drpstudent.SelectedValue = str;
            }
            getbalance(txtadmnno.Text.ToString().Trim());
            ViewState["FY"] = "n";
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('Invalid Admission Number !');", true);
    }

    private void fillStudent()
    {
        drpstudent.DataSource = new Common().ExecuteSql("select fullname,s.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno");
        drpstudent.DataTextField = "fullname";
        drpstudent.DataValueField = "admnno";
        drpstudent.DataBind();
    }
}