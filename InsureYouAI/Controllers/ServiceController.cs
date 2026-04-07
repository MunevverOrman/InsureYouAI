using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace InsureYouAI.Controllers
{
    public class ServiceController : Controller
    {
        private readonly InsureContext _context;

        public ServiceController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult ServiceList()
        {
            var values = _context.Services.ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateService()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateService(Service service)
        {
            _context.Services.Add(service);
            _context.SaveChanges();
            return RedirectToAction("ServiceList");
        }

        public IActionResult DeleteService(int id)
        {
            var value = _context.Services.Find(id);
            _context.Services.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("ServiceList");
        }

        [HttpGet]
        public IActionResult UpdateService(int id)
        {
            var value = _context.Services.Find(id);
            return View(value);
        }

        [HttpPost]
        public IActionResult UpdateService(Service service)
        {
            _context.Services.Update(service);
            _context.SaveChanges();
            return RedirectToAction("ServiceList");
        }

        public async Task<IActionResult> CreateServiceWithAnthropicClaude()
        {
            string apiKey = "anthropıcAIKey";

            // Yapay zekaya gönderilecek olan Türkçe talimat metni
            string prompt = "Bir sigorta şirketi için hizmetler bölümü hazırlamanı istiyorum. " +
                            "Burada 5 farklı hizmet olmalı. Bana maksimum 100 karakterden oluşan " +
                            "cümlelerle 5 tane hizmet içeriği yazar mısın?";

            // HTTP isteklerini yönetmek için bir istemci örneği oluşturuluyor
            using var client = new HttpClient();

            // API'nin temel URL adresi tanımlanıyor
            client.BaseAddress = new Uri("https://api.anthropic.com/");

            // API güvenliği ve versiyon kontrolü için gerekli olan Header (başlık) bilgileri ekleniyor
            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API'ye gönderilecek veri paketi (model, sınırlar ve mesajlar) anonim nesne olarak hazırlanıyor
            var requestBody = new
            {
                model = "claude-opus-4-5", // Kullanılacak yapay zeka modelinin adı
                max_tokens = 512,          // Yanıtın uzunluk sınırı
                temperature = 0.5,         // Yanıtın yaratıcılık seviyesi (0-1 arası)
                messages = new[]           // Konuşma geçmişi/mesaj listesi
                {
            new { role = "user", content = prompt }
        }
            };

            // Hazırlanan nesne, JSON formatına dönüştürülüp HTTP içeriği haline getiriliyor
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            // Anthropic uç noktasına (endpoint) POST isteği yapılıyor
            var response = await client.PostAsync("v1/messages", jsonContent);

            // Eğer API isteği başarısız olursa (400, 401, 500 vb.) hata mesajını ViewBag'e atar
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                ViewBag.services = new List<string>
        {
            $"Claude API Hatası: {response.StatusCode} - {errorBody}"
        };
                return View();
            }

            // İstek başarılıysa gelen yanıt string olarak okunuyor
            var responseString = await response.Content.ReadAsStringAsync();

            // JSON formatındaki metin, programlanabilir bir dökümana dönüştürülüyor
            using var doc = JsonDocument.Parse(responseString);

            // JSON yapısı içindeki "content" dizisinin ilk elemanındaki "text" değeri çekiliyor
            var fullText = doc.RootElement
                              .GetProperty("content")[0]
                              .GetProperty("text")
                              .GetString();

            // Gelen ham metin satırlara bölünüyor, boşluklar siliniyor ve başındaki numaralar temizleniyor
            var services = fullText.Split('\n')
                                   .Where(x => !string.IsNullOrEmpty(x))
                                   .Select(x => x.TrimStart('1', '2', '3', '4', '5', '.', ' '))
                                   .ToList();

            // Temizlenmiş liste View tarafında kullanılmak üzere ViewBag'e yükleniyor
            ViewBag.services = services;


            return View();
        }
    }
}
