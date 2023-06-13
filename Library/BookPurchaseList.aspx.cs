using AjaxControlToolkit;
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

public partial class Library_BookPurchaseList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
        }
    }

    private void BindGrid(string qry)
    {
        dt = obj.GetDataTableQry(qry);
        grdPurchaseList.DataSource = dt;
        grdPurchaseList.DataBind();
        lblRecords.Text = "Total Record(s): " + dt.Rows.Count.ToString();
    }

    private string GetQry()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select * from Lib_VW_PurchaseList where CollegeId=" + Session["SchoolId"]);
        if (txtFrmDt.Text != "")
            stringBuilder.Append(" and PurchaseDate >='" + dtpFrmDt.GetDateValue() + "' and PurchaseDate <='" + dtpToDt.GetDateValue() + "'");
        if (txttitle.Text != "")
            stringBuilder.Append(" and BookTitle like '%" + txttitle.Text.Trim() + "%'");
        stringBuilder.Append(" order by PurchaseDate desc");
        return stringBuilder.ToString();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("BookPurchase.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            string str1 = "";
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                string str2 = Request["Checkb"];
                char[] chArray = new char[1] { ',' };
                foreach (string id in str2.Split(chArray))
                {
                    str1 = DeleteAccession(id);
                    if (str1 != "")
                        break;
                }
                if (str1 == "")
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Deleted Successfully');", true);
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(" + str1 + ");", true);
            }
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }

    private string DeleteAccession(string id)
    {
        ht.Clear();
        ht.Add("@PurchaseId", int.Parse(id));
        return obj.ExecuteScalar("Lib_SP_DeleteBookPurchase", ht);
    }

    protected void grdPurchaseList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdPurchaseList.PageIndex = e.NewPageIndex;
            BindGrid(GetQry());
        }
        catch (Exception ex)
        {
        }
    }
}