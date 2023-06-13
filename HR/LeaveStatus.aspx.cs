using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class HR_LeaveStatus : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        ViewState["LeaveAcYr"] = ConfigurationManager.AppSettings["LeaveAcYr"].ToString().Trim();
        GetLeaveDetails();
    }

    private void GetLeaveDetails()
    {
        ResetFields();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@cmd", drpStatusFor.SelectedValue);
        if (drpReporting.Visible && drpReporting.SelectedValue == "T")
            hashtable.Add("@Date", Convert.ToDateTime(DateTime.Now.ToString("MMM-dd-yyyy")));
        DataTable dataTable = obj.GetDataTable("HR_GetLeaveDtlsForStatus", hashtable);
        grdLeave.DataSource = dataTable.DefaultView;
        grdLeave.DataBind();
        lblRecCount.Text = "No. of Record(s) : " + dataTable.Rows.Count;
    }

    protected void grdLeave_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandName == "SetStatus"))
            return;
        ResetFields();
        if (ChkRunningLeave())
        {
            int int32 = Convert.ToInt32(e.CommandArgument);
            HiddenField control1 = grdLeave.Rows[int32].FindControl("hfId") as HiddenField;
            HiddenField control2 = grdLeave.Rows[int32].FindControl("hfLeaveId") as HiddenField;
            HiddenField control3 = grdLeave.Rows[int32].FindControl("hfAppliedDt") as HiddenField;
            HiddenField control4 = grdLeave.Rows[int32].FindControl("hfStartDt") as HiddenField;
            HiddenField control5 = grdLeave.Rows[int32].FindControl("hfLeaveTo") as HiddenField;
            HiddenField control6 = grdLeave.Rows[int32].FindControl("hfEmpId") as HiddenField;
            Label control7 = grdLeave.Rows[int32].FindControl("lblEmpName") as Label;
            Label control8 = grdLeave.Rows[int32].FindControl("lblLeaveCode") as Label;
            if (!(ViewState["LeaveApplyId"].ToString() != control1.Value))
                return;
            pnlStatus.Enabled = true;
            ViewState["LeaveAppliedDt"] = control3.Value;
            ViewState["LeaveCode"] = control8.Text;
            ViewState["LeaveId"] = control2.Value;
            ViewState["LeaveApplyId"] = control1.Value;
            ViewState["EmpId"] = control6.Value;
            ViewState["LeaveTo"] = control5.Value;
            lblName.Text = control7.Text;
            pnlStatus.BackColor = Color.Wheat;
            dtpEndDt.Enabled = true;
            if (control4.Value != "")
            {
                dtpStartDt.SetDateValue(Convert.ToDateTime(control4.Value));
                dtpStartDt.Enabled = false;
                txtStartDt.Enabled = false;
                ImageButton1.Enabled = false;
                dtpEndDt.Enabled = true;
                txtEndDt.Enabled = true;
                ImageButton2.Enabled = true;
                txtDaysAvailed.Enabled = true;
            }
            else
            {
                dtpStartDt.Enabled = true;
                txtStartDt.Enabled = true;
                ImageButton1.Enabled = true;
                dtpEndDt.Enabled = false;
                txtEndDt.Enabled = false;
                ImageButton2.Enabled = false;
                txtDaysAvailed.Enabled = false;
            }
        }
        else
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "A Running Leave is available. Can not Set Status for another Leave.";
        }
    }

    private bool ChkRunningLeave()
    {
        return Convert.ToInt32(obj.ExecuteScalarQry("SELECT COUNT(*) FROM dbo.HR_EmpLeave WHERE EmpId=" + ViewState["EmpId"].ToString() + " AND LeaveStartDt IS NOT NULL AND LeaveEndDt IS NULL")) == 0;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetFields();
    }

    private void ResetFields()
    {
        ViewState["LeaveApplyId"] = "0";
        ViewState["LeaveId"] = "0";
        ViewState["EmpId"] = "0";
        ViewState["LeaveCode"] = null;
        ViewState["LeaveTo"] = null;
        ViewState["LeaveAppliedDt"] = null;
        lblName.Text = string.Empty;
        txtDaysAvailed.Text = "0";
        txtStartDt.Text = string.Empty;
        txtEndDt.Text = string.Empty;
        dtpStartDt.Enabled = false;
        dtpEndDt.Enabled = false;
        pnlStatus.Enabled = false;
        pnlStatus.BackColor = SystemColors.Control;
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            trMsg.Style["background-color"] = (string)null;
            lblMsg.Text = string.Empty;
            if (txtStartDt.Enabled && txtStartDt.Text.Trim() == string.Empty)
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Enter Leave Start Date";
            }
            else if (!txtStartDt.Enabled && txtEndDt.Text.Trim() == string.Empty)
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Enter the Leave End Date";
            }
            else if (!txtStartDt.Enabled && (double)float.Parse(txtDaysAvailed.Text) <= 0.0)
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Enter the Days Availed";
            }
            else if (txtEndDt.Text.Trim() != string.Empty && dtpEndDt.GetDateValue() < dtpStartDt.GetDateValue())
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Enter Correct Leave End Date";
            }
            else if (txtEndDt.Text.Trim() != string.Empty)
            {
                DateTime dateTime = dtpStartDt.GetDateValue().AddDays(-1.0);
                DateTime dateValue = dtpEndDt.GetDateValue();
                float num = float.Parse(txtDaysAvailed.Text);
                string s = (dateValue - dateTime).TotalDays.ToString();
                if (Math.Ceiling((double)num) > (double)int.Parse(s) || Math.Ceiling((double)num) < (double)int.Parse(s))
                {
                    trMsg.Style["background-color"] = "Red";
                    lblMsg.Text = "Enter Correct Leave Start & End Date OR Check the Days Availed";
                }
                else
                    SetLeaveStatus();
            }
            else
                SetLeaveStatus();
        }
        catch (Exception ex)
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Transaction Failed. Try Again.";
        }
    }

    private void SetLeaveStatus()
    {
        try
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@LeaveApplyId", ViewState["LeaveApplyId"]);
            hashtable.Add("@StartDt", dtpStartDt.GetDateValue());
            if (txtEndDt.Text.Trim() != "")
            {
                hashtable.Add("@EndDt", Convert.ToDateTime(dtpEndDt.GetDateValue().ToString("MMM-dd-yyyy")));
                hashtable.Add("@DaysAvailed", float.Parse(txtDaysAvailed.Text));
            }
            hashtable.Add("@UserId", Session["User_Id"]);
            hashtable.Add("@EmpId", ViewState["EmpId"]);
            string str = obj.ExecuteScalar("HR_SetLeaveStatus", hashtable);
            DateTime dateTime1 = dtpStartDt.GetDateValue();
            DateTime dateTime2 = Convert.ToDateTime(ViewState["LeaveTo"]);
            if (str.Trim() == string.Empty && txtStartDt.Enabled)
            {
                for (; dateTime1 <= dateTime2; dateTime1 = dateTime1.AddDays(1.0))
                {
                    hashtable.Clear();
                    hashtable.Add("@EmpId", ViewState["EmpId"]);
                    hashtable.Add("@AttendDate", Convert.ToDateTime(dateTime1.ToString("MMM-dd-yyyy")));
                    hashtable.Add("@UserId", Session["User_Id"]);
                    hashtable.Add("@AttendStatus", "L");
                    hashtable.Add("@Remarks", ViewState["LeaveCode"]);
                    str = obj.ExecuteScalar("HR_InsUpdtEmpAttnFromLeaveStatus", hashtable);
                    if (str.Trim() != string.Empty)
                        break;
                }
                DateTime dateValue = dtpStartDt.GetDateValue();
                float totalDays = float.Parse(txtDaysAvailed.Text);
                if (txtEndDt.Text.Trim() != string.Empty)
                    str = InsUpdtEmpLeaveAvl(dateValue, totalDays);
            }
            if (str.Trim() == string.Empty)
            {
                GetLeaveDetails();
                trMsg.Style["background-color"] = "Green";
                lblMsg.Text = "Leave Status Updated Successfully";
            }
            else if (str.Trim().ToUpper() == "R")
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "A Leave already Running. Can not Set Status for another Leave.";
                btnSave.Focus();
            }
            else if (str.Trim().ToUpper() == "A")
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Leave already applied for this Date.";
                btnSave.Focus();
            }
            else
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Failed to Set Leave Status.";
            }
        }
        catch (Exception ex)
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Transaction Failed. Try Again.";
        }
    }

    private string InsUpdtEmpLeaveAvl(DateTime date, float totalDays)
    {
        string str1 = string.Empty;
        if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL")
        {
            str1 = date.Year.ToString();
            ViewState["Yr"] = str1;
        }
        else if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "SESS")
        {
            string str2 = obj.ExecuteScalarQry("select dbo.fuGetSessionYr('" + date.ToString("dd-MMM-yyyy") + "')");
            ViewState["Yr"] = str2;
            string[] strArray = str2.Split('-');
            str1 = strArray[0] + strArray[1];
        }
        ViewState["LeaveYr"] = str1;
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@LeaveApplyId", ViewState["LeaveApplyId"]);
        hashtable.Add("@CalYear", Convert.ToInt32(ViewState["LeaveYr"]));
        hashtable.Add("@EmpId", ViewState["EmpId"]);
        hashtable.Add("@LeaveId", ViewState["LeaveId"]);
        hashtable.Add("@LeaveCode", ViewState["LeaveCode"]);
        hashtable.Add("@DaysAvailed", totalDays);
        hashtable.Add("@UserId", Session["User_Id"]);
        hashtable.Add("@StartDate", Convert.ToDateTime(dtpStartDt.GetDateValue().ToString("MMM-dd-yyyy")));
        hashtable.Add("@EndDate", Convert.ToDateTime(dtpEndDt.GetDateValue().ToString("MMM-dd-yyyy")));
        return obj.ExecuteScalar("HR_InsEmpLeaveAvl", hashtable);
    }

    protected void drpStatusFor_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpReporting.SelectedIndex = 0;
        if (drpStatusFor.SelectedValue == "R")
            drpReporting.Visible = true;
        else
            drpReporting.Visible = false;
        GetLeaveDetails();
    }

    protected void drpReporting_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetLeaveDetails();
    }

    protected void dtpEndDt_SelectionChanged(object sender, EventArgs e)
    {
        if (!(txtStartDt.Enabled = txtStartDt.Text.Trim() != string.Empty && txtEndDt.Text != string.Empty))
            return;
        txtDaysAvailed.Text = (dtpEndDt.GetDateValue() - dtpStartDt.GetDateValue().AddDays(-1.0)).Days.ToString();
        txtDaysAvailed.Focus();
    }
}