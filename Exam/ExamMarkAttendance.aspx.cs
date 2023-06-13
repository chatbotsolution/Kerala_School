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

public partial class Exam_ExamMarkAttendance : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    private int RecCount;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (!Page.IsPostBack)
        {
            Session["title"] = Page.Title.ToString();
            BindSession();
            //objStatic.FillSection(drpSection);
            //drpSection.Items.RemoveAt(0);
            drpSection.Items.Insert(0, new ListItem("- ALL -", "0"));
            BindDropDown(drpClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
        }
        lblMsg.Text = string.Empty;
        trMsg.BgColor = "Transparent";
    }

    private void BindSession()
    {
        ViewState["Session"] = obj.ExecuteScalarQry("select top 1 SessionYr from dbo.ExamDetails order by UserDt desc");
        DataTable dataTableQry = obj.GetDataTableQry("SELECT DISTINCT SessionYr FROM ExamDetails ORDER BY SessionYr DESC");
        if (dataTableQry.Rows.Count > 0)
        {
            drpSession.DataSource = dataTableQry;
            drpSession.DataValueField = "SessionYr";
            drpSession.DataTextField = "SessionYr";
            drpSession.DataBind();
            drpSession.Items.Insert(0, new ListItem("- Select -", "0"));
            drpClass.Enabled = true;
            drpSection.Enabled = true;
            drpExamName.Enabled = true;
            drpSubject.Enabled = true;
        }
        else
        {
            drpSession.Items.Insert(0, new ListItem("- No Exams Defined -", "0"));
            drpClass.Enabled = false;
            drpSection.Enabled = false;
            drpExamName.Enabled = false;
            drpSubject.Enabled = false;
        }
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        trMsg.BgColor = (string)null;
        drpClass.SelectedIndex = 0;
        drpSection.SelectedIndex = 0;
        drpExamName.Items.Clear();
        drpSubject.Items.Clear();
        gvExamAtten.DataSource = null;
        gvExamAtten.DataBind();
        lblRecCount.Text = string.Empty;
        lblNote.Visible = false;
        btnSubmit.Enabled = false;
        btnAbsent.Enabled = false;
        drpSession.Focus();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        trMsg.BgColor = (string)null;
        drpSection.SelectedIndex = 0;
        drpExamName.Items.Clear();
        drpSubject.Items.Clear();
        gvExamAtten.DataSource = null;
        gvExamAtten.DataBind();
        lblRecCount.Text = string.Empty;
        lblNote.Visible = false;
        btnSubmit.Enabled = false;
        btnAbsent.Enabled = false;
        DataTable dataTable = obj.GetDataTable("ExamGetExDtlsSession", new Hashtable()
    {
      {
         "@ClassID",
         drpClass.SelectedValue
      },
      {
         "@SessionYr",
         drpSession.SelectedValue
      }
    });
        drpExamName.DataSource = dataTable;
        if (dataTable.Rows.Count > 0)
        {
            drpExamName.DataTextField = "ExamName";
            drpExamName.DataValueField = "ExamClassId";
            drpExamName.DataBind();
            drpExamName.Items.Insert(0, new ListItem("- Select -", "0"));
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

    protected void drpExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        trMsg.BgColor = (string)null;
        drpSubject.Items.Clear();
        BindExamSub();
        drpExamName.Focus();
    }

    protected void drpSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        trMsg.BgColor = (string)null;
        FillGrid();
        drpSubject.Focus();
    }

    private void BindExamSub()
    {
        gvExamAtten.DataSource = null;
        gvExamAtten.DataBind();
        lblRecCount.Text = string.Empty;
        lblNote.Visible = false;
        btnSubmit.Enabled = false;
        btnAbsent.Enabled = false;
        if (drpExamName.SelectedIndex <= 0)
            return;
        obj = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("ExamGetExamwiseSub", new Hashtable()
    {
      {
         "@ExamId",
         drpExamName.SelectedValue
      }
    });
        if (dataTable2.Rows.Count <= 0)
            return;
        drpSubject.DataSource = dataTable2;
        drpSubject.DataValueField = "ExamSubId";
        drpSubject.DataTextField = "SubjectName";
        drpSubject.DataBind();
        drpSubject.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    private void FillGrid()
    {
        gvExamAtten.DataSource = null;
        gvExamAtten.DataBind();
        lblRecCount.Text = string.Empty;
        lblNote.Visible = false;
        btnSubmit.Enabled = false;
        btnAbsent.Enabled = false;
        if (drpExamName.SelectedIndex <= 0 || drpSubject.SelectedIndex <= 0)
            return;
        string empty = string.Empty;
        Hashtable hashtable = new Hashtable();
        DataSet dataSet1 = new DataSet();
        DataTable dataTable = new DataTable();
        obj = new clsDAL();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
        hashtable.Add("@Class", drpClass.SelectedValue.ToString().Trim());
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue.ToString().Trim());
        hashtable.Add("@ExamId", drpExamName.SelectedValue.ToString().Trim());
        hashtable.Add("@ExamSubId", drpSubject.SelectedValue);
        DataSet dataSet2 = obj.GetDataSet("ExamGetStudentList", hashtable);
        DataTable table = dataSet2.Tables[0];
        string str = dataSet2.Tables[1].Rows[0][0].ToString();
        gvExamAtten.DataSource = table;
        gvExamAtten.DataBind();
        lblRecCount.Text = "No Of Records : " + table.Rows.Count.ToString();
        if (table.Rows.Count <= 0)
            return;
        if (str.Trim() == "0")
        {
            lblNote.Text = "Note : Select the Records to Mark the Attendance";
            gvExamAtten.Columns[0].Visible = true;
            btnSubmit.Enabled = true;
            btnAbsent.Enabled = true;
        }
        else
        {
            lblNote.Text = "Note : Marks already Entered for this Exam. Attendance can not be modified.";
            gvExamAtten.Columns[0].Visible = false;
        }
        lblNote.Visible = true;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select any Record');", true);
            gvExamAtten.Focus();
        }
        else
        {
            string str = string.Empty;
            string[] strArray = Request["Checkb"].Split(',');
            for (int index = 0; index < strArray.Length; ++index)
            {
                Label control1 = (Label)gvExamAtten.Rows[Convert.ToInt32(strArray[index])].FindControl("lblAdmn");
                HiddenField control2 = (HiddenField)gvExamAtten.Rows[Convert.ToInt32(strArray[index])].FindControl("hfAttenID");
                if (control2.Value == "0")
                {
                    hashtable.Clear();
                    hashtable.Add("@ExamId", drpExamName.SelectedValue.ToString());
                    hashtable.Add("@AdmnNo", control1.Text.Trim());
                    hashtable.Add("@ExStatus", 1);
                    hashtable.Add("@UserId", Session["User_Id"].ToString());
                    hashtable.Add("@SessionYr", drpSession.SelectedValue);
                    hashtable.Add("@ExamSubjectId", drpSubject.SelectedValue);
                    str = obj.ExecuteScalar("ExamInsUpdtAttendance", hashtable);
                    if (str.Trim() != string.Empty)
                        break;
                }
                else
                {
                    hashtable.Clear();
                    hashtable.Add("@ExamMarksId", control2.Value);
                    hashtable.Add("@ExStatus", 1);
                    hashtable.Add("@UserId", Session["User_Id"].ToString());
                    hashtable.Add("@ExamId", drpExamName.SelectedValue.ToString());
                    hashtable.Add("@AdmnNo", control1.Text.Trim());
                    hashtable.Add("@SessionYr", drpSession.SelectedValue);
                    hashtable.Add("@ExamSubjectId", drpSubject.SelectedValue);
                    str = obj.ExecuteScalar("ExamInsUpdtAttendance", hashtable);
                    if (str.Trim() != string.Empty)
                        break;
                }
            }
            if (str.Trim() == string.Empty)
            {
                lblMsg.Text = "Attendance Marked Successfully";
                trMsg.BgColor = "Green";
            }
            else
            {
                lblMsg.Text = "Failed to Mark Attendance";
                trMsg.BgColor = "Red";
            }
            FillGrid();
        }
    }

    protected void btnAbsent_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string empty = string.Empty;
            string[] strArray = Request["Checkb"].Split(',');
            for (int index = 0; index < strArray.Length; ++index)
            {
                Label control1 = (Label)gvExamAtten.Rows[Convert.ToInt32(strArray[index])].FindControl("lblAdmn");
                HiddenField control2 = (HiddenField)gvExamAtten.Rows[Convert.ToInt32(strArray[index])].FindControl("hfAttenID");
                if (control2.Value == "0")
                {
                    hashtable.Clear();
                    hashtable.Add("@ExamId", drpExamName.SelectedValue.ToString());
                    hashtable.Add("@AdmnNo", control1.Text.Trim());
                    hashtable.Add("@ExStatus", 0);
                    hashtable.Add("@UserId", Session["User_Id"].ToString());
                    hashtable.Add("@SessionYr", drpSession.SelectedValue);
                    hashtable.Add("@ExamSubjectId", drpSubject.SelectedValue);
                    if (obj.ExecuteScalar("ExamInsUpdtAttendance", hashtable).Trim() != string.Empty)
                        break;
                }
                else
                {
                    hashtable.Clear();
                    hashtable.Add("@ExamMarksId", control2.Value);
                    hashtable.Add("@ExStatus", 0);
                    hashtable.Add("@UserId", Session["User_Id"].ToString());
                    hashtable.Add("@ExamId", drpExamName.SelectedValue.ToString());
                    hashtable.Add("@AdmnNo", control1.Text.Trim());
                    hashtable.Add("@SessionYr", drpSession.SelectedValue);
                    hashtable.Add("@ExamSubjectId", drpSubject.SelectedValue);
                    if (obj.ExecuteScalar("ExamInsUpdtAttendance", hashtable).Trim() != string.Empty)
                        break;
                }
            }
            FillGrid();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        BindSession();
        drpClass.SelectedIndex = 0;
        drpSection.SelectedIndex = 0;
        drpExamName.Items.Clear();
        drpSubject.Items.Clear();
        gvExamAtten.DataSource = null;
        gvExamAtten.DataBind();
        lblRecCount.Text = string.Empty;
        lblNote.Visible = false;
        btnSubmit.Enabled = false;
        btnAbsent.Enabled = false;
        drpSession.Focus();
        lblMsg.Text = string.Empty;
        trMsg.BgColor = (string)null;
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