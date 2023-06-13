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

public partial class Masters_ItemMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.DefaultButton = btnSubmit.UniqueID;
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        ViewState["Item"] = string.Empty;
        ViewState["ItemImg"] = string.Empty;
        ViewState["BrochureImg"] = string.Empty;
        FillDropdowns();
        if (Request.QueryString["PId"] == null)
            return;
        ViewState["Item"] = "Update";
        FillData();
        btnCancel.Enabled = false;
    }

    private void bindCategory()
    {
        DataTable dataTableQry = obj.GetDataTableQry("SELECT CatId,REPLICATE('-',pos)+CatName AS CatName FROM dbo.Web_vwCategories where ParentCatId=1 ORDER BY hierarchy");
        if (dataTableQry.Rows.Count <= 0)
            return;
        drpCat.Items.Clear();
        drpCat.Items.Add(new ListItem("--Select--", "0"));
        foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
            drpCat.Items.Add(new ListItem(row["CatName"].ToString(), row["CatId"].ToString()));
    }

    private void bindCategoryOthers()
    {
        DataTable dataTableQry = obj.GetDataTableQry("SELECT CatId,REPLICATE('--',pos)+CatName AS CatName FROM dbo.Web_vwCategories where CatId<>1 and ParentCatId<>1 ORDER BY hierarchy");
        if (dataTableQry.Rows.Count <= 0)
            return;
        drpCat.Items.Clear();
        drpCat.Items.Add(new ListItem("--Select--", "0"));
        foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
            drpCat.Items.Add(new ListItem(row["CatName"].ToString(), row["CatId"].ToString()));
    }

    private void FillDropdowns()
    {
        chklClass.Enabled = true;
        chklClass.Height = (Unit)225;
        bindClass();
        new clsStaticDropdowns().FillMeasuringUnits(drpMeasureUnit);
        bindCategory();
        obj = new clsDAL();
        DataTable dataTable = obj.GetDataTable("SI_DrpBrandList");
        if (dataTable.Rows.Count > 0)
        {
            drpBrand.Items.Clear();
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                drpBrand.Items.Add(new ListItem(row["BrandName"].ToString(), row["BrandId"].ToString()));
        }
        fillTax();
    }

    private void bindClass()
    {
        DataTable dataTable = new DataTable();
        chklClass.DataSource = obj.GetDataTableQry("select ClassID, ClassName from dbo.PS_ClassMaster").DefaultView;
        chklClass.DataTextField = "ClassName";
        chklClass.DataValueField = "ClassID";
        chklClass.DataBind();
        chklClass.Items.Insert(0, new ListItem("All", "0"));
        
        
    }

    private void fillTax()
    {
        DataTable dataTable = new DataTable();
        drpTax.DataSource = obj.GetDataTableQry("select TaxId, TaxCode from dbo.SI_TaxMaster").DefaultView;
        drpTax.DataTextField = "TaxCode";
        drpTax.DataValueField = "TaxId";

        drpTax.DataBind();
    }
    private void BindStream()
    {
        ddlStream.Visible = true;
        DataTable dataTable = new DataTable();
        ddlStream.DataSource = obj.GetDataTableQry("select distinct ps_Classwisestudent.stream ,[PS_StreamMaster].[Description] from ps_Classwisestudent inner join  PS_StreamMaster on ps_Classwisestudent.Stream=PS_StreamMaster.StreamID ").DefaultView;
        ddlStream.DataTextField = "Description";
        ddlStream.DataValueField = "stream";
        ddlStream.DataBind();
        ddlStream.Items.Insert(0, new ListItem("STREAM", "0"));
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }
    protected void chklClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (chklClass.SelectedIndex == 14 || chklClass.SelectedIndex == 15)
        {

            ddlStream.Visible = true;
            DataTable dataTable = new DataTable();
            ddlStream.DataSource = obj.GetDataTableQry("select distinct ps_Classwisestudent.stream ,[PS_StreamMaster].[Description] from ps_Classwisestudent inner join  PS_StreamMaster on ps_Classwisestudent.Stream=PS_StreamMaster.StreamID WHERE ps_Classwisestudent.classId="+chklClass.SelectedIndex).DefaultView;
            ddlStream.DataTextField = "Description";
            ddlStream.DataValueField = "stream";
            ddlStream.DataBind();
            ddlStream.Items.Insert(0, new ListItem("STREAM", "0"));
        }

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string str = InsertData();
        if (str.Trim() == "DUP")
        {
            lblMsg.Text = "Duplicate Item Exists";
            lblMsg.ForeColor = Color.Red;
        }
        else if (str.Trim() == "NO")
        {
            lblMsg.Text = "Data Cannot Be Saved!! Please Try Again!!";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            if (Request.QueryString["PId"] != null)
                Response.Redirect("ItemMasterList.aspx");
            ClearAll();
            btnCancel.Enabled = true;
            lblMsg.Text = "Data saved successfully";
            lblMsg.ForeColor = Color.Green;
        }
    }

    public string InsertData()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable ht1 = new Hashtable();
        try
        {
            if (Request.QueryString["PId"] != null)
                ht1.Add("@ItemCode", int.Parse(Request.QueryString["PId"].ToString()));
            if (!txtBarCode.Text.Trim().Equals(string.Empty))
                ht1.Add("@ItemBarCode", txtBarCode.Text.ToString().Trim());
            ht1.Add("@BrandId", int.Parse(drpBrand.SelectedValue.ToString().Trim()));
            ht1.Add("@CatId", int.Parse(drpCat.SelectedValue.ToString().Trim()));
            ht1.Add("@ItemName", txtItemName.Text.ToString().Trim());
            if (!txtItemDesc.Text.Trim().Equals(string.Empty))
                ht1.Add("@ItemDesc", txtItemDesc.Text.ToString().Trim());
            if (drpMeasureUnit.SelectedIndex > 0)
                ht1.Add("@MesuringUnit", drpMeasureUnit.SelectedValue.ToString().Trim());

            if (!txtRol.Text.Trim().Equals(string.Empty))
                ht1.Add("@ROL", txtRol.Text.ToString().Trim());
            if (!txtDepreciation.Text.Trim().Equals(string.Empty))
                ht1.Add("@DepreciationPercent", txtDepreciation.Text.Trim());
            if (rbtnStatusYes.Checked.Equals(true))
                ht1.Add("@ActiveStatus", 1);
            else
                ht1.Add("@ActiveStatus", 0);
            if (rbtnCapitalYes.Checked)
                ht1.Add("@IsCapital", 1);
            else
                ht1.Add("@IsCapital", 0);
            if (rbtnConsumeYes.Checked)
                ht1.Add("@IsConsumable", 1);
            else
                ht1.Add("@IsConsumable", 0);
            if (rbtnSaleYes.Checked)
                ht1.Add("@IsSalable", 1);
            else
                ht1.Add("@IsSalable", 0);
            ht1.Add("@TaxId", drpTax.SelectedValue.ToString().Trim());
            ht1.Add("@UserId", Session["User_Id"]);
            ht1.Add("@SchoolId", Session["SchoolId"]);
            string str = clsDal.ExecuteScalar("ACTS_InsertItem", ht1);
            if (str.Trim() != "DUP" && str.Trim() != "NO")
            {
                clsDal.ExecuteScalarQry("delete from dbo.SI_ItemMaster_Class where ItemCode=" + str.Trim());
                if (rbtBooks.Checked)
                {
                    if (chklClass.SelectedIndex == 0)
                    {
                        Hashtable ht2 = new Hashtable();
                        ht2.Clear();
                        ht2.Add("@ItemCode", str.Trim());
                        ht2.Add("@ForClassId", 0);
                        clsDal.ExecuteScalar("ACTS_InsUpdtItemClass", ht2);
                    }
                    else
                    {
                        for (int index = 0; index <= chklClass.Items.Count - 1; ++index)
                        {
                            if (chklClass.Items[index].Selected)
                            {
                                if (chklClass.SelectedIndex == 14 || chklClass.SelectedIndex == 15)
                                {
                                    Hashtable ht2 = new Hashtable();
                                    ht2.Clear();
                                    ht2.Add("@ItemCode", str.Trim());
                                    ht2.Add("@ForClassId", Convert.ToInt32(chklClass.Items[index].Value));
                                    ht2.Add("@streamId", Convert.ToInt32(ddlStream.SelectedValue.ToString()));
                                    clsDal.ExecuteScalar("ACTS_InsUpdtItemClass", ht2);
                                }
                                else
                                {
                                    Hashtable ht2 = new Hashtable();
                                    ht2.Clear();
                                    ht2.Add("@ItemCode", str.Trim());
                                    ht2.Add("@ForClassId", Convert.ToInt32(chklClass.Items[index].Value));
                                    clsDal.ExecuteScalar("ACTS_InsUpdtItemClass", ht2);
                                }
                            }
                        }
                    }
                }
            }
            ViewState["Item"] = string.Empty;
            ViewState["ItemImg"] = string.Empty;
            ViewState["BrochureImg"] = string.Empty;
            btnCancel.Enabled = true;
            return str;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('" + ex.Message.ToString() + "');", true);
            return "NO";
        }
    }

    private string IsAvailTransaction(int itemcode)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable ht = new Hashtable();
        ht.Clear();
        ht.Add("@ItemCode", itemcode);
        return clsDal.ExecuteScalar("SI_CheckTranStatus", ht);
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Response.Redirect("ItemMasterList.aspx");
    }

    private void FillData()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTableQry1 = clsDal.GetDataTableQry("SELECT * FROM SI_ItemMaster WHERE ItemCode=" + Request.QueryString["PId"].ToString());
        string str = clsDal.ExecuteScalarQry("Select ParentCatId from dbo.SI_CategoryMaster where CatId=" + dataTableQry1.Rows[0]["CatId"].ToString());
        if (dataTableQry1.Rows[0]["CatId"].ToString() == "1" || str == "1")
        {
            rbtBooks.Checked = true;
            rbtBooks_CheckedChanged(rbtBooks, EventArgs.Empty);
            DataTable dataTable2 = new DataTable();
            DataTable dataTableQry2 = clsDal.GetDataTableQry("Select ForClassId,streamid from dbo.SI_ItemMaster_Class where ItemCode=" + Request.QueryString["PId"].ToString());
            if (dataTableQry2.Rows.Count > 0)
            {
                ddlStream.SelectedValue = dataTableQry2.Rows[0]["streamid"].ToString();
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry2.Rows)
                {
                    for (int index = 0; index <= chklClass.Items.Count - 1; ++index)
                    {
                        if (chklClass.Items[index].Value == row["ForClassId"].ToString())
                            chklClass.Items[index].Selected = true;
                    }
                }
            }
        }
        else
        {
            rbtOther.Checked = true;
            rbtOther_CheckedChanged(rbtOther, EventArgs.Empty);
        }
        drpCat.SelectedValue = dataTableQry1.Rows[0]["CatId"].ToString();
        drpBrand.SelectedValue = dataTableQry1.Rows[0]["BrandId"].ToString().Trim();
        txtBarCode.Text = dataTableQry1.Rows[0]["ItemBarCode"].ToString().Trim();
        txtItemDesc.Text = dataTableQry1.Rows[0]["ItemDesc"].ToString().Trim();
        txtItemName.Text = dataTableQry1.Rows[0]["ItemName"].ToString().Trim();
        drpMeasureUnit.SelectedValue = dataTableQry1.Rows[0]["MesuringUnit"].ToString().Trim();
        txtRol.Text = dataTableQry1.Rows[0]["ROL"].ToString();
        drpTax.SelectedValue = dataTableQry1.Rows[0]["ApplicableTaxId"].ToString().Trim();
        if (dataTableQry1.Rows[0]["ActiveStatus"].ToString() == "True")
        {
            rbtnStatusNo.Checked = false;
            rbtnStatusYes.Checked = true;
        }
        else
        {
            rbtnStatusNo.Checked = true;
            rbtnStatusYes.Checked = false;
        }
        if (dataTableQry1.Rows[0]["IsCapital"].ToString() == "True")
        {
            rbtnCapitalNo.Checked = false;
            rbtnCapitalYes.Checked = true;
        }
        else
        {
            rbtnCapitalNo.Checked = true;
            rbtnCapitalYes.Checked = false;
        }
        if (dataTableQry1.Rows[0]["IsConsumable"].ToString() == "True")
        {
            rbtnConsumeNo.Checked = false;
            rbtnConsumeYes.Checked = true;
        }
        else
        {
            rbtnConsumeNo.Checked = true;
            rbtnConsumeYes.Checked = false;
        }
        if (dataTableQry1.Rows[0]["IsSalable"].ToString() == "True")
        {
            rbtnSaleNo.Checked = false;
            rbtnSaleYes.Checked = true;
        }
        else
        {
            rbtnSaleNo.Checked = true;
            rbtnSaleYes.Checked = false;
        }
        if (dataTableQry1.Rows[0]["DepreciationPercent"].ToString() != "")
            txtDepreciation.Text = dataTableQry1.Rows[0]["DepreciationPercent"].ToString();
        if (dataTableQry1.Rows[0]["IsCapital"].ToString() == "1")
        {
            rbtnCapitalYes.Checked = true;
            rbtnCapitalNo.Checked = false;
        }
        else
        {
            rbtnCapitalYes.Checked = false;
            rbtnCapitalNo.Checked = true;
        }
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Masters/MastersHome.aspx");
    }

    protected void btnCancel_Click1(object sender, EventArgs e)
    {
        ClearAll();
    }

    private void ClearAll()
    {
        drpCat.SelectedIndex = 0;
        drpBrand.SelectedIndex = 0;
        txtBarCode.Text = string.Empty;
        txtItemDesc.Text = string.Empty;
        txtItemName.Text = string.Empty;
        drpMeasureUnit.SelectedIndex = 0;
        drpTax.SelectedIndex = 0;
        rbtnCapitalNo.Checked = true;
        rbtnCapitalYes.Checked = false;
        rbtnSaleNo.Checked = false;
        rbtnSaleYes.Checked = true;
        rbtnConsumeNo.Checked = true;
        rbtnConsumeYes.Checked = false;
        txtRol.Text = string.Empty;
        txtDepreciation.Text = string.Empty;
        if (chklClass.Items.Count <= 0)
            return;
        chklClass.SelectedIndex = -1;
        
    }

    protected void rbtnSaleYes_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnSaleYes.Checked.Equals(true))
        {
            rbtnConsumeNo.Checked = true;
            rbtnConsumeYes.Checked = false;
            rbtnCapitalNo.Checked = true;
            rbtnCapitalYes.Checked = false;
        }
        else
        {
            rbtnConsumeNo.Checked = false;
            rbtnConsumeYes.Checked = true;
            rbtnCapitalNo.Checked = true;
            rbtnCapitalYes.Checked = false;
        }
    }

    protected void rbtnConsumeYes_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnConsumeYes.Checked.Equals(true))
        {
            rbtnSaleNo.Checked = true;
            rbtnSaleYes.Checked = false;
            rbtnCapitalNo.Checked = true;
            rbtnCapitalYes.Checked = false;
        }
        else
        {
            rbtnCapitalNo.Checked = false;
            rbtnCapitalYes.Checked = true;
        }
    }

    protected void rbtnCapitalYes_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnCapitalYes.Checked.Equals(true))
        {
            rbtnSaleNo.Checked = true;
            rbtnSaleYes.Checked = false;
            rbtnConsumeNo.Checked = true;
            rbtnConsumeYes.Checked = false;
        }
        else
        {
            rbtnConsumeNo.Checked = false;
            rbtnConsumeYes.Checked = true;
        }
    }

    protected void rbtBooks_CheckedChanged(object sender, EventArgs e)
    {
        bindCategory();
        chklClass.Enabled = true;
        chklClass.Height = (Unit)225;
        bindClass();
        BindStream();
    }

    protected void rbtOther_CheckedChanged(object sender, EventArgs e)
    {
        bindCategoryOthers();
        chklClass.Items.Clear();
        chklClass.Enabled = false;
        chklClass.Height = (Unit)20;
    }
}