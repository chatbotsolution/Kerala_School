using ASP;
using Classes.DA;
using System;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
public partial class Reports_rptFeeReceiptChequePrint : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack || Session["printchequedata"] == null)
            return;
        literaldata.Text = Session["printchequedata"].ToString().Trim();
        DataTable dataTable = new Common().ExecuteSql("select ChequeDate,ChequeNo,DrawanOnBank from PS_ChequeDetails where AdmnNo=" + (object)Convert.ToInt32(Request.QueryString["admnno"].ToString()));
        lblchequeno.Text = dataTable.Rows[0]["ChequeNo"].ToString();
        lblchequedate.Text = Convert.ToDateTime(dataTable.Rows[0]["ChequeDate"].ToString()).ToString("dd/MM/yyyy");
        lblbank.Text = dataTable.Rows[0]["DrawanOnBank"].ToString();
    }
}