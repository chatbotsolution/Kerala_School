using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_rptBankReconciliation : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        fillBanks();
    }

    private void fillBanks()
    {
        DataTable dataTable = new DataTable();
        drpBank.DataSource = obj.GetDataTableQry("SELECT AcctsHeadId,AcctsHead FROM dbo.ACTS_AccountHeads a inner join dbo.Acts_AccountGroups ag on ag.AG_Code=a.AG_Code where (a.AG_Code=7 or ag.AG_Parent=7) ORDER BY AcctsHead");
        drpBank.DataTextField = "AcctsHead";
        drpBank.DataValueField = "AcctsHeadId";
        drpBank.DataBind();
        drpBank.Items.Insert(0, new ListItem("--All--", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GetPendingChks();
    }

    private void GetPendingChks()
    {
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        StringBuilder stringBuilder = new StringBuilder();
        Decimal num = new Decimal(0);
        if (drpBank.SelectedIndex > 0)
            hashtable.Add("@BankId", drpBank.SelectedValue);
        DataTable dataTable2 = obj.GetDataTable("ACTS_GetBankReconciliation", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            stringBuilder.Append("<table border='1px' style='border-collapse:collapse;' width='100%'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='height:20px;' colspan='8' align='center'><b>Bank Reconciliation Report As On " + DateTime.Now.ToString("dd MMM yyyy") + " </b></td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width:24%;'><b>Bank Account</b></td>");
            stringBuilder.Append("<td style='width:8%;'><b>Transaction Type</b></td>");
            stringBuilder.Append("<td style='width:15%;'><b>Party</b></td>");
            stringBuilder.Append("<td style='width:25%;'><b>Transaction Dtls</b></td>");
            stringBuilder.Append("<td style='width:8%;'><b>Payble Bank</b></td>");
            stringBuilder.Append("<td style='width:10%;'><b>Instrument No</b></td>");
            stringBuilder.Append("<td style='width:25%;'><b>Transaction Date</b></td>");
            stringBuilder.Append("<td align='right' style='width:25%'><b>Amount</b></td>");
            stringBuilder.Append("</tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td>" + row["BankName"] + "</td>");
                stringBuilder.Append("<td>" + row["TransType"] + "</td>");
                if (row["AcctsHead"].ToString().Trim() != "")
                    stringBuilder.Append("<td>" + row["AcctsHead"] + "</td>");
                else
                    stringBuilder.Append("<td>" + row["RcvdFrom"] + "</td>");
                stringBuilder.Append("<td>" + row["Description"] + "</td>");
                stringBuilder.Append("<td>" + row["DrawanOnBank"] + "</td>");
                stringBuilder.Append("<td>" + row["InstrumentNo"] + "</td>");
                stringBuilder.Append("<td>" + row["TransDtStr"] + "</td>");
                stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["Amount"].ToString().Trim()).ToString("0.00") + "</td>");
                num += Convert.ToDecimal(row["Amount"].ToString().Trim());
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='height:20px;' colspan='7' align='right'><b>Total : </b></td>");
            stringBuilder.Append("<td align='right'><b>" + num.ToString("0.00") + "</b></td>");
            stringBuilder.Append("</table>");
            lblReport.ForeColor = Color.Black;
            lblReport.Text = stringBuilder.ToString();
            btnprnt.Enabled = true;
        }
        else
        {
            lblReport.Text = "No Records Found";
            lblReport.ForeColor = Color.Red;
            btnprnt.Enabled = true;
        }
    }
}