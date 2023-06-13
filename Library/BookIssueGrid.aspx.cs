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

public partial class Library_BookIssueGrid : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User_Id"] != null)
        {
            bindDropDown(ddlClass, "select ClassID,ClassName from dbo.PS_ClassMaster", "ClassName", "ClassID");
        }
        else
            Response.Redirect("../Login.aspx");
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
    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(ddlClass.SelectedValue.ToString().Trim()) <= 0)
            return;
        bindSection();
        try
        {
           // BindStudent();
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
    private void BindStudent()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select M.MemberId,MemberName,P.OldAdmnno,C.RollNo,C.Section,Cm.ClassName from Lib_Member M");
        stringBuilder.Append(" inner join dbo.PS_ClasswiseStudent C on C.admnno=M.EmpNo inner join dbo.Ps_StudMaster P on P.admnno=C.admnno inner join Ps_ClassMaster cm on cm.classid=C.ClassId");
        stringBuilder.Append(" where.MemberId not in (select MemberId from Lib_IssueReturnMaster where ReturnDate is null) M MemberType='2' and (ExpiryDate is null or ExpiryDate > '" + DateTime.Now.ToString() + "') and M.CollegeId=" + Session["SchoolId"] + " and detained_promoted=''");
        if (ddlClass.SelectedIndex > 0)
            stringBuilder.Append(" and C.ClassID=" + int.Parse(ddlClass.SelectedValue));
        if (ddlSection.SelectedIndex > 0)
            stringBuilder.Append(" and C.Section='" + ddlSection.SelectedValue + "'");
        if(txtIssueDt.Text !="")
            stringBuilder.Append(" and (I.IssueDate is null or I.IssueDate <> Convert(datetime,'" + dtpIssueDt.SelectedDate + "'))");
        stringBuilder.Append(" order by C.RollNo,MemberName");
        BindGrid( stringBuilder.ToString());
        
    }

    private void BindGrid(string qry)
    {
        
        DataTable  dt = obj.GetDataTableQry(qry);
        grdIssueList.DataSource = dt;
        grdIssueList.DataBind();
        grdIssueList.Visible = true;
        dvIssue.Visible = true;
    }
    protected void dtpIssueDt_SelectionChanged(object sender, EventArgs e)
    {
        dtpDueDt.From.Date = dtpIssueDt.GetDateValue();
        calculateDueDate();
        BindStudent();
    }
    private void calculateDueDate()
    {
        obj = new clsDAL();

        dtpDueDt.SetDateValue(dtpIssueDt.GetDateValue().AddDays(7));     // 7 days as per Loyola School 
    }
    protected void txtAccnNo_textChanged(object sender, EventArgs e)
     {
         DataTable dt = new DataTable();
         Hashtable ht = new Hashtable();
         ht.Clear();
         GridViewRow row = ((GridViewRow)((TextBox)sender).Parent.Parent);
         ht.Add("@AccessionNo", (row.FindControl("txtAccnNo") as TextBox).Text.ToString().Trim());
         ht.Add("@CollegeId", Session["SchoolId"]);
         dt = obj.GetDataTable("Lib_get_BookDtls", ht);
         if (dt.Rows.Count > 0)
         {
             if (obj.ExecuteScalar("Lib_SP_GetStatus", ht) == "R")
             {
                 (row.FindControl("label1") as Label).ForeColor = System.Drawing.Color.Black;
                 (row.FindControl("label1") as Label).Text = "BookName: " + dt.Rows[0]["Booktitle"].ToString() + ", Category: " + dt.Rows[0]["CatName"].ToString() + ", Subject: " + dt.Rows[0]["SubName"].ToString();
                 (row.FindControl("btnIssue") as Button).Enabled = true;
                 (row.FindControl("hfAccNo") as HiddenField).Value = (row.FindControl("txtAccnNo") as TextBox).Text.ToString().Trim();
             }
             else
             {
                 (row.FindControl("label1") as Label).Text = "This book is not available to issue !";
                 (row.FindControl("label1") as Label).ForeColor = System.Drawing.Color.Red;
                 (row.FindControl("btnIssue") as Button).Enabled = false;
             }
         }
         else
         {
             (row.FindControl("label1") as Label).Text = "No books for this Accn No.";
             (row.FindControl("btnIssue") as Button).Enabled = false;
         }
         
    }
    protected void btnIssue_Clicked(object sender, EventArgs e)
    {
        if (txtIssueDt.Text == "")
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select Issue Date');", true);
            return;
        }
        GridViewRow row = ((GridViewRow)((Button)sender).Parent.Parent);
        Hashtable ht = new Hashtable();
        ht.Clear();
       
       ht.Add("@IRId", 0);
       ht.Add("@MemberId", int.Parse((row.FindControl("lblMId")as Label).Text.Trim()));
       ht.Add("@AccessionNo", (row.FindControl("txtAccnNo") as TextBox).Text.ToString().Trim());
        if (txtIssueDt.Text != "")
           ht.Add("@IssueDate", dtpIssueDt.SelectedDate.ToString());
        if (txtDueDt.Text != "")
           ht.Add("@duedate", dtpDueDt.SelectedDate.ToString());
       ht.Add("@UserId", 1);
       ht.Add("@CollegeId",Session["SchoolId"]);
       //dt.Clear();
        string str =obj.ExecuteScalar("Lib_SP_InsUpdtIssue",ht);
        if (str != "")
        {
           (row.FindControl("Label1")as Label).Text = str;
           (row.FindControl("Label1")as Label).ForeColor = System.Drawing.Color.Red;
        }
        else
        {
           (row.FindControl("Label1")as Label).Text = "Issue Saved Successfully";
           (row.FindControl("Label1")as Label).ForeColor = System.Drawing.Color.Green;
          
        }
        (row.FindControl("txtAccnNo") as TextBox).Enabled = false;
        (row.FindControl("btnIssue") as Button).Enabled = false;

       }
    
}