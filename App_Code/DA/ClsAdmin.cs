using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Classes.DA
{
    public class ClsAdmin
    {
        private SqlConnection con = new SqlConnection();

        public ClsAdmin()
        {
            this.con.ConnectionString = ConfigurationManager.AppSettings["conString"];
        }

        public string GetScalar(string qry)
        {
            if (this.con.State == ConnectionState.Closed)
                this.con.Open();
            SqlCommand sqlCommand = new SqlCommand(qry, this.con);
            string str = Convert.ToString(sqlCommand.ExecuteScalar());
            this.con.Close();
            sqlCommand.Dispose();
            return str;
        }

        public DataTable GetDatatable(string qry)
        {
            if (this.con.State == ConnectionState.Closed)
                this.con.Open();
            SqlCommand selectCommand = new SqlCommand(qry, this.con);
            DataTable dataTable = new DataTable();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            sqlDataAdapter.Fill(dataTable);
            this.con.Close();
            selectCommand.Dispose();
            sqlDataAdapter.Dispose();
            return dataTable;
        }

        public DataSet GetDataset(string qry)
        {
            if (this.con.State == ConnectionState.Closed)
                this.con.Open();
            SqlCommand selectCommand = new SqlCommand(qry, this.con);
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            sqlDataAdapter.Fill(dataSet);
            this.con.Close();
            selectCommand.Dispose();
            sqlDataAdapter.Dispose();
            return dataSet;
        }

        public void ResetUser(string uid)
        {
            byte[] bytes = Encoding.ASCII.GetBytes("ps");
            for (int index = 0; index < bytes.Length; ++index)
                bytes[index] = Convert.ToByte(Convert.ToInt32(bytes[index]) + 4);
            string str = Encoding.ASCII.GetString(bytes);
            SqlConnection connection = new SqlConnection(ConfigurationSettings.AppSettings["constring"]);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            SqlCommand sqlCommand = new SqlCommand("update PS_USER_MASTER set PW='" + str + "' where USER_ID=" + uid, connection);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            connection.Close();
        }

        public void UpdateUserMod(string uid, DateTime PSD, DateTime PED, string status)
        {
            SqlConnection connection = new SqlConnection(ConfigurationSettings.AppSettings["constring"]);
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            SqlCommand sqlCommand = new SqlCommand("update PS_USER_MASTER set PERM_START_DATE='" + (object)PSD + "',PERM_EXP_DATE='" + (object)PED + "',Rights='" + status + "'where USER_ID='" + (object)Convert.ToInt32(uid) + "'", connection);
            sqlCommand.ExecuteNonQuery();
            sqlCommand.Dispose();
            connection.Close();
        }

        public void ExecuteQuery(string qry)
        {
            SqlConnection connection = new SqlConnection(ConfigurationSettings.AppSettings["constring"]);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand sqlCommand = new SqlCommand(qry, connection);
                sqlCommand.ExecuteNonQuery();
                connection.Close();
                sqlCommand.Dispose();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            finally
            {
                if (this.con.State == ConnectionState.Open)
                    connection.Close();
                if (this.con != null)
                    this.con = (SqlConnection)null;
                if (this.con != null)
                    this.con = (SqlConnection)null;
            }
        }

        public DataTable select_User(string uid)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationSettings.AppSettings["constring"]);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand selectCommand = new SqlCommand("select * from PS_USER_MASTER where USER_ID='" + uid + "'", connection);
                DataSet dataSet = new DataSet();
                new SqlDataAdapter(selectCommand).Fill(dataSet);
                return dataSet.Tables[0].Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.con.State == ConnectionState.Open)
                    this.con.Close();
                if (this.con != null)
                    this.con = (SqlConnection)null;
            }
        }

        public DataTable select_User(string uname, string pw)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationSettings.AppSettings["constring"]);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand selectCommand = new SqlCommand("select * from PS_USER_MASTER where USER_NAME='" + uname + "' and PW = '" + pw + "'", connection);
                DataSet dataSet = new DataSet();
                new SqlDataAdapter(selectCommand).Fill(dataSet);
                return dataSet.Tables[0].Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.con.State == ConnectionState.Open)
                    this.con.Close();
                if (this.con != null)
                    this.con = (SqlConnection)null;
            }
        }

        public DataTable GetDatatable(string sp, Hashtable ht)
        {
            this.con.ConnectionString = ConfigurationSettings.AppSettings["constring"];
            if (this.con.State == ConnectionState.Closed)
                this.con.Open();
            SqlCommand selectCommand = new SqlCommand(sp, this.con);
            selectCommand.CommandType = CommandType.StoredProcedure;
            foreach (DictionaryEntry dictionaryEntry in ht)
                selectCommand.Parameters.Add("@" + (string)dictionaryEntry.Key, dictionaryEntry.Value);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            this.con.Close();
            selectCommand.Dispose();
            return dataTable;
        }

        public DataTable GetDataTable(string sp)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationSettings.AppSettings["constring"]);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand selectCommand = new SqlCommand(sp, connection);
                selectCommand.CommandType = CommandType.StoredProcedure;
                new SqlDataAdapter(selectCommand).Fill(dataTable);
                connection.Close();
                selectCommand.Dispose();
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
            return dataTable;
        }

        public DataTable select_UserName(string uname)
        {
            DataTable dataTable = new DataTable();
            try
            {
                SqlConnection connection = new SqlConnection(ConfigurationSettings.AppSettings["constring"]);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                SqlCommand selectCommand = new SqlCommand("select * from PS_USER_MASTER where USER_NAME='" + uname + "'", connection);
                DataSet dataSet = new DataSet();
                new SqlDataAdapter(selectCommand).Fill(dataSet);
                return dataSet.Tables[0].Copy();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.con.State == ConnectionState.Open)
                    this.con.Close();
                if (this.con != null)
                    this.con = (SqlConnection)null;
            }
        }

        public string DeleteRecords(string sp, Hashtable ht)
        {
            if (this.con.State == ConnectionState.Closed)
                this.con.Open();
            SqlCommand sqlCommand = new SqlCommand(sp, this.con);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            foreach (DictionaryEntry dictionaryEntry in ht)
                sqlCommand.Parameters.Add((string)dictionaryEntry.Key, dictionaryEntry.Value);
            SqlParameter sqlParameter = new SqlParameter("@retval", SqlDbType.BigInt);
            sqlParameter.Direction = ParameterDirection.Output;
            sqlCommand.Parameters.Add(sqlParameter);
            sqlCommand.ExecuteNonQuery();
            string str = Convert.ToString(sqlParameter.Value);
            this.con.Close();
            sqlCommand.Dispose();
            return str;
        }

        public static string getdata(string textstring, int len)
        {
            if (textstring.Length > len)
            {
                textstring = textstring.Substring(0, len);
                textstring = ClsAdmin.getdata(textstring, len);
            }
            else if (textstring.Length == len)
            {
                textstring = textstring.Substring(0, textstring.LastIndexOf(" "));
                textstring += "..";
            }
            return textstring;
        }

        public static string noimage(string imagepath)
        {
            if (imagepath == "")
                return "noimage.jpg";
            return imagepath;
        }

        public static string pathfind(string name, int i)
        {
            if (i == 1)
                name = "../Gallery/ADS/" + name;
            else if (i == 2)
                name = "../Gallery/ALS/" + name;
            else if (i == 3)
                name = "../Gallery/AVR/" + name;
            else if (i == 4)
                name = "../Gallery/AVS/" + name;
            return name;
        }
    }
}
