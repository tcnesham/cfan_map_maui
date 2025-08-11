using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui;

namespace CFAN.SchoolMap.Services
{
    public static class EmailSender
    {
        public static async Task SendEmail(string subject, string body, List<string> recipients, string attachmentPath=null)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                };
                if (attachmentPath != null)
                {
                    message.Attachments.Add(new EmailAttachment(attachmentPath));
                }

                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device  
            }
            catch (Exception ex)
            {
                // Some other exception occurred  
            }
        }
    }
}
