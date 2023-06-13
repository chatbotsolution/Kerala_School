using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Inventory_InvProcurementList : System.Web.UI.Page
{

    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            lblmsg.Text = string.Empty;
            if (Page.IsPostBack)
                return;
            Page.Form.DefaultButton = btnSearch.UniqueID;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void BindList(string query)
    {
        dt = obj.GetDataTableQry(query);
        grdShowDtls.DataSource = dt;
        grdShowDtls.DataBind();
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("InvProcurement.aspx");
    }

    protected void btnDel_Click(object sender, EventArgs e)
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
                if (str.Split(',').Length > 1)
                {
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select Only One Record');", true);
                }
                else
                {
                    if (obj.ExecuteScalar("SI_DelPurchaseDocs", new Hashtable()
          {
            {
               "@PurchaseId",
               Convert.ToInt16(str)
            }
          }) != "")
                    {
                        lblmsg.Text = "Deleted Successfully";
                        lblmsg.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblmsg.Text = "Dependency Exist. Can not be Deleted";
                        lblmsg.ForeColor = Color.Red;
                    }
                    BindList("select * from SI_PurchaseDoc where SchoolId='" + Convert.ToInt32(Session["SchoolId"]) + "' order by PurDate desc");
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void DeleteAccession(string id)
    {
        obj.GetDataTable("SI_DelPurchaseDocs", new Hashtable()
    {
      {
         "@PurchaseId",
         Convert.ToInt16(id)
      }
    });
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select * from  SI_PurchaseDoc where 1=1 ");
            if (Session["User"].ToString() != "admin")
                stringBuilder.Append("and  SchoolId='" + Convert.ToInt32(Session["SchoolId"]) + "'");
            if (txtFrmDt.Text != "" & txtTo.Text != "")
                stringBuilder.Append(" and PurDate>='" + PopCalendar1.GetDateValue().ToString("MM/dd/yyyy") + " 00:00:00' and  PurDate<='" + PopCalendar2.GetDateValue().ToString("MM/dd/yyyy") + " 23:59:59'");
            stringBuilder.Append(" order by PurDate desc");
            BindList(stringBuilder.ToString());
        }
        catch (Exception ex)
        {
        }
    }

    protected void grdShowDtls_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            grdShowDtls.PageIndex = e.NewPageIndex;
            btnSearch_Click(sender, (EventArgs)e);
        }
        catch (Exception ex)
        {
        }
    }
}