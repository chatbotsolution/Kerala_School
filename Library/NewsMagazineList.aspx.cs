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

public partial class Library_NewsMagazineList : System.Web.UI.Page
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

    private void BindGrid(string qry)
    {
        dt = obj.GetDataTableQry(qry);
        grdNewsMagazineList.DataSource = dt;
        grdNewsMagazineList.DataBind();
        lblRecords.Text = "Total Record(s): " + dt.Rows.Count.ToString();
    }

    private string GetQry()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select MagazineId, MagazineName, convert(varchar,SubscriptionDate,105) SubscriptionDate, convert(varchar,SubscriptionExpired,105) SubscriptionExpired, Periodicity, cast(TotalAmountPaid as decimal(10,2)) TotalAmountPaid from Lib_NewsMagazineMaster where CollegeId=" + Session["SchoolId"] + " AND 1=1");
        if (txtFrmDt.Text != "")
            stringBuilder.Append(" and SubscriptionDate >='" + dtpFrmDt.SelectedDate + "' and SubscriptionDate <='" + dtpToDt.SelectedDate + "'");
        if (ddlPeriodicity.SelectedIndex > 0)
            stringBuilder.Append(" and Periodicity='" + ddlPeriodicity.SelectedValue + "'");
        stringBuilder.Append(" order by MagazineId");
        return stringBuilder.ToString();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("NewsMagazineMaster.aspx");
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
        ht.Add("@MagazineId", id);
        ht.Add("@CollegeId", Session["SchoolId"]);
        string str = obj.ExecuteScalar("Lib_SP_DeleteNewsMagazine", ht);
        if (str == "")
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Deleted Successfully');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(" + str + ");", true);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        BindGrid(GetQry());
    }

    protected void grdNewsMagazineList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdNewsMagazineList.PageIndex = e.NewPageIndex;
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }
}