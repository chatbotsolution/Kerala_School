using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text;
using SanLib;

public partial class HR_SalaryAdj : System.Web.UI.Page
{
    clsDAL obj = new clsDAL();
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
                BindSalYear();
                bindDropDown(drpDesignation, "select DesgId,Designation from dbo.HR_DesignationMaster order by Designation", "Designation", "DesgId");
                bindDropDown(drpEmployee, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster ORDER BY EmpName", "EmpName", "EmpId");
            }
        }
        lblMsg.Text = String.Empty;
    }

    private bool ChkIsHRUsed()
    {
        obj = new clsDAL();

        string str = obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()");
        if (str == "0")
            return true;
        else
            return false;
    }
    private void BindSalYear()
    {
        int CurrYr = DateTime.Now.Year;
        int PrevYr = CurrYr - 1;

        drpYear.Items.Insert(0, new ListItem(CurrYr.ToString(), CurrYr.ToString()));
        drpYear.Items.Insert(1, new ListItem(PrevYr.ToString(), PrevYr.ToString()));
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dt = new DataTable();
        obj = new clsDAL();
        dt = obj.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    protected void drpDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDesignation.SelectedIndex > 0)
        {
            bindDropDown(drpEmployee, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster where DesignationId=" + drpDesignation.SelectedValue + " ORDER BY EmpName", "EmpName", "EmpId");
        }
        else
        {
            bindDropDown(drpEmployee, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster ORDER BY EmpName", "EmpName", "EmpId");
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        obj = new clsDAL();
        Hashtable ht = new Hashtable();
        DataTable dt = new DataTable();
        ht.Add("@Year", drpYear.SelectedValue.Trim());
        ht.Add("@Month", drpMonth.SelectedValue.Trim());
        ht.Add("@EmpId", drpEmployee.SelectedValue.Trim());
        dt = obj.GetDataTable("HR_GetEmpGenSal", ht);
    }

    private void getSalDtls()
    {
        if (drpEmployee.SelectedIndex > 0)
        {
            obj = new clsDAL();
            Hashtable ht = new Hashtable();
            ht.Add("@SalYear", drpYear.SelectedValue.Trim());
            ht.Add("@Month", drpMonth.SelectedValue.Trim());
            ht.Add("@EmpId", drpEmployee.SelectedValue.Trim());
            DataSet ds = new DataSet();
            ds = obj.GetDataSet("HR_GetEmpGenSal", ht);

            DataTable dtSal = new DataTable();
            dtSal = ds.Tables[0];

            DataTable dtLoan = new DataTable();

            if (dtSal.Rows.Count > 0 && dtSal.Rows[0][0].ToString().Trim() == "Paid")
            {
                lblMsg.Text = "Salary already paid for the selected employee and month!!";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                ClearAll();
            }
            else if (dtSal.Rows.Count > 0 && dtSal.Rows[0][0].ToString().Trim() == "Not")
            {
                lblMsg.Text = "Salary not generated for the selected Month!!";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                ClearAll();
            }
            else if (dtSal.Rows.Count > 0)
            {
                if (dtSal.Rows[0][0].ToString().Trim() != "")
                {
                    txtBasic.Text = Convert.ToDecimal(dtSal.Rows[0][0].ToString().Trim()).ToString("0.00");
                }
                else
                {
                    txtBasic.Text = "0";
                }
                grdEmpDed.DataSource = dtSal;
                grdEmpDed.DataBind();

                if (ds.Tables[1].Rows[0][0].ToString().Trim() == "")
                {
                    lblLoan.Text = "0";
                }
                else
                {
                    lblLoan.Text = ds.Tables[1].Rows[0][0].ToString().Trim();
                }

                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {
                    grdEmpAlw.DataSource = ds.Tables[2];
                    grdEmpAlw.DataBind();
                }
                else
                {
                    grdEmpAlw.DataSource = null;
                    grdEmpAlw.DataBind();
                }

                if (ds.Tables.Count > 3 && ds.Tables[3].Rows.Count > 0 && Convert.ToDecimal(ds.Tables[3].Rows[0][0].ToString().Trim()) > 0)
                {
                    txtClaimAmt.Visible = true;
                    lblClaimDesc.Visible = true;
                    txtClaimAmt.Text = ds.Tables[3].Rows[0][0].ToString().Trim();
                }
                else
                {
                    txtClaimAmt.Visible = false;
                    lblClaimDesc.Visible = false;
                    txtClaimAmt.Text = "0";
                }

                if (ds.Tables.Count > 4 && ds.Tables[4].Rows.Count > 0 && Convert.ToDecimal(ds.Tables[4].Rows[0][0].ToString().Trim()) > 0)
                {
                    txtEncshAmt.Visible = true;
                    lblEncashDesc.Visible = true;
                    txtEncshAmt.Text = ds.Tables[4].Rows[0][0].ToString().Trim();
                }
                else
                {
                    txtEncshAmt.Visible = false;
                    lblEncashDesc.Visible = false;
                    txtEncshAmt.Text = "0";
                }

                if (ds.Tables.Count > 5 && ds.Tables[5].Rows.Count > 0 && Convert.ToDecimal(ds.Tables[5].Rows[0][0].ToString().Trim()) > 0)
                {
                    //txtDischLoss.Visible = true;
                    //lblDis.Visible = true;
                    //txtDischLoss.Text = ds.Tables[5].Rows[0][0].ToString().Trim();
                    hfGrossTotDisc.Value = ds.Tables[5].Rows[0][0].ToString().Trim(); ;
                }
                else
                {

                    //txtDischLoss.Visible = false;
                    //lblDis.Visible = false;
                    //txtDischLoss.Text = "0";
                    hfGrossTotDisc.Value = "";
                }


                GetTotPay();
                hfSalBefChange.Value = lblPayblSal.Text.Trim();

                btnSave1.Visible = true;
                btnClear.Visible = true;
                btnCalculate.Visible = true;
                txtBasic.Enabled = true;
            }
            else
            {
                lblMsg.Text = "No Records Found !!";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                txtBasic.Enabled = false;
                btnSave1.Visible = false;
                btnClear.Visible = false;
                btnCalculate.Visible = false;
                grdEmpDed.DataSource = null;
                grdEmpDed.DataBind();
                grdEmpAlw.DataSource = null;
                grdEmpAlw.DataBind();
                lblLoan.Text = "0";
                txtBasic.Text = "0";
            }
        }
        else
        {
            ClearAll();
        }
    }

    protected void drpEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        getSalDtls();
    }

    private void GetTotPay()
    {
        decimal totDed = 0;
        decimal grossSal = 0;

        decimal Awl = 0;

        grossSal = Convert.ToDecimal(txtBasic.Text.Trim());

        decimal LossOfPay = 0;
        decimal Arrear = 0;


        foreach (GridViewRow gr in grdEmpDed.Rows)
        {
            TextBox txtDedAmt = (TextBox)gr.FindControl("txtEmpAmt") as TextBox;
            HiddenField hfId = (HiddenField)gr.FindControl("hfDedId") as HiddenField;
            HiddenField hfArrer = (HiddenField)gr.FindControl("hfArrer") as HiddenField;

            string ChkDedAmt = txtDedAmt.Text;
            string ChkHfId = hfId.Value;
            string ChkHfAer = hfArrer.Value;


            //*Note Arrer deductions are not directed from salary as salary arrer is caluclated by deducting this amount

            if (hfId.Value.Trim() != "0" && hfArrer.Value.Trim().ToUpper() != "A") // hfArrer value is feched as A for deduction arrer
            {
                totDed = totDed + Convert.ToDecimal(txtDedAmt.Text.Trim());
            }
            else if (Convert.ToDecimal(hfId.Value.Trim()) == 0 && hfArrer.Value.Trim().ToUpper() != "A")
            {
                LossOfPay = LossOfPay + Convert.ToDecimal(txtDedAmt.Text.Trim());
            }
        }



        foreach (GridViewRow gr in grdEmpAlw.Rows)
        {
            TextBox txtAlw = (TextBox)gr.FindControl("txtAlwAmt") as TextBox;
            HiddenField hfId = (HiddenField)gr.FindControl("hfAlwId") as HiddenField;
            if (hfId.Value.Trim() != "0")
            {
                Awl = Awl + Convert.ToDecimal(txtAlw.Text.Trim());
            }
            else
            {
                Arrear = Arrear + Convert.ToDecimal(txtAlw.Text.Trim());
            }
        }
        //paybleSal=grossSal-(total deduction with employee share)-Loan Amount

        grossSal = grossSal + Awl;

        if (txtClaimAmt.Text.Trim() != "" && Convert.ToDecimal(txtClaimAmt.Text.Trim()) > 0)
        {
            grossSal = Math.Round(grossSal + Convert.ToDecimal(txtClaimAmt.Text.Trim()));
        }

        if (txtEncshAmt.Text.Trim() != "" && Convert.ToDecimal(txtEncshAmt.Text.Trim()) > 0)
        {
            grossSal = Math.Round(grossSal + Convert.ToDecimal(txtEncshAmt.Text.Trim()));
        }

        grossSal = Math.Round(grossSal - LossOfPay);

        if (hfGrossTotDisc.Value.ToString().Trim() != "" && Convert.ToDecimal(hfGrossTotDisc.Value.ToString().Trim()) > 0)
        {
            if (txtDischLoss.Text.Trim() == "" || Convert.ToDecimal(txtDischLoss.Text.Trim()) == 0)
            {
                txtDischLoss.Text = Math.Round(grossSal - Convert.ToDecimal(hfGrossTotDisc.Value.ToString().Trim())).ToString();
            }
            grossSal = Math.Round(grossSal - Convert.ToDecimal(txtDischLoss.Text.Trim()));
            lblDis.Visible = true;
            txtDischLoss.Visible = true;
        }
        else
        {
            lblDis.Visible = false;
            txtDischLoss.Visible = false;
        }

        decimal totPay = 0;
        if (lblLoan.Text.Trim() != "")
        {
            totDed = totDed + Convert.ToDecimal(lblLoan.Text.Trim());
        }

        totPay = Math.Round(grossSal - totDed);

        totPay = totPay + Arrear;

        lblPayblSal.Text = totPay.ToString("0.00");
    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        GetTotPay();
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        GetTotPay();

        string msg = "";

        decimal totDedAmt = 0;
        decimal grossSal = 0;

        decimal LossOfPay = 0;
        decimal Arrear = 0;
        decimal Awl = 0;

        string DedAmtList = String.Empty;
        string DedEmplrAmtList = String.Empty;
        string DedIdList = String.Empty;
        string DedAccId = String.Empty;
        string DedDtls = String.Empty;

        string DedAmtArrerEmpLst = String.Empty;
        string DedAmtArrerOrgLst = String.Empty;
        string DedArrerIdList = String.Empty;
        string DedArrerAccIdLst = String.Empty;
        string DedDtlsArr = String.Empty;

        string AlwAmt = String.Empty;
        string AlwIdList = String.Empty;

        string salMonth = drpMonth.SelectedValue;
        string salYear = drpYear.SelectedValue;

        grossSal = Convert.ToDecimal(txtBasic.Text.Trim());

        foreach (GridViewRow gr in grdEmpAlw.Rows)
        {
            HiddenField hfAlwId = (HiddenField)gr.FindControl("hfAlwId");
            TextBox txtAlwAmount = (TextBox)gr.FindControl("txtAlwAmt");
            if (hfAlwId.Value.Trim() != "0")
            {
                Awl = Awl + Convert.ToDecimal(txtAlwAmount.Text.Trim());
            }
            else
            {
                Arrear = Arrear + Convert.ToDecimal(txtAlwAmount.Text.Trim());
            }

            if (AlwIdList.Trim() == String.Empty)
                AlwIdList = hfAlwId.Value;
            else
                AlwIdList = AlwIdList + "," + hfAlwId.Value;

            if (AlwAmt.Trim() == String.Empty)
                AlwAmt = Convert.ToDecimal(txtAlwAmount.Text).ToString("0.00") + ",";
            else
                AlwAmt = AlwAmt + Convert.ToDecimal(txtAlwAmount.Text).ToString("0.00") + ",";

        }

        int count = 0;

        foreach (GridViewRow gr in grdEmpDed.Rows)
        {
            HiddenField hfDedId = (HiddenField)gr.FindControl("hfDedId");
            HiddenField hfArrer = (HiddenField)gr.FindControl("hfArrer");
            TextBox txtDedAmount = (TextBox)gr.FindControl("txtEmpAmt");
            TextBox txtDedEmplrAmt = (TextBox)gr.FindControl("txtEmplrAmt");
            Label lblDedDesc = (Label)gr.FindControl("lblDedDtls");
            HiddenField hfDtls = (HiddenField)gr.FindControl("hfDedDtlsOrgnl");


            HiddenField hfAccId = (HiddenField)gr.FindControl("hfAccId");

            //*Note Arrer deductions are not directed from salary as salary arrer is caluclated by deducting this amount

            if (hfDedId.Value.Trim() != "0" && hfArrer.Value.Trim().ToUpper() != "A")
            {
                totDedAmt = totDedAmt + Convert.ToDecimal(txtDedAmount.Text.Trim());
            }
            else if (Convert.ToDecimal(hfDedId.Value.Trim()) == 0 && hfArrer.Value.Trim().ToUpper() != "A")
            {
                LossOfPay = LossOfPay + Convert.ToDecimal(txtDedAmount.Text.Trim());
            }

            if (hfArrer.Value.Trim().ToUpper() != "A")
            {
                if (DedAmtList.Trim() == String.Empty)
                    DedAmtList = Convert.ToDecimal(txtDedAmount.Text).ToString("0.00") + ",";
                else
                    DedAmtList = DedAmtList + Convert.ToDecimal(txtDedAmount.Text).ToString("0.00") + ",";

                if (DedEmplrAmtList.Trim() == String.Empty)
                    DedEmplrAmtList = Convert.ToDecimal(txtDedEmplrAmt.Text).ToString("0.00") + ",";
                else
                    DedEmplrAmtList = DedEmplrAmtList + Convert.ToDecimal(txtDedEmplrAmt.Text).ToString("0.00") + ",";

                if (DedIdList.Trim() == String.Empty)
                    DedIdList = hfDedId.Value;
                else
                    DedIdList = DedIdList + "," + hfDedId.Value;

                if (DedAccId.Trim() == String.Empty)
                {
                    if (hfAccId.Value.Trim() != "")
                    {
                        DedAccId = hfAccId.Value;
                    }
                    else
                    {
                        DedAccId = "0";
                    }
                }
                else
                {
                    if (hfAccId.Value.Trim() != "")
                    {
                        DedAccId = DedAccId + "," + hfAccId.Value;
                    }
                    else
                    {
                        DedAccId = DedAccId + ",0";
                    }
                }

                if (DedDtls.Trim() == String.Empty)
                    DedDtls = lblDedDesc.Text.Trim();
                else
                    DedDtls = DedDtls + "," + lblDedDesc.Text.Trim();
            }
            else // If deduction arrer exists then create a separate list
            {
                if (DedAmtArrerEmpLst.Trim() == String.Empty)
                    DedAmtArrerEmpLst = Convert.ToDecimal(txtDedAmount.Text).ToString("0.00") + ",";
                else
                    DedAmtArrerEmpLst = DedAmtArrerEmpLst + Convert.ToDecimal(txtDedAmount.Text).ToString("0.00") + ",";

                if (DedAmtArrerOrgLst.Trim() == String.Empty)
                    DedAmtArrerOrgLst = Convert.ToDecimal(txtDedEmplrAmt.Text).ToString("0.00") + ",";
                else
                    DedAmtArrerOrgLst = DedAmtArrerOrgLst + Convert.ToDecimal(txtDedEmplrAmt.Text).ToString("0.00") + ",";

                if (DedArrerIdList.Trim() == String.Empty)
                    DedArrerIdList = hfDedId.Value;
                else
                    DedArrerIdList = DedArrerIdList + "," + hfDedId.Value;

                if (DedArrerAccIdLst.Trim() == String.Empty)
                {
                    if (hfAccId.Value.Trim() != "")
                    {
                        DedArrerAccIdLst = hfAccId.Value;
                    }
                    else
                    {
                        DedArrerAccIdLst = "0";
                    }
                }
                else
                {
                    if (hfAccId.Value.Trim() != "")
                    {
                        DedArrerAccIdLst = DedArrerAccIdLst + "," + hfAccId.Value;
                    }
                    else
                    {
                        DedArrerAccIdLst = DedArrerAccIdLst + ",0";
                    }
                }

                if (DedDtlsArr.Trim() == String.Empty)
                    DedDtlsArr = hfDtls.Value.Trim();
                else
                    DedDtlsArr = DedDtlsArr + "," + hfDtls.Value.Trim();
            }
            count = count + 1;
        }

        grossSal = grossSal + Awl;

        if (txtClaimAmt.Text.Trim() != "" && Convert.ToDecimal(txtClaimAmt.Text.Trim()) > 0)
        {
            grossSal = Math.Round(grossSal + Convert.ToDecimal(txtClaimAmt.Text.Trim()));
        }

        if (txtEncshAmt.Text.Trim() != "" && Convert.ToDecimal(txtEncshAmt.Text.Trim()) > 0)
        {
            grossSal = Math.Round(grossSal + Convert.ToDecimal(txtEncshAmt.Text.Trim()));
        }

        grossSal = Math.Round(grossSal - LossOfPay);

        if (hfGrossTotDisc.Value.ToString().Trim() != "" && Convert.ToDecimal(hfGrossTotDisc.Value.ToString().Trim()) > 0)
        {
            if (txtDischLoss.Text.Trim() == "" || Convert.ToDecimal(txtDischLoss.Text.Trim()) == 0)
            {
                txtDischLoss.Text = Math.Round(grossSal - Convert.ToDecimal(hfGrossTotDisc.Value.ToString().Trim())).ToString();
            }
            grossSal = Math.Round(grossSal - Convert.ToDecimal(txtDischLoss.Text.Trim()));
        }

        Hashtable ht = new Hashtable();
        ht.Add("@SalMonth", drpMonth.SelectedValue);
        ht.Add("@SalYear", Convert.ToInt32(salYear));
        ht.Add("@BasicPay", txtBasic.Text.Trim());
        ht.Add("@GrossSal", grossSal.ToString("0"));
        ht.Add("@EmpId", drpEmployee.SelectedValue.Trim());
        ht.Add("@TransDate", Convert.ToDateTime(salMonth + "-01-" + salYear));
        ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
        ht.Add("@SchoolId", Session["SchoolId"]);
        ht.Add("@TotalDedAmt", totDedAmt); // excludes the arrer amount
        ht.Add("@AlwIdList", AlwIdList.Trim());
        ht.Add("@AlwAmtList", AlwAmt.Trim());
        ht.Add("@DedIdList", DedIdList.Trim());
        ht.Add("@DedAmtList", DedAmtList.Trim());
        ht.Add("@DedEmplrAmtList", DedEmplrAmtList.Trim());
        ht.Add("@DedAccIdList", DedAccId.Trim());
        ht.Add("@DedDtlsList", DedDtls.Trim());

        ht.Add("@DedIdArInput", DedArrerIdList.Trim());
        ht.Add("@DedEmpListArInput", DedAmtArrerEmpLst.Trim());
        ht.Add("@DedOrgAmtListArInput", DedAmtArrerOrgLst.Trim());
        ht.Add("@DedAccIdListArInput", DedArrerAccIdLst.Trim());
        ht.Add("@DedDtlsListArInput", DedDtlsArr.Trim());

        ht.Add("@TotalPayble", lblPayblSal.Text.Trim());
        if (txtClaimAmt.Visible == true)
        {
            ht.Add("@ClaimAmt", txtClaimAmt.Text.Trim());
        }
        if (txtEncshAmt.Visible == true)
        {
            ht.Add("@EncashAmt", txtEncshAmt.Text.Trim());
        }
        ht.Add("@PrevTotPayble", hfSalBefChange.Value.Trim());
        try
        {
            msg = obj.ExecuteScalar("HR_ModEmpSalary", ht);
            if (msg.Trim() == "")
            {
                lblMsg.ForeColor = System.Drawing.Color.Green;
                lblMsg.Text = "Data Saved Successfully";
                ClearAll();
            }
            else
            {
                lblMsg.ForeColor = System.Drawing.Color.Red;
                lblMsg.Text = "Unable to save data!! Please Try again!!";
            }
        }
        catch (Exception ex)
        {
            lblMsg.ForeColor = System.Drawing.Color.Red;
            lblMsg.Text = "Unable to save data!! Please Try again!!";
        }
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearAll();
    }

    private void ClearAll()
    {
        txtBasic.Enabled = false;
        btnSave1.Visible = false;
        btnClear.Visible = false;
        btnCalculate.Visible = false;
        grdEmpDed.DataSource = null;
        grdEmpDed.DataBind();
        grdEmpAlw.DataSource = null;
        grdEmpAlw.DataBind();
        lblLoan.Text = "";
        txtBasic.Text = "0";
        drpEmployee.SelectedIndex = 0;
        lblPayblSal.Text = "0";
        txtClaimAmt.Text = "0";
        txtEncshAmt.Text = "0";
        txtDischLoss.Text = "0";
    }
    protected void drpMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        getSalDtls();
    }
    protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        getSalDtls();
    }
}