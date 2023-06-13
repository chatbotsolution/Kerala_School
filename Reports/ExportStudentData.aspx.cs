using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_ExportStudentData : System.Web.UI.Page
{
     protected void Page_Load(object sender, EventArgs e)
  {
    if (Page.IsPostBack)
      return;
    fillsession();
    fillclass();
    btnExpExcel.Visible = true;
  }

  private void fillclass()
  {
    drpClasses.Items.Clear();
    drpClasses.DataSource =  new Common().GetDataTable("ps_sp_get_classesForDDL");
    drpClasses.DataTextField = "classname";
    drpClasses.DataValueField = "classid";
    drpClasses.DataBind();
    drpClasses.Items.Insert(0, new ListItem("All", "0"));
  }

  private void fillsession()
  {
    drpSession.DataSource =  new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
    drpSession.DataTextField = "SessionYr";
    drpSession.DataValueField = "SessionYr";
    drpSession.DataBind();
  }

  protected void btnExpExcel_Click(object sender, EventArgs e)
  {
    Common common = new Common();
    DataTable dataTable1 = new DataTable();
    Hashtable hashtable = new Hashtable();
    hashtable.Add( "@sessionYr",  drpSession.SelectedValue.ToString());
    if (drpClasses.SelectedIndex > 0)
      hashtable.Add( "@ClassID",  drpClasses.SelectedValue.ToString());
    int int16 = (int) Convert.ToInt16(ConfigurationManager.AppSettings["SchoolId"].ToString());
    hashtable.Add( "@SSVMID",  int16);
    DataTable dataTable2 = common.GetDataTable("ps_sp_get_ExportStudentDetails", hashtable);
    if (dataTable2.Rows.Count > 0)
      GenerateReport(dataTable2);
    else
      lblReport.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='center'>No Records Found  !</div>";
    try
    {
      if (lblReport.Text.ToString().Trim() != "")
      {
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
        stringBuilder.Append("<tr><td style='height: 10px'>");
        stringBuilder.Append(lblReport.Text.ToString().Trim());
        stringBuilder.Append("</td></tr>");
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

  private void GenerateReport(DataTable dt)
  {
    try
    {
      if (dt.Rows.Count > 0)
      {
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='width:100%;' border='1'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>AdmnNo</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>AdmnDate</b></td>");
        stringBuilder.Append("<td style='width: 110px' align='left' class='tblheader'><b>SessionYear</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>ReadingClass</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>Section</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>RollNo</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>FullName</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>NickName</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>DOB</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>FatherName</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>FatherOccupation</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>MotherName</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>MotherOccupation</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>Gender</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>Religion</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>Category</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>Nationality</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>MotherTongue</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>PresAddress</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>PresAddrDist</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>PresAddrPin</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>PresAddrPS</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>PermAddress</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>PermAddrDist</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>PermAddrPin</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>PermAddrPS</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>Phone</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>Mobile</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>UrbanRuralTribe</b></td>");
        stringBuilder.Append("<td style='width: 100px' align='left' class='tblheader'><b>SSVMID</b></td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase) dt.Rows)
        {
          stringBuilder.Append("<tr>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["AdmnNo"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["admdate"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["SessionYear"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["ClassName"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["Section"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["RollNo"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["FullName"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["NickName"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["dateofbirth"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["FatherName"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["FatherOccupation"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["MotherName"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["MotherOccupation"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["Gender"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["religion"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["CatName"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["Nationality"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["MotherTongue"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["PresentAddress"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["PresAddrDist"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["PresAddrPin"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["PresAddrPS"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["PermAddress"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["PermAddrDist"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["PermAddrPin"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["PermAddrPS"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["TelNoResidence"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["TeleNoOffice"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["UrbanRuralTribe"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("<td class='tbltd'>");
          stringBuilder.Append(row["SSVMID"].ToString().Trim());
          stringBuilder.Append("</td>");
          stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["printData"] =  stringBuilder.ToString().Trim();
      }
      else
      {
        lblReport.Text = "";
        Session["printData"] =  "";
      }
    }
    catch (Exception ex)
    {
    }
  }

  private void ExportToExcel(string dataToExport)
  {
    try
    {
      string str = Server.MapPath("Exported_Files/StudentDataRpt" + DateTime.Now.ToString("dd-MMM-yyyy hh-mm-ss") + ".xls");
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

  protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
  {
    lblReport.Text = "";
  }

  protected void drpClasses_SelectedIndexChanged(object sender, EventArgs e)
  {
    lblReport.Text = "";
  }
}