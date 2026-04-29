using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace InsureYouAINew.Controllers
{
    public class TavilyController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TavilyController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private readonly string tavilyApiKey = "";
        private readonly string openAIApiKey = "";

        [HttpGet]
        public IActionResult Search()
        {
            ViewBag.ControllerName = "Tavily AI";
            ViewBag.PageName = "Web Tarama Özetleme Yapay Zeka Asistanı";
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SearchWithTavily(string query)
        {
            ViewBag.ControllerName = "Tavily AI";
            ViewBag.PageName = "Web Tarama Özetleme Yapay Zeka Asistanı";

            if (string.IsNullOrEmpty(query))
            {
                ViewBag.Error = "Lütfen bir arama sorgusu girin.";
                return View("Index");
            }

            // 1) Tavily web araması
            var tavilyResponse = await CallTavilyAsync(query);

            // 2) OpenAI ile sonuçların analiz edilmesi
            var openAIResponse = await SummarizeWithOpenAI(query, tavilyResponse);

            ViewBag.Query = query;
            ViewBag.TavilyRaw = tavilyResponse;
            ViewBag.OpenAIResult = openAIResponse;

            return View("Search");
        }

        private async Task<string> CallTavilyAsync(string query)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.tavily.com/");

            var requestBody = new
            {
                api_key = tavilyApiKey, // Tavily API anahtarı
                query = query, // Tavily'nin webde ne arayacağını belirten sorgu 
                include_answer = true, // Arama sonuçlarında sadece link değil, aynı zamanda kısa bir özet de almak için
                max_results = 5 // Arama sonuçlarının sayısını sınırlamak için
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("search", content);

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<string> SummarizeWithOpenAI(string query, string tavilyJson)
        {
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.openai.com/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", openAIApiKey);

            var prompt = $@"
Kullanıcının sorusu: {query}

Aşağıdaki Tavily web araması sonuçlarını oku ve kullanıcıya kısa, net ve akademik bir açıklama yap.
Önemli noktaları sade şekilde özetle. Gereksiz cümle kurma.

Tavily sonuçları:
{tavilyJson}
";

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new {
                        role = "user",
                        content = prompt
                    }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("v1/chat/completions", content);
            var responseString = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(responseString);
            return result?.choices?[0]?.message?.content ?? "OpenAI yanıt üretemedi.";
        }

    }
}
