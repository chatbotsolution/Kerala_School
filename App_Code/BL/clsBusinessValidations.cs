using Classes.DA;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web.Mail;

namespace Classes.BL
{
    public class clsBusinessValidations
    {
        public bool IsExists(string strSQL)
        {
            DataTable dataTable1 = new DataTable();
            clsData clsData = new clsData();
            clsData.strSql = strSQL;
            DataTable dataTable2 = clsData.GetDataTable();
            clsData.Dispose();
            return dataTable2.Rows.Count > 0;
        }

        public NameValueCollection getUserIdTypeCollection(string strEmail, string strPassword)
        {
            NameValueCollection nameValueCollection = new NameValueCollection();
            clsData clsData = new clsData();
            Hashtable ht = new Hashtable();
            ht.Add((object)"Email", (object)strEmail);
            ht.Add((object)"Password", (object)strPassword);
            DataTable dataTable1 = new DataTable();
            DataTable dataTable2 = clsData.GetDataTable("uspLogin", ht);
            if (dataTable2.Rows.Count > 0)
            {
                nameValueCollection.Add("ID", dataTable2.Rows[0]["UserId"].ToString());
                nameValueCollection.Add("Type", dataTable2.Rows[0]["Type"].ToString());
            }
            else
            {
                nameValueCollection.Add("ID", "-1");
                nameValueCollection.Add("Type", "S");
            }
            dataTable2.Dispose();
            clsData.Dispose();
            return nameValueCollection;
        }

        public void SendMail(string strMailTo)
        {
            MailMessage message = new MailMessage();
            SmtpMail.SmtpServer = ConfigurationSettings.AppSettings["smtpServer"];
            message.From = ConfigurationSettings.AppSettings["FromEmail"];
            message.To = strMailTo;
            message.Subject = "Your password for wanted NI";
            message.Body = GeneratePassword.GetPassword(6);
            SmtpMail.Send(message);
        }

        public void SendMail(string strMailTo, string strPassword)
        {
            MailMessage message = new MailMessage();
            SmtpMail.SmtpServer = ConfigurationSettings.AppSettings["smtpServer"];
            message.From = ConfigurationSettings.AppSettings["FromEmail"];
            message.To = strMailTo;
            message.Subject = "Your password for wanted NI";
            message.Body = strPassword;
            SmtpMail.Send(message);
        }
    }
}