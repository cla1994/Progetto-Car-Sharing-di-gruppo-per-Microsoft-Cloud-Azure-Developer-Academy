using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Academy2023.Net.Controllers
{
    public class SkebbyController : Controller
    {
        private  HttpClient SMSClient;

        public async Task<IActionResult> SMSAuth()
        {
            Soluzione1.skebby.Skebby Obj = new Soluzione1.skebby.Skebby();
            Obj.Username = "leonardo@as4u.it";
            Obj.Password = "RGRW0nmFwteIZ0xBK89XPSS2";

            SMSClient = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://api.skebby.it/API/v1.0/REST/" + "login?username=" + "leonardo@as4u.it" + "&password=" + "4c4d3mY!");
            HttpResponseMessage httpResponseMessage = await SMSClient.SendAsync(request);

            var Result = await Obj.SendSMSAsync("+393480716266", new string[] { "+393333251275"}, "Prova");
            var SMSHistoryList = await Obj.GetSMSHistoryAsync(new DateTime(2022, 12, 20), null, 1, 100);
            return Ok(SMSHistoryList);
        }

         public async Task<IActionResult> Login(string user = "leonardo@as4u.it", string password = "4c4d3mY!")
        {

            using (var wb = new WebClient())
            {
                // Setting the encoding is required when sending UTF8 characters!
                wb.Encoding = System.Text.Encoding.UTF8;

                try
                {
                    string encoded2 = System.Convert.ToBase64String(Encoding.GetEncoding("UTF-8")
                                                   .GetBytes($"{user}:{password}"));

                    wb.Headers.Add("Authorization", $"Basic { encoded2}");

                    String response = wb.DownloadString("https://api.skebby.it/API/v1.0/REST/login");

                    Console.WriteLine(response);

                    String[] auth = response.Split(';');
                    Console.WriteLine("user_key: " + auth[0]);
                    Console.WriteLine("Session_key: " + auth[1]);
                }
                catch (WebException ex)
                {
                    var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                    var errorResponse = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                    Console.WriteLine("Error!, http code: " + statusCode + ", body message: ");
                    Console.WriteLine(errorResponse);
                }
            }

            //TODO: Get from config file!
            string encoded = System.Convert.ToBase64String(Encoding.GetEncoding("UTF-8")
                                                   .GetBytes(string.Format("{0}:{1}", user, password)));
            //* Get Auth Keys
            var wbr = new HttpClient();

            string base_url = "https://api.skebby.it/API/v1.0/REST";

            var request = new HttpRequestMessage()

            {
                //RequestUri = new Uri("http://smspanel.aruba.it/API/v1.0/REST/login"),
                RequestUri = new Uri(string.Format("{0}/login", base_url)),
                Method = HttpMethod.Get,
            };
            request.Headers.Add("Authorization", string.Format("Basic {0}", encoded));
            var resp = wbr.Send(request);
            if (resp.StatusCode == HttpStatusCode.OK)
            {
                string response = resp.Content.ReadAsStringAsync().Result;
                String[] auth = response.Split(";");

                Console.WriteLine(auth[0]);
                Console.WriteLine(auth[1]);

            }
            return Ok(resp);
        }

                /*   public async Task<IActionResult> SendSMS()
                   {
                       using (var wb = new WebClient())
                       {
                           // Setting the encoding is required when sending UTF8 characters!
                           wb.Encoding = System.Text.Encoding.UTF8;

                           try
                           {
                               wb.Headers.Set(HttpRequestHeader.ContentType, "application/json");
                               wb.Headers.Add("user_key", "USER_KEY");
                               wb.Headers.Add("Session_key", "SESSION_KEY");

                               String payload = "{" +
                                 "    \"message_type\": \"MESSAGE_TYPE\", " +
                                 "    \"message\": \"Hello world!\", " +
                                 "    \"recipient\": [" +
                                 "        \"+393333251275\", " +
                                 "        \"+393420006263\"" +
                                 "    ], " +
                                 "    \"sender\": \"MySender\", " +
                                 "    \"scheduled_delivery_time\": \"20161223101010\", " +
                                 "    \"order_id\": \"123456789\", " +
                                 "    \"returnCredits\": true" +
                                 "}";

                               String response = wb.UploadString("https://api.skebby.it/API/v1.0/REST/sms", "POST", payload);

                               dynamic obj = JsonConvert.DeserializeObject(response);
                               Console.WriteLine(obj);
                               return Ok(obj);
                           }
                           catch (WebException ex)
                           {
                               var statusCode = ((HttpWebResponse)ex.Response).StatusCode;
                               var errorResponse = new StreamReader(ex.Response.GetResponseStream()).ReadToEnd();
                               Console.WriteLine("Error!, http code: " + statusCode + ", body message: ");
                               Console.WriteLine(errorResponse);
                               return Ok(errorResponse);
                           }
                       }
                   } */
            }
}
