using ASP;
using Classes.DA;
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

public partial class Masters_FeeAmount : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
        {
            lblMsg.Text = "";
            lblClass.Text = lblStream.Text = "";
            fillsession();
            FillStreamDropDown();
            FillClassDropDown();
            SetBtnStatus(false);
            grid.Visible = false;
            if (!(Session["userrights"].ToString().Trim() == "a"))
                return;
            btnCopyAmt.Visible = true;
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillStreamDropDown()
    {
        Common common = new Common();
        DataTable dataTable = new DataTable();
        drpStream.DataSource = common.ExecuteSql("select StreamID,Description from dbo.PS_StreamMaster order  by StreamID");
        drpStream.DataTextField = "Description";
        drpStream.DataValueField = "StreamID";
        drpStream.DataBind();
        drpStream.Items.RemoveAt(0);
        drpStream.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    private void FillClassDropDown()
    {
        Common common = new Common();
        DataTable dataTable = new DataTable();
        drpClass.DataSource = common.ExecuteSql("select ClassID,ClassName from dbo.PS_ClassMaster order  by ClassID");
        drpClass.DataValueField = "ClassID";
        drpClass.DataTextField = "ClassName";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    private void SetBtnStatus(bool st)
    {
        btnSaveAddNew.Enabled = st;
        btnSaveAddNew2.Enabled = st;
        BtnReGenFee.Enabled = st;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        GridFill();
    }

    protected void GridFill()
    {
        if (drpClass.SelectedValue.ToString().Trim() == "0")
        {
            lblClass.Text = "";

        }
        

        else
            lblClass.Text = drpClass.SelectedItem.Text.Trim();
        //string strclass = drpClass.SelectedValue.ToString().Trim();
        if (Convert.ToInt32(drpClass.SelectedValue.ToString().Trim()) > 13)
        {
            ViewState["Stream"] = drpStream.SelectedValue.ToString().Trim();
            lblStream.Visible = true;
            lblStream.Text = drpStream.SelectedItem.Text.Trim(); ;
        }
        else
        {
            ViewState["Stream"] = "1";
            lblStream.Text = "";
        }
       
        
       

        DataSet dataSet = new Common().GetDataSet("ps_sp_get_FeeAmountNew", new Hashtable()
    {
      {
         "@SessionYr",
         drpSession.SelectedValue.ToString().Trim()
      },
       {
         "@ClassID",
         drpClass.SelectedValue.ToString().Trim()
      },
      {
         "@StreamID",
          ViewState["Stream"].ToString()
      }
    });
        ViewState["FeeTbl"] = dataSet.Tables[0];
        grdFeeAmount.DataSource = dataSet.Tables[0];
        grdFeeAmount.DataBind();
        if (dataSet.Tables[1].Rows.Count <= 0)
            return;
        grid.Visible = true;

        if (drpClass.SelectedValue.ToString().Trim() == "1")
        {
            foreach (GridViewRow row in grdFeeAmount.Rows)
            {
                TextBox control1 = (TextBox)row.FindControl("txtExistingAmount");
                control1.Enabled = false;
            }

        }
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
            grid.Visible = false;
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
               // TextBox control3 = (TextBox)row.FindControl("txtTCAmount");
                double num2;
                try
                {
                    num2 = Convert.ToDouble(((TextBox)row.FindControl("txtExistingAmount")).Text.Trim()) + Convert.ToDouble(((TextBox)row.FindControl("txtNewAmount")).Text.Trim());
                    // + Convert.ToDouble(((TextBox)row.FindControl("txtTCAmount")).Text.Trim()) + Convert.ToDouble(((TextBox)row.FindControl("txtCasualAmount")).Text.Trim()
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
            ExistStudEntry();
            NewStudEntry();
           // TCStudEntry();
           // CasualStudEntry();
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

    private void ExistStudEntry()
    {
        if (grdFeeAmount.Rows.Count <= 0)
            return;
        if (drpClass.SelectedValue.ToString().Trim() == "1" || drpClass.SelectedItem.Text.Trim()=="NRY")
        {
            return;
        }
        foreach (GridViewRow row in grdFeeAmount.Rows)
        {
            TextBox control = (TextBox)row.FindControl("txtExistingAmount");
            double num;
            try
            {
                num = Convert.ToDouble(((TextBox)row.FindControl("txtExistingAmount")).Text.Trim());
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock((Control)control, control.GetType(), "ShowMessage", "alert('Invalid  Amount');", true);
                break;
            }
            InsertFeeAmt(new Hashtable()
      {
        {
           "SessionYr",
           drpSession.SelectedValue.ToString().Trim()
        },
        {
           "feecompid",
           ((Label) row.FindControl("lblFeeID")).Text.Trim()
        },
         {
           "classid",
           ((Label) row.FindControl("lblClassID")).Text.Trim()
        },
        {
           "streamid",
           ((Label) row.FindControl("lblStreamID")).Text.Trim()
        },
        {
           "amount",
           num
        },
        {
           "UserID",
           Convert.ToInt32(Session["User_Id"])
        },
        {
           "SchoolID",
           Session["SchoolId"].ToString()
        },
        {
           "mode",
           ViewState["mode"].ToString().Trim()
        },
        {
           "StudType",
           "E"
        }
      });
        }
    }

    private void NewStudEntry()
    {
        if (grdFeeAmount.Rows.Count <= 0)
            return;
        foreach (GridViewRow row in grdFeeAmount.Rows)
        {
            TextBox control = (TextBox)row.FindControl("txtNewAmount");
            double num;
            try
            {
                num = Convert.ToDouble(((TextBox)row.FindControl("txtNewAmount")).Text.Trim());
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock((Control)control, control.GetType(), "ShowMessage", "alert('Invalid  Amount');", true);
                break;
            }
            InsertFeeAmt(new Hashtable()
      {
        {
           "SessionYr",
           drpSession.SelectedValue.ToString().Trim()
        },
        {
           "feecompid",
           ((Label) row.FindControl("lblFeeID")).Text.Trim()
        },
         {
           "classid",
           ((Label) row.FindControl("lblClassID")).Text.Trim()
        },
        {
           "streamid",
           ((Label) row.FindControl("lblStreamID")).Text.Trim()
        },
        {
           "amount",
           num
        },
        {
           "UserID",
           Convert.ToInt32(Session["User_Id"])
        },
        {
           "SchoolID",
           Session["SchoolId"].ToString()
        },
        {
           "mode",
           ViewState["mode"].ToString().Trim()
        },
        {
           "StudType",
           "N"
        }
      });
        }
    }

    private void TCStudEntry()
    {
        if (grdFeeAmount.Rows.Count <= 0)
            return;
        foreach (GridViewRow row in grdFeeAmount.Rows)
        {
            TextBox control = (TextBox)row.FindControl("txtTCAmount");
            double num;
            try
            {
                num = Convert.ToDouble(((TextBox)row.FindControl("txtTCAmount")).Text.Trim());
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock((Control)control, control.GetType(), "ShowMessage", "alert('Invalid  Amount');", true);
                break;
            }
            InsertFeeAmt(new Hashtable()
      {
        {
           "SessionYr",
           drpSession.SelectedValue.ToString().Trim()
        },
        {
           "feecompid",
           ((Label) row.FindControl("lblFeeID")).Text.Trim()
        },
        {
           "classid",
           ((Label) row.FindControl("lblClassID")).Text.Trim()
        },
        {
           "streamid",
           ((Label) row.FindControl("lblStreamID")).Text.Trim()
        },
        {
           "amount",
           num
        },
        {
           "UserID",
           Convert.ToInt32(Session["User_Id"])
        },
        {
           "SchoolID",
           Session["SchoolId"].ToString()
        },
        {
           "mode",
           ViewState["mode"].ToString().Trim()
        },
        {
           "StudType",
           "T"
        }
      });
        }
    }

    private void CasualStudEntry()
    {
        if (grdFeeAmount.Rows.Count <= 0)
            return;
        foreach (GridViewRow row in grdFeeAmount.Rows)
        {
            TextBox control = (TextBox)row.FindControl("txtCasualAmount");
            double num;
            try
            {
                num = Convert.ToDouble(((TextBox)row.FindControl("txtCasualAmount")).Text.Trim());
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock((Control)control, control.GetType(), "ShowMessage", "alert('Invalid  Amount');", true);
                break;
            }
            InsertFeeAmt(new Hashtable()
      {
        {
           "SessionYr",
           drpSession.SelectedValue.ToString().Trim()
        },
        {
           "feecompid",
           ((Label) row.FindControl("lblFeeID")).Text.Trim()
        },
         {
           "classid",
           ((Label) row.FindControl("lblClassID")).Text.Trim()
        },
        {
           "streamid",
           ((Label) row.FindControl("lblStreamID")).Text.Trim()
        },
        {
           "amount",
           num
        },
        {
           "UserID",
           Convert.ToInt32(Session["User_Id"])
        },
        {
           "SchoolID",
           Session["SchoolId"].ToString()
        },
        {
           "mode",
           ViewState["mode"].ToString().Trim()
        },
        {
           "StudType",                                                                                                                                                                                                 
           "C"
        }
      });
        }
    }

    protected void InsertFeeAmt(Hashtable ht1)
    {
        new Common().GetDataTable("ps_sp_insert_FeeAmount", ht1);
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("MastersHome.aspx");
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

    protected void BtnReGenFee_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        BtnReGenFee.Enabled = false;
        if (new clsDAL().ExecuteScalarQry("select finalised from dbo.PS_FeeNormsNew where SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "'").ToLower() == "yes")
        {
            lblMsg.Text = "The selected session  year is finalized. Fee cann't be regenerated now.";
            lblMsg.ForeColor = Color.Red;
        }
        else
            GetStreamClass();
    }

    protected void GetStreamClass()
    {
        foreach (DataRow row in (InternalDataCollectionBase)new clsDAL().GetDataTableQry("Select ClassID from PS_ClassMaster where classid=" + drpClass.SelectedValue.ToString().Trim()).Rows)
            GenerateFeeAll(row["ClassID"].ToString());
    }

    protected void GenerateFeeAll(string ClassId)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select distinct c.admnno,s.StudType from dbo.PS_ClasswiseStudent c ");
        stringBuilder.Append(" inner join dbo.PS_StudMaster s on s.AdmnNo=c.AdmnNo");
        stringBuilder.Append(" where c.SessionYear='" + drpSession.SelectedValue.ToString().Trim() + "'");
        stringBuilder.Append(" and c.ClassID=" + ClassId.Trim());
        stringBuilder.Append(" order by c.admnno");
        DataTable dataTableQry = clsDal.GetDataTableQry(stringBuilder.ToString());
        int num = 0;
        foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
        {
            try
            {
                InsertDetailsInLedger(row["admnno"].ToString().Trim(), row["StudType"].ToString().Trim());
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                ++num;
                BtnReGenFee.Enabled = true;
            }
        }
        if (num > 0)
        {
            lblMsg.Text = "Records could not be generated: " + num.ToString();
            lblMsg.ForeColor = Color.Red;
            BtnReGenFee.Enabled = true;
        }
        else
        {
            lblMsg.Text = "All records generated successfully";
            lblMsg.ForeColor = Color.Green;
            BtnReGenFee.Enabled = true;
        }
    }

    private void InsertDetailsInLedger(string Admn, string stype)
    {
        try
        {
            clsDAL clsDal = new clsDAL();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@SessYr", drpSession.SelectedValue.ToString().Trim());
            hashtable.Add("@AdmnNo", Admn);
            string str1 = "04/01/" + drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
            hashtable.Add("@TransDt", str1);
            string FeeOT = "Y";
            string FeeAnnual = "Y";
            clsGenerateFee clsGenerateFee = new clsGenerateFee();
            string str2 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
            string str3 = drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
            DateTime dateTime = Convert.ToDateTime("04/01/" + str2);
            clsGenerateFee.GenerateFeeOnFeeMod(dateTime, dateTime, Admn, FeeOT, FeeAnnual, drpSession.SelectedValue.ToString().Trim(), Session["User_Id"].ToString(), Session["SchoolId"].ToString(), char.Parse(stype));
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message.ToString();
            BtnReGenFee.Enabled = true;
        }
    }

    protected void btnCopyAmt_Click(object sender, EventArgs e)
    {
        if (drpStream.SelectedIndex > 0)
        {
            DataSet dataSet = new Common().GetDataSet("ps_sp_get_FeeAmount", new Hashtable()
      {
        {
           "@SessionYr",
           drpSession.SelectedValue.ToString().Trim()
        },
        {
           "@StreamID",
           drpStream.Items[drpStream.SelectedIndex - 1].Value.ToString().Trim()
        }
      });
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    row["StreamID"] = drpStream.SelectedValue.ToString().Trim();
                    row["Description"] = drpStream.SelectedItem.Text.Trim();
                    row.AcceptChanges();
                }
            }
            ViewState["FeeTbl"] = dataSet.Tables[0];
            grdFeeAmount.DataSource = dataSet.Tables[0];
            grdFeeAmount.DataBind();
        }
        else
        {
            lblMsg.Text = "No Prev Data Available!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void drpStream_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        GridFill();
        grdFeeAmount.Visible = true;
    }
    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToInt32(drpClass.SelectedValue) >13)
        {
            drpStream.Visible = true;
            grid.Visible = false;
        }
        else
        {
            drpStream.Visible = false;
            grid.Visible = true;
            GridFill();

        }
    }
}