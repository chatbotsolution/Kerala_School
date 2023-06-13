using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_PublisherList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
       Session["title"] = Page.Title.ToString();
        if (Session["User_Id"] != null)
           FillGrid(0);
        else
           Response.Redirect("../Login.aspx");
    }

    private void FillGrid(int PublisherId)
    {
       ht.Clear();
       ht.Add("@PublisherId", PublisherId);
       ht.Add("@CollegeId",Session["SchoolId"]);
       dt =obj.GetDataTable("Lib_SP_GetPublisherList",ht);
       grdPublisherList.DataSource = dt;
       grdPublisherList.DataBind();
       lblRecords.Text =dt.Rows.Count.ToString();
    }

    protected void grdPublisherList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
           grdPublisherList.PageIndex = e.NewPageIndex;
           FillGrid(0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
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
       ht.Add("@PublisherId", id);
       ht.Add("@CollegeId",Session["SchoolId"]);
        string str =obj.ExecuteScalar("Lib_SP_DeletePublisher",ht);
        if (str == "")
            ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('Deleted Successfully');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(" + str + ");", true);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
       Response.Redirect("PublisherMaster.aspx");
    }
}