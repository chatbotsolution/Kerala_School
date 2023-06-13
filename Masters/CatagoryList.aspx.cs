using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Masters_CatagoryList : System.Web.UI.Page
{
    private static int editcatid = 0;
    private static string[] prevpantid = new string[100];
    private static int j = 0;
    private static int parentadd = 0;
    private clsDAL obj = new clsDAL();
    private DataTable dt = new DataTable();
    private Hashtable ht = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
        Page.Form.DefaultButton = btnAddNew.UniqueID;
        if (Session["User"] != null)
        {
            fillgrd(0, 0);
            lblTotRecord.Visible = true;
        }
        else
            Response.Redirect("~/Login.aspx");
    }

    private void fillgrd(int parentid, int pageindex)
    {
        DataTable dataTable = new DataTable();
        HiddenField1.Value = parentid.ToString();
        DataTable dataTableQry = obj.GetDataTableQry("select CatId, ParentCatId, CatName from SI_CategoryMaster");
        if (dataTableQry.Rows.Count > 0)
        {
            grdcatmaster.PageIndex = pageindex;
            grdcatmaster.DataSource = (object)dataTableQry.DefaultView;
            grdcatmaster.DataBind();
            lblnorecord.Visible = false;
            btnDelete.Enabled = true;
            lblTotRecord.Text = "Total Record(s) : " + dataTableQry.Rows.Count.ToString();
        }
        else
        {
            lblTotRecord.Text = string.Empty;
            lblMsg.Text = "No Record";
            grdcatmaster.DataSource = (object)null;
            grdcatmaster.DataBind();
            lblnorecord.Visible = true;
            btnDelete.Enabled = false;
        }
    }

    protected void grdcatmaster_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        grdcatmaster.PageIndex = e.NewPageIndex;
        fillgrd(Convert.ToInt32(HiddenField1.Value), grdcatmaster.PageIndex);
    }

    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("ItemCatagory.aspx");
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request["Checkb"] == null)
            {
                ScriptManager.RegisterClientScriptBlock((Control)btnDelete, btnDelete.GetType(), "ShowMessage", "alert('Select a checkbox');", true);
            }
            else
            {
                string[] strArray = Request["Checkb"].Split(',');
                string empty1 = string.Empty;
                string empty2 = string.Empty;
                for (int index = 0; index < strArray.Length; ++index)
                {
                    string str = DeleteCat(strArray[index].ToString());
                    if (str != "")
                    {
                        if (index > 0 && empty2 != "")
                            empty2 += ", ";
                        empty2 += str;
                    }
                }
                if (empty2 != "")
                {
                    lblMsg.Text = "Category <span style='color:Maroon'>[" + empty2 + "]</span> Can not be deleted ";
                    lblMsg.ForeColor = Color.Red;
                }
                else
                {
                    lblMsg.Text = "Record Deleted Successfully !";
                    lblMsg.ForeColor = Color.Green;
                }
                fillgrd(0, 0);
            }
        }
        catch (Exception ex)
        {
        }
    }

    private string DeleteCat(string Id)
    {
        string str = string.Empty;
        try
        {
            Hashtable ht = new Hashtable();
            DataTable dataTable = new DataTable();
            clsDAL clsDal = new clsDAL();
            ht.Add((object)"@CatId", (object)Id);
            str = clsDal.ExecuteScalar("SI_DelCat", ht);
        }
        catch (Exception ex)
        {
        }
        return str;
    }
}