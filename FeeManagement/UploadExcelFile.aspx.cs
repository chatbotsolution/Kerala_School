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

public partial class FeeManagement_UploadExcelFile : System.Web.UI.Page
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

            grdstudentsColl.DataSource = ErrDt;
            grdstudentsColl.DataBind();
            //spanError.Visible = true;
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
      //  DataTable ErrorTable = new DataTable();
       // int admnno; 
        DateTime TransDate = DateTime.Now;
        //DateTime Crtd_date = DateTime.Now;
        //DateTime FineDate = DateTime.Now;
        string name = FileUpload1.FileName;
     
        if (dt.Rows.Count > 0 &&  dt!=null)
        {
            try
            {
                //DataTable Errordt = dt.Clone();

                foreach (DataRow row in dt.Rows)
                {
                    clsDAL clsdal = new clsDAL();
                    Hashtable ht = new Hashtable();

                    ht.Add("@Account_Number", row["Account_Number"].ToString().Trim());
                    ht.Add("@Category", row["Category"].ToString().Trim());
                    ht.Add("@PayMode", row["PayMode"].ToString().Trim());
                    ht.Add("@Trans_Num", row["Trans_Num"].ToString().Trim());
                    ht.Add("@Trans_Date", row["Trans_Date"].ToString().Trim());
                    ht.Add("@Amount", row["Amount"].ToString().Trim());
                    ht.Add("@Status", row["Status"].ToString().Trim());
                    ht.Add("@Year", row["Year"].ToString().Trim());
                    ht.Add("@Student_Name", row["Student_Name"].ToString().Trim());
                    ht.Add("@AdmnNo", row["AdmnNo"].ToString().Trim());
                    ht.Add("@Class", row["Class"].ToString().Trim());
                    ht.Add("@Section", row["Section"].ToString().Trim());
                    ht.Add("@Quarter", row["Quarter"].ToString().Trim());
                    ht.Add("@General_Fees", row["General_Fees"].ToString().Trim());
                    ht.Add("@Library_Fees", row["Library_Fees"].ToString().Trim());
                    ht.Add("@Maintenance_Fees", row["Maintenance_Fees"].ToString().Trim());
                    ht.Add("@SSS_Fees", row["SSS_Fees"].ToString().Trim());
                    ht.Add("@SC_Lab_Fees", row["SC_Lab_ Fees"].ToString().Trim());
                    ht.Add("@Stationery_Exam_Fees", row["Stationery_Exam_Fees"].ToString().Trim());
                    ht.Add("@Tuition_Fees", row["Tuition_Fees"].ToString().Trim());
                    ht.Add("@Computer_Fees", row["Computer_Fees"].ToString().Trim());
                    ht.Add("@Smart_Class_Fees", row["Smart_Class_Fees"].ToString().Trim());
                    ht.Add("@English_LL_Fees", row["English_LL_ Fees"].ToString().Trim());
                    ht.Add("@Activities_Fees", row["Activities_Fees"].ToString().Trim());
                    ht.Add("@Late_Fees", row["Late_Fees"].ToString().Trim());
                    ht.Add("@Remarks", row["Remarks"].ToString().Trim());

                   // ht.Add("@OutputMsg", ParameterDirection.Output);
                 
                  msg =clsdal.ExecuteScalar("insertExcelFile", ht);
                  
                }
                //msg = (string)cmd.Parameters["@OutputMsg"].Value;
                // msg = "Fee Inserted Successfully";
               // ViewState["ErrorExcel"] = Errordt;

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
}