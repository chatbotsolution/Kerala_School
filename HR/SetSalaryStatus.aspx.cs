using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class HR_SetSalaryStatus : System.Web.UI.Page
{
    private clsDAL ObjDAL = new clsDAL();
    private DataTable dt;
    private Hashtable ht;
    private int RecCount;
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
                bindSalYear();
                bindDropDown(drpDesignation, "select DesgId,Designation from dbo.HR_DesignationMaster order by Designation", "Designation", "DesgId");
                ClearFields();
            }
        }
        lblMsg.Text = "";
    }

    private bool ChkIsHRUsed()
    {
        return ObjDAL.ExecuteScalarQry("SELECT dbo.fuIsHRUsed()") == "0";
    }

    private void bindSalYear()
    {
        int year = DateTime.Now.Year;
        int num = year - 1;
        drpYear.Items.Insert(0, new ListItem("- SELECT -", "0"));
        drpYear.Items.Insert(1, new ListItem(year.ToString(), year.ToString()));
        drpYear.Items.Insert(2, new ListItem(num.ToString(), num.ToString()));
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        ObjDAL = new clsDAL();
        dt = ObjDAL.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    private void ClearFields()
    {
        drpDesignation.SelectedIndex = 0;
        drpMonth.SelectedIndex = -1;
        drpYear.SelectedIndex = -1;
        btnWithheld.Visible = false;
        lblNoOfRec.Text = string.Empty;
    }

    private void FillGrid()
    {
        ht = new Hashtable();
        dt = new DataTable();
        ObjDAL = new clsDAL();
        ht.Add("@DesignationID", drpDesignation.SelectedValue);
        ht.Add("@SalYr", drpYear.SelectedValue);
        ht.Add("@SalMonth", drpMonth.SelectedValue);
        dt = ObjDAL.GetDataTable("HR_GetEmpForSalStatus", ht);
        grdEmp.DataSource = dt;
        grdEmp.DataBind();
        if (dt.Rows.Count > 0)
        {
            btnWithheld.Visible = true;
            btnCancelWithheld.Visible = true;
        }
        else
        {
            btnWithheld.Visible = false;
            btnCancelWithheld.Visible = false;
        }
        lblNoOfRec.Text = "No Of Records : " + dt.Rows.Count.ToString();
    }

    protected void btnWithheld_Click(object sender, EventArgs e)
    {
        if (!CheckPaymentStatus())
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Control)btnWithheld, btnWithheld.GetType(), "ShowMessage", "alert('Select any Record');", true);
            }
            else
            {
                string str1 = Request["Checkb"];
                RecCount = 0;
                string str2 = str1;
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str2.Split(chArray))
                {
                    if (SetStatus(obj.ToString()) > 0)
                        ++RecCount;
                }
                if (RecCount > 0)
                {
                    grdEmp.Visible = false;
                    FillGrid();
                    lblMsg.Text = "Record Updated Successfully";
                }
                else
                    lblMsg.Text = "";
            }
        }
        else
        {
            lblMsg.Text = "Payment already made. Can not change Status.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private int SetStatus(string id)
    {
        string str = "update dbo.HR_EmpMlySalary set PayWithHeld='Y' where EmpId='" + id + "' and [Year]='" + drpYear.SelectedValue + "' and [Month]='" + drpMonth.SelectedValue + "'";
        ObjDAL = new clsDAL();
        ObjDAL.ExcuteQryInsUpdt(str);
        return 1;
    }

    protected void gvEmpList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdEmp.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void drpDesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void btnCancelWithheld_Click(object sender, EventArgs e)
    {
        if (!CheckPaymentStatus())
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Control)btnWithheld, btnWithheld.GetType(), "ShowMessage", "alert('Select any Record');", true);
            }
            else
            {
                string str1 = Request["Checkb"];
                RecCount = 0;
                string str2 = str1;
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str2.Split(chArray))
                {
                    if (RevertStatus(obj.ToString()) > 0)
                        ++RecCount;
                }
                if (RecCount > 0)
                {
                    grdEmp.Visible = false;
                    lblMsg.Text = "Record Updated Successfully";
                    FillGrid();
                }
                else
                    lblMsg.Text = "";
            }
        }
        else
        {
            lblMsg.Text = "Payment already made. Can not change Status.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private bool CheckPaymentStatus()
    {
        ObjDAL = new clsDAL();
        ht = new Hashtable();
        ht.Clear();
        ht.Add("@SalYear", int.Parse(drpYear.SelectedValue));
        ht.Add("@SalMonth", drpMonth.SelectedValue);
        return ObjDAL.ExecuteScalar("HR_CheckPaymentStatus", ht) == "PAID";
    }

    private int RevertStatus(string id)
    {
        string str = "update dbo.HR_EmpMlySalary set PayWithHeld='N' where EmpId='" + id + "' and [Year]='" + drpYear.SelectedValue + "' and [Month]='" + drpMonth.SelectedValue + "'";
        ObjDAL = new clsDAL();
        ObjDAL.ExcuteQryInsUpdt(str);
        return 1;
    }
}