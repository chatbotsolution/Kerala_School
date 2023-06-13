using ASP;
using Classes.DA;
using RJS.Web.WebControl;
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

public partial class Reports_rptFeeReceived : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
  {
    if (Session["User_Id"] != null)
    {
      if (Page.IsPostBack)
        return;
      fillsession();
      fillclass();
      FillClassSection();
      fillstudent();
      txtadminno.Text = "0";
      btnprint.Visible = false;
      btnExpExcel.Visible = false;
    }
    else
      Response.Redirect("~/Login.aspx");
  }

  protected void fillsession()
  {
    drpSession.DataSource =  new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID ASC");
    drpSession.DataTextField = "SessionYr";
    drpSession.DataValueField = "SessionYr";
    drpSession.DataBind();
  }

  private void fillclass()
  {
    drpclass.Items.Clear();
    drpclass.DataSource =  new Common().GetDataTable("ps_sp_get_classesForDDL");
    drpclass.DataTextField = "classname";
    drpclass.DataValueField = "classid";
    drpclass.DataBind();
    drpclass.Items.Insert(0, new ListItem("--SELECT--", "0"));
  }

  private void FillClassSection()
  {
    drpSection.Items.Clear();
    drpSection.DataSource =  new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
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
    drpSection.Items.Insert(0, new ListItem("All", "0"));
  }

  private void fillstudent()
  {
    drpstudents.Items.Clear();
    Common common = new Common();
    Hashtable hashtable = new Hashtable();
    if (drpSession.Items.Count > 0)
      hashtable.Add( "@Session",  drpSession.SelectedValue);
    if (drpclass.SelectedValue.ToString().Trim() != "0")
      hashtable.Add( "@Class",  drpclass.SelectedValue);
    if (drpSection.SelectedValue.ToString().Trim() != "0")
      hashtable.Add( "@Section",  drpSection.SelectedValue);
    DataTable dataTable = common.GetDataTable("ps_sp_get_StudNamesForClassSec", hashtable);
    drpstudents.DataSource =  dataTable;
    drpstudents.DataTextField = "fullname";
    drpstudents.DataValueField = "admnno";
    drpstudents.DataBind();
    if (dataTable.Rows.Count > 0)
      drpstudents.Items.Insert(0, new ListItem("--SELECT--", "0"));
    else
      drpstudents.Items.Insert(0, new ListItem("No Record", "0"));
  }

  protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (drpSession.SelectedIndex > 0)
    {
      LblReportClear();
      txtadminno.Text = "0";
      fillclass();
      FillClassSection();
      fillstudent();
    }
    else
      LblReportClear();
  }

  protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (drpclass.SelectedIndex > 0)
    {
      LblReportClear();
      drpSection.SelectedIndex = -1;
      drpstudents.SelectedIndex = -1;
      txtadminno.Text = "0";
      FillClassSection();
      fillstudent();
    }
    else
      LblReportClear();
  }

  protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
  {
    fillstudent();
  }

  protected void drpstudents_SelectedIndexChanged(object sender, EventArgs e)
  {
    if (drpstudents.SelectedIndex > 0)
    {
      LblReportClear();
      txtadminno.Text = drpstudents.SelectedValue;
    }
    else
    {
      txtadminno.Text = "0";
      LblReportClear();
    }
  }

  protected void txtadminno_TextChanged(object sender, EventArgs e)
  {
    try
    {
      lblReport.Text = "";
      btnprint.Visible = false;
      Common common = new Common();
      if (Convert.ToInt32(common.ExecuteScalarQry("select count(*) from PS_StudMaster where AdmnNo=" + txtadminno.Text)) == 0)
      {
        ScriptManager.RegisterClientScriptBlock((Control) txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Admission no. does not exists')", true);
      }
      else
      {
        fillclass();
        fillstudent();
        drpstudents.SelectedValue = txtadminno.Text;
        DataTable dataTable = common.ExecuteSql("select section,ClassID from PS_ClasswiseStudent where admnno=" + txtadminno.Text);
        fillclass();
        drpclass.SelectedValue = dataTable.Rows[0]["ClassID"].ToString();
        FillClassSection();
        drpSection.SelectedValue = dataTable.Rows[0]["section"].ToString();
      }
    }
    catch (Exception ex)
    {
      if (!(ex.Message != ""))
        return;
      ScriptManager.RegisterClientScriptBlock((Control) txtadminno, txtadminno.GetType(), "ShowMessage", "alert('Invalid Data')", true);
    }
  }

  protected void btngo_Click(object sender, EventArgs e)
  {
    GenerateReport();
  }

  private void GenerateReport()
  {
    DataTable dataTable1 = new DataTable();
    clsDAL clsDal = new clsDAL();
    Hashtable hashtable = new Hashtable();
    hashtable.Add( "@SessionYr",  drpSession.SelectedValue.ToString());
    if (drpstudents.SelectedIndex > 0)
      hashtable.Add( "@AdmnNo",  drpstudents.SelectedValue);
    if (!txtFmdate.Text.Trim().Equals(string.Empty))
      hashtable.Add( "@FromDate",  dtpFromDate.GetDateValue());
    if (!txtToDate.Text.Trim().Equals(string.Empty))
      hashtable.Add( "@ToDate",  dtpToDate.GetDateValue());
    DataTable dataTable2 = clsDal.GetDataTable("Ps_Sp_StudWiseFeeReceived_Detail", hashtable);
    if (dataTable2.Rows.Count > 0)
    {
      StringBuilder stringBuilder = new StringBuilder("");
      stringBuilder.Append("<table cellpadding='2' cellspacing='0'  width='100%' class='tbltxt'>");
      stringBuilder.Append("<tr style='background-color: Silver;' class='gridtxt'>");
      stringBuilder.Append("<td align='left'><b>Date</b></td>");
      stringBuilder.Append("<td align='left'><b>Fee Name</b></td>");
      stringBuilder.Append("<td align='left'><b>Receipt No</b></td>");
      stringBuilder.Append("<td style='width: 120px' align='right'><b>Amount</b></td>");
      stringBuilder.Append("</tr>");
      double num = 0.0;
      string str = string.Empty;
      bool flag = false;
      foreach (DataRow row in (InternalDataCollectionBase) dataTable2.Rows)
      {
        if (!str.Equals(row["DisDate"].ToString().Trim()))
        {
          if (flag)
          {
            stringBuilder.Append("<tr style='font-weight:bold;'>");
            stringBuilder.Append("<td colspan='2'>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td colspan='2'>");
            stringBuilder.Append("<hr color='Black'>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr style='font-weight:bold;'>");
            stringBuilder.Append("<td colspan='2'>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append("Total :");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='gridtext'>");
            stringBuilder.Append(string.Format("{0:f2}",  num));
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            num = 0.0;
            stringBuilder.Append("<tr style='font-weight:bold;'>");
            stringBuilder.Append("<td colspan='4'>");
            stringBuilder.Append("<hr color='Black'>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
          }
          if (dataTable2.Rows.IndexOf(row) % 2 == 0)
            stringBuilder.Append("<tr style='background-color: #DFDFDF;'>");
          else
            stringBuilder.Append("<tr>");
          stringBuilder.Append("<td align='left' class='gridtext'>");
          stringBuilder.Append(row["DisDate"].ToString().Trim());
          stringBuilder.Append("</td>");
          str = row["DisDate"].ToString().Trim();
          if (!str.Equals(string.Empty))
            flag = true;
        }
        else
        {
          if (dataTable2.Rows.IndexOf(row) % 2 == 0)
            stringBuilder.Append("<tr style='background-color: #DFDFDF;'>");
          else
            stringBuilder.Append("<tr>");
          stringBuilder.Append("<td align='left' class='gridtext'>&nbsp;");
          stringBuilder.Append("</td>");
        }
        stringBuilder.Append("<td align='left' class='gridtext'>");
        stringBuilder.Append(row["TransDesc"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='left' class='gridtext'>");
        stringBuilder.Append(row["Receipt_VrNo"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td align='right' class='gridtext'>");
        stringBuilder.Append(row["Credit"].ToString().Trim());
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        num += Convert.ToDouble(row["Credit"].ToString());
        if (dataTable2.Rows.Count.Equals(dataTable2.Rows.IndexOf(row) + 1))
        {
          stringBuilder.Append("<tr>");
          stringBuilder.Append("<td colspan='2'>");
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td colspan='2'>");
          stringBuilder.Append("<hr color='Black'>");
          stringBuilder.Append("</td>");
          stringBuilder.Append("</tr>");
          stringBuilder.Append("<tr style='font-weight:bold;'>");
          stringBuilder.Append("<td colspan='2'>");
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td align='left' class='gridtext'>");
          stringBuilder.Append("Total :");
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td align='right' class='gridtext'>");
          stringBuilder.Append(string.Format("{0:f2}",  num));
          stringBuilder.Append("</td>");
          stringBuilder.Append("</tr>");
          num = 0.0;
          stringBuilder.Append("<tr style='font-weight:bold;'>");
          stringBuilder.Append("<td colspan='4'>");
          stringBuilder.Append("<hr color='Black'>");
          stringBuilder.Append("</td>");
          stringBuilder.Append("</tr>");
        }
      }
      stringBuilder.Append("</table>");
      lblReport.Text = stringBuilder.ToString().Trim();
      Session["FeeRecv"] =  stringBuilder.ToString().Trim();
      btnprint.Visible = true;
      btnExpExcel.Visible = true;
    }
    else
    {
      lblReport.Text = "No records found !";
      btnprint.Visible = false;
      btnExpExcel.Visible = false;
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
        stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
        stringBuilder.Append("<tr><td align='left'><b>");
        stringBuilder.Append("Fee Received:-");
        stringBuilder.Append("</b></td></tr>");
        stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
        stringBuilder.Append("</table>");
        ExportToExcel((stringBuilder.ToString().Trim() + lblReport.Text.ToString().Trim()).ToString().Trim());
      }
      else
        Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
    }
    catch (Exception ex)
    {
      string message = ex.Message;
    }
  }

  private void ExportToExcel(string dataToExport)
  {
    try
    {
      string str = Server.MapPath("Exported_Files/" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
      FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
      StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
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

  protected void btnprint_Click(object sender, EventArgs e)
  {
    try
    {
      Response.Redirect("rptFeeReceivedPrint.aspx");
    }
    catch (Exception ex)
    {
      string message = ex.Message;
    }
  }

  private void LblReportClear()
  {
      lblReport.Text = "";
      btnprint.Visible = false;
      btnExpExcel.Visible = false;
  }
}