using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Globalization;

public partial class Library_BookReturn : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

           
           hfFines.Value = "0";
           txtAmntRcvd.Enabled = false;
           txtNarration.Enabled = false;
           lblmsg.Text = string.Empty;
         
           if (Page.IsPostBack)
                 return;
           else
               txtReturnDt.Text = DateTime.Now.ToString("dd-MMM-yyyy");
           lblFineAmnt.Text = "0.00";
           Session["title"] = Page.Title.ToString();
           
            if (Session["User_Id"] != null)
            {
               lblMemberName.Text =Request.QueryString["Name"].ToString();
               lblAccNo.Text =Request.QueryString["Accno"].ToString();
               lblBookTitle.Text =Request.QueryString["BookTitle"].ToString();
               hdnMemberId.Value =Request.QueryString["MemId"].ToString();
               lblIssudt.Text = DateTime.Parse(Request.QueryString["issuedt"]).ToString("dd-MMM-yyyy");
               lblDuedt.Text = DateTime.Parse(Request.QueryString["duedt"]).ToString("dd-MMM-yyyy"); 
                dtpReturnDt.From.Date=DateTime.Parse(Request.QueryString["issuedt"]);
               CreateHolidayCalendar();
            }
            else
               Response.Redirect("../Login.aspx");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (lblFineAmnt.Text != "0.00" &&lblFineAmnt.Text != "No Dues Available !")
           FinePaid();
       ReturnBook();
       Clear();
       lblmsg.Text = "Information Saved Successfully !";
       lblmsg.ForeColor = Color.Green;
    }

    private void FinePaid()
    {
        try
        {
            if (txtReturnDt.Text != "")
                ht.Add("@FinePaidDate", dtpReturnDt.SelectedDate.ToString());
           ht.Add("@LibMemberId", int.Parse(hdnMemberId.Value.ToString()));
            if (txtNarration.Text != "")
               ht.Add("@Narration", txtNarration.Text.ToString());
           ht.Add("@Debit", double.Parse(lblFineAmnt.Text.ToString()));
           ht.Add("@Credit", double.Parse(txtAmntRcvd.Text.ToString()));
           ht.Add("@FineAmtBalance", double.Parse(lblFineAmnt.Text.ToString()));
           ht.Add("@CollegeId",Session["SchoolId"]);
           dt.Clear();
           dt =obj.GetDataTable("Lib_InsFineLedger",ht);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void ReturnBook()
    {
        try
        {
           ht.Clear();
            if (Request.QueryString["IRId"] == null)
               ht.Add("@IRId", 0);
            else
               ht.Add("@IRId", int.Parse(Request.QueryString["IRId"].ToString()));
           ht.Add("@AccessionNo", lblAccNo.Text);
            if (txtReturnDt.Text != "")
                ht.Add("@ReturnDate", dtpReturnDt.SelectedDate.ToString());
           ht.Add("@UserId", 1);
           ht.Add("@CollegeId",Session["SchoolId"]);
           dt.Clear();
           dt =obj.GetDataTable("Lib_SP_InsUpdtReturn",ht);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void Clear()
    {
       txtReturnDt.Text = string.Empty;
       lblmsg.Text = string.Empty;
       hfFines.Value = string.Empty;
       hfFineAmnt.Value = string.Empty;
       txtAmntRcvd.Text = string.Empty;
       txtNarration.Text = string.Empty;
       lblFineAmnt.Text = "0.00";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
       Clear();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
       Response.Redirect("IssuePendingList.aspx");
    }

    protected void btnCalFine_Click(object sender, EventArgs e)
    {
        try
        {
            if (!txtReturnDt.Text.Trim().Equals(string.Empty))
            {
               hfFines.Value = "1";
               obj = new clsDAL();
               ht = new Hashtable();
               ht.Add("@AccessionNo", lblAccNo.Text.ToString());
               ht.Add("@MemberId", int.Parse(hdnMemberId.Value.ToString()));
               ht.Add("@ReturnDate", dtpReturnDt.GetDateValue());
                string str =obj.ExecuteScalar("Lib_SP_CalFineAmount",ht);
                if (str != "")
                {
                   lblFineAmnt.Text = str;
                   hfFineAmnt.Value = str;
                   txtAmntRcvd.Enabled = true;
                   txtNarration.Enabled = true;
                  
                }
                else
                {
                   lblFineAmnt.Text = "No Dues Available !";
                   lblFineAmnt.ForeColor = Color.Green;
                   txtAmntRcvd.Enabled = false;
                   txtNarration.Enabled = false;
                  
                }
              // CreateHolidayCalendar();
            }
            else
               ClientScript.RegisterClientScriptBlock(btnCalFine.GetType(), "Message", "alert('Please provide date of return !');", true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
       
    }
    protected void dtpReturnDt_Changed(object sender, EventArgs e)
    {
        CreateHolidayCalendar();
    }
    protected void CreateHolidayCalendar()
    {

        DateTime Duedt = DateTime.Parse(Request.QueryString["duedt"]);
        DateTime returndt = DateTime.Parse(txtReturnDt.Text.Trim());
        DataTable dt = new clsDAL().GetDataTableQry("Select *,CASE WHEN ToDate IS NULL OR ToDate='' Or ToDate=FromDate THEN Convert(varchar,Fromdate,105) ELSE Convert(varchar,Fromdate,106)+' To '+Convert(varchar,Todate,106) End As Date from Lib_Holidaylist where FromDate >='" + Duedt + "' and FromDate < '" + returndt + "' and ToDate >='" + Duedt + "' and ToDate < '" + returndt + "' ");
        if (dt.Rows.Count > 0)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("<table cellpadding='2px' cellspacing='2px' align='center' width='100%' class='tbltxt'>");
            sb.Append("<tr style='background-color:#ccc'>");
            sb.Append("<th>DATE</th>");
            sb.Append("<th>DAY</th>");
            sb.Append("<th>Description</th>");
            sb.Append("</tr>");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                sb.Append("<th>" + row["Date"].ToString() + "</th>");
                sb.Append("<th>" + row["HDay"].ToString() + "</th>");
                sb.Append("<th>" + row["Descrip"].ToString() + "</th>");
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            lblCalendar.Text = sb.ToString();
        }
        else
            lblCalendar.Text = "";
    }
}