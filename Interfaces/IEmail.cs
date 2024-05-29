using Microsoft.AspNetCore.Mvc;

namespace APICruzber.Interfaces
{
    public interface IEmail
    {
        Task<IActionResult> GetMails(string token,string lang);
    }
}
