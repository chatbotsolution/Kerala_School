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
using System.Web.UI.WebControls;

public partial class Library_BookIssueList : System.Web.UI.Page
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
            bindDropDown(ddlClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
            bindDropDown(ddlMemberId, "select MemberId,MemberName+' ( '+EmpNo+' )' as MemberName from Lib_Member where MemberType='2' and  (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') AND CollegeId=" + Session["SchoolId"] + " order by MemberName", "MemberName", "MemberId");
            BindGrid(GetQry());
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void BindGrid(string qry)
    {
        btnDelete.Visible = true;
        dt = obj.GetDataTableQry(qry);
        grdIssueList.DataSource = dt;
        grdIssueList.DataBind();
        lblRecords.Text = "Total Record(s): " + dt.Rows.Count.ToString();
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

    private string GetQry()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select P.Class,Cm.Classname,cs.Section,I.IRId,I.MemberId,L.EmpNo,P.OldAdmnNo,I.AccessionNo,convert(varchar,I.IssueDate,105) IssueDate,convert(varchar,I.DueDate,105) DueDate,L.MemberName,B.BookTitle from dbo.Lib_IssueReturnMaster I left join dbo.Lib_Member L on I.MemberId=L.MemberId left join dbo.Ps_studmaster P on P.admnno=L.EmpNo left join dbo.Lib_BookAccession A on A.AccessionNo=I.AccessionNo left join Lib_BookMaster B on B.BookId=A.BookId left join Ps_Classwisestudent cs on cs.admnno=L.EmpNo inner join Ps_ClassMaster Cm on cm.Classid=Cs.classid  where IssueDate is not null and A.Status='I' and I.CollegeId=" + Session["SchoolId"]);
        if (ddlClass.SelectedValue != "0")
            stringBuilder.Append(" and  P.Class ='" + ddlClass.SelectedValue + "' ");
        if (ddlSection.SelectedValue != "0")
            stringBuilder.Append(" and  cs.Section ='" + ddlSection.SelectedValue + "' ");

        if (txtFrmDt.Text != "")
            stringBuilder.Append(" and IssueDate >='" + dtpFrmDt.SelectedDate + "' and IssueDate <='" + dtpToDt.SelectedDate + "'");
        if (ddlMemberId.SelectedIndex > 0)
            stringBuilder.Append(" and I.MemberId=" + ddlMemberId.SelectedValue);
        if (rdbtnlstUsertype.SelectedValue == "0")
            stringBuilder.Append(" and L.MemberType=1");
        else
            stringBuilder.Append(" and L.MemberType=2");
        if (txtOldDuedt.Text != "")
            stringBuilder.Append(" and I.DueDate=Convert(datetime,'" + txtOldDuedt.Text + "')");
        stringBuilder.Append(" and Cs.Detained_Promoted='' order by Convert(Datetime,IssueDate) Desc");
        return stringBuilder.ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid(GetQry());
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("BookIssue.aspx");
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
        ht.Add("@IRId", id);
        ht.Add("@CollegeId", Session["SchoolId"]);
        string str = obj.ExecuteScalar("Lib_SP_DeleteBookIssue", ht);
        if (str == "")
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Deleted Successfully');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(" + str + ");", true);
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
            btnImport.Enabled = false;
        }
        else
        {
            bindDropDown(ddlClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
            ddlClass.Enabled = true;
            ddlSection.Enabled = true;
            btnImport.Enabled = true;
            BindStudent();
        }
    }

    private void BindStudent()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select MemberId,MemberName+' ( '+P.OldAdmnno+' )' as MemberName from Lib_Member M");
        stringBuilder.Append(" inner join dbo.PS_ClasswiseStudent C on C.admnno=M.EmpNo inner join dbo.Ps_StudMaster P on P.admnno=C.admnno");
        stringBuilder.Append(" where MemberType='2' and (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') and M.CollegeId=" + Session["SchoolId"] + " and detained_promoted=''");
        if (ddlClass.SelectedIndex > 0)
            stringBuilder.Append(" and C.ClassID=" + int.Parse(ddlClass.SelectedValue));
        if (ddlSection.SelectedIndex > 0)
            stringBuilder.Append(" and C.Section='" + ddlSection.SelectedValue + "'");
        stringBuilder.Append(" order by MemberName");
        bindDropDown(ddlMemberId, stringBuilder.ToString(), "MemberName", "MemberId");
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            dt = obj.GetDataTableQry("select * from dbo.PS_StudMaster where Status=1 and AdmnNo not in(select EmpNo from dbo.Lib_Member where MemberType='2')");
            if (dt.Rows.Count <= 0)
                return;
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                ht.Clear();
                ht.Add("@MemberId", 0);
                ht.Add("@EmpNo", row["AdmnNo"].ToString());
                ht.Add("@MemberType", "2");
                ht.Add("@MemberName", row["FullName"].ToString());
                ht.Add("@Address", row["PresentAddress"].ToString());
                ht.Add("@EmailId", row["EmailId"].ToString());
                ht.Add("@RegdDate", row["AdmnDate"].ToString());
                ht.Add("@AllowedDays", 30);
                ht.Add("@NoOfBooksAllowed", 5);
                ht.Add("@IsFineApplicable", true);
                ht.Add("@MemberFee", 0);
                ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
                ht.Add("@CollegeId", int.Parse(Session["SchoolId"].ToString()));
                obj.ExcuteProcInsUpdt("Lib_SP_InsUpdtMember", ht);
            }
            BindStudent();
        }
        catch (Exception ex)
        {
        }
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
    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        try
        {

            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                // ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "Confirm('Proceed to Update?');", true);
                string str = Request["Checkb"];
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str.Split(chArray))
                    UpdateDueDate(obj.ToString());
            }
            txtNewDuedt.Text = txtOldDuedt.Text = "";
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void UpdateDueDate(string id)
    {
        try
        {
            string qry = "Update dbo.Lib_IssueReturnMaster Set DueDate=convert(datetime, '" + txtNewDuedt.Text + "') where IRId=" + id + "";
            string str = new clsDAL().ExecuteScalarQry(qry);
            //new clsDAL().ExecuteScalarQry("Update dbo.Lib_IssueReturnMaster Set DueDate=" + Convert.ToDateTime(txtNewDuedt.Text) + " where IRId=" + id + "");
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Due Date Updated!');", true);
        }
        catch (Exception ex)
        {
            string msg = ex.Message;
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + msg + "');", true);
        }
       

    }
    protected void PopCalendar1_SelectionChanged(object sender, EventArgs e)
    {
        BindGrid(GetQry());
    }
}

