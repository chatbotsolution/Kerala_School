using ASP;
using Classes.DA;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admissions_rptConductCertificate : System.Web.UI.Page
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
        ViewState["AdmnSessYr"] = null;
        txtadminno.Text = "0";
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

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSection.SelectedIndex > 0)
        {
            fillstudent();
            lblReport.Text = "";
            btnPrint.Visible = false;
            txtadminno.Text = "0";
        }
        else
        {
            drpstudents.SelectedIndex = 0;
            txtadminno.Text = "0";
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
        {
            ViewState["AdmnSessYr"] = dataTable.Rows[0]["AdmnSessYr"];
            drpstudents.Items.Insert(0, new ListItem("--ALL--", "0"));
        }
        else
        {
            ViewState["AdmnSessYr"] = null;
            drpstudents.Items.Insert(0, new ListItem("No Record", "0"));
        }
    }

    protected void drpstudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = new clsDAL().GetDataTableQry("select OldAdmnno from Ps_StudMaster where Admnno =" + drpstudents.SelectedValue + "");
        //int admno = Convert.ToInt32(dt.Rows[0]["Admnno"].ToString().Trim());
        if (drpstudents.SelectedIndex > 0)
        {

            txtadminno.Text = dt.Rows[0]["OldAdmnno"].ToString().Trim();
            txtAdmYr.Text = ViewState["AdmnSessYr"].ToString();
            lblReport.Text = "";
            btnPrint.Visible = false;
        }
        else
        {
            txtAdmYr.Text = string.Empty;
            txtadminno.Text = "0";
            lblReport.Text = "";
            btnPrint.Visible = false;
        }
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
            txtadminno.Text = "0";
        }
        else
        {
            drpSection.SelectedIndex = 0;
            drpstudents.SelectedIndex = 0;
            txtadminno.Text = "0";
            lblReport.Text = "";
            btnPrint.Visible = false;
        }
    }

    private void FillStudent()
    {
        try
        {
            lblReport.Text = "";
            btnPrint.Visible = false;
            Common common = new Common();
            if (Convert.ToInt32(common.ExecuteScalarQry("select count(*) from PS_StudMaster where AdmnNo=" + txtadminno.Text)) == 0)
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Admission no. does not exists')", true);
            }
            else
            {
                DataTable dataTable = common.ExecuteSql("select SessionYear,section,ClassID from PS_ClasswiseStudent where Detained_Promoted='' and  admnno=" + txtadminno.Text);
                drpSession.SelectedValue = dataTable.Rows[0]["SessionYear"].ToString();
                fillclass();
                drpclass.SelectedValue = dataTable.Rows[0]["ClassID"].ToString();
                FillClassSection();
                drpSection.SelectedValue = dataTable.Rows[0]["section"].ToString();
                fillstudent();
                drpstudents.SelectedValue = txtadminno.Text;
            }
        }
        catch (Exception ex)
        {
            if (!(ex.Message != ""))
                return;
            ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Invalid Data')", true);
        }
    }

    private string Information()
    {
        Common common = new Common();
        DataTable dataTable1 = new DataTable();
        DataSet dataSet = new DataSet();
        StringBuilder stringBuilder = new StringBuilder();
        clsDAL clsDal = new clsDAL();
        string str1 = Server.MapPath("../XMLFiles") + "\\Detail.xml";
        if (File.Exists(str1))
        {
            int num = (int)dataSet.ReadXml(str1);
            string str2 = Session["SSVMID"].ToString();
            if (dataSet.Tables.Count > 0)
            {
                DataTable dataTable2 = new DataTable();
                DataTable table = dataSet.Tables[0];
                foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    stringBuilder.Append("<table width='85%' cellpadding='2' cellspacing='2' class='tbltd'><tr><td rowspan='5'> <img src='../images/logo-new.png' width='75px' height='80px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/ ></td></tr>");

                    stringBuilder.Append("<tr><td align='center' style='font-size:16px;'><b> AFFILATED TO C.B.S.E.,NEW DELHI </b></td></tr>");

                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() + "</b></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + clsDal.Decrypt(row["Pin"].ToString().Trim(), str2) + "</b> </td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>Phone-" + clsDal.Decrypt(row["Phone"].ToString().Trim(), str2) + "</b></td></tr>");
                    stringBuilder.Append(" <tr><td colspan='4' ></td></tr>");
                    stringBuilder.Append("<tr><td colspan='4' align='center' style='font-size:20px;'><strong><u>CONDUCT&nbsp;&nbsp;CERTIFICATE</u></strong></td></tr><br/>");
                    //try
                    //{
                    //    if (WebConfigurationManager.AppSettings["SchoolReg"].ToString().Trim() != "")
                    //        stringBuilder.Append("<tr><td colspan='5' align='center' style='font-size:14px'><strong>School Registration No: " + WebConfigurationManager.AppSettings["SchoolReg"].ToString().Trim() + "</strong></td></tr>");
                    //    else
                    //        stringBuilder.Append("<tr><td colspan='5'></td></tr>");
                    //}
                    //catch
                    //{
                    //    stringBuilder.Append("<tr><td colspan='5'></td></tr>");
                    //}
                    stringBuilder.Append("</table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptConductCertificatePrint.aspx");
    }

    protected void drpForCls_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
    }

    private void getdata()
    {
        string gender1="";
        string gender2="";
         string gender3="";
         string date = DateTime.Now.ToString("dd-MM-yyyy");
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
        DataTable dataTable2 = clsDal.GetDataTable("GetConductCertificate", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            btnPrint.Enabled = true;
            btnPrint.Visible = true;
            StringBuilder stringBuilder = new StringBuilder();
           foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
               if( row["Sex"].ToString()== "Male")
               {
                   gender1= "Son of";
                   gender2= "He";
                    gender3= "His";
               }
               else
               {
                   gender1= "Daughter of";
                   gender2= "She";
                    gender3= "Her";
               }

                string str = Information();
                stringBuilder.Append(str);
                stringBuilder.Append("<table width='100%' >");
                stringBuilder.Append("<tr><td colspan='2' style='text-align:justify; font-size:28px; font-family:Calibri;'>");
                //stringBuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; This is to certify that  <b><i>&nbsp;" + row["FullName"].ToString() + ",</i></b>&nbsp;S.o/D.o.<b><i>&nbsp;" + row["FatherName"].ToString() + "</i></b>&nbsp;");
                //stringBuilder.Append("<b><i>" + row["PresAddr1"].ToString() + " , " + row["PresAddrCity"].ToString() + "</i></b>&nbsp;was a bonafied student of the school from&nbsp;<b><i>" + txtAdmYr.Text.ToString().Trim() + " </i></b>&nbsp;to&nbsp;");
                //stringBuilder.Append("<b><i>" + txtPassYr.Text.ToString().Trim() + "</i></b>.&nbsp;.He/She has passed the <b><i>" + txtExamType.Text.ToString().Trim() + "</i></b>&nbsp;&nbsp;Examination&nbsp;<b><i>" + txtExamYr.Text.ToString().Trim() + "</i></b>&nbsp; in <b><i>" + txtPassDiv.Text.ToString().Trim() + "</i></b>&nbsp; Division. To the best of my ");
                //stringBuilder.Append("knowledge he/she bears a good moral character and I know nothing against him/her.");
                //stringBuilder.Append("I wish him/her all success in every sphere of future life.");

                stringBuilder.Append("This is to certify that   <b>" + row["FullName"].ToString() + "</b> <br/>");
                stringBuilder.Append(" " + gender1 + " Mr. <b> " + row["FatherName"].ToString() + "</b> and Mrs. <b> " + row["MotherName"].ToString() + " </b></br>");
                stringBuilder.Append(" was a bonafide student of this school.<br/> " + gender2 + " is awarded ");
                stringBuilder.Append("<b>" + TxtCertificate.Text.Trim() + "</b>.<br/><br/>");
                stringBuilder.Append("" + gender3 + " conduct was <b>" + TxtConduct.Text.Trim() + ".</b><br/><br/><br/>");
                stringBuilder.Append("</td></tr>");
                stringBuilder.Append("<tr><td width='200px' style='font-size:28px; font-family:Calibri;'><br /><br/>Date: " + date + "</td>");
                stringBuilder.Append("<td align='right' class='txtconduct' style='height: 50px; width:180px;font-size:28px; font-family:Calibri;' valign='bottom'><br/><strong>Principal</strong></td>");
                stringBuilder.Append("</td></tr>");
                stringBuilder.Append("</table>");
            }
            lblReport.Text = stringBuilder.ToString();
            Session["conductcertificateprint"] = stringBuilder.ToString();
        }
        else
        {
            btnPrint.Enabled = false;
            btnPrint.Visible = false;
        }
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    protected void btnGenerate_Click(object sender, EventArgs e)
    {
        getdata();
    }
}