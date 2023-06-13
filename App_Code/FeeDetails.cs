
using Classes.DA;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

/// <summary>
/// Getting Feedetails For each student by ClassId and StreamID
/// </summary>
public class FeeDetails
{
    private SqlConnection con;
	public FeeDetails()
	{
        this.con = new SqlConnection();
        this.con.ConnectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
	}
    public DataTable GetFeeDetailsbyAdmnNo(string OldAdmno, string Session)
    {
        int admnno=0;
        int ClassId = 0; int StreamId = 0;
        int sixth = 0;
        string studtype = "";
        DataTable datatable = new DataTable();
        try
        {
            DataTable dt = new clsDAL().GetDataTableQry("Select AdmnNo,SixthOptional,StudType From Ps_StudMaster where OldAdmnno='" + OldAdmno + "'");
            if (dt.Rows.Count > 0)
            {
                admnno = Convert.ToInt32(dt.Rows[0]["AdmnNo"].ToString());
                studtype = dt.Rows[0]["StudType"].ToString();
                if(dt.Rows[0]["SixthOptional"].ToString() !=string.Empty)
                   sixth = Convert.ToInt32(dt.Rows[0]["SixthOptional"].ToString());
            }
            else
                return datatable = null;
            DataTable dt1 = new clsDAL().GetDataTableQry("Select Classid,Stream from Ps_ClasswiseStudent where Admnno=" + admnno + " and SessionYear='" + Session + "'");
            if (dt1.Rows.Count > 0)
            {
               ClassId = Convert.ToInt32(dt1.Rows[0]["Classid"].ToString());
               StreamId = Convert.ToInt32(dt1.Rows[0]["Stream"].ToString());
            }
            else
                return datatable = null;

            Hashtable ht = new Hashtable();
            ht.Add("AdmnNo", admnno);
            ht.Add("Session", Session);
            ht.Add("Classid", ClassId);
            ht.Add("StreamId", StreamId);
            ht.Add("Studtype", studtype);

            datatable = new clsDAL().GetDataTable("FeeDetail", ht);
            if (datatable.Rows.Count > 0)
            {
                if (ClassId == 11 || ClassId == 12)
                {
                    if (sixth == 83 || sixth == 84)
                    {
                        double compfee =Convert.ToDouble( new Common().ExecuteScalarQry("Select Amount from Ps_FeeAmount where FeeCompId=11 and Classid=" + ClassId + " and StreamId=1 and SessionYr='" + Session + "'"));
                        datatable.Rows[0]["first"] = (Convert.ToDouble(datatable.Rows[0]["first"].ToString()) - compfee).ToString();
                        datatable.Rows[0]["second"] = (Convert.ToDouble(datatable.Rows[0]["first"].ToString()) - compfee).ToString();
                        datatable.Rows[0]["third"] = (Convert.ToDouble(datatable.Rows[0]["first"].ToString()) - compfee).ToString();
                        datatable.Rows[0]["fourth"] = (Convert.ToDouble(datatable.Rows[0]["first"].ToString()) - compfee).ToString();
                    }
                }
               
                double AnnualFeeForEx = Convert.ToDouble(datatable.Rows[0]["first"].ToString()) + Convert.ToDouble(datatable.Rows[0]["second"].ToString()) + Convert.ToDouble(datatable.Rows[0]["third"].ToString()) + Convert.ToDouble(datatable.Rows[0]["fourth"].ToString());
                double AnnualFeeForNew = Convert.ToDouble(datatable.Rows[0]["NewAdmnFee"].ToString()) + Convert.ToDouble(datatable.Rows[0]["second"].ToString()) + Convert.ToDouble(datatable.Rows[0]["third"].ToString()) + Convert.ToDouble(datatable.Rows[0]["fourth"].ToString());
                double ThreeQtrForNew = Convert.ToDouble(datatable.Rows[0]["second"].ToString()) + Convert.ToDouble(datatable.Rows[0]["third"].ToString()) + Convert.ToDouble(datatable.Rows[0]["fourth"].ToString());
                datatable.Columns.Add("Session");
                datatable.Columns.Add("ClassId");
                datatable.Columns.Add("StreamId");
                datatable.Columns.Add("AnnualFeeForEx");
                datatable.Columns.Add("AnnualFeeForNew");
                datatable.Columns.Add("ThreeQtrForNew");
                datatable.Rows[0]["Session"] = Session;
                datatable.Rows[0]["Classid"] = ClassId;
                datatable.Rows[0]["StreamId"] = StreamId;
                datatable.Rows[0]["AnnualFeeForEx"] = AnnualFeeForEx.ToString();
                datatable.Rows[0]["AnnualFeeForNew"] = AnnualFeeForNew.ToString();
                datatable.Rows[0]["ThreeQtrForNew"] = ThreeQtrForNew.ToString();
                    
                
            }
            else
                return datatable = null;
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return datatable; 

    }
    public DataTable GetFeeDetails(string session)
    {
        DataTable datatable = new DataTable();
        datatable.Columns.Add("Session");
        datatable.Columns.Add("ClassId");
        datatable.Columns.Add("Class");
        datatable.Columns.Add("StreamId");
        datatable.Columns.Add("Stream");
        datatable.Columns.Add("NewAdmnFee");
        datatable.Columns.Add("first");
        datatable.Columns.Add("second");
        datatable.Columns.Add("third");
        datatable.Columns.Add("fourth");
        datatable.Columns.Add("AnnualFeeForEx");
        datatable.Columns.Add("AnnualFeeForNew");
        StringBuilder st = new StringBuilder();
        
        for (int clsid = 1; clsid <= 14; clsid++)
        {
            if (clsid > 12)
            {
                for (int streamid = 2; streamid <= 5; streamid++)
                {
                    Hashtable ht = new Hashtable();
                    ht.Add("AdmnNo", null);
                    ht.Add("Session", session);
                    ht.Add("Classid", clsid);
                    ht.Add("StreamId", streamid);
                   DataTable datatable1 = new clsDAL().GetDataTable("FeeDetail-", ht);
                    if (datatable.Rows.Count > 0)
                    {
                        double AnnualFeeForEx = Convert.ToDouble(datatable1.Rows[0]["first"].ToString()) + Convert.ToDouble(datatable1.Rows[0]["second"].ToString()) + Convert.ToDouble(datatable1.Rows[0]["third"].ToString()) + Convert.ToDouble(datatable1.Rows[0]["fourth"].ToString());
                        double AnnualFeeForNew = Convert.ToDouble(datatable1.Rows[0]["NewAdmnFee"].ToString()) + Convert.ToDouble(datatable1.Rows[0]["second"].ToString()) + Convert.ToDouble(datatable1.Rows[0]["third"].ToString()) + Convert.ToDouble(datatable1.Rows[0]["fourth"].ToString());
                        datatable1.Columns.Add("Session");
                        datatable1.Columns.Add("ClassId");
                        datatable1.Columns.Add("StreamId");
                        datatable1.Columns.Add("AnnualFeeForEx");
                        datatable1.Columns.Add("AnnualFeeForNew");
                        datatable1.Rows[0]["Session"] = session.ToString();
                        datatable1.Rows[0]["ClassId"] = clsid.ToString();
                        datatable1.Rows[0]["StreamId"] = streamid.ToString();
                        datatable1.Rows[0]["AnnualFeeForEx"] = AnnualFeeForEx.ToString();
                        datatable1.Rows[0]["AnnualFeeForNew"] = AnnualFeeForNew.ToString();
                        datatable.Rows.Add(datatable1.Rows[0].ItemArray);
                        st.Append(datatable1.Rows[0].ToString());
                    }
                    else
                        return datatable = null;
                }
            }
            else
            {
                Hashtable ht1 = new Hashtable();
                ht1.Add("AdmnNo", null);
                ht1.Add("Session", session);
                ht1.Add("Classid", clsid);
                ht1.Add("StreamId", 1);
               DataTable datatable2 = new clsDAL().GetDataTable("FeeDetails", ht1);
                if (datatable2.Rows.Count > 0)
                {
                    double AnnualFeeForEx = Convert.ToDouble(datatable2.Rows[0]["first"].ToString()) + Convert.ToDouble(datatable2.Rows[0]["second"].ToString()) + Convert.ToDouble(datatable2.Rows[0]["third"].ToString()) + Convert.ToDouble(datatable2.Rows[0]["fourth"].ToString());
                    double AnnualFeeForNew = Convert.ToDouble(datatable2.Rows[0]["NewAdmnFee"].ToString()) + Convert.ToDouble(datatable2.Rows[0]["second"].ToString()) + Convert.ToDouble(datatable2.Rows[0]["third"].ToString()) + Convert.ToDouble(datatable2.Rows[0]["fourth"].ToString());
                    datatable2.Columns.Add("Session");
                    datatable2.Columns.Add("ClassId");
                    datatable2.Columns.Add("StreamId");
                    datatable2.Columns.Add("AnnualFeeForEx"); 
                    datatable2.Rows[0]["Session"] = session.ToString();
                    datatable2.Rows[0]["ClassId"] = clsid.ToString();
                    datatable2.Rows[0]["StreamId"] = "1";
                    datatable2.Rows[0]["AnnualFeeForEx"] = AnnualFeeForEx.ToString();
                    datatable2.Columns.Add("AnnualFeeForNew");
                    datatable2.Rows[0]["AnnualFeeForNew"] = AnnualFeeForNew.ToString();
                    st.Append(datatable2.Rows[0].ToString());
                    datatable.Rows.Add(datatable2.Rows[0].ItemArray);
                }
                else
                    return datatable = null;
            }

        }
        return datatable;
   
    }
    public DataSet GetFeeAmountByComp(string oldAdmno, string session,string quarter)
    {
        DataSet ds = new DataSet();
        int admnno; int sixth = 0; int ClassId = 0; int StreamId = 0;
        string studtype = "";
        try
        {
            DataTable dt = new clsDAL().GetDataTableQry("Select AdmnNo,SixthOptional,StudType From Ps_StudMaster where OldAdmnno='" + oldAdmno + "'");
            if (dt.Rows.Count > 0)
            {
                admnno = Convert.ToInt32(dt.Rows[0]["AdmnNo"].ToString());
                if(dt.Rows[0]["SixthOptional"].ToString() !=string.Empty)
                  sixth = Convert.ToInt32(dt.Rows[0]["SixthOptional"].ToString());
                studtype = dt.Rows[0]["StudType"].ToString();
            }
            else
                return ds = null;
            DataTable dt1 = new clsDAL().GetDataTableQry("Select Classid,Stream from Ps_ClasswiseStudent where Admnno=" + admnno + " and SessionYear='" + session + "'");
            if (dt1.Rows.Count > 0)
            {
                ClassId = Convert.ToInt32(dt1.Rows[0]["Classid"].ToString());
                StreamId = Convert.ToInt32(dt1.Rows[0]["Stream"].ToString());
            }
            else
                return ds = null;
            Hashtable ht = new Hashtable();
            ht.Add("Session", session);
            ht.Add("Classid", ClassId);
            ht.Add("StreamId", StreamId);
            ht.Add("Studtype", studtype);
            ht.Add("Quarter", quarter);
            ds = new clsDAL().GetDataSet("NewFeeCompAmnt", ht);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ClassId == 11 || ClassId == 12)
                {
                    if (sixth == 83 || sixth == 84)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (Convert.ToInt32(ds.Tables[0].Rows[i]["FeeCompId"].ToString()) == 11)
                            {
                                ds.Tables[0].Rows[i]["Amount"] = "0.00";
                                
                            }
                        }
                    
                    }

                }
            }
        }
        catch (Exception ex)
        { throw ex; }
        return ds;
    }

    public DateTime getDuedate(string p, string session)
    {
        DateTime duedate;
        switch (p)
        {
            case "APR-JUN":
                duedate = Convert.ToDateTime(new clsDAL().ExecuteScalarQry("Select StartDate from Ps_FeeNormsNew where SessionYr='" + session + "'"));
                break;

            case "JUL-SEP":
                duedate = Convert.ToDateTime(new clsDAL().ExecuteScalarQry("Select DueDate1 from Ps_FeeNormsNew where SessionYr='" + session + "'"));

                break;
            case "OCT-DEC":
                duedate = Convert.ToDateTime(new clsDAL().ExecuteScalarQry("Select DueDate2 from Ps_FeeNormsNew where SessionYr='" + session + "'"));
                break;
            case "JAN-MAR":
                duedate = Convert.ToDateTime(new clsDAL().ExecuteScalarQry("Select DueDate3 from Ps_FeeNormsNew where SessionYr='" + session + "'"));
                break;

            default:
                duedate = DateTime.Now;
                break;
        }
        return duedate;
    }

    public DataTable getFeeforOffline(string OldAdmno, DateTime dt)
    {
          int admnno=0;
         DataTable datatable = new DataTable();
         try
         {
             DataTable dtable = new clsDAL().GetDataTableQry("Select AdmnNo From Ps_StudMaster where OldAdmnno='" + OldAdmno + "'");
             if (dtable.Rows.Count > 0)
             {
                 admnno = Convert.ToInt32(dtable.Rows[0]["AdmnNo"].ToString());

                 // sixth = Convert.ToInt32(dtable.Rows[0]["SixthOptional"].ToString());
             }
             else
                 return datatable = null;

             DataTable dtnew = new clsDAL().GetDataTableQry("select sum(Debit) as Debit,Sum(Credit) as Credit,Sum(Balance) as Balance from Ps_Feeledger where admnno=" + admnno + " and Transdate='" + dt + "'");

             if (dtnew.Rows.Count > 0)
             {
                 datatable = dtnew;
                 return datatable;
             }
             else
                 return datatable = null;
         }
         catch (Exception ex)
         {
             throw ex;
         }


    }
}