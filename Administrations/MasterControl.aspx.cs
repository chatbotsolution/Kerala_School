using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administrations_MasterControl : System.Web.UI.Page
{
    
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnGenBus_Click(object sender, EventArgs e)
    {
        int num1 = 0;
        Hashtable ht1 = new Hashtable();
        DataSet dataSet1 = new DataSet();
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        ht1.Add((object)"@SYr", (object)clsGenerateFee.CreateCurrSession().ToString());
        DataSet dataSet2 = obj.GetDataSet("ps_sp_GetStudBusLedger", ht1);
        if (dataSet2.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow row in (InternalDataCollectionBase)dataSet2.Tables[0].Rows)
            {
                ht1.Clear();
                ht1.Add((object)"@AdmnNo", row["AdmnNo"]);
                ht1.Add((object)"uid", (object)Convert.ToInt32(Session["User_Id"]));
                ht1.Add((object)"Session", (object)clsGenerateFee.CreateCurrSession().ToString());
                ht1.Add((object)"SchoolId", (object)Session["SchoolId"].ToString().Trim());
                if (obj.ExecuteScalar("ps_sp_insert_AdFeeLedgerBus", ht1).Trim() == "")
                    ++num1;
            }
            lblMsg.Text = "Total Records Updated In AdFeeLedger: " + (object)num1;
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = "No Records Found";
            lblMsg.ForeColor = Color.Green;
        }
        int num2 = 0;
        Hashtable ht2 = new Hashtable();
        if (dataSet2.Tables[1].Rows.Count > 0)
        {
            foreach (DataRow row in (InternalDataCollectionBase)dataSet2.Tables[1].Rows)
            {
                ht2.Clear();
                ht2.Add((object)"@AdmnNo", row["AdmnNo"]);
                ht2.Add((object)"uid", (object)Convert.ToInt32(Session["User_Id"]));
                ht2.Add((object)"Session", (object)clsGenerateFee.CreateCurrSession().ToString());
                ht2.Add((object)"SchoolId", (object)Session["SchoolId"].ToString().Trim());
                if (obj.ExecuteScalar("ps_sp_GenAdFeeLedgerBus", ht2).Trim() == "")
                    ++num2;
            }
            lblMsg2.Text = "Total Records Updated In BusHostelChoice: " + (object)num2;
            lblMsg2.ForeColor = Color.Green;
        }
        else
        {
            lblMsg2.Text = "No Records Found";
            lblMsg2.ForeColor = Color.Green;
        }
    }

    protected void btnGenLedger_Click(object sender, EventArgs e)
    {
        DataTable dataTable1 = new DataTable();
        Hashtable ht = new Hashtable();
        int num = 0;
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        foreach (DataRow row in (InternalDataCollectionBase)obj.GetDataTableQry("select * from Acts_PaymentReceiptVoucher where TransDt>'31-Mar-2014' order by PR_Id").Rows)
        {
            DataTable dataTable2 = new DataTable();
            DataTable dataTableQry1 = obj.GetDataTableQry("select isnull(SUM(fl.Credit),0)+isnull(fi.Credit,0) as Amnt from PS_FeeLedger fl left join PS_FineLedger fi on fi.Receipt_VrNo=fl.Receipt_VrNo where fl.Receipt_VrNo='" + row["InvoiceReceiptNo"].ToString().Trim() + "' and fl.Credit>0 group by fi.Credit ");
            DataTable dataTable3 = new DataTable();
            DataTable dataTableQry2 = obj.GetDataTableQry("select isnull(sum(Credit),0) as Amnt from PS_AdFeeLedger where Receipt_No='" + row["InvoiceReceiptNo"].ToString().Trim() + "' and Credit>0");
            if (row["Description"].ToString().Trim() == "Prospectus Sale")
            {
                ht.Clear();
                ht.Add((object)"@PR_Id", (object)row["PR_Id"].ToString().Trim());
                ht.Add((object)"@SessionYr", (object)row["SessionYr"].ToString().Trim());
                ht.Add((object)"@TransDate", (object)row["TransDt"].ToString().Trim());
                ht.Add((object)"@ReceiptNo", (object)row["InvoiceReceiptNo"].ToString().Trim());
                ht.Add((object)"@Particulars", (object)row["Description"].ToString().Trim());
                ht.Add((object)"@Amount", (object)row["Amount"].ToString().Trim());
                ht.Add((object)"@UserId", (object)row["UserId"].ToString().Trim());
                ht.Add((object)"@UserDate", (object)row["UserDate"].ToString().Trim());
                ht.Add((object)"@SchoolId", (object)row["SchoolId"].ToString().Trim());
                ht.Add((object)"@Achd", (object)obj.ExecuteScalarQry("select AcctsHeadId from PS_AdditionalFeeMaster where Ad_Id=3").ToString());
                if (obj.ExecuteScalar("ACTS_InsGenLedgerOldSchool", ht).Trim() == "")
                    ++num;
            }
            else if (row["Description"].ToString().Trim() == "Book Materials")
            {
                ht.Clear();
                ht.Add((object)"@PR_Id", (object)row["PR_Id"].ToString().Trim());
                ht.Add((object)"@SessionYr", (object)row["SessionYr"].ToString().Trim());
                ht.Add((object)"@TransDate", (object)row["TransDt"].ToString().Trim());
                ht.Add((object)"@ReceiptNo", (object)row["InvoiceReceiptNo"].ToString().Trim());
                ht.Add((object)"@Particulars", (object)row["Description"].ToString().Trim());
                ht.Add((object)"@Amount", (object)row["Amount"].ToString().Trim());
                ht.Add((object)"@UserId", (object)row["UserId"].ToString().Trim());
                ht.Add((object)"@UserDate", (object)row["UserDate"].ToString().Trim());
                ht.Add((object)"@SchoolId", (object)row["SchoolId"].ToString().Trim());
                ht.Add((object)"@Achd", (object)obj.ExecuteScalarQry("select AcctsHeadId from PS_AdditionalFeeMaster where Ad_Id=8").ToString());
                if (obj.ExecuteScalar("ACTS_InsGenLedgerOldSchool", ht).Trim() == "")
                    ++num;
            }
            else if (row["Description"].ToString().Trim() == "Anudan" || row["Description"].ToString().Trim().ToUpper() == "DONATION")
            {
                ht.Clear();
                ht.Add((object)"@PR_Id", (object)row["PR_Id"].ToString().Trim());
                ht.Add((object)"@SessionYr", (object)row["SessionYr"].ToString().Trim());
                ht.Add((object)"@TransDate", (object)row["TransDt"].ToString().Trim());
                ht.Add((object)"@ReceiptNo", (object)row["InvoiceReceiptNo"].ToString().Trim());
                ht.Add((object)"@Particulars", (object)row["Description"].ToString().Trim());
                ht.Add((object)"@Amount", (object)row["Amount"].ToString().Trim());
                ht.Add((object)"@UserId", (object)row["UserId"].ToString().Trim());
                ht.Add((object)"@UserDate", (object)row["UserDate"].ToString().Trim());
                ht.Add((object)"@SchoolId", (object)row["SchoolId"].ToString().Trim());
                ht.Add((object)"@Achd", (object)20);
                if (obj.ExecuteScalar("ACTS_InsGenLedgerOldSchool", ht).Trim() == "")
                    ++num;
            }
            else if (dataTableQry1.Rows.Count > 0 || dataTableQry2.Rows.Count > 0)
            {
                if (dataTableQry1.Rows.Count > 0)
                {
                    ht.Clear();
                    ht.Add((object)"@PR_Id", (object)row["PR_Id"].ToString().Trim());
                    ht.Add((object)"@SessionYr", (object)row["SessionYr"].ToString().Trim());
                    ht.Add((object)"@TransDate", (object)row["TransDt"].ToString().Trim());
                    ht.Add((object)"@ReceiptNo", (object)row["InvoiceReceiptNo"].ToString().Trim());
                    ht.Add((object)"@Particulars", (object)row["Description"].ToString().Trim());
                    ht.Add((object)"@Amount", (object)dataTableQry1.Rows[0]["Amnt"].ToString().Trim());
                    ht.Add((object)"@UserId", (object)row["UserId"].ToString().Trim());
                    ht.Add((object)"@UserDate", (object)row["UserDate"].ToString().Trim());
                    ht.Add((object)"@SchoolId", (object)row["SchoolId"].ToString().Trim());
                    ht.Add((object)"@Achd", (object)obj.ExecuteScalarQry("select AcctsHeadId from PS_AdditionalFeeMaster where Ad_Id=5").ToString());
                    if (obj.ExecuteScalar("ACTS_InsGenLedgerOldSchool", ht).Trim() == "")
                        ++num;
                }
                if (dataTableQry2.Rows.Count > 0 && Convert.ToDecimal(dataTableQry2.Rows[0]["Amnt"].ToString().Trim()) > new Decimal(0))
                {
                    ht.Clear();
                    ht.Add((object)"@PR_Id", (object)row["PR_Id"].ToString().Trim());
                    ht.Add((object)"@SessionYr", (object)row["SessionYr"].ToString().Trim());
                    ht.Add((object)"@TransDate", (object)row["TransDt"].ToString().Trim());
                    ht.Add((object)"@ReceiptNo", (object)row["InvoiceReceiptNo"].ToString().Trim());
                    ht.Add((object)"@Particulars", (object)"Bus Fee");
                    ht.Add((object)"@Amount", (object)dataTableQry2.Rows[0]["Amnt"].ToString().Trim());
                    ht.Add((object)"@UserId", (object)row["UserId"].ToString().Trim());
                    ht.Add((object)"@UserDate", (object)row["UserDate"].ToString().Trim());
                    ht.Add((object)"@SchoolId", (object)row["SchoolId"].ToString().Trim());
                    ht.Add((object)"@Achd", (object)obj.ExecuteScalarQry("select AcctsHeadId from PS_AdditionalFeeMaster where Ad_Id=1").ToString());
                    if (obj.ExecuteScalar("ACTS_InsGenLedgerOldSchool", ht).Trim() == "")
                        ++num;
                }
            }
        }
        lblMsg.Text = "No. Of Records Updated: " + (num * 2).ToString();
        lblMsg.ForeColor = Color.Green;
    }

    protected void btnGenFine_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        Hashtable ht = new Hashtable();
        int num = 0;
        foreach (DataRow row in (InternalDataCollectionBase)obj.GetDataTableQry("select distinct Receipt_VrNo from dbo.PS_FineLedger where Receipt_VrNo Like '%201415%' ").Rows)
        {
            ht.Clear();
            ht.Add((object)"@ReceiptNo", (object)row["Receipt_VrNo"].ToString().Trim());
            ht.Add((object)"@FeeId", (object)28);
            ht.Add((object)"@SchoolId", (object)1149);
            if (obj.ExecuteScalar("ps_sp_GenLedForFine", ht).Trim() == "")
                ++num;
        }
        lblMsg.Text = "No. Of Records Updated: " + (num * 2).ToString();
        lblMsg.ForeColor = Color.Green;
    }

    protected void btnFineUnpaid_Click(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        Hashtable ht = new Hashtable();
        int num = 0;
        DataTable dataTableQry = obj.GetDataTableQry("select * from dbo.PS_FineLedger where Receipt_VrNo='' and balance>0 ");
        foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
        {
            ht.Clear();
            ht.Add((object)"@TransDt", (object)row["TransDate"].ToString().Trim());
            ht.Add((object)"@AdmnNo", (object)row["AdmnNo"].ToString().Trim());
            ht.Add((object)"@Debit", (object)row["Debit"].ToString().Trim());
            ht.Add((object)"@Credit", (object)row["Credit"].ToString().Trim());
            ht.Add((object)"@Balance", (object)row["Balance"].ToString().Trim());
            ht.Add((object)"@UId", (object)row["UserID"].ToString().Trim());
            ht.Add((object)"@UDate", (object)row["UserDate"].ToString().Trim());
            ht.Add((object)"@SchoolId", (object)row["SchoolID"].ToString().Trim());
            ht.Add((object)"@Sy", (object)"2014-15");
            ht.Add((object)"@FeeId", (object)28);
            if (obj.ExecuteScalar("ps_sp_GenLedForFine", ht).Trim() == "")
                ++num;
        }
        if (num == dataTableQry.Rows.Count)
        {
            lblMsg.Text = "No. Of Records Updated: " + num.ToString();
            lblMsg.ForeColor = Color.Green;
        }
        else
        {
            lblMsg.Text = num.ToString() + " Records could not be Updated";
            lblMsg.ForeColor = Color.Red;
        }
    }
}