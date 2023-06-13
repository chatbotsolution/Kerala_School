using ASP;
using System;
using System.Data;
using System.Web.Profile;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class MasterPages_SMS : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["user"] != null && Session["SSVMID"] != null)
        {
            lblSSVM.Text = Session["SchoolName"].ToString();
            lblUser.Text = Session["User"].ToString();
            CheckPermissions();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void CheckPermissions()
    {
        try
        {
            string absoluteUri = Request.Url.AbsoluteUri;
            string str = absoluteUri.Substring(absoluteUri.LastIndexOf("/") + 1).Split('?')[0];
            if (Session["permission"] == null)
                return;
            foreach (DataRow row in (InternalDataCollectionBase)(Session["permission"] as DataTable).Rows)
            {
                string lower = row["PageID"].ToString().ToLower();
                bool boolean = Convert.ToBoolean(row["status"]);
                if (lower == str.ToLower() && !boolean)
                {
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('You are not permitted. Contact your administrator'); window.location='" + "../Administrations/Home.aspx" + "';", true);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void lnkHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("Administrations/Home.aspx");
    }

    //protected void imgBtnDashboard_Click(object sender, ImageClickEventArgs e)
    //{
    //    Response.Redirect("../Dashboard.aspx");
    //}

    //protected void lmgBtnLogout_Click(object sender, ImageClickEventArgs e)
    //{
    //    Session.RemoveAll();
    //    Response.Redirect("../Login.aspx");
    //}

    protected void imgBtnSetting_Click(object sender, ImageClickEventArgs e)
    {
    }

    protected void lmgBtnChangePW_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Administrations/ChangePW.aspx");
    }
    protected void lbLogOut_Click(object sender, EventArgs e)
    {
        Session.RemoveAll();
        Response.Redirect("../Login.aspx");
    }
    protected void lbDashboard_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Dashboard.aspx");
    }
}
