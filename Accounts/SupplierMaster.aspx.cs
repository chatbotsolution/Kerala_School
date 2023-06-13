using AjaxControlToolkit;
using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_SupplierMaster : System.Web.UI.Page
{
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CollegeId"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Request.QueryString["suppno"] == null)
            return;
        FillData();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        InsertSupplierInfo();
        ClearFields();
    }

    private void InsertSupplierInfo()
    {
        Hashtable hashtable = new Hashtable();
        dt = new DataTable();
        if (Request.QueryString["suppno"] != null)
            hashtable.Add("SupId", Convert.ToInt32(Request.QueryString["suppno"]));
        hashtable.Add("SupName", txtSupplierName.Text.Trim());
        hashtable.Add("Address", txtAddress.Text.Trim());
        if (!txtTelNo.Text.Trim().Equals(string.Empty))
            hashtable.Add("ContactTel", txtTelNo.Text.Trim());
        if (!txtFAXNo.Text.Trim().Equals(string.Empty))
            hashtable.Add("Fax", txtFAXNo.Text.Trim());
        hashtable.Add("EmailID", txtEmailId.Text.Trim());
        hashtable.Add("CreatedOn", DateTime.Today);
        hashtable.Add("TIN", txtTIN.Text.Trim());
        hashtable.Add("CST", txtCST.Text.Trim());
        if (!txtAcBalance.Text.Trim().Equals(string.Empty))
            hashtable.Add("AcBal", txtAcBalance.Text.Trim());
        if (drpPayMethod.SelectedIndex > 0)
            hashtable.Add("CrDr", drpPayMethod.SelectedValue);
        if (!txtAcOnDate.Text.Equals(string.Empty))
            hashtable.Add("AcBalAsOnDate", txtAcOnDate.Text.Trim());
        hashtable.Add("UserId", Session["User_Id"]);
        hashtable.Add("CollegeId", Session["CollegeId"]);
        dt = obj.GetDataTable("ps_sp_insert_Supplier", hashtable);
        if (dt.Rows.Count > 0)
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('" + dt.Rows[0][0].ToString() + "');", true);
        else
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Information Saved'); window.location='" + "SupplierList.aspx" + "';", true);
    }

    private void FillData()
    {
        obj = new Common();
        DataSet dataSet1 = new DataSet();
        DataSet dataSet2 = obj.GetDataSet("ps_sp_get_SupplierForFillData", new Hashtable()
    {
      {
         "@SupId",
         Request.QueryString["suppno"]
      }
    });
        txtSupplierName.Text = dataSet2.Tables[0].Rows[0]["SupName"].ToString();
        txtAddress.Text = dataSet2.Tables[0].Rows[0]["Address"].ToString();
        txtTelNo.Text = dataSet2.Tables[0].Rows[0]["ContactTel"].ToString();
        txtFAXNo.Text = dataSet2.Tables[0].Rows[0]["Fax"].ToString();
        txtEmailId.Text = dataSet2.Tables[0].Rows[0]["EmailID"].ToString();
        txtTIN.Text = dataSet2.Tables[0].Rows[0]["TIN"].ToString();
        txtCST.Text = dataSet2.Tables[0].Rows[0]["CST"].ToString();
        txtAcBalance.Text = dataSet2.Tables[0].Rows[0]["AcBal"].ToString();
        if (!txtAcBalance.Text.Trim().Equals(string.Empty))
            rfvPmtType.Enabled = true;
        drpPayMethod.SelectedValue = dataSet2.Tables[0].Rows[0]["CrDr"].ToString();
        if (dataSet2.Tables[0].Rows[0]["AcBalAsOnDate"].ToString().Trim().Equals(string.Empty))
            return;
        txtAcOnDate.Text = dataSet2.Tables[0].Rows[0]["AcBalAsOnDate"].ToString();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearFields();
    }

    private void ClearFields()
    {
        txtSupplierName.Text = string.Empty;
        txtAddress.Text = string.Empty;
        txtTelNo.Text = string.Empty;
        txtFAXNo.Text = string.Empty;
        txtEmailId.Text = string.Empty;
        txtTIN.Text = string.Empty;
        txtCST.Text = string.Empty;
        txtAcBalance.Text = string.Empty;
        drpPayMethod.SelectedIndex = 0;
        txtAcOnDate.Text = string.Empty;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Response.Redirect("SupplierList.aspx");
    }
}