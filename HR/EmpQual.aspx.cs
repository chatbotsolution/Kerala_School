using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_EmpQual : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User_Id"] == null)
            this.Response.Redirect("../Login.aspx");
        this.lblMsg.Text = "";
        if (this.Page.IsPostBack)
            return;
        this.ViewState["id"] = "";
    }

    private void fillfields()
    {
        DataTable dataTable = new DataTable();
        this.txtQual.Text = this.obj.GetDataTableQry("select * from dbo.HR_QualMaster where QualId=" + this.ViewState["id"].ToString()).Rows[0]["QualName"].ToString();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            Hashtable hashtable = new Hashtable();
            DataTable dataTable = new DataTable();
            if (this.ViewState["id"].ToString().Trim() != "")
                hashtable.Add("@QualId", this.ViewState["id"].ToString());
            hashtable.Add("@QualName", this.txtQual.Text.Trim());
            if (this.obj.ExecuteScalar("HR_InsUpdtQual", hashtable).Trim() == "OK")
            {
                this.Clear();
                this.lblMsg.Text = "Data Saved Successfully!!";
                this.lblMsg.ForeColor = Color.Green;
            }
            else
            {
                this.lblMsg.Text = "Data Already Exisits!!";
                this.lblMsg.ForeColor = Color.Red;
            }
            this.FillGrid();
        }
        catch (Exception ex)
        {
        }
    }

    private void FillGrid()
    {
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        this.View_grd.Visible = true;
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Select QualId, QualName from HR_QualMaster");
        if (this.txtSerch.Text.Trim() != "")
            stringBuilder.Append(" where QualName like '%" + this.txtSerch.Text.Trim() + "%'");
        stringBuilder.Append(" order by QualName");
        this.grdQual.DataSource = this.obj.GetDataTableQry(stringBuilder.ToString());
        this.grdQual.DataBind();
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        this.ViewState["id"] = this.grdQual.DataKeys[((GridViewRow)((Control)sender).Parent.Parent).DataItemIndex].Value.ToString();
        this.fillfields();
    }

    private void Clear()
    {
        this.txtQual.Text = string.Empty;
        this.ViewState["id"] = "";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        this.Clear();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        this.FillGrid();
        this.ViewState["id"] = "";
    }

    protected void grdQual_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        this.grdQual.PageIndex = e.NewPageIndex;
        this.FillGrid();
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        this.Clear();
        this.txtQual.Focus();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        int num = 0;
        if (this.Request["Checkb"] == null)
        {
            this.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowMessage", "alert('select a checkbox');", true);
        }
        else
        {
            string[] strArray = this.Request["Checkb"].Split(',');
            for (int index = 0; index < strArray.Length; ++index)
            {
                if (this.deleteRecord(strArray[index].ToString()).Trim() == "OK")
                    ++num;
            }
            this.FillGrid();
            if (num == strArray.Length)
            {
                this.lblMsg2.Text = "Record(s) Deleted Successfully!!";
                this.lblMsg2.ForeColor = Color.Green;
            }
            else
            {
                this.lblMsg2.Text = "Some Record(s) Could Not Be Deleted!! As Already In Use!!";
                this.lblMsg2.ForeColor = Color.Red;
            }
        }
    }

    private string deleteRecord(string id)
    {
        return this.obj.ExecuteScalar("HR_DelEmpQual", new Hashtable()
    {
      {
         "@QualId",
         id
      }
    });
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        this.FillGrid();
    }
}