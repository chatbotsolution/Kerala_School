using AjaxControlToolkit;
using ASP;
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

public partial class Accounts_ReceiveStockPOEdit : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            return;
        if (Request.QueryString["PId"] == null)
        {
            Response.Redirect("ReceiveStockList.aspx");
        }
        else
        {
            lblMsg.Text = "";
            if (Page.IsPostBack)
                return;
            ConfigurationSettings.AppSettings["mc"].ToString();
            hfPurchaseId.Value = Request.QueryString["PId"];
            GetPurchaseDetail();
        }
    }

    private void GetPurchaseDetail()
    {
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        Decimal num3 = new Decimal(0);
        DataTable dataTableQry = obj.GetDataTableQry("SELECT * FROM ACTS_VWStockList WHERE PurchaseId=" + hfPurchaseId.Value);
        dtpPurchaseDt.SetDateValue(Convert.ToDateTime(dataTableQry.Rows[0]["PurDate"].ToString()));
        txtInvoice.Text = dataTableQry.Rows[0]["InvNo"].ToString();
        lblSupplier.Text = dataTableQry.Rows[0]["PartyName"].ToString();
        Decimal num4 = Convert.ToDecimal(dataTableQry.Rows[0]["TotPurAmt"].ToString());
        Decimal num5 = Convert.ToDecimal(dataTableQry.Rows[0]["VAT_CST_TotAmt"].ToString());
        Decimal num6 = Convert.ToDecimal(dataTableQry.Rows[0]["ShippingCharges"].ToString());
        Decimal num7 = Convert.ToDecimal(dataTableQry.Rows[0]["AddnlChargesAmt"].ToString());
        Decimal num8 = Convert.ToDecimal(dataTableQry.Rows[0]["TotBillAmt"].ToString());
        Decimal num9 = num8 - (num5 + num6 + num7);
        Decimal num10 = num4 - num9;
        Decimal num11 = num10 / num4 * new Decimal(100);
        txtPurAmt.Text = num4.ToString();
        txtDiscPer.Text = num11.ToString("0.0000");
        txtDiscAmt.Text = num10.ToString("0.0000");
        txtVAT.Text = num5.ToString();
        txtShipCharge.Text = num6.ToString();
        txtOtherCharge.Text = num7.ToString();
        txtDesc.Text = dataTableQry.Rows[0]["AddnlChargesDesc"].ToString();
        txtInvoiceAmt.Text = num8.ToString("0.0000");
        txtRemarks.Text = dataTableQry.Rows[0]["Remarks"].ToString();
        DataTable dataTable = obj.GetDataTable("ACTS_RcvStockDtls", new Hashtable()
    {
      {
         "@PurchaseId",
         hfPurchaseId.Value
      }
    });
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
        {
            Convert.ToDecimal(row["QtyIn"]);
            Decimal num12 = Convert.ToDecimal(row["Unit_PurPrice"]);
            Decimal num13 = num11 * num12 / new Decimal(100);
            row["Unit_PurPrice"] = Math.Round(num12 + num13).ToString();
            dataTable.AcceptChanges();
        }
        grdPurchase.DataSource = dataTable;
        grdPurchase.DataBind();
        ViewState["PurchaseDetails"] = dataTable;
    }

    protected void btnShowList_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReceiveStockList.aspx");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (hfPurchaseDtlId.Value == "0")
        {
            lblMsg.Text = "Select a Record from the List to Update";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            DataTable dataTable = (DataTable)ViewState["PurchaseDetails"];
            foreach (DataRow dataRow in dataTable.Select("PurchaseDetailId=" + hfPurchaseDtlId.Value))
            {
                if (Convert.ToDecimal(txtRcvQty.Text) < Convert.ToDecimal(dataRow["QtyOut"]))
                {
                    lblMsg.Text = "Received Quantity shouldn't be more than Sold Quantity.";
                    lblMsg.ForeColor = Color.Red;
                }
                else
                {
                    dataRow["QtyIn"] = txtRcvQty.Text;
                    dataRow["Unit_MRP"] = txtUnitMRP.Text;
                    dataRow["Unit_PurPrice"] = txtUnitPurPrice.Text;
                    dataRow["Unit_SalePrice"] = txtUnitSalePrice.Text;
                    dataRow["TotalAmt"] = (Convert.ToDecimal(dataRow["QtyIn"]) * Convert.ToDecimal(dataRow["Unit_PurPrice"]));
                    dataTable.AcceptChanges();
                    ViewState["PurchaseDetails"] = dataTable;
                    txtPurAmt.Text = Convert.ToDecimal(dataTable.Compute("SUM(TotalAmt)", "")).ToString("0.00");
                    ScriptManager.RegisterStartupScript((Page)this, typeof(Page), "calDiscAmt();", "calDiscAmt();", true);
                    grdPurchase.DataSource = dataTable;
                    grdPurchase.DataBind();
                    ResetFields();
                }
            }
        }
    }

    protected void grdPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandName == "Modify"))
            return;
        foreach (DataRow dataRow in ((DataTable)ViewState["PurchaseDetails"]).Select("PurchaseDetailId=" + e.CommandArgument))
        {
            lblItemName.Text = dataRow["ItemName"].ToString();
            txtRcvQty.Text = dataRow["QtyIn"].ToString();
            txtUnitMRP.Text = Convert.ToDecimal(dataRow["Unit_MRP"]).ToString("0.00");
            txtUnitPurPrice.Text = Convert.ToDecimal(dataRow["Unit_PurPrice"]).ToString("0.00");
            txtUnitSalePrice.Text = Convert.ToDecimal(dataRow["Unit_SalePrice"]).ToString("0.00");
        }
        hfPurchaseDtlId.Value = e.CommandArgument.ToString();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDouble(txtPurAmt.Text) == 0.0 || Convert.ToDouble(txtInvoiceAmt.Text) == 0.0)
            {
                lblMsg.Text = "Invoice Amount shouldn't be Zero";
                lblMsg.ForeColor = Color.Red;
            }
            else
                UpdateReceivedStock();
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void UpdateReceivedStock()
    {
        double num = double.Parse(txtPurAmt.Text.Trim());
        if (txtDiscAmt.Text.Trim() != "")
            num -= double.Parse(txtDiscAmt.Text.Trim());
        if (txtVAT.Text.Trim() != "")
            num += double.Parse(txtVAT.Text.Trim());
        if (txtOtherCharge.Text.Trim() != "")
            num += double.Parse(txtOtherCharge.Text.Trim());
        if (txtShipCharge.Text.Trim() != "")
            num += double.Parse(txtShipCharge.Text.Trim());
        string str = obj.ExecuteScalar("ACTS_UpdtReceivedStock", new Hashtable()
    {
      {
         "@PurchaseId",
         hfPurchaseId.Value
      },
      {
         "@TotPurAmt",
         txtPurAmt.Text
      },
      {
         "@VatAmt",
         txtVAT.Text
      },
      {
         "@AddnlChrgDesc",
         txtDesc.Text
      },
      {
         "@AddnlChrgAmt",
         txtOtherCharge.Text
      },
      {
         "@ShipCharge",
         txtShipCharge.Text
      },
      {
         "@TotBillAmt",
         num
      },
      {
         "@Remarks",
         txtRemarks.Text
      },
      {
         "@SchoolId",
        Session["SchoolId"]
      },
      {
         "@UserId",
        Session["User_Id"]
      }
    });
        if (str.Trim() == string.Empty)
        {
            UpdateReceivedStockDtls();
        }
        else
        {
            lblMsg.Text = str;
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void UpdateReceivedStockDtls()
    {
        string str = "";
        Hashtable hashtable = new Hashtable();
        DataTable dataTable = (DataTable)ViewState["PurchaseDetails"];
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        Decimal num3 = new Decimal(0);
        Decimal num4 = new Decimal(0);
        if (txtDiscPer.Text.Trim() != "")
            num1 = Convert.ToDecimal(txtDiscPer.Text);
        if (txtVAT.Text.Trim() != "")
            num2 = Convert.ToDecimal(txtVAT.Text);
        if (txtShipCharge.Text.Trim() != "")
            num3 = Convert.ToDecimal(txtShipCharge.Text);
        if (txtOtherCharge.Text.Trim() != "")
            num4 = Convert.ToDecimal(txtOtherCharge.Text);
        Decimal num5 = Convert.ToDecimal(dataTable.Compute("SUM(QtyIn)", ""));
        Decimal num6 = (num2 + num3 + num4) / num5;
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
        {
            Decimal num7 = Decimal.Parse(row["Unit_PurPrice"].ToString());
            Decimal num8 = num1 * num7 / new Decimal(100);
            Decimal num9 = Decimal.Parse(row["QtyIn"].ToString());
            Decimal num10 = num8 * num9;
            Decimal num11 = ((num7 - num8) * num9 + num6 * num9) / num9;
            Decimal num12 = num7 - num8;
            hashtable.Clear();
            hashtable.Add("@PurchaseId", hfPurchaseId.Value);
            hashtable.Add("@PurchaseDetailId", row["PurchaseDetailId"].ToString());
            hashtable.Add("@QtyIn", row["QtyIn"]);
            hashtable.Add("@Unit_MRP", row["Unit_MRP"]);
            hashtable.Add("@Unit_PurPrice", num12);
            hashtable.Add("@Unit_SalePrice", row["Unit_SalePrice"]);
            hashtable.Add("@UserId", Session["User_Id"]);
            hashtable.Add("@UnitLandingCost", num11);
            str = obj.ExecuteScalar("ACTS_UpdtReceivesStockDtls", hashtable);
            if (str.Trim() != string.Empty)
                break;
        }
        if (str == string.Empty)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Data Saved Successfully');window.location='ReceiveStockList.aspx'", true);
        }
        else
        {
            lblMsg.Text = str;
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetFields();
    }

    private void ResetFields()
    {
        lblItemName.Text = string.Empty;
        txtRcvQty.Text = "0";
        txtUnitMRP.Text = "0";
        txtUnitPurPrice.Text = "0";
        txtUnitSalePrice.Text = "0";
        lblMsg.Text = string.Empty;
        hfPurchaseDtlId.Value = "0";
    }
}