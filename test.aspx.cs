using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SanLib;
using System.Web.Services;
using AnthemUtility.AnthemSecurityEngine;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Data;

public partial class test : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        clsDAL clsDal = new clsDAL();
        //string str = clsDal.Decrypt(TextBox1.Text, "7011");
        //string str = clsDal.Encrypt(TextBox1.Text, "7011");
        //string str = CryptoEngine.Decrypt(TextBox1.Text.Trim(), "aGtSpL", "114208513", "SHA1", 5, "ANTHEMGLOBALTECH", 256);
        //Label1.Text = str;

    }

}