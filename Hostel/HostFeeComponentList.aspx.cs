using ASP;
using Classes.DA;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Hostel_HostFeeComponentList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
            GridFill();
        else
            Response.Redirect("../Login.aspx");
    }

    protected void GridFill()
    {
        int pageIndex = grdFeeComponent.PageIndex;
        grdFeeComponent.DataSource = new Common().GetDataTable("Host_GetFeeComponents", new Hashtable()
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
        grdFeeComponent.DataBind();
        grdFeeComponent.PageIndex = pageIndex;
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("HostFeeComponents.aspx");
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        Common common1 = new Common();
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btndelete, btndelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string[] strArray = Request["Checkb"].Split(',');
            int num = 0;
            for (int index = 0; index < strArray.Length; ++index)
            {
                Common common2 = new Common();
                int int32 = Convert.ToInt32(strArray[index].ToString());
                string str = "select count(*) from dbo.Host_FeeLedger where FeeId=" + int32;
                if (Convert.ToInt32(common2.ExecuteSql(str, "")) == 0)
                {
                    new Common().GetDataTable("Host_GetFeeComponents", new Hashtable()
          {
            {
               "id",
               int32
            },
            {
               "mode",
               'd'
            }
          });
                    new Common().ExcuteQryInsUpdt("delete Host_FeeAmount where FeeCompID=" + int32);
                }
                else
                    ++num;
            }
            GridFill();
            if (num > 0)
            {
                lblMsg.Text = num != 1 ? num.ToString() + " Records could not be deleted. The fee components are in use" : num.ToString() + " Record could not be deleted. The fee component is in use";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                lblMsg.Text = "Data Deleted Successfully !";
                lblMsg.ForeColor = Color.Green;
            }
        }
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridFill();
    }

    protected void lnkbtnAccHead_Click(object sender, EventArgs e)
    {
        GridViewRow parent = (GridViewRow)((Control)sender).Parent.Parent;
        int int32 = Convert.ToInt32(grdFeeComponent.DataKeys[parent.DataItemIndex].Value);
        DropDownList control = (DropDownList)parent.FindControl("drpAccHeads");
        if (control.SelectedIndex > 0)
        {
            clsDAL clsDal = new clsDAL();
            Hashtable hashtable = new Hashtable();
            DataTable dataTable = new DataTable();
            hashtable.Add("@FeeID", int32);
            hashtable.Add("@AcctsHeadId", control.SelectedValue.Trim());
            if (clsDal.GetDataTable("Host_UpdtAccHeadIdInFeeComp", hashtable).Rows.Count > 0)
            {
                lblMsg.Text = "Data cann't be Modified  !";
                lblMsg.ForeColor = Color.Red;
                GridFill();
            }
            else
            {
                GridFill();
                lblMsg.Text = "Data Saved Successfully !";
                lblMsg.ForeColor = Color.Green;
            }
        }
        else
        {
            lblMsg.Text = "Please Select Account Head !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void grdFeeComponent_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        DropDownList control = (DropDownList)e.Row.FindControl("drpAccHeads");
        clsDAL clsDal = new clsDAL();
        control.DataSource = clsDal.GetDataTableQry("select AcctsHeadId, AcctsHead from dbo.Acts_AccountHeads order by AcctsHead");
        control.DataTextField = "AcctsHead";
        control.DataValueField = "AcctsHeadId";
        control.DataBind();
        control.Items.Insert(0, new ListItem("-Select-", ""));
        int int32 = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "AcctsHeadId"));
        if (int32 == 0)
            return;
        control.SelectedValue = int32.ToString();
        control.BackColor = ColorTranslator.FromHtml("#E5FFCE");
    }

    protected void drpAccHeads_SelectedIndexChanged(object sender, EventArgs e)
    {
        ((Control)sender).Parent.Parent.FindControl("lnkbtnAccHead").Visible = true;
    }
}