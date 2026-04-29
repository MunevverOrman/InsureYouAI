using Microsoft.AspNetCore.Mvc;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace InsureYouAINew.Controllers
{
    public class ElevenLabsAIController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ElevenLabsAIController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult SpeakInsuranceAnswer()
        {
            ViewBag.ControllerName = "Eleven Labs";
            ViewBag.PageName = "Sesli Yapay Zeka Asistanı";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SpeakInsuranceAnswer(string text)
        {
            ViewBag.ControllerName = "Eleven Labs";
            ViewBag.PageName = "Sesli Yapay Zeka Asistanı";

            if (string.IsNullOrWhiteSpace(text))
            {
                ViewBag.Error = "Lütfen metin gir.";
                return View();
            }

            var apiKey = "";
            var voiceId = "EXAVITQu4vr4xnSDxMaL";  //elevenlabs'teki Rachel sesi 

            var url = $"https://api.elevenlabs.io/v1/text-to-speech/{voiceId}/stream";

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("xi-api-key", apiKey);

            var requestBody = new
            {
                text = text,
                model_id = "eleven_multilingual_v2",
                voice_settings = new //sesin doğallığını ve benzerliğini artırmak için ayarlar
                {
                    stability = 0.5, //sesin tutarlılığını artırır, 0.5 iyi bir denge sağlar
                    similarity_boost = 0.8 //sesin orijinalliğini artırır, 0.8 iyi bir denge sağlar
                }
            };

            var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Ses oluşturulamadı. Model veya voice ID hatalı olabilir.";
                return View();
            }

            var audioBytes = await response.Content.ReadAsByteArrayAsync();

            var fileName = $"eleven_{Guid.NewGuid()}.mp3";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "voices", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            await System.IO.File.WriteAllBytesAsync(filePath, audioBytes);

            ViewBag.AudioUrl = "/voices/" + fileName;
            return View();
        }
       

public IActionResult SpeakInsuranceAnswer2()
{
    ViewBag.ControllerName = "Eleven Labs";
    ViewBag.PageName = "Sesli Yapay Zeka Asistanı";
    return View();
}

[HttpPost]
public async Task<IActionResult> SpeakInsuranceAnswer2(string text)
{

    ViewBag.ControllerName = "Eleven Labs";
    ViewBag.PageName = "Sesli Yapay Zeka Asistanı";

    if (string.IsNullOrWhiteSpace(text))
    {
        ViewBag.Error = "Lütfen metin gir.";
        return View();
    }

    var apiKey = "";
    var voiceId = "EXAVITQu4vr4xnSDxMaL";

    var url = $"https://api.elevenlabs.io/v1/text-to-speech/{voiceId}/stream";

    var client = _httpClientFactory.CreateClient();
    client.DefaultRequestHeaders.Add("xi-api-key", apiKey);

    var requestBody = new
    {
        text = text,
        model_id = "eleven_multilingual_v2",
        voice_settings = new
        {
            stability = 0.5,
            similarity_boost = 0.8
        }
    };

    var json = System.Text.Json.JsonSerializer.Serialize(requestBody);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await client.PostAsync(url, content);

    if (!response.IsSuccessStatusCode)
    {
        ViewBag.Error = "Ses oluşturulamadı. Model veya voice ID hatalı olabilir.";
        return View();
    }

    var audioBytes = await response.Content.ReadAsByteArrayAsync();

    var fileName = $"eleven_{Guid.NewGuid()}.mp3";
    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "voices", fileName);

    Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
    await System.IO.File.WriteAllBytesAsync(filePath, audioBytes);

    ViewBag.AudioUrl = "/voices/" + fileName;
    return View();
}

public IActionResult SpeakInsuranceAnswer3()
{
    ViewBag.ControllerName = "Eleven Labs";
    ViewBag.PageName = "Sesli Yapay Zeka Asistanı";
    return View();
}

[HttpPost]
public async Task<IActionResult> SpeakInsuranceAnswer3(string text)
{

    ViewBag.ControllerName = "Yapay Zeka Sigorta Asistanı";
    ViewBag.PageName = "Sesli Yanıt & Akıllı Sohbet Alanı";

    if (string.IsNullOrWhiteSpace(text))
    {
        ViewBag.Answer = "Lütfen bir metin girin.";
        return View();
    }

    // 1) AI METİN CEVABI (şimdilik direkt text'i döndürüyoruz)
    // Gerçek senaryoda burada OpenAI / Gemini / Claude çağrın olacak.
    string aiTextResponse = $"InsureYOU AI yanıtı: {text}";

    ViewBag.Answer = aiTextResponse;


    // 2) ELEVENLABS AYARLARI
    string apiKey = "";
    string voiceId = "EXAVITQu4vr4xnSDxMaL"; // Rachel (güzel bir ses)
    string url = $"https://api.elevenlabs.io/v1/text-to-speech/{voiceId}/stream";

    var client = _httpClientFactory.CreateClient();
    client.DefaultRequestHeaders.Add("xi-api-key", apiKey);

    var payload = new
    {
        text = aiTextResponse, // konuşsun diye cevabı okuyor
        model_id = "eleven_multilingual_v2"
    };

    var json = System.Text.Json.JsonSerializer.Serialize(payload);
    var content = new StringContent(json, Encoding.UTF8, "application/json");


    // 3) ELEVENLABS'TEN SES OLUŞTURMA
    var response = await client.PostAsync(url, content);

    if (!response.IsSuccessStatusCode)
    {
        ViewBag.Answer = "Ses oluşturulamadı.";
        ViewBag.AudioUrl = null;
        return View();
    }

    var audioBytes = await response.Content.ReadAsByteArrayAsync();


    // 4) MP3 DOSYASINI KAYDETME
    var fileName = $"voice_{Guid.NewGuid()}.mp3";
    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/voices", fileName);

    Directory.CreateDirectory("wwwroot/voices");

    await System.IO.File.WriteAllBytesAsync(path, audioBytes);


    // 5) UI'YE VERİLERİ GÖNDER
    ViewBag.AudioUrl = "/voices/" + fileName;
    ViewBag.Answer = aiTextResponse;

    return View();
}
    }
}
