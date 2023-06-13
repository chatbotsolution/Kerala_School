using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class HR_SchoolMCMaster : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        litMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        try
        {
            ViewState["SchoolId"] = ConfigurationManager.AppSettings["SchoolId"].ToString().Trim();
            ViewState["MCDesignation"] = new DataTable();
            FillAllFields();
        }
        catch (Exception ex)
        {
            Response.Redirect("../Login.aspx");
        }
    }

    private void FillAllFields()
    {
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("HR_SelectSchoolMCForEdit", new Hashtable()
    {
      {
         "@SchoolId",
         ConfigurationManager.AppSettings["SchoolId"].ToString().Trim()
      }
    });
        ViewState["dt"] = dataTable2;
        if (dataTable2.Rows.Count > 0)
        {
            hfApprLtrNo.Value = txtApLtrNo.Text = dataTable2.Rows[0]["ApprovalLetterNo"].ToString();
            dtpAprDate.SetDateValue(Convert.ToDateTime(dataTable2.Rows[0]["ApprovalDate"]));
            dtpPerFrom.SetDateValue(Convert.ToDateTime(dataTable2.Rows[0]["MCPeriodStartDt"]));
            dtpPerTo.SetDateValue(Convert.ToDateTime(dataTable2.Rows[0]["MCPeriodEndDt"]));
            txtApLtrNo.Enabled = txtAprDate.Enabled = txtPerFrom.Enabled = false;
            txtPerTo.Enabled = true;
           // PopCalendar dtpAprDate = dtpAprDate;
            bool flag;
            dtpPerFrom.Enabled = flag = false;
            int num = flag ? 1 : 0;
            dtpAprDate.Enabled = num != 0;
            dtpPerTo.Enabled = true;
            ImageButton1.Visible = ImageButton2.Visible = ImageButton3.Visible = false;
            gvMC.DataSource = dataTable2;
            gvMC.DataBind();
            gvMC.Columns[6].Visible = true;
            ViewState["MCDesignation"] = dataTable2;
            hfCheck.Value = "1";
            btnNew.Visible = true;
        }
        else
        {
            hfApprLtrNo.Value = "";
            FillGridMC();
        }
    }

    private void FillGridMC()
    {
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = clsDal.GetDataTable("HR_GetMCDesignation");
        if (dataTable2.Rows.Count > 0)
        {
            gvMC.DataSource = dataTable2;
            gvMC.DataBind();
            gvMC.Columns[6].Visible = false;
            ViewState["MCDesignation"] = dataTable2;
            txtApLtrNo.Enabled = txtAprDate.Enabled = txtPerFrom.Enabled = txtPerTo.Enabled = true;
            txtApLtrNo.Text = txtAprDate.Text = txtPerFrom.Text = txtPerTo.Text = string.Empty;
            //PopCalendar dtpAprDate = dtpAprDate;
            //PopCalendar dtpPerFrom = dtpPerFrom;
            bool flag1;
            dtpPerTo.Enabled =flag1 = true;
            int num1;
            bool flag2 = (num1 = flag1 ? 1 : 0) != 0;
            dtpPerFrom.Enabled = num1 != 0;
            int num2 = flag2 ? 1 : 0;
            dtpAprDate.Enabled = num2 != 0;
            ImageButton1.Visible = ImageButton2.Visible = ImageButton3.Visible = true;
            btnNew.Visible = false;
            hfCheck.Value = "0";
        }
        else
        {
            UpdatePanel1.Visible = false;
            divNoDesg.Visible = true;
        }
    }

    private void ClearFields()
    {
        txtApLtrNo.Text = string.Empty;
        txtAprDate.Text = string.Empty;
        txtPerFrom.Text = string.Empty;
        txtPerTo.Text = string.Empty;
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        string str = "";
        if (hfCheck.Value == "0" && hfApprLtrNo.Value != "")
            str = obj.ExecuteScalar("HR_CloseExistingMC", new Hashtable()
      {
        {
           "@ApprLtrNo",
           hfApprLtrNo.Value
        },
        {
           "@ToDt",
           Convert.ToDateTime(dtpPerFrom.GetDateValue().AddDays(-1.0).ToString("MMM-dd-yyyy"))
        }
      });
        if (str.Trim().Equals(string.Empty))
            str = InsertUpdateTable();
        if (str.Trim().Equals(string.Empty))
        {
            FillAllFields();
            litMsg.Text = "<div style='background-color:green;color:white;padding:3px;' align='center'>Data Saved Successfully</div>";
        }
        else
            litMsg.Text = "<div style='background-color:red;color:white;padding:3px;'>Problem Found in Following Records :-<br>" + str + "</div>";
    }

    private string InsertUpdateTable()
    {
        bool flag = false;
        foreach (Control row in gvMC.Rows)
        {
            if (!((TextBox)row.FindControl("txtEmployee")).Text.Trim().Equals(string.Empty))
            {
                flag = true;
                break;
            }
        }
        if (!flag)
            return "Provide Data for at-least One Row";
        DataTable dataTable = new DataTable();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SSVMID", ViewState["SchoolId"]);
        hashtable.Add("@ApprovalLetterNo", txtApLtrNo.Text.Trim());
        hashtable.Add("@ApprovalDate", Convert.ToDateTime(dtpAprDate.GetDateValue().ToString("MMM-dd-yyyy")));
        hashtable.Add("@FromDt", Convert.ToDateTime(dtpPerFrom.GetDateValue().ToString("MMM-dd-yyyy")));
        hashtable.Add("@UserId", Session["User_Id"]);
        hashtable.Add("@IsApproved", "A");
        hashtable.Add("@MCPeriodStartDt", Convert.ToDateTime(dtpPerFrom.GetDateValue().ToString("MMM-dd-yyyy")));
        hashtable.Add("@MCPeriodEndDt", Convert.ToDateTime(dtpPerTo.GetDateValue().ToString("MMM-dd-yyyy")));
        string str1 = string.Empty;
        foreach (GridViewRow row in gvMC.Rows)
        {
            string empty = string.Empty;
            HiddenField control1 = (HiddenField)row.FindControl("hfDesgId");
            HiddenField control2 = (HiddenField)row.FindControl("hfSortOrder");
            TextBox control3 = (TextBox)row.FindControl("txtEmployee");
            TextBox control4 = (TextBox)row.FindControl("txtContactNo");
            TextBox control5 = (TextBox)row.FindControl("txtEmailID");
            TextBox control6 = (TextBox)row.FindControl("txtAddress");
            string str2 = gvMC.DataKeys[row.DataItemIndex].Value.ToString().Trim();
            if (!str2.Trim().Equals(string.Empty))
                hashtable.Add("@ReconstId", str2);
            hashtable.Add("@DesignationId", control1.Value);
            hashtable.Add("@Name", control3.Text.Trim());
            hashtable.Add("@ContactTel", control4.Text.Trim());
            hashtable.Add("@EmailId", control5.Text.Trim());
            hashtable.Add("@Address", control6.Text.Trim());
            string str3 = obj.ExecuteScalar("HR_InstUpdtSchoolMCRecon", hashtable);
            if (!str3.Trim().Equals(string.Empty))
            {
                str1 = str1 + control3.Text + " : " + str3;
                if (!str1.Trim().Equals(string.Empty))
                    str1 += "<br/>";
            }
            hashtable.Remove("@DesignationId");
            hashtable.Remove("@Name");
            hashtable.Remove("@ContactTel");
            hashtable.Remove("@EmailId");
            hashtable.Remove("@Address");
            if (hashtable["@ReconstId"] != null)
                hashtable.Remove("@ReconstId");
        }
        return str1;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("HRHome.aspx");
    }

    private void SaveData()
    {
        DataTable dataTable = (DataTable)ViewState["MCDesignation"];
        foreach (GridViewRow row1 in gvMC.Rows)
        {
            int rowIndex = row1.RowIndex;
            TextBox control1 = (TextBox)row1.FindControl("txtEmployee");
            TextBox control2 = (TextBox)row1.FindControl("txtContactNo");
            TextBox control3 = (TextBox)row1.FindControl("txtEmailID");
            TextBox control4 = (TextBox)row1.FindControl("txtAddress");
            DataRow row2 = dataTable.Rows[rowIndex];
            row2["Name"] = control1.Text;
            row2["ContactTel"] = control2.Text;
            row2["EmailId"] = control3.Text;
            row2["Address"] = control4.Text;
            dataTable.AcceptChanges();
        }
        dataTable.AcceptChanges();
        ViewState["MCDesignation"] = dataTable;
    }

    protected void btnAdd_Click(object sender, ImageClickEventArgs e)
    {
        SaveData();
        DataTable dataTable = (DataTable)ViewState["MCDesignation"];
        HiddenField control1 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfDesgId");
        HiddenField control2 = (HiddenField)((Control)sender).Parent.Parent.FindControl("hfSortOrder");
        Label control3 = (Label)((Control)sender).Parent.Parent.FindControl("lblDesignation");
        DataRow row = dataTable.NewRow();
        row["Designation"] = control3.Text.Trim();
        row["DesignationId"] = control1.Value.ToString();
        if (!control2.Value.ToString().Trim().Equals(string.Empty))
            row["SortOrder"] = control2.Value.ToString();
        dataTable.Rows.Add(row);
        dataTable.AcceptChanges();
        DataView defaultView = dataTable.DefaultView;
        defaultView.Sort = "SortOrder,DesignationId";
        ViewState["MCDesignation"] = defaultView.ToTable();
        gvMC.DataSource = defaultView;
        gvMC.DataBind();
    }

    protected void btnRemove_Click(object sender, ImageClickEventArgs e)
    {
        DataTable dataTable = (DataTable)ViewState["MCDesignation"];
        ImageButton control = (ImageButton)((Control)sender).Parent.Parent.FindControl("btnRemove");
        dataTable.Rows.RemoveAt(Convert.ToInt32(control.CommandArgument));
        dataTable.AcceptChanges();
        DataView defaultView = dataTable.DefaultView;
        defaultView.Sort = "SortOrder,DesignationId";
        ViewState["MCDesignation"] = defaultView.ToTable();
        gvMC.DataSource = defaultView;
        gvMC.DataBind();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        FillGridMC();
    }

    protected void btnDesg_Click(object sender, EventArgs e)
    {
        Response.Redirect("Designation.aspx");
    }

    protected void gvMC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;
        HiddenField control1 = (HiddenField)e.Row.FindControl("hfReconstId");
        HiddenField control2 = (HiddenField)e.Row.FindControl("hfDesgId");
        ImageButton control3 = (ImageButton)e.Row.FindControl("btnAdd");
        ImageButton control4 = (ImageButton)e.Row.FindControl("btnRemove");
        if (((DataTable)ViewState["dt"]).Rows.Count > 0)
        {
            if (control2.Value.ToString() == "6" || control2.Value.ToString().Trim() == "7")
            {
                if (control1.Value.Trim() != string.Empty)
                {
                    control3.Visible = true;
                    control4.Visible = false;
                }
                else
                {
                    control3.Visible = true;
                    control4.Visible = true;
                }
            }
            else
            {
                control3.Visible = false;
                control4.Visible = false;
            }
        }
        else if (control2.Value.ToString() == "6" || control2.Value.ToString().Trim() == "7")
        {
            control3.Visible = true;
            control4.Visible = true;
        }
        else
        {
            control3.Visible = false;
            control4.Visible = false;
        }
    }
}