using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using ASP;
using SanLib;
using System.Data.SqlTypes;

public partial class FeeManagement_UploadExcelOffline : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        Panel1.Visible = false;
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
        grdstudentsColl.DataSource = ds.Tables[0];
        grdstudentsColl.DataBind();
        Insertfee(ds.Tables[0]);
        da.Dispose();
        conn.Close();
        conn.Dispose();
    }

    protected void grdstudentsColl_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GridViewRow row = grdstudentsColl.SelectedRow;
        //lblName.Text = (row.FindControl("lblname") as Label).Text;
        //lblAdmnNo.Text = (row.FindControl("lbladmnno") as Label).Text;
        //lblClass.Text = (row.FindControl("lblClass1") as Label).Text;
        //lblTtnFee.Text = (row.FindControl("lblTuition") as Label).Text;
        //lblSplAct.Text = (row.FindControl("lblSplAct1") as Label).Text;
        //lblSmtCls.Text = (row.FindControl("lblSmtCls1") as Label).Text;

        //lblEngLab.Text = (row.FindControl("lblEngLab1") as Label).Text;
        //lblComp.Text = (row.FindControl("lblComp1") as Label).Text;
        //lblGfee.Text = (row.FindControl("lblGfee1") as Label).Text;
        //lblLib.Text = (row.FindControl("lblLib1") as Label).Text;
        //lblMain.Text = (row.FindControl("lblMain1") as Label).Text;
        //lblsss.Text = (row.FindControl("lblsss1") as Label).Text;
        //lblSclab.Text = (row.FindControl("lblSclab1") as Label).Text;
        //lblSExam.Text = (row.FindControl("lblSExam1") as Label).Text;
        //lblLfine.Text = (row.FindControl("lblFine") as Label).Text;
        //lblCAmount.Text = (row.FindControl("lblCAmnt1") as Label).Text;
        //lblTotalAmnt.Text = (row.FindControl("lblTamnt") as Label).Text;
        //Panel1.Visible = true;
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        Panel1.Visible = false;
    }

    private void Insertfee(DataTable dt)
    {
        int admnno; DateTime TransDate = DateTime.Now;
        string name = FileUpload1.FileName;
        //string dat = name.Substring(name.Length - 15);
        //string a = dat.Substring(0, 10);
        //DateTime Misdate = Convert.ToDateTime(a);
        //SqlDateTime sqldatenull; 
        double balance = 0.00;
        double fine;
        //string[] admnarray = new string[]{};
        List<string> admnlist = new List<string>();
        clsDAL clsdal = new clsDAL();
        try
        {
            if (dt.Rows.Count > 0)
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["ADMISSIONNO"].ToString().Trim() == "" || dt.Rows[i]["ADMISSIONNO"].ToString().Trim() == string.Empty)
                        admnlist.Add(dt.Rows[i]["ADMISSIONNO"].ToString().Trim());
                    else
                    {

                        FeeDetails FD = new FeeDetails();
                        //DataTable dt1 = FD.GetFeeDetailsbyAdmnNo(dt.Rows[i]["ADMISSIONNO"].ToString().Trim(), dt.Rows[i]["SESSION"].ToString().Trim());

                        Hashtable ht = new Hashtable();
                        if (dt.Rows[i]["Late Fee"].ToString().Trim() == null || dt.Rows[i]["Late Fee"].ToString().Trim() == "")
                            fine = 0.00;
                        else
                            fine = Convert.ToDouble(dt.Rows[i]["Late Fee"].ToString().Trim());

                        ht.Add("@Credit", dt.Rows[i]["TOTL_AMNT"].ToString().Trim());
                        if (dt.Rows[i]["QUARTER"].ToString().Trim() == "APR-JUN")
                        {
                            ht.Add("@FeeDesc", "APR-JUN Quarterly Fee");
                            TransDate = FD.getDuedate("APR-JUN", dt.Rows[i]["SESSION"].ToString().Trim());
                        }
                        else if (dt.Rows[i]["QUARTER"].ToString().Trim() == "JUL-SEP")
                        {

                            ht.Add("@FeeDesc", "JUL-SEP Quarterly Fee");
                            TransDate = FD.getDuedate("JUL-SEP", dt.Rows[i]["SESSION"].ToString().Trim());
                        }
                        else if (dt.Rows[i]["QUARTER"].ToString().Trim() == "OCT-DEC")
                        {

                            ht.Add("@FeeDesc", "OCT-DEC Quarterly Fee");
                            TransDate = FD.getDuedate("OCT-DEC", dt.Rows[i]["SESSION"].ToString().Trim());
                        }
                        else if (dt.Rows[i]["QUARTER"].ToString().Trim() == "JAN-MAR")
                        {

                            ht.Add("@FeeDesc", "JAN-MAR Quarterly Fee");
                            TransDate = FD.getDuedate("JAN-MAR", dt.Rows[i]["SESSION"].ToString().Trim());
                        }
                        ht.Add("@Fine", fine);
                        ht.Add("@TrnsNo", dt.Rows[i]["TRNSCTN_NMBR"].ToString().Trim());
                        ht.Add("@AdmnNo", dt.Rows[i]["ADMISSIONNO"].ToString().Trim());
                        ht.Add("@Quarter", dt.Rows[i]["QUARTER"].ToString().Trim());
                        ht.Add("@Amount", dt.Rows[i]["TOTL_AMNT"].ToString().Trim());
                        ht.Add("@Session", dt.Rows[i]["SESSION"].ToString().Trim());
                        ht.Add("@RECIEPTDATE", DateTime.ParseExact(dt.Rows[i]["RECIEPTDATE"].ToString().Trim(), "dd/MM/yyyy", null));
                        ht.Add("@UserID", Session["User_Id"].ToString().Trim());

                        int classid = Convert.ToInt32(new clsDAL().ExecuteScalarQry("Select ClassId From Ps_ClassMaster where classname='"+dt.Rows[i]["CLASS"].ToString().Trim()+"'"));
                        ht.Add("@ClassId", classid);

                        DataTable dtnew = new clsDAL().GetDataTableQry("select sum(Debit) as Debit,Sum(Credit) as Credit,Sum(Balance) as Balance from Ps_Feeledger F inner join Ps_StudMaster S on F.admnno=S.admnno where OldAdmnNO='" + dt.Rows[i]["ADMISSIONNO"].ToString().Trim() + "' and Transdate='" + TransDate + "'");
                        if (dtnew.Rows.Count > 0 && dtnew.Rows[0]["Debit"].ToString() != string.Empty)
                        {
                            ht.Add("@Debit", dtnew.Rows[0]["Debit"].ToString().Trim());
                            balance = (Convert.ToDouble(dtnew.Rows[0]["Debit"].ToString().Trim()) + fine) - Convert.ToDouble(dt.Rows[i]["TOTL_AMNT"].ToString().Trim());
                            ht.Add("@Balance", balance);

                            clsdal.ExecuteScalar("insertFeeOffline", ht);
                            DataTable newdt = clsdal.GetDataTableQry("Select AdmnNo from Ps_StudMaster where OldAdmnNo='" + dt.Rows[i]["ADMISSIONNO"].ToString().Trim() + "'");
                            if (newdt.Rows.Count > 0)
                            {
                                admnno = Convert.ToInt32(newdt.Rows[0]["AdmnNo"].ToString().Trim());
                                DataSet ds = FD.GetFeeAmountByComp(dt.Rows[i]["ADMISSIONNO"].ToString().Trim(), dt.Rows[i]["SESSION"].ToString().Trim(), "E");
                                if (ds != null)
                                {
                                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                                    {
                                        InsertfeeDetails(admnno, TransDate, ds.Tables[0].Rows[j]["Amount"].ToString().Trim(), balance, ds.Tables[0].Rows[j]["FeeCompId"].ToString().Trim(), dt.Rows[i]["TRNSCTN_NMBR"].ToString().Trim(), dt.Rows[i]["SESSION"].ToString().Trim());
                                    }
                                    //for (int k = 0; k < ds.Tables[1].Rows.Count; k++)
                                    //{
                                    //    InsertfeeDetails(admnno, TransDate, ds.Tables[0].Rows[k]["Amount"].ToString(), ds.Tables[0].Rows[k]["FeeCompId"].ToString().Trim(), dt.Rows[i]["TRNSCTN_NMBR"].ToString().Trim(), session);
                                    //}
                                }
                            }
                        }
                        else
                            admnlist.Add(dt.Rows[i]["ADMISSIONNO"].ToString().Trim());

                    }
                }
            }
            else
                return;
        }
        catch (Exception ex)
        {
           // throw ex;
        }
        finally
        {
            if (admnlist.Count > 0)
            {
                string text = "For These Admission Nos ";
                foreach (string s in admnlist)

                {
                    text = text + s + ",";
                }
                text = text.Remove(text.Length - 1, 1);
                text = text + " fee can not be updated";

                ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('" + text + "');", true);
            }
            else
                ScriptManager.RegisterStartupScript(this, GetType(), "Alert", "alert('Fee Updated');", true);
        }
    }
    private void InsertfeeDetails(int admnno, DateTime TransDate, string Amount,double balance, string FeeId, string TransNo, string session)
    {
        double credit = Math.Round(Convert.ToDouble(Amount), 2);
        if (FeeId != "10")
            balance = 0.00;

        new clsDAL().ExecuteScalar("NewUpdateFeeLedger", new Hashtable()
                        {
                        {
                            "@TransDate",
                            TransDate
                        },
                         {
                            "@AdmnNO",
                            admnno
                        },
                        {
                            "@FeeId",
                            FeeId
                        },
                        
                         {
                            "@Credit",
                              credit
                        },
                         {
                            "@Balance",
                           balance
                        },
                         {
                            "@Receipt_VrNo",
                            TransNo
                        },
                         {
                            "@UserID",
                            Session["User_Id"].ToString().Trim()
                        },
                        {
                            "@UserDate",
                            DateTime.Now
                        },
                        {
                            "@PayMode",
                            "Offline"
                        },
                         {
                            "@Session",
                            session
                        }
                        });
    }
}