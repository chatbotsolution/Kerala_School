using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admissions_ProspectusSaleList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private clsStaticDropdowns DRP = new clsStaticDropdowns();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblmsg1.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        FillSession();
        BindDropDown();
        btnDelete.Enabled = false;
        if (!(Session["userrights"].ToString().Trim() == "a"))
            return;
        btnDelete.Visible = true;
    }

    private void FillSession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select distinct SessionID,SessionYr from dbo.PS_FeeNormsNew order by sessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void Fillgrid()
    {
        ViewState["dt"] = null;
        dt = new DataTable();
        ht = new Hashtable();
        obj = new clsDAL();
        ht.Add("@SessionYr", drpSession.SelectedValue);
        if (txtFromDt.Text != "")
            ht.Add("@FromDate", dtpPros.GetDateValue());
        if (txtToDt.Text != "")
            ht.Add("@Todate", dtpPros1.GetDateValue());
        if (drpForCls.SelectedIndex > 0)
            ht.Add("@ForClass", drpForCls.SelectedValue);
        int pageIndex = grdProspect.PageIndex;
        dt = obj.GetDataTable("Sp_GetProspectusStockSale", ht);
        grdProspect.PageIndex = pageIndex;
        if (dt.Rows.Count > 0)
        {
            ViewState["dt"] = dt;
            grdProspect.DataSource = dt;
            grdProspect.DataBind();
            lblRecCount.Text = "Total Record: " + dt.Rows.Count.ToString();
            btnDelete.Enabled = true;
        }
        else
        {
            grdProspect.DataSource = null;
            grdProspect.DataBind();
            lblRecCount.Text = string.Empty;
            btnDelete.Enabled = false;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            lblmsg1.Text = "Select a checkbox !";
            lblmsg1.ForeColor = Color.Red;
        }
        else if (getValidInput())
        {
            string str1 = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str1.Split(chArray))
                hfRcptId.Value = obj.ToString();
            Hashtable hashtable = new Hashtable();
            clsDAL clsDal = new clsDAL();
            string str2 = clsDal.ExecuteScalarQry("select MR_No from dbo.PS_ProspectusSale where SaleId=" + hfRcptId.Value);
            hashtable.Add("@RcptNo", str2);
            string str3 = clsDal.ExecuteScalar("ps_sp_FeeCancelBankChk", hashtable);
            if (str3.Trim() == "")
            {
                DataRow[] dataRowArray = ((DataTable)ViewState["dt"]).Select("SaleId=" + hfRcptId.Value);
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<div><b>Records to be deleted :-</b></div>");
                stringBuilder.Append("<table border='0px' cellpadding='3' cellspacing='3' style='border:solid black 1px;' class='tbltxt'  width='100%'><tr>");
                stringBuilder.Append("");
                stringBuilder.Append("<td width='100'>Prospectus Sl. No </td><td width='5'>: </td><td width='150'>" + dataRowArray[0]["ProspectusSlNo"].ToString() + "</td>");
                stringBuilder.Append("<td>Sale date </td><td>: </td><td>" + dataRowArray[0]["SaleDateStr"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td>Student Name </td><td>: </td><td>" + dataRowArray[0]["StudentName"].ToString() + "</td>");
                stringBuilder.Append("<td>Amount </td><td>: </td><td>" + dataRowArray[0]["Amount"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
                stringBuilder.Append("</table>");
                litDetails.Text = stringBuilder.ToString().Trim();
                pnlDelReason.Visible = true;
                pnlRcptList.Visible = false;
                btnDelete.Enabled = false;
                lblRecCount.Text = string.Empty;
            }
            else if (str3.Trim() == "DATE")
            {
                lblmsg1.Text = "Cannot Delete Receipt!! Fee Already Received For Later Date!!";
                lblmsg1.ForeColor = Color.Red;
            }
            else if (str3.Trim() == "NO")
            {
                lblmsg1.Text = "Fee Receipt For This Student Exists After This Transaction!!!Please Delete Previous Receipt First!!";
                lblmsg1.ForeColor = Color.Red;
            }
            else
            {
                lblmsg1.Text = "Cannot Delete Received Fee!!! Fee Amount Already Encashed !!!";
                lblmsg1.ForeColor = Color.Red;
            }
        }
        else
        {
            lblmsg1.Text = "Select only one checkbox !";
            lblmsg1.ForeColor = Color.Red;
        }
    }

    protected void grdProspect_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdProspect.PageIndex = e.NewPageIndex;
        Fillgrid();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Fillgrid();
        pnlDelReason.Visible = false;
        pnlRcptList.Visible = true;
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("ProspectousSale.aspx");
    }

    private void BindDropDown()
    {
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        drpForCls.DataSource = obj.GetDataTable("Ps_Sp_BindClassDrp");
        drpForCls.DataTextField = "ClassName";
        drpForCls.DataValueField = "ClassID";
        drpForCls.DataBind();
        drpForCls.Items.Insert(0, new ListItem("All", ""));
    }

    protected void btnSaveReason_Click(object sender, EventArgs e)
    {
        Delete(hfRcptId.Value.ToString());
        Fillgrid();
        lblmsg1.Text = "Receipt cancelled successfully !";
        lblmsg1.ForeColor = Color.Green;
        pnlDelReason.Visible = false;
        txtDelReason.Text = string.Empty;
        pnlRcptList.Visible = true;
    }

    private void Delete(string id)
    {
        ht = new Hashtable();
        dt = new DataTable();
        obj = new clsDAL();
        ht.Add("@SaleId", id);
        ht.Add("@CancelledReason", txtDelReason.Text.Trim().ToString());
        dt = obj.GetDataTable("Ps_DelProspectusSale", ht);
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        txtDelReason.Text = string.Empty;
        pnlRcptList.Visible = true;
        pnlDelReason.Visible = false;
    }

    private bool getValidInput()
    {
        int num = 0;
        string[] strArray = Request["Checkb"].Split(',');
        for (int index = 0; index < strArray.Length; ++index)
            ++num;
        return num <= 1;
    }
}