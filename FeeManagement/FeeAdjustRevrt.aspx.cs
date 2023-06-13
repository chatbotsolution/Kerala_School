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
using System.Web.UI.WebControls;

public partial class FeeManagement_FeeAdjustRevrt : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (!Page.IsPostBack)
            {
                fillsession();
                fillclass();
            }
        }
        else
            Response.Redirect("../Login.aspx");
        lblMsg.Text = "";
    }

    private void fillsession()
    {
        obj = new clsDAL();
        drpSession.DataSource = obj.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry("select classname,classid from ps_classmaster");
        drpclass.DataSource = dataTableQry;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("-Select-", "0"));
        if (dataTableQry.Rows.Count > 0)
            return;
        drpclass.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpstudent.Items.Clear();
        drpclass.SelectedIndex = 0;
        txtadmnno.Text = string.Empty;
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    private void fillstudent()
    {
        txtadmnno.Text = "";
        obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        DataTable dataTable = obj.GetDataTable("Ps_sp_ConsdStudList", new Hashtable()
    {
      {
         "@ClassId",
         drpclass.SelectedValue
      },
      {
         "@SessionYr",
         drpSession.SelectedValue.ToString().Trim()
      }
    });
        try
        {
            drpstudent.DataSource = dataTable;
            drpstudent.DataTextField = "fullname";
            drpstudent.DataValueField = "admnno";
            drpstudent.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        if (dataTable.Rows.Count > 0)
        {
            drpstudent.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        else
        {
            drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
            txtadmnno.Text = "";
        }
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillGrid();
    }

    private void FillGrid()
    {
        if (drpstudent.SelectedIndex > 0)
        {
            txtadmnno.Text = drpstudent.SelectedValue.ToString();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@Admnno", drpstudent.SelectedValue.ToString());
            hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = obj.GetDataTable("Ps_sp_GetStudConsAmt", hashtable);
            if (dataTable2.Rows.Count > 0)
            {
                grdConsession.DataSource = null;
                grdConsession.DataSource = dataTable2;
                grdConsession.DataBind();
                btnReveret.Visible = true;
                grdConsession.Visible = true;
            }
            else
            {
                grdConsession.DataSource = null;
                grdConsession.DataBind();
                btnReveret.Visible = false;
                grdConsession.Visible = true;
            }
        }
        else
        {
            lblMsg.Text = "Plese Select Student or Enter AdmnNo to View Details!!!";
            lblMsg.ForeColor = Color.Red;
            txtadmnno.Text = "";
            grdConsession.Visible = false;
            btnReveret.Visible = false;
        }
    }

    protected void btnReveret_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            lblMsg.ForeColor = Color.Red;
            lblMsg.Text = "Please Select a checkbox!!!";
        }
        else
        {
            string[] strArray = Request["Checkb"].Split(',');
            int num = 0;
            for (int index = 0; index < strArray.Length; ++index)
            {
                obj = new clsDAL();
                if (obj.ExecuteScalar("Ps_sp_RevrtStudConsAmt", new Hashtable()
        {
          {
             "@AdjId",
             Convert.ToInt32(strArray[index].ToString())
          }
        }) != "")
                    ++num;
            }
            FillGrid();
            if (num > 0)
            {
                lblMsg.Text = num != 1 ? num.ToString() + " Concessions could not be Reverted" : num.ToString() + " Concession could not be Reverted";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                lblMsg.Text = "Concession Reverted Successfully !";
                lblMsg.ForeColor = Color.Green;
            }
        }
    }

    protected void btnDetail_Click(object sender, EventArgs e)
    {
        try
        {
            obj = new clsDAL();
            Hashtable hashtable = new Hashtable();
            if (txtadmnno.Text.Trim() != "")
                hashtable.Add("@AdmnNo", txtadmnno.Text.Trim());
            DataTable dataTable = obj.GetDataTable("Ps_sp_ConsdStudList", hashtable);
            if (dataTable.Rows.Count > 0)
            {
                drpSession.SelectedValue = dataTable.Rows[0]["SessionYear"].ToString();
                drpclass.SelectedValue = dataTable.Rows[0]["ClassID"].ToString();
                string str = txtadmnno.Text.Trim();
                fillstudent();
                drpstudent.SelectedValue = str;
                ViewState["FY"] = "n";
                drpstudent_SelectedIndexChanged(drpstudent, EventArgs.Empty);
            }
            else
                ScriptManager.RegisterClientScriptBlock((Control)txtadmnno, txtadmnno.GetType(), "ShowMessage", "alert('Admission number does not exists')", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}