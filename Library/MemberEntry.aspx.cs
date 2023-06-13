using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_MemberEntry : System.Web.UI.Page
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
               Form.DefaultButton =btnSaveAddNew.UniqueID;
                if (Request.QueryString["MemberId"] == null)
                    return;
               ht.Clear();
               ht.Add("@MemberId", int.Parse(Request.QueryString["MemberId"].ToString()));
               ht.Add("@CollegeId",Session["SchoolId"]);
               dt.Clear();
               dt =obj.GetDataTable("Lib_SP_MemberList",ht);
                if (dt.Rows.Count <= 0)
                    return;
               FillField(dt);
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

    private void Clear()
    {
       txtRegdDt.Text = "";
       txtExpDt.Text = "";
       txtMemId.Text = "";
       txtName.Text = "";
       txtAddress.Text = "";
       txtEmailId.Text = "";
       txtPhno.Text = "";
       txtAllowDay.Text = "";
       txtAllowBook.Text = "";
       txtFee.Text = "";
    }

    private void FillField(DataTable dt)
    {
       lblMemberType.Text = !(dt.Rows[0]["MemberType"].ToString() == "1") ? "Student" : "Employee";
       txtName.Text = dt.Rows[0]["MemberName"].ToString();
       txtMemId.Text = dt.Rows[0]["EmpNo"].ToString();
       txtAddress.Text = dt.Rows[0]["Address"].ToString();
       txtEmailId.Text = dt.Rows[0]["EmailId"].ToString();
       txtPhno.Text = dt.Rows[0]["Phone"].ToString();
        if (dt.Rows[0]["RegdDate"].ToString() != "")
        {
            dtpRegdDt.SetDateValue(Convert.ToDateTime(dt.Rows[0]["RegdDate"].ToString()));
            dtpExpireDt.From.Date=dtpRegdDt.GetDateValue();
        }
        if (dt.Rows[0]["ExpiryDate"].ToString() != "")
            dtpExpireDt.SetDateValue(Convert.ToDateTime(dt.Rows[0]["ExpiryDate"].ToString()));
       txtAllowDay.Text = dt.Rows[0]["AllowedDays"].ToString();
       txtAllowBook.Text = dt.Rows[0]["NoOfBooksAllowed"].ToString();
        if (dt.Rows[0]["IsFineApplicable"].ToString() == "False")
           rdbtnFineStatus.SelectedValue = "0";
        else
           rdbtnFineStatus.SelectedValue = "1";
       txtFee.Text = dt.Rows[0]["MemberFee"].ToString();
    }

    protected void btnSaveAddNew_Click(object sender, EventArgs e)
    {
       SaveData();
       lblMemberType.Text = "Employee";
    }

    protected void btnSaveGotoList_Click(object sender, EventArgs e)
    {
       SaveData();
       Response.Redirect("MemberList.aspx");
    }

    private void SaveData()
    {
        try
        {
           ht.Clear();
            if (Request.QueryString["MemberId"] == null)
               ht.Add("@MemberId", 0);
            else if (btnSaveAddNew.Text == "Update & AddNew" ||btnSaveGotoList.Text == "Update & GotoList")
               ht.Add("@MemberId", int.Parse(Request.QueryString["MemberId"].ToString()));
            else
               ht.Add("@MemberId", 0);
           ht.Add("@EmpNo", txtMemId.Text.Trim());
           ht.Add("@MemberType", "1");
           ht.Add("@MemberName", txtName.Text.Trim());
           ht.Add("@DesignationId", "1");
           ht.Add("@DeptId", "1");
           ht.Add("@Address", txtAddress.Text.Trim());
           ht.Add("@Phone", txtPhno.Text.Trim());
           ht.Add("@EmailId", txtEmailId.Text.Trim());
            if (txtRegdDt.Text != "")
                ht.Add("@RegdDate", dtpRegdDt.SelectedDate.ToString());
            if (txtExpDt.Text != "")
                ht.Add("@ExpiryDate", dtpExpireDt.SelectedDate.ToString());
           ht.Add("@AllowedDays", txtAllowDay.Text.Trim());
           ht.Add("@NoOfBooksAllowed", txtAllowBook.Text.Trim());
            if (rdbtnFineStatus.SelectedValue == "1")
               ht.Add("@IsFineApplicable", true);
            else
               ht.Add("@IsFineApplicable", false);
           ht.Add("@MemberFee", txtFee.Text.Trim());
           ht.Add("@UserId", 1);
           ht.Add("@CollegeId",Session["SchoolId"]);
           dt.Clear();
           dt =obj.GetDataTable("Lib_SP_InsUpdtMember",ht);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(' Member Name Already Exist');", true);
            }
            else
            {
                if (btnSaveAddNew.Text == "Update & AddNew" ||btnSaveGotoList.Text == "Update & GotoList")
                {
                    ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(' Data Updated Successfully');", true);
                   btnSaveAddNew.Text = "Save & AddNew";
                   btnSaveGotoList.Text = "Save & GotoList";
                }
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(' Data Save Successfully');", true);
               Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       Clear();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
       Response.Redirect("MemberList.aspx");
    }

    protected void dtpRegdDt_SelectionChanged(object sender, EventArgs e)
    {
       txtExpDt.Text = "";
       dtpExpireDt.From.Date=dtpRegdDt.GetDateValue();
    }
}