using AjaxControlToolkit;
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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Accounts_OpenFY : System.Web.UI.Page
{
    private static string clear = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            clsDAL clsDal = new clsDAL();
            lblMsg.Text = string.Empty;
            lblBottomMsg.Text = string.Empty;
            FillAccountCodesDynamically("");
            if (Page.IsPostBack)
                return;
            calYearStartDate.SetDateValue(new DateTime(DateTime.Now.Year, 4, 1));
            calYearEndDate.SetDateValue(new DateTime(DateTime.Now.Year + 1, 3, 31));
            if (Request.QueryString["id"] != null)
            {
                phControls.Controls.Clear();
                FillAccountCodesDynamically(Request.QueryString["id"].ToString().Trim());
                CalculateAmount();
                txtYearStartDate.Enabled = false;
                calYearStartDate.Enabled = false;
                txtYearEndDate.Enabled = false;
                calYearEndDate.Enabled = false;
            }
            else
            {
                phControls.Controls.Clear();
                GetNewFYDtls();
                CalculateAmount();
            }
            HiddenField1.Value = Convert.ToString(DateTime.Now.Year + 1);
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void GetNewFYDtls()
    {
        clsDAL clsDal1 = new clsDAL();
        DataTable dataTable1 = new DataTable();
        string str1 = "Select top(1) convert(char(10),StartDate,103) as startDate ,convert(char(10),EndDate,103) as endDate from ACTS_FinancialYear order by FY_Id desc";
        clsDAL clsDal2 = new clsDAL();
        dataTable1 = new DataTable();
        DataTable dataTableQry = clsDal2.GetDataTableQry(str1);
        if (dataTableQry.Rows.Count > 0)
        {
            char[] chArray = new char[1] { '/' };
            string[] strArray1 = dataTableQry.Rows[0][0].ToString().Trim().Split(chArray);
            string[] strArray2 = dataTableQry.Rows[0][1].ToString().Trim().Split(chArray);
            calYearStartDate.SetDateValue(new DateTime(Convert.ToInt32(strArray1[2].ToString().Trim()) + 1, Convert.ToInt32(strArray1[1].ToString().Trim()), Convert.ToInt32(strArray1[0].ToString().Trim())));
            calYearEndDate.SetDateValue(new DateTime(Convert.ToInt32(strArray2[2].ToString().Trim()) + 1, Convert.ToInt32(strArray2[1].ToString().Trim()), Convert.ToInt32(strArray2[0].ToString().Trim())));
            Hashtable hashtable = new Hashtable();
            hashtable.Add("@FYStartDt", new DateTime(Convert.ToInt32(strArray1[2].ToString().Trim()), Convert.ToInt32(strArray1[1].ToString().Trim()), Convert.ToInt32(strArray1[0].ToString().Trim())));
            hashtable.Add("@FYEndDt", new DateTime(Convert.ToInt32(strArray2[2].ToString().Trim()), Convert.ToInt32(strArray2[1].ToString().Trim()), Convert.ToInt32(strArray2[0].ToString().Trim())));
            clsDAL clsDal3 = new clsDAL();
            DataTable dataTable2 = new DataTable();
            DataTable dataTable3 = clsDal3.GetDataTable("ACTS_GetNewFyOpngBal", hashtable);
            StringBuilder stringBuilder = new StringBuilder();
            if (dataTable3.Rows.Count > 0)
            {
                int num1 = 1;
                HtmlTable htmlTable = new HtmlTable();
                htmlTable.CellPadding = 0;
                htmlTable.CellSpacing = 0;
                htmlTable.Width = "600px";
                htmlTable.Border = 1;
                htmlTable.Attributes.Add("class", "innertbltxt");
                HtmlTableRow row1 = new HtmlTableRow();
                HtmlTableCell cell1 = new HtmlTableCell();
                cell1.Align = "left";
                cell1.VAlign = "top";
                cell1.ColSpan = 4;
                cell1.Attributes.Add("class", "pageSectionLabel");
                cell1.InnerHtml = "List Of Accounts Heads<hr />";
                row1.Cells.Add(cell1);
                htmlTable.Rows.Add(row1);
                HtmlTableRow row2 = new HtmlTableRow();
                HtmlTableCell cell2 = new HtmlTableCell();
                cell2.Style.Add("height", "10px");
                cell2.ColSpan = 4;
                row2.Cells.Add(cell2);
                htmlTable.Rows.Add(row2);
                htmlTable.Rows.Add(row2);
                HtmlTableRow row3 = new HtmlTableRow();
                row3.Align = "left";
                row3.VAlign = "top";
                row3.Attributes.Add("class", "pageLabel");
                HtmlTableCell cell3 = new HtmlTableCell();
                cell3.Align = "center";
                cell3.Style.Add("width", "10%");
                cell3.InnerText = "S.No.";
                row3.Cells.Add(cell3);
                row3.Style.Add("text-decoration", "underline");
                HtmlTableCell cell4 = new HtmlTableCell();
                cell4.Style.Add("width", "50%");
                cell4.InnerText = "Account Head";
                row3.Cells.Add(cell4);
                HtmlTableCell cell5 = new HtmlTableCell();
                cell5.Style.Add("width", "20%");
                cell5.InnerText = "Debit/Credit";
                row3.Cells.Add(cell5);
                HtmlTableCell cell6 = new HtmlTableCell();
                cell6.Style.Add("width", "20%");
                cell6.InnerText = "Amount";
                row3.Cells.Add(cell6);
                htmlTable.Rows.Add(row3);
                htmlTable.Rows.Add(row2);
                foreach (DataRow row4 in (InternalDataCollectionBase)dataTable3.Rows)
                {
                    double num2 = 0.0;
                    string str2 = row4["CrDr"].ToString().Trim();
                    num2 = Convert.ToDouble(row4["TransAmt"].ToString().Trim());
                    HtmlTableRow row5 = new HtmlTableRow();
                    row5.Align = "left";
                    row5.VAlign = "top";
                    HtmlTableCell cell7 = new HtmlTableCell();
                    cell7.Align = "center";
                    cell7.Style.Add("width", "10%");
                    cell7.InnerText = num1.ToString().Trim();
                    row5.Cells.Add(cell7);
                    HtmlTableCell cell8 = new HtmlTableCell();
                    cell8.Style.Add("width", "50%");
                    cell8.InnerText = row4["AcctsHead"].ToString().Trim();
                    row5.Cells.Add(cell8);
                    HtmlTableCell cell9 = new HtmlTableCell();
                    cell9.Style.Add("width", "20%");
                    DropDownList dropDownList = new DropDownList();
                    dropDownList.ID = "txtDebit_" + num1.ToString().Trim();
                    dropDownList.Items.Insert(0, new ListItem("Debit", "DR"));
                    dropDownList.Items.Insert(1, new ListItem("Credit", "CR"));
                    dropDownList.Width = new Unit(60);
                    dropDownList.SelectedValue = str2;
                    cell9.Controls.Add((Control)dropDownList);
                    HiddenField hiddenField1 = new HiddenField();
                    hiddenField1.ID = "htAccountDebit_" + num1.ToString().Trim();
                    hiddenField1.Value = row4["AcctsHeadId"].ToString().Trim();
                    cell9.Controls.Add((Control)hiddenField1);
                    row5.Cells.Add(cell9);
                    HtmlTableCell cell10 = new HtmlTableCell();
                    cell10.Style.Add("width", "20%");
                    TextBox textBox = new TextBox();
                    textBox.ID = "txtCredit_" + num1.ToString().Trim();
                    textBox.MaxLength = 15;
                    textBox.Attributes.Add("onkeypress", "return blockNonNumbers(this, event, true, false);");
                    textBox.Attributes.Add("onkeyup", "extractNumber(this,2,false);");
                    textBox.Width = new Unit(100);
                    textBox.Text = num2.ToString().Trim();
                    cell10.Controls.Add((Control)textBox);
                    HiddenField hiddenField2 = new HiddenField();
                    hiddenField2.ID = "htAccountCredit_" + num1.ToString().Trim();
                    hiddenField2.Value = row4["AcctsHeadId"].ToString().Trim();
                    cell10.Controls.Add((Control)hiddenField2);
                    row5.Cells.Add(cell10);
                    ++num1;
                    htmlTable.Rows.Add(row5);
                }
                hidAccountCount.Value = num1.ToString().Trim();
                phControls.Controls.Add((Control)htmlTable);
            }
            else
            {
                calYearStartDate.SetDateValue(new DateTime(DateTime.Now.Year, 4, 1));
                calYearEndDate.SetDateValue(new DateTime(DateTime.Now.Year + 1, 3, 31));
            }
        }
        else
            FillAccountCodesDynamically("");
    }

    private void FillAccountCodesDynamically(string yearId)
    {
        clsDAL clsDal1 = new clsDAL();
        DataTable dataTable = new DataTable();
        try
        {
            if (yearId != "")
            {
                string str = "Select convert(char(10),StartDate,103) as startDate ,convert(char(10),EndDate,103) as endDate from ACTS_FinancialYear where FY_Id=" + yearId.ToString().Trim();
                clsDAL clsDal2 = new clsDAL();
                dataTable = new DataTable();
                DataTable dataTableQry = clsDal2.GetDataTableQry(str);
                if (dataTableQry.Rows.Count > 0)
                {
                    char[] chArray = new char[1] { '/' };
                    string[] strArray1 = dataTableQry.Rows[0][0].ToString().Trim().Split(chArray);
                    string[] strArray2 = dataTableQry.Rows[0][1].ToString().Trim().Split(chArray);
                    calYearStartDate.SetDateValue(new DateTime(Convert.ToInt32(strArray1[2].ToString().Trim()), Convert.ToInt32(strArray1[1].ToString().Trim()), Convert.ToInt32(strArray1[0].ToString().Trim())));
                    calYearEndDate.SetDateValue(new DateTime(Convert.ToInt32(strArray2[2].ToString().Trim()), Convert.ToInt32(strArray2[1].ToString().Trim()), Convert.ToInt32(strArray2[0].ToString().Trim())));
                }
                else
                {
                    calYearStartDate.SetDateValue(new DateTime(DateTime.Now.Year, 4, 1));
                    calYearEndDate.SetDateValue(new DateTime(DateTime.Now.Year + 1, 3, 31));
                }
            }
            else if (txtYearStartDate.Text == "" && txtYearEndDate.Text == "")
            {
                DateTime dateTime = Convert.ToDateTime(DateTime.Now.Year.ToString() + "/Apr/1");
                if (clsDal1.GetDataTableQry("select count(*) as count from ACTS_FinancialYear where StartDate='" + dateTime + "'").Rows[0]["count"].ToString() == "0")
                {
                    calYearStartDate.SetDateValue(new DateTime(DateTime.Now.Year, 4, 1));
                    calYearEndDate.SetDateValue(new DateTime(DateTime.Now.Year + 1, 3, 31));
                }
            }
            clsDAL clsDal3 = new clsDAL();
            DataTable dataTableQry1 = clsDal3.GetDataTableQry("ACTS_GetInitAcHeads");
            StringBuilder stringBuilder = new StringBuilder();
            if (dataTableQry1.Rows.Count <= 0)
                return;
            int num1 = 1;
            HtmlTable htmlTable = new HtmlTable();
            htmlTable.CellPadding = 0;
            htmlTable.CellSpacing = 0;
            htmlTable.Width = "600px";
            htmlTable.Border = 1;
            htmlTable.Attributes.Add("class", "innertbltxt");
            HtmlTableRow row1 = new HtmlTableRow();
            HtmlTableCell cell1 = new HtmlTableCell();
            cell1.Align = "left";
            cell1.VAlign = "top";
            cell1.ColSpan = 4;
            cell1.Attributes.Add("class", "pageSectionLabel");
            cell1.InnerHtml = "List Of Accounts Heads<hr />";
            row1.Cells.Add(cell1);
            htmlTable.Rows.Add(row1);
            HtmlTableRow row2 = new HtmlTableRow();
            HtmlTableCell cell2 = new HtmlTableCell();
            cell2.Style.Add("height", "10px");
            cell2.ColSpan = 4;
            row2.Cells.Add(cell2);
            htmlTable.Rows.Add(row2);
            htmlTable.Rows.Add(row2);
            HtmlTableRow row3 = new HtmlTableRow();
            row3.Align = "left";
            row3.VAlign = "top";
            row3.Attributes.Add("class", "pageLabel");
            HtmlTableCell cell3 = new HtmlTableCell();
            cell3.Align = "center";
            cell3.Style.Add("width", "10%");
            cell3.InnerText = "S.No.";
            row3.Cells.Add(cell3);
            row3.Style.Add("text-decoration", "underline");
            HtmlTableCell cell4 = new HtmlTableCell();
            cell4.Style.Add("width", "50%");
            cell4.InnerText = "Account Head";
            row3.Cells.Add(cell4);
            HtmlTableCell cell5 = new HtmlTableCell();
            cell5.Style.Add("width", "20%");
            cell5.InnerText = "Debit/Credit";
            row3.Cells.Add(cell5);
            HtmlTableCell cell6 = new HtmlTableCell();
            cell6.Style.Add("width", "20%");
            cell6.InnerText = "Amount";
            row3.Cells.Add(cell6);
            htmlTable.Rows.Add(row3);
            htmlTable.Rows.Add(row2);
            foreach (DataRow row4 in (InternalDataCollectionBase)dataTableQry1.Rows)
            {
                double num2 = 0.0;
                string str1 = "";
                if (yearId != "")
                {
                    string str2 = clsDal3.ExecuteScalarQry("SELECT FY FROM dbo.ACTS_FinancialYear WHERE FY_Id=" + yearId);
                    string str3 = "Select * from  ACTS_GenLedger where TransType = 'I' and AccountHead=" + row4["AcctsHeadId"].ToString().Trim() + "and SessionYr = '" + str2.ToString().Trim() + "' order by TransType";
                    clsDal3 = new clsDAL();
                    dataTable = new DataTable();
                    DataTable dataTableQry2 = clsDal3.GetDataTableQry(str3);
                    if (dataTableQry2.Rows.Count > 0)
                    {
                        str1 = dataTableQry2.Rows[0]["CrDr"].ToString().Trim();
                        num2 = Convert.ToDouble(dataTableQry2.Rows[0]["TransAmt"].ToString().Trim());
                    }
                }
                HtmlTableRow row5 = new HtmlTableRow();
                row5.Align = "left";
                row5.VAlign = "top";
                HtmlTableCell cell7 = new HtmlTableCell();
                cell7.Align = "center";
                cell7.Style.Add("width", "10%");
                cell7.InnerText = num1.ToString().Trim();
                row5.Cells.Add(cell7);
                HtmlTableCell cell8 = new HtmlTableCell();
                cell8.Style.Add("width", "50%");
                cell8.InnerText = row4["AcctsHead"].ToString().Trim();
                row5.Cells.Add(cell8);
                HtmlTableCell cell9 = new HtmlTableCell();
                cell9.Style.Add("width", "20%");
                DropDownList dropDownList = new DropDownList();
                dropDownList.ID = "txtDebit_" + num1.ToString().Trim();
                dropDownList.Items.Insert(0, new ListItem("Debit", "DR"));
                dropDownList.Items.Insert(1, new ListItem("Credit", "CR"));
                dropDownList.Width = new Unit(60);
                dropDownList.SelectedValue = str1;
                cell9.Controls.Add((Control)dropDownList);
                HiddenField hiddenField1 = new HiddenField();
                hiddenField1.ID = "htAccountDebit_" + num1.ToString().Trim();
                hiddenField1.Value = row4["AcctsHeadId"].ToString().Trim();
                cell9.Controls.Add((Control)hiddenField1);
                row5.Cells.Add(cell9);
                HtmlTableCell cell10 = new HtmlTableCell();
                cell10.Style.Add("width", "20%");
                TextBox textBox = new TextBox();
                textBox.ID = "txtCredit_" + num1.ToString().Trim();
                textBox.MaxLength = 15;
                textBox.Attributes.Add("onkeypress", "return blockNonNumbers(this, event, true, false);");
                textBox.Attributes.Add("onkeyup", "extractNumber(this,2,false);");
                textBox.Width = new Unit(100);
                textBox.Text = num2.ToString().Trim();
                cell10.Controls.Add((Control)textBox);
                HiddenField hiddenField2 = new HiddenField();
                hiddenField2.ID = "htAccountCredit_" + num1.ToString().Trim();
                hiddenField2.Value = row4["AcctsHeadId"].ToString().Trim();
                cell10.Controls.Add((Control)hiddenField2);
                row5.Cells.Add(cell10);
                ++num1;
                htmlTable.Rows.Add(row5);
            }
            hidAccountCount.Value = num1.ToString().Trim();
            phControls.Controls.Add((Control)htmlTable);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    protected void btnInitialYear_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            clsDAL clsDal1 = new clsDAL();
            ArrayList typeList = new ArrayList();
            ArrayList AccountHeadList = new ArrayList();
            ArrayList Amountlist = new ArrayList();
            ArrayList arrayList1 = new ArrayList();
            ArrayList arrayList2 = new ArrayList();
            clsDAL clsDal2 = new clsDAL();
            double num1 = 0.0;
            double num2 = 0.0;
            double num3 = 0.0;
            string financialYearId = "";
            try
            {
                if (Request.QueryString["id"] != null)
                    financialYearId = Request.QueryString["id"].ToString().Trim();
                if (clsDal1.GetDataTableQry("select count(*) as count from ACTS_FinancialYear").Rows[0]["count"].ToString() == "0")
                {
                    if (!(calYearEndDate.GetDateValue().Day.ToString() == "31") || !(calYearEndDate.GetDateValue().Month.ToString() == "3") || !(calYearEndDate.GetDateValue().Year.ToString() == Convert.ToDateTime(txtYearStartDate.Text).AddYears(1).Year.ToString()))
                    {
                        lblMsg.Text = "Selected Financial Year is invalid";
                        lblMsg.ForeColor = Color.Red;
                        lblBottomMsg.Text = "Selected Financial Year is invalid";
                        lblBottomMsg.ForeColor = Color.Red;
                        return;
                    }
                }
                else
                {
                    TimeSpan timeSpan = calYearEndDate.GetDateValue().Subtract(calYearStartDate.GetDateValue());
                    if (!(calYearStartDate.GetDateValue().Day.ToString() == "1") || !(calYearStartDate.GetDateValue().Month.ToString() == "4") || (!(calYearEndDate.GetDateValue().Day.ToString() == "31") || !(calYearEndDate.GetDateValue().Month.ToString() == "3")))
                    {
                        lblMsg.Text = "Selected Financial Year is invalid";
                        lblMsg.ForeColor = Color.Red;
                        lblBottomMsg.Text = "Selected Financial Year is invalid";
                        lblBottomMsg.ForeColor = Color.Red;
                        return;
                    }
                    if (timeSpan.TotalDays + 1.0 > 366.0 || timeSpan.TotalDays + 1.0 < 365.0)
                    {
                        lblMsg.Text = "Selected Financial Year is invalid";
                        lblMsg.ForeColor = Color.Red;
                        lblBottomMsg.Text = "Selected Financial Year is invalid";
                        lblBottomMsg.ForeColor = Color.Red;
                        return;
                    }
                }
                foreach (Control control1 in phControls.Controls)
                {
                    foreach (Control control2 in control1.Controls)
                    {
                        foreach (Control control3 in control2.Controls)
                        {
                            foreach (Control control4 in control3.Controls)
                            {
                                TextBox textBox = new TextBox();
                                DropDownList dropDownList = new DropDownList();
                                if (control4.GetType() == dropDownList.GetType())
                                {
                                    if (((ListControl)control4).SelectedValue == "DR")
                                        Session["amtType"] = "DR";
                                    else if (((ListControl)control4).SelectedValue == "CR")
                                        Session["amtType"] = "CR";
                                }
                                if (control4.GetType() == textBox.GetType() && control4.GetType() == textBox.GetType())
                                {
                                    switch (Session["amtType"].ToString().Trim().ToUpper())
                                    {
                                        case "DR":
                                            num1 += Convert.ToDouble(((TextBox)control4).Text.ToString().Trim());
                                            continue;
                                        case "CR":
                                            num2 += Convert.ToDouble(((TextBox)control4).Text.ToString().Trim());
                                            continue;
                                        default:
                                            continue;
                                    }
                                }
                            }
                        }
                    }
                }
                lblDebitAmount.Text = num1.ToString().Trim();
                lblCreditAmount.Text = num2.ToString().Trim();
                txtClosingBal.Text = (double.Parse(num1.ToString().Trim()) - double.Parse(num2.ToString().Trim())).ToString();
                foreach (Control control1 in phControls.Controls)
                {
                    foreach (Control control2 in control1.Controls)
                    {
                        foreach (Control control3 in control2.Controls)
                        {
                            foreach (Control control4 in control3.Controls)
                            {
                                TextBox textBox = new TextBox();
                                HiddenField hiddenField = new HiddenField();
                                DropDownList dropDownList = new DropDownList();
                                if (control4.GetType() == dropDownList.GetType())
                                    typeList.Add(((ListControl)control4).SelectedValue);
                                if (control4.GetType() == textBox.GetType())
                                {
                                    double num4 = !(((TextBox)control4).Text == "") ? Convert.ToDouble(((TextBox)control4).Text.ToString()) : 0.0;
                                    Amountlist.Add(num4.ToString());
                                    num3 += num4;
                                }
                                if (control4.GetType() == hiddenField.GetType() && control4.ID.Contains("Debit"))
                                    AccountHeadList.Add(((HiddenField)control4).Value.ToString().Trim());
                            }
                        }
                    }
                    InsertData(typeList, AccountHeadList, Amountlist, financialYearId);
                    txtYearStartDate.Enabled = false;
                    txtYearEndDate.Enabled = false;
                    calYearStartDate.Enabled = false;
                    calYearEndDate.Enabled = false;
                }
                if (Request.QueryString["id"] == null)
                {
                   if (clsDal1.ExecuteScalar("Acts_InsUpdtStockFY", new Hashtable()
                          {
                            {
                               "@StDt",
                               calYearStartDate.GetDateValue().ToString("dd MMM yyyy")
                            },
                            {
                               "@EndDt",
                               calYearEndDate.GetDateValue().ToString("dd MMM yyyy")
                            },
                            {
                               "@UserId",
                              Session["User_Id"]
                            },
                            {
                               "@SchoolId",
                              Session["SchoolId"]
                            }
                          }).Trim() != "")
                    {
                        lblMsg.Text = "Unable to Initialize Stock For New Financial Year";
                        lblMsg.ForeColor = Color.Red;
                        lblBottomMsg.Text = "Unable to Initialize Stock For New Financial Year";
                        lblBottomMsg.ForeColor = Color.Red;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        clear = "";
    }
        private void InsertData(ArrayList typeList, ArrayList AccountHeadList, ArrayList Amountlist, string financialYearId)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        DateTime dateTime = Convert.ToDateTime(calYearStartDate.GetDateValue().ToString("dd MMM yyyy"));
        int month = dateTime.Month;
        int year1 = dateTime.Year;
        string year2 = month <= 3 ? Convert.ToString(year1 - 1) + "-" + year1.ToString().Substring(2, 2) : year1.ToString() + "-" + Convert.ToString(year1 + 1).Substring(2, 2);
        if (Request.QueryString["id"] != null)
            hashtable.Add("@FY_Id", financialYearId);
        hashtable.Add("@FY", year2);
        hashtable.Add("@StartDate", calYearStartDate.GetDateValue().ToString("dd MMM yyyy"));
        hashtable.Add("@EndDate", calYearEndDate.GetDateValue().ToString("dd MMM yyyy"));
        hashtable.Add("@ClosingBal", txtClosingBal.Text.ToString().Trim());
        hashtable.Add("@UserId", Session["User_Id"]);
        hashtable.Add("@SchoolId", Session["SchoolId"]);
        if ((Request.QueryString["id"] != null ? clsDal.GetDataTable("ACTS_UpdateFinancialYr", hashtable) : clsDal.GetDataTable("ACTS_InsertFinancialYr", hashtable)).Rows.Count > 0)
        {
            lblMsg.Text = "Selected Financial Year Exists";
            lblMsg.ForeColor = Color.Red;
            lblBottomMsg.Text = "Selected Financial Year Exists";
            lblBottomMsg.ForeColor = Color.Red;
        }
        else
        {
            InsertGenLedger(typeList, AccountHeadList, Amountlist, year2);
            lblBottomMsg.Text = "Record Saved Successfully";
            lblBottomMsg.ForeColor = Color.Green;
            lblMsg.Text = "Record Saved Successfully";
            lblMsg.ForeColor = Color.Green;
        }
    }

    private void InsertGenLedger(ArrayList typeList, ArrayList AccountHeadList, ArrayList Amountlist, string year)
    {
        clsDAL clsDal1 = new clsDAL();
        DataTable dataTable1 = new DataTable();
        Hashtable hashtable = new Hashtable();
        for (int index = 0; index < AccountHeadList.Count; ++index)
        {
            clsDal1 = new clsDAL();
            clsDAL clsDal2 = new clsDAL();
            DataTable dataTable2 = new DataTable();
            clsDal2.GetDataTable("ACTS_InsertGenLedger", new Hashtable()
      {
        {
           "@FY",
           year
        },
        {
           "@TransDate",
           calYearStartDate.GetDateValue().ToString("dd MMM yyyy")
        },
        {
           "@AccountHead",
           AccountHeadList[index].ToString().Trim()
        },
        {
           "@Particulars",
           "Accounts Initialization"
        },
        {
           "@TransType",
           "I"
        },
        {
           "@CrDr",
           typeList[index].ToString().Trim()
        },
        {
           "@TransAmt",
           Amountlist[index].ToString().Trim()
        },
        {
           "@UserId",
          Session["User_Id"]
        },
        {
           "@SchoolId",
          Session["SchoolId"]
        }
      });
        }
    }

    private void CalculateAmount()
    {
        double num1 = 0.0;
        double num2 = 0.0;
        foreach (Control control1 in phControls.Controls)
        {
            foreach (Control control2 in control1.Controls)
            {
                foreach (Control control3 in control2.Controls)
                {
                    foreach (Control control4 in control3.Controls)
                    {
                        TextBox textBox = new TextBox();
                        DropDownList dropDownList = new DropDownList();
                        if (control4.GetType() == dropDownList.GetType())
                        {
                            if (((ListControl)control4).SelectedValue == "DR")
                                Session["amtType"] = "DR";
                            else if (((ListControl)control4).SelectedValue == "CR")
                                Session["amtType"] = "CR";
                        }
                        if (control4.GetType() == textBox.GetType() && control4.GetType() == textBox.GetType())
                        {
                            switch (Session["amtType"].ToString().Trim().ToUpper())
                            {
                                case "DR":
                                    num1 += Convert.ToDouble(((TextBox)control4).Text.ToString().Trim());
                                    continue;
                                case "CR":
                                    num2 += Convert.ToDouble(((TextBox)control4).Text.ToString().Trim());
                                    continue;
                                default:
                                    continue;
                            }
                        }
                    }
                }
            }
        }
        lblDebitAmount.Text = num1.ToString().Trim();
        lblCreditAmount.Text = num2.ToString().Trim();
        txtClosingBal.Text = (double.Parse(num1.ToString().Trim()) - double.Parse(num2.ToString().Trim())).ToString();
    }

    protected void btnCalculateTotals_Click(object sender, EventArgs e)
    {
        CalculateAmount();
    }

    protected void btnGoToList_Click(object sender, EventArgs e)
    {
        Response.Redirect("OpenFYList.aspx");
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        txtYearEndDate.Text = string.Empty;
        txtYearStartDate.Text = string.Empty;
        txtClosingBal.Text = string.Empty;
        clear = "Yes";
        lblCreditAmount.Text = "0.00";
        lblDebitAmount.Text = "0.00";
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Welcome.aspx");
    }
}