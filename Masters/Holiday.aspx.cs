using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Masters_Holiday : System.Web.UI.Page
{
    private clsGenerateFee objfee = new clsGenerateFee();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
        {
            if (Request.QueryString["status"] != null)
               ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Previous information cannot be edited');", true);
           fillgrid();
           CheckPermissions();
        }
        else
           Response.Redirect("../Login.aspx");
    }

    private void CheckPermissions()
    {
        try
        {
            string absoluteUri =Request.Url.AbsoluteUri;
            string str = absoluteUri.Substring(absoluteUri.LastIndexOf("/") + 1).Split('?')[0];
            if (Session["permission"] == null)
                return;
            foreach (DataRow row in (InternalDataCollectionBase)(Session["permission"] as DataTable).Rows)
            {
                string lower = row["PageID"].ToString().ToLower();
                bool boolean = Convert.ToBoolean(row["status"]);
                if (lower == str.ToLower() && !boolean)
                {
                   ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('You are not permitted. Contact your administrator'); window.location='" + "../Administrations/Home.aspx" + "';", true);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
           Response.Write("<script>alert('Select a checkbox');</script>");
        }
        else
        {
            string str =Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                new Common().GetDataTable("ps_sp_delete_holiday", new Hashtable()
        {
          {
            "holidayid",
            obj
          }
        });
        }
       fillgrid();
    }

    protected void btnshow_Click(object sender, EventArgs e)
    {
       fillgrid();
    }

    private void fillgrid()
    {
        Common common = new Common();
        string currSession =objfee.CreateCurrSession();
       grdHoliday.DataSource =common.ExecuteSql("select HolidayID,HolidayName,HolidayDate as HDate,convert(varchar,HolidayDate,103)as HolidayDate from dbo.PS_HolidayMaster where HolidayDate between '" + currSession.Substring(0, 4) + "-04-01' and '20" + currSession.Substring(5, 2) + "-03-31' order by HDate desc");
       grdHoliday.DataBind();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
       Response.Redirect("Holidaydetail.aspx");
    }

    protected void grdHoliday_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       grdHoliday.PageIndex = e.NewPageIndex;
       fillgrid();
    }
}