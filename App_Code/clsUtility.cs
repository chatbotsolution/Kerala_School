using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mail;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

public class clsUtility
{
    public static string ReplaceSpacialChar(string strString)
    {
        string[] strArray = new string[30]
    {
      "\\",
      "(",
      "/",
      "-",
      ",",
      ">",
      "<",
      "_",
      "&",
      "'",
      "’",
      "  ",
      " ",
      "   ",
      "*",
      "`",
      "!",
      "@",
      "#",
      "$",
      "^",
      "=",
      "+",
      "?",
      ")",
      "{",
      "}",
      "[",
      "]",
      "|"
    };
        foreach (object obj in strArray)
            strString = strString.Replace(obj.ToString(), "");
        return strString.Trim();
    }

    public static string ReplaceSpacialChar(string strString, string strChar)
    {
        string[] strArray = new string[30]
    {
      "\\",
      "(",
      "/",
      "-",
      ",",
      ">",
      "<",
      "_",
      "&",
      "'",
      "’",
      "  ",
      " ",
      "   ",
      "*",
      "`",
      "!",
      "@",
      "#",
      "$",
      "^",
      "=",
      "+",
      "?",
      ")",
      "{",
      "}",
      "[",
      "]",
      "|"
    };
        foreach (object obj in strArray)
            strString = strString.Replace(obj.ToString(), strChar);
        return strString.Trim();
    }

    public static string GetImage(string strImageName, string strPath)
    {
        FileInfo[] files = new DirectoryInfo(strPath).GetFiles("*" + strImageName + ".*");
        if (files.Length <= 0)
            return "noimage.jpg";
        return files[0].Name;
    }

    public static string FindWord(string MainText, string WordsToFind, string Separator)
    {
        StringBuilder stringBuilder = new StringBuilder();
        MainText = clsUtility.ReplaceSpacialChar(MainText).ToUpper();
        string[] strArray = WordsToFind.ToUpper().Split(Separator.ToCharArray());
        for (int index = 0; index < strArray.Length; ++index)
        {
            if (MainText.IndexOf(strArray[index].Trim()) >= 0 && strArray[index].Trim().Length > 0)
                stringBuilder.Append(strArray[index] + ", ");
        }
        if (stringBuilder.Length != 0)
            return stringBuilder.ToString().Remove(stringBuilder.Length - 2, 2);
        return "";
    }

