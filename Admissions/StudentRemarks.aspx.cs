using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admissions_StudentRemarks : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        txtsession.Text = new clsGenerateFee().CreateCurrSession();
        btnDelete.Visible = false;
        ClearAll();
        FillClassDropDown();
        FillSectionDropDown();
        fillstudent();
    }

    private void fillstudent()
    {
        drpSelectStudent.Items.Clear();
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (txtsession.Text != "")
            hashtable.Add("@Session", txtsession.Text);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        DataTable dataTable = common.GetDataTable("ps_sp_get_StudNamesForClassSec", hashtable);
        drpSelectStudent.DataSource = dataTable;
        drpSelectStudent.DataTextField = "fullname";
        drpSelectStudent.DataValueField = "admnno";
        drpSelectStudent.DataBind();
        if (dataTable.Rows.Count > 0)
            drpSelectStudent.Items.Insert(0, new ListItem("ALL", "0"));
        else
            drpSelectStudent.Items.Insert(0, new ListItem("No Record", "0"));
        btnDelete.Enabled = true;
    }

    private void FillClassDropDown()
    {
        drpClass.DataSource = new Common().GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassID";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("ALL", "0"));
    }

    protected void FillSectionDropDown()
    {
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_Section", new Hashtable()
    {
      {
         "class",
         drpClass.SelectedValue
      },
      {
         "session",
         txtsession.Text
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("ALL", "0"));
    }

    protected void btnSaveAddNew_Click(object sender, EventArgs e)
    {
        if (drpSelectStudent.SelectedIndex != 0)
        {
            if (txtremarksdate.Text == "")
                ScriptManager.RegisterClientScriptBlock((Control)btnSaveAddNew, btnSaveAddNew.GetType(), "ShowMessage", "alert('Enter the date');", true);
            else if (txtteachers.Text == "")
                ScriptManager.RegisterClientScriptBlock((Control)btnSaveAddNew, btnSaveAddNew.GetType(), "ShowMessage", "alert('Enter teacher name');", true);
            else if (txtStudyremarks.Text == "" && txtSportremarks.Text == "" && (txtCulturalremarks.Text == "" && txtSpecialReference.Text == "") && txtAnnualResult.Text == "")
            {
                ScriptManager.RegisterClientScriptBlock((Control)btnSaveAddNew, btnSaveAddNew.GetType(), "ShowMessage", "alert('Enter atleast one type of Remark');", true);
            }
            else
            {
                Common common = new Common();
                Hashtable hashtable = new Hashtable();
                if (hdnsts.Value != "")
                    hashtable.Add("remarkid", hdnsts.Value);
                else
                    hashtable.Add("remarkid", 0);
                hashtable.Add("admnno", drpSelectStudent.SelectedValue.ToString().Trim());
                if (PopCalendar2.GetDateValue() <= DateTime.Today)
                {
                    hashtable.Add("remarksdate", PopCalendar2.GetDateValue().ToString("MM-dd-yyyy"));
                    hashtable.Add("teacher", txtteachers.Text);
                    hashtable.Add("studyremarks", txtStudyremarks.Text);
                    hashtable.Add("sportsremarks", txtSportremarks.Text);
                    hashtable.Add("culturalremarks", txtCulturalremarks.Text);
                    hashtable.Add("specialremarks", txtSpecialReference.Text);
                    hashtable.Add("annualremarks", txtAnnualResult.Text);
                    if (Session != null)
                        hashtable.Add("UserID", Convert.ToInt16(Session["User_Id"].ToString()));
                    hashtable.Add("schoolid", Session["SchoolId"].ToString());
                    DataTable dataTable = common.GetDataTable("ps_sp_insert_StudRemarks", hashtable);
                    string str = "";
                    if (dataTable.Rows.Count > 0)
                        str = dataTable.Rows[0][0].ToString();
                    if (str != "")
                    {
                        lblerr.Visible = true;
                        lblerr.Text = "Already Exists";
                    }
                    else
                    {
                        lblerr.Visible = false;
                        ClearAll();
                        drpSelectStudent.SelectedIndex = 0;
                        ScriptManager.RegisterClientScriptBlock((Control)btnSaveAddNew, btnSaveAddNew.GetType(), "ShowMessage", "alert(' Data Saved Successfully');", true);
                        btnDelete.Visible = false;
                        if (hdnsts.Value != "")
                            hdnsts.Value = "";
                        grdadmnno.Visible = false;
                    }
                }
                else
                    ScriptManager.RegisterClientScriptBlock((Control)btnSaveAddNew, btnSaveAddNew.GetType(), "ShowMessage", "alert('Future date is not permitted');", true);
            }
        }
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnSaveAddNew, btnSaveAddNew.GetType(), "ShowMessage", "alert('Select The Student Name');", true);
    }

    protected void ClearAll()
    {
        txtremarksdate.Text = "";
        txtteachers.Text = "";
        lblerr.Text = "";
        lblAdminno.Text = "";
        txtAnnualResult.Text = "";
        txtCulturalremarks.Text = "";
        txtSpecialReference.Text = "";
        txtSportremarks.Text = "";
        txtStudyremarks.Text = "";
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        Common common = new Common();
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select A Record');", true);
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                new Common().GetDataTable("ps_sp_get_StudRemarks", new Hashtable()
        {
          {
             "id",
             Convert.ToInt32(obj.ToString())
          },
          {
             "mode",
             'd'
          }
        });
            FillGrid();
            ClearAll();
            fillstudent();
        }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Administrations/Home.aspx");
    }

    protected void FillGrid()
    {
        grdadmnno.Visible = true;
        DataTable dataTable = new Common().GetDataTable("ps_sp_get_grdstudremarks", new Hashtable()
    {
      {
         "admno",
         drpSelectStudent.SelectedValue.ToString().Trim()
      }
    });
        if (dataTable.Rows.Count > 0)
        {
            grdadmnno.DataSource = dataTable;
            grdadmnno.DataBind();
            grdadmnno.Visible = true;
            btnDelete.Visible = true;
        }
        else
        {
            grdadmnno.Visible = false;
            btnDelete.Visible = false;
        }
    }

    protected void grdadmnno_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdadmnno.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void grdadmnno_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            lblerr.Text = "";
            DataTable dataTable = new Common().GetDataTable("ps_sp_get_StudentsRemarks", new Hashtable()
      {
        {
           "mode",
           's'
        },
        {
           "id",
           Convert.ToInt32(e.CommandArgument.ToString().Trim())
        }
      });
            hdnsts.Value = dataTable.Rows[0][0].ToString();
            txtremarksdate.Text = Convert.ToDateTime(dataTable.Rows[0]["RemarksDate"].ToString()).ToString("MM-dd-yyyy");
            txtteachers.Text = dataTable.Rows[0]["Teacher"].ToString();
            txtStudyremarks.Text = dataTable.Rows[0]["StudyRemarks"].ToString();
            txtSportremarks.Text = dataTable.Rows[0]["SportsRemarks"].ToString();
            txtCulturalremarks.Text = dataTable.Rows[0]["CulturalRemarks"].ToString();
            txtAnnualResult.Text = dataTable.Rows[0]["AnnualResult"].ToString();
            txtSpecialReference.Text = dataTable.Rows[0]["SpecialReference"].ToString();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
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

    protected void txtremarksdate_TextChanged(object sender, EventArgs e)
    {
        Convert.ToDateTime(checkDate(txtremarksdate.Text));
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSectionDropDown();
        fillstudent();
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    protected void drpSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSelectStudent.SelectedIndex > 0)
            FillGrid();
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnSaveAddNew, btnSaveAddNew.GetType(), "ShowMessage", "alert('Select The Student Namr');", true);
    }
}