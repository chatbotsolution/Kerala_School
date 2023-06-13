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

public partial class HR_Allowances : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private int RecCount;
    private string AllowanceMsg;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
            FillGrid();
        trMsg.Style["Background-Color"] = (string)null;
        lblMsg.Text = string.Empty;
    }

    private void FillGrid()
    {
        ClearFields();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("HR_GetAllowanceList");
        grdAllowance.DataSource = dataTable2.DefaultView;
        grdAllowance.DataBind();
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
                lblMsg.Text = AllowanceMsg;
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
        string str = obj.ExecuteScalar("HR_DeleteAllowance", new Hashtable()
    {
      {
         "@AllowanceId",
         id
      }
    });
        if (!(str.Trim() != string.Empty))
            return 0;
        AllowanceMsg = AllowanceMsg + "</br>" + str;
        return 1;
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        HiddenField control1 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfId");
        HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfPerAmt");
        HiddenField control3 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfEffectiveDt");
        HiddenField control4 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfType");
        Label control5 = (Label)((Control)sender).Parent.Parent.FindControl("lblValue");
        Label control6 = (Label)((Control)sender).Parent.Parent.FindControl("lblAllowance");
        hfAllowanceId.Value = control1.Value;
        txtAllowance.Text = control6.Text;
        if (control4.Value.Trim().ToUpper() == "SYSTEM")
            txtAllowance.Enabled = false;
        else
            txtAllowance.Enabled = true;
        txtValue.Text = control5.Text;
        drpPerAmt.SelectedValue = control2.Value;
        if (!(control3.Value != string.Empty))
            return;
        dtpDate.SetDateValue(Convert.ToDateTime(control3.Value));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (drpPerAmt.SelectedValue == "P" && Convert.ToDouble(txtValue.Text) >= 100.0)
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = "Percantage should be less than 100.";
            txtValue.Focus();
        }
        else
            InsUpdtDeductionMaster();
    }

    private void InsUpdtDeductionMaster()
    {
        Hashtable hashtable = new Hashtable();
        if (hfAllowanceId.Value != "0")
            hashtable.Add("@AllowanceId", hfAllowanceId.Value);
        hashtable.Add("@Allowance", txtAllowance.Text.Trim());
        hashtable.Add("@PerAmt", drpPerAmt.SelectedValue);
        hashtable.Add("@Value", txtValue.Text);
        if (txtDate.Text.Trim() != string.Empty)
            hashtable.Add("@StartDt", Convert.ToDateTime(dtpDate.GetDateValue().ToString("MMM-dd-yyyy")));
        hashtable.Add("@UserId", Session["User_Id"]);
        string str = obj.ExecuteScalar("HR_InsUpdtAllowanceMaster", hashtable);
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
        txtAllowance.Text = string.Empty;
        hfAllowanceId.Value = "0";
        drpPerAmt.SelectedValue = "A";
        txtValue.Text = "0";
        txtDate.Text = string.Empty;
    }
}