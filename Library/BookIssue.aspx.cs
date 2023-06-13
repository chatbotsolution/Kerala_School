using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Library_BookIssue : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           txtAccNo.Enabled = true;
           lblmsg.Text = string.Empty;
           Page.Form.DefaultButton =btnBookDtls.UniqueID;
            if (Page.IsPostBack)
                return;
           Session["title"] = Page.Title.ToString();
            if (Session["User_Id"] != null)
            {
               bindDropDown(ddlClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
               bindDropDown(ddlMemberId, "select MemberId,MemberName+' ( '+EmpNo+' )' as MemberName from Lib_Member where MemberType='2' and  (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') AND CollegeId=" +Session["SchoolId"] + " order by MemberName", "MemberName", "MemberId");
               if (!(Request.QueryString["select"] == "true"))
               {
                   Session["HT"] = null;
                   return;
               }
        
               pnl1.Visible = true;
               pnl2.Visible = false;
               pnl3.Visible = true;
               Hashtable htSess = new Hashtable();
               if (Session["HT"]== null)
                   return;
                htSess = Session["HT"] as Hashtable;
               if (htSess["Type"].ToString() == "0")
               {
                   //bindDropDown(ddlMemberId, "select MemberId,MemberName+' ( '+EmpNo+' )' as MemberName from Lib_Member where MemberType='1' and (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') and CollegeId=" + Session["SchoolId"] + " order by MemberName", "MemberName", "MemberId");
                   ddlClass.Enabled = false;
                   ddlSection.Enabled = false;
                   
               }
               else
               {
                   //bindDropDown(ddlClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
                   ddlClass.Enabled = true;
                   ddlClass.SelectedValue = htSess["Class"].ToString();
                   bindSection();
                   ddlSection.Enabled = true;
                   ddlClass.SelectedValue = htSess["Section"].ToString();
                   BindStudent();

               }
               ddlMemberId.SelectedValue = htSess["Member"].ToString();
               lblCategory.Text =Request.QueryString["catname"].ToString();
               lblSubject.Text =Request.QueryString["subject"].ToString();
               txtAccNo.Text =Request.QueryString["AccNo"].ToString();
               txtAccNo.Enabled = false;
               lblName.Text =Request.QueryString["BookTitle"].ToString();
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
        DataTable dataTableQry =obj.GetDataTableQry(query);
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
            string empty = string.Empty;
           ht = new Hashtable();
           ht.Add("@AccessionNo", txtAccNo.Text.ToString().Trim());
           ht.Add("@CollegeId",Session["SchoolId"]);
            if (obj.ExecuteScalar("Lib_SP_GetStatus",ht) == "R")
            {
               IssueBook();
            }
            else
            {
               lblmsg.Text = "This book is not available to issue !";
               lblmsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void IssueBook()
    {
       ht.Clear();
        if (Request.QueryString["IRId"] == null)
           ht.Add("@IRId", 0);
        else
           ht.Add("@IRId", int.Parse(Request.QueryString["IRId"].ToString()));
       ht.Add("@MemberId", int.Parse(ddlMemberId.SelectedItem.Value));
       ht.Add("@AccessionNo", txtAccNo.Text);
        if (txtIssueDt.Text != "")
           ht.Add("@IssueDate", dtpIssueDt.SelectedDate.ToString());
        if (txtDueDt.Text != "")
           ht.Add("@duedate", dtpDueDt.SelectedDate.ToString());
       ht.Add("@UserId", 1);
       ht.Add("@CollegeId",Session["SchoolId"]);
       dt.Clear();
        string str =obj.ExecuteScalar("Lib_SP_InsUpdtIssue",ht);
        if (str != "")
        {
           lblmsg.Text = str;
           lblmsg.ForeColor = Color.Red;
        }
        else
        {
           lblmsg.Text = "Issue Saved Successfully";
           lblmsg.ForeColor = Color.Green;
           BindGrid(int.Parse(ddlMemberId.SelectedValue));
        }
       Clear();
    }

    private void Clear()
    {
       txtIssueDt.Text = string.Empty;
       txtDueDt.Text = string.Empty;
       ddlMemberId.SelectedIndex = 0;
       ddlClass.SelectedIndex = 0;
       ddlSection.SelectedIndex = 0;
       txtAccNo.Text = string.Empty;
       pnl1.Visible = false;
       pnl2.Visible = true;
       pnl3.Visible = false;
       dvIssue.Visible = false;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       Clear();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
       Response.Redirect("BookIssueList.aspx");
    }

    protected void ddlMemberId_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMemberId.SelectedIndex > 0)
        {
           dvIssue.Visible = true;
           BindGrid(int.Parse(ddlMemberId.SelectedValue));
        }
        else
        {
           grdIssueList.DataSource = null;
           grdIssueList.DataBind();
           grdIssueList.Visible = false;
           dvIssue.Visible = false;
        }
    }

    protected void grdIssueList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
           grdIssueList.PageIndex = e.NewPageIndex;
           BindGrid(int.Parse(ddlMemberId.SelectedValue));
        }
        catch (Exception ex)
        {
        }
    }

    private void BindGrid(int memberid)
    {
       dt =obj.GetDataTableQry("select I.IRId,I.MemberId,I.AccessionNo,convert(varchar,I.IssueDate,105) IssueDate,convert(varchar,I.DueDate,105) DueDate,L.MemberName,L.EmpNo,B.BookTitle from dbo.Lib_IssueReturnMaster I left join dbo.Lib_Member L on I.MemberId=L.MemberId left join dbo.Lib_BookAccession A on A.AccessionNo=I.AccessionNo left join Lib_BookMaster B on B.BookId=A.BookId where ReturnDate is null and I.CollegeId=" +Session["SchoolId"] + " and I.MemberId=" + memberid);
       grdIssueList.DataSource = dt;
       grdIssueList.DataBind();
       grdIssueList.Visible = true;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        Hashtable ht = new Hashtable();
        ht.Add("Type", rdbtnlstUsertype.SelectedValue);
        ht.Add("Class",ddlClass.SelectedValue);
        ht.Add("Section",  ddlSection.SelectedValue);
        ht.Add("Member", ddlMemberId.SelectedValue);
        Session["HT"] = ht;
        Response.Redirect("BookListToIssue.aspx");
    }

    protected void btnBookDtls_Click(object sender, EventArgs e)
    {
       dt = new DataTable();
       ht = new Hashtable();
       ht.Add("@AccessionNo", txtAccNo.Text.ToString().Trim());
       ht.Add("@CollegeId",Session["SchoolId"]);
       dt =obj.GetDataTable("Lib_get_BookDtls",ht);
        if (dt.Rows.Count > 0)
        {
           lblCategory.Text =dt.Rows[0]["catname"].ToString();
           lblSubject.Text =dt.Rows[0]["SubName"].ToString();
           lblName.Text =dt.Rows[0]["BookTitle"].ToString();
           pnl1.Visible = true;
           pnl2.Visible = false;
           pnl3.Visible = true;
        }
        else
        {
           lblmsg.Text = "No records available for this AccessionNo ";
           lblmsg.ForeColor = Color.Red;
           pnl1.Visible = false;
           pnl2.Visible = true;
           pnl3.Visible = false;
        }
    }

    protected void dtpIssueDt_SelectionChanged(object sender, EventArgs e)
    {
        dtpDueDt.From.Date = dtpIssueDt.GetDateValue();
       calculateDueDate();
    }

    private void calculateDueDate()
    {
       obj = new clsDAL();
        if (ddlMemberId.SelectedIndex <= 0)
            return;
       dtpDueDt.SetDateValue(dtpIssueDt.GetDateValue().AddDays((double)int.Parse(obj.ExecuteScalarQry("SELECT AllowedDays FROM dbo.Lib_Member WHERE MemberId=" +ddlMemberId.SelectedValue.ToString()))));
    }

    protected void rdbtnlstUsertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtnlstUsertype.SelectedValue == "0")
        {
           bindDropDown(ddlMemberId, "select MemberId,MemberName+' ( '+EmpNo+' )' as MemberName from Lib_Member where MemberType='1' and (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') and CollegeId=" +Session["SchoolId"] + " order by MemberName", "MemberName", "MemberId");
           ddlClass.Enabled = false;
           ddlSection.Enabled = false;
        }
        else
        {
           bindDropDown(ddlClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
           ddlClass.Enabled = true;
           bindSection();
           ddlSection.Enabled = true;
           BindStudent();
          
        }
    }

    private void BindStudent()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select MemberId,MemberName+' ( '+EmpNo+' )' as MemberName from Lib_Member M");
        stringBuilder.Append(" inner join dbo.PS_ClasswiseStudent C on C.admnno=M.EmpNo");
        stringBuilder.Append(" where MemberType='2' and (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') and M.CollegeId=" +Session["SchoolId"] + " and (detained_promoted='' or Detained_Promoted is null)");
        if (ddlClass.SelectedIndex > 0)
            stringBuilder.Append(" and C.ClassID=" + int.Parse(ddlClass.SelectedValue));
        if (ddlSection.SelectedIndex > 0)
            stringBuilder.Append(" and C.Section='"+ddlSection.SelectedValue+"'");
        stringBuilder.Append(" order by MemberName");
       bindDropDown(ddlMemberId, stringBuilder.ToString(), "MemberName", "MemberId");
    }

    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindSection();
        try
        {
           BindStudent();
        }
        catch (Exception ex)
        {
        }
    }

    private void bindSection()
    {
        ddlSection.SelectedIndex = -1;
        if (Convert.ToInt32(ddlClass.SelectedValue) == 0)
            return;
        int int16 = (int)Convert.ToInt16(new clsDAL().ExecuteScalarQry("Select NoOfSections from PS_ClassMaster where ClassId=" + ddlClass.SelectedValue.ToString().Trim() + ""));
        int num1 = int16;
        if (int16 <= 0)
            return;
        ddlSection.Items.Clear();
        int num2 = 65;
        int index = 0;
        while (num1 > 0)
        {
            ddlSection.Items.Insert(index, new ListItem(Convert.ToChar(num2).ToString(), Convert.ToChar(num2).ToString()));
            --num1;
            ++index;
            ++num2;
        }
        ddlSection.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindStudent();
        }
        catch (Exception ex)
        {
        }
    }
}