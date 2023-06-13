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

public partial class Attendance_StudentAttendance : System.Web.UI.Page
{
    private clsDAL DAL = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    private const string colorPresent = "#BDF8B9";
    private const string colorAbsent = "#FFB5B2";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        litFinalMsg.Text = string.Empty;
        lblMsg2.Text = "";
        if (Page.IsPostBack)
            return;
        lblMsg.Visible = false;
        bindDropDown(drpClass, "select ClassId,ClassName from PS_ClassMaster", "ClassName", "ClassId");
        dtpDate.SetDateValue(DateTime.Today);
        btnSubmit.Enabled = false;
        gvStudAttendance.DataSource = null;
        gvStudAttendance.DataBind();
        objStatic.FillSection(drpSection);
        drpSection.Items.RemoveAt(0);
        drpSection.Items.Insert(0, new ListItem("- All -", "0"));
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        dt = DAL.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void FillGrid()
    {
        if (drpClass.SelectedIndex > 0)
        {
            ht = new Hashtable();
            dt = new DataTable();
            ht.Add("AttendDate", dtpDate.GetDateValue().ToString("MM/dd/yyyy"));
            ht.Add("ClassId", drpClass.SelectedValue);
            if (drpSection.SelectedIndex > 0)
                ht.Add("@Section", drpSection.SelectedValue);
            ht.Add("SchoolId", Session["SchoolId"]);
            dt = DAL.GetDataTable("ps_sp_get_StudAttendance", ht);
            gvStudAttendance.DataSource = dt;
            gvStudAttendance.DataBind();
            lblMsg.Visible = false;
            btnSubmit.Text = "Submit";
            if (gvStudAttendance.Rows.Count > 0)
                btnSubmit.Enabled = true;
            gvStudAttendance.Visible = true;
        }
        else
        {
            gvStudAttendance.DataSource = null;
            gvStudAttendance.DataBind();
        }
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgridAfterCheck();
    }

    protected void txtDate_TextChanged(object sender, EventArgs e)
    {
        fillgridAfterCheck();
    }

    private void fillgridAfterCheck()
    {
        FillGrid();
        btnSubmit.Enabled = true;
        gvStudAttendance.Visible = true;
    }

    protected void dtpDate_SelectionChanged(object sender, EventArgs e)
    {
        fillgridAfterCheck();
    }

    private void InsertVaueToStudAttendance()
    {
        string empty = string.Empty;
        if (new clsDAL().ExecuteScalarQry("select count(*) from dbo.PS_StudAttendance where AttendDate >'" + dtpDate.GetDateValue().ToString("dd MMM yyyy") + "' and ClassId=" + drpClass.SelectedValue.Trim()).Trim() == "0")
        {
            foreach (GridViewRow row in gvStudAttendance.Rows)
            {
                string str1 = ((CheckBox)row.FindControl("chkStatus")).Checked ? "Present" : "Absent";
                TextBox control = (TextBox)row.FindControl("txtRemarks");
                Int64 int32 = (long)Convert.ToInt64(((Label)row.FindControl("lblAdmnNo")).Text);
                ht = new Hashtable();
                ht.Add("AttendDate", dtpDate.GetDateValue());
                ht.Add("ClassId", drpClass.SelectedValue);
                ht.Add("AdmnNo", int32);
                ht.Add("AttendStatus", str1);
                ht.Add("Remarks", control.Text.Trim());
                ht.Add("UserId", Session["User_Id"]);
                ht.Add("SchoolId", Session["SchoolId"]);
                string str2 = DAL.ExecuteScalar("ps_sp_insert_StudAttendanceAfterDelete", ht);
                empty += empty.Trim().Equals(string.Empty) ? str2 : "," + str2;
            }
            if (empty.Trim().Equals(string.Empty))
            {
                ResetAll();
                litFinalMsg.Text = "<div style='color:Green;' class='successMessage'>Attendance marked successfully for selected date !</div>";
            }
            else
            {
                lblMsg.Visible = false;
                litFinalMsg.Text = "<div style='color:Red;' class='failureMessage'>Marking attendance failed for :" + empty + "  !</div>";
            }
        }
        else
        {
            litFinalMsg.Text = "<div style='color:Red;'>Attendance cannot be marked for previous date!!</div>";
            lblMsg2.Text = "Attendance cannot be marked for previous date!!";
            lblMsg2.ForeColor = Color.Red;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        InsertVaueToStudAttendance();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ResetAll();
    }

    private void ResetAll()
    {
        drpClass.SelectedIndex = 0;
        dtpDate.SetDateValue(DateTime.Now);
        lblMsg.Visible = false;
        gvStudAttendance.Visible = false;
        btnSubmit.Text = "Submit";
        btnSubmit.Enabled = false;
    }

    protected void btnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Attendance/AttendHome.aspx");
    }

    protected void chkStatus_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox checkBox = (CheckBox)sender;
        GridViewRow parent = (GridViewRow)checkBox.Parent.Parent;
        TextBox control = (TextBox)parent.FindControl("txtRemarks");
        control.Text = string.Empty;
        if (checkBox.Checked)
        {
            control.Enabled = false;
            parent.BackColor = ColorTranslator.FromHtml("#BDF8B9");
        }
        else
        {
            control.Enabled = true;
            parent.BackColor = ColorTranslator.FromHtml("#FFB5B2");
        }
    }

    protected void gvStudAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!e.Row.RowType.Equals(DataControlRowType.DataRow))
            return;
        TextBox control1 = (TextBox)e.Row.FindControl("txtRemarks");
        CheckBox control2 = (CheckBox)e.Row.FindControl("chkStatus");
        if (DataBinder.Eval(e.Row.DataItem, "AttendStatus").ToString().Trim().Equals("Present") || DataBinder.Eval(e.Row.DataItem, "AttendStatus").ToString().Trim().Equals(string.Empty))
        {
            e.Row.BackColor = ColorTranslator.FromHtml("#BDF8B9");
            control1.Text = string.Empty;
            control1.Enabled = false;
        }
        else
        {
            e.Row.BackColor = ColorTranslator.FromHtml("#FFB5B2");
            control1.Enabled = true;
            control2.Checked = false;
        }
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }
}