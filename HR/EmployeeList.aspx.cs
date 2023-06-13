using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_EmployeeList : System.Web.UI.Page
{
    private clsDAL ObjDAL;
    private DataTable dt;
    private Hashtable ht;
    private int RecCount;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        bindDropDown(drpDept, "select DeptId,DeptName from DeptMaster order by DeptName", "DeptName", "DeptId");
        bindDropDown(drpDesignation, "select DesignationId,Designation from DesignationMaster order by Designation", "Designation", "DesignationId");
        ClearFields();
        FillGrid();
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        ObjDAL = new clsDAL();
        dt = ObjDAL.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--All--", "0"));
    }

    private void ClearFields()
    {
        drpDept.SelectedIndex = 0;
        drpDesignation.SelectedIndex = 0;
        txtEmpName.Text = string.Empty;
    }

    private void FillGrid()
    {
        ht = new Hashtable();
        dt = new DataTable();
        ObjDAL = new clsDAL();
        if (!drpDept.SelectedIndex.Equals(0))
            ht.Add("@DeptId", drpDept.SelectedValue);
        if (!drpDesignation.SelectedIndex.Equals(0))
            ht.Add("@DesignationID", drpDesignation.SelectedValue);
        if (!txtEmpName.Text.Trim().Equals(string.Empty))
            ht.Add("@EmpName", txtEmpName.Text.Trim());
        dt = ObjDAL.GetDataTable("GetEmployeeForGvBind", ht);
        grdEmp.DataSource = dt;
        grdEmp.DataBind();
        lblNoOfRec.Text = "No Of Records: " + dt.Rows.Count.ToString();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Employee.aspx");
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btndelete, btndelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str1 = Request["Checkb"];
            RecCount = 0;
            string str2 = str1;
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str2.Split(chArray))
            {
                if (DeleteRecord(obj.ToString()) > 0)
                    ++RecCount;
            }
            if (RecCount > 0)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Some of the Records could not be deleted');", true);
            FillGrid();
        }
    }

    private int DeleteRecord(string id)
    {
        ht = new Hashtable();
        dt = new DataTable();
        ObjDAL = new clsDAL();
        ht.Add("@EmpId", id);
        dt = ObjDAL.GetDataTable("DelEmpAfterCheck", ht);
        return dt.Rows.Count > 0 ? 1 : 0;
    }

    protected void gvEmpList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdEmp.PageIndex = e.NewPageIndex;
        FillGrid();
    }
}