using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Classes.BL
{
    public class clsServerValidator
    {
        public string gstrFlag = "";
        protected string mstrErrMsgHtml;
        protected string mstrMessageXMLPath;

        public string ErrMessage
        {
            get
            {
                return this.mstrErrMsgHtml;
            }
            set
            {
                this.mstrErrMsgHtml = value;
            }
        }

        public string MessageXMLPath
        {
            set
            {
                this.mstrMessageXMLPath = value;
            }
        }

        public clsServerValidator()
        {
        }

        public clsServerValidator(string strMassagePath)
        {
            this.mstrMessageXMLPath = strMassagePath;
        }

        public bool IsValidUrl(string pstrURL)
        {
            string pattern = "http://([\\w-]+\\.)+[\\w-]+(/[\\w- ./?%&=]*)?";
            MatchCollection matchCollection = Regex.Matches(pstrURL, pattern);
            if (matchCollection.Count == 0 || matchCollection.Count > 1)
                return false;
            foreach (Match match in matchCollection)
                pstrURL = pstrURL.Replace(match.Value, "");
            return pstrURL.Length <= 0;
        }

        public bool IsValidEmail(string pstrEmail)
        {
            string pattern = "\\w+([-+.]\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            MatchCollection matchCollection = Regex.Matches(pstrEmail, pattern);
            if (matchCollection.Count == 0 || matchCollection.Count > 1)
                return false;
            foreach (Match match in matchCollection)
                pstrEmail = pstrEmail.Replace(match.Value, "");
            return pstrEmail.Length <= 0;
        }

        public bool IsStrContainSpChar(string pstrToCheck, string[] pspCharList)
        {
            if (pspCharList.Length > 0)
            {
                for (int index = 0; index < pspCharList.Length; ++index)
                {
                    if (pstrToCheck.IndexOf(pspCharList[index]) >= 0)
                        return true;
                }
            }
            return false;
        }

        public bool IsValidUserName(string pstrToCheck)
        {
            pstrToCheck = pstrToCheck.Replace("-", "").Replace("_", "");
            return this.IsAlphaNumeric(pstrToCheck.Replace(" ", ""));
        }

        public bool IsNumeric(string pstrToCheck)
        {
            for (int index = 0; index < pstrToCheck.Length; ++index)
            {
                if ((int)Encoding.ASCII.GetBytes(pstrToCheck)[index] > 57 || (int)Encoding.ASCII.GetBytes(pstrToCheck)[index] < 48)
                    return false;
            }
            return true;
        }

        public bool IsAlphaNumeric(string pstrToCheck)
        {
            bool flag = false;
            for (int index = 0; index < pstrToCheck.Length; ++index)
            {
                int num = (int)Encoding.ASCII.GetBytes(pstrToCheck)[index];
                if ((num <= 47 || num >= 58) && (num <= 64 || num >= 91) && (num <= 96 || num >= 123))
                    return false;
                flag = true;
            }
            return flag;
        }

        public bool IsAlphabet(string pstrToCheck)
        {
            bool flag = false;
            for (int index = 0; index < pstrToCheck.Length; ++index)
            {
                int num = (int)Encoding.ASCII.GetBytes(pstrToCheck)[index];
                if ((num <= 64 || num >= 91) && (num <= 96 || num >= 123))
                    return false;
                flag = true;
            }
            return flag;
        }

        public bool IsValidDate(string pstrToCheck)
        {
            string pattern = "^(?:(?:(?:0?[13578]|1[02])(\\/|-|\\.)31)\\1|(?:(?:0?[1,3-9]|1[0-2])(\\/|-|\\.)(?:29|30)\\2))(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$|^(?:0?2(\\/|-|\\.)29\\3(?:(?:(?:1[6-9]|[2-9]\\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:(?:0?[1-9])|(?:1[0-2]))(\\/|-|\\.)(?:0?[1-9]|1\\d|2[0-8])\\4(?:(?:1[6-9]|[2-9]\\d)?\\d{2})$";
            MatchCollection matchCollection = Regex.Matches(pstrToCheck, pattern);
            if (matchCollection.Count == 0 || matchCollection.Count > 1)
                return false;
            foreach (Match match in matchCollection)
                pstrToCheck = pstrToCheck.Replace(match.Value, "");
            return pstrToCheck.Length <= 0;
        }

        public bool IsWord(string pstrToCheck)
        {
            string[] pspCharList = new string[1] { " " };
            if (!this.IsStrContainSpChar(pstrToCheck, pspCharList))
                return this.IsAlphabet(pstrToCheck);
            return false;
        }

        public bool IsAlphNumric_SpChar(string pstrToCheck)
        {
            pstrToCheck = pstrToCheck.Replace(",", "").Replace("-", "").Replace("_", "").Replace(".", "");
            return this.IsAlphaNumeric(pstrToCheck.Replace(" ", ""));
        }

        public bool IsFirstCharAlphabet(string pstrToCheck)
        {
            return this.IsAlphabet(pstrToCheck.Substring(0, 1));
        }

        public bool IsFloat(string pstrToCheck)
        {
            bool flag = true;
            string pattern = "(\\d\\.\\d)";
            MatchCollection matchCollection = Regex.Matches(pstrToCheck, pattern);
            if (matchCollection.Count == 0)
                flag = false;
            else if (matchCollection.Count > 1)
                flag = false;
            if (!flag)
                return this.IsNumeric(pstrToCheck);
            return true;
        }

        public bool IsValidIP(string pstrToCheck)
        {
            string pattern = "\\b(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\b";
            MatchCollection matchCollection = Regex.Matches(pstrToCheck, pattern);
            if (matchCollection.Count == 0 || matchCollection.Count > 1)
                return false;
            foreach (Match match in matchCollection)
                pstrToCheck = pstrToCheck.Replace(match.Value, "");
            return pstrToCheck.Length <= 0;
        }

        public bool IsValidPath(string pstrFilePath, string pstrFileExtTypes)
        {
            string pstrToCheck = pstrFilePath.Substring(pstrFilePath.LastIndexOf("\\") + 1);
            string str1 = pstrFilePath.Substring(0, pstrFilePath.LastIndexOf("\\") + 1);
            string str2 = pstrFilePath.Substring(pstrFilePath.LastIndexOf("."));
            string str3 = pstrFileExtTypes;
            char[] chArray = new char[1] { ',' };
            foreach (string oldValue in str3.Split(chArray))
                pstrToCheck = pstrToCheck.Replace(oldValue, "");
            string[] pspCharList = new string[9]
      {
        "\\",
        "/",
        ":",
        "*",
        "?",
        "\"",
        "<",
        ">",
        "|"
      };
            if (this.IsStrContainSpChar(pstrToCheck, pspCharList))
                return false;
            string str4 = pstrToCheck.Replace("`", "").Replace("~", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace("+", "").Replace("=", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace(";", "").Replace("'", "").Replace(",", "").Replace(" ", "").Replace(".", "");
            pstrFilePath = str1.Replace("`", "").Replace("~", "").Replace("!", "").Replace("@", "").Replace("#", "").Replace("$", "").Replace("%", "").Replace("^", "").Replace("&", "").Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace("+", "").Replace("=", "").Replace("{", "").Replace("}", "").Replace("[", "").Replace("]", "").Replace(";", "").Replace("'", "").Replace(",", "").Replace(" ", "").Replace(".", "") + str4 + str2;
            bool flag = false;
            if (pstrFilePath == null)
                return false;
            string str5 = "";
            if (pstrFileExtTypes.Trim().Length > 0)
            {
                string[] strArray = pstrFileExtTypes.Split(',');
                for (int index = 0; index < strArray.Length; ++index)
                {
                    str5 = str5 + strArray[index] + "|" + strArray[index].ToUpper();
                    if (index + 1 != strArray.Length)
                        str5 += "|";
                }
            }
            flag = false;
            return new Regex("^(([a-zA-Z]:)|(\\\\{2}\\w+)\\\\\\w+\\$?)(\\\\(\\w[-\\w ]*))+\\.(" + str5 + ")$").Match(pstrFilePath).Success;
        }

        public bool IsValidPath(string pstrFilePath)
        {
            bool flag = false;
            if (pstrFilePath == null)
                return false;
            flag = false;
            return new Regex("^(([a-zA-Z]:)|(\\\\{2}\\w+)\\$?)(\\\\(\\w[\\w ]*))+\\.([a-zA-Z]){3}$").Match(pstrFilePath).Success;
        }

        public bool IsBlank(string pstrToCheck)
        {
            return pstrToCheck.Length == 0;
        }

        public bool IsProperMaxLength(string pstrToCheck, int iMaxLen)
        {
            return pstrToCheck.Length <= iMaxLen;
        }

        public bool IsProperMinLength(string pstrToCheck, int iMinLen)
        {
            return pstrToCheck.Length >= iMinLen;
        }

        public bool IsFieldValid(int piValType, string pstrToCheck, string pstrIsReq, string pstrMaxLen, string pstrMinLen, string pstrChkDup, string pstrDupFieldValue)
        {
            if (pstrIsReq == "1")
            {
                if (this.IsBlank(pstrToCheck))
                {
                    this.gstrFlag = "Req";
                    return false;
                }
            }
            else if (this.IsBlank(pstrToCheck))
                return true;
            if (pstrMinLen != "" && pstrMinLen != "0" && !this.IsProperMinLength(pstrToCheck, (int)Convert.ToInt16(pstrMinLen)))
            {
                this.gstrFlag = "MinLen";
                return false;
            }
            if (pstrMaxLen != "" && pstrMaxLen != "0" && !this.IsProperMaxLength(pstrToCheck, (int)Convert.ToInt16(pstrMaxLen)))
            {
                this.gstrFlag = "MaxLen";
                return false;
            }
            bool flag;
            switch (piValType)
            {
                case 1:
                    flag = this.IsNumeric(pstrToCheck);
                    break;
                case 2:
                    flag = this.IsWord(pstrToCheck);
                    break;
                case 3:
                    flag = this.IsAlphaNumeric(pstrToCheck.Trim().Replace(" ", ""));
                    break;
                case 4:
                    flag = this.IsAlphNumric_SpChar(pstrToCheck);
                    break;
                case 5:
                    flag = this.IsAlphabet(pstrToCheck.Trim().Substring(0, 1));
                    break;
                case 6:
                    flag = this.IsValidIP(pstrToCheck);
                    break;
                case 7:
                    flag = this.IsValidEmail(pstrToCheck);
                    break;
                case 8:
                    flag = this.IsFloat(pstrToCheck);
                    break;
                case 9:
                    flag = this.IsValidUrl(pstrToCheck);
                    break;
                case 10:
                    flag = this.IsValidPath(pstrToCheck);
                    break;
                case 12:
                    flag = this.IsValidPath(pstrToCheck, "jpg,jpeg");
                    break;
                case 13:
                    flag = this.IsValidPath(pstrToCheck, "jpg,jpeg,bmp,gif,png");
                    break;
                case 14:
                    flag = this.IsValidPath(pstrToCheck, "csv,xls");
                    break;
                case 15:
                    flag = this.IsValidPath(pstrToCheck, "pdf");
                    break;
                case 16:
                    flag = this.IsValidPath(pstrToCheck, "doc,rtf");
                    break;
                case 17:
                    flag = this.IsValidPath(pstrToCheck, "txt");
                    break;
                case 18:
                    flag = this.IsValidUserName(pstrToCheck);
                    break;
                case 19:
                    flag = this.IsValidDate(pstrToCheck);
                    break;
                case 20:
                    flag = this.IsValidPath(pstrToCheck, "jpg,jpeg,gif");
                    break;
                default:
                    flag = true;
                    break;
            }
            if (!(pstrChkDup != "") || !flag)
                return flag;
            if (pstrToCheck == pstrDupFieldValue)
                return true;
            this.gstrFlag = "Dup";
            return false;
        }

        public bool IsFieldValid(string pstrToCheck, string pstrIsReq)
        {
            if (pstrIsReq == "0" || pstrIsReq == "")
                return true;
            if (!(pstrIsReq == "1"))
                return false;
            if (pstrToCheck != "-1" && pstrToCheck != "" && pstrToCheck != null)
                return true;
            this.gstrFlag = "Req";
            return false;
        }

        public string DisplayMessage(string pstrMsgId, string pstrCaption)
        {
            this.gstrFlag = "";
            XmlDocument xmlDocument = new XmlDocument();
            string[] strArray = pstrCaption.Split(",".ToCharArray());
            xmlDocument.Load(this.mstrMessageXMLPath);
            XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("message");
            string str1 = "";
            for (int index1 = 0; index1 < elementsByTagName.Count; ++index1)
            {
                if (elementsByTagName[index1].Attributes.GetNamedItem("id").Value == pstrMsgId.Trim())
                {
                    string str2 = elementsByTagName[index1].InnerText;
                    for (int index2 = 0; index2 < strArray.Length; ++index2)
                        str2 = str2.Replace("{" + (object)index2 + "}", strArray[index2]);
                    str1 = "<div style='COLOR: " + elementsByTagName[index1].Attributes.GetNamedItem("color").Value + "; FONT-FAMILY: " + elementsByTagName[index1].Attributes.GetNamedItem("font").Value + ";FONT-SIZE: " + elementsByTagName[index1].Attributes.GetNamedItem("fontSize").Value + "px'>" + str2 + "</div>";
                    break;
                }
            }
            return str1;
        }

        public string DisplayMessage(string pstrMsgId)
        {
            this.gstrFlag = "";
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(this.mstrMessageXMLPath);
            XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("message");
            string str = "";
            for (int index = 0; index < elementsByTagName.Count; ++index)
            {
                if (elementsByTagName[index].Attributes.GetNamedItem("id").Value == pstrMsgId.Trim())
                {
                    string innerText = elementsByTagName[index].InnerText;
                    str = "<div style='COLOR: " + elementsByTagName[index].Attributes.GetNamedItem("color").Value + "; FONT-FAMILY: " + elementsByTagName[index].Attributes.GetNamedItem("font").Value + ";FONT-SIZE: " + elementsByTagName[index].Attributes.GetNamedItem("fontSize").Value + "px'>" + innerText + "</div>";
                    break;
                }
            }
            return str;
        }

        protected virtual string ValidateObject(string pstrControltype, int piValType, string pstrToCheck, string pstrIsReq, string pstrMaxLen, string pstrMinLen, string pstrMsgId, string pstrCaption, string pstrDupField, string pstrIsUnique, string pstrDupFieldValue)
        {
            bool flag = true;
            switch (pstrControltype)
            {
                case "1":
                    flag = this.IsFieldValid(piValType, pstrToCheck, pstrIsReq, pstrMaxLen, pstrMinLen, pstrDupField, pstrDupFieldValue);
                    break;
                case "2":
                    flag = this.IsFieldValid(piValType, pstrToCheck.Replace("\r", "").Replace("\n", "").Replace("\t", ""), pstrIsReq, pstrMaxLen, pstrMinLen, pstrDupField, pstrDupFieldValue);
                    break;
                case "3":
                    flag = this.IsFieldValid(pstrToCheck, pstrIsReq);
                    break;
                case "6":
                    flag = this.IsFieldValid(pstrToCheck, pstrIsReq);
                    break;
                case "10":
                    flag = this.IsFieldValid(piValType, pstrToCheck, pstrIsReq, pstrMaxLen, pstrMinLen, pstrDupField, pstrDupFieldValue);
                    break;
            }
            if (flag)
                return "";
            switch (this.gstrFlag)
            {
                case "Req":
                    return this.DisplayMessage("1", pstrCaption);
                case "MaxLen":
                    return this.DisplayMessage("2", pstrCaption + "," + pstrMaxLen);
                case "MinLen":
                    return this.DisplayMessage("3", pstrCaption + "," + pstrMinLen);
                case "Dup":
                    return this.DisplayMessage("4", pstrCaption);
                default:
                    return this.DisplayMessage(pstrMsgId, pstrCaption);
            }
        }

        public virtual bool IsValidForm(XmlDocument pxdFormDetail, int pstrFrmType)
        {
            return true;
        }
    }
}