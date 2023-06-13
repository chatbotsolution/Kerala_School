using AjaxControlToolkit;
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

public partial class Accounts_DefineROL : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        lblRecCount.Text = string.Empty;
        btnSubmit2.Visible = false;
        Page.Form.DefaultButton = btnShow.UniqueID;
        btnSubmit.Enabled = false;
        if (Page.IsPostBack)
            return;
        filldropdowns();
        lblRecCount.Text = string.Empty;
    }

    private void filldropdowns()
    {
        FillCategory();
        FillItem();
    }

    private void FillCategory()
    {
        DataTable dataTable = new clsDAL().GetDataTable("Proc_DrpGetCat");
        if (dataTable.Rows.Count <= 0)
            return;
        drpCategory.Items.Clear();
        drpCategory.Items.Add(new ListItem("--All--", "0"));
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            drpCategory.Items.Add(new ListItem(row["CatName"].ToString(), row["CatId"].ToString()));
    }

    private void FillItem()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpCategory.SelectedIndex > 0)
            hashtable.Add("@CatId", drpCategory.SelectedValue.ToString().Trim());
        hashtable.Add("@UserId", Session["User_Id"]);
        hashtable.Add("@ShopId", Session["ShopId"]);
        DataTable dataTable = clsDal.GetDataTable("GetROLItemList", hashtable);
        if (dataTable.Rows.Count <= 0)
            return;
        drpItem.Items.Clear();
        drpItem.Items.Add(new ListItem("--All--", "0"));
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            drpItem.Items.Add(new ListItem(row["ItemName"].ToString(), row["ItemCode"].ToString()));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void drpCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillItem();
        grdRolItem.Visible = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string str = string.Empty;
        foreach (GridViewRow row in grdRolItem.Rows)
        {
            clsDAL clsDal = new clsDAL();
            Hashtable hashtable = new Hashtable();
            DataTable dataTable1 = new DataTable();
            TextBox control1 = (TextBox)row.FindControl("txtROL");
            HiddenField control2 = (HiddenField)row.FindControl("hfItemCode");
            if (control1.Text != "0")
            {
                if (control1.Text.Trim() != "")
                    hashtable.Add("@ROL", Convert.ToDecimal(control1.Text.ToString()));
                else
                    hashtable.Add("@ROL", 0);
                hashtable.Add("@ItemCode", Convert.ToInt64(control2.Value));
                hashtable.Add("@UserId", Session["User_Id"]);
                hashtable.Add("@ShopId", Session["ShopId"]);
                DataTable dataTable2 = clsDal.GetDataTable("UpdateRol", hashtable);
                if (dataTable2.Rows.Count > 0)
                    str = str + dataTable2.Rows[0][0].ToString() + ',';
            }
        }
        FillGrid();
        if (str.Trim().Equals(string.Empty))
        {
            lblMsg.Text = "Record Saved successfully !";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = "Data Saved Failed !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void FillGrid()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpCategory.SelectedIndex > 0)
            hashtable.Add("@CatId", drpCategory.SelectedValue.ToString().Trim());
        if (drpItem.SelectedIndex > 0)
            hashtable.Add("@ItemCode", drpItem.SelectedValue.ToString().Trim());
        hashtable.Add("@UserId", Session["User_Id"]);
        hashtable.Add("@ShopId", Session["ShopId"]);
        DataTable dataTable = clsDal.GetDataTable("GetROLItemGrid", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            btnSubmit.Enabled = true;
            grdRolItem.DataSource = dataTable;
            grdRolItem.DataBind();
            grdRolItem.Visible = true;
            lblRecCount.Text = "Total Number of Items : " + dataTable.Rows.Count.ToString();
            if (dataTable.Rows.Count <= 10)
                return;
            btnSubmit2.Visible = true;
        }
        else
        {
            btnSubmit.Enabled = false;
            grdRolItem.Visible = false;
            lblRecCount.Text = string.Empty;
            lblMsg.Text = "No Records Found !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void drpItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdRolItem.Visible = false;
        btnSubmit.Enabled = false;
        lblRecCount.Text = string.Empty;
    }
}