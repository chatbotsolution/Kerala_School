
using Classes.DA;
using SanLib;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

public class clsGenerateFee
{
    private string curSess = "";
    private SqlConnection con;
    private bool feeLedgerGenerated;

    public clsGenerateFee()
    {
        this.con = new SqlConnection();
        this.con.ConnectionString = ConfigurationManager.ConnectionStrings["conString"].ConnectionString;
    }

    public void GenerateFine(string uid, string schoolid)
    {
      //  this.curSess = this.CreateCurrSession();
        DataTable dataTable1 = new Common().ExecuteSql("select  cs.admnno,cs.Classid,cs.section from dbo.PS_ClasswiseStudent cs inner join dbo.PS_StudMaster s on s.admnno=cs.admnno and Status = 1 where SessionYear='" + this.curSess + "' order by Classid,section");
        if (dataTable1.Rows.Count <= 0)
            return;
        foreach (DataRow row1 in (InternalDataCollectionBase)dataTable1.Rows)
        {
            int int32 = Convert.ToInt32(new Common().ExecuteSql("select FineConcessionPer from dbo.PS_FeeConcession where Session='" + this.curSess + "' and admnno =" + (object)Convert.ToInt32(row1["admnno"].ToString().Trim())).Rows[0][0]);
            foreach (DataRow row2 in (InternalDataCollectionBase)new Common().ExecuteSql("select distinct TransDate from dbo.PS_FeeLedger where Balance > 0 and admnno=" + (object)Convert.ToInt32(row1["admnno"].ToString().Trim())).Rows)
            {
                for (int index1 = 0; index1 < 11; ++index1)
                {
                    Common common = new Common();
                    DateTime dateTime1 = Convert.ToDateTime(row2["TransDate"].ToString());
                    DataTable dataTable2 = common.ExecuteSql("select * from dbo.PS_FeeNormsNew where SessionYr='" + this.curSess + "' and DueDate" + (object)(index1 + 1) + "='" + dateTime1.ToString("MM/dd/yyyy") + "'");
                    if (dataTable2.Rows.Count > 0)
                    {
                        string index2 = "FineDate" + (object)(index1 + 1);
                        DateTime dateTime2 = Convert.ToDateTime(dataTable2.Rows[0][index2].ToString());
                        dataTable2.Rows[0]["FineAmount"].ToString();
                        double num1 = Convert.ToDouble(dataTable2.Rows[0]["FineAmount"].ToString());
                        double num2 = num1 - num1 * (double)int32 / 100.0;
                        if (dateTime2 < DateTime.Today)
                            new Common().GetDataTable("ps_sp_insert_FineLedger", new Hashtable()
              {
                {
                  (object) "TransDate",
                  (object) dateTime1.ToString("MM/dd/yyyy")
                },
                {
                  (object) "AdmnNo",
                  (object) Convert.ToInt32(row1["admnno"].ToString().Trim())
                },
                {
                  (object) "TransDesc",
                  (object) "Fine- Due not paid"
                },
                {
                  (object) "debit",
                  (object) num2
                },
                {
                  (object) "credit",
                  (object) 0
                },
                {
                  (object) "balance",
                  (object) num2
                },
                {
                  (object) "Receipt_VrNo",
                  (object) ""
                },
                {
                  (object) "userid",
                  (object) uid
                },
                {
                  (object) "schoolid",
                  (object) schoolid
                }
              });
                    }
                }
            }
        }
    }

