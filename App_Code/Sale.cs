using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class Sale
{
    private SqlConnection con = new SqlConnection();

    public Sale()
    {
        this.con.ConnectionString = ConfigurationManager.AppSettings["conString"];
    }

    public string SaveSales(DataTable dtDetails, Hashtable htSale, out long InvNo)
    {
        if (this.con.State == ConnectionState.Closed)
            this.con.Open();
        SqlTransaction transaction = this.con.BeginTransaction();
        try
        {
            SqlCommand sqlCommand = new SqlCommand("dbo.ACTS_InsUpdtItemSale", this.con, transaction);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            foreach (DictionaryEntry dictionaryEntry in htSale)
                sqlCommand.Parameters.AddWithValue((string)dictionaryEntry.Key, dictionaryEntry.Value);
            string s = sqlCommand.ExecuteScalar().ToString();
            if (!long.TryParse(s, out InvNo))
            {
                transaction.Rollback();
                return s;
            }
            if (dtDetails.Rows[0]["ItemCode"].ToString().Trim().Equals(string.Empty))
                dtDetails.Rows.RemoveAt(0);
            foreach (DataRow row in (InternalDataCollectionBase)dtDetails.Rows)
            {
                if (Convert.ToDecimal(row["Amount"].ToString()) > new Decimal(0))
                {
                    sqlCommand = new SqlCommand("dbo.ACTS_InsUpdtItemSaleDetails", this.con, transaction);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("@InvId", (object)InvNo);
                    sqlCommand.Parameters.AddWithValue("@ItemCode", row["ItemCode"]);
                    sqlCommand.Parameters.AddWithValue("@Qty", row["Qty"]);
                    sqlCommand.Parameters.AddWithValue("@SalePrice", row["SalePrice"]);
                    sqlCommand.Parameters.AddWithValue("@DiscRate", (object)0);
                    sqlCommand.Parameters.AddWithValue("@DiscAmt", row["Discount"]);
                    sqlCommand.Parameters.AddWithValue("@CST_VAT_Rate", row["TaxRate"]);
                    sqlCommand.Parameters.AddWithValue("@CST_VAT_Amt", row["TaxAmount"]);
                    sqlCommand.Parameters.AddWithValue("@TotAmt", row["Amount"]);
                    sqlCommand.ExecuteNonQuery();
                }
            }
            transaction.Commit();
            if (this.con.State == ConnectionState.Open)
                this.con.Close();
            sqlCommand.Dispose();
            return "Data saved successfully !";
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            if (this.con.State == ConnectionState.Open)
                this.con.Close();
            InvNo = 0L;
            return "Data saved failed. Please try again !";
        }
    }
}
