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
using System.Web.UI.WebControls;

public partial class Accounts_AdvReceiptVoucher : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            bindDropDown(drpCreditHead, "SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads WHERE AG_Code=11 ORDER BY AcctsHead", "AcctsHead", "AcctsHeadId");
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime now = DateTime.Now;
            if (obj.GetDataTableQry("select FY,StartDate,EndDate from dbo.ACTS_FinancialYear where '" + dtpTransDt.GetDateValue().ToString("MM/dd/yyyy") + "' between StartDate and EndDate and IsFinalized is null").Rows.Count > 0)
            {
                DataTable dataTable = new DataTable();
                ViewState["MId"] = obj.GetDataTable("ACTS_InsAdvRcptVoucher", new Hashtable()
        {
          {
             "@TransDate",
             dtpTransDt.GetDateValue().ToString("MM/dd/yyyy")
          },
          {
             "@AccountHeadDr",
             3
          },
          {
             "@TransAmt",
             txtAmount.Text.ToString().Trim()
          },
          {
             "@AccountHeadCr",
             int.Parse(drpCreditHead.SelectedValue.ToString())
          },
          {
             "@Description",
             txtDesc.Text.Trim()
          },
          {
             "@UserId",
            Session["User_Id"]
          },
          {
             "@SchoolId",
            Session["SchoolId"]
          }
        }).Rows[0][0].ToString().Trim();
                if (Request.QueryString["party"] == null)
                {
                    lblMsg.Text = "Data saved successfully !";
                    lblMsg.ForeColor = Color.Green;
                    btnPrint.Enabled = true;
                    ClearPage();
                }
                else
                    ScriptManager.RegisterClientScriptBlock((Control)btnSubmit, btnSubmit.GetType(), "Message", "alert('Data saved successfully !');window.location='../Sales/ItemSale.aspx';", true);
            }
            else if (dtpTransDt.GetDateValue().Date.CompareTo(now.Date) > 0)
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "showmess", "alert('Year is not initialized');window.open ('../FinancialYearChild.aspx?idate=" + dtpTransDt.GetDateValue().ToString("MM/dd/yyyy") + "','mywindow','menubar=1,resizable=1,width=600,height=600,scrollbars=1');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, GetType(), "showmess", "alert('Transaction date is not valid')", true);
                txtTransactionDate.Focus();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message.ToString();
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdvReceiptVoucherList.aspx");
    }

    public void ClearPage()
    {
        txtDesc.Text = string.Empty;
        txtAmount.Text = string.Empty;
        txtTransactionDate.Text = string.Empty;
        drpCreditHead.SelectedIndex = 0;
        txtTransactionDate.Focus();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Administrations/Welcome.aspx");
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["MId"].ToString().Trim();
            string str = "rptActReceiptVoucherPrint.aspx?PrId=" + ViewState["MId"].ToString().Trim();
            btnPrint.Enabled = false;
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "Message", "window.open('" + str + "');", true);
        }
        catch
        {
        }
    }
}