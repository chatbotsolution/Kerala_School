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

public partial class Accounts_ItemReturn : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Request.QueryString["PId"] == null)
            return;
        long int64 = Convert.ToInt64(Request.QueryString["PId"].ToString());
        ViewState["PurId"] = (object)int64.ToString();
        ViewState["POID"] = (object)Request.QueryString["POID"].ToString();
        bindGVStockDetail(int64);
        dtpReturnDt.SetDateValue(DateTime.Now);
    }

    private void bindGVStockDetail(long pid)
    {
        ht.Clear();
        ht.Add((object)"@PurchaseId", (object)pid);
        ht.Add((object)"@NoOfChar", (object)8);
        dt = obj.GetDataTable("ACTS_StockDtls", ht);
        if (dt.Rows.Count > 0)
        {
            dv1.Visible = true;
            dv2.Visible = true;
        }
        else
        {
            dv1.Visible = false;
            dv2.Visible = false;
        }
        gvStockDtls.DataSource = (object)dt;
        gvStockDtls.DataBind();
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        DataTable dataTableQry = new clsDAL().GetDataTableQry("select top(1) FY,StartDate,EndDate from dbo.ACTS_FinancialYear where (IsFinalized is null or IsFinalized=0) order by FY_Id desc");
        if (dataTableQry.Rows.Count > 0)
        {
            if (Convert.ToDateTime(dtpReturnDt.GetDateValue().ToString("dd-MMM-yyyy")) >= Convert.ToDateTime(dataTableQry.Rows[0]["StartDate"]) && Convert.ToDateTime(dtpReturnDt.GetDateValue().ToString("dd-MMM-yyyy")) <= Convert.ToDateTime(dataTableQry.Rows[0]["EndDate"]))
            {
                SqlConnection sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = ConfigurationManager.AppSettings["conString"];
                SqlTransaction sqlTransaction = (SqlTransaction)null;
                try
                {
                    string qty = CheckReceiveQty();
                    if (qty != "")
                    {
                        ScriptManager.RegisterClientScriptBlock((Control)btnReturn, btnReturn.GetType(), "ShowMessage", "alert('" + qty + "');", true);
                    }
                    else
                    {
                        if (sqlConnection.State == ConnectionState.Closed)
                            sqlConnection.Open();
                        sqlTransaction = sqlConnection.BeginTransaction();
                        SqlCommand sqlCommand = new SqlCommand();
                        sqlCommand.Connection = sqlConnection;
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.CommandText = "ACTS_InsReturnDoc";
                        sqlCommand.Transaction = sqlTransaction;
                        SqlParameter sqlParameter1 = new SqlParameter("@PurchaseId", (object)long.Parse(ViewState["PurId"].ToString()));
                        SqlParameter sqlParameter2 = new SqlParameter("@RetInvNo", (object)txtreturnInvNo.Text.Trim());
                        SqlParameter sqlParameter3 = new SqlParameter("@RetDate", (object)dtpReturnDt.GetDateValue());
                        SqlParameter sqlParameter4 = new SqlParameter("@Remarks", (object)txtRemarks.Text.Trim());
                        SqlParameter sqlParameter5 = new SqlParameter("@UserId", (object)int.Parse(Session["User_Id"].ToString()));
                        SqlParameter sqlParameter6 = new SqlParameter("@SchoolId", (object)int.Parse(Session["SchoolId"].ToString()));
                        SqlParameter sqlParameter7 = new SqlParameter("@PurchaseRetId", SqlDbType.Int);
                        sqlParameter7.Direction = ParameterDirection.Output;
                        SqlParameter sqlParameter8 = new SqlParameter("@msg", SqlDbType.VarChar, 20);
                        sqlParameter8.Direction = ParameterDirection.Output;
                        sqlCommand.Parameters.Add(sqlParameter1);
                        sqlCommand.Parameters.Add(sqlParameter2);
                        sqlCommand.Parameters.Add(sqlParameter3);
                        sqlCommand.Parameters.Add(sqlParameter4);
                        sqlCommand.Parameters.Add(sqlParameter5);
                        sqlCommand.Parameters.Add(sqlParameter6);
                        sqlCommand.Parameters.Add(sqlParameter7);
                        sqlCommand.Parameters.Add(sqlParameter8);
                        sqlCommand.ExecuteNonQuery();
                        if (sqlParameter8.Value.ToString().Trim() == "Return Invoice Exist")
                        {
                            lblMsg.Text = sqlParameter8.Value.ToString();
                            lblMsg.ForeColor = Color.Red;
                        }
                        else
                        {
                            int int32 = Convert.ToInt32(sqlParameter7.Value);
                            double num = 0.0;
                            foreach (GridViewRow row in gvStockDtls.Rows)
                            {
                                HiddenField control1 = (HiddenField)row.FindControl("hdnPurDtlId");
                                HiddenField control2 = (HiddenField)row.FindControl("hdnItemCode");
                                HiddenField control3 = (HiddenField)row.FindControl("hdnUniPurPrice");
                                HiddenField control4 = (HiddenField)row.FindControl("hfLandingCost");
                                TextBox control5 = (TextBox)row.FindControl("txtReturnQty");
                                if (Convert.ToDecimal(control5.Text) > new Decimal(0))
                                {
                                    num += double.Parse(control4.Value) * (double)float.Parse(control5.Text.Trim());
                                    sqlCommand.CommandType = CommandType.StoredProcedure;
                                    sqlCommand.CommandText = "ACTS_StockReturn";
                                    sqlCommand.Parameters.Clear();
                                    SqlParameter sqlParameter9 = new SqlParameter("@POID", (object)int.Parse(ViewState["POID"].ToString()));
                                    SqlParameter sqlParameter10 = new SqlParameter("@PurchaseDetailId", (object)long.Parse(control1.Value.ToString()));
                                    SqlParameter sqlParameter11 = new SqlParameter("@ItemCode", (object)int.Parse(control2.Value.ToString()));
                                    SqlParameter sqlParameter12 = new SqlParameter("@ReturnQty", (object)float.Parse(control5.Text.Trim()));
                                    SqlParameter sqlParameter13 = new SqlParameter("@PurchaseRetId", (object)int32);
                                    sqlCommand.Parameters.Add(sqlParameter9);
                                    sqlCommand.Parameters.Add(sqlParameter10);
                                    sqlCommand.Parameters.Add(sqlParameter11);
                                    sqlCommand.Parameters.Add(sqlParameter12);
                                    sqlCommand.Parameters.Add(sqlParameter13);
                                    sqlCommand.ExecuteNonQuery();
                                }
                            }
                            sqlCommand.CommandType = CommandType.StoredProcedure;
                            sqlCommand.CommandText = "ACTS_StockReturnGenLedger";
                            sqlCommand.Parameters.Clear();
                            SqlParameter sqlParameter14 = new SqlParameter("@InvNo", (object)txtreturnInvNo.Text.Trim());
                            SqlParameter sqlParameter15 = new SqlParameter("@PurchaseRetid", (object)int32);
                            SqlParameter sqlParameter16 = new SqlParameter("@PurchaseId", (object)long.Parse(ViewState["PurId"].ToString()));
                            SqlParameter sqlParameter17 = new SqlParameter("@TransAmt", (object)num);
                            SqlParameter sqlParameter18 = new SqlParameter("@UserId", (object)int.Parse(Session["User_Id"].ToString()));
                            SqlParameter sqlParameter19 = new SqlParameter("@SchoolId", (object)int.Parse(Session["SchoolId"].ToString()));
                            SqlParameter sqlParameter20 = new SqlParameter("@RetDate", (object)dtpReturnDt.GetDateValue());
                            SqlParameter sqlParameter21 = new SqlParameter("@desc", (object)txtRemarks.Text.Trim());
                            sqlCommand.Parameters.Add(sqlParameter14);
                            sqlCommand.Parameters.Add(sqlParameter15);
                            sqlCommand.Parameters.Add(sqlParameter16);
                            sqlCommand.Parameters.Add(sqlParameter17);
                            sqlCommand.Parameters.Add(sqlParameter18);
                            sqlCommand.Parameters.Add(sqlParameter19);
                            sqlCommand.Parameters.Add(sqlParameter20);
                            sqlCommand.Parameters.Add(sqlParameter21);
                            sqlCommand.ExecuteNonQuery();
                        }
                        sqlTransaction.Commit();
                        sqlTransaction.Dispose();
                        sqlConnection.Dispose();
                        if (sqlParameter8.Value.ToString().Trim() != "Return Invoice Exist")
                        {
                            lblMsg.Text = "Stock Returned Successfully";
                            lblMsg.ForeColor = Color.Green;
                        }
                        Clear();
                        bindGVStockDetail(long.Parse(ViewState["PurId"].ToString()));
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Transaction Failed ! Try Again.";
                    lblMsg.ForeColor = Color.Red;
                    sqlTransaction.Rollback();
                }
            }
            else
            {
                lblMsg.Text = "Transaction Date not in the current financial year!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
        {
            lblMsg.Text = "No running Financial available !!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private string CheckReceiveQty()
    {
        string str = "";
        foreach (GridViewRow row in gvStockDtls.Rows)
        {
            TextBox control1 = (TextBox)row.FindControl("txtReturnQty");
            HiddenField control2 = (HiddenField)row.FindControl("hdnRcvQty");
            if (control1.Text.Trim() == "")
            {
                Label control3 = (Label)row.FindControl("lblItemName");
                Label control4 = (Label)row.FindControl("lblMeasuringUnit");
                str = str + control3.Text + " can not be blank \\n";
            }
            else if ((double)float.Parse(control1.Text.Trim()) > (double)float.Parse(control2.Value.Trim()))
            {
                Label control3 = (Label)row.FindControl("lblItemName");
                Label control4 = (Label)row.FindControl("lblMeasuringUnit");
                str = str + control3.Text + " Maximum Return Capacity " + control2.Value + " " + control4.Text + "\\n";
            }
        }
        return str;
    }

    private void Clear()
    {
        txtReturnDt.Text = "";
        txtreturnInvNo.Text = "";
        txtRemarks.Text = "";
        foreach (Control row in gvStockDtls.Rows)
            ((TextBox)row.FindControl("txtReturnQty")).Text = "";
    }
}