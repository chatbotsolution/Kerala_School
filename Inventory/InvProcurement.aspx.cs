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

public partial class Inventory_InvProcurement : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            lblmsg.Text = string.Empty;
            if (Page.IsPostBack)
                return;
            string str = Request.QueryString[""];
            BindCat();
            CreateDataTable();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void BindCat()
    {
        DataTable dataTable = new clsDAL().GetDataTable("SI_ItemCatList");
        if (dataTable.Rows.Count <= 0)
            return;
        ddlItemCat.Items.Clear();
        ddlItemCat.Items.Add(new ListItem("--Select--", "0"));
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            ddlItemCat.Items.Add(new ListItem(row["CatName"].ToString(), row["CatId"].ToString()));
        foreach (ListItem listItem in ddlItemCat.Items)
        {
            if (!listItem.Text.StartsWith("-"))
            {
                listItem.Attributes.Add("Style", "font-weight:bold");
                listItem.Attributes.Add("Style", "text-transform:uppercase");
                listItem.Attributes.Add("Style", "color:maroon");
            }
        }
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource =null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource =dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void CreateDataTable()
    {
        DataTable dataTable = new DataTable();
        DataColumn column1 = new DataColumn("ItemCat", Type.GetType("System.String"));
        dataTable.Columns.Add(column1);
        DataColumn column2 = new DataColumn("ItemCode", Type.GetType("System.String"));
        dataTable.Columns.Add(column2);
        DataColumn column3 = new DataColumn("ItemName", Type.GetType("System.String"));
        dataTable.Columns.Add(column3);
        DataColumn column4 = new DataColumn("Quantity", Type.GetType("System.String"));
        dataTable.Columns.Add(column4);
        DataColumn column5 = new DataColumn("UnitPrice", Type.GetType("System.String"));
        dataTable.Columns.Add(column5);
        DataColumn column6 = new DataColumn("Warranty", Type.GetType("System.String"));
        dataTable.Columns.Add(column6);
        DataColumn column7 = new DataColumn("MfdDt", Type.GetType("System.String"));
        dataTable.Columns.Add(column7);
        DataColumn column8 = new DataColumn("ExpDt", Type.GetType("System.String"));
        dataTable.Columns.Add(column8);
        ViewState["Table"] =dataTable;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTable = (DataTable)ViewState["Table"];
            if (btnAdd.Text == "Add")
            {
                DataRow row = dataTable.NewRow();
                row["ItemCat"] =ddlItemCat.SelectedValue;
                row["ItemCode"] =ddlItem.SelectedValue;
                row["ItemName"] =ddlItem.SelectedItem;
                row["Quantity"] =txtQty.Text;
                row["UnitPrice"] = !(txtUnitPrice.Text.Trim() != "") ?"0" :txtUnitPrice.Text;
                row["Warranty"] = PCWarranty.SelectedDate;
                row["MfdDt"] = !(txtMfdDt.Text != "") ? "" :PopCalendar3.GetDateValue().ToString();
                row["ExpDt"] = !(txtExpDt.Text != "") ? "" :PopCalendar4.GetDateValue().ToString();
                dataTable.Rows.Add(row);
            }
            else
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                {
                    if (row["ItemCode"].ToString() == ViewState["ItemCode"].ToString())
                    {
                        row["ItemCat"] =ddlItemCat.SelectedValue;
                        row["ItemCode"] =ddlItem.SelectedValue;
                        row["ItemName"] =ddlItem.SelectedItem;
                        row["Quantity"] =txtQty.Text;
                        row["UnitPrice"] =txtUnitPrice.Text;
                        row["Warranty"] = PCWarranty.SelectedDate;
                        row["MfdDt"] = !(txtMfdDt.Text != "") ?"" :PopCalendar3.GetDateValue().ToString();
                        row["ExpDt"] = !(txtExpDt.Text != "") ?"" :PopCalendar4.GetDateValue().ToString();
                        dataTable.AcceptChanges();
                        btnAdd.Text = "Add";
                    }
                }
            }
            ViewState["Table"] =dataTable;
            Clear();
            Bind();
        }
        catch (Exception ex)
        {
        }
    }

    private void Clear()
    {
        ddlItemCat.SelectedIndex = 0;
        ddlItem.SelectedIndex = 0;
        txtQty.Text = "";
        txtUnitPrice.Text = "";
        txtWarranty.Text = "";
        txtMfdDt.Text = "";
        txtExpDt.Text = "";
        btnAdd.Text = "Add";
    }

    private void Bind()
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = (DataTable)ViewState["Table"];
        if (dataTable2.Rows.Count > 0)
            btnSave.Visible = true;
        else
            btnSave.Visible = false;
        grvItemPurchase.DataSource =dataTable2;
        grvItemPurchase.DataBind();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTable1 = new DataTable();
            Hashtable ht = new Hashtable();
            DataTable dataTable2 = (DataTable)ViewState["Table"];
            double num = !(txtAddnlCharge.Text.Trim() != "") ? double.Parse(txtTotAmt.Text.Trim()) : double.Parse(txtTotAmt.Text.Trim()) + double.Parse(txtAddnlCharge.Text.Trim());
            if (dataTable2.Rows.Count > 0)
            {
                ht.Clear();
                ht.Add("@PurDate",PopCalendar2.GetDateValue());
                ht.Add("@InvNo",txtInvoiceNo.Text.Trim());
                ht.Add("@SupplierName",txtSupplierName.Text.Trim());
                ht.Add("@SupplierAddress",txtSupplierAddress.Text.Trim());
                ht.Add("@SupplierContactNo",txtSupplierNo.Text.Trim());
                ht.Add("@TotBillAmt",txtTotAmt.Text.Trim());
                ht.Add("@AddnlChargesAmt",txtAddnlCharge.Text.Trim());
                ht.Add("@TotAmt",num);
                ht.Add("@Remarks",txtRemarks.Text.Trim());
                ht.Add("@SourseOfAquiring",ddlSource.SelectedValue);
                ht.Add("@UserId",int.Parse(Session["User_Id"].ToString()));
                ht.Add("@SchoolId",int.Parse(Session["SchoolId"].ToString()));
                DataTable dataTable3 = obj.GetDataTable("SI_InsInvoice", ht);
                if (dataTable3.Rows.Count > 0)
                {
                    if (dataTable3.Rows[0][0].ToString() == "Invoice Already exist")
                    {
                        lblmsg.Text = dataTable3.Rows[0][0].ToString();
                        lblmsg.ForeColor = Color.Red;
                    }
                    else
                    {
                        StoreItemDetail(int.Parse(dataTable3.Rows[0][0].ToString()));
                        lblmsg.Text = "Invoice Saved Successfully";
                        lblmsg.ForeColor = Color.Green;
                        CreateDataTable();
                        Bind();
                        ClearInvoice();
                    }
                }
            }
            else
                lblmsg.Text = "No Items Under the Invoice";
            UpdatePanel1.Update();
        }
        catch (Exception ex)
        {
        }
    }

    private void StoreItemDetail(int purchaseid)
    {
        DataTable dataTable = new DataTable();
        Hashtable ht = new Hashtable();
        foreach (DataRow row in (InternalDataCollectionBase)((DataTable)ViewState["Table"]).Rows)
        {
            ht.Clear();
            ht.Add("@PurchaseId",purchaseid);
            ht.Add("@ItemCode",int.Parse(row["ItemCode"].ToString()));
            if (row["MfdDt"].ToString() != "")
            ht.Add("@MfdDate",DateTime.Parse(row["MfdDt"].ToString()));
            if (row["ExpDt"].ToString() != "")
            ht.Add("@ExpDate",DateTime.Parse(row["ExpDt"].ToString()));
            if (row["Warranty"].ToString() != "")
            ht.Add("@WarrantyPeriod",row["Warranty"].ToString());
            ht.Add("@Qty",float.Parse(row["Quantity"].ToString()));
            ht.Add("@UnitPrice",double.Parse(row["UnitPrice"].ToString()));
            ht.Add("@UserId",int.Parse(Session["User_Id"].ToString()));
            ht.Add("@SchoolId",int.Parse(Session["SchoolId"].ToString()));
            obj.ExcuteProcInsUpdt("SI_InsInvoiceDtls", ht);
        }
    }

    private void ClearInvoice()
    {
        txtDate.Text = txtInvoiceNo.Text = txtTotAmt.Text = txtAddnlCharge.Text = txtSupplierName.Text = txtSupplierAddress.Text = txtSupplierNo.Text = txtRemarks.Text = "";
        ddlSource.SelectedIndex = 0;
    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        Response.Redirect("InvProcurementList.aspx");
    }

    protected void grvItemPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataTable dataTable = (DataTable)ViewState["Table"];
            DataRow[] dataRowArray = dataTable.Select("ItemCode=" + e.CommandArgument.ToString());
            if (e.CommandName == "Modify")
            {
                
                ddlItemCat.SelectedValue = dataRowArray[0]["ItemCat"].ToString();
                bindDropDown(ddlItem, "select ItemCode, ItemName from dbo.SI_ItemMaster where CatId IN (SELECT CatId FROM SI_fuGetCatAncestors(" + ddlItemCat.SelectedValue.ToString().Trim() + "))", "ItemName", "ItemCode");
                ddlItem.SelectedValue = dataRowArray[0]["ItemCode"].ToString();
                txtUnitPrice.Text = dataRowArray[0]["UnitPrice"].ToString();
                txtQty.Text = dataRowArray[0]["Quantity"].ToString();
                PCWarranty.SelectedDate = dataRowArray[0]["Warranty"].ToString();
                if (dataRowArray[0]["MfdDt"].ToString().Trim() != "")
                    PopCalendar3.SetDateValue(DateTime.Parse(dataRowArray[0]["MfdDt"].ToString()).Date);
                if (dataRowArray[0]["ExpDt"].ToString().Trim() != "")
                    PopCalendar4.SetDateValue(DateTime.Parse(dataRowArray[0]["ExpDt"].ToString()).Date);
                ViewState["ItemCode"] =dataRowArray[0]["ItemCode"].ToString();
                btnAdd.Text = "Update";
            }
            else
            {
                if (!(e.CommandName == "Remove"))
                    return;
                foreach (DataRow row in dataTable.Select("ItemCode=" + e.CommandArgument.ToString()))
                    dataTable.Rows.Remove(row);
                dataTable.AcceptChanges();
                ViewState["Table"] =dataTable;
                Bind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlItemCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindDropDown(ddlItem, "select ItemCode, ItemName from dbo.SI_ItemMaster where CatId IN (SELECT CatId FROM SI_fuGetCatAncestors(" + ddlItemCat.SelectedValue.ToString().Trim() + "))", "ItemName", "ItemCode");
    }

    protected void btnCancel2_Click(object sender, EventArgs e)
    {
        txtDate.Text = "";
        txtInvoiceNo.Text = "";
        txtTotAmt.Text = "";
        txtAddnlCharge.Text = "";
        txtSupplierName.Text = "";
        txtSupplierAddress.Text = "";
        txtSupplierNo.Text = "";
        ddlSource.SelectedIndex = 0;
        txtRemarks.Text = "";
    }
}