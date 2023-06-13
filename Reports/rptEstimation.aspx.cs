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

public partial class Reports_rptEstimation : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        btnPrint.Enabled = false;
        btnExpExcel.Enabled = false;
        fillSession();
    }

    private void fillSession()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        drpSessionYr.DataSource = clsDal.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSessionYr.DataTextField = "SessionYr";
        drpSessionYr.DataValueField = "SessionYr";
        drpSessionYr.DataBind();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        DataSet dataSet1 = new DataSet();
        hashtable.Add("@SessionYr", drpSessionYr.SelectedValue.ToString().Trim());
        string str1 = drpSessionYr.SelectedValue.ToString().Substring(0, 4);
        DataSet dataSet2;
        int num;
        if (Convert.ToInt32(str1) >= 2014)
        {
            string[] strArray = drpSessionYr.SelectedValue.ToString().Split('-');
            string str2 = Convert.ToString(Convert.ToInt32(strArray[0].Trim()) - 1) + "-" + Convert.ToString(Convert.ToInt32(strArray[1].Trim()) - 1);
            hashtable.Add("@StartDt", Convert.ToDateTime("1 Apr" + str1));
            hashtable.Add("@PrevSnY", str2.Trim());
            dataSet2 = obj.GetDataSet("Ps_Sp_GetCompWiseEstimationNew", hashtable);
            num = dataSet2.Tables[0].Rows.Count + dataSet2.Tables[1].Rows.Count + dataSet2.Tables[2].Rows.Count + dataSet2.Tables[3].Rows.Count + dataSet2.Tables[4].Rows.Count + dataSet2.Tables[5].Rows.Count + dataSet2.Tables[6].Rows.Count;
        }
        else
        {
            dataSet2 = obj.GetDataSet("Ps_Sp_GetCompWiseEstimation", hashtable);
            num = dataSet2.Tables[0].Rows.Count + dataSet2.Tables[1].Rows.Count + dataSet2.Tables[2].Rows.Count + dataSet2.Tables[3].Rows.Count + dataSet2.Tables[4].Rows.Count + dataSet2.Tables[5].Rows.Count + dataSet2.Tables[6].Rows.Count + dataSet2.Tables[7].Rows.Count;
        }
        if (num > 0)
            CreateReport(dataSet2);
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
    }

    protected void CreateReport(DataSet ds)
    {
        double num1 = 0.0;
        double num2 = 0.0;
        double num3 = 0.0;
        double num4 = 0.0;
        string str1 = Information();
        StringBuilder stringBuilder1 = new StringBuilder();
        stringBuilder1.Append(str1);
        StringBuilder stringBuilder2 = new StringBuilder("");
        StringBuilder stringBuilder3 = new StringBuilder("");
        StringBuilder stringBuilder4 = new StringBuilder("");
        StringBuilder stringBuilder5 = new StringBuilder("");
        StringBuilder stringBuilder6 = new StringBuilder("");
        StringBuilder stringBuilder7 = new StringBuilder("");
        StringBuilder stringBuilder8 = new StringBuilder("");
        StringBuilder stringBuilder9 = new StringBuilder("");
        StringBuilder stringBuilder10 = new StringBuilder("");
        StringBuilder stringBuilder11 = new StringBuilder("");
        StringBuilder stringBuilder12 = new StringBuilder("");
        StringBuilder stringBuilder13 = new StringBuilder("");
        double num5 = 0.0;
        double num6 = 0.0;
        string str2 = drpSessionYr.SelectedValue.ToString().Substring(0, 4);
        if (ds.Tables[0].Rows.Count > 0)
        {
            stringBuilder2.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td style='text-align:left;' class='tblheader'>Fee Components</td>");
            stringBuilder2.Append("<td style='width: 120px' align='right' class='tblheader'>Received Amount</td>");
            stringBuilder2.Append("<td style='width: 140px' align='right' class='tblheader'>Outstanding Amount</td>");
            stringBuilder2.Append("<td style='width: 120px' align='right' class='tblheader'>Total Amount</td>");
            stringBuilder2.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[0].Rows)
            {
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td align='left' class='tbltd'>");
                stringBuilder2.Append(row["TransDesc"].ToString().Trim());
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='right' class='tbltd'>");
                stringBuilder2.Append(row["credit"].ToString().Trim());
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='right' class='tbltd'>");
                stringBuilder2.Append(row["Balance"].ToString().Trim());
                stringBuilder2.Append("</td>");
                stringBuilder2.Append("<td align='right' class='tbltd'><b>");
                stringBuilder2.Append(row["Total"].ToString().Trim());
                stringBuilder2.Append("</b></td>");
                stringBuilder2.Append("</tr>");
                num5 += Convert.ToDouble(row["credit"].ToString().Trim());
                num6 += Convert.ToDouble(row["Balance"].ToString().Trim());
                num1 += Convert.ToDouble(row["Total"].ToString().Trim());
            }
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='right' class='tbltd'>");
            stringBuilder2.Append("<strong>Total &nbsp;:&nbsp; </strong>");
            stringBuilder2.Append("</td>");
            stringBuilder2.Append("<td align='right' class='tbltd'><strong>");
            stringBuilder2.Append(string.Format("{0:F2}", num5));
            stringBuilder2.Append("</strong></td>");
            stringBuilder2.Append("<td align='right' class='tbltd'><strong>");
            stringBuilder2.Append(string.Format("{0:F2}", num6));
            stringBuilder2.Append("</strong></td>");
            stringBuilder2.Append("<td align='right' class='tbltd'><strong>");
            stringBuilder2.Append(string.Format("{0:F2}", num1));
            stringBuilder2.Append("</strong></td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("</table>");
            lblReport.Text = stringBuilder2.ToString().Trim();
        }
        //if (ds.Tables[1].Rows.Count > 0)
        //{
        //    stringBuilder5.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
        //    foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[1].Rows)
        //    {
        //        stringBuilder5.Append("<tr>");
        //        stringBuilder5.Append("<td align='left' class='tbltd'><b>");
        //        stringBuilder5.Append(row["TransDesc"].ToString().Trim());
        //        stringBuilder5.Append("</b></td>");
        //        stringBuilder5.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
        //        stringBuilder5.Append(row["credit"].ToString().Trim());
        //        stringBuilder5.Append("</b></td>");
        //        stringBuilder5.Append("<td align='right' class='tbltd' style='width: 140px'><b>");
        //        stringBuilder5.Append(row["Balance"].ToString().Trim());
        //        stringBuilder5.Append("</b></td>");
        //        stringBuilder5.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
        //        stringBuilder5.Append(row["Total"].ToString().Trim());
        //        stringBuilder5.Append("</b></td>");
        //        stringBuilder5.Append("</tr>");
        //    }
        //    stringBuilder5.Append("</table>");
        //    lblFine.Text = stringBuilder5.ToString().Trim();
        //}
        //if (ds.Tables[2].Rows.Count > 0)
        //{
        //    stringBuilder6.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
        //    foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[2].Rows)
        //    {
        //        stringBuilder6.Append("<tr>");
        //        stringBuilder6.Append("<td align='left' class='tbltd'><b>");
        //        stringBuilder6.Append(row["TransDesc"].ToString().Trim());
        //        stringBuilder6.Append("</b></td>");
        //        stringBuilder6.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
        //        stringBuilder6.Append(row["credit"].ToString().Trim());
        //        stringBuilder6.Append("</b></td>");
        //        stringBuilder6.Append("<td align='right' class='tbltd' style='width: 140px'><b>");
        //        stringBuilder6.Append(row["Balance"].ToString().Trim());
        //        stringBuilder6.Append("</b></td>");
        //        stringBuilder6.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
        //        stringBuilder6.Append(row["Total"].ToString().Trim());
        //        stringBuilder6.Append("</b></td>");
        //        stringBuilder6.Append("</tr>");
        //    }
        //    stringBuilder6.Append("</table>");
        //    lblBus.Text = stringBuilder6.ToString().Trim();
        //}
        double num7;
        double num8;
        double num9;
        if (Convert.ToInt32(str2) >= 2014)
        {
            //stringBuilder3.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
            //stringBuilder3.Append("<tr>");
            //stringBuilder3.Append("<td style='text-align:left;' class='tblheader'><b>Others</b></td>");
            //stringBuilder3.Append("<td style='width: 120px' align='right' class='tblheader'></td>");
            //stringBuilder3.Append("<td style='width: 140px' align='right' class='tblheader'></td>");
            //stringBuilder3.Append("<td style='width: 120px' align='right' class='tblheader'></td>");
            //stringBuilder3.Append("</tr>");
            //foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[3].Rows)
            //{
            //    stringBuilder3.Append("<tr>");
            //    stringBuilder3.Append("<td align='left' class='tbltd'>");
            //    stringBuilder3.Append(row["TransDesc"].ToString().Trim());
            //    stringBuilder3.Append("</td>");
            //    stringBuilder3.Append("<td align='right' class='tbltd'>");
            //    stringBuilder3.Append(row["credit"].ToString().Trim());
            //    stringBuilder3.Append("</td>");
            //    stringBuilder3.Append("<td align='right' class='tbltd'>");
            //    stringBuilder3.Append(row["Balance"].ToString().Trim());
            //    stringBuilder3.Append("</td>");
            //    stringBuilder3.Append("<td align='right' class='tbltd'><b>");
            //    stringBuilder3.Append(row["Total"].ToString().Trim());
            //    stringBuilder3.Append("</b></td>");
            //    stringBuilder3.Append("</tr>");
            //    num2 += Convert.ToDouble(row["credit"].ToString().Trim());
            //    num3 += Convert.ToDouble(row["Total"].ToString().Trim());
            //}
            //stringBuilder3.Append("<tr>");
            //stringBuilder3.Append("<td align='right' class='tbltd'>");
            //stringBuilder3.Append("<strong>Total &nbsp;:&nbsp; </strong>");
            //stringBuilder3.Append("</td>");
            //stringBuilder3.Append("<td align='right' class='tbltd'><strong>");
            //stringBuilder3.Append(string.Format("{0:F2}", num2));
            //stringBuilder3.Append("</strong></td>");
            //stringBuilder3.Append("<td align='right' class='tbltd'><strong>");
            //stringBuilder3.Append(string.Format("{0:F2}", '0'));
            //stringBuilder3.Append("</strong></td>");
            //stringBuilder3.Append("<td align='right' class='tbltd'><strong>");
            //stringBuilder3.Append(string.Format("{0:F2}", num3));
            //stringBuilder3.Append("</strong></td>");
            //stringBuilder3.Append("</tr>");
            //stringBuilder3.Append("</table>");
            //lblProsFee.Text = stringBuilder3.ToString().Trim();
            //if (ds.Tables[4].Rows.Count > 0)
            //{
            //    stringBuilder4.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
            //    foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[4].Rows)
            //    {
            //        stringBuilder4.Append("<tr>");
            //        stringBuilder4.Append("<td align='left' class='tbltd'><b>");
            //        stringBuilder4.Append(row["TransDesc"].ToString().Trim());
            //        stringBuilder4.Append("</b></td>");
            //        stringBuilder4.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder4.Append(row["credit"].ToString().Trim());
            //        stringBuilder4.Append("</b></td>");
            //        stringBuilder4.Append("<td align='right' class='tbltd' style='width: 140px'><b>");
            //        stringBuilder4.Append(row["Balance"].ToString().Trim());
            //        stringBuilder4.Append("</b></td>");
            //        stringBuilder4.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder4.Append(row["Total"].ToString().Trim());
            //        stringBuilder4.Append("</b></td>");
            //        stringBuilder4.Append("</tr>");
            //    }
            //    stringBuilder4.Append("</table>");
            //}
            //if (ds.Tables[5].Rows.Count > 0)
            //{
            //    stringBuilder4.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
            //    foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[5].Rows)
            //    {
            //        stringBuilder4.Append("<tr>");
            //        stringBuilder4.Append("<td align='left' class='tbltd'><b>");
            //        stringBuilder4.Append(row["TransDesc"].ToString().Trim());
            //        stringBuilder4.Append("</b></td>");
            //        stringBuilder4.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder4.Append(row["credit"].ToString().Trim());
            //        stringBuilder4.Append("</b></td>");
            //        stringBuilder4.Append("<td align='right' class='tbltd' style='width: 140px'><b>0.00");
            //        stringBuilder4.Append("</b></td>");
            //        stringBuilder4.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder4.Append(row["credit"].ToString().Trim());
            //        stringBuilder4.Append("</b></td>");
            //        stringBuilder4.Append("</tr>");
            //    }
            //    stringBuilder4.Append("</table>");
            //}
            //if (ds.Tables[6].Rows.Count > 0)
            //{
            //    stringBuilder4.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
            //    foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[6].Rows)
            //    {
            //        stringBuilder4.Append("<tr>");
            //        stringBuilder4.Append("<td align='left' class='tbltd'><b>");
            //        stringBuilder4.Append(row["TransDesc"].ToString().Trim());
            //        stringBuilder4.Append("</b></td>");
            //        stringBuilder4.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder4.Append(row["credit"].ToString().Trim());
            //        stringBuilder4.Append("</b></td>");
            //        stringBuilder4.Append("<td align='right' class='tbltd' style='width: 140px'><b>0.00");
            //        stringBuilder4.Append("</b></td>");
            //        stringBuilder4.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder4.Append(row["credit"].ToString().Trim());
            //        stringBuilder4.Append("</b></td>");
            //        stringBuilder4.Append("</tr>");
            //    }
            //    stringBuilder4.Append("</table>");
            //}
            //lblPrev.Text = stringBuilder4.ToString().Trim();
            num7 = num5 + Convert.ToDouble(ds.Tables[1].Rows[0][0].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[0][0].ToString()) + num2 + Convert.ToDouble(ds.Tables[4].Rows[0][0].ToString()) + Convert.ToDouble(ds.Tables[5].Rows[0][0].ToString()) + Convert.ToDouble(ds.Tables[6].Rows[0][0].ToString());
            num8 = num6 + Convert.ToDouble(ds.Tables[1].Rows[0][1].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[0][1].ToString()) + Convert.ToDouble(ds.Tables[4].Rows[0][1].ToString()) + Convert.ToDouble(ds.Tables[5].Rows[0][1].ToString()) + Convert.ToDouble(ds.Tables[6].Rows[0][1].ToString());
            num9 = num1 + Convert.ToDouble(ds.Tables[1].Rows[0][2].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[0][2].ToString()) + num3 + Convert.ToDouble(ds.Tables[4].Rows[0][2].ToString()) + Convert.ToDouble(ds.Tables[5].Rows[0][2].ToString()) + Convert.ToDouble(ds.Tables[6].Rows[0][2].ToString());
        }
        else
        {
            //if (ds.Tables[3].Rows.Count > 0)
            //{
            //    stringBuilder7.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
            //    foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[3].Rows)
            //    {
            //        stringBuilder7.Append("<tr>");
            //        stringBuilder7.Append("<td align='left' class='tbltd'><b>");
            //        stringBuilder7.Append(row["TransDesc"].ToString().Trim());
            //        stringBuilder7.Append("</b></td>");
            //        stringBuilder7.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder7.Append(row["credit"].ToString().Trim());
            //        stringBuilder7.Append("</b></td>");
            //        stringBuilder7.Append("<td align='right' class='tbltd' style='width: 140px'><b>");
            //        stringBuilder7.Append(row["Balance"].ToString().Trim());
            //        stringBuilder7.Append("</b></td>");
            //        stringBuilder7.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder7.Append(row["Total"].ToString().Trim());
            //        stringBuilder7.Append("</b></td>");
            //        stringBuilder7.Append("</tr>");
            //    }
            //    stringBuilder7.Append("</table>");
            //    lblHostel.Text = stringBuilder7.ToString().Trim();
            //}
            //if (ds.Tables[4].Rows.Count > 0)
            //{
            //    stringBuilder8.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
            //    foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[4].Rows)
            //    {
            //        stringBuilder8.Append("<tr>");
            //        stringBuilder8.Append("<td align='left' class='tbltd'><b>");
            //        stringBuilder8.Append(row["TransDesc"].ToString().Trim());
            //        stringBuilder8.Append("</b></td>");
            //        stringBuilder8.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder8.Append(row["credit"].ToString().Trim());
            //        stringBuilder8.Append("</b></td>");
            //        stringBuilder8.Append("<td align='right' class='tbltd' style='width: 140px'><b>0.00");
            //        stringBuilder8.Append("</b></td>");
            //        stringBuilder8.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder8.Append(row["credit"].ToString().Trim());
            //        stringBuilder8.Append("</b></td>");
            //        stringBuilder8.Append("</tr>");
            //    }
            //    stringBuilder8.Append("</table>");
            //    lblProsFee.Text = stringBuilder8.ToString().Trim();
            //}
            //if (ds.Tables[5].Rows.Count > 0)
            //{
            //    stringBuilder9.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
            //    foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[5].Rows)
            //    {
            //        stringBuilder9.Append("<tr>");
            //        stringBuilder9.Append("<td align='left' class='tbltd'><b>");
            //        stringBuilder9.Append(row["TransDesc"].ToString().Trim());
            //        stringBuilder9.Append("</b></td>");
            //        stringBuilder9.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder9.Append(row["credit"].ToString().Trim());
            //        stringBuilder9.Append("</b></td>");
            //        stringBuilder9.Append("<td align='right' class='tbltd' style='width: 140px'><b>0.00");
            //        stringBuilder9.Append("</b></td>");
            //        stringBuilder9.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder9.Append(row["credit"].ToString().Trim());
            //        stringBuilder9.Append("</b></td>");
            //        stringBuilder9.Append("</tr>");
            //    }
            //    stringBuilder9.Append("</table>");
            //    lblMiscFee.Text = stringBuilder9.ToString().Trim();
            //}
            //if (ds.Tables[6].Rows.Count > 0)
            //{
            //    stringBuilder10.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
            //    foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[6].Rows)
            //    {
            //        stringBuilder10.Append("<tr>");
            //        stringBuilder10.Append("<td align='left' class='tbltd'><b>");
            //        stringBuilder10.Append(row["TransDesc"].ToString().Trim());
            //        stringBuilder10.Append("</b></td>");
            //        stringBuilder10.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder10.Append(row["credit"].ToString().Trim());
            //        stringBuilder10.Append("</b></td>");
            //        stringBuilder10.Append("<td align='right' class='tbltd' style='width: 140px'><b>0.00");
            //        stringBuilder10.Append("</b></td>");
            //        stringBuilder10.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder10.Append(row["credit"].ToString().Trim());
            //        stringBuilder10.Append("</b></td>");
            //        stringBuilder10.Append("</tr>");
            //    }
            //    stringBuilder10.Append("</table>");
            //    lblBookMaterial.Text = stringBuilder10.ToString().Trim();
            //}
            //if (ds.Tables[7].Rows.Count > 0)
            //{
            //    stringBuilder11.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
            //    foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[7].Rows)
            //    {
            //        stringBuilder11.Append("<tr>");
            //        stringBuilder11.Append("<td align='left' class='tbltd'><b>");
            //        stringBuilder11.Append(row["TransDesc"].ToString().Trim());
            //        stringBuilder11.Append("</b></td>");
            //        stringBuilder11.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder11.Append(row["credit"].ToString().Trim());
            //        stringBuilder11.Append("</b></td>");
            //        stringBuilder11.Append("<td align='right' class='tbltd' style='width: 140px'><b>0.00");
            //        stringBuilder11.Append("</b></td>");
            //        stringBuilder11.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
            //        stringBuilder11.Append(row["credit"].ToString().Trim());
            //        stringBuilder11.Append("</b></td>");
            //        stringBuilder11.Append("</tr>");
            //    }
            //    stringBuilder11.Append("</table>");
            //    lblAnudan.Text = stringBuilder11.ToString().Trim();
            //}
            num7 = num5 + Convert.ToDouble(ds.Tables[1].Rows[0][0].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[0][0].ToString()) + Convert.ToDouble(ds.Tables[3].Rows[0][0].ToString()) + Convert.ToDouble(ds.Tables[4].Rows[0][0].ToString()) + Convert.ToDouble(ds.Tables[5].Rows[0][0].ToString()) + Convert.ToDouble(ds.Tables[6].Rows[0][0].ToString()) + Convert.ToDouble(ds.Tables[7].Rows[0][0].ToString());
            num8 = num6 + Convert.ToDouble(ds.Tables[1].Rows[0][1].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[0][1].ToString()) + Convert.ToDouble(ds.Tables[3].Rows[0][1].ToString());
            num9 = num1 + Convert.ToDouble(ds.Tables[1].Rows[0][2].ToString()) + Convert.ToDouble(ds.Tables[2].Rows[0][2].ToString()) + Convert.ToDouble(ds.Tables[3].Rows[0][2].ToString()) + Convert.ToDouble(ds.Tables[4].Rows[0][0].ToString()) + Convert.ToDouble(ds.Tables[5].Rows[0][0].ToString()) + Convert.ToDouble(ds.Tables[6].Rows[0][0].ToString()) + Convert.ToDouble(ds.Tables[7].Rows[0][0].ToString());
        }
        stringBuilder12.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
        stringBuilder12.Append("<tr>");
        stringBuilder12.Append("<td align='right' class='tbltd'><b>");
        stringBuilder12.Append("All Total &nbsp;:&nbsp; ");
        stringBuilder12.Append("</b></td>");
        stringBuilder12.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
        stringBuilder12.Append(string.Format("{0:F2}", num7));
        stringBuilder12.Append("</b></td>");
        stringBuilder12.Append("<td align='right' class='tbltd' style='width: 140px'><b>");
        stringBuilder12.Append(string.Format("{0:F2}", num8));
        stringBuilder12.Append("</b></td>");
        stringBuilder12.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
        stringBuilder12.Append(string.Format("{0:F2}", num9));
        stringBuilder12.Append("</b></td>");
        stringBuilder12.Append("</b></td>");
        stringBuilder12.Append("</tr>");
        stringBuilder12.Append("</table>");
        StringBuilder stringBuilder14 = new StringBuilder();
        if (Convert.ToInt32(str2) >= 2014)
        {
            if (ds.Tables[7].Rows.Count > 0)
            {
                stringBuilder13.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
                stringBuilder13.Append("<tr>");
                stringBuilder13.Append("<td style='text-align:left;' class='tblheader'><b>Adjustment</b></td>");
                stringBuilder13.Append("<td style='width: 120px' align='right' class='tblheader'></td>");
                stringBuilder13.Append("<td style='width: 140px' align='right' class='tblheader'></td>");
                stringBuilder13.Append("<td style='width: 120px' align='right' class='tblheader'></td>");
                stringBuilder13.Append("</tr>");
                foreach (DataRow row in (InternalDataCollectionBase)ds.Tables[7].Rows)
                {
                    stringBuilder13.Append("<tr>");
                    stringBuilder13.Append("<td align='left' class='tbltd'>");
                    stringBuilder13.Append(row["TransDesc"].ToString().Trim());
                    stringBuilder13.Append("</td>");
                    stringBuilder13.Append("<td align='right' class='tbltd'>");
                    stringBuilder13.Append(row["credit"].ToString().Trim());
                    stringBuilder13.Append("</td>");
                    stringBuilder13.Append("<td align='right' class='tbltd'>");
                    stringBuilder13.Append(row["Balance"].ToString().Trim());
                    stringBuilder13.Append("</td>");
                    stringBuilder13.Append("<td align='right' class='tbltd'><b>");
                    stringBuilder13.Append(row["Total"].ToString().Trim());
                    stringBuilder13.Append("</b></td>");
                    stringBuilder13.Append("</tr>");
                    num4 += Convert.ToDouble(row["credit"].ToString().Trim());
                }
                stringBuilder13.Append("<tr>");
                stringBuilder13.Append("<td align='right' class='tbltd'>");
                stringBuilder13.Append("<strong>Total &nbsp;:&nbsp; </strong>");
                stringBuilder13.Append("</td>");
                stringBuilder13.Append("<td align='right' class='tbltd'><strong>");
                stringBuilder13.Append(string.Format("{0:F2}", num4));
                stringBuilder13.Append("</strong></td>");
                stringBuilder13.Append("<td align='right' class='tbltd'><strong>");
                stringBuilder13.Append(string.Format("{0:F2}", '0'));
                stringBuilder13.Append("</strong></td>");
                stringBuilder13.Append("<td align='right' class='tbltd'><strong>");
                stringBuilder13.Append(string.Format("{0:F2}", num4));
                stringBuilder13.Append("</strong></td>");
                stringBuilder13.Append("</tr>");
                stringBuilder13.Append("</table>");
                lblAdj.Text = stringBuilder13.ToString().Trim();
                stringBuilder14.Append("<table cellpadding='1' cellspacing='1'  width='800px' border='1' style='border-collapse:collapse' >");
                stringBuilder14.Append("<tr>");
                stringBuilder14.Append("<td align='right' class='tbltd'><b>");
                stringBuilder14.Append("Grand Total &nbsp;:&nbsp; ");
                stringBuilder14.Append("</b></td>");
                stringBuilder14.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
                stringBuilder14.Append(string.Format("{0:F2}", (num7 + num4)));
                stringBuilder14.Append("</b></td>");
                stringBuilder14.Append("<td align='right' class='tbltd' style='width: 140px'><b>");
                stringBuilder14.Append(string.Format("{0:F2}", num8));
                stringBuilder14.Append("</b></td>");
                stringBuilder14.Append("<td align='right' class='tbltd' style='width: 120px'><b>");
                stringBuilder14.Append(string.Format("{0:F2}", (num9 + num4)));
                stringBuilder14.Append("</b></td>");
                stringBuilder14.Append("</b></td>");
                stringBuilder14.Append("</tr>");
                stringBuilder14.Append("</table>");
                lblGTotal.Text = stringBuilder14.ToString();
            }
            else
            {
                lblAdj.Text = string.Empty;
                lblGTotal.Text = string.Empty;
            }
        }
        else
        {
            lblAdj.Text = string.Empty;
            lblGTotal.Text = string.Empty;
        }
        if (Convert.ToDouble(num7) > 0.0 || Convert.ToDouble(num8) > 0.0)
        {
            lblTotal.Text = stringBuilder12.ToString().Trim();
            if (Convert.ToInt32(str2) >= 2014)
                Session["EstimateData"] = (stringBuilder1.ToString().Trim() + stringBuilder2.ToString().Trim() + stringBuilder5.ToString().Trim() + stringBuilder6.ToString().Trim() + stringBuilder7.ToString().Trim() + stringBuilder3.ToString().Trim() + stringBuilder4.ToString().Trim() + stringBuilder12.ToString().Trim() + stringBuilder13.ToString().Trim() + stringBuilder14.ToString().Trim());
            else
                Session["EstimateData"] = (stringBuilder1.ToString().Trim() + stringBuilder2.ToString().Trim() + stringBuilder5.ToString().Trim() + stringBuilder6.ToString().Trim() + stringBuilder7.ToString().Trim() + stringBuilder8.ToString().Trim() + stringBuilder10.ToString().Trim() + stringBuilder11.ToString().Trim() + stringBuilder9.ToString().Trim() + stringBuilder12.ToString().Trim());
            btnExpExcel.Enabled = true;
            btnPrint.Enabled = true;
        }
        else
        {
            lblAnudan.Text = string.Empty;
            lblBookMaterial.Text = string.Empty;
            lblBus.Text = string.Empty;
            lblFine.Text = string.Empty;
            lblHostel.Text = string.Empty;
            lblMiscFee.Text = string.Empty;
            lblProsFee.Text = string.Empty;
            lblReport.Text = string.Empty;
            lblTotal.Text = string.Empty;
            btnExpExcel.Enabled = false;
            btnPrint.Enabled = false;
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
                    stringBuilder.Append("<table width='800px' border='0px'><tr>");
                    stringBuilder.Append("<td colspan='4' align='center' style='font-size:20px;'><strong>" + clsDal.Decrypt(row["SchoolName"].ToString().Trim(), str2) + "</strong></td></tr>");
                    stringBuilder.Append("<tr><td colspan='4' align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address1"].ToString().Trim(), str2) + "," + clsDal.Decrypt(row["Address2"].ToString().Trim(), str2) + "</b></td></tr>");
                    stringBuilder.Append("<tr><td colspan='4' align='center' style='font-size:14px;'><b>" + clsDal.Decrypt(row["Address3"].ToString().Trim(), str2) + ",Phone-" + clsDal.Decrypt(row["Phone"].ToString().Trim(), str2) + "</b>");
                    stringBuilder.Append("</td></tr>");
                    stringBuilder.Append("<tr><td colspan='2' align='center'><strong>Estimation Report</strong>  For ");
                    stringBuilder.Append(" Session Yr " + drpSessionYr.SelectedValue.ToString() + " </td></tr>");
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
            Response.Redirect("rptEstimationPrint.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    protected void drpSessionYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        Clear();
    }

    protected void Clear()
    {
        lblReport.Text = string.Empty;
        lblFine.Text = string.Empty;
        lblBus.Text = string.Empty;
        lblHostel.Text = string.Empty;
        lblProsFee.Text = string.Empty;
        lblBookMaterial.Text = string.Empty;
        lblAnudan.Text = string.Empty;
        lblMiscFee.Text = string.Empty;
        lblTotal.Text = string.Empty;
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
                stringBuilder.Append("Estimation Report  :-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 15px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["EstimateData"].ToString().Trim()).ToString().Trim());
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
        string str = Server.MapPath("Exported_Files/Estimation Report -" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
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
}