using ASP;
using Classes.DA;
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

public partial class FeeManagement_GenerateFeeOnFeeMod : System.Web.UI.Page
{
    private string curSess = "";
    private clsDAL obj = new clsDAL();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["SchoolId"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        fillsession();
        FillClassDropDown();
    }

    protected void fillsession()
    {
        obj = new clsDAL();
        DataTable dataTable = new DataTable();
        drpsession.DataSource = obj.GetDataTableQry("SELECT SessionID,SessionYr FROM dbo.PS_FeeNormsNew ORDER BY SessionID DESC");
        drpsession.DataTextField = "SessionYr";
        drpsession.DataValueField = "SessionYr";
        drpsession.DataBind();
    }

    private void FillClassDropDown()
    {
        obj = new clsDAL();
        drpClass.DataSource = obj.GetDataTable("ps_sp_get_ClassesForDDL", new Hashtable());
        drpClass.DataTextField = "ClassName";
        drpClass.DataValueField = "ClassID";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("--Select--", "0"));
    }

    protected void btnGenFee_Click(object sender, EventArgs e)
    {
        lblerr.Text = "";
        btnGenFee.Enabled = false;
        if (new clsDAL().ExecuteScalarQry("select finalised from dbo.PS_FeeNormsNew where SessionYr='" + drpsession.SelectedValue.ToString().Trim() + "'").ToLower() == "yes")
        {
            lblerr.Text = "The selected session  year is finalized. Fee cann't be regenerated now.";
            lblerr.ForeColor = Color.Red;
        }
        else if (drpstudent.SelectedValue.ToString().Trim() == "0")
        {
            clsDAL clsDal = new clsDAL();
            DataTable dataTable = new DataTable();
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select distinct c.admnno from dbo.PS_ClasswiseStudent c ");
            stringBuilder.Append(" inner join dbo.PS_StudMaster s on s.AdmnNo=c.AdmnNo");
            stringBuilder.Append(" where c.SessionYear='" + drpsession.SelectedValue.ToString().Trim() + "' and c.ClassID=" + drpClass.SelectedValue.ToString());
            stringBuilder.Append(" order by c.admnno");
            foreach (DataRow row in (InternalDataCollectionBase)clsDal.GetDataTableQry(stringBuilder.ToString()).Rows)
            {
                clsDal.GetDataTableQry("select * from dbo.ps_studmaster where Admnno=" + drpstudent.SelectedValue.ToString().Trim() + " and AdmnSessYr='" + drpsession.SelectedValue.Trim() + "'");
                if (dataTable.Rows.Count > 0)
                    InsertDetailsInLedger(row["admnno"].ToString().Trim(), dataTable.Rows[0]["StudType"].ToString().ToUpper());
                else
                    InsertDetailsInLedger(row["admnno"].ToString().Trim(), "E");
            }
        }
        else
            InsertDetailsInLedger(drpstudent.SelectedValue.ToString().Trim(), txtStudType.Text.Trim());
    }

    private void InsertDetailsInLedger(string Admn, string stype)
    {
        clsDAL clsDal = new clsDAL();
        Hashtable hashtable = new Hashtable();
        hashtable.Add("@SessYr", drpsession.SelectedValue.ToString().Trim());
        hashtable.Add("@AdmnNo", Admn);
        string str1 = "04/01/" + drpsession.SelectedValue.ToString().Trim().Substring(0, 4);
        hashtable.Add("@TransDt", str1);
        if (clsDal.ExecuteScalar("Del_AllRefOnReGenFeeIndividual", hashtable).Trim() == "")
        {
            string str2 = "";
            try
            {
                string FeeOT = "Y";
                string FeeAnnual = "Y";
                clsGenerateFee clsGenerateFee = new clsGenerateFee();
                string str3 = drpsession.SelectedValue.ToString().Trim().Substring(0, 4);
                string str4 = drpsession.SelectedValue.ToString().Trim().Substring(0, 2) + drpsession.SelectedValue.ToString().Trim().Substring(5, 2);
                DateTime dateTime = Convert.ToDateTime("04/01/" + str3);
                GenerateFeeOnFeeMod(dateTime, dateTime, Admn, FeeOT, FeeAnnual, drpsession.SelectedValue.ToString().Trim(), Session["User_Id"].ToString(), Session["SchoolId"].ToString(), char.Parse(stype));
                lblerr.Text = "Generated successfully";
                lblerr.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                str2 = ex.ToString();
            }
            if (str2.Trim() != "")
            {
                lblerr.Text = "Error While Generating School Fee! Please Try Again!!";
                lblerr.ForeColor = Color.Red;
            }
            btnGenFee.Enabled = true;
        }
        else
        {
            lblerr.Text = "Unable to ReGenerate fee!!Please Try Again later!";
            lblerr.ForeColor = Color.Red;
            btnGenFee.Enabled = true;
        }
    }

    public void GenerateFeeOnFeeMod(DateTime AdmnDate, DateTime FeeStartDate, string AdmnNo, string FeeOT, string FeeAnnual, string AdmnSession, string UserId, string SchoolId, char studType)
    {
        string str = new clsDAL().ExecuteScalarQry("select Stream from dbo.PS_ClasswiseStudent where SessionYear='" + AdmnSession + "' and admnno =" + AdmnNo);
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
           "@StudType",
           studType
        }
      });
            GenOT_AnnualFeeOnFeeMod(AdmnNo, dataTable, dateTime, AdmnSession, UserId, SchoolId);
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
           "@StudType",
           studType
        }
      });
            GenOT_AnnualFeeOnFeeMod(AdmnNo, dataTable, dateTime, AdmnSession, UserId, SchoolId);
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
         "@PeriodicityId",
         5
      },
      {
         "@StudType",
         studType
      }
    });
        GenerateMlyFeeOnFeeMod(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable1);
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
         "@PeriodicityId",
         3
      },
      {
         "@StudType",
         studType
      }
    });
        if (dataTable2.Rows.Count > 0)
            GenerateHlyFeeOnFeeMod(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable2);
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
        GenerateQtlyFeeOnFeeMod(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable3);
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
            new Common().GetDataTable("ps_sp_insert_FeeLedgerOnFeeMod", new Hashtable()
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

    private void InsertDetailsForBusFee(string StudAdmnNo)
    {
        new clsDAL().ExcuteProcInsUpdt("ps_sp_insert_AdFeeLedgerBus", new Hashtable()
    {
      {
         "Admnno",
         StudAdmnNo
      },
      {
         "uid",
         Convert.ToInt32(Session["User_Id"])
      },
      {
         "Session",
         drpsession.SelectedValue.ToString().Trim()
      },
      {
         "SchoolId",
         Session["SchoolId"].ToString().Trim()
      }
    });
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        fillstudent();
    }

    private void fillstudent()
    {
        clsDAL clsDal = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select fullname,s.admnno from PS_ClasswiseStudent cs join ps_studmaster s on ");
        stringBuilder.Append(" s.admnno=cs.admnno where cs.classid=" + drpClass.SelectedValue);
        stringBuilder.Append(" and cs.SessionYear='" + drpsession.SelectedValue.ToString().Trim() + "' order by fullname ");
        DataTable dataTableQry = clsDal.GetDataTableQry(stringBuilder.ToString().Trim());
        lblerr.Text = "Total Students: " + dataTableQry.Rows.Count.ToString();
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
            drpstudent.Items.Insert(0, new ListItem("-Select-", "0"));
        else
            drpstudent.Items.Insert(0, new ListItem("No Record", "0"));
    }

    protected void drpstudent_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerr.Text = "Admission No: " + drpstudent.SelectedValue.ToString();
        obj = new clsDAL();
        DataTable dataTableQry1 = obj.GetDataTableQry("select * from dbo.ps_studmaster where Admnno=" + drpstudent.SelectedValue.ToString().Trim() + " and AdmnSessYr='" + drpsession.SelectedValue.Trim() + "'");
        DataTable dataTableQry2 = obj.GetDataTableQry("select * from dbo.ps_studmaster where Admnno=" + drpstudent.SelectedValue.ToString().Trim());
        if (dataTableQry1.Rows.Count > 0)
            txtStudType.Text = dataTableQry1.Rows[0]["StudType"].ToString().ToUpper();
        else if (dataTableQry2.Rows[0]["StudType"].ToString().ToUpper() == "C")
            txtStudType.Text = "C";
        else
            txtStudType.Text = "E";
    }
}