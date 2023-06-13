using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Exam_ExamSubjectList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            Session["title"] = Page.Title.ToString();
            BindSession();
            BindDropDown(drpClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
            btnDelete.Enabled = false;
        }
        else
            Response.Redirect("~/Login.aspx");
    }

    private void BindSession()
    {
        ViewState["Session"] = obj.ExecuteScalarQry("select top 1 SessionYr from dbo.ExamDetails order by UserDt desc");
        drpSession.DataSource = obj.GetDataTableQry("SELECT DISTINCT SessionYr FROM ExamDetails ORDER BY SessionYr DESC");
        drpSession.DataValueField = "SessionYr";
        drpSession.DataTextField = "SessionYr";
        drpSession.DataBind();
        drpSession.SelectedValue = DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.AddYears(1).ToString("yy");
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
        drpExamName.Items.Clear();
        GetExamName();
        drpSession.Focus();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpExamName.Items.Clear();
        GetExamName();
        drpClass.Focus();
    }

    private void GetExamName()
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        gvExamSubject.DataSource = null;
        gvExamSubject.DataBind();
        lblRecCount.Text = string.Empty;
        DataTable dataTable = obj.GetDataTable("ExamGetExamDetails", new Hashtable()
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
        if (dataTable.Rows.Count <= 0)
            return;
        drpExamName.DataSource = dataTable;
        drpExamName.DataTextField = "ExamName";
        drpExamName.DataValueField = "ExamClassId";
        drpExamName.DataBind();
        drpExamName.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    protected void drpExamName_SelectedIndexChanged(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        gvExamSubject.DataSource = null;
        gvExamSubject.DataBind();
        lblRecCount.Text = string.Empty;
        FillGrid();
        drpExamName.Focus();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        FillGrid();
    }

    private void FillGrid()
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@ClassId", drpClass.SelectedValue.ToString());
        hashtable.Add("@ExamClassId", drpExamName.SelectedValue.ToString());
        DataTable dataTable2 = obj.GetDataTable("ExamGetExamSub", hashtable);
        gvExamSubject.DataSource = dataTable2.DefaultView;
        gvExamSubject.DataBind();
        lblRecCount.Text = "No of Records : " + dataTable2.Rows.Count.ToString();
        if (dataTable2.Rows.Count <= 0)
            btnDelete.Enabled = false;
        else
            btnDelete.Enabled = true;
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("ExamSubject.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        string empty = string.Empty;
        int num1 = 0;
        int num2 = 0;
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select any Record');", true);
            btnDelete.Focus();
        }
        else
        {
            string str1 = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str1.Split(chArray))
            {
                string str2 = DelExamSubject(obj.ToString());
                if (str2.Trim().ToUpper() == "S")
                    ++num2;
                else if (str2.Trim().ToUpper() == "F")
                    ++num1;
            }
            drpExamName_SelectedIndexChanged(drpExamName, EventArgs.Empty);
            FillGrid();
            string str3;
            if (num1 > 0)
                str3 = num2.ToString() + " Record(s) Deleted. " + num1 + " Record(s) Unable to Delete.";
            else
                str3 = num2.ToString() + " Record(s) Deleted.";
            trMsg.Style["background-color"] = "Green";
            lblMsg.Text = str3;
        }
        btnDelete.Focus();
    }

    private string DelExamSubject(string id)
    {
        string empty = string.Empty;
        return obj.ExecuteScalar("ExamDeleteSubject", new Hashtable()
    {
      {
         "@ExamSubId",
         id
      }
    });
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ExportToExcelFromGridview(gvExamSubject);
    }

    private void ExportToExcelFromGridview(GridView gv)
    {
        gv.Columns[0].Visible = false;
        gv.Columns[1].Visible = false;
        Response.Clear();
        Response.AddHeader("content-disposition", string.Format("attachment;filename=ExamSubjectList.xls"));
        Response.Charset = "";
        Response.ContentType = "application/vnd.xls";
        StringWriter stringWriter = new StringWriter();
        HtmlTextWriter writer = new HtmlTextWriter((TextWriter)stringWriter);
        gv.RenderControl(writer);
        Response.Write(stringWriter.ToString());
        Response.End();
    }
}