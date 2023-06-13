using ASP;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Error : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnActivate_Click(object sender, EventArgs e)
    {
        DataSet dataSet = new DataSet();
        string str = Server.MapPath("XMLFiles") + "\\Detail.xml";
        if (File.Exists(str))
        {
            int num = (int)dataSet.ReadXml(str);
            if (dataSet.Tables.Count <= 0)
                return;
            DataTable dataTable = new DataTable();
            UpdateTempTableForExtend(dataSet.Tables[0]);
            Response.Redirect("Login.aspx");
        }
        else
        {
            lblMsg.Text = "There is some problem with validity, contact software vendor";
            lblMsg.ForeColor = Color.Red;
        }
    }

    protected void UpdateTempTableForExtend(DataTable dt)
    {
        DataRow[] dataRowArray = dt.Select();
        if (dataRowArray.Length > 0)
        {
            foreach (DataRow dataRow in dataRowArray)
                dataRow["ValidUpto"] = (object)txtActKey.Text.Trim();
            dt.AcceptChanges();
        }
        DataTable table = dt.Copy();
        DataSet dataSet = new DataSet();
        dataSet.Tables.Add(table);
        string path = Server.MapPath("XMLFiles") + "\\Detail.xml";
        if (File.Exists(path))
            File.Delete(path);
        FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
        dataSet.WriteXml((Stream)fileStream);
        fileStream.Close();
    }
}