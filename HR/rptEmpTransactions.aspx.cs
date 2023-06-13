using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_rptEmpTransactions : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (!ChkIsHRUsed())
        {
            Response.Redirect("HRHome.aspx?IsHRUsed=0");
        }
        else
        {
            bindDropDown(drpEmpName, "SELECT EmpId,SevName FROM dbo.HR_EmployeeMaster ORDER BY SevName", "SevName", "EmpId");
            DataTable dataTableQry = obj.GetDataTableQry("SELECT TOP 1 StartDate,EndDate+' 23:59:59' AS EndDate FROM ACTS_FinancialYear ORDER BY FY_Id DESC");
            drpMonth.SelectedValue = Convert.ToDateTime(dataTableQry.Rows[0]["StartDate"].ToString()).ToString("MMM");
            drpYear.SelectedValue = Convert.ToDateTime(dataTableQry.Rows[0]["EndDate"].ToString()).ToString("yyyy");
            Session["rptEmpTransactions"] = null;
            bindDrpYear();
        }
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void bindDrpYear()
    {
        int year = DateTime.Now.Year;
        drpYear.Items.Insert(0, new ListItem("- SELECT -", "0"));
        for (int index = 1; index < 11; ++index)
        {
            drpYear.Items.Insert(index, new ListItem(year.ToString(), year.ToString()));
            --year;
        }
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GenerateReport();
        btnSearch.Focus();
    }

    private void GenerateReport()
    {
        StringBuilder stringBuilder = new StringBuilder();
        DataSet dataSet = obj.GetDataSet("HR_rptEmpTransactions", new Hashtable()
    {
      {
         "@EmpId",
         drpEmpName.SelectedValue
      },
      {
         "@Month",
         drpMonth.SelectedValue
      },
      {
         "@Year",
         drpYear.SelectedValue
      }
    });
        DataTable table1 = dataSet.Tables[0];
        DataTable table2 = dataSet.Tables[1];
        DataTable table3 = dataSet.Tables[2];
        DataTable table4 = dataSet.Tables[3];
        DataTable table5 = dataSet.Tables[4];
        int num = table1.Rows.Count + table2.Rows.Count + table3.Rows.Count + table4.Rows.Count + table5.Rows.Count;
        stringBuilder.Append("<fieldset>");
        stringBuilder.Append("<legend style='color: #135e8a; font-size: small; font-weight: bold; font-family: Verdana'>");
        stringBuilder.Append("Transaction History of " + drpEmpName.SelectedItem.ToString().ToUpper() + ",&nbsp;" + drpMonth.SelectedItem.Text.ToUpper() + "-" + drpYear.SelectedValue + "</legend>");
        stringBuilder.Append("<table border='0' cellpadding='2' cellspacing='0'  width='100%' style='border:solid 1px black;border-bottom:0px;'>");
        if (num > 0)
        {
            if (table1.Rows.Count > 0)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='border-bottom:solid 1px black;font-size:14px;' align='left' colspan='3'><b><u>Salary</u> :-</b></td>");
                stringBuilder.Append("<tr>");
                foreach (DataRow row in (InternalDataCollectionBase)table1.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black; border-right:solid 1px black;' align='left'>Salary</td>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black; border-right:solid 1px black;' align='left'>Salary of " + drpMonth.SelectedItem.Text + "-" + drpYear.SelectedValue + "</td>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black;' align='right'>" + Convert.ToDecimal(row["Salary"]).ToString("0.00") + "</td>");
                    stringBuilder.Append("<tr>");
                }
            }
            if (table2.Rows.Count > 0)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='border-bottom:solid 1px black;font-size:14px;' align='left' colspan='3'><b><u>Allowances</u> :-</b></td>");
                stringBuilder.Append("<tr>");
                foreach (DataRow row in (InternalDataCollectionBase)table2.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black; border-right:solid 1px black;' align='left'>" + row["AllowanceName"] + "</td>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black; border-right:solid 1px black;' align='left'>" + row["AllowanceRemarks"] + "</td>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black;' align='right'>" + Convert.ToDecimal(row["AllowanceAmt"]).ToString("0.00") + "</td>");
                    stringBuilder.Append("<tr>");
                }
            }
            if (table3.Rows.Count > 0)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='border-bottom:solid 1px black;font-size:14px;' align='left' colspan='3'><b><u>Arrears</u> :-</b></td>");
                stringBuilder.Append("<tr>");
                foreach (DataRow row in (InternalDataCollectionBase)table3.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black; border-right:solid 1px black;' align='left'>" + row["ArrearName"] + "</td>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black; border-right:solid 1px black;' align='left'>" + row["ArrearRemarks"] + "</td>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black;' align='right'>" + Convert.ToDecimal(row["ArrearAmt"]).ToString("0.00") + "</td>");
                    stringBuilder.Append("<tr>");
                }
            }
            if (table4.Rows.Count > 0)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='border-bottom:solid 1px black;font-size:14px;' align='left' colspan='3'><b><u>Deductions</u> :-</b></td>");
                stringBuilder.Append("<tr>");
                foreach (DataRow row in (InternalDataCollectionBase)table4.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black; border-right:solid 1px black;' align='left'>" + row["DeductionType"] + "</td>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black; border-right:solid 1px black;' align='left'>" + row["DeductionRemarks"] + "</td>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black;' align='right'>" + Convert.ToDecimal(row["DeductionAmt"]).ToString("0.00") + "</td>");
                    stringBuilder.Append("<tr>");
                }
            }
            if (table5.Rows.Count > 0)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='border-bottom:solid 1px black;font-size:14px;' align='left' colspan='3'><b><u>Loan Transactions</u> :-</b></td>");
                stringBuilder.Append("<tr>");
                foreach (DataRow row in (InternalDataCollectionBase)table5.Rows)
                {
                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black; border-right:solid 1px black;' align='left'>" + row["LoanType"] + "</td>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black; border-right:solid 1px black;' align='left'>" + row["LoanRemarks"] + "</td>");
                    stringBuilder.Append("<td style='border-bottom:solid 1px black;' align='right'>" + Convert.ToDecimal(row["LoanAmt"]).ToString("0.00") + "</td>");
                    stringBuilder.Append("<tr>");
                }
            }
            btnPrint.Visible = true;
        }
        else
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='border-bottom:solid 1px black;font-size:14px;' align='center' colspan='3'><b>No Transaction Available</b></td>");
            stringBuilder.Append("<tr>");
            btnPrint.Visible = false;
        }
        stringBuilder.Append("</table");
        stringBuilder.Append("</fieldset>");
        Session["rptEmpTransactions"] = (lblReport.Text = stringBuilder.ToString().Trim());
    }

    protected void drpEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        drpEmpName.Focus();
    }

    protected void drpMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        drpMonth.Focus();
    }

    protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        drpYear.Focus();
    }
}