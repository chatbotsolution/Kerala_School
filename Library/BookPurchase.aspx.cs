using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Library_BookPurchase : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            pnl1.Visible = false;
            if (Session["User_Id"] != null)
            {
                if (Page.IsPostBack)
                    return;
                Session["title"] = Page.Title.ToString();
                Form.DefaultButton = btnSaveAddNew.UniqueID;
                bindDropDown(ddlBookCat, "select  BriefName as CatName,CatCode from Lib_Category where CollegeId=" + Session["SchoolId"] + " order by CatName", "CatName", "CatCode");
                FillNewAccNo();
                FillPostBack(sender, e);
                if (Request.QueryString["PurchaseId"] == null)
                    return;
                FillData(int.Parse(Request.QueryString["PurchaseId"]));
            }
            else
                Response.Redirect("../Login.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    private void FillNewAccNo()
    {
        obj = new clsDAL();
        string empty = string.Empty;
        ht = new Hashtable();
        ht.Add("@CollegeId", Session["SchoolId"]);
        txtNewAccNo.Text = obj.ExecuteScalar("Lib_SP_GetMaxAccNo", ht);
    }

    private void FillPostBack(object sender, EventArgs e)
    {
        if (Session["PurData"] == null)
            return;
        dt = Session["PurData"] as DataTable;
        ddlBookCat.SelectedValue = dt.Rows[0]["CatCode"].ToString();
        ddlBookCat_SelectedIndexChanged(sender, e);
        ddlSubject.SelectedValue = dt.Rows[0]["Subject"].ToString();
        ddlSubject_SelectedIndexChanged(sender, e);
        Session["PurData"] = null;
    }

    private void FillData(int purchaseId)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Select * from Lib_VW_PurchaseList where PurchaseId=" + purchaseId + " and CollegeId=" + int.Parse(Session["SchoolId"].ToString()));
        dt.Clear();
        dt = obj.GetDataTableQry(stringBuilder.ToString());
        if (dt.Rows.Count > 0)
        {
            fillField(dt);
            btnSaveAddNew.Text = "Update & AddNew";
            btnSaveGotoList.Text = "Update & GotoList";
        }
        else
            Response.Redirect("BookPurchaseList.aspx");
    }

    private void fillField(DataTable dt)
    {
        ddlBookCat.SelectedValue = dt.Rows[0]["CatCode"].ToString();
        bindDropDown(ddlSubject, "select  SubName ,SubjectId from Lib_Subjects where CollegeId=" + Session["SchoolId"] + " AND CatCode=" + ddlBookCat.SelectedValue + " order by SubName", "SubName", "SubjectId");
        ddlSubject.SelectedValue = dt.Rows[0]["SubjectId"].ToString();
        bindDropDown(ddlBookTitle, "select BookId, BookTitle from dbo.Lib_BookMaster where CollegeId=" + Session["SchoolId"] + " AND SubjectId=" + ddlSubject.SelectedValue + " order by BookTitle", "BookTitle", "BookId");
        ddlBookTitle.SelectedValue = dt.Rows[0]["BookId"].ToString();
        Txtsubtitle.Text = dt.Rows[0]["SubTitle"].ToString();
        ddlAttachedMedia.SelectedValue = dt.Rows[0]["AttachedMedia"].ToString();
        txtVol.Text = dt.Rows[0]["Vol"].ToString();
        txtPurchaseFrm.Text = dt.Rows[0]["ReceivedFrom"].ToString();
        txtReference.Text = dt.Rows[0]["Reference"].ToString();
        if (dt.Rows[0]["PurchaseDate"].ToString() != "")
        {
            dtpPurchaseDt.SetDateValue(Convert.ToDateTime(dt.Rows[0]["PurchaseDate"]));
            dtpBillDt.From.Date=dtpPurchaseDt.GetDateValue();
        }
        ddlSouceAcqr.SelectedValue = dt.Rows[0]["SourceOfAquiring"].ToString();
        txtQty.Text = dt.Rows[0]["Qty"].ToString();
        txtPrice.Text = dt.Rows[0]["PurchasePrice"].ToString();
        txtBillNo.Text = dt.Rows[0]["BillNo"].ToString();
        if (dt.Rows[0]["BillDate"].ToString() != "")
            dtpBillDt.SetDateValue(Convert.ToDateTime(dt.Rows[0]["BillDate"]));
        txtPubPlace.Text = dt.Rows[0]["PubPlace"].ToString();
        txtPubYr.Text = dt.Rows[0]["PubYear"].ToString();
        txtPages.Text = dt.Rows[0]["Pages"].ToString();
        txtISBN.Text = dt.Rows[0]["ISBN"].ToString();
        txtEdition.Text = dt.Rows[0]["Edition"].ToString();
        txtRemarks.Text = dt.Rows[0]["Remarks"].ToString();
        txtQty.Enabled = false;
        row1.Visible = false;
        btnNotInList.Enabled = false;
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    protected void btnSaveAddNew_Click(object sender, EventArgs e)
    {
        SaveData();
    }

    protected void btnSaveGotoList_Click(object sender, EventArgs e)
    {
        SaveData();
        Response.Redirect("BookPurchaseList.aspx");
    }

    private void SaveData()
    {
        try
        {
            ht.Clear();
            if (Request.QueryString["PurchaseId"] == null)
                ht.Add("@PurchaseId", 0);
            else if (btnSaveAddNew.Text == "Update & AddNew" || btnSaveGotoList.Text == "Update & GotoList")
                ht.Add("@PurchaseId", int.Parse(Request.QueryString["PurchaseId"].ToString()));
            else
                ht.Add("@PurchaseId", 0);
            if (btnSaveAddNew.Text != "Update & AddNew" || btnSaveGotoList.Text != "Update & GotoList")
                ht.Add("@AccessionNo", txtNewAccNo.Text.Trim());
            ht.Add("@BookId", int.Parse(ddlBookTitle.SelectedValue));
            if (ddlAttachedMedia.SelectedIndex > 0)
                ht.Add("@AttachedMedia", ddlAttachedMedia.SelectedValue);
            ht.Add("@PubPlace", txtPubPlace.Text.Trim());
            ht.Add("@PubYear", txtPubYr.Text.Trim());
            ht.Add("@Pages", txtPages.Text.Trim());
            ht.Add("@ISBN", txtISBN.Text.Trim());
            ht.Add("@Edition", txtEdition.Text.Trim());
            ht.Add("@Vol", txtVol.Text.Trim());
            ht.Add("@SourceOfAquiring", ddlSouceAcqr.SelectedValue);
            ht.Add("@ReceivedFrom", txtPurchaseFrm.Text.Trim());
            ht.Add("@Reference", txtReference.Text.Trim());
            if (txtPurchaseDt.Text != "")
                ht.Add("@PurchaseDate", dtpPurchaseDt.SelectedDate.ToString());
            ht.Add("@Quantity", int.Parse(txtQty.Text.Trim()));
            ht.Add("@PurchasePrice", txtPrice.Text.Trim());
            ht.Add("@BillNo", txtBillNo.Text.Trim());
            if (txtBillDt.Text != "")
                ht.Add("@BillDate", dtpBillDt.SelectedDate.ToString());
            ht.Add("@Remarks", txtRemarks.Text.Trim());
            ht.Add("@UserId", int.Parse(Session["User_Id"].ToString()));
            ht.Add("@CollegeId", Session["SchoolId"]);
            dt = obj.GetDataTable("Lib_SP_InsUpdtBookPurchase", ht);
            if (dt.Rows.Count > 0)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + dt.Rows[0][0].ToString().Trim() + "');", true);
            }
            else
            {
                if (btnSaveAddNew.Text == "Update & AddNew" || btnSaveGotoList.Text == "Update & GotoList")
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Data Updated Successfully');window.location='BookPurchase.aspx';", true);
                else
                    ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert(' Data Save Successfully');", true);
                Clear();
                FillNewAccNo();
                txtQty.Enabled = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Clear();
    }

    private void Clear()
    {
        ddlBookCat.SelectedValue = "0";
        ddlSubject.SelectedValue = "0";
        ddlBookTitle.SelectedValue = "0";
        ddlAttachedMedia.SelectedValue = "0";
        txtVol.Text = "";
        txtPurchaseFrm.Text = "";
        txtPurchaseDt.Text = "";
        ddlSouceAcqr.SelectedValue = "0";
        txtPrice.Text = "";
        txtBillNo.Text = "";
        txtBillDt.Text = "";
        txtRemarks.Text = "";
        txtEdition.Text = string.Empty;
        txtQty.Text = Convert.ToString(1);
        txtReference.Text = "";
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        Response.Redirect("BookPurchaseList.aspx");
    }

    protected void ddlBookCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlBookCat.SelectedIndex > 0)
            {
                bindDropDown(ddlSubject, "select SubName ,SubjectId from Lib_Subjects where CollegeId=" + Session["SchoolId"] + " AND CatCode=" + ddlBookCat.SelectedValue + " order by SubName", "SubName", "SubjectId");
            }
            else
            {
                ddlSubject.Items.Clear();
                ddlSubject.Items.Insert(0, new ListItem("---Select---", "0"));
            }
            ddlBookTitle.Items.Clear();
            ddlBookTitle.Items.Insert(0, new ListItem("---Select---", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlSubject.SelectedIndex > 0)
            {
                bindDropDown(ddlBookTitle, "select BookId, BookTitle from dbo.Lib_BookMaster where CollegeId=" + Session["SchoolId"] + " AND SubjectId=" + ddlSubject.SelectedValue + " order by BookTitle", "BookTitle", "BookId");
            }
            else
            {
                ddlBookTitle.Items.Clear();
                ddlBookTitle.Items.Insert(0, new ListItem("---Select---", "0"));
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void SavePostBackData()
    {
        DataTable dataTable = new DataTable();
        DataColumn column1 = new DataColumn("CatCode", Type.GetType("System.String"));
        dataTable.Columns.Add(column1);
        DataColumn column2 = new DataColumn("Subject", Type.GetType("System.String"));
        dataTable.Columns.Add(column2);
        DataRow row = dataTable.NewRow();
        row["CatCode"] = ddlBookCat.SelectedValue;
        row["Subject"] = ddlSubject.SelectedValue;
        dataTable.Rows.Add(row);
        Session["PurData"] = dataTable;
    }

    protected void btnNotInList_Click(object sender, EventArgs e)
    {
        SavePostBackData();
        Response.Redirect("NewBookEntry.aspx?FrmPurchase=T");
    }

    protected void ddlBookTitle_SelectedIndexChanged(object sender, EventArgs e)
    {
        dt = obj.GetDataTableQry("Select B.BookSubTitle,B.AuthorName1,P.PublisherName from dbo.Lib_BookMaster B inner join dbo.Lib_Publisher P on P.PublisherId=B.PublisherId where Bookid=" + ddlBookTitle.SelectedValue);
        if (dt.Rows.Count <= 0)
            return;
        Txtsubtitle.Text = dt.Rows[0]["BookSubTitle"].ToString();

        lblAuthor.Text = dt.Rows[0]["AuthorName1"].ToString();
        lblPublisher.Text = dt.Rows[0]["PublisherName"].ToString();
        pnl1.Visible = true;
    }

    protected void dtpPurchaseDt_SelectionChanged(object sender, EventArgs e)
    {
        txtBillDt.Text = "";
        //dtpBillDt.From.Date = dtpPurchaseDt.GetDateValue();
    }
   
}