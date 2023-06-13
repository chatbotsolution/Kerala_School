using AjaxControlToolkit;
using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Accounts_rptPresentStock : System.Web.UI.Page
{
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        FillDroupDown();
        BindSessionYr();
        getFyDate();
        btnPrint.Visible = false;
        btnExcel.Visible = false;
    }

    private void BindSessionYr()
    {
        DataTable dataTable = new DataTable();
        drpSession.DataSource = obj.GetDataTableQry("select FY_Id,FY from dbo.ACTS_FinancialYear order by FY_Id desc");
        drpSession.DataTextField = "FY";
        drpSession.DataValueField = "FY";
        drpSession.DataBind();
    }

    private void getFyDate()
    {
        string str = drpSession.SelectedValue.Trim().Split('-')[0];
        dtpfromdt.SetDateValue(Convert.ToDateTime("1 Apr " + str));
        dtptodt.SetDateValue(Convert.ToDateTime("31 Mar " + (Convert.ToInt32(str.Trim()) + 1).ToString()));
        lblReport.Text = "";
    }

    private string checkDate()
    {
        obj = new clsDAL();
        string str = obj.ExecuteScalarQry("select count(*) from dbo.ACTS_FinancialYear where FY='" + drpSession.SelectedValue.Trim() + "' and StartDate<='" + dtpfromdt.GetDateValue().ToString("dd MMM yyyy") + "' and EndDate>='" + dtpfromdt.GetDateValue().ToString("dd MMM yyyy") + "'");
        if (str.Trim() == "" || str.Trim() == "0")
        {
            lblMsg.Text = "'From Date' is not in the selected finnancial year!!";
            lblMsg.ForeColor = Color.Red;
            return "NO";
        }
        if (!(obj.ExecuteScalarQry("select count(*) from dbo.ACTS_FinancialYear where FY='" + drpSession.SelectedValue.Trim() + "' and StartDate<='" + dtptodt.GetDateValue().ToString("dd MMM yyyy") + "' and EndDate>='" + dtptodt.GetDateValue().ToString("dd MMM yyyy") + "'").Trim() == "") && !(str.Trim() == "0"))
            return "";
        lblMsg.Text = "'To Date' is not in the selected finnancial year!!";
        lblMsg.ForeColor = Color.Red;
        return "NO";
    }

    private void FillDroupDown()
    {
        drpCat.DataSource = new clsDAL().GetDataTable("Proc_DrpGetCat");
        drpCat.DataTextField = "CatName";
        drpCat.DataValueField = "CatId";
        drpCat.DataBind();
        drpCat.Items.Insert(0, new ListItem("--All--", "0"));
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

    private void BindItem()
    {
        obj = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpCat.SelectedIndex > 0)
        {
            if (drpCat.SelectedValue == "1" || obj.ExecuteScalarQry("Select ParentCatId from dbo.SI_CategoryMaster where CatId=" + drpCat.SelectedValue.ToString()).Trim() == "1")
            {
                drpClass.Enabled = true;
                bindClass(drpClass, "select ClassID, ClassName from dbo.PS_ClassMaster order by ClassID", "ClassName", "ClassID");
            }
            else
            {
                drpClass.Items.Clear();
                drpClass.Enabled = false;
            }
        }
        else
        {
            drpClass.Items.Clear();
            drpClass.Enabled = false;
        }
    }

    protected void btnView_Click(object sender, EventArgs e)
    {
        if (checkDate().Trim() == "")
        {
            if (drpCat.SelectedIndex == 0)
                GenerateReportNew2();
            else
                GenerateReportNew();
        }
        else
            lblReport.Text = "";
    }

    private void GenerateReport()
    {
        obj = new clsDAL();
        dt = new DataTable();
        ht = new Hashtable();
        if (drpCat.SelectedIndex > 0)
            ht.Add("@CatId", drpCat.SelectedValue.ToString().Trim());
        if (drpClass.Enabled)
            ht.Add("@ClassId", drpClass.SelectedValue.ToString().Trim());
        ht.Add("@FromDt", dtpfromdt.GetDateValue().ToString("dd MMM yyyy 00:00:00"));
        ht.Add("@ToDt", dtptodt.GetDateValue().ToString("dd MMM yyyy 23:59:59"));
        ht.Add("@FY", drpSession.SelectedValue.Trim());
        ht.Add("@SchoolId", Session["SchoolId"]);
        dt = obj.GetDataTable("ACTS_GetPresentStockItem", ht);
        if (dt.Rows.Count > 0)
        {
            CreateHtmlRepotr(dt);
        }
        else
        {
            lblReport.Text = string.Empty;
            lblMsg.Text = "No Records Found";
            lblMsg.ForeColor = Color.Red;
            btnPrint.Visible = false;
            btnExcel.Visible = false;
        }
    }

    private void CreateHtmlRepotr(DataTable dt)
    {
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td colspan='4' style='border-right-style:none;' align='right' class='gridtext'><font color='Black'><b>Consolidated Stock</b></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("<td colspan='2' style='border-left-style:none;' class='gridtext'><font color='Black'><div style='float:right;'>No Of Records: " + dt.Rows.Count.ToString() + "</div></font>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        stringBuilder.Append("<table border='1px' cellpadding='1' cellspacing='0' width='100%' style='background-color: #FFF; border-collapse:collapse;border-color:black;'>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style='width: 30px' align='left'  class='gridtext'><b>Sl No</b></td>");
        stringBuilder.Append("<td align='left' class='gridtext'><b>Item Name</b></td>");
        stringBuilder.Append("<td style='width: 80px' align='left' class='gridtext'><b>Openning Qty</b></td>");
        stringBuilder.Append("<td style='width: 50px' align='left' class='gridtext'><b>Purchased/SaleRet Qty</b></td>");
        stringBuilder.Append("<td style='width: 80px' align='left' class='gridtext'><b>Sale Qty</b></td>");
        stringBuilder.Append("<td style='width: 80px' align='left' class='gridtext'><b>Closing Qty</b></td>");
        stringBuilder.Append("<td style='width: 50px' align='left' class='gridtext'><b>Unit Price</b></td>");
        stringBuilder.Append("<td style='width: 60px;text-align:right;' align='right' class='gridtext'><b>Stock Value</b></td>");
        stringBuilder.Append("</tr>");
        int num = 1;
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td align='left' class='gridtext'>" + num.ToString() + "</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["ItemName"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["OpngStock"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["PurQty"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='left' class='gridtext'>");
            stringBuilder.Append(row["SaleQty"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='gridtext' style='width: 120px;text-align:right;'>");
            stringBuilder.Append(Convert.ToDecimal(row["OpngStock"].ToString().Trim()) + Convert.ToDecimal(row["PurQty"].ToString().Trim()) - Convert.ToDecimal(row["SaleQty"].ToString().Trim()));
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='gridtext' style='width: 120px;text-align:right;'>");
            stringBuilder.Append(row["SalePrice"].ToString().Trim());
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td align='right' class='gridtext' style='width: 120px;text-align:right;'>");
            stringBuilder.Append((Convert.ToDecimal(row["OpngStock"].ToString().Trim()) + Convert.ToDecimal(row["PurQty"].ToString().Trim()) - Convert.ToDecimal(row["SaleQty"].ToString().Trim())) * Convert.ToDecimal(row["SalePrice"].ToString().Trim()));
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            ++num;
        }
        string empty = string.Empty;
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td align='right' class='tbltd' style='Font-Size:12px;' colspan='4'><b>Total Stock Value :</b></td>");
        stringBuilder.Append("<td class='gridtext' style='text-align:right;'><b>" + empty.ToString() + "</b></td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["PresentStock"] = stringBuilder.ToString().Trim();
        btnExcel.Visible = true;
        btnPrint.Visible = true;
    }

    private void GenerateReportNew()
    {
        StringBuilder stringBuilder = new StringBuilder();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        if (drpCat.SelectedIndex > 0)
            hashtable.Add("@CatId", drpCat.SelectedValue.ToString().Trim());
        if (drpClass.Enabled)
            hashtable.Add("@ClassId", drpClass.SelectedValue.ToString().Trim());
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        hashtable.Add("@FromDt", dtpfromdt.GetDateValue().ToString("dd-MMM-yyyy"));
        hashtable.Add("@ToDt", dtptodt.GetDateValue().ToString("dd-MMM-yyyy"));
        DataTable dataTable2 = obj.GetDataTable("Acts_RptGetItemList", hashtable);
        int num1 = 1;
        if (dataTable2.Rows.Count > 0)
        {
            stringBuilder.Append("<table border='1px' cellpadding='2' cellspacing='0' align='center' width='100%' style='border-collapse:collapse;border-color:black;font-size:12px'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='6' style='border-right-style:none;' align='center'><font color='Black'><b>Consolidated Stock Statement&nbsp;From&nbsp;" + dtpfromdt.GetDateValue().ToString("dd MMM yyyy") + " &nbsp;To&nbsp;" + dtptodt.GetDateValue().ToString("dd MMM yyyy") + "</b></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td colspan='4' style='border-left-style:none;' class='gridtext'><font color='Black'><div style='float:right;'>No Of Records: " + dataTable2.Rows.Count.ToString() + "</div></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 4%' align='center'><b>Sl.No.</b></td>");
            stringBuilder.Append("<td style='width: 20%' align='left'><b>Item Name</b></td>");
            stringBuilder.Append("<td style='width: 10%' align='left'><b>Opening Stock</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Purchase/Sale Ret</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Sale</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Issue</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Damaged</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Net Stock</b></td>");
            stringBuilder.Append("<td style='width: 9%' align='right'><b>Unit Price</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='right'><b>StockValue</b></td>");
            stringBuilder.Append("</tr>");
            Decimal num2 = new Decimal(0);
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='center'>" + num1 + "</td>");
                stringBuilder.Append("<td align='left'>" + row["ItemName"] + "</td>");
                DataTable dataTable3 = new DataTable();
                DataTable dataTable4 = obj.GetDataTable("ACTS_GetPresentStockItem", new Hashtable()
        {
          {
             "@ItemCode",
            row["ItemCode"]
          },
          {
             "@FY",
             drpSession.Text.ToString().Trim()
          },
          {
             "@FromDt",
             dtpfromdt.GetDateValue().ToString("dd MMM yyyy")
          },
          {
             "@ToDt",
             dtptodt.GetDateValue().ToString("dd MMM yyyy")
          }
        });
                if (dataTable4.Rows.Count > 0)
                {
                    Decimal num3 = new Decimal(0);
                    Decimal num4 = new Decimal(0);
                    Decimal num5 = new Decimal(0);
                    Decimal num6 = new Decimal(0);
                    Decimal num7 = new Decimal(0);
                    Decimal num8 = new Decimal(0);
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["OpeningStock"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["Purchase"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["Sale"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["Issue"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["WritesOff"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    Decimal num9 = (!(dataTable4.Rows[0]["OpeningStock"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["OpeningStock"].ToString().Trim())) + (!(dataTable4.Rows[0]["Purchase"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["Purchase"].ToString().Trim())) - (!(dataTable4.Rows[0]["Sale"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["Sale"].ToString().Trim())) - (!(dataTable4.Rows[0]["Issue"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["Issue"].ToString().Trim())) - (!(dataTable4.Rows[0]["Writesoff"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["Writesoff"].ToString().Trim()));
                    stringBuilder.Append("<td align='left'>" + num9.ToString("0.00") + "</td>");
                    stringBuilder.Append("<td align='right'>" + row["SalePrice"] + "</td>");
                    Decimal num10 = new Decimal(0);
                    Decimal num11 = Decimal.Parse(row["SalePrice"].ToString()) * num9;
                    stringBuilder.Append("<td align='right'>" + num11.ToString("0.00") + "</td>");
                    num2 += num11;
                }
                stringBuilder.Append("</tr>");
                ++num1;
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='9' align='right'><b>Total Stock Value </b></td>");
            stringBuilder.Append("<td align='right'><b>" + num2.ToString("0.00") + "</b></td>");
            stringBuilder.Append("</tr>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["PresentStock"] = stringBuilder.ToString().Trim();
            btnExcel.Visible = true;
            btnPrint.Visible = true;
        }
        else
        {
            lblReport.Text = string.Empty;
            lblMsg.Text = "No Records Found";
            lblMsg.ForeColor = Color.Red;
            btnPrint.Visible = false;
            btnExcel.Visible = false;
        }
    }

    private void GenerateReportNew2()
    {
        StringBuilder stringBuilder = new StringBuilder();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable1 = new Hashtable();
        if (drpCat.SelectedIndex > 0)
            hashtable1.Add("@CatId", drpCat.SelectedValue.ToString().Trim());
        if (drpCat.SelectedIndex == 0)
            hashtable1.Add("@Status", 1);
        if (drpClass.Enabled)
            hashtable1.Add("@ClassId", drpClass.SelectedValue.ToString().Trim());
        hashtable1.Add("@SchoolId", Session["SchoolId"]);
        hashtable1.Add("@FromDt", dtpfromdt.GetDateValue().ToString("dd-MMM-yyyy"));
        hashtable1.Add("@ToDt", dtptodt.GetDateValue().ToString("dd-MMM-yyyy"));
        DataTable dataTable2 = obj.GetDataTable("Acts_RptGetItemList", hashtable1);
        int num1 = 1;
        if (dataTable2.Rows.Count > 0)
        {
            stringBuilder.Append("<table border='1px' cellpadding='2' cellspacing='0' align='center' width='100%' style='border-collapse:collapse;border-color:black;font-size:12px'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='6' style='border-right-style:none;' align='center'><font color='Black'><b>Consolidated Stock Statement&nbsp;From&nbsp;" + dtpfromdt.GetDateValue().ToString("dd MMM yyyy") + " &nbsp;To&nbsp;" + dtptodt.GetDateValue().ToString("dd MMM yyyy") + "</b></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td colspan='4' style='border-left-style:none;' class='gridtext'><font color='Black'><div style='float:right;'>No Of Records: " + dataTable2.Rows.Count.ToString() + "</div></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='10' style='border-left-style:none;' class='gridtext'><font color='Black'><div style='float:Left;'><b>All Salable Items<b></div></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 4%' align='center'><b>Sl.No.</b></td>");
            stringBuilder.Append("<td style='width: 20%' align='left'><b>Item Name</b></td>");
            stringBuilder.Append("<td style='width: 10%' align='left'><b>Opening Stock</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Purchase/Sale Ret</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Sale</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Issue</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Damaged</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Net Stock</b></td>");
            stringBuilder.Append("<td style='width: 9%' align='right'><b>Unit Price</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='right'><b>StockValue</b></td>");
            stringBuilder.Append("</tr>");
            Decimal num2 = new Decimal(0);
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='center'>" + num1 + "</td>");
                stringBuilder.Append("<td align='left'>" + row["ItemName"] + "</td>");
                DataTable dataTable3 = new DataTable();
                DataTable dataTable4 = obj.GetDataTable("ACTS_GetPresentStockItem", new Hashtable()
        {
          {
             "@ItemCode",
            row["ItemCode"]
          },
          {
             "@FY",
             drpSession.Text.ToString().Trim()
          },
          {
             "@FromDt",
             dtpfromdt.GetDateValue().ToString("dd MMM yyyy")
          },
          {
             "@ToDt",
             dtptodt.GetDateValue().ToString("dd MMM yyyy")
          }
        });
                if (dataTable4.Rows.Count > 0)
                {
                    Decimal num3 = new Decimal(0);
                    Decimal num4 = new Decimal(0);
                    Decimal num5 = new Decimal(0);
                    Decimal num6 = new Decimal(0);
                    Decimal num7 = new Decimal(0);
                    Decimal num8 = new Decimal(0);
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["OpeningStock"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["Purchase"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["Sale"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["Issue"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["WritesOff"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    Decimal num9 = (!(dataTable4.Rows[0]["OpeningStock"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["OpeningStock"].ToString().Trim())) + (!(dataTable4.Rows[0]["Purchase"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["Purchase"].ToString().Trim())) - (!(dataTable4.Rows[0]["Sale"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["Sale"].ToString().Trim())) - (!(dataTable4.Rows[0]["Issue"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["Issue"].ToString().Trim())) - (!(dataTable4.Rows[0]["Writesoff"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["Writesoff"].ToString().Trim()));
                    stringBuilder.Append("<td align='left'>" + num9.ToString("0.00") + "</td>");
                    stringBuilder.Append("<td align='right'>" + row["SalePrice"] + "</td>");
                    Decimal num10 = new Decimal(0);
                    Decimal num11 = Decimal.Parse(row["SalePrice"].ToString()) * num9;
                    stringBuilder.Append("<td align='right'>" + num11.ToString("0.00") + "</td>");
                    num2 += num11;
                }
                stringBuilder.Append("</tr>");
                ++num1;
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='9' align='right'><b>Total Stock Value </b></td>");
            stringBuilder.Append("<td align='right'><b>" + num2.ToString("0.00") + "</b></td>");
            stringBuilder.Append("</tr>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["PresentStock"] = stringBuilder.ToString().Trim();
            btnExcel.Visible = true;
            btnPrint.Visible = true;
        }
        else
        {
            lblReport.Text = string.Empty;
            lblMsg.Text = "No Records Found";
            lblMsg.ForeColor = Color.Red;
        }
        dataTable1 = new DataTable();
        Hashtable hashtable2 = new Hashtable();
        if (drpCat.SelectedIndex > 0)
            hashtable2.Add("@CatId", drpCat.SelectedValue.ToString().Trim());
        if (drpCat.SelectedIndex == 0)
            hashtable2.Add("@Status", 2);
        if (drpClass.Enabled)
            hashtable2.Add("@ClassId", drpClass.SelectedValue.ToString().Trim());
        hashtable2.Add("@SchoolId", Session["SchoolId"]);
        hashtable2.Add("@FromDt", dtpfromdt.GetDateValue().ToString("dd-MMM-yyyy"));
        hashtable2.Add("@ToDt", dtptodt.GetDateValue().ToString("dd-MMM-yyyy"));
        DataTable dataTable5 = obj.GetDataTable("Acts_RptGetItemList", hashtable2);
        int num12 = 1;
        if (dataTable5.Rows.Count > 0)
        {
            stringBuilder.Append("<table border='1px' cellpadding='2' cellspacing='0' align='center' width='100%' style='border-collapse:collapse;border-color:black;font-size:12px'>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='6' style='border-right-style:none;' align='center'><font color='Black'><b>Consolidated Stock Statement&nbsp;From&nbsp;" + dtpfromdt.GetDateValue().ToString("dd MMM yyyy") + " &nbsp;To&nbsp;" + dtptodt.GetDateValue().ToString("dd MMM yyyy") + "</b></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("<td colspan='4' style='border-left-style:none;' class='gridtext'><font color='Black'><div style='float:right;'>No Of Records: " + dataTable5.Rows.Count.ToString() + "</div></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='10' style='border-left-style:none;' class='gridtext'><font color='Black'><div style='float:Left;'><b>All Non Salable Items<b></div></font>");
            stringBuilder.Append("</td>");
            stringBuilder.Append("</tr>");
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td style='width: 4%' align='center'><b>Sl.No.</b></td>");
            stringBuilder.Append("<td style='width: 20%' align='left'><b>Item Name</b></td>");
            stringBuilder.Append("<td style='width: 10%' align='left'><b>Opening Stock</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Purchase/Sale Ret</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Sale</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Issue</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Damaged</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='left'><b>Net Stock</b></td>");
            stringBuilder.Append("<td style='width: 9%' align='right'><b>Unit Price</b></td>");
            stringBuilder.Append("<td style='width: 8%' align='right'><b>StockValue</b></td>");
            stringBuilder.Append("</tr>");
            Decimal num2 = new Decimal(0);
            foreach (DataRow row in (InternalDataCollectionBase)dataTable5.Rows)
            {
                stringBuilder.Append("<tr>");
                stringBuilder.Append("<td align='center'>" + num12 + "</td>");
                stringBuilder.Append("<td align='left'>" + row["ItemName"] + "</td>");
                DataTable dataTable3 = new DataTable();
                DataTable dataTable4 = obj.GetDataTable("ACTS_GetPresentStockItem", new Hashtable()
        {
          {
             "@ItemCode",
            row["ItemCode"]
          },
          {
             "@FY",
             drpSession.Text.ToString().Trim()
          },
          {
             "@FromDt",
             dtpfromdt.GetDateValue().ToString("dd MMM yyyy")
          },
          {
             "@ToDt",
             dtptodt.GetDateValue().ToString("dd MMM yyyy")
          }
        });
                if (dataTable4.Rows.Count > 0)
                {
                    Decimal num3 = new Decimal(0);
                    Decimal num4 = new Decimal(0);
                    Decimal num5 = new Decimal(0);
                    Decimal num6 = new Decimal(0);
                    Decimal num7 = new Decimal(0);
                    Decimal num8 = new Decimal(0);
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["OpeningStock"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["Purchase"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["Sale"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["Issue"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    stringBuilder.Append("<td align='left'>" + dataTable4.Rows[0]["WritesOff"].ToString() + " " + row["MesuringUnit"] + "</td>");
                    Decimal num9 = (!(dataTable4.Rows[0]["OpeningStock"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["OpeningStock"].ToString().Trim())) + (!(dataTable4.Rows[0]["Purchase"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["Purchase"].ToString().Trim())) - (!(dataTable4.Rows[0]["Sale"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["Sale"].ToString().Trim())) - (!(dataTable4.Rows[0]["Issue"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["Issue"].ToString().Trim())) - (!(dataTable4.Rows[0]["Writesoff"].ToString() != "") ? new Decimal(0) : Decimal.Parse(dataTable4.Rows[0]["Writesoff"].ToString().Trim()));
                    stringBuilder.Append("<td align='left'>" + num9.ToString("0.00") + "</td>");
                    stringBuilder.Append("<td align='right'>" + row["SalePrice"] + "</td>");
                    Decimal num10 = new Decimal(0);
                    Decimal num11 = Decimal.Parse(row["SalePrice"].ToString()) * num9;
                    stringBuilder.Append("<td align='right'>" + num11.ToString("0.00") + "</td>");
                    num2 += num11;
                }
                stringBuilder.Append("</tr>");
                ++num12;
            }
            stringBuilder.Append("<tr>");
            stringBuilder.Append("<td colspan='9' align='right'><b>Total Stock Value </b></td>");
            stringBuilder.Append("<td align='right'><b>" + num2.ToString("0.00") + "</b></td>");
            stringBuilder.Append("</tr>");
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["PresentStock"] = stringBuilder.ToString().Trim();
            btnExcel.Visible = true;
            btnPrint.Visible = true;
        }
        else
        {
            lblReport.Text = stringBuilder.ToString().Trim();
            Session["PresentStock"] = stringBuilder.ToString().Trim();
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptPresentStockPrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                ExportToExcel((stringBuilder.ToString().Trim() + Session["PresentStock"].ToString()).ToString().Trim());
            }
            else
                Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/PresentStock" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=rptPresentStock" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpCat_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindItem();
        lblMsg.Text = string.Empty;
        lblReport.Text = string.Empty;
    }

    protected void drpBrand_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindItem();
        lblMsg.Text = string.Empty;
        lblReport.Text = string.Empty;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        getFyDate();
    }
}