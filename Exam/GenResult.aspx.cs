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

public partial class Exam_GenResult : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            Session["title"] = Page.Title.ToString();
            if (Session["User_Id"] != null)
            {
                BindSession();
                bindDropDown(drpClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
            }
            else
                Response.Redirect("../Login.aspx");
        }
        lblMsg.Text = string.Empty;
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
            drpExam.Enabled = true;
        }
        else
        {
            drpSession.Items.Insert(0, new ListItem("- No Exams Defined -", "0"));
            drpClass.Enabled = false;
            drpExam.Enabled = false;
        }
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        BindGrid();
    }

    private void BindGrid()
    {
        try
        {
            string str1 = obj.ExecuteScalar("ExamGetMarksEntered", new Hashtable()
      {
        {
           "@ExamId",
           drpExam.SelectedValue.ToString()
        }
      });
            if (str1.Trim().ToUpper() == "GO")
            {
                ht.Add("@SessionYr", drpSession.SelectedValue.ToString());
                ht.Add("@ExamId", drpExam.SelectedValue.ToString());
                ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
                string str2 = obj.ExecuteScalar("ExamGenerateResult", ht);
                if (str2.Trim() == string.Empty)
                {
                    lblMsg.Text = "Result Generated Successfully for Class : " + drpClass.SelectedItem.Text + ", Exam : " + drpExam.SelectedItem.Text + ", " + drpSession.SelectedItem.Text;
                    lblMsg.ForeColor = Color.Green;
                }
                else
                    lblMsg.Text = str2;
            }
            else if (str1.Trim().ToUpper() == "SUB")
            {
                lblMsg.Text = "Enter Marks of All Subjects for All Students before Generating Result.";
                lblMsg.ForeColor = Color.Red;
                btnGenerate.Focus();
            }
            else if (str1.Trim().ToUpper() == "STD")
            {
                lblMsg.Text = "Enter Marks of All Students before Generating Result.";
                lblMsg.ForeColor = Color.Red;
                btnGenerate.Focus();
            }
            else
            {
                lblMsg.Text = "Failed to Generate Result. Try Again.";
                lblMsg.ForeColor = Color.Red;
                btnGenerate.Focus();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Failed to Generate Result. Try Again.";
            lblMsg.ForeColor = Color.Red;
            btnGenerate.Focus();
        }
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpExam.Items.Clear();
        if (drpSession.SelectedIndex > 0 && drpClass.SelectedIndex > 0)
            FillExam();
        else
            drpClass.SelectedIndex = 0;
        drpSession.Focus();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpExam.Items.Clear();
        if (drpSession.SelectedIndex > 0 && drpClass.SelectedIndex > 0)
        {
            FillExam();
            drpClass.Focus();
        }
        else if (drpSession.SelectedIndex == 0)
        {
            lblMsg.Text = "Select a Session";
            lblMsg.ForeColor = Color.Red;
            drpClass.SelectedIndex = 0;
            drpSession.Focus();
        }
        else
            drpClass.Focus();
    }

    private void FillExam()
    {
        DataTable dataTable = obj.GetDataTable("ExamsForGenResult", new Hashtable()
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
        if (dataTable.Rows.Count > 0)
        {
            drpExam.DataSource = dataTable;
            drpExam.DataTextField = "ExamName";
            drpExam.DataValueField = "ExamClassId";
            drpExam.DataBind();
            drpExam.Items.Insert(0, new ListItem("- Select -", "0"));
        }
        else
            drpExam.Items.Insert(0, new ListItem("- No Exams -", "0"));
    }

    protected void drpExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpExam.Focus();
    }
}