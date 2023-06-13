using AnthemUtility.AnthemSecurityEngine;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Classes.DA
{
    public class Common
    {
        public SqlConnection con = new SqlConnection();

        public Common()
        {

            con.ConnectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
        }

        public int GetID(string sp)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand sqlCommand = new SqlCommand(sp, con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                StructDAL.empID = Convert.ToInt32(sqlCommand.ExecuteScalar().ToString());
                return Convert.ToInt32(sqlCommand.ExecuteScalar().ToString());
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public DataSet GetDatasetQry(string qry)
        {
            DataSet dataSet = new DataSet();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand selectCommand = new SqlCommand(qry, con);
                selectCommand.CommandTimeout = 0;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
                sqlDataAdapter.Fill(dataSet);
                con.Close();
                selectCommand.Dispose();
                sqlDataAdapter.Dispose();
                return dataSet;
            }
            catch (Exception ex)
            {
            }
            return dataSet;
        }

        public void UpdateUserMod(string uid, string PSD, string PED, string status)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand sqlCommand = new SqlCommand("update PS_USER_MASTER set PERM_START_DATE='" + PSD + "',PERM_EXP_DATE='" + PED + "',Rights='" + status + "'where USER_ID='" + (object)Convert.ToInt32(uid) + "'", con);
            sqlCommand.CommandTimeout = 0;
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            con.Close();
        }

        public void ResetUser(string uid)
        {
            string str = CryptoEngine.Encrypt("password", "aGtSpL", "114208513", "SHA1", 5, "ANTHEMGLOBALTECH", 256);
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand sqlCommand = new SqlCommand("update PS_USER_MASTER set PW='" + str + "' where USER_ID=" + uid, con);
            sqlCommand.CommandTimeout = 0;
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            con.Close();
        }

        public void DeleteRecord(string sp)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlCommand sqlCommand = new SqlCommand(sp, con);
            sqlCommand.CommandTimeout = 0;
            sqlCommand.ExecuteNonQuery();
            con.Close();
            sqlCommand.Dispose();
        }

        public void ExcuteQryInsUpdt(string qry)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand sqlCommand = new SqlCommand(qry, con);
                sqlCommand.CommandTimeout = 0;
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Dispose();
                if (con.State != ConnectionState.Open)
                    return;
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExcuteProcInsUpdt(string sp, Hashtable ht)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand sqlCommand = new SqlCommand(sp, con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 0;
                foreach (DictionaryEntry dictionaryEntry in ht)
                    sqlCommand.Parameters.Add((string)dictionaryEntry.Key, dictionaryEntry.Value);
                sqlCommand.ExecuteNonQuery();
                sqlCommand.Dispose();
                if (con.State != ConnectionState.Open)
                    return;
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ExecuteSql(string qry)
        {
            DataTable dataTable = new DataTable();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand selectCommand = new SqlCommand(qry, con);
                selectCommand.CommandTimeout = 0;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
                sqlDataAdapter.Fill(dataTable);
                con.Close();
                selectCommand.Dispose();
                sqlDataAdapter.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataTable;
        }

        public DataSet ExecuteSql1(string qry)
        {
            DataSet dataSet = new DataSet();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand selectCommand = new SqlCommand(qry, con);
                selectCommand.CommandTimeout = 0;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
                sqlDataAdapter.Fill(dataSet);
                con.Close();
                selectCommand.Dispose();
                sqlDataAdapter.Dispose();
            }
            catch
            {
                throw;
            }
            return dataSet;
        }

        public string ExecuteSql(string qry, string s)
        {
            string str;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand sqlCommand = new SqlCommand(qry, con);
                sqlCommand.CommandTimeout = 0;
                str = Convert.ToString(sqlCommand.ExecuteScalar());
                sqlCommand.Dispose();
                con.Close();
            }
            catch
            {
                throw;
            }
            return str;
        }

        public string ExecuteScalarQry(string qry)
        {
            string str;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand sqlCommand = new SqlCommand(qry, con);
                sqlCommand.CommandTimeout = 0;
                str = Convert.ToString(sqlCommand.ExecuteScalar());
                sqlCommand.Dispose();
                con.Close();
            }
            catch
            {
                throw;
            }
            return str;
        }

        public DataTable CheckDuplicateRecords(string qry)
        {
            DataTable dataTable = new DataTable();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand selectCommand = new SqlCommand(qry, con);
                selectCommand.CommandTimeout = 0;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
                sqlDataAdapter.Fill(dataTable);
                con.Close();
                selectCommand.Dispose();
                sqlDataAdapter.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dataTable;
        }

        public DataTable GetDataTable(string sp, Hashtable ht)
        {
            DataTable dataTable = new DataTable();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand selectCommand = new SqlCommand(sp, con);
                selectCommand.CommandType = CommandType.StoredProcedure;
                selectCommand.CommandTimeout = 0;
                foreach (DictionaryEntry dictionaryEntry in ht)
                    selectCommand.Parameters.Add((string)dictionaryEntry.Key, dictionaryEntry.Value);
                new SqlDataAdapter(selectCommand).Fill(dataTable);
                con.Close();
                selectCommand.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                if (con != null)
                    con = (SqlConnection)null;
            }
            return dataTable;
        }

        public string ExecSqlWithHT(string sp, Hashtable ht)
        {
            string str = "";
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand sqlCommand = new SqlCommand(sp, con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 0;
                foreach (DictionaryEntry dictionaryEntry in ht)
                    sqlCommand.Parameters.Add((string)dictionaryEntry.Key, dictionaryEntry.Value);
                sqlCommand.ExecuteNonQuery();
                con.Close();
                sqlCommand.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                if (con != null)
                    con = (SqlConnection)null;
            }
            return str;
        }

        public DataSet GetDataSet(string sp, Hashtable ht)
        {
            DataSet dataSet = new DataSet();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand selectCommand = new SqlCommand(sp, con);
                selectCommand.CommandType = CommandType.StoredProcedure;
                selectCommand.CommandTimeout = 0;
                foreach (DictionaryEntry dictionaryEntry in ht)
                    selectCommand.Parameters.Add((string)dictionaryEntry.Key, dictionaryEntry.Value);
                new SqlDataAdapter(selectCommand).Fill(dataSet);
                con.Close();
                selectCommand.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                if (con != null)
                    con = (SqlConnection)null;
            }
            return dataSet;
        }

        public DataTable GetDataTable(string sp)
        {
            DataTable dataTable = new DataTable();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand selectCommand = new SqlCommand(sp, con);
                selectCommand.CommandType = CommandType.StoredProcedure;
                selectCommand.CommandTimeout = 0;
                new SqlDataAdapter(selectCommand).Fill(dataTable);
                con.Close();
                selectCommand.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                if (con != null)
                    con = (SqlConnection)null;
            }
            return dataTable;
        }

        public DataSet GetDataSet(string sp)
        {
            DataSet dataSet = new DataSet();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand selectCommand = new SqlCommand(sp, con);
                selectCommand.CommandType = CommandType.StoredProcedure;
                selectCommand.CommandTimeout = 0;
                new SqlDataAdapter(selectCommand).Fill(dataSet);
                con.Close();
                selectCommand.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                if (con != null)
                    con = (SqlConnection)null;
            }
            return dataSet;
        }
    }
}
