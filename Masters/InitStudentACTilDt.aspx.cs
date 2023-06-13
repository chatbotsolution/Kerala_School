using AjaxControlToolkit;
using ASP;
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


public partial class Masters_InitStudentACTilDt : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
       lblmsg.Text = string.Empty;
       btnUpdate.Enabled = false;
        if (Page.IsPostBack)
            return;
       FillSessionDropdown();
       FillClassDropDown();
       drpYear.Items.Insert(0, new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString()));
       drpYear.Items.Insert(1, new ListItem((DateTime.Now.Year - 1).ToString(), (DateTime.Now.Year - 1).ToString()));
       lblRecCount.Text = "";
       grdStudAC.Visible = false;
    }

    private void FillClassDropDown()
    {
       obj = new clsDAL();
       drpClass.DataSource =obj.GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
       drpClass.DataTextField = "ClassName";
       drpClass.DataValueField = "ClassID";
       drpClass.DataBind();
       drpClass.Items.Insert(0, new ListItem("Select", "0"));
    }

    private void FillSessionDropdown()
    {
       obj = new clsDAL();
       drpSession.DataSource =obj.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
       drpSession.DataTextField = "SessionYr";
       drpSession.DataValueField = "SessionYr";
       drpSession.DataBind();
    }

    private void FillSectionDropDown()
    {
       obj = new clsDAL();
       drpSection.DataSource =obj.GetDataTable("ps_sp_get_ClassSections", new Hashtable()
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
            ht.Add((object)"@SessionYear",drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            ht.Add((object)"@ClassID",drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            ht.Add((object)"@Section",drpSection.SelectedValue);
       drpSelectStudent.DataSource =clsDal.GetDataTable("ps_GetStudForInit", ht);
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
       FillGrid(grdStudAC, "ps_sp_GetStudForTilDtInit");
       gvInitializedAc.Visible = false;
       btnDelete.Visible = false;
    }

    private void IntializePage()
    {
       drpClass.SelectedIndex = -1;
       drpSession.SelectedIndex = -1;
       grdStudAC.Visible = false;
       drpMonth.SelectedIndex = 0;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
       btnDelete.Visible = false;
       grdStudAC.Visible = true;
       gvInitializedAc.Visible = false;
       FillGrid(grdStudAC, "ps_sp_GetStudForTilDtInit");
    }

    private void FillGrid(GridView gv, string procName)
    {
       obj = new clsDAL();
        Hashtable ht = new Hashtable();
        if (drpSession.Items.Count > 0)
            ht.Add((object)"@Session",drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            ht.Add((object)"@Class",drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            ht.Add((object)"@Section",drpSection.SelectedValue);
        if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
            ht.Add((object)"@AdmnNo",drpSelectStudent.SelectedValue.ToString());
        DataTable dataTable =obj.GetDataTable(procName, ht);
        if (dataTable.Rows.Count > 0)
        {
            gv.DataSource =dataTable;
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
        if (!getValidInput())
            return;
        string str = "";
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
            if (num8 > new Decimal(0))
            {
                DataSet dataSet = new clsDAL().GetDataSet("ps_sp_InsStudInitTilDt", new Hashtable()
        {
          {
            "Session",
           drpSession.SelectedValue
          },
          {
            "RecvDate",
            Convert.ToDateTime("28 " +drpMonth.SelectedValue.Trim() + " " +drpYear.SelectedValue.Trim())
          },
          {
            "AdmnNo",
            control.Text.ToString()
          },
          {
            "Class",
           drpClass.SelectedValue
          },
          {
            "Description",
            "Current Init"
          },
          {
            "Amount",
            num8
          },
          {
            "PaymentMode",
            "Cash"
          },
          {
            "UserID",
            Convert.ToInt32(Session["User_Id"].ToString().Trim())
          },
          {
            "SchoolID",
           Session["SchoolId"].ToString()
          }
        });
                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows[0][0].ToString().Trim() != "DUP" && dataSet.Tables[0].Rows[0][0].ToString().Trim() != "ERROR")
                {
                    if (dataSet.Tables[3].Rows.Count > 0)
                    {
                       ViewState["recptno"] =dataSet.Tables[3].Rows[0][0].ToString().Trim();
                        if (num5 > new Decimal(0))
                           insertfeeledger(dataSet.Tables[0], num5);
                        if (dataSet.Tables[1].Rows.Count > 0 && num6 > new Decimal(0))
                           insertBusHostFee(dataSet.Tables[1], num6);
                        if (dataSet.Tables[2].Rows.Count > 0 && num7 > new Decimal(0))
                            insert_host_feeledger(dataSet.Tables[2], num7);
                           //insertBusHostFee(dataSet.Tables[2], num7);
                       insertGenLedger(num5, num6, num7);
                    }
                }
                else
                {
                    str = "No";
                   lblmsg.ForeColor = Color.Red;
                   lblmsg.Text = "Receipt No already Exists !";
                    break;
                }
            }
            else
            {
                str = "Some";
                if (drpSelectStudent.SelectedIndex > 0)
                {
                    str = "No";
                   lblmsg.ForeColor = Color.Red;
                   lblmsg.Text = "Please Enter Fee Amount!";
                }
            }
        }
       FillGrid(grdStudAC, "ps_sp_GetStudForTilDtInit");
        if (str == "")
        {
           lblmsg.ForeColor = Color.Green;
           lblmsg.Text = "A/c initialized successfully !";
        }
        else
        {
            if (!(str == "Some"))
                return;
           lblmsg.ForeColor = Color.Green;
           lblmsg.Text = "Students A/c initialized successfully!!";
        }
    }
    private void insert_host_feeledger(DataTable tb, Decimal amt)
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
                //InsertCrAmtInLedger(row, CrAmt);//doing nothing
                if (amt <= new Decimal(0))
                    break;
            }
            update_host_feeledgerAcInit(tb, 0);
        }
        catch (Exception ex)
        {
            lblmsg.ForeColor = Color.Red;
            lblmsg.Text = ex.ToString();
        }
    }
    private void update_host_feeledgerAcInit(DataTable tbcredit, int mode)
    {
        obj = new clsDAL();
        foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
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
    }
    private bool getValidInput()
    {
        bool flag = true;
        foreach (GridViewRow row in grdStudAC.Rows)
        {
            Label control1 = (Label)row.FindControl("lblBalance");
            TextBox control2 = (TextBox)row.FindControl("txtFeeAmount");
            Label control3 = (Label)row.FindControl("lblBusFee");
            TextBox control4 = (TextBox)row.FindControl("txtBusAmount");
            Label control5 = (Label)row.FindControl("lblHostFee");
            TextBox control6 = (TextBox)row.FindControl("txtHostAmount");
            if (Convert.ToDecimal(control2.Text.Trim()) > Convert.ToDecimal(control1.Text.Trim()))
            {
                control2.Focus();
                flag = false;
               lblmsg.Text = "Entered Amount is not valid!!";
               lblmsg.ForeColor = Color.Red;
                return flag;
            }
            if (Convert.ToDecimal(control4.Text.Trim()) > Convert.ToDecimal(control3.Text.Trim()))
            {
                control4.Focus();
                flag = false;
               lblmsg.Text = "Entered Amount is not valid!!";
               lblmsg.ForeColor = Color.Red;
                return flag;
            }
            if (Convert.ToDecimal(control6.Text.Trim()) > Convert.ToDecimal(control5.Text.Trim()))
            {
                control6.Focus();
                flag = false;
               lblmsg.Text = "Entered Amount is not valid!!";
               lblmsg.ForeColor = Color.Red;
                return flag;
            }
        }
        return flag;
    }
    
    private void insertfeeledger(DataTable tb, Decimal amt)
    {
        try
        {
            foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
            {
                if (amt > new Decimal(0))
                {
                    Decimal result = new Decimal(0);
                    Decimal num = new Decimal(0);
                    Decimal.TryParse(Convert.ToString(row["Balance"]), out result);
                    Decimal CrAmt;

                    if (amt > result)
                    {
                        row["credit"] = row["Debit"];
                        row["balance"] = 0;
                        amt -= result;
                        CrAmt = result;
                    }
                    else
                    {
                        row["credit"] = Convert.ToDecimal(row["credit"]) + amt;
                        row["balance"] = (result - amt);
                        CrAmt = amt;
                        amt = new Decimal(0);
                        feecreditinsert(row);
                    }
                    tb.AcceptChanges();
                    InsertCrAmtInLedger(row, CrAmt);//doing nothing
                }
                else
                    row["ReceiptNo"] = "";
            }
           updatefeeledgerAcInit(tb, 0);
        }
        catch (Exception ex)
        {
           lblmsg.ForeColor = Color.Red;
           lblmsg.Text = ex.ToString();
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
    private void InsertCrAmtInLedger(DataRow dr, Decimal CrAmt)//doing nothing
    {
        new clsDAL().GetDataTable("ps_sp_insert_FeeLedgerNew", new Hashtable()
    {
      {
        "date",
        Convert.ToDateTime(dr["TransDate"])
      },
      {
        "TransDate",
        Convert.ToDateTime("28 " +drpMonth.SelectedValue.Trim() + " " +drpYear.SelectedValue.Trim())
      },
      {
        "AdmnNo",
        Convert.ToInt64(dr["AdmnNo"])
      },
      {
        "TransDesc",
        "Current Init"
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
        dr["SessionYr"].ToString()
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



    private void insertGenLedger(Decimal Amnt, Decimal busFee, Decimal hostFee)
    {
        if (ViewState["recptno"] == null)
            return;
        clsDAL clsDal = new clsDAL();
        Hashtable ht = new Hashtable();
        ht.Add((object)"@Receipt_VrNo",ViewState["recptno"].ToString().Trim());
        if (Amnt > new Decimal(0))
            ht.Add((object)"@Amount",Amnt);
        if (busFee > new Decimal(0))
            ht.Add((object)"@BusFee",busFee);
        if (hostFee > new Decimal(0))
            ht.Add((object)"@HostelFee",hostFee);
        ht.Add((object)"@TransDesc","Current Init");
        ht.Add((object)"@TransDate",Convert.ToDateTime("28 " +drpMonth.SelectedValue.Trim() + " " +drpYear.SelectedValue.Trim()));
        ht.Add((object)"@SessionYr",drpSession.SelectedValue);
        ht.Add((object)"@UserId",Session["User_Id"].ToString());
        ht.Add((object)"SchoolID",Session["SchoolId"].ToString());
        ht.Add((object)"AccountHeadDr",3);
        clsDal.ExecuteScalar("ps_insert_GenLedgerInit", ht);
    }

    private void UpdateFineLedger(Decimal InitAmt, string RNo, string AdmnNo)
    {
        Decimal CrAmt = InitAmt;
       obj = new clsDAL();
        DataTable dataTableQry =obj.GetDataTableQry("select  " + RNo + " as ReceiptNo,* from PS_FineLedger where admnno=" + AdmnNo + " and balance<>0 order by TransDate");
        foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
        {
            Decimal result1 = new Decimal(0);
            Decimal result2 = new Decimal(0);
            Decimal result3 = new Decimal(0);
            Decimal.TryParse(Convert.ToString(row["debit"]), out result1);
            Decimal.TryParse(Convert.ToString(row["credit"]), out result2);
            Decimal.TryParse(Convert.ToString(row["Balance"]), out result3);
            if (CrAmt > result3)
            {
                row["credit"] =(result2 + result3);
                CrAmt -= result3;
                result2 += result3;
            }
            else
            {
                row["credit"] =(result2 + CrAmt);
                CrAmt = new Decimal(0);
                result2 += CrAmt;
            }
            Decimal.TryParse(Convert.ToString(row["credit"]), out result2);
            CrAmt -= result3;
            row["balance"] =(result1 - result2);
            dataTableQry.AcceptChanges();
            if (CrAmt <= new Decimal(0))
                break;
        }
       updateFineLedger(dataTableQry);
        if (!(CrAmt > new Decimal(0)))
            return;
       UpdateCreditAmount(CrAmt, AdmnNo);
    }

    private void updateFineLedger(DataTable tbFine)
    {
       obj = new clsDAL();
        foreach (DataRow row in (InternalDataCollectionBase)tbFine.Rows)
        {
           obj = new clsDAL();
           obj.GetDataTable("ps_sp_update_fineledger", new Hashtable()
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
    }

    private void UpdateCreditAmount(Decimal CrAmt, string AdmnNo)
    {
        int int32 = Convert.ToInt32(Session["User_Id"].ToString());
        string qry = "update PS_StudCreditLedger set FeeCredit= " +CrAmt + ",UserId=" +int32 + ", UserDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' where admnno=" + AdmnNo;
       obj = new clsDAL();
       obj.ExcuteQryInsUpdt(qry);
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
                ht.Add((object)"@Ad_Id", row["Ad_Id"].ToString().Trim());
                ht.Add((object)"@AdFeeNo", row["AdFeeNo"].ToString().Trim());
                ht.Add((object)"@RcptNo", row["ReceiptNo"].ToString().Trim());
                ht.Add((object)"@AdMonth", row["AdMonth"].ToString().Trim());
                ht.Add((object)"@Balance", row["balance"].ToString().Trim());
                ht.Add((object)"@Credit", num2);
                ht.Add((object)"userid", Convert.ToInt32(Session["User_Id"].ToString()));
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
    private void updatefeeledgerAcInit(DataTable tbcredit, int mode)
    {
       obj = new clsDAL();
        foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
        {
           obj = new clsDAL();
           obj.GetDataTable("ps_sp_update_feeledger", new Hashtable()
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
    }

    protected void btnInitializedAC_Click(object sender, EventArgs e)
    {
       gvInitializedAc.Visible = true;
       grdStudAC.Visible = false;
       FillGrid(gvInitializedAc, "ps_sp_GetStudInitTilDtForDel");
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
           FillGrid(gvInitializedAc, "ps_sp_GetStudInitTilDtForDel");
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

    private string DeleteTrans(string RecptNo, out bool isError)
    {
        isError = false;
        try
        {
            Decimal CrAmount = new Decimal(0);
            Hashtable ht = new Hashtable();
           obj = new clsDAL();
            ht.Add((object)"@ReceiptNo",Convert.ToInt64(RecptNo.Trim()));
            DataSet dataSet =obj.GetDataSet("ps_sp_DelStudInitTillDt", ht);
            if (dataSet.Tables[1].Rows[0][0].ToString().Trim().Equals("1"))
            {
               updatefeeledger(dataSet.Tables[0], CrAmount);
                return string.Empty;
            }
            if (dataSet.Tables[1].Rows[0][0].ToString().Trim().Equals("0"))
            {
                isError = false;
                return dataSet.Tables[0].Rows[0][0].ToString().Trim();
            }
            isError = true;
            return dataSet.Tables[0].Rows[0][0].ToString().Trim();
        }
        catch (Exception ex)
        {
            return string.Empty;
        }
    }

    private void updatefeeledger(DataTable tbcredit, int mode)
    {
        foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
        {
           obj = new clsDAL();
           obj.GetDataTable("ps_sp_update_feeledger", new Hashtable()
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
    }

    private void updatefeeledger(DataTable tb, Decimal CrAmount)
    {
        Convert.ToDecimal(tb.Rows[0]["oldamount"].ToString());
        clsDAL clsDal = new clsDAL();
        foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
        {
            Decimal result = new Decimal(0);
            Decimal num = new Decimal(0);
            Decimal.TryParse(Convert.ToString(row["Credit"]), out result);
            row["Credit"] =0;
            Decimal bal = result;
            tb.AcceptChanges();
           UpdateBalAmtInLedger(row, bal);
        }
       updatefeeledger(tb, 0);
    }

    private void UpdateBalAmtInLedger(DataRow dr, Decimal bal)
    {
        Hashtable ht = new Hashtable();
        ht.Add((object)"transno",Convert.ToInt64(dr["TransNo"]));
        ht.Add((object)"AdmnNo",Convert.ToInt64(dr["AdmnNo"]));
        ht.Add((object)"SessionYr",dr["SessionYr"].ToString());
        ht.Add((object)"FeeId",Convert.ToInt64(dr["FeeId"]));
        ht.Add((object)"Balance",bal);
        ht.Add((object)"userid",Convert.ToInt32(Session["User_Id"].ToString()));
       obj = new clsDAL();
       obj.GetDataTable("ps_sp_update_FeeLedgerBank", ht);
       obj = new clsDAL();
       obj.ExecuteScalarQry("delete from dbo.PS_FeeLedger where transno=" +Convert.ToInt64(dr["TransNo"]));
    }
}