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

public partial class HR_EmpShiftMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
   

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        if (hfShiftId.Value.ToString().Trim() != "")
            hashtable.Add("@ShiftId", hfShiftId.Value.ToString().Trim());
        hashtable.Add("@ShiftName", txtShift.Text.Trim());
        hashtable.Add("@StrtTime", txtStrtTime.Text.Trim());
        hashtable.Add("@EndTime", txtEndTime.Text.Trim());
        hashtable.Add("@UserId", Session["User_Id"]);
        string str = obj.ExecuteScalar("HR_InsUpdtShiftMaster", hashtable);
        if (str.Trim() == string.Empty)
        {
            trMsg.Style["Background-Color"] = "Green";
            lblMsg.Text = "Data Saved Successfully";
            FillGrid();
            clearall();
        }
        else
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = str.ToString().Trim();
        }
    }

    private void FillGrid()
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = obj.GetDataTableQry("Select ShiftId,ShiftCode,StartTime,Endtime from HR_EmpShift");
        grdShift.DataSource = dataTableQry.DefaultView;
        grdShift.DataBind();
        lblRecCount.Text = "No Of Records : " + dataTableQry.Rows.Count.ToString();
        if (dataTableQry.Rows.Count > 0)
        {
            btnDelete.Visible = true;
            btnAdd.Visible = true;
        }
        else
        {
            btnDelete.Visible = false;
            btnAdd.Visible = false;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearall();
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        clearall();
        txtShift.Focus();
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        HiddenField control = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfId");
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = obj.GetDataTableQry("Select ShiftCode,StartTime,Endtime from HR_EmpShift where ShiftId=" + control.Value);
        if (dataTableQry.Rows.Count <= 0)
            return;
        txtShift.Text = dataTableQry.Rows[0]["ShiftCode"].ToString().Trim();
        txtStrtTime.Text = dataTableQry.Rows[0]["StartTime"].ToString().Trim();
        txtEndTime.Text = dataTableQry.Rows[0]["Endtime"].ToString().Trim();
        hfShiftId.Value = control.Value.ToString().Trim();
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
            int num = 0;
            string str2 = str1;
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str2.Split(chArray))
            {
                if (DeleteRecord(obj.ToString()) > 0)
                    ++num;
            }
            if (num > 0)
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Some Records Could Not Be Deleted";
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
        return obj.ExecuteScalar("HR_DeleteShift", new Hashtable()
    {
      {
         "@ShiftId",
         id
      }
    }).Trim() != string.Empty ? 1 : 0;
    }

    private void clearall()
    {
        txtShift.Text = "";
        txtStrtTime.Text = "";
        txtEndTime.Text = "";
        hfShiftId.Value = "";
    }
}