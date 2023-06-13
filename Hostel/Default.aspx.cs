using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Hostel_Default : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private string Rcptno = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.btnsave.Enabled = true;
        this.lblMsg.Text = "";
        this.lblMsg2.Text = "";
        if (this.Session["User_Id"] == null)
            this.Response.Redirect("../Login.aspx");
        if (this.Page.IsPostBack)
            return;
        this.ViewState["Bal"] = 0;
        this.btnRcvOldDue.Visible = false;
        this.ViewState["FY"] = "n";
        this.ViewState["Misc"] = "n";
        this.ViewState["PrevDue"] = "0";
        this.ViewState["OldDue"] = "0";
        this.Page.Form.DefaultButton = this.btnsave.UniqueID;
        this.lblbalance.Text = "0.00";
        this.lblMiscFee.Text = "0.00";
        this.fillsession();
        this.fillclass();
        this.fillAditionalFee();
        this.dtpFeeDt.SetDateValue(DateTime.Today);
        this.dtpMiscDt.SetDateValue(DateTime.Today);
        if (this.Request.QueryString["admnno"] != null)
        {
            this.txtadmnno.Text = this.Request.QueryString["admnno"].ToString();
            this.FillSelStudent();
            this.FillFeeMonth(this.drpMonthPayble);
            this.FillMiscFeeNames(this.drpMiscFee);
            this.FillFeeAmt();
            this.txtadmnno.Text = this.drpstudent.SelectedValue;
            this.getbalance(this.drpstudent.SelectedValue.ToString());
            this.getMiscFee(this.drpstudent.SelectedValue.ToString());
        }
        this.txtadmnno.Focus();
        this.txtBank.Enabled = false;
        this.txtChqDate.Enabled = false;
        this.txtChqNo.Enabled = false;
        this.bindDropDown(this.drpBankAc, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=7 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
        this.drpBankAc.Visible = false;
        this.setMiscDate();
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        this.obj = new clsDAL();
        DataTable dataTableQry = this.obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- Select Bank Account -", "0"));
    }

    private void fillsession()
    {
        this.obj = new clsDAL();
        this.drpSession.DataSource = this.obj.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        this.drpSession.DataTextField = "SessionYr";
        this.drpSession.DataValueField = "SessionYr";
        this.drpSession.DataBind();
    }

    private void setMiscDate()
    {
        string str = this.drpSession.SelectedValue.Trim();
        DateTime dateTime1 = Convert.ToDateTime("1 Apr" + str.Substring(0, 4));
        DateTime dateTime2 = Convert.ToDateTime("31 Mar" + Convert.ToString(Convert.ToInt32(str.Substring(0, 4)) + 1));
        if (DateTime.Today < dateTime2 && DateTime.Today < dateTime1)
        {
            if (this.rBtnPrevSess.Checked)
                this.dtpMiscDt.SetDateValue(dateTime1.AddDays(-1.0));
            else
                this.dtpMiscDt.SetDateValue(dateTime1);
            this.txtMiscDt.ReadOnly = true;
            this.dtpMiscDt.Enabled = false;
        }
        else if (DateTime.Today > dateTime2 && DateTime.Today > dateTime1)
        {
            this.dtpMiscDt.SetDateValue(dateTime2);
            this.txtMiscDt.ReadOnly = true;
            this.dtpMiscDt.Enabled = false;
        }
        else
        {
            if (this.rBtnPrevSess.Checked)
            {
                this.dtpMiscDt.SetDateValue(dateTime1.AddDays(-1.0));
                this.dtpMiscDt.Enabled = false;
            }
            else
            {
                this.dtpMiscDt.SetDateValue(DateTime.Today);
                this.dtpMiscDt.Enabled = true;
            }
            this.txtMiscDt.ReadOnly = true;
        }
    }

    private void fillclass()
    {
        this.obj = new clsDAL();
        DataTable dataTableQry = this.obj.GetDataTableQry("select * from ps_classmaster");
        this.drpclass.DataSource = dataTableQry;
        this.drpclass.DataTextField = "classname";
        this.drpclass.DataValueField = "classid";
        this.drpclass.DataBind();
        if (dataTableQry.Rows.Count > 0)
            this.drpclass.Items.Insert(0, new ListItem("- Select -", "0"));
        else
            this.drpclass.Items.Insert(0, new ListItem("No Record", "0"));
    }

    private void GetStudImg()
    {
        if (this.drpstudent.SelectedIndex <= 0)
            return;
        this.obj = new clsDAL();
        string str = this.obj.ExecuteScalarQry("Select StudentPhoto from dbo.PS_StudMaster where Admnno=" + this.drpstudent.SelectedValue.ToString());
        if (str.Trim() != "")
            this.imgStud.ImageUrl = "../Up_Files/Studimage/" + str.Trim();
        else
            this.imgStud.ImageUrl = "../Up_Files/Studimage/noimage.jpg";
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.fillstudent();
        this.drpstudent_SelectedIndexChanged(this.drpstudent, EventArgs.Empty);
        this.drpclass.Focus();
    }

    private void fillstudent()
    {
        this.obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select s.fullname,s.admnno from dbo.Host_Admission ad");
        stringBuilder.Append(" inner join dbo.PS_ClasswiseStudent cs on ad.admnno=cs.admnno");
        stringBuilder.Append(" inner join ps_studmaster s on s.admnno=ad.admnno");
        stringBuilder.Append(" where 1=1 and s.Status=1 and ad.LeavingDt is null");
        if (this.drpclass.SelectedIndex != 0)
            stringBuilder.Append(" and cs.classid=" + this.drpclass.SelectedValue);
        stringBuilder.Append(" and cs.SessionYear='" + this.drpSession.SelectedValue.ToString().Trim() + "' and Detained_Promoted='' order by fullname asc");
        DataTable dataTableQry = this.obj.GetDataTableQry(stringBuilder.ToString().Trim());
        try
        {
            this.drpstudent.DataSource = dataTableQry;
            this.drpstudent.DataTextField = "fullname";
            this.drpstudent.DataValueField = "admnno";
            this.drpstudent.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        if (dataTableQry.Rows.Count > 0)
        {
            this.drpstudent.Items.Insert(0, new ListItem("- Select -", "0"));
        }
        else
        {
            this.drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
            this.lblbalance.Text = "0";
            this.txtadmnno.Text = string.Empty;
        }
    }

    protected void fillAditionalFee()
    {
        this.drpAdditionalFee.DataSource = new clsDAL().GetDataTableQry("SELECT FC.FeeID,FC.FeeName,FC.PeriodicityID FROM dbo.Host_FeeComponents FC WHERE PeriodicityID=6 ORDER BY FeeName DESC");
        this.drpAdditionalFee.DataTextField = "FeeName";
        this.drpAdditionalFee.DataValueField = "FeeID";
        this.drpAdditionalFee.DataBind();
        this.drpAdditionalFee.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        this.TotalAmount();
        clsDAL clsDal = new clsDAL();
        if (!this.getValidInput())
            return;
        this.lblMsg.Text = string.Empty;
        this.lblMsg2.Text = string.Empty;
        string str1 = "select count(*) from dbo.Acts_PaymentReceiptVoucher where TransDt > '" + this.dtpFeeDt.GetDateValue().ToString("dd MMM yyyy") + "' and (IsDeleted is null or IsDeleted=0)";
        if (Convert.ToInt32(clsDal.ExecuteScalarQry(str1)) > 0)
        {
            Label lblMsg = this.lblMsg;
            Label lblMsg2 = this.lblMsg2;
            string str2 = "Receipt already generated for Later Date. Receipt can not be generated now for ";
            string str3 = this.dtpFeeDt.GetDateValue().ToString("dd MMM yyyy");
            string str4;
            string str5 = str4 = str2 + str3;
            lblMsg2.Text = str4;
            string str6 = str5;
            lblMsg.Text = str6;
            this.lblMsg.ForeColor = this.lblMsg2.ForeColor = Color.Red;
        }
        else
            this.SaveData();
    }

    private bool getValidInput()
    {
        double num1 = Convert.ToDouble(this.ViewState["PrevDue"]);
        double num2 = Convert.ToDouble(this.ViewState["OldDue"]);
        if ((num1 <= 0.0 || !this.rBtnCurrentSess.Checked.Equals(true)) && num2 <= 0.0)
            return true;
        this.lblMsg.Text = num2 <= 0.0 ? (this.lblMsg2.Text = "Please Clear Previous Due Rs." + this.ViewState["PrevDue"] + " first.") : (this.lblMsg2.Text = "Please Clear Old Due Rs." + this.ViewState["OldDue"] + "  first.");
        this.lblMsg.ForeColor = this.lblMsg2.ForeColor = Color.Red;
        return false;
    }

    private void SaveData()
    {
        this.ViewState["FeeTable"] = string.Empty;
        this.ViewState["DrAcHead"] = string.Empty;
        this.ViewState["Status"] = string.Empty;
        if (this.drpPmtMode.SelectedValue.ToString().Trim() == "Cash")
            this.ViewState["DrAcHead"] = "3";
        else
            this.ViewState["DrAcHead"] = this.drpBankAc.SelectedValue.ToString();
        if (this.Request.QueryString["rno"] != null)
        {
            if (double.Parse(this.txtamt.Text.Trim()) > 0.0)
                this.save();
            if (double.Parse(this.txtMiscFee.Text.Trim()) > 0.0)
                this.MiscFee();
            if (this.ViewState["RcptNo"] != null)
                this.obj.ExecuteScalar("Host_Insert_GenLedgerNew", new Hashtable()
        {
          {
             "@Receipt_VrNo",
             this.ViewState["RcptNo"].ToString().Trim()
          },
          {
             "@Amount",
             (!(this.txtamt.Text.Trim() != "") && !(this.txtamt.Text.Trim() != "0") || !(this.txtMiscFee.Text.Trim() != "") && !(this.txtMiscFee.Text.Trim() != "0") ? (!(this.txtamt.Text.Trim() != "") && !(this.txtamt.Text.Trim() != "0") || !(this.txtMiscFee.Text.Trim() == "") && !(this.txtMiscFee.Text.Trim() == "0") ? Convert.ToDecimal(this.txtMiscFee.Text.Trim()) : Convert.ToDecimal(this.txtamt.Text.Trim())) : Convert.ToDecimal(this.txtamt.Text.Trim()) + Convert.ToDecimal(this.txtMiscFee.Text.Trim()))
          },
          {
             "@TransDesc",
             this.txtdesc.Text.Trim()
          },
          {
             "@SessionYr",
             this.drpSession.SelectedValue
          },
          {
             "@UserId",
             this.Session["User_Id"].ToString()
          },
          {
             "@SchoolID",
             this.Session["SchoolId"].ToString()
          },
          {
             "@TransDate",
             this.dtpFeeDt.GetDateValue().ToString("dd MMM yyyy")
          },
          {
             "@AccountHeadDr",
             Convert.ToInt32(this.ViewState["DrAcHead"].ToString())
          }
        });
            this.clear();
        }
        else
        {
            string str = string.Empty;
            if (double.Parse(this.txtamt.Text.Trim()) > 0.0)
                this.save();
            if (this.ViewState["Status"].ToString().Trim() != "DUP" && this.ViewState["Status"].ToString().Trim() != "NO")
            {
                if (double.Parse(this.txtMiscFee.Text.Trim()) > 0.0)
                    this.MiscFee();
                if (this.ViewState["Status"].ToString().Trim() != "DUP" && this.ViewState["Status"].ToString().Trim() != "NO")
                {
                    if (this.ViewState["RcptNo"] != null)
                        str = this.obj.ExecuteScalar("Host_Insert_GenLedgerNew", new Hashtable()
            {
              {
                 "@Receipt_VrNo",
                 this.ViewState["RcptNo"].ToString().Trim()
              },
              {
                 "@Amount",
                 (!(this.txtamt.Text.Trim() != "") && !(this.txtamt.Text.Trim() != "0.00") || !(this.txtMiscFee.Text.Trim() != "") && !(this.txtMiscFee.Text.Trim() != "0.00") ? (!(this.txtamt.Text.Trim() != "") && !(this.txtamt.Text.Trim() != "0.00") || !(this.txtMiscFee.Text.Trim() == "") && !(this.txtMiscFee.Text.Trim() == "0.00") ? Convert.ToDecimal(this.txtMiscFee.Text.Trim()) : Convert.ToDecimal(this.txtamt.Text.Trim())) : Convert.ToDecimal(this.txtamt.Text.Trim()) + Convert.ToDecimal(this.txtMiscFee.Text.Trim()))
              },
              {
                 "@TransDesc",
                 this.txtdesc.Text.Trim()
              },
              {
                 "@SessionYr",
                 this.drpSession.SelectedValue
              },
              {
                 "@UserId",
                 this.Session["User_Id"].ToString()
              },
              {
                 "@SchoolID",
                 this.Session["SchoolId"].ToString()
              },
              {
                 "@TransDate",
                 this.dtpFeeDt.GetDateValue().ToString("dd MMM yyyy")
              },
              {
                 "@AccountHeadDr",
                 Convert.ToInt32(this.ViewState["DrAcHead"].ToString())
              }
            });
                    if (str.Trim() != string.Empty)
                    {
                        this.ViewState["Status"] = "NO";
                        this.statusOnFail();
                    }
                }
                else
                    this.statusOnFail();
            }
            else
                this.statusOnFail();
        }
        DataTable dataTable = (DataTable)this.ViewState["FeeTable"];
        if (dataTable.Rows.Count <= 0 || !(dataTable.Rows[0][0].ToString().Trim() != "DUP") || !(dataTable.Rows[0][0].ToString().Trim() != "NO"))
            return;
        if (this.Request.QueryString["Sw"] != null)
            this.Response.Redirect("../Hostel/rptHostFeeReceipt.aspx?admnno=" + this.txtadmnno.Text.Trim() + " &sess=" + this.drpSession.SelectedValue.ToString().Trim() + " &status=1 &rno=" + this.Rcptno + " &rcptdt=" + this.dtpFeeDt.GetDateValue().ToString("dd-MM-yyyy") + " &class=" + this.drpclass.SelectedValue.ToString() + " &D=p &ChkNo=" + this.txtChqNo.Text.Trim() + " &ChkDt=" + this.txtChqDate.Text.ToString().Trim() + " &Bn=" + this.txtBank.Text.ToString().Trim() + " &Pmnt=" + this.drpPmtMode.SelectedValue.ToString().Trim() + "&Sw=" + this.Request.QueryString["Sw"]);
        else
            this.Response.Redirect("../Hostel/rptHostFeeReceipt.aspx?admnno=" + this.txtadmnno.Text.Trim() + " &sess=" + this.drpSession.SelectedValue.ToString().Trim() + " &status=1 &rno=" + this.Rcptno + " &rcptdt=" + this.dtpFeeDt.GetDateValue().ToString("dd-MM-yyyy") + " &class=" + this.drpclass.SelectedValue.ToString() + " &D=p &ChkNo=" + this.txtChqNo.Text.Trim() + " &ChkDt=" + this.txtChqDate.Text.ToString().Trim() + " &Bn=" + this.txtBank.Text.ToString().Trim() + " &Pmnt=" + this.drpPmtMode.SelectedValue.ToString().Trim());
    }

    private void save()
    {
        Decimal num1 = new Decimal(0);
        Decimal CrAmt = new Decimal(0);
        Decimal num2;
        try
        {
            num2 = Convert.ToDecimal(this.txtamt.Text);
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock((Control)this.txtamt, this.txtamt.GetType(), "ShowMessage", "alert('Invalid amount');", true);
            return;
        }
        if (num2 <= new Decimal(0) && CrAmt <= new Decimal(0))
        {
            ScriptManager.RegisterClientScriptBlock((Control)this.txtamt, this.txtamt.GetType(), "ShowMessage", "alert('Enter Amount greater than 0, There is no credit available in credit ledger.');", true);
        }
        else
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("Session", this.drpSession.SelectedValue);
            hashtable.Add("RecvDate", this.dtpFeeDt.GetDateValue().ToString("dd MMM yyyy"));
            hashtable.Add("AdmnNo", this.txtadmnno.Text);
            hashtable.Add("Description", this.txtdesc.Text.Trim());
            hashtable.Add("Amount", double.Parse(this.lblTotalFee.Text.Trim()));
            hashtable.Add("PaymentMode", this.drpPmtMode.SelectedValue.ToString());
            if (this.drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
            {
                hashtable.Add("ChequeNo", this.txtChqNo.Text.Trim());
                hashtable.Add("ChequeDate", this.txtChqDate.Text.ToString());
                hashtable.Add("DrawanOnBank", this.txtBank.Text);
                hashtable.Add("BankAcHeadId", int.Parse(this.drpBankAc.SelectedValue.ToString()));
            }
            hashtable.Add("UserID", Convert.ToInt32(this.Session["User_Id"].ToString().Trim()));
            hashtable.Add("SchoolID", this.Session["SchoolId"].ToString());
            StringBuilder stringBuilder = new StringBuilder();
            if (this.rBtnCurrentSess.Checked)
                hashtable.Add("CurrSessFee", 1);
            else
                hashtable.Add("CurrSessFee", 0);
            this.obj = new clsDAL();
            DataTable dataTable = new DataTable();
            DataTable tb = !(this.drpMonthPayble.SelectedValue.ToString().Trim() != "0") ? this.obj.GetDataTable("Host_Insert_FeeCash_OnAdmn", hashtable) : this.obj.GetDataTable("Host_Insert_FeeCash_OnFeeReceipt", hashtable);
            this.ViewState["FeeTable"] = tb;
            if (tb.Rows.Count > 0 && tb.Rows[0][0].ToString().Trim() != "DUP" && tb.Rows[0][0].ToString().Trim() != "NO")
            {
                this.Rcptno = tb.Rows[0]["ReceiptNo"].ToString();
                this.ViewState["RcptNo"] = this.Rcptno;
                this.Session["rcptFee"] = this.Rcptno;
                this.insertfeeledger(tb, CrAmt);
            }
            else
            {
                if (tb.Rows.Count <= 0 || !(tb.Rows[0][0].ToString().Trim() == "DUP") && !(tb.Rows[0][0].ToString().Trim() == "NO"))
                    return;
                this.ViewState["Status"] = tb.Rows[0][0].ToString().Trim();
            }
        }
    }

    private void MiscFee()
    {
        Decimal AvlCredit = new Decimal(0);
        try
        {
            Convert.ToDecimal(this.txtMiscFee.Text);
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock((Control)this.txtamt, this.txtamt.GetType(), "ShowMessage", "alert('Invalid Amount');", true);
            return;
        }
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (this.ViewState["RcptNo"] != null)
            hashtable.Add("receiptno", this.ViewState["RcptNo"].ToString().Trim());
        hashtable.Add("Session", this.drpSession.SelectedValue);
        hashtable.Add("RecvDate", this.dtpFeeDt.GetDateValue().ToString("dd MMM yyyy"));
        hashtable.Add("AdmnNo", this.txtadmnno.Text);
        hashtable.Add("Description", this.txtdesc.Text.Trim());
        hashtable.Add("Amount", double.Parse(this.lblTotalFee.Text.Trim()));
        hashtable.Add("PaymentMode", this.drpPmtMode.SelectedValue.ToString());
        if (this.drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
        {
            hashtable.Add("ChequeNo", this.txtChqNo.Text.Trim());
            hashtable.Add("ChequeDate", this.txtChqDate.Text.ToString());
            hashtable.Add("DrawanOnBank", this.txtBank.Text);
            hashtable.Add("BankAcHeadId", int.Parse(this.drpBankAc.SelectedValue.ToString()));
        }
        hashtable.Add("UserID", Convert.ToInt32(this.Session["User_Id"].ToString().Trim()));
        hashtable.Add("SchoolID", this.Session["SchoolId"].ToString());
        hashtable.Add("@FeeId", this.drpMiscFee.SelectedValue.Trim().ToString());
        if (this.rBtnCurrentSess.Checked)
            hashtable.Add("@CurrSessFee", 1);
        else
            hashtable.Add("@CurrSessFee", 0);
        this.obj = new clsDAL();
        DataTable dataTable2 = this.obj.GetDataTable("Host_Insert_FeeCash_OnMiscReceipt", hashtable);
        this.ViewState["FeeTable"] = dataTable2;
        if (dataTable2.Rows.Count > 0 && dataTable2.Rows[0][0].ToString().Trim() != "DUP" && dataTable2.Rows[0][0].ToString().Trim() != "NO")
        {
            this.Rcptno = dataTable2.Rows[0]["ReceiptNo"].ToString();
            this.ViewState["RcptNo"] = this.Rcptno;
            this.Session["rcptFee"] = this.Rcptno;
            this.InsertMiscfeeLedger(dataTable2, AvlCredit);
        }
        else
        {
            if (dataTable2.Rows.Count <= 0 || !(dataTable2.Rows[0][0].ToString().Trim() == "DUP") && !(dataTable2.Rows[0][0].ToString().Trim() == "NO"))
                return;
            this.ViewState["Status"] = dataTable2.Rows[0][0].ToString().Trim();
        }
    }

    private void InsertMiscfeeLedger(DataTable tb, Decimal AvlCredit)
    {
        Decimal result1 = new Decimal(0);
        Decimal.TryParse(this.txtMiscFee.Text, out result1);
        Decimal num1 = result1 + AvlCredit;
        foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
        {
            Decimal result2 = new Decimal(0);
            Decimal num2 = new Decimal(0);
            Decimal.TryParse(Convert.ToString(row["Balance"]), out result2);
            Decimal CrAmt;
            if (num1 > result2)
            {
                row["balance"] = 0;
                num1 -= result2;
                CrAmt = result2;
            }
            else
            {
                row["balance"] = (result2 - num1);
                CrAmt = num1;
                num1 = new Decimal(0);
            }
            tb.AcceptChanges();
            this.InsertCrAmtInLedger(row, CrAmt);
            if (num1 <= new Decimal(0))
                break;
        }
        this.updatefeeledger(tb, 0);
    }

    private void InsertCrAmtInLedger(DataRow dr, Decimal CrAmt)
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("date", Convert.ToDateTime(dr["TransDate"]));
        string str1 = this.dtpFeeDt.GetDateValue().ToString("dd MMM yyyy");
        hashtable.Add("TransDate", str1);
        hashtable.Add("AdmnNo", Convert.ToInt64(dr["AdmnNo"]));
        hashtable.Add("TransDesc", Convert.ToString(dr["TransDesc"]));
        hashtable.Add("debit", 0);
        hashtable.Add("credit", CrAmt);
        hashtable.Add("balance", 0);
        hashtable.Add("Receipt_VrNo", this.Rcptno);
        hashtable.Add("userid", Convert.ToInt32(this.Session["User_Id"].ToString()));
        string str2 = DateTime.Now.ToString("dd MMM yyyy");
        hashtable.Add("UserDate", Convert.ToDateTime(str2, (IFormatProvider)CultureInfo.InvariantCulture));
        hashtable.Add("schoolid", this.Session["SchoolId"].ToString());
        hashtable.Add("FeeId", Convert.ToString(dr["FeeId"]));
        hashtable.Add("PayMode", this.drpPmtMode.SelectedValue.ToString().Trim());
        hashtable.Add("Session", dr["SessionYr"].ToString());
        hashtable.Add("AccountHeadDr", Convert.ToInt32(this.ViewState["DrAcHead"].ToString()));
        hashtable.Add("PR_Id", Convert.ToInt64(dr["PR_Id"]));
        this.obj = new clsDAL();
        this.obj.GetDataTable("Host_Insert_FeeLedgerNew", hashtable);
    }

    private void insertfeeledger(DataTable tb, Decimal CrAmt)
    {
        Decimal result1 = new Decimal(0);
        Decimal.TryParse(this.txtamt.Text, out result1);
        Decimal num1 = result1 + CrAmt;
        foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
        {
            Decimal result2 = new Decimal(0);
            Decimal num2 = new Decimal(0);
            Decimal.TryParse(Convert.ToString(row["Balance"]), out result2);
            Decimal CrAmt1;
            if (num1 > result2)
            {
                row["balance"] = 0;
                num1 -= result2;
                CrAmt1 = result2;
            }
            else
            {
                row["balance"] = (result2 - num1);
                CrAmt1 = num1;
                num1 = new Decimal(0);
            }
            tb.AcceptChanges();
            this.InsertCrAmtInLedger(row, CrAmt1);
            if (num1 <= new Decimal(0))
                break;
        }
        this.updatefeeledger(tb, 0);
    }

    private void updatefeeledger(DataTable tbcredit, int mode)
    {
        foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
        {
            if (row["debit"].ToString() != row["balance"].ToString())
            {
                this.obj = new clsDAL();
                this.obj.GetDataTable("Host_Update_FeeLedger", new Hashtable()
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
             this.Session["User_Id"].ToString()
          },
          {
             "UserDate",
             DateTime.Now.ToString("dd MMM yyyy")
          },
          {
             "SchoolID",
             this.Session["SchoolId"].ToString()
          },
          {
             "mode",
             this.drpPmtMode.SelectedValue.ToString().Trim()
          }
        });
            }
        }
    }

    private void UpdateBalAmtInLedger(DataRow dr, Decimal bal)
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("transno", Convert.ToInt64(dr["TransNo"]));
        hashtable.Add("AdmnNo", Convert.ToInt64(dr["AdmnNo"]));
        hashtable.Add("VrNo", dr["receiptno"].ToString());
        hashtable.Add("FeeId", Convert.ToInt64(dr["FeeId"]));
        hashtable.Add("Balance", bal);
        hashtable.Add("userid", Convert.ToInt32(this.Session["User_Id"].ToString()));
        this.obj = new clsDAL();
        this.obj.GetDataTable("Host_Update_FeeLedgerBank", hashtable);
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("feercptcashdetail.aspx");
    }

    private void clear()
    {
        this.txtamt.Text = "";
        this.dtpFeeDt.SetDateValue(DateTime.Today);
        this.txtdesc.Text = "";
        this.ViewState["OldDue"] = "0";
        this.ViewState["PrevDue"] = "0";
    }

    private void DisableControls()
    {
        this.drpSession.Enabled = false;
        this.drpclass.Enabled = false;
        this.drpstudent.Enabled = false;
        this.txtadmnno.ReadOnly = true;
    }

    private bool IsBalance()
    {
        return double.Parse(new clsDAL().ExecuteScalarQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(balance),0)) as balance FROM Host_FeeLedger WHERE admnno='" + this.drpstudent.SelectedValue.ToString().Trim() + "' and Transdate <=getdate() ")) > 0.0;
    }

    protected void drpPmtMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpPmtMode.SelectedValue == "Cash")
        {
            this.txtBank.Enabled = false;
            this.txtChqDate.Enabled = false;
            this.txtChqNo.Enabled = false;
            this.txtChqDate.Text = string.Empty;
            this.txtChqNo.Text = string.Empty;
            this.txtBank.Text = string.Empty;
            this.drpBankAc.Visible = false;
        }
        else
        {
            this.txtBank.Enabled = true;
            this.txtChqDate.Enabled = true;
            this.txtChqNo.Enabled = true;
            this.drpBankAc.Visible = true;
        }
        this.drpPmtMode.Focus();
    }

    private void FillSelStudent()
    {
        this.drpSession.SelectedValue = this.Request.QueryString["sess"].ToString();
        if (this.Request.QueryString["cid"].ToString() != "0")
        {
            this.drpclass.SelectedValue = this.Request.QueryString["cid"].ToString();
        }
        else
        {
            this.obj = new clsDAL();
            this.drpclass.SelectedValue = this.obj.GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent where AdmnNo=" + this.Request.QueryString["admnno"].ToString() + " and Detained_Promoted=''").Rows[0]["ClassID"].ToString();
        }
        this.fillstudent();
        this.drpstudent.SelectedValue = this.Request.QueryString["admnno"].ToString().Trim();
        this.GetStudImg();
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.drpstudent.Items.Clear();
        this.drpclass.SelectedIndex = 0;
        this.txtadmnno.Text = string.Empty;
        this.lblTotalFee.Text = "0.00";
        this.lblbalance.Text = "0.00";
        this.txtamt.Text = string.Empty;
        this.txtRcevMiscFee.Text = string.Empty;
        this.txtdesc.Text = string.Empty;
        this.setMiscDate();
        this.drpSession.Focus();
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.lblMsg.Text = string.Empty;
        this.lblMsg2.Text = string.Empty;
        this.ViewState["FY"] = "n";
        this.txtadmnno.Text = this.drpstudent.SelectedValue;
        this.GetAllFeeDetails();
        this.drpstudent.Focus();
    }

    protected void GetAllFeeDetails()
    {
        this.FillFeeMonth(this.drpMonthPayble);
        this.FillMiscFeeNames(this.drpMiscFee);
        this.FillPaybleMonth();
        this.FillFeeAmt();
        this.GetStudImg();
    }

    protected void btnDetail_Click(object sender, EventArgs e)
    {
        this.lblMsg.Text = string.Empty;
        this.lblMsg2.Text = string.Empty;
        this.ViewState["FY"] = "n";
        try
        {
            this.obj = new clsDAL();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select SessionYear,ClassID from dbo.PS_ClasswiseStudent cs");
            stringBuilder.Append(" inner join dbo.Host_Admission ad on ad.AdmnNo=cs.AdmnNo");
            stringBuilder.Append(" inner join dbo.PS_StudMaster sm on sm.AdmnNo=cs.AdmnNo");
            stringBuilder.Append(" where cs.AdmnNo=" + this.txtadmnno.Text.Trim() + " and cs.Detained_Promoted='' and sm.Status=1 and ad.LeavingDt is null");
            DataTable dataTableQry = this.obj.GetDataTableQry(stringBuilder.ToString().Trim());
            if (dataTableQry.Rows.Count > 0)
            {
                this.drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
                this.drpclass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
                this.fillstudent();
                this.drpstudent.SelectedValue = this.txtadmnno.Text.Trim();
                this.ViewState["FY"] = "n";
                this.GetAllFeeDetails();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock((Control)this.txtadmnno, this.txtadmnno.GetType(), "ShowMessage", "alert('Admission number does not exists')", true);
                return;
            }
        }
        catch (Exception ex)
        {
            string empty = string.Empty;
            string message = ex.Message;
        }
        this.setMiscDate();
        this.btnDetail.Focus();
    }

    protected void FillFeeAmt()
    {
        this.CalculateTotPayble();
    }

    private void CalculateTotPayble()
    {
        this.ViewState["OldDue"] = "0";
        this.ViewState["PrevDue"] = "0";
        this.getbalance(this.txtadmnno.Text.Trim());
        this.getMiscFee(this.txtadmnno.Text.Trim());
        this.txtamt.Text = Convert.ToDouble(this.lblbalance.Text).ToString();
        this.txtMiscFee.Text = Convert.ToDouble(this.lblMiscFee.Text).ToString();
        this.TotalAmount();
        this.CalculatePrevDue(this.txtadmnno.Text.Trim());
    }

    private void CalculatePrevDue(string AdmnNo)
    {
        this.obj = new clsDAL();
        StringBuilder stringBuilder1 = new StringBuilder();
        string str1 = "04/01/" + this.drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        stringBuilder1.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
        stringBuilder1.Append(" from Host_FeeLedger where admnno='" + AdmnNo + "'  and TransDate < '" + str1 + "' AND TransDesc<>'Previous Balance'");
        string str2 = this.obj.ExecuteScalarQry(stringBuilder1.ToString().Trim());
        this.lblPrevYrBal.Text = "Previous Session Due :- Rs. " + string.Format("{0:f2}", Convert.ToDouble(str2));
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
        stringBuilder2.Append(" from Host_FeeLedger where admnno='" + AdmnNo + "'  and TransDate < '" + str1 + "' AND TransDesc='Previous Balance'");
        string str3 = this.obj.ExecuteScalarQry(stringBuilder2.ToString().Trim());
        this.lblOldDues.Text = "Old Dues (Before Computerisation):- Rs. " + string.Format("{0:f2}", Convert.ToDouble(str3));
        if (Convert.ToDouble(str3) > 0.0)
        {
            this.btnRcvOldDue.Visible = true;
            this.ViewState["OldDue"] = str3;
        }
        else
        {
            this.ViewState["OldDue"] = "0";
            this.btnRcvOldDue.Visible = false;
        }
        this.ViewState["PrevDue"] = Convert.ToDouble(str2);
        this.drpMonthPayble_SelectedIndexChanged(this.drpMonthPayble, EventArgs.Empty);
    }

    private void getMiscFee(string AdmnNo)
    {
        this.obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        if (this.rBtnCurrentSess.Checked)
        {
            this.ViewState["Misc"] = "n";
            string currSession = new clsGenerateFee().CreateCurrSession();
            this.lblMiscFee.Text = this.obj.GetDataTable("Host_GetStudMiscBalAmnt", new Hashtable()
      {
        {
           "@AdmnNo",
           AdmnNo
        },
        {
           "@SessionYr",
           this.drpSession.SelectedValue.ToString().Trim()
        },
        {
           "@PrevSession",
           currSession
        }
      }).Rows[0]["Balance"].ToString();
        }
        else
        {
            string str = "04/01/" + this.drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
            stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
            stringBuilder.Append(" from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where PeriodicityID=6 AND admnno='" + AdmnNo + "'  and TransDate < '" + str + "' and TransDesc<>'Previous Balance'");
            this.lblMiscFee.Text = this.obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
        }
    }

    private void getbalance(string AdmnNo)
    {
        this.obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        if (this.rBtnCurrentSess.Checked)
        {
            this.ViewState["FY"] = "n";
            this.lblbalance.Text = this.obj.GetDataTable("Host_GetStudCurrentBalAmnt", new Hashtable()
      {
        {
           "@AdmnNo",
           this.drpstudent.SelectedValue.ToString().Trim()
        },
        {
           "@SessionYr",
           this.drpSession.SelectedValue.ToString().Trim()
        },
        {
           "@PrevSession",
           new clsGenerateFee().CreateCurrSession()
        }
      }).Rows[0]["Balance"].ToString();
        }
        else
        {
            string str = "04/01/" + this.drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
            stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
            stringBuilder.Append(" from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where PeriodicityID<>6 AND admnno='" + AdmnNo + "'  and TransDate < '" + str + "' and TransDesc<>'Previous Balance'");
            this.lblbalance.Text = this.obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
        }
    }

    private void GetBalFullSession(string AdmnNo)
    {
        this.obj = new clsDAL();
        this.lblbalance.Text = !this.rBtnCurrentSess.Checked.Equals(true) ? this.obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + this.PrevYr(this.drpSession.SelectedValue.ToString().Trim()) + "' AND PeriodicityID<>6 and admnno=" + AdmnNo) : this.obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + this.drpSession.SelectedValue.ToString().Trim() + "' AND PeriodicityID<>6 and admnno=" + AdmnNo);
        string text = this.lblbalance.Text;
        this.drpMonthPayble.SelectedValue = "3";
    }

    protected void btnFeeFullYr_Click(object sender, EventArgs e)
    {
        this.FillFeeMonth(this.drpMonthPayble);
        this.FillPaybleMonth();
        this.obj = new clsDAL();
        string str = this.obj.ExecuteScalarQry("select ad.admnno from dbo.PS_ClasswiseStudent c inner join dbo.Host_Admission ad on c.admnno=ad.AdmnNo where c.admnno=" + this.txtadmnno.Text.ToString().Trim());
        if (str != "")
        {
            if (this.drpstudent.SelectedValue.ToString().Trim() != this.txtadmnno.Text.ToString().Trim())
            {
                this.fillstudent();
                this.drpstudent.SelectedValue = str;
            }
            this.GetBalFullSession(this.txtadmnno.Text.ToString().Trim());
            if (this.rBtnCurrentSess.Checked.Equals(true))
                this.ViewState["FY"] = "y";
            else
                this.ViewState["FY"] = "n";
            this.txtamt.Text = Convert.ToDouble(this.lblbalance.Text.Trim()).ToString();
            this.TotalAmount();
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMassage", "alert('Invalid Admission Number');", true);
        this.btnFeeFullYr.Focus();
    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        if (this.drpstudent.SelectedIndex > 0)
        {
            if (this.lblbalance.Text != "0.00")
            {
                if (this.Request.QueryString["Sw"] != null)
                {
                    if (this.rBtnCurrentSess.Checked)
                    {
                        if (this.drpMonthPayble.SelectedIndex > 1)
                            ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + this.txtadmnno.Text.Trim() + " &sess=" + this.drpSession.SelectedValue.ToString().Trim() + "&class=" + this.drpclass.SelectedValue.ToString() + " &FY=" + this.ViewState["FY"].ToString().Trim() + "&Sw=" + this.Request.QueryString["Sw"] + "&Due=C',null,'width=700,height=500');", true);
                        else
                            ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + this.txtadmnno.Text.Trim() + " &sess=" + this.drpSession.SelectedValue.ToString().Trim() + "&class=" + this.drpclass.SelectedValue.ToString() + " &FY=" + this.ViewState["FY"].ToString().Trim() + "&Sw=" + this.Request.QueryString["Sw"] + "&Due=C&AdmnFee=T',null,'width=700,height=500');", true);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + this.txtadmnno.Text.Trim() + " &sess=" + this.drpSession.SelectedValue.ToString().Trim() + "&class=" + this.drpclass.SelectedValue.ToString() + " &FY=" + this.ViewState["FY"].ToString().Trim() + "&Sw=" + this.Request.QueryString["Sw"] + "&Due=P',null,'width=700,height=500');", true);
                }
                else if (this.rBtnCurrentSess.Checked)
                {
                    if (this.drpMonthPayble.SelectedIndex > 1)
                        ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + this.txtadmnno.Text.Trim() + " &sess=" + this.drpSession.SelectedValue.ToString().Trim() + "&class=" + this.drpclass.SelectedValue.ToString() + " &FY=" + this.ViewState["FY"].ToString().Trim() + "&Due=C',null,'width=700,height=500');", true);
                    else
                        ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + this.txtadmnno.Text.Trim() + " &sess=" + this.drpSession.SelectedValue.ToString().Trim() + "&class=" + this.drpclass.SelectedValue.ToString() + " &FY=" + this.ViewState["FY"].ToString().Trim() + "&Due=C&AdmnFee=T',null,'width=700,height=500');", true);
                }
                else if (this.drpMonthPayble.SelectedIndex > 1)
                    ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + this.txtadmnno.Text.Trim() + " &sess=" + this.drpSession.SelectedValue.ToString().Trim() + "&class=" + this.drpclass.SelectedValue.ToString() + " &FY=" + this.ViewState["FY"].ToString().Trim() + "&Due=P',null,'width=700,height=500');", true);
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + this.txtadmnno.Text.Trim() + " &sess=" + this.drpSession.SelectedValue.ToString().Trim() + "&class=" + this.drpclass.SelectedValue.ToString() + " &FY=" + this.ViewState["FY"].ToString().Trim() + "&Due=P&AdmnFee=T',null,'width=700,height=500');", true);
                this.btnShowDetails.Focus();
            }
            else
                ScriptManager.RegisterClientScriptBlock((Control)this.btnShowDetails, this.btnShowDetails.GetType(), "ShowMessage", "alert('No dues for the selected Student');", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock((Control)this.btnShowDetails, this.btnShowDetails.GetType(), "ShowMessage", "alert('Please Select a Student');", true);
    }

    protected void btnInstDisc_Click(object sender, EventArgs e)
    {
        if (this.Request.QueryString["Sw"] != null)
            this.Response.Redirect("FeeAdjustment.aspx?admnno=" + this.txtadmnno.Text.Trim() + "&sess=" + this.drpSession.SelectedValue.ToString().Trim() + "&cid=" + this.drpclass.SelectedValue.ToString() + "&Sw=" + this.Request.QueryString["Sw"]);
        else
            this.Response.Redirect("FeeAdjustment.aspx?admnno=" + this.txtadmnno.Text.Trim() + "&sess=" + this.drpSession.SelectedValue.ToString().Trim() + "&cid=" + this.drpclass.SelectedValue.ToString());
        this.btnInstDisc.Focus();
    }

    protected void drpMonthPayble_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.obj = new clsDAL();
        if (this.drpMonthPayble.SelectedIndex > 0)
        {
            if (this.drpMonthPayble.SelectedValue.ToString().Trim() == "0")
            {
                this.GetBalFeeOnAdmn(this.txtadmnno.Text.ToString().Trim());
                this.ViewState["FY"] = "m";
                this.txtamt.Text = Convert.ToDouble(this.lblbalance.Text.Trim()).ToString();
            }
            else
            {
                this.getMonthbalance(this.txtadmnno.Text.ToString().Trim());
                this.ViewState["FY"] = "m";
                this.txtamt.Text = Convert.ToString(Convert.ToDouble(this.ViewState["Bal"]) + 0.0);
            }
            this.TotalAmount();
        }
        else
        {
            this.txtamt.Text = "0.00";
            this.lblbalance.Text = "0.00";
            this.TotalAmount();
        }
        this.drpMonthPayble.Focus();
    }

    private void TotalAmount()
    {
        this.lblTotalFee.Text = string.Format("{0:f2}", (Convert.ToDouble(this.txtamt.Text) + Convert.ToDouble(this.txtMiscFee.Text)));
    }

    protected void btnTillDec_Click(object sender, EventArgs e)
    {
        this.obj = new clsDAL();
        if (this.drpMonthPayble.SelectedIndex > 0)
        {
            if (this.drpMonthPayble.SelectedValue.ToString().Trim() == "0")
            {
                this.GetBalFeeOnAdmn(this.txtadmnno.Text.ToString().Trim());
                this.ViewState["FY"] = "m";
                this.txtamt.Text = Convert.ToDouble(this.lblbalance.Text.Trim()).ToString();
                this.TotalAmount();
            }
            else
            {
                this.getMonthbalance(this.txtadmnno.Text.ToString().Trim());
                this.ViewState["FY"] = "m";
                this.txtamt.Text = Convert.ToString(Convert.ToDouble(this.ViewState["Bal"]) + 0.0);
                this.TotalAmount();
            }
        }
        else
        {
            this.txtamt.Text = "0.00";
            this.lblbalance.Text = "0.00";
            this.TotalAmount();
        }
    }

    private void getMonthbalance(string AdmnNo)
    {
        string str1 = this.drpMonthPayble.SelectedValue.ToString();
        string str2 = (Convert.ToInt32(this.drpMonthPayble.SelectedValue.ToString().Trim()) + 1).ToString();
        string str3 = this.drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        if (this.rBtnCurrentSess.Checked.Equals(false))
            str3 = (Convert.ToInt32(str3) - 1).ToString().Trim();
        string str4 = this.drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + this.drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
        if (this.rBtnCurrentSess.Checked.Equals(false))
            str4 = (Convert.ToInt32(str4) - 1).ToString().Trim();
        string str5 = Convert.ToInt32(str1) <= 3 ? str2 + "/01/" + str4 : (Convert.ToInt32(str1) <= 11 ? str2 + "/01/" + str3 : "01/01/" + str4);
        this.Session["FeeMonth"] = str5;
        this.obj = new clsDAL();
        if (this.rBtnCurrentSess.Checked.Equals(true))
        {
            //"select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + this.drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6";
            this.lblbalance.Text = this.obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + this.drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6");
            this.ViewState["Bal"] = this.lblbalance.Text;
        }
        else
        {
            this.lblbalance.Text = this.obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + this.PrevYr(this.drpSession.SelectedValue.ToString().Trim()) + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6");
            this.ViewState["Bal"] = this.lblbalance.Text;
        }
        string text = this.lblbalance.Text;
    }

    private void GetBalFeeOnAdmn(string AdmnNo)
    {
        this.obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        if (this.rBtnCurrentSess.Checked.Equals(true))
        {
            stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(balance),0)) from Host_FeeLedger where admnno='" + AdmnNo + "' and balance > 0");
            stringBuilder.Append(" and feeid in(select distinct feeid from dbo.Host_FeeComponents where PeriodicityID in (1,2)) and SessionYr='" + this.drpSession.SelectedValue.ToString().Trim() + "'");
        }
        else
        {
            stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(balance),0)) from Host_FeeLedger where admnno='" + AdmnNo + "' and balance > 0");
            stringBuilder.Append(" and feeid in(select distinct feeid from dbo.Host_FeeComponents where PeriodicityID in (1,2)) and sessionYr='" + this.PrevYr(this.drpSession.SelectedValue.ToString().Trim()) + "'");
        }
        this.lblbalance.Text = this.obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
        string text = this.lblbalance.Text;
    }

    protected void btnMiscDtl_Click(object sender, EventArgs e)
    {
        if (this.txtadmnno.Text != "")
        {
            if (this.rBtnCurrentSess.Checked)
                ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMessage", "window.open('../Hostel/rptHostMiscDueDetails.aspx?admino=" + this.txtadmnno.Text.Trim() + "&Fid=" + this.drpMiscFee.SelectedValue.ToString().Trim() + "&sess=" + this.drpSession.SelectedValue.ToString().Trim() + "&Due=C',null,'width=700,height=500');", true);
            else
                ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMessage", "window.open('../Hostel/rptHostMiscDueDetails.aspx?admino=" + this.txtadmnno.Text.Trim() + "&Fid=" + this.drpMiscFee.SelectedValue.ToString().Trim() + "&sess=" + this.PrevYr(this.drpSession.SelectedValue.ToString().Trim()) + "&Due=P',null,'width=700,height=500');", true);
            this.btnMiscDtl.Focus();
        }
        else
            ScriptManager.RegisterClientScriptBlock((Control)this.btnShowDetails, this.btnShowDetails.GetType(), "ShowMessage", "alert('Enter Admission Number');", true);
    }

    protected void btnSFClear_Click(object sender, EventArgs e)
    {
        this.txtamt.Text = "0.00";
        this.TotalAmount();
    }

    protected void btnMFClear_Click(object sender, EventArgs e)
    {
        this.txtMiscFee.Text = "0.00";
        this.TotalAmount();
    }

    protected void btnMiscFee_Click(object sender, EventArgs e)
    {
        string str1 = this.drpSession.SelectedValue.Trim();
        DateTime dateTime1 = Convert.ToDateTime("31 Mar" + str1.Substring(0, 4));
        DateTime dateTime2 = Convert.ToDateTime("1 Apr" + Convert.ToString(Convert.ToInt32(str1.Substring(0, 4)) + 1));
        if (DateTime.Today < dateTime2 && DateTime.Today > dateTime1 && this.dtpMiscDt.GetDateValue().Date > DateTime.Today)
        {
            this.lblMsg.Text = this.lblMsg2.Text = "Misc Fee Date cannot be greater than Current Date.";
            this.lblMsg.ForeColor = Color.Red;
            this.lblMsg2.ForeColor = Color.Red;
        }
        else if (this.dtpMiscDt.GetDateValue().Date >= dateTime2 || this.dtpMiscDt.GetDateValue().Date <= dateTime1 && this.rBtnCurrentSess.Checked)
        {
            this.lblMsg.Text = this.lblMsg2.Text = "Misc Fee Date not in the selected Session Year.";
            this.lblMsg.ForeColor = Color.Red;
            this.lblMsg2.ForeColor = Color.Red;
        }
        else
        {
            clsDAL clsDal = new clsDAL();
            string str2 = clsDal.ExecuteScalar("Host_InsAdditionalFeeChk", new Hashtable()
      {
        {
           "@TransDate",
           this.dtpMiscDt.GetDateValue().ToString("dd MMM yyyy")
        },
        {
           "@AdmnNo",
           this.txtadmnno.Text.ToString().Trim()
        },
        {
           "@AdFeeId",
           this.drpAdditionalFee.SelectedValue.ToString().Trim()
        }
      });
            if (str2.Trim() != "No")
            {
                Hashtable hashtable = new Hashtable();
                DataTable dataTable1 = new DataTable();
                hashtable.Add("@TransDate", this.dtpMiscDt.GetDateValue().ToString("dd MMM yyyy"));
                hashtable.Add("@AdmnNo", this.txtadmnno.Text.ToString().Trim());
                hashtable.Add("@TransDesc", this.drpAdditionalFee.SelectedItem.Text);
                hashtable.Add("@Debit", this.txtRcevMiscFee.Text.Trim());
                hashtable.Add("@Credit", "0.0000");
                hashtable.Add("@Balance", this.txtRcevMiscFee.Text.Trim());
                hashtable.Add("@UserID", this.Session["User_Id"]);
                hashtable.Add("@AdFeeId", this.drpAdditionalFee.SelectedValue.ToString().Trim());
                hashtable.Add("@SchoolId", this.Session["SchoolId"].ToString());
                DataTable dataTable2 = clsDal.GetDataTable("Host_InsAdditionalFee", hashtable);
                if (dataTable2.Rows.Count <= 0 && str2.Trim() == "Yes")
                {
                    this.lblMsg.Text = this.lblMsg2.Text = "Misc Fee Updated Successfully";
                    this.lblMsg.ForeColor = this.lblMsg2.ForeColor = Color.Green;
                }
                else if (dataTable2.Rows.Count <= 0)
                {
                    this.lblMsg.Text = this.lblMsg2.Text = "Misc Fee Added Successfully";
                    this.lblMsg.ForeColor = this.lblMsg2.ForeColor = Color.Green;
                }
                this.drpAdditionalFee.SelectedIndex = 0;
                this.txtRcevMiscFee.Text = "0";
                this.setMiscDate();
            }
            else if (str2.Trim() == "No")
            {
                this.lblMsg.Text = this.lblMsg2.Text = "Cannot Update Misc Fee. Fee already Received on this or later date.";
                this.lblMsg.ForeColor = this.lblMsg2.ForeColor = Color.Red;
                return;
            }
            this.FillMiscFeeNames(this.drpMiscFee);
            this.FillPaybleMonth();
            this.FillFeeAmt();
            this.btnMiscFee.Focus();
        }
    }

    private void FillPaybleMonth()
    {
        this.FillFeeMonth(this.drpMonthPayble);
    }

    private void FillMiscFeeNames(DropDownList drpMiscFee)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = clsDal.GetDataTableQry("SELECT DISTINCT FL.FeeId,FC.FeeName FROM dbo.Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FC.FeeID=FL.FeeID WHERE AdmnNo=" + this.drpstudent.SelectedValue.ToString().Trim() + " AND Balance>0 AND PeriodicityID=6");
        drpMiscFee.DataSource = dataTableQry;
        drpMiscFee.DataTextField = "FeeName";
        drpMiscFee.DataValueField = "FeeId";
        drpMiscFee.DataBind();
        drpMiscFee.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    private string PrevYr(string p)
    {
        int int32_1 = Convert.ToInt32(this.drpSession.SelectedValue.ToString().Trim().Substring(0, 4));
        int num = int32_1 - 1;
        int int32_2 = Convert.ToInt32(int32_1.ToString().Trim().Substring(2));
        string empty = string.Empty;
        return Convert.ToString(num) + "-" + Convert.ToString(int32_2);
    }

    protected void drpAdditionalFee_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.txtRcevMiscFee.Text = new clsDAL().ExecuteScalarQry("select CAST(Amount AS DECIMAL(10,2)) AS Amount from dbo.Host_FeeAmount where FeeCompId=" + this.drpAdditionalFee.SelectedValue + " and StreamId=(select StreamID from dbo.PS_ClassMaster where ClassID=" + this.drpclass.SelectedValue.ToString() + ")");
        this.drpAdditionalFee.Focus();
    }

    private void FillFeeMonth(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("- Select -", ""));
        drp.Items.Insert(1, new ListItem("On Admission", "0"));
        drp.Items.Insert(2, new ListItem("Apr", "4"));
        drp.Items.Insert(3, new ListItem("May", "5"));
        drp.Items.Insert(4, new ListItem("June", "6"));
        drp.Items.Insert(5, new ListItem("July", "7"));
        drp.Items.Insert(6, new ListItem("Aug", "8"));
        drp.Items.Insert(7, new ListItem("Sep", "9"));
        drp.Items.Insert(8, new ListItem("Oct", "10"));
        drp.Items.Insert(9, new ListItem("Nov", "11"));
        drp.Items.Insert(10, new ListItem("Dec", "12"));
        drp.Items.Insert(11, new ListItem("Jan", "1"));
        drp.Items.Insert(12, new ListItem("Feb", "2"));
        drp.Items.Insert(13, new ListItem("Mar", "3"));
        if (new clsGenerateFee().CreateCurrSession() != this.drpSession.SelectedValue.ToString().Trim())
        {
            if (!this.rBtnCurrentSess.Checked)
                return;
            drp.SelectedValue = "4";
        }
        else
        {
            DateTime.Now.Month.ToString();
            drp.SelectedValue = DateTime.Now.Month.ToString();
        }
    }

    protected void rBtnCurrentSess_CheckedChanged(object sender, EventArgs e)
    {
        this.setMiscDate();
        if (this.txtadmnno.Text.Trim() != string.Empty)
            this.GetAllFeeDetails();
        this.drpSession.Focus();
    }

    protected void btnRcvOldDue_Click(object sender, EventArgs e)
    {
        if (this.Request.QueryString["Sw"] != null)
            this.Response.Redirect("HostRcptPrevDue.aspx?admnno=" + this.txtadmnno.Text.Trim() + "&sess=" + this.drpSession.SelectedValue.ToString().Trim() + "&cid=" + this.drpclass.SelectedValue.ToString() + "&Sw=" + this.Request.QueryString["Sw"]);
        else
            this.Response.Redirect("HostRcptPrevDue.aspx?admnno=" + this.txtadmnno.Text.Trim() + "&sess=" + this.drpSession.SelectedValue.ToString().Trim() + "&cid=" + this.drpclass.SelectedValue.ToString());
    }

    protected void drpMiscFee_SelectedIndexChanged(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry;
        if (this.rBtnCurrentSess.Checked.Equals(true))
        {
            string str = this.drpSession.SelectedValue.Trim();
            DateTime dateTime1 = Convert.ToDateTime("1 Apr " + str.Substring(0, 4));
            DateTime dateTime2 = Convert.ToDateTime("31 Mar " + Convert.ToString(Convert.ToInt32(str.Substring(0, 4)) + 1));
            if (this.drpMiscFee.SelectedIndex > 0)
            {
                dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger WHERE AdmnNo=" + this.drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" + this.drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" + this.drpSession.SelectedValue.Trim() + "' AND TransDate <= GETDATE()");
                if (DateTime.Today < dateTime2 && DateTime.Today < dateTime1)
                    dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger WHERE AdmnNo=" + this.drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" + this.drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" + this.drpSession.SelectedValue.Trim() + "' AND TransDate >='1 Apr " + str.Substring(0, 4) + "'");
            }
            else
            {
                dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" + this.drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" + this.drpSession.SelectedValue.Trim() + "' AND TransDate <= GETDATE()");
                if (DateTime.Today < dateTime2 && DateTime.Today < dateTime1)
                    dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" + this.drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" + this.drpSession.SelectedValue.Trim() + "' AND TransDate >='1 Apr " + str.Substring(0, 4) + "'");
            }
        }
        else if (this.drpMiscFee.SelectedIndex > 0)
            dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger WHERE AdmnNo=" + this.drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" + this.drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" + this.PrevYr(this.drpSession.SelectedValue.Trim()) + "' AND TransDate <= GETDATE()");
        else
            dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" + this.drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" + this.PrevYr(this.drpSession.SelectedValue.Trim()) + "' AND TransDate <= GETDATE()");
        this.lblMiscFee.Text = dataTableQry.Rows[0][0].ToString();
        this.txtMiscFee.Text = this.lblMiscFee.Text;
        this.TotalAmount();
        this.drpMiscFee.Focus();
    }

    private void getMiscFeeMly()
    {
        string str1;
        string str2;
        if (this.drpMonthPayble.SelectedValue.Trim() == "0")
        {
            str1 = "4";
            str2 = 5.ToString();
        }
        else
        {
            str1 = this.drpMonthPayble.SelectedValue.ToString();
            str2 = (Convert.ToInt32(this.drpMonthPayble.SelectedValue.ToString().Trim()) + 1).ToString();
        }
        string str3 = this.drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        if (this.rBtnCurrentSess.Checked.Equals(false))
            str3 = (Convert.ToInt32(str3) - 1).ToString().Trim();
        string str4 = this.drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + this.drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
        if (this.rBtnCurrentSess.Checked.Equals(false))
            str4 = (Convert.ToInt32(str4) - 1).ToString().Trim();
        string str5 = Convert.ToInt32(str1) <= 3 ? str2 + "/01/" + str4 : (Convert.ToInt32(str1) <= 11 ? str2 + "/01/" + str3 : "01/01/" + str4);
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry;
        if (this.rBtnCurrentSess.Checked.Equals(true))
        {
            if (this.drpMiscFee.SelectedIndex > 0)
                dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger WHERE AdmnNo=" + this.drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" + this.drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" + this.drpSession.SelectedValue.Trim() + "' AND TransDate <'" + str5 + "'");
            else
                dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" + this.drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" + this.drpSession.SelectedValue.Trim() + "' AND TransDate <'" + str5 + "'");
        }
        else if (this.drpMiscFee.SelectedIndex > 0)
            dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger WHERE AdmnNo=" + this.drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" + this.drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" + this.PrevYr(this.drpSession.SelectedValue.Trim()) + "' AND TransDate <'" + str5 + "'");
        else
            dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" + this.drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" + this.PrevYr(this.drpSession.SelectedValue.Trim()) + "' AND TransDate <'" + str5 + "'");
        this.lblMiscFee.Text = dataTableQry.Rows[0][0].ToString();
        this.txtMiscFee.Text = this.lblMiscFee.Text;
    }

    private void statusOnFail()
    {
        if (this.ViewState["RcptNo"] != null)
            this.DelFeeOnFail(this.ViewState["RcptNo"].ToString().Trim());
        if (this.ViewState["Status"].ToString().Trim() == "DUP")
        {
            this.lblMsg.Text = "Receipt No already Exists.";
            this.lblMsg.ForeColor = Color.Red;
        }
        else
        {
            this.lblMsg.Text = "Transaction Failed. Try Again.";
            this.lblMsg.ForeColor = Color.Red;
        }
    }

    private void DelFeeOnFail(string RecptNo)
    {
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal1 = new clsDAL();
        hashtable.Add("receiptno", RecptNo);
        DataTable dataTable1 = new DataTable();
        clsDAL clsDal2 = new clsDAL();
        DataTable dataTable2 = clsDal1.GetDataTable("Host_DeleteFeeOnTransFail", hashtable);
        if (dataTable2.Rows.Count <= 0)
            return;
        this.updatefeeledger(dataTable2, 0);
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
            this.UpdateBalAmtInLedger(row, bal);
            num1 += bal;
        }
        this.updatefeeledgerDel(tb, 0);
    }

    private void updatefeeledgerDel(DataTable tbcredit, int mode)
    {
        clsDAL clsDal = new clsDAL();
        foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
            new clsDAL().GetDataTable("Host_Update_FeeLedger", new Hashtable()
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
          this.Session["User_Id"]
        },
        {
           "UserDate",
           DateTime.Now.ToString("dd MMM yyyy")
        },
        {
           "SchoolID",
           this.Session["SchoolId"].ToString()
        },
        {
           "mode",
           "Cash"
        }
      });
    }
}