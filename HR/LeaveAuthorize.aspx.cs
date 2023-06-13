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

public partial class HR_LeaveAuthorize : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        BindDesignation(drpDesignation, "SELECT DesgId,Designation FROM dbo.HR_DesignationMaster WHERE TeachingStaff<>'M' ORDER BY Designation", "Designation", "DesgId");
        FillGrid();
        ViewState["LeaveYr"] = GetLeaveYear(DateTime.Now);
    }

    private string GetLeaveYear(DateTime LeaveStartDt)
    {
        string str = string.Empty;
        ViewState["LeaveAcYr"] = ConfigurationManager.AppSettings["LeaveAcYr"].ToString().Trim();
        if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "CAL")
            str = LeaveStartDt.Year.ToString();
        else if (ViewState["LeaveAcYr"].ToString().Trim().ToUpper() == "SESS")
        {
            string[] strArray = obj.ExecuteScalarQry("select dbo.fuGetSessionYr('" + LeaveStartDt.ToString("dd-MMM-yyyy") + "')").Split('-');
            str = strArray[0] + strArray[1];
        }
        return str;
    }

    private void BindDesignation(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    private void FillGrid()
    {
        trMsg.Style["Background-Color"] = string.Empty;
        lblMsg.Text = string.Empty;
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpDesignation.SelectedIndex > 0)
            hashtable.Add("@DesignationId", drpDesignation.SelectedValue);
        hashtable.Add("@CalYear", Convert.ToInt32(ViewState["LeaveYr"]));
        DataTable dataTable2 = obj.GetDataTable("HR_GetLeaveAuthorized", hashtable);
        grdLeave.DataSource = dataTable2.DefaultView;
        grdLeave.DataBind();
        grdLeave.Columns[5].Visible = drpDesignation.SelectedIndex != 0;
        lblRecCount.Text = "No Of Records : " + dataTable2.Rows.Count.ToString();
        if (dataTable2.Rows.Count > 0)
            btnUpdateList.Visible = true;
        else
            btnUpdateList.Visible = false;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        HiddenField control1 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfYear");
        HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfDesgId");
        HiddenField control3 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfLeaveId");
        TextBox control4 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtTotDaysAuth");
        TextBox control5 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtEncAllowed");
        TextBox control6 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtMaxDaysAllowed");
        TextBox control7 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtMinAttReq");
        string str = obj.ExecuteScalar("HR_InsUpdtLeaveAuthorized", new Hashtable()
    {
      {
         "@DesignationId",
         control2.Value
      },
      {
         "@LeaveId",
         control3.Value
      },
      {
         "@TotDaysAuth",
         control4.Text
      },
      {
         "@TotEncashAllowed",
         control5.Text
      },
      {
         "@MinAttandReq",
         control7.Text
      },
      {
         "@UserId",
        Session["User_Id"]
      },
      {
         "@MaxDaysAllowed",
         control6.Text
      },
      {
         "@CalYear",
         Convert.ToInt32(ViewState["LeaveYr"])
      }
    });
        if (str.Trim() == string.Empty)
        {
            LeaveAuthEmpwise(Convert.ToInt32(control2.Value), Convert.ToInt32(ViewState["LeaveYr"]), Convert.ToInt32(control3.Value), float.Parse(control4.Text));
            FillGrid();
            trMsg.Style["Background-Color"] = "Green";
            lblMsg.Text = "Record Updated Successfully";
        }
        else
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = str.ToString().Trim();
        }
    }

    protected void btnUpdateList_Click(object sender, EventArgs e)
    {
        int num = 0;
        if (drpDesignation.SelectedIndex == 0)
        {
            foreach (ListItem listItem in drpDesignation.Items)
            {
                if (listItem.Value != "0")
                    num = UpdateList(Convert.ToInt32(listItem.Value));
            }
        }
        else
            num = UpdateList(Convert.ToInt32(drpDesignation.SelectedValue));
        FillGrid();
        if (num == 0)
        {
            trMsg.Style["Background-Color"] = "Green";
            lblMsg.Text = "List Updated Successfully";
        }
        else
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = "Error Updating " + num + " Record(s)";
        }
    }

    private int UpdateList(int desgId)
    {
        int num = 0;
        foreach (GridViewRow row in grdLeave.Rows)
        {
            HiddenField control1 = (HiddenField)row.FindControl("hfYear");
            HiddenField control2 = (HiddenField)row.FindControl("hfDesgId");
            HiddenField control3 = (HiddenField)row.FindControl("hfLeaveId");
            TextBox control4 = (TextBox)row.FindControl("txtTotDaysAuth");
            TextBox control5 = (TextBox)row.FindControl("txtEncAllowed");
            TextBox control6 = (TextBox)row.FindControl("txtMaxDaysAllowed");
            TextBox control7 = (TextBox)row.FindControl("txtMinAttReq");
            if (obj.ExecuteScalar("HR_InsUpdtLeaveAuthorized", new Hashtable()
      {
        {
           "@DesignationId",
           desgId
        },
        {
           "@LeaveId",
           control3.Value
        },
        {
           "@TotDaysAuth",
           control4.Text
        },
        {
           "@TotEncashAllowed",
           control5.Text
        },
        {
           "@MinAttandReq",
           control7.Text
        },
        {
           "@UserId",
          Session["User_Id"]
        },
        {
           "@MaxDaysAllowed",
           control6.Text
        },
        {
           "@CalYear",
           Convert.ToInt32(ViewState["LeaveYr"])
        }
      }).Trim() != string.Empty)
                ++num;
            else
                LeaveAuthEmpwise(desgId, Convert.ToInt32(ViewState["LeaveYr"]), Convert.ToInt32(control3.Value), float.Parse(control4.Text));
        }
        return num;
    }

    private void LeaveAuthEmpwise(int desgId, int calYear, int leaveId, float authDays)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTableQry = obj.GetDataTableQry("SELECT EmpId FROM dbo.HR_EmployeeMaster WHERE DesignationId=" + desgId);
        if (dataTableQry.Rows.Count <= 0)
            return;
        foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
        {
            hashtable.Clear();
            hashtable.Add("@EmpId", row["EmpId"]);
            hashtable.Add("@CalYear", calYear);
            hashtable.Add("@LeaveId", leaveId);
            hashtable.Add("@UserId", Session["User_Id"]);
            hashtable.Add("@AuthDays", authDays);
            obj.ExecuteScalar("HR_LeaveAuthEmpwise", hashtable);
        }
    }

    protected void drpDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        drpDesignation.Focus();
    }

    protected void btnModAuth_Click(object sender, EventArgs e)
    {
        Response.Redirect("LeaveAuthModify.aspx");
    }
}