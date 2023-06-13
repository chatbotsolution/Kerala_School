using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_Holidaydetail : System.Web.UI.Page
{
    private Common obj = new Common();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Request.QueryString["hno"] == null)
            return;
        if (obj.ExecuteSql("select * from PS_HolidayMaster where HolidayDate<getdate() and holidayid='" + Request.QueryString["hno"] + "'").Rows.Count > 0)
            Response.Redirect("Holiday.aspx?status=1");
        else
            showdata();
    }

    private void showdata()
    {
        DataTable dataTable = new Common().ExecuteSql("select HolidayID,HolidayName,convert(varchar,HolidayDate,103) as HolidayDate from dbo.PS_HolidayMaster where holidayid='" + Request.QueryString["hno"] + "'");
        txtholidaydesc.Text = Convert.ToString(dataTable.Rows[0]["HolidayName"]);
        txtdate.Text = Convert.ToString(dataTable.Rows[0]["HolidayDate"]);
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        if (checkDate(txtdate.Text) < checkDate(DateTime.Now.ToString("dd-MM-yyyy")))
        {
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Holiday cannot be declared for previous date');", true);
        }
        else
        {
            Common common1 = new Common();
            Hashtable ht = new Hashtable();
            ht.Add("holidaydesc ", txtholidaydesc.Text);
            ht.Add("holidaydate", PopCalendar2.GetDateValue().ToString("MM/dd/yyyy"));
            ht.Add("UserID", Convert.ToInt32(Session["User_Id"]));
            DataTable dataTable1 = new DataTable();
            if (Request.QueryString["hno"] != null)
            {
                ht.Add("holidayid", Convert.ToInt32(Request.QueryString["hno"]));
                common1.ExcuteProcInsUpdt("ps_sp_update_holidaymaster", ht);
            }
            else
            {
                if (common1.CheckDuplicateRecords("select * from PS_HolidayMaster where convert(varchar,HolidayDate,105)='" + txtdate.Text + "'").Rows.Count > 0)
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Infomation already exists');", true);
                    return;
                }
                common1.ExcuteProcInsUpdt("ps_sp_insert_Holiday", ht);
                Common common2 = new Common();
                clsGenerateFee clsGenerateFee = new clsGenerateFee();
                string qry = "select FineDate1,FineDate2,FineDate3,FineDate4,FineDate5,FineDate6,FineDate7,FineDate8,FineDate9,FineDate10,FineDate11 from dbo.PS_FeeNormsNew where sessionYr='" + clsGenerateFee.CreateCurrSession() + "'";
                DataTable dataTable2 = common2.ExecuteSql(qry);
                for (int index = 0; index < dataTable2.Columns.Count; ++index)
                {
                    Convert.ToDateTime(dataTable2.Rows[0][index].ToString());
                    PopCalendar2.GetDateValue();
                    if (Convert.ToDateTime(dataTable2.Rows[0][index].ToString()) == PopCalendar2.GetDateValue())
                        new Common().ExcuteQryInsUpdt("update PS_FeeNormsNew set FineDate" + (index + 1) + " = '" + PopCalendar2.GetDateValue().AddDays(1.0).ToString("MM/dd/yyyy") + "' where sessionYr='" + clsGenerateFee.CreateCurrSession() + "'");
                }
            }
            Response.Redirect("Holiday.aspx");
        }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Holiday.aspx");
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
}