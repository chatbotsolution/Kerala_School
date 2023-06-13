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

public partial class Hostel_InventoryReturn : System.Web.UI.Page
{
    private clsDAL obj = new clsDAL();
    private Hashtable ht = new Hashtable();
    private DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] != null)
        {
            lblMsg.Text = string.Empty;
            if (Page.IsPostBack)
                return;
            Page.Form.DefaultButton = btnSearch.UniqueID;
            bindDropDown(drpReturnedBy, "select S.FullName,H.AdmnNo from dbo.PS_StudMaster S inner join dbo.Host_Admission H on S.AdmnNo=H.AdmnNo Where H.LeavingDt is null or LeavingDt ='' ", "FullName", "AdmnNo");
            bindDropDown(drpReturnTo, "Select Location,LocationId from dbo.SI_LocationMaster where SchoolId=" + Convert.ToInt32(Session["SchoolId"]) + " and Location='Hostel Store'", "Location", "LocationId");
            drpReturnTo.Items.RemoveAt(0);
        }
        else
            Response.Redirect("../Login.aspx");
    }

    private void bindDropDown(DropDownList drp, string query, string textfield, string valuefield)
    {
        drp.DataSource = null;
        drp.DataBind();
        DataTable dataTableQry = obj.GetDataTableQry(query);
        drp.DataSource = dataTableQry;
        drp.DataTextField = textfield;
        drp.DataValueField = valuefield;
        drp.DataBind();
        drp.Items.Insert(0, new ListItem("---Select---", "0"));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindGrid();
        }
        catch (Exception ex)
        {
        }
    }

    private void BindGrid()
    {
        dt = obj.GetDataTableQry("Select S.ItemCode,I.ItemName,S.AvlQty from dbo.SI_LocationwiseStock S inner join dbo.SI_ItemMaster I on I.ItemCode=S.ItemCode where IsConsumable=0 and AvlQty > 0 and LocationId=" + drpReturnedBy.SelectedValue);
        grdItem.DataSource = dt;
        grdItem.DataBind();
        if (dt.Rows.Count > 0)
            btnReturn.Visible = true;
        else
            btnReturn.Visible = false;
    }

    protected void btnReturn_Click(object sender, EventArgs e)
    {
        try
        {
            string str1 = "";
            string str2 = "";
            string str3 = "";
            bool flag1 = true;
            bool flag2 = false;
            foreach (GridViewRow row in grdItem.Rows)
            {
                HiddenField control1 = (HiddenField)row.FindControl("hdnItemCode");
                TextBox control2 = (TextBox)row.FindControl("txtReturnQty");
                HiddenField control3 = (HiddenField)row.FindControl("hdnMaxQtyToReturn");
                if (control2.Text != "")
                {
                    str1 = str1 + control1.Value.ToString() + ",";
                    str2 = str2 + control2.Text + ",";
                    str3 = str3 + control3.Value.ToString() + ",";
                    if ((double)float.Parse(control2.Text) > (double)float.Parse(control3.Value))
                    {
                        flag1 = false;
                        break;
                    }
                    flag2 = true;
                }
            }
            if (flag1)
            {
                if (!flag2)
                    return;
                ht.Clear();
                ht.Add("@FromLocationId", drpReturnedBy.SelectedValue);
                ht.Add("@ToLocationId", drpReturnTo.SelectedValue);
                ht.Add("@ItemcodeList", str1);
                ht.Add("@ReturnQtyList", str2);
                ht.Add("@AvlQtyList", str3);
                ht.Add("@ReturnedBy", drpReturnedBy.SelectedItem.ToString());
                ht.Add("@UserId", Session["User_Id"].ToString());
                ht.Add("@SchoolId", Session["SchoolId"].ToString());
                obj.ExcuteProcInsUpdt("HOST_InsReturn", ht);
                lblMsg.Text = "Items Returned Successfully";
                lblMsg.ForeColor = Color.Green;
            }
            else
            {
                lblMsg.Text = "Return Quantity Must not be greater than Avl Qty to return";
                lblMsg.ForeColor = Color.Red;
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Item Return Failed";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void grdItem_PreRender(object sender, EventArgs e)
    {
    }
}