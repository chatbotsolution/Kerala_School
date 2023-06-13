using AjaxControlToolkit;
using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Reports_rptAdmnSum : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
  {
    if (Session["User_Id"] == null)
      Response.Redirect("../Login.aspx");
    if (Page.IsPostBack)
      return;
    fillclass();
    rbtnAll.Checked = true;
    btnExpExcel.Enabled = false;
  }

  protected void btnShow_Click(object sender, EventArgs e)
  {
    GetCatRpt();
  }

  private void fillclass()
  {
    drpClass.Items.Clear();
    dt = obj.GetDataTable("ps_sp_get_classesForDDL");
    drpClass.DataSource =  dt;
    drpClass.DataTextField = "classname";
    drpClass.DataValueField = "classid";
    drpClass.DataBind();
    drpClass.Items.Insert(0, new ListItem("All", "0"));
  }

  private void GetCatRpt()
  {
    DataTable dataTable1 = new DataTable();
    Hashtable hashtable = new Hashtable();
    DataTable dataTableQry = obj.GetDataTableQry("select distinct admnsessyr from dbo.PS_StudMaster order by admnsessyr desc");
    if (dataTableQry.Rows.Count > 0)
    {
      DataTable dataTable2 = new DataTable();
      DataTable dataTable3 = obj.GetDataTable("Ps_Sp_GetCatNames");
      foreach (DataRow row in (InternalDataCollectionBase) dataTable3.Rows)
        dataTableQry.Columns.Add(new DataColumn(row["CatId"].ToString(), Type.GetType("System.Double")));
      dataTableQry.Columns.Add(new DataColumn("Female", Type.GetType("System.Double")));
      dataTableQry.Columns.Add(new DataColumn("Male", Type.GetType("System.Double")));
      dataTableQry.AcceptChanges();
      StringBuilder report = CreateReport(mergertable(dataTableQry, 1), dataTable3, 1);
      lblReport.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='left'><b>&nbsp;Admn Session Year Wise(Admission Summary)</b></div>" + report.ToString().Trim();
      Session["AdmnSum"] =  report.ToString().Trim();
      btnExpExcel.Enabled = true;
    }
    else
    {
      lblReport.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='center'>No Records Found  !</div>";
      btnExpExcel.Enabled = false;
    }
  }

  private DataTable mergertable(DataTable tbstudent, int no)
  {
    Hashtable hashtable1 = new Hashtable();
    for (int index = 0; index < tbstudent.Rows.Count; ++index)
    {
      string str1 = tbstudent.Rows[index]["AdmnSessYr"].ToString();
      string str2 = drpClass.SelectedValue.ToString();
      DataSet dataSet1 = new DataSet();
      DataTable dataTable1 = new DataTable();
      DataTable dataTable2 = new DataTable();
      Hashtable hashtable2 = new Hashtable();
      hashtable2.Add( "@fy",  str1);
      if (drpClass.SelectedIndex > 0)
        hashtable2.Add( "@ClassId",  str2);
      if (no == 1)
      {
        if (rbtnAll.Checked)
        {
          DataSet dataSet2 = drpClass.SelectedIndex != 0 ? obj.GetDataSet("PS_SP_get_CountAllFy", hashtable2) : obj.GetDataSet("PS_SP_get_CountAll", hashtable2);
          dataTable1 = dataSet2.Tables[0];
          dataTable2 = dataSet2.Tables[1];
        }
        else if (rbtnExisting.Checked)
        {
          DataSet dataSet2 = drpClass.SelectedIndex != 0 ? obj.GetDataSet("PS_SP_get_CountOldStudFy", hashtable2) : obj.GetDataSet("PS_SP_get_CountOldStud", hashtable2);
          dataTable1 = dataSet2.Tables[0];
          dataTable2 = dataSet2.Tables[1];
        }
        else if (rbtnNew.Checked)
        {
          DataSet dataSet2 = drpClass.SelectedIndex != 0 ? obj.GetDataSet("PS_SP_get_CountNewStudFy", hashtable2) : obj.GetDataSet("PS_SP_get_CountNewStud", hashtable2);
          dataTable1 = dataSet2.Tables[0];
          dataTable2 = dataSet2.Tables[1];
        }
      }
      foreach (DataRow row in (InternalDataCollectionBase) dataTable1.Rows)
      {
        string str3 = row["CountCat"].ToString();
        Decimal num1 = new Decimal(0);
        Decimal num2 = Convert.ToDecimal(str3);
        tbstudent.Rows[index][row["CatId"].ToString()] =  num2.ToString();
      }
      foreach (DataRow row in (InternalDataCollectionBase) dataTable2.Rows)
      {
        string str3 = row["CountCat"].ToString();
        if (row["Sex"].ToString().ToUpper() == "MALE")
        {
          Decimal num1 = new Decimal(0);
          Decimal num2 = Convert.ToDecimal(str3);
          tbstudent.Rows[index][row["Sex"].ToString()] =  num2.ToString();
        }
        if (row["Sex"].ToString().ToUpper() == "FEMALE")
        {
          Decimal num1 = new Decimal(0);
          Decimal num2 = Convert.ToDecimal(str3);
          tbstudent.Rows[index][row["Sex"].ToString()] =  num2.ToString();
        }
      }
    }
    return tbstudent;
  }

  protected StringBuilder CreateReport(DataTable dt, DataTable tbcomp, int type)
  {
    StringBuilder stringBuilder1 = new StringBuilder();
    StringBuilder stringBuilder2 = new StringBuilder();
    Hashtable hashtable1 = new Hashtable();
    Hashtable hashtable2 = new Hashtable();
    int num1 = 0;
    int num2 = 1;
    if (dt.Rows.Count > 0)
    {
      stringBuilder1.Append("<table border='1px' cellpadding='1' cellspacing='0' style='background-color: #FFF; border-collapse:collapse;border-color:black;' class='tbltxt'  width='100%'>");
      stringBuilder1.Append("<tr style='background-color:#CCCCCC'>");
      stringBuilder1.Append("<td style='width: 30px' align='center'><strong>Sl.No</strong></td>");
      stringBuilder1.Append("<td style='width:100px;'>");
      stringBuilder1.Append("<strong>Session Year</strong>");
      stringBuilder1.Append("</td>");
      double num3 = 0.0;
      double num4 = 0.0;
      double num5 = 0.0;
      int num6 = 1;
      int index1 = 1;
      foreach (DataRow row in (InternalDataCollectionBase) tbcomp.Rows)
      {
        double num7 = Convert.ToDouble(dt.Compute("Sum([" + dt.Columns[index1].ColumnName + "])", "").ToString() == "" ? "0" : dt.Compute("Sum([" + dt.Columns[index1].ColumnName + "])", "").ToString());
        if (num7 > 0.0)
        {
          stringBuilder1.Append("<td style='width:50px;' align='right'>");
          stringBuilder1.Append("<strong>" + row["CatName"].ToString() + "</strong>");
          stringBuilder1.Append("</td>");
        }
        ++index1;
        if (num7 > 0.0)
          ++num2;
      }
      stringBuilder1.Append("<td style='width:100px; 'align='right'>");
      stringBuilder1.Append("<strong>Girls</strong>");
      stringBuilder1.Append("</td>");
      stringBuilder1.Append("<td style='width:100px; 'align='right'>");
      stringBuilder1.Append("<strong>Boys</strong>");
      stringBuilder1.Append("</td>");
      stringBuilder1.Append("<td style='width:100px; 'align='right'>");
      stringBuilder1.Append("<strong>Total</strong>");
      stringBuilder1.Append("</td>");
      stringBuilder1.Append("</tr>");
      double num8 = 0.0;
      foreach (DataRow row1 in (InternalDataCollectionBase) dt.Rows)
      {
        stringBuilder1.Append("<tr>");
        stringBuilder1.Append("<td align='center'>" + num6.ToString() + "</td>");
        stringBuilder1.Append("<td  valign='top' align='left' style='white-space:nowrap;'>");
        stringBuilder1.Append(row1["AdmnSessYr"].ToString().Trim());
        stringBuilder1.Append("</td>");
        double num7 = 0.0;
        double num9 = 0.0;
        num8 = 0.0;
        int count = tbcomp.Rows.Count;
        int num10 = 0;
        int index2 = 1;
        foreach (DataRow row2 in (InternalDataCollectionBase) tbcomp.Rows)
        {
          double num11 = Convert.ToDouble(dt.Compute("Sum([" + dt.Columns[index2].ColumnName + "])", "").ToString() == "" ? "0" : dt.Compute("Sum([" + dt.Columns[index2].ColumnName + "])", "").ToString());
          if (num10++ < count)
          {
            string str = row1[row2["CatID"].ToString()].ToString().Trim();
            if (row2["CatID"].ToString() == dt.Columns[index2].ColumnName.ToString())
            {
              if (num11 > 0.0)
              {
                stringBuilder1.Append("<td align='right'>");
                stringBuilder1.Append(row1[row2["CatID"].ToString()].ToString().Trim());
                stringBuilder1.Append("</td>");
              }
            }
            else
            {
              stringBuilder1.Append("<td align='right'>");
              stringBuilder1.Append(row1[row2["CatID"].ToString()].ToString().Trim());
              stringBuilder1.Append("</td>");
            }
            if (str != "")
              num7 += Convert.ToDouble(str);
            ++index2;
          }
        }
        stringBuilder1.Append("<td  valign='top' align='right' style='white-space:nowrap;'>");
        stringBuilder1.Append(row1["Female"].ToString().Trim());
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("<td  valign='top' align='right' style='white-space:nowrap;'>");
        stringBuilder1.Append(row1["Male"].ToString().Trim());
        stringBuilder1.Append("</td>");
        stringBuilder1.Append("<td align='right'>");
        stringBuilder1.Append(num7.ToString());
        stringBuilder1.Append("</td>");
        ++num1;
        ++num6;
        stringBuilder1.Append("</tr>");
        num3 += Convert.ToDouble(num7);
        num9 = row1["Male"].ToString().Trim() == "" || row1["Male"].ToString().Trim() == null ? 0.0 : Convert.ToDouble(row1["Male"].ToString().Trim());
        double num12 = row1["Female"].ToString().Trim() == "" || row1["Female"].ToString().Trim() == null ? 0.0 : Convert.ToDouble(row1["Female"].ToString().Trim());
        num4 += Convert.ToDouble(num9.ToString().Trim());
        num5 += Convert.ToDouble(num12.ToString().Trim());
      }
      stringBuilder1.Append("<tr>");
      stringBuilder1.Append("<td colspan='2' align='right'><strong>Total:");
      stringBuilder1.Append("</strong></td>");
      int index3 = 1;
      foreach (DataRow row in (InternalDataCollectionBase) tbcomp.Rows)
      {
        double num7 = Convert.ToDouble(dt.Compute("Sum([" + dt.Columns[index3].ColumnName + "])", "").ToString() == "" ? "0" : dt.Compute("Sum([" + dt.Columns[index3].ColumnName + "])", "").ToString());
        if (row["CatID"].ToString() == dt.Columns[index3].ColumnName && row["CatName"].ToString() != "Total")
        {
          if (num7 > 0.0)
          {
            stringBuilder1.Append("<td style='width:50px;' align='right'>");
            stringBuilder1.Append("<strong>" +  num7 + "</strong>");
            stringBuilder1.Append("</td>");
          }
          ++index3;
        }
      }
      stringBuilder1.Append("<td  valign='top' align='right' style='white-space:nowrap;'><strong>");
      stringBuilder1.Append(num5.ToString());
      stringBuilder1.Append("</strong></td>");
      stringBuilder1.Append("<td  valign='top' align='right' style='white-space:nowrap;'><strong>");
      stringBuilder1.Append(num4.ToString());
      stringBuilder1.Append("</strong></td>");
      stringBuilder1.Append("<td align='right'><strong>");
      stringBuilder1.Append(num3.ToString());
      string str1 = num3.ToString();
      stringBuilder1.Append("</strong></td>");
      stringBuilder1.Append("</tr>");
      stringBuilder1.Append("</table>");
      if (type == 1)
        ViewState["TotFee"] =  str1;
    }
    return stringBuilder1;
  }

  protected void rbtnAll_CheckedChanged(object sender, EventArgs e)
  {
    GetCatRpt();
  }

  protected void rbtnExisting_CheckedChanged(object sender, EventArgs e)
  {
    GetCatRpt();
  }

  protected void rbtnNew_CheckedChanged(object sender, EventArgs e)
  {
    GetCatRpt();
  }

  protected void btnPrint_Click(object sender, EventArgs e)
  {
    try
    {
      ScriptManager.RegisterClientScriptBlock((Page) this, GetType(), "ShowMessage", "window.open('rptAdmnSumPrint.aspx');", true);
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
      string str = Server.MapPath("Exported_Files/Admn Summary" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
      FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
      StreamWriter streamWriter = new StreamWriter((Stream) fileStream);
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

  protected void btnExpExcel_Click(object sender, EventArgs e)
  {
    try
    {
      if (lblReport.Text.ToString().Trim() != "")
      {
        StringBuilder stringBuilder = new StringBuilder("");
        stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
        stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
        stringBuilder.Append("<tr><td align='left' colspan='14'><b>");
        stringBuilder.Append("Admission Summary :- ");
        stringBuilder.Append("</b></td></tr>");
        stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
        stringBuilder.Append(lblReport.Text.ToString().Trim());
        stringBuilder.Append("<table><tr><td style='height: 10px'></td></tr>");
        stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
        stringBuilder.Append("</table>");
        ExportToExcel(stringBuilder.ToString());
      }
      else
        Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
    }
    catch (Exception ex)
    {
      string message = ex.Message;
    }
  }

  protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
  {
    GetCatRpt();
  }
}

