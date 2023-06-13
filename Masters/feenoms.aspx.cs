using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_feenoms : System.Web.UI.Page
{
    private string Dt1 = "";
    private string Dt2 = "";
    private string Dt3 = "";
    private string Dt4 = "";
    private string Dt5 = "";
    private string Dt6 = "";
    private string Dt7 = "";
    private string Dt8 = "";
    private string Dt9 = "";
    private string Dt10 = "";
    private string Dt11 = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
        {
            txtsession.Text = new clsGenerateFee().CreateCurrSession();
            FillDropDown();
            if (Request.QueryString["sid"] != null)
                showdata();
            CheckPermissions();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void CheckPermissions()
    {
        try
        {
            string absoluteUri = Request.Url.AbsoluteUri;
            string str = absoluteUri.Substring(absoluteUri.LastIndexOf("/") + 1).Split('?')[0];
            if (Session["permission"] == null)
                return;
            foreach (DataRow row in (InternalDataCollectionBase)(Session["permission"] as DataTable).Rows)
            {
                string lower = row["PageID"].ToString().ToLower();
                bool boolean = Convert.ToBoolean(row["status"]);
                if (lower == str.ToLower() && !boolean)
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('You are not permitted. Contact your administrator'); window.location='" + "../Administrations/Home.aspx" + "';", true);
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void FillDropDown()
    {
        drpFeeCollPeriod.Items.Clear();
        drpFeeCollPeriod.Items.Add("Monthly");
        drpFeeCollPeriod.Items.Add("Quarterly");
        DispayHideRow();
    }

    private void showdata()
    {
        DataTable dataTable = new Common().GetDataTable("sp_display_FeeNormsNewEdit", new Hashtable()
    {
      {
         "SID",
         Request.QueryString["sid"]
      }
    });
        drpFeeCollPeriod.Text = dataTable.Rows[0]["FeeCollPeriod"].ToString();
        txtsession.Text = dataTable.Rows[0]["SessionYr"].ToString();
        PopCalendarSt.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["StartDate"].ToString()));
        Popcalendar1.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["DueDate1"].ToString()));
        Popcalendar2.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["DueDate2"].ToString()));
        Popcalendar3.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["DueDate3"].ToString()));
        Popcalendar4.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["DueDate4"].ToString()));
        Popcalendar5.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["DueDate5"].ToString()));
        Popcalendar6.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["DueDate6"].ToString()));
        Popcalendar7.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["DueDate7"].ToString()));
        Popcalendar8.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["DueDate8"].ToString()));
        Popcalendar9.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["DueDate9"].ToString()));
        Popcalendar10.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["DueDate10"].ToString()));
        Popcalendar11.SetDateValue(Convert.ToDateTime(dataTable.Rows[0]["DueDate11"].ToString()));
        txtfineamt.Text = dataTable.Rows[0]["FineAmount"].ToString();
        drpFinal.Text = dataTable.Rows[0]["Finalised"].ToString();
        txtFineDays.Text = dataTable.Rows[0]["FineDateDuration"].ToString();
        DispayHideRow();
    }

    protected void txtdate_TextChanged(object sender, EventArgs e)
    {
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        if (lblDay.Text == "01" && lblMonth.Text == "04")
        {
            string scalar = new ClsAdmin().GetScalar("select top 1 SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
            if (scalar.Trim() != "")
            {
                if (Request.QueryString["sid"] == null && Convert.ToInt32(txtsession.Text.Trim().Substring(0, 4)) - 1 == Convert.ToInt32(scalar.Trim().Substring(0, 4)))
                {
                    save();
                }
                else
                {
                    lblMsg.Text = "The defined session year should be in order";
                    lblMsg.ForeColor = Color.Red;
                }
            }
            else
                save();
        }
        else
        {
            lblMsg.Text = "The Session year should start from 1st of April";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void save()
    {
        GetDates();
        string str1 = (Convert.ToInt32(txtsession.Text.Trim().Substring(0, 4)) - 1).ToString() + "-" + txtsession.Text.Trim().Substring(2, 2);
        string str2 = ValidateDates();
        if (str2 != "")
        {
            ScriptManager.RegisterClientScriptBlock((Control)txtdate, txtdate.GetType(), "ShowMessage", "alert('" + str2 + "');", true);
        }
        else
        {
            int int32;
            try
            {
                int32 = Convert.ToInt32(txtFineDays.Text.Trim());
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtdate, txtdate.GetType(), "ShowMessage", "alert('Invalid fine due days');", true);
                return;
            }
            double num;
            try
            {
                num = Convert.ToDouble(txtfineamt.Text);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtfineamt, txtfineamt.GetType(), "ShowMessage", "alert('Invalid  Fine Amount');", true);
                return;
            }
            if (Request.QueryString["sid"] != null)
            {
                if (new Common().GetDataTable("ps_sp_Updt_FeeNormsNew", new Hashtable()
        {
          {
             "SessionID",
             Request.QueryString["sid"].ToString()
          },
          {
             "StartDate",
             PopCalendarSt.GetDateValue().ToString("MM/dd/yyyy")
          },
          {
             "DueDate1",
             Dt1
          },
          {
             "DueDate2",
             Dt2
          },
          {
             "DueDate3",
             Dt3
          },
          {
             "DueDate4",
             Dt4
          },
          {
             "DueDate5",
             Dt5
          },
          {
             "DueDate6",
             Dt6
          },
          {
             "DueDate7",
             Dt7
          },
          {
             "DueDate8",
             Dt8
          },
          {
             "DueDate9",
             Dt9
          },
          {
             "DueDate10",
             Dt10
          },
          {
             "DueDate11",
             Dt11
          },
          {
             "Amount",
             num
          },
          {
             "Period",
             drpFeeCollPeriod.Text
          },
          {
             "final",
             drpFinal.SelectedValue.ToString()
          },
          {
             "fdd",
             int32
          },
          {
             "PrevSessYr",
             str1
          }
        }).Rows.Count > 0)
                {
                    ScriptManager.RegisterClientScriptBlock((Control)btnSave, btnSave.GetType(), "ShowMessage", "alert('Record finalized Can not be modified');", true);
                }
                else
                {
                    UpdateFineDates();
                    ScriptManager.RegisterClientScriptBlock((Control)txtsession, txtsession.GetType(), "ShowMessage", "alert('Record Modified');", true);
                    Response.Redirect("FeeNormsList.aspx");
                }
            }
            else if (new Common().GetDataTable("ps_sp_insert_FeeNormsNew", new Hashtable()
      {
        {
           "SessionYr",
           txtsession.Text.Trim()
        },
        {
           "StartDate",
           PopCalendarSt.GetDateValue().ToString("MM/dd/yyyy")
        },
        {
           "DueDate1",
           Dt1
        },
        {
           "DueDate2",
           Dt2
        },
        {
           "DueDate3",
           Dt3
        },
        {
           "DueDate4",
           Dt4
        },
        {
           "DueDate5",
           Dt5
        },
        {
           "DueDate6",
           Dt6
        },
        {
           "DueDate7",
           Dt7
        },
        {
           "DueDate8",
           Dt8
        },
        {
           "DueDate9",
           Dt9
        },
        {
           "DueDate10",
           Dt10
        },
        {
           "DueDate11",
           Dt11
        },
        {
           "FineAmt",
           num
        },
        {
           "UserID",
           Session["User_Id"].ToString()
        },
        {
           "UserDate",
           DateTime.Now.ToString("MM/dd/yyyy")
        },
        {
           "SchoolID",
           Session["SchoolId"].ToString()
        },
        {
           "Period",
           drpFeeCollPeriod.Text
        },
        {
           "final",
           drpFinal.SelectedValue.ToString()
        },
        {
           "fdd",
           int32
        },
        {
           "PrevSessYr",
           str1
        }
      }).Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtsession, txtsession.GetType(), "ShowMessage", "alert('Record Already Exists');", true);
            }
            else
            {
                UpdateFineDates();
                ScriptManager.RegisterClientScriptBlock((Control)txtsession, txtsession.GetType(), "ShowMessage", "alert('Record Inserted');", true);
                Response.Redirect("FeeNormsList.aspx");
            }
        }
    }

    protected void UpdateFineDates()
    {
        string str1 = txtsession.Text.Trim();
        string str2 = "04/01/" + str1.Substring(0, 4);
        str1.Substring(0, 2);
        str1.Substring(5, 2);
        string str3 = "03/31/" + str1.Substring(0, 2) + str1.Substring(5, 2);
        DataTable dataTable1 = new Common().ExecuteSql("select HolidayDate from dbo.PS_HolidayMaster where HolidayDate >= '" + str2 + "' and HolidayDate <= '" + str3 + "' order by HolidayDate");
        DataTable dataTable2 = new Common().ExecuteSql("select FineDate1,FineDate2,FineDate3,FineDate4,FineDate5,FineDate6,FineDate7,FineDate8,FineDate9,FineDate10,FineDate11 from dbo.PS_FeeNormsNew where sessionYr='" + txtsession.Text.Trim() + "'");
        for (int index1 = 0; index1 < dataTable1.Rows.Count; ++index1)
        {
            DateTime dateTime1 = Convert.ToDateTime(dataTable1.Rows[index1]["HolidayDate"].ToString());
            for (int index2 = 0; index2 < dataTable2.Columns.Count; ++index2)
            {
                DateTime dateTime2 = Convert.ToDateTime(dataTable2.Rows[0][index2].ToString());
                if (dateTime2 == dateTime1)
                {
                    new Common().ExcuteQryInsUpdt("update PS_FeeNormsNew set FineDate" + (index2 + 1) + " = '" + dateTime2.AddDays(1.0).ToString("MM/dd/yyyy") + "' where sessionYr='" + txtsession.Text.Trim() + "'");
                    dateTime2 = dateTime2.AddDays(1.0);
                }
                dateTime2.DayOfWeek.ToString();
                if (dateTime2.DayOfWeek.ToString() == "Sunday")
                    new Common().ExcuteQryInsUpdt("update PS_FeeNormsNew set FineDate" + (index2 + 1) + " = '" + dateTime2.AddDays(1.0).ToString("MM/dd/yyyy") + "' where sessionYr='" + txtsession.Text.Trim() + "'");
            }
        }
    }

    protected void GetDates()
    {
        switch (drpFeeCollPeriod.Text.ToString().Trim())
        {
            case "Monthly":
                Dt1 = Popcalendar1.GetDateValue().ToString("MM/dd/yyyy");
                Dt2 = Popcalendar2.GetDateValue().ToString("MM/dd/yyyy");
                Dt3 = Popcalendar3.GetDateValue().ToString("MM/dd/yyyy");
                Dt4 = Popcalendar4.GetDateValue().ToString("MM/dd/yyyy");
                Dt5 = Popcalendar5.GetDateValue().ToString("MM/dd/yyyy");
                Dt6 = Popcalendar6.GetDateValue().ToString("MM/dd/yyyy");
                Dt7 = Popcalendar7.GetDateValue().ToString("MM/dd/yyyy");
                Dt8 = Popcalendar8.GetDateValue().ToString("MM/dd/yyyy");
                Dt9 = Popcalendar9.GetDateValue().ToString("MM/dd/yyyy");
                Dt10 = Popcalendar10.GetDateValue().ToString("MM/dd/yyyy");
                Dt11 = Popcalendar11.GetDateValue().ToString("MM/dd/yyyy");
                break;
            case "Bi Monthly":
                Dt1 = Popcalendar1.GetDateValue().ToString("MM/dd/yyyy");
                Dt2 = Popcalendar2.GetDateValue().ToString("MM/dd/yyyy");
                Dt3 = Popcalendar3.GetDateValue().ToString("MM/dd/yyyy");
                Dt4 = Popcalendar4.GetDateValue().ToString("MM/dd/yyyy");
                Dt5 = Popcalendar5.GetDateValue().ToString("MM/dd/yyyy");
                Dt6 = "";
                Dt7 = "";
                Dt8 = "";
                Dt9 = "";
                Dt10 = "";
                Dt11 = "";
                break;
            case "Quarterly":
                Dt1 = Popcalendar1.GetDateValue().ToString("MM/dd/yyyy");
                Dt2 = Popcalendar2.GetDateValue().ToString("MM/dd/yyyy");
                Dt3 = Popcalendar3.GetDateValue().ToString("MM/dd/yyyy");
                Dt4 = "";
                Dt5 = "";
                Dt6 = "";
                Dt7 = "";
                Dt8 = "";
                Dt9 = "";
                Dt10 = "";
                Dt11 = "";
                break;
            case "Half Yearly":
                Dt1 = Popcalendar1.GetDateValue().ToString("MM/dd/yyyy");
                Dt2 = "";
                Dt3 = "";
                Dt4 = "";
                Dt5 = "";
                Dt6 = "";
                Dt7 = "";
                Dt8 = "";
                Dt9 = "";
                Dt10 = "";
                Dt11 = "";
                break;
            case "Yearly":
                Dt1 = "";
                Dt2 = "";
                Dt3 = "";
                Dt4 = "";
                Dt5 = "";
                Dt6 = "";
                Dt7 = "";
                Dt8 = "";
                Dt9 = "";
                Dt10 = "";
                Dt11 = "";
                break;
        }
    }

    protected string ValidateDates()
    {
        DateTime dateTime = Convert.ToDateTime(PopCalendarSt.GetDateValue());
        string str = "";
        switch (drpFeeCollPeriod.Text.ToString().Trim())
        {
            case "Monthly":
                if (dateTime.AddMonths(1).Month != Popcalendar1.GetDateValue().Month)
                {
                    str = "Invalid Due Date1";
                    break;
                }
                if (Popcalendar1.GetDateValue().AddMonths(1).Month != Popcalendar2.GetDateValue().Month)
                {
                    str = "Invalid Due Date2";
                    break;
                }
                if (Popcalendar2.GetDateValue().AddMonths(1).Month != Popcalendar3.GetDateValue().Month)
                {
                    str = "Invalid Due Date3";
                    break;
                }
                if (Popcalendar3.GetDateValue().AddMonths(1).Month != Popcalendar4.GetDateValue().Month)
                {
                    str = "Invalid Due Date4";
                    break;
                }
                if (Popcalendar4.GetDateValue().AddMonths(1).Month != Popcalendar5.GetDateValue().Month)
                {
                    str = "Invalid Due Date5";
                    break;
                }
                if (Popcalendar5.GetDateValue().AddMonths(1).Month != Popcalendar6.GetDateValue().Month)
                {
                    str = "Invalid Due Date6";
                    break;
                }
                if (Popcalendar6.GetDateValue().AddMonths(1).Month != Popcalendar7.GetDateValue().Month)
                {
                    str = "Invalid Due Date7";
                    break;
                }
                if (Popcalendar7.GetDateValue().AddMonths(1).Month != Popcalendar8.GetDateValue().Month)
                {
                    str = "Invalid Due Date8";
                    break;
                }
                if (Popcalendar8.GetDateValue().AddMonths(1).Month != Popcalendar9.GetDateValue().Month)
                {
                    str = "Invalid Due Date9";
                    break;
                }
                if (Popcalendar9.GetDateValue().AddMonths(1).Month != Popcalendar10.GetDateValue().Month)
                {
                    str = "Invalid Due Date10";
                    break;
                }
                if (Popcalendar10.GetDateValue().AddMonths(1).Month != Popcalendar11.GetDateValue().Month)
                {
                    str = "Invalid Due Date11";
                    break;
                }
                break;
            case "Bi Monthly":
                if (dateTime.AddMonths(2).Month != Popcalendar1.GetDateValue().Month)
                {
                    str = "Invalid Due Date1";
                    break;
                }
                if (Popcalendar1.GetDateValue().AddMonths(2).Month != Popcalendar2.GetDateValue().Month)
                {
                    str = "Invalid Due Date2";
                    break;
                }
                if (Popcalendar2.GetDateValue().AddMonths(2).Month != Popcalendar3.GetDateValue().Month)
                {
                    str = "Invalid Due Date3";
                    break;
                }
                if (Popcalendar3.GetDateValue().AddMonths(2).Month != Popcalendar4.GetDateValue().Month)
                {
                    str = "Invalid Due Date4";
                    break;
                }
                if (Popcalendar4.GetDateValue().AddMonths(2).Month != Popcalendar5.GetDateValue().Month)
                {
                    str = "Invalid Due Date5";
                    break;
                }
                break;
            case "Quarterly":
                if (dateTime.AddMonths(3).Month != Popcalendar1.GetDateValue().Month)
                {
                    str = "Invalid Due Date1";
                    break;
                }
                if (Popcalendar1.GetDateValue().AddMonths(3).Month != Popcalendar2.GetDateValue().Month)
                {
                    str = "Invalid Due Date2";
                    break;
                }
                if (Popcalendar2.GetDateValue().AddMonths(3).Month != Popcalendar3.GetDateValue().Month)
                {
                    str = "Invalid Due Date3";
                    break;
                }
                break;
            case "Half Yearly":
                if (dateTime.Month + 6 != Popcalendar1.GetDateValue().Month)
                {
                    str = "Invalid Due Date1";
                    break;
                }
                break;
        }
        return str;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("FeeNormsList.aspx");
    }

    protected void DispayHideRow()
    {
        switch (drpFeeCollPeriod.Text.ToString().Trim())
        {
            case "Monthly":
                RowDt1.Visible = true;
                RowDt2.Visible = true;
                RowDt3.Visible = true;
                RowDt4.Visible = true;
                RowDt5.Visible = true;
                RowDt6.Visible = true;
                RowDt7.Visible = true;
                RowDt8.Visible = true;
                RowDt9.Visible = true;
                RowDt10.Visible = true;
                RowDt11.Visible = true;
                break;
            case "Bi Monthly":
                RowDt1.Visible = true;
                RowDt2.Visible = true;
                RowDt3.Visible = true;
                RowDt4.Visible = true;
                RowDt5.Visible = true;
                RowDt6.Visible = false;
                RowDt7.Visible = false;
                RowDt8.Visible = false;
                RowDt9.Visible = false;
                RowDt10.Visible = false;
                RowDt11.Visible = false;
                break;
            case "Quarterly":
                RowDt1.Visible = true;
                RowDt2.Visible = true;
                RowDt3.Visible = true;
                RowDt4.Visible = false;
                RowDt5.Visible = false;
                RowDt6.Visible = false;
                RowDt7.Visible = false;
                RowDt8.Visible = false;
                RowDt9.Visible = false;
                RowDt10.Visible = false;
                RowDt11.Visible = false;
                break;
            case "Half Yearly":
                RowDt1.Visible = true;
                RowDt2.Visible = false;
                RowDt3.Visible = false;
                RowDt4.Visible = false;
                RowDt5.Visible = false;
                RowDt6.Visible = false;
                RowDt7.Visible = false;
                RowDt8.Visible = false;
                RowDt9.Visible = false;
                RowDt10.Visible = false;
                RowDt11.Visible = false;
                break;
            case "Yearly":
                RowDt1.Visible = false;
                RowDt2.Visible = false;
                RowDt3.Visible = false;
                RowDt4.Visible = false;
                RowDt5.Visible = false;
                RowDt6.Visible = false;
                RowDt7.Visible = false;
                RowDt8.Visible = false;
                RowDt9.Visible = false;
                RowDt10.Visible = false;
                RowDt11.Visible = false;
                break;
            default:
                RowDt1.Visible = true;
                RowDt2.Visible = true;
                RowDt3.Visible = true;
                RowDt4.Visible = true;
                RowDt5.Visible = true;
                RowDt6.Visible = true;
                RowDt7.Visible = true;
                RowDt8.Visible = true;
                RowDt9.Visible = true;
                RowDt10.Visible = true;
                RowDt11.Visible = true;
                break;
        }
    }

    protected void drpFeeCollPeriod_SelectedIndexChanged(object sender, EventArgs e)
    {
        DispayHideRow();
    }

    protected void PopCalendarSt_SelectionChanged(object sender, EventArgs e)
    {
        if (txtdate.Text.Trim() != "")
        {
            string[] strArray = txtdate.Text.Trim().Split('-');
            lblDay.Text = strArray[0];
            lblMonth.Text = strArray[1];
            string str1 = strArray[2];
            string str2 = ((int)Convert.ToInt16(str1.Substring(Math.Max(0, str1.Length - 2))) + 1).ToString();
            txtsession.Text = str1 + "-" + str2;
        }
        if (drpFeeCollPeriod.SelectedItem.ToString() == "Monthly")
        {
            DateTime dateTime = new DateTime();
            DateTime dateValue = PopCalendarSt.GetDateValue();
            txtduedate1.Text = dateValue.AddMonths(1).ToString("dd-MM-yyyy");
            txtduedate2.Text = dateValue.AddMonths(2).ToString("dd-MM-yyyy");
            txtduedate3.Text = dateValue.AddMonths(3).ToString("dd-MM-yyyy");
            txtduedate4.Text = dateValue.AddMonths(4).ToString("dd-MM-yyyy");
            txtduedate5.Text = dateValue.AddMonths(5).ToString("dd-MM-yyyy");
            txtduedate6.Text = dateValue.AddMonths(6).ToString("dd-MM-yyyy");
            txtduedate7.Text = dateValue.AddMonths(7).ToString("dd-MM-yyyy");
            txtduedate8.Text = dateValue.AddMonths(8).ToString("dd-MM-yyyy");
            txtduedate9.Text = dateValue.AddMonths(9).ToString("dd-MM-yyyy");
            txtduedate10.Text = dateValue.AddMonths(10).ToString("dd-MM-yyyy");
            txtduedate11.Text = dateValue.AddMonths(11).ToString("dd-MM-yyyy");
        }
        else if (drpFeeCollPeriod.SelectedItem.ToString() == "Bi Monthly")
        {
            DateTime dateTime = new DateTime();
            DateTime dateValue = PopCalendarSt.GetDateValue();
            txtduedate1.Text = dateValue.AddMonths(2).ToString("dd-MM-yyyy");
            txtduedate2.Text = dateValue.AddMonths(4).ToString("dd-MM-yyyy");
            txtduedate3.Text = dateValue.AddMonths(6).ToString("dd-MM-yyyy");
            txtduedate4.Text = dateValue.AddMonths(8).ToString("dd-MM-yyyy");
            txtduedate5.Text = dateValue.AddMonths(10).ToString("dd-MM-yyyy");
        }
        else if (drpFeeCollPeriod.SelectedItem.ToString() == "Quarterly")
        {
            DateTime dateTime = new DateTime();
            DateTime dateValue = PopCalendarSt.GetDateValue();
            txtduedate1.Text = dateValue.AddMonths(3).ToString("dd-MM-yyyy");
            txtduedate2.Text = dateValue.AddMonths(6).ToString("dd-MM-yyyy");
            txtduedate3.Text = dateValue.AddMonths(9).ToString("dd-MM-yyyy");
        }
        else if (drpFeeCollPeriod.SelectedItem.ToString() == "Half Yearly")
        {
            DateTime dateTime = new DateTime();
            txtduedate1.Text = PopCalendarSt.GetDateValue().AddMonths(6).ToString("dd-MM-yyyy");
        }
        UpdatePanel1.Update();
    }
}