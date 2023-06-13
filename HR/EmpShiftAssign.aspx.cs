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

public partial class HR_EmpShiftAssign : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            bindDrp(drpDesig, "SELECT DesgId,Designation FROM dbo.HR_DesignationMaster where upper(TeachingStaff)<>'M' ORDER BY Designation", "Designation", "DesgId");
            drpDesig.Items.Insert(0, new ListItem("- All -", "0"));
            bindDrp(drpShift, "SELECT ShiftId,ShiftCode+' ('+StartTime+' - '+EndTime+')' as ShiftCode FROM dbo.HR_EmpShift", "ShiftCode", "ShiftId");
            drpShift.Items.Insert(0, new ListItem("- Select -", "0"));
            FillEmployee(drpEmp, 0);
            drpEmp.Items.Insert(0, new ListItem("- All -", "0"));
            FillGrid();
        }
        lblMsg.Text = "";
    }

    private void FillGrid()
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        if (drpDesig.SelectedIndex > 0)
            hashtable.Add("@Desigid", drpDesig.SelectedValue);
        if (drpType.SelectedIndex > 0)
            hashtable.Add("@Type", drpType.SelectedValue.ToString().Trim());
        if (drpEmp.SelectedIndex > 0)
            hashtable.Add("@EmployeeId", drpEmp.SelectedValue);
        DataTable dataTable2 = obj.GetDataTable("HR_GetEmployeeForShift", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            grdEmployee.DataSource = dataTable2;
            grdEmployee.DataBind();
            lblCount.Text = "No Of Records: " + dataTable2.Rows.Count.ToString();
        }
        else
        {
            grdEmployee.DataSource = null;
            grdEmployee.DataBind();
            lblCount.Text = "";
        }
    }

    protected void bindDrp(DropDownList drp, string query, string text, string value)
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = text;
        drp.DataValueField = value;
        drp.DataBind();
    }

    private void FillEmployee(DropDownList drp, int Reassign)
    {
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpDesig.SelectedIndex > 0)
            hashtable.Add("@Desigid", drpDesig.SelectedValue);
        if (drpType.SelectedIndex > 0)
            hashtable.Add("@Type", drpType.SelectedValue.ToString().Trim());
        if (Reassign > 0)
            hashtable.Add("@Reassign", Reassign);
        DataTable dataTable2 = obj.GetDataTable("HR_GetEmployeeList", hashtable);
        drp.DataSource = dataTable2;
        drp.DataTextField = "SevName";
        drp.DataValueField = "EmpId";
        drp.DataBind();
    }

    protected void drpDesig_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEmployee(drpEmp, 0);
        drpEmp.Items.Insert(0, new ListItem("- All -", "0"));
        FillGrid();
    }

    protected void drpType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEmployee(drpEmp, 0);
        drpEmp.Items.Insert(0, new ListItem("- All -", "0"));
        FillGrid();
    }

    protected void drpEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        int num1 = 0;
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a Checkbox');", true);
        }
        else
        {
            string str1 = Request["Checkb"];
            int num2 = 0;
            string str2 = str1;
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str2.Split(chArray))
            {
                if (InsertShift(obj.ToString()) <= 0)
                    ++num2;
                else
                    ++num1;
            }
            if (num2 > 0 && num1 > 0)
            {
                lblMsg.Text = num2.ToString() + " record(s) updated successfully " + num1.ToString() + " record(s) could not be updated";
                lblMsg.ForeColor = Color.Red;
                drpShift.SelectedIndex = 0;
            }
            else if (num2 > 0 && num1 == 0)
            {
                lblMsg.Text = num2.ToString() + " record(s) updated successfully";
                lblMsg.ForeColor = Color.Green;
                drpShift.SelectedIndex = 0;
            }
            else if (num2 == 0 && num1 > 0)
            {
                lblMsg.Text = num1.ToString() + " record(s) could not be updated";
                lblMsg.ForeColor = Color.Red;
            }
            FillGrid();
        }
    }

    private int InsertShift(string id)
    {
        return obj.ExecuteScalar("HR_InsertEmpShift", new Hashtable()
    {
      {
         "@EmpId",
         id
      },
      {
         "@ShiftId",
         drpShift.SelectedValue.ToString().Trim()
      },
      {
         "@Startdt",
         dtpStrtDt.GetDateValue().ToString("dd MMM yyyy")
      }
    }).Trim() != string.Empty ? 1 : 0;
    }

    protected void drpEmpAsign_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpEmpAsign.SelectedIndex > 0)
        {
            string str = obj.ExecuteScalarQry("Select AsigndShiftId from dbo.HR_EmpShiftAsign where EmpId=" + drpEmpAsign.SelectedValue.ToString().Trim() + " and EndDt is null");
            if (str.Trim() != "")
                drpShiftAsign.SelectedValue = str.Trim();
            else
                drpShiftAsign.SelectedIndex = 0;
        }
        else
            drpShiftAsign.SelectedIndex = 0;
        Page.ClientScript.RegisterStartupScript(GetType(), "Call my function", "pop('pop1');", true);
    }

    protected void btnSaveShift_Click(object sender, EventArgs e)
    {
        string str = obj.ExecuteScalar("HR_InsertEmpShift", new Hashtable()
    {
      {
         "@EmpId",
         drpEmpAsign.SelectedValue.ToString().Trim()
      },
      {
         "@StartDt",
         dtpReShiftDt.GetDateValue().ToString("dd MMM yyyy")
      },
      {
         "@ShiftId",
         drpShiftAsign.SelectedValue.ToString().Trim()
      },
      {
         "@Type",
         "UPDATE"
      }
    });
        if (str.Trim() == "")
        {
            lblMsg2.Text = "Shift Reassigned Successfully";
            lblMsg2.ForeColor = Color.Green;
            txtReShftDt.Text = "";
            drpEmpAsign.SelectedIndex = 0;
            drpShiftAsign.SelectedIndex = 0;
        }
        else if (str.Trim() == "DUP")
        {
            lblMsg2.Text = "Same Shift Already Assigned To The Employee Please Change Shift To Reassign!!!";
            lblMsg2.ForeColor = Color.Red;
        }
        else if (str.Trim() == "Date")
        {
            lblMsg2.Text = "Cannot Reassign Shift! As Already Assigned For Later Date!";
            lblMsg2.ForeColor = Color.Red;
        }
        else
        {
            lblMsg2.Text = "Cannot Assing Shift !! Please Try Again!!";
            lblMsg2.ForeColor = Color.Red;
        }
        Page.ClientScript.RegisterStartupScript(GetType(), "Call my function", "pop('pop1');", true);
    }

    protected void btnReassign_Click(object sender, EventArgs e)
    {
        bindDrp(drpShiftAsign, "SELECT ShiftId,ShiftCode+' ('+StartTime+' - '+EndTime+')' as ShiftCode FROM dbo.HR_EmpShift", "ShiftCode", "ShiftId");
        drpShiftAsign.Items.Insert(0, new ListItem("- Select -", "0"));
        FillEmployee(drpEmpAsign, 1);
        drpEmpAsign.Items.Insert(0, new ListItem("- Select -", "0"));
        Page.ClientScript.RegisterStartupScript(GetType(), "Call my function", "pop('pop1');", true);
        txtReShftDt.Text = "";
        lblMsg2.Text = "";
    }
}