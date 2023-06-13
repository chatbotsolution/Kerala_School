using ASP;
using SanLib;
using System;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Inventory_PurchaseDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["PurchaseId"] != null)
            BindList("select PD.*,IM.ItemName from SI_PurchaseDetail PD inner join SI_ItemMaster IM on IM.ItemCode=PD.ItemCode WHERE PurchaseId=" + int.Parse(Request.QueryString["PurchaseId"].ToString()) + " order by ItemName");
        else
            lblmsg.Text = "No Data Found";
    }

    private void BindList(string query)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        grdShowDtls.DataSource = clsDal.GetDataTableQry(query);
        grdShowDtls.DataBind();
    }
}