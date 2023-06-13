using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Hostel_HostelFeeConcession : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Page.IsPostBack)
            return;
        Page.Form.DefaultButton = btnsave.UniqueID;
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        fillsession();
        fillclass();
        PopCalendar2.To.Date = DateTime.Today;
        txtadmnno.ReadOnly = false;
        drpclass.Enabled = true;
        drpSession.Enabled = true;
        drpstudent.Enabled = true;
        btnBack.Visible = false;
        btncancel.Visible = true;
        if (Request.QueryString["admnno"] != null)
        {
            drpSession.SelectedValue = Request.QueryString["sess"].ToString();
            drpclass.SelectedValue = Request.QueryString["cid"].ToString();
            fillstudent();
            drpstudent.SelectedValue = Request.QueryString["admnno"];
            txtadmnno.Text = Request.QueryString["admnno"];
            txtadmnno.ReadOnly = true;
            drpclass.Enabled = false;
            drpSession.Enabled = false;
            drpstudent.Enabled = false;
            btnBack.Visible = true;
            btncancel.Visible = false;
        }
        if (Request.QueryString["aid"] == null)
            return;
        FillAdjustmentDetails();
        txtadmnno.ReadOnly = true;
        drpclass.Enabled = false;
        drpSession.Enabled = false;
        drpstudent.Enabled = false;
        btnBack.Visible = false;
        btncancel.Visible = true;
    }

    private void FillAdjustmentDetails()
    {
        obj = new clsDAL();
        DataSet datasetQry = obj.GetDatasetQry("select *,Convert(Decimal(18,2),Amount) as Amt,convert(varchar(20),AdjDate,103) as recvddate from dbo.Host_FeeAdjustment where AdjustmentId=" + Convert.ToInt32(Request.QueryString["aid"].ToString()));
        txtadmnno.Text = datasetQry.Tables[0].Rows[0]["AdmnNo"].ToString();
        txtdate.Text = datasetQry.Tables[0].Rows[0]["recvddate"].ToString();
        txtdesc.Text = datasetQry.Tables[0].Rows[0]["Reason"].ToString();
        txtamt.Text = datasetQry.Tables[0].Rows[0]["Amt"].ToString();
        FillStudDetailsAsperAdmnNo();
    }

    private void FillStudDetailsAsperAdmnNo()
    {
        obj = new clsDAL();
        try
        {
            fillstudentSearch();
            drpstudent.SelectedValue = txtadmnno.Text;
            fillclassSearch();
            drpclass.SelectedValue = obj.GetDataTableQry("select section,ClassID from PS_ClasswiseStudent c inner join dbo.Host_Admission h on h.AdmnNo=c.AdmnNo and LeavingDt is null where c.admnno=" + txtadmnno.Text).Rows[0]["ClassID"].ToString();
        }
        catch (Exception ex)
        {
            if (!(ex.Message != ""))
                return;
            ScriptManager.RegisterClientScriptBlock((Control)txtadmnno, txtadmnno.GetType(), "ShowMessage", "alert('Invalid Admission Number')", true);
        }
    }

    private void fillclass()
    {
        drpclass.Items.Clear();
        obj = new clsDAL();
        drpclass.DataSource = obj.GetDataTable("ps_sp_get_classesForDDL");
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("All", "0"));
    }

    private void fillstudent()
    {
        obj = new clsDAL();
        DataTable dataTableQry;
        if (drpclass.SelectedIndex != 0)
            dataTableQry = obj.GetDataTableQry("select fullname,cs.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno inner join dbo.Host_Admission h on h.AdmnNo=cs.AdmnNo and LeavingDt is null where [Status]=1 and cs.classid=" + drpclass.SelectedValue + " and cs.SessionYear='" + drpSession.SelectedValue.ToString() + "' order by fullname");
        else
            dataTableQry = obj.GetDataTableQry("select fullname,cs.admnno from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno inner join dbo.Host_Admission h on h.AdmnNo=cs.AdmnNo and LeavingDt is null where [Status]=1 and cs.SessionYear='" + drpSession.SelectedValue.ToString() + "' order by fullname");
        drpstudent.DataSource = dataTableQry;
        drpstudent.DataTextField = "fullname";
        drpstudent.DataValueField = "admnno";
        drpstudent.DataBind();
        if (dataTableQry.Rows.Count > 0)
            drpstudent.Items.Insert(0, new ListItem("--Select--", "0"));
        else
            drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
    }

    private void fillstudentSearch()
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry("select * from PS_ClasswiseStudent cs join ps_studmaster s on s.admnno=cs.admnno inner join dbo.Host_Admission h on h.AdmnNo=cs.AdmnNo and LeavingDt is null where [Status]=1 order by fullname");
        drpstudent.DataSource = dataTableQry;
        drpstudent.DataTextField = "fullname";
        drpstudent.DataValueField = "admnno";
        drpstudent.DataBind();
        if (dataTableQry.Rows.Count > 0)
            return;
        drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
        txtadmnno.Text = "";
    }

    private void fillclassSearch()
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry("select * from ps_classmaster");
        drpclass.DataSource = dataTableQry;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        if (dataTableQry.Rows.Count > 0)
            return;
        drpclass.Items.Insert(0, new ListItem("No Record", "0"));
    }

    private void fillsession()
    {
        obj = new clsDAL();
        drpSession.DataSource = obj.GetDataTableQry("select SessionID,SessionYr from dbo.PS_FeeNormsNew order by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    protected void drpSession_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
        txtadmnno.Text = string.Empty;
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
        txtadmnno.Text = string.Empty;
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtadmnno.Text = drpstudent.SelectedValue;
        if (rBtnFeeHeadAmt.Checked.Equals(true))
        {
            tdAmt.Visible = false;
            if (drpstudent.SelectedIndex <= 0 || !(drpstudent.SelectedValue != ""))
                return;
            FillGrid(1);
        }
        else
        {
            if (!rBtnMlyAmt.Checked.Equals(true))
                return;
            tdAmt.Visible = false;
            if (drpstudent.SelectedIndex <= 0 || !(drpstudent.SelectedValue != ""))
                return;
            FillGrid(2);
        }
    }

    private void FillGrid(int FeeType)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        hashtable.Add("@AdmnNo", drpstudent.SelectedValue.ToString().Trim());
        hashtable.Add("@SessionYr", drpSession.SelectedValue.ToString().Trim());
        DataTable dataTable2;
        if (FeeType == 1)
        {
            dataTable2 = clsDal.GetDataTable("Host_RegularFeeDueForConcession", hashtable);
        }
        else
        {
            hashtable.Remove("@SessionYr");
            dataTable2 = clsDal.GetDataTable("Host_FeeDueListForInstConcession", hashtable);
        }
        if (dataTable2.Rows.Count > 0)
        {
            grdRegFeeAmt.DataSource = dataTable2;
            grdRegFeeAmt.DataBind();
        }
        else
        {
            lblMsg.Text = "No Amount Pending For Concession";
            grdRegFeeAmt.DataSource = null;
            grdRegFeeAmt.DataBind();
        }
    }

    protected void btnDetail_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dataTableQry = new clsDAL().GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent c inner join dbo.Host_Admission h on h.AdmnNo=c.AdmnNo and LeavingDt is null where c.AdmnNo=" + txtadmnno.Text + " and Detained_Promoted='' and StatusId=1");
            if (dataTableQry.Rows.Count > 0)
            {
                drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
                drpclass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
                fillstudent();
                drpstudent.SelectedValue = txtadmnno.Text;
                if (rBtnFeeHeadAmt.Checked.Equals(true))
                {
                    tdAmt.Visible = false;
                    if (drpstudent.SelectedIndex <= 0 || !(drpstudent.SelectedValue != ""))
                        return;
                    FillGrid(1);
                }
                else
                {
                    if (!rBtnMlyAmt.Checked.Equals(true))
                        return;
                    tdAmt.Visible = false;
                    if (drpstudent.SelectedIndex <= 0 || !(drpstudent.SelectedValue != ""))
                        return;
                    FillGrid(2);
                }
            }
            else
                ScriptManager.RegisterClientScriptBlock((Control)txtadmnno, txtadmnno.GetType(), "ShowMessage", "alert('Admission number does not exists')", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void rBtnFeeHeadAmt_CheckedChanged(object sender, EventArgs e)
    {
        if (rBtnFeeHeadAmt.Checked.Equals(true))
        {
            tdAmt.Visible = false;
            btnCopyAll.Visible = true;
            if (drpstudent.SelectedIndex <= 0 || !(drpstudent.SelectedValue != ""))
                return;
            FillGrid(1);
        }
        else if (rBtnMlyAmt.Checked.Equals(true))
        {
            tdAmt.Visible = false;
            btnCopyAll.Visible = true;
            if (drpstudent.SelectedIndex <= 0 || !(drpstudent.SelectedValue != ""))
                return;
            FillGrid(2);
        }
        else
        {
            tdAmt.Visible = true;
            btnCopyAll.Visible = false;
            grdRegFeeAmt.DataSource = null;
            grdRegFeeAmt.DataBind();
        }
    }

    protected void btnCopyAll_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in grdRegFeeAmt.Rows)
            ((TextBox)row.FindControl("txtConsAmt")).Text = ((Label)row.FindControl("lblFineApplicable")).Text.Trim();
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        double ConPer = 0.0;
        double NoOfRec = 0.0;
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        try
        {
            if (rBtnFeeHeadAmt.Checked.Equals(false))
            {
                if (rBtnMlyAmt.Checked.Equals(false))
                    ConPer = Convert.ToDouble(txtamt.Text);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock((Control)btnsave, btnsave.GetType(), "ShowMessage", "alert('Enter correct amount');", true);
            return;
        }
        if (rBtnMlyAmt.Checked)
        {
            ReGenFeeOnInstantRegFeeAmt(out NoOfRec);
            grdRegFeeAmt.DataSource = null;
            grdRegFeeAmt.DataBind();
        }
        else if (rBtnPer.Checked)
        {
            NoOfRec = PerWiseFeeCons(txtadmnno.Text.Trim(), PopCalendar2.GetDateValue().ToString("MM/dd/yyyy"), txtdesc.Text.Trim(), Session["User_Id"].ToString(), ConPer);
        }
        else
        {
            ReGenFeeOnInstantRegFeeAmt(out NoOfRec);
            grdRegFeeAmt.DataSource = null;
            grdRegFeeAmt.DataBind();
        }
        if (NoOfRec > 0.0)
        {
            lblMsg.Text = "Discount Updated Successfully";
            lblMsg.ForeColor = Color.Green;
            txtamt.Text = "";
            txtdesc.Text = "";
            PopCalendar2.To.Date = DateTime.Today;
        }
        else if (NoOfRec < 0.0)
        {
            NoOfRec *= -1.0;
            lblMsg.Text = "Discount Updated Successfully. After all discount adjusted in fee ledger the balance amount Rs. " + NoOfRec.ToString() + " is entered in student advance ledger.";
            lblMsg.ForeColor = Color.Green;
            ClearFields();
        }
        else
        {
            lblMsg.Text = "Discount could not be updated as fee already received or discount already made.";
            lblMsg.ForeColor = Color.Red;
        }
    }

    public double PerWiseFeeCons(string admnno, string ConDate, string reason, string uid, double ConPer)
    {
        DataTable dataTable = new clsDAL().GetDataTable("ps_sp_FeeDueListForPerCons", new Hashtable()
    {
      {
         "@AdmnNo",
         admnno
      },
      {
         "@FeeTyp",
         drpConsession.SelectedIndex.ToString()
      }
    });
        double num1 = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
        {
            double num2 = Convert.ToDouble(row["Debit"]) * (ConPer / 100.0);
            double num3 = Convert.ToDouble(row["Debit"]) - num2;
            double num4 = Convert.ToDouble(row["Balance"]) - num2;
            long int64 = Convert.ToInt64(row["TransNo"]);
            new clsDAL().ExcuteQryInsUpdt("update dbo.Host_FeeLedger set Debit=" + num3 + ", Balance=" + num4 + " where TransNo=" + int64);
            new clsDAL().ExcuteProcInsUpdt("Host_InsFeeAdjustment", new Hashtable()
      {
        {
           "AdmnNo",
           admnno
        },
        {
           "AdjDate",
           ConDate
        },
        {
           "Reason",
           reason
        },
        {
           "FeeLedgerTransNo",
           int64
        },
        {
           "ConAmount",
           num2
        },
        {
           "UID",
           uid
        }
      });
            ++num1;
        }
        return num1;
    }

    private void ReGenFeeOnInstantRegFeeAmt(out double NoOfRec)
    {
        NoOfRec = 0.0;
        if (grdRegFeeAmt.Rows.Count <= 0)
            return;
        double num1 = 0.0;
        foreach (GridViewRow row in grdRegFeeAmt.Rows)
        {
            TextBox control1 = (TextBox)row.FindControl("txtConsAmt");
            HiddenField control2 = (HiddenField)row.FindControl("hfActualAmt");
            string str = grdRegFeeAmt.DataKeys[row.DataItemIndex].Value.ToString();
            if (control1.Text.Trim() != "" && Convert.ToInt32(control1.Text.Trim()) > 0)
            {
                double num2 = Convert.ToDouble(control2.Value) - Convert.ToDouble(control1.Text);
                double num3 = Convert.ToDouble(control2.Value) - Convert.ToDouble(control1.Text);
                long int64 = Convert.ToInt64(str);
                obj = new clsDAL();
                obj.ExcuteQryInsUpdt("update dbo.Host_FeeLedger set Debit=" + num2 + ", Balance=" + num3 + " where TransNo=" + int64);
                Hashtable hashtable = new Hashtable();
                hashtable.Add("AdmnNo", drpstudent.SelectedValue.ToString().Trim());
                hashtable.Add("AdjDate", PopCalendar2.GetDateValue().ToString("MM/dd/yyyy"));
                hashtable.Add("Reason", txtdesc.Text.Trim());
                hashtable.Add("FeeLedgerTransNo", int64);
                hashtable.Add("ConAmount", Convert.ToDouble(control1.Text));
                hashtable.Add("UID", Session["User_Id"].ToString());
                obj = new clsDAL();
                obj.ExcuteProcInsUpdt("Host_InsFeeAdjustment", hashtable);
                ++num1;
            }
        }
        NoOfRec = num1;
    }

    private void ClearFields()
    {
        drpSession.SelectedIndex = 0;
        drpclass.SelectedIndex = 0;
        drpstudent.SelectedIndex = 0;
        txtadmnno.Text = "";
        txtamt.Text = "";
        txtdesc.Text = "";
        PopCalendar2.To.Date = DateTime.Today;
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Hostel/HostelHome.aspx");
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        if (Request.QueryString["Sw"] != null)
            Response.Redirect("FeeReceiptHostel.aspx?admnno=" + drpstudent.SelectedValue + "&sess=" + Request.QueryString["sess"].ToString() + "&cid=" + Request.QueryString["cid"].ToString() + "&D=p&Sw=" + Request.QueryString["Sw"]);
        else
            Response.Redirect("FeeReceiptHostel.aspx?admnno=" + drpstudent.SelectedValue + "&sess=" + Request.QueryString["sess"].ToString() + "&cid=" + Request.QueryString["cid"].ToString() + "&D=p");
    }

    protected void grdRegFeeAmt_PreRender(object sender, EventArgs e)
    {
        ClientScriptManager clientScript = Page.ClientScript;
        foreach (GridViewRow row in grdRegFeeAmt.Rows)
        {
            TextBox control1 = (TextBox)row.FindControl("txtConsAmt");
            HiddenField control2 = (HiddenField)row.FindControl("hfActualAmt");
            clientScript.RegisterArrayDeclaration("ConcAmt", "'" + control1.ClientID + "'");
            clientScript.RegisterArrayDeclaration("FeeAmt", "'" + control2.ClientID + "'");
        }
    }
}