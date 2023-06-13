using ASP;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Admissions_NewAdmnFeeGen : System.Web.UI.Page
{
    public static int grdrowcount;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        FillSession();
        FillClass();
    }
    private void FillSession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select distinct SessionID,SessionYr from dbo.PS_FeeNormsNew order by sessionID Desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }
    private void FillClass()
    {
        drpClass.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassForAdmnDDL");
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "classid";
        drpClass.DataBind();
        drpClass.Items.RemoveAt(0);
        drpClass.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    protected void btnFee_Click(object sender, EventArgs e)
    {
        string str1 = "Y";
        string str2 = "Y";
        string str3 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        DateTime dateTime = Convert.ToDateTime("04/01/" + str3);
        clsGenerateFee clsGenerateFee = new clsGenerateFee();

        foreach (GridViewRow row in grdstudents.Rows)
        {
            string Admn = (row.FindControl("lblid") as Label).Text.ToString();
            string sixthopt = (row.FindControl("lblSixthSub") as Label).Text.ToString();
            clsGenerateFee.GenerateFeeOnAdmission(dateTime, dateTime, Admn, str1, str2, drpSession.SelectedValue.ToString().Trim(), Session["User_Id"].ToString(), Session["SchoolId"].ToString(), char.Parse("N"));
            if (sixthopt == "83" || sixthopt == "84")
            {
                new clsDAL().ExecuteScalarQry("Update Ps_FeeLedger Set Debit=0.00,Balance=0.00 where Credit=0.00 and AdmnNo=" + Admn + " and FeeId=11");
            }
        }
        FillGrid();
        lblMsg.Text = "Fee Generated";
        //clsGenerateFee.GenerateFeeOnAdmission(dateTime, dateTime, Admn, str1, str2, drpSession.SelectedValue.ToString().Trim(), Session["User_Id"].ToString(), Session["SchoolId"].ToString(), char.Parse("N"));
    }
    private void FillGrid()
    {
        string query = "Select P.*,P.admnNo as AdmsnNo,Cs.Classid,cs.Stream from Ps_StudMaster P inner join Ps_Classwisestudent cs on P.admnNo= cs.admnNo where P.admnno not in(Select distinct admnno from Ps_FeeLedger) and P.admnsessyr='" + drpSession.SelectedValue.ToString().Trim() + "' and cs.classid='" + drpClass.SelectedValue.ToString().Trim() + "' ";
        clsDAL clsDal = new clsDAL();
        DataTable dt = clsDal.GetDataTableQry(query);
        Admissions_NewAdmnFeeGen.grdrowcount = grdstudents.Rows.Count;
        if (dt.Rows.Count > 0)
        {
            grdstudents.DataSource = dt;
            grdstudents.DataBind();
            grdstudents.Visible = true;
            lblRecCount.Text = "Total Number of Student: " + dt.Rows.Count.ToString();
        }
        else
        {
         
            grdstudents.Visible = false;
            lblRecCount.Text = "";
            lblMsg.Text = "No Records Found !";
            lblMsg.ForeColor = Color.Red;
        }

    }
    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdstudents.Visible = true;
        FillGrid();
    }
}