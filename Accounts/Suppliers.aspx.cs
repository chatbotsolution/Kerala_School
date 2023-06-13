using AjaxControlToolkit;
using ASP;
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

public partial class Accounts_Suppliers : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            bindDropDown(ddlSuppliers, "Select PartyId,PartyName from dbo.ACTS_PartyMaster where PartyType='Supplier' and IsActive=1", "PartyName", "PartyId");
            bindCategory(ddlCategory, "Proc_DrpGetCat", "CatName", "CatId");
            CreateDataTable();
            if (Request.QueryString["SupplierId"] == null)
                return;
            ddlSuppliers.SelectedValue = Request.QueryString["SupplierId"].ToString();
            ddlSuppliers_SelectedIndexChanged(sender, e);
            ddlSuppliers.Enabled = false;
            ht.Clear();
            ht.Add("@SupplierPartyId", int.Parse(Request.QueryString["SupplierId"].ToString()));
            dt = obj.GetDataTable("ACTS_EditSupplier", ht);
            ViewState["Table"] = dt;
            Bind();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void CreateDataTable()
    {
        DataTable dataTable = new DataTable();
        DataColumn column1 = new DataColumn("Category", Type.GetType("System.String"));
        dataTable.Columns.Add(column1);
        DataColumn column2 = new DataColumn("CatCode", Type.GetType("System.String"));
        dataTable.Columns.Add(column2);
        DataColumn column3 = new DataColumn("ItemCode", Type.GetType("System.String"));
        dataTable.Columns.Add(column3);
        DataColumn column4 = new DataColumn("ItemName", Type.GetType("System.String"));
        dataTable.Columns.Add(column4);
        DataColumn column5 = new DataColumn("MaxSupply", Type.GetType("System.String"));
        dataTable.Columns.Add(column5);
        DataColumn column6 = new DataColumn("Unit", Type.GetType("System.String"));
        dataTable.Columns.Add(column6);
        DataColumn column7 = new DataColumn("SupplyDelay", Type.GetType("System.String"));
        dataTable.Columns.Add(column7);
        ViewState["Table"] = dataTable;
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void bindClass()
    {
        ddlClass.DataSource = obj.GetDataTableQry("select ClassID, ClassName from dbo.PS_ClassMaster order by ClassID");
        ddlClass.DataTextField = "ClassName";
        ddlClass.DataValueField = "ClassID";
        ddlClass.DataBind();
        ddlClass.Items.Insert(0, new ListItem("--All--", "0"));
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
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void ddlSuppliers_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSuppliers.SelectedIndex > 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            dt = obj.GetDataTableQry("select ContactPerson,Address,Mobile from ACTS_PartyMaster where PartyId=" + int.Parse(ddlSuppliers.SelectedValue));
            stringBuilder.Append("<div><strong> Contact Person : </strong>" + dt.Rows[0]["ContactPerson"].ToString() + "</div>");
            stringBuilder.Append("<div><strong> Address : </strong>" + dt.Rows[0]["Address"].ToString() + "</div>");
            stringBuilder.Append("<div><strong> Mobile : </strong>" + dt.Rows[0]["Mobile"].ToString() + "</div>");
            lblSupplierDtls.Text = stringBuilder.ToString();
            pnlSupplierDtls.Visible = true;
        }
        else
        {
            lblSupplierDtls.Text = "";
            pnlSupplierDtls.Visible = false;
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCategory.SelectedIndex > 0)
            {
                if (ddlCategory.SelectedValue == "1" || obj.ExecuteScalarQry("Select ParentCatId from dbo.SI_CategoryMaster where CatId=" + ddlCategory.SelectedValue.ToString()).Trim() == "1")
                {
                    pnlclass.Visible = true;
                    bindClass();
                }
                else
                {
                    ddlClass.SelectedIndex = 0;
                    pnlclass.Visible = false;
                }
                ht.Clear();
                ht.Add("@NoOfChar", 10);
                ht.Add("@Catid", int.Parse(ddlCategory.SelectedValue));
                dt = obj.GetDataTable("ACTS_GetItemList", ht);
                ddlItem.DataSource = dt;
                ddlItem.DataTextField = "ItemNameFull";
                ddlItem.DataValueField = "ItemCode";
                ddlItem.DataBind();
                if (dt.Rows.Count > 0)
                    ddlItem.Items.Insert(0, new ListItem("--Item List--", "0"));
                else
                    ddlItem.Items.Insert(0, new ListItem("--No Items--", "0"));
            }
            else
            {
                ddlClass.SelectedIndex = 0;
                pnlclass.Visible = false;
                ddlItem.Items.Clear();
                ddlItem.Items.Insert(0, new ListItem("--Select--", "0"));
                lblUnit.Text = "";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            ht.Add("@NoOfChar", 10);
            ht.Add("@Catid", int.Parse(ddlCategory.SelectedValue));
            ht.Add("@ClassId", int.Parse(ddlClass.SelectedValue));
            dt = obj.GetDataTable("ACTS_GetItemListForClass", ht);
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemNameFull";
            ddlItem.DataValueField = "ItemCode";
            ddlItem.DataBind();
            if (dt.Rows.Count > 0)
                ddlItem.Items.Insert(0, new ListItem("--Item List--", "0"));
            else
                ddlItem.Items.Insert(0, new ListItem("--No Items--", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlItem.SelectedIndex > 0)
            lblUnit.Text = "(" + obj.ExecuteScalarQry("select MesuringUnit from SI_ItemMaster where ItemCode=" + int.Parse(ddlItem.SelectedValue)) + ")";
        else
            lblUnit.Text = "";
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTable = (DataTable)ViewState["Table"];
            if (btnAdd.Text == "Add")
            {
                string str = "false";
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                {
                    if (row["ItemCode"].ToString() == ddlItem.SelectedValue.ToString())
                    {
                        row["MaxSupply"] = txtMaxSupply.Text;
                        row["SupplyDelay"] = txtDelayInDays.Text;
                        dataTable.AcceptChanges();
                        str = "true";
                        break;
                    }
                }
                if (str == "false")
                {
                    DataRow row = dataTable.NewRow();
                    row["CatCode"] = ddlCategory.SelectedValue;
                    row["Category"] = ddlCategory.SelectedItem;
                    row["ItemCode"] = ddlItem.SelectedValue;
                    row["ItemName"] = ddlItem.SelectedItem;
                    row["MaxSupply"] = txtMaxSupply.Text.Trim();
                    row["Unit"] = lblUnit.Text.Trim();
                    row["SupplyDelay"] = txtDelayInDays.Text.Trim();
                    dataTable.Rows.Add(row);
                }
            }
            else
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                {
                    if (row["ItemCode"].ToString() == ViewState["ItemCode"].ToString())
                    {
                        row["MaxSupply"] = txtMaxSupply.Text;
                        row["SupplyDelay"] = txtDelayInDays.Text;
                        dataTable.AcceptChanges();
                        btnAdd.Text = "Add";
                    }
                }
            }
            ViewState["Table"] = dataTable;
            Clear();
            Bind();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void Clear()
    {
        ddlCategory.Enabled = true;
        ddlItem.Enabled = true;
        ddlCategory.SelectedIndex = 0;
        ddlItem.SelectedIndex = 0;
        txtMaxSupply.Text = "";
        txtDelayInDays.Text = "";
        lblUnit.Text = "";
        btnAdd.Text = "Add";
    }

    private void Bind()
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = (DataTable)ViewState["Table"];
        if (dataTable2.Rows.Count > 0)
            pnl.Visible = true;
        else
            pnl.Visible = false;
        gvItemSupply.DataSource = dataTable2;
        gvItemSupply.DataBind();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTable = new DataTable();
            Hashtable hashtable = new Hashtable();
            if (((DataTable)ViewState["Table"]).Rows.Count > 0)
            {
                hashtable.Clear();
                obj.ExcuteQryInsUpdt("delete from dbo.ACTS_SupplierItems where SupplierPartyId=" + int.Parse(ddlSuppliers.SelectedValue));
                StoreSupplierDetail();
                lblMsg.Text = "Supplier Items Added Successfully";
                lblMsg.ForeColor = Color.Green;
                CreateDataTable();
                Bind();
                ddlSuppliers.SelectedIndex = 0;
                ddlSuppliers.Enabled = true;
                pnlSupplierDtls.Visible = false;
                lblSupplierDtls.Text = "";
            }
            else
            {
                lblMsg.Text = "No Items Under the Supplier";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void StoreSupplierDetail()
    {
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        foreach (DataRow row in (InternalDataCollectionBase)((DataTable)ViewState["Table"]).Rows)
        {
            hashtable.Clear();
            hashtable.Add("@SupplierPartyId", int.Parse(ddlSuppliers.SelectedValue));
            hashtable.Add("@ItemCode", int.Parse(row["ItemCode"].ToString()));
            hashtable.Add("@MaxSupplyCapacity", float.Parse(row["MaxSupply"].ToString()));
            hashtable.Add("@SupplyDelay", int.Parse(row["SupplyDelay"].ToString()));
            obj.ExcuteProcInsUpdt("ACTS_InsItemAgainstSupplier", hashtable);
        }
    }

    protected void gvItemSupply_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            Control commandSource = e.CommandSource as Control;
            ImageButton imageButton = new ImageButton();
            if (commandSource != null)
                imageButton = (ImageButton)(commandSource.Parent.NamingContainer as GridViewRow).FindControl("imgbtnDelete");
            DataTable dataTable = (DataTable)ViewState["Table"];
            DataRow[] dataRowArray = dataTable.Select("ItemCode=" + e.CommandArgument.ToString());
            if (e.CommandName == "Modify")
            {
                ddlCategory.SelectedValue = dataRowArray[0]["CatCode"].ToString();
                ddlCategory_SelectedIndexChanged(sender, (EventArgs)e);
                ddlItem.SelectedValue = dataRowArray[0]["ItemCode"].ToString();
                txtMaxSupply.Text = dataRowArray[0]["MaxSupply"].ToString();
                txtDelayInDays.Text = dataRowArray[0]["SupplyDelay"].ToString();
                lblUnit.Text = dataRowArray[0]["Unit"].ToString();
                ViewState["ItemCode"] = dataRowArray[0]["ItemCode"].ToString();
                btnAdd.Text = "Update";
                ddlCategory.Enabled = false;
                ddlItem.Enabled = false;
                imageButton.Enabled = false;
            }
            else
            {
                if (!(e.CommandName == "Remove"))
                    return;
                foreach (DataRow row in dataRowArray)
                    dataTable.Rows.Remove(row);
                dataTable.AcceptChanges();
                ViewState["Table"] = dataTable;
                Bind();
                Clear();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again .";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        Response.Redirect("SuppliersList.aspx");
    }

    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        CreateDataTable();
        Bind();
        ddlSuppliers.Enabled = true;
        ddlSuppliers.SelectedIndex = 0;
        ddlSuppliers_SelectedIndexChanged(sender, e);
    }
}