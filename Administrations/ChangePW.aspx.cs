
using AnthemUtility.AnthemSecurityEngine;
using ASP;
using Classes.DA;
using System;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administrations_ChangePW : System.Web.UI.Page
{
    private int uid;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
        {
           txtUname.Text =Session["User"].ToString();
           txtUname.ReadOnly = true;
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

    private bool CheckUser()
    {
        try
        {
            string str1 = "";
            string str2 = "";
            Common common = new Common();
            DataTable dataTable1 = new DataTable();
            string str3 = "select * from PS_USER_MASTER where USER_NAME='" +txtUname.Text.Trim() + "'";
            DataTable dataTable2 = common.ExecuteSql(str3);
            if (dataTable2.Rows.Count <= 0)
                return false;
            for (int index = 0; index < dataTable2.Rows.Count; ++index)
            {
               uid = Convert.ToInt32(dataTable2.Rows[index][0].ToString());
                str2 = dataTable2.Rows[index][1].ToString();
                str1 = dataTable2.Rows[index][2].ToString();
            }
            if (Session["User"].ToString() == str2)
            {
                string str4 = CryptoEngine.Encrypt(txtOldPass.Text.Trim(), "aGtSpL", "114208513", "SHA1", 5, "ANTHEMGLOBALTECH", 256);
               ViewState["OldPW"] = (object)str4;
                return !(str1.Trim() != str4);
            }
            ScriptManager.RegisterClientScriptBlock((Control)btnSubmit,btnSubmit.GetType(), "ShowMessage", "alert('You are not authorized');", true);
            return false;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    private void clearUser()
    {
       txtOldPass.Text = "";
       txtNewPass.Text = "";
       txtConPass.Text = "";
    }

    private bool UpdateUser()
    {
        try
        {
            new Common().ExcuteQryInsUpdt("update PS_USER_MASTER set PW='" + CryptoEngine.Encrypt(txtNewPass.Text.Trim(), "aGtSpL", "114208513", "SHA1", 5, "ANTHEMGLOBALTECH", 256) + "' where USER_ID=" + (object)uid);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtOldPass.Text.Trim() ==txtNewPass.Text.Trim())
               ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('New password should be different from the old one.');", true);
            else if (txtNewPass.Text == "sedna")
               ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('New password should be different from default password.');", true);
            else if (txtOldPass.Text.Trim() ==txtNewPass.Text.Trim())
                ScriptManager.RegisterClientScriptBlock((Control)txtNewPass,txtNewPass.GetType(), "ShowMessage", "alert('Old and new password cannot be same');", true);
            else if (CheckUser())
            {
                if (!UpdateUser())
                    return;
               clearUser();
               ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Password Modified'); window.location='" + "Home.aspx" + "';", true);
            }
            else
                ScriptManager.RegisterClientScriptBlock((Control)btnSubmit,btnSubmit.GetType(), "ShowMessage", "alert('No matching password');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       clearUser();
    }
}