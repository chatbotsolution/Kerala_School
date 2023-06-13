using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_JournalVoucher : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Decimal totCredit;
    private Decimal totDebit;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (!Page.IsPostBack)
        {
            BindDropdown(drpAccGroup, "SELECT AG_Code,AG_Name from dbo.Acts_AccountGroups ORDER BY AG_Name ", "AG_Name", "AG_Code");
            CreateTempTbl();
            txtTransactionDate.Focus();
        }
        lblMsg.Text = "";
    }

    private void BindDropdown(DropDownList drp, string Qry, string Text, string Value)
    {
        DataTable dataTableQry = obj.GetDataTableQry(Qry.Trim());
        drp.DataSource = dataTableQry.DefaultView;
        drp.DataTextField = Text;
        drp.DataValueField = Value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    private void CreateTempTbl()
    {
        DataTable dataTable = new DataTable();
        dataTable.Columns.Add(new DataColumn("AccHdId", typeof(int)));
        dataTable.Columns.Add(new DataColumn("AccName", typeof(string)));
        dataTable.Columns.Add(new DataColumn("CrDr", typeof(string)));
        dataTable.Columns.Add(new DataColumn("AmountDr", typeof(double)));
        dataTable.Columns.Add(new DataColumn("AmountCr", typeof(double)));
        dataTable.AcceptChanges();
        ViewState["Jrnl"] = dataTable;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!ValidateSave())
            return;
        SqlConnection sqlConnection = new SqlConnection();
        sqlConnection.ConnectionString = ConfigurationManager.AppSettings["ConString"];
        if (sqlConnection.State == ConnectionState.Closed)
            sqlConnection.Open();
        SqlTransaction sqlTransaction = sqlConnection.BeginTransaction();
        try
        {
            DataTable dataTableQry = obj.GetDataTableQry("select FY,StartDate,EndDate from dbo.ACTS_FinancialYear where '" + dtpTransDt.GetDateValue().ToString("dd MMM yyyy") + "' between StartDate and EndDate and (IsFinalized=0 or IsFinalized is null) order by FY_Id desc");
            if (dataTableQry.Rows.Count > 0)
            {
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Transaction = sqlTransaction;
                string str = "";
                SqlParameter sqlParameter1 = new SqlParameter();
                SqlParameter sqlParameter2 = new SqlParameter();
                SqlParameter sqlParameter3 = new SqlParameter();
                SqlParameter sqlParameter4 = new SqlParameter();
                SqlParameter sqlParameter5 = new SqlParameter();
                SqlParameter sqlParameter6 = new SqlParameter();
                SqlParameter sqlParameter7 = new SqlParameter();
                SqlParameter sqlParameter8 = new SqlParameter();
                SqlParameter sqlParameter9 = new SqlParameter();
                SqlParameter sqlParameter10 = new SqlParameter();
                SqlParameter sqlParameter11 = new SqlParameter();
                SqlParameter sqlParameter12 = new SqlParameter();
                SqlParameter sqlParameter13 = new SqlParameter();
                SqlParameter sqlParameter14 = new SqlParameter();
                int num1 = 0;
                Decimal num2 = new Decimal(0);
                foreach (Control row in grdJrnl.Rows)
                {
                    Label control = (Label)row.FindControl("lblDrAmount");
                    num2 += Convert.ToDecimal(control.Text.Trim());
                }
                foreach (GridViewRow row in grdJrnl.Rows)
                {
                    Label control1 = (Label)row.FindControl("lblCrAmount");
                    Label control2 = (Label)row.FindControl("lblDrAmount");
                    Label control3 = (Label)row.FindControl("lblCrDr");
                    HiddenField control4 = (HiddenField)row.FindControl("hfAccId");
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = "ACTS_InsertJournalVoucher";
                    sqlCommand.Parameters.Clear();
                    sqlParameter1.ParameterName = "@FY";
                    sqlParameter1.Value = dataTableQry.Rows[0]["FY"].ToString();
                    sqlParameter2.ParameterName = "@TransDate";
                    sqlParameter2.Value = dtpTransDt.GetDateValue().ToString("dd MMM yyyy");
                    sqlParameter2.SqlDbType = SqlDbType.DateTime;
                    sqlParameter3.ParameterName = "@AccHd";
                    sqlParameter3.Value = control4.Value.ToString().Trim();
                    sqlParameter5.ParameterName = "@Desc";
                    sqlParameter5.Value = txtDetails.Text.Trim();
                    sqlParameter7.ParameterName = "@CrDr";
                    sqlParameter4.ParameterName = "@Amount";
                    if (control3.Text.ToString().Trim() == "Credit")
                    {
                        sqlParameter7.Value = "CR";
                        sqlParameter4.Value = control1.Text.ToString().Trim();
                    }
                    else
                    {
                        sqlParameter7.Value = "DR";
                        sqlParameter4.Value = control2.Text.ToString().Trim();
                    }
                    if (str.Trim() != "")
                    {
                        sqlParameter9.ParameterName = "@JournalId";
                        sqlParameter9.Value = str.Trim();
                    }
                    sqlParameter12.ParameterName = "@Totalamt";
                    sqlParameter12.Value = num2;
                    sqlParameter10.ParameterName = "@UserId";
                    sqlParameter10.Value = Session["User_Id"];
                    sqlParameter11.ParameterName = "@SchoolId";
                    sqlParameter11.Value = Session["SchoolId"];
                    SqlParameter sqlParameter15 = new SqlParameter("@msg", SqlDbType.VarChar, 100);
                    sqlParameter15.Direction = ParameterDirection.Output;
                    SqlParameter sqlParameter16 = new SqlParameter("@JrId", SqlDbType.BigInt);
                    sqlParameter16.Direction = ParameterDirection.Output;
                    sqlCommand.Parameters.Add(sqlParameter1);
                    sqlCommand.Parameters.Add(sqlParameter2);
                    sqlCommand.Parameters.Add(sqlParameter3);
                    sqlCommand.Parameters.Add(sqlParameter4);
                    sqlCommand.Parameters.Add(sqlParameter5);
                    sqlCommand.Parameters.Add(sqlParameter7);
                    if (str.Trim() != "")
                        sqlCommand.Parameters.Add(sqlParameter9);
                    sqlCommand.Parameters.Add(sqlParameter10);
                    sqlCommand.Parameters.Add(sqlParameter11);
                    sqlCommand.Parameters.Add(sqlParameter12);
                    sqlCommand.Parameters.Add(sqlParameter15);
                    sqlCommand.Parameters.Add(sqlParameter16);
                    sqlCommand.ExecuteNonQuery();
                    if (sqlParameter15.Value.ToString().Trim().ToUpper() == "S")
                    {
                        ++num1;
                        str = sqlParameter16.Value.ToString().Trim();
                    }
                    else
                    {
                        sqlTransaction.Rollback();
                        break;
                    }
                }
                if (num1 == grdJrnl.Rows.Count)
                {
                    lblMsg.Text = "Data Saved Successfully!!";
                    lblMsg.ForeColor = Color.Green;
                    ClearAll();
                    ((Control)dtpTransDt).Focus();
                    sqlTransaction.Commit();
                }
                else
                {
                    sqlTransaction.Rollback();
                    lblMsg.Text = "Transaction Failed Please Try Again!!";
                    lblMsg.ForeColor = Color.Red;
                }
            }
            else
            {
                lblMsg.Text = "Transaction Date is not in the current financial year";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed Please Try Again!!";
            lblMsg.ForeColor = Color.Red;
            sqlTransaction.Rollback();
        }
        sqlTransaction.Dispose();
        sqlConnection.Dispose();
    }

    private bool ValidateSave()
    {
        bool flag = true;
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        foreach (GridViewRow row in grdJrnl.Rows)
        {
            Label control1 = (Label)row.FindControl("lblCrAmount");
            Label control2 = (Label)row.FindControl("lblDrAmount");
            num2 += Convert.ToDecimal(control2.Text.Trim());
            num1 += Convert.ToDecimal(control1.Text.Trim());
        }
        if (num2 != num1)
        {
            lblMsg.Text = "Credit and Debit amount not equal please check!!";
            lblMsg.ForeColor = Color.Red;
            flag = false;
        }
        return flag;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    private void ClearAll()
    {
        txtTransactionDate.Text = "";
        txtDetails.Text = "";
        drpAccGroup.SelectedIndex = 0;
        drpAccHead.Items.Clear();
        CreateTempTbl();
        grdJrnl.DataSource = null;
        grdJrnl.DataBind();
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("JournalVchrList.aspx");
    }

    protected void drpAccGroup_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpAccGroup.SelectedIndex > 0)
        {
            BindDropdown(drpAccHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads Where AG_Code=" + drpAccGroup.SelectedValue.Trim() + " ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
        }
        else
        {
            drpAccHead.Items.Clear();
            drpAccHead.DataSource = null;
            drpAccHead.DataBind();
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = (DataTable)ViewState["Jrnl"];
        DataRow row = dataTable2.NewRow();
        row["AccHdId"] = Convert.ToInt32(drpAccHead.SelectedValue.ToString());
        row["AccName"] = drpAccHead.SelectedItem.Text.Trim().ToString();
        row["CrDr"] = drpType.SelectedValue.ToString().Trim();
        if (drpType.SelectedValue.ToString().Trim() == "Credit")
        {
            row["AmountCr"] = Convert.ToDouble(txtAmount.Text.ToString().Trim());
            row["AmountDr"] = 0;
        }
        else
        {
            row["AmountCr"] = 0;
            row["AmountDr"] = Convert.ToDouble(txtAmount.Text.ToString().Trim());
        }
        dataTable2.Rows.Add(row);
        ViewState["Jrnl"] = dataTable2;
        grdJrnl.DataSource = dataTable2;
        grdJrnl.DataBind();
        drpType.SelectedIndex = 0;
        drpAccGroup.SelectedIndex = 0;
        drpAccGroup_SelectedIndexChanged(drpAccGroup, EventArgs.Empty);
        txtAmount.Text = "";
    }

    protected void grdJrnl_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = (DataTable)ViewState["Jrnl"];
        dataTable2.Rows.RemoveAt(e.RowIndex);
        dataTable2.AcceptChanges();
        grdJrnl.DataSource = dataTable2;
        grdJrnl.DataBind();
        ViewState["Jrnl"] = dataTable2;
    }

    protected void grdJrnl_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label control1 = (Label)e.Row.FindControl("lblCrAmount");
            Label control2 = (Label)e.Row.FindControl("lblDrAmount");
            totCredit = totCredit + Decimal.Parse(control1.Text);
            totDebit = totDebit + Decimal.Parse(control2.Text);
        }
        else
        {
            if (e.Row.RowType != DataControlRowType.Footer)
                return;
            Label control1 = (Label)e.Row.FindControl("lblCrTot");
            Label control2 = (Label)e.Row.FindControl("lblDrTot");
            e.Row.Cells[0].Text = "Total: ";
            e.Row.Cells[0].ForeColor = Color.White;
            control2.Text = totDebit.ToString("0.00");
            control2.ForeColor = Color.White;
            control1.Text = totCredit.ToString("0.00");
            control1.ForeColor = Color.White;
        }
    }
}