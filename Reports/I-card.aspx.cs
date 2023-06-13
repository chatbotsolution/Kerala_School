using ASP;
using Classes.DA;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Web.Profile;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using BarcodeLib;
using System.Drawing.Text;
using System.Drawing.Drawing2D;

public partial class Reports_I_card : System.Web.UI.Page
    
{
    System.Drawing.Image img;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["User_Id"] == null)
            Response.Redirect("../Login.aspx");
        if (Page.IsPostBack)
            return;
        FillSession();
        FillClass();
        FillSectionDropDown();
        createdt();
    }
    public void createdt()
    {
        DataTable dtformultselec = new DataTable();
        dtformultselec.Columns.Add("OldAdmnNo");
        dtformultselec.Columns.Add("AdmnNo");
        dtformultselec.Columns.Add("FullName");
        ViewState["dtformultselec"] = dtformultselec;
    }
    protected void FillSession()
    {
        drpSession.DataSource = new Common().ExecuteSql("select SessionID,SessionYr from dbo.PS_FeeNormsNew order  by SessionID desc");
        drpSession.DataTextField = "SessionYr";
        drpSession.DataValueField = "SessionYr";
        drpSession.DataBind();
    }

    private void FillSectionDropDown()
    {
        drpSection.DataSource = new Common().GetDataTable("ps_sp_get_ClassSections", new Hashtable()
    {
      {
         "classId",
         drpClass.SelectedValue
      },
      {
         "session",
         drpSession.SelectedValue
      }
    });
        drpSection.DataTextField = "Section";
        drpSection.DataValueField = "Section";
        drpSection.DataBind();
        drpSection.Items.Insert(0, new ListItem("ALL", "0"));
    }

    private void FillClass()
    {
        drpClass.Items.Clear();
        drpClass.DataSource = new Common().GetDataTable("ps_sp_get_classesForDDL");
        drpClass.DataTextField = "classname";
        drpClass.DataValueField = "classid";
        drpClass.DataBind();
        drpClass.Items.Insert(0, new ListItem("--Select--", "0"));
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        
        DataTable dataTable2 = new DataTable();
        DataTable dt = (DataTable)ViewState["dtformultselec"];
        if (drpClass.SelectedIndex>0)//-1
        {
            Common common = new Common();
            Hashtable hashtable = new Hashtable();
            if (drpSession.Items.Count > 0)
                hashtable.Add("@Session", drpSession.SelectedValue);
            if (drpClass.SelectedValue.ToString().Trim() != "0")
                hashtable.Add("@Class", drpClass.SelectedValue);
            if (drpSection.SelectedValue.ToString().Trim() != "0")
                hashtable.Add("@Section", drpSection.SelectedValue);
            if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
                hashtable.Add("@Admnno", drpSelectStudent.SelectedValue);
            dataTable2 = common.GetDataTable("ps_sp_get_StudInfoForTempICard ", hashtable);
        }
        else
        {
            int x = 0;
            foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
            {
                Common common = new Common();
                Hashtable hashtable = new Hashtable();
                if (drpSession.Items.Count > 0)
                    hashtable.Add("@Session", drpSession.SelectedValue);
                if (drpSelectStudent.SelectedValue.ToString().Trim() != "0")
                    hashtable.Add("@Admnno", row["AdmnNo"]);
                if (x == 0)
                {
                    dataTable2 = common.GetDataTable("ps_sp_get_StudInfoForTempICard ", hashtable); x++;
                }
                else
                {
                    DataRow dr = (common.GetDataTable("ps_sp_get_StudInfoForTempICard ", hashtable)).Rows[0];
                    dataTable2.ImportRow(dr);
                }
                    
            }

        }
        if (dataTable2.Rows.Count > 0)
        {
            GenerateReport(dataTable2);
            btnPrint.Enabled = true;
            btnExpExcel.Enabled = true;
            dataTable2.Clear();
        }
        else
        {
            lblReport.Text = "<div style='background-color:Gray; color:White;'  class='tblheader'  align='center'>No Records Found  !</div>";
            btnPrint.Enabled = false;
            btnExpExcel.Enabled = false;
        }
    }
    private void GenerateReport(DataTable dt)
    {
        StringBuilder stringBuilder = new StringBuilder("");
        int num = 0;
        stringBuilder.Append("<table style='width: 100% !important;height:100% !important;display: table;float: left;'>");
        foreach (DataRow row in (InternalDataCollectionBase)dt.Rows)
        {
            string table1;
            //stringBuilder.Append("<div style='margin:0px;padding: 0px; padding-bottom:10px; width: 100%'>");
            if (drpside.SelectedValue == "1")
                table1= CreateTable2(row, num);
            else if (drpside.SelectedValue == "2")
                table1 = CreateTable1(row, num);
            else
                table1 = CreateTable2(row, num);
            //string table2 = CreateTable(row);
            stringBuilder.Append(table1);
            //stringBuilder.Append("</div>");
            //stringBuilder.Append("<hr/>");
            ++num;
        }
        stringBuilder.Append("</table>");
        lblReport.Text = stringBuilder.ToString().Trim();
        Session["TempICard"] = stringBuilder.ToString().Trim();
    }
    private string CreateTable2(DataRow row, int num)
    {
       TextInfo info = CultureInfo.CurrentCulture.TextInfo;
        string FullName = info.ToTitleCase(row["FullName"].ToString().ToLower());
        string FatherName = info.ToTitleCase(row["FatherName"].ToString().ToLower());
        string MotherName = info.ToTitleCase(row["MotherName"].ToString().ToLower());
        string PresAddr1 = info.ToTitleCase(row["PresAddr1"].ToString().ToLower());
        string PresAddr2 = info.ToTitleCase(row["PresAddr2"].ToString().ToLower());
        string[] icard = row["OldAdmnNo"].ToString().Split('/');
        string admnno = icard[0] + "_" + icard[1];
        StringBuilder stringBuilder = new StringBuilder();
        if (num % 7 == 0)
            stringBuilder.Append("<table style='height:20px;width: 100%;'><tr><td>  &nbsp;   </td></tr></table>");
        //stringBuilder.Append("<div style='position: absolute;height:30px;'> </div>");
        //stringBuilder.Append("<table style='font-size:10;margin-top: 10px; padding-left: 15px; padding-right: 15px; width: 5.5cm !important;height:8.7cm !important; float: left; border:solid 1px red; display: table'>");
        //stringBuilder.Append("<table style='width: 5.5cm !important;height:8.7cm !important;display: table;float: left;'>");
        stringBuilder.Append("<table style='font-family: Arial;background-image: url(../images/id_card_front.png);background-repeat:no-repeat;background-size: cover; padding-left: 10px; padding-right: 10px;width: 12.5% !important;height:40% !important;display: table;float: left;border:solid 3px white;'>");
        stringBuilder.Append("<tr><td style='font-size: 11px;color:white;text-align: center;white-space: nowrap;' colspan='2' ><b> KERALA ENGLISH MEDIUM SCHOOL</b></td></tr>");
        stringBuilder.Append("<tr><td style='font-size: 9px;color:white;text-align: center;white-space: nowrap;' colspan='2'> (Affiliated to C.B.S.E.,Delhi)</td></tr>");//,06767 - 240177
        stringBuilder.Append("<tr><td style='font-size: 8px;color:Black;text-align: center;white-space: nowrap;text-shadow: 0 0 3px white;' colspan='2'> Champua,Keonjhor-758041</td></tr>");//,06767 - 240177
        stringBuilder.Append("<tr><td style='height:27px;'></td></tr>");
        stringBuilder.Append("<tr><td style='font-size: 10px;color:black;text-align: center;white-space: nowrap;' colspan='2'>  IDENTITY CARD (" + drpSession.SelectedValue + ")</td></tr>");

        stringBuilder.Append("<tr>");
        //stringBuilder.Append("<td colspan='2' style='float: center;'>");
        stringBuilder.Append("<td colspan='2' style='width:80px; height:95px; text-align: center;font-size: 7px;'>");
        stringBuilder.Append("<div>");
        if(row["StudentPhoto"].ToString() != "" & row["StudentPhoto"].ToString() != "NoImage.jpg")
            stringBuilder.Append("<img src='https://kemschampua.co.in/Temp_Files/temp_img/"+row["StudentPhoto"].ToString()+ "' width='75px' height='95px' alt='Stamp Photo'style='border:solid 2px #A64B2A;'>");
        else
            stringBuilder.Append("<img src='../images/noimage.png' width='65px' height='80px' alt='Stamp Photo'style='border:solid 1px #A64B2A;'>");
        stringBuilder.Append("</div>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");

        stringBuilder.Append("<tr><td  style='font-size: 13px;text-align: center;white-space: nowrap;width:80px;height:30px;' colspan='2'><b style='color: #A64B2A;'>" + FullName + "</b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: center;white-space: nowrap;' colspan='2'><b>Class :</b>&nbsp<b style='color: black; '>" + row["ClassName"] + "</b>&nbsp&nbsp&nbsp <b>Sec :</b>&nbsp<b style='color: black; '>" + row["Section"] + "</b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: center;white-space: nowrap;'><b>Adm No:</b><b style='color: black; '>" + row["OldAdmnNo"] + "</b></td>    <td style='font-size: 10px;text-align: center;white-space: nowrap;'> <b>D.O.B:</b><b style='color: black; '>" + row["dob"] + "</b>&nbsp&nbsp&nbsp</td></tr>");
        //stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: left;white-space: nowrap;' colspan='2'><b>Blood Group :</b>&nbsp<b style='color: blue; '>" + row["BloodGroup"] + "</b></td></tr>");
       
        stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: center;width:80px;height:17px;'colspan='2'><b>Mobile&nbsp:&nbsp" + row["MobileNo1"] + ","+ row["MobileNo2"] + "</b></td></tr>");
        stringBuilder.Append("<tr><td style='height:20px;'></td></tr>");
        stringBuilder.Append("<tr><td style='text-align: center;'colspan='2'><img src='../images/principal.jpg' width='55px' height='30'></td></tr>");
        stringBuilder.Append("<tr><td style='text-align: center;' colspan='2'><b style='color: white;font-size: 8px;'>PRINCIPAL</b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: center;white-space: nowrap;color:yellow;height:10px;' colspan='2'></td></tr>");
        
        stringBuilder.Append("</table>");
        //stringBuilder.Append("</div>");

        return stringBuilder.ToString();
    }
    private string CreateTable(DataRow row,int num)
    {
        TextInfo info = CultureInfo.CurrentCulture.TextInfo;
        string FullName = info.ToTitleCase(row["FullName"].ToString().ToLower());
        string FatherName = info.ToTitleCase(row["FatherName"].ToString().ToLower());
        string MotherName = info.ToTitleCase(row["MotherName"].ToString().ToLower());
        string PresAddr1 = info.ToTitleCase(row["PresAddr1"].ToString().ToLower());
        string PresAddr2 = info.ToTitleCase(row["PresAddr2"].ToString().ToLower());
        string[] icard = row["OldAdmnNo"].ToString().Split('/');
        string admnno = icard[0] + "_" + icard[1];
        StringBuilder stringBuilder = new StringBuilder();
        if (num % 10 == 0)
            stringBuilder.Append("<table><tr><td style='height:43px;'>  &nbsp;   </td></tr></table>");
        //stringBuilder.Append("<div style='position: absolute;height:30px;'> </div>");
        //stringBuilder.Append("<table style='font-size:10;margin-top: 10px; padding-left: 15px; padding-right: 15px; width: 5.5cm !important;height:8.7cm !important; float: left; border:solid 1px red; display: table'>");
        //stringBuilder.Append("<table style='width: 5.5cm !important;height:8.7cm !important;display: table;float: left;'>");
        stringBuilder.Append("<table style='font-family: Arial;background-image: url(../images/id_card_scl_back_2.jpg);background-repeat:no-repeat;background-size: cover;margin-top: 10px; padding-left: 10px; padding-right: 10px;width: 20% !important;height:40% !important;display: table;float: left;border:solid 1px red;'>");
        stringBuilder.Append("<tr><td style='font-size: 13px;color:black;text-align: center;white-space: nowrap;' colspan='2' ><b> Kerala English Medium School</b></td></tr>");
        stringBuilder.Append("<tr><td style='font-size: 7px;color:black;text-align: center;white-space: nowrap;' colspan='2'> Champua-758041,06767 - 240177</td></tr>");
        stringBuilder.Append("<tr><td style='font-size: 10px;color:black;text-align: center;white-space: nowrap;' colspan='2'>IDENTITY CARD(" + drpSession.SelectedValue + ")</td></tr>");

        stringBuilder.Append("<tr>");
        //stringBuilder.Append("<td colspan='2' style='float: center;'>");
        stringBuilder.Append("<td colspan='2' style='width:70px; height:85px; text-align: center;font-size: 7px;'>");
        stringBuilder.Append("<div>");
        stringBuilder.Append("<img src='../images/student.png' width='70px' height='85px' alt='Stamp Photo';>");
        stringBuilder.Append("</div>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");

        stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: center;white-space: nowrap;' colspan='2'><b style='color: black; '>" + FullName + "</b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 12px;text-align: left;white-space: nowrap;' colspan='2'><b>Class :</b>&nbsp<b style='color: black; '>" + row["ClassName"] + "</b>&nbsp&nbsp&nbsp <b>Sec :</b>&nbsp<b style='color: black; '>" + row["Section"] + "</b>&nbsp&nbsp&nbsp <b>Roll :</b>&nbsp<b style='color: black; '></b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: left;white-space: nowrap;'><b>Adm No :</b>&nbsp<b style='color: black; '>" + row["OldAdmnNo"] + "</b></td>    <td style='font-size: 10px;text-align: center;white-space: nowrap;'> <b>D.O.B :</b>&nbsp<b style='color: black; '>" + row["dob"] + "</b>&nbsp&nbsp&nbsp</td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: left;white-space: nowrap;' colspan='2'><b>Blood Group :</b>&nbsp<b style='color: black; '>" + row["BloodGroup"] + "</b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: center;white-space: nowrap;' colspan='2'><b><u>Personal Details</u></b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: left;white-space: nowrap;' colspan='2'><b>Father's Name :</b>&nbsp<b style='color: black; '>" + FatherName + " </b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: left;white-space: nowrap;' colspan='2'><b>Mother's Name :</b>&nbsp<b style='color: black; '>" + MotherName + " </b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: left;'colspan='2'><b><div>Address :-</div><div style='margin-left:42px; margin-top:-10px; width:80%;height:40px;'> </div></b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 10px;text-align: left;width:95px;height:35;'><b>Mobile&nbsp:&nbsp </b></td><td><img src='../images/principal.png' width='55px' height='25' style='margin-left:25px; '><b style='color: black;font-size: 8px;text-align: right;'>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbspPRINCIPAL</b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: center;white-space: nowrap;color:black;' colspan='2'></td></tr>");
        
        stringBuilder.Append("</table>");
        //stringBuilder.Append("</div>");

        return stringBuilder.ToString();
    }
    private string CreateTable1(DataRow row, int num)
    {
        TextInfo info = CultureInfo.CurrentCulture.TextInfo;
        string FullName = info.ToTitleCase(row["FullName"].ToString().ToLower());
        string FatherName = info.ToTitleCase(row["FatherName"].ToString().ToLower());
        string MotherName = info.ToTitleCase(row["MotherName"].ToString().ToLower());
        string PresAddr1 = info.ToTitleCase(row["PresAddr1"].ToString().ToLower());
        string PresAddr2 = info.ToTitleCase(row["PresAddr2"].ToString().ToLower());
        string[] icard = row["OldAdmnNo"].ToString().Split('/');
        string admnno = icard[0] + "_" + icard[1];
        StringBuilder stringBuilder = new StringBuilder();
        if (num % 7 == 0)
            stringBuilder.Append("<table style='height:20px;width: 100%;'><tr><td>  &nbsp;   </td></tr></table>");
        //stringBuilder.Append("<div style='position: absolute;height:30px;'> </div>");
        //stringBuilder.Append("<table style='font-size:10;margin-top: 10px; padding-left: 15px; padding-right: 15px; width: 5.5cm !important;height:8.7cm !important; float: left; border:solid 1px red; display: table'>");
        //stringBuilder.Append("<table style='width: 5.5cm !important;height:8.7cm !important;display: table;float: left;'>");
        stringBuilder.Append("<table style='font-family: Arial;background-image: url(../images/backBagraund.png);background-repeat:no-repeat;background-size: cover;padding-left: 10px; padding-right: 10px;width: 14% !important;height:40% !important;display: table;float: left;border:solid 3px white;'>");
        //stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: left;white-space: nowrap;'>&nbsp</td></tr>");
        stringBuilder.Append("<tr>");
        //stringBuilder.Append("<td colspan='2' style='float: center;'>");
        stringBuilder.Append("<td style='width:65px; height:57px; text-align: center;'>");
        stringBuilder.Append("<div>");
        stringBuilder.Append("<img src='../images/logo-new.png' width='55' height='48' alt='logo';>");
        stringBuilder.Append("</div>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr><td style='text-align: center;'><b style='font-size: 10px;white-space: nowrap;'>KERALA ENGLISH MEDIUM SCHOOL</b></br><b style='font-size: 8px;white-space: nowrap;'>(AFFILIATED TO C.B.S.E., DELHI)</b></br><b style='font-size: 8px;white-space: nowrap;'>SHIKSHA NAGARI, CHAMPUA,KEONJHAR</b></br><b style='font-size: 8px;white-space: nowrap;'>ODISHA - 758041, Ph: 06767-240177</b></td></tr>");
        //stringBuilder.Append("<tr><td  style='font-size: 12px;text-align: center;white-space: nowrap;'></td></tr>");

        stringBuilder.Append("<tr><td  style='font-size: 12px;text-align: center;white-space: nowrap;color: #A64B2A;'><b>" + FullName + "</b></td></tr>");
        stringBuilder.Append("<tr>");
        stringBuilder.Append("<td style=' text-align: center;'>");
        stringBuilder.Append("<div>");
        stringBuilder.Append("<img src='"+ generatebarcode(row["admnno"].ToString())+"' width='90%' height='30' alt='logo';>");
        stringBuilder.Append("</div>");
        stringBuilder.Append("</td>");
        stringBuilder.Append("</tr>");
        stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: center;white-space: nowrap;' colspan='2'><b>"+ row["admnno"].ToString() + "</b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: center;white-space: nowrap;' colspan='2'><b><u>Personal Details</u></b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: left;white-space: nowrap;' colspan='2'>Father   :&nbsp<b style='color: black; '>" + FatherName + " </b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: left;white-space: nowrap;' colspan='2'>Mother :&nbsp<b style='color: black; '>" + MotherName + " </b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: left;'colspan='2'><div>Address :</div><div style='margin-left:42px; margin-top:-10px; width:80%;height:45px;'><b>" + PresAddr1 + PresAddr2 + row["PresAddrCity"] + row["PresAddrPin"] + "</b></div></td></tr>");

        //stringBuilder.Append("<tr><td style='font-size: 13px;color:black;text-align: center;white-space: nowrap;' colspan='2' > LOYOLA SCHOOL</td></tr>");
        //stringBuilder.Append("<tr><td style='font-size: 9px;color:black;text-align: center;white-space: nowrap;' colspan='2'> BHUBANESWAR-751023,(0674)-2300791</td></tr>");
        //stringBuilder.Append("<tr></tr>");
        //stringBuilder.Append("<tr><td style='font-size: 10px;color:black;text-align: center;white-space: nowrap;' colspan='2'> IDENTITY CARD(" + drpSession.SelectedValue + ")</td></tr>");

        
        //stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: left;white-space: nowrap;'>&nbsp</td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: center;white-space: nowrap;'><b><u>Rules and Regulations</u></b></td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: left;white-space: nowrap;'>1. This ID card is valid for academic session ("+drpSession.SelectedValue+ ")</br>&nbsp&nbsp&nbsp or till the student leaves school, during the session.</td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: left;white-space: nowrap;'>2. This card is not Transferable.</td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: left;white-space: nowrap;'>3. This card should be produced on demand.</br>&nbsp&nbsp&nbsp Especially, when issuing library books.</td></tr>");
        stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: left;white-space: nowrap;'>4. In case of loss/damage, Rs.100/- shall be charged</br>&nbsp&nbsp&nbsp for a new card.</td></tr>");
        //stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: left;'><b><div>Address :-</div><div style='margin-left:42px; margin-top:-10px; width:80%;height:40px;'>" + row["PresAddr1"] + " " + row["PresAddrPin"] + "</div></b></td></tr>");
        //stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: left;width:95px;height:35;'><b>Mobile&nbsp:&nbsp" + row["MobileNo1"] + ", " + row["MobileNo2"] + ", " + row["TelNoResidence"] + " </b></td></tr>");
        //stringBuilder.Append("<tr><td  style='font-size: 8px;text-align: center;white-space: nowrap;color:white;'>'In all Things to Love and to Serve' -St Ignatious of Loyola</td></tr>");
        //stringBuilder.Append("<tr><td  style='font-size: 11px;text-align: left;white-space: nowrap;'>&nbsp</td></tr>");
        stringBuilder.Append("<tr><td style='height:5px;'></td></tr>");
        stringBuilder.Append("</table>");
        //stringBuilder.Append("</div>");

        return stringBuilder.ToString();
    }
    private string generatebarcode(string barCode)
    {
        ////string barCode = admnno;
        //using (Bitmap bitMap = new Bitmap(barCode.Length * 21, 85))
        //{
        //    using (Graphics graphics = Graphics.FromImage(bitMap))
        //    {
        //        Font oFont = new Font("IDAutomationHC39M Free Version", 16);
        //        PointF point = new PointF(2f, 2f);
        //        SolidBrush blackBrush = new SolidBrush(Color.Black);
        //        SolidBrush whiteBrush = new SolidBrush(Color.White);
        //        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
        //        graphics.DrawString(barCode, oFont, blackBrush, point);
        //    }
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        bitMap.Save(ms, ImageFormat.Png);
        //        string str = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
        //        return str;
        //    }
        //}

        //string str;
        //using (Bitmap bitMap = new Bitmap(barCode.Length * 21, 80))
        //{
        //    using (Graphics graphics = Graphics.FromImage(bitMap))
        //    {
        //        Font oFont = new Font("IDAutomationHC39M Free Version", 16);
        //        PointF point = new PointF(2f, 2f);
        //        SolidBrush blackBrush = new SolidBrush(Color.Black);
        //        SolidBrush whiteBrush = new SolidBrush(Color.White);
        //        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
        //        graphics.DrawString( barCode , oFont, blackBrush, point);
        //    }
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        bitMap.Save(ms, ImageFormat.Png);
        //        byte[] byteImage = ms.ToArray();
        //        str = "data:image/Png;base64," + Convert.ToBase64String(byteImage);
        //        return str;
        //    }
        //}


        string str;
        Barcode barcode = new Barcode();
        Color foreColor = Color.Black;
        Color backColor = Color.White;//Transparent;
        img = barcode.Encode(TYPE.CODE128, barCode, foreColor, backColor);
        using (MemoryStream ms = new MemoryStream())
        {            
            img.Save(ms, ImageFormat.Png);
            byte[] byteImage = ms.ToArray();
            str = "data:image/Png;base64," + Convert.ToBase64String(byteImage);
            return str;
        }
    }

    private void ExportToExcel(string dataToExport)
    {
        try
        {
            string str = Server.MapPath("Exported_Files/StudentICardrpt" + DateTime.Now.ToString("dd-MM-yyyy hh-mm-ss") + ".xls");
            FileStream fileStream = new FileStream(str, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
            streamWriter.WriteLine(dataToExport.ToString().Trim());
            streamWriter.Close();
            fileStream.Close();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("content-disposition", "attachment;filename=" + str);
            Response.WriteFile(str);
            Response.End();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            //Response.Write("<script language='javascript'>alert('a');</script>");
            ScriptManager.RegisterClientScriptBlock((Page)this, GetType(), "ShowMessage", "window.open('rptTemporaryIdCardPrint.aspx');", true);
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void btnExpExcel_Click(object sender, EventArgs e)
    {
        try
        {
            if (lblReport.Text.ToString().Trim() != "")
            {
                StringBuilder stringBuilder = new StringBuilder("");
                stringBuilder.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0'>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td align='left' colspan='14'><b>");
                stringBuilder.Append("Temporary ICard Report :-");
                stringBuilder.Append("</b></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr></table>");
                stringBuilder.Append(lblReport.Text.ToString().Trim());
                stringBuilder.Append("<table><tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("<tr><td style='height: 10px'></td></tr>");
                stringBuilder.Append("</table>");
                ExportToExcel(stringBuilder.ToString());
            }
            else
                Response.Write("<script language='javascript'>alert('No data exist to export');</script>");
        }
        catch (Exception ex)
        {
            string message = ex.Message;
        }
    }

    protected void drpClass_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillSectionDropDown();
        FillSelectStudent();
    }
    protected void drpSection_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillSelectStudent();
    }

    private void FillSelectStudent()
    {
        Common common = new Common();
        Hashtable hashtable = new Hashtable();
        if (drpSession.Items.Count > 0)
            hashtable.Add("@SessionYear", drpSession.SelectedValue);
        if (drpClass.SelectedIndex > 0)
            hashtable.Add("@ClassID", drpClass.SelectedValue);
        if (drpSection.SelectedIndex > 0)
            hashtable.Add("@Section", drpSection.SelectedValue);
        drpSelectStudent.DataSource = common.GetDataTable("ps_StudSectionAlert", hashtable);
        drpSelectStudent.DataTextField = "NameAdmn";
        drpSelectStudent.DataValueField = "admnno";
        drpSelectStudent.DataBind();
        drpSelectStudent.Items.Insert(0, new ListItem("--ALL--", "0"));
    }

    protected void BtnAdmSave_Click(object sender, EventArgs e)
    {
        string str="";
        DataTable dtrow= new Common().ExecuteSql("select * from PS_StudMaster where AdmnNo ='" + txtAdmNo.Text+"'");
        DataTable dt = (DataTable)ViewState["dtformultselec"];
        foreach (DataRow row in (InternalDataCollectionBase)dtrow.Rows)
        {
            dt.Rows.Add(row["OldAdmnNo"], row["AdmnNo"], row["FullName"]);
            ViewState["dtformultselec"] = dt;
        }
        grddatalist.DataSource = dt;
        grddatalist.DataBind();
        txtAdmNo.Text = "";
    }
}
