using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_LeaveCancel : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        txtToDt.Text = DateTime.Now.ToString("dd-MMM-yyyy");
        dtpFromDt.SetDateValue(dtpToDt.GetDateValue().AddMonths(-1));
        GetLeaveDetails();
    }

    private void GetLeaveDetails()
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        Hashtable hashtable = new Hashtable();
        if (txtFromDt.Text.Trim() != "" && txtToDt.Text.Trim() != "")
        {
            hashtable.Add("@FromDt", dtpFromDt.GetDateValue().ToString("dd-MMM-yyyy"));
            hashtable.Add("@ToDt", dtpToDt.GetDateValue().ToString("dd-MMM-yyyy"));
        }
        DataTable dataTable = obj.GetDataTable("HR_GetLeaveDtlsForCancel", hashtable);
        grdLeave.DataSource = dataTable.DefaultView;
        grdLeave.DataBind();
        lblRecCount.Text = "No. of Record(s) : " + dataTable.Rows.Count;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetLeaveDetails();
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        TextBox control1 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtReason");
        HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfId");
        ModalPopupExtender control3 = (ModalPopupExtender)((Control)sender).Parent.Parent.FindControl("mdlPopUp");
        if (control1.Text.Trim() == "")
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Enter the Resaon of Cancel');", true);
            control3.Show();
            control1.Focus();
        }
        else
        {
            string str = obj.ExecuteScalar("HR_CancelLeave", new Hashtable()
      {
        {
           "@LeaveApplyId",
           control2.Value
        },
        {
           "@Remarks",
           control1.Text
        },
        {
           "@UserId",
          Session["User_Id"]
        }
      });
            if (str.Trim() == string.Empty)
            {
                GetLeaveDetails();
                trMsg.Style["background-color"] = "Green";
                lblMsg.Text = "Leave Cancelled Succesfully";
            }
            else
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = str;
            }
        }
    }
}