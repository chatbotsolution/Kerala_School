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


public partial class Hostel_feercptcashdetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (!Page.IsPostBack)
        {
            PopCalToDt.SetDateValue(DateTime.Today);
            PopCalFmDt.SetDateValue(DateTime.Now.AddDays(-1.0));
            btndelete.Enabled = false;
        }
        lblMsg.Text = "";
    }

    private void fillgrid()
    {
        clsDAL clsDal = new clsDAL();
        ViewState["dt"] = null;
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("PmtMode", drpPmtMode.SelectedValue.ToString().Trim());
        if (txtdate.Text != "" && txtDateTo.Text.Trim() != "")
        {
            string str1 = PopCalFmDt.GetDateValue().ToString("MM/dd/yyyy");
            string str2 = PopCalToDt.GetDateValue().ToString("MM/dd/yyyy");
            hashtable.Add("fromdate", str1);
            hashtable.Add("todate", str2);
        }
        DataTable dataTable2;
        if (txtadmnno.Text.Trim() != "")
        {
            hashtable.Add("admnno", txtadmnno.Text.Trim());
            dataTable2 = clsDal.GetDataTable("Host_get_FeeRecptSelStud", hashtable);
        }
        else
            dataTable2 = clsDal.GetDataTable("Host_get_FeeRecptALL", hashtable);
        ViewState["dt"] = dataTable2;
        GridView2.DataSource = dataTable2;
        GridView2.DataBind();
        if (dataTable2.Rows.Count > 0)
            btndelete.Enabled = true;
        else
            btndelete.Enabled = false;
    }

    protected void btnshow_Click(object sender, EventArgs e)
    {
        if (txtdate.Text != "" || txtDateTo.Text != "")
        {
            fillgrid();
            pnlRcptList.Visible = true;
            pnlDelReason.Visible = false;
        }
        else
            ScriptManager.RegisterClientScriptBlock((Control)txtdate, txtdate.GetType(), "ShowMessage", "alert('Mention Date');", true);
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            lblMsg.Text = "Select a checkbox !";
            lblMsg.ForeColor = Color.Red;
        }
        else if (getValidInput())
        {
            string str1 = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str1.Split(chArray))
                hfRcptId.Value = obj.ToString();
            Hashtable hashtable = new Hashtable();
            clsDAL clsDal = new clsDAL();
            hashtable.Add("@RcptNo", hfRcptId.Value);
            string str2 = clsDal.ExecuteScalar("ps_sp_FeeCancelBankChk", hashtable);
            if (str2.Trim() == "")
            {
                DataRow[] dataRowArray = ((DataTable)ViewState["dt"]).Select("ReceiptNo=" + hfRcptId.Value);
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<div><b>Records to be deleted :-</b></div>");
                stringBuilder.Append("<table border='0px' cellpadding='3' cellspacing='3' style='border:solid black 1px;' class='tbltxt'  width='100%'><tr>");
                stringBuilder.Append("");
                stringBuilder.Append("<td width='100'>Receipt No </td><td width='5'>: </td><td width='150'>" + dataRowArray[0]["ReceiptNo"].ToString() + "</td>");
                stringBuilder.Append("<td>Recieved date </td><td>: </td><td>" + dataRowArray[0]["recvddate"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td>Recieved From </td><td>: </td><td>" + dataRowArray[0]["fullname"].ToString() + "</td>");
                stringBuilder.Append("<td>Amount </td><td>: </td><td>" + dataRowArray[0]["Amt"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                litDetails.Text = stringBuilder.ToString().Trim();
                pnlDelReason.Visible = true;
                pnlRcptList.Visible = false;
            }
            else if (str2.Trim() == "DATE")
            {
                lblMsg.Text = "Cannot Delete Receipt!! Fee Already Received For Later Date!!";
                lblMsg.ForeColor = Color.Red;
            }
            else if (str2.Trim() == "NO")
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
        else
        {
            lblMsg.Text = "Select only one checkbox !";
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

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("FeeReceiptHostel.aspx");
    }

    protected void btnSaveReason_Click(object sender, EventArgs e)
    {
        DeleteTrans(hfRcptId.Value.ToString());
        fillgrid();
        lblMsg.Text = "Receipt cancelled successfully !";
        lblMsg.ForeColor = Color.Green;
        pnlDelReason.Visible = false;
        txtDelReason.Text = string.Empty;
        pnlRcptList.Visible = true;
    }

    private void DeleteTrans(string RecptNo)
    {
        Decimal CrAmount = new Decimal(0);
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal1 = new clsDAL();
        hashtable.Add("receiptno", RecptNo);
        hashtable.Add("@CancelledReason", txtDelReason.Text.Trim().ToString());
        DataSet dataSet1 = new DataSet();
        DataSet dataSet2 = new clsDAL().GetDataSet("Host_DelFeeReceipt", hashtable);
        clsDAL clsDal2 = new clsDAL();
        if (dataSet2.Tables[0].Rows.Count > 0)
            updatefeeledger(dataSet2.Tables[0], CrAmount);
        UpdateGenLedger(dataSet2.Tables[1]);
    }

    private void UpdateGenLedger(DataTable dtGenLedger)
    {
        clsDAL clsDal = new clsDAL();
        foreach (DataRow row in (InternalDataCollectionBase)dtGenLedger.Rows)
            clsDal.GetDataTable("Host_CancelGenLedgerFR", new Hashtable()
      {
        {
           "GenLedgerId",
          row["GenLedgerId"]
        }
      });
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
            UpdateBalAmtInLedger(row, bal);
            num1 += bal;
        }
        updatefeeledger(tb, 0);
    }

    private void UpdateBalAmtInLedger(DataRow dr, Decimal bal)
    {
        new clsDAL().GetDataTable("Host_Update_FeeLedgerBank", new Hashtable()
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

    private void updatefeeledger(DataTable tbcredit, int mode)
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtDelReason.Text = string.Empty;
        pnlRcptList.Visible = true;
        pnlDelReason.Visible = false;
    }
}