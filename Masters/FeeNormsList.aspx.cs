using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_FeeNormsList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
        {
            FillGrid();
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

    private void FillGrid()
    {
        grdFeeNorms.DataSource =new clsDAL().GetDataTable("sp_display_FeeNormsNew", new Hashtable()
    {
      {
        "SID",
        0
      }
    });
        grdFeeNorms.DataBind();
    }

    protected void btndel_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        if (Request["Checkb"] == null)
        {
            Response.Write("<script>alert('Select a checkbox');</script>");
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                clsDal.ExecuteScalarQry("Delete from PS_FeeNormsNew where Finalised='No' and SessionID=" +Convert.ToInt32(obj.ToString()));
        }
        FillGrid();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        if (new clsDAL().ExecuteScalarQry("select top 1 finalised from dbo.PS_FeeNormsNew order by SessionID desc").ToLower() == "no")
        {
            lblMsg.Text = "You are required to Finalize the previous session, before proceeding to create a new session year.";
            lblMsg.ForeColor = Color.Red;
        }
        else
            Response.Redirect("feenoms.aspx");
    }

    protected void grdFeeNorms_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        HtmlControl control1 = (HtmlControl)e.Row.FindControl("dvEdit");
        HtmlControl control2 = (HtmlControl)e.Row.FindControl("dvShow");
        LinkButton control3 = (LinkButton)e.Row.FindControl("lnkbtnFinalise");
        if (Session["User"].ToString() != "admin")
        {
            control1.Visible = false;
            control2.Visible = true;
        }
        else
        {
            control1.Visible = true;
            control2.Visible = false;
        }
        if (Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Finalised")) == "Finalised")
            control3.Enabled = false;
        else
            control3.Enabled = true;
    }

    protected void lnkbtnFinalise_Click(object sender, EventArgs e)
    {
        new clsDAL().ExecuteScalarQry("update dbo.PS_FeeNormsNew set Finalised='Yes' where SessionID=" +Convert.ToInt32(grdFeeNorms.DataKeys[((GridViewRow)((Control)sender).Parent.Parent).DataItemIndex].Value));
        FillGrid();
    }
}