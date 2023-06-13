using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_ItemSaleList : System.Web.UI.Page
{
    private clsDAL DAL = new clsDAL();
   // protected Literal lblMsg;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (!Page.IsPostBack)
        {
            fillsession();
            fillclass();
        }
        lblMsg.Text = string.Empty;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        FillBills();
    }

    private void FillBills()
    {
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (!txtFromDt.Text.Trim().Equals(string.Empty) && !txtToDt.Text.Trim().Equals(string.Empty))
        {
            hashtable.Add("@FromDt", dtpFromDt.GetDateValue());
            hashtable.Add("@ToDt", dtpToDt.GetDateValue());
        }
        if (drpstudent.SelectedIndex > 0)
            hashtable.Add("@AdmnNo", drpstudent.SelectedValue);
        if (!txtBillNo.Text.Trim().Equals(string.Empty))
            hashtable.Add("@BillNo", txtBillNo.Text.Trim());
        gvBills.DataSource = DAL.GetDataTable("ACTS_GetBills", hashtable);
        gvBills.DataBind();
    }

    protected void gvBills_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBills.PageIndex = e.NewPageIndex;
        FillBills();
    }

    [ScriptMethod]
    [WebMethod]
    public static string GetDynamicContent(string contextKey)
    {
        StringBuilder stringBuilder = new StringBuilder();
        DataSet dataSet = new DataSet();
        Hashtable hashtable = new Hashtable();
        clsDAL clsDal = new clsDAL();
        try
        {
            hashtable.Add("InvNo", contextKey);
            dataSet = clsDal.GetDataSet("ACTS_GetSaleDetailForReceipt", hashtable);
            if (dataSet.Tables[0].Rows.Count <= 0 || dataSet.Tables[1].Rows.Count <= 0)
                return string.Empty;
            stringBuilder.Append("<div style='width:90%;text-align:right;'>");
            stringBuilder.Append("<a style='cursor:pointer;background-color:black;color:white;font-weight:bold;padding:2px 5px 2px 5px;border:solid 2px red;border-radius:20px 20px;box-shadow:15px 8px 15px #21252C;' onclick='AjaxControlToolkit.PopupControlBehavior.__VisiblePopup.hidePopup();' return false;title='Close'>X</a></div>");
            stringBuilder.Append("<table style='background-color:Gray;' id='popup' border='1' cellspacing='0' cellpadding='4' style='width:90%;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<th align='left' scope='col'>Sl No.</th>");
            stringBuilder.Append("<th align='left' scope='col'>Description of Goods</th>");
            stringBuilder.Append("<th align='left' scope='col'>VAT%</th>");
            stringBuilder.Append("<th align='right' scope='col'>Quantity</th>");
            stringBuilder.Append("<th align='right' scope='col'>Rate</th>");
            stringBuilder.Append("<th align='right' scope='col'>Disc. %</th>");
            stringBuilder.Append("<th align='right' scope='col'>Amount</th>");
            stringBuilder.Append("</tr>");
            int num = 1;
            foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[1].Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left'>" + num + "</td>");
                if (double.Parse(row["TotalAmt"].ToString()) > 0.0)
                    stringBuilder.Append("<td align='left'>" + row["ItemName"].ToString() + "</td>");
                else
                    stringBuilder.Append("<td align='left'><div style='width:100%;'>" + row["ItemName"].ToString() + "<div style='float:right;padding-right:1px;color:maroon;font-style:italic;'>free</div></div></td>");
                stringBuilder.Append("<td align='left'>" + row["CST_VAT_Rate"].ToString() + "</td>");
                stringBuilder.Append("<td align='right'>" + row["Qty"].ToString() + "</td>");
                stringBuilder.Append("<td align='right'>" + row["SalePrice"].ToString() + "</td>");
                stringBuilder.Append("<td align='right'>" + row["DiscRate"].ToString() + "</td>");
                stringBuilder.Append("<td align='right'>" + row["TotalAmt"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
                ++num;
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td>&nbsp;</td>");
            stringBuilder.Append("<td align='right' style='border-color:White;border-width:0px;font-weight:bold;'>");
            stringBuilder.Append("<table border='0 style='border-color: white;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td>&nbsp;</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align: left;'>");
            stringBuilder.Append("<table cellspacing='0' border='0' style='border-color:White;border-width:0px;width:100%;border-collapse:collapse;'>");
            foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[2].Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='left'>Output VAT @" + row["CST_VAT_Rate"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align: right;'>Total</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td>&nbsp;</td>");
            stringBuilder.Append("<td>&nbsp;</td>");
            stringBuilder.Append("<td>&nbsp;</td>");
            stringBuilder.Append("<td>&nbsp;</td>");
            stringBuilder.Append("<td align='right' style='font-weight:bold;'>");
            stringBuilder.Append("<table border='0' style='width: 100%;'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align: right;'>" + dataSet.Tables[1].Compute("SUM(TotalAmt)", "").ToString() + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align: right;'>");
            stringBuilder.Append("<table cellspacing='0' border='0' style='border-width:0px;width:100%;border-collapse:collapse;'>");
            foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[2].Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='right' style='border-width:0px;'>" + row["CST_VAT_Amt"].ToString() + "</td>");
                stringBuilder.Append("</tr>");
            }
            stringBuilder.Append("</table>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='text-align: right; border-top: dotted 1px gray;'>" + string.Format("{0:0,0.00}", dataSet.Tables[1].Compute("SUM(TotAmt)", "")) + "</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("</table>");
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            dataSet.Dispose();
        }
        return stringBuilder.ToString();
    }

    protected void gvBills_RowCreated(object sender, GridViewRowEventArgs e)
    {
                
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        PopupControlExtender control1 = e.Row.FindControl("pceDetails") as PopupControlExtender;
        string str1 = "pce" + e.Row.RowIndex;
        ((ExtenderControlBase)control1).BehaviorID = str1;
        Image control2 = (Image)e.Row.Cells[1].FindControl("imgDetails");
        string str2 = string.Format("$find('{0}').showPopup();", str1);
        string.Format("$find('{0}').hidePopup();", str1);
        control2.Attributes.Add("onclick", str2);
    }

    protected void btnNewSale_Click(object sender, EventArgs e)
    {
        Response.Redirect("ItemSale.aspx");
    }

    protected void gvBills_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (!e.Row.RowType.Equals(DataControlRowType.DataRow))
            return;
        e.Row.Attributes.Add("onmouseover", "style.backgroundColor='#FFFF85'");
        e.Row.Attributes.Add("onmouseout", "style.backgroundColor='white'");
        if (!(Session["userrights"].ToString().Trim() == "a"))
            return;
        e.Row.FindControl("btnDelete").Visible = true;
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    private void fillsession()
    {
        DAL = new clsDAL();
        drpSession.DataSource = DAL.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void fillclass()
    {
        DAL = new clsDAL();
        DataTable dataTableQry = DAL.GetDataTableQry("select * from ps_classmaster");
        drpclass.DataSource = dataTableQry;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("-Select-", "0"));
        if (dataTableQry.Rows.Count > 0)
            fillstudent();
        else
            drpclass.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        drpstudent.Items.Clear();
        drpclass.SelectedIndex = 0;
        txtadmnno.Text = string.Empty;
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtadmnno.Text = drpstudent.SelectedValue;
    }

    private void fillstudent()
    {
        DAL = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select fullname,s.admnno from PS_ClasswiseStudent cs join ps_studmaster s on ");
        stringBuilder.Append(" s.admnno=cs.admnno where 1=1 ");
        if (drpclass.SelectedIndex != 0)
            stringBuilder.Append(" and cs.classid=" + drpclass.SelectedValue);
        stringBuilder.Append(" and cs.SessionYear='" + drpSession.SelectedValue.ToString().Trim() + "' and  Detained_Promoted='' order by fullname ");
        DataTable dataTableQry = DAL.GetDataTableQry(stringBuilder.ToString().Trim());
        try
        {
            drpstudent.DataSource = dataTableQry;
            drpstudent.DataTextField = "fullname";
            drpstudent.DataValueField = "admnno";
            drpstudent.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        if (dataTableQry.Rows.Count > 0)
        {
            drpstudent.Items.Insert(0, new ListItem("-Select-", "0"));
        }
        else
        {
            drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
            txtadmnno.Text = "";
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTableQry = new clsDAL().GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent where AdmnNo=" + txtadmnno.Text.Trim() + " and Detained_Promoted=''");
            if (dataTableQry.Rows.Count > 0)
            {
                drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
                drpclass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
                fillstudent();
                drpstudent.SelectedValue = txtadmnno.Text.Trim();
            }
            else
                ScriptManager.RegisterClientScriptBlock((Control)txtadmnno, txtadmnno.GetType(), "ShowMessage", "alert('Admission number does not exist !')", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void gvBills_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            Hashtable hashtable = new Hashtable();
            Label control = (Label)gvBills.Rows[e.RowIndex].FindControl("lbl2");
            hashtable.Add("@InvNo", control.Text.Trim());
            string str = DAL.ExecuteScalar("ACTS_RevertSale", hashtable);
            if (str.Trim().Equals(string.Empty))
            {
                lblMsg.Text = "<center><div style='text-align=center;width:97%;padding:5px;background-color:green;color:white;font-weight:bold;box-shadow:4px 4px 10px black;text-shadow:2px 1px black;'>Receipt reverted successfully !</div></center>";
                FillBills();
            }
            else
                lblMsg.Text = "<center><div style='text-align:center;width:97%;padding:5px;background-color:red;color:white;font-weight:bold;box-shadow4px 4px 10px black;text-shadow:2px 1px black;'>" + str + "</div></center>";
        }
        catch
        {
        }
    }
}