using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class HR_SalaryInit : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (!Page.IsPostBack)
        {
            if (!ChkIsHRUsed())
                Response.Redirect("HRHome.aspx?IsHRUsed=0");
            else
                FillDrp(drpEmp, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster ORDER BY EmpName", "EmpName", "EmpId");
        }
        trMsg.Style["background-color"] = "White";
        lblMsg.Text = "";
    }

    private bool ChkIsHRUsed()
    {
        return obj.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void FillDrp(DropDownList drp, string Qry, string Text, string Value)
    {
        DataTable dataTableQry = obj.GetDataTableQry(Qry.Trim());
        drp.DataSource = dataTableQry.DefaultView;
        drp.DataTextField = Text;
        drp.DataValueField = Value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = "Transparent";
        lblMsg.Text = string.Empty;
        string empty = string.Empty;
        string str1 = ValGrid();
        if (str1.Trim() == string.Empty)
        {
            Decimal num = new Decimal(0);
            string str2 = string.Empty;
            string str3 = string.Empty;
            string str4 = string.Empty;
            string selectedValue = drpMonth.SelectedValue;
            string str5 = "2016";
            if (str5.Trim() != string.Empty)
            {
                string str6;
                if (selectedValue == "JAN")
                    str6 = "FEB";
                else if (selectedValue == "FEB")
                    str6 = "MAR";
                else if (selectedValue == "MAR")
                    str6 = "APR";
                else if (selectedValue == "APR")
                    str6 = "MAY";
                else if (selectedValue == "MAY")
                    str6 = "JUN";
                else if (selectedValue == "JUN")
                    str6 = "JUL";
                else if (selectedValue == "JUL")
                    str6 = "AUG";
                else if (selectedValue == "AUG")
                    str6 = "SEP";
                else if (selectedValue == "SEP")
                    str6 = "OCT";
                else if (selectedValue == "OCT")
                    str6 = "NOV";
                else if (selectedValue == "NOV")
                {
                    str6 = "DEC";
                }
                else
                {
                    str6 = "JAN";
                    str5 = (Convert.ToInt32(str5) + 1).ToString();
                }
                foreach (GridViewRow row in grdEmpDed.Rows)
                {
                    HiddenField control1 = (HiddenField)row.FindControl("hfDedId");
                    TextBox control2 = (TextBox)row.FindControl("txtEmpAmt");
                    TextBox control3 = (TextBox)row.FindControl("txtEmplrAmt");
                    num += Convert.ToDecimal(control2.Text.Trim());
                    str2 = !(str2.Trim() == string.Empty) ? str2 + Convert.ToDecimal(control2.Text).ToString("0") + "," : Convert.ToDecimal(control2.Text).ToString("0") + ",";
                    str3 = !(str3.Trim() == string.Empty) ? str3 + Convert.ToDecimal(control3.Text).ToString("0") + "," : Convert.ToDecimal(control3.Text).ToString("0") + ",";
                    str4 = !(str4.Trim() == string.Empty) ? str4 + "," + control1.Value : control1.Value;
                }
                str1 = obj.ExecuteScalar("HR_InitEmpSalary", new Hashtable()
        {
          {
             "@SalMonth",
             drpMonth.SelectedValue
          },
          {
             "@SalYear",
             Convert.ToInt32(str5)
          },
          {
             "@GrossSal",
             txtGrossSal.Text.Trim()
          },
          {
             "@EmpId",
             drpEmp.SelectedValue.Trim()
          },
          {
             "@TransDate",
             Convert.ToDateTime(str6 + "-01-" + str5)
          },
          {
             "@UserId",
             int.Parse(Session["User_Id"].ToString())
          },
          {
             "@SchoolId",
            Session["SchoolId"]
          },
          {
             "@TotalDedAmt",
             num
          },
          {
             "@BillNo",
             ("Sal-1-" + drpMonth.SelectedValue +  '-' + str5)
          },
          {
             "@DedIdList",
             str4.Trim()
          },
          {
             "@DedAmtList",
             str2.Trim()
          },
          {
             "@DedEmplrAmtList",
             str3.Trim()
          }
        });
            }
            else
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('First Initialize the Current Session Year.');window.location='HRHome.aspx'", true);
        }
        if (str1.Trim() == string.Empty)
        {
            trMsg.Style["background-color"] = "Green";
            lblMsg.Text = "Data Saved Successfully";
        }
        else
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = str1;
        }
    }

    private string ValGrid()
    {
        return string.Empty;
    }

    protected void drpMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpMonth.Focus();
    }

    private void GetTotPay()
    {
        Decimal num1 = new Decimal(0);
        foreach (Control row in grdEmpDed.Rows)
        {
            TextBox control = (TextBox)row.FindControl("txtEmpAmt");
            num1 += Convert.ToDecimal(control.Text.Trim());
        }
        Decimal num2 = new Decimal(0);
        if (lblLoan.Text.Trim() != "")
            num2 = Convert.ToDecimal(txtGrossSal.Text.Trim()) - num1 - Convert.ToDecimal(lblLoan.Text.Trim().Split(new string[1]
      {
        "MAR"
      }, StringSplitOptions.RemoveEmptyEntries)[1].Trim());
        lblPayblSal.Text = "Total Payble Salary : " + num2.ToString("0.00");
    }

    protected void drpEmp_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpEmp.SelectedIndex > 0)
        {
            GetInitDtls();
            btnSave1.Visible = true;
            btnClear.Visible = true;
            btnCalculate.Visible = true;
            txtGrossSal.Enabled = true;
            GetTotPay();
            txtGrossSal.Focus();
        }
        else
        {
            txtGrossSal.Enabled = false;
            btnSave1.Visible = false;
            btnClear.Visible = false;
            btnCalculate.Visible = false;
            grdEmpDed.DataSource = null;
            grdEmpDed.DataBind();
            lblLoan.Text = "";
            txtGrossSal.Text = "0";
        }
    }

    private void GetInitDtls()
    {
        obj = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@EmpId", drpEmp.SelectedValue.Trim());
        hashtable.Add("@Month", drpMonth.SelectedValue.Trim());
        string str = obj.ExecuteScalarQry("SELECT YEAR(StartDate) FROM dbo.ACTS_FinancialYear WHERE IsFinalized IS NULL order by FY_Id desc");
        hashtable.Add("@SalYear", str);
        DataSet dataSet1 = new DataSet();
        DataSet dataSet2 = obj.GetDataSet("HR_GetEmpSalInitDtls", hashtable);
        if (dataSet2.Tables.Count > 0)
        {
            if (dataSet2.Tables[0].Rows.Count > 0)
            {
                txtGrossSal.Text = !(dataSet2.Tables[0].Rows[0][0].ToString().Trim() != "") ? "0" : Convert.ToDecimal(dataSet2.Tables[0].Rows[0][0].ToString().Trim()).ToString("0.00");
                grdEmpDed.DataSource = new DataView(dataSet2.Tables[0])
                {
                    RowFilter = "DedTypeId is not null"
                };
                grdEmpDed.DataBind();
            }
            else
            {
                grdEmpDed.DataSource = null;
                grdEmpDed.DataBind();
                txtGrossSal.Text = "0";
                txtGrossSal.Enabled = false;
            }
            lblLoan.Text = "Loan/Advance to be Recovered from the salary of " + drpMonth.SelectedValue + " " + Convert.ToDecimal((!(dataSet2.Tables[1].Rows[0][0].ToString().Trim() == "") ? dataSet2.Tables[1].Rows[0][0].ToString().Trim() : "0").Trim()).ToString("0.00");
        }
        else
        {
            txtGrossSal.Enabled = false;
            btnSave1.Visible = false;
            btnClear.Visible = false;
            btnCalculate.Visible = false;
            grdEmpDed.DataSource = null;
            grdEmpDed.DataBind();
            lblLoan.Text = "";
            txtGrossSal.Text = "0";
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = "Cannot alter any other details!! Salary already generate!!";
        }
    }

    protected void btnCalculate_Click(object sender, EventArgs e)
    {
        GetTotPay();
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtGrossSal.Enabled = false;
        btnSave1.Visible = false;
        btnClear.Visible = false;
        btnCalculate.Visible = false;
        grdEmpDed.DataSource = null;
        grdEmpDed.DataBind();
        lblLoan.Text = "";
        txtGrossSal.Text = "0";
        drpEmp.SelectedIndex = 0;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("HRInit.aspx");
    }
}