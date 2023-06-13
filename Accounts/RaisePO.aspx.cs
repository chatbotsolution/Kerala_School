using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_RaisePO : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            lblMsg.Text = "";
            if (!Page.IsPostBack)
            {
                bindDropDown(ddlCategory, "Proc_DrpGetCat", "CatName", "CatId");
                bindSupplier();
                Bind();
            }
            if (Session["Table"] != null)
                return;
            CreateDataTable();
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTable = obj.GetDataTable(query);
        drp.DataSource = dataTable;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    private void bindSupplier()
    {
        try
        {
            drpSupplier.DataSource = obj.GetDataTableQry("select PartyName,PartyId from dbo.ACTS_PartyMaster where PartyType='Supplier'");
            drpSupplier.DataTextField = "PartyName";
            drpSupplier.DataValueField = "PartyId";
            drpSupplier.DataBind();
            drpSupplier.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    private void bindClass(DropDownList drp, string query, string textfield, string valuefield)
    {
        try
        {
            drp.DataSource = null;
            drp.DataBind();
            DataTable dataTableQry = obj.GetDataTableQry(query);
            drp.DataSource = dataTableQry;
            drp.DataTextField = textfield;
            drp.DataValueField = valuefield;
            drp.DataBind();
            drp.Items.Insert(0, new ListItem("--All--", "0"));
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void CreateDataTable()
    {
        DataTable dataTable = new DataTable();
        DataColumn column1 = new DataColumn("CatCode", Type.GetType("System.String"));
        dataTable.Columns.Add(column1);
        DataColumn column2 = new DataColumn("Category", Type.GetType("System.String"));
        dataTable.Columns.Add(column2);
        DataColumn column3 = new DataColumn("ItemCode", Type.GetType("System.String"));
        dataTable.Columns.Add(column3);
        DataColumn column4 = new DataColumn("ItemName", Type.GetType("System.String"));
        dataTable.Columns.Add(column4);
        DataColumn column5 = new DataColumn("Quantity", Type.GetType("System.String"));
        dataTable.Columns.Add(column5);
        DataColumn column6 = new DataColumn("Unit", Type.GetType("System.String"));
        dataTable.Columns.Add(column6);
        Session["Table"] = dataTable;
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlCategory.SelectedIndex > 0)
        {
            if (ddlCategory.SelectedValue == "1" || obj.ExecuteScalarQry("Select ParentCatId from dbo.SI_CategoryMaster where CatId=" + ddlCategory.SelectedValue.ToString()).Trim() == "1")
            {
                pnlclass.Visible = true;
                bindClass(ddlClass, "select ClassID, ClassName from dbo.PS_ClassMaster order by ClassID", "ClassName", "ClassID");
            }
            else
            {
                ddlClass.SelectedIndex = 0;
                pnlclass.Visible = false;
            }
            pnl1.Visible = true;
            ht.Clear();
            ht.Add("@NoOfChar", 10);
            ht.Add("@Catid", int.Parse(ddlCategory.SelectedValue));
            if (ddlClass.SelectedIndex > 0)
                ht.Add("@ClassId", ddlClass.SelectedValue);
            dt = obj.GetDataTable("ACTS_GetItemList", ht);
            ddlItem.DataSource = null;
            ddlItem.DataBind();
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemNameFull";
            ddlItem.DataValueField = "ItemCode";
            ddlItem.DataBind();
            if (dt.Rows.Count > 0)
            {
                btnAddItem.Visible = true;
                ddlItem.Items.Insert(0, new ListItem("--Item List--", "0"));
            }
            else
            {
                btnAddItem.Visible = false;
                ddlItem.Items.Insert(0, new ListItem("--No Items--", "0"));
            }
            gvItemList.DataSource = dt;
            gvItemList.DataBind();
            ViewState["ItemList"] = dt;
        }
        else
        {
            pnlclass.Visible = false;
            pnl1.Visible = false;
            btnAddItem.Visible = false;
            ddlItem.Items.Clear();
            ddlItem.Items.Insert(0, new ListItem("--No Items--", "0"));
        }
    }

    protected void ddlItem_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnl1.Visible = true;
        if (ddlItem.SelectedIndex > 0)
        {
            dt = (DataTable)ViewState["ItemList"];
            dt = ((IEnumerable<DataRow>)dt.Select("ItemCode=" + ddlItem.SelectedValue)).CopyToDataTable<DataRow>();
            if (dt.Rows.Count > 0)
                btnAddItem.Visible = true;
            else
                btnAddItem.Visible = false;
        }
        else
        {
            dt = obj.GetDataTable("ACTS_GetItemListForClass", new Hashtable()
      {
        {
           "@NoOfChar",
           10
        },
        {
           "@Catid",
           int.Parse(ddlCategory.SelectedValue)
        },
        {
           "@ClassId",
           int.Parse(ddlClass.SelectedValue)
        }
      });
            if (dt.Rows.Count > 0)
                btnAddItem.Visible = true;
            else
                btnAddItem.Visible = false;
        }
        gvItemList.DataSource = dt;
        gvItemList.DataBind();
    }

    protected void btnAddItem_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTable = (DataTable)Session["Table"];
            string[] s = Request["Checkb"].Split(',');
            if (ValidateQty(s))
            {
                lblMsg.Text = "";
                foreach (GridViewRow row1 in gvItemList.Rows)
                {
                    string str1 = "false";
                    string str2 = "false";
                    HiddenField control1 = (HiddenField)row1.FindControl("hdnItemCode");
                    for (int index = 0; index < s.Length; ++index)
                    {
                        if (control1.Value == s[index].ToString())
                        {
                            str2 = "true";
                            break;
                        }
                    }
                    if (str2 == "true")
                    {
                        HiddenField control2 = (HiddenField)row1.FindControl("hdnItemName");
                        TextBox control3 = (TextBox)row1.FindControl("txtQty");
                        Label control4 = (Label)row1.FindControl("lblUnit");
                        foreach (DataRow row2 in (InternalDataCollectionBase)dataTable.Rows)
                        {
                            if (row2["ItemCode"].ToString() == control1.Value.ToString())
                            {
                                row2["Quantity"] = control3.Text;
                                dataTable.AcceptChanges();
                                str1 = "true";
                                break;
                            }
                        }
                        if (str1 == "false")
                        {
                            DataRow row2 = dataTable.NewRow();
                            row2["CatCode"] = ddlCategory.SelectedValue;
                            row2["Category"] = ddlCategory.SelectedItem;
                            row2["ItemCode"] = control1.Value;
                            row2["ItemName"] = control2.Value;
                            row2["Quantity"] = control3.Text.Trim();
                            row2["Unit"] = control4.Text.Trim();
                            dataTable.Rows.Add(row2);
                        }
                    }
                }
                Session["Table"] = dataTable;
                Bind();
                ddlCategory.SelectedValue = "0";
                ddlCategory_SelectedIndexChanged(sender, e);
            }
            else
            {
                lblMsg.Text = "Please Enter Quantity !";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private bool ValidateQty(string[] s)
    {
        bool flag = true;
        foreach (GridViewRow row in gvItemList.Rows)
        {
            HiddenField control1 = (HiddenField)row.FindControl("hdnItemCode");
            TextBox control2 = (TextBox)row.FindControl("txtQty");
            for (int index = 0; index < s.Length; ++index)
            {
                if (control1.Value == s[index].ToString() && control2.Text.Trim() == "")
                {
                    flag = false;
                    break;
                }
            }
        }
        return flag;
    }

    private void Bind()
    {
        if (Session["Table"] != null)
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = (DataTable)Session["Table"];
            if (dataTable2.Rows.Count > 0)
            {
                pnl2.Visible = true;
                gvItemsToPurchase.DataSource = dataTable2;
                gvItemsToPurchase.DataBind();
            }
            else
                pnl2.Visible = false;
        }
        else
            pnl2.Visible = false;
    }

    protected void gvItemsToPurchase_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            DataTable dataTable = (DataTable)Session["Table"];
            if (!(e.CommandName == "Remove"))
                return;
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                if (row["ItemCode"].ToString() == e.CommandArgument.ToString())
                {
                    dataTable.Rows.Remove(row);
                    break;
                }
            }
            dataTable.AcceptChanges();
            Session["Table"] = dataTable;
            Bind();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again .";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        SqlConnection sqlConnection = new SqlConnection();
        sqlConnection.ConnectionString = ConfigurationManager.AppSettings["conString"];
        SqlTransaction sqlTransaction = (SqlTransaction)null;
        try
        {
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = (DataTable)Session["Table"];
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            sqlTransaction = sqlConnection.BeginTransaction();
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "ACTS_InsPurchaseOrder";
            sqlCommand.Transaction = sqlTransaction;
            SqlParameter sqlParameter1 = new SqlParameter();
            SqlParameter sqlParameter2 = new SqlParameter();
            SqlParameter sqlParameter3 = new SqlParameter();
            SqlParameter sqlParameter4 = new SqlParameter();
            SqlParameter sqlParameter5 = new SqlParameter();
            SqlParameter sqlParameter6 = new SqlParameter();
            SqlParameter sqlParameter7 = new SqlParameter();
            string str1 = "";
            string str2 = "";
            for (int index = 0; index < dataTable2.Rows.Count; ++index)
            {
                str1 = str1 + dataTable2.Rows[index]["ItemCode"].ToString() + ",";
                str2 = str2 + dataTable2.Rows[index]["Quantity"].ToString() + ",";
            }
            sqlCommand.Parameters.Clear();
            SqlParameter sqlParameter8 = new SqlParameter("@PartyId", drpSupplier.SelectedValue.ToString());
            SqlParameter sqlParameter9 = new SqlParameter("@OrderDate", dtpOrederDt.GetDateValue());
            SqlParameter sqlParameter10 = new SqlParameter("@ExpectedDeliveryDate", dtpExpDelDt.GetDateValue());
            SqlParameter sqlParameter11 = new SqlParameter("@ItemList", str1);
            SqlParameter sqlParameter12 = new SqlParameter("@QtyList", str2);
            SqlParameter sqlParameter13 = new SqlParameter("@SchoolId", int.Parse(Session["SchoolId"].ToString()));
            SqlParameter sqlParameter14 = new SqlParameter("@UserId", int.Parse(Session["User_Id"].ToString()));
            sqlCommand.Parameters.Add(sqlParameter8);
            sqlCommand.Parameters.Add(sqlParameter9);
            sqlCommand.Parameters.Add(sqlParameter10);
            sqlCommand.Parameters.Add(sqlParameter11);
            sqlCommand.Parameters.Add(sqlParameter12);
            sqlCommand.Parameters.Add(sqlParameter13);
            sqlCommand.Parameters.Add(sqlParameter14);
            sqlCommand.ExecuteScalar();
            lblMsg.Text = "Order Placed Successfully";
            lblMsg.ForeColor = Color.Green;
            drpSupplier.SelectedIndex = 0;
            pnl2.Visible = false;
            Session["Table"] = null;
            sqlTransaction.Commit();
            sqlTransaction.Dispose();
            sqlConnection.Dispose();
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Transaction Failed ! Try Again";
            lblMsg.ForeColor = Color.Red;
            sqlTransaction.Rollback();
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        Response.Redirect("RaisePOList.aspx");
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Session["Table"] = null;
        Bind();
    }

    protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ht.Clear();
            ht.Add("@NoOfChar", 10);
            ht.Add("@Catid", int.Parse(ddlCategory.SelectedValue));
            ht.Add("@ClassId", int.Parse(ddlClass.SelectedValue));
            dt = obj.GetDataTable("ACTS_GetItemListForClass", ht);
            ddlItem.DataSource = dt;
            ddlItem.DataTextField = "ItemNameFull";
            ddlItem.DataValueField = "ItemCode";
            ddlItem.DataBind();
            if (dt.Rows.Count > 0)
                ddlItem.Items.Insert(0, new ListItem("--Item List--", "0"));
            else
                ddlItem.Items.Insert(0, new ListItem("--No Items--", "0"));
            gvItemList.DataSource = dt;
            gvItemList.DataBind();
            if (dt.Rows.Count > 0)
                btnAddItem.Visible = true;
            else
                btnAddItem.Visible = false;
            ViewState["ItemList"] = dt;
        }
        catch (Exception ex)
        {
        }
    }
}