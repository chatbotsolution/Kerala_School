using AnthemUtility.AnthemSecurityEngine;
using ASP;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Session.Abandon();
    }

    //protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    //{
    //    if (!ValidateSoftware())
    //        Response.Redirect("Error.aspx");
    //    else
    //        CheckUser();
    //}

    public bool ValidateSoftware()
    {
        clsDAL clsDal = new clsDAL();
        try
        {
            DataTable dataTable1 = new DataTable();
            DataSet dataSet = new DataSet();
            string str1 = Server.MapPath("XMLFiles") + "\\Detail.xml";
            string str2 = ConfigurationManager.AppSettings["SchoolId"].ToString().Trim();
            if (!File.Exists(str1))
                return false;
            int num = (int)dataSet.ReadXml(str1);
            if (dataSet.Tables.Count <= 0)
                return true;
            DataTable dataTable2 = new DataTable();
            DataTable table = dataSet.Tables[0];
            string str3 = clsDal.Decrypt(table.Rows[0]["SSVM_ID"].ToString(), str2);
            string str4 = clsDal.Decrypt(table.Rows[0]["SchoolName"].ToString(), str2);
            //if (DateTime.Compare(Convert.ToDateTime(clsDal.Decrypt(table.Rows[0]["ValidUpto"].ToString(), str2)), DateTime.Now) < 0 || !(str2 == str3))
            //return false;
            Session["SSVMID"] = str3;
            Session["SchoolName"] = str4;
           // Session["SchoolName"] = "LOYOLA SCHOOL,BHUBANESWAR";
            return true;
        }
        catch
        {
            return false;
        }
    }

    private void CheckUser()
    {
        try
        {
            clsDAL clsDal = new clsDAL();
            DataTable dataTable1 = new DataTable();
            Hashtable hashtable = new Hashtable();
            string str = CryptoEngine.Encrypt(txtPassword.Text.Trim(), "aGtSpL", "114208513", "SHA1", 5, "ANTHEMGLOBALTECH", 256);
            hashtable.Add("uname", txtUID.Text.Trim());
            hashtable.Add("pw", str);
            DataTable dataTable2 = clsDal.GetDataTable("SP_GetUserDetails", hashtable);
            if (dataTable2.Rows.Count > 0)
            {
                CheckPermission(Convert.ToInt32(dataTable2.Rows[0]["user_id"]));
                //Session["User"] = txtUID.Text.Trim();
                Session["User"] = dataTable2.Rows[0]["Designation"].ToString();
                Session["SchoolId"] = ConfigurationManager.AppSettings["SchoolId"].ToString();
                Session["User_Id"] = dataTable2.Rows[0]["user_id"].ToString();
                Session["LoginTime"] = DateTime.Now.ToString("dd MMM yyyy HH:mm");
                Session["userrights"] = dataTable2.Rows[0]["Rights"].ToString();
                Session.Timeout = 120;
                Response.Redirect("Dashboard.aspx");
            }
            else
                lblMsg.Text = "Invalid username or password";
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void CheckPermission(int userid)
    {
        try
        {
            DataTable dataTableQry = new clsDAL().GetDataTableQry("select * from PS_userpermissions inner join dbo.PS_PermPageMaster on USER_PERM_ID=PermID where user_id='" + userid + "' order by USER_PERM_ID");
            if (dataTableQry.Rows.Count <= 0)
                return;
            Session["permission"] = dataTableQry;
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private bool getExpireDate(DateTime dt1)
    {
        return Convert.ToDateTime(dt1.ToShortDateString()) >= Convert.ToDateTime(DateTime.Now.ToShortDateString());
    }

    protected void btnForgetPassword_Click(object sender, EventArgs e)
    {
    }

    protected void Lnkbtn_Click(object sender, EventArgs e)
    {
        
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
         if (!ValidateSoftware())
             Response.Redirect("Error.aspx");
         else
             CheckUser();
    }
}