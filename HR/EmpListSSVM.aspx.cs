using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class HR_EmpListSSVM : System.Web.UI.Page
{
    private clsDAL ObjCommon = new clsDAL();
    private Hashtable ht;
    private DataTable dt;
    private DropDownList drpSambhagObj;
    private DropDownList drpBibhagObj;
    private DropDownList drpZilaObj;
    private DropDownList drpSankulaObj;
    private DropDownList drpBlockObj;
    private DropDownList drpSSVMObj;
    private int RecCount;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User_Id"] == null)
            this.Response.Redirect("../Login.aspx");
        if (this.Page.IsPostBack)
            return;
        this.Session["title"] = (object)this.Page.Title.ToString();
        this.bindSingleDrp(this.drpDesig, "select DesgId,Designation,TeachingStaff from dbo.HR_DesignationMaster order by Designation", "Designation", "DesgId");
        clsStaticDropdowns clsStaticDropdowns = new clsStaticDropdowns();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        this.fillgrid();
    }

    private void fillgrid()
    {
        this.dt = new DataTable();
        this.ObjCommon = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        string empty3 = string.Empty;
        stringBuilder.Append("SELECT EmpId, SevName, Sex, DOB, MaritalStatus, FatherName, SpouseName, MotherName, EM.Phone, Mobile, EM.emailid, ActiveStatus, Remarks,CASE WHEN PresentEstType='' THEN 'Not Defined' ELSE PresentEstType END AS PresentEstType,Designation,PresAddress");
        stringBuilder.Append(" from HR_EmployeeMaster EM ");
        stringBuilder.Append(" inner join HR_DesignationMaster D on D.DesgId=EM.DesignationId");
        stringBuilder.Append(" WHERE 1=1 ");
        if (this.drpDesig.SelectedIndex > 0)
            stringBuilder.Append(" AND DesignationId=" + this.drpDesig.SelectedValue.ToString());
        if (!this.txtEmp.Text.Trim().Equals(string.Empty))
            stringBuilder.Append(" AND SevName LIKE'%" + this.txtEmp.Text.Trim() + "%'");
        if (this.drpStatus.SelectedIndex > 0)
            stringBuilder.Append(" AND ActiveStatus=" + this.drpStatus.SelectedValue.ToString());
        stringBuilder.Append("  order by PresentEstType,SortOrder");
        this.dt = this.ObjCommon.GetDataTableQry(stringBuilder.ToString());
        this.grdEmpDetails.DataSource = (object)this.dt.DefaultView;
        this.grdEmpDetails.DataBind();
        this.grdEmpDetails.Visible = true;
        if (this.dt.Rows.Count > 0)
            this.lblRecCount.Text = "Total Records: " + this.dt.Rows.Count.ToString();
        else
            this.lblRecCount.Text = "";
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("EmpOld.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (this.Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)this.btnDelete, this.btnDelete.GetType(), "ShowMessage", "alert('Select any Record');", true);
        }
        else
        {
            string str1 = this.Request["Checkb"];
            this.RecCount = 0;
            string str2 = str1;
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str2.Split(chArray))
            {
                if (this.DeleteEmployeeDet(obj.ToString()) > 0)
                    ++this.RecCount;
            }
            this.fillgrid();
            if (this.RecCount > 0)
                ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMessage", "alert('" + this.RecCount.ToString() + " of the selected Records could not be Deleted');", true);
            else
                ScriptManager.RegisterClientScriptBlock((Page)this, this.GetType(), "ShowMessage", "alert('Selected Records Deleted Successfully');", true);
        }
    }

    private int DeleteEmployeeDet(string id)
    {
        this.ht = new Hashtable();
        this.dt = new DataTable();
        this.ObjCommon = new clsDAL();
        this.ht.Add((object)"EmpId", (object)id);
        this.dt = this.ObjCommon.GetDataTable("HR_DeleteEmployee", this.ht);
        return this.dt.Rows.Count > 0 ? 1 : 0;
    }

    protected void grdEmpDetails_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.grdEmpDetails.PageIndex = e.NewPageIndex;
        this.fillgrid();
    }

    private void bindSingleDrp(DropDownList drp, string query, string text, string value)
    {
        this.dt = new DataTable();
        this.dt = this.ObjCommon.GetDataTableQry(query);
        drp.DataSource = (object)this.dt;
        drp.DataTextField = text;
        drp.DataValueField = value;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    private void ClearAllDrp()
    {
        this.drpSambhagObj.SelectedIndex = -1;
        this.drpBibhagObj.SelectedIndex = -1;
        this.drpZilaObj.SelectedIndex = -1;
        this.drpSankulaObj.SelectedIndex = -1;
        this.drpSSVMObj.SelectedIndex = -1;
        this.drpBlockObj.SelectedIndex = -1;
        this.grdEmpDetails.Visible = false;
    }
}