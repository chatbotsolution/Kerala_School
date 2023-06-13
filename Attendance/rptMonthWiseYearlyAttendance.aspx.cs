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
using System.Web.UI.WebControls;

public partial class Attendance_rptMonthWiseYearlyAttendance : System.Web.UI.Page
{
    private clsDAL DAL = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        bindDropDown(drpClass, "select ClassId,ClassName from PS_ClassMaster", "ClassName", "ClassId");
        bindDropDown(drpSession, "select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc", "SessionYr", "SessionYr");
        drpSession.Items.RemoveAt(0);
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = DAL.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@Session", drpSession.SelectedValue);
        hashtable.Add("@ClassId", drpClass.SelectedValue);
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue.Trim());
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = DAL.GetDataTable("ATTEND_StudAttendMonthWiseColumn", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            GenerateAttendanceRport(dataTable2);
        }
        else
        {
            btnPrint.Enabled = false;
            btnExpExcel.Enabled = false;
            litReport.Text = "<div style='color:Red;' class='tbltxt'><b>No Record Found !!!</b></div>";
        }
    }

    private void GenerateAttendanceRport(DataTable dtAttend)
    {
        int int32 = Convert.ToInt32(dtAttend.Rows[0]["AprP"].ToString() + dtAttend.Rows[0]["AprA"].ToString());
        int num1 = Convert.ToInt32(dtAttend.Rows[0]["MayP"].ToString()) + Convert.ToInt32(dtAttend.Rows[0]["MayA"].ToString());
        int num2 = Convert.ToInt32(dtAttend.Rows[0]["JunP"].ToString()) + Convert.ToInt32(dtAttend.Rows[0]["JunA"].ToString());
        int num3 = Convert.ToInt32(dtAttend.Rows[0]["JulP"].ToString()) + Convert.ToInt32(dtAttend.Rows[0]["JulA"].ToString());
        int num4 = Convert.ToInt32(dtAttend.Rows[0]["AugP"].ToString()) + Convert.ToInt32(dtAttend.Rows[0]["AugA"].ToString());
        int num5 = Convert.ToInt32(dtAttend.Rows[0]["SepP"].ToString()) + Convert.ToInt32(dtAttend.Rows[0]["SepA"].ToString());
        int num6 = Convert.ToInt32(dtAttend.Rows[0]["OctP"].ToString()) + Convert.ToInt32(dtAttend.Rows[0]["OctA"].ToString());
        int num7 = Convert.ToInt32(dtAttend.Rows[0]["NovP"].ToString()) + Convert.ToInt32(dtAttend.Rows[0]["NovA"].ToString());
        int num8 = Convert.ToInt32(dtAttend.Rows[0]["DecP"].ToString()) + Convert.ToInt32(dtAttend.Rows[0]["DecA"].ToString());
        int num9 = Convert.ToInt32(dtAttend.Rows[0]["JanP"].ToString()) + Convert.ToInt32(dtAttend.Rows[0]["JanA"].ToString());
        int num10 = Convert.ToInt32(dtAttend.Rows[0]["FebP"].ToString()) + Convert.ToInt32(dtAttend.Rows[0]["FebA"].ToString());
        int num11 = Convert.ToInt32(dtAttend.Rows[0]["MarP"].ToString()) + Convert.ToInt32(dtAttend.Rows[0]["MarA"].ToString());
        int num12 = Convert.ToInt32(dtAttend.Rows[0]["TotP"].ToString()) + Convert.ToInt32(dtAttend.Rows[0]["TotA"].ToString());
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<table class='tbltxt' style='border-collapse:collapse;' border='1px' cellpadding='2px' cellspacing='0' width='100%'> <tr>");
        stringBuilder.Append("<td rowspan='2' align='left' style='width: 90px'><b>Admn. No</b></td><td rowspan='2' align='left' style='width: 220px'><b>Student Name</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Apr(" + int32 + ")</b> </td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>May(" + num1 + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Jun(" + num2 + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Jul(" + num3 + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Aug(" + num4 + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Sep(" + num5 + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Oct(" + num6 + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Nov(" + num7 + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Dec(" + num8 + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Jan(" + num9 + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Feb(" + num10 + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Mar(" + num11 + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Total(" + num12 + ")</b></td>");
        stringBuilder.Append("<td colspan='2' align='center'><b>Attendance<b></td>");
        stringBuilder.Append("</tr> <tr>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' style='color:Green'>P</td><td align='center' style='color:Red'>A</td>");
        stringBuilder.Append("<td align='center' colspan='2' style='color:Green'>%</td></tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dtAttend.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left'>");
            stringBuilder.Append(row["AdmnNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' style='white-space:nowrap;'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["AprP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["AprA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["MayP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["MayA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["JunP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["JunA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["JulP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["JulA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["AugP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["AugA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["SepP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["SepA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["OctP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["OctA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["NovP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["NovA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["DecP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["DecA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["JanP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["JanA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["FebP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["FebA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["MarP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["MarA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Green'>");
            stringBuilder.Append(row["TotP"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center' style='color:Red'>");
            stringBuilder.Append(row["TotA"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='center'>");
            stringBuilder.Append((Convert.ToDecimal(row["TotP"].ToString().Trim()) / (Convert.ToDecimal(row["TotP"].ToString().Trim()) + Convert.ToDecimal(row["TotA"].ToString().Trim())) * new Decimal(100)).ToString("0.00"));
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        litReport.Text = stringBuilder.ToString();
        Session["MonthwiseYrlyAttendance"] = stringBuilder.ToString().Trim();
        Session["SessYr"] = drpSession.SelectedValue.ToString();
        btnPrint.Enabled = true;
        btnExpExcel.Enabled = true;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptMonthWiseYearlyAttendancePrint.aspx');", true);
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
            if (litReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td align='left' colspan='14'><b>");
                stringBuilder.Append("Studentwise Attendance Report :- ");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(litReport.Text.ToString().Trim());
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

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("../Up_Files/Exported_Files/MonthWiseYearlyAttendance" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSectionDropDown();
    }

    private void FillSectionDropDown()
    {
        drpSection.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "classId",
         drpClass.SelectedValue
      },
      {
         "session",
         drpSession.SelectedValue
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("ALL", "0"));
    }
}