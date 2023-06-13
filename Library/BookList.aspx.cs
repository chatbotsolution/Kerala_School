using AjaxControlToolkit;
using ASP;
using SanLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;



public partial class Library_BookList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();
    public static int num =5;
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            if (Session["User_Id"] != null)
            {
                if (Page.IsPostBack)
                    return;
               bindDropDown(ddlCategory, "select  BriefName as CatName,CatCode from Lib_Category where CollegeId=" +Session["SchoolId"] + " order by CatName", "CatName", "CatCode");
               bindDropDown(ddlPublisher, "select PublisherName,PublisherId from Lib_Publisher where CollegeId=" +Session["SchoolId"] + " order by PublisherName", "PublisherName", "PublisherId");
              
               BindGrid(GetQry());
               Session["title"] = Page.Title.ToString();
               Form.DefaultButton =btnSearch.UniqueID;
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
        drp.Items.Insert(0, new ListItem("---All---", "0"));
    }

    private void BindGrid(string qry)
    {
       dt =obj.GetDataTableQry(qry);
       grdBookList.DataSource = dt;
       grdBookList.DataBind();
       foreach (GridViewRow row in grdBookList.Rows)
       {

           TextBox control2 = (TextBox)row.FindControl("TxtBktitle");
           TextBox control12 = (TextBox)row.FindControl("TxtBkSubtitle");
           DropDownList control3 = (DropDownList)row.FindControl("DdlCat");
           bindDropDown(control3, "select  BriefName as CatName,CatCode from Lib_Category where CollegeId=" + Session["SchoolId"] + " order by CatName", "CatName", "CatCode");
           Label control4 = (Label)row.FindControl("hfDdlCatValue");
           DropDownList control16= (DropDownList)row.FindControl("DdlSubject");
           try
           {
               if (int.Parse(control4.Text.ToString()) > 0)
               {
                   bindDropDown(control16, "select  SubName,SubjectId from Lib_Subjects where CollegeId=" + Session["SchoolId"] + " and CatCode=" + control4.Text.ToString() + " order by SubName", "SubName", "SubjectId");
               }
               else
               {
                   ddlSubject.Items.Clear();
                   ddlSubject.Items.Insert(0, new ListItem("---All---", "0"));
               }
           }
           catch (Exception ex)
           {
           }
           Label control17 = (Label)row.FindControl("hfDdlSubject");

           control3.SelectedValue = control4.Text;
           control16.SelectedValue = control17.Text;

           TextBox control5 = (TextBox)row.FindControl("TxtAutNm1");
           TextBox control13 = (TextBox)row.FindControl("AuthorLastName1");
           TextBox control6 = (TextBox)row.FindControl("TxtAutNm2");
           TextBox control14 = (TextBox)row.FindControl("AuthorLastName2");
           TextBox control7 = (TextBox)row.FindControl("TxtAutNm3");
           DropDownList control8 = (DropDownList)row.FindControl("DdlPub");
           bindDropDown(control8, "select PublisherName,PublisherId from Lib_Publisher where CollegeId=" + Session["SchoolId"] + " order by PublisherName", "PublisherName", "PublisherId");
           Label control9 = (Label)row.FindControl("hfDdlPubValue");
           control8.SelectedValue = control9.Text;
           TextBox control10 = (TextBox)row.FindControl("TxtBkNo");
           TextBox control11 = (TextBox)row.FindControl("TxtDimen");

           
           
       }
       
       lblRecords.Text = "Total Record(s): " +dt.Rows.Count.ToString();
    }

    protected string GetQry()
    {
        StringBuilder stringBuilder = new StringBuilder();
        //stringBuilder.Append("select AccessionNo,Dimension,Book_No,Authortype,BookId,BookTitle,AuthorName1,AuthorName2,AuthorName3,Remarks,CatCode,CatName,SubjectId,SubName,PublisherId,PublisherName from dbo.Lib_VW_ListOfBooks where CollegeId=" + Session["SchoolId"]);

        stringBuilder.Append("select distinct * from dbo.Lib_VW_ListOfBooks  where CollegeId=" + Session["SchoolId"]);
        if (ddlCategory.SelectedIndex > 0)
            stringBuilder.Append(" and CatCode='" +ddlCategory.SelectedValue + "'");
        if (ddlSubject.SelectedIndex > 0)
            stringBuilder.Append(" and SubjectId='" +ddlSubject.SelectedValue + "'");
        if (txtAuthor.Text.Trim() != "")
            stringBuilder.Append(" and (AuthorName1+' '+AuthorLastname1)='" + txtAuthor.Text.Trim() + "' or (AuthorName2+' '+AuthorLastname2)='" + txtAuthor.Text.Trim() + "' or AuthorName3='" + txtAuthor.Text.Trim() + "'");
        if (ddlPublisher.SelectedIndex > 0)
            stringBuilder.Append(" and PublisherId='" +ddlPublisher.SelectedValue + "'");
        if (txtBookName.Text.Trim() != "")
            stringBuilder.Append(" and BookTitle like '%" +txtBookName.Text.Trim() + "%'");
        stringBuilder.Append(" order by BookId,BookTitle");
        return stringBuilder.ToString();
    }

    protected string GetQry1()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select AccessionNo,Dimension,Book_No,Authortype,BookId,BookTitle,BookSubTitle,AuthorName1,AuthorName2,AuthorName3,Remarks,CatCode,CatName,SubjectId,SubName,PublisherId,PublisherName from dbo.VW_ListOfAccBooks where CollegeId=" + Session["SchoolId"]);
        if (TxtfrmAcc.Text.Trim() != "")
            stringBuilder.Append(" and AccessionNo between '" + TxtfrmAcc.Text + "' ");
        if (TxttoAcc.Text.Trim() != "")
            stringBuilder.Append(" and '" + TxttoAcc.Text + "' ");
        
        //stringBuilder.Append(" order by CatCode,SubjectId,BookTitle");
        return stringBuilder.ToString();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
       Response.Redirect("NewBookEntry.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string str1 = "";
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                string str2 =Request["Checkb"];
                char[] chArray = new char[1] { ',' };
                foreach (string id in str2.Split(chArray))
                {
                    str1 =DeleteBook(id);
                    if (str1 != "")
                        break;
                }
                if (str1 == "")
                    ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('Deleted Successfully');", true);
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('" + str1 + "');", true);
            }
           BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }

    private string DeleteBook(string id)
    {
       ht.Clear();
       ht.Add("@BookId", int.Parse(id));
        return obj.ExecuteScalar("Lib_SP_DeleteBook",ht);
    }

    protected void grdBookList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
           grdBookList.PageIndex = e.NewPageIndex;
           BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (int.Parse(ddlCategory.SelectedValue) > 0)
            {
                bindDropDown(ddlSubject, "select  SubName,SubjectId from Lib_Subjects where CollegeId=" + Session["SchoolId"] + " and CatCode=" + ddlCategory.SelectedValue + " order by SubName", "SubName", "SubjectId");
            }
            else
            {
                ddlSubject.Items.Clear();
                ddlSubject.Items.Insert(0, new ListItem("---All---", "0"));
            }
        }
        catch (Exception ex)
        {
        }
    }

    

    
    protected void btnSearch_Click(object sender, EventArgs e)
    {
       BindGrid(GetQry());
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
            case 51:
                ht.Add("@Flag", 51);
                break;
            case 52:
                ht.Add("@Flag", 52);
                break;
            case 60:
                ht.Add("@Flag", 60);
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

    protected void BtnEdit_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdBookList.Rows)
        {
            DropDownList dd = (DropDownList)row.FindControl("DdlCat");
            dd.Enabled = true;
            DropDownList d = (DropDownList)row.FindControl("DdlPub");
            d.Enabled = true;

            //TextBox txtAcNo = (TextBox)row.FindControl("TxtAccNo");
            //txtAcNo.Enabled = true;

            TextBox txtbx = (TextBox)row.FindControl("TxtBktitle");
            txtbx.Enabled = true;
            TextBox txtb = (TextBox)row.FindControl("TxtAutNm1");
            txtb.Enabled = true;
            TextBox txtbl = (TextBox)row.FindControl("txtLName1");
            txtbl.Enabled = true;

            TextBox A2 = (TextBox)row.FindControl("TxtAutNm2");
            A2.Enabled = true;
            TextBox A3 = (TextBox)row.FindControl("TxtAutNm3");
            A3.Enabled = true;

            TextBox txt = (TextBox)row.FindControl("TxtBkNo");
            txt.Enabled = true;
            TextBox tx = (TextBox)row.FindControl("TxtDimen");
            tx.Enabled = true;
            
            //foreach (Control c in grdBookList.Controls)
            //{
            //    if ((c.GetType() == typeof(DropDownList)))
            //    {
            //        ((DropDownList)(c)).Enabled = true;
            //    }

            //    if ((c.GetType() == typeof(TextBox)))
            //    {
            //        ((TextBox)(c)).Enabled = true;
            //    }
            //}
        }
    }

    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdBookList.Rows)
        {
            ht.Clear();
            DropDownList DdlCat = (DropDownList)row.FindControl("DdlCat");
            DropDownList DdlPub = (DropDownList)row.FindControl("DdlPub");
            //TextBox TxtAccNo = (TextBox)row.FindControl("TxtAccNo");
            TextBox TxtBktitle = (TextBox)row.FindControl("TxtBktitle");
            TextBox TxtBkSubtitle = (TextBox)row.FindControl("TxtBkSubtitle");
            TextBox TxtAutNm1 = (TextBox)row.FindControl("TxtAutNm1");
            TextBox txtLName1 = (TextBox)row.FindControl("txtLName1");
            TextBox TxtAutNm2 = (TextBox)row.FindControl("TxtAutNm2");
            TextBox txtLName2 = (TextBox)row.FindControl("txtLName2");
            TextBox TxtAutNm3 = (TextBox)row.FindControl("TxtAutNm3");
            TextBox TxtBkNo = (TextBox)row.FindControl("TxtBkNo");
            TextBox TxtDimen = (TextBox)row.FindControl("TxtDimen");

            Label LblBkid = (Label)row.FindControl("LblBkid");
            Label Label1 = (Label)row.FindControl("Label1");
            Label Label2 = (Label)row.FindControl("Label2");
            Label Label3 = (Label)row.FindControl("Label3");


            ht.Add("@CatCode", DdlCat.SelectedValue);
            ht.Add("@BookId", LblBkid.Text.Trim());

            //ht.Add("@AccessionNo", TxtAccNo.Text.Trim());
            ht.Add("@BookTitle", TxtBktitle.Text.Trim());
            ht.Add("@BookSubTitle", TxtBkSubtitle.Text.Trim());
            ht.Add("@SubjectId", Label1.Text.Trim());
            ht.Add("@AuthorName1", TxtAutNm1.Text.Trim());
            ht.Add("@AuthorLastName1", txtLName1.Text.Trim());
            ht.Add("@AuthorName2", TxtAutNm2.Text.Trim());
            ht.Add("@AuthorLastName2", txtLName2.Text.Trim());
            ht.Add("@AuthorName3", TxtAutNm3.Text.Trim());
            ht.Add("@PublisherId", DdlPub.SelectedValue);
            ht.Add("@Remarks", Label2.Text.Trim());
            ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
            ht.Add("@CollegeId", int.Parse(Session["SchoolId"].ToString()));
            ht.Add("@AuthorType", Label3.Text.Trim());
            ht.Add("@Dimension", TxtDimen.Text.Trim());
            ht.Add("@BookNo", TxtBkNo.Text.Trim());
            dt.Clear();
            dt = obj.GetDataTable("Lib_SP_InsUpdtBook", ht);
        }

        foreach (GridViewRow row in grdBookList.Rows)
        {
            DropDownList dd = (DropDownList)row.FindControl("DdlCat");
            dd.Enabled = false;
            DropDownList d = (DropDownList)row.FindControl("DdlPub");
            d.Enabled = false;

            TextBox txtbx = (TextBox)row.FindControl("TxtBktitle");
            txtbx.Enabled = false;
            TextBox TxtBkSubtitle = (TextBox)row.FindControl("TxtBkSubtitle");
            TxtBkSubtitle.Enabled = false;
            TextBox txtb = (TextBox)row.FindControl("TxtAutNm1");
            txtb.Enabled = false;

            TextBox A2 = (TextBox)row.FindControl("TxtAutNm2");
            A2.Enabled = false;
            TextBox A3 = (TextBox)row.FindControl("TxtAutNm3");
            A3.Enabled = false;

            TextBox txt = (TextBox)row.FindControl("TxtBkNo");
            txt.Enabled = false;
            TextBox tx = (TextBox)row.FindControl("TxtDimen");
            tx.Enabled = false;
        }

    }


    protected void ButnSearch_Click(object sender, EventArgs e)
    {
        BindGrid(GetQry1());
    }
   
   


    
    protected void cbBkTitle_CheckedChanged(object sender, EventArgs e)
    {
        

        if (cbBkTitle.Checked)
        {
            
            num++;
            if (CalculateCount(cbBkTitle) == false) { num--; return; };
            grdBookList.Columns[2].Visible = true;
        }
        else
        {
            grdBookList.Columns[2].Visible = false;
            num--;
        }

    }
    protected void cbBkSubTitle_CheckedChanged(object sender, EventArgs e)
    {
        
        if (cbBkSubTitle.Checked)
        {
          
            num++;
            if (CalculateCount(cbBkSubTitle) == false) { num--; return; }
            grdBookList.Columns[3].Visible = true;
        }
        else
        {
            grdBookList.Columns[3].Visible = false;
            num--;
        }
    }
    protected void cbCat_CheckedChanged(object sender, EventArgs e)
    {
        
        if (cbCat.Checked)
        {
           
            num++;
            if (CalculateCount(cbCat) == false) { num--; return; }
            grdBookList.Columns[4].Visible = true;
        }
        else
        {
            grdBookList.Columns[4].Visible = false;
            num--;
        }
    }
    protected void cbSubject_CheckedChanged(object sender, EventArgs e)
    {

        if (cbSubject.Checked)
        {

            num++;
            if (CalculateCount(cbSubject) == false) { num--; return; }
            grdBookList.Columns[5].Visible = true;
        }
        else
        {
            grdBookList.Columns[5].Visible = false;
            num--;
        }
    }
    protected void cbAuthNm1_CheckedChanged(object sender, EventArgs e)
    {
        
        if (cbAuthNm1.Checked)
        {
            
            num++;
            if (CalculateCount(cbAuthNm1) == false) { num--; return; }
            grdBookList.Columns[6].Visible = true;
        }
        else
        {
            grdBookList.Columns[6].Visible = false;
            num--;
        }

    }
    protected void cbAuthNm2_CheckedChanged(object sender, EventArgs e)
    {
        
        if (cbAuthNm2.Checked)
        {
           
            num++;
            if (CalculateCount(cbAuthNm2) == false) { num--; return; }
            grdBookList.Columns[7].Visible = true;
        }
        else
        {
            grdBookList.Columns[7].Visible = false;
            num--;
        }
    }
    protected void cbAuthNm3_CheckedChanged(object sender, EventArgs e)
    {
        
        if (cbAuthNm3.Checked)
        {
           
            num++;
            if (CalculateCount(cbAuthNm3) == false) { num--; return; }
            grdBookList.Columns[8].Visible = true;
        }
        else
        {
            grdBookList.Columns[8].Visible = false;
            num--;
        }
    }
    protected void cbPublishNm_CheckedChanged(object sender, EventArgs e)
    {
      
        if (cbPublishNm.Checked)
        {
           
            num++;
            if (CalculateCount(cbPublishNm) == false) { num--; return; }
            grdBookList.Columns[9].Visible = true;
        }
        else
        {
            grdBookList.Columns[9].Visible = false;
            num--;
        }
    }
    protected void cbBk_No_CheckedChanged(object sender, EventArgs e)
    {
        
        if (cbBk_No.Checked)
        {
           
            num++;
            if (CalculateCount(cbBk_No) == false) { num--; return; }
            grdBookList.Columns[10].Visible = true;
        }
        else
        {
            grdBookList.Columns[10].Visible = false;
            num--;
        }
    }
    protected void cbDimen_CheckedChanged(object sender, EventArgs e)
    {
        
        if (cbDimen.Checked)
        {
            
            num++;
            if (CalculateCount(cbDimen) == false) { num--; return; }
            grdBookList.Columns[11].Visible = true;
        }
        else
        {
            grdBookList.Columns[11].Visible = false;
            num--;
        }
    }

    protected bool CalculateCount(CheckBox ck)
    {
        //var checkedBoxes = 0;
        //// Iterate through all of the Controls in your Form
        //foreach (Control c in Form.Controls)
        //{
        //    string id = c.ID;
        //    // If one of the Controls is a CheckBox and it is checked, then
        //    // increment your count
        //    if (c.GetType() == typeof(CheckBox))
        //    {

        //        checkedBoxes++;
        //        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + checkedBoxes + "');", true);

        //    }
        //}

        if (num > 6)
        {
            // ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Only Six to be Checked')", true);
            ck.Checked = false;
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Only Six to be Checked');", true);
            return false;
        }
        else
            return true;
 
       
    }
}