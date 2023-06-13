using System.Text;

namespace Classes.BL
{
    public class clsJavaScriptValidator
    {
        private string strReturnString;

        public string AllValidations
        {
            get
            {
                return this.strReturnString + " } ";
            }
        }

        public clsJavaScriptValidator()
        {
            this.strReturnString = "function ValidateAll() { ";
        }

        public clsJavaScriptValidator(string strFunctionName)
        {
            this.strReturnString = "function " + strFunctionName + "() { ";
        }

        public string GetValidationWithAjax(string strFunctionName)
        {
            return this.strReturnString + "      " + strFunctionName + "();  return false;}";
        }

        public string GetValidationWithJavaScriptCall(string strFunctionName)
        {
            return this.strReturnString + "      " + strFunctionName + "(); }";
        }

        public string trimString(string strId)
        {
            return "" + "function trimString" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + " var sString=el.value;" + " \twhile (sString.substring(0,1) == ' ')" + " \tsString = sString.substring(1, sString.length);" + " while (sString.substring(sString.length-1, sString.length) == ' ')" + " \tsString = sString.substring(0,sString.length-1);" + " \tel.value=sString" + " }";
        }

        public string trimEL(string strId)
        {
            return "" + "function trimEL" + strId + "()" + "{" + "var el=document.getElementById('" + strId + "');" + " var str = el.replace(/[\\n\\r\\s]+/,\"\");" + " if(str!==\"\") while(str.charAt(0)==' '){str = str.substring(1,str.length)}" + " if(str!==\"\") while(str.charAt(str.length-1)==' '){str = str.substring(0,(str.length)-1)}" + " return str;" + "}";
        }