    public void GenFeeOnAdmnAddonFacility(DateTime FeeStartDate, string AdmnNo, string AdmnSession, string UserId, string SchoolId, char studType)
    {
        string str = new clsDAL().ExecuteScalarQry("select Stream from dbo.PS_ClasswiseStudent where SessionYear='" + AdmnSession + "' and admnno =" + AdmnNo);
        clsDAL clsDal = new clsDAL();
        clsDal.GetDataTableQry("select * from PS_FeeNormsNew where SessionYr = '" + AdmnSession + "'");
        DataTable dataTable1 = clsDal.GetDataTable("ps_sp_get_FeeCompAmtAddonFacility", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
        (object) "@PeriodicityId",
        (object) 1
      },
      {
        (object) "@StudType",
        (object) studType
      },
      {
        (object) "@AdmnNo",
        (object) AdmnNo
      }
    });
        this.GenOT_AnnualFeeOnAdmn(AdmnNo, dataTable1, FeeStartDate, AdmnSession, UserId, SchoolId);
        DataTable dataTable2 = clsDal.GetDataTable("ps_sp_get_FeeCompAmtAddonFacility", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
        (object) "@PeriodicityId",
        (object) 2
      },
      {
        (object) "@StudType",
        (object) studType
      },
      {
        (object) "@AdmnNo",
        (object) AdmnNo
      }
    });
        this.GenOT_AnnualFeeOnAdmn(AdmnNo, dataTable2, FeeStartDate, AdmnSession, UserId, SchoolId);
        DataTable dataTable3 = clsDal.GetDataTable("ps_sp_get_FeeCompAmtAddonFacility", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
        (object) "@PeriodicityId",
        (object) 3
      },
      {
        (object) "@StudType",
        (object) studType
      },
      {
        (object) "@AdmnNo",
        (object) AdmnNo
      }
    });
        if (dataTable3.Rows.Count > 0)
            this.GenerateHlyFeeOnAdmn(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable3);
        DataTable dataTable4 = clsDal.GetDataTable("ps_sp_get_FeeCompAmtAddonFacility", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
        (object) "@PeriodicityId",
        (object) 4
      },
      {
        (object) "@StudType",
        (object) studType
      },
      {
        (object) "@AdmnNo",
        (object) AdmnNo
      }
    });
        if (dataTable4.Rows.Count > 0)
            this.GenerateQtlyFeeOnAdmn(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable4);
        DataTable dataTable5 = clsDal.GetDataTable("ps_sp_get_FeeCompAmtAddonFacility", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
        (object) "@PeriodicityId",
        (object) 5
      },
      {
        (object) "@StudType",
        (object) studType
      },
      {
        (object) "@AdmnNo",
        (object) AdmnNo
      }
    });
        if (dataTable5.Rows.Count <= 0)
            return;
        this.GenerateMlyFeeOnAdmn(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable5);
    }

    public void GenerateExtraFee(DateTime startDate, string AdmnNo, string AdmnSession, string UserId, string SchoolId, char studType, int FeeCompId)
    {
        string str = new clsDAL().ExecuteScalarQry("select Stream from dbo.PS_ClasswiseStudent where SessionYear='" + AdmnSession + "' and admnno =" + AdmnNo);
        DataTable dataTable1 = new clsDAL().GetDataTable("ps_sp_get_FeeCompAmtSingleFee", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
        (object) "@PeriodicityId",
        (object) 1
      },
      {
        (object) "@StudType",
        (object) studType
      },
      {
        (object) "@FeeCompID",
        (object) FeeCompId
      }
    });
        if (dataTable1.Rows.Count > 0)
            this.GenOT_AnnualFeeOnAdmn(AdmnNo, dataTable1, startDate, AdmnSession, UserId, SchoolId);
        DataTable dataTable2 = new clsDAL().GetDataTable("ps_sp_get_FeeCompAmtSingleFee", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
        (object) "@PeriodicityId",
        (object) 2
      },
      {
        (object) "@StudType",
        (object) studType
      },
      {
        (object) "@FeeCompID",
        (object) FeeCompId
      }
    });
        if (dataTable2.Rows.Count > 0)
            this.GenOT_AnnualFeeOnAdmn(AdmnNo, dataTable2, startDate, AdmnSession, UserId, SchoolId);
        DataTable dataTable3 = new clsDAL().GetDataTable("ps_sp_get_FeeCompAmtSingleFee", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
        (object) "@PeriodicityId",
        (object) 3
      },
      {
        (object) "@StudType",
        (object) studType
      },
      {
        (object) "@FeeCompID",
        (object) FeeCompId
      }
    });
        if (dataTable3.Rows.Count > 0)
            this.GenerateHlyFeeOnAdmn(AdmnNo, startDate, AdmnSession, UserId, SchoolId, dataTable3);
        DataTable dataTable4 = new clsDAL().GetDataTable("ps_sp_get_FeeCompAmtSingleFee", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
        (object) "@PeriodicityId",
        (object) 4
      },
      {
        (object) "@StudType",
        (object) studType
      },
      {
        (object) "@FeeCompID",
        (object) FeeCompId
      }
    });
        if (dataTable4.Rows.Count > 0)
            this.GenerateQtlyFeeOnAdmn(AdmnNo, startDate, AdmnSession, UserId, SchoolId, dataTable4);
        DataTable dataTable5 = new clsDAL().GetDataTable("ps_sp_get_FeeCompAmtSingleFee", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
        (object) "@PeriodicityId",
        (object) 5
      },
      {
        (object) "@StudType",
        (object) studType
      },
      {
        (object) "@FeeCompID",
        (object) FeeCompId
      }
    });
        if (dataTable5.Rows.Count <= 0)
            return;
        this.GenerateMlyFeeOnAdmn(AdmnNo, startDate, AdmnSession, UserId, SchoolId, dataTable5);
    }

    public void GenerateFeeOnAdmission(DateTime AdmnDate, DateTime FeeStartDate, string AdmnNo, string FeeOT, string FeeAnnual, string AdmnSession, string UserId, string SchoolId, char studType)
    {
        string str = new clsDAL().ExecuteScalarQry("select Stream from dbo.PS_ClasswiseStudent where SessionYear='" + AdmnSession + "' and admnno =" + AdmnNo);
        string strnew = new clsDAL().ExecuteScalarQry("select Classid from dbo.PS_ClasswiseStudent where SessionYear='" + AdmnSession + "' and admnno =" + AdmnNo);
        clsDAL clsDal = new clsDAL();
        DateTime dateTime = Convert.ToDateTime(clsDal.GetDataTableQry("select * from PS_FeeNormsNew where SessionYr = '" + AdmnSession + "'").Rows[0][2].ToString());
        if (FeeOT == "Y")
        {
            DataTable dataTable = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
      {
        {
          (object) "@SessYr",
          (object) AdmnSession
        },
        {
          (object) "@StreamId",
          (object) str
        },
         {
          (object) "@ClassId",
          (object) strnew
        },
        {
          (object) "@PeriodicityId",
          (object) 1
        },
        {
          (object) "@StudType",
          (object) studType
        }
      });
            this.GenOT_AnnualFeeOnAdmn(AdmnNo, dataTable, dateTime, AdmnSession, UserId, SchoolId);
        }
        if (FeeAnnual == "Y")
        {
            DataTable dataTable = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
      {
        {
          (object) "@SessYr",
          (object) AdmnSession
        },
        {
          (object) "@StreamId",
          (object) str
        },
        {
          (object) "@ClassId",
          (object) strnew
        },
        {
          (object) "@PeriodicityId",
          (object) 2
        },
        {
          (object) "@StudType",
          (object) studType
        }
      });
            this.GenOT_AnnualFeeOnAdmn(AdmnNo, dataTable, dateTime, AdmnSession, UserId, SchoolId);
        }
        DataTable dataTable1 = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
          (object) "@ClassId",
          (object) strnew
        },
      {
        (object) "@PeriodicityId",
        (object) 3
      },
      {
        (object) "@StudType",
        (object) studType
      }
    });
        if (dataTable1.Rows.Count > 0)
            this.GenerateHlyFeeOnAdmn(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable1);
        DataTable dataTable2 = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
          (object) "@ClassId",
          (object) strnew
        },
      {
        (object) "@PeriodicityId",
        (object) 4
      },
      {
        (object) "@StudType",
        (object) studType
      }
    });
        if (dataTable2.Rows.Count > 0)
            this.GenerateQtlyFeeOnAdmn(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable2);
        DataTable dataTable3 = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
          (object) "@ClassId",
          (object) strnew
        },
      {
        (object) "@PeriodicityId",
        (object) 5
      },
      {
        (object) "@StudType",
        (object) studType
      }
    });
        if (dataTable3.Rows.Count <= 0)
            return;
        this.GenerateMlyFeeOnAdmn(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable3);
    }

    protected void GenOT_AnnualFeeOnAdmn(string AdmnNo, DataTable dtFeeAmt, DateTime FeeDate, string session, string uid, string schoolid)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("ps_sp_GetConcessionPer", new Hashtable()
    {
      {
        (object) "SessYr",
        (object) session
      },
      {
        (object) "AdmnNo",
        (object) AdmnNo
      }
    }));
        foreach (DataRow row in (InternalDataCollectionBase)dtFeeAmt.Rows)
        {
            string str = row["ConcessionApplicable"].ToString().Trim();
            double num1 = Convert.ToDouble(row["amount"].ToString().Trim());
            double num2 = !(str == "Y") || num1 <= 0.0 ? num1 : num1 - num1 * (double)int32 / 100.0;
            Convert.ToInt32(row["FeeCompID"].ToString().Trim());
            new Common().GetDataTable("ps_sp_insert_FeeLedger", new Hashtable()
      {
        {
          (object) "TransDate",
          (object) FeeDate.ToString("MM/dd/yyyy")
        },
        {
          (object) "AdmnNo",
          (object) AdmnNo
        },
        {
          (object) "TransDesc",
          (object) row["FeeName"].ToString().Trim()
        },
        {
          (object) "debit",
          (object) num2
        },
        {
          (object) "credit",
          (object) 0
        },
        {
          (object) "balance",
          (object) num2
        },
        {
          (object) "Receipt_VrNo",
          (object) ""
        },
        {
          (object) "userid",
          (object) uid
        },
        {
          (object) "UserDate",
          (object) DateTime.Now.ToString("MM/dd/yyyy")
        },
        {
          (object) "schoolid",
          (object) schoolid
        },
        {
          (object) "FeeId",
          (object) Convert.ToInt32(row["FeeCompID"].ToString().Trim())
        },
        {
          (object) "PayMode",
          (object) ""
        },
        {
          (object) "Session",
          (object) session
        },
        
      });
        }
    }

    protected void GenerateMlyFeeOnAdmn(string AdmnNo, DateTime FeeStDate, string Sess, string uid, string SchoolId, DataTable dtFeeAmt)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("ps_sp_GetConcessionPer", new Hashtable()
    {
      {
        (object) "SessYr",
        (object) Sess
      },
      {
        (object) "AdmnNo",
        (object) AdmnNo
      }
    }));
        foreach (DataRow row in (InternalDataCollectionBase)dtFeeAmt.Rows)
        {
            string str = row["ConcessionApplicable"].ToString().Trim();
            double num1 = Convert.ToDouble(row["amount"].ToString().Trim());
            double num2 = !(str == "Y") || num1 <= 0.0 ? num1 : num1 - num1 * (double)int32 / 100.0;
            Convert.ToInt32(row["FeeCompID"].ToString());
            new clsDAL().ExcuteProcInsUpdt("ps_sp_InsMlyFeeAllSingleFee", new Hashtable()
      {
        {
          (object) "AdmnNo",
          (object) AdmnNo
        },
        {
          (object) "TransDesc",
          (object) row["FeeName"].ToString().Trim()
        },
        {
          (object) "Amt",
          (object) num2
        },
        {
          (object) "userid",
          (object) uid
        },
        {
          (object) "schoolid",
          (object) SchoolId
        },
        {
          (object) "FeeId",
          (object) Convert.ToInt32(row["FeeCompID"].ToString().Trim())
        },
        {
          (object) "Session",
          (object) Sess
        },
        {
          (object) "FeeStartDt",
          (object) FeeStDate
        }
      });
        }
    }

    protected void GenerateHlyFeeOnAdmn(string AdmnNo, DateTime FeeStDate, string Sess, string uid, string SchoolId, DataTable dtFeeAmt)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("ps_sp_GetConcessionPer", new Hashtable()
    {
      {
        (object) "SessYr",
        (object) Sess
      },
      {
        (object) "AdmnNo",
        (object) AdmnNo
      }
    }));
        foreach (DataRow row in (InternalDataCollectionBase)dtFeeAmt.Rows)
        {
            string str = row["ConcessionApplicable"].ToString().Trim();
            double num1 = Convert.ToDouble(row["amount"].ToString().Trim());
            double num2 = !(str == "Y") || num1 <= 0.0 ? num1 : num1 - num1 * (double)int32 / 100.0;
            Convert.ToInt32(row["FeeCompID"].ToString());
            new clsDAL().ExcuteProcInsUpdt("ps_sp_InsHlyFeeAllSingleFee", new Hashtable()
      {
        {
          (object) "AdmnNo",
          (object) AdmnNo
        },
        {
          (object) "TransDesc",
          (object) row["FeeName"].ToString().Trim()
        },
        {
          (object) "Amt",
          (object) num2
        },
        {
          (object) "userid",
          (object) uid
        },
        {
          (object) "schoolid",
          (object) SchoolId
        },
        {
          (object) "FeeId",
          (object) Convert.ToInt32(row["FeeCompID"].ToString().Trim())
        },
        {
          (object) "Session",
          (object) Sess
        },
        {
          (object) "FeeStartDt",
          (object) FeeStDate
        }
      });
        }
    }

    protected void GenerateQtlyFeeOnAdmn(string AdmnNo, DateTime FeeStDate, string Sess, string uid, string SchoolId, DataTable dtFeeAmt)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("ps_sp_GetConcessionPer", new Hashtable()
    {
      {
        (object) "SessYr",
        (object) Sess
      },
      {
        (object) "AdmnNo",
        (object) AdmnNo
      }
    }));
        foreach (DataRow row in (InternalDataCollectionBase)dtFeeAmt.Rows)
        {
            string str = row["ConcessionApplicable"].ToString().Trim();
            double num1 = Convert.ToDouble(row["amount"].ToString().Trim());
            double num2 = !(str == "Y") || num1 <= 0.0 ? num1 : num1 - num1 * (double)int32 / 100.0;
            Convert.ToInt32(row["FeeCompID"].ToString());
            new clsDAL().ExcuteProcInsUpdt("ps_sp_InsQtlyFeeAllSingleFee", new Hashtable()
      {
        {
          (object) "AdmnNo",
          (object) AdmnNo
        },
        {
          (object) "TransDesc",
          (object) row["FeeName"].ToString().Trim()
        },
        {
          (object) "Amt",
          (object) num2
        },
        {
          (object) "userid",
          (object) uid
        },
        {
          (object) "schoolid",
          (object) SchoolId
        },
        {
          (object) "FeeId",
          (object) Convert.ToInt32(row["FeeCompID"].ToString().Trim())
        },
        {
          (object) "Session",
          (object) Sess
        },
        {
          (object) "FeeStartDt",
          (object) FeeStDate
        }
      });
        }
    }

    public void GenerateFeeOnClassPromotion(string newSession, string AdmnNos, string userid, string schoolid, int NewClass)
    {
        string str1 = new clsDAL().ExecuteScalarQry("select StreamId from dbo.PS_ClassMaster where ClassId=" + (object)NewClass);
        string strnew = new clsDAL().ExecuteScalarQry("select ClassId from dbo.PS_ClassMaster where ClassId=" + (object)NewClass);
        DateTime dateTime = Convert.ToDateTime(new clsDAL().ExecuteScalarQry("select StartDate from PS_FeeNormsNew where SessionYr = '" + newSession + "'"));
        clsDAL clsDal = new clsDAL();
        DataTable dataTable1 = new DataTable();
        string qry = "select classid,section,admnno from dbo.PS_ClasswiseStudent where SessionYear='" + newSession + "' and admnno in(" + AdmnNos + ") order by classid,section,admnno";
        foreach (DataRow row1 in (InternalDataCollectionBase)clsDal.GetDataTableQry(qry).Rows)
        {
            DataTable dataTable2 = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
      {
        {
          (object) "@SessYr",
          (object) newSession
        },
        {
          (object) "@StreamId",
          (object) str1
        },
        {
          (object) "@ClassId",
          (object) strnew
        },
        {
          (object) "@PeriodicityId",
          (object) 2
        },
        {
          (object) "@StudType",
          (object) "E"
        }
      });
            this.GenOT_AnnualFeeOnAdmn(row1["admnno"].ToString(), dataTable2, dateTime, newSession, userid, schoolid);
            clsDal = new clsDAL();
            int int32 = Convert.ToInt32(clsDal.ExecuteScalar("ps_sp_GetConcessionPer", new Hashtable()
      {
        {
          (object) "SessYr",
          (object) newSession
        },
        {
          (object) "AdmnNo",
          row1["admnno"]
        }
      }));
            foreach (DataRow row2 in (InternalDataCollectionBase)clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
      {
        {
          (object) "@SessYr",
          (object) newSession
        },
        {
          (object) "@StreamId",
          (object) str1
        },
        {
          (object) "@ClassId",
          (object) strnew
        },

        {
          (object) "@PeriodicityId",
          (object) 5
        },
        {
          (object) "@StudType",
          (object) "E"
        }
      }).Rows)
            {
                string str2 = row2["ConcessionApplicable"].ToString().Trim();
                double num1 = Convert.ToDouble(row2["amount"].ToString().Trim());
                double num2 = !(str2 == "Y") || num1 <= 0.0 ? num1 : num1 - num1 * (double)int32 / 100.0;
                new Common().GetDataTable("ps_sp_insert_FeeLedger", new Hashtable()
        {
          {
            (object) "TransDate",
            (object) dateTime
          },
          {
            (object) "AdmnNo",
            (object) Convert.ToInt64(row1["admnno"].ToString())
          },
          {
            (object) "TransDesc",
            (object) row2["FeeName"].ToString().Trim()
          },
          {
            (object) "debit",
            (object) num2
          },
          {
            (object) "credit",
            (object) 0
          },
          {
            (object) "balance",
            (object) num2
          },
          {
            (object) "Receipt_VrNo",
            (object) ""
          },
          {
            (object) "userid",
            (object) userid
          },
          {
            (object) "UserDate",
            (object) DateTime.Now.ToString("MM/dd/yyyy")
          },
          {
            (object) "schoolid",
            (object) schoolid
          },
          {
            (object) "FeeId",
            (object) Convert.ToInt32(row2["FeeCompID"].ToString().Trim())
          },
          {
            (object) "PayMode",
            (object) ""
          },
          {
            (object) "Session",
            (object) newSession
          }
        });
            }
        }
    }

    public void CheckForFeeGenerated(string userid, string schoolid)
    {
        this.CheckFeeGenerated(userid, schoolid);
        this.GenerateFine(userid, schoolid);
    }

    protected void CheckFeeGenerated(string uid, string schoolid)
    {
        this.curSess = this.CreateCurrSession();
        DataTable dataTable = new Common().ExecuteSql("select * from PS_FeeNormsNew where SessionYr = '" + this.curSess + "'");
        int ColNo = 0;
        int num = 0;
        while (num < 31)
        {
            ++ColNo;
            DateTime dateTime = Convert.ToDateTime(dataTable.Rows[0][num + 3].ToString());
            int int16 = (int)Convert.ToInt16(dataTable.Rows[0][num + 4].ToString());
            if (dateTime <= DateTime.Today && int16 == 0 && dateTime.ToString("dd/MM/yyyy") != "01/01/1900")
            {
                this.GenerateFeeAll(dateTime, schoolid, uid);
                if (this.feeLedgerGenerated)
                    this.updateFeeNorms(dateTime, ColNo);
            }
            num += 3;
        }
    }

    protected void updateFeeNorms(DateTime FeeDueDate, int ColNo)
    {
        new Common().ExecuteSql("update dbo.PS_FeeNormsNew set Due" + (object)ColNo + "_Generated=1 where DueDate" + (object)ColNo + " = '" + FeeDueDate.ToString("MM/dd/yyyy") + "' and SessionYr='" + this.curSess + "'");
    }

    protected void GenerateFeeAll(DateTime FeeDueDate, string schoolid, string userid)
    {
        Common common1 = new Common();
        DataTable dataTable1 = new DataTable();
        string qry = "select cs.classid,cs.section,cs.admnno from dbo.PS_ClasswiseStudent cs inner join PS_StudMaster s on cs.admnno=s.admnno  where SessionYear='" + this.curSess + "' and s.status=1 order by cs.classid,cs.section,cs.admnno";
        foreach (DataRow row1 in (InternalDataCollectionBase)common1.ExecuteSql(qry).Rows)
        {
            Common common2 = new Common();
            row1["admnno"].ToString();
            DataTable dataTable2 = common2.ExecuteSql("select SessionYear,ClassID,Stream,admnno from dbo.PS_ClasswiseStudent where SessionYear='" + this.curSess + "' and admnno =" + row1["admnno"]);
            DataTable dataTable3 = new Common().GetDataTable("ps_sp_GetConcessionPer", new Hashtable()
      {
        {
          (object) "SessYr",
          (object) this.curSess
        },
        {
          (object) "AdmnNo",
          row1["admnno"]
        }
      });
            DataTable dataTable4 = new Common().ExecuteSql("select SiblingNo from dbo.PS_SiblingDetails where AdmnNo =" + row1["admnno"]);
            int num1 = 0;
            if (dataTable4.Rows.Count > 0)
                num1 = Convert.ToInt32(dataTable4.Rows[0][0].ToString());
            foreach (DataRow row2 in (InternalDataCollectionBase)new Common().ExecuteSql("select a.FeeCompID,c.FeeName,a.Amount,c.PeriodicityID,c.FineApplicable,c.ConcessionApplicable from PS_FeeAmount a, PS_FeeComponents c where a.Amount > 0 and a.FeeCompID=c.FeeID and a.StreamID = " + dataTable2.Rows[0]["Stream"] + " and PeriodicityID > 2").Rows)
            {
                string str = row2["ConcessionApplicable"].ToString().Trim();
                double num2 = Convert.ToDouble(row2["amount"].ToString().Trim());
                int int32_1 = Convert.ToInt32(dataTable3.Rows[0][0]);
                double num3 = !(str == "Y") || num2 <= 0.0 ? num2 : num2 - num2 * (double)int32_1 / 100.0;
                int int32_2 = Convert.ToInt32(row2["FeeCompID"].ToString());
                if (num1 == 1 && int32_2 == 8 && num3 > 0.0)
                    num3 -= 100.0;
                else if (num1 == 2 && int32_2 == 8 && num3 > 0.0)
                    num3 -= 200.0;
                new Common().GetDataTable("ps_sp_insert_FeeLedger", new Hashtable()
        {
          {
            (object) "TransDate",
            (object) FeeDueDate.ToString("MM/dd/yyyy")
          },
          {
            (object) "AdmnNo",
            (object) Convert.ToInt64(row1["admnno"].ToString())
          },
          {
            (object) "TransDesc",
            (object) row2["FeeName"].ToString().Trim()
          },
          {
            (object) "debit",
            (object) num3
          },
          {
            (object) "credit",
            (object) 0
          },
          {
            (object) "balance",
            (object) num3
          },
          {
            (object) "Receipt_VrNo",
            (object) ""
          },
          {
            (object) "userid",
            (object) userid
          },
          {
            (object) "UserDate",
            (object) DateTime.Now.ToString("MM/dd/yyyy")
          },
          {
            (object) "schoolid",
            (object) schoolid
          },
          {
            (object) "FeeId",
            (object) Convert.ToInt32(row2["FeeCompID"].ToString().Trim())
          },
          {
            (object) "PayMode",
            (object) ""
          },
          {
            (object) "Session",
            (object) this.CreateCurrSession()
          }
        });
            }
            this.feeLedgerGenerated = true;
        }
    }

    public string CreateCurrSession()
    {
        int int32 = Convert.ToInt32(DateTime.Today.ToString("yyyy"));
        int month = DateTime.Today.Month;
        string str = "";
        if (month > 0 && month < 4)
            str = Convert.ToString(int32 - 1) + "-" + Convert.ToString(DateTime.Today.ToString("yy"));
        else if (month > 3 && month < 13)
            str = Convert.ToString(int32) + "-" + Convert.ToString(DateTime.Today.AddYears(1).ToString("yy"));
        return str;
    }

    public void updatebusfee()
    {
        Common common = new Common();
        int int32 = Convert.ToInt32(DateTime.Today.Month);
        string currSession = this.CreateCurrSession();
        int num = int32 != 1 ? int32 - 1 : 12;
        string qry = "select count(*) from dbo.PS_AdFeeLedger where SessionYr='" + currSession + "' and ad_id=1 and admonth=" + (object)int32;
        if (Convert.ToInt32(common.ExecuteScalarQry(qry)) <= 0)
            return;
        if (Convert.ToDouble(new Common().ExecuteSql(("select isnull(sum(Debit),0) from dbo.PS_AdFeeLedger where SessionYr='" + currSession + "' and ad_id=1 and admonth=" + (object)int32).ToString().Trim(), "")) != 0.0)
            return;
        new Common().ExcuteProcInsUpdt("ps_sp_update_AdFeeLedgerOnLogin", new Hashtable()
    {
      {
        (object) "@AdFeeDate",
        (object) this.CreateAdFeeDate()
      },
      {
        (object) "@UserId",
        (object) 1
      },
      {
        (object) "@SessionYr",
        (object) currSession
      },
      {
        (object) "@month",
        (object) num
      }
    });
    }

    public string CreateAdFeeDate()
    {
        string currSession = this.CreateCurrSession();
        string str1 = currSession.Substring(0, 4);
        string str2 = currSession.Substring(5, 2);
        int int32 = Convert.ToInt32(DateTime.Today.Month);
        string str3 = "";
        if (int32 > 0 && int32 < 4)
            str3 = int32.ToString().Trim() + "/01/" + str1.Substring(0, 2) + str2;
        else if (int32 > 3 && int32 < 13)
            str3 = int32.ToString().Trim() + "/01/" + str1;
        return str3;
    }

    public int ReGenerateFee(string admnno, string SessYr, double ConPer, double ExistingConPer)
    {
        int num1 = 0;
        DataSet dataSet = new DataSet();
        if (this.con.State == ConnectionState.Closed)
            this.con.Open();
        clsDAL clsDal1 = new clsDAL();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("select TransNo,AdmnNo,FeeId,Debit,Credit,Balance from PS_FeeLedger");
        stringBuilder.Append(" where admnno=" + admnno + " and debit = balance and debit > 0 ");
        stringBuilder.Append(" and FeeId in(select FeeId from dbo.PS_FeeComponents where ConcessionApplicable='Y')");
        stringBuilder.Append(" order by TransNo");
        DataTable dataTableQry = clsDal1.GetDataTableQry(stringBuilder.ToString().Trim());
        clsDAL clsDal2 = new clsDAL();
        foreach (DataRow row in (InternalDataCollectionBase)dataTableQry.Rows)
        {
            double num2 = Convert.ToDouble(row["Debit"]);
            double num3 = num2;
            if (ExistingConPer > 0.0)
                num3 = num2 / ExistingConPer * 100.0;
            double num4 = num3 - num3 * (ConPer / 100.0);
            long int64 = Convert.ToInt64(row["TransNo"]);
            new clsDAL().ExcuteQryInsUpdt("update PS_FeeLedger set Debit=" + (object)num4 + ", Balance=" + (object)num4 + " where TransNo=" + (object)int64);
            ++num1;
        }
        return num1;
    }

    public double ReGenFeeOnInstantConPercent(string admnno, string ConDate, string reason, string uid, double ConPer)
    {
        if (this.con.State == ConnectionState.Closed)
            this.con.Open();
        DataTable dataTable = new clsDAL().GetDataTable("ps_sp_FeeDueListForInstConcession", new Hashtable()
    {
      {
        (object) "@AdmnNo",
        (object) admnno
      }
    });
        double num1 = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
        {
            double num2 = Convert.ToDouble(row["Debit"]) * (ConPer / 100.0);
            double num3 = Convert.ToDouble(row["Debit"]) - num2;
            double num4 = Convert.ToDouble(row["Balance"]) - num2;
            long int64 = Convert.ToInt64(row["TransNo"]);
            new clsDAL().ExcuteQryInsUpdt("update PS_FeeLedger set Debit=" + (object)num3 + ", Balance=" + (object)num4 + " where TransNo=" + (object)int64);
            new clsDAL().ExcuteProcInsUpdt("ps_sp_insert_FeeAdjustment", new Hashtable()
      {
        {
          (object) "AdmnNo",
          (object) admnno
        },
        {
          (object) "AdjDate",
          (object) ConDate
        },
        {
          (object) "Reason",
          (object) reason
        },
        {
          (object) "FeeLedgerTransNo",
          (object) int64
        },
        {
          (object) "ConAmount",
          (object) num2
        },
        {
          (object) "UID",
          (object) uid
        }
      });
            ++num1;
        }
        return num1;
    }

    public double ReGenFeeOnInstantConMlyAmt(string admnno, string ConDate, string reason, string uid, double MlyAmt)
    {
        if (this.con.State == ConnectionState.Closed)
            this.con.Open();
        DataTable dataTable = new clsDAL().GetDataTable("ps_sp_FeeDueListForInstConcession", new Hashtable()
    {
      {
        (object) "@AdmnNo",
        (object) admnno
      }
    });
        double num1 = 0.0;
        DateTime dateTime1 = DateTime.Now.AddYears(-5);
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
        {
            DateTime dateTime2 = Convert.ToDateTime(row["TransDate"]);
            if (dateTime1.ToString("MM/dd/yyyy") != dateTime2.ToString("MM/dd/yyyy"))
            {
                double num2 = Convert.ToDouble(row["Debit"]) - MlyAmt;
                double num3 = Convert.ToDouble(row["Balance"]) - MlyAmt;
                long int64 = Convert.ToInt64(row["TransNo"]);
                new clsDAL().ExcuteQryInsUpdt("update PS_FeeLedger set Debit=" + (object)num2 + ", Balance=" + (object)num3 + " where TransNo=" + (object)int64);
                new clsDAL().ExcuteProcInsUpdt("ps_sp_insert_FeeAdjustment", new Hashtable()
        {
          {
            (object) "AdmnNo",
            (object) admnno
          },
          {
            (object) "AdjDate",
            (object) ConDate
          },
          {
            (object) "Reason",
            (object) reason
          },
          {
            (object) "FeeLedgerTransNo",
            (object) int64
          },
          {
            (object) "ConAmount",
            (object) MlyAmt
          },
          {
            (object) "UID",
            (object) uid
          }
        });
                dateTime1 = dateTime2;
                ++num1;
            }
        }
        return num1;
    }

    public double ReGenFeeOnInstantConTotAmt(string admnno, string ConDate, string reason, string uid, double TotAmt, string SessYr)
    {
        if (this.con.State == ConnectionState.Closed)
            this.con.Open();
        DataTable dataTable = new clsDAL().GetDataTable("ps_sp_FeeDueListForTotInstConcession", new Hashtable()
    {
      {
        (object) "@AdmnNo",
        (object) admnno
      },
      {
        (object) "@SessionYr",
        (object) SessYr
      }
    });
        double num1 = 0.0;
        foreach (DataRow row in (InternalDataCollectionBase)dataTable.Rows)
        {
            double num2 = Convert.ToDouble(row["Debit"]);
            long int64 = Convert.ToInt64(row["TransNo"]);
            double num3 = Convert.ToDouble(row["Balance"]);
            if (TotAmt > num2 || TotAmt == num2)
            {
                new clsDAL().ExcuteQryInsUpdt("update PS_FeeLedger set Debit=0, Balance=0 where TransNo=" + (object)int64);
                new clsDAL().ExcuteProcInsUpdt("ps_sp_insert_FeeAdjustment", new Hashtable()
        {
          {
            (object) "AdmnNo",
            (object) admnno
          },
          {
            (object) "AdjDate",
            (object) ConDate
          },
          {
            (object) "Reason",
            (object) reason
          },
          {
            (object) "FeeLedgerTransNo",
            (object) int64
          },
          {
            (object) "ConAmount",
            (object) Convert.ToDouble(row["Debit"])
          },
          {
            (object) "UID",
            (object) uid
          }
        });
                TotAmt -= num2;
            }
            else
            {
                new clsDAL().ExcuteQryInsUpdt("update PS_FeeLedger set Debit=" + (object)(num2 - TotAmt) + ", Balance=" + (object)(num3 - TotAmt) + " where TransNo=" + (object)int64);
                new clsDAL().ExcuteProcInsUpdt("ps_sp_insert_FeeAdjustment", new Hashtable()
        {
          {
            (object) "AdmnNo",
            (object) admnno
          },
          {
            (object) "AdjDate",
            (object) ConDate
          },
          {
            (object) "Reason",
            (object) reason
          },
          {
            (object) "FeeLedgerTransNo",
            (object) int64
          },
          {
            (object) "ConAmount",
            (object) TotAmt
          },
          {
            (object) "UID",
            (object) uid
          }
        });
                TotAmt = 0.0;
            }
            ++num1;
            if (TotAmt <= 0.0)
                break;
        }
        if (TotAmt > 0.0)
        {
            new clsDAL().ExcuteQryInsUpdt("update PS_StudCreditLedger set FeeCredit= " + (object)TotAmt + ",UserId=" + uid + ", UserDate=getdate() where admnno=" + admnno);
            num1 = TotAmt - 2.0 * TotAmt;
        }
        return num1;
    }

    public void GenerateFeeOnHostelAdmn(DateTime FeeStartDate, string AdmnNo, string FeeOT, string FeeAnnual, string AdmnSession, string UserId, string SchoolId)
    {
        string str = new clsDAL().ExecuteScalarQry("select ClassId from dbo.PS_ClasswiseStudent where SessionYear='" + AdmnSession + "' and admnno =" + AdmnNo);
        clsDAL clsDal = new clsDAL();
        DateTime dateTime = Convert.ToDateTime(clsDal.GetDataTableQry("select * from PS_FeeNormsNew where SessionYr = '" + AdmnSession + "'").Rows[0][2].ToString());
        if (FeeOT == "Y")
        {
            DataTable dataTable = clsDal.GetDataTable("Host_GetHostFeeCompAmt", new Hashtable()
      {
        {
          (object) "@SessYr",
          (object) AdmnSession
        },
        {
          (object) "@ClassId",
          (object) str
        },
        {
          (object) "@PeriodicityId",
          (object) 1
        }
      });
            this.GenOT_AnnualFeeOnHostAdmn(AdmnNo, dataTable, dateTime, AdmnSession, UserId, SchoolId);
        }
        if (FeeAnnual == "Y")
        {
            DataTable dataTable = clsDal.GetDataTable("Host_GetHostFeeCompAmt", new Hashtable()
      {
        {
          (object) "@SessYr",
          (object) AdmnSession
        },
        {
          (object) "@ClassId",
          (object) str
        },
        {
          (object) "@PeriodicityId",
          (object) 2
        }
      });
            this.GenOT_AnnualFeeOnHostAdmn(AdmnNo, dataTable, dateTime, AdmnSession, UserId, SchoolId);
        }
        DataTable dataTable1 = clsDal.GetDataTable("Host_GetHostFeeCompAmt", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@ClassId",
        (object) str
      },
      {
        (object) "@PeriodicityId",
        (object) 5
      }
    });
        this.GenerateMlyFeeOnHostAdmn(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable1);
    }

    protected void GenOT_AnnualFeeOnHostAdmn(string AdmnNo, DataTable dtFeeAmt, DateTime FeeDate, string session, string uid, string schoolid)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("Host_GetConcessionPer", new Hashtable()
    {
      {
        (object) "SessYr",
        (object) session
      },
      {
        (object) "AdmnNo",
        (object) AdmnNo
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
          (object) "TransDate",
          (object) FeeDate.ToString("MM/dd/yyyy")
        },
        {
          (object) "AdmnNo",
          (object) AdmnNo
        },
        {
          (object) "TransDesc",
          (object) row["FeeName"].ToString().Trim()
        },
        {
          (object) "debit",
          (object) num2
        },
        {
          (object) "credit",
          (object) 0
        },
        {
          (object) "balance",
          (object) num2
        },
        {
          (object) "Receipt_VrNo",
          (object) ""
        },
        {
          (object) "userid",
          (object) uid
        },
        {
          (object) "UserDate",
          (object) DateTime.Now.ToString("MM/dd/yyyy")
        },
        {
          (object) "schoolid",
          (object) schoolid
        },
        {
          (object) "FeeId",
          (object) Convert.ToInt32(row["FeeCompID"].ToString().Trim())
        },
        {
          (object) "PayMode",
          (object) ""
        },
        {
          (object) "Session",
          (object) session
        }
      });
        }
    }

    protected void GenerateMlyFeeOnHostAdmn(string AdmnNo, DateTime FeeStDate, string Sess, string uid, string SchoolId, DataTable dtFeeAmt)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("Host_GetConcessionPer", new Hashtable()
    {
      {
        (object) "SessYr",
        (object) Sess
      },
      {
        (object) "AdmnNo",
        (object) AdmnNo
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
          (object) "AdmnNo",
          (object) AdmnNo
        },
        {
          (object) "TransDesc",
          (object) row["FeeName"].ToString().Trim()
        },
        {
          (object) "Amt",
          (object) num2
        },
        {
          (object) "userid",
          (object) uid
        },
        {
          (object) "schoolid",
          (object) SchoolId
        },
        {
          (object) "FeeId",
          (object) Convert.ToInt32(row["FeeCompID"].ToString().Trim())
        },
        {
          (object) "Session",
          (object) Sess
        },
        {
          (object) "FeeStartDt",
          (object) FeeStDate
        }
      });
        }
    }

    public void GenerateFeeOnFeeMod(DateTime AdmnDate, DateTime FeeStartDate, string AdmnNo, string FeeOT, string FeeAnnual, string AdmnSession, string UserId, string SchoolId, char studType)
    {
        string str = new clsDAL().ExecuteScalarQry("select Stream from dbo.PS_ClasswiseStudent where SessionYear='" + AdmnSession + "' and admnno =" + AdmnNo);
        string strnew = new clsDAL().ExecuteScalarQry("select ClassId from dbo.PS_ClasswiseStudent where SessionYear='" + AdmnSession + "' and admnno =" + AdmnNo);
        clsDAL clsDal = new clsDAL();
        DateTime dateTime = Convert.ToDateTime(clsDal.GetDataTableQry("select * from PS_FeeNormsNew where SessionYr = '" + AdmnSession + "'").Rows[0][2].ToString());
        if (FeeOT == "Y")
        {
            DataTable dataTable = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
      {
        {
          (object) "@SessYr",
          (object) AdmnSession
        },
        {
          (object) "@StreamId",
          (object) str
        },
        {
          (object) "@ClassId",
          (object) strnew
        },
        {
          (object) "@PeriodicityId",
          (object) 1
        },
        {
          (object) "@StudType",
          (object) studType
        }
      });
            this.GenOT_AnnualFeeOnFeeMod(AdmnNo, dataTable, dateTime, AdmnSession, UserId, SchoolId);
        }
        if (FeeAnnual == "Y")
        {
            DataTable dataTable = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
      {
        {
          (object) "@SessYr",
          (object) AdmnSession
        },
        {
          (object) "@StreamId",
          (object) str
        },
        {
          (object) "@PeriodicityId",
          (object) 2
        },
        {
          (object) "@StudType",
          (object) studType
        }
      });
            this.GenOT_AnnualFeeOnFeeMod(AdmnNo, dataTable, dateTime, AdmnSession, UserId, SchoolId);
        }
        DataTable dataTable1 = clsDal.GetDataTable("ps_sp_get_FeeCompAmt", new Hashtable()
    {
      {
        (object) "@SessYr",
        (object) AdmnSession
      },
      {
        (object) "@StreamId",
        (object) str
      },
      {
        (object) "@PeriodicityId",
        (object) 5
      },
      {
        (object) "@StudType",
        (object) studType
      }
    });
        this.GenerateMlyFeeOnFeeMod(AdmnNo, FeeStartDate, AdmnSession, UserId, SchoolId, dataTable1);
    }

    protected void GenOT_AnnualFeeOnFeeMod(string AdmnNo, DataTable dtFeeAmt, DateTime FeeDate, string session, string uid, string schoolid)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("ps_sp_GetConcessionPer", new Hashtable()
    {
      {
        (object) "SessYr",
        (object) session
      },
      {
        (object) "AdmnNo",
        (object) AdmnNo
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
          (object) "TransDate",
          (object) FeeDate.ToString("MM/dd/yyyy")
        },
        {
          (object) "AdmnNo",
          (object) AdmnNo
        },
        {
          (object) "TransDesc",
          (object) row["FeeName"].ToString().Trim()
        },
        {
          (object) "debit",
          (object) num2
        },
        {
          (object) "credit",
          (object) 0
        },
        {
          (object) "balance",
          (object) num2
        },
        {
          (object) "Receipt_VrNo",
          (object) ""
        },
        {
          (object) "userid",
          (object) uid
        },
        {
          (object) "UserDate",
          (object) DateTime.Now.ToString("MM/dd/yyyy")
        },
        {
          (object) "schoolid",
          (object) schoolid
        },
        {
          (object) "FeeId",
          (object) Convert.ToInt32(row["FeeCompID"].ToString().Trim())
        },
        {
          (object) "PayMode",
          (object) ""
        },
        {
          (object) "Session",
          (object) session
        }
      });
        }
    }

    protected void GenerateMlyFeeOnFeeMod(string AdmnNo, DateTime FeeStDate, string Sess, string uid, string SchoolId, DataTable dtFeeAmt)
    {
        int int32 = Convert.ToInt32(new clsDAL().ExecuteScalar("ps_sp_GetConcessionPer", new Hashtable()
    {
      {
        (object) "SessYr",
        (object) Sess
      },
      {
        (object) "AdmnNo",
        (object) AdmnNo
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
          (object) "AdmnNo",
          (object) AdmnNo
        },
        {
          (object) "TransDesc",
          (object) row["FeeName"].ToString().Trim()
        },
        {
          (object) "Amt",
          (object) num2
        },
        {
          (object) "userid",
          (object) uid
        },
        {
          (object) "schoolid",
          (object) SchoolId
        },
        {
          (object) "FeeId",
          (object) Convert.ToInt32(row["FeeCompID"].ToString().Trim())
        },
        {
          (object) "Session",
          (object) Sess
        },
        {
          (object) "FeeStartDt",
          (object) FeeStDate
        }
      });
        }
    }
}
