using InsureYouAI.Context;
using InsureYouAI.Entities;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InsureYouAI.Controllers
{
    public class DefaultController : Controller
    {
        private readonly InsureContext _context;

        public DefaultController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult SendMessage()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(Message message)
        {
            message.SendDate= DateTime.Now;
            message.IsRead= false;
            _context.Messages.Add(message);
            _context.SaveChanges();

            #region Claude_AI_Analiz

            string apiKey = "";
            //kullanıcının attıgı mesaja verilen cevap
            string prompt = $"Sen bir sigorta firmasının müşteri iletişim asistanısın.\r\n\r\nKurumsal ama samimi, net ve anlaşılır bir dille yaz.\r\n\r\nYanıtlarını 2–3 paragrafla sınırla.\r\n\r\nEksik bilgi (poliçe numarası, kimlik vb.) varsa kibarca talep et.\r\n\r\nFiyat, ödeme, teminat gibi kritik konularda kesin rakam verme, müşteri temsilcisine yönlendir.\r\n\r\nHasar ve sağlık gibi hassas durumlarda empati kur.\r\n\r\nCevaplarını teşekkür ve iyi dilekle bitir.\r\n\r\n kullanıcının sana gönderdiği mesaj şu şekilde:'{message.MessageDetail}.'";

                using var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.anthropic.com/");
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var requestBody = new
            {
                model = "claude-sonnet-4-20250514",
                max_tokens =1000,
                temperature = 0.5,
                messages = new[]
                {
                    new 
                    { 
                        role = "user", 
                        content = prompt 

                    }
                }            
            };
            var jsonContent= new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("v1/messages", jsonContent);
            var responseString = await response.Content.ReadAsStringAsync();

          
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Claude API Hatası: {response.StatusCode}");
                Console.WriteLine($"Yanıt: {responseString}");
            }
            var json=JsonNode.Parse(responseString);
            string? textContent = json?["content"]?[0]?["text"]?.ToString();

            //ViewBag.v= textContent;

            #endregion

            #region Email_Gönderme 

            MimeMessage mimeMessage = new MimeMessage();
            MailboxAddress mailboxAddress = new MailboxAddress("InsureYouAI Admin", "munevverorman1@gmail.com");
            mimeMessage.From.Add(mailboxAddress);

            MailboxAddress mailboxAddressTo = new MailboxAddress("User", message.Email);
            mimeMessage.To.Add(mailboxAddressTo);

            var bodyBuilder = new BodyBuilder();
            string body = !string.IsNullOrEmpty(textContent)
    ? textContent
    : "Talebiniz alınmıştır. En kısa sürede size dönüş yapılacaktır.";

            bodyBuilder.TextBody = body;
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            mimeMessage.Subject = "InsureYouAI Email Yanıtı";

            SmtpClient client2 = new SmtpClient();
            client2.Connect("smtp.gmail.com", 587, false);
            client2.Authenticate("munevverorman1@gmail.com", "");
            client2.Send(mimeMessage);
            client2.Disconnect(true);
            #endregion

            #region ClaudeAIMessage_DBKayıt
            ClaudeAIMessage claudeAIMessage = new ClaudeAIMessage
            {
              MessageDetail=textContent,
                ReceiveEmail= message.Email,
                ReceiveNameSurname= message.NameSurname,
                SendDate= DateTime.Now
            };
            _context.ClaudeAIMessages.Add(claudeAIMessage);
            _context.SaveChanges();
            #endregion

            return RedirectToAction("Index");
        }

        public PartialViewResult SubscribeEmail()
        {
            return PartialView();
        }
    }
}
