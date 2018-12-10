using System.Collections.Generic;

namespace Gwc.Common.Dtos
{
    public class SendGridTransactionalMessageDto
    {
        public string ApiKey { get; set; }
        public string Bcc { get; set; }
        public List<string> Categories { get; set; }
        public bool DevelopmentMode { get; set; }
        public bool DisableUnsubscribe { get; set; }
        public bool EnableClickTracking { get; set; }
        public string FromAddress { get; set; }
        public string HtmlMessage { get; set; }
        public string Recipients { get; set; }
        public string Subject { get; set; }
        public Dictionary<string, string> Substitutions { get; set; }
        public string TemplateEngine { get; set; }
        public string TextMessage { get; set; }

        public SendGridTransactionalMessageDto()
        {
            DevelopmentMode = false;
            DisableUnsubscribe = true;
            EnableClickTracking = false;
            HtmlMessage = "<span></span>";
            TextMessage = " ";
        }
    }
}
