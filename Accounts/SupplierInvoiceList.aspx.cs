using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_SupplierInvoiceList : System.Web.UI.Page
{
    private Hashtable ht = new Hashtable();
    private Common obj = new Common();
    private DataTable dt = new DataTable();
    private string PesSecretary = ConfigurationManager.AppSettings["PESSecretaryLogin"].ToString();
    private string CpsSecretary = ConfigurationManager.AppSettings["CPSSecretaryLogin"].ToString();
    private string PesHeadCleark = ConfigurationManager.AppSettings["PESHeadClerkLogin"].ToString();
    private string CpsHeadCleark = ConfigurationManager.AppSettings["CPSHeadClerkLogin"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["CollegeId"] == null)
            Response.Redirect("../Login.aspx");
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        checkAccess();
        bindDropDown(drpCreditor, "SELECT DISTINCT S.SupName,I.SupId FROM dbo.PS_SupplierInvoice I INNER JOIN  dbo.PS_Supplier S ON I.SupId=S.SupId order by SupName", "SupName", "SupId");
        ClearFields();
        fillgrid();
        AccessFields();
    }

    private void checkAccess()
    {
        string str = "../Admin/Home.aspx";
        if (Request.QueryString["mgmt"] == null || Session["User"].ToString().Trim() == ConfigurationManager.AppSettings["PESSecretaryLogin"].ToString() || (Session["User"].ToString().Trim() == ConfigurationManager.AppSettings["CPSSecretaryLogin"].ToString() || Session["User"].ToString().Trim() == ConfigurationManager.AppSettings["PESHeadClerkLogin"].ToString()) || Session["User"].ToString().Trim() == ConfigurationManager.AppSettings["CPSHeadClerkLogin"].ToString())
            return;
        ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Access Denied !'); window.location='" + str + "';", true);
    }

    private void ClearFields()
    {
        drpCreditor.SelectedIndex = 0;
        dtpFromDate.SetDateValue(Convert.ToDateTime("01/01/" + DateTime.Now.Year.ToString()));
        dtpToDate.SetDateValue(DateTime.Now);
    }

    private void AccessFields()
    {
        if (Session["User"].ToString().Trim() == PesSecretary || Session["User"].ToString().Trim() == CpsSecretary || (Session["User"].ToString().Trim() == PesHeadCleark || Session["User"].ToString().Trim() == CpsHeadCleark))
        {
            btnDelete.Visible = false;
            btnNew.Visible = false;
        }
        else
        {
            btnDelete.Visible = true;
            btnNew.Visible = true;
        }
    }

    protected void btnshow_Click(object sender, EventArgs e)
    {
        fillgrid();
    }

    private void fillgrid()
    {
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        Common common = new Common();
        int pageIndex = grdSupplierMaster.PageIndex;
        if (dtpFromDate.GetDateValue() > dtpToDate.GetDateValue())
        {
            ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('From Date can not be greater than To Date');", true);
        }
        else
        {
            if (!drpCreditor.SelectedIndex.Equals(0))
                hashtable.Add("@SupId", drpCreditor.SelectedValue.Trim());
            hashtable.Add("@CollegeId", Session["CollegeId"].ToString());
            hashtable.Add("@FmDt", (dtpFromDate.GetDateValue().ToString("MM/dd/yyyy") + " 00:00:00"));
            hashtable.Add("@ToDt", (dtpToDate.GetDateValue().ToString("MM/dd/yyyy") + " 23:59:59"));
            DataTable dataTable2 = common.GetDataTable("ps_sp_get_Invoicelist", hashtable);
            grdSupplierMaster.DataSource = dataTable2;
            grdSupplierMaster.DataBind();
            grdSupplierMaster.PageIndex = pageIndex;
            if (dataTable2.Rows.Count > 0)
                lblRecCount.Text = "Total Record: " + dataTable2.Rows.Count.ToString();
            else
                lblRecCount.Text = "";
        }
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("SupplierInvoice.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                DeleteRecord(obj.ToString());
            fillgrid();
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Record Deleted');", true);
        }
    }

    private void DeleteRecord(string id)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable = new DataTable();
        Common common = new Common();
        hashtable.Add("InvId", id);
        hashtable.Add("CollegeId", Session["CollegeId"].ToString());
        common.GetDataTable("ps_sp_DelInvPaid", hashtable);
    }

    protected void grdSupplierMaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdSupplierMaster.PageIndex = e.NewPageIndex;
        fillgrid();
    }

    protected void btnStatus_Click(object sender, EventArgs e)
    {
        GridViewRow parent = (GridViewRow)((Control)sender).Parent.Parent;
        Button control1 = (Button)parent.FindControl("btnStatus");
        HiddenField control2 = (HiddenField)parent.FindControl("hfId");
        obj = new Common();
        ht = new Hashtable();
        dt = new DataTable();
        ht.Add("@InvoiceId", Convert.ToInt64(control2.Value));
        if (Session["User"].ToString().Trim() == PesSecretary || Session["User"].ToString().Trim() == CpsSecretary)
        {
            ht.Add("@User", "S");
            ht.Add("@SecyUserId", Session["User_Id"].ToString().Trim());
        }
        if (Session["User"].ToString().Trim() == PesHeadCleark || Session["User"].ToString().Trim() == CpsHeadCleark)
        {
            ht.Add("@User", "H");
            ht.Add("@HCUserId", Session["User_Id"].ToString().Trim());
        }
        ht.Add("@CollegeId", Session["CollegeId"].ToString().Trim());
        dt = obj.GetDataTable("ps_sp_UpdtSupplierInvoice", ht);
        if (dt.Rows.Count > 0)
        {
            lblMsg.Text = "Approved failed for this transaction !";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Approved Successfully !";
            lblMsg.ForeColor = Color.Green;
        }
        fillgrid();
    }

    protected void grdSupplierMaster_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string str1 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Secy"));
            string str2 = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "HC"));
            if (Session["User"].ToString().Trim() == PesSecretary || Session["User"].ToString().Trim() == CpsSecretary || (Session["User"].ToString().Trim() == PesHeadCleark || Session["User"].ToString().Trim() == CpsHeadCleark))
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;
                e.Row.Cells[10].Visible = true;
                e.Row.Cells[11].Visible = true;
            }
            if (!(Session["User"].ToString().Trim() == PesSecretary) && !(Session["User"].ToString().Trim() == CpsSecretary) && (!(Session["User"].ToString().Trim() == PesHeadCleark) && !(Session["User"].ToString().Trim() == CpsHeadCleark)))
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
                e.Row.Cells[10].Visible = false;
                e.Row.Cells[11].Visible = false;
            }
            if (Session["User"].ToString().Trim() == PesSecretary || Session["User"].ToString().Trim() == CpsSecretary)
            {
                Button control1 = (Button)e.Row.FindControl("btnSecr");
                if (str1 == "")
                {
                    control1.Text = "Approve";
                    control1.Enabled = true;
                }
                else
                {
                    control1.Text = "Approved";
                    control1.Enabled = false;
                }
                Button control2 = (Button)e.Row.FindControl("btnHeadClerk");
                if (str2 == "")
                {
                    control2.Text = "Approve";
                    control2.Enabled = false;
                }
                else
                {
                    control2.Text = "Approved";
                    control2.Enabled = false;
                }
            }
            if (Session["User"].ToString().Trim() == PesHeadCleark || Session["User"].ToString().Trim() == CpsHeadCleark)
            {
                Button control1 = (Button)e.Row.FindControl("btnSecr");
                if (str1 == "")
                {
                    control1.Text = "Approve";
                    control1.Enabled = false;
                }
                else
                {
                    control1.Text = "Approved";
                    control1.Enabled = false;
                }
                Button control2 = (Button)e.Row.FindControl("btnHeadClerk");
                if (str2 == "")
                {
                    control2.Text = "Approve";
                    control2.Enabled = true;
                }
                else
                {
                    control2.Text = "Approved";
                    control2.Enabled = false;
                }
            }
            if (!(str1 != "") || !(str2 != ""))
                e.Row.ForeColor = Color.Maroon;
            HyperLink control3 = (HyperLink)e.Row.FindControl("hlEdit");
            if (str1 != "" && str2 != "")
            {
                e.Row.ForeColor = ColorTranslator.FromHtml("#006600");
                HtmlControl control1 = (HtmlControl)e.Row.FindControl("divReal");
                HtmlControl control2 = (HtmlControl)e.Row.FindControl("divDummy");
                control3.Enabled = false;
                control1.Visible = false;
                control2.Visible = true;
            }
            else
                control3.NavigateUrl = "SupplierInvoice.aspx?inv=" + DataBinder.Eval(e.Row.DataItem, "InvoiceId").ToString().Trim();
        }
        if (e.Row.RowType != DataControlRowType.Header)
            return;
        if (Session["User"].ToString().Trim() == PesSecretary || Session["User"].ToString().Trim() == CpsSecretary || (Session["User"].ToString().Trim() == PesHeadCleark || Session["User"].ToString().Trim() == CpsHeadCleark))
        {
            e.Row.Cells[0].Visible = false;
            e.Row.Cells[1].Visible = false;
            e.Row.Cells[10].Visible = true;
            e.Row.Cells[11].Visible = true;
        }
        else
        {
            e.Row.Cells[0].Visible = true;
            e.Row.Cells[1].Visible = true;
            e.Row.Cells[10].Visible = false;
            e.Row.Cells[11].Visible = false;
        }
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new Common();
        DataTable dataTable = obj.ExecuteSql(query);
        drp.DataSource = dataTable;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--All--", "0"));
    }

    protected void grdSupplierMaster_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.Header || !(Session["User"].ToString().Trim() == PesSecretary) && !(Session["User"].ToString().Trim() == CpsSecretary) && (!(Session["User"].ToString().Trim() == PesHeadCleark) && !(Session["User"].ToString().Trim() == CpsHeadCleark)))
            return;
        GridView gridView = (GridView)sender;
        GridViewRow gridViewRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
        gridViewRow.Cells.Add(new TableCell() { ColumnSpan = 8 });
        gridViewRow.Cells.Add(new TableCell()
        {
            Text = "Approved By",
            HorizontalAlign = HorizontalAlign.Center,
            ColumnSpan = 2
        });
        gridView.Controls[0].Controls.AddAt(0, (Control)gridViewRow);
    }

    protected void btnHeadClerk_Click(object sender, EventArgs e)
    {
        GridViewRow parent = (GridViewRow)((Control)sender).Parent.Parent;
        Button control1 = (Button)parent.FindControl("btnStatus");
        HiddenField control2 = (HiddenField)parent.FindControl("hfId");
        obj = new Common();
        ht = new Hashtable();
        dt = new DataTable();
        ht.Add("@InvoiceId", Convert.ToInt64(control2.Value));
        if (Session["User"].ToString().Trim() == PesSecretary || Session["User"].ToString().Trim() == CpsSecretary)
        {
            ht.Add("@User", "S");
            ht.Add("@SecyUserId", Session["User_Id"].ToString().Trim());
        }
        if (Session["User"].ToString().Trim() == PesHeadCleark || Session["User"].ToString().Trim() == CpsHeadCleark)
        {
            ht.Add("@User", "H");
            ht.Add("@HCUserId", Session["User_Id"].ToString().Trim());
        }
        ht.Add("@CollegeId", Session["CollegeId"].ToString().Trim());
        dt = obj.GetDataTable("ps_sp_UpdtSupplierInvoice", ht);
        if (dt.Rows.Count > 0)
        {
            lblMsg.Text = "Approved failed for this transaction !";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            lblMsg.Text = "Approved Successfully !";
            lblMsg.ForeColor = Color.Green;
        }
        fillgrid();
    }
}