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

public partial class Masters_SetPrevBal : System.Web.UI.Page
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
       drpSection.SelectedIndex = 0;
       drpSelectStudent.SelectedIndex = 0;
       lblRecCount.Text = "";
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
        Hashtable ht = new Hashtable();
        if (drpSession.Items.Count > 0)
            ht.Add("@SessionYear", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            ht.Add("@ClassID", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            ht.Add("@Section", drpSection.SelectedValue);
       drpSelectStudent.DataSource = clsDal.GetDataTable("ps_StudForPrevBal", ht);
       drpSelectStudent.DataTextField = "fullname";
       drpSelectStudent.DataValueField = "admnno";
       drpSelectStudent.DataBind();
       drpSelectStudent.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        string qry = "select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=4";
        clsDAL clsDal = new clsDAL();
        try
        {
            int.Parse(clsDal.ExecuteScalarQry(qry));
           grdStudPrevAC.Visible = true;
           FillGrid(grdStudPrevAC, "ps_sp_get_StudListForprevBal");
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
        Hashtable ht = new Hashtable();
        if (drpSession.Items.Count > 0)
            ht.Add("@Session", drpSession.SelectedValue.Trim());
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            ht.Add("@Class", drpClass.SelectedValue.Trim());
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            ht.Add("@Section", drpSection.SelectedValue.Trim());
        if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
            ht.Add("@AdmnNo", drpSelectStudent.SelectedValue.Trim());
        DataTable dataTable = clsDal.GetDataTable(procName, ht);
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
            ScriptManager.RegisterClientScriptBlock((Control)btnShow,btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
       getValidInput();
       FillGrid(grdStudPrevAC, "ps_sp_get_StudListForprevBal");
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
            Decimal num2 = new Decimal(0);
            TextBox control1 = (TextBox)row.FindControl("txtPrevAmount");
            TextBox control2 = (TextBox)row.FindControl("txtPrevAmount2");
            Label control3 = (Label)row.FindControl("Label1");
            if (control1 != null && control2 != null && control3 != null)
            {
                num2 = Convert.ToDecimal(control1.Text) - Convert.ToDecimal(control2.Text);
                control3.Text = (Convert.ToDecimal(control2.Text) + num2).ToString();
                string str = string.Format("textBoxOnBlur('{0}', '{1}', '{2}');", control1.ClientID, control2.ClientID, control3.ClientID);
                control1.Attributes.Add("onblur", str);
            }
            clsDAL clsDal = new clsDAL();
            TextBox control4 = (TextBox)row.FindControl("txtPrevAmount");
            Label control5 = (Label)row.FindControl("lblAdmnNo");
            if (clsDal.ExecuteScalarQry("select Receipt_VrNo from dbo.PS_FeeLedger where TransDesc='Previous Balance' and AdmnNo=" + control5.Text.Trim()) != "")
            {
                Decimal num3 = new Decimal(0);
                string empty2 = string.Empty;
                Decimal num4;
                try
                {
                    num4 = Convert.ToDecimal(control2.Text) + num2;
                    empty2 = Convert.ToString(((Label)row.FindControl("lblAdmnNo")).Text.ToString().Trim());
                }
                catch (Exception ex)
                {
                    num4 = new Decimal(0);
                }
                Hashtable ht = new Hashtable();
                DateTime dateTime = Convert.ToDateTime("MAR-31-" + Convert.ToInt32(drpSession.SelectedValue.ToString().Trim().Substring(0, 4)).ToString().Trim());
                ht.Add("TransDate", dateTime);
                ht.Add("AdmnNo", empty2);
                ht.Add("TransDesc", "Previous Balance");
                Decimal num5 = Convert.ToDecimal(clsDal.ExecuteScalarQry("select Debit from dbo.PS_FeeLedger where TransDesc='Previous Balance' and AdmnNo=" + control5.Text.Trim())) + num2;
                ht.Add("debit", num5);
                ht.Add("credit", 0);
                ht.Add("balance", num4);
                ht.Add("Receipt_VrNo", "");
                ht.Add("userid", Convert.ToInt32(Session["User_Id"].ToString().Trim()));
                ht.Add("schoolid", Session["SchoolId"].ToString());
                clsDal.GetDataTable("ps_sp_Update_PrevYrBal", ht);
                ++num1;
                control4.BackColor = Color.LightBlue;
            }
            else
            {
                Decimal num3 = new Decimal(0);
                string empty2 = string.Empty;
                Decimal num4;
                try
                {
                    num4 = Convert.ToDecimal(((TextBox)row.FindControl("txtPrevAmount")).Text.ToString().Trim());
                    empty2 = Convert.ToString(((Label)row.FindControl("lblAdmnNo")).Text.ToString().Trim());
                }
                catch (Exception ex)
                {
                    num4 = new Decimal(0);
                }
                Hashtable ht = new Hashtable();
                DateTime dateTime = Convert.ToDateTime("MAR-31-" + Convert.ToInt32(drpSession.SelectedValue.ToString().Trim().Substring(0, 4)).ToString().Trim());
                ht.Add("TransDate", dateTime);
                ht.Add("AdmnNo", empty2);
                ht.Add("TransDesc", "Previous Balance");
                ht.Add("debit", num4);
                ht.Add("credit", 0);
                ht.Add("balance", num4);
                ht.Add("Receipt_VrNo", "");
                ht.Add("userid", Convert.ToInt32(Session["User_Id"].ToString().Trim()));
                ht.Add("schoolid", Session["SchoolId"].ToString());
                clsDal.GetDataTable("ps_sp_insert_PrevYrBal", ht);
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

    protected void drpSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
       grdStudPrevAC.Visible = false;
       lblRecCount.Text = string.Empty;
    }

    protected void btnSetActHead_Click(object sender, EventArgs e)
    {
       Response.Redirect("SetAccountHead.aspx");
    }

    protected void grdStudPrevAC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        TextBox control1 = (TextBox)e.Row.FindControl("txtPrevAmount");
        TextBox control2 = (TextBox)e.Row.FindControl("txtPrevAmount2");
        Label control3 = (Label)e.Row.FindControl("Label1");
        if (control1 == null || control2 == null || control3 == null)
            return;
        double num = Convert.ToDouble(control1.Text) - Convert.ToDouble(control2.Text);
        control3.Text = num.ToString();
        string str = string.Format("textBoxOnBlur('{0}', '{1}', '{2}');", control1.ClientID, control2.ClientID, control3.ClientID);
        control1.Attributes.Add("onblur", str);
    }
}