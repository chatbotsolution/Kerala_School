using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Admissions_StudentWithdrawal : System.Web.UI.Page
{
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        fillsession();
        fillclass();
        fillstudent();
        //  DRP.FillStatus(drpStatus);
        fillstatus();
       

        btnFeeReceive.Enabled = false;
        if (Request.QueryString["admnno"] == null)
            return;
        FillSelStudent();
        fillstudent();
        hfTxtAdmno.Text = drpstudent.SelectedValue;
        DataTable dt = new clsDAL().GetDataTableQry("Select OldAdmnNo from Ps_Studmaster where AdmnNo =" + drpstudent.SelectedValue.ToString().Trim() + "");
        txtStudId.Text = dt.Rows[0]["OldAdmnNo"].ToString().Trim();
        ViewState["AdmnNo"] = hfTxtAdmno.Text.Trim();
        ViewState["OldAdmnNo"] = dt.Rows[0]["OldAdmnNo"].ToString().Trim();
    }

    private void FillSelStudent()
    {
        drpSession.SelectedValue = Request.QueryString["sess"].ToString();
        if (Request.QueryString["cid"].ToString() != "0")
            drpclass.SelectedValue = Request.QueryString["cid"].ToString();
        else
            drpclass.SelectedValue = new clsDAL().GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent where AdmnNo=" + Request.QueryString["admnno"].ToString() + " and Detained_Promoted=''").Rows[0]["ClassID"].ToString();
        fillstudent();
        drpstudent.SelectedValue = Request.QueryString["admnno"].ToString().Trim();
    }

    private void fillsession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = new clsDAL().GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("All", "0"));
    }

    private void fillstudent()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTableQry;
        if (drpclass.SelectedIndex != 0)
            dataTableQry = clsDal.GetDataTableQry("select fullname,cs.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.classid=" + drpclass.SelectedValue + " and cs.SessionYear='" + drpSession.SelectedValue.ToString() + "' and StatusId in(1,4)  order by fullname");
        else
            dataTableQry = clsDal.GetDataTableQry("select fullname,cs.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.SessionYear='" + drpSession.SelectedValue.ToString() + "' and StatusId in(1,4) order by fullname");
        drpstudent.DataSource = dataTableQry;
        drpstudent.DataTextField = "fullname";
        drpstudent.DataValueField = "admnno";
        drpstudent.DataBind();
        if (dataTableQry.Rows.Count > 0)
            drpstudent.Items.Insert(0, new ListItem("--All--", "0"));
        else
            drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblDues.Text = "";
        fillstudent();
        txtStudId.Text = string.Empty;
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblDues.Text = "";
        fillstudent();
        txtStudId.Text = string.Empty;
        drpStatus.SelectedIndex = 0;
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblDues.Text = "";
        DataTable dt = new clsDAL().GetDataTableQry("Select AdmnNo,OldAdmnNo from Ps_Studmaster where AdmnNo =" + drpstudent.SelectedValue.ToString().Trim() + "");
        
        txtStudId.Text = dt.Rows[0]["AdmnNo"].ToString().Trim();
        hfTxtAdmno.Text = drpstudent.SelectedValue.ToString().Trim();
        ViewState["AdmnNo"] = dt.Rows[0]["AdmnNo"].ToString().Trim();

        ViewState["OldAdmnNo"] = dt.Rows[0]["OldAdmnNo"].ToString().Trim();


        drpStatus.SelectedIndex = 0;
        GetStudDetails();
    }

    protected void GetStudDetails()
    {
        btnPrint.Enabled = false;
        lblStudDet.Text = new clsDAL().ExecuteScalarQry("select FullName + '(DOB: ' + convert(varchar,DOB,106) + ')' + ', Father Name: ' + FatherName from dbo.PS_StudMaster where AdmnNo=" + drpstudent.SelectedValue);
        ViewState["AdmnNo"] = hfTxtAdmno.Text.Trim();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = clsDal.GetDataTableQry("select SessionYear,ClassID,s.admnno from dbo.PS_ClasswiseStudent c inner join  dbo.PS_StudMaster s on c.admnno=s.AdmnNo where s.admnno='" + txtStudId.Text.ToString().Trim() + "' and StatusId in(1,4) and Detained_Promoted='' ");
        if (dataTableQry.Rows.Count > 0)
        {
            fillsession();
            drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
            fillclass();
            drpclass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
            fillstudent();
            drpstudent.SelectedValue = dataTableQry.Rows[0]["admnno"].ToString();
            GetStudDetails();
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('Invalid Admission Number !');", true);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string s = new clsDAL().ExecuteScalarQry("SELECT ISNULL(StatusId,0) FROM PS_StudWithdrawl WHERE AdmnNo='" + drpstudent.SelectedValue.ToString().Trim() + "' AND fromclass=" + drpclass.SelectedValue.ToString().Trim() + " AND StatusId in (2,3)");
        if (s != "")
        {
            if (int.Parse(s) == 2 || int.Parse(s) == 3)
            {
                if (int.Parse(s) == 2)
                    ScriptManager.RegisterClientScriptBlock((Control)drpstudent, drpstudent.GetType(), "ShowMessage", "alert('Passed out student status can not be modify');", true);
                else
                    ScriptManager.RegisterClientScriptBlock((Control)drpstudent, drpstudent.GetType(), "ShowMessage", "alert('TC student status can not be modify');", true);
            }
            else
                ModifyStudStatus();
        }
        else
            ModifyStudStatus();
    }

    private void ModifyStudStatus()
    {
        try
        {
            if (drpstudent.SelectedValue.ToString() == "0")
            {
                ScriptManager.RegisterClientScriptBlock((Control)drpstudent, drpstudent.GetType(), "ShowMessage", "alert('Select a Student');", true);
            }
            else
            {
                new clsDAL().ExcuteProcInsUpdt("ps_sp_insert_StundentWithdrawal", new Hashtable()
        {
          {
             "adminno",
             Convert.ToInt64(drpstudent.SelectedValue.ToString())
          },
          {
             "withdrawaldate",
             PopCalendar2.GetDateValue().ToString("MM/dd/yyyy")
          },
          {
             "StatusId",
             drpStatus.SelectedValue
          },
          {
             "fromclass",
             drpclass.SelectedValue
          },
          {
             "reason",
             txtReason.Text
          },
          {
             "UserID",
             Convert.ToInt32(Session["User_Id"])
          },
          {
             "UserDate",
             DateTime.Now.ToString("MM/dd/yyyy")
          },
          {
             "schoolid",
             Session["SchoolId"].ToString()
          }
        });
                ScriptManager.RegisterClientScriptBlock((Control)txtReason, txtReason.GetType(), "ShowMessage", "alert('Status updated successfully');", true);
                btnPrint.Enabled = true;
                ClearAll();
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public string checkDate(string dt2)
    {
        try
        {
            CultureInfo cultureInfo = new CultureInfo("en-US")
            {
                DateTimeFormat = new DateTimeFormatInfo()
                {
                    ShortDatePattern = "dd-MM-yyyy"
                }
            };
            DateTime dateTime = DateTime.Parse(dt2);
            return dateTime.Year.ToString() + "/" + ("0" + dateTime.Month).Substring(("0" + dateTime.Month).Length - 2) + "/" + ("0" + dateTime.Day).Substring(("0" + dateTime.Day).Length - 2);
        }
        catch
        {
            return "";
        }
    }

    private void ClearAll()
    {
        drpstudent.SelectedIndex = drpSession.SelectedIndex = drpStatus.SelectedIndex = drpclass.SelectedIndex = -1;
        txtReason.Text = "";
        txtWithdrawalDate.Text = "";
        txtStudId.Text = string.Empty;
        lblDues.Text = string.Empty;
        hfTxtAdmno.Text = string.Empty;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Administrations/Home.aspx");
    }

    protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpStatus.SelectedValue.ToString().Trim() == "2" || drpStatus.SelectedValue.ToString().Trim() == "3" || drpStatus.SelectedValue.ToString().Trim() == "4")
        {
            clsDAL clsDal = new clsDAL();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@AdmnNo", drpstudent.SelectedValue.ToString().Trim());
            if (txtWithdrawalDate.Text.Trim() != "")
                hashtable.Add("@RecvDate", PopCalendar2.GetDateValue().ToString("MM/dd/yyyy"));
            else
                hashtable.Add("@RecvDate", DateTime.Today.ToString("MM/dd/yyyy"));
            DataSet dataSet = clsDal.GetDataSet("Ps_Sp_GetDueFee", hashtable);
            double num = double.Parse(dataSet.Tables[0].Rows[0][0].ToString()) + double.Parse(dataSet.Tables[1].Rows[0][0].ToString());
            lblDues.Text = "Total Dues: Rs." + num;
            if (Convert.ToDouble(num) > 0.0)
                btnFeeReceive.Enabled = true;
            else
                btnFeeReceive.Enabled = false;
        }
        else
            lblDues.Text = string.Empty;
    }
   void fillstatus() //by susovit on-23/mar/21
    {
        drpStatus.Items.Clear();
        drpStatus.DataSource = new clsDAL().GetDataTable("ps_sp_get_Status");
        drpStatus.DataTextField = "Status";
        drpStatus.DataValueField = "StatusID";
        drpStatus.DataBind();
        drpStatus.Items.Insert(0, new ListItem("All", "0"));
    }
    protected void btnFeeReceive_Click(object sender, EventArgs e)
    {
        Response.Redirect("../FeeManagement/feercptcash.aspx?admnno=" + hfTxtAdmno.Text.ToString() + "&cid=" + drpclass.SelectedValue.ToString() + "&sess=" + drpSession.SelectedValue.ToString() + "&Sw=t");
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("StudentTc.aspx?AdmnNo=" + ViewState["OldAdmnNo"].ToString());
    }
    

}