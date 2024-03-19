using Tms.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using EcommerceSystem.Core.Configurations;

namespace Tms.Services
{
    public interface IMailService : IDisposable
    {
        Task<bool> SendMail(MailAddress mailFrom, MailAddress mailTo, string subject, string body, SmtpInfo smtp, out string errorMsg, List<Attachment> attachments = null);
        bool SendMail(string sendTo, string title, string body, out string errorMsg, List<string> attachments = null);

        void SendMail(string sendFrom, string sendTo, string title, string body);
    }
    public class MailService : IMailService
    {
        protected static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #region Methods
        public bool SendMail(string sendTo, string title, string body, out string errorMsg, List<string> attachments = null)
        {

            var host = SystemConfiguration.GetStringKey("SmtpMailServer");
            var username = SystemConfiguration.GetStringKey("MailServerUserName");
            var dispayName = SystemConfiguration.GetStringKey("MailServerUserName");
            var password = SystemConfiguration.GetStringKey("MailServerPassword");
            var port = SystemConfiguration.GetInt32("MailServerPort");

            var mailFrom = new MailAddress(username, "aff.edumore.vn");
            var mailTo = new MailAddress(sendTo);
            var smtpInfo = new SmtpInfo()
            {
                AuthenticationPassword = password,
                AuthenticationUserName = username,
                HasAuthentication = true,
                SmtpHost = host,
                SmtpPort = port
            };
            var attachmentFiles = attachments?.Select(x => new Attachment(x)).ToList();
            return SendMail(mailFrom, mailTo, title, body, smtpInfo, out errorMsg, attachmentFiles).Result;

        }

        public Task<bool> SendMail(MailAddress mailFrom, MailAddress mailTo, string subject, string body, SmtpInfo smtp, out string errorMsg, List<Attachment> attachments = null)
        {
            //step1: validate param input
            errorMsg = string.Empty;
            if (!ValidateParamForSendMail(mailFrom, mailTo, subject, body, smtp, out errorMsg))
            {
                return Task.FromResult(false);
            }
            try
            {
                //Send mail.
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Host = smtp.SmtpHost;
                    smtpClient.Port = smtp.SmtpPort;
                    smtpClient.UseDefaultCredentials = false;
                    if (smtp.HasAuthentication)
                    {
                        smtpClient.Credentials = new NetworkCredential(smtp.AuthenticationUserName,
                                                                       smtp.AuthenticationPassword);
                    }

                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.EnableSsl = true;
                    ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = mailFrom;
                        mailMessage.To.Add(mailTo);
                        mailMessage.Subject = subject;
                        mailMessage.Body = body;
                        mailMessage.IsBodyHtml = true;
                        if (attachments != null)
                            foreach (var attachment in attachments)
                            {
                                mailMessage.Attachments.Add(attachment);
                            }
                        smtpClient.Send(mailMessage);
                        return Task.FromResult(true);
                    }
                }
            }
            catch (Exception exception)
            {
                errorMsg = "Server Internal Error: " + exception.Message;
                return Task.FromResult(false);
            }

        }

        public void SendMail(string sendFrom, string sendTo, string title, string body)
        {

            var host = SystemConfiguration.GetStringKey("SmtpMailServer");
            var username = SystemConfiguration.GetStringKey("MailServerUserName");
            var dispayName = SystemConfiguration.GetStringKey("MailServerUserName");
            var password = SystemConfiguration.GetStringKey("MailServerPassword");
            var port = SystemConfiguration.GetInt32("MailServerPort");

            var mailFrom = new MailAddress(sendFrom);
            var mailTo = new MailAddress(sendTo);
            var smtpInfo = new SmtpInfo()
            {
                AuthenticationPassword = password,
                AuthenticationUserName = username,
                HasAuthentication = true,
                SmtpHost = host,
                SmtpPort = port
            };
            var errorMsg = string.Empty;
            SendMail(mailFrom, mailTo, title, body, smtpInfo, out errorMsg);

        }
        #endregion

        #region Helpers    
        private bool ValidateParamForSendMail(MailAddress mailFrom, MailAddress mailTo, string subject, string body,
                                            SmtpInfo smtp, out string errorMsg)
        {
            errorMsg = string.Empty;
            if (mailFrom == null)
            {
                _log.Error(
                    string.Format(
                        "Email sender null when send mail for: {0} subject: {1} body: {2} smtp: {3} at: {4} - {5}",
                        mailTo, subject, body, smtp, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
                errorMsg = "MailFrom is required";
                return false;
            }

            if (mailTo == null)
            {
                _log.Error(
                    string.Format("Email receive null when send mail by: {0} subject: {1} body: {2} smtp: {3} at: {4} - {5}",
                                  mailFrom, subject, body, smtp, DateTime.Now.ToShortDateString(),
                                  DateTime.Now.ToShortTimeString()));
                errorMsg = "MailTo is required";
                return false;
            }

            if (string.IsNullOrEmpty(subject))
            {
                _log.Error(string.Format("Subject when send Email by: {0} To: {1} body: {2} smtp: {3} at: {4} - {5}",
                                              mailTo, mailFrom, body, smtp, DateTime.Now.ToShortDateString(),
                                              DateTime.Now.ToShortTimeString()));
                errorMsg = "Subject email is required";
                return false;
            }

            if (body == null)
            {
                _log.Error(string.Format("body when send Email by: {0} To: {1} subject: {2} smtp: {3} at: {4} - {5}",
                                              mailTo, mailFrom, subject, smtp, DateTime.Now.ToShortDateString(),
                                              DateTime.Now.ToShortTimeString()));

                errorMsg = "Body email is required";
                return false;
            }

            if (smtp == null)
            {
                _log.Error(string.Format("smtp null when send mail by: {0} To: {1} subject: {2} body: {3} at: {4} - {5}",
                                              mailTo, mailFrom, subject, body, DateTime.Now.ToShortDateString(),
                                              DateTime.Now.ToShortTimeString()));
                errorMsg = "Smtp info email is required";
                return false;
            }
            return true;
        }
        #endregion

        #region GCs    
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        // Dispose() calls Dispose(true)
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
