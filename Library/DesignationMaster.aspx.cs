using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_DesignationMaster : System.Web.UI.Page
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
                if (Request.QueryString["DesignationId"] == null)
                    return;
               ht.Clear();
               ht.Add("@DesignationId", int.Parse(Request.QueryString["DesignationId"].ToString()));
               dt.Clear();
               dt =obj.GetDataTable("Lib_SP_GetDesignationList",ht);
                if (dt.Rows.Count <= 0)
                    return;
               txtDesigName.Text =dt.Rows[0]["DesignationName"].ToString();
               txtDesc.Text =dt.Rows[0]["Description"].ToString();
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
            if (Request.QueryString["DesignationId"] == null)
               ht.Add("@DesignationId", 0);
            else
               ht.Add("@DesignationId", int.Parse(Request.QueryString["DesignationId"].ToString()));
           ht.Add("@DesignationName", txtDesigName.Text.Trim());
           ht.Add("@Description", txtDesc.Text.Trim());
           ht.Add("@UserId", 1);
           dt.Clear();
           dt =obj.GetDataTable("Lib_SP_InsUpdtDesignation",ht);
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
       txtDesigName.Text = "";
       txtDesc.Text = "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       Clear();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
       Response.Redirect("DesignationList.aspx");
    }
}