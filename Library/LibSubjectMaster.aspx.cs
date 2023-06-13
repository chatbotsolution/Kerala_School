using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_LibSubjectMaster : System.Web.UI.Page
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
                ddlCatName.Focus();
                if (Page.IsPostBack)
                    return;
                Session["title"] = Page.Title.ToString();
                Form.DefaultButton = btnSaveAddNew.UniqueID;
                bindDropDown(ddlCatName, "select BriefName as CatName,CatCode from Lib_Category where CollegeId=" + Session["SchoolId"] + " order by CatName", "CatName", "CatCode");
                if (Request.QueryString["SubjectId"] == null)
                    return;
                ht.Clear();
                ht.Add("@SubjectId", int.Parse(Request.QueryString["SubjectId"].ToString()));
                dt.Clear();
                dt = obj.GetDataTable("Lib_SP_GetSubjectList", ht);
                if (dt.Rows.Count <= 0)
                    return;
                ddlCatName.SelectedValue = dt.Rows[0]["CatCode"].ToString();
                txtSubName.Text = dt.Rows[0]["SubName"].ToString();
                txtDesc.Text = dt.Rows[0]["SubDescription"].ToString();
                txtClsNo.Text = dt.Rows[0]["Classi_No"].ToString();
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

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    protected void btnSaveAddNew_Click(object sender, EventArgs e)
    {
        SaveData();
    }

    protected void btnSaveGotoList_Click(object sender, EventArgs e)
    {
        SaveData();
        Response.Redirect("LibSubjectList.aspx");
    }

    private void SaveData()
    {
        try
        {
            ht.Clear();
            if (Request.QueryString["SubjectId"] == null)
                ht.Add("@SubjectId", 0);
            else if (btnSaveAddNew.Text == "Update & AddNew" || btnSaveGotoList.Text == "Update & GotoList")
                ht.Add("@SubjectId", int.Parse(Request.QueryString["SubjectId"].ToString()));
            else
                ht.Add("@SubjectId", 0);
            ht.Add("@CatCode", ddlCatName.SelectedValue.Trim());
            ht.Add("@SubName", txtSubName.Text.Trim());
            ht.Add("@SubDescription", txtDesc.Text.Trim());
            ht.Add("@UserId", 1);
            ht.Add("@CollegeId", Session["SchoolId"]);
            ht.Add("@Classification_No", txtClsNo.Text.Trim());
            dt.Clear();
            dt = obj.GetDataTable("Lib_SP_InsUpdtSubject", ht);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Subject Name Already Exist');", true);
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
            throw ex;
        }
    }

    private void Clear()
    {
        ddlCatName.SelectedIndex = 0;
        txtSubName.Text = "";
        txtDesc.Text = "";
        txtClsNo.Text = "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Response.Redirect("LibSubjectList.aspx");
    }
}