using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_AccountHead : System.Web.UI.Page
{
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private Common obj = new Common();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        drpAccType.SelectedIndex = 1;
        if (Request.QueryString["em"] != null)
        {
            DispAccHead();
            pnl1.Visible = true;
            Pnl2.Visible = true;
        }
        else if (Request.QueryString["new"] != null)
        {
            txtAccHead.Text = "";
            lblMsgSucc.Text = "";
            trMsg.Style["Background-Color"] = "Transaparent";
            pnl1.Visible = true;
            Pnl2.Visible = false;
        }
        else
        {
            txtAccHead.Text = "";
            lblMsgSucc.Text = "";
            trMsg.Style["Background-Color"] = "Transaparent";
            pnl1.Visible = false;
            Pnl2.Visible = true;
        }
        disp();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        InsAccHead();
    }

    private void InsAccHead()
    {
        obj = new Common();
        ht = new Hashtable();
        if (Request.QueryString["em"] != null)
            ht.Add("AccountHeadId", Request.QueryString["em"]);
        ht.Add("AccountHeadDetail", txtAccHead.Text.Trim());
        ht.Add("AccountTypeCrDr", drpAccType.SelectedValue.Trim());
        ht.Add("UserId", Session["user_id"]);
        ht.Add("CollegeId", Session["CollegeId"]);
        string str = obj.ExecSqlWithHT("PS_InsAccountHeadMaster", ht);
        if (str.Trim().Equals(string.Empty))
        {
            if (Request.QueryString["em"] != null)
            {
                Response.Redirect("AccountHead.aspx");
            }
            else
            {
                trMsg.Style["Background-Color"] = "Green";
                lblMsgSucc.Text = "Record Saved";
            }
        }
        else
        {
            trMsg.Style["Background-Color"] = "Red";
            lblMsgSucc.Text = str;
        }
        disp();
        drpAccType.SelectedValue = "0";
        txtAccHead.Text = "";
    }

    private void DispAccHead()
    {
        obj = new Common();
        ht = new Hashtable();
        dt = new DataTable();
        ht.Add("AccountHeadId", Request.QueryString["em"].ToString());
        if (drpAccountType.SelectedIndex > 0)
            ht.Add("@AccountTypeCrDr", drpAccountType.SelectedValue.ToString().Trim());
        ht.Add("@CollegeId", Session["CollegeId"]);
        dt = obj.GetDataTable("PS_DispAccHead", ht);
        txtAccHead.Text = dt.Rows[0]["AccountHeadDetail"].ToString().Trim();
        drpAccType.SelectedValue = dt.Rows[0]["AccountTypeCrDr"].ToString().Trim();
        drpAccountType.SelectedValue = dt.Rows[0]["AccountTypeCrDr"].ToString().Trim();
        btnSubmit.Text = "Update";
    }

    protected void disp()
    {
        obj = new Common();
        dt = new DataTable();
        ht = new Hashtable();
        if (drpAccountType.SelectedIndex > 0)
            ht.Add("@AccountTypeCrDr", drpAccountType.SelectedValue.ToString().Trim());
        ht.Add("@CollegeId", Session["CollegeId"]);
        dt = obj.GetDataTable("PS_DispAccHead", ht);
        gidAccHead.DataSource = dt.DefaultView;
        gidAccHead.DataBind();
        lblRecCount.Text = "No Of Record(s) : " + dt.Rows.Count.ToString();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                DeleteAccHead(obj.ToString());
        }
        disp();
    }

    private void DeleteAccHead(string id)
    {
        ht = new Hashtable();
        dt = new DataTable();
        obj = new Common();
        ht.Add("AccountHeadId", id);
        dt = obj.GetDataTable("PS_DeleteActHead", ht);
    }

    protected void gidAccHead_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gidAccHead.PageIndex = e.NewPageIndex;
        disp();
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Response.Redirect("AccountHead.aspx?new=1");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtAccHead.Text = "";
        drpAccType.SelectedValue = "0";
        trMsg.Style["Background-Color"] = "Transparent";
        lblMsgSucc.Text = "";
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Response.Redirect("AccountHead.aspx");
    }

    protected void drpAccountType_SelectedIndexChanged(object sender, EventArgs e)
    {
        disp();
    }
}