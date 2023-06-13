using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Admissions_PreAdmn_Showstudents : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.DefaultButton = btnShow.UniqueID;
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        FillSessionDropdown();
        FillClassDropDown();
        FillSelectStudent();
        lblRecCount.Text = "";
    }

    private void FillSessionDropdown()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillClassDropDown()
    {
        drpClass.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassID";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("ALL", "0"));
    }

    private void FillSelectStudent()
    {
        drpSelectStudent.DataSource = new clsDAL().GetDataTable("ps_sp_get_StudNamesPreAdmn", new Hashtable()
    {
      {
         "@Session",
         drpSession.SelectedValue
      },
      {
         "@Class",
         drpClass.SelectedValue
      }
    });
        drpSelectStudent.DataTextField = "fullname";
        drpSelectStudent.DataValueField = "admnno";
        drpSelectStudent.DataBind();
        drpSelectStudent.Items.Insert(0, new ListItem("ALL", "0"));
    }

    private void FillGrid()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTableQry = clsDal.GetDataTableQry(StudentDetail());
        if (dataTableQry.Rows.Count > 0)
        {
            grdstuddet.DataSource = dataTableQry;
            grdstuddet.DataBind();
            grdstuddet.Visible = true;
            lblRecCount.Text = "Total Number of Student: " + dataTableQry.Rows.Count.ToString();
            btnDelete.Enabled = true;
        }
        else
        {
            btnDelete.Enabled = false;
            grdstuddet.Visible = false;
            lblRecCount.Text = "";
            ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
        }
    }

    private string StudentDetail()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select distinct s.AdmnNo as Admissionno ,OldAdmnNo,FullName , convert(varchar(10),s.dob,103) as dateofbirth, ");
        stringBuilder.Append("c.classname , s.TelNoResidence , Convert(varchar(10),s.AdmnDate ,103) as AdmnDate, FatherName, MotherName,s.StudType ");
        stringBuilder.Append(" from dbo.PS_StudMasterPreAdmn s");
        stringBuilder.Append(" inner join PS_ClassMaster c on s.Class = c.classid");
        stringBuilder.Append(" where 1=1");
        int num = 0;
        try
        {
            num = Convert.ToInt32(txtStudentName.Text);
        }
        catch (Exception ex)
        {
            string text = txtStudentName.Text;
        }
        switch (txtStudentName.Text.ToString().Trim() == "")
        {
            case false:
                if (num != 0)
                {
                    stringBuilder.Append(" and s.AdmnSessYr ='" + drpSession.SelectedValue + "'");
                    stringBuilder.Append(" and s.AdmnNo Like'" + num + "%'");
                    break;
                }
                stringBuilder.Append(" and s.AdmnSessYr ='" + drpSession.SelectedValue + "'");
                stringBuilder.Append(" and fullName Like '%" + txtStudentName.Text.ToString().Trim() + "%'");
                break;
            case true:
                if (drpSession.Items.Count > 0)
                    stringBuilder.Append(" and s.AdmnSessYr ='" + drpSession.SelectedValue + "'");
                if (drpClass.SelectedValue.ToString().Trim() != "0")
                    stringBuilder.Append(" and s.class =" + drpClass.SelectedValue);
                if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
                {
                    stringBuilder.Append(" and s.admnno =" + drpSelectStudent.SelectedValue);
                    break;
                }
                break;
        }
        stringBuilder.Append(" order by fullName");
        return stringBuilder.ToString();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("PreAdmn_Addstudentinfo.aspx?Type=N");
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdstuddet.Visible = false;
        FillSelectStudent();
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdstuddet.Visible = false;
        FillSelectStudent();
    }

    protected void drpSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdstuddet.Visible = false;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClassDropDown();
        FillSelectStudent();
        grdstuddet.Visible = false;
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        grdstuddet.Visible = true;
        FillGrid();
    }

    protected void grdstuddet_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdstuddet.PageIndex = e.NewPageIndex;
        FillGrid();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = clsDal.GetDataTable("Ps_Sp_GetStudDataForExport");
        if (dataTable2.Rows.Count > 0)
        {
            foreach (DataRow row in (InternalDataCollectionBase)dataTable2.Rows)
                new clsDAL().ExecuteScalar("ps_sp_InsertExpExlData", new Hashtable()
        {
          {
             "@StudId",
            row["StudId"]
          }
        });
            Admissions_PreAdmn_Showstudents.ExportToExcelFromDataTable(dataTable2, new int[1], new string[30]
      {
        "AdmnNo",
        "AdmnDate",
        "SessionYear",
        "ReadingClass",
        "Section",
        "RollNo",
        "FullName",
        "NickName",
        "DOB",
        "FatherName",
        "FatherOccupation",
        "MotherName",
        "MotherOccupation",
        "Gender",
        "Religion",
        "Category",
        "Nationality",
        "MotherTongue",
        "PresAddress",
        "PresAddrDist",
        "PresAddrPin",
        "PresAddrPS",
        "PermAddress",
        "PermAddrDist",
        "PermAddrPin",
        "PermAddrPS",
        "Phone",
        "Mobile",
        "UrbanRuralTribe",
        "SSVMID"
      }, "../Reports/Exported_Files/SSVM_StudentDetails" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
        }
        else
            lblMsg.Text = "No data exist to export !";
    }

    public static string ExportToExcelFromDataTable(DataTable dtOrig, int[] delColIndx, string[] colNames, string filename)
    {
        try
        {
            if (!Admissions_PreAdmn_Showstudents.validateDelColIndx(dtOrig, delColIndx))
                return "Verify the list of indices to be deleted !";
            if (colNames.Length > dtOrig.Columns.Count - delColIndx.Length)
                return "No of column names didn't match the no of columns that will be available in the final result !";
            DataTable dataTable = dtOrig.Copy();
            int num1 = 0;
            foreach (int num2 in delColIndx)
            {
                dataTable.Columns.RemoveAt(num2 - num1);
                ++num1;
            }
            dataTable.AcceptChanges();
            int index1 = 0;
            foreach (string colName in colNames)
            {
                dataTable.Columns[index1].ColumnName = colName;
                ++index1;
            }
            dataTable.AcceptChanges();
            string str1 = "attachment; filename=" + filename + ".xls";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.AddHeader("content-disposition", str1);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            string str2 = "";
            foreach (DataColumn column in (InternalDataCollectionBase)dataTable.Columns)
            {
                HttpContext.Current.Response.Write(str2 + column.ColumnName);
                str2 = "\t";
            }
            HttpContext.Current.Response.Write("\n");
            foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
            {
                string str3 = "";
                for (int index2 = 0; index2 < dataTable.Columns.Count; ++index2)
                {
                    HttpContext.Current.Response.Write(str3 + row[index2].ToString());
                    str3 = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
            HttpContext.Current.Response.End();
            return string.Empty;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private static bool validateDelColIndx(DataTable dtOrig, int[] colIndx)
    {
        try
        {
            bool flag = true;
            foreach (int num in colIndx)
            {
                if (num > dtOrig.Columns.Count)
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }
        catch
        {
            return false;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
        }
        else
        {
            string str1 = Request["Checkb"];
            int num = 0;
            string str2 = str1;
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str2.Split(chArray))
            {
                if (DeleteStud(obj.ToString()) > 0)
                    ++num;
            }
            if (num > 0)
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('" + num.ToString() + " of the selected Records could not be deleted');", true);
            else
                ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Record Deleted Successfully !');", true);
            grdstuddet.Visible = true;
            FillGrid();
        }
    }

    private int DeleteStud(string id)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable = new DataTable();
        clsDAL clsDal = new clsDAL();
        hashtable.Add("@Admissionno", id);
        return clsDal.GetDataTable("Ps_sp_DelStud", hashtable).Rows.Count > 0 ? 1 : 0;
    }

    protected void btnExst_Click(object sender, EventArgs e)
    {
        Response.Redirect("Addstudentinfo.aspx?Type=E");
    }

    protected void btnTc_Click(object sender, EventArgs e)
    {
        Response.Redirect("Addstudentinfo.aspx?Type=T");
    }

    protected void btnCasual_Click(object sender, EventArgs e)
    {
        Response.Redirect("Addstudentinfo.aspx?Type=C");
    }
}