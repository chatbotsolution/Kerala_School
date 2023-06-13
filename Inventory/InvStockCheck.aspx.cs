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

public partial class Inventory_InvStockCheck : System.Web.UI.Page
{

    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
           Response.Redirect("~/Login.aspx");
       lblMessage.Text = "";
        if (Page.IsPostBack)
            return;
       Pnl.Visible = false;
       lblMessage.Text = "";
       Session["dtgrv"] = null;
        if (Request.QueryString["Id"] != null)
        {
           Pnl.Visible = true;
           Filldata();
           FillGrid();
           item();
           BindCat();
        }
        if (Session["User"].ToString() == "admin")
           bindDropDown(drpLoc, "select LocationId, Location from dbo.SI_LocationMaster", "Location", "LocationId");
        else
           bindDropDown(drpLoc, "select LocationId, Location from dbo.SI_LocationMaster WHERE SchoolId=" + Convert.ToInt32(Session["SchoolId"]), "Location", "LocationId");
       BindCat();
       btnChkList.Enabled = false;
    }

    private void item()
    {
       bindDropDown(ddlItem, "select ItemCode, ItemName from dbo.SI_ItemMaster", "ItemName", "ItemCode");
    }

    private void Filldata()
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry =obj.GetDataTableQry("SELECT s.StockCheckId, s.CheckDate, s.LocationId, s.CheckedBy,L.Location FROM dbo.SI_StockCheckDoc s INNER JOIN SI_LocationMaster L on s.LocationId=L.LocationId WHERE StockCheckId=" +Request.QueryString["id"].ToString());
       txtChkDate.Text = Convert.ToDateTime(dataTableQry.Rows[0]["CheckDate"].ToString()).ToString("dd-MMM-yyyy");
       drpLoc.SelectedValue = dataTableQry.Rows[0]["LocationId"].ToString();
       txtChkBy.Text = dataTableQry.Rows[0]["CheckedBy"].ToString();
    }

    private void FillGrid()
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry =obj.GetDataTableQry("SELECT s.StockCheckDetailId, s.StockCheckId, s.ItemCode, s.AvlQty,I.ItemName FROM dbo.SI_StockCheckDetail s Inner Join SI_ItemMaster I on s.ItemCode=I.ItemCode  WHERE StockCheckId=" +Request.QueryString["id"].ToString());
       Session["dtgrv"] = dataTableQry;
       ViewState["StockId"] = dataTableQry.Rows[0]["StockCheckId"].ToString();
       grvItemPurchase.DataSource = dataTableQry;
       grvItemPurchase.DataBind();
    }

    private void BindCat()
    {
        DataTable dataTable = new clsDAL().GetDataTable("SI_ItemCatList");
        if (dataTable.Rows.Count <= 0)
            return;
       drpCat.Items.Clear();
       drpCat.Items.Add(new ListItem("--Select--", "0"));
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
           drpCat.Items.Add(new ListItem(row["CatName"].ToString(), row["CatId"].ToString()));
        foreach (ListItem listItem in drpCat.Items)
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
        DataTable dataTableQry =obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void Clear()
    {
       drpCat.SelectedIndex = 0;
       ddlItem.SelectedIndex = 0;
       txtQty.Text = "";
    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
       Response.Redirect("StockCheckList.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       ClearDetails();
       lblMessage.Text = "";
    }

    private void ClearDetails()
    {
       txtChkBy.Text = "";
       txtChkDate.Text = "";
       txtQty.Text = "";
       BindCat();
       item();
    }

    protected void grvItemPurchase_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       grvItemPurchase.PageIndex = e.NewPageIndex;
       Bindgrid();
    }

    protected void btnChkList_Click(object sender, EventArgs e)
    {
       Bindgrid();
       lblMessage.Text = "";
       lblMessage.Visible = false;
    }

    private void Bindgrid()
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 =obj.GetDataTable("SI_GetAllStockItems", new Hashtable()
    {
      {
         "StockCheckId",
        ViewState["StockId"].ToString()
      }
    });
       Session["dtgrv"] = dataTable2;
       grvItemPurchase.DataSource = dataTable2;
       grvItemPurchase.DataBind();
        if (dataTable2.Rows.Count > 0)
        {
           Pnl.Visible = true;
           grvItemPurchase.Visible = true;
        }
        else
        {
           Pnl.Visible = false;
           grvItemPurchase.Visible = false;
        }
    }

    protected void btnSave_Click1(object sender, EventArgs e)
    {
       Insertdata();
       btnChkList.Enabled = true;
    }

    private void Insertdata()
    {
        DataTable dataTable1 = new DataTable();
        Hashtable ht = new Hashtable();
        if (ViewState["StockId"] == null)
        {
            ht.Add("CheckDate", dtpChkDate.GetDateValue());
            if (drpLoc.SelectedIndex > 0)
                ht.Add("LocationId", drpLoc.SelectedValue);
            if (txtChkBy.Text.Trim() != "")
                ht.Add("CheckedBy", txtChkBy.Text.Trim());
            ht.Add("@UserID", Convert.ToInt32(Session["User_Id"]));
            int int32 = Convert.ToInt32(obj.GetDataTable("SI_InsUpdtStockCheckDoc", ht).Rows[0][0].ToString());
           ViewState["StockId"] = int32;
            ht.Clear();
            DataTable dataTable2 = new DataTable();
            ht.Add("StockCheckId", int32);
            if (ddlItem.SelectedIndex > 0)
                ht.Add("ItemCode", ddlItem.SelectedValue);
            if (txtQty.Text.Trim() != "")
                ht.Add("AvlQty", txtQty.Text.Trim());
           obj.GetDataTable("SI_InsUpdtStockChkDtl", ht);
           Session["dtgrv"] = null;
           lblMessage.Text = "Data Save Successfully";
           lblMessage.ForeColor = Color.Green;
           Clearpage();
        }
        else
        {
            ht.Clear();
            DataTable dataTable2 = new DataTable();
            if (ViewState["Sid"] != null)
                ht.Add("StockCheckDetailId", ViewState["Sid"].ToString());
            ht.Add("StockCheckId", ViewState["StockId"].ToString());
            if (ddlItem.SelectedIndex > 0)
                ht.Add("ItemCode", ddlItem.SelectedValue);
            if (txtQty.Text.Trim() != "")
                ht.Add("AvlQty", txtQty.Text.Trim());
           obj.GetDataTable("SI_InsUpdtStockChkDtl", ht);
           ViewState["Sid"] = null;
           Session["dtgrv"] = null;
           lblMessage.Text = "Data Save Successfully";
           lblMessage.ForeColor = Color.Green;
        }
       Clear();
       Pnl.Visible = false;
    }

    private void Clearpage()
    {
       ddlItem.SelectedIndex = 0;
       drpCat.SelectedIndex = 0;
       txtQty.Text = "";
    }

    protected void drpCat_SelectedIndexChanged(object sender, EventArgs e)
    {
       binditem();
    }

    private void binditem()
    {
        if (drpCat.SelectedIndex > 0)
           bindDropDown(ddlItem, "select ItemCode, ItemName from dbo.SI_ItemMaster where CatId IN (SELECT CatId FROM SI_fuGetCatAncestors(" +drpCat.SelectedValue.ToString().Trim() + "))", "ItemName", "ItemCode");
        else
           bindDropDown(ddlItem, "select ItemCode, ItemName from dbo.SI_ItemMaster ", "ItemName", "ItemCode");
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry =obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    protected void grvItemPurchase_RowEditing(object sender, GridViewEditEventArgs e)
    {
       BindCat();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry =obj.GetDataTableQry("SELECT SD.StockCheckDetailId,I.ItemName,C.CatId ,SD.StockCheckId, SD.ItemCode, SD.AvlQty FROM SI_StockCheckDetail SD INNER JOIN dbo.SI_ItemMaster I ON SD.ItemCode=I.ItemCode  INNER JOIN dbo.SI_CategoryMaster C ON I.CatId=C.CatId WHERE SD.StockCheckDetailId=" + ((HiddenField)grvItemPurchase.Rows[e.NewEditIndex].FindControl("hfid")).Value.ToString().Trim());
       drpCat.SelectedValue = dataTableQry.Rows[0]["CatId"].ToString();
       binditem();
       ddlItem.SelectedValue = dataTableQry.Rows[0]["ItemCode"].ToString();
       txtQty.Text = dataTableQry.Rows[0]["AvlQty"].ToString();
       ViewState["Sid"] = dataTableQry.Rows[0]["StockCheckDetailId"].ToString();
    }

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Session["dtgrv"] == null)
            return;
       dt =obj.GetDataTableQry("select StockCheckDetailId,StockCheckId,AvlQty from dbo.SI_StockCheckDetail where ItemCode=" +ddlItem.SelectedValue.ToString() + " and StockCheckId=" +ViewState["StockId"].ToString());
        if (dt.Rows.Count > 0)
        {
           ViewState["Sid"] = dt.Rows[0]["StockCheckDetailId"].ToString();
           txtQty.Text =dt.Rows[0]["AvlQty"].ToString();
           lblMessage.Text = "";
        }
        else
           txtQty.Text = string.Empty;
    }

    protected void drpLoc_SelectedIndexChanged(object sender, EventArgs e)
    {
       grdclear();
       txtChkBy.Text = string.Empty;
       txtQty.Text = string.Empty;
       drpCat.SelectedIndex = 0;
       btnChkList.Enabled = false;
       Pnl.Visible = false;
       lblMessage.Text = "";
       Session["dtgrv"] = null;
       ViewState["Sid"] = null;
       ViewState["StockId"] = null;
    }

    private void grdclear()
    {
       grvItemPurchase.DataSource = null;
       grvItemPurchase.DataBind();
    }

    protected void btnGoList_Click(object sender, EventArgs e)
    {
       Response.Redirect("InvStockCheckList.aspx");
    }
}