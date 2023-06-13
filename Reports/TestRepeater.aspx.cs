using ASP;
using SanLib;
using System;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_TestRepeater : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        FillGrid();
    }

    private void FillGrid()
    {
        DataTable dataTable = new DataTable();
        clsDAL clsDal = new clsDAL();
        dataTable.Clear();
        Repeater1.DataSource = clsDal.GetDataTable("GetNotice");
        Repeater1.DataBind();
    }
}