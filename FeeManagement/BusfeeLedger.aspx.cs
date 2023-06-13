using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FeeManagement_BusfeeLedger : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        Page.Form.DefaultButton = btngo.UniqueID;
        ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
        fillclass();
        FillClassSection();
        fillsession();
        FillSelectStudent();
        chkFullSession.Checked = true;
        chkFullSession.Visible = false;
        btnSubmit.Visible = false;
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL").DefaultView;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
    }

    private void FillClassSection()
    {
        txtStudentName.Text = "";
        ddlSection.Items.Clear();
        ddlSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "@classId",
         drpclass.SelectedValue.ToString().Trim()
      },
      {
         "@session",
         drpSession.SelectedValue.ToString().ToString().Trim()
      }
    });
        ddlSection.DataTextField = "Section";
        ddlSection.DataValueField = "Section";
        ddlSection.DataBind();
        ddlSection.Items.Insert(0, new ListItem("ALL", "0"));
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        grdHostel.DataSource = null;
        grdHostel.DataBind();
        grdStudentList.DataSource = null;
        grdStudentList.DataBind();
        FillGrid();
        UpdatePanel1.Update();
    }

    protected void FillGrid()
    {
        if (Session["updatebusfee"] != null)
            Session.Remove("updatebusfee");
        string[] strArray = new string[1] { "ClasswiseId" };
        if (rBtnBus.Checked)
            grdStudentList.DataKeyNames = strArray;
        else
            grdHostel.DataKeyNames = strArray;
        FillBusFeesStudentDetails();
        Session["updatebusfee"] = "true";
    }

    private void FillBusFeesPreviousMonthDetails()
    {
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Session", drpSession.SelectedValue.ToString().Trim());
        hashtable.Add("@classId", drpclass.SelectedValue.ToString().Trim());
        if (ddlSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@section", ddlSection.SelectedValue.ToString().Trim());
        int num = Convert.ToInt32(ddlMonth.SelectedValue.ToString().Trim()) - 1;
        hashtable.Add("@Month", num == 0 ? "12" : num.ToString().Trim());
        if (rBtnBus.Checked)
            hashtable.Add("@FeeType", 1);
        else
            hashtable.Add("@FeeType", 2);
        if (rBtnBus.Checked)
        {
            DataTable dataTable = common.GetDataTable("ps_sp_get_AdFeeLedger", hashtable);
            if (dataTable.Rows.Count > 0)
            {
                grdStudentList.DataSource = dataTable.DefaultView;
                grdStudentList.DataBind();
                SetVisible(true);
                lblRecCount.Text = "Total Record: " + dataTable.Rows.Count.ToString();
            }
            else
            {
                lblMsg.Text = "No Record Found";
                if (rBtnBus.Checked)
                    SetVisible(false);
                else
                    SetVisibleHostel(false);
            }
        }
        else
        {
            DataTable dataTable = common.GetDataTable("ps_sp_get_AdFeeLedger", hashtable);
            if (dataTable.Rows.Count > 0)
            {
                grdHostel.DataSource = dataTable.DefaultView;
                grdHostel.DataBind();
                SetVisibleHostel(true);
                lblRecCount.Text = "Total Record: " + dataTable.Rows.Count.ToString();
            }
            else
            {
                lblMsg.Text = "No Record Found";
                if (rBtnBus.Checked)
                    SetVisible(false);
                else
                    SetVisibleHostel(false);
            }
        }
    }

    private void FillBusFeesStudentDetails()
    {
        int num = 0;
        string str = "";
        try
        {
            num = Convert.ToInt32(txtStudentName.Text);
        }
        catch (Exception ex)
        {
            str = txtStudentName.Text;
        }
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Session", drpSession.SelectedValue.ToString().Trim());
        hashtable.Add("@classId", drpclass.SelectedValue.ToString().Trim());
        if (ddlSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@section", ddlSection.SelectedValue.ToString().Trim());
        hashtable.Add("@Month", ddlMonth.SelectedValue.ToString().Trim());
        if (rBtnBus.Checked)
            hashtable.Add("@FeeType", 1);
        else
            hashtable.Add("@FeeType", 2);
        if (num != 0)
            hashtable.Add("@SerAdmnNo", num);
        else if (str != string.Empty)
            hashtable.Add("@SerAdmnNo", str);
        if (rBtnBus.Checked)
        {
            DataTable dataTable = common.GetDataTable("ps_sp_get_AdFeeLedger", hashtable);
            if (dataTable.Rows.Count > 0)
            {
                grdStudentList.DataSource = dataTable;
                grdStudentList.DataBind();
                lblRecCount.Text = "Total Record: " + dataTable.Rows.Count.ToString();
                SetVisible(true);
            }
            else
            {
                lblMsg.Text = "No Record Found";
                if (rBtnBus.Checked)
                    SetVisible(false);
                else
                    SetVisibleHostel(false);
            }
        }
        else
        {
            DataTable dataTable = common.GetDataTable("ps_sp_get_AdFeeLedger", hashtable);
            if (dataTable.Rows.Count > 0)
            {
                grdHostel.DataSource = dataTable;
                grdHostel.DataBind();
                lblRecCount.Text = "Total Record: " + dataTable.Rows.Count.ToString();
                SetVisibleHostel(true);
            }
            else
            {
                lblMsg.Text = "No Record Found";
                if (rBtnBus.Checked)
                    SetVisible(false);
                else
                    SetVisibleHostel(false);
            }
        }
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
        if (rBtnBus.Checked)
            SetVisible(false);
        else
            SetVisibleHostel(false);
        FillSelectStudent();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (chkFullSession.Checked.Equals(true))
        {
            if (rBtnBus.Checked)
                FullSessionBus();
            else
                FullSessionHostel();
        }
        else if (rBtnBus.Checked)
            CurrentMonthBus();
        else
            CurrentMonthHostel();
        grdStudentList.Visible = false;
        grdHostel.Visible = false;
        if (rBtnBus.Checked)
            SetVisible(false);
        else
            SetVisibleHostel(false);
        UpdatePanel1.Update();
    }

    private void FullSessionHostel()
    {
        DataTable dataTable = new DataTable();
        foreach (GridViewRow row in grdHostel.Rows)
        {
            int M = Convert.ToInt32(ddlMonth.SelectedValue.ToString().Trim());
            if (M >= 4)
            {
                for (int index = M; index <= 15; ++index)
                {
                    Common common = new Common();
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("@ClasswiseId", Convert.ToInt32(grdHostel.DataKeys[row.DataItemIndex].Value.ToString().Trim()));
                    hashtable.Add("@Debit", Convert.ToDecimal(((TextBox)row.Cells[3].FindControl("txtFeeAmountBus")).Text.ToString().Trim()));
                    hashtable.Add("@UserId", Session["User_Id"]);
                    hashtable.Add("@SchoolId", Session["SchoolId"].ToString());
                    hashtable.Add("@month", M);
                    hashtable.Add("@Ad_Id", 2);
                    hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
                    string adFeeDate = CreateAdFeeDate(M);
                    hashtable.Add("@AdFeeDate", adFeeDate);
                    dataTable = common.GetDataTable("ps_sp_update_AdFeeLedger", hashtable);
                    ++M;
                    if (M == 13)
                        M = 1;
                }
            }
            else
            {
                for (int index = M; index <= 3; ++index)
                {
                    Common common = new Common();
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("@ClasswiseId", Convert.ToInt32(grdHostel.DataKeys[row.DataItemIndex].Value.ToString().Trim()));
                    hashtable.Add("@Debit", Convert.ToDecimal(((TextBox)row.Cells[3].FindControl("txtFeeAmountBus")).Text.ToString().Trim()));
                    hashtable.Add("@UserId", Session["User_Id"]);
                    hashtable.Add("@SchoolId", Session["SchoolId"].ToString());
                    hashtable.Add("@month", index);
                    hashtable.Add("@Ad_Id", 2);
                    hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
                    string adFeeDate = CreateAdFeeDate(M);
                    hashtable.Add("@AdFeeDate", adFeeDate);
                    dataTable = common.GetDataTable("ps_sp_update_AdFeeLedger", hashtable);
                    ++M;
                }
            }
        }
        if (dataTable.Rows.Count > 0)
        {
            lblMsg.Text = "'" + dataTable.Rows[0][0].ToString() + "'";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Fees Defined Successfully";
            lblMsg.ForeColor = Color.Green;
        }
    }

    private void FullSessionBus()
    {
        DataTable dataTable = new DataTable();
        foreach (GridViewRow row in grdStudentList.Rows)
        {
            int M1 = Convert.ToInt32(ddlMonth.SelectedValue.ToString().Trim());
            if (M1 >= 4)
            {
                for (int index = M1; index <= 15; ++index)
                {
                    Common common = new Common();
                    Hashtable hashtable = new Hashtable();
                    grdStudentList.DataKeys[row.DataItemIndex].Value.ToString().Trim();
                    hashtable.Add("@ClasswiseId", Convert.ToInt32(grdStudentList.DataKeys[row.DataItemIndex].Value.ToString().Trim()));
                    hashtable.Add("@Debit", Convert.ToDecimal(((TextBox)row.Cells[3].FindControl("txtFeeAmountBus")).Text.ToString().Trim()));
                    hashtable.Add("@UserId", Session["User_Id"]);
                    hashtable.Add("@SchoolId", Session["SchoolId"].ToString());
                    hashtable.Add("@month", M1);
                    hashtable.Add("@Ad_Id", 1);
                    hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
                    string adFeeDate = CreateAdFeeDate(M1);
                    hashtable.Add("@AdFeeDate", adFeeDate);
                    hashtable.Add("@RouteId", Convert.ToInt32(((Label)row.Cells[3].FindControl("lblRouteID")).Text.ToString().Trim()));
                    dataTable = common.GetDataTable("ps_sp_update_AdFeeLedger", hashtable);
                    ++M1;
                    if (M1 == 13)
                        M1 = 1;
                }
            }
            else
            {
                for (int M2 = M1; M2 <= 3; ++M2)
                {
                    Common common = new Common();
                    Hashtable hashtable = new Hashtable();
                    grdStudentList.DataKeys[row.DataItemIndex].Value.ToString().Trim();
                    hashtable.Add("@ClasswiseId", Convert.ToInt32(grdStudentList.DataKeys[row.DataItemIndex].Value.ToString().Trim()));
                    hashtable.Add("@Debit", Convert.ToDecimal(((TextBox)row.Cells[3].FindControl("txtFeeAmountBus")).Text.ToString().Trim()));
                    hashtable.Add("@UserId", Session["User_Id"]);
                    hashtable.Add("@SchoolId", Session["SchoolId"].ToString());
                    hashtable.Add("@month", M2);
                    hashtable.Add("@Ad_Id", 1);
                    hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
                    string adFeeDate = CreateAdFeeDate(M2);
                    hashtable.Add("@AdFeeDate", adFeeDate);
                    hashtable.Add("@RouteId", Convert.ToInt32(((Label)row.Cells[3].FindControl("lblRouteID")).Text.ToString().Trim()));
                    dataTable = common.GetDataTable("ps_sp_update_AdFeeLedger", hashtable);
                    ++M1;
                }
            }
        }
        if (dataTable.Rows.Count > 0)
        {
            lblMsg.Text = "'" + dataTable.Rows[0][0].ToString() + "'";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Fees Defined Successfully";
            lblMsg.ForeColor = Color.Green;
        }
    }

    private void CurrentMonthBus()
    {
        DataTable dataTable = new DataTable();
        foreach (GridViewRow row in grdStudentList.Rows)
        {
            Common common = new Common();
            Hashtable hashtable = new Hashtable();
            grdStudentList.DataKeys[row.DataItemIndex].Value.ToString().Trim();
            hashtable.Add("@ClasswiseId", Convert.ToInt32(grdStudentList.DataKeys[row.DataItemIndex].Value.ToString().Trim()));
            hashtable.Add("@Debit", Convert.ToDecimal(((TextBox)row.Cells[3].FindControl("txtFeeAmountBus")).Text.ToString().Trim()));
            hashtable.Add("@UserId", Session["User_Id"]);
            hashtable.Add("@SchoolId", Session["SchoolId"].ToString());
            hashtable.Add("@month", ddlMonth.SelectedValue.ToString().Trim());
            hashtable.Add("@Ad_Id", 1);
            hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
            string adFeeDate = CreateAdFeeDate((int)Convert.ToInt16(ddlMonth.SelectedValue.ToString().Trim()));
            hashtable.Add("@AdFeeDate", adFeeDate);
            hashtable.Add("@RouteId", Convert.ToInt32(((Label)row.Cells[3].FindControl("lblRouteID")).Text.ToString().Trim()));
            dataTable = common.GetDataTable("ps_sp_update_AdFeeLedger", hashtable);
        }
        if (dataTable.Rows.Count > 0)
        {
            lblMsg.Text = "'" + dataTable.Rows[0][0].ToString() + "'";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Fees Defined Successfully";
            lblMsg.ForeColor = Color.Green;
        }
    }

    private void CurrentMonthHostel()
    {
        DataTable dataTable = new DataTable();
        foreach (GridViewRow row in grdHostel.Rows)
        {
            Common common = new Common();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@ClasswiseId", Convert.ToInt32(grdHostel.DataKeys[row.DataItemIndex].Value.ToString().Trim()));
            hashtable.Add("@Debit", Convert.ToDecimal(((TextBox)row.Cells[3].FindControl("txtFeeAmountBus")).Text.ToString().Trim()));
            hashtable.Add("@UserId", Session["User_Id"]);
            hashtable.Add("@SchoolId", Session["SchoolId"].ToString());
            hashtable.Add("@month", ddlMonth.SelectedValue.ToString().Trim());
            HiddenField control = (HiddenField)row.FindControl("hfHostelFeeId");
            hashtable.Add("@Ad_Id", 2);
            hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
            string adFeeDate = CreateAdFeeDate((int)Convert.ToInt16(ddlMonth.SelectedValue.ToString().Trim()));
            hashtable.Add("@AdFeeDate", adFeeDate);
            dataTable = common.GetDataTable("ps_sp_update_AdFeeLedger", hashtable);
        }
        if (dataTable.Rows.Count > 0)
        {
            lblMsg.Text = "'" + dataTable.Rows[0][0].ToString() + "'";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Fees Defined Successfully";
            lblMsg.ForeColor = Color.Green;
        }
    }

    private void CurrentMonth()
    {
        DataTable dataTable = new DataTable();
        foreach (GridViewRow row in grdStudentList.Rows)
        {
            Common common1 = new Common();
            Hashtable hashtable = new Hashtable();
            grdStudentList.DataKeys[row.DataItemIndex].Value.ToString().Trim();
            hashtable.Add("@ClasswiseId", Convert.ToInt32(grdStudentList.DataKeys[row.DataItemIndex].Value.ToString().Trim()));
            hashtable.Add("@Debit", Convert.ToDecimal(((TextBox)row.Cells[3].FindControl("txtFeeAmountBus")).Text.ToString().Trim()));
            hashtable.Add("@Balance", Convert.ToDecimal(((TextBox)row.Cells[3].FindControl("txtFeeAmountBus")).Text.ToString().Trim()));
            hashtable.Add("@UserId", Session["User_Id"]);
            hashtable.Add("@SchoolId", Session["SchoolId"].ToString());
            hashtable.Add("@month", ddlMonth.SelectedValue.ToString().Trim());
            hashtable.Add("@Ad_Id", 1);
            hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
            string adFeeDate = CreateAdFeeDate((int)Convert.ToInt16(ddlMonth.SelectedValue.ToString().Trim()));
            hashtable.Add("@AdFeeDate", adFeeDate);
            dataTable = common1.GetDataTable("ps_sp_update_AdFeeLedger", hashtable);
            hashtable.Remove("@Ad_Id");
            hashtable.Remove("@Debit");
            hashtable.Remove("@Balance");
            Common common2 = new Common();
            HiddenField control = (HiddenField)row.FindControl("hfHostelFeeId");
            hashtable.Add("@Ad_Id", 2);
            hashtable.Add("@Debit", Convert.ToDecimal(((TextBox)row.Cells[4].FindControl("txtFeeAmountHostel")).Text.ToString().Trim()));
            hashtable.Add("@Balance", Convert.ToDecimal(((TextBox)row.Cells[4].FindControl("txtFeeAmountHostel")).Text.ToString().Trim()));
            dataTable = common2.GetDataTable("ps_sp_update_AdFeeLedger", hashtable);
        }
        if (dataTable.Rows.Count > 0)
        {
            lblMsg.Text = "'" + dataTable.Rows[0][0].ToString() + "'";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Fees Defined Successfully";
            lblMsg.ForeColor = Color.Green;
        }
    }

    public string CreateAdFeeDate(int M)
    {
        string str1 = drpSession.SelectedValue.ToString().Substring(0, 4);
        string str2 = drpSession.SelectedValue.ToString().Substring(5, 2);
        int int16 = (int)Convert.ToInt16(M);
        string str3 = "";
        if (int16 > 0 && int16 < 4)
            str3 = int16.ToString().Trim() + "/01/" + str1.Substring(0, 2) + str2;
        else if (int16 > 3 && int16 < 13)
            str3 = M.ToString() + "/01/" + str1;
        return str3;
    }

    protected void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rBtnBus.Checked)
            SetVisible(false);
        else
            SetVisibleHostel(false);
        FillSelectStudent();
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rBtnBus.Checked)
            SetVisible(false);
        else
            SetVisibleHostel(false);
    }

    private void SetVisible(bool st)
    {
        grdStudentList.Visible = st;
        chkFullSession.Visible = st;
        btnSubmit.Visible = st;
        lblRecCount.Text = "";
    }

    private void SetVisibleHostel(bool st)
    {
        grdHostel.Visible = st;
        chkFullSession.Visible = st;
        btnSubmit.Visible = st;
        lblRecCount.Text = "";
    }

    protected void rBtnHostel_CheckedChanged(object sender, EventArgs e)
    {
    }

    protected void rBtnBus_CheckedChanged(object sender, EventArgs e)
    {
    }

    private void FillSelectStudent()
    {
        txtStudentName.Text = "";
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@Session", drpSession.SelectedValue);
        if (drpclass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpclass.SelectedValue);
        if (ddlSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", ddlSection.SelectedValue);
        drpSelectStudent.DataSource = common.GetDataTable("ps_sp_get_StudNamesForClassSec", hashtable);
        drpSelectStudent.DataTextField = "fullname";
        drpSelectStudent.DataValueField = "admnno";
        drpSelectStudent.DataBind();
        drpSelectStudent.Items.Insert(0, new ListItem("ALL", "0"));
    }

    protected void drpSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSelectStudent.SelectedIndex > 0)
            txtStudentName.Text = drpSelectStudent.SelectedValue;
        else
            txtStudentName.Text = string.Empty;
    }
}