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

public partial class Hostel_HostLeaving : System.Web.UI.Page
{
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        fillsession();
        fillclass();
        fillstudent();
        btnFeeReceive.Enabled = false;
        if (Request.QueryString["admnno"] == null)
            return;
        FillSelStudent();
        fillstudent();
        txtStudId.Text = drpstudent.SelectedValue;
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
            dataTableQry = clsDal.GetDataTableQry("select fullname,cs.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno inner join dbo.Host_Admission ha on ha.AdmnNo=cs.admnno where cs.classid=" + drpclass.SelectedValue + " and cs.SessionYear='" + drpSession.SelectedValue.ToString() + "' and StatusId=1 and ha.LeavingDt is null and Detained_Promoted='' order by fullname");
        else
            dataTableQry = clsDal.GetDataTableQry("select fullname,cs.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno inner join dbo.Host_Admission ha on ha.AdmnNo=cs.admnno where cs.SessionYear='" + drpSession.SelectedValue.ToString() + "' and StatusId=1 and ha.LeavingDt is null and Detained_Promoted='' order by fullname");
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
        lblHostelDues.Text = "";
        fillstudent();
        txtStudId.Text = string.Empty;
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblHostelDues.Text = "";
        fillstudent();
        txtStudId.Text = string.Empty;
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblHostelDues.Text = "";
        txtStudId.Text = drpstudent.SelectedValue;
        GetStudDetails();
        if (drpstudent.SelectedIndex > 0)
        {
            GetHostelDues();
        }
        else
        {
            lblHostelDues.Text = "";
            btnFeeReceive.Enabled = false;
        }
    }

    private void GetHostelDues()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@AdmnNo", drpstudent.SelectedValue.ToString().Trim());
        if (txtWithdrawalDate.Text.Trim() != "")
            hashtable.Add("@RecvDate", PopCalendar2.GetDateValue().ToString("MM/dd/yyyy"));
        DataTable dataTable = new DataTable();
        double num = double.Parse(clsDal.GetDataTable("Host_GetDueFee", hashtable).Rows[0][0].ToString());
        lblHostelDues.Text = "Total Dues: Rs." + num;
        if (Convert.ToDouble(num) > 0.0)
            btnFeeReceive.Enabled = true;
        else
            btnFeeReceive.Enabled = false;
    }

    protected void GetStudDetails()
    {
        lblStudDet.Text = new clsDAL().ExecuteScalarQry("select FullName + '(DOB: ' + convert(varchar,DOB,106) + ')' + ', Father Name: ' + FatherName from dbo.PS_StudMaster where AdmnNo=" + drpstudent.SelectedValue);
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = clsDal.GetDataTableQry("select SessionYear,ClassID from dbo.PS_ClasswiseStudent c inner join  dbo.PS_StudMaster s on c.admnno=s.AdmnNo inner join dbo.Host_Admission ha on ha.AdmnNo=c.admnno where c.admnno=" + txtStudId.Text.ToString().Trim() + " and StatusId=1 and ha.LeavingDt is null and Detained_Promoted='' ");
        if (dataTableQry.Rows.Count > 0)
        {
            fillsession();
            drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
            fillclass();
            drpclass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
            fillstudent();
            drpstudent.SelectedValue = txtStudId.Text.Trim();
            GetStudDetails();
            if (drpstudent.SelectedIndex > 0)
            {
                GetHostelDues();
            }
            else
            {
                lblHostelDues.Text = "";
                btnFeeReceive.Enabled = false;
            }
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMassage", "alert('Invalid Admission Number !');", true);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
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
                string str = new clsDAL().ExecuteScalar("Host_UpdtStudHostelStatus", new Hashtable()
        {
          {
             "adminno",
             Convert.ToInt32(drpstudent.SelectedValue.ToString())
          },
          {
             "withdrawaldate",
             PopCalendar2.GetDateValue().ToString("MM/dd/yyyy")
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
                if (str.Trim() == "")
                {
                    ScriptManager.RegisterClientScriptBlock((Control)txtReason, txtReason.GetType(), "ShowMessage", "alert('Status updated successfully');", true);
                    ClearAll();
                }
                else if (str.Trim().ToUpper() == "PRIOR")
                    ScriptManager.RegisterClientScriptBlock((Control)txtReason, txtReason.GetType(), "ShowMessage", "alert('Fee Already Received On Later Date');", true);
                else
                    ScriptManager.RegisterClientScriptBlock((Control)txtReason, txtReason.GetType(), "ShowMessage", "alert('Unable To Update Student Hostel Status Please Try Again!!');", true);
            }
        }
        catch (Exception ex)
        {
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
        txtReason.Text = "";
        txtWithdrawalDate.Text = "";
        txtStudId.Text = string.Empty;
        lblHostelDues.Text = string.Empty;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Hostel/HostelHome.aspx");
    }

    protected void drpStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void btnFeeReceive_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Hostel/FeeReceiptHostel.aspx?admnno=" + txtStudId.Text.ToString() + "&cid=" + drpclass.SelectedValue.ToString() + "&sess=" + drpSession.SelectedValue.ToString() + "&Sw=t");
    }
}