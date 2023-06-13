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

public partial class Exam_GradesubMarkEntry : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            BindSession();
            bindDrp(drpClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
            objStatic.FillSection(drpSection);
            drpSection.Items.RemoveAt(0);
            drpSection.Items.Insert(0, new ListItem("- ALL -", "0"));
        }
        else
            Response.Redirect("~/Login.aspx");
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
            drpExam.Enabled = true;
            drpSubject.Enabled = true;
            drpStudent.Enabled = true;
        }
        else
        {
            drpSession.Items.Insert(0, new ListItem("- No Exams Defined -", "0"));
            drpClass.Enabled = false;
            drpSection.Enabled = false;
            drpExam.Enabled = false;
            drpSubject.Enabled = false;
            drpStudent.Enabled = false;
        }
    }

    private void bindDrp(DropDownList drp, string query, string textField, string valueField)
    {
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textField;
        drp.DataValueField = valueField;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpClass.SelectedIndex = 0;
        drpSection.SelectedIndex = 0;
        drpExam.Items.Clear();
        drpStudent.Items.Clear();
        drpSubject.Items.Clear();
        drpSession.Focus();
        ClearGrids();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpSection.SelectedIndex = 0;
        drpExam.Items.Clear();
        drpStudent.Items.Clear();
        drpSubject.Items.Clear();
        ClearGrids();
        if (drpSession.SelectedIndex > 0)
        {
            drpExam.DataSource = obj.GetDataTable("ExamGetExDtlsSession", new Hashtable()
      {
        {
           "@SessionYr",
           drpSession.SelectedValue
        },
        {
           "@ClassID",
           drpClass.SelectedValue
        }
      });
            drpExam.DataTextField = "ExamName";
            drpExam.DataValueField = "ExamClassId";
            drpExam.DataBind();
            drpExam.Items.Insert(0, new ListItem("- Select -", "0"));
            drpClass.Focus();
        }
        else
        {
            drpClass.SelectedIndex = 0;
            ScriptManager.RegisterClientScriptBlock((Control)drpClass, drpClass.GetType(), "ShowMessage", "alert('Select a Session');", true);
            drpSession.Focus();
        }
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearGrids();
        if (drpSession.SelectedIndex > 0 && drpClass.SelectedIndex > 0 && (drpExam.SelectedIndex > 0 && rbType.Enabled))
        {
            if (rbType.SelectedIndex == 0)
                BindStudent();
            else
                Subjectwise();
        }
        else
        {
            drpStudent.Items.Clear();
            drpSubject.Items.Clear();
        }
        drpSection.Focus();
    }

    protected void drpExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearGrids();
        if (drpExam.SelectedIndex > 0)
            rbType.Enabled = true;
        else
            rbType.Enabled = false;
        if (drpSession.SelectedIndex > 0 && drpClass.SelectedIndex > 0 && rbType.Enabled)
        {
            if (rbType.SelectedIndex == 0)
                BindStudent();
            else
                BindSubjects();
        }
        else
        {
            drpStudent.Items.Clear();
            drpSubject.Items.Clear();
        }
        drpExam.Focus();
    }

    private void BindStudent()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@Modify", "False");
        hashtable.Add("@SessionYr", drpSession.SelectedValue);
        hashtable.Add("@ClassId", drpClass.SelectedValue);
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue);
        hashtable.Add("@ExamId", drpExam.SelectedValue);
        DataTable dataTable = obj.GetDataTable("GradeGetStudentName", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            drpStudent.DataSource = dataTable.DefaultView;
            drpStudent.DataValueField = "AdmnNo";
            drpStudent.DataTextField = "StudentName";
            drpStudent.DataBind();
            drpStudent.Items.Insert(0, new ListItem("- Select -", "0"));
        }
        else
            drpStudent.Items.Clear();
        lblRecords.Text = "No of Records : " + dataTable.Rows.Count;
    }

    private void BindSubjects()
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@ClassId", drpClass.SelectedValue);
        DataTable dataTable2 = obj.GetDataTable("ExamGetGradeSub", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            drpSubject.DataSource = dataTable2;
            drpSubject.DataTextField = "SubjectName";
            drpSubject.DataValueField = "SubjectId";
            drpSubject.DataBind();
            drpSubject.Items.Insert(0, new ListItem("- Select -", "0"));
        }
        else
            drpSubject.Items.Clear();
    }

    protected void rbType_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearGrids();
        if (rbType.SelectedIndex == 0)
        {
            drpSubject.Items.Clear();
            BindStudent();
           
        }
        else
        {
            drpStudent.Items.Clear();
            BindSubjects();
        }
        rbType.Focus();
    }

    protected void drpStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearGrids();
        Studentwise();
        drpStudent.Focus();
    }

    private void Studentwise()
    {
        if (drpStudent.SelectedIndex <= 0)
            return;
        grdStudentwise.DataSource = obj.GetDataTable("GradeSubListForGrid", new Hashtable()
    {
      {
         "@admnno",
         drpStudent.SelectedValue
      },
               {
         "@ClassId",
         drpClass.SelectedValue
      },
                           {
         "@ExamId",
         drpExam.SelectedValue
      }
    });
        grdStudentwise.DataBind();
    }

    protected void drpSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearGrids();
        Subjectwise();
        drpSubject.Focus();
    }

    private void Subjectwise()
    {
        if (drpSubject.SelectedIndex <= 0)
            return;
        Hashtable hashtable = new Hashtable();
       
        hashtable.Add("@SessionYr", drpSession.SelectedValue);
        hashtable.Add("@ClassId", drpClass.SelectedValue);
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue);
        hashtable.Add("@ExamId", drpExam.SelectedValue);
        hashtable.Add("@SubjectId", drpSubject.SelectedValue);

        DataTable dataTable = obj.GetDataTable("GradeStudentListForGrid", hashtable);
        grdSubjectwise.DataSource = dataTable;
        grdSubjectwise.DataBind();
        lblRecords.Text = "No of Records : " + dataTable.Rows.Count;
    }

    private void ClearGrids()
    {
        lblRecords.Text = string.Empty;
        grdStudentwise.DataSource = null;
        grdStudentwise.DataBind();   
        grdSubjectwise.DataSource = null;
        grdSubjectwise.DataBind();
        lblMsg.Text = string.Empty;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Button control1 = (Button)((Control)sender).Parent.Parent.FindControl("btnSubmit");
            HiddenField subid = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfExamSubId");
            HiddenField admno = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfadmnno");
            HiddenField grdmrkid = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfGradeMrkId");
            TextBox grade = (TextBox)((Control)sender).Parent.Parent.FindControl("txtgrade");
            
            if (grade.Text==""||grade.Text==null)
            {
                control1.Attributes.Add("onclick", "javascript:return checkObtMarks('" + grade.ClientID + "');");
            }
            else
            {
                
            if (InsertExamMarks(Convert.ToInt64(admno.Value), Convert.ToInt32(subid.Value), Convert.ToString(grade.Text),Convert.ToInt64(grdmrkid.Value)).Trim().ToUpper() == "S")
            {
                ClearGrids();
                Studentwise();
                lblMsg.Text = admno.Value + " Marks Submitted Successfully for " + drpStudent.SelectedItem.Text;
                lblMsg.ForeColor = Color.Green;
                if (grdStudentwise.Rows.Count == 0)
                {
                    BindStudent();
                    drpStudent.Focus();
                }
                else
                    grdStudentwise.Focus();
            }
            else
            {
                lblMsg.Text = "Failed to Save Marks for " + drpStudent.SelectedItem.Text;
                lblMsg.ForeColor = Color.Red;
            }
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Failed to Save Data. Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
       
           
            TextBox grade = (TextBox)((Control)sender).Parent.Parent.FindControl("txtgrade");

            Button control1 = (Button)((Control)sender).Parent.Parent.FindControl("btnSave");
     
            Label admn = (Label)((Control)sender).Parent.Parent.FindControl("lblAdmnNo");
            Label control2 = (Label)((Control)sender).Parent.Parent.FindControl("lblStudName");
            HiddenField grdmrkid = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfGradeMrkId");
            if (grade.Text == "" || grade.Text == null)
            {
                control1.Attributes.Add("onclick", "javascript:return checkObtMarks('" + grade.ClientID + "');");
            }
            else
            {
                if (InsertExamMarks(Convert.ToInt64(admn.Text), Convert.ToInt32(drpSubject.SelectedValue), Convert.ToString(grade.Text), Convert.ToInt64(grdmrkid.Value)).Trim().ToUpper() == "S")
                {
                    Subjectwise();
                    lblMsg.Text = drpSubject.SelectedItem.Text + " Marks Submitted Successfully for " + control2.Text + " (" + admn.Text + ") ";
                    lblMsg.ForeColor = Color.Green;
                }
                else
                {
                    lblMsg.Text = "Failed to Save Marks for " + control2.Text + " (" + admn.Text + ") ";
                    lblMsg.ForeColor = Color.Red;
                }
            }
            grdSubjectwise.Focus();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Failed to Save Data. Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private string InsertExamMarks(long admno, int subid, string grade, long grdsubid)
    {

        lblMsg.Text = string.Empty;     
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@GradeMarkId",grdsubid);
        hashtable.Add("@SessionYr",drpSession.SelectedItem.Text);
        hashtable.Add("@ExamClsId",drpExam.SelectedValue);
        hashtable.Add("@AdmnNo", admno);
        hashtable.Add("@GradeSubjectId", subid);
        hashtable.Add("@Grade", grade);
        hashtable.Add("@UserId", Convert.ToInt32(Session["User_Id"].ToString()));

        return obj.ExecuteScalar("InsertGradeSubGrade", hashtable);
    }

    protected void grdStudentwise_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
     
        TextBox control2 = e.Row.FindControl("txtgrade") as TextBox;
        control2.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control2.ClientID + "');");


    }

    protected void grdSubjectwise_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        TextBox control2 = e.Row.FindControl("txtgrade") as TextBox;
        control2.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control2.ClientID + "');");

        Label subject = e.Row.FindControl("lblSubjectName")as Label;
        if (Convert.ToInt32(drpClass.SelectedValue) > 5 && Convert.ToInt32(drpClass.SelectedValue) < 11)
            if(drpSubject.SelectedItem.ToString()== "3RD LANGUAGE")
                subject.Visible = true;
            else
                subject.Visible = false;
        else
            subject.Visible = false;
    }



    protected void btnsaveall_Click(object sender, EventArgs e)
    {
        if (grdStudentwise.Rows.Count > 0)
        {
            foreach (GridViewRow row in grdStudentwise.Rows)
            {
                try
                {
                    //--------------------------------
                    Button control1 = (Button)row.FindControl("btnSubmit");
                    HiddenField subid = (HiddenField)row.FindControl("hfExamSubId");
                    HiddenField admno = (HiddenField)row.FindControl("hfadmnno");
                    HiddenField grdmrkid = (HiddenField)row.FindControl("hfGradeMrkId");
                    TextBox grade = (TextBox)row.FindControl("txtgrade");
                    if (InsertExamMarks(Convert.ToInt64(admno.Value), Convert.ToInt32(subid.Value), Convert.ToString(grade.Text), Convert.ToInt64(grdmrkid.Value)).Trim().ToUpper() == "S")
                    {
                        ClearGrids();
                        Studentwise();
                        lblMsg.Text = admno.Value + " Marks Submitted Successfully for " + drpStudent.SelectedItem.Text;
                        lblMsg.ForeColor = Color.Green;
                            
                    }
                    else
                    {
                        lblMsg.Text = "Failed to Save Marks for " + drpStudent.SelectedItem.Text;
                        lblMsg.ForeColor = Color.Red;
                    }
                    grdStudentwise.Focus();
                }
                catch (Exception ex)
                {
                    lblMsg.Text = "Failed to Save Data. Try Again.";
                    lblMsg.ForeColor = Color.Red;
                }

            }
        }
        else if(grdSubjectwise.Rows.Count>0)
        {
            foreach(GridViewRow row in grdSubjectwise.Rows)
            {
                try
                {
                    TextBox grade = (TextBox)row.FindControl("txtgrade");
                    Button control1 = (Button)row.FindControl("btnSave");
                    Label admn = (Label)row.FindControl("lblAdmnNo");
                    Label control2 = (Label)row.FindControl("lblStudName");
                    HiddenField grdmrkid = (HiddenField)row.FindControl("hfGradeMrkId");
                    if (InsertExamMarks(Convert.ToInt64(admn.Text), Convert.ToInt32(drpSubject.SelectedValue), Convert.ToString(grade.Text), Convert.ToInt64(grdmrkid.Value)).Trim().ToUpper() == "S")
                    {
                        Subjectwise();
                        lblMsg.Text = drpSubject.SelectedItem.Text + "  All Record Submitted Successfully " ;
                        lblMsg.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblMsg.Text = "Failed to Save Marks for " + control2.Text + " (" + admn.Text + ") ";
                        lblMsg.ForeColor = Color.Red;
                    }
                        grdSubjectwise.Focus();
                 }
                 catch (Exception ex)
                 {
                        lblMsg.Text = "Failed to Save Data. Try Again.";
                        lblMsg.ForeColor = Color.Red;
                 }
            }
        }
        else
        {
            return;
        }
    }
}