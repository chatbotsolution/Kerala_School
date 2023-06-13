using ASP;
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
public partial class HR_LeaveBalance : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        trMsg.Style["background-color"] = (string)null;
        if (Page.IsPostBack)
            return;
        BindDropDown(drpEmp, "SELECT EmpId,SevName FROM dbo.HR_EmployeeMaster WHERE ActiveStatus=1 ORDER BY SevName", "SevName", "EmpId");
        GetPrevFinYear();
    }

    private void GetPrevFinYear()
    {
        ViewState["LeaveAcYr"] = ConfigurationManager.AppSettings["LeaveAcYr"].ToString().Trim();
        string str1 = obj.ExecuteScalarQry("select top 1 CalYear from dbo.HR_EmpLeaveDtls where LeaveDtlsId<0");
        string empty = string.Empty;
        if (str1.Trim() == string.Empty)
        {
            if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "SESS")
            {
                string str2 = obj.ExecuteScalarQry("select top 1 FY from dbo.ACTS_FinancialYear where IsFinalized IS NULL OR IsFinalized=0");
                if (str2.Trim() == string.Empty)
                {
                    lblMsg.Text = "First, Initialize Current Financial Year";
                    trMsg.Style["background-color"] = "Red";
                }
                else
                    str1 = (Convert.ToInt32(str2.Substring(0, 4)) - 1).ToString() + "-" + (Convert.ToInt32(str2.Substring(5)) - 1).ToString();
                string[] strArray = str1.Split('-');
                ViewState["LeaveYr"] = (strArray[0] + strArray[1]);
            }
            else if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL")
            {
                str1 = DateTime.Now.AddYears(-1).Year.ToString();
                ViewState["LeaveYr"] = str1;
            }
        }
        else if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "SESS")
        {
            str1 = str1.Substring(0, 4) + "-" + str1.Substring(4);
            string[] strArray = str1.Split('-');
            ViewState["LeaveYr"] = (strArray[0] + strArray[1]);
        }
        else if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL")
            ViewState["LeaveYr"] = str1;
        if (!(str1.Trim() != string.Empty))
            return;
        if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "SESS")
        {
            lblFinYr.Text = "1 Apr " + (Convert.ToInt32(str1.Substring(0, 4).Trim()) + 1).ToString();
        }
        else
        {
            if (!(ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL"))
                return;
            lblFinYr.Text = "1 Jan " + (Convert.ToInt32(str1.Trim()) + 1).ToString();
        }
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    protected void drpEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpEmp.SelectedIndex > 0)
            GetLeaveBalance();
        else
            row1.Visible = false;
        drpEmp.Focus();
    }

    private void GetLeaveBalance()
    {
        obj = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("HR_GetLeaveBalance", new Hashtable()
    {
      {
         "@EmpId",
         drpEmp.SelectedValue
      }
    });
        if (dataTable2.Rows.Count > 0)
        {
            grdLeave.DataSource = dataTable2;
            grdLeave.DataBind();
            row1.Visible = true;
        }
        else
        {
            lblMsg.Text = "No Leave Available. First Define and Authorize Leave";
            row1.Visible = false;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string str = string.Empty;
            foreach (GridViewRow row in grdLeave.Rows)
            {
                obj = new clsDAL();
                Label control1 = row.FindControl("lblLeaveCode") as Label;
                HiddenField control2 = row.FindControl("hfLeaveDtlsId") as HiddenField;
                HiddenField control3 = row.FindControl("hfLeaveId") as HiddenField;
                HiddenField control4 = row.FindControl("hfDaysAvailed") as HiddenField;
                TextBox control5 = row.FindControl("txtBalance") as TextBox;
                if (Convert.ToDouble(control5.Text) < Convert.ToDouble(control4.Value))
                {
                    str = control1.Text + " balance leave already availed. Can't be updated.<br/>";
                }
                else
                {
                    Hashtable hashtable = new Hashtable();
                    if (control2.Value.Trim() != string.Empty)
                        hashtable.Add("@LeaveDtlsId", control2.Value);
                    hashtable.Add("@FinYr", Convert.ToInt32(ViewState["LeaveYr"].ToString().Trim()));
                    hashtable.Add("@EmpId", drpEmp.SelectedValue);
                    hashtable.Add("@LeaveId", control3.Value);
                    hashtable.Add("@BalanceLeave", Convert.ToDouble(control5.Text));
                    hashtable.Add("@UserId", Session["User_Id"]);
                    str = obj.ExecuteScalar("HR_InsUpdtLeaveBalance", hashtable);
                    if (str.Trim() != string.Empty)
                        str = "Failed to Update " + control1.Text + " Balance<br/>";
                }
            }
            if (str.Trim() == string.Empty)
            {
                GetLeaveBalance();
                lblMsg.Text = "Data Saved Successfully";
                trMsg.Style["background-color"] = "Green";
            }
            else
            {
                lblMsg.Text = str;
                trMsg.Style["background-color"] = "Red";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
            trMsg.Style["background-color"] = "Red";
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("HRInit.aspx");
    }
}