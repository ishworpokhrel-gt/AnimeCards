using Models.Email;

namespace Common_Shared.Email
{
    public interface IEmailSender
    {
        void QueueEmail(SendEmailRequestModel model);
        Task SendEmailAsync(SendEmailRequestModel model);

    }
}
