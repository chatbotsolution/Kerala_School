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

public partial class Library_BookListToIssue : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Session["title"] = (object)Page.Title.ToString();
        if (Session["User_Id"] != null)
        {
            bindDropDown(ddlCategory, "select  BriefName as CatName,CatCode from Lib_Category where CollegeId=" + Session["SchoolId"] + " order by CatName", "CatName", "CatCode");
            bindDropDown(ddlPublisher, "select PublisherName,PublisherId from Lib_Publisher where CollegeId=" + Session["SchoolId"] + " order by PublisherName", "PublisherName", "PublisherId");
            BindGrid(GetQry());
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = (object)null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = (object)dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    private void BindGrid(string qry)
    {
        dt = obj.GetDataTableQry(qry);
        grdCatList.DataSource = (object)dt;
        grdCatList.DataBind();
        lblRecords.Text = "Total Record(s): " + dt.Rows.Count.ToString();
    }

    protected string GetQry()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select BookId,AccessionNo,BookTitle,AuthorName1,AuthorName2,AuthorName3,Edition,PubPlace,PubYear,Pages,Vol,ReceivedFrom,BillNo,BillDate,Status,PurchaseDate,PurchasePrice,ISBN,Remarks,CatCode,CatName,SubjectId,SubName,PublisherId,PublisherName from dbo.Lib_VW_BookList where Status='R' and CollegeId=" + Session["SchoolId"]);
        if (ddlCategory.SelectedIndex > 0)
            stringBuilder.Append(" and CatCode='" + ddlCategory.SelectedValue + "'");
        if (ddlSubject.SelectedIndex > 0)
            stringBuilder.Append(" and SubjectId='" + ddlSubject.SelectedValue + "'");
        if (txtAuthor.Text.Trim() != "")
            stringBuilder.Append(" and AuthorName1='" + txtAuthor.Text.Trim() + "' or AuthorName2='" + txtAuthor.Text.Trim() + "' or AuthorName3='" + txtAuthor.Text.Trim() + "'");
        if (ddlPublisher.SelectedIndex > 0)
            stringBuilder.Append(" and PublisherId='" + ddlPublisher.SelectedValue + "'");
        return stringBuilder.ToString();
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (int.Parse(ddlCategory.SelectedValue) > 0)
            bindDropDown(ddlSubject, "select SubName,SubjectId from Lib_Subjects where CatCode=" + ddlCategory.SelectedValue + " and CollegeId=" + Session["SchoolId"] + " order by SubName", "SubName", "SubjectId");
        else
            ddlSubject.Items.Clear();
    }

    protected void grdCatList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdCatList.PageIndex = e.NewPageIndex;
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid(GetQry());
    }
}