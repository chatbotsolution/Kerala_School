using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_EmpAttendance : System.Web.UI.Page
{
    private clsDAL ObjDAL;
    private DataTable dt;
    private Hashtable ht;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg1.Text = "";
        lblMsg3.Text = "";
        if (Page.IsPostBack)
            return;
        lblMsg1.Text = string.Empty;
        lblMsg2.Visible = false;
        dtpDate.DateValue =  DateTime.Now;
        btnSubmit.Enabled = false;
        bindDropDown(drpShift, "SELECT ShiftId,ShiftCode+' ('+StartTime+' - '+EndTime+')' as ShiftCode FROM dbo.HR_EmpShift", "ShiftCode", "ShiftId");
        FillGrid();
        ObjDAL = new clsDAL();
        if (Convert.ToInt32(ObjDAL.ExecuteScalarQry("select (Select count(*) from dbo.HR_EmployeeMaster where (LeavingDate is null or '" + dtpDate.GetDateValue().ToString("dd MMM yyyy") + "'<=LeavingDate))-(Select count(*) EmpId from HR_EmpShiftAsign where EndDt is null) as diff").Trim()) > 0)
        {
            lblMsg1.Text = "Some Employees Are Yet To Be Assigned Any Shift!! Please Assign Shift And Then Mark Attendance!!";
            lblMsg1.ForeColor = Color.Red;
            lblMsg3.Text = "Some Employees Are Yet To Be Assigned Any Shift!! Please Assign Shift And Then Mark Attendance!!";
            lblMsg3.ForeColor = Color.Red;
        }
        CheckIfHoliday();
        getLastAttendanceDt();
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        ObjDAL = new clsDAL();
        dt = new DataTable();
        dt = ObjDAL.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- All -", "0"));
    }

    private void CheckIfHoliday()
    {
        ObjDAL = new clsDAL();
        string str = ObjDAL.ExecuteScalarQry("select HolidayName from dbo.HR_HolidayMaster where HolidayDate='" + dtpDate.GetDateValue().ToString("dd MMM yyyy") + "' and Is_Working=0");
        if (str.Trim() != "")
        {
            grdEmpAttendance.Enabled = false;
            btnSubmit.Enabled = false;
            chkbHolidy.Checked = true;
            txtHolidayDesc.Text = str.Trim();
            txtHolidayDesc.Enabled = false;
            btnSaveHoliday.Enabled = false;
        }
        else
        {
            grdEmpAttendance.Enabled = true;
            btnSubmit.Enabled = true;
            chkbHolidy.Checked = false;
            txtHolidayDesc.Text = "";
            txtHolidayDesc.Enabled = true;
            btnSaveHoliday.Enabled = true;
            ObjDAL = new clsDAL();
            if (!(ObjDAL.ExecuteScalarQry("select HolidayName from dbo.HR_HolidayMaster where HolidayDate='" + dtpDate.GetDateValue().ToString("dd MMM yyyy") + "'").Trim() == "") || !(dtpDate.GetDateValue().DayOfWeek.ToString().Trim().ToUpper() == "SUNDAY"))
                return;
            if (!(new clsDAL().ExecuteScalar("HR_InsUpdtHolidayMaster", new Hashtable()
      {
        {
           "@HolidayName",
           "Sunday"
        },
        {
           "@HolidayTithi",
           "Sunday"
        },
        {
           "@FrmDate",
           dtpDate.GetDateValue().ToString("dd MMM yyyy")
        },
        {
           "@ToDate",
           dtpDate.GetDateValue().ToString("dd MMM yyyy")
        },
        {
           "@UserID",
           Convert.ToInt32(Session["User_Id"])
        }
      }).Trim() == string.Empty))
                return;
            CheckIfHoliday();
        }
    }

    private void getLastAttendanceDt()
    {
        ObjDAL = new clsDAL();
        string str = ObjDAL.ExecuteScalarQry("select convert(varchar(30),max(AttendDate),106) as Dt from dbo.HR_EmpAttendance where AttendStatus<>'L'");
        if (str.Trim() != "")
        {
            lblLastAtt.Text = "Attendance Last Marked On " + str;
            lblLastAtt.ForeColor = Color.Red;
        }
        else
            lblLastAtt.Text = "";
    }

    private void FillGrid()
    {
        ChkForAtt();
        ht = new Hashtable();
        dt = new DataTable();
        ObjDAL = new clsDAL();
        ht.Add("AttendDate", dtpDate.GetDateValue().ToString("MM/dd/yyyy"));
        if (drpShift.SelectedIndex > 0)
            ht.Add("@ShiftId", drpShift.SelectedValue.ToString().Trim());
        dt = ObjDAL.GetDataTable("HR_GetEmpAttendanceForGvBind", ht);
        if (dt.Rows.Count > 0)
        {
            grdEmpAttendance.DataSource = dt;
            grdEmpAttendance.DataBind();
            lblRecords.Text = "No Of Records: " + dt.Rows.Count.ToString();
            grdEmpAttendance.Visible = true;
            ChkForDisGridFields();
        }
        if (grdEmpAttendance.Rows.Count > 0)
            btnSubmit.Enabled = true;
        else
            btnSubmit.Enabled = false;
    }

    private void ChkForAtt()
    {
        ObjDAL = new clsDAL();
        ht = new Hashtable();
        ht.Add("@AttendDate", dtpDate.GetDateValue().ToString("dd MMM yyyy"));
        if (Convert.ToInt32(ObjDAL.ExecuteScalar("HR_GetAttStatus", ht)) > 0)
            lblMsg2.Visible = true;
        else
            lblMsg2.Visible = false;
    }

    private void ChkForDisGridFields()
    {
        foreach (GridViewRow row in grdEmpAttendance.Rows)
        {
            DropDownList control1 = (DropDownList)row.FindControl("drpStatus");
            TextBox control2 = (TextBox)row.FindControl("txtInTime");
            TextBox control3 = (TextBox)row.FindControl("txtOutTime");
            Label control4 = (Label)row.FindControl("ChkID");
            TextBox control5 = (TextBox)row.FindControl("txtRemarks");
            HiddenField control6 = (HiddenField)row.FindControl("hfIn");
            HiddenField control7 = (HiddenField)row.FindControl("hfOut");
            if (control1.SelectedValue.Equals("A") || control1.SelectedValue.Equals("DL") || (control1.SelectedValue.Equals("Off") || control1.SelectedValue.Equals("Tour")))
            {
                control2.Text = "0000";
                control2.Enabled = false;
                control3.Text = "0000";
                control3.Enabled = false;
            }
            else if (control2.Enabled.Equals(false))
            {
                control2.Enabled = true;
                control3.Enabled = true;
                control2.Text = control6.Value;
                control3.Text = control7.Value;
            }
        }
    }

    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        fillgridAfterCheck();
    }

    protected void drpDept_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgridAfterCheck();
    }

    protected void dtpDate_SelectionChanged(object sender, EventArgs e)
    {
        fillgridAfterCheck();
        CheckIfHoliday();
    }

    protected void chkbHolidy_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbHolidy.Checked)
        {
            grdEmpAttendance.Enabled = false;
            btnSubmit.Enabled = false;
        }
        else
        {
            grdEmpAttendance.Enabled = true;
            btnSubmit.Enabled = true;
        }
    }

    private void fillgridAfterCheck()
    {
        FillGrid();
        grdEmpAttendance.Visible = true;
    }

    protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList dropDownList = (DropDownList)sender;
        GridViewRow parent = (GridViewRow)dropDownList.Parent.Parent;
        TextBox control1 = (TextBox)parent.FindControl("txtInTime");
        TextBox control2 = (TextBox)parent.FindControl("txtOutTime");
        HiddenField control3 = (HiddenField)parent.FindControl("hfIn");
        HiddenField control4 = (HiddenField)parent.FindControl("hfOut");
        if (dropDownList.SelectedValue.Equals("A") || dropDownList.SelectedValue.Equals("DL") || (dropDownList.SelectedValue.Equals("Off") || dropDownList.SelectedValue.Equals("Tour")))
        {
            control1.Text = "0000";
            control1.Enabled = false;
            control2.Text = "0000";
            control2.Enabled = false;
        }
        else
        {
            if (!control1.Enabled.Equals(false))
                return;
            control1.Enabled = true;
            control2.Enabled = true;
            control1.Text = !(control3.Value.ToString().Trim() == "") ? control3.Value : "0000";
            if (control3.Value.ToString().Trim() == "")
                control2.Text = "0000";
            else
                control2.Text = control4.Value;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (dtpDate.GetDateValue() <= DateTime.Today)
        {
            string str = new clsDAL().ExecuteScalar("HR_EmpAttendanceChk", new Hashtable()
      {
        {
           "@Year",
           dtpDate.GetDateValue().Year.ToString().Trim()
        },
        {
           "@Month",
           dtpDate.GetDateValue().ToString("MMM").ToUpper()
        },
        {
           "@AttndDate",
           dtpDate.GetDateValue().ToString("dd MMM yyyy")
        }
      });
            if (str.Trim() == "")
                InsertVaueToEmpAttendance();
            else if (str.Trim().ToUpper() == "SAL")
            {
                lblMsg1.Text = "Cannot Mark Attendance!! Salary Already Generated For This Month";
                lblMsg1.ForeColor = Color.Red;
                lblMsg3.Text = "Cannot Mark Attendance!! Salary Already Generated For This Month";
                lblMsg3.ForeColor = Color.Red;
            }
            else if (str.Trim().ToUpper() == "NXT")
            {
                lblMsg1.Text = "Cannot Mark Attendance!! Attendance Already Marked For Later Date";
                lblMsg1.ForeColor = Color.Red;
                lblMsg3.Text = "Cannot Mark Attendance!! Attendance Already Marked For Later Date";
                lblMsg3.ForeColor = Color.Red;
            }
            else if (str.Trim().ToUpper() == "EW")
            {
                lblMsg1.Text = "Extra Working Already Marked On this date Please remove Extra Working! To Mark Attendance!!";
                lblMsg1.ForeColor = Color.Red;
                lblMsg3.Text = "Cannot Mark Attendance!! Attendance Already Marked For Later Date";
                lblMsg3.ForeColor = Color.Red;
            }
            else if (str.Trim().ToUpper() == "PREV")
            {
                lblMsg1.Text = "Please Mark Full Attendance For Previous Day!!";
                lblMsg1.ForeColor = Color.Red;
                lblMsg3.Text = "Please Mark Full Attendance For Previous Day!!";
                lblMsg3.ForeColor = Color.Red;
            }
            else
            {
                lblMsg1.Text = "Cannot Mark Attendance! Please Try Again";
                lblMsg1.ForeColor = Color.Red;
                lblMsg3.Text = "Cannot Mark Attendance! Please Try Again";
                lblMsg3.ForeColor = Color.Red;
            }
        }
        else
        {
            lblMsg1.Text = "Attendance Cannot Be Marked For Future Date!";
            lblMsg1.ForeColor = Color.Red;
            lblMsg3.Text = "Attendance Cannot Be Marked For Future Date!";
            lblMsg3.ForeColor = Color.Red;
        }
    }

    private void InsertVaueToEmpAttendance()
    {
        DateTime dateValue = dtpDate.GetDateValue();
        int num = 0;
        foreach (GridViewRow row in grdEmpAttendance.Rows)
        {
            string str = validateTime((TextBox)row.FindControl("txtInTime"), (TextBox)row.FindControl("txtOutTime"), (DropDownList)row.FindControl("drpStatus"));
            if (str != "")
            {
                lblMsg1.Text = str;
                lblMsg1.ForeColor = Color.Red;
                lblMsg3.Text = str;
                lblMsg3.ForeColor = Color.Red;
                ++num;
                break;
            }
        }
        if (num != 0)
            return;
        foreach (GridViewRow row in grdEmpAttendance.Rows)
        {
            string text1 = ((TextBox)row.FindControl("txtInTime")).Text;
            string text2 = ((TextBox)row.FindControl("txtOutTime")).Text;
            DropDownList control1 = (DropDownList)row.FindControl("drpStatus");
            string text3 = control1.Text;
            TextBox control2 = (TextBox)row.FindControl("txtRemarks");
            string text4 = control2.Text;
            int int32 = Convert.ToInt32(((Label)row.FindControl("lblEmpId")).Text);
            if (control1.Enabled && control2.Enabled)
            {
                ht = new Hashtable();
                ObjDAL = new clsDAL();
                ht.Add("AttendDate", dateValue);
                ht.Add("EmpId", int32);
                if (control1.SelectedValue != "A" && control1.SelectedValue != "DL" && (control1.SelectedValue != "Off" && control1.SelectedValue != "Tour"))
                {
                    ht.Add("InTime", text1);
                    ht.Add("OutTime", text2);
                }
                ht.Add("AttendStatus", text3);
                ht.Add("Remarks", text4);
                ht.Add("UserId", Session["User_Id"]);
                string str = ObjDAL.ExecuteScalar("HR_InstEmpAttendanceAfterDelete", ht);
                if (str.Trim().ToUpper() != "S" && str.Trim().ToUpper() != "U")
                {
                    lblMsg2.Visible = false;
                    lblMsg1.Text = "Attendance Marking Failed! Please Try Again!";
                    lblMsg1.ForeColor = Color.Red;
                    lblMsg3.Text = "Attendance Marking Failed!  Please Try Again!";
                    lblMsg3.ForeColor = Color.Red;
                    break;
                }
                lblMsg1.Text = "Attendance Marked Successfully";
                lblMsg1.ForeColor = Color.Green;
                lblMsg3.Text = "Attendance Marked Successfully";
                lblMsg3.ForeColor = Color.Green;
                getLastAttendanceDt();
            }
        }
    }

    private string validateTime(TextBox InT, TextBox OutT, DropDownList drpStatus)
    {
        if (InT.Text.Trim() == "")
        {
            InT.Focus();
            return "Out Time Cannot Be Blank";
        }
        if (InT.Text.Trim().Length < 4)
        {
            InT.Focus();
            return "Please Enter 4 Digit Time Format";
        }
        if ((Convert.ToInt32(InT.Text.Trim()) <= 0 || Convert.ToInt32(InT.Text.Trim()) > 2359) && drpStatus.SelectedValue == "P")
        {
            InT.Focus();
            return "Please Enter Correct Time";
        }
        if (OutT.Text.Trim() == "")
        {
            OutT.Focus();
            return "Out Time Cannot Be Blank";
        }
        if (OutT.Text.Trim().Length < 4)
        {
            OutT.Focus();
            return "Please Enter 4 Digit Time Format";
        }
        if ((Convert.ToInt32(OutT.Text.Trim()) <= 0 || Convert.ToInt32(OutT.Text.Trim()) > 2359) && drpStatus.SelectedValue == "P")
        {
            OutT.Focus();
            return "Please Enter Correct Time";
        }
        if (InT.Text.Trim().Length < 4)
        {
            InT.Focus();
            return "Please Enter 4 Digit Time Format";
        }
        if (!(drpStatus.SelectedValue == "P") || Convert.ToInt32(InT.Text.Trim()) <= Convert.ToInt32(OutT.Text.Trim()) && Convert.ToInt32(InT.Text.Trim()) != Convert.ToInt32(OutT.Text.Trim()))
            return "";
        OutT.Focus();
        return "Out Time Cannot Be Less Than or Equal to In Time";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetAll();
    }

    private void ResetAll()
    {
        dtpDate.DateValue =  DateTime.Now;
        lblMsg2.Visible = false;
        FillGrid();
        CheckIfHoliday();
    }

    protected void grdEmpAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        GridViewRow row = e.Row;
        DropDownList control1 = (DropDownList)row.FindControl("drpStatus");
        Label control2 = (Label)row.FindControl("lblStatus");
        if (control1 == null)
            return;
        control1.SelectedValue = control2.Text.Trim();
    }

    protected void drpShift_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void btnSaveHoliday_Click(object sender, EventArgs e)
    {
        ObjDAL = new clsDAL();
        if (ObjDAL.ExecuteScalarQry("Select count(*) from dbo.HR_EmpAttendance where AttendDate>='" + dtpDate.GetDateValue().ToString("dd MMM yyyy") + "' and AttendDate<='" + dtpDate.GetDateValue().ToString("dd MMM yyyy") + "' and AttendStatus<>'L'").Trim() != "0")
        {
            lblMsg3.Text = "Holiday cannot be Declared for Previous Date!";
            lblMsg1.Text = "Holiday cannot be Declared for Previous Date!";
            lblMsg3.ForeColor = Color.Red;
            lblMsg3.ForeColor = Color.Red;
        }
        else
        {
            ht = new Hashtable();
            ObjDAL = new clsDAL();
            ht.Add("@HolidayName", txtHolidayDesc.Text);
            ht.Add("@HolidayTithi", txtHolidayDesc.Text);
            ht.Add("@FrmDate", Convert.ToDateTime(dtpDate.GetDateValue().ToString("dd MMM yyyy")));
            ht.Add("@ToDate", Convert.ToDateTime(dtpDate.GetDateValue().ToString("dd MMM yyyy")));
            ht.Add("@UserID", Convert.ToInt32(Session["User_Id"]));
            string str = ObjDAL.ExecuteScalar("HR_InsUpdtHolidayMaster", ht);
            if (str.Trim() == string.Empty)
            {
                lblMsg3.Text = "Data Saved Successfully";
                lblMsg1.Text = "Data Saved Successfully";
                lblMsg3.ForeColor = Color.Green;
                lblMsg3.ForeColor = Color.Green;
                CheckIfHoliday();
            }
            else if (str.Trim() == "DUP")
            {
                lblMsg3.Text = "Holiday Altready Exists On This Date";
                lblMsg1.Text = "Holiday Altready Exists On This Date";
                lblMsg3.ForeColor = Color.Red;
                lblMsg1.ForeColor = Color.Red;
            }
            else if (str.Trim() == "SALARY")
            {
                lblMsg3.Text = "Salary Already Generated for this month";
                lblMsg1.Text = "Salary Already Generated for this month";
                lblMsg3.ForeColor = Color.Red;
                lblMsg1.ForeColor = Color.Red;
            }
            else
            {
                lblMsg3.Text = "Unable to save holiday!Please Try Again!";
                lblMsg1.Text = "Unable to save holiday!Please Try Again!";
                lblMsg3.ForeColor = Color.Red;
                lblMsg3.ForeColor = Color.Red;
            }
        }
    }

    protected void btnExtraAtt_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmpExtraWorking.aspx");
    }
}