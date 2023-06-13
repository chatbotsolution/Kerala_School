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

public partial class Library_CatalogReport : System.Web.UI.Page
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
        }
        else
            Response.Redirect("../Login.aspx");
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
                   
                    foreach (DataRow row in (InternalDataCollectionBase)dataForReport.Rows)
                    {

                    stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;;' class='tbltxt'  width='50%'>");

                    stringBuilder.Append("<tr>");
                    stringBuilder.Append("<td width='40px' align='center' class='tbltxt'>" + row["Reference"] + "");
                    stringBuilder.Append("</br>" + row["Classi_No"] + "");
                    stringBuilder.Append("</br>" + row["Book_No"] + "</td>");
                        if (ddlCatalogue.SelectedValue == "1")
                            stringBuilder.Append("<td width='200px' class='tbltxt' style ='font-size:15px'>&nbsp&nbsp" + row["SubName"].ToString().ToUpper() + "</br>");

                        else if (ddlCatalogue.SelectedValue == "2")
                            stringBuilder.Append("<td width='200px' class='tbltxt' style ='font-size:15px'>"+ row["BookTitle"] + "</br>");

                        else
                        {
                            stringBuilder.Append("<td width='200px' class='tbltxt' style ='font-size:15px'></br>");
                        }

                        stringBuilder.Append("</br>"+row["AuthorLastName1"].ToString()+"&nbsp");
                        stringBuilder.Append(row["AuthorName1"] + "</td>");
                        stringBuilder.Append("</tr>");

                        stringBuilder.Append("<tr>");
                        stringBuilder.Append("<td  width='40px' align='center' class='tbltxt'>"+ row["AccessionNo"] + "</td>");

                        if (ddlCatalogue.SelectedValue == "2")
                        {
                            stringBuilder.Append("<td width='200px' class='tbltxt' style ='font-size:15px;'>&nbsp&nbsp" + row["BookTitle"] +" : "+  row["BookSubTitle"]);
                        }
                        else
                        {
                            stringBuilder.Append("<td width='200px' class='tbltxt' style ='font-size:15px;'>&nbsp&nbsp" + row["BookTitle"]);
                        }

                        //stringBuilder.Append("<td width='200px' class='tbltxt' style ='font-size:15px;'>&nbsp&nbsp" + row["BookTitle"]);
                        stringBuilder.Append("</br>" + row["AuthorName1"] + "&nbsp " + row["AuthorLastName1"] +"");
                        stringBuilder.Append("</br>" + row["AuthorName2"] + "&nbsp" + row["AuthorLastName2"] + " ");

                        if (row["AuthorName3"] == "")
                        {
                        }
                        else
                        {
                        stringBuilder.Append("</br>Edited by :" + row["AuthorName3"] + " ");
                        }
                        stringBuilder.Append("</br>Edition : " + row["Edition"] +  ", Place :  " + row["PubPlace"] + ", ");
                        stringBuilder.Append("</br> Publisher : " + row["PublisherName"] + " , PubYear :" + row["PubYear"] + ",");
                        stringBuilder.Append("</br>Pages : " + row["Pages"] + ", Book Size :" + row["Dimension"]);
                        stringBuilder.Append("</br>ISBN :" + row["ISBN"] + "</br></br> <center><span style='height: 15px;width: 15px;border: 1px solid black;border-radius: 50%;  display: inline-block;'></span></center> </br></td>");
                        stringBuilder.Append("</tr>");
                      

                    stringBuilder.Append("</table>");
                    stringBuilder.Append("</br>");

                    lblReport.Text = stringBuilder.ToString();
                   

                    }
    
                btnPrint.Visible = true;
                btnExport.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
                //btnExport.Visible = false;
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
        stringBuilder.Append("select A.AccessionNo,B.Dimension,B.Book_No,A.bookid,C.CatCode,C.BriefName,S.SubjectId,S.SubName,B.BookTitle,B.AuthorName1,B.AuthorName2,B.AuthorName3,S.Classi_No,");
        stringBuilder.Append("P.PublisherId,P.PublisherName,BP.Vol,BP.ISBN,BP.PubYear,BP.PubPlace,BP.Edition,BP.Pages,B.BookSubTitle,B.Dimension,B.AuthorLastName1,B.AuthorLastName2,B.Book_No,BP.Reference,S.Classi_No,count(*) Qty,A.status from dbo.Lib_BookAccession A ");
        stringBuilder.Append(" inner join dbo.Lib_BookMaster B on B.bookid=A.bookid");
        stringBuilder.Append(" inner join dbo.Lib_Category C on C.CatCode=B.CatCode");
        stringBuilder.Append(" inner join dbo.Lib_Subjects S on S.SubjectId=B.SubjectId");
        stringBuilder.Append(" inner join dbo.Lib_Publisher P on P.PublisherId=B.PublisherId");
        stringBuilder.Append(" inner join dbo.Lib_BookPurchase BP on BP.PurchaseId=A.PurchaseId");
        stringBuilder.Append(" where A.CollegeId=" + Session["SchoolId"]);
        if (txtAccFrm.Text !="")
            stringBuilder.Append(" and A.AccessionNo >='" + txtAccFrm.Text + "'");
        if (txtAccTo.Text != "")
            stringBuilder.Append(" and A.AccessionNo <='" + txtAccTo.Text + "'");

        stringBuilder.Append(" group by A.AccessionNo, A.bookid,B.BookTitle,B.AuthorName1,B.AuthorName2,B.AuthorName3,B.Dimension,B.Book_No,S.Classi_No,C.CatCode,C.BriefName,S.SubjectId,S.SubName,A.status,P.PublisherId,P.PublisherName,BP.Vol,BP.ISBN,BP.PubYear,BP.PubPlace,BP.Edition,BP.Pages,B.BookSubTitle,B.AuthorLastName1,B.AuthorLastName2,B.Book_No,BP.Reference,S.Classi_No");
        return obj.GetDataTableQry(stringBuilder.ToString());
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        generateReport();
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
            string str = Server.MapPath("Exported_Files/Catalogue_Report_" + DateTime.Now.ToString("ddMMyyyyhhmmssfffffff") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=Catalogue_Report_Report_" + DateTime.Now.ToString("ddMMyyyyhhmmssfffffff") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            Response.Redirect("CatalogReport_Print.aspx");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }
}