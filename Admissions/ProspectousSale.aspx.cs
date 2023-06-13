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

public partial class Admissions_ProspectousSale : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    private DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblerr.Text = "";
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        btnPrint.Enabled = false;
        if (Page.IsPostBack)
            return;
        FillSession();
        FillProspType();
        ViewState["Sale"] = string.Empty;
        BindDropDown();
        MaxSlNo();
        getSalePrice();
        if (Request.QueryString["SId"] == null)
            return;
        FillData();
    }

    private void MaxSlNo()
    {
        clsDAL clsDal = new clsDAL();
        try
        {
            txtSlNo.Text = clsDal.ExecuteScalarQry("select max(cast(ProspectusSlNo as bigint))+1 from dbo.PS_ProspectusSale");
        }
        catch
        {
            txtSlNo.Text = string.Empty;
        }
    }

    private void getSalePrice()
    {
        clsDAL clsDal = new clsDAL();
        try
        {
            if (drpProspectusType.SelectedIndex <= 0)
                return;
            string str = clsDal.ExecuteScalarQry("select cast(UnitSalePrice as decimal (12,2)) as Price from dbo.PS_ProspectusStock where SaleId is null and UnitSalePrice is not null and SessionYr='" + drpSession.SelectedValue.Trim() + "' and ProspectusType ='" + drpProspectusType.SelectedValue.Trim() + "' order by TransDate desc");
            if (str.Trim() != "")
            {
                txtAmm.Text = str.Trim();
                txtAmm.Enabled = false;
                btnSave.Enabled = true;
                if (!(lblerr.Text.Trim().ToUpper() != "DATA SAVED SUCESSFULLY"))
                    return;
                lblerr.Text = "";
            }
            else
            {
                txtAmm.Text = "";
                txtAmm.Enabled = true;
                lblerr.Text = "Please Assign Prospectus Stock for Selected Session Year for selling!!";
                lblerr.ForeColor = Color.Red;
                btnSave.Enabled = false;
            }
        }
        catch
        {
            txtAmm.Text = "";
            txtAmm.Enabled = true;
        }
    }

    private void FillSession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select distinct SessionID,SessionYr from dbo.PS_FeeNormsNew order by sessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillProspType()
    {
        obj = new clsDAL();
        DataTable dataTable = new DataTable();
        drpProspectusType.DataSource = obj.GetDataTableQry("select ProspTypeId,ProspType from dbo.ProspTypeMaster order by ProspTypeId");
        drpProspectusType.DataTextField = "ProspType";
        drpProspectusType.DataValueField = "ProspType";
        drpProspectusType.DataBind();
        drpProspectusType.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    public void FillData()
    {
        obj = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("Ps_Sp_SelProspectusStockSale", new Hashtable()
    {
      {
         "@SaleId",
         Request.QueryString["SId"].ToString()
      }
    });
        drpSession.SelectedValue = dataTable2.Rows[0]["SessionYr"].ToString();
        txtSaledt.Text = Convert.ToDateTime(dataTable2.Rows[0]["SaleDate"].ToString()).ToString("dd-MMM-yyyy");
        txtName.Text = dataTable2.Rows[0]["StudentName"].ToString();
        drpForCls.SelectedValue = dataTable2.Rows[0]["ForClass"].ToString().Trim();
        txtSlNo.Text = dataTable2.Rows[0]["ProspectusSlNo"].ToString();
        txtAmm.Text = dataTable2.Rows[0]["Amount"].ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblerr.Text = "";
        clsDAL clsDal = new clsDAL();
        string str = "select count(*) from dbo.Acts_PaymentReceiptVoucher where TransDt > '" + txtSaledt.Text.Trim() + "' and (IsDeleted is null or IsDeleted=0)";
        if (clsDal.GetDataTableQry("select top(1) FY,StartDate,EndDate from dbo.ACTS_FinancialYear where (IsFinalized is null or IsFinalized=0) and fy=dbo.fuGetSessionYr('" + dtpSaledt.GetDateValue().ToString("dd MMM yyyy") + "') order by FY_Id desc").Rows.Count > 0)
        {
            if (clsDal.ExecuteScalarQry("select fy from [dbo].[ACTS_FinancialYear] where StartDate<='" + dtpSaledt.GetDateValue().ToString("dd MMM yyyy") + "' and enddate>='" + dtpSaledt.GetDateValue().ToString("dd MMM yyyy") + "'").ToString().Trim() != clsDal.ExecuteScalarQry("select fy from [dbo].[ACTS_FinancialYear] where StartDate<='" + DateTime.Now.ToString("dd MMM yyyy") + "' and enddate>='" + DateTime.Now.ToString("dd MMM yyyy") + "'").ToString().Trim())
            {
                lblerr.Text = "Receive date is not within the current Financial Year";
                lblerr.ForeColor = Color.Red;
            }
            else
                SaveData();
        }
        else
        {
            lblerr.Text = "No running Financial available For Given Trans Date !!";
            lblerr.ForeColor = Color.Red;
        }
    }

    private void SaveData()
    {
        string str = obj.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=3");
        if (str.ToString().Trim() == "")
        {
            ScriptManager.RegisterStartupScript((Page)this, GetType(), "alertMessage", "alert('No Account Head Set To Receive Prospectus Sale!! Please Set Account Head To Continue Sale!!'); window.location='" + Request.ApplicationPath + "/Masters/SetAccountHead.aspx'", true);
        }
        else
        {
            Hashtable hashtable = new Hashtable();
            if (Request.QueryString["SId"] != null)
                hashtable.Add("@SaleId", Request.QueryString["SId"].ToString());
            hashtable.Add("@SessionYr", drpSession.SelectedValue);
            hashtable.Add("@SaleDate", dtpSaledt.GetDateValue().ToString("MM/dd/yyyy"));
            hashtable.Add("@StudentName", txtName.Text);
            hashtable.Add("@ForClass", drpForCls.SelectedValue);
            hashtable.Add("@ProspType", drpProspectusType.SelectedValue);
            hashtable.Add("@ProspectusSlNo", txtSlNo.Text.Trim());
            hashtable.Add("@Amount", txtAmm.Text);
            hashtable.Add("@UserId", Session["User_Id"]);
            hashtable.Add("@SchoolID", Session["SchoolId"]);
            hashtable.Add("@ContactNo", txtContNo.Text.Trim());
            hashtable.Add("@CurrAddress", txtaddr.Text.Trim());
            hashtable.Add("@AccountHeadCr", str.ToString().Trim());
            DataTable dataTable = obj.GetDataTable("Ps_Sp_InsUpdPS_ProspectusSale", hashtable);
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0][0].ToString().Trim() == "dup")
            {
                lblerr.Text = "Prospectus Slno. already Exists";
                lblerr.ForeColor = Color.Red;
                btnPrint.Enabled = false;
            }
            else if (dataTable.Rows.Count > 0 && dataTable.Rows[0][0].ToString().Trim() == "NO")
            {
                lblerr.Text = "Receipt No. already Exists";
                lblerr.ForeColor = Color.Red;
                btnPrint.Enabled = false;
            }
            else
            {
                ViewState["MR_No"] = dataTable.Rows[0][0].ToString().Trim();
                btnPrint.Enabled = true;
                lblerr.Text = "Data Saved Sucessfully";
                lblerr.ForeColor = Color.Green;
                ClearAfterSave();
            }
        }
    }

    private void ClearAll()
    {
        txtAmm.Text = string.Empty;
        txtSaledt.Text = string.Empty;
        drpForCls.SelectedIndex = 0;
        drpSession.SelectedIndex = 0;
        drpProspectusType.SelectedIndex = 0;
        txtName.Text = string.Empty;
        txtSlNo.Text = string.Empty;
        lblerr.Text = "";
        getSalePrice();
    }

    private void ClearAfterSave()
    {
        txtAmm.Text = string.Empty;
        txtSaledt.Text = string.Empty;
        drpForCls.SelectedIndex = 0;
        drpSession.SelectedIndex = 0;
        drpProspectusType.SelectedIndex = 0;
        txtName.Text = string.Empty;
        txtSlNo.Text = string.Empty;
        MaxSlNo();
        txtaddr.Text = "";
        txtContNo.Text = "";
        getSalePrice();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProspectusSaleList.aspx");
    }

    private void BindDropDown()
    {
        drpForCls.DataSource = obj.GetDataTableQry("select ClassId,ClassName from ps_classmaster order by ClassID");
        drpForCls.DataTextField = "classname";
        drpForCls.DataValueField = "classid";
        drpForCls.DataBind();
        drpForCls.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('../Reports/rptReceiptProspSale.aspx?mrno=" + ViewState["MR_No"].ToString().Trim() + "');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        getSalePrice();
    }

    protected void drpProspectusType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpProspectusType.SelectedIndex <= 0)
            return;
        getSalePrice();
    }
}