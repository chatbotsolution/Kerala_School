using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_ClassMaster : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
        {
            CheckPermissions();
            GridFill();
            BindStream();
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
        int pageIndex = grdClassMaster.PageIndex;
        grdClassMaster.DataSource = new Common().GetDataTable("ps_sp_get_ClassMaster", new Hashtable()
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
        grdClassMaster.DataBind();
        grdClassMaster.PageIndex = pageIndex;
    }

    private void BindStream()
    {
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = common.ExecuteSql("SELECT StreamID,Description FROM dbo.PS_StreamMaster ORDER BY StreamID");
        if (dataTable2.Rows.Count > 0)
        {
            drpStream.DataSource = dataTable2;
            drpStream.DataTextField = "Description";
            drpStream.DataValueField = "StreamID";
            drpStream.DataBind();
            drpStream.Items.Insert(0, new ListItem("--SELECT--", "0"));
        }
        else
        {
            drpStream.DataSource = null;
            drpStream.DataBind();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            Convert.ToInt32(txtNoOfSections.Text.Trim());
            lblerr.Text = "";
            Common common = new Common();
            Hashtable ht = new Hashtable();
            if (hdnsts.Value != "")
                ht.Add("classid", hdnsts.Value);
            else
                ht.Add("classid", 0);
            ht.Add("classname", txtClassName.Text);
            ht.Add("noofsections", txtNoOfSections.Text);
            ht.Add("UserID", Convert.ToInt32(Session["User_Id"]));
            ht.Add("UserDate", DateTime.Now.ToString("MM/dd/yyyy"));
            ht.Add("schoolid", Session["SchoolId"].ToString());
           // ht.Add("@StreamId", drpStream.SelectedValue.ToString());
            DataTable dataTable = common.GetDataTable("ps_sp_insert_ClassMaster", ht);
            string str = "";
            if (dataTable.Rows.Count > 0)
                str = dataTable.Rows[0][0].ToString();
            if (str != "")
            {
                lblerr.Text = str;
            }
            else
            {
                lblerr.Text = "Record Saved Successfully !";
                lblerr.ForeColor = Color.Green;
            }
            txtClassName.Text = "";
            txtNoOfSections.Text = "";
            GridFill();
           // drpStream.SelectedIndex = 0;
            if (!(hdnsts.Value != ""))
                return;
            hdnsts.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock((Control)txtNoOfSections, txtNoOfSections.GetType(), "ShowMassage", "alert('Invalid No. Of Sections');", true);
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        lblerr.Text = "";
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
            {
                lblerr.Text = new Common().GetDataTable("ps_sp_get_ClassMaster", new Hashtable()
        {
          {
             "id",
             Convert.ToInt32(obj.ToString())
          },
          {
             "mode",
             'd'
          }
        }).Rows[0][0].ToString();
                lblerr.ForeColor = Color.Red;
            }
            GridFill();
            txtClassName.Text = "";
            txtNoOfSections.Text = "";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MastersHome.aspx");
    }

    protected void grdClassMaster_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (!(e.CommandName == "show"))
            return;
        try
        {
            lblerr.Text = "";
            DataTable dataTable = new Common().GetDataTable("ps_sp_get_ClassMaster", new Hashtable()
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
            txtClassName.Text = dataTable.Rows[0][1].ToString();
            txtNoOfSections.Text = dataTable.Rows[0][2].ToString();
            hdnsts.Value = dataTable.Rows[0][0].ToString();
           // drpStream.SelectedValue = dataTable.Rows[0]["StreamId"].ToString();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void grdClassMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdClassMaster.PageIndex = e.NewPageIndex;
        GridFill();
        txtClassName.Text = "";
        txtNoOfSections.Text = "";
        lblerr.Text = "";
    }
}