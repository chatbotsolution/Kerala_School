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

public partial class Exam_ExamMarkEntry : System.Web.UI.Page
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
            //objStatic.FillSection(drpSection);
            //drpSection.Items.RemoveAt(0);
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
            FillSectionDropDown();
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
        DataTable dataTable = obj.GetDataTable("ExamGetStudentName", hashtable);
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
        hashtable.Add("@ExamClassId", drpExam.SelectedValue);
        DataTable dataTable2 = obj.GetDataTable("ExamGetExamSub", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            drpSubject.DataSource = dataTable2;
            drpSubject.DataTextField = "SubjectName";
            drpSubject.DataValueField = "ExamSubId";
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
        grdStudentwise.DataSource = obj.GetDataTable("ExamSubListForMarkEntry", new Hashtable()
    {
      {
         "@ExamId",
         drpExam.SelectedValue
      },
      {
         "@AdmnNo",
         drpStudent.SelectedValue
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
        hashtable.Add("@ExamId", drpExam.SelectedValue);
        hashtable.Add("@ExamSubId", drpSubject.SelectedValue);
        hashtable.Add("@SessionYr", drpSession.SelectedValue);
        if (drpSection.SelectedIndex > 0)
        hashtable.Add("@Section", drpSection.SelectedValue);
        DataTable dataTable = obj.GetDataTable("ExamStdListForMarkEntry", hashtable);
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
            HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfExamMarksId");
            HiddenField control3 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfPassMarks");
            TextBox control4 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtTheory");
            TextBox control5 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtPract");
            TextBox control6 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtProject");
            TextBox control7 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtProjName");
            Label control8 = (Label)((Control)sender).Parent.Parent.FindControl("lblSubject");
            TextBox control10 = (TextBox)((Control)sender).Parent.Parent.FindControl("tctmaviva");
            if (InsertExamMarks(Convert.ToInt32(control2.Value), Convert.ToDouble(control3.Value), Convert.ToDouble(control4.Text), Convert.ToDouble(control5.Text), Convert.ToDouble(control6.Text), control7.Text.Trim(), Convert.ToDouble(control10.Text)).Trim().ToUpper() == "S")
            {
                ClearGrids();
                Studentwise();
                lblMsg.Text = control8.Text + " Marks Submitted Successfully for " + drpStudent.SelectedItem.Text;
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
            Button control1 = (Button)((Control)sender).Parent.Parent.FindControl("btnSave");
            HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfExamMarksId");
            HiddenField control3 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfPassMarks");
            TextBox control4 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtTheory");
            TextBox control5 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtPract");
            TextBox control6 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtProject");
            TextBox control7 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtProjName");
            TextBox control10 = (TextBox)((Control)sender).Parent.Parent.FindControl("tctmaviva");
            Label control8 = (Label)((Control)sender).Parent.Parent.FindControl("lblAdmnNo");
            Label control9 = (Label)((Control)sender).Parent.Parent.FindControl("lblStudName");
            if (InsertExamMarks(Convert.ToInt32(control2.Value), Convert.ToDouble(control3.Value), Convert.ToDouble(control4.Text), Convert.ToDouble(control5.Text), Convert.ToDouble(control6.Text), control7.Text.Trim(), Convert.ToDouble(control10.Text)).Trim().ToUpper() == "S")
            {
                Subjectwise();
                lblMsg.Text = drpSubject.SelectedItem.Text + " Marks Submitted Successfully for " + control9.Text + " (" + control8.Text + ") ";
                lblMsg.ForeColor = Color.Green;
            }
            else
            {
                lblMsg.Text = "Failed to Save Marks for " + control9.Text + " (" + control8.Text + ") ";
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

    private string InsertExamMarks(int examMarksId, double passMarks, double theo, double pract, double proj, string projName,double MAVIVA)
    {
        lblMsg.Text = string.Empty;
        double num = theo + pract + proj;
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@ExamMarksId", examMarksId);
        hashtable.Add("@MarksObtained", num);
        hashtable.Add("@TheoryMarks", theo);
        hashtable.Add("@PractMarks", pract);
        hashtable.Add("@ProjMarks", proj);
        hashtable.Add("@MAVIVA", MAVIVA);
        if (projName.Trim() != string.Empty)
            hashtable.Add("@ProjName", projName);
        if (num >= passMarks)
            hashtable.Add("@IndSubResult", "PASS");
        else
            hashtable.Add("@IndSubResult", "FAIL");
        hashtable.Add("@UserId", Convert.ToInt32(Session["User_Id"].ToString()));
        return obj.ExecuteScalar("ExamInsertMarks", hashtable);
    }

    protected void grdStudentwise_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        Label control1 = e.Row.FindControl("lblTheory") as Label;
        TextBox control2 = e.Row.FindControl("txtTheory") as TextBox;
        control2.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control1.Text + "', '" + control2.ClientID + "');");
        Label control3 = e.Row.FindControl("lblProject") as Label;
        TextBox control4 = e.Row.FindControl("txtProject") as TextBox;
        control4.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control3.Text + "', '" + control4.ClientID + "');");
        Label control5 = e.Row.FindControl("lblPract") as Label;
        TextBox control6 = e.Row.FindControl("txtPract") as TextBox;
        control6.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control5.Text + "', '" + control6.ClientID + "');");
        Label control7 = e.Row.FindControl("lblmaviva") as Label;
        TextBox control8 = e.Row.FindControl("tctmaviva") as TextBox;
        control8.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control7.Text + "', '" + control8.ClientID + "');");
    }

    protected void grdSubjectwise_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        Label control1 = e.Row.FindControl("lblTheory") as Label;
        TextBox control2 = e.Row.FindControl("txtTheory") as TextBox;
        control2.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control1.Text + "', '" + control2.ClientID + "');");
        Label control3 = e.Row.FindControl("lblProject") as Label;
        TextBox control4 = e.Row.FindControl("txtProject") as TextBox;
        control4.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control3.Text + "', '" + control4.ClientID + "');");
        Label control5 = e.Row.FindControl("lblPract") as Label;
        TextBox control6 = e.Row.FindControl("txtPract") as TextBox;
        control6.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control5.Text + "', '" + control6.ClientID + "');");
        Label control7 = e.Row.FindControl("lblmaviva") as Label;
        TextBox control8 = e.Row.FindControl("tctmaviva") as TextBox;
        control8.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control7.Text + "', '" + control8.ClientID + "');");
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