using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ASP;
using SanLib;
using System.Data;
using System.IO;
using System.Text;

public partial class Reports_rptSixthSubjectList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        Fillsession();
        btnPrint.Visible = false;
    }
    private DataTable GetClass()
    {
        DataTable dt = new clsDAL().GetDataTableQry("Select ClassId,ClassName,NoOfSections from Ps_ClassMaster");
        return dt;

    }

    private void Fillsession()
    {
        DataTable dt = new clsDAL().GetDataTableQry("Select distinct AdmnSessYr From Ps_StudMaster order by AdmnSessYr desc");
        ddlSession.DataSource = dt;
        ddlSession.DataTextField = "AdmnSessYr";
        ddlSession.DataValueField = "AdmnSessYr";
        ddlSession.DataBind();

    }
    protected void btnShow_Clicked(object sender, EventArgs e)
    {
        GetDetails();
        btnPrint.Visible = true;
    }

    private void GetDetails()
    {

        DataTable datatable = new DataTable();
        DataTable dt = GetClass();
        StringBuilder stringBuilder1 = new StringBuilder();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sec = "";
                string cls = dt.Rows[i]["ClassName"].ToString();
                int clsid = Convert.ToInt32(dt.Rows[i]["ClassId"].ToString());
                if (clsid == 11 || clsid == 12)
                {
                    int secNo = Convert.ToInt32(dt.Rows[i]["NoOfSections"].ToString());
                    if (secNo <= 0)
                        return;
                    int num = 65;
                    while (secNo > 0)
                    {
                        sec = Convert.ToChar(num).ToString();
                        string data = GetQuery(clsid, cls, sec);
                        secNo--;
                        num++;
                        stringBuilder1.Append(data);
                        // string nt =ViewState["Data"].ToString();
                    }
                }
            }

        }

        string text = stringBuilder1.ToString();
        Printform(text);
    }

    private string GetQuery(int clsid, string cls, string sec)
    {
        string lang = "";
        Hashtable ht = new Hashtable();
        ht.Add("Session", ddlSession.SelectedValue.Trim());
        ht.Add("Section", sec);
        ht.Add("Classid", clsid);
        ht.Add("@num1", txtFirstNo.Text.Trim());
        ht.Add("@num2", txtSecondNo.Text.Trim());
        DataTable dt = new clsDAL().GetDataTable("SixthSubjectList", ht);

        if (dt.Rows[0]["ECO"].ToString() != "0" && dt.Rows[0]["COMP"].ToString() != "0")
        {
            lang = "Combo";
        }
        else if (dt.Rows[0]["ECO"].ToString() == "0" && dt.Rows[0]["COMP"].ToString() != "0")
        {
            lang = "Computer Application";
        }
        else if (dt.Rows[0]["ECO"].ToString() != "0" && dt.Rows[0]["COMP"].ToString() == "0")
        {
            lang = "Economics Application";
        }
        StringBuilder stringBuilder1 = new StringBuilder();
        if (txtFirstNo.Text.Trim() == "" && txtSecondNo.Text.Trim() == "")
        {

            stringBuilder1.Append("<tr><td>" + cls + "-" + sec + "</td>");
            stringBuilder1.Append("<td>" + lang + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["Total"].ToString() + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["ECO"].ToString() + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["firstECO"].ToString() + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["SecondECO"].ToString() + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["COMP"].ToString() + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["firstCOMP"].ToString() + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["SecondCOMP"].ToString() + "</td></tr>");
        }
        else if (txtSecondNo.Text.Trim() == "" && txtFirstNo.Text.Trim() != "")
        {
            stringBuilder1.Append("<tr><td>" + cls + "-" + sec + "</td>");
            stringBuilder1.Append("<td>" + lang + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["Total"].ToString() + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["ECO"].ToString() + "</td>");
            stringBuilder1.Append("<td colspan='2'>" + dt.Rows[0]["firstECO"].ToString() + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["COMP"].ToString() + "</td>");
            stringBuilder1.Append("<td colspan='2'>" + dt.Rows[0]["firstCOMP"].ToString() + "</td></tr>");

        }
        else if (txtSecondNo.Text.Trim() != "" && txtFirstNo.Text.Trim() != "")
        {

            stringBuilder1.Append("<tr><td>" + cls + "-" + sec + "</td>");
            stringBuilder1.Append("<td>" + lang + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["Total"].ToString() + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["ECO"].ToString() + "</td>");
            stringBuilder1.Append("<td colspan='2'>" + dt.Rows[0]["firstECO"].ToString() + "</td>");
            stringBuilder1.Append("<td>" + dt.Rows[0]["COMP"].ToString() + "</td>");
            stringBuilder1.Append("<td colspan='2'>" + dt.Rows[0]["firstCOMP"].ToString() + "</td></tr>");


        }
        return stringBuilder1.ToString();
    }
    protected void btnPrint_Clicked(object sender, EventArgs e)
    {
        Response.Redirect("rptSixthSubjectPrint.aspx");
    }

    protected string AddRow()
    {
        string no1 = "";
        string no2 = "";
        StringBuilder stringBuilder1 = new StringBuilder();
        if (txtFirstNo.Text.Trim() == "" && txtSecondNo.Text.Trim() == "")
        {

            stringBuilder1.Append("<tr style='background-color:#f2f2f2; font-weight:bold;' ><td colspan='3'></td>");
            stringBuilder1.Append("<td>Total</td>");
            stringBuilder1.Append("<td>1-25</td>");
            stringBuilder1.Append("<td>26-Above</td>");
            stringBuilder1.Append("<td>Total</td>");
            stringBuilder1.Append("<td>1-25</td>");
            stringBuilder1.Append("<td>26-Above</td></tr>");
        }
        else if (txtSecondNo.Text.Trim() == "" && txtFirstNo.Text.Trim() != "")
        {
            no1 = txtFirstNo.Text;
            stringBuilder1.Append("<tr  style='background-color:#f2f2f2; font-weight:bold;'><td colspan='3'></td>");

            stringBuilder1.Append("<td>Total</td>");
            stringBuilder1.Append("<td colspan='2'>" + no1 + " -Above</td>");
            stringBuilder1.Append("<td>Total</td>");
            stringBuilder1.Append("<td colspan='2'>" + no1 + "-Above</td></tr>");
        }
        else if (txtSecondNo.Text.Trim() != "" && txtFirstNo.Text.Trim() != "")
        {
            no1 = txtFirstNo.Text;
            no2 = txtSecondNo.Text;
            stringBuilder1.Append("<tr  style='background-color:#f2f2f2; font-weight:bold;'><td colspan='3'></td>");

            stringBuilder1.Append("<td>Total</td>");
            stringBuilder1.Append("<td colspan='2'>" + no1 + "-" + no2 + "</td>");
            stringBuilder1.Append("<td>Total</td>");
            stringBuilder1.Append("<td colspan='2'>" + no1 + "-" + no2 + "</td></tr>");
        }
        return stringBuilder1.ToString();
    }

    protected void Printform(string text)
    {
        StringBuilder stringBuilder1 = new StringBuilder();
        stringBuilder1.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black; text-align:center;' class='tbltxt'  width='100%'>");
        stringBuilder1.Append("<tr style='background-color:#cccccc'><th width='15%'>Class</th><th width='15%'>Sixth Subject</th><th width='15%'>Occupancy</th><th colspan='3'>Economics Appl.</th><th colspan='3'>Computer Appl.</th><tr>");

        stringBuilder1.Append(AddRow());
        stringBuilder1.Append(text);
        LblReport.Text = stringBuilder1.ToString();
        Session["SixthSubjectList"] = stringBuilder1.ToString();
    }
}