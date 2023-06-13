using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_Holidaylist : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["User_Id"] != null)
            {
                txtHolDt.Focus();
                if (Page.IsPostBack)
                    return;

                fillgrid();
            }
            else
                Response.Redirect("../Login.aspx");
        }
        catch (Exception ex)
        {
        }
      //  RblDatePicker.SelectedIndex = 0;
       // txtHolDt2.Visible = false;
     //   txtHolDt2.Enabled = false;
        
    }

    private void fillgrid()
    {
        DataTable dt = new clsDAL().GetDataTableQry("Select *,CASE WHEN ToDate IS NULL OR ToDate=''  Or ToDate=FromDate THEN Convert(varchar,Fromdate,105) ELSE Convert(varchar,Fromdate,106)+' To '+Convert(varchar,Todate,106) End As HDate  from Lib_Holidaylist order by FromDate");
        if (dt.Rows.Count > 0)
        {
            gridHolidayList.DataSource = dt;
            gridHolidayList.DataBind();

        }
        else
        {
            gridHolidayList.DataSource = null;
            gridHolidayList.DataBind();
        }
    }
    protected void btnSaveAddNew_Click(object sender, EventArgs e)
    {
        SaveData();
        fillgrid();
    }

    private void SaveData()
    {
        try
        {
            ht.Clear();

            if (btnSaveAddNew.Text == "Update & AddNew")
            {
                ht.Add("@Id", hfUserId.Value.Trim());
            }
            else
            {
                ht.Add("@Id",0);
            }
            //if (Request.QueryString["Hdate"] == null)
            //    ht.Add("@Hdate", 0);
            //else if (btnSaveAddNew.Text == "Update & AddNew" )
            //    ht.Add("@Hdate", int.Parse(Request.QueryString["Hdate"].ToString()));
            //else
            //    ht.Add("@CatCode", 0);
            ht.Add("@FromDate", txtHolDt.Text.Trim());
            ht.Add("@ToDate", txtHolDt2.Text.Trim());
            if(DdlDay.SelectedIndex > 0)
               ht.Add("@HDay", DdlDay.SelectedItem.ToString());
            ht.Add("@Description", txtDesc.Text.Trim());
            ht.Add("@UserId", Session["User_Id"].ToString().Trim());
          
            dt.Clear();
            dt = obj.GetDataTable("Lib_SP_InsUpdtHolidaylist", ht);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Date Name Already Exist');", true);
            }
            else
            {
                if (btnSaveAddNew.Text == "Update & AddNew" )
                {
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Data Updated Successfully');", true);
                    btnSaveAddNew.Text = "Save & AddNew";
                    //btnSaveGotoList.Text = "Save & GotoList";
                }
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Data Saved Successfully');", true);
                Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void Clear()
    {
        txtHolDt.Text = "";
        txtHolDt2.Text = "";
        txtDesc.Text = "";
        DdlDay.SelectedValue = "0";
        lblImg.Text = "";
       // ViewState["CatPath"] = "";
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }


    protected void gridHolidayList_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = gridHolidayList.SelectedRow;
        txtDesc.Text = (row.FindControl("lblDesc") as Label).Text;
        txtHolDt.Text = (row.FindControl("lblFromDate") as Label).Text;
        txtHolDt2.Text = (row.FindControl("lblToDate") as Label).Text;
        hfUserId.Value = (row.FindControl("lblId") as Label).Text;
        DdlDay.SelectedItem.Text = (row.FindControl("lblDay") as Label).Text;
        btnSaveAddNew.Text = "Update & AddNew";
    }
    protected void gridHolidayList_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "Confirm(' Are you sure to Delete?');", true);
        GridViewRow row = gridHolidayList.Rows[e.RowIndex];
        int id =Convert.ToInt32 ((row.FindControl("lblId") as Label).Text.Trim());
        new clsDAL().ExecuteScalarQry("Delete from Lib_Holidaylist where Id=" + id);
        fillgrid();
    }
    //protected void RblDatePicker_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (RblDatePicker.SelectedValue == "2")
    //    {
    //       // txtHolDt2.Visible = true;
    //       // dtpHolnDt2.Visible = true;
    //        txtHolDt2.Enabled = true;
    //        DdlDay.Enabled = false;
    //    }
    //    else
    //    {
    //        txtHolDt2.Visible = false;
    //        dtpHolnDt2.Visible = false;
    //        DdlDay.Enabled = true;
    //    }

    //}
}