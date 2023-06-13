using AjaxControlToolkit;
using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_EmpInfoDtls : System.Web.UI.Page
{
    private DataTable dt;
    private clsDAL ObjCommon;
    private Hashtable ht;
    private int RecCount;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Session["User_Id"] == null)
            this.Response.Redirect("../Login.aspx");
        if (this.Page.IsPostBack)
            return;
        this.ViewState["ssvmid"] = (object)string.Empty;
        this.Session["title"] = (object)this.Page.Title.ToString();
        if (this.Request.QueryString["empno"] != null)
        {
            if (this.Request.QueryString["empno"].ToString().Equals("0"))
                this.Response.Redirect("../Login.aspx");
            this.ObjCommon = new clsDAL();
            this.dt = new DataTable();
            this.dt = this.ObjCommon.GetDataTableQry("select SevName,SevabratiId from HR_EmployeeMaster where EmpId=" + this.Request.QueryString["empno"]);
            this.lblEmpName.Text = this.dt.Rows[0]["SevName"].ToString() + "(" + this.dt.Rows[0]["SevabratiId"].ToString() + ")";
            this.FillEmpDataList();
            GridView control = (GridView)((Control)this.APDakshina).FindControl("grdDakshinaDetails");
            this.SalaryReport();
            this.chkFrmPage();
        }
        else
            this.Response.Write("<script>history.back();</script>");
    }

    private void SalaryReport()
    {
        this.lblSalary.Text = "";
        Hashtable hashtable = new Hashtable();
        StringBuilder stringBuilder = new StringBuilder();
        Decimal num1 = new Decimal(0);
        if (this.Request.QueryString["empno"] != null)
            hashtable.Add((object)"@EmpId", (object)this.Request.QueryString["empno"]);
        DataSet dataSet1 = new DataSet();
        DataSet dataSet2 = this.ObjCommon.GetDataSet("HR_GetEmpSalryDed", hashtable);
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = new DataTable();
        DataTable dataTable3 = new DataTable();
        DataTable table = dataSet2.Tables[0];
        if (dataSet2.Tables.Count > 2)
        {
            dataTable2 = dataSet2.Tables[2];
            dataTable3 = dataSet2.Tables[1];
        }
        else if (dataSet2.Tables.Count > 1)
        {
            try
            {
                dataSet2.Tables[1].Rows[0]["AmtFromOrg"].ToString();
                dataTable3 = dataSet2.Tables[1];
            }
            catch
            {
                dataTable2 = dataSet2.Tables[1];
            }
        }
        if (table.Rows.Count <= 0 && dataTable3.Rows.Count <= 0)
            return;
        stringBuilder.Append("<table><tr>");
        if (table.Rows.Count > 0)
        {
            stringBuilder.Append("<td  colspan='2'>");
            stringBuilder.Append("<b>Applicable From &nbsp;" + table.Rows[0]["FromDtStr"] + "</b></td></tr>");
            stringBuilder.Append("<tr><td ><table>");
            stringBuilder.Append("<tr><td colspan='2'><u><b>Authorized Salary</b></u></td></tr>");
            stringBuilder.Append("<tr><td style='width:150px;'>Basic Pay</td>");
            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(table.Rows[0]["Pay"].ToString()).ToString("0.00") + "</td></tr>");
            Decimal num2 = num1 + Convert.ToDecimal(table.Rows[0]["Pay"]);
            stringBuilder.Append("<tr><td>Daily Allowance</td>");
            stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(table.Rows[0]["DA"].ToString()).ToString("0.00") + "</td></tr>");
            num1 = num2 + Convert.ToDecimal(table.Rows[0]["DA"]);
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
            {
                stringBuilder.Append("<tr><td>" + row["Allowance"].ToString().Trim() + "</td>");
                num1 += Convert.ToDecimal(row["Amount"].ToString().Trim());
                stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["Amount"].ToString().Trim()).ToString("0.00") + "</td></tr>");
            }
            stringBuilder.Append("</table></td>");
        }
        stringBuilder.Append("<td valign='top' style='width:100px;'></td>");
        Decimal num3 = new Decimal(0);
        if (dataTable3.Rows.Count > 0)
        {
            stringBuilder.Append("<td valign='top'><table>");
            stringBuilder.Append("<tr><td colspan='2' ><u><b>Deductions</b></u></td></tr>");
            foreach (DataRow row in (InternalDataCollectionBase)dataTable3.Rows)
            {
                stringBuilder.Append("<tr><td style='width:100px;'>" + row["DedDetails"].ToString() + "</td>");
                stringBuilder.Append("<td align='right'>" + Convert.ToDecimal(row["AmtFromEmp"].ToString()).ToString("0.00") + "</td></tr>");
                num3 += Convert.ToDecimal(row["AmtFromEmp"].ToString());
            }
            stringBuilder.Append("</table></td>");
        }
        stringBuilder.Append("<tr><td style='border-top:1px solid #333;'><b>Total</b>");
        stringBuilder.Append("&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;" + num1.ToString("0.00") + "</td>");
        stringBuilder.Append("<td valign='top' style='width:100px;'></td>");
        stringBuilder.Append("<td style='border-top:1px solid #333;' valign='bottom'><b>Total</b>");
        stringBuilder.Append("&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;" + num3.ToString("0.00") + "</td></tr>");
        Decimal num4 = num1 - num3;
        stringBuilder.Append("</tr><tr><td colspan='3'><hr/></td></tr>");
        stringBuilder.Append("</tr><tr><td colspan='2'><b>Net Payble Amount:&nbsp;</b>" + num4.ToString("0.00") + "</td></tr></table>");
        this.lblSalary.Text = stringBuilder.ToString();
    }

    private void FillEmpDataList()
    {
        this.ht = new Hashtable();
        this.dt = new DataTable();
        this.ObjCommon = new clsDAL();
        if (this.Request.QueryString["empno"] != null)
            this.ht.Add((object)"@EmpId", (object)this.Request.QueryString["empno"]);
        this.dt = this.ObjCommon.GetDataTable("HR_SelEmployeeForDL", this.ht);
        DataList control = (DataList)((Control)this.APEmpDet).FindControl("DLEmployee");
        control.DataSource = (object)this.dt;
        control.DataBind();
    }

    private void FillGrid(GridView gv, string sp, Label lbl)
    {
        this.ht = new Hashtable();
        this.dt = new DataTable();
        this.ObjCommon = new clsDAL();
        if (this.Request.QueryString["empno"] != null)
            this.ht.Add((object)"@EmpId", (object)this.Request.QueryString["empno"]);
        this.dt = this.ObjCommon.GetDataTable(sp, this.ht);
        gv.DataSource = (object)this.dt;
        gv.DataBind();
        lbl.Text = "Total Records : " + this.dt.Rows.Count.ToString();
    }

    protected void btnAddNewApp_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("EmpPastAppmtDtls.aspx?em=" + this.Request.QueryString["empno"] + "&d=1");
    }

    protected void btnAddNewTraining_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("EmpTrainingDetails.aspx?em=" + this.Request.QueryString["empno"] + "&d=1");
    }

    protected void btnAddNewDakshina_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("EmployeeSalaryStructure.aspx?em=" + this.Request.QueryString["empno"] + "&d=1");
    }

    private int DeleteRecord(string id, string key, string StoredProc)
    {
        this.ht = new Hashtable();
        this.dt = new DataTable();
        this.ObjCommon = new clsDAL();
        this.ht.Add((object)key, (object)id);
        this.dt = this.ObjCommon.GetDataTable(StoredProc, this.ht);
        return this.dt.Rows.Count > 0 ? 1 : 0;
    }

    protected void btnEditEmp_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("EmpOld.aspx?empno=" + this.Request.QueryString["empno"].ToString());
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("EmpOld.aspx");
    }

    private void chkFrmPage()
    {
        if (this.Request.QueryString["a"] != null)
            this.Accordion1.SelectedIndex =1;
        if (this.Request.QueryString["b"] != null)
            this.Accordion1.SelectedIndex =2;
        if (this.Request.QueryString["c"] != null)
            this.Accordion1.SelectedIndex =3;
        if (this.Request.QueryString["d"] != null)
            this.Accordion1.SelectedIndex =4;
        if (this.Request.QueryString["e"] == null)
            return;
        this.Accordion1.SelectedIndex =5;
    }

    public void MergeCell(GridView gv, int[] colIndices)
    {
        foreach (int colIndex in colIndices)
        {
            for (int index = gv.Rows.Count - 2; index >= 0; --index)
            {
                GridViewRow row1 = gv.Rows[index];
                GridViewRow row2 = gv.Rows[index + 1];
                if (row1.Cells[colIndex].Text == row2.Cells[colIndex].Text)
                {
                    row1.Cells[colIndex].RowSpan = row2.Cells[colIndex].RowSpan < 2 ? 2 : row2.Cells[colIndex].RowSpan + 1;
                    row2.Cells[colIndex].Visible = false;
                }
            }
        }
    }

    protected void DLEmployee_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        GridView control = (GridView)e.Item.FindControl("grdEduQualApp");
        this.ht = new Hashtable();
        this.dt = new DataTable();
        this.ObjCommon = new clsDAL();
        if (this.Request.QueryString["empno"] != null)
            this.ht.Add((object)"@EmpId", (object)this.Request.QueryString["empno"]);
        this.dt = this.ObjCommon.GetDataTable("HR_EmpQualDetail", this.ht);
        control.DataSource = (object)this.dt;
        control.DataBind();
    }

    protected void grdEduQualApp_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        ((GridView)sender).PageIndex = e.NewPageIndex;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        this.Response.Redirect("EmpListSSVM.aspx");
    }
}