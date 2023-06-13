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

public partial class Exam_ExamModResult : System.Web.UI.Page
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
            drpStudent.Enabled = true;
        }
        else
        {
            drpSession.Items.Insert(0, new ListItem("- No Exams Defined -", "0"));
            drpClass.Enabled = false;
            drpSection.Enabled = false;
            drpExam.Enabled = false;
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
        drpSession.Focus();
        ClearGrid();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpSection.SelectedIndex = 0;
        drpExam.Items.Clear();
        drpStudent.Items.Clear();
        ClearGrid();
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
            FillSectionDropDown();
        }
        else
        {
            drpClass.SelectedIndex = 0;
            ScriptManager.RegisterClientScriptBlock((Control)drpClass, drpClass.GetType(), "ShowMessage", "alert('Select a Session');", true);
            drpSession.Focus();
        }
        drpClass.Focus();
        
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearGrid();
        if (drpSession.SelectedIndex > 0 && drpClass.SelectedIndex > 0 && drpExam.SelectedIndex > 0)
            BindStudent();
        else
            drpStudent.Items.Clear();
        drpSection.Focus();
    }

    protected void drpExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearGrid();
        if (drpSession.SelectedIndex > 0 && drpClass.SelectedIndex > 0)
            BindStudent();
        else
            drpStudent.Items.Clear();
        drpExam.Focus();
    }

    private void BindStudent()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@Modify", "True");
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
        lblTotStud.Text = "Total Students : " + dataTable.Rows.Count.ToString();
    }

    private void ClearGrid()
    {
        lblTotStud.Text = string.Empty;
        lblMsg.Text = string.Empty;
        grdMarks.DataSource = null;
        grdMarks.DataBind();
    }

    protected void drpStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        SearchStudent();
        drpStudent.Focus();
    }

    private void SearchStudent()
    {
        ClearGrid();
        grdMarks.DataSource = obj.GetDataTable("ExamGetMarksStudwise", new Hashtable()
    {
      {
         "@SessionYr",
         drpSession.SelectedValue
      },
      {
         "@ExamId",
         drpExam.SelectedValue
      },
      {
         "@AdmnNo",
         drpStudent.SelectedValue
      }
    }).DefaultView;
        grdMarks.DataBind();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        try
        {
            HiddenField control1 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfExamMarksId");
            HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfPassMarks");
            TextBox control3 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtTheory");
            TextBox control4 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtPract");
            TextBox control5 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtProject");
            TextBox control6 = (TextBox)((Control)sender).Parent.Parent.FindControl("txtProjName");
            Label control7 = (Label)((Control)sender).Parent.Parent.FindControl("lblSubject");
            TextBox control8 = (TextBox)((Control)sender).Parent.Parent.FindControl("tctmaviva");
            if (InsertExamMarks(Convert.ToInt32(control1.Value), Convert.ToDouble(control2.Value), Convert.ToDouble(control3.Text), Convert.ToDouble(control4.Text), Convert.ToDouble(control5.Text), control6.Text.Trim(), Convert.ToDouble(control8.Text)).Trim().ToUpper() == "S")
            {
                SearchStudent();
                lblMsg.Text = control7.Text + " Marks Saved Successfully for " + drpStudent.SelectedItem.Text;
                lblMsg.ForeColor = Color.Green;
            }
            else
            {
                lblMsg.Text = "Failed to Save Marks for " + drpStudent.SelectedItem.Text;
                lblMsg.ForeColor = Color.Red;
            }
            drpStudent.Focus();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Failed to Modify Marks. Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private string InsertExamMarks(int examMarksId, double passMarks, double theo, double pract, double proj, string projName, double MAVIVA)
    {
        lblMsg.Text = string.Empty;
        double num = theo + pract + proj + MAVIVA;
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

    protected void grdMarks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        Label control1 = e.Row.FindControl("lblTheory") as Label;
        TextBox control2 = e.Row.FindControl("txtTheory") as TextBox;
        HiddenField control3 = e.Row.FindControl("hfObtThMarks") as HiddenField;
        control2.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control3.Value + "','" + control1.Text + "', '" + control2.ClientID + "');");
        Label control4 = e.Row.FindControl("lblProject") as Label;
        TextBox control5 = e.Row.FindControl("txtProject") as TextBox;
        HiddenField control6 = e.Row.FindControl("hfObtProjMarks") as HiddenField;
        control5.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control6.Value + "','" + control4.Text + "', '" + control5.ClientID + "');");
        Label control7 = e.Row.FindControl("lblPract") as Label;
        TextBox control8 = e.Row.FindControl("txtPract") as TextBox;
        HiddenField control9 = e.Row.FindControl("hfObtPractMarks") as HiddenField;
        control8.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control9.Value + "','" + control7.Text + "', '" + control8.ClientID + "');");
        HiddenField control10 = e.Row.FindControl("hfIsPresent") as HiddenField;
        TextBox control11 = e.Row.FindControl("txtProjName") as TextBox;
        Button control12 = e.Row.FindControl("btnSubmit") as Button;
        Label control13 = e.Row.FindControl("lblmaviva") as Label;
        TextBox control14 = e.Row.FindControl("tctmaviva") as TextBox;
        control14.Attributes.Add("onkeyup", "javascript:return checkObtMarks('" + control13.Text + "', '" + control14.ClientID + "');");
        if (bool.Parse(control10.Value.ToString()))
            return;
        control1.Visible = false;
        control2.Visible = false;
        control7.Visible = false;
        control8.Visible = false;
        control4.Visible = false;
        control5.Visible = false;
        control11.Visible = false;
        control12.Visible = false;

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