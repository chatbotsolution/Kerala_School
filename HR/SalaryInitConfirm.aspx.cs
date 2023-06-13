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


public partial class HR_SalaryInitConfirm : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Decimal totalPayable;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        FillEmployees();
    }

    private void FillEmployees()
    {
        StringBuilder stringBuilder = new StringBuilder();
        DataTable dataTable = obj.GetDataTable("HR_GetInitSal");
        if (dataTable.Rows.Count > 0)
        {
            grdSalary.DataSource = dataTable;
            grdSalary.DataBind();
            hfMonth.Value = dataTable.Rows[0]["Month"].ToString().Trim();
            hfYear.Value = dataTable.Rows[0]["Year"].ToString().Trim();
            btnFinalize.Visible = true;
            lblCount.Text = "No Of Records:" + dataTable.Rows.Count.ToString();
        }
        else
        {
            grdSalary.DataSource = null;
            grdSalary.DataBind();
            btnFinalize.Visible = false;
            lblMsg.Text = "Salary Not Initiated Yet Or the Salary Has Already Been Generated";
            trMsg.Style["background-color"] = "Red";
            lblCount.Text = "";
        }
    }

    protected void btnReInit_Click(object sender, EventArgs e)
    {
        Response.Redirect("SalaryInit.aspx");
    }

    protected void btnFinalize_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SalYear", hfYear.Value.ToString().Trim());
        hashtable.Add("@SalMonth", hfMonth.Value.ToString().Trim());
        hashtable.Add("@SalMonthNo", CheckMonthNo(hfMonth.Value.ToString().Trim()));
        hashtable.Add("@BillNo", (hfMonth.Value.ToString().Trim() + '-' + hfYear.Value.ToString().Trim()));
        hashtable.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        string empty = string.Empty;
        string str = clsDal.ExecuteScalar("HR_GenMlyInitSalary", hashtable);
        if (str.Trim() != string.Empty)
        {
            lblMsg.Text = str;
            trMsg.Style["background-color"] = "Red";
        }
        else
        {
            FillEmployees();
            lblMsg.Text = "Salary Generated Successfully for " + hfMonth.Value.Trim() + " " + hfYear.Value.Trim();
            trMsg.Style["background-color"] = "Green";
        }
    }

    private int CheckMonthNo(string s)
    {
        return !(s.ToUpper() == "JAN") ? (!(s.ToUpper() == "FEB") ? (!(s.ToUpper() == "MAR") ? (!(s.ToUpper() == "APR") ? (!(s.ToUpper() == "MAY") ? (!(s.ToUpper() == "JUN") ? (!(s.ToUpper() == "JUL") ? (!(s.ToUpper() == "AUG") ? (!(s.ToUpper() == "SEP") ? (!(s.ToUpper() == "OCT") ? (!(s.ToUpper() == "NOV") ? 12 : 11) : 10) : 9) : 8) : 7) : 6) : 5) : 4) : 3) : 2) : 1;
    }

    protected void grdSalary_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            totalPayable += Convert.ToDecimal(((HiddenField)e.Row.FindControl("hfNetSal")).Value.Trim());
        lblTotal.Text = totalPayable.ToString("0.00");
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("HRInit.aspx");
    }
}