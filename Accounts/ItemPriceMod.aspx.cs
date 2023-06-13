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

public partial class Accounts_ItemPriceMod : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            lblMsg.Text = "";
            if (Page.IsPostBack)
                return;
            bindBrand();
            bindDropDown(drpCategory, "Proc_DrpGetCat", "CatName", "CatId");
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
            drp.Items.Insert(0, new ListItem("--All--", "0"));
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void bindBrand()
    {
        try
        {
            drpBrand.DataSource = null;
            drpBrand.DataBind();
            drpBrand.DataSource = obj.GetDataTableQry("Select BrandId,BrandName from dbo.SI_BrandMaster");
            drpBrand.DataTextField = "BrandName";
            drpBrand.DataValueField = "BrandId";
            drpBrand.DataBind();
        }
        catch (Exception ex)
        {
        }
    }

    protected void drpCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (drpCategory.SelectedValue == "1" || obj.ExecuteScalarQry("Select ParentCatId from dbo.SI_CategoryMaster where CatId=" + drpCategory.SelectedValue.ToString()).Trim() == "1")
            {
                pnlclass.Visible = true;
                bindDropDown(drpClass, "select ClassID, ClassName from dbo.PS_ClassMaster order by ClassID", "ClassName", "ClassID");
            }
            else
                pnlclass.Visible = false;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error Occured ! Try Again";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTable1 = new DataTable();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@BrandId", drpBrand.SelectedValue.ToString().Trim());
            if (drpCategory.SelectedIndex > 0)
                hashtable.Add("@Catid", drpCategory.SelectedValue.ToString().Trim());
            hashtable.Add("@ClassId", drpClass.SelectedValue.ToString().Trim());
            DataTable dataTable2 = obj.GetDataTable("ACTS_GetItemsPrice", hashtable);
            grdItemPrice.DataSource = dataTable2;
            grdItemPrice.DataBind();
            if (dataTable2.Rows.Count > 0)
                btnSubmit.Visible = true;
            else
                btnSubmit.Visible = false;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Cannot Find Item!! Plz Try Again!!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private bool CheckDataFill(GridView gv)
    {
        bool flag = true;
        if (gv.Rows.Count == 0)
            flag = false;
        foreach (Control row in gv.Rows)
        {
            if (((TextBox)row.FindControl("txtUnitSalePrice")).Text.Trim() == "")
            {
                flag = false;
                break;
            }
        }
        return flag;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        try
        {
            if (!CheckDataFill(grdItemPrice))
            {
                lblMsg.Text = "Please Fill All Data in Grid";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                foreach (GridViewRow row in grdItemPrice.Rows)
                {
                    Label control1 = row.FindControl("lblItemCode") as Label;
                    TextBox control2 = row.FindControl("txtUnitSalePrice") as TextBox;
                    hashtable.Clear();
                    hashtable.Add("@ItemCode", control1.Text.Trim());
                    hashtable.Add("@SalePrice", control2.Text.Trim());
                    hashtable.Add("@UserId", Convert.ToInt32(Session["User_Id"].ToString()));
                    obj.ExecuteScalar("ACTS_UpdtItemPrice", hashtable);
                }
                lblMsg.Text = "Sale Price Modified Successfully";
                lblMsg.ForeColor = Color.Green;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Unable to Update Sale Price!!Plz Try Again!!";
            lblMsg.ForeColor = Color.Red;
        }
    }
}