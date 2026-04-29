using InsureYouAI.Context;
using InsureYouAI.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace InsureYouAI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly InsureContext _context;

        public ArticleController(InsureContext context)
        {
            _context = context;
        }

        public IActionResult ArticleList()
        {
            ViewBag.ControllerName = "Makaleler";
            ViewBag.PageName = "Makale Listesi";
            var values = _context.Articles.Include(x => x.AppUser).Include(y => y.Category).ToList();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateArticle()
        {
            ViewBag.ControllerName = "Makaleler";
            ViewBag.PageName = "Yeni Makale Oluştur";

            var categories = _context.Categories
                           .Select(x => new SelectListItem
                           {
                               Text = x.CategoryName,
                               Value = x.CategoryId.ToString()
                           })
                           .ToList();

            ViewBag.Categories = categories;

            var authors = _context.Users
                         .Select(x => new SelectListItem
                         {
                             Text = x.Name + " " + x.Surname,
                             Value = x.Id
                         })
                         .ToList();

            ViewBag.Authors = authors;

            return View();
        }

        [HttpPost]
        public IActionResult CreateArticle(Article article)
        {
            // Kayıt oluşturulurken bugünün tarihini atıyoruz
            article.CreatedDate = DateTime.Now;
            _context.Articles.Add(article);
            _context.SaveChanges();
            return RedirectToAction("ArticleList");
        }

        public IActionResult DeleteArticle(int id)
        {
            var values = _context.Articles.Find(id);
            _context.Articles.Remove(values);
            _context.SaveChanges();
            return RedirectToAction("ArticleList");
        }

        [HttpGet]
        public IActionResult UpdateArticle(int id)
        {
            ViewBag.ControllerName = "Makaleler";
            ViewBag.PageName = " Makale Güncelleme Sayfası";

            var categories = _context.Categories
                           .Select(x => new SelectListItem
                           {
                               Text = x.CategoryName,
                               Value = x.CategoryId.ToString()
                           })
                           .ToList();

            ViewBag.Categories = categories;

            var authors = _context.Users
                         .Select(x => new SelectListItem
                         {
                             Text = x.Name + " " + x.Surname,
                             Value = x.Id
                         })
                         .ToList();

            ViewBag.Authors = authors;
            var value = _context.Articles.Find(id);
            return View(value);
        }
        [HttpPost]
        public IActionResult UpdateArticle(Article article)
        {
            _context.Articles.Update(article);
            _context.SaveChanges();
            return RedirectToAction("ArticleList");
        }

        [HttpGet]
        public IActionResult CreateArticleWithOpenAI()
        {

            ViewBag.ControllerName = "Makaleler";
            ViewBag.PageName = "Yapay Zeka Makale Oluşturucu";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticleWithOpenAI(string prompt)
        {
            var apiKey = "";
            using var client= new HttpClient();
            client.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer", apiKey);
            var requestData = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new {role="system",content="Sen bir sigorta şirketi için çalışan ,içerik yazarlıı yapan bir yapay zekasın.Kullanıcının verdiği özet ve anahtar kelimelere göre ,sigortacılık sektörüyle ilgili makale üret. En az 3000 karakter olsun."},
                    new{role="user",content=prompt}
                },
                temperature=0.7
            };
            var response = await client.PostAsJsonAsync("https://api.openai.com/v1/chat/completions", requestData);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<OpenAIResponse>();
                var content = result.choices[0].message.content;
                ViewBag.article = content;
            }
            else
            {
                ViewBag.article = "Bir hata oluştu:" +response.StatusCode;
            }
            return View();
          
        }
        public class OpenAIResponse
        {
            public List<Choice> choices { get; set; }
        }
        public class Choice 
        {
            public Message message { get; set; }
        }
        public class Message 
        {
            public string role { get; set; }

            public string content { get; set; }
        }


     

    }
}
