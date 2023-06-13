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

public partial class HR_LoanPremRec : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
        {
            Response.Redirect("../Login.aspx");
        }
        else
        {
            DataTable dataTableQry = obj.GetDataTableQry("SELECT TOP 1 * FROM dbo.ACTS_FinancialYear ORDER BY FY_Id DESC");
            if (dataTableQry.Rows.Count > 0)
            {
                
                RangeFrom from = dtpPayDt.From;
                DateTime dateTime1;
                dtpInstDate.To.Date = dateTime1 = Convert.ToDateTime(dataTableQry.Rows[0]["StartDate"].ToString().Trim());
                DateTime dateTime2 = dateTime1;
                from.Date = dateTime2;
                RangeTo to = dtpPayDt.To;
                DateTime dateTime3;
                dtpInstDate.To.Date = dateTime3 = Convert.ToDateTime(dataTableQry.Rows[0]["EndDate"].ToString().Trim());
                DateTime dateTime4 = dateTime3;
                to.Date = dateTime4;
            }
        }
        if (!Page.IsPostBack)
        {
            if (!ChkIsHRUsed())
            {
                Response.Redirect("HRHome.aspx?IsHRUsed=0");
            }
            else
            {
                BindEmp();
                BindDropDown(drpBank, "SELECT AcctsHeadId, AcctsHead FROM dbo.ACTS_AccountHeads a inner join dbo.Acts_AccountGroups ag on ag.AG_Code=a.AG_Code WHERE (a.AG_Code in (7) or ag.AG_Parent=7) ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
                ViewState["Principal"] = 0;
                ViewState["PendingAmt"] = 0;
                ViewState["TransDate"] = null;
                txtBankName.Enabled = txtInstrNo.Enabled = drpBank.Enabled = false;
            }
        }
        lblMsg.Text = string.Empty;
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void BindEmp()
    {
        drpEmp.Items.Clear();
        drpEmp.DataSource = obj.GetDataTable("HR_GetEmployeeList").DefaultView;
        drpEmp.DataTextField = "Sevname";
        drpEmp.DataValueField = "EmpId";
        drpEmp.DataBind();
        drpEmp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    private void BindDropDown(DropDownList drp, string query, string TextField, string ValueField)
    {
        DataTable dataTable = new DataTable();
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = TextField;
        drp.DataValueField = ValueField;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    protected void drpEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Sal"] = 0;
        drpLoan.Items.Clear();
        grdLoan.DataSource = null;
        grdLoan.DataBind();
        lblSalary.Text = string.Empty;
        lblRecovered.Text = "";
        if (drpEmp.SelectedIndex > 0)
        {
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@EmpId", drpEmp.SelectedValue);
            if (rbRecType.SelectedIndex == 1)
                hashtable.Add("@IsDirect", true);
            DataSet dataSet = obj.GetDataSet("HR_GetLoanList", hashtable);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                drpLoan.DataSource = dataSet.Tables[0].DefaultView;
                drpLoan.DataTextField = "AcctsHead";
                drpLoan.DataValueField = "AcctsHeadId";
                drpLoan.DataBind();
                drpLoan.Items.Insert(0, new ListItem("- SELECT -", "0"));
                if (dataSet.Tables[1].Rows.Count > 0)
                {
                    ViewState["Sal"] = Convert.ToDecimal(dataSet.Tables[1].Rows[0][0].ToString().Trim());
                    lblSalary.Text = "Authorized Salary " + Convert.ToDecimal(dataSet.Tables[1].Rows[0][0].ToString().Trim()).ToString("0.00");
                }
            }
            else
                drpLoan.Items.Insert(0, new ListItem("- No Loan Available -", "0"));
        }
        drpEmp.Focus();
    }

    protected void drpLoan_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetLoanDtls();
        drpLoan.Focus();
    }

    private void GetLoanDtls()
    {
        ViewState["Principal"] = 0;
        ViewState["TotRecAmt"] = 0;
        if (drpLoan.SelectedIndex > 0)
        {
            DataSet dataSet1 = new DataSet();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@EmpId", drpEmp.SelectedValue.ToString().Trim());
            hashtable.Add("@LoanId", drpLoan.SelectedValue.ToString().Trim());
            if (rbRecType.SelectedIndex == 1)
                hashtable.Add("@IsDirect", true);
            DataSet dataSet2 = obj.GetDataSet("HR_GetEmpLoan", hashtable);
            if (rbRecType.SelectedIndex == 0)
            {
                if (dataSet2.Tables[2].Rows.Count > 0)
                {
                    if (dataSet2.Tables[1].Rows.Count > 0 && dataSet2.Tables[0].Rows.Count > 0)
                    {
                        lblRecovered.Text = "Amount to be Recovered " + (Convert.ToDecimal(dataSet2.Tables[0].Rows[0][0].ToString().Trim()) + Convert.ToDecimal(dataSet2.Tables[1].Rows[0][0].ToString().Trim())).ToString("0.00");
                        txtTotAmount.Text = (Convert.ToDecimal(dataSet2.Tables[0].Rows[0][0].ToString().Trim()) + Convert.ToDecimal(dataSet2.Tables[1].Rows[0][0].ToString().Trim())).ToString("0.00");
                        ViewState["TotRecAmt"] = (Convert.ToDecimal(dataSet2.Tables[0].Rows[0][0].ToString().Trim()) + Convert.ToDecimal(dataSet2.Tables[1].Rows[0][0].ToString().Trim()));
                    }
                    else
                    {
                        lblRecovered.Text = "Amount to be Recovered " + Convert.ToDecimal(dataSet2.Tables[0].Rows[0][0].ToString().Trim()).ToString("0.00");
                        txtTotAmount.Text = Convert.ToDecimal(dataSet2.Tables[0].Rows[0][0].ToString().Trim()).ToString("0.00");
                        ViewState["TotRecAmt"] = Convert.ToDecimal(dataSet2.Tables[0].Rows[0][0].ToString().Trim());
                    }
                    ViewState["Principal"] = Convert.ToDecimal(dataSet2.Tables[0].Rows[0][0].ToString().Trim());
                    grdLoan.DataSource = dataSet2.Tables[2];
                    grdLoan.DataBind();
                }
                else
                {
                    grdLoan.DataSource = null;
                    grdLoan.DataBind();
                    lblRecovered.Text = "";
                    txtTotAmount.Text = "";
                }
            }
            else
            {
                if (rbRecType.SelectedIndex != 1)
                    return;
                lblRecovered.Text = txtTotAmount.Text = string.Empty;
                ViewState["PendingAmt"] = 0;
                ViewState["TransDate"] = null;
                if (dataSet2.Tables[0].Rows.Count <= 0)
                    return;
                lblRecovered.Text = "Amount to be Recovered " + dataSet2.Tables[0].Rows[0][0].ToString();
                txtTotAmount.Text = dataSet2.Tables[0].Rows[0][0].ToString();
                if (dataSet2.Tables[0].Rows[0][0].ToString().Trim() != string.Empty)
                    ViewState["PendingAmt"] = dataSet2.Tables[0].Rows[0][0].ToString();
                if (!(dataSet2.Tables[0].Rows[0][1].ToString().Trim() != string.Empty))
                    return;
                ViewState["TransDate"] = Convert.ToDateTime(dataSet2.Tables[0].Rows[0][1]).ToString("dd-MMM-yyyy");
            }
        }
        else
        {
            ClearAll();
            grdLoan.DataSource = null;
            grdLoan.DataBind();
            lblRecovered.Text = "";
            txtTotAmount.Text = "";
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (rbRecType.SelectedIndex == 0)
        {
            PremRec();
        }
        else
        {
            if (rbRecType.SelectedIndex != 1)
                return;
            if (dtpPayDt.GetDateValue() < Convert.ToDateTime(ViewState["TransDate"]))
            {
                lblMsg.Text = "Received Date shouldn't be less then the Loan Paid Date i.e- " + Convert.ToDateTime(ViewState["TransDate"]).ToString("dd-MMM-yyyy");
                lblMsg.ForeColor = Color.Red;
                txtPayDt.Focus();
            }
            else
                DirectRec();
        }
    }

    private void PremRec()
    {
        if (rbtnPayment.SelectedValue == "0")
        {
            if (grdLoan.Rows.Count > 1)
            {
                Hashtable hashtable = new Hashtable();
                Label control1 = (Label)grdLoan.Rows[0].FindControl("lblYear");
                Label control2 = (Label)grdLoan.Rows[0].FindControl("lblMonth");
                HiddenField control3 = (HiddenField)grdLoan.Rows[0].FindControl("hfLoanId");
                HiddenField control4 = (HiddenField)grdLoan.Rows[0].FindControl("hfGenledgerId");
                if (Convert.ToDecimal(txtTotAmount.Text.Trim()) >= Math.Round(Convert.ToDecimal(ViewState["Principal"])) && Convert.ToDecimal(txtTotAmount.Text.Trim()) + Convert.ToDecimal(obj.ExecuteScalarQry("Select isnull(sum(RecAmt),0) as PrinAmt from dbo.HR_EmpLoanRecovery where EmpId=" + drpEmp.SelectedValue.ToString().Trim() + " and RcvdStatus=0 and LoanAcHeadId<>" + drpLoan.SelectedValue.ToString().Trim() + " and CalYear=" + control1.Text.Trim() + " and CalMonth='" + control2.Text.Trim() + "'")) <= Convert.ToDecimal(ViewState["Sal"]))
                {
                    hashtable.Add("@EmpId", drpEmp.SelectedValue.ToString().Trim());
                    hashtable.Add("@Principal", Math.Round(Convert.ToDecimal(ViewState["Principal"])));
                    hashtable.Add("@LoanRecId", control3.Value.Trim());
                    hashtable.Add("@LoanId", drpLoan.SelectedValue.ToString().Trim());
                    hashtable.Add("@GenLedgerId", control4.Value.Trim());
                    hashtable.Add("@Interest", (Convert.ToDecimal(txtTotAmount.Text.Trim()) - Math.Round(Convert.ToDecimal(ViewState["Principal"]))));
                    hashtable.Add("@CalYear", control1.Text.Trim());
                    hashtable.Add("@CalMonth", control2.Text.Trim());
                    hashtable.Add("@UserId", Session["User_Id"]);
                    string str = obj.ExecuteScalar("HR_UpdtLoanRec", hashtable);
                    if (str.Trim() == "")
                    {
                        GetLoanDtls();
                        lblMsg.Text = "Loan Amount Saved Successfully";
                        lblMsg.ForeColor = Color.Green;
                    }
                    else if (str.Trim().ToUpper() == "ALREADY")
                    {
                        lblMsg.Text = "Loan Already Consolidated to a Single Month!! Please Revert Prev Recovery before receiving amount!!";
                        lblMsg.ForeColor = Color.Red;
                    }
                    else
                    {
                        lblMsg.Text = "Loan Amount Could Not Be Saved!";
                        lblMsg.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblMsg.Text = "Recovery Amount Should Not Be Greater Than The Payable Salary";
                    lblMsg.ForeColor = Color.Red;
                }
            }
            else
            {
                lblMsg.Text = "Single Installment Loan Amount Cannot Be Changed!!";
                lblMsg.ForeColor = Color.Red;
                lblMsg.Font.Bold = false;
            }
        }
        else if (rbtnPayment.SelectedValue == "1")
        {
            if (Convert.ToDecimal(txtTotAmount.Text.Trim()) > Math.Round(Convert.ToDecimal(ViewState["Principal"])))
            {
                Hashtable hashtable = new Hashtable();
                Label control1 = (Label)grdLoan.Rows[0].FindControl("lblYear");
                Label control2 = (Label)grdLoan.Rows[0].FindControl("lblMonth");
                HiddenField control3 = (HiddenField)grdLoan.Rows[0].FindControl("hfLoanId");
                HiddenField control4 = (HiddenField)grdLoan.Rows[0].FindControl("hfGenledgerId");
                hashtable.Add("@EmpId", drpEmp.SelectedValue.ToString().Trim());
                hashtable.Add("@Principal", Math.Round(Convert.ToDecimal(ViewState["Principal"])));
                hashtable.Add("@LoanRecId", control3.Value.Trim());
                hashtable.Add("@LoanId", drpLoan.SelectedValue.ToString().Trim());
                hashtable.Add("@GenLedgerId", control4.Value.Trim());
                hashtable.Add("@Interest", (Convert.ToDecimal(txtTotAmount.Text.Trim()) - Math.Round(Convert.ToDecimal(ViewState["Principal"]))));
                hashtable.Add("@CalYear", control1.Text.Trim());
                hashtable.Add("@CalMonth", control2.Text.Trim());
                hashtable.Add("@UserId", Session["User_Id"]);
                hashtable.Add("@SchoolId", Session["SchoolId"]);
                hashtable.Add("@TotRecAmt", Convert.ToDecimal(txtTotAmount.Text.Trim()));
                hashtable.Add("@EmployeeNm", drpEmp.SelectedItem.ToString().Trim());
                hashtable.Add("@PayDt", dtpPayDt.GetDateValue().ToString("dd MMM yyy"));
                if (rbtnPmtBank.Checked)
                {
                    hashtable.Add("@InstrumentNo", txtInstrNo.Text);
                    hashtable.Add("@InstrumentDt", dtpInstDate.GetDateValue().ToString());
                    hashtable.Add("@DrawnOnbank", txtBankName.Text);
                    hashtable.Add("@BankAcHead", int.Parse(drpBank.SelectedValue.ToString()));
                }
                hashtable.Add("@Status", "D");
                string str = obj.ExecuteScalar("HR_UpdtLoanRec", hashtable);
                if (str.Trim() == "")
                {
                    lblMsg.Text = "Loan Amount Received Successfully";
                    lblMsg.ForeColor = Color.Green;
                    ViewState["Principal"] = 0;
                    ClearAll();
                    GetLoanDtls();
                }
                else if (str.Trim().ToUpper() == "ALREADY")
                {
                    lblMsg.Text = "Loan Already Consolidated to a Single Month!! Please Revert Prev Recovery to receive loan amount!!";
                    lblMsg.ForeColor = Color.Red;
                }
                else if (str.Trim().ToUpper() == "INVALID")
                {
                    lblMsg.Text = "Loan Payment date cannot be prior to Loan sanction date!!";
                    lblMsg.ForeColor = Color.Red;
                }
                else
                {
                    lblMsg.Text = "Loan Amount Could Not Be Saved!";
                    lblMsg.ForeColor = Color.Red;
                }
            }
            else
            {
                lblMsg.Text = "Recovery Amount Cannot Be Greater Than Pending Amount!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        else
        {
            string empty = string.Empty;
            Label control1 = (Label)grdLoan.Rows[0].FindControl("lblYear");
            Label control2 = (Label)grdLoan.Rows[0].FindControl("lblMonth");
            Label control3 = (Label)grdLoan.Rows[0].FindControl("lblAmount");
            HiddenField control4 = (HiddenField)grdLoan.Rows[0].FindControl("hfLoanId");
            HiddenField control5 = (HiddenField)grdLoan.Rows[0].FindControl("hfGenledgerId");
            if (Convert.ToDecimal(control3.Text) > Convert.ToDecimal(txtTotAmount.Text.Trim()))
            {
                if (obj.ExecuteScalar("HR_LoanRecPartly", new Hashtable()
        {
          {
             "@LoanRecId",
             control4.Value.Trim()
          },
          {
             "@TotRecAmt",
             Convert.ToDecimal(txtTotAmount.Text.Trim())
          },
          {
             "@UserId",
            Session["User_Id"]
          }
        }).Trim().ToUpper() == "S")
                {
                    lblMsg.ForeColor = Color.Green;
                    lblMsg.Text = "Loan Amount Received Successfully";
                    GetLoanDtls();
                }
                else
                {
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Failed to Save. Try Again.";
                }
            }
            else
            {
                if (!(Convert.ToDecimal(control3.Text) < Convert.ToDecimal(txtTotAmount.Text.Trim())))
                    return;
                if (obj.ExecuteScalar("HR_LoanRecUpdate", new Hashtable()
        {
          {
             "@LoanRecId",
             control4.Value.Trim()
          },
          {
             "@TotRecAmt",
             Convert.ToDecimal(txtTotAmount.Text.Trim())
          },
          {
             "@UserId",
            Session["User_Id"]
          }
        }).Trim().ToUpper() == "S")
                {
                    Decimal result1 = new Decimal(0);
                    Decimal.TryParse((Convert.ToDecimal(txtTotAmount.Text.Trim()) - Convert.ToDecimal(control3.Text)).ToString(), out result1);
                    foreach (DataRow row in (InternalDataCollectionBase)obj.GetDataTable("HR_GetEmpLoanDesc", new Hashtable()
          {
            {
               "@EmpId",
               drpEmp.SelectedValue.ToString().Trim()
            },
            {
               "@LoanId",
               drpLoan.SelectedValue.ToString().Trim()
            }
          }).Rows)
                    {
                        Decimal num1 = new Decimal(0);
                        Decimal result2 = new Decimal(0);
                        Decimal.TryParse(Convert.ToString(row["RecAmt"]), out result2);
                        Decimal num2;
                        if (result1 > result2)
                        {
                            row["RecAmt"] = 0;
                            result1 -= result2;
                            num2 = result2;
                        }
                        else
                        {
                            row["RecAmt"] = (result2 - result1);
                            num2 = result1;
                            result1 = new Decimal(0);
                        }
                        if (obj.ExecuteScalar("HR_LoanRecUpdateCredit", new Hashtable()
            {
              {
                 "@LoanRecId",
                 row["LoanRecId"].ToString().Trim()
              },
              {
                 "@TotRecAmt",
                 num2
              },
              {
                 "@UserId",
                Session["User_Id"]
              }
            }).Trim().ToUpper() == "S")
                        {
                            lblMsg.ForeColor = Color.Green;
                            lblMsg.Text = "Loan Amount Received Successfully";
                            if (result1 <= new Decimal(0))
                                break;
                        }
                        else
                            break;
                    }
                    GetLoanDtls();
                }
                else
                {
                    lblMsg.ForeColor = Color.Red;
                    lblMsg.Text = "Failed to Save. Try Again.";
                }
            }
        }
    }

    private void DirectRec()
    {
        Decimal num1 = new Decimal(0);
        Decimal num2 = new Decimal(0);
        if (Convert.ToDecimal(txtTotAmount.Text.Trim()) > Convert.ToDecimal(ViewState["PendingAmt"]))
            num2 = Convert.ToDecimal(txtTotAmount.Text.Trim()) - Convert.ToDecimal(ViewState["PendingAmt"]);
        Decimal num3 = Convert.ToDecimal(txtTotAmount.Text.Trim()) - num2;
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@EmpId", drpEmp.SelectedValue.ToString().Trim());
        hashtable.Add("@TotPendingAmt", Math.Round(Convert.ToDecimal(ViewState["PendingAmt"])));
        hashtable.Add("@LoanId", drpLoan.SelectedValue.ToString().Trim());
        hashtable.Add("@UserId", Session["User_Id"]);
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        hashtable.Add("@TotRecAmt", num3);
        hashtable.Add("@InterestAmt", num2);
        hashtable.Add("@EmployeeNm", drpEmp.SelectedItem.ToString().Trim());
        hashtable.Add("@PayDt", dtpPayDt.GetDateValue().ToString("dd-MMM-yyyy"));
        if (rbtnPmtBank.Checked)
        {
            hashtable.Add("@InstrumentNo", txtInstrNo.Text);
            hashtable.Add("@InstrumentDt", dtpInstDate.GetDateValue().ToString());
            hashtable.Add("@DrawnOnbank", txtBankName.Text);
            hashtable.Add("@BankAcHead", int.Parse(drpBank.SelectedValue.ToString()));
        }
        if (obj.ExecuteScalar("HR_DirectLoanRec", hashtable).Trim() == string.Empty)
        {
            lblMsg.Text = "Loan Amount Received Successfully";
            lblMsg.ForeColor = Color.Green;
            ViewState["PendingAmt"] = 0;
            ClearAll();
            GetLoanDtls();
        }
        else
        {
            lblMsg.Text = "Transaction Failed. Try Again.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void ClearAll()
    {
        txtBankName.Text = "";
        txtInstrDate.Text = "";
        txtInstrNo.Text = "";
        txtPayDt.Text = "";
        drpBank.SelectedIndex = 0;
        rbtnPmtCash.Checked = true;
        rbtnPmtCash_CheckedChanged(rbtnPmtCash, EventArgs.Empty);
        txtTotAmount.Text = "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAll();
        GetLoanDtls();
    }

    protected void rbtnPmtCash_CheckedChanged(object sender, EventArgs e)
    {
        if (rbtnPmtCash.Checked)
        {
            txtBankName.Text = txtInstrDate.Text = txtInstrNo.Text = string.Empty;
            drpBank.SelectedIndex = -1;
            txtBankName.Enabled = txtInstrNo.Enabled = drpBank.Enabled = false;
            btnSave.Focus();
        }
        else
        {
            txtBankName.Enabled = txtInstrNo.Enabled = drpBank.Enabled = true;
            drpBank.Focus();
        }
    }

    protected void rbtnPayment_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtnPayment.SelectedValue == "0" || rbtnPayment.SelectedValue == "2")
        {
            txtBankName.Text = txtInstrDate.Text = txtInstrNo.Text = string.Empty;
            drpBank.SelectedIndex = -1;
            txtBankName.Enabled = txtInstrNo.Enabled = drpBank.Enabled = false;
            pnlPayment.Enabled = false;
        }
        else
            pnlPayment.Enabled = true;
    }

    protected void rbRecType_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtBankName.Text = string.Empty;
        txtInstrDate.Text = string.Empty;
        txtInstrNo.Text = string.Empty;
        txtPayDt.Text = string.Empty;
        txtTotAmount.Text = string.Empty;
        drpBank.SelectedIndex = 0;
        if (rbRecType.SelectedIndex == 0)
        {
            row1.Visible = true;
            col1.Visible = true;
            grdLoan.DataSource = null;
            grdLoan.DataBind();
            grdLoan.Visible = true;
            rbtnPayment.SelectedIndex = 0;
            rbtnPayment.Enabled = true;
        }
        else
        {
            row1.Visible = false;
            col1.Visible = false;
            grdLoan.DataSource = null;
            grdLoan.DataBind();
            grdLoan.Visible = false;
            rbtnPayment.SelectedIndex = 1;
            rbtnPayment_SelectedIndexChanged(rbtnPayment, EventArgs.Empty);
            rbtnPayment.Enabled = false;
        }
        drpEmp_SelectedIndexChanged(drpEmp, EventArgs.Empty);
    }
}