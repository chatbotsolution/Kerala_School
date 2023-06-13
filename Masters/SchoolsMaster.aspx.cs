using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_SchoolsMaster : System.Web.UI.Page
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
        GridView1.DataSource =new Common().GetDataTable("ps_sp_get_SchoolMaster", new Hashtable()
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
        GridView1.DataBind();
    }

    protected void ClearControls()
    {
        txtcat.Text = "";
        txtdesc.Text = "";
        txteat.Text = "";
        txtedt.Text = "";
        txtlat.Text = "";
        txtldt.Text = "";
    }

    protected void btnSaveAddNew_Click(object sender, EventArgs e)
    {
        try
        {
            if (txteat.Text != "")
                Convert.ToInt64(txteat.Text);
            if (txtldt.Text != "")
                Convert.ToInt64(txtldt.Text);
            lblerr.Text = "";
            Common common = new Common();
            Hashtable ht = new Hashtable();
            if (hdnschool.Value != "")
                ht.Add((object)"schoolid",hdnschool.Value);
            else
                ht.Add((object)"schoolid",0);
            ht.Add((object)"schoolname",txtcat.Text);
            ht.Add((object)"address1",txtdesc.Text);
            ht.Add((object)"address2",txtlat.Text);
            ht.Add((object)"principalname",txtedt.Text);
            ht.Add((object)"schooltelno",txteat.Text);
            ht.Add((object)"schoolfaxno",txtldt.Text);
            ht.Add((object)"UserID",0);
            ht.Add((object)"UserDate",DateTime.Now.ToString("MM/dd/yyyy"));
            DataTable dataTable = common.GetDataTable("ps_sp_insert_SchoolMaster", ht);
            string str = "";
            if (dataTable.Rows.Count > 0)
                str = dataTable.Rows[0][0].ToString();
            if (str != "")
                lblerr.Text = "Already Exists";
            GridFill();
            ClearControls();
            if (!(hdnschool.Value != ""))
                return;
            hdnschool.Value = "";
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock((Control)txteat, txtldt.GetType(), "ShowMessage", "alert('Invalid  Telephone Number');", true);
        }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Administrations/Home.aspx");
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            lblerr.Text = "";
            DataTable dataTable = new Common().GetDataTable("ps_sp_get_SchoolMaster", new Hashtable()
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
            txtcat.Text = dataTable.Rows[0][1].ToString();
            txtdesc.Text = dataTable.Rows[0][2].ToString();
            txteat.Text = dataTable.Rows[0][5].ToString();
            txtedt.Text = dataTable.Rows[0][4].ToString();
            txtlat.Text = dataTable.Rows[0][3].ToString();
            txtldt.Text = dataTable.Rows[0][6].ToString();
            hdnschool.Value = dataTable.Rows[0][0].ToString();
            GridView1.DataSource =dataTable;
            GridView1.DataBind();
            GridFill();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridFill();
        ClearControls();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
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
                new Common().GetDataTable("ps_sp_get_SchoolMaster", new Hashtable()
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
            ClearControls();
            if (!(hdnschool.Value != ""))
                return;
            hdnschool.Value = "";
        }
    }
}