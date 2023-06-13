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

public partial class Library_rptBookdetailsreport : System.Web.UI.Page
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
                
                stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
                stringBuilder.Append("<tr><td colspan='20' class='tbltd'><strong>Total no of Books :" + dataForReport.Rows.Count + "</strong></td></tr>");
                stringBuilder.Append("<tr  style='background-color:#CCCCCC'>");

                stringBuilder.Append("<td width='30px' class='tbltxt'><strong>Sl No.</strong></td>");
                if (cbAccDate.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Accession Date.</strong></td>");
                if (cbAccNo.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Accession No.</strong></td>");
                if (cbBrfNm.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Brief Name</strong></td>");
                if (cbSubNm.Checked)
                    stringBuilder.Append("<td  style='width: 100px' class='tbltxt' ><strong>Subject</strong></td>");
                if (cbBkTitle.Checked)
                    stringBuilder.Append("<td style='width: 120px' class='tbltxt'><strong>BookTitle</strong></td>");
                if (cbBkSubTitle.Checked)
                    stringBuilder.Append("<td style='width: 120px' class='tbltxt'><strong>SubTitle</strong></td>");
                if (cbAuthNm1.Checked)
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>AuthorName1</strong></td>");
                if (cbAuthNm2.Checked)
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>AuthorName2</strong></td>");
                if (cbAuthNm3.Checked)
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>AuthorName3</strong></td>");
                if (cbEdi.Checked)
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Edition</strong></td>");
                if (cbPublishNm.Checked)
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Publisher</strong></td>");
                if (cbPubPlace.Checked)
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Pub. Place</strong></td>");
                if (cbPubYr.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Pub Year</strong></td>");
                if (cbPage.Checked)
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Pages</strong></td>");
                if (cbVol.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Volume</strong></td>");
                if (cbSource.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Source</strong></td>");
                if (cbSourcetype.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>SourceType</strong></td>");
                if (cbPrice.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Price/Piece</strong></td>");
                if (cbClassi_No.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Classfi.No</strong></td>");
                if (cbBkno.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Book No</strong></td>");
                if (cbDimen.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Dimension</strong></td>");
                if (cbISBN.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>ISBN</strong></td>");
                if (cbBillNo.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>BillNo</strong></td>");
                if (cbBillDate.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>BIllDate</strong></td>");

                if (cbReference.Checked)
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Reference</strong></td>");
               
               
                if (cbstatus.Checked)
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Status</strong></td>");

                if (cbAll.Checked)
                {
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Accession Date</strong></td>");
                    stringBuilder.Append("<td style='width: 100px' class='tbltxt'><strong>Accession No.</strong></td>");
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Brief Name</strong></td>");
                    stringBuilder.Append("<td style='width: 100px' class='tbltxt' ><strong>Subject</strong></td>");
                    stringBuilder.Append("<td style='width: 120px' class='tbltxt'><strong>BookTitle</strong></td>");
                    stringBuilder.Append("<td style='width: 120px' class='tbltxt'><strong>SubTitle</strong></td>");
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>AuthorName1</strong></td>");
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>AuthorName2</strong></td>");
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>AuthorName3</strong></td>");
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Edition</strong></td>");
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Publisher</strong></td>");
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Pub. Place</strong></td>");
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Pub. Year</strong></td>");
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Pages</strong></td>");
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Volume</strong></td>");
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Source</strong></td>");
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>SourceType</strong></td>");
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Price/Piece</strong></td>");
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Classification</strong></td>");
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Book No</strong></td>");
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Dimension</strong></td>");
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>ISBN</strong></td>");
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>BillNo</strong></td>");
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>BillDate</strong></td>");
                    stringBuilder.Append("<td style='width: 80px' class='tbltxt'><strong>Reference</strong></td>");
                 
                 
                 
                    stringBuilder.Append("<td style='width: 50px' class='tbltxt'><strong>Status</strong></td>");

                }

                stringBuilder.Append("</tr>");
                int num1 = 1;
                string str = dataForReport.Rows[0]["bookid"].ToString();
                int num2;
                int num3;
                int num4;
                int num5;
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
                    //if (str != dataForReport.Rows[index]["bookid"].ToString())
                    //{
                        stringBuilder.Append("<tr style='text-align:left'>");
                        stringBuilder.Append("<td>" + num1.ToString() + "</td>");
                        if (cbAccDate.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PurchaseDate"].ToString() + "</td>");
                        }


                        if (cbAccNo.Checked)
                        {       
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AccessionNo"].ToString() + "</td>");
                        }

                        if (cbBrfNm.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BriefName"].ToString() + "</td>");
                        }
                        if (cbSubNm.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["SubName"].ToString() + "</td>");
                        }
                        if (cbBkTitle.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BookTitle"].ToString() + "</td>");
                        }
                        if (cbBkSubTitle.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BookSubTitle"].ToString() + "</td>");
                        }
                        if (cbAuthNm1.Checked)
                        {
                            //stringBuilder.Append(row["AdmnSessYr"].ToString().Trim());
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName1"].ToString() + " " + dataForReport.Rows[index - 1]["AuthorLastName1"].ToString() + "</td>");
                        }
                        if (cbAuthNm2.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName2"].ToString() + " " + dataForReport.Rows[index - 1]["AuthorLastName2"].ToString() + "</td>");
                        }
                        if (cbAuthNm3.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName3"].ToString() + "</td>");
                        }
                        if (cbEdi.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Edition"].ToString() + "</td>");
                        }

                        if (cbPublishNm.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PublisherName"].ToString() + "</td>");
                        }
                        if (cbPubPlace.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PublisherPlace"].ToString() + "</td>");
                        }

                        if (cbPubYr.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PubYear"].ToString() + "</td>");
                        }

                        if (cbPage.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Pages"].ToString() + "</td>");
                        }

                        if (cbVol.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Vol"].ToString() + "</td>");
                        }
                        if (cbSource.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["ReceivedFrom"].ToString() + "</td>");
                        }
                        if (cbSourcetype.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["SourceOfAquiring"].ToString() + "</td>");
                        }
                        if (cbPrice.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PurchasePrice"].ToString() + "</td>");
                        }

                       if (cbClassi_No.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Classi_No"].ToString() + "</td>");
                        }
                        if (cbBkno.Checked)
                        {
                           stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Book_No"].ToString() + "</td>");
                        }
                        if (cbDimen.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Dimension"].ToString() + "</td>");
                        }
                       
                        if (cbISBN.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["ISBN"].ToString() + "</td>");
                        }
                        if (cbBillNo.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BillNo"].ToString() + "</td>");
                        }
                        if (cbBillDate.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BillDate"].ToString() + "</td>");
                        }
                        if (cbReference.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Reference"].ToString() + "</td>");
                        }
                        if (cbstatus.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["status"].ToString() + "</td>");
                        }


                        if (cbAll.Checked)
                        {
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PurchaseDate"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AccessionNo"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BriefName"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["SubName"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BookTitle"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BookSubTitle"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName1"].ToString() +" " + dataForReport.Rows[index - 1]["AuthorLastName1"].ToString()+ "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName2"].ToString()+ " " + dataForReport.Rows[index - 1]["AuthorLastName2"].ToString()+"</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName3"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Edition"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PublisherName"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PublisherPlace"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PubYear"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Pages"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Vol"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["ReceivedFrom"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["SourceOfAquiring"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PurchasePrice"].ToString() + "</td>");

                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Classi_No"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Book_No"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Dimension"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["ISBN"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BillNo"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BillDate"].ToString() + "</td>");
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Reference"].ToString() + "</td>");
                            
                            
                          
                          
                            stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["status"].ToString() + "</td>");
                        }
                      
                        
                        
                        //stringBuilder.Append("<td>" + num2 + "</td>");
                        //stringBuilder.Append("<td>" + num4 + "</td>");
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
                    //}
                    //else if (dataForReport.Rows[index]["status"].ToString() == "I" || dataForReport.Rows[index]["status"].ToString() == "R")
                    //{
                    //    num2 += int.Parse(dataForReport.Rows[index]["Qty"].ToString());
                    //    num3 += int.Parse(dataForReport.Rows[index]["Qty"].ToString());
                    //}
                    //else
                    //{
                    //    num4 += int.Parse(dataForReport.Rows[index]["Qty"].ToString());
                    //    num5 += int.Parse(dataForReport.Rows[index]["Qty"].ToString());
                    //}
                }
                stringBuilder.Append("<tr style='text-align:left'>");
                stringBuilder.Append("<td>" + num1.ToString() + "</td>");
                if (cbAccDate.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PurchaseDate"].ToString() + "</td>");
                }
                if (cbAccNo.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AccessionNo"].ToString() + "</td>");
                }

                if (cbBrfNm.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BriefName"].ToString() + "</td>");
                }
                if (cbSubNm.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["SubName"].ToString() + "</td>");
                }
                if (cbBkTitle.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BookTitle"].ToString() + "</td>");
                }
                if (cbBkSubTitle.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BookSubTitle"].ToString() + "</td>");
                }
                if (cbAuthNm1.Checked)
                {
                    //stringBuilder.Append(row["AdmnSessYr"].ToString().Trim());

                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName1"].ToString() +" " + dataForReport.Rows[index - 1]["AuthorLastName1"].ToString()+ "</td>");
                }
                if (cbAuthNm2.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName2"].ToString() + " " + dataForReport.Rows[index - 1]["AuthorLastName2"].ToString() + "</td>");
                }
                if (cbAuthNm3.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName3"].ToString() + "</td>");
                }
                if (cbEdi.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Edition"].ToString() + "</td>");
                }

                if (cbPublishNm.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PublisherName"].ToString() + "</td>");
                }
                if (cbPubPlace.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PublisherPlace"].ToString() + "</td>");
                }
                if (cbPubYr.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PubYear"].ToString() + "</td>");
                }
                if (cbPage.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Pages"].ToString() + "</td>");
                }
                if (cbVol.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Vol"].ToString() + "</td>");
                }
                if (cbSource.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["ReceivedFrom"].ToString() + "</td>");
                }
                if (cbSourcetype.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["SourceOfAquiring"].ToString() + "</td>");
                }
                if (cbPrice.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PurchasePrice"].ToString() + "</td>");
                }



                if (cbClassi_No.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Classi_No"].ToString() + "</td>");
                }
                if (cbBkno.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Book_No"].ToString() + "</td>");
                }
              
                if (cbDimen.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Dimension"].ToString() + "</td>");
                }
               
               if (cbISBN.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["ISBN"].ToString() + "</td>");
                }

               if (cbBillNo.Checked)
               {
                   stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BillNo"].ToString() + "</td>");
               }
               if (cbBillDate.Checked)
               {
                   stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BillDate"].ToString() + "</td>");
               }
               if (cbReference.Checked)
               {
                   stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Reference"].ToString() + "</td>");
               }
               
                if (cbstatus.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["status"].ToString() + "</td>");
                }



                if (cbAll.Checked)
                {
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PurchaseDate"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AccessionNo"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BriefName"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["SubName"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BookTitle"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BookSubTitle"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName1"].ToString() +" " + dataForReport.Rows[index - 1]["AuthorLastName1"].ToString()+ "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName2"].ToString() + " " + dataForReport.Rows[index - 1]["AuthorLastName2"].ToString()+ "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName3"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Edition"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PublisherName"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PublisherPlace"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PubYear"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Pages"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Vol"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PurchasePrice"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["ReceivedFrom"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["SourceOfAquiring"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Classi_No"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Book_No"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Dimension"].ToString() + "</td>");
                                   
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["ISBN"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BillNo"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BillDate"].ToString() + "</td>");
                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Reference"].ToString() + "</td>");


                    stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["status"].ToString() + "</td>");
                }
                      
                //stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AccessionNo"].ToString() + "</td>");
                //stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["BookTitle"].ToString() + "</td>");
                //stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["SubName"].ToString() + "</td>");
                //stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["PublisherName"].ToString() + "</td>");
                //stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["Classi_No"].ToString() + "</td>");
                //stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName1"].ToString() + "</td>");
                //stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName2"].ToString() + "</td>");
                //stringBuilder.Append("<td>" + dataForReport.Rows[index - 1]["AuthorName3"].ToString() + "</td>");
                //stringBuilder.Append("<td>" + num2 + "</td>");
                //stringBuilder.Append("<td>" + num4 + "</td>");
                stringBuilder.Append("</tr>");
                //stringBuilder.Append("<tr>");
                //stringBuilder.Append("<td style='text-align:right; font-weight:bold' colspan='5'>Total =</td>");
                //stringBuilder.Append("<td style='font-weight:bold'>" + num3 + "</td>");
                //stringBuilder.Append("<td style='font-weight:bold'>" + num5 + "</td>");
                //stringBuilder.Append("</tr>");
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
                Session["bookdetailsReport"] = stringBuilder.ToString();
            else
                Session["bookdetailsReport"] = null;
        }
        catch (Exception ex)
        {
        }
    }

    private DataTable getDataForReport()
    {
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Select A.AccessionNo,B.Dimension,B.Book_No,A.bookid,C.CatCode,C.BriefName,S.SubjectId,S.SubName,B.BookTitle,B.BookSubTitle,B.AuthorName1,B.AuthorLastName1,B.AuthorName2,B.AuthorLastName2,B.AuthorName3,S.Classi_No,");
        stringBuilder.Append("P.PublisherId,P.PublisherName,P.PublisherPlace,BP.Vol,BP.ISBN,BP.PubYear,Convert(varchar,Bp.PurchaseDate,103) as PurchaseDate,BP.Edition,BP.Pages,BP.SourceofAquiring,BP.ReceivedFrom,BP.BillNo,BP.BillDate,BP.Reference,count(*) Qty,Bp.PurchasePrice,A.status from dbo.Lib_BookAccession A ");
        stringBuilder.Append(" inner join dbo.Lib_BookMaster B on B.bookid=A.bookid");
        stringBuilder.Append(" inner join dbo.Lib_Category C on C.CatCode=B.CatCode");
        stringBuilder.Append(" inner join dbo.Lib_Subjects S on S.SubjectId=B.SubjectId");
        stringBuilder.Append(" inner join dbo.Lib_Publisher P on P.PublisherId=B.PublisherId");
        stringBuilder.Append(" inner join dbo.Lib_BookPurchase BP on BP.PurchaseId=A.PurchaseId");
        stringBuilder.Append(" where A.CollegeId=" + Session["SchoolId"]);
        if (ddlCategory.SelectedIndex > 0)
            stringBuilder.Append(" and C.CatCode='" + ddlCategory.SelectedValue + "'");
        if (ddlSubject.SelectedIndex > 0)
            stringBuilder.Append(" and S.SubjectId='" + ddlSubject.SelectedValue + "'");
        if (txtAuthor.Text.Trim() != "")
            stringBuilder.Append(" and B.AuthorName1='" + txtAuthor.Text.Trim() + "' or B.AuthorName2='" + txtAuthor.Text.Trim() + "' or B.AuthorName3='" + txtAuthor.Text.Trim() + "'");
        if (ddlPublisher.SelectedIndex > 0)
            stringBuilder.Append(" and P.PublisherId='" + ddlPublisher.SelectedValue + "'");
        stringBuilder.Append(" group by A.AccessionNo, A.bookid,B.BookTitle,B.AuthorName1,B.AuthorLastName1,B.AuthorName2,B.AuthorLastName2,B.AuthorName3,B.Dimension,B.Book_No,S.Classi_No,C.CatCode,C.BriefName,S.SubjectId,S.SubName,A.status,P.PublisherId,P.PublisherName,BP.Vol,BP.ISBN,BP.PubYear,BP.Edition,BP.Pages,B.bookSubTitle,P.PublisherPlace,Bp.Purchasedate,BP.SourceofAquiring,BP.ReceivedFrom,BP.BillNo,BP.BillDate,BP.Reference,Bp.PurchasePrice");
        return obj.GetDataTableQry(stringBuilder.ToString());
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (Session["bookdetailsReport"] != null)
        {
            try
            {
                ExportToExcel(Session["bookdetailsReport"].ToString());
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
            Response.AddHeader("content-disposition", "attachment;filename=Book_Details_Report_" + DateTime.Now.ToString("ddMMyyyyhhmmssfffffff") + ".xls");
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
    protected void cbAll_CheckedChanged(object sender, EventArgs e)
    {
        if (!cbAll.Checked)
            return;
        cbAccNo.Checked = false;
        cbBrfNm.Checked = false;
        cbSubNm.Checked = false;
        cbBkTitle.Checked = false;
        cbAuthNm1.Checked = false;
        cbAuthNm2.Checked = false;
        cbAuthNm3.Checked = false;
        cbClassi_No.Checked = false;
        cbPublishNm.Checked = false;
        cbstatus.Checked = false;
        cbBkSubTitle.Checked = false;
        cbAccDate.Checked = false;
        cbPubPlace.Checked = false;
        cbSource.Checked = false;
        cbSourcetype.Checked = false;
        cbBillNo.Checked = false;
        cbBillDate.Checked = false;
        cbReference.Checked = false;
        cbPrice.Checked = false;
        
       
    }
    protected void cbAccNo_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbBrfNm_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbSubNm_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbBkTitle_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbAuthNm1_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbAuthNm2_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbAuthNm3_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbClassi_No_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbPublishNm_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbstatus_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbDimen_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbBkno_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbVol_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbISBN_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbPubYr_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbEdi_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbPage_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbAccDate_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbBkSubTitle_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbPubPlace_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbSourcetype_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbSource_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbBillNo_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbBillDate_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbReference_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
    protected void cbPrice_CheckedChanged(object sender, EventArgs e)
    {
        cbAll.Checked = false;
    }
}