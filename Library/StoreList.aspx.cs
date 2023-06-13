using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_StoreList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            Session["title"] = (object)Page.Title.ToString();
            FillGrid(0);
        }
        else
            Response.Redirect("../Login.aspx");
    }

    protected void grdStoreList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdStoreList.PageIndex = e.NewPageIndex;
            FillGrid(0);
        }
        catch (Exception ex)
        {
        }
    }

    private void FillGrid(int StoreId)
    {
        ht.Clear();
        ht.Add((object)"@StoreId", (object)StoreId);
        ht.Add((object)"@CollegeId", Session["SchoolId"]);
        dt = obj.GetDataTable("Lib_SP_GetStoreList", ht);
        grdStoreList.DataSource = (object)dt;
        grdStoreList.DataBind();
        lblRecords.Text = dt.Rows.Count.ToString();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                string str = Request["Checkb"];
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str.Split(chArray))
                    DeleteStore(obj.ToString());
            }
            FillGrid(0);
        }
        catch (Exception ex)
        {
        }
    }

    private void DeleteStore(string id)
    {
        ht.Clear();
        ht.Add((object)"@StoreId", (object)id);
        ht.Add((object)"@CollegeId", Session["SchoolId"]);
        string str = obj.ExecuteScalar("Lib_SP_DeleteStore", ht);
        if (str == "")
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Deleted Successfully');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(" + str + ");", true);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("LibStoreEntry.aspx");
    }
}