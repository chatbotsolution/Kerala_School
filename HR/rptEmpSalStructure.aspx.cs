using ASP;
using SanLib;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HR_rptEmpSalStructure : System.Web.UI.Page
{
    private clsDAL objDAL = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Session["rptSalStr"] = null;
        bindDropDown(drpDesgn, "SELECT DesgId, Designation FROM dbo.HR_DesignationMaster ORDER BY Designation", "Designation", "DesgId");
        bindDropDown(drpEmp, "SELECT EmpId,SevName as EmpName FROM dbo.HR_EmployeeMaster Where ActiveStatus=1 ORDER BY EmpName", "EmpName", "EmpId");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = objDAL.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- ALL -", "0"));
    }

    protected void GenerateReport()
    {
        int num1 = 8;
        Decimal num2 = new Decimal(0);
        Decimal num3 = new Decimal(0);
        Decimal num4 = new Decimal(0);
        Decimal num5 = new Decimal(0);
        Decimal num6 = new Decimal(0);
        DataTable dataTable = new DataTable();
        StringBuilder stringBuilder1 = new StringBuilder();
        stringBuilder1.Append("SELECT * FROM HR_view_GetCurrentSalStructure WHERE 1=1");
        if (drpDesgn.SelectedIndex > 0)
            stringBuilder1.Append(" AND DesignationId=" + drpDesgn.SelectedValue);
        if (drpEmp.SelectedIndex > 0)
            stringBuilder1.Append(" AND EmpId=" + drpEmp.SelectedValue.ToString());
        stringBuilder1.Append(" ORDER BY DesignationId");
        DataTable dataTableQry1 = objDAL.GetDataTableQry(stringBuilder1.ToString());
        if (dataTableQry1.Rows.Count > 0)
        {
            StringBuilder stringBuilder2 = new StringBuilder();
            string str1 = Information();
            stringBuilder2.Append("<table width='100%' cellpadding='2' cellspacing='0'>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td style='font-size: 14px; background-color: Silver;' align='center'><b>" + str1 + "</b></td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td style='font-size: 14px; background-color: Silver;' align='center'>");
            stringBuilder2.Append("<b>CURRENT SALARY STRUCTURE</b>");
            stringBuilder2.Append("</td>");
            stringBuilder2.Append("</tr>");
            stringBuilder2.Append("</table>");
            stringBuilder2.Append("<div style='height:5px;'></div>");
            stringBuilder2.Append("<table width='100%' cellpadding='2' cellspacing='0' style='border:solid 1px;border-bottom:none 1px;'>");
            stringBuilder2.Append("<tr>");
            stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;' width='10px'><b>S.No.</b></td>");
            stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;' width='150px'><b>Name</b></td>");
            stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;' width='70px'><b>Date Of Joining</b></td>");
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='50px'><b>Basic</b></td>");
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='50px'><b>D.A</b></td>");
            DataTable dataTableQry2 = objDAL.GetDataTableQry("SELECT DISTINCT AllowanceId,Allowance FROM HR_view_GetCurrentAllwStructure ORDER BY AllowanceId ASC");
            if (dataTableQry2.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry2.Rows)
                {
                    stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='50px'><b>" + row["Allowance"].ToString() + "</b></td>");
                    ViewState["totAllw" + row["Allowance"]] = 0;
                    ++num1;
                }
            }
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='50px'><b>Gross Total</b></td>");
            DataTable dataTableQry3 = objDAL.GetDataTableQry("SELECT DISTINCT DedTypeId,DedDetails FROM HR_view_GetCurrentDedStructure ORDER BY DedTypeId ASC");
            if (dataTableQry3.Rows.Count > 0)
            {
                foreach (DataRow row in (InternalDataCollectionBase)dataTableQry3.Rows)
                {
                    stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='50px'><b>" + row["DedDetails"].ToString() + "</b></td>");
                    ViewState["totDed" + row["DedDetails"]] = 0;
                    ++num1;
                }
            }
            stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;' width='50px'><b>Total Ded.</b></td>");
            stringBuilder2.Append("<td align='right' style='border-bottom:solid 1px;' width='50px'><b>Net Payment</b></td>");
            stringBuilder2.Append("</tr>");
            int num7 = 1;
            foreach (DataRow row1 in (InternalDataCollectionBase)dataTableQry1.Rows)
            {
                Decimal num8 = new Decimal(0);
                stringBuilder2.Append("<tr>");
                stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;'>" + num7 + "</td>");
                stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;'>" + row1["EmpName"] + " (" + row1["Designation"] + ")</td>");
                stringBuilder2.Append("<td align='left' style='border-right:solid 1px;border-bottom:solid 1px;'>" + row1["DOJ"] + "</td>");
                stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(row1["Pay"]).ToString("0.00") + "</td>");
                stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(row1["DA"]).ToString("0.00") + "</td>");
                foreach (DataRow row2 in (InternalDataCollectionBase)dataTableQry2.Rows)
                {
                    string str2 = objDAL.ExecuteScalarQry("SELECT Amount FROM HR_view_GetCurrentAllwStructure  WHERE EmpId=" + row1["EmpId"].ToString() + " AND Allowance='" + row2["Allowance"] + "' ORDER BY AllowanceId ASC");
                    if (str2.Trim() != string.Empty)
                    {
                        stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(str2).ToString("0.00") + "</td>");
                    }
                    else
                    {
                        str2 = "0";
                        stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                    }
                    ViewState["totAllw" + row2["Allowance"]] = (Convert.ToDecimal(ViewState["totAllw" + row2["Allowance"]]) + Convert.ToDecimal(str2));
                }
                stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(row1["GrossTot"]).ToString("0.00") + "</td>");
                Decimal num9 = new Decimal(0);
                foreach (DataRow row2 in (InternalDataCollectionBase)dataTableQry3.Rows)
                {
                    string str2 = objDAL.ExecuteScalarQry("SELECT AmtFromEmp FROM HR_view_GetCurrentDedStructure WHERE EmpId=" + row1["EmpId"].ToString() + " AND DedDetails='" + row2["DedDetails"] + "' AND AmtFromEmp>0 ORDER BY DedTypeId ASC");
                    if (str2.Trim() != string.Empty)
                    {
                        stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + Convert.ToDecimal(str2).ToString("0.00") + "</td>");
                        num9 += Convert.ToDecimal(str2);
                    }
                    else
                    {
                        str2 = "0";
                        stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>0.00</td>");
                    }
                    ViewState["totDed" + row2["DedDetails"]] = (Convert.ToDecimal(ViewState["totDed" + row2["DedDetails"]]) + Convert.ToDecimal(str2));
                }
                Decimal num10 = Convert.ToDecimal(row1["GrossTot"].ToString()) - num9;
                stringBuilder2.Append("<td align='right' style='border-right:solid 1px;border-bottom:solid 1px;'>" + num9.ToString("0.00") + "</td>");
                stringBuilder2.Append("<td align='right' style='border-bottom:solid 1px;'>" + num10.ToString("0.00") + "</td>");
                stringBuilder2.Append("</tr>");
                num2 += Convert.ToDecimal(row1["Pay"]);
                num3 += Convert.ToDecimal(row1["DA"]);
                num4 += Convert.ToDecimal(row1["GrossTot"]);
                num6 += num9;
                num5 += num10;
                ++num7;
            }
            stringBuilder2.Append("</table>");
            lblReport.Text = stringBuilder2.ToString();
            Session["rptSalStr"] = finalReport(stringBuilder2.ToString());
        }
        else
        {
            lblReport.Font.Bold = true;
            lblReport.Text = "Salary Structure not Defined for the Selected Employee";
            Session["rptSalStr"] = null;
        }
    }

    private string Information()
    {
        DataTable dataTable1 = new DataTable();
        DataSet dataSet = new DataSet();
        StringBuilder stringBuilder = new StringBuilder();
        string str1 = Server.MapPath("../XMLFiles") + "\\Detail.xml";
        if (File.Exists(str1))
        {
            int num = (int)dataSet.ReadXml(str1);
            string str2 = Session["SSVMID"].ToString();
            if (dataSet.Tables.Count > 0)
            {
                DataTable dataTable2 = new DataTable();
                DataTable table = dataSet.Tables[0];
                foreach (DataRow row in (InternalDataCollectionBase)dataSet.Tables[0].Rows)
                {
                    stringBuilder.Append(objDAL.Decrypt(row["SchoolName"].ToString().Trim(), str2).ToUpper() + ", ");
                    stringBuilder.Append(objDAL.Decrypt(row["Address1"].ToString().Trim(), str2).ToUpper() + ", " + objDAL.Decrypt(row["Address2"].ToString().Trim(), str2).ToUpper());
                }
            }
        }
        return stringBuilder.ToString();
    }

    protected string finalReport(string strRpt)
    {
        return strRpt;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        GenerateReport();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        if (Session["rptSalStr"] != null)
            Response.Redirect("rptEmpSalStructurePrint.aspx");
        else
            ScriptManager.RegisterClientScriptBlock((Control)btnPrint, btnPrint.GetType(), "ShowMessage", "alert('No Data Exists to Print');", true);
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            if (Session["rptSalStr"] != null)
                ExportToExcel(Session["rptSalStr"].ToString());
            else
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('No Data Exists to Export');", true);
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
            string str = Server.MapPath("Exported_Files/" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + str);
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpDesgn_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpDesgn.SelectedIndex > 0)
            bindDropDown(drpEmp, "SELECT EmpId, SevName as EmpName FROM dbo.HR_EmployeeMaster WHERE DesignationId=" + drpDesgn.SelectedValue.ToString() + " ORDER BY EmpName", "EmpName", "EmpId");
        else
            bindDropDown(drpEmp, "SELECT EmpId, SevName as EmpName FROM dbo.HR_EmployeeMaster ORDER BY EmpName", "EmpName", "EmpId");
    }
}