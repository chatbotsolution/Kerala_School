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


public partial class Hostel_InventoryReturnList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            lblMsg.Text = string.Empty;
            if (Page.IsPostBack)
                return;
            Page.Form.DefaultButton = btnSearch.UniqueID;
            bindDropDown(drpReturnedBy, "select S.FullName,H.AdmnNo from dbo.PS_StudMaster S inner join dbo.Host_Admission H on S.AdmnNo=H.AdmnNo Where H.LeavingDt is null or LeavingDt ='' ", "FullName", "AdmnNo");
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            bindGrid();
        }
        catch (Exception ex)
        {
        }
    }

    private void bindGrid()
    {
        if (txtFrmDt.Text != "")
            ht.Add("@FromDate", PopCalendar1.GetDateValue());
        if (txtTo.Text != "")
            ht.Add("@Todate", PopCalendar2.GetDateValue());
        if (drpReturnedBy.SelectedIndex > 0)
            ht.Add("@AdmnNo", drpReturnedBy.SelectedValue);
        ht.Add("@SchoolId", Convert.ToInt32(Session["SchoolId"]));
        ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
        dt = obj.GetDataTable("Host_GetAllReturn", ht);
        grdItem.DataSource = dt;
        grdItem.DataBind();
        if (dt.Rows.Count != 0)
            return;
        lblMsg.Text = "No Record !";
        lblMsg.ForeColor = Color.Red;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("InventoryReturn.aspx");
    }

    protected void grdItem_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdItem.PageIndex = e.NewPageIndex;
        bindGrid();
    }

    protected void grdItem_PreRender(object sender, EventArgs e)
    {
        MergeCell(new int[3] { 0, 1, 2 });
    }

    public void MergeCell(int[] colIndices)
    {
        foreach (int colIndex in colIndices)
        {
            for (int index = grdItem.Rows.Count - 2; index >= 0; --index)
            {
                GridViewRow row1 = grdItem.Rows[index];
                GridViewRow row2 = grdItem.Rows[index + 1];
                if (row1.Cells[colIndex].Text == row2.Cells[colIndex].Text)
                {
                    row1.Cells[colIndex].RowSpan = row2.Cells[colIndex].RowSpan < 2 ? 2 : row2.Cells[colIndex].RowSpan + 1;
                    row2.Cells[colIndex].Visible = false;
                }
            }
        }
    }
}