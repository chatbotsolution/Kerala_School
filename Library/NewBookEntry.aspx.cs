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
using System.Collections.Generic;
using System.Collections;

public partial class Library_NewBookEntry : System.Web.UI.Page
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
               Form.DefaultButton =btnSaveAddNew.UniqueID;
               bindDropDown(ddlBookCat, "select  BriefName as CatName,CatCode from Lib_Category where CollegeId=" +Session["SchoolId"] + " order by CatName", "CatName", "CatCode");
               bindDropDown(ddlPublisher, "select  PublisherName ,PublisherId from Lib_Publisher where CollegeId=" +Session["SchoolId"] + " order by PublisherName", "PublisherName", "PublisherId");
                if (Request.QueryString["BookId"] == null)
                    return;
               ht.Clear();
               ht.Add("@BookId", int.Parse(Request.QueryString["BookId"].ToString()));
               ht.Add("@CollegeId",Session["SchoolId"]);
               dt.Clear();
               dt =obj.GetDataTable("Lib_SP_GetBookList",ht);
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
        DataTable dataTableQry =obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    private void fillField(DataTable dt)
    {
       ddlBookCat.SelectedValue = dt.Rows[0]["CatCode"].ToString();
       bindDropDown(ddlSubject, "select  SubName ,SubjectId from Lib_Subjects where CollegeId=" +Session["SchoolId"] + " AND CatCode=" +ddlBookCat.SelectedValue + " order by SubName", "SubName", "SubjectId");
       ddlSubject.SelectedValue = dt.Rows[0]["SubjectId"].ToString();
       txtTitle.Text = dt.Rows[0]["BookTitle"].ToString();
       txtAuthor1.Text = dt.Rows[0]["AuthorName1"].ToString();
       txtLName1.Text = dt.Rows[0]["AuthorLastName1"].ToString();
       txtAuthor2.Text = dt.Rows[0]["AuthorName2"].ToString();
       txtLName2.Text = dt.Rows[0]["AuthorLastName2"].ToString();
       txtAuthor3.Text = dt.Rows[0]["AuthorName3"].ToString();
       ddlPublisher.SelectedValue = dt.Rows[0]["PublisherId"].ToString();
       txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
       txtsubtitle.Text = dt.Rows[0]["BookSubTitle"].ToString();

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
           


        try
        {
           ht.Clear();
            if (Request.QueryString["BookId"] == null)
               ht.Add("@BookId", 0);
            else if (btnSaveAddNew.Text == "Update & AddNew" ||btnSaveGotoList.Text == "Update & GotoList")
               ht.Add("@BookId", int.Parse(Request.QueryString["BookId"].ToString()));
            else
               ht.Add("@BookId", 0);
           ht.Add("@CatCode", ddlBookCat.SelectedValue);
           ht.Add("@SubjectId", ddlSubject.SelectedValue);
           ht.Add("@BookTitle", txtTitle.Text.Trim());
           ht.Add("@BookSubTitle", txtsubtitle.Text.Trim());
           ht.Add("@AuthorName1", txtAuthor1.Text.Trim());
           ht.Add("@AuthorLastName1", txtLName1.Text.Trim());
           ht.Add("@AuthorName2", txtAuthor2.Text.Trim());
           ht.Add("@AuthorLastName2", txtLName2.Text.Trim());
           if(txtAuthor3.Text.Trim() !="")
               ht.Add("@AuthorName3", txtAuthor3.Text.Trim()+" "+ txtLName3.Text );
           else
               ht.Add("@AuthorName3",txtLName3.Text);
          
           ht.Add("@PublisherId", ddlPublisher.SelectedValue);
           ht.Add("@Remarks", txtRemarks.Text.Trim());
           ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
           ht.Add("@CollegeId", int.Parse(Session["SchoolId"].ToString()));
           ht.Add("@AuthorType", ddlAuthortp.SelectedValue);
           ht.Add("@Dimension", Txtdimension.Text.Trim());
           ht.Add("@BookNo", TxtBookno.Text.Trim());
           dt.Clear();
           dt =obj.GetDataTable("Lib_SP_InsUpdtBook",ht);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('" +dt.Rows[0][0].ToString().Trim() + "');", true);
            }
            else
            {
                if (btnSaveAddNew.Text == "Update & AddNew" ||btnSaveGotoList.Text == "Update & GotoList")
                    ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(' Data Updated Successfully');", true);
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(' Data Saved Successfully');", true);
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
       txtsubtitle.Text = "";
       TxtBookno.Text = "";
       txtTitle.Text = "";
       txtAuthor1.Text = "";
       txtLName1.Text = "";
       txtAuthor2.Text = "";
       txtLName2.Text = "";
       txtAuthor3.Text = "";
       txtLName3.Text = "";
       TxtBookno.Text = "";
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
           bindDropDown(ddlSubject, "select  SubName ,SubjectId from Lib_Subjects where CollegeId=" +Session["SchoolId"] + " AND CatCode=" +ddlBookCat.SelectedValue + " order by SubName", "SubName", "SubjectId");
        }
        catch (Exception ex)
        {
        }
    }
   
    protected void ddlAuthortp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlAuthortp.SelectedValue == "Publisher")
        {
            if (ddlPublisher.SelectedIndex > 0)
            {
                txtAuthor1.Text = txtAuthor2.Text = txtAuthor3.Text = txtLName1.Text = txtLName2.Text = "";
                txtAuthor3.Enabled = false;
                txtLName3.Text = ddlPublisher.SelectedItem.Text;
                TxtBookno.Text = (txtLName3.Text).Substring(0, 3).ToUpper();
            }
            else
            {
                txtLName3.Text = "";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Select Publisher first')", true);
            }
            
        }
        else
        {
            txtLName3.Text = "";
            txtAuthor3.Enabled = true;
        }
       
    }
    protected void txtLName1_TextChanged(object sender, EventArgs e)
    {
        GenBookNo(txtAuthor1, txtLName1);
        }
    protected void txtLName2_TextChanged(object sender, EventArgs e)
    {
        if (txtLName1.Text == "")
        {
            GenBookNo(txtAuthor2, txtLName2);
        }
    }
    protected void txtLName3_TextChanged(object sender, EventArgs e)
    {
        if (txtLName1.Text == "" && txtLName2.Text == "")
        {

            GenBookNo(txtAuthor3,txtLName3);
            
        }
    }

    private void GenBookNo(TextBox txtAuthor, TextBox txtLName)
    {
        if (txtLName.Text == "")
        {
            if (txtAuthor.Text == "")
                return;
            else if (txtAuthor.Text.Length <= 3)
            {
                TxtBookno.Text = txtAuthor.Text.ToUpper();
            }
            else
            {
                TxtBookno.Text = (txtAuthor.Text).Substring(0, 3).ToUpper();
            }
        }
        else if (txtLName.Text.Length <= 3)
        {
            TxtBookno.Text = txtLName.Text.ToUpper();
        }
        else
        {
            TxtBookno.Text = (txtLName.Text).Substring(0, 3).ToUpper();
        }
    }

    protected void btnSaveClose_Click(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            if (Request.QueryString["PublisherId"] == null)
                ht.Add("@PublisherId", 0);
            else if (btnSaveAddNew.Text == "Update & AddNew" || btnSaveGotoList.Text == "Update & GotoList")
                ht.Add("@PublisherId", int.Parse(Request.QueryString["PublisherId"].ToString()));
            else
                ht.Add("@PublisherId", 0);
            ht.Add("@PublisherName", txtPublisherName.Text.Trim());
            ht.Add("@PublisherPlace", txtPublisherPlace.Text.Trim());
            ht.Add("@Remarks", txtRemarks.Text.Trim());
            ht.Add("@UserId", 1);
            ht.Add("@CollegeId", Session["SchoolId"]);
            dt.Clear();
            dt = obj.GetDataTable("Lib_SP_InsUpdtPublisher", ht);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Publisher Name Already Exist');", true);
            }
            else
            {
               
                    ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert(' Data Save Successfully');", true);

                    Clear1();
            }
          
            mp1.Hide();
            bindDropDown(ddlPublisher, "select  PublisherName ,PublisherId from Lib_Publisher where CollegeId=" + Session["SchoolId"] + " order by PublisherName", "PublisherName", "PublisherId");
        }
        catch (Exception ex)
        {
        }
    }

    private void Clear1()
    {
        txtPublisherName.Text = "";
        txtPublisherPlace.Text = "";
        txtRemarks.Text = "";
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> AutoCompleteLib(string prefixText, int count, string contextKey)
    {

        int id = Convert.ToInt32(contextKey);

        List<string> newlist = new List<string>();
        Hashtable ht = new Hashtable();
        ht.Add("@Term", prefixText);
        switch (id)
        {
            case 53:
                ht.Add("@Flag", 53);
                break;
            case 52:
                ht.Add("@Flag", 52);
                break;
            default:
                ht.Add("@Flag", 0);
                break;

        }
        // ht.Add("@Flag", 1);
        DataTable dt = new clsDAL().GetDataTable("AutoComplete", ht);
        foreach (DataRow row in dt.Rows)
        {
            newlist.Add(row["Result"].ToString());
        }
        return newlist;
    }


    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear1();
    }
}
