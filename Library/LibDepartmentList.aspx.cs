using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_LibDepartmentList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
       Session["title"] = Page.Title.ToString();
       FillGrid(0);
    }

    private void FillGrid(int DeptId)
    {
       ht.Clear();
       ht.Add("@DeptId", DeptId);
       dt =obj.GetDataTable("Lib_SP_GetDepartmentList",ht);
       grdDeptList.DataSource = dt;
       grdDeptList.DataBind();
       lblRecords.Text =dt.Rows.Count.ToString();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                string str =Request["Checkb"];
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str.Split(chArray))
                   DeleteAccession(obj.ToString());
            }
           FillGrid(0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DeleteAccession(string id)
    {
       ht.Clear();
       ht.Add("@DeptId", id);
        string str =obj.ExecuteScalar("Lib_SP_DeleteDePartment",ht);
        if (str == "")
            ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('Deleted Successfully');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(" + str + ");", true);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
       Response.Redirect("DepartmentMaster.aspx");
    }

    protected void grdDeptList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
           grdDeptList.PageIndex = e.NewPageIndex;
           FillGrid(0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}