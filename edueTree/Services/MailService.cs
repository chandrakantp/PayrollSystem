using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace edueTree.Services
{
    public class MailService
    {
        public string SendMail(string mMailId, string mBody, string mSubject, string mCc)
        {
            ////from sending mail Credential  information
            string senderMail = ConfigurationManager.AppSettings["UserID"].ToString();
            string password = ConfigurationManager.AppSettings["Password"].ToString();
            string host = ConfigurationManager.AppSettings["Host"].ToString();
            int port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());

            try
            {
                using (MailMessage message = new MailMessage(senderMail, mMailId)
                {
                    Subject = mSubject,
                    Body = mBody,
                    IsBodyHtml = true
                })
                {
                    using (SmtpClient client = new SmtpClient(host, port)
                    {
                        EnableSsl = true,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderMail, password)
                    })
                    {
                        client.Send(message);
                    }
                }


                ////SMTP server setting
                //var credentials = new System.Net.NetworkCredential(senderMail, password);
                //var client = new SmtpClient(host)
                //{
                //    Port = Convert.ToInt32(port),
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    UseDefaultCredentials = true,
                //    EnableSsl = true,
                //    Credentials = credentials,
                //    Timeout = 500000
                //};

                ////Mail sending 
                //var mail = new MailMessage(senderMail.Trim(), mMailId.Trim(), mSubject, mBody);
                //mail.IsBodyHtml = true;
                //client.Send(mail);
                return "please check your mail - " + mMailId;
            }
            catch (SmtpException exs)
            {
                return exs.ToString();
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }
    }
}