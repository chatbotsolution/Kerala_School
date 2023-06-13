using AjaxControlToolkit;
using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_BookAccUpdate : System.Web.UI.Page
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
                ddlBookCat.Focus();
                if (Page.IsPostBack)
                    return;
                Session["title"] = Page.Title.ToString();
                Form.DefaultButton = btnSaveAddNew.UniqueID;

                bindDropDown(ddlBookAcc, "select  AccessionNo as Accession,AccessionNo from Lib_BookAccession where CollegeId=" + Session["SchoolId"] + " order by AccessionNo", "AccessionNo", "AccessionNo");

                bindDropDown(ddlBookCat, "select  BriefName as CatName,CatCode from Lib_Category where CollegeId=" + Session["SchoolId"] + " order by CatName", "CatName", "CatCode");
                bindDropDown(ddlPublisher, "select  PublisherName ,PublisherId from Lib_Publisher where CollegeId=" + Session["SchoolId"] + " order by PublisherName", "PublisherName", "PublisherId");
                if (Request.QueryString["BookId"] == null)
                    return;
                ht.Clear();
                ht.Add("@BookId", int.Parse(Request.QueryString["BookId"].ToString()));
                ht.Add("@CollegeId", Session["SchoolId"]);
                dt.Clear();
                dt = obj.GetDataTable("Lib_SP_GetBookList", ht);
                if (dt.Rows.Count > 0)
                {
                    fillField(dt);
                    btnSaveAddNew.Text = "Update & AddNew";
                    btnSaveGotoList.Text = "Update & GotoList";
                }
                else
                    Response.Redirect("BookList.aspx");
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

    private void fillField(DataTable dt)
    {
        ddlBookCat.SelectedValue = dt.Rows[0]["CatCode"].ToString();
        bindDropDown(ddlSubject, "select  SubName ,SubjectId from Lib_Subjects where CollegeId=" + Session["SchoolId"] + " AND CatCode=" + ddlBookCat.SelectedValue + " order by SubName", "SubName", "SubjectId");
        ddlSubject.SelectedValue = dt.Rows[0]["SubjectId"].ToString();
        txtTitle.Text = dt.Rows[0]["BookTitle"].ToString();
        txtAuthor1.Text = dt.Rows[0]["AuthorName1"].ToString();
        txtAuthor2.Text = dt.Rows[0]["AuthorName2"].ToString();
        txtAuthor3.Text = dt.Rows[0]["AuthorName3"].ToString();
        ddlPublisher.SelectedValue = dt.Rows[0]["PublisherId"].ToString();
        txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();

        ddlAuthortp.SelectedValue = dt.Rows[0]["Authortype"].ToString();
        Txtdimension.Text = dt.Rows[0]["Dimension"].ToString();
        TxtBookno.Text = dt.Rows[0]["Book_No"].ToString();
    }

    protected void btnSaveAddNew_Click(object sender, EventArgs e)
    {
        SaveData();
    }

    protected void btnSaveGotoList_Click(object sender, EventArgs e)
    {
        SaveData();
        Response.Redirect("BookList.aspx");
    }

    private void SaveData()
    {
        //if (txtAuthor1.Text.Trim() == "")
        //{

        //    if (ddlAuthortp.SelectedValue == "Publisher")
        //    {
        //        txtAuthor1.Text = ddlPublisher.SelectedValue;
        //    }

        //}
        //else
        //{
        //    TxtBookno.Text = (txtAuthor1.Text).Substring(0, 3);
        //}





        try
        {
            ht.Clear();
            if (Request.QueryString["BookId"] == null)
                ht.Add("@BookId", 0);
            else if (btnSaveAddNew.Text == "Update & AddNew" || btnSaveGotoList.Text == "Update & GotoList")
                ht.Add("@BookId", int.Parse(Request.QueryString["BookId"].ToString()));
            else
                ht.Add("@BookId", 0);

            ht.Add("@AccessionNo", ddlBookAcc.Text.Trim());

            ht.Add("@CatCode", ddlBookCat.SelectedValue);
            ht.Add("@SubjectId", ddlSubject.SelectedValue);
            ht.Add("@BookTitle", txtTitle.Text.Trim());
            ht.Add("@AuthorName1", txtAuthor1.Text.Trim());
            ht.Add("@AuthorName2", txtAuthor2.Text.Trim());
            ht.Add("@AuthorName3", txtAuthor3.Text.Trim());
            ht.Add("@PublisherId", ddlPublisher.SelectedValue);
            ht.Add("@Remarks", txtRemarks.Text.Trim());
            ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
            ht.Add("@CollegeId", int.Parse(Session["SchoolId"].ToString()));
            ht.Add("@AuthorType", ddlAuthortp.SelectedValue);
            ht.Add("@Dimension", Txtdimension.Text.Trim());
            ht.Add("@BookNo", TxtBookno.Text.Trim());
            dt.Clear();
            dt = obj.GetDataTable("Lib_SP_InsUpdtAccBook", ht);

           
            //Update Lib_BookAccession  set BookId=@BookId where AccessionNo=ddlBookAcc.selectedindex

            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + dt.Rows[0][0].ToString().Trim() + "');", true);
            }
            else
            {
                if (btnSaveAddNew.Text == "Update & AddNew" || btnSaveGotoList.Text == "Update & GotoList")
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Data Updated Successfully');", true);
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Data Saved Successfully');", true);
                Clear();
            }
            if (Request.QueryString["FrmPurchase"] == null)
                return;
            Response.Redirect("BookPurchase.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["FrmPurchase"] != null)
            Response.Redirect("BookPurchase.aspx");
        Clear();
    }

    private void Clear()
    {
        ddlBookCat.SelectedValue = "0";
        ddlSubject.SelectedValue = "0";
        txtTitle.Text = "";
        txtAuthor1.Text = "";
        txtAuthor2.Text = "";
        txtAuthor3.Text = "";
        ddlPublisher.SelectedValue = "0";
        txtRemarks.Text = "";
        btnSaveAddNew.Text = "Save & AddNew";
        btnSaveGotoList.Text = "Save & GotoList";
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["FrmPurchase"] != null)
            Response.Redirect("BookPurchase.aspx");
        Response.Redirect("BookList.aspx");
    }

    protected void ddlBookCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlBookCat.SelectedIndex <= 0)
                return;
            bindDropDown(ddlSubject, "select  SubName ,SubjectId from Lib_Subjects where CollegeId=" + Session["SchoolId"] + " AND CatCode=" + ddlBookCat.SelectedValue + " order by SubName", "SubName", "SubjectId");
        }
        catch (Exception ex)
        {
        }
    }
    protected void BtnAddPublisher_Click(object sender, EventArgs e)
    {
        Response.Redirect("PublisherMaster.aspx");
    }
    protected void ddlAuthortp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAuthortp.SelectedValue == "Publisher")
        {
            if (ddlPublisher.SelectedIndex > 0)
            {
                txtAuthor1.Text = ddlPublisher.SelectedItem.Text;
            }
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Publisher first')", true);
        }
    }
    protected void txtAuthor1_TextChanged(object sender, EventArgs e)
    {
        if (txtAuthor1.Text == "")
            return;
        else if (txtAuthor1.Text.Length <= 3)
        {
            TxtBookno.Text = txtAuthor1.Text;
        }
        else
        {
            TxtBookno.Text = (txtAuthor1.Text).Substring(0, 3);
        }
    }
    protected void ddlBookAcc_SelectedIndexChanged(object sender, EventArgs e)
    {
        ht.Add("@AccessionNo", ddlBookAcc.Text.Trim());
        ht.Add("@CollegeId", int.Parse(Session["SchoolId"].ToString()));

        dt.Clear();

        dt = obj.GetDataTable("Lib_SP_GetAccBookList", ht);
         if (dt.Rows.Count > 0)
         {

             ddlBookCat.SelectedValue = dt.Rows[0]["CatCode"].ToString();
            bindDropDown(ddlSubject, "select  SubName ,SubjectId from Lib_Subjects where CollegeId=" + Session["SchoolId"] + " AND CatCode=" + ddlBookCat.SelectedValue + " order by SubName", "SubName", "SubjectId");
             ddlSubject.SelectedValue = dt.Rows[0]["SubjectId"].ToString();
             txtTitle.Text = dt.Rows[0]["BookTitle"].ToString();
             txtAuthor1.Text = dt.Rows[0]["AuthorName1"].ToString();
             txtAuthor2.Text = dt.Rows[0]["AuthorName2"].ToString();
             txtAuthor3.Text = dt.Rows[0]["AuthorName3"].ToString();
             ddlPublisher.SelectedValue = dt.Rows[0]["PublisherId"].ToString();
             txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
             ddlAuthortp.SelectedValue = dt.Rows[0]["Authortype"].ToString();
             Txtdimension.Text = dt.Rows[0]["Dimension"].ToString();
             TxtBookno.Text = dt.Rows[0]["Book_No"].ToString();

         }

    }
}