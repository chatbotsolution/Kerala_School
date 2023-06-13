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

public partial class HR_EmpExtraWorking : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        lblMsg1.Text = "";
        if (IsPostBack)
            return;
        bindDropDown(drpEmp, "Select EmpId,SevName from dbo.HR_EmployeeMaster where ActiveStatus=1 order by SevName", "SevName", "EmpId");
        bindDropDown(drpEmpList, "Select EmpId,SevName from dbo.HR_EmployeeMaster where ActiveStatus=1 order by SevName", "SevName", "EmpId");
        drpEmpList.Items.RemoveAt(0);
        drpEmpList.Items.Insert(0, new ListItem("- Select -", "0"));
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
        drp.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (dtpDate.GetDateValue() <= DateTime.Today)
        {
            obj = new clsDAL();
            if (obj.ExecuteScalarQry("Select EW_Days_Count from dbo.HR_Conditions").Trim() != "")
            {
                string str1 = obj.ExecuteScalarQry("select HolidayID from dbo.HR_HolidayMaster where HolidayDate='" + dtpDate.GetDateValue().ToString("dd MMM yyyy") + "'");
                if (str1.Trim() != "")
                {
                    Hashtable hashtable = new Hashtable();
                    obj = new clsDAL();
                    hashtable.Add("@EW_Date", dtpDate.GetDateValue().ToString("dd MMM yyyy"));
                    hashtable.Add("@EmpId", drpEmp.SelectedValue.Trim());
                    hashtable.Add("@Remarks", txtRemarks.Text.Trim());
                    hashtable.Add("@UserId", Session["User_Id"].ToString().Trim());
                    hashtable.Add("@HolidayId", str1.Trim());
                    string str2 = obj.ExecuteScalar("HR_InsExWorking", hashtable);
                    if (str2.Trim() == "")
                    {
                        lblMsg.Text = "Extra Working Marked Successfully!!";
                        lblMsg.ForeColor = Color.Green;
                        ClearFields1();
                    }
                    else if (str2.Trim().ToUpper() == "DUP")
                    {
                        lblMsg.Text = "Extra Working Already Marked for this employee on this date!!";
                        lblMsg.ForeColor = Color.Red;
                    }
                    else if (str2.Trim().ToUpper() == "LEAVE")
                    {
                        lblMsg.Text = "Cannot Mark Extra Working! Employee Already On Leave!!";
                        lblMsg.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblMsg.Text = "Unable to mark Extra Working Attendance!!";
                        lblMsg.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblMsg.Text = "Extra Working Can Be Only marked on holidays!";
                    lblMsg.ForeColor = Color.Red;
                }
            }
            else
            {
                lblMsg.Text = "Please Set Conversion Factor for extra working to EL!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
        {
            lblMsg1.Text = "Extra Working Cannot Be Marked For Future Date!";
            lblMsg1.ForeColor = Color.Red;
        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        pnlAdd.Visible = false;
        pnlList.Visible = true;
        ClearFields2();
    }

    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        HiddenField control = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfEWId");
        obj = new clsDAL();
        string str = obj.ExecuteScalar("HR_DelExWorking", new Hashtable()
    {
      {
         "@EW_Id",
         control.Value.Trim()
      }
    });
        if (str.Trim() == "")
        {
            lblMsg1.Text = "Extra Working Deleted Successfully!!";
            lblMsg1.ForeColor = Color.Green;
            FillGrid();
        }
        else if (str.Trim().ToUpper() == "INVALID")
        {
            lblMsg1.Text = "Cannot Delete Record! Extra Working days marked for later date!!";
            lblMsg1.ForeColor = Color.Red;
        }
        else if (str.Trim().ToUpper() == "N")
        {
            lblMsg1.Text = "Not Enough EL available for the employee!!";
            lblMsg1.ForeColor = Color.Red;
        }
        else
        {
            lblMsg1.Text = "Unable to Delete Extra Working Attendance!!";
            lblMsg1.ForeColor = Color.Red;
        }
    }

    protected void btnAddEW_Click(object sender, EventArgs e)
    {
        pnlList.Visible = false;
        pnlAdd.Visible = true;
        ClearFields1();
    }

    private void ClearFields1()
    {
        txtDate.Text = "";
        drpEmp.SelectedIndex = 0;
        txtRemarks.Text = "";
    }

    private void ClearFields2()
    {
        txtFrmDt.Text = "";
        txtToDt.Text = "";
        drpEmpList.SelectedIndex = 0;
        if (grdEW.Rows.Count <= 0)
            return;
        grdEW.DataSource = null;
        grdEW.DataBind();
    }

    private void FillGrid()
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        if (txtFrmDt.Text != "")
            hashtable.Add("@FrmDt", dtpFrmDt.GetDateValue().ToString("dd MMM yyyy"));
        if (txtToDt.Text != "")
            hashtable.Add("@ToDt", dtpToDt.GetDateValue().ToString("dd MMM yyyy"));
        if (drpEmpList.SelectedIndex > 0)
            hashtable.Add("@EmpId", drpEmpList.SelectedValue.Trim());
        DataTable dataTable2 = obj.GetDataTable("HR_GetExWorking", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            grdEW.DataSource = dataTable2;
            grdEW.DataBind();
        }
        else
        {
            grdEW.DataSource = null;
            grdEW.DataBind();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
    }
}