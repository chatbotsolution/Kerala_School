using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_CategoryMaster : System.Web.UI.Page
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
                txtCatName.Focus();
                if (Page.IsPostBack)
                    return;
                Session["title"] = Page.Title.ToString();
                Form.DefaultButton = btnSaveAddNew.UniqueID;
                if (Request.QueryString["CatCode"] == null)
                    return;
                ht.Clear();
                ht.Add("@CatCode", int.Parse(Request.QueryString["CatCode"].ToString()));
                ht.Add("@CollegeId", Session["SchoolId"]);
                dt.Clear();
                dt = obj.GetDataTable("Lib_SP_GetCategoryList", ht);
                if (dt.Rows.Count <= 0)
                    return;
                txtCatName.Text = dt.Rows[0]["CatName"].ToString();
               // txtCatInShort.Text = dt.Rows[0]["CatInShort"].ToString();
                txtDesc.Text = dt.Rows[0]["CatDesc"].ToString();
                if (dt.Rows[0]["CatPath"].ToString() != "")
                {
                    lblImg.Text = dt.Rows[0]["Original"].ToString();
                    ViewState["CatPath"] = dt.Rows[0]["CatPath"].ToString();
                }
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
        Response.Redirect("CategoryList.aspx");
    }

    private void SaveData()
    {
        try
        {
            ht.Clear();
            if (Request.QueryString["CatCode"] == null)
                ht.Add("@CatCode", 0);
            else if (btnSaveAddNew.Text == "Update & AddNew" || btnSaveGotoList.Text == "Update & GotoList")
                ht.Add("@CatCode", int.Parse(Request.QueryString["CatCode"].ToString()));
            else
                ht.Add("@CatCode", 0);
            ht.Add("@CatName", txtCatName.Text.Trim());
           // ht.Add("@CatInShort", txtCatInShort.Text.Trim());
            ht.Add("@Description", txtDesc.Text.Trim());
            ht.Add("@UserId", 1);
            ht.Add("@CollegeId", Session["SchoolId"]);
            dt.Clear();
            dt = obj.GetDataTable("Lib_SP_InsUpdtCategory", ht);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Category Name Already Exist');", true);
            }
            else
            {
                if (btnSaveAddNew.Text == "Update & AddNew" || btnSaveGotoList.Text == "Update & GotoList")
                {
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Data Updated Successfully');", true);
                    btnSaveAddNew.Text = "Save & AddNew";
                    btnSaveGotoList.Text = "Save & GotoList";
                }
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Data Saved Successfully');", true);
                Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void Clear()
    {
        txtCatName.Text = "";
        txtDesc.Text = "";
        lblImg.Text = "";
        ViewState["CatPath"] = "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Response.Redirect("CategoryList.aspx");
    }
}