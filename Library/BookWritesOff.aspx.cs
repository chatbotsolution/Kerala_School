using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_BookWritesOff : System.Web.UI.Page
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
                if (Page.IsPostBack)
                    return;
                Session["title"] = Page.Title.ToString();
                bindDropDown(ddlCatName, "select  BriefName as CatName,CatCode from Lib_Category where CollegeId=" + Session["SchoolId"] + " order by CatName", "CatName", "CatCode");
                if (Request.QueryString["WriteOffId"] == null)
                    return;
                ht.Clear();
                ht.Add("@WriteOffId", int.Parse(Request.QueryString["WriteOffId"].ToString()));
                ht.Add("@CollegeId", Session["SchoolId"]);
                dt.Clear();
                dt = obj.GetDataTable("Lib_SP_GetWritesOffList", ht);
                if (dt.Rows.Count <= 0)
                    return;
                Fillfield(dt);
            }
            else
                Response.Redirect("../Login.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    private void Fillfield(DataTable dt)
    {
        ddlCatName.SelectedValue = dt.Rows[0]["CatCode"].ToString();
        bindDropDown(ddlSubject, "select SubName,SubjectId from dbo.Lib_Subjects where CatCode=" + ddlCatName.SelectedValue + " and CollegeId=" + Session["SchoolId"] + " order by SubName", "SubName", "SubjectId");
        ddlSubject.SelectedValue = dt.Rows[0]["SubjectId"].ToString();
        bindDropDown(ddlAccessionNo, "select AccessionNo from dbo.Lib_BookMaster where SubjectId=" + ddlSubject.SelectedValue + " and Status !='I' and CollegeId=" + Session["SchoolId"] + " order by AccessionNo", "AccessionNo", "AccessionNo");
        ddlAccessionNo.SelectedValue = dt.Rows[0]["AccessionNo"].ToString();
        dtpWriteOffDt.SetDateValue(Convert.ToDateTime(dt.Rows[0]["WriteOffDate"].ToString()));
        txtDescription.Text = dt.Rows[0]["Description"].ToString();
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

    protected void ddlCatName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCatName.SelectedIndex > 0)
            bindDropDown(ddlSubject, "select SubName,SubjectId from dbo.Lib_Subjects where CatCode=" + ddlCatName.SelectedValue + " and CollegeId=" + Session["SchoolId"] + " order by SubName", "SubName", "SubjectId");
        else
            ddlSubject.Items.Clear();
    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSubject.SelectedIndex > 0)
            bindDropDown(ddlAccessionNo, "select AccessionNo from dbo.Lib_BookAccession A inner join dbo.Lib_BookMaster B on B.BookId=A.BookId where B.SubjectId=" + ddlSubject.SelectedValue + " and A.Status='R' and A.CollegeId=" + Session["SchoolId"] + " order by AccessionNo", "AccessionNo", "AccessionNo");
        else
            ddlAccessionNo.Items.Clear();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            if (Request.QueryString["WriteOffId"] == null)
                ht.Add("@WriteOffId", 0);
            else
                ht.Add("@WriteOffId", int.Parse(Request.QueryString["WriteOffId"].ToString()));
            ht.Add("@AccessionNo", ddlAccessionNo.SelectedValue.Trim());
            ht.Add("@WriteOffDate", dtpWriteOffDt.SelectedDate);
            ht.Add("@status", ddlReason.SelectedValue.ToString());
            ht.Add("@Description", txtDescription.Text.Trim());
            ht.Add("@UserId", 1);
            ht.Add("@CollegeId", Session["SchoolId"]);
            dt.Clear();
            dt = obj.GetDataTable("Lib_SP_InsUpdtWritesOff", ht);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' WritesOff Already Exist to this AccessionNo');", true);
            }
            else
            {
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
        ddlCatName.SelectedIndex = 0;
        ddlSubject.Items.Clear();
        ddlAccessionNo.Items.Clear();
        txtWriteOffDt.Text = "";
        txtDescription.Text = "";
        ddlReason.SelectedIndex = 0;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Response.Redirect("BookWritesOffList.aspx");
    }
}