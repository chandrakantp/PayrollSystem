using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace edueTree.Models
{
    #region ------ External Login Confirmation View Model --------
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    } 
    #endregion

    #region --------------- Manage User View Model ---------------
    public class ManageUserViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    } 
    #endregion

    #region ---------------- Login View Model --------------------
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    } 
    #endregion

    #region --------------- Register View Model ------------------
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    } 
    #endregion

    #region --------------- Lost Password Model ------------------
    public class LostPasswordModel
    {
        //[Required(ErrorMessage = "We need your email to send you a reset link!")]
        //[Display(Name = "Your account email")]
        [Required(ErrorMessage = "We need your email to send you a reset link!")]
        [EmailAddress(ErrorMessage = "Not a valid email--what are you trying to do here?")]
        public string Email { get; set; }

        //[Required(ErrorMessage = "User Name field cannot be empty")]
        public string UserName { get; set; }
    } 
    #endregion

    #region -------------- Reset Password View Model -------------
    public class ResetPasswordViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "New Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "New password and confirmation does not match.")]
        public string ConfirmPassword { get; set; }

        public string ReturnToken { get; set; }
    } 
    #endregion

    #region ----------------- Menu Aggregate ---------------------
    public class MenuAggregate
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Action { get; set; }
        public string Control { get; set; }
        public string MainMenu { get; set; }
    } 
    #endregion

    #region ---------------- Email Manager -----------------------
    public class EmailManager
    {
        public void AppSettings(out string UserID, out string Password, out string SMTPPort, out string Host)
        {
            UserID = ConfigurationManager.AppSettings.Get("UserID");
            Password = ConfigurationManager.AppSettings.Get("Password");
            SMTPPort = ConfigurationManager.AppSettings.Get("SMTPPort");
            Host = ConfigurationManager.AppSettings.Get("Host");
        }

        public void SendEmail(string from, string subject, string body, string mailTo, string userId, string password, string smtpPort, string host)
        {
            //string from = "innovativefusiontest@gmail.com";
            //string userId = "innovativefusiontest@gmail.com";
            //string password = "innovate123";
            //string smtpPort = "25";
            //string host = "smtp.gmail.com";
            //relay-hosting.secureserver.net/
            var mail = new MailMessage();
            foreach (var address in mailTo.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                mail.To.Add(address);
            }
            //mail.To.Add(mailTo);
            mail.From = new MailAddress(from);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            var smtp = new SmtpClient(host, Convert.ToInt16(smtpPort))
            {
                Credentials = new NetworkCredential(userId, password),
                EnableSsl = true,

            };
            smtp.Send(mail);
        }

    } 
    #endregion
}
