using ASP;
using Classes.DA;
using RJS.Web.WebControl;
using System;
using System.Collections;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FeeManagement_FeeAdjustmentList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    private void fillgrid(string date)
    {
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (date != "")
            hashtable.Add("@AdjDate", PopCalendar2.GetDateValue());
        GridView1.DataSource = common.GetDataTable("ps_sp_get_AdjustmentFee", hashtable);
        GridView1.DataBind();
    }

    protected void btnshow_Click(object sender, EventArgs e)
    {
        if (txtdate.Text == "")
            ScriptManager.RegisterClientScriptBlock((Control)txtdate, txtdate.GetType(), "ShowMessage", "alert('Select Received Date');", true);
        else
            fillgrid(PopCalendar2.GetDateValue().ToString("MM/dd/yyyy"));
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("FeeAdjustment.aspx");
    }

    protected void btndelete_Click(object sender, EventArgs e)
    {
        if (Request["Checkb"] == null)
        {
            Response.Write("<script>alert('Select a checkbox');</script>");
        }
        else
        {
            string str = Request["Checkb"];
            char[] chArray = new char[1] { ',' };
            foreach (object obj in str.Split(chArray))
                new Common().GetDataTable("ps_sp_delete_FeeAdjustment", new Hashtable()
        {
          {
             "AdjustmentId",
            obj
          },
          {
             "userid",
             1
          }
        });
        }
        fillgrid(txtdate.Text);
    }
}