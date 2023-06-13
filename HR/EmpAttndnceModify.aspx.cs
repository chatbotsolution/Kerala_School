using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_EmpAttndnceModify : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        if (Page.IsPostBack)
            return;
        bindDropDown(drpShift, "SELECT ShiftId,ShiftCode+' ('+StartTime+' - '+EndTime+')' as ShiftCode FROM dbo.HR_EmpShift", "ShiftCode", "ShiftId");
        GetEmp();
        dtpDate.SetDateValue(DateTime.Now);
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTable = new DataTable();
        obj = new clsDAL();
        dataTable = new DataTable();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- All -", "0"));
    }

    private void GetEmp()
    {
        if (drpShift.SelectedIndex > 0)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Select e.EmpId,e.SevName from dbo.HR_EmployeeMaster e inner join HR_EmpShiftAsign s on e.EmpId=s.EmpId and s.EndDt is null where");
            stringBuilder.Append(" s.AsigndShiftId=" + drpShift.SelectedValue.Trim() + " order by SevName");
            bindDropDown(drpEmp, stringBuilder.ToString(), "SevName", "EmpId");
            drpEmp.Items.RemoveAt(0);
            drpEmp.Items.Insert(0, new ListItem("- Select -", "0"));
        }
        else
        {
            bindDropDown(drpEmp, "Select EmpId,SevName from dbo.HR_EmployeeMaster order by SevName", "SevName", "EmpId");
            drpEmp.Items.RemoveAt(0);
            drpEmp.Items.Insert(0, new ListItem("- Select -", "0"));
        }
    }

    protected void dtpDate_SelectionChanged(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = new clsDAL().GetDataTableQry("select top 1 * from dbo.HR_GenerateMlySalary order by SalGenId desc");
        if (dataTableQry.Rows.Count > 0)
        {
            if (dtpDate.GetDateValue() > Convert.ToDateTime("25 " + dataTableQry.Rows[0]["SalMonth"].ToString() + " " + dataTableQry.Rows[0]["SalYear"].ToString()))
            {
                drpEmp.Enabled = true;
                if (drpEmp.SelectedIndex > 0)
                    GetAttendance();
                else
                    ClearAll();
                CheckIfHoliday();
            }
            else
            {
                lblMsg.Text = "Salary Already Generated For This Month! Cannot Modify Attendance";
                lblMsg.ForeColor = Color.Red;
                drpEmp.SelectedIndex = 0;
                drpEmp.Enabled = false;
            }
        }
        else
        {
            if (drpEmp.SelectedIndex > 0)
                GetAttendance();
            else
                ClearAll();
            CheckIfHoliday();
        }
    }

    protected void drpShift_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetEmp();
    }

    protected void drpEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpEmp.SelectedIndex > 0)
            GetAttendance();
        else
            ClearAll();
    }

    private void GetAttendance()
    {
        obj = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@AttndDate", dtpDate.GetDateValue().ToString("dd MMM yyyy"));
        hashtable.Add("@EmpId", drpEmp.SelectedValue.Trim());
        DataTable dataTable2 = obj.GetDataTable("HR_IndvEmpAttendnc", hashtable);
        if (dataTable2.Rows.Count > 0 && dataTable2.Rows[0][0].ToString().Trim().ToUpper() != "NO" && (dataTable2.Rows[0][0].ToString().Trim().ToUpper() != "LEAVE" && dataTable2.Rows[0][0].ToString().Trim().ToUpper() != "DISC"))
        {
            if (dataTable2.Rows[0]["AttendStatus"].ToString().Trim() == "A" || dataTable2.Rows[0]["AttendStatus"].ToString().Trim() == "DL" || (dataTable2.Rows[0]["AttendStatus"].ToString().Trim() == "Off" || dataTable2.Rows[0]["AttendStatus"].ToString().Trim() == "Tour"))
            {
                txtInTime.Text = "0000";
                txtInTime.Enabled = false;
                hfIn.Value = "0000";
                hfOut.Value = "0000";
                txtOutTime.Text = "0000";
                drpStatus.SelectedValue = dataTable2.Rows[0]["AttendStatus"].ToString();
                txtOutTime.Enabled = false;
                btnSubmit.Enabled = true;
            }
            else
            {
                txtInTime.Text = dataTable2.Rows[0]["InTime"].ToString();
                hfIn.Value = dataTable2.Rows[0]["InTime"].ToString();
                txtOutTime.Text = dataTable2.Rows[0]["OutTime"].ToString();
                hfOut.Value = dataTable2.Rows[0]["OutTime"].ToString();
                drpStatus.SelectedValue = dataTable2.Rows[0]["AttendStatus"].ToString();
                txtRemarks.Text = dataTable2.Rows[0]["Remarks"].ToString();
                btnSubmit.Enabled = true;
                txtInTime.Enabled = true;
                txtOutTime.Enabled = true;
            }
        }
        else if (dataTable2.Rows[0][0].ToString().Trim().ToUpper() == "NO")
        {
            obj = new clsDAL();
            string str1 = obj.ExecuteScalarQry("Select DOJ from dbo.HR_EmployeeMaster where EmpId=" + drpEmp.SelectedValue.Trim() + " and ActiveStatus=1 and DOJ<=(select max(AttendDate) from dbo.HR_EmpAttendance where AttendStatus<>'L')");
            if (str1.Trim() != "")
            {
                obj = new clsDAL();
                string str2 = obj.ExecuteScalarQry("select max(AttendDate) from dbo.HR_EmpAttendance where AttendStatus<>'L'");
                if (dtpDate.GetDateValue().Date >= Convert.ToDateTime(str1.Trim()) && dtpDate.GetDateValue().Date <= Convert.ToDateTime(str2.Trim()))
                {
                    btnSubmit.Enabled = true;
                    txtInTime.Enabled = true;
                    txtOutTime.Enabled = true;
                }
                else
                {
                    lblMsg.Text = "Attendance Has Not Been Marked For The Day! Please Mark Attendance To Modify";
                    lblMsg.ForeColor = Color.Red;
                    ClearAll();
                }
            }
            else
            {
                lblMsg.Text = "Attendance Has Not Been Marked For The Day! Please Mark Attendance To Modify";
                lblMsg.ForeColor = Color.Red;
                ClearAll();
            }
        }
        else if (dataTable2.Rows[0][0].ToString().Trim().ToUpper() == "LEAVE")
        {
            lblMsg.Text = "Cannot Modify Attendance Status As the Employee Is On Leave!";
            lblMsg.ForeColor = Color.Red;
            txtInTime.Text = "0000";
            txtOutTime.Text = "0000";
            drpStatus.SelectedIndex = 0;
            txtRemarks.Text = "";
            btnSubmit.Enabled = false;
        }
        else if (dataTable2.Rows[0][0].ToString().Trim().ToUpper() == "DISC")
        {
            lblMsg.Text = "Cannot Modify Attendance Status As the Employee Has Been Discharged!";
            lblMsg.ForeColor = Color.Red;
            txtInTime.Text = "0000";
            txtOutTime.Text = "0000";
            drpStatus.SelectedIndex = 0;
            txtRemarks.Text = "";
            btnSubmit.Enabled = false;
        }
        else
        {
            lblMsg.Text = "Unable To Fetch Employee Attendnce Details ! Please Try again";
            lblMsg.ForeColor = Color.Red;
            btnSubmit.Enabled = false;
        }
    }

    private void CheckIfHoliday()
    {
        obj = new clsDAL();
        if (obj.ExecuteScalarQry("select HolidayName from dbo.HR_HolidayMaster where HolidayDate='" + dtpDate.GetDateValue().ToString("dd MMM yyyy") + "' and Is_Working=0").Trim() != "")
        {
            btnSubmit.Enabled = false;
            drpEmp.SelectedIndex = 0;
            drpEmp.Enabled = false;
            lblMsg.Text = "Attendance Cannot Be Modified On a Holiday!!";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            btnSubmit.Enabled = true;
            drpEmp.SelectedIndex = 0;
            drpEmp.Enabled = true;
            lblMsg.Text = "";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (validateTime().Trim() == "")
        {
            Hashtable hashtable = new Hashtable();
            obj = new clsDAL();
            hashtable.Add("AttendDate", dtpDate.GetDateValue().ToString("dd MMM yyyy"));
            hashtable.Add("EmpId", drpEmp.SelectedValue.Trim());
            if (drpStatus.SelectedValue != "A" && drpStatus.SelectedValue != "DL" && (drpStatus.SelectedValue != "Off" && drpStatus.SelectedValue != "Tour"))
            {
                hashtable.Add("InTime", txtInTime.Text.Trim());
                hashtable.Add("OutTime", txtOutTime.Text.Trim());
            }
            hashtable.Add("AttendStatus", drpStatus.SelectedValue.Trim());
            hashtable.Add("Remarks", txtRemarks.Text.Trim());
            hashtable.Add("UserId", Session["User_Id"]);
            string str = obj.ExecuteScalar("HR_InstEmpAttendanceAfterDelete", hashtable);
            if (str.Trim().ToUpper() != "S" && str.Trim().ToUpper() != "U")
            {
                lblMsg.Text = "Attendance Marking Failed! Please Try Again!";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                lblMsg.Text = "Attendance Marked Successfully";
                lblMsg.ForeColor = Color.Green;
                ClearAll();
            }
        }
        else
        {
            lblMsg.Text = validateTime();
            lblMsg.ForeColor = Color.Red;
        }
    }

    private string validateTime()
    {
        if (txtInTime.Text.Trim() == "")
        {
            txtInTime.Focus();
            return "Out Time Cannot Be Blank";
        }
        if (txtInTime.Text.Trim().Length < 4)
        {
            txtInTime.Focus();
            return "Please Enter 4 Digit Time Format";
        }
        if ((Convert.ToInt32(txtInTime.Text.Trim()) <= 0 || Convert.ToInt32(txtInTime.Text.Trim()) > 2359) && drpStatus.SelectedValue == "P")
        {
            txtInTime.Focus();
            return "Please Enter Correct Time";
        }
        if (txtOutTime.Text.Trim() == "")
        {
            txtOutTime.Focus();
            return "Out Time Cannot Be Blank";
        }
        if (txtOutTime.Text.Trim().Length < 4)
        {
            txtOutTime.Focus();
            return "Please Enter 4 Digit Time Format";
        }
        if ((Convert.ToInt32(txtOutTime.Text.Trim()) <= 0 || Convert.ToInt32(txtOutTime.Text.Trim()) > 2359) && drpStatus.SelectedValue == "P")
        {
            txtOutTime.Focus();
            return "Please Enter Correct Time";
        }
        if (txtInTime.Text.Trim().Length < 4)
        {
            txtInTime.Focus();
            return "Please Enter 4 Digit Time Format";
        }
        if (drpStatus.SelectedValue == "P" && (Convert.ToInt32(txtInTime.Text.Trim()) > Convert.ToInt32(txtOutTime.Text.Trim()) || Convert.ToInt32(txtInTime.Text.Trim()) == Convert.ToInt32(txtOutTime.Text.Trim())))
        {
            txtOutTime.Focus();
            return "Out Time Cannot Be Less Than or Equal to In Time";
        }
        if (!(dtpDate.GetDateValue() > DateTime.Today))
            return "";
        txtDate.Focus();
        return "Attendance Cannot Be Marked For Future Date!";
    }

    protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpStatus.SelectedValue.Equals("A") || drpStatus.SelectedValue.Equals("DL") || (drpStatus.SelectedValue.Equals("Off") || drpStatus.SelectedValue.Equals("Tour")))
        {
            txtInTime.Text = "0000";
            txtInTime.Enabled = false;
            txtOutTime.Text = "0000";
            txtOutTime.Enabled = false;
        }
        else
        {
            if (!txtInTime.Enabled.Equals(false))
                return;
            txtInTime.Enabled = true;
            txtOutTime.Enabled = true;
            txtInTime.Text = !(hfIn.Value.ToString().Trim() == "") ? hfIn.Value : "0000";
            if (hfIn.Value.ToString().Trim() == "")
                txtOutTime.Text = "0000";
            else
                txtOutTime.Text = hfOut.Value;
        }
    }

    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Convert.ToDateTime(txtDate.Text.Trim());
            if (drpEmp.SelectedIndex > 0)
                GetAttendance();
            else
                ClearAll();
        }
        catch
        {
            lblMsg.Text = "Invalid Date";
            txtDate.Focus();
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void ClearAll()
    {
        txtInTime.Text = "0000";
        txtOutTime.Text = "0000";
        drpEmp.SelectedIndex = 0;
        drpStatus.SelectedIndex = 0;
        txtRemarks.Text = "";
        btnSubmit.Enabled = false;
    }
}