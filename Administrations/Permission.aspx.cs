using ASP;
using Classes.DA;
using System;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Administrations_Permission : System.Web.UI.Page
{
    private static string aid;
    private static string aname;
    protected void Page_Load(object sender, EventArgs e)
    {
        btnassperm.Attributes.Add("onclick", "javascript:return checkselect()");
        btnUnassign.Attributes.Add("onclick", "javascript:return checkselect()");
        if (Page.IsPostBack)
            return;
        if (Session["User"] != null)
            BindList();
        else
            Response.Redirect("Login.aspx");
    }

    private void BindList()
    {
        try
        {
            chklistapp.DataSource = new Common().ExecuteSql("select * from PS_PermPageMaster where moduleid=0");
            chklistapp.DataTextField = "PageName";
            chklistapp.DataValueField = "PermID";
            chklistapp.DataBind();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    private void GridBind(string appid, string app)
    {
        try
        {
            DataSet datasetQry = new Common().GetDatasetQry("select *,case when status=1 then 'Assigned' else 'Unassigned' end Assigned from PS_PermPageMaster inner join ps_userpermissions on USER_PERM_ID=PermID where user_id=" + Convert.ToInt32(Request.QueryString["uid"].ToString()) + " and moduleid in(" + appid + ")");
            datasetQry.Tables[0].Columns.Add("application");
            string[] strArray1 = appid.Split(',');
            string[] strArray2 = app.Split(',');
            for (int index = 0; index < strArray1.Length; ++index)
            {
                foreach (DataRow row in (InternalDataCollectionBase)datasetQry.Tables[0].Rows)
                {
                    if (row["moduleid"].ToString() == strArray1[index])
                        row["application"] = strArray2[index];
                }
            }
            gridperm.DataSource = datasetQry.Tables[0];
            gridperm.DataBind();
            gridperm.Visible = true;
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnassperm_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Control)btnassperm, btnassperm.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                string str = Request["Checkb"];
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str.Split(chArray))
                    new Common().GetDatasetQry("update PS_userpermissions set status=1 where user_id=" + Request.QueryString["uid"] + " and user_perm_id=" + Convert.ToInt32(obj.ToString()));
                GridBind(aid, aname);
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnUnassign_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Control)btnUnassign, btnUnassign.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                int int32 = Convert.ToInt32(Request.QueryString["uid"].ToString());
                string[] strArray = Request["Checkb"].Split(',');
                for (int index = 0; index < strArray.Length; ++index)
                {
                    Common common = new Common();
                    //"update PS_UserPermissions set status=0 where user_id=" + int32 + " and user_perm_id=" + Convert.ToInt32(strArray[index].ToString());
                    common.GetDatasetQry("update PS_UserPermissions set status=0 where user_id=" + int32 + " and user_perm_id=" + Convert.ToInt32(strArray[index].ToString()));
                }
                GridBind(aid, aname);
            }
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void chklistapp_SelectedIndexChanged(object sender, EventArgs e)
    {
        aid = "";
        aname = "";
        for (int index = 0; index < chklistapp.Items.Count; ++index)
        {
            if (chklistapp.Items[index].Selected)
            {
                aid = aid + chklistapp.Items[index].Value + ",";
                aname = aname + chklistapp.Items[index].Text + ",";
            }
        }
        if (aid != "")
        {
            aid = aid.Substring(0, aid.LastIndexOf(","));
            aname = aname.Substring(0, aname.LastIndexOf(","));
            GridBind(aid, aname);
        }
        else
            gridperm.Visible = false;
    }
}