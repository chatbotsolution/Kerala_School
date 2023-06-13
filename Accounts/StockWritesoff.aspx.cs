using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_StockWritesoff : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (!Page.IsPostBack)
            {
                bindDropDown(drpBrand, "Select BrandId,BrandName from dbo.SI_BrandMaster", "BrandName", "BrandId");
                bindDropDown(ddlCategory, "Proc_DrpGetCat", "CatName", "CatId");
                btnWritesOff.Visible = false;
            }
        }
        else
            Response.Redirect("../Login.aspx");
        lblMsg.Text = "";
        lblMsg2.Text = "";
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
        drp.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindItem();
    }

    private void BindItem()
    {
        obj = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (ddlCategory.SelectedIndex > 0)
        {
            if (ddlCategory.SelectedValue == "1" || obj.ExecuteScalarQry("Select ParentCatId from dbo.SI_CategoryMaster where CatId=" + ddlCategory.SelectedValue.ToString()).Trim() == "1")
            {
                drpClass.Enabled = true;
                bindDropDown(drpClass, "select ClassID, ClassName from dbo.PS_ClassMaster order by ClassID", "ClassName", "ClassID");
            }
            else
            {
                drpClass.Items.Clear();
                drpClass.Enabled = false;
            }
        }
        else
        {
            drpClass.Items.Clear();
            drpClass.Enabled = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    private void ClearAll()
    {
        ddlCategory.SelectedIndex = 0;
        drpClass.Items.Clear();
        drpBrand.SelectedIndex = 1;
        btnWritesOff.Visible = false;
        gvItemList.Visible = false;
        txtAuthorizedBy.Text = "";
        txtReason.Text = "";
    }

    protected void btnWritesOff_Click(object sender, EventArgs e)
    {
        SqlConnection sqlConnection = new SqlConnection();
        sqlConnection.ConnectionString = ConfigurationManager.AppSettings["conString"];
        SqlTransaction sqlTransaction = (SqlTransaction)null;
        try
        {
            if (Convert.ToInt32(obj.ExecuteScalarQry("select count(*) from dbo.ACTS_FinancialYear where '" + dtpWritesOff.GetDateValue().ToString("dd MMM yyyy") + "'>=StartDate and '" + dtpWritesOff.GetDateValue().ToString("dd MMM yyyy") + "'<=EndDate and (IsFinalized is null or IsFinalized=0)").Trim()) > 0)
            {
                string str = CheckWritesOffQty();
                if (str != "")
                {
                    lblMsg.Text = str;
                    lblMsg.ForeColor = Color.Red;
                    lblMsg2.Text = str;
                    lblMsg2.ForeColor = Color.Red;
                }
                else
                {
                    int num1 = 0;
                    if (sqlConnection.State == ConnectionState.Closed)
                        sqlConnection.Open();
                    sqlTransaction = sqlConnection.BeginTransaction();
                    SqlCommand sqlCommand = new SqlCommand();
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "ACTS_InsWritesOff";
                    sqlCommand.Transaction = sqlTransaction;
                    foreach (GridViewRow row in gvItemList.Rows)
                    {
                        TextBox control1 = (TextBox)row.FindControl("txtWritesOffQty");
                        HiddenField control2 = (HiddenField)row.FindControl("hdnPurDtlId");
                        HiddenField control3 = (HiddenField)row.FindControl("hfItemCode");
                        if (control1.Text.Trim() != "" && Convert.ToInt32(control1.Text.Trim()) != 0)
                        {
                            Label control4 = (Label)row.FindControl("lblAvlQty");
                            Label control5 = (Label)row.FindControl("lblPurPrice");
                            float num2 = float.Parse(control4.Text.Trim()) - float.Parse(control1.Text.Trim());
                            num1 = 1;
                            sqlCommand.Parameters.Clear();
                            if (txtWritesOffDt.Text.Trim() != "")
                            {
                                SqlParameter sqlParameter = new SqlParameter("@WritesOffDate", dtpWritesOff.GetDateValue().ToString("dd MMM yyyy"));
                                sqlCommand.Parameters.Add(sqlParameter);
                            }
                            SqlParameter sqlParameter1 = new SqlParameter("@ItemCode", int.Parse(control3.Value));
                            SqlParameter sqlParameter2 = new SqlParameter("@Qty", float.Parse(control1.Text.Trim()));
                            SqlParameter sqlParameter3 = new SqlParameter("@Reason", txtReason.Text.Trim());
                            SqlParameter sqlParameter4 = new SqlParameter("@AuthorizedBy", txtAuthorizedBy.Text.Trim());
                            SqlParameter sqlParameter5 = new SqlParameter("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
                            SqlParameter sqlParameter6 = new SqlParameter("@UserId", int.Parse(Session["User_Id"].ToString()));
                            SqlParameter sqlParameter7 = new SqlParameter("@PurchaseDetailId", long.Parse(control2.Value));
                            SqlParameter sqlParameter8 = new SqlParameter("@BalQty", num2);
                            SqlParameter sqlParameter9 = new SqlParameter("@TotAmount", (Convert.ToDecimal(control1.Text.Trim()) * Convert.ToDecimal(control5.Text.Trim())));
                            sqlCommand.Parameters.Add(sqlParameter1);
                            sqlCommand.Parameters.Add(sqlParameter2);
                            sqlCommand.Parameters.Add(sqlParameter3);
                            sqlCommand.Parameters.Add(sqlParameter4);
                            sqlCommand.Parameters.Add(sqlParameter5);
                            sqlCommand.Parameters.Add(sqlParameter6);
                            sqlCommand.Parameters.Add(sqlParameter7);
                            sqlCommand.Parameters.Add(sqlParameter8);
                            sqlCommand.Parameters.Add(sqlParameter9);
                            sqlCommand.ExecuteNonQuery();
                        }
                    }
                    if (num1 == 0)
                    {
                        lblMsg.Text = "Enter Quantity For WritesOff";
                        lblMsg.ForeColor = Color.Red;
                        lblMsg2.Text = "Enter Quantity For WritesOff";
                        lblMsg2.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblMsg.Text = "WritesOff Applied Successfully";
                        lblMsg.ForeColor = Color.Green;
                        lblMsg2.Text = "WritesOff Applied Successfully";
                        lblMsg2.ForeColor = Color.Green;
                        ClearAll();
                    }
                    sqlTransaction.Commit();
                    sqlTransaction.Dispose();
                    sqlConnection.Dispose();
                }
            }
            else
            {
                lblMsg.Text = "Transaction Date not in running Financial Year!!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again .";
            lblMsg.ForeColor = Color.Red;
            sqlTransaction.Rollback();
        }
    }

    private string CheckWritesOffQty()
    {
        string str = "";
        foreach (GridViewRow row in gvItemList.Rows)
        {
            TextBox control1 = (TextBox)row.FindControl("txtWritesOffQty");
            Label control2 = (Label)row.FindControl("lblAvlQty");
            if (control1.Text.Trim() != "" && (double)float.Parse(control1.Text.Trim()) > (double)float.Parse(control2.Text.Trim()))
            {
                str = "WritesOff Qty Should not be greater than Avl Qty";
                break;
            }
        }
        return str;
    }

    protected void cdrCalendar_SelectionChanged(object sender, EventArgs e)
    {
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        Response.Redirect("StockWritesoffList.aspx");
    }

    private void FillItems()
    {
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (ddlCategory.SelectedIndex > 0)
            hashtable.Add("@CatId", ddlCategory.SelectedValue.ToString().Trim());
        if (drpBrand.SelectedIndex > 0)
            hashtable.Add("@BrandId", drpBrand.SelectedValue.ToString().Trim());
        if (drpClass.Enabled)
            hashtable.Add("@ClassId", drpClass.SelectedValue.ToString().Trim());
        hashtable.Add("@WrOffDate", dtpWritesOff.GetDateValue().ToString("dd MMM yyyy"));
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        DataTable dataTable2 = obj.GetDataTable("ACTS_GetWritesOffItems", hashtable);
        if (dataTable2.Rows.Count > 0 && dataTable2.Rows[0][0].ToString().Trim().ToUpper() != "INVALID")
        {
            btnWritesOff.Visible = true;
            gvItemList.Visible = true;
            gvItemList.DataSource = dataTable2;
            gvItemList.DataBind();
        }
        else
        {
            if (dataTable2.Rows.Count > 0 && dataTable2.Rows[0][0].ToString().Trim().ToUpper() == "INVALID")
            {
                lblMsg.Text = "Selected date not in a running financial year!!";
                lblMsg.ForeColor = Color.Red;
                lblMsg2.Text = "Selected date not in a running financial year!!";
                lblMsg2.ForeColor = Color.Red;
            }
            btnWritesOff.Visible = false;
            gvItemList.Visible = true;
            gvItemList.DataSource = null;
            gvItemList.DataBind();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillItems();
    }

    protected void dtpWritesOff_SelectionChanged(object sender, EventArgs e)
    {
        FillItems();
    }
}