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

public partial class Masters_StudentStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
        {
           GridFill();
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
                   ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('You are not permitted. Contact your administrator'); window.location='" + "../Administrations/Home.aspx" + "';", true);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void GridFill()
    {
        int pageIndex =grdStudentStatus.PageIndex;
       grdStudentStatus.DataSource = new Common().GetDataTable("ps_sp_get_StudentStatus", new Hashtable()
    {
      {
        "id",
        0
      },
      {
        "mode",
        's'
      }
    });
       grdStudentStatus.DataBind();
       grdStudentStatus.PageIndex = pageIndex;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
       lblerr.Text = "";
        Common common = new Common();
        Hashtable ht = new Hashtable();
        if (hdnsts.Value != "")
            ht.Add("statusid", hdnsts.Value);
        else
            ht.Add("statusid", 0);
        ht.Add("status", txtDescription.Text);
        ht.Add("UserID", Convert.ToInt32(Session["User_Id"]));
        ht.Add("UserDate", DateTime.Now.ToString("MM/dd/yyyy"));
        DataTable dataTable = common.GetDataTable("ps_sp_insert_StudentStatus", ht);
        string str = "";
        if (dataTable.Rows.Count > 0)
            str = dataTable.Rows[0][0].ToString();
        if (str != "")
           lblerr.Text = str;
       GridFill();
       txtDescription.Text = "";
        if (hdnsts.Value != "")
           hdnsts.Value = "";
       UpdatePanel1.Update();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       Response.Redirect("MastersHome.aspx");
    }

    protected void grdStudentStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandName == "show"))
            return;
        try
        {
           lblerr.Text = "";
            DataTable dataTable = new Common().GetDataTable("ps_sp_get_StudentStatus", new Hashtable()
      {
        {
          "mode",
          's'
        },
        {
          "id",
          Convert.ToInt32(e.CommandArgument.ToString().Trim())
        }
      });
           txtDescription.Text = dataTable.Rows[0][1].ToString();
           hdnsts.Value = dataTable.Rows[0][0].ToString();
           GridFill();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void grdStudentStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       grdStudentStatus.PageIndex = e.NewPageIndex;
       hdnsts.Value = "";
       txtDescription.Text = "";
       GridFill();
    }

    protected void btnDelet_Click(object sender, EventArgs e)
    {
        lblerr.Text = "";
        Common common = new Common();
        Hashtable ht = new Hashtable();
        if (hdnsts.Value != "")
            ht.Add("statusid", hdnsts.Value);
        else
            ht.Add("statusid", 0);
        ht.Add("status", txtDescription.Text);
        ht.Add("UserID", Convert.ToInt32(Session["User_Id"]));
        ht.Add("UserDate", DateTime.Now.ToString("MM/dd/yyyy"));
        DataTable dataTable = common.GetDataTable("ps_sp_delete_StudentStatus", ht);
        GridFill();
        txtDescription.Text = "";
        lblerr.Text = "selected status deleted successfully";
    }
}