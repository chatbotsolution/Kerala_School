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

public partial class HR_EmpTrngList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        FillGrid();
    }

    private void FillGrid()
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        if (txtTrngName.Text.Trim() != "")
            hashtable.Add("@TrngName", txtTrngName.Text.Trim());
        DataTable dataTable2 = obj.GetDataTable("HR_GetEmpTrngDtls", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            grdTrng.DataSource = dataTable2;
            grdTrng.DataBind();
        }
        else
        {
            grdTrng.DataSource = null;
            grdTrng.DataBind();
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmpTrngMaster.aspx");
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btndelete, btndelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str1 = Request["Checkb"];
            int num = 0;
            string str2 = str1;
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str2.Split(chArray))
            {
                if (DeleteRecord(obj.ToString()) > 0)
                    ++num;
            }
            if (num > 0)
            {
                lblMsg.Text = "Some Record(s) Could Not Be Deleted!!As Already In Use";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                lblMsg.Text = "Record(s) Deleted Successfully!!";
                lblMsg.ForeColor = Color.Green;
            }
            FillGrid();
        }
    }

    private int DeleteRecord(string id)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable = new DataTable();
        hashtable.Add("@TrngId", id);
        return obj.GetDataTable("HR_DelEmpTrng", hashtable).Rows.Count > 0 ? 1 : 0;
    }

    protected void grdTrng_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdTrng.PageIndex = e.NewPageIndex;
        FillGrid();
    }
}