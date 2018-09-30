using EnjoyOurTour.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace EnjoyOurTour.Helpers.Services
{
    public static class MetadataServices
    {
        public static DateTime GetCurrentDate()
        {
            return DateTime.Now.AddHours(8);
        }

        public static string GetUsername(string id)
        {
            using (var db = new TourEntities())
            {
                var userId = Convert.ToInt64(id);
                var username = (from u in db.User
                                where u.UserId == userId
                                select u.Username).FirstOrDefault();
                return username;
            }
        }

        public static string GetDateTimeWithoutSlash()
        {
            return DateTime.Now.AddHours(8).ToString("yyyyMMddHHmmss");
        }

        public static User GetCurrentUser()
        {
            User user = new User();
            var requestCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (requestCookie != null)
            {
                string cookie = requestCookie.Value;
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie);

                if (ticket != null)
                {
                    using (var db = new TourEntities())
                    {
                        user = db.User.FirstOrDefault(e => e.UserId.ToString() == ticket.Name);
                    }
                }
            }
            return user;
        }

        public static string GenerateCode(int length)
        {
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            return result.ToString();
        }

        public static async Task SendEmail(string toEmailAddress, string subject, string body)
        {
            Environment.SetEnvironmentVariable("SENDGRID_APIKEY", "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");
            var apiKey = System.Environment.GetEnvironmentVariable("SENDGRID_APIKEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jefferyleo@gmail.com", "jefferyleo");
            var to = new EmailAddress(toEmailAddress);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", body);
            var response = await client.SendEmailAsync(msg);
            var statusCode = response.StatusCode;
        }

        public static void SendWhatsappMessage(string content, string phoneNumber)
        {
            string accountSid = "ACb9fedb74e2e8f915ac28d17318b24da1";
            string authToken = "94db3315dbcc31653f4c5f4b2a4f7611";

            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
                    body: content,
                    from: new Twilio.Types.PhoneNumber("whatsapp:+14155238886"),
                    to: new Twilio.Types.PhoneNumber("whatsapp:" + phoneNumber + "")
                );
        }
        /// <summary>
        /// format the string, example in startup.cs
        /// </summary>
        /// <param name="fstr"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string F(
            this string fstr,
            params object[] args)
        {
            return string.Format(fstr, args);
        }
    }
}