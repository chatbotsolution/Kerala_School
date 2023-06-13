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
public partial class Reports_RptIndexNoList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            fillsession();
            fillclass();
            FillClassSection();
            lblCount.Text = "";
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void fillsession()
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
        drpclass.Items.Insert(0, new ListItem("Select", "0"));
    }

    private void FillClassSection()
    {
        ddlSection.Items.Clear();
        ddlSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "@classId",
         drpclass.SelectedValue.ToString().Trim()
      },
      {
         "@session",
         drpSession.SelectedValue
      }
    });
        ddlSection.DataTextField = "Section";
        ddlSection.DataValueField = "Section";
        ddlSection.DataBind();
        ddlSection.Items.Insert(0, new ListItem("All", "0"));
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    private void GenerateReport()
    {
        string empty = string.Empty;
        string str;
        if (ddlSection.SelectedIndex > 0)
            str = obj.ExecuteScalarQry("SELECT COUNT(*) FROM dbo.PS_ClasswiseStudent WHERE SessionYear='" + drpSession.SelectedValue + "' AND ClassID=" + drpclass.SelectedValue + " AND Section='" + ddlSection.SelectedValue + "'");
        else
            str = obj.ExecuteScalarQry("SELECT COUNT(*) FROM dbo.PS_ClasswiseStudent WHERE SessionYear='" + drpSession.SelectedValue + "' AND ClassID=" + drpclass.SelectedValue);
        ViewState["TolStud"] = str.ToString();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@ClassId", drpclass.SelectedValue);
        hashtable.Add("@SessionYear", drpSession.SelectedValue);
        if (ddlSection.SelectedIndex > 0)
            hashtable.Add("@Section", ddlSection.SelectedValue);
        if (txtFrmRoll.Text.ToString() != string.Empty || txtFrmRoll.Text.ToString() != "")
            hashtable.Add("@FrmRollNo", txtFrmRoll.Text.ToString().Trim());
        if (txtToRoll.Text.ToString() != string.Empty || txtToRoll.Text.ToString() != "")
            hashtable.Add("@ToRollNo", txtToRoll.Text.ToString().Trim());
        hashtable.Add("@SortBy", drpSort.SelectedValue.ToString().Trim());
        DataTable dataTable2 = obj.GetDataTable("RptGetIndexNo", hashtable);
        StringBuilder stringBuilder = new StringBuilder("");
        ViewState["ActStud"] = dataTable2.Rows.Count.ToString();
        if (dataTable2.Rows.Count > 0)
        {
            lblCount.Text = "No Of Students:" + dataTable2.Rows.Count.ToString();
            stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='96%'>");
            stringBuilder.Append("<tr><td style='text-align:center' colspan='4' class='tbltd'><strong>Class :" + drpclass.SelectedItem.Text + "&nbsp; &nbsp; Section:" + ddlSection.SelectedItem.Text + "</strong></td></tr>");
            stringBuilder.Append("<tr  style='background-color:#CCCCCC'>");
            stringBuilder.Append("</tr>");
            int num1 = 0;
            int num2 = 1;
            stringBuilder.Append("<tr style='Height:63px'>");
            for (int index1 = 0; index1 < dataTable2.Rows.Count; ++index1)
            {
                stringBuilder.Append("<td align='center' valign='middle'>");
                stringBuilder.Append(dataTable2.Rows[index1]["IndexNo"].ToString().Trim());
                stringBuilder.Append("</td>");
                ++num1;
                if (num1 == 4)
                {
                    num1 = 0;
                    stringBuilder.Append("</tr>");
                    if (index1 != dataTable2.Rows.Count - 1)
                        stringBuilder.Append("<tr style='Height:63px'>");
                    ++num2;
                }
                if (index1 == dataTable2.Rows.Count - 1)
                {
                    if (num1 == 4)
                        stringBuilder.Append("</tr>");
                    else if (num1 != 0)
                    {
                        for (int index2 = 1; index2 <= 4 - num1; ++index2)
                        {
                            stringBuilder.Append("<td align='center' valign='middle'>&nbsp;");
                            stringBuilder.Append("</td>");
                        }
                        stringBuilder.Append("</tr>");
                    }
                }
                if (num2 == 15)
                {
                    stringBuilder.Append("</tr>");
                    stringBuilder.Append("</table>");
                    stringBuilder.Append("<p style='page-break-after:always;'</p>");
                    stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='96%'>");
                    stringBuilder.Append("<tr><td style='text-align:center' colspan='4' class='tbltd'><strong>Class :" + drpclass.SelectedItem.Text + "&nbsp; &nbsp; Section:" + ddlSection.SelectedItem.Text + "</strong></td></tr>");
                    stringBuilder.Append("<tr style='Height:63px'>");
                    num2 = 1;
                }
            }
            stringBuilder.Append("</table>");
            lblmsg.Text = stringBuilder.ToString();
            Session["printdata"] = stringBuilder.ToString();
            btnExpExcel.Enabled = true;
            btnprint.Enabled = true;
        }
        else
        {
            lblCount.Text = "";
            lblmsg.Text = "NO RECORDS FOUND";
            Session["printdata"] = "";
            btnExpExcel.Enabled = false;
            btnprint.Enabled = false;
        }
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblmsg.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("Index No. List:-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(lblmsg.Text.ToString().Trim());
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
            string str = Server.MapPath("Exported_Files/Index No. List" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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

    protected void btnprint_Click(object sender, EventArgs e)
    {
        if (int.Parse(ViewState["TolStud"].ToString()) != int.Parse(ViewState["ActStud"].ToString()))
            Response.Write("<script language='javascript'>alert('Total No of students are not equal to students that are printed');</script>");
        Response.Redirect("RptIndexNoListPrint.aspx");
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassSection();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GenerateStud();
    }

    private void GenerateStud()
    {
        string empty = string.Empty;
        string str;
        if (ddlSection.SelectedIndex > 0)
            str = obj.ExecuteScalarQry("SELECT COUNT(*) FROM dbo.PS_ClasswiseStudent WHERE SessionYear='" + drpSession.SelectedValue + "' AND ClassID=" + drpclass.SelectedValue + " AND Section='" + ddlSection.SelectedValue + "'");
        else
            str = obj.ExecuteScalarQry("SELECT COUNT(*) FROM dbo.PS_ClasswiseStudent WHERE SessionYear='" + drpSession.SelectedValue + "' AND ClassID=" + drpclass.SelectedValue);
        ViewState["TolStud"] = str.ToString();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@ClassId", drpclass.SelectedValue);
        hashtable.Add("@SessionYear", drpSession.SelectedValue);
        if (ddlSection.SelectedIndex > 0)
            hashtable.Add("@Section", ddlSection.SelectedValue);
        if (txtFrmRoll.Text.ToString() != string.Empty || txtFrmRoll.Text.ToString() != "")
            hashtable.Add("@FrmRollNo", txtFrmRoll.Text.ToString().Trim());
        if (txtToRoll.Text.ToString() != string.Empty || txtToRoll.Text.ToString() != "")
            hashtable.Add("@ToRollNo", txtToRoll.Text.ToString().Trim());
        hashtable.Add("@SortBy", drpSort.SelectedValue.ToString().Trim());
        DataTable dataTable2 = obj.GetDataTable("RptGetStudentList", hashtable);
        StringBuilder stringBuilder = new StringBuilder("");
        ViewState["ActStud"] = dataTable2.Rows.Count.ToString();
        if (dataTable2.Rows.Count > 0)
        {
            stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='98%'>");
            stringBuilder.Append("<tr><td style='text-align:center' colspan='5' class='tbltd'><strong>Class :" + drpclass.SelectedItem.Text + "&nbsp; &nbsp; Section:" + ddlSection.SelectedItem.Text + "</strong></td></tr>");
            stringBuilder.Append("<tr><td style='text-align:right' colspan='5' class='tbltd'><strong>Total no of Students :" + dataTable2.Rows.Count + "</strong></td></tr>");
            stringBuilder.Append("<tr  style='background-color:#CCCCCC'>");
            stringBuilder.Append("<td style='width: 8%'  class='tbltxt'><strong>Sl No.</strong></td>");
            stringBuilder.Append("<td style='width: 9%'  class='tbltxt'><strong>Roll No.</strong></td>");
            stringBuilder.Append("<td style='width: 12%'  class='tbltxt'><strong>Index No.</strong></td>");
            stringBuilder.Append("<td style='width: 12%'  class='tbltxt'><strong>Student Name</strong></td>");
            stringBuilder.Append("<td style='width: 12%'  class='tbltxt'><strong>Father Name</strong></td>");
            stringBuilder.Append("</tr>");
            int num = 1;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td>");
                stringBuilder.Append(num.ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td>");
                stringBuilder.Append(row["RollNo"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td>");
                stringBuilder.Append(row["OldAdmnNo"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td>");
                stringBuilder.Append(row["FullName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td>");
                stringBuilder.Append(row["FatherName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
                ++num;
            }
            stringBuilder.Append("</table>");
            lblmsg.Text = stringBuilder.ToString();
            Session["printdata"] = stringBuilder.ToString();
            btnExpExcel.Enabled = true;
            btnprint.Enabled = true;
        }
        else
        {
            lblmsg.Text = "NO RECORDS FOUND";
            Session["printdata"] = "";
            btnExpExcel.Enabled = false;
            btnprint.Enabled = false;
            lblCount.Text = "";
        }
    }
}