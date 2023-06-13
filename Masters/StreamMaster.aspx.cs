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

public partial class Masters_StreamMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
           GridFill();
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
       grdStreamMaster.DataSource =new Common().GetDataTable("ps_sp_get_StreamMaster", new Hashtable()
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
       grdStreamMaster.DataBind();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
       lblerr.Text = "";
        if (txtStream.Text.Length > 20)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnSave,btnSave.GetType(), "ShowMessage", "alert('Maximum 20 characters allowed');", true);
        }
        else
        {
            Common common = new Common();
            Hashtable ht = new Hashtable();
            if (hdnsts.Value != "")
                ht.Add((object)"streamid",hdnsts.Value);
            else
                ht.Add((object)"streamid",0);
            ht.Add((object)"description",txtStream.Text);
            DataTable dataTable = common.GetDataTable("ps_sp_insert_StreamMaster", ht);
            string str = "";
            if (dataTable.Rows.Count > 0)
                str = dataTable.Rows[0][0].ToString();
            if (str != "")
               lblerr.Text = "Already Exists";
           GridFill();
           txtStream.Text = "";
            if (!(hdnsts.Value != ""))
                return;
           hdnsts.Value = "";
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
       lblerr.Text = "";
        Common common = new Common();
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete,btnDelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str =Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
            {
                if (new Common().GetDataTable("ps_sp_Del_Stream", new Hashtable()
        {
          {
            "id",
            Convert.ToInt32(obj.ToString())
          }
        }).Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock((Control)btnDelete,btnDelete.GetType(), "ShowMessage", "alert('The stream is already in use, cannot be deleted');", true);
                    return;
                }
            }
           GridFill();
           txtStream.Text = "";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       Response.Redirect("MastersHome.aspx");
    }

    protected void grdStreamMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandName == "show"))
            return;
        try
        {
           lblerr.Text = "";
            DataTable dataTable = new Common().GetDataTable("ps_sp_get_StreamMaster", new Hashtable()
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
           txtStream.Text = dataTable.Rows[0][1].ToString();
           hdnsts.Value = dataTable.Rows[0][0].ToString();
           grdStreamMaster.DataSource =dataTable;
           grdStreamMaster.DataBind();
           GridFill();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void grdStreamMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       grdStreamMaster.PageIndex = e.NewPageIndex;
       hdnsts.Value = "";
       txtStream.Text = "";
       GridFill();
    }
}