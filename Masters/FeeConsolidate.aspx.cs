using ASP;
using SanLib;
using System;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_FeeConsolidate : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnConsolidt_Click(object sender, EventArgs e)
    {
        if (obj.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=5").ToString().Trim() == "")
        {
            ScriptManager.RegisterStartupScript((Page)this, GetType(), "alertMessage", "alert('No Account Head Set To Receive Fee!! Please Set Account Head To Continue!!'); window.location='" + Request.ApplicationPath + "/Masters/SetAccountHead.aspx'", true);
        }
        else
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTableQry1 = obj.GetDataTableQry("Select InvoiceReceiptNo,Description from dbo.Acts_PaymentReceiptVoucher where TransDt>='" + Convert.ToDateTime("1 Apr 2014 00:00:00") + "' and PartyId>2000000000 and LinkForm='fr' and Payment_Receipt='R'");
            if (dataTableQry1.Rows.Count > 0)
            {
                Decimal num = new Decimal(0);
                DataTable dataTableQry2 = obj.GetDataTableQry("Select AcctsHeadId from dbo.Acts_AccountHeads where AcctsHeadId=3 or AG_Code=7");
                string str1 = "";
                for (int index = 0; index < dataTableQry2.Rows.Count; ++index)
                    str1 = str1 + dataTableQry2.Rows[index]["AcctsHeadId"].ToString() + ",";
                string str2 = str1.Substring(0, str1.LastIndexOf(","));
                foreach (DataRow row1 in (InternalDataCollectionBase)dataTableQry1.Rows)
                {
                    DataTable dataTable2 = new DataTable();
                    DataTable dataTable3 = new DataTable();
                    DataTable dataTableQry3 = obj.GetDataTableQry("Select * from dbo.ACTS_GenLedger where PmtRecptVoucherNo=cast(" + row1["InvoiceReceiptNo"].ToString() + " as nvarchar) and AccountHead not in (select AcctsHeadId from PS_AdditionalFeeMaster) and Particulars not Like 'Cancelled%' and Particulars not Like '%Bus Fee%'");
                    if (dataTableQry3.Rows.Count > 1)
                    {
                        foreach (DataRow dataRow in dataTableQry3.Select("AccountHead in (" + str2 + ")"))
                            num += Convert.ToDecimal(dataRow["TransAmt"].ToString());
                        for (int index = 2; index < dataTableQry3.Rows.Count; ++index)
                            dataTableQry3.Rows[index].Delete();
                        dataTableQry3.AcceptChanges();
                        obj.ExecuteScalarQry("delete from dbo.ACTS_GenLedger where PmtRecptVoucherNo=cast(" + row1["InvoiceReceiptNo"].ToString() + " as nvarchar) and AccountHead not in (select AcctsHeadId from PS_AdditionalFeeMaster) and Particulars not Like 'Cancelled%' and Particulars not Like '%Bus Fee%'");
                        foreach (DataRow row2 in (InternalDataCollectionBase)dataTableQry3.Rows)
                        {
                            row2["TransAmt"] = num;
                            if (row2["CrDr"].ToString().Trim() == "CR")
                                row2["AccountHead"] = obj.ExecuteScalarQry("select AcctsHeadId from PS_AdditionalFeeMaster where Ad_Id=5").ToString();
                            row2["Particulars"] = row1["Description"].ToString();
                        }
                        num = new Decimal(0);
                        dataTableQry3.AcceptChanges();
                        obj.BulkCopyTable(dataTableQry3, "Acts_GenLedger");
                    }
                }
                lblMsg.Text = "Records Updated Sucessfully";
                lblMsg.ForeColor = Color.Green;
            }
            else
            {
                lblMsg.Text = "Sorry No Records Found!!";
                lblMsg.ForeColor = Color.Red;
            }
        }
    }

    protected void btnInit_Click(object sender, EventArgs e)
    {
        if (obj.ExecuteScalarQry("select AcctsHeadId from dbo.PS_AdditionalFeeMaster where Ad_Id=5").ToString().Trim() == "")
        {
            ScriptManager.RegisterStartupScript((Page)this, GetType(), "alertMessage", "alert('No Account Head Set To Receive Fee!! Please Set Account Head To Continue!!'); window.location='" + Request.ApplicationPath + "/Masters/SetAccountHead.aspx'", true);
        }
        else
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTableQry1 = obj.GetDataTableQry("Select InvoiceReceiptNo from dbo.Acts_PaymentReceiptVoucher where Description Like '%AC Initialized%' and PartyId>2000000000 and LinkForm='fr' and Payment_Receipt='R'");
            if (dataTableQry1.Rows.Count > 0)
            {
                Decimal num = new Decimal(0);
                DataTable dataTableQry2 = obj.GetDataTableQry("Select AcctsHeadId from dbo.Acts_AccountHeads where AcctsHeadId=3 or AG_Code=7");
                string str1 = "";
                for (int index = 0; index < dataTableQry2.Rows.Count; ++index)
                    str1 = str1 + dataTableQry2.Rows[index]["AcctsHeadId"].ToString() + ",";
                string str2 = str1.Substring(0, str1.LastIndexOf(","));
                foreach (DataRow row1 in (InternalDataCollectionBase)dataTableQry1.Rows)
                {
                    DataTable dataTable2 = new DataTable();
                    DataTable dataTable3 = new DataTable();
                    DataTable dataTableQry3 = obj.GetDataTableQry("Select * from dbo.ACTS_GenLedger where PmtRecptVoucherNo=cast(" + row1["InvoiceReceiptNo"].ToString() + " as nvarchar) and AccountHead not in (select AcctsHeadId from PS_AdditionalFeeMaster) and Particulars not Like 'Cancelled%' and Particulars not Like '%Bus Fee%'");
                    if (dataTableQry3.Rows.Count > 1)
                    {
                        foreach (DataRow dataRow in dataTableQry3.Select("AccountHead in (" + str2 + ")"))
                            num += Convert.ToDecimal(dataRow["TransAmt"].ToString());
                        for (int index = 2; index < dataTableQry3.Rows.Count; ++index)
                            dataTableQry3.Rows[index].Delete();
                        dataTableQry3.AcceptChanges();
                        obj.ExecuteScalarQry("delete from dbo.ACTS_GenLedger where PmtRecptVoucherNo=cast(" + row1["InvoiceReceiptNo"].ToString() + " as nvarchar) and AccountHead not in (select AcctsHeadId from PS_AdditionalFeeMaster) and Particulars not Like 'Cancelled%' and Particulars not Like '%Bus Fee%'");
                        foreach (DataRow row2 in (InternalDataCollectionBase)dataTableQry3.Rows)
                        {
                            row2["TransAmt"] = num;
                            if (row2["CrDr"].ToString().Trim() == "CR")
                                row2["AccountHead"] = obj.ExecuteScalarQry("select AcctsHeadId from PS_AdditionalFeeMaster where Ad_Id=5").ToString();
                        }
                        num = new Decimal(0);
                        dataTableQry3.AcceptChanges();
                        obj.BulkCopyTable(dataTableQry3, "Acts_GenLedger");
                    }
                }
                lblMsg.Text = "Records Updated Sucessfully";
                lblMsg.ForeColor = Color.Green;
            }
            else
            {
                lblMsg.Text = "Sorry No Records Found!!";
                lblMsg.ForeColor = Color.Red;
            }
        }
    }
}