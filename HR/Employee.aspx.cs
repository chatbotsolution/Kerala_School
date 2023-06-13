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

public partial class HR_Employee : System.Web.UI.Page
{
    private clsDAL ObjDAL;
    private DataTable dt;
    private Hashtable ht;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        bindDropDown(drpDept, "select DeptId,DeptName from DeptMaster order by DeptName", "DeptName", "DeptId");
        bindDropDown(drpDesignation, "select DesignationId,Designation from DesignationMaster order by Designation", "Designation", "DesignationId");
        ClearFields();
        if (Request.QueryString["eno"] == null)
            return;
        FillEmployeeDetails();
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        ObjDAL = new clsDAL();
        dt = ObjDAL.GetDataTableQry(query);
        drp.DataSource = dt.DefaultView;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    private void FillEmployeeDetails()
    {
        ObjDAL = new clsDAL();
        dt = new DataTable();
        dt = ObjDAL.GetDataTableQry("select * from EmpMaster where EmpId=" + Request.QueryString["eno"].ToString());
        txtEmpName.Text = dt.Rows[0]["EmpName"].ToString();
        txtEmpAddress.Text = dt.Rows[0]["EmpAddress"].ToString();
        DateTime dateTime1 = Convert.ToDateTime(dt.Rows[0]["EmpDOB"].ToString());
        if (dateTime1.ToString("dd MMM yyyy").Equals("01 Jan 1900"))
            txtEmpDOB.Text = string.Empty;
        else
            dtpDOB.SetDateValue(dateTime1);
        drpDept.SelectedValue = dt.Rows[0]["DeptId"].ToString();
        drpDesignation.SelectedValue = dt.Rows[0]["DesignationID"].ToString();
        DateTime dateTime2 = Convert.ToDateTime(dt.Rows[0]["JoiningDate"].ToString());
        if (dateTime2.ToString("dd MMM yyyy").Equals("01 Jan 1900"))
            txtEmpJoinDate.Text = string.Empty;
        else
            dtpJDate.SetDateValue(dateTime2);
        DateTime dateTime3 = Convert.ToDateTime(dt.Rows[0]["LeavingDate"].ToString());
        if (dateTime3.ToString("dd MMM yyyy").Equals("01 Jan 1900"))
            txtEmpLeaveDate.Text = string.Empty;
        else
            dtpLDate.SetDateValue(dateTime3);
        if (Convert.ToInt32(dt.Rows[0]["ActiveStatus"]).Equals(1))
            optStatActive.Checked = true;
        else
            optStatInactive.Checked = true;
        txtContactNo.Text = dt.Rows[0]["ContactTel"].ToString();
        txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
        txtQualification.Text = dt.Rows[0]["Qualification"].ToString();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        InsertToTable();
    }

    private void InsertToTable()
    {
        ObjDAL = new clsDAL();
        ht = new Hashtable();
        if (Request.QueryString["eno"] != null)
            ht.Add("EmpId", Request.QueryString["eno"].ToString());
        ht.Add("EmpName", txtEmpName.Text.Trim());
        ht.Add("EmpAddress", txtEmpAddress.Text.Trim());
        if (!txtEmpDOB.Text.Trim().Equals(string.Empty))
        {
            try
            {
                ht.Add("EmpDOB", Convert.ToDateTime(txtEmpDOB.Text.Trim()));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid Date Of Birth');", true);
                return;
            }
        }
        else
            ht.Add("EmpDOB", "01/01/1900");
        ht.Add("@DeptId", drpDept.SelectedValue.ToString());
        ht.Add("DesignationID", drpDesignation.SelectedValue.ToString());
        if (!txtEmpJoinDate.Text.Trim().Equals(string.Empty))
        {
            try
            {
                ht.Add("JoiningDate", Convert.ToDateTime(txtEmpJoinDate.Text.Trim()));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid Joining Date');", true);
                txtEmpJoinDate.Focus();
                return;
            }
        }
        else
            ht.Add("JoiningDate", "01/01/1900");
        if (!txtEmpLeaveDate.Text.Trim().Equals(string.Empty))
        {
            try
            {
                ht.Add("LeavingDate", Convert.ToDateTime(txtEmpLeaveDate.Text.Trim()));
            }
            catch (Exception ex)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid Leaving Date');", true);
                txtEmpLeaveDate.Focus();
                return;
            }
        }
        else
            ht.Add("LeavingDate", "01/01/1900");
        if (optStatActive.Checked.Equals(true))
            ht.Add("ActiveStatus", 1);
        else
            ht.Add("ActiveStatus", 0);
        ht.Add("ContactTel", txtContactNo.Text.Trim());
        ht.Add("Remarks", txtRemarks.Text.Trim());
        ht.Add("Qualification", txtQualification.Text.Trim());
        ht.Add("UserId", Session["UserId"].ToString());
        dt = ObjDAL.GetDataTable("InstUpdtEmployee", ht);
        if (dt.Rows.Count > 0)
        {
            lblMsg.Style["color"] = "Red";
            lblMsg.Text = dt.Rows[0][0].ToString();
        }
        else if (Request.QueryString["eno"] != null)
        {
            Response.Redirect("EmployeeList.aspx");
        }
        else
        {
            ClearFields();
            lblMsg.Style["color"] = "Green";
            lblMsg.Text = "Record Saved";
        }
    }

    private void ClearFields()
    {
        txtEmpName.Text = string.Empty;
        txtEmpAddress.Text = string.Empty;
        drpDept.SelectedIndex = 0;
        drpDesignation.SelectedIndex = 0;
        txtEmpDOB.Text = string.Empty;
        txtEmpJoinDate.Text = string.Empty;
        txtEmpLeaveDate.Text = string.Empty;
        txtQualification.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        optStatActive.Checked = true;
        txtContactNo.Text = string.Empty;
        optStatActive.Checked = true;
        optStatInactive.Checked = false;
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    protected void btnShowList_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmployeeList.aspx");
    }
}