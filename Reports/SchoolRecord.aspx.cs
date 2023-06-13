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
using System.Web.UI.WebControls;

public partial class Reports_SchoolRecord : System.Web.UI.Page
{
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["user"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        fillsession();
        fillclass();
        FillClassSection();
        fillstudent();
        btnPrint.Visible = false;
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
        drpclass.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    private void FillClassSection()
    {
        drpSection.Items.Clear();
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
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
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }
    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpclass.SelectedIndex > 0)
        {
            FillClassSection();
            fillstudent();
            drpSection.SelectedIndex = 0;
            lblReport.Text = "";
            btnPrint.Visible = false;
            
        }
        else
        {
            drpSection.SelectedIndex = 0;
            drpstudents.SelectedIndex = 0;
           
            lblReport.Text = "";
            btnPrint.Visible = false;
        }
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSection.SelectedIndex > 0)
        {
            fillstudent();
            lblReport.Text = "";
            btnPrint.Visible = false;
           
        }
        else
        {
            drpstudents.SelectedIndex = 0;
            
            lblReport.Text = "";
            btnPrint.Visible = false;
        }
    }

    private void fillstudent()
    {
        drpstudents.Items.Clear();
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@Session", drpSession.SelectedValue);
        if (drpclass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpclass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        DataTable dataTable = common.GetDataTable("ps_sp_get_StudNamesForMly", hashtable);
        drpstudents.DataSource = dataTable;
        drpstudents.DataTextField = "fullname";
        drpstudents.DataValueField = "admnno";
        drpstudents.DataBind();
        if (dataTable.Rows.Count > 0)
            drpstudents.Items.Insert(0, new ListItem("--ALL--", "0"));
        else
            drpstudents.Items.Insert(0, new ListItem("No Record", "0"));
    }
    protected void drpstudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpstudents.SelectedIndex > 0)
        {
            string query = "Select OldAdmnno from Ps_StudMaster where admnno=" + drpstudents.SelectedValue;
            DataTable dt = new clsDAL().GetDataTableQry(query);
            if (dt.Rows.Count > 0)
               // txtadminno.Text = dt.Rows[0]["OldAdmnno"].ToString();
           // else
                //txtadminno.Text = "";

            lblReport.Text = "";
            btnPrint.Visible = false;
        }
        else
        {
           // txtadminno.Text = "0";
            lblReport.Text = "";
            btnPrint.Visible = false;
        }
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
       getdata();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("SchoolRecordPrint.aspx");
    }
    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }
    private void getdata()
    {

        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
        if (drpclass.SelectedValue.ToString() != "0")
            hashtable.Add("@ClassId", drpclass.SelectedValue.ToString().Trim());
        if (drpSection.SelectedValue.ToString() != "0")
            hashtable.Add("@section", drpSection.SelectedValue.ToString().Trim());
        if (drpstudents.SelectedValue.ToString() != "0")
            hashtable.Add("@Admnno", drpstudents.SelectedValue.ToString().Trim());
        DataTable dataTable2 = clsDal.GetDataTable("rptSchoolRecord", hashtable);
         if (dataTable2.Rows.Count > 0)
         {
             btnPrint.Enabled = true;
             btnPrint.Visible = true;
             StringBuilder stringBuilder = new StringBuilder();
             string dt = System.DateTime.Today.ToString("dd MMM yyyy");
             string dt1 = dtpDt.GetDateValue().ToString("dd MMM yyyy");
             foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
             {
                 stringBuilder.Append("<table width='100%' border='0' cellpadding='0' cellspacing='0'><tr><td colspan='4' style='text-align:center; color:#09F; font-weight:bold; font-size:25px;'>LOYOLA SCHOOL, BHUBANESWAR</td></tr>");
                 stringBuilder.Append("<tr><td colspan='2'>Dear Parents,</td><td colspan='2'align='right'>" + dt + " </td></tr>");
                 stringBuilder.Append("<tr><td colspan='4'><p>Before moving to the new academic session, we would like you to help us in updating your child’s database in our records. Kindly declare below your information with your signature and<strong> return the same to the class teacher by " + dt1 + ".</strong></p></td></tr></table>");
                 stringBuilder.Append("<table width='100%' border='1' cellpadding='0' cellspacing='0'>");
                 stringBuilder.Append("<tr bgcolor='#999999'><td style='width:70px;'><strong>SL. NO.</strong></td>");
                 stringBuilder.Append("<td style='width:200px;'><strong>DATA</strong></td>");
                 stringBuilder.Append("<td style='width:300px;'><strong>SCHOOL RECORD</strong></td>");
                 stringBuilder.Append("<td style='width:300px;'><strong>CORRECT DATA (IF ANY)</strong></td></tr>");
                 stringBuilder.Append("<tr><td style='width:50px;'>1</td><td>Class:</td><td>Class:" + row["ClassName"].ToString() + "-" + row["Section"].ToString() + "-" + row["RollNo"].ToString() + " &nbsp:&nbsp; ADMN NO : &nbsp;" + row["OldAdmnNo"].ToString() + "</td>&nbsp; <td></td></tr>");
                 stringBuilder.Append("<tr><td>2</td><td>Candidate’s Name:</td><td>" + row["FullName"].ToString() +"</td><td>&nbsp;</td></tr>");
                 stringBuilder.Append("<tr><td>3.</td><td>Gender:</td> <td>" + row["sex"].ToString() + "</td>&nbsp;<td></td></tr>");
                 stringBuilder.Append(" <tr><td>4.</td><td>Nationality:</td><td>" + row["Nationality"].ToString() + "</td><td>&nbsp;</td></tr>");
                // stringBuilder.Append("<tr><td>5.</td><td>Category:</td><td>"+ row["Nationality"].ToString() +"</td><td>&nbsp;</td> </tr>");
                 stringBuilder.Append(" <tr><td>5.</td><td>Date of Birth:</td> <td>" + row["DOBStr"].ToString() + "</td><td style='color: #999;'><i>(Change in D.O.B. is NOT admissible)</i></td></tr>");
                 stringBuilder.Append("<tr><td>6.</td><td>Blood Group:</td><td>" + row["BloodGroup"].ToString() + "</td><td style='color: #999;'><i>(attach Blood test report, if any correction)</i></td></tr>");
                 stringBuilder.Append("<tr><td>7.</td><td>Religion:</td><td>" + row["Religion"].ToString() + "</td><td>&nbsp;</td></tr>");
                 stringBuilder.Append("<tr><td>8.</td><td>Category:</td><td>" + row["CatName"].ToString() + "</td><td>&nbsp;</td></tr>");
                 stringBuilder.Append("<tr><td>9.</td><td>Aadhaar No.:</td><td>" + row["StudAdhar"].ToString() + "</td><td>&nbsp;</td>  </tr>");
                 stringBuilder.Append(" <tr><td>10.</td><td>Father’s Name:</td><td>" + row["FatherName"].ToString()+"</td> <td>&nbsp;</td></tr>");
                 stringBuilder.Append("<tr> <td>11.</td>  <td>Mother’s Name:</td> <td>"+ row["MotherName"].ToString() +"</td><td>&nbsp;</td> </tr>");
                 stringBuilder.Append("<tr> <td>12.</td> <td>Present Address:</td><td>" + row["PresAddr1"].ToString() + " , " + row["PresAddrCity"].ToString() + "</td><td>&nbsp;</td></tr>");
                 stringBuilder.Append("<tr><td>13.</td> <td>Phone (R):</td><td>" + row["TelNoResidence"].ToString() + "</td><td>&nbsp;</td></tr>");
                 stringBuilder.Append("<tr> <td>14.</td> <td>Phone (O):</td> <td>" + row["TeleNoOffice"].ToString() + "</td> <td>&nbsp;</td></tr>");
                 stringBuilder.Append("<tr><td>15.</td><td>Mobile (1) (for sms alert):</td><td>" + row["MobileNo1"].ToString() + "</td><td>&nbsp;</td></tr>");
                 stringBuilder.Append("<tr><td>16.</td><td>Mobile (2):</td><td>" + row["MobileNo2"].ToString() + "</td><td>&nbsp;</td></tr></table>");
                 stringBuilder.Append("<table width='100%' border='0' cellpadding='0' cellspacing='0' style='margin-top:10px;'> <tr><td style='border:1px solid #333; height:10px; width:20px;'>&nbsp;</td> <td>&nbsp;</td><td colspan='2'>(Tick &#10004;)  All data in ‘SCHOOL RECORD’ is correct.</td></tr>");
                 stringBuilder.Append("<tr><td style='border:1px solid #333; height:10px; width:20px;'>&nbsp;</td><td>&nbsp;</td><td colspan='2'>(Tick &#10004)  Corrections are made in the ‘CORRECT DATA’ column.</td></tr> ");
                 stringBuilder.Append("<tr><td colspan='4' align='right'>___________________<br />(Parents’ Signature)</td></tr> <tr height='50'><td colspan='4' style='border-bottom:1px #333 solid;'>&nbsp;</td></tr></table>");
                 
             }
             lblReport.Text = stringBuilder.ToString();
             Session["DetailsVerify"] = stringBuilder.ToString();

         }
         else
         {
             btnPrint.Enabled = false;
             btnPrint.Visible = false;
         }
    }
}