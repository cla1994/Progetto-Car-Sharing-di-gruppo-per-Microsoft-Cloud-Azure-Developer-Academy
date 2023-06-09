using Academy2023.Net.Models;
using Academy2023.Net.Services;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace Academy2023.Net.Controllers
{
    public class SMTPController : Controller
    {
        private readonly IMailService _mail;

        public SMTPController(IMailService mail)
        {
            _mail = mail;
        }

        public async Task<IActionResult> InvioEmail(List<string> to_list,string subject,string body)
        {
            //Creo se vuoto
            if (to_list.Count() == 0)
            {
                to_list = new List<string>();
                to_list.Add("asaro.alex00@gmail.com");
                to_list.Add("nabbazzo9@gmail.com");
            }

            if(subject==null || subject=="") subject = "ziopaolo";

            string html_body = System.IO.File.ReadAllText("./Services/base_mail.html");

            MailData data = new MailData(to_list, subject,html_body);

            bool result = await _mail.SendAsync(data, new CancellationToken());
            if (result)
            {
                return StatusCode(StatusCodes.Status200OK, "Mail has successfully been sent.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occured. The Mail could not be sent.");
            }
        }
    }
}
