using ASP;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Admissions_AllotRollNo : System.Web.UI.Page
{
    public static int grdrowcount;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        Page.Form.DefaultButton = btnShow.UniqueID;
        if (Page.IsPostBack)
            return;
        btnUpdtRoll.Enabled = false;
        btnAllotRoll.Enabled = false;
        filldropdowns();
        lblRecCount.Text = "";
    }

    private void filldropdowns()
    {
        FillClass();
       // FillSectionDropDown();
        FillSession();
    }

    private void FillClass()
    {
        drpClass.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassForAdmnDDL");
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "classid";
        drpClass.DataBind();
        drpClass.Items.RemoveAt(0);
        drpClass.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    private void FillSession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select distinct SessionID,SessionYr from dbo.PS_FeeNormsNew order by sessionID DESC");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillSectionDropDown()
    {
       // int int16 = (int)Convert.ToInt16(ConfigurationManager.AppSettings["TotSec"].ToString());
        int int16 =(int)Convert.ToInt16( new clsDAL().ExecuteScalarQry("Select NoOfSections from PS_ClassMaster where ClassId=" + drpClass.SelectedValue.ToString().Trim() + ""));
        int num1 = int16;
        if (int16 <= 0)
            return;
        drpSection.Items.Clear();
        int num2 = 65;
        int index = 0;
        while (num1 > 0)
        {
            drpSection.Items.Insert(index, new ListItem(Convert.ToChar(num2).ToString(), Convert.ToChar(num2).ToString()));
            --num1;
            ++index;
            ++num2;
        }
        drpSection.Items.Insert(0, new ListItem("--SELECT--", "0"));
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
        drpSelectStudent.DataSource = clsDal.GetDataTable("ps_StudForRollAlert", hashtable);
        drpSelectStudent.DataTextField = "fullname";
        drpSelectStudent.DataValueField = "admnno";
        drpSelectStudent.DataBind();
        drpSelectStudent.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        grdstudents.Visible = true;
        FillGrid();
    }

    private void FillGrid()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@Session", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@ClassID", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@AdmnNo", drpSelectStudent.SelectedValue);
        hashtable.Add("@OrderBy", drpOrder.SelectedValue.Trim());
        DataTable dataTable = clsDal.GetDataTable("ps_sp_getStudAllotRoll", hashtable);
        Admissions_AllotRollNo.grdrowcount = grdstudents.Rows.Count;
        if (dataTable.Rows.Count > 0)
        {
            btnAllotRoll.Enabled = true;
            btnUpdtRoll.Enabled = true;
            grdstudents.DataSource = dataTable;
            grdstudents.DataBind();
            grdstudents.Visible = true;
            lblRecCount.Text = "Total Number of Student: " + dataTable.Rows.Count.ToString();
        }
        else
        {
            btnAllotRoll.Enabled = false;
            btnUpdtRoll.Enabled = false;
            grdstudents.Visible = false;
            lblRecCount.Text = "";
            lblMsg.Text = "No Records Found !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private string StudentDetail()
    {
        string str = "select distinct s.AdmnNo as Admissionno,s.OldAdmnNo,FullName , convert(varchar(10),s.dob,103) as dateofbirth , c.classname , cs.Section , s.TelNoResidence , Convert(varchar(10),s.AdmnDate ,103) as AdmnDate, FatherName, MotherName,cs.RollNo  from PS_StudMaster s join PS_ClasswiseStudent cs on s.admnno = cs.admnno  join PS_ClassMaster c on cs.classid = c.classid  " + " where 1=1 and StatusId=1 ";
        if (drpSession.Items.Count > 0)
            str = str + " and cs.sessionyear ='" + drpSession.SelectedValue + "'";
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            str = str + " and cs.classid =" + drpClass.SelectedValue;
        if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
            str = str + " and cs.admnno =" + drpSelectStudent.SelectedValue;
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            str = str + " and cs.section ='" + drpSection.SelectedValue.ToString().Trim() + "'";
        if (drpClass.SelectedValue.ToString().Trim() == "0")
            str += " order by  cs.RollNo";
        return str;
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSectionDropDown();
        grdstudents.Visible = false;
        lblRecCount.Text = string.Empty;
        drpSection.SelectedIndex = 0;
        drpSelectStudent.Items.Clear();
        btnAllotRoll.Enabled = false;
        btnUpdtRoll.Enabled = false;
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdstudents.Visible = false;
        lblRecCount.Text = string.Empty;
        btnAllotRoll.Enabled = false;
        btnUpdtRoll.Enabled = false;
        if (drpSection.SelectedIndex > 0)
            FillSelectStudent();
        else
            drpSelectStudent.Items.Clear();
    }

    protected void drpSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdstudents.Visible = false;
        lblRecCount.Text = string.Empty;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClass();
        drpSelectStudent.Items.Clear();
        lblRecCount.Text = string.Empty;
        drpSection.SelectedIndex = -1;
        grdstudents.Visible = false;
        btnAllotRoll.Enabled = false;
        btnUpdtRoll.Enabled = false;
    }

    protected void btnUpdtRoll_Click(object sender, EventArgs e)
    {
        string str = string.Empty;
        int num = 1;
        foreach (GridViewRow row in grdstudents.Rows)
        {
            clsDAL clsDal = new clsDAL();
            Hashtable hashtable = new Hashtable();
            DataTable dataTable1 = new DataTable();
            HtmlInputCheckBox control1 = (HtmlInputCheckBox)row.FindControl("Checkb");
            TextBox control2 = (TextBox)row.FindControl("txtRoll");
            HiddenField control3 = (HiddenField)row.FindControl("hfAdNo");
            hashtable.Add("@RollNo", num);
            hashtable.Add("@admnno", Convert.ToInt64(control3.Value));
            hashtable.Add("@Section", Convert.ToChar(drpSection.SelectedValue.ToString()));
            DataTable dataTable2 = clsDal.GetDataTable("ps_sp_UpdtRoll", hashtable);
            if (dataTable2.Rows.Count > 0)
                str = str + dataTable2.Rows[0][0].ToString() + ',';
            ++num;
        }
        FillGrid();
        if (str.Trim().Equals(string.Empty))
        {
            lblMsg.Text = "Roll number alloted successfully !";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = "Roll number alloted failed !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnAllotRoll_Click(object sender, EventArgs e)
    {
        string str = string.Empty;
        if (getValidInput())
        {
            foreach (GridViewRow row in grdstudents.Rows)
            {
                clsDAL clsDal = new clsDAL();
                Hashtable hashtable = new Hashtable();
                DataTable dataTable1 = new DataTable();
                HtmlInputCheckBox control1 = (HtmlInputCheckBox)row.FindControl("Checkb");
                TextBox control2 = (TextBox)row.FindControl("txtRoll");
                HiddenField control3 = (HiddenField)row.FindControl("hfAdNo");
                if (control2.Text != "0")
                {
                    hashtable.Add("@RollNo", Convert.ToInt64(control2.Text.ToString()));
                    hashtable.Add("@admnno", Convert.ToInt64(control3.Value));
                    hashtable.Add("@Section", Convert.ToChar(drpSection.SelectedValue.ToString()));
                    DataTable dataTable2 = clsDal.GetDataTable("ps_sp_UpdtRoll", hashtable);
                    if (dataTable2.Rows.Count > 0)
                        str = str + dataTable2.Rows[0][0].ToString() + ',';
                }
            }
            FillGrid();
            if (str.Trim().Equals(string.Empty))
            {
                lblMsg.Text = "Roll number alloted successfully !";
                lblMsg.ForeColor = Color.Green;
            }
            else
            {
                lblMsg.Text = "Roll number alloted failed !";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
        {
            lblMsg.Text = "Duplicate Roll number already exist in Grid ";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private bool getValidInput()
    {
        string str = string.Empty;
        string empty = string.Empty;
        foreach (Control row in grdstudents.Rows)
            ((WebControl)row.FindControl("txtRoll")).BackColor = Color.Transparent;
        foreach (GridViewRow row in grdstudents.Rows)
        {
            TextBox control1 = (TextBox)row.FindControl("txtRoll");
            if (control1.Text.Trim() != "0")
            {
                for (int index = row.RowIndex + 1; index < grdstudents.Rows.Count; ++index)
                {
                    TextBox control2 = (TextBox)grdstudents.Rows[index].FindControl("txtRoll");
                    if (control2.Text != "0" || control2.BackColor != Color.LightBlue || control1.BackColor != Color.LightBlue)
                    {
                        if (control2.Text.Trim() == control1.Text.Trim())
                        {
                            control1.BackColor = Color.LightBlue;
                            control2.BackColor = Color.LightBlue;
                            str = control1.Text;
                        }
                    }
                    else
                        control1.BackColor = Color.Transparent;
                }
            }
        }
        return str == "";
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //    e.Row.Attributes.Add("onclick", "javascript:ChangeRowColor('" + e.Row.ClientID + "')");

        if (e.Row.RowType == DataControlRowType.DataRow &&  (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] =
              string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            //e.Row.Attributes.Add("onclick", "javascript:SelectRow('" + e.Row.ClientID + "',"+e.Row.RowIndex+");");
             e.Row.Attributes.Add("onkeydown","javascript:return SelectSibling(event);");
             e.Row.Attributes.Add("onselectstart", "javascript:return false;");

            //e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event);";
            //e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    }
}