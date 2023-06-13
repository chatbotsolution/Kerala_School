using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SanLib;
using System.Data;

public partial class Admissions_SearchStudent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        
        
            if (Session["User"] != null)
            {
                txtSearch.Text = string.Empty;
                // txtDate.Visible = false;
                txtSearch.Visible = false;
                lblMsg.Text = "";
                lblMsg.ForeColor = System.Drawing.Color.Red;
                FillSessionDropdown();
                FillClassDropDown();
                FillSectionDropDown();
                FillSelectStudent();
                FillStudStatusDropDown();
                ViewState["Search"] = "";
            }
            else
            {
                Response.Redirect("../Login.aspx");
                return;
            }

            Hashtable ht = new Hashtable();
            if (Session["Show"] != null)
            {
                ht = Session["Show"] as Hashtable;
                drpSession.SelectedValue = ht["Session"].ToString();
                drpClass.SelectedValue = ht["Class"].ToString();
                drpSection.SelectedValue = ht["Section"].ToString();
                drpSelectStudent.SelectedValue = ht["Student"].ToString();
                FillGrid();
            }
            if (Session["Search"] != null)
            {
                ht.Clear();
                ht = Session["Search"] as Hashtable;
                drpSearch.SelectedValue = ht["Search"].ToString();
                txtSearch.Text = ht["SearchText"].ToString();
                if (ht["SearchDate"].ToString() != DateTime.Now.Date.ToString())
                {

                    txtDate.Visible = true;
                    txtSearch.Visible = false;
                    PopCalAdmnDt.Visible = true;
                    PopCalAdmnDt.SetDateValue(Convert.ToDateTime(ht["SearchDate"].ToString()));
                  //  DateTime dt = PopCalAdmnDt.GetDateValue();
                    txtDate.Text =Convert.ToDateTime( ht["SearchDate"]).ToString("dd-MM-yyyy");
                }
                else
                {
                    
                    txtDate.Visible = false;
                    txtSearch.Visible = true;
                    PopCalAdmnDt.SetDateValue(DateTime.Now.Date);
                   // PopCalAdmnDt.Visible = false;
                }
              FillGridSearch();
            }

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
        if (chkCasual.Checked.Equals(true))
            hashtable.Add("@StudType", "C");
        drpSelectStudent.DataSource = clsDal.GetDataTable("ps_sp_get_StudNamesForClassSec", hashtable);
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
            lblCount.Visible = true;
            lblCount.Text = "Total Number of Student: " + dataTableQry.Rows.Count.ToString();
            btnDelete.Enabled = true;
            ViewState["Search"] = "";
        }
        else
        {
            btnDelete.Enabled = false;
            grdstuddet.Visible = false;
            lblCount.Visible = false;
            lblCount.Text = "";
            ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
        }
    }

    private string StudentDetail()
    {
        string str = "select distinct s.*,cs.SessionYear,Cs.RollNo,sm.Description,sub.SubjectName,cm.CatName,Sr.Religion as StudReligion, s.AdmnNo as Admissionno ,OldAdmnNo,FullName,sex, convert(varchar(10),s.dob,103) as dateofbirth ,cs.classid, c.classname,ss.status as studentstatus , cs.Section , s.TelNoResidence , Convert(varchar(10),s.AdmnDate ,103) as AdmnDt, FatherName, MotherName,s.StudType  from PS_StudMaster s join PS_ClasswiseStudent cs on s.admnno = cs.admnno join PS_ClassMaster c on cs.classid = c.classid inner join Ps_StreamMaster sm On Sm.StreamId= cs.Stream left join PS_StudentReligion sr on sr.religionId= s.religion left join Ps_SubjectMaster sub on sub.subjectid =s.sixthoptional and sixthoptional <>0 left join Ps_CategoryMaster cm on cm.catid= s.cat  inner join Ps_StudStatus ss on cs.Statusid =ss.statusid  where 1=1 ";
       
        if (drpSession.Items.Count > 0)
            str = str + " and cs.sessionyear ='" + drpSession.SelectedValue + "'";
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            str = str + " and cs.classid =" + drpClass.SelectedValue;
        if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
            str = str + " and cs.admnno =" + drpSelectStudent.SelectedValue;
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            str = str + " and cs.section ='" + drpSection.SelectedValue.ToString().Trim() + "'";
        // return str + " order by fullName";
        if (drpStudStatus.SelectedIndex != 0)
            str = str + " and cs.StatusId ='" + drpStudStatus.SelectedValue.ToString().Trim() + "'";

        return str;
    }
    protected void BtnSearch_Clicked(object sender, EventArgs e)
    {
    if (drpSearch.SelectedValue.Trim() == "0")
        {
            lblMsg.Visible = true;
            lblMsg.Text = "Please Select One type to search";
            return;
        }
    else if ((txtSearch.Text.Trim() == string.Empty || txtSearch.Text.Trim() == "") && txtDate.Text ==DateTime.Now.Date.ToString("dd-MM-yyyy"))
    {
        lblMsg.Visible = true;
        lblMsg.Text = "Search Box Can't be blank";
       // return;
    }
    else
    {
        lblMsg.Visible = false;
        FillGridSearch();
    }
      

    Hashtable ht = new Hashtable();
    ht.Add("Search", drpSearch.SelectedValue.Trim());
    ht.Add("SearchText", txtSearch.Text.Trim());
    ht.Add("SearchDate", PopCalAdmnDt.GetDateValue());
    Session["Search"] = ht;
    Session["Show"] = null;
    }

