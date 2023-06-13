using AjaxControlToolkit;
using ASP;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_LeaveClaimAppList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = "Transparent";
        lblMsg.Text = string.Empty;
        trMessage.Style["background-color"] = "Transparent";
        lblMessage.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        if (!ChkIsHRUsed())
        {
            Response.Redirect("HRHome.aspx?IsHRUsed=0");
        }
        else
        {
            BindEmployee(drpEmp);
            GetPendingClaim();
            ViewState["LeaveYr"] = GetLeaveYear();
        }
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
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

    private void BindEmployee(DropDownList drp)
    {
        DataTable dataTable = obj.GetDataTable("HR_GetEmpList");
        drp.DataSource = dataTable;
        drp.DataTextField = "Employee";
        drp.DataValueField = "EmpId";
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    private void GetPendingClaim()
    {
        DataTable dataTable = obj.GetDataTable("HR_GetClaimForApproval");
        grdLeave.DataSource = dataTable;
        grdLeave.DataBind();
        lblRecords.Text = "No of Record(s) : " + dataTable.Rows.Count;
    }

    protected void grdLeave_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandName == "Approve"))
            return;
        mv1.ActiveViewIndex = 1;
        DataSet dataSet = obj.GetDataSet("HR_GetClaimForApproval", new Hashtable()
    {
      {
         "@ClaimId",
         Convert.ToInt32(e.CommandArgument)
      },
      {
         "@CalYear",
         Convert.ToInt32(ViewState["LeaveYr"])
      }
    });
        DataTable table1 = dataSet.Tables[0];
        DataTable table2 = dataSet.Tables[1];
        lblName.Text = table1.Rows[0]["EmpName"].ToString();
        lblDate.Text = Convert.ToDateTime(table1.Rows[0]["ClaimDate"]).ToString("dd-MMM-yyyy");
        lblLeaveType.Text = table1.Rows[0]["LeaveCode"].ToString();
        lblExtraDays.Text = table1.Rows[0]["WPL"].ToString();
        lblReason.Text = table1.Rows[0]["Reason"].ToString();
        txtDaysApproved.Text = lblExtraDays.Text;
        hfId.Value = e.CommandArgument.ToString();
        hfLeaveApplyId.Value = table1.Rows[0]["LeaveApplyId"].ToString();
        hfEmpId.Value = table1.Rows[0]["EmpId"].ToString();
        ViewState["ClaimDate"] = table1.Rows[0]["ClaimDate"];
        grdAvlLeave.DataSource = table2;
        grdAvlLeave.DataBind();
        txtDaysApproved.Focus();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = "Transparent";
        lblMsg.Text = string.Empty;
        trMessage.Style["background-color"] = "Transparent";
        lblMessage.Text = string.Empty;
        if (rbApproved.Checked && Convert.ToDecimal(txtDaysApproved.Text) <= new Decimal(0))
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Enter No of Days Approved";
            txtDaysApproved.Focus();
        }
        else if (Convert.ToDecimal(txtDaysApproved.Text) > Convert.ToDecimal(lblExtraDays.Text))
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "No of Approved Days shouldn't exceed Extra Days Availed";
            txtDaysApproved.Focus();
        }
        else if (!ChkAdjDays())
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Adjusted Days shouldn't  be more than the Days Approved.";
            txtDaysApproved.Focus();
        }
        else if (txtDate.Text == string.Empty)
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Enter Approved/Rejected Date";
            txtDate.Focus();
        }
        else if (!dateChk(txtDate))
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Enter Approved Date in correct format i.e- (DD-MM-YYYY)";
            txtDate.Focus();
        }
        else if (txtRemarks.Text.Trim() == string.Empty)
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Enter Approved/Rejectd Remarks";
            txtRemarks.Focus();
        }
        else
        {
            string[] strArray = txtDate.Text.Trim().Split(new char[3]
      {
        '-',
        '.',
        '/'
      });
            string str1 = new DateTime(Convert.ToInt32(strArray[2]), Convert.ToInt32(strArray[1]), Convert.ToInt32(strArray[0])).ToString("MMM", (IFormatProvider)CultureInfo.InvariantCulture);
            DateTime dateTime = Convert.ToDateTime(strArray[0] + "-" + str1 + "-" + strArray[2]);
            if (dateTime < Convert.ToDateTime(ViewState["ClaimDate"]))
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Approved Date should not be less than Claim Date";
                txtDate.Focus();
            }
            else
            {
                string str2 = txtRemarks.Text;
                if (ViewState["AdjDays"].ToString().Trim() != string.Empty && Convert.ToDecimal(ViewState["AdjDays"]) > new Decimal(0))
                    str2 = str2 + ", Leave Adjusted - " + ViewState["AdjRemarks"].ToString();
                Hashtable hashtable = new Hashtable();
                hashtable.Add("@EmpId", hfEmpId.Value);
                hashtable.Add("@ClaimId", hfId.Value);
                hashtable.Add("@LeaveApplyId", hfLeaveApplyId.Value);
                hashtable.Add("@ApprovedDate", Convert.ToDateTime(dateTime).ToString("dd-MMM-yyyy"));
                hashtable.Add("@CalYear", Convert.ToInt32(ViewState["LeaveYr"]));
                if (rbApproved.Checked)
                {
                    hashtable.Add("@Approved_Rejected", "Approved");
                    hashtable.Add("@ClaimStatus", "Approved");
                    hashtable.Add("@DaysApproved", Convert.ToDecimal(txtDaysApproved.Text));
                }
                else
                {
                    hashtable.Add("@Approved_Rejected", "Rejected");
                    hashtable.Add("@ClaimStatus", "Rejected");
                }
                hashtable.Add("@ApprovedByUserId", Session["User_Id"]);
                hashtable.Add("@Remarks", str2.Trim());
                string str3 = obj.ExecuteScalar("HR_InsUpdtLeaveClaim", hashtable);
                if (str3.Trim().ToUpper() == "S")
                {
                    if (ViewState["AdjDays"].ToString().Trim() != string.Empty && Convert.ToDecimal(ViewState["AdjDays"]) > new Decimal(0))
                        AdjustLeave();
                    ResetFields();
                    trMessage.Style["background-color"] = "Green";
                    lblMessage.Text = "Data Saved Successfully";
                    mv1.ActiveViewIndex = 0;
                    GetPendingClaim();
                }
                else
                {
                    trMsg.Style["background-color"] = "Red";
                    if (str3.Trim().ToUpper() == "SAL")
                    {
                        lblMsg.Text = "Salary Already Generated for the selected month!!";
                    }
                    else if (str3.Trim().ToUpper() == "SALN")
                    {
                        lblMsg.Text = "Salary Not Generated for the month for which leave is being availed!!";
                    }
                    else
                    {
                        lblMsg.Text = "Failed to Save Data. Try Again.";
                    }
                }
            }
        }
    }

    private bool ChkAdjDays()
    {
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        string str = string.Empty;
        if (txtDaysApproved.Text.Trim() != string.Empty)
            num1 = Convert.ToDecimal(txtDaysApproved.Text);
        foreach (GridViewRow row in grdAvlLeave.Rows)
        {
            TextBox control1 = row.FindControl("txtAdjLeave") as TextBox;
            Label control2 = row.FindControl("lblLeaveCode") as Label;
            if (control1.Text.Trim() != string.Empty)
            {
                num2 += Convert.ToDecimal(control1.Text);
                if (Convert.ToDecimal(control1.Text) > new Decimal(0))
                    str = str + control2.Text.Trim() + " - " + control1.Text.Trim() + " days,";
            }
        }
        ViewState["AdjDays"] = num2;
        ViewState["AdjRemarks"] = str;
        return !(num2 > num1);
    }

    private void AdjustLeave()
    {
        string empty = string.Empty;
        foreach (GridViewRow row in grdAvlLeave.Rows)
        {
            HiddenField control1 = row.FindControl("hfLeaveId") as HiddenField;
            TextBox control2 = row.FindControl("txtAdjLeave") as TextBox;
            if (control2.Text.Trim() != string.Empty && (double)float.Parse(control2.Text) > 0.0)
                obj.ExecuteScalar("HR_LeaveAdjustment", new Hashtable()
        {
          {
             "@EmpId",
             hfEmpId.Value
          },
          {
             "@LeaveId",
             control1.Value
          },
          {
             "@CalYear",
             Convert.ToInt32(ViewState["LeaveYr"])
          },
          {
             "@AdjDays",
             float.Parse(control2.Text)
          }
        });
        }
    }

    private bool dateChk(TextBox txtDt)
    {
        try
        {
            string[] strArray = txtDt.Text.Trim().Split(new char[3]
      {
        '-',
        '.',
        '/'
      });
            string str = obj.ExecuteScalarQry("SELECT UPPER(LEFT(DATENAME(MONTH," + strArray[1] + "),3))");
            Convert.ToDateTime(strArray[0] + "-" + str + "-" + strArray[2]);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetFields();
        mv1.ActiveViewIndex = 0;
        GetPendingClaim();
    }

    private void ResetFields()
    {
        ViewState["LeaveEndDt"] = null;
        trMsg.Style["background-color"] = "Transparent";
        lblMsg.Text = string.Empty;
        trMessage.Style["background-color"] = "Transparent";
        lblMessage.Text = string.Empty;
        txtDate.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        hfLeaveApplyId.Value = "0";
        hfId.Value = "0";
        hfEmpId.Value = "0";
        grdAvlLeave.DataSource = null;
        grdAvlLeave.DataBind();
        ViewState["AdjDays"] = "0";
        ViewState["ClaimDate"] = null;
    }

    protected void rbReject_CheckedChanged(object sender, EventArgs e)
    {
        ResetGrid();
        if (rbApproved.Checked)
        {
            txtDaysApproved.Text = lblExtraDays.Text;
            txtDaysApproved.Enabled = true;
            grdAvlLeave.Enabled = true;
        }
        else
        {
            txtDaysApproved.Text = "0";
            txtDaysApproved.Enabled = false;
            grdAvlLeave.Enabled = false;
        }
    }

    protected void rbApproved_CheckedChanged(object sender, EventArgs e)
    {
        ResetGrid();
        if (rbApproved.Checked)
        {
            txtDaysApproved.Text = lblExtraDays.Text;
            txtDaysApproved.Enabled = true;
            grdAvlLeave.Enabled = true;
        }
        else
        {
            txtDaysApproved.Text = "0";
            txtDaysApproved.Enabled = false;
            grdAvlLeave.Enabled = false;
        }
    }

    private void ResetGrid()
    {
        foreach (Control row in grdAvlLeave.Rows)
            (row.FindControl("txtAdjLeave") as TextBox).Text = "0";
    }

    protected void rbClaim_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbClaim.SelectedValue == "L")
        {
            ClearFields();
            GetPendingClaim();
            mv1.ActiveViewIndex = 0;
        }
        else
        {
            if (!(rbClaim.SelectedValue == "S"))
                return;
            mv1.ActiveViewIndex = 2;
        }
    }

    protected void drpEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpSalary.Items.Clear();
        btnSubmit.Enabled = false;
        trMsg1.Style["background-color"] = "Transparent";
        lblMsg1.Text = string.Empty;
        if (drpEmp.SelectedIndex > 0)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = obj.GetDataTable("HR_GetWithheldSalList", new Hashtable()
      {
        {
           "@EmpId",
           drpEmp.SelectedValue
        }
      });
            if (dataTable2.Rows.Count > 0)
            {
                drpSalary.DataSource = dataTable2;
                drpSalary.DataSource = dataTable2;
                drpSalary.DataTextField = "Salary";
                drpSalary.DataValueField = "SalId";
                drpSalary.DataBind();
                drpSalary.Items.Insert(0, new ListItem("Month - Year - Payable Amount", "0"));
                btnSubmit.Enabled = true;
            }
            else
                btnSubmit.Enabled = false;
        }
        drpEmp.Focus();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        trMsg1.Style["background-color"] = "Transparent";
        lblMsg1.Text = string.Empty;
        string empty = string.Empty;
        if (!dateChk(txtAppDate))
        {
            trMsg1.Style["background-color"] = "Red";
            lblMsg1.Text = "Enter Approved Date in correct format i.e- (DD-MM-YYYY)";
            txtAppDate.Focus();
        }
        else
        {
            string str1 = chkInput();
            if (str1.Trim() == string.Empty)
            {
                string[] strArray = txtAppDate.Text.Trim().Split(new char[3]
        {
          '-',
          '.',
          '/'
        });
                string str2 = obj.ExecuteScalar("HR_ApprWithheldSalary", new Hashtable()
        {
          {
             "@EmpId",
             drpEmp.SelectedValue
          },
          {
             "@SalId",
             drpSalary.SelectedValue
          },
          {
             "@ApprovedDate",
             Convert.ToDateTime(strArray[1] + "-" + strArray[0] + "-" + strArray[2])
          },
          {
             "@PaidAmt",
             Convert.ToDecimal(txtAppAmt.Text.Trim())
          }
        });
                if (str2.Trim().ToUpper() == "F")
                {
                    trMsg1.Style["background-color"] = "Red";
                    lblMsg1.Text = "Failed to Save Data.";
                }
                else
                {
                    ClearFields();
                    trMsg1.Style["background-color"] = "Green";
                    lblMsg1.Text = str2;
                }
            }
            else
            {
                trMsg1.Style["background-color"] = "Red";
                lblMsg1.Text = str1.Trim();
            }
        }
    }

    private string chkInput()
    {
        string str = string.Empty;
        string[] strArray = txtAppDate.Text.Trim().Split(new char[3]
    {
      '-',
      '.',
      '/'
    });
        int int32_1 = Convert.ToInt32(strArray[1].ToString().Trim());
        int int32_2 = Convert.ToInt32(strArray[2].ToString().Trim());
        int num = 0;
        DataTable dataTableQry = obj.GetDataTableQry("SELECT Month,Year,GrossTot FROM dbo.HR_EmpMlySalary WHERE SalId=" + drpSalary.SelectedValue);
        string upper = dataTableQry.Rows[0]["Month"].ToString().Trim().ToUpper();
        int int32_3 = Convert.ToInt32(dataTableQry.Rows[0]["Year"].ToString().Trim());
        if (Convert.ToDouble(txtAppAmt.Text) > Convert.ToDouble(dataTableQry.Rows[0]["GrossTot"].ToString().Trim()))
        {
            str = "Approved Amount shouldn't be more than Pending Amount";
            txtAppAmt.Focus();
        }
        else
        {
            if (upper == "JAN")
                num = 1;
            else if (upper == "FEB")
                num = 2;
            else if (upper == "MAR")
                num = 3;
            else if (upper == "APR")
                num = 4;
            else if (upper == "MAY")
                num = 5;
            else if (upper == "JUN")
                num = 6;
            else if (upper == "JUL")
                num = 7;
            else if (upper == "AUG")
                num = 8;
            else if (upper == "SEP")
                num = 9;
            else if (upper == "OCT")
                num = 10;
            else if (upper == "NOV")
                num = 11;
            else if (upper == "DEC")
                num = 12;
            if (int32_1 < num && int32_2 <= int32_3)
            {
                str = "Approved Date shouldn't be the Previous Date";
                txtAppDate.Focus();
            }
        }
        return str;
    }

    private void ClearFields()
    {
        trMsg1.Style["background-color"] = "Transparent";
        lblMsg1.Text = string.Empty;
        drpEmp.SelectedIndex = 0;
        drpSalary.Items.Clear();
        txtAppAmt.Text = "0";
        txtAppDate.Text = string.Empty;
        btnSubmit.Enabled = false;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    protected void grdAvlLeave_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        Label control1 = e.Row.FindControl("lblBalance") as Label;
        TextBox control2 = e.Row.FindControl("txtAdjLeave") as TextBox;
        control2.Attributes.Add("onkeyup", "javascript:return checkAdjLeave('" + control1.Text + "', '" + control2.ClientID + "');");
    }
}