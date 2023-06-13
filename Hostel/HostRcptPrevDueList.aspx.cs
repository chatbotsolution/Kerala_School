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

public partial class Hostel_HostRcptPrevDueList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        PopCalToDt.SetDateValue(DateTime.Today);
        PopCalFmDt.SetDateValue(DateTime.Now.AddDays(-1.0));
        fillsession();
        btndelete.Enabled = false;
        if (!(Session["userrights"].ToString().Trim() == "a"))
            return;
        btndelete.Enabled = true;
    }

    private void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillgrid()
    {
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("Session", drpSession.SelectedValue.ToString().Trim());
        if (drpPmtMode.SelectedValue != "All")
            hashtable.Add("PmtMode", drpPmtMode.SelectedValue.ToString().Trim());
        if (txtdate.Text != "" && txtDateTo.Text.Trim() != "")
        {
            string str1 = PopCalFmDt.GetDateValue().ToString("MM/dd/yyyy");
            string str2 = PopCalToDt.GetDateValue().ToString("MM/dd/yyyy");
            hashtable.Add("fromdate", str1);
            hashtable.Add("todate", str2);
        }
        if (txtadmnno.Text.Trim() != "")
            hashtable.Add("admnno", txtadmnno.Text.Trim());
        DataTable dataTable2 = common.GetDataTable("Host_GetFeeRecptPrevBal", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            grdStud.DataSource = dataTable2;
            grdStud.DataBind();
            if (Session["userrights"].ToString().Trim() == "a")
                btndelete.Enabled = true;
            else
                btndelete.Enabled = false;
        }
        else
        {
            btndelete.Enabled = false;
            grdStud.DataSource = null;
            grdStud.DataBind();
        }
    }

    protected void btnshow_Click(object sender, EventArgs e)
    {
        fillgrid();
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
            Response.Write("<script>alert('Select a checkbox');</script>");
        else if (getValidInput())
        {
            string[] strArray = Request["Checkb"].Split(',');
            for (int index = 0; index < strArray.Length; ++index)
            {
                Hashtable hashtable = new Hashtable();
                clsDAL clsDal = new clsDAL();
                hashtable.Add("@RcptNo", strArray[index].ToString());
                string str = clsDal.ExecuteScalar("ps_sp_FeeCancelBankChk", hashtable);
                if (str.Trim() == "")
                {
                    DeleteTrans(strArray[index].ToString());
                    fillgrid();
                    lblMsg.Text = "Receipt cancelled successfully !";
                    lblMsg.ForeColor = Color.Green;
                }
                else if (str.Trim() == "DATE")
                {
                    lblMsg.Text = "Cannot Delete Receipt!! Fee Already Received For Later Date!!";
                    lblMsg.ForeColor = Color.Red;
                }
                else if (str.Trim() == "NO")
                {
                    lblMsg.Text = "Fee Receipt For This Student Exists After This Transaction!!!Please Delete Previous Receipt First!!";
                    lblMsg.ForeColor = Color.Red;
                }
                else
                {
                    lblMsg.Text = "Cannot Delete Received Fee!!! Fee Amount Already Encashed !!!";
                    lblMsg.ForeColor = Color.Red;
                }
            }
        }
        else
        {
            lblMsg.Text = "Select only one Record !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private bool getValidInput()
    {
        int num = 0;
        string[] strArray = Request["Checkb"].Split(',');
        for (int index = 0; index < strArray.Length; ++index)
            ++num;
        return num <= 1;
    }

    private void DeleteTrans(string RecptNo)
    {
        Decimal CrAmount = new Decimal(0);
        Hashtable hashtable = new Hashtable();
        Common common = new Common();
        hashtable.Add("receiptno", RecptNo);
        hashtable.Add("UserId", Session["User_Id"]);
        updatefeeledger(common.GetDataTable("Host_DelPrevYrfeecash", hashtable), CrAmount);
    }

    private void updatefeeledger(DataTable tb, Decimal CrAmount)
    {
        Convert.ToDecimal(tb.Rows[0]["oldamount"].ToString());
        Decimal num1 = new Decimal(0);
        Common common = new Common();
        foreach (DataRow row in (InternalDataCollectionBase)tb.Rows)
        {
            Decimal result = new Decimal(0);
            Decimal num2 = new Decimal(0);
            Decimal.TryParse(Convert.ToString(row["Credit"]), out result);
            row["Credit"] = 0;
            Decimal bal = result;
            tb.AcceptChanges();
            UpdateBalAmtInLedger(row, bal);
            num1 += bal;
        }
        updatefeeledger(tb, 0);
    }

    private void UpdateBalAmtInLedger(DataRow dr, Decimal bal)
    {
        new Common().GetDataTable("Host_update_FeeLedgerPrevYr", new Hashtable()
    {
      {
         "AdmnNo",
         Convert.ToInt64(dr["AdmnNo"])
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

    private void updatefeeledger(DataTable tbcredit, int mode)
    {
        Common common = new Common();
        foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
            new Common().GetDataTable("Host_Update_FeeLedger", new Hashtable()
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
           Session["SchoolId"].ToString()
        },
        {
           "mode",
           "Cash"
        }
      });
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("HostRcptPrevDue.aspx");
    }
}