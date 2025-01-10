namespace SecretSantaAPI.Interface
{
    public interface IEmailSenderService
    {
        public Task<bool> SendEmail(string email, string buddyName, string buddyEmail, string userName);
    }
}
