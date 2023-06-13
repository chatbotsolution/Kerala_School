using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
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

public partial class Accounts_TaxDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Request.QueryString["Id"] == null)
            return;
        fillData();
    }

    private void fillData()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = clsDal.GetDataTable("ACTS_FillTaxDetails", new Hashtable()
    {
      {
         "@TaxId",
         Request.QueryString["Id"].ToString()
      }
    });
        txtTaxCode.Text = dataTable2.Rows[0]["TaxCode"].ToString();
        txtTaxDesc.Text = dataTable2.Rows[0]["TaxDesc"].ToString();
        txtTaxRate.Text = dataTable2.Rows[0]["TaxRate"].ToString().Trim();
        dtpdt.SetDateValue(Convert.ToDateTime(dataTable2.Rows[0]["EffectiveDate"].ToString()));
    }

    private void clear()
    {
        txtTaxCode.Text = "";
        txtTaxDesc.Text = "";
        txtTaxRate.Text = "";
        txtDt.Text = "";
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        insertData();
    }

    private void insertData()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (Request.QueryString["Id"] != null)
            hashtable.Add("@TaxId", int.Parse(Request.QueryString["Id"].ToString()));
        if (txtTaxCode.Text.Trim() != "")
            hashtable.Add("@TaxCode", txtTaxCode.Text.ToString().Trim());
        if (txtTaxDesc.Text.Trim() != "")
            hashtable.Add("@TaxDesc", txtTaxDesc.Text.ToString().Trim());
        if (txtTaxRate.Text.Trim() != "")
            hashtable.Add("@TaxRate", txtTaxRate.Text.ToString().Trim());
        if (!txtDt.Text.Trim().Equals(string.Empty))
            hashtable.Add("@EffectiveDate", dtpdt.GetDateValue());
        hashtable.Add("@UserId", Session["UserId"]);
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        string str = Request.QueryString["Id"] == null ? clsDal.ExecuteScalar("ACTS_InsertTax", hashtable) : clsDal.ExecuteScalar("ACTS_UpdateTAX", hashtable);
        if (str.Trim() != "")
        {
            lblShow.Text = str;
            lblShow.ForeColor = Color.Red;
        }
        else
        {
            clear();
            tdMsg.Visible = true;
            lblShow.Visible = true;
            lblShow.Text = "Data Saved Successfully";
            lblShow.ForeColor = Color.Green;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        Response.Redirect("TaxDetailsList.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Accounts/Welcome.aspx");
    }
}