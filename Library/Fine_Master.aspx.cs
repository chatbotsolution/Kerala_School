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

public partial class Library_Fine_Master : System.Web.UI.Page
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
                Form.DefaultButton = btnSaveAddNew.UniqueID;
                if (Request.QueryString["MemberId"] == null)
                    return;
                ht.Clear();
                ht.Add("@MemberId", int.Parse(Request.QueryString["MemberId"].ToString()));
                ht.Add("@CollegeId", Session["SchoolId"]);
                dt.Clear();
               
                btnSaveAddNew.Text = "Update & AddNew";
                btnSaveGotoList.Text = "Update & GotoList";
            }
            else
                Response.Redirect("../Login.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    private void Clear()
    {
        txtAllowDay.Text = "";
        txtAllowBook.Text = "";
        txtFee.Text = "";
    }

    private void SaveData()
    {
        try
        {
            ht.Clear();
            if (Request.QueryString["MemberId"] == null)
                ht.Add("@MemberId", 0);
            else if (btnSaveAddNew.Text == "Update & AddNew" || btnSaveGotoList.Text == "Update & GotoList")
                ht.Add("@MemberId", int.Parse(Request.QueryString["MemberId"].ToString()));
            else
                ht.Add("@MemberId", 0);
          
            ht.Add("@AllowedDays", txtAllowDay.Text.Trim());
            ht.Add("@NoOfBooksAllowed", txtAllowBook.Text.Trim());
            if (rdbtnFineStatus.SelectedValue == "1")
                ht.Add("@IsFineApplicable", true);
            else
                ht.Add("@IsFineApplicable", false);
            ht.Add("@MemberFee", txtFee.Text.Trim());
            ht.Add("@UserId", 1);
            ht.Add("@CollegeId", Session["SchoolId"]);
            dt.Clear();
            dt = obj.GetDataTable("Lib_SP_InsUpdtMember", ht);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Member Name Already Exist');", true);
            }
            else
            {
                if (btnSaveAddNew.Text == "Update & AddNew" || btnSaveGotoList.Text == "Update & GotoList")
                {
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Data Updated Successfully');", true);
                    btnSaveAddNew.Text = "Save & AddNew";
                    btnSaveGotoList.Text = "Save & GotoList";
                }
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Data Save Successfully');", true);
                Clear();
            }
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
        }
        else
        {
            bindDropDown(ddlClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
            ddlClass.Enabled = true;
            BindStudent();
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

    private void BindStudent()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select MemberId,MemberName+' ( '+EmpNo+' )' as MemberName from Lib_Member M");
        stringBuilder.Append(" inner join dbo.PS_ClasswiseStudent C on C.admnno=M.EmpNo");
        stringBuilder.Append(" where MemberType='2' and (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') and M.CollegeId=" + Session["SchoolId"] + " and (detained_promoted='' or Detained_Promoted is null)");
        if (ddlClass.SelectedIndex > 0)
            stringBuilder.Append(" and C.ClassID=" + int.Parse(ddlClass.SelectedValue));
        stringBuilder.Append(" order by MemberName");
        bindDropDown(ddlMemberId, stringBuilder.ToString(), "MemberName", "MemberId");
    }
    protected void btnSaveAddNew_Click(object sender, EventArgs e)
    {
        SaveData();
        //lblMemberType.Text = "Employee";
    }
    protected void btnSaveGotoList_Click(object sender, EventArgs e)
    {
        SaveData();
        //Response.Redirect("MemberList.aspx");
    }

}