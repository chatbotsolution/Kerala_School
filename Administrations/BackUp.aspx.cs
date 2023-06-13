
using ASP;
using SanLib;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;


public partial class Administrations_BackUp : System.Web.UI.Page
{

    DataTable dt = new DataTable();

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

        string connectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;

        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

        string database = builder.InitialCatalog;




       // dt = clsDal.GetDataTable("ps_sp_get_databases");
       // drpDatabase.DataSource = (object)dt;
        //drpDatabase.DataTextField = database;
        //drpDatabase.DataValueField = database;
        //drpDatabase.DataBind();
        drpDatabase.Items.Insert(0, new ListItem("--Select--", "0"));
        drpDatabase.Items.Insert(1, new ListItem(database, database));
    }

    protected void btnBackUp_Click(object sender, EventArgs e)
    {
        CreateDir();
        string str = "C:\\DB_Backup\\" + drpDatabase.SelectedValue.ToString().Trim() + "_" + DateTime.Now.ToString("dd-MM-yyyy-HHmm") + ".bak";
        clsDAL clsDal = new clsDAL();
        string qry = "BACKUP DATABASE " + drpDatabase.SelectedValue + " TO DISK = '" + str + "'";
        try
        {
            clsDal.ExcuteQryInsUpdt(qry);
            Lblmsg.Text = "Database BackUp Done Successfully and the backup file Saved as " + str;
            Lblmsg.ForeColor = Color.Green;
        }
        catch (Exception ex)
        {
            Lblmsg.Text = ex.Message.ToString();
            Lblmsg.ForeColor = Color.Red;
        }
    }

    private void CreateDir()
    {
        string path = "C:\\DB_Backup\\";
        try
        {
            if (Directory.Exists(path))
                return;
            Directory.CreateDirectory(path);
        }
        catch (Exception ex)
        {
        }
    }

    private void clear()
    {
        drpDatabase.SelectedIndex = 0;
    }
}