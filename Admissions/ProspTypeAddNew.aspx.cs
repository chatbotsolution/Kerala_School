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

public partial class Admissions_ProspTypeAddNew : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private string msg = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        bindClass();
    }

    private void bindClass()
    {
        DataTable dataTable = new DataTable();
        chklClass.DataSource = obj.GetDataTableQry("select ClassID, ClassName from dbo.PS_ClassMaster").DefaultView;
        chklClass.DataTextField = "ClassName";
        chklClass.DataValueField = "ClassID";
        chklClass.DataBind();
        chklClass.Items.Insert(0, new ListItem("ALL", "0"));
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        InsertProsType();
    }

    private void InsertProsType()
    {
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        string str = string.Empty;
        hashtable.Add("@ProspType", txtProsType.Text.ToString().Trim());
        if (chklClass.SelectedIndex == 0)
        {
            for (int index = 0; index <= chklClass.Items.Count - 1; ++index)
            {
                if (chklClass.Items[index].Value != "0")
                    str = !(str.ToString().Trim() != string.Empty) ? Convert.ToString(chklClass.Items[index].Text) : str + "," + Convert.ToString(chklClass.Items[index].Text);
            }
        }
        else
        {
            for (int index = 0; index <= chklClass.Items.Count - 1; ++index)
            {
                if (chklClass.Items[index].Selected)
                    str = !(str.ToString().Trim() != string.Empty) ? Convert.ToString(chklClass.Items[index].Text) : str + "," + Convert.ToString(chklClass.Items[index].Text);
            }
        }
        hashtable.Add("@ForClass", str);
        hashtable.Add("@UserId", Session["User_Id"]);
        msg = obj.ExecuteScalar("InsertProsType", hashtable);
        if (msg.ToString().Trim() == "D")
        {
            lblerr.Text = "Prospectus Type Already Exists";
            lblerr.ForeColor = Color.Red;
        }
        else
        {
            lblerr.Text = "Prospectus Type Saved Sucessfully";
            lblerr.ForeColor = Color.Green;
            Clear();
            Response.Redirect("~/Admissions/ProspStock.aspx?Id=" + int.Parse(msg.ToString()));
        }
    }

    private void Clear()
    {
        txtProsType.Text = string.Empty;
        chklClass.ClearSelection();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void btnback_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Admissions/ProspStock.aspx");
    }
}