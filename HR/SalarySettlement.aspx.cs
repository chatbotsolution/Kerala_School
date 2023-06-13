using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_SalarySettlement : System.Web.UI.Page
{
    private clsDAL objDAL = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (!ChkIsHRUsed())
        {
            Response.Redirect("HRHome.aspx?IsHRUsed=0");
        }
        else
        {
            string query = getquery();
            bindDrp(drpDesignation, "SELECT DesgId,Designation FROM dbo.HR_DesignationMaster ORDER BY Designation", "Designation", "DesgId");
            drpDesignation.Items.RemoveAt(0);
            drpDesignation.Items.Insert(0, new ListItem("- ALL -", "0"));
            bindDrp(drpEmployee, query, "EmpName", "EmpId");
            string str = objDAL.ExecuteScalar("HR_LastSalGenDt", new Hashtable());
            if (!(str != ""))
                return;
            dtpDisDate.From.Date =Convert.ToDateTime(str);
        }
    }

    private bool ChkIsHRUsed()
    {
        return objDAL.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    protected void bindDrp(DropDownList drp, string query, string text, string value)
    {
        DataTable dataTable = new DataTable();
        dataTable = new DataTable();
        DataTable dataTableQry = objDAL.GetDataTableQry(query);
        drp.DataSource = (object)dataTableQry;
        drp.DataTextField = text;
        drp.DataValueField = value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    protected void drpDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearFields();
        bindDrp(drpEmployee, getquery(), "EmpName", "EmpId");
        if (drpEmployee.Items.Count == 1)
            drpEmployee.Items.Clear();
        (Master.FindControl("ScriptManager1") as ScriptManager).SetFocus(drpDesignation.ClientID);
    }

    protected string getquery()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("SELECT EmpId,SevName+' ('+Mobile+')' AS EmpName FROM dbo.HR_EmployeeMaster WHERE ActiveStatus=1");
        if (drpDesignation.SelectedIndex > 0)
            stringBuilder.Append(" AND DesignationId=" + drpDesignation.SelectedValue.ToString());
        stringBuilder.Append(" ORDER BY SevName");
        return stringBuilder.ToString();
    }

    protected void drpEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearFields();
        if (drpEmployee.SelectedIndex > 0)
        {
            DataSet dataSet1 = new DataSet();
            DataSet dataSet2 = objDAL.GetDataSet("HR_GetLatestSalStructure", new Hashtable()
      {
        {
          (object) "@EmpId",
          (object) int.Parse(drpEmployee.SelectedValue.ToString())
        }
      });
            pnlSal.Enabled = true;
            if (dataSet2.Tables[0].Rows.Count > 0)
                GetSalStruct(dataSet2);
        }
        (Master.FindControl("ScriptManager1") as ScriptManager).SetFocus(drpEmployee.ClientID);
    }

    private void GetSalStruct(DataSet dsSal)
    {
        double num1 = 0.0;
        double num2 = 0.0;
        txtPay.Text = Convert.ToDecimal(dsSal.Tables[0].Rows[0]["Pay"].ToString()).ToString("0.00");
        txtGrossTot.Text = Convert.ToDecimal(dsSal.Tables[0].Rows[0]["GrossTot"].ToString()).ToString("0.00");
        Convert.ToDouble(txtDA.Text);
        grdDeductions.DataSource = (object)null;
        grdDeductions.DataBind();
        grdDeductions.DataSource = (object)dsSal.Tables[1];
        grdDeductions.DataBind();
        ViewState["deductions"] = (object)dsSal.Tables[1];
        if (dsSal.Tables[1].Rows.Count > 0)
        {
            for (int index = 0; index < dsSal.Tables[1].Rows.Count; ++index)
                num1 += double.Parse(dsSal.Tables[1].Rows[index]["AmtFromEmp"].ToString().Trim());
        }
        grdAllowance.DataSource = (object)null;
        grdAllowance.DataBind();
        grdAllowance.DataSource = (object)dsSal.Tables[2];
        grdAllowance.DataBind();
        ViewState["allowances"] = (object)dsSal.Tables[2];
        if (dsSal.Tables[2].Rows.Count > 0)
        {
            for (int index = 0; index < dsSal.Tables[2].Rows.Count; ++index)
                num2 += double.Parse(dsSal.Tables[2].Rows[index]["Amount"].ToString().Trim());
        }
        string str = objDAL.ExecuteScalarQry("SELECT SUM(RecAmt) FROM dbo.HR_EmpLoanRecovery WHERE EmpId=" + drpEmployee.SelectedValue + " AND RcvdStatus=0");
        if (str.Trim() != string.Empty)
        {
            txtTotalLoan.Text = Convert.ToDouble(str).ToString("0.00");
            num1 += Convert.ToDouble(str);
        }
        double num3 = double.Parse(dsSal.Tables[0].Rows[0]["GrossTot"].ToString()) - num1;
        txtTotalDed.Text = num1.ToString("0.00");
        txtNetPayable.Text = Convert.ToDecimal(num3.ToString()).ToString("0.00");
    }

    private void ClearFields()
    {
        lblMsg.Text = string.Empty;
        pnlSal.Enabled = false;
        txtPay.Text = "0";
        txtDA.Text = "0";
        grdAllowance.DataSource = (object)null;
        grdAllowance.DataBind();
        grdDeductions.DataSource = (object)null;
        grdDeductions.DataBind();
        txtGrossTot.Text = "0";
        txtTotalDed.Text = "0";
        txtTotalLoan.Text = "0";
        txtNetPayable.Text = "0";
        txtReason.Text = string.Empty;
        txtDisDate.Text = string.Empty;
        txtReason.Text = string.Empty;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Convert.ToDecimal(txtTotalLoan.Text) > new Decimal(0))
        {
            lblMsg.Text = "Recover the Loan Amount before Discharge.";
            lblMsg.ForeColor = Color.Red;
        }
        else if (objDAL.ExecuteScalar("HR_DischargeSettlement", new Hashtable()
    {
      {
        (object) "@EmpId",
        (object) drpEmployee.SelectedValue
      },
      {
        (object) "@LeavingDate",
        (object) Convert.ToDateTime(dtpDisDate.GetDateValue().ToString("MMM-dd-yyyy"))
      },
      {
        (object) "@ExitReason",
        (object) txtReason.Text.Trim()
      }
    }).Trim().ToUpper() == "S")
        {
            bindDrp(drpEmployee, getquery(), "EmpName", "EmpId");
            ClearFields();
            lblMsg.Text = "Data Saved Successfully";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = "Failed to Saved Data. Try Again.";
            lblMsg.ForeColor = Color.Green;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        drpDesignation.SelectedIndex = 0;
        drpEmployee.SelectedIndex = 0;
        ClearFields();
    }
}