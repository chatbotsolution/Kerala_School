using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class HR_Holidaydetail : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Request.QueryString["hId"] == null)
            return;
        DataTable dataTable = obj.GetDataTable("HR_GetHolidayDetails", new Hashtable()
    {
      {
         "@HolidayID",
         Request.QueryString["hId"]
      }
    });
        txtHolidayName.Text = dataTable.Rows[0]["HolidayName"].ToString();
        txtHolidayTithi.Text = dataTable.Rows[0]["HolidayTithi"].ToString();
        dtpFrmDt.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["FromDt"].ToString()));
        dtpToDt.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["ToDt"].ToString()));
        obj = new clsDAL();
        if (obj.ExecuteScalarQry("select count(*) from dbo.HR_EmpAttendance where AttendDate>='" + dtpFrmDt.GetDateValue().ToString("dd MMM yyyy") + "' and AttendStatus<>'L'").Trim() != "0" || obj.ExecuteScalarQry("Select count(*) from dbo.HR_EmpExtraWorking where EW_Date>='" + dtpFrmDt.GetDateValue().ToString("dd MMM yyyy") + "'").Trim() != "0")
        {
            txtFrmDt.Enabled = false;
            dtpFrmDt.Enabled = false;
        }
        else
        {
            txtFrmDt.Enabled = true;
            dtpFrmDt.Enabled = true;
        }
    }

    private void ResetFields()
    {
        txtHolidayName.Text = string.Empty;
        txtHolidayTithi.Text = string.Empty;
        txtFrmDt.Text = string.Empty;
        txtToDt.Text = string.Empty;
        trMsg.Style["Background-Color"] = (string)null;
        lblMsg.Text = string.Empty;
    }

    public DateTime checkDate(string dt2)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US")
        {
            DateTimeFormat = new DateTimeFormatInfo()
            {
                ShortDatePattern = "dd-MM-yyyy"
            }
        };
        return DateTime.Parse(dt2);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        trMsg.Style["Background-Color"] = (string)null;
        lblMsg.Text = string.Empty;
        if (obj.ExecuteScalarQry("Select count(*) from dbo.HR_EmpAttendance where AttendDate>='" + dtpFrmDt.GetDateValue().ToString("dd MMM yyyy") + "' and AttendDate<='" + dtpFrmDt.GetDateValue().ToString("dd MMM yyyy") + "' and AttendStatus<>'L'").Trim() != "0")
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsg.Text = "Holiday cannot be Declared for Previous Date";
        }
        else
        {
            Hashtable hashtable = new Hashtable();
            if (Request.QueryString["hId"] != null)
                hashtable.Add("@HolidayID", Request.QueryString["hId"]);
            hashtable.Add("@HolidayName", txtHolidayName.Text);
            hashtable.Add("@HolidayTithi", txtHolidayTithi.Text);
            hashtable.Add("@FrmDate", Convert.ToDateTime(dtpFrmDt.GetDateValue().ToString("dd MMM yyyy")));
            hashtable.Add("@ToDate", Convert.ToDateTime(dtpToDt.GetDateValue().ToString("dd MMM yyyy")));
            hashtable.Add("@UserID", Convert.ToInt32(Session["User_Id"]));
            string str = obj.ExecuteScalar("HR_InsUpdtHolidayMaster", hashtable);
            if (str.Trim() == string.Empty)
            {
                if (Request.QueryString["hId"] != null)
                {
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Data Saved Successfully.');window.location='Holiday.aspx'", true);
                }
                else
                {
                    ResetFields();
                    trMsg.Style["Background-Color"] = "Green";
                    lblMsg.Text = "Data Saved Successfully";
                }
            }
            else if (str.Trim().ToUpper() == "SALARY")
            {
                trMsg.Style["Background-Color"] = "Red";
                lblMsg.Text = "Salary Already Generated for this month";
            }
            else if (str.Trim().ToUpper() == "DUP")
            {
                trMsg.Style["Background-Color"] = "Red";
                lblMsg.Text = "Holiday Altready Exists On This Date";
            }
            else if (str.Trim().ToUpper() == "A")
            {
                trMsg.Style["Background-Color"] = "Red";
                lblMsg.Text = "Cannot Update This Holiday as End date conflicts with marked attendance!!";
            }
            else
            {
                trMsg.Style["Background-Color"] = "Red";
                lblMsg.Text = str.ToString().Trim();
            }
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ResetFields();
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("Holiday.aspx");
    }
}