using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace edueTree.Services
{
    public class MailServiceConfig
    {
        public string SendMail(string mMailId, string mBody, string mSubject, string mCc ,string pass , string hostname , string portNo)
        {
           
            //string senderMail = ConfigurationManager.AppSettings["UserID"].ToString();
            //string password = ConfigurationManager.AppSettings["Password"].ToString();
            //string host = ConfigurationManager.AppSettings["Host"].ToString();
            //int port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"].ToString());
            

            string senderMail = mCc;
            string password = pass;
            string host = hostname;
            int port = Convert.ToInt32(portNo);


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
                        EnableSsl = false,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderMail, password)
                    })
                    {
                        client.Send(message);
                    }
                }
                return "Please check your Email and click Reset Password link";
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