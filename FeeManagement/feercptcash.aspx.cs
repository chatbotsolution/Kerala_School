using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class FeeManagement_feercptcash : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private string Rcptno = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnsave.Enabled = true;
        lblMsg.Text = "";
        lblMsg2.Text = "";
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        ViewState["Bal"] = 0;
        btnRcvOldDue.Visible = false;
        Session["BusFeeMonth"] = null;
        Session["HostelFeeMonth"] = null;
        Session["BookFeeMonth"] = null;
        ViewState["FY"] = "n";
        ViewState["Misc"] = "n";
        ViewState["BFY"] = "n";
        ViewState["BOOK"] = "n";
        ViewState["HFY"] = "n";
        ViewState["PrevDue"] = "0";
        ViewState["OldhostDue"] = "0";
        ViewState["hostPrevDue"]="0";
        ViewState["OldDue"] = "0";
        Page.Form.DefaultButton = btnsave.UniqueID;
        lblbalance.Text = "0.00";
        lblBusFee.Text = "0.00";
        //lblBookFee.Text = "0.00";
        lblMiscFee.Text = "0.00";
        lblHostelFee.Text = "0.00";
        fillsession();
        fillclass();
        fillAditionalFee();
        dtpFeeDt.SetDateValue(DateTime.Today);
        dtpMiscDt.SetDateValue(DateTime.Today);
        if (Request.QueryString["admnno"] != null)
        {
            txtadmnno.Text = Request.QueryString["admnno"].ToString();
            FillSelStudent();
            if (rbAll.Checked == true)
                FillFeeMonth(drpMonthPayble);
            else if (rbOnAdmission.Checked == true)
                FillFeeMonthOA(drpMonthPayble);
            else if (rbMonthly.Checked == true)
                FillFeeMonthly(drpMonthPayble);
            FillMonth(drpBusPayble);
            //FillMonth(drpBookPayble);
            FillMonth(drpHosPayble);
            FillMiscFeeNames(drpMiscFee);
            FillFeeAmt();
            txtadmnno.Text = drpstudent.SelectedValue;
            getbalance(drpstudent.SelectedValue.ToString());
            getMiscFee(drpstudent.SelectedValue.ToString());
        }
        txtadmnno.Focus();
        txtBank.Enabled = false;
        txtChqDate.Enabled = false;
        txtChqNo.Enabled = false;
        bindDropDown(drpBankAc, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=7 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
        drpBankAc.Visible = false;
        setMiscDate();
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select Bank Account---", "0"));
    }

    private void fillsession()
    {
        obj = new clsDAL();
        drpSession.DataSource = obj.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void setMiscDate()
    {
        string str = drpSession.SelectedValue.Trim();
        DateTime dateTime1 = Convert.ToDateTime("1 Apr" + str.Substring(0, 4));
        DateTime dateTime2 = Convert.ToDateTime("31 Mar" + Convert.ToString(Convert.ToInt32(str.Substring(0, 4)) + 1));
        if (DateTime.Today < dateTime2 && DateTime.Today < dateTime1)
        {
            if (rBtnPrevSess.Checked)
                dtpMiscDt.SetDateValue(dateTime1.AddDays(-1.0));
            else
                dtpMiscDt.SetDateValue(dateTime1);
            txtMiscDt.ReadOnly = true;
            dtpMiscDt.Enabled = false; ;
        }
        else if (DateTime.Today > dateTime2 && DateTime.Today > dateTime1)
        {
            dtpMiscDt.SetDateValue(dateTime2);
            txtMiscDt.ReadOnly = true;
            dtpMiscDt.Enabled = false; ;
        }
        else
        {
            if (rBtnPrevSess.Checked)
            {
                dtpMiscDt.SetDateValue(dateTime1.AddDays(-1.0));
                dtpMiscDt.Enabled = false;
            }
            else
            {
                dtpMiscDt.SetDateValue(DateTime.Today);
                dtpMiscDt.Enabled = false;
            }
            txtMiscDt.ReadOnly = true;
        }
    }

    private void fillclass()
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry("select * from ps_classmaster");
        drpclass.DataSource = dataTableQry;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("-Select-", "0"));
        if (dataTableQry.Rows.Count > 0)
            return;
        drpclass.Items.Insert(0, new ListItem("No Record", "0"));
    }

    private void GetStudImg()
    {
        if (drpstudent.SelectedIndex <= 0)
            return;
        obj = new clsDAL();
        string str = obj.ExecuteScalarQry("Select StudentPhoto from dbo.PS_StudMaster where Admnno=" + drpstudent.SelectedValue.ToString());
        if (str.Trim() != "")
            imgStud.ImageUrl = "../Up_Files/Studimage/" + str.Trim();
        else
            imgStud.ImageUrl = "../Up_Files/Studimage/noimage.jpg";
    }

    private void fillclassWithAdmnNo()
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry("select * from ps_classmaster");
        drpclass.DataSource = dataTableQry;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("-Select-", "0"));
        if (dataTableQry.Rows.Count > 0)
            return;
        drpclass.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    private void fillstudent()
    {
        obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select fullname,s.admnno from PS_ClasswiseStudent cs join ps_studmaster s on ");
        stringBuilder.Append(" s.admnno=cs.admnno where 1=1 and Status=1 ");
        if (drpclass.SelectedIndex != 0)
            stringBuilder.Append(" and cs.classid=" + drpclass.SelectedValue);
        //stringBuilder.Append(" and cs.SessionYear='" + drpSession.SelectedValue.ToString().Trim() + "' and  Detained_Promoted='' order by fullname ");
        stringBuilder.Append(" and cs.SessionYear='" + drpSession.SelectedValue.ToString().Trim() + "'order by fullname ");
        DataTable dataTableQry = obj.GetDataTableQry(stringBuilder.ToString().Trim());
        try
        {
            drpstudent.DataSource = dataTableQry;
            drpstudent.DataTextField = "fullname";
            drpstudent.DataValueField = "admnno";
            drpstudent.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        if (dataTableQry.Rows.Count > 0)
        {
            drpstudent.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        else
        {
            drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
            lblbalance.Text = "0";
            txtadmnno.Text = "";
        }
    }

    protected void fillAditionalFee()
    {
        drpAdditionalFee.DataSource = new clsDAL().GetDataTableQry("SELECT FC.FeeID,FC.FeeName,FC.PeriodicityID FROM dbo.PS_FeeComponents FC WHERE PeriodicityID=6 ORDER BY FeeName DESC");
        drpAdditionalFee.DataTextField = "FeeName";
        drpAdditionalFee.DataValueField = "FeeID";
        drpAdditionalFee.DataBind();
        drpAdditionalFee.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        TotalAmount();
        clsDAL clsDal = new clsDAL();
        if (clsDal.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=5").ToString().Trim() == "")
        {
            ScriptManager.RegisterStartupScript((Page)this, GetType(), "alertMessage", "alert('No Account Head Set To Receive Fee!! Please Set Account Head To Continue!!'); window.location='" + Request.ApplicationPath + "/Masters/SetAccountHead.aspx'", true);
        }
        else
        {
            if (!getValidInput())
                return;
            lblMsg.Text = "";
            lblMsg2.Text = "";
            string str = "select count(*) from dbo.Acts_PaymentReceiptVoucher where TransDt > '" + dtpFeeDt.GetDateValue().ToString("dd MMM yyyy") + "' and (IsDeleted is null or IsDeleted=0)";
            if (clsDal.GetDataTableQry("select top(1) FY,StartDate,EndDate from dbo.ACTS_FinancialYear where (IsFinalized is null or IsFinalized=0) and fy=dbo.fuGetSessionYr('" + dtpFeeDt.GetDateValue().ToString("dd MMM yyyy") + "') order by FY_Id desc").Rows.Count > 0)
            {
                if (clsDal.ExecuteScalarQry("select fy from [dbo].[ACTS_FinancialYear] where StartDate<='" + dtpFeeDt.GetDateValue().ToString("dd MMM yyyy") + "' and enddate>='" + dtpFeeDt.GetDateValue().ToString("dd MMM yyyy") + "'").ToString().Trim() != clsDal.ExecuteScalarQry("select fy from [dbo].[ACTS_FinancialYear] where StartDate<='" + DateTime.Now.ToString("dd MMM yyyy") + "' and enddate>='" + DateTime.Now.ToString("dd MMM yyyy") + "'").ToString().Trim())
                {
                    lblMsg.Text = "Receive date is not within the current Financial Year";
                    lblMsg.ForeColor = Color.Red;
                }
                else
                {
                    SaveData();
                   
                }
            }
            else
            {
                lblMsg.Text = "No running Financial available For Given Trans Date !!";
                lblMsg.ForeColor = Color.Red;
            }
        }
    }
    private void SaveData_hstl()
    {
        ViewState["FeeTable"] = string.Empty;
        ViewState["DrAcHead"] = string.Empty;
        ViewState["Status"] = string.Empty;
        if (drpPmtMode.SelectedValue.ToString().Trim() == "Cash")
            ViewState["DrAcHead"] = "3";
        else
            ViewState["DrAcHead"] = drpBankAc.SelectedValue.ToString();
        if (Request.QueryString["rno"] != null)
        {
            if (double.Parse(txtHostelFee.Text.Trim()) > 0.0)
                save_hstl();
            if (double.Parse(txtMiscFee.Text.Trim()) > 0.0)
                MiscFee_hstl();
            if (ViewState["RcptNo"] != null)
                obj.ExecuteScalar("Host_Insert_GenLedgerNew", new Hashtable()
        {
          {
             "@Receipt_VrNo",
            ViewState["RcptNo"].ToString().Trim()
          },
          {
             "@Amount",
             (!(txtHostelFee.Text.Trim() != "") && !(txtHostelFee.Text.Trim() != "0") || !(txtMiscFee.Text.Trim() != "") && !(txtMiscFee.Text.Trim() != "0") ? (!(txtHostelFee.Text.Trim() != "") && !(txtHostelFee.Text.Trim() != "0") || !(txtMiscFee.Text.Trim() == "") && !(txtMiscFee.Text.Trim() == "0") ? Convert.ToDecimal(txtMiscFee.Text.Trim()) : Convert.ToDecimal(txtHostelFee.Text.Trim())) : Convert.ToDecimal(txtHostelFee.Text.Trim()) + Convert.ToDecimal(txtMiscFee.Text.Trim()))
          },
          {
             "@TransDesc",
            txtdesc.Text.Trim()
          },
          {
             "@SessionYr",
            drpSession.SelectedValue
          },
          {
             "@UserId",
            Session["User_Id"].ToString()
          },
          {
             "@SchoolID",
            Session["SchoolId"].ToString()
          },
          {
             "@TransDate",
            dtpFeeDt.GetDateValue().ToString("dd MMM yyyy")
          },
          {
             "@AccountHeadDr",
             Convert.ToInt32(ViewState["DrAcHead"].ToString())
          }
        });
            clear();
        }
        else
        {
            string str = string.Empty;
            if (double.Parse(txtHostelFee.Text.Trim()) > 0.0)
                save_hstl();
            if (ViewState["Status"].ToString().Trim() != "DUP" && ViewState["Status"].ToString().Trim() != "NO")
            {
                if (double.Parse(txtMiscFee.Text.Trim()) > 0.0)
                    MiscFee_hstl();
                if (ViewState["Status"].ToString().Trim() != "DUP" && ViewState["Status"].ToString().Trim() != "NO")
                {
                    if (ViewState["RcptNo"] != null)
                        str = obj.ExecuteScalar("Host_Insert_GenLedgerNew", new Hashtable()
            {
              {
                 "@Receipt_VrNo",
                ViewState["RcptNo"].ToString().Trim()
              },
              {
                 "@Amount",
                 (!(txtHostelFee.Text.Trim() != "") && !(txtHostelFee.Text.Trim() != "0.00") || !(txtMiscFee.Text.Trim() != "") && !(txtMiscFee.Text.Trim() != "0.00") ? (!(txtHostelFee.Text.Trim() != "") && !(txtHostelFee.Text.Trim() != "0.00") || !(txtMiscFee.Text.Trim() == "") && !(txtMiscFee.Text.Trim() == "0.00") ? Convert.ToDecimal(txtMiscFee.Text.Trim()) : Convert.ToDecimal(txtHostelFee.Text.Trim())) : Convert.ToDecimal(txtHostelFee.Text.Trim()) + Convert.ToDecimal(txtMiscFee.Text.Trim()))
              },
              {
                 "@TransDesc",
                txtdesc.Text.Trim()
              },
              {
                 "@SessionYr",
                drpSession.SelectedValue
              },
              {
                 "@UserId",
                Session["User_Id"].ToString()
              },
              {
                 "@SchoolID",
                Session["SchoolId"].ToString()
              },
              {
                 "@TransDate",
                dtpFeeDt.GetDateValue().ToString("dd MMM yyyy")
              },
              {
                 "@AccountHeadDr",
                 Convert.ToInt32(ViewState["DrAcHead"].ToString())
              }
            });
                    if (str.Trim() != string.Empty)
                    {
                        ViewState["Status"] = "NO";
                        statusOnFail();
                    }
                }
                else
                    statusOnFail();
            }
            else
                statusOnFail();
        }
        DataTable dataTable = (DataTable)ViewState["FeeTable"];
        if (dataTable.Rows.Count <= 0 || !(dataTable.Rows[0][0].ToString().Trim() != "DUP") || !(dataTable.Rows[0][0].ToString().Trim() != "NO"))
            return;
        if (Request.QueryString["Sw"] != null)
        {
            if (ViewState["Amount"].ToString() != "")
            {
                // int num =Convert.ToInt64(Rcptno)-1;
                Session["OldRcptno"] = Rcptno;
                Rcptno = ViewState["Amount"].ToString();
            }
            Response.Redirect("../Reports/rptFeeReceipt.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + " &status=1 &rno=" + Rcptno + " &rcptdt=" + dtpFeeDt.GetDateValue().ToString("dd-MM-yyyy") + " &class=" + drpclass.SelectedValue.ToString() + " &D=p &ChkNo=" + txtChqNo.Text.Trim() + " &ChkDt=" + txtChqDate.Text.ToString().Trim() + " &Bn=" + txtBank.Text.ToString().Trim() + " &Pmnt=" + drpPmtMode.SelectedValue.ToString().Trim() + "&Sw=" + Request.QueryString["Sw"]);
        }
        else
        {
            if (ViewState["Amount"].ToString() != "")
            {
                Session["OldRcptno"] = Rcptno;
                Rcptno = ViewState["Amount"].ToString();
            }
            Response.Redirect("../Reports/rptFeeReceipt.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + " &status=1 &rno=" + Rcptno + " &rcptdt=" + dtpFeeDt.GetDateValue().ToString("dd-MM-yyyy") + " &class=" + drpclass.SelectedValue.ToString() + " &D=p &ChkNo=" + txtChqNo.Text.Trim() + " &ChkDt=" + txtChqDate.Text.ToString().Trim() + " &Bn=" + txtBank.Text.ToString().Trim() + " &Pmnt=" + drpPmtMode.SelectedValue.ToString().Trim());
        }

    }


    private void save_hstl()
    {
        Decimal num1 = new Decimal(0);
        Decimal CrAmt = new Decimal(0);
        Decimal num2;
        try
        {
            num2 = Convert.ToDecimal(txtHostelFee.Text);
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock((Control)txtHostelFee, txtHostelFee.GetType(), "ShowMessage", "alert('Invalid amount');", true);
            return;
        }
        if (num2 <= new Decimal(0) && CrAmt <= new Decimal(0))
        {
            ScriptManager.RegisterClientScriptBlock((Control)txtHostelFee, txtHostelFee.GetType(), "ShowMessage", "alert('Enter Amount greater than 0, There is no credit available in credit ledger.');", true);
        }
        else
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("Session", drpSession.SelectedValue);
            hashtable.Add("RecvDate", dtpFeeDt.GetDateValue().ToString("dd MMM yyyy"));
            hashtable.Add("AdmnNo", txtadmnno.Text);
            hashtable.Add("Description", txtdesc.Text.Trim());
            hashtable.Add("Amount", double.Parse(txtHostelFee.Text.Trim()));
            hashtable.Add("PaymentMode", drpPmtMode.SelectedValue.ToString());
            if (drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
            {
                hashtable.Add("ChequeNo", txtChqNo.Text.Trim());
                hashtable.Add("ChequeDate", txtChqDate.Text.ToString());
                hashtable.Add("DrawanOnBank", txtBank.Text);
                hashtable.Add("BankAcHeadId", int.Parse(drpBankAc.SelectedValue.ToString()));
            }
            hashtable.Add("UserID", Convert.ToInt32(Session["User_Id"].ToString().Trim()));
            hashtable.Add("SchoolID", Session["SchoolId"].ToString());
            StringBuilder stringBuilder = new StringBuilder();
            if (rBtnCurrentSess.Checked)
                hashtable.Add("CurrSessFee", 1);
            else
                hashtable.Add("CurrSessFee", 0);
            obj = new clsDAL();
            DataTable dataTable = new DataTable();
            DataTable tb = !(drpMonthPayble.SelectedValue.ToString().Trim() != "0") ? obj.GetDataTable("Host_Insert_FeeCash_OnAdmn", hashtable) : obj.GetDataTable("Host_Insert_FeeCash_OnFeeReceipt", hashtable);
            ViewState["FeeTable"] = tb;
            if (tb.Rows.Count > 0 && tb.Rows[0][0].ToString().Trim() != "DUP" && tb.Rows[0][0].ToString().Trim() != "NO")
            {
                Rcptno = tb.Rows[0]["ReceiptNo"].ToString();
                ViewState["RcptNo"] = Rcptno;
                Session["rcptFee"] = Rcptno;
                insertfeeledger_hstl(tb, CrAmt);
            }
            else
            {
                if (tb.Rows.Count <= 0 || !(tb.Rows[0][0].ToString().Trim() == "DUP") && !(tb.Rows[0][0].ToString().Trim() == "NO"))
                    return;
                ViewState["Status"] = tb.Rows[0][0].ToString().Trim();
            }
        }
    }


    private void insertfeeledger_hstl(DataTable tb, Decimal CrAmt)
    {
        Decimal result1 = new Decimal(0);
        Decimal.TryParse(txtHostelFee.Text, out result1);
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
            InsertCrAmtInLedger_hstl(row, CrAmt1);
            if (num1 <= new Decimal(0))
                break;
        }
        updatefeeledger_hstl(tb, 0);
    }

    private void updatefeeledger_hstl(DataTable tbcredit, int mode)
    {
        foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
        {
            if (row["debit"].ToString() != row["balance"].ToString())
            {
                obj = new clsDAL();
                obj.GetDataTable("Host_Update_FeeLedger", new Hashtable()
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
            drpPmtMode.SelectedValue.ToString().Trim()
          }
        });
            }
        }
    }

    private void InsertCrAmtInLedger_hstl(DataRow dr, Decimal CrAmt)
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("date", Convert.ToDateTime(dr["TransDate"]));
        string str1 = dtpFeeDt.GetDateValue().ToString("dd MMM yyyy");
        hashtable.Add("TransDate", str1);
        hashtable.Add("AdmnNo", Convert.ToInt64(dr["AdmnNo"]));
        hashtable.Add("TransDesc", Convert.ToString(dr["TransDesc"]));
        hashtable.Add("debit", 0);
        hashtable.Add("credit", CrAmt);
        hashtable.Add("balance", 0);
        hashtable.Add("Receipt_VrNo", Rcptno);
        hashtable.Add("userid", Convert.ToInt32(Session["User_Id"].ToString()));
        string str2 = DateTime.Now.ToString("dd MMM yyyy");
        hashtable.Add("UserDate", Convert.ToDateTime(str2, (IFormatProvider)CultureInfo.InvariantCulture));
        hashtable.Add("schoolid", Session["SchoolId"].ToString());
        hashtable.Add("FeeId", Convert.ToString(dr["FeeId"]));
        hashtable.Add("PayMode", drpPmtMode.SelectedValue.ToString().Trim());
        hashtable.Add("Session", dr["SessionYr"].ToString());
        hashtable.Add("AccountHeadDr", Convert.ToInt32(ViewState["DrAcHead"].ToString()));
        hashtable.Add("PR_Id", Convert.ToInt64(dr["PR_Id"]));
        obj = new clsDAL();
        obj.GetDataTable("Host_Insert_FeeLedgerNew", hashtable);
    }

    private void MiscFee_hstl()
    {
        Decimal AvlCredit = new Decimal(0);
        try
        {
            Convert.ToDecimal(txtMiscFee.Text);
        }
        catch
        {
            ScriptManager.RegisterClientScriptBlock((Control)txtHostelFee, txtHostelFee.GetType(), "ShowMessage", "alert('Invalid Amount');", true);
            return;
        }
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (ViewState["RcptNo"] != null)
            hashtable.Add("receiptno", ViewState["RcptNo"].ToString().Trim());
        hashtable.Add("Session", drpSession.SelectedValue);
        hashtable.Add("RecvDate", dtpFeeDt.GetDateValue().ToString("dd MMM yyyy"));
        hashtable.Add("AdmnNo", txtadmnno.Text);
        hashtable.Add("Description", txtdesc.Text.Trim());
        hashtable.Add("Amount", double.Parse(lblTotalFee.Text.Trim()));
        hashtable.Add("PaymentMode", drpPmtMode.SelectedValue.ToString());
        if (drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
        {
            hashtable.Add("ChequeNo", txtChqNo.Text.Trim());
            hashtable.Add("ChequeDate", txtChqDate.Text.ToString());
            hashtable.Add("DrawanOnBank", txtBank.Text);
            hashtable.Add("BankAcHeadId", int.Parse(drpBankAc.SelectedValue.ToString()));
        }
        hashtable.Add("UserID", Convert.ToInt32(Session["User_Id"].ToString().Trim()));
        hashtable.Add("SchoolID", Session["SchoolId"].ToString());
        hashtable.Add("@FeeId", drpMiscFee.SelectedValue.Trim().ToString());
        if (rBtnCurrentSess.Checked)
            hashtable.Add("@CurrSessFee", 1);
        else
            hashtable.Add("@CurrSessFee", 0);
        obj = new clsDAL();
        DataTable dataTable2 = obj.GetDataTable("Host_Insert_FeeCash_OnMiscReceipt", hashtable);
        ViewState["FeeTable"] = dataTable2;
        if (dataTable2.Rows.Count > 0 && dataTable2.Rows[0][0].ToString().Trim() != "DUP" && dataTable2.Rows[0][0].ToString().Trim() != "NO")
        {
            Rcptno = dataTable2.Rows[0]["ReceiptNo"].ToString();
            ViewState["RcptNo"] = Rcptno;
            Session["rcptFee"] = Rcptno;
            InsertMiscfeeLedger_hstl(dataTable2, AvlCredit);
        }
        else
        {
            if (dataTable2.Rows.Count <= 0 || !(dataTable2.Rows[0][0].ToString().Trim() == "DUP") && !(dataTable2.Rows[0][0].ToString().Trim() == "NO"))
                return;
            ViewState["Status"] = dataTable2.Rows[0][0].ToString().Trim();
        }
    }

    private void InsertMiscfeeLedger_hstl(DataTable tb, Decimal AvlCredit)
    {
        Decimal result1 = new Decimal(0);
        Decimal.TryParse(txtMiscFee.Text, out result1);
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
            InsertCrAmtInLedger_hstl(row, CrAmt);
            if (num1 <= new Decimal(0))
                break;
        }
        updatefeeledger_hstl(tb, 0);
    }


    private bool getValidInput()
    {
        double num = Convert.ToDouble(ViewState["PrevDue"]);
        double num1= Convert.ToDouble(ViewState["OldDue"]);
        double num2 = Convert.ToDouble(ViewState["hostPrevDue"]);
        double num3 = Convert.ToDouble(ViewState["OldhostDue"]);
        if (num <= 0.0 || !rBtnCurrentSess.Checked.Equals(true)&&num1<=0.0)
            if ((num2 <= 0.0 || !rBtnCurrentSess.Checked.Equals(true)) && num3 <= 0.0)
                return true;
        //lblMsg.Text = "Please Clear Previous Due Rs." + ViewState["PrevDue"] + " first !";
        lblMsg.Text= num1 <= 0.0 ? (lblMsg2.Text = "Please Clear Previous Due Rs." + ViewState["PrevDue"] + " first.") : (lblMsg2.Text = "Please Clear Old Due Rs." + ViewState["OldDue"] + "  first.");
        //lblMsg2.Text = "Please Clear Previous Due Rs." + ViewState["PrevDue"] + " first !";
        lblMsg.ForeColor = Color.Red;
        //lblMsg2.ForeColor = Color.Red;

        lblMsg.Text = num3 <= 0.0 ? (lblMsg2.Text = "Please Clear Previous Due Rs." + ViewState["hostPrevDue"] + " first.") : (lblMsg2.Text = "Please Clear Old Hostel Due Rs." + ViewState["OldhostDue"] + "  first.");
        lblMsg.ForeColor = lblMsg2.ForeColor = Color.Red;
        return false;
    }

    private void SaveData()
    {
        DataTable dataTable = new DataTable();
        SqlConnection Conn = new SqlConnection();
        Conn.ConnectionString = ConfigurationManager.AppSettings["conString"];
        if (Conn.State == ConnectionState.Closed)
            Conn.Open();
        SqlTransaction transaction = Conn.BeginTransaction();
        try
        {
            ViewState["FeeTable"] = string.Empty;
            ViewState["FeeTblRvt"] = string.Empty;
            ViewState["DrAcHead"] = string.Empty;
            ViewState["Status"] = string.Empty;
            if (drpPmtMode.SelectedValue.ToString().Trim() == "Cash")
                ViewState["DrAcHead"] = "3";
            else
                ViewState["DrAcHead"] = drpBankAc.SelectedValue.ToString();
            string str = "";
            if (double.Parse(txtamt.Text.Trim()) > 0.0)
                save(Conn, transaction);
            if (ViewState["Status"].ToString().Trim() != "DUP" && ViewState["Status"].ToString().Trim() != "NO")
            {
                if (double.Parse(txtMiscFee.Text.Trim()) > 0.0)
                    MiscFee(Conn, transaction);
                if (ViewState["Status"].ToString().Trim() != "DUP" && ViewState["Status"].ToString().Trim() != "NO")
                {
                    if (ViewState["RcptNo"] != null)
                    {
                      

                        Decimal num = !(txtamt.Text.Trim() != "") && !(txtamt.Text.Trim() != "0.00") || !(txtMiscFee.Text.Trim() != "") && !(txtMiscFee.Text.Trim() != "0.00") ? (!(txtamt.Text.Trim() != "") && !(txtamt.Text.Trim() != "0.00") || !(txtMiscFee.Text.Trim() == "") && !(txtMiscFee.Text.Trim() == "0.00") ? Convert.ToDecimal(txtMiscFee.Text.Trim()) : Convert.ToDecimal(txtamt.Text.Trim())) : Convert.ToDecimal(txtamt.Text.Trim()) + Convert.ToDecimal(txtMiscFee.Text.Trim());
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.Connection = Conn;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Transaction = transaction;
                        SqlParameter sqlParameter1 = new SqlParameter("@Receipt_VrNo", ViewState["RcptNo"].ToString().Trim());
                        SqlParameter sqlParameter2 = new SqlParameter("@Amount", num);
                        SqlParameter sqlParameter3 = new SqlParameter("@TransDesc", txtdesc.Text.Trim());
                        SqlParameter sqlParameter4 = new SqlParameter("@SessionYr", drpSession.SelectedValue);
                        SqlParameter sqlParameter5 = new SqlParameter("@UserId", Session["User_Id"].ToString());
                        SqlParameter sqlParameter6 = new SqlParameter("@SchoolID", Session["SchoolId"].ToString());
                        SqlParameter sqlParameter7 = new SqlParameter("@TransDate", dtpFeeDt.GetDateValue().ToString("dd MMM yyyy"));
                        SqlParameter sqlParameter8 = new SqlParameter("@AccountHeadDr", Convert.ToInt32(ViewState["DrAcHead"].ToString()));
                        sqlCommand.CommandText = "ps_insert_GenLedgerNew";
                        sqlCommand.Parameters.Add(sqlParameter1);
                        sqlCommand.Parameters.Add(sqlParameter2);
                        sqlCommand.Parameters.Add(sqlParameter3);
                        sqlCommand.Parameters.Add(sqlParameter4);
                        sqlCommand.Parameters.Add(sqlParameter5);
                        sqlCommand.Parameters.Add(sqlParameter6);
                        sqlCommand.Parameters.Add(sqlParameter7);
                        sqlCommand.Parameters.Add(sqlParameter8);
                        str = sqlCommand.ExecuteScalar().ToString().Trim();
                    }
                    if (str.Trim() == "")
                    {
                        if (double.Parse(txtBusFee.Text.Trim()) > 0.0)
                            insertBusFee(Conn, transaction);
                        if (ViewState["Status"].ToString().Trim() != "DUP" && ViewState["Status"].ToString().Trim() != "NO")
                        {
                            
                            //if (double.Parse(txtBookFee.Text.Trim()) > 0.0)
                            //    insertBookFee(Conn, transaction);
                            if (ViewState["Status"].ToString().Trim() == "DUP" || ViewState["Status"].ToString().Trim() == "NO")
                                transaction.Rollback();
                        }
                        else
                            transaction.Rollback();
                    }
                    else
                    {
                        ViewState["Status"] = "NO";
                        transaction.Rollback();
                    }
                }
                else
                    transaction.Rollback();
            }
            else
                transaction.Rollback();
            if (ViewState["FeeTable"] != null)
                dataTable = (DataTable)ViewState["FeeTable"];
            if (transaction.Connection != null)
                transaction.Commit();

          



        }
        catch (Exception ex)
        {
            transaction.Rollback();
        }
        transaction.Dispose();
        Conn.Dispose();
        ViewState["Amount"] = "";
        Session["OldRcptno"] = "";
        if (dataTable.Rows.Count > 0 && dataTable.Rows[0][0].ToString().Trim() != "DUP" && (dataTable.Rows[0][0].ToString().Trim() != "NO" && ViewState["Status"].ToString().Trim() != "NO") && ViewState["Status"].ToString().Trim() != "DUP")
        {
            if (txtHostelFee.Text != "0.00")
            {
                ViewState["Amount"] = ViewState["RcptNo"].ToString();
                SaveData_hstl();
            }
            else
            {


                if (Request.QueryString["Sw"] != null)
                    Response.Redirect("../Reports/rptFeeReceipt.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + " &status=1 &rno=" + Rcptno + " &rcptdt=" + dtpFeeDt.GetDateValue().ToString("dd-MM-yyyy") + "&class=" + drpclass.SelectedValue.ToString() + " &D=p &ChkNo=" + txtChqNo.Text.Trim() + " &ChkDt=" + txtChqDate.Text.ToString().Trim() + " &Bn=" + txtBank.Text.ToString().Trim() + " &Pmnt=" + drpPmtMode.SelectedValue.ToString().Trim() + "&Sw=" + Request.QueryString["Sw"]);
                else
                    Response.Redirect("../Reports/rptFeeReceipt.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + " &status=1 &rno=" + Rcptno + " &rcptdt=" + dtpFeeDt.GetDateValue().ToString("dd-MM-yyyy") + "&class=" + drpclass.SelectedValue.ToString() + " &D=p &ChkNo=" + txtChqNo.Text.Trim() + " &ChkDt=" + txtChqDate.Text.ToString().Trim() + " &Bn=" + txtBank.Text.ToString().Trim() + " &Pmnt=" + drpPmtMode.SelectedValue.ToString().Trim());


            }
        }
        else if (ViewState["Status"].ToString().Trim() == "DUP")
        {
            if (txtHostelFee.Text != "0.00")
            {
                SaveData_hstl();
            }

            lblMsg.Text = "Receipt No Already Exists!";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            if (txtHostelFee.Text != "0.00")
            {
                SaveData_hstl();
            }
            lblMsg.Text = "Transaction Failed Please Try Again Later!";
            lblMsg.ForeColor = Color.Red;
        }

       



    }

    private void save(SqlConnection Conn, SqlTransaction transaction)
    {
        try
        {
            Decimal num1 = new Decimal(0);
            Decimal num2 = new Decimal(0);
            Decimal num3;
            Decimal CrAmt;
            try
            {
                num3 = Convert.ToDecimal(txtamt.Text);
                CrAmt = Convert.ToDecimal(lblCreditAmt.Text);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtamt, txtamt.GetType(), "ShowMessage", "alert('Invalid amount');", true);
                return;
            }
            if (num3 <= new Decimal(0) && CrAmt <= new Decimal(0))
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtamt, txtamt.GetType(), "ShowMessage", "alert('Enter Amount greater than 0, There is no credit available in credit ledger.');", true);
            }
            else
            {
                Hashtable hashtable = new Hashtable();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = Conn;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = transaction;
                obj = new clsDAL();
                DataTable dataTable = new DataTable();
                if (rbMonthly.Checked)
                    sqlCommand.CommandText = "ps_sp_insert_feecash_OnFeeReceiptMonth";
                else if (drpMonthPayble.SelectedValue.ToString().Trim() != "0")
                    sqlCommand.CommandText = "ps_sp_insert_feecash_OnFeeReceipt";
                else
                    sqlCommand.CommandText = "ps_sp_insert_feecash_OnAdmn";
                SqlParameter sqlParameter1 = new SqlParameter();
                SqlParameter sqlParameter2 = new SqlParameter();
                SqlParameter sqlParameter3 = new SqlParameter();
                SqlParameter sqlParameter4 = new SqlParameter();
                SqlParameter sqlParameter5 = new SqlParameter();
                SqlParameter sqlParameter6 = new SqlParameter();
                SqlParameter sqlParameter7 = new SqlParameter();
                SqlParameter sqlParameter8 = new SqlParameter();
                SqlParameter sqlParameter9 = new SqlParameter();
                SqlParameter sqlParameter10 = new SqlParameter();
                SqlParameter sqlParameter11 = new SqlParameter();
                SqlParameter sqlParameter12 = new SqlParameter();
                SqlParameter sqlParameter13 = new SqlParameter();
                SqlParameter sqlParameter14 = new SqlParameter();
                sqlParameter2.ParameterName = "@Session";
                sqlParameter2.Value = drpSession.SelectedValue.Trim();
                sqlParameter3.ParameterName = "@RecvDate";
                sqlParameter3.Value = dtpFeeDt.GetDateValue().ToString("dd MMM yyyy");
                sqlParameter3.SqlDbType = SqlDbType.DateTime;
                sqlParameter4.ParameterName = "@AdmnNo";
                sqlParameter4.Value = txtadmnno.Text;
                sqlParameter5.ParameterName = "@Description";
                sqlParameter5.Value = txtdesc.Text.Trim();
                sqlParameter6.ParameterName = "@Amount";
                sqlParameter6.Value = double.Parse(lblTotalFee.Text.Trim());
                sqlParameter7.ParameterName = "@PaymentMode";
                sqlParameter7.Value = drpPmtMode.SelectedValue.ToString();
                if (drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
                {
                    sqlParameter8.ParameterName = "@ChequeNo";
                    sqlParameter8.Value = txtChqNo.Text.Trim();
                    sqlParameter9.ParameterName = "@ChequeDate";
                    sqlParameter9.Value = txtChqDate.Text.ToString();
                    sqlParameter10.ParameterName = "@DrawanOnBank";
                    sqlParameter10.Value = txtBank.Text;
                    sqlParameter11.ParameterName = "@BankAcHeadId";
                    sqlParameter11.Value = int.Parse(drpBankAc.SelectedValue.ToString());
                }
                sqlParameter12.ParameterName = "@UserID";
                sqlParameter12.Value = Convert.ToInt32(Session["User_Id"].ToString().Trim());
                sqlParameter13.ParameterName = "@SchoolID";
                sqlParameter13.Value = Session["SchoolId"].ToString();
                if (rBtnCurrentSess.Checked)
                {
                    sqlParameter14.ParameterName = "@CurrSessFee";
                    sqlParameter14.Value = "1";
                }
                else
                {
                    sqlParameter14.ParameterName = "@CurrSessFee";
                    sqlParameter14.Value = "0";
                }
                sqlCommand.Parameters.Add(sqlParameter2);
                sqlCommand.Parameters.Add(sqlParameter3);
                sqlCommand.Parameters.Add(sqlParameter4);
                sqlCommand.Parameters.Add(sqlParameter5);
                sqlCommand.Parameters.Add(sqlParameter6);
                sqlCommand.Parameters.Add(sqlParameter7);
                if (drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
                {
                    sqlCommand.Parameters.Add(sqlParameter8);
                    sqlCommand.Parameters.Add(sqlParameter9);
                    sqlCommand.Parameters.Add(sqlParameter10);
                    sqlCommand.Parameters.Add(sqlParameter11);
                }
                sqlCommand.Parameters.Add(sqlParameter12);
                sqlCommand.Parameters.Add(sqlParameter13);
                sqlCommand.Parameters.Add(sqlParameter14);
                new SqlDataAdapter() { SelectCommand = sqlCommand }.Fill(dataTable);
                ViewState["FeeTable"] = dataTable;
                if (dataTable.Rows.Count > 0 && dataTable.Rows[0][0].ToString().Trim() != "DUP" && dataTable.Rows[0][0].ToString().Trim() != "NO")
                {
                    Rcptno = dataTable.Rows[0]["ReceiptNo"].ToString();
                    ViewState["RcptNo"] = Rcptno;
                    Session["rcptFee"] = Rcptno;
                    insertfeeledger(dataTable, CrAmt, Conn, transaction);
                }
                else
                {
                    if (dataTable.Rows.Count <= 0 || !(dataTable.Rows[0][0].ToString().Trim() == "DUP") && !(dataTable.Rows[0][0].ToString().Trim() == "NO"))
                        return;
                    ViewState["Status"] = dataTable.Rows[0][0].ToString().Trim();
                }
            }
        }
        catch (Exception ex)
        {
            ViewState["Status"] = "NO";
        }
    }

    private void MiscFee(SqlConnection Conn, SqlTransaction transaction)
    {
        try
        {
            Decimal num1 = new Decimal(0);
            Decimal num2 = new Decimal(0);
            Decimal num3;
            Decimal AvlCredit;
            try
            {
                num3 = Convert.ToDecimal(txtMiscFee.Text);
                AvlCredit = Convert.ToDecimal(lblCreditAmt.Text);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtamt, txtamt.GetType(), "ShowMessage", "alert('Invalid amount');", true);
                return;
            }
            if (num3 <= new Decimal(0) && AvlCredit <= new Decimal(0))
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtamt, txtamt.GetType(), "ShowMessage", "alert('Enter Amount greater than 0, There is no credit available in credit ledger.');", true);
            }
            else
            {
                DataTable dataTable = new DataTable();
                Hashtable hashtable = new Hashtable();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = Conn;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = transaction;
                sqlCommand.CommandText = "ps_sp_insert_feecash_OnMiscReceipt";
                SqlParameter sqlParameter1 = new SqlParameter();
                SqlParameter sqlParameter2 = new SqlParameter();
                SqlParameter sqlParameter3 = new SqlParameter();
                SqlParameter sqlParameter4 = new SqlParameter();
                SqlParameter sqlParameter5 = new SqlParameter();
                SqlParameter sqlParameter6 = new SqlParameter();
                SqlParameter sqlParameter7 = new SqlParameter();
                SqlParameter sqlParameter8 = new SqlParameter();
                SqlParameter sqlParameter9 = new SqlParameter();
                SqlParameter sqlParameter10 = new SqlParameter();
                SqlParameter sqlParameter11 = new SqlParameter();
                SqlParameter sqlParameter12 = new SqlParameter();
                SqlParameter sqlParameter13 = new SqlParameter();
                SqlParameter sqlParameter14 = new SqlParameter();
                SqlParameter sqlParameter15 = new SqlParameter();
                if (ViewState["RcptNo"] != null)
                {
                    sqlParameter1.ParameterName = "@receiptno";
                    sqlParameter1.Value = ViewState["RcptNo"].ToString().Trim();
                }
                sqlParameter2.ParameterName = "@Session";
                sqlParameter2.Value = drpSession.SelectedValue.Trim();
                sqlParameter3.ParameterName = "@RecvDate";
                sqlParameter3.Value = dtpFeeDt.GetDateValue().ToString("dd MMM yyyy");
                sqlParameter3.SqlDbType = SqlDbType.DateTime;
                sqlParameter4.ParameterName = "@AdmnNo";
                sqlParameter4.Value = txtadmnno.Text;
                sqlParameter5.ParameterName = "@Description";
                sqlParameter5.Value = txtdesc.Text.Trim();
                sqlParameter6.ParameterName = "@Amount";
                sqlParameter6.Value = double.Parse(lblTotalFee.Text.Trim());
                sqlParameter7.ParameterName = "@PaymentMode";
                sqlParameter7.Value = drpPmtMode.SelectedValue.ToString();
                if (drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
                {
                    sqlParameter8.ParameterName = "@ChequeNo";
                    sqlParameter8.Value = txtChqNo.Text.Trim();
                    sqlParameter9.ParameterName = "@ChequeDate";
                    sqlParameter9.Value = txtChqDate.Text.ToString();
                    sqlParameter10.ParameterName = "@DrawanOnBank";
                    sqlParameter10.Value = txtBank.Text;
                    sqlParameter11.ParameterName = "@BankAcHeadId";
                    sqlParameter11.Value = int.Parse(drpBankAc.SelectedValue.ToString());
                }
                sqlParameter12.ParameterName = "@UserID";
                sqlParameter12.Value = Convert.ToInt32(Session["User_Id"].ToString().Trim());
                sqlParameter13.ParameterName = "@SchoolID";
                sqlParameter13.Value = Session["SchoolId"].ToString();
                if (rBtnCurrentSess.Checked)
                {
                    sqlParameter14.ParameterName = "@CurrSessFee";
                    sqlParameter14.Value = "1";
                }
                else
                {
                    sqlParameter14.ParameterName = "@CurrSessFee";
                    sqlParameter14.Value = "0";
                }
                sqlParameter15.ParameterName = "@FeeId";
                sqlParameter15.Value = drpMiscFee.SelectedValue.Trim().ToString();
                if (ViewState["RcptNo"] != null)
                    sqlCommand.Parameters.Add(sqlParameter1);
                sqlCommand.Parameters.Add(sqlParameter2);
                sqlCommand.Parameters.Add(sqlParameter3);
                sqlCommand.Parameters.Add(sqlParameter4);
                sqlCommand.Parameters.Add(sqlParameter5);
                sqlCommand.Parameters.Add(sqlParameter6);
                sqlCommand.Parameters.Add(sqlParameter7);
                if (drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
                {
                    sqlCommand.Parameters.Add(sqlParameter8);
                    sqlCommand.Parameters.Add(sqlParameter9);
                    sqlCommand.Parameters.Add(sqlParameter10);
                    sqlCommand.Parameters.Add(sqlParameter11);
                }
                sqlCommand.Parameters.Add(sqlParameter12);
                sqlCommand.Parameters.Add(sqlParameter13);
                sqlCommand.Parameters.Add(sqlParameter14);
                sqlCommand.Parameters.Add(sqlParameter15);
                new SqlDataAdapter() { SelectCommand = sqlCommand }.Fill(dataTable);
                ViewState["FeeTable"] = dataTable;
                if (dataTable.Rows.Count > 0 && dataTable.Rows[0][0].ToString().Trim() != "DUP" && dataTable.Rows[0][0].ToString().Trim() != "NO")
                {
                    Rcptno = dataTable.Rows[0]["ReceiptNo"].ToString();
                    ViewState["RcptNo"] = Rcptno;
                    Session["rcptFee"] = Rcptno;
                    InsertMiscfeeLedger(dataTable, AvlCredit, Conn, transaction);
                }
                else
                {
                    if (dataTable.Rows.Count <= 0 || !(dataTable.Rows[0][0].ToString().Trim() == "DUP") && !(dataTable.Rows[0][0].ToString().Trim() == "NO"))
                        return;
                    ViewState["Status"] = dataTable.Rows[0][0].ToString().Trim();
                }
            }
        }
        catch (Exception ex)
        {
            ViewState["Status"] = "NO";
        }
    }

    private void InsertMiscfeeLedger(DataTable tb, Decimal AvlCredit, SqlConnection Conn, SqlTransaction transaction)
    {
        try
        {
            Decimal result1 = new Decimal(0);
            Decimal.TryParse(txtMiscFee.Text, out result1);
            Decimal num1 = result1 + AvlCredit;
            string msg = "";
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
                InsertCrAmtInLedger(row, CrAmt, Conn, transaction, out msg);
                if (msg.ToUpper().Trim() != "S")
                {
                    ViewState["Status"] = "NO";
                    break;
                }
                if (num1 <= new Decimal(0))
                    break;
            }
            if (ViewState["Status"].ToString().Trim() != "NO")
                updatefeeledger(tb, 0, Conn, transaction);
            if (num1 > new Decimal(0))
                Session["CreditAmount"] = string.Format("{0:F2}", num1);
            else
                Session["CreditAmount"] = string.Format("{0:F2}", new Decimal(0));
        }
        catch (Exception ex)
        {
            ViewState["Status"] = "NO";
        }
    }

    private void insertBusFee(SqlConnection Conn, SqlTransaction transaction)
    {
        try
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = Conn;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Transaction = transaction;
            Convert.ToDecimal(lblBusFee.Text.Trim());
            Convert.ToDecimal(txtBusFee.Text.Trim());
            Decimal num1 = new Decimal(0);
            Decimal num2;
            try
            {
                num2 = Convert.ToDecimal(txtBusFee.Text);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtBusFee, txtBusFee.GetType(), "ShowMessage", "alert('Invalid amount');", true);
                return;
            }
            if (num2 <= new Decimal(0))
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtBusFee, txtBusFee.GetType(), "ShowMessage", "alert('Amount should be greater than 0');", true);
            }
            else
            {
                DataTable dataTable1 = new DataTable();
                if (double.Parse(txtamt.Text.Trim()) <= 0.0 && double.Parse(txtMiscFee.Text.Trim()) <= 0.0)
                {
                    sqlCommand.CommandText = "ps_sp_insert_feecashBus_Hostel";
                    SqlParameter sqlParameter1 = new SqlParameter();
                    if (Request.QueryString["rno"] != null)
                        sqlParameter1 = new SqlParameter("@receiptno", Request.QueryString["rno"]);
                    SqlParameter sqlParameter2 = new SqlParameter("@Session", drpSession.SelectedValue);
                    SqlParameter sqlParameter3 = new SqlParameter("@RecvDate", dtpFeeDt.GetDateValue().ToString("dd MMM yyyy"));
                    SqlParameter sqlParameter4 = new SqlParameter("@AdmnNo", txtadmnno.Text);
                    SqlParameter sqlParameter5 = new SqlParameter("@Description", txtdesc.Text.Trim());
                    SqlParameter sqlParameter6 = new SqlParameter("@Amount", double.Parse(lblTotalFee.Text.Trim()));
                    SqlParameter sqlParameter7 = new SqlParameter("@PaymentMode", drpPmtMode.SelectedValue.ToString());
                    SqlParameter sqlParameter8 = new SqlParameter();
                    SqlParameter sqlParameter9 = new SqlParameter();
                    SqlParameter sqlParameter10 = new SqlParameter();
                    SqlParameter sqlParameter11 = new SqlParameter();
                    if (drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
                    {
                        sqlParameter8 = new SqlParameter("@ChequeNo", txtChqNo.Text.Trim());
                        sqlParameter9 = new SqlParameter("@ChequeDate", txtChqDate.Text.ToString());
                        sqlParameter10 = new SqlParameter("@DrawanOnBank", txtBank.Text);
                        sqlParameter11 = new SqlParameter("@BankAcHeadId", int.Parse(drpBankAc.SelectedValue.ToString()));
                    }
                    SqlParameter sqlParameter12 = new SqlParameter("@UserID", Convert.ToInt32(Session["User_Id"].ToString().Trim()));
                    SqlParameter sqlParameter13 = new SqlParameter("@SchoolID", Session["SchoolId"].ToString());
                    if (Request.QueryString["rno"] != null)
                        sqlCommand.Parameters.Add(sqlParameter1);
                    sqlCommand.Parameters.Add(sqlParameter2);
                    sqlCommand.Parameters.Add(sqlParameter3);
                    sqlCommand.Parameters.Add(sqlParameter4);
                    sqlCommand.Parameters.Add(sqlParameter5);
                    sqlCommand.Parameters.Add(sqlParameter6);
                    sqlCommand.Parameters.Add(sqlParameter7);
                    if (drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
                    {
                        sqlCommand.Parameters.Add(sqlParameter8);
                        sqlCommand.Parameters.Add(sqlParameter9);
                        sqlCommand.Parameters.Add(sqlParameter10);
                        sqlCommand.Parameters.Add(sqlParameter11);
                    }
                    sqlCommand.Parameters.Add(sqlParameter12);
                    sqlCommand.Parameters.Add(sqlParameter13);
                    new SqlDataAdapter()
                    {
                        SelectCommand = sqlCommand
                    }.Fill(dataTable1);
                    ViewState["FeeTable"] = dataTable1;
                }
                if (dataTable1.Rows.Count > 0 && dataTable1.Rows[0][0].ToString().Trim() != "DUP" && dataTable1.Rows[0][0].ToString().Trim() != "NO")
                {
                    Rcptno = dataTable1.Rows[0]["ReceiptNo"].ToString();
                    ViewState["RcptNo"] = Rcptno;
                    Session["rcptFee"] = Rcptno;
                }
                else if (dataTable1.Rows.Count > 0 && (dataTable1.Rows[0][0].ToString().Trim() == "DUP" || dataTable1.Rows[0][0].ToString().Trim() == "NO"))
                    ViewState["Status"] = dataTable1.Rows[0][0].ToString().Trim();
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = "select  AdditinalFeeCredit from PS_StudCreditLedger where admnno=" + txtadmnno.Text.Trim();
                Convert.ToDouble(sqlCommand.ExecuteScalar());
                StringBuilder stringBuilder = new StringBuilder();
                sqlCommand.Parameters.Clear();
                if (rBtnCurrentSess.Checked)
                {
                    stringBuilder.Append("Select AdFeeNo, ClassWiseId, credit , Balance from PS_AdFeeLedger f ");
                    stringBuilder.Append(" inner join PS_ClasswiseStudent c on  c.Id = f.ClassWiseId ");
                    stringBuilder.Append(" where c.admnno='" + txtadmnno.Text.Trim() + "' and f.balance > 0 and f.Ad_Id=1 and f.sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' order by f.AdFeeDate asc");
                }
                else
                {
                    string str = "04/01/" + drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
                    stringBuilder.Append("Select AdFeeNo, ClassWiseId, credit , Balance from PS_AdFeeLedger f");
                    stringBuilder.Append(" inner join PS_ClasswiseStudent c on  c.Id = f.ClassWiseId ");
                    stringBuilder.Append(" where c.admnno='" + txtadmnno.Text.Trim() + "' and f.balance > 0 and f.Ad_Id=1 and f.AdFeeDate < '" + str + "' order by f.AdFeeDate asc");
                }
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = stringBuilder.ToString().Trim();
                DataTable dataTable2 = new DataTable();
                new SqlDataAdapter() { SelectCommand = sqlCommand }.Fill(dataTable2);
                if (dataTable2.Rows.Count <= 0 || !(Rcptno != ""))
                    return;
                insertBusledger(dataTable2, Conn, transaction);
            }
        }
        catch (Exception ex)
        {
            ViewState["Status"] = "NO";
        }
    }

    private void insertBookFee(SqlConnection Conn, SqlTransaction transaction)
    {
        //try
        //{
        //    SqlCommand sqlCommand = new SqlCommand();
        //    sqlCommand.Connection = Conn;
        //    sqlCommand.CommandType = CommandType.StoredProcedure;
        //    sqlCommand.Transaction = transaction;
        //    Convert.ToDecimal(lblBookFee.Text.Trim());
        //    Convert.ToDecimal(txtBookFee.Text.Trim());
        //    Decimal num1 = new Decimal(0);
        //    Decimal num2;
        //    try
        //    {
        //        num2 = Convert.ToDecimal(txtBookFee.Text);
        //    }
        //    catch
        //    {
        //        ScriptManager.RegisterClientScriptBlock((Control)txtBookFee, txtBookFee.GetType(), "ShowMessage", "alert('Invalid amount');", true);
        //        return;
        //    }
        //    if (num2 <= new Decimal(0))
        //    {
        //        ScriptManager.RegisterClientScriptBlock((Control)txtBookFee, txtBookFee.GetType(), "ShowMessage", "alert('Amount should be greater than 0');", true);
        //    }
        //    else
        //    {
        //        DataTable dataTable1 = new DataTable();
        //        if (double.Parse(txtamt.Text.Trim()) == 0.0 && double.Parse(txtMiscFee.Text.Trim()) == 0.0 && double.Parse(txtBusFee.Text.Trim()) == 0.0)
        //        {
        //            if (Request.QueryString["rno"] != null)
        //                sqlCommand.CommandText = "ps_sp_update_feecash";
        //            else
        //                sqlCommand.CommandText = "ps_sp_insert_feecashBook";
        //            SqlParameter sqlParameter1 = new SqlParameter();
        //            if (Request.QueryString["rno"] != null)
        //                sqlParameter1 = new SqlParameter("@receiptno", Request.QueryString["rno"]);
        //            SqlParameter sqlParameter2 = new SqlParameter("@Session", drpSession.SelectedValue);
        //            SqlParameter sqlParameter3 = new SqlParameter("@RecvDate", dtpFeeDt.GetDateValue().ToString("dd MMM yyyy"));
        //            SqlParameter sqlParameter4 = new SqlParameter("@AdmnNo", txtadmnno.Text);
        //            SqlParameter sqlParameter5 = new SqlParameter("@Description", txtdesc.Text.Trim());
        //            SqlParameter sqlParameter6 = new SqlParameter("@Amount", double.Parse(lblTotalFee.Text.Trim()));
        //            SqlParameter sqlParameter7 = new SqlParameter("@PaymentMode", drpPmtMode.SelectedValue.ToString());
        //            SqlParameter sqlParameter8 = new SqlParameter();
        //            SqlParameter sqlParameter9 = new SqlParameter();
        //            SqlParameter sqlParameter10 = new SqlParameter();
        //            SqlParameter sqlParameter11 = new SqlParameter();
        //            if (drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
        //            {
        //                sqlParameter8 = new SqlParameter("@ChequeNo", txtChqNo.Text.Trim());
        //                sqlParameter9 = new SqlParameter("@ChequeDate", txtChqDate.Text.ToString());
        //                sqlParameter10 = new SqlParameter("@DrawanOnBank", txtBank.Text);
        //                sqlParameter11 = new SqlParameter("@BankAcHeadId", int.Parse(drpBankAc.SelectedValue.ToString()));
        //            }
        //            SqlParameter sqlParameter12 = new SqlParameter("@UserID", Convert.ToInt32(Session["User_Id"].ToString().Trim()));
        //            SqlParameter sqlParameter13 = new SqlParameter("@SchoolID", Session["SchoolId"].ToString());
        //            if (Request.QueryString["rno"] != null)
        //                sqlCommand.Parameters.Add(sqlParameter1);
        //            sqlCommand.Parameters.Add(sqlParameter2);
        //            sqlCommand.Parameters.Add(sqlParameter3);
        //            sqlCommand.Parameters.Add(sqlParameter4);
        //            sqlCommand.Parameters.Add(sqlParameter5);
        //            sqlCommand.Parameters.Add(sqlParameter6);
        //            sqlCommand.Parameters.Add(sqlParameter7);
        //            if (drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
        //            {
        //                sqlCommand.Parameters.Add(sqlParameter8);
        //                sqlCommand.Parameters.Add(sqlParameter9);
        //                sqlCommand.Parameters.Add(sqlParameter10);
        //                sqlCommand.Parameters.Add(sqlParameter11);
        //            }
        //            sqlCommand.Parameters.Add(sqlParameter12);
        //            sqlCommand.Parameters.Add(sqlParameter13);
        //            new SqlDataAdapter()
        //            {
        //                SelectCommand = sqlCommand
        //            }.Fill(dataTable1);
        //            ViewState["FeeTable"] = dataTable1;
        //        }
        //        if (dataTable1.Rows.Count > 0 && dataTable1.Rows[0][0].ToString().Trim() != "DUP" && dataTable1.Rows[0][0].ToString().Trim() != "NO")
        //        {
        //            Rcptno = dataTable1.Rows[0]["ReceiptNo"].ToString();
        //            ViewState["RcptNo"] = Rcptno;
        //            Session["rcptFee"] = Rcptno;
        //        }
        //        else if (dataTable1.Rows.Count > 0 && (dataTable1.Rows[0][0].ToString().Trim() == "DUP" || dataTable1.Rows[0][0].ToString().Trim() == "NO"))
        //            ViewState["Status"] = dataTable1.Rows[0][0].ToString().Trim();
        //        sqlCommand.Parameters.Clear();
        //        sqlCommand.CommandType = CommandType.Text;
        //        sqlCommand.CommandText = "select  AdditinalFeeCredit from PS_StudCreditLedger where admnno=" + txtadmnno.Text.Trim();
        //        Convert.ToDouble(sqlCommand.ExecuteScalar());
        //        StringBuilder stringBuilder = new StringBuilder();
        //        sqlCommand.Parameters.Clear();
        //        if (rBtnCurrentSess.Checked)
        //        {
        //            stringBuilder.Append("select f.TransNo,f.TransNoParent,f.TransDate,f.AdmnNo,f.TransDesc,f.SessionYr,f.Debit,f.Credit,f.Balance from PS_FeeLedgerBooks f ");
        //            stringBuilder.Append(" where f.admnno='" + txtadmnno.Text.Trim() + "' and f.balance <> 0 and f.sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' order by f.TransDate asc");
        //        }
        //        else
        //        {
        //            string str = "04/01/" + drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        //            stringBuilder.Append("Select f.TransNo,f.TransNoParent,f.TransDate,f.AdmnNo,f.TransDesc,f.SessionYr,f.Debit,f.Credit,f.Balance from PS_FeeLedgerBooks f ");
        //            stringBuilder.Append(" where f.admnno='" + txtadmnno.Text.Trim() + "' and f.balance <> 0 and f.TransDate < '" + str + "' order by f.TransDate asc");
        //        }
        //        sqlCommand.CommandType = CommandType.Text;
        //        sqlCommand.CommandText = stringBuilder.ToString().Trim();
        //        DataTable dataTable2 = new DataTable();
        //        new SqlDataAdapter() { SelectCommand = sqlCommand }.Fill(dataTable2);
        //        if (dataTable2.Rows.Count <= 0 || !(Rcptno != ""))
        //            return;
        //        insertBookledger(dataTable2, Conn, transaction);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ViewState["Status"] = "NO";
        //}
    }

    private void insertBookledger(DataTable tb, SqlConnection Conn, SqlTransaction transaction)
    {
        //try
        //{
        //    Decimal result1 = new Decimal(0);
        //    Decimal.TryParse(txtBookFee.Text, out result1);
        //    string msg = "";
        //    foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
        //    {
        //        Decimal result2 = new Decimal(0);
        //        Decimal num = new Decimal(0);
        //        Decimal.TryParse(Convert.ToString(row["Balance"]), out result2);
        //        Decimal CrAmt;
        //        if (result1 > result2)
        //        {
        //            row["balance"] = 0;
        //            result1 -= result2;
        //            CrAmt = result2;
        //        }
        //        else
        //        {
        //            row["balance"] = (result2 - result1);
        //            CrAmt = result1;
        //            result1 = new Decimal(0);
        //        }
        //        tb.AcceptChanges();
        //        InsertBookinLedger(row, CrAmt, Conn, transaction, out msg);
        //        if (msg.ToUpper().Trim() != "S")
        //        {
        //            ViewState["Status"] = "NO";
        //            break;
        //        }
        //        if (result1 <= new Decimal(0))
        //            break;
        //    }
        //    if (ViewState["Status"].ToString().Trim() != "NO")
        //        updatefeeledgerBooks(tb, 0, Conn, transaction);
        //    if (result1 > new Decimal(0))
        //        Session["CreditAmount"] = string.Format("{0:F2}", result1);
        //    else
        //        Session["CreditAmount"] = string.Format("{0:F2}", new Decimal(0));
        //}
        //catch (Exception ex)
        //{
        //    ViewState["Status"] = "NO";
        //}
    }

    private void InsertBookinLedger(DataRow dr, Decimal CrAmt, SqlConnection Conn, SqlTransaction transaction, out string msg)
    {
        try
        {
            msg = "";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = Conn;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Transaction = transaction;
            sqlCommand.CommandText = "ps_sp_insert_FeeLedgerBooks";
            sqlCommand.Parameters.Clear();
            SqlParameter sqlParameter1 = new SqlParameter("@date", Convert.ToDateTime(dr["TransDate"]));
            SqlParameter sqlParameter2 = new SqlParameter("@TransDate", dtpFeeDt.GetDateValue().ToString("dd MMM yyyy"));
            SqlParameter sqlParameter3 = new SqlParameter("@AdmnNo", Convert.ToInt64(dr["AdmnNo"]));
            SqlParameter sqlParameter4 = new SqlParameter("@TransDesc", Convert.ToString(dr["TransDesc"]));
            SqlParameter sqlParameter5 = new SqlParameter("@TransNoParent", Convert.ToInt64(dr["TransNo"]));
            SqlParameter sqlParameter6 = new SqlParameter("@debit", "0");
            SqlParameter sqlParameter7 = new SqlParameter("@credit", CrAmt);
            SqlParameter sqlParameter8 = new SqlParameter("@balance", "0");
            SqlParameter sqlParameter9 = new SqlParameter("@Receipt_VrNo", Rcptno);
            SqlParameter sqlParameter10 = new SqlParameter("@userid", Convert.ToInt32(Session["User_Id"].ToString()));
            SqlParameter sqlParameter11 = new SqlParameter("@UserDate", Convert.ToDateTime(DateTime.Now.ToString("dd MMM yyyy"), (IFormatProvider)CultureInfo.InvariantCulture));
            SqlParameter sqlParameter12 = new SqlParameter("@schoolid", Session["SchoolId"].ToString());
            SqlParameter sqlParameter13 = new SqlParameter("@PayMode", drpPmtMode.SelectedValue.ToString().Trim());
            SqlParameter sqlParameter14 = new SqlParameter("@Session", dr["SessionYr"].ToString());
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlCommand.Parameters.Add(sqlParameter2);
            sqlCommand.Parameters.Add(sqlParameter3);
            sqlCommand.Parameters.Add(sqlParameter4);
            sqlCommand.Parameters.Add(sqlParameter5);
            sqlCommand.Parameters.Add(sqlParameter6);
            sqlCommand.Parameters.Add(sqlParameter7);
            sqlCommand.Parameters.Add(sqlParameter8);
            sqlCommand.Parameters.Add(sqlParameter9);
            sqlCommand.Parameters.Add(sqlParameter10);
            sqlCommand.Parameters.Add(sqlParameter11);
            sqlCommand.Parameters.Add(sqlParameter12);
            sqlCommand.Parameters.Add(sqlParameter13);
            sqlCommand.Parameters.Add(sqlParameter14);
            msg = sqlCommand.ExecuteScalar().ToString().Trim();
        }
        catch (Exception ex)
        {
            ViewState["Status"] = "NO";
            msg = "";
        }
    }

    private void InsertCrAmtInLedger(DataRow dr, Decimal CrAmt, SqlConnection Conn, SqlTransaction transaction, out string msg)
    {
        try
        {
            msg = "";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = Conn;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Transaction = transaction;
            sqlCommand.CommandText = "ps_sp_insert_FeeLedgerNew_Kems";
            sqlCommand.Parameters.Clear();
            SqlParameter sqlParameter1 = new SqlParameter("@date", Convert.ToDateTime(dr["TransDate"]));
            SqlParameter sqlParameter2 = new SqlParameter("@TransDate", dtpFeeDt.GetDateValue().ToString("dd MMM yyyy"));
            SqlParameter sqlParameter3 = new SqlParameter("@AdmnNo", Convert.ToInt64(dr["AdmnNo"]));
            SqlParameter sqlParameter4 = new SqlParameter("@TransDesc", Convert.ToString(dr["TransDesc"]));
            SqlParameter sqlParameter5 = new SqlParameter("@debit", "0");
            SqlParameter sqlParameter6 = new SqlParameter("@credit", CrAmt);
            SqlParameter sqlParameter7 = new SqlParameter("@balance", "0");
            SqlParameter sqlParameter8 = new SqlParameter("@Receipt_VrNo", Rcptno);
            SqlParameter sqlParameter9 = new SqlParameter("@userid", Convert.ToInt32(Session["User_Id"].ToString()));
            SqlParameter sqlParameter10 = new SqlParameter("@UserDate", Convert.ToDateTime(DateTime.Now.ToString("dd MMM yyyy"), (IFormatProvider)CultureInfo.InvariantCulture));
            SqlParameter sqlParameter11 = new SqlParameter("@schoolid", Session["SchoolId"].ToString());
            SqlParameter sqlParameter12 = new SqlParameter("@FeeId", Convert.ToString(dr["FeeId"]));
            SqlParameter sqlParameter13 = new SqlParameter("@PayMode", drpPmtMode.SelectedValue.ToString().Trim());
            SqlParameter sqlParameter14 = new SqlParameter("@Session", dr["SessionYr"].ToString());
            SqlParameter sqlParameter15 = new SqlParameter("@AccountHeadDr", Convert.ToInt32(ViewState["DrAcHead"].ToString()));
            SqlParameter sqlParameter16 = new SqlParameter("@PR_Id", Convert.ToInt64(dr["PR_Id"]));
            sqlCommand.Parameters.Add(sqlParameter1);
            sqlCommand.Parameters.Add(sqlParameter2);
            sqlCommand.Parameters.Add(sqlParameter3);
            sqlCommand.Parameters.Add(sqlParameter4);
            sqlCommand.Parameters.Add(sqlParameter5);
            sqlCommand.Parameters.Add(sqlParameter6);
            sqlCommand.Parameters.Add(sqlParameter7);
            sqlCommand.Parameters.Add(sqlParameter8);
            sqlCommand.Parameters.Add(sqlParameter9);
            sqlCommand.Parameters.Add(sqlParameter10);
            sqlCommand.Parameters.Add(sqlParameter11);
            sqlCommand.Parameters.Add(sqlParameter12);
            sqlCommand.Parameters.Add(sqlParameter13);
            sqlCommand.Parameters.Add(sqlParameter14);
            sqlCommand.Parameters.Add(sqlParameter15);
            sqlCommand.Parameters.Add(sqlParameter16);
            msg = sqlCommand.ExecuteScalar().ToString().Trim();
        }
        catch (Exception ex)
        {
            ViewState["Status"] = "NO";
            msg = "";
        }
    }

    private void insertfeeledger(DataTable tb, Decimal CrAmt, SqlConnection Conn, SqlTransaction transaction)
    {
        try
        {
            Decimal result1 = new Decimal(0);
            Decimal.TryParse(txtamt.Text, out result1);
            Decimal num1 = result1 + CrAmt;
            string msg = "";
            foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
            {
                Decimal result2 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                Decimal.TryParse(Convert.ToString(row["Balance"]), out result2);
                Decimal CrAmt1;
                if (num1 > result2)
                {
                    row["credit"] = row["Debit"];
                    row["balance"] = 0;
                    num1 -= result2;
                    CrAmt1 = result2;
                }
                else
                {
                    row["credit"] = Convert.ToDecimal(row["credit"]) + num1;
                    row["balance"] = (result2 - num1);
                    CrAmt1 = num1;
                    num1 = new Decimal(0);
                    feecreditinsert(row);
                }
                tb.AcceptChanges();
                InsertCrAmtInLedger(row, CrAmt1, Conn, transaction, out msg);
                if (msg.ToUpper().Trim() != "S")
                {
                    ViewState["Status"] = "NO";
                    break;
                }
                if (num1 <= new Decimal(0))
                    break;
            }
            if (ViewState["Status"].ToString().Trim() != "NO")
                updatefeeledger(tb, 0, Conn, transaction);
            if (num1 > new Decimal(0))
                Session["CreditAmount"] = string.Format("{0:F2}", num1);
            else
                Session["CreditAmount"] = string.Format("{0:F2}", new Decimal(0));
        }
        catch (Exception ex)
        {
            ViewState["Status"] = "NO";
        }
    }
    private void feecreditinsert(DataRow dr)
    {
        Hashtable ht = new Hashtable();
        ht.Add("TransDate", dr["TransDate"]);
        ht.Add("AdmnNo", dr["AdmnNo"]);
        ht.Add("TransDesc", dr["FeeName"]);
        ht.Add("Debit", dr["Balance"]);
        ht.Add("Credit", 0.00);
        ht.Add("Balance", dr["Balance"]);
        ht.Add("Receipt_VrNo", "");
        ht.Add("FeeId", dr["FeeId"]);
        ht.Add("Session", dr["SessionYr"]);
        ht.Add("@UserID", Convert.ToInt32(Session["User_Id"]));
        ht.Add("@UserDate", DateTime.Today);
        ht.Add("@schoolid", 7011);
        ht.Add("@PayMode", "");
        string str = obj.ExecuteScalar("ps_sp_insert_FeeLedger", ht);
    }
    private void insertBusledger(DataTable tb, SqlConnection Conn, SqlTransaction transaction)
    {
        try
        {
            Decimal result1 = new Decimal(0);
            Decimal.TryParse(txtBusFee.Text, out result1);
            foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
            {
                Decimal result2 = new Decimal(0);
                Decimal num1 = new Decimal(0);
                Decimal.TryParse(Convert.ToString(row["Balance"]), out result2);
                if (result1 > result2)
                {
                    row["balance"] = 0;
                    result1 -= result2;
                    Decimal num2 = result2;
                    row["Credit"] = num2;
                }
                else
                {
                    row["balance"] = (result2 - result1);
                    Decimal num2 = result1;
                    row["Credit"] = num2;
                    result1 = new Decimal(0);
                }
                tb.AcceptChanges();
                if (result1 <= new Decimal(0))
                    break;
            }
            InsertBusFeeLedger(tb, Conn, transaction);
            if (result1 > new Decimal(0))
                Session["CreditAmount"] = string.Format("{0:F2}", result1);
            else
                Session["CreditAmount"] = string.Format("{0:F2}", new Decimal(0));
        }
        catch (Exception ex)
        {
            ViewState["Status"] = "NO";
        }
    }

    private void InsertBusFeeLedger(DataTable tb, SqlConnection Conn, SqlTransaction transaction)
    {
        try
        {
            string str1 = "";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = Conn;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Transaction = transaction;
            sqlCommand.CommandText = "ps_sp_updateCredit_AdFeeLedger";
            SqlParameter sqlParameter1 = new SqlParameter();
            SqlParameter sqlParameter2 = new SqlParameter();
            SqlParameter sqlParameter3 = new SqlParameter();
            SqlParameter sqlParameter4 = new SqlParameter();
            SqlParameter sqlParameter5 = new SqlParameter();
            SqlParameter sqlParameter6 = new SqlParameter();
            SqlParameter sqlParameter7 = new SqlParameter();
            SqlParameter sqlParameter8 = new SqlParameter();
            SqlParameter sqlParameter9 = new SqlParameter();
            string str2 = tb.Rows[0]["AdFeeNo"].ToString().Trim();
            foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
            {
                sqlCommand.Parameters.Clear();
                double num = Convert.ToDouble(row["credit"].ToString().Trim());
                sqlParameter1.ParameterName = "@adfeeno";
                sqlParameter1.Value = row["AdFeeNo"].ToString().Trim();
                sqlParameter3.ParameterName = "@adfeedesc";
                sqlParameter3.Value = "Bus Fee";
                sqlParameter5.ParameterName = "@credit";
                sqlParameter5.Value = num;
                sqlParameter2.ParameterName = "@balance";
                sqlParameter2.Value = row["Balance"];
                sqlParameter6.ParameterName = "@receipt";
                sqlParameter6.Value = Rcptno;
                sqlParameter7.ParameterName = "@UserID";
                sqlParameter7.Value = Session["User_Id"].ToString();
                sqlCommand.Parameters.Add(sqlParameter1);
                sqlCommand.Parameters.Add(sqlParameter2);
                sqlCommand.Parameters.Add(sqlParameter3);
                sqlCommand.Parameters.Add(sqlParameter5);
                sqlCommand.Parameters.Add(sqlParameter6);
                sqlCommand.Parameters.Add(sqlParameter7);
                str1 = sqlCommand.ExecuteScalar().ToString().Trim();
                if (str1.Trim() != "S")
                    break;
            }
            if (str1.Trim() == "S")
            {
                sqlCommand.Parameters.Clear();
                sqlCommand.CommandText = "ps_sp_insert_GenLedger";
                sqlParameter1.ParameterName = "@TransDate";
                sqlParameter1.Value = dtpFeeDt.GetDateValue().ToString("dd MMM yyyy");
                sqlParameter2.ParameterName = "@Receipt_VrNo";
                sqlParameter2.Value = Rcptno;
                sqlParameter3.ParameterName = "@AccountHeadDr";
                sqlParameter3.Value = ViewState["DrAcHead"].ToString();
                sqlParameter4.ParameterName = "@TransDesc";
                sqlParameter4.Value = "Bus Fee";
                sqlParameter5.ParameterName = "@Amount";
                sqlParameter5.Value = float.Parse(txtBusFee.Text.Trim());
                sqlParameter6.ParameterName = "@UserID";
                sqlParameter6.Value = Convert.ToInt32(Session["User_Id"].ToString().Trim());
                sqlParameter7.ParameterName = "@schoolid";
                sqlParameter7.Value = Session["SchoolId"].ToString();
                sqlParameter8.ParameterName = "@AdFeeNo";
                sqlParameter8.Value = str2;
                sqlCommand.Parameters.Add(sqlParameter1);
                sqlCommand.Parameters.Add(sqlParameter2);
                sqlCommand.Parameters.Add(sqlParameter3);
                sqlCommand.Parameters.Add(sqlParameter4);
                sqlCommand.Parameters.Add(sqlParameter5);
                sqlCommand.Parameters.Add(sqlParameter6);
                sqlCommand.Parameters.Add(sqlParameter7);
                sqlCommand.Parameters.Add(sqlParameter8);
                if (!(sqlCommand.ExecuteScalar().ToString().Trim().Trim() != ""))
                    return;
                ViewState["Status"] = "NO";
            }
            else
                ViewState["Status"] = "NO";
        }
        catch (Exception ex)
        {
            ViewState["Status"] = "NO";
        }
    }

    private void UpdateCreditAmount(Decimal CrAmt)
    {
        int int32 = Convert.ToInt32(Session["User_Id"].ToString());
        string str = "update PS_StudCreditLedger set FeeCredit= " + CrAmt + ",UserId=" + int32 + ", UserDate='" + dtpFeeDt.GetDateValue().ToString("dd MMM yyyy") + "' where admnno=" + txtadmnno.Text.Trim();
        obj = new clsDAL();
        obj.ExcuteQryInsUpdt(str);
    }

    private void updatefeeledger(DataTable tbcredit, int mode, SqlConnection Conn, SqlTransaction transaction)
    {
        try
        {
            string str = "";
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = Conn;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Transaction = transaction;
            sqlCommand.CommandText = "ps_sp_update_feeledger";
            SqlParameter sqlParameter1 = new SqlParameter();
            SqlParameter sqlParameter2 = new SqlParameter();
            SqlParameter sqlParameter3 = new SqlParameter();
            SqlParameter sqlParameter4 = new SqlParameter();
            SqlParameter sqlParameter5 = new SqlParameter();
            SqlParameter sqlParameter6 = new SqlParameter();
            SqlParameter sqlParameter7 = new SqlParameter();
            SqlParameter sqlParameter8 = new SqlParameter();
            SqlParameter sqlParameter9 = new SqlParameter();
            SqlParameter sqlParameter10 = new SqlParameter();
            foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
            {
                if (row["debit"].ToString() != row["balance"].ToString())
                {
                    sqlCommand.Parameters.Clear();
                    sqlParameter2.ParameterName = "@TransNo";
                    sqlParameter2.Value = row["TransNo"];
                    sqlParameter3.ParameterName = "@AdmnNo";
                    sqlParameter3.Value = row["AdmnNo"];
                    sqlParameter4.ParameterName = "@debit";
                    sqlParameter4.Value = row["debit"];
                    sqlParameter5.ParameterName = "@credit";
                    sqlParameter5.Value = row["credit"];
                    sqlParameter1.ParameterName = "@balance";
                    sqlParameter1.Value = row["balance"];
                    sqlParameter6.ParameterName = "@Receipt_VrNo";
                    sqlParameter6.Value = row["ReceiptNo"];
                    sqlParameter7.ParameterName = "@UserID";
                    sqlParameter7.Value = Session["User_Id"].ToString();
                    sqlParameter8.ParameterName = "@UserDate";
                    sqlParameter8.Value = DateTime.Now.ToString("dd MMM yyyy");
                    sqlParameter9.ParameterName = "@SchoolID";
                    sqlParameter9.Value = Session["SchoolId"].ToString();
                    sqlParameter10.ParameterName = "@mode";
                    sqlParameter10.Value = drpPmtMode.SelectedValue.ToString().Trim();
                    sqlCommand.Parameters.Add(sqlParameter2);
                    sqlCommand.Parameters.Add(sqlParameter3);
                    sqlCommand.Parameters.Add(sqlParameter4);
                    sqlCommand.Parameters.Add(sqlParameter5);
                    sqlCommand.Parameters.Add(sqlParameter6);
                    sqlCommand.Parameters.Add(sqlParameter7);
                    sqlCommand.Parameters.Add(sqlParameter8);
                    sqlCommand.Parameters.Add(sqlParameter9);
                    sqlCommand.Parameters.Add(sqlParameter10);
                    sqlCommand.Parameters.Add(sqlParameter1);
                    str = sqlCommand.ExecuteScalar().ToString().Trim();
                    if (str.Trim().ToUpper() != "S")
                        break;
                }
            }
            if (!(str.Trim().ToUpper() != "S"))
                return;
            ViewState["Status"] = "NO";
        }
        catch (Exception ex)
        {
            ViewState["Status"] = "NO";
        }
    }

    private void updatefeeledgerBooks(DataTable tbcredit, int mode, SqlConnection Conn, SqlTransaction transaction)
    {
        //try
        //{
        //    string str = "";
        //    SqlCommand sqlCommand = new SqlCommand();
        //    sqlCommand.Connection = Conn;
        //    sqlCommand.CommandType = CommandType.StoredProcedure;
        //    sqlCommand.Transaction = transaction;
        //    sqlCommand.CommandText = "ps_sp_update_feeledgerBooks";
        //    SqlParameter sqlParameter1 = new SqlParameter();
        //    SqlParameter sqlParameter2 = new SqlParameter();
        //    SqlParameter sqlParameter3 = new SqlParameter();
        //    SqlParameter sqlParameter4 = new SqlParameter();
        //    SqlParameter sqlParameter5 = new SqlParameter();
        //    SqlParameter sqlParameter6 = new SqlParameter();
        //    SqlParameter sqlParameter7 = new SqlParameter();
        //    SqlParameter sqlParameter8 = new SqlParameter();
        //    SqlParameter sqlParameter9 = new SqlParameter();
        //    SqlParameter sqlParameter10 = new SqlParameter();
        //    foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
        //    {
        //        if (row["debit"].ToString() != row["balance"].ToString())
        //        {
        //            sqlCommand.Parameters.Clear();
        //            sqlParameter2.ParameterName = "@TransNo";
        //            sqlParameter2.Value = row["TransNo"];
        //            sqlParameter3.ParameterName = "@AdmnNo";
        //            sqlParameter3.Value = row["AdmnNo"];
        //            sqlParameter4.ParameterName = "@debit";
        //            sqlParameter4.Value = row["debit"];
        //            sqlParameter5.ParameterName = "@credit";
        //            sqlParameter5.Value = row["credit"];
        //            sqlParameter1.ParameterName = "@balance";
        //            sqlParameter1.Value = row["balance"];
        //            sqlParameter6.ParameterName = "@Receipt_VrNo";
        //            sqlParameter6.Value = Rcptno;
        //            sqlParameter7.ParameterName = "@UserID";
        //            sqlParameter7.Value = Session["User_Id"].ToString();
        //            sqlParameter8.ParameterName = "@UserDate";
        //            sqlParameter8.Value = DateTime.Now.ToString("dd MMM yyyy");
        //            sqlParameter9.ParameterName = "@SchoolID";
        //            sqlParameter9.Value = Session["SchoolId"].ToString();
        //            sqlParameter10.ParameterName = "@mode";
        //            sqlParameter10.Value = drpPmtMode.SelectedValue.ToString().Trim();
        //            sqlCommand.Parameters.Add(sqlParameter2);
        //            sqlCommand.Parameters.Add(sqlParameter3);
        //            sqlCommand.Parameters.Add(sqlParameter4);
        //            sqlCommand.Parameters.Add(sqlParameter5);
        //            sqlCommand.Parameters.Add(sqlParameter6);
        //            sqlCommand.Parameters.Add(sqlParameter7);
        //            sqlCommand.Parameters.Add(sqlParameter8);
        //            sqlCommand.Parameters.Add(sqlParameter9);
        //            sqlCommand.Parameters.Add(sqlParameter10);
        //            sqlCommand.Parameters.Add(sqlParameter1);
        //            str = sqlCommand.ExecuteScalar().ToString().Trim();
        //            if (str.Trim().ToUpper() != "S")
        //                break;
        //        }
        //    }
        //    if (str.Trim().ToUpper() != "S")
        //        ViewState["Status"] = "NO";
        //    if (str.Trim() == "S")
        //    {
        //        sqlCommand.Parameters.Clear();
        //        sqlCommand.CommandText = "ps_sp_insert_GenLedgerBooks";
        //        sqlParameter1.ParameterName = "@TransDate";
        //        sqlParameter1.Value = dtpFeeDt.GetDateValue().ToString("dd MMM yyyy");
        //        sqlParameter2.ParameterName = "@Receipt_VrNo";
        //        sqlParameter2.Value = Rcptno;
        //        sqlParameter3.ParameterName = "@AccountHeadDr";
        //        sqlParameter3.Value = ViewState["DrAcHead"].ToString();
        //        sqlParameter4.ParameterName = "@TransDesc";
        //        sqlParameter4.Value = "Book Fee";
        //        sqlParameter5.ParameterName = "@Amount";
        //        sqlParameter5.Value = float.Parse(txtBookFee.Text.Trim());
        //        sqlParameter6.ParameterName = "@UserID";
        //        sqlParameter6.Value = Convert.ToInt32(Session["User_Id"].ToString().Trim());
        //        sqlParameter7.ParameterName = "@schoolid";
        //        sqlParameter7.Value = Session["SchoolId"].ToString();
        //        sqlCommand.Parameters.Add(sqlParameter1);
        //        sqlCommand.Parameters.Add(sqlParameter2);
        //        sqlCommand.Parameters.Add(sqlParameter3);
        //        sqlCommand.Parameters.Add(sqlParameter4);
        //        sqlCommand.Parameters.Add(sqlParameter5);
        //        sqlCommand.Parameters.Add(sqlParameter6);
        //        sqlCommand.Parameters.Add(sqlParameter7);
        //        if (!(sqlCommand.ExecuteScalar().ToString().Trim().Trim() != ""))
        //            return;
        //        ViewState["Status"] = "NO";
        //    }
        //    else
        //        ViewState["Status"] = "NO";
        //}
        //catch (Exception ex)
        //{
        //    ViewState["Status"] = "NO";
        //}
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
         "FeeId",
         Convert.ToInt64(dr["FeeId"])
      },
      {
         "SessionYr",
         dr["SessionYr"].ToString().Trim()
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

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("feercptcashdetail.aspx");
    }

    private void clear()
    {
        txtamt.Text = "";
        dtpFeeDt.SetDateValue(DateTime.Today);
        txtdesc.Text = "";
        ViewState["OldDue"] = "0";
        ViewState["PrevDue"] = "0";
        ViewState["OldhostDue"] = "0";
        ViewState["hostPrevDue"] = "0";
    }

    private void DisableControls()
    {
        drpSession.Enabled = false;
        drpclass.Enabled = false;
        drpstudent.Enabled = false;
        txtadmnno.ReadOnly = true;
    }

    private bool IsBalance()
    {
        return double.Parse(new clsDAL().ExecuteScalarQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(balance),0)) as balance FROM ps_feeledger WHERE admnno='" + drpstudent.SelectedValue.ToString().Trim() + "' and Transdate <=getdate() ")) > 0.0;
    }

    protected void drpPmtMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpPmtMode.SelectedValue == "Cash")
        {
            txtBank.Enabled = false;
            txtChqDate.Enabled = false;
            txtChqNo.Enabled = false;
            txtChqDate.Text = string.Empty;
            txtChqNo.Text = string.Empty;
            txtBank.Text = string.Empty;
            drpBankAc.Visible = false;
        }
        else
        {
            txtBank.Enabled = true;
            txtChqDate.Enabled = true;
            txtChqNo.Enabled = true;
            drpBankAc.Visible = true;
        }
    }

    private void FillSelStudent()
    {
        drpSession.SelectedValue = Request.QueryString["sess"].ToString();
        if (Request.QueryString["cid"].ToString() != "0")
        {
            drpclass.SelectedValue = Request.QueryString["cid"].ToString();
        }
        else
        {
            obj = new clsDAL();
            drpclass.SelectedValue = obj.GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent where AdmnNo=" + Request.QueryString["admnno"].ToString() + " and Detained_Promoted=''").Rows[0]["ClassID"].ToString();
        }
        fillstudent();
        drpstudent.SelectedValue = Request.QueryString["admnno"].ToString().Trim();
        GetStudImg();
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpstudent.Items.Clear();
        drpclass.SelectedIndex = 0;
        txtadmnno.Text = string.Empty;
        lblTotalFee.Text = "0.00";
        lblbalance.Text = "0.00";
        lblBusFee.Text = "0.00";
        //lblBookFee.Text = "0.00";
        txtamt.Text = string.Empty;
        txtBusFee.Text = string.Empty;
        //txtBookFee.Text = string.Empty;
        txtHostelFee.Text = string.Empty;
        txtRcevMiscFee.Text = string.Empty;
        txtdesc.Text = string.Empty;
        setMiscDate();
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        lblMsg2.Text = "";
        Session["BusFeeMonth"] = null;
        Session["HostelFeeMonth"] = null;
        Session["BookFeeMonth"] = null;
        ViewState["FY"] = "n";
        ViewState["BFY"] = "n";
        ViewState["HFY"] = "n";
        ViewState["BOOK"] = "n";
        txtadmnno.Text = drpstudent.SelectedValue;
        GetAllFeeDetails();
        GetStudFather();
        UpdatePanel1.Update();
        rbAll.Checked = true;
        rbMonthly.Checked = false;
        rbOnAdmission.Checked = false;
        btnFeeFullYr.Enabled = true;
    }

    private void GetStudFather()
    {
        lblStudDet.Text = new clsDAL().ExecuteScalarQry("select 'Father Name: ' + FatherName + ' (Mob No-' + TelNoResidence + ')' as StudDet from dbo.PS_StudMaster where admnno=" + drpstudent.SelectedValue.ToString());
        lblStudDet.ForeColor = Color.Green;
        lblStudDet.Font.Bold = true;
    }

    protected void GetAllFeeDetails()
    {
        FillFeeMonth(drpMonthPayble);
        FillMiscFeeNames(drpMiscFee);
        FillPaybleMonth();
        FillMonth(drpBusPayble);
        //FillMonth(drpBookPayble);
        FillMonth(drpHosPayble);
        FillFeeAmt();
        GetStudImg();
    }

    protected void btnDetail_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtadmnno.Text.ToString() != "")
                drpstudent.SelectedValue = txtadmnno.Text.ToString();
        }
        catch { }
        lblMsg.Text = "";
        lblMsg2.Text = "";
        Session["BusFeeMonth"] = null;
        Session["HostelFeeMonth"] = null;
        Session["BookFeeMonth"] = null;
        ViewState["FY"] = "n";
        ViewState["BFY"] = "n";
        ViewState["BOOK"] = "n";
        ViewState["HFY"] = "n";
        try
        {
            Session["CreditAmount"] = "0";
            obj = new clsDAL();
            //DataTable dataTableQry = obj.GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent cs inner join PS_StudMaster sm on sm.AdmnNo=cs.AdmnNo where cs.AdmnNo=" + txtadmnno.Text.Trim() + " and Detained_Promoted='' and Status=1 ");
            DataTable dataTableQry = obj.GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent cs inner join PS_StudMaster sm on sm.AdmnNo=cs.AdmnNo where cs.AdmnNo=" + txtadmnno.Text.Trim() + " and Status=1 ");
            if (dataTableQry.Rows.Count > 0)
            {
                drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
                drpclass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
                fillstudent();
                drpstudent.SelectedValue = txtadmnno.Text.Trim();
                ViewState["FY"] = "n";
                GetAllFeeDetails();
                GetStudFather();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtadmnno, txtadmnno.GetType(), "ShowMessage", "alert('Admission number does not exists')", true);
                return;
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
        setMiscDate();
    }

    protected void FillFeeAmt()
    {
        CalculateTotPayble();
    }

    private void CalculateTotPayble()
    {
        ViewState["OldDue"] = "0";
        ViewState["PrevDue"] = "0";
        getbalance(txtadmnno.Text.Trim());
        getMiscFee(txtadmnno.Text.Trim());
        getBusFee(txtadmnno.Text.Trim());
        getBookFee(txtadmnno.Text.Trim());
        getHosFee(txtadmnno.Text.Trim());
        txtamt.Text = (Convert.ToDouble(lblbalance.Text) - Convert.ToDouble(lblCreditAmt.Text)).ToString();
        txtMiscFee.Text = Convert.ToDouble(lblMiscFee.Text).ToString();
        TotalAmount();
        CalculatePrevDue(txtadmnno.Text.Trim());
    }

    private void CalculatePrevDue(string AdmnNo)
    {
        obj = new clsDAL();
        StringBuilder stringBuilder1 = new StringBuilder();
        string str1 = "04/01/" + drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        stringBuilder1.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
        stringBuilder1.Append(" from ps_feeledger where admnno='" + AdmnNo + "'  and TransDate < '" + str1 + "' AND TransDesc<>'Previous Balance'");
        string str2 = obj.ExecuteScalarQry(stringBuilder1.ToString().Trim());
        obj = new clsDAL();
        obj = new clsDAL();
        StringBuilder stringBuilder2 = new StringBuilder();
        stringBuilder2.Append("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger");
        stringBuilder2.Append(" inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId");
        stringBuilder2.Append(" where admnno='" + AdmnNo + "' and AdFeeDate < '" + str1 + "'");
        string str3 = obj.ExecuteScalarQry(stringBuilder2.ToString().Trim());
        lblPrevYrBal.Text = "Previous Session Due :-Rs. " + string.Format("{0:f2}", (Convert.ToDouble(str2) + Convert.ToDouble(str3)));
        StringBuilder stringBuilder3 = new StringBuilder();
        stringBuilder3.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
        stringBuilder3.Append(" from ps_feeledger where admnno='" + AdmnNo + "'  and TransDate < '" + str1 + "' AND TransDesc='Previous Balance'");
        string str4 = obj.ExecuteScalarQry(stringBuilder3.ToString().Trim());
        lblOldDues.Text = "Old Dues (Before Computerisation):- Rs. " + string.Format("{0:f2}", Convert.ToDouble(str4));
        if (Convert.ToDouble(str4) > 0.0)
        {
            btnRcvOldDue.Visible = true;
            ViewState["OldDue"] = str4;
        }
        else
        {
            ViewState["OldDue"] = "0";
            btnRcvOldDue.Visible = false;
        }

        StringBuilder stringBuilder4 = new StringBuilder();
        string str5 = "04/01/" + drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        stringBuilder4.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
        stringBuilder4.Append(" from Host_FeeLedger where admnno='" + AdmnNo + "'  and TransDate < '" + str5 + "' AND TransDesc<>'Previous Balance'");
        string str6 = obj.ExecuteScalarQry(stringBuilder4.ToString().Trim());
        lblhostPrevYrBal.Text = "Previous Session Host Due :- Rs. " + string.Format("{0:f2}", Convert.ToDouble(str6));
        StringBuilder stringBuilder5 = new StringBuilder();
        stringBuilder5.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
        stringBuilder5.Append(" from Host_FeeLedger where admnno='" + AdmnNo + "'  and TransDate < '" + str5 + "' AND TransDesc='Previous Balance'");
        string str7 = obj.ExecuteScalarQry(stringBuilder5.ToString().Trim());
        lbloldhostdue.Text = "Old Host Dues (Before Computerisation):- Rs. " + string.Format("{0:f2}", Convert.ToDouble(str7));
        if (Convert.ToDouble(str7) > 0.0)
        {
            btnoldhostreciv.Visible = true;
            ViewState["OldhostDue"] = str7;
        }
        else
        {
            ViewState["OldhostDue"] = "0";
            btnoldhostreciv.Visible = false;
        }
        ViewState["hostPrevDue"] = Convert.ToDouble(str6);

        ViewState["PrevDue"] = (Convert.ToDouble(str2) + Convert.ToDouble(str3));
    }

    private void getMiscFee(string AdmnNo)
    {
        obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        if (rBtnCurrentSess.Checked)
        {
            ViewState["Misc"] = "n";
            string currSession = new clsGenerateFee().CreateCurrSession();
            lblMiscFee.Text = obj.GetDataTable("Ps_Sp_GetStudMiscBalAmnt", new Hashtable()
      {
        {
           "@AdmnNo",
           AdmnNo
        },
        {
           "@SessionYr",
           drpSession.SelectedValue.ToString().Trim()
        },
        {
           "@PrevSession",
           currSession
        }
      }).Rows[0]["Balance"].ToString();
        }
        else
        {
            string str = "04/01/" + drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
            stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
            stringBuilder.Append(" from ps_feeledger FL INNER JOIN dbo.PS_FeeComponents FC ON FL.FeeId=FC.FeeID where PeriodicityID=6 AND admnno='" + AdmnNo + "'  and TransDate < '" + str + "' and TransDesc<>'Previous Balance'");
            lblMiscFee.Text = obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
        }
    }

    private void getbalance(string AdmnNo)
    {
        ViewState["FeeTilldate"] = null;
        Session["FeeTilldate"] = null;
        obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        if (rBtnCurrentSess.Checked)
        {
            ViewState["FY"] = "n";
            string currSession = new clsGenerateFee().CreateCurrSession();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@AdmnNo", drpstudent.SelectedValue.ToString().Trim());
            hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
            hashtable.Add("@PrevSession", currSession);
            string str;
            if (int.Parse(drpMonthPayble.SelectedValue.ToString()) == 1 || int.Parse(drpMonthPayble.SelectedValue.ToString()) == 2 || int.Parse(drpMonthPayble.SelectedValue.ToString()) == 3)
                str = "1-" + GetMonthStr(int.Parse(drpMonthPayble.SelectedValue.ToString()) + 1) + "-" + (int.Parse(drpSession.SelectedValue.ToString().Trim().Substring(0, 4)) + 1);
            else
                str = int.Parse(drpMonthPayble.SelectedValue.ToString()) != 12 ? "1-" + GetMonthStr(int.Parse(drpMonthPayble.SelectedValue.ToString()) + 1) + "-" + drpSession.SelectedValue.ToString().Trim().Substring(0, 4) : "01-01-" + (int.Parse(drpSession.SelectedValue.ToString().Trim().Substring(0, 4)) + 1);
            DateTime dateTime = Convert.ToDateTime(str);
            hashtable.Add("@FeeTillDt", dateTime.ToString("dd-MMM-yyyy"));
            ViewState["FeeTilldate"] = str;
            Session["FeeTilldate"] = str;
            DataTable dataTable = obj.GetDataTable("Ps_Sp_GetStudCurrentBalAmnt", hashtable);
            lblbalance.Text = dataTable.Rows[0]["Balance"].ToString();
            if (dataTable.Rows[0][1].ToString().Trim() == "Annual" && rbAll.Checked)
                txtdesc.Text = "Admission/Readmission & upto " + drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
            else if (dataTable.Rows[0][1].ToString().Trim() == "Annual" && rbMonthly.Checked)
                txtdesc.Text = "upto " + drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
            else if (dataTable.Rows[0][1].ToString().Trim() == "Same")
                txtdesc.Text = drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
            else if (dataTable.Rows[0][1].ToString().Trim() != "")
                txtdesc.Text = dataTable.Rows[0][1].ToString().Trim() + " to " + drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
            else
                txtdesc.Text = "";
        }
        else
        {
            string str = "04/01/" + drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
            stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
            stringBuilder.Append(" from ps_feeledger FL INNER JOIN dbo.PS_FeeComponents FC ON FL.FeeId=FC.FeeID where PeriodicityID<>6 AND admnno='" + AdmnNo + "'  and TransDate < '" + str + "' and TransDesc<>'Previous Balance'");
            lblbalance.Text = obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
            drpMonthPayble.SelectedIndex = drpMonthPayble.Items.Count - 1;
            txtdesc.Text = "Prev Session Due";
        }
        obj = new clsDAL();
        string str1 = obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(FeeCredit),0)) as FeeCredit from dbo.PS_StudCreditLedger where admnno=" + AdmnNo);
        lblCreditAmt.Text = str1;
        Session["CreditAmount"] = str1;
    }

    private string GetMonthStr(int MonthNo)
    {
        string str;
        switch (MonthNo)
        {
            case 0:
                str = "APR";
                break;
            case 1:
                str = "JAN";
                break;
            case 2:
                str = "FEB";
                break;
            case 3:
                str = "MAR";
                break;
            case 4:
                str = "APR";
                break;
            case 5:
                str = "MAY";
                break;
            case 6:
                str = "JUN";
                break;
            case 7:
                str = "JUL";
                break;
            case 8:
                str = "AUG";
                break;
            case 9:
                str = "SEP";
                break;
            case 10:
                str = "OCT";
                break;
            case 11:
                str = "NOV";
                break;
            case 12:
                str = "DEC";
                break;
            default:
                str = "APR";
                break;
        }
        return str;
    }

    private void GetBalFullSession(string AdmnNo)
    {
        if (rbOnAdmission.Checked)
        {
            try
            {
                drpMonthPayble.SelectedIndex = 1;
            }
            catch
            {
            }
        }
        else
        {
            try
            {
                drpMonthPayble.SelectedValue = "3";
            }
            catch
            {
            }
        }
        obj = new clsDAL();
        if (rBtnCurrentSess.Checked.Equals(true))
        {
            Hashtable hashtable = new Hashtable();
            DataTable dataTable1 = new DataTable();
            hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
            hashtable.Add("@AdmnNo", AdmnNo);
            hashtable.Add("@NxtFeeDt", ("1 Apr " + (Convert.ToInt32(drpSession.SelectedValue.Split('-')[0]) + 1).ToString()));
            DataTable dataTable2 = !rbMonthly.Checked ? obj.GetDataTable("ps_GetMonthWiseFee", hashtable) : obj.GetDataTable("ps_GetOnlyMonthWiseFee", hashtable);
            if (dataTable2.Rows.Count > 0)
            {
                lblbalance.Text = dataTable2.Rows[0][0].ToString().Trim();
                if (dataTable2.Rows[0][1].ToString().Trim() == "Annual" && rbAll.Checked)
                    txtdesc.Text = "Admission/Readmission & upto " + drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
                else if (dataTable2.Rows[0][1].ToString().Trim() == "Annual" && rbMonthly.Checked)
                    txtdesc.Text = "upto " + drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
                else if (dataTable2.Rows[0][1].ToString().Trim() == "Same")
                    txtdesc.Text = drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
                else if (dataTable2.Rows[0][1].ToString().Trim() != "")
                    txtdesc.Text = dataTable2.Rows[0][1].ToString().Trim() + " to " + drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
                else
                    txtdesc.Text = "";
            }
        }
        else
        {
            lblbalance.Text = obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance   from ps_feeledger FL INNER JOIN dbo.PS_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' AND PeriodicityID<>6 and admnno=" + AdmnNo);
            txtdesc.Text = "Prev Session Due";
        }
        string text = lblbalance.Text;
        obj = new clsDAL();
        string str = obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(FeeCredit),0)) as FeeCredit from dbo.PS_StudCreditLedger where admnno=" + AdmnNo);
        lblCreditAmt.Text = str;
        Session["CreditAmount"] = str;
    }

    private void getBusFee(string Admnno)
    {
        obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        if (rBtnCurrentSess.Checked)
        {
            string currSession = new clsGenerateFee().CreateCurrSession();
            obj = new clsDAL();
            if (obj.ExecuteScalarQry("SELECT TOP 1 SessionYr FROM dbo.PS_FeeNormsNew order by SessionID desc") != currSession)
            {
                obj = new clsDAL();
                if (drpBusPayble.SelectedIndex > 0)
                {
                    string str = Convert.ToInt32(drpBusPayble.SelectedValue.Trim()) < 1 || Convert.ToInt32(drpBusPayble.SelectedValue.Trim()) > 3 ? (!(drpBusPayble.SelectedValue.Trim().Trim() == "12") ? Convert.ToString(Convert.ToInt32(drpBusPayble.SelectedValue.Trim()) + 1) + "/1/" + drpSession.SelectedValue.Substring(0, 4).Trim() : "1/1/" + Convert.ToString(Convert.ToInt32(drpSession.SelectedValue.Substring(0, 4).Trim()) + 1)) : Convert.ToString(Convert.ToInt32(drpBusPayble.SelectedValue.Trim()) + 1) + "/1/" + Convert.ToString(Convert.ToInt32(drpSession.SelectedValue.Substring(0, 4).Trim()) + 1);
                    stringBuilder.Append("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger");
                    stringBuilder.Append(" inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId");
                    stringBuilder.Append(" where admnno='" + Admnno + "'  and sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "'  and Ad_Id=1 and AdFeeDate < '" + str + "'");
                }
                else
                {
                    string str = obj.ExecuteScalarQry("SELECT TOP 1 StartDate+1 AS StartDate FROM dbo.PS_FeeNormsNew order by SessionID desc");
                    stringBuilder.Append("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger");
                    stringBuilder.Append(" inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId");
                    stringBuilder.Append(" where admnno='" + Admnno + "'  and sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "'  and Ad_Id=1 and AdFeeDate <= '" + str + "'");
                }
            }
            else
            {
                stringBuilder.Append("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger");
                stringBuilder.Append(" inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId");
                stringBuilder.Append(" where admnno='" + Admnno + "'  and sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "'  and Ad_Id=1 and AdFeeDate < getdate()");
            }
        }
        else
        {
            string str = "04/01/" + drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
            stringBuilder.Append("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger");
            stringBuilder.Append(" inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId");
            stringBuilder.Append(" where admnno='" + Admnno + "'  and Ad_Id=1 and AdFeeDate < '" + str + "' ");
            drpBusPayble.SelectedIndex = drpBusPayble.Items.Count - 1;
        }
        lblBusFee.Text = obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
        txtBusFee.Text = lblBusFee.Text;
        ViewState["BFY"] = "n";
    }
    private void getHosFee(string Admnno)
    {
        //obj = new clsDAL();
        //StringBuilder stringBuilder = new StringBuilder();
        //if (rBtnCurrentSess.Checked)
        //{
        //    string currSession = new clsGenerateFee().CreateCurrSession();
        //    obj = new clsDAL();
        //    if (obj.ExecuteScalarQry("SELECT TOP 1 SessionYr FROM dbo.PS_FeeNormsNew order by SessionID desc") != currSession)
        //    {
        //        obj = new clsDAL();
        //        if (drpHosPayble.SelectedIndex > 0)
        //        {
        //            string str = Convert.ToInt32(drpHosPayble.SelectedValue.Trim()) < 1 || Convert.ToInt32(drpHosPayble.SelectedValue.Trim()) > 3 ? (!(drpHosPayble.SelectedValue.Trim().Trim() == "12") ? Convert.ToString(Convert.ToInt32(drpHosPayble.SelectedValue.Trim()) + 1) + "/1/" + drpSession.SelectedValue.Substring(0, 4).Trim() : "1/1/" + Convert.ToString(Convert.ToInt32(drpSession.SelectedValue.Substring(0, 4).Trim()) + 1)) : Convert.ToString(Convert.ToInt32(drpHosPayble.SelectedValue.Trim()) + 1) + "/1/" + Convert.ToString(Convert.ToInt32(drpSession.SelectedValue.Substring(0, 4).Trim()) + 1);
        //            stringBuilder.Append("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger");
        //            stringBuilder.Append(" inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId");
        //            stringBuilder.Append(" where admnno='" + Admnno + "'  and sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "'  and Ad_Id=2 and AdFeeDate < '" + str + "'");
        //        }
        //        else
        //        {
        //            string str = obj.ExecuteScalarQry("SELECT TOP 1 StartDate+1 AS StartDate FROM dbo.PS_FeeNormsNew order by SessionID desc");
        //            stringBuilder.Append("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger");
        //            stringBuilder.Append(" inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId");
        //            stringBuilder.Append(" where admnno='" + Admnno + "'  and sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "'  and Ad_Id=2 and AdFeeDate <= '" + str + "'");
        //        }
        //    }
        //    else
        //    {
        //        stringBuilder.Append("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger");
        //        stringBuilder.Append(" inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId");
        //        stringBuilder.Append(" where admnno='" + Admnno + "'  and sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "'  and Ad_Id=2 and AdFeeDate < getdate()");
        //    }
        //}
        //else
        //{
        //    string str = "04/01/" + drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        //    stringBuilder.Append("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger");
        //    stringBuilder.Append(" inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId");
        //    stringBuilder.Append(" where admnno='" + Admnno + "'  and Ad_Id=2 and AdFeeDate < '" + str + "' ");
        //    drpHosPayble.SelectedIndex = drpHosPayble.Items.Count - 1;
        //}
        //lblHostelFee.Text = obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
        //txtHostelFee.Text = lblHostelFee.Text;
        //ViewState["HFY"] = "n";

        
            obj = new clsDAL();
            StringBuilder stringBuilder = new StringBuilder();
            if (rBtnCurrentSess.Checked)
            {
                ViewState["HFY"] = "n";
            lblHostelFee.Text = obj.GetDataTable("Host_GetStudCurrentBalAmnt", new Hashtable()
      {
        {
           "@AdmnNo",
          drpstudent.SelectedValue.ToString().Trim()
        },
        {
           "@SessionYr",
          drpSession.SelectedValue.ToString().Trim()
        },
        {
           "@PrevSession",
           new clsGenerateFee().CreateCurrSession()
        }
      }).Rows[0]["Balance"].ToString();
            txtHostelFee.Text = lblHostelFee.Text;
        }
            else
            {
                string str = "04/01/" + drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
                stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
                stringBuilder.Append(" from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where PeriodicityID<>6 AND admnno='" + Admnno + "'  and TransDate < '" + str + "' and TransDesc<>'Previous Balance'");
            lblHostelFee.Text = obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
            txtHostelFee.Text = lblHostelFee.Text;
            ViewState["HFY"] = "n";
        }



    }

    private void getBookFee(string Admnno)
    {
        obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        if (rBtnCurrentSess.Checked)
        {
            ViewState["Book"] = "n";
            string currSession = new clsGenerateFee().CreateCurrSession();
            obj = new clsDAL();
            if (obj.ExecuteScalarQry("SELECT TOP 1 SessionYr FROM dbo.PS_FeeNormsNew order by SessionID desc") != currSession)
            {
                obj = new clsDAL();
                if (drpBusPayble.SelectedIndex > 0)
                {
                    string str = Convert.ToInt32(drpBusPayble.SelectedValue.Trim()) < 1 || Convert.ToInt32(drpBusPayble.SelectedValue.Trim()) > 3 ? (!(drpBusPayble.SelectedValue.Trim().Trim() == "12") ? Convert.ToString(Convert.ToInt32(drpBusPayble.SelectedValue.Trim()) + 1) + "/1/" + drpSession.SelectedValue.Substring(0, 4).Trim() : "1/1/" + Convert.ToString(Convert.ToInt32(drpSession.SelectedValue.Substring(0, 4).Trim()) + 1)) : Convert.ToString(Convert.ToInt32(drpBusPayble.SelectedValue.Trim()) + 1) + "/1/" + Convert.ToString(Convert.ToInt32(drpSession.SelectedValue.Substring(0, 4).Trim()) + 1);
                    stringBuilder.Append("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedgerBooks ");
                    stringBuilder.Append("WHERE AdmnNo='" + Admnno + "' AND SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' AND TransDate < '" + str + "'");
                }
                else
                {
                    string str = obj.ExecuteScalarQry("SELECT TOP 1 StartDate+1 AS StartDate FROM dbo.PS_FeeNormsNew order by SessionID desc");
                    stringBuilder.Append("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedgerBooks ");
                    stringBuilder.Append("WHERE AdmnNo='" + Admnno + "' AND SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' AND TransDate <= '" + str + "'");
                }
            }
            else
            {
                stringBuilder.Append("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedgerBooks ");
                stringBuilder.Append("WHERE AdmnNo='" + Admnno + "' AND SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' AND TransDate <= getdate()");
            }
        }
        else
        {
            string str = "04/01/" + drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
            stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance ");
            stringBuilder.Append("from ps_feeledgerBooks where admnno='" + Admnno + "'  and TransDate < '" + str + "'");
            //drpBookPayble.SelectedIndex = drpBookPayble.Items.Count - 1;
        }
        //lblBookFee.Text = obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
        //txtBookFee.Text = lblBookFee.Text;
    }

    protected void btnFeeFullYr_Click(object sender, EventArgs e)
    {
        if (rbAll.Checked)
            FillFeeMonth(drpMonthPayble);
        else if (rbOnAdmission.Checked)
        {
            FillFeeMonthOA(drpMonthPayble);
            drpMonthPayble.SelectedIndex = 1;
        }
        else if (rbMonthly.Checked)
            FillFeeMonthly(drpMonthPayble);
        obj = new clsDAL();
        string str = obj.ExecuteScalarQry("select s.admnno from dbo.PS_ClasswiseStudent c inner join  dbo.PS_StudMaster s on c.admnno=s.AdmnNo where c.admnno=" + txtadmnno.Text.ToString().Trim());
        if (str != "")
        {
            if (drpstudent.SelectedValue.ToString().Trim() != txtadmnno.Text.ToString().Trim())
            {
                fillstudent();
                drpstudent.SelectedValue = str;
            }
            GetBalFullSession(txtadmnno.Text.ToString().Trim());
            if (rBtnCurrentSess.Checked.Equals(true))
                ViewState["FY"] = "y";
            else
                ViewState["FY"] = "n";
            txtamt.Text = Convert.ToString(Convert.ToDouble(lblbalance.Text.Trim().ToString()) + Convert.ToDouble(lblCreditAmt.Text.Trim().ToString()));
            TotalAmount();
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('Invalid Admission Number !');", true);
    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        if (drpstudent.SelectedIndex > 0)
        {
            if (lblbalance.Text != "0.00")
            {
                if (Request.QueryString["Sw"] != null)
                {
                    if (rBtnCurrentSess.Checked)
                    {
                        if (drpMonthPayble.SelectedIndex > 1)
                        {
                            if (rbMonthly.Checked)
                                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetailsMonthly.aspx?admnno=" + txtadmnno.Text.Trim() + " &ft=" + ViewState["FeeTilldate"].ToString() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Sw=" + Request.QueryString["Sw"] + "&Due=C',null,'width=700,height=500');", true);
                            else
                                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetails.aspx?admnno=" + txtadmnno.Text.Trim() + " &ft=" + ViewState["FeeTilldate"].ToString() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Sw=" + Request.QueryString["Sw"] + "&Due=C',null,'width=700,height=500');", true);
                        }
                        else if (rbMonthly.Checked)
                            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetailsMonthly.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Sw=" + Request.QueryString["Sw"] + "&Due=C&AdmnFee=T',null,'width=700,height=500');", true);
                        else
                            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetails.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Sw=" + Request.QueryString["Sw"] + "&Due=C&AdmnFee=T',null,'width=700,height=500');", true);
                    }
                    else if (rbMonthly.Checked)
                        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetailsMonthly.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Sw=" + Request.QueryString["Sw"] + "&Due=P',null,'width=700,height=500');", true);
                    else
                        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetails.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Sw=" + Request.QueryString["Sw"] + "&Due=P',null,'width=700,height=500');", true);
                }
                else if (rBtnCurrentSess.Checked)
                {
                    if (drpMonthPayble.SelectedIndex > 1)
                    {
                        if (rbMonthly.Checked)
                            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetailsMonthly.aspx?admnno=" + txtadmnno.Text.Trim() + " &ft=" + ViewState["FeeTilldate"].ToString() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Due=C',null,'width=700,height=500');", true);
                        else
                            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetails.aspx?admnno=" + txtadmnno.Text.Trim() + " &ft=" + ViewState["FeeTilldate"].ToString() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Due=C',null,'width=700,height=500');", true);
                    }
                    else if (rbMonthly.Checked)
                        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetailsMonthly.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Due=C&AdmnFee=T',null,'width=700,height=500');", true);
                    else
                        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetails.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Due=C&AdmnFee=T',null,'width=700,height=500');", true);
                }
                else if (drpMonthPayble.SelectedIndex > 1)
                {
                    obj = new clsDAL();
                    DateTime dateTime = Convert.ToDateTime(obj.ExecuteScalarQry("SELECT STARTDATE FROM dbo.PS_FeeNormsNew WHERE SessionYr= '" + drpSession.SelectedValue + "'"));
                    if (rbMonthly.Checked)
                        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetailsMonthly.aspx?admnno=" + txtadmnno.Text.Trim() + "&ft=" + dateTime.ToString("dd-MMM-yyyy") + "&sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Due=P',null,'width=700,height=500');", true);
                    else
                        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetails.aspx?admnno=" + txtadmnno.Text.Trim() + "&ft=" + dateTime.ToString("dd-MMM-yyyy") + "&sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Due=P',null,'width=700,height=500');", true);
                }
                else if (rbMonthly.Checked)
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetailsMonthly.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Due=P&AdmnFee=T',null,'width=700,height=500');", true);
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptFeeDueDetails.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Due=P&AdmnFee=T',null,'width=700,height=500');", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails, btnShowDetails.GetType(), "ShowMessage", "alert('No dues for the selected student');", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails, btnShowDetails.GetType(), "ShowMessage", "alert('Please select a student !');", true);
    }

    protected void btnInstDisc_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Sw"] != null)
            Response.Redirect("FeeAdjustment.aspx?admnno=" + txtadmnno.Text.Trim() + "&sess=" + drpSession.SelectedValue.ToString().Trim() + "&cid=" + drpclass.SelectedValue.ToString() + "&Sw=" + Request.QueryString["Sw"]);
        else
            Response.Redirect("FeeAdjustment.aspx?admnno=" + txtadmnno.Text.Trim() + "&sess=" + drpSession.SelectedValue.ToString().Trim() + "&cid=" + drpclass.SelectedValue.ToString());
    }

    protected void drpMonthPayble_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbMonthly.Checked)
            amtCalcMonth();
        else
            amtCalc();
    }

    private void amtCalc()
    {
        try
        {
            obj = new clsDAL();
            if (drpMonthPayble.SelectedIndex > 0)
            {
                if (drpMonthPayble.SelectedValue.ToString().Trim() == "0")
                {
                    GetBalFeeOnAdmn(txtadmnno.Text.ToString().Trim());
                    ViewState["FY"] = "m";
                    txtamt.Text = Convert.ToString(Convert.ToDouble(lblbalance.Text.Trim().ToString()) + Convert.ToDouble(lblCreditAmt.Text.Trim().ToString()));
                    txtdesc.Text = "Admission/Readmission Fee " + drpSession.SelectedValue.Trim();
                }
                else
                {
                    getMonthbalance(txtadmnno.Text.ToString().Trim());
                    ViewState["FY"] = "m";
                    txtamt.Text = Convert.ToString(Convert.ToDouble(ViewState["Bal"]) + Convert.ToDouble(lblCreditAmt.Text.Trim().ToString()));
                }
                TotalAmount();
            }
            else
            {
                txtamt.Text = "0.00";
                lblbalance.Text = "0.00";
                TotalAmount();
            }
        }
        catch
        {
        }
    }

    private void amtCalcMonth()
    {
        try
        {
            obj = new clsDAL();
            if (drpMonthPayble.SelectedIndex > 0)
            {
                getOnlyMonthbalance(txtadmnno.Text.ToString().Trim());
                ViewState["FY"] = "m";
                txtamt.Text = Convert.ToString(Convert.ToDouble(ViewState["Bal"]) + Convert.ToDouble(lblCreditAmt.Text.Trim().ToString()));
                TotalAmount();
            }
            else
            {
                txtamt.Text = "0.00";
                lblbalance.Text = "0.00";
                TotalAmount();
            }
        }
        catch
        {
        }
    }

    private void TotalAmount()
    {
        lblTotalFee.Text = string.Format("{0:f2}", (Convert.ToDouble(txtamt.Text) + Convert.ToDouble(txtMiscFee.Text) + Convert.ToDouble(txtBusFee.Text) +Convert.ToDouble(txtHostelFee.Text) - Convert.ToDouble(lblCreditAmt.Text)));//+ Convert.ToDouble(txtBookFee.Text)  // belongs to this line befor substaction
    }

    protected void btnTillDec_Click(object sender, EventArgs e)
    {
        obj = new clsDAL();
        if (drpMonthPayble.SelectedIndex > 0)
        {
            if (drpMonthPayble.SelectedValue.ToString().Trim() == "0")
            {
                GetBalFeeOnAdmn(txtadmnno.Text.ToString().Trim());
                ViewState["FY"] = "m";
                txtamt.Text = Convert.ToString(Convert.ToDouble(lblbalance.Text.Trim().ToString()) + Convert.ToDouble(lblCreditAmt.Text.Trim().ToString()));
                TotalAmount();
            }
            else
            {
                getMonthbalance(txtadmnno.Text.ToString().Trim());
                ViewState["FY"] = "m";
                txtamt.Text = Convert.ToString(Convert.ToDouble(ViewState["Bal"]) + Convert.ToDouble(lblCreditAmt.Text.Trim().ToString()));
                TotalAmount();
            }
        }
        else
        {
            txtamt.Text = "0.00";
            lblbalance.Text = "0.00";
            TotalAmount();
        }
    }

    private void getMonthbalance(string AdmnNo)
    {
        string str1 = drpMonthPayble.SelectedValue.ToString();
        string str2 = (Convert.ToInt32(drpMonthPayble.SelectedValue.ToString().Trim()) + 1).ToString();
        string str3 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        if (rBtnCurrentSess.Checked.Equals(false))
            str3 = (Convert.ToInt32(str3) - 1).ToString().Trim();
        string str4 = drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
        if (rBtnCurrentSess.Checked.Equals(false))
            str4 = (Convert.ToInt32(str4) - 1).ToString().Trim();
        string str5 = Convert.ToInt32(str1) <= 3 ? str2 + "/01/" + str4 : (Convert.ToInt32(str1) <= 11 ? str2 + "/01/" + str3 : "01/01/" + str4);
        Session["FeeMonth"] = str5;
        obj = new clsDAL();
        if (rBtnCurrentSess.Checked.Equals(true))
        {
            //"select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance   from ps_feeledger FL\tINNER JOIN dbo.PS_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6";
            Hashtable hashtable = new Hashtable();
            DataTable dataTable1 = new DataTable();
            hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
            hashtable.Add("@AdmnNo", AdmnNo);
            hashtable.Add("@NxtFeeDt", str5);
            DataTable dataTable2 = obj.GetDataTable("ps_GetMonthWiseFee", hashtable);
            if (dataTable2.Rows.Count > 0)
            {
                lblbalance.Text = dataTable2.Rows[0][0].ToString().Trim();
                if (dataTable2.Rows[0][1].ToString().Trim() == "Annual" && rbAll.Checked)
                    txtdesc.Text = "Admission/Readmission & upto " + drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
                else if (dataTable2.Rows[0][1].ToString().Trim() == "Annual" && rbMonthly.Checked)
                    txtdesc.Text = "upto " + drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
                else if (dataTable2.Rows[0][1].ToString().Trim() == "Same")
                    txtdesc.Text = drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
                else if (dataTable2.Rows[0][1].ToString().Trim() != "")
                    txtdesc.Text = dataTable2.Rows[0][1].ToString().Trim() + " to " + drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
                else
                    txtdesc.Text = "";
            }
            ViewState["Bal"] = lblbalance.Text;
        }
        else
        {
            lblbalance.Text = obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance   from ps_feeledger FL\tINNER JOIN dbo.PS_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6");
            txtdesc.Text = "Prev Session Due";
            ViewState["Bal"] = lblbalance.Text;
        }
        string text = lblbalance.Text;
        obj = new clsDAL();
        string str6 = obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(FeeCredit),0)) as FeeCredit from dbo.PS_StudCreditLedger where admnno=" + AdmnNo);
        lblCreditAmt.Text = str6;
        Session["CreditAmount"] = str6;
    }

    private void getOnlyMonthbalance(string AdmnNo)
    {
        string str1 = drpMonthPayble.SelectedValue.ToString();
        string str2 = (Convert.ToInt32(drpMonthPayble.SelectedValue.ToString().Trim()) + 1).ToString();
        string str3 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        if (rBtnCurrentSess.Checked.Equals(false))
            str3 = (Convert.ToInt32(str3) - 1).ToString().Trim();
        string str4 = drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
        if (rBtnCurrentSess.Checked.Equals(false))
            str4 = (Convert.ToInt32(str4) - 1).ToString().Trim();
        string str5 = Convert.ToInt32(str1) <= 3 ? str2 + "/01/" + str4 : (Convert.ToInt32(str1) <= 11 ? str2 + "/01/" + str3 : "01/01/" + str4);
        Session["FeeMonth"] = str5;
        obj = new clsDAL();
        if (rBtnCurrentSess.Checked.Equals(true))
        {
            //"select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance   from ps_feeledger FL\tINNER JOIN dbo.PS_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6";
            Hashtable hashtable = new Hashtable();
            DataTable dataTable1 = new DataTable();
            hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
            hashtable.Add("@AdmnNo", AdmnNo);
            hashtable.Add("@NxtFeeDt", str5);
            DataTable dataTable2 = obj.GetDataTable("ps_GetOnlyMonthWiseFee", hashtable);
            if (dataTable2.Rows.Count > 0)
            {
                lblbalance.Text = dataTable2.Rows[0][0].ToString().Trim();
                if (dataTable2.Rows[0][1].ToString().Trim() == "Annual" && rbAll.Checked)
                    txtdesc.Text = "Admission/Readmission & upto " + drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
                else if (dataTable2.Rows[0][1].ToString().Trim() == "Annual" && rbMonthly.Checked)
                    txtdesc.Text = "upto " + drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
                else if (dataTable2.Rows[0][1].ToString().Trim() == "Same")
                    txtdesc.Text = drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
                else if (dataTable2.Rows[0][1].ToString().Trim() != "")
                    txtdesc.Text = dataTable2.Rows[0][1].ToString().Trim() + " to " + drpMonthPayble.SelectedItem.Text.Trim() + " " + drpSession.SelectedValue.Trim();
                else
                    txtdesc.Text = "";
            }
            ViewState["Bal"] = lblbalance.Text;
        }
        else
        {
            lblbalance.Text = obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance   from ps_feeledger FL\tINNER JOIN dbo.PS_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6");
            txtdesc.Text = "Prev Session Due";
            ViewState["Bal"] = lblbalance.Text;
        }
        string text = lblbalance.Text;
        obj = new clsDAL();
        string str6 = obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(FeeCredit),0)) as FeeCredit from dbo.PS_StudCreditLedger where admnno=" + AdmnNo);
        lblCreditAmt.Text = str6;
        Session["CreditAmount"] = str6;
    }

    private void GetBalFeeOnAdmn(string AdmnNo)
    {
        obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        if (rBtnCurrentSess.Checked.Equals(true))
        {
            stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(balance),0)) from PS_FeeLedger where admnno='" + AdmnNo + "' and balance > 0");
            stringBuilder.Append(" and feeid in(select distinct feeid from dbo.PS_FeeComponents where PeriodicityID in (1,2)) and sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "'");
        }
        else
        {
            stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(balance),0)) from PS_FeeLedger where admnno='" + AdmnNo + "' and balance > 0");
            stringBuilder.Append(" and feeid in(select distinct feeid from dbo.PS_FeeComponents where PeriodicityID in (1,2)) and sessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "'");
        }
        lblbalance.Text = obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
        string text = lblbalance.Text;
        obj = new clsDAL();
        string str = obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(FeeCredit),0)) as FeeCredit from dbo.PS_StudCreditLedger where admnno=" + AdmnNo);
        lblCreditAmt.Text = str;
        Session["CreditAmount"] = str;
    }

    protected void btnBusDetails_Click(object sender, EventArgs e)
    {
        if (txtadmnno.Text != "")
        {
            if (rBtnCurrentSess.Checked)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptAddFeeDueDetails.aspx?admino=" + txtadmnno.Text.Trim() + "&sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + "&BH=1&FY=" + ViewState["BFY"].ToString().Trim() + "&Due=C',null,'width=700,height=500');", true);
            else
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptAddFeeDueDetails.aspx?admino=" + txtadmnno.Text.Trim() + "&sess=" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "&class=" + drpclass.SelectedValue.ToString() + "&BH=1&FY=" + ViewState["BFY"].ToString().Trim() + "&Due=P',null,'width=700,height=500');", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails, btnShowDetails.GetType(), "ShowMessage", "alert('Enter Admission Number');", true);
    }

    protected void btnHostelDetails_Click(object sender, EventArgs e)
    {
        //if (txtadmnno.Text != "")
        //{
        //    if (rBtnCurrentSess.Checked)
        //        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptAddFeeDueDetails.aspx?admino=" + txtadmnno.Text.Trim() + "&sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + "&BH=2&FY=" + ViewState["HFY"].ToString().Trim() + "&Due=C',null,'width=700,height=500');", true);
        //    else
        //        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptAddFeeDueDetails.aspx?admino=" + txtadmnno.Text.Trim() + "&sess=" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "&class=" + drpclass.SelectedValue.ToString() + "&BH=2&FY=" + ViewState["HFY"].ToString().Trim() + "&Due=P',null,'width=700,height=500');", true);
        //}
        //else
        //    ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails, btnShowDetails.GetType(), "ShowMessage", "alert('Enter Admission Number');", true);
        if (drpstudent.SelectedIndex > 0)
        {
            if (lblbalance.Text != "0.00")
            {
                if (Request.QueryString["Sw"] != null)
                {
                    if (rBtnCurrentSess.Checked)
                    {
                        if (drpMonthPayble.SelectedIndex > 1)
                            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Sw=" + Request.QueryString["Sw"] + "&Due=C',null,'width=700,height=500');", true);
                        else
                            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Sw=" + Request.QueryString["Sw"] + "&Due=C&AdmnFee=T',null,'width=700,height=500');", true);
                    }
                    else
                        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Sw=" + Request.QueryString["Sw"] + "&Due=P',null,'width=700,height=500');", true);
                }
                else if (rBtnCurrentSess.Checked)
                {
                    if (drpMonthPayble.SelectedIndex > 1)
                        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Due=C',null,'width=700,height=500');", true);
                    else
                        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Due=C&AdmnFee=T',null,'width=700,height=500');", true);
                }
                else if (drpMonthPayble.SelectedIndex > 1)
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Due=P',null,'width=700,height=500');", true);
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" + txtadmnno.Text.Trim() + " &sess=" + drpSession.SelectedValue.ToString().Trim() + "&class=" + drpclass.SelectedValue.ToString() + " &FY=" + ViewState["FY"].ToString().Trim() + "&Due=P&AdmnFee=T',null,'width=700,height=500');", true);
                btnShowDetails.Focus();
            }
            else
                ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails, btnShowDetails.GetType(), "ShowMessage", "alert('No dues for the selected Student');", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails, btnShowDetails.GetType(), "ShowMessage", "alert('Please Select a Student');", true);
    }

    protected void btnBookDetails_Click(object sender, EventArgs e)
    {
        if (txtadmnno.Text != "")
        {
            if (rBtnCurrentSess.Checked)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptBookDueDetails.aspx?admino=" + txtadmnno.Text.Trim() + "&sess=" + drpSession.SelectedValue.ToString().Trim() + "&Due=C',null,'width=700,height=500');", true);
            else
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptBookDueDetails.aspx?admino=" + txtadmnno.Text.Trim() + "&sess=" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "&Due=P',null,'width=700,height=500');", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails, btnShowDetails.GetType(), "ShowMessage", "alert('Enter Admission Number');", true);
    }

    protected void btnMiscDtl_Click(object sender, EventArgs e)
    {
        if (txtadmnno.Text != "")
        {
            if (rBtnCurrentSess.Checked)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptMiscDueDetails.aspx?admino=" + txtadmnno.Text.Trim() + "&Fid=" + drpMiscFee.SelectedValue.ToString().Trim() + "&sess=" + drpSession.SelectedValue.ToString().Trim() + "&Due=C',null,'width=700,height=500');", true);
            else
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('../Reports/rptMiscDueDetails.aspx?admino=" + txtadmnno.Text.Trim() + "&Fid=" + drpMiscFee.SelectedValue.ToString().Trim() + "&sess=" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "&Due=P',null,'width=700,height=500');", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails, btnShowDetails.GetType(), "ShowMessage", "alert('Enter Admission Number');", true);
    }

    protected void btnBusFeeFull_Click(object sender, EventArgs e)
    {
        getFullBusFee(txtadmnno.Text.Trim());
    }

    private void getFullBusFee(string Admnno)
    {
        FillMonth(drpBusPayble);
        obj = new clsDAL();
        string empty = string.Empty;
        lblBusFee.Text = obj.ExecuteScalarQry(!rBtnCurrentSess.Checked.Equals(true) ? "Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=1 AND SessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and admnno=" + Admnno : "Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=1 AND SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + Admnno);
        txtBusFee.Text = lblBusFee.Text;
        TotalAmount();
        ViewState["BFY"] = "y";
        drpBusPayble.SelectedValue = "3";
    }

    private void getFullHostelFee(string Admnno)
    {
        FillMonth(drpHosPayble);
        obj = new clsDAL();
        string empty = string.Empty;
        lblHostelFee.Text = obj.ExecuteScalarQry(!rBtnCurrentSess.Checked.Equals(true) ? "Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=2 AND SessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and admnno=" + Admnno : "Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=2 AND SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + Admnno);
        txtHostelFee.Text = lblHostelFee.Text;
        TotalAmount();
        ViewState["HFY"] = "y";
        drpHosPayble.SelectedValue = "3";
    }
    protected void btnHosFeeFull_Click(object sender, EventArgs e)
    {
        //getFullHostelFee(txtadmnno.Text.Trim());

        FillFeeMonth(drpMonthPayble);
        FillPaybleMonth();
        obj = new clsDAL();
        string str = obj.ExecuteScalarQry("select ad.admnno from dbo.PS_ClasswiseStudent c inner join dbo.Host_Admission ad on c.admnno=ad.AdmnNo where c.admnno=" + txtadmnno.Text.ToString().Trim());
        if (str != "")
        {
            if (drpstudent.SelectedValue.ToString().Trim() != txtadmnno.Text.ToString().Trim())
            {
                fillstudent();
                drpstudent.SelectedValue = str;
            }
            GetHostBalFullSession(txtadmnno.Text.ToString().Trim());
            if (rBtnCurrentSess.Checked.Equals(true))
                ViewState["FY"] = "y";
            else
                ViewState["FY"] = "n";
            txtHostelFee.Text = Convert.ToDouble(lblHostelFee.Text.Trim()).ToString();
            TotalAmount();
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('Invalid Admission Number');", true);
        btnFeeFullYr.Focus();
    }
    private void GetHostBalFullSession(string AdmnNo)
    {
        obj = new clsDAL();
        lblHostelFee.Text = !rBtnCurrentSess.Checked.Equals(true) ? obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' AND PeriodicityID<>6 and admnno=" + AdmnNo) : obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' AND PeriodicityID<>6 and admnno=" + AdmnNo);
        string text = lblHostelFee.Text;
        drpHosPayble.SelectedValue = "3";
    }
    protected void btnBookFeeFull_Click(object sender, EventArgs e)
    {
        //getFullBookFee(txtadmnno.Text.Trim());
    }

    private void getFullBookFee(string Admnno)
    {
    //    FillMonth(drpBookPayble);
    //    obj = new clsDAL();
    //    string empty = string.Empty;
    //    lblBookFee.Text = obj.ExecuteScalarQry(!rBtnCurrentSess.Checked.Equals(true) ? "Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_FeeLedgerBooks where SessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and admnno=" + Admnno : "Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_FeeLedgerBooks where SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + Admnno);
    //    txtBookFee.Text = lblBookFee.Text;
    //    TotalAmount();
    //    ViewState["BOOK"] = "y";
    //    drpBookPayble.SelectedValue = "3";
    }

    protected void drpBusPayble_SelectedIndexChanged(object sender, EventArgs e)
    {
        obj = new clsDAL();
        string str = obj.ExecuteScalarQry("select s.admnno from dbo.PS_ClasswiseStudent c inner join  dbo.PS_StudMaster s on c.admnno=s.AdmnNo where c.admnno=" + txtadmnno.Text.ToString().Trim());
        if (drpBusPayble.SelectedIndex > 0)
        {
            if (str != "")
            {
                if (drpstudent.SelectedValue.ToString().Trim() != txtadmnno.Text.ToString().Trim())
                {
                    fillstudent();
                    drpstudent.SelectedValue = str;
                }
                getBusFeebalance(txtadmnno.Text.ToString().Trim());
                ViewState["BFY"] = "m";
            }
            else
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('Invalid Admission Number !');", true);
        }
        else if (str != "")
        {
            if (drpstudent.SelectedValue.ToString().Trim() != txtadmnno.Text.ToString().Trim())
            {
                fillstudent();
                drpstudent.SelectedValue = str;
            }
            getBusFeebalance(txtadmnno.Text.ToString().Trim());
            ViewState["BFY"] = "n";
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('Invalid Admission Number !');", true);
        txtBusFee.Text = lblBusFee.Text;
        TotalAmount();
    }

    protected void drpHosPayble_SelectedIndexChanged(object sender, EventArgs e)
    {
        //obj = new clsDAL();
        //string str = obj.ExecuteScalarQry("select s.admnno from dbo.PS_ClasswiseStudent c inner join  dbo.PS_StudMaster s on c.admnno=s.AdmnNo where c.admnno=" + txtadmnno.Text.ToString().Trim());
        //if (drpHosPayble.SelectedIndex > 0)
        //{
        //    if (str != "")
        //    {
        //        if (drpstudent.SelectedValue.ToString().Trim() != txtadmnno.Text.ToString().Trim())
        //        {
        //            fillstudent();
        //            drpstudent.SelectedValue = str;
        //        }
        //        getHosFeebalance(txtadmnno.Text.ToString().Trim());
        //        ViewState["HFY"] = "m";
        //    }
        //    else
        //        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('Invalid Admission Number !');", true);
        //}
        //else if (str != "")
        //{
        //    if (drpstudent.SelectedValue.ToString().Trim() != txtadmnno.Text.ToString().Trim())
        //    {
        //        fillstudent();
        //        drpstudent.SelectedValue = str;
        //    }
        //    getHosFeebalance(txtadmnno.Text.ToString().Trim());
        //    ViewState["HFY"] = "n";
        //}
        //else
        //    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('Invalid Admission Number !');", true);
        //txtHostelFee.Text = lblHostelFee.Text;
        //TotalAmount();

        obj = new clsDAL();
        if (drpHosPayble.SelectedIndex > 0)
        {
            if (drpHosPayble.SelectedValue.ToString().Trim() == "0")
            {
                GetBalFeeOnAdmn_hstl(txtadmnno.Text.ToString().Trim());
                ViewState["FY"] = "m";
                txtHostelFee.Text = Convert.ToDouble(lblHostelFee.Text.Trim()).ToString();
            }
            else
            {
                getMonthbalance_hstl(txtadmnno.Text.ToString().Trim());
                ViewState["FY"] = "m";
                txtHostelFee.Text = Convert.ToString(Convert.ToDouble(ViewState["Bal"]) + 0.0);
            }
            TotalAmount();
        }
        else
        {
            txtHostelFee.Text = "0.00";
            lblHostelFee.Text = "0.00";
            TotalAmount();
        }



    }

    private void getMonthbalance_hstl(string AdmnNo)
    {
        string str1 = drpHosPayble.SelectedValue.ToString();
        string str2 = (Convert.ToInt32(drpHosPayble.SelectedValue.ToString().Trim()) + 1).ToString();
        string str3 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        if (rBtnCurrentSess.Checked.Equals(false))
            str3 = (Convert.ToInt32(str3) - 1).ToString().Trim();
        string str4 = drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
        if (rBtnCurrentSess.Checked.Equals(false))
            str4 = (Convert.ToInt32(str4) - 1).ToString().Trim();
        string str5 = Convert.ToInt32(str1) <= 3 ? str2 + "/01/" + str4 : (Convert.ToInt32(str1) <= 11 ? str2 + "/01/" + str3 : "01/01/" + str4);
        Session["FeeMonth"] = str5;
        obj = new clsDAL();
        if (rBtnCurrentSess.Checked.Equals(true))
        {
            // "select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" +drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6";
            lblHostelFee.Text = obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6");
            ViewState["Bal"] = lblHostelFee.Text;
        }
        else
        {
            lblHostelFee.Text = obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6");
            ViewState["Bal"] = lblbalance.Text;
        }
        string text = lblHostelFee.Text;
    }

    private void GetBalFeeOnAdmn_hstl(string AdmnNo)
    {
        obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        if (rBtnCurrentSess.Checked.Equals(true))
        {
            stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(balance),0)) from Host_FeeLedger where admnno='" + AdmnNo + "' and balance > 0");
            stringBuilder.Append(" and feeid in(select distinct feeid from dbo.Host_FeeComponents where PeriodicityID in (1,2)) and SessionYr='" + drpHosPayble.SelectedValue.ToString().Trim() + "'");
        }
        else
        {
            stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(balance),0)) from Host_FeeLedger where admnno='" + AdmnNo + "' and balance > 0");
            stringBuilder.Append(" and feeid in(select distinct feeid from dbo.Host_FeeComponents where PeriodicityID in (1,2)) and sessionYr='" + PrevYr(drpHosPayble.SelectedValue.ToString().Trim()) + "'");
        }
        lblHostelFee.Text = obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
        string text = lblHostelFee.Text;
    }
    protected void drpBookPayble_SelectedIndexChanged(object sender, EventArgs e)
    {
        //obj = new clsDAL();
        //string str = obj.ExecuteScalarQry("select s.admnno from dbo.PS_ClasswiseStudent c inner join  dbo.PS_StudMaster s on c.admnno=s.AdmnNo where c.admnno=" + txtadmnno.Text.ToString().Trim());
        //if (drpBookPayble.SelectedIndex > 0)
        //{
        //    if (str != "")
        //    {
        //        if (drpstudent.SelectedValue.ToString().Trim() != txtadmnno.Text.ToString().Trim())
        //        {
        //            fillstudent();
        //            drpstudent.SelectedValue = str;
        //        }
        //        getBookFeebalance(txtadmnno.Text.ToString().Trim());
        //        ViewState["BOOK"] = "m";
        //    }
        //    else
        //        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('Invalid Admission Number !');", true);
        //}
        //else if (str != "")
        //{
        //    if (drpstudent.SelectedValue.ToString().Trim() != txtadmnno.Text.ToString().Trim())
        //    {
        //        fillstudent();
        //        drpstudent.SelectedValue = str;
        //    }
        //    getBookFeebalance(txtadmnno.Text.ToString().Trim());
        //    ViewState["BOOK"] = "n";
        //}
        //else
        //    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('Invalid Admission Number !');", true);
        //txtBookFee.Text = lblBookFee.Text;
        //TotalAmount();
    }

    private void getBusFeebalance(string AdmnNo)
    {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string str1;
        if (drpBusPayble.SelectedIndex > 0)
        {
            str1 = drpBusPayble.SelectedValue.ToString();
            empty2 = (Convert.ToInt32(drpBusPayble.SelectedValue.ToString().Trim()) + 1).ToString();
        }
        else
            str1 = DateTime.Now.ToString();
        string str2 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        if (rBtnPrevSess.Checked.Equals(true))
            str2 = (Convert.ToInt32(str2) - 1).ToString();
        string str3 = drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
        if (rBtnPrevSess.Checked.Equals(true))
            str3 = (Convert.ToInt32(str3) - 1).ToString();
        obj = new clsDAL();
        if (drpBusPayble.SelectedIndex > 0)
        {
            DateTime dateTime = Convert.ToInt32(str1) <= 3 ? Convert.ToDateTime(empty2 + "/01/" + str3) : (Convert.ToInt32(str1) <= 11 ? Convert.ToDateTime(empty2 + "/01/" + str2) : Convert.ToDateTime("01/01/" + str3));
            Session["BusFeeMonth"] = dateTime;
            if (rBtnCurrentSess.Checked.Equals(true))
                lblBusFee.Text = obj.ExecuteScalarQry("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=1 and SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + AdmnNo + "  and AdFeeDate < '" + dateTime + "'");
            else
                lblBusFee.Text = obj.ExecuteScalarQry("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=1 and SessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and admnno=" + AdmnNo + "  and AdFeeDate < '" + dateTime + "'");
        }
        else if (rBtnCurrentSess.Checked.Equals(true))
            lblBusFee.Text = obj.ExecuteScalarQry("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=1 and SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + AdmnNo + " and AdFeeDate <= getdate()");
        else
            lblBusFee.Text = obj.ExecuteScalarQry("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=1 and SessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and admnno=" + AdmnNo + " and AdFeeDate <= getdate()");
    }

    private void getHosFeebalance(string AdmnNo)
    {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string str1;
        if (drpHosPayble.SelectedIndex > 0)
        {
            str1 = drpHosPayble.SelectedValue.ToString();
            empty2 = (Convert.ToInt32(drpHosPayble.SelectedValue.ToString().Trim()) + 1).ToString();
        }
        else
            str1 = DateTime.Now.ToString();
        string str2 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        if (rBtnPrevSess.Checked.Equals(true))
            str2 = (Convert.ToInt32(str2) - 1).ToString();
        string str3 = drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
        if (rBtnPrevSess.Checked.Equals(true))
            str3 = (Convert.ToInt32(str3) - 1).ToString();
        obj = new clsDAL();
        if (drpHosPayble.SelectedIndex > 0)
        {
            DateTime dateTime = Convert.ToInt32(str1) <= 3 ? Convert.ToDateTime(empty2 + "/01/" + str3) : (Convert.ToInt32(str1) <= 11 ? Convert.ToDateTime(empty2 + "/01/" + str2) : Convert.ToDateTime("01/01/" + str3));
            Session["HostelFeeMonth"] = dateTime;
            if (rBtnCurrentSess.Checked.Equals(true))
                lblHostelFee.Text = obj.ExecuteScalarQry("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=2 and SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + AdmnNo + "  and AdFeeDate < '" + dateTime + "'");
            else
                lblHostelFee.Text = obj.ExecuteScalarQry("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=2 and SessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and admnno=" + AdmnNo + "  and AdFeeDate < '" + dateTime + "'");
        }
        else if (rBtnCurrentSess.Checked.Equals(true))
            lblHostelFee.Text = obj.ExecuteScalarQry("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=2 and SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + AdmnNo + " and AdFeeDate <= getdate()");
        else
            lblHostelFee.Text = obj.ExecuteScalarQry("Select Convert(Decimal(18,2),isnull(sum(balance),0)) as Balance  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=2 and SessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and admnno=" + AdmnNo + " and AdFeeDate <= getdate()");
    }
    private void getBookFeebalance(string AdmnNo)
    {
    //    string empty1 = string.Empty;
    //    string empty2 = string.Empty;
    //    string str1;
    //    if (drpBookPayble.SelectedIndex > 0)
    //    {
    //        str1 = drpBookPayble.SelectedValue.ToString();
    //        empty2 = (Convert.ToInt32(drpBookPayble.SelectedValue.ToString().Trim()) + 1).ToString();
    //    }
    //    else
    //        str1 = DateTime.Now.ToString();
    //    string str2 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
    //    if (rBtnPrevSess.Checked.Equals(true))
    //        str2 = (Convert.ToInt32(str2) - 1).ToString();
    //    string str3 = drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
    //    if (rBtnPrevSess.Checked.Equals(true))
    //        str3 = (Convert.ToInt32(str3) - 1).ToString();
    //    obj = new clsDAL();
    //    if (drpBookPayble.SelectedIndex > 0)
    //    {
    //        DateTime dateTime = Convert.ToInt32(str1) <= 3 ? Convert.ToDateTime(empty2 + "/01/" + str3) : (Convert.ToInt32(str1) <= 11 ? Convert.ToDateTime(empty2 + "/01/" + str2) : Convert.ToDateTime("01/01/" + str3));
    //        Session["BookFeeMonth"] = dateTime;
    //        if (rBtnCurrentSess.Checked.Equals(true))
    //            lblBookFee.Text = obj.ExecuteScalarQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedgerBooks WHERE SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' and AdmnNo=" + AdmnNo + "  and TransDate < '" + dateTime + "'");
    //        else
    //            lblBookFee.Text = obj.ExecuteScalarQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedgerBooks WHERE SessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and AdmnNo=" + AdmnNo + "  and TransDate < '" + dateTime + "'");
    //    }
    //    else if (rBtnCurrentSess.Checked.Equals(true))
    //        lblBookFee.Text = obj.ExecuteScalarQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedgerBooks WHERE SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "' and AdmnNo=" + AdmnNo + "  and TransDate <= getdate()");
    //    else
    //        lblBookFee.Text = obj.ExecuteScalarQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedgerBooks WHERE SessionYr='" + PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and AdmnNo=" + AdmnNo + "  and TransDate <= getdate()");
    }

    protected void btnSFClear_Click(object sender, EventArgs e)
    {
        txtamt.Text = "0.00";
        TotalAmount();
    }

    protected void btnMFClear_Click(object sender, EventArgs e)
    {
        txtMiscFee.Text = "0.00";
        TotalAmount();
    }

    protected void btnBFClear_Click(object sender, EventArgs e)
    {
        txtBusFee.Text = "0.00";
        TotalAmount();
    }

    protected void btnHFClear_Click(object sender, EventArgs e)
    {
        //txtBookFee.Text = "0.00";
        //TotalAmount();
    }

    protected void btnMiscFee_Click(object sender, EventArgs e)
    {
        string str1 = drpSession.SelectedValue.Trim();
        DateTime dateTime1 = Convert.ToDateTime("31 Mar" + str1.Substring(0, 4));
        DateTime dateTime2 = Convert.ToDateTime("1 Apr" + Convert.ToString(Convert.ToInt32(str1.Substring(0, 4)) + 1));
        if (DateTime.Today < dateTime2 && DateTime.Today > dateTime1 && dtpMiscDt.GetDateValue().Date > DateTime.Today)
        {
            lblMsg.Text = "Misc fee date cannot be greater than current date!";
            lblMsg2.Text = "Misc fee date cannot be greater than current date!";
            lblMsg.ForeColor = Color.Red;
            lblMsg2.ForeColor = Color.Red;
        }
        else if (dtpMiscDt.GetDateValue().Date >= dateTime2 || dtpMiscDt.GetDateValue().Date <= dateTime1 && rBtnCurrentSess.Checked)
        {
            lblMsg.Text = "Misc fee date not in the selected Session year!";
            lblMsg2.Text = "Misc fee date not in the selected Session year!";
            lblMsg.ForeColor = Color.Red;
            lblMsg2.ForeColor = Color.Red;
        }
        else
        {
            clsDAL clsDal = new clsDAL();
            string str2 = clsDal.ExecuteScalar("Ps_Sp_InsAdditionalFeeChk", new Hashtable()
      {
        {
           "@TransDate",
           dtpMiscDt.GetDateValue().ToString("dd MMM yyyy")
        },
        {
           "@AdmnNo",
           txtadmnno.Text.ToString().Trim()
        },
        {
           "@AdFeeId",
           drpAdditionalFee.SelectedValue.ToString().Trim()
        }
      });
            if (str2.Trim() != "No")
            {
                Hashtable hashtable = new Hashtable();
                DataTable dataTable1 = new DataTable();
                hashtable.Add("@TransDate", dtpMiscDt.GetDateValue().ToString("dd MMM yyyy"));
                hashtable.Add("@AdmnNo", txtadmnno.Text.ToString().Trim());
                hashtable.Add("@TransDesc", drpAdditionalFee.SelectedItem.Text);
                hashtable.Add("@Debit", txtRcevMiscFee.Text.Trim());
                hashtable.Add("@Credit", "0.0000");
                hashtable.Add("@Balance", txtRcevMiscFee.Text.Trim());
                hashtable.Add("@UserID", Session["User_Id"]);
                hashtable.Add("@AdFeeId", drpAdditionalFee.SelectedValue.ToString().Trim());
                hashtable.Add("@SchoolId", Session["SchoolId"].ToString());
                DataTable dataTable2 = clsDal.GetDataTable("Ps_Sp_InsAdditionalFee", hashtable);
                if (dataTable2.Rows.Count <= 0 && str2.Trim() == "Yes")
                {
                    lblMsg.Text = "Misc Fee Updated Successfully";
                    lblMsg.ForeColor = Color.Green;
                    lblMsg2.Text = "Misc Fee Updated Successfully";
                    lblMsg2.ForeColor = Color.Green;
                }
                else if (dataTable2.Rows.Count <= 0)
                {
                    lblMsg.Text = "Misc Fee Added Successfully";
                    lblMsg.ForeColor = Color.Green;
                    lblMsg2.Text = "Misc Fee Added Successfully";
                    lblMsg2.ForeColor = Color.Green;
                }
            }
            else if (str2.Trim() == "No")
            {
                lblMsg.Text = "Cannot Update Mics Fee!!Fee Alraedy Received On this or later date!!";
                lblMsg.ForeColor = Color.Red;
                lblMsg2.Text = "Cannot Update Mics Fee!!Fee Alraedy Received On this or later date!!";
                lblMsg2.ForeColor = Color.Red;
                return;
            }
            FillMiscFeeNames(drpMiscFee);
            FillFeeAmt();
            if (rbAll.Checked)
            {
                FillFeeMonth(drpMonthPayble);
                amtCalc();
            }
            else if (rbOnAdmission.Checked)
            {
                FillFeeMonthOA(drpMonthPayble);
                drpMonthPayble.SelectedIndex = 1;
                amtCalc();
            }
            else
            {
                if (!rbMonthly.Checked)
                    return;
                FillFeeMonthly(drpMonthPayble);
                amtCalcMonth();
            }
        }
    }

    private void FillPaybleMonth()
    {
        FillFeeMonth(drpMonthPayble);
    }

    private void FillMiscFeeNames(DropDownList drpMiscFee)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = clsDal.GetDataTableQry("SELECT DISTINCT FL.FeeId,FC.FeeName FROM dbo.PS_FeeLedger FL INNER JOIN dbo.PS_FeeComponents FC ON FC.FeeID=FL.FeeID WHERE AdmnNo=" + drpstudent.SelectedValue.ToString().Trim() + " AND Balance>0 AND PeriodicityID=6");
        drpMiscFee.DataSource = dataTableQry;
        drpMiscFee.DataTextField = "FeeName";
        drpMiscFee.DataValueField = "FeeId";
        drpMiscFee.DataBind();
        drpMiscFee.Items.Insert(0, new ListItem("-All-", "0"));
    }

    private string PrevYr(string p)
    {
        int int32_1 = Convert.ToInt32(drpSession.SelectedValue.ToString().Trim().Substring(0, 4));
        int num = int32_1 - 1;
        int int32_2 = Convert.ToInt32(int32_1.ToString().Trim().Substring(2));
        string empty = string.Empty;
        return Convert.ToString(num) + "-" + Convert.ToString(int32_2);
    }

    private void FillBusPaybleMonth()
    {
        obj = new clsDAL();
        int num = int.Parse(obj.ExecuteScalarQry("Select count(*)  from PS_AdFeeLedger inner join PS_ClasswiseStudent on  PS_ClasswiseStudent.Id = PS_AdFeeLedger.ClassWiseId where Ad_Id=1  and admnno='" + txtadmnno.Text.Trim() + "' AND Balance=0 and SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "'"));
        if (num < 13)
        {
            for (int index = num; index > 0; --index)
                drpBusPayble.Items.RemoveAt(index);
        }
        else
            FillMonth(drpBusPayble);
    }

    private void FillBookPaybleMonth()
    {
    //    obj = new clsDAL();
    //    int num = int.Parse(obj.ExecuteScalarQry("Select count(*)  from PS_FeeLedgerBooks where admnno='" + txtadmnno.Text.Trim() + "' AND Balance=0 and SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "'"));
    //    if (num < 13)
    //    {
    //        for (int index = num; index > 0; --index)
    //            drpBookPayble.Items.RemoveAt(index);
    //    }
    //    else
    //        FillMonth(drpBookPayble);
    }

    protected void drpAdditionalFee_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtRcevMiscFee.Text = new clsDAL().ExecuteScalarQry("select CAST(Amount AS DECIMAL(10,2)) AS Amount from dbo.PS_FeeAmount where FeeCompId=" + drpAdditionalFee.SelectedValue + " and StreamId=(select StreamID from dbo.PS_ClassMaster where ClassID=" + drpclass.SelectedValue.ToString() + ")");
    }

    private void FillFeeMonth(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("-Select-", ""));
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
        if (new clsGenerateFee().CreateCurrSession() != drpSession.SelectedValue.ToString().Trim())
        {
            if (!rBtnCurrentSess.Checked)
                return;
            drp.SelectedValue = "4";
        }
        else
        {
            DateTime.Now.Month.ToString();
            drp.SelectedValue = DateTime.Now.Month.ToString();
        }
    }

    private void FillFeeMonthOA(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("-Select-", ""));
        drp.Items.Insert(1, new ListItem("On Admission", "0"));
        new clsGenerateFee().CreateCurrSession();
    }

    private void FillFeeMonthly(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("-Select-", ""));
        drp.Items.Insert(1, new ListItem("Apr", "4"));
        drp.Items.Insert(2, new ListItem("May", "5"));
        drp.Items.Insert(3, new ListItem("June", "6"));
        drp.Items.Insert(4, new ListItem("July", "7"));
        drp.Items.Insert(5, new ListItem("Aug", "8"));
        drp.Items.Insert(6, new ListItem("Sep", "9"));
        drp.Items.Insert(7, new ListItem("Oct", "10"));
        drp.Items.Insert(8, new ListItem("Nov", "11"));
        drp.Items.Insert(9, new ListItem("Dec", "12"));
        drp.Items.Insert(10, new ListItem("Jan", "1"));
        drp.Items.Insert(11, new ListItem("Feb", "2"));
        drp.Items.Insert(12, new ListItem("Mar", "3"));
        if (new clsGenerateFee().CreateCurrSession() != drpSession.SelectedValue.ToString().Trim())
        {
            if (!rBtnCurrentSess.Checked)
                return;
            drp.SelectedValue = "3";
        }
        else
        {
            DateTime.Now.Month.ToString();
            drp.SelectedValue = DateTime.Now.Month.ToString();
        }
    }

    private void FillMonth(DropDownList drp)
    {
        drp.Items.Clear();
        drp.Items.Insert(0, new ListItem("-Select-", "0"));
        drp.Items.Insert(1, new ListItem("Apr", "4"));
        drp.Items.Insert(2, new ListItem("May", "5"));
        drp.Items.Insert(3, new ListItem("June", "6"));
        drp.Items.Insert(4, new ListItem("July", "7"));
        drp.Items.Insert(5, new ListItem("Aug", "8"));
        drp.Items.Insert(6, new ListItem("Sep", "9"));
        drp.Items.Insert(7, new ListItem("Oct", "10"));
        drp.Items.Insert(8, new ListItem("Nov", "11"));
        drp.Items.Insert(9, new ListItem("Dec", "12"));
        drp.Items.Insert(10, new ListItem("Jan", "1"));
        drp.Items.Insert(11, new ListItem("Feb", "2"));
        drp.Items.Insert(12, new ListItem("Mar", "3"));
        if (new clsGenerateFee().CreateCurrSession() != drpSession.SelectedValue.ToString().Trim())
        {
            if (!rBtnCurrentSess.Checked)
                return;
            drp.SelectedValue = "4";
        }
        else
            drp.SelectedValue = DateTime.Now.Month.ToString();
    }

    protected void rBtnCurrentSess_CheckedChanged(object sender, EventArgs e)
    {
        setMiscDate();
        if (!(txtadmnno.Text.Trim() != ""))
            return;
        GetAllFeeDetails();
    }

    protected void btnRcvOldDue_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Sw"] != null)
            Response.Redirect("FeeRecptPrevDue.aspx?admnno=" + txtadmnno.Text.Trim() + "&sess=" + drpSession.SelectedValue.ToString().Trim() + "&cid=" + drpclass.SelectedValue.ToString() + "&Sw=" + Request.QueryString["Sw"]);
        else
            Response.Redirect("FeeRecptPrevDue.aspx?admnno=" + txtadmnno.Text.Trim() + "&sess=" + drpSession.SelectedValue.ToString().Trim() + "&cid=" + drpclass.SelectedValue.ToString());
    }

    protected void drpMiscFee_SelectedIndexChanged(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry;
        if (rBtnCurrentSess.Checked.Equals(true))
        {
            string str = drpSession.SelectedValue.Trim();
            DateTime dateTime1 = Convert.ToDateTime("1 Apr" + str.Substring(0, 4));
            DateTime dateTime2 = Convert.ToDateTime("31 Mar" + Convert.ToString(Convert.ToInt32(str.Substring(0, 4)) + 1));
            if (drpMiscFee.SelectedIndex > 0)
            {
                dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedger WHERE AdmnNo=" + drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" + drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" + drpSession.SelectedValue.Trim() + "' AND TransDate <= GETDATE()");
                if (DateTime.Today < dateTime2 && DateTime.Today < dateTime1)
                    dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedger WHERE AdmnNo=" + drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" + drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" + drpSession.SelectedValue.Trim() + "' AND TransDate >='1 Apr" + str.Substring(0, 4) + "'");
            }
            else
            {
                dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedger FL\tINNER JOIN dbo.PS_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" + drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" + drpSession.SelectedValue.Trim() + "' AND TransDate <= GETDATE()");
                if (DateTime.Today < dateTime2 && DateTime.Today < dateTime1)
                    dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedger FL\tINNER JOIN dbo.PS_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" + drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" + drpSession.SelectedValue.Trim() + "' AND TransDate >='1 Apr" + str.Substring(0, 4) + "'");
            }
        }
        else if (drpMiscFee.SelectedIndex > 0)
            dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedger WHERE AdmnNo=" + drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" + drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" + PrevYr(drpSession.SelectedValue.Trim()) + "' AND TransDate <= GETDATE()");
        else
            dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedger FL\tINNER JOIN dbo.PS_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" + drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" + PrevYr(drpSession.SelectedValue.Trim()) + "' AND TransDate <= GETDATE()");
        lblMiscFee.Text = dataTableQry.Rows[0][0].ToString();
        txtMiscFee.Text = lblMiscFee.Text;
        TotalAmount();
    }

    private void getMiscFeeMly()
    {
        string str1;
        string str2;
        if (drpMonthPayble.SelectedValue.Trim() == "0")
        {
            str1 = "4";
            str2 = 5.ToString();
        }
        else
        {
            str1 = drpMonthPayble.SelectedValue.ToString();
            str2 = (Convert.ToInt32(drpMonthPayble.SelectedValue.ToString().Trim()) + 1).ToString();
        }
        string str3 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        if (rBtnCurrentSess.Checked.Equals(false))
            str3 = (Convert.ToInt32(str3) - 1).ToString().Trim();
        string str4 = drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
        if (rBtnCurrentSess.Checked.Equals(false))
            str4 = (Convert.ToInt32(str4) - 1).ToString().Trim();
        string str5 = Convert.ToInt32(str1) <= 3 ? str2 + "/01/" + str4 : (Convert.ToInt32(str1) <= 11 ? str2 + "/01/" + str3 : "01/01/" + str4);
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry;
        if (rBtnCurrentSess.Checked.Equals(true))
        {
            if (drpMiscFee.SelectedIndex > 0)
                dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedger WHERE AdmnNo=" + drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" + drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" + drpSession.SelectedValue.Trim() + "' AND TransDate <'" + str5 + "'");
            else
                dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedger FL\tINNER JOIN dbo.PS_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" + drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" + drpSession.SelectedValue.Trim() + "' AND TransDate <'" + str5 + "'");
        }
        else if (drpMiscFee.SelectedIndex > 0)
            dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedger WHERE AdmnNo=" + drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" + drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" + PrevYr(drpSession.SelectedValue.Trim()) + "' AND TransDate <'" + str5 + "'");
        else
            dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.PS_FeeLedger FL\tINNER JOIN dbo.PS_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" + drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" + PrevYr(drpSession.SelectedValue.Trim()) + "' AND TransDate <'" + str5 + "'");
        lblMiscFee.Text = dataTableQry.Rows[0][0].ToString();
        txtMiscFee.Text = lblMiscFee.Text;
    }

    private void statusOnFail()
    {
        if (ViewState["RcptNo"] != null)
            DelFeeOnFail(ViewState["RcptNo"].ToString().Trim());
        else
            DelFeeOnFail("");
        if (ViewState["Status"].ToString().Trim() == "DUP")
        {
            lblMsg.Text = "Receipt No Already Exists!";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Transaction Failed Please Try Again Later!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void DelFeeOnFail(string RecptNo)
    {
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal1 = new clsDAL();
        if (RecptNo.Trim() != "")
            hashtable.Add("@receiptno", RecptNo);
        hashtable.Add("@AdmnNo", drpstudent.SelectedValue.Trim());
        hashtable.Add("@TransDt", dtpFeeDt.GetDateValue().ToString("dd MMM yyyy"));
        DataTable dataTable = new DataTable();
        clsDAL clsDal2 = new clsDAL();
        clsDal1.GetDataTable("ps_sp_deleteFeeOnTransFail", hashtable);
    }

    protected void rbAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillFeeMonth(drpMonthPayble);
            amtCalc();
            btnFeeFullYr.Enabled = true;
        }
        catch
        {
        }
    }

    protected void rbOnAdmission_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillFeeMonthOA(drpMonthPayble);
            drpMonthPayble.SelectedIndex = 1;
            amtCalc();
            btnFeeFullYr.Enabled = false;
        }
        catch
        {
        }
    }

    protected void rbMonthly_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            FillFeeMonthly(drpMonthPayble);
            amtCalcMonth();
            btnFeeFullYr.Enabled = true;
        }
        catch
        {
        }
    }

    protected void btnoldhostreciv_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Sw"] != null)
            Response.Redirect("../Hostel/HostRcptPrevDue.aspx?admnno=" + txtadmnno.Text.Trim() + "&sess=" + drpSession.SelectedValue.ToString().Trim() + "&cid=" + drpclass.SelectedValue.ToString() + "&Sw=" + Request.QueryString["Sw"]);
        else
            Response.Redirect("../Hostel/HostRcptPrevDue.aspx?admnno=" + txtadmnno.Text.Trim() + "&sess=" + drpSession.SelectedValue.ToString().Trim() + "&cid=" + drpclass.SelectedValue.ToString());
    }
}