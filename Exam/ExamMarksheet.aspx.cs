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

public partial class Exam_ExamMarksheet : System.Web.UI.Page
{
    private clsDAL objCom;
    private DataTable dt;
    private Hashtable ht;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Session["title"] = Page.Title.ToString();
        if (Request.QueryString["id"] == null)
            return;
        FillGrid();
    }

    private void FillGrid()
    {
        objCom = new clsDAL();
        dt = new DataTable();
        ht = new Hashtable();
        ht.Add("@ExamEnrolId", Request.QueryString["id"].ToString());
        ht.Add("@CallPage", 'I');
        dt = objCom.GetDataTable("SelectPradesikMedhaExam", ht);
        lblCul.Text = dt.Rows[0]["P2SansMO"].ToString();
        lblEng.Text = dt.Rows[0]["P2EngMO"].ToString();
        lblExam.Text = dt.Rows[0]["ExamName"].ToString();
        lblGK.Text = dt.Rows[0]["P1GKMO"].ToString();
        lblMath.Text = dt.Rows[0]["P1MathMO"].ToString();
        lblMIL.Text = dt.Rows[0]["P2MILMO"].ToString();
        lblName.Text = dt.Rows[0]["FullName"].ToString().ToUpper();
        lblPrat.Text = dt.Rows[0]["P1PratMO"].ToString();
        lblSc.Text = dt.Rows[0]["P1ScMO"].ToString();
        lblSchool.Text = dt.Rows[0]["NameOfSSVM"].ToString();
        lblSST.Text = dt.Rows[0]["P2SSTMO"].ToString();
        lblTot1.Text = dt.Rows[0]["P1TotMO"].ToString();
        lblTot2.Text = dt.Rows[0]["P2TotMo"].ToString();
        lblCulFM.Text = dt.Rows[0]["P2SansFM"].ToString();
        lblEngFM.Text = dt.Rows[0]["P2EngFM"].ToString();
        lblGKFM.Text = dt.Rows[0]["P1GKFM"].ToString();
        lblMathFM.Text = dt.Rows[0]["P1MathFM"].ToString();
        lblMILFM.Text = dt.Rows[0]["P2MILFM"].ToString();
        lblPratFM.Text = dt.Rows[0]["P1PratFM"].ToString();
        lblScFM.Text = dt.Rows[0]["P1ScFM"].ToString();
        lblSSTFM.Text = dt.Rows[0]["P2SSTFM"].ToString();
        lblTot1FM.Text = dt.Rows[0]["P1TotFM"].ToString();
        lblTot2FM.Text = dt.Rows[0]["P2TotFM"].ToString();
        lblRoll.Text = dt.Rows[0]["MedhaRollNo"].ToString();
        lblResult.Text = dt.Rows[0]["Result"].ToString();
        lblTotMarks.Text = dt.Rows[0]["P1P2TotMO"].ToString();
    }
}