    public static string RootUrl()
    {
        return HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath);
    }

    public static string GetXmlMessage(int intId, params string[] args)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(clsUtility.RootUrl() + "/Message.xml");
        XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/messages/message[@id=" + Convert.ToString(intId) + "]");
        if (xmlNodeList.Count <= 0)
            return "";
        return string.Format(xmlNodeList[0].InnerText, (object[])args);
    }

    public static void DeleteFile(string strFileName, string strPath)
    {
        foreach (FileSystemInfo file in new DirectoryInfo(strPath).GetFiles("*" + strFileName + ".*"))
            file.Delete();
    }

    public static DropDownList FillYearOfBirth(DropDownList ddlYOB)
    {
        for (int year = DateTime.Now.Year; year >= 1900; --year)
            ddlYOB.Items.Add(year.ToString());
        return ddlYOB;
    }

    public static string CreatePopUp(string strIdentity, string strUrl, string strName, int intHeight, int intWidth, string strText)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<script language='javascript'>");
        stringBuilder.Append("function CreatePopUp" + strIdentity + "()");
        stringBuilder.Append("{");
        stringBuilder.Append("newwindow=window.open('" + strUrl + "','" + strName + "','height=" + (object)intHeight + ",width=" + (object)intWidth + "');");
        stringBuilder.Append("var temp=newwindow.document;");
        stringBuilder.Append("temp.write('" + strText + "');");
        stringBuilder.Append("temp.write('<hr size=1 color=red>');");
        stringBuilder.Append("temp.write('<A HREF=javascript:window.close()>Close</A>');");
        stringBuilder.Append("}");
        stringBuilder.Append("</script>");
        return stringBuilder.ToString();
    }

    public static string CreatePopUp(string strUrl, string strName, int intToolBar, int intScrollbar, int intlocation, int intStatusbar, int intMenubar, int intResizable, int intLeft, int intTop, int intHeight, int intWidth)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("<script language='javascript'>");
        stringBuilder.Append("function CreatePopUp()");
        stringBuilder.Append("{");
        stringBuilder.Append("newwindow=window.open('" + strUrl + "','" + strName + "','height=" + (object)intHeight + ",width=" + (object)intWidth);
        stringBuilder.Append("toolbar=" + (object)intToolBar + ", scrollbars=" + (object)intScrollbar + ", location=" + (object)intlocation + ", statusbar=" + (object)intStatusbar + ", menubar=" + (object)intMenubar + ", resizable=" + (object)intResizable + ", left=" + (object)intLeft + ", top=" + (object)intTop + "')");
        stringBuilder.Append("}");
        stringBuilder.Append("</script>");
        return stringBuilder.ToString();
    }

    public static void SetFocus(Control control)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("\r\n<script language='JavaScript'>\r\n");
        stringBuilder.Append("<!--\r\n");
        stringBuilder.Append("function SetFocus()\r\n");
        stringBuilder.Append("{\r\n");
        stringBuilder.Append("\tdocument.");
        Control parent = control.Parent;
        while (!(parent is HtmlForm))
            parent = parent.Parent;
        stringBuilder.Append(parent.ClientID);
        stringBuilder.Append("['");
        stringBuilder.Append(control.UniqueID);
        stringBuilder.Append("'].focus();\r\n");
        stringBuilder.Append("}\r\n");
        stringBuilder.Append("window.onload = SetFocus;\r\n");
        stringBuilder.Append("// -->\r\n");
        stringBuilder.Append("</script>");
        control.Page.RegisterClientScriptBlock("SetFocus", stringBuilder.ToString());
    }

    public static string UpperXml(string strXml)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(strXml);
        clsUtility.ProcessXml(xmlDoc.ChildNodes, ref xmlDoc);
        return xmlDoc.InnerXml;
    }

    private static void ProcessXml(XmlNodeList xnList, ref XmlDocument xmlDoc)
    {
        for (int index1 = 0; index1 < xnList.Count && xnList[index1].NodeType == XmlNodeType.Element; ++index1)
        {
            XmlNode xmlNode = xnList[index1].Clone();
            XmlAttributeCollection attributes = xmlNode.Attributes;
            XmlNode element = (XmlNode)xmlDoc.CreateElement(xmlNode.Name.ToUpper(), "");
            element.InnerXml = xmlNode.InnerXml;
            xnList[index1].ParentNode.ReplaceChild(element, xnList[index1]);
            if (attributes != null)
            {
                for (int index2 = 0; index2 < attributes.Count; ++index2)
                {
                    XmlAttribute attribute = xmlDoc.CreateAttribute(attributes[index2].Name.ToUpper(), "");
                    attribute.Value = attributes[index2].Value;
                    element.Attributes.Append(attribute);
                }
            }
            if (xnList[index1].HasChildNodes)
                clsUtility.ProcessXml(xnList[index1].ChildNodes, ref xmlDoc);
        }
    }

    public static string TruncateColumn(string strColumn)
    {
        if (strColumn.Length <= 20)
            return strColumn;
        return strColumn.Substring(0, 20) + "...";
    }

    public static string TruncateColumn(string strColumn, int intNoOfCharacters)
    {
        if (strColumn.Length <= intNoOfCharacters)
            return strColumn;
        return strColumn.Substring(0, intNoOfCharacters) + "...";
    }

    public static void SendMail(string strMailTo, string strSubject, string strBody)
    {
        try
        {
            MailMessage message = new MailMessage();
            SmtpMail.SmtpServer = ConfigurationSettings.AppSettings["smtpServer"];
            message.From = ConfigurationSettings.AppSettings["FromEmail"];
            message.BodyFormat = MailFormat.Html;
            message.To = strMailTo;
            message.Subject = strSubject;
            message.Body = strBody;
            SmtpMail.Send(message);
        }
        catch
        {
        }
    }

    public static System.Drawing.Image ResizeImage(System.Drawing.Image img, int intMax)
    {
        int width1 = img.Width;
        int height1 = img.Height;
        int width2;
        int height2;
        if (width1 > height1)
        {
            width2 = intMax;
            height2 = height1 * intMax / width1;
        }
        else
        {
            height2 = intMax;
            width2 = width1 * intMax / height1;
        }
        Bitmap bitmap1 = new Bitmap(img);
        Bitmap bitmap2 = new Bitmap(width2, height2);
        Graphics.FromImage((System.Drawing.Image)bitmap2).DrawImage((System.Drawing.Image)bitmap1, 0, 0, width2, height2);
        return (System.Drawing.Image)bitmap2;
    }

    public static bool StringInFiles(string strDirPath, string strFindString)
    {
        try
        {
            foreach (FileSystemInfo file in new DirectoryInfo(strDirPath.ToUpper()).GetFiles())
            {
                StreamReader streamReader = File.OpenText(file.FullName);
                string upper = streamReader.ReadToEnd().ToUpper();
                streamReader.Close();
                if (upper.IndexOf(strFindString.ToUpper()) > -1)
                    return true;
            }
            return false;
        }
        catch
        {
            return true;
        }
        finally
        {
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
