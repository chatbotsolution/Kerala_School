using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_PublisherMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["User_Id"] != null)
            {
               txtPublisherName.Focus();
                if (Page.IsPostBack)
                    return;
               Session["title"] = Page.Title.ToString();
               Form.DefaultButton =btnSaveAddNew.UniqueID;
                if (Request.QueryString["PublisherId"] == null)
                    return;
               ht.Clear();
               ht.Add("@PublisherId", int.Parse(Request.QueryString["PublisherId"].ToString()));
               ht.Add("@CollegeId",Session["SchoolId"]);
               dt.Clear();
               dt =obj.GetDataTable("Lib_SP_GetPublisherList",ht);
                if (dt.Rows.Count <= 0)
                    return;
               txtPublisherName.Text =dt.Rows[0]["PublisherName"].ToString();
               txtPublisherPlace.Text = dt.Rows[0]["PublisherPlace"].ToString();
               txtRemarks.Text =dt.Rows[0]["Remarks"].ToString();
               btnSaveAddNew.Text = "Update & AddNew";
               btnSaveGotoList.Text = "Update & GotoList";
            }
            else
               Response.Redirect("../Login.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnSaveAddNew_Click(object sender, EventArgs e)
    {
       SaveData();
    }

    protected void btnSaveGotoList_Click(object sender, EventArgs e)
    {
       SaveData();
       Response.Redirect("PublisherList.aspx");
    }

    private void SaveData()
    {
        try
        {
           ht.Clear();
            if (Request.QueryString["PublisherId"] == null)
               ht.Add("@PublisherId", 0);
            else if (btnSaveAddNew.Text == "Update & AddNew" ||btnSaveGotoList.Text == "Update & GotoList")
               ht.Add("@PublisherId", int.Parse(Request.QueryString["PublisherId"].ToString()));
            else
               ht.Add("@PublisherId", 0);
           ht.Add("@PublisherName", txtPublisherName.Text.Trim());
           ht.Add("@PublisherPlace", txtPublisherPlace.Text.Trim());
           ht.Add("@Remarks", txtRemarks.Text.Trim());
           ht.Add("@UserId", 1);
           ht.Add("@CollegeId",Session["SchoolId"]);
           dt.Clear();
           dt =obj.GetDataTable("Lib_SP_InsUpdtPublisher",ht);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(' Publisher Name Already Exist');", true);
            }
            else
            {
                if (btnSaveAddNew.Text == "Update & AddNew" ||btnSaveGotoList.Text == "Update & GotoList")
                {
                    ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(' Data Updated Successfully');", true);
                   btnSaveAddNew.Text = "Save & AddNew";
                   btnSaveGotoList.Text = "Save & GotoList";
                }
                else
                {
                   ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert(' Data Save Successfully');", true);
                    ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(' Data Updated Successfully');", true);
                }
               Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void Clear()
    {
       txtPublisherName.Text = "";
       txtPublisherPlace.Text="";
       txtRemarks.Text = "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       Clear();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
       Response.Redirect("PublisherList.aspx");
    }
}