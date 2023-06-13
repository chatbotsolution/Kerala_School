using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_FineMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsPostBack)
                return;
            Session["title"] = Page.Title.ToString();
            if (Session["User_Id"] != null)
            {
                rbtnFixed.Checked = true;
                txtFixedAmnt.Enabled = true;
                txtDailyRate.Enabled = false;
                txtWeeklyRate.Enabled = false;
                txtFortNRate.Enabled = false;
                txtMlyrate.Enabled = false;
                txtYrlyrate.Enabled = false;
                if (Request.QueryString["Id"] == null)
                    return;
                ht.Clear();
                ht.Add("@FineId", int.Parse(Request.QueryString["Id"].ToString()));
                ht.Add("@CollegeId", Session["SchoolId"]);
                dt.Clear();
                dt = obj.GetDataTable("Lib_SP_GetFine", ht);
                if (dt.Rows.Count <= 0)
                    return;
                fillField(dt);
            }
            else
                Response.Redirect("../Login.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void fillField(DataTable dt)
    {
        if (dt.Rows[0]["FromDtStr"].ToString() != "")
            dtpEffDt.SetDateValue(Convert.ToDateTime(dt.Rows[0]["FromDtStr"]));
        if (dt.Rows[0]["FineType"].ToString() == "F")
        {
            rbtnFixed.Checked = true;
            rbtnVariable.Checked = false;
            rbtnFixed.Enabled = false;
            rbtnVariable.Enabled = false;
            txtFixedAmnt.Enabled = true;
            txtDailyRate.Enabled = false;
            txtWeeklyRate.Enabled = false;
            txtFortNRate.Enabled = false;
            txtMlyrate.Enabled = false;
            txtYrlyrate.Enabled = false;
            txtFixedAmnt.Text = dt.Rows[0]["FixedAmtStr"].ToString();
            txtDailyRate.Text = string.Empty;
            txtWeeklyRate.Text = string.Empty;
            txtFortNRate.Text = string.Empty;
            txtMlyrate.Text = string.Empty;
            txtYrlyrate.Text = string.Empty;
        }
        else
        {
            if (!(dt.Rows[0]["FineType"].ToString() == "V"))
                return;
            rbtnFixed.Checked = false;
            rbtnVariable.Checked = true;
            rbtnFixed.Enabled = false;
            rbtnVariable.Enabled = false;
            txtFixedAmnt.Enabled = false;
            txtDailyRate.Enabled = true;
            txtWeeklyRate.Enabled = true;
            txtFortNRate.Enabled = true;
            txtMlyrate.Enabled = true;
            txtYrlyrate.Enabled = true;
            txtFixedAmnt.Text = string.Empty;
            txtDailyRate.Text = dt.Rows[0]["DailyRateStr"].ToString();
            txtWeeklyRate.Text = dt.Rows[0]["WeeklyRateStr"].ToString();
            txtFortNRate.Text = dt.Rows[0]["FortnightlyRateStr"].ToString();
            txtMlyrate.Text = dt.Rows[0]["MonthlyRateStr"].ToString();
            txtYrlyrate.Text = dt.Rows[0]["YearlyRateStr"].ToString();
        }
    }

    protected void rbtnFixed_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnFixed.Checked)
        {
            txtFixedAmnt.Enabled = true;
            txtDailyRate.Enabled = false;
            txtWeeklyRate.Enabled = false;
            txtFortNRate.Enabled = false;
            txtMlyrate.Enabled = false;
            txtYrlyrate.Enabled = false;
            rbtnVariable.Checked = false;
            rbtnFixed.Checked = true;
            Clear();
        }
        if (!rbtnVariable.Checked)
            return;
        txtFixedAmnt.Enabled = false;
        txtDailyRate.Enabled = true;
        txtWeeklyRate.Enabled = true;
        txtFortNRate.Enabled = true;
        txtMlyrate.Enabled = true;
        txtYrlyrate.Enabled = true;
        rbtnFixed.Checked = false;
        rbtnVariable.Checked = true;
        Clear();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        SaveData();
        Response.Redirect("FineList.aspx");
    }

    private void SaveData()
    {
        try
        {
            ht.Clear();
            if (Request.QueryString["Id"] != null)
                ht.Add("@FineId", int.Parse(Request.QueryString["Id"].ToString()));
            if (txtEffDate.Text != "")
                ht.Add("@FromDt", dtpEffDt.SelectedDate.ToString());
            if (rbtnFixed.Checked)
                ht.Add("@FineType", "F");
            else if (rbtnVariable.Checked)
                ht.Add("@FineType", "V");
            if (txtFixedAmnt.Text != "")
                ht.Add("@FixedAmt", Decimal.Parse(txtFixedAmnt.Text.Trim()));
            if (txtDailyRate.Text != "")
                ht.Add("@DailyRate", Decimal.Parse(txtDailyRate.Text.Trim()));
            if (txtWeeklyRate.Text != "")
                ht.Add("@WeeklyRate", Decimal.Parse(txtWeeklyRate.Text.Trim()));
            if (txtFortNRate.Text != "")
                ht.Add("@FortnightlyRate", Decimal.Parse(txtFortNRate.Text.Trim()));
            if (txtMlyrate.Text != "")
                ht.Add("@MonthlyRate", Decimal.Parse(txtMlyrate.Text.Trim()));
            if (txtYrlyrate.Text != "")
                ht.Add("@YearlyRate", Decimal.Parse(txtYrlyrate.Text.Trim()));
            ht.Add("@CollegeId", Session["SchoolId"]);
            dt.Clear();
            dt = obj.GetDataTable("Lib_InsUpdtFine", ht);
            Clear();
            txtEffDate.Text = string.Empty;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
        txtEffDate.Text = string.Empty;
    }

    private void Clear()
    {
        txtFixedAmnt.Text = string.Empty;
        txtDailyRate.Text = string.Empty;
        txtWeeklyRate.Text = string.Empty;
        txtFortNRate.Text = string.Empty;
        txtMlyrate.Text = string.Empty;
        txtYrlyrate.Text = string.Empty;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Response.Redirect("FineList.aspx");
    }
}