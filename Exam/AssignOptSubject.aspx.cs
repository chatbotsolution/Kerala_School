using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Exam_AssignOptSubject : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        objStatic.FillSessionYr(drpSession);
        drpSession.Items.RemoveAt(0);
        drpSession.Items.Insert(0, new ListItem("- SELECT -", "0"));
        //objStatic.FillSection(drpSection);
        //drpSection.Items.RemoveAt(0);
        drpSection.Items.Insert(0, new ListItem("- ALL -", "0"));
        BindDropDown(drpClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        trMsg.BgColor = (string)null;
        drpSection.SelectedIndex = 0;
        lblRecords.Text = string.Empty;
        FillGrid();
        drpSession.Focus();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        trMsg.BgColor = (string)null;
        drpSection.SelectedIndex = 0;
        lblRecords.Text = string.Empty;
        if (drpClass.SelectedIndex > 0)
        {
            BindDropDown(drpOptSub, "select SubjectName,SubjectId from dbo.PS_SubjectMaster where ClassId=" + drpClass.SelectedValue + " and IsOptional=1", "SubjectName", "SubjectId");
            if (drpOptSub.Items.Count > 1)
            {
                FillGrid();
            }
            else
            {
                grdStudent.DataSource = null;
                grdStudent.DataBind();
                drpOptSub.Items.Clear();
                btnAssign.Visible = false;
                btnUnAssign.Visible = false;
            }
        }
        drpClass.Focus();
        FillSectionDropDown();
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        trMsg.BgColor = (string)null;
        FillGrid();
        drpSection.Focus();
    }

    private void FillGrid()
    {
        lblMsg.Text = string.Empty;
        trMsg.BgColor = (string)null;
        grdStudent.DataSource = null;
        grdStudent.DataBind();
        btnAssign.Visible = false;
        btnUnAssign.Visible = false;
        if (drpOptSub.SelectedIndex <= 0)
            return;
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
        hashtable.Add("@ClassId", drpClass.SelectedValue.ToString().Trim());
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue.ToString().Trim());
        hashtable.Add("@OptSubId", drpOptSub.SelectedValue);
        DataTable dataTable2 = obj.GetDataTable("ExamOptSubStudList", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            btnAssign.Visible = true;
            btnUnAssign.Visible = true;
        }
        grdStudent.DataSource = dataTable2;
        grdStudent.DataBind();
        lblRecords.Text = "No Of Records : " + dataTable2.Rows.Count.ToString();
    }

    protected void drpOptSub_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        drpOptSub.Focus();
    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        trMsg.BgColor = (string)null;
        string str1 = string.Empty;
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select any Student');", true);
            grdStudent.Focus();
        }
        else
        {
            string str2 = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj1 in str2.Split(chArray))
            {
                hashtable.Clear();
                hashtable.Add("@AdmnNo", Convert.ToInt32(obj1.ToString()));
                hashtable.Add("@SessionYr", drpSession.SelectedValue);
                hashtable.Add("@ClassId", drpClass.SelectedValue);
                hashtable.Add("@OptSubId", drpOptSub.SelectedValue);
                hashtable.Add("@Action", "A");
                hashtable.Add("@UserId", Session["User_Id"].ToString());
                
                str1 = obj.ExecuteScalar("ExamAssignStudOptSub", hashtable);
                if (str1.Trim().ToUpper() != "S")
                    break;
            }
            if (str1.Trim().ToUpper() == "S")
            {
                FillGrid();
                lblMsg.Text = "Data Saved Successfully";
                trMsg.BgColor = "Green";
            }
            else
            {
                lblMsg.Text = str1;
                trMsg.BgColor = "Red";
            }
        }
    }

    protected void btnUnAssign_Click(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        trMsg.BgColor = (string)null;
        string str1 = string.Empty;
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select any Student');", true);
            grdStudent.Focus();
        }
        else
        {
            string str2 = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj1 in str2.Split(chArray))
            {
                hashtable.Clear();
                hashtable.Add("@AdmnNo", Convert.ToInt32(obj1.ToString()));
                hashtable.Add("@SessionYr", drpSession.SelectedValue);
                hashtable.Add("@ClassId", drpClass.SelectedValue);
                hashtable.Add("@OptSubId", drpOptSub.SelectedValue);
                hashtable.Add("@Action", "U");
                hashtable.Add("@UserId", Session["User_Id"].ToString());
                str1 = obj.ExecuteScalar("ExamAssignStudOptSub", hashtable);
                if (str1.Trim().ToUpper() != "S")
                    break;
            }
            if (str1.Trim().ToUpper() == "S")
            {
                FillGrid();
                lblMsg.Text = "Data Saved Successfully";
                trMsg.BgColor = "Green";
            }
            else
            {
                lblMsg.Text = str1;
                trMsg.BgColor = "Red";
            }
        }
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
        drpSection.Items.Insert(0, new ListItem("- ALL -", "0"));
    }
}