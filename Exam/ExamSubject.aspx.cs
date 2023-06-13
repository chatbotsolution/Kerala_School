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

public partial class Exam_ExamSubject : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (!Page.IsPostBack)
            {
                ViewState["Sid"] = string.Empty;
                Session["title"] = Page.Title.ToString();
                BindSession();
                BindDropDown(drpClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
                if (Request.QueryString["Sid"] != null)
                {
                    FillData(sender, e);
                    ViewState["Sid"] = "Update";
                }
            }
            btnShow.Enabled = true;
            btnSubmit.Enabled = true;
            lbl.Visible = false;
        }
        else
            Response.Redirect("~/Login.aspx");
    }

    private void BindSession()
    {
        ViewState["Session"] = obj.ExecuteScalarQry("select top 1 SessionYr from dbo.ExamDetails order by UserDt desc");
        DataTable dataTableQry = obj.GetDataTableQry("SELECT DISTINCT SessionYr FROM ExamDetails ORDER BY SessionYr DESC");
        if (dataTableQry.Rows.Count == 0)
        {
            DataRow row1 = dataTableQry.NewRow();
            row1["SessionYr"] = (DateTime.Now.AddYears(-1).ToString("yyyy") + "-" + DateTime.Now.ToString("yy"));
            dataTableQry.Rows.Add(row1);
            DataRow row2 = dataTableQry.NewRow();
            row2["SessionYr"] = (DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.AddYears(1).ToString("yy"));
            dataTableQry.Rows.Add(row2);
            DataRow row3 = dataTableQry.NewRow();
            row3["SessionYr"] = (DateTime.Now.AddYears(1).ToString("yyyy") + "-" + DateTime.Now.AddYears(2).ToString("yy"));
            dataTableQry.Rows.Add(row3);
            dataTableQry.AcceptChanges();
        }
        drpSession.DataSource = dataTableQry;
        drpSession.DataValueField = "SessionYr";
        drpSession.DataTextField = "SessionYr";
        drpSession.DataBind();
        drpSession.Items.Insert(0, new ListItem("- Select -", "0"));
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

    private void FillData(object send, EventArgs ev)
    {
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        float num4 = 0.0f;
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        btnSubmit.Visible = false;
        btnCancel.Visible = false;
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("ExamGetSubDtls", new Hashtable()
    {
      {
         "@ExamSubId",
         Request.QueryString["Sid"].ToString().Trim()
      }
    });
        drpSession.SelectedValue = dataTable2.Rows[0]["SessionYr"].ToString();
        drpClass.SelectedValue = dataTable2.Rows[0]["ForClassId"].ToString();
        GetSubjects();
        drpSubject.SelectedValue = dataTable2.Rows[0]["ExamSubject"].ToString();
        GetExams();
        chkExam.SelectedValue = dataTable2.Rows[0]["ExamClassId"].ToString();
        drpSession.Enabled = false;
        drpSubject.Enabled = false;
        drpClass.Enabled = false;
        chkExam.Enabled = false;
        if (dataTable2.Rows[0]["FullMarks"].ToString().Trim() != string.Empty)
            num1 = float.Parse(dataTable2.Rows[0]["FullMarks"].ToString().Trim());
        if (dataTable2.Rows[0]["ProjMarks"].ToString().Trim() != string.Empty)
            num3 = float.Parse(dataTable2.Rows[0]["ProjMarks"].ToString().Trim());
        if (dataTable2.Rows[0]["PractMarks"].ToString().Trim() != string.Empty)
            num2 = float.Parse(dataTable2.Rows[0]["PractMarks"].ToString().Trim());
        if (dataTable2.Rows[0]["MAVIVA"].ToString().Trim() != string.Empty)
            num4 = float.Parse(dataTable2.Rows[0]["MAVIVA"].ToString().Trim());
        txtTheory.Text = num1.ToString();
        txtPractical.Text = num2.ToString();
        txtProject.Text = num3.ToString();
        txtmaviva.Text = num4.ToString();
        txtFullMarks.Text = (num1 + num3 + num3+num4).ToString();
        txtPassMarks.Text = dataTable2.Rows[0]["PassMarks"].ToString();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        chkExam.Items.Clear();
        drpSubject.Items.Clear();
        GetSubjects();
        drpClass.Focus();
    }

    private void GetSubjects()
    {
        if (drpClass.SelectedIndex <= 0)
            return;
        BindDropDown(drpSubject, "SELECT SubjectId,SubjectName FROM dbo.PS_SubjectMaster WHERE ClassId=" + drpClass.SelectedValue + " ORDER BY SubjectName ASC", "SubjectName", "SubjectId");
    }

    protected void drpSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        GetExams();
        drpSubject.Focus();
    }

    private void GetExams()
    {
        if (drpClass.SelectedIndex > 0 && drpSubject.SelectedIndex > 0)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@ClassID", drpClass.SelectedValue);
            hashtable.Add("@SessionYr", drpSession.SelectedValue);
            hashtable.Add("@SubjectId", drpSubject.SelectedValue);
            if (Request.QueryString["Sid"] != null)
                hashtable.Add("@ExamSubId", Request.QueryString["Sid"]);
            DataTable dataTable = obj.GetDataTable("ExamGetExamDtls", hashtable);
            if (dataTable.Rows.Count > 0)
            {
                chkExam.DataSource = dataTable;
                chkExam.DataTextField = "ExamName";
                chkExam.DataValueField = "ExamClassId";
                chkExam.DataBind();
            }
            else
            {
                lbl.Visible = true;
                btnShow.Enabled = false;
                btnSubmit.Enabled = false;
            }
        }
        else
            chkExam.Items.Clear();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        InsertData();
        drpSession.Focus();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        InsertData();
        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Saved Successfully');window.location='ExamSubjectList.aspx'", true);
    }

    private void InsertData()
    {
        string str = string.Empty;
        for (int index = 0; index < chkExam.Items.Count; ++index)
        {
            if (chkExam.Items[index].Selected)
            {
                Hashtable hashtable = new Hashtable();
                hashtable.Clear();
                if (Request.QueryString["Sid"] != null && ViewState["Sid"].ToString() == "Update")
                    hashtable.Add("@ExamSubId", Request.QueryString["Sid"].ToString());
                hashtable.Add("@ExamId", chkExam.Items[index].Value);
                hashtable.Add("@ExamSubject", drpSubject.SelectedValue);
                hashtable.Add("@FullMarks", txtTheory.Text.Trim());
                hashtable.Add("@ProjMarks", txtProject.Text.Trim());
                hashtable.Add("@PractMarks", txtPractical.Text.Trim());
                hashtable.Add("@PassMarks", txtPassMarks.Text.Trim());
                hashtable.Add("@MAVIVA", txtmaviva.Text.Trim());
                str = obj.ExecuteScalar("ExamInsUpdtSub", hashtable);
                if (str.Trim() != string.Empty)
                {
                    trMsg.Style["background-color"] = "Red";
                    lblMsg.Text = str;
                    break;
                }
            }
        }
        if (!(str.Trim() == string.Empty))
            return;
        ClearAllFields();
        ViewState["Sid"] = string.Empty;
        trMsg.Style["background-color"] = "Green";
        lblMsg.Text = "Data Saved Successfully";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        drpSession.Enabled = true;
        drpSession.SelectedIndex = 0;
        drpClass.Enabled = true;
        drpClass.SelectedIndex = 0;
        drpSubject.Items.Clear();
        ClearAllFields();
    }

    private void ClearAllFields()
    {
        drpSubject.Enabled = true;
        lbl.Visible = false;
        chkExam.Items.Clear();
        drpSubject.SelectedIndex = 0;
        txtTheory.Text = txtPractical.Text = txtProject.Text = txtFullMarks.Text = txtPassMarks.Text=txtmaviva.Text = "0";
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        btnShow.Enabled = true;
        btnSubmit.Enabled = true;
        drpSession.Focus();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ExamSubjectList.aspx");
    }
}