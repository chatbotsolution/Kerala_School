using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_SubjectMaster : System.Web.UI.Page
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
                bindDropDown(ddlCatName, "select  BriefName as CatName,CatCode from Lib_Category where CollegeId=" + Session["SchoolId"] + " order by CatName", "CatName", "CatCode");
                if (Request.QueryString["SubjectId"] == null)
                    return;
                ht.Clear();
                ht.Add("@SubjectId", int.Parse(Request.QueryString["SubjectId"].ToString()));
                ht.Add("@CollegeId", Session["SchoolId"]);
                dt.Clear();
                dt = obj.GetDataTable("Lib_SP_GetSubjectList", ht);
                if (dt.Rows.Count <= 0)
                    return;
                ddlCatName.SelectedValue = dt.Rows[0]["CatCode"].ToString();
                txtSubName.Text = dt.Rows[0]["SubName"].ToString();
                txtDesc.Text = dt.Rows[0]["SubDescription"].ToString();
            }
            else
                Response.Redirect("../Login.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            if (Request.QueryString["SubjectId"] == null)
                ht.Add("@SubjectId", 0);
            else
                ht.Add("@SubjectId", int.Parse(Request.QueryString["SubjectId"].ToString()));
            ht.Add("@CatCode", ddlCatName.SelectedValue.Trim());
            ht.Add("@SubName", txtSubName.Text.Trim());
            ht.Add("@SubDescription", txtDesc.Text.Trim());
            ht.Add("@UserId", 1);
            ht.Add("@CollegeId", Session["SchoolId"]);
            dt.Clear();
            dt = obj.GetDataTable("Lib_SP_InsUpdtSubject", ht);
            if (dt.Rows.Count > 0)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert(' Subject Name Already Exist');", true);
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
        ddlCatName.SelectedIndex = 0;
        txtSubName.Text = "";
        txtDesc.Text = "";
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Response.Redirect("SubjectList.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }
}