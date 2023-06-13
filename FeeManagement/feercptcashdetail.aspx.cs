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

public partial class FeeManagement_feercptcashdetail : System.Web.UI.Page
{
    private DateTime dtchk;
    private string admno;
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
            dataTable2 = clsDal.GetDataTable("ps_sp_get_FeeRecptSelStud", hashtable);
        }
        else
            dataTable2 = clsDal.GetDataTable("ps_sp_get_FeeRecptALL", hashtable);
        ViewState["dt"] = dataTable2;
        GridView2.DataSource = dataTable2;
        GridView2.DataBind();
        lblTotalAmt.Text = "Total Amount : " + dataTable2.Compute("Sum(Amt)", "").ToString();
        if (dataTable2.Rows.Count > 0 && Session["userrights"].ToString().Trim() == "a")
            btndelete.Enabled = true;
        else
            btndelete.Enabled = false;
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            Response.Write("<script>alert('Select a Record');</script>");
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                new clsDAL().GetDataTable("ps_sp_delete_feecash", new Hashtable()
        {
          {
             "ReceiptNo",
            obj
          },
          {
             "userid",
             Session["User_Id"].ToString()
          },
          {
             "schoolid",
             Session["SchoolId"].ToString()
          }
        });
        }
        fillgrid();
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            lblMsg.Text = "Select a Record !";
            lblMsg.ForeColor = Color.Red;
        }
        else if (getValidInput())
        {
            string str1 = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str1.Split(chArray))
            {
                clsDAL clsDal = new clsDAL();
                DataTable dataTable = new DataTable();
                Hashtable hashtable = new Hashtable();
                hfRcptId.Value = obj.ToString();
                hashtable.Add("@RcptNo", hfRcptId.Value);
                dtchk = Convert.ToDateTime(clsDal.GetDataTable("ps_sp_get_DATA", hashtable).Rows[0]["TransDt"].ToString());
            }
            Hashtable hashtable1 = new Hashtable();
            clsDAL clsDal1 = new clsDAL();
            hashtable1.Add("@RcptNo", hfRcptId.Value);
            hashtable1.Add("@RecvdDate", dtchk);
            string str2 = clsDal1.ExecuteScalar("ps_sp_FeeCancelBankChk", hashtable1);
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
            else if (str2.Trim() == "PAID")
            {
                lblMsg.Text = "Book Fee Receipt For This Student Exists After This Transaction!!!Please Delete Previous Receipt First!!";
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
            lblMsg.Text = "Select only one Record !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void DeleteTrans(string RecptNo)
    {
        Decimal CrAmount = new Decimal(0);
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal1 = new clsDAL();
        hashtable.Add("receiptno", RecptNo);
        hashtable.Add("@CancelledReason", txtDelReason.Text.Trim().ToString());
        DataSet dataSet1 = new DataSet();
        DataSet dataSet2 = new clsDAL().GetDataSet("ps_sp_delete_feecashNew", hashtable);
        clsDAL clsDal2 = new clsDAL();
        if (dataSet2.Tables[0].Rows.Count > 0)
        {
            new clsDAL().ExecuteScalar("ps_sp_insert_FeeLedger_h", new Hashtable()
      {
        {
           "AdmnNo",
          dataSet2.Tables[0].Rows[0]["AdmnNo"]
        },
        {
           "VrNo",
          dataSet2.Tables[0].Rows[0]["receiptno"]
        }
      });
            updatefeeledger(dataSet2.Tables[0], CrAmount);
        }
        if (dataSet2.Tables[2].Rows.Count > 0)
            updatefeeledgerBooks(dataSet2.Tables[2]);
        UpdateGenLedger(dataSet2.Tables[1]);
    }

    private void UpdateGenLedger(DataTable dtGenLedger)
    {
        clsDAL clsDal = new clsDAL();
        foreach (DataRow row in (InternalDataCollectionBase)dtGenLedger.Rows)
            clsDal.GetDataTable("PS_SP_CancelGenLedgerFR", new Hashtable()
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

    private void updatefeeledgerBooks(DataTable tb)
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
            UpdateBalAmtInLedgerBooks(row, bal);
            num1 += bal;
        }
        updatefeeledgerBooks(tb, 0);
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

    private void UpdateBalAmtInLedgerBooks(DataRow dr, Decimal bal)
    {
        new clsDAL().GetDataTable("ps_sp_update_FeeLedgerBankBooks", new Hashtable()
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
            new clsDAL().GetDataTable("ps_sp_update_feeledger", new Hashtable()
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

    private void updatefeeledgerBooks(DataTable tbcredit, int mode)
    {
        clsDAL clsDal = new clsDAL();
        foreach (DataRow row in (InternalDataCollectionBase)tbcredit.Rows)
            new clsDAL().GetDataTable("ps_sp_update_feeledgerBooks", new Hashtable()
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

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("feercptcash.aspx");
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtDelReason.Text = string.Empty;
        pnlRcptList.Visible = true;
        pnlDelReason.Visible = false;
    }

    private bool getValidInput()
    {
        int num = 0;
        string[] strArray = Request["Checkb"].Split(',');
        for (int index = 0; index < strArray.Length; ++index)
            ++num;
        return num <= 1;
    }
}