using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_SalaryPayment : System.Web.UI.Page
{
    private clsDAL ObjDAL = new clsDAL();
    private DataTable dt;
    private Hashtable ht;
    private int RecCount;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!ChkIsHRUsed())
            {
                Response.Redirect("HRHome.aspx?IsHRUsed=0");
            }
            else
            {
                BindSalYear();
                bindDropDown(drpDesignation, "select DesgId,Designation from dbo.HR_DesignationMaster order by Designation", "Designation", "DesgId");
                bindDropDown(drpEmployee, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster ORDER BY EmpName", "EmpName", "EmpId");
                bindDropDown(drpBank, "select AcctsHeadId, AcctsHead from dbo.Acts_AccountHeads where AG_Code=7", "AcctsHead", "AcctsHeadId");
                drpBank.Items.RemoveAt(0);
                drpBank.Items.Insert(0, new ListItem("- SELECT -", "0"));
            }
        }
        lblMsg.Text = string.Empty;
    }

    private bool ChkIsHRUsed()
    {
        return ObjDAL.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void BindSalYear()
    {
        int year = DateTime.Now.Year;
        int num = year - 1;
        drpYear.Items.Insert(0, new ListItem(year.ToString(), year.ToString()));
        drpYear.Items.Insert(1, new ListItem(num.ToString(), num.ToString()));
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        ObjDAL = new clsDAL();
        dt = ObjDAL.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    private void ClearFields()
    {
        drpDesignation.SelectedIndex = 0;
        drpMonth.SelectedIndex = -1;
        drpYear.SelectedIndex = -1;
        btnPay.Visible = false;
        lblNoOfRec.Text = string.Empty;
        lblTotal.Text = "0.00";
        lblMessage.Text = string.Empty;
    }

    private void FillGrid()
    {
        lblMsg.Text = string.Empty;
        dt = new DataTable();
        ObjDAL = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select SalId,EmpName,Designation,Pay,GrossTot,TotalDeduction,NetSal,ISNULL(PaidAmount,0) AS PaidAmount,EmpId,SalPaidDate,PaidDays from HR_view_MonthlySalaryNew where [Year]=" + drpYear.SelectedValue + " and [Month]='" + drpMonth.SelectedValue + "'");
        stringBuilder.Append(" and PayWithHeld='N' and SalPaidDate IS NULL");
        if (drpDesignation.SelectedIndex > 0)
            stringBuilder.Append(" and DesignationId=" + drpDesignation.SelectedValue);
        if (drpEmployee.SelectedIndex > 0)
            stringBuilder.Append(" and EmpId=" + drpEmployee.SelectedValue);
        stringBuilder.Append(" Order by EmpName");
        dt = ObjDAL.GetDataTableQry(stringBuilder.ToString());
        if (dt.Rows.Count > 0)
        {
            if (Convert.ToInt32(ObjDAL.ExecuteScalarQry("SELECT COUNT(*) FROM HR_view_MonthlySalaryNew WHERE SalPaidDate IS NOT NULL AND [Year]=" + drpYear.SelectedValue + " AND [Month]='" + drpMonth.SelectedValue + "'")) == dt.Rows.Count)
            {
                btnPay.Visible = false;
                lblMessage.Text = "Salary has already been Paid for " + drpMonth.SelectedItem.Text + ", " + drpYear.SelectedValue;
                lblMessage.ForeColor = Color.Red;
            }
            else
            {
                btnPay.Visible = true;
                lblMessage.Text = string.Empty;
            }
        }
        else
        {
            btnPay.Visible = false;
            lblMessage.Text = string.Empty;
        }
        grdEmpPament.DataSource = dt;
        grdEmpPament.DataBind();
        lblNoOfRec.Text = "No Of Records : " + dt.Rows.Count.ToString();
        GetTotPay();
    }

    private void GetTotPay()
    {
        Decimal num = new Decimal(0);
        foreach (GridViewRow row in grdEmpPament.Rows)
        {
            if (btnPay.Visible)
            {
                TextBox control = (TextBox)row.FindControl("txtPaidAmt");
                num += Convert.ToDecimal(control.Text);
            }
            else
            {
                Label control = (Label)row.FindControl("lblPaid");
                if (control.Text.Trim() != string.Empty)
                    num += Convert.ToDecimal(control.Text);
            }
        }
        lblTotal.Text = num.ToString("0.00");
    }

    private void GetTotPayPending()
    {
        Decimal num = new Decimal(0);
        foreach (GridViewRow row in grdEmpPending.Rows)
        {
            if (btnPay.Visible)
            {
                TextBox control = (TextBox)row.FindControl("txtPaidAmt");
                num += Convert.ToDecimal(control.Text);
            }
            else
            {
                Label control = (Label)row.FindControl("lblPaid");
                if (control.Text.Trim() != string.Empty)
                    num += Convert.ToDecimal(control.Text);
            }
        }
        lblTotal.Text = num.ToString("0.00");
    }

    protected void btnPay_Click(object sender, EventArgs e)
    {
        string str = ObjDAL.ExecuteScalar("HR_ChkSalGenNextMonth", new Hashtable()
    {
      {
         "SalYr",
         drpYear.SelectedValue
      },
      {
         "SalMonth",
         drpMonth.SelectedValue
      }
    });
        if (str.Trim().ToUpper() != "S")
        {
            lblMsg.Text = str;
            lblMsg.ForeColor = Color.Red;
        }
        else if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnPay, btnPay.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string idlist = Request["Checkb"];
            if (rdbtnPayMode.SelectedValue == "1")
            {
                if (drpBank.SelectedIndex <= 0)
                    return;
                if (grdEmpPament.Visible)
                    SetStatus(idlist);
                else if (grdEmpPending.Visible)
                {
                    SetStatusPending(idlist);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock((Control)btnPay, btnPay.GetType(), "ShowMessage", "alert('Select Bank Name');", true);
                    (Master.FindControl("ScriptManager1") as ScriptManager).SetFocus(drpBank.ClientID);
                }
            }
            else if (grdEmpPament.Visible)
                SetStatus(idlist);
            else
                SetStatusPending(idlist);
        }
    }

    private void SetStatus(string idlist)
    {
        string[] strArray = idlist.Split(',');
        Decimal amt = new Decimal(0);
        string str1 = "";
        string str2 = "";
        for (int index = 0; index < strArray.Length; ++index)
        {
            foreach (GridViewRow row in grdEmpPament.Rows)
            {
                HiddenField control1 = (HiddenField)row.FindControl("hdnSalId");
                HiddenField control2 = (HiddenField)row.FindControl("hdnNetAmt");
                TextBox control3 = (TextBox)row.FindControl("txtPaidAmt");
                if (control1.Value == strArray[index].ToString())
                {
                    if (Convert.ToDecimal(control3.Text) > Convert.ToDecimal(control2.Value))
                    {
                        str2 = "1";
                        control3.Focus();
                        break;
                    }
                    if (control3.Text.Trim() == "")
                        control3.Text = Convert.ToDecimal(control2.Value).ToString("0.00");
                    amt += Convert.ToDecimal(control3.Text.Trim());
                    str1 = str1 + Convert.ToDecimal(control3.Text.Trim()).ToString("0.00") + ",";
                    break;
                }
            }
            if (str2 != "")
                break;
        }
        if (str2.Trim() == "")
        {
            if (chkCashBal(amt))
            {
                ObjDAL = new clsDAL();
                ht = new Hashtable();
                ht.Clear();
                ht.Add("@SalYear", drpYear.SelectedValue);
                ht.Add("@SalMonth", drpMonth.SelectedValue);
                ht.Add("@SalIdList", idlist);
                ht.Add("@PaidAmtList", str1);
                ht.Add("@PaidDate", dtpPaymentDt.GetDateValue());
                if (rdbtnPayMode.SelectedValue == "0")
                {
                    ht.Add("@CrHeadId", 3);
                    ht.Add("@PayMode", "Cash");
                }
                else
                {
                    ht.Add("@CrHeadId", int.Parse(drpBank.SelectedValue));
                    ht.Add("@PayMode", "Bank");
                }
                ht.Add("@InstrumentNo", txtInstrNo.Text.Trim());
                ht.Add("@BillNo", txtPayBill.Text.Trim());
                ht.Add("@SchoolId", Session["SchoolId"]);
                ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
                string str3 = ObjDAL.ExecuteScalar("HR_UpdtPayment", ht);
                FillGrid();
                if (str3.Trim() == "")
                {
                    clearAll();
                    lblMsg.Text = "Payment Made Successful.";
                    lblMsg.ForeColor = Color.Green;
                }
                else if (str3.Trim() == "DUP")
                {
                    lblMsg.Text = "Bill No Already Exists!!";
                    lblMsg.ForeColor = Color.Red;
                }
                else
                {
                    lblMsg.Text = "Transaction Failed. Try Again";
                    lblMsg.ForeColor = Color.Red;
                }
            }
            else
            {
                lblMsg.Text = "Cash Balance not enough to complete this transaction !!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
        {
            lblMsg.Text = "Paid amount cannot be more than the Payble amount!!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void SetStatusPending(string idlist)
    {
        string[] strArray = idlist.Split(',');
        Decimal amt = new Decimal(0);
        string str1 = "";
        string str2 = "";
        for (int index = 0; index < strArray.Length; ++index)
        {
            foreach (GridViewRow row in grdEmpPending.Rows)
            {
                HiddenField control1 = (HiddenField)row.FindControl("hdnSalId");
                HiddenField control2 = (HiddenField)row.FindControl("hdnNetAmt");
                TextBox control3 = (TextBox)row.FindControl("txtPaidAmt");
                if (control1.Value == strArray[index].ToString())
                {
                    if (Convert.ToDecimal(control3.Text) > Convert.ToDecimal(control2.Value))
                    {
                        str2 = "1";
                        control3.Focus();
                        break;
                    }
                    if (control3.Text.Trim() == "")
                        control3.Text = Convert.ToDecimal(control2.Value).ToString("0.00");
                    amt += Convert.ToDecimal(control3.Text.Trim());
                    str1 = str1 + Convert.ToDecimal(control3.Text.Trim()).ToString("0.00") + ",";
                    break;
                }
            }
            if (str2 != "")
                break;
        }
        if (str2.Trim() == "")
        {
            if (chkCashBal(amt))
            {
                ObjDAL = new clsDAL();
                ht = new Hashtable();
                ht.Clear();
                ht.Add("@SalYear", drpYear.SelectedValue);
                ht.Add("@SalMonth", drpMonth.SelectedValue);
                ht.Add("@SalIdList", idlist);
                ht.Add("@PaidAmtList", str1);
                ht.Add("@PaidDate", dtpPaymentDt.GetDateValue());
                if (rdbtnPayMode.SelectedValue == "0")
                {
                    ht.Add("@CrHeadId", 3);
                    ht.Add("@PayMode", "Cash");
                }
                else
                {
                    ht.Add("@CrHeadId", int.Parse(drpBank.SelectedValue));
                    ht.Add("@PayMode", "Bank");
                }
                ht.Add("@InstrumentNo", txtInstrNo.Text.Trim());
                ht.Add("@BillNo", txtPayBill.Text.Trim());
                ht.Add("@SchoolId", Session["SchoolId"]);
                ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
                string str3 = ObjDAL.ExecuteScalar("HR_UpdtPendPayment", ht);
                FillGridPending();
                if (str3.Trim() == "")
                {
                    clearAll();
                    lblMsg.Text = "Payment Made Successful.";
                    lblMsg.ForeColor = Color.Green;
                }
                else if (str3.Trim() == "DUP")
                {
                    lblMsg.Text = "Bill No Already Exists!!";
                    lblMsg.ForeColor = Color.Red;
                }
                else
                {
                    lblMsg.Text = "Transaction Failed. Try Again";
                    lblMsg.ForeColor = Color.Red;
                }
            }
            else
            {
                lblMsg.Text = "Cash Balance not enough to complete this transaction !!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
        {
            lblMsg.Text = "Paid amount cannot be more than the Pending amount!!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private bool chkCashBal(Decimal amt)
    {
        if (!(rdbtnPayMode.SelectedValue.Trim() == "0"))
            return true;
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal = new clsDAL();
        hashtable.Add("@CurrTransDt", dtpPaymentDt.GetDateValue().ToString("dd MMM yyyy"));
        string str = clsDal.ExecuteScalar("ACTS_ChkCashBalance", hashtable);
        return !(str.Trim() == "") && !(Convert.ToDecimal(str.Trim()) < amt);
    }

    protected void drpDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDesignation.SelectedIndex > 0)
            bindDropDown(drpEmployee, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster where DesignationId=" + drpDesignation.SelectedValue + " ORDER BY EmpName", "EmpName", "EmpId");
        else
            bindDropDown(drpEmployee, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster ORDER BY EmpName", "EmpName", "EmpId");
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        grdEmpPament.Visible = true;
        grdEmpPending.Visible = false;
        FillGrid();
    }

    protected void btnShowPending_Click(object sender, EventArgs e)
    {
        grdEmpPament.Visible = false;
        grdEmpPending.Visible = true;
        FillGridPending();
    }

    private void FillGridPending()
    {
        lblMsg.Text = string.Empty;
        dt = new DataTable();
        ObjDAL = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Year", drpYear.SelectedValue.Trim());
        hashtable.Add("@Month", drpMonth.SelectedValue.Trim());
        if (drpEmployee.SelectedIndex > 0)
            hashtable.Add("@EmpId", drpEmployee.SelectedValue.Trim());
        if (drpDesignation.SelectedIndex > 0)
            hashtable.Add("@Designation", drpDesignation.SelectedValue.Trim());
        dt = ObjDAL.GetDataTable("HR_GetEmpSalPending", hashtable);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0][0].ToString().Trim().ToUpper() == "ALREADY")
            {
                lblMessage.Text = "Salary has already been Paid for next Month!!";
                lblMessage.ForeColor = Color.Red;
                grdEmpPending.DataSource = null;
                grdEmpPending.DataBind();
                lblNoOfRec.Text = "";
                btnPay.Visible = false;
                lblMessage.Text = string.Empty;
            }
            else
            {
                grdEmpPending.DataSource = dt;
                grdEmpPending.DataBind();
                lblNoOfRec.Text = "No Of Records : " + dt.Rows.Count.ToString();
                GetTotPayPending();
                btnPay.Visible = true;
                lblMessage.Text = string.Empty;
            }
        }
        else
        {
            grdEmpPending.DataSource = null;
            grdEmpPending.DataBind();
            lblNoOfRec.Text = "";
            btnPay.Visible = false;
            lblMessage.Text = string.Empty;
        }
    }

    protected void rdbtnPayMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtnPayMode.SelectedValue == "0")
        {
            drpBank.SelectedIndex = 0;
            drpBank.Enabled = false;
        }
        else
            drpBank.Enabled = true;
        drpBank.Focus();
    }

    protected void grdEmpPament_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        HiddenField control1 = (HiddenField)e.Row.FindControl("hfEmpId");
        HiddenField control2 = (HiddenField)e.Row.FindControl("hdnNetAmt");
        HiddenField control3 = (HiddenField)e.Row.FindControl("hfGross");
        HiddenField control4 = (HiddenField)e.Row.FindControl("hfDed");
        Label control5 = (Label)e.Row.FindControl("lblPrevAmt");
        Label control6 = (Label)e.Row.FindControl("lblPaid");
        Label control7 = (Label)e.Row.FindControl("lblCurMonth");
        Label control8 = (Label)e.Row.FindControl("lblGross");
        TextBox control9 = (TextBox)e.Row.FindControl("txtPaidAmt");
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@EmpId", control1.Value);
        hashtable.Add("@Month", drpMonth.SelectedItem.Text.Trim().ToUpper());
        hashtable.Add("@Year", drpYear.SelectedValue);
        DataTable dataTable = ObjDAL.GetDataTable("HR_GetPrevAmt", hashtable);
        string str1 = dataTable.Rows[0]["ClaimAmt"].ToString();
        string str2 = dataTable.Rows[0]["PrevAmt"].ToString();
        string str3 = dataTable.Rows[0]["Arrear"].ToString();
        if (str1.Trim() == string.Empty)
            str1 = "0";
        if (str2.Trim() == string.Empty)
            str2 = "0";
        if (str3.Trim() == string.Empty)
            str3 = "0";
        Decimal d = Convert.ToDecimal(str2) + Convert.ToDecimal(str3) + Convert.ToDecimal(str1);
        control7.Text = Math.Round(Convert.ToDecimal(control3.Value) - Convert.ToDecimal(str1) - Convert.ToDecimal(control4.Value)).ToString("0.00");
        control5.Text = Math.Round(d).ToString("0.00");
        control9.Text = Math.Round(Convert.ToDecimal(control3.Value) - (Convert.ToDecimal(control4.Value) + Convert.ToDecimal(str1)) + d).ToString("0.00");
        control2.Value = control9.Text.Trim();
        control8.Text = Math.Round(Convert.ToDecimal(control3.Value) - Convert.ToDecimal(str1)).ToString("0.00");
        if (btnPay.Visible)
        {
            control6.Visible = false;
            control9.Visible = true;
        }
        else
        {
            control6.Visible = true;
            control9.Visible = false;
        }
    }

    protected void bnRevert_Click(object sender, EventArgs e)
    {
        if (ObjDAL.ExecuteScalar("HR_ChkSalGenNextMonth", new Hashtable()
    {
      {
         "SalYr",
         drpYear.SelectedValue
      },
      {
         "SalMonth",
         drpMonth.SelectedValue
      }
    }).Trim().ToUpper() != "S")
        {
            lblMsg.Text = "Next Month Salary Has Generated.You Cannot Revert Salary";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            ObjDAL = new clsDAL();
            string str = ObjDAL.ExecuteScalar("HR_DelSalary", new Hashtable()
      {
        {
           "@SalMonth",
           drpMonth.SelectedValue
        },
        {
           "@SalYr",
           drpYear.SelectedValue
        }
      });
            if (str.Trim().ToUpper() == "S")
            {
                lblMsg.Text = "Salary Deleted Successfully for " + drpMonth.SelectedValue + '-' + drpYear.SelectedValue;
                lblMsg.ForeColor = Color.Green;
                grdEmpPament.DataSource = null;
                grdEmpPament.DataBind();
                lblNoOfRec.Text = string.Empty;
                lblTotal.Text = "0.00";
                lblMessage.Text = string.Empty;
            }
            else
            {
                lblMsg.Text = str;
                lblMsg.ForeColor = Color.Red;
            }
        }
    }

    private void clearAll()
    {
        txtInstrNo.Text = "";
        txtPayBill.Text = "";
        txtPaymentDate.Text = "";
        rdbtnPayMode.SelectedValue = "1";
        rdbtnPayMode_SelectedIndexChanged(rdbtnPayMode, EventArgs.Empty);
        drpBank.SelectedIndex = 0;
    }

    protected void drpEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
}