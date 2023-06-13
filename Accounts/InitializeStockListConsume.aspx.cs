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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_InitializeStockListConsume : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private ScriptManager sm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (!Page.IsPostBack)
            {
                bindDropDown(ddlBrand, "Select BrandId,BrandName from dbo.SI_BrandMaster", "BrandName", "BrandId");
                bindCategory(ddlCategory, "Proc_DrpGetCat", "CatName", "CatId");
            }
        }
        else
            Response.Redirect("../Login.aspx");
        lblMsg.Text = "";
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
            drp.Items.Insert(0, new ListItem("- ALL -", "0"));
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
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
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    private void BindInitialStockList()
    {
        dt = obj.GetDataTableQry("Select *,convert(varchar,PurDate,105) purDt from dbo.ACTS_PurchaseDoc where TotBillAmt = 0");
        gvStockInitializeList.DataSource = dt;
        gvStockInitializeList.DataBind();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("InitializeStockListConsume.aspx");
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlCategory.SelectedValue == "1" || obj.ExecuteScalarQry("Select ParentCatId from dbo.SI_CategoryMaster where CatId=" + ddlCategory.SelectedValue.ToString()).Trim() == "1")
            {
                spanClass.Visible = true;
                bindDropDown(ddlClass, "select ClassID, ClassName from dbo.PS_ClassMaster order by ClassID", "ClassName", "ClassID");
            }
            else
                spanClass.Visible = false;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
        sm = Master.FindControl("ScriptManager1") as ScriptManager;
        sm.SetFocus((Control)ddlCategory);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            ht.Add("@NoOfChar", 10);
            if (ddlBrand.SelectedIndex > 0)
                ht.Add("@Brandid", int.Parse(ddlBrand.SelectedValue));
            ht.Add("@Catid", int.Parse(ddlCategory.SelectedValue));
            ht.Add("@ClassId", int.Parse(ddlClass.SelectedValue));
            dt = obj.GetDataTable("ACTS_GetItemInitListConsume", ht);
            gvUpdtStock.DataSource = dt;
            gvUpdtStock.DataBind();
            if (dt.Rows.Count > 0)
            {
                foreach (Control row in gvUpdtStock.Rows)
                {
                    DropDownList control = (DropDownList)row.FindControl("ddlCurrency");
                    bindDropDown(control, "select CurrencyId,CurrencyCode+'('+CurrencySymbol+')' as Currency from dbo.ACTS_CurrencyMaster", "Currency", "CurrencyId");
                    control.Items.RemoveAt(0);
                    control.SelectedValue = "1";
                }
                btnUpdate.Visible = true;
                sm = Master.FindControl("ScriptManager1") as ScriptManager;
                sm.SetFocus((Control)ddlCategory);
            }
            else
            {
                btnUpdate.Visible = false;
                sm = Master.FindControl("ScriptManager1") as ScriptManager;
                sm.SetFocus((Control)ddlBrand);
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            obj = new clsDAL();
            if (!CheckDataFill(gvUpdtStock))
            {
                if (lblMsg.Text.Trim() == "")
                    lblMsg.Text = "Fill all the Fields";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                try
                {
                    foreach (GridViewRow row in gvUpdtStock.Rows)
                    {
                        HiddenField control1 = (HiddenField)row.FindControl("hdnPurDtlId");
                        TextBox control2 = (TextBox)row.FindControl("txtQty");
                        TextBox control3 = (TextBox)row.FindControl("txtUnitMRP");
                        TextBox control4 = (TextBox)row.FindControl("txtUnitPurPrice");
                        TextBox control5 = (TextBox)row.FindControl("txtUnitSalePrice");
                        DropDownList control6 = (DropDownList)row.FindControl("ddlCurrency");
                        ht.Clear();
                        ht.Add("@PurchaseDetailId", long.Parse(control1.Value));
                        ht.Add("@QtyIn", float.Parse(control2.Text.Trim()));
                        ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
                        dt.Clear();
                        dt = obj.GetDataTable("ACTS_UpdtInitializeStock", ht);
                    }
                    obj = new clsDAL();
                    if (obj.ExecuteScalar("ACTS_InsUpdtStockInTrade", new Hashtable()
          {
            {
               "@SchoolId",
               int.Parse(Session["SchoolId"].ToString())
            },
            {
               "@UserId",
               int.Parse(Session["User_Id"].ToString())
            }
          }).Trim() == "")
                    {
                        lblMsg.Text = "Stock Initialized Successfully Updated";
                        lblMsg.ForeColor = Color.Green;
                        gvUpdtStock.DataSource = null;
                        gvUpdtStock.DataBind();
                        btnUpdate.Visible = false;
                    }
                    else
                    {
                        lblMsg.Text = "Stock Initialized Successfully. Updated But Stock In Trade Value Not Updated!!";
                        lblMsg.ForeColor = Color.Green;
                        gvUpdtStock.DataSource = null;
                        gvUpdtStock.DataBind();
                        btnUpdate.Visible = false;
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private bool CheckDataFill(GridView gv)
    {
        bool flag = true;
        if (gv.Rows.Count == 0)
            flag = false;
        foreach (GridViewRow row in gv.Rows)
        {
            TextBox control1 = (TextBox)row.FindControl("txtQty");
            if (control1.Text.Trim() == "")
            {
                flag = false;
                break;
            }
            if (((TextBox)row.FindControl("txtUnitMRP")).Text.Trim() == "")
            {
                flag = false;
                break;
            }
            if (((TextBox)row.FindControl("txtUnitPurPrice")).Text.Trim() == "")
            {
                flag = false;
                break;
            }
            if (((TextBox)row.FindControl("txtUnitSalePrice")).Text.Trim() == "")
            {
                flag = false;
                break;
            }
            HiddenField control2 = (HiddenField)row.FindControl("hfQtyOut");
            if (Convert.ToDecimal(control1.Text.Trim()) < Convert.ToDecimal(control2.Value.ToString().Trim()))
            {
                flag = false;
                control1.Focus();
                lblMsg.Text = "Init stock Qty cannot be less than Sold stock Qty!!";
                break;
            }
        }
        return flag;
    }
}