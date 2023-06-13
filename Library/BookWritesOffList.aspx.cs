using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_BookWritesOffList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Session["title"] = Page.Title.ToString();
        Form.DefaultButton = btnSearch.UniqueID;
        if (Session["User_Id"] != null)
        {
            bindDropDown(ddlCategory, "select  BriefName as CatName,CatCode from Lib_Category where CollegeId=" + Session["SchoolId"] + " order by CatName", "CatName", "CatCode");
            BindGrid(GetQry());
        }
        else
            Response.Redirect("../Login.aspx");
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
        drp.Items.Insert(0, new ListItem("---All---", "0"));
    }

    private void BindGrid(string qry)
    {
        dt = obj.GetDataTableQry(qry);
        grdWriteOffList.DataSource = dt;
        grdWriteOffList.DataBind();
        lblRecords.Text = "Total Record(s): " + dt.Rows.Count.ToString();
    }

    protected string GetQry()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select WriteOffId,AccessionNo,BookTitle,WriteOffDate,Description from dbo.Lib_VW_BookWriteOffList where CollegeId=" + Session["SchoolId"]);
        if (ddlCategory.SelectedIndex > 0)
            stringBuilder.Append(" and CatCode='" + ddlCategory.SelectedValue + "'");
        if (ddlSubject.SelectedIndex > 0)
            stringBuilder.Append(" and SubjectId='" + ddlSubject.SelectedValue + "'");
        if (ddlReason.SelectedIndex > 0)
            stringBuilder.Append(" and Status='" + ddlReason.SelectedValue + "'");
        return stringBuilder.ToString();
    }

    protected void grdWriteOffList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdWriteOffList.PageIndex = e.NewPageIndex;
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
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
                    DeleteAccession(obj.ToString());
            }
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DeleteAccession(string id)
    {
        ht.Clear();
        ht.Add("@WriteOffId", id);
        ht.Add("@CollegeId", Session["SchoolId"]);
        string str = obj.ExecuteScalar("Lib_SP_DeleteWriteOff", ht);
        if (str == "")
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Deleted Successfully');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(" + str + ");", true);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("BookWritesOff.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid(GetQry());
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCategory.SelectedIndex > 0)
        {
            bindDropDown(ddlSubject, "select SubName,SubjectId from Lib_Subjects where CatCode=" + ddlCategory.SelectedValue + " and CollegeId=" + Session["SchoolId"] + " order by SubName", "SubName", "SubjectId");
        }
        else
        {
            ddlSubject.Items.Clear();
            ddlSubject.Items.Insert(0, new ListItem("---All---", "0"));
        }
    }
}