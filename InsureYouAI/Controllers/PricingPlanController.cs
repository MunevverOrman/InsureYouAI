using InsureYouAI.Context;
using InsureYouAI.Entities;
using InsureYouAI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace InsureYouAI.Controllers
{
    public class PricingPlanController : Controller
    {
        private readonly InsureContext _context;

        public PricingPlanController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult PricingPlanList()
        {
            ViewBag.ControllerName = "AI Destekli Sigorta Planı";
            ViewBag.PageName = "Mevcut Sigorta Plan Listeleri";
            var values = _context.PricingPlans.ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreatePricingPlan()
        {
            ViewBag.ControllerName = "AI Destekli Sigorta Planı";
            ViewBag.PageName = "Yeni Sigorta Planı Oluşturma";
            return View();
        }

        [HttpPost]
        public IActionResult CreatePricingPlan(PricingPlan pricingPlan)
        {
            _context.PricingPlans.Add(pricingPlan);
            _context.SaveChanges();
            return RedirectToAction("PricingPlanList");
        }

        [HttpGet]
        public IActionResult UpdatePricingPlan(int id)
        {
            ViewBag.ControllerName = "AI Destekli Sigorta Planı";
            ViewBag.PageName = "Sigorta Plan Revizyonu";
            var values = _context.PricingPlans.Find(id);
            return View(values);
        }

        [HttpPost]
        public IActionResult UpdatePricingPlan(PricingPlan pricingPlan)
        {
            _context.PricingPlans.Update(pricingPlan);
            _context.SaveChanges();
            return RedirectToAction("PricingPlanList");
        }

        public IActionResult DeletePricingPlan(int id)
        {
            var values= _context.PricingPlans.Find(id);
            _context.PricingPlans.Remove(values);
            _context.SaveChanges();
            return RedirectToAction("PricingPlanList");

        }

        public IActionResult ChangeStatus(int id)
        {
            var values = _context.PricingPlans.Find(id);
            if (values.IsFeature == true)
            {
                values.IsFeature = false;
            }
            else
            {
                values.IsFeature = true;
            }
      
            _context.SaveChanges();
            return RedirectToAction("PricingPlanList");
        }


        [HttpGet]
        public IActionResult CreateUserCustomizePlan()
        {
            ViewBag.ControllerName="AI Destekli Sigorta Paketi Oluştur";
            ViewBag.PageName="Kullanıcıdan alınan bilgilere göre AI destekli sigorta paketi önerisi oluşturma sayfası";
            var model = new AIInsuranceRecommendationViewModel();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUserCustomizePlan(AIInsuranceRecommendationViewModel model)
        {
            ViewBag.ControllerName = "AI Destekli Sigorta Paketi Oluştur";
            ViewBag.PageName = "Kullanıcıdan alınan bilgilere göre AI destekli sigorta paketi önerisi oluşturma sayfası";
            string apiKey = "";
            // Kullanıcı girdilerini JSON'a çeviriyoruz
            var userJson = JsonConvert.SerializeObject(model);

            // OpenAI'ye göndereceğimiz prompt:
            var prompt = $@"
Sen profesyonel bir sigorta uzmanı AI asistanısın. 
Aşağıdaki kullanıcının bilgilerini analiz ederek en uygun sigorta paketini öner.

Paketler ve özellikleri:
1) Premium Paket (1999 TL/ay): Yatarak tedavi, check-up, geniş yol yardım, yurtiçi seyahat güvencesi.
2) Standart Paket (1499 TL/ay): Acil sağlık, müşteri hizmetleri, kaza sonrası tıbbi destek.
3) Ekonomik Paket (1399 TL/ay): Temel sağlık, temel yol yardım.

Kullanıcı bilgileri:
{userJson}

Sadece şu formatta JSON döndür:

{{
  ""onerilenPaket"": ""Premium | Standart | Ekonomik"",
  ""ikinciSecenek"": ""Premium | Standart | Ekonomik"",
  ""neden"": ""Kısa analiz metni""
}}
";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);

            var body = new
            {
                model = "gpt-4.1-mini",
                messages = new[]
                {
                    new { role = "user", content = prompt }
                }
            };

            var jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            dynamic ai = JsonConvert.DeserializeObject(jsonResponse);
            string aiResult = ai.choices[0].message.content;

            // AI cevabı JSON formatında gelmiş olacak
            var result = JsonConvert.DeserializeObject<AIInsuranceRecommendationViewModel>(aiResult);

            // Sonuçları modele geri yazıyoruz
            model.RecommendedPackage = result.onerilenPaket;
            model.SecondBestPackage = result.ikinciSecenek;
            model.AnalysisText = result.neden;
            TempData["RawAI"]=aiResult; //  ham AI cevabını da göstermek için 

            return View(model);
        }


    }
}
