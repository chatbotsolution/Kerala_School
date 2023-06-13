using AjaxControlToolkit;
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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_InitStockConsume : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private DataSet ds = new DataSet();
    private Hashtable ht = new Hashtable();
    private ScriptManager sm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            lblMsg.Text = "";
            if (Page.IsPostBack)
                return;
            bindDropDown(ddlBrand, "Select BrandId,BrandName from dbo.SI_BrandMaster", "BrandName", "BrandId");
            bindCategory(ddlCategory, "Proc_DrpGetCat", "CatName", "CatId");
            if (Request.QueryString["PID"] != null)
                fillUpdateGrid(long.Parse(Request.QueryString["PID"].ToString()));
            obj = new clsDAL();
            string str = obj.ExecuteScalarQry("select convert(varchar(20),PurDate,106) as Dt from dbo.ACTS_PurchaseDoc where InvNo like 'Init_%' and PurDate>=(select top(1) StartDate from dbo.ACTS_FinancialYear where (IsFinalized is null or IsFinalized=0) order by FY_Id desc) and PurDate<=(select top(1) EndDate from dbo.ACTS_FinancialYear where (IsFinalized is null or IsFinalized=0) order by FY_Id desc)");
            if (str.Trim() != "")
            {
                dtpInitDt.SetDateValue(Convert.ToDateTime(str.Trim()));
                dtpInitDt.Enabled = false;
                txtInitDt.Enabled = false;
            }
            else
            {
                dtpInitDt.Enabled = false;
                txtInitDt.Enabled = true;
            }
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void fillUpdateGrid(long pid)
    {
        try
        {
            ht.Clear();
            ht.Add("@PID", pid);
            ht.Add("@NoOfChar", 8);
            ds = obj.GetDataSet("ACTS_InitialStockDtls", ht);
            gvUpdtStock.DataSource = ds.Tables[0];
            gvUpdtStock.DataBind();
            foreach (GridViewRow row in gvUpdtStock.Rows)
            {
                DropDownList control1 = (DropDownList)row.FindControl("ddlCurrency");
                HiddenField control2 = (HiddenField)row.FindControl("hdncurrency");
                bindDropDown(control1, "select CurrencyId,CurrencyCode+'('+CurrencySymbol+')' as Currency from dbo.ACTS_CurrencyMaster", "Currency", "CurrencyId");
                control1.Items.RemoveAt(0);
                control1.SelectedValue = control2.Value;
            }
            pnlInitialize.Visible = false;
            pnlSelection.Visible = false;
            btnUpdate.Visible = true;
            pnlUpdate.Visible = true;
        }
        catch (Exception ex)
        {
        }
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

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (!CheckDataFill(gvStockInitialize))
            {
                lblMsg.Text = "Fill all the Fields";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                double num1 = 0.0;
                double num2 = 0.0;
                double num3 = 0.0;
                double num4 = 0.0;
                string str;
                if (ddlCategory.SelectedValue == "1")
                    str = "Init_" + ddlBrand.SelectedValue.ToString() + "_" + ddlCategory.SelectedValue.ToString() + "_" + ddlClass.SelectedValue.ToString().Trim();
                else
                    str = "Init_" + ddlBrand.SelectedValue.ToString() + "_" + ddlCategory.SelectedValue.ToString();
                ht.Clear();
                ht.Add("@POID", int.Parse("0"));
                ht.Add("@PurDate", dtpInitDt.GetDateValue().ToString("dd MMM yyyy"));
                ht.Add("@InvNo", str);
                ht.Add("@SupplierId", int.Parse("0"));
                ht.Add("@TotPurAmt", double.Parse("0"));
                ht.Add("@VatAmt", num2);
                ht.Add("@AddnlChrgAmt", num3);
                ht.Add("@ShipCharge", num4);
                ht.Add("@TotBillAmt", num1);
                ht.Add("@Remarks", txtRemarks.Text.Trim());
                ht.Add("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
                ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
                ht.Add("@Initialize", 1);
                dt.Clear();
                dt = obj.GetDataTable("ACTS_InsReceiveOrder", ht);
                int int32 = Convert.ToInt32(dt.Rows[0][0].ToString());
                foreach (GridViewRow row in gvStockInitialize.Rows)
                {
                    HiddenField control1 = (HiddenField)row.FindControl("hdnItemCode");
                    TextBox control2 = (TextBox)row.FindControl("txtQty");
                    TextBox control3 = (TextBox)row.FindControl("txtUnitMRP");
                    TextBox control4 = (TextBox)row.FindControl("txtUnitPurPrice");
                    TextBox control5 = (TextBox)row.FindControl("txtUnitSalePrice");
                    DropDownList control6 = (DropDownList)row.FindControl("ddlCurrency");
                    ViewState["ItemCode"] = control1.Value.ToString();
                    ht.Clear();
                    ht.Add("@PurchaseId", long.Parse(int32.ToString()));
                    ht.Add("@ItemCode", int.Parse(control1.Value.ToString()));
                    ht.Add("@QtyIn", float.Parse(control2.Text.Trim().ToString()));
                    ht.Add("@Unit_MRP", double.Parse(control3.Text.Trim().ToString()));
                    ht.Add("@Unit_PurPrice", double.Parse(control4.Text.Trim().ToString()));
                    ht.Add("@Unit_SalePrice", double.Parse(control5.Text.Trim().ToString()));
                    ht.Add("@CurrencyId", int.Parse(control6.SelectedValue));
                    ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
                    if (double.Parse(control4.Text.Trim()) > 0.0)
                        ht.Add("@UnitLandingCost", double.Parse(control4.Text.Trim()));
                    else if (double.Parse(control5.Text.Trim()) > 0.0)
                        ht.Add("@UnitLandingCost", double.Parse(control5.Text.Trim()));
                    dt.Clear();
                    dt = obj.GetDataTable("ACTS_InsReceiveOrderDtls", ht);
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
                    lblMsg.Text = "Stock Initialized Successfully for Category " + ddlCategory.SelectedItem;
                    lblMsg.ForeColor = Color.Green;
                }
                else
                {
                    lblMsg.Text = "Stock Initialized Successfully for Category " + ddlCategory.SelectedItem + " But Stock In Trade Was Not Updated";
                    lblMsg.ForeColor = Color.Green;
                }
                ClearAll();
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
            if (((TextBox)row.FindControl("txtQty")).Text.Trim() == "")
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
        }
        return flag;
    }

    private void ClearAll()
    {
        gvStockInitialize.DataSource = null;
        gvStockInitialize.DataBind();
        pnlSave.Visible = false;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            ht.Add("@NoOfChar", 10);
            ht.Add("@Brandid", int.Parse(ddlBrand.SelectedValue));
            ht.Add("@Catid", int.Parse(ddlCategory.SelectedValue));
            ht.Add("@ClassId", int.Parse(ddlClass.SelectedValue));
            dt = obj.GetDataTable("ACTS_GetListOfItemsConsume", ht);
            gvStockInitialize.DataSource = dt;
            gvStockInitialize.DataBind();
            if (dt.Rows.Count > 0)
            {
                foreach (Control row in gvStockInitialize.Rows)
                {
                    DropDownList control = (DropDownList)row.FindControl("ddlCurrency");
                    bindDropDown(control, "select CurrencyId,CurrencyCode+'('+CurrencySymbol+')' as Currency from dbo.ACTS_CurrencyMaster", "Currency", "CurrencyId");
                    control.Items.RemoveAt(0);
                    control.SelectedValue = "1";
                }
                pnlSave.Visible = true;
                sm = Master.FindControl("ScriptManager1") as ScriptManager;
                sm.SetFocus((Control)ddlCategory);
            }
            else
            {
                pnlSave.Visible = false;
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
            if (!CheckDataFill(gvUpdtStock))
            {
                lblMsg.Text = "Fill all th Fileds";
                lblMsg.ForeColor = Color.Red;
            }
            else
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
                lblMsg.Text = "Stock Initialized Successfully Updated";
                lblMsg.ForeColor = Color.Green;
                ClearAll();
                pnlInitialize.Visible = true;
                pnlSelection.Visible = true;
                pnlUpdate.Visible = false;
                btnUpdate.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }
}