using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administrations_ModifyUser : System.Web.UI.Page
{
    private string ped = "";
    private string status;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
        {
            GetData();
            GetRecords();
            if (drpUName.Items.Count > 0)
            {
                btnModPerm.Enabled = true;
                btnResetPW.Enabled = true;
                btnModify.Enabled = true;
            }
            else
            {
                btnModPerm.Enabled = false;
                btnResetPW.Enabled = false;
                btnModify.Enabled = false;
            }
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

    protected void GetData()
    {
        try
        {
            Common common = new Common();
            DataTable dataTable = new DataTable();
            drpUName.DataSource = (object)common.GetDataTable("SP_Disp_UserMaster");
            drpUName.DataTextField = "USER_NAME";
            drpUName.DataValueField = "user_id";
            drpUName.DataBind();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    public void GetRecords()
    {
        try
        {
            Common common = new Common();
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = common.GetDataTable("SP_GetUserMaster", new Hashtable()
      {
        {
          (object) "USER_ID",
          (object) drpUName.SelectedValue.ToString()
        }
      });
            if (dataTable2.Rows.Count <= 0)
                return;
            for (int index = 0; index < dataTable2.Rows.Count; ++index)
            {
                dataTable2.Rows[index][3].ToString();
                txtPSD.Text = Convert.ToDateTime(dataTable2.Rows[index][3]).ToString("dd-MM-yyyy");
                dataTable2.Rows[index][4].ToString();
                txtPED.Text = Convert.ToDateTime(dataTable2.Rows[index][4]).ToString("dd-MM-yyyy");
                if (dataTable2.Rows[index]["Rights"].ToString().ToLower() == "a")
                {
                    chkAdmin.Checked = true;
                    btnModPerm.Enabled = false;
                }
                else
                {
                    chkAdmin.Checked = false;
                    btnModPerm.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void Getpwd()
    {
        object obj = ViewState["oldpwd"];
    }

    private void checkPermissions()
    {
        try
        {
            DataTable dataTable = Session["permission"] as DataTable;
            string str1 = Request.Url.AbsoluteUri.ToString();
            string str2 = str1.Substring(str1.LastIndexOf("/") + 1).Split('?')[0];
            if (dataTable.Select("name='" + str2 + "'").Length != 0)
                return;
            Response.Redirect("home.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private bool CheckUser()
    {
        try
        {
            Common common = new Common();
            DataTable dataTable = new DataTable();
            string str = "select * from PS_USER_MASTER where USER_ID='" + drpUName.SelectedValue.ToString().Trim() + "'";
            return common.ExecuteSql(str).Rows.Count > 0;
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            return false;
        }
    }

    private bool InsertUser()
    {
        try
        {
            
            Common common = new Common();
            Convert.ToDateTime(DateTime.Now).ToShortDateString();
            DateTime dateTime1 = !(txtPED.Text == "") ? Convert.ToDateTime(PopCalendar1.GetDateValue()) : Convert.ToDateTime("01/01/2100");
            DateTime dateTime2 = Convert.ToDateTime(PopCalendar2.GetDateValue());
            if (dateTime2 <= dateTime1)
            {
                status = !chkAdmin.Checked ? "u" : "a";
                string selectedValue = drpUName.SelectedValue;
                common.UpdateUserMod(drpUName.SelectedValue.ToString(), dateTime2.ToString("MM/dd/yyyy"), dateTime1.ToString("MM/dd/yyyy"), status.Trim());
                if (status == "a")
                    common.ExecuteSql("update PS_UserPermissions set STATUS=1 where USER_ID='" + drpUName.SelectedValue + "'");
                else
                    common.ExecuteSql("update PS_UserPermissions set STATUS=0 where USER_ID='" + drpUName.SelectedValue + "'");
                return true;
            }
            Response.Write("<script>alert('Permission End Date Should be Greater than or Equal to Permission Start Date');</script>");
            return false;
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            return false;
        }
    }

    private bool ResetUser()
    {
        try
        {
            new Common().ResetUser(drpUName.SelectedValue.ToString());
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    protected void btnsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (CheckUser())
            {
                if (!InsertUser())
                    return;
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('User Modified'); window.location='" + "Home.aspx" + "';", true);
            }
            else
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Invalid User Name or Password');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnreset_Click(object sender, EventArgs e)
    {
        if (!ResetUser())
            return;
        ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Password Reset'); window.location='" + "Home.aspx" + "';", true);
    }

    protected void btnPerm_Click(object sender, EventArgs e)
    {
        try
        {
            string absoluteUri = Request.Url.AbsoluteUri;
            Response.Redirect("Permission.aspx?uid=" + drpUName.SelectedValue + "&url=" + absoluteUri.Substring(absoluteUri.LastIndexOf("/") + 1).Split('?')[0]);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpUName_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetRecords();
    }

    private void clearUser()
    {
        txtPSD.Text = Convert.ToDateTime(DateTime.Now).ToShortDateString();
        txtPED.Text = "";
        chkAdmin.Checked = false;
        btnModPerm.Enabled = true;
    }

    protected void chkAdmin_CheckedChanged(object sender, EventArgs e)
    {
        ped = txtPED.Text.Trim();
        if (chkAdmin.Checked)
            btnModPerm.Enabled = false;
        else
            btnModPerm.Enabled = true;
    }
}