using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Library_IssuePendingList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            Session["title"] = Page.Title.ToString();
            Form.DefaultButton = btnSearch.UniqueID;
            bindDropDown(ddlClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
            bindDropDown(ddlMemberId, "select MemberId,MemberName+' ( '+EmpNo+' )' as MemberName from Lib_Member where MemberType='2' and (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') AND CollegeId=" + Session["SchoolId"] + " order by MemberName", "MemberName", "MemberId");
            BindGrid(GetQry());
            dvIrlist.Visible = true;
        }
        else
        {
            dvIrlist.Visible = false;
            Response.Redirect("../Login.aspx");
        }
    }

    private void BindGrid(string qry)
    {
       dt =obj.GetDataTableQry(qry);
       grdIssueList.DataSource = dt;
       grdIssueList.DataBind();
       lblRecords.Text = "Total Record(s): " +dt.Rows.Count.ToString();
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

    private string GetQry()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select I.IRId,S.OldAdmnNo,cs.Section,cm.ClassName,I.MemberId,I.AccessionNo,I.IssueDate as issuedt,convert(varchar,I.IssueDate,105) IssueDate,I.DueDate as duedt,convert(varchar,I.DueDate,105) DueDate,L.MemberName,L.EmpNo,B.BookTitle from dbo.Lib_IssueReturnMaster I left join dbo.Lib_Member L on I.MemberId=L.MemberId left join dbo.Ps_StudMaster S on S.admnno=L.empNo left join dbo.Lib_BookAccession A on A.AccessionNo=I.AccessionNo left join Lib_BookMaster B on B.BookId=A.BookId left join Ps_classwisestudent cs on cs.admnno=L.EmpNo left join Ps_classmaster cm on cs.classid = cm.classid where ReturnDate is null and I.CollegeId=" + Session["SchoolId"]);
        if (ddlClass.SelectedValue != "0")
            stringBuilder.Append(" and  cs.ClassId ='" + ddlClass.SelectedValue + "' ");
        if (ddlSection.SelectedValue != "0")
            stringBuilder.Append(" and  cs.Section ='" + ddlSection.SelectedValue + "' ");
        
        if (rdbtnlstUsertype.SelectedValue == "0")
            stringBuilder.Append(" and MemberType=1");
        else
            stringBuilder.Append(" and MemberType=2");
        if (txtFrmDt.Text != "")
            stringBuilder.Append(" and IssueDate >='" +dtpFrmDt.SelectedDate + "' and IssueDate <='" +dtpToDt.SelectedDate + "'");
        if (ddlMemberId.SelectedIndex > 0)
            stringBuilder.Append(" and I.MemberId=" +ddlMemberId.SelectedValue);
        if (txtAccno.Text != "")
       if (ddlClass.SelectedValue != "0")
        stringBuilder.Append(" and cs.Detained_Promoted='' order by IssueDate");
        return stringBuilder.ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
       BindGrid(GetQry());
       dvIrlist.Visible = true;
    }

    protected void grdIssueList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
           grdIssueList.PageIndex = e.NewPageIndex;
           BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }

    protected void rdbtnlstUsertype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtnlstUsertype.SelectedValue == "0")
        {
            bindDropDown(ddlMemberId, "select MemberId,MemberName+' ( '+EmpNo+' )' as MemberName from Lib_Member where MemberType='1' and (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') and CollegeId=" + Session["SchoolId"] + " order by MemberName", "MemberName", "MemberId");
            ddlClass.Enabled = false;
            ddlSection.Enabled = false;
            
        }
        else
        {
            bindDropDown(ddlClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
            ddlClass.Enabled = true;
            ddlSection.Enabled = true;
            BindStudent();
        }
    }

    private void BindStudent()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select MemberId,MemberName+' ( '+EmpNo+' )' as MemberName from Lib_Member M");
        stringBuilder.Append(" inner join dbo.PS_ClasswiseStudent C on C.admnno=M.EmpNo");
        stringBuilder.Append(" where MemberType='2' and (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') and M.CollegeId=" +Session["SchoolId"] + " and detained_promoted=''");
        if (ddlClass.SelectedIndex > 0)
            stringBuilder.Append(" and C.ClassID=" + int.Parse(ddlClass.SelectedValue));
        if (ddlSection.SelectedIndex > 0)
            stringBuilder.Append(" and C.Section='" + ddlSection.SelectedValue + "'");
        stringBuilder.Append(" order by MemberName");
       bindDropDown(ddlMemberId, stringBuilder.ToString(), "MemberName", "MemberId");
    }

    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlClass.SelectedValue.ToString().Trim()) <= 0)
            return;
        bindSection();
        try
        {
            BindStudent();
        }
        catch (Exception ex)
        {
        }
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
    private void bindSection()
    {



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
}