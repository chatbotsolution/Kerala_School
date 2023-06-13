using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Hostel_HostExpenseList : System.Web.UI.Page
{
    private Hashtable ht = new Hashtable();
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
       ClearFields();
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
       obj = new clsDAL();
        DataTable dataTableQry =obj.GetDataTableQry(query);
        drp.DataSource = (object)dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("All", "0"));
    }

    private void ClearFields()
    {
       dtpFromDate.SetDateValue(Convert.ToDateTime("01/01/" + DateTime.Now.Year.ToString()));
       dtpToDate.SetDateValue(DateTime.Now);
    }

    private void fillgrid()
    {
       ht = new Hashtable();
       dt = new DataTable();
        if (dtpFromDate.GetDateValue() >dtpToDate.GetDateValue())
        {
           ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('From Date can not be greater than To Date');", true);
        }
        else
        {
           ht.Add((object)"@FrmDt", (object)(dtpFromDate.GetDateValue().ToString("MM/dd/yyyy") + " 00:00:00"));
           ht.Add((object)"@ToDt", (object)(dtpToDate.GetDateValue().ToString("MM/dd/yyyy") + " 23:59:59"));
           dt =obj.GetDataTable("Host_GetMiscExp",ht);
           gvMiscExp.DataSource = (object)dt;
           gvMiscExp.DataBind();
           lblNoOfRec.Text = "TotalRecords : " +dt.Rows.Count.ToString();
            if (dt.Rows.Count > 0)
            {
               btndelete.Enabled = true;
               lblNoOfRec.Visible = true;
            }
            else
            {
               btndelete.Enabled = false;
               lblNoOfRec.Visible = false;
            }
        }
    }

    protected void btnshow_Click(object sender, EventArgs e)
    {
       fillgrid();
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btndelete,btndelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str1 =Request["Checkb"];
            int num = 0;
            string str2 = str1;
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str2.Split(chArray))
            {
                if (DeleteStud(obj.ToString()) > 0)
                    ++num;
            }
            if (num > 0)
                ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('" + num.ToString() + " of the selected Records could not be deleted');", true);
            else
                ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('Record Cancelled Successfully !');", true);
           fillgrid();
        }
    }

    private int DeleteStud(string id)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable = new DataTable();
        clsDAL clsDal = new clsDAL();
        hashtable.Add((object)"@PR_Id", (object)id);
        return clsDal.GetDataTable("Host_DelMiscExp", hashtable).Rows.Count > 0 ? 1 : 0;
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
       Response.Redirect("HostExpensesEntry.aspx");
    }

    protected void gvMiscExp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
       gvMiscExp.PageIndex = e.NewPageIndex;
       fillgrid();
    }
}