using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_EmployeeSalaryStructure : System.Web.UI.Page
{
    private DataTable dt;
    private Hashtable ht;
    private clsDAL objDAL = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            TempTable();
            ViewState["allowances"] = string.Empty;
            ViewState["deductions"] = string.Empty;
            grdAllowance.DataSource = null;
            grdAllowance.DataBind();
            grdDeductions.DataSource = null;
            grdDeductions.DataBind();
            string query = getquery();
            bindDrp(drpDeductionType, "SELECT DedTypeId,DedDetails FROM dbo.HR_EmpDedMaster DM INNER JOIN dbo.Acts_AccountHeads AH ON AH.AcctsHeadId=DM.AcctsHeadId WHERE ah.AG_Code!=25 AND DM.EndDate IS NULL ORDER BY DedDetails", "DedDetails", "DedTypeId");
            bindDrp(drpAllowance, "SELECT AllowanceId,Allowance FROM dbo.HR_EmpAllowanceMaster WHERE StartDate IS NOT NULL AND EndDate IS NULL AND AllowanceId<>0 ORDER BY Allowance", "Allowance", "AllowanceId");
            bindDrp(drpDesignation, "SELECT DesgId,Designation FROM dbo.HR_DesignationMaster ORDER BY Designation", "Designation", "DesgId");
            bindDrp(drpEmployee, query, "EmpName", "EmpId");
            drpDesignation.Items.RemoveAt(0);
            drpDesignation.Items.Insert(0, new ListItem("- ALL -", "0"));
        }
        lblMsg.Text = "";
        divMsg.Style["Background-Color"] = "Transparent";
    }

    private void TempTable()
    {
        DataTable dataTable1 = new DataTable();
        dataTable1.Columns.Add(new DataColumn("EmpId", typeof(int)));
        dataTable1.Columns.Add(new DataColumn("DedTypeId", typeof(int)));
        dataTable1.Columns.Add(new DataColumn("DedDetails", typeof(string)));
        dataTable1.Columns.Add(new DataColumn("AmtFromEmp", typeof(double)));
        dataTable1.Columns.Add(new DataColumn("AmtFromOrg", typeof(double)));
        dataTable1.Columns.Add(new DataColumn("EmpShare", typeof(string)));
        dataTable1.Columns.Add(new DataColumn("OrgShare", typeof(string)));
        dataTable1.Columns.Add(new DataColumn("PerAmt", typeof(string)));
        dataTable1.AcceptChanges();
        ViewState["dedMod"] = dataTable1;
        DataTable dataTable2 = new DataTable();
        dataTable2.Columns.Add(new DataColumn("EmpId", typeof(int)));
        dataTable2.Columns.Add(new DataColumn("AllowanceId", typeof(int)));
        dataTable2.Columns.Add(new DataColumn("Allowance", typeof(string)));
        dataTable2.Columns.Add(new DataColumn("Amount", typeof(double)));
        dataTable2.Columns.Add(new DataColumn("Allw", typeof(string)));
        dataTable2.Columns.Add(new DataColumn("PerAmt", typeof(string)));
        dataTable2.AcceptChanges();
        ViewState["allwMod"] = dataTable2;
    }

    protected void bindDrp(DropDownList drp, string query, string text, string value)
    {
        dt = new DataTable();
        dt = objDAL.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = text;
        drp.DataValueField = value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
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

    protected void drpDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindDrp(drpEmployee, getquery(), "EmpName", "EmpId");
        (Master.FindControl("ScriptManager1") as ScriptManager).SetFocus(drpDesignation.ClientID);
    }

    protected void drpEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        clearFields();
        GetSalDtls();
        (Master.FindControl("ScriptManager1") as ScriptManager).SetFocus(drpEmployee.ClientID);
    }

    private void GetSalDtls()
    {
        if (drpEmployee.SelectedIndex <= 0)
            return;
        DataSet dataSet1 = new DataSet();
        ht = new Hashtable();
        ht.Add("@EmpId", int.Parse(drpEmployee.SelectedValue.ToString()));
        DataSet dataSet2 = objDAL.GetDataSet("HR_GetLatestSalStructure", ht);
        if (dataSet2.Tables[0].Rows.Count > 0)
        {
            fillRecentSal(dataSet2);
            if (dataSet2.Tables[3].Rows.Count > 0)
            {
                string empty = string.Empty;
                string str1 = dataSet2.Tables[3].Rows[0]["Month"].ToString();
                string str2 = dataSet2.Tables[3].Rows[0]["Year"].ToString();
                hfLastSalDt.Value = str1.ToUpper() == "JAN" || str1.ToUpper() == "MAR" || (str1.ToUpper() == "MAY" || str1.ToUpper() == "JUL") || (str1.ToUpper() == "AUG" || str1.ToUpper() == "OCT" || str1.ToUpper() == "DEC") ? "31-" + str1 + "-" + str2 : (str1.ToUpper() == "APR" || str1.ToUpper() == "JUN" || (str1.ToUpper() == "SEP" || str1.ToUpper() == "NOV") ? "30-" + str1 + "-" + str2 : (Convert.ToInt32(str2) % 4 != 0 ? "28-" + str1 + "-" + str2 : "29-" + str1 + "-" + str2));
            }
        }
        drpAllowance.Enabled = true;
        btnClearAllowance.Enabled = true;
        drpDeductionType.Enabled = true;
        btnClearDeduction.Enabled = true;
    }

    private void fillRecentSal(DataSet dsSal)
    {
        double num1 = 0.0;
        double num2 = 0.0;
        txtPay.Text = Convert.ToDecimal(dsSal.Tables[0].Rows[0]["Pay"].ToString()).ToString("0.00");
        dtpFromDate.SetDateValue(Convert.ToDateTime(dsSal.Tables[0].Rows[0]["FromDt"].ToString()));
        HiddenField hfDa = hfDA;
        TextBox txtDa = txtDA;
        Decimal num3 = Convert.ToDecimal(dsSal.Tables[0].Rows[0]["DA"].ToString());
        string str1;
        string str2 = str1 = num3.ToString("0.00");
        txtDa.Text = str1;
        string str3 = str2;
        hfDa.Value = str3;
        txtGrossTot.Text = Convert.ToDecimal(dsSal.Tables[0].Rows[0]["GrossTot"].ToString()).ToString("0.00");
        Convert.ToDouble(txtDA.Text);
        txtRemarks.Text = dsSal.Tables[0].Rows[0]["Remarks"].ToString();
        grdDeductions.DataSource = null;
        grdDeductions.DataBind();
        grdDeductions.DataSource = dsSal.Tables[1];
        grdDeductions.DataBind();
        ViewState["deductions"] = dsSal.Tables[1];
        if (dsSal.Tables[1].Rows.Count > 0)
        {
            for (int index = 0; index < dsSal.Tables[1].Rows.Count; ++index)
                num1 += double.Parse(dsSal.Tables[1].Rows[index]["AmtFromEmp"].ToString().Trim());
        }
        grdAllowance.DataSource = null;
        grdAllowance.DataBind();
        grdAllowance.DataSource = dsSal.Tables[2];
        grdAllowance.DataBind();
        grdAllw.DataSource = dsSal.Tables[2];
        grdAllw.DataBind();
        ViewState["allowances"] = dsSal.Tables[2];
        if (dsSal.Tables[2].Rows.Count > 0)
        {
            for (int index = 0; index < dsSal.Tables[2].Rows.Count; ++index)
                num2 += double.Parse(dsSal.Tables[2].Rows[index]["Amount"].ToString().Trim());
        }
        double num4 = double.Parse(dsSal.Tables[0].Rows[0]["GrossTot"].ToString()) - num1;
        txtTotalDed.Text = num1.ToString("0.00");
        txtNetPayable.Text = Convert.ToDecimal(num4.ToString()).ToString("0.00");
        hfPrevSal.Value = txtNetPayable.Text;
    }

    protected void txtPay_TextChanged(object sender, EventArgs e)
    {
        CalAllowances();
        CalDeductions();
        CalculatePay();
        txtDA.Focus();
    }

    protected void txtDA_TextChanged(object sender, EventArgs e)
    {
        if (Convert.ToDecimal(txtDA.Text) > Math.Ceiling(Convert.ToDecimal(hfDA.Value)))
            txtDA.Text = hfDA.Value;
        CalAllowances();
        CalDeductions();
        CalculatePay();
        txtDA.Focus();
    }

    protected void drpDeductionType_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnAdd.Text = "Add";
        btnCal.Visible = false;
        hfDedPerEmp.Value = "0";
        hfDedPerOrg.Value = "0";
        hfPerAmt.Value = string.Empty;
        if (drpDeductionType.SelectedIndex == 0)
        {
            txtAmtEmp.Enabled = false;
            txtAmtEmp.Text = "0";
            txtAmtOrg.Enabled = false;
            txtAmtOrg.Text = "0";
            btnAdd.Enabled = false;
        }
        else
        {
            DataTable dataTableQry = objDAL.GetDataTableQry("SELECT DedTypeId, PerAmt, EmpValue, OrgValue FROM dbo.HR_EmpDedMaster WHERE DedTypeId=" + drpDeductionType.SelectedValue + " AND EndDate IS NULL");
            if (dataTableQry.Rows[0]["PerAmt"].ToString().Trim().ToUpper() == "A")
            {
                txtAmtEmp.Enabled = true;
                txtAmtEmp.Text = dataTableQry.Rows[0]["EmpValue"].ToString();
                txtAmtOrg.Enabled = true;
                txtAmtOrg.Text = dataTableQry.Rows[0]["OrgValue"].ToString();
            }
            else if (dataTableQry.Rows[0]["PerAmt"].ToString().Trim().ToUpper() == "P")
            {
                Decimal num1 = Convert.ToDecimal(dataTableQry.Rows[0]["EmpValue"]);
                Decimal num2 = Convert.ToDecimal(txtPay.Text);
                txtAmtEmp.Text = (num2 * num1 / new Decimal(100)).ToString();
                txtAmtEmp.Enabled = true;
                Decimal num3 = Convert.ToDecimal(dataTableQry.Rows[0]["OrgValue"]);
                txtAmtOrg.Text = (num2 * num3 / new Decimal(100)).ToString();
                txtAmtOrg.Enabled = true;
                btnCal.Visible = true;
                hfDedPerEmp.Value = dataTableQry.Rows[0]["EmpValue"].ToString();
                hfDedPerOrg.Value = dataTableQry.Rows[0]["OrgValue"].ToString();
                hfPerAmt.Value = dataTableQry.Rows[0]["PerAmt"].ToString();
            }
            else
            {
                txtAmtEmp.Enabled = true;
                txtAmtEmp.Text = "0";
                txtAmtOrg.Enabled = true;
                txtAmtOrg.Text = "0";
            }
            btnAdd.Enabled = true;
        }
        drpDeductionType.Focus();
    }

    private bool checkDupDed()
    {
        bool flag = true;
        if (grdDeductions.Rows.Count > 0)
        {
            foreach (Control row in grdDeductions.Rows)
            {
                if ((row.FindControl("hdnDedTypeId") as HiddenField).Value.Equals(drpDeductionType.SelectedValue))
                    flag = false;
            }
        }
        return flag;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if ((txtAmtEmp.Text.Trim() == string.Empty || Convert.ToDouble(txtAmtEmp.Text) == 0.0) && (txtAmtOrg.Text.Trim() == string.Empty || Convert.ToDouble(txtAmtOrg.Text) == 0.0))
        {
            divMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = "Both Employee AND Employee share should not be Zero";
            txtAmtEmp.Focus();
        }
        else
        {
            if (btnAdd.Text == "Update")
            {
                btnAdd.Text = "Add";
                drpDeductionType.Enabled = true;
                DataTable dataTable = (DataTable)ViewState["deductions"];
                foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
                {
                    if (row["DedTypeId"].ToString() == ViewState["DedtypeId"].ToString())
                    {
                        row["AmtFromEmp"] = txtAmtEmp.Text.Trim();
                        row["AmtFromOrg"] = txtAmtOrg.Text.Trim();
                        if (row["PerAmt"].ToString().Trim().ToUpper() == "P")
                        {
                            row["EmpShare"] = ("(" + row["EmpValue"].ToString() + " %) " + txtAmtEmp.Text.Trim());
                            row["OrgShare"] = ("(" + row["OrgValue"].ToString() + " %) " + txtAmtOrg.Text.Trim());
                            break;
                        }
                        row["EmpShare"] = txtAmtEmp.Text.Trim();
                        row["OrgShare"] = txtAmtOrg.Text.Trim();
                        break;
                    }
                }
                dataTable.AcceptChanges();
                ViewState["deductions"] = dataTable;
                grdDeductions.DataSource = dataTable;
                grdDeductions.DataBind();
                txtAmtEmp.Text = "0";
                txtAmtOrg.Text = "0";
                drpDeductionType.SelectedIndex = 0;
                txtAmtEmp.Enabled = false;
                txtAmtOrg.Enabled = false;
                btnAdd.Enabled = false;
                CalculatePay();
            }
            else
            {
                DataTable dataTable1 = new DataTable();
                dataTable1.Columns.Add(new DataColumn("DedTypeId", typeof(int)));
                dataTable1.Columns.Add(new DataColumn("DedDetails", typeof(string)));
                dataTable1.Columns.Add(new DataColumn("AmtFromEmp", typeof(double)));
                dataTable1.Columns.Add(new DataColumn("AmtFromOrg", typeof(double)));
                dataTable1.Columns.Add(new DataColumn("EmpShare", typeof(string)));
                dataTable1.Columns.Add(new DataColumn("OrgShare", typeof(string)));
                dataTable1.Columns.Add(new DataColumn("PerAmt", typeof(string)));
                if (grdDeductions.Rows.Count > 0)
                    dataTable1 = (DataTable)ViewState["deductions"];
                if (checkDupDed())
                {
                    DataTable dataTable2 = (DataTable)ViewState["dedMod"];
                    DataTable dataTable3 = dataTable2;
                    string filterExpression = "DedTypeId=" + Convert.ToInt32(drpDeductionType.SelectedValue) + " AND AmtFromEmp=" + Convert.ToDouble(txtAmtEmp.Text.Trim());
                    foreach (DataRow dataRow in dataTable3.Select(filterExpression))
                        dataRow.Delete();
                    dataTable2.AcceptChanges();
                    ViewState["dedMod"] = dataTable2;
                    DataRow row = dataTable1.NewRow();
                    row["DedTypeId"] = Convert.ToInt32(drpDeductionType.SelectedValue.ToString());
                    row["DedDetails"] = drpDeductionType.SelectedItem.Text.Trim().ToString();
                    row["AmtFromEmp"] = Convert.ToDouble(txtAmtEmp.Text.ToString().Trim());
                    row["AmtFromOrg"] = Convert.ToDouble(txtAmtOrg.Text.ToString().Trim());
                    if (hfPerAmt.Value.ToString().Trim().ToUpper() == "P")
                    {
                        row["EmpShare"] = ("(" + hfDedPerEmp.Value.ToString() + " %) " + txtAmtEmp.Text.Trim());
                        row["OrgShare"] = ("(" + hfDedPerOrg.Value.ToString() + " %) " + txtAmtOrg.Text.Trim());
                    }
                    else
                    {
                        row["EmpShare"] = txtAmtEmp.Text.Trim();
                        row["OrgShare"] = txtAmtOrg.Text.Trim();
                    }
                    dataTable1.Rows.Add(row);
                    ViewState["deductions"] = dataTable1;
                    grdDeductions.DataSource = dataTable1;
                    grdDeductions.DataBind();
                    drpDeductionType.SelectedIndex = 0;
                    txtAmtEmp.Text = "0";
                    txtAmtOrg.Text = "0";
                    txtAmtEmp.Enabled = false;
                    txtAmtOrg.Enabled = false;
                    btnAdd.Enabled = false;
                    CalculatePay();
                }
                else
                {
                    divMsg.Style["Background-Color"] = "Red";
                    lblMsg.Text = drpDeductionType.SelectedItem.Text + " already Added";
                    drpDeductionType.Focus();
                }
            }
            drpDeductionType.Focus();
        }
    }

    protected void btnEditDeduction_Click(object sender, EventArgs e)
    {
        HiddenField control1 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hdnDedTypeId");
        HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hdnRemarks");
        HiddenField control3 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfAmtEmp");
        HiddenField control4 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfAmtOrg");
        ViewState["DedtypeId"] = control1.Value;
        if (int.Parse(objDAL.ExecuteScalarQry("select COUNT(*) from dbo.HR_EmpDedMaster DM inner join dbo.Acts_AccountHeads AH on DM.AcctsHeadId=AH.AcctsHeadId where ah.AG_Code=25 and DM.DedTypeId=" + control1.Value)) == 0)
        {
            drpDeductionType.SelectedValue = control1.Value;
            drpDeductionType.Enabled = false;
            txtAmtEmp.Text = Convert.ToDecimal(control3.Value).ToString("0.00");
            txtAmtOrg.Text = Convert.ToDecimal(control4.Value).ToString("0.00");
            btnAdd.Text = "Update";
            btnAdd.Enabled = true;
            txtAmtEmp.Enabled = true;
            txtAmtOrg.Enabled = true;
            CalculatePay();
            txtAmtEmp.Focus();
        }
        else
        {
            divMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = "Can not Edit Loan Deduction";
        }
    }

    protected void grdDeductions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = (DataTable)ViewState["deductions"];
        if (int.Parse(objDAL.ExecuteScalarQry("select COUNT(*) from dbo.HR_EmpDedMaster DM inner join dbo.Acts_AccountHeads AH on DM.AcctsHeadId=AH.AcctsHeadId where ah.AG_Code=25 and DM.DedTypeId=" + int.Parse(dataTable2.Rows[e.RowIndex]["DedTypeId"].ToString()))) == 0)
        {
            DataTable dataTable3 = (DataTable)ViewState["dedMod"];
            DataRow row = dataTable3.NewRow();
            row["AmtFromEmp"] = dataTable2.Rows[e.RowIndex]["AmtFromEmp"];
            row["AmtFromOrg"] = dataTable2.Rows[e.RowIndex]["AmtFromOrg"];
            row["DedTypeId"] = dataTable2.Rows[e.RowIndex]["DedTypeId"];
            row["DedDetails"] = dataTable2.Rows[e.RowIndex]["DedDetails"];
            dataTable3.Rows.Add(row);
            dataTable3.AcceptChanges();
            ViewState["dedMod"] = dataTable3;
            dataTable2.Rows.RemoveAt(e.RowIndex);
            grdDeductions.DataSource = dataTable2;
            grdDeductions.DataBind();
            ViewState["deductions"] = dataTable2;
            CalculatePay();
        }
        else
        {
            divMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = "Can not Delete Loan Type Deduction";
        }
    }

    protected void drpAllowance_SelectedIndexChanged(object sender, EventArgs e)
    {
        hfPerAmt.Value = string.Empty;
        hfAllwPer.Value = "0";
        if (drpAllowance.SelectedIndex == 0)
        {
            txtAllowance.Enabled = false;
            txtAllowance.Text = "0";
            btnAddAllowance.Enabled = false;
        }
        else
        {
            DataTable dataTableQry = objDAL.GetDataTableQry("SELECT AllowanceId,Value,PerAmt FROM dbo.HR_EmpAllowanceMaster WHERE AllowanceId=" + drpAllowance.SelectedValue + " AND EndDate IS NULL");
            if (dataTableQry.Rows[0]["PerAmt"].ToString().Trim().ToUpper() == "A")
            {
                txtAllowance.Enabled = true;
                txtAllowance.Text = dataTableQry.Rows[0]["Value"].ToString();
            }
            else if (dataTableQry.Rows[0]["PerAmt"].ToString().Trim().ToUpper() == "P")
            {
                MidpointRounding mode = MidpointRounding.ToEven;
                txtAllowance.Text = (Convert.ToDecimal(txtPay.Text) * Math.Round(Convert.ToDecimal(dataTableQry.Rows[0]["Value"]), mode) / new Decimal(100)).ToString();
                txtAllowance.Enabled = true;
                hfPerAmt.Value = dataTableQry.Rows[0]["PerAmt"].ToString();
                hfAllwPer.Value = dataTableQry.Rows[0]["Value"].ToString();
            }
            else
            {
                txtAllowance.Enabled = true;
                txtAllowance.Text = "0";
            }
            btnAddAllowance.Enabled = true;
        }
        drpAllowance.Focus();
    }

    private bool checkDupAllw()
    {
        bool flag = true;
        if (grdAllowance.Rows.Count > 0)
        {
            foreach (Control row in grdAllowance.Rows)
            {
                if ((row.FindControl("hdnAllowanceId") as HiddenField).Value.Equals(drpAllowance.SelectedValue))
                {
                    flag = false;
                    break;
                }
            }
        }
        return flag;
    }

    protected void btnAddAllowance_Click(object sender, EventArgs e)
    {
        if (txtAllowance.Text.Trim() == string.Empty || Convert.ToDouble(txtAllowance.Text) == 0.0)
        {
            divMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = "Allowance Amount should be greater than Zero";
            txtAllowance.Focus();
        }
        else if (btnAddAllowance.Text == "Update")
        {
            btnAddAllowance.Text = "Add";
            drpAllowance.Enabled = true;
            DataTable dataTable = (DataTable)ViewState["allowances"];
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                if (row["AllowanceId"].ToString() == ViewState["AllowanceId"].ToString())
                {
                    row["Amount"] = txtAllowance.Text.Trim();
                    row["Allw"] = !(row["PerAmt"].ToString().Trim().ToUpper() == "P") ? txtAllowance.Text.Trim() : ("(" + row["Value"].ToString() + " %) " + txtAllowance.Text.Trim());
                    break;
                }
            }
            dataTable.AcceptChanges();
            ViewState["allowances"] = dataTable;
            grdAllowance.DataSource = dataTable;
            grdAllowance.DataBind();
            txtAllowance.Text = "0";
            drpAllowance.SelectedIndex = 0;
            txtAllowance.Enabled = false;
            btnAddAllowance.Enabled = false;
            CalculatePay();
        }
        else
        {
            DataTable dataTable1 = new DataTable();
            dataTable1.Columns.Add(new DataColumn("AllowanceId", typeof(int)));
            dataTable1.Columns.Add(new DataColumn("Allowance", typeof(string)));
            dataTable1.Columns.Add(new DataColumn("Amount", typeof(double)));
            dataTable1.Columns.Add(new DataColumn("Allw", typeof(string)));
            dataTable1.Columns.Add(new DataColumn("PerAmt", typeof(string)));
            if (grdAllowance.Rows.Count > 0)
                dataTable1 = (DataTable)ViewState["allowances"];
            if (checkDupAllw())
            {
                DataTable dataTable2 = (DataTable)ViewState["allwMod"];
                DataTable dataTable3 = dataTable2;
                string filterExpression = "AllowanceId=" + Convert.ToInt32(drpAllowance.SelectedValue) + " AND Amount=" + Convert.ToDouble(txtAllowance.Text.Trim());
                foreach (DataRow dataRow in dataTable3.Select(filterExpression))
                    dataRow.Delete();
                dataTable2.AcceptChanges();
                ViewState["allwMod"] = dataTable2;
                DataRow row = dataTable1.NewRow();
                row["AllowanceId"] = Convert.ToInt32(drpAllowance.SelectedValue.ToString());
                row["Allowance"] = drpAllowance.SelectedItem.Text.Trim().ToString();
                row["Amount"] = Convert.ToDouble(txtAllowance.Text.Trim());
                row["Allw"] = !(hfPerAmt.Value.ToString().Trim().ToUpper() == "P") ? txtAllowance.Text.Trim() : ("(" + hfAllwPer.Value.ToString() + " %) " + txtAllowance.Text.Trim());
                dataTable1.Rows.Add(row);
                ViewState["allowances"] = dataTable1;
                grdAllowance.DataSource = dataTable1;
                grdAllowance.DataBind();
                drpAllowance.SelectedIndex = 0;
                txtAllowance.Text = "0";
                txtAllowance.Enabled = false;
                btnAddAllowance.Enabled = false;
                CalculatePay();
            }
            else
            {
                divMsg.Style["Background-Color"] = "Red";
                lblMsg.Text = drpAllowance.SelectedItem.Text + " already Added";
            }
            drpAllowance.Focus();
        }
    }

    protected void btnEditAllowance_Click(object sender, EventArgs e)
    {
        HiddenField control1 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hdnAllowanceId");
        HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfAmt");
        ViewState["AllowanceId"] = control1.Value;
        drpAllowance.SelectedValue = control1.Value;
        drpAllowance.Enabled = false;
        txtAllowance.Text = Convert.ToDecimal(control2.Value).ToString("0.00");
        btnAddAllowance.Text = "Update";
        btnAddAllowance.Enabled = true;
        txtAllowance.Enabled = true;
        CalculatePay();
        txtAllowance.Focus();
    }

    protected void grdAllowance_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = (DataTable)ViewState["allowances"];
        DataTable dataTable3 = (DataTable)ViewState["allwMod"];
        DataRow row = dataTable3.NewRow();
        row["Amount"] = dataTable2.Rows[e.RowIndex]["Amount"];
        row["AllowanceId"] = dataTable2.Rows[e.RowIndex]["AllowanceId"];
        row["Allowance"] = dataTable2.Rows[e.RowIndex]["Allowance"];
        dataTable3.Rows.Add(row);
        dataTable3.AcceptChanges();
        ViewState["allwMod"] = dataTable3;
        dataTable2.Rows.RemoveAt(e.RowIndex);
        grdAllowance.DataSource = dataTable2;
        grdAllowance.DataBind();
        ViewState["allowances"] = dataTable2;
        CalculatePay();
    }

    private void CalAllowances()
    {
        if (ViewState["allowances"] == null || ViewState["allowances"] == string.Empty)
            return;
        DataTable dataTable = (DataTable)ViewState["allowances"];
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
        {
            DataTable dataTableQry = objDAL.GetDataTableQry("SELECT Value,PerAmt FROM dbo.HR_EmpAllowanceMaster WHERE AllowanceId=" + row["AllowanceId"] + " AND EndDate IS NULL");
            if (dataTableQry.Rows[0]["PerAmt"].ToString().Trim().ToUpper() == "P")
            {
                Decimal num = Convert.ToDecimal(txtPay.Text) * Convert.ToDecimal(dataTableQry.Rows[0]["Value"].ToString()) / new Decimal(100);
                row["Amount"] = num.ToString("0.00");
                row["Allw"] = ("(" + row["Value"] + " %) " + row["Amount"]);
            }
        }
        dataTable.AcceptChanges();
        ViewState["allowances"] = dataTable;
        grdAllowance.DataSource = dataTable;
        grdAllowance.DataBind();
    }

    private void CalDeductions()
    {
        if (ViewState["deductions"] == null || ViewState["deductions"] == string.Empty)
            return;
        DataTable dataTable = (DataTable)ViewState["deductions"];
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
        {
            DataTable dataTableQry = objDAL.GetDataTableQry("SELECT EmpValue,OrgValue,PerAmt FROM dbo.HR_EmpDedMaster WHERE DedTypeId=" + row["DedTypeId"] + " AND EndDate IS NULL");
            if (dataTableQry.Rows[0]["PerAmt"].ToString().Trim().ToUpper() == "P")
            {
                Decimal num1 = Convert.ToDecimal(dataTableQry.Rows[0]["EmpValue"]);
                Decimal num2 = Convert.ToDecimal(txtPay.Text);
                Decimal num3 = num2 * num1 / new Decimal(100);
                row["AmtFromEmp"] = num3.ToString("0.00");
                row["EmpShare"] = ("(" + row["EmpValue"] + " %) " + Convert.ToDecimal(row["AmtFromEmp"]).ToString("0.00"));
                Decimal num4 = Convert.ToDecimal(dataTableQry.Rows[0]["OrgValue"]);
                Decimal num5 = num2 * num4 / new Decimal(100);
                row["AmtFromOrg"] = num5.ToString("0.00");
                row["OrgShare"] = ("(" + row["OrgValue"] + " %) " + Convert.ToDecimal(row["AmtFromOrg"]).ToString("0.00"));
            }
        }
        dataTable.AcceptChanges();
        ViewState["deductions"] = dataTable;
        grdDeductions.DataSource = dataTable;
        grdDeductions.DataBind();
    }

    private void CalculatePay()
    {
        divMsg.Style["Background-Color"] = (string)null;
        lblMsg.Text = string.Empty;
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = objDAL.GetDataTableQry("SELECT ISNULL(DAPer,0) AS DAPer FROM dbo.HR_EmpDAMaster WHERE ToDt IS NULL");
        if (txtPay.Text.Trim().Equals(string.Empty))
            return;
        foreach (Control row in grdAllowance.Rows)
        {
            HiddenField control = (HiddenField)row.FindControl("hfAmt");
            num2 += Convert.ToDouble(control.Value);
        }
        double num4 = Convert.ToDouble(txtPay.Text.Trim());
        if (dataTableQry.Rows.Count > 0)
            num1 = Convert.ToDouble(dataTableQry.Rows[0]["DAPer"].ToString()) / 100.0 * num4;
        double num5 = num4 + num1 + num2;
        txtDA.Text = num1.ToString();
        txtGrossTot.Text = num5.ToString("0.00");
        foreach (Control row in grdDeductions.Rows)
        {
            HiddenField control = (HiddenField)row.FindControl("hfAmtEmp");
            num3 += Convert.ToDouble(control.Value);
        }
        txtTotalDed.Text = num3.ToString("0.00");
        txtNetPayable.Text = (num5 - num3).ToString("0.00");
    }

    private string ChkSalStr()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@EffDate", dtpFromDate.GetDateValue().ToString("dd MMM yyyy"));
        hashtable.Add("@EmpId", drpEmployee.SelectedValue.Trim());
        if (rbtnModSal.Checked)
            hashtable.Add("@ModType", "EXISTING");
        else
            hashtable.Add("@ModType", "NEW");
        objDAL = new clsDAL();
        return objDAL.ExecuteScalar("HR_ChkSalStrDate", hashtable);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Convert.ToDecimal(txtPay.Text.Trim()) > new Decimal(0))
        {
            string str = ChkSalStr();
            if (str == "")
            {
                if (hfLastSalDt.Value.Trim() != string.Empty && Convert.ToDecimal(txtNetPayable.Text) < Convert.ToDecimal(hfPrevSal.Value) && Convert.ToDateTime(dtpFromDate.GetDateValue().ToString("dd-MMM-yyyy")) < Convert.ToDateTime(hfLastSalDt.Value))
                {
                    divMsg.Style["Background-Color"] = "Red";
                    lblMsg.Text = "Effective Date should be greater than " + hfLastSalDt.Value + " as Salary has already been generated for " + Convert.ToDateTime(hfLastSalDt.Value).ToString("MMM-yyyy").ToUpper();
                }
                else
                {
                    double num1 = 0.0;
                    double num2 = 0.0;
                    if (hfPrevSal.Value != string.Empty)
                        num1 = Convert.ToDouble(hfPrevSal.Value);
                    if (txtNetPayable.Text != string.Empty)
                        num2 = Convert.ToDouble(txtNetPayable.Text);
                    double num3 = num2 - num1;
                    ht = new Hashtable();
                    string empty = string.Empty;
                    ht.Add("@EmpId", int.Parse(drpEmployee.SelectedValue));
                    ht.Add("@FromDt", dtpFromDate.GetDateValue().ToString("dd-MMM-yyyy"));
                    ht.Add("@Pay", txtPay.Text.Trim());
                    ht.Add("@DA", txtDA.Text);
                    ht.Add("@GrossTot", txtGrossTot.Text);
                    ht.Add("@Remarks", txtRemarks.Text.Trim());
                    ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
                    ht.Add("@LastSalDt", hfLastSalDt.Value);
                    ht.Add("@Arrear", num3);
                    ht.Add("@EffDate", dtpFromDate.GetDateValue().ToString("01-MMM-yyyy"));
                    if (!objDAL.ExecuteScalar("HR_InsEmpSalaryStructure ", ht).Trim().Equals(string.Empty))
                        return;
                    int num4 = 0;
                    ht.Clear();
                    ht.Add("@EmpId", int.Parse(drpEmployee.SelectedValue));
                    objDAL.ExcuteProcInsUpdt("HR_DelDeduction", ht);
                    DataTable dataTable1 = new DataTable();
                    DataTable dataTable2 = new DataTable();
                    if (ViewState["deductions"] != null && ViewState["deductions"] != string.Empty)
                        dataTable1 = (DataTable)ViewState["deductions"];
                    if (ViewState["dedMod"] != null && ViewState["dedMod"] != string.Empty)
                        dataTable2 = (DataTable)ViewState["dedMod"];
                    foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
                    {
                        ht = new Hashtable();
                        ht.Add("@FromDate", Convert.ToDateTime(dtpFromDate.GetDateValue().ToString("MMM-dd-yyyy")));
                        ht.Add("@EmpId", int.Parse(drpEmployee.SelectedValue));
                        ht.Add("@DedTypeId", int.Parse(row["DedTypeId"].ToString().Trim()));
                        ht.Add("@AmtFromEmp", Convert.ToDouble(row["AmtFromEmp"].ToString().Trim()));
                        ht.Add("@AmtFromOrg", Convert.ToDouble(row["AmtFromOrg"].ToString().Trim()));
                        objDAL.ExcuteProcInsUpdt("HR_UpdtDeductionStructure", ht);
                    }
                    foreach (DataRow row in (InternalDataCollectionBase)dataTable1.Rows)
                    {
                        ht = new Hashtable();
                        ht.Add("@FromDt", Convert.ToDateTime(dtpFromDate.GetDateValue().ToString("MMM-dd-yyyy")));
                        ht.Add("@EmpId", int.Parse(drpEmployee.SelectedValue));
                        ht.Add("@DedTypeId", int.Parse(row["DedTypeId"].ToString().Trim()));
                        ht.Add("@AmtFromEmp", Convert.ToDouble(row["AmtFromEmp"].ToString().Trim()));
                        ht.Add("@AmtFromOrg", Convert.ToDouble(row["AmtFromOrg"].ToString().Trim()));
                        if (!objDAL.ExecuteScalar("HR_InsDeductionStructure", ht).Trim().Equals(string.Empty))
                            ++num4;
                    }
                    DataTable dataTable3 = new DataTable();
                    DataTable dataTable4 = new DataTable();
                    if (ViewState["allowances"] != null && ViewState["allowances"] != string.Empty)
                        dataTable3 = (DataTable)ViewState["allowances"];
                    if (ViewState["allwMod"] != null && ViewState["allwMod"] != string.Empty)
                        dataTable4 = (DataTable)ViewState["allwMod"];
                    foreach (DataRow row in (InternalDataCollectionBase)dataTable4.Rows)
                    {
                        ht = new Hashtable();
                        ht.Add("@FromDate", Convert.ToDateTime(dtpFromDate.GetDateValue().ToString("MMM-dd-yyyy")));
                        ht.Add("@EmpId", int.Parse(drpEmployee.SelectedValue));
                        ht.Add("@AllowanceId", int.Parse(row["AllowanceId"].ToString().Trim()));
                        ht.Add("@Amount", Convert.ToDouble(row["Amount"].ToString().Trim()));
                        objDAL.ExcuteProcInsUpdt("HR_UpdtAllowanceStructure", ht);
                    }
                    foreach (DataRow row in (InternalDataCollectionBase)dataTable3.Rows)
                    {
                        ht = new Hashtable();
                        ht.Add("@FromDt", Convert.ToDateTime(dtpFromDate.GetDateValue().ToString("MMM-dd-yyyy")));
                        ht.Add("@EmpId", int.Parse(drpEmployee.SelectedValue));
                        ht.Add("@AllowanceId", int.Parse(row["AllowanceId"].ToString().Trim()));
                        ht.Add("@Amount", Convert.ToDouble(row["Amount"].ToString().Trim()));
                        if (!objDAL.ExecuteScalar("HR_InsAllowanceStructure", ht).Trim().Equals(string.Empty))
                            ++num4;
                    }
                    clearAllFields();
                    divMsg.Style["Background-Color"] = "Green";
                    lblMsg.Text = "Salary Structure Saved Successfully";
                }
            }
            else if (str == "DUP")
            {
                divMsg.Style["Background-Color"] = "Red";
                lblMsg.Text = "Salary Structure Already modified on selected month";
            }
            else if (str == "EXT")
            {
                divMsg.Style["Background-Color"] = "Red";
                lblMsg.Text = "Salary Structure already defined late to this date!!";
            }
            else
            {
                divMsg.Style["Background-Color"] = "Red";
                lblMsg.Text = "Salary with arrer already generated for selected month";
            }
        }
        else
        {
            divMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = "Net Payable Amount should be more than Zero.";
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        clearAllFields();
    }

    private void clearAllFields()
    {
        hfLastSalDt.Value = string.Empty;
        hfPrevSal.Value = string.Empty;
        txtFromDt.Text = string.Empty;
        txtRemarks.Text = string.Empty;
        drpDeductionType.SelectedIndex = -1;
        drpDesignation.SelectedIndex = -1;
        bindDrp(drpEmployee, getquery(), "EmpName", "EmpId");
        clearFields();
        drpEmployee.Focus();
    }

    private void clearFields()
    {
        txtPay.Text = "0";
        divMsg.Style["Background-Color"] = "Transparent";
        lblMsg.Text = string.Empty;
        txtDA.Text = "0";
        txtGrossTot.Text = "0";
        txtNetPayable.Text = "0";
        txtTotalDed.Text = "0";
        txtRemarks.Text = string.Empty;
        drpAllowance.SelectedIndex = 0;
        drpAllowance.Enabled = false;
        txtAllowance.Text = "0";
        grdAllowance.DataSource = null;
        grdAllowance.DataBind();
        txtAllowance.Enabled = false;
        btnAddAllowance.Enabled = false;
        btnAddAllowance.Text = "Add";
        btnClearAllowance.Enabled = false;
        drpDeductionType.SelectedIndex = 0;
        drpDeductionType.Enabled = false;
        txtAmtEmp.Text = "0";
        txtAmtOrg.Text = "0";
        grdDeductions.DataSource = null;
        grdDeductions.DataBind();
        txtAmtEmp.Enabled = false;
        txtAmtOrg.Enabled = false;
        btnAdd.Enabled = false;
        btnAdd.Text = "Add";
        btnClearDeduction.Enabled = false;
        btnCal.Visible = false;
        ViewState["deductions"] = string.Empty;
        ViewState["allowances"] = string.Empty;
        if (ViewState["dedMod"] != string.Empty)
        {
            DataTable dataTable = (DataTable)ViewState["dedMod"];
            dataTable.Clear();
            ViewState["dedMod"] = dataTable;
        }
        if (ViewState["allwMod"] == string.Empty)
            return;
        DataTable dataTable1 = (DataTable)ViewState["allwMod"];
        dataTable1.Clear();
        ViewState["allwMod"] = dataTable1;
    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        btnAdd.Focus();
    }

    protected void btnClearAllowance_Click(object sender, EventArgs e)
    {
        drpAllowance.SelectedIndex = 0;
        drpAllowance.Enabled = true;
        txtAllowance.Text = "0";
        txtAllowance.Enabled = false;
        btnAddAllowance.Text = "Add";
        drpAllowance.Focus();
    }

    protected void btnClearDeduction_Click(object sender, EventArgs e)
    {
        drpDeductionType.SelectedIndex = 0;
        drpDeductionType.Enabled = true;
        txtAmtEmp.Text = "0";
        txtAmtEmp.Enabled = false;
        txtAmtOrg.Text = "0";
        txtAmtOrg.Enabled = false;
        hfDedPerEmp.Value = "0";
        hfDedPerOrg.Value = "0";
        hfPerAmt.Value = string.Empty;
        btnAdd.Text = "Add";
        grdAllw.DataSource = null;
        btnCal.Visible = false;
        drpDeductionType.Focus();
    }

    protected void rbtnNewSal_CheckedChanged(object sender, EventArgs e)
    {
        GetSalDtls();
        if (rbtnModSal.Checked)
            dtpFromDate.Enabled = false;
        else
            dtpFromDate.Enabled = false;
    }
}