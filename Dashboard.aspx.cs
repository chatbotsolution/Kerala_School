using ASP;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Dashboard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
            lblUser.Text = Session["User"].ToString();
        else
            Response.Redirect("Login.aspx");
    }

    //protected void imgBtnDashboard_Click(object sender, ImageClickEventArgs e)
    //{
    //    Response.Redirect("Dashboard.aspx");
    //}

    //protected void imgBtnLogout_Click(object sender, ImageClickEventArgs e)
    //{
    //    Session.Abandon(); 
    //    Response.Redirect("Login.aspx");
    //}

    //protected void imgBtnChangePW_Click(object sender, ImageClickEventArgs e)
    //{
    //    Response.Redirect("Administrations/ChangePW.aspx");
    //}




    protected void lbDashboard_Click(object sender, EventArgs e)
    {
        Response.Redirect("Dashboard.aspx");
    }
    protected void lbChangePw_Click(object sender, EventArgs e)
    {
        Response.Redirect("Administrations/ChangePW.aspx");
    }
    protected void lbLogOut_Click(object sender, EventArgs e)
    {
        Session.Abandon();
       Response.Redirect("Login.aspx");
    }
}