﻿using ASP;
using SanLib;
using System;
using System.Collections;
using System.Data;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Library_DesignationList : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack)
            return;
       Session["title"] = Page.Title.ToString();
       FillGrid(0);
    }

    private void FillGrid(int DesignationId)
    {
       ht.Clear();
       ht.Add("@DesignationId", DesignationId);
       dt =obj.GetDataTable("Lib_SP_GetDesignationList",ht);
       grdDesigList.DataSource = dt;
       grdDesigList.DataBind();
       lblRecords.Text =dt.Rows.Count.ToString();
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
                string str =Request["Checkb"];
                char[] chArray = new char[1] { ',' };
                foreach (object obj in str.Split(chArray))
                   DeleteAccession(obj.ToString());
            }
           FillGrid(0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void DeleteAccession(string id)
    {
       ht.Clear();
       ht.Add("@DesignationId", id);
        string str =obj.ExecuteScalar("Lib_SP_DeleteDesignation",ht);
        if (str == "")
            ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert('Deleted Successfully');", true);
        else
            ScriptManager.RegisterClientScriptBlock((Page)this,GetType(), "ShowMessage", "alert(" + str + ");", true);
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
       Response.Redirect("DesignationMaster.aspx");
    }

    protected void grdDesigList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
           grdDesigList.PageIndex = e.NewPageIndex;
           FillGrid(0);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}