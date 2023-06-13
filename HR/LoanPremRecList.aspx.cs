using ASP;
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

public partial class HR_LoanPremRecList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            if (!ChkIsHRUsed())
                Response.Redirect("HRHome.aspx?IsHRUsed=0");
            else
                BindEmp();
        }
        else
            Response.Redirect("../Login.aspx");
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
        drpEmp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    private void FillGrid()
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        if (drpEmp.SelectedIndex > 0)
            hashtable.Add("@EmpId", drpEmp.SelectedValue.Trim());
        DataTable dataTable2 = obj.GetDataTable("HR_GetLoanRecList", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            grdLoan.DataSource = dataTable2;
            grdLoan.DataBind();
        }
        else
        {
            grdLoan.DataSource = null;
            grdLoan.DataBind();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void grdLoan_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdLoan.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void btnDelete_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton control = (ImageButton)((Control)sender).Parent.Parent.FindControl("btnSave");
        string str = obj.ExecuteScalar("HR_DelLoanRec", new Hashtable()
    {
      {
         "@PR_Id",
         ((HiddenField) ((Control) sender).Parent.Parent.FindControl("hfPRId")).Value.ToString().Trim()
      },
      {
         "@EmpId",
         ((HiddenField) ((Control) sender).Parent.Parent.FindControl("hfEmpId")).Value.ToString().Trim()
      },
      {
         "@GenledgerId",
         ((HiddenField) ((Control) sender).Parent.Parent.FindControl("hfGenLedgerId")).Value.ToString().Trim()
      }
    });
        if (str.Trim() == "")
        {
            lblMsg.Text = "Loan Recovery deleted Successfully!!";
            lblMsg.ForeColor = Color.Green;
            FillGrid();
        }
        else if (str.Trim().ToUpper() == "GENERATED")
        {
            lblMsg.Text = "Unable to revert loan recovery! Salary already generated for the recovered month!!";
            lblMsg.ForeColor = Color.Red;
        }
        else if (str.Trim() == "No")
        {
            lblMsg.Text = "Unable to revert loan recovery! New Loan has already been Applied!!!!";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Unable to deleted Loan Recovery!Please Try Again Later!!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnNewRec_Click(object sender, EventArgs e)
    {
        Response.Redirect("LoanPremRec.aspx");
    }
}