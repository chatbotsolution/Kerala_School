using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class FeeManagement_GenerateFee : System.Web.UI.Page
{
    private string curSess = "";
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SchoolId"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        fillsession();
        FillClassDropDown();
    }

    protected void fillsession()
    {
        obj = new clsDAL();
        DataTable dataTable = new DataTable();
        drpsession.DataSource = obj.GetDataTableQry("SELECT SessionID,SessionYr FROM dbo.PS_FeeNormsNew ORDER BY SessionID DESC");
        drpsession.DataTextField = "SessionYr";
        drpsession.DataValueField = "SessionYr";
        drpsession.DataBind();
    }

    private void FillClassDropDown()
    {
        obj = new clsDAL();
        drpClass.DataSource = obj.GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassID";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void btnGenFee_Click(object sender, EventArgs e)
    {
        btnGenFee.Enabled = false;
        if (drpstudent.SelectedValue.ToString().Trim() == "0")
            GenerateFeeAll();
        else
            InsertDetailsInLedger(drpstudent.SelectedValue.ToString().Trim());
    }

    protected void GenerateFeeAll()
    {
        obj = new clsDAL();
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select distinct c.admnno from dbo.PS_ClasswiseStudent c ");
        stringBuilder.Append(" inner join dbo.PS_StudMaster s on s.AdmnNo=c.AdmnNo");
        stringBuilder.Append(" where c.SessionYear='" + drpsession.SelectedValue.ToString().Trim() + "' and c.ClassID=" + drpClass.SelectedValue.ToString());
        stringBuilder.Append(" and s.StudType='" + txtStudType.Text.Trim() + "' order by c.admnno");
        DataTable dataTableQry = obj.GetDataTableQry(stringBuilder.ToString());
        int num = 0;
        foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
        {
            try
            {
                InsertDetailsInLedger(row["admnno"].ToString());
            }
            catch (Exception ex)
            {
                ++num;
                btnGenFee.Enabled = true;
            }
        }
        if (num > 0)
            lblerr.Text = "Records could not be generated: " + num.ToString();
        else
            lblerr.Text = "All records generated successfully";
    }

    private void InsertDetailsInLedger(string Admn)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SessYr", drpsession.SelectedValue.ToString().Trim());
        hashtable.Add("@AdmnNo", Admn);
        string str1 = "04/01/" + drpsession.SelectedValue.ToString().Trim().Substring(0, 4);
        hashtable.Add("@TransDt", str1);
        if (clsDal.ExecuteScalar("Del_AllRefOnReGenFee", hashtable).Trim() == "")
        {
            string str2 = "";
            try
            {
                InsertDetailsForBusFee(Admn);
            }
            catch (Exception ex)
            {
                str2 = ex.ToString();
                btnGenFee.Enabled = true;
            }
            if (str2.Trim() == "")
            {
                try
                {
                    string str3 = "Y";
                    string str4 = "Y";
                    clsGenerateFee clsGenerateFee = new clsGenerateFee();
                    string str5 = drpsession.SelectedValue.ToString().Trim().Substring(0, 4);
                    string str6 = drpsession.SelectedValue.ToString().Trim().Substring(0, 2) + drpsession.SelectedValue.ToString().Trim().Substring(5, 2);
                    DateTime dateTime = Convert.ToDateTime("04/01/" + str5);
                    clsGenerateFee.GenerateFeeOnAdmission(dateTime, dateTime, Admn, str3, str4, drpsession.SelectedValue.ToString().Trim(), Session["User_Id"].ToString(), Session["SchoolId"].ToString(), char.Parse(txtStudType.Text.Trim()));
                    btnGenFee.Enabled = true;
                    lblerr.Text = "Generated successfully";
                    lblerr.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    str2 = ex.ToString();
                }
                if (!(str2.Trim() != ""))
                    return;
                lblerr.Text = "Error While Generating School Fee! Please Try Again!!";
                lblerr.ForeColor = Color.Red;
                btnGenFee.Enabled = true;
            }
            else
            {
                lblerr.Text = "Error While Generating Bus Fee! Please Try Again!!";
                lblerr.ForeColor = Color.Red;
                btnGenFee.Enabled = true;
            }
        }
        else
        {
            lblerr.Text = "Unable to ReGenerate fee!!Please Try Again later!";
            lblerr.ForeColor = Color.Red;
            btnGenFee.Enabled = true;
        }
    }

    private void InsertDetailsForBusFee(string StudAdmnNo)
    {
        new clsDAL().ExcuteProcInsUpdt("ps_sp_insert_AdFeeLedgerBus", new Hashtable()
    {
      {
         "Admnno",
         StudAdmnNo
      },
      {
         "uid",
         Convert.ToInt32(Session["User_Id"])
      },
      {
         "Session",
         drpsession.SelectedValue.ToString().Trim()
      },
      {
         "SchoolId",
         Session["SchoolId"].ToString().Trim()
      }
    });
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    private void fillstudent()
    {
        clsDAL clsDal = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select fullname,s.admnno from PS_ClasswiseStudent cs join ps_studmaster s on ");
        stringBuilder.Append(" s.admnno=cs.admnno where cs.classid=" + drpClass.SelectedValue);
        stringBuilder.Append(" and cs.SessionYear='" + drpsession.SelectedValue.ToString().Trim() + "' and s.StudType='" + txtStudType.Text.Trim().ToUpper() + "' order by fullname ");
        DataTable dataTableQry = clsDal.GetDataTableQry(stringBuilder.ToString().Trim());
        lblerr.Text = "Total Students: " + dataTableQry.Rows.Count.ToString();
        try
        {
            drpstudent.DataSource = dataTableQry;
            drpstudent.DataTextField = "fullname";
            drpstudent.DataValueField = "admnno";
            drpstudent.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        if (dataTableQry.Rows.Count > 0)
            drpstudent.Items.Insert(0, new ListItem("-Select-", "0"));
        else
            drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerr.Text = "Admission No: " + drpstudent.SelectedValue.ToString();
    }
}