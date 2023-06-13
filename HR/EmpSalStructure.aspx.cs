using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_EmpSalStructure : System.Web.UI.Page
{
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        bindDropDown(drpDeptName, "select DeptName,DeptId from DeptMaster", "DeptName", "DeptId");
        bindDropDown(drpEmpName, "select EmpName,EmpId from EmpMaster", "EmpName", "EmpId");
        if (Request.QueryString["em"] == null)
            return;
        ht.Add("SalStrId", Request.QueryString["em"]);
        if (obj.ExecuteScalar("GetToDate", ht).ToString() != "")
            disable();
        disp();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        insertEmpSalStructure();
        clear();
    }

    private void insertEmpSalStructure()
    {
        dt = new DataTable();
        ht = new Hashtable();
        obj = new clsDAL();
        if (Request.QueryString["em"] != null)
            ht.Add("SalStrId", int.Parse(Request.QueryString["em"].ToString()));
        else
            ht.Add("SalStrId", 0);
        ht.Add("EmpId", drpEmpName.SelectedValue);
        ht.Add("FromDt", dob.GetDateValue());
        ht.Add("Pay", txtPay.Text.Trim());
        ht.Add("GP", txtGp.Text.Trim());
        ht.Add("DA", txtDa.Text.Trim());
        ht.Add("HR", txtHr.Text.Trim());
        ht.Add("Medicine", txtMedicine.Text.Trim());
        ht.Add("EPF", txtEpf.Text.Trim());
        double num1 = Convert.ToDouble(txtPay.Text.Trim().ToString()) + Convert.ToDouble(txtGp.Text);
        double num2 = Convert.ToDouble(txtDa.Text.Trim().ToString()) * num1 / 100.0;
        double num3 = Convert.ToDouble(txtHr.Text) * num1 / 100.0;
        ht.Add("GrossTotal", (num1 + num2 + num3 + Convert.ToDouble(txtMedicine.Text.Trim().ToString()) - Convert.ToDouble(txtEpf.Text.Trim().ToString())));
        ht.Add("Remarks", txtRemark.Text.Trim());
        ht.Add("UserId", Session["UserId"]);
        obj.ExecuteScalar("InsEmpSalStructure", ht);
        Response.Redirect("EmpSalStructureList.aspx");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        dt = new DataTable();
        dt = obj.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    protected void drpDeptName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDeptName.SelectedIndex <= 0)
            return;
        bindDropDown(drpEmpName, "select EmpName,EmpId from EmpMaster where DeptId=" + drpDeptName.SelectedValue.ToString() + "  order by EmpName", "EmpName", "EmpId");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        clear();
    }

    private void clear()
    {
        drpDeptName.SelectedValue = "0";
        drpEmpName.SelectedValue = "0";
        txtFrom.Text = "";
        txtPay.Text = "";
        txtGp.Text = "";
        txtDa.Text = "";
        txtHr.Text = "";
        txtMedicine.Text = "";
        txtRemark.Text = "";
        txtEpf.Text = "";
    }

    protected void btnDetaails_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmpSalStructureList.aspx");
    }

    private void disp()
    {
        obj = new clsDAL();
        ht = new Hashtable();
        dt = new DataTable();
        ht.Add("SalStrId", Request.QueryString["em"].ToString());
        dt = obj.GetDataTable("DispEmpSal", ht);
        drpDeptName.SelectedValue = dt.Rows[0]["DeptId"].ToString();
        drpEmpName.SelectedValue = dt.Rows[0]["EmpId"].ToString();
        dob.SetDateValue(Convert.ToDateTime(dt.Rows[0]["FromStr"]));
        txtPay.Text = dt.Rows[0]["Pay"].ToString();
        txtGp.Text = dt.Rows[0]["GP"].ToString();
        txtDa.Text = dt.Rows[0]["DA"].ToString();
        txtHr.Text = dt.Rows[0]["HR"].ToString();
        txtMedicine.Text = dt.Rows[0]["Medicine"].ToString();
        txtEpf.Text = dt.Rows[0]["EPF"].ToString();
        txtRemark.Text = dt.Rows[0]["Remarks"].ToString();
    }

    private void disable()
    {
        drpDeptName.Enabled = false;
        drpEmpName.Enabled = false;
        txtFrom.ReadOnly = true;
        txtPay.ReadOnly = true;
        txtGp.ReadOnly = true;
        txtDa.ReadOnly = true;
        txtHr.ReadOnly = true;
        txtMedicine.ReadOnly = true;
        txtEpf.ReadOnly = true;
        txtRemark.ReadOnly = true;
        dob.Visible = false;
    }
}