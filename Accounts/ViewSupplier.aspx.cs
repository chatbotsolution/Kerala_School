using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_ViewSupplier : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null || Request.QueryString["ItemCode"] == null)
            return;
        BindGrid(int.Parse(Request.QueryString["ItemCode"].ToString()));
    }

    private void BindGrid(int itemcode)
    {
        try
        {
            ht.Clear();
            ht.Add("@ItemCode", itemcode);
            dt = obj.GetDataTable("ACTS_ItemwiseSupplier", ht);
            gvSuppliers.DataSource = dt;
            gvSuppliers.DataBind();
            lblItem.Text = Request.QueryString["ItemName"].ToString();
        }
        catch (Exception ex)
        {
        }
    }

    protected void gvSuppliers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (!(e.CommandName == "Select"))
                return;
            foreach (GridViewRow row1 in gvSuppliers.Rows)
            {
                if (((HiddenField)row1.FindControl("hdnSupplierId")).Value == e.CommandArgument.ToString())
                {
                    string str = ((HiddenField)row1.FindControl("hdnSupplierName")).Value.ToString();
                    DataTable dataTable = (DataTable)Session["Table"];
                    foreach (DataRow row2 in (InternalDataCollectionBase)dataTable.Rows)
                    {
                        if (row2["ItemCode"].ToString() == Request.QueryString["ItemCode"].ToString())
                        {
                            row2["SupplierName"] = str;
                            row2["SupplierId"] = e.CommandArgument.ToString();
                            dataTable.AcceptChanges();
                            break;
                        }
                    }
                    Session["Table"] = dataTable;
                    break;
                }
            }
            RegisterStartupScript("Success", "<script language='javascript'>opener.location.reload(true);self.close();</script>");
        }
        catch (Exception ex)
        {
        }
    }
}