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

public partial class Hostel_HostRcptPrevDue : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblerr.Text = "";
        if (Page.IsPostBack)
            return;
        Session["PaidFine"] = (object)"0";
        ViewState["FY"] = (object)"n";
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
        drpSession.DataSource = (object)new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        DataTable dataTable = new Common().ExecuteSql("select * from ps_classmaster");
        drpclass.DataSource = (object)dataTable;
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
        drpclass.DataSource = (object)dataTable;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("-Select-", "0"));
        if (dataTable.Rows.Count > 0)
            fillstudent();
        else
            drpclass.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpstudent.Items.Clear();
        drpclass.SelectedIndex = 0;
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    private void fillstudent()
    {
        DataTable dataTable = new Common().ExecuteSql("select fullname,s.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno inner join dbo.Host_Admission h on h.admnno=cs.admnno and h.LeavingDt is null where cs.classid=" + drpclass.SelectedValue + " and cs.SessionYear='" + drpSession.SelectedValue.ToString().Trim() + "' order by fullname");
        drpstudent.DataSource = (object)dataTable;
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

    protected void FillFeeAmt()
    {
        txtadmnno.Text = drpstudent.SelectedValue;
        getbalance(drpstudent.SelectedValue.ToString());
    }

    protected void btnDetail_Click(object sender, EventArgs e)
    {
        string str = new Common().ExecuteScalarQry("select s.admnno from dbo.PS_ClasswiseStudent c inner join  dbo.Host_Admission s on c.admnno=s.AdmnNo where s.LeavingDt is null and c.admnno=" + txtadmnno.Text.ToString().Trim());
        if (str != "")
        {
            if (drpstudent.SelectedValue.ToString().Trim() != txtadmnno.Text.ToString().Trim())
            {
                fillStudent();
                drpstudent.SelectedValue = str;
            }
            getbalance(txtadmnno.Text.ToString().Trim());
            ViewState["FY"] = (object)"n";
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('Invalid Admission Number !');", true);
    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        lblerr.Text = "";
        if (Convert.ToInt32(new clsDAL().ExecuteScalarQry("select count(*) from dbo.Acts_PaymentReceiptVoucher where TransDt > '" + PopCalendar2.GetDateValue().ToString("dd MMM yyyy") + "' and (IsDeleted is null or IsDeleted=0)")) > 0)
        {
            lblerr.Text = "Receipt already generated for later date.Receipt can not be generated now for " + PopCalendar2.GetDateValue().ToString("dd MMM yyyy");
            lblerr.ForeColor = Color.Red;
        }
        else
            SaveData();
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
                Response.Redirect("rptHostPrevDueRcpt.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + " &status=1 &rno=" + RcptNo + " &class=" + drpclass.SelectedValue.ToString() + " &D=p");
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
            hashtable.Add((object)"Session", (object)drpSession.SelectedValue);
            hashtable.Add((object)"RecvDate", (object)PopCalendar2.GetDateValue().ToString("dd MMM yyyy"));
            hashtable.Add((object)"AdmnNo", (object)txtadmnno.Text);
            hashtable.Add((object)"Description", (object)"Hostel Previous due");
            hashtable.Add((object)"Amount", (object)num2);
            hashtable.Add((object)"UserID", (object)Convert.ToInt32(Session["User_Id"].ToString().Trim()));
            hashtable.Add((object)"SchoolID", (object)Session["SchoolId"].ToString());
            Common common = new Common();
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = common.GetDataTable("Host_Ins_prevfeecash", hashtable);
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
        hashtable.Add((object)"@Receipt_VrNo", (object)RcptNo);
        hashtable.Add((object)"TransDate", (object)PopCalendar2.GetDateValue().ToString("dd MMM yyyy"));
        hashtable.Add((object)"AdmnNo", (object)Convert.ToInt64(txtadmnno.Text));
        hashtable.Add((object)"TransDesc", (object)"Hostel Previous due");
        hashtable.Add((object)"debit", (object)0);
        hashtable.Add((object)"credit", (object)result2);
        hashtable.Add((object)"balance", (object)0);
        hashtable.Add((object)"userid", (object)Convert.ToInt32(Session["User_Id"].ToString()));
        hashtable.Add((object)"UserDate", (object)DateTime.Now.ToString("dd MMM yyyy"));
        hashtable.Add((object)"schoolid", (object)Session["SchoolId"].ToString());
        hashtable.Add((object)"PayMode", (object)"Cash");
        hashtable.Add((object)"Session", (object)drpSession.SelectedValue.ToString().Trim());
        DataTable dataTable = new DataTable();
        if (new Common().GetDataTable("Host_insert_FeeLedgerPrevBal", hashtable).Rows.Count > 0)
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Previous due already received !');", true);
        else
            updatefeeledger(Balance, RcptNo);
    }

    private void updatefeeledger(Decimal Balance, string RcptNo)
    {
        new Common().GetDataTable("Host_update_feeledgerPrevBal", new Hashtable()
    {
      {
        (object) "@Receipt_VrNo",
        (object) RcptNo
      },
      {
        (object) "AdmnNo",
        (object) txtadmnno.Text
      },
      {
        (object) "balance",
        (object) Balance
      },
      {
        (object) "UserID",
        (object) Session["User_Id"].ToString()
      },
      {
        (object) "UserDate",
        (object) DateTime.Now.ToString("dd MMM yyyy")
      },
      {
        (object) "SchoolID",
        (object) Session["SchoolId"].ToString()
      },
      {
        (object) "mode",
        (object) "Cash"
      }
    });
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("HostRcptPrevDueList.aspx");
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

    protected void txtadminno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Session["PaidFine"] = (object)"0";
            Session["CreditAmount"] = (object)"0";
            Common common = new Common();
            if (Convert.ToInt32(common.ExecuteScalarQry("select count(*) from dbo.Host_Admission where AdmnNo=" + txtadmnno.Text + " and LeavingDt is null")) == 0)
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtadmnno, txtadmnno.GetType(), "ShowMessage", "alert('Admission number does not exists')", true);
            }
            else
            {
                getbalance(txtadmnno.Text.Trim());
                fillStudent();
                drpstudent.SelectedValue = txtadmnno.Text;
                drpclass.SelectedValue = common.ExecuteSql("select section,ClassID from PS_ClasswiseStudent cs inner join dbo.Host_Admission on cs.admnno=h.admnno where cs.admnno=" + txtadmnno.Text + " and h.LeavingDt is null").Rows[0]["ClassID"].ToString();
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void getbalance(string AdmnNo)
    {
        lblbalance.Text = new Common().ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(Balance),0)) from dbo.Host_FeeLedger where TransDesc='Previous Balance' and AdmnNo=" + AdmnNo);
        string text = lblbalance.Text;
    }

    private void FillSelStudent()
    {
        drpSession.SelectedValue = Request.QueryString["sess"].ToString();
        drpclass.SelectedValue = Request.QueryString["cid"].ToString();
        fillstudent();
        drpstudent.SelectedValue = Request.QueryString["admnno"].ToString().Trim();
        FillFeeAmt();
    }

    private void fillStudent()
    {
        drpstudent.DataSource = (object)new Common().ExecuteSql("select fullname,s.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno inner join dbo.Host_Admission h on cs.admnno=h.admnno where h.LeavingDt is null");
        drpstudent.DataTextField = "fullname";
        drpstudent.DataValueField = "admnno";
        drpstudent.DataBind();
    }
}