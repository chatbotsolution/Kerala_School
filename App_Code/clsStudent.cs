using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class clsStudent
{
    private SqlConnection con;

    public clsStudent()
    {
        this.con = new SqlConnection();
        this.con.ConnectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    }

    public void Bulkinsert(string sp, DataTable tb, Hashtable htextraparam)
    {
        DataSet dataSet = new DataSet();
        try
        {
            if (this.con.State == ConnectionState.Closed)
                this.con.Open();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter();
            sqlDataAdapter.InsertCommand = new SqlCommand(sp, this.con);
            sqlDataAdapter.InsertCommand.CommandType = CommandType.StoredProcedure;
            foreach (DictionaryEntry dictionaryEntry in htextraparam)
                sqlDataAdapter.InsertCommand.Parameters.AddWithValue((string)dictionaryEntry.Key, dictionaryEntry.Value);
            sqlDataAdapter.InsertCommand.Parameters.Add("@SiblingAdmnNo", SqlDbType.BigInt, Convert.ToInt32(2147483646), "SiblingAdmnNo");
            sqlDataAdapter.InsertCommand.Parameters.Add("@SiblingClass", SqlDbType.Int, 2147483646, "ClassId");
            sqlDataAdapter.InsertCommand.Parameters.Add("@SiblingNo", SqlDbType.Int, 2147483646, "SiblingNo");
            sqlDataAdapter.Update(tb);
            this.con.Close();
        }
        catch (Exception ex)
        {
        }
        finally
        {
            if (this.con.State == ConnectionState.Open)
                this.con.Close();
            if (this.con != null)
                this.con = (SqlConnection)null;
        }
    }
}
