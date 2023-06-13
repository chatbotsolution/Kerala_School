using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class HR_EmpSalStructureList : System.Web.UI.Page
{
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["em"] != null)
            {
                ht.Add("SalStrId", Request.QueryString["em"]);
                if (obj.ExecuteScalar("GetToDate", ht).ToString() != "")
                    lbledit.Text = "Can't Edit";
                disp();
            }
            bindDropDown(drpDeptName, "select DeptName,DeptId from DeptMaster", "DeptName", "DeptId");
            bindDropDown(drpEmpName, "select EmpName,EmpId from EmpMaster", "EmpName", "EmpId");
        }
        ShowDetails();
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        dt = new DataTable();
        dt = obj.GetDataTableQry(query);
        drp.DataSource = dt;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    private void ShowDetails()
    {
        dt = new DataTable();
        ht = new Hashtable();
        obj = new clsDAL();
        dt = obj.GetDataTable("GetEmpSalStructure", ht);
        grdEmployeSalary.DataSource = dt.DefaultView;
        grdEmployeSalary.DataBind();
        lblCount.Text = "No Of Record : " + dt.Rows.Count.ToString();
    }

    private void DeleteEmpSal(string id)
    {
        ht = new Hashtable();
        dt = new DataTable();
        obj = new clsDAL();
        ht.Add("SalStrId", id);
        dt = obj.GetDataTable("DeleteEmpSal", ht);
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                DeleteEmpSal(obj.ToString());
            ShowDetails();
        }
    }

    protected void drpDeptName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDeptName.SelectedIndex <= 0)
            return;
        bindDropDown(drpEmpName, "select EmpName,EmpId from EmpMaster where DeptId=" + drpDeptName.SelectedValue.ToString() + "  order by EmpName", "EmpName", "EmpId");
    }

    private void disp()
    {
        obj = new clsDAL();
        ht = new Hashtable();
        dt = new DataTable();
        ht.Add("SalStrId", Request.QueryString["em"].ToString());
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("EmpSalStructure.aspx");
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        grdEmployeSalary.DataSource = obj.GetDataTableQry(!(drpDeptName.SelectedIndex > 0 & drpEmpName.SelectedIndex > 0) ? "SELECT\td.DeptName,d.DeptId,e.EmpName,e.EmpId,convert(varchar,es.FromDt,106) as FromStr,convert(varchar,es.ToDt,106) as ToStr,es.ToD,es.SalStrId,es.Pay,es.GP,es.DA,es.Medicine,es.EPF,es.HR,es.GrossTot,es.Medicine\tFROM dbo.DeptMaster d inner join dbo.EmpMaster e on e.DeptId=d.DeptId inner join dbo.EmpSalStructure es on es.EmpId=e.EmpId" : "SELECT\td.DeptName,d.DeptId,e.EmpName,e.EmpId,convert(varchar,es.FromDt,106) as FromStr,convert(varchar,es.ToDt,106) as ToStr,es.ToDt,es.SalStrId,es.Pay,es.GP,es.DA,es.Medicine,es.EPF,es.HR,es.GrossTot,es.Medicine\tFROM dbo.DeptMaster d inner join dbo.EmpMaster e on e.DeptId=d.DeptId inner join dbo.EmpSalStructure es on es.EmpId=e.EmpId where d.DeptId=" + drpDeptName.SelectedValue + " and e.EmpId=" + drpEmpName.SelectedValue);
        grdEmployeSalary.DataBind();
    }
}