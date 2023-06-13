using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_LeaveEncashmentList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!ChkIsHRUsed())
            {
                Response.Redirect("HRHome.aspx?IsHRUsed=0");
            }
            else
            {
                GetEncashmentList();
                bindDropDown(drpBank, "select AcctsHeadId, AcctsHead from dbo.Acts_AccountHeads where AG_Code=7", "AcctsHead", "AcctsHeadId");
                drpBank.Items.RemoveAt(0);
                drpBank.Items.Insert(0, new ListItem("- SELECT -", "0"));
                drpPmtMode.SelectedIndex = 1;
                ViewState["AppDate"] = null;
            }
        }
        trMsg.Style["background-color"] = trMessage.Style["background-color"] = "Transparent";
        lblMsg.Text = lblMessage.Text = string.Empty;
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.Items.Clear();
        DataTable dataTable = new DataTable();
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        if (dataTableQry.Rows.Count <= 0)
            return;
        drp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    private void GetEncashmentList()
    {
        if (drpStatus.SelectedValue == "P")
        {
            grdLeave.Columns[4].Visible = false;
            grdLeave.Columns[4].HeaderText = "Approved Date";
            grdLeave.Columns[5].Visible = false;
            grdLeave.Columns[6].Visible = false;
            grdLeave.Columns[7].Visible = false;
            grdLeave.Columns[8].Visible = false;
            grdLeave.Columns[9].Visible = false;
            grdLeave.Columns[10].Visible = true;
            grdLeave.Columns[11].Visible = true;
            grdLeave.Columns[12].Visible = true;
        }
        else if (drpStatus.SelectedValue == "A")
        {
            grdLeave.Columns[4].Visible = true;
            grdLeave.Columns[4].HeaderText = "Approved Date";
            grdLeave.Columns[5].Visible = true;
            grdLeave.Columns[6].Visible = true;
            grdLeave.Columns[7].Visible = true;
            grdLeave.Columns[8].Visible = true;
            grdLeave.Columns[9].Visible = false;
            grdLeave.Columns[10].Visible = false;
            grdLeave.Columns[11].Visible = true;
            grdLeave.Columns[12].Visible = false;
        }
        else if (drpStatus.SelectedValue == "E")
        {
            grdLeave.Columns[4].Visible = true;
            grdLeave.Columns[4].HeaderText = "Approved Date";
            grdLeave.Columns[5].Visible = true;
            grdLeave.Columns[6].Visible = true;
            grdLeave.Columns[7].Visible = true;
            grdLeave.Columns[8].Visible = false;
            grdLeave.Columns[9].Visible = true;
            grdLeave.Columns[10].Visible = false;
            grdLeave.Columns[11].Visible = false;
            grdLeave.Columns[12].Visible = false;
        }
        else if (drpStatus.SelectedValue == "R")
        {
            grdLeave.Columns[4].Visible = true;
            grdLeave.Columns[4].HeaderText = "Rejected Date";
            grdLeave.Columns[5].Visible = false;
            grdLeave.Columns[6].Visible = true;
            grdLeave.Columns[7].Visible = false;
            grdLeave.Columns[8].Visible = false;
            grdLeave.Columns[9].Visible = false;
            grdLeave.Columns[10].Visible = false;
            grdLeave.Columns[11].Visible = false;
            grdLeave.Columns[12].Visible = false;
        }
        DataTable dataTable = obj.GetDataTable("HR_LeaveEncashmentList", new Hashtable()
    {
      {
         "@action",
         drpStatus.SelectedValue
      }
    });
        grdLeave.DataSource = dataTable;
        grdLeave.DataBind();
        lblRecords.Text = "No of Record(s) : " + dataTable.Rows.Count;
    }

    protected void grdLeave_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Approve")
        {
            mv1.ActiveViewIndex = 1;
            tr1.Visible = true;
            tr2.Visible = true;
            tr3.Visible = true;
            tr4.Visible = true;
            DataTable dataTable = obj.GetDataTable("HR_LeaveEncashmentList", new Hashtable()
      {
        {
           "@action",
           drpStatus.SelectedValue
        },
        {
           "@Id",
          e.CommandArgument
        }
      });
            lblName.Text = dataTable.Rows[0]["EmpName"].ToString();
            lblLeaveType.Text = dataTable.Rows[0]["LeaveCode"].ToString() + " (" + dataTable.Rows[0]["LeaveDesc"].ToString() + ")";
            lblDate.Text = Convert.ToDateTime(dataTable.Rows[0]["AppliedDate"]).ToString("dd-MMM-yyyy");
            lblAppliedDays.Text = dataTable.Rows[0]["AppliedDays"].ToString();
            txtSalary.Text = (Convert.ToDecimal(dataTable.Rows[0]["GrossTot"]) - Convert.ToDecimal(dataTable.Rows[0]["TotDeduction"])).ToString("0.00");
            hfId.Value = e.CommandArgument.ToString();
            rbAppRej.SelectedIndex = 0;
            txtDate.Focus();
        }
        else if (e.CommandName == "Reject")
        {
            mv1.ActiveViewIndex = 1;
            tr1.Visible = false;
            tr2.Visible = false;
            tr3.Visible = false;
            tr4.Visible = false;
            DataTable dataTable = obj.GetDataTable("HR_LeaveEncashmentList", new Hashtable()
      {
        {
           "@action",
           drpStatus.SelectedValue
        },
        {
           "@Id",
          e.CommandArgument
        }
      });
            lblName.Text = dataTable.Rows[0]["EmpName"].ToString();
            lblLeaveType.Text = dataTable.Rows[0]["LeaveCode"].ToString() + " (" + dataTable.Rows[0]["LeaveDesc"].ToString() + ")";
            lblDate.Text = Convert.ToDateTime(dataTable.Rows[0]["AppliedDate"]).ToString("dd-MMM-yyyy");
            lblAppliedDays.Text = dataTable.Rows[0]["AppliedDays"].ToString();
            hfId.Value = e.CommandArgument.ToString();
            rbAppRej.SelectedIndex = 1;
            txtDate.Focus();
        }
        else
        {
            if (!(e.CommandName == "Remove"))
                return;
            obj.ExecuteScalarQry("DELETE FROM dbo.HR_LeaveEncashment WHERE Id=" + e.CommandArgument.ToString());
            GetEncashmentList();
            trMessage.Style["background-color"] = "Green";
            lblMessage.Text = "Record Deleted Successfully";
            drpStatus.Focus();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = "Transparent";
        lblMsg.Text = string.Empty;
        trMessage.Style["background-color"] = "Transparent";
        lblMessage.Text = string.Empty;
        if (txtDate.Text == string.Empty)
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
        else if (rbAppRej.SelectedIndex == 0 && Convert.ToDecimal(txtDaysApproved.Text) <= new Decimal(0))
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Enter No of Days Approved";
            txtDaysApproved.Focus();
        }
        else if (Convert.ToDecimal(txtDaysApproved.Text) > Convert.ToDecimal(lblAppliedDays.Text))
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "No of Approved Days shouldn't exceed Applied Days";
            txtDaysApproved.Focus();
        }
        else if (rbAppRej.SelectedIndex == 0 && Convert.ToDecimal(txtPayableAmt.Text) <= new Decimal(0))
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Payable Amount should be more than Zero.";
            txtPayableAmt.Focus();
        }
        else if (rbAppRej.SelectedIndex == 0 && drpPmtMode.SelectedIndex == 1 && txtPaymentDate.Text.Trim() == string.Empty)
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Enter Payment Date";
            txtPaymentDate.Focus();
        }
        else if (rbAppRej.SelectedIndex == 0 && drpPmtMode.SelectedIndex == 1 && !dateChk(txtPaymentDate))
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Enter Payment Date in correct format i.e- (DD-MM-YYYY)";
            txtPaymentDate.Focus();
        }
        else if (rbAppRej.SelectedIndex == 0 && drpPmtMode.SelectedIndex == 1 && (rbPmtMode.SelectedIndex == 1 && drpBank.SelectedIndex <= 0))
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Select a Bank Name";
            drpBank.Focus();
        }
        else if (rbAppRej.SelectedIndex == 0 && drpPmtMode.SelectedIndex == 1 && (rbPmtMode.SelectedIndex == 1 && txtInstrNo.Text.Trim() == string.Empty))
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Enter Instrument No";
            txtInstrNo.Focus();
        }
        else if (txtRemarks.Text.Trim() == string.Empty)
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Enter Approved/Rejectd Remarks";
            txtRemarks.Focus();
        }
        else
        {
            string[] strArray1 = txtDate.Text.Trim().Split(new char[3]
      {
        '-',
        '.',
        '/'
      });
            string str1 = new DateTime(Convert.ToInt32(strArray1[2]), Convert.ToInt32(strArray1[1]), Convert.ToInt32(strArray1[0])).ToString("MMM", (IFormatProvider)CultureInfo.InvariantCulture);
            string str2 = strArray1[0] + "-" + str1 + "-" + strArray1[2];
            DateTime dateTime1 = Convert.ToDateTime(str2);
            if (drpPmtMode.SelectedIndex == 1)
            {
                string[] strArray2 = txtPaymentDate.Text.Trim().Split(new char[3]
        {
          '-',
          '.',
          '/'
        });
                string str3 = new DateTime(Convert.ToInt32(strArray2[2]), Convert.ToInt32(strArray2[1]), Convert.ToInt32(strArray2[0])).ToString("MMM", (IFormatProvider)CultureInfo.InvariantCulture);
                str2 = strArray2[0] + "-" + str3 + "-" + strArray2[2];
            }
            DateTime dateTime2 = Convert.ToDateTime(str2);
            if (dateTime1 < Convert.ToDateTime(lblDate.Text.Trim()))
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Approved Date should not be less than Applied Date";
                txtDate.Focus();
            }
            else if (rbAppRej.SelectedIndex == 0 && drpPmtMode.SelectedIndex == 1 && dateTime2 < dateTime1)
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Payment Date should not be less than Approved Date";
                txtDate.Focus();
            }
            else
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Add("@Id", hfId.Value);
                hashtable.Add("@SchoolId", Session["SchoolId"]);
                hashtable.Add("@UserId", Session["User_Id"]);
                hashtable.Add("@AppRejDate", dateTime1.ToString("dd-MMM-yyyy"));
                hashtable.Add("@Remarks", txtRemarks.Text.Trim());
                if (rbAppRej.SelectedIndex == 0)
                {
                    hashtable.Add("@Approved_Rejected", "Approved");
                    hashtable.Add("@ApprovedDays", txtDaysApproved.Text);
                    hashtable.Add("@PaidAmount", txtPayableAmt.Text);
                    hashtable.Add("@PmtMode", drpPmtMode.SelectedValue);
                    if (drpPmtMode.SelectedIndex == 1)
                    {
                        hashtable.Add("@PaidDate", dateTime2.ToString("dd-MMM-yyyy"));
                        if (rbPmtMode.SelectedIndex == 1)
                        {
                            hashtable.Add("@CrHeadId", drpBank.SelectedValue);
                            hashtable.Add("@InstNo", txtInstrNo.Text.Trim());
                            hashtable.Add("@Cash_Bank", "Bank");
                        }
                        else
                        {
                            hashtable.Add("@Cash_Bank", "Cash");
                            hashtable.Add("@CrHeadId", 3);
                        }
                    }
                }
                else
                    hashtable.Add("@Approved_Rejected", "Rejected");
                if (obj.ExecuteScalar("HR_InsUpdtLeaveEncashment", hashtable).Trim() == string.Empty)
                {
                    GetEncashmentList();
                    ResetFields();
                    mv1.ActiveViewIndex = 0;
                    trMsg.Style["background-color"] = "Green";
                    lblMsg.Text = rbAppRej.SelectedIndex != 0 ? "Rejected Successfully" : "Approved Successfully";
                    drpStatus.Focus();
                }
                else
                {
                    trMsg.Style["background-color"] = "Red";
                    lblMsg.Text = "Transaction Failed. Try Again.";
                    btnSave.Focus();
                }
            }
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
            if (Convert.ToInt32(strArray[1]) > 0 && Convert.ToInt32(strArray[1]) < 13)
            {
                string str = obj.ExecuteScalarQry("SELECT DATENAME(mm,DATEADD(mm," + strArray[1] + ",-1))");
                ViewState["AppDate"] = Convert.ToDateTime(strArray[0] + "-" + str + "-" + strArray[2]).ToString("dd-MMM-yyyy");
                return true;
            }
            ViewState["AppDate"] = null;
            return false;
        }
        catch (Exception ex)
        {
            ViewState["AppDate"] = null;
            return false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetFields();
        mv1.ActiveViewIndex = 0;
        GetEncashmentList();
    }

    private void ResetFields()
    {
        trMsg.Style["background-color"] = "Transparent";
        trMessage.Style["background-color"] = "Transparent";
        lblMsg.Text = lblMessage.Text = txtDate.Text = txtPaymentDate.Text = txtInstrNo.Text = txtRemarks.Text = string.Empty;
        txtDaysApproved.Text = txtSalary.Text = txtPayableAmt.Text = "0";
        rbAppRej.SelectedIndex = 0;
        drpPmtMode.SelectedIndex = 1;
        rbPmtMode.SelectedIndex = 1;
        drpBank.SelectedIndex = 0;
        hfId.Value = "0";
        ViewState["AppDate"] = null;
        txtDaysApproved.Enabled = true;
        txtPayableAmt.Enabled = true;
        drpPmtMode.Enabled = true;
        rbPmtMode.Enabled = true;
        txtPaymentDate.Enabled = true;
        drpBank.Enabled = true;
        txtInstrNo.Enabled = true;
        drpStatus.Focus();
    }

    protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetEncashmentList();
        drpStatus.Focus();
    }

    protected void drpPmtMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        rbPmtMode.SelectedIndex = 1;
        if (drpPmtMode.SelectedIndex == 0)
        {
            rbPmtMode.Enabled = false;
            drpBank.Enabled = false;
            txtInstrNo.Enabled = false;
            txtPaymentDate.Enabled = false;
        }
        else
        {
            rbPmtMode.Enabled = true;
            drpBank.Enabled = true;
            txtInstrNo.Enabled = true;
            txtPaymentDate.Enabled = true;
        }
        drpPmtMode.Focus();
    }

    protected void rbPmtMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpBank.SelectedIndex = 0;
        if (rbPmtMode.SelectedValue == "B")
        {
            drpBank.Enabled = true;
            txtInstrNo.Enabled = true;
            txtPaymentDate.Enabled = true;
        }
        else if (rbPmtMode.SelectedValue == "C")
        {
            drpBank.Enabled = false;
            txtInstrNo.Enabled = false;
            txtPaymentDate.Enabled = true;
        }
        rbPmtMode.Focus();
    }

    protected void rbAppRej_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbAppRej.SelectedIndex == 0)
        {
            txtDaysApproved.Text = lblAppliedDays.Text;
            txtDaysApproved.Enabled = true;
            txtPayableAmt.Enabled = true;
            drpPmtMode.Enabled = true;
            rbPmtMode.Enabled = true;
            drpBank.Enabled = true;
            txtInstrNo.Enabled = true;
            txtPaymentDate.Enabled = true;
        }
        else
        {
            txtDaysApproved.Text = "0";
            txtPayableAmt.Text = "0";
            drpPmtMode.SelectedIndex = 1;
            rbPmtMode.SelectedIndex = 1;
            drpBank.SelectedIndex = 0;
            txtInstrNo.Text = string.Empty;
            txtPaymentDate.Text = string.Empty;
            txtDaysApproved.Enabled = false;
            txtPayableAmt.Enabled = false;
            drpPmtMode.Enabled = false;
            rbPmtMode.Enabled = false;
            drpBank.Enabled = false;
            txtInstrNo.Enabled = false;
            txtPaymentDate.Enabled = false;
        }
        rbAppRej.Focus();
    }

    protected void txtDaysApproved_TextChanged(object sender, EventArgs e)
    {
        CalAmt();
        txtPayableAmt.Focus();
    }

    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        dateChk(txtDate);
        CalAmt();
        if (rbAppRej.SelectedIndex == 0)
            txtDaysApproved.Focus();
        else
            txtRemarks.Focus();
    }

    private void CalAmt()
    {
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        if (txtDaysApproved.Text.Trim() != string.Empty)
            num1 = Convert.ToDouble(txtDaysApproved.Text.Trim());
        if (txtSalary.Text != string.Empty)
            num2 = Convert.ToDouble(txtSalary.Text.Trim());
        if (ViewState["AppDate"] != null)
        {
            int num4 = DateTime.DaysInMonth(Convert.ToDateTime(ViewState["AppDate"]).Year, Convert.ToDateTime(ViewState["AppDate"]).Month);
            num3 = num2 / (double)num4 * num1;
        }
        txtPayableAmt.Text = num3.ToString("0.00");
    }

    protected void grdLeave_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdLeave.PageIndex = e.NewPageIndex;
            drpStatus_SelectedIndexChanged(sender, (EventArgs)e);
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("LeaveEncashment.aspx");
    }
}