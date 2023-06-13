using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_ItemMasterList : System.Web.UI.Page
{
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private clsDAL obj = new clsDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
       Page.Form.DefaultButton =btnSearch.UniqueID;
       lblMsg.Text = string.Empty;
        if (this.Page.IsPostBack)
            return;
       FillDroupDown();
    }

    private void fillclass()
    {
       drpclass.DataSource = (object)this.obj.GetDataTableQry("select * from ps_classmaster");
       drpclass.DataTextField = "classname";
       drpclass.DataValueField = "classid";
       drpclass.DataBind();
       drpclass.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void FillDroupDown()
    {
        DataTable dataTable = new clsDAL().GetDataTable("Proc_DrpGetCat");
        if (dataTable.Rows.Count > 0)
        {
           drpCat.Items.Clear();
           drpCat.Items.Add(new ListItem("--All--", "0"));
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
               drpCat.Items.Add(new ListItem(row["CatName"].ToString(), row["CatId"].ToString()));
        }
        DataTable dataTableQry = new clsDAL().GetDataTableQry("SELECT BrandName,BrandId FROM SI_BrandMaster order by BrandName");
        if (dataTableQry.Rows.Count <= 0)
            return;
       drpBrand.Items.Clear();
       drpBrand.Items.Add(new ListItem("--All--", ""));
        foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
           drpBrand.Items.Add(new ListItem(row["BrandName"].ToString(), row["BrandId"].ToString()));
       drpBrand.SelectedValue = "1";
    }

    protected void grdItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       grdItem.PageIndex = e.NewPageIndex;
       Fillgrid();
    }

    protected void drpCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpCat.SelectedValue == "1" ||obj.ExecuteScalarQry("Select ParentCatId from dbo.SI_CategoryMaster where CatId=" +drpCat.SelectedValue.ToString()) == "1")
        {
           fillclass();
           drpclass.Enabled = true;
        }
        else
        {
            if (this.drpclass.SelectedIndex > 0)
               drpclass.SelectedIndex = 0;
           drpclass.Enabled = false;
        }
    }

    protected void grdItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            clsDAL clsDal = new clsDAL();
           dt = new DataTable();
           ht = new Hashtable();
           ht.Add((object)"@ItemCode", (object)((Label)this.grdItem.Rows[e.RowIndex].FindControl("lbl2")).Text);
           dt = clsDal.GetDataTable("ACTS_DeleteItem",ht);
            if (this.dt.Rows.Count > 0)
            {
               lblMsg.Text = "Dependency for this record exist ";
               lblMsg.ForeColor = Color.Red;
            }
            else
            {
               Fillgrid();
               lblMsg.Text = "Record Deleted Successfully ";
               lblMsg.ForeColor = Color.Green;
            }
        }
        catch (Exception ex)
        {
           lblMsg.Text = "Dependency for this record exist ";
           lblMsg.ForeColor = Color.Red;
        }
    }

    private string IsAvailTransaction(int itemcode)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable ht = new Hashtable();
        ht.Clear();
        ht.Add((object)"@ItemCode", (object)itemcode);
        return clsDal.ExecuteScalar("SI_CheckTranStatus", ht);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (this.drpCat.SelectedValue == "1" ||obj.ExecuteScalarQry("Select ParentCatId from dbo.SI_CategoryMaster where CatId=" +drpCat.SelectedValue.ToString()) == "1")
        {
            if (this.drpclass.SelectedIndex == 0)
                ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('Please Select Class');", true);
            else
               Fillgrid();
        }
        else
           Fillgrid();
    }

    private void Fillgrid()
    {
       obj = new clsDAL();
       dt = new DataTable();
       ht = new Hashtable();
        if (this.drpCat.SelectedIndex > 0)
           ht.Add((object)"@CatId", (object)this.drpCat.SelectedValue.ToString().Trim());
        if (this.drpBrand.SelectedValue != "")
           ht.Add((object)"@BrandId", (object)this.drpBrand.SelectedValue.ToString().Trim());
        if (this.drpclass.SelectedIndex > 0)
           ht.Add((object)"@ClassId", (object)this.drpclass.SelectedValue.ToString().Trim());
        if (this.txtItem.Text.Trim() != "Search by Item Name or Item Barcode")
           ht.Add((object)"@ItemName", (object)this.txtItem.Text.Trim());
       dt =obj.GetDataTable("ACTS_GetItems",ht);
        if (this.dt.Rows.Count > 0)
        {
           grdItem.DataSource = (object)this.dt;
           grdItem.DataBind();
           lblRecord.Text = "Total Record : " +dt.Rows.Count.ToString();
        }
        else
        {
           grdItem.DataSource = (object)null;
           grdItem.DataBind();
           lblRecord.Text = string.Empty;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
       Response.Redirect("ItemMaster.aspx");
    }
}