using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Exam_ExamClass : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["ExamId"] == null)
            return;
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@ExamId", Request.QueryString["ExamId"].ToString().Trim());
        DataTable dataTable2 = obj.GetDataTable("ExamGetExamClass", hashtable);
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<b>Exam: " + Request.QueryString["ExamNm"].ToString() + "</b><br />");
        stringBuilder.Append("<b>Applicable to Classes: </b>");
        if (dataTable2.Rows.Count <= 0)
            return;
        foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            stringBuilder.Append(row["ClassName"].ToString() + "&nbsp;&nbsp;");
        lblClass.Text = stringBuilder.ToString();
    }
}