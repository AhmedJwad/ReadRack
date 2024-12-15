using ReadRack.Shared.Responses;

namespace ReadRack.Backend.Helpers
{
    public interface IMailHelper
    {
        ActionResponse<string> SendMail(string toName, string toEmail, string subject, string body);
    }
}
