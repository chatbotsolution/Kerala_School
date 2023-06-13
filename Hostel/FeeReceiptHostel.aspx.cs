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


public partial class Hostel_FeeReceiptHostel : System.Web.UI.Page
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
   ViewState["Bal"] =  0;
   btnRcvOldDue.Visible = false;
   ViewState["FY"] =  "n";
   ViewState["Misc"] =  "n";
   ViewState["PrevDue"] =  "0";
   ViewState["OldDue"] =  "0";
   Page.Form.DefaultButton =btnsave.UniqueID;
   lblbalance.Text = "0.00";
   lblMiscFee.Text = "0.00";
   fillsession();
   fillclass();
   fillAditionalFee();
   dtpFeeDt.SetDateValue(DateTime.Today);
   dtpMiscDt.SetDateValue(DateTime.Today);
    if (Request.QueryString["admnno"] != null)
    {
     txtadmnno.Text =Request.QueryString["admnno"].ToString();
     FillSelStudent();
     FillFeeMonth(drpMonthPayble);
     FillMiscFeeNames(drpMiscFee);
     FillFeeAmt();
     txtadmnno.Text =drpstudent.SelectedValue;
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
    DataTable dataTableQry =obj.GetDataTableQry(query);
    drp.DataSource =  dataTableQry;
    drp.DataTextField = textfield;
    drp.DataValueField = valuefield;
    drp.DataBind();
    drp.Items.Insert(0, new ListItem("- Select Bank Account -", "0"));
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
    string str =drpSession.SelectedValue.Trim();
    DateTime dateTime1 = Convert.ToDateTime("1 Apr" + str.Substring(0, 4));
    DateTime dateTime2 = Convert.ToDateTime("31 Mar" + Convert.ToString(Convert.ToInt32(str.Substring(0, 4)) + 1));
    if (DateTime.Today < dateTime2 && DateTime.Today < dateTime1)
    {
      if (rBtnPrevSess.Checked)
          dtpMiscDt.SetDateValue(dateTime1.AddDays(-1.0));
      else
          dtpMiscDt.SetDateValue(dateTime1);
     txtMiscDt.ReadOnly = true;
     dtpMiscDt.Enabled=false;
    }
    else if (DateTime.Today > dateTime2 && DateTime.Today > dateTime1)
    {
        dtpMiscDt.SetDateValue(dateTime2);
     txtMiscDt.ReadOnly = true;
     dtpMiscDt.Enabled=false;
    }
    else
    {
      if (rBtnPrevSess.Checked)
      {
       dtpMiscDt.SetDateValue(dateTime1.AddDays(-1.0));
       dtpMiscDt.Enabled=false;
      }
      else
      {
       dtpMiscDt.SetDateValue(DateTime.Today);
       dtpMiscDt.Enabled=true;
      }
     txtMiscDt.ReadOnly = true;
    }
  }

  private void fillclass()
  {
   obj = new clsDAL();
    DataTable dataTableQry =obj.GetDataTableQry("select * from ps_classmaster");
   drpclass.DataSource =  dataTableQry;
   drpclass.DataTextField = "classname";
   drpclass.DataValueField = "classid";
   drpclass.DataBind();
    if (dataTableQry.Rows.Count > 0)
     drpclass.Items.Insert(0, new ListItem("- Select -", "0"));
    else
     drpclass.Items.Insert(0, new ListItem("No Record", "0"));
  }

  private void GetStudImg()
  {
    if (drpstudent.SelectedIndex <= 0)
      return;
   obj = new clsDAL();
    string str =obj.ExecuteScalarQry("Select StudentPhoto from dbo.PS_StudMaster where Admnno=" +drpstudent.SelectedValue.ToString());
    if (str.Trim() != "")
     imgStud.ImageUrl = "../Up_Files/Studimage/" + str.Trim();
    else
     imgStud.ImageUrl = "../Up_Files/Studimage/noimage.jpg";
  }

  protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
  {
   fillstudent();
   drpstudent_SelectedIndexChanged(drpstudent, EventArgs.Empty);
   drpclass.Focus();
  }

  private void fillstudent()
  {
   obj = new clsDAL();
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("select s.fullname,s.admnno from dbo.Host_Admission ad");
    stringBuilder.Append(" inner join dbo.PS_ClasswiseStudent cs on ad.admnno=cs.admnno");
    stringBuilder.Append(" inner join ps_studmaster s on s.admnno=ad.admnno");
    stringBuilder.Append(" where 1=1 and s.Status=1 and ad.LeavingDt is null");
    if (drpclass.SelectedIndex != 0)
      stringBuilder.Append(" and cs.classid=" +drpclass.SelectedValue);
    stringBuilder.Append(" and cs.SessionYear='" +drpSession.SelectedValue.ToString().Trim() + "' and Detained_Promoted='' order by fullname asc");
    DataTable dataTableQry =obj.GetDataTableQry(stringBuilder.ToString().Trim());
    try
    {
     drpstudent.DataSource =  dataTableQry;
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
     drpstudent.Items.Insert(0, new ListItem("- Select -", "0"));
    }
    else
    {
     drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
     lblbalance.Text = "0";
     txtadmnno.Text = string.Empty;
    }
  }

  protected void fillAditionalFee()
  {
   drpAdditionalFee.DataSource =  new clsDAL().GetDataTableQry("SELECT FC.FeeID,FC.FeeName,FC.PeriodicityID FROM dbo.Host_FeeComponents FC WHERE PeriodicityID=6 ORDER BY FeeName DESC");
   drpAdditionalFee.DataTextField = "FeeName";
   drpAdditionalFee.DataValueField = "FeeID";
   drpAdditionalFee.DataBind();
   drpAdditionalFee.Items.Insert(0, new ListItem("- SELECT -", "0"));
  }

  protected void btnsave_Click(object sender, EventArgs e)
  {
   TotalAmount();
    clsDAL clsDal = new clsDAL();
    if (!getValidInput())
      return;
   lblMsg.Text = string.Empty;
   lblMsg2.Text = string.Empty;
    string str1 = "select count(*) from dbo.Acts_PaymentReceiptVoucher where TransDt > '" +dtpFeeDt.GetDateValue().ToString("dd MMM yyyy") + "' and (IsDeleted is null or IsDeleted=0)";
    if (Convert.ToInt32(clsDal.ExecuteScalarQry(str1)) > 0)
    {
      //Label lblMsg =lblMsg;
      //Label lblMsg2 =lblMsg2;
      string str2 = "Receipt already generated for Later Date. Receipt can not be generated now for ";
      string str3 =dtpFeeDt.GetDateValue().ToString("dd MMM yyyy");
      string str4;
      string str5 = str4 = str2 + str3;
      lblMsg2.Text = str4;
      string str6 = str5;
      lblMsg.Text = str6;
     lblMsg.ForeColor =lblMsg2.ForeColor = Color.Red;
    }
    else
     SaveData();
  }

  private bool getValidInput()
  {
    double num1 = Convert.ToDouble(ViewState["PrevDue"]);
    double num2 = Convert.ToDouble(ViewState["OldDue"]);
    if ((num1 <= 0.0 || !rBtnCurrentSess.Checked.Equals(true)) && num2 <= 0.0)
      return true;
   lblMsg.Text = num2 <= 0.0 ? (lblMsg2.Text = "Please Clear Previous Due Rs." +ViewState["PrevDue"] + " first.") : (lblMsg2.Text = "Please Clear Old Due Rs." +ViewState["OldDue"] + "  first.");
   lblMsg.ForeColor =lblMsg2.ForeColor = Color.Red;
    return false;
  }

  private void SaveData()
  {
   ViewState["FeeTable"] =  string.Empty;
   ViewState["DrAcHead"] =  string.Empty;
   ViewState["Status"] =  string.Empty;
    if (drpPmtMode.SelectedValue.ToString().Trim() == "Cash")
     ViewState["DrAcHead"] =  "3";
    else
     ViewState["DrAcHead"] = drpBankAc.SelectedValue.ToString();
    if (Request.QueryString["rno"] != null)
    {
      if (double.Parse(txtamt.Text.Trim()) > 0.0)
       save();
      if (double.Parse(txtMiscFee.Text.Trim()) > 0.0)
       MiscFee();
      if (ViewState["RcptNo"] != null)
       obj.ExecuteScalar("Host_Insert_GenLedgerNew", new Hashtable()
        {
          {
             "@Receipt_VrNo",
            ViewState["RcptNo"].ToString().Trim()
          },
          {
             "@Amount",
             (!(txtamt.Text.Trim() != "") && !(txtamt.Text.Trim() != "0") || !(txtMiscFee.Text.Trim() != "") && !(txtMiscFee.Text.Trim() != "0") ? (!(txtamt.Text.Trim() != "") && !(txtamt.Text.Trim() != "0") || !(txtMiscFee.Text.Trim() == "") && !(txtMiscFee.Text.Trim() == "0") ? Convert.ToDecimal(txtMiscFee.Text.Trim()) : Convert.ToDecimal(txtamt.Text.Trim())) : Convert.ToDecimal(txtamt.Text.Trim()) + Convert.ToDecimal(txtMiscFee.Text.Trim()))
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
      if (double.Parse(txtamt.Text.Trim()) > 0.0)
       save();
      if (ViewState["Status"].ToString().Trim() != "DUP" &&ViewState["Status"].ToString().Trim() != "NO")
      {
        if (double.Parse(txtMiscFee.Text.Trim()) > 0.0)
         MiscFee();
        if (ViewState["Status"].ToString().Trim() != "DUP" &&ViewState["Status"].ToString().Trim() != "NO")
        {
          if (ViewState["RcptNo"] != null)
            str =obj.ExecuteScalar("Host_Insert_GenLedgerNew", new Hashtable()
            {
              {
                 "@Receipt_VrNo",
                ViewState["RcptNo"].ToString().Trim()
              },
              {
                 "@Amount",
                 (!(txtamt.Text.Trim() != "") && !(txtamt.Text.Trim() != "0.00") || !(txtMiscFee.Text.Trim() != "") && !(txtMiscFee.Text.Trim() != "0.00") ? (!(txtamt.Text.Trim() != "") && !(txtamt.Text.Trim() != "0.00") || !(txtMiscFee.Text.Trim() == "") && !(txtMiscFee.Text.Trim() == "0.00") ? Convert.ToDecimal(txtMiscFee.Text.Trim()) : Convert.ToDecimal(txtamt.Text.Trim())) : Convert.ToDecimal(txtamt.Text.Trim()) + Convert.ToDecimal(txtMiscFee.Text.Trim()))
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
           ViewState["Status"] =  "NO";
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
     Response.Redirect("../Hostel/rptHostFeeReceipt.aspx?admnno=" +txtadmnno.Text.Trim() + " &sess=" +drpSession.SelectedValue.ToString().Trim() + " &status=1 &rno=" +Rcptno + " &rcptdt=" +dtpFeeDt.GetDateValue().ToString("dd-MM-yyyy") + " &class=" +drpclass.SelectedValue.ToString() + " &D=p &ChkNo=" +txtChqNo.Text.Trim() + " &ChkDt=" +txtChqDate.Text.ToString().Trim() + " &Bn=" +txtBank.Text.ToString().Trim() + " &Pmnt=" +drpPmtMode.SelectedValue.ToString().Trim() + "&Sw=" +Request.QueryString["Sw"]);
    else
     Response.Redirect("../Hostel/rptHostFeeReceipt.aspx?admnno=" +txtadmnno.Text.Trim() + " &sess=" +drpSession.SelectedValue.ToString().Trim() + " &status=1 &rno=" +Rcptno + " &rcptdt=" +dtpFeeDt.GetDateValue().ToString("dd-MM-yyyy") + " &class=" +drpclass.SelectedValue.ToString() + " &D=p &ChkNo=" +txtChqNo.Text.Trim() + " &ChkDt=" +txtChqDate.Text.ToString().Trim() + " &Bn=" +txtBank.Text.ToString().Trim() + " &Pmnt=" +drpPmtMode.SelectedValue.ToString().Trim());
  }

  private void save()
  {
    Decimal num1 = new Decimal(0);
    Decimal CrAmt = new Decimal(0);
    Decimal num2;
    try
    {
      num2 = Convert.ToDecimal(txtamt.Text);
    }
    catch
    {
      ScriptManager.RegisterClientScriptBlock((Control)txtamt,txtamt.GetType(), "ShowMessage", "alert('Invalid amount');", true);
      return;
    }
    if (num2 <= new Decimal(0) && CrAmt <= new Decimal(0))
    {
      ScriptManager.RegisterClientScriptBlock((Control)txtamt,txtamt.GetType(), "ShowMessage", "alert('Enter Amount greater than 0, There is no credit available in credit ledger.');", true);
    }
    else
    {
      Hashtable hashtable = new Hashtable();
      hashtable.Add( "Session", drpSession.SelectedValue);
      hashtable.Add( "RecvDate", dtpFeeDt.GetDateValue().ToString("dd MMM yyyy"));
      hashtable.Add( "AdmnNo", txtadmnno.Text);
      hashtable.Add( "Description", txtdesc.Text.Trim());
      hashtable.Add( "Amount",  double.Parse(lblTotalFee.Text.Trim()));
      hashtable.Add( "PaymentMode", drpPmtMode.SelectedValue.ToString());
      if (drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
      {
        hashtable.Add( "ChequeNo", txtChqNo.Text.Trim());
        hashtable.Add( "ChequeDate", txtChqDate.Text.ToString());
        hashtable.Add( "DrawanOnBank", txtBank.Text);
        hashtable.Add( "BankAcHeadId",  int.Parse(drpBankAc.SelectedValue.ToString()));
      }
      hashtable.Add( "UserID",  Convert.ToInt32(Session["User_Id"].ToString().Trim()));
      hashtable.Add( "SchoolID", Session["SchoolId"].ToString());
      StringBuilder stringBuilder = new StringBuilder();
      if (rBtnCurrentSess.Checked)
        hashtable.Add( "CurrSessFee",  1);
      else
        hashtable.Add( "CurrSessFee",  0);
     obj = new clsDAL();
      DataTable dataTable = new DataTable();
      DataTable tb = !(drpMonthPayble.SelectedValue.ToString().Trim() != "0") ?obj.GetDataTable("Host_Insert_FeeCash_OnAdmn", hashtable) :obj.GetDataTable("Host_Insert_FeeCash_OnFeeReceipt", hashtable);
     ViewState["FeeTable"] =  tb;
      if (tb.Rows.Count > 0 && tb.Rows[0][0].ToString().Trim() != "DUP" && tb.Rows[0][0].ToString().Trim() != "NO")
      {
       Rcptno = tb.Rows[0]["ReceiptNo"].ToString();
       ViewState["RcptNo"] = Rcptno;
       Session["rcptFee"] = Rcptno;
       insertfeeledger(tb, CrAmt);
      }
      else
      {
        if (tb.Rows.Count <= 0 || !(tb.Rows[0][0].ToString().Trim() == "DUP") && !(tb.Rows[0][0].ToString().Trim() == "NO"))
          return;
       ViewState["Status"] =  tb.Rows[0][0].ToString().Trim();
      }
    }
  }

  private void MiscFee()
  {
    Decimal AvlCredit = new Decimal(0);
    try
    {
      Convert.ToDecimal(txtMiscFee.Text);
    }
    catch
    {
      ScriptManager.RegisterClientScriptBlock((Control)txtamt,txtamt.GetType(), "ShowMessage", "alert('Invalid Amount');", true);
      return;
    }
    DataTable dataTable1 = new DataTable();
    Hashtable hashtable = new Hashtable();
    if (ViewState["RcptNo"] != null)
      hashtable.Add( "receiptno", ViewState["RcptNo"].ToString().Trim());
    hashtable.Add( "Session", drpSession.SelectedValue);
    hashtable.Add( "RecvDate", dtpFeeDt.GetDateValue().ToString("dd MMM yyyy"));
    hashtable.Add( "AdmnNo", txtadmnno.Text);
    hashtable.Add( "Description", txtdesc.Text.Trim());
    hashtable.Add( "Amount",  double.Parse(lblTotalFee.Text.Trim()));
    hashtable.Add( "PaymentMode", drpPmtMode.SelectedValue.ToString());
    if (drpPmtMode.SelectedValue.ToString().Trim() != "Cash")
    {
      hashtable.Add( "ChequeNo", txtChqNo.Text.Trim());
      hashtable.Add( "ChequeDate", txtChqDate.Text.ToString());
      hashtable.Add( "DrawanOnBank", txtBank.Text);
      hashtable.Add( "BankAcHeadId",  int.Parse(drpBankAc.SelectedValue.ToString()));
    }
    hashtable.Add( "UserID",  Convert.ToInt32(Session["User_Id"].ToString().Trim()));
    hashtable.Add( "SchoolID", Session["SchoolId"].ToString());
    hashtable.Add( "@FeeId", drpMiscFee.SelectedValue.Trim().ToString());
    if (rBtnCurrentSess.Checked)
      hashtable.Add( "@CurrSessFee",  1);
    else
      hashtable.Add( "@CurrSessFee",  0);
   obj = new clsDAL();
    DataTable dataTable2 =obj.GetDataTable("Host_Insert_FeeCash_OnMiscReceipt", hashtable);
   ViewState["FeeTable"] =  dataTable2;
    if (dataTable2.Rows.Count > 0 && dataTable2.Rows[0][0].ToString().Trim() != "DUP" && dataTable2.Rows[0][0].ToString().Trim() != "NO")
    {
     Rcptno = dataTable2.Rows[0]["ReceiptNo"].ToString();
     ViewState["RcptNo"] = Rcptno;
     Session["rcptFee"] = Rcptno;
     InsertMiscfeeLedger(dataTable2, AvlCredit);
    }
    else
    {
      if (dataTable2.Rows.Count <= 0 || !(dataTable2.Rows[0][0].ToString().Trim() == "DUP") && !(dataTable2.Rows[0][0].ToString().Trim() == "NO"))
        return;
     ViewState["Status"] =  dataTable2.Rows[0][0].ToString().Trim();
    }
  }

  private void InsertMiscfeeLedger(DataTable tb, Decimal AvlCredit)
  {
    Decimal result1 = new Decimal(0);
    Decimal.TryParse(txtMiscFee.Text, out result1);
    Decimal num1 = result1 + AvlCredit;
    foreach (DataRow row in (InternalDataCollectionBase) tb.Rows)
    {
      Decimal result2 = new Decimal(0);
      Decimal num2 = new Decimal(0);
      Decimal.TryParse(Convert.ToString(row["Balance"]), out result2);
      Decimal CrAmt;
      if (num1 > result2)
      {
        row["balance"] =  0;
        num1 -= result2;
        CrAmt = result2;
      }
      else
      {
        row["balance"] =  (result2 - num1);
        CrAmt = num1;
        num1 = new Decimal(0);
      }
      tb.AcceptChanges();
     InsertCrAmtInLedger(row, CrAmt);
      if (num1 <= new Decimal(0))
        break;
    }
   updatefeeledger(tb, 0);
  }

  private void InsertCrAmtInLedger(DataRow dr, Decimal CrAmt)
  {
    Hashtable hashtable = new Hashtable();
    hashtable.Add( "date",  Convert.ToDateTime(dr["TransDate"]));
    string str1 =dtpFeeDt.GetDateValue().ToString("dd MMM yyyy");
    hashtable.Add( "TransDate",  str1);
    hashtable.Add( "AdmnNo",  Convert.ToInt64(dr["AdmnNo"]));
    hashtable.Add( "TransDesc",  Convert.ToString(dr["TransDesc"]));
    hashtable.Add( "debit",  0);
    hashtable.Add( "credit",  CrAmt);
    hashtable.Add( "balance",  0);
    hashtable.Add( "Receipt_VrNo", Rcptno);
    hashtable.Add( "userid",  Convert.ToInt32(Session["User_Id"].ToString()));
    string str2 = DateTime.Now.ToString("dd MMM yyyy");
    hashtable.Add( "UserDate",  Convert.ToDateTime(str2, (IFormatProvider) CultureInfo.InvariantCulture));
    hashtable.Add( "schoolid", Session["SchoolId"].ToString());
    hashtable.Add( "FeeId",  Convert.ToString(dr["FeeId"]));
    hashtable.Add( "PayMode", drpPmtMode.SelectedValue.ToString().Trim());
    hashtable.Add( "Session",  dr["SessionYr"].ToString());
    hashtable.Add( "AccountHeadDr",  Convert.ToInt32(ViewState["DrAcHead"].ToString()));
    hashtable.Add( "PR_Id",  Convert.ToInt64(dr["PR_Id"]));
   obj = new clsDAL();
   obj.GetDataTable("Host_Insert_FeeLedgerNew", hashtable);
  }

  private void insertfeeledger(DataTable tb, Decimal CrAmt)
  {
    Decimal result1 = new Decimal(0);
    Decimal.TryParse(txtamt.Text, out result1);
    Decimal num1 = result1 + CrAmt;
    foreach (DataRow row in (InternalDataCollectionBase) tb.Rows)
    {
      Decimal result2 = new Decimal(0);
      Decimal num2 = new Decimal(0);
      Decimal.TryParse(Convert.ToString(row["Balance"]), out result2);
      Decimal CrAmt1;
      if (num1 > result2)
      {
        row["balance"] =  0;
        num1 -= result2;
        CrAmt1 = result2;
      }
      else
      {
        row["balance"] =  (result2 - num1);
        CrAmt1 = num1;
        num1 = new Decimal(0);
      }
      tb.AcceptChanges();
     InsertCrAmtInLedger(row, CrAmt1);
      if (num1 <= new Decimal(0))
        break;
    }
   updatefeeledger(tb, 0);
  }

  private void updatefeeledger(DataTable tbcredit, int mode)
  {
    foreach (DataRow row in (InternalDataCollectionBase) tbcredit.Rows)
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

  private void UpdateBalAmtInLedger(DataRow dr, Decimal bal)
  {
    Hashtable hashtable = new Hashtable();
    hashtable.Add( "transno",  Convert.ToInt64(dr["TransNo"]));
    hashtable.Add( "AdmnNo",  Convert.ToInt64(dr["AdmnNo"]));
    hashtable.Add( "VrNo",  dr["receiptno"].ToString());
    hashtable.Add( "FeeId",  Convert.ToInt64(dr["FeeId"]));
    hashtable.Add( "Balance",  bal);
    hashtable.Add( "userid",  Convert.ToInt32(Session["User_Id"].ToString()));
   obj = new clsDAL();
   obj.GetDataTable("Host_Update_FeeLedgerBank", hashtable);
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
   ViewState["OldDue"] =  "0";
   ViewState["PrevDue"] =  "0";
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
    return double.Parse(new clsDAL().ExecuteScalarQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(balance),0)) as balance FROM Host_FeeLedger WHERE admnno='" +drpstudent.SelectedValue.ToString().Trim() + "' and Transdate <=getdate() ")) > 0.0;
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
   drpPmtMode.Focus();
  }

  private void FillSelStudent()
  {
   drpSession.SelectedValue =Request.QueryString["sess"].ToString();
    if (Request.QueryString["cid"].ToString() != "0")
    {
     drpclass.SelectedValue =Request.QueryString["cid"].ToString();
    }
    else
    {
     obj = new clsDAL();
     drpclass.SelectedValue =obj.GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent where AdmnNo=" +Request.QueryString["admnno"].ToString() + " and Detained_Promoted=''").Rows[0]["ClassID"].ToString();
    }
   fillstudent();
   drpstudent.SelectedValue =Request.QueryString["admnno"].ToString().Trim();
   GetStudImg();
  }

  protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
  {
   drpstudent.Items.Clear();
   drpclass.SelectedIndex = 0;
   txtadmnno.Text = string.Empty;
   lblTotalFee.Text = "0.00";
   lblbalance.Text = "0.00";
   txtamt.Text = string.Empty;
   txtRcevMiscFee.Text = string.Empty;
   txtdesc.Text = string.Empty;
   setMiscDate();
   drpSession.Focus();
  }

  protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
  {
   lblMsg.Text = string.Empty;
   lblMsg2.Text = string.Empty;
   ViewState["FY"] =  "n";
   txtadmnno.Text =drpstudent.SelectedValue;
   GetAllFeeDetails();
   drpstudent.Focus();
  }

  protected void GetAllFeeDetails()
  {
   FillFeeMonth(drpMonthPayble);
   FillMiscFeeNames(drpMiscFee);
   FillPaybleMonth();
   FillFeeAmt();
   GetStudImg();
  }

  protected void btnDetail_Click(object sender, EventArgs e)
  {
   lblMsg.Text = string.Empty;
   lblMsg2.Text = string.Empty;
   ViewState["FY"] =  "n";
    try
    {
     obj = new clsDAL();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("select SessionYear,ClassID from dbo.PS_ClasswiseStudent cs");
      stringBuilder.Append(" inner join dbo.Host_Admission ad on ad.AdmnNo=cs.AdmnNo");
      stringBuilder.Append(" inner join dbo.PS_StudMaster sm on sm.AdmnNo=cs.AdmnNo");
      stringBuilder.Append(" where cs.AdmnNo=" +txtadmnno.Text.Trim() + " and cs.Detained_Promoted='' and sm.Status=1 and ad.LeavingDt is null");
      DataTable dataTableQry =obj.GetDataTableQry(stringBuilder.ToString().Trim());
      if (dataTableQry.Rows.Count > 0)
      {
       drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
       drpclass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
       fillstudent();
       drpstudent.SelectedValue =txtadmnno.Text.Trim();
       ViewState["FY"] =  "n";
       GetAllFeeDetails();
      }
      else
      {
        ScriptManager.RegisterClientScriptBlock((Control)txtadmnno,txtadmnno.GetType(), "ShowMessage", "alert('Admission number does not exists')", true);
        return;
      }
    }
    catch (Exception ex)
    {
      string empty = string.Empty;
      string message = ex.Message;
    }
   setMiscDate();
   btnDetail.Focus();
  }

  protected void FillFeeAmt()
  {
   CalculateTotPayble();
  }

  private void CalculateTotPayble()
  {
   ViewState["OldDue"] =  "0";
   ViewState["PrevDue"] =  "0";
   getbalance(txtadmnno.Text.Trim());
   getMiscFee(txtadmnno.Text.Trim());
   txtamt.Text = Convert.ToDouble(lblbalance.Text).ToString();
   txtMiscFee.Text = Convert.ToDouble(lblMiscFee.Text).ToString();
   TotalAmount();
   CalculatePrevDue(txtadmnno.Text.Trim());
  }

  private void CalculatePrevDue(string AdmnNo)
  {
   obj = new clsDAL();
    StringBuilder stringBuilder1 = new StringBuilder();
    string str1 = "04/01/" +drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
    stringBuilder1.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
    stringBuilder1.Append(" from Host_FeeLedger where admnno='" + AdmnNo + "'  and TransDate < '" + str1 + "' AND TransDesc<>'Previous Balance'");
    string str2 =obj.ExecuteScalarQry(stringBuilder1.ToString().Trim());
   lblPrevYrBal.Text = "Previous Session Due :- Rs. " + string.Format("{0:f2}",  Convert.ToDouble(str2));
    StringBuilder stringBuilder2 = new StringBuilder();
    stringBuilder2.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
    stringBuilder2.Append(" from Host_FeeLedger where admnno='" + AdmnNo + "'  and TransDate < '" + str1 + "' AND TransDesc='Previous Balance'");
    string str3 =obj.ExecuteScalarQry(stringBuilder2.ToString().Trim());
   lblOldDues.Text = "Old Dues (Before Computerisation):- Rs. " + string.Format("{0:f2}",  Convert.ToDouble(str3));
    if (Convert.ToDouble(str3) > 0.0)
    {
     btnRcvOldDue.Visible = true;
     ViewState["OldDue"] =  str3;
    }
    else
    {
     ViewState["OldDue"] =  "0";
     btnRcvOldDue.Visible = false;
    }
   ViewState["PrevDue"] =  Convert.ToDouble(str2);
   drpMonthPayble_SelectedIndexChanged(drpMonthPayble, EventArgs.Empty);
  }

  private void getMiscFee(string AdmnNo)
  {
   obj = new clsDAL();
    StringBuilder stringBuilder = new StringBuilder();
    if (rBtnCurrentSess.Checked)
    {
     ViewState["Misc"] =  "n";
      string currSession = new clsGenerateFee().CreateCurrSession();
     lblMiscFee.Text =obj.GetDataTable("Host_GetStudMiscBalAmnt", new Hashtable()
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
      string str = "04/01/" +drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
      stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
      stringBuilder.Append(" from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where PeriodicityID=6 AND admnno='" + AdmnNo + "'  and TransDate < '" + str + "' and TransDesc<>'Previous Balance'");
     lblMiscFee.Text =obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
    }
  }

  private void getbalance(string AdmnNo)
  {
   obj = new clsDAL();
    StringBuilder stringBuilder = new StringBuilder();
    if (rBtnCurrentSess.Checked)
    {
     ViewState["FY"] =  "n";
     lblbalance.Text =obj.GetDataTable("Host_GetStudCurrentBalAmnt", new Hashtable()
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
    }
    else
    {
      string str = "04/01/" +drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
      stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(Balance),0)) as balance");
      stringBuilder.Append(" from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where PeriodicityID<>6 AND admnno='" + AdmnNo + "'  and TransDate < '" + str + "' and TransDesc<>'Previous Balance'");
     lblbalance.Text =obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
    }
  }

  private void GetBalFullSession(string AdmnNo)
  {
   obj = new clsDAL();
   lblbalance.Text = !rBtnCurrentSess.Checked.Equals(true) ?obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" +PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' AND PeriodicityID<>6 and admnno=" + AdmnNo) :obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" +drpSession.SelectedValue.ToString().Trim() + "' AND PeriodicityID<>6 and admnno=" + AdmnNo);
    string text =lblbalance.Text;
   drpMonthPayble.SelectedValue = "3";
  }

  protected void btnFeeFullYr_Click(object sender, EventArgs e)
  {
   FillFeeMonth(drpMonthPayble);
   FillPaybleMonth();
   obj = new clsDAL();
    string str =obj.ExecuteScalarQry("select ad.admnno from dbo.PS_ClasswiseStudent c inner join dbo.Host_Admission ad on c.admnno=ad.AdmnNo where c.admnno=" +txtadmnno.Text.ToString().Trim());
    if (str != "")
    {
      if (drpstudent.SelectedValue.ToString().Trim() !=txtadmnno.Text.ToString().Trim())
      {
       fillstudent();
       drpstudent.SelectedValue = str;
      }
     GetBalFullSession(txtadmnno.Text.ToString().Trim());
      if (rBtnCurrentSess.Checked.Equals(true))
       ViewState["FY"] =  "y";
      else
       ViewState["FY"] =  "n";
     txtamt.Text = Convert.ToDouble(lblbalance.Text.Trim()).ToString();
     TotalAmount();
    }
    else
      ScriptManager.RegisterClientScriptBlock((Page) this,GetType(), "ShowMassage", "alert('Invalid Admission Number');", true);
   btnFeeFullYr.Focus();
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
              ScriptManager.RegisterClientScriptBlock((Page) this,GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" +txtadmnno.Text.Trim() + " &sess=" +drpSession.SelectedValue.ToString().Trim() + "&class=" +drpclass.SelectedValue.ToString() + " &FY=" +ViewState["FY"].ToString().Trim() + "&Sw=" +Request.QueryString["Sw"] + "&Due=C',null,'width=700,height=500');", true);
            else
              ScriptManager.RegisterClientScriptBlock((Page) this,GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" +txtadmnno.Text.Trim() + " &sess=" +drpSession.SelectedValue.ToString().Trim() + "&class=" +drpclass.SelectedValue.ToString() + " &FY=" +ViewState["FY"].ToString().Trim() + "&Sw=" +Request.QueryString["Sw"] + "&Due=C&AdmnFee=T',null,'width=700,height=500');", true);
          }
          else
            ScriptManager.RegisterClientScriptBlock((Page) this,GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" +txtadmnno.Text.Trim() + " &sess=" +drpSession.SelectedValue.ToString().Trim() + "&class=" +drpclass.SelectedValue.ToString() + " &FY=" +ViewState["FY"].ToString().Trim() + "&Sw=" +Request.QueryString["Sw"] + "&Due=P',null,'width=700,height=500');", true);
        }
        else if (rBtnCurrentSess.Checked)
        {
          if (drpMonthPayble.SelectedIndex > 1)
            ScriptManager.RegisterClientScriptBlock((Page) this,GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" +txtadmnno.Text.Trim() + " &sess=" +drpSession.SelectedValue.ToString().Trim() + "&class=" +drpclass.SelectedValue.ToString() + " &FY=" +ViewState["FY"].ToString().Trim() + "&Due=C',null,'width=700,height=500');", true);
          else
            ScriptManager.RegisterClientScriptBlock((Page) this,GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" +txtadmnno.Text.Trim() + " &sess=" +drpSession.SelectedValue.ToString().Trim() + "&class=" +drpclass.SelectedValue.ToString() + " &FY=" +ViewState["FY"].ToString().Trim() + "&Due=C&AdmnFee=T',null,'width=700,height=500');", true);
        }
        else if (drpMonthPayble.SelectedIndex > 1)
          ScriptManager.RegisterClientScriptBlock((Page) this,GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" +txtadmnno.Text.Trim() + " &sess=" +drpSession.SelectedValue.ToString().Trim() + "&class=" +drpclass.SelectedValue.ToString() + " &FY=" +ViewState["FY"].ToString().Trim() + "&Due=P',null,'width=700,height=500');", true);
        else
          ScriptManager.RegisterClientScriptBlock((Page) this,GetType(), "ShowMessage", "window.open('../Hostel/rptHostFeeDueDtls.aspx?admnno=" +txtadmnno.Text.Trim() + " &sess=" +drpSession.SelectedValue.ToString().Trim() + "&class=" +drpclass.SelectedValue.ToString() + " &FY=" +ViewState["FY"].ToString().Trim() + "&Due=P&AdmnFee=T',null,'width=700,height=500');", true);
       btnShowDetails.Focus();
      }
      else
        ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails,btnShowDetails.GetType(), "ShowMessage", "alert('No dues for the selected Student');", true);
    }
    else
      ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails,btnShowDetails.GetType(), "ShowMessage", "alert('Please Select a Student');", true);
  }

  protected void btnInstDisc_Click(object sender, EventArgs e)
  {
    if (Request.QueryString["Sw"] != null)
     Response.Redirect("FeeAdjustment.aspx?admnno=" +txtadmnno.Text.Trim() + "&sess=" +drpSession.SelectedValue.ToString().Trim() + "&cid=" +drpclass.SelectedValue.ToString() + "&Sw=" +Request.QueryString["Sw"]);
    else
     Response.Redirect("FeeAdjustment.aspx?admnno=" +txtadmnno.Text.Trim() + "&sess=" +drpSession.SelectedValue.ToString().Trim() + "&cid=" +drpclass.SelectedValue.ToString());
   btnInstDisc.Focus();
  }

  protected void drpMonthPayble_SelectedIndexChanged(object sender, EventArgs e)
  {
   obj = new clsDAL();
    if (drpMonthPayble.SelectedIndex > 0)
    {
      if (drpMonthPayble.SelectedValue.ToString().Trim() == "0")
      {
       GetBalFeeOnAdmn(txtadmnno.Text.ToString().Trim());
       ViewState["FY"] =  "m";
       txtamt.Text = Convert.ToDouble(lblbalance.Text.Trim()).ToString();
      }
      else
      {
       getMonthbalance(txtadmnno.Text.ToString().Trim());
       ViewState["FY"] =  "m";
       txtamt.Text = Convert.ToString(Convert.ToDouble(ViewState["Bal"]) + 0.0);
      }
     TotalAmount();
    }
    else
    {
     txtamt.Text = "0.00";
     lblbalance.Text = "0.00";
     TotalAmount();
    }
   drpMonthPayble.Focus();
  }

  private void TotalAmount()
  {
   lblTotalFee.Text = string.Format("{0:f2}",  (Convert.ToDouble(txtamt.Text) + Convert.ToDouble(txtMiscFee.Text)));
  }

  protected void btnTillDec_Click(object sender, EventArgs e)
  {
   obj = new clsDAL();
    if (drpMonthPayble.SelectedIndex > 0)
    {
      if (drpMonthPayble.SelectedValue.ToString().Trim() == "0")
      {
       GetBalFeeOnAdmn(txtadmnno.Text.ToString().Trim());
       ViewState["FY"] =  "m";
       txtamt.Text = Convert.ToDouble(lblbalance.Text.Trim()).ToString();
       TotalAmount();
      }
      else
      {
       getMonthbalance(txtadmnno.Text.ToString().Trim());
       ViewState["FY"] =  "m";
       txtamt.Text = Convert.ToString(Convert.ToDouble(ViewState["Bal"]) + 0.0);
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
    string str1 =drpMonthPayble.SelectedValue.ToString();
    string str2 = (Convert.ToInt32(drpMonthPayble.SelectedValue.ToString().Trim()) + 1).ToString();
    string str3 =drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
    if (rBtnCurrentSess.Checked.Equals(false))
      str3 = (Convert.ToInt32(str3) - 1).ToString().Trim();
    string str4 =drpSession.SelectedValue.ToString().Trim().Substring(0, 2) +drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
    if (rBtnCurrentSess.Checked.Equals(false))
      str4 = (Convert.ToInt32(str4) - 1).ToString().Trim();
    string str5 = Convert.ToInt32(str1) <= 3 ? str2 + "/01/" + str4 : (Convert.ToInt32(str1) <= 11 ? str2 + "/01/" + str3 : "01/01/" + str4);
   Session["FeeMonth"] =  str5;
   obj = new clsDAL();
    if (rBtnCurrentSess.Checked.Equals(true))
    {
     // "select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" +drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6";
     lblbalance.Text =obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" +drpSession.SelectedValue.ToString().Trim() + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6");
     ViewState["Bal"] = lblbalance.Text;
    }
    else
    {
     lblbalance.Text =obj.ExecuteScalarQry("select Convert(Decimal(18,2),isnull(sum(balance),0)) as balance from Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID where sessionYr='" +PrevYr(drpSession.SelectedValue.ToString().Trim()) + "' and admnno=" + AdmnNo + "  and TransDate < '" + str5 + "' AND PeriodicityID<>6");
     ViewState["Bal"] = lblbalance.Text;
    }
    string text =lblbalance.Text;
  }

  private void GetBalFeeOnAdmn(string AdmnNo)
  {
   obj = new clsDAL();
    StringBuilder stringBuilder = new StringBuilder();
    if (rBtnCurrentSess.Checked.Equals(true))
    {
      stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(balance),0)) from Host_FeeLedger where admnno='" + AdmnNo + "' and balance > 0");
      stringBuilder.Append(" and feeid in(select distinct feeid from dbo.Host_FeeComponents where PeriodicityID in (1,2)) and SessionYr='" +drpSession.SelectedValue.ToString().Trim() + "'");
    }
    else
    {
      stringBuilder.Append("select Convert(Decimal(18,2),isnull(sum(balance),0)) from Host_FeeLedger where admnno='" + AdmnNo + "' and balance > 0");
      stringBuilder.Append(" and feeid in(select distinct feeid from dbo.Host_FeeComponents where PeriodicityID in (1,2)) and sessionYr='" +PrevYr(drpSession.SelectedValue.ToString().Trim()) + "'");
    }
   lblbalance.Text =obj.ExecuteScalarQry(stringBuilder.ToString().Trim());
    string text =lblbalance.Text;
  }

  protected void btnMiscDtl_Click(object sender, EventArgs e)
  {
    if (txtadmnno.Text != "")
    {
      if (rBtnCurrentSess.Checked)
        ScriptManager.RegisterClientScriptBlock((Page) this,GetType(), "ShowMessage", "window.open('../Hostel/rptHostMiscDueDetails.aspx?admino=" +txtadmnno.Text.Trim() + "&Fid=" +drpMiscFee.SelectedValue.ToString().Trim() + "&sess=" +drpSession.SelectedValue.ToString().Trim() + "&Due=C',null,'width=700,height=500');", true);
      else
        ScriptManager.RegisterClientScriptBlock((Page) this,GetType(), "ShowMessage", "window.open('../Hostel/rptHostMiscDueDetails.aspx?admino=" +txtadmnno.Text.Trim() + "&Fid=" +drpMiscFee.SelectedValue.ToString().Trim() + "&sess=" +PrevYr(drpSession.SelectedValue.ToString().Trim()) + "&Due=P',null,'width=700,height=500');", true);
     btnMiscDtl.Focus();
    }
    else
      ScriptManager.RegisterClientScriptBlock((Control)btnShowDetails,btnShowDetails.GetType(), "ShowMessage", "alert('Enter Admission Number');", true);
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

  protected void btnMiscFee_Click(object sender, EventArgs e)
  {
    string str1 =drpSession.SelectedValue.Trim();
    DateTime dateTime1 = Convert.ToDateTime("31 Mar" + str1.Substring(0, 4));
    DateTime dateTime2 = Convert.ToDateTime("1 Apr" + Convert.ToString(Convert.ToInt32(str1.Substring(0, 4)) + 1));
    if (DateTime.Today < dateTime2 && DateTime.Today > dateTime1 &&dtpMiscDt.GetDateValue().Date > DateTime.Today)
    {
     lblMsg.Text =lblMsg2.Text = "Misc Fee Date cannot be greater than Current Date.";
     lblMsg.ForeColor = Color.Red;
     lblMsg2.ForeColor = Color.Red;
    }
    else if (dtpMiscDt.GetDateValue().Date >= dateTime2 ||dtpMiscDt.GetDateValue().Date <= dateTime1 &&rBtnCurrentSess.Checked)
    {
     lblMsg.Text =lblMsg2.Text = "Misc Fee Date not in the selected Session Year.";
     lblMsg.ForeColor = Color.Red;
     lblMsg2.ForeColor = Color.Red;
    }
    else
    {
      clsDAL clsDal = new clsDAL();
      string str2 = clsDal.ExecuteScalar("Host_InsAdditionalFeeChk", new Hashtable()
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
        hashtable.Add( "@TransDate", dtpMiscDt.GetDateValue().ToString("dd MMM yyyy"));
        hashtable.Add( "@AdmnNo", txtadmnno.Text.ToString().Trim());
        hashtable.Add( "@TransDesc", drpAdditionalFee.SelectedItem.Text);
        hashtable.Add( "@Debit", txtRcevMiscFee.Text.Trim());
        hashtable.Add( "@Credit",  "0.0000");
        hashtable.Add( "@Balance", txtRcevMiscFee.Text.Trim());
        hashtable.Add( "@UserID",Session["User_Id"]);
        hashtable.Add( "@AdFeeId", drpAdditionalFee.SelectedValue.ToString().Trim());
        hashtable.Add( "@SchoolId", Session["SchoolId"].ToString());
        DataTable dataTable2 = clsDal.GetDataTable("Host_InsAdditionalFee", hashtable);
        if (dataTable2.Rows.Count <= 0 && str2.Trim() == "Yes")
        {
         lblMsg.Text =lblMsg2.Text = "Misc Fee Updated Successfully";
         lblMsg.ForeColor =lblMsg2.ForeColor = Color.Green;
        }
        else if (dataTable2.Rows.Count <= 0)
        {
         lblMsg.Text =lblMsg2.Text = "Misc Fee Added Successfully";
         lblMsg.ForeColor =lblMsg2.ForeColor = Color.Green;
        }
       drpAdditionalFee.SelectedIndex = 0;
       txtRcevMiscFee.Text = "0";
       setMiscDate();
      }
      else if (str2.Trim() == "No")
      {
       lblMsg.Text =lblMsg2.Text = "Cannot Update Misc Fee. Fee already Received on this or later date.";
       lblMsg.ForeColor =lblMsg2.ForeColor = Color.Red;
        return;
      }
     FillMiscFeeNames(drpMiscFee);
     FillPaybleMonth();
     FillFeeAmt();
     btnMiscFee.Focus();
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
    DataTable dataTableQry = clsDal.GetDataTableQry("SELECT DISTINCT FL.FeeId,FC.FeeName FROM dbo.Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FC.FeeID=FL.FeeID WHERE AdmnNo=" +drpstudent.SelectedValue.ToString().Trim() + " AND Balance>0 AND PeriodicityID=6");
    drpMiscFee.DataSource =  dataTableQry;
    drpMiscFee.DataTextField = "FeeName";
    drpMiscFee.DataValueField = "FeeId";
    drpMiscFee.DataBind();
    drpMiscFee.Items.Insert(0, new ListItem("- ALL -", "0"));
  }

  private string PrevYr(string p)
  {
    int int32_1 = Convert.ToInt32(drpSession.SelectedValue.ToString().Trim().Substring(0, 4));
    int num = int32_1 - 1;
    int int32_2 = Convert.ToInt32(int32_1.ToString().Trim().Substring(2));
    string empty = string.Empty;
    return Convert.ToString(num) + "-" + Convert.ToString(int32_2);
  }

  protected void drpAdditionalFee_SelectedIndexChanged(object sender, EventArgs e)
  {
   txtRcevMiscFee.Text = new clsDAL().ExecuteScalarQry("select CAST(Amount AS DECIMAL(10,2)) AS Amount from dbo.Host_FeeAmount where FeeCompId=" +drpAdditionalFee.SelectedValue + " and StreamId=(select StreamID from dbo.PS_ClassMaster where ClassID=" +drpclass.SelectedValue.ToString() + ")");
   drpAdditionalFee.Focus();
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
    if (new clsGenerateFee().CreateCurrSession() !=drpSession.SelectedValue.ToString().Trim())
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

  protected void rBtnCurrentSess_CheckedChanged(object sender, EventArgs e)
  {
   setMiscDate();
    if (txtadmnno.Text.Trim() != string.Empty)
     GetAllFeeDetails();
   drpSession.Focus();
  }

  protected void btnRcvOldDue_Click(object sender, EventArgs e)
  {
    if (Request.QueryString["Sw"] != null)
     Response.Redirect("HostRcptPrevDue.aspx?admnno=" +txtadmnno.Text.Trim() + "&sess=" +drpSession.SelectedValue.ToString().Trim() + "&cid=" +drpclass.SelectedValue.ToString() + "&Sw=" +Request.QueryString["Sw"]);
    else
     Response.Redirect("HostRcptPrevDue.aspx?admnno=" +txtadmnno.Text.Trim() + "&sess=" +drpSession.SelectedValue.ToString().Trim() + "&cid=" +drpclass.SelectedValue.ToString());
  }

  protected void drpMiscFee_SelectedIndexChanged(object sender, EventArgs e)
  {
    clsDAL clsDal = new clsDAL();
    DataTable dataTable = new DataTable();
    DataTable dataTableQry;
    if (rBtnCurrentSess.Checked.Equals(true))
    {
      string str =drpSession.SelectedValue.Trim();
      DateTime dateTime1 = Convert.ToDateTime("1 Apr " + str.Substring(0, 4));
      DateTime dateTime2 = Convert.ToDateTime("31 Mar " + Convert.ToString(Convert.ToInt32(str.Substring(0, 4)) + 1));
      if (drpMiscFee.SelectedIndex > 0)
      {
        dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger WHERE AdmnNo=" +drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" +drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" +drpSession.SelectedValue.Trim() + "' AND TransDate <= GETDATE()");
        if (DateTime.Today < dateTime2 && DateTime.Today < dateTime1)
          dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger WHERE AdmnNo=" +drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" +drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" +drpSession.SelectedValue.Trim() + "' AND TransDate >='1 Apr " + str.Substring(0, 4) + "'");
      }
      else
      {
        dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" +drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" +drpSession.SelectedValue.Trim() + "' AND TransDate <= GETDATE()");
        if (DateTime.Today < dateTime2 && DateTime.Today < dateTime1)
          dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" +drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" +drpSession.SelectedValue.Trim() + "' AND TransDate >='1 Apr " + str.Substring(0, 4) + "'");
      }
    }
    else if (drpMiscFee.SelectedIndex > 0)
      dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger WHERE AdmnNo=" +drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" +drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" +PrevYr(drpSession.SelectedValue.Trim()) + "' AND TransDate <= GETDATE()");
    else
      dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" +drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" +PrevYr(drpSession.SelectedValue.Trim()) + "' AND TransDate <= GETDATE()");
   lblMiscFee.Text = dataTableQry.Rows[0][0].ToString();
   txtMiscFee.Text =lblMiscFee.Text;
   TotalAmount();
   drpMiscFee.Focus();
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
      str1 =drpMonthPayble.SelectedValue.ToString();
      str2 = (Convert.ToInt32(drpMonthPayble.SelectedValue.ToString().Trim()) + 1).ToString();
    }
    string str3 =drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
    if (rBtnCurrentSess.Checked.Equals(false))
      str3 = (Convert.ToInt32(str3) - 1).ToString().Trim();
    string str4 =drpSession.SelectedValue.ToString().Trim().Substring(0, 2) +drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
    if (rBtnCurrentSess.Checked.Equals(false))
      str4 = (Convert.ToInt32(str4) - 1).ToString().Trim();
    string str5 = Convert.ToInt32(str1) <= 3 ? str2 + "/01/" + str4 : (Convert.ToInt32(str1) <= 11 ? str2 + "/01/" + str3 : "01/01/" + str4);
    clsDAL clsDal = new clsDAL();
    DataTable dataTable = new DataTable();
    DataTable dataTableQry;
    if (rBtnCurrentSess.Checked.Equals(true))
    {
      if (drpMiscFee.SelectedIndex > 0)
        dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger WHERE AdmnNo=" +drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" +drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" +drpSession.SelectedValue.Trim() + "' AND TransDate <'" + str5 + "'");
      else
        dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" +drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" +drpSession.SelectedValue.Trim() + "' AND TransDate <'" + str5 + "'");
    }
    else if (drpMiscFee.SelectedIndex > 0)
      dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger WHERE AdmnNo=" +drpstudent.SelectedValue.Trim().ToString() + "  AND FeeID=" +drpMiscFee.SelectedValue.ToString().Trim() + " AND SessionYr='" +PrevYr(drpSession.SelectedValue.Trim()) + "' AND TransDate <'" + str5 + "'");
    else
      dataTableQry = clsDal.GetDataTableQry("SELECT CONVERT(DECIMAL(18,2),ISNULL(SUM(Balance),0)) AS Balance FROM dbo.Host_FeeLedger FL INNER JOIN dbo.Host_FeeComponents FC ON FL.FeeId=FC.FeeID WHERE AdmnNo=" +drpstudent.SelectedValue.Trim().ToString() + " AND PeriodicityID=6  AND SessionYr='" +PrevYr(drpSession.SelectedValue.Trim()) + "' AND TransDate <'" + str5 + "'");
   lblMiscFee.Text = dataTableQry.Rows[0][0].ToString();
   txtMiscFee.Text =lblMiscFee.Text;
  }

  private void statusOnFail()
  {
    if (ViewState["RcptNo"] != null)
     DelFeeOnFail(ViewState["RcptNo"].ToString().Trim());
    if (ViewState["Status"].ToString().Trim() == "DUP")
    {
     lblMsg.Text = "Receipt No already Exists.";
     lblMsg.ForeColor = Color.Red;
    }
    else
    {
     lblMsg.Text = "Transaction Failed. Try Again.";
     lblMsg.ForeColor = Color.Red;
    }
  }

  private void DelFeeOnFail(string RecptNo)
  {
    Hashtable hashtable = new Hashtable();
    clsDAL clsDal1 = new clsDAL();
    hashtable.Add( "receiptno",  RecptNo);
    DataTable dataTable1 = new DataTable();
    clsDAL clsDal2 = new clsDAL();
    DataTable dataTable2 = clsDal1.GetDataTable("Host_DeleteFeeOnTransFail", hashtable);
    if (dataTable2.Rows.Count <= 0)
      return;
   updatefeeledger(dataTable2, 0);
  }

  private void updatefeeledger(DataTable tb, Decimal CrAmount)
  {
    Convert.ToDecimal(tb.Rows[0]["oldamount"].ToString());
    Decimal num1 = new Decimal(0);
    clsDAL clsDal = new clsDAL();
    foreach (DataRow row in (InternalDataCollectionBase) tb.Rows)
    {
      Decimal result = new Decimal(0);
      Decimal num2 = new Decimal(0);
      Decimal.TryParse(Convert.ToString(row["Credit"]), out result);
      row["Credit"] =  0;
      Decimal bal = result;
      tb.AcceptChanges();
     UpdateBalAmtInLedger(row, bal);
      num1 += bal;
    }
   updatefeeledgerDel(tb, 0);
  }

  private void updatefeeledgerDel(DataTable tbcredit, int mode)
  {
    clsDAL clsDal = new clsDAL();
    foreach (DataRow row in (InternalDataCollectionBase) tbcredit.Rows)
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
         Session["User_Id"]
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
}