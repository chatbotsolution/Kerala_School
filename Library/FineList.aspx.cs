using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_FineList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["User_Id"] != null)
            {
               lblMsg.Text = string.Empty;
                if (Page.IsPostBack)
                    return;
               Session["title"] = Page.Title.ToString();
               pnlVar.Visible = false;
               pnlFixed.Visible = false;
               bindDropDown(drpFineType, "SELECT DISTINCT FineType,CASE WHEN FineType='V'\tTHEN 'Variable' ELSE 'Fixed' END AS FType FROM dbo.Lib_FineMaster where CollegeId=" +Session["SchoolId"], "FType", "FineType");
            }
            else
               Response.Redirect("../Login.aspx");
        }
        catch (Exception ex)
        {
        }
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry =obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---SELECT---", "0"));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
       ht = new Hashtable();
       ht.Add("@FineType", drpFineType.SelectedValue.ToString());
       ht.Add("@CollegeId",Session["SchoolId"]);
       dt =obj.GetDataTable("Lib_SP_GetFine",ht);
        if (dt.Rows.Count > 0)
        {
            if ((int)Convert.ToChar(dt.Rows[0]["FineType"].ToString()) == 86)
            {
               pnlVar.Visible = true;
               pnlFixed.Visible = false;
               grdVarFine.DataSource = dt;
               grdVarFine.DataBind();
               lblRecords.Text = "Total Record(s): " +dt.Rows.Count.ToString();
            }
            if ((int)Convert.ToChar(dt.Rows[0]["FineType"].ToString()) != 70)
                return;
           pnlVar.Visible = false;
           pnlFixed.Visible = true;
           grdFixedFine.DataSource = dt;
           grdFixedFine.DataBind();
           lblRecords.Text = "Total Record(s): " +dt.Rows.Count.ToString();
        }
        else
        {
           pnlVar.Visible = false;
           pnlFixed.Visible = false;
           lblMsg.Text = "No Records Found !";
           lblRecords.Text = string.Empty;
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                string id =Request["Checkb"];
                id.Split(',');
               DeleteFine(id);
               pnlVar.Visible = false;
               pnlFixed.Visible = false;
               lblMsg.Text = "No Records Found !";
               lblRecords.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
        }
    }

    private void DeleteFine(string id)
    {
       ht.Clear();
       ht.Add("@FineId", id);
       ht.Add("@CollegeId",Session["SchoolId"]);
        string str =obj.ExecuteScalar("Lib_SP_DeleteFine",ht);
        if (str == "")
            ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('Deleted Successfully');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(" + str + ");", true);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
       Response.Redirect("FineMaster.aspx");
    }
}