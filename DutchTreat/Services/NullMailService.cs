namespace DutchTreat.Services
{
    public class NullMailService : IMailService
    {
        private readonly ILogger<NullMailService> _logger;

        public NullMailService(ILogger<NullMailService> logger)
        {
            this._logger = logger;
        }
        public void SendEmail(string email, string subject, string body)
        {
            // Log it
            this._logger.LogInformation($"Email: {email} Subject: {subject} Body: {body}");
        }
    }
}
