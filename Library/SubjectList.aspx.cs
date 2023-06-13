using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_SubjectList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page.IsPostBack)
            return;
       Session["title"] =Page.Title.ToString();
        if (this.Session["User_Id"] != null)
        {
           bindDropDown(this.ddlCategory, "select  BriefName as CatName,CatCode from Lib_Category where CollegeId=" +Session["SchoolId"] + " order by CatName", "CatName", "CatCode");
            if (this.ddlCategory.SelectedIndex > 0)
               FillGrid(int.Parse(this.ddlCategory.SelectedValue));
            else
               FillGrid(0);
        }
        else
           Response.Redirect("../Login.aspx");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry =obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    protected void gidSubjectShow_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
           grdSubjectShow.PageIndex = e.NewPageIndex;
            if (this.ddlCategory.SelectedIndex > 0)
               FillGrid(int.Parse(this.ddlCategory.SelectedValue));
            else
               FillGrid(0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void FillGrid(int CatId)
    {
       ht.Clear();
       ht.Add("@CatCode", CatId);
       ht.Add("@CollegeId",Session["SchoolId"]);
       dt =obj.GetDataTable("Lib_SP_SearchSubjectList",ht);
       grdSubjectShow.DataSource =dt;
       grdSubjectShow.DataBind();
       lblRecords.Text =dt.Rows.Count.ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            if (this.ddlCategory.SelectedIndex > 0)
               FillGrid(int.Parse(this.ddlCategory.SelectedValue));
            else
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
            if (this.Request["Checkb"] == null)
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
            if (this.ddlCategory.SelectedIndex > 0)
               FillGrid(int.Parse(this.ddlCategory.SelectedValue));
            else
               FillGrid(0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DeleteAccession(string id)
    {
       obj.ExcuteQryInsUpdt("delete from Lib_Subjects where SubjectId=" + Convert.ToInt16(id));
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
       Response.Redirect("SubjectMaster.aspx");
    }
}