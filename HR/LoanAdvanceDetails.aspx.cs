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

public partial class HR_LoanAdvanceDetails : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        FillGrid();
    }

    private void FillGrid()
    {
        trMsg.Style["background-color"] = "Green";
        lblMsg.Text = "Month-wise Loan Recovery Details of " + Request.QueryString["emp"] + ", Total Loan Amount : " + Convert.ToDecimal(Request.QueryString["amt"]).ToString("0.00");
        DataTable dataTable = new DataTable();
        grdLoan.DataSource = obj.GetDataTable("HR_LoanAdvanceDetails", new Hashtable()
    {
      {
         "@GenLedgerId",
         Request.QueryString["gl_id"]
      }
    });
        grdLoan.DataBind();
    }
}