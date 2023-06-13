using ASP;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Hostel_ShowHostelStudents : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.DefaultButton = btnShow.UniqueID;
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        FillSessionDropdown();
        FillClassDropDown();
        FillSectionDropDown();
        FillSelectStudent();
        lblRecCount.Text = "";
        if (!(Session["userrights"].ToString().Trim() == "a"))
            return;
        btnImport.Visible = true;
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

    private void FillSectionDropDown()
    {
        drpSection.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "classId",
         drpClass.SelectedValue
      },
      {
         "session",
         drpSession.SelectedValue
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("ALL", "0"));
    }

    private void FillSelectStudent()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@Session", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Class", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            hashtable.Add("@Section", drpSection.SelectedValue);
        drpSelectStudent.DataSource = clsDal.GetDataTable("Host_GetStudNamesForDrp", hashtable);
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
        string str = "select distinct s.AdmnNo as Admissionno ,FullName , convert(varchar(10),s.dob,103) as dateofbirth , c.classname , cs.Section , s.TelNoResidence , Convert(varchar(10),h.AdmissionDt ,103) as AdmnDate, FatherName, MotherName,s.StudType  from Host_Admission h inner join  PS_StudMaster s on h.admnno=s.admnno join PS_ClasswiseStudent cs on h.admnno = cs.admnno  join PS_ClassMaster c on cs.classid = c.classid  " + " where 1=1 and StatusId=1";
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
                    str = str + " and cs.sessionyear ='" + drpSession.SelectedValue + "'" + " and s.AdmnNo Like'" + num + "%'";
                    break;
                }
                str = str + " and cs.sessionyear ='" + drpSession.SelectedValue + "'" + " and fullName Like'" + txtStudentName.Text.ToString().Trim() + "%'";
                break;
            case true:
                if (drpSession.Items.Count > 0)
                    str = str + " and cs.sessionyear ='" + drpSession.SelectedValue + "'";
                if (drpClass.SelectedValue.ToString().Trim() != "0")
                    str = str + " and cs.classid =" + drpClass.SelectedValue;
                if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
                    str = str + " and cs.admnno =" + drpSelectStudent.SelectedValue;
                if (drpSection.SelectedValue.ToString().Trim() != "0")
                {
                    str = str + " and cs.section ='" + drpSection.SelectedValue.ToString().Trim() + "'";
                    break;
                }
                break;
        }
        return str + " order by fullName";
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("HostelAdmission.aspx");
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdstuddet.Visible = false;
        FillSectionDropDown();
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
        FillSectionDropDown();
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
            Hostel_ShowHostelStudents.ExportToExcelFromDataTable(dataTable2, new int[1], new string[30]
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
            if (!Hostel_ShowHostelStudents.validateDelColIndx(dtOrig, delColIndx))
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
        return clsDal.GetDataTable("Host_DelStud", hashtable).Rows.Count > 0 ? 1 : 0;
    }

    protected void btnImport_Click(object sender, EventArgs e)
    {
        string str = ConfigurationManager.AppSettings["DBMain"].ToString().Trim();
        clsDAL clsDal = new clsDAL();
        try
        {
            StringBuilder stringBuilder1 = new StringBuilder();
            stringBuilder1.Append("Insert into dbo.PS_StudMaster (StudId, AdmnNo, AdmnDate, FullName, NickName, DOB, MotherTongue, Locality, FatherName, FatherOccupation, MotherName, MotherOccupation, LocalGuardianName,");
            stringBuilder1.Append("RelationWithLG, PresentAddress, PresAddrDist, PresAddrPin, PresAddrPS, PermAddress, PermAddrDist, PermAddrPin, PermAddrPS, TeleNoOffice, TelNoResidence, EmailId, Nationality, Class, SportsProf, Hobbies, Cat, HouseID, Status, FeestartDate, sex, religion, OldAdmnNo, StudentPhoto, PrevSchoolname, Prevclass, MediumOfInst, TCNo, TCDate, UserID, UserDate, SchoolID, StudType, AdmnSessYr)");
            stringBuilder1.Append(" select * from " + str + ".dbo.PS_StudMaster s where s.AdmnNo not in (select AdmnNo from dbo.PS_StudMaster)");
            clsDal.ExecuteScalarQry(stringBuilder1.ToString().Trim());
            StringBuilder stringBuilder2 = new StringBuilder();
            stringBuilder2.Append("Insert into dbo.PS_ClasswiseStudent (SessionYear, ClassID, Section, Stream, RollNo, ClassJoinDate, Detained_Promoted, Grade, UserID, UserDate, SchoolID, admnno, StatusId, StatusDate)");
            stringBuilder2.Append(" select SessionYear, ClassID, Section, Stream, RollNo, ClassJoinDate, Detained_Promoted, Grade,");
            stringBuilder2.Append("UserID, UserDate, SchoolID, admnno, StatusId, StatusDate from " + str + ".dbo.PS_ClasswiseStudent s where s.id not in (select id from dbo.PS_ClasswiseStudent)");
            clsDal.ExecuteScalarQry(stringBuilder2.ToString().Trim());
            StringBuilder stringBuilder3 = new StringBuilder();
            stringBuilder3.Append("Insert into dbo.BusHostelChoice");
            stringBuilder3.Append("(BusHostelChId, SessionYr, AdmnNo, BusFacility, HostelFacility, SchoolId, UserId, UserDate)");
            stringBuilder3.Append("select * from " + str + ".dbo.BusHostelChoice b where ");
            stringBuilder3.Append("b.BusHostelChId not in (select BusHostelChId from dbo.BusHostelChoice)");
            clsDal.ExecuteScalarQry(stringBuilder3.ToString().Trim());
            lblMsg.Text = "Students Imported Successfully";
            lblMsg.ForeColor = Color.Green;
        }
        catch
        {
            lblMsg.Text = "Students could not be Imported !";
            lblMsg.ForeColor = Color.Red;
        }
    }
}