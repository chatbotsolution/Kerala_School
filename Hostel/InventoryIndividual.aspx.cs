﻿using ASP;
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

public partial class Hostel_InventoryIndividual : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
            Response.Redirect("../Login.aspx");
        else
            lblMsg.Text = "";
        if (Page.IsPostBack)
            return;
        BindList();
        binditem();
        ViewState["EditSlNo"] = string.Empty;
        ViewState["dtInsp"] = setInitialDt();
        hfAvlQty.Value = string.Empty;
    }

    private void BindList()
    {
        if (Session["User"].ToString() == "admin")
        {
            bindDropDown(drpIssuedFrom, "Select Location,LocationId from dbo.SI_LocationMaster where SchoolId=" + Convert.ToInt32(Session["SchoolId"]) + " and Location='Hostel Store'", "Location", "LocationId");
            drpIssuedFrom.Items.RemoveAt(0);
        }
        bindDropDown(drpIssuedto, "select S.FullName,H.AdmnNo from dbo.PS_StudMaster S inner join dbo.Host_Admission H on S.AdmnNo=H.AdmnNo Where H.LeavingDt is null or LeavingDt ='' ", "FullName", "AdmnNo");
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
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    private void binditem()
    {
        dt = new DataTable();
        dt = obj.GetDataTable("SI_BindAllItem", new Hashtable()
    {
      {
         "@LocationId",
         drpIssuedFrom.SelectedValue
      }
    });
        drpItem.DataSource = dt;
        drpItem.DataTextField = "ItemName";
        drpItem.DataValueField = "ItemCode";
        drpItem.DataBind();
        drpItem.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    private DataTable setInitialDt()
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add("SlNo", typeof(int));
        dataTable.Columns.Add("ItemCode", typeof(string));
        dataTable.Columns.Add("ItemName", typeof(string));
        dataTable.Columns.Add("Qty", typeof(string));
        dataTable.AcceptChanges();
        return dataTable;
    }

    protected void drpItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpItem.SelectedIndex > 0)
            AvailableQuan();
        else
            lblAvlQty.Text = hfAvlQty.Value = "";
    }

    private void AvailableQuan()
    {
        dt = obj.GetDataTableQry("select Sum(AvlQty)as AvlQun from dbo.SI_LocationwiseStock  where Itemcode=" + drpItem.SelectedValue + " and LocationId=" + drpIssuedFrom.SelectedValue);
        if (dt.Rows.Count > 0)
        {
            double num1 = double.Parse(dt.Rows[0]["AvlQun"].ToString());
            double num2;
            if (ViewState["dtInsp"] != null)
            {
                DataRow[] dataRowArray = ((DataTable)ViewState["dtInsp"]).Select("ItemCode=" + drpItem.SelectedValue.ToString());
                double num3 = 0.0;
                if (dataRowArray.Length > 0)
                    num3 = double.Parse(dataRowArray[0]["Qty"].ToString());
                num2 = num1 - num3;
            }
            else
                num2 = num1;
            lblAvlQty.Text = "Available Quantity: " + num2;
            lblAvlQty.ForeColor = Color.Red;
            hfAvlQty.Value = num1.ToString();
        }
        else
            lblAvlQty.Text = "No Stock Available ! ";
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        btnSave.Enabled = true;
        btnCancel2.Enabled = true;
        btnShowDetails.Enabled = true;
        string empty = string.Empty;
        string str = obj.ExecuteScalarQry("Select ItemName from dbo.SI_ItemMaster where ItemCode='" + drpItem.SelectedValue.ToString() + "'");
        if (ViewState["dtInsp"].ToString() == string.Empty)
        {
            ViewState["dtInsp"] = setInitialDt();
            DataTable dataTable = (DataTable)ViewState["dtInsp"];
            if (dataTable.Select(" ItemCode='" + drpItem.SelectedValue + "'").Length > 0)
            {
                lblMsg.Text = "Record already exists";
                lblMsg.ForeColor = Color.Red;
            }
            else if (ViewState["EditSlNo"].ToString().Trim() == "")
            {
                DataRow row = dataTable.NewRow();
                if (dataTable.Rows.Count > 0)
                {
                    int int32 = Convert.ToInt32(dataTable.Compute("MAX(SlNo)", ""));
                    row["SlNo"] = (int32 + 1);
                }
                else
                    row["SlNo"] = (dataTable.Rows.Count + 1);
                row["ItemCode"] = drpItem.SelectedValue;
                row["ItemName"] = str;
                row["Qty"] = txtQty.Text.Trim();
                dataTable.Rows.Add(row);
                ViewState["dtInsp"] = dataTable;
                Bindgrid(dataTable);
                Clear();
            }
        }
        else
        {
            DataTable dataTable1 = (DataTable)ViewState["dtInsp"];
            if (ViewState["EditSlNo"].ToString().Trim() == "")
            {
                if (dataTable1.Select("ItemCode=" + drpItem.SelectedValue).Length > 0)
                {
                    lblMsg.Text = "Item already exists If You Want To Modify Please Go to Edit! ";
                    lblMsg.ForeColor = Color.Red;
                    Clear();
                }
                else
                {
                    DataRow row = dataTable1.NewRow();
                    if (dataTable1.Rows.Count > 0)
                    {
                        int int32 = Convert.ToInt32(dataTable1.Compute("MAX(SlNo)", ""));
                        row["SlNo"] = (int32 + 1);
                    }
                    else
                        row["SlNo"] = (dataTable1.Rows.Count + 1);
                    row["ItemCode"] = drpItem.SelectedValue;
                    row["ItemName"] = str;
                    row["Qty"] = txtQty.Text.Trim();
                    dataTable1.Rows.Add(row);
                    ViewState["dtInsp"] = dataTable1;
                    Bindgrid(dataTable1);
                    AvailableQuan();
                    Clear();
                }
            }
            else
            {
                DataTable dataTable2 = new DataTable();
                DataTable dataTable3 = (DataTable)ViewState["dtInsp"];
                foreach (DataRow row in dataTable3.Select("SlNo ='" + ViewState["EditSlNo"].ToString().Trim() + "'"))
                    dataTable3.Rows.Remove(row);
                dataTable3.AcceptChanges();
                ViewState["dtInsp"] = dataTable3;
                DataRow row1 = dataTable1.NewRow();
                row1["SlNo"] = ViewState["EditSlNo"].ToString().Trim();
                row1["ItemCode"] = drpItem.SelectedValue;
                row1["ItemName"] = str;
                row1["Qty"] = txtQty.Text.Trim();
                dataTable1.Rows.Add(row1);
                ViewState["dtInsp"] = dataTable1;
                Bindgrid(dataTable3);
                ViewState["EditSlNo"] = string.Empty;
                AvailableQuan();
                Clear();
            }
        }
        lblAvlQty.Text = "";
    }

    private void Bindgrid(DataTable dataTable)
    {
        grvAdd.DataSource = dataTable;
        Session["IssueDtl"] = dataTable;
        grvAdd.DataBind();
        if (dataTable.Rows.Count > 0)
        {
            grvAdd.Visible = true;
            btnSave.Visible = true;
            btnCancel.Visible = true;
            Session["grd"] = dataTable;
        }
        else
            grvAdd.Visible = false;
    }

    protected void grvAdd_RowEditing(object sender, GridViewEditEventArgs e)
    {
        HiddenField control = grvAdd.Rows[e.NewEditIndex].FindControl("hfqty") as HiddenField;
        DataRow[] dataRowArray = ((DataTable)ViewState["dtInsp"]).Select("SlNo ='" + grvAdd.DataKeys[e.NewEditIndex].Value.ToString().Trim() + "'");
        drpItem.SelectedValue = dataRowArray[0]["ItemCode"].ToString();
        txtQty.Text = dataRowArray[0]["Qty"].ToString();
        txtQty.Text = Convert.ToInt32(control.Value).ToString();
        AvailableQuan();
        ViewState["EditSlNo"] = grvAdd.DataKeys[e.NewEditIndex].Value.ToString();
        lblAvlQty.Text = "Available Quantity: " + hfAvlQty.Value;
        lblAvlQty.ForeColor = Color.Red;
    }

    protected void grvAdd_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            grvAdd.Rows[e.RowIndex].FindControl("hfqty");
            DataTable dataTable = (DataTable)ViewState["dtInsp"];
            dataTable.Rows.RemoveAt(e.RowIndex);
            grvAdd.DataSource = dataTable;
            grvAdd.DataBind();
            AvailableQuan();
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
        drpItem.SelectedIndex = 0;
        txtQty.Text = "";
        lblAvlQty.Text = "";
    }

    protected void grvAdd_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grvAdd.PageIndex = e.NewPageIndex;
        Bindgrid(dt);
    }

    protected void btnShowDetails_Click(object sender, EventArgs e)
    {
        Response.Redirect("InventoryIndividualList.aspx");
    }

    protected void btnCancel2_Click(object sender, EventArgs e)
    {
        Clear();
        txtIssueDt.Text = "";
        drpIssuedto.SelectedIndex = 0;
        drpIssuedFrom.SelectedIndex = 0;
        ViewState["dtInsp"] = null;
        grvAdd.DataSource = null;
        grvAdd.DataBind();
        ViewState["dtInsp"] = setInitialDt();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        InsertDetails();
        lblAvlQty.Text = string.Empty;
        grvAdd.DataSource = null;
        grvAdd.DataBind();
        ViewState["dtInsp"] = setInitialDt();
    }

    private void InsertDetails()
    {
        try
        {
            DataTable dataTable = (DataTable)ViewState["dtInsp"];
            Hashtable hashtable = new Hashtable();
            if (Request.QueryString["IId"] != null)
                hashtable.Add("IssueId", int.Parse(Request.QueryString["IId"].ToString()));
            hashtable.Add("IssueDate", dtpdt.GetDateValue());
            hashtable.Add("IssuedBy", Session["User"]);
            hashtable.Add("IssuedFromLocId", drpIssuedFrom.SelectedValue);
            hashtable.Add("IssuedTo", drpIssuedto.SelectedItem.ToString());
            hashtable.Add("IssuedToAdmnNo", drpIssuedto.SelectedValue);
            hashtable.Add("@UserID", Convert.ToInt32(Session["User_Id"]));
            hashtable.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
            dt = obj.GetDataTable("Host_InsUpdtIssue", hashtable);
            int int32 = Convert.ToInt32(dt.Rows[0][0].ToString());
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                hashtable.Clear();
                hashtable.Add("ItemCode", row["ItemCode"].ToString());
                hashtable.Add("Qty", row["Qty"].ToString());
                hashtable.Add("IssueId", int32);
                hashtable.Add("FromLocId", drpIssuedFrom.SelectedValue.ToString().Trim());
                hashtable.Add("ToLocId", drpIssuedto.SelectedValue.ToString().Trim());
                hashtable.Add("UserID", Convert.ToInt32(Session["User_Id"]));
                hashtable.Add("SchoolId", Convert.ToInt32(Session["SchoolId"]));
                dt = obj.GetDataTable("Host_InsUpdtIssueDtl", hashtable);
            }
            if (dt.Rows.Count > 0)
            {
                lblMsg.Text = "Record Cant  Saved ";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                lblMsg.Text = "Data Saved Successfully";
                lblMsg.ForeColor = Color.Green;
            }
        }
        catch (Exception ex)
        {
        }
    }
}