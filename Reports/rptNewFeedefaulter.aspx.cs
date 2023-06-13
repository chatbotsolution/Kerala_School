using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SanLib;
using Classes.DA;
using System.Data;
using System.Text;
using System.IO;

public partial class Reports_rptNewFeedefaulter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        FillSession();
        FillClass();
        FillClassSection();
        fillstudent();
    }
    protected void FillSession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }
    private void FillClass()
    {
        drpClass.Items.Clear();
        drpClass.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpClass.DataTextField = "classname";
        drpClass.DataValueField = "classid";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("ALL", "0"));
    }

    private void FillClassSection()
    {
        drpSection.Items.Clear();
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassWiseSections", new Hashtable()
    {
      {
         "@classId",
         drpClass.SelectedValue.ToString().Trim()
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("All", "0"));
    }
    private void fillstudent()
    {
        drpstudents.Items.Clear();
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        DataTable dataTable = common.GetDataTable("ps_sp_get_StudNamesForFeeColRpt", hashtable);
        drpstudents.DataSource = dataTable;
        drpstudents.DataTextField = "fullname";
        drpstudents.DataValueField = "admnno";
        drpstudents.DataBind();
        if (dataTable.Rows.Count > 0)
            drpstudents.Items.Insert(0, new ListItem("--ALL--", "0"));
        else
            drpstudents.Items.Insert(0, new ListItem("No Record", "0"));
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptNewDefaulterPrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
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
                stringBuilder.Append("Fee Defaulter Report : ");
                
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(lblReport.Text.ToString().Trim());
                stringBuilder.Append("<table><tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel(stringBuilder.ToString());
            }
            else
                Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
    protected void btnShow_Click(object sender, EventArgs e)
    
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Session", drpSession.SelectedValue.Trim());
        if(drpClass.SelectedIndex >0)
             hashtable.Add("@ClassId", drpClass.SelectedValue.Trim());
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue.Trim());

        if (drpstudents.SelectedIndex > 0)
            hashtable.Add("@Admnno", drpstudents.SelectedValue.Trim());

        Getdefaulter(hashtable);

    }

    private void Getdefaulter(Hashtable hashtable)
    {
        DataSet ds = new clsDAL().GetDataSet("NewFeedefaulter", hashtable);
        ds.Tables[0].Columns.Add("SecondQtr");
        ds.Tables[0].Columns.Add("ThirdQtr");
        ds.Tables[0].Columns.Add("FourthQtr");
        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
        {
            ds.Tables[0].Rows[i]["SecondQtr"] = ds.Tables[1].Rows[i]["SecondQtr"].ToString();
        }
        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
        {
            ds.Tables[0].Rows[i]["ThirdQtr"] = ds.Tables[2].Rows[i]["ThirdQtr"].ToString();
        }
        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
        {
            ds.Tables[0].Rows[i]["FourthQtr"] = ds.Tables[3].Rows[i]["FourthQtr"].ToString();
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            GenerateReport(ds.Tables[0]);
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
    protected void drpstudents_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpstudents.SelectedIndex > 0)
        {
            lblReport.Text = "";
            txtadminno.Text = drpstudents.SelectedValue;
        }
        else
        {
            txtadminno.Text = "";
            lblReport.Text = "";
        }
    }
    protected void drpSection_SelectedIndexChanged1(object sender, EventArgs e)
    {
        lblReport.Text = "";
        fillstudent();
    }
    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpClass.SelectedIndex > 0)
        {
            lblReport.Text = "";
            drpSection.SelectedIndex = -1;
            drpstudents.SelectedIndex = -1;
            txtadminno.Text = "";
            FillClassSection();
            fillstudent();
        }
        else
            lblReport.Text = "";
    }
    protected bool ValidAdmn()
    {
        if (!(txtadminno.Text.Trim() != ""))
            return true;
        try
        {
            btnPrint.Enabled = false;
            Common common = new Common();

            int admno = Convert.ToInt32(common.ExecuteScalarQry("Select AdmnNo from Ps_Studmaster where OldAdmnNo='" + txtadminno.Text + "'"));
            //if (dt.Rows.Count > 0)
            //{
            // int admno = Convert.ToInt32(dt.Rows[0]["AdmnNo"].ToString());

            if (Convert.ToInt32(common.ExecuteScalarQry("select count(*) from PS_StudMaster where AdmnNo=" + admno)) == 0)
            {
                lblReport.Text = "";
                drpSection.SelectedIndex = -1;
                drpstudents.SelectedIndex = -1;
                ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Admission no. does not exists')", true);
                return false;
            }
            FillClass();
            FillClassSection();
            fillstudent();
            drpstudents.SelectedValue = admno.ToString();
            DataTable dataTable = common.ExecuteSql("select section,ClassID from PS_ClasswiseStudent where admnno=" + admno + " and Detained_Promoted='' ");
            FillClass();
            drpClass.SelectedValue = dataTable.Rows[0]["ClassID"].ToString();
            FillClassSection();
            drpSection.SelectedValue = dataTable.Rows[0]["section"].ToString();
            return true;
        }
        //else
        //    ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Admission no. does not exists')", true);
        //    return false;
        //}
        catch (Exception ex)
        {
            if (!(ex.Message != ""))
                return false;
            ScriptManager.RegisterClientScriptBlock((Control)txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Invalid Data')", true);
            return false;
        }
    }
    private void GenerateReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table id='reporttable' border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
        stringBuilder.Append("<tr  style='background-color:#CCCCCC'>");
        stringBuilder.Append("<td style='width: 30px' align='left'><strong>Sl.No</strong></td>");
        stringBuilder.Append("<td style='width: 30px' align='left'><strong>Roll.No</strong></td>");
        stringBuilder.Append("<td style='width: 60px' align='left'>Admission No</td>");
        stringBuilder.Append("<td style='width: 150px' align='left'>Name</td>");
        stringBuilder.Append("<td style='width: 40px' align='left'>Class</td>");
        stringBuilder.Append("<td style='width: 30px' align='left'>Section</td>");
        stringBuilder.Append("<td style='width: 30px' align='left'>Status</td>");
        stringBuilder.Append("<td style='width: 100px' align='left'>MobileNo</td>");
        stringBuilder.Append("<td style='width: 60px' align='left'>1stQuarter</td>");
        stringBuilder.Append("<td style='width: 60px' align='left'>2ndQuarter</td>");
        stringBuilder.Append("<td style='width: 60px' align='left'>3rdQuarter</td>");
        stringBuilder.Append("<td style='width: 60px' align='left'>4thQuarter</td>");
       
        stringBuilder.Append("</tr>");
       // double num1 = 0.0;
        int num2 = 1;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='center'>" + num2.ToString() + "</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["RollNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["OldAdmnno"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["ClassName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Section"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["Status"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["MobileNo1"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["FirstQtr"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["SecondQtr"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["ThirdQtr"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right'>");
            stringBuilder.Append(row["FourthQtr"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
           // num1 += Convert.ToDouble(row["Amount"].ToString().Trim());
            ++num2;
        }
        //stringBuilder.Append("<tr>");
        //stringBuilder.Append("<td colspan='8'  align='right'>");
        //stringBuilder.Append("<strong class='error'>Grand Total &nbsp;:&nbsp;</strong>");
        //stringBuilder.Append("</td>");
        //stringBuilder.Append("<td align='right'>");
        //stringBuilder.Append("<strong class='error'>" + string.Format("{0:F2}", num1));
        //stringBuilder.Append("</strong></td>");
        //stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["NewDefaulter"] = stringBuilder.ToString().Trim();
    }
    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/NewDeafulter" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
    protected void BtnSearch_Click(object sender, EventArgs e)
    {       
        if (txtadminno.Text.Trim() == string.Empty)
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Search Box Can't be blank";
            return;
        }
        else
        {
            int admnno ;
            clsDAL clsdal = new clsDAL();        

            DataTable newdt = clsdal.GetDataTableQry("Select AdmnNo from Ps_StudMaster where OldAdmnNo='" + txtadminno.Text.Trim() + "'");
                if (newdt.Rows.Count > 0)
                {
                    admnno = Convert.ToInt32(newdt.Rows[0]["AdmnNo"].ToString().Trim());
                    Hashtable hashtable = new Hashtable();
                    hashtable.Add("@Session", drpSession.SelectedValue.Trim());
                    if (drpClass.SelectedIndex > 0)
                        hashtable.Add("@ClassId", drpClass.SelectedValue.Trim());
                    if (drpSection.SelectedIndex > 0)
                        hashtable.Add("@Section", drpSection.SelectedValue.Trim());
                  
                        hashtable.Add("@Admnno", admnno);
                    Getdefaulter(hashtable);
                }

                lblMsg.Visible = false;
        }
    }

}