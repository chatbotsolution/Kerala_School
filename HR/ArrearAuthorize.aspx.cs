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

public partial class HR_ArrearAuthorize : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (!ChkIsHRUsed())
            {
                Response.Redirect("HRHome.aspx?IsHRUsed=0");
            }
            else
            {
                bindDropDown(drpDesignation, "select DesgId,Designation from dbo.HR_DesignationMaster where TeachingStaff<>'M' order by Designation", "Designation", "DesgId");
                bindDropDown(drpEmpName, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster Where ActiveStatus=1 ORDER BY EmpName", "EmpName", "EmpId");
                bindDropDown(drpBank, "select AcctsHeadId, AcctsHead from dbo.Acts_AccountHeads where AG_Code=7", "AcctsHead", "AcctsHeadId");
                drpBank.Items.RemoveAt(0);
                drpBank.Items.Insert(0, new ListItem("- SELECT -", "0"));
                drpPmtMode.SelectedIndex = 1;
                ViewState["PR_Id"] = null;
            }
        }
        lblMsg.Text = string.Empty;
        trMsg.BgColor = "Transparent";
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.Items.Clear();
        DataTable dataTable = new DataTable();
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        if (dataTableQry.Rows.Count <= 0)
            return;
        drp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    protected void drpEmpName_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        drpEmpName.Focus();
    }

    protected void drpDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
        if (drpDesignation.SelectedIndex > 0)
            bindDropDown(drpEmpName, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster where DesignationId=" + drpDesignation.SelectedValue + " ORDER BY EmpName", "EmpName", "EmpId");
        else
            bindDropDown(drpEmpName, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster ORDER BY EmpName", "EmpName", "EmpId");
        drpDesignation.Focus();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
        btnSearch.Focus();
    }

    private void FillGrid()
    {
        grdEmp.DataSource = null;
        grdEmp.DataBind();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpDesignation.SelectedIndex > 0)
            hashtable.Add("@DesignationId", drpDesignation.SelectedValue);
        if (drpEmpName.SelectedIndex > 0)
            hashtable.Add("@EmpId", drpEmpName.SelectedValue);
        DataTable dataTable2 = obj.GetDataTable("HR_GetEmployee", hashtable);
        grdEmp.DataSource = dataTable2;
        grdEmp.DataBind();
        if (dataTable2.Rows.Count > 0)
        {
            btnApplyToAll.Enabled = true;
            btnSave.Enabled = true;
        }
        else
        {
            btnApplyToAll.Enabled = false;
            btnSave.Enabled = false;
        }
    }

    protected void btnApplyToAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdEmp.Rows)
        {
            TextBox control1 = row.FindControl("txtDesc") as TextBox;
            TextBox control2 = row.FindControl("txtAmt") as TextBox;
            control1.Text = txtArrearDesc.Text;
            control2.Text = txtAmount.Text;
        }
        btnApplyToAll.Focus();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnSave, btnSave.GetType(), "ShowMessage", "alert('Select an Employee');", true);
            btnSave.Focus();
        }
        else if (!CheckGrid())
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnSave, btnSave.GetType(), "ShowMessage", "alert('Enter Description and Amount for at least on Employee');", true);
            grdEmp.Focus();
        }
        else if (drpPmtMode.SelectedIndex == 1 && txtBillNo.Text == string.Empty)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnSave, btnSave.GetType(), "ShowMessage", "alert('Enter Bill No');", true);
            txtBillNo.Focus();
        }
        else if (drpPmtMode.SelectedIndex == 1 && txtPaymentDate.Text.Trim() == string.Empty)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnSave, btnSave.GetType(), "ShowMessage", "alert('Enter Payment Date');", true);
            txtPaymentDate.Focus();
        }
        else if (drpPmtMode.SelectedIndex == 1 && rbPmtMode.SelectedIndex == 1 && drpBank.SelectedIndex == 0)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnSave, btnSave.GetType(), "ShowMessage", "alert('Select a Bank Name');", true);
            drpBank.Focus();
        }
        else if (drpPmtMode.SelectedIndex == 1 && rbPmtMode.SelectedIndex == 1 && txtInstrNo.Text == string.Empty)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnSave, btnSave.GetType(), "ShowMessage", "alert('Enter Instrument Nos');", true);
            txtInstrNo.Focus();
        }
        else if (!chkCashBal())
        {
            lblMsg.Text = "Cash Balance not enough to complete this transaction !!";
            lblMsg.ForeColor = Color.Red;
        }
        else
            SetStatus(Request["Checkb"]);
    }

    private bool chkCashBal()
    {
        if (!(rbPmtMode.SelectedValue.Trim() == "C") || !(drpPmtMode.SelectedValue.Trim() == "I"))
            return true;
        Hashtable hashtable = new Hashtable();
        obj = new clsDAL();
        hashtable.Add("@CurrTransDt", dtpPaymentDt.GetDateValue().ToString("dd MMM yyyy"));
        string str = obj.ExecuteScalar("ACTS_ChkCashBalance", hashtable);
        return !(str.Trim() == "") && !(Convert.ToDecimal(str.Trim()) < Convert.ToDecimal(txtAmount.Text.Trim()));
    }

    private bool CheckGrid()
    {
        bool flag = false;
        foreach (GridViewRow row in grdEmp.Rows)
        {
            Decimal num = new Decimal(0);
            TextBox control1 = (TextBox)row.FindControl("txtDesc");
            TextBox control2 = (TextBox)row.FindControl("txtAmt");
            if (control2.Text.Trim() != string.Empty)
                num = Convert.ToDecimal(control2.Text);
            if (control1.Text.Trim() != string.Empty && num > new Decimal(0))
            {
                flag = true;
                break;
            }
        }
        return flag;
    }

    private void SetStatus(string idlist)
    {
        try
        {
            string str1 = string.Empty;
            string str2 = idlist;
            char[] chArray = new char[1] { ',' };
            foreach (object obj1 in str2.Split(chArray))
            {
                foreach (GridViewRow row in grdEmp.Rows)
                {
                    Decimal num = new Decimal(0);
                    HiddenField control1 = (HiddenField)row.FindControl("hfEmpId");
                    TextBox control2 = (TextBox)row.FindControl("txtDesc");
                    TextBox control3 = (TextBox)row.FindControl("txtAmt");
                    if (control3.Text.Trim() != string.Empty)
                        num = Convert.ToDecimal(control3.Text);
                    if (control1.Value == obj1.ToString() && control2.Text.Trim() != string.Empty && num > new Decimal(0))
                    {
                        Hashtable hashtable = new Hashtable();
                        hashtable.Add("@EmpId", control1.Value);
                        hashtable.Add("@AuthDate", dtpDate.GetDateValue().ToString("dd-MMM-yyyy"));
                        hashtable.Add("@ArrearDesc", control2.Text.Trim());
                        hashtable.Add("@PaidAmt", num);
                        hashtable.Add("@SchoolId", Session["SchoolId"]);
                        hashtable.Add("@UserId", Session["User_Id"]);
                        if (ViewState["PR_Id"] != null)
                            hashtable.Add("@PR_Id", ViewState["PR_Id"]);
                        if (drpPmtMode.SelectedIndex == 0)
                        {
                            hashtable.Add("@PmtMode", "S");
                            hashtable.Add("@Remarks", "Interim Payment");
                        }
                        else
                        {
                            hashtable.Add("@PmtMode", "I");
                            hashtable.Add("@Remarks", ("Interim Payment - Bill No " + txtBillNo.Text.Trim()));
                            hashtable.Add("@PmtDate", dtpPaymentDt.GetDateValue().ToString("dd-MMM-yyyy"));
                            hashtable.Add("@BillNo", txtBillNo.Text.Trim());
                            if (rbPmtMode.SelectedIndex == 1)
                            {
                                hashtable.Add("@CrHeadId", drpBank.SelectedValue);
                                hashtable.Add("@InstNo", txtInstrNo.Text.Trim());
                                hashtable.Add("@Cash_Bank", "Bank");
                            }
                            else
                            {
                                hashtable.Add("@CrHeadId", 3);
                                hashtable.Add("@Cash_Bank", "Cash");
                            }
                        }
                        
                        str1 = obj.ExecuteScalar("HR_ArrearAuthorize", hashtable);
                        if (str1.Trim().ToUpper() != "F" && str1.Trim() != string.Empty)
                            ViewState["PR_Id"] = str1;
                    }
                }
            }
            if (str1.Trim().ToUpper() == "F")
            {
                lblMsg.Text = "Failed to Save. Try Again.";
                trMsg.BgColor = "Red";
            }
            else
            {
                lblMsg.Text = "Data Saved Successfully.";
                trMsg.BgColor = "Green";
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Failed to Save. Try Again.";
            trMsg.BgColor = "Red";
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ResetFields();
    }

    private void ResetFields()
    {
        ViewState["PR_Id"] = null;
        drpDesignation.SelectedIndex = 0;
        drpEmpName.SelectedIndex = 0;
        drpPmtMode.SelectedIndex = 1;
        txtArrearDesc.Text = string.Empty;
        txtAmount.Text = string.Empty;
        txtDate.Text = string.Empty;
        rbPmtMode.Enabled = true;
        drpBank.Enabled = true;
        txtInstrNo.Enabled = true;
        txtPaymentDate.Enabled = true;
        txtBillNo.Enabled = true;
        grdEmp.DataSource = null;
        grdEmp.DataBind();
    }

    protected void rbPmtMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpBank.SelectedIndex = 0;
        if (rbPmtMode.SelectedValue == "B")
        {
            drpBank.Enabled = true;
            txtInstrNo.Enabled = true;
            txtPaymentDate.Enabled = true;
            txtBillNo.Enabled = true;
        }
        else if (rbPmtMode.SelectedValue == "C")
        {
            drpBank.Enabled = false;
            txtInstrNo.Enabled = false;
            txtPaymentDate.Enabled = true;
            txtBillNo.Enabled = true;
        }
        rbPmtMode.Focus();
    }

    protected void drpPmtMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        rbPmtMode.SelectedIndex = 1;
        if (drpPmtMode.SelectedIndex == 0)
        {
            rbPmtMode.Enabled = false;
            drpBank.Enabled = false;
            txtInstrNo.Enabled = false;
            txtPaymentDate.Enabled = false;
            txtBillNo.Enabled = false;
        }
        else
        {
            rbPmtMode.Enabled = true;
            drpBank.Enabled = true;
            txtInstrNo.Enabled = true;
            txtPaymentDate.Enabled = true;
            txtBillNo.Enabled = true;
        }
        drpPmtMode.Focus();
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("ArrearList.aspx");
    }
}