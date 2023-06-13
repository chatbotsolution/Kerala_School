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

public partial class Inventory_InvWritesOff : System.Web.UI.Page
{

    private clsDAL obj = new clsDAL();
  private Hashtable ht = new Hashtable();
  private DataTable dt = new DataTable();

   

         protected void Page_Load(object sender, EventArgs e)
  {
    if (Session["User"] != null)
    {
      if (Page.IsPostBack)
        return;
      Page.Form.DefaultButton = btnSave.UniqueID;
      BindCat();
      if (Session["User"].ToString() == "admin")
        bindDropDown(ddlLocation, "select LocationId,Location from dbo.SI_LocationMaster order by Location", "Location", "LocationId");
      else
        bindDropDown(ddlLocation, "select LocationId,Location from dbo.SI_LocationMaster where UserId=" +  int.Parse(Session["User_Id"].ToString()) + " and SchoolId=" +  int.Parse(Session["SchoolId"].ToString()), "Location", "LocationId");
    }
    else
      Response.Redirect("../Login.aspx");
  }

  private void BindCat()
  {
    DataTable dataTable = new clsDAL().GetDataTable("SI_ItemCatList");
    if (dataTable.Rows.Count <= 0)
      return;
    ddlCatName.Items.Clear();
    ddlCatName.Items.Add(new ListItem("--All--", "0"));
    foreach (DataRow row in (InternalDataCollectionBase) dataTable.Rows)
      ddlCatName.Items.Add(new ListItem(row["CatName"].ToString(), row["CatId"].ToString()));
    foreach (ListItem listItem in ddlCatName.Items)
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
    drp.DataSource =  null;
    drp.DataBind();
    DataTable dataTableQry = obj.GetDataTableQry(query);
    drp.DataSource =  dataTableQry;
    drp.DataTextField = textfield;
    drp.DataValueField = valuefield;
    drp.DataBind();
    drp.Items.Insert(0, new ListItem("--Select--", "0"));
  }

  protected void ddlCatName_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (ddlCatName.SelectedIndex > 0)
      bindDropDown(ddlItem, "select L.ItemCode,I.ItemName from dbo.SI_LocationwiseStock L inner join dbo.SI_ItemMaster I on I.ItemCode=L.ItemCode inner join dbo.SI_CategoryMaster C on C.CatId=I.CatId where C.CatId IN (SELECT CatId FROM SI_fuGetCatAncestors(" + ddlCatName.SelectedValue.ToString().Trim() + ")) and L.LocationId=" + ddlLocation.SelectedValue + " order by ItemName", "ItemName", "ItemCode");
    else
      ddlItem.Items.Clear();
    lblQty.Text = "";
  }

  protected void btnSave_Click(object sender, EventArgs e)
  {
    try
    {
      ht.Clear();
      ht.Add( "@LocationId",  ddlLocation.SelectedValue);
      ht.Add( "@ItemCode",  ddlItem.SelectedValue);
      ht.Add( "@Quantity",  txtQty.Text.Trim());
      ht.Add( "@WriteOffDate",  dtpWriteOffDt.SelectedDate);
      ht.Add( "@Description",  txtDescription.Text.Trim());
      ht.Add( "@UserId",  int.Parse(Session["User_Id"].ToString()));
      ht.Add( "@SchoolId",  int.Parse(Session["SchoolId"].ToString()));
      string str = obj.ExecuteScalar("SI_InsUpdtWritesOff", ht);
      if (str != "")
      {
        lblMsg.Text = str ?? "";
        lblMsg.ForeColor = Color.Red;
      }
      else
      {
        lblMsg.Text = " Data Saved Successfully";
        lblMsg.ForeColor = Color.Green;
        Clear();
      }
    }
    catch (Exception ex)
    {
    }
  }

  protected void btnCancel_Click(object sender, EventArgs e)
  {
    Clear();
  }

  private void Clear()
  {
    ddlCatName.SelectedIndex = 0;
    ddlItem.Items.Clear();
    txtQty.Text = "";
    txtWriteOffDt.Text = "";
    txtDescription.Text = "";
    lblQty.Text = string.Empty;
  }

  protected void btnShow_Click(object sender, EventArgs e)
  {
    Response.Redirect("InvWritesOffList.aspx");
  }

  protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (ddlItem.SelectedIndex > 0)
    {
      lblQty.Text = "(Max Qty Avail in Stock ";
      lblQty.Text = lblQty.Text + obj.ExecuteScalarQry("select AvlQty from dbo.SI_LocationwiseStock where ItemCode=" + ddlItem.SelectedValue + " and LocationId=" + ddlLocation.SelectedValue) + ")";
      lblQty.Visible = true;
    }
    else
    {
      txtQty.Text = "";
      lblQty.Visible = false;
    }
  }
   
}