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

public partial class Exam_SubjectMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        BindDropDown(drpSelectClass, "SELECT ClassId,ClassName FROM dbo.PS_ClassMaster ORDER BY ClassId ASC", "ClassName", "ClassId");
        BindClass();
        //BindStream();
    }

    private void BindClass()
    {
        chkListClass.DataSource = obj.GetDataTableQry("SELECT ClassId,ClassName FROM dbo.PS_ClassMaster ORDER BY ClassId ASC");
        chkListClass.DataTextField = "ClassName";
        chkListClass.DataValueField = "ClassId";
        chkListClass.DataBind();
    }
    private void BindStream()
    {
        ChkListStream.DataSource =DrpSelectStream.DataSource=  obj.GetDataTableQry("SELECT StreamID,Description FROM dbo.PS_StreamMaster ORDER BY StreamID ASC");
        ChkListStream.DataTextField =  DrpSelectStream.DataTextField="Description";
        ChkListStream.DataValueField = DrpSelectStream.DataValueField = "StreamID";
        ChkListStream.DataBind();
        DrpSelectStream.DataBind();
    }

    private void BindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    private void GetSubjectList()
    {
        if (drpSelectClass.SelectedIndex > 0)
        {
           if(drpSelectClass.SelectedItem.Text == "XI" || drpSelectClass.SelectedItem.Text == "XII")
            {
                grdSubject.Visible = true;
                DataTable dataTable1 = obj.GetDataTable("PS_GetSubjectList2", new Hashtable()
                  {
                    {
                       "@ClassId",
                       drpSelectClass.SelectedValue
                    },
                    {
                      "@StreamId",
                      DrpSelectStream.SelectedValue
                    }
               });
                grdSubject.DataSource = dataTable1;
                grdSubject.DataBind();
                lblRecords.Text = "No of Records : " + dataTable1.Rows.Count;
            }

            else
            {
                grdSubject.Visible = true;
                DataTable dataTable = obj.GetDataTable("PS_GetSubjectList", new Hashtable()
                                          {
                                            {
                                               "@ClassId",
                                               drpSelectClass.SelectedValue
                                            }
                                          });
                grdSubject.DataSource = dataTable;
                grdSubject.DataBind();
                lblRecords.Text = "No of Records : " + dataTable.Rows.Count;
            }

           }
            
        else
        {
            grdSubject.DataSource = null;
            grdSubject.DataBind();
            grdSubject.Visible = false;
            lblRecords.Text = string.Empty;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string empty = string.Empty;
        int num1 = 0;
        int num2 = 0;
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        if (chkListClass.Items.Count == 0)
        {
            lblMsg.Text = "Define Classes";
            trMsg.Style["background-color"] = "Red";
        }
        
        else if (ChkListStream.Visible= ChkListStream.Visible && ChkListStream.SelectedIndex == -1)
        {
            lblMsg.Text = "Define Stream";
            trMsg.Style["background-color"] = "Red";
        }
        else if (txtSubject.Text.Trim() == string.Empty)
        {
            lblMsg.Text = "Enter Subject Name";
            trMsg.Style["background-color"] = "Red";
            txtSubject.Focus();
        }

        else
        {
            foreach (ListItem listItem in chkListClass.Items)
            {
                if (listItem.Selected)
                {
                    if (ChkListStream.SelectedIndex > -1)
                    {
                        foreach (ListItem listitem2 in ChkListStream.Items)
                        {
                            if (listitem2.Selected)
                            {
                                Hashtable hashtable1 = new Hashtable();
                                hashtable1.Clear();
                                hashtable1.Add("@SubjectId", hfSubjectId.Value);
                                hashtable1.Add("@SubjectName", txtSubject.Text.Trim().ToUpper());
                                hashtable1.Add("@ClassId", listItem.Value);
                                hashtable1.Add("@StreamId", listitem2.Value);
                                hashtable1.Add("@UserId", Session["User_Id"]);
                                string abc =Session["User_Id"].ToString();
                                if (chkOpt.Checked)
                                    hashtable1.Add("@IsOptional", 1);
                                else
                                    hashtable1.Add("@IsOptional", 0);
                                if (obj.ExecuteScalar("PS_InsUpdtSubjectMaster", hashtable1).Trim().ToUpper() == "S")
                                    ++num2;
                                else
                                    ++num1;
                            }
                        }

                    }
                    else
                    {
                        Hashtable hashtable = new Hashtable();
                        hashtable.Clear();
                        hashtable.Add("@SubjectId", hfSubjectId.Value);
                        hashtable.Add("@SubjectName", txtSubject.Text.Trim().ToUpper());
                        hashtable.Add("@ClassId", listItem.Value);
                        hashtable.Add("@StreamId", 1);
                        hashtable.Add("@UserId", Session["User_Id"]);
                        if (chkOpt.Checked)
                            hashtable.Add("@IsOptional", 1);
                        else
                            hashtable.Add("@IsOptional", 0);
                        if (obj.ExecuteScalar("PS_InsUpdtSubjectMaster", hashtable).Trim().ToUpper() == "S")
                            ++num2;
                        else
                            ++num1;
                    }
                }
            }
            drpSelectClass.SelectedIndex = 0;
            ResetFields();
            GetSubjectList();
            string str = num2.ToString() + " Record(s) Saved Successfully. ";
            if (num1 > 0)
                str = str + num1 + " Record(s) Unable to Save.";
            trMsg.Style["background-color"] = "Green";
            lblMsg.Text = str;
        }
    }

    protected void btnEdit_Click(object sender, EventArgs e)
    {
        ResetFields();
        ImageButton control1 = (ImageButton)((Control)sender).Parent.Parent.FindControl("btnEdit");
        HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfClassId");
        HiddenField control5 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfStreamID");
        HiddenField control3 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfOpt");
        Label control4 = (Label)((Control)sender).Parent.Parent.FindControl("lblSubjectName");
        chkListClass.SelectedValue = control2.Value;
        if (Convert.ToInt32(control5.Value) > 1)
        {
            ChkListStream.Visible = true;
            ChkListStream.SelectedValue = control5.Value.Trim();
        }
        else
        {
            ChkListStream.Visible = false;
        }

        txtSubject.Text = control4.Text.Trim();
        if (control3.Value == "True")
            chkOpt.Checked = true;
        hfSubjectId.Value = control1.CommandArgument;
        btnSave.Text = "Update";
        chkListClass.Enabled = false;
        txtSubject.Focus();
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        ResetFields();
        ImageButton control = (ImageButton)((Control)sender).Parent.Parent.FindControl("btnDelete");
        string str = obj.ExecuteScalar("PS_DeleteSubjectMaster", new Hashtable()
    {
      {
         "@SubjectId",
         control.CommandArgument
      }
    });
        if (str.Trim().ToUpper() == "S")
        {
            trMsg.Style["background-color"] = "Green";
            lblMsg.Text = "Subject Deleted Successfully";
        }
        else
        {
            trMsg.Style["background-color"] = "Red";
            lblMsg.Text = str + control.CommandArgument;
        }
        GetSubjectList();
        drpSelectClass.Focus();
    }

    protected void drpSelectClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (drpSelectClass.SelectedItem.Text == "XI" || drpSelectClass.SelectedItem.Text == "XII")
        {
            grdSubject.Visible = false;
            BindStream();
            DrpSelectStream.Visible = true;
            DrpSelectStream.Items.RemoveAt(0);
            DrpSelectStream.Items.Insert(0, "<--Select Stream-->");
            return;
            
        }
        else
        {
            DrpSelectStream.Visible = false;
            ResetFields();
            GetSubjectList();
            drpSelectClass.Focus();
        }
        
    }

    private void ResetFields()
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        chkListClass.Enabled = true;
        BindClass();
        txtSubject.Text = string.Empty;
        btnSave.Text = "Save";
        hfSubjectId.Value = "0";
        chkOpt.Checked = false;
    }

    protected void chkListClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        trMsg.Style["background-color"] = (string)null;
        lblMsg.Text = string.Empty;
        if (chkListClass.SelectedIndex != -1)
        {

            if (chkListClass.SelectedItem.Text == "XI" || chkListClass.SelectedItem.Text == "XII")
            {
               
                BindStream();
                ChkListStream.Items.RemoveAt(0);
                ChkListStream.Visible = true;
            }
            else
            {
                ChkListStream.Visible = false;
            }
        }
        else
        {
            ChkListStream.Visible = false;
            return;
        }

    }
    protected void DrpSelectStream_SelectedIndexChanged(object sender, EventArgs e)
    {
        ResetFields();
        GetSubjectList();
        drpSelectClass.Focus();
    }


}