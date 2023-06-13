using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptNotice : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
            return;
        Response.Redirect("../Login.aspx");
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        DataSet dataSet = new DataSet();
        StringBuilder stringBuilder1 = new StringBuilder();
        StringBuilder stringBuilder2 = new StringBuilder();
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
                    stringBuilder2.Append("<tr><td align='center' style='font-size:16px; white-space:nowrap;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + "</strong></td></tr>");
            }
        }
        int int32 = Convert.ToInt32(txtNo.Text.Trim());
        for (int index = 1; index <= int32; ++index)
        {
            stringBuilder1.Append("<table width='100%'>");
            stringBuilder1.Append(stringBuilder2.ToString());
            stringBuilder1.Append("<tr><td></td></tr>");
            stringBuilder1.Append("<tr><td align='center' style='font-size:14px; white-space:nowrap;'><strong>NOTICE</strong></td></tr>");
            stringBuilder1.Append("<tr><td></td></tr>");
            stringBuilder1.Append("<tr><td align='left' style='font-size:12px;'>" + dtpDt.GetDateValue().ToString("dd MMM yyyy") + "</td></tr>");
            stringBuilder1.Append("<tr><td align='center' style='font-size:14px;'><strong>" + txtHeading.Text.Trim().ToUpper() + "</strong></td></tr>");
            stringBuilder1.Append("<tr><td></td></tr>");
            stringBuilder1.Append("<tr><td align='left' style='font-size:12px;'>" + txtDtls.Text.Trim() + "</td></tr>");
            stringBuilder1.Append("<tr><td></td></tr>");
            stringBuilder1.Append("<tr><td align='left' style='font-size:12px;'>" + txtName.Text.Trim() + "</td></tr>");
            stringBuilder1.Append("<tr><td align='left' style='font-size:12px;'>( " + txtDesig.Text.Trim() + " )</td></tr>");
            stringBuilder1.Append("<tr><td> <hr /></td></tr>");
            stringBuilder1.Append("</table>");
        }
        ClearAll();
        Session["Notice"] = (object)stringBuilder1.ToString().Trim();
        Response.Redirect("rptNoticePrint.aspx");
    }

    private void ClearAll()
    {
        txtDesig.Text = "";
        txtDt.Text = "";
        txtDtls.Text = "";
        txtHeading.Text = "";
        txtName.Text = "";
        txtNo.Text = "";
    }
}