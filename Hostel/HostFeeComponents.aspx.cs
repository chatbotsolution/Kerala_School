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
public partial class Hostel_HostFeeComponents : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] == null)
            Response.Redirect("../Login.aspx");
        lblerr.Text = string.Empty;
        DDLFill();
        if (Request.QueryString["fid"] == null)
            return;
        FillRecordForUpdate();
    }

    protected void DDLFill()
    {
        drlPeriodicity.DataSource = new clsDAL().GetDataTable("Host_GetFeeCollPeriodicity", new Hashtable()
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
        drlPeriodicity.DataTextField = "PeriodicityType";
        drlPeriodicity.DataValueField = "PeriodicityID";
        drlPeriodicity.DataBind();
        drpAccountHead.DataSource = new clsDAL().GetDataTableQry("select AcctsHeadId, AcctsHead from dbo.Acts_AccountHeads order by AcctsHead");
        drpAccountHead.DataTextField = "AcctsHead";
        drpAccountHead.DataValueField = "AcctsHeadId";
        drpAccountHead.DataBind();
        drpAccountHead.Items.Insert(0, new ListItem("-Select-", ""));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (hdnsts.Value != "")
            hashtable.Add("@FeeID", hdnsts.Value);
        else
            hashtable.Add("@FeeID", 0);
        hashtable.Add("@FeeName", txtfeedesc.Text.Trim());
        hashtable.Add("@PeriodicityID", drlPeriodicity.SelectedValue);
        if (chkFineApplicable.Checked)
            hashtable.Add("@FineApplicable", "Y");
        else if (!chkFineApplicable.Checked)
            hashtable.Add("@FineApplicable", "N");
        if (chkConcessionApplicable.Checked)
            hashtable.Add("@ConcessionApplicable", "Y");
        else if (!chkConcessionApplicable.Checked)
            hashtable.Add("@ConcessionApplicable", "N");
        if (chkRefundable.Checked)
            hashtable.Add("@Refundable", "Y");
        else if (!chkRefundable.Checked)
            hashtable.Add("@Refundable", "N");
        hashtable.Add("@UserID", Convert.ToInt32(Session["User_Id"].ToString()));
        hashtable.Add("@SchoolID", Session["SchoolId"].ToString());
        hashtable.Add("@ActiveStatus", drpActiveSt.SelectedValue);
        hashtable.Add("@AcctsHeadId", drpAccountHead.SelectedValue.ToString().Trim());
        string str = clsDal.ExecuteScalar("Host_InsertFeeComponents", hashtable);
        if (str.Trim().ToUpper() == "S")
            Response.Redirect("HostFeeComponentList.aspx");
        else if (str.Trim().ToUpper() == "E")
        {
            lblerr.Text = "Fee Component already Exists.";
            lblerr.ForeColor = Color.Red;
            txtfeedesc.Focus();
        }
        else
        {
            lblerr.Text = "Failed to Save Data. Try Again.";
            lblerr.ForeColor = Color.Red;
            btnSave.Focus();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("HostFeeComponentList.aspx");
    }

    protected void ClearAll()
    {
        txtfeedesc.Text = string.Empty;
        chkConcessionApplicable.Checked = false;
        chkFineApplicable.Checked = false;
        chkRefundable.Checked = false;
    }

    protected void FillRecordForUpdate()
    {
        lblerr.Text = string.Empty;
        try
        {
            DataTable dataTable = new clsDAL().GetDataTable("Host_GetFeeComponents", new Hashtable()
      {
        {
           "mode",
           's'
        },
        {
           "id",
           Convert.ToInt32(Request.QueryString["fid"])
        }
      });
            txtfeedesc.Text = dataTable.Rows[0][1].ToString();
            drlPeriodicity.SelectedValue = dataTable.Rows[0]["PeriodicityID"].ToString();
            drpAccountHead.SelectedValue = dataTable.Rows[0]["AcctsHeadId"].ToString();
            chkFineApplicable.Checked = dataTable.Rows[0][3].ToString() == "Y";
            chkConcessionApplicable.Checked = dataTable.Rows[0][4].ToString() == "Y";
            chkRefundable.Checked = dataTable.Rows[0][5].ToString() == "Y";
            drpActiveSt.SelectedValue = dataTable.Rows[0]["ActiveStatus"].ToString();
            hdnsts.Value = dataTable.Rows[0][0].ToString();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}