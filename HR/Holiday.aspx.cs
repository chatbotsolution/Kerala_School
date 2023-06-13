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

public partial class HR_Holiday : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DateTime fromDt;
    private DateTime toDt;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        trMsg.Style["Background-Color"] = (string)null;
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        bindDropDown(drpYear, "SELECT DISTINCT(YEAR(ISNULL(HolidayDate,GETDATE()))) AS HolidayYear FROM dbo.HR_HolidayMaster", "HolidayYear", "HolidayYear");
        drpYear.Items.RemoveAt(0);
        drpYear.SelectedValue = DateTime.Now.Year.ToString();
        FillGrid();
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = obj.GetDataTableQry(query);
        if (dataTableQry.Rows.Count == 0)
        {
            DataRow row = dataTableQry.NewRow();
            row["HolidayYear"] = DateTime.Now.Year;
            dataTableQry.Rows.InsertAt(row, 0);
            dataTableQry.AcceptChanges();
        }
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    private void FillGrid()
    {
        fromDt = Convert.ToDateTime("Jan-01-" + drpYear.SelectedValue + " 00:00:00");
        toDt = Convert.ToDateTime("Dec-31-" + drpYear.SelectedValue + " 23:59:59");
        DataTable dataTable = obj.GetDataTable("HR_GetHolidayDetails", new Hashtable()
    {
      {
         "@FromDt",
         fromDt
      },
      {
         "@ToDt",
         toDt
      }
    });
        grdHoliday.DataSource = dataTable.DefaultView;
        grdHoliday.DataBind();
        lblRecCount.Text = "No of Record(s) : " + dataTable.Rows.Count;
        if (dataTable.Rows.Count > 0)
        {
            btnDelete.Visible = true;
            btnWorking.Visible = true;
        }
        else
        {
            btnDelete.Visible = false;
            btnWorking.Visible = false;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int num = 0;
            if (Request["Checkb"] == null)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select any Record');", true);
            else if (getValidInput())
            {
                string[] strArray = Request["Checkb"].Split(',');
                for (int index = 0; index < strArray.Length; ++index)
                {
                    if (DelHoliday(strArray[index]))
                        ++num;
                }
                if (num == strArray.Length)
                {
                    trMsg.Style["Background-Color"] = "Green";
                    lblMsg.Text = "Records Deleted Successfully!";
                    FillGrid();
                }
                else if (num == 0)
                {
                    trMsg.Style["Background-Color"] = "Red";
                    lblMsg.Text = "Records Cannot Be Deleted!!Attendance on this date has already been made!";
                }
                else
                {
                    trMsg.Style["Background-Color"] = "White";
                    lblMsg.Text = "<span style='color:Green;'>" + num.ToString() + " Records deleted successfully!!</span> <span style='color:Red;'>" + (strArray.Length - num).ToString() + " Records cannot be deleted as attendance already made on this date</span>";
                    FillGrid();
                }
            }
            else
            {
                lblMsg.Text = "Select only one checkbox !";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = ex.Message;
        }
    }

    private bool getValidInput()
    {
        int num = 0;
        string[] strArray = Request["Checkb"].Split(',');
        for (int index = 0; index < strArray.Length; ++index)
            ++num;
        return num <= 1;
    }

    private bool DelHoliday(string HId)
    {
        return obj.ExecuteScalar("HR_DelHoliday", new Hashtable()
    {
      {
         "@HolidayId",
         HId.Trim()
      }
    }).Trim() == "";
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("Holidaydetail.aspx");
    }

    protected void drpYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    protected void grdHoliday_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdHoliday.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void btnWorking_Click(object sender, EventArgs e)
    {
        try
        {
            int num = 0;
            if (Request["Checkb"] == null)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select any Record');", true);
            else if (getValidInput())
            {
                string[] strArray = Request["Checkb"].Split(',');
                for (int index = 0; index < strArray.Length; ++index)
                {
                    if (InsertWorking(strArray[index]))
                        ++num;
                }
                if (num == strArray.Length)
                {
                    trMsg.Style["Background-Color"] = "Green";
                    lblMsg.Text = "Record Updated Successfully!";
                    lblMsg.ForeColor = Color.White;
                    FillGrid();
                }
                else if (num == 0)
                {
                    trMsg.Style["Background-Color"] = "Red";
                    lblMsg.Text = "Cannot mark this day as working!";
                    lblMsg.ForeColor = Color.White;
                }
                else
                {
                    trMsg.Style["Background-Color"] = "White";
                    lblMsg.Text = "<span style='color:Green;'>" + num.ToString() + " Records deleted successfully!!</span> <span style='color:Red;'>" + (strArray.Length - num).ToString() + " Records cannot be deleted as attendance already made on this date</span>";
                    lblMsg.ForeColor = Color.White;
                    FillGrid();
                }
            }
            else
            {
                lblMsg.Text = "Select only one checkbox !";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = ex.Message;
        }
    }

    private bool InsertWorking(string HId)
    {
        return obj.ExecuteScalar("HR_InsWorkingHoliday", new Hashtable()
    {
      {
         "@HolidayId",
         HId.Trim()
      },
      {
         "@UserId",
         Session["User_Id"].ToString().Trim()
      }
    }).Trim() == "";
    }

    protected void grdHoliday_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        Label control1 = (Label)e.Row.FindControl("lblStatus");
        Label control2 = (Label)e.Row.FindControl("lblEdit");
        if (!(control1.Text.Trim() == "Working"))
            return;
        control2.Visible = false;
    }
}