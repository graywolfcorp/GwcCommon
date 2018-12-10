using Gwc.Common.Dtos;

namespace Gwc.Common.Interfaces.Services
{
    public interface ISendGridService
    {
        SendGridResponseDto SendSingleEmail(SendGridTransactionalMessageDto messageDto);
        SendGridResponseDto SendTransactionalEmail(SendGridTransactionalMessageDto messageDto);
    }
}
