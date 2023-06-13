using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Classes.DA
{
    public class clsData : IDisposable
    {
        public SqlConnection objCon = new SqlConnection(ConfigurationSettings.AppSettings["ConString"]);
        private bool m_writeLog;
        private bool m_handleError;
        public string strSql;

        public bool writeLogEvent
        {
            get
            {
                return m_writeLog;
            }
            set
            {
                m_writeLog = value;
            }
        }

        public bool HandleError
        {
            get
            {
                return m_handleError;
            }
            set
            {
                m_handleError = value;
            }
        }

        public event clsData.ErrorProvider onError;

        ~clsData()
        {
            objCon.Close();
            objCon.Dispose();
            GC.SuppressFinalize((object)this);
        }

        public bool OpenConnection()
        {
            try
            {
                if (objCon.State != ConnectionState.Open)
                    objCon.Open();
                return true;
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return false;
            }
        }

        public bool ExecuteSql()
        {
            try
            {
                OpenConnection();
                return new SqlCommand(strSql, objCon).ExecuteNonQuery() != -1;
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return false;
            }
            finally
            {
                objCon.Close();
            }
        }

        public bool ExecuteSql(string strProcedureName, Hashtable ht, SqlTransaction sqlTran)
        {
            OpenConnection();
            SqlCommand sqlCommand = new SqlCommand(strProcedureName, objCon, sqlTran);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            foreach (object key in (IEnumerable)ht.Keys)
            {
                if (ht[key].ToString() == "")
                    sqlCommand.Parameters.Add(string.Format("@{0}", (object)key.ToString()), (object)DBNull.Value);
                else
                    sqlCommand.Parameters.Add(string.Format("@{0}", (object)key.ToString()), (object)ReplaceQuote(ht[key].ToString()));
            }
            return sqlCommand.ExecuteNonQuery() != -1;
        }

        public bool ExecuteSql(string strProcedureName, Hashtable ht)
        {
            try
            {
                OpenConnection();
                SqlCommand sqlCommand = new SqlCommand(strProcedureName, objCon);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (object key in (IEnumerable)ht.Keys)
                {
                    if (ht[key].ToString() == "")
                        sqlCommand.Parameters.Add(string.Format("@{0}", (object)key.ToString()), (object)DBNull.Value);
                    else
                        sqlCommand.Parameters.Add(string.Format("@{0}", (object)key.ToString()), (object)ReplaceQuote(ht[key].ToString()));
                }
                return sqlCommand.ExecuteNonQuery() != -1;
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return false;
            }
            finally
            {
                objCon.Close();
            }
        }

        public bool ExecuteSql(string strProcedureName)
        {
            try
            {
                OpenConnection();
                SqlCommand sqlCommand = new SqlCommand(strProcedureName, objCon);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                return sqlCommand.ExecuteNonQuery() != -1;
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return false;
            }
            finally
            {
                objCon.Close();
            }
        }

        private string ReplaceQuote(string strValue)
        {
            return strValue.Trim().Replace("'", "");
        }

        public SqlDataReader GetDataReader()
        {
            try
            {
                if (!OpenConnection())
                    return (SqlDataReader)null;
                return new SqlCommand(strSql, objCon).ExecuteReader();
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return (SqlDataReader)null;
            }
        }

        public int getReturnValue(string strProcedureName, Hashtable ht)
        {
            try
            {
                OpenConnection();
                SqlCommand sqlCommand = new SqlCommand(strProcedureName, objCon);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (object key in (IEnumerable)ht.Keys)
                    sqlCommand.Parameters.Add(string.Format("@{0}", (object)key.ToString()), (object)ReplaceQuote(ht[key].ToString()));
                return (int)sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return -1;
            }
            finally
            {
                objCon.Close();
            }
        }

        public float getReturnValueFloat(string strProcedureName, Hashtable ht)
        {
            try
            {
                OpenConnection();
                SqlCommand sqlCommand = new SqlCommand(strProcedureName, objCon);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                foreach (object key in (IEnumerable)ht.Keys)
                    sqlCommand.Parameters.Add(string.Format("@{0}", (object)key.ToString()), (object)ReplaceQuote(ht[key].ToString()));
                return float.Parse(Convert.ToString(sqlCommand.ExecuteScalar()));
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return -1f;
            }
            finally
            {
                objCon.Close();
            }
        }

        public DataTable GetDataTable()
        {
            try
            {
                OpenConnection();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(strSql, objCon);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return (DataTable)null;
            }
            finally
            {
                objCon.Close();
            }
        }

        public DataSet GetDataSet(string strProcedureName, Hashtable ht)
        {
            try
            {
                OpenConnection();
                SqlCommand selectCommand = new SqlCommand(strProcedureName, objCon);
                selectCommand.CommandType = CommandType.StoredProcedure;
                foreach (object key in (IEnumerable)ht.Keys)
                    selectCommand.Parameters.Add(string.Format("@{0}", (object)key.ToString()), (object)ReplaceQuote(ht[key].ToString()));
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
                DataSet dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet);
                return dataSet;
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return (DataSet)null;
            }
            finally
            {
                objCon.Close();
            }
        }

        public int GetCount(string strTableName, string strFieldName, string strFieldValue)
        {
            try
            {
                OpenConnection();
                return (int)new SqlCommand("select count(" + strFieldName + ") from " + strTableName + " where " + strFieldName + "='" + strFieldValue + "'", objCon).ExecuteScalar();
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return -1;
            }
            finally
            {
                objCon.Close();
            }
        }

        public int GetCountAll(string strTableName)
        {
            try
            {
                OpenConnection();
                return (int)new SqlCommand("select count(*) from " + strTableName, objCon).ExecuteScalar();
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return -1;
            }
            finally
            {
                objCon.Close();
            }
        }

        public string GetFirstValue()
        {
            try
            {
                OpenConnection();
                return Convert.ToString(new SqlCommand(strSql, objCon).ExecuteScalar());
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return (string)null;
            }
            finally
            {
                objCon.Close();
            }
        }

        public int GetintValue()
        {
            try
            {
                OpenConnection();
                return (int)new SqlCommand(strSql, objCon).ExecuteScalar();
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return -1;
            }
            finally
            {
                objCon.Close();
            }
        }

        public int returnCountInt(string strTableName, string strFieldName, int intFieldValue)
        {
            try
            {
                OpenConnection();
                return (int)new SqlCommand("select count(" + strFieldName + ") from " + strTableName + " where " + strFieldName + "=" + (object)intFieldValue, objCon).ExecuteScalar();
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return -1;
            }
            finally
            {
                objCon.Close();
            }
        }

        public int CountRecord(string strSql)
        {
            try
            {
                OpenConnection();
                SqlCommand sqlCommand = new SqlCommand();
                OpenConnection();
                sqlCommand.Connection = objCon;
                sqlCommand.CommandText = strSql.Remove(7, strSql.IndexOf("FROM") - 8).Insert(7, "Count(*) ");
                return (int)sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return -1;
            }
            finally
            {
                objCon.Close();
            }
        }

        public bool IsExist(string strTableName, string strFieldName, int intFieldValue)
        {
            try
            {
                OpenConnection();
                return (int)new SqlCommand("select count(" + strFieldName + ") from " + strTableName + " where " + strFieldName + "=" + (object)intFieldValue, objCon).ExecuteScalar() > 0;
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return false;
            }
            finally
            {
                objCon.Close();
            }
        }

        public int returnValueInt()
        {
            try
            {
                OpenConnection();
                int num = -1;
                SqlDataReader dataReader = GetDataReader();
                while (dataReader.Read())
                    num = dataReader.GetInt32(0);
                return num;
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return -1;
            }
            finally
            {
                objCon.Close();
            }
        }

        public string returnValueString()
        {
            try
            {
                OpenConnection();
                string str = "-1";
                SqlDataReader dataReader = GetDataReader();
                while (dataReader.Read())
                    str = dataReader.GetString(0);
                return str;
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return (string)null;
            }
            finally
            {
                objCon.Close();
            }
        }

        public int returnNextValue(string strTableName, string strFieldName)
        {
            try
            {
                OpenConnection();
                return (int)new SqlCommand("SELECT ISNULL(MAX(" + strFieldName + ")+1,1) from " + strTableName + "", objCon).ExecuteScalar();
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return -1;
            }
            finally
            {
                objCon.Close();
            }
        }

        public DataTable GetDataTable(string strProcedureName, Hashtable ht)
        {
            try
            {
                OpenConnection();
                SqlCommand selectCommand = new SqlCommand(strProcedureName, objCon);
                selectCommand.CommandType = CommandType.StoredProcedure;
                foreach (object key in (IEnumerable)ht.Keys)
                    selectCommand.Parameters.Add(string.Format("@{0}", (object)key.ToString()), (object)ReplaceQuote(ht[key].ToString()));
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return (DataTable)null;
            }
            finally
            {
                objCon.Close();
            }
        }

        public DataTable GetDataTable(string strProcedureName)
        {
            try
            {
                OpenConnection();
                SqlCommand selectCommand = new SqlCommand(strProcedureName, objCon);
                selectCommand.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                return dataTable;
            }
            catch (Exception ex)
            {
                CatchErr(ex);
                return (DataTable)null;
            }
            finally
            {
                objCon.Close();
            }
        }

        public void Dispose()
        {
            objCon.Close();
            objCon.Dispose();
            GC.SuppressFinalize((object)this);
        }

        private void CatchErr(Exception e)
        {
            if (!m_handleError)
                throw e;
            try
            {
                onError(e);
            }
            catch
            {
            }
        }

        public delegate void ErrorProvider(Exception e);
    }
}
