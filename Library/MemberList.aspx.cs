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

public partial class Library_MemberList : System.Web.UI.Page
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
            BindGrid(GetQry());
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
        drp.Items.Insert(0, new ListItem("---All---", "0"));
    }

    private void BindGrid(string qry)
    {
        dt = obj.GetDataTableQry(qry);
        grdMemberList.DataSource = dt;
        grdMemberList.DataBind();
        lblRecords.Text = "Total Record(s): " + dt.Rows.Count.ToString();
    }

    private string GetQry()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select MemberId, MemberName, EmpNo, Phone,case when MemberType=1 then 'Employee' else 'Student' end as MemberType from Lib_Member where CollegeId=" + Session["SchoolId"]);
        if (txtName.Text != "")
            stringBuilder.Append(" and MemberName like '%" + txtName.Text.Trim() + "%'");
        if (txtEmpId.Text != "")
            stringBuilder.Append(" and EmpNo='" + txtEmpId.Text.Trim() + "'");
        stringBuilder.Append(" order by MemberId");
        return stringBuilder.ToString();
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
        ht.Add("@MemberId", id);
        ht.Add("@CollegeId", Session["SchoolId"]);
        string str = obj.ExecuteScalar("Lib_SP_DeleteMember", ht);
        if (str == "")
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Deleted Successfully');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(" + str + ");", true);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("MemberEntry.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid(GetQry());
    }

    protected void grdMemberList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdMemberList.PageIndex = e.NewPageIndex;
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }
    protected void btnImport_Click(object sender, EventArgs e)
    {
        try
        {
            dt = obj.GetDataTableQry("select * from dbo.PS_StudMaster P inner join dbo.ps_classwisestudent C on p.admnno=c.admnno where Status=1 and c.sessionyear='2018-19' and C.AdmnNo not in(select EmpNo from dbo.Lib_Member where MemberType='2')");
            if (dt.Rows.Count <= 0)
                return;
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                ht.Clear();
                ht.Add("@MemberId", 0);
                ht.Add("@EmpNo", row["AdmnNo"].ToString());
                ht.Add("@MemberType", "2");
                ht.Add("@MemberName", row["FullName"].ToString());
                ht.Add("@Address", row["PresAddr1"].ToString() + "," + row["PresAddr2"].ToString() + "," + row["PresAddrCity"].ToString());
                ht.Add("@EmailId", row["EmailId1"].ToString());
                ht.Add("@RegdDate", row["AdmnDate"].ToString());
                ht.Add("@AllowedDays", 7);
                ht.Add("@NoOfBooksAllowed", 2);
                ht.Add("@IsFineApplicable", true);
                ht.Add("@MemberFee", 0);
                ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
                ht.Add("@CollegeId", int.Parse(Session["SchoolId"].ToString()));
                obj.ExcuteProcInsUpdt("Lib_SP_InsUpdtMember", ht);
            }
           // BindStudent();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}