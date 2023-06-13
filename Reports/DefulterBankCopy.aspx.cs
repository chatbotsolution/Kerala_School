using ASP;
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

public partial class Reports_DefulterBankCopy : System.Web.UI.Page
{
     private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        fillclass();
        fillSession();
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
    }
    private void fillclass()
    {
        drpclass.Items.Clear();
        drpclass.DataSource = obj.GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("--All--", "0"));
    }

    private void fillSession()
    {
        DataTable dataTable = new DataTable();
        drpSession.DataSource = obj.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
        DateTime Transdate = DateTime.Now;
      //  DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Session", drpSession.SelectedValue.ToString().Trim());
        if (drpclass.SelectedIndex >0)
               hashtable.Add("@ClassID", drpclass.SelectedValue.ToString().Trim());
        if (drpQuarter.SelectedIndex > 0)
        {
            FeeDetails fd = new FeeDetails();
            if (drpQuarter.SelectedValue == "1")
            {
               
               Transdate= fd.getDuedate("APR-JUN", drpSession.SelectedValue.ToString().Trim());
            }
            else if (drpQuarter.SelectedValue == "2")
            {
               // hashtable.Add("@Quarter", "JUL-SEP");
                Transdate= fd.getDuedate("JUL-SEP", drpSession.SelectedValue.ToString().Trim());
            }
            else if (drpQuarter.SelectedValue == "3")
            {
               // hashtable.Add("@Quarter", "OCT-DEC");
                 Transdate=  fd.getDuedate("OCT-DEC", drpSession.SelectedValue.ToString().Trim());
            }
            else
            {
               // hashtable.Add("@Quarter", "JAN-MAR");
              Transdate=   fd.getDuedate("JAN-MAR", drpSession.SelectedValue.ToString().Trim());
            }
        }
        hashtable.Add("@TransDate", Transdate);
        // DataTable dt = drpclass.SelectedIndex <= 0 ? obj.GetDataTable("NewGetAllQtrlyFeeStatus", hashtable) : obj.GetDataTable("NewQuarterlyFeeStatus", hashtable);
        DataTable dt = new DataTable();
        DataSet ds = new DataSet();
        dt = obj.GetDataTable("NewDefaulterList", hashtable);
        if( dt.Rows.Count >0 )
        {
            dt.Columns.Add("GeneralFees");
            dt.Columns.Add("Library");
            dt.Columns.Add("Maintainance");
            dt.Columns.Add("SocialServiceFee");
            dt.Columns.Add("ScienceLab");
            dt.Columns.Add("Stationary&Examinations");
            dt.Columns.Add("TuitionFee");
            dt.Columns.Add("ComputerFee");
            dt.Columns.Add("SmartClassFee");
            dt.Columns.Add("EnglishLanguageLab");
            dt.Columns.Add("Activities");
            for (int i=0; i < dt.Rows.Count; i++)
            {
                Hashtable ht = new Hashtable();
                ht.Add("@Session", drpSession.SelectedValue.ToString().Trim());
                ht.Add("@ClassId", dt.Rows[i]["ClassID"].ToString().Trim());
                ht.Add("@TransDate", Transdate);
                ht.Add("@AdmnNo", dt.Rows[i]["AdmnNo"].ToString().Trim());
                DataTable dt2 = obj.GetDataTable("NewGetCompAmtByAdmnNo", ht);
                 for (int j = 0; j < dt2.Rows.Count; j++)
                 {
                            if(drpQuarter.SelectedValue.ToString() !="1")
                            {
                                 dt.Rows[i]["GeneralFees"]="0";
                                 dt.Rows[i]["Library"]="0";
                                 dt.Rows[i]["Maintainance"]="0";
                                 dt.Rows[i]["SocialServiceFee"]="0";
                                 dt.Rows[i]["ScienceLab"]="0";
                                 dt.Rows[i]["Stationary&Examinations"]="0";
                                 SwitchCase(dt2.Rows[j]["FeeName"].ToString(),dt.Rows[i],dt2.Rows[j]);
                            
                            }
                            else
                                SwitchCase(dt2.Rows[j]["FeeName"].ToString(),dt.Rows[i],dt2.Rows[j]);
                 }
            }


                GenerateReport(dt);
                btnPrint.Enabled = true;
                btnExpExcel.Enabled = true;

        }

               
            
            else
            {
                lblReport.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='center'>No Records Found  !</div>";
                btnPrint.Enabled = false;
                btnExpExcel.Enabled = false;
            }
        }

    private void SwitchCase(string FeeName,DataRow dataRow,DataRow dataRow_2)
{
 	 switch (FeeName)
        {
               case "General Fees":
                               dataRow["GeneralFees"] =dataRow_2["Amount"].ToString();
                               break;
               case "Library":
                               dataRow["Library"] =dataRow_2["Amount"].ToString();
                                break;
               case "Maintainance":
                              dataRow["Maintainance"] =dataRow_2["Amount"].ToString();
                                 break;
              case "Social Service Fee":
                             dataRow["SocialServiceFee"] =dataRow_2["Amount"].ToString();
                                break;
              case "Science Lab":
                               dataRow["ScienceLab"] = dataRow_2["Amount"].ToString();
                                break;
              case "Stationary & Examinations":
                            dataRow["Stationary&Examinations"] =dataRow_2["Amount"].ToString();
                               break;
              case "Tuition Fee":
                              dataRow["TuitionFee"] =dataRow_2["Amount"].ToString();
                                break;
               case "Computer Fee":
                             dataRow["ComputerFee"] =dataRow_2["Amount"].ToString();
                                break;
               case "Smart Class Fee":
                             dataRow["SmartClassFee"] = dataRow_2["Amount"].ToString();
                                break;
               case "English Language Lab":
                              dataRow["EnglishLanguageLab"] =dataRow_2["Amount"].ToString();
                                break;
                 case "Activities":
                              dataRow["Activities"] = dataRow_2["Amount"].ToString();
                                 break;
                 default:
                                  break;
            }
}

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Response.Redirect("DefaulterBankCopyPrint.aspx");
    }
    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td align='left' colspan='14'><b>");
                stringBuilder.Append("Defaulter For Bank :-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["BankCopy"].ToString()).ToString().Trim());
            }
            else
                Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
    private void GenerateReport(DataTable dt)
    {
        StringBuilder stringBuilder1 = new StringBuilder("");
        stringBuilder1.Append("<table border='1px' cellpadding='0' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;'   width='100%'>");
        if (drpclass.SelectedIndex > 0)
            stringBuilder1.Append("<tr style='background-color:#CCCCCC'><td colspan='20' align='center'><b>Defaulter List of Class-" + drpclass.SelectedItem.ToString() + " For The Session Year " + drpSession.SelectedItem.ToString() + " ( " + drpQuarter.SelectedItem.ToString() + " ) </b></td></tr>");
        else
            stringBuilder1.Append("<tr style='background-color:#CCCCCC'><td colspan='18' align='center'><b>Defaulter List for all Classes For The Session Year " + drpSession.SelectedItem.ToString() + " ( " + drpQuarter.SelectedItem.ToString() + " ) </b></td></tr>");
        stringBuilder1.Append("<tr style='background-color:#eee'>");
       // stringBuilder1.Append("<td style='width: 40px' align='center'><strong>Sl. No.</strong></td>");
        stringBuilder1.Append("<td style='width: 30px' align='center'><strong>Default</strong></td>");
        stringBuilder1.Append("<td style='width: 100px' align='center'><strong>Financial Year</strong></td>");
        stringBuilder1.Append("<td align='left'><strong>Name</strong></td>");
        stringBuilder1.Append("<td style='width: 100px' align='right'><strong>Adm. No</strong></td>");
        stringBuilder1.Append("<td style='width: 100px;' align='left'><strong>Class</strong></td>");
        stringBuilder1.Append("<td style='width: 50px' align='right'><strong>Section</strong></td>");
        stringBuilder1.Append("<td style='width: 100px' align='center'><strong>Quarter</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>General Fees</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Library</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Maintenance</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Social Service Scheme</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Science Lab</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Stationery & Examinations</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Tuition</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Computer</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Smart Class</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>English Lag Lab</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Activities</strong></td>");
        stringBuilder1.Append("<td style='width: 90px' align='right'><strong>Late fees</strong></td>");
        stringBuilder1.Append("</tr>");
        int num1 = 1;
      
      
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder1.Append("<tr>");
            // stringBuilder1.Append("<td align='center'>" + num1.ToString() + "</td>");
            stringBuilder1.Append("<td align='center'> ICM </td>");
                stringBuilder1.Append("<td align='right'>");
                //stringBuilder1.Append(row["SessionYear"].ToString().Trim());
                stringBuilder1.Append(drpSession.SelectedItem.Text.Trim());
                stringBuilder1.Append("</td>");
            //----------
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["FullName"].ToString().Trim());
                stringBuilder1.Append("</td>");
            //--------------
                stringBuilder1.Append("<td align='right'>");
                stringBuilder1.Append(row["OldAdmnno"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["ClassName"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='left'>");
                stringBuilder1.Append(row["Section"].ToString().Trim());
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='right'>");
                stringBuilder1.Append(drpQuarter.SelectedItem.Text.Trim());
                stringBuilder1.Append("</td>"); 
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(CheckFeeEmptyOrNot(row["GeneralFees"].ToString().Trim()));
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(CheckFeeEmptyOrNot(row["Library"].ToString().Trim()));
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(CheckFeeEmptyOrNot(row["Maintainance"].ToString().Trim()));
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(CheckFeeEmptyOrNot(row["SocialServiceFee"].ToString().Trim()));
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(CheckFeeEmptyOrNot(row["ScienceLab"].ToString().Trim()));
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(CheckFeeEmptyOrNot(row["Stationary&Examinations"].ToString().Trim()));
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(CheckFeeEmptyOrNot(row["TuitionFee"].ToString().Trim()));
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(CheckFeeEmptyOrNot(row["ComputerFee"].ToString().Trim()));
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(CheckFeeEmptyOrNot(row["SmartClassFee"].ToString().Trim()));
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(CheckFeeEmptyOrNot(row["EnglishLanguageLab"].ToString().Trim()));
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append(CheckFeeEmptyOrNot(row["Activities"].ToString().Trim()));
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("<td align='center'>");
                stringBuilder1.Append("0");
                stringBuilder1.Append("</td>");
                stringBuilder1.Append("</tr>");
                num1 = num1 + 1;
            }
           
            
     
        stringBuilder1.Append("</table>");
        lblReport.Text = stringBuilder1.ToString().Trim();
        Session["BankCopy"] = stringBuilder1.ToString();
    }

    private double CheckFeeEmptyOrNot(string Fee)
    {
        if (Fee != string.Empty)
            return Convert.ToDouble(Fee);
        else
            return 0;
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/DefaulterBankCopy_" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + str);
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}