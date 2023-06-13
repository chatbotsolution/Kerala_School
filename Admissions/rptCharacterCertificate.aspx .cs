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

public partial class Admissions_rptCharacterCertificate : System.Web.UI.Page
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
        DataSet dataset2 = new DataSet();
        string slno = "";
        dataset2 = clsDal.GetDatasetQry("select * from CharcterCertificate where AdmnNo="+ Convert.ToInt32(drpstudents.SelectedValue) + " select max(SlNo)+1 from CharcterCertificate");
        if (dataset2.Tables[0].Rows.Count > 0)
            slno = dataset2.Tables[0].Rows[0]["SlNo"].ToString();
        else if (dataset2.Tables[1].Rows[0][0].ToString() != "")
        {
            slno = dataset2.Tables[1].Rows[0][0].ToString();
            clsDal.ExcuteQryInsUpdt("insert into CharcterCertificate values(" + Convert.ToInt32(drpstudents.SelectedValue) + "," + Convert.ToInt32(slno) + ")");
        }
        else
        {
            slno = "1652";
            clsDal.ExcuteQryInsUpdt("insert into CharcterCertificate values(" + Convert.ToInt32(drpstudents.SelectedValue) + "," + Convert.ToInt32(slno) + ")");
        }
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
                    stringBuilder.Append("<table width='100%' cellpadding='2' cellspacing='2' class='tbltd'><tr><td rowspan='5'> <img src='../images/logo-new.png'  width='120px' height='100px'/></td>");
                    stringBuilder.Append("<td align='center' style='font-size:20px; white-space:nowrap;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td><td rowspan='4'> <img src='../images/rightlogo.jpg' width='85px' height='80px'/ ></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>AFFILIATED TO C.B.S.E NEW DELHI</b></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper() +","+ clsDal.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() +"</b></td></tr>");
                    //stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2).ToUpper() + ":-" + clsDal.Decrypt(row["Pin"].ToString().Trim(), str2) + "</b> </td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>Tele-" + clsDal.Decrypt(row["Phone"].ToString().Trim(), str2) + "</b></td></tr>");
                    //stringBuilder.Append("<tr><td><td align='right' style='font-size:17px;'><b>School Code No: 15399</b></td></td> </tr>");                   
                    stringBuilder.Append(" <tr><td colspan='4' ></td></tr>");
                    stringBuilder.Append("<tr><td align='left' style='font-size:15px;'><b>Affl No. 1530109</b></td></tr> ");
                    stringBuilder.Append("<tr><td align='left'style='font-size:15px; margin-left:10px;'><b>Sl No. "+slno+"</b></td> ");
                    stringBuilder.Append("<td><td align='right' style='font-size:17px;'><b width:100px;>School Code No: 15399</b></td></td></tr>");
                    
                 
                    stringBuilder.Append("<tr><td colspan='4' align='center' style='font-size:20px;'><strong><u>CHARACTER&nbsp;&nbsp;CERTIFICATE</u></strong></td></tr><br/>");
                    try
                    {
                        //if (WebConfigurationManager.AppSettings["SchoolReg"].ToString().Trim() != "")
                        //    stringBuilder.Append("<tr><td colspan='5' align='center' style='font-size:14px'><strong>School Registration No: " + WebConfigurationManager.AppSettings["SchoolReg"].ToString().Trim() + "</strong></td></tr>");
                        //else
                        //    stringBuilder.Append("<tr><td colspan='5'></td></tr>");
                    }
                    catch
                    {
                        stringBuilder.Append("<tr><td colspan='5'></td></tr>");
                    }
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
        string gender1 = "";
        string gender2 = "";
        string gender3 = "";
        string gender4 = "";
        string Mas_ms = "";
        string dob = "";
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
                if (row["Sex"].ToString() == "Male")
                {
                    gender1 = "Son of";
                    gender2 = "He";
                    gender3 = "His";
                    gender4 = "Him";
                    Mas_ms = "Master";
                }
                else
                {
                    gender1 = "Daughter of";
                    gender2 = "She";
                    gender3 = "Her";
                    gender4 = "Her";
                    Mas_ms = "Miss";
                }
                dob =Convert.ToDateTime( row["DOB"]).ToString("dd/MM/yyyy");
             //   dob=Convert.ToDateTime (dt.Rows[0]["DOB"]).ToString("dd/MM/yyyy");
               string str = Information();
                stringBuilder.Append(str);
                stringBuilder.Append("<table width='100%' >");
                stringBuilder.Append("<tr><td colspan='2' style='text-align:justify; font-size:28px; font-family:Calibri;'>");
                //stringBuilder.Append("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; This is to certify that  <b><i>&nbsp;" + row["FullName"].ToString() + ",</i></b>&nbsp;S.o/D.o.<b><i>&nbsp;" + row["FatherName"].ToString() + "</i></b>&nbsp;");
                //stringBuilder.Append("<b><i>" + row["PresAddr1"].ToString() + " , " + row["PresAddrCity"].ToString() + "</i></b>&nbsp;was a bonafied student of the school from&nbsp;<b><i>" + txtAdmYr.Text.ToString().Trim() + " </i></b>&nbsp;to&nbsp;");
                //stringBuilder.Append("<b><i>" + txtPassYr.Text.ToString().Trim() + "</i></b>.&nbsp;.He/She has passed the <b><i>" + txtExamType.Text.ToString().Trim() + "</i></b>&nbsp;&nbsp;Examination&nbsp;<b><i>" + txtExamYr.Text.ToString().Trim() + "</i></b>&nbsp; in <b><i>" + txtPassDiv.Text.ToString().Trim() + "</i></b>&nbsp; Division. To the best of my ");
                //stringBuilder.Append("knowledge he/she bears a good moral character and I know nothing against him/her.");
                //stringBuilder.Append("I wish him/her all success in every sphere of future life.");

                stringBuilder.Append("Certified that Master/Miss <b>" + row["FullName"].ToString() + "</b> ");
                stringBuilder.Append("Son/Daughter of Mr./Mrs.<b> "+ row["FatherName"].ToString() + "</b>");
                stringBuilder.Append(" is/was a bonafide student of this school during the session "+drpSession.SelectedValue+".<br/> ");
                //stringBuilder.Append("<b>" + TxtCertificate.Text.Trim() + ".</b>");
                stringBuilder.Append( "His/Her Date of Birth is <b>" + dob + "</b> as recorded in the admission register .");
                stringBuilder.Append("</br>");
                stringBuilder.Append("</br>");
                stringBuilder.Append("</br>");
                //stringBuilder.Append("<tr><td>&nbsp</td></tr>");
                //stringBuilder.Append("<tr><td>&nbsp</td></tr>");
                //stringBuilder.Append("<tr><td>&nbsp</td></tr>");
                stringBuilder.Append( "He/She bears a good moral character.</br>");
                stringBuilder.Append("I wish him/her all success in life.");
                stringBuilder.Append("</td></tr>");
                
                
                stringBuilder.Append("<tr><td width='200px' style='font-size:28px; font-family:Calibri;'>Date: " + date + "</td>");
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