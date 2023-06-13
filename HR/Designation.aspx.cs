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

public partial class HR_Designation : System.Web.UI.Page
{
    private clsDAL ObjDAL;
    private DataTable dt;
    private Hashtable ht;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Request.QueryString["dgn"] == null)
            return;
        FillFields();
    }

    private void FillFields()
    {
        ObjDAL = new clsDAL();
        dt = new DataTable();
        dt = ObjDAL.GetDataTableQry("Select * From HR_DesignationMaster Where DesgId=" + Request.QueryString["dgn"].ToString());
        txtDesignation.Text = dt.Rows[0]["Designation"].ToString();
        drpType.SelectedValue = dt.Rows[0]["TeachingStaff"].ToString().Trim().ToUpper();
        hfSortOrder.Value = dt.Rows[0]["SortOrder"].ToString().Trim();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        InsertToTable();
        ClearFields();
    }

    private void InsertToTable()
    {
        ht = new Hashtable();
        dt = new DataTable();
        ObjDAL = new clsDAL();
        if (Request.QueryString["dgn"] != null)
            ht.Add("@DesignationId", Request.QueryString["dgn"].ToString());
        ht.Add("@Designation", txtDesignation.Text.Trim());
        ht.Add("@TeachingStaff", drpType.SelectedValue);
        ht.Add("@SortOrder", hfSortOrder.Value);
        ht.Add("@UserId", Session["User_Id"]);
        dt = ObjDAL.GetDataTable("HR_InstUpdtDesignation", ht);
        if (dt.Rows.Count > 0)
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = dt.Rows[0][0].ToString();
        }
        else if (Request.QueryString["dgn"] != null)
        {
            Response.Redirect("DesignationList.aspx");
        }
        else
        {
            ClearFields();
            trMsg.Style["Background-Color"] = "Green";
            lblMsg.Text = "Data Saved Successfully";
        }
    }

    private void ClearFields()
    {
        txtDesignation.Text = string.Empty;
        drpType.SelectedIndex = 0;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Response.Redirect("DesignationList.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/HR/HRHome.aspx");
    }
}