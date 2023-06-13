using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Hostel_SetHostPrevBal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblmsg.Text = string.Empty;
        lblRecCount.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnUpdate1.Enabled = false;
        if (Page.IsPostBack)
            return;
        IntializePage();
    }

    private void IntializePage()
    {
        FillSessionDropdown();
        FillClassDropDown();
        lblRecCount.Text = "";
        drpSelectStudent.SelectedIndex = -1;
        grdStudPrevAC.Visible = false;
    }

    private void FillClassDropDown()
    {
        drpClass.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassID";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void FillSessionDropdown()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillSectionDropDown()
    {
        drpSection.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
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

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdStudPrevAC.Visible = false;
        lblRecCount.Text = string.Empty;
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdStudPrevAC.Visible = false;
        lblRecCount.Text = string.Empty;
        FillSectionDropDown();
        FillSelectStudent();
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSection.SelectedIndex > 0)
            FillSelectStudent();
        else
            drpSelectStudent.Items.Clear();
        grdStudPrevAC.Visible = false;
        lblRecCount.Text = string.Empty;
    }

    private void FillSelectStudent()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@SessionYear", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@ClassID", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        drpSelectStudent.DataSource = clsDal.GetDataTable("Host_StudForPrevBal", hashtable);
        drpSelectStudent.DataTextField = "fullname";
        drpSelectStudent.DataValueField = "admnno";
        drpSelectStudent.DataBind();
        drpSelectStudent.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    protected void drpSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdStudPrevAC.Visible = false;
        lblRecCount.Text = string.Empty;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        string str = "select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=4";
        clsDAL clsDal = new clsDAL();
        try
        {
            int.Parse(clsDal.ExecuteScalarQry(str));
            grdStudPrevAC.Visible = true;
            FillGrid(grdStudPrevAC, "Host_StudListForprevBal");
            lblmsg.Visible = false;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            lblmsg.Text = "Account Head is not defined for Old dues. First set the account Head";
        }
    }

    private void FillGrid(GridView gv, string procName)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@Session", drpSession.SelectedValue.Trim());
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpClass.SelectedValue.Trim());
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue.Trim());
        if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@AdmnNo", drpSelectStudent.SelectedValue.Trim());
        DataTable dataTable = clsDal.GetDataTable(procName, hashtable);
        if (dataTable.Rows.Count > 0)
        {
            gv.DataSource = dataTable;
            gv.DataBind();
            gv.Visible = true;
            lblRecCount.Text = "<font color='maroon'><b>No. of Student(s): " + dataTable.Rows.Count.ToString() + "</b></font>";
            lblRecCount.Visible = true;
            btnUpdate.Enabled = true;
            btnUpdate1.Enabled = true;
        }
        else
        {
            gv.Visible = false;
            btnUpdate.Enabled = false;
            btnUpdate1.Enabled = false;
            lblRecCount.Text = "";
            ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
        }
    }

    protected void btnSetActHead_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Masters/SetAccountHead.aspx");
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        getValidInput();
        FillGrid(grdStudPrevAC, "Host_StudListForprevBal");
        lblmsg.Visible = true;
        IntializePage();
        lblRecCount.Text = string.Empty;
        btnUpdate.Enabled = false;
        btnUpdate1.Enabled = false;
        UpdatePanel1.Update();
    }

    private void getValidInput()
    {
        int num1 = 0;
        string empty1 = string.Empty;
        foreach (Control row in grdStudPrevAC.Rows)
            ((WebControl)row.FindControl("txtPrevAmount")).BackColor = Color.Transparent;
        foreach (GridViewRow row in grdStudPrevAC.Rows)
        {
            clsDAL clsDal = new clsDAL();
            TextBox control1 = (TextBox)row.FindControl("txtPrevAmount");
            Label control2 = (Label)row.FindControl("lblAdmnNo");
            if (clsDal.ExecuteScalarQry("select Receipt_VrNo from dbo.Host_FeeLedger where TransDesc='Previous Balance' and AdmnNo=" + control2.Text.Trim()) != "")
            {
                control1.BackColor = Color.LightBlue;
            }
            else
            {
                Decimal num2 = new Decimal(0);
                string empty2 = string.Empty;
                Decimal num3;
                try
                {
                    num3 = Convert.ToDecimal(((TextBox)row.FindControl("txtPrevAmount")).Text.ToString().Trim());
                    empty2 = Convert.ToString(((Label)row.FindControl("lblAdmnNo")).Text.ToString().Trim());
                }
                catch (Exception ex)
                {
                    num3 = new Decimal(0);
                }
                Hashtable hashtable = new Hashtable();
                DateTime dateTime = Convert.ToDateTime("MAR-31-" + Convert.ToInt32(drpSession.SelectedValue.ToString().Trim().Substring(0, 4)).ToString().Trim());
                hashtable.Add("TransDate", dateTime);
                hashtable.Add("AdmnNo", empty2);
                hashtable.Add("TransDesc", "Previous Balance");
                hashtable.Add("debit", num3);
                hashtable.Add("credit", 0);
                hashtable.Add("balance", num3);
                hashtable.Add("Receipt_VrNo", "");
                hashtable.Add("userid", Convert.ToInt32(Session["User_Id"].ToString().Trim()));
                hashtable.Add("schoolid", Session["SchoolId"].ToString());
                clsDal.GetDataTable("Host_Insert_PrevYrBal", hashtable);
                ++num1;
            }
            if (num1 != 0)
            {
                lblmsg.Text = "Data saved successfully !";
                lblmsg.ForeColor = Color.Green;
            }
            else
            {
                lblmsg.Text = "Please Enter amount !";
                lblmsg.ForeColor = Color.Red;
            }
        }
    }
}