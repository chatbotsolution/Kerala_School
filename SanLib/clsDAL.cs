using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SanLib
{
    public class clsDAL
    {
        public SqlConnection con = new SqlConnection();

        public clsDAL()
        {
            con.ConnectionString = ConfigurationSettings.AppSettings["conString"].ToString();
            con.ConnectionTimeout.Equals(0);
        }

        public static void GetInfo()
        {
        }

        public DataTable GetDataTableQry(string qry)
        {
            DataTable dataTable = new DataTable();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand selectCommand = new SqlCommand(qry, con);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
                sqlDataAdapter.Fill(dataTable);
                con.Close();
                selectCommand.Dispose();
                sqlDataAdapter.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
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
            }
            return dataTable;
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
            }
            return dataTable;
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
            }
            return dataSet;
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
            }
            return dataSet;
        }

        public DataSet GetDatasetQry(string qry)
        {
            DataSet dataSet = new DataSet();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand selectCommand = new SqlCommand(qry, con);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
                sqlDataAdapter.Fill(dataSet);
                con.Close();
                selectCommand.Dispose();
                sqlDataAdapter.Dispose();
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public void ExcuteQryInsUpdt(string qry)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand sqlCommand = new SqlCommand(qry, con);
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
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
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
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public void ExcuteProcInsUpdt(string sp)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand sqlCommand = new SqlCommand(sp, con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
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
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public string ExecuteScalarQry(string qry)
        {
            string str = "";
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand sqlCommand = new SqlCommand(qry, con);
                str = Convert.ToString(sqlCommand.ExecuteScalar());
                sqlCommand.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return str;
        }

        public string ExecuteScalar(string sp, Hashtable ht)
        {
            string str = "";
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand sqlCommand = new SqlCommand(sp, con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (DictionaryEntry dictionaryEntry in ht)
                    sqlCommand.Parameters.Add((string)dictionaryEntry.Key, dictionaryEntry.Value);
                str = Convert.ToString(sqlCommand.ExecuteScalar());
                sqlCommand.Dispose();
                con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            return str;
        }

        public SqlDataReader GetDataReader(string qry)
        {
            if (con.State == ConnectionState.Closed)
                con.Open();
            SqlDataReader sqlDataReader = new SqlCommand(qry, con).ExecuteReader();
            con.Close();
            return sqlDataReader;
        }

        public SqlDataReader GetDataReader(string sp, Hashtable ht)
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand sqlCommand = new SqlCommand(sp, con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (DictionaryEntry dictionaryEntry in ht)
                    sqlCommand.Parameters.Add((string)dictionaryEntry.Key, dictionaryEntry.Value);
                sqlCommand.Connection.Open();
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
                con.Close();
                return sqlDataReader;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public void BulkCopyTable(DataTable dt, string DestTable)
        {
            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.TableLock, (SqlTransaction)null);
            sqlBulkCopy.BatchSize = dt.Rows.Count;
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                sqlBulkCopy.DestinationTableName = DestTable;
                sqlBulkCopy.WriteToServer(dt);
                sqlBulkCopy.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        public string Decrypt(string TextToBeDecrypted, string Password)
        {
            string strPassword = Password + "7087";
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            string str;
            try
            {
                byte[] buffer = Convert.FromBase64String(TextToBeDecrypted);
                byte[] bytes = Encoding.ASCII.GetBytes(strPassword.Length.ToString());
                PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(strPassword, bytes);
                ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
                MemoryStream memoryStream = new MemoryStream(buffer);
                CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] numArray = new byte[buffer.Length];
                int count = cryptoStream.Read(numArray, 0, numArray.Length);
                memoryStream.Close();
                cryptoStream.Close();
                str = Encoding.Unicode.GetString(numArray, 0, count);
            }
            catch
            {
                str = TextToBeDecrypted;
            }
            return str;
        }

        public string Encrypt(string TextToBeEncrypted, string Password)
        {
            string strPassword = Password + "7087";
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            byte[] bytes1 = Encoding.Unicode.GetBytes(TextToBeEncrypted);
            byte[] bytes2 = Encoding.ASCII.GetBytes(strPassword.Length.ToString());
            PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(strPassword, bytes2);
            ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(passwordDeriveBytes.GetBytes(32), passwordDeriveBytes.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(bytes1, 0, bytes1.Length);
            cryptoStream.FlushFinalBlock();
            byte[] array = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(array);
        }
    }
}
