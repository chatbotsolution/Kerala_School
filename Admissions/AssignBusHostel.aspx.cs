using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admissions_AssignBusHostel : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        clsStaticDropdowns clsStaticDropdowns = new clsStaticDropdowns();
        if (!Page.IsPostBack)
        {
            BindDropDown(drpclass, "SELECT ClassID,ClassName FROM dbo.PS_ClassMaster", "ClassName", "ClassID");
            drpclass.Items.Insert(0, new ListItem("-Select-", "0"));
            BindDropDown(drpSession, "select SessionYr from dbo.PS_FeeNormsNew order by SessionID desc", "SessionYr", "SessionYr");
            clsStaticDropdowns.FillSection(drpSection);
            drpSection.Items.RemoveAt(0);
            drpSection.Items.Insert(0, new ListItem("-All-", "0"));
        }
        lblMsg.Text = "";
        string str = drpSession.SelectedValue.Trim();
        DateTime dateTime = Convert.ToDateTime("1 Apr" + str.Substring(0, 4));
        if (DateTime.Today > Convert.ToDateTime("31 Mar" + Convert.ToString(Convert.ToInt32(str.Substring(0, 4)) + 1)) && DateTime.Today > dateTime)
        {
            rbtnBus.Enabled = false;
            rbtnHostel.Enabled = false;
            btnAssign.Enabled = false;
            btnDeny.Enabled = false;
        }
        else
        {
            rbtnBus.Enabled = true;
            rbtnHostel.Enabled = true;
            btnAssign.Enabled = true;
            btnDeny.Enabled = true;
        }
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
    }

    private void fillstudent()
    {
        obj = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.Trim());
        hashtable.Add("@ClassId", drpclass.SelectedValue.Trim());
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue.Trim());
        if (drpExistingList.SelectedIndex > 0)
            hashtable.Add("@Existing", drpExistingList.SelectedIndex);
        DataTable dataTable2 = obj.GetDataTable("ps_sp_getBusHostStatus", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            grdStudentList.DataSource = dataTable2;
            grdStudentList.DataBind();
            lblRecCount.Text = "No Of Records :" + dataTable2.Rows.Count.ToString();
        }
        else
        {
            grdStudentList.DataSource = null;
            grdStudentList.DataBind();
            lblRecCount.Text = "";
        }
    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        ChangeStatus(1);
    }

    protected void btnDeny_Click(object sender, EventArgs e)
    {
        ChangeStatus(2);
    }

    private void ChangeStatus(int i)
    {
        if (Request["Checkb"] == null)
        {
            lblMsg.Text = "Please A Check Box!!";
            lblMsg.ForeColor = Color.Red;
        }
        else
        {
            string str = Request["Checkb"];
            int num = 0;
            string[] strArray = str.Split(',');
            for (int index = 0; index < strArray.Length; ++index)
            {
                if ((i != 1 ? DenyBusHostel(strArray[index].ToString()) : AllowBusHostl(strArray[index].ToString())) > 0)
                    ++num;
            }
            if (num > 0)
            {
                lblMsg.Text = (strArray.Length - num).ToString() + " Records Updated successfully " + num.ToString() + " Records Could Not Be Update";
                lblMsg.ForeColor = Color.Red;
            }
            else
            {
                lblMsg.Text = " Records Updated successfully ";
                lblMsg.ForeColor = Color.Green;
            }
            fillstudent();
        }
    }

    private int AllowBusHostl(string id)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        obj = new clsDAL();
        hashtable.Add("@AdmnNo", id);
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
        if (rbtnBus.Checked)
            hashtable.Add("@Facility", "BUS");
        else
            hashtable.Add("@Facility", "HOSTEL");
        hashtable.Add("@Type", 1);
        hashtable.Add("@UserId", Session["User_Id"].ToString().Trim());
        DataTable dataTable2 = obj.GetDataTable("Ps_Sp_SetBusHostStatus", hashtable);
        InsertDetailsForBusFee(id, drpSession.SelectedValue.ToString().Trim());
        return dataTable2.Rows.Count > 0 ? 1 : 0;
    }

    private void InsertDetailsForBusFee(string AdmnNo, string SessYr)
    {
        drpSession.SelectedValue.ToString().Trim();
        new clsDAL().ExcuteProcInsUpdt("ps_sp_insert_AdFeeLedgerBus", new Hashtable()
    {
      {
         "Admnno",
         AdmnNo
      },
      {
         "uid",
         Convert.ToInt32(Session["User_Id"])
      },
      {
         "Session",
         SessYr
      },
      {
         "SchoolId",
         Session["SchoolId"].ToString().Trim()
      }
    });
    }

    private int DenyBusHostel(string id)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable = new DataTable();
        obj = new clsDAL();
        hashtable.Add("@AdmnNo", id);
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString());
        if (rbtnBus.Checked)
            hashtable.Add("@Facility", "BUS");
        else
            hashtable.Add("@Facility", "HOSTEL");
        hashtable.Add("@UserId", Session["User_Id"].ToString().Trim());
        return obj.GetDataTable("Ps_Sp_SetBusHostStatus", hashtable).Rows.Count > 0 ? 1 : 0;
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpclass.SelectedIndex > 0)
        {
            fillstudent();
        }
        else
        {
            grdStudentList.DataSource = null;
            grdStudentList.DataBind();
        }
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    protected void drpExistingList_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpExistingList.SelectedIndex == 1)
        {
            grdStudentList.Columns[4].Visible = true;
            grdStudentList.Columns[5].Visible = false;
            rbtnHostel.Enabled = false;
            rbtnBus.Enabled = true;
            rbtnHostel.Checked = false;
            rbtnBus.Checked = true;
        }
        else if (drpExistingList.SelectedIndex == 2)
        {
            grdStudentList.Columns[5].Visible = true;
            grdStudentList.Columns[4].Visible = false;
            rbtnBus.Enabled = false;
            rbtnHostel.Enabled = true;
            rbtnHostel.Checked = true;
        }
        else
        {
            grdStudentList.Columns[4].Visible = true;
            grdStudentList.Columns[5].Visible = true;
            rbtnBus.Enabled = true;
            rbtnHostel.Enabled = true;
        }
        fillstudent();
    }
}