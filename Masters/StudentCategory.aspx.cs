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

public partial class Masters_StudentCategory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        trMsg.BgColor = "Transparent";
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
        int pageIndex = grdcategory.PageIndex;
        grdcategory.DataSource =new Common().GetDataTable("ps_sp_get_CategoryMaster", new Hashtable()
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
        grdcategory.DataBind();
        grdcategory.PageIndex = pageIndex;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblerr.Text = "";
        Common common = new Common();
        Hashtable ht = new Hashtable();
        if (hdnsts.Value != "")
            ht.Add((object)"catid",hdnsts.Value);
        else
            ht.Add((object)"catid",0);
        ht.Add((object)"catname",txtCategory.Text);
        ht.Add((object)"UserID",Convert.ToInt32(Session["User_Id"]));
        ht.Add((object)"UserDate",DateTime.Now.ToString("MM/dd/yyyy"));
        ht.Add((object)"schoolid",Session["User_Id"].ToString());
        DataTable dataTable = common.GetDataTable("ps_sp_insert_CategoryMaster", ht);
        string str = "";
        if (dataTable.Rows.Count > 0)
            str = dataTable.Rows[0][0].ToString();
        if (str != "")
        {
            lblerr.Text = str;
            trMsg.BgColor = "#ad3333";
        }
        GridFill();
        txtCategory.Text = "";
        if (!(hdnsts.Value != ""))
            return;
        hdnsts.Value = "";
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        lblerr.Text = "";
        DataTable dataTable = new DataTable();
        Common common = new Common();
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str1 = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str1.Split(chArray))
                dataTable = new Common().GetDataTable("ps_sp_get_CategoryMaster", new Hashtable()
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
            string str2 = "";
            if (dataTable.Rows.Count > 0)
                str2 = dataTable.Rows[0][0].ToString();
            if (str2 != "")
            {
                lblerr.Text = str2;
                trMsg.BgColor = "#ad3333";
            }
            GridFill();
            txtCategory.Text = "";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MastersHome.aspx");
    }

    protected void grdcategory_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandName == "show"))
            return;
        try
        {
            lblerr.Text = "";
            DataTable dataTable = new Common().GetDataTable("ps_sp_get_CategoryMaster", new Hashtable()
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
            txtCategory.Text = dataTable.Rows[0][1].ToString();
            hdnsts.Value = dataTable.Rows[0][0].ToString();
            GridFill();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void grdcategory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdcategory.PageIndex = e.NewPageIndex;
        GridFill();
        txtCategory.Text = "";
    }

}