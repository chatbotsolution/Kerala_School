using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FeeManagement_AdditionalCharges : System.Web.UI.Page
{
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            clsGenerateFee clsGenerateFee = new clsGenerateFee();
            fillclass();
            fillsession();
            drpSession.SelectedValue = clsGenerateFee.CreateCurrSession();
            FillClassSection();
            dtpFeeDt.SetDateValue(DateTime.Now);
            fillAditionalFee();
            setMiscDate();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private string GetCurrentSession()
    {
        string str = "";
        if (DateTime.Today.Month > 3 && DateTime.Today.Month <= 12)
            str = DateTime.Today.Year.ToString().Trim() + "-" + Convert.ToString(DateTime.Today.Year + 1).Substring(2, 2);
        if (DateTime.Today.Month >= 1 && DateTime.Today.Month <= 3)
            str = Convert.ToString(DateTime.Today.Year - 1) + "-" + DateTime.Today.Year.ToString().Substring(2, 2);
        return str.ToString().Trim();
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        if (drpclass.SelectedIndex > 0)
            FillClassSection();
        GrdClear();
        setMiscDate();
    }

    private void FillClassSection()
    {
        ddlSection.Items.Clear();
        ddlSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "@classId",
         drpclass.SelectedValue.ToString().Trim()
      },
      {
         "@session",
         drpSession.SelectedValue.ToString().Trim()
      }
    });
        ddlSection.DataTextField = "Section";
        ddlSection.DataValueField = "Section";
        ddlSection.DataBind();
        ddlSection.Items.Insert(0, new ListItem("ALL", "0"));
    }

    protected void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select top(2) SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    protected void fillAditionalFee()
    {
        drpAdditionalFee.DataSource = new Common().ExecuteSql("SELECT FC.FeeID,FC.FeeName,FC.PeriodicityID FROM dbo.PS_FeeComponents FC WHERE PeriodicityID=6 ORDER BY FeeName DESC");
        drpAdditionalFee.DataTextField = "FeeName";
        drpAdditionalFee.DataValueField = "FeeID";
        drpAdditionalFee.DataBind();
        drpAdditionalFee.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    private void FillStudentDetails()
    {
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SessionYear", drpSession.SelectedValue.ToString().Trim());
        hashtable.Add("@ClassID", drpclass.SelectedValue.ToString().Trim());
        if (ddlSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", ddlSection.SelectedValue.ToString().Trim());
        DataTable dataTable = common.GetDataTable("Ps_Sp_GetStudentAdditionalCharges", hashtable);
        if (dataTable.Rows.Count > 0)
        {
            grdStudentList.DataSource = dataTable.DefaultView;
            grdStudentList.DataBind();
            grdStudentList.Visible = true;
            btnSubmit.Visible = true;
        }
        else
        {
            grdStudentList.Visible = false;
            lblMsg.Text = "No record found !";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void setMiscDate()
    {
        string str = drpSession.SelectedValue.Trim();
        DateTime dateTime1 = Convert.ToDateTime("1 Apr" + str.Substring(0, 4));
        DateTime dateTime2 = Convert.ToDateTime("31 Mar" + Convert.ToString(Convert.ToInt32(str.Substring(0, 4)) + 1));
        if (DateTime.Today < dateTime2 && DateTime.Today < dateTime1)
        {
            dtpFeeDt.SetDateValue(dateTime1);
            txtFeeDt.ReadOnly = true;
            dtpFeeDt.Enabled = false;;
        }
        else if (DateTime.Today > dateTime2 && DateTime.Today > dateTime1)
        {
            dtpFeeDt.SetDateValue(dateTime2);
            txtFeeDt.ReadOnly = true;
            dtpFeeDt.Enabled = false;;
        }
        else
        {
            dtpFeeDt.SetDateValue(DateTime.Today);
            dtpFeeDt.Enabled = false;;
            txtFeeDt.ReadOnly = true;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        FillStudentDetails();
    }

    protected void drpAdditionalFee_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtAddFee.Text = new clsDAL().ExecuteScalarQry("select CAST(Amount AS DECIMAL(10,2)) AS Amount from dbo.PS_FeeAmount where FeeCompId=" + drpAdditionalFee.SelectedValue + " and StreamId=(select StreamID from dbo.PS_ClassMaster where ClassID=" + drpclass.SelectedValue.ToString() + ")");
    }

    private void GrdClear()
    {
        grdStudentList.DataSource = null;
        grdStudentList.DataBind();
        btnSubmit.Visible = false;
        txtAddFee.Text = "";
        dtpFeeDt.SetDateValue(DateTime.Now);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            lblMsg.Text = "Select a checkbox  !";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                UpadateTrans(obj.ToString());
            lblMsg.Text = "Misc fee generated successfully !";
            lblMsg.ForeColor = Color.Green;
            GrdClear();
        }
    }

    private void UpadateTrans(string Admnno)
    {
        ht.Clear();
        ht.Add("@TransDate", dtpFeeDt.GetDateValue());
        ht.Add("@AdmnNo", Admnno);
        ht.Add("@TransDesc", drpAdditionalFee.SelectedItem.ToString().Trim());
        ht.Add("@Debit", txtAddFee.Text.Trim());
        ht.Add("@Credit", "0.0000");
        ht.Add("@Balance", txtAddFee.Text.Trim());
        ht.Add("@UserID", Session["User_Id"]);
        ht.Add("@AdFeeId", drpAdditionalFee.SelectedValue.ToString());
        ht.Add("@SchoolId", Session["SchoolId"].ToString());
        dt = obj.GetDataTable("Ps_Sp_InsAdditionalFee", ht);
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        GrdClear();
        setMiscDate();
        UpdatePanel2.Update();
    }
}