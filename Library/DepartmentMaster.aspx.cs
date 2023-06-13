using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_DepartmentMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsPostBack)
                return;
            Session["title"] = Page.Title.ToString();
            if (Session["User_Id"] != null)
            {
                if (Request.QueryString["DeptId"] == null)
                    return;
                ht.Clear();
                ht.Add("@DeptId", int.Parse(Request.QueryString["DeptId"].ToString()));
                dt.Clear();
                dt = obj.GetDataTable("Lib_SP_GetDepartmentList", ht);
                if (dt.Rows.Count <= 0)
                    return;
                txtDeptName.Text = dt.Rows[0]["DeptName"].ToString();
                txtDesc.Text = dt.Rows[0]["Description"].ToString();
            }
            else
                Response.Redirect("../Login.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            if (Request.QueryString["DeptId"] == null)
                ht.Add("@DeptId", 0);
            else
                ht.Add("@DeptId", int.Parse(Request.QueryString["DeptId"].ToString()));
            ht.Add("@DeptName", txtDeptName.Text.Trim());
            ht.Add("@Description", txtDesc.Text.Trim());
            ht.Add("@UserId", 1);
            dt.Clear();
            dt = obj.GetDataTable("Lib_SP_InsUpdtDepartment", ht);
            if (dt.Rows.Count > 0)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert(' Designation Already Exist');", true);
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert(' Data Save Successfully');", true);
                Clear();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void Clear()
    {
        txtDeptName.Text = "";
        txtDesc.Text = "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Response.Redirect("DepartmentList.aspx");
    }
}