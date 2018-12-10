using Gwc.Common.Dtos;
using Gwc.Common.Utilities.Constants;
using NUnit.Framework;

namespace Gwc.Testing.IntegrationTesting
{
    public class SendGridServiceIntegrationFixture : BaseIntegrationTestFixture
    {
        [Test]
        public void LoggingTest()
        {
            var email = new SendGridTransactionalMessageDto
            {
                ApiKey = _sendGridApiKey,
                Recipients = SendGridConstants.SendGrid_DevelopmentRecipientEmail,
                FromAddress = SendGridConstants.SendGrid_DevelopmentRecipientEmail,
                Subject = "Test of SendGrid email",
                TextMessage = "Test of SendGrid email"
            };

            var response = _sendGridService.SendSingleEmail(email);

           
            Assert.That(response.Valid, Is.True);
        }

    }
}