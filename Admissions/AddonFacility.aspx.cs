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

public partial class Admissions_AddonFacility : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        FillSessionDropdown();
        FillClassDropDown();
        bindExtraFee();
        if (Request.QueryString["admnno"] == null)
            return;
        drpSession.SelectedValue = Request.QueryString["sess"].ToString();
        drpClass.SelectedValue = Request.QueryString["cid"].ToString();
        FillSectionDropDown();
        FillSelectStudent();
        drpSelectStudent.SelectedValue = Request.QueryString["admnno"].ToString();
        SetAddonFacilityChoice();
    }

    private void FillClassDropDown()
    {
        obj = new clsDAL();
        drpClass.DataSource = obj.GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassID";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("Select", "0"));
    }

    private void FillSessionDropdown()
    {
        obj = new clsDAL();
        drpSession.DataSource = obj.GetDataTableQry("select top 2 SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillSectionDropDown()
    {
        obj = new clsDAL();
        drpSection.DataSource = obj.GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "classId",
         drpClass.SelectedValue
      },
      {
         "session",
         drpSession.SelectedValue
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("ALL", "0"));
    }

    private void bindExtraFee()
    {
        DataTable dataTable = new DataTable();
        chkFee.DataSource = obj.GetDataTableQry("select FeeID, FeeName from dbo.PS_FeeComponents where Priority=777 order by FeeName").DefaultView;
        chkFee.DataTextField = "FeeName";
        chkFee.DataValueField = "FeeID";
        chkFee.DataBind();
    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        try
        {
            lblMsg.Text = "";
            for (int index = 0; index <= chkFee.Items.Count - 1; ++index)
            {
                Hashtable hashtable = new Hashtable();
                DataTable dataTable = new DataTable();
                if (chkFee.Items[index].Selected)
                {
                    clsDAL clsDal = new clsDAL();
                    hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
                    hashtable.Add("@AdmnNo", drpSelectStudent.SelectedValue.ToString().Trim());
                    hashtable.Add("@AddnlFacilityFeeId", chkFee.Items[index].Value.ToString());
                    hashtable.Add("@UserId", Session["User_Id"].ToString());
                    if (clsDal.GetDataTable("PS_InsUpdtAddnlFacilityChoice", hashtable).Rows.Count > 0)
                        lblMsg.Text = lblMsg.Text + chkFee.Items[index].Text + ",";
                }
            }
            if (lblMsg.Text.Trim() != "")
            {
                GenFeeForAddonFacility(drpSelectStudent.SelectedValue.ToString().Trim());
                lblMsg.Text = "Record Updated Successfully For : " + lblMsg.Text;
                lblMsg.ForeColor = Color.Green;
            }
            else
            {
                lblMsg.Text = "Duplicate Record Exists";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message.ToString();
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void GenFeeForAddonFacility(string AdmnNo)
    {
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        clsDAL clsDal = new clsDAL();
        DateTime dateTime = Convert.ToDateTime(clsDal.ExecuteScalarQry("select StartDate from dbo.PS_FeeNormsNew  where SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "'"));
        string s = !(Convert.ToDateTime(clsDal.ExecuteScalarQry("select AdmnDate from dbo.PS_StudMaster where admnno=" + drpSelectStudent.SelectedValue.ToString())) < dateTime) ? "N" : "E";
        clsGenerateFee.GenFeeOnAdmnAddonFacility(dateTime, AdmnNo, drpSession.SelectedValue.ToString().Trim(), Session["User_Id"].ToString(), Session["SchoolId"].ToString(), char.Parse(s));
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSectionDropDown();
        FillSelectStudent();
    }

    private void FillSelectStudent()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@SessionYear", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@ClassID", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        drpSelectStudent.DataSource = clsDal.GetDataTable("ps_GetStudents", hashtable);
        drpSelectStudent.DataTextField = "fullname";
        drpSelectStudent.DataValueField = "admnno";
        drpSelectStudent.DataBind();
        drpSelectStudent.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSection.SelectedIndex > 0)
            FillSelectStudent();
        else
            drpSelectStudent.Items.Clear();
    }

    private void FillGrid()
    {
        obj = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@Session", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@AdmnNo", drpSelectStudent.SelectedValue.ToString());
        int count = obj.GetDataTable("ps_GetStudListExtraFee", hashtable).Rows.Count;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void drpSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetAddonFacilityChoice();
    }

    protected void SetAddonFacilityChoice()
    {
        for (int index = 0; index <= chkFee.Items.Count - 1; ++index)
            chkFee.Items[index].Selected = false;
        lblMsg.Text = "";
        DataTable dataTableQry = new clsDAL().GetDataTableQry("select AddnlFacilityFeeId from dbo.PS_AddnlFacilityChoice where AdmnNo=" + drpSelectStudent.SelectedValue + " and SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "'");
        if (dataTableQry.Rows.Count <= 0)
            return;
        foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
        {
            string str = row[0].ToString();
            for (int index = 0; index <= chkFee.Items.Count - 1; ++index)
            {
                if (str.Trim() == chkFee.Items[index].Value.ToString())
                    chkFee.Items[index].Selected = true;
            }
        }
    }

    protected void btnStudAdmn_Click(object sender, EventArgs e)
    {
        Response.Redirect("Addstudentinfo.aspx");
    }

    protected void btnStudList_Click(object sender, EventArgs e)
    {
        Response.Redirect("showstudents.aspx");
    }

    protected void btnDelChoice_Click(object sender, EventArgs e)
    {
        for (int index = 0; index <= chkFee.Items.Count - 1; ++index)
        {
            if (chkFee.Items[index].Selected)
            {
                if (new clsDAL().GetDataTable("Del_AddonFacility", new Hashtable()
        {
          {
             "@AdmnNo",
             drpSelectStudent.SelectedValue.ToString()
          },
          {
             "@SessYr",
             drpSession.SelectedValue.ToString().Trim()
          },
          {
             "@FeeId",
             chkFee.Items[index].Value.ToString()
          }
        }).Rows.Count > 0)
                {
                    lblMsg.Text = "The selected choice Can not be deleted now. Try Again.";
                    lblMsg.ForeColor = Color.Red;
                }
                else
                {
                    lblMsg.Text = "The selected choice deleted successfully.";
                    lblMsg.ForeColor = Color.Green;
                }
            }
        }
    }
}