using System.Text;
using System.Text.Json;

namespace InsureYouAINew.Services
{
    public class AIService
    {

        private readonly string _apiKey = "";
        private readonly string _model = "gemini-2.5-flash";

        public async Task<string> PredictCategoryAsync(string messageText)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";

            using var http = new HttpClient();

            var requestBody = new
            {
                contents = new[]
                {
                new
                {
                    parts = new[]
                    {
                        new { text = $"Aşağıdaki kullanıcı mesajını sigortacılık alanında kategorize et. Sadece kategori adı döndür.\n\nMesaj: {messageText}\n\nOlası kategoriler:\n- Kasko\n- Trafik Sigortası\n- Sağlık Sigortası\n- Konut Sigortası\n- Hasar Bildirimi\n- Fiyat Teklifi\n- Poliçe Yenileme\n- Genel Soru\n- İletişim Talebi\n" }
                    }
                }
            }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);
            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text").GetString();

            return text.Trim();
        }

        public async Task<string> PredictPriorityAsync(string messageText)
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{_model}:generateContent?key={_apiKey}";

            using var http = new HttpClient();

            var requestBody = new
            {
                contents = new[]
                {
            new
            {
                parts = new[]
                {
                    new { text = $@"
Aşağıdaki kullanıcı mesajının aciliyet seviyesini belirle.
Sadece 3 seçenekten birini döndür: High, Medium, Low.

Kurallar:
- Kaza, hasar, ödeme sorunları, acil durumlar → High
- Fiyat teklifi, yenileme, teminat soruları → Medium
- Genel sorular, merak edilen bilgiler → Low

Mesaj:
{messageText}
" }
                }
            }
        }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await http.PostAsync(url, content);
            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);
            var text = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text").GetString();

            return text.Trim();
        }

    }
}
