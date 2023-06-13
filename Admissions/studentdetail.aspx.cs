using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Admissions_studentdetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        fillclass();
        fillsession();
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        DataTable dataTable = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataSource = dataTable;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        if (dataTable.Rows.Count > 1)
            fillstudent();
        else
            drpclass.Items.Insert(0, new ListItem("No Record", "0"));
    }

    private void fillstudent()
    {
        drpstudents.Items.Clear();
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable = common.GetDataTable("ps_sp_get_StudentsForDDL");
        drpstudents.DataSource = dataTable;
        drpstudents.DataTextField = "fullname";
        drpstudents.DataValueField = "admnno";
        drpstudents.DataBind();
        if (dataTable.Rows.Count != 1)
            return;
        drpstudents.Items.Insert(0, new ListItem("No Record", "0"));
    }

    private void fillsession()
    {
        txtsession.Text = new clsGenerateFee().CreateCurrSession();
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
    }

    private void bindsinglestudent()
    {
    }

    private void bindstudentlist()
    {
    }
}