private void FillGridSearch()
{
    string query = SearchQuery();
    if (query != "")
    {
        DataTable dt = new clsDAL().GetDataTableQry(query);
        if (dt.Rows.Count > 0)
        {
            lblCount.Visible = true;
            lblCount.Text = "Total Number of Student: " + dt.Rows.Count.ToString();
            ViewState["Search"] = "Has Data";
            grdstuddet.DataSource = dt;
            grdstuddet.DataBind();
            lblMsg.Visible = false;
            grdstuddet.Visible = true;
            ShowHide();
            btnDelete.Enabled = true;
        }

        else
        {
            ViewState["Search"] = "";
            btnDelete.Enabled = false;
            grdstuddet.DataSource = dt;
            grdstuddet.DataBind();
            grdstuddet.Visible = true;
            lblMsg.Visible = false;
            lblMsg.Text = "";
            lblCount.Visible = false;
            lblCount.Text = "";

            //  ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "ShowMessage", "alert('No Record Found');", true);
        }
    }
    else
    {
        ViewState["Search"] = "";
        btnDelete.Enabled = false;
        grdstuddet.DataSource = null;
        grdstuddet.DataBind();
        grdstuddet.Visible = false;
        lblMsg.Visible = true;
    }
}

private void ShowHide()
{
    string val = drpSearch.SelectedValue.Trim();
    if(val=="4")
       ChangeGrid(14);
    else if (val == "5")
        ChangeGrid(15);
    else if (val == "8")
        ChangeGrid(12);
    else if (val == "10")
        ChangeGrid(19);
    else if (val == "11")
        ChangeGrid(20);
    else if (val == "12")
        ChangeGrid(21);
    else if (val == "15")
        ChangeGrid(22);
    else if (val == "16")
        ChangeGrid(23);
    else if (val == "17")
        ChangeGrid(25);
    else if (val == "18")
        ChangeGrid(24);
    else if (val == "19")
        ChangeGrid(27);
    else if (val == "20")
        ChangeGrid(26);
    else if (val == "21")
        ChangeGrid(16);
    else if (val == "23")
        ChangeGrid(28);
    else if (val == "24")
        ChangeGrid(13);
    else if (val == "25")
        ChangeGrid(17);
    else if (val == "26")
        ChangeGrid(18);
  
}

private void ChangeGrid(int p)
{
      for (int i = 12; i <= grdstuddet.Columns.Count; i++)
    {
        if (i == p)
        {
            grdstuddet.Columns[p-1].Visible = true;
        }
        else
            grdstuddet.Columns[i-1].Visible = false;

    }
}

