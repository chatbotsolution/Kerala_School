using AjaxControlToolkit;
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
using System.Web.UI.WebControls;

public partial class Hostel_InitStudHostAC : System.Web.UI.Page
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
        lblRecCount.Text = "";
        grdStudAC.Visible = false;
        PopCalendar3.SetDateValue(Convert.ToDateTime("31 Mar" + drpSession.SelectedValue.ToString().Substring(0, 4)));
    }

    private void FillClassDropDown()
    {
        obj = new clsDAL();
        drpClass.DataSource = obj.GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassID";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("Select", "0"));
    }

    private void FillSessionDropdown()
    {
        obj = new clsDAL();
        drpSession.DataSource = obj.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillSectionDropDown()
    {
        obj = new clsDAL();
        drpSection.DataSource = obj.GetDataTable("ps_sp_get_ClassSections", new Hashtable()
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
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@SessionYear", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@ClassID", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        drpSelectStudent.DataSource = clsDal.GetDataTable("Host_GetStudForInit", hashtable);
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
        FillGrid(grdStudAC, "Host_GetStudListForAcInit", 0);
        gvInitializedAc.Visible = false;
        btnDelete.Visible = false;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        btnDelete.Visible = false;
        grdStudAC.Visible = true;
        gvInitializedAc.Visible = false;
        FillGrid(grdStudAC, "Host_GetStudListForAcInit", 0);
    }

    protected void btnFullSession_Click(object sender, EventArgs e)
    {
        btnDelete.Visible = false;
        grdStudAC.Visible = true;
        gvInitializedAc.Visible = false;
        FillGrid(grdStudAC, "Host_GetStudListForAcInit", 1);
    }

    private void FillGrid(GridView gv, string procName, int FullSession)
    {
        obj = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@Session", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@AdmnNo", drpSelectStudent.SelectedValue.ToString());
        if (FullSession == 1)
            hashtable.Add("@FullSy", FullSession);
        DataTable dataTable = obj.GetDataTable(procName, hashtable);
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

    private void IntializePage()
    {
        drpClass.SelectedIndex = -1;
        drpSession.SelectedIndex = -1;
        grdStudAC.Visible = false;
        txtDate.Text = "";
    }

    private string CheckAmount()
    {
        string str = "";
        foreach (GridViewRow row in grdStudAC.Rows)
        {
            TextBox control1 = (TextBox)row.FindControl("txtFeeAmount");
            Label control2 = (Label)row.FindControl("lblTotDue");
            if (control1.Text.Trim() != "" && (double)float.Parse(control1.Text.Trim()) > (double)float.Parse(control2.Text.Trim()))
            {
                str = "Paid Amount cannot be greater than total due!!";
                control1.Focus();
                break;
            }
        }
        return str;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (CheckAmount() != "")
        {
            lblmsg.Text = CheckAmount().ToString();
            lblmsg.ForeColor = Color.Red;
        }
        else
        {
            string str1 = "";
            foreach (GridViewRow row in grdStudAC.Rows)
            {
                Decimal num1 = new Decimal(0);
                Decimal busFee = new Decimal(0);
                Decimal hostFee = new Decimal(0);
                Decimal num2 = new Decimal(0);
                Decimal num3;
                try
                {
                    num3 = Convert.ToDecimal(((TextBox)row.Cells[4].FindControl("txtFeeAmount")).Text.ToString().Trim());
                }
                catch (Exception ex)
                {
                    num3 = new Decimal(0);
                    busFee = new Decimal(0);
                    hostFee = new Decimal(0);
                }
                Decimal num4 = num3 + busFee + hostFee;
                Label control = (Label)row.FindControl("lblAdmnNo");
                string str2 = "111" + control.Text.ToString() + "2";
                if (num4 > new Decimal(0))
                {
                    Hashtable hashtable = new Hashtable();
                    ViewState["recptno"] = str2;
                    hashtable.Add("receiptno", str2);
                    hashtable.Add("Session", drpSession.SelectedValue);
                    hashtable.Add("RecvDate", PopCalendar3.GetDateValue().ToString("MM/dd/yyyy"));
                    hashtable.Add("AdmnNo", control.Text.ToString());
                    hashtable.Add("Class", drpClass.SelectedValue);
                    hashtable.Add("Description", "AC Initialized");
                    hashtable.Add("Amount", num4);
                    hashtable.Add("PaymentMode", "Cash");
                    hashtable.Add("UserID", Convert.ToInt32(Session["User_Id"].ToString().Trim()));
                    hashtable.Add("SchoolID", Session["SchoolId"].ToString());
                    DataSet dataSet = new clsDAL().GetDataSet("Host_InsFeecashInit", hashtable);
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows[0][0].ToString().Trim() != "Receipt No already Exists")
                    {
                        if (num3 > new Decimal(0))
                            insertfeeledger(dataSet.Tables[0], num3);
                        insertGenLedger(num3, busFee, hostFee);
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
            FillGrid(grdStudAC, "Host_GetStudListForAcInit", 0);
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
        new clsDAL().GetDataTable("Host_Insert_FeeLedgerNew", new Hashtable()
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

    private void updatefeeledgerAcInit(DataTable tbcredit, int mode)
    {
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

    private void insertGenLedger(Decimal Amnt, Decimal busFee, Decimal hostFee)
    {
        if (ViewState["recptno"] == null)
            return;
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Receipt_VrNo", ViewState["recptno"].ToString().Trim());
        if (Amnt > new Decimal(0))
            hashtable.Add("@Amount", Amnt);
        if (busFee > new Decimal(0))
            hashtable.Add("@BusFee", busFee);
        if (hostFee > new Decimal(0))
            hashtable.Add("@HostelFee", hostFee);
        hashtable.Add("@TransDesc", "AC Initialized");
        hashtable.Add("@TransDate", PopCalendar3.GetDateValue().ToString("MM/dd/yyyy"));
        hashtable.Add("@SessionYr", drpSession.SelectedValue);
        hashtable.Add("@UserId", Session["User_Id"].ToString());
        hashtable.Add("SchoolID", Session["SchoolId"].ToString());
        hashtable.Add("AccountHeadDr", 3);
        clsDal.ExecuteScalar("Host_Insert_GenLedgerNew", hashtable);
    }

    protected void btnInitializedAC_Click(object sender, EventArgs e)
    {
        gvInitializedAc.Visible = true;
        grdStudAC.Visible = false;
        FillGrid(gvInitializedAc, "Host_GetStudListInitialized", 0);
        btnDelete.Visible = gvInitializedAc.Rows.Count > 0;
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
            string[] strArray = Request["Checkb"].Split(',');
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            for (int index = 0; index < strArray.Length; ++index)
            {
                string empty3 = string.Empty;
                bool isError = false;
                string str = DeleteTrans(strArray[index].ToString(), out isError);
                if (!str.Trim().Equals(string.Empty))
                {
                    if (isError)
                        empty2 += empty2.Trim().Equals(string.Empty) ? str : "," + str;
                    else
                        empty1 += empty1.Trim().Equals(string.Empty) ? str : "," + str;
                }
            }
            FillGrid(gvInitializedAc, "Host_GetStudListInitialized", 0);
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
            Hashtable hashtable = new Hashtable();
            obj = new clsDAL();
            long int64 = Convert.ToInt64("111" + AdmnNo + "2");
            hashtable.Add("@AdmnNo", AdmnNo);
            hashtable.Add("@ReceiptNo", int64);
            DataSet dataSet = obj.GetDataSet("Host_DelInitializedStudAc", hashtable);
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

    private void updatefeeledger(DataTable tb, Decimal CrAmount)
    {
        Convert.ToDecimal(tb.Rows[0]["oldamount"].ToString());
        clsDAL clsDal = new clsDAL();
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
        updatefeeledger(tb, new Decimal(0));
    }

    private void UpdateBalAmtInLedger(DataRow dr, Decimal bal)
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("transno", Convert.ToInt64(dr["TransNo"]));
        hashtable.Add("AdmnNo", Convert.ToInt64(dr["AdmnNo"]));
        hashtable.Add("VrNo", dr["receiptno"].ToString());
        hashtable.Add("FeeId", Convert.ToInt64(dr["FeeId"]));
        hashtable.Add("Balance", bal);
        hashtable.Add("userid", Convert.ToInt32(Session["User_Id"].ToString()));
        obj = new clsDAL();
        obj.GetDataTable("Host_Update_FeeLedgerBank", hashtable);
    }
}