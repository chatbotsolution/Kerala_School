using ASP;
using SanLib;
using System;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administrations_Restore : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        GetAllDatabase();
    }

    private void GetAllDatabase()
    {
        clsDAL clsDal = new clsDAL();
        dt = new DataTable();
        dt = clsDal.GetDataTable("ps_sp_get_databases");
        drpDatabase.DataSource = dt;
        drpDatabase.DataTextField = "name";
        drpDatabase.DataValueField = "name";
        drpDatabase.DataBind();
        drpDatabase.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void btnRestore_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        try
        {
            string str = "RESTORE DATABASE " + drpDatabase.SelectedValue + " From DISK='" + ("C:\\DB_Backup\\" + Restore.FileName.ToString()) + "' with replace";
            clsDal.GetDataTableQry(str);
            Lblmsg.Text = "Database Restored Successfully";
            Lblmsg.ForeColor = Color.Green;
        }
        catch (Exception ex)
        {
            Lblmsg.Text = ex.Message.ToString();
            Lblmsg.ForeColor = Color.Red;
        }
    }

    private void clear()
    {
        drpDatabase.SelectedIndex = 0;
    }
}