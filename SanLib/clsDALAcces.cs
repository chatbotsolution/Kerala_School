using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;

namespace SanLib
{
    public class clsDALAcces
    {
        public OleDbConnection con = new OleDbConnection();

        public clsDALAcces()
        {
            this.con.ConnectionString = ConfigurationSettings.AppSettings["conString"].ToString();
            this.con.ConnectionTimeout.Equals(0);
        }

        public static void GetInfo()
        {
        }

        public DataTable GetDataTableQry(string qry)
        {
            DataTable dataTable = new DataTable();
            try
            {
                if (this.con.State == ConnectionState.Closed)
                    this.con.Open();
                OleDbCommand selectCommand = new OleDbCommand(qry, this.con);
                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommand);
                oleDbDataAdapter.Fill(dataTable);
                this.con.Close();
                selectCommand.Dispose();
                oleDbDataAdapter.Dispose();
                return dataTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.con.State == ConnectionState.Open)
                    this.con.Close();
            }
        }

        public DataSet GetDatasetQry(string qry)
        {
            DataSet dataSet = new DataSet();
            try
            {
                if (this.con.State == ConnectionState.Closed)
                    this.con.Open();
                OleDbCommand selectCommand = new OleDbCommand(qry, this.con);
                OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(selectCommand);
                oleDbDataAdapter.Fill(dataSet);
                this.con.Close();
                selectCommand.Dispose();
                oleDbDataAdapter.Dispose();
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.con.State == ConnectionState.Open)
                    this.con.Close();
            }
        }

        public void ExcuteQryInsUpdt(string qry)
        {
            try
            {
                if (this.con.State == ConnectionState.Closed)
                    this.con.Open();
                OleDbCommand oleDbCommand = new OleDbCommand(qry, this.con);
                oleDbCommand.ExecuteNonQuery();
                oleDbCommand.Dispose();
                if (this.con.State != ConnectionState.Open)
                    return;
                this.con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.con.State == ConnectionState.Open)
                    this.con.Close();
            }
        }

        public string ExecuteScalarQry(string qry)
        {
            string str = "";
            try
            {
                if (this.con.State == ConnectionState.Closed)
                    this.con.Open();
                OleDbCommand oleDbCommand = new OleDbCommand(qry, this.con);
                str = Convert.ToString(oleDbCommand.ExecuteScalar());
                oleDbCommand.Dispose();
                this.con.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (this.con.State == ConnectionState.Open)
                    this.con.Close();
            }
            return str;
        }
    }
}
