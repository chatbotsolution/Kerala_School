using ASP;
using Classes.DA;
using System;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Reports_rptStudFeeReceipt : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.FillDropDown();
    }

    protected void FillDropDown()
    {
        this.drpStudent.DataSource = new Common().ExecuteSql("select AdmnNo,Convert(varchar(20),AdmnNo) + '-' + FullName as Student from dbo.PS_StudMaster order by AdmnNo");
        this.drpStudent.DataTextField = "";
        this.drpStudent.DataValueField = "";
        this.drpStudent.DataBind();
    }

    protected void btnShow_Click(object sender, EventArgs e)
    {
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
    }
}