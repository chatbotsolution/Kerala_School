using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.OleDb;
using System.Data;
using ASP;
using SanLib;
using System.Globalization;
using System.Data.SqlClient;
using System.Configuration;

public partial class FeeManagement_UploadExcelOnline : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        Panel1.Visible = false;
        spanError.Visible = false;
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        string connString = "";
        string strFileType = Path.GetExtension(FileUpload1.FileName).ToLower();
        string path = Server.MapPath("~/Up_Files/Fee/") + Path.GetFileName(FileUpload1.PostedFile.FileName);
        if (File.Exists(path))
            File.Delete(path);
        FileUpload1.PostedFile.SaveAs(path);
        //Connection String to Excel Workbook
        if (strFileType.Trim() == ".xls")
        {
            connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
        }
        else if (strFileType.Trim() == ".xlsx")
        {
            connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
        }
        OleDbConnection conn = new OleDbConnection(connString);
        if (conn.State == ConnectionState.Closed)
            conn.Open();
        string sheet1 = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
        // DataTable activityDataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        string query = "SELECT* FROM [" + sheet1 + "]";

        if (conn.State == ConnectionState.Closed)
            conn.Open();
        OleDbCommand cmd = new OleDbCommand(query, conn);
        OleDbDataAdapter da = new OleDbDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        lblMsg.Text = Insertfee(ds.Tables[0]);
        DataTable ErrDt = (DataTable)ViewState["ErrorExcel"];
        if (ErrDt != null)
        {
          
            grdstudentsColl.DataSource =ErrDt ;
            grdstudentsColl.DataBind();
            spanError.Visible = true;
        }

        // Response.Write(count);
        da.Dispose();
        conn.Close();
        conn.Dispose();


    }
    private string Insertfee(DataTable dt)
    {
        dt.Columns.Add("Error");
        string msg = "";
        DataTable ErrorTable = new DataTable();
       // SqlConnection con = new SqlConnection();
        //con.ConnectionString = ConfigurationSettings.AppSettings["conString"].ToString();
        //con.ConnectionTimeout.Equals(0);
        int admnno; DateTime TransDate = DateTime.Now;
        DateTime Crtd_date =DateTime.Now;
        DateTime FineDate = DateTime.Now;
        string name = FileUpload1.FileName;
       // con.Open();

       // SqlTransaction transaction = null;
        //DateTime ctddate = DateTime.ParseExact("03/05/2018 17:30:35", "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
        if (dt.Rows.Count > 0)
        {
            try
            {
               // transaction = con.BeginTransaction();
                DataTable Errordt = dt.Clone();
               
                foreach (DataRow row in dt.Rows)
                {
                    clsDAL clsdal = new clsDAL();
                    DataTable newdt = clsdal.GetDataTableQry("Select AdmnNo from Ps_StudMaster where OldAdmnNo='" + row["AdmnNo"].ToString().Trim() + "'");
                    if (newdt.Rows.Count > 0)
                    {
                        admnno = Convert.ToInt32(newdt.Rows[0]["AdmnNo"].ToString().Trim());
                    }
                    else
                    {
                        row["Error"] = "Invalid Admission Number";
                        DataRow drError = Errordt.NewRow();
                        drError.ItemArray = row.ItemArray;
                        Errordt.Rows.Add(drError);
                                               
                        continue;
                    }

                    Hashtable ht = new Hashtable();
                    string session = row["Year"].ToString().Trim();
                    double balance = 0.00;
                    double fine = 0.00; string debit = "", FeeDesc = "";
                    FeeDetails FD = new FeeDetails();
                   // DataTable dt1 = FD.GetFeeDetailsbyAdmnNo(row["AdmnNo"].ToString().Trim(), session);

                      
                    
                    if (row["Late_Fees"].ToString().Trim() != String.Empty)
                        fine = Convert.ToDouble(row["Late_Fees"].ToString().Trim());
                    else
                        fine = 0.00;
                   ht.Add("@Credit", row["Amount"].ToString().Trim());
                 
                    switch (row["Quarter"].ToString().Trim())
                        {
                            case "ALL FOUR":
                               
                                FeeDesc = "WHOLE YEAR FEE";
                                TransDate = FD.getDuedate("APR-JUN", session);
                                FineDate = TransDate;
                                
                                break;  
                            case "ALL THREE":
                               
                                FeeDesc = "THREE QTR FEE";
                                TransDate = FD.getDuedate("APR-JUN", session);
                                FineDate =FD.getDuedate("JUL-SEP", session);
                                break;
                            case "FIRST,SECOND,THIRD":

                                FeeDesc = "THREE QTR FEE";
                                TransDate = FD.getDuedate("JAN-MAR", session);
                                FineDate = FD.getDuedate("OCT-DEC", session);
                                break;
                            case "FIRST,SECOND":

                                FeeDesc = "THREE QTR FEE";
                                TransDate = FD.getDuedate("JAN-MAR", session);
                                Crtd_date = FD.getDuedate("OCT-DEC", session);
                                FineDate = FD.getDuedate("JUL-SEP", session);
                                ht.Add("@Crtd_date", FD.getDuedate("OCT-DEC", session));
                                break;


                            case "FIRST":
                               
                               
                                FeeDesc = "APR-JUN Quarterly Fee";
                                TransDate = FD.getDuedate("APR-JUN", session);
                                FineDate = TransDate;
                                break;
                            case "SECOND":
                               
                                FeeDesc = "JUL-SEP Quarterly Fee";
                                TransDate = FD.getDuedate("JUL-SEP", session);
                                FineDate = TransDate;
                                break;
                            case "THIRD":
                               
                                FeeDesc = "OCT-DEC Quarterly Fee";
                                TransDate = FD.getDuedate("OCT-DEC", session);
                                FineDate = TransDate;
                                break;
                            case "FOURTH":
                               
                                FeeDesc = "JAN-MAR Quarterly Fee";
                                TransDate = FD.getDuedate("JAN-MAR", session);
                                FineDate = TransDate;
                                break;
                            default:
                                break;
                        }
                       DataTable DtAmount= clsdal.GetDataTable("NewGetFeeDetails",new Hashtable(){
                           {"@AdmnNo",admnno},{"@Quarter", row["Quarter"].ToString()},{"@Session",session},{"@TransDate",TransDate},{"@Crtd_date",Crtd_date}});
                       if (DtAmount !=null )
                       {
                           if((DtAmount.Rows[0]["TOTAL"].ToString()!=string.Empty))
                           {
                           if (Convert.ToDouble(DtAmount.Rows[0]["TOTAL"].ToString()) == (Convert.ToDouble(row["Amount"].ToString()) - fine))
                           {
                               balance = (Convert.ToDouble(DtAmount.Rows[0]["TOTAL"].ToString().Trim()) + fine) - Convert.ToDouble(row["Amount"].ToString().Trim());
                               debit=DtAmount.Rows[0]["TOTAL"].ToString().Trim();
                           }
                           else
                           {
                               row["Error"] = "Amount Mismatch";
                               DataRow drError = Errordt.NewRow();
                               drError.ItemArray = row.ItemArray;
                               Errordt.Rows.Add(drError);
                               continue;
                           }
                           }
                           else
                           {
                               row["Error"] = "Amount Mismatch";
                               DataRow drError = Errordt.NewRow();
                               drError.ItemArray = row.ItemArray;
                               Errordt.Rows.Add(drError);
                               continue;
                           }
                       }

                        ht.Add("@Balance", balance);
                        ht.Add("@Fine", fine);
                        ht.Add("@Debit", debit);
                        ht.Add("@FeeDesc", FeeDesc);
                        ht.Add("@TransDate", TransDate);
                        ht.Add("@Authorised_date", FineDate);
                      

                    int classid = Convert.ToInt32(new clsDAL().ExecuteScalarQry("Select Classid from Ps_ClassMaster where ClassName='" + row["Class"].ToString().Trim() + "'"));
                    ht.Add("@ClassId", classid);

                    ht.Add("@TrnsNo", row["Trans_Num"].ToString().Trim());
                    ht.Add("@AdmnNo", admnno);
                    ht.Add("@Session", session);
                    ht.Add("@Quarter", row["Quarter"].ToString().Trim());
                    ht.Add("@Aggregator", row["PayMode"].ToString().Trim());
                    ht.Add("@Amount", row["Amount"].ToString().Trim());



                    // ht.Add("@Clearance_date", DateTime.ParseExact(row["Transaction Date"].ToString().Trim(), "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture));
                    ht.Add("@Clearance_date", row["Trans_Date"].ToString().Trim());
                    ht.Add("@TrnsDesc", "");
                    ht.Add("@UserID", Session["User_Id"].ToString().Trim());
                    ht.Add("@MisDate", row["Trans_Date"].ToString().Trim());
                    ht.Add("@MisFileName", name);
                    ht.Add("@schoolid", Session["SchoolId"].ToString());
                  
                    //Check for duplicates--------------------
                    string ab = clsdal.ExecuteScalar("insertCheckPayment", ht);
                    if (clsdal.ExecuteScalar("insertCheckPayment", ht) == "D")
                    {
                        row["Error"] = "Fee details Exists or Possible duplicate payment";
                        DataRow drError = Errordt.NewRow();
                        drError.ItemArray = row.ItemArray;
                        Errordt.Rows.Add(drError);
                    }
                    else
                    {

                        //------------Insert Fee------------------------------------------------------

                        // sqlCmd1 = ExecuteProc("insertFeeOnline", transaction, con, ht);
                        clsdal.ExecuteScalar("insertFeeOnline", ht);

                        // SqlCommand sqlCommand1 = new SqlCommand("insertFeeOnline", con);
                        // sqlCommand1.CommandType = CommandType.StoredProcedure;
                        // AddParam(sqlCommand1, ht);
                        //int x= sqlCommand1.ExecuteNonQuery();
                        //--------------Insert Fine Details if any--------------------------------   
                        #region Fine Details


                        //if (fine > 0.00)
                        //{
                        //    SqlCommand sqlCommand2 = new SqlCommand("New_insert_FineLedger", con, transaction);
                        //    sqlCommand2.CommandType = CommandType.StoredProcedure;

                        //    AddParam(sqlCommand2, new Hashtable()
                        //           {
                        //                            {
                        //                                "@TransDate",
                        //                                 TransDate
                        //                            },
                        //                             {
                        //                                "@AdmnNo",
                        //                                 admnno
                        //                            },
                        //                            {
                        //                                "@FeeDesc",
                        //                                 FeeDesc
                        //                            },
                        //                           {
                        //                                "@debit",
                        //                                 fine
                        //                            },
                        //                            {
                        //                                "@credit",
                        //                                fine
                        //                            },
                        //                              {
                        //                                "@TrnsNo",
                        //                                row["Trans_Num"].ToString().Trim()
                        //                            },
                        //                              {
                        //                                "@UserID",
                        //                                 Session["User_Id"].ToString().Trim()
                        //                            },
                        //                             {
                        //                                "@schoolid",
                        //                                Session["SchoolId"].ToString()
                        //                            },
                        //                 });

                        //    sqlCommand2.ExecuteNonQuery();
                        //}

                        #endregion

                    }

                }
               // transaction.Commit();
                msg = "Fee Inserted Successfully";
                ViewState["ErrorExcel"] = Errordt;

            }
            catch (Exception ex)
            {
               // transaction.Rollback();
                msg = ex.Message.ToString();

            }
            finally
            {
               // transaction.Dispose();
                //if (con.State == ConnectionState.Open)
                //    con.Close();

            }
        }
        return msg;
    }
    private void AddParam(SqlCommand sqlCommand, Hashtable ht)
    {
        foreach (DictionaryEntry dictionaryEntry in ht)
            sqlCommand.Parameters.AddWithValue((string)dictionaryEntry.Key, dictionaryEntry.Value);
    }
    protected void grdstudentsColl_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow row = grdstudentsColl.SelectedRow;
        lblName.Text = (row.FindControl("lblname") as Label).Text;
        lblAdmnNo.Text = (row.FindControl("lbladmnno") as Label).Text;
        lblClass.Text = (row.FindControl("lblClass1") as Label).Text;
        lblTtnFee.Text = (row.FindControl("lblTuition") as Label).Text;
        lblSplAct.Text = (row.FindControl("lblSplAct1") as Label).Text;
        lblSmtCls.Text = (row.FindControl("lblSmtCls1") as Label).Text;
        lblEngLab.Text = (row.FindControl("lblEngLab1") as Label).Text;
        lblComp.Text = (row.FindControl("lblComp1") as Label).Text;
        lblGfee.Text = (row.FindControl("lblGfee1") as Label).Text;
        lblLib.Text = (row.FindControl("lblLib1") as Label).Text;
        lblMain.Text = (row.FindControl("lblMain1") as Label).Text;
        lblsss.Text = (row.FindControl("lblsss1") as Label).Text;
        lblSclab.Text = (row.FindControl("lblSclab1") as Label).Text;
        lblSExam.Text = (row.FindControl("lblSExam1") as Label).Text;
        lblLfine.Text = (row.FindControl("lblFine") as Label).Text;
        lblCAmount.Text = (row.FindControl("lblCAmnt1") as Label).Text;
        lblTotalAmnt.Text = (row.FindControl("lblTamnt") as Label).Text;
        Panel1.Visible = true;
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Panel1.Visible = false;
    }
}