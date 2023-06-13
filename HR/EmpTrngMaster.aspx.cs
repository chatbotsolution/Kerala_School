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

public partial class HR_EmpTrngMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        lblMsg.Text = "";
        if (Page.IsPostBack)
            return;
        rbtnStatus.SelectedIndex = 0;
        if (Request.QueryString["TrngId"] == null)
            return;
        fillFields();
    }

    private void fillFields()
    {
        try
        {
            Hashtable hashtable = new Hashtable();
            DataTable dataTable = new DataTable();
            hashtable.Add("@TrngId", Request.QueryString["TrngId"].ToString().Trim());
            DataTable dataTableQry = obj.GetDataTableQry("Select TrgId, TrgName, TrgPlace, TrgDetails, FromDt,ToDt,ActiveStatus from dbo.HR_EmpTrgMaster where TrgId=" + Request.QueryString["TrngId"].ToString().Trim());
            txtName.Text = dataTableQry.Rows[0]["TrgName"].ToString();
            txtPlace.Text = dataTableQry.Rows[0]["TrgPlace"].ToString();
            txtDetails.Text = dataTableQry.Rows[0]["TrgDetails"].ToString();
            dtpFrom.SetDateValue(Convert.ToDateTime(dataTableQry.Rows[0]["FromDt"].ToString()));
            dtpTo.SetDateValue(Convert.ToDateTime(dataTableQry.Rows[0]["ToDt"].ToString()));
            if (dataTableQry.Rows[0]["ActiveStatus"].ToString().Trim() == "True")
                rbtnStatus.SelectedIndex = 0;
            else
                rbtnStatus.SelectedIndex = 1;
        }
        catch (Exception ex)
        {
            lblMsg.Text = "No Details Of Such Training Found!!!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Hashtable hashtable = new Hashtable();
            if (Request.QueryString["TrngId"] != null)
                hashtable.Add("@TrngId", Request.QueryString["TrngId"].ToString().Trim());
            hashtable.Add("@TrngName", txtName.Text.Trim());
            if (txtPlace.Text.Trim() != "")
                hashtable.Add("@TrngPlace", txtPlace.Text.Trim());
            hashtable.Add("@FromDt", dtpFrom.GetDateValue().ToString("dd/MMM/yyyy"));
            hashtable.Add("@ToDt", dtpTo.GetDateValue().ToString("dd/MMM/yyyy"));
            if (txtDetails.Text.Trim() != "")
                hashtable.Add("@TrngDtls", txtDetails.Text.Trim());
            if (rbtnStatus.SelectedIndex == 0)
                hashtable.Add("@Status", 1);
            else
                hashtable.Add("@Status", 0);
            hashtable.Add("@UserId", Session["User_Id"].ToString());
            if (obj.ExecuteScalar("HR_InsUpdtEmpTraining", hashtable).Trim() == "OK")
            {
                if (Request.QueryString["empno"] != null)
                    Response.Redirect("EmpTrainingDetails.aspx?em=" + Request.QueryString["empno"].ToString());
                else if (Request.QueryString["trngNo"] != null)
                    Response.Redirect("EmpTrainingDetails.aspx?trainingno=" + Request.QueryString["trngNo"].ToString());
                else if (Request.QueryString["TrngId"] != null)
                {
                    Response.Redirect("EmpTrngList.aspx");
                }
                else
                {
                    lblMsg.Text = "Data Saved Successfully!!";
                    lblMsg.ForeColor = Color.Green;
                    clrAll();
                }
            }
            else
            {
                lblMsg.Text = "Data Already Exisits!!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.ToString();
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clrAll();
    }

    private void clrAll()
    {
        txtName.Text = "";
        txtPlace.Text = "";
        txtFrom.Text = "";
        txtTo.Text = "";
        txtDetails.Text = "";
        rbtnStatus.SelectedIndex = 0;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmpTrngList.aspx");
    }
}