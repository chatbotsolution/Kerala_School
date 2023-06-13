using ASP;
using Classes.DA;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Admissions_AllotSection : System.Web.UI.Page
{
    public static int grdrowcount;
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        Page.Form.DefaultButton = btnShow.UniqueID;
        btnUpdtSec.Enabled = false;
        if (Page.IsPostBack)
            return;
        filldropdowns();
        lblRecCount.Text = "";
    }

    private void filldropdowns()
    {
        FillClass();
        FillSession();

        
       
    }
    private void FillSectionDropDown()
    {
        // int int16 = (int)Convert.ToInt16(ConfigurationManager.AppSettings["TotSec"].ToString());
        int int16 = (int)Convert.ToInt16(new clsDAL().ExecuteScalarQry("Select NoOfSections from PS_ClassMaster where ClassId=" + drpClass.SelectedValue.ToString().Trim() + ""));
        int num1 = int16;
        if (int16 <= 0)
            return;
        drpSection.Items.Clear();
        int num2 = 65;
        int index = 0;
        while (num1 > 0)
        {
            drpSection.Items.Insert(index, new ListItem(Convert.ToChar(num2).ToString(), Convert.ToChar(num2).ToString()));
            --num1;
            ++index;
            ++num2;
        }
        drpSection.Items.Insert(0, new ListItem("--SELECT--", "0"));
        drpSection.Items.Insert(1, new ListItem("N", "N"));
    }
    private void FillClass()
    {
        drpClass.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassForAdmnDDL");
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "classid";
        drpClass.DataBind();
        drpClass.Items.RemoveAt(0);
        drpClass.Items.Insert(0, new ListItem("--SELECT--", "0"));
    }

    private void FillSession()
    {
        drpSession.DataSource = new clsDAL().GetDataTableQry("select distinct SessionID,SessionYr from dbo.PS_FeeNormsNew order by sessionID DESC");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillStream()
    {
        drpStream.DataSource = new clsDAL().GetDataTableQry("select StreamID,Description from dbo.PS_StreamMaster order by StreamID");
        drpStream.DataTextField = "Description";
        drpStream.DataValueField = "StreamID";
        drpStream.DataBind();
        drpStream.Items.RemoveAt(0);
        drpStream.Items.RemoveAt(0);
    }


    private void FillSelectStudent()
    {
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@SessionYear", drpSession.SelectedValue);
        if (drpClass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpClass.SelectedValue);
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue);
        if (Convert.ToInt32(drpClass.SelectedValue) > 13)
            hashtable.Add("@Stream", drpStream.SelectedValue);
            drpSelectStudent.DataSource= common.GetDataTable("ps_StudSectionAlert", hashtable);
          drpSelectStudent.DataTextField = "FullName";
        drpSelectStudent.DataValueField = "admnno";
        drpSelectStudent.DataBind();
        drpSelectStudent.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        grdstudents.Visible = true;
        FillGrid();
    }

    private void FillGrid()
    {
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable = common.GetDataTable("Ps_sp_GetStudForSectionAlot",StudentDetail());
        Admissions_AllotSection.grdrowcount = grdstudents.Rows.Count;
        if (dataTable.Rows.Count > 0)
        {
           
            btnUpdtSec.Enabled = true;
            grdstudents.DataSource = dataTable;
            grdstudents.DataBind();
            grdstudents.Visible = true;
            lblRecCount.Text = "Total Number of Student: " + dataTable.Rows.Count.ToString();
            UpdatePanel1.Update();
            BingGridSection();
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                ((DropDownList)grdstudents.Rows[i].FindControl("drpsection")).SelectedValue = dataTable.Rows[i]["Section"].ToString();

            }

        }
        else
        {
            btnUpdtSec.Enabled = false;
            grdstudents.Visible = false;
            lblRecCount.Text = "";
            lblMsg.Text = "No Record Found !";
            UpdatePanel1.Update();
        }
    }

    private Hashtable StudentDetail()
    {
        Hashtable ht = new Hashtable();
        if (drpSession.Items.Count > 0)
            ht.Add("@sessionyear", drpSession.SelectedValue);
        if (drpClass.SelectedValue.ToString().Trim() != "0")
            ht.Add("@classid", drpClass.SelectedValue);
        if (drpSection.SelectedValue.ToString().Trim() != "0")
            ht.Add("@Section", drpSection.SelectedValue);
        if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
            ht.Add("@admnno", drpSelectStudent.SelectedValue);
        if (Convert.ToInt32(drpClass.SelectedValue) > 13)
            ht.Add("@Stream", drpStream.SelectedValue);
        return ht;

        //string str = "select distinct s.AdmnNo as Admissionno ,s.OldAdmnNo,FullName , convert(varchar(10),s.dob,103) as dateofbirth , c.classname , cs.Section , s.TelNoResidence , Convert(varchar(10),s.AdmnDate ,103) as AdmnDate, FatherName, MotherName  from PS_StudMaster s join PS_ClasswiseStudent cs on s.admnno = cs.admnno  join PS_ClassMaster c on cs.classid = c.classid  " + " where 1=1 and StatusId=1 ";
        //if (drpSession.Items.Count > 0)
        //    str = str + " and cs.sessionyear ='" + drpSession.SelectedValue + "'";
        //if (drpClass.SelectedValue.ToString().Trim() != "0")
        //    str = str + " and cs.classid =" + drpClass.SelectedValue;
        //if (drpSection.SelectedValue.ToString().Trim() != "0")
        //    str = str + " and cs.Section ='" + drpSection.SelectedValue+"'";
        //if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
        //    str = str + " and cs.admnno =" + drpSelectStudent.SelectedValue;
        //if (Convert.ToInt32(drpClass.SelectedValue) > 13)
        //    str = str + "and cs.Stream=" + drpStream.SelectedValue;
        //    return str + "ORDER BY FullName";
       // return str;
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnUpdtSec.Enabled = false;
        grdstudents.Visible = false;
        lblRecCount.Text = "";
        FillSectionDropDown();
        FillSelectStudent();
        if (Convert.ToInt32(drpClass.SelectedValue) > 13)
        {
            FillStream();
            drpStream.Enabled = true;
        }
    }
    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnUpdtSec.Enabled = false;
        grdstudents.Visible = false;
        lblRecCount.Text = "";
        //FillSectionDropDown();
        FillSelectStudent();
    }
    protected void drpSelectStudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        grdstudents.Visible = false;
        lblRecCount.Text = string.Empty;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillClass();
        FillSelectStudent();
        btnUpdtSec.Enabled = false;
        lblRecCount.Text = string.Empty;
        grdstudents.Visible = false;
    }

    private void BingGridSection()
    {
        foreach (GridViewRow row in grdstudents.Rows)
        {
            ((HtmlInputCheckBox)row.FindControl("Checkb")).Checked = true;
            DropDownList control = (DropDownList)row.FindControl("drpsection");
            DataTable dt = new clsDAL().GetDataTableQry("Select NoOfSections from PS_ClassMaster where ClassId=" + drpClass.SelectedValue.ToString().Trim() + "");
            int int16 = (int)Convert.ToInt16(dt.Rows[0][0].ToString());
            int num1 = int16;
            if (int16 > 0)
            {
                control.Items.Clear();
                int num2 = 65;
                int index = 0;
                while (num1 > 0)
                {
                    control.Items.Insert(index, new ListItem(Convert.ToChar(num2).ToString(), Convert.ToChar(num2).ToString()));
                    --num1;
                    ++index;
                    ++num2;
                }
            }
        }
    }

    protected void btnUpdtSec_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdstudents.Rows)
        {
            clsDAL clsDal = new clsDAL();
            Hashtable hashtable = new Hashtable();
            DataTable dataTable = new DataTable();
            HtmlInputCheckBox control1 = (HtmlInputCheckBox)row.FindControl("Checkb");
            DropDownList control2 = (DropDownList)row.FindControl("drpsection");
            if (control1.Checked)
            {
                HiddenField control3 = (HiddenField)row.FindControl("hfAdNo");
                hashtable.Add("@Section", Convert.ToChar(control2.SelectedValue.ToString()));
                hashtable.Add("@admnno", Convert.ToInt64(control3.Value));
                clsDal.GetDataTable("ps_sp_UpdtSection", hashtable);
                lblMsg.Text = "Section alloted successfully !";
                lblMsg.ForeColor = Color.Green;
                grdstudents.Visible = false;
                lblRecCount.Text = string.Empty;
                drpSelectStudent.SelectedIndex = 0;
            }
        }
        FillGrid();
    }

    protected void grdstudents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
         if (e.Row.RowType == DataControlRowType.DataRow && (e.Row.RowState == DataControlRowState.Normal || e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] =
              string.Format("javascript:SelectRow(this, {0});", e.Row.RowIndex);
            // e.Row.Attributes.Add("onclick", "javascript:SelectRow('" + e.Row.ClientID + "',"+e.Row.RowIndex+");");
            e.Row.Attributes.Add("onkeydown", "javascript:return SelectSibling(event);");
            e.Row.Attributes.Add("onselectstart", "javascript:return false;");
         }
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        ((HtmlInputCheckBox)e.Row.FindControl("Checkb")).Checked = true;
        DropDownList control = (DropDownList)e.Row.FindControl("drpsection");
        int int16 = (int)Convert.ToInt16(ConfigurationManager.AppSettings["TotSec"].ToString());
        int num1 = int16;
        if (int16 > 0)
        {
            control.Items.Clear();
            int num2 = 65;
            int index = 0;
            while (num1 > 0)
            {
                control.Items.Insert(index, new ListItem(Convert.ToChar(num2).ToString(), Convert.ToChar(num2).ToString()));
                --num1;
                ++index;
                ++num2;
            }
        }
        string str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Section"));
        if (str == null || !(str.Trim() != ""))
            return;
        control.SelectedValue = str;
        control.BackColor = ColorTranslator.FromHtml("#E5FFCE");
       
    }


    protected void drpStream_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}