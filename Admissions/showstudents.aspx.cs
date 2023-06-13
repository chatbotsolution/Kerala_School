using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admissions_showstudents : System.Web.UI.Page
{
    string studtype;
    protected void Page_Load(object sender, EventArgs e)
    {
       // Page.Form.DefaultButton = btnShow.UniqueID;
//lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        //FillSessionDropdown();
        //FillClassDropDown();
        //FillSectionDropDown();
        //FillSelectStudent();
        //lblRecCount.Text = "";
    }

    //private void FillSessionDropdown()
    //{
    //    drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
    //    drpSession.DataTextField = "SessionYr";
    //    drpSession.DataValueField = "SessionYr";
    //    drpSession.DataBind();
    //}

    //private void FillClassDropDown()
    //{
    //    drpClass.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
    //    drpClass.DataTextField = "ClassName";
    //    drpClass.DataValueField = "ClassID";
    //    drpClass.DataBind();
    //    drpClass.Items.Insert(0, new ListItem("ALL", "0"));
    //}

    //private void FillSectionDropDown()
    //{
    //    drpSection.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    //{
    //  {
    //     "classId",
    //     drpClass.SelectedValue
    //  },
    //  {
    //     "session",
    //     drpSession.SelectedValue
    //  }
    //});
    //    drpSection.DataTextField = "Section";
    //    drpSection.DataValueField = "Section";
    //    drpSection.DataBind();
    //    drpSection.Items.Insert(0, new ListItem("ALL", "0"));
    //}

    //private void FillSelectStudent()
    //{
    //    clsDAL clsDal = new clsDAL();
    //    Hashtable hashtable = new Hashtable();
    //    if (drpSession.Items.Count > 0)
    //        hashtable.Add("@Session", drpSession.SelectedValue);
    //    if (drpClass.SelectedValue.ToString().Trim() != "0")
    //        hashtable.Add("@Class", drpClass.SelectedValue);
    //    if (drpSection.SelectedValue.ToString().Trim() != "0")
    //        hashtable.Add("@Section", drpSection.SelectedValue);
    //    if (chkCasual.Checked.Equals(true))
    //        hashtable.Add("@StudType", "C");
    //    drpSelectStudent.DataSource = clsDal.GetDataTable("ps_sp_get_StudNamesForClassSec", hashtable);
    //    drpSelectStudent.DataTextField = "fullname";
    //    drpSelectStudent.DataValueField = "admnno";
    //    drpSelectStudent.DataBind();
    //    drpSelectStudent.Items.Insert(0, new ListItem("ALL", "0"));
    //}

    //private void FillGrid()
    //{
    //    clsDAL clsDal = new clsDAL();
    //    Hashtable hashtable = new Hashtable();
    //    DataTable dataTableQry = clsDal.GetDataTableQry(StudentDetail());
    //    if (dataTableQry.Rows.Count > 0)
    //    {
    //        grdstuddet.DataSource = dataTableQry;
    //        grdstuddet.DataBind();
    //        grdstuddet.Visible = true;
    //        lblRecCount.Text = "Total Number of Student: " + dataTableQry.Rows.Count.ToString();
    //        btnDelete.Enabled = true;
    //    }
    //    else
    //    {
    //        btnDelete.Enabled = false;
    //        grdstuddet.Visible = false;
    //        lblRecCount.Text = "";
    //        ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
    //    }
    //}

    //private string StudentDetail()
    //{
    //    string str = "select distinct s.AdmnNo as Admissionno ,OldAdmnNo,FullName , convert(varchar(10),s.dob,103) as dateofbirth ,cs.classid, c.classname , cs.Section , s.TelNoResidence , Convert(varchar(10),s.AdmnDate ,103) as AdmnDate, FatherName, MotherName,s.StudType  from PS_StudMaster s join PS_ClasswiseStudent cs on s.admnno = cs.admnno  join PS_ClassMaster c on cs.classid = c.classid  " + " where 1=1 and StatusId=1";
    //    int num = 0;
    //    try
    //    {
    //        string no = txtStudentName.Text.Trim().Substring(txtStudentName.Text.Length - 4);
    //        num = Convert.ToInt32(no.Trim());

    //       // num = Convert.ToInt32(txtStudentName.Text);
    //    }
    //    catch (Exception ex)
    //    {
    //        string text = txtStudentName.Text.Trim();
    //    }
    //    switch (txtStudentName.Text.ToString().Trim() == "")
    //    {
    //        case false:
    //            if (num != 0)
    //            {
    //                str = str + " and cs.sessionyear ='" + drpSession.SelectedValue + "'" + " and s.OldAdmnno Like'" + txtStudentName.Text.ToString().Trim() + "%'";
    //                break;
    //            }
    //            str = str + " and cs.sessionyear ='" + drpSession.SelectedValue + "'" + " and s.FullName Like '%" + txtStudentName.Text.ToString().Trim() + "%'";
    //            break;
    //        case true:
    //            if (drpSession.Items.Count > 0)
    //                str = str + " and cs.sessionyear ='" + drpSession.SelectedValue + "'";
    //            if (drpClass.SelectedValue.ToString().Trim() != "0")
    //                str = str + " and cs.classid =" + drpClass.SelectedValue;
    //            if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
    //                str = str + " and cs.admnno =" + drpSelectStudent.SelectedValue;
    //            if (drpSection.SelectedValue.ToString().Trim() != "0")
    //                str = str + " and cs.section ='" + drpSection.SelectedValue.ToString().Trim() + "'";
    //            if (chkCasual.Checked.Equals(true))
    //            {
    //                str += " and StudType='C'";
    //                break;
    //            }
    //            break;
    //    }
    //    return str + " order by fullName";
    //}

   

    //protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    grdstuddet.Visible = false;
    //    FillSectionDropDown();
    //    FillSelectStudent();
    //}

    //protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    grdstuddet.Visible = false;
    //    FillSelectStudent();
    //}

    //protected void drpSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    grdstuddet.Visible = false;
    //}

    //protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    FillClassDropDown();
    //    FillSectionDropDown();
    //    FillSelectStudent();
    //    grdstuddet.Visible = false;
    //}

    //protected void btnShow_Click(object sender, EventArgs e)
    //{
    //    grdstuddet.Visible = true;
    //    FillGrid();
    //}
    //protected void btnSearch_Click(object sender, EventArgs e)
    //{
    //    grdstuddet.Visible = true;
    //    FillGrid();
    //}

    //protected void grdstuddet_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    grdstuddet.PageIndex = e.NewPageIndex;
    //    FillGrid();
    //}

    //protected void btnExport_Click(object sender, EventArgs e)
    //{
    //    clsDAL clsDal = new clsDAL();
    //    DataTable dataTable1 = new DataTable();
    //    DataTable dataTable2 = clsDal.GetDataTable("Ps_Sp_GetStudDataForExport");
    //    if (dataTable2.Rows.Count > 0)
    //    {
    //        foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
    //            new clsDAL().ExecuteScalar("ps_sp_InsertExpExlData", new Hashtable()
    //    {
    //      {
    //         "@StudId",
    //        row["StudId"]
    //      }
    //    });
    //        Admissions_showstudents.ExportToExcelFromDataTable(dataTable2, new int[1], new string[30]
    //  {
    //    "AdmnNo",
    //    "AdmnDate",
    //    "SessionYear",
    //    "ReadingClass",
    //    "Section",
    //    "RollNo",
    //    "FullName",
    //    "NickName",
    //    "DOB",
    //    "FatherName",
    //    "FatherOccupation",
    //    "MotherName",
    //    "MotherOccupation",
    //    "Gender",
    //    "Religion",
    //    "Category",
    //    "Nationality",
    //    "MotherTongue",
    //    "PresAddress1",
    //    "PresAddrDist",
    //    "PresAddrPin",
    //    "PresAddrCity",
    //    "PermAddress1",
    //    "PermAddrDist",
    //    "PermAddrPin",
    //    "PermAddrCity",
    //    "Phone",
    //    "Mobile",
    //    "UrbanRuralTribe",
    //    "SSVMID"
    //  }, "../Reports/Exported_Files/SSVM_StudentDetails" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
    //    }
    //    else
    //        lblMsg.Text = "No data exist to export !";
    //}

    //public static string ExportToExcelFromDataTable(DataTable dtOrig, int[] delColIndx, string[] colNames, string filename)
    //{
    //    try
    //    {
    //        if (!Admissions_showstudents.validateDelColIndx(dtOrig, delColIndx))
    //            return "Verify the list of indices to be deleted !";
    //        if (colNames.Length > dtOrig.Columns.Count - delColIndx.Length)
    //            return "No of column names didn't match the no of columns that will be available in the final result !";
    //        DataTable dataTable = dtOrig.Copy();
    //        int num1 = 0;
    //        foreach (int num2 in delColIndx)
    //        {
    //            dataTable.Columns.RemoveAt(num2 - num1);
    //            ++num1;
    //        }
    //        dataTable.AcceptChanges();
    //        int index1 = 0;
    //        foreach (string colName in colNames)
    //        {
    //            dataTable.Columns[index1].ColumnName = colName;
    //            ++index1;
    //        }
    //        dataTable.AcceptChanges();
    //        string str1 = "attachment; filename=" + filename + ".xls";
    //        HttpContext.Current.Response.Clear();
    //        HttpContext.Current.Response.AddHeader("content-disposition", str1);
    //        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
    //        string str2 = "";
    //        foreach (DataColumn column in (InternalDataCollectionBase)dataTable.Columns)
    //        {
    //            HttpContext.Current.Response.Write(str2 + column.ColumnName);
    //            str2 = "\t";
    //        }
    //        HttpContext.Current.Response.Write("\n");
    //        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
    //        {
    //            string str3 = "";
    //            for (int index2 = 0; index2 < dataTable.Columns.Count; ++index2)
    //            {
    //                HttpContext.Current.Response.Write(str3 + row[index2].ToString());
    //                str3 = "\t";
    //            }
    //            HttpContext.Current.Response.Write("\n");
    //        }
    //        HttpContext.Current.Response.End();
    //        return string.Empty;
    //    }
    //    catch (Exception ex)
    //    {
    //        return ex.Message;
    //    }
    //}

    //private static bool validateDelColIndx(DataTable dtOrig, int[] colIndx)
    //{
    //    try
    //    {
    //        bool flag = true;
    //        foreach (int num in colIndx)
    //        {
    //            if (num > dtOrig.Columns.Count)
    //            {
    //                flag = false;
    //                break;
    //            }
    //        }
    //        return flag;
    //    }
    //    catch
    //    {
    //        return false;
    //    }
    //}

    //protected void btnDelete_Click(object sender, EventArgs e)
    //{
    //    if (Request["Checkb"] == null)
    //    {
    //        ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
    //    }
    //    else
    //    {
    //        string str1 = Request["Checkb"];
    //        int num = 0;
    //        string str2 = str1;
    //        char[] chArray = new char[1] { ',' };
    //        foreach (object obj in str2.Split(chArray))
    //        {
    //            if (DeleteStud(obj.ToString()) > 0)
    //                ++num;
    //        }
    //        if (num > 0)
    //            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + num.ToString() + " of the selected Records could not be deleted');", true);
    //        else
    //            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Record Deleted Successfully !');", true);
    //        grdstuddet.Visible = true;
    //        FillGrid();
    //    }
    //}

    //private int DeleteStud(string id)
    //{
    //    Hashtable hashtable = new Hashtable();
    //    DataTable dataTable = new DataTable();
    //    clsDAL clsDal = new clsDAL();
    //    hashtable.Add("@Admissionno", id);
    //    return clsDal.GetDataTable("Ps_sp_DelStud", hashtable).Rows.Count > 0 ? 1 : 0;
    //}
    protected void btnNew_Click(object sender, EventArgs e)
    {
        studtype = CheckRbFormSelect();
        if (studtype == "-1")
        {
            lblError.Visible = true;
            lblError.Text = "Select Student Class Type";
            return;
        }
      // Response.Redirect("Addstudentinfo.aspx?Type=N&FormType=" + studtype + "");
        Response.Redirect("AddAStudent.aspx?Type=N&FormType=" + studtype + "");
    }
    protected void btnExst_Click(object sender, EventArgs e)
    {
        studtype = CheckRbFormSelect();
        if (studtype == "-1")
        {
            lblError.Visible = true;
            lblError.Text = "Select Student Class Type";
            return;
        }

        Response.Redirect("AddAStudent.aspx?Type=E&FormType=" + studtype + "");
      
    }

    protected void btnTc_Click(object sender, EventArgs e)
    {
        studtype = CheckRbFormSelect();
        if (studtype == "-1")
        {
            lblError.Visible = true;
            lblError.Text = "Select Student Class Type";
            return;
        }

        Response.Redirect("AddAStudent.aspx?Type=T&FormType=" + studtype + "");
       
    }

    protected void btnCasual_Click(object sender, EventArgs e)
    {
        studtype = CheckRbFormSelect();
        if (studtype == "-1")
        {
            lblError.Visible = true;
            lblError.Text = "Select Student Admission Type";
            return;
        }

        Response.Redirect("AddAStudent.aspx?Type=C&FormType=" + studtype + "");
       
    }

    private string  CheckRbFormSelect()
    {
        string formtype;
       
        if (RbFormSelect.SelectedIndex == -1)
        {
            //lblError.Text = "Select Student Class Type";
            formtype = "-1";
        }
        else
        {
            formtype = RbFormSelect.SelectedValue;
        }
        return formtype;

    }
    


}