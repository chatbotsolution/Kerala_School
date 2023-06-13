using ASP;
using RJS.Web.WebControl;
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


public partial class HR_LeaveEncashment : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!ChkIsHRUsed())
                Response.Redirect("HRHome.aspx?IsHRUsed=0");
            else
                bindEmployee();
        }
        lblMsg.Text = string.Empty;
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void bindEmployee()
    {
        drpEmpName.DataSource = obj.GetDataTable("HR_GetEmpList");
        drpEmpName.DataTextField = "Employee";
        drpEmpName.DataValueField = "EmpId";
        drpEmpName.DataBind();
        drpEmpName.Items.Insert(0, new ListItem("- SELECT -", "0"));
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

    protected void drpEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtLeaveAvail.Text = "0";
        txtMaxEncashment.Text = "0";
        txtPending.Text = "0";
        txtTotEncashed.Text = "0";
        txtEncash.Text = "0";
        if (drpEmpName.SelectedIndex > 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (drpEmpName.SelectedIndex > 0)
                ViewState["gender"] = obj.ExecuteScalarQry("SELECT Sex FROM dbo.HR_EmployeeMaster WHERE EmpId=" + drpEmpName.SelectedValue);
            if (ViewState["gender"].ToString().Trim().ToUpper() == "MALE")
            {
                stringBuilder.Append("SELECT LeaveId,LeaveCode+' ('+LeaveDesc+')' AS LeaveDesc");
                stringBuilder.Append(" FROM dbo.HR_LeaveMaster WHERE  CFAllowed=1 and LeaveId NOT IN (4) ORDER BY LeaveId");
            }
            else
            {
                stringBuilder.Append("SELECT LeaveId,LeaveCode+' ('+LeaveDesc+')' AS LeaveDesc");
                stringBuilder.Append(" FROM dbo.HR_LeaveMaster WHERE  CFAllowed=1 and LeaveId NOT IN (4)");
                stringBuilder.Append(" UNION");
                stringBuilder.Append(" SELECT LeaveId,LeaveCode+' ('+LeaveDesc+')' AS LeaveDesc");
                stringBuilder.Append(" FROM dbo.HR_LeaveMaster WHERE  CFAllowed=1 and LeaveId IN (4) ORDER BY LeaveId");
            }
            bindDropDown(drpLeave, stringBuilder.ToString().Trim(), "LeaveDesc", "LeaveId");
        }
        else
            drpLeave.Items.Clear();
        drpEmpName.Focus();
    }

    protected void drpLeave_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetLeaveDtls();
        drpLeave.Focus();
    }

    private void GetLeaveDtls()
    {
        if (drpEmpName.SelectedIndex > 0 && drpLeave.SelectedIndex > 0)
        {
            DataTable dataTable = obj.GetDataTable("HR_GetLeaveForEncash", new Hashtable()
      {
        {
           "@EmpId",
           drpEmpName.SelectedValue
        },
        {
           "@LeaveId",
           drpLeave.SelectedValue
        }
      });
            if (dataTable.Rows.Count > 0)
            {
                txtLeaveAvail.Text = (float.Parse(dataTable.Rows[0]["AuthDays"].ToString()) - (float.Parse(dataTable.Rows[0]["AvlDays"].ToString()) + float.Parse(dataTable.Rows[0]["TotPending"].ToString()))).ToString();
                txtMaxEncashment.Text = dataTable.Rows[0]["EncashAllowed"].ToString();
                txtPending.Text = dataTable.Rows[0]["TotPending"].ToString();
                txtTotEncashed.Text = dataTable.Rows[0]["TotEncashed"].ToString();
                txtEncash.Text = "0";
            }
            else
            {
                txtLeaveAvail.Text = "0";
                txtMaxEncashment.Text = "0";
                txtPending.Text = "0";
                txtTotEncashed.Text = "0";
                txtEncash.Text = "0";
            }
        }
        else
        {
            txtLeaveAvail.Text = "0";
            txtMaxEncashment.Text = "0";
            txtPending.Text = "0";
            txtTotEncashed.Text = "0";
            txtEncash.Text = "0";
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (obj.ExecuteScalar("HR_InsUpdtLeaveEncashment", new Hashtable()
    {
      {
         "@EmpId",
         drpEmpName.SelectedValue
      },
      {
         "@LeaveId",
         drpLeave.SelectedValue
      },
      {
         "@UserId",
        Session["User_Id"]
      },
      {
         "@AppliedDays",
         txtEncash.Text
      },
      {
         "@AppliedDate",
         dtpAppliedDt.GetDateValue()
      }
    }).Trim() == string.Empty)
        {
            Clear();
            trMsg.Style["background-color"] = "Green";
            lblMsg.Text = "Data Saved Successfully";
        }
        else
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Failed to Save.";
            btnSave.Focus();
        }
    }

    private void Clear()
    {
        lblMsg.Text = string.Empty;
        drpEmpName.SelectedIndex = 0;
        drpLeave.Items.Clear();
        txtLeaveAvail.Text = "0";
        txtMaxEncashment.Text = "0";
        txtPending.Text = "0";
        txtTotEncashed.Text = "0";
        txtEncash.Text = "0";
        txtAppliedDt.Text = string.Empty;
        drpEmpName.Focus();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
    }
}