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

public partial class HR_ArrearRevert : System.Web.UI.Page
{
    private clsDAL ObjDAL = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        trMsg.BgColor = "Transparent";
        if (Page.IsPostBack)
            return;
        bindDropDown(drpEmpName, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster Where ActiveStatus=1 ORDER BY EmpName", "EmpName", "EmpId");
        bindSalYear();
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.Items.Clear();
        DataTable dataTable = new DataTable();
        ObjDAL = new clsDAL();
        DataTable dataTableQry = ObjDAL.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        if (dataTableQry.Rows.Count <= 0)
            return;
        drp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    private void bindSalYear()
    {
        int year = DateTime.Now.Year;
        int num = year - 1;
        drpYear.Items.Insert(0, new ListItem("- SELECT -", "0"));
        drpYear.Items.Insert(1, new ListItem(year.ToString(), year.ToString()));
        drpYear.Items.Insert(2, new ListItem(num.ToString(), num.ToString()));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid();
        btnSearch.Focus();
    }

    private void FillGrid()
    {
        try
        {
            btnRevert.Enabled = false;
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@Month", drpMonth.SelectedValue);
            hashtable.Add("@Year", drpYear.SelectedValue);
            if (drpEmpName.SelectedIndex > 0)
                hashtable.Add("@EmpId", drpEmpName.SelectedValue);
            DataSet dataSet = ObjDAL.GetDataSet("HR_GetArrear", hashtable);
            if (dataSet.Tables.Count > 0)
            {
                DataTable table1 = dataSet.Tables[0];
                DataTable table2 = dataSet.Tables[1];
                if (table1.Rows.Count > 0)
                {
                    grdEmp.DataSource = table1;
                    grdEmp.DataBind();
                    if (table2.Rows.Count > 0)
                    {
                        if (table1.Rows[0][0].ToString() == "0")
                            btnRevert.Enabled = true;
                        else
                            btnRevert.Enabled = false;
                    }
                    else
                        btnRevert.Enabled = false;
                }
                else
                {
                    grdEmp.EmptyDataText = "No Record(s)";
                    grdEmp.DataSource = null;
                    grdEmp.DataBind();
                }
            }
            else
            {
                grdEmp.EmptyDataText = "No Record(s)";
                grdEmp.DataSource = null;
                grdEmp.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnRevert_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnRevert, btnRevert.GetType(), "ShowMessage", "alert('Select any Record');", true);
            grdEmp.Focus();
        }
        else if (ObjDAL.ExecuteScalarQry("DELETE FROM dbo.HR_EmpMlyAllowance WHERE AllwId IN (" + Request["Checkb"].ToString() + ")").Trim() == string.Empty)
        {
            FillGrid();
            lblMsg.Text = "Arrear Reverted Successfully";
            trMsg.BgColor = "Green";
        }
        else
        {
            lblMsg.Text = "Failed to Revert. Try Again.";
            trMsg.BgColor = "Red";
        }
    }
}