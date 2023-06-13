using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Reports_rptFeeBalance : System.Web.UI.Page
{
    private string FullSess = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Request.QueryString["admnno"] == null)
            return;
        GetBalanceAmount();
    }

    private void GetBalanceAmount()
    {
        Common common = new Common();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        string currSession = new clsGenerateFee().CreateCurrSession();
        hashtable.Add("@SessionYr", currSession);
        FullSess = Request.QueryString["FP"];
        if (FullSess != null)
            hashtable.Add("@FullSess", Request.QueryString["FP"].ToString());
        hashtable.Add("@AdmnNo", Request.QueryString["admnno"].ToString());
        hashtable.Add("@SchoolID", int.Parse(Session["SchoolId"].ToString()));
        hashtable.Add("@CurrSessStDt", ("04/01/" + currSession.Substring(0, 4)));
        hashtable.Add("@CurrSessEndDt", ("03/31/" + currSession.Substring(0, 2) + currSession.Substring(5, 2)));
        if (Convert.ToInt32(currSession.Split('-')[0]) >= 2014)
        {
            DataSet dataSet2 = common.GetDataSet("Ps_Sp_GetBalanceAmountNew", hashtable);
            if (dataSet2.Tables[0].Rows.Count > 0)
            {
                lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
                lblRegd.Text = dataSet2.Tables[0].Rows[0]["OldAdmnNo"].ToString();
                if (dataSet2.Tables[0].Rows.Count > 0)
                {
                    CreateIndividualRpt(dataSet2.Tables[1], lblBalReg);
                  //  CreateIndividualRpt(dataSet2.Tables[2], lblBalMisc);
                  //  CreateIndividualRpt(dataSet2.Tables[3], lblBalBus);
                    //if (FullSess != null)
                    //    CreateIndividualRpt(dataSet2.Tables[5], lblBalBook);
                    //else
                    //    CreateIndividualRpt(dataSet2.Tables[6], lblBalBook);
                    //if (Request.QueryString["FP"] == null)
                    //{
                    //    if (dataSet2.Tables[5].Rows.Count > 0)
                    //        CreateIndividualRpt(dataSet2.Tables[5], lblAdvancePaid);
                    //}
                    //else
                    //    lblAdvancePaid.Text = "No record to display !";
                }
                TotalAmount(dataSet2);
            }
            else
                lblName.Text = "No record to display !";
        }
        else
        {
            DataSet dataSet2 = common.GetDataSet("Ps_Sp_GetBalanceAmount", hashtable);
            if (dataSet2.Tables[0].Rows.Count > 0)
            {
                lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
                lblRegd.Text = dataSet2.Tables[0].Rows[0]["AdmnNo"].ToString();
                if (dataSet2.Tables[0].Rows.Count > 0)
                {
                    CreateIndividualRpt(dataSet2.Tables[1], lblBalReg);
                    //CreateIndividualRpt(dataSet2.Tables[2], lblBalMisc);
                    //CreateIndividualRpt(dataSet2.Tables[3], lblBalBus);
                    //CreateIndividualRpt(dataSet2.Tables[7], lblBalBook);
                    //CreateIndividualRpt(dataSet2.Tables[5], lblFine);
                    //if (Request.QueryString["FP"] == null)
                    //{
                    //    if (dataSet2.Tables[6].Rows.Count > 0)
                    //        CreateIndividualRpt(dataSet2.Tables[6], lblAdvancePaid);
                    //}
                    //else
                    //    lblAdvancePaid.Text = "No record to display !";
                }
                TotalAmount(dataSet2);
            }
            else
                lblName.Text = "No record to display !";
        }
    }

    private void CreateIndividualRpt(DataTable dt, Label lbl)
    {
        if (dt.Rows.Count > 0)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
            stringBuilder.Append("<tr style='background-color: #666; color: White;'>");
            stringBuilder.Append("<td style='width: 120px; background-color: #666; color: White;' align='right' class='tbltxt'>Transaction Date</td>");
            stringBuilder.Append("<td  align='left' class='tbltxt' style='background-color: #666; color: White;'>Fee Name</td>");
            stringBuilder.Append("<td style='background-color: #666; color: White; width: 100px' align='right' class='tbltxt'>Balance Amount</td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='right' class='tbltd'>");
                stringBuilder.Append(row["TransDt"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='left' class='tbltd'>");
                stringBuilder.Append(row["FeeName"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("<td align='right' class='tbltd'>");
                stringBuilder.Append(row["balance"].ToString().Trim());
                stringBuilder.Append("</td>");
                stringBuilder.Append("</tr>");
            }
            string empty = string.Empty;
            string str = dt.Compute("SUM(balance)", "").ToString();
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='right' class='tbltd' style='Font-Size:12px;' colspan='2'><b>Total Balance :</b></td>");
            stringBuilder.Append("<td align='right' class='tbltd'><b>" + str.ToString() + "</b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            lbl.Text = stringBuilder.ToString().Trim();
        }
        else
            lbl.Text = "No balance !";
    }

    private void TotalAmount(DataSet ds)
    {
        string[] strArray = new clsGenerateFee().CreateCurrSession().Split('-');
        double num1;
        double num2 = num1 = 0.0;
        double num3 = num1;
        double num4 = num1;
        double num5 = num1;
        double num6 = num1;
        double num7 = num1;
        if (Convert.ToInt32(strArray[0]) >= 2014)
        {
            if (ds.Tables[1].Rows.Count > 0)
                num7 = Convert.ToDouble(ds.Tables[1].Compute("SUM(balance)", "").ToString());
            if (ds.Tables[2].Rows.Count > 0)
                num6 = Convert.ToDouble(ds.Tables[2].Compute("SUM(balance)", "").ToString());
            if (ds.Tables[3].Rows.Count > 0)
                num5 = Convert.ToDouble(ds.Tables[3].Compute("SUM(balance)", "").ToString());
            if (FullSess != null)
            {
                if (ds.Tables[5].Rows.Count > 0)
                    Convert.ToDouble(ds.Tables[6].Compute("SUM(balance)", "").ToString());
            }
            else if (ds.Tables[6].Rows.Count > 0)
                Convert.ToDouble(ds.Tables[6].Compute("SUM(balance)", "").ToString());
            if (ds.Tables[4].Rows.Count > 0)
            {
                num4 = Convert.ToDouble(ds.Tables[4].Compute("SUM(AdvFee)", "").ToString());
               // lblAdvance.Text = "Total Advance Amount :" + num4;
            }
            if (Request.QueryString["FP"] == null && ds.Tables[5].Rows.Count > 0)
                num2 = Convert.ToDouble(ds.Tables[5].Compute("SUM(balance)", "").ToString());
        }
        else
        {
            if (ds.Tables[1].Rows.Count > 0)
                num7 = Convert.ToDouble(ds.Tables[1].Compute("SUM(balance)", "").ToString());
            if (ds.Tables[2].Rows.Count > 0)
                num6 = Convert.ToDouble(ds.Tables[2].Compute("SUM(balance)", "").ToString());
            if (ds.Tables[3].Rows.Count > 0)
                num5 = Convert.ToDouble(ds.Tables[3].Compute("SUM(balance)", "").ToString());
            if (ds.Tables[7].Rows.Count > 0)
                Convert.ToDouble(ds.Tables[7].Compute("SUM(balance)", "").ToString());
            if (ds.Tables[4].Rows.Count > 0)
            {
                num4 = Convert.ToDouble(ds.Tables[4].Compute("SUM(AdvFee)", "").ToString());
               // lblAdvance.Text = "Total Advance Amount :" + num4;
            }
            if (ds.Tables[5].Rows.Count > 0)
                num3 = Convert.ToDouble(ds.Tables[5].Compute("SUM(balance)", "").ToString());
            if (Request.QueryString["FP"] == null && ds.Tables[6].Rows.Count > 0)
                num2 = Convert.ToDouble(ds.Tables[6].Compute("SUM(balance)", "").ToString());
        }
        lblTotal.Text = "<b>Total Balance Amount : </b>" + string.Format("{0:f2}", (num7 + num6 + num5 + num4 + num3 + num2));
    }
}