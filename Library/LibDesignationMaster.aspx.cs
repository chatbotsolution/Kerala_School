using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_LibDesignationMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (this.Page.IsPostBack)
                return;
           Session["title"] =Page.Title.ToString();
           Form.DefaultButton =btnSaveAddNew.UniqueID;
            if (this.Session["User_Id"] != null)
            {
                if (this.Request.QueryString["DesignationId"] == null)
                    return;
               ht.Clear();
               ht.Add("@DesignationId", int.Parse(this.Request.QueryString["DesignationId"].ToString()));
               dt.Clear();
               dt =obj.GetDataTable("Lib_SP_GetDesignationList",ht);
                if (this.dt.Rows.Count <= 0)
                    return;
               txtDesigName.Text =dt.Rows[0]["DesignationName"].ToString();
               txtDesc.Text =dt.Rows[0]["Description"].ToString();
               btnSaveAddNew.Text = "Update & AddNew";
               btnSaveGotoList.Text = "Update & GotoList";
            }
            else
               Response.Redirect("../Login.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnSaveAddNew_Click(object sender, EventArgs e)
    {
       SaveData();
    }

    protected void btnSaveGotoList_Click(object sender, EventArgs e)
    {
       SaveData();
       Response.Redirect("LibDesignationList.aspx");
    }

    private void SaveData()
    {
        try
        {
           ht.Clear();
            if (this.Request.QueryString["DesignationId"] == null)
               ht.Add("@DesignationId", 0);
            else if (this.btnSaveAddNew.Text == "Update & AddNew" ||btnSaveGotoList.Text == "Update & GotoList")
               ht.Add("@DesignationId", int.Parse(this.Request.QueryString["DesignationId"].ToString()));
            else
               ht.Add("@DesignationId", 0);
           ht.Add("@DesignationName",txtDesigName.Text.Trim());
           ht.Add("@Description",txtDesc.Text.Trim());
           ht.Add("@UserId", 1);
           dt.Clear();
           dt =obj.GetDataTable("Lib_SP_InsUpdtDesignation",ht);
            if (this.dt.Rows.Count > 0)
            {
               ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowMessage", "alert(' Designation Already Exist');", true);
            }
            else
            {
                if (this.btnSaveAddNew.Text == "Update & AddNew" ||btnSaveGotoList.Text == "Update & GotoList")
                {
                   ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowMessage", "alert(' Data Updated Successfully');", true);
                   btnSaveAddNew.Text = "Save & AddNew";
                   btnSaveGotoList.Text = "Save & GotoList";
                }
                else
                   ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowMessage", "alert(' Data Save Successfully');", true);
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
       txtDesigName.Text = "";
       txtDesc.Text = "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       Clear();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
       Response.Redirect("LibDesignationList.aspx");
    }
}