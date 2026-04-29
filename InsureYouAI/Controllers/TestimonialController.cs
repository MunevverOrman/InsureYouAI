using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace InsureYouAI.Controllers
{
    public class TestimonialController : Controller
    {
        private readonly InsureContext _context;

        public TestimonialController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult TestimonialList()
        {
            var values = _context.Testimonials.ToList();
            return View(values);
           
        }
        [HttpGet]
        public IActionResult CreateTestimonial()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateTestimonial(Testimonial testimonial)
        {
            _context.Testimonials.Add(testimonial);
            _context.SaveChanges();
            return RedirectToAction("TestimonialList");
        }
        public IActionResult DeleteTestimonial(int id)
        {
            var value = _context.Testimonials.Find(id);
            _context.Testimonials.Remove(value);
            _context.SaveChanges();
            return RedirectToAction("TestimonialList");
        }
        [HttpGet]
        public IActionResult UpdateTestimonial(int id)
        {
            var value = _context.Testimonials.Find(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult UpdateTestimonial(Testimonial testimonial)
        {
            _context.Testimonials.Update(testimonial);
            _context.SaveChanges();
            return RedirectToAction("TestimonialList");
        }

        // Anthropic Claude API'sini kullanarak sigorta şirketi için müşteri yorumları (testimonial) oluşturan action metodu
      
        public async Task<IActionResult> CreateTestimonialWithClaudeAI()
        {
            
            string apiKey = "";

                 
            string prompt = "Bir sigorta şirketi için müşteri deneyimlerine dair yorum oluşturmak istiyorum " +
                            "yani ingilizce karşılığı ile:Testimonial. Bu alanda Türkçe olarak 6 tane yorum, " +
                            "6 tane müşteri adı ve soyadı, bu müşterilerin unvanı olsun. Buna göre içeriği hazırla.";

            
            using var client = new HttpClient();

            client.BaseAddress = new Uri("https://api.anthropic.com/");

            client.DefaultRequestHeaders.Add("x-api-key", apiKey);
         
            client.DefaultRequestHeaders.Add("anthropic-version", "2023-06-01");

            // Sunucuya "JSON formatında veri kabul ediyorum" diyoruz
            // API'nin cevabını JSON olarak almak için gerekli
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // API'ye gönderilecek istek gövdesi (request body) oluşturuluyor
            var requestBody = new
            {
                model = "claude-opus-4-5",

                max_tokens = 512,

               // Yaratıcılık seviyesi: 0 (deterministik) ile 1 (çok yaratıcı) arası
                // 0.5: Tutarlı ama biraz çeşitli cevaplar üretir; yorumlar için ideal denge
                temperature = 0.5,

                // Konuşma geçmişi: Claude'a "user" rolüyle mesaj gönderiyoruz
                // Dizi olarak tanımlandı çünkü çok turlu konuşmalarda birden fazla mesaj eklenebilir
                messages = new[]
                {
            new { role = "user", content = prompt }
        }
            };

            // Request body'yi JSON string'e çevirip HTTP içeriği haline getiriyoruz
            // UTF8 encoding: Türkçe karakterlerin (ş, ğ, ü, ö vb.) bozulmaması için şart
            // "application/json": Sunucuya "JSON gönderiyorum" diyoruz — bu olmadan BadRequest alınır
            var jsonContent = new StringContent(
                JsonSerializer.Serialize(requestBody),
                System.Text.Encoding.UTF8,
                "application/json"
            );

            // POST isteği gönderiliyor ve cevap bekleniyor (await ile asenkron bekleme)
            // "v1/messages": BaseAddress'e eklenen endpoint — tam URL: https://api.anthropic.com/v1/messages
            var response = await client.PostAsync("v1/messages", jsonContent);

            // API'den başarısız yanıt geldiyse (HTTP 200 dışı bir kod)
            // IsSuccessStatusCode: 200-299 arası kodlar için true döner
            if (!response.IsSuccessStatusCode)
            {
                // Hata detayını da okuyoruz ki kullanıcıya/geliştiriciye anlamlı bilgi verelim               
                var errorBody = await response.Content.ReadAsStringAsync();

                // View'a hata mesajını liste olarak gönderiyoruz
                // Liste formatı: View tarafında foreach ile kolayca gösterilebilir
                ViewBag.testimonials = new List<string>
        {
            $"Claude API Hatası: {response.StatusCode} - {errorBody}"
        };
                return View(); // Hata mesajıyla birlikte View'ı döndür, işlemi sonlandır
            }

            // Başarılı cevabın içeriğini string olarak okuyoruz
            var responseString = await response.Content.ReadAsStringAsync();

            // JSON string'i parse ediyoruz — JsonDocument bellekte verimli çalışır
            // "using" ile dispose ediliyor: büyük JSON'larda bellek sızıntısını önler
            using var doc = JsonDocument.Parse(responseString);

            // Anthropic API'nin döndürdüğü JSON yapısı:
            // { "content": [ { "type": "text", "text": "Claude'un cevabı..." } ] }
            // content[0].text: İlk (ve genellikle tek) mesaj bloğundaki metni alıyoruz
            var fullText = doc.RootElement
                              .GetProperty("content")[0]
                              .GetProperty("text")
                              .GetString();

            // Claude'un cevabı tek bir metin bloğu olarak geliyor
            // Satır satır ayırıyoruz ('\n' ile) ve boş satırları temizliyoruz
            // TrimStart: Satır başındaki numara ve noktalama işaretlerini kaldırıyoruz
            // Örn: "1. Harika bir hizmet aldım." → "Harika bir hizmet aldım."
            var testimonials = fullText.Split('\n')
                                       .Where(x => !string.IsNullOrEmpty(x))
                                       .Select(x => x.TrimStart('1', '2', '3', '4', '5', '6', '.', ' '))
                                       .ToList();

            // İşlenmiş testimonial listesini View'a gönderiyoruz
            // ViewBag: Controller'dan View'a dinamik veri taşımak için kullanılan yapı
            ViewBag.testimonials = testimonials;

      
            return View();
        }
    }


    }

