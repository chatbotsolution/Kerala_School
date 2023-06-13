using AjaxControlToolkit;
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

public partial class Accounts_OpenFYList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            FillGrid();
            if (!(Session["userrights"].ToString().Trim() == "a"))
                return;
            btnReInit.Visible = true;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void FillGrid()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = clsDal.GetDataTable("ACTS_GetFinancialYrList");
        if (dataTable2.Rows.Count > 0)
        {
            grdFinancialYear.DataSource = (object)dataTable2.DefaultView;
            grdFinancialYear.DataBind();
            grdFinancialYear.Visible = true;
            lblMsg.Text = "";
        }
        else
        {
            grdFinancialYear.DataSource = (object)null;
            grdFinancialYear.DataBind();
            lblMsg.Text = "No Financial Year Exists";
        }
    }

    protected void grdFinancialYear_RowEditing(object sender, GridViewEditEventArgs e)
    {
        Response.Redirect("OpenFy.aspx?id=" + grdFinancialYear.DataKeys[e.NewEditIndex].Value.ToString().Trim());
    }

    protected void grdFinancialYear_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        string str = clsDal.ExecuteScalarQry("Select EndDate from dbo.ACTS_FinancialYear where FY_Id=" + grdFinancialYear.DataKeys[e.RowIndex].Value.ToString().Trim());
        if (str.Trim() != "" && Convert.ToDateTime(str.Trim()) <= DateTime.Today)
        {
            switch (int.Parse(inpHide.Value))
            {
                case 1:
                    try
                    {
                        clsDal.ExecuteScalarQry("Update ACTS_FinancialYear set IsFinalized=1 where FY_Id=" + grdFinancialYear.DataKeys[e.RowIndex].Value.ToString().Trim());
                        Hashtable hashtable = new Hashtable();
                        if (clsDal.ExecuteScalar("Acts_FinalizeFY", new Hashtable()
            {
              {
                (object) "@FyId",
                (object) grdFinancialYear.DataKeys[e.RowIndex].Value.ToString().Trim()
              },
              {
                (object) "@UserId",
                (object) Session["User_Id"].ToString().Trim()
              },
              {
                (object) "@SchoolId",
                Session["SchoolId"]
              }
            }).Trim() == "")
                        {
                            FillGrid();
                            break;
                        }
                        lblMsg.Text = "Unable to finalize Financial Year!Please Try Again!!";
                        lblMsg.ForeColor = Color.Red;
                        break;
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text = "Unable to finalize Financial Year!Please Try Again!!";
                        lblMsg.ForeColor = Color.Red;
                        break;
                    }
                case 0:
                    try
                    {
                        if (clsDal.ExecuteScalarQry("Update ACTS_FinancialYear set IsFinalized=1 where FY_Id=" + grdFinancialYear.DataKeys[e.RowIndex].Value.ToString().Trim()).Trim() == "")
                        {
                            if (!(clsDal.ExecuteScalar("Acts_InsUpdtStockFY", new Hashtable()
              {
                {
                  (object) "@FyId",
                  (object) grdFinancialYear.DataKeys[e.RowIndex].Value.ToString().Trim()
                },
                {
                  (object) "@UserId",
                  (object) Session["User_Id"].ToString().Trim()
                },
                {
                  (object) "@SchoolId",
                  Session["SchoolId"]
                }
              }).Trim() == ""))
                                break;
                            FillGrid();
                            lblMsg.Text = "Year Finalized Successfully!!";
                            lblMsg.ForeColor = Color.Red;
                            break;
                        }
                        lblMsg.Text = "Unable to finalize Financial Year!Please Try Again!!";
                        lblMsg.ForeColor = Color.Red;
                        break;
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text = "Unable to finalize Financial Year!Please Try Again!!";
                        lblMsg.ForeColor = Color.Red;
                        break;
                    }
            }
        }
        else
        {
            lblMsg.Text = "Financial Year Can only be finalized on Financial year End Date!!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void grdFinancialYear_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow || !(e.Row.Cells[5].Text.ToString().Trim().ToLower() != "running"))
            return;
        e.Row.Cells[0].FindControl("lnkFinalizeYear").Visible = false;
        e.Row.Cells[0].FindControl("lnkYearReset").Visible = false;
        e.Row.Cells[0].FindControl("lblStatus").Visible = true;
        ((Label)e.Row.Cells[0].FindControl("lblStatus")).Text = "Year finalized";
        ((WebControl)e.Row.Cells[0].FindControl("lblStatus")).ForeColor = Color.Red;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (new clsDAL().ExecuteScalarQry("Select top(1) EndDate from dbo.ACTS_FinancialYear where IsFinalized=0 or IsFinalized is null order by FY_Id desc").Trim() != "")
            Response.Redirect("OpenFY.aspx");
        else
            Response.Redirect("OpenFY.aspx");
    }

    protected void btnReInit_Click(object sender, EventArgs e)
    {
        Response.Redirect("OpenFYReInit.aspx");
    }
}