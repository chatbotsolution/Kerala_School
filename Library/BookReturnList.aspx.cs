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

public partial class Library_BookReturnList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Session["title"] = Page.Title.ToString();
        if (Session["User_Id"] != null)
            BindGrid(GetQry());
        else
            Response.Redirect("../Login.aspx");
    }

    private void BindGrid(string qry)
    {
        dt = obj.GetDataTableQry(qry);
        grdReturnList.DataSource = dt;
        grdReturnList.DataBind();
        lblRecords.Text = "Total Record(s): " + dt.Rows.Count.ToString();
    }

    private string GetQry()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select I.IRId,I.MemberId,I.AccessionNo,convert(varchar,I.ReturnDate,105) ReturnDate,cast(I.FineAmt as decimal(10,2)) FineAmt,L.MemberName from dbo.Lib_IssueReturnMaster I left join dbo.Lib_Member L on I.MemberId=L.MemberId where ReturnDate is not null");
        if (txtFrmDt.Text != "")
            stringBuilder.Append(" and ReturnDate >='" + dtpFrmDt.SelectedDate + "' and ReturnDate <='" + dtpToDt.SelectedDate + "'");
        if (txtMemberId.Text != "")
            stringBuilder.Append(" and I.MemberId=" + txtMemberId.Text);
        stringBuilder.Append(" order by ReturnDate");
        return stringBuilder.ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid(GetQry());
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
        string str = obj.ExecuteScalar("Lib_SP_DeleteBookReturn", ht);
        if (str == "")
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Deleted Successfully');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(" + str + ");", true);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("BookReturn.aspx");
    }

    protected void grdReturnList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdReturnList.PageIndex = e.NewPageIndex;
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }
}