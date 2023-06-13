using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_LeaveMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private int RecCount;
    private string leaveMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        FillGrid();
    }

    private void FillGrid()
    {
        ClearFields();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("HR_GetAllLeave");
        grdDesignation.DataSource = dataTable2.DefaultView;
        grdDesignation.DataBind();
        lblRecCount.Text = "No Of Records : " + dataTable2.Rows.Count.ToString();
        if (dataTable2.Rows.Count > 0)
            btnDelete.Visible = true;
        else
            btnDelete.Visible = false;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select a Checkbox');", true);
        }
        else
        {
            string str1 = Request["Checkb"];
            RecCount = 0;
            string str2 = str1;
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str2.Split(chArray))
            {
                if (DeleteRecord(obj.ToString()) > 0)
                    ++RecCount;
            }
            if (RecCount > 0)
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = leaveMsg;
            }
            else
            {
                trMsg.Style["background-color"] = "Green";
                lblMsg.Text = "Record(s) Deleted Successfully";
            }
            FillGrid();
        }
    }

    private int DeleteRecord(string id)
    {
        string str = obj.ExecuteScalar("HR_DeleteLeaveMaster", new Hashtable()
    {
      {
         "@LeaveId",
         id
      }
    });
        if (!(str.Trim() != string.Empty))
            return 0;
        leaveMsg = leaveMsg + "</br>" + str;
        return 1;
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        HiddenField control1 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfId");
        HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfSalDed");
        HiddenField control3 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfCFAllowed");
        HiddenField control4 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfLeaveType");
        Label control5 = (Label)((Control)sender).Parent.Parent.FindControl("lblLeaveCode");
        Label control6 = (Label)((Control)sender).Parent.Parent.FindControl("lblLeaveDesc");
        if (control4.Value.Trim().ToUpper() == "S")
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = control5.Text + " is System Defined. Can not be modified.";
        }
        else
        {
            hfLeaveId.Value = control1.Value;
            txtLeaveCode.Text = control5.Text;
            txtLeaveDesc.Text = control6.Text;
            drpSalDed.SelectedValue = control2.Value;
            chkCF.Checked = Convert.ToBoolean(control3.Value);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        InsUpdtLeaveMaster();
    }

    private void InsUpdtLeaveMaster()
    {
        Hashtable hashtable = new Hashtable();
        if (hfLeaveId.Value != "0")
            hashtable.Add("@LeaveId", hfLeaveId.Value);
        hashtable.Add("@LeaveCode", txtLeaveCode.Text.Trim());
        hashtable.Add("@LeaveDesc", txtLeaveDesc.Text.Trim());
        hashtable.Add("@SalaryDeduction", drpSalDed.SelectedValue);
        if (chkCF.Checked)
            hashtable.Add("@CFAllowed", 1);
        else
            hashtable.Add("@CFAllowed", 0);
        hashtable.Add("@UserId", Session["User_Id"]);
        string str = obj.ExecuteScalar("HR_InsUpdtLeaveMaster", hashtable);
        if (str.Trim() == string.Empty)
        {
            trMsg.Style["Background-Color"] = "Green";
            lblMsg.Text = "Data Saved Successfully";
            FillGrid();
        }
        else
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = str.ToString().Trim();
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        trMsg.Style["Background-Color"] = (string)null;
        lblMsg.Text = string.Empty;
        ClearFields();
    }

    private void ClearFields()
    {
        txtLeaveCode.Text = string.Empty;
        txtLeaveDesc.Text = string.Empty;
        drpSalDed.SelectedValue = "1";
        hfLeaveId.Value = "0";
        chkCF.Checked = false;
    }
}