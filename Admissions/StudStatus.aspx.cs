using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admissions_StudStatus : System.Web.UI.Page
{
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblRecCount.Text = "";
        if (Page.IsPostBack)
            return;
        DRP.FillStatus(drpStatus);
        FillSessionDropdown();
        FillClassDropDown();
        grdStudStatus.Visible = false;
    }

    private void FillSessionDropdown()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillClassDropDown()
    {
        drpClass.DataSource = new Common().GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassID";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void FillSectionDropDown()
    {
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "classId",
         drpClass.SelectedValue
      },
      {
         "session",
         drpSession.SelectedValue
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("ALL", "0"));
    }

    private void FillGrid()
    {
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@Session", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        DataTable dataTable = common.GetDataTable("ps_sp_get_StudNamesForStatus", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            grdStudStatus.DataSource = dataTable;
            grdStudStatus.DataBind();
            grdStudStatus.Visible = true;
            lblRecCount.Text = "Total Number of Student: " + dataTable.Rows.Count.ToString();
            lblRecCount.Visible = true;
        }
        else
        {
            grdStudStatus.Visible = false;
            lblRecCount.Text = "";
            ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
        }
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdStudStatus.Visible = false;
        FillSectionDropDown();
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdStudStatus.Visible = false;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdStudStatus.Visible = false;
    }

    private void IntializePage()
    {
        drpClass.SelectedIndex = -1;
        drpSession.SelectedIndex = -1;
        grdStudStatus.Visible = false;
        txtDate.Text = "";
        drpStatus.SelectedIndex = 0;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        grdStudStatus.Visible = true;
        FillGrid();
        lblmsg.Visible = false;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                Addstatus(obj.ToString());
            FillGrid();
            IntializePage();
            lblRecCount.Visible = false;
            lblmsg.Visible = true;
        }
    }

    private void Addstatus(string AdmnNo)
    {
        string empty = string.Empty;
        Hashtable hashtable = new Hashtable();
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@adminno", AdmnNo);
        if (drpStatus.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@StatusId", drpStatus.SelectedValue);
        hashtable.Add("@withdrawaldate", PopCalendar3.GetDateValue());
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@fromclass", drpClass.SelectedValue);
        hashtable.Add("@reason", "");
        hashtable.Add("@UserID", Convert.ToInt32(Session["User_Id"]));
        hashtable.Add("@UserDate", DateTime.Now.ToString("MM/dd/yyyy"));
        hashtable.Add("@schoolid", 1);
        DataTable dataTable2 = common.GetDataTable("ps_sp_insert_StudStatusWithdrawal", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            empty += dataTable2.Rows[0][0].ToString();
        }
        else
        {
            lblmsg.Text = "Student Status Modified successfully";
            UpdateStudMster(AdmnNo);
        }
        if (!(empty != ""))
            return;
        lblmsg.Text = "Status already modified for " + empty + " admission number";
    }

    private void UpdateStudMster(string AdmnNo)
    {
        Hashtable hashtable = new Hashtable();
        Common common = new Common();
        DataTable dataTable = new DataTable();
        hashtable.Add("@adminno", AdmnNo);
        if (drpStatus.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@status", drpStatus.SelectedValue);
        common.GetDataTable("ps_sp_insert_StudentMaster", hashtable);
    }
}