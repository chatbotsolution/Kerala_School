using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Hostel_HostFeeAmount : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
        {
            lblMsg.Text = "";
            fillsession();
            FillClass();
            SetBtnStatus(false);
            grid.Visible = false;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void fillsession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillClass()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        drpClass.DataSource = clsDal.GetDataTableQry("select ClassId,ClassName from dbo.PS_ClassMaster order  by ClassId");
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassId";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    private void SetBtnStatus(bool st)
    {
        btnSaveAddNew.Enabled = st;
        btnSaveAddNew2.Enabled = st;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = "";
    }

    protected void GridFill()
    {
        DataSet dataSet = new clsDAL().GetDataSet("Host_GetFeeAmount", new Hashtable()
    {
      {
         "@SessionYr",
         drpSession.SelectedValue.ToString().Trim()
      },
      {
         "@ClassId",
         drpClass.SelectedValue.ToString().Trim()
      }
    });
        ViewState["FeeTbl"] = dataSet.Tables[0];
        grdFeeAmount.DataSource = dataSet.Tables[0];
        grdFeeAmount.DataBind();
        if (dataSet.Tables[1].Rows.Count <= 0)
            return;
        grid.Visible = true;
        string str = dataSet.Tables[1].Rows[0][0].ToString().Trim();
        ViewState["mode"] = dataSet.Tables[1].Rows[0][1].ToString().Trim();
        if (str == "No")
        {
            SetBtnStatus(true);
        }
        else
        {
            lblMsg.Text = "Fees for this session year is already finalized can not be modified now.";
            lblMsg.ForeColor = Color.Red;
            SetBtnStatus(false);
        }
    }

    protected void btnSaveAddNew_Click(object sender, EventArgs e)
    {
        double num1 = 0.0;
        if (grdFeeAmount.Rows.Count > 0)
        {
            foreach (GridViewRow row in grdFeeAmount.Rows)
            {
                TextBox control1 = (TextBox)row.FindControl("txtExistingAmount");
                TextBox control2 = (TextBox)row.FindControl("txtNewAmount");
                TextBox control3 = (TextBox)row.FindControl("txtTCAmount");
                double num2;
                try
                {
                    num2 = Convert.ToDouble(((TextBox)row.FindControl("txtExistingAmount")).Text.Trim()) + Convert.ToDouble(((TextBox)row.FindControl("txtNewAmount")).Text.Trim());
                }
                catch
                {
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Invalid  Amount');", true);
                    return;
                }
                if (num2 > 0.0)
                    num1 += num2;
            }
        }
        if (num1 > 0.0)
        {
            SaveAmount();
            grid.Visible = false;
            lblMsg.Text = "Fee Amount Set Successfully";
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = "Fee amount for all the components can not be zero";
            lblMsg.ForeColor = Color.Red;
            lblMsg.Focus();
        }
    }

    private void SaveAmount()
    {
        if (grdFeeAmount.Rows.Count <= 0)
            return;
        foreach (GridViewRow row in grdFeeAmount.Rows)
        {
            double num1;
            double num2;
            try
            {
                num1 = Convert.ToDouble(((TextBox)row.FindControl("txtExistingAmount")).Text.Trim());
                num2 = Convert.ToDouble(((TextBox)row.FindControl("txtNewAmount")).Text.Trim());
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Invalid  Amount');", true);
                break;
            }
            Hashtable hashtable = new Hashtable();
            if (num1 > 0.0 || num2 > 0.0)
            {
                hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
                hashtable.Add("@FeeCompID", ((Label)row.FindControl("lblFeeID")).Text.Trim());
                hashtable.Add("@ClassId", ((Label)row.FindControl("lblClassID")).Text.Trim());
                hashtable.Add("@Amount", num1);
                hashtable.Add("@UserID", Convert.ToInt32(Session["User_Id"]));
                hashtable.Add("@SchoolID", Session["SchoolId"].ToString());
                hashtable.Add("@StudType", "E");
                new clsDAL().GetDataTable("Host_InsertFeeAmount", hashtable);
                hashtable.Remove("@Amount");
                hashtable.Remove("@StudType");
                hashtable.Add("@StudType", "N");
                hashtable.Add("@Amount", num2);
                new clsDAL().GetDataTable("Host_InsertFeeAmount", hashtable);
            }
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GridFill();
        lblMsg.Text = string.Empty;
    }

    public string GetTot(string Amount)
    {
        return (ViewState["FeeTbl"] as DataTable).Compute("Sum(" + Amount + ")", "").ToString();
    }
}