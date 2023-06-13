using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_FeeComponents : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] == null)
            Response.Redirect("../Login.aspx");
        lblerr.Text = "";
        DDLFill();
        if (Request.QueryString["fid"] == null)
            return;
        FillRecordForUpdate();
    }

    protected void DDLFill()
    {
        drlPeriodicity.DataSource =new clsDAL().GetDataTable("ps_sp_get_FeeCollPeriodicity", new Hashtable()
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
        drpAccountHead.DataSource =new clsDAL().GetDataTableQry("select AcctsHeadId, AcctsHead from dbo.Acts_AccountHeads a inner join dbo.Acts_AccountGroups ag on ag.AG_Code=a.AG_Code where (a.AG_Code=15 or ag.AG_Parent=15) order by AcctsHead");
        drpAccountHead.DataTextField = "AcctsHead";
        drpAccountHead.DataValueField = "AcctsHeadId";
        drpAccountHead.DataBind();
        drpAccountHead.Items.Insert(0, new ListItem("-Select-", ""));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        if (clsDal.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=5").ToString().Trim() == "")
        {
            ScriptManager.RegisterStartupScript((Page)this, GetType(), "alertMessage", "alert('No Account Head Set To Receive Fee!! Please Set Account Head To Continue!!'); window.location='" + Request.ApplicationPath + "/Masters/SetAccountHead.aspx'", true);
        }
        else
        {
            Hashtable ht = new Hashtable();
            if (hdnsts.Value != "")
                ht.Add((object)"feeid",hdnsts.Value);
            else
                ht.Add((object)"feeid",0);
            ht.Add((object)"feename",txtfeedesc.Text);
            ht.Add((object)"periodicity",drlPeriodicity.SelectedValue);
            if (chkFineApplicable.Checked)
                ht.Add((object)"fineapplicable","Y");
            else if (!chkFineApplicable.Checked)
                ht.Add((object)"fineapplicable","N");
            if (chkConcessionApplicable.Checked)
                ht.Add((object)"concessionapplicable","Y");
            else if (!chkConcessionApplicable.Checked)
                ht.Add((object)"concessionapplicable","N");
            if (chkRefundable.Checked)
                ht.Add((object)"refundable","Y");
            else if (!chkRefundable.Checked)
                ht.Add((object)"refundable","N");
            ht.Add((object)"UserID",Convert.ToInt32(Session["User_Id"].ToString()));
            ht.Add((object)"UserDate",DateTime.Now.ToString("MM/dd/yyyy"));
            ht.Add((object)"schoolid",Session["SchoolId"].ToString());
            ht.Add((object)"ActiveSt",drpActiveSt.SelectedValue);
            ht.Add((object)"@AcctsHeadId",drpAccountHead.SelectedValue.ToString().Trim());
            clsDal.GetDataTable("ps_sp_insert_FeeComponents", ht);
            Response.Redirect("FeeComponentList.aspx");
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("FeeComponentList.aspx");
    }

    protected void ClearAll()
    {
        txtfeedesc.Text = "";
        chkConcessionApplicable.Checked = false;
        chkFineApplicable.Checked = false;
        chkRefundable.Checked = false;
    }

    protected void FillRecordForUpdate()
    {
        lblerr.Text = "";
        try
        {
            DataTable dataTable = new clsDAL().GetDataTable("ps_sp_get_FeeComponents", new Hashtable()
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