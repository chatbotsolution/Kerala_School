using AjaxControlToolkit;
using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_DetailedEmployeeInformation : System.Web.UI.Page
{
    private DataTable dt;
    private clsDAL ObjCommon;
    private Hashtable ht;
    private int RecCount;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserId"] == null)
            Response.Redirect("../UserLogin.aspx");
        if (Page.IsPostBack)
            return;
        ViewState["ssvmid"] = string.Empty;
        Session["title"] = Page.Title.ToString();
        if (Request.QueryString["empno"] != null)
        {
            if (Request.QueryString["empno"].ToString().Equals("0"))
                Response.Redirect("../UserLogin.aspx");
            ObjCommon = new clsDAL();
            dt = new DataTable();
            dt = ObjCommon.GetDataTableQry("select SevName,SevabratiId from EmployeeMaster where EmpId=" + Request.QueryString["empno"]);
            lblEmpName.Text = dt.Rows[0]["SevName"].ToString() + "(" + dt.Rows[0]["SevabratiId"].ToString() + ")";
            FillEmpDataList();
            GridView control1 = (GridView)((Control)APAppointDet).FindControl("grdPastAppDetails");
            GridView control2 = (GridView)((Control)APTeachDet).FindControl("grdTeachingDetails");
            GridView control3 = (GridView)((Control)APGyanPariksha).FindControl("grdAddExamDetails");
            GridView control4 = (GridView)((Control)APTraining).FindControl("grdTrainingDetails");
            GridView control5 = (GridView)((Control)APDakshina).FindControl("grdDakshinaDetails");
            FillGrid(control1, "SelectEmpPastAptmnts", lblAppontRC);
            FillGrid(control2, "SelectEmpTeachingSubjects", lblTeachSubRC);
            FillGrid(control3, "SelectEmpAddnlExams", lblPrikshaRC);
            FillGrid(control4, "SelectEmpTrainings", lblTrainingRC);
            FillGrid(control5, "SelectEmpSalaryStruct", lblDakshinaRC);
            chkFrmPage();
        }
        else
            Response.Write("<script>history.back();</script>");
    }

    private void FillEmpDataList()
    {
        ht = new Hashtable();
        dt = new DataTable();
        ObjCommon = new clsDAL();
        if (Request.QueryString["empno"] != null)
            ht.Add("@EmpId", Request.QueryString["empno"]);
        dt = ObjCommon.GetDataTable("Est_SelEmployeeForDL", ht);
        DataList control = (DataList)((Control)APEmpDet).FindControl("DLEmployee");
        control.DataSource = dt;
        control.DataBind();
    }

    private void FillGrid(GridView gv, string sp, Label lbl)
    {
        ht = new Hashtable();
        dt = new DataTable();
        ObjCommon = new clsDAL();
        if (Request.QueryString["empno"] != null)
            ht.Add("@EmpId", Request.QueryString["empno"]);
        dt = ObjCommon.GetDataTable(sp, ht);
        gv.DataSource = dt;
        gv.DataBind();
        lbl.Text = "Total Records : " + dt.Rows.Count.ToString();
    }

    protected void btnAddNewApp_Click(object sender, EventArgs e)
    {
        Response.Redirect("PastAppointmentDetails.aspx?em=" + Request.QueryString["empno"] + "&d=1");
    }

    protected void btnAddNewTeach_Click(object sender, EventArgs e)
    {
        Response.Redirect("TeachingDetails.aspx?em=" + Request.QueryString["empno"] + "&d=1");
    }

    protected void btnAddNewPariksha_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdditionalExamDetails.aspx?em=" + Request.QueryString["empno"] + "&d=1");
    }

    protected void btnAddNewTraining_Click(object sender, EventArgs e)
    {
        Response.Redirect("TrainingDetails.aspx?em=" + Request.QueryString["empno"] + "&d=1");
    }

    protected void btnAddNewDakshina_Click(object sender, EventArgs e)
    {
        Response.Redirect("SalaryDetails.aspx?em=" + Request.QueryString["empno"] + "&d=1");
    }

    private int DeleteRecord(string id, string key, string StoredProc)
    {
        ht = new Hashtable();
        dt = new DataTable();
        ObjCommon = new clsDAL();
        ht.Add(key, id);
        dt = ObjCommon.GetDataTable(StoredProc, ht);
        return dt.Rows.Count > 0 ? 1 : 0;
    }

    protected void btnDeleteApp_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string[] strArray = Request["Checkb"].Split(',');
            RecCount = 0;
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (DeleteRecord(strArray[index].ToString(), "@PastApptId", "DeleteEmpPastAppointments") > 0)
                    ++RecCount;
            }
            if (RecCount > 0)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Some of the Records could not be deleted');", true);
            FillGrid((GridView)((Control)APAppointDet).FindControl("grdPastAppDetails"), "SelectEmpPastAptmnts", lblAppontRC);
        }
    }

    protected void btnDeleteTeach_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string[] strArray = Request["Checkb"].Split(',');
            RecCount = 0;
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (DeleteRecord(strArray[index].ToString(), "@EmpTeachingSubId", "DeleteEmpTeachingSubjects") > 0)
                    ++RecCount;
            }
            if (RecCount > 0)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Some of the Records could not be deleted');", true);
            FillGrid((GridView)((Control)APTeachDet).FindControl("grdTeachingDetails"), "SelectEmpTeachingSubjects", lblTeachSubRC);
        }
    }

    protected void btnDeletePariksha_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string[] strArray = Request["Checkb"].Split(',');
            RecCount = 0;
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (DeleteRecord(strArray[index].ToString(), "@EmpAddnlExamId", "DeleteEmpAddnlExamS") > 0)
                    ++RecCount;
            }
            if (RecCount > 0)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Some of the Records could not be deleted');", true);
            FillGrid((GridView)((Control)APGyanPariksha).FindControl("grdAddExamDetails"), "SelectEmpAddnlExams", lblPrikshaRC);
        }
    }

    protected void btnDeleteTraining_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string[] strArray = Request["Checkb"].Split(',');
            RecCount = 0;
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (DeleteRecord(strArray[index].ToString(), "@EmpTrgId", "DeleteEmpTrainings") > 0)
                    ++RecCount;
            }
            if (RecCount > 0)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Some of the Records could not be deleted');", true);
            FillGrid((GridView)((Control)APTraining).FindControl("grdTrainingDetails"), "SelectEmpTrainings", lblTrainingRC);
        }
    }

    protected void btnDeleteDakshina_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string[] strArray = Request["Checkb"].Split(',');
            RecCount = 0;
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (DeleteRecord(strArray[index].ToString(), "@EmpSalId", "DeleteEmpSalaryStruct") > 0)
                    ++RecCount;
            }
            if (RecCount > 0)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Some of the Records could not be deleted');", true);
            FillGrid((GridView)((Control)APDakshina).FindControl("grdDakshinaDetails"), "SelectEmpSalaryStruct", lblDakshinaRC);
        }
    }

    protected void grdPastAppDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdPastAppDetails.PageIndex = e.NewPageIndex;
        FillGrid((GridView)((Control)APAppointDet).FindControl("grdPastAppDetails"), "SelectEmpPastAptmnts", lblAppontRC);
    }

    protected void grdTeachingDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTeachingDetails.PageIndex = e.NewPageIndex;
        FillGrid((GridView)((Control)APTeachDet).FindControl("grdTeachingDetails"), "SelectEmpTeachingSubjects", lblTeachSubRC);
    }

    protected void grdAddExamDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdAddExamDetails.PageIndex = e.NewPageIndex;
        FillGrid((GridView)((Control)APGyanPariksha).FindControl("grdAddExamDetails"), "SelectEmpAddnlExams", lblPrikshaRC);
    }

    protected void grdTrainingDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTrainingDetails.PageIndex = e.NewPageIndex;
        FillGrid((GridView)((Control)APTraining).FindControl("grdTrainingDetails"), "SelectEmpTrainings", lblTrainingRC);
    }

    protected void grdDakshinaDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdDakshinaDetails.PageIndex = e.NewPageIndex;
        FillGrid((GridView)((Control)APDakshina).FindControl("grdDakshinaDetails"), "SelectEmpSalaryStruct", lblDakshinaRC);
    }

    protected void btnEditEmp_Click(object sender, EventArgs e)
    {
        Response.Redirect("OldEmployee.aspx?empno=" + Request.QueryString["empno"].ToString());
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("OldEmployee.aspx");
    }

    private void chkFrmPage()
    {
        if (Request.QueryString["a"] != null)
            Accordion1.SelectedIndex = 1;
        if (Request.QueryString["b"] != null)
            Accordion1.SelectedIndex = 2;
        if (Request.QueryString["c"] != null)
            Accordion1.SelectedIndex = 3;
        if (Request.QueryString["d"] != null)
            Accordion1.SelectedIndex = 4;
        if (Request.QueryString["e"] == null)
            return;
        Accordion1.SelectedIndex = 5;
    }

    protected void grdTeachingDetails_PreRender(object sender, EventArgs e)
    {
        MergeCell((GridView)sender, new int[2]
    {
      2,
      3
    });
    }

    public void MergeCell(GridView gv, int[] colIndices)
    {
        foreach (int colIndex in colIndices)
        {
            for (int index = gv.Rows.Count - 2; index >= 0; --index)
            {
                GridViewRow row1 = gv.Rows[index];
                GridViewRow row2 = gv.Rows[index + 1];
                if (row1.Cells[colIndex].Text == row2.Cells[colIndex].Text)
                {
                    row1.Cells[colIndex].RowSpan = row2.Cells[colIndex].RowSpan < 2 ? 2 : row2.Cells[colIndex].RowSpan + 1;
                    row2.Cells[colIndex].Visible = false;
                }
            }
        }
    }

    protected void btnAddNewSS_Click(object sender, EventArgs e)
    {
        Response.Redirect("Employee.aspx");
    }

    protected void grdPastAppDetails_RowDataBound(object sender, GridViewRowEventArgs e)
    {
    }

    protected void DLEmployee_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        GridView control = (GridView)e.Item.FindControl("grdEduQualApp");
        ht = new Hashtable();
        dt = new DataTable();
        ObjCommon = new clsDAL();
        if (Request.QueryString["empno"] != null)
            ht.Add("@EmpId", Request.QueryString["empno"]);
        dt = ObjCommon.GetDataTable("Est_EmpQualDetail", ht);
        control.DataSource = dt;
        control.DataBind();
    }

    protected void grdEduQualApp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmpListSSVM.aspx");
    }
}