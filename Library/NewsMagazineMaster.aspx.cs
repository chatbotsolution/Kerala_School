using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_NewsMagazineMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["User_Id"] != null)
            {
               txtMagCode.Focus();
                if (Page.IsPostBack)
                    return;
               Session["title"] = Page.Title.ToString();
               Form.DefaultButton =btnSaveAddNew.UniqueID;
                if (Request.QueryString["MagazineId"] == null)
                    return;
               ht.Clear();
               ht.Add("@MagazineId", int.Parse(Request.QueryString["MagazineId"].ToString()));
               ht.Add("@CollegeId",Session["SchoolId"]);
               dt.Clear();
               dt =obj.GetDataTable("Lib_SP_GetNewsMagazineList",ht);
                if (dt.Rows.Count <= 0)
                    return;
               txtMagCode.Text =dt.Rows[0]["MagazineCode"].ToString();
               txtMagName.Text =dt.Rows[0]["MagazineName"].ToString();
               dtpSubscibeDt.SetDateValue(Convert.ToDateTime(dt.Rows[0]["SubscriptionDate"]));
               dtpExpireDt.SetDateValue(Convert.ToDateTime(dt.Rows[0]["SubscriptionExpired"]));
               ddlPeriodicity.SelectedValue =dt.Rows[0]["Periodicity"].ToString();
               txtAmount.Text =dt.Rows[0]["TotalAmountPaid"].ToString();
               btnSaveAddNew.Text = "Update & AddNew";
               btnSaveGotoList.Text = "Update & GotoList";
            }
            else
               Response.Redirect("../Login.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    private void Clear()
    {
       txtMagCode.Text = "";
       txtMagName.Text = "";
       txtSubscribeDt.Text = "";
       txtExpDt.Text = "";
       ddlPeriodicity.SelectedIndex = 0;
       txtAmount.Text = "";
    }

    protected void btnSaveAddNew_Click(object sender, EventArgs e)
    {
       SaveData();
    }

    protected void btnSaveGotoList_Click(object sender, EventArgs e)
    {
       SaveData();
       Response.Redirect("NewsMagazineList.aspx");
    }

    private void SaveData()
    {
        try
        {
           ht.Clear();
            if (Request.QueryString["MagazineId"] == null)
               ht.Add("@MagazineId", 0);
            else if (btnSaveAddNew.Text == "Update & AddNew" ||btnSaveGotoList.Text == "Update & GotoList")
               ht.Add("@MagazineId", int.Parse(Request.QueryString["MagazineId"].ToString()));
            else
               ht.Add("@MagazineId", 0);
           ht.Add("@MagazineCode", txtMagCode.Text.Trim());
           ht.Add("@MagazineName", txtMagName.Text.Trim());
           ht.Add("@SubscriptionDate", dtpSubscibeDt.SelectedDate);
           ht.Add("@SubscriptionExpired", dtpExpireDt.SelectedDate);
           ht.Add("@Periodicity", ddlPeriodicity.SelectedValue.ToString());
           ht.Add("@TotalAmountPaid", txtAmount.Text.Trim());
           ht.Add("@CollegeId",Session["SchoolId"]);
           dt.Clear();
           dt =obj.GetDataTable("Lib_SP_InsUpdtNewsMagazine",ht);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(' Magazine Name or Code Already Exist');", true);
            }
            else
            {
                if (btnSaveAddNew.Text == "Update & AddNew" ||btnSaveGotoList.Text == "Update & GotoList")
                {
                    ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(' Data Updated Successfully');", true);
                   btnSaveAddNew.Text = "Save & AddNew";
                   btnSaveGotoList.Text = "Save & GotoList";
                }
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(' Data Saved Successfully');", true);
               Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       Clear();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
       Response.Redirect("NewsMagazineList.aspx");
    }
   
}