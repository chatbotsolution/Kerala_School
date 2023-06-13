using AjaxControlToolkit;
using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_InitStudentAC : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       lblmsg.Text = string.Empty;
       btnUpdate.Enabled = false;
        if (Page.IsPostBack)
            return;
       FillSessionDropdown();
       FillClassDropDown();
       lblRecCount.Text = "";
       grdStudAC.Visible = false;
       PopCalendar3.SetDateValue(Convert.ToDateTime("31 Mar" + drpSession.SelectedValue.ToString().Substring(0, 4)));
    }

    private void FillClassDropDown()
    {
       drpClass.DataSource = new Common().GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
       drpClass.DataTextField = "ClassName";
       drpClass.DataValueField = "ClassID";
       drpClass.DataBind();
       drpClass.Items.Insert(0, new ListItem("Select", "0"));
    }

    private void FillSessionDropdown()
    {
       drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
       drpSession.DataTextField = "SessionYr";
       drpSession.DataValueField = "SessionYr";
       drpSession.DataBind();
    }

    private void FillSectionDropDown()
    {
       drpSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "classId",
        drpClass.SelectedValue
      },
      {
         "session",
        drpSession.SelectedValue
      }
    });
       drpSection.DataTextField = "Section";
       drpSection.DataValueField = "Section";
       drpSection.DataBind();
       drpSection.Items.Insert(0, new ListItem("ALL", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
       grdStudAC.Visible = false;
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
       grdStudAC.Visible = false;
       btnUpdate.Enabled = false;
       gvInitializedAc.Visible = false;
       btnDelete.Visible = false;
       FillSectionDropDown();
       lblRecCount.Text = string.Empty;
       FillSelectStudent();
    }

    private void FillSelectStudent()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable ht = new Hashtable();
        if (drpSession.Items.Count > 0)
            ht.Add("@SessionYear", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            ht.Add("@ClassID", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            ht.Add("@Section", drpSection.SelectedValue);
       drpSelectStudent.DataSource = clsDal.GetDataTable("ps_GetStudForInit", ht);
       drpSelectStudent.DataTextField = "fullname";
       drpSelectStudent.DataValueField = "admnno";
       drpSelectStudent.DataBind();
       drpSelectStudent.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSection.SelectedIndex > 0)
           FillSelectStudent();
        else
           drpSelectStudent.Items.Clear();
       grdStudAC.Visible = false;
       lblRecCount.Text = string.Empty;
    }

    protected void drpSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSelectStudent.SelectedIndex <= 0)
            return;
       FillGrid(grdStudAC, "ps_sp_get_StudListForAcInit");
       gvInitializedAc.Visible = false;
       btnDelete.Visible = false;
    }

    private void IntializePage()
    {
       drpClass.SelectedIndex = -1;
       drpSession.SelectedIndex = -1;
       grdStudAC.Visible = false;
       txtDate.Text = "";
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
       btnDelete.Visible = false;
       grdStudAC.Visible = true;
       gvInitializedAc.Visible = false;
       FillGrid(grdStudAC, "ps_sp_get_StudListForAcInit");
    }

    private void FillGrid(GridView gv, string procName)
    {
        Common common = new Common();
        Hashtable ht = new Hashtable();
        if (drpSession.Items.Count > 0)
            ht.Add("@Session", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            ht.Add("@Class", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            ht.Add("@Section", drpSection.SelectedValue);
        if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
            ht.Add("@AdmnNo", drpSelectStudent.SelectedValue.ToString());
        DataTable dataTable = common.GetDataTable(procName, ht);
        if (dataTable.Rows.Count > 0)
        {
            gv.DataSource = dataTable;
            gv.DataBind();
            gv.Visible = true;
           lblRecCount.Text = "<font color='maroon'><b>No. of Student(s): " + dataTable.Rows.Count.ToString() + "</b></font>";
           lblRecCount.Visible = true;
           btnUpdate.Enabled = true;
        }
        else
        {
            gv.Visible = false;
           btnUpdate.Enabled = false;
           lblRecCount.Text = "";
           lblmsg.Text = "No Record Found";
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string str1 = "";
        foreach (GridViewRow row in grdStudAC.Rows)
        {
            Decimal num1 = new Decimal(0);
            Decimal num2 = new Decimal(0);
            Decimal num3 = new Decimal(0);
            Decimal num4 = new Decimal(0);
            Decimal num5;
            Decimal num6;
            Decimal num7;
            try
            {
                num5 = Convert.ToDecimal(((TextBox)row.Cells[4].FindControl("txtFeeAmount")).Text.ToString().Trim());
                num6 = Convert.ToDecimal(((TextBox)row.Cells[4].FindControl("txtBusAmount")).Text.ToString().Trim());
                num7 = Convert.ToDecimal(((TextBox)row.Cells[4].FindControl("txtHostAmount")).Text.ToString().Trim());
            }
            catch (Exception ex)
            {
                num5 = new Decimal(0);
                num6 = new Decimal(0);
                num7 = new Decimal(0);
            }
            Decimal num8 = num5 + num6 + num7;
            Label control = (Label)row.FindControl("lblAdmnNo");
            string str2 = "111" + control.Text.ToString();
            if (num8 > new Decimal(0))
            {
                Hashtable ht = new Hashtable();
               ViewState["recptno"] = str2;
                ht.Add("receiptno", str2);
                ht.Add("Session", drpSession.SelectedValue);
                ht.Add("RecvDate", PopCalendar3.GetDateValue().ToString("MM/dd/yyyy"));
                ht.Add("AdmnNo", control.Text.ToString());
                ht.Add("Class", drpClass.SelectedValue);
                ht.Add("Description", "AC Initialized");
                ht.Add("Amount", num8);
                ht.Add("PaymentMode", "Cash");
                ht.Add("UserID", Convert.ToInt32(Session["User_Id"].ToString().Trim()));
                ht.Add("SchoolID", Session["SchoolId"].ToString());
                DataSet dataSet = new clsDAL().GetDataSet("ps_sp_insert_Initfeecash", ht);
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows[0][0].ToString().Trim() != "Receipt No already Exists")
                {
                    if (num5 > new Decimal(0))
                       insertfeeledger(dataSet.Tables[0], num5);
                    if (dataSet.Tables[1].Rows.Count > 0 && num6 > new Decimal(0))
                       insertBusHostFee(dataSet.Tables[1], num6);
                    if (dataSet.Tables[2].Rows.Count > 0 && num7 > new Decimal(0))
                       insertBusHostFee(dataSet.Tables[2], num7);
                   insertGenLedger(num5, num6, num7);
                }
                else
                {
                    str1 = "No";
                   lblmsg.ForeColor = Color.Red;
                   lblmsg.Text = "Receipt No already Exists !";
                    break;
                }
            }
            else
            {
                str1 = "Some";
                if (drpSelectStudent.SelectedIndex > 0)
                {
                    str1 = "No";
                   lblmsg.ForeColor = Color.Red;
                   lblmsg.Text = "Please Enter Fee Amount!";
                }
            }
        }
       FillGrid(grdStudAC, "ps_sp_get_StudListForAcInit");
        if (str1 == "")
        {
           lblmsg.ForeColor = Color.Green;
           lblmsg.Text = "A/c initialized successfully !";
        }
        else
        {
            if (!(str1 == "Some"))
                return;
           lblmsg.ForeColor = Color.Red;
           lblmsg.Text = "Some Students Could Not Be Initialized!! Please Enter Fee Amount To Initialize!";
        }
    }

    private void insertfeeledger(DataTable tb, Decimal amt)
    {
        try
        {
            foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
            {
                Decimal result = new Decimal(0);
                Decimal num = new Decimal(0);
                Decimal.TryParse(Convert.ToString(row["Balance"]), out result);
                Decimal CrAmt;
                if (amt > result)
                {
                    row["balance"] = 0;
                    amt -= result;
                    CrAmt = result;
                }
                else
                {
                    row["balance"] = (result - amt);
                    CrAmt = amt;
                    amt = new Decimal(0);
                }
                tb.AcceptChanges();
               InsertCrAmtInLedger(row, CrAmt);
                if (amt <= new Decimal(0))
                    break;
            }
           updatefeeledgerAcInit(tb, 0);
        }
        catch (Exception ex)
        {
           lblmsg.ForeColor = Color.Red;
           lblmsg.Text = ex.ToString();
        }
    }

    private void InsertCrAmtInLedger(DataRow dr, Decimal CrAmt)
    {
        new clsDAL().GetDataTable("ps_sp_insert_FeeLedgerNew", new Hashtable()
    {
      {
         "date",
         Convert.ToDateTime(dr["TransDate"])
      },
      {
         "TransDate",
        PopCalendar3.GetDateValue().ToString("MM/dd/yyyy")
      },
      {
         "AdmnNo",
         Convert.ToInt64(dr["AdmnNo"])
      },
      {
         "TransDesc",
         "AC Initialized"
      },
      {
         "debit",
         0
      },
      {
         "credit",
         CrAmt
      },
      {
         "balance",
         0
      },
      {
         "Receipt_VrNo",
        ViewState["recptno"].ToString()
      },
      {
         "userid",
         Convert.ToInt32(Session["User_Id"].ToString())
      },
      {
         "UserDate",
         DateTime.Now.ToString("MM/dd/yyyy")
      },
      {
         "schoolid",
        Session["SchoolId"].ToString()
      },
      {
         "FeeId",
         Convert.ToString(dr["FeeId"])
      },
      {
         "PayMode",
         "Cash"
      },
      {
         "Session",
        drpSession.SelectedValue.ToString().Trim()
      },
      {
         "AccountHeadDr",
         3
      },
      {
         "PR_Id",
         Convert.ToInt64(dr["PR_Id"])
      }
    });
    }

    private void insertBusHostFee(DataTable tb, Decimal amt)
    {
        try
        {
            foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
            {
                Decimal result = new Decimal(0);
                Decimal num1 = new Decimal(0);
                Decimal.TryParse(Convert.ToString(row["Balance"]), out result);
                Decimal num2;
                if (amt > result)
                {
                    row["balance"] = 0;
                    amt -= result;
                    num2 = result;
                }
                else
                {
                    row["balance"] = (result - amt);
                    num2 = amt;
                    amt = new Decimal(0);
                }
                tb.AcceptChanges();
                Hashtable ht = new Hashtable();
                ht.Clear();
                ht.Add("@Ad_Id", row["Ad_Id"].ToString().Trim());
                ht.Add("@AdFeeNo", row["AdFeeNo"].ToString().Trim());
                ht.Add("@RcptNo", row["ReceiptNo"].ToString().Trim());
                ht.Add("@AdMonth", row["AdMonth"].ToString().Trim());
                ht.Add("@Balance", row["balance"].ToString().Trim());
                ht.Add("@Credit", num2);
                ht.Add("userid", Convert.ToInt32(Session["User_Id"].ToString()));
                new clsDAL().ExecuteScalar("ps_sp_insert_BusHostlInit", ht);
                if (amt <= new Decimal(0))
                    break;
            }
        }
        catch (Exception ex)
        {
           lblmsg.ForeColor = Color.Red;
           lblmsg.Text = ex.ToString();
        }
    }

    private void insertGenLedger(Decimal Amnt, Decimal busFee, Decimal hostFee)
    {
        if (ViewState["recptno"] == null)
            return;
        clsDAL clsDal = new clsDAL();
        Hashtable ht = new Hashtable();
        ht.Add("@Receipt_VrNo", ViewState["recptno"].ToString().Trim());
        if (Amnt > new Decimal(0))
            ht.Add("@Amount", Amnt);
        if (busFee > new Decimal(0))
            ht.Add("@BusFee", busFee);
        if (hostFee > new Decimal(0))
            ht.Add("@HostelFee", hostFee);
        ht.Add("@TransDesc", "AC Initialized");
        ht.Add("@TransDate", PopCalendar3.GetDateValue().ToString("MM/dd/yyyy"));
        ht.Add("@SessionYr", drpSession.SelectedValue);
        ht.Add("@UserId", Session["User_Id"].ToString());
        ht.Add("SchoolID", Session["SchoolId"].ToString());
        ht.Add("AccountHeadDr", 3);
        clsDal.ExecuteScalar("ps_insert_GenLedgerInit", ht);
    }

    private void UpdateFineLedger(Decimal InitAmt, string RNo, string AdmnNo)
    {
        Decimal CrAmt = InitAmt;
        DataTable tbFine = new Common().ExecuteSql("select  " + RNo + " as ReceiptNo,* from PS_FineLedger where admnno=" + AdmnNo + " and balance<>0 order by TransDate");
        foreach (DataRow row in (InternalDataCollectionBase)tbFine.Rows)
        {
            Decimal result1 = new Decimal(0);
            Decimal result2 = new Decimal(0);
            Decimal result3 = new Decimal(0);
            Decimal.TryParse(Convert.ToString(row["debit"]), out result1);
            Decimal.TryParse(Convert.ToString(row["credit"]), out result2);
            Decimal.TryParse(Convert.ToString(row["Balance"]), out result3);
            Decimal result4;
            if (CrAmt > result3)
            {
                row["credit"] = (result2 + result3);
                CrAmt -= result3;
                result4 = result2 + result3;
            }
            else
            {
                row["credit"] = (result2 + CrAmt);
                CrAmt = new Decimal(0);
                result4 = result2 + CrAmt;
            }
            Decimal.TryParse(Convert.ToString(row["credit"]), out result4);
            CrAmt -= result3;
            row["balance"] = (result1 - result4);
            tbFine.AcceptChanges();
            if (CrAmt <= new Decimal(0))
                break;
        }
       updateFineLedger(tbFine);
        if (!(CrAmt > new Decimal(0)))
            return;
       UpdateCreditAmount(CrAmt, AdmnNo);
    }

    private void updateFineLedger(DataTable tbFine)
    {
        Common common = new Common();
        foreach (DataRow row in (InternalDataCollectionBase)tbFine.Rows)
            new Common().GetDataTable("ps_sp_update_fineledger", new Hashtable()
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
          row["receiptno"]
        },
        {
           "UserID",
           Convert.ToInt32(Session["User_Id"].ToString())
        },
        {
           "SchoolID",
          Session["SchoolId"].ToString()
        }
      });
    }

    private void UpdateCreditAmount(Decimal CrAmt, string AdmnNo)
    {
        int int32 = Convert.ToInt32(Session["User_Id"].ToString());
        new Common().ExcuteQryInsUpdt("update PS_StudCreditLedger set FeeCredit= " + CrAmt + ",UserId=" + int32 + ", UserDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' where admnno=" + AdmnNo);
    }

    private void updatefeeledgerAcInit(DataTable tbcredit, int mode)
    {
        Common common = new Common();
        foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
            new Common().GetDataTable("ps_sp_update_feeledger", new Hashtable()
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
           Convert.ToInt32(Session["User_Id"].ToString().Trim())
        },
        {
           "UserDate",
           DateTime.Now.ToString("MM/dd/yyyy")
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

    protected void btnInitializedAC_Click(object sender, EventArgs e)
    {
       gvInitializedAc.Visible = true;
       grdStudAC.Visible = false;
       FillGrid(gvInitializedAc, "ps_sp_get_StudListAcInitzedForDel");
       btnDelete.Visible =gvInitializedAc.Rows.Count > 0;
       btnUpdate.Enabled = false;
       drpSelectStudent.SelectedIndex = 0;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
           lblmsg.ForeColor = Color.Red;
           lblmsg.Text = "Please select select a checkbox !";
        }
        else
        {
            string[] strArray =Request["Checkb"].Split(',');
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            for (int index = 0; index < strArray.Length; ++index)
            {
                string empty3 = string.Empty;
                bool isError = false;
                string str =DeleteTrans(strArray[index].ToString(), out isError);
                if (!str.Trim().Equals(string.Empty))
                {
                    if (isError)
                        empty2 += empty2.Trim().Equals(string.Empty) ? str : "," + str;
                    else
                        empty1 += empty1.Trim().Equals(string.Empty) ? str : "," + str;
                }
            }
           FillGrid(gvInitializedAc, "ps_sp_get_StudListAcInitzedForDel");
            if (!empty2.Trim().Equals(string.Empty) || !empty1.Trim().Equals(string.Empty))
            {
               lblmsg.ForeColor = Color.Red;
               lblmsg.Text = "Could not revert A/c initialization for :" + empty2.Trim() + (empty2.Trim().Equals(string.Empty) ? empty1.Trim() : "," + empty1.Trim());
            }
            else
            {
               lblmsg.ForeColor = Color.Green;
               lblmsg.Text = "A/c initialization reverted successfully for the selected records !";
            }
        }
    }

    private string DeleteTrans(string AdmnNo, out bool isError)
    {
        isError = false;
        try
        {
            Decimal CrAmount = new Decimal(0);
            Hashtable ht = new Hashtable();
            Common common = new Common();
            long int64 = Convert.ToInt64("111" + AdmnNo);
            ht.Add("@AdmnNo", AdmnNo);
            ht.Add("@ReceiptNo", int64);
            DataSet dataSet = common.GetDataSet("ps_sp_delete_InitializedStudAc", ht);
            if (dataSet.Tables[1].Rows[0]["Msg"].ToString().Trim().Equals("1"))
            {
               updatefeeledger(dataSet.Tables[0], CrAmount);
                return string.Empty;
            }
            if (dataSet.Tables[1].Rows[0]["Msg"].ToString().Trim().Equals("0"))
            {
                isError = false;
                return AdmnNo;
            }
            isError = true;
            return AdmnNo;
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }

    private void updatefeeledger(DataTable tbcredit, int mode)
    {
        Common common = new Common();
        foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
            new Common().GetDataTable("ps_sp_update_feeledger", new Hashtable()
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
           DateTime.Now.ToString("MM/dd/yyyy")
        },
        {
           "SchoolID",
           ConfigurationManager.AppSettings["SchoolId"]
        },
        {
           "mode",
           "Cash"
        }
      });
    }

    private void updatefeeledger(DataTable tb, Decimal CrAmount)
    {
        Convert.ToDecimal(tb.Rows[0]["oldamount"].ToString());
        Common common = new Common();
        foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
        {
            Decimal result = new Decimal(0);
            Decimal num = new Decimal(0);
            Decimal.TryParse(Convert.ToString(row["Credit"]), out result);
            row["Credit"] = 0;
            Decimal bal = result;
            tb.AcceptChanges();
           UpdateBalAmtInLedger(row, bal);
        }
       updatefeeledger(tb, 0);
    }

    private void UpdateBalAmtInLedger(DataRow dr, Decimal bal)
    {
        new Common().GetDataTable("ps_sp_update_FeeLedgerBank", new Hashtable()
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
         "VrNo",
         dr["receiptno"].ToString()
      },
      {
         "FeeId",
         Convert.ToInt64(dr["FeeId"])
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

    protected void btnShowFullSess_Click(object sender, EventArgs e)
    {
       btnDelete.Visible = false;
       grdStudAC.Visible = true;
       gvInitializedAc.Visible = false;
       FillGrid(grdStudAC, "ps_sp_get_StudListForAcInitFull");
    }
}