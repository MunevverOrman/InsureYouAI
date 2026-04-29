using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

namespace InsureYouAI.Controllers
{
    public class AboutItemController : Controller
    {
        private readonly InsureContext _context;

        public AboutItemController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult AboutItemList()
        {
            ViewBag.ControllerName = "Hakkımızda Öğeleri";
            ViewBag.PageName = "Mevcut Hakkımızda Öğeleri";
            var values = _context.AboutItems.ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateAboutItem()
        {
            ViewBag.ControllerName = "Hakkımızda Öğeleri";
            ViewBag.PageName = "Yeni Hakkımızda Öğe Girişi";
            return View();
        }
        [HttpPost]
        public IActionResult CreateAboutItem(AboutItem aboutItem)
        {
            _context.AboutItems.Add(aboutItem);
            _context.SaveChanges();
            return RedirectToAction("AboutItemList");
        }

        [HttpGet]
        public IActionResult UpdateAboutItem(int id)
        {
            ViewBag.ControllerName = "Hakkımızda Öğeleri";
            ViewBag.PageName = " Hakkımızda Öğeleri Güncelleme ";
            var values = _context.AboutItems.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult UpdateAboutItem(AboutItem aboutItem)
        {
            _context.AboutItems.Update(aboutItem);
            _context.SaveChanges();
            return RedirectToAction("AboutItemList");
        }

        public IActionResult DeleteAboutItem(int id)
        {
            var values = _context.AboutItems.Find(id);
            _context.AboutItems.Remove(values);
            _context.SaveChanges();
            return RedirectToAction("AboutItemList");
        }
        [HttpGet]
        public async Task<IActionResult> CreateAboutWithGoogleGemini()
        {
            // Google AI Studio'dan alınan API anahtarı
            var apiKey = "";
            
          
            var model = "gemini-2.5-flash";

            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";

          
            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new
                            {
                               
                                text = "Kurumsal bir sigorta firması için etkileyici, güven verici ve profesyonel bir 'Hakkımızda alanları(about item)' yazısı oluştur.Risklerinizi analiz ediyor, size en uygun poliçeyi hızla sunuyoruz. şeklinde veya bunun gibi ve buna benzer daha zengin içerikler gelsin en az 10 tane item bekliyorum"
                            }
                        }
                    }
                }
            };

          
            using var httpClient = new HttpClient();

     
            httpClient.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);

           
            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

   
            var response = await httpClient.PostAsync(url, content);

       
            var responseJson = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.value = $"API Hatası: {response.StatusCode} - {responseJson}";
                return View();
            }

            using var jsonDoc = JsonDocument.Parse(responseJson);
            var root = jsonDoc.RootElement;

            // "candidates" alanı yoksa beklenmeyen yanıt hatası gösteriliyor
            if (!root.TryGetProperty("candidates", out var candidates))
            {
                ViewBag.value = $"Beklenmeyen yanıt: {responseJson}";
                return View();
            }

            // Gemini'nin döndürdüğü JSON yapısından metin içeriği çekiliyor
            // Yapı: candidates[0] -> content -> parts[0] -> text
            var aboutText = candidates[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            // Oluşturulan metin View'a gönderiliyor
            ViewBag.value = aboutText;
            return View();
        }
    }
}
