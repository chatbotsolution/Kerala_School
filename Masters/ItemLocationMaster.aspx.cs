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

public partial class Masters_ItemLocationMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User"] == null)
            Response.Redirect("~/Login.aspx");
        if (Page.IsPostBack)
            return;
        Page.Form.DefaultButton = btnSave.UniqueID;
        BindLocationIncharge();
        if (Request.QueryString["Id"] == null)
            return;
        Fillfilds(sender, e);
    }

    private void BindLocationIncharge()
    {
        DataTable dataTable = new clsDAL().GetDataTable("SI_GetLocIncharge");
        drpLocInCharge.Items.Clear();
        drpLocInCharge.Items.Add(new ListItem("-Select-", "0"));
        if (dataTable.Rows.Count <= 0)
            return;
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            drpLocInCharge.Items.Add(new ListItem(row["LocIncharge"].ToString(), row["USER_ID"].ToString()));
    }

    private void Fillfilds(object sender, EventArgs e)
    {
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = obj.GetDataTableQry("SELECT LocationId, Location, LocationInvIc, ContactTelNo, UserId, UserDate FROM dbo.SI_LocationMaster WHERE LocationId=" + Request.QueryString["Id"].ToString() + " ");
        txtLocName.Text = dataTableQry.Rows[0]["Location"].ToString();
        drpLocInCharge.SelectedValue = dataTableQry.Rows[0]["UserId"].ToString();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Id"] != null)
            ht.Add((object)"@LocationId", (object)int.Parse(Request.QueryString["Id"].ToString()));
        if (txtLocName.Text != "")
            ht.Add((object)"@Location", (object)txtLocName.Text.Trim());
        DataTable dataTableQry = obj.GetDataTableQry("Select FullName,ContactNo from PS_USER_MASTER where USER_ID=" + drpLocInCharge.SelectedValue.ToString().Trim());
        if (drpLocInCharge.SelectedIndex > 0)
        {
            ht.Add((object)"@LocationInvIc", (object)dataTableQry.Rows[0]["FullName"].ToString());
            ht.Add((object)"@ContactTelNo", (object)dataTableQry.Rows[0]["ContactNo"].ToString());
        }
        ht.Add((object)"@UserID", (object)drpLocInCharge.SelectedValue.ToString().Trim());
        ht.Add((object)"@SchoolId", (object)Convert.ToInt32(Session["SchoolId"]));
        DataTable dataTable = new DataTable();
        obj = new clsDAL();
        if (obj.GetDataTable("SI_InsUpdtItemLocation", ht).Rows.Count > 0)
        {
            lblMsg.Text = "Data Already Exits  !";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Data Inserted Successfully !";
            lblMsg.ForeColor = Color.Green;
            Clear();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        txtLocName.Text = string.Empty;
        drpLocInCharge.SelectedIndex = 0;
    }

    protected void btngoto_Click(object sender, EventArgs e)
    {
        Response.Redirect("ItemLocationList.aspx");
    }
}