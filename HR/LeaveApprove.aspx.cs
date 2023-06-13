using ASP;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_LeaveApprove : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        GetLeaveDetails();
    }

    private void GetLeaveDetails()
    {
        try
        {
            ViewState["LeaveAcYr"] = ConfigurationManager.AppSettings["LeaveAcYr"].ToString().Trim();
            string[] strArray = obj.ExecuteScalarQry("select dbo.fuGetSessionYr('" + DateTime.Now.ToString("dd-MMM-yyyy") + "')").Split('-');
            DateTime dateTime1 = Convert.ToDateTime("01-APR-" + strArray[0] + " 00:00:00");
            DateTime dateTime2 = Convert.ToDateTime("31-MAR-" + (Convert.ToInt32(strArray[0]) + 1) + " 23:59:59");
            if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL")
            {
                dateTime1 = Convert.ToDateTime("01-JAN-" + DateTime.Now.Year + " 00:00:00");
                dateTime2 = Convert.ToDateTime("31-DEC-" + DateTime.Now.Year + " 23:59:59");
            }
            if (Request.QueryString["appId"] == null)
                return;
            DataSet dataSet = obj.GetDataSet("HR_GetLeaveDtlsForApproval", new Hashtable()
      {
        {
           "LeaveApplyId",
           Request.QueryString["appId"]
        },
        {
           "@FromDt",
           dateTime1
        },
        {
           "@ToDt",
           dateTime2
        }
      });
            DataTable table1 = dataSet.Tables[0];
            DataTable table2 = dataSet.Tables[1];
            ViewState["LeaveFrom"] = table1.Rows[0]["LeaveFrom"].ToString();
            ViewState["CFAllowed"] = table1.Rows[0]["CFAllowed"].ToString();
            lblEmpName.Text = table1.Rows[0]["EmpName"].ToString();
            lblLeaveType.Text = table1.Rows[0]["LeaveCode"].ToString() + " (" + table1.Rows[0]["LeaveDesc"].ToString() + ")";
            lblDaysApplied.Text = table1.Rows[0]["DaysApplied"].ToString();
            lblFromDt.Text = Convert.ToDateTime(table1.Rows[0]["LeaveFrom"].ToString()).ToString("dd-MMM-yyyy");
            lblToDt.Text = Convert.ToDateTime(table1.Rows[0]["LeaveTo"].ToString()).ToString("dd-MMM-yyyy");
            lblReason.Text = table1.Rows[0]["Reason"].ToString();
            txtRemarks.Text = table1.Rows[0]["AppRejRemarks"].ToString();
            ViewState["LeaveCode"] = table1.Rows[0]["LeaveCode"].ToString().Trim();
            ViewState["DaysAppPrev"] = table1.Rows[0]["DaysApproved"].ToString();
            if (Request.QueryString["action"] == "a")
            {
                lblStatus.Text = "Not Approved";
                txtDaysApproved.Text = table1.Rows[0]["DaysApplied"].ToString();
                rbReject.Enabled = true;
            }
            else if (Request.QueryString["action"] == "m" && table1.Rows[0]["LeaveStartDt"].ToString().Trim() == string.Empty)
            {
                lblStatus.Text = "Approved but Not Started, Days Approved : " + table1.Rows[0]["DaysApproved"].ToString();
                txtDaysApproved.Text = table1.Rows[0]["DaysApproved"].ToString();
                rbReject.Enabled = true;
            }
            else if (Request.QueryString["action"] == "m" && table1.Rows[0]["LeaveStartDt"].ToString().Trim() != string.Empty)
            {
                lblStatus.Text = "Started on " + table1.Rows[0]["LeaveStartDt"].ToString() + ", Days Approved : " + table1.Rows[0]["DaysApproved"].ToString();
                txtDaysApproved.Text = table1.Rows[0]["DaysApproved"].ToString();
                rbReject.Enabled = false;
            }
            grdLeave.DataSource = table2.DefaultView;
            grdLeave.DataBind();
        }
        catch (Exception ex)
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Transaction Failed. Try Again.";
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            trMsg.Style["background-color"] = (string)null;
            lblMsg.Text = string.Empty;
            int num = 0;
            string str1 = string.Empty;
            if (txtDaysApproved.Text.Trim() != string.Empty)
                num = Convert.ToInt32(txtDaysApproved.Text);
            ViewState["LeaveTo"] = Convert.ToDateTime(ViewState["LeaveFrom"]).AddDays((double)num);
            if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL")
            {
                string str2 = DateTime.Now.Year.ToString();
                ViewState["Yr"] = str2;
                ViewState["YearEnd"] = Convert.ToDateTime("31-DEC-" + str2);
            }
            else if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "SESS")
            {
                string str2 = obj.ExecuteScalarQry("select dbo.fuGetSessionYr('" + DateTime.Now.ToString("dd-MMM-yyyy") + "')");
                ViewState["Yr"] = str2;
                string[] strArray = str2.Split('-');
                str1 = strArray[0] + strArray[1];
                ViewState["YearEnd"] = Convert.ToDateTime("31-MAR-" + (Convert.ToInt32(strArray[0]) + 1).ToString());
            }
            if (rbApproved.Checked && Convert.ToDouble(txtDaysApproved.Text) == 0.0)
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Enter Total Days Approved";
                txtDaysApproved.Focus();
            }
            else if (ViewState["CFAllowed"].ToString().Trim().ToUpper() == "FALSE" && Convert.ToDateTime(ViewState["LeaveTo"]) > Convert.ToDateTime(ViewState["YearEnd"]))
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Carry Forward not allowed for " + lblLeaveType.Text + ". Leave End Date should not exceed " + Convert.ToDateTime(ViewState["YearEnd"]).ToString("dd-MMM-yyyy") + ". Enter Correct Approved Days.";
                txtDaysApproved.Focus();
            }
            else if (txtRemarks.Text.Trim() == "")
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Enter Remarks";
                txtRemarks.Focus();
            }
            else if (Request.QueryString["Action"].ToString().Trim() == "m" && Convert.ToDecimal(txtDaysApproved.Text) < Convert.ToDecimal(ViewState["DaysAppPrev"]))
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Leave already Started. Decrement in Days is not allowed.";
                btnSave.Focus();
            }
            else
            {
                Hashtable hashtable = new Hashtable();
                if (rbReject.Checked)
                    hashtable.Add("@Action", "R");
                else
                    hashtable.Add("@Action", Request.QueryString["Action"]);
                hashtable.Add("@LeaveApplyId", Request.QueryString["appId"]);
                if (rbApproved.Checked)
                {
                    hashtable.Add("@DaysApproved", txtDaysApproved.Text);
                    hashtable.Add("@Approved_Rejected", "Approved");
                }
                else if (rbReject.Checked)
                    hashtable.Add("@Approved_Rejected", "Rejected");
                hashtable.Add("@ApproveRejectByUserId", Session["User_Id"]);
                hashtable.Add("@AppRejRemarks", txtRemarks.Text.Trim());
                hashtable.Add("@UserId", Session["User_Id"]);
                hashtable.Add("@LeaveCode", ViewState["LeaveCode"]);
                if (obj.ExecuteScalar("HR_ApproveLeave", hashtable).Trim() == string.Empty)
                {
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Leave Approved Successfully.');window.location='LeaveApproveList.aspx'", true);
                }
                else
                {
                    trMsg.Style["background-color"] = "Red";
                    lblMsg.Text = "Failed to Approve. Try Again.";
                }
            }
        }
        catch (Exception ex)
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Failed to Approve. Try Again.";
        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("LeaveApproveList.aspx");
    }

    protected void rbApproved_CheckedChanged(object sender, EventArgs e)
    {
        if (!rbApproved.Checked)
            return;
        txtDaysApproved.Enabled = true;
    }

    protected void rbReject_CheckedChanged(object sender, EventArgs e)
    {
        if (!rbReject.Checked)
            return;
        txtDaysApproved.Enabled = false;
    }
}