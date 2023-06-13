using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class HR_LeaveClaim : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (!ChkIsHRUsed())
        {
            Response.Redirect("HRHome.aspx?IsHRUsed=0");
        }
        else
        {
            BindEmployee(drpEmpName);
            dtpClaimDt.SetDateValue(DateTime.Now);
        }
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private string GetLeaveYear()
    {
        string str1 = string.Empty;
        ViewState["LeaveAcYr"] = ConfigurationManager.AppSettings["LeaveAcYr"].ToString().Trim();
        if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL")
        {
            str1 = DateTime.Now.Year.ToString();
            ViewState["Yr"] = str1;
            ViewState["YearStart"] = Convert.ToDateTime("01-JAN-" + str1);
            ViewState["YearEnd"] = Convert.ToDateTime("31-DEC-" + str1);
        }
        else if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "SESS")
        {
            string str2 = obj.ExecuteScalarQry("select dbo.fuGetSessionYr('" + DateTime.Now.ToString("dd-MMM-yyyy") + "')");
            ViewState["Yr"] = str2;
            string[] strArray = str2.Split('-');
            str1 = strArray[0] + strArray[1];
            ViewState["YearStart"] = Convert.ToDateTime("01-APR-" + strArray[0]);
            ViewState["YearEnd"] = Convert.ToDateTime("31-MAR-" + (Convert.ToInt32(strArray[0]) + 1).ToString());
        }
        return str1;
    }

    private void BindEmployee(DropDownList drp)
    {
        DataTable dataTable = obj.GetDataTable("HR_GetEmpList");
        drp.DataSource = dataTable;
        drp.DataTextField = "Employee";
        drp.DataValueField = "EmpId";
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    protected void grdLeave_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        trMsg.Style["background-color"] = "Transparent";
        lblMsg.Text = string.Empty;
        int int32 = Convert.ToInt32(e.CommandArgument);
        HiddenField control1 = grdLeave.Rows[int32].FindControl("hfLeaveApplyId") as HiddenField;
        HiddenField control2 = grdLeave.Rows[int32].FindControl("hfLeaveStartDt") as HiddenField;
        HiddenField control3 = grdLeave.Rows[int32].FindControl("hfLeaveEndDt") as HiddenField;
        Label control4 = grdLeave.Rows[int32].FindControl("lblApproved") as Label;
        Label control5 = grdLeave.Rows[int32].FindControl("lblAvailed") as Label;
        hfId.Value = control1.Value;
        txtDaysApproved.Text = control4.Text;
        txtDaysAvailed.Text = control5.Text;
        txtExtraDays.Text = (Convert.ToDecimal(control5.Text) - Convert.ToDecimal(control4.Text)).ToString();
        ViewState["LeaveEndDt"] = control3.Value;
        txtWPL.Text = obj.ExecuteScalar("HR_GetTotalWPL", new Hashtable()
    {
      {
         "@EmpId",
         drpEmpName.SelectedValue
      },
      {
         "@LeaveStartDt",
         Convert.ToDateTime(control2.Value)
      },
      {
         "@LeaveEndDt",
         Convert.ToDateTime(control3.Value)
      }
    });
        txtReason.Focus();
    }

    protected void drpEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetLeaveForClaim();
        drpEmpName.Focus();
    }

    private void GetLeaveForClaim()
    {
        trMsg.Style["background-color"] = "Transparent";
        lblMsg.Text = string.Empty;
        grdLeave.Visible = true;
        if (drpEmpName.SelectedIndex > 0)
        {
            grdLeave.DataSource = obj.GetDataTable("HR_GetLeaveForClaim", new Hashtable()
      {
        {
           "@EmpId",
           drpEmpName.SelectedValue
        }
      });
            grdLeave.DataBind();
        }
        else
        {
            grdLeave.DataSource = null;
            grdLeave.DataBind();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = "Transparent";
        lblMsg.Text = string.Empty;
        string empty = string.Empty;
        string str = IsValid();
        if (str.Trim() == string.Empty)
            str = UploadDoc();
        if (str.Trim().ToUpper() == "S")
        {
            ResetFields();
            trMsg.Style["background-color"] = "Green";
            lblMsg.Text = "Data Saved Successfully";
        }
        else
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = str.Trim();
        }
    }

    private string IsValid()
    {
        string str = string.Empty;
        if (drpEmpName.SelectedIndex == 0)
        {
            drpEmpName.Focus();
            str = "Select an Employee";
        }
        else if (grdLeave.Rows.Count == 0)
        {
            drpEmpName.Focus();
            str = "No ML/MATL Leave available for Claim";
        }
        else if (hfId.Value == "0")
        {
            grdLeave.Focus();
            str = "Select a Leave from the List";
        }
        else if (txtClaimDt.Text == string.Empty)
        {
            txtClaimDt.Focus();
            str = "Enter a Claim Date";
        }
        else if (txtClaimDt.Text != string.Empty && dtpClaimDt.GetDateValue() < Convert.ToDateTime(ViewState["LeaveEndDt"]))
        {
            txtClaimDt.Focus();
            str = "Claim Date should be greater than Leave End Date";
        }
        else if (txtReason.Text == string.Empty)
        {
            txtReason.Focus();
            str = "Enter a Valid Reason for Extra Leave";
        }
        else if (!fuDoc.HasFile)
        {
            fuDoc.Focus();
            str = "Upload Related Documents";
        }
        return str;
    }

    private string UploadDoc()
    {
        string empty = string.Empty;
        string fileName = string.Empty;
        try
        {
            if (fuDoc.HasFile)
            {
                string str1 = fuDoc.FileName.Split('.')[1];
                string str2 = Server.MapPath("../Up_Files/Claim_Doc/");
                fileName = "UF_Claim_Emp" + drpEmpName.SelectedValue + "_Leave" + hfId.Value + "." + str1;
                fuDoc.SaveAs(str2 + fileName);
            }
            return SaveClaim(fileName);
        }
        catch (Exception ex)
        {
            return "Failed to Upload Document. Try Again.";
        }
    }

    private string SaveClaim(string fileName)
    {
        string empty = string.Empty;
        try
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@ClaimDate", dtpClaimDt.GetDateValue().ToString("dd-MMM-yyyy"));
            hashtable.Add("@LeaveApplyId", hfId.Value);
            hashtable.Add("@EmpId", drpEmpName.SelectedValue);
            hashtable.Add("@ExtraDaysAvailed", txtWPL.Text);
            hashtable.Add("@Reason", txtReason.Text.Trim());
            hashtable.Add("@ClaimStatus", "Pending");
            hashtable.Add("@UserId", Session["User_Id"]);
            if (fileName.Trim() != string.Empty)
                hashtable.Add("@RelatedFile", fileName);
            return obj.ExecuteScalar("HR_InsUpdtLeaveClaim", hashtable);
        }
        catch (Exception ex)
        {
            return "Failed to Save Data. Try Again.";
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ResetFields();
    }

    private void ResetFields()
    {
        ViewState["LeaveEndDt"] = null;
        trMsg.Style["background-color"] = "Transparent";
        lblMsg.Text = string.Empty;
        hfId.Value = "0";
        drpEmpName.SelectedIndex = 0;
        txtDaysApproved.Text = "0";
        txtDaysAvailed.Text = "0";
        txtExtraDays.Text = "0";
        txtWPL.Text = "0";
        txtReason.Text = string.Empty;
        grdLeave.DataSource = null;
        grdLeave.DataBind();
        grdLeave.Visible = false;
        dtpClaimDt.SetDateValue(DateTime.Now);
        drpEmpName.Focus();
    }
}