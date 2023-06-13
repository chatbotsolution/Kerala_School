using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptFeeAdjustment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        lblMsg.Text = string.Empty;
        fillsession();
        fillclass();
        fillstudent();
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
    }

    protected void fillsession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        drpClasses.Items.Clear();
        drpClasses.DataSource = new clsDAL().GetDataTable("ps_sp_get_classesForDDL");
        drpClasses.DataTextField = "classname";
        drpClasses.DataValueField = "classid";
        drpClasses.DataBind();
        drpClasses.Items.Insert(0, new ListItem("All", "0"));
    }

    private void fillstudent()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTableQry;
        if (drpClasses.SelectedIndex != 0)
            dataTableQry = clsDal.GetDataTableQry("select fullname,cs.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.classid=" + drpClasses.SelectedValue + " and cs.SessionYear='" + drpSession.SelectedValue.ToString() + "' order by fullname");
        else
            dataTableQry = clsDal.GetDataTableQry("select fullname,cs.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno where cs.SessionYear='" + drpSession.SelectedValue.ToString() + "' order by fullname");
        drpstudent.DataSource = dataTableQry;
        drpstudent.DataTextField = "fullname";
        drpstudent.DataValueField = "admnno";
        drpstudent.DataBind();
        if (dataTableQry.Rows.Count > 0)
            drpstudent.Items.Insert(0, new ListItem("--All--", "0"));
        else
            drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (drpstudent.SelectedValue == "0" && txtadmnno.Text.Trim() != "")
            SetStudDetails();
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        if (drpClasses.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpClasses.SelectedValue.Trim());
        if (drpstudent.SelectedIndex > 0)
            hashtable.Add("@AdmnNo", drpstudent.SelectedValue.Trim());
        hashtable.Add("@SessionYear", drpSession.SelectedValue.Trim());
        DataTable dataTable2 = clsDal.GetDataTable("ps_get_StudConcession", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            createRemarkReport(dataTable2);
            btnExpExcel.Enabled = true;
            btnPrint.Enabled = true;
            lblMsg.Text = string.Empty;
        }
        else
        {
            lblReport.Text = string.Empty;
            btnPrint.Enabled = false;
            btnExpExcel.Enabled = false;
            lblMsg.Text = "The selected student do not have any concession";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void SetStudDetails()
    {
        try
        {
            clsDAL clsDal = new clsDAL();
            if (clsDal.ExecuteScalarQry("select count(*) from PS_StudMaster where AdmnNo=" + txtadmnno.Text.Trim()) == "0")
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtadmnno, txtadmnno.GetType(), "ShowMessage", "alert('Admission number does not exists')", true);
            }
            else
            {
                fillclass();
                fillstudent();
                drpstudent.SelectedValue = txtadmnno.Text.Trim();
                DataTable dataTableQry = clsDal.GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent where Detained_Promoted='' and admnno=" + txtadmnno.Text);
                drpClasses.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
                drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
            }
        }
        catch (Exception ex)
        {
            if (!(ex.Message != ""))
                return;
            ScriptManager.RegisterClientScriptBlock((Control)txtadmnno, txtadmnno.GetType(), "ShowMessage", "alert('Invalid Admission Number')", true);
        }
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        btnPrint.Visible = false;
        txtadmnno.Text = string.Empty;
        fillstudent();
    }

    protected void drpClasses_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblReport.Text = string.Empty;
        btnPrint.Visible = false;
        fillstudent();
        txtadmnno.Text = string.Empty;
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (drpstudent.SelectedValue != "0")
        {
            txtadmnno.Text = drpstudent.SelectedValue;
            lblReport.Text = string.Empty;
        }
        else
            txtadmnno.Text = string.Empty;
    }

    private void createRemarkReport(DataTable dt)
    {
        if (dt.Rows.Count <= 0)
            return;
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
        stringBuilder.Append("<tr style='background-color: #666; color: White;'>");
        stringBuilder.Append("<td style='width: 100px; text-align:left;' class='tblheader'>Admission No</td>");
        stringBuilder.Append("<td style='text-align:left;' class='tblheader'' align='left'>Stud. Name</td>");
        stringBuilder.Append("<td style='width: 120px; text-align:left;' class='tblheader'>Concession in (%)</td>");
        stringBuilder.Append("<td style='width: 100px; text-align:left;' class='tblheader'>Authority</td>");
        stringBuilder.Append("<td style='width: 150px; text-align:left;' class='tblheader'>Reason</td>");
        stringBuilder.Append("</tr>");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["AdmnNo"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["FullName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["FeeConcessionPer"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["Authority"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append(row["Res"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["Concession"] = stringBuilder.ToString().Trim();
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
                stringBuilder.Append("List of Student with Concession :-");
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