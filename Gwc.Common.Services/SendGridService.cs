using Gwc.Common.Dtos;
using Gwc.Common.Interfaces.Services;
using Gwc.Common.Utilities.Constants;
using Gwc.Common.Utilities.Extensions;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Gwc.Common.Services
{
    public class SendGridService : ISendGridService
    {
        public SendGridResponseDto SendTransactionalEmail(SendGridTransactionalMessageDto messageDto)
                    {
            var response = new SendGridResponseDto
            {
                Valid = false
            };

            if (messageDto.TemplateEngine == null)
            {
                response.Message = "SendGrid Template is missing";
                return response;
            }

            if (messageDto.Subject == null)
            {
                response.Message = "Subject required";
                return response;
            }

            if (string.IsNullOrEmpty(messageDto.Recipients))
            {
                response.Message = "Recipient(s) required";
                return response;
            }

            try
            {
                var client = new SendGridClient(messageDto.ApiKey);
                var message = new SendGridMessage();
                message.AddTo(messageDto.Recipients);

                if (!string.IsNullOrEmpty(messageDto.Bcc))
                {
                    message.AddBcc(messageDto.Bcc);
                }

                message.SetFrom(new EmailAddress(messageDto.FromAddress));
                message.SetSubject(messageDto.Subject);

                if (messageDto.TextMessage.IsNotNullOrEmpty())
                {
                    message.AddContent(MimeType.Text, messageDto.TextMessage);
                }

                if (messageDto.HtmlMessage.IsNotNullOrEmpty())
                {
                    message.AddContent(MimeType.Html, messageDto.HtmlMessage);
                }

                if (messageDto.Categories != null)
                {
                    message.Categories = messageDto.Categories;
                }

                if (messageDto.Substitutions != null)
                {
                    foreach (var key in messageDto.Substitutions.Keys)
                    {
                        message.AddSubstitution(key, messageDto.Substitutions[key].ToString());
                    }
                }

                if (messageDto.TemplateEngine.IsNotNullOrEmpty())
                {
                    message.TemplateId = messageDto.TemplateEngine;
                }

                message.TrackingSettings = new TrackingSettings();
                message.TrackingSettings.ClickTracking =
                    new ClickTracking
                    {
                        Enable = messageDto.EnableClickTracking
                    };

                message.SetSubscriptionTracking(!messageDto.DisableUnsubscribe);
                var sgResponse = client.SendEmailAsync(message);
                Thread.Sleep(1000);

                response.Valid = true;
                response.Message = string.Concat(messageDto.Subject, " sent to ", messageDto.Recipients);
            }
            catch (Exception ex)
            {
                response.Valid = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public SendGridResponseDto SendSingleEmail(SendGridTransactionalMessageDto messageDto)
        {
            var response = new SendGridResponseDto
            {
                Valid = false
            };

            if (messageDto.Subject == null)
            {
                response.Message = "Subject required";
                return response;
            }

            if (string.IsNullOrEmpty(messageDto.Recipients))
            {
                response.Message = "Recipient(s) required";
                return response;
            }

            if (messageDto.TextMessage.IsNullOrEmpty() && messageDto.HtmlMessage.IsNullOrEmpty())
            {
                response.Message = "Email body required.";
                return response;
            }

            try
            {
                var client = new SendGridClient(messageDto.ApiKey);
                var message = new SendGridMessage();
                var recipient = messageDto.DevelopmentMode ? SendGridConstants.SendGrid_DevelopmentRecipientEmail : messageDto.Recipients;
                message.AddTo(recipient);

                if (!string.IsNullOrEmpty(messageDto.Bcc))
                {
                    message.AddBcc(messageDto.Bcc);
                }

                message.SetFrom(new EmailAddress(messageDto.FromAddress));
                message.SetSubject(messageDto.Subject);

                if (messageDto.TextMessage.IsNotNullOrEmpty())
                {
                    message.AddContent(MimeType.Text, messageDto.TextMessage);
                }

                if (messageDto.HtmlMessage.IsNotNullOrEmpty())
                {
                    message.AddContent(MimeType.Html, messageDto.HtmlMessage);
                }

                if (messageDto.Categories != null)
                {
                    message.Categories = messageDto.Categories;
                }

                var sgResponse = client.SendEmailAsync(message);
                Thread.Sleep(1000);

                response.Valid = true;
                response.Message = string.Concat(messageDto.Subject, " sent to ", messageDto.Recipients);
            }
            catch (Exception ex)
            {
                response.Valid = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
