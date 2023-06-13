using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_GenerateSalary : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Decimal totalPayable;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        lblTotal.Text = "0";
        if (Page.IsPostBack)
            return;
        if (!ChkIsHRUsed())
        {
            Response.Redirect("HRHome.aspx?IsHRUsed=0");
        }
        else
        {
            bindSalYear();
            CheckLastSal();
        }
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = clsDal.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    private void CheckLastSal()
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = new clsDAL().GetDataTableQry("select top 1 * from dbo.HR_GenerateMlySalary order by SalGenId desc");
        if (dataTableQry.Rows.Count > 0)
        {
            lblMsg.Text = "Last Salary was Generated for " + dataTableQry.Rows[0]["SalMonth"].ToString() + " " + dataTableQry.Rows[0]["SalYear"].ToString();
            trMsg.Style["background-color"] = "Red";
        }
        else
        {
            trMsg.Style["background-color"] = (string)null;
            lblMsg.Text = string.Empty;
        }
    }

    private void bindSalYear()
    {
        int year = DateTime.Now.Year;
        int num = year - 1;
        drpYear.Items.Insert(0, new ListItem("- SELECT -", "0"));
        drpYear.Items.Insert(1, new ListItem(year.ToString(), year.ToString()));
        drpYear.Items.Insert(2, new ListItem(num.ToString(), num.ToString()));
    }

    protected void btnGenSalary_Click(object sender, EventArgs e)
    {
        int month = 1;
        if (drpMonth.SelectedValue.ToString() == "JAN")
            month = 1;
        else if (drpMonth.SelectedValue.ToString() == "FEB")
            month = 2;
        else if (drpMonth.SelectedValue.ToString() == "MAR")
            month = 3;
        else if (drpMonth.SelectedValue.ToString() == "APR")
            month = 4;
        else if (drpMonth.SelectedValue.ToString() == "MAY")
            month = 5;
        else if (drpMonth.SelectedValue.ToString() == "JUN")
            month = 6;
        else if (drpMonth.SelectedValue.ToString() == "JUL")
            month = 7;
        else if (drpMonth.SelectedValue.ToString() == "AUG")
            month = 8;
        else if (drpMonth.SelectedValue.ToString() == "SEP")
            month = 9;
        else if (drpMonth.SelectedValue.ToString() == "OCT")
            month = 10;
        else if (drpMonth.SelectedValue.ToString() == "NOV")
            month = 11;
        else if (drpMonth.SelectedValue.ToString() == "DEC")
            month = 12;
        int num = DateTime.DaysInMonth(int.Parse(drpYear.SelectedValue.ToString()), month);
        if (Convert.ToInt32(obj.ExecuteScalarQry("select datediff(day,'1 " + drpMonth.SelectedValue + " " + drpYear.SelectedValue + "',dateadd(month, 1, '1 " + drpMonth.SelectedValue + " " + drpYear.SelectedValue + "'))")) - Convert.ToInt32(obj.ExecuteScalarQry("select count(distinct holidaydate) from dbo.HR_HolidayMaster where HolidayDate>='1 " + drpMonth.SelectedValue + " " + drpYear.SelectedValue + "' and HolidayDate<='" + num + " " + drpMonth.SelectedValue + " " + drpYear.SelectedValue + "' and is_working=0")) == Convert.ToInt32(obj.ExecuteScalarQry("select count(distinct AttendDate) from dbo.HR_EmpAttendance where AttendDate>='1 " + drpMonth.SelectedValue + " " + drpYear.SelectedValue + "' and AttendDate<='" + num + " " + drpMonth.SelectedValue + " " + drpYear.SelectedValue + "' and AttendStatus<>'L'")))
        {
            GenMlySalary();
        }
        else
        {
            lblMsg.Text = "Mark Attendance of whole Month Before Generating Salary";
            trMsg.Style["background-color"] = "Red";
        }
    }

    protected void GenMlySalary()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SalYear", drpYear.SelectedValue);
        hashtable.Add("@SalMonth", drpMonth.SelectedValue);
        hashtable.Add("@SalMonthNo", CheckMonthNo(drpMonth.SelectedValue));
        hashtable.Add("@BillNo", (drpMonth.SelectedValue + '-' + drpYear.SelectedValue));
        hashtable.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        string empty = string.Empty;
        string str = rdbtnlstGenSal.SelectedIndex != 0 ? clsDal.ExecuteScalar("HR_GenMlySalaryWithoutDed", hashtable) : clsDal.ExecuteScalar("HR_GenMlySalaryWithExcessDed", hashtable);
        if (str.Trim() != string.Empty)
        {
            lblMsg.Text = str;
            trMsg.Style["background-color"] = "Red";
        }
        else
        {
            ShowSalary();
            lblMsg.Text = "Salary Generated Successfully for " + drpMonth.SelectedValue.ToString() + " " + drpYear.SelectedValue.ToString();
            trMsg.Style["background-color"] = "Green";
        }
    }

    private int CheckMonthNo(string s)
    {
        return !(s.ToUpper() == "JAN") ? (!(s.ToUpper() == "FEB") ? (!(s.ToUpper() == "MAR") ? (!(s.ToUpper() == "APR") ? (!(s.ToUpper() == "MAY") ? (!(s.ToUpper() == "JUN") ? (!(s.ToUpper() == "JUL") ? (!(s.ToUpper() == "AUG") ? (!(s.ToUpper() == "SEP") ? (!(s.ToUpper() == "OCT") ? (!(s.ToUpper() == "NOV") ? 12 : 11) : 10) : 9) : 8) : 7) : 6) : 5) : 4) : 3) : 2) : 1;
    }

    protected void btnPrintPayRoll_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptPayBillNew.aspx");
    }

    protected void btnPrintSalSlip_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptSalSlip.aspx");
    }

    protected void btnDelMonthSal_Click(object sender, EventArgs e)
    {
        ClearGrid();
        string str = obj.ExecuteScalar("HR_DelSalGenerated", new Hashtable()
    {
      {
         "SalYr",
         drpYear.SelectedValue
      },
      {
         "SalMonth",
         drpMonth.SelectedValue
      }
    });
        if (str.Trim().ToUpper() != "S")
        {
            lblMsg.Text = str;
            trMsg.Style["background-color"] = "Red";
        }
        else
        {
            lblMsg.Text = "Salary Deleted Successfully for " + drpMonth.SelectedValue.ToString() + " " + drpYear.SelectedValue.ToString();
            trMsg.Style["background-color"] = "Green";
        }
    }

    private void ShowSalary()
    {
        totalPayable = new Decimal(0);
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT SalId,EmpName,Designation,Pay,GrossTot,TotalDeduction,NetSal,EmpId,SalPaidDate,PaidDays FROM HR_view_MonthlySalaryNew");
        stringBuilder.Append(" WHERE [Year]=" + drpYear.SelectedValue + " AND [Month]='" + drpMonth.SelectedValue + "'");
        stringBuilder.Append(" AND PayWithHeld='N'");
        stringBuilder.Append(" ORDER BY EmpName");
        DataTable dataTableQry = obj.GetDataTableQry(stringBuilder.ToString());
        grdSalary.DataSource = dataTableQry;
        grdSalary.DataBind();
        lblRecords.Text = "No of Records : " + dataTableQry.Rows.Count;
        if (dataTableQry.Rows.Count > 0)
        {
            trMsg.Style["background-color"] = "Green";
            lblMsg.Text = "Showing Generated Salary for " + drpMonth.SelectedValue + "-" + drpYear.SelectedValue;
        }
        else
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "No Record Available for " + drpMonth.SelectedValue + "-" + drpYear.SelectedValue;
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        ShowSalary();
    }

    protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearGrid();
        drpYear.Focus();
    }

    protected void drpMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearGrid();
        drpMonth.Focus();
    }

    private void ClearGrid()
    {
        grdSalary.DataSource = null;
        grdSalary.DataBind();
        CheckLastSal();
        lblRecords.Text = string.Empty;
    }

    protected void grdSalary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            HiddenField control1 = (HiddenField)e.Row.FindControl("hfEmpId");
            HiddenField control2 = (HiddenField)e.Row.FindControl("hfNetSal");
            HiddenField control3 = (HiddenField)e.Row.FindControl("hfGross");
            HiddenField control4 = (HiddenField)e.Row.FindControl("hfDed");
            Label control5 = (Label)e.Row.FindControl("lblGross");
            Label control6 = (Label)e.Row.FindControl("lblPrevAmt");
            Label control7 = (Label)e.Row.FindControl("lblPayable");
            Hashtable hashtable = new Hashtable();
            hashtable.Clear();
            hashtable.Add("@EmpId", control1.Value);
            hashtable.Add("@Month", drpMonth.SelectedValue);
            hashtable.Add("@Year", drpYear.SelectedValue);
            DataTable dataTable = obj.GetDataTable("HR_GetPrevAmt", hashtable);
            string str1 = dataTable.Rows[0]["ClaimAmt"].ToString();
            string str2 = dataTable.Rows[0]["PrevAmt"].ToString();
            string str3 = dataTable.Rows[0]["Arrear"].ToString();
            if (str1.Trim() == string.Empty)
                str1 = "0";
            if (str2.Trim() == string.Empty)
                str2 = "0";
            if (str3.Trim() == string.Empty)
                str3 = "0";
            Decimal d = Convert.ToDecimal(str2) + Convert.ToDecimal(str3) + Convert.ToDecimal(str1);
            control6.Text = Math.Round(d).ToString("0.00");
            control7.Text = Math.Round(Convert.ToDecimal(control3.Value) - (Convert.ToDecimal(control4.Value) + Convert.ToDecimal(str1)) + d).ToString("0.00");
            control5.Text = Math.Round(Convert.ToDecimal(control3.Value) - Convert.ToDecimal(str1)).ToString("0.00");
            totalPayable += Convert.ToDecimal(control7.Text);
        }
        lblTotal.Text = totalPayable.ToString("0.00");
    }
}