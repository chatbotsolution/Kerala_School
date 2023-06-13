using ASP;
using SanLib;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Collections;

public partial class Library_rptBookDetails : System.Web.UI.Page
{
    SqlConnection con;
    SqlCommand com;
    SqlDataReader dr;


    private clsDAL obj = new clsDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            btnExport.Visible = false;
            btnPrint.Visible = false;
            Session["title"] = Page.Title.ToString();
            Form.DefaultButton = btnShow.UniqueID;
            bindDropDown(ddlCategory, "select  BriefName as CatName,CatCode from Lib_Category where CollegeId=" + Session["SchoolId"] + " order by CatName", "CatName", "CatCode");
            bindDropDown(ddlPublisher, "select PublisherName,PublisherId from Lib_Publisher where CollegeId=" + Session["SchoolId"] + " order by PublisherName", "PublisherName", "PublisherId");
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--All--", "0"));
    }

    private void generateReport()
    {
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder = new StringBuilder();
        try
        {
            DataTable dataForReport = getDataForReport();
            if (dataForReport.Rows.Count > 0)
            {
                stringBuilder.Append("<table width='100%' border='1px' cellspacing='0' cellpadding='2px' class='tbltxt'>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='text-align:center; font-size:large; font-weight:bold' colspan='7'>Stock List Of Books</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr style='font-weight:bold; text-align:left'>");                
                stringBuilder.Append("<td width='50px'>Sl No.</td>");
                //stringBuilder.Append("<td width='100px'>Accession No</td>");
                stringBuilder.Append("<td width='200px'>Book Name</td>");
                stringBuilder.Append("<td width='200px'>Subject</td>");
                stringBuilder.Append("<td width='200px'>Publisher</td>");
                stringBuilder.Append("<td>Author</td>");
                stringBuilder.Append("<td width='50px'>Qty</td>");
                stringBuilder.Append("<td width='90px'>WritesOff Qty</td>");
                stringBuilder.Append("</tr>");
                int num1 = 1;
                string str = dataForReport.Rows[0]["bookid"].ToString();
                int num2=0;
                int num3=0;
                int num4=0;
                int num5=0;
                if (dataForReport.Rows[0]["status"].ToString() == "I" || dataForReport.Rows[0]["status"].ToString() == "R")
                {
                    num3 = num2 = int.Parse(dataForReport.Rows[0]["Qty"].ToString());
                    num5 = num4 = 0;
                }
                else
                {
                    num3 = num2 = 0;
                    num5 = num4 = int.Parse(dataForReport.Rows[0]["Qty"].ToString());
                }
                int index;
                for (index = 1; index < dataForReport.Rows.Count; ++index)
                {
                    if (str != dataForReport.Rows[index]["bookid"].ToString())
                    {
                        stringBuilder.Append("<tr style='text-align:left'>");
                        stringBuilder.Append("<td>" + num1.ToString() + "</td>");
                        //stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AccessionNo"].ToString() + "</td>");
                        stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BookTitle"].ToString() + "</td>");
                        stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["SubName"].ToString() + "</td>");
                        stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PublisherName"].ToString() + "</td>");
                        stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName1"].ToString() + "</td>");
                        stringBuilder.Append("<td>" + num2 + "</td>");
                        stringBuilder.Append("<td>" + num4 + "</td>");
                        stringBuilder.Append("</tr>");
                        ++num1;
                        str = dataForReport.Rows[index]["bookid"].ToString();
                        if (dataForReport.Rows[index]["status"].ToString() == "I" || dataForReport.Rows[index]["status"].ToString() == "R")
                        {
                            num2 = int.Parse(dataForReport.Rows[index]["Qty"].ToString());
                            num3 += num2;
                            num4 = 0;
                        }
                        else
                        {
                            num2 = 0;
                            num4 = int.Parse(dataForReport.Rows[index]["Qty"].ToString());
                            num5 += num4;
                        }
                    }
                    else if (dataForReport.Rows[index]["status"].ToString() == "I" || dataForReport.Rows[index]["status"].ToString() == "R")
                    {
                        num2 += int.Parse(dataForReport.Rows[index]["Qty"].ToString());
                        num3 += int.Parse(dataForReport.Rows[index]["Qty"].ToString());
                    }
                    else
                    {
                        num4 += int.Parse(dataForReport.Rows[index]["Qty"].ToString());
                        num5 += int.Parse(dataForReport.Rows[index]["Qty"].ToString());
                    }


                }
                stringBuilder.Append("<tr style='text-align:left'>");
                stringBuilder.Append("<td>" + num1.ToString() + "</td>");
                //stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AccessionNo"].ToString() + "</td>");
                stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BookTitle"].ToString() + "</td>");
                stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["SubName"].ToString() + "</td>");
                stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PublisherName"].ToString() + "</td>");
                stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName1"].ToString() + "</td>");
                stringBuilder.Append("<td>" + num2 + "</td>");
                stringBuilder.Append("<td>" + num4 + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td style='text-align:right; font-weight:bold' colspan='5'>Total =</td>");
                stringBuilder.Append("<td style='font-weight:bold'>" + num3 + "</td>");
                stringBuilder.Append("<td style='font-weight:bold'>" + num5 + "</td>");
                stringBuilder.Append("</tr>");
                lblReport.Text = stringBuilder.ToString();
                stringBuilder.Append("</table>");
                btnPrint.Visible = true;
                btnExport.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
                btnExport.Visible = false;
                lblReport.Text = "No Data Found";
            }
            if (stringBuilder.ToString() != "")
                Session["bookReport"] = stringBuilder.ToString();
            else
                Session["bookReport"] = null;
        }
        catch (Exception ex)
        {
        }
    }

    private DataTable getDataForReport()
    {
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select A.bookid,C.CatCode,C.BriefName,S.SubjectId,S.SubName,B.BookTitle,B.AuthorName1,B.AuthorName2,B.AuthorName3,");
        stringBuilder.Append("P.PublisherId,P.PublisherName,count(*) Qty,A.status from dbo.Lib_BookAccession A ");
        stringBuilder.Append(" inner join dbo.Lib_BookMaster B on B.bookid=A.bookid");
        stringBuilder.Append(" inner join dbo.Lib_Category C on C.CatCode=B.CatCode");
        stringBuilder.Append(" inner join dbo.Lib_Subjects S on S.SubjectId=B.SubjectId");
        stringBuilder.Append(" inner join dbo.Lib_Publisher P on P.PublisherId=B.PublisherId");
        stringBuilder.Append(" where A.CollegeId=" + Session["SchoolId"]);
        if (ddlCategory.SelectedIndex > 0)
            stringBuilder.Append(" and C.CatCode='" + ddlCategory.SelectedValue + "'");
        if (ddlSubject.SelectedIndex > 0)
            stringBuilder.Append(" and S.SubjectId='" + ddlSubject.SelectedValue + "'");
        if (txtAuthor.Text.Trim() != "")
            stringBuilder.Append(" and B.AuthorName1='" + txtAuthor.Text.Trim() + "' or B.AuthorName2='" + txtAuthor.Text.Trim() + "' or B.AuthorName3='" + txtAuthor.Text.Trim() + "'");
        if (ddlPublisher.SelectedIndex > 0)
            stringBuilder.Append(" and P.PublisherId='" + ddlPublisher.SelectedValue + "'");
        stringBuilder.Append(" group by A.bookid, B.BookTitle,B.AuthorName1,B.AuthorName2,B.AuthorName3,C.CatCode,C.BriefName,S.SubjectId,S.SubName,A.status,P.PublisherId,P.PublisherName ");
        return obj.GetDataTableQry(stringBuilder.ToString());
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (Session["bookReport"] != null)
        {
            try
            {
                ExportToExcel(Session["bookReport"].ToString());
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }
        else
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('No data exists to export !');", true);
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/Book_Details_Report_" + DateTime.Now.ToString("ddMMyyyyhhmmssfffffff") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Stock_Details_Report_" + DateTime.Now.ToString("ddMMyyyyhhmmssfffffff") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCategory.SelectedIndex > 0)
        {
            bindDropDown(ddlSubject, "select SubName,SubjectId from Lib_Subjects where CatCode=" + ddlCategory.SelectedValue + " and CollegeId=" + Session["SchoolId"] + " order by SubName", "SubName", "SubjectId");
        }
        else
        {
            ddlSubject.Items.Clear();
            ddlSubject.Items.Insert(0, new ListItem("--All--", "0"));
        }
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        generateReport();
    }

    protected void txtAuthor_TextChanged(object sender, EventArgs e)
    {
        //con = new SqlConnection("Data Source=XPRO-PC;Initial Catalog=Loyola;user=sa;password=123");
        //con.Open();
        //com = new SqlCommand("select AuthorName1 from Lib_BookMaster where AuthorName1 like '" + txtAuthor.Text + "'+'%' ", con);
        //dr = com.ExecuteReader();
        //if (dr.Read())
        //{
        //    txtAuthor.Text = dr["AuthorName1"].ToString();
        //}
        //con.Close();
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> AutoCompleteLib(string prefixText, int count, string contextKey)
    {

        int id = Convert.ToInt32(contextKey);

        List<string> newlist = new List<string>();
        Hashtable ht = new Hashtable();
        ht.Add("@Term", prefixText);
        switch (id)
        {
            case 51:
                ht.Add("@Flag", 51);
                break;
            case 52:
                ht.Add("@Flag", 52);
                break;
            default:
                ht.Add("@Flag", 0);
                break;

        }
        // ht.Add("@Flag", 1);
        DataTable dt = new clsDAL().GetDataTable("AutoComplete", ht);
        foreach (DataRow row in dt.Rows)
        {
            newlist.Add(row["Result"].ToString());
        }
        return newlist;
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {

    }
}