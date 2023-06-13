using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Exam_ExaminationLIst : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt;
    private int RecCount;
    private Hashtable ht;
    //protected Label lblMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (!Page.IsPostBack)
            {
                Session["title"] = Page.Title.ToString();
                BindSession();
                bindDrp(drpClass, "select ClassId,ClassName from dbo.PS_ClassMaster order by ClassId", "ClassName", "ClassId");
                if (ViewState["Session"].ToString().Trim() == string.Empty)
                    btnExpXam.Enabled = false;
            }
            btnExport.Enabled = false;
            btnDelete.Enabled = false;
            lblMsg.Text = string.Empty;
            lblMsg.ForeColor = Color.Green;
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
        drpSession.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    private void bindDrp(DropDownList drp, string query, string textField, string valueField)
    {
        dt = obj.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = textField;
        drp.DataValueField = valueField;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        drpSession.Focus();
    }

    protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSession.SelectedIndex > 0)
        {
            FillGrid();
            drpStatus.Focus();
        }
        else
        {
            drpStatus.SelectedIndex = 0;
            drpClass.SelectedIndex = 0;
            lblMsg.Text = "Select a Session";
            lblMsg.ForeColor = Color.Red;
            drpSession.Focus();
        }
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSession.SelectedIndex > 0)
        {
            FillGrid();
            drpClass.Focus();
        }
        else
        {
            drpStatus.SelectedIndex = 0;
            drpClass.SelectedIndex = 0;
            lblMsg.Text = "Select a Session";
            lblMsg.ForeColor = Color.Red;
            drpSession.Focus();
        }
    }

    private void FillGrid()
    {
        if (drpSession.SelectedIndex > 0)
        {
            clsDAL clsDal = new clsDAL();
            ht = new Hashtable();
            ht.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
            ht.Add("@ClassId", drpClass.SelectedValue.ToString().Trim());
            dt = new DataTable();
            dt = clsDal.GetDataTable("ExamGetFullExamDetails", ht);
            gvExam.DataSource = dt.DefaultView;
            gvExam.DataBind();
            lblRecCount.Text = "No Of Records : " + dt.Rows.Count.ToString();
            gvExam.Visible = true;
            if (dt.Rows.Count > 0)
            {
                btnExport.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnExport.Enabled = false;
                btnDelete.Enabled = false;
            }
        }
        else
        {
            gvExam.Visible = false;
            btnExport.Enabled = false;
            btnDelete.Enabled = false;
            lblRecCount.Text = string.Empty;
        }
    }

    private int DelExamDetails(string id)
    {
        ht = new Hashtable();
        dt = new DataTable();
        obj = new clsDAL();
        ht.Add("@ExamId", id);
        dt = obj.GetDataTable("ExamDeleteExamDetails", ht);
        return dt.Rows.Count > 0 ? 1 : 0;
    }

    protected void gvExam_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvExam.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        btnExport.Enabled = true;
        btnDelete.Enabled = true;
        fldexamination.Visible = true;
        FillGrid();
    }

    private void ExportToExcelFromGridview(GridView gv)
    {
        gv.Columns[0].Visible = false;
        gv.Columns[1].Visible = false;
        Response.Clear();
        Response.AddHeader("content-disposition", string.Format("attachment;filename=ExaminationList.xls"));
        Response.Charset = "";
        Response.ContentType = "application/vnd.xls";
        StringWriter stringWriter = new StringWriter();
        HtmlTextWriter writer = new HtmlTextWriter((TextWriter)stringWriter);
        gv.RenderControl(writer);
        Response.Write(stringWriter.ToString());
        Response.End();
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ExportToExcelFromGridview(gvExam);
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Examination.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str1 = Request["Checkb"];
            RecCount = 0;
            string str2 = str1;
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str2.Split(chArray))
            {
                if (DelExamDetails(obj.ToString()) > 0)
                    ++RecCount;
            }
            if (RecCount > 0)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Record already in use. Cannot be Deleted.');", true);
            FillGrid();
        }
    }

    protected void btnExpXam_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        string[] strArray = ViewState["Session"].ToString().Trim().Split('-');
        string str1 = (Convert.ToInt32(strArray[0]) + 1).ToString();
        string str2 = (Convert.ToInt32(strArray[1]) + 1).ToString();
        hashtable.Add("@SessionYrNxt", (str1 + "-" + str2));
        hashtable.Add("@SessionYrCrr", ViewState["Session"].ToString().Trim());
        hashtable.Add("@UserId", Session["User_Id"].ToString().Trim());
        if (obj.ExecuteScalar("ExamExportExamDtls", hashtable).Trim().ToUpper() == "S")
        {
            BindSession();
            lblMsg.Text = "Exam Details Imported Successfully for the Session " + ViewState["Session"] + ". Please Change Exam Start Dates, End Dates and Subjects According to your Requirement.";
            lblMsg.ForeColor = Color.Green;
            btnExpXam.Enabled = false;
        }
        else
        {
            lblMsg.Text = "Failed to Import Exam Details. Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void lbtnEdit_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = (LinkButton)sender;
        if (obj.ExecuteScalar("ExamGetExamStatus", new Hashtable()
    {
      {
         "@ExamId",
         linkButton.CommandArgument.ToString().Trim()
      }
    }) == "No")
        {
            lblMsg.Text = "Exam already in use. Can't be modified.";
            lblMsg.ForeColor = Color.Red;
        }
        else
            Response.Redirect("Examination.aspx?eid=" + linkButton.CommandArgument.ToString().Trim());
    }
}