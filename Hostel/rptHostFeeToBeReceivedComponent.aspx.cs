using ASP;
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

public partial class Hostel_rptHostFeeToBeReceivedComponent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SSVMID"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        dtpDate.SetDateValue(DateTime.Today);
        GenerateData();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        lblPrevDue.Text = "";
        lblReport.Text = "";
        lblTotal.Text = "";
        GenerateData();
    }

    public string GetCurrSession(DateTime suppliedDate)
    {
        int int32_1 = Convert.ToInt32(suppliedDate.ToString("yyyy"));
        int int32_2 = Convert.ToInt32(suppliedDate.Month.ToString());
        string str = "";
        if (int32_2 > 0 && int32_2 < 4)
            str = Convert.ToString(int32_1 - 1) + "-" + Convert.ToString(suppliedDate.ToString("yy"));
        else if (int32_2 > 3 && int32_2 < 13)
            str = Convert.ToString(int32_1) + "-" + Convert.ToString(suppliedDate.AddYears(1).ToString("yy"));
        return str;
    }

    private void GenerateData()
    {
        StringBuilder stringBuilder = new StringBuilder();
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        string currSession = GetCurrSession(dtpDate.GetDateValue());
        if (txtDate.Text.Trim() != "")
            hashtable.Add("@ToDt", dtpDate.GetDateValue().ToString("MM/dd/yyyy"));
        hashtable.Add("@SessionYr", currSession.Trim());
        hashtable.Add("@SchoolID", Convert.ToInt32(Session["SchoolId"]));
        DataSet dataSet = new DataSet();
        currSession.Split('-');
        CreateReport(clsDal.GetDataSet("HostRptGetCompWiseFeeToBRcvdNew", hashtable));
    }

    protected void CreateReport(DataSet ds)
    {
        string str = Information();
        StringBuilder stringBuilder1 = new StringBuilder();
        stringBuilder1.Append(str);
        StringBuilder stringBuilder2 = new StringBuilder("");
        StringBuilder stringBuilder3 = new StringBuilder("");
        StringBuilder stringBuilder4 = new StringBuilder("");
        StringBuilder stringBuilder5 = new StringBuilder("");
        StringBuilder stringBuilder6 = new StringBuilder("");
        StringBuilder stringBuilder7 = new StringBuilder("");
        double num1 = 0.0;
        GetCurrSession(dtpDate.GetDateValue()).Split('-');
        if (ds.Tables[0].Rows.Count > 0)
        {
            stringBuilder2.Append("<table cellpadding='1' cellspacing='1'  width='700px' style='background-color:#CCC;' >");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td style='text-align:left;' class='tblheader'>Fee Components</td>");
            stringBuilder2.Append("<td style='width: 120px' align='right' class='tblheader'>Amount</td>");
            stringBuilder2.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[0].Rows)
            {
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td align='left' class='tbltd'>");
                stringBuilder2.Append(row["TransDesc"].ToString().Trim());
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='right' class='tbltd'>");
                stringBuilder2.Append(row["Balance"].ToString().Trim());
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("</tr>");
                num1 += Convert.ToDouble(row["Balance"].ToString().Trim());
            }
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td colspan='2' align='right' class='tbltd'>");
            stringBuilder2.Append("<strong>Total &nbsp;:&nbsp; &nbsp;&nbsp;" + string.Format("{0:F2}", num1));
            stringBuilder2.Append("</td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("</table>");
            lblReport.Text = stringBuilder2.ToString().Trim();
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            stringBuilder6.Append("<table cellpadding='1' cellspacing='1'  width='700px' style='background-color:#CCC;' >");
            foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[1].Rows)
            {
                stringBuilder6.Append("<tr>");
                stringBuilder6.Append("<td align='left' class='tbltd'><b>");
                stringBuilder6.Append(row["TransDesc"].ToString().Trim());
                stringBuilder6.Append("</b></td>");
                stringBuilder6.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
                stringBuilder6.Append(row["Balance"].ToString().Trim());
                stringBuilder6.Append("</b></td>");
                stringBuilder6.Append("</tr>");
            }
            stringBuilder6.Append("</table>");
            lblPrevDue.Text = stringBuilder6.ToString().Trim();
        }
        double num2 = num1 + Convert.ToDouble(ds.Tables[1].Rows[0][0].ToString());
        stringBuilder7.Append("<table cellpadding='1' cellspacing='1'  width='700px' style='background-color:#CCC;' >");
        stringBuilder7.Append("<tr>");
        stringBuilder7.Append("<td align='right' class='tbltd'><b>");
        stringBuilder7.Append("<strong>All Total &nbsp;:&nbsp; &nbsp;&nbsp;" + string.Format("{0:F2}", num2));
        stringBuilder7.Append("</b></td>");
        stringBuilder7.Append("</tr>");
        stringBuilder7.Append("</table>");
        if (Convert.ToDouble(num2) > 0.0)
        {
            lblTotal.Text = stringBuilder7.ToString().Trim();
            Session["OutStandingFee"] = (stringBuilder1.ToString().Trim() + stringBuilder2.ToString().Trim() + stringBuilder6.ToString().Trim() + stringBuilder7.ToString().Trim());
            btnPrint.Visible = true;
        }
        else
        {
            lblPrevDue.Text = string.Empty;
            lblReport.Text = string.Empty;
            lblTotal.Text = string.Empty;
            ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
        }
    }

    private string Information()
    {
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
                    stringBuilder.Append("<table width='700px' border='0px'><tr>");
                    stringBuilder.Append("<td align='center' style='font-size:20px;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2) + "</strong></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2) + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2) + "</b></td></tr>");
                    stringBuilder.Append("<tr><td align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2) + ",Phone-" + clsDal.Decrypt(row["Phone"].ToString().Trim(), str2) + "</b>");
                    stringBuilder.Append("</td></tr>");
                    stringBuilder.Append("</table>");
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("rptHostFeeToBeReceivedPrtnt.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptFeeReceivedComponent.aspx");
    }
}