        public string isInteger(string strId, string strMsg)
        {
            string str1 = "function isInteger" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + "var elval= el.value;" + "if(elval==\"\") return true;" + "var strValidChars =\"0123456789\";" + "var blnResult = true;" + "for (i = 0; i < elval.length && blnResult == true; i++)" + " {" + "\tstrChar = elval.charAt(i);" + "if (strValidChars.indexOf(strChar) == -1)" + "{" + "alert('" + strMsg + "');" + "el.focus();el.select();" + "blnResult = false;" + "}" + "}" + "return blnResult;" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if(!isInteger" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string IsInteger(string strId, string strMsg, string strBlankMsg)
        {
            string str1 = "function IsInteger" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + "var elval= el.value.replace(/^\\s+/g, '').replace(/\\s+$/g, '');" + "if(elval==\"\") { alert('" + strBlankMsg + "');el.focus();el.select(); return false;}" + "var strValidChars =\"0123456789\";" + "var blnResult = true;" + "for (i = 0; i < elval.length && blnResult == true; i++)" + " {" + "\tstrChar = elval.charAt(i);" + "if (strValidChars.indexOf(strChar) == -1)" + "{" + "alert('" + strMsg + "');" + "el.focus();el.select();" + "blnResult = false;" + "}" + "}" + "return blnResult;" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if(!IsInteger" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string IsValidFile(string strId, string MesForPath, string MesForExt, params string[] strExt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("function IsValidFile" + strId + "(){");
            stringBuilder.Append("var objUpload=eval(document.getElementById('" + strId + "'));");
            stringBuilder.Append("var sUpload=objUpload.value;if(sUpload!=''){var iExt=sUpload.indexOf('\\\\');var iDot=sUpload.indexOf('.');if((iExt < 0 ) || (iDot < 0) || sUpload.substr(1,2)!=':\\\\' ){alert('");
            stringBuilder.Append(MesForPath + "');objUpload.focus();event.returnValue=false;return; }if(iDot > 0){var aUpload=sUpload.split('.');");
            stringBuilder.Append("if(");
            for (int index = 0; index < strExt.Length; ++index)
                stringBuilder.Append("aUpload[aUpload.length-1].toUpperCase()!='" + strExt[index].ToUpper() + "' &&");
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.Append("){alert('" + MesForExt + "');objUpload.focus();event.returnValue=false;return;}}}return true;}");
            clsJavaScriptValidator javaScriptValidator = this;
            string str = javaScriptValidator.strReturnString + "if(!IsValidFile" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str;
            return stringBuilder.ToString();
        }

        public string IsValidFileNotBlank(string strId, string MesForPath, string MesForExt, string strBlankMsg, params string[] strExt)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("function IsValidFileNoSpace" + strId + "(){");
            stringBuilder.Append("var objUpload=eval(document.getElementById('" + strId + "'));");
            stringBuilder.Append("var sUpload=objUpload.value.replace(/^\\s+/g, '').replace(/\\s+$/g, '');");
            stringBuilder.Append("if(sUpload==\"\") { alert('" + strBlankMsg + "');objUpload.focus();objUpload.select(); return false;}");
            stringBuilder.Append("if(sUpload!=''){var iExt=sUpload.indexOf('\\\\');var iDot=sUpload.indexOf('.');if((iExt < 0 ) || (iDot < 0) || sUpload.substr(1,2)!=':\\\\' ){alert('");
            stringBuilder.Append(MesForPath + "');objUpload.focus();event.returnValue=false;return; }if(iDot > 0){var aUpload=sUpload.split('.');");
            stringBuilder.Append("if(");
            for (int index = 0; index < strExt.Length; ++index)
                stringBuilder.Append("aUpload[aUpload.length-1].toUpperCase()!='" + strExt[index].ToUpper() + "' &&");
            stringBuilder.Remove(stringBuilder.Length - 2, 2);
            stringBuilder.Append("){alert('" + MesForExt + "');objUpload.focus();event.returnValue=false;return;}}}return true;}");
            clsJavaScriptValidator javaScriptValidator = this;
            string str = javaScriptValidator.strReturnString + "if(!IsValidFileNoSpace" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str;
            return stringBuilder.ToString();
        }

        public string IsValidImageSize(string strId, int intMax)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("function IsValidImageSize" + strId + "(){var imgU= new Image();");
            stringBuilder.Append("var objUpload=eval(document.getElementById('" + strId + "'));");
            stringBuilder.Append("var sUpload=objUpload.value.replace(/^\\s+/g, '').replace(/\\s+$/g, '');if(sUpload=='') return true; else{imgU.src=sUpload;if( imgU.width > " + (object)intMax + " || imgU.height > " + (object)intMax + " ){if(confirm('");
            stringBuilder.Append("The size of image is greater than " + (object)intMax + ", Do you want to save the image? If yes then click on OK and the image will be saved within the specified size else you can click on cancel and upload a new image.')==true){return true;} else{objUpload.focus(); return false;}}else{return true;}}}");
            clsJavaScriptValidator javaScriptValidator = this;
            string str = javaScriptValidator.strReturnString + "if(!IsValidImageSize" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str;
            return stringBuilder.ToString();
        }

        public string IsFloat(string strId, string strMsg)
        {
            string str1 = "function IsFloat" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + "var elval= el.value.replace(/^\\s+/g, '').replace(/\\s+$/g, '');" + "if(elval ==\"\") return true;" + "var strValidChars = \"0123456789.\";" + "if (elval.charAt(elval.length-1)=='.'){ alert('" + strMsg + "');el.focus();el.select(); return false;}" + "if (elval.replace('.','').indexOf('.') >= 0){ alert('" + strMsg + "');el.focus();el.select(); return false;}" + "var blnResult = true;" + "for (i = 0; i < elval.length && blnResult == true; i++)" + " {" + "\tstrChar = elval.charAt(i);" + "if (strValidChars.indexOf(strChar) == -1)" + "{" + "alert('" + strMsg + "');" + "el.focus();el.select();" + "blnResult = false;" + "}" + "}" + "return blnResult;" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if(!IsFloat" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string IsFloat(string strId, string strMsg, string strBlankMsg)
        {
            string str1 = "function IsFloat" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + "var elval= el.value.replace(/^\\s+/g, '').replace(/\\s+$/g, '');" + "if(elval==\"\") { alert('" + strBlankMsg + "');el.focus();el.select(); return false;}" + "var strValidChars = \"0123456789.\";" + "if (elval.charAt(elval.length-1)=='.'){ alert('" + strMsg + "');el.focus();el.select(); return false;}" + "if (elval.replace('.','').indexOf('.') >= 0){ alert('" + strMsg + "');el.focus();el.select(); return false;}" + "var blnResult = true;" + "for (i = 0; i < elval.length && blnResult == true; i++)" + " {" + "\tstrChar = elval.charAt(i);" + "if (strValidChars.indexOf(strChar) == -1)" + "{" + "alert('" + strMsg + "');" + "el.focus();el.select();" + "blnResult = false;" + "}" + "}" + "return blnResult;" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if(!IsFloat" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string IsChar(string strId, int intMode, string strMsg)
        {
            string str1 = "function IsChar" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + "var valid=1;" + "var mode=" + (object)intMode + ";" + "elValue = el.value.replace(/^\\s+/g, '').replace(/\\s+$/g, '');" + "\tif(mode==0){for(i=0;i<elValue.length;i++){cc=elValue.charCodeAt(i);if((cc<65)||(cc>122)) valid=0;}}" + "\tif(mode==1){for(i=0;i<elValue.length;i++){cc=elValue.charCodeAt(i);if(((cc<65)||(cc>122))&&(cc!=32)&&((cc<48)||(cc>57))) valid=0;}}" + "\tif(mode==2){for(i=0;i<elValue.length;i++){cc=elValue.charCodeAt(i);if(((cc<65)||(cc>122))&&(cc!=32)&&(cc!=44)&&(cc!=95)&&(cc!=45)&&(cc!=46)&&((cc<48)||(cc>57))) valid=0;}}" + "\tif(mode==3){for(i=0;i<elValue.length;i++){cc=elValue.charCodeAt(i);if(((cc<65)||(cc>122))&&(cc!=46)&&(cc!=45)&&(cc!=95)&&((cc<48)||(cc>57))) valid=0;}}" + "\tif(mode==4)if((elValue.charCodeAt(0)<65)||(elValue.charCodeAt(0)>122)||(elValue.charCodeAt(0)==91)||(elValue.charCodeAt(0)==92)||(elValue.charCodeAt(0)==93)||(elValue.charCodeAt(0)==94)||(elValue.charCodeAt(0)==95)||(elValue.charCodeAt(0)==96)) valid=0;" + "\tif(mode==5){for(i=0;i<elValue.length;i++){cc=elValue.charCodeAt(i);if(((cc<65)||(cc>122))&&((cc<48)||(cc>57))) valid=0;}}" + "\tif(!valid) {alert('" + strMsg + "');el.focus();el.select();return false;}" + "\t\telse return true;" + "\t}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!IsChar" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string IsChar(string strId, int intMode, string strMsg, string strBlankMsg)
        {
            string str1 = "function IsChar" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + "var valid=1;" + "var mode=" + (object)intMode + ";" + "elValue = el.value.replace(/^\\s+/g, '').replace(/\\s+$/g, '');" + "if(elValue==\"\") { alert('" + strBlankMsg + "');el.focus();el.select(); return false;}" + "\tif(mode==0){for(i=0;i<elValue.length;i++){cc=elValue.charCodeAt(i);if((cc<65)||(cc>122)) valid=0;}}" + "\tif(mode==1){for(i=0;i<elValue.length;i++){cc=elValue.charCodeAt(i);if(((cc<65)||(cc>122))&&(cc!=32)&&((cc<48)||(cc>57))) valid=0;}}" + "\tif(mode==2){for(i=0;i<elValue.length;i++){cc=elValue.charCodeAt(i);if(((cc<65)||(cc>122))&&(cc!=32)&&(cc!=44)&&(cc!=95)&&(cc!=45)&&(cc!=46)&&((cc<48)||(cc>57))) valid=0;}}" + "\tif(mode==3){for(i=0;i<elValue.length;i++){cc=elValue.charCodeAt(i);if(((cc<65)||(cc>122))&&(cc!=46)&&(cc!=45)&&(cc!=95)&&((cc<48)||(cc>57))) valid=0;}}" + "\tif(mode==4)if((elValue.charCodeAt(0)<65)||(elValue.charCodeAt(0)>122)||(elValue.charCodeAt(0)==91)||(elValue.charCodeAt(0)==92)||(elValue.charCodeAt(0)==93)||(elValue.charCodeAt(0)==94)||(elValue.charCodeAt(0)==95)||(elValue.charCodeAt(0)==96)) valid=0;" + "\tif(mode==5){for(i=0;i<elValue.length;i++){cc=elValue.charCodeAt(i);if(((cc<65)||(cc>122))&&((cc<48)||(cc>57))) valid=0;}}" + "\tif(!valid) {alert('" + strMsg + "');el.focus();el.select();return false;}" + "\t\telse return true;" + "\t}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!IsChar" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string txtLimit(string strId, int intLimit, string strMsg)
        {
            return "" + "function txtLimit" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + "\tlimit = parseInt(" + (object)intLimit + ");" + "\tif(el.value.length>limit) {" + " alert('" + strMsg + "');" + "\tel.focus();" + "\treturn false;" + "\t} else return true;" + "}";
        }

        public string txtMinLimit(string strId, int intLimit, string strMsg)
        {
            string str1 = "" + "function txtMinLimit" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + "\tlimit = parseInt(" + (object)intLimit + ");" + " if(el.value.length<limit) {" + " alert('" + strMsg + "');" + " el.focus();" + "\treturn false;" + "} else return true;" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!txtMinLimit" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string strCheckExtention(string strId, string strExtentionType, string strMsg)
        {
            string str1 = "function strCheckExtention" + strId + "(){" + "var testresult = true;" + "var el=document.getElementById('" + strId + "');" + "var ext = trimEL(el.value);" + "ext = ext.substring(ext.lastIndexOf(\".\")+1,ext.length);" + "ext = ext.toLowerCase();" + "switch('" + strExtentionType + "'){" + "case 'IMG' :" + "if((ext!='gif')&&(ext!='jpg')&&(ext!='png')&&(ext!='bmp')&&(ext!='jpeg')){" + " alert('" + strMsg + "');" + "\tel.focus();" + "\tel.select();" + "\ttestresult = false;" + "} break;" + "case 'DOC' :" + "if((ext!='rtf')&&(ext!='txt')&&(ext!='doc')) {" + " alert('" + strMsg + "');" + "\tel.focus();" + "\tel.select();" + "\ttestresult = false;" + "} break;" + " case 'JPG' :" + "\tif((ext!='jpg')&&(ext!='jpeg')) {" + " alert('" + strMsg + "');" + "\tel.focus();" + "\tel.select();" + "\ttestresult = false;" + "} break;" + " case 'JPG/GIF' :" + "\tif((ext!='jpg')&&(ext!='jpeg')&&(ext!='gif')) {" + " alert('" + strMsg + "');" + "\tel.focus();" + "\tel.select();" + "\ttestresult = false;" + "} break;" + "case 'XLS' :" + "if((ext!='xls')&&(ext!='csv')) {" + " alert('" + strMsg + "');" + "\tel.focus();" + "\tel.select();" + "\ttestresult = false;" + "} break;" + " default :" + "if(ext!=tp.toLowerCase()) {" + " alert('" + strMsg + "');" + "\tel.focus(); " + "\tel.select(); " + "\ttestresult = false; " + " } break;" + "}" + "return testresult;" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!strCheckExtention" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string ValidateEmail(string strId, string strMsg)
        {
            string str1 = "" + "function ValidateEmail" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + " var elval= el.value.replace(/[\\n\\r\\s]+/,\"\");" + "if(elval ==\"\")return true;" + "var str=el.value;\n" + " var filter=/^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*\\.(\\w{2}|(com|net|org|edu|int|mil|gov|arpa|biz|aero|name|coop|info|pro|museum))$/i\n" + " if (filter.test(str))" + " testresults=true;" + "\telse {alert('" + strMsg + "'); el.focus();el.select();testresults=false;}" + "\treturn (testresults);" + "\t}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!ValidateEmail" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string ValidateEmail(string strId, string strMsg, string strBlankMsg)
        {
            string str1 = "" + "function ValidateEmail" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + " var elval= el.value.replace(/[\\n\\r\\s]+/,\"\");" + "if(elval==\"\") { alert('" + strBlankMsg + "');el.focus();el.select(); return false;}" + "var str=el.value;\n" + " var filter=/^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*\\.(\\w{2}|(com|net|org|edu|int|mil|gov|arpa|biz|aero|name|coop|info|pro|museum))$/i\n" + " if (filter.test(str))" + " testresults=true;" + "\telse {alert('" + strMsg + "');el.focus();el.select();testresults=false;}" + "\treturn (testresults);" + "\t}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!ValidateEmail" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string ValidateMultipleEmails(string strId, string strMsg, string strBlankMsg, string separator)
        {
            string str1 = "" + "function ValidateMultipleEmails" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + " var elval= el.value.replace(/[\\n\\r\\s]+/,\"\");" + "if(elval==\"\") { alert('" + strBlankMsg + "');el.focus();el.select(); return false;}" + "var arrayMail=elval.split('" + separator + "');" + "for(var i=0;i<arrayMail.length;i++)\n{" + "var str=arrayMail[i];\n" + " var filter=/^\\w+([\\.-]?\\w+)*@\\w+([\\.-]?\\w+)*\\.(\\w{2}|(com|net|org|edu|int|mil|gov|arpa|biz|aero|name|coop|info|pro|museum))$/i\n" + " if (filter.test(str))" + " testresults= true;" + "\telse {alert('" + strMsg + "');el.focus();el.select();return false;}}" + "\treturn (testresults);" + "\t}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!ValidateMultipleEmails" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string ValidateURL(string strId, string strMsg)
        {
            string str1 = "" + "function ValidateURL" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + " var elval= el.value.replace(/[\\n\\r\\s]+/,\"\");" + "if(elval ==\"\") return true;" + "var str=el.value;\n" + "var filter=/^(http:\\/\\/www.|https:\\/\\/www.|ftp:\\/\\/www.|www.){1}([0-9A-Za-z]+\\.)/;" + "if (filter.test(str))" + "testresults=true;" + "else {alert('" + strMsg + "'); el.focus();el.select();testresults=false;}" + "return (testresults);" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!ValidateURL" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string ValidateURL(string strId, string strMsg, string strBlankMsg)
        {
            string str1 = "" + "function ValidateURL" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + " var elval= el.value.replace(/[\\n\\r\\s]+/,\"\");" + "if(elval==\"\") { alert('" + strBlankMsg + "');el.focus();el.select(); return false;}" + "var str=el.value;\n" + "var filter=/(ftp|http|https):\\/\\/(\\w+:{0,1}\\w*@)?(\\S+)(:[0-9]+)?(\\/|\\/([\\w#!:.?+=&%@!\\-\\/]))?/;" + "if (filter.test(str))" + "testresults=true;" + "else {alert('" + strMsg + "'); el.focus();el.select();testresults=false;}" + "return (testresults);" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!ValidateURL" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string ValidateIP(string strId)
        {
            string str1 = "" + "function ValidateIP" + strId + "() {" + "var el=document.getElementById('" + strId + "');" + "var elval= el.value.replace(/[\\n\\r\\s]+/,\"\");" + "if(elval ==\"\") return true;" + "var IPvalue=el.value;" + "errorString = \"\";" + "theName = \"IPaddress\";" + "var ipPattern = /^(\\d{1,3})\\.(\\d{1,3})\\.(\\d{1,3})\\.(\\d{1,3})$/;" + "var ipArray = IPvalue.match(ipPattern);" + "if (IPvalue == \"0.0.0.0\")" + "errorString = errorString + theName + ': '+IPvalue+' is a special IP address and cannot be used here.';" + "else if (IPvalue == \"255.255.255.255\")" + "errorString = errorString + theName + ': '+IPvalue+' is a special IP address and cannot be used here.';" + "if (ipArray == null)" + "errorString = errorString + theName + ': '+IPvalue+' is not a valid IP address.';" + "else {" + "for (i = 0; i < 4; i++) {" + "thisSegment = ipArray[i];" + "if (thisSegment > 255) {" + "errorString = errorString + theName + ': '+IPvalue+' is not a valid IP address.';" + "i = 4;" + "}" + "if ((i == 0) && (thisSegment > 255)) {" + "errorString = errorString + theName + ': '+IPvalue+' is a special IP address and cannot be used here.';" + "i = 4;" + "}" + "}" + "}" + "extensionLength = 3;" + "if (errorString != \"\"){" + "alert(errorString);" + "el.focus();el.select();" + "return false;" + "}" + "return true;" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!ValidateIP" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string ValidateIP(string strId, string strBlankMsg)
        {
            string str1 = "" + "function ValidateIP" + strId + "() {" + "var el=document.getElementById('" + strId + "');" + "var elval= el.value.replace(/[\\n\\r\\s]+/,\"\");" + "if(elval==\"\") { alert('" + strBlankMsg + "');el.focus();el.select(); return false;}" + "var IPvalue=el.value;" + "errorString = \"\";" + "theName = \"IPaddress\";" + "var ipPattern = /^(\\d{1,3})\\.(\\d{1,3})\\.(\\d{1,3})\\.(\\d{1,3})$/;" + "var ipArray = IPvalue.match(ipPattern);" + "if (IPvalue == \"0.0.0.0\")" + "errorString = errorString + theName + ': '+IPvalue+' is a special IP address and cannot be used here.';" + "else if (IPvalue == \"255.255.255.255\")" + "errorString = errorString + theName + ': '+IPvalue+' is a special IP address and cannot be used here.';" + "if (ipArray == null)" + "errorString = errorString + theName + ': '+IPvalue+' is not a valid IP address.';" + "else {" + "for (i = 0; i < 4; i++) {" + "thisSegment = ipArray[i];" + "if (thisSegment > 255) {" + "errorString = errorString + theName + ': '+IPvalue+' is not a valid IP address.';" + "i = 4;" + "}" + "if ((i == 0) && (thisSegment > 255)) {" + "errorString = errorString + theName + ': '+IPvalue+' is a special IP address and cannot be used here.';" + "i = 4;" + "}" + "}" + "}" + "extensionLength = 3;" + "if (errorString != \"\"){" + "alert(errorString);" + "el.focus();el.select();" + "return false;" + "}" + "return true;" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!ValidateIP" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string ValidatePATH(string strId, string strMsg)
        {
            string str1 = "" + "function ValidatePATH" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + "var elval= el.value.replace(/[\\n\\r\\s]+/,\"\");" + "if(elval ==\"\") return true;" + "var str=el.value;" + "if(1){" + "var filter1=/^(([a-zA-Z]:)|(\\\\{2}\\w+)\\$?)(\\\\(\\w[\\w ]*))+(\\.([a-zA-Z]){3}){0,1}$/i  }" + "if(1){" + " var filter2=/^\\/(\\w+\\/)*([\\w])+(\\.([a-zA-Z]){3}){0,1}$/i\t" + "\t}" + " if (filter1.test(str) || filter2.test(str))" + "\tvar testresults=true;" + "\telse {alert('" + strMsg + "'); el.focus();el.select();testresults=false;}" + "\treturn (testresults);" + "\t}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!ValidatePATH" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string ValidatePATH(string strId, string strMsg, string strBlankMsg)
        {
            string str1 = "" + "function ValidatePATH" + strId + "(){" + "var el=document.getElementById('" + strId + "');" + "var elval= el.value.replace(/[\\n\\r\\s]+/,\"\");" + "if(elval==\"\") { alert('" + strBlankMsg + "');el.focus();el.select(); return false;}" + "var str=el.value;" + "if(1){" + "var filter1=/^(([a-zA-Z]:)|(\\\\{2}\\w+)\\$?)(\\\\(\\w[\\w ]*))+(\\.([a-zA-Z]){3}){0,1}$/i  }" + "if(1){" + " var filter2=/^\\/(\\w+\\/)*([\\w])+(\\.([a-zA-Z]){3}){0,1}$/i\t" + "\t}" + " if (filter1.test(str) || filter2.test(str))" + "\tvar testresults=true;" + "\telse {alert('" + strMsg + "'); el.focus();el.select();testresults=false;}" + "\treturn (testresults);" + "\t}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!ValidatePATH" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string CountCharacter()
        {
            return "" + "function CountCharacter(strpCharObject,strpCntObject)" + "{" + "var el = eval(\"strpCharObject\");" + "var strVal=strpCharObject.value;" + "if(strpCharObject.value!=''){" + "var str=strpCharObject.value;" + "var Bal=el.maxLen-str.length;" + "if(Bal>=0){strpCntObject.value=Bal};" + "if(Bal<=0){" + "sub=strVal.substr(0,20);" + "strpCharObject.value=sub;" + "}" + "}" + "if(strpCharObject.value==''){" + "strpCntObject.value=el.maxLen;" + "}" + "}";
        }

        public string IsDate(string strId, string strMsg)
        {
            string str1 = "" + "function IsDate" + strId + "()" + "{" + "var el=document.getElementById('" + strId + "');" + "var txt = el.value;" + "\tif (/(((0[13578]|10|12)([/])(0[1-9]|[12][0-9]|3[01])([/])(\\d{4}))|((0[469]|11)([/])([0][1-9]|[12][0-9]|30)([/])(\\d{4}))|((02)([/])(0[1-9]|1[0-9]|2[0-8])([/])(\\d{4}))|((02)([/])(29)(\\.|-|\\/)([02468][048]00))|((02)([/])(29)([/])([13579][26]00))|((02)([/])(29)([/])([0-9][0-9][0][48]))|((02)([/])(29)([/])([0-9][0-9][2468][048]))|((02)([/])(29)([/])([0-9][0-9][13579][26])))/.test(txt)) " + "{" + "return true;" + " }" + "else " + "{ alert('" + strMsg + "'); el.focus();el.select();return false;" + "}" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!IsDate" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        private string IsDate()
        {
            return "" + "function IsDate(el)" + "{" + "var txt = el.value;" + "\tif (/(((0[13578]|10|12)([/])(0[1-9]|[12][0-9]|3[01])([/])(\\d{4}))|((0[469]|11)([/])([0][1-9]|[12][0-9]|30)([/])(\\d{4}))|((02)([/])(0[1-9]|1[0-9]|2[0-8])([/])(\\d{4}))|((02)([/])(29)(\\.|-|\\/)([02468][048]00))|((02)([/])(29)([/])([13579][26]00))|((02)([/])(29)([/])([0-9][0-9][0][48]))|((02)([/])(29)([/])([0-9][0-9][2468][048]))|((02)([/])(29)([/])([0-9][0-9][13579][26])))/.test(txt)) " + "{" + "return true;" + " }" + "\telse" + "{" + "el.focus();el.select();" + "return false;" + "}" + "}";
        }

        public string CheckSelectedIndex(string strId, string strMsg)
        {
            string str1 = "" + "function CheckSelectedIndex" + strId + "()" + "{" + "var el=document.getElementById('" + strId + "');" + "if((el.selectedIndex==0)||(el.selectedIndex==-1))" + "{" + "alert('" + strMsg + "');" + "el.focus();" + "return false;" + "}" + "else " + "{" + "return true;" + "}" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!CheckSelectedIndex" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string CheckSelectedIndexListBox(string strId, string strMsg)
        {
            string str1 = "" + "function CheckSelectedIndexListBox" + strId + "()" + "{" + "var el=document.getElementById('" + strId + "');" + "if(el.selectedIndex==-1)" + "{" + "alert('" + strMsg + "');" + "el.focus();" + "return false;" + "}" + "else " + "{" + "return true;" + "}" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!CheckSelectedIndexListBox" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string IsBlank(string strId, string strMsg)
        {
            string str1 = "" + "function IsBlank" + strId + "()" + "{" + "var el=document.getElementById('" + strId + "');" + "var elvalue=el.value.replace(/^\\s+/g,'').replace(/\\s+$/g,'');" + "if(elvalue=='')" + "{" + "alert('" + strMsg + "');" + "el.focus();" + "return false;" + "}" + "else " + "{" + "return true;" + "}" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!IsBlank" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string IsBlankHidden(string strId, string strMsg)
        {
            string str1 = "" + "function IsBlankHidden" + strId + "()" + "{" + "var el=document.getElementById('" + strId + "');" + "var elvalue=el.value.replace(/^\\s+/g,'').replace(/\\s+$/g,'');" + "if(elvalue=='')" + "{" + "alert('" + strMsg + "');" + "return false;" + "}" + "else " + "{" + "return true;" + "}" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!IsBlankHidden" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string IsImageSRCBlank(string strId, string strMsg)
        {
            string str1 = "" + "function IsImageSRCBlank" + strId + "()" + "{" + "var el=document.getElementById('" + strId + "');" + "var elvalue=el.src.replace(/^\\s+/g,'').replace(/\\s+$/g,'');" + "if(elvalue=='')" + "{" + "alert('" + strMsg + "');" + "return false;" + "}" + "else " + "{" + "return true;" + "}" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!IsImageSRCBlank" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string ConfirmValue(string strFirstControlId, string strSecondControlId, string strMsg)
        {
            string str1 = "" + "function ConfirmValue" + strFirstControlId + "()" + "{" + "var elFirst=document.getElementById('" + strFirstControlId + "');" + "var elSecond=document.getElementById('" + strSecondControlId + "');" + "if(elFirst.value!=elSecond.value)" + "{" + "alert('" + strMsg + "');" + "elSecond.focus();" + "return false;" + "}" + "else " + "{" + "return true;" + "}" + "}";
            clsJavaScriptValidator javaScriptValidator = this;
            string str2 = javaScriptValidator.strReturnString + "if (!ConfirmValue" + strFirstControlId + "()) return false;";
            javaScriptValidator.strReturnString = str2;
            return str1;
        }

        public string IsChecked(string strId, string strMsg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("function IsChecked" + strId + "()");
            stringBuilder.Append("{");
            stringBuilder.Append("var el=document.getElementById('" + strId + "');");
            stringBuilder.Append("if(!el.checked)");
            stringBuilder.Append("{");
            stringBuilder.Append("alert('" + strMsg + "');");
            stringBuilder.Append("el.focus();");
            stringBuilder.Append("return false;");
            stringBuilder.Append("}");
            stringBuilder.Append("else");
            stringBuilder.Append("{");
            stringBuilder.Append("return true;");
            stringBuilder.Append("}");
            stringBuilder.Append("}");
            clsJavaScriptValidator javaScriptValidator = this;
            string str = javaScriptValidator.strReturnString + "if (!IsChecked" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str;
            return stringBuilder.ToString();
        }

        public string CompareTwoValues(string strFirstControl, string strSecondControl, string strMsg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("function IsChecked" + strFirstControl + "()");
            stringBuilder.Append("{");
            stringBuilder.Append("var el1=document.getElementById('" + strFirstControl + "');");
            stringBuilder.Append("var el2=document.getElementById('" + strSecondControl + "');");
            stringBuilder.Append("if(el1.value>el2.value)");
            stringBuilder.Append("{");
            stringBuilder.Append("alert('" + strMsg + "');");
            stringBuilder.Append("return false;");
            stringBuilder.Append("}");
            stringBuilder.Append("else");
            stringBuilder.Append("{");
            stringBuilder.Append("return true;");
            stringBuilder.Append("}");
            stringBuilder.Append("}");
            clsJavaScriptValidator javaScriptValidator = this;
            string str = javaScriptValidator.strReturnString + "if (!CompareTwoValues" + strFirstControl + "()) return false;";
            javaScriptValidator.strReturnString = str;
            return stringBuilder.ToString();
        }

        public string CompareDate(string strFirstControl, string strSecondControl, string strMsg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("function CompareDate" + strFirstControl + "()");
            stringBuilder.Append("{");
            stringBuilder.Append("var el1=document.getElementById('" + strFirstControl + "');");
            stringBuilder.Append("var el2=document.getElementById('" + strSecondControl + "');");
            stringBuilder.Append("if((new Date(el1.value))>(new Date(el2.value)))");
            stringBuilder.Append("{");
            stringBuilder.Append("alert('" + strMsg + "');");
            stringBuilder.Append("return false;");
            stringBuilder.Append("}");
            stringBuilder.Append("else");
            stringBuilder.Append("{");
            stringBuilder.Append("return true;");
            stringBuilder.Append("}");
            stringBuilder.Append("}");
            clsJavaScriptValidator javaScriptValidator = this;
            string str = javaScriptValidator.strReturnString + "if (!CompareDate" + strFirstControl + "()) return false;";
            javaScriptValidator.strReturnString = str;
            return stringBuilder.ToString();
        }

        public string MinLimit(string strId, int intMinLimit, string strMsg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("function MinLimit" + strId + "()");
            stringBuilder.Append("{");
            stringBuilder.Append("var el=document.getElementById('" + strId + "');");
            stringBuilder.Append("if((el.value.length)<" + (object)intMinLimit + ")");
            stringBuilder.Append("{");
            stringBuilder.Append("alert('" + strMsg + "');");
            stringBuilder.Append("el.focus();");
            stringBuilder.Append("return false;");
            stringBuilder.Append("}");
            stringBuilder.Append("else");
            stringBuilder.Append("{");
            stringBuilder.Append("return true;");
            stringBuilder.Append("}");
            stringBuilder.Append("}");
            clsJavaScriptValidator javaScriptValidator = this;
            string str = javaScriptValidator.strReturnString + "if (!MinLimit" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str;
            return stringBuilder.ToString();
        }

        public string MaxLimit(string strId, int intMaxLimit, string strMsg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("function MaxLimit" + strId + "()");
            stringBuilder.Append("{");
            stringBuilder.Append("var el=document.getElementById('" + strId + "');");
            stringBuilder.Append("if((el.value.length)>" + (object)intMaxLimit + ")");
            stringBuilder.Append("{");
            stringBuilder.Append("alert('" + strMsg + "');");
            stringBuilder.Append("el.focus();");
            stringBuilder.Append("return false;");
            stringBuilder.Append("}");
            stringBuilder.Append("else");
            stringBuilder.Append("{");
            stringBuilder.Append("return true;");
            stringBuilder.Append("}");
            stringBuilder.Append("}");
            clsJavaScriptValidator javaScriptValidator = this;
            string str = javaScriptValidator.strReturnString + "if (!MaxLimit" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str;
            return stringBuilder.ToString();
        }

        public string Limit(string strId, int intMinLimit, int intMaxLimit, string strMsg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("function Limit" + strId + "()");
            stringBuilder.Append("{");
            stringBuilder.Append("var el=document.getElementById('" + strId + "');");
            stringBuilder.Append("if(((el.value.length)>" + (object)intMaxLimit + ")||((el.value.length)<" + (object)intMinLimit + "))");
            stringBuilder.Append("{");
            stringBuilder.Append("alert('" + strMsg + "');");
            stringBuilder.Append("el.focus();");
            stringBuilder.Append("return false;");
            stringBuilder.Append("}");
            stringBuilder.Append("else");
            stringBuilder.Append("{");
            stringBuilder.Append("return true;");
            stringBuilder.Append("}");
            stringBuilder.Append("}");
            clsJavaScriptValidator javaScriptValidator = this;
            string str = javaScriptValidator.strReturnString + "if (!Limit" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str;
            return stringBuilder.ToString();
        }

        public string isMaxValue(string strId, int intMaxValue, string strMsg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("function isMaxValue" + strId + "()");
            stringBuilder.Append("{");
            stringBuilder.Append("var el=document.getElementById('" + strId + "');");
            stringBuilder.Append("if(el.value >" + (object)intMaxValue + ")");
            stringBuilder.Append("{");
            stringBuilder.Append("alert('" + strMsg + "');");
            stringBuilder.Append("el.focus();");
            stringBuilder.Append("return false;");
            stringBuilder.Append("}");
            stringBuilder.Append("else");
            stringBuilder.Append("{");
            stringBuilder.Append("return true;");
            stringBuilder.Append("}");
            stringBuilder.Append("}");
            clsJavaScriptValidator javaScriptValidator = this;
            string str = javaScriptValidator.strReturnString + "if (!isMaxValue" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str;
            return stringBuilder.ToString();
        }

        public string isMinValue(string strId, int intMinValue, string strMsg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("function isMinValue" + strId + "()");
            stringBuilder.Append("{");
            stringBuilder.Append("var el=document.getElementById('" + strId + "');");
            stringBuilder.Append("if(el.value <" + (object)intMinValue + ")");
            stringBuilder.Append("{");
            stringBuilder.Append("alert('" + strMsg + "');");
            stringBuilder.Append("el.focus();");
            stringBuilder.Append("return false;");
            stringBuilder.Append("}");
            stringBuilder.Append("else");
            stringBuilder.Append("{");
            stringBuilder.Append("return true;");
            stringBuilder.Append("}");
            stringBuilder.Append("}");
            clsJavaScriptValidator javaScriptValidator = this;
            string str = javaScriptValidator.strReturnString + "if (!isMinValue" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str;
            return stringBuilder.ToString();
        }

        public string LimitValue(string strId, int intMinLimitValue, int intMaxLimitValue, string strMsg)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("function LimitValue" + strId + "()");
            stringBuilder.Append("{");
            stringBuilder.Append("var el=document.getElementById('" + strId + "');");
            stringBuilder.Append("if((el.value >" + (object)intMaxLimitValue + ")||(el.value <" + (object)intMinLimitValue + "))");
            stringBuilder.Append("{");
            stringBuilder.Append("alert('" + strMsg + "');");
            stringBuilder.Append("el.focus();");
            stringBuilder.Append("return false;");
            stringBuilder.Append("}");
            stringBuilder.Append("else");
            stringBuilder.Append("{");
            stringBuilder.Append("return true;");
            stringBuilder.Append("}");
            stringBuilder.Append("}");
            clsJavaScriptValidator javaScriptValidator = this;
            string str = javaScriptValidator.strReturnString + "if (!LimitValue" + strId + "()) return false;";
            javaScriptValidator.strReturnString = str;
            return stringBuilder.ToString();
        }
    }
}