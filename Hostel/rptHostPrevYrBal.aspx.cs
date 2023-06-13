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

public partial class Hostel_rptHostPrevYrBal : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack || Request.QueryString["Admnno"] == null)
            return;
        GetPrevDue();
    }

    private void GetPrevDue()
    {
        Common common = new Common();
        DataSet dataSet1 = new DataSet();
        Hashtable hashtable = new Hashtable();
        string currSession = new clsGenerateFee().CreateCurrSession();
        hashtable.Add("@SessionYr", currSession);
        hashtable.Add("@AdmnNo", Request.QueryString["admnno"].ToString());
        hashtable.Add("@SchoolID", int.Parse(Session["SchoolId"].ToString()));
        hashtable.Add("@CurrSessStDt", ("04/01/" + currSession.Substring(0, 4)));
        hashtable.Add("@CurrSessEndDt", ("03/31/" + currSession.Substring(0, 2) + currSession.Substring(5, 2)));
        currSession.Split('-');
        DataSet dataSet2 = common.GetDataSet("HostRptGetPrevBalance", hashtable);
        if (dataSet2.Tables[0].Rows.Count > 0)
        {
            lblName.Text = dataSet2.Tables[0].Rows[0]["FullName"].ToString();
            lblRegd.Text = dataSet2.Tables[0].Rows[0]["AdmnNo"].ToString();
            if (dataSet2.Tables[0].Rows.Count > 0)
            {
                CreateIndividualRpt(dataSet2.Tables[1], lblBalReg);
                CreateIndividualRpt(dataSet2.Tables[2], lblBalMisc);
                CreateIndividualRpt(dataSet2.Tables[3], lblPreviousFee);
                if (dataSet2.Tables[3].Rows.Count > 0)
                    PrevDue(dataSet2.Tables[3]);
            }
            TotalAmount(dataSet2);
        }
        else
            lblName.Text = "No record to display !";
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

    private void PrevDue(DataTable dt)
    {
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table cellpadding='1' cellspacing='1' style='background-color:#CCC; width:100%;'>");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='tbltd'>");
            stringBuilder.Append("Previous Year Balance :");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='tbltd' style='width: 100px'>");
            stringBuilder.Append(row["PrevBal"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
        }
        stringBuilder.Append("</table>");
        lblPreviousFee.Text = stringBuilder.ToString().Trim();
    }

    private void TotalAmount(DataSet ds)
    {
        new clsGenerateFee().CreateCurrSession().Split('-');
        double num1;
        double num2 = num1 = 0.0;
        double num3 = num1;
        double num4 = num1;
        if (ds.Tables[1].Rows.Count > 0)
            num4 = Convert.ToDouble(ds.Tables[1].Compute("SUM(balance)", "").ToString());
        if (ds.Tables[2].Rows.Count > 0)
            num3 = Convert.ToDouble(ds.Tables[2].Compute("SUM(balance)", "").ToString());
        if (ds.Tables[3].Rows.Count > 0)
            num2 = Convert.ToDouble(ds.Tables[3].Compute("SUM(PrevBal)", "").ToString());
        lblTotal.Text = "<b>Total Balance Amount : </b>" + string.Format("{0:f2}", (num4 + num3 + num2));
    }
}