
using AjaxControlToolkit;
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

public partial class Admissions_classpromotion : System.Web.UI.Page
{
    private clsStaticDropdowns clsSt = new clsStaticDropdowns();
    private string StudType = "";
    public static int grdrowcount;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        btncancel.Visible = false;
        btndetain.Visible = false;
        btnprom.Visible = false;
        filldropdown();
        if (drpsession.Items.Count > 0)
            FillNewSession();
        else
            txtnewsession.Text = "";
    }

    private void FillNewSession()
    {
        string str1 = drpsession.SelectedValue.ToString().Trim();
        if (str1.Length != 7)
            return;
        int int32_1 = Convert.ToInt32(str1.Substring(0, 4));
        int int32_2 = Convert.ToInt32(str1.Substring(5, 2));
        string str2 = int32_2 >= 9 ? Convert.ToString(int32_2 + 1) : "0" + Convert.ToString(int32_2 + 1);
        txtnewsession.Text = Convert.ToString(int32_1 + 1) + "-" + str2;
    }

    private void filldropdown()
    {
        fillsession();
        fillclass();
    }

    private void fillclass()
    {
        DataTable dataTableQry = new clsDAL().GetDataTableQry("select * from ps_classmaster");
        drpclass.DataSource = dataTableQry;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("--SELECT--", "0"));
        drpnewclass.DataSource = dataTableQry;
        drpnewclass.DataTextField = "classname";
        drpnewclass.DataValueField = "classid";
        drpnewclass.DataBind();
        drpnewclass.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    private void FillSectionDropDown()
    {
        // int int16 = (int)Convert.ToInt16(ConfigurationManager.AppSettings["TotSec"].ToString());
        int int16 = (int)Convert.ToInt16(new clsDAL().ExecuteScalarQry("Select NoOfSections from PS_ClassMaster where ClassId=" + drpclass.SelectedValue.ToString().Trim() + ""));
        int num1 = int16;
        if (int16 <= 0)
            return;
        drpSection.Items.Clear();
        int num2 = 65;
        int index = 0;
        while (num1 > 0)
        {
            drpSection.Items.Insert(index, new ListItem(Convert.ToChar(num2).ToString(), Convert.ToChar(num2).ToString()));
            --num1;
            ++index;
            ++num2;
        }
        drpSection.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    private void changeclass()
    {
        try
        {
            drpnewclass.SelectedIndex = drpclass.SelectedIndex + 1;
            if (Convert.ToInt32(drpnewclass.SelectedValue) > 13)
            {
                drpStream.Enabled = true;
            }
            else
                drpStream.Enabled = false;
        }
        catch
        {
        }
    }

    private void fillsession()
    {
        drpsession.DataSource = new clsDAL().GetDataTableQry("select distinct sessionyear from PS_ClasswiseStudent  where Detained_Promoted ='' order by  sessionyear desc");
        drpsession.DataTextField = "sessionyear";
        drpsession.DataValueField = "sessionyear";
        drpsession.DataBind();
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSectionDropDown();
        lblMsg.Text = "";
       // fillgrid();
        changeclass();
        if (drpsession.Items.Count > 0)
            FillNewSession();
        else
            txtnewsession.Text = "";
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillgrid();
    }
    protected void drpsession_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = "";
        fillgrid();
        FillNewSession();
    }

    //protected void drpsection_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    lblMsg.Text = "";
    //    foreach (Control row in grdstudents.Rows)
    //        ((ListControl)row.FindControl("drpsection")).SelectedItem.ToString();
    //}

    private void fillgrid()
    {
        DataTable dataTable = new clsDAL().GetDataTable("ps_sp_show_classstudents", new Hashtable()
    {
      {
         "classid",
         drpclass.SelectedValue
      },
      {
         "session",
         drpsession.SelectedValue
      },
       {
         "Section",
         drpSection.SelectedValue
      }
    });
        grdstudents.DataSource = dataTable;
        grdstudents.DataBind();
        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
            ((DropDownList)grdstudents.Rows[i].FindControl("drpsection")).SelectedValue = dataTable.Rows[i]["Section"].ToString();

        }
        
        Admissions_classpromotion.grdrowcount = grdstudents.Rows.Count;
        if (Admissions_classpromotion.grdrowcount > 0)
        {
            lblRecCount.Text = "Total Records: " + dataTable.Rows.Count.ToString();
            grdstudents.Visible = true;
            btncancel.Visible = true;
            btndetain.Visible = true;
            btnprom.Visible = true;
        }
        else
        {
            lblRecCount.Text = "";
            grdstudents.Visible = false;
            btncancel.Visible = false;
            btndetain.Visible = false;
            btnprom.Visible = false;
        }
    }

    protected void btndetain_Click(object sender, EventArgs e)
    {
        save("d");
    }

    protected void btnprom_Click(object sender, EventArgs e)
    {
        save("p");
    }

    private void save(string dp)
    {
        if (Convert.ToInt32(new clsDAL().ExecuteScalarQry("select count(*) from dbo.PS_FeeNormsNew where SessionYr='" + txtnewsession.Text.Trim() + "'")) > 0)
        {
            int num1 = 0;
            foreach (Control row in grdstudents.Rows)
            {
                if (((CheckBox)row.FindControl("chk")).Checked)
                    ++num1;
            }
            if (num1 == 0)
            {
                ScriptManager.RegisterClientScriptBlock((Control)btnprom, btnprom.GetType(), "ShowMessage", "alert('Select a Record');", true);
            }
            else
            {
                int num2 = !(dp == "d") ? Convert.ToInt32(drpnewclass.SelectedValue) : Convert.ToInt32(drpclass.SelectedValue);
                string str = "";
                foreach (GridViewRow row in grdstudents.Rows)
                {
                    if (((CheckBox)row.FindControl("chk")).Checked)
                    {
                        Hashtable hashtable = new Hashtable();
                        //hashtable.Add("grade", ((ListControl)row.FindControl("drpgrade")).SelectedItem.ToString());
                        if (((ListControl)row.FindControl("drpgrade")).SelectedIndex != -1)
                        {
                            hashtable.Add("grade", ((ListControl)row.FindControl("drpgrade")).SelectedItem.ToString());
                        }
                        else
                            hashtable.Add("grade", null);
                        if (Convert.ToInt32(drpnewclass.SelectedValue) > 13)
                            hashtable.Add("StreamId", drpStream.SelectedValue);
                        else
                            hashtable.Add("StreamId", 1);
                        hashtable.Add("oldsession", drpsession.SelectedValue);
                        hashtable.Add("session", txtnewsession.Text.Trim());
                        hashtable.Add("classid", num2);
                        hashtable.Add("section", ((ListControl)row.FindControl("drpsection")).SelectedItem.ToString());
                        hashtable.Add("userid", Convert.ToInt32(Session["User_Id"].ToString().Trim()));
                        hashtable.Add("schoolid", Session["SchoolId"].ToString());
                        hashtable.Add("rollno", ((TextBox)row.FindControl("txtrollno")).Text.Trim());
                        hashtable.Add("id", ((Label)row.FindControl("lblid")).Text.Trim());
                        hashtable.Add("admnno", ((Label)row.FindControl("lbladmnno")).Text.Trim());
                        hashtable.Add("dp", dp);
                        Label control1 = (Label)row.FindControl("lbladmnno");
                        Label control2 = (Label)row.FindControl("lblid");
                        Label control3 = (Label)row.FindControl("lblStudType");
                        Label control4 = (Label)row.FindControl("lblOldadmnno");
                        str = str + "," + control1.Text.Trim();
                        new clsDAL().GetDataTable("ps_sp_updatestudentclass", hashtable);
                        StudType = control3.Text.ToString().ToUpper().Trim();
                        InsertDetailsInLedger(control1.Text.Trim(), control2.Text.Trim());
                     //   DataTable dt=new FeeDetails().GetFeeDetailsbyAdmnNo(control1.Text.Trim(), txtnewsession.Text.Trim());
                        //InsertIntoFeeLedger(dt,control1.Text.Trim());
                    }
                }
                fillgrid();
                lblMsg.Text = "Record Saved Successfuly";
                lblMsg.ForeColor = Color.Green;
            }
        }
        else
        {
            lblMsg.Text = "First define new session before promoting the students";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void InsertIntoFeeLedger(DataTable dt, string p)
    {
        clsDAL clsdal = new clsDAL();
        

    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Home.aspx");
    }

    public DataTable getgrades()
    {
        return new clsDAL().GetDataTableQry("select * from ps_grademaster");
    }

    public DataTable getsection()
    {
        clsSt = new clsStaticDropdowns();
        return clsSt.GetSectionDT();
    }

    private void InsertDetailsInLedger(string Admn, string ClasswiseId1)
    {
        try
        {
            new clsDAL().ExcuteProcInsUpdt("Del_AllRefOnClassPromotion", new Hashtable()
      {
        {
           "@SessYr",
           txtnewsession.Text.Trim()
        },
        {
           "@AdmnNo",
           Admn
        }
      });
            InsAdFeeLedger(Admn, ClasswiseId1, 1, "Bus Fee");
            InsAdFeeLedger(Admn, ClasswiseId1, 2, "Hostel Fee");
            string str1 = "Y";
            string str2 = "Y";
            clsGenerateFee clsGenerateFee = new clsGenerateFee();
            string str3 = txtnewsession.Text.Trim().Substring(0, 4);
            string str4 = txtnewsession.Text.Trim().Substring(0, 2) + txtnewsession.Text.Trim().Substring(5, 2);
            DateTime dateTime = Convert.ToDateTime("04/01/" + str3);
            if (StudType == "C")
                clsGenerateFee.GenerateFeeOnAdmission(dateTime, dateTime, Admn, str1, str2, txtnewsession.Text.Trim(), Session["User_Id"].ToString(), Session["SchoolId"].ToString(), char.Parse("C"));
            else
                clsGenerateFee.GenerateFeeOnAdmission(dateTime, dateTime, Admn, str1, str2, txtnewsession.Text.Trim(), Session["User_Id"].ToString(), Session["SchoolId"].ToString(), char.Parse("E"));
        }
        catch (Exception ex)
        {
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

    private void InsAdFeeLedger(string AdmnNo, string ClasswiseId2, int AdId, string AdFeeDesc)
    {
        DataTable dataTable = new DataTable();
        int M1 = 4;
        for (int index = M1; index <= 15; ++index)
        {
            clsDAL clsDal = new clsDAL();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@OldSessYr", drpsession.SelectedValue.ToString().Trim());
            hashtable.Add("@NewSessYr", txtnewsession.Text.Trim());
            string adFeeDate = CreateAdFeeDate(M1);
            hashtable.Add("@AdFeeDate", adFeeDate);
            hashtable.Add("@ClasswiseIdOld", Convert.ToInt32(ClasswiseId2));
            hashtable.Add("@AdmnNo", AdmnNo);
            hashtable.Add("@AddFeeDesc", AdFeeDesc);
            hashtable.Add("@UserId", Session["User_Id"]);
            hashtable.Add("@SchoolId", Session["SchoolId"].ToString());
            hashtable.Add("@AdMonth", M1);
            hashtable.Add("@Ad_Id", AdId);
            clsDal.ExcuteProcInsUpdt("ps_sp_InsAdFeeLedgerOnClassPromotion", hashtable);
            ++M1;
        }
        int M2 = 1;
        for (int index = M2; index <= 3; ++index)
        {
            clsDAL clsDal = new clsDAL();
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@OldSessYr", drpsession.SelectedValue.ToString().Trim());
            hashtable.Add("@NewSessYr", txtnewsession.Text.Trim());
            string adFeeDate = CreateAdFeeDate(M2);
            hashtable.Add("@AdFeeDate", adFeeDate);
            hashtable.Add("@ClasswiseIdOld", Convert.ToInt32(ClasswiseId2));
            hashtable.Add("@AdmnNo", AdmnNo);
            hashtable.Add("@AddFeeDesc", AdFeeDesc);
            hashtable.Add("@UserId", Session["User_Id"]);
            hashtable.Add("@SchoolId", Session["SchoolId"].ToString());
            hashtable.Add("@AdMonth", M2);
            hashtable.Add("@Ad_Id", AdId);
            clsDal.ExcuteProcInsUpdt("ps_sp_InsAdFeeLedgerOnClassPromotion", hashtable);
            ++M2;
        }
    }

    public string CreateAdFeeDate(int M)
    {
        string str1 = txtnewsession.Text.Trim().Substring(0, 4);
        string str2 = txtnewsession.Text.Trim().Substring(5, 2);
        int int16 = (int)Convert.ToInt16(M);
        string str3 = "";
        if (int16 > 0 && int16 < 4)
            str3 = int16.ToString().Trim() + "/01/" + str1.Substring(0, 2) + str2;
        else if (int16 > 3 && int16 < 13)
            str3 = M.ToString() + "/01/" + str1;
        return str3;
    }
}