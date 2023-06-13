using ASP;
using RJS.Web.WebControl;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Hostel_HostelAdmission : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        fillsession();
        fillclass();
        FillSectionDropDown();
        ViewState["Admn"] = string.Empty;
        ViewState["cnt"] = "0";
        if (Request.QueryString["sid"] == null)
            return;
        DispStudInfo();
        btnFeeRcv.Enabled = false;
        ViewState["Admn"] = "Update";
    }

    private void DispStudInfo()
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        DataTable dataTable = new DataTable();
        hashtable.Add("@AdmnNo", Request.QueryString["sid"]);
        hashtable.Add("@SessionYr", Request.QueryString["sy"].ToString());
        showdata(clsDal.GetDataTable("Host_GetStudentinfo", hashtable));
    }

    private void showdata(DataTable dt)
    {
        drpSession.SelectedValue = dt.Rows[0]["SessionYr"].ToString();
        drpclass.SelectedValue = dt.Rows[0]["ClassID"].ToString();
        ViewState["cnt"] = dt.Rows[0]["cnt"].ToString();
        fillstudent();
        drpstudent.SelectedValue = dt.Rows[0]["AdmnNo"].ToString();
        txtadmnno.Text = drpstudent.SelectedValue.Trim();
        InitializeCulture();
        txtFeeConcession.Text = dt.Rows[0]["FeeConcessionPer"].ToString();
        txtConceReason.Text = dt.Rows[0]["Reason"].ToString();
        dtpAdmnDt.SetDateValue(DateTime.Parse(dt.Rows[0]["AdmissionDt"].ToString(), (IFormatProvider)CultureInfo.InvariantCulture));
        drpSession.Enabled = false;
        drpclass.Enabled = false;
        drpstudent.Enabled = false;
        txtadmnno.Enabled = false;
        btnDetail.Enabled = false;
        drpFeeStartFrom.Enabled = false;
        txtFeeConcession.Enabled = false;
        txtConceReason.Enabled = false;
        FillGrid("Edit");
        btnClear.Enabled = false;
        btnFeeRcv.Enabled = true;
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
        drpstudent.Items.Clear();
        drpclass.SelectedIndex = 0;
        txtadmnno.Text = string.Empty;
        drpSession.Focus();
        ClearGrid();
    }

    private void FillSectionDropDown()
    {
        drpSection.DataSource = new clsDAL().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "classId",
         drpclass.SelectedValue
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

    private void fillclass()
    {
        obj = new clsDAL();
        DataTable dataTableQry = obj.GetDataTableQry("select * from ps_classmaster");
        drpclass.DataSource = dataTableQry;
        drpclass.DataTextField = "classname";
        drpclass.DataValueField = "classid";
        drpclass.DataBind();
        drpclass.Items.Insert(0, new ListItem("- Select -", "0"));
        if (dataTableQry.Rows.Count > 0)
            return;
        drpclass.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void drpclass_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
        drpclass.Focus();
        ClearGrid();
    }

    private void fillstudent()
    {
        obj = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select fullname,s.admnno from PS_ClasswiseStudent cs inner join ps_studmaster s on ");
        stringBuilder.Append(" s.admnno=cs.admnno ");
        stringBuilder.Append("inner join dbo.BusHostelChoice bh on bh.AdmnNo=s.admnno and SessionYr='" + drpSession.SelectedValue.Trim().ToString() + "'");
        stringBuilder.Append(" where HostelFacility='Y' and Status=1");
        if (drpclass.SelectedIndex != 0)
            stringBuilder.Append(" and cs.classid=" + drpclass.SelectedValue);
        if (Request.QueryString["sid"] == null)
            stringBuilder.Append(" and s.admnno not in (select AdmnNo from dbo.Host_Admission where SessionYr='" + drpSession.SelectedValue.ToString().Trim() + "')");
        //stringBuilder.Append(" and cs.SessionYear='" + drpSession.SelectedValue.ToString().Trim() + "' and  Detained_Promoted='' order by fullname ");
        stringBuilder.Append(" and cs.SessionYear='" + drpSession.SelectedValue.ToString().Trim() + "'  order by fullname ");
        DataTable dataTableQry = obj.GetDataTableQry(stringBuilder.ToString().Trim());
        try
        {
            drpstudent.DataSource = dataTableQry;
            drpstudent.DataTextField = "fullname";
            drpstudent.DataValueField = "admnno";
            drpstudent.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        if (dataTableQry.Rows.Count > 0)
        {
            drpstudent.Items.Insert(0, new ListItem("- Select -", "0"));
        }
        else
        {
            drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
            txtadmnno.Text = string.Empty;
        }
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        txtadmnno.Text = drpstudent.SelectedIndex <= 0 ? string.Empty : drpstudent.SelectedValue;
        drpstudent.Focus();
        ClearGrid();
    }

    private void FillGrid(string Typ)
    {
        Hashtable hashtable = new Hashtable();
        DataTable dataTable1 = new DataTable();
        obj = new clsDAL();
        hashtable.Add("@SessionYr", drpSession.SelectedValue.Trim());
        hashtable.Add("@ClassId", drpclass.SelectedValue.Trim());
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue.Trim());
        if (drpstudent.SelectedIndex > 0)
            hashtable.Add("@Admnno", drpstudent.SelectedValue.Trim());
        if (Typ.Trim() != "")
            hashtable.Add("@Type", 1);
        DataTable dataTable2 = obj.GetDataTable("Host_GetStudForAdmn", hashtable);
        if (dataTable2.Rows.Count > 0)
        {
            grdstuddet.DataSource = dataTable2;
            grdstuddet.DataBind();
            lblCount.Text = "No Of Records:" + dataTable2.Rows.Count.ToString();
        }
        else
        {
            grdstuddet.DataSource = null;
            grdstuddet.DataBind();
            lblCount.Text = "";
        }
    }

    private void ClearGrid()
    {
        grdstuddet.DataSource = null;
        grdstuddet.DataBind();
    }

    protected void btnDetail_Click(object sender, EventArgs e)
    {
        try
        {
            obj = new clsDAL();
            DataTable dataTableQry = obj.GetDataTableQry("select SessionYear,ClassID from PS_ClasswiseStudent cs inner join PS_StudMaster sm on sm.AdmnNo=cs.AdmnNo inner join dbo.BusHostelChoice bh on bh.AdmnNo=cs.admnno and cs.SessionYear=bh.SessionYr where cs.AdmnNo=" + txtadmnno.Text.Trim() + " and Detained_Promoted='' and Status=1 and HostelFacility='Y' ");
            if (dataTableQry.Rows.Count > 0)
            {
                drpSession.SelectedValue = dataTableQry.Rows[0]["SessionYear"].ToString();
                drpclass.SelectedValue = dataTableQry.Rows[0]["ClassID"].ToString();
                fillstudent();
                drpstudent.SelectedValue = txtadmnno.Text.Trim();
                txtadmnno.Focus();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock((Control)txtadmnno, txtadmnno.GetType(), "ShowMessage", "alert('Admission Number does not Exists')", true);
                drpstudent.Items.Clear();
                txtadmnno.Focus();
                return;
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
        UpdatePanel1.Update();
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        obj = new clsDAL();
        string str;
        try
        {
            str = obj.ExecuteScalarQry("select count(*) from Host_FeeAmount where ClassId=" + drpclass.SelectedValue.Trim() + " and SessionYr='" + drpSession.SelectedValue.Trim() + "'");
        }
        catch
        {
            str = "0";
        }
        if ((int)Convert.ToInt16(str.Trim()) > 0)
        {
            lblMsg.Text = string.Empty;
            ViewState["Admn"] = null;
            ViewState["classid"] = null;
            ViewState["SessYr"] = null;
            SaveRecord();
        }
        else
        {
            lblMsg.Text = "Fee Amount not defined for the selected Class!!Please define fee for admission!";
            lblMsg.ForeColor = Color.Red;
        }
    }

    private void SaveRecord()
    {
        if (Request.QueryString["sid"] != null)
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Control)btnSubmit, btnSubmit.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                string str = Request["Checkb"];
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str.Split(chArray))
                    InsertStudDetails(obj.ToString());
            }
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "alert('Updated Successfully.');window.location='ShowHostelStudents.aspx'", true);
        }
        else
        {
            try
            {
                if (Request["Checkb"] == null)
                {
                    ScriptManager.RegisterClientScriptBlock((Control)btnSubmit, btnSubmit.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
                }
                else
                {
                    string[] strArray = Request["Checkb"].Split(',');
                    for (int index = 0; index < strArray.Length; ++index)
                    {
                        obj = new clsDAL();
                        string str1 = obj.ExecuteScalarQry("select top(1) SessionYr from PS_FeeNormsNew order by SessionID desc");
                        string str2 = obj.ExecuteScalarQry("select AdmnSessYr from PS_StudMaster where AdmnNo=" + strArray[index].ToString());
                        InsertStudDetails(strArray[index].ToString());
                        if (str1 == str2.Trim())
                            InsertDetailsInLedger(strArray[index].ToString(), 'N');
                        else
                            InsertDetailsInLedger(strArray[index].ToString(), 'E');
                    }
                    Clear();
                    lblMsg.Text = "Data Saved Successfully !!";
                    lblMsg.ForeColor = Color.Green;
                    FillGrid("");
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message.ToString();
                lblMsg.ForeColor = Color.Red;
            }
        }
    }

    private void InsertDetailsInLedger(string AdmnNo, char StudType)
    {
        clsGenerateFee clsGenerateFee = new clsGenerateFee();
        string FeeOT = "Y";
        string FeeAnnual = "Y";
        string str1 = drpFeeStartFrom.SelectedValue.ToString();
        string str2 = drpSession.SelectedValue.ToString().Trim().Substring(0, 4);
        string str3 = drpSession.SelectedValue.ToString().Trim().Substring(0, 2) + drpSession.SelectedValue.ToString().Trim().Substring(5, 2);
        GenerateFeeOnHostelAdmn(Convert.ToInt32(str1) <= 3 ? Convert.ToDateTime(str1 + "/01/" + str3) : Convert.ToDateTime(str1 + "/01/" + str2), AdmnNo.ToString().Trim(), FeeOT, FeeAnnual, drpSession.SelectedValue.ToString().Trim(), Session["User_Id"].ToString(), Session["SchoolId"].ToString(), StudType);
        btnFeeRcv.Enabled = true;
        ViewState["classid"] = drpclass.SelectedValue;
        ViewState["SessYr"] = drpSession.SelectedValue.ToString();
    }

    private void InsertStudDetails(string AdmnNo)
    {
        obj = new clsDAL();
        DataTable dataTable1 = new DataTable();
        DataTable dataTable2 = obj.GetDataTable("Host_Insertaddmissioninfo", new Hashtable()
    {
      {
         "@SessionYr",
         drpSession.SelectedValue.Trim().ToString()
      },
      {
         "@AdmnNo",
         AdmnNo.Trim().ToString()
      },
      {
         "@AdmissionDt",
         dtpAdmnDt.GetDateValue().ToString("dd-MMM-yyyy")
      },
      {
         "@UserId",
        Session["User_Id"]
      }
    });
        if (dataTable2.Rows.Count > 0)
            ViewState["Admn"] = dataTable2.Rows[0]["AdmnNo"].ToString();
        else
            ViewState["Admn"] = Request.QueryString["sid"].ToString();
        if (Request.QueryString["sid"] == null || !(Request.QueryString["sid"].ToString() != ""))
            return;
        InsertConcession();
    }

    private void InsertConcession()
    {
        int num;
        try
        {
            num = Convert.ToInt32(txtFeeConcession.Text);
        }
        catch
        {
            num = 0;
        }
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("adminno", ViewState["Admn"].ToString());
        hashtable.Add("session", drpSession.SelectedValue.ToString());
        hashtable.Add("feeconssionper", num);
        hashtable.Add("fineconssionper", "0");
        hashtable.Add("authority", "Principal");
        if (num > 0)
            hashtable.Add("reason", txtConceReason.Text);
        else
            hashtable.Add("reason", string.Empty);
        hashtable.Add("UserID", Convert.ToInt32(Session["User_Id"]));
        hashtable.Add("schoolid", Session["SchoolId"].ToString());
        clsDal.ExcuteProcInsUpdt("Host_InsertFeeConcession", hashtable);
    }

    public void GenerateFeeOnHostelAdmn(DateTime FeeStartDate, string AdmnNo, string FeeOT, string FeeAnnual, string AdmnSession, string UserId, string SchoolId, char studType)
    {
        string str = new clsDAL().ExecuteScalarQry("select ClassId from dbo.PS_ClasswiseStudent where SessionYear='" + AdmnSession + "' and admnno =" + AdmnNo);
        clsDAL clsDal = new clsDAL();
        DateTime dateTime = Convert.ToDateTime(clsDal.GetDataTableQry("select * from PS_FeeNormsNew where SessionYr = '" + AdmnSession + "'").Rows[0][2].ToString());
        if (FeeOT == "Y")
        {
            DataTable dataTable = clsDal.GetDataTable("Host_GetHostFeeCompAmt", new Hashtable()
      {
        {
           "@SessYr",
           AdmnSession
        },
        {
           "@ClassId",
           str
        },
        {
           "@PeriodicityId",
           1
        },
        {
           "@StudType",
           studType
        }
      });
            GenOT_AnnualFeeOnHostAdmn(AdmnNo, dataTable, dateTime, AdmnSession, UserId, SchoolId);
        }
        if (FeeAnnual == "Y")
        {
            DataTable dataTable = clsDal.GetDataTable("Host_GetHostFeeCompAmt", new Hashtable()
      {
        {
           "@SessYr",
           AdmnSession
        },
        {
           "@ClassId",
           str
        },
        {
           "@PeriodicityId",
           2
        },
        {
           "@StudType",
           studType
        }
      });
            GenOT_AnnualFeeOnHostAdmn(AdmnNo, dataTable, dateTime, AdmnSession, UserId, SchoolId);
        }
        DataTable dataTable1 = clsDal.GetDataTable("Host_GetHostFeeCompAmt", new Hashtable()
    {
      {
         "@SessYr",
         AdmnSession
      },
      {
         "@ClassId",
         str
      },
      {
         "@PeriodicityId",
         5
      },
      {
         "@StudType",
         studType
      }
    });
        GenerateMlyFeeOnHostAdmn(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable1);
    }

    protected void GenOT_AnnualFeeOnHostAdmn(string AdmnNo, DataTable dtFeeAmt, DateTime FeeDate, string session, string uid, string schoolid)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("Host_GetConcessionPer", new Hashtable()
    {
      {
         "SessYr",
         session
      },
      {
         "AdmnNo",
         AdmnNo
      }
    }));
        foreach (DataRow row in (InternalDataCollectionBase)dtFeeAmt.Rows)
        {
            string str = row["ConcessionApplicable"].ToString().Trim();
            double num1 = Convert.ToDouble(row["amount"].ToString().Trim());
            double num2 = !(str == "Y") || num1 <= 0.0 ? num1 : num1 - num1 * (double)int32 / 100.0;
            Convert.ToInt32(row["FeeCompID"].ToString().Trim());
            new clsDAL().GetDataTable("Host_InsertFeeLedger", new Hashtable()
      {
        {
           "TransDate",
           FeeDate.ToString("MM/dd/yyyy")
        },
        {
           "AdmnNo",
           AdmnNo
        },
        {
           "TransDesc",
           row["FeeName"].ToString().Trim()
        },
        {
           "debit",
           num2
        },
        {
           "credit",
           0
        },
        {
           "balance",
           num2
        },
        {
           "Receipt_VrNo",
           ""
        },
        {
           "userid",
           uid
        },
        {
           "UserDate",
           DateTime.Now.ToString("MM/dd/yyyy")
        },
        {
           "schoolid",
           schoolid
        },
        {
           "FeeId",
           Convert.ToInt32(row["FeeCompID"].ToString().Trim())
        },
        {
           "PayMode",
           ""
        },
        {
           "Session",
           session
        }
      });
        }
    }

    protected void GenerateMlyFeeOnHostAdmn(string AdmnNo, DateTime FeeStDate, string Sess, string uid, string SchoolId, DataTable dtFeeAmt)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("Host_GetConcessionPer", new Hashtable()
    {
      {
         "SessYr",
         Sess
      },
      {
         "AdmnNo",
         AdmnNo
      }
    }));
        foreach (DataRow row in (InternalDataCollectionBase)dtFeeAmt.Rows)
        {
            string str = row["ConcessionApplicable"].ToString().Trim();
            double num1 = Convert.ToDouble(row["amount"].ToString().Trim());
            double num2 = !(str == "Y") || num1 <= 0.0 ? num1 : num1 - num1 * (double)int32 / 100.0;
            Convert.ToInt32(row["FeeCompID"].ToString());
            new clsDAL().ExcuteProcInsUpdt("Host_InsMlyFeeAll", new Hashtable()
      {
        {
           "AdmnNo",
           AdmnNo
        },
        {
           "TransDesc",
           row["FeeName"].ToString().Trim()
        },
        {
           "Amt",
           num2
        },
        {
           "userid",
           uid
        },
        {
           "schoolid",
           SchoolId
        },
        {
           "FeeId",
           Convert.ToInt32(row["FeeCompID"].ToString().Trim())
        },
        {
           "Session",
           Sess
        },
        {
           "FeeStartDt",
           FeeStDate
        }
      });
        }
    }

    private void Clear()
    {
        txtadmnno.Text = string.Empty;
        drpclass.SelectedIndex = -1;
        drpstudent.SelectedIndex = -1;
        drpSection.SelectedIndex = -1;
        txtAdmnDt.Text = string.Empty;
        txtFeeConcession.Text = "0";
        txtConceReason.Text = string.Empty;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        Clear();
    }

    protected void btnFeeRcv_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Hostel/FeeReceiptHostel.aspx?admnno=" + ViewState["Admn"].ToString() + "&cid=" + ViewState["classid"].ToString() + "&sess=" + ViewState["SessYr"].ToString());
    }

    protected void btnList_Click(object sender, EventArgs e)
    {
        Response.Redirect("ShowHostelStudents.aspx");
    }

    protected override void InitializeCulture()
    {
        CultureInfo cultureInfo = new CultureInfo("en-US");
        cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
        Thread.CurrentThread.CurrentCulture = cultureInfo;
        Thread.CurrentThread.CurrentUICulture = cultureInfo;
        base.InitializeCulture();
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillGrid("");
    }

    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        ClearGrid();
    }
}