private string SearchQuery()
        {

        string text = txtSearch.Text.Trim();
        string str = drpSearch.SelectedValue.Trim();
        string param = string.Empty;
        string count = string.Empty;
        string query = "Select distinct P.*,P.Admnno as Admissionno,cw.classid,cm.catName,sm.SubjectName,cw.SessionYear,Cw.RollNo,S.description,Sr.Religion as StudReligion,C.ClassName,S.Description,ss.Status as StudentStatus,Cw.Section,convert(varchar,P.DOB,106) as dateofbirth,Convert(varchar,P.AdmnDate ,103) as AdmnDt From PS_StudMaster P inner join PS_ClasswiseStudent Cw on P.admnno=CW.admnno inner join PS_StreamMaster S on cw.stream= s.streamid inner join PS_ClassMaster C on cw.classid = c.classid inner join Ps_Studstatus ss on cw.statusid= ss.statusid left join PS_StudentReligion SR on Sr.ReligionId=P.Religion left join Ps_SubjectMaster sm on sm.subjectid=P.SixthOptional and sixthoptional <>0  left join Ps_CategoryMaster cm on cm.catid= P.cat or P.cat is null where cw.Detained_Promoted='' and ";
        if (str == "1")
        {
            param = "FullName";
            text = "%" + text + "%";
        }

         //else if (str == "2")
        //{
        //    param = "ClassID";
        //    text = text.ToUpper();
        //    DataTable dt1 = new clsDAL().GetDataTableQry("Select ClassID from PS_ClassMaster where ClassName='" + text + "'");
        //    if (dt1.Rows.Count > 0)
        //    {
        //        text = dt1.Rows[0][0].ToString();
        //        text = text + "%";
        //        param = "CW." + param;

         //        DataTable dt2 = new clsDAL().GetDataTableQry("Select P.*,C.ClassName,S.Description,Cw.Section,convert(varchar,P.DOB,106) as dateofbirth,Convert(varchar,P.AdmnDate ,103) as AdmnDt From PS_StudMaster P inner join PS_ClasswiseStudent Cw on P.admnno=CW.admnno inner join PS_StreamMaster S on cw.stream= s.streamid inner join PS_ClassMaster C on cw.classid = c.classid where " + param + " like '" + text + "'");
        //        grdstuddet.DataSource = dt2;
        //        grdstuddet.DataBind();
        //        lblMsg.Visible = false;
        //        grdstuddet.Visible = true;
        //        lblCount.Visible = true;
        //        lblCount.Text = dt2.Rows.Count.ToString() + "Records found !";
        //    }
        //    else
        //    {
        //        lblMsg.Visible = true;
        //        lblMsg.Text = "Type Class Name Correctly";
        //        grdstuddet.Visible = false;
        //        lblCount.Visible = false;
        //    }
        //    return;

         //}
        //else if (str == "3")
        //{
        //    param = "Section";
        //    text = text + "%";
        //    param = "CW." + param;

         //    DataTable dt3 = new clsDAL().GetDataTableQry("Select P.*,C.ClassName,S.Description,Cw.Section,convert(varchar,P.DOB,106) as dateofbirth,Convert(varchar,P.AdmnDate ,103) as AdmnDt From PS_StudMaster P inner join PS_ClasswiseStudent Cw on P.admnno=CW.admnno inner join PS_StreamMaster S on cw.stream= s.streamid inner join PS_ClassMaster C on cw.classid = c.classid where " + param + " like '" + text + "'");
        //    lblCount.Visible = true;
        //    lblCount.Text = dt3.Rows.Count.ToString() + "Records found !";
        //    grdstuddet.DataSource = dt3;
        //    grdstuddet.DataBind();
        //    lblMsg.Visible = false;
        //    grdstuddet.Visible = true;
        //    return;
        //}

        else if (str == "4")
        {
            param = "Stream";
            text = "%" + text + "%";
            DataTable dt4 = new clsDAL().GetDataTableQry("Select StreamID from PS_StreamMaster where Description like'" + text + "'");
            if (dt4.Rows.Count > 0)
            {
                text = dt4.Rows[0][0].ToString();
                text = text + "%";
                param = "CW." + param;
                //DataTable dt5 = new clsDAL().GetDataTableQry("Select P.*,C.ClassName,S.Description,Cw.Section,convert(varchar,P.DOB,106) as dateofbirth,Convert(varchar,P.AdmnDate ,103) as AdmnDt From PS_StudMaster P inner join PS_ClasswiseStudent Cw on P.admnno=CW.admnno inner join PS_StreamMaster S on cw.stream= s.streamid inner join PS_ClassMaster C on cw.classid = c.classid where " + param + " like '" + text + "'");
                //grdstuddet.DataSource = dt5;
                //grdstuddet.DataBind();
                //lblMsg.Visible = false;
                //grdstuddet.Visible = true;
                //lblCount.Visible = true;
                //lblCount.Text = dt5.Rows.Count.ToString() + " Records found !";
            }
            else
            {
              
               // text = "";
                lblMsg.Visible = true;
                lblMsg.Text = "InCorrect Stream";
                grdstuddet.Visible = false;
                lblCount.Visible = false;
                return query = "";
            }

        }
        else if (str == "5")
        {
            param ="CW."+ "SessionYear";
            text = text + "%";
        }

        else if (str == "6")
        {
            param = "RollNo";
            text = "%" + text + "%";
        }
        else if (str == "7")
        {
            param = "CW.AdmnNo";
            text = text + "%";
        }
        else if (str == "8")
        {
            param = "AdmnDate";
            DateTime dt = PopCalAdmnDt.GetDateValue();
            string admdt = dt.ToString("yyyy-MM-dd");
            string dobquery = "Select distinct P.*,P.Admnno as Admissionno,cw.classid,cm.catName,sm.SubjectName,cw.SessionYear,Cw.RollNo,S.description,Sr.Religion as StudReligion,C.ClassName,S.Description,ss.Status as StudentStatus,Cw.Section,convert(varchar,P.DOB,106) as dateofbirth,Convert(varchar,P.AdmnDate ,103) as AdmnDt From PS_StudMaster P inner join PS_ClasswiseStudent Cw on P.admnno=CW.admnno inner join PS_StreamMaster S on cw.stream= s.streamid inner join PS_ClassMaster C on cw.classid = c.classid inner join Ps_Studstatus ss on cw.statusid= ss.statusid left join PS_StudentReligion SR on Sr.ReligionId=P.Religion left join Ps_SubjectMaster sm on sm.subjectid=P.SixthOptional and sixthoptional <>0  left join Ps_CategoryMaster cm on cm.catid= P.cat or P.cat is null where cw.Detained_Promoted='' and AdmnDate='" + admdt + "'";
            return dobquery;
            //text = text + "%";
        }
        else if (str == "9")
         {
           
            param = "DOB";
            DateTime dt = PopCalAdmnDt.GetDateValue();
           string dob = dt.ToString("yyyy-MM-dd");

           // string dob = txtDate.Text;
           string dobquery = "Select distinct P.*,P.Admnno as Admissionno,cw.classid,cm.catName,sm.SubjectName,cw.SessionYear,Cw.RollNo,S.description,Sr.Religion as StudReligion,C.ClassName,S.Description,ss.Status as StudentStatus,Cw.Section,convert(varchar,P.DOB,106) as dateofbirth,Convert(varchar,P.AdmnDate ,103) as AdmnDt From PS_StudMaster P inner join PS_ClasswiseStudent Cw on P.admnno=CW.admnno inner join PS_StreamMaster S on cw.stream= s.streamid inner join PS_ClassMaster C on cw.classid = c.classid inner join Ps_Studstatus ss on cw.statusid= ss.statusid left join PS_StudentReligion SR on Sr.ReligionId=P.Religion left join Ps_SubjectMaster sm on sm.subjectid=P.SixthOptional and sixthoptional <>0  left join Ps_CategoryMaster cm on cm.catid= P.cat or P.cat is null where cw.Detained_Promoted='' and DOB='" + dob + "'";
            return dobquery;
           // text = text + "%";
        }
        else if (str == "10")
        {
            param = "PlaceOfBirth";
            text = text + "%";
        }
        else if (str == "11")
        {
            param = "BloodGroup";
            text = text + "%";
        }
        else if (str == "12")
        {
            param = "PresAddrCity";
            text = text + "%";
        }
        else if (str == "13")
        {
            param = "FatherName";
            text = "%" + text + "%"; 
        }
        else if (str == "14")
        {
            param = "MotherName";
            text = "%" + text + "%"; 
        }
        else if (str == "15")
        {
            param = "FatherOccupation";
            text = "%" + text + "%";
        }
        else if (str == "16")
        {
            param = "MotherOccupation";
            text = "%" + text + "%";
        }
        else if (str == "17")
        {
            param = "FatherDesig";
            text = "%" + text + "%";
        }
        else if (str == "18")
        {
            param = "MotherDesig";
            text = "%" + text + "%";
        }
        else if (str == "19")
        {
            param = "FatherComp";
            text = "%" + text + "%";
        }
        else if (str == "20")
        {
            param = "MotherComp";
            text = "%" + text + "%";
        }
        else if (str == "21")
        {
            param = "Religion";
            text = text + "%";
            DataTable dt6 = new clsDAL().GetDataTableQry("Select ReligionId from PS_StudentReligion where Religion like'" + text + "'");
            if (dt6.Rows.Count > 0)
            {
                param = "P." + param;
                text = dt6.Rows[0][0].ToString();
                text = text + "%";

            }
            else
            {
               
                lblMsg.Visible = true;
                lblMsg.Text = "InCorrect Religion";
                return query = "";
               // grdstuddet.Visible = false;
            }
           

        }
        else if (str == "22")
        {
            param = "P." + "Sex";
            text = text + "%";

            //DataTable dt7 = new clsDAL().GetDataTableQry("Select P.*,C.ClassName,S.Description,Cw.Section,convert(varchar,P.DOB,106) as dateofbirth,Convert(varchar,P.AdmnDate ,103) as AdmnDt From PS_StudMaster P inner join PS_ClasswiseStudent Cw on P.admnno=CW.admnno inner join PS_StreamMaster S on cw.stream= s.streamid inner join PS_ClassMaster C on cw.classid = c.classid where " + param + " like '" + text + "'");
            //lblCount.Visible = true;
            //lblCount.Text = dt7.Rows.Count.ToString() + " Records found !";
            //grdstuddet.DataSource = dt7;
            //grdstuddet.DataBind();
            //lblMsg.Visible = false;
            //grdstuddet.Visible = true;
            //return;
        }
        else if (str == "23")
        {
            param = "Cat";
            text = text + "%";
            DataTable dt7 = new clsDAL().GetDataTableQry("Select CATID from PS_CategoryMaster where CatName like'" + text + "'");
            if (dt7.Rows.Count > 0)
            {
                param = "P." + param;
                text = dt7.Rows[0][0].ToString();
                text = text + "%";

            }
            else
            {
               
                lblMsg.Visible = true;
                lblMsg.Text = "InCorrect Religion";
               return  query = "";
                // grdstuddet.Visible = false;
            }
        }
        else if (str == "24")
        {
            param = "StatusID";
            text = text + "%";
            DataTable dt8 = new clsDAL().GetDataTableQry("Select StatusID from PS_StudStatus where  Status like'" + text + "'");
            if (dt8.Rows.Count > 0)
            {
                param = "CW." + param;
                text = dt8.Rows[0][0].ToString();
                text = text + "%";

            }
            else
            {
              
                lblMsg.Visible = true;
                lblMsg.Text = "InCorrect Status";
                return query = "";
                // grdstuddet.Visible = false;
            }
            

        }
        else if (str == "25")
        {
            param = "SecondLang";
            text = text + "%";
        }
        else if (str == "26")
        {
            string text2 = "";
            param = "SixthOptional";
            text = text + "%";
            DataTable dt9 = new clsDAL().GetDataTableQry("Select SubjectId from PS_SubjectMaster where StreamId=1 and (ClassId=11 or Classid=12) and IsOptional=1 and SubjectName like'" + text + "'");
            if (dt9.Rows.Count > 0)
            {
                for(int i=0;i<dt9.Rows.Count;i++)
                 {
                   text = dt9.Rows[i]["SubjectID"].ToString();
                   text2 = text2 + "," + text;

                 }
                text2 = text2.Remove(0, 1);
                param = "P." + param;
                string newquery = "Select distinct P.*,P.Admnno as Admissionno,cw.classid,cm.catName,sm.SubjectName,cw.SessionYear,Cw.RollNo,S.description,Sr.Religion as StudReligion,C.ClassName,S.Description,ss.Status as StudentStatus,Cw.Section,convert(varchar,P.DOB,106) as dateofbirth,Convert(varchar,P.AdmnDate ,103) as AdmnDt From PS_StudMaster P inner join PS_ClasswiseStudent Cw on P.admnno=CW.admnno inner join PS_StreamMaster S on cw.stream= s.streamid inner join PS_ClassMaster C on cw.classid = c.classid inner join Ps_Studstatus ss on cw.statusid= ss.statusid left join PS_StudentReligion SR on Sr.ReligionId=P.Religion left join Ps_SubjectMaster sm on sm.subjectid=P.SixthOptional and sixthoptional <>0  left join Ps_CategoryMaster cm on cm.catid= P.cat or P.cat is null where cw.Detained_Promoted='' and " + param + " in (" + text2 + ")";
                return newquery;

            }
            else
            {
               
                lblMsg.Visible = true;
                lblMsg.Text = "InCorrect Status";
                return query = "";
                // grdstuddet.Visible = false;
            }
        }
        text = "'" + text + "'";
        query = query +" "+ param +" like "+ text;
        return query;
    }
    
    protected void grdstuddet_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdstuddet.PageIndex = e.NewPageIndex;
        if (ViewState["Search"].ToString() != "")
            FillGridSearch();
        else
            FillGrid();
            
        
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
        Hashtable ht = new Hashtable();
        ht.Add("Session", drpSession.SelectedValue.Trim());
        ht.Add("Class", drpClass.SelectedValue.Trim());
        ht.Add("Section", drpSection.SelectedValue.Trim());
        ht.Add("Student", drpSelectStudent.SelectedValue.Trim());
        Session["Show"] = ht;
        Session["Search"] = null;
    }
    //protected void btnSearch_Click(object sender, EventArgs e)
    //{
    //    ViewState["Search"] = "";
    //    grdstuddet.Visible = true;
    //    FillGrid();
    //    drpSearch.SelectedIndex = -1;
    //    txtSearch.Text = "";
        
    //}

    //protected void grdstuddet_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    grdstuddet.PageIndex = e.NewPageIndex;
    //    FillGrid();
    //}

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
            Admissions_SearchStudent.ExportToExcelFromDataTable(dataTable2, new int[1], new string[30]
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
        "PresAddress1",
        "PresAddrDist",
        "PresAddrPin",
        "PresAddrCity",
        "PermAddress1",
        "PermAddrDist",
        "PermAddrPin",
        "PermAddrCity",
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
            if (!Admissions_SearchStudent.validateDelColIndx(dtOrig, delColIndx))
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

    protected static bool validateDelColIndx(DataTable dtOrig, int[] colIndx)
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
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
            e.Row.Attributes.Add("onclick", "javascript:ChangeRowColor('" + e.Row.ClientID + "')");
        if (e.Row.RowType == DataControlRowType.DataRow && !((e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Alternate)) || (e.Row.RowState == DataControlRowState.Edit)))
        {
            Label lblsts = (Label)e.Row.FindControl("lblStatus");
            // DateTime EndDate = DateTime.Parse(lblEndDate.Text);
            if (lblsts.Text == "TC")
            {
                e.Row.BackColor = System.Drawing.Color.Yellow;
            }
            else if (lblsts.Text == "Passout")
            {
                e.Row.BackColor = System.Drawing.Color.Pink;
            }
            else if (lblsts.Text == "Absent")
            {
                e.Row.BackColor = System.Drawing.Color.Cyan;
            }
        }
        
    }

    protected void drpSearch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSearch.SelectedValue.Trim() == "9" || drpSearch.SelectedValue.Trim() == "8")
        {
            
            txtDate.Visible = true;
            txtDate.Text = "";
            txtSearch.Visible = false;
            PopCalAdmnDt.Visible = true;
            PopCalAdmnDt.SetDateValue(DateTime.Now.Date);
            //Hashtable ht = Session["Search"] as Hashtable;
            //ht.Clear();
            //txtSearch.Enabled = false;
        }
        else
        {
           
            txtSearch.Visible = true;
            txtDate.Visible = false;
            PopCalAdmnDt.SetDateValue(DateTime.Now.Date);
            PopCalAdmnDt.Visible = false;
            txtSearch.Text = "";
        }
    }

    protected void drpStudStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    private void FillStudStatusDropDown()
    {

        drpStudStatus.DataSource = new clsDAL().GetDataTable("[ps_sp_get_StudStatusForDDL]", new Hashtable());
        drpStudStatus.DataTextField = "Status";
        drpStudStatus.DataValueField = "StatusID";
        drpStudStatus.DataBind();
        drpStudStatus.Items.Insert(0, new ListItem("ALL", "0"));
        drpStudStatus.SelectedIndex = 1;
    }

}