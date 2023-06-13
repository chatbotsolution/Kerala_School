using ASP;
using Classes.DA;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class FeeManagement_ConcessionCovid : System.Web.UI.Page
{
    Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
      //  txtadminno.Text = "0";
        fillsession();
       // fillclass();
       // FillClassSection();
       // fillstudent();
       // btnprint.Visible = false;
    }
    protected void fillsession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }
    private void fillclass()
    {
        drpclass.Items.Clear();
        ht.Clear();
        ht.Add("@SessionYear",drpSession.SelectedValue);
        drpclass.DataSource = new Common().GetDataTable("ps_sp_get_classesForCovidConc",ht);
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillclass();
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void drpstudents_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void drpFeeHead_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            clsDAL clsDal = new clsDAL();
            Common common = new Common();
            DataTable dataTable = common.ExecuteSql("Select Admnno from PS_ClasswiseStudent where classId=" + drpclass.SelectedValue + " AND SessionYear='" + drpSession.SelectedValue + "'");

            int rowCheck = 0;

            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                Int32 admnno = Convert.ToInt32(row["Admnno"].ToString().Trim());
                DataTable dataTableForEachFeeid = common.ExecuteSql("Select distinct debit from PS_FeeLedger where SessionYr='" + drpSession.SelectedValue + "' AND AdmnNo=" + admnno + " and feeId=" + drpFeeHead.SelectedValue);

                double amount = Convert.ToDouble(dataTableForEachFeeid.Rows[0]["debit"].ToString());
                double newAmount = amount - Convert.ToDouble(Convert.ToDecimal(txtPer.Text.ToString()) / 100 * Convert.ToDecimal(dataTableForEachFeeid.Rows[0]["debit"].ToString()));

                ht.Clear();
                ht.Add("@SessionYear", drpSession.SelectedValue);
                ht.Add("@Amount", newAmount);
                ht.Add("@Admnno", admnno);
                ht.Add("@feeId", drpFeeHead.SelectedValue);

                DataTable dataTableAdd = clsDal.GetDataTable("ps_UpdateFeeLedForCovid", ht);
                rowCheck++;
            }
            if(rowCheck==dataTable.Rows.Count)
            {
                lblReport.Text = "Concession Process Success";
            }
            else
            {
                lblReport.Text = "Something wrong go throug again!!!";
            }
        }
        catch (Exception rx)
        {

        }
        }
}