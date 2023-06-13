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

public partial class Reports_rptClassStrength : System.Web.UI.Page
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
        DataTable dt = new clsDAL().GetDataTableQry("Select distinct AdmnSessYr From Ps_StudMaster Order by AdmnSessYr Desc");
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

   
    protected void GetDetails()
    {
        int totavlbl = 0;
        int totcount = 0;
        int totVacant = 0;
        ViewState["PrintData"] = "";
        string row = "";
        StringBuilder sb = new StringBuilder();
        DataTable datatable = new DataTable();
        DataTable dt = GetClass();
        if (dt.Rows.Count > 0)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string sec = "";
                string cls = dt.Rows[i]["ClassName"].ToString();
                int clsid = Convert.ToInt32(dt.Rows[i]["ClassId"].ToString());
                int secNo = Convert.ToInt32(dt.Rows[i]["NoOfSections"].ToString());
                if (secNo <= 0)
                    return;
                int num = 65;
                while (secNo > 0)
                {
                    sec = Convert.ToChar(num).ToString();
                    string query = "Select Count(*) as Count From Ps_studMaster s inner join Ps_classwiseStudent cs on s.admnno=cs.admnno where cs.classid=" + clsid + " and cs.section='" + sec + "' and cs.SessionYear='" + ddlSession.SelectedValue.ToString().Trim() + "'";
                    datatable = new clsDAL().GetDataTableQry("Select Count(*) as Count From Ps_studMaster s inner join Ps_classwiseStudent cs on s.admnno=cs.admnno where cs.classid="+ clsid+" and cs.section='"+ sec +"' and cs.SessionYear='" + ddlSession.SelectedValue.ToString().Trim() + "' and cs.StatusId=1");
                    if (datatable.Rows.Count > 0)
                    {
                       
                        int count = Convert.ToInt32(datatable.Rows[0]["Count"].ToString());
                        row=BuildTable(count,cls,clsid,sec);
                        sb.Append(row);
                    }
                    else
                        return;
                    num++;
                    secNo--;
                    totavlbl = totavlbl + Convert.ToInt32(ViewState["TAvl"].ToString());
                    totcount = totcount + Convert.ToInt32(ViewState["TCount"].ToString());
                    totVacant = totVacant + Convert.ToInt32(ViewState["TVac"].ToString());
                }

            }
        }
        sb.Append("<tr><td colspan='2' style='width:100px; text-align:right; height:20px;'><strong>Total</strong></td><td style='width:100px; text-align:center; font-weight:bold;'>" + totavlbl + "</td><td style='width:100px; text-align:center;font-weight:bold;'>" + totcount + "</td><td style='width:100px; text-align:center;font-weight:bold;'>" + totVacant + "</td>");
        ViewState["PrintData"] = sb.ToString();
        PrintForm();
        
    }

    private string BuildTable(int count, string classname,int classid, string section)
    {
        int totavlbl = 0;
        int totcount = 0;
        int totVacant = 0;
        int avlbl = 0;
        int vacant = 0;
       
        StringBuilder stringbuilder = new StringBuilder();
        stringbuilder.Append("<tr>");
        stringbuilder.Append("<td style='width:100px; text-align:center;'>");
        stringbuilder.Append(""+classname+"");
        stringbuilder.Append("</td>");
        stringbuilder.Append("<td style='width:100px; text-align:center;'>");
        stringbuilder.Append("" + section + "");
        stringbuilder.Append("</td>");
        if (classid == 1)
        {
            avlbl = 40;
            
        }
        else
            avlbl = 52;
        totavlbl = totavlbl + avlbl;
        totcount = totcount + count;
        stringbuilder.Append("<td style='width:100px; text-align:center;'>" + avlbl + "</td>");
        stringbuilder.Append("<td style='width:100px; text-align:center;'>" + count + "</td>");

        vacant = avlbl - count;
        totVacant = totVacant + vacant;
        stringbuilder.Append("<td style='width:100px; text-align:center;'>" + vacant + "</td>");
        stringbuilder.Append("</tr>");
       

        ViewState["TAvl"] = totavlbl.ToString();
        ViewState["TCount"] = totcount.ToString();
        ViewState["TVac"] = totVacant.ToString();
        return stringbuilder.ToString();
    }
    private void PrintForm()
    {
        StringBuilder stringBuilder1 = new StringBuilder();
        stringBuilder1.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
        stringBuilder1.Append("<tr style='background-color:#CCCCCC'>");
        stringBuilder1.Append("<td style='width:100px; text-align:center;'>");
        stringBuilder1.Append("<strong> Class</strong>");
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("<td style='width:100px; text-align:center;'>");
        stringBuilder1.Append("<strong> Section</strong>");
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("<td style='width:100px; text-align:center;'>");
        stringBuilder1.Append("<strong> Available Seats</strong>");
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("<td style='width:100px; text-align:center;'>");
        stringBuilder1.Append("<strong> Occupied</strong>");
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("<td style='width:100px; text-align:center;'>");
        stringBuilder1.Append("<strong> Vacant</strong>");
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("</tr>");
        stringBuilder1.Append(ViewState["PrintData"].ToString());
        LblReport.Text = stringBuilder1.ToString();
        Session["ClassStrength"] = stringBuilder1.ToString();

    }

    //protected void btnPrint_Clicked(object sender, EventArgs e)
    //{
    //     Response.Redirect("rptClassStrengthPrint.aspx");
    //}

  
}