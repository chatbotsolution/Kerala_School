using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SanLib;
using System.Text;


public partial class insertfee : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataTable dt = new clsDAL().GetDataTableQry("select * from Ps_Classwisestudent where sessionyear='2018-19' and classid=3 order by admnno");
        foreach (DataRow row in dt.Rows)
        {
            new clsDAL().ExcuteProcInsUpdt("ps_sp_InsQtlyFeeAllSingleFee", new Hashtable()
      {
        {
          (object) "AdmnNo",
          (object) Convert.ToInt32(row["admnno"].ToString())
        },
        {
          (object) "TransDesc",
          (object) "Computer Fee"
        },
        {
          (object) "Amt",
          (object) 450
        },
        {
          (object) "userid",
          (object) Session["User_Id"].ToString()
        },
        {
          (object) "schoolid",
          (object) Session["SchoolId"].ToString()
        },
        {
          (object) "FeeId",
          (object) 11
        },
        {
          (object) "Session",
          (object) "2018-19"
        },
        {
          (object) "FeeStartDt",
          (object) Convert.ToDateTime("04/01/2018")
        }
      });
        }
    }
}