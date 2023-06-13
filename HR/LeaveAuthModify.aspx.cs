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

public partial class HR_LeaveAuthModify : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        bindDropDown(drpEmp, "SELECT EmpId,SevName FROM dbo.HR_EmployeeMaster WHERE ActiveStatus=1 ORDER BY SevName", "SevName", "EmpId");
        ViewState["LeaveYr"] = GetLeaveYear();
        lblYear.Text = ViewState["Yr"].ToString();
    }

    private string GetLeaveYear()
    {
        string str1 = string.Empty;
        ViewState["LeaveAcYr"] = ConfigurationManager.AppSettings["LeaveAcYr"].ToString().Trim();
        if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL")
        {
            str1 = DateTime.Now.Year.ToString();
            ViewState["Yr"] = str1;
        }
        else if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "SESS")
        {
            string str2 = obj.ExecuteScalarQry("select dbo.fuGetSessionYr('" + DateTime.Now.ToString("dd-MMM-yyyy") + "')");
            ViewState["Yr"] = str2;
            string[] strArray = str2.Split('-');
            str1 = strArray[0] + strArray[1];
        }
        return str1;
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

    protected void drpEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpEmp.SelectedIndex > 0)
        {
            BindData();
        }
        else
        {
            grdLeave.DataSource = null;
            grdLeave.DataBind();
            lblRecCount.Text = string.Empty;
            btnUpdateList.Visible = false;
        }
        drpEmp.Focus();
    }

    private void BindData()
    {
        lblMsg.Text = string.Empty;
        trMsg.Style["background-color"] = (string)null;
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("HR_GetEmpLeaveDtls", new Hashtable()
    {
      {
         "@EmpId",
         drpEmp.SelectedValue
      },
      {
         "@FY",
         Convert.ToInt32(ViewState["LeaveYr"])
      }
    });
        grdLeave.DataSource = dataTable2;
        grdLeave.DataBind();
        lblRecCount.Text = "No of Records : " + dataTable2.Rows.Count;
        if (dataTable2.Rows.Count > 0)
            btnUpdateList.Visible = true;
        else
            btnUpdateList.Visible = false;
    }

    protected void btnUpdateList_Click(object sender, EventArgs e)
    {
        string empty = string.Empty;
        string str = Chk();
        if (str.Trim() == string.Empty)
        {
            foreach (GridViewRow row in grdLeave.Rows)
            {
                HiddenField control1 = (HiddenField)row.FindControl("hfLeaveId");
                HiddenField control2 = (HiddenField)row.FindControl("hfLeaveDtlsId");
                TextBox control3 = (TextBox)row.FindControl("txtAuthDays");
                Hashtable hashtable = new Hashtable();
                if (control2.Value.Trim() != string.Empty)
                    hashtable.Add("@LeaveDtlsId", control2.Value);
                hashtable.Add("@EmpId", drpEmp.SelectedValue);
                hashtable.Add("@LeaveId", control1.Value);
                hashtable.Add("@AuthDays", control3.Text);
                hashtable.Add("@FY", ViewState["LeaveYr"]);
                hashtable.Add("@UserId", Session["User_Id"]);
                if (obj.ExecuteScalar("HR_LeaveAuthEmp", hashtable).Trim() != string.Empty)
                {
                    str = "Failed to Save Data. Try Again.";
                    break;
                }
            }
            if (str.Trim() == string.Empty)
            {
                BindData();
                lblMsg.Text = "Data Saved Successfully";
                trMsg.Style["background-color"] = "Green";
            }
            else
            {
                lblMsg.Text = str;
                trMsg.Style["background-color"] = "Red";
            }
        }
        else
        {
            lblMsg.Text = str;
            trMsg.Style["background-color"] = "Red";
        }
    }

    private string Chk()
    {
        string str = string.Empty;
        foreach (GridViewRow row in grdLeave.Rows)
        {
            Label control1 = (Label)row.FindControl("lblLeaveCode");
            TextBox control2 = (TextBox)row.FindControl("txtAuthDays");
            HiddenField control3 = (HiddenField)row.FindControl("hfAvlDays");
            if (control3.Value.Trim() != string.Empty && Convert.ToDouble(control2.Text) < Convert.ToDouble(control3.Value))
                str = str + control1.Text + " avalied more than the No of Authorized Days";
        }
        return str;
    }

    protected void btnAuthLeave_Click(object sender, EventArgs e)
    {
        Response.Redirect("LeaveAuthorize.aspx");
    }
}