using AnthemUtility.AnthemSecurityEngine;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administrations_NewUser : System.Web.UI.Page
{
    private static string uid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
            CheckPermissions();
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
                    ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('You are not permitted. Contact your administrator'); window.location='" + "Home.aspx" + "';", true);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTable = InsertUser();
            if (dataTable.Rows.Count > 0 && dataTable.Rows[0][0].ToString() != "dup")
            {
                lblMsg.Text = "User Created Successfully. Click on Assign Permissions button to assign page permission.";
                lblMsg.ForeColor = Color.Green;
                btnpermission.Enabled = true;
                ViewState["uid"] = (object)dataTable.Rows[0][0].ToString();
                clearUser();
            }
            else
            {
                ViewState["uid"] = (object)null;
                lblMsg.Text = "User Already Exists";
                lblMsg.ForeColor = Color.Red;
                btnpermission.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private DataTable InsertUser()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        try
        {
            Hashtable hashtable = new Hashtable();
            string str = CryptoEngine.Encrypt(!(txtPW.Text != "") ? "password" : txtPW.Text.Trim(), "aGtSpL", "114208513", "SHA1", 5, "ANTHEMGLOBALTECH", 256);
            hashtable.Add((object)"@uname", (object)txtUserName.Text.Trim());
            hashtable.Add((object)"@pwd", (object)str);
            hashtable.Add((object)"@ped", (object)dtpPED.GetDateValue().ToString("MM/dd/yyyy"));
            if (chkAdminRight.Checked)
                hashtable.Add((object)"@Rights", (object)"a");
            else
                hashtable.Add((object)"@Rights", (object)"u");
            hashtable.Add((object)"@WorkStationID", (object)txtWSId.Text);
            if (rBtnYes.Checked)
                hashtable.Add((object)"@IsCashier", (object)1);
            else
                hashtable.Add((object)"@IsCashier", (object)0);
            hashtable.Add((object)"@FullName", (object)txtFullName.Text.Trim());
            hashtable.Add((object)"@Designation", (object)txtDesignation.Text.Trim());
            hashtable.Add((object)"@ContactNo", (object)txtContactNo.Text.Trim());
            hashtable.Add((object)"@SchoolID", (object)Session["SchoolId"].ToString());
            return clsDal.GetDataTable("sp_insertuser", hashtable);
        }
        catch (Exception ex)
        {
            return (DataTable)null;
        }
    }

    private void clearUser()
    {
        txtUserName.Text = "";
        txtPW.Text = "";
        txtRePw.Text = "";
        txtPED.Text = "";
    }

    protected void btnpermission_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("Permission.aspx?uid=" + ViewState["uid"].ToString());
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}