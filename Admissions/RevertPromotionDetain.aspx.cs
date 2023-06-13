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

public partial class Admissions_RevertPromotionDetain : System.Web.UI.Page
{
    public static int grdrowcount;
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        fillsession();
        fillclass();
        drpSession.Enabled = false;
    }

    protected void fillsession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        drpClass.Items.Clear();
        drpClass.DataSource = new clsDAL().GetDataTableQry("SELECT ClassID,ClassName FROM dbo.PS_ClassMaster");
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassID";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("-Select-", "0"));
    }
    private void FillSectionDropDown()
    {
        int ClassId = Convert.ToInt32(drpClass.SelectedValue.ToString().Trim());
        DataTable dt = new clsDAL().GetDataTableQry("SELECT NoOfSections FROM dbo.PS_ClassMaster where classid =" + ClassId + "");
        int int16 = (int)Convert.ToInt16(dt.Rows[0]["NoOfSections"].ToString().Trim());
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
    //protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    //{
        
    //    btnRevert.Enabled = false;
    //    StringBuilder stringBuilder = new StringBuilder();
    //    Hashtable hashtable = new Hashtable();
    //    DataTable dataTable1 = new DataTable();
    //    if (drpClass.SelectedIndex > 0)
    //    {
    //        hashtable.Add("@Class", drpClass.SelectedValue.ToString().Trim());
    //        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
    //        hashtable.Add("@Section",drpSection.SelectedValue.ToString().Trim());
    //        DataTable dataTable2 = obj.GetDataTable("Ps_Sp_GetStudsBySection", hashtable);
    //        try
    //        {
    //            Admissions_RevertPromotionDetain.grdrowcount = dataTable2.Rows.Count;
    //            grdstudents.DataSource = dataTable2;
    //            grdstudents.DataBind();
    //            //drpStudent.DataSource = dataTable2;
    //            //drpStudent.DataTextField = "fullname";
    //            //drpStudent.DataValueField = "admnno";
    //            //drpStudent.DataBind();
    //        }
    //        catch (Exception ex)
    //        {
    //            ex.Message.ToString();
    //        }
    //       // drpStudent.Items.Insert(0, new ListItem("-Select-", "0"));
    //    }
    //    else
    //        ClearAll();
    //}
    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSectionDropDown();
        //btnRevert.Enabled = false;
        //StringBuilder stringBuilder = new StringBuilder();
        //Hashtable hashtable = new Hashtable();
        //DataTable dataTable1 = new DataTable();
        //if (drpClass.SelectedIndex > 0)
        //{
        //    hashtable.Add("@Class", drpClass.SelectedValue.ToString().Trim());
        //    hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
        //    DataTable dataTable2 = obj.GetDataTable("Ps_Sp_GetStuds", hashtable);
        //    try
        //    {
        //        Admissions_RevertPromotionDetain.grdrowcount = dataTable2.Rows.Count;
        //        grdstudents.DataSource = dataTable2;
        //        grdstudents.DataBind();
        //        //drpStudent.DataSource = dataTable2;
        //        //drpStudent.DataTextField = "fullname";
        //        //drpStudent.DataValueField = "admnno";
        //        //drpStudent.DataBind();
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.Message.ToString();
        //    }
        //   // drpStudent.Items.Insert(0, new ListItem("-Select-", "0"));
        //}
        //else
        //    ClearAll();
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearAll();
    }

    protected void btnRevert_Click(object sender, EventArgs e)
    {
        int num1 = 0;
        foreach (Control row in grdstudents.Rows)
        {
            if (((CheckBox)row.FindControl("chk")).Checked)
                ++num1;
        }
        if (num1 == 0)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnRevert, btnRevert.GetType(), "ShowMessage", "alert('Select a Record');", true);
        }
        else
        {
            foreach (GridViewRow row in grdstudents.Rows)
            {
                if (((CheckBox)row.FindControl("chk")).Checked)
                {
                    string admnno = ((Label)row.FindControl("hftxtAdmno")).Text;
                    //DataTable dt = new clsDAL().GetDataTableQry("select Admnno from Ps_StudMaster where OldAdmnno ='" + no + "'");
                    //int admnno = Convert.ToInt32(dt.Rows[0]["Admnno"].ToString().Trim());


                    try
                    {
                        Hashtable hashtable = new Hashtable();
                        if (Convert.ToInt32(obj.ExecuteScalarQry("Select COUNT(*) from dbo.PS_StudMaster where AdmnNo=" + admnno + " and AdmnSessYr='" + drpSession.SelectedValue.ToString() + "'")) == 0)
                        {
                            if (Convert.ToInt32(obj.ExecuteScalarQry("Select COUNT(*) from dbo.PS_FeeLedger where AdmnNo=" + admnno + " and SessionYr='" + drpSession.SelectedValue.ToString() + "' and Credit>0")) == 0)
                            {
                                string[] strArray = drpSession.SelectedValue.ToString().Trim().Split('-');
                                string str = Convert.ToString(Convert.ToInt32(strArray[0]) - 1) + "-" + Convert.ToString(Convert.ToInt32(strArray[1]) - 1);

                                hashtable.Add("@AdmnNo", admnno);
                                hashtable.Add("@CurrSY", drpSession.SelectedValue.ToString().Trim());
                                hashtable.Add("@PrevSY", str.Trim());
                                hashtable.Add("@UserId", Session["User_Id"].ToString().Trim());
                                if (obj.ExecuteScalar("Ps_Sp_RevertStuds", hashtable) == "Ok")
                                {
                                    lblMsg.Text = "Student Reverted Sucessfully!!";
                                    lblMsg.ForeColor = Color.Green;
                                }
                                else
                                {
                                    lblMsg.Text = "Cannot Revert This Student!! Try Again!!";
                                    lblMsg.ForeColor = Color.Red;
                                }
                            }
                            else
                            {
                                lblMsg.Text = "Fee Already Received!! Cannot Revert This Student!!";
                                lblMsg.ForeColor = Color.Red;
                            }
                        }
                        else
                        {
                            lblMsg.Text = "New student!! Cannot Revert This Student!!";
                            lblMsg.ForeColor = Color.Red;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            FillGrid();
        }
    }

    //protected void drpStudent_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    DataTable dt = new clsDAL().GetDataTableQry("select OldAdmnNo From Ps_StudMaster where admnNo =" + drpStudent.SelectedValue.ToString().Trim() + "");
    //    txtAdmnno.Text = dt.Rows[0]["OldAdmnNo"].ToString();
    //    if (drpStudent.SelectedIndex > 0)
    //        btnRevert.Enabled = true;
    //    else
    //        btnRevert.Enabled = false;
    //}

    private void ClearAll()
    {
        drpClass.SelectedIndex = 0;
        drpSection.SelectedIndex = 0;
       // drpStudent.Items.Clear();
        //drpStudent.Items.Insert(0, new ListItem("-Select-", "0"));
        txtAdmnno.Text = "";
        btnRevert.Enabled = false;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        //DataTable dataTableQry = obj.GetDataTableQry("select SessionYear,ClassID,Section,rollno,sm.oldAdmnNo,sm.AdmnNo from PS_ClasswiseStudent cs inner join PS_StudMaster sm on sm.AdmnNo=cs.AdmnNo where cs.AdmnNo=" + drpStudent.SelectedValue.ToString().Trim() + " and Detained_Promoted='' and Status=1 ");
        FillGrid();
    }

    private void FillGrid()
    {
        DataTable dataTableQry = obj.GetDataTableQry("select SessionYear,ClassID,Section,rollno,sm.oldAdmnNo,sm.AdmnNo,sm.FullName from PS_ClasswiseStudent cs inner join PS_StudMaster sm on sm.AdmnNo=cs.AdmnNo where cs.Classid=" + drpClass.SelectedValue.ToString().Trim() + "and cs.Section='" + drpSection.SelectedValue.ToString().Trim() + "' and cs.SessionYear='" + drpSession.SelectedValue.ToString().Trim() + "'  and Detained_Promoted='' and Status=1 ");
        if (dataTableQry.Rows.Count > 0)
        {
            //drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
            //drpClass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
            //drpSection.SelectedValue = dataTableQry.Rows[0]["Section"].ToString();
            //drpClass_SelectedIndexChanged(drpStudent, EventArgs.Empty);

            //drpStudent.SelectedValue = dataTableQry.Rows[0]["AdmnNo"].ToString();
            //btnRevert.Enabled = true;
            Admissions_RevertPromotionDetain.grdrowcount = dataTableQry.Rows.Count;
            grdstudents.DataSource = dataTableQry;
            grdstudents.DataBind();
            btnRevert.Enabled = true;
        }
        else
        {
            grdstudents.DataSource = null;
            grdstudents.DataBind();
            ScriptManager.RegisterClientScriptBlock((Control)txtAdmnno, txtAdmnno.GetType(), "ShowMessage", "alert('No student record Found')", true);
            btnRevert.Enabled = false;
        }
    }
    
    protected void BtnSearch_clicked(object sender, EventArgs e)
    {
      
             DataTable dataTableQry = obj.GetDataTableQry("select SessionYear,ClassID,Section,rollno,sm.oldAdmnNo,sm.AdmnNo,sm.FullName from PS_ClasswiseStudent cs inner join PS_StudMaster sm on sm.AdmnNo=cs.AdmnNo where cs.Classid=" + drpClass.SelectedValue.ToString().Trim() + "and cs.Section='" + drpSection.SelectedValue.ToString().Trim()+ "' and cs.SessionYear='"+drpSession.SelectedValue.ToString().Trim()+"' and sm.OldAdmnno ='"+txtAdmnno.Text.Trim()+"'  and Detained_Promoted='' and Status=1 ");
             if (dataTableQry.Rows.Count > 0)
             {
                 //drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
                 //drpClass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
                 //drpSection.SelectedValue = dataTableQry.Rows[0]["Section"].ToString();
                 //drpClass_SelectedIndexChanged(drpStudent, EventArgs.Empty);

                 //drpStudent.SelectedValue = dataTableQry.Rows[0]["AdmnNo"].ToString();
                 //btnRevert.Enabled = true;
                 Admissions_RevertPromotionDetain.grdrowcount = dataTableQry.Rows.Count;
                 grdstudents.DataSource = dataTableQry;
                 grdstudents.DataBind();
                 btnRevert.Enabled = true;
             }
             else
             {
                 ScriptManager.RegisterClientScriptBlock((Control)txtAdmnno, txtAdmnno.GetType(), "ShowMessage", "alert('Admission number does not exists')", true);
                 btnRevert.Enabled = false;
             }
    
     }
    
}
