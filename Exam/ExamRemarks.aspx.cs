using ASP;
using SanLib;
using System;
using System.Collections;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Exam_ExamRemarks : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            lblStudentName.Text = Request.QueryString["SName"].ToString();
            lblAdmnNo.Text = Request.QueryString["AdmnNo"].ToString();
            txtRemarks.Text = obj.ExecuteScalarQry("select Remarks from dbo.ExamRemarks where AdmnNo=" + Request.QueryString["AdmnNo"] + "AND ExamId=" + Request.QueryString["ExamId"]);
        }
        else
            Response.Redirect("~/Login.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        obj.ExcuteProcInsUpdt("ExamSetRemarks", new Hashtable()
    {
      {
         "@ExamId",
         Request.QueryString["ExamId"]
      },
      {
         "@AdmnNo",
         Request.QueryString["AdmnNo"]
      },
      {
         "@Remarks",
         txtRemarks.Text
      },
      {
         "@OverAllMarks",
         Request.QueryString["Marks"]
      }
    });
        ScriptManager.RegisterClientScriptBlock((Control)btnSubmit, btnSubmit.GetType(), "ShowMessage", "alert('Remarks Saved Sucessfully');", true);
    }
}