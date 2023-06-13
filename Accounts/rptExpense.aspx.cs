using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Accounts_rptExpense : System.Web.UI.Page
{
    private NumberFormatInfo format = new NumberFormatInfo();
    private int[] indSizes = new int[4] { 3, 2, 2, 2 };
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        drpAccHead.Focus();
        fillacchead();
        txtstartdate.Text = DateTime.Now.Month <= 3 ? new DateTime(DateTime.Now.Year - 1, 4, 1).ToString("dd-MM-yyyy") : new DateTime(DateTime.Now.Year, 4, 1).ToString("dd-MM-yyyy");
        txtenddate.Text = DateTime.Now.ToString("dd-MM-yyyy");
    }

    private void fillacchead()
    {
        drpAccHead.Items.Clear();
        DataTable dataTable = new clsDAL().GetDataTable("ACTS_GetAccountHdsExp");
        if (dataTable.Rows.Count > 0)
        {
            drpAccHead.DataSource = dataTable;
            drpAccHead.DataTextField = "AcctsHead";
            drpAccHead.DataValueField = "AcctsHeadId";
            drpAccHead.DataBind();
            drpAccHead.Items.Insert(0, new ListItem("--All--", "0"));
        }
        else
            drpAccHead.Items.Insert(0, new ListItem("N0 DATA FOUND", "0"));
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        StringBuilder stringBuilder = new StringBuilder();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        obj = new clsDAL();
        Decimal num1 = new Decimal(0);
        int num2 = 1;
        hashtable.Add("@FromDt", dtpStart.GetDateValue().ToString("dd MMM yyyy"));
        hashtable.Add("@ToDt", dtpEnd.GetDateValue().ToString("dd MMM yyyy"));
        if (drpAccHead.SelectedIndex > 0)
            hashtable.Add("@ExpAcHeadId", drpAccHead.SelectedValue);
        DataTable dataTable2 = obj.GetDataTable("ACTS_ExpenseRpt", hashtable);
        stringBuilder.Append("<table width='100%' class='innertbltxt'><tr><td colspan='5'></td></tr>");
        stringBuilder.Append("<tr><td colspan='5' height='2px'></td></tr>");
        stringBuilder.Append("<tr><td colspan='5' align='center' class='pageSectionLabel' style= 'border-bottom:dotted 1px black;'><b>Expense for " + drpAccHead.SelectedItem.Text.ToString().Trim() + "  From:&nbsp;  " + txtstartdate.Text + " &nbsp;&nbsp;&nbsp;To:&nbsp;  " + txtenddate.Text + "</b></td></tr>");
        stringBuilder.Append("<tr><td colspan='5' height='2px'>&nbsp;</td></tr>");
        stringBuilder.Append("<tr><td colspan='5' height='2px'></td></tr>");
        stringBuilder.Append("<td width='5%' align='left'><b>Sno.</b></td>");
        stringBuilder.Append("<td width='15%' align='left'><b>Date</b></td>");
        if (drpAccHead.SelectedIndex == 0)
            stringBuilder.Append("<td width='35%' align='left'><b>Exp Account Head</b></td>");
        stringBuilder.Append("<td width='30%' align='left'><b>Particulars</b></td>");
        stringBuilder.Append("<td width='15%' align='right'><b>Total Amount</b></td>");
        foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td width='5%' align='left'>" + num2 + "</td>");
            stringBuilder.Append("<td width='15%' align='left'>" + row["ExpDateStr"] + "</td>");
            if (drpAccHead.SelectedIndex == 0)
                stringBuilder.Append("<td width='35%' align='left'>" + row["AcctsHead"] + "</td>");
            stringBuilder.Append("<td width='30%' align='left'>" + row["ExpDetails"] + "</td>");
            stringBuilder.Append("<td width='15%' align='right'>" + row["Amount"] + "</td>");
            stringBuilder.Append("</tr>");
            num1 += Convert.ToDecimal(row["Amount"]);
            ++num2;
        }
        stringBuilder.Append("<tr><td colspan='5' height='2px' align='right'><b>Total Expenses: " + num1.ToString("0.00") + "</b></td>");
        stringBuilder.Append("</table>");
        Literal1.Text = stringBuilder.ToString();
    }
}