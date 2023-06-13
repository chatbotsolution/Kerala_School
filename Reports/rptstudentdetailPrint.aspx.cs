using ASP;
using System;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Reports_rptstudentdetailPrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        liststudent.Visible = false;
        if (Session["printData"] == null)
            return;
        if (Session["Datalist"] != null)
        {
            DataTable dataTable = (DataTable)Session["Datalist"];
            liststudent.Visible = true;
            lblReport.Text = "";
            liststudent.DataSource = (object)dataTable;
            liststudent.DataBind();
        }
        else
            lblReport.Text = Session["printData"].ToString().Trim();
    }
}