using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admissions_ChangeStudClass : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (this.Page.IsPostBack)
            return;
        if (this.Session["User"] != null)
        {
            this.fillsession();
            this.FillClassDropDown();
        }
        else
            this.Response.Redirect("../Login.aspx");
    }

    protected void fillsession()
    {
        this.obj = new clsDAL();
        DataTable dataTable = new DataTable();
        this.drpsession.DataSource = this.obj.GetDataTableQry("SELECT SessionID,SessionYr FROM dbo.PS_FeeNormsNew ORDER BY SessionID DESC");
        this.drpsession.DataTextField = "SessionYr";
        this.drpsession.DataValueField = "SessionYr";
        this.drpsession.DataBind();
    }

    private void FillClassDropDown()
    {
        this.obj = new clsDAL();
        DataTable dataTable = this.obj.GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
        this.drpClass.DataSource = dataTable;
        this.drpClass.DataTextField = "ClassName";
        this.drpClass.DataValueField = "ClassID";
        this.drpClass.DataBind();
        this.drpClass.Items.Insert(0, new ListItem("--Select--", "0"));
        this.drpNewClass.DataSource = dataTable;
        this.drpNewClass.DataTextField = "ClassName";
        this.drpNewClass.DataValueField = "ClassID";
        this.drpNewClass.DataBind();
        this.drpNewClass.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        changeclass();
        clsDAL clsDal = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select fullname,s.admnno,s.OldAdmnNo from PS_ClasswiseStudent cs join ps_studmaster s on ");
        stringBuilder.Append(" s.admnno=cs.admnno where cs.classid=" + this.drpClass.SelectedValue);
        stringBuilder.Append(" and cs.SessionYear='" + this.drpsession.SelectedValue.ToString().Trim() + "' order by fullname ");
        DataTable dataTableQry = clsDal.GetDataTableQry(stringBuilder.ToString().Trim());
        this.lblerr.Text = "Total Students: " + dataTableQry.Rows.Count.ToString();
        try
        {
            this.drpstudent.DataSource = dataTableQry;
            this.drpstudent.DataTextField = "fullname";
            this.drpstudent.DataValueField = "admnno";
            this.drpstudent.DataBind();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        if (dataTableQry.Rows.Count > 0)
            this.drpstudent.Items.Insert(0, new ListItem("-Select-", "0"));
        else
            this.drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
    }

    private void changeclass()
    {
        try
        {
            drpNewClass.SelectedIndex = drpClass.SelectedIndex + 1;
            if (Convert.ToInt32(drpNewClass.SelectedValue) > 12)
            {
                drpStream.Enabled = true;
            }
            else
                drpStream.Enabled = false;
        }
        catch
        {
        }
    }
    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        //this.lblerr.Text = "Admission No: " + this.drpstudent.SelectedValue.ToString();
        this.obj = new clsDAL();
        DataTable dataTableQry = this.obj.GetDataTableQry("select * from dbo.ps_studmaster where AdmnNo=" + this.drpstudent.SelectedValue.ToString().Trim() + " and AdmnSessYr='" + this.drpsession.SelectedValue.Trim() + "'");
        if (dataTableQry.Rows.Count > 0)
        {
            this.txtStudType.Text = dataTableQry.Rows[0]["StudType"].ToString().ToUpper();
            this.lblerr.Text = "Admission No: " + dataTableQry.Rows[0]["AdmnNo"].ToString().ToUpper();
        }
        else
        {
            this.txtStudType.Text = "E";
            this.lblerr.Text = "";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SessionYr", this.drpsession.SelectedValue.Trim());
        hashtable.Add("@AdmnNo", this.drpstudent.SelectedValue.Trim());
        hashtable.Add("@CurrClass", this.drpClass.SelectedValue.Trim());
        hashtable.Add("@NewClass", this.drpNewClass.SelectedValue.Trim());
        if(drpStream.SelectedIndex >0)
            hashtable.Add("@StreamId", this.drpStream.SelectedValue.Trim());
        else
            hashtable.Add("@StreamId", 1);
        
        hashtable.Add("@UserID", Convert.ToInt32(this.Session["User_Id"]));
        hashtable.Add("@SchoolID", this.Session["SchoolId"].ToString());
        this.obj = new clsDAL();
        if (this.obj.ExecuteScalar("ps_ChangeStudClass", hashtable).Trim() == "")
        {
            try
            {
                this.InsertDetailsInLedger(this.drpstudent.SelectedValue.ToString().Trim(), this.txtStudType.Text.Trim());
                if (this.obj.ExecuteScalar("ps_DelFeeCompOnClassMod", new Hashtable()
        {
          {
             "@StudType",
             this.txtStudType.Text.Trim()
          },
          {
             "@NewClassId",
             this.drpNewClass.SelectedValue
          },
          {
             "@SessionYr",
             this.drpsession.SelectedValue.Trim()
          },
          {
             "@Admnno",
             this.drpstudent.SelectedValue.Trim()
          },
           {
             "@NewStreamId",
             this.drpStream.SelectedValue.Trim()
          }
        }).Trim() == "")
                {
                    this.lblerr.Text = "Class Changed Successfully";
                    this.lblerr.ForeColor = Color.Green;
                    this.ClearAll();
                }
                else
                {
                    this.lblerr.Text = "Unable to Modify Class Please try again!";
                    this.lblerr.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                this.lblerr.Text = "Unable to Generate Fee For New Class Please try again!";
                this.lblerr.ForeColor = Color.Red;
            }
        }
        else
        {
            this.lblerr.Text = "Unable to Change Student Class Please try again!";
            this.lblerr.ForeColor = Color.Red;
        }
    }

    private void InsertDetailsInLedger(string Admn, string stype)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SessYr", this.drpsession.SelectedValue.ToString().Trim());
        hashtable.Add("@AdmnNo", Admn);
        string str1 = "04/01/" + this.drpsession.SelectedValue.ToString().Trim().Substring(0, 4);
        hashtable.Add("@TransDt", str1);
        if (clsDal.ExecuteScalar("Del_AllRefOnReGenFeeIndividual", hashtable).Trim() == "")
        {
            string str2 = "";
            try
            {
                string FeeOT = "Y";
                string FeeAnnual = "Y";
                clsGenerateFee clsGenerateFee = new clsGenerateFee();
                string str3 = this.drpsession.SelectedValue.ToString().Trim().Substring(0, 4);
                string str4 = this.drpsession.SelectedValue.ToString().Trim().Substring(0, 2) + this.drpsession.SelectedValue.ToString().Trim().Substring(5, 2);
               


                DateTime dateTime = Convert.ToDateTime("04/01/" + str3);
                this.GenerateFeeOnFeeMod(dateTime, dateTime, Admn, FeeOT, FeeAnnual, this.drpsession.SelectedValue.ToString().Trim(), this.Session["User_Id"].ToString(), this.Session["SchoolId"].ToString(), char.Parse(stype));
            }
            catch (Exception ex)
            {
                str2 = ex.ToString();
            }
            if (!(str2.Trim() != ""))
                return;
            this.lblerr.Text = "Error While modifying class! Please Try Again!!";
            this.lblerr.ForeColor = Color.Red;
        }
        else
        {
            this.lblerr.Text = "Unable to ReGenerate fee!!Please Try Again later!";
            this.lblerr.ForeColor = Color.Red;
        }
    }

    public void GenerateFeeOnFeeMod(DateTime AdmnDate, DateTime FeeStartDate, string AdmnNo, string FeeOT, string FeeAnnual, string AdmnSession, string UserId, string SchoolId, char studType)
    {
        //string stream = "";
        //if (drpStream.SelectedIndex > 0)
        //    stream = this.drpStream.SelectedValue.Trim();
        //else
        //    stream = "1";
        string str = new clsDAL().ExecuteScalarQry("select Stream from dbo.PS_ClasswiseStudent where SessionYear='" + AdmnSession + "' and admnno =" + AdmnNo);
        string strnew = new clsDAL().ExecuteScalarQry("select Classid from dbo.PS_ClasswiseStudent where SessionYear='" + AdmnSession + "' and admnno =" + AdmnNo);
        clsDAL clsDal = new clsDAL();
        DateTime dateTime = Convert.ToDateTime(clsDal.GetDataTableQry("select * from PS_FeeNormsNew where SessionYr = '" + AdmnSession + "'").Rows[0][2].ToString());
        if (FeeOT == "Y")
        {
            DataTable dataTable = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
      {
        {
           "@SessYr",
           AdmnSession
        },
        {
           "@StreamId",
           str
        },
        {
           "@PeriodicityId",
           1
        },
         {
          (object) "@ClassId",
          (object) strnew
        },
        {
           "@StudType",
           studType
        }
      });
            this.GenOT_AnnualFeeOnFeeMod(AdmnNo, dataTable, dateTime, AdmnSession, UserId, SchoolId);
        }
        if (FeeAnnual == "Y")
        {
            DataTable dataTable = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
      {
        {
           "@SessYr",
           AdmnSession
        },
        {
           "@StreamId",
           str
        },
        {
           "@PeriodicityId",
           2
        },
         {
          (object) "@ClassId",
          (object) strnew
        },
        {
           "@StudType",
           studType
        }
      });
            this.GenOT_AnnualFeeOnFeeMod(AdmnNo, dataTable, dateTime, AdmnSession, UserId, SchoolId);
        }
        DataTable dataTable1 = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
    {
      {
         "@SessYr",
         AdmnSession
      },
      {
         "@StreamId",
         str
      },
       {
          (object) "@ClassId",
          (object) strnew
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
        this.GenerateMlyFeeOnFeeMod(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable1);
        DataTable dataTable2 = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
    {
      {
         "@SessYr",
         AdmnSession
      },
      {
         "@StreamId",
         str
      },
       {
          (object) "@ClassId",
          (object) strnew
        },
      {
         "@PeriodicityId",
         3
      },
      {
         "@StudType",
         studType
      }
    });
        if (dataTable2.Rows.Count > 0)
            this.GenerateHlyFeeOnFeeMod(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable2);
        DataTable dataTable3 = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
    {
      {
         "@SessYr",
         AdmnSession
      },
      {
         "@StreamId",
         str
      },
       {
          (object) "@ClassId",
          (object) strnew
        },
      {
         "@PeriodicityId",
         4
      },
      {
         "@StudType",
         studType
      }
    });
        if (dataTable3.Rows.Count <= 0)
            return;
        this.GenerateQtlyFeeOnFeeMod(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable3);
    }

    protected void GenOT_AnnualFeeOnFeeMod(string AdmnNo, DataTable dtFeeAmt, DateTime FeeDate, string session, string uid, string schoolid)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("ps_sp_GetConcessionPer", new Hashtable()
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
            new clsDAL().GetDataTable("ps_sp_insert_FeeLedgerOnFeeMod", new Hashtable()
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

    protected void GenerateMlyFeeOnFeeMod(string AdmnNo, DateTime FeeStDate, string Sess, string uid, string SchoolId, DataTable dtFeeAmt)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("ps_sp_GetConcessionPer", new Hashtable()
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
            new clsDAL().ExcuteProcInsUpdt("ps_sp_insert_FeeLedgerOnFeeModMly", new Hashtable()
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

    protected void GenerateHlyFeeOnFeeMod(string AdmnNo, DateTime FeeStDate, string Sess, string uid, string SchoolId, DataTable dtFeeAmt)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("ps_sp_GetConcessionPer", new Hashtable()
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
            new clsDAL().ExcuteProcInsUpdt("ps_sp_InsHlyFeeOnFeeMod", new Hashtable()
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

    protected void GenerateQtlyFeeOnFeeMod(string AdmnNo, DateTime FeeStDate, string Sess, string uid, string SchoolId, DataTable dtFeeAmt)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("ps_sp_GetConcessionPer", new Hashtable()
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
            new clsDAL().ExcuteProcInsUpdt("ps_sp_InsQtlyFeeOnFeeMod", new Hashtable()
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

    private void ClearAll()
    {
        this.drpClass.SelectedIndex = 0;
        this.drpNewClass.SelectedIndex = 0;
        this.txtStudType.Text = "";
        this.drpstudent.Items.Clear();
        this.drpstudent.DataSource = null;
        this.drpstudent.DataBind();
        drpStream.SelectedIndex = 0;
    }
}