using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_SchoolMCMasterEdit : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["ReconstId"] != null)
            {
                if (Page.IsPostBack)
                    return;
                getMCMemberDetails();
            }
            else
                Response.Redirect("HRHome.aspx");
        }
        catch (Exception ex)
        {
            Response.Redirect("../Login.aspx");
        }
    }

    private void getMCMemberDetails()
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("HR_SelectSchoolMCForEdit", new Hashtable()
    {
      {
         "@ReconstId",
         Request.QueryString["ReconstId"]
      }
    });
        if (dataTable2.Rows.Count > 0)
        {
            lblApLtrNo.Text = dataTable2.Rows[0]["ApprovalLetterNo"].ToString();
            lblAprDate.Text = Convert.ToDateTime(dataTable2.Rows[0]["ApprovalDate"]).ToString("dd-MMM-yyyy");
            lblStartDt.Text = Convert.ToDateTime(dataTable2.Rows[0]["MCPeriodStartDt"]).ToString("dd-MMM-yyyy");
            lblEndDt.Text = Convert.ToDateTime(dataTable2.Rows[0]["MCPeriodEndDt"]).ToString("dd-MMM-yyyy");
            lblDesg.Text = txtDesg.Text = dataTable2.Rows[0]["Designation"].ToString();
            lblName.Text = dataTable2.Rows[0]["Name"].ToString();
            lblFromDate.Text = dataTable2.Rows[0]["FromDt"].ToString();
            lblContactNo.Text = dataTable2.Rows[0]["ContactTel"].ToString();
            lblEmail.Text = dataTable2.Rows[0]["EmailId"].ToString();
            lblAddress.Text = dataTable2.Rows[0]["Address"].ToString();
            hfDesgId.Value = dataTable2.Rows[0]["DesignationId"].ToString();
        }
        else
            Response.Redirect("HRHome.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("SchoolMCMaster.aspx");
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string str = "";
        if (txtToDt.Text.Trim() != "")
        {
            obj.ExecuteScalarQry("UPDATE dbo.HR_SchoolMCReconst SET ToDt='" + Convert.ToDateTime(dtpToDt.GetDateValue().ToString("MMM-dd-yyyy")) + "' WHERE ReconstId=" + Request.QueryString["ReconstId"]);
            if (txtName.Text.Trim() != "")
                str = !(txtFromDt.Text.Trim() == "") ? InsertUpdateTable() : "Enter From Date";
        }
        else
            str = "Enter Membership End Date";
        if (!str.Trim().Equals(string.Empty))
            litMsg.Text = "<div style='background-color:red;color:white;padding:3px;'>" + str + "</div>";
        else
            Response.Redirect("SchoolMCMaster.aspx");
    }

    private string InsertUpdateTable()
    {
        return obj.ExecuteScalar("HR_InstUpdtSchoolMCRecon", new Hashtable()
    {
      {
         "@SSVMID",
         ConfigurationManager.AppSettings["SchoolId"].ToString().Trim()
      },
      {
         "@ApprovalLetterNo",
         lblApLtrNo.Text
      },
      {
         "@ApprovalDate",
         Convert.ToDateTime(lblAprDate.Text)
      },
      {
         "@FromDt",
         Convert.ToDateTime(dtpFromDt.GetDateValue().ToString("MMM-dd-yyyy"))
      },
      {
         "@UserId",
        Session["User_Id"]
      },
      {
         "@IsApproved",
         "A"
      },
      {
         "@MCPeriodStartDt",
         Convert.ToDateTime(lblStartDt.Text)
      },
      {
         "@MCPeriodEndDt",
         Convert.ToDateTime(lblEndDt.Text)
      },
      {
         "@DesignationId",
         hfDesgId.Value
      },
      {
         "@Name",
         txtName.Text
      },
      {
         "@ContactTel",
         txtContact.Text.Trim()
      },
      {
         "@EmailId",
         txtEmail.Text.Trim()
      },
      {
         "@Address",
         txtAddress.Text.Trim()
      }
    });
    }
}