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

public partial class Masters_HouseMaster : System.Web.UI.Page
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
        int pageIndex = grdHouseMaster.PageIndex;
        grdHouseMaster.DataSource = new Common().GetDataTable("ps_sp_get_HouseMaster", new Hashtable()
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
        grdHouseMaster.DataBind();
        grdHouseMaster.PageIndex = pageIndex;
        UpdatePanel1.Update();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblerr.Text = "";
        Common common = new Common();
        Hashtable ht = new Hashtable();
        if (hdnsts.Value != "")
            ht.Add("houseid", hdnsts.Value);
        else
            ht.Add("houseid", 0);
        ht.Add("housename", txtHouse.Text);
        DataTable dataTable = common.GetDataTable("ps_sp_insert_HouseMaster", ht);
        string str = "";
        if (dataTable.Rows.Count > 0)
            str = dataTable.Rows[0][0].ToString();
        if (str != "")
            lblerr.Text = "Already Exists";
        GridFill();
        txtHouse.Text = "";
        if (hdnsts.Value != "")
            hdnsts.Value = "";
        UpdatePanel1.Update();
        Page.Form.DefaultButton = btnSave.UniqueID;
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        lblerr.Text = "";
        txtHouse.Text = "";
        Common common = new Common();
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                new Common().GetDataTable("ps_sp_get_HouseMaster", new Hashtable()
        {
          {
             "id",
             Convert.ToInt32(obj.ToString())
          },
          {
             "mode",
             'd'
          }
        });
            GridFill();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Administrations/Home.aspx");
    }

    protected void grdHouseMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandName == "show"))
            return;
        try
        {
            lblerr.Text = "";
            DataTable dataTable = new Common().GetDataTable("ps_sp_get_HouseMaster", new Hashtable()
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
            txtHouse.Text = dataTable.Rows[0][1].ToString();
            hdnsts.Value = dataTable.Rows[0][0].ToString();
            grdHouseMaster.DataSource = dataTable;
            grdHouseMaster.DataBind();
            GridFill();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void grdHouseMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdHouseMaster.PageIndex = e.NewPageIndex;
        hdnsts.Value = "";
        txtHouse.Text = "";
        GridFill();
    }
}