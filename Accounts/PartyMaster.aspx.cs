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

public partial class Accounts_PartyMaster : System.Web.UI.Page
{
    private ScriptManager sm;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User_Id"] != null)
        {
            this.lblMsg.Text = string.Empty;
            if (this.Page.IsPostBack)
                return;
            this.ViewState["Party"] = (object)string.Empty;
            this.ViewState["PartyName"] = (object)string.Empty;
            this.FillDropdowns();
            this.drpCustomerType.Enabled = false;
            if (this.Request.QueryString["PId"] == null)
                return;
            this.ViewState["Party"] = (object)"Update";
            this.FillData();
            this.btnCancel.Enabled = false;
        }
        else
            this.Response.Redirect("../Login.aspx");
    }

    private void FillDropdowns()
    {
        clsStaticDropdowns clsStaticDropdowns = new clsStaticDropdowns();
        clsStaticDropdowns.FillCusomerCat(this.drpCustomerType);
        clsStaticDropdowns.FillPartyType(this.drpPartyType);
        this.FillAcGroup();
    }

    private void FillAcGroup()
    {
        this.drpAccGroups.Items.Clear();
        DataTable dataTable = new DataTable();
        this.drpAccGroups.DataSource = (object)new clsDAL().GetDataTableQry("select * from [ACTS_vwAccountGroup] where AG_Code in(11,12) or AG_Parent in(11,12)");
        this.drpAccGroups.DataTextField = "AG_Name";
        this.drpAccGroups.DataValueField = "AG_Code";
        this.drpAccGroups.DataBind();
        this.drpAccGroups.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    private void ClearAll()
    {
        this.txtPartyNm.Text = string.Empty;
        this.txtContactPrsn.Text = string.Empty;
        this.txtAddress.Text = string.Empty;
        this.txtCity.Text = string.Empty;
        this.txtState.Text = string.Empty;
        this.txtPinNo.Text = string.Empty;
        this.txtPhone.Text = string.Empty;
        this.txtMobile.Text = string.Empty;
        this.txtFax.Text = string.Empty;
        this.txtEmail.Text = string.Empty;
        this.drpAccGroups.SelectedIndex = 0;
        this.drpPartyType.SelectedIndex = 0;
        this.drpCustomerType.SelectedIndex = 0;
        this.txtCrLimit.Text = string.Empty;
        this.txtPanNo.Text = string.Empty;
        this.txtTinNo.Text = string.Empty;
        this.txtCstNo.Text = string.Empty;
        this.rbtnActive.Checked = true;
        this.rbtnInactive.Checked = false;
        this.txtStartDt.Text = string.Empty;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.ClearAll();
    }

    public DataTable InsertData()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        try
        {
            if (this.Request.QueryString["PId"] != null && this.ViewState["Party"].ToString() == "Update")
            {
                hashtable.Add((object)"@PartyId", (object)int.Parse(this.Request.QueryString["PId"].ToString()));
                hashtable.Add((object)"@Patry", this.ViewState["PartyName"]);
            }
            hashtable.Add((object)"@PartyName", (object)this.txtPartyNm.Text.ToString().Trim());
            hashtable.Add((object)"@ContactPerson", (object)this.txtContactPrsn.Text.ToString().Trim());
            if (this.txtAddress.Text.Trim() != "")
                hashtable.Add((object)"@Address", (object)this.txtAddress.Text.ToString().Trim());
            if (this.txtCity.Text.Trim() != "")
                hashtable.Add((object)"@City", (object)this.txtCity.Text.ToString().Trim());
            if (this.txtState.Text.Trim() != "")
                hashtable.Add((object)"@State", (object)this.txtState.Text.ToString().Trim());
            if (this.txtPinNo.Text.Trim() != "")
                hashtable.Add((object)"@PinCode", (object)this.txtPinNo.Text.ToString().Trim());
            if (this.txtPhone.Text.Trim() != "")
                hashtable.Add((object)"@Phone", (object)this.txtPhone.Text.ToString().Trim());
            if (this.txtMobile.Text.Trim() != "")
                hashtable.Add((object)"@Mobile", (object)this.txtMobile.Text.ToString().Trim());
            if (this.txtFax.Text.Trim() != "")
                hashtable.Add((object)"@FAX", (object)this.txtFax.Text.ToString().Trim());
            if (this.txtEmail.Text.Trim() != "")
                hashtable.Add((object)"@Email", (object)this.txtEmail.Text.ToString().Trim());
            hashtable.Add((object)"@AGCode", (object)int.Parse(this.drpAccGroups.SelectedValue.ToString().Trim()));
            if (this.drpPartyType.SelectedIndex > 0)
                hashtable.Add((object)"@PartyType", (object)this.drpPartyType.SelectedValue.ToString().Trim());
            if (this.drpCustomerType.SelectedIndex > 0)
                hashtable.Add((object)"@CustomerType", (object)this.drpCustomerType.SelectedValue.ToString().Trim());
            if (this.txtCrLimit.Text.Trim() != "")
                hashtable.Add((object)"@CreditLimit", (object)this.txtCrLimit.Text.ToString().Trim());
            if (this.txtPanNo.Text.Trim() != "")
                hashtable.Add((object)"@PAN_No", (object)this.txtPanNo.Text.ToString().Trim());
            if (this.txtTinNo.Text.Trim() != "")
                hashtable.Add((object)"@TIN_No", (object)this.txtTinNo.Text.ToString().Trim());
            if (this.txtCstNo.Text.Trim() != "")
                hashtable.Add((object)"@CST_No", (object)this.txtCstNo.Text.ToString().Trim());
            if (this.rbtnActive.Checked)
                hashtable.Add((object)"@IsActive", (object)1);
            else
                hashtable.Add((object)"@IsActive", (object)0);
            if (this.txtStartDt.Text.Trim() != "")
                hashtable.Add((object)"@StartDate", (object)this.dtpStartDate.GetDateValue().ToString());
            hashtable.Add((object)"@UserId", this.Session["User_Id"]);
            hashtable.Add((object)"@SchoolId", this.Session["SchoolId"]);
            if (this.Request.QueryString["PId"] != null && this.ViewState["Party"].ToString() == "Update")
            {
                this.ViewState["Party"] = (object)string.Empty;
                dataTable = clsDal.GetDataTable("ACTS_UpdateParty", hashtable);
                this.ViewState["PartyName"] = (object)string.Empty;
            }
            else
            {
                this.ViewState["Party"] = (object)string.Empty;
                dataTable = clsDal.GetDataTable("ACTS_InsertParty", hashtable);
            }
            this.btnCancel.Enabled = true;
            return dataTable;
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMassage", "alert('" + ex.Message.ToString() + "');", true);
            return dataTable;
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("PartyMasterList.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        if (this.InsertData().Rows.Count > 0)
        {
            this.lblMsg.Text = "Data Already Exist";
            this.lblMsg.ForeColor = Color.Red;
        }
        else
        {
            this.ClearAll();
            this.lblMsg.Text = "Data saved successfully";
            this.lblMsg.ForeColor = Color.Green;
        }
    }

    private void FillData()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = clsDal.GetDataTableQry("SELECT p.*,a.AG_Code FROM ACTS_PartyMaster p inner join dbo.Acts_AccountHeads a on a.AcctsHeadId=p.AcctsHeadId WHERE PartyId=" + this.Request.QueryString["PId"].ToString());
        this.txtPartyNm.Text = dataTableQry.Rows[0]["PartyName"].ToString();
        this.txtContactPrsn.Text = dataTableQry.Rows[0]["ContactPerson"].ToString();
        this.txtAddress.Text = dataTableQry.Rows[0]["Address"].ToString();
        this.txtCity.Text = dataTableQry.Rows[0]["City"].ToString();
        this.txtState.Text = dataTableQry.Rows[0]["State"].ToString();
        this.txtPinNo.Text = dataTableQry.Rows[0]["PinCode"].ToString();
        this.txtPhone.Text = dataTableQry.Rows[0]["Phone"].ToString();
        this.txtMobile.Text = dataTableQry.Rows[0]["Mobile"].ToString();
        this.txtFax.Text = dataTableQry.Rows[0]["FAX"].ToString();
        this.txtEmail.Text = dataTableQry.Rows[0]["Email"].ToString();
        this.drpAccGroups.SelectedValue = dataTableQry.Rows[0]["AG_Code"].ToString();
        if (dataTableQry.Rows[0]["PartyType"].ToString() != null)
            this.drpPartyType.SelectedValue = dataTableQry.Rows[0]["PartyType"].ToString();
        if (dataTableQry.Rows[0]["CustomerType"].ToString() != null)
            this.drpCustomerType.SelectedValue = dataTableQry.Rows[0]["CustomerType"].ToString();
        this.drpAccGroups_SelectedIndexChanged((object)this.drpPartyType, EventArgs.Empty);
        this.txtCrLimit.Text = dataTableQry.Rows[0]["CreditLimit"].ToString();
        this.txtPanNo.Text = dataTableQry.Rows[0]["PAN_No"].ToString();
        this.txtTinNo.Text = dataTableQry.Rows[0]["TIN_No"].ToString();
        this.txtCstNo.Text = dataTableQry.Rows[0]["CST_No"].ToString();
        if (dataTableQry.Rows[0]["IsActive"].ToString() == "0")
        {
            this.rbtnActive.Checked = false;
            this.rbtnInactive.Checked = true;
        }
        else
        {
            this.rbtnActive.Checked = true;
            this.rbtnInactive.Checked = false;
        }
        this.txtStartDt.Text = dataTableQry.Rows[0]["StartDate"].ToString();
        this.ViewState["PartyName"] = (object)dataTableQry.Rows[0]["PartyName"].ToString();
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("Welcome.aspx");
    }

    protected void drpPartyType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.drpPartyType.SelectedValue.ToString() == "Customer")
        {
            this.drpCustomerType.Enabled = true;
        }
        else
        {
            this.drpCustomerType.SelectedIndex = 0;
            this.drpCustomerType.Enabled = false;
        }
    }

    protected void btnCancel_Click1(object sender, EventArgs e)
    {
        this.ClearAll();
    }

    protected void drpAccGroups_SelectedIndexChanged(object sender, EventArgs e)
    {
        clsStaticDropdowns clsStaticDropdowns = new clsStaticDropdowns();
        this.drpPartyType.Items.Clear();
        clsStaticDropdowns.FillPartyType(this.drpPartyType);
        if (this.drpAccGroups.SelectedValue.ToString() == "11")
        {
            this.drpPartyType.SelectedIndex = 2;
            this.drpPartyType.Enabled = false;
            this.drpCustomerType.SelectedIndex = 0;
            this.drpCustomerType.Enabled = false;
        }
        else if (this.drpAccGroups.SelectedValue.ToString() == "12")
        {
            this.drpPartyType.SelectedIndex = 3;
            this.drpPartyType.Enabled = false;
            this.drpCustomerType.SelectedIndex = 0;
            this.drpCustomerType.Enabled = true;
        }
        else if (this.drpAccGroups.SelectedValue.ToString() == "22")
        {
            this.drpPartyType.SelectedIndex = 4;
            this.drpPartyType.Enabled = false;
            this.drpCustomerType.SelectedIndex = 0;
            this.drpCustomerType.Enabled = false;
        }
        else if (this.drpAccGroups.SelectedValue.ToString() == "15")
        {
            this.drpPartyType.SelectedIndex = 4;
            this.drpPartyType.Enabled = false;
        }
        else
        {
            this.drpPartyType.SelectedIndex = 1;
            this.drpPartyType.Enabled = false;
            this.drpCustomerType.SelectedIndex = 0;
            this.drpCustomerType.Enabled = false;
        }
        this.sm = this.Master.FindControl("ScriptManager1") as ScriptManager;
        this.sm.SetFocus((Control)this.drpAccGroups);
    }
}