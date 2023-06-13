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

public partial class Masters_InitUpdtFine : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       lblmsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
       FillSessionDropdown();
       FillClassDropDown();
       lblRecCount.Text = "";
       grdStudAC.Visible = false;
       btnUpdate.Enabled = false;
       btnUpdate2.Visible = false;
    }

    private void FillClassDropDown()
    {
       drpClass.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
       drpClass.DataTextField = "ClassName";
       drpClass.DataValueField = "ClassID";
       drpClass.DataBind();
       drpClass.Items.Insert(0, new ListItem("-Select-", "0"));
    }

    private void FillSessionDropdown()
    {
       drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
       drpSession.DataTextField = "SessionYr";
       drpSession.DataValueField = "SessionYr";
       drpSession.DataBind();
    }

    private void FillSectionDropDown()
    {
       drpSection.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "classId",
        drpClass.SelectedValue
      },
      {
         "session",
        drpSession.SelectedValue
      }
    });
       drpSection.DataTextField = "Section";
       drpSection.DataValueField = "Section";
       drpSection.DataBind();
       drpSection.Items.Insert(0, new ListItem("ALL", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
       grdStudAC.Visible = false;
       btnUpdate.Enabled = false;
       btnUpdate2.Visible = false;
       lblRecCount.Text = string.Empty;
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
       grdStudAC.Visible = false;
       btnUpdate.Enabled = false;
       btnUpdate2.Visible = false;
       lblRecCount.Text = string.Empty;
       FillSectionDropDown();
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
       grdStudAC.Visible = false;
       btnUpdate.Enabled = false;
       btnUpdate2.Visible = false;
       lblRecCount.Text = string.Empty;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
       grdStudAC.Visible = true;
       FillGrid(this.grdStudAC, "ps_sp_get_StudListForFineInit");
       lblmsg.Text = string.Empty;
    }

    private void FillGrid(GridView gv, string procName)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable ht = new Hashtable();
        if (this.drpSession.Items.Count > 0)
            ht.Add("@Session", this.drpSession.SelectedValue);
        if (this.drpClass.SelectedValue.ToString().Trim() != "0")
            ht.Add("@Class", this.drpClass.SelectedValue);
        if (this.drpSection.SelectedValue.ToString().Trim() != "0")
            ht.Add("@Section", this.drpSection.SelectedValue);
        DataTable dataTable = clsDal.GetDataTable(procName, ht);
        if (dataTable.Rows.Count > 0)
        {
            gv.DataSource = dataTable;
            gv.DataBind();
            gv.Visible = true;
           lblRecCount.Text = "<font color='maroon'><b>No. of Student(s): " + dataTable.Rows.Count.ToString() + "</b></font>";
           lblRecCount.Visible = true;
           btnUpdate.Enabled = true;
           btnUpdate2.Visible = true;
        }
        else
        {
            gv.Visible = false;
           btnUpdate.Enabled = false;
           btnUpdate2.Visible = false;
           lblRecCount.Text = "";
            ScriptManager.RegisterClientScriptBlock((Control)this.btnShow,btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
        }
    }

    private void IntializePage()
    {
       drpClass.SelectedIndex = -1;
       drpSession.SelectedIndex = -1;
       grdStudAC.Visible = false;
       txtDate.Text = "";
       btnUpdate.Enabled = false;
       btnUpdate2.Visible = false;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        Decimal num1 = new Decimal(0);
        foreach (GridViewRow row in grdStudAC.Rows)
        {
            Decimal num2 = new Decimal(0);
            string empty = string.Empty;
            Decimal num3;
            try
            {
                num3 = Convert.ToDecimal(((TextBox)row.Cells[4].FindControl("txtFineAmount")).Text.ToString().Trim());
                empty = Convert.ToString(((Label)row.Cells[0].FindControl("lblAdmnNo")).Text.ToString().Trim());
            }
            catch (Exception ex)
            {
                num3 = new Decimal(0);
            }
            if (num3 > new Decimal(0))
            {
                new clsDAL().GetDataTable("ps_sp_insert_FineLedger", new Hashtable()
        {
          {
             "TransDate",
            dtpFine.GetDateValue().ToString("MM/dd/yyyy")
          },
          {
             "AdmnNo",
             empty
          },
          {
             "TransDesc",
             "Fine- Due not paid"
          },
          {
             "debit",
             num3
          },
          {
             "credit",
             0
          },
          {
             "Receipt_VrNo",
             ""
          },
          {
             "userid",
             Convert.ToInt32(this.Session["User_Id"].ToString().Trim())
          },
          {
             "schoolid",
            Session["SchoolId"].ToString()
          }
        });
                num1 += num3;
            }
        }
        if (num1 > new Decimal(0))
        {
           grdStudAC.Visible = false;
           FillGrid(this.grdStudAC, "ps_sp_get_StudListForFineInit");
           lblmsg.Text = "Fine amount received successfully !";
           lblmsg.ForeColor = Color.Green;
        }
        else
        {
           lblmsg.Text = "Fine amount for all students can not be zero";
           lblmsg.ForeColor = Color.Red;
           lblmsg.Focus();
        }
    }

    protected void BtnShowDefaulters_Click(object sender, EventArgs e)
    {
       grdStudAC.Visible = true;
       FillGrid(this.grdStudAC, "ps_sp_get_DefaultStudListForFine");
       lblmsg.Text = string.Empty;
    }
}