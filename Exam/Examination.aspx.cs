using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Exam_Examination : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private DataTable dt;
    private Hashtable ht;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            if (Page.IsPostBack)
                return;
            bindClass();
            ViewState["eid"] = string.Empty;
            trMsg.BgColor = "Transparent";
            lblMsg.Text = string.Empty;
            trMsg.BgColor = "Transparent";
            Session["title"] = Page.Title.ToString();
            BindSession();
            if (Request.QueryString["eid"] == null)
                return;
            FillAllFields();
            ViewState["eid"] = "Update";
        }
        else
            Response.Redirect("~/Login.aspx");
    }

    private void BindSession()
    {
        DataTable dataTableQry = obj.GetDataTableQry("SELECT DISTINCT SessionId,SessionYr FROM dbo.PS_FeeNormsNew ORDER BY SessionId DESC");
        if (dataTableQry.Rows.Count == 0)
        {
            DataRow row1 = dataTableQry.NewRow();
            row1["SessionYr"] = (DateTime.Now.AddYears(-1).ToString("yyyy") + "-" + DateTime.Now.ToString("yy"));
            dataTableQry.Rows.Add(row1);
            DataRow row2 = dataTableQry.NewRow();
            row2["SessionYr"] = (DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.AddYears(1).ToString("yy"));
            dataTableQry.Rows.Add(row2);
            DataRow row3 = dataTableQry.NewRow();
            row3["SessionYr"] = (DateTime.Now.AddYears(1).ToString("yyyy") + "-" + DateTime.Now.AddYears(2).ToString("yy"));
            dataTableQry.Rows.Add(row3);
            dataTableQry.AcceptChanges();
        }
        drpSession.DataSource = dataTableQry;
        drpSession.DataValueField = "SessionYr";
        drpSession.DataTextField = "SessionYr";
        drpSession.DataBind();
        drpSession.Items.Insert(0, new ListItem("- Select -", "0"));
    }

    private void bindClass()
    {
        DataTable dataTable = new DataTable();
        chklClass.DataSource = obj.GetDataTableQry("select ClassID, ClassName from dbo.PS_ClassMaster").DefaultView;
        chklClass.DataTextField = "ClassName";
        chklClass.DataValueField = "ClassID";
        chklClass.DataBind();
        chklClass.Items.Insert(0, new ListItem("ALL", "0"));
    }

    private void ClearAllFields()
    {
        trMsg.BgColor = "Transparent";
        lblMsg.Text = string.Empty;
        chklClass.ClearSelection();
        drpSession.SelectedIndex = 0;
        txtExamName.Text = string.Empty;
        drpExamTyp.SelectedIndex = 0;
        txtFromDate.Text = string.Empty;
        txtToDate.Text = string.Empty;
        txtPassPercent.Text = "0";
    }

    private void InstToTable()
    {
        clsDAL clsDal = new clsDAL();
        dt = new DataTable();
        ht = new Hashtable();
        try
        {
            if (dtpFromDate.GetDateValue() > dtptoDate.GetDateValue())
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "ShowMessage", "alert('Enter date is not valid');", true);
                txtFromDate.Text = "";
                txtToDate.Text = "";
            }
            else
            {
                if (Request.QueryString["eid"] != null && ViewState["eid"].ToString() == "Update")
                    ht.Add("ExamId", Request.QueryString["eid"].ToString());
                ht.Add("@SessionYr", drpSession.SelectedValue.ToString());
                ht.Add("@ExamName", txtExamName.Text.Trim());
                ht.Add("@ExamType", drpExamTyp.SelectedValue.ToString());
                ht.Add("@ExamFromDate", dtpFromDate.GetDateValue());
                ht.Add("@ExamToDate", dtptoDate.GetDateValue());
                ht.Add("@PassPercent", txtPassPercent.Text);
                if (rbnYes.Checked)
                    ht.Add("PassAllSubRequired", "1");
                if (rbnNo.Checked)
                    ht.Add("PassAllSubRequired", "0");
                if (rbnActive.Checked)
                    ht.Add("ActiveStatus", "1");
                if (rbnInAct.Checked)
                    ht.Add("ActiveStatus", "0");
                ht.Add("@UserId", Session["User_Id"]);
                dt = clsDal.GetDataTable("ExamInstUpdtExamDetails", ht);
                if (dt.Rows.Count > 0 && dt.Rows[0][0].ToString().Trim() != "DUP")
                {
                    string str = dt.Rows[0][0].ToString().Trim();
                    clsDal.ExecuteScalarQry("delete from dbo.ExamClass where ExamId=" + str);
                    if (chklClass.SelectedIndex == 0)
                    {
                        for (int index = 0; index <= chklClass.Items.Count - 1; ++index)
                        {
                            if (chklClass.Items[index].Value != "0")
                            {
                                Hashtable hashtable = new Hashtable();
                                hashtable.Clear();
                                hashtable.Add("@ExamId", str);
                                hashtable.Add("@ForClassId", Convert.ToInt32(chklClass.Items[index].Value));
                                clsDal.ExecuteScalar("ExamInsExamClass", hashtable);
                            }
                        }
                    }
                    else
                    {
                        for (int index = 0; index <= chklClass.Items.Count - 1; ++index)
                        {
                            if (chklClass.Items[index].Selected)
                            {
                                Hashtable hashtable = new Hashtable();
                                hashtable.Clear();
                                hashtable.Add("@ExamId", str);
                                hashtable.Add("@ForClassId", Convert.ToInt32(chklClass.Items[index].Value));
                                dt = clsDal.GetDataTable("ExamInsExamClass", hashtable);
                            }
                        }
                    }
                    ClearAllFields();
                    trMsg.BgColor = "Green";
                    lblMsg.Text = "Data Saved Successfully.";
                    ViewState["eid"] = string.Empty;
                }
                else
                {
                    trMsg.BgColor = "Red";
                    lblMsg.Text = "Examination Name already Exists";
                }
            }
        }
        catch (Exception ex)
        {
            trMsg.BgColor = "Red";
            lblMsg.Text = "Failed to Save. Try Again.";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        InstToTable();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ClearAllFields();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
        InstToTable();
        if (!trMsg.BgColor.Equals("Green"))
            return;
        ScriptManager.RegisterClientScriptBlock((Control)btnShow, btnShow.GetType(), "Message", "alert('Data saved successfully !');window.location='ExaminationList.aspx';", true);
    }

    private void FillAllFields()
    {
        dt = new DataTable();
        clsDAL clsDal = new clsDAL();
        ht = new Hashtable();
        ht.Add("@ExamId", Request.QueryString["eid"].ToString());
        dt = clsDal.GetDataTable("ExamSelectExamDetails", ht);
        drpSession.SelectedValue = dt.Rows[0]["SessionYr"].ToString();
        txtExamName.Text = dt.Rows[0]["ExamName"].ToString();
        drpExamTyp.SelectedValue = dt.Rows[0]["ExamType"].ToString().Trim();
        if (dt.Rows[0]["ExamFromDate"] != null && dt.Rows[0]["ExamFromDate"].ToString() != "")
            txtFromDate.Text = Convert.ToDateTime(dt.Rows[0]["ExamFromDate"].ToString()).ToString("dd-MMM-yyyy");
        if (dt.Rows[0]["ExamToDate"] != null && dt.Rows[0]["ExamToDate"].ToString() != "")
            txtToDate.Text = Convert.ToDateTime(dt.Rows[0]["ExamToDate"].ToString()).ToString("dd-MMM-yyyy");
        chklClass.SelectedValue = dt.Rows[0]["ExamClass"].ToString();
        DataTable dataTable = new DataTable();
        DataTable dataTableQry = clsDal.GetDataTableQry("Select ForClassId from dbo.ExamClass where ExamId=" + Request.QueryString["eid"].ToString());
        if (dataTableQry.Rows.Count > 0)
        {
            foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
            {
                for (int index = 0; index <= chklClass.Items.Count - 1; ++index)
                {
                    if (chklClass.Items[index].Value == row["ForClassId"].ToString())
                        chklClass.Items[index].Selected = true;
                }
            }
        }
        if (Convert.ToBoolean(dt.Rows[0]["PassAllSubRequired"]))
            rbnYes.Checked = true;
        else
            rbnNo.Checked = true;
        txtPassPercent.Text = dt.Rows[0]["PassPercent"].ToString();
        if (Convert.ToBoolean(dt.Rows[0]["ActiveStatus"]))
            rbnActive.Checked = true;
        else
            rbnInAct.Checked = true;
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("ExaminationLIst.aspx");
    }

    protected void chklClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        trMsg.BgColor = "Transparent";
        lblMsg.Text = string.Empty;
    }
}