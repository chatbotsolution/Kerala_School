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


public partial class HR_DesignationList : System.Web.UI.Page
{
    private clsDAL ObjDAL = new clsDAL();
    private int RecCount;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        FillGrid();
    }

    private void FillGrid()
    {
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable2 = ObjDAL.GetDataTable("HR_GetDesignationForGvBind");
        grdDesignation.DataSource = dataTable2.DefaultView;
        grdDesignation.DataBind();
        lblRecCount.Text = "No Of Records : " + dataTable2.Rows.Count.ToString();
        if (dataTable2.Rows.Count > 0)
        {
            btnDelete.Visible = true;
            btnSortOrder.Visible = true;
        }
        else
        {
            btnDelete.Visible = false;
            btnSortOrder.Visible = false;
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Designation.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select a Checkbox');", true);
        }
        else
        {
            string str1 = Request["Checkb"];
            RecCount = 0;
            string str2 = str1;
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str2.Split(chArray))
            {
                if (DeleteRecord(obj.ToString()) > 0)
                    ++RecCount;
            }
            if (RecCount > 0)
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = RecCount.ToString() + " Record(s) could not be Deleted";
            }
            else
            {
                trMsg.Style["background-color"] = "Green";
                lblMsg.Text = "Record(s) Deleted Successfully";
            }
            FillGrid();
        }
    }

    private int DeleteRecord(string id)
    {
        DataTable dataTable = new DataTable();
        return ObjDAL.GetDataTable("HR_DelDesgnAfterCheck", new Hashtable()
    {
      {
         "@DesignationId",
         id
      }
    }).Rows.Count > 0 ? 1 : 0;
    }

    protected void btnSortOrder_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdDesignation.Rows)
        {
            HiddenField control = row.FindControl("hfDesgId") as HiddenField;
            ObjDAL.ExcuteQryInsUpdt("UPDATE dbo.HR_DesignationMaster SET SortOrder=" + (row.FindControl("txtSortOrder") as TextBox).Text + " WHERE DesgId=" + control.Value);
        }
        FillGrid();
    }
}