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

public partial class Inventory_IssueList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dtItemSection = new DataTable();
    private Hashtable htSection = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            lblMsg.Text = string.Empty;
            if (Page.IsPostBack)
                return;
            Page.Form.DefaultButton = btnSearch.UniqueID;
            btnDelete.Enabled = false;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        bindGrid();
    }

    private void bindGrid()
    {
        DataTable dataTable1 = new DataTable();
        Hashtable ht = new Hashtable();
        if (txtFrmDt.Text != "")
            ht.Add("@FromDate", PopCalendar1.GetDateValue());
        if (txtTo.Text != "")
            ht.Add("@Todate", PopCalendar2.GetDateValue());
        if (Session["User"].ToString() != "admin")
        {
            ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
            ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
        }
        DataTable dataTable2 = obj.GetDataTable("SI_GetAllIssue", ht);
        grdItem.DataSource = dataTable2;
        Session["IssueDetails"] = dataTable2;
        grdItem.DataBind();
        if (dataTable2.Rows.Count > 0)
        {
            btnDelete.Enabled = true;
        }
        else
        {
            btnDelete.Enabled = false;
            lblMsg.Text = "No Record !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("IssueMaster.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                string str = Request["Checkb"];
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str.Split(chArray))
                    DeleteIssue(obj.ToString());
                bindGrid();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DeleteIssue(string Id)
    {
        Hashtable ht = new Hashtable();
        DataTable dataTable = new DataTable();
        clsDAL clsDal = new clsDAL();
        ht.Add("@IssueId", Id);
        if (clsDal.GetDataTable("SI_DelIssue", ht).Rows.Count > 0)
        {
            lblMsg.Text = "Already Used !";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Data Deleted Successfully !";
            lblMsg.ForeColor = Color.Green;
        }
    }

    protected void grdItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdItem.PageIndex = e.NewPageIndex;
        bindGrid();
    }
}