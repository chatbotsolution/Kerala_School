using ASP;
using Classes.DA;
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
using System.Globalization;

public partial class Admissions_AllotSixDateTime : System.Web.UI.Page
{
    public static int grdrowcount;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        Page.Form.DefaultButton = btnShow.UniqueID;
        btnUpdtSec.Enabled = false;
        if (Page.IsPostBack)
            return;
        filldropdowns();
        lblRecCount.Text = "";
    }
    private void filldropdowns()
    {
        FillSession();
        FillClass();
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
        int int16 = (int)Convert.ToInt16(new clsDAL().ExecuteScalarQry("Select NoOfSections from PS_ClassMaster where ClassId=" + drpClass.SelectedValue.ToString().Trim() + ""));
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
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@SessionYear", drpSession.SelectedValue);
        if (drpClass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpClass.SelectedValue);
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue);
        drpSelectStudent.DataSource = common.GetDataTable("ps_StudSectionAlert", hashtable);
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
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable = common.ExecuteSql(StudentDetail());
        Admissions_AllotSixDateTime.grdrowcount = grdstudents.Rows.Count;
        if (dataTable.Rows.Count > 0)
        {

            btnUpdtSec.Enabled = true;
            grdstudents.DataSource = dataTable;
            grdstudents.DataBind();
            grdstudents.Visible = true;
            lblRecCount.Text = "Total Number of Student: " + dataTable.Rows.Count.ToString();
            UpdatePanel1.Update();
             BingGridSection();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                //((DropDownList)grdstudents.Rows[i].FindControl("drpSixthSub")).SelectedValue = dataTable.Rows[i]["Sixthoptional"].ToString();
               // ((TextBox)grdstudents.Rows[i].FindControl("txtDt")).Text = Convert.ToDateTime(dataTable.Rows[i]["AdmnDate"]).ToString("dd/MM/yyyy");
            }
        }
        else
        {
            btnUpdtSec.Enabled = false;
            grdstudents.Visible = false;
            lblRecCount.Text = "";
            lblMsg.Text = "No Record Found !";
            UpdatePanel1.Update();
        }
    }
    private string StudentDetail()
    {
        string str = "select distinct s.AdmnNo as Admissionno,  s.AdmnDate as AdmnDate ,s.OldAdmnNo,FullName, convert(varchar(10),s.dob,103) as dateofbirth , c.classname , cs.Section , s.TelNoResidence   from PS_StudMaster s join PS_ClasswiseStudent cs on s.admnno = cs.admnno  join PS_ClassMaster c on cs.classid = c.classid  " + " where 1=1 and StatusId=1 and (admnDate is null or admnDate='1900-01-01 00:00:00.000')";
        if (drpSession.Items.Count > 0)
            str = str + " and cs.sessionyear ='" + drpSession.SelectedValue + "'";
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            str = str + " and cs.classid =" + drpClass.SelectedValue;
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            str = str + " and cs.Section ='" + drpSection.SelectedValue + "'";
        if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
            str = str + " and cs.admnno =" + drpSelectStudent.SelectedValue;
        return str + "ORDER BY FullName";
        // return str;
    }
    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnUpdtSec.Enabled = false;
        grdstudents.Visible = false;
        lblRecCount.Text = "";
        FillSectionDropDown();
        FillSelectStudent();
    }
    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnUpdtSec.Enabled = false;
        grdstudents.Visible = false;
        lblRecCount.Text = "";
        // FillSectionDropDown();
        FillSelectStudent();
    }
    protected void drpSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdstudents.Visible = false;
        lblRecCount.Text = string.Empty;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FillClass();
        FillSelectStudent();
        btnUpdtSec.Enabled = false;
        lblRecCount.Text = string.Empty;
        grdstudents.Visible = false;
    }

    private void BingGridSection()
    {
        try
        {
            foreach (GridViewRow row in grdstudents.Rows)
            {
                ((HtmlInputCheckBox)row.FindControl("Checkb")).Checked = true;
                TextBox controlDt = (TextBox)row.FindControl("txtDt");
                DataTable dt = new clsDAL().GetDataTableQry("Select AdmnNo, AdmnDate   from PS_StudMaster where Class=" + drpClass.SelectedValue.ToString().Trim() + " ");
                if (dt.Rows.Count > 0)
                {
                    controlDt.Text = Convert.ToDateTime(dt.Rows[0]["AdmnDate"]).ToString("dd/MM/yyyy");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnUpdtSixSub_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in grdstudents.Rows)
            {
                clsDAL clsDal = new clsDAL();
                Hashtable hashtable = new Hashtable();  
                DataTable dataTable = new DataTable();
                HtmlInputCheckBox control1 = (HtmlInputCheckBox)row.FindControl("Checkb");
                // DropDownList control2 = (DropDownList)row.FindControl("drpSixthSub");
                TextBox controlDt = (TextBox)row.FindControl("txtDt");
                if (control1.Checked)
                {
                    HiddenField control3 = (HiddenField)row.FindControl("hfAdNo");
                    //if (control2.SelectedValue.ToString() == "83" || control2.SelectedValue.ToString() == "84")
                    //if (control2.SelectedValue.ToString() == "182" || control2.SelectedValue.ToString() == "183")//---changed Id for school
                    //{
                    //    hashtable.Add("@Flag", 1);
                    //}
                    //hashtable.Add("@AdmnDateTime", controlDt.Text!=""?Convert.ToDateTime(controlDt.Text):DateTime.MinValue);
                    //DateTime dtUp = DateTime.ParseExact(controlDt.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    //hashtable.Add("@AdmnDateTime", dtUp);
                   // hashtable.Add("@AdmnDateTime",Convert.ToDateTime(controlDt.Text));
                    hashtable.Add("@AdmnDateTime", controlDt.Text.Split('-')[1]+"/"+controlDt.Text.Split('-')[0]+"/"+controlDt.Text.Split('-')[2]);
                    hashtable.Add("@admnno", Convert.ToInt64(control3.Value));
                    hashtable.Add("@Session", drpSession.SelectedValue.Trim());
                    hashtable.Add("@Class", drpClass.SelectedValue.Trim());
                    clsDal.GetDataTable("ps_sp_UpdateAdmissionDate", hashtable);
                    lblMsg.Text = "Admission Date alloted successfully !";
                    lblMsg.ForeColor = Color.Green;
                    grdstudents.Visible = true;
                    lblRecCount.Text = string.Empty;
                    drpSelectStudent.SelectedIndex = 0;
                }
            }
            FillGrid();
        }
        catch(Exception ex)
        {
        }
    }
}