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

public partial class HR_LeaveApply : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            dtpDate.SetDateValue(DateTime.Now);
            dtpDate.To.Date = DateTime.Today.AddMonths(3);
            bindDropDown(drpEmpName, "SELECT EmpId,SevName FROM dbo.HR_EmployeeMaster WHERE ActiveStatus=1 ORDER BY SevName", "SevName", "EmpId");
            ViewState["LeaveYr"] = GetLeaveYear();
            lblYear.Text = ViewState["Yr"].ToString();
            CheckAuthLeaveAll();
        }
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
    }

    private string GetLeaveYear()
    {
        string str1 = string.Empty;
        ViewState["LeaveAcYr"] = ConfigurationManager.AppSettings["LeaveAcYr"].ToString().Trim();
        if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL")
        {
            str1 = DateTime.Now.Year.ToString();
            ViewState["Yr"] = str1;
            ViewState["YearEnd"] = Convert.ToDateTime("31-DEC-" + str1);
        }
        else if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "SESS")
        {
            string str2 = obj.ExecuteScalarQry("select dbo.fuGetSessionYr('" + DateTime.Now.ToString("dd-MMM-yyyy") + "')");
            ViewState["Yr"] = str2;
            string[] strArray = str2.Split('-');
            str1 = strArray[0] + strArray[1];
            ViewState["YearEnd"] = Convert.ToDateTime("31-MAR-" + (Convert.ToInt32(strArray[0]) + 1).ToString());
        }
        return str1;
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

    private string IsValid()
    {
        int num = 0;
        string str = string.Empty;
        if (txtApplied.Text.Trim() != string.Empty)
            num = Convert.ToInt32(txtDaysApplied.Text);
        ViewState["LeaveTo"] = dtpDate.GetDateValue().AddDays((double)num);
        if (drpEmpName.SelectedIndex == 0)
        {
            str = "Select an Employee";
            drpEmpName.Focus();
        }
        else if (hfLeaveId.Value == string.Empty)
        {
            str = "Select a Leave Type";
            grdLeave.Focus();
        }
        else if (txtDate.Text == string.Empty)
        {
            str = "Enter From Date";
            txtDate.Focus();
        }
        else if (txtDaysApplied.Text == string.Empty || Convert.ToDouble(txtDaysApplied.Text) == 0.0)
        {
            str = "Enter Total Days";
            txtDaysApplied.Focus();
        }
        else if (ViewState["CFAllowed"].ToString().Trim().ToUpper() == "FALSE" && Convert.ToDateTime(ViewState["LeaveTo"]) > Convert.ToDateTime(ViewState["YearEnd"]))
        {
            str = "Carry Forward not allowed for " + txtLeave.Text + ". Leave End Date should not exceed " + Convert.ToDateTime(ViewState["YearEnd"]).ToString("dd-MMM-yyyy");
            txtDaysApplied.Focus();
        }
        else if (txtReason.Text == string.Empty)
        {
            str = "Enter Reason";
            txtReason.Focus();
        }
        else if (hfLeaveId.Value != "5")
        {
            if (Convert.ToDouble(txtBalLeave.Text) <= 0.0)
            {
                str = "No Balance available for " + txtLeave.Text + ".";
                grdLeave.Focus();
            }
            else if (Convert.ToDouble(txtDaysApplied.Text) > Convert.ToDouble(txtMaxDays.Text))
            {
                str = "Total Days shouldn't exceed Maximun Days allowed.";
                txtDaysApplied.Focus();
            }
            else if (Convert.ToDouble(txtDaysApplied.Text) > Convert.ToDouble(txtBalLeave.Text))
            {
                str = "Total Days shouldn't exceed Balance Days.";
                txtDaysApplied.Focus();
            }
        }
        return str;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (CheckSalGenForLeaveDt())
        {
            lblMsg.Text = "Salary already generated for the applied date. Leave can not be applied for this date now.";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            string empty = string.Empty;
            string str = IsValid();
            if (str.Trim() == string.Empty)
                str = grdLeave.Rows.Count <= 0 ? "No Leave has been Authorized for this Employee." : ApplyLeave();
            if (str.Trim().ToUpper() == "S")
            {
                ClearFields();
                trMsg.Style["background-color"] = "Green";
                lblMsg.Text = "Data Saved Successfully";
                drpEmpName.Focus();
            }
            else if (str.Trim().ToUpper() == "R")
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "A Leave already Running. Can not apply another Leave.";
                btnSave.Focus();
            }
            else if (str.Trim().ToUpper() == "A")
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Leave already applied for this Date.";
                btnSave.Focus();
            }
            else if (str.Trim().ToUpper() == "F")
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Failed to Save Data. Try Again.";
                btnSave.Focus();
            }
            else
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = str;
            }
        }
    }

    private bool CheckSalGenForLeaveDt()
    {
        return (int)Convert.ToInt16(new clsDAL().ExecuteScalarQry("select COUNT(*) from dbo.HR_EmpMlySalary where EmpId=" + drpEmpName.SelectedValue + " and Year=" + dtpDate.GetDateValue().Year + " and Month='" + dtpDate.GetDateValue().ToString("MMM") + "'")) > 0;
    }

    private string ApplyLeave()
    {
        string str = string.Empty;
        string empty = string.Empty;
        trMsg.Style["background-color"] = "";
        lblMsg.Text = string.Empty;
        try
        {
            if (Convert.ToInt32(obj.ExecuteScalarQry("SELECT COUNT(*) FROM dbo.HR_EmpLeaveDtls WHERE EmpId=" + drpEmpName.SelectedValue + " AND CalYear=" + ViewState["LeaveYr"] + " AND LeaveId=" + hfLeaveId.Value)) == 0)
                str = InsUpdtEmpLeaveDtls();
            if (str.Trim() == string.Empty)
                str = obj.ExecuteScalar("HR_ApplyLeave", new Hashtable()
        {
          {
             "@EmpId",
             drpEmpName.SelectedValue
          },
          {
             "@LeaveId",
             hfLeaveId.Value
          },
          {
             "@LeaveFrom",
             dtpDate.GetDateValue().ToString("dd-MMM-yyyy")
          },
          {
             "@LeaveTo",
             dtpDate.GetDateValue().AddDays(Math.Ceiling((double) float.Parse(txtDaysApplied.Text)) - 1.0).ToString("dd-MMM-yyyy")
          },
          {
             "@DaysApplied",
             txtDaysApplied.Text
          },
          {
             "@Reason",
             txtReason.Text.Trim()
          }
        });
        }
        catch (Exception ex)
        {
            str = "Transaction Failed. Try Again.";
        }
        return str;
    }

    private string InsUpdtEmpLeaveDtls()
    {
        string str = string.Empty;
        try
        {
            DataTable dataTableQry = obj.GetDataTableQry("SELECT LeaveId FROM dbo.HR_LeaveAuthorized WHERE DesignationId=" + obj.ExecuteScalarQry("SELECT DesignationId FROM dbo.HR_EmployeeMaster WHERE EmpId=" + drpEmpName.SelectedValue));
            if (dataTableQry.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
                    obj.ExecuteScalar("HR_InsUpdtEmpLeaveDtls", new Hashtable()
          {
            {
               "@CalYear",
               Convert.ToInt32(ViewState["LeaveYr"])
            },
            {
               "@LeaveId",
              row["LeaveId"]
            },
            {
               "@EmpId",
               drpEmpName.SelectedValue
            },
            {
               "@UserId",
              Session["User_Id"]
            }
          });
                ApplyLeave();
            }
            else
                str = "Please Authorize Leave for this Employee. (Go to Leave --> Auhtorize Leave)";
        }
        catch (Exception ex)
        {
            str = "Failed to Authorize Leave. Try Again.";
        }
        return str;
    }

    private void LeaveAuthEmpwise(int desgId, int calYear, int leaveId, float authDays)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTableQry = obj.GetDataTableQry("SELECT EmpId FROM dbo.HR_EmployeeMaster WHERE DesignationId=" + desgId);
        if (dataTableQry.Rows.Count <= 0)
            return;
        foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
        {
            hashtable.Clear();
            hashtable.Add("@EmpId", row["EmpId"]);
            hashtable.Add("@CalYear", calYear);
            hashtable.Add("@LeaveId", leaveId);
            hashtable.Add("@UserId", Session["User_Id"]);
            hashtable.Add("@AuthDays", authDays);
            obj.ExecuteScalar("HR_LeaveAuthEmpwise", hashtable);
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    private void ClearFields()
    {
        hfBalLeave.Value = "0";
        drpEmpName.SelectedIndex = 0;
        hfLeaveId.Value = string.Empty;
        txtLeave.Text = string.Empty;
        dtpDate.SetDateValue(DateTime.Now);
        txtMaxDays.Text = "0";
        txtDaysApplied.Text = "0";
        txtAuth.Text = "0";
        txtNotAvailed.Text = "0";
        txtBalLeave.Text = "0";
        txtReason.Text = string.Empty;
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        ViewState["gender"] = null;
        grdLeave.DataSource = null;
        grdLeave.DataBind();
        grdLeave.Visible = false;
        drpEmpName.Focus();
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        GridViewRow parent = (GridViewRow)((Control)sender).Parent.Parent;
        hfLeaveId.Value = grdLeave.DataKeys[parent.DataItemIndex].Value.ToString();
        hfBalLeave.Value = ((HiddenField)parent.FindControl("hfBalance")).Value;
        if (drpEmpName.SelectedIndex > 0 && hfLeaveId.Value != string.Empty)
        {
            txtLeave.Text = obj.ExecuteScalarQry("SELECT RTRIM(LeaveCode)+' ('+LeaveDesc+')' FROM dbo.HR_LeaveMaster WHERE LeaveId=" + hfLeaveId.Value);
            CheckAvlLeave();
        }
        else
        {
            hfBalLeave.Value = "0";
            txtBalLeave.Text = "0";
            txtAuth.Text = "0";
            txtMaxDays.Text = "0";
            txtNotAvailed.Text = "0";
            txtDaysApplied.Text = "0";
        }
        txtDate.Focus();
    }

    private void CheckAvlLeave()
    {
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        Decimal num3 = new Decimal(0);
        Decimal num4 = new Decimal(0);
        Hashtable hashtable = new Hashtable();
        if (ViewState["gender"].ToString().Trim().ToUpper() == "MALE")
            hashtable.Add("@Exclude", 4);
        hashtable.Add("@EmpId", drpEmpName.SelectedValue);
        if (hfLeaveId.Value != string.Empty)
            hashtable.Add("@LeaveId", hfLeaveId.Value);
        hashtable.Add("@CalYear", ViewState["LeaveYr"]);
        DataSet dataSet = obj.GetDataSet("HR_CheckAvlLeave", hashtable);
        DataTable table1 = dataSet.Tables[0];
        DataTable table2 = dataSet.Tables[1];
        DataTable table3 = dataSet.Tables[2];
        DataTable table4 = dataSet.Tables[3];
        DataTable table5 = dataSet.Tables[4];
        DataTable table6 = dataSet.Tables[5];
        if (table6.Rows.Count > 0)
            ViewState["CFAllowed"] = table6.Rows[0][0].ToString();
        if (table4.Rows.Count > 0)
            num3 = Convert.ToDecimal(table4.Rows[0][0].ToString());
        grdLeave.DataSource = table3;
        grdLeave.DataBind();
        grdLeave.Visible = true;
        if (table2.Rows.Count > 0)
            num1 = Convert.ToDecimal(table2.Rows[0]["AvlDays"].ToString());
        if (table1.Rows.Count > 0)
        {
            Decimal num5 = Convert.ToDecimal(table5.Rows[0]["AuthDays"]);
            txtAuth.Text = table5.Rows[0]["AuthDays"].ToString();
            txtMaxDays.Text = table1.Rows[0]["MaxDaysAllowed"].ToString();
            txtNotAvailed.Text = obj.ExecuteScalarQry("SELECT ISNULL(SUM(DaysApproved),0) FROM dbo.HR_EmpLeave WHERE EmpId=" + drpEmpName.SelectedValue + " AND LeaveId=" + hfLeaveId.Value + " AND Approved_Rejected='Approved' AND YEAR(LeaveAppliedDt)=" + dtpDate.GetDateValue().Year + " AND DaysAvailed IS NULL");
            Decimal num6 = Convert.ToDecimal(txtNotAvailed.Text);
            txtBalLeave.Text = (num5 - (num1 + num6 + num3)).ToString();
            txtApplied.Text = num3.ToString();
        }
        else
        {
            hfBalLeave.Value = "0";
            txtBalLeave.Text = "0";
            txtMaxDays.Text = "0";
            txtAuth.Text = "0";
            txtNotAvailed.Text = "0";
            txtDaysApplied.Text = "0";
        }
    }

    protected void drpEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        hfLeaveId.Value = string.Empty;
        txtLeave.Text = string.Empty;
        if (drpEmpName.SelectedIndex > 0)
        {
            ViewState["gender"] = obj.ExecuteScalarQry("SELECT Sex FROM dbo.HR_EmployeeMaster WHERE EmpId=" + drpEmpName.SelectedValue);
            CheckAuthLeave();
            CheckAvlLeave();
        }
        else
        {
            hfBalLeave.Value = "0";
            dtpDate.DateValue = DateTime.Now; 
            txtMaxDays.Text = "0";
            txtDaysApplied.Text = "0";
            txtAuth.Text = "0";
            txtNotAvailed.Text = "0";
            txtBalLeave.Text = "0";
            txtReason.Text = string.Empty;
            trMsg.Style["background-color"] = (string)null;
            lblMsg.Text = string.Empty;
            grdLeave.DataSource = null;
            grdLeave.DataBind();
            grdLeave.Visible = false;
        }
        drpEmpName.Focus();
    }

    private void CheckAuthLeave()
    {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        btnSave.Enabled = true;
        if (Convert.ToInt32(obj.ExecuteScalarQry("SELECT COUNT(*) FROM dbo.HR_EmpLeaveDtls WHERE EmpId=" + drpEmpName.SelectedValue + " AND CalYear=" + ViewState["LeaveYr"])) != 0)
            return;
        string str = obj.ExecuteScalarQry("SELECT DesignationId FROM dbo.HR_EmployeeMaster WHERE EmpId=" + drpEmpName.SelectedValue);
        DataTable dataTableQry = obj.GetDataTableQry("SELECT LeaveId,TotDaysAuth FROM dbo.HR_LeaveAuthorized WHERE DesignationId=" + str);
        if (dataTableQry.Rows.Count > 0)
        {
            foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
                LeaveAuthEmpwise(Convert.ToInt32(str), Convert.ToInt32(ViewState["LeaveYr"]), Convert.ToInt32(row["LeaveId"]), (float)Convert.ToInt32(row["TotDaysAuth"]));
        }
        else
            btnSave.Enabled = false;
    }

    private void CheckAuthLeaveAll()
    {
        string str = obj.ExecuteScalar("HR_LeaveAuthAll", new Hashtable()
    {
      {
         "@CalYear",
         Convert.ToInt32(ViewState["LeaveYr"])
      },
      {
         "@UserId",
        Session["User_Id"]
      }
    });
        if (!(str.Trim() != "") || !(str.Trim().ToUpper() == "NA"))
            return;
        trMsg.Style["background-color"] = "Red";
        lblMsg.Text = "Please Authorize Leave For all Designations!!";
    }

    protected void dtpDate_SelectionChanged(object sender, EventArgs e)
    {
        string str1 = string.Empty;
        if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL")
        {
            str1 = DateTime.Now.Year.ToString();
            ViewState["Yr"] = str1;
        }
        else if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "SESS")
        {
            string str2 = obj.ExecuteScalarQry("select dbo.fuGetSessionYr('" + dtpDate.GetDateValue().ToString("dd-MMM-yyyy") + "')");
            ViewState["Yr"] = str2;
            string[] strArray = str2.Split('-');
            str1 = strArray[0] + strArray[1];
        }
        lblYear.Text = ViewState["Yr"].ToString();
        ViewState["LeaveYr"] = str1;
        if (drpEmpName.SelectedIndex > 0)
        {
            CheckAuthLeave();
            CheckAvlLeave();
        }
        txtDaysApplied.Focus();
    }

    protected void grdLeave_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        Label control1 = e.Row.FindControl("lblBalance") as Label;
        Button control2 = e.Row.FindControl("btnSelect") as Button;
        if (!(control2.CommandArgument != "5") || !(control1.Text == "0"))
            return;
        control2.Visible = false;
    }
}