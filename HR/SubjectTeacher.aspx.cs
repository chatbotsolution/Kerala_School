using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_SubjectTeacher : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private clsStaticDropdowns objStatic = new clsStaticDropdowns();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            objStatic.FillSessionYr(drpSession);
            drpSession.Items.RemoveAt(0);
            bindDropDown(drpClass, "SELECT ClassId,ClassName FROM dbo.PS_ClassMaster ORDER BY ClassId ASC", "ClassName", "ClassId");
            objStatic.FillSection(drpSection);
            drpSection.Items.RemoveAt(0);
            drpSection.Items.Insert(0, new ListItem("- SELECT -", "0"));
        }
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- SELECT -", "0"));
    }

    private string GridCheck()
    {
        string str = string.Empty;
        try
        {
            foreach (GridViewRow row in grdAssign.Rows)
            {
                DropDownList control1 = row.FindControl("drpTeacher") as DropDownList;
                TextBox control2 = row.FindControl("txtEffDate") as TextBox;
                Label control3 = row.FindControl("lblSubject") as Label;
                if (control1.SelectedIndex > 0 && control2.Text.Trim() == string.Empty)
                {
                    str = "Enter Effective Date for " + control3.Text;
                    break;
                }
                if (control1.SelectedIndex == 0 && control2.Text.Trim() != string.Empty)
                {
                    str = "Select Teacher Name for " + control3.Text;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            str = "Error";
        }
        if (str.Trim() == string.Empty)
            str = DateChk();
        return str;
    }

    private string DateChk()
    {
        string str = string.Empty;
        Label label = new Label();
        try
        {
            foreach (GridViewRow row in grdAssign.Rows)
            {
                DropDownList control1 = row.FindControl("drpTeacher") as DropDownList;
                TextBox control2 = row.FindControl("txtEffDate") as TextBox;
                label = row.FindControl("lblSubject") as Label;
                if (control1.SelectedIndex > 0 && control2.Text.Trim() != string.Empty)
                {
                    string[] strArray = control2.Text.Trim().Split(new char[3]
          {
            '-',
            '.',
            '/'
          });
                    Convert.ToDateTime(strArray[1] + "-" + strArray[0] + "-" + strArray[2]);
                }
            }
        }
        catch (Exception ex)
        {
            str = "Invalid Effective Date for " + label.Text;
        }
        return str;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        string empty = string.Empty;
        string str = GridCheck();
        if (str.Trim() == string.Empty)
        {
            foreach (GridViewRow row in grdAssign.Rows)
            {
                HiddenField control1 = row.FindControl("hfSubjectId") as HiddenField;
                DropDownList control2 = row.FindControl("drpTeacher") as DropDownList;
                TextBox control3 = row.FindControl("txtEffDate") as TextBox;
                if (control2.SelectedIndex > 0 && control3.Text.Trim() != string.Empty)
                {
                    string[] strArray = control3.Text.Trim().Split(new char[3]
          {
            '-',
            '.',
            '/'
          });
                    DateTime dateTime = Convert.ToDateTime(strArray[1] + "-" + strArray[0] + "-" + strArray[2]);
                    str = obj.ExecuteScalar("PS_InsUpdtSubTeacher", new Hashtable()
          {
            {
               "@SessionYr",
               drpSession.SelectedValue
            },
            {
               "@ClassId",
               drpClass.SelectedValue
            },
            {
               "@Section",
               drpSection.SelectedValue
            },
            {
               "@TeacherEmpId",
               control2.SelectedValue
            },
            {
               "@EffectiveDt",
               dateTime
            },
            {
               "@SubjectId",
               control1.Value
            },
            {
               "@UserId",
              Session["User_Id"]
            }
          });
                    if (str.Trim() != string.Empty)
                        break;
                }
            }
            if (str.Trim() == string.Empty)
            {
                GetSubTeacherList();
                trMsg.Style["background-color"] = "Green";
                lblMsg.Text = "Data Saved Successfully";
            }
            else
            {
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Error";
            }
        }
        else
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = str;
        }
    }

    private void GetSubTeacherList()
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        if (drpClass.SelectedIndex > 0 && drpSection.SelectedIndex > 0)
        {
            DataTable dataTable = obj.GetDataTable("PS_GetSubTeacherList", new Hashtable()
      {
        {
           "@SessionYr",
           drpSession.SelectedValue
        },
        {
           "@ClassId",
           drpClass.SelectedValue
        },
        {
           "@Section",
           drpSection.SelectedValue.Trim().ToUpper()
        }
      });
            grdAssign.DataSource = dataTable;
            grdAssign.DataBind();
            if (dataTable.Rows.Count > 0)
            {
                btnSave.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
                trMsg.Style["background-color"] = "Red";
                lblMsg.Text = "Define Classwise Subjects before Assigning Subjectwise Teacher.";
            }
        }
        else
        {
            grdAssign.DataSource = null;
            grdAssign.DataBind();
            btnSave.Visible = false;
        }
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubTeacherList();
        drpSession.Focus();
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubTeacherList();
        drpClass.Focus();
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        GetSubTeacherList();
        drpSection.Focus();
    }

    protected void grdAssign_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        DropDownList control = (DropDownList)e.Row.FindControl("drpTeacher");
        DataTable dataTable = obj.GetDataTable("PS_GetTeacherList");
        control.DataSource = dataTable;
        control.DataTextField = "TeacherName";
        control.DataValueField = "TeacherId";
        control.DataBind();
        control.Items.Insert(0, new ListItem("- Not Assigned -", "0"));
        string str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "TeacherEmpId"));
        control.SelectedValue = str;
    }
}