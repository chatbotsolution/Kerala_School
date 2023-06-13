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

public partial class Accounts_DefineBoard : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            ViewState["Id"] = "";
            BindGrid();
            BoardList.Visible = true;
            BoardDetails.Visible = false;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void BindGrid()
    {
        DataTable dataTable = new clsDAL().GetDataTable("ACTS_GetBoard");
        if (dataTable.Rows.Count > 0)
        {
            lblRecord.Text = "Total Record : " + dataTable.Rows.Count.ToString();
            grdBoard.DataSource = dataTable;
            grdBoard.DataBind();
            grdBoard.Visible = true;
        }
        else
            grdBoard.Visible = false;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        if (SaveData().Rows.Count > 0)
        {
            lblMsg.Text = "Data Already Exist";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            BoardDetails.Visible = true;
            BoardList.Visible = false;
            lblMsg.Text = "Data Saved Successfully";
            lblMsg.ForeColor = Color.Green;
        }
    }

    public DataTable SaveData()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@BoardOrUnivName", txtBoardName.Text.ToString().Trim());
        DataTable dataTable2;
        if (ViewState["Id"].ToString().Trim() == "")
        {
            dataTable2 = clsDal.GetDataTable("ACTS_InsertBoardMaster", hashtable);
            BindGrid();
            BoardList.Visible = true;
            txtBoardName.Text = "";
        }
        else
        {
            hashtable.Add("@BoardOrUnivId", int.Parse(ViewState["Id"].ToString()));
            dataTable2 = clsDal.GetDataTable("ACTS_UpdateBoardMaster", hashtable);
            BoardList.Visible = true;
            txtBoardName.Text = "";
            BindGrid();
        }
        return dataTable2;
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Welcome.aspx");
    }

    private void ClearAccountGroupControls()
    {
        txtBoardName.Text = "";
        ViewState["Id"] = "";
    }

    protected void grdBoard_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdBoard.PageIndex = e.NewPageIndex;
        BindGrid();
    }

    protected void grdBoard_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            DataTable dataTable1 = new DataTable();
            Hashtable hashtable = new Hashtable();
            clsDAL clsDal = new clsDAL();
            Label control = (Label)grdBoard.Rows[e.RowIndex].FindControl("lbl2");
            hashtable.Add("@BoardOrUnivId", control.Text);
            DataTable dataTable2 = clsDal.GetDataTable("ACTS_DelBoard", hashtable);
            BindGrid();
            if (dataTable2.Rows.Count > 0)
            {
                lblMsg.Text = "Record Cant be Deleted!";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                lblMsg.Text = "Record Deleted Successfully";
                lblMsg.ForeColor = Color.Green;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        BoardDetails.Visible = true;
        BoardList.Visible = false;
        ViewState["Id"] = "";
        lblMsg.Text = "";
    }

    protected void grdBoard_RowEditing1(object sender, GridViewEditEventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        grdBoard.EditIndex = e.NewEditIndex;
        BindGrid();
        DataTable dataTableQry = clsDal.GetDataTableQry("select * from ACTS_BoardUniv where BoardOrUnivId='" + grdBoard.DataKeys[e.NewEditIndex].Value.ToString().Trim() + "'");
        if (dataTableQry.Rows.Count > 0)
        {
            txtBoardName.Text = dataTableQry.Rows[0]["BoardOrUnivName"].ToString();
            ViewState["Id"] = dataTableQry.Rows[0]["BoardOrUnivId"].ToString();
        }
        BoardDetails.Visible = true;
        BoardList.Visible = false;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtBoardName.Text = "";
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        ViewState["Id"] = "";
        BoardDetails.Visible = false;
        BoardList.Visible = true;
        ClearAccountGroupControls();
        BindGrid();
        lblMsg.Text = "";
    }
}