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

public partial class HR_Deductions : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private int RecCount;
    private string dedMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            BindDropdown(drpAccGroup, "SELECT AG_Code,AG_Name from dbo.Acts_AccountGroups ORDER BY AG_Name ", "AG_Name", "AG_Code");
            BindDropdown(drpAccHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads Where AG_Code in(6,14,15,19,21,24,25) ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
            FillGrid();
        }
        trMsg.Style["Background-Color"] = (string)null;
        lblMsg.Text = string.Empty;
    }

    private void BindDropdown(DropDownList drp, string Qry, string Text, string Value)
    {
        DataTable dataTableQry = obj.GetDataTableQry(Qry.Trim());
        drp.DataSource = dataTableQry.DefaultView;
        drp.DataTextField = Text;
        drp.DataValueField = Value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    private void FillGrid()
    {
        ClearFields();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("HR_GetDeductionLIst");
        grdDeduction.DataSource = dataTable2.DefaultView;
        grdDeduction.DataBind();
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
                lblMsg.Text = dedMsg;
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
        string str = obj.ExecuteScalar("HR_DeleteDeduction", new Hashtable()
    {
      {
         "@DedTypeId",
         id
      }
    });
        if (!(str.Trim() != string.Empty))
            return 0;
        dedMsg = dedMsg + "</br>" + str;
        return 1;
    }

    protected void btnEdit_Click(object sender, ImageClickEventArgs e)
    {
        HiddenField control1 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfId");
        HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfAccHeadId");
        HiddenField control3 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfType");
        HiddenField control4 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfStartDt");
        HiddenField control5 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfPerAmt");
        Label control6 = (Label)((Control)sender).Parent.Parent.FindControl("lblDedDesc");
        Label control7 = (Label)((Control)sender).Parent.Parent.FindControl("lblEmpShare");
        Label control8 = (Label)((Control)sender).Parent.Parent.FindControl("lblOrgShare");
        hfDedId.Value = control1.Value;
        txtDeduction.Text = control6.Text;
        if (control3.Value.ToUpper() == "SYSTEM")
            txtDeduction.Enabled = false;
        else
            txtDeduction.Enabled = true;
        drpAccHead.SelectedValue = control2.Value;
        rdbtnPerAmt.SelectedValue = control5.Value;
        if (control4.Value.Trim() != string.Empty)
            dtpDate.SetDateValue(Convert.ToDateTime(control4.Value));
        txtEmpShare.Text = control7.Text;
        txtOrgShare.Text = control8.Text;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (rdbtnPerAmt.SelectedValue == "P" && Convert.ToDouble(txtEmpShare.Text) >= 100.0)
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Employee Share Percentage should be less than 100.";
            txtEmpShare.Focus();
        }
        else if (rdbtnPerAmt.SelectedValue == "P" && Convert.ToDouble(txtOrgShare.Text) >= 100.0)
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Employer Share Percentage should be less than 100.";
            txtOrgShare.Focus();
        }
        else
            InsUpdtDeductionMaster();
    }

    private void InsUpdtDeductionMaster()
    {
        Hashtable hashtable = new Hashtable();
        if (hfDedId.Value != "0")
            hashtable.Add("@DedTypeId", hfDedId.Value);
        hashtable.Add("@DedDetails", txtDeduction.Text.Trim().ToUpper());
        hashtable.Add("@AcctsHeadId", int.Parse(drpAccHead.SelectedValue.ToString()));
        hashtable.Add("@PerAmt", rdbtnPerAmt.SelectedValue);
        hashtable.Add("@EmpValue", Convert.ToDouble(txtEmpShare.Text.Trim()));
        hashtable.Add("@OrgValue", Convert.ToDouble(txtOrgShare.Text.Trim()));
        hashtable.Add("@StartDate", Convert.ToDateTime(dtpDate.GetDateValue().ToString("MMM-dd-yyyy")));
        hashtable.Add("@UserId", Session["User_Id"]);
        string str = obj.ExecuteScalar("HR_InsUpdtDedMaster", hashtable);
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
        txtDeduction.Text = string.Empty;
        hfDedId.Value = "0";
        drpAccHead.SelectedIndex = 0;
        drpAccHead.Enabled = true;
        txtDeduction.Enabled = true;
        txtEmpShare.Text = "0";
        txtOrgShare.Text = "0";
        rdbtnPerAmt.SelectedValue = "A";
        txtDate.Text = string.Empty;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@AcctsHead", txtAccHead.Text.Trim());
        hashtable.Add("@AG_Code", drpAccGroup.SelectedValue);
        hashtable.Add("@AH_Type", "System");
        hashtable.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
        DataTable dataTable = obj.GetDataTable("HR_InsertAccountHeads", hashtable);
        if (dataTable.Rows[0][0].ToString().Trim().ToUpper() == "S")
        {
            BindDropdown(drpAccHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
            drpAccHead.SelectedValue = dataTable.Rows[0][1].ToString();
       drpAccHead.Focus();
        }
        else
        {
            lblmsg1.Text = dataTable.Rows[0][0].ToString();
            modPopUpNewAcHead.Show();
        }
    }
}