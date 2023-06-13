using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_ReceiveStockPO : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private ScriptManager sm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            string gh = dtpPurchaseDt.GetDateValue().ToString();
            lblMsg.Text = "";
            if (Page.IsPostBack)
                return;
            rdbtnRcvStock_SelectedIndexChanged(rdbtnRcvStock, e);
            bindBankName();
            if ((int)Convert.ToInt16(ConfigurationSettings.AppSettings["mc"].ToString()) != 0)
            {
                if (Session["TransDt"] != null)
                {
                   
                    if (Session["ActsClosedStatus"].ToString() != "True")
                    {
                        bindDropDown(ddlSupplier, "Select PartyId,PartyName from dbo.ACTS_PartyMaster where PartyType='Supplier'", "PartyName", "PartyId");
                        ddlSupplier.Items.RemoveAt(0);
                        ddlSupplier.Items.Insert(0, new ListItem("- SELECT -", "0"));
                        createDataTable();
                        dtpPurchaseDt.To.Date = DateTime.Parse(Session["TransDt"].ToString());
                        txtPurchaseDt.Text = DateTime.Parse(Session["TransDt"].ToString()).ToString("dd-MMM-yyyy");
                    }
                    else
                        Response.Redirect("AcDayOpenClose.aspx?msg=Please Open the account for any valid date before Receive Stock");
                }
                else
                    Response.Redirect("AcDayOpenClose.aspx?msg=Please Open the account for any valid date before Receive Stock");
            }
            else
            {
                bindDropDown(ddlSupplier, "Select PartyId,PartyName from dbo.ACTS_PartyMaster where PartyType='Supplier'", "PartyName", "PartyId");
                ddlSupplier.Items.RemoveAt(0);
                ddlSupplier.Items.Insert(0, new ListItem("- SELECT -", "0"));
                createDataTable();
                dtpPurchaseDt.SetDateValue(DateTime.Now);
            }
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        try
        {
            drp.DataSource = null;
            drp.DataBind();
            DataTable dataTableQry = obj.GetDataTableQry(query);
            drp.DataSource = dataTableQry;
            drp.DataTextField = textfield;
            drp.DataValueField = valuefield;
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("- ALL -", "0"));
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void bindCategory(DropDownList drp, string SP, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(SP);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    private void createDataTable()
    {
        DataTable dataTable = new DataTable();
        DataColumn column1 = new DataColumn("CatCode", Type.GetType("System.String"));
        dataTable.Columns.Add(column1);
        DataColumn column2 = new DataColumn("Category", Type.GetType("System.String"));
        dataTable.Columns.Add(column2);
        DataColumn column3 = new DataColumn("ClassId", Type.GetType("System.String"));
        dataTable.Columns.Add(column3);
        DataColumn column4 = new DataColumn("ItemCode", Type.GetType("System.String"));
        dataTable.Columns.Add(column4);
        DataColumn column5 = new DataColumn("ItemName", Type.GetType("System.String"));
        dataTable.Columns.Add(column5);
        DataColumn column6 = new DataColumn("RcvQty", Type.GetType("System.Decimal"));
        dataTable.Columns.Add(column6);
        DataColumn column7 = new DataColumn("MeasuringUnit", Type.GetType("System.String"));
        dataTable.Columns.Add(column7);
        DataColumn column8 = new DataColumn("UnitMRP", Type.GetType("System.String"));
        dataTable.Columns.Add(column8);
        DataColumn column9 = new DataColumn("UnitPurPrice", Type.GetType("System.String"));
        dataTable.Columns.Add(column9);
        DataColumn column10 = new DataColumn("UnitSalePrice", Type.GetType("System.String"));
        dataTable.Columns.Add(column10);
        DataColumn column11 = new DataColumn("Currency", Type.GetType("System.String"));
        dataTable.Columns.Add(column11);
        DataColumn column12 = new DataColumn("CurrencyId", Type.GetType("System.String"));
        dataTable.Columns.Add(column12);
        DataColumn column13 = new DataColumn("TotalAmt", Type.GetType("System.Double"));
        dataTable.Columns.Add(column13);
        ViewState["Table"] = dataTable;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
       
        try
        {
           
            if (Convert.ToInt32(obj.ExecuteScalarQry("select count(*) from dbo.ACTS_PurchaseDoc where InvNo like '%Init%' and PurDate>'" + dtpPurchaseDt.GetDateValue().ToString("dd MMM yyyy") + "'").Trim()) == 0)
            {
                DataTable dt = (DataTable)ViewState["Table"];
                if (btnAdd.Text == "ADD")
                {
                    string str = "false";
                    foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                    {
                        if (row["ItemCode"].ToString() == ddlItem.SelectedValue.ToString())
                        {
                            row["RcvQty"] = txtRcvQty.Text.Trim();
                            row["MeasuringUnit"] = lblUnit.Text;
                            row["UnitMRP"] = txtUnitMRP.Text.Trim();
                            row["UnitPurPrice"] = txtUnitPurPrice.Text.Trim();
                            row["UnitSalePrice"] = txtUnitSalePrice.Text.Trim();
                            row["Currency"] = ddlCurrency.SelectedItem;
                            row["CurrencyId"] = ddlCurrency.SelectedValue;
                            row["TotalAmt"] = (Convert.ToDecimal(txtRcvQty.Text.Trim()) * Convert.ToDecimal(txtUnitPurPrice.Text.Trim()));
                            dt.AcceptChanges();
                            str = "true";
                            break;
                        }
                    }
                    if (str == "false")
                    {
                        DataRow row = dt.NewRow();
                        row["CatCode"] = ddlCategory.SelectedValue;
                        if (pnlclass.Visible)
                            row["ClassId"] = ddlClass.SelectedValue;
                        row["Category"] = ddlCategory.SelectedItem.Text;
                        row["ItemCode"] = ddlItem.SelectedValue;
                        row["ItemName"] = ddlItem.SelectedItem;
                        row["RcvQty"] = txtRcvQty.Text.Trim();
                        row["MeasuringUnit"] = lblUnit.Text;
                        row["UnitMRP"] = txtUnitMRP.Text.Trim();
                        row["UnitPurPrice"] = txtUnitPurPrice.Text.Trim();
                        row["UnitSalePrice"] = txtUnitSalePrice.Text.Trim();
                        row["Currency"] = ddlCurrency.SelectedItem;
                        row["CurrencyId"] = ddlCurrency.SelectedValue;
                        row["TotalAmt"] = (Convert.ToDecimal(txtRcvQty.Text.Trim()) * Convert.ToDecimal(txtUnitPurPrice.Text.Trim()));
                        dt.Rows.Add(row);
                    }
                }
                else
                {
                    foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
                    {
                        if (row["ItemCode"].ToString() == ViewState["ItemCode"].ToString())
                        {
                            row["RcvQty"] = txtRcvQty.Text.Trim();
                            row["MeasuringUnit"] = lblUnit.Text;
                            row["UnitMRP"] = txtUnitMRP.Text.Trim();
                            row["UnitPurPrice"] = txtUnitPurPrice.Text.Trim();
                            row["UnitSalePrice"] = txtUnitSalePrice.Text.Trim();
                            row["Currency"] = ddlCurrency.SelectedItem;
                            row["CurrencyId"] = ddlCurrency.SelectedValue;
                            row["TotalAmt"] = (Convert.ToDecimal(txtRcvQty.Text.Trim()) * Convert.ToDecimal(txtUnitPurPrice.Text.Trim()));
                            dt.AcceptChanges();
                            btnAdd.Text = "ADD";
                        }
                    }
                }
                ViewState["Table"] = dt;
                calPurAmt(dt);
                ScriptManager.RegisterStartupScript((Page)this, typeof(Page), "getInvAmt();", "getInvAmt();", true);
                Clear();
                Bind();
                sm = Master.FindControl("ScriptManager1") as ScriptManager;
                sm.SetFocus((Control)ddlCategory);
            }
            else
            {
                lblMsg.Text = "Purchase cannot be made before stock Initialization!!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void calPurAmt(DataTable dt)
    {
        double num = 0.0;
        if (dt.Rows.Count > 0)
            num = Convert.ToDouble(dt.Compute("SUM(TotalAmt)", ""));
        txtPurAmt.Text = num.ToString("0.00");
        txtInvoiceAmt.Text = num.ToString("0.00");
    }

    private void Bind()
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = (DataTable)ViewState["Table"];
        if (dataTable2.Rows.Count > 0)
        {
            btnSubmit.Enabled = true;
            gvDirectPur.Visible = true;
        }
        else
        {
            btnSubmit.Enabled = false;
            gvDirectPur.Visible = false;
        }
        gvDirectPur.DataSource = dataTable2;
        gvDirectPur.DataBind();
    }

    private void Clear()
    {
        ddlCategory.Enabled = true;
        ddlClass.SelectedIndex = 0;
        ddlItem.Enabled = true;
        ddlCategory.SelectedIndex = 0;
        ddlItem.SelectedIndex = 0;
        ddlCurrency.SelectedIndex = 0;
        txtRcvQty.Text = txtUnitMRP.Text = txtUnitPurPrice.Text = txtUnitSalePrice.Text = "";
        ddlItem.Items.Clear();
        ddlItem.Items.Insert(0, new ListItem("- Select -", "0"));
        btnAdd.Text = "ADD";
        ddlClass.Enabled = true;
    }

    protected void gvDirectPur_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = (DataTable)ViewState["Table"];
        Decimal num = new Decimal(0);
        foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            num += Convert.ToDecimal(row["RcvQty"]) * Convert.ToDecimal(row["UnitPurPrice"]);
        txtPurAmt.Text = num.ToString();
    }

    protected void gvDirectPur_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["Table"];
            DataRow[] dataRowArray = dt.Select("ItemCode=" + e.CommandArgument.ToString());
            if (e.CommandName == "Modify")
            {
                ddlCategory.SelectedValue = dataRowArray[0]["CatCode"].ToString();
                ddlCategory_SelectedIndexChanged(sender, (EventArgs)e);
                if (dataRowArray[0]["ClassId"].ToString().Trim() != "")
                {
                    ddlClass.SelectedValue = dataRowArray[0]["ClassId"].ToString();
                    ddlClass_SelectedIndexChanged(sender, (EventArgs)e);
                }
                ddlItem.SelectedValue = dataRowArray[0]["ItemCode"].ToString();
                txtRcvQty.Text = dataRowArray[0]["RcvQty"].ToString();
                txtUnitMRP.Text = dataRowArray[0]["UnitMRP"].ToString();
                txtUnitPurPrice.Text = dataRowArray[0]["UnitPurPrice"].ToString();
                txtUnitSalePrice.Text = dataRowArray[0]["UnitSalePrice"].ToString();
                ddlCurrency.SelectedValue = dataRowArray[0]["CurrencyId"].ToString();
                ViewState["ItemCode"] = dataRowArray[0]["ItemCode"].ToString();
                btnAdd.Text = "UPDATE";
                ddlCategory.Enabled = false;
                ddlClass.Enabled = false;
                ddlItem.Enabled = false;
                sm = Master.FindControl("ScriptManager1") as ScriptManager;
                sm.SetFocus((Control)txtRcvQty);
            }
            else
            {
                if (!(e.CommandName == "Remove"))
                    return;
                foreach (DataRow row in dataRowArray)
                    dt.Rows.Remove(row);
                dt.AcceptChanges();
                ViewState["Table"] = dt;
                calPurAmt(dt);
                Bind();
                sm = Master.FindControl("ScriptManager1") as ScriptManager;
                sm.SetFocus((Control)ddlCategory);
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        sm = Master.FindControl("ScriptManager1") as ScriptManager;
        sm.SetFocus((Control)ddlCategory);
        try
        {
            if (ddlCategory.SelectedIndex > 0)
            {
                if (ddlCategory.SelectedValue == "1" || obj.ExecuteScalarQry("Select ParentCatId from dbo.SI_CategoryMaster where CatId=" + ddlCategory.SelectedValue.ToString()).Trim() == "1")
                {
                    pnlclass.Visible = true;
                    pnlclassVal.Visible = true;
                    bindDropDown(ddlClass, "select ClassID, ClassName from dbo.PS_ClassMaster order by ClassID", "ClassName", "ClassID");
                }
                else
                {
                    pnlclass.Visible = false;
                    pnlclassVal.Visible = false;
                }
                ht.Clear();
                ht.Add("@NoOfChar", 10);
                ht.Add("@Catid", int.Parse(ddlCategory.SelectedValue));
                if (ddlClass.SelectedIndex > 0 && pnlclassVal.Visible)
                    ht.Add("@ClassId", ddlClass.SelectedValue);
                dt = obj.GetDataTable("ACTS_GetItemList", ht);
                ddlItem.DataSource = dt;
                ddlItem.DataTextField = "ItemNameFull";
                ddlItem.DataValueField = "ItemCode";
                ddlItem.DataBind();
                if (dt.Rows.Count > 0)
                    ddlItem.Items.Insert(0, new ListItem("- Item List -", "0"));
                else
                    ddlItem.Items.Insert(0, new ListItem("- No Items -", "0"));
            }
            else
            {
                pnlclass.Visible = false;
                pnlclassVal.Visible = false;
                ddlItem.Items.Clear();
                ddlItem.Items.Insert(0, new ListItem("- Select -", "0"));
                lblUnit.Text = "";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }
    protected void ddlStream_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            ht.Add("@NoOfChar", 10);
            ht.Add("@Catid", int.Parse(ddlCategory.SelectedValue));
            ht.Add("@ClassId", int.Parse(ddlClass.SelectedValue));
            ht.Add("@StreamId", int.Parse(ddlStream.SelectedValue));
            dt = obj.GetDataTable("ACTS_GetItemListForClass", ht);
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemNameFull";
            ddlItem.DataValueField = "ItemCode";
            ddlItem.DataBind();
            if (dt.Rows.Count > 0)
                ddlItem.Items.Insert(0, new ListItem("- Item List -", "0"));
            else
                ddlItem.Items.Insert(0, new ListItem("- No Items -", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlClass.SelectedIndex == 14 || ddlClass.SelectedIndex == 15)
        {

            ddlStream.Enabled = true;
            DataTable dataTable = new DataTable();
            ddlStream.DataSource = obj.GetDataTableQry("select distinct ps_Classwisestudent.stream ,[PS_StreamMaster].[Description] from ps_Classwisestudent inner join  PS_StreamMaster on ps_Classwisestudent.Stream=PS_StreamMaster.StreamID where ps_Classwisestudent.ClassID=" + ddlClass.SelectedValue).DefaultView;
            ddlStream.DataTextField = "Description";
            ddlStream.DataValueField = "stream";
            ddlStream.DataBind();
            ddlStream.Items.Insert(0, new ListItem("STREAM", "0"));

            sm = Master.FindControl("ScriptManager1") as ScriptManager;
            sm.SetFocus((Control)ddlClass);
            try
            {
                ht.Clear();
                ht.Add("@NoOfChar", 10);
                ht.Add("@Catid", int.Parse(ddlCategory.SelectedValue));
                ht.Add("@ClassId", int.Parse(ddlClass.SelectedValue));
                ht.Add("@StreamId", int.Parse(ddlStream.SelectedValue));
                dt = obj.GetDataTable("ACTS_GetItemListForClass", ht);
                ddlItem.DataSource = dt;
                ddlItem.DataTextField = "ItemNameFull";
                ddlItem.DataValueField = "ItemCode";
                ddlItem.DataBind();
                if (dt.Rows.Count > 0)
                    ddlItem.Items.Insert(0, new ListItem("- Item List -", "0"));
                else
                    ddlItem.Items.Insert(0, new ListItem("- No Items -", "0"));
            }
            catch (Exception ex)
            {
            }
        }
        else
        {
            ddlStream.SelectedIndex = 0;
            ddlStream.Enabled = false;
            sm = Master.FindControl("ScriptManager1") as ScriptManager;
            sm.SetFocus((Control)ddlClass);
            try
            {
                ht.Clear();
                ht.Add("@NoOfChar", 10);
                ht.Add("@Catid", int.Parse(ddlCategory.SelectedValue));
                ht.Add("@ClassId", int.Parse(ddlClass.SelectedValue));
                ht.Add("@StreamId", 0);
                dt = obj.GetDataTable("ACTS_GetItemListForClass", ht);
                ddlItem.DataSource = dt;
                ddlItem.DataTextField = "ItemNameFull";
                ddlItem.DataValueField = "ItemCode";
                ddlItem.DataBind();
                if (dt.Rows.Count > 0)
                    ddlItem.Items.Insert(0, new ListItem("- Item List -", "0"));
                else
                    ddlItem.Items.Insert(0, new ListItem("- No Items -", "0"));
            }
            catch (Exception ex)
            {
            }
        }
        
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        obj = new clsDAL();
        try
        {
       string str = obj.ExecuteScalar("ACTS_ChkPurchaseDt", new Hashtable()
      {
        {
           "@PurDate",
           dtpPurchaseDt.GetDateValue().ToString("dd MMM yyyy")
        }
      });
            if (str.Trim() == "")
            {
                if (!(rdbtnRcvStock.SelectedValue == "1"))
                    return;
                ReceiveDirectly(sender, e);
            }
            else if (str.Trim().ToUpper() == "FY")
            {
                lblMsg.Text = "Purchase Date not in the current financial year!!";
                lblMsg.ForeColor = Color.Red;
            }
            else if (str.Trim().ToUpper() == "PUR")
            {
                lblMsg.Text = "Purchase already made after this date!!";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                if (!(str.Trim().ToUpper() == "INIT"))
                    return;
                lblMsg.Text = "Purchase cannot be made before date of stock Initialization!!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void ReceiveDirectly(object sender, EventArgs e)
    {
        SqlConnection sqlConnection = new SqlConnection();
        sqlConnection.ConnectionString = ConfigurationManager.AppSettings["conString"];
        SqlTransaction sqlTransaction = (SqlTransaction)null;
        try
        {
            int num1 = 0;
            int num2 = 0;
            if (rbtnMode.SelectedValue == "C")
            {
                num1 = 3;
                num2 = 4;
            }
            else if (rbtnMode.SelectedValue == "B")
            {
                num1 = Convert.ToInt32(drpBankName.SelectedValue);
                num2 = 4;
            }
            int num3 = !(rdbtnPayMode.SelectedValue == "1") ? 0 : 1;
            double num4 = double.Parse(txtPurAmt.Text.Trim());
            if (txtDiscAmt.Text.Trim() != "")
                num4 -= double.Parse(txtDiscAmt.Text.Trim());
            if (txtVAT.Text.Trim() != "")
                num4 += double.Parse(txtVAT.Text.Trim());
            if (txtOtherCharge.Text.Trim() != "")
                num4 += double.Parse(txtOtherCharge.Text.Trim());
            if (txtShipCharge.Text.Trim() != "")
                num4 += double.Parse(txtShipCharge.Text.Trim());
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            sqlTransaction = sqlConnection.BeginTransaction();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "ACTS_InsReceiveOrder";
            sqlCommand.Transaction = sqlTransaction;
            SqlParameter sqlParameter1 = new SqlParameter("@PurDate", dtpPurchaseDt.GetDateValue().ToString("dd MMM yyyy"));
            SqlParameter sqlParameter2 = new SqlParameter("@InvNo", txtInvoice.Text.Trim());
            SqlParameter sqlParameter3 = new SqlParameter("@SupplierId", int.Parse(ddlSupplier.SelectedValue));
            SqlParameter sqlParameter4 = new SqlParameter("@TotPurAmt", double.Parse(txtPurAmt.Text.Trim()));
            SqlParameter sqlParameter5 = new SqlParameter("@VatAmt", txtVAT.Text.Trim());
            SqlParameter sqlParameter6 = new SqlParameter("@AddnlChrgDesc", txtDesc.Text.Trim());
            SqlParameter sqlParameter7 = new SqlParameter("@AddnlChrgAmt", txtOtherCharge.Text.Trim());
            SqlParameter sqlParameter8 = new SqlParameter("@ShipCharge", txtShipCharge.Text.Trim());
            SqlParameter sqlParameter9 = new SqlParameter("@TotBillAmt", num4);
            SqlParameter sqlParameter10 = new SqlParameter("@Remarks", txtRemarks.Text.Trim());
            SqlParameter sqlParameter11 = new SqlParameter("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
            SqlParameter sqlParameter12 = new SqlParameter("@UserId", int.Parse(Session["User_Id"].ToString()));
            SqlParameter sqlParameter13 = new SqlParameter("@PurID", SqlDbType.BigInt);
            sqlParameter13.Direction = ParameterDirection.Output;
            SqlParameter sqlParameter14 = new SqlParameter("@msg", SqlDbType.VarChar, 20);
            sqlParameter14.Direction = ParameterDirection.Output;
            SqlParameter sqlParameter15 = new SqlParameter("@RcvdFrom", ddlSupplier.SelectedItem.Text.Trim());
            SqlParameter sqlParameter16 = new SqlParameter("@AccountHeadDR", num2);
            SqlParameter sqlParameter17 = new SqlParameter("@AccountHeadCR", num1);
            SqlParameter sqlParameter18 = new SqlParameter("@PurType", num3);
            SqlParameter sqlParameter19 = new SqlParameter("@InstrumentNo", txtInstrumentNo.Text.ToString().Trim());
            SqlParameter sqlParameter20 = new SqlParameter("@InstrumentDt", dtpInstrumentDt.GetDateValue());
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
            sqlCommand.Parameters.Add(sqlParameter17);
            sqlCommand.Parameters.Add(sqlParameter18);
            if (txtInstrumentNo.Text.Trim() != "")
                sqlCommand.Parameters.Add(sqlParameter19);
            if (txtInstrumentDt.Text.Trim() != "")
                sqlCommand.Parameters.Add(sqlParameter20);
            sqlCommand.ExecuteNonQuery();
            if (sqlParameter14.Value.ToString() == "Invoice Exist")
            {
                lblMsg.Text = sqlParameter14.Value.ToString();
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                long int64 = Convert.ToInt64(sqlParameter13.Value);
                DataTable dataTable = ViewState["Table"] as DataTable;
                Decimal num5 = new Decimal(0);
                Decimal num6 = new Decimal(0);
                Decimal num7 = new Decimal(0);
                Decimal num8 = new Decimal(0);
                if (txtDiscPer.Text.Trim() != "")
                    num5 = Convert.ToDecimal(txtDiscPer.Text);
                if (txtVAT.Text.Trim() != "")
                    num6 = Convert.ToDecimal(txtVAT.Text);
                if (txtShipCharge.Text.Trim() != "")
                    num7 = Convert.ToDecimal(txtShipCharge.Text);
                if (txtOtherCharge.Text.Trim() != "")
                    num8 = Convert.ToDecimal(txtOtherCharge.Text);
                Decimal num9 = Convert.ToDecimal(dataTable.Compute("SUM(RcvQty)", ""));
                Decimal num10 = (num6 + num7 + num8) / num9;
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                {
                    Decimal num11 = Decimal.Parse(row["UnitPurPrice"].ToString());
                    Decimal num12 = num5 * num11 / new Decimal(100);
                    Decimal num13 = Decimal.Parse(row["RcvQty"].ToString());
                    Decimal num14 = num12 * num13;
                    Decimal num15 = ((num11 - num12) * num13 + num10 * num13) / num13;
                    Decimal num16 = num11 - num12;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "ACTS_InsReceiveOrderDtls";
                    sqlCommand.Parameters.Clear();
                    ViewState["ItemCode"] = row["ItemCode"].ToString();
                    SqlParameter sqlParameter21 = new SqlParameter("@PurchaseId", long.Parse(int64.ToString()));
                    SqlParameter sqlParameter22 = new SqlParameter("@ItemCode", int.Parse(row["ItemCode"].ToString()));
                    SqlParameter sqlParameter23 = new SqlParameter("@QtyIn", float.Parse(row["RcvQty"].ToString()));
                    SqlParameter sqlParameter24 = new SqlParameter("@Unit_MRP", double.Parse(row["UnitMRP"].ToString()));
                    SqlParameter sqlParameter25 = new SqlParameter("@Unit_PurPrice", num16);
                    SqlParameter sqlParameter26 = new SqlParameter("@Unit_SalePrice", double.Parse(row["UnitSalePrice"].ToString()));
                    SqlParameter sqlParameter27 = new SqlParameter("@CurrencyId", int.Parse(row["CurrencyId"].ToString()));
                    SqlParameter sqlParameter28 = new SqlParameter("@UserId", int.Parse(Session["User_Id"].ToString()));
                    SqlParameter sqlParameter29 = new SqlParameter("@UnitLandingCost", num15);
                    SqlParameter sqlParameter30 = new SqlParameter("@PurDtlID", SqlDbType.BigInt);
                    sqlParameter30.Direction = ParameterDirection.Output;
                    SqlParameter sqlParameter31 = new SqlParameter("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
                    SqlParameter sqlParameter32 = new SqlParameter("@PurDate", dtpPurchaseDt.GetDateValue().ToString("dd MMM yyyy"));
                    sqlCommand.Parameters.Add(sqlParameter21);
                    sqlCommand.Parameters.Add(sqlParameter22);
                    sqlCommand.Parameters.Add(sqlParameter23);
                    sqlCommand.Parameters.Add(sqlParameter24);
                    sqlCommand.Parameters.Add(sqlParameter25);
                    sqlCommand.Parameters.Add(sqlParameter26);
                    sqlCommand.Parameters.Add(sqlParameter27);
                    sqlCommand.Parameters.Add(sqlParameter28);
                    sqlCommand.Parameters.Add(sqlParameter30);
                    sqlCommand.Parameters.Add(sqlParameter29);
                    sqlCommand.Parameters.Add(sqlParameter31);
                    sqlCommand.Parameters.Add(sqlParameter32);
                    sqlCommand.ExecuteNonQuery();
                    Convert.ToInt64(sqlParameter30.Value);
                }
                lblMsg.Text = "Stock Received Successfully";
                lblMsg.ForeColor = Color.Green;
                ClearAll();
                ViewState["Table"] = null;
                createDataTable();
                Bind();
            }
            sqlTransaction.Commit();
            sqlTransaction.Dispose();
            sqlConnection.Dispose();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
            sqlTransaction.Rollback();
        }
    }

    private void ClearAll()
    {
        rdbtnPayMode.SelectedValue = "0";
        rdbtnPayMode_SelectedIndexChanged(rdbtnPayMode, EventArgs.Empty);
        rbtnMode_SelectedIndexChanged(rbtnMode, EventArgs.Empty);
        ddlSupplier.SelectedIndex = 0;
        dtpPurchaseDt.SetDateValue(DateTime.Today);
        txtInvoice.Text = txtRemarks.Text = txtDesc.Text = "";
        txtPurAmt.Text = txtVAT.Text = txtShipCharge.Text = txtOtherCharge.Text = txtInvoiceAmt.Text = "0";
        txtDiscPer.Text = txtDiscAmt.Text = "0.0000";
        txtInstrumentDt.Text = txtInstrumentNo.Text = "";
        pnlSave.Enabled = false;
    }

    protected void btnShowList_Click(object sender, EventArgs e)
    {
        Response.Redirect("ReceiveStockList.aspx");
    }

    protected void rdbtnRcvStock_SelectedIndexChanged(object sender, EventArgs e)
    {
        sm = Master.FindControl("ScriptManager1") as ScriptManager;
        sm.SetFocus((Control)rdbtnRcvStock);
        try
        {
           
            dtpPurchaseDt.SetDateValue(DateTime.Now);
            lblMsg.Text = "";
            ddlSupplier.SelectedIndex = 0;
            if (!(rdbtnRcvStock.SelectedValue == "1"))
                return;
            dtpPurchaseDt.Visible = true;
            txtPurchaseDt.Enabled = true;
            bindCategory(ddlCategory, "Proc_DrpGetCat", "CatName", "CatId");
            bindDropDown(ddlCurrency, "select CurrencyId,CurrencyCode+'('+CurrencySymbol+')' as Currency from dbo.ACTS_CurrencyMaster", "Currency", "CurrencyId");
            ddlCurrency.Items.RemoveAt(0);
            pnlRcvDirect.Visible = true;
            if (gvDirectPur.Rows.Count > 0)
            {
                pnlSave.Enabled = true;
                gvDirectPur.Visible = true;
            }
            else
            {
                pnlSave.Enabled = false;
                gvDirectPur.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void rdbtnPayMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtnPayMode.SelectedValue == "0")
            pnlSave.Enabled = false;
        else
            pnlSave.Enabled = true;
        rdbtnPayMode.Focus();
    }

    protected void rbtnMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ScriptManager control = (ScriptManager)Master.FindControl("ScriptManager1");
        bindBankName();
        enabletxt();
        control.SetFocus(rbtnMode.ClientID);
    }

    private void enabletxt()
    {
        if (rbtnMode.SelectedValue == "C")
        {
            txtInstrumentNo.Enabled = false;
            txtInstrumentDt.Enabled = false;
            txtInstrumentDt.Text = string.Empty;
            txtInstrumentNo.Text = string.Empty;
            spBankNm.Visible = false;
            spInstDt.Visible = false;
            spInstNo.Visible = false;
        }
        else
        {
            txtInstrumentNo.Enabled = true;
            txtInstrumentDt.Enabled = true;
            spBankNm.Visible = true;
            spInstDt.Visible = true;
            spInstNo.Visible = true;
        }
    }

    private void bindBankName()
    {
        drpBankName.Items.Clear();
        drpBankName.Enabled = false;
        if (!(rbtnMode.SelectedValue == "B"))
            return;
        drpBankName.Enabled = true;
        drpBankName.DataSource = new clsDAL().GetDataTableQry("SELECT AcctsHeadId,AcctsHead FROM dbo.Acts_AccountHeads WHERE AG_Code IN (7) ORDER BY AcctsHead".ToString().Trim()).DefaultView;
        drpBankName.DataTextField = "AcctsHead";
        drpBankName.DataValueField = "AcctsHeadId";
        drpBankName.DataBind();
        drpBankName.Items.Insert(0, new ListItem("Select Bank Name", "0"));
    }
}