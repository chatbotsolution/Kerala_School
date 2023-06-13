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

public partial class Inventory_InvWritesOffList : System.Web.UI.Page
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
            Page.Form.DefaultButton = btnSearch.UniqueID;
            BindCat();
            if (Session["User"].ToString() == "admin")
            {
                bindDropDown(ddlLocation, "select LocationId,Location from dbo.SI_LocationMaster order by Location", "Location", "LocationId");
                ddlLocation.Visible = true;
                lbllocation.Visible = true;
            }
            else
            {
                ddlLocation.Visible = false;
                lbllocation.Visible = false;
            }
            btnDelete.Enabled = false;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void BindCat()
    {
        DataTable dataTable = new clsDAL().GetDataTable("SI_ItemCatList");
        if (dataTable.Rows.Count <= 0)
            return;
        ddlCategory.Items.Clear();
        ddlCategory.Items.Add(new ListItem("--All--", "0"));
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            ddlCategory.Items.Add(new ListItem(row["CatName"].ToString(), row["CatId"].ToString()));
        foreach (ListItem listItem in ddlCategory.Items)
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
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--All--", "0"));
    }

    private void BindGrid(string qry)
    {
        dt = obj.GetDataTableQry(qry);
        grdWriteOffList.DataSource = dt;
        grdWriteOffList.DataBind();
        if (dt.Rows.Count > 0)
        {
            lblRecords.Text = "Total Record(s): " + dt.Rows.Count.ToString();
            btnDelete.Enabled = true;
        }
        else
        {
            lblMsg.Text = "No Record Found !";
            lblMsg.ForeColor = Color.Red;
            lblRecords.Text = string.Empty;
            btnDelete.Enabled = false;
        }
    }

    private string GetQry()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select W.WriteOffId,W.LocationId,I.CatId,W.ItemCode,I.ItemName,W.Quantity,WriteOffDate,convert(varchar,WriteOffDate,106) writeoffDt,W.Description from dbo.SI_WriteOff W inner join dbo.SI_ItemMaster I on I.ItemCode=W.ItemCode where 1=1");
        if (ddlLocation.Visible && ddlLocation.SelectedIndex > 0)
            stringBuilder.Append(" and W.LocationId='" + ddlLocation.SelectedValue + "'");
        if (ddlCategory.SelectedIndex > 0)
            stringBuilder.Append(" and I.CatId IN (SELECT CatId FROM SI_fuGetCatAncestors(" + ddlCategory.SelectedValue.ToString().Trim() + "))");
        if (ddlItem.SelectedIndex > 0)
            stringBuilder.Append(" and W.ItemCode='" + ddlItem.SelectedValue + "'");
        stringBuilder.Append(" order by CatId,ItemCode");
        return stringBuilder.ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                string str = Request["Checkb"];
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str.Split(chArray))
                    DeleteAccession(obj.ToString());
            }
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }

    private void DeleteAccession(string IdList)
    {
        ht.Clear();
        ht.Add("@WriteOffIdList", IdList);
        if (obj.ExecuteScalar("SI_DeleteWriteOff", ht) == "SUCCESS")
        {
            lblMsg.Text = "Deleted Successfully";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = "Deletion Failed";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("InvWritesOff.aspx");
    }

    protected void grdWriteOffList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdWriteOffList.PageIndex = e.NewPageIndex;
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCategory.SelectedIndex > 0)
            bindDropDown(ddlItem, "select ItemCode,ItemName from dbo.SI_ItemMaster where CatId IN (SELECT CatId FROM SI_fuGetCatAncestors(" + ddlCategory.SelectedValue.ToString().Trim() + ")) order by ItemName", "ItemName", "ItemCode");
        else
            ddlItem.Items.Clear();
    }
}