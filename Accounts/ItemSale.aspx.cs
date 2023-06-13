using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_ItemSale : System.Web.UI.Page
{
    private DataTable dtItems = new DataTable();
    private NumberFormatInfo format = new NumberFormatInfo();
    private int[] indSizes = new int[4] { 3, 2, 2, 2 };
    private clsDAL DAL = new clsDAL();
   
    private static int edittemp;
    private ScriptManager sm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        litHeaderMsg.Text = litFooterMsg.Text = string.Empty;
        if (!Page.IsPostBack)
        {
            PaymentMode_CheckedChanged(optPmtCash, (EventArgs)null);
            ViewState["NewItem"] = null;
            btnPrepBill.Enabled = false;
            ViewState["ItemCart"] = new DataTable();
            ViewState["SalesDetails"] = new DataTable();
            ViewState["InvNo"] = false;
            ViewState["CheckedItems"] = new List<string>();
            Accounts_ItemSale.edittemp = 0;
            txttotalamt.Text = "";
            dtpSaleDate.SetDateValue(DateTime.Now);
            txttotalamt.Text = "0.0";
            txttotaldis.Text = "0.0";
            txttotalvat.Text = "0.0";
            txttotalbill.Text = "0.0";
            ddlStream.Items.Insert(0, new ListItem("STREAM", "0"));
            BindDropDown(drpSession, "select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc", "SessionYr", "SessionYr");
            BindDropDown(drpclass, "select ClassID, ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
            BindDropDown(drpBank, "SELECT AcctsHeadId, AcctsHead FROM dbo.ACTS_AccountHeads a inner join dbo.Acts_AccountGroups ag on ag.AG_Code=a.AG_Code WHERE (a.AG_Code in (7) or ag.AG_Parent=7) ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
        }
        format.CurrencyDecimalDigits = 2;
        format.CurrencyDecimalSeparator = ".";
        format.CurrencyGroupSeparator = ",";
        format.CurrencySymbol = "";
        format.CurrencyGroupSizes = indSizes;
    }

    private void BindDropDown(DropDownList drp, string query, string TextField, string ValueField)
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = DAL.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = TextField;
        drp.DataValueField = ValueField;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void bindCategory(DropDownList drp, string SP, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry = DAL.GetDataTableQry(SP);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DAL = new clsDAL();
        if (DAL.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=8").ToString().Trim() == "")
        {
            ScriptManager.RegisterStartupScript((Page)this, GetType(), "alertMessage", "alert('No Account Head For Book Sale!! Please Set Account Head To Continue!!'); window.location='" + Request.ApplicationPath + "/Masters/SetAccountHead.aspx'", true);
        }
        else
        {
            DAL = new clsDAL();
            DataTable dataTableQry = DAL.GetDataTableQry("select top(1) FY,StartDate,EndDate from dbo.ACTS_FinancialYear where (IsFinalized is null or IsFinalized=0) order by FY_Id desc");
            if (dataTableQry.Rows.Count > 0)
            {
                //if (Convert.ToDateTime(dtpSaleDate.GetDateValue().ToString("dd-MMM-yyyy")) >= Convert.ToDateTime(dataTableQry.Rows[0]["StartDate"]) && Convert.ToDateTime(dtpSaleDate.GetDateValue().ToString("dd-MMM-yyyy")) <= Convert.ToDateTime(dataTableQry.Rows[0]["EndDate"]))
                //{
                    string str = SaveData();
                    if (str.Trim().Equals("Data saved successfully !"))
                    {
                        clearcontrol();
                        litFooterMsg.Text = litHeaderMsg.Text = "<div style='padding:5px;font-weight:bold;color:white;background-color:green;border:solid 2px black;'>" + str + "</div>";
                    }
                    else
                        litFooterMsg.Text = litHeaderMsg.Text = "<div style='padding:5px;font-weight:bold;color:white;background-color:red;border:solid 2px black;'>" + str + "</div>";
                // }
                // else
                //litFooterMsg.Text = litHeaderMsg.Text = "<div style='padding:5px;font-weight:bold;color:white;background-color:red;border:solid 2px black;'>Transaction Date not in the current financial year!</div>";
            }
            else
                litFooterMsg.Text = litHeaderMsg.Text = "<div style='padding:5px;font-weight:bold;color:white;background-color:red;border:solid 2px black;'>No running Financial available !!</div>";
        }
    }

    private string SaveData()
    {
        string str1 = string.Empty;
        try
        {
            DAL = new clsDAL();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@InvDate", dtpSaleDate.GetDateValue().ToString("dd-MMM-yyyy"));
            if (drpSaleList.SelectedIndex == 0)
                hashtable.Add("@AdmnNo", Convert.ToInt64(drpstudent.SelectedValue.ToString()));
            hashtable.Add("@TotalAmt", Convert.ToDecimal(txttotalbill.Text));
            hashtable.Add("@TotDiscount", Convert.ToDecimal(txttotaldis.Text));
            hashtable.Add("@Tot_VAT_CST", Convert.ToDecimal(txttotalvat.Text));
            if (rbtnInvType.SelectedValue == "I")
                hashtable.Add("@NetAmt", Convert.ToDecimal(txttotalbill.Text.Trim()));
            else
                hashtable.Add("@NetAmt", (Convert.ToDecimal(txttotalbill.Text.Trim()) + Convert.ToDecimal(txttotalvat.Text.Trim())));
            hashtable.Add("@AddnlDiscount", 0);
            hashtable.Add("@AddnlCharges", 0);
            if (txtRcvdAmt.Text == "")
                txtRcvdAmt.Text = "0.00";
            hashtable.Add("@Debit", (Convert.ToDouble(txttotalamt.Text) - Convert.ToDouble(txtRcvdAmt.Text)));
            hashtable.Add("@PaidAmt", Convert.ToDouble(txtRcvdAmt.Text));
            hashtable.Add("@Session", drpSession.SelectedValue.ToString());
            hashtable.Add("@NetPayableAmt", Convert.ToDecimal(txttotalamt.Text));
            hashtable.Add("@IsCashInvoice", (optPmtCash.Checked ? 1 : 0));
            hashtable.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
            hashtable.Add("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
            hashtable.Add("@mc", ConfigurationManager.AppSettings["mc"]);
            if (optPmtBank.Checked)
            {
                hashtable.Add("@InstrumentNo", txtInstrNo.Text);
                hashtable.Add("@InstrumentDt", dtpInstDate.GetDateValue().ToString());
                hashtable.Add("@DrawnOnbank", txtBankName.Text);
                hashtable.Add("@BankAcHead", int.Parse(drpBank.SelectedValue.ToString()));
            }
            DAL = new clsDAL();
            string str2 = DAL.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=8");
            hashtable.Add("@AcHead", int.Parse(str2.Trim()));
            if (drpSaleList.SelectedIndex == 1)
                hashtable.Add("@ReceivedFrom", (txtRcvdFrom.Text + "    Class:" + drpclass.SelectedItem.Text.Trim()));
            else
                hashtable.Add("@ReceivedFrom", txtRcvdFrom.Text);
            Sale sale = new Sale();
            DataTable dataTable = (DataTable)ViewState["SalesDetails"];
            long num = 0;
            str1 = sale.SaveSales(dataTable, hashtable, out num);
            ViewState["InvNo"] = num;
            str1.Trim().Equals("Data saved successfully !");
        }
        catch (Exception ex)
        {
        }
        return str1;
    }

    private TextBox checkblank(TextBox txt)
    {
        if (txt.Text == "")
            txt.Text = "0";
        return txt;
    }

    private string checkblank(string value)
    {
        if (value == "")
            value = "0";
        return value;
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        clearcontrol();
    }

    protected void clearcontrol()
    {
        txtissuedate.Text = DateTime.Now.ToString("dd-MM-yyyy");
        txttotalamt.Text = "0";
        txttotaldis.Text = "0";
        txttotalvat.Text = "0";
        txttotalbill.Text = "0";
        dtpSaleDate.SetDateValue(DateTime.Now);
        btnSubmit.Visible = false;
        btnSubmitReturn.Visible = false;
        btnSubmitPrint.Visible = false;
        Accounts_ItemSale.edittemp = 0;
        gvItemsForSale.DataSource = null;
        gvItemsForSale.DataBind();
        btnPrepBill.Enabled = false;
        rbtnInvType.SelectedValue = "I";
        optPmtCash.Checked = true;
        optPmtBank.Checked = false;
        txtRcvdFrom.Text = string.Empty;
        PaymentMode_CheckedChanged(optPmtCash, (EventArgs)null);
    }

    private void UpdateVatSummary()
    {
        phVatDesc.Controls.Clear();
        phVatValue.Controls.Clear();
        DataTable dataTable = (DataTable)ViewState["SalesDetails"];
        DataTable table = dataTable.DefaultView.ToTable(true, "VCrate");
        if (table.Rows.Count == 0)
            return;
        if (table.Rows[0]["VCrate"].ToString().Trim().Equals(string.Empty))
            table.Rows.RemoveAt(0);
        for (int index = 0; index < table.Rows.Count; ++index)
        {
            if (double.Parse(table.Rows[index]["VCrate"].ToString()) > 0.0)
            {
                Label label1 = new Label();
                label1.ID = "lblVatDesc" + index.ToString();
                label1.Style["font-weight"] = "Bold";
                label1.Text = "VAT @" + table.Rows[index]["VCrate"].ToString() + ":";
                phVatDesc.Controls.Add((Control)label1);
                phVatDesc.Controls.Add((Control)new LiteralControl("<br>"));
                Label label2 = new Label();
                label2.ID = "lblVatValue" + index.ToString();
                string str = string.Format("{0:f2}", dataTable.Compute("SUM(TotalVCAmt)", "VCrate=" + table.Rows[index]["VCrate"].ToString()));
                label2.Text = "&nbsp;" + str;
                phVatValue.Controls.Add((Control)label2);
                phVatValue.Controls.Add((Control)new LiteralControl("<br>"));
            }
        }
    }

    private void updatedata()
    {
        DataTable dataTable = ViewState["SalesDetails"] as DataTable;
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        Decimal num3 = new Decimal(0);
        Decimal num4 = new Decimal(0);
        Decimal num5 = new Decimal(0);
        Decimal num6 = new Decimal(0);
        Decimal num7 = new Decimal(0);
        Decimal num8 = new Decimal(0);
        Decimal num9 = new Decimal(0);
        Decimal num10 = new Decimal(0);
        if (dataTable.Rows.Count > 1)
        {
            for (int index = 1; index < dataTable.Rows.Count; ++index)
            {
                num4 += Convert.ToDecimal(dataTable.Rows[index]["Qty"]);
                num5 += Convert.ToDecimal(dataTable.Rows[index]["ItemRate"]);
                Decimal num11 = new Decimal(0);
                Decimal d;
                if (rbtnInvType.SelectedValue == "I")
                {
                    Decimal num12 = Convert.ToDecimal(dataTable.Rows[index]["ItemRate"]) * Convert.ToDecimal(dataTable.Rows[index]["Qty"]);
                    Decimal num13 = Convert.ToDecimal(dataTable.Rows[index]["VCrate"]) / (Convert.ToDecimal(dataTable.Rows[index]["VCrate"]) + new Decimal(100)) * num12;
                    Decimal num14 = num12 - num13;
                    Decimal num15 = num14 * Convert.ToDecimal(dataTable.Rows[index]["DisRate"]) / new Decimal(100);
                    Decimal num16 = Convert.ToDecimal(num14 - num15);
                    Decimal num17 = Convert.ToDecimal(dataTable.Rows[index]["VCrate"]) * num16 / new Decimal(100);
                    d = num16 + num17;
                    num2 += num17;
                    num3 += num15;
                }
                else
                {
                    d = Convert.ToDecimal(Convert.ToDecimal(Convert.ToDecimal(dataTable.Rows[index]["ItemRate"]) - Convert.ToDecimal(dataTable.Rows[index]["ItemRate"]) * Convert.ToDecimal(dataTable.Rows[index]["DisRate"]) / new Decimal(100)) * (Decimal)Convert.ToInt32(dataTable.Rows[index]["Qty"]));
                    Decimal num12 = Math.Round(Convert.ToDecimal(Convert.ToDecimal(d) * Convert.ToDecimal(dataTable.Rows[index]["VCrate"]) / new Decimal(100)), 2);
                    num2 += num12;
                    num3 += Convert.ToDecimal(Convert.ToDecimal((Decimal)Convert.ToInt32(dataTable.Rows[index]["Qty"]) * Convert.ToDecimal(dataTable.Rows[index]["ItemRate"])) * (Decimal)Convert.ToInt32(dataTable.Rows[index]["DisRate"])) / new Decimal(100);
                }
                num6 += Convert.ToDecimal(Math.Round(d, 2));
                num8 = Convert.ToDecimal(dataTable.Rows[index]["addcharges"]);
                num7 = Convert.ToDecimal(dataTable.Rows[index]["adddis"]);
            }
            txttotalamt.Text = string.Format("{0:F2}", (Convert.ToDecimal(dataTable.Compute("sum(Amt)", "")) - num7 + num8));
            txttotalbill.Text = string.Format("{0:F2}", num6);
            txttotaldis.Text = string.Format("{0:F2}", num3);
            txttotalvat.Text = string.Format("{0:F2}", num2);
            Decimal num18 = new Decimal(0);
            Decimal num19 = new Decimal(0);
            btnSubmit.Visible = true;
            btnSubmitReturn.Visible = true;
            btnSubmitPrint.Visible = true;
            ViewState["SalesDetails"] = dataTable;
        }
        else
        {
            txttotalamt.Text = "0.00";
            txttotalbill.Text = "0.00";
            txttotaldis.Text = "0.00";
            txttotalvat.Text = "0.00";
            btnSubmit.Visible = false;
            btnSubmitReturn.Visible = false;
            btnSubmitPrint.Visible = false;
        }
    }

    protected void btnSubmitPrint_Click(object sender, EventArgs e)
    {
        DAL = new clsDAL();
        if (DAL.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=8").ToString().Trim() == "")
        {
            ScriptManager.RegisterStartupScript((Page)this, GetType(), "alertMessage", "alert('No Account Head Set To Receive Fee!! Please Set Account Head To Continue!!'); window.location='" + Request.ApplicationPath + "/Masters/SetAccountHead.aspx'", true);
        }
        else
        {
            string str1 = SaveData();
            if (str1.Trim().Equals("Data saved successfully !"))
            {
                clearcontrol();
                string str2 = "ItemSaleMiniInv.aspx?InvNo=" + ViewState["InvNo"].ToString();
                ScriptManager.RegisterClientScriptBlock((Control)btnSubmitPrint, btnSubmitPrint.GetType(), "Message", "alert('" + str1 + "');window.open('" + str2 + "');", true);
            }
            else
                litFooterMsg.Text = litHeaderMsg.Text = "<div style='padding:5px;font-weight:bold;color:white;background-color:red;border:solid 2px black;'>" + str1 + "</div>";
        }
    }

    protected void btnSoldList_Click(object sender, EventArgs e)
    {
        Response.Redirect("ItemSaleList.aspx");
    }
    protected void ddlStream_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
        gvItemsForSale.Visible = true;
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        if (drpclass.SelectedIndex > 0)
            hashtable.Add("@ForClassId", drpclass.SelectedValue);
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        hashtable.Add("@SaleDt", dtpSaleDate.GetDateValue().ToString("dd MMM yyyy"));
        hashtable.Add("@StreamId", ddlStream.SelectedValue);
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = DAL.GetDataTable("ACTS_GetItemDetailsForSale", hashtable);
        dataTable2.Columns.Add("Qty", typeof(Decimal));
        dataTable2.Columns.Add("Amount", typeof(Decimal));
        dataTable2.Columns.Add("Discount");
        dataTable2.Columns.Add("TaxAmount", typeof(Decimal));
        gvItemsForSale.DataSource = dataTable2;
        gvItemsForSale.DataBind();
        int num = 0;
        foreach (GridViewRow row in gvItemsForSale.Rows)
        {
            ((TextBox)row.FindControl("txtQty")).Text = "1";
            dataTable2.Rows[num++]["Qty"] = ((TextBox)row.FindControl("txtQty")).Text;
        }
        hfGvRecCount.Value = gvItemsForSale.Rows.Count.ToString();
        ViewState["ItemCart"] = dataTable2;
        ViewState["NewItem"] = dataTable2;
        txttotalbill.Text = "0.00";
        txttotaldis.Text = "0.00";
        if (gvItemsForSale.Rows.Count <= 0)
            return;
        btnPrepBill.Enabled = true;
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpclass.SelectedIndex == 14 || drpclass.SelectedIndex == 15)
        {
            ddlStream.Enabled = false;
            ddlStream.Enabled = true;
            DataTable dataTable = new DataTable();
            ddlStream.DataSource = DAL.GetDataTableQry("select distinct ps_Classwisestudent.stream ,[PS_StreamMaster].[Description] from ps_Classwisestudent inner join  PS_StreamMaster on ps_Classwisestudent.Stream=PS_StreamMaster.StreamID where ps_Classwisestudent.ClassID=" + drpclass.SelectedValue).DefaultView;
            ddlStream.DataTextField = "Description";
            ddlStream.DataValueField = "stream";
            ddlStream.DataBind();
            ddlStream.Items.Insert(0, new ListItem("STREAM", "0"));
        }
        else
        {
        }
        fillstudent();
        fillcat();
        FillBookList();
        drpstudent.Focus();
        lblMsg2.Text = "";
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpstudent.Items.Clear();
        drpclass.SelectedIndex = 0;
        txtadmnno.Text = string.Empty;
        drpclass.Focus();
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtadmnno.Text = drpstudent.SelectedValue;
        if (drpSaleList.SelectedIndex == 0)
            txtRcvdFrom.Text = drpstudent.SelectedItem.Text.Trim();
        lblMsg2.Text = "";
    }

    private void fillstudent()
    {
        DAL = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select fullname,s.admnno from PS_ClasswiseStudent cs join ps_studmaster s on ");
        stringBuilder.Append(" s.admnno=cs.admnno where 1=1 ");
        if (drpclass.SelectedIndex != 0)
            stringBuilder.Append(" and cs.classid=" + drpclass.SelectedValue);
        if (ddlStream.SelectedIndex != 0)
            stringBuilder.Append(" and cs.stream=" + ddlStream.SelectedValue);
        stringBuilder.Append(" and cs.SessionYear='" + drpSession.SelectedValue.ToString().Trim() + "' and  Detained_Promoted='' order by fullname ");
        DataTable dataTableQry = DAL.GetDataTableQry(stringBuilder.ToString().Trim());
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
            txtadmnno.Text = "";
        }
    }

    private void fillcat()
    {
        try
        {
            DataTable dataTableQry = new clsDAL().GetDataTableQry("ACTS_GetOthrCategory");
            if (dataTableQry.Rows.Count <= 0)
                return;
            ddlCategory.DataSource = dataTableQry;
            ddlCategory.DataTextField = "catname";
            ddlCategory.DataValueField = "catid";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new ListItem("--Item List--", "0"));
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTableQry = new clsDAL().GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent where AdmnNo=" + txtadmnno.Text.Trim() + " and Detained_Promoted=''");
            if (dataTableQry.Rows.Count > 0)
            {
                drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
                drpclass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
                fillstudent();
                drpstudent.SelectedValue = txtadmnno.Text.Trim();
            }
            else
                ScriptManager.RegisterClientScriptBlock((Control)txtadmnno, txtadmnno.GetType(), "ShowMessage", "alert('Admission number does not exist !')", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void FillBookList()
    {
        gvItemsForSale.Visible = true;
        Hashtable hashtable = new Hashtable();
        if (drpclass.SelectedIndex > 0)
            hashtable.Add("@ForClassId", drpclass.SelectedValue);
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        hashtable.Add("@SaleDt", dtpSaleDate.GetDateValue().ToString("dd MMM yyyy"));
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = DAL.GetDataTable("ACTS_GetItemDetailsForSale", hashtable);
        dataTable2.Columns.Add("Qty", typeof(Decimal));
        dataTable2.Columns.Add("Amount", typeof(Decimal));
        dataTable2.Columns.Add("Discount");
        dataTable2.Columns.Add("TaxAmount", typeof(Decimal));
        gvItemsForSale.DataSource = dataTable2;
        gvItemsForSale.DataBind();
        int num = 0;
        foreach (GridViewRow row in gvItemsForSale.Rows)
        {
            ((TextBox)row.FindControl("txtQty")).Text = "1";
            dataTable2.Rows[num++]["Qty"] = ((TextBox)row.FindControl("txtQty")).Text;
        }
        hfGvRecCount.Value = gvItemsForSale.Rows.Count.ToString();
        ViewState["ItemCart"] = dataTable2;
        ViewState["NewItem"] = dataTable2;
        txttotalbill.Text = "0.00";
        txttotaldis.Text = "0.00";
        if (gvItemsForSale.Rows.Count <= 0)
            return;
        btnPrepBill.Enabled = true;
    }

    protected void optItemAll_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox control = (CheckBox)gvItemsForSale.HeaderRow.FindControl("optItemAll");
        foreach (Control row in gvItemsForSale.Rows)
            ((CheckBox)row.FindControl("optItem")).Checked = control.Checked;
    }

    protected void optItem_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        CheckBox control = (CheckBox)gvItemsForSale.HeaderRow.FindControl("optItemAll");
        if (!checkBox.Checked)
            control.Checked = false;
        prepareCheckedItemsList();
    }

    private void prepareCheckedItemsList()
    {
        List<string> stringList = new List<string>();
        foreach (GridViewRow row in gvItemsForSale.Rows)
        {
            if (((CheckBox)row.FindControl("optItem")).Checked)
            {
                string str = gvItemsForSale.DataKeys[row.DataItemIndex].Value.ToString();
                stringList.Add(str);
            }
        }
        ViewState["CheckedItems"] = stringList;
    }

    protected void PrepBill()
    {
        bool flag = false;
        foreach (Control row in gvItemsForSale.Rows)
        {
            if (((CheckBox)row.FindControl("optItem")).Checked)
            {
                flag = true;
                break;
            }
        }
        if (flag)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = (DataTable)ViewState["ItemCart"];
            if (gvItemsForSale.Rows.Count > 0)
            {
                foreach (GridViewRow row in gvItemsForSale.Rows)
                {
                    CheckBox control1 = (CheckBox)row.FindControl("optItem");
                    Label control2 = (Label)row.FindControl("lblSalePrice");
                    TextBox control3 = (TextBox)row.FindControl("txtQty");
                    Label control4 = (Label)row.FindControl("lblTaxRate");
                    Decimal result1 = new Decimal(0);
                    Decimal num1 = new Decimal(0);
                    Decimal result2 = new Decimal(0);
                    Decimal result3 = new Decimal(0);
                    Decimal num2 = new Decimal(0);
                    Decimal.TryParse(control2.Text.Trim(), out result1);
                    Decimal.TryParse(control3.Text.Trim(), out result2);
                    Decimal.TryParse(control4.Text.Trim(), out result3);
                    string str = gvItemsForSale.DataKeys[row.DataItemIndex].Value.ToString();
                    DataRow[] dataRowArray = dataTable2.Select("ItemCode=" + str);
                    if (control1.Checked)
                    {
                        Decimal num3 = result1 * result2;
                        dataRowArray[0]["Qty"] = result2;
                        dataRowArray[0]["Discount"] = new Decimal(0);
                        if (rbtnInvType.SelectedValue == "E")
                        {
                            Decimal num4 = num3 * result3 / new Decimal(100);
                            Decimal num5 = num3 + num4;
                            dataRowArray[0]["TaxAmount"] = num4;
                            dataRowArray[0]["Amount"] = string.Format("{0:f2}", num5);
                        }
                        else
                        {
                            Decimal num4 = result3 / (result3 + new Decimal(100)) * num3;
                            dataRowArray[0]["TaxAmount"] = num4;
                            dataRowArray[0]["Amount"] = string.Format("{0:f2}", num3);
                        }
                    }
                    else
                    {
                        dataRowArray[0]["Qty"] = DBNull.Value;
                        dataRowArray[0]["Discount"] = DBNull.Value;
                        dataRowArray[0]["TaxAmount"] = DBNull.Value;
                        dataRowArray[0]["Amount"] = DBNull.Value;
                    }
                    dataTable2.AcceptChanges();
                }
                DataView defaultView = dataTable2.DefaultView;
                defaultView.Sort = "TaxAmount DESC,ItemName ASC";
                gvItemsForSale.DataSource = defaultView;
                gvItemsForSale.DataBind();
                restoreSelectionOfItems();
                string str1 = prepareSelectedItemListFromGrid();
                DataRow[] dataRowArray1 = dataTable2.Select("ItemCode IN(" + str1 + ")");
                dataTable2.Clone();
                DataTable dataTable3 = ((IEnumerable<DataRow>)dataRowArray1).CopyToDataTable<DataRow>();
                Decimal num = (Decimal)dataTable3.Compute("SUM(Amount)", "");
                txttotalbill.Text = num.ToString();
                dataTable3.Columns.Add("MRPAmount", typeof(Decimal));
                foreach (DataRow row in (InternalDataCollectionBase)dataTable3.Rows)
                {
                    if (Convert.ToInt32(row["Qty"].ToString().Trim()) != 0)
                        row["MRPAmount"] = (Decimal.Parse(row["Qty"].ToString()) * Decimal.Parse(row["MRP"].ToString()));
                    else
                        row.Delete();
                }
                dataTable3.AcceptChanges();
                txttotaldis.Text = ((Decimal)dataTable3.Compute("SUM(MRPAmount)", "") - num).ToString();
                txttotalamt.Text = txttotalbill.Text;
                ViewState["SalesDetails"] = dataTable3;
                btnSubmit.Visible = btnSubmitPrint.Visible = btnSubmitReturn.Visible = true;
            }
        }
        updadd.Update();
    }

    private void CalculateBillAmt()
    {
        bool flag = true;
        int num1 = 0;
        foreach (GridViewRow row in gvItemsForSale.Rows)
        {
            CheckBox control1 = (CheckBox)row.FindControl("optItem");
            TextBox control2 = (TextBox)row.FindControl("txtQty");
            Label control3 = (Label)row.FindControl("lblAvlQty");
            if (Convert.ToInt32(control3.Text.Trim()) < 0)
                control2.Text = "0";
            else if (Convert.ToInt32(control2.Text.Trim()) > Convert.ToInt32(control3.Text.Trim()) && control1.Checked)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Selected Item Exceeds The Available Qty')", true);
                control2.Focus();
                flag = false;
            }
            if (control1.Checked)
                ++num1;
        }
        if (num1 > 0)
        {
            if (flag)
            {
                DataTable dataTable1 = new DataTable();
                DataTable dataTable2 = (DataTable)ViewState["ItemCart"];
                if (gvItemsForSale.Rows.Count > 0)
                {
                    foreach (GridViewRow row in gvItemsForSale.Rows)
                    {
                        CheckBox control1 = (CheckBox)row.FindControl("optItem");
                        Label control2 = (Label)row.FindControl("lblSalePrice");
                        TextBox control3 = (TextBox)row.FindControl("txtQty");
                        Label control4 = (Label)row.FindControl("lblTaxRate");
                        Decimal result1 = new Decimal(0);
                        Decimal num2 = new Decimal(0);
                        Decimal result2 = new Decimal(0);
                        Decimal result3 = new Decimal(0);
                        Decimal num3 = new Decimal(0);
                        Decimal.TryParse(control2.Text.Trim(), out result1);
                        Decimal.TryParse(control3.Text.Trim(), out result2);
                        Decimal.TryParse(control4.Text.Trim(), out result3);
                        string str = gvItemsForSale.DataKeys[row.DataItemIndex].Value.ToString();
                        DataRow[] dataRowArray = dataTable2.Select("ItemCode=" + str);
                        if (control1.Checked)
                        {
                            Decimal num4 = result1 * result2;
                            dataRowArray[0]["Qty"] = result2;
                            dataRowArray[0]["Discount"] = new Decimal(0);
                            if (rbtnInvType.SelectedValue == "E")
                            {
                                num3 = num4 * result3 / new Decimal(100);
                                Decimal num5 = num4 + num3;
                                dataRowArray[0]["TaxAmount"] = num3.ToString("0.000");
                                dataRowArray[0]["Amount"] = string.Format("{0:f2}", num5);
                            }
                            else
                            {
                                num3 = result3 / (result3 + new Decimal(100)) * num4;
                                dataRowArray[0]["TaxAmount"] = num3.ToString("0.000");
                                dataRowArray[0]["Amount"] = string.Format("{0:f2}", num4);
                            }
                        }
                        else
                        {
                            dataRowArray[0]["Qty"] = result2;
                            dataRowArray[0]["Discount"] = DBNull.Value;
                            dataRowArray[0]["TaxAmount"] = DBNull.Value;
                            dataRowArray[0]["Amount"] = DBNull.Value;
                        }
                        dataTable2.AcceptChanges();
                    }
                    string str1 = prepareSelectedItemListFromGrid();
                    DataView defaultView = dataTable2.DefaultView;
                    defaultView.Sort = "TaxAmount DESC,ItemName ASC";
                    gvItemsForSale.DataSource = defaultView;
                    gvItemsForSale.DataBind();
                    DataRow[] dataRowArray1 = dataTable2.Select("ItemCode IN(" + str1 + ")");
                    dataTable2.Clone();
                    DataTable dataTable3 = ((IEnumerable<DataRow>)dataRowArray1).CopyToDataTable<DataRow>();
                    Decimal num6 = (Decimal)dataTable3.Compute("SUM(Amount)", "");
                    Decimal num7 = (Decimal)dataTable3.Compute("SUM(TaxAmount)", "");
                    txttotalbill.Text = !(rbtnInvType.SelectedValue == "E") ? num6.ToString() : (num6 - num7).ToString();
                    txttotalvat.Text = num7.ToString();
                    dataTable3.Columns.Add("MRPAmount", typeof(Decimal));
                    foreach (DataRow row in (InternalDataCollectionBase)dataTable3.Rows)
                    {
                        if (Convert.ToInt32(row["Qty"].ToString().Trim()) != 0)
                            row["MRPAmount"] = (Decimal.Parse(row["Qty"].ToString()) * Decimal.Parse(row["SalePrice"].ToString()));
                        else
                            row.Delete();
                    }
                    dataTable3.AcceptChanges();
                    Decimal num8 = (Decimal)dataTable3.Compute("SUM(MRPAmount)", "");
                    txttotalamt.Text = !(rbtnInvType.SelectedValue == "E") ? txttotalbill.Text : Convert.ToString(num6);
                    ViewState["SalesDetails"] = dataTable3;
                    btnSubmit.Visible = btnSubmitPrint.Visible = btnSubmitReturn.Visible = true;
                }
            }
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Please Select an Item To Sell !')", true);
        updadd.Update();
    }

    protected void btnPrepBill_Click(object sender, EventArgs e)
    {
        CalculateBillAmt();
        txtRcvdAmt.Text = txttotalamt.Text;
        if (drpstudent.SelectedIndex == 0)
            txtRcvdAmt.ReadOnly = true;
        else
            txtRcvdAmt.ReadOnly = false;
    }

    private void restoreSelectionOfItems()
    {
        List<string> stringList = (List<string>)ViewState["CheckedItems"];
        if (stringList.Count <= 0)
            return;
        CheckBox control = (CheckBox)gvItemsForSale.HeaderRow.FindControl("optItemAll");
        foreach (GridViewRow row in gvItemsForSale.Rows)
        {
            string str = gvItemsForSale.DataKeys[row.DataItemIndex].Value.ToString();
            if (!stringList.Contains(str))
                ((CheckBox)row.FindControl("optItem")).Checked = control.Checked = false;
        }
    }

    private string prepareSelectedItemListFromGrid()
    {
        string empty = string.Empty;
        foreach (GridViewRow row in gvItemsForSale.Rows)
        {
            if (((CheckBox)row.FindControl("optItem")).Checked)
                empty += empty.Trim().Equals(string.Empty) ? gvItemsForSale.DataKeys[row.DataItemIndex].Value.ToString() : "," + gvItemsForSale.DataKeys[row.DataItemIndex].Value.ToString();
        }
        return empty;
    }

    protected void PaymentMode_CheckedChanged(object sender, EventArgs e)
    {
        if (optPmtCash.Checked)
        {
            txtBankName.Text = txtInstrDate.Text = txtInstrNo.Text = string.Empty;
            drpBank.SelectedIndex = -1;
            txtBankName.Enabled = txtInstrNo.Enabled = drpBank.Enabled = false;
            btnSubmit.Focus();
        }
        else
        {
            txtBankName.Enabled = txtInstrNo.Enabled = drpBank.Enabled = true;
            ((Control)dtpSaleDate).Focus();
        }
    }

    protected void drpSaleList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSaleList.SelectedIndex == 0)
        {
            drpSession.Enabled = true;
            drpclass.Enabled = true;
            drpstudent.Enabled = true;
            txtadmnno.Enabled = true;
            txtRcvdFrom.Text = "";
            drpSession.Focus();
            gvItemsForSale.DataSource = null;
            gvItemsForSale.DataBind();
            btnPrepBill.Enabled = false;
        }
        else if (drpSaleList.SelectedIndex == 1)
        {
            drpSession.Enabled = true;
            drpclass.Enabled = true;
            drpstudent.Enabled = false;
            drpstudent.SelectedIndex = -1;
            txtadmnno.Text = "";
            txtadmnno.Enabled = false;
            txtRcvdFrom.Text = "";
            drpSession.Focus();
            gvItemsForSale.DataSource = null;
            gvItemsForSale.DataBind();
            btnPrepBill.Enabled = false;
        }
        else
        {
            ddlItem.Items.Clear();
            drpSession.Enabled = false;
            drpclass.Enabled = false;
            drpstudent.Enabled = false;
            drpstudent.SelectedIndex = -1;
            txtadmnno.Text = "";
            txtadmnno.Enabled = false;
            txtRcvdFrom.Text = "";
            pnlAddItem.Visible = false;
            bindCategory(ddlCategory, "ACTS_GetOthrCategory", "CatName", "CatId");
            CreateDataTable();
            ddlCategory.Focus();
            gvItemsForSale.Visible = false;
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCategory.SelectedIndex > 0)
            {
                Hashtable hashtable = new Hashtable();
                DataTable dataTable1 = new DataTable();
                hashtable.Clear();
                hashtable.Add("@NoOfChar", 10);
                hashtable.Add("@Catid", int.Parse(ddlCategory.SelectedValue));
                DataTable dataTable2 = DAL.GetDataTable("ACTS_GetSaleItemList", hashtable);
                ddlItem.DataSource = dataTable2;
                ddlItem.DataTextField = "ItemName";
                ddlItem.DataValueField = "ItemCode";
                ddlItem.DataBind();
                if (dataTable2.Rows.Count > 0)
                    ddlItem.Items.Insert(0, new ListItem("--Item List--", "0"));
                else
                    ddlItem.Items.Insert(0, new ListItem("--No Items--", "0"));
                ddlItem.Focus();
            }
            else
            {
                ddlItem.Items.Clear();
                ddlItem.Items.Insert(0, new ListItem("--Select--", "0"));
            }
        }
        catch (Exception ex)
        {
            litFooterMsg.Text = "Transaction Failed ! Try Again";
            litHeaderMsg.Text = "Transaction Failed ! Try Again";
        }
    }

    private void CreateDataTable()
    {
        DataTable dataTable = new DataTable();
        DataColumn column1 = new DataColumn("ItemCode", Type.GetType("System.String"));
        dataTable.Columns.Add(column1);
        DataColumn column2 = new DataColumn("ItemName", Type.GetType("System.String"));
        dataTable.Columns.Add(column2);
        DataColumn column3 = new DataColumn("MesuringUnit", Type.GetType("System.String"));
        dataTable.Columns.Add(column3);
        DataColumn column4 = new DataColumn("TaxCode", Type.GetType("System.String"));
        dataTable.Columns.Add(column4);
        DataColumn column5 = new DataColumn("TaxRate", Type.GetType("System.String"));
        dataTable.Columns.Add(column5);
        DataColumn column6 = new DataColumn("SalePrice", Type.GetType("System.String"));
        dataTable.Columns.Add(column6);
        DataColumn column7 = new DataColumn("AvlQty", Type.GetType("System.String"));
        dataTable.Columns.Add(column7);
        DataColumn column8 = new DataColumn("Qty", Type.GetType("System.Decimal"));
        dataTable.Columns.Add(column8);
        DataColumn column9 = new DataColumn("Amount", Type.GetType("System.Decimal"));
        dataTable.Columns.Add(column9);
        DataColumn column10 = new DataColumn("Discount", Type.GetType("System.Decimal"));
        dataTable.Columns.Add(column10);
        DataColumn column11 = new DataColumn("TaxAmount", Type.GetType("System.Decimal"));
        dataTable.Columns.Add(column11);
        ViewState["NewItem"] = dataTable;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        bool flag = false;
        DataTable dataTable1 = (DataTable)ViewState["NewItem"];
        Hashtable hashtable = new Hashtable();
        new DataTable().Clear();
        int index = 0;
        foreach (DataRow row in (InternalDataCollectionBase)dataTable1.Rows)
        {
            if (row["ItemCode"].ToString().Trim() == ddlItem.SelectedValue.ToString().Trim())
            {
                flag = true;
                ((TextBox)gvItemsForSale.Rows[index].FindControl("txtQty")).Text = "1";
            }
            ++index;
        }
        if (!flag)
        {
            lblMsg2.Text = "";
            if (ddlItem.SelectedIndex > 0)
                hashtable.Add("@ItemCode", ddlItem.SelectedValue.ToString());
            hashtable.Add("@SaleDt", dtpSaleDate.GetDateValue().ToString("dd MMM yyyy"));
            hashtable.Add("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
            DataTable dataTable2 = DAL.GetDataTable("ACTS_GetSaleItemsIndv", hashtable);
            if (dataTable2.Rows.Count > 0)
            {
                dataTable2.Columns.Add("Qty", typeof(Decimal));
                dataTable2.Columns.Add("Amount", typeof(Decimal));
                dataTable2.Columns.Add("Discount");
                dataTable2.Columns.Add("TaxAmount", typeof(Decimal));
                dataTable2.Rows[0]["Qty"] = "1";
                DataRow row = dataTable2.Rows[0];
                dataTable1.ImportRow(row);
                dataTable1.AcceptChanges();
                ViewState["NewItem"] = dataTable1;
                ViewState["ItemCart"] = dataTable1;
                gvItemsForSale.DataSource = dataTable1;
                gvItemsForSale.DataBind();
                gvItemsForSale.Visible = true;
                hfGvRecCount.Value = gvItemsForSale.Rows.Count.ToString();
                txttotalbill.Text = "0.00";
                txttotaldis.Text = "0.00";
                if (gvItemsForSale.Rows.Count > 0)
                    btnPrepBill.Enabled = true;
                gvItemsForSale.Focus();
                lblMsg2.Text = "Success Fully Added!!";
                lblMsg2.ForeColor = Color.Green;
                ddlItem.SelectedIndex = 0;
            }
            else
            {
                lblMsg2.Text = "Please Add Stock For The Item To Sale!!";
                lblMsg2.ForeColor = Color.Red;
            }
        }
        else
        {
            lblMsg2.Text = "Selected Item Already Added!!";
            lblMsg2.ForeColor = Color.Red;
            ddlItem.Focus();
        }
    }

    protected void rbtnInvType_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculateBillAmt();
    }

    protected void gvItemsForSale_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow || !(((Label)e.Row.Cells[0].FindControl("lblAmt")).Text.Trim() != ""))
            return;
        ((CheckBox)e.Row.Cells[0].FindControl("optItem")).Checked = true;
    }

    protected void dtpSaleDate_SelectionChanged(object sender, EventArgs e)
    {
        DateTime dateValue = dtpSaleDate.GetDateValue();
        clearcontrol();
        dtpSaleDate.SetDateValue(dateValue);
        string str = new clsDAL().ExecuteScalarQry("select max(TransDt) from dbo.Acts_PaymentReceiptVoucher where upper(Payment_Receipt)='R'");
        if (!(str.Trim() != "") || !(Convert.ToDateTime(dtpSaleDate.GetDateValue().ToString("dd-MMM-yyyy")) < Convert.ToDateTime(str.Trim())))
            return;
        litFooterMsg.Text = litHeaderMsg.Text = "<div style='padding:5px;font-weight:bold;color:white;background-color:red;border:solid 2px black;'>Warning :You are going to make a back date transaction. The DCR will be modified for this date! So verify the DCR accordingly!</div>";
    }

    protected void btnSubmitReturn_Click(object sender, EventArgs e)
    {
        DAL = new clsDAL();
        if (DAL.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=8").ToString().Trim() == "")
        {
            ScriptManager.RegisterStartupScript((Page)this, GetType(), "alertMessage", "alert('No Account Head For Book Sale!! Please Set Account Head To Continue!!'); window.location='" + Request.ApplicationPath + "/Masters/SetAccountHead.aspx'", true);
        }
        else
        {
            DAL = new clsDAL();
            DataTable dataTableQry = DAL.GetDataTableQry("select top(1) FY,StartDate,EndDate from dbo.ACTS_FinancialYear where (IsFinalized is null or IsFinalized=0) order by FY_Id desc");
            if (dataTableQry.Rows.Count > 0)
            {
               // if (Convert.ToDateTime(dtpSaleDate.GetDateValue().ToString("dd-MMM-yyyy")) >= Convert.ToDateTime(dataTableQry.Rows[0]["StartDate"]) && Convert.ToDateTime(dtpSaleDate.GetDateValue().ToString("dd-MMM-yyyy")) <= Convert.ToDateTime(dataTableQry.Rows[0]["EndDate"]))
                //{
                    string str = SaveData();
                    if (str.Trim().Equals("Data saved successfully !"))
                    {
                        clearcontrol();
                        ScriptManager.RegisterClientScriptBlock((Control)btnSubmitReturn, btnSubmitReturn.GetType(), "Message", "alert('" + str + "');", true);
                        ScriptManager.RegisterStartupScript((Page)HttpContext.Current.Handler, typeof(Page), "Script1", "<script language='javascript' ID='script1'>window.open('ItemSaleReturn.aspx?InvNo=" + HttpUtility.UrlEncode(ViewState["InvNo"].ToString()) + "','new window', 'top=40, left=100, width=1100, height=600, dependant=no, location=0, alwaysRaised=no, menubar=no, resizeable=no, scrollbars=n, toolbar=no, status=no, center=yes')</script>", false);
                    }
                    else
                        litFooterMsg.Text = litHeaderMsg.Text = "<div style='padding:5px;font-weight:bold;color:white;background-color:red;border:solid 2px black;'>" + str + "</div>";
               // }
              //  else
                 //   litFooterMsg.Text = litHeaderMsg.Text = "<div style='padding:5px;font-weight:bold;color:white;background-color:red;border:solid 2px black;'>Transaction Date not in the current financial year!</div>";
            }
            else
                litFooterMsg.Text = litHeaderMsg.Text = "<div style='padding:5px;font-weight:bold;color:white;background-color:red;border:solid 2px black;'>No running Financial available !!</div>";
        }
    }
}