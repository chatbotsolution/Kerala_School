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

public partial class Exam_ExamMarkList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (!Page.IsPostBack)
            {
                clsStaticDropdowns clsStaticDropdowns = new clsStaticDropdowns();
                BindSession();
                bindDrp(drpClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
                //clsStaticDropdowns.FillSection(drpSection);
                //drpSection.Items.RemoveAt(0);
                drpSection.Items.Insert(0, new ListItem("- ALL -", "0"));
            }
            lblTotStud.Text = string.Empty;
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
            drpExam.Enabled = true;
        }
        else
        {
            drpSession.Items.Insert(0, new ListItem("- No Exams Defined -", "0"));
            drpClass.Enabled = false;
            drpExam.Enabled = false;
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
        ClearGrid();
        drpClass.SelectedIndex = 0;
        drpSection.SelectedIndex = 0;
        drpExam.Items.Clear();
        drpSession.Focus();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearGrid();
        drpExam.Items.Clear();
        if (drpSession.SelectedIndex > 0)
        {
            drpExam.DataSource = obj.GetDataTable("ExamsForGenResult", new Hashtable()
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
        SearchStudents();
        drpSection.Focus();
    }

    protected void drpExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpExam.SelectedIndex > 0)
        {
            SearchStudents();
        }
        else
        {
            grdMarks.DataSource = null;
            grdMarks.DataBind();
        }
        drpExam.Focus();
    }

    private void SearchStudents()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Clear();
        hashtable.Add("@SessionYr", drpSession.SelectedValue);
        hashtable.Add("@ExamId", drpExam.SelectedValue);
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue);
        DataTable dataTable = obj.GetDataTable("ExamMarkList", hashtable);
        lblTotStud.Text = "Total Students : " + dataTable.Rows.Count.ToString();
        grdMarks.DataSource = dataTable.DefaultView;
        grdMarks.DataBind();
    }

    private void ClearGrid()
    {
        lblMsg.Text = string.Empty;
        grdMarks.DataSource = null;
        grdMarks.DataBind();
    }

    protected void grdMarks_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdMarks.PageIndex = e.NewPageIndex;
            SearchStudents();
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void grdMarks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        Label control1 = (Label)e.Row.FindControl("lblRemarks");
        Label control2 = (Label)e.Row.FindControl("lblView");
        if (!(control1.Text != ""))
            return;
        control2.Text = "View";
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