using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace InsureYouAI.Controllers
{
    public class AboutController : Controller
    {
      
        private readonly InsureContext _context;

       
        public AboutController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult AboutList()
        {
            ViewBag.ControllerName = "Hakkımızda";
            ViewBag.PageName = "Mevcut Hakkımızda Yazısı";
            var values = _context.Abouts.ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateAbout()
        {
            ViewBag.ControllerName = "Hakkımızda";
            ViewBag.PageName = "Yeni Hakkımızda Yazısı Girişi";
            return View();
        }

       
        [HttpPost]
        public IActionResult CreateAbout(About about)
        {
            _context.Abouts.Add(about);
            _context.SaveChanges();
            return RedirectToAction("AboutList");
        }

        [HttpGet]
        public IActionResult UpdateAbout(int id)
        {
            ViewBag.ControllerName = "Hakkımızda";
            ViewBag.PageName = " Hakkımızda Yazı Düzenlenmesi";
            var values = _context.Abouts.Find(id);
            return View(values);
        }

        [HttpPost]
        public IActionResult UpdateAbout(About About)
        {
            _context.Abouts.Update(About);
            _context.SaveChanges();
            return RedirectToAction("AboutList");
        }

        public IActionResult DeleteAbout(int id)
        {
            var value = _context.Abouts.Find(id);
            _context.Abouts.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("AboutList");
        }

        // Google Gemini API kullanarak otomatik 'Hakkımızda' metni oluşturur 
        [HttpGet]
        public async Task<IActionResult> CreateAboutWithGoogleGemini()
        {
     
            var apiKey = "";

         
            var model = "gemini-2.5-flash";

            // API endpoint URL'i - model adı dinamik olarak ekleniyor
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";

            // API'ye gönderilecek istek gövdesi oluşturuluyor
            // "contents" ve "parts" Gemini API'nin beklediği JSON yapısı
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
                                // Gemini'ye gönderilen Türkçe prompt
                                text = "Kurumsal bir sigorta firması için etkileyici, güven verici ve profesyonel bir 'Hakkımızda' yazısı oluştur."
                            }
                        }
                    }
                }
            };

            // HttpClient oluşturuluyor (using ile bellek yönetimi sağlanıyor)
            using var httpClient = new HttpClient();

            // API anahtarı URL parametresi yerine güvenli şekilde header'a ekleniyor
            httpClient.DefaultRequestHeaders.Add("x-goog-api-key", apiKey);

            // requestBody nesnesi JSON'a dönüştürülüp UTF-8 formatında içerik oluşturuluyor
            var content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json"
            );

            // API'ye POST isteği gönderiliyor ve yanıt bekleniyor
            var response = await httpClient.PostAsync(url, content);

            // Yanıt string olarak okunuyor
            var responseJson = await response.Content.ReadAsStringAsync();

            // HTTP isteği başarısız olduysa (4xx, 5xx) hata mesajı View'a gönderiliyor
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.value = $"API Hatası: {response.StatusCode} - {responseJson}";
                return View();
            }

            // Gelen JSON yanıtı parse ediliyor
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