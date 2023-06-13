using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Masters_BusRouteMaster : System.Web.UI.Page
{
    private Hashtable ht = new Hashtable();
    private clsDAL ObjCommon = new clsDAL();
    private DataTable dt = new DataTable();
    private clsStaticDropdowns objStaticDrop = new clsStaticDropdowns();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
            return;
        if (Session["User"] == null)
            Response.Redirect("../Login.aspx");
        txtRouteNm.Focus();
        if (Request.QueryString["rid"] == null)
            return;
        FillRecordForUpdate();
    }

    protected void FillRecordForUpdate()
    {
        try
        {
            Convert.ToInt32(Request.QueryString["rid"]);
            clsDAL clsDal = new clsDAL();
            Hashtable hashtable = new Hashtable();
            DataTable dataTableQry = clsDal.GetDataTableQry("select * from PS_BusRouteMaster where RouteId=" + Request.QueryString["rid"].ToString().Trim());
            txtRouteNm.Text = dataTableQry.Rows[0]["RouteName"].ToString();
            txtDistance.Text = dataTableQry.Rows[0]["Distance"].ToString();
            txtFee.Text = dataTableQry.Rows[0]["Fee"].ToString();
            txtRemarks.Text = dataTableQry.Rows[0]["Remarks"].ToString();
            if (dataTableQry.Rows[0]["ActiveStatus"].ToString() == "True")
                chkActive.Checked = true;
            else
                chkActive.Checked = false;
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string currSessionYr = objStaticDrop.GetCurrSessionYr();
            if (Request.QueryString["rid"] != null)
                ht.Add("@RouteId", Request.QueryString["rid"].ToString().Trim());
            ht.Add("@RouteName", txtRouteNm.Text);
            ht.Add("@Distance", float.Parse(txtDistance.Text));
            ht.Add("@SessYr", currSessionYr);
            ht.Add("@Fee", txtFee.Text);
            ht.Add("@Remarks", txtRemarks.Text);
            if (chkActive.Checked)
                ht.Add("@ActiveStatus", 1);
            else
                ht.Add("@ActiveStatus", 0);
            ht.Add("@UserID", Convert.ToInt32(Session["User_Id"].ToString()));
            ht.Add("@UserDate", DateTime.Now.ToString("dd-MMM-yyyy"));
            if (ObjCommon.ExecuteScalar("[ps_instBusRoute]", ht) != "")
            {
                lblMsg.Text = " NOT SUCCESS";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                clearAll();
                lblMsg.Text = "SUCCESS";
                lblMsg.ForeColor = Color.Green;
                txtRouteNm.Focus();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = ex.Message;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
    }

    private void clearAll()
    {
        txtRouteNm.Text = "";
        txtDistance.Text = "";
        txtFee.Text = "";
        txtRemarks.Text = "";
        chkActive.Checked = true;
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("BusRouteMasterList.aspx");
    }
}