namespace DutchTreat.Services
{
    public interface IMailService
    {
        void SendEmail(string email, string subject, string body);
    